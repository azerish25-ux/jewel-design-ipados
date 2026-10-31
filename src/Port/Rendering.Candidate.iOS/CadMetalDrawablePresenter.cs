using System.Numerics;
using CoreAnimation;
using Metal;
using JewelDesign.Rendering.Candidate;
namespace JewelDesign.Rendering.Candidate.iOS;

public readonly record struct CadMetalFrameInfo(long FrameId,long Revision,long MeshUploadId,int Width,int Height,double? PresentedTime,bool SupportsNativePresentationTiming,Matrix4x4 ViewProjection);
public readonly record struct CadMetalViewStats(long MeshUploads,long SubmittedFrames,long CompletedFrames,long PresentedFrames,
    int InFlightFrames,int PeakInFlightFrames,int LiveMeshResources,int LiveMeshBuffers,bool IsDisposed,string? LastError);

/// <summary>Owns retained CAD uploads and asynchronous frame resources for one visible view.</summary>
internal sealed class CadMetalDrawablePresenter : IDisposable
{
    private readonly object sync=new();
    private readonly CadMetalRenderer renderer=new();
    private readonly CadFrameGate<CadMetalGpuMesh> frames=new();
    private long uploads,submitted,completed,presented,nextFrame;
    private int buffers,peak;
    private bool disposed,rendererDisposed;
    private Exception? lastError;
    internal Action<CadMetalFrameInfo>? Presented;
    internal Action<CadMetalFrameInfo>? GpuCompleted;
    internal Action<Exception>? Failed;
    internal Action? Completed;
    internal bool CanSubmit{get{lock(sync){var state=frames.State;return !disposed&&state.NeedsFrame&&state.InFlight<CadFrameGate<CadMetalGpuMesh>.MaximumFramesInFlight;}}}
    internal CadMetalViewStats Stats{get{lock(sync){var state=frames.State;return new(uploads,submitted,completed,presented,state.InFlight,peak,state.LiveResources,buffers,disposed,lastError?.ToString());}}}

    internal long SetMesh(CadViewportMesh mesh)
    {
        ArgumentNullException.ThrowIfNull(mesh);
        lock(sync)
        {
            ObjectDisposedException.ThrowIf(disposed,this);
            IMTLBuffer? vertices=null,indices=null;CadMetalGpuMesh? resource=null;bool transferred=false;
            try
            {
                if(mesh.TriangleCount>0)
                {
                    vertices=renderer.Device.CreateBuffer(mesh.CopyInterleavedVertices(),MTLResourceOptions.StorageModeShared)??throw new InvalidOperationException("Visible vertex upload failed.");
                    indices=renderer.Device.CreateBuffer(mesh.CopyIndices(),MTLResourceOptions.StorageModeShared)??throw new InvalidOperationException("Visible index upload failed.");
                }
                resource=new CadMetalGpuMesh(checked(uploads+1),vertices,indices,mesh.TriangleCount,MeshReleased);
                buffers+=resource.BufferCount;
                frames.ReplaceResource(resource);transferred=true;uploads=resource.UploadId;
                return uploads;
            }
            catch{if(resource is not null){if(!transferred)resource.Dispose();}else{var cleanupError=DisposeAll(vertices,indices);lastError??=cleanupError;}throw;}
        }
    }
    private void MeshReleased(int count,Exception? error){lock(sync){buffers-=count;if(error is not null)lastError??=error;}}
    internal void Invalidate(){lock(sync){ObjectDisposedException.ThrowIf(disposed,this);frames.Invalidate();}}

    // A successful submission takes ownership of drawable. A false/throwing submission leaves it with the caller.
    internal bool TrySubmit(MTLRenderPassDescriptor pass,ICAMetalDrawable drawable,Matrix4x4 matrix,int width,int height)
    {
        lock(sync)
        {
            ObjectDisposedException.ThrowIf(disposed,this);
            var lease=frames.TryBeginFrame();if(lease is null)return false;
            IMTLBuffer? camera=null;IMTLCommandBuffer? command=null;Submission? frame=null;
            try
            {
                var mesh=lease.Resource;
                camera=renderer.Device.CreateBuffer(CadViewportCamera.MatrixScalars(matrix),MTLResourceOptions.StorageModeShared)??throw new InvalidOperationException("Visible camera upload failed.");
                command=renderer.CreateCommandBuffer();
                if(!command.RetainedReferences)throw new InvalidOperationException("Visible commands must retain presentation textures.");
                using(var encoder=command.CreateRenderCommandEncoder(pass)??throw new InvalidOperationException("Visible render encoder unavailable."))
                {renderer.Encode(encoder,mesh.Vertices,mesh.Indices,camera,mesh.TriangleCount);encoder.EndEncoding();}
                var info=new CadMetalFrameInfo(checked(++nextFrame),lease.Revision,mesh.UploadId,width,height,null,CadMetalTimingCapabilities.SupportsNativeTiming(drawable),matrix);
                frame=new Submission(lease,camera,command,drawable,info);
                var owned=frame;
                if(info.SupportsNativePresentationTiming)drawable.AddPresentedHandler(value=>OnPresented(owned.Info with{PresentedTime=value.PresentedTime}));
                command.AddCompletedHandler(value=>OnCompleted(owned,value));
                command.PresentDrawable(drawable);command.Commit();submitted++;
                peak=Math.Max(peak,frames.State.InFlight);
                return true;
            }
            catch
            {
                // Nothing is published until commit; camera/mesh ownership is always balanced on failure.
                var cleanupError=frame is null?DisposeAll(camera,command,lease):frame.Release(includeDrawable:false);
                lastError??=cleanupError;
                frames.Invalidate();throw;
            }
        }
    }
    private void OnPresented(CadMetalFrameInfo info)
    {
        Action<CadMetalFrameInfo>? notification;
        lock(sync){presented++;notification=disposed?null:Presented;}
        notification?.Invoke(info);
    }
    private void OnCompleted(Submission frame,IMTLCommandBuffer command)
    {
        Exception? error=null;Action<Exception>? failure;Action? notification;Action<CadMetalFrameInfo>? gpuNotification;
        lock(sync)
        {
            if(command.Status!=MTLCommandBufferStatus.Completed)error=new InvalidOperationException("Visible GPU command failed: "+command.Status+" "+command.Error);
            var cleanupError=frame.Release(includeDrawable:true);error??=cleanupError;completed++;
            error??=lastError;
            if(error is not null)lastError??=error;
            if(disposed&&frames.State.InFlight==0)ReleaseRenderer();
            failure=disposed?null:Failed;notification=disposed?null:Completed;gpuNotification=disposed?null:GpuCompleted;
        }
        if(error is not null)failure?.Invoke(error);else gpuNotification?.Invoke(frame.Info);
        notification?.Invoke();
    }
    private static Exception? DisposeAll(params IDisposable?[] values)
    {
        Exception? failure=null;foreach(var value in values)try{value?.Dispose();}catch(Exception error){failure??=error;}return failure;
    }
    private void ReleaseRenderer()
    {
        if(rendererDisposed)return;rendererDisposed=true;
        try{renderer.Dispose();}catch(Exception error){lastError??=error;}
    }
    public void Dispose()
    {
        lock(sync){if(disposed)return;disposed=true;frames.Dispose();if(frames.State.InFlight==0)ReleaseRenderer();}
    }
    private sealed class Submission(CadFrameLease<CadMetalGpuMesh> lease,IMTLBuffer camera,IMTLCommandBuffer command,ICAMetalDrawable drawable,CadMetalFrameInfo info)
    {
        private int released;
        internal readonly CadMetalFrameInfo Info=info;
        internal Exception? Release(bool includeDrawable)
        {
            if(Interlocked.Exchange(ref released,1)!=0)return null;
            Exception? failure=null;
            void Free(IDisposable value){try{value.Dispose();}catch(Exception error){failure??=error;}}
            Free(camera);Free(lease);Free(command);if(includeDrawable)Free(drawable);
            return failure;
        }
    }
}
