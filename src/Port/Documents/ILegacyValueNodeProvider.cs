using JewelDesign.Graph;
namespace JewelDesign.Documents;

/// <summary>Extension for explicitly supported producers; implementations own their runtime/native state.</summary>
public interface ILegacyValueNodeProvider
{
    bool Supports(int recordIndex);
    IReadOnlyList<SerializedChannelLink> Inputs(int recordIndex);
    void Update(int recordIndex);
    LegacyChannelValue ReadOutput(int recordIndex,int channelIndex);
}
public readonly record struct LegacyCurveSelection(int ProducerRecordIndex,int OutputChannelIndex,int ItemIndex=0);
