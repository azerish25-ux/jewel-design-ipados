using JewelDesign.Documents;
using JewelDesign.Features.Candidate;
using JewelDesign.Geometry.Candidate;
namespace CandidateGeometryContractChecks;
internal static class SelectRailAssertions
{
    private static void Check(bool value,string message){if(!value)throw new Exception(message);}
    private static void Near(double actual,double expected){if(!double.IsFinite(actual)||Math.Abs(actual-expected)>1e-5)throw new Exception("Selector point mismatch");}
    private static LegacyChannelValue Read(CandidateRailGraphSession graph,int record,int channel)=>record<0?LegacyChannelValue.Missing:graph.ReadOutput(record,channel);
    private static void Same(CandidateContext context,CandidateRailGraphSession graph,LegacyChannelValue actual,LegacyChannelValue expected)
    {
        Check(actual.IsMissing==expected.IsMissing&&actual.HasNumber==expected.HasNumber&&actual.Profile==expected.Profile&&actual.Curve.HasValue==expected.Curve.HasValue,"Selector forwarded wrong data kind");
        if(expected.HasNumber){double a=0,e=0;actual.GetDouble(ref a);expected.GetDouble(ref e);Check(BitConverter.DoubleToInt64Bits(a)==BitConverter.DoubleToInt64Bits(e),"Selector changed number bits");}
        if(expected.Curve is{} reference){
            using var upstream=graph.RetainCurve(reference.ProducerRecordIndex,reference.OutputChannelIndex);using var chosen=graph.RetainCurve(16);
            Check(graph.ReadMetadata(16)==graph.ReadMetadata(reference.ProducerRecordIndex),"Forwarded rail metadata changed");
            var source=context.InspectCurve(upstream);var selected=context.InspectCurve(chosen);
            for(int i=0;i<=16;i++){var a=context.PointAt(upstream,source.FirstParameter+(source.LastParameter-source.FirstParameter)*i/16);var b=context.PointAt(chosen,selected.FirstParameter+(selected.LastParameter-selected.FirstParameter)*i/16);Near(a.X,b.X);Near(a.Y,b.Y);Near(a.Z,b.Z);}
        }
    }
    public static string Run()
    {
        using var stream=typeof(SelectRailAssertions).Assembly.GetManifestResourceStream("RingRailLibraryFixture")!;
        using var memory=new MemoryStream();stream.CopyTo(memory);var document=LegacyCsg4Codec.Read(memory.ToArray());
        using var context=CandidateContext.Create();var settings=new OutsideRailCandidateSettings(1e-7,1e-7,1e-7);
        Check(document.Records[16].SimpleBlock is not null,"Selector header missing");
        var inputs=new[]{(-1,-5),(13,99),(8,-5),(7,0),(4,0)};
        double[] controls={0,.5,Math.BitIncrement(.5),double.NaN,double.PositiveInfinity,double.NegativeInfinity};
        int cases=0;
        foreach(var first in inputs)foreach(var second in inputs)foreach(double control in controls){
            var wired=document.WithInputLink(16,0,first.Item1,first.Item2).WithInputLink(16,1,second.Item1,second.Item2).WithInputLink(16,2,95,0);
            using var graph=new CandidateRailGraphSession(context,wired,settings);graph.Values.SetValue(95,control);graph.Generate(16);
            var oracle=new Recovered.SelectRails.CSSelectRingRail(Read(graph,first.Item1,first.Item2),Read(graph,second.Item1,second.Item2),graph.Values.ReadOutput(95,0));
            Check(oracle.Update(),"Extracted source Update failed");Same(context,graph,graph.ReadOutput(16,0),oracle.oRingRail.Data?.Value??LegacyChannelValue.Missing);cases++;
        }
        // Failed Boolean conversion and disconnected control retain source defaultfalse.
        foreach(var control in new[]{(-1,-5),(13,99),(4,0),(7,0)}){
            var wired=document.WithInputLink(16,2,control.Item1,control.Item2);
            using var graph=new CandidateRailGraphSession(context,wired,settings);graph.Generate(16);
            Check(graph.ReadMetadata(16).Kind==CandidateRailOutputKind.Ring,"Non-Boolean selector control did not defaultfalse");
        }
        CandidateShape retained;double radius=document.Records[2].Scalar!.Value;
        using(var graph=new CandidateRailGraphSession(context,document,settings)){
            Check(!graph.ReadOutput(16,-1).IsMissing,"Invalid output is an empty CSData, not null");
            graph.Generate(16);retained=graph.RetainCurve(16);Near(graph.ReadMetadata(16).Radius!.Value,radius);
            Check(LegacyCsg4Codec.Write(graph.Values.ExportCurrentDocument()).AsSpan().SequenceEqual(memory.ToArray()),"Selection generation changed document");
            graph.Values.SetValue(2,radius+.5);graph.Generate(4);
            using(var oldSelection=graph.RetainCurve(16)){var info=context.InspectCurve(oldSelection);Near(context.PointAt(oldSelection,info.FirstParameter).Z,radius);}
            graph.Generate(16);Near(graph.ReadMetadata(16).Radius!.Value,radius+.5);
            graph.Values.SetValue(15,1);graph.Generate(16);Check(graph.ReadMetadata(16).Kind==CandidateRailOutputKind.Outside,"Selection edit did not forward outside data");
            var saved=LegacyCsg4Codec.Read(LegacyCsg4Codec.Write(graph.Values.ExportCurrentDocument()));
            using var reopened=new CandidateRailGraphSession(context,saved,settings);reopened.Generate(16);Check(reopened.ReadMetadata(16).Kind==CandidateRailOutputKind.Outside,"Saved selection changed");
            using var curve=reopened.RetainCurve(16);var state=context.InspectCurve(curve);Near(context.PointAt(curve,state.FirstParameter).Z,radius+.5+4.38);
        }
        Check(context.DiagnosticLiveShapeCount==1,"Selector output alias ownership leak");
        var old=context.InspectCurve(retained);Near(context.PointAt(retained,old.FirstParameter).Z,radius);retained.Dispose();
        var downstream=document.WithInputLink(16,1,-1,-5).WithInputLink(13,1,16,0);
        using(var graph=new CandidateRailGraphSession(context,downstream,settings)){
            graph.Generate(13);using var curve=graph.RetainCurve(13);var info=context.InspectCurve(curve);Near(context.PointAt(curve,info.FirstParameter).Z,radius+4.38);
        }
        using(var cycle=new CandidateRailGraphSession(context,document.WithInputLink(16,0,16,0),settings)){
            bool rejected=false;try{cycle.Generate(16);}catch(InvalidOperationException){rejected=true;}Check(rejected,"Selector cycle accepted");
        }
        Check(context.DiagnosticLiveShapeCount==0,"Selector native ownership leak");
        return "PASS select ring rail:"+cases+" exact-source forwarding cases, null vs empty/numeric/profile/native curve, Boolean defaults/threshold/NaN, saved selection/edit/reopen/downstream and retained generations/disposal; known candidate subset.";
    }
}
