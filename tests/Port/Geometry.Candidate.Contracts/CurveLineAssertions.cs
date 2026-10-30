using JewelDesign.Geometry.Candidate;
namespace CandidateGeometryContractChecks;

internal static class CurveLineAssertions
{
    private static void Check(bool value,string message){if(!value)throw new Exception(message);}
    private static void Near(double actual,double expected,double tolerance=1e-7)=>Check(double.IsFinite(actual)&&Math.Abs(actual-expected)<tolerance,"Intersection mismatch: "+actual.ToString("R")+" expected "+expected.ToString("R"));
    private static void Point(CandidatePoint3 actual,double x,double y,double z){Near(actual.X,x);Near(actual.Y,y);Near(actual.Z,z);}
    private static CandidateGeometryException Fails(Action action)
    {try{action();}catch(CandidateGeometryException error){return error;}throw new Exception("Expected native intersection failure");}
    public static string Run()
    {
        using var context=CandidateContext.Create();
        using(var circle=context.CreateCircle(7,CandidatePlane.WorldZX)){
            var start=new CandidatePoint3(-200,0,0);var end=new CandidatePoint3(200,0,0);
            var result=context.IntersectCurveLine(circle,start,end,1e-6);
            Near(result.RequestedTolerance,1e-6);Near(result.EffectiveFuzzyTolerance,1e-6);
            Check(result.Events.Count==2,"Diameter must have two distinct point events");
            var first=result.Events[0];var last=result.Events[1];
            Check(first.Kind==CandidateCurveLineKind.Point&&last.Kind==CandidateCurveLineKind.Point,"Diameter event kind");
            Near(first.CurveStart,Math.PI/2);Near(last.CurveStart,3*Math.PI/2);
            Near(first.CurveEnd,first.CurveStart);Near(first.LineStart,.5175);Near(last.LineStart,.4825);
            Point(first.CurvePointStart,7,0,0);Point(first.CurvePointEnd,7,0,0);
            Point(first.LinePointStart,7,0,0);Point(first.LinePointEnd,7,0,0);Point(last.CurvePointStart,-7,0,0);
            var backwards=context.IntersectCurveLine(circle,end,start,1e-6);
            Near(backwards.Events[0].LineStart,1-first.LineStart);Near(backwards.Events[1].CurveStart,last.CurveStart);
            foreach(var direction in new[]{new CandidatePoint3(100,0,0),new CandidatePoint3(-100,0,0),new CandidatePoint3(0,0,100),new CandidatePoint3(0,0,-100)}){
                var ray=context.IntersectCurveLine(circle,new(0,0,0),direction,1e-6);
                Check(ray.Events.Count>0,"Finite axis ray missed rail");
                foreach(var hit in ray.Events){Check(hit.Kind==CandidateCurveLineKind.Point,"Ray overlap");Point(hit.CurvePointStart,direction.X*.07,0,direction.Z*.07);}
            }
            Check(context.IntersectCurveLine(circle,new(0,0,0),new(6,0,0),1e-6).Events.Count==0,"Finite ray was extended");
            Check(context.IntersectCurveLine(circle,new(-20,3,0),new(20,3,0),1e-6).Events.Count==0,"Skew line was projected into rail plane");
            var tangent=context.IntersectCurveLine(circle,new(-7,0,-20),new(-7,0,20),1e-6);
            Check(tangent.Events.Count==1&&tangent.Events[0].Kind==CandidateCurveLineKind.Point,"Tangent event cardinality/kind");
            Point(tangent.Events[0].CurvePointStart,-7,0,0);
            using var quarter=context.TrimCurve(circle,0,Math.PI/2);
            var endpoint=context.IntersectCurveLine(quarter,start,end,1e-6);
            Check(endpoint.Events.Count==1,"Trim endpoint lost or full circle used");Near(endpoint.Events[0].CurveStart,Math.PI/2);
            var tight=context.IntersectCurveLine(circle,start,end,1e-12);
            Near(tight.RequestedTolerance,1e-12,1e-15);Check(tight.EffectiveFuzzyTolerance>=1e-7,"Native minimum fuzzy tolerance hidden");
            foreach(var tolerance in new[]{0,-1,double.NaN,double.PositiveInfinity})
                Check(Fails(()=>context.IntersectCurveLine(circle,start,end,tolerance)).Status==CandidateNativeStatus.InvalidArgument,"Invalid intersection tolerance");
            Check(Fails(()=>context.IntersectCurveLine(circle,start,start,1e-6)).Status==CandidateNativeStatus.InvalidArgument,"Zero segment accepted");
            Check(Fails(()=>context.IntersectCurveLine(circle,new(double.NaN,0,0),end,1e-6)).Status==CandidateNativeStatus.InvalidArgument,"Nonfinite segment accepted");
        }
        using(var line=context.CreateLine(new(0,0,0),new(10,0,0))){
            var info=context.InspectCurve(line);Near(info.Length,10);Check(info.Degree==1&&info.PoleCount==2,"Line must be exact linear NURBS");
            var overlap=context.IntersectCurveLine(line,new(3,0,0),new(12,0,0),1e-6);
            Check(overlap.Events.Count==1&&overlap.Events[0].Kind==CandidateCurveLineKind.Overlap,"Overlap must not be dropped or converted to point: count="+overlap.Events.Count+" "+string.Join(";",overlap.Events));
            var hit=overlap.Events[0];Near(hit.CurveStart,3,1e-5);Near(hit.CurveEnd,10,1e-5);Near(hit.LineStart,0,1e-5);Near(hit.LineEnd,7.0/9,1e-5);
            Point(hit.CurvePointStart,3,0,0);Point(hit.CurvePointEnd,10,0,0);Point(hit.LinePointStart,3,0,0);Point(hit.LinePointEnd,10,0,0);
            var reverse=context.IntersectCurveLine(line,new(12,0,0),new(3,0,0),1e-6).Events[0];
            Check(reverse.Kind==CandidateCurveLineKind.Overlap,"Reversed overlap kind");
            Point(reverse.CurvePointStart,3,0,0);Point(reverse.CurvePointEnd,10,0,0);Point(reverse.LinePointStart,10,0,0);Point(reverse.LinePointEnd,3,0,0);

            using(var changed=context.TransformAffine(line,new(2,0,0,5,0,1,0,0,0,0,1,0))){
                foreach(var segment in new[]{(new CandidatePoint3(9,0,0),new CandidatePoint3(21,0,0)),(new CandidatePoint3(21,0,0),new CandidatePoint3(9,0,0))}){
                    var transformed=context.IntersectCurveLine(changed,segment.Item1,segment.Item2,1e-6);
                    Check(transformed.Events.Count==1&&transformed.Events[0].Kind==CandidateCurveLineKind.Overlap,"Scaled parameter overlap");
                    var mapped=transformed.Events[0];Near(mapped.CurveStart,2);Near(mapped.CurveEnd,8);
                    Point(mapped.CurvePointStart,9,0,0);Point(mapped.CurvePointEnd,21,0,0);
                }
            }
            using(var reversedLine=context.ReverseCurve(line)){
                var backwards=context.IntersectCurveLine(reversedLine,new(3,0,0),new(12,0,0),1e-6);
                Check(backwards.Events.Count==1&&backwards.Events[0].Kind==CandidateCurveLineKind.Overlap,"Reversed source overlap");
                Near(backwards.Events[0].CurveStart,0);Near(backwards.Events[0].CurveEnd,7);
                Point(backwards.Events[0].CurvePointStart,10,0,0);Point(backwards.Events[0].CurvePointEnd,3,0,0);
            }
            Check(context.IntersectCurveLine(line,new(11,0,0),new(12,0,0),1e-6).Events.Count==0,"Disjoint collinear segment overlaps");
        }
        using(var diagonal=context.CreateLine(new(0,0,0),new(10,10,10))){
            var diagonalOverlap=context.IntersectCurveLine(diagonal,new(12,12,12),new(3,3,3),1e-6);
            Check(diagonalOverlap.Events.Count==1&&diagonalOverlap.Events[0].Kind==CandidateCurveLineKind.Overlap,"3D overlap");
            Point(diagonalOverlap.Events[0].CurvePointStart,3,3,3);Point(diagonalOverlap.Events[0].CurvePointEnd,10,10,10);
            Point(diagonalOverlap.Events[0].LinePointStart,10,10,10);Point(diagonalOverlap.Events[0].LinePointEnd,3,3,3);
            var crossing=context.IntersectCurveLine(diagonal,new(0,10,0),new(10,0,10),1e-6);
            Check(crossing.Events.Count==1&&crossing.Events[0].Kind==CandidateCurveLineKind.Point,"True3D crossing");
            var hit=crossing.Events[0];Point(hit.CurvePointStart,5,5,5);Point(hit.LinePointStart,5,5,5);Near(hit.LineStart,.5);
        }
        using(var solid=context.CreateBox(1,1,1))Check(Fails(()=>context.IntersectCurveLine(solid,new(0,0,0),new(10,0,0),1e-6)).Status==CandidateNativeStatus.UnsupportedOperation,"Solid produced fake curve events");
        Check(Fails(()=>{using var invalid=context.CreateLine(new(1,2,3),new(1,2,3));}).Status==CandidateNativeStatus.InvalidArgument,"Invalid line created");
        Check(context.DiagnosticLiveShapeCount==0,"Intersection query retained native shape entries");
        return "PASS native finite curve-line events: exact linear NURBS, diameter/rays/finite/skew/tangent/trim/3D and point/overlap/reversed ranges, explicit fuzzy tolerance and typed guards; no Rhino ordering/full builder/PC claim.";
    }
}
