using JewelDesign.Documents;
using JewelDesign.Features.Candidate;
using JewelDesign.Geometry.Candidate;
namespace CandidateGeometryContractChecks;
internal static class PairedSweepAssertions
{
 private static void Check(bool value,string message){if(!value)throw new Exception(message);}
 private static void Near(CandidatePoint3 a,CandidatePoint3 b,double tolerance=1e-6){if(Math.Abs(a.X-b.X)>tolerance||Math.Abs(a.Y-b.Y)>tolerance||Math.Abs(a.Z-b.Z)>tolerance)throw new Exception("Paired native surface contact mismatch actual="+a+" expected="+b);}
 private static void Throws<T>(Action action)where T:Exception{try{action();}catch(T){return;}throw new Exception("Expected "+typeof(T).Name);}
 public static string Run(){
  using var stream=typeof(PairedSweepAssertions).Assembly.GetManifestResourceStream("RingRailLibraryFixture")!;using var memory=new MemoryStream();stream.CopyTo(memory);var original=memory.ToArray();var document=LegacyCsg4Codec.Read(original);
  using var context=CandidateContext.Create();CandidateShape retained;int contacts=0;
  using(var graph=new CandidateRailGraphSession(context,document,new(1e-7,1e-7,1e-7)))
  using(var prepared=Sweep2PreparationCandidate.FromNode(context,graph,134)){
   Check(!prepared.EffectiveClosed&&prepared.MaintainHeight,"Original sweep flag premise changed");
   using var face=context.CreatePairedSweep(prepared.Rail1,prepared.Rail2,prepared.Profiles,prepared.Parameters1,prepared.Parameters2,prepared.MaintainHeight,1e-7);
   Check(context.Inspect(face).IsValid,"Original prepared inputs produced invalid native face");
   var domain=context.InspectSurface(face);
   double U(double fraction)=>domain.FirstU+(domain.LastU-domain.FirstU)*fraction;
   double V(double parameter)=>domain.FirstV+(domain.LastV-domain.FirstV)*(parameter-prepared.Parameters1[0])/(prepared.Parameters1[^1]-prepared.Parameters1[0]);
   for(int i=0;i<3;i++){
    var curve=context.InspectCurve(prepared.Profiles[i]);Check(context.IsCurveClosed(prepared.Profiles[i]),"Actual stored profile unexpectedly open");
    for(int j=0;j<=32;j++){double f=j/32.0;var actual=context.PointAtSurface(face,U(f),V(prepared.Parameters1[i]));var expected=context.PointAt(prepared.Profiles[i],curve.FirstParameter+(curve.LastParameter-curve.FirstParameter)*f);Near(actual,expected);contacts++;}
   }
   for(int j=0;j<=100;j++){
    double t=prepared.Parameters1[0]+(prepared.Parameters1[^1]-prepared.Parameters1[0])*j/100.0;
    var inner=context.PointAt(prepared.Rail1,t);var outer=context.PointAtSurface(face,U(.5),V(t));Near(context.PointAtSurface(face,U(0),V(t)),inner);Check(context.ClosestPoint(prepared.Rail2,outer).Distance<=1e-6,"Native surface leaves the secondary rail");var tangent=context.TangentAt(prepared.Rail1,t);Check(Math.Abs(tangent.X*(outer.X-inner.X)+tangent.Y*(outer.Y-inner.Y)+tangent.Z*(outer.Z-inner.Z))<=1e-6,"Original radial section plane lost");contacts+=2;
   }
   var point=context.PointAtSurface(face,U(.2),V(prepared.Parameters1[1]));using var moved=context.Translate(face,1,2,3);Near(context.PointAtSurface(moved,U(.2),V(prepared.Parameters1[1])),new(point.X+1,point.Y+2,point.Z+3));
   ulong count=context.DiagnosticLiveShapeCount;
   Throws<ArgumentException>(()=>context.CreatePairedSweep(prepared.Rail1,prepared.Rail2,prepared.Profiles,new[]{1.0},prepared.Parameters2,true,1e-7));
   var invalid=prepared.Parameters1.ToArray();invalid[1]=double.NaN;Throws<CandidateGeometryException>(()=>context.CreatePairedSweep(prepared.Rail1,prepared.Rail2,prepared.Profiles,invalid,prepared.Parameters2,true,1e-7));
   using(var foreign=CandidateContext.Create()){using var foreignRail=foreign.CreateLine(new(0,0,0),new(1,0,0));Throws<ArgumentException>(()=>context.CreatePairedSweep(foreignRail,prepared.Rail2,prepared.Profiles,prepared.Parameters1,prepared.Parameters2,true,1e-7));}
   using(var disposed=context.Clone(prepared.Rail1)){disposed.Dispose();Throws<ObjectDisposedException>(()=>context.CreatePairedSweep(disposed,prepared.Rail2,prepared.Profiles,prepared.Parameters1,prepared.Parameters2,true,1e-7));}
   using var solid=context.CreateBox(1,1,1);Throws<CandidateGeometryException>(()=>context.CreatePairedSweep(solid,prepared.Rail2,prepared.Profiles,prepared.Parameters1,prepared.Parameters2,true,1e-7));
   Throws<CandidateGeometryException>(()=>context.InspectSurface(solid));Throws<CandidateGeometryException>(()=>context.PointAtSurface(face,double.NaN,0));
   Check(context.DiagnosticLiveShapeCount==count+1,"Failed paired native calls leaked entries");
   retained=context.Retain(face);
   Check(LegacyCsg4Codec.Write(graph.Values.ExportCurrentDocument()).AsSpan().SequenceEqual(original),"Native sweep changed serialized input");
   graph.Values.SetValue(2,document.Records[2].Scalar!.Value+.25);
   using var edited=Sweep2PreparationCandidate.FromNode(context,graph,134);
   using var editedFace=context.CreatePairedSweep(edited.Rail1,edited.Rail2,edited.Profiles,edited.Parameters1,edited.Parameters2,edited.MaintainHeight,1e-7);
   var saved=LegacyCsg4Codec.Read(LegacyCsg4Codec.Write(graph.Values.ExportCurrentDocument()));
   using var reopened=new CandidateRailGraphSession(context,saved,new(1e-7,1e-7,1e-7));using var loaded=Sweep2PreparationCandidate.FromNode(context,reopened,134);
   using var loadedFace=context.CreatePairedSweep(loaded.Rail1,loaded.Rail2,loaded.Profiles,loaded.Parameters1,loaded.Parameters2,loaded.MaintainHeight,1e-7);
   var d1=context.InspectSurface(editedFace);var d2=context.InspectSurface(loadedFace);
   for(int i=0;i<=8;i++){double u=i/8.0;Near(context.PointAtSurface(editedFace,d1.FirstU+(d1.LastU-d1.FirstU)*u,(d1.FirstV+d1.LastV)*.5),context.PointAtSurface(loadedFace,d2.FirstU+(d2.LastU-d2.FirstU)*u,(d2.FirstV+d2.LastV)*.5));}
   Near(context.PointAtSurface(face,U(.2),V(prepared.Parameters1[1])),point);
   graph.Values.SetValue(129,1);Throws<NotSupportedException>(()=>graph.Generate(134)); // Closed source policy remains unsupported.
  }
  Check(context.DiagnosticLiveShapeCount==1,"Output did not survive prepared input/graph disposal");context.InspectSurface(retained);retained.Dispose();retained.Dispose();Check(context.DiagnosticLiveShapeCount==0,"Paired output cleanup leaked");
  return "PASS managed paired-rail surface:"+contacts+" actual134 rail/profile samples, native BRep/affine/retain/input isolation/edit-save-reopen/error cleanup; experimental explicit map, no original-PC/full sweep-block parity.";
 }
}
