using System.Text.Json;
using JewelDesign.Documents;
using JewelDesign.Features.Candidate;
using JewelDesign.Geometry.Candidate;
namespace CandidateGeometryContractChecks;
internal static class RingRailAssertions
{
    private static void Check(bool condition,string message){if(!condition)throw new Exception(message);}
    private static void Near(double a,double b)=>Check(double.IsFinite(a)&&Math.Abs(a-b)<1e-7,"Ring rail actual geometry mismatch");
    private static void Inspect(CandidateContext context,RingRailCandidate rail,double radius)
    {
        Near(rail.Radius,radius);var info=context.InspectCurve(rail.Curve);
        Check(info.Degree==2&&info.PoleCount==9&&info.IsRational,"Ring rail must own actual four-span NURBS");
        Near(info.Length,2*Math.PI*radius);var start=context.PointAt(rail.Curve,info.FirstParameter);
        Near(start.X,0);Near(start.Y,0);Near(start.Z,radius);
    }
    internal static string Run()
    {
        using var context=CandidateContext.Create();
        Check(RingRailV1Candidate.TypeGuid==new Guid("65aa45c6-d1cd-4c45-92bb-b5a9a6a2ec47"),"Legacy ring registration changed");
        using(var fallback=RingRailV1Candidate.Build(context))Inspect(context,fallback,7);
        using var fixture=typeof(RingRailAssertions).Assembly.GetManifestResourceStream("RingRailLibraryFixture")!;
        using var memory=new MemoryStream();fixture.CopyTo(memory);var bytes=memory.ToArray();var document=LegacyCsg4Codec.Read(bytes);
        using var input=typeof(RingRailAssertions).Assembly.GetManifestResourceStream("RingRailInputOracle")!;using var oracle=JsonDocument.Parse(input);
        var link=oracle.RootElement.GetProperty("recordedInputCandidates")[0];
        Check(link.GetProperty("railIndex").GetInt32()==4&&link.GetProperty("sourceIndex").GetInt32()==2&&link.GetProperty("sourceChannel").GetInt32()==0,"Fixture source link changed");
        Check(document.Records[4].TypeGuid==RingRailV1Candidate.TypeGuid&&document.Records[2].TypeGuid==LegacyCsg4Codec.RingTypeGuid&&document.Records[2].Version==new LegacyCsg4Version(1,2),"Fixture source profile changed");
        double radius=document.Records[2].Scalar!.Value;Near(radius,8.667196228921199);
        using(var original=RingRailV1Candidate.Build(context,radius)){
            Inspect(context,original,radius);
            var editor=new LegacyCsg4EditSession(document);editor.BeginEditTransaction();editor.SetValue(editor.GetParameter(2),radius+.5);editor.EndEditTransaction();
            var reopened=LegacyCsg4Codec.Read(editor.ExportArchiveData());double editedRadius=reopened.Records[2].Scalar!.Value;
            using(var changed=RingRailV1Candidate.Build(context,editedRadius))Inspect(context,changed,radius+.5);
            Inspect(context,original,radius);
            Check(editor.Undo(),"Radius undo missing");Check(editor.ExportArchiveData().AsSpan().SequenceEqual(bytes),"Radius undo changed archive");
            using(var restored=RingRailV1Candidate.Build(context,editor.Document.Records[2].Scalar!.Value))Inspect(context,restored,radius);
        }
        Check(context.DiagnosticLiveShapeCount==0,"Ring rail subcomponent retained native entries");
        return "PASS ring rail candidate: source default7/ZX radius plan, actual NURBS output, recorded library radius edit/save/reopen/rebuild/undo; stored input supplied explicitly, no full channel/graph/UI/export/PC parity claim.";
    }
}
