using JewelDesign.Documents;
namespace JewelDesign.Features.Candidate;
public sealed partial class CandidateRailGraphSession
{
 private Output SweepOutput(int index)
 {
  using var prepared=Sweep2PreparationCandidate.FromNodeCore(context,this,index,false);
  if(prepared.EffectiveClosed)throw new NotSupportedException("Closed-path Sweep2 requires its separate native/source contract.");
  if(prepared.Profiles.Count<2)throw new NotSupportedException("Single-profile Sweep2 requires its separate native/source contract.");
  var shape=context.CreatePairedSweep(prepared.Rail1,prepared.Rail2,prepared.Profiles,prepared.Parameters1,prepared.Parameters2,prepared.MaintainHeight,settings.ApplicationTolerance);
  // This candidate returns one actual BRep. Full Rhino split/topology policy remains unverified.
  try{return new(shape,null,LegacyChannelValue.FromBrep(index,0),identifiers:new[]{Values.Document.Records[index].SimpleBlock!.Identifier+"_0"});}
  catch{shape.Dispose();throw;}
 }
}
