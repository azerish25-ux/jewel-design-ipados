using JewelDesign.Documents;
using JewelDesign.Geometry.Candidate;
namespace JewelDesign.Features.Candidate;
public sealed partial class CandidateRailGraphSession
{
 /// <summary>Diagnostics for the output's last successful native sewing; null updates retain previous output and diagnostics.</summary>
 public CandidateSewingInfo ReadSewingDiagnostics(int record)=>Find(record).Sewing??throw new NotSupportedException("Output has no successful sewing diagnostics.");
 private Output LegacyJoinOutput(int index)
 {
  var values=new List<LegacyChannelValue>();
  foreach(var link in CollectionLayout(index).OperationChannels){var data=link.ObjectIndex<0?LegacyChannelValue.Missing:Values.ReadOutput(link.ObjectIndex,link.ChannelIndex);if(!data.IsMissing)values.Add(data);}
  var flattened=LegacyArrayData.Flatten(LegacyChannelValue.FromArray(values));var shapes=new List<CandidateShape>();
  try{
   foreach(var data in flattened.Array!)if(data.Brep is{} reference)shapes.Add(RetainBrep(reference));
   // Acquired SDK5 native TL_JoinBreps returns null for Count0. Source keeps OutJoined unchanged.
   if(shapes.Count==0)return outputs.TryGetValue(index,out var previous)?previous.RetainedCopy(RetainState):new(null,null,LegacyChannelValue.Empty,identifiers:new[]{"OutJoined"});
   using var joined=context.SewBreps(shapes,.01);
   if(joined.Info.DeletedFaces!=0)throw new NotSupportedException("Legacy join deleted-face/repair behavior needs its separate original-reference contract.");
   var shape=context.Retain(joined.Shape);try{return new(shape,null,LegacyChannelValue.FromBrep(index,0),identifiers:new[]{"OutJoined"},sewing:joined.Info);}catch{shape.Dispose();throw;}
  }finally{foreach(var shape in shapes)shape.Dispose();}
 }
}
