namespace JewelDesign.Geometry.Candidate;

/// <summary>Row-major affine coefficients; no projective row or implicit orientation repair.</summary>
public readonly record struct CandidateAffine(double M11,double M12,double M13,double M14,
    double M21,double M22,double M23,double M24,double M31,double M32,double M33,double M34)
{
    public static CandidateAffine Identity=>new(1,0,0,0,0,1,0,0,0,0,1,0);
    internal double[] ToArray()=>new[]{M11,M12,M13,M14,M21,M22,M23,M24,M31,M32,M33,M34};
}
