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

 public static CandidateNurbsSurfaceDefinition Build(CandidateContext context,CandidateNurbsDefinition profile,CandidateNurbsDefinition rail,int direction,bool zAdjust=false,double? referenceRadius=null)
 {
  ArgumentNullException.ThrowIfNull(context);ArgumentNullException.ThrowIfNull(profile);ArgumentNullException.ThrowIfNull(rail);if(direction is not (0 or 1))throw new NotSupportedException("Rail-revolve tensor direction must be0 or1.");
  using var profileCurve=context.CreateNurbs(profile);using var railCurve=context.CreateNurbs(rail);var domain=context.InspectCurve(profileCurve);var start=context.PointAt(profileCurve,domain.FirstParameter);double endZ=context.PointAt(profileCurve,domain.LastParameter).Z,s1=referenceRadius??Math.Sqrt(start.X*start.X+start.Y*start.Y);
  int np=profile.Poles.Count,nr=rail.Poles.Count;if((long)np*nr>65536)throw new NotSupportedException("Recovered tensor exceeds native surface control limit.");
  int nu=direction==0?np:nr,nv=direction==0?nr:np;var points=new CandidatePoint3[checked(nu*nv)];var weights=new double[points.Length];
  for(int i=0;i<np;i++){
   var p=profile.Poles[i];double radius=Math.Sqrt(p.X*p.X+p.Y*p.Y),adjust=zAdjust?1.0-p.Z/endZ:0.0;if(double.IsNaN(adjust))adjust=0.0;
   for(int j=0;j<nr;j++){
    var r=rail.Poles[j];double x=r.X*radius/s1,y=r.Y*radius/s1,z=p.Z+adjust*r.Z,w=(profile.PoleWeights?[i]??1)*(rail.PoleWeights?[j]??1);int k=direction==0?i*nv+j:j*nv+i;
    // Source SetCV writes homogeneous coordinates; the native import takes explicit Euclidean coordinates.
    points[k]=new(x*w/w,y*w/w,z*w/w);weights[k]=w;
   }
  }
  return new(direction==0?profile.Degree:rail.Degree,direction==0?rail.Degree:profile.Degree,(uint)nu,(uint)nv,points,direction==0?profile.KnotVector:rail.KnotVector,direction==0?rail.KnotVector:profile.KnotVector,weights);
 }
}
