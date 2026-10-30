using JewelDesign.Geometry.Candidate;
namespace CandidateGeometryContractChecks;

internal static class CurveSeamAssertions
{
    private static void Check(bool condition,string message){if(!condition)throw new Exception(message);}
    private static void Near(double actual,double expected)=>Check(double.IsFinite(actual)&&Math.Abs(actual-expected)<1e-8,"Seam mismatch: "+actual.ToString("R")+" expected "+expected.ToString("R"));
    private static void Point(CandidatePoint3 actual,CandidatePoint3 expected){Near(actual.X,expected.X);Near(actual.Y,expected.Y);Near(actual.Z,expected.Z);}
    private static CandidateGeometryException Fails(Action action)
    {try{action();}catch(CandidateGeometryException error){return error;}throw new Exception("Expected native seam failure");}
    private static void Equivalent(CandidateContext context,CandidateShape source,double seam)
    {
        var original=context.InspectCurve(source);double period=original.LastParameter-original.FirstParameter;
        var before=new CandidatePoint3[65];
        for(int i=0;i<=64;i++)before[i]=context.PointAt(source,original.FirstParameter+period*i/64.0);
        using var moved=context.RelocateClosedCurveSeam(source,seam);
        var info=context.InspectCurve(moved);
        Near(info.FirstParameter,seam);Near(info.LastParameter,seam+period);Near(info.Length,original.Length);
        Point(context.PointAt(moved,info.FirstParameter),context.PointAt(source,seam));
        Point(context.PointAt(moved,info.LastParameter),context.PointAt(source,seam));
        for(int i=0;i<=64;i++){
            double parameter=seam+period*i/64.0,wrapped=parameter;
            if(wrapped>original.LastParameter)wrapped-=period;
            Point(context.PointAt(moved,parameter),context.PointAt(source,wrapped));
        }
        for(int i=0;i<=64;i++)Point(context.PointAt(source,original.FirstParameter+period*i/64.0),before[i]);
        Point(context.PointAt(source,original.FirstParameter),context.PointAt(source,original.LastParameter));
    }
    public static string Run()
    {
        using var context=CandidateContext.Create();
        using(var circle=context.CreateCircle(7,CandidatePlane.WorldZX)){
            foreach(double seam in new[]{0,.3,Math.PI/2,2*Math.PI})Equivalent(context,circle,seam);
            using(var moved=context.RelocateClosedCurveSeam(circle,.3)){
                Equivalent(context,moved,.9);
                var info=context.InspectCurve(moved);var expected=context.TangentAt(circle,.3);var tangent=context.TangentAt(moved,info.FirstParameter);
                Near(tangent.X,expected.X);Near(tangent.Y,expected.Y);Near(tangent.Z,expected.Z);
                using var reversed=context.ReverseCurve(moved);var opposite=context.TangentAt(reversed,info.FirstParameter);
                Near(opposite.X,-expected.X);Near(opposite.Y,-expected.Y);Near(opposite.Z,-expected.Z);
                var hit=context.ClosestPoint(moved,new(7,0,0));
                using var trimmed=context.TrimCurve(moved,info.FirstParameter,hit.Parameter);
                Point(context.PointAt(trimmed,hit.Parameter),new(7,0,0));
            }
            using(var changed=context.TransformAffine(circle,new(2,0,.5,1,0,1,0,2,0,0,3,-4))){
                Equivalent(context,changed,.7);
                using var trim=context.TrimCurve(changed,.7,2);Point(context.PointAt(trim,.7),context.PointAt(changed,.7));Point(context.PointAt(trim,2),context.PointAt(changed,2));
                using var reverse=context.ReverseCurve(changed);Point(context.PointAt(reverse,0),context.PointAt(changed,2*Math.PI));
            }
            using(var quarter=context.TrimCurve(circle,0,Math.PI/2))
                Check(Fails(()=>{using var rejected=context.RelocateClosedCurveSeam(quarter,.3);}).Status==CandidateNativeStatus.InvalidArgument,"Open curve accepted");
            foreach(double bad in new[]{-.1,7,double.NaN,double.PositiveInfinity})
                Check(Fails(()=>{using var rejected=context.RelocateClosedCurveSeam(circle,bad);}).Status==CandidateNativeStatus.InvalidArgument,"Invalid seam parameter accepted");
            using var owned=context.RelocateClosedCurveSeam(circle,.3);var start=context.PointAt(owned,.3);
            circle.Dispose();Point(context.PointAt(owned,.3),start);
        }
        using(var a=context.CreateLine(new(0,0,0),new(4,0,0)))
        using(var b=context.CreateLine(new(4,0,0),new(4,0,6)))
        using(var c=context.CreateLine(new(4,0,6),new(0,0,6)))
        using(var d=context.CreateLine(new(0,0,6),new(0,0,0)))
        using(var ab=context.JoinCurves(a,b,1e-8))
        using(var abc=context.JoinCurves(ab,c,1e-8))
        using(var rectangle=context.JoinCurves(abc,d,1e-8)){
            var info=context.InspectCurve(rectangle);Near(info.Length,20);
            Equivalent(context,rectangle,info.FirstParameter+.37*(info.LastParameter-info.FirstParameter));
        }
        using(var solid=context.CreateBox(1,1,1))
            Check(Fails(()=>{using var rejected=context.RelocateClosedCurveSeam(solid,0);}).Status==CandidateNativeStatus.UnsupportedOperation,"Solid accepted as closed curve");
        Check(context.DiagnosticLiveShapeCount==0,"Seam operation retained native shape entries");
        return "PASS native closed-curve seam: copied circle/affine/corner curves, requested domain start, wrapped points/length/orientation, reverse/closest/trim and ownership/error guards; no original-PC/full OutsideRail claim.";
    }
}
