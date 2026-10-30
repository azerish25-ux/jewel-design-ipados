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
        [LegacyCsg4Codec.ProfilePlacement2RailTypeGuid]=4,[LegacyCsg4Codec.AddTypeGuid]=2,[LegacyCsg4Codec.TranslateObjectTypeGuid]=4,[LegacyCsg4Codec.ScaleObjectToSizeTypeGuid]=4
    };
    private sealed class Output:IDisposable
    {
        internal CandidateShape? Curve {get;}
        internal LegacyChannelValue Data {get;}
        internal CandidateRailOutputInfo? Info {get;}
        internal IReadOnlyList<LegacyChannelValue>? Additional {get;}
        internal Output(CandidateShape? curve,CandidateRailOutputInfo? info,LegacyChannelValue data,IReadOnlyList<LegacyChannelValue>? additional=null){Curve=curve;Info=info;Data=data;Additional=additional is null?null:Array.AsReadOnly(additional.ToArray());}
        public void Dispose()=>Curve?.Dispose();
    }
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
        var record=Values.Document.Records[index];var block=record.SimpleBlock!;int count=InputCounts[record.TypeGuid];
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
        if(Values.Document.Records[index].TypeGuid==LegacyCsg4Codec.ProfilePlacement2RailTypeGuid){
            entry=PlacementOutput(index);
        }else if(Values.Document.Records[index].TypeGuid==LegacyCsg4Codec.AddTypeGuid||Values.Document.Records[index].TypeGuid==LegacyCsg4Codec.TranslateObjectTypeGuid||Values.Document.Records[index].TypeGuid==LegacyCsg4Codec.ScaleObjectToSizeTypeGuid){
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
            entry=Own(context.Retain(result.Curve),info,LegacyChannelValue.FromCurve(index,0));
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
        if(selected.Curve is{} reference){
            var info=Find(reference.ProducerRecordIndex).Info;
            return Own(ResolveCurve(reference),info,LegacyChannelValue.FromCurve(index,0));
        }
        return new(null,null,selected);
    }
    private static Output Own(CandidateShape curve,CandidateRailOutputInfo? info,LegacyChannelValue data)
    {try{return new(curve,info,data);}catch{curve.Dispose();throw;}}
    private Output Find(int index)
    {
        Check();if(pending is not null&&pending.TryGetValue(index,out var current))return current;
        return outputs.TryGetValue(index,out var existing)?existing:throw new InvalidOperationException("Rail output has not been generated.");
    }
    private CandidateShape ResolveCurve(LegacyCurveSelection reference)=>RetainCurve(reference.ProducerRecordIndex,reference.OutputChannelIndex);
    public CandidateShape RetainCurve(int record,int channel=0)
    {
        if(channel!=0)throw new NotSupportedException("Only the rail curve output is retainable.");
        return context.Retain(Find(record).Curve??throw new NotSupportedException("Output does not contain a native rail curve."));
    }
    public CandidateRailOutputInfo ReadMetadata(int record)=>Find(record).Info??throw new NotSupportedException("Output has no rail metadata.");
    public LegacyChannelValue ReadOutput(int record,int channel)
    {
        if(!Supports(record))return Values.ReadOutput(record,channel);
        bool outside=Values.Document.Records[record].TypeGuid==OutsideRailV1Candidate.TypeGuid;
        bool add=Values.Document.Records[record].TypeGuid==LegacyCsg4Codec.AddTypeGuid;
        bool placement=Values.Document.Records[record].TypeGuid==LegacyCsg4Codec.ProfilePlacement2RailTypeGuid;
        if(channel!=0&&!(channel==1&&outside)&&!(add&&channel is 1 or 2)&&!(placement&&channel is >=1 and <=3))return LegacyChannelValue.Empty;
        var output=Find(record);
        if(placement&&channel>0)return output.Additional![channel-1];
        if(add&&channel>0){double number=0;output.Data.GetDouble(ref number);return LegacyChannelValue.FromNumber(channel==1?0.0-number:1.0-number);}
        return channel==0?output.Data:LegacyChannelValue.FromNumber(0);
    }
    public void Dispose()
    {
        if(disposed)return;disposed=true;
        foreach(var output in outputs.Values)output.Dispose();outputs.Clear();
        if(pending is not null){foreach(var output in pending.Values)output.Dispose();pending=null;}
    }
}
