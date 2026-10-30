using JewelDesign.Geometry.Candidate;
namespace JewelDesign.Features.Candidate;

/// <summary>Source-derived radius/ZX geometry plan only; channel and CSBlock lifecycle are separate.</summary>
public static class RingRailV1Candidate
{
    public static readonly Guid TypeGuid=new("65aa45c6-d1cd-4c45-92bb-b5a9a6a2ec47");
    public static RingRailCandidate Build(CandidateContext context,double radius=7)
    {
        ArgumentNullException.ThrowIfNull(context);
        var curve=context.CreateCircle(radius,CandidatePlane.WorldZX);
        try {return new RingRailCandidate(curve,radius);}
        catch {curve.Dispose();throw;}
    }
}

/// <summary>Owns actual candidate NURBS geometry and preserves the supplied radius metadata.</summary>
public sealed class RingRailCandidate:IDisposable
{
    public CandidateShape Curve {get;}
    public double Radius {get;}
    internal RingRailCandidate(CandidateShape curve,double radius){Curve=curve;Radius=radius;}
    public void Dispose()=>Curve.Dispose();
}
