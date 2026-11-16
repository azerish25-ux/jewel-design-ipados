using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using CoreGraphics;
using UIKit;
using JewelDesign.Geometry.Candidate;
using JewelDesign.Rendering.Candidate;
using JewelDesign.Rendering.Candidate.iOS;
namespace ViewportPresentationCandidate;
internal sealed record TraceFrameMark(string Stage,long Revision,long MeshUploadId,long TraceUploadId,int Width,int Height,float[] Matrix);
internal sealed record TracePresentationReport(int SchemaVersion,List<TraceFrameMark> Frames,CadMetalViewStats Hidden,CadMetalViewStats BeforeDisposal,CadMetalViewStats AfterDisposal,int DisposalInFlight,string Scope);
internal static class TracePresentationChecks
{
 private static void Check(bool ok,string why){if(!ok)throw new Exception(why);}
 private static async Task Drain(Func<bool> ready){for(int i=0;i<500;i++){if(ready())return;await Task.Delay(20);}throw new Exception("Trace scene drain timeout");}
 private static async Task<CadMetalFrameInfo> Present(CadMetalView view,Action change){
  long revision=long.MaxValue;var done=new TaskCompletionSource<CadMetalFrameInfo>(TaskCreationOptions.RunContinuationsAsynchronously);
  void P(CadMetalFrameInfo f){if(f.Revision>=revision&&f.SupportsNativePresentationTiming&&f.PresentedTime>0)done.TrySetResult(f);}
  void G(CadMetalFrameInfo f){if(f.Revision>=revision&&!f.SupportsNativePresentationTiming)done.TrySetResult(f);}
  void E(Exception e)=>done.TrySetException(e);
  view.FramePresented+=P;view.FrameGpuCompleted+=G;view.FrameFailed+=E;
  try{change();revision=view.Stats.SceneRevision;view.Draw();return await done.Task.WaitAsync(TimeSpan.FromSeconds(30));}
  finally{view.FramePresented-=P;view.FrameGpuCompleted-=G;view.FrameFailed-=E;}
 }
 public static async Task<TracePresentationReport> Run(UIView parent,Func<string,Task> capture){
  foreach(var old in parent.Subviews.OfType<CadMetalView>().ToArray()){old.RemoveFromSuperview();old.Dispose();}
  var mesh=await Task.Run(ViewportPresentationChecks.BuildOriginalMesh);
  using var input=typeof(CandidateGeometryContractChecks.TracePlaneAssertions).Assembly.GetManifestResourceStream("VisibleWallpaperFixture")!;
  using var fixture=JsonDocument.Parse(input);
  using var image=AppleLocalImageDecoder.DecodeStraight(LocalImageAsset.FromBytes("world-trace.png",Convert.FromBase64String(fixture.RootElement.GetProperty("green").GetString()!)));
  var c=mesh.Center;double r=mesh.Radius;
  var plane=new Rhino5ViewPlane(new(c.X-1.2*r,c.Y-.9*r,c.Z-.35*r),new(1,0,0),new(0,1,0),new(0,0,1),new(0,0,1,-(c.Z-.35*r)));
  var info=new Rhino5TraceImage(plane,2.4*r,1.8*r,"world-trace.png",false,false,false);
  var view=new CadMetalView(parent.Bounds){AutoresizingMask=UIViewAutoresizing.FlexibleWidth|UIViewAutoresizing.FlexibleHeight};parent.AddSubview(view);parent.LayoutIfNeeded();
  var marks=new List<TraceFrameMark>();
  async Task Record(string name,CadMetalFrameInfo frame){marks.Add(new(name,frame.Revision,frame.MeshUploadId,frame.TraceUploadId,frame.Width,frame.Height,CadViewportCamera.MatrixScalars(frame.ViewProjection)));await capture(name);}
  await Record("trace-original",await Present(view,()=>view.SetMesh(mesh)));
  long meshes=view.Stats.MeshUploads;
  await Record("trace-image",await Present(view,()=>view.SetTracePixels(image,info,32)));
  long images=view.Stats.ImageUploads;
  Check(view.Stats.MeshUploads==meshes&&view.Stats.LiveImageTextures==1,"Trace upload changed mesh identity/resources");
  await Record("trace-orbit",await Present(view,()=>view.SetCameraPose(-.7f,.2f)));
  Check(view.Stats.MeshUploads==meshes&&view.Stats.ImageUploads==images,"Orbit reuploaded world trace/mesh");
  await Record("trace-hidden",await Present(view,()=>{view.SetCameraPose(.6f,.35f);view.SetTraceState(info with{Hidden=true});}));
  var hidden=view.Stats;Check(hidden.LiveImageTextures==1&&hidden.ImageUploads==images,"Hidden state discarded/reuploaded loaded trace");
  var moved=info with{Width=0,Height=1.1*r,Filtered=true,Plane=plane with{Origin=new(c.X-.8*r,c.Y-.55*r,c.Z-.7*r)}};
  await Record("trace-moved",await Present(view,()=>view.SetTraceState(moved)));
  Check(view.Stats.ImageUploads==images,"Trace pose/filter metadata reuploaded pixels");
  await Record("trace-final",await Present(view,view.ClearTrace));
  await Drain(()=>view.Stats.InFlightFrames==0&&view.Stats.LiveImageTextures==0);
  for(int i=0;i<12;i++){view.SetTracePixels(image,info with{Hidden=i%3==0,Filtered=i%2==0},32);view.SetMesh(mesh);view.Draw();}
  var before=view.Stats;int pending=before.InFlightFrames;view.RemoveFromSuperview();view.Dispose();
  await Drain(()=>view.Stats.InFlightFrames==0&&view.Stats.LiveSceneResources==0&&view.Stats.LiveImageTextures==0&&view.Stats.LiveMeshResources==0);
  var after=view.Stats;Check(after.LastError is null&&after.LiveMeshBuffers==0,"Trace scene disposal retained work/failure");
  return new(1,marks,hidden,before,after,pending,"Actual world-plane trace pixels with current fitted candidate camera/mesh normalization. Original source camera/cache/mipmaps/resampling/missing-texture diagnostics/document/PC/physical parity remains open.");
 }
}
