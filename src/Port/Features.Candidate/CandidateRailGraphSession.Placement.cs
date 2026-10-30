using JewelDesign.Documents;
using JewelDesign.Geometry.Candidate;
namespace JewelDesign.Features.Candidate;

public sealed partial class CandidateRailGraphSession
{
    private CandidateShape? ResolveCurveData(LegacyChannelValue data)
    {
        if(data.Array is not null)throw new NotSupportedException("Array curve conversion requires A13-A03 first-item semantics.");
        if(data.Curve is{} reference)return ResolveCurve(reference);
        if(data.Profile is{} selection){
            using var imported=StoredMatrixProfileCandidate.Build(context,Values.Document.Records[selection.GroupRecordIndex].MatrixProfiles!,selection.ProfileIndex);
            return context.Retain(imported.Curve);
        }
        return null;
    }
    private Output PlacementOutput(int index)
    {
        var links=Inputs(index);
        LegacyChannelValue Read(int slot)=>links[slot].ObjectIndex<0?LegacyChannelValue.Missing:Values.ReadOutput(links[slot].ObjectIndex,links[slot].ChannelIndex);
        using var inner=ResolveCurveData(Read(0))??throw new NotSupportedException("Profile Placer Simple: Missing Nurbs Curve");
        using var outer=ResolveCurveData(Read(1));
        double fraction=0,outsideScale=1;
        if(!Read(3).GetDouble(ref fraction))throw new NotSupportedException("ProfilePlacerSimple: Unable to find parameter");
        Read(2).GetDouble(ref outsideScale);
        var result=ProfilePlacement2RailCandidate.Build(context,inner,outer,fraction,outsideScale,settings);
        return new(null,null,LegacyChannelValue.FromTransform(result.Transform),new[]{
            LegacyChannelValue.FromNumber(result.InnerParameter),
            result.OuterParameter is{} parameter?LegacyChannelValue.FromNumber(parameter):LegacyChannelValue.Empty,
            result.Height is{} height?LegacyChannelValue.FromNumber(height):LegacyChannelValue.Empty
        });
    }
}
