using JewelDesign.Documents;
using JewelDesign.Features.Candidate;
using JewelDesign.Geometry.Candidate;
namespace CandidateGeometryContractChecks;
internal static class CurveExportAssertions
{
 static void Check(bool value,string message){if(!value)throw new Exception(message);}
 static void Throws<T>(Action action) where T:Exception{try{action();}catch(T){return;}throw new Exception("Expected "+typeof(T).Name);}
 public static string Run(){using var context=CandidateContext.Create();int samples=0,cases=0;
  void RoundTrip(CandidateShape curve){var before=context.InspectCurve(curve);var definition=context.ExportNurbs(curve);using var rebuilt=context.CreateNurbs(definition);var after=context.InspectCurve(rebuilt);Check(before.FirstParameter==after.FirstParameter&&before.LastParameter==after.LastParameter,"Export changed active domain");for(int i=0;i<=32;i++){double t=before.FirstParameter+(before.LastParameter-before.FirstParameter)*i/32;var a=context.PointAt(curve,t);var b=context.PointAt(rebuilt,t);Check(Math.Abs(a.X-b.X)<1e-9&&Math.Abs(a.Y-b.Y)<1e-9&&Math.Abs(a.Z-b.Z)<1e-9,"Export/reimport curve geometry differs");samples++;}Check(context.InspectCurve(curve)==before,"Export mutated input");cases++;}
  CandidateNurbsDefinition detached;
  using(var circle=context.CreateCircle(2,CandidatePlane.WorldXY)){
   RoundTrip(circle);detached=context.ExportNurbs(circle);
   using var affine=context.TransformAffine(circle,new(2,.3,0,1,0,1,0,-2,0,0,1,.4));RoundTrip(affine);
   using var trimmed=context.TrimCurve(affine,.2,4.8);RoundTrip(trimmed);
   using var reversed=context.ReverseCurve(trimmed);RoundTrip(reversed);
   using var seam=context.RelocateClosedCurveSeam(circle,.4);RoundTrip(seam);
  }
  var raw=new CandidateNurbsDefinition(2,new[]{new CandidatePoint3(0,0,0),new(1,0,1),new(2,1,0),new(3,2,1)},new[]{-2.0,-1,0,1,2,3,4},new[]{1.0,.7,1.2,1});
  using(var imported=context.CreateNurbs(raw)){RoundTrip(imported);var copy=context.ExportNurbs(imported);Check(raw.Poles.SequenceEqual(copy.Poles)&&raw.KnotVector.SequenceEqual(copy.KnotVector)&&raw.PoleWeights!.SequenceEqual(copy.PoleWeights!),"Nonclamped source controls changed");}
  var k=new[]{0.0,0,1,1};using(var a=context.CreateNurbs(new(1,new[]{new CandidatePoint3(0,0,0),new(1,0,0)},k)))using(var b=context.CreateNurbs(new(1,new[]{new CandidatePoint3(1,0,0),new(2,1,0)},k)))using(var joined=context.JoinCurves(a,b,1e-8)){RoundTrip(a);RoundTrip(joined);}
  using(var producer=CandidateContext.Create())using(var original=producer.CreateNurbs(detached)){detached=producer.ExportNurbs(original);}
  using(var other=CandidateContext.Create())using(var saved=other.CreateNurbs(detached)){Check(other.InspectCurve(saved).PoleCount==9,"Detached definition lost controls");Throws<ArgumentException>(()=>context.ExportNurbs(saved));}
  using(var box=context.CreateBox(1,1,1))Throws<CandidateGeometryException>(()=>context.ExportNurbs(box));
  var disposed=context.CreateNurbs(detached);disposed.Dispose();Throws<ObjectDisposedException>(()=>context.ExportNurbs(disposed));

  using(var stream=typeof(CurveExportAssertions).Assembly.GetManifestResourceStream("RingRailLibraryFixture")!)using(var memory=new MemoryStream()){
   stream.CopyTo(memory);var original=memory.ToArray();var document=LegacyCsg4Codec.Read(original);
   using var graph=new CandidateRailGraphSession(context,document,new(1e-7,1e-7,1e-7));graph.Generate(115,122);
   foreach(int index in new[]{115,122}){using var curve=graph.RetainCurve(index);RoundTrip(curve);}
   Check(LegacyCsg4Codec.Write(graph.Values.ExportCurrentDocument()).AsSpan().SequenceEqual(original),"Native export changed library document");
   graph.Values.SetValue(2,document.Records[2].Scalar!.Value+.25);graph.Generate(115,122);
   foreach(int index in new[]{115,122}){using var curve=graph.RetainCurve(index);RoundTrip(curve);}
   var saved=LegacyCsg4Codec.Read(LegacyCsg4Codec.Write(graph.Values.ExportCurrentDocument()));using var reopened=new CandidateRailGraphSession(context,saved,new(1e-7,1e-7,1e-7));reopened.Generate(115,122);
   foreach(int index in new[]{115,122}){using var curve=reopened.RetainCurve(index);RoundTrip(curve);}
  }
  Check(context.DiagnosticLiveShapeCount==0,"Export retained native entries");
  return "PASS native NURBS export:"+cases+" curve cases and"+samples+" native reimport samples; rational/nonclamped/affine/trim/reverse/seam/join, detached definitions, actual115/122 edited-save-reopen and ownership guards; plane/cap/PC parity separate.";
 }
}
