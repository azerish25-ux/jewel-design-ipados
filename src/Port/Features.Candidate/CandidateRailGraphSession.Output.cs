using JewelDesign.Documents;
using JewelDesign.Geometry.Candidate;
namespace JewelDesign.Features.Candidate;

public sealed partial class CandidateRailGraphSession
{
    private sealed class Output:IDisposable
    {
        internal IReadOnlyList<LegacyChannelValue> Channels {get;}
        private readonly Dictionary<(int Channel,int Item),CandidateCurveState> curves;
        internal Output(CandidateShape? curve,CandidateRailOutputInfo? info,LegacyChannelValue data,
            IReadOnlyList<LegacyChannelValue>? additional=null,CandidateProfileOutputInfo? profile=null,LegacyAffineTransform? applied=null)
        {
            Channels=Array.AsReadOnly(new[]{data}.Concat(additional??Array.Empty<LegacyChannelValue>()).ToArray());
            curves=new();
            if(curve is not null)curves.Add((0,0),new(curve,info,profile,applied??LegacyAffineTransform.Identity));
        }
        internal Output(IReadOnlyList<LegacyChannelValue> channels,Dictionary<(int,int),CandidateCurveState> curves)
        {Channels=Array.AsReadOnly(channels.ToArray());this.curves=curves;}
        internal LegacyChannelValue Read(int channel)=>(uint)channel<(uint)Channels.Count?Channels[channel]:LegacyChannelValue.Empty;
        internal CandidateCurveState Curve(int channel,int item)=>curves.TryGetValue((channel,item),out var curve)?curve:throw new NotSupportedException("Output item has no owned native curve.");
        public void Dispose(){foreach(var curve in curves.Values)curve.Dispose();curves.Clear();}
    }
    private CandidateCurveState FindCurve(LegacyCurveSelection reference)=>Find(reference.ProducerRecordIndex).Curve(reference.OutputChannelIndex,reference.ItemIndex);
    private CandidateCurveState RetainState(CandidateCurveState source)
    {
        var shape=context.Retain(source.Curve);
        try{return new(shape,source.Rail,source.Profile,source.AppliedTransform);}catch{shape.Dispose();throw;}
    }
    private CandidateCurveState CaptureCurve(LegacyChannelValue data)
    {
        if(data.Curve is{} reference)return RetainState(FindCurve(reference));
        var selection=data.Profile??throw new NotSupportedException("Not curve data.");
        var group=Values.Document.Records[selection.GroupRecordIndex].MatrixProfiles!;
        var metadata=new CandidateProfileOutputInfo(selection.GroupRecordIndex,selection.ProfileIndex,selection.ProfileIndex<group.Names.Count?group.Names[selection.ProfileIndex]:null,group.SweepParameters[selection.ProfileIndex]);
        using var profile=StoredMatrixProfileCandidate.Build(context,group,selection.ProfileIndex);
        var curve=context.Retain(profile.Curve);
        try{return new(curve,null,metadata,LegacyAffineTransform.Identity);}catch{curve.Dispose();throw;}
    }
    public CandidateDataSnapshot CaptureOutput(int record,int channel=0)
    {Check();return CaptureValue(ReadOutput(record,channel));}
    /// <summary>Capture graph-local value references now; bare tokens address the current producer cache, not an earlier generation.</summary>
    public CandidateDataSnapshot CaptureValue(LegacyChannelValue value)
    {Check();ArgumentNullException.ThrowIfNull(value);return CandidateDataSnapshot.Capture(context,value,CaptureCurve);}
    private Output RelayOutput(int index,IReadOnlyList<LegacyChannelValue> values)
    {
        var curves=new Dictionary<(int,int),CandidateCurveState>();var channels=new List<LegacyChannelValue>();int visited=0;
        try{
            for(int channel=0;channel<values.Count;channel++){
                int nextItem=1;
                LegacyChannelValue Copy(LegacyChannelValue value,int depth){
                    if(depth>LegacyArrayData.MaximumDepth||++visited>LegacyArrayData.MaximumItems)throw new NotSupportedException("Native output exceeds explicit collection limits.");
                    if(value.Curve is{} reference){
                        int item=depth==0?0:nextItem++;var token=LegacyChannelValue.FromCurve(index,channel,item);
                        var state=RetainState(FindCurve(reference));
                        try{curves.Add((channel,item),state);}catch{state.Dispose();throw;}
                        return token;
                    }
                    return value.Array is{} children?LegacyChannelValue.FromArray(children.Select(x=>Copy(x,depth+1))):value;
                }
                channels.Add(Copy(values[channel],0));
            }
            return new(channels,curves);
        }catch{foreach(var curve in curves.Values)curve.Dispose();throw;}
    }
}
