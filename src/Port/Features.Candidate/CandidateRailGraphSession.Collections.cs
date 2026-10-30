using JewelDesign.Documents;
using JewelDesign.Graph;
namespace JewelDesign.Features.Candidate;
public sealed partial class CandidateRailGraphSession
{
    private LegacyTransformInputLayout CollectionLayout(int index)
    {
        var block=Values.Document.Records[index].SimpleBlock!;
        var stored=block.ObjectIndices.Select((node,i)=>new SerializedChannelLink(node,block.ChannelIndices[i])).ToArray();
        return LegacyTransformInputLayout.Reconstruct(stored,Values.Document.Records.Count,fixedInputCount:0);
    }
    public int OutputChannelCount(int index)
    {
        Check();if((uint)index>=(uint)Values.Document.Records.Count)throw new ArgumentOutOfRangeException(nameof(index));
        var record=Values.Document.Records[index];
        if(record.MatrixProfiles is{} profiles)return profiles.Curves.Count;
        if(record.Scalar is not null)return record.TypeGuid==LegacyCsg4Codec.OnOffTypeGuid?5:record.TypeGuid==LegacyCsg4Codec.ChooserTypeGuid?4:3;
        if(Supports(index))return Find(index).Channels.Count;
        throw new NotSupportedException("Unknown producer output count.");
    }
    private Output CollectionOutput(int index)
    {
        var links=CollectionLayout(index).OperationChannels;var data=new List<LegacyChannelValue>();
        bool unarray=Values.Document.Records[index].TypeGuid==LegacyCsg4Codec.UnarrayV1TypeGuid;
        foreach(var link in links){
            if(!unarray&&link.ChannelIndex==-1){
                if(link.ObjectIndex>=0){
                    int count=OutputChannelCount(link.ObjectIndex);
                    if(count>LegacyArrayData.MaximumItems-data.Count)throw new NotSupportedException("Collection wildcard exceeds explicit item limit.");
                    for(int channel=0;channel<count;channel++)data.Add(Values.ReadOutput(link.ObjectIndex,channel));
                }
                continue;
            }
            var value=link.ObjectIndex<0?LegacyChannelValue.Missing:Values.ReadOutput(link.ObjectIndex,link.ChannelIndex);
            if(unarray){
                if(value.IsMissing)throw new NullReferenceException("Legacy Unarray_V1 null input.");
                if(value.Array is{} items){if(items.Count>LegacyArrayData.MaximumItems-data.Count)throw new NotSupportedException("Unarray output exceeds explicit item limit.");data.AddRange(items);}else data.Add(value);
            }else data.Add(value);
            if(data.Count>LegacyArrayData.MaximumItems)throw new NotSupportedException("Collection output count exceeds explicit limit.");
        }
        if(unarray){if(data.Count>=LegacyArrayData.MaximumItems)throw new NotSupportedException("Unarray trailing output exceeds explicit item limit.");data.Add(LegacyChannelValue.FromArray(Array.Empty<LegacyChannelValue>()));return RelayOutput(index,data);}
        return RelayOutput(index,new[]{LegacyChannelValue.FromArray(data)});
    }
}
