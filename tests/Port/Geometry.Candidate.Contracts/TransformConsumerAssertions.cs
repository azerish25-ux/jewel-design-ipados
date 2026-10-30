using JewelDesign.Documents;
using JewelDesign.Features.Candidate;
using JewelDesign.Geometry.Candidate;
using JewelDesign.Graph;
using Oracle=CandidateGeometryContractChecks.Recovered.Consumer;
namespace CandidateGeometryContractChecks;

internal static class TransformConsumerAssertions
{
    private static void Check(bool v,string message){if(!v)throw new Exception(message);}
    private static void Near(double a,double b,double tolerance=1e-7){if(!double.IsFinite(a)||!double.IsFinite(b)||Math.Abs(a-b)>tolerance)throw new Exception("Consumer mismatch "+a+" vs "+b);}
    private static void Throws<T>(Action action)where T:Exception{try{action();}catch(T){return;}throw new Exception("Expected "+typeof(T).Name);}
    private static double[] Fields(LegacyAffineTransform m)=>new[]{m.M11,m.M12,m.M13,m.M14,m.M21,m.M22,m.M23,m.M24,m.M31,m.M32,m.M33,m.M34};
    private static void Same(LegacyAffineTransform a,LegacyAffineTransform b){var x=Fields(a);var y=Fields(b);for(int i=0;i<x.Length;i++)Near(x[i],y[i],1e-12);}
    private static CandidatePoint3 Apply(LegacyAffineTransform m,CandidatePoint3 p)=>new(m.M11*p.X+m.M12*p.Y+m.M13*p.Z+m.M14,m.M21*p.X+m.M22*p.Y+m.M23*p.Z+m.M24,m.M31*p.X+m.M32*p.Y+m.M33*p.Z+m.M34);
    private static void SamePoint(CandidatePoint3 a,CandidatePoint3 b){Near(a.X,b.X);Near(a.Y,b.Y);Near(a.Z,b.Z);}
    private static int SocketCases()
    {
        var choices=new[]{new SerializedChannelLink(-1,-5),new(1,0),new(2,-1),new(99,3)};int count=0;
        for(int length=0;length<=5;length++){
            int patterns=1<<(2*length);
            for(int pattern=0;pattern<patterns;pattern++){
                var links=new SerializedChannelLink[length];int n=pattern;
                for(int i=0;i<length;i++){links[i]=choices[n&3];n>>=2;}
                foreach(int records in new[]{0,3,10})foreach(int offset in new[]{0,2,int.MaxValue}){
                    var original=links.ToArray();var actual=LegacyTransformInputLayout.Reconstruct(links,records,offset);
                    var expected=Oracle.CSBlockExpandableInput.Observe(links,records,offset);
                    Check(actual.AllChannels.SequenceEqual(expected),"Source socket replay differs");
                    Check(actual.OperationChannels.SequenceEqual(expected.Take(expected.Length-1)),"Source spare slot exclusion differs");
                    Check(links.SequenceEqual(original),"Socket replay mutated saved links");count++;
                }
            }
        }
        var gap=LegacyTransformInputLayout.Reconstruct(new[]{new SerializedChannelLink(0,0),new(-1,-5),new(1,0)},3);
        Check(gap.OperationChannels.SequenceEqual(new[]{new SerializedChannelLink(0,0),new(1,0)}),"Unlinked expansion slot was not reused");
        var negative=LegacyTransformInputLayout.Reconstruct(new[]{new SerializedChannelLink(0,0),new(1,-5)},3);
        Check(negative.AllChannels.Count==2&&negative.OperationChannels.Count==1,"Negative output index should not create a new spare");
        Throws<NotSupportedException>(()=>((IList<SerializedChannelLink>)gap.AllChannels)[0]=default);
        Throws<ArgumentOutOfRangeException>(()=>LegacyTransformInputLayout.Reconstruct(Array.Empty<SerializedChannelLink>(),-1));
        return count;
    }
    private static LegacyCsg4Document Minimal(LegacyCsg4Document doc,int index,int objectIndex,int objectChannel,int transformIndex,int transformChannel=0)
    {
        var result=doc.WithInputLink(index,0,objectIndex,objectChannel).WithInputLink(index,1,transformIndex,transformChannel);
        for(int i=2;i<doc.Records[index].SimpleBlock!.ObjectIndices.Count;i++)result=result.WithInputLink(index,i,-1,-5);
        return result;
    }
    public static string Run()
    {
        int sourceCases=SocketCases();Check(sourceCases==12285,"Missing source socket cases");
        using var stream=typeof(TransformConsumerAssertions).Assembly.GetManifestResourceStream("RingRailLibraryFixture")!;
        using var memory=new MemoryStream();stream.CopyTo(memory);var original=memory.ToArray();var document=LegacyCsg4Codec.Read(original);
        using var context=CandidateContext.Create();var settings=new OutsideRailCandidateSettings(1e-7,1e-7,1e-7);
        CandidateShape retained;CandidatePoint3 oldPoint;
        using(var graph=new CandidateRailGraphSession(context,document,settings)){
            Check(!graph.ReadOutput(115,1).HasNumber,"Invalid consumer output not empty before generation");
            graph.Generate(115,122,127);
            foreach(int index in new[]{115,122,127}){
                var block=document.Records[index].SimpleBlock!;var raw=block.ObjectIndices.Select((node,i)=>new SerializedChannelLink(node,block.ChannelIndices[i])).ToArray();
                var layout=LegacyTransformInputLayout.Reconstruct(raw,document.Records.Count);
                Check(layout.AllChannels.Count==raw.Length+1&&layout.OperationChannels.Count==raw.Length&&graph.Inputs(index).SequenceEqual(layout.AllChannels),"Original saved runtime slots");
                var profile=graph.Values.ReadOutput(raw[0].ObjectIndex,raw[0].ChannelIndex).Profile!.Value;
                using var imported=StoredMatrixProfileCandidate.Build(context,document.Records[profile.GroupRecordIndex].MatrixProfiles!,profile.ProfileIndex);
                using var generated=graph.RetainCurve(index);var domain=context.InspectCurve(imported.Curve);var info=graph.ReadProfileMetadata(index);
                Check(info.SourceGroupRecordIndex==profile.GroupRecordIndex&&info.SourceProfileIndex==profile.ProfileIndex&&info.Name==imported.Name,"Profile identity/name lost");
                Check(info.SweepParameters.Select(BitConverter.DoubleToInt64Bits).SequenceEqual(imported.SweepParameters.Select(BitConverter.DoubleToInt64Bits)),"Profile sweep parameters changed");
                Throws<NotSupportedException>(()=>((IList<double>)info.SweepParameters)[0]=0);
                var combined=LegacyAffineTransform.Identity;var matrices=new List<LegacyAffineTransform>();
                foreach(var link in layout.OperationChannels.Skip(1)){Check(graph.TryReadTransform(link.ObjectIndex,link.ChannelIndex,out var matrix),"Original transform capability missing");matrices.Add(matrix);combined=matrix*combined;}
                Check(graph.TryReadTransform(index,0,out var applied),"Generated profile transform capability missing");Same(applied,combined);
                for(int i=0;i<=32;i++){
                    double t=domain.FirstParameter+(domain.LastParameter-domain.FirstParameter)*i/32;
                    var expected=context.PointAt(imported.Curve,t);
                    foreach(var matrix in matrices)expected=Apply(matrix,expected);
                    SamePoint(context.PointAt(generated,t),expected);
                }
                Throws<NotSupportedException>(()=>graph.ReadMetadata(index));
            }
            foreach(int index in new[]{52,58,215,221})Throws<NotSupportedException>(()=>graph.Generate(index));
            Check(LegacyCsg4Codec.Write(graph.Values.ExportCurrentDocument()).AsSpan().SequenceEqual(original),"Consumer generation changed document");
            retained=graph.RetainCurve(115);var domainOld=context.InspectCurve(retained);oldPoint=context.PointAt(retained,domainOld.FirstParameter);
            int x=document.Records[114].SimpleBlock!.ObjectIndices[0];graph.Values.SetValue(x,graph.Values.ReadState(x).Numeric.RealValue+35);graph.Generate(115);
            using(var changed=graph.RetainCurve(115)){double change=0;for(int i=0;i<=32;i++){double t=domainOld.FirstParameter+(domainOld.LastParameter-domainOld.FirstParameter)*i/32;var p=context.PointAt(changed,t);var q=context.PointAt(retained,t);change=Math.Max(change,Math.Abs(p.X-q.X)+Math.Abs(p.Y-q.Y)+Math.Abs(p.Z-q.Z));}Check(change>1e-4,"Edited rotation did not change generated profile");}
            SamePoint(context.PointAt(retained,domainOld.FirstParameter),oldPoint);
            var saved=LegacyCsg4Codec.Read(LegacyCsg4Codec.Write(graph.Values.ExportCurrentDocument()));
            using(var reopened=new CandidateRailGraphSession(context,saved,settings)){
                reopened.Generate(115);using var before=graph.RetainCurve(115);using var after=reopened.RetainCurve(115);
                for(int i=0;i<=16;i++){double t=domainOld.FirstParameter+(domainOld.LastParameter-domainOld.FirstParameter)*i/16;SamePoint(context.PointAt(before,t),context.PointAt(after,t));}
                Check(reopened.ReadProfileMetadata(115).Name==graph.ReadProfileMetadata(115).Name,"Reopened profile name changed");
            }
            var old=graph.ReadOutput(115,0).Curve;ulong entries=context.DiagnosticLiveShapeCount;
            graph.Values.SetValue(x,double.NaN);Throws<NotSupportedException>(()=>graph.Generate(115));
            Check(graph.ReadOutput(115,0).Curve==old&&context.DiagnosticLiveShapeCount==entries,"Failed consumer generation corrupted cache or leaked native objects");
        }
        Check(context.DiagnosticLiveShapeCount==1,"Consumer cache disposal invalidated retained reader or leaked shapes");
        retained.Dispose();Check(context.DiagnosticLiveShapeCount==0,"Old generated profile did not release");
        // Nested profile retains name/sweeps and prepends its transformation metadata.
        var nested=Minimal(document,122,115,0,50);
        using(var graph=new CandidateRailGraphSession(context,nested,settings)){
            graph.Values.SetValue(40,1);graph.Values.SetValue(41,2);graph.Values.SetValue(42,3);graph.Generate(122);
            Check(graph.TryReadTransform(115,0,out var before)&graph.TryReadTransform(50,0,out var translation)&graph.TryReadTransform(122,0,out var after),"Nested transform capability");
            Same(after,translation*before);Check(graph.ReadProfileMetadata(115).Name==graph.ReadProfileMetadata(122).Name,"Nested profile kind/name lost");
        }
        // Rails and pure transform objects postmultiply metadata, unlike profiles.
        var rail=Minimal(Minimal(document,122,4,0,50),127,122,0,49);
        using(var graph=new CandidateRailGraphSession(context,rail,settings)){
            graph.Values.SetValue(40,1);graph.Values.SetValue(41,2);graph.Values.SetValue(42,3);graph.Generate(127);
            Check(graph.TryReadTransform(50,0,out var translation)&graph.TryReadTransform(49,0,out var scale)&graph.TryReadTransform(127,0,out var applied),"Rail transform capability");
            Same(applied,translation*scale);Check(graph.ReadMetadata(127).Kind==CandidateRailOutputKind.Ring,"Transformed rail kind lost");
            using var source=graph.RetainCurve(4);using var result=graph.RetainCurve(127);var d=context.InspectCurve(source);
            SamePoint(context.PointAt(result,d.FirstParameter),Apply(scale,Apply(translation,context.PointAt(source,d.FirstParameter))));
        }
        using(var graph=new CandidateRailGraphSession(context,Minimal(document,122,51,0,50),settings)){
            graph.Values.SetValue(43,90);graph.Values.SetValue(44,0);graph.Values.SetValue(45,0);graph.Values.SetValue(40,1);graph.Values.SetValue(41,2);graph.Values.SetValue(42,3);graph.Generate(122);
            Check(graph.TryReadTransform(51,0,out var rotation)&graph.TryReadTransform(50,0,out var translation)&graph.TryReadTransform(122,0,out var actual),"Pure transform object missing");Same(actual,rotation*translation);
        }
        using(var graph=new CandidateRailGraphSession(context,Minimal(document,122,8,0,50),settings)){graph.Values.SetValue(8,BitConverter.Int64BitsToDouble(long.MinValue));graph.Generate(122);double n=1;Check(graph.ReadOutput(122,0).GetDouble(ref n)&&BitConverter.DoubleToInt64Bits(n)==long.MinValue,"Non-transformable scalar bits changed");}
        using(var graph=new CandidateRailGraphSession(context,Minimal(document,122,13,99,50),settings)){graph.Generate(122);Check(!graph.ReadOutput(122,0).HasNumber&&graph.ReadOutput(122,0).Curve is null&&!graph.ReadOutput(122,0).IsMissing,"Empty object was treated as missing");}
        using(var graph=new CandidateRailGraphSession(context,Minimal(document,122,-1,-5,50),settings))Throws<NotSupportedException>(()=>graph.Generate(122));
        using(var graph=new CandidateRailGraphSession(context,Minimal(document,122,49,0,50),settings)){graph.Values.SetValue(35,double.NaN);graph.Generate(122);Check(graph.TryReadTransform(50,0,out var t)&graph.TryReadTransform(122,0,out var actual),"Invalid object transform did not recover on valid application");Same(actual,t);}
        var skipped=Minimal(document,122,4,0,50).WithInputLink(122,2,49,0);
        using(var graph=new CandidateRailGraphSession(context,skipped,settings)){graph.Values.SetValue(35,double.NaN);graph.Generate(122);Check(graph.TryReadTransform(50,0,out var t)&graph.TryReadTransform(122,0,out var actual),"Invalid input transform lost remaining valid inputs");Same(actual,t);}
        var overflow=Minimal(document,122,4,0,50).WithInputLink(122,2,113,0);
        using(var graph=new CandidateRailGraphSession(context,overflow,settings)){
            graph.Values.SetValue(40,1e308);graph.Values.SetValue(102,1e308);graph.Generate(122);
            Check(graph.TryReadTransform(122,0,out var actual),"Overflow skip lost original object transform");Same(actual,LegacyAffineTransform.Identity);
            using var source=graph.RetainCurve(4);using var cloned=graph.RetainCurve(122);var d=context.InspectCurve(source);SamePoint(context.PointAt(source,d.FirstParameter),context.PointAt(cloned,d.FirstParameter));
        }
        // Profiles expose an independent transform capability in addition to their curve data.
        foreach(int producer in new[]{7,115}){
            using var graph=new CandidateRailGraphSession(context,Minimal(document,122,4,0,producer),settings);graph.Generate(122);
            Check(graph.TryReadTransform(producer,0,out var matrix),"Profile transform capability missing");
            using var source=graph.RetainCurve(4);using var result=graph.RetainCurve(122);var d=context.InspectCurve(source);
            SamePoint(context.PointAt(result,d.FirstParameter),Apply(matrix,context.PointAt(source,d.FirstParameter)));
        }
        // A spare socket can retain a negative-index producer: source still schedules its dependency.
        var spare=document.WithInputLink(115,4,95,-5).WithScalarInputLink(95,1,0,0);
        using(var graph=new CandidateRailGraphSession(context,spare,settings))Throws<NotSupportedException>(()=>graph.Generate(115));
        var selected=document.WithInputLink(16,0,115,0).WithInputLink(16,1,-1,-5);
        using(var graph=new CandidateRailGraphSession(context,selected,settings)){graph.Generate(16);Check(graph.ReadProfileMetadata(16).Name==graph.ReadProfileMetadata(115).Name,"Selector dropped generated profile metadata");using var profile=graph.RetainCurve(16);Check(graph.TryReadTransform(16,0,out var a)&graph.TryReadTransform(115,0,out var b),"Selector transform capability missing");Same(a,b);}
        using(var graph=new CandidateRailGraphSession(context,document.WithInputLink(115,0,115,0),settings))Throws<InvalidOperationException>(()=>graph.Generate(115));
        Check(context.DiagnosticLiveShapeCount==0,"Consumer tests retained native entries");
        return "PASS expandable transform consumer:"+sourceCases+" exact source socket cases, original3 native profile pipelines/metadata/parameters/matrix/points, source-specific multiply order, nested/selector copies, edit/save/reopen/retained readers, invalid/missing/overflow/cycle/cleanup; known non-array candidate subset, no original-PC parity.";
    }
}
