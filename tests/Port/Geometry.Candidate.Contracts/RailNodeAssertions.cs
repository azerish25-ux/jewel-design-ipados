using JewelDesign.Documents;
using JewelDesign.Features.Candidate;
using JewelDesign.Geometry.Candidate;
namespace CandidateGeometryContractChecks;

internal static class RailNodeAssertions
{
    private static void Check(bool value,string message){if(!value)throw new Exception(message);}
    private static void Near(double actual,double expected){if(!double.IsFinite(actual)||Math.Abs(actual-expected)>1e-5)throw new Exception("Record-bound rail geometry mismatch");}
    private static void Top(CandidateContext context,OutsideRailCandidate result,double expected)
    {
        Check(result.Diagnostics.Count==0&&context.IsCurveClosed(result.Curve),"Record-bound outside rail fallback/closure");
        var info=context.InspectCurve(result.Curve);var first=context.PointAt(result.Curve,info.FirstParameter);
        Near(first.X,0);Near(first.Y,0);Near(first.Z,expected);
    }
    public static string Run()
    {
        using var stream=typeof(RailNodeAssertions).Assembly.GetManifestResourceStream("RingRailLibraryFixture")!;
        using var memory=new MemoryStream();stream.CopyTo(memory);var bytes=memory.ToArray();var document=LegacyCsg4Codec.Read(bytes);
        using var context=CandidateContext.Create();var settings=new OutsideRailCandidateSettings(1e-7,1e-7,1e-7);
        double radius=document.Records[2].Scalar!.Value;
        using(var ring=RingRailV1Candidate.BuildFromNode(context,new(document),4)){Near(ring.Radius,radius);Near(context.PointAt(ring.Curve,0).Z,radius);}
        using(var outside=OutsideRailV1Candidate.BuildFromNode(context,new(document),13,settings))Top(context,outside,radius+4.38);
        var rewired=document.WithInputLink(13,2,9,0);
        var reopened=LegacyCsg4Codec.Read(LegacyCsg4Codec.Write(rewired));
        using(var changed=OutsideRailV1Candidate.BuildFromNode(context,new(reopened),13,settings))Top(context,changed,radius+1.84);
        using(var restored=OutsideRailV1Candidate.BuildFromNode(context,new(reopened.WithInputLink(13,2,8,0)),13,settings))Top(context,restored,radius+4.38);
        using(var defaultRing=RingRailV1Candidate.BuildFromNode(context,new(document.WithInputLink(4,0,-1,-5)),4))Near(defaultRing.Radius,7);
        using(var defaultTop=OutsideRailV1Candidate.BuildFromNode(context,new(document.WithInputLink(13,2,-1,-5)),13,settings))Top(context,defaultTop,radius+3);
        using(var nonNumericTop=OutsideRailV1Candidate.BuildFromNode(context,new(document.WithInputLink(13,2,7,0)),13,settings))Top(context,nonNumericTop,radius+3);
        Check(context.DiagnosticLiveShapeCount==0,"Record-bound builders retained native entries");
        return "PASS record-bound rail builders:actual saved RingRail/OutsideRail links, generation, consumer rewire/save/reopen/rebuild/reversal, disconnected/non-numeric defaults and ownership; no full scheduler/PC claim.";
    }
}
