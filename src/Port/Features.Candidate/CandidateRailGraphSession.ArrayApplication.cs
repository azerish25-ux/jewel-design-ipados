using JewelDesign.Documents;
using JewelDesign.Geometry.Candidate;
namespace JewelDesign.Features.Candidate;
public sealed partial class CandidateRailGraphSession
{
    private Output ApplyOutput(int index,LegacyChannelValue value,LegacyAffineTransform combined)
    {
        var curves=new Dictionary<(int,int),CandidateCurveState>();int nextItem=1,visited=0;
        bool apply=combined.IsFinite;
        try{
            LegacyChannelValue Copy(LegacyChannelValue data,int depth){
                if(depth>LegacyArrayData.MaximumDepth||++visited>LegacyArrayData.MaximumItems)throw new NotSupportedException("Array application exceeds explicit collection limits.");
                if(data.IsMissing)return LegacyChannelValue.Empty; // CSDataArray copy turns null members into empty data.
                if(data.Array is{} items)return LegacyChannelValue.FromArray(items.Select(x=>Copy(x,depth+1)));
                if(data.Profile.HasValue||data.Curve.HasValue){
                    using var source=CaptureCurve(data);
                    var applied=source.AppliedTransform;
                    if(apply)applied=source.Profile is null?applied*combined:combined*applied;
                    int item=depth==0?0:nextItem++;var token=LegacyChannelValue.FromCurve(index,0,item);
                    var curve=context.TransformAffine(source.Curve,NativeAffine(apply?combined:LegacyAffineTransform.Identity));
                    CandidateCurveState state;
                    try{state=new(curve,source.Rail,source.Profile,applied);}catch{curve.Dispose();throw;}
                    try{curves.Add((0,item),state);}catch{state.Dispose();throw;}
                    return token;
                }
                if(data.Transform is{} original&&apply)
                    return LegacyChannelValue.FromTransform(original.IsFinite?original*combined:combined);
                return data;
            }
            var copied=Copy(value,0);
            return new(new[]{copied},curves);
        }catch{foreach(var curve in curves.Values)curve.Dispose();throw;}
    }
    private CandidateCurveBounds? DataBounds(LegacyChannelValue data)
    {
        var leaves=data.Array is null?new[]{data}:LegacyArrayData.Flatten(data).Array!;
        CandidateCurveBounds? result=null;
        foreach(var leaf in leaves){
            if(!leaf.Curve.HasValue&&!leaf.Profile.HasValue)continue;
            using var curve=ResolveCurveData(leaf)!;var next=context.GetCurveBounds(curve,settings.BoundsTolerance);
            if(result is not{} current){result=next;continue;}
            result=new(new(Math.Min(current.Minimum.X,next.Minimum.X),Math.Min(current.Minimum.Y,next.Minimum.Y),Math.Min(current.Minimum.Z,next.Minimum.Z)),
                new(Math.Max(current.Maximum.X,next.Maximum.X),Math.Max(current.Maximum.Y,next.Maximum.Y),Math.Max(current.Maximum.Z,next.Maximum.Z)),
                settings.BoundsTolerance,Math.Max(current.KernelPadding,next.KernelPadding));
        }
        return result;
    }
}
