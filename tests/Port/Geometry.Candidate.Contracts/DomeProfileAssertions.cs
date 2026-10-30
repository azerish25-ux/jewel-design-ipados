using JewelDesign.Features.Candidate;
using JewelDesign.Geometry.Candidate;
using Oracle=CandidateGeometryContractChecks.Recovered.DomeProfile;
using TensorOracle=CandidateGeometryContractChecks.Recovered.RailRevolve;
namespace CandidateGeometryContractChecks;
internal static class DomeProfileAssertions
{
 public static string Run(){using var context=CandidateContext.Create();int count=0;foreach(double amount in new[]{-.5,-.005,-.004999,0.0,.004999,.005,.5})foreach(double radius in new[]{.25,.5,2.0}){
  var definition=RailRevolveTensorCandidate.CreateDomeRhino5(amount,radius);using var curve=context.CreateNurbs(definition);var info=context.InspectCurve(curve);using var reference=context.CreateNurbs(Oracle.SourceDome.CreateDomeRC(amount,radius).Definition());var referenceInfo=context.InspectCurve(reference);if(info!=referenceInfo)throw new Exception("Recovered dome definition dimensions differ");for(int i=0;i<=32;i++){double t=info.FirstParameter+(info.LastParameter-info.FirstParameter)*i/32;var a=context.PointAt(curve,t);var b=context.PointAt(reference,t);if(Math.Abs(a.X-b.X)>1e-12||Math.Abs(a.Y-b.Y)>1e-12||Math.Abs(a.Z-b.Z)>1e-12)throw new Exception("Recovered dome source profile differs");}
  if(info.PoleCount!=(Math.Abs(amount)<.005?2u:5u)||info.Degree!=(Math.Abs(amount)<.005?1u:2u))throw new Exception("Source dome threshold/shape mismatch");
  if(Math.Abs(amount)<.005&&Math.Abs(info.LastParameter-Math.Abs(radius))>1e-12)throw new Exception("Flat dome must preserve length-based line parameter domain");
  var start=context.PointAt(curve,info.FirstParameter);var end=context.PointAt(curve,info.LastParameter);if(Math.Abs(start.X-radius)>1e-12||Math.Abs(start.Z)>1e-12||Math.Abs(end.X)>1e-12||Math.Abs(end.Z-(Math.Abs(amount)<.005?0:amount))>1e-12)throw new Exception("Source dome endpoints changed");count++;
 }
  var rail=new CandidateNurbsDefinition(1,new[]{new CandidatePoint3(2,0,.2),new(0,2,.4),new(-2,0,.2),new(0,-2,0),new(2,0,.2)},new[]{0.0,0,.25,.5,.75,1,1});using var railCurve=context.CreateNurbs(rail);int tensorPoints=0;
  foreach(double amount in new[]{-.5,0.0,.5})foreach(int direction in new[]{0,1})foreach(bool adjust in new[]{false,true}){var definition=RailRevolveTensorCandidate.Build(context,RailRevolveTensorCandidate.CreateDomeRhino5(amount),rail,direction,adjust);using var face=context.CreateNurbsSurface(definition,1e-8);var profile=RailRevolveTensorCandidate.CreateDomeRhino5(amount);using var profileCurve=context.CreateNurbs(profile);var pi=context.InspectCurve(profileCurve);double ez=context.PointAt(profileCurve,pi.LastParameter).Z;var source=TensorOracle.SourceTensor.Build(new(profile,ez),new(rail,0),.5,direction,adjust);if(!definition.Poles.SequenceEqual(source.Poles)||!definition.PoleWeights!.SequenceEqual(source.PoleWeights!)||!definition.KnotVectorU.SequenceEqual(source.KnotVectorU)||!definition.KnotVectorV.SequenceEqual(source.KnotVectorV))throw new Exception("Recovered tensor grid coefficients differ");var bounds=context.InspectSurface(face);for(int i=0;i<=16;i++){double t=i/16.0;double u=direction==0?bounds.FirstU:bounds.FirstU+(bounds.LastU-bounds.FirstU)*t,v=direction==0?bounds.FirstV+(bounds.LastV-bounds.FirstV)*t:bounds.FirstV;var actual=context.PointAtSurface(face,u,v);var expected=context.PointAt(railCurve,t);if(!adjust||Math.Abs(amount)<.005)expected=new(expected.X,expected.Y,0);if(Math.Abs(actual.X-expected.X)>1e-8||Math.Abs(actual.Y-expected.Y)>1e-8||Math.Abs(actual.Z-expected.Z)>1e-8)throw new Exception("Revolved cap boundary differs: amount="+amount+" direction="+direction+" adjust="+adjust+" actual="+actual+" expected="+expected);tensorPoints++;}}

  var weightedRail=new CandidateNurbsDefinition(2,new[]{new CandidatePoint3(2,0,.2),new(2,2,.4),new(0,2,.1)},new[]{-2.0,-2,-2,3,3,3},new[]{1.0,.70710678,1.2});int extraGrids=0;
  foreach(int direction in new[]{0,1})foreach(bool adjust in new[]{false,true})foreach(double referenceRadius in new[]{.25,1.0}){
   var profile=RailRevolveTensorCandidate.CreateDomeRhino5(-.3);var actual=RailRevolveTensorCandidate.Build(context,profile,weightedRail,direction,adjust,referenceRadius);var source=TensorOracle.SourceTensor.Build(new(profile,-.3),new(weightedRail,0),referenceRadius,direction,adjust);
   if(!actual.Poles.SequenceEqual(source.Poles)||!actual.PoleWeights!.SequenceEqual(source.PoleWeights!)||!actual.KnotVectorU.SequenceEqual(source.KnotVectorU)||!actual.KnotVectorV.SequenceEqual(source.KnotVectorV))throw new Exception("Weighted explicit-radius tensor differs from source");
   using var face=context.CreateNurbsSurface(actual,1e-8);if(!context.Inspect(face).IsValid)throw new Exception("Weighted tensor face invalid");extraGrids++;
  }
  var baseline=context.DiagnosticLiveShapeCount;
  try{RailRevolveTensorCandidate.Build(context,RailRevolveTensorCandidate.CreateDomeRhino5(.5),rail,2);throw new Exception("Invalid tensor direction accepted");}catch(NotSupportedException){}
  var singular=RailRevolveTensorCandidate.Build(context,RailRevolveTensorCandidate.CreateDomeRhino5(.5),rail,0,referenceRadius:0);
  try{using var invalid=context.CreateNurbsSurface(singular,1e-8);throw new Exception("Nonfinite zero-radius tensor accepted");}catch(CandidateGeometryException){}
  if(context.DiagnosticLiveShapeCount!=baseline)throw new Exception("Tensor build/failure leaked native entries");
  return "PASS recovered Rhino5 dome:"+count+" threshold/sign/radius profiles and693 exact-source native point comparisons; tensor-boundary="+tensorPoints+"; tensor-grid="+(12+extraGrids)+"; full cap/plane/PC parity separate.";}
}
