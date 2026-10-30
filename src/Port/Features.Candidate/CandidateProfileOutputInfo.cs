namespace JewelDesign.Features.Candidate;

/// <summary>Immutable preserved stored-profile identity/name/sweep metadata for a generated native curve.</summary>
public sealed class CandidateProfileOutputInfo
{
    public int SourceGroupRecordIndex {get;}
    public int SourceProfileIndex {get;}
    public string? Name {get;}
    public IReadOnlyList<double> SweepParameters {get;}
    internal CandidateProfileOutputInfo(int group,int index,string? name,IEnumerable<double> parameters)
    {SourceGroupRecordIndex=group;SourceProfileIndex=index;Name=name;SweepParameters=Array.AsReadOnly(parameters.OrderBy(x=>x).ToArray());}
}
