#nullable disable
// Exact recovered frame/helper/seam fragments. Arithmetic stubs are observation aids, not an original native oracle.
using JewelDesign.Documents;
using JewelDesign.Geometry.Candidate;
namespace CandidateGeometryContractChecks.Recovered.Placement;
internal interface IOn3dPoint{double x{get;}double y{get;}double z{get;}}
internal interface IOn3dVector{double x{get;}double y{get;}double z{get;}}
internal sealed class On3dPoint:IOn3dPoint
{
    public double x{get;set;}public double y{get;set;}public double z{get;set;}
    internal On3dPoint(){}internal On3dPoint(double x,double y,double z){this.x=x;this.y=y;this.z=z;}
    public static On3dVector operator-(On3dPoint a,On3dPoint b)=>new(a.x-b.x,a.y-b.y,a.z-b.z);
}
internal sealed class On3dVector:IOn3dVector
{
    public double x{get;set;}public double y{get;set;}public double z{get;set;}
    internal On3dVector(){}internal On3dVector(double x,double y,double z){this.x=x;this.y=y;this.z=z;}
    internal On3dVector(IOn3dVector v):this(v.x,v.y,v.z){}
    internal bool Unitize(){double n=Math.Sqrt(x*x+y*y+z*z);if(n==0||!double.IsFinite(n))return false;x/=n;y/=n;z/=n;return true;}
}
internal static class OnUtil
{
    internal static readonly On3dPoint On_origin=new();
    internal static readonly On3dVector On_yaxis=new(0,1,0),On_zaxis=new(0,0,1);
    internal static On3dVector ON_CrossProduct(IOn3dVector a,IOn3dVector b)=>new(a.y*b.z-a.z*b.y,a.z*b.x-a.x*b.z,a.x*b.y-a.y*b.x);
}
internal sealed class OnXform
{
    private readonly double[,] a=new double[4,4];
    internal OnXform(){for(int i=0;i<4;i++)a[i,i]=1;}
    internal double this[int r,int c]{get=>a[r,c];set=>a[r,c]=value;}
    internal void set_m_xform(int r,int c,double value){a[r,c]=value;}
    public static OnXform operator*(OnXform x,OnXform y){var z=new OnXform();for(int r=0;r<4;r++)for(int c=0;c<4;c++){z[r,c]=0;for(int k=0;k<4;k++)z[r,c]+=x[r,k]*y[k,c];}return z;}
    internal LegacyAffineTransform Matrix=>new(a[0,0],a[0,1],a[0,2],a[0,3],a[1,0],a[1,1],a[1,2],a[1,3],a[2,0],a[2,1],a[2,2],a[2,3]);
}
internal interface IOnCurve{bool GetNormalizedArcLengthPoint(double fraction,ref double parameter);bool Ev1Der(double parameter,ref On3dPoint point,ref On3dVector derivative);}
internal sealed class ObservedCurve:IOnCurve
{
    private readonly CandidatePoint3 point;private readonly CandidateVector3 tangent;
    internal ObservedCurve(CandidatePoint3 point,CandidateVector3 tangent){this.point=point;this.tangent=tangent;}
    public bool GetNormalizedArcLengthPoint(double fraction,ref double parameter){parameter=fraction;return true;}
    public bool Ev1Der(double parameter,ref On3dPoint p,ref On3dVector t){p=new(point.X,point.Y,point.Z);t=new(tangent.X,tangent.Y,tangent.Z);return true;}
}
internal sealed class DomainCurve
{
    private readonly double first,last;internal DomainCurve(double first,double last){this.first=first;this.last=last;}
    internal DomainCurve Domain()=>this;internal double Min()=>first;internal double Max()=>last;
}
internal static class CSUtilMath{
public static OnXform Orient3Point(IOn3dPoint source0, IOn3dVector source1, IOn3dVector source2, IOn3dPoint destination0, IOn3dVector destination1, IOn3dVector destination2)
	{
		On3dVector on3dVector = new On3dVector(source1);
		On3dVector arg = new On3dVector(source2);
		On3dVector on3dVector2 = OnUtil.ON_CrossProduct(on3dVector, arg);
		arg = OnUtil.ON_CrossProduct(on3dVector2, on3dVector);
		on3dVector.Unitize();
		arg.Unitize();
		on3dVector2.Unitize();
		OnXform onXform = new OnXform();
		onXform.set_m_xform(0, 0, on3dVector.x);
		onXform.set_m_xform(0, 1, on3dVector.y);
		onXform.set_m_xform(0, 2, on3dVector.z);
		onXform.set_m_xform(1, 0, arg.x);
		onXform.set_m_xform(1, 1, arg.y);
		onXform.set_m_xform(1, 2, arg.z);
		onXform.set_m_xform(2, 0, on3dVector2.x);
		onXform.set_m_xform(2, 1, on3dVector2.y);
		onXform.set_m_xform(2, 2, on3dVector2.z);
		onXform.set_m_xform(0, 3, (0.0 - source0.x) * on3dVector.x - source0.y * on3dVector.y - source0.z * on3dVector.z);
		onXform.set_m_xform(1, 3, (0.0 - source0.x) * arg.x - source0.y * arg.y - source0.z * arg.z);
		onXform.set_m_xform(2, 3, (0.0 - source0.x) * on3dVector2.x - source0.y * on3dVector2.y - source0.z * on3dVector2.z);
		On3dVector on3dVector3 = new On3dVector(destination1);
		On3dVector arg2 = new On3dVector(destination2);
		On3dVector on3dVector4 = OnUtil.ON_CrossProduct(on3dVector3, arg2);
		arg2 = OnUtil.ON_CrossProduct(on3dVector4, on3dVector3);
		on3dVector3.Unitize();
		arg2.Unitize();
		on3dVector4.Unitize();
		return new OnXform
		{
			[0, 0] = on3dVector3.x,
			[1, 0] = on3dVector3.y,
			[2, 0] = on3dVector3.z,
			[0, 1] = arg2.x,
			[1, 1] = arg2.y,
			[2, 1] = arg2.z,
			[0, 2] = on3dVector4.x,
			[1, 2] = on3dVector4.y,
			[2, 2] = on3dVector4.z,
			[0, 3] = destination0.x,
			[1, 3] = destination0.y,
			[2, 3] = destination0.z
		} * onXform;
	}
}
internal static class SourceGeometry{
public static bool OrientProfileToCurveNalp(IOnCurve rail, double param, ref OnXform x, double yScale)
	{
		if (!rail.GetNormalizedArcLengthPoint(param, ref param))
		{
			return false;
		}
		On3dPoint point = new On3dPoint();
		On3dVector first_derivative = new On3dVector();
		if (!rail.Ev1Der(param, ref point, ref first_derivative))
		{
			return false;
		}
		first_derivative.Unitize();
		On3dVector on3dVector = point - OnUtil.On_origin;
		on3dVector.y *= yScale;
		on3dVector.Unitize();
		x = CSUtilMath.Orient3Point(OnUtil.On_origin, OnUtil.On_yaxis, OnUtil.On_zaxis, point, on3dVector, first_derivative);
		return true;
	}
}
internal static class SourceAdjust{
internal static double Adjust(double value,double innerFirst,double innerLast,double outerFirst,double outerLast,double num2){
var nurbsCurve=new DomainCurve(innerFirst,innerLast);var nurbsCurve2=new DomainCurve(outerFirst,outerLast);
if (Math.Abs(value - nurbsCurve.Domain().Min()) < 0.0001 && Math.Abs(num2 - nurbsCurve2.Domain().Max()) < 0.0001)
				{
					num2 = nurbsCurve2.Domain().Min();
				}
if (Math.Abs(value - nurbsCurve.Domain().Max()) < 0.0001 && Math.Abs(num2 - nurbsCurve2.Domain().Min()) < 0.0001)
				{
					num2 = nurbsCurve2.Domain().Max();
				}
return num2;}
internal static double Height(double num){
if (num < 0.01)
				{
					num = 0.01;
				}
return num;}
}
