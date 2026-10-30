using JewelDesign.Documents;
using JewelDesign.Geometry.Candidate;
using JewelDesign.Graph;
namespace JewelDesign.Features.Candidate;

public static partial class OutsideRailV1Candidate
{
    public static OutsideRailCandidate BuildFromNode(CandidateContext context,LegacyValueGraphSession session,int recordIndex,OutsideRailCandidateSettings settings)
    {
        ArgumentNullException.ThrowIfNull(session);
        if((uint)recordIndex>=(uint)session.Document.Records.Count)throw new ArgumentOutOfRangeException(nameof(recordIndex));
        var record=session.Document.Records[recordIndex];
        if(record.TypeGuid!=TypeGuid||record.Version!=new LegacyCsg4Version(1,0)||record.SimpleBlock is not{} block)
            throw new NotSupportedException("OutsideRail_V1 consumer header is not decoded.");
        var inputs=new SerializedChannelLink[6];
        for(int i=0;i<inputs.Length;i++)inputs[i]=i<block.ObjectIndices.Count?new(block.ObjectIndices[i],block.ChannelIndices[i]):SerializedChannelLink.Disconnected;
        return BuildFromChannels(context,session,inputs,settings);
    }

    /// <summary>Generate/read a known scalar/profile input slice. Bindings can be supplied directly or decoded from the known consumer record.</summary>
    public static OutsideRailCandidate BuildFromChannels(CandidateContext context,LegacyValueGraphSession session,
        IReadOnlyList<SerializedChannelLink> bindings,OutsideRailCandidateSettings settings)
    {
        ArgumentNullException.ThrowIfNull(context);ArgumentNullException.ThrowIfNull(session);ArgumentNullException.ThrowIfNull(bindings);
        if(bindings.Count!=6)throw new ArgumentException("OutsideRail requires six source-ordered inputs.",nameof(bindings));
        var inputs=bindings.ToArray();
        var roots=inputs.Where(x=>x.ObjectIndex>=0).Select(x=>x.ObjectIndex).Distinct().ToArray();
        if(roots.Length!=0)session.Generate(roots);
        LegacyChannelValue Read(int input){var link=inputs[input];return link.ObjectIndex<0?LegacyChannelValue.Empty:session.ReadOutput(link.ObjectIndex,link.ChannelIndex);}
        StoredMatrixProfileCandidate? outside=null,radiusCurve=null;
        try{
            var curve=Read(1).Profile;
            if(curve.HasValue)outside=StoredMatrixProfileCandidate.Build(context,session.Document.Records[curve.Value.GroupRecordIndex].MatrixProfiles!,curve.Value.ProfileIndex);
            var radiusData=Read(0);double numeric=0;double? radius=radiusData.GetDouble(ref numeric)?numeric:null;
            if(!radius.HasValue&&radiusData.Profile is{} profile)
                radiusCurve=StoredMatrixProfileCandidate.Build(context,session.Document.Records[profile.GroupRecordIndex].MatrixProfiles!,profile.ProfileIndex);
            double top=3,side=2,bottom=1;bool locked=false;
            Read(2).GetDouble(ref top);Read(3).GetDouble(ref side);Read(4).GetDouble(ref bottom);Read(5).GetBoolean(ref locked);
            return Build(context,outside?.Curve,radius,settings,new(top,side,bottom,locked),radiusCurve?.Curve);
        }finally{outside?.Dispose();radiusCurve?.Dispose();}
    }
}
