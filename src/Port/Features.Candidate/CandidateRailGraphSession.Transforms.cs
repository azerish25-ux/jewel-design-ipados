using JewelDesign.Documents;
using JewelDesign.Geometry.Candidate;
namespace JewelDesign.Features.Candidate;

public sealed partial class CandidateRailGraphSession
{
    private Output TransformOutput(int index)
    {
        var links=Inputs(index);
        LegacyChannelValue Read(int slot)=>links[slot].ObjectIndex<0?LegacyChannelValue.Missing:Values.ReadOutput(links[slot].ObjectIndex,links[slot].ChannelIndex);
        if(Values.Document.Records[index].TypeGuid==LegacyCsg4Codec.AddTypeGuid)
            return new(null,null,LegacyChannelValue.FromNumber(TransformProducerCandidate.AddScalars(Read(0),Read(1))));
        LegacyAffineTransform transform;
        if(Values.Document.Records[index].TypeGuid==LegacyCsg4Codec.RotateObjectTypeGuid)
            transform=RotationProducerCandidate.Build(Read(0),Read(1),Read(2),Read(3));
        else if(Values.Document.Records[index].TypeGuid==LegacyCsg4Codec.TranslateObjectTypeGuid)
            transform=TransformProducerCandidate.Translation(Read(0),Read(1),Read(2),Read(3));
        else{
            var data=Read(0);
            var curve=ResolveCurveData(data)??throw new NotSupportedException("ScaleObjectToSize requires supported object bounds; source UpdateMissing(Object/Bounding Box).");
            using(curve){
                var bounds=context.GetCurveBounds(curve,settings.BoundsTolerance);
                transform=TransformProducerCandidate.ScaleToSize(bounds,Read(1),Read(2),Read(3));
            }
        }
        return new(null,null,LegacyChannelValue.FromTransform(transform));
    }
}
