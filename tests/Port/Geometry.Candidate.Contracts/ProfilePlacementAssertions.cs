using JewelDesign.Documents;
using JewelDesign.Features.Candidate;
using JewelDesign.Geometry.Candidate;
using Oracle=CandidateGeometryContractChecks.Recovered.Placement;
namespace CandidateGeometryContractChecks;

internal static class ProfilePlacementAssertions
{
    private static void Check(bool v,string message){if(!v)throw new Exception(message);}
    private static void Near(double a,double b,double tolerance=1e-5){if(!double.IsFinite(a)||!double.IsFinite(b)||Math.Abs(a-b)>tolerance)throw new Exception("Placement mismatch "+a+" vs "+b);}
    private static void Throws<T>(Action action)where T:Exception{try{action();}catch(T){return;}throw new Exception("Expected "+typeof(T).Name);}
    private static double Number(LegacyChannelValue value){double n=123;Check(value.GetDouble(ref n),"Missing placement numeric output");return n;}
    private static double[] Fields(LegacyAffineTransform m)=>new[]{m.M11,m.M12,m.M13,m.M14,m.M21,m.M22,m.M23,m.M24,m.M31,m.M32,m.M33,m.M34};
    private static CandidateAffine Native(LegacyAffineTransform m)=>new(m.M11,m.M12,m.M13,m.M14,m.M21,m.M22,m.M23,m.M24,m.M31,m.M32,m.M33,m.M34);
    private static int SourceCases()
    {
        int count=0;double[] coordinates={-3,-1,0,1,3};
        var directions=new[]{new CandidateVector3(1,0,0),new(0,1,0),new(0,0,1),new(1,1,0),new(1,0,1),new(0,1,1),new(1,2,3)};
        foreach(double x in coordinates)foreach(double y in coordinates)foreach(double z in coordinates)foreach(var tangent in directions)foreach(double scale in new[]{-2.0,0,.5,1,2}){
            var point=new CandidatePoint3(x,y,z);double sy=y*scale;
            double cx=sy*tangent.Z-z*tangent.Y,cy=z*tangent.X-x*tangent.Z,cz=x*tangent.Y-sy*tangent.X;
            if(cx*cx+cy*cy+cz*cz==0)continue; // Source native Unitize/degenerate policy is not modeled by these stubs.
            var observed=new Oracle.ObservedCurve(point,tangent);var source=new Oracle.OnXform();
            Check(Oracle.SourceGeometry.OrientProfileToCurveNalp(observed,.3,ref source,scale),"Source orientation failed");
            var actual=Fields(ProfilePlacement2RailCandidate.OrientFrame(point,tangent,scale));var expected=Fields(source.Matrix);
            for(int i=0;i<actual.Length;i++)Near(actual[i],expected[i],1e-12);count++;
        }
        foreach(var inner in new[]{(0.0,1.0),(0.0,2*Math.PI),(-2.0,7.0),(0.0,.00001)})
        foreach(var outer in new[]{(0.0,1.0),(2.0,5.0)})
        foreach(double fraction in new[]{0.0,1.0,.5,.0001,Math.BitDecrement(.0001),Math.BitIncrement(.0001),2*Math.PI,double.NaN})
        foreach(double parameter in new[]{outer.Item1,outer.Item2,outer.Item1+.0001,outer.Item2-.0001,double.NaN}){
            double a=ProfilePlacement2RailCandidate.AdjustOuterParameter(fraction,inner.Item1,inner.Item2,outer.Item1,outer.Item2,parameter);
            double e=Oracle.SourceAdjust.Adjust(fraction,inner.Item1,inner.Item2,outer.Item1,outer.Item2,parameter);
            Check(BitConverter.DoubleToInt64Bits(a)==BitConverter.DoubleToInt64Bits(e),"Source sequential seam branch differs");count++;
        }
        Check(ProfilePlacement2RailCandidate.AdjustOuterParameter(1,0,2*Math.PI,0,2*Math.PI,0)==0,"Normalized fraction was incorrectly replaced by native parameter");
        Throws<NotSupportedException>(()=>ProfilePlacement2RailCandidate.OrientFrame(new(0,0,0),new(1,0,0),1));
        Throws<NotSupportedException>(()=>ProfilePlacement2RailCandidate.OrientFrame(new(0,0,7),new(0,0,1),1));
        return count;
    }
    public static string Run()
    {
        int sourceCases=SourceCases();
        var settings=new OutsideRailCandidateSettings(1e-7,1e-7,1e-7);
        using var context=CandidateContext.Create();
        using(var inner=context.CreateCircle(7,CandidatePlane.WorldZX)){
            using var outer=context.CreateCircle(9,CandidatePlane.WorldZX);
            for(int i=0;i<=20;i++){
                double fraction=i/20.0;var result=ProfilePlacement2RailCandidate.Build(context,inner,outer,fraction,0,settings);
                Near(result.Height!.Value,2);Check(result.IntersectionEventCount>0&&result.EffectiveIntersectionTolerance>=1e-7,"Missing native intersection diagnostics");
                var p=context.PointAt(inner,result.InnerParameter);var m=result.Transform;
                Near(m.M14,p.X);Near(m.M24,p.Y);Near(m.M34,p.Z);
                Near(m.M12,p.X/7);Near(m.M22,0);Near(m.M32,p.Z/7);
                var q=context.PointAt(outer,result.OuterParameter!.Value);Near(q.X,p.X*9/7);Near(q.Y,0);Near(q.Z,p.Z*9/7);
            }
            foreach(double delta in new[]{.005,.01,.02,29.9,30.5}){
                using var boundary=context.CreateCircle(7+delta,CandidatePlane.WorldZX);
                var result=ProfilePlacement2RailCandidate.Build(context,inner,boundary,0,1,settings);
                if(delta>30)Check(result.Height is null&&result.OuterParameter is null,"Finite30-unit ray was extended");
                else Near(result.Height!.Value,Oracle.SourceAdjust.Height(delta),2e-6);
            }
            var noOuter=ProfilePlacement2RailCandidate.Build(context,inner,null,.25,1,settings);
            Check(noOuter.Height is null&&noOuter.OuterParameter is null&&noOuter.EffectiveIntersectionTolerance is null,"Missing optional outside fabricated outputs");
            using var ellipse=context.TransformAffine(outer,new(2,0,0,0,0,1,0,0,0,0,1.5,0));
            for(int i=0;i<12;i++){
                var result=ProfilePlacement2RailCandidate.Build(context,inner,ellipse,i/12.0,1,settings);var p=context.PointAt(inner,result.InnerParameter);
                double ux=p.X/7,uz=p.Z/7,radius=1/Math.Sqrt(ux*ux/(18*18)+uz*uz/(13.5*13.5));Near(result.Height!.Value,radius-7,2e-5);
            }
            // Curve parameter order differs from line-A order: nearest hit is the later curve event.
            using var offset=context.CreateCircle(3,new(new(12,0,0),new(0,1,0),new(0,0,1)));
            var nearest=ProfilePlacement2RailCandidate.Build(context,inner,offset,.25,1,settings);Near(nearest.Height!.Value,2);Near(nearest.OuterParameter!.Value,3*Math.PI/2);
            using var overlap=context.CreateLine(new(10,0,0),new(8,0,0));
            var paired=ProfilePlacement2RailCandidate.Build(context,inner,overlap,.25,1,settings);Near(paired.Height!.Value,1);Near(paired.OuterParameter!.Value,context.InspectCurve(overlap).LastParameter);
            Throws<CandidateGeometryException>(()=>ProfilePlacement2RailCandidate.Build(context,inner,outer,2,1,settings));
        }
        using(var inner=context.CreateCircle(7,new(new(0,2,0),new(0,1,0),new(0,0,1))))
        using(var outer=context.CreateCircle(9,new(new(0,2,0),new(0,1,0),new(0,0,1)))){
            Near(ProfilePlacement2RailCandidate.Build(context,inner,outer,.13,0,settings).Height!.Value,2);
            Check(ProfilePlacement2RailCandidate.Build(context,inner,outer,.13,2,settings).Height is null,"Source Y-scaled ray was projected onto the outer plane");
        }
        using var stream=typeof(ProfilePlacementAssertions).Assembly.GetManifestResourceStream("RingRailLibraryFixture")!;
        using var memory=new MemoryStream();stream.CopyTo(memory);var original=memory.ToArray();var document=LegacyCsg4Codec.Read(original);
        using(var graph=new CandidateRailGraphSession(context,document,settings)){
            Check(!graph.ReadOutput(110,4).HasNumber,"Invalid placement output not empty before generation");
            graph.Generate(112,119,126);
            foreach(var path in new[]{(110,111,112,97),(117,118,119,97),(124,125,126,99)}){
                var matrix=graph.ReadOutput(path.Item3,0).Transform??throw new Exception("Original scale chain did not regenerate");
                double height=Number(graph.ReadOutput(path.Item1,3));Check(height>=.01,"Actual original placement height missing");
                Near(Number(graph.ReadOutput(path.Item2,0)),height+document.Records[path.Item4].Scalar!.Value);
                var block=document.Records[path.Item3].SimpleBlock!;var selection=graph.Values.ReadOutput(block.ObjectIndices[0],block.ChannelIndices[0]).Profile!.Value;
                using var profile=StoredMatrixProfileCandidate.Build(context,document.Records[selection.GroupRecordIndex].MatrixProfiles!,selection.ProfileIndex);
                using var scaled=context.TransformAffine(profile.Curve,Native(matrix));var bounds=context.GetCurveBounds(scaled,1e-10);
                Near(bounds.Maximum.Y-bounds.Minimum.Y,height+document.Records[path.Item4].Scalar!.Value,3e-5);
            }
            Check(LegacyCsg4Codec.Write(graph.Values.ExportCurrentDocument()).AsSpan().SequenceEqual(original),"Placement generation changed stored archive");
            graph.Values.SetValue(94,0);graph.Generate(112);Near(Number(graph.ReadOutput(110,3)),4.38);
            graph.Values.SetValue(8,4.88);graph.Generate(112);Near(Number(graph.ReadOutput(110,3)),4.88);
            var saved=LegacyCsg4Codec.Read(LegacyCsg4Codec.Write(graph.Values.ExportCurrentDocument()));
            using(var reopened=new CandidateRailGraphSession(context,saved,settings)){
                reopened.Generate(112);Near(Number(reopened.ReadOutput(110,3)),4.88);
                var a=Fields(graph.ReadOutput(112,0).Transform!.Value);var b=Fields(reopened.ReadOutput(112,0).Transform!.Value);
                for(int i=0;i<a.Length;i++)Near(a[i],b[i],1e-10);
            }
            graph.Values.SetValue(8,40);graph.Generate(110);
            Check(!graph.ReadOutput(110,2).HasNumber&&!graph.ReadOutput(110,3).HasNumber,"No-hit generation retained stale outer/height outputs");
            graph.Values.SetValue(8,4.38);graph.Generate(110);Near(Number(graph.ReadOutput(110,3)),4.38);
            var previous=graph.ReadOutput(110,0).Transform!.Value;ulong entries=context.DiagnosticLiveShapeCount;
            graph.Values.SetValue(94,2);Throws<CandidateGeometryException>(()=>graph.Generate(110));
            Check(graph.ReadOutput(110,0).Transform!.Value==previous&&context.DiagnosticLiveShapeCount==entries,"Failed placement corrupted cache or leaked native entries");
        }
        var forwarded=document.WithScalarInputLink(196,2,110,-1);
        using(var graph=new CandidateRailGraphSession(context,forwarded,settings)){
            graph.Values.SetValue(196,3);graph.Generate(196);
            Near(Number(graph.Values.ReadOutput(196,3)),Number(graph.ReadOutput(110,3)));
        }
        using(var graph=new CandidateRailGraphSession(context,document.WithInputLink(110,1,-1,-5),settings)){
            graph.Generate(110);Check(!graph.ReadOutput(110,2).HasNumber&&!graph.ReadOutput(110,3).HasNumber,"Missing outside graph outputs not empty");
        }
        using(var graph=new CandidateRailGraphSession(context,document.WithInputLink(110,0,11,0),settings))Throws<NotSupportedException>(()=>graph.Generate(110));
        using(var graph=new CandidateRailGraphSession(context,document.WithInputLink(110,0,110,0),settings))Throws<InvalidOperationException>(()=>graph.Generate(110));
        Check(context.DiagnosticLiveShapeCount==0,"Placement retained native entries");
        return "PASS two-rail profile placement:"+sourceCases+" extracted frame/seam cases; actual native circle/ellipse/3D/finite ray/overlap/order/height floor, original3 placement->Add->scale chains, native sizing/edit/save/reopen/chooser/no-hit reset/failure/cycle/cleanup; candidate subset, no original-PC parity.";
    }
}
