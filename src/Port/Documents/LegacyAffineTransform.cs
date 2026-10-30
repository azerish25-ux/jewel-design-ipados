namespace JewelDesign.Documents;

/// <summary>Immutable row-major affine coefficients; fixed last row0,0,0,1. Not a native handle or stored file payload.</summary>
public readonly record struct LegacyAffineTransform(double M11,double M12,double M13,double M14,
    double M21,double M22,double M23,double M24,double M31,double M32,double M33,double M34)
{
    public static LegacyAffineTransform Translation(double x,double y,double z)=>new(1,0,0,x,0,1,0,y,0,0,1,z);
    public static LegacyAffineTransform Scale(double x,double y,double z)=>new(x,0,0,0,0,y,0,0,0,0,z,0);
}
