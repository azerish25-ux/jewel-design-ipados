using JewelDesign.Documents;
using JewelDesign.Features.Candidate;
using JewelDesign.Geometry.Candidate;
using Oracle=CandidateGeometryContractChecks.Recovered.Collections;
namespace CandidateGeometryContractChecks;
internal static class BrepGraphAssertions
{
 private static void Check(bool v,string m){if(!v)throw new Exception(m);}
 private static void Near(CandidatePoint3 a,CandidatePoint3 b){if(!double.IsFinite(a.X)||!double.IsFinite(b.X)||Math.Abs(a.X-b.X)>1e-6||Math.Abs(a.Y-b.Y)>1e-6||Math.Abs(a.Z-b.Z)>1e-6)throw new Exception("BRep graph geometry mismatch");}
 private static void Throws<T>(Action a)where T:Exception{try{a();}catch(T){return;}throw new Exception("Expected "+typeof(T).Name);}
 private static LegacyCsg4Document Wire(LegacyCsg4Document d,int index,params (int Node,int Channel)[] links){for(int i=0;i<d.Records[index].SimpleBlock!.ObjectIndices.Count;i++)d=d.WithInputLink(index,i,i<links.Length?links[i].Node:-1,i<links.Length?links[i].Channel:-5);return d;}
 private static CandidatePoint3 Apply(LegacyAffineTransform m,CandidatePoint3 p)=>new(m.M11*p.X+m.M12*p.Y+m.M13*p.Z+m.M14,m.M21*p.X+m.M22*p.Y+m.M23*p.Z+m.M24,m.M31*p.X+m.M32*p.Y+m.M33*p.Z+m.M34);
 public static string Run(){
  using var input=typeof(BrepGraphAssertions).Assembly.GetManifestResourceStream("RingRailLibraryFixture")!;using var bytes=new MemoryStream();input.CopyTo(bytes);var original=bytes.ToArray();var document=LegacyCsg4Codec.Read(original);
  using var context=CandidateContext.Create();CandidateDataSnapshot captured;
  using(var box=context.CreateBox(2,3,4))foreach(var mode in new[]{CandidateShapeBoundsMode.Fast,CandidateShapeBoundsMode.Accurate}){var bounds=context.GetShapeBounds(box,mode);Near(bounds.Minimum,new(0,0,0));Near(bounds.Maximum,new(2,3,4));Throws<CandidateGeometryException>(()=>context.GetShapeBounds(box,(CandidateShapeBoundsMode)99));}
  using(var circle=context.CreateCircle(2,CandidatePlane.WorldXY))using(var rotated=context.TransformAffine(circle,new(Math.Sqrt(.5),-Math.Sqrt(.5),0,0,Math.Sqrt(.5),Math.Sqrt(.5),0,0,0,0,1,0))){var fast=context.GetShapeBounds(rotated,CandidateShapeBoundsMode.Fast);var accurate=context.GetShapeBounds(rotated,CandidateShapeBoundsMode.Accurate);Check(Math.Abs(accurate.Maximum.X-2)<=1e-6&&fast.Maximum.X>accurate.Maximum.X+.1,"Fast/accurate native policies were conflated");}

  using(var graph=new CandidateRailGraphSession(context,document,new(1e-7,1e-7,1e-7))){
   graph.Generate(134);var value=graph.ReadOutput(134,0);Check(value.Brep.HasValue&&!value.Curve.HasValue&&!value.Profile.HasValue&&!value.HasNumber,"BRep was coerced to another channel kind");
   Check(graph.OutputChannelCount(134)==1&&ReferenceEquals(graph.ReadOutput(134,1),LegacyChannelValue.Empty),"Sweep output cardinality");
   Check(graph.ReadOutputIdentifier(134,0)==document.Records[134].SimpleBlock!.Identifier+"_0","Source sweep output name");
   double number=7;bool flag=true;Check(!value.GetDouble(ref number)&&number==7&&!value.GetBoolean(ref flag)&&flag,"BRep numeric conversion changed defaults");
   Check(graph.TryReadTransform(134,0,out var identity)&&identity==LegacyAffineTransform.Identity,"BRep initial transform");
   Throws<NotSupportedException>(()=>graph.RetainCurve(134));using var surface=graph.RetainBrep(134);Check(context.Inspect(surface).IsValid,"Graph BRep invalid");
   captured=graph.CaptureOutput(134);Throws<NotSupportedException>(()=>captured.RetainCurve(captured.Value));using var before=captured.RetainBrep(captured.Value);var domain=context.InspectSurface(before);var point=context.PointAtSurface(before,.25,(domain.FirstV+domain.LastV)*.5);
   Check(LegacyCsg4Codec.Write(graph.Values.ExportCurrentDocument()).AsSpan().SequenceEqual(original),"Generating BRep changed source archive");
   graph.Values.SetValue(2,document.Records[2].Scalar!.Value+.25);graph.Generate(134);using var fresh=graph.RetainBrep(134);var freshDomain=context.InspectSurface(fresh);Check(context.PointAtSurface(fresh,.25,(freshDomain.FirstV+freshDomain.LastV)*.5)!=point,"Parameter edit did not change BRep");Near(context.PointAtSurface(before,.25,(domain.FirstV+domain.LastV)*.5),point);
   var saved=LegacyCsg4Codec.Read(LegacyCsg4Codec.Write(graph.Values.ExportCurrentDocument()));using var loaded=new CandidateRailGraphSession(context,saved,new(1e-7,1e-7,1e-7));loaded.Generate(134);using var reopened=loaded.RetainBrep(134);var reopenedDomain=context.InspectSurface(reopened);Near(context.PointAtSurface(reopened,.25,(reopenedDomain.FirstV+reopenedDomain.LastV)*.5),context.PointAtSurface(fresh,.25,(freshDomain.FirstV+freshDomain.LastV)*.5));
   var tight=context.GetShapeBounds(fresh,CandidateShapeBoundsMode.Accurate);var fast=context.GetShapeBounds(fresh,CandidateShapeBoundsMode.Fast);Check(fast.Minimum.X<=tight.Minimum.X+1e-6&&fast.Maximum.X>=tight.Maximum.X-1e-6,"Fast BRep box does not contain accurate geometry");
   var old=graph.ReadOutput(134,0);ulong entries=context.DiagnosticLiveShapeCount;graph.Values.SetValue(129,1);Throws<NotSupportedException>(()=>graph.Generate(134));Check(ReferenceEquals(old,graph.ReadOutput(134,0))&&entries==context.DiagnosticLiveShapeCount,"Unsupported sweep replaced cache or leaked");
  }
  using(var surviving=captured.RetainBrep(captured.Value))Check(context.Inspect(surviving).IsValid,"Snapshot did not survive graph disposal");captured.Dispose();captured.Dispose();Check(context.DiagnosticLiveShapeCount==0,"BRep graph snapshot cleanup leaked");
  var mixed=Wire(Wire(Wire(document,187,(134,0),(4,0)),238,(187,0),(8,0)),216,(187,0),(8,0));mixed=Wire(mixed,61,(134,-1));mixed=Wire(mixed,52,(238,0),(50,0));mixed=Wire(mixed,58,(52,0),(49,0));
  using(var graph=new CandidateRailGraphSession(context,mixed,new(1e-7,1e-7,1e-7))){
   graph.Values.SetValue(40,1);graph.Values.SetValue(41,2);graph.Values.SetValue(42,3);graph.Generate(58,216,61);Check(graph.OutputChannelCount(216)==4,"Unarray BRep/mixed count");Check(graph.ReadOutput(216,0).Brep.HasValue&&graph.ReadOutput(216,1).Curve.HasValue&&graph.ReadOutput(216,2).HasNumber&&graph.ReadOutput(216,3).Array!.Count==0,"Unarray changed mixed kinds");
   Check(graph.ReadOutput(61,0).Array!.Count==0,"Saved negative-channel spare-socket behavior was normalized away");
   var wildcard=Oracle.Values.Run(false,new[]{new Oracle.ObservationInput(LegacyChannelValue.Empty,new[]{graph.ReadOutput(134,0),graph.ReadOutput(4,0)},-1)});
   using var wildcardSnapshot=graph.CaptureValue(wildcard[0]);using var wildcardBrep=wildcardSnapshot.RetainBrep(wildcardSnapshot.Value.Array![0]);Check(context.Inspect(wildcardBrep).IsValid,"Source wildcard body lost actual BRep ownership");
   using var source=graph.CaptureOutput(238);using var transformed=graph.CaptureOutput(58);var from=source.Value.Array![0].Array![0];var to=transformed.Value.Array![0].Array![0];Check(to.Brep.HasValue&&!to.Curve.HasValue,"Array transform coerced BRep into curve");
   using var a=source.RetainBrep(from);using var b=transformed.RetainBrep(to);var d=context.InspectSurface(a);var m=graph.ReadOutput(50,0).Transform!.Value;var n=graph.ReadOutput(49,0).Transform!.Value;Check(m*n!=n*m,"Transform order test needs noncommuting inputs");Near(context.PointAtSurface(b,.25,(d.FirstV+d.LastV)*.5),Apply(n,Apply(m,context.PointAtSurface(a,.25,(d.FirstV+d.LastV)*.5))));Check(transformed.ReadTransform(to)==source.ReadTransform(from)*m*n,"BRep cumulative transform order differs from source");
  }
  Check(context.DiagnosticLiveShapeCount==0,"Mixed BRep forwarding leaked native entries");
  var sizing=Wire(document,49,(134,0),(35,0),(36,0),(37,0));using(var graph=new CandidateRailGraphSession(context,sizing,new(1e-7,1e-7,1e-7))){graph.Generate(49);using var brep=graph.RetainBrep(134);var bounds=context.GetShapeBounds(brep,CandidateShapeBoundsMode.Fast);var expected=TransformProducerCandidate.ScaleToSize(bounds,graph.ReadOutput(35,0),graph.ReadOutput(36,0),graph.ReadOutput(37,0));Check(graph.ReadOutput(49,0).Transform==expected,"Source old BRep box policy was not used for scale");}
  Check(context.DiagnosticLiveShapeCount==0,"BRep sizing leaked native entries");
  return "PASS owned BRep graph:real134 typed output/name/edit-save-reopen, kind rejection/transform metadata, nested mixed arrays/unarray/wildcards/affine, retained snapshots and failed-generation cleanup; candidate subset, no original-PC/full block parity.";
 }
}
