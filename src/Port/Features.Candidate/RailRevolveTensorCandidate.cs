using JewelDesign.Geometry.Candidate;
namespace JewelDesign.Features.Candidate;
/// <summary>Recovered tensor inputs; Rhino5 setter convention is explicit, not inferred from modern Rhino APIs.</summary>
public static class RailRevolveTensorCandidate
{
 public static CandidateNurbsDefinition CreateDomeRhino5(double domeAmount,double radius=.5)
 {
  if(Math.Abs(domeAmount)<.005){double end=Math.Abs(radius);if(end<=2.3283064365386962890625e-10)end=1;return new(1,new[]{new CandidatePoint3(radius,0,0),new(0,0,0)},new[]{0.0,0,end,end});}
  // Original Rhino5 Point4d setter consumes Euclidean XYZ plus weight.
  return new(2,new[]{new CandidatePoint3(radius,0,0),new(radius,0,domeAmount),new(radius/2,0,domeAmount),new(radius/3,0,domeAmount),new(0,0,domeAmount)},new[]{0.0,0,0,1,1,2,2,2},new[]{1.0,.70710678,1,1,1});
 }
}
