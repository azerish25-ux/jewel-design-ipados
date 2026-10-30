using JewelDesign.Documents;
using JewelDesign.Geometry.Candidate;
using JewelDesign.Graph;
namespace JewelDesign.Features.Candidate;

public static partial class OutsideRailV1Candidate
{
    public static OutsideRailCandidate BuildFromNode(CandidateContext context,LegacyValueGraphSession session,int recordIndex,OutsideRailCandidateSettings settings)=>
        BuildFromNodeCore(context,session,recordIndex,settings,true,null);
    internal static OutsideRailCandidate BuildFromNodeCore(CandidateContext context,LegacyValueGraphSession session,int recordIndex,
        OutsideRailCandidateSettings settings,bool generateInputs,Func<LegacyCurveSelection,CandidateShape>? resolveCurve)
    {
        ArgumentNullException.ThrowIfNull(session);
        if((uint)recordIndex>=(uint)session.Document.Records.Count)throw new ArgumentOutOfRangeException(nameof(recordIndex));
        var record=session.Document.Records[recordIndex];
        if(record.TypeGuid!=TypeGuid||record.Version!=new LegacyCsg4Version(1,0)||record.SimpleBlock is not{} block)
            throw new NotSupportedException("OutsideRail_V1 consumer header is not decoded.");
        var inputs=new SerializedChannelLink[6];
        for(int i=0;i<inputs.Length;i++)inputs[i]=i<block.ObjectIndices.Count?new(block.ObjectIndices[i],block.ChannelIndices[i]):SerializedChannelLink.Disconnected;
        return BuildFromChannelsCore(context,session,inputs,settings,generateInputs,resolveCurve);
    }
    public static OutsideRailCandidate BuildFromChannels(CandidateContext context,LegacyValueGraphSession session,
        IReadOnlyList<SerializedChannelLink> bindings,OutsideRailCandidateSettings settings)=>
        BuildFromChannelsCore(context,session,bindings,settings,true,null);
    private static OutsideRailCandidate BuildFromChannelsCore(CandidateContext context,LegacyValueGraphSession session,
        IReadOnlyList<SerializedChannelLink> bindings,OutsideRailCandidateSettings settings,bool generateInputs,
        Func<LegacyCurveSelection,CandidateShape>? resolveCurve)
    {
        ArgumentNullException.ThrowIfNull(context);ArgumentNullException.ThrowIfNull(session);ArgumentNullException.ThrowIfNull(bindings);
        if(bindings.Count!=6)throw new ArgumentException("OutsideRail requires six source-ordered inputs.",nameof(bindings));
        var inputs=bindings.ToArray();var roots=inputs.Where(x=>x.ObjectIndex>=0).Select(x=>x.ObjectIndex).Distinct().ToArray();
        if(generateInputs&&roots.Length!=0)session.Generate(roots);
        LegacyChannelValue Read(int input){var link=inputs[input];return link.ObjectIndex<0?LegacyChannelValue.Empty:session.ReadOutput(link.ObjectIndex,link.ChannelIndex);}
        CandidateShape? Curve(LegacyChannelValue value)
        {
            // CSDataHelper.GetNurbsCurve flattens before selecting its first item.
            if(value.Array is not null){var flat=LegacyArrayData.Flatten(value).Array!;if(flat.Count==0)return null;value=flat[0];}
            if(value.Profile is{} profile){
                using var imported=StoredMatrixProfileCandidate.Build(context,session.Document.Records[profile.GroupRecordIndex].MatrixProfiles!,profile.ProfileIndex);
                return context.Retain(imported.Curve);
            }
            if(value.Curve is{} reference)return resolveCurve?.Invoke(reference)??throw new NotSupportedException("No graph curve resolver is available.");
            return null;
        }
        CandidateShape? outside=null,radiusCurve=null;
        try{
            outside=Curve(Read(1));
            var radiusData=Read(0);double numeric=0;double? radius=radiusData.GetDouble(ref numeric)?numeric:null;
            if(!radius.HasValue)radiusCurve=Curve(radiusData);
            double top=3,side=2,bottom=1;bool locked=false;
            Read(2).GetDouble(ref top);Read(3).GetDouble(ref side);Read(4).GetDouble(ref bottom);Read(5).GetBoolean(ref locked);
            return Build(context,outside,radius,settings,new(top,side,bottom,locked),radiusCurve);
        }finally{outside?.Dispose();radiusCurve?.Dispose();}
    }
}
