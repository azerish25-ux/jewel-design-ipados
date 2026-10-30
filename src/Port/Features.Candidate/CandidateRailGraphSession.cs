using JewelDesign.Documents;
using JewelDesign.Geometry.Candidate;
using JewelDesign.Graph;
namespace JewelDesign.Features.Candidate;

public enum CandidateRailOutputKind { Ring,Outside }
public sealed record CandidateRailOutputInfo(CandidateRailOutputKind Kind,double? Radius,double? DataScale,IReadOnlyList<OutsideRailKernelDiagnostic> Diagnostics);

/// <summary>Known rail generation/cache owner. This is not the complete original graph scheduler.</summary>
public sealed class CandidateRailGraphSession:ILegacyValueNodeProvider,IDisposable
{
    private sealed class Output:IDisposable
    {
        internal CandidateShape Curve {get;}
        internal CandidateRailOutputInfo Info {get;}
        internal Output(CandidateShape curve,CandidateRailOutputInfo info){Curve=curve;Info=info;}
        public void Dispose()=>Curve.Dispose();
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
            (record.TypeGuid==RingRailV1Candidate.TypeGuid||record.TypeGuid==OutsideRailV1Candidate.TypeGuid);
    }
    public IReadOnlyList<SerializedChannelLink> Inputs(int index)
    {
        if(!Supports(index))throw new NotSupportedException("Unsupported rail producer.");
        var record=Values.Document.Records[index];var block=record.SimpleBlock!;int count=record.TypeGuid==RingRailV1Candidate.TypeGuid?1:6;
        var result=new SerializedChannelLink[count];
        for(int i=0;i<count;i++)result[i]=i<block.ObjectIndices.Count?new(block.ObjectIndices[i],block.ChannelIndices[i]):SerializedChannelLink.Disconnected;
        return Array.AsReadOnly(result);
    }
    public void Generate(params int[] roots)
    {
        Check();ArgumentNullException.ThrowIfNull(roots);if(pending is not null)throw new InvalidOperationException("Reentrant rail generation.");
        pending=new();
        try{
            var targets=roots.Length==0?Enumerable.Range(0,Values.Document.Records.Count).Where(Supports).ToArray():roots;
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
        if(!Supports(index))throw new NotSupportedException("Unsupported rail producer.");
        Output entry;
        if(Values.Document.Records[index].TypeGuid==RingRailV1Candidate.TypeGuid){
            using var result=RingRailV1Candidate.BuildFromNodeCore(context,Values,index,false);
            var info=new CandidateRailOutputInfo(CandidateRailOutputKind.Ring,result.Radius,null,Array.Empty<OutsideRailKernelDiagnostic>());
            entry=Own(context.Retain(result.Curve),info);
        }else{
            using var result=OutsideRailV1Candidate.BuildFromNodeCore(context,Values,index,settings,false,ResolveCurve);
            var info=new CandidateRailOutputInfo(CandidateRailOutputKind.Outside,null,1,result.Diagnostics);
            entry=Own(context.Retain(result.Curve),info);
        }
        try{pending.Add(index,entry);}catch{entry.Dispose();throw;}
    }
    private static Output Own(CandidateShape curve,CandidateRailOutputInfo info)
    {try{return new(curve,info);}catch{curve.Dispose();throw;}}
    private Output Find(int index)
    {
        Check();if(pending is not null&&pending.TryGetValue(index,out var current))return current;
        return outputs.TryGetValue(index,out var existing)?existing:throw new InvalidOperationException("Rail output has not been generated.");
    }
    private CandidateShape ResolveCurve(LegacyCurveSelection reference)=>RetainCurve(reference.ProducerRecordIndex,reference.OutputChannelIndex);
    public CandidateShape RetainCurve(int record,int channel=0)
    {
        if(channel!=0)throw new NotSupportedException("Only the rail curve output is retainable.");
        return context.Retain(Find(record).Curve);
    }
    public CandidateRailOutputInfo ReadMetadata(int record)=>Find(record).Info;
    public LegacyChannelValue ReadOutput(int record,int channel)
    {
        if(!Supports(record))return Values.ReadOutput(record,channel);
        bool outside=Values.Document.Records[record].TypeGuid==OutsideRailV1Candidate.TypeGuid;
        if(channel!=0&&!(channel==1&&outside))return LegacyChannelValue.Empty;
        Find(record);
        return channel==0?LegacyChannelValue.FromCurve(record,channel):LegacyChannelValue.FromNumber(0);
    }
    public void Dispose()
    {
        if(disposed)return;disposed=true;
        foreach(var output in outputs.Values)output.Dispose();outputs.Clear();
        if(pending is not null){foreach(var output in pending.Values)output.Dispose();pending=null;}
    }
}
