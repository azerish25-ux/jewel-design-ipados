using JewelDesign.Features.Candidate;
using JewelDesign.Geometry.Candidate;
namespace CandidateGeometryContractChecks;
internal static class ProfileCapPlacementAssertions
{
 static void Check(bool value,string message){if(!value)throw new Exception(message);}
 static void Near(CandidatePoint3 a,CandidatePoint3 b){Check(Math.Abs(a.X-b.X)<1e-8&&Math.Abs(a.Y-b.Y)<1e-8&&Math.Abs(a.Z-b.Z)<1e-8,"Cap placement point differs: "+a+" / "+b);}
 static void Throws<T>(Action action)where T:Exception{try{action();}catch(T){return;}throw new Exception("Expected "+typeof(T).Name);}
 public static string Run(){using var context=CandidateContext.Create();int faces=0,samples=0;
  var planes=new[]{CandidatePlane.WorldXY,new CandidatePlane(new(5,-4,3),new(0,1,0),new(0,0,1)),new CandidatePlane(new(-2,8,1),new(0,.6,.8),new(1,0,0))};
  foreach(var plane in planes)foreach(bool asymmetric in new[]{false,true})foreach(double amount in new[]{-.5,0.0,.5}){
   using var local=asymmetric?context.CreateNurbs(new(2,new[]{new CandidatePoint3(2,1,0),new(9,7,0),new(4,-2,0)},new[]{0.0,0,0,1,1,1},new[]{1.0,.4,1})):context.CreateCircle(2,CandidatePlane.WorldXY);
   var x=plane.XDirection;var z=plane.Normal;var y=new CandidateVector3(z.Y*x.Z-z.Z*x.Y,z.Z*x.X-z.X*x.Z,z.X*x.Y-z.Y*x.X);var origin=plane.Origin;
   CandidatePoint3 World(CandidatePoint3 p)=>new(origin.X+x.X*p.X+y.X*p.Y+z.X*p.Z,origin.Y+x.Y*p.X+y.Y*p.Y+z.Y*p.Z,origin.Z+x.Z*p.X+y.Z*p.Y+z.Z*p.Z);
   using var rail=context.TransformAffine(local,new(x.X,y.X,z.X,origin.X,x.Y,y.Y,z.Y,origin.Y,x.Z,y.Z,z.Z,origin.Z));var before=context.InspectCurve(rail);using var cap=ProfileCapWithPlaneCandidate.Build(context,rail,plane,amount,1e-8);var domain=context.InspectSurface(cap.Shape);
   for(int i=0;i<=32;i++){double t=i/32.0;Near(context.PointAtSurface(cap.Shape,domain.FirstU,domain.FirstV+(domain.LastV-domain.FirstV)*t),context.PointAt(rail,before.FirstParameter+(before.LastParameter-before.FirstParameter)*t));samples++;}
   var center=asymmetric?new CandidatePoint3(5.5,2.5,0):new CandidatePoint3(0,0,0);Near(context.PointAtSurface(cap.Shape,domain.LastU,(domain.FirstV+domain.LastV)*.5),World(new(center.X,center.Y,amount)));
   var m=cap.InverseTransform;Near(new(m.M14,m.M24,m.M34),World(center));Check(context.InspectCurve(rail)==before,"Cap build mutated rail");Check(context.Inspect(cap.Shape).IsValid,"Cap native face invalid");faces++;
  }
  using(var rail=context.CreateCircle(2,CandidatePlane.WorldXY)){
   ulong baseline=context.DiagnosticLiveShapeCount;
   Throws<NotSupportedException>(()=>ProfileCapWithPlaneCandidate.Build(context,rail,new(new(0,0,0),new(0,0,1),new(0,0,1))));
   Throws<CandidateGeometryException>(()=>ProfileCapWithPlaneCandidate.Build(context,rail,CandidatePlane.WorldXY,double.NaN));
   Throws<CandidateGeometryException>(()=>ProfileCapWithPlaneCandidate.Build(context,rail,CandidatePlane.WorldXY,.5,0));
   using(var other=CandidateContext.Create())Throws<ArgumentException>(()=>ProfileCapWithPlaneCandidate.Build(other,rail,CandidatePlane.WorldXY));
   Check(context.DiagnosticLiveShapeCount==baseline,"Failed cap creation leaked native entries");
  }
  using(var solid=context.CreateBox(1,1,1))Throws<CandidateGeometryException>(()=>ProfileCapWithPlaneCandidate.Build(context,solid,CandidatePlane.WorldXY));
  Check(context.DiagnosticLiveShapeCount==0,"Cap placement leaked native entries");return "PASS explicit-plane profile caps:"+faces+" native faces and"+samples+" boundary samples, signed/flat domes, translated/tilted frames, asymmetric control-hull center/apex and inverse metadata; automatic plane/graph/join/PC parity separate.";
 }
}
