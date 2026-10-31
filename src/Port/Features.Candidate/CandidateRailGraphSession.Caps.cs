using JewelDesign.Documents;
namespace JewelDesign.Features.Candidate;
public sealed partial class CandidateRailGraphSession
{
 private Output ProfileCapOutput(int index)
 {
  var links=Inputs(index);
  LegacyChannelValue Read(int slot)=>links[slot].ObjectIndex<0?LegacyChannelValue.Missing:Values.ReadOutput(links[slot].ObjectIndex,links[slot].ChannelIndex);
  // CSDataHelper.GetNurbsCurve flattens first, then converts only its first leaf.
  var input=Read(0);IReadOnlyList<LegacyChannelValue> leaves=input.Array is null?new[]{input}:LegacyArrayData.Flatten(input).Array!;
  using var curve=leaves.Count==0?null:ResolveCurveData(leaves[0]);
  if(curve is null)throw new NotSupportedException("ProfileEndCap Missing Nurbscurve");
  if(!CurvePlaneCandidate.TryAveragePlane(context,curve,out var plane))throw new NotSupportedException("ProfileEndCap missing");
  double height=.5;Read(1).GetDouble(ref height);
  using var cap=ProfileCapWithPlaneCandidate.Build(context,curve,plane,height,settings.ApplicationTolerance);
  var shape=context.Retain(cap.Shape);
  // Geometry is already world-placed; source stores inverse metadata with applyTransform:false.
  try{return new(shape,null,LegacyChannelValue.FromBrep(index,0),applied:cap.InverseTransform,identifiers:new[]{"oBrep"});}
  catch{shape.Dispose();throw;}
 }
}
