using JewelDesign.Rendering.Candidate;
using JewelDesign.Geometry.Candidate;
namespace CandidateGeometryContractChecks;
public static class TracePlaneAssertions
{
 public static string Run()
 {
  int checks=0;void Check(bool ok,string why){checks++;if(!ok)throw new Exception(why);}
  var plane=new Rhino5ViewPlane(new(10,20,30),new(2,0,1),new(0,3,-1),new(99,98,97),new(0,0,0,123));
  var info=new Rhino5TraceImage(plane,4,5,"inert.png",false,false,false);
  var quad=Rhino5TracePlane.Create(info,400,200)!;
  Check(quad.Width==4&&quad.Height==5,"Saved extents preserved");
  Check(quad.CopyWorldCorners().AsSpan().SequenceEqual(new CandidatePoint3[]{new(18,20,34),new(18,35,29),new(10,35,25),new(10,20,30)}),"Original point arithmetic and UV corner order without axis normalization");
  Check(Rhino5TracePlane.Create(info with{Width=0,Height=5},400,200)!.Width==10,"Missing width derives from texture aspect");
  Check(Rhino5TracePlane.Create(info with{Width=4,Height=0},400,200)!.Height==2,"Missing height uses reciprocal aspect");
  Check(Rhino5TracePlane.Create(info with{Width=0,Height=0},400,200)is null,"Both extents zero skip");
  Check(Rhino5TracePlane.Create(info with{Hidden=true},400,200)is null,"Hidden skips");
  Check(Rhino5TracePlane.Create(info with{Width=-4},400,200)!.CopyWorldCorners()[0]==new CandidatePoint3(2,20,26),"Negative source extents not silently normalized");
  var packed=quad.CopyNormalizedVertices(new(10,20,30),2);
  Check(packed.AsSpan().SequenceEqual(new float[]{4,0,2,1,4,7.5f,-.5f,1,0,7.5f,-2.5f,1,0,0,0,1}),"World normalization matches current CAD mesh center/radius");
  checks++;try{quad.CopyNormalizedVertices(new(0,0,0),0);throw new Exception("Expected radius guard");}catch(ArgumentOutOfRangeException){}
  checks++;try{Rhino5TracePlane.Create(info with{Width=double.NaN},400,200);throw new Exception("Expected nonfinite guard");}catch(ArgumentException){}
  byte[] colors=[255,0,0,7,0,255,0,9,0,0,255,11,255,255,255,13];
  var gray32=Rhino5TraceRaster.Prepare(colors,4,1,32,true);Check(gray32.AsSpan().SequenceEqual(new byte[]{76,76,76,76,150,150,150,150,28,28,28,28,255,255,255,255}),"Original trace gray rounding and all-four-channel32bit writes");
  var gray24=Rhino5TraceRaster.Prepare(colors,4,1,24,true);Check(Enumerable.Range(0,4).All(i=>gray24[i*4+3]==255),"Original24bit trace stays opaque");
  Check(Rhino5TraceRaster.Prepare(colors,4,1,32,false).AsSpan().SequenceEqual(colors),"Color32bit alpha preserved");
  return "PASS original trace plane:"+checks+" extent/aspect/point/corner/normalization/boundary cases; source camera/gray/cache/document/PC parity separate.";
 }
}
