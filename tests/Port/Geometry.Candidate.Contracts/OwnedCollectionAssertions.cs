using JewelDesign.Documents;
using JewelDesign.Features.Candidate;
using JewelDesign.Geometry.Candidate;
using Oracle=CandidateGeometryContractChecks.Recovered.Collections;
namespace CandidateGeometryContractChecks;

internal static class OwnedCollectionAssertions
{
    private static void Check(bool v,string message){if(!v)throw new Exception(message);}
    private static void Near(double a,double b){if(!double.IsFinite(a)||!double.IsFinite(b)||Math.Abs(a-b)>1e-7)throw new Exception("Collection point mismatch");}
    private static void Point(CandidatePoint3 a,CandidatePoint3 b){Near(a.X,b.X);Near(a.Y,b.Y);Near(a.Z,b.Z);}
    private static void Throws<T>(Action action)where T:Exception{try{action();}catch(T){return;}throw new Exception("Expected "+typeof(T).Name);}
    private static IEnumerable<LegacyChannelValue> Leaves(LegacyChannelValue value)
    {if(value.Array is{} items){foreach(var item in items)foreach(var leaf in Leaves(item))yield return leaf;}else yield return value;}
    private static void Same(CandidateContext context,CandidateRailGraphSession graph,LegacyChannelValue actual,LegacyChannelValue expected)
    {
        Check(actual.IsMissing==expected.IsMissing&&actual.HasNumber==expected.HasNumber&&actual.Profile==expected.Profile&&actual.Transform==expected.Transform&&(actual.Array is null)==(expected.Array is null)&&actual.Curve.HasValue==expected.Curve.HasValue,"Collection value kind mismatch");
        if(actual.HasNumber){double a=0,b=0;actual.GetDouble(ref a);expected.GetDouble(ref b);Check(BitConverter.DoubleToInt64Bits(a)==BitConverter.DoubleToInt64Bits(b),"Collection number bits changed");}
        if(actual.Array is{} items){Check(items.Count==expected.Array!.Count,"Collection item count differs");for(int i=0;i<items.Count;i++)Same(context,graph,items[i],expected.Array[i]);}
        if(actual.Curve is{} reference){
            using var a=graph.RetainCurve(reference);using var b=graph.RetainCurve(expected.Curve!.Value);var domain=context.InspectCurve(a);
            for(int i=0;i<=8;i++){double t=domain.FirstParameter+(domain.LastParameter-domain.FirstParameter)*i/8;Point(context.PointAt(a,t),context.PointAt(b,t));}
        }
    }
    private static LegacyChannelValue[] Observe(CandidateRailGraphSession graph,int record,bool unarray)
    {
        var channels=graph.Inputs(record);var inputs=new List<Oracle.ObservationInput>();
        foreach(var link in channels.Take(channels.Count-1)){
            IReadOnlyList<LegacyChannelValue>? outputs=link.ObjectIndex>=0?Enumerable.Range(0,graph.OutputChannelCount(link.ObjectIndex)).Select(i=>graph.ReadOutput(link.ObjectIndex,i)).ToArray():null;
            var value=link.ObjectIndex<0?LegacyChannelValue.Missing:graph.ReadOutput(link.ObjectIndex,link.ChannelIndex);
            inputs.Add(new(value,outputs!,link.ChannelIndex));
        }
        return Oracle.Values.Run(unarray,inputs);
    }
    private static LegacyCsg4Document Wire(LegacyCsg4Document document,int record,params int[] producers)
    {var result=document;var block=document.Records[record].SimpleBlock!;for(int i=0;i<block.ObjectIndices.Count;i++)result=result.WithInputLink(record,i,i<producers.Length?producers[i]:-1,i<producers.Length?0:-5);return result;}
    public static string Run()
    {
        using var stream=typeof(OwnedCollectionAssertions).Assembly.GetManifestResourceStream("RingRailLibraryFixture")!;
        using var memory=new MemoryStream();stream.CopyTo(memory);var original=memory.ToArray();var document=LegacyCsg4Codec.Read(original);
        using var context=CandidateContext.Create();var settings=new OutsideRailCandidateSettings(1e-7,1e-7,1e-7);
        int compared=0;
        using(var graph=new CandidateRailGraphSession(context,document,settings)){
            graph.Generate(131,132,133,116,123,128);
            foreach(int index in new[]{131,132,133,116,123,128}){
                var expected=Observe(graph,index,document.Records[index].TypeGuid==LegacyCsg4Codec.UnarrayV1TypeGuid);
                Check(graph.OutputChannelCount(index)==expected.Length,"Dynamic output count mismatch");
                for(int channel=0;channel<expected.Length;channel++){Same(context,graph,graph.ReadOutput(index,channel),expected[channel]);compared++;}
                Check(!graph.ReadOutput(index,-1).HasNumber&&graph.ReadOutput(index,999).Array is null,"Invalid collection channel not empty");
            }
            Check(graph.ReadOutput(131,0).Array!.Count==3,"Original profile array length");
            foreach(int index in new[]{116,123,128}){Check(graph.OutputChannelCount(index)==4&&graph.ReadOutput(index,3).Array!.Count==0,"Source trailing empty output array was corrected away");Check(graph.ReadProfileMetadata(index).Name is not null,"Unarray profile metadata lost");}
            Throws<NotSupportedException>(()=>graph.RetainCurve(131));
            Throws<NotSupportedException>(()=>graph.TryReadTransform(131,0,out _));
            Check(LegacyCsg4Codec.Write(graph.Values.ExportCurrentDocument()).AsSpan().SequenceEqual(original),"Collection generation changed source document");
            foreach(int index in new[]{53,59,61,81,157,179,183,187,190,216,222,232,235,238})Throws<NotSupportedException>(()=>graph.Generate(index));
            int x=document.Records[114].SimpleBlock!.ObjectIndices[0];graph.Values.SetValue(x,graph.Values.ReadState(x).Numeric.RealValue+20);graph.Generate(131);
            var saved=LegacyCsg4Codec.Read(LegacyCsg4Codec.Write(graph.Values.ExportCurrentDocument()));
            using var reopened=new CandidateRailGraphSession(context,saved,settings);reopened.Generate(131);
            using var old=graph.CaptureOutput(131);using var fresh=reopened.CaptureOutput(131);
            var a=Leaves(old.Value).Where(x=>x.Curve.HasValue).ToArray();var b=Leaves(fresh.Value).Where(x=>x.Curve.HasValue).ToArray();Check(a.Length==3&&b.Length==3,"Reopened collection shapes lost");
            for(int i=0;i<a.Length;i++){using var aa=old.RetainCurve(a[i]);using var bb=fresh.RetainCurve(b[i]);var d=context.InspectCurve(aa);Point(context.PointAt(aa,(d.FirstParameter+d.LastParameter)/2),context.PointAt(bb,(d.FirstParameter+d.LastParameter)/2));Check(old.ReadProfileMetadata(a[i]).Name==fresh.ReadProfileMetadata(b[i]).Name,"Reopened collection profile name changed");}
        }
        Check(context.DiagnosticLiveShapeCount==0,"Original collection graph leaked native entries");
        var nestedDoc=Wire(document,132,131,4,16);
        CandidateDataSnapshot captured;LegacyChannelValue[] capturedLeaves;CandidatePoint3[] capturedPoints;double[] parameters;
        using(var graph=new CandidateRailGraphSession(context,nestedDoc,settings)){
            graph.Generate(132);captured=graph.CaptureOutput(132);
            capturedLeaves=Leaves(captured.Value).Where(x=>x.Curve.HasValue).ToArray();Check(capturedLeaves.Length==5&&captured.Value.Array![0].Array!.Count==3,"Nested array was flattened or reduced to one curve");
            parameters=new double[5];capturedPoints=new CandidatePoint3[5];
            for(int i=0;i<5;i++){using var curve=captured.RetainCurve(capturedLeaves[i]);var d=context.InspectCurve(curve);parameters[i]=d.FirstParameter+(d.LastParameter-d.FirstParameter)*.37;capturedPoints[i]=context.PointAt(curve,parameters[i]);}
            int x=document.Records[114].SimpleBlock!.ObjectIndices[0];graph.Values.SetValue(x,graph.Values.ReadState(x).Numeric.RealValue+50);graph.Generate(115);
            var still=Leaves(graph.ReadOutput(132,0)).Where(x=>x.Curve.HasValue).ToArray();
            for(int i=0;i<5;i++){using var curve=graph.RetainCurve(still[i].Curve!.Value);Point(context.PointAt(curve,parameters[i]),capturedPoints[i]);}
            graph.Generate(132);
            for(int i=0;i<5;i++){using var curve=captured.RetainCurve(capturedLeaves[i]);Point(context.PointAt(curve,parameters[i]),capturedPoints[i]);}
            var good=graph.ReadOutput(115,0);
            Throws<InvalidOperationException>(()=>graph.CaptureValue(LegacyChannelValue.FromArray(new[]{good,LegacyChannelValue.FromCurve(99999,0)})));
            var wide=LegacyChannelValue.FromArray(Enumerable.Repeat(LegacyChannelValue.Empty,LegacyArrayData.MaximumItems));
            Throws<NotSupportedException>(()=>graph.CaptureValue(LegacyChannelValue.FromArray(new[]{good,wide})));
            var previous=graph.ReadOutput(132,0);ulong entries=context.DiagnosticLiveShapeCount;
            graph.Values.SetValue(x,double.NaN);Throws<NotSupportedException>(()=>graph.Generate(132));Check(ReferenceEquals(previous,graph.ReadOutput(132,0))&&context.DiagnosticLiveShapeCount==entries,"Failed collection generation replaced cache or leaked geometry");
        }
        for(int i=0;i<5;i++){using var curve=captured.RetainCurve(capturedLeaves[i]);Point(context.PointAt(curve,parameters[i]),capturedPoints[i]);}
        var survivor=captured.RetainCurve(capturedLeaves[0]);captured.Dispose();Throws<ObjectDisposedException>(()=>captured.RetainCurve(capturedLeaves[0]));Check(context.DiagnosticLiveShapeCount==1,"Captured leaf disposal/independent reader lifetime failed");survivor.Dispose();Check(context.DiagnosticLiveShapeCount==0,"Snapshot/partial capture leaked native retains");
        // Unarray expands exactly one level and preserves nested arrays/null members.
        var oneLevel=Wire(Wire(document,132,131,8),128,132);
        using(var graph=new CandidateRailGraphSession(context,oneLevel,settings)){
            graph.Generate(128);Check(graph.OutputChannelCount(128)==3&&graph.ReadOutput(128,0).Array!.Count==3&&graph.ReadOutput(128,2).Array!.Count==0,"Unarray recursively flattened instead of one-level expansion");
            var expected=Observe(graph,128,true);for(int i=0;i<expected.Length;i++){Same(context,graph,graph.ReadOutput(128,i),expected[i]);compared++;}
        }
        var missing=document.WithInputLink(16,0,-1,-5).WithInputLink(16,1,-1,-5);
        var nullArray=Wire(Wire(missing,131,16,8),128,131);
        using(var graph=new CandidateRailGraphSession(context,nullArray,settings)){graph.Generate(128);Check(graph.ReadOutput(128,0).IsMissing,"Null member changed to empty during collection aliasing");}
        using(var graph=new CandidateRailGraphSession(context,Wire(missing,128,16),settings))Throws<NullReferenceException>(()=>graph.Generate(128));
        // Source wildcard body handles every producer output; saved load replay remains independently tested.
        var outputs=new[]{LegacyChannelValue.FromNumber(2),LegacyChannelValue.FromArray(new[]{LegacyChannelValue.Missing}),LegacyChannelValue.Empty};
        var wildcard=Oracle.Values.Run(false,new[]{new Oracle.ObservationInput(LegacyChannelValue.Empty,outputs,-1)});
        Check(wildcard[0].Array!.Count==3&&wildcard[0].Array![1].Array![0].IsMissing,"Source all-output wildcard behavior lost");
        var rawProfile=Wire(document,131,7);
        CandidateDataSnapshot imported;
        using(var graph=new CandidateRailGraphSession(context,rawProfile,settings)){graph.Generate(131);imported=graph.CaptureOutput(131);}
        var leaf=imported.Value.Array![0];using(var curve=imported.RetainCurve(leaf)){Check(imported.ReadProfileMetadata(leaf).SourceGroupRecordIndex==7,"Stored profile snapshot identity lost");context.InspectCurve(curve);}imported.Dispose();
        var selected=Wire(document,16,131); // reset selector control separately below
        selected=selected.WithInputLink(16,1,-1,-5).WithInputLink(16,2,-1,-5);
        using(var graph=new CandidateRailGraphSession(context,selected,settings)){graph.Generate(16);using var snap=graph.CaptureOutput(16);Check(snap.Value.Array!.Count==3,"Selector dropped array output");foreach(var item in snap.Value.Array){using var curve=snap.RetainCurve(item);Check(snap.ReadProfileMetadata(item).Name is not null,"Selector dropped array curve metadata");}}
        using(var graph=new CandidateRailGraphSession(context,document.WithInputLink(131,0,131,0),settings))Throws<InvalidOperationException>(()=>graph.Generate(131));
        Check(context.DiagnosticLiveShapeCount==0,"Collection tests retained native entries");
        return "PASS owned native collections:"+compared+" source output comparisons, original6 array/unarray chains, nested/per-item metadata, trailing-empty/null/wildcard semantics, retained snapshots across rebuild/disposal, save/reopen/failure/cycle/cleanup; known candidate subset, no full array-consumer/PC parity.";
    }
}
