using JewelDesign.Documents;
namespace JewelDesign.Features.Candidate;

/// <summary>Recovered degree/mirror/product plan with pinned published openNURBS6 numeric comparison policy; not original Rhino5 proof.</summary>
public static class RotationProducerCandidate
{
    private const double Epsilon=2.2204460492503131e-16,SqrtEpsilon=1.490116119385000000e-8;
    private static LegacyAffineTransform Axis(double degrees,int axis)
    {
        double angle=(Math.PI/180.0)*degrees,s=Math.Sin(angle),c=Math.Cos(angle);
        // Preserve the comparison implementation's cardinal-noise cleanup and branch order.
        if(Math.Abs(s)>=1-SqrtEpsilon&&Math.Abs(c)<=SqrtEpsilon){c=0;s=s<0?-1:1;}
        else if(Math.Abs(c)>=1-SqrtEpsilon&&Math.Abs(s)<=SqrtEpsilon){c=c<0?-1:1;s=0;}
        else{
            if(Math.Abs(c*c+s*s-1)>SqrtEpsilon){
                double length=Math.Sqrt(c*c+s*s);
                if(length>0){c/=length;s/=length;}else{c=1;s=0;}
            }
            if(Math.Abs(c)>1-Epsilon||Math.Abs(s)<Epsilon){c=c<0?-1:1;s=0;}
            else if(Math.Abs(s)>1-Epsilon||Math.Abs(c)<Epsilon){c=0;s=s<0?-1:1;}
        }
        if(s==0&&c==1)return LegacyAffineTransform.Identity;
        double x=axis==0?1:0,y=axis==1?1:0,z=axis==2?1:0,d=1-c;
        return new(x*x*d+c,x*y*d-z*s,x*z*d+y*s,0,
            y*x*d+z*s,y*y*d+c,y*z*d-x*s,0,
            z*x*d-y*s,z*y*d+x*s,z*z*d+c,0);
    }
    public static LegacyAffineTransform Build(LegacyChannelValue x,LegacyChannelValue y,LegacyChannelValue z,LegacyChannelValue mirror)
    {
        double dx=0,dy=0,dz=0,m=0;x.GetDouble(ref dx);y.GetDouble(ref dy);z.GetDouble(ref dz);mirror.GetDouble(ref m);
        if(!double.IsFinite(dx)||!double.IsFinite(dy)||!double.IsFinite(dz))
            throw new NotSupportedException("Nonfinite rotation angles require original invalid-matrix/Unset behavior outside this affine candidate.");
        if(m>.5){dx=0.0-dx;dz=0.0-dz;}
        return Axis(dx,0)*Axis(dy,1)*Axis(dz,2);
    }
}
