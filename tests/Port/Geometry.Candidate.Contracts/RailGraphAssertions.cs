using JewelDesign.Documents;
using JewelDesign.Features.Candidate;
using JewelDesign.Geometry.Candidate;
namespace CandidateGeometryContractChecks;

internal static class RailGraphAssertions
{
    private static void Check(bool value,string message){if(!value)throw new Exception(message);}
    private static void Near(double actual,double expected){if(!double.IsFinite(actual)||Math.Abs(actual-expected)>1e-5)throw new Exception("Rail graph output mismatch");}
    public static string Run()
    {
        using var stream=typeof(RailGraphAssertions).Assembly.GetManifestResourceStream("RingRailLibraryFixture")!;
        using var memory=new MemoryStream();stream.CopyTo(memory);var document=LegacyCsg4Codec.Read(memory.ToArray());
        using var context=CandidateContext.Create();var settings=new OutsideRailCandidateSettings(1e-7,1e-7,1e-7);

        using(var baseline=new CandidateRailGraphSession(context,document,settings)){
            baseline.Generate();
            using var actual=baseline.RetainCurve(13);var info=context.InspectCurve(actual);
            Near(context.PointAt(actual,info.FirstParameter).Z,document.Records[2].Scalar!.Value+4.38);
            Check(LegacyCsg4Codec.Write(baseline.Values.ExportCurrentDocument()).AsSpan().SequenceEqual(memory.ToArray()),"Graph read/generation changed source archive");
        }
        var linked=document.WithInputLink(13,0,4,0).WithInputLink(13,1,4,0);
        CandidateShape? retained=null;
        using(var graph=new CandidateRailGraphSession(context,linked,settings)){
            Check(graph.ReadOutput(13,-1).Curve is null,"Invalid channel should be empty before generation");
            graph.Generate(13);
            Check(graph.ReadOutput(4,0).Curve is not null,"Ring curve channel missing");
            double number=99;Check(!graph.ReadOutput(4,0).GetDouble(ref number)&&number==99,"Ring curve converted to numeric radius");
            Check(graph.ReadOutput(13,1).GetDouble(ref number)&&number==0,"Outside numeric scale output");
            Check(graph.ReadMetadata(13).DataScale==1,"Outside curve data Scale differs from numeric output");
            double radius=linked.Records[2].Scalar!.Value;Near(graph.ReadMetadata(4).Radius!.Value,radius);
            retained=graph.RetainCurve(13);var initial=context.InspectCurve(retained);
            Near(context.PointAt(retained,initial.FirstParameter).Z,radius+4.38);
            graph.Values.SetValue(2,radius+.5);graph.Generate(13);
            using(var changed=graph.RetainCurve(13)){var info=context.InspectCurve(changed);Near(context.PointAt(changed,info.FirstParameter).Z,radius+.5+4.38);}
            Near(context.PointAt(retained,initial.FirstParameter).Z,radius+4.38);
            Check(graph.ReadOutput(13,-1).Curve is null&&!graph.ReadOutput(13,99).HasNumber,"Geometry invalid-index fallback");

            var saved=LegacyCsg4Codec.Read(LegacyCsg4Codec.Write(graph.Values.ExportCurrentDocument()));
            using(var loaded=new CandidateRailGraphSession(context,saved,settings)){
                loaded.Generate(13);using var reopened=loaded.RetainCurve(13);var info=context.InspectCurve(reopened);
                Near(context.PointAt(reopened,info.FirstParameter).Z,radius+.5+4.38);
            }
            graph.Values.SetValue(2,0);
            try{graph.Generate(13);throw new Exception("Invalid ring generated");}catch(CandidateGeometryException){}
            using(var previous=graph.RetainCurve(13)){var info=context.InspectCurve(previous);Near(context.PointAt(previous,info.FirstParameter).Z,radius+.5+4.38);}
        }
        Check(context.DiagnosticLiveShapeCount==1,"Cache disposal invalidated retained curve or leaked outputs");
        retained!.Dispose();Check(context.DiagnosticLiveShapeCount==0,"Retained old output did not release");
        using(var cycle=new CandidateRailGraphSession(context,document.WithInputLink(13,1,13,0),settings)){
            try{cycle.Generate(13);throw new Exception("Rail cycle accepted");}catch(InvalidOperationException){}
        }
        Check(context.DiagnosticLiveShapeCount==0,"Failed/cyclic graph retained native entries");
        return "PASS graph-managed rail outputs:source curve/number/metadata conversions, dependency generation, real curve edges, replacement/retained readers, failure cache/disposal and cycle guards; known candidate subset.";
    }
}
