using JewelDesign.Geometry.Candidate;
namespace JewelDesign.Rendering.Candidate;

/// <summary>Original OGL trace-image extents and PointAt corner order. Input pixels are already
/// prepared; this does not resolve files, gray conversion, original view camera or cache state.</summary>
public sealed class Rhino5TracePlane
{
 private readonly CandidatePoint3[] corners;
 public double Width{get;}public double Height{get;}
 private Rhino5TracePlane(double w,double h,CandidatePoint3[] points){Width=w;Height=h;corners=points;}
 public CandidatePoint3[] CopyWorldCorners()=>(CandidatePoint3[])corners.Clone();
 public static Rhino5TracePlane? Create(Rhino5TraceImage trace,int textureWidth,int textureHeight)
 {
  if(trace.Hidden||trace.Width==0&&trace.Height==0)return null;
  ArgumentOutOfRangeException.ThrowIfNegativeOrZero(textureWidth);ArgumentOutOfRangeException.ThrowIfNegativeOrZero(textureHeight);
  double w=trace.Width,h=trace.Height;
  if(!double.IsFinite(w)||!double.IsFinite(h))throw new ArgumentException("Nonfinite trace extents are outside the managed draw domain.");
  double aspect=(double)textureWidth/textureHeight;
  if(w==0)w=aspect*h;else if(h==0)h=(1d/aspect)*w;
  var p=trace.Plane;
  CandidatePoint3 Point(double s,double t)=>new((s*p.X.X+p.Origin.X)+t*p.Y.X,(s*p.X.Y+p.Origin.Y)+t*p.Y.Y,(s*p.X.Z+p.Origin.Z)+t*p.Y.Z);
  CandidatePoint3[] points=[Point(w,0),Point(w,h),Point(0,h),Point(0,0)];
  if(points.Any(q=>!double.IsFinite(q.X)||!double.IsFinite(q.Y)||!double.IsFinite(q.Z)))throw new ArgumentException("Trace point overflow/nonfinite plane.");
  return new(w,h,points);
 }
 public float[] CopyNormalizedVertices(CandidatePoint3 center,double radius)
 {
  if(!double.IsFinite(radius)||radius<=0)throw new ArgumentOutOfRangeException(nameof(radius));
  float[] packed=new float[16];
  for(int i=0;i<4;i++){packed[i*4]=(float)((corners[i].X-center.X)/radius);packed[i*4+1]=(float)((corners[i].Y-center.Y)/radius);packed[i*4+2]=(float)((corners[i].Z-center.Z)/radius);packed[i*4+3]=1;}
  if(packed.Any(v=>!float.IsFinite(v)))throw new ArgumentException("Normalized trace coordinates are not representable.");
  return packed;
 }
}
