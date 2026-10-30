using JewelDesign.Geometry.Candidate;
namespace JewelDesign.Features.Candidate;

/// <summary>Adapted OutsideRail_V1 geometry body; channels, chooser evaluation and CSBlock lifecycle are separate.</summary>
public static partial class OutsideRailV1Candidate
{
    public static readonly Guid TypeGuid=new("f9f6400f-67c2-4883-8c44-a91f411f2ee3");
    public static OutsideRailCandidate Build(CandidateContext context,CandidateShape? outsideCurve,double? numericRadius,
        OutsideRailCandidateSettings settings,OutsideRailV1Parameters? parameters=null,CandidateShape? radiusCurve=null)
    {
        ArgumentNullException.ThrowIfNull(context);ArgumentNullException.ThrowIfNull(settings);
        if(outsideCurve is null)throw new OutsideRailCandidateException("Outside Ring Rail: Missing Custom Rail (No Curve)");
        foreach(var tolerance in new[]{settings.HelperTolerance,settings.ApplicationTolerance,settings.JoinTolerance,settings.BoundsTolerance})
            if(!double.IsFinite(tolerance)||tolerance<=0)throw new ArgumentOutOfRangeException(nameof(settings),"Candidate tolerances must be finite positive.");
        var p=parameters??new OutsideRailV1Parameters();
        double radius;
        if(numericRadius.HasValue)radius=numericRadius.Value;
        else if(radiusCurve is null)throw new OutsideRailCandidateException("Missing Ring Rail");
        else radius=context.GetCurveBounds(radiusCurve,settings.BoundsTolerance).Maximum.Z;
        using var owned=new CurveScope();
        var diagnostics=new List<OutsideRailKernelDiagnostic>();
        var curve=owned.Add(context.Clone(outsideCurve));
        double minimum=double.MaxValue;
        for(int i=0;i<50;i++){
            double parameter=context.ParameterAtNormalizedLength(curve,i/50.0,settings.HelperTolerance);
            var point=context.PointAt(curve,parameter);double distance=Math.Sqrt(point.X*point.X+point.Y*point.Y+point.Z*point.Z);
            if(distance<minimum)minimum=distance;
        }
        if(minimum==0||minimum==double.MaxValue||radius==0)throw new OutsideRailCandidateException("Intersect of custom Rail Failed");
        double uniform=radius/minimum;
        curve=owned.Add(context.TransformAffine(curve,Scale(uniform,uniform,uniform)));
        var extents=Extents(context,curve,settings.HelperTolerance);
        var join=OutsideRailJoinDisposition.NotUsed;
        if(!p.LockOn){
            double stretch=(radius+p.TopThickness-extents.Bottom)/(extents.Top-extents.Bottom);
            curve=owned.Add(context.TransformAffine(curve,Scale(1,1,stretch,extents.Bottom)));
            extents=Extents(context,curve,settings.HelperTolerance);
            stretch=(extents.Top-(-radius-p.BottomThickness))/(extents.Top-extents.Bottom);
            curve=owned.Add(context.TransformAffine(curve,Scale(1,1,stretch,extents.Top)));
            extents=Extents(context,curve,settings.HelperTolerance);
            curve=owned.Add(context.TransformAffine(curve,Scale((radius+p.SideThickness)/extents.Right,1,1)));
        }else{
            curve=owned.Add(context.TransformAffine(curve,Scale((radius+p.SideThickness)/extents.Right,1,1)));
            var crossings=context.IntersectCurveLine(curve,new(-200,0,0),new(200,0,0),settings.ApplicationTolerance).Events;
            if(crossings.Count!=2)throw new OutsideRailCandidateException("Intersect of custom Rail Failed");
            var forward=ChangeSeam(context,owned,owned.Add(context.Clone(curve)),crossings[0].CurveStart,"split seam",diagnostics);
            var backward=owned.Add(context.ReverseCurve(forward));
            double end=context.ClosestPoint(forward,crossings[1].CurvePointStart).Parameter;
            forward=owned.Add(context.TrimCurve(forward,context.InspectCurve(forward).FirstParameter,end));
            end=context.ClosestPoint(backward,crossings[1].CurvePointStart).Parameter;
            backward=owned.Add(context.TrimCurve(backward,context.InspectCurve(backward).FirstParameter,end));
            var domain=context.InspectCurve(forward);
            if(context.PointAt(forward,(domain.FirstParameter+domain.LastParameter)/2).Z<0)(forward,backward)=(backward,forward);
            forward=owned.Add(context.TransformAffine(forward,Scale(1,1,(p.TopThickness+radius)/extents.Top)));
            backward=owned.Add(context.TransformAffine(backward,Scale(1,1,(0.0-p.BottomThickness+(-radius))/extents.Bottom)));
            var joined=MergePair(context,owned,forward,backward,settings.JoinTolerance,diagnostics,out join);
            // Preserve the recovered first-result/empty-result selection.
            if(joined.Length!=0)curve=joined[0];
            var finalHits=context.IntersectCurveLine(curve,new(0,0,0),new(0,0,100),settings.ApplicationTolerance).Events;
            double seam=finalHits.Count>0?finalHits[0].CurveStart:context.ClosestPoint(curve,new(0,0,100)).Parameter;
            curve=ChangeSeam(context,owned,curve,seam,"final seam",diagnostics);
            if(context.TangentAt(curve,context.InspectCurve(curve).FirstParameter).X<0)curve=owned.Add(context.ReverseCurve(curve));
        }
        var result=owned.Detach(curve);
        try{return new OutsideRailCandidate(result,radius,minimum,join,diagnostics);}
        catch{result.Dispose();throw;}
    }

    private readonly record struct AxisExtents(double Top,double Bottom,double Right,double Left);
    private static AxisExtents Extents(CandidateContext context,CandidateShape curve,double tolerance)
    {
        // Source evaluates every extent helper; its first-event indexing is unchecked.
        var top=First(context,curve,new(0,0,100),tolerance);
        var bottom=First(context,curve,new(0,0,-100),tolerance);
        var right=First(context,curve,new(100,0,0),tolerance);
        var left=First(context,curve,new(-100,0,0),tolerance);
        if(!double.IsFinite(top.Z)||!double.IsFinite(bottom.Z)||!double.IsFinite(right.X)||!double.IsFinite(left.X))
            throw new OutsideRailCandidateException("Failed to get rail curve size");
        return new(top.Z,bottom.Z,right.X,left.X);
    }
    private static CandidatePoint3 First(CandidateContext context,CandidateShape curve,CandidatePoint3 end,double tolerance)=>
        context.IntersectCurveLine(curve,new(0,0,0),end,tolerance).Events[0].CurvePointStart;
    private static CandidateAffine Scale(double x,double y,double z,double zPivot=0)=>
        new(x,0,0,0,0,y,0,0,0,0,z,zPivot*(1-z));

    private static CandidateShape ChangeSeam(CandidateContext context,CurveScope owned,CandidateShape curve,double parameter,
        string stage,List<OutsideRailKernelDiagnostic> diagnostics)
    {
        if(!context.IsCurveClosed(curve)){
            diagnostics.Add(new(stage,CandidateNativeStatus.InvalidArgument,"Open curve: source Boolean seam call leaves geometry unchanged."));
            return curve;
        }
        try{return owned.Add(context.RelocateClosedCurveSeam(curve,parameter));}
        catch(CandidateGeometryException error) when(error.Status is CandidateNativeStatus.KernelFailure or CandidateNativeStatus.InvalidArgument){
            diagnostics.Add(new(stage,error.Status,error.NativeMessage));return curve;
        }
    }

    private static CandidateShape[] MergePair(CandidateContext context,CurveScope owned,CandidateShape first,CandidateShape second,
        double tolerance,List<OutsideRailKernelDiagnostic> diagnostics,out OutsideRailJoinDisposition disposition)
    {
        var a=context.InspectCurve(first);var b=context.InspectCurve(second);
        var a0=context.PointAt(first,a.FirstParameter);var a1=context.PointAt(first,a.LastParameter);
        var b0=context.PointAt(second,b.FirstParameter);var b1=context.PointAt(second,b.LastParameter);
        static double Distance(CandidatePoint3 p,CandidatePoint3 q)=>Math.Sqrt((p.X-q.X)*(p.X-q.X)+(p.Y-q.Y)*(p.Y-q.Y)+(p.Z-q.Z)*(p.Z-q.Z));
        CandidateShape lhs=first,rhs=second;
        if(Distance(a1,b0)>tolerance){
            if(Distance(a1,b1)<=tolerance)rhs=owned.Add(context.ReverseCurve(second));
            else if(Distance(a0,b1)<=tolerance){lhs=second;rhs=first;}
            else if(Distance(a0,b0)<=tolerance)lhs=owned.Add(context.ReverseCurve(first));
            else{
                disposition=OutsideRailJoinDisposition.UnmergedInputs;
                return new[]{owned.Add(context.Clone(first)),owned.Add(context.Clone(second))};
            }
        }
        try{
            var merged=owned.Add(context.JoinCurves(lhs,rhs,tolerance));disposition=OutsideRailJoinDisposition.Merged;
            return new[]{merged};
        }catch(CandidateGeometryException error) when(error.Status==CandidateNativeStatus.KernelFailure){
            diagnostics.Add(new("join",error.Status,error.NativeMessage));
            disposition=OutsideRailJoinDisposition.KernelFailureFallback;return Array.Empty<CandidateShape>();
        }
    }
    private sealed class CurveScope:IDisposable
    {
        private readonly List<CandidateShape> curves=new();
        internal CandidateShape Add(CandidateShape curve){try{curves.Add(curve);return curve;}catch{curve.Dispose();throw;}}
        internal CandidateShape Detach(CandidateShape curve){if(!curves.Remove(curve))throw new InvalidOperationException("Unowned candidate output.");return curve;}
        public void Dispose(){for(int i=curves.Count-1;i>=0;i--)curves[i].Dispose();}
    }
}
