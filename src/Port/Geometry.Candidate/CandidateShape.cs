namespace JewelDesign.Geometry.Candidate;

/// <summary>Owned candidate geometry, not a serialized project identity.</summary>
public sealed class CandidateShape:IDisposable
{
    internal CandidateContext Owner {get;}
    internal ShapeHandle Handle {get;}
    internal CandidateShape(CandidateContext owner,ShapeHandle handle)
    {Owner=owner;Handle=handle;}
    public void Dispose()=>Handle.Dispose();
}
