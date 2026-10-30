using JewelDesign.Documents;
using JewelDesign.Features.Candidate;
using JewelDesign.Geometry.Candidate;
using Oracle=CandidateGeometryContractChecks.Recovered.SweepPreparation;
namespace CandidateGeometryContractChecks;

internal static class SweepPreparationAssertions
{
    private static void Check(bool v,string message){if(!v)throw new Exception(message);}
    private static void Near(double a,double b,double tolerance=1e-7){if(!double.IsFinite(a)||!double.IsFinite(b)||Math.Abs(a-b)>tolerance)throw new Exception("Sweep preparation mismatch "+a+" vs "+b);}
    private static void Point(CandidatePoint3 a,CandidatePoint3 b){Near(a.X,b.X);Near(a.Y,b.Y);Near(a.Z,b.Z);}
    private static void Throws<T>(Action action)where T:Exception{try{action();}catch(T){return;}throw new Exception("Expected "+typeof(T).Name);}
    public static string Run()
    {
        int sourceCases=0;double[] choices={-1,0,1,double.NaN,BitConverter.Int64BitsToDouble(long.MinValue)};
        for(int length=0;length<=5;length++){
            int patterns=(int)Math.Pow(choices.Length,length);
            for(int pattern=0;pattern<patterns;pattern++){
                int n=pattern;var values=new double[length];for(int i=0;i<length;i++){values[i]=choices[n%choices.Length];n/=choices.Length;}
                Check(Sweep2PreparationCandidate.StableOrder(values).SequenceEqual(Oracle.SourcePlan.Order(values)),"Source stable first-parameter ordering differs");sourceCases++;
            }
        }
        using var context=CandidateContext.Create();
        using(var line1=context.CreateLine(new(0,0,0),new(10,0,0)))
        using(var line2=context.CreateLine(new(0,5,0),new(20,5,0)))
        using(var p0=context.CreateLine(new(8,0,0),new(8,5,0)))
        using(var p1=context.CreateLine(new(0,0,1),new(0,5,1)))
        using(var p2=context.CreateLine(new(8,0,2),new(8,5,2))){
            var a=new[]{8.0,-2,8};var b=new[]{99.0,-3,-3};var profiles=new[]{p0,p1,p2};
            var da=context.InspectCurve(line1);var db=context.InspectCurve(line2);
            var source=Oracle.SourcePlan.Clamp(a,b,da.FirstParameter,da.LastParameter,db.FirstParameter,db.LastParameter);
            var order=Oracle.SourcePlan.Order(source.Item1);
            using var prepared=Sweep2PreparationCandidate.Prepare(context,line1,line2,profiles,a,b,true,true);
            Check(prepared.RequestedClosed&&!prepared.EffectiveClosed&&prepared.MaintainHeight,"Open rails forced closed or height flag lost");
            Check(prepared.SourceProfileOrder.SequenceEqual(order)&&order.SequenceEqual(new[]{1,0,2}),"Secondary parameter reordered ties");
            for(int i=0;i<3;i++){Near(prepared.Parameters1[i],source.Item1[order[i]]);Near(prepared.Parameters2[i],source.Item2[order[i]]);Point(context.PointAt(prepared.Profiles[i],context.InspectCurve(prepared.Profiles[i]).FirstParameter),context.PointAt(profiles[order[i]],context.InspectCurve(profiles[order[i]]).FirstParameter));}
            Check(a.SequenceEqual(new[]{8.0,-2,8})&&b.SequenceEqual(new[]{99.0,-3,-3}),"Preparation mutated input parameter arrays");
            using var solid=context.CreateBox(1,1,1);
            ulong count=context.DiagnosticLiveShapeCount;
            Throws<CandidateGeometryException>(()=>Sweep2PreparationCandidate.Prepare(context,line1,line2,new[]{p0,solid,p2},a,b,false,false));
            Check(context.DiagnosticLiveShapeCount==count,"Noncurve profile failure leaked partial preparation");
            Throws<ArgumentException>(()=>Sweep2PreparationCandidate.Prepare(context,line1,line2,profiles,new[]{1.0},b,false,false));
            Throws<NotSupportedException>(()=>Sweep2PreparationCandidate.Prepare(context,line1,line2,profiles,new[]{double.NaN,0,1},b,false,false));
            Check(context.DiagnosticLiveShapeCount==count,"Validation failure retained curves");
            using var circle=context.CreateCircle(7,CandidatePlane.WorldZX);
            using var mixed=Sweep2PreparationCandidate.Prepare(context,circle,line2,profiles,new[]{-1.0,9,0},b,true,false);
            Check(!mixed.EffectiveClosed,"Mixed rails moved to periodic closed path");
            Near(context.InspectCurve(mixed.Rail1).FirstParameter,context.InspectCurve(circle).FirstParameter);
        }
        using(var first=context.CreateCircle(7,CandidatePlane.WorldZX))
        using(var second=context.CreateCircle(9,CandidatePlane.WorldZX))
        using(var profile=context.CreateLine(new(7,0,0),new(9,0,0))){
            var a=new[]{Math.PI/2,0.0,3*Math.PI/2};var b=a.ToArray();var profiles=new[]{profile,profile,profile};
            using var prepared=Sweep2PreparationCandidate.Prepare(context,first,second,profiles,a,b,true,false);
            Check(prepared.EffectiveClosed&&prepared.SourceProfileOrder.SequenceEqual(new[]{0,2,1}),"Closed seam mapping/order mismatch");
            for(int i=0;i<3;i++){int source=prepared.SourceProfileOrder[i];Point(context.PointAt(prepared.Rail1,prepared.Parameters1[i]),context.PointAt(first,a[source]));Point(context.PointAt(prepared.Rail2,prepared.Parameters2[i]),context.PointAt(second,b[source]));}
            ulong count=context.DiagnosticLiveShapeCount;
            Throws<CandidateGeometryException>(()=>Sweep2PreparationCandidate.Prepare(context,first,second,profiles,new[]{-1.0,0,1},b,false,false));
            Check(context.DiagnosticLiveShapeCount==count,"Closed seam failure leaked prepared shapes");
        }
        Check(context.DiagnosticLiveShapeCount==0,"Analytical preparation leaked native entries");
        using var stream=typeof(SweepPreparationAssertions).Assembly.GetManifestResourceStream("RingRailLibraryFixture")!;
        using var memory=new MemoryStream();stream.CopyTo(memory);var original=memory.ToArray();var document=LegacyCsg4Codec.Read(original);
        var settings=new OutsideRailCandidateSettings(1e-7,1e-7,1e-7);
        PreparedSweep2Candidate retained;
        using(var graph=new CandidateRailGraphSession(context,document,settings)){
            retained=Sweep2PreparationCandidate.FromNode(context,graph,134);
            Check(retained.Profiles.Count==3&&retained.Parameters1.Count==3&&retained.Parameters2.Count==3,"Original profile/parameter counts");
            var source1=graph.ReadOutput(132,0).Array!.Select(x=>{double n=0;Check(x.GetDouble(ref n),"Source param1");return n;}).ToArray();
            var source2=graph.ReadOutput(133,0).Array!.Select(x=>{double n=0;Check(x.GetDouble(ref n),"Source param2");return n;}).ToArray();
            using var rail1=graph.RetainCurve(4);using var rail2=graph.RetainCurve(13);var d1=context.InspectCurve(rail1);var d2=context.InspectCurve(rail2);
            // Real OutsideRail near-end knots exposed the initial default Segment snap; keep full domain and wrapped geometry strict.
            var moved=context.InspectCurve(retained.Rail2);double period=d2.LastParameter-d2.FirstParameter;
            Near(moved.FirstParameter,source2[0],1e-14);Near(moved.LastParameter,source2[0]+period,1e-14);Near(moved.Length,d2.Length,1e-7);
            for(int i=0;i<=64;i++){double t=source2[0]+period*i/64,wrapped=t;if(wrapped>d2.LastParameter)wrapped-=period;Point(context.PointAt(retained.Rail2,t),context.PointAt(rail2,wrapped));}
            var profiles=graph.ReadOutput(131,0).Array!;
            for(int i=0;i<3;i++){
                int source=retained.SourceProfileOrder[i];Point(context.PointAt(retained.Rail1,retained.Parameters1[i]),context.PointAt(rail1,source1[source]));Point(context.PointAt(retained.Rail2,retained.Parameters2[i]),context.PointAt(rail2,source2[source]));
                using var curve=graph.RetainCurve(profiles[source].Curve!.Value);var domain=context.InspectCurve(curve);
                for(int sample=0;sample<=16;sample++){double t=domain.FirstParameter+(domain.LastParameter-domain.FirstParameter)*sample/16;Point(context.PointAt(retained.Profiles[i],t),context.PointAt(curve,t));}
                Check(retained.ProfileMetadata[i]!.Name==graph.ReadProfileMetadata(profiles[source].Curve!.Value.ProducerRecordIndex,0,profiles[source].Curve!.Value.ItemIndex).Name,"Sorted profile metadata lost");
            }
            Check(LegacyCsg4Codec.Write(graph.Values.ExportCurrentDocument()).AsSpan().SequenceEqual(original),"Preparation changed source document");
            graph.Values.SetValue(2,document.Records[2].Scalar!.Value+.5);
            using var changed=Sweep2PreparationCandidate.FromNode(context,graph,134);
            var saved=LegacyCsg4Codec.Read(LegacyCsg4Codec.Write(graph.Values.ExportCurrentDocument()));
            using var loadedGraph=new CandidateRailGraphSession(context,saved,settings);using var loaded=Sweep2PreparationCandidate.FromNode(context,loadedGraph,134);
            Check(changed.SourceProfileOrder.SequenceEqual(loaded.SourceProfileOrder),"Reopened sweep order changed");
            for(int i=0;i<3;i++){Near(changed.Parameters1[i],loaded.Parameters1[i]);Near(changed.Parameters2[i],loaded.Parameters2[i]);}
            Throws<NotSupportedException>(()=>graph.Generate(134)); // No BRep implementation is silently registered.
        }
        Check(context.DiagnosticLiveShapeCount==5,"Prepared ownership did not survive graph disposal or leaked graph state");
        foreach(var profile in retained.Profiles)context.InspectCurve(profile);retained.Dispose();retained.Dispose();Check(context.DiagnosticLiveShapeCount==0,"Prepared native copies did not release");
        var missingParams=document.WithInputLink(134,3,-1,-5).WithInputLink(134,4,-1,-5);
        using(var graph=new CandidateRailGraphSession(context,missingParams,settings))Throws<NotSupportedException>(()=>Sweep2PreparationCandidate.FromNode(context,graph,134));
        Check(context.DiagnosticLiveShapeCount==0,"Adapter failure leaked native entries");
        return "PASS explicit sweep2 preparation:"+sourceCases+" exact stable-order cases; open/mixed clamps, closed native seam/remap/ties/ownership, original134 actual3 profile copies/metadata/params, real near-knot seam domain/geometry, edit/save/reopen/cleanup; no BRep or original-PC sweep claim.";
    }
}
