using JewelDesign.Features.Candidate;
using JewelDesign.Geometry.Candidate;
using Oracle=CandidateGeometryContractChecks.Recovered.DomeProfile;
namespace CandidateGeometryContractChecks;
internal static class DomeProfileAssertions
{
 public static string Run(){using var context=CandidateContext.Create();int count=0;foreach(double amount in new[]{-.5,-.005,-.004999,0.0,.004999,.005,.5})foreach(double radius in new[]{.25,.5,2.0}){
  var definition=RailRevolveTensorCandidate.CreateDomeRhino5(amount,radius);using var curve=context.CreateNurbs(definition);var info=context.InspectCurve(curve);using var reference=context.CreateNurbs(Oracle.SourceDome.CreateDomeRC(amount,radius).Definition());var referenceInfo=context.InspectCurve(reference);if(info!=referenceInfo)throw new Exception("Recovered dome definition dimensions differ");for(int i=0;i<=32;i++){double t=info.FirstParameter+(info.LastParameter-info.FirstParameter)*i/32;var a=context.PointAt(curve,t);var b=context.PointAt(reference,t);if(Math.Abs(a.X-b.X)>1e-12||Math.Abs(a.Y-b.Y)>1e-12||Math.Abs(a.Z-b.Z)>1e-12)throw new Exception("Recovered dome source profile differs");}
  if(info.PoleCount!=(Math.Abs(amount)<.005?2u:5u)||info.Degree!=(Math.Abs(amount)<.005?1u:2u))throw new Exception("Source dome threshold/shape mismatch");
  if(Math.Abs(amount)<.005&&Math.Abs(info.LastParameter-Math.Abs(radius))>1e-12)throw new Exception("Flat dome must preserve length-based line parameter domain");
  var start=context.PointAt(curve,info.FirstParameter);var end=context.PointAt(curve,info.LastParameter);if(Math.Abs(start.X-radius)>1e-12||Math.Abs(start.Z)>1e-12||Math.Abs(end.X)>1e-12||Math.Abs(end.Z-(Math.Abs(amount)<.005?0:amount))>1e-12)throw new Exception("Source dome endpoints changed");count++;
 }return "PASS recovered Rhino5 dome:"+count+" threshold/sign/radius profiles and693 exact-source native point comparisons; full cap/plane/PC parity separate.";}
}
