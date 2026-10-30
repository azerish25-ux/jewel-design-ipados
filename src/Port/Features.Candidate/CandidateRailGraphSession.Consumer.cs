using JewelDesign.Documents;
using JewelDesign.Geometry.Candidate;
namespace JewelDesign.Features.Candidate;

public sealed partial class CandidateRailGraphSession
{
    private LegacyTransformInputLayout ConsumerLayout(int index)
    {
        var block=Values.Document.Records[index].SimpleBlock!;
        var stored=block.ObjectIndices.Select((node,i)=>new JewelDesign.Graph.SerializedChannelLink(node,block.ChannelIndices[i])).ToArray();
        return LegacyTransformInputLayout.Reconstruct(stored,Values.Document.Records.Count);
    }
    public CandidateProfileOutputInfo ReadProfileMetadata(int record,int channel=0,int item=0)=>FindCurve(new(record,channel,item)).Profile??throw new NotSupportedException("Output has no profile metadata.");
    public bool TryReadTransform(int record,int channel,out LegacyAffineTransform transform)
    {
        var data=ReadOutput(record,channel);
        if(data.Array is{} items){
            if(items.Count==0||items[0].Array is not null){transform=default;return false;}
            data=items[0];
        }
        return TryDataTransform(data,out transform);
    }
    private bool TryDataTransform(LegacyChannelValue data,out LegacyAffineTransform transform)
    {
        if(data.Transform is{} matrix){transform=matrix;return matrix.IsFinite;}
        if(data.Profile.HasValue){transform=LegacyAffineTransform.Identity;return true;}
        if(data.Curve is{} reference){transform=FindCurve(reference).AppliedTransform;return transform.IsFinite;}
        transform=default;return false;
    }
    private Output ConsumeTransform(int index)
    {
        var links=ConsumerLayout(index).OperationChannels;
        LegacyChannelValue Read(int slot)=>links[slot].ObjectIndex<0?LegacyChannelValue.Missing:Values.ReadOutput(links[slot].ObjectIndex,links[slot].ChannelIndex);
        var data=Read(0);if(data.IsMissing)throw new NotSupportedException("TransformObject: Missing Object");
        var transformInputs=new List<LegacyChannelValue>();
        for(int i=1;i<links.Count;i++){var input=Read(i);if(!input.IsMissing)transformInputs.Add(input);}
        var flattened=LegacyArrayData.Flatten(LegacyChannelValue.FromArray(transformInputs));
        var combined=LegacyAffineTransform.Identity;
        foreach(var input in flattened.Array!){
            if(TryDataTransform(input,out var matrix)&&combined.IsFinite)combined=matrix*combined;
        }
        return ApplyOutput(index,data,combined);
    }

    private static CandidateAffine NativeAffine(LegacyAffineTransform m)=>new(m.M11,m.M12,m.M13,m.M14,m.M21,m.M22,m.M23,m.M24,m.M31,m.M32,m.M33,m.M34);
}
