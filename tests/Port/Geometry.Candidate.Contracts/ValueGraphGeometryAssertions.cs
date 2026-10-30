using JewelDesign.Documents;
using JewelDesign.Features.Candidate;
using JewelDesign.Geometry.Candidate;
using JewelDesign.Graph;
namespace CandidateGeometryContractChecks;

internal static class ValueGraphGeometryAssertions
{
    private static void Check(bool condition,string message){if(!condition)throw new Exception(message);}
    private static void Near(double actual,double expected){if(!double.IsFinite(actual)||Math.Abs(actual-expected)>1e-5)throw new Exception("Live channel geometry mismatch");}
    public static string Run()
    {
        using var stream=typeof(ValueGraphGeometryAssertions).Assembly.GetManifestResourceStream("RingRailLibraryFixture")!;
        using var memory=new MemoryStream();stream.CopyTo(memory);var bytes=memory.ToArray();var original=LegacyCsg4Codec.Read(bytes);
        SerializedChannelLink[] bindings={new(2,0),new(6,3),new(8,0),new(9,0),new(10,0),new(11,0)};
        var settings=new OutsideRailCandidateSettings(1e-7,1e-7,1e-7);
        using var context=CandidateContext.Create();
        var wired=original.WithScalarInputLink(8,1,9,0);
        var session=new LegacyValueGraphSession(wired);session.SetValue(9,2.1);
        using(var result=OutsideRailV1Candidate.BuildFromChannels(context,session,bindings,settings)){
            var info=context.InspectCurve(result.Curve);Near(context.PointAt(result.Curve,info.FirstParameter).Z,original.Records[2].Scalar!.Value+2.1);
            Check(result.Diagnostics.Count==0,"Live-channel rebuild fallback");
        }
        var saved=LegacyCsg4Codec.Read(LegacyCsg4Codec.Write(session.ExportCurrentDocument()));
        Check(saved.Records[8].Scalar!.Value==2.1,"Runtime override not exported");
        using(var result=OutsideRailV1Candidate.BuildFromChannels(context,new(saved),bindings,settings)){
            var info=context.InspectCurve(result.Curve);Near(context.PointAt(result.Curve,info.FirstParameter).Z,saved.Records[2].Scalar!.Value+2.1);
        }
        var inactive=original.WithScalarInputLink(6,0,11,0);var disabled=new LegacyValueGraphSession(inactive);disabled.SetValue(11,0);
        try{using var result=OutsideRailV1Candidate.BuildFromChannels(context,disabled,bindings,settings);throw new Exception("Inactive chooser still produced geometry");}
        catch(OutsideRailCandidateException error){Check(error.Message=="Outside Ring Rail: Missing Custom Rail (No Curve)","Inactive chooser source error");}
        Check(context.DiagnosticLiveShapeCount==0,"Channel geometry temporary ownership");
        return "PASS evaluated value-channel geometry: generated override changes actual selected-profile OutsideRail, state save/reopen rebuild and inactive chooser/missing-curve/ownership guards; explicit input bindings, not full graph/PC parity.";
    }
}
