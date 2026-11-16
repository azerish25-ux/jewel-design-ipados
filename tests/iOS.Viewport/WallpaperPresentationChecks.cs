using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using CoreGraphics;
using UIKit;
using JewelDesign.Rendering.Candidate;
using JewelDesign.Rendering.Candidate.iOS;
namespace ViewportPresentationCandidate;
internal sealed record WallpaperFrameMark(string Stage,long Revision,long MeshUploadId,long ImageUploadId,int Width,int Height,int Left,int Top,int ImageWidth,int ImageHeight);
internal sealed record WallpaperPresentationReport(int SchemaVersion,List<WallpaperFrameMark> Frames,CadMetalViewStats AfterClear,CadMetalViewStats BeforeDisposal,CadMetalViewStats AfterDisposal,CadMetalViewStats FinalView,int DisposalInFlight,string Scope);
internal static class WallpaperPresentationChecks
{
 private static void Check(bool ok,string why){if(!ok)throw new Exception(why);}
 private static async Task Drain(Func<bool> ready){for(int i=0;i<500;i++){if(ready())return;await Task.Delay(20);}throw new Exception("Wallpaper native resource drain timeout");}
 private static async Task<CadMetalFrameInfo> Present(CadMetalView view,Action change)
 {
  long revision=long.MaxValue;
  var done=new TaskCompletionSource<CadMetalFrameInfo>(TaskCreationOptions.RunContinuationsAsynchronously);
  void Presented(CadMetalFrameInfo f){if(f.Revision>=revision&&f.SupportsNativePresentationTiming&&f.PresentedTime>0)done.TrySetResult(f);}
  void Completed(CadMetalFrameInfo f){if(f.Revision>=revision&&!f.SupportsNativePresentationTiming)done.TrySetResult(f);}
  void Failed(Exception e)=>done.TrySetException(e);
  view.FramePresented+=Presented;view.FrameGpuCompleted+=Completed;view.FrameFailed+=Failed;
  try{change();revision=view.Stats.SceneRevision;view.Draw();return await done.Task.WaitAsync(TimeSpan.FromSeconds(30));}
  finally{view.FramePresented-=Presented;view.FrameGpuCompleted-=Completed;view.FrameFailed-=Failed;}
 }
 public static async Task<WallpaperPresentationReport> Run(UIView parent,Func<string,Task> capture)
 {
  foreach(var old in parent.Subviews.OfType<CadMetalView>().ToArray()){old.RemoveFromSuperview();old.Dispose();}
  var mesh=await Task.Run(ViewportPresentationChecks.BuildOriginalMesh);
  var assembly=typeof(CandidateGeometryContractChecks.WallpaperPlacementAssertions).Assembly;
  using var fixtureStream=assembly.GetManifestResourceStream("VisibleWallpaperFixture")??throw new Exception("Wallpaper fixture missing");
  using var fixture=JsonDocument.Parse(fixtureStream);
  using var green=AppleLocalImageDecoder.DecodeStraight(LocalImageAsset.FromBytes("green.png",Convert.FromBase64String(fixture.RootElement.GetProperty("green").GetString()!)));
  using var blue=AppleLocalImageDecoder.DecodeStraight(LocalImageAsset.FromBytes("blue.png",Convert.FromBase64String(fixture.RootElement.GetProperty("blue").GetString()!)));
  var marks=new List<WallpaperFrameMark>();
  var view=new CadMetalView(parent.Bounds){AutoresizingMask=UIViewAutoresizing.FlexibleWidth|UIViewAutoresizing.FlexibleHeight};
  parent.AddSubview(view);parent.LayoutIfNeeded();
  async Task Record(string stage,CadMetalFrameInfo frame,int scale){
   var p=scale==0?default:Rhino5WallpaperPlacement.Gdi(frame.Width,frame.Height,512,512,scale);
   marks.Add(new(stage,frame.Revision,frame.MeshUploadId,frame.ImageUploadId,frame.Width,frame.Height,p.Left,p.Top,p.Width,p.Height));await capture(stage);
  }
  await Record("paper-original",await Present(view,()=>view.SetMesh(mesh)),0);
  long uploads=view.Stats.MeshUploads;
  await Record("paper-green",await Present(view,()=>view.SetWallpaperSource(green,32,512,512)),1);
  long sourceRevision=view.Stats.SceneRevision;try{view.SetWallpaperSource(green,32,512,512,false,null,1);throw new Exception("Expected source wallpaper budget failure");}catch(System.IO.InvalidDataException){}Check(view.Stats.SceneRevision==sourceRevision,"Failed wallpaper source replaced current scene");
  Check(view.Stats.MeshUploads==uploads&&view.Stats.LiveMeshBuffers==2,"Wallpaper upload replaced/reuploaded CAD mesh");
  await Record("paper-blue",await Present(view,()=>view.SetWallpaperPixels(blue,2)),2);
  Check(view.Stats.MeshUploads==uploads&&view.Stats.LiveImageTextures==1,"Image replacement retained obsolete texture after completed frame");
  await Record("paper-clear",await Present(view,view.ClearWallpaper),0);
  await Drain(()=>view.Stats.InFlightFrames==0&&view.Stats.LiveImageTextures==0);
  var cleared=view.Stats;Check(cleared.LiveMeshBuffers==2&&cleared.MeshUploads==uploads,"Clear destroyed/reuploaded CAD geometry");
  await Present(view,()=>view.SetWallpaperPixels(blue,2));
  var bounds=parent.Bounds;
  var resized=await Present(view,()=>{view.Frame=new CGRect(bounds.X+24,bounds.Y+40,bounds.Width-48,bounds.Height-80);view.SetNeedsLayout();view.LayoutIfNeeded();});
  await Record("paper-resized",resized,2);
  for(int i=0;i<12;i++){view.SetWallpaperPixels(i%2==0?green:blue,1+i%2);view.SetMesh(mesh);view.Draw();}
  var before=view.Stats;int pending=before.InFlightFrames;Check(before.PeakInFlightFrames<=3,"Combined scene exceeded native frame bound");
  view.RemoveFromSuperview();view.Dispose();
  await Drain(()=>view.Stats.InFlightFrames==0&&view.Stats.LiveMeshResources==0&&view.Stats.LiveImageTextures==0&&view.Stats.LiveSceneResources==0);
  var after=view.Stats;Check(after.LastError is null&&after.LiveMeshBuffers==0,"Combined scene disposal retained native resources/failure");
  var final=new CadMetalView(parent.Bounds){AutoresizingMask=UIViewAutoresizing.FlexibleWidth|UIViewAutoresizing.FlexibleHeight};parent.AddSubview(final);
  await Record("paper-final",await Present(final,()=>final.SetMesh(mesh)),0);await Drain(()=>final.Stats.InFlightFrames==0);
  return new(1,marks,cleared,before,after,final.Stats,pending,"Actual visible source-GDI placement of supplied resized opaque wallpaper pixels composed with regenerated CAD. GDI resampling, trace world-plane, source setters/document/PC/physical parity remain open.");
 }
}
