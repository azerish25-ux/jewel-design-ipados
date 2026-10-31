using JewelDesign.Documents;
using JewelDesign.Geometry.Candidate;
using JewelDesign.Graph;
namespace JewelDesign.Features.Candidate;

public enum CandidateRailOutputKind { Ring,Outside }
public sealed record CandidateRailOutputInfo(CandidateRailOutputKind Kind,double? Radius,double? DataScale,IReadOnlyList<OutsideRailKernelDiagnostic> Diagnostics);

/// <summary>Known rail/transform dependency cache owner. This is not the complete original graph scheduler.</summary>
public sealed partial class CandidateRailGraphSession:ILegacyValueNodeProvider,IDisposable
{
    private static readonly IReadOnlyDictionary<Guid,int> InputCounts=new Dictionary<Guid,int>{
        [RingRailV1Candidate.TypeGuid]=1,[OutsideRailV1Candidate.TypeGuid]=6,[LegacyCsg4Codec.SelectRingRailTypeGuid]=3,
        [LegacyCsg4Codec.ProfileCapTypeGuid]=2,[LegacyCsg4Codec.SimpleSweep2TypeGuid]=8,[LegacyCsg4Codec.ArrayTypeGuid]=1,[LegacyCsg4Codec.UnarrayV1TypeGuid]=1,[LegacyCsg4Codec.ProfilePlacement2RailTypeGuid]=4,[LegacyCsg4Codec.TransformObjectTypeGuid]=2,[LegacyCsg4Codec.RotateObjectTypeGuid]=4,[LegacyCsg4Codec.AddTypeGuid]=2,[LegacyCsg4Codec.TranslateObjectTypeGuid]=4,[LegacyCsg4Codec.ScaleObjectToSizeTypeGuid]=4
    };
    private readonly CandidateContext context;
    private readonly OutsideRailCandidateSettings settings;
    private Dictionary<int,Output> outputs=new();
    private Dictionary<int,Output>? pending;
    private bool disposed;
    public LegacyValueGraphSession Values {get;}
    public CandidateRailGraphSession(CandidateContext context,LegacyCsg4Document document,OutsideRailCandidateSettings settings)
    {
        ArgumentNullException.ThrowIfNull(context);ArgumentNullException.ThrowIfNull(document);ArgumentNullException.ThrowIfNull(settings);
        this.context=context;this.settings=settings;Values=new(document,this);
    }
    private void Check(){if(disposed)throw new ObjectDisposedException(nameof(CandidateRailGraphSession));}
    public bool Supports(int index)
    {
        Check();if((uint)index>=(uint)Values.Document.Records.Count)return false;
        var record=Values.Document.Records[index];
        return record.SimpleBlock is not null&&record.Version==new LegacyCsg4Version(1,0)&&
            InputCounts.ContainsKey(record.TypeGuid);
    }
    public IReadOnlyList<SerializedChannelLink> Inputs(int index)
    {
        if(!Supports(index))throw new NotSupportedException("Unsupported candidate producer.");
        var record=Values.Document.Records[index];
        if(record.TypeGuid==LegacyCsg4Codec.TransformObjectTypeGuid)return ConsumerLayout(index).AllChannels;
        if(record.TypeGuid==LegacyCsg4Codec.ArrayTypeGuid||record.TypeGuid==LegacyCsg4Codec.UnarrayV1TypeGuid)return CollectionLayout(index).AllChannels;
        var block=record.SimpleBlock!;int count=InputCounts[record.TypeGuid];
        var result=new SerializedChannelLink[count];
        for(int i=0;i<count;i++)result[i]=i<block.ObjectIndices.Count?new(block.ObjectIndices[i],block.ChannelIndices[i]):SerializedChannelLink.Disconnected;
        return Array.AsReadOnly(result);
    }
    private bool IsDefaultRailRoot(int index)
    {
        var type=Values.Document.Records[index].TypeGuid;
        return Supports(index)&&(type==RingRailV1Candidate.TypeGuid||type==OutsideRailV1Candidate.TypeGuid||type==LegacyCsg4Codec.SelectRingRailTypeGuid);
    }
    public void Generate(params int[] roots)
    {
        Check();ArgumentNullException.ThrowIfNull(roots);if(pending is not null)throw new InvalidOperationException("Reentrant rail generation.");
        pending=new();
        try{
            var targets=roots.Length==0?Enumerable.Range(0,Values.Document.Records.Count).Where(IsDefaultRailRoot).ToArray():roots;
            if(targets.Length!=0)Values.Generate(targets);
            var merged=new Dictionary<int,Output>(outputs);
            foreach(var pair in pending)merged[pair.Key]=pair.Value;
            var previous=outputs;outputs=merged;var committed=pending;pending=null;
            foreach(int node in committed.Keys)if(previous.TryGetValue(node,out var old))old.Dispose();
        }finally{
            if(pending is not null){foreach(var value in pending.Values)value.Dispose();pending=null;}
        }
    }
    public void Update(int index)
    {
        Check();if(pending is null)throw new InvalidOperationException("Use the rail graph Generate method.");
        if(!Supports(index))throw new NotSupportedException("Unsupported candidate producer.");
        Output entry;
        if(Values.Document.Records[index].TypeGuid==LegacyCsg4Codec.ArrayTypeGuid||Values.Document.Records[index].TypeGuid==LegacyCsg4Codec.UnarrayV1TypeGuid){
            entry=CollectionOutput(index);
        }else if(Values.Document.Records[index].TypeGuid==LegacyCsg4Codec.ProfileCapTypeGuid){
            entry=ProfileCapOutput(index);
        }else if(Values.Document.Records[index].TypeGuid==LegacyCsg4Codec.SimpleSweep2TypeGuid){
            entry=SweepOutput(index);
        }else if(Values.Document.Records[index].TypeGuid==LegacyCsg4Codec.TransformObjectTypeGuid){
            entry=ConsumeTransform(index);
        }else if(Values.Document.Records[index].TypeGuid==LegacyCsg4Codec.ProfilePlacement2RailTypeGuid){
            entry=PlacementOutput(index);
        }else if(Values.Document.Records[index].TypeGuid==LegacyCsg4Codec.RotateObjectTypeGuid||Values.Document.Records[index].TypeGuid==LegacyCsg4Codec.AddTypeGuid||Values.Document.Records[index].TypeGuid==LegacyCsg4Codec.TranslateObjectTypeGuid||Values.Document.Records[index].TypeGuid==LegacyCsg4Codec.ScaleObjectToSizeTypeGuid){
            entry=TransformOutput(index);
        }else if(Values.Document.Records[index].TypeGuid==LegacyCsg4Codec.SelectRingRailTypeGuid){
            entry=SelectOutput(index);
        }else if(Values.Document.Records[index].TypeGuid==RingRailV1Candidate.TypeGuid){
            using var result=RingRailV1Candidate.BuildFromNodeCore(context,Values,index,false);
            var info=new CandidateRailOutputInfo(CandidateRailOutputKind.Ring,result.Radius,null,Array.Empty<OutsideRailKernelDiagnostic>());
            entry=Own(context.Retain(result.Curve),info,LegacyChannelValue.FromCurve(index,0));
        }else{
            using var result=OutsideRailV1Candidate.BuildFromNodeCore(context,Values,index,settings,false,ResolveCurve);
            var info=new CandidateRailOutputInfo(CandidateRailOutputKind.Outside,null,1,result.Diagnostics);
            entry=Own(context.Retain(result.Curve),info,LegacyChannelValue.FromCurve(index,0),additional:new[]{LegacyChannelValue.FromNumber(0)});
        }
        try{pending.Add(index,entry);}catch{entry.Dispose();throw;}
    }
    private Output SelectOutput(int index)
    {
        var links=Inputs(index);
        LegacyChannelValue Read(int slot)=>links[slot].ObjectIndex<0?LegacyChannelValue.Missing:Values.ReadOutput(links[slot].ObjectIndex,links[slot].ChannelIndex);
        bool useSecond=false;Read(2).GetBoolean(ref useSecond);
        var first=Read(0);var second=Read(1);
        LegacyChannelValue selected;
        if(first.IsMissing)selected=second;
        else if(second.IsMissing)selected=first;
        else selected=useSecond?second:first;
        return RelayOutput(index,new[]{selected});
    }
    private static Output Own(CandidateShape curve,CandidateRailOutputInfo? info,LegacyChannelValue data,CandidateProfileOutputInfo? profile=null,LegacyAffineTransform? applied=null,IReadOnlyList<LegacyChannelValue>? additional=null)
    {try{return new(curve,info,data,additional,profile:profile,applied:applied);}catch{curve.Dispose();throw;}}
    private Output Find(int index)
    {
        Check();if(pending is not null&&pending.TryGetValue(index,out var current))return current;
        return outputs.TryGetValue(index,out var existing)?existing:throw new InvalidOperationException("Rail output has not been generated.");
    }
    private CandidateShape ResolveCurve(LegacyCurveSelection reference)=>RetainCurve(reference);
    public CandidateShape RetainCurve(int record,int channel=0,int item=0)=>RetainCurve(new LegacyCurveSelection(record,channel,item));
    public CandidateShape RetainCurve(LegacyCurveSelection reference){Check();return context.Retain(FindCurve(reference).Curve);}
    public CandidateRailOutputInfo ReadMetadata(int record,int channel=0,int item=0)=>FindCurve(new(record,channel,item)).Rail??throw new NotSupportedException("Output has no rail metadata.");
    public LegacyChannelValue ReadOutput(int record,int channel)
    {
        if(!Supports(record))return Values.ReadOutput(record,channel);
        var type=Values.Document.Records[record].TypeGuid;
        int count=type==OutsideRailV1Candidate.TypeGuid?2:type==LegacyCsg4Codec.AddTypeGuid?3:type==LegacyCsg4Codec.ProfilePlacement2RailTypeGuid?4:1;
        if(channel<0||(type!=LegacyCsg4Codec.UnarrayV1TypeGuid&&type!=LegacyCsg4Codec.SimpleSweep2TypeGuid&&channel>=count))return LegacyChannelValue.Empty;
        return Find(record).Read(channel);
    }
    public void Dispose()
    {
        if(disposed)return;disposed=true;
        foreach(var output in outputs.Values)output.Dispose();outputs.Clear();
        if(pending is not null){foreach(var output in pending.Values)output.Dispose();pending=null;}
    }
}
