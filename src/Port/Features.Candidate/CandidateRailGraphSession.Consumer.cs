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
        if(data.Array is not null)throw new NotSupportedException("Array transform capabilities require A13-A03.");
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
        var data=Read(0);if(data.Array is not null)throw new NotSupportedException("Array object transforms require A13-A03 ownership/application integration.");if(data.IsMissing)throw new NotSupportedException("TransformObject: Missing Object");
        var combined=LegacyAffineTransform.Identity;
        for(int i=1;i<links.Count;i++){
            var input=Read(i);if(input.Array is not null)throw new NotSupportedException("Array transform flattening requires A13-A03.");
            if(TryDataTransform(input,out var matrix)&&combined.IsFinite)combined=matrix*combined;
            // Once the product is nonfinite, later finite products cannot make it valid.
        }
        bool apply=combined.IsFinite;
        if(data.Profile.HasValue||data.Curve.HasValue){
            CandidateProfileOutputInfo? profile=null;CandidateRailOutputInfo? rail=null;
            var old=LegacyAffineTransform.Identity;
            if(data.Profile is{} stored){
                var group=Values.Document.Records[stored.GroupRecordIndex].MatrixProfiles!;
                profile=new(stored.GroupRecordIndex,stored.ProfileIndex,stored.ProfileIndex<group.Names.Count?group.Names[stored.ProfileIndex]:null,group.SweepParameters[stored.ProfileIndex]);
            }else{
                var source=FindCurve(data.Curve!.Value);profile=source.Profile;rail=source.Rail;old=source.AppliedTransform;
            }
            var applied=old;
            if(apply)applied=profile is null?old*combined:combined*old;
            var outputData=LegacyChannelValue.FromCurve(index,0);
            using var original=ResolveCurveData(data)!;
            var curve=context.TransformAffine(original,NativeAffine(apply?combined:LegacyAffineTransform.Identity));
            return Own(curve,rail,outputData,profile,applied);
        }
        if(data.Transform is{} sourceTransform&&apply){
            var transformed=sourceTransform.IsFinite?sourceTransform*combined:combined;
            return new(null,null,LegacyChannelValue.FromTransform(transformed));
        }
        // CSNumberData/empty CSData Duplicate is not transformable; source preserves the value.
        return new(null,null,data);
    }
    private static CandidateAffine NativeAffine(LegacyAffineTransform m)=>new(m.M11,m.M12,m.M13,m.M14,m.M21,m.M22,m.M23,m.M24,m.M31,m.M32,m.M33,m.M34);
}
