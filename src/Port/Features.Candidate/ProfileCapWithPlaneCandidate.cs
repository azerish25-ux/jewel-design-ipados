using JewelDesign.Documents;
using JewelDesign.Geometry.Candidate;
namespace JewelDesign.Features.Candidate;
/// <summary>Owns already placed surface geometry and its source inverse metadata. Do not apply metadata again.</summary>
public sealed class ProfileCapWithPlaneResult:IDisposable
{
 public CandidateShape Shape {get;}
 public LegacyAffineTransform InverseTransform {get;}
 internal ProfileCapWithPlaneResult(CandidateShape shape,LegacyAffineTransform inverseTransform){Shape=shape;InverseTransform=inverseTransform;}
 public void Dispose()=>Shape.Dispose();
}
/// <summary>Recovered cap construction after plane selection. Automatic AveragePlaneOfCurve is a separate prerequisite.</summary>
public static class ProfileCapWithPlaneCandidate
{
 private static CandidateAffine Native(LegacyAffineTransform m)=>new(m.M11,m.M12,m.M13,m.M14,m.M21,m.M22,m.M23,m.M24,m.M31,m.M32,m.M33,m.M34);
 private static double Dot(CandidateVector3 a,CandidateVector3 b)=>a.X*b.X+a.Y*b.Y+a.Z*b.Z;
 private static LegacyAffineTransform Inverse(LegacyAffineTransform m)
 {
  double a=m.M11,b=m.M12,c=m.M13,d=m.M21,e=m.M22,f=m.M23,g=m.M31,h=m.M32,i=m.M33;
  double determinant=a*(e*i-f*h)-b*(d*i-f*g)+c*(d*h-e*g);
  if(!double.IsFinite(determinant)||Math.Abs(determinant)<1e-12)throw new NotSupportedException("Cap plane transform is singular.");
  double aa=(e*i-f*h)/determinant,bb=(c*h-b*i)/determinant,cc=(b*f-c*e)/determinant;
  double dd=(f*g-d*i)/determinant,ee=(a*i-c*g)/determinant,ff=(c*d-a*f)/determinant;
  double gg=(d*h-e*g)/determinant,hh=(b*g-a*h)/determinant,ii=(a*e-b*d)/determinant;
  return new(aa,bb,cc,-(aa*m.M14+bb*m.M24+cc*m.M34),dd,ee,ff,-(dd*m.M14+ee*m.M24+ff*m.M34),gg,hh,ii,-(gg*m.M14+hh*m.M24+ii*m.M34));
 }
 public static ProfileCapWithPlaneResult Build(CandidateContext context,CandidateShape curve,CandidatePlane plane,double domeAmount=.5,double tolerance=1e-8)
 {
  ArgumentNullException.ThrowIfNull(context);ArgumentNullException.ThrowIfNull(curve);
  var x=plane.XDirection;var z=plane.Normal;var p=plane.Origin;
  if(!double.IsFinite(Dot(x,x))||!double.IsFinite(Dot(z,z))||Math.Abs(Dot(x,x)-1)>1e-10||Math.Abs(Dot(z,z)-1)>1e-10||Math.Abs(Dot(x,z))>1e-10)throw new NotSupportedException("Cap requires a finite orthonormal selected plane.");
  var y=new CandidateVector3(z.Y*x.Z-z.Z*x.Y,z.Z*x.X-z.X*x.Z,z.X*x.Y-z.Y*x.X);
  var basis=new LegacyAffineTransform(x.X,x.Y,x.Z,-(x.X*p.X+x.Y*p.Y+x.Z*p.Z),y.X,y.Y,y.Z,-(y.X*p.X+y.Y*p.Y+y.Z*p.Z),z.X,z.Y,z.Z,-(z.X*p.X+z.Y*p.Y+z.Z*p.Z));
  using var inPlane=context.TransformAffine(curve,Native(basis));var controls=context.ExportNurbs(inPlane).Poles;
  // Source fast NURBS GetBBox uses Euclidean control-list bounds, not tight curve extrema.
  double cx=(controls.Min(q=>q.X)+controls.Max(q=>q.X))*.5,cy=(controls.Min(q=>q.Y)+controls.Max(q=>q.Y))*.5;
  var center=LegacyAffineTransform.Translation(-cx,-cy,0);using var centered=context.TransformAffine(inPlane,Native(center));
  var inverse=Inverse(center*basis);var rail=context.ExportNurbs(centered);
  var tensor=RailRevolveTensorCandidate.Build(context,RailRevolveTensorCandidate.CreateDomeRhino5(domeAmount),rail,0,zAdjust:true);
  // Source transforms the NURBS surface controls before ToBrep, not an already built face.
  var points=tensor.Poles.Select((q,index)=>{double w=tensor.PoleWeights?[index]??1,x=q.X*w,y=q.Y*w,z=q.Z*w;return new CandidatePoint3((inverse.M11*x+inverse.M12*y+inverse.M13*z+inverse.M14*w)/w,(inverse.M21*x+inverse.M22*y+inverse.M23*z+inverse.M24*w)/w,(inverse.M31*x+inverse.M32*y+inverse.M33*z+inverse.M34*w)/w);}).ToArray();
  var worldTensor=new CandidateNurbsSurfaceDefinition(tensor.DegreeU,tensor.DegreeV,tensor.CountU,tensor.CountV,points,tensor.KnotVectorU,tensor.KnotVectorV,tensor.PoleWeights);
  var placed=context.CreateNurbsSurface(worldTensor,tolerance);
  try{return new(placed,inverse);}catch{placed.Dispose();throw;}
 }
}
