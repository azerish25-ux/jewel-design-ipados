using JewelDesign.Documents;
using JewelDesign.Geometry.Candidate;
using JewelDesign.Graph;
namespace JewelDesign.Features.Candidate;

public static partial class OutsideRailV1Candidate
{
    /// <summary>Generate/read a known scalar/profile input slice. Bindings are explicit until consumer headers are decoded.</summary>
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
