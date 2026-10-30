using JewelDesign.Documents;
using JewelDesign.Geometry.Candidate;
namespace JewelDesign.Features.Candidate;

/// <summary>Source-derived radius/ZX geometry plan only; channel and CSBlock lifecycle are separate.</summary>
public static class RingRailV1Candidate
{
    public static readonly Guid TypeGuid=new("65aa45c6-d1cd-4c45-92bb-b5a9a6a2ec47");

    public static RingRailCandidate BuildFromNode(CandidateContext context,LegacyValueGraphSession session,int recordIndex)=>BuildFromNodeCore(context,session,recordIndex,true);
    internal static RingRailCandidate BuildFromNodeCore(CandidateContext context,LegacyValueGraphSession session,int recordIndex,bool generateInputs)
    {
        ArgumentNullException.ThrowIfNull(session);
        if((uint)recordIndex>=(uint)session.Document.Records.Count)throw new ArgumentOutOfRangeException(nameof(recordIndex));
        var record=session.Document.Records[recordIndex];
        if(record.TypeGuid!=TypeGuid||record.Version!=new LegacyCsg4Version(1,0)||record.SimpleBlock is not{} block)
            throw new NotSupportedException("RingRail_V1 consumer header is not decoded.");
        double radius=7;
        if(block.ObjectIndices.Count>0&&block.ObjectIndices[0]>=0){
            if(generateInputs)session.Generate(block.ObjectIndices[0]);session.ReadOutput(block.ObjectIndices[0],block.ChannelIndices[0]).GetDouble(ref radius);
        }
        return Build(context,radius);
    }
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
