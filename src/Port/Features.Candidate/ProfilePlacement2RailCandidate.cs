using JewelDesign.Documents;
using JewelDesign.Geometry.Candidate;
namespace JewelDesign.Features.Candidate;

public sealed record ProfilePlacement2RailResult(LegacyAffineTransform Transform,double InnerParameter,
    double? OuterParameter,double? Height,double? EffectiveIntersectionTolerance,int IntersectionEventCount);

/// <summary>Adapted legacy placement over actual native curves. Kernel/order/degenerate policies remain candidate limits.</summary>
public static class ProfilePlacement2RailCandidate
{
    private static CandidateVector3 Cross(CandidateVector3 a,CandidateVector3 b)=>new(a.Y*b.Z-a.Z*b.Y,a.Z*b.X-a.X*b.Z,a.X*b.Y-a.Y*b.X);
    private static CandidateVector3 Unit(CandidateVector3 v)
    {
        double scale=Math.Max(Math.Abs(v.X),Math.Max(Math.Abs(v.Y),Math.Abs(v.Z)));
        if(!double.IsFinite(scale)||scale==0)throw new NotSupportedException("Degenerate/nonfinite placement frame; original Unitize failure policy is not established.");
        double x=v.X/scale,y=v.Y/scale,z=v.Z/scale,length=Math.Sqrt(x*x+y*y+z*z);
        return new(x/length,y/length,z/length);
    }
    public static LegacyAffineTransform OrientFrame(CandidatePoint3 point,CandidateVector3 tangent,double yScale)
    {
        if(!double.IsFinite(point.X)||!double.IsFinite(point.Y)||!double.IsFinite(point.Z))
            throw new NotSupportedException("Nonfinite placement origin.");
        // Source OrientProfileToCurveNalp + Orient3Point with source axesY/Z:
        // local columns become destination normal/radial/orthogonalized tangent.
        var direction=Unit(tangent);
        var radial=Unit(new(point.X,point.Y*yScale,point.Z));
        var normal=Unit(Cross(radial,direction));
        var up=Unit(Cross(normal,radial));
        return new(normal.X,radial.X,up.X,point.X,normal.Y,radial.Y,up.Y,point.Y,normal.Z,radial.Z,up.Z,point.Z);
    }
    public static double AdjustOuterParameter(double fraction,double innerFirst,double innerLast,double outerFirst,double outerLast,double parameter)
    {
        // Preserve source comparison of normalized fraction with native domains.
        if(Math.Abs(fraction-innerFirst)<.0001&&Math.Abs(parameter-outerLast)<.0001)parameter=outerFirst;
        if(Math.Abs(fraction-innerLast)<.0001&&Math.Abs(parameter-outerFirst)<.0001)parameter=outerLast;
        return parameter;
    }
    private static double DistanceSquared(CandidatePoint3 a,CandidatePoint3 b)=>(a.X-b.X)*(a.X-b.X)+(a.Y-b.Y)*(a.Y-b.Y)+(a.Z-b.Z)*(a.Z-b.Z);
    public static ProfilePlacement2RailResult Build(CandidateContext context,CandidateShape inner,CandidateShape? outer,
        double fraction,double outsideScale,OutsideRailCandidateSettings settings)
    {
        ArgumentNullException.ThrowIfNull(context);ArgumentNullException.ThrowIfNull(inner);ArgumentNullException.ThrowIfNull(settings);
        double parameter=context.ParameterAtNormalizedLength(inner,fraction,settings.ApplicationTolerance);
        // Source helper independently performs the same length conversion a second time.
        double frameParameter=context.ParameterAtNormalizedLength(inner,fraction,settings.ApplicationTolerance);
        var point=context.PointAt(inner,frameParameter);var tangent=context.TangentAt(inner,frameParameter);
        var transform=OrientFrame(point,tangent,outsideScale/2.0);
        if(outer is null)return new(transform,parameter,null,null,null,0);
        var end=new CandidatePoint3(point.X+30*transform.M12,point.Y+30*transform.M22,point.Z+30*transform.M32);
        var intersections=context.IntersectCurveLine(outer,point,end,settings.ApplicationTolerance);
        if(intersections.Events.Count==0)return new(transform,parameter,null,null,intersections.EffectiveFuzzyTolerance,0);
        // Transpose the curve-first adapter to line-A candidate order. Original Rhino order remains a PC gate.
        var first=intersections.Events.OrderBy(x=>x.LineStart).ThenBy(x=>x.CurveStart).First();
        var hit=first.LinePointStart;
        double outerParameter=first.CurveStart;
        if(first.Kind==CandidateCurveLineKind.Overlap&&DistanceSquared(hit,first.CurvePointEnd)<DistanceSquared(hit,first.CurvePointStart))
            outerParameter=first.CurveEnd;
        double height=Math.Sqrt(DistanceSquared(point,hit));
        if(!double.IsFinite(height))throw new NotSupportedException("Nonfinite placement intersection distance.");
        if(height<.01)height=.01;
        var innerDomain=context.InspectCurve(inner);var outerDomain=context.InspectCurve(outer);
        outerParameter=AdjustOuterParameter(fraction,innerDomain.FirstParameter,innerDomain.LastParameter,outerDomain.FirstParameter,outerDomain.LastParameter,outerParameter);
        return new(transform,parameter,outerParameter,height,intersections.EffectiveFuzzyTolerance,intersections.Events.Count);
    }
}
