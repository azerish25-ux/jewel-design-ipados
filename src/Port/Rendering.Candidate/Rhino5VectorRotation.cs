// Numerical adaptation; original published notices in THIRD_PARTY_NOTICE.md.
using JewelDesign.Geometry.Candidate;
namespace JewelDesign.Rendering.Candidate;

/// <summary>Original5-derived linear rotation for camera vectors. No point/center translation.</summary>
public sealed class Rhino5VectorRotation
{
    private const double Epsilon=2.220446049250313e-16;
    private const double SqrtEpsilon=1.490116119385e-8;
    private const double MinimumNormal=2.2250738585072014e-308;
    private readonly double m00,m01,m02,m10,m11,m12,m20,m21,m22;
    public int DiagnosticCount {get;}
    private Rhino5VectorRotation(double a,double b,double c,double d,double e,double f,double g,double h,double i,int diagnostics=0)
    {m00=a;m01=b;m02=c;m10=d;m11=e;m12=f;m20=g;m21=h;m22=i;DiagnosticCount=diagnostics;}
    private static Rhino5VectorRotation Identity(int diagnostics=0)=>new(1,0,0,0,1,0,0,0,1,diagnostics);
    public static Rhino5VectorRotation FromAngle(double angle,CandidateVector3 axis)=>FromSinCos(Math.Sin(angle),Math.Cos(angle),axis);
    public static Rhino5VectorRotation FromSinCos(double sine,double cosine,CandidateVector3 axis)
    {
        if(Math.Abs(sine)>=1-SqrtEpsilon&&Math.Abs(cosine)<=SqrtEpsilon)
        {cosine=0;sine=sine<0?-1:1;}
        else if(Math.Abs(cosine)>=1-SqrtEpsilon&&Math.Abs(sine)<=SqrtEpsilon)
        {cosine=cosine<0?-1:1;sine=0;}
        else
        {
            if(Math.Abs(sine*sine+cosine*cosine-1)>SqrtEpsilon&&!UnitizePair(ref cosine,ref sine))return Identity(1);
            if(Math.Abs(cosine)>1-Epsilon||Math.Abs(sine)<Epsilon){cosine=cosine<0?-1:1;sine=0;}
            else if(Math.Abs(sine)>1-Epsilon||Math.Abs(cosine)<Epsilon){cosine=0;sine=sine<0?-1:1;}
        }
        if(sine==0&&cosine==1)return Identity();
        if(Math.Abs(axis.Y*axis.Y+axis.X*axis.X+axis.Z*axis.Z-1)>Epsilon)CandidateCameraFrame.Unitize(ref axis);
        double x=axis.X,y=axis.Y,z=axis.Z,d=1-cosine;
        return new(x*x*d+cosine,x*y*d-z*sine,x*z*d+y*sine,
            y*x*d+z*sine,y*y*d+cosine,y*z*d-x*sine,
            z*x*d-y*sine,z*y*d+x*sine,z*z*d+cosine);
    }
    /// <summary>Vector operator deliberately excludes the rotation center's affine translation.</summary>
    public CandidateVector3 Apply(CandidateVector3 v)=>new(v.Y*m01+v.X*m00+v.Z*m02,v.Y*m11+v.X*m10+v.Z*m12,v.Y*m21+v.X*m20+v.Z*m22);
    private static double Length2(double x,double y)
    {
        x=Math.Abs(x);y=Math.Abs(y);if(y>x)(x,y)=(y,x);
        if(x>MinimumNormal){y/=x;return x*Math.Sqrt(1+y*y);}
        return x>0&&double.IsFinite(x)?x:0;
    }
    private static bool UnitizePair(ref double x,ref double y)
    {
        double d=Length2(x,y);
        if(double.IsFinite(d))
        {
            if(d>MinimumNormal){x/=d;y/=d;return true;}
            if(d>0)
            {
                double a=x*8.9884656743115795386465259539451e307,b=y*8.9884656743115795386465259539451e307;d=Length2(a,b);
                if(double.IsFinite(d)&&d>MinimumNormal){x=a/d;y=b/d;return true;}
            }
        }
        x=0;y=0;return false;
    }
}
