using JewelDesign.Documents;
using JewelDesign.Geometry.Candidate;
namespace JewelDesign.Features.Candidate;
public sealed partial class CandidateRailGraphSession
{
 private sealed class Output:IDisposable
 {
  internal IReadOnlyList<LegacyChannelValue> Channels {get;}
  internal CandidateSewingInfo? Sewing {get;}
  private readonly Dictionary<(int Channel,int Item),CandidateGeometryState> geometry;
  private readonly IReadOnlyList<string>? identifiers;
  internal Output(CandidateShape? shape,CandidateRailOutputInfo? info,LegacyChannelValue data,IReadOnlyList<LegacyChannelValue>? additional=null,CandidateProfileOutputInfo? profile=null,LegacyAffineTransform? applied=null,IReadOnlyList<string>? identifiers=null,CandidateSewingInfo? sewing=null){
   Channels=Array.AsReadOnly(new[]{data}.Concat(additional??Array.Empty<LegacyChannelValue>()).ToArray());geometry=new();Sewing=sewing;this.identifiers=identifiers is null?null:Array.AsReadOnly(identifiers.ToArray());
   if(shape is not null)geometry.Add((0,0),new(shape,info,profile,applied??LegacyAffineTransform.Identity,data.Brep.HasValue?CandidateGeometryKind.Brep:CandidateGeometryKind.Curve));
  }
  internal Output(IReadOnlyList<LegacyChannelValue> channels,Dictionary<(int,int),CandidateGeometryState> geometry,IReadOnlyList<string>? identifiers=null,CandidateSewingInfo? sewing=null){Channels=Array.AsReadOnly(channels.ToArray());this.geometry=geometry;this.identifiers=identifiers is null?null:Array.AsReadOnly(identifiers.ToArray());Sewing=sewing;}
  internal Output RetainedCopy(Func<CandidateGeometryState,CandidateGeometryState> retain){var owned=new Dictionary<(int,int),CandidateGeometryState>();try{foreach(var pair in geometry){var state=retain(pair.Value);try{owned.Add(pair.Key,state);}catch{state.Dispose();throw;}}return new(Channels,owned,identifiers,Sewing);}catch{foreach(var state in owned.Values)state.Dispose();throw;}}
  internal LegacyChannelValue Read(int channel)=>(uint)channel<(uint)Channels.Count?Channels[channel]:LegacyChannelValue.Empty;
  internal string Identifier(int channel)=>identifiers is not null&&(uint)channel<(uint)identifiers.Count?identifiers[channel]:throw new NotSupportedException("Output identifier is not represented for this producer/channel.");
  internal CandidateGeometryState Geometry(int channel,int item,CandidateGeometryKind kind){var state=geometry.TryGetValue((channel,item),out var value)?value:throw new NotSupportedException("Output item has no owned native geometry.");return state.Kind==kind?state:throw new NotSupportedException("Output geometry kind does not match reference.");}
  public void Dispose(){foreach(var state in geometry.Values)state.Dispose();geometry.Clear();}
 }
 private CandidateGeometryState FindCurve(LegacyCurveSelection reference)=>Find(reference.ProducerRecordIndex).Geometry(reference.OutputChannelIndex,reference.ItemIndex,CandidateGeometryKind.Curve);
 private CandidateGeometryState FindBrep(LegacyBrepSelection reference)=>Find(reference.ProducerRecordIndex).Geometry(reference.OutputChannelIndex,reference.ItemIndex,CandidateGeometryKind.Brep);
 private CandidateGeometryState RetainState(CandidateGeometryState source){var shape=context.Retain(source.Shape);try{return new(shape,source.Rail,source.Profile,source.AppliedTransform,source.Kind);}catch{shape.Dispose();throw;}}
 private CandidateGeometryState CaptureGeometry(LegacyChannelValue data){
  if(data.Curve is{} reference)return RetainState(FindCurve(reference));
  if(data.Brep is{} brep)return RetainState(FindBrep(brep));
  var selection=data.Profile??throw new NotSupportedException("Not represented geometry data.");var group=Values.Document.Records[selection.GroupRecordIndex].MatrixProfiles!;
  var metadata=new CandidateProfileOutputInfo(selection.GroupRecordIndex,selection.ProfileIndex,selection.ProfileIndex<group.Names.Count?group.Names[selection.ProfileIndex]:null,group.SweepParameters[selection.ProfileIndex]);
  using var profile=StoredMatrixProfileCandidate.Build(context,group,selection.ProfileIndex);var curve=context.Retain(profile.Curve);try{return new(curve,null,metadata,LegacyAffineTransform.Identity);}catch{curve.Dispose();throw;}
 }
 public CandidateDataSnapshot CaptureOutput(int record,int channel=0){Check();return CaptureValue(ReadOutput(record,channel));}
 /// <summary>Retain referenced geometry now; bare typed tokens address the current producer cache.</summary>
 public CandidateDataSnapshot CaptureValue(LegacyChannelValue value){Check();ArgumentNullException.ThrowIfNull(value);return CandidateDataSnapshot.Capture(context,value,CaptureGeometry);}
 public CandidateShape RetainBrep(int record,int channel=0,int item=0)=>RetainBrep(new LegacyBrepSelection(record,channel,item));
 public CandidateShape RetainBrep(LegacyBrepSelection reference){Check();return context.Retain(FindBrep(reference).Brep);}
 public string ReadOutputIdentifier(int record,int channel=0){Check();return Find(record).Identifier(channel);}
 private Output RelayOutput(int index,IReadOnlyList<LegacyChannelValue> values){
  var geometry=new Dictionary<(int,int),CandidateGeometryState>();var channels=new List<LegacyChannelValue>();int visited=0;
  try{
   for(int channel=0;channel<values.Count;channel++){int nextItem=1;
    LegacyChannelValue Copy(LegacyChannelValue value,int depth){
     if(depth>LegacyArrayData.MaximumDepth||++visited>LegacyArrayData.MaximumItems)throw new NotSupportedException("Native output exceeds explicit collection limits.");
     if(value.Curve.HasValue||value.Brep.HasValue){int item=depth==0?0:nextItem++;var state=CaptureGeometry(value);try{geometry.Add((channel,item),state);}catch{state.Dispose();throw;}return state.Kind==CandidateGeometryKind.Curve?LegacyChannelValue.FromCurve(index,channel,item):LegacyChannelValue.FromBrep(index,channel,item);}
     return value.Array is{} children?LegacyChannelValue.FromArray(children.Select(x=>Copy(x,depth+1))):value;
    }
    channels.Add(Copy(values[channel],0));
   }
   return new(channels,geometry);
  }catch{foreach(var state in geometry.Values)state.Dispose();throw;}
 }
}
