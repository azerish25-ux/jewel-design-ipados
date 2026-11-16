using JewelDesign.Views.Candidate;
using JewelDesign.Rendering.Candidate;
using JewelDesign.Geometry.Candidate;
namespace CandidateGeometryContractChecks;
public static class Rhino5SetViewPreflightAssertions
{
 public static string Run()
 {
  int checks=0;void Check(bool ok,string why){checks++;if(!ok)throw new Exception(why);}
  using var view=Rhino5ViewDefaults.Create(new Rhino5UserDataRuntime());
  Check(!Rhino5SetViewPreflight.Accepts(view),"Original fresh port-invalid default rejected by virtual validity guard");
  view.TrySetScreenPort(0,100,100,0,0,0);var baseline=view.Viewport;var details=view.Details;var mirror=view.TargetMirror;var metadata=view.Metadata;
  void State(Rhino5ViewportState state,Rhino5ViewportDetails? context=null)=>view.SetConvertedValues(state,mirror,context??details,metadata,[]);
  Check(Rhino5SetViewPreflight.Accepts(view),"Source-valid view accepted");
  State(baseline with {Frame=baseline.Frame with {CameraValid=false}});Check(!Rhino5SetViewPreflight.Accepts(view),"Camera-valid flag guard");
  State(baseline with {FrustumValid=false});Check(!Rhino5SetViewPreflight.Accepts(view),"Frustum-valid flag guard");
  State(baseline,details with {ScreenPort=details.ScreenPort with {Valid=false}});Check(!Rhino5SetViewPreflight.Accepts(view),"Port-valid flag guard");
  var unset=BitConverter.Int64BitsToDouble(unchecked((long)0xffe5f8bd9e331dd2));
  foreach(double bad in new[]{unset,double.NaN,double.PositiveInfinity,double.NegativeInfinity})for(int group=0;group<6;group++)for(int axis=0;axis<3;axis++)
  {
   var f=baseline.Frame;var p=new CandidateVector3(axis==0?bad:0,axis==1?bad:0,axis==2?bad:0);
   f=group switch {0=>f with {Location=new(p.X,p.Y,p.Z)},1=>f with {Direction=p},2=>f with {Up=p},3=>f with {X=p},4=>f with {Y=p},_=>f with {Z=p}};
   State(baseline with {Frame=f});Check(!Rhino5SetViewPreflight.Accepts(view),"Every camera vector/location component uses original scalar validity");
  }
  State(baseline with {Frame=baseline.Frame with {Direction=new(0,0,0),Up=new(0,0,0),X=new(2,3,4),Y=new(4,5,6),Z=new(6,7,8)}});
  Check(Rhino5SetViewPreflight.Accepts(view),"No invented unit/nonzero/orthogonality normalization beyond original flags/scalars");
  foreach(double bad in new[]{unset,double.NaN,double.PositiveInfinity,double.NegativeInfinity})for(int field=0;field<6;field++)
  {
   var b=baseline.Bounds;b=field switch {0=>b with {Left=bad},1=>b with {Right=bad},2=>b with {Bottom=bad},3=>b with {Top=bad},4=>b with {Near=bad},_=>b with {Far=bad}};
   State(baseline with {Bounds=b});Check(!Rhino5SetViewPreflight.Accepts(view),"Every frustum scalar checked");
  }
  foreach(double span in new[]{0d,Math.BitDecrement(1e-18),1e-18,Math.BitIncrement(1e-18),Math.BitDecrement(1e18),1e18,Math.BitIncrement(1e18),double.MaxValue})foreach(bool reversed in new[]{false,true})foreach(bool horizontal in new[]{false,true})
  {
   var b=baseline.Bounds;double edge=reversed? -span:span;b=horizontal?b with {Left=0,Right=edge}:b with {Bottom=0,Top=edge};
   State(baseline with {Bounds=b});bool expected=span>=1e-18&&span<=1e18;Check(Rhino5SetViewPreflight.Accepts(view)==expected,"Inclusive absolute span limits and reversed bounds");
  }
  State(baseline with {Bounds=baseline.Bounds with {Left= -double.MaxValue,Right=double.MaxValue}});Check(!Rhino5SetViewPreflight.Accepts(view),"Finite endpoints whose difference overflows rejected");
  foreach(uint projection in new uint[]{0,1,2,3,uint.MaxValue})foreach(double near in new[]{-10d,-0d,0d,1e-18,Math.BitDecrement(Rhino5SetViewPreflight.MinimumPerspectiveNear),Rhino5SetViewPreflight.MinimumPerspectiveNear,Math.BitIncrement(Rhino5SetViewPreflight.MinimumPerspectiveNear),100d})
  {
   State(baseline with {Projection=projection,Bounds=baseline.Bounds with {Near=near,Far= -99}});
   Check(Rhino5SetViewPreflight.Accepts(view)==(projection!=2||near>=Rhino5SetViewPreflight.MinimumPerspectiveNear),"Only projection2 imposes native near minimum; no extra near/far ordering or enum guard");
  }
  double positiveUnset= -unset;
  State(baseline with {Frame=baseline.Frame with {Location=new(positiveUnset,0,0)},Projection=2,Bounds=baseline.Bounds with {Near=positiveUnset,Far= -1}});
  Check(Rhino5SetViewPreflight.Accepts(view),"Original positive-unset counterpart remains scalar-valid where no span upper bound applies");
  State(baseline,details with {ScreenPort=new(0,0,0,0,0,0,true),OpaqueCopyByte=255});
  Check(Rhino5SetViewPreflight.Accepts(view),"Virtual port validity trusts flag rather than adding unobserved port-coordinate checks");
  State(baseline with {Target=new(double.NaN,0,0)},details with {PerspectiveMinNearDist=double.NaN,PerspectiveMinNearOverFar=double.NaN});
  Check(Rhino5SetViewPreflight.Accepts(view),"Target/mirror/near-constraint metadata not silently added to source validation");
  var prior=view.Viewport;var oldMirror=view.TargetMirror;Rhino5SetViewPreflight.Accepts(view);
  Check(view.Viewport==prior&&view.TargetMirror==oldMirror,"Preflight is read-only and does not repair or normalize state");
  return "PASS original-derived SetView preflight:"+checks+" flag/component/span/projection-near boundary checks; complete SetView/conduits/document/Pop/PC/physical parity separate.";
 }
}
