using JewelDesign.Geometry.Candidate;
namespace JewelDesign.Features.Candidate;
/// <summary>Published openNURBS6 NURBS plane policy plus recovered average-plane fallback. Original Rhino5 equivalence remains unverified.</summary>
public static class CurvePlaneCandidate
{
 private readonly record struct V(double X,double Y,double Z){
  public static V operator+(V a,V b)=>new(a.X+b.X,a.Y+b.Y,a.Z+b.Z);
  public static V operator-(V a,V b)=>new(a.X-b.X,a.Y-b.Y,a.Z-b.Z);
  public static V operator*(double a,V b)=>new(a*b.X,a*b.Y,a*b.Z);
  public static V operator/(V a,double b)=>new(a.X/b,a.Y/b,a.Z/b);
 }
 private static V P(CandidatePoint3 p)=>new(p.X,p.Y,p.Z);
 private static V D(CandidateVector3 v)=>new(v.X,v.Y,v.Z);
 private static double Dot(V a,V b)=>a.X*b.X+a.Y*b.Y+a.Z*b.Z;
 private static V Cross(V a,V b)=>new(a.Y*b.Z-a.Z*b.Y,a.Z*b.X-a.X*b.Z,a.X*b.Y-a.Y*b.X);
 private const double MinimumNormal=2.2250738585072014e-308;
 private static double Length(V a){
  double x=Math.Abs(a.X),y=Math.Abs(a.Y),z=Math.Abs(a.Z),swap;
  if(y>=x&&y>=z){swap=x;x=y;y=swap;}else if(z>=x&&z>=y){swap=x;x=z;z=swap;}
  if(x>MinimumNormal){y/=x;z/=x;return x*Math.Sqrt(1+y*y+z*z);}return x>0&&double.IsFinite(x)?x:0;
 }
 private static V Unit(V a){
  double length=Length(a);if(double.IsFinite(length)){
   if(length>MinimumNormal)return a/length;
   if(length>0){var scaled=8.9884656743115795386465259539451e307*a;length=Length(scaled);if(double.IsFinite(length)&&length>MinimumNormal)return scaled/length;}
  }
  return default;
 }
 private static double Length2(double x,double y){x=Math.Abs(x);y=Math.Abs(y);if(y>x){double swap=x;x=y;y=swap;}if(x>MinimumNormal){y/=x;return x*Math.Sqrt(1+y*y);}return x>0&&double.IsFinite(x)?x:0;}
 private static CandidatePlane Plane(V p,V n,V x)=>new(new(p.X,p.Y,p.Z),new(n.X,n.Y,n.Z),new(x.X,x.Y,x.Z));
 // Published ON_3dVector::PerpendicularTo tie ordering is significant for fallback orientation.
 private static V Perpendicular(V v){
  if(Math.Abs(v.Y)>Math.Abs(v.X)){
   if(Math.Abs(v.Z)>Math.Abs(v.Y))return new(0,v.Z,-v.Y);
   if(Math.Abs(v.Z)>=Math.Abs(v.X))return new(0,-v.Z,v.Y);
   return new(v.Y,-v.X,0);
  }
  if(Math.Abs(v.Z)>Math.Abs(v.X))return new(v.Z,0,-v.X);
  if(Math.Abs(v.Z)>Math.Abs(v.Y))return new(-v.Z,0,v.X);
  return new(-v.Y,v.X,0);
 }
 private static bool Linear(CandidateNurbsDefinition curve,double tolerance){
  var k=curve.KnotVector;int degree=(int)curve.Degree;for(int i=1;i<=degree;i++)if(k[i]!=k[0]||k[^(i+1)]!=k[^1])return false;
  var points=curve.Poles;var from=P(points[0]);var to=P(points[^1]);var d=to-from;double squared=Dot(d,d);if(!double.IsFinite(squared)||!(squared>0))return false;if(points.Count==2)return true;d=d/squared;double previous=0;
  for(int i=1;i<points.Count-1;i++){
   var p=P(points[i]);var a=p-from;var b=p-to;double t=Dot(a,a)<=Dot(b,b)?Dot(a,d):1+Dot(b,d);
   if((Dot(a,a)<=Dot(b,b)&&t<-.01)||(Dot(a,a)>Dot(b,b)&&t>1.01))return false;
   var q=(1-t)*from+t*to;var difference=p-q;double remaining=tolerance*tolerance-difference.X*difference.X;if(remaining<0)return false;remaining-=difference.Y*difference.Y;if(remaining<0)return false;remaining-=difference.Z*difference.Z;if(remaining<0)return false;
   if(t>previous&&previous<1)previous=t<1?t:1;
   if(!(t>=previous&&t<=1)&&!(Length(q-((1-previous)*from+previous*to))<=tolerance))return false;
  }
  return true;
 }
 private static CandidatePlane LinePlane(V from,V to,double tolerance){
  var v=to-from;bool tinyX=Math.Abs(v.X)<=tolerance,tinyY=Math.Abs(v.Y)<=tolerance,tinyZ=Math.Abs(v.Z)<=tolerance;V x,y;
  if(tinyZ&&(!tinyX||!tinyY)){x=new(1,0,0);y=new(0,1,0);}
  else if(tinyX&&(!tinyY||!tinyZ)){x=new(0,1,0);y=new(0,0,1);}
  else if(tinyY&&(!tinyZ||!tinyX)){x=new(0,0,1);y=new(1,0,0);}
  else{x=Unit(v);y=Perpendicular(x);if(tinyX&&tinyY&&tinyZ&&Length(x)==0){x=new(1,0,0);y=new(0,1,0);}}
  x=Unit(x);y=Unit(y-Dot(y,x)*x);return Plane(from,Unit(Cross(x,y)),x);
 }
 private static bool Triangle(V p,V q,V r,out V n,out V x,out V y){
  n=x=y=default;var v0=r-q;var v1=p-r;var v2=q-p;var n0=Unit(Cross(v1,v2));var n1=Unit(Cross(v2,v0));var n2=Unit(Cross(v0,v1));if(Length(n0)==0||Length(n1)==0||Length(n2)==0)return false;
  double Error(V normal)=>(1.0/Length(v0))*Math.Abs(Dot(normal,v0))+(1.0/Length(v1))*Math.Abs(Dot(normal,v1))+(1.0/Length(v2))*Math.Abs(Dot(normal,v2));
  double e0=Error(n0),e1=Error(n1),e2=Error(n2);n=e0<=e1?(e0<=e2?n0:n2):(e1<=e2?n1:n2);x=Unit(q-p);y=Unit(Cross(n,x));return true;
 }
 public static bool TryPlanar(CandidateContext context,CandidateShape curve,out CandidatePlane plane){
  ArgumentNullException.ThrowIfNull(context);var definition=context.ExportNurbs(curve);var domain=context.InspectCurve(curve);var p=P(context.PointAt(curve,domain.FirstParameter));plane=default;
  if(Linear(definition,1e-12)){
   var to=P(context.PointAt(curve,domain.LastParameter));plane=LinePlane(p,to,1e-12);var v=to-p;
   if(Math.Abs(v.X)<=1e-12&&Math.Abs(v.Y)<=1e-12&&Math.Abs(v.Z)<=1e-12)plane=LinePlane(p,to,0);return true;
  }
  int count=definition.Poles.Count,step=Math.Max(1,count/64);V q=p,r=p;double largest=0;
  for(int i=1;i<count;i+=step)for(int j=i+step;j<count;j+=step){var a=P(definition.Poles[i]);var b=P(definition.Poles[j]);double area=Length(Cross(a-p,b-p));if(area>largest){largest=area;q=a;r=b;}}
  if(!Triangle(p,q,r,out var n,out var x,out var y))return false;
  var tangent=D(context.TangentAt(curve,domain.FirstParameter));double vx=Dot(tangent,x),vy=Dot(tangent,y),length=Length2(vx,vy);
  if(length>0){vx/=length;vy/=length;const double epsilon=1.490116119385000000e-8;if(Math.Abs(vy)<=epsilon){vx=vx>=0?1:-1;vy=0;}else if(Math.Abs(vx)<=epsilon){vy=vy>=0?1:-1;vx=0;}x=vx*x+vy*y;}
  foreach(var point in definition.Poles)if(Math.Abs(Dot(P(point)-p,n))>1e-12)return false;
  plane=Plane(p,n,x);return true;
 }
 public static bool TryAveragePlane(CandidateContext context,CandidateShape curve,out CandidatePlane plane){
  if(TryPlanar(context,curve,out plane))return true;var domain=context.InspectCurve(curve);var start=P(context.PointAt(curve,domain.FirstParameter));
  var points=new[]{.25,.33,.5,.66,.75}.Select(f=>P(context.PointAt(curve,context.ParameterAtNormalizedLength(curve,f,1e-9)))).ToArray();V normal=default;int count=0;
  for(int i=0;i<points.Length-1;i++){var a=Unit(points[i]-start);var b=Unit(points[i+1]-start);double dot=Dot(a,b);if(!(dot<.01)&&!(dot>3.14)){normal+=Unit(Cross(a,b));count++;}}
  if(count==0){plane=default;return false;}normal=Unit(normal/count);if(Length(normal)==0)throw new NotSupportedException("Recovered average-plane normal cancels to an invalid plane.");var x=Unit(Perpendicular(normal));plane=Plane(start,normal,x);return true;
 }
}
