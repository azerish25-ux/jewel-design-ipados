using Fallback=CandidateGeometryContractChecks.Recovered.AveragePlane;
using JewelDesign.Documents;
using JewelDesign.Features.Candidate;
using JewelDesign.Geometry.Candidate;
namespace CandidateGeometryContractChecks;
internal static class CurvePlaneAssertions
{
 static void Check(bool v,string message){if(!v)throw new Exception(message);}
 static double Dot(CandidateVector3 a,CandidateVector3 b)=>a.X*b.X+a.Y*b.Y+a.Z*b.Z;
 public static string Run(){using var context=CandidateContext.Create();int planes=0,boundary=0;
  foreach(var frame in new[]{CandidatePlane.WorldXY,CandidatePlane.WorldZX,new CandidatePlane(new(5,-4,3),new(0,.6,.8),new(1,0,0))}){
   using var circle=context.CreateCircle(2,frame);foreach(bool reverse in new[]{false,true}){using var curve=reverse?context.ReverseCurve(circle):context.Clone(circle);Check(CurvePlaneCandidate.TryPlanar(context,curve,out var plane),"Circle planar test failed");Check(Dot(plane.Normal,frame.Normal)*(reverse?-1:1)>1-1e-10,"Circle normal orientation changed");var info=context.InspectCurve(curve);Check(Dot(plane.XDirection,context.TangentAt(curve,info.FirstParameter))>1-1e-10,"Plane X axis lost initial tangent");planes++;}
  }
  foreach(var end in new[]{new CandidatePoint3(1,0,0),new(0,1,0),new(0,0,1),new(1,2,3)}){using var line=context.CreateNurbs(new(1,new[]{new CandidatePoint3(0,0,0),end},new[]{0.0,0,1,1}));Check(CurvePlaneCandidate.TryPlanar(context,line,out _),"Line plane failed");planes++;}
  foreach(double height in new[]{-.5,-.1,.1,.5})using(var curve=context.CreateNurbs(new(1,new[]{new CandidatePoint3(1,0,0),new(1,1,height),new(0,1,height*.5),new(-1,1,-height*.5),new(-1,0,0)},new[]{0.0,0,1,2,3,4,4}))){Check(!CurvePlaneCandidate.TryPlanar(context,curve,out _),"Nonplanar curve accepted");Check(CurvePlaneCandidate.TryAveragePlane(context,curve,out var fallback),"Recovered fallback failed");Check(Fallback.SourceAverage.AveragePlaneOfCurve(new(context,curve),out var source),"Source fallback failed");Check(Math.Abs(fallback.Normal.X-source.Normal.X)<1e-12&&Math.Abs(fallback.Normal.Y-source.Normal.Y)<1e-12&&Math.Abs(fallback.Normal.Z-source.Normal.Z)<1e-12,"Fallback normal differs from source body");planes++;}
  using(var stream=typeof(CurvePlaneAssertions).Assembly.GetManifestResourceStream("RingRailLibraryFixture")!)using(var memory=new MemoryStream()){
   stream.CopyTo(memory);var document=LegacyCsg4Codec.Read(memory.ToArray());using var graph=new CandidateRailGraphSession(context,document,new(1e-7,1e-7,1e-7));
   void Run(CandidateRailGraphSession g){g.Generate(115,122);foreach(int index in new[]{115,122}){using var curve=g.RetainCurve(index);Check(CurvePlaneCandidate.TryAveragePlane(context,curve,out var plane),"Generated cap profile plane missing");using var cap=ProfileCapWithPlaneCandidate.Build(context,curve,plane,index==115?.5:-.5);var d=context.InspectCurve(curve);var s=context.InspectSurface(cap.Shape);for(int i=0;i<=32;i++){double t=i/32.0;var a=context.PointAt(curve,d.FirstParameter+(d.LastParameter-d.FirstParameter)*t);var b=context.PointAtSurface(cap.Shape,s.FirstU,s.FirstV+(s.LastV-s.FirstV)*t);Check(Math.Abs(a.X-b.X)<1e-8&&Math.Abs(a.Y-b.Y)<1e-8&&Math.Abs(a.Z-b.Z)<1e-8,"Actual cap profile boundary lost");boundary++;}planes++;}}
   Run(graph);graph.Values.SetValue(2,document.Records[2].Scalar!.Value+.25);Run(graph);using var reopened=new CandidateRailGraphSession(context,LegacyCsg4Codec.Read(LegacyCsg4Codec.Write(graph.Values.ExportCurrentDocument())),new(1e-7,1e-7,1e-7));Run(reopened);
  }
  Check(context.DiagnosticLiveShapeCount==0,"Plane/cap cases leaked native entries");return "PASS automatic cap plane candidate:"+planes+" oriented/linear/fallback/generated cases and"+boundary+" actual-profile cap boundary points; published6 policy, original-PC/source differential/full graph parity separate.";
 }
}
