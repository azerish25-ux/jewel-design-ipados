namespace JewelDesign.Geometry.Candidate;

/// <summary>Immutable standard nonperiodic NURBS definition with full knot vector and Euclidean poles.</summary>
public sealed class CandidateNurbsDefinition
{
    internal double[] Coordinates {get;}
    internal double[] FullKnots {get;}
    internal double[]? Weights {get;}
    public uint Degree {get;}
    public IReadOnlyList<CandidatePoint3> Poles {get;}
    public IReadOnlyList<double> KnotVector {get;}
    public IReadOnlyList<double>? PoleWeights {get;}
    internal uint PoleCount {get;}
    public CandidateNurbsDefinition(uint degree,IEnumerable<CandidatePoint3> poles,IEnumerable<double> fullKnots,IEnumerable<double>? weights=null)
    {
        ArgumentNullException.ThrowIfNull(poles);ArgumentNullException.ThrowIfNull(fullKnots);
        Degree=degree;var points=poles.ToArray();Poles=Array.AsReadOnly(points);PoleCount=checked((uint)points.Length);
        Coordinates=new double[checked(points.Length*3)];
        for(int i=0;i<points.Length;i++){Coordinates[i*3]=points[i].X;Coordinates[i*3+1]=points[i].Y;Coordinates[i*3+2]=points[i].Z;}
        FullKnots=fullKnots.ToArray();Weights=weights?.ToArray();KnotVector=Array.AsReadOnly(FullKnots);PoleWeights=Weights is null?null:Array.AsReadOnly(Weights);
    }
}
