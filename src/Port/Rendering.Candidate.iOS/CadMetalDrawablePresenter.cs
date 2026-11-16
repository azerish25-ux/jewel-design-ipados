using System.Numerics;
using CoreAnimation;
using Metal;
using JewelDesign.Rendering.Candidate;
namespace JewelDesign.Rendering.Candidate.iOS;

public readonly record struct CadMetalFrameInfo(long FrameId,long Revision,long MeshUploadId,int Width,int Height,double? PresentedTime,bool SupportsNativePresentationTiming,Matrix4x4 ViewProjection){public long ImageUploadId{get;init;}public long TraceUploadId{get;init;}}
public readonly record struct CadMetalViewStats(long MeshUploads,long SubmittedFrames,long CompletedFrames,long PresentedFrames,
    int InFlightFrames,int PeakInFlightFrames,int LiveMeshResources,int LiveMeshBuffers,bool IsDisposed,string? LastError){public long ImageUploads{get;init;}public int LiveImageTextures{get;init;}public int LiveSceneResources{get;init;}public long SceneRevision{get;init;}}

/// <summary>Owns retained CAD uploads and asynchronous frame resources for one visible view.</summary>
internal sealed class CadMetalDrawablePresenter : IDisposable
{
    private readonly object sync=new();
    private readonly CadMetalRenderer renderer=new();
    private readonly CadFrameGate<CadMetalGpuScene> frames=new();
    private long uploads,submitted,completed,presented,nextFrame;
    private int buffers,peak,meshes,imageTextures;private long imageUploads;private CadMetalGpuScene? current;private MetalImageQuadPipeline? imagePipeline,tracePipeline;
    private bool disposed,rendererDisposed;
    private Exception? lastError;
    internal Action<CadMetalFrameInfo>? Presented;
    internal Action<CadMetalFrameInfo>? GpuCompleted;
    internal Action<Exception>? Failed;
    internal Action? Completed;
    internal bool CanSubmit{get{lock(sync){var state=frames.State;return !disposed&&state.NeedsFrame&&state.InFlight<CadFrameGate<CadMetalGpuScene>.MaximumFramesInFlight;}}}
    internal CadMetalViewStats Stats{get{lock(sync){var state=frames.State;return new(uploads,submitted,completed,presented,state.InFlight,peak,meshes,buffers,disposed,lastError?.ToString()){ImageUploads=imageUploads,LiveImageTextures=imageTextures,LiveSceneResources=state.LiveResources,SceneRevision=state.Revision};}}}

    internal long SetMesh(CadViewportMesh mesh)
    {
     ArgumentNullException.ThrowIfNull(mesh);lock(sync){ObjectDisposedException.ThrowIf(disposed,this);
      IMTLBuffer? vertices=null,indices=null;CadMetalGpuMesh? resource=null;CadMetalGpuWallpaper? wallpaper=null;CadMetalGpuTrace? trace=null;CadMetalGpuScene? scene=null;bool transferred=false;
      try{
       if(mesh.TriangleCount>0){vertices=renderer.Device.CreateBuffer(mesh.CopyInterleavedVertices(),MTLResourceOptions.StorageModeShared)??throw new InvalidOperationException("Visible vertex upload failed.");indices=renderer.Device.CreateBuffer(mesh.CopyIndices(),MTLResourceOptions.StorageModeShared)??throw new InvalidOperationException("Visible index upload failed.");}
       resource=new CadMetalGpuMesh(checked(uploads+1),vertices,indices,mesh.TriangleCount,MeshReleased,mesh.Center,mesh.Radius);buffers+=resource.BufferCount;meshes++;
       wallpaper=current?.Wallpaper?.Retain();trace=current?.Trace?.Retain();scene=new(resource,wallpaper,trace,current?.TraceInfo);frames.ReplaceResource(scene);transferred=true;current=scene;uploads=resource.UploadId;return uploads;
      }catch{if(!transferred){if(scene is not null)scene.Dispose();else{if(resource is not null)resource.Dispose();else lastError??=DisposeAll(vertices,indices);wallpaper?.Dispose();trace?.Dispose();}}throw;}
     }
    }
    // Input is an already-resized straight image. Opaque RGB follows the original24-bit wallpaper DIB; original file bytes remain external.
    internal long SetWallpaperPixels(DecodedLocalImage image,int storedScale)
    {
     ArgumentNullException.ThrowIfNull(image);lock(sync){ObjectDisposedException.ThrowIf(disposed,this);if(image.AlphaRepresentation!=RgbaAlphaRepresentation.Straight)throw new NotSupportedException("Wallpaper pixels require explicit straight RGB values.");
      _=Rhino5WallpaperPlacement.Gdi(1,1,image.Width,image.Height,storedScale);byte[] bytes=image.CopyRgba();for(int i=3;i<bytes.Length;i+=4)bytes[i]=255;
      imagePipeline??=new(renderer.Device,MTLPixelFormat.Depth32Float);
      using var description=MTLTextureDescriptor.CreateTexture2DDescriptor(MTLPixelFormat.RGBA8Unorm,(nuint)image.Width,(nuint)image.Height,false);description.StorageMode=MTLStorageMode.Shared;description.Usage=MTLTextureUsage.ShaderRead;
      IMTLTexture? texture=renderer.Device.CreateTexture(description)??throw new InvalidOperationException("Visible wallpaper texture allocation failed.");CadMetalGpuWallpaper? resource=null;CadMetalGpuMesh? mesh=null;CadMetalGpuTrace? trace=null;CadMetalGpuScene? scene=null;bool transferred=false;
      try{var pin=System.Runtime.InteropServices.GCHandle.Alloc(bytes,System.Runtime.InteropServices.GCHandleType.Pinned);try{texture.ReplaceRegion(MTLRegion.Create2D(0,0,(nuint)image.Width,(nuint)image.Height),0,pin.AddrOfPinnedObject(),(nuint)(image.Width*4));}finally{pin.Free();}
       resource=new(checked(imageUploads+1),texture,image.Width,image.Height,storedScale,ImageReleased);texture=null;imageTextures++;mesh=current?.Mesh?.Retain();trace=current?.Trace?.Retain();scene=new(mesh,resource,trace,current?.TraceInfo);frames.ReplaceResource(scene);transferred=true;current=scene;imageUploads=resource.UploadId;return imageUploads;
      }catch{if(!transferred){if(scene is not null)scene.Dispose();else{resource?.Dispose();mesh?.Dispose();trace?.Dispose();}}throw;}finally{texture?.Dispose();}
     }
    }
    internal void ClearWallpaper(){lock(sync){ObjectDisposedException.ThrowIf(disposed,this);var mesh=current?.Mesh?.Retain();var trace=current?.Trace?.Retain();var scene=new CadMetalGpuScene(mesh,null,trace,current?.TraceInfo);try{frames.ReplaceResource(scene);current=scene;}catch{scene.Dispose();throw;}}}
    internal long SetTracePixels(DecodedLocalImage image,Rhino5TraceImage info,int sourceDibDepth)
    {
     ArgumentNullException.ThrowIfNull(image);lock(sync){ObjectDisposedException.ThrowIf(disposed,this);if(image.AlphaRepresentation!=RgbaAlphaRepresentation.Straight)throw new NotSupportedException("Trace pixels require explicit straight RGBA.");
      _=Rhino5TracePlane.Create(info,image.Width,image.Height);byte[] bytes=Rhino5TraceRaster.Prepare(image.CopyRgba(),image.Width,image.Height,sourceDibDepth,info.GrayScale);tracePipeline??=new(renderer.Device,MTLPixelFormat.Depth32Float,true);
      using var description=MTLTextureDescriptor.CreateTexture2DDescriptor(MTLPixelFormat.RGBA8Unorm,(nuint)image.Width,(nuint)image.Height,false);description.StorageMode=MTLStorageMode.Shared;description.Usage=MTLTextureUsage.ShaderRead;
      IMTLTexture? texture=renderer.Device.CreateTexture(description)??throw new InvalidOperationException("Trace texture allocation failed.");CadMetalGpuTrace? resource=null;CadMetalGpuMesh? mesh=null;CadMetalGpuWallpaper? wallpaper=null;CadMetalGpuScene? scene=null;bool transferred=false;
      try{var pin=System.Runtime.InteropServices.GCHandle.Alloc(bytes,System.Runtime.InteropServices.GCHandleType.Pinned);try{texture.ReplaceRegion(MTLRegion.Create2D(0,0,(nuint)image.Width,(nuint)image.Height),0,pin.AddrOfPinnedObject(),(nuint)(image.Width*4));}finally{pin.Free();}
       resource=new(checked(imageUploads+1),texture,image.Width,image.Height,info,sourceDibDepth,ImageReleased);texture=null;imageTextures++;mesh=current?.Mesh?.Retain();wallpaper=current?.Wallpaper?.Retain();scene=new(mesh,wallpaper,resource,info);frames.ReplaceResource(scene);transferred=true;current=scene;imageUploads=resource.UploadId;return imageUploads;
      }catch{if(!transferred){if(scene is not null)scene.Dispose();else{resource?.Dispose();mesh?.Dispose();wallpaper?.Dispose();}}throw;}finally{texture?.Dispose();}
     }
    }
    internal void SetTraceState(Rhino5TraceImage info)
    {lock(sync){ObjectDisposedException.ThrowIf(disposed,this);var loaded=current?.Trace??throw new InvalidOperationException("No loaded trace pixels.");if(info.Filename!=loaded.Info.Filename)throw new InvalidOperationException("Filename changes require source image resolution and new pixels.");if(info.GrayScale!=loaded.Info.GrayScale)throw new InvalidOperationException("Gray changes require source pixel preparation.");_=Rhino5TracePlane.Create(info,loaded.Width,loaded.Height);var scene=new CadMetalGpuScene(current?.Mesh?.Retain(),current?.Wallpaper?.Retain(),loaded.Retain(),info);try{frames.ReplaceResource(scene);current=scene;}catch{scene.Dispose();throw;}}}
    internal void ClearTrace()
    {lock(sync){ObjectDisposedException.ThrowIf(disposed,this);var scene=new CadMetalGpuScene(current?.Mesh?.Retain(),current?.Wallpaper?.Retain());try{frames.ReplaceResource(scene);current=scene;}catch{scene.Dispose();throw;}}}
    private void MeshReleased(int count,Exception? error){lock(sync){buffers-=count;meshes--;if(error is not null)lastError??=error;}}
    private void ImageReleased(Exception? error){lock(sync){imageTextures--;if(error is not null)lastError??=error;}}
    internal void Invalidate(){lock(sync){ObjectDisposedException.ThrowIf(disposed,this);frames.Invalidate();}}

    // A successful submission takes ownership of drawable. A false/throwing submission leaves it with the caller.
    internal bool TrySubmit(MTLRenderPassDescriptor pass,ICAMetalDrawable drawable,Matrix4x4 matrix,int width,int height)
    {
        lock(sync)
        {
            ObjectDisposedException.ThrowIf(disposed,this);
            var lease=frames.TryBeginFrame();if(lease is null)return false;
            IMTLBuffer? camera=null,quad=null,traceQuad=null;IMTLCommandBuffer? command=null;Submission? frame=null;
            try
            {
                var scene=lease.Resource;var mesh=scene.Mesh;
                if(scene.Wallpaper is {} image){var placement=Rhino5WallpaperPlacement.Gdi(width,height,image.Width,image.Height,image.Scale);quad=renderer.Device.CreateBuffer(placement.CopyClipQuad(),MTLResourceOptions.StorageModeShared)??throw new InvalidOperationException("Visible wallpaper quad allocation failed.");}
                if(scene.Trace is {} traceImage&&scene.TraceInfo is {} traceInfo&&Rhino5TracePlane.Create(traceInfo,traceImage.Width,traceImage.Height) is {} plane){traceQuad=renderer.Device.CreateBuffer(plane.CopyNormalizedVertices(mesh?.Center??new(0,0,0),mesh?.Radius??1),MTLResourceOptions.StorageModeShared)??throw new InvalidOperationException("Trace world quad allocation failed.");}
                camera=renderer.Device.CreateBuffer(CadViewportCamera.MatrixScalars(matrix),MTLResourceOptions.StorageModeShared)??throw new InvalidOperationException("Visible camera upload failed.");
                command=renderer.CreateCommandBuffer();
                if(!command.RetainedReferences)throw new InvalidOperationException("Visible commands must retain presentation textures.");
                using(var encoder=command.CreateRenderCommandEncoder(pass)??throw new InvalidOperationException("Visible render encoder unavailable."))
                {if(scene.Wallpaper is {} wallpaper)imagePipeline!.Encode(encoder,wallpaper.Texture,quad!,false,false);if(traceQuad is not null&&scene.Trace is {} trace){encoder.SetVertexBuffer(camera,0,1);tracePipeline!.Encode(encoder,trace.Texture,traceQuad,scene.TraceInfo!.Value.Filtered,trace.DibDepth==32);}renderer.Encode(encoder,mesh?.Vertices,mesh?.Indices,camera,mesh?.TriangleCount??0);encoder.EndEncoding();}
                var info=new CadMetalFrameInfo(checked(++nextFrame),lease.Revision,mesh?.UploadId??0,width,height,null,CadMetalTimingCapabilities.SupportsNativeTiming(drawable),matrix){ImageUploadId=scene.Wallpaper?.UploadId??0,TraceUploadId=scene.Trace?.UploadId??0};
                frame=new Submission(lease,camera,quad,traceQuad,command,drawable,info);
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
                var cleanupError=frame is null?DisposeAll(camera,quad,traceQuad,command,lease):frame.Release(includeDrawable:false);
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
            if(frame.IsReleased)return;
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
        try{tracePipeline?.Dispose();imagePipeline?.Dispose();renderer.Dispose();}catch(Exception error){lastError??=error;}
    }
    public void Dispose()
    {
        lock(sync){if(disposed)return;disposed=true;current=null;frames.Dispose();if(frames.State.InFlight==0)ReleaseRenderer();}
    }
    private sealed class Submission(CadFrameLease<CadMetalGpuScene> lease,IMTLBuffer camera,IMTLBuffer? quad,IMTLBuffer? traceQuad,IMTLCommandBuffer command,ICAMetalDrawable drawable,CadMetalFrameInfo info)
    {
        private int released;
        internal bool IsReleased=>Volatile.Read(ref released)!=0;
        internal readonly CadMetalFrameInfo Info=info;
        internal Exception? Release(bool includeDrawable)
        {
            if(Interlocked.Exchange(ref released,1)!=0)return null;
            Exception? failure=null;
            void Free(IDisposable value){try{value.Dispose();}catch(Exception error){failure??=error;}}
            Free(camera);if(quad is not null)Free(quad);if(traceQuad is not null)Free(traceQuad);Free(lease);Free(command);if(includeDrawable)Free(drawable);
            return failure;
        }
    }
}
