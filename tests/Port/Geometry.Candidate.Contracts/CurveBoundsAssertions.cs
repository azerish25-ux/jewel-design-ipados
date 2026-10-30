using JewelDesign.Geometry.Candidate;
namespace CandidateGeometryContractChecks;

internal static class CurveBoundsAssertions
{
    private static void Check(bool value,string message){if(!value)throw new Exception(message);}
    private static void Near(double actual,double expected)=>Check(double.IsFinite(actual)&&Math.Abs(actual-expected)<1e-6,"Bounds mismatch: "+actual.ToString("R")+" expected "+expected.ToString("R"));
    private static void Point(CandidatePoint3 actual,double x,double y,double z){Near(actual.X,x);Near(actual.Y,y);Near(actual.Z,z);}
    private static CandidateGeometryException Fails(Action action)
    {try{action();}catch(CandidateGeometryException error){return error;}throw new Exception("Expected native bounds failure");}
    public static string Run()
    {
        using var context=CandidateContext.Create();
        using(var circle=context.CreateCircle(7,CandidatePlane.WorldZX)){
            var bounds=context.GetCurveBounds(circle,1e-10);
            Point(bounds.Minimum,-7,0,-7);Point(bounds.Maximum,7,0,7);
            Check(bounds.RequestedTolerance==1e-10&&bounds.KernelPadding>=1e-7,"Requested tolerance/kernel padding lost");
            Check(bounds.PaddedMaximum.Z>bounds.Maximum.Z&&bounds.PaddedMinimum.Z<bounds.Minimum.Z,"Native padding must remain explicit");
            Near(bounds.PaddedMaximum.Z-bounds.Maximum.Z,bounds.KernelPadding);
            using(var arc=context.TrimCurve(circle,Math.PI/4,3*Math.PI/4)){
                var clipped=context.GetCurveBounds(arc,1e-10);
                Point(clipped.Minimum,7/Math.Sqrt(2),0,-7/Math.Sqrt(2));Point(clipped.Maximum,7,0,7/Math.Sqrt(2));
                using var reversed=context.ReverseCurve(arc);var reverseBounds=context.GetCurveBounds(reversed,1e-10);
                Point(reverseBounds.Minimum,clipped.Minimum.X,clipped.Minimum.Y,clipped.Minimum.Z);
                Point(reverseBounds.Maximum,clipped.Maximum.X,clipped.Maximum.Y,clipped.Maximum.Z);
            }
            using(var ellipse=context.TransformAffine(circle,new(2,0,.5,1,0,1,0,2,0,0,3,-4))){
                var actual=context.GetCurveBounds(ellipse,1e-10);double xAmplitude=7*Math.Sqrt(4.25);
                Point(actual.Minimum,1-xAmplitude,2,-25);Point(actual.Maximum,1+xAmplitude,2,17);
                Check(actual.Maximum.X<17,"Pole hull substituted for curve extrema");
            }
            using(var translated=context.Translate(circle,1,2,3)){
                var actual=context.GetCurveBounds(translated,1e-10);Point(actual.Minimum,-6,2,-4);Point(actual.Maximum,8,2,10);
            }
            using(var moved=context.RelocateClosedCurveSeam(circle,.3)){
                var actual=context.GetCurveBounds(moved,1e-10);Point(actual.Minimum,-7,0,-7);Point(actual.Maximum,7,0,7);
            }
            foreach(double tolerance in new[]{0,-1,double.NaN,double.PositiveInfinity})
                Check(Fails(()=>context.GetCurveBounds(circle,tolerance)).Status==CandidateNativeStatus.InvalidArgument,"Invalid bound tolerance");
            var start=context.PointAt(circle,0);Point(start,0,0,7);
        }
        using(var line=context.CreateLine(new(2,-3,4),new(-5,6,-7))){
            var actual=context.GetCurveBounds(line,1e-10);Point(actual.Minimum,-5,-3,-7);Point(actual.Maximum,2,6,4);
        }
        using(var solid=context.CreateBox(1,1,1))Check(Fails(()=>context.GetCurveBounds(solid,1e-10)).Status==CandidateNativeStatus.UnsupportedOperation,"Solid produced fake curve bounds");
        Check(context.DiagnosticLiveShapeCount==0,"Bounds query retained native shape entries");
        return "PASS native curve bounds: unpadded optimal extrema and explicit padding, circle/trim/interior maximum/affine ellipse/line/translation/seam/reverse, input preservation and typed guards; no arbitrary-spline/Rhino/PC guarantee.";
    }
}
