using JewelDesign.Documents;
using JewelDesign.Features.Candidate;
using JewelDesign.Geometry.Candidate;
using Oracle=CandidateGeometryContractChecks.Recovered.ArrayQueries;
namespace CandidateGeometryContractChecks;

internal static class ArrayConsumerAssertions
{
    private static void Check(bool v,string message){if(!v)throw new Exception(message);}
    private static void Near(double a,double b,double tolerance=1e-6){if(!double.IsFinite(a)||!double.IsFinite(b)||Math.Abs(a-b)>tolerance)throw new Exception("Array consumer mismatch "+a+" vs "+b);}
    private static void Throws<T>(Action action)where T:Exception{try{action();}catch(T){return;}throw new Exception("Expected "+typeof(T).Name);}
    private static double[] Fields(LegacyAffineTransform m)=>new[]{m.M11,m.M12,m.M13,m.M14,m.M21,m.M22,m.M23,m.M24,m.M31,m.M32,m.M33,m.M34};
    private static void Matrix(LegacyAffineTransform a,LegacyAffineTransform b){var x=Fields(a);var y=Fields(b);for(int i=0;i<x.Length;i++)Near(x[i],y[i],1e-12);}
    private static CandidatePoint3 Apply(LegacyAffineTransform m,CandidatePoint3 p)=>new(m.M11*p.X+m.M12*p.Y+m.M13*p.Z+m.M14,m.M21*p.X+m.M22*p.Y+m.M23*p.Z+m.M24,m.M31*p.X+m.M32*p.Y+m.M33*p.Z+m.M34);
    private static void Point(CandidatePoint3 a,CandidatePoint3 b){Near(a.X,b.X);Near(a.Y,b.Y);Near(a.Z,b.Z);}
    private static LegacyCsg4Document Wire(LegacyCsg4Document d,int index,params (int Node,int Channel)[] links)
    {var result=d;for(int i=0;i<d.Records[index].SimpleBlock!.ObjectIndices.Count;i++)result=result.WithInputLink(index,i,i<links.Length?links[i].Node:-1,i<links.Length?links[i].Channel:-5);return result;}
    private static IEnumerable<LegacyChannelValue> Leaves(LegacyChannelValue value)
    {if(value.Array is{} a){foreach(var item in a)foreach(var leaf in Leaves(item))yield return leaf;}else yield return value;}
    private static int FirstCases()
    {
        var n=LegacyChannelValue.FromNumber(2);var empty=LegacyChannelValue.Empty;var missing=LegacyChannelValue.Missing;var curve=LegacyChannelValue.FromCurve(4,0);
        var values=new[]{n,empty,missing,curve,LegacyChannelValue.FromTransform(LegacyAffineTransform.Identity),LegacyChannelValue.FromArray(Array.Empty<LegacyChannelValue>()),LegacyChannelValue.FromArray(new[]{n,curve})};
        int count=0;
        foreach(var a in values)foreach(var b in values){
            var value=LegacyChannelValue.FromArray(new[]{a,b});
            var observed=Oracle.Values.From(value,_=>null,_=>null);
            Check(ReferenceEquals(LegacyArrayData.FirstItem(value),Oracle.Values.First(observed)),"Source first-item conversion differs");count++;
        }
        return count;
    }
    public static string Run()
    {
        int firstCases=FirstCases();
        using var stream=typeof(ArrayConsumerAssertions).Assembly.GetManifestResourceStream("RingRailLibraryFixture")!;
        using var memory=new MemoryStream();stream.CopyTo(memory);var original=memory.ToArray();var document=LegacyCsg4Codec.Read(original);
        using var context=CandidateContext.Create();var settings=new OutsideRailCandidateSettings(1e-7,1e-7,1e-7);
        // Nested transformation inputs flatten; nested object arrays preserve structure and replace null children with empty data.
        var mixed=Wire(document,16,(-1,-5),(-1,-5),(-1,-5));
        mixed=Wire(mixed,133,(131,0),(4,0),(16,0));mixed=Wire(mixed,132,(49,0),(51,0),(50,0));mixed=Wire(mixed,81,(132,0),(110,0));
        mixed=Wire(mixed,52,(133,0),(81,0));
        CandidateDataSnapshot retained;
        using(var graph=new CandidateRailGraphSession(context,mixed,settings)){
            graph.Values.SetValue(40,1);graph.Values.SetValue(41,2);graph.Values.SetValue(42,3);graph.Values.SetValue(43,20);graph.Generate(52);
            using var source=graph.CaptureOutput(133);retained=graph.CaptureOutput(52);
            Check(retained.Value.Array!.Count==3&&retained.Value.Array[0].Array!.Count==3&&!retained.Value.Array[2].IsMissing,"Object nesting/null copy changed");
            var before=Leaves(source.Value).Where(x=>x.Curve.HasValue).ToArray();var after=Leaves(retained.Value).Where(x=>x.Curve.HasValue).ToArray();Check(before.Length==4&&after.Length==4,"Array application dropped curves");
            var matrices=new List<LegacyAffineTransform>();foreach(int index in new[]{49,51,50,110}){Check(graph.TryReadTransform(index,0,out var m),"Expected transform missing");matrices.Add(m);}
            var combined=LegacyAffineTransform.Identity;foreach(var m in matrices)combined=m*combined;
            for(int leaf=0;leaf<4;leaf++){
                using var a=source.RetainCurve(before[leaf]);using var b=retained.RetainCurve(after[leaf]);var domain=context.InspectCurve(a);
                for(int i=0;i<=16;i++){double t=domain.FirstParameter+(domain.LastParameter-domain.FirstParameter)*i/16;var p=context.PointAt(a,t);foreach(var m in matrices)p=Apply(m,p);Point(context.PointAt(b,t),p);}
                if(leaf<3){Check(source.ReadProfileMetadata(before[leaf]).Name==retained.ReadProfileMetadata(after[leaf]).Name,"Profile name dropped in array transform");Check(source.ReadProfileMetadata(before[leaf]).SweepParameters.SequenceEqual(retained.ReadProfileMetadata(after[leaf]).SweepParameters),"Sweep parameters changed");Matrix(retained.ReadTransform(after[leaf]),combined*source.ReadTransform(before[leaf]));}
                else Matrix(retained.ReadTransform(after[leaf]),source.ReadTransform(before[leaf])*combined);
            }
            // A nested array's transform capability is false; a flat array uses its first non-array item.
            Check(!graph.TryReadTransform(52,0,out _),"Nested first item was recursively treated as transform");
            Check(graph.TryReadTransform(132,0,out var first),"Flat transform array first capability missing");Check(graph.TryReadTransform(49,0,out var scale),"Scale missing");Matrix(first,scale);
            var saved=LegacyCsg4Codec.Read(LegacyCsg4Codec.Write(graph.Values.ExportCurrentDocument()));
            using var reopened=new CandidateRailGraphSession(context,saved,settings);reopened.Generate(52);using var loaded=reopened.CaptureOutput(52);
            var loadedLeaves=Leaves(loaded.Value).Where(x=>x.Curve.HasValue).ToArray();Check(loadedLeaves.Length==4,"Reopened array lost geometry");
            for(int i=0;i<4;i++){using var a=retained.RetainCurve(after[i]);using var b=loaded.RetainCurve(loadedLeaves[i]);var d=context.InspectCurve(a);Point(context.PointAt(a,(d.FirstParameter+d.LastParameter)/2),context.PointAt(b,(d.FirstParameter+d.LastParameter)/2));}
            ulong entries=context.DiagnosticLiveShapeCount;
            var oldOutput=graph.ReadOutput(52,0);graph.Values.SetValue(35,0);Throws<CandidateGeometryException>(()=>graph.Generate(52));Check(ReferenceEquals(oldOutput,graph.ReadOutput(52,0))&&context.DiagnosticLiveShapeCount==entries,"Native array failure replaced cache or leaked entries");graph.Values.SetValue(35,document.Records[35].Scalar!.Value);
            graph.Values.SetValue(43,double.NaN);Throws<NotSupportedException>(()=>graph.Generate(52));Check(context.DiagnosticLiveShapeCount==entries,"Failed array application leaked entries");
        }
        foreach(var leaf in Leaves(retained.Value).Where(x=>x.Curve.HasValue)){using var curve=retained.RetainCurve(leaf);context.InspectCurve(curve);}retained.Dispose();
        Check(context.DiagnosticLiveShapeCount==0,"Recursive application snapshot/graph cleanup leaked");
        // Source bbox aggregation over all curve leaves, using actual native leaf boxes.
        var boundsDoc=document.WithInputLink(49,0,131,0);
        using(var graph=new CandidateRailGraphSession(context,boundsDoc,settings)){
            graph.Generate(49);using var snap=graph.CaptureOutput(131);
            CandidateCurveBounds? Bounds(LegacyChannelValue v){if(!v.Curve.HasValue&&!v.Profile.HasValue)return null;using var c=snap.RetainCurve(v);return context.GetCurveBounds(c,settings.BoundsTolerance);}
            LegacyAffineTransform? Transform(LegacyChannelValue v)=>v.Transform??(v.Curve.HasValue||v.Profile.HasValue?snap.ReadTransform(v):null);
            var observed=(Oracle.CSDataArray)Oracle.Values.From(snap.Value,Bounds,Transform);var box=Oracle.BoundingBox.Unset;
            Check(observed.GetTightBoundingBox(ref box,false,Oracle.Transform.Identity),"Source native observed bbox missing");
            var expected=TransformProducerCandidate.ScaleToSize(new(box.Min,box.Max,settings.BoundsTolerance,0),LegacyChannelValue.FromNumber(document.Records[35].Scalar!.Value),LegacyChannelValue.FromNumber(document.Records[36].Scalar!.Value),LegacyChannelValue.Missing);
            Matrix(graph.ReadOutput(49,0).Transform!.Value,expected);
            Check(observed.GetTransform(out var sourceTransform)&&graph.TryReadTransform(131,0,out _),"Flat profile array transform capability differs");Check(graph.TryReadTransform(131,0,out var actual),"Array matrix missing");Matrix(actual,sourceTransform.Matrix);
        }
        using(var graph=new CandidateRailGraphSession(context,Wire(document,131,(8,0),(9,0)).WithInputLink(49,0,131,0),settings))Throws<NotSupportedException>(()=>graph.Generate(49));
        // Repeated first-item geometry conversion for nested rail input and OutsideRail profile/radius arrays.
        var firstDoc=Wire(Wire(document,132,(4,0),(13,0)),133,(132,0),(13,0)).WithInputLink(110,0,133,0);
        using(var graph=new CandidateRailGraphSession(context,firstDoc,settings)){graph.Generate(110);Check(graph.ReadOutput(110,3).HasNumber,"Nested first rail geometry missing");}
        var missingFirst=Wire(document,16,(-1,-5),(-1,-5),(-1,-5));
        missingFirst=Wire(missingFirst,133);
        foreach(var bad in new[]{(16,0),(133,0),(8,0)}){
            var data=Wire(missingFirst,132,bad,(4,0)).WithInputLink(110,0,132,0);
            using var graph=new CandidateRailGraphSession(context,data,settings);Throws<NotSupportedException>(()=>graph.Generate(110));
        }
        var outside=Wire(Wire(document,132,(7,0),(4,0)),133,(4,0),(8,0)).WithInputLink(13,1,132,0).WithInputLink(13,0,133,0);
        using(var graph=new CandidateRailGraphSession(context,outside,settings)){graph.Generate(13);using var curve=graph.RetainCurve(13);var d=context.InspectCurve(curve);Near(context.PointAt(curve,d.FirstParameter).Z,document.Records[2].Scalar!.Value+4.38);}
        // Provider arithmetic emits all source array channels; non-array scalar fallback remains distinct.
        var arithmetic=Wire(Wire(document,132,(8,0),(9,0),(11,0)),133,(10,0),(8,0),(11,0)).WithInputLink(111,0,132,0).WithInputLink(111,1,133,0);
        using(var graph=new CandidateRailGraphSession(context,arithmetic,settings)){
            graph.Generate(111);var left=graph.ReadOutput(132,0).Array!;var right=graph.ReadOutput(133,0).Array!;Check(graph.OutputChannelCount(111)==3,"Array arithmetic channel count");
            int index=0;foreach(var a in left)foreach(var b in right){double x=0,y=0;a.GetDouble(ref x);b.GetDouble(ref y);double n=x+y;foreach(int channel in new[]{0,1,2}){double actual=0;Check(graph.ReadOutput(111,channel).Array![index].GetDouble(ref actual),"Array arithmetic result missing");Check(BitConverter.DoubleToInt64Bits(actual)==BitConverter.DoubleToInt64Bits(channel==0?n:channel==1?0.0-n:1.0-n),"Array arithmetic source order/bits");}index++;}
        }
        var emptyMath=Wire(Wire(document,132),133).WithInputLink(111,0,132,0).WithInputLink(111,1,133,0);
        using(var graph=new CandidateRailGraphSession(context,emptyMath,settings)){graph.Generate(111);double zero=1;Check(graph.ReadOutput(111,0).Array is null&&graph.ReadOutput(111,0).GetDouble(ref zero)&&zero==0,"Empty arrays lost scalar fallback");}
        var nullMath=Wire(document,16,(-1,-5),(-1,-5),(-1,-5));nullMath=Wire(Wire(nullMath,132,(16,0),(8,0)),133,(9,0)).WithInputLink(111,0,132,0).WithInputLink(111,1,133,0);
        using(var graph=new CandidateRailGraphSession(context,nullMath,settings))Throws<NullReferenceException>(()=>graph.Generate(111));
        Check(context.DiagnosticLiveShapeCount==0,"Array consumer tests leaked native entries");
        return "PASS native array consumers:"+firstCases+" source first-item cases; nested transform flatten/object preservation/null copy, actual native leaf geometry/metadata/order, source aggregate bounds/first transform capability, rail/profile first-item queries, arithmetic array channels, saved reopen/failure/disposal; candidate subset, no original-PC/full data parity.";
    }
}
