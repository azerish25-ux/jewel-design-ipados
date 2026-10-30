namespace JewelDesign.Documents;

/// <summary>Immutable row-major affine coefficients; fixed last row0,0,0,1. Not a native handle or stored file payload.</summary>
public readonly record struct LegacyAffineTransform(double M11,double M12,double M13,double M14,
    double M21,double M22,double M23,double M24,double M31,double M32,double M33,double M34)
{
    public static LegacyAffineTransform Identity=>Scale(1,1,1);
    public static LegacyAffineTransform Translation(double x,double y,double z)=>new(1,0,0,x,0,1,0,y,0,0,1,z);
    public static LegacyAffineTransform Scale(double x,double y,double z)=>new(x,0,0,0,0,y,0,0,0,0,z,0);
    public bool IsFinite=>double.IsFinite(M11)&&double.IsFinite(M12)&&double.IsFinite(M13)&&double.IsFinite(M14)
        &&double.IsFinite(M21)&&double.IsFinite(M22)&&double.IsFinite(M23)&&double.IsFinite(M24)
        &&double.IsFinite(M31)&&double.IsFinite(M32)&&double.IsFinite(M33)&&double.IsFinite(M34);
    private double At(int row,int column)=>(row,column) switch{
        (0,0)=>M11,(0,1)=>M12,(0,2)=>M13,(0,3)=>M14,
        (1,0)=>M21,(1,1)=>M22,(1,2)=>M23,(1,3)=>M24,
        (2,0)=>M31,(2,1)=>M32,(2,2)=>M33,(2,3)=>M34,
        (3,3)=>1,_=>0
    };
    public static LegacyAffineTransform Multiply(LegacyAffineTransform left,LegacyAffineTransform right)
    {
        if(!left.IsFinite||!right.IsFinite)throw new NotSupportedException("Nonfinite matrix products can require invalid projective rows outside the affine DTO.");
        double Dot(int r,int c)=>left.At(r,0)*right.At(0,c)+left.At(r,1)*right.At(1,c)+left.At(r,2)*right.At(2,c)+left.At(r,3)*right.At(3,c);
        return new(Dot(0,0),Dot(0,1),Dot(0,2),Dot(0,3),Dot(1,0),Dot(1,1),Dot(1,2),Dot(1,3),Dot(2,0),Dot(2,1),Dot(2,2),Dot(2,3));
    }
    public static LegacyAffineTransform operator*(LegacyAffineTransform left,LegacyAffineTransform right)=>Multiply(left,right);
}
