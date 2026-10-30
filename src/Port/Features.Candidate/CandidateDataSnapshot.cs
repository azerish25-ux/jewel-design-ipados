using JewelDesign.Documents;
using JewelDesign.Geometry.Candidate;
namespace JewelDesign.Features.Candidate;

internal sealed class CandidateCurveState:IDisposable
{
    internal CandidateShape Curve {get;}
    internal CandidateRailOutputInfo? Rail {get;}
    internal CandidateProfileOutputInfo? Profile {get;}
    internal LegacyAffineTransform AppliedTransform {get;}
    internal CandidateCurveState(CandidateShape curve,CandidateRailOutputInfo? rail,CandidateProfileOutputInfo? profile,LegacyAffineTransform applied)
    {Curve=curve;Rail=rail;Profile=profile;AppliedTransform=applied;}
    public void Dispose()=>Curve.Dispose();
}

/// <summary>Explicit retained collection snapshot. Curve operations require the original live CandidateContext.</summary>
public sealed class CandidateDataSnapshot:IDisposable
{
    private readonly CandidateContext context;
    private readonly Dictionary<LegacyChannelValue,CandidateCurveState> curves;
    private bool disposed;
    public LegacyChannelValue Value {get;}
    private CandidateDataSnapshot(CandidateContext context,LegacyChannelValue value,Dictionary<LegacyChannelValue,CandidateCurveState> curves)
    {this.context=context;Value=value;this.curves=curves;}
    internal static CandidateDataSnapshot Capture(CandidateContext context,LegacyChannelValue value,Func<LegacyChannelValue,CandidateCurveState> retain)
    {
        var curves=new Dictionary<LegacyChannelValue,CandidateCurveState>();
        try{
            var stack=new Stack<(LegacyChannelValue Value,int Depth)>();stack.Push((value,0));int visited=0;
            while(stack.Count>0){
                var (item,depth)=stack.Pop();
                if(depth>LegacyArrayData.MaximumDepth||++visited>LegacyArrayData.MaximumItems)throw new NotSupportedException("Native snapshot exceeds explicit collection limits.");
                if(item.Array is{} children){
                    if(children.Count>LegacyArrayData.MaximumItems-visited-stack.Count)throw new NotSupportedException("Pending native snapshot exceeds explicit collection limits.");
                    for(int i=children.Count-1;i>=0;i--)stack.Push((children[i],depth+1));
                }else if((item.Curve.HasValue||item.Profile.HasValue)&&!curves.ContainsKey(item)){
                    var state=retain(item);try{curves.Add(item,state);}catch{state.Dispose();throw;}
                }
            }
            return new(context,value,curves);
        }catch{foreach(var state in curves.Values)state.Dispose();throw;}
    }
    private CandidateCurveState Find(LegacyChannelValue value)
    {if(disposed)throw new ObjectDisposedException(nameof(CandidateDataSnapshot));return curves.TryGetValue(value,out var curve)?curve:throw new NotSupportedException("Value is not a captured curve leaf.");}
    public CandidateShape RetainCurve(LegacyChannelValue value)=>context.Retain(Find(value).Curve);
    public CandidateProfileOutputInfo ReadProfileMetadata(LegacyChannelValue value)=>Find(value).Profile??throw new NotSupportedException("Leaf is not profile data.");
    public CandidateRailOutputInfo ReadRailMetadata(LegacyChannelValue value)=>Find(value).Rail??throw new NotSupportedException("Leaf is not rail data.");
    public LegacyAffineTransform ReadTransform(LegacyChannelValue value)=>Find(value).AppliedTransform;
    public void Dispose(){if(disposed)return;disposed=true;foreach(var curve in curves.Values)curve.Dispose();curves.Clear();}
}
