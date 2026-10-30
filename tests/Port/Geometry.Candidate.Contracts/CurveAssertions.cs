using JewelDesign.Geometry.Candidate;
namespace CandidateGeometryContractChecks;

internal static class CurveAssertions
{
    private static void Check(bool condition,string message){if(!condition)throw new Exception(message);}
    private static void Near(double value,double expected)=>Check(double.IsFinite(value)&&Math.Abs(value-expected)<1e-8,"Native circle coordinate/length mismatch");
    private static void Point(CandidatePoint3 value,double x,double y,double z){Near(value.X,x);Near(value.Y,y);Near(value.Z,z);}
    private static CandidateGeometryException Fails(Action action)
    {try{action();}catch(CandidateGeometryException e){return e;}throw new Exception("Expected native curve failure");}
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
            double previous=-1;
            for(int i=0;i<50;i++){
                double fraction=i/50.0,parameter=context.ParameterAtNormalizedLength(circle,fraction,1e-9);
                Check(parameter>=previous,"Arc-length parameters are not monotone");previous=parameter;
                Point(context.PointAt(circle,parameter),7*Math.Sin(2*Math.PI*fraction),0,7*Math.Cos(2*Math.PI*fraction));
            }
            Near(context.ParameterAtNormalizedLength(circle,0,1e-9),info.FirstParameter);
            Near(context.ParameterAtNormalizedLength(circle,1,1e-9),info.LastParameter);
            Check(Math.Abs(context.ParameterAtNormalizedLength(circle,.0625,1e-9)-Math.PI/8)>1e-4,"Arc-length mapper used naive parameter scaling");
            foreach(double bad in new[]{-.1,1.1,double.NaN,double.PositiveInfinity})Check(Fails(()=>context.ParameterAtNormalizedLength(circle,bad,1e-9)).Status==CandidateNativeStatus.InvalidArgument,"Invalid length fraction");
            Check(Fails(()=>context.ParameterAtNormalizedLength(circle,.5,0)).Status==CandidateNativeStatus.InvalidArgument,"Invalid length tolerance");
            var affine=new CandidateAffine(2,0,.5,1,0,1,0,2,0,0,3,-4);
            using(var changed=context.TransformAffine(circle,affine)){
                Point(context.PointAt(changed,0),4.5,2,17);
                Point(context.PointAt(changed,Math.PI/2),15,2,-4);
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
            using var moved=context.Translate(circle,1,2,3);
            Point(context.PointAt(moved,info.FirstParameter),1,2,10);
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
        foreach(double radius in new[]{0,-1,double.NaN,double.PositiveInfinity})
            Check(Fails(()=>{using var rejected=context.CreateCircle(radius,CandidatePlane.WorldZX);}).Status==CandidateNativeStatus.InvalidArgument,"Invalid circle radius");
        Fails(()=>{using var rejected=context.CreateCircle(1,new(new(0,0,0),new(0,0,0),new(1,0,0)));});
        Fails(()=>{using var rejected=context.CreateCircle(1,new(new(0,0,0),new(0,0,1),new(0,0,1)));});
        Check(context.DiagnosticLiveShapeCount==0,"Curve ownership retained native entries");
        return "PASS candidate native curves: actual bounded3D closest points, actual affine scale/shear/translation/source preservation, normalized arc-length mapping/50 analytic samples, four-span rational quadratic circles, ZX/translated/rotated frames, domain/length/poles and 28 rational parameter samples, copy/retain/disposal and invalid-input guards; no original-PC or full ring-graph claim.";
    }
}
