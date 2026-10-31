using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using CoreGraphics;
using UIKit;
using JewelDesign.Documents;
using JewelDesign.Features.Candidate;
using JewelDesign.Geometry.Candidate;
using JewelDesign.Rendering.Candidate;
using JewelDesign.Rendering.Candidate.iOS;
namespace ViewportPresentationCandidate;

internal sealed record ViewportFrameMark(string Stage,long FrameId,long Revision,long MeshUploadId,int Width,int Height,double? PresentedTime,bool SupportsNativePresentationTiming,string EvidenceKind);
internal sealed record ViewportPresentationReport(int SchemaVersion,string Scope,List<ViewportFrameMark> Frames,
    CadMetalViewStats BeforeDisposal,CadMetalViewStats AfterDisposal,CadMetalViewStats FinalView,int DisposalInFlight);
internal static class ViewportPresentationChecks
{
    private sealed record Meshes(CadViewportMesh Original,CadViewportMesh Edited,CadViewportMesh Empty,CadViewportMesh Fine);
    private static void Check(bool value,string message) { if(!value)throw new Exception(message); }
    private static Meshes BuildMeshes()
    {
        using var input=typeof(CandidateGeometryContractChecks.Assertions).Assembly.GetManifestResourceStream("RingRailLibraryFixture")!;
        using var memory=new MemoryStream();input.CopyTo(memory);
        using var context=CandidateContext.Create();
        using var graph=new CandidateRailGraphSession(context,LegacyCsg4Codec.Read(memory.ToArray()),new(1e-7,1e-7,1e-7));
        graph.Generate(140);using var source=graph.RetainBrep(140);
        var original=CadViewportMesh.FromSnapshot(context.TessellateBrep(source,.04,.35));
        var fine=CadViewportMesh.FromSnapshot(context.TessellateBrep(source,.004,.15));
        graph.Values.SetValue(2,graph.Values.Document.Records[2].Scalar!.Value+.4);graph.Values.SetValue(136,.8);graph.Generate(140);
        using var changed=graph.RetainBrep(140);var edited=CadViewportMesh.FromSnapshot(context.TessellateBrep(changed,.04,.35));
        using var box=context.CreateBox(1,1,1);using var far=context.Translate(box,10,0,0);
        using var emptyShape=context.Boolean(CandidateBooleanOperation.Common,box,far,0);
        var empty=CadViewportMesh.FromSnapshot(context.TessellateBrep(emptyShape,.04,.35));
        return new(original,edited,empty,fine);
    }
    private static async Task<CadMetalFrameInfo> Present(CadMetalView view,Func<long> change)
    {
        long expected=-1;
        var completion=new TaskCompletionSource<CadMetalFrameInfo>(TaskCreationOptions.RunContinuationsAsynchronously);
        void Presented(CadMetalFrameInfo frame) { if(frame.MeshUploadId==expected&&frame.SupportsNativePresentationTiming&&frame.PresentedTime>0)completion.TrySetResult(frame); }
        void GpuCompleted(CadMetalFrameInfo frame) { if(frame.MeshUploadId==expected&&!frame.SupportsNativePresentationTiming)completion.TrySetResult(frame); }
        void Failed(Exception error)=>completion.TrySetException(error);
        view.FramePresented+=Presented;view.FrameGpuCompleted+=GpuCompleted;view.FrameFailed+=Failed;
        try
        {
            expected=change();view.Draw();
            var frame=await completion.Task.WaitAsync(TimeSpan.FromSeconds(30));
            Check(frame.Width>0&&frame.Height>0&&(!frame.SupportsNativePresentationTiming||frame.PresentedTime is double time&&double.IsFinite(time)&&time>0),"Presented frame has invalid dimensions/time");
            return frame;
        }
        finally {view.FramePresented-=Presented;view.FrameGpuCompleted-=GpuCompleted;view.FrameFailed-=Failed;}
    }
    private static async Task Drain(Func<bool> done)
    {
        for(int i=0;i<500;i++){if(done())return;await Task.Delay(20);}
        throw new Exception("Visible Metal resource completion timeout");
    }
    public static async Task<ViewportPresentationReport> Run(UIView parent,Func<string,Task> capture)
    {
        var meshes=await Task.Run(BuildMeshes);
        Check(meshes.Original.TriangleCount>100&&meshes.Edited.TriangleCount>100&&meshes.Empty.TriangleCount==0,"Actual native inputs invalid");
        var frames=new List<ViewportFrameMark>();
        var view=new CadMetalView(parent.Bounds){AutoresizingMask=UIViewAutoresizing.FlexibleWidth|UIViewAutoresizing.FlexibleHeight};
        parent.AddSubview(view);parent.LayoutIfNeeded();
        async Task Record(string stage,CadMetalFrameInfo frame)
        {
            frames.Add(new(stage,frame.FrameId,frame.Revision,frame.MeshUploadId,frame.Width,frame.Height,frame.PresentedTime,frame.SupportsNativePresentationTiming,frame.SupportsNativePresentationTiming?"native-presented":"gpu-completed"));
            await capture(stage);
        }
        var original=await Present(view,()=>view.SetMesh(meshes.Original));await Record("original",original);
        long uploads=view.Stats.MeshUploads;
        var orbit=await Present(view,()=>{view.SetCameraPose(-.7f,.2f);return uploads;});await Record("orbit",orbit);
        Check(view.Stats.MeshUploads==uploads,"Orbit re-uploaded an unchanged mesh");
        var edited=await Present(view,()=>view.SetMesh(meshes.Edited));await Record("edited",edited);
        var empty=await Present(view,()=>view.SetMesh(meshes.Empty));await Record("empty",empty);
        await Drain(()=>view.Stats.InFlightFrames==0);Check(view.Stats.LiveMeshBuffers==0,"Empty output retained obsolete GPU buffers");
        await Present(view,()=>view.SetMesh(meshes.Original));
        var bounds=parent.Bounds;
        var resized=await Present(view,()=>{view.Frame=new CGRect(bounds.X+24,bounds.Y+40,bounds.Width-48,bounds.Height-80);view.SetNeedsLayout();view.LayoutIfNeeded();return view.Stats.MeshUploads;});
        Check(resized.Width!=original.Width||resized.Height!=original.Height,"Viewport resize did not change its drawable");await Record("resized",resized);
        for(int i=0;i<12;i++) {view.SetMesh(i%2==0?meshes.Fine:meshes.Edited);view.SetCameraPose(-.8f+i*.1f,.2f);view.Draw();}
        var before=view.Stats;int pending=before.InFlightFrames;
        Check(before.PeakInFlightFrames<=3&&before.SubmittedFrames>0,"Visible frame bound failed");
        view.RemoveFromSuperview();view.Dispose();
        await Drain(()=>view.Stats.InFlightFrames==0&&view.Stats.LiveMeshResources==0&&view.Stats.LiveMeshBuffers==0);
        var after=view.Stats;Check(after.IsDisposed&&after.LastError is null,"Disposed viewport failed/retained GPU work");
        var finalView=new CadMetalView(parent.Bounds){AutoresizingMask=UIViewAutoresizing.FlexibleWidth|UIViewAutoresizing.FlexibleHeight};parent.AddSubview(finalView);
        var final=await Present(finalView,()=>finalView.SetMesh(meshes.Original));await Record("final",final);
        await Drain(()=>finalView.Stats.InFlightFrames==0);Check(finalView.Stats.LiveMeshBuffers==2&&finalView.Stats.LastError is null,"Visible final mesh ownership invalid");
        return new(2,"Actual visible MTKView CAD/GPU ownership checkpoint; native timing is recorded only when supported; full product UI/source-camera/material/picking/PC/physical parity separate",frames,before,after,finalView.Stats,pending);
    }
}
