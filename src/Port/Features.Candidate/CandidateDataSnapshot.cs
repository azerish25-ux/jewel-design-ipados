using JewelDesign.Documents;
using JewelDesign.Geometry.Candidate;
namespace JewelDesign.Features.Candidate;

internal enum CandidateGeometryKind {Curve,Brep}
internal sealed class CandidateGeometryState:IDisposable
{
    internal CandidateShape Shape {get;}
    internal CandidateGeometryKind Kind {get;}
    internal CandidateShape Curve=>Kind==CandidateGeometryKind.Curve?Shape:throw new NotSupportedException("BRep data is not curve data.");
    internal CandidateShape Brep=>Kind==CandidateGeometryKind.Brep?Shape:throw new NotSupportedException("Curve data is not BRep data.");
    internal CandidateRailOutputInfo? Rail {get;}
    internal CandidateProfileOutputInfo? Profile {get;}
    internal LegacyAffineTransform AppliedTransform {get;}
    internal CandidateGeometryState(CandidateShape curve,CandidateRailOutputInfo? rail,CandidateProfileOutputInfo? profile,LegacyAffineTransform applied,CandidateGeometryKind kind=CandidateGeometryKind.Curve)
    {Shape=curve;Kind=kind;Rail=rail;Profile=profile;AppliedTransform=applied;}
    public void Dispose()=>Shape.Dispose();
}

/// <summary>Explicit retained collection snapshot. Geometry operations require the original live CandidateContext.</summary>
public sealed class CandidateDataSnapshot:IDisposable
{
    private readonly CandidateContext context;
    private readonly Dictionary<LegacyChannelValue,CandidateGeometryState> curves;
    private bool disposed;
    public LegacyChannelValue Value {get;}
    private CandidateDataSnapshot(CandidateContext context,LegacyChannelValue value,Dictionary<LegacyChannelValue,CandidateGeometryState> curves)
    {this.context=context;Value=value;this.curves=curves;}
    internal static CandidateDataSnapshot Capture(CandidateContext context,LegacyChannelValue value,Func<LegacyChannelValue,CandidateGeometryState> retain)
    {
        var curves=new Dictionary<LegacyChannelValue,CandidateGeometryState>();
        try{
            var stack=new Stack<(LegacyChannelValue Value,int Depth)>();stack.Push((value,0));int visited=0;
            while(stack.Count>0){
                var (item,depth)=stack.Pop();
                if(depth>LegacyArrayData.MaximumDepth||++visited>LegacyArrayData.MaximumItems)throw new NotSupportedException("Native snapshot exceeds explicit collection limits.");
                if(item.Array is{} children){
                    if(children.Count>LegacyArrayData.MaximumItems-visited-stack.Count)throw new NotSupportedException("Pending native snapshot exceeds explicit collection limits.");
                    for(int i=children.Count-1;i>=0;i--)stack.Push((children[i],depth+1));
                }else if((item.Curve.HasValue||item.Profile.HasValue||item.Brep.HasValue)&&!curves.ContainsKey(item)){
                    var state=retain(item);try{curves.Add(item,state);}catch{state.Dispose();throw;}
                }
            }
            return new(context,value,curves);
        }catch{foreach(var state in curves.Values)state.Dispose();throw;}
    }
    private CandidateGeometryState Find(LegacyChannelValue value)
    {if(disposed)throw new ObjectDisposedException(nameof(CandidateDataSnapshot));return curves.TryGetValue(value,out var curve)?curve:throw new NotSupportedException("Value is not a captured geometry leaf.");}
    public CandidateShape RetainCurve(LegacyChannelValue value)=>context.Retain(Find(value).Curve);
    public CandidateShape RetainBrep(LegacyChannelValue value)=>context.Retain(Find(value).Brep);
    public CandidateProfileOutputInfo ReadProfileMetadata(LegacyChannelValue value)=>Find(value).Profile??throw new NotSupportedException("Leaf is not profile data.");
    public CandidateRailOutputInfo ReadRailMetadata(LegacyChannelValue value)=>Find(value).Rail??throw new NotSupportedException("Leaf is not rail data.");
    public LegacyAffineTransform ReadTransform(LegacyChannelValue value)=>Find(value).AppliedTransform;
    public void Dispose(){if(disposed)return;disposed=true;foreach(var curve in curves.Values)curve.Dispose();curves.Clear();}
}
