// Numerical adaptation of pinned openNURBS camera transforms; see THIRD_PARTY_NOTICE.md.
// Original Copyright (c) 1993-2018 Robert McNeel & Associates; this C# adaptation is distinct.
using System.Numerics;
using JewelDesign.Geometry.Candidate;
namespace JewelDesign.Rendering.Candidate;

/// <summary>Double row-major projective coefficients for pinned source camera math.</summary>
public sealed class CandidateCameraTransform
{
    private double[] values;
    public CandidateCameraTransform(ReadOnlySpan<double> coefficients)
    {if(coefficients.Length!=16)throw new ArgumentException("Camera matrix requires16 coefficients.",nameof(coefficients));values=coefficients.ToArray();}
    public double[] CopyCoefficients()=>(double[])values.Clone();
    public void SetWorldToCamera(CandidatePoint3 location,CandidateVector3 x,CandidateVector3 y,CandidateVector3 z)
    {
        values=new[]{x.X,x.Y,x.Z,-(x.X*location.X+x.Y*location.Y+x.Z*location.Z),
            y.X,y.Y,y.Z,-(y.X*location.X+y.Y*location.Y+y.Z*location.Z),
            z.X,z.Y,z.Z,-(z.X*location.X+z.Y*location.Y+z.Z*location.Z),0d,0d,0d,1d};
    }
    public void SetCameraToWorld(CandidatePoint3 location,CandidateVector3 x,CandidateVector3 y,CandidateVector3 z)
    {
        values=new[]{x.X,y.X,z.X,location.X,x.Y,y.Y,z.Y,location.Y,x.Z,y.Z,z.Z,location.Z,0d,0d,0d,1d};
    }
    public bool TrySetCameraToClip(bool perspective,CandidateFrustumBounds bounds)
    {
        var (left,right,bottom,top,near,far)=bounds;
        // Source failure retains every previous coefficient; other raw inputs follow source arithmetic.
        if(left==right||bottom==top||near==far)return false;
        var m=new double[16];double difference;
        if(!perspective)
        {
            difference=left-right;m[0]=-2/difference;m[3]=(left+right)/difference;
            difference=bottom-top;m[5]=-2/difference;m[7]=(bottom+top)/difference;
            difference=far-near;m[10]=2/difference;m[11]=(far+near)/difference;m[15]=1;
        }
        else
        {
            difference=right-left;m[0]=2*near/difference;m[2]=(right+left)/difference;
            difference=top-bottom;m[5]=2*near/difference;m[6]=(top+bottom)/difference;
            difference=far-near;m[10]=(far+near)/difference;m[11]=2*near*far/difference;m[14]=-1;
        }
        values=m;return true;
    }
    /// <summary>Interpret this as a source clip transform; map source near+1/far-1 to Metal0/1 and row-vector storage.</summary>
    public bool TryGetMetalClipMatrix(out Matrix4x4 matrix)
    {
        matrix=default;var source=(double[])values.Clone();
        for(int column=0;column<4;column++)source[8+column]=.5*(values[12+column]-values[8+column]);
        var floats=new float[16];
        for(int i=0;i<16;i++){if(!double.IsFinite(source[i]))return false;floats[i]=(float)source[i];if(!float.IsFinite(floats[i]))return false;}
        matrix=new(floats[0],floats[4],floats[8],floats[12],floats[1],floats[5],floats[9],floats[13],floats[2],floats[6],floats[10],floats[14],floats[3],floats[7],floats[11],floats[15]);return true;
    }
}
