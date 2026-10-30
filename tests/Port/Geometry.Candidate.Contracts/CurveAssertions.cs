using JewelDesign.Geometry.Candidate;
namespace CandidateGeometryContractChecks;

internal static class CurveAssertions
{
    private static void Check(bool condition,string message){if(!condition)throw new Exception(message);}
    private static void Near(double value,double expected)=>Check(double.IsFinite(value)&&Math.Abs(value-expected)<1e-8,"Native coordinate/length mismatch: actual="+value.ToString("R")+", expected="+expected.ToString("R"));
    private static void Point(CandidatePoint3 value,double x,double y,double z){Near(value.X,x);Near(value.Y,y);Near(value.Z,z);}
    private static CandidateGeometryException Fails(Action action)
    {try{action();}catch(CandidateGeometryException e){return e;}throw new Exception("Expected native curve failure");}
    private static void Direction(CandidateVector3 value,double x,double y,double z){Near(value.X,x);Near(value.Y,y);Near(value.Z,z);Near(value.X*value.X+value.Y*value.Y+value.Z*value.Z,1);}
    public static string Run()
    {
        using var context=CandidateContext.Create();
        using(var circle=context.CreateCircle(7,CandidatePlane.WorldZX)){
            var info=context.InspectCurve(circle);
            Check(info.Degree==2&&info.PoleCount==9&&info.IsRational,"Four-span rational circle representation");
            Near(info.FirstParameter,0);Near(info.LastParameter,2*Math.PI);Near(info.Length,14*Math.PI);
            double w=1/Math.Sqrt(2);int[] c={1,0,-1,0},s={0,1,0,-1};
            for(int quarter=0;quarter<4;quarter++)foreach(double u in new[]{0,.125,.25,.5,.75,.875,1}){
                double b0=(1-u)*(1-u),b1=2*u*(1-u),b2=u*u,den=b0+w*b1+b2;
                double a=7*(b0+w*b1)/den,b=7*(w*b1+b2)/den;
                double localX=c[quarter]*a-s[quarter]*b,localY=s[quarter]*a+c[quarter]*b;
                Point(context.PointAt(circle,(quarter+u)*Math.PI/2),localY,0,localX);
            }
            Direction(context.TangentAt(circle,0),1,0,0);Direction(context.TangentAt(circle,2*Math.PI),1,0,0);
            foreach(double bad in new[]{-.1,2*Math.PI+.1,double.NaN,double.PositiveInfinity})Check(Fails(()=>context.TangentAt(circle,bad)).Status==CandidateNativeStatus.InvalidArgument,"Invalid tangent parameter");
            double previous=-1;
            for(int i=0;i<50;i++){
                double fraction=i/50.0,parameter=context.ParameterAtNormalizedLength(circle,fraction,1e-9);
                Check(parameter>=previous,"Arc-length parameters are not monotone");previous=parameter;
                Point(context.PointAt(circle,parameter),7*Math.Sin(2*Math.PI*fraction),0,7*Math.Cos(2*Math.PI*fraction));
                Direction(context.TangentAt(circle,parameter),Math.Cos(2*Math.PI*fraction),0,-Math.Sin(2*Math.PI*fraction));
            }
            Near(context.ParameterAtNormalizedLength(circle,0,1e-9),info.FirstParameter);
            Near(context.ParameterAtNormalizedLength(circle,1,1e-9),info.LastParameter);
            Check(Math.Abs(context.ParameterAtNormalizedLength(circle,.0625,1e-9)-Math.PI/8)>1e-4,"Arc-length mapper used naive parameter scaling");
            foreach(double bad in new[]{-.1,1.1,double.NaN,double.PositiveInfinity})Check(Fails(()=>context.ParameterAtNormalizedLength(circle,bad,1e-9)).Status==CandidateNativeStatus.InvalidArgument,"Invalid length fraction");
            Check(Fails(()=>context.ParameterAtNormalizedLength(circle,.5,0)).Status==CandidateNativeStatus.InvalidArgument,"Invalid length tolerance");
            foreach(double scale in new[]{1.0,1.0000000000000002,2.0,-1.0}){
                using var uniform=context.TransformAffine(circle,new(scale,0,0,1,0,scale,0,2,0,0,scale,3));
                Point(context.PointAt(uniform,0),1,2,7*scale+3);
                Point(context.PointAt(uniform,Math.PI/2),7*scale+1,2,3);
                using var twice=context.TransformAffine(uniform,new(1,0,0,-1,0,1,0,-2,0,0,2,-6));
                Point(context.PointAt(twice,0),0,0,14*scale);
            }
            var affine=new CandidateAffine(2,0,.5,1,0,1,0,2,0,0,3,-4);
            using(var changed=context.TransformAffine(circle,affine)){
                Point(context.PointAt(changed,0),4.5,2,17);
                Point(context.PointAt(changed,Math.PI/2),15,2,-4);
                Direction(context.TangentAt(changed,Math.PI/2),-.5/Math.Sqrt(9.25),0,-3/Math.Sqrt(9.25));
                Direction(context.TangentAt(circle,Math.PI/2),0,0,-1);
                Point(context.PointAt(changed,Math.PI),-2.5,2,-25);
                Point(context.PointAt(circle,0),0,0,7);
            }
            Check(Fails(()=>{using var rejected=context.TransformAffine(circle,new(0,0,0,0,0,1,0,0,0,0,1,0));}).Status==CandidateNativeStatus.InvalidArgument,"Singular affine matrix accepted");
            Check(Fails(()=>{using var rejected=context.TransformAffine(circle,new(double.NaN,0,0,0,0,1,0,0,0,0,1,0));}).Status==CandidateNativeStatus.InvalidArgument,"Nonfinite affine matrix accepted");
            var nearest=context.ClosestPoint(circle,new(10,3,10));double diagonal=7/Math.Sqrt(2);
            Point(nearest.Point,diagonal,0,diagonal);
            Near(nearest.Distance,Math.Sqrt(Math.Pow(Math.Sqrt(200)-7,2)+9));
            var evaluated=context.PointAt(circle,nearest.Parameter);Point(evaluated,nearest.Point.X,nearest.Point.Y,nearest.Point.Z);
            var seam=context.ClosestPoint(circle,new(0,0,7));Point(seam.Point,0,0,7);Near(seam.Distance,0);
            Check(Fails(()=>context.ClosestPoint(circle,new(double.NaN,0,0))).Status==CandidateNativeStatus.InvalidArgument,"Nonfinite projection input");
            using(var quarter=context.TrimCurve(circle,0,Math.PI/2)){
                var trimmed=context.InspectCurve(quarter);Near(trimmed.FirstParameter,0);Near(trimmed.LastParameter,Math.PI/2);Near(trimmed.Length,7*Math.PI/2);
                Point(context.PointAt(quarter,0),0,0,7);Point(context.PointAt(quarter,Math.PI/2),7,0,0);
                var outsideEnd=context.ClosestPoint(quarter,new(10,0,-2));Point(outsideEnd.Point,7,0,0);Near(outsideEnd.Distance,Math.Sqrt(13));
                var outsideStart=context.ClosestPoint(quarter,new(-2,0,10));Point(outsideStart.Point,0,0,7);
                using var reversed=context.ReverseCurve(quarter);
                Direction(context.TangentAt(quarter,Math.PI/2),0,0,-1);Direction(context.TangentAt(reversed,0),0,0,1);
                Point(context.PointAt(reversed,0),7,0,0);Point(context.PointAt(reversed,Math.PI/2),0,0,7);
                var sample=context.PointAt(quarter,.3);var opposite=context.PointAt(reversed,Math.PI/2-.3);Point(opposite,sample.X,sample.Y,sample.Z);
                Point(context.PointAt(quarter,0),0,0,7);
            }
            using(var firstArc=context.TrimCurve(circle,0,Math.PI/2))using(var secondArc=context.TrimCurve(circle,Math.PI/2,Math.PI)){
                using var joined=context.JoinCurves(firstArc,secondArc,1e-8);var joinedInfo=context.InspectCurve(joined);Near(joinedInfo.Length,7*Math.PI);
                Point(context.PointAt(joined,joinedInfo.FirstParameter),0,0,7);Point(context.PointAt(joined,joinedInfo.LastParameter),0,0,-7);
                Point(context.PointAt(firstArc,0),0,0,7);Point(context.PointAt(secondArc,Math.PI/2),7,0,0);
                Check(Fails(()=>{using var rejected=context.JoinCurves(secondArc,firstArc,1e-8);}).Status==CandidateNativeStatus.InvalidArgument,"Unordered join silently reordered inputs");
                Check(Fails(()=>{using var rejected=context.JoinCurves(firstArc,secondArc,0);}).Status==CandidateNativeStatus.InvalidArgument,"Invalid join tolerance");
                firstArc.Dispose();secondArc.Dispose();Near(context.InspectCurve(joined).Length,7*Math.PI);
            }
            Check(Fails(()=>{using var rejected=context.TrimCurve(circle,1,0);}).Status==CandidateNativeStatus.InvalidArgument,"Decreasing trim interval");
            Check(Fails(()=>{using var rejected=context.TrimCurve(circle,0,7);}).Status==CandidateNativeStatus.InvalidArgument,"Out-of-domain trim interval");
            using var moved=context.Translate(circle,1,2,3);
            Point(context.PointAt(moved,info.FirstParameter),1,2,10);
            Direction(context.TangentAt(moved,info.FirstParameter),1,0,0);
            var projectedMoved=context.ClosestPoint(moved,new(11,5,13));Point(projectedMoved.Point,1+diagonal,2,3+diagonal);
            using var clone=context.Clone(circle);using var alias=context.Retain(circle);
            circle.Dispose();Point(context.PointAt(clone,info.FirstParameter),0,0,7);Point(context.PointAt(alias,info.FirstParameter),0,0,7);
        }
        var frame=new CandidatePlane(new(1,2,3),new(0,0,1),new(0,1,0));
        using(var circle=context.CreateCircle(2,frame)){
            var info=context.InspectCurve(circle);Near(info.Length,4*Math.PI);
            Point(context.PointAt(circle,info.FirstParameter),1,4,3);Point(context.PointAt(circle,(info.FirstParameter+info.LastParameter)/4),-1,2,3);
            Check(Fails(()=>context.PointAt(circle,double.NaN)).Status==CandidateNativeStatus.InvalidArgument,"Nonfinite curve parameter");
        }
        using(var box=context.CreateBox(1,1,1))Check(Fails(()=>context.InspectCurve(box)).Status==CandidateNativeStatus.UnsupportedOperation,"Solid must not produce fake curve metadata");
        using(var box=context.CreateBox(1,2,3))using(var changed=context.TransformAffine(box,new(2,0,1,5,0,3,0,-2,0,0,4,7))){
            Near(context.Inspect(changed).Volume,144);Near(context.Inspect(box).Volume,6);
        }
        using(var solid=context.CreateBox(1,1,1))Check(Fails(()=>context.TangentAt(solid,0)).Status==CandidateNativeStatus.UnsupportedOperation,"Solid produced fake tangent");
        foreach(double radius in new[]{0,-1,double.NaN,double.PositiveInfinity})
            Check(Fails(()=>{using var rejected=context.CreateCircle(radius,CandidatePlane.WorldZX);}).Status==CandidateNativeStatus.InvalidArgument,"Invalid circle radius");
        Fails(()=>{using var rejected=context.CreateCircle(1,new(new(0,0,0),new(0,0,0),new(1,0,0)));});
        Fails(()=>{using var rejected=context.CreateCircle(1,new(new(0,0,0),new(0,0,1),new(0,0,1)));});
        Check(context.DiagnosticLiveShapeCount==0,"Curve ownership retained native entries");
        return "PASS candidate native curves: actual native unit tangents/50 analytic directions, actual ordered NURBS join, actual NURBS trim/reverse/open-end projection, actual bounded3D closest points, actual affine scale/shear/translation/source preservation, normalized arc-length mapping/50 analytic samples, four-span rational quadratic circles, ZX/translated/rotated frames, domain/length/poles and 28 rational parameter samples, copy/retain/disposal and invalid-input guards; no original-PC or full ring-graph claim.";
    }
}
