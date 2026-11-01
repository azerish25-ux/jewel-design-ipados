namespace JewelDesign.Rendering.Candidate;

/// <summary>Immutable raw original5-derived projective coefficients, including nonfinite values.</summary>
public sealed class Rhino5Matrix4
{
    private readonly double[] values;
    public static Rhino5Matrix4 Identity {get;}=new(new double[]{1,0,0,0,0,1,0,0,0,0,1,0,0,0,0,1});
    public Rhino5Matrix4(ReadOnlySpan<double> coefficients)
    {if(coefficients.Length!=16)throw new ArgumentException("Matrix requires16 coefficients.",nameof(coefficients));values=coefficients.ToArray();}
    public double this[int row,int column]=>row>=0&&row<4&&column>=0&&column<4?values[4*row+column]:throw new ArgumentOutOfRangeException(nameof(row));
    public double[] CopyCoefficients()=>(double[])values.Clone();
    /// <summary>Original zero-tolerance cache predicate: unordered comparisons reject.</summary>
    public bool IsIdentity()
    {
        for(int i=0;i<16;i++)if(!(Math.Abs((i%5==0?1d:0d)-values[i])<=0))return false;
        return true;
    }
    public Rhino5Matrix4 Multiply(Rhino5Matrix4 right)
    {
        ArgumentNullException.ThrowIfNull(right);var m=new double[16];
        for(int r=0;r<4;r++)for(int c=0;c<4;c++)m[4*r+c]=values[4*r]*right.values[c]+values[4*r+1]*right.values[4+c]+values[4*r+2]*right.values[8+c]+values[4*r+3]*right.values[12+c];
        return new(m);
    }
    /// <summary>Original GetXform(world0,clip2), including clip modification and ignored projection return.</summary>
    public static bool TryWorldToClip(Rhino5ViewportState viewport,Rhino5Matrix4 clipModification,out Rhino5Matrix4 result)
    {
        ArgumentNullException.ThrowIfNull(clipModification);result=Identity;
        if(!viewport.Frame.CameraValid||!viewport.FrustumValid)return false;
        var f=viewport.Frame;var world=new CandidateCameraTransform(Identity.values);world.SetWorldToCamera(f.Location,f.X,f.Y,f.Z);
        var fallback=new double[16];fallback[15]=1;var projection=new CandidateCameraTransform(fallback);
        // GetXform ignores CameraToClip's false return and still returns true for valid flags.
        projection.TrySetCameraToClip(viewport.Projection==Rhino5Viewport.Perspective,viewport.Bounds);
        result=clipModification.Multiply(new(projection.CopyCoefficients())).Multiply(new(world.CopyCoefficients()));return true;
    }
}
