using JewelDesign.Documents;
using JewelDesign.Features.Candidate;
using JewelDesign.Geometry.Candidate;
using Oracle=CandidateGeometryContractChecks.Recovered.Transforms;
namespace CandidateGeometryContractChecks;

internal static class TransformProducerAssertions
{
    private static void Check(bool value,string message){if(!value)throw new Exception(message);}
    private static void Throws<T>(Action action)where T:Exception{try{action();}catch(T){return;}throw new Exception("Expected "+typeof(T).Name);}
    private static double[] Coefficients(LegacyAffineTransform m)=>new[]{m.M11,m.M12,m.M13,m.M14,m.M21,m.M22,m.M23,m.M24,m.M31,m.M32,m.M33,m.M34};
    private static void Same(LegacyAffineTransform a,LegacyAffineTransform b)
    {
        Check(Coefficients(a).Select(BitConverter.DoubleToInt64Bits).SequenceEqual(Coefficients(b).Select(BitConverter.DoubleToInt64Bits)),"Source transform coefficient bits differ");
    }
    private static void Near(double a,double b){if(!double.IsFinite(a)||Math.Abs(a-b)>1e-5)throw new Exception("Transform mismatch: "+a+" vs "+b);}
    private static CandidateAffine Native(LegacyAffineTransform m)=>new(m.M11,m.M12,m.M13,m.M14,m.M21,m.M22,m.M23,m.M24,m.M31,m.M32,m.M33,m.M34);
    private static void Apply(CandidateContext context,CandidateShape curve,LegacyAffineTransform m)
    {
        var info=context.InspectCurve(curve);var first=context.PointAt(curve,info.FirstParameter);
        using var changed=context.TransformAffine(curve,Native(m));
        for(int i=0;i<=16;i++){
            double t=info.FirstParameter+(info.LastParameter-info.FirstParameter)*i/16;
            var p=context.PointAt(curve,t);var q=context.PointAt(changed,t);
            Near(q.X,m.M11*p.X+m.M12*p.Y+m.M13*p.Z+m.M14);
            Near(q.Y,m.M21*p.X+m.M22*p.Y+m.M23*p.Z+m.M24);
            Near(q.Z,m.M31*p.X+m.M32*p.Y+m.M33*p.Z+m.M34);
        }
        var retained=context.PointAt(curve,info.FirstParameter);Check(first==retained,"Affine application mutated input");
    }
    private static int SourceCases()
    {
        var inputs=new[]{LegacyChannelValue.Missing,LegacyChannelValue.Empty,LegacyChannelValue.FromCurve(4,0),LegacyChannelValue.FromTransform(LegacyAffineTransform.Scale(1,1,1))}
            .Concat(new[]{0.0,BitConverter.Int64BitsToDouble(long.MinValue),-3.25,2.5,double.NaN,double.PositiveInfinity,double.NegativeInfinity}.Select(LegacyChannelValue.FromNumber)).ToArray();
        var controls=new[]{LegacyChannelValue.Missing,LegacyChannelValue.Empty}.Concat(new[]{.5,Math.BitIncrement(.5),double.NaN,double.PositiveInfinity,double.NegativeInfinity}.Select(LegacyChannelValue.FromNumber)).ToArray();
        int cases=0;
        foreach(var x in inputs)foreach(var y in inputs)foreach(var z in inputs)foreach(var mirror in controls){
            var source=new Oracle.TranslateSource(x,y,z,mirror);Check(source.Update(),"Translation source failed");
            Same(TransformProducerCandidate.Translation(x,y,z,mirror),source.oXform.Data.Value.Transform!.Value);cases++;
        }
        foreach(double size in new[]{0.0,Math.BitDecrement(.01),.01,Math.BitIncrement(.01),2.0}){
            var bounds=new CandidateCurveBounds(new(-4,2,-3),new(-4+size,2+2*size,-3+3*size),1e-10,1e-7);
            foreach(var x in inputs)foreach(var y in inputs)foreach(var z in inputs){
                var source=new Oracle.ScaleSource(bounds,x,y,z);Check(source.Update(),"Scale source failed");
                Same(TransformProducerCandidate.ScaleToSize(bounds,x,y,z),source.oXform.Data.Value.Transform!.Value);cases++;
            }
        }
        foreach(var x in inputs)foreach(var y in inputs){
            var source=new Oracle.AddSource(x,y);Check(source.Update(),"Scalar Add source failed");
            double sum=TransformProducerCandidate.AddScalars(x,y),a=0,b=0,c=0;
            source.oValue.Data.GetDouble(ref a);source.oNegated.Data.GetDouble(ref b);source.oOneMinus.Data.GetDouble(ref c);
            Check(BitConverter.DoubleToInt64Bits(a)==BitConverter.DoubleToInt64Bits(sum)&&BitConverter.DoubleToInt64Bits(b)==BitConverter.DoubleToInt64Bits(0.0-sum)&&BitConverter.DoubleToInt64Bits(c)==BitConverter.DoubleToInt64Bits(1.0-sum),"Scalar Add source outputs differ");cases++;
        }
        // Independent boundary/origin assertions, separate from the extracted observation harness.
        var edge=new CandidateCurveBounds(new(0,0,6),new(.009,.01,8),1e-10,1e-7);
        var target=LegacyChannelValue.FromNumber(2);
        var scaled=TransformProducerCandidate.ScaleToSize(edge,target,target,target);
        Near(scaled.M11,2);Near(scaled.M22,200);Near(scaled.M33,1);
        Check(scaled.M14==0&&scaled.M24==0&&scaled.M34==0,"Scale centered bounding box instead of WorldXY origin");
        return cases;
    }
    public static string Run()
    {
        int sourceCases=SourceCases();
        using var stream=typeof(TransformProducerAssertions).Assembly.GetManifestResourceStream("RingRailLibraryFixture")!;
        using var memory=new MemoryStream();stream.CopyTo(memory);var original=memory.ToArray();var document=LegacyCsg4Codec.Read(original);
        Check(document.Records[50].SimpleBlock is not null,"Translation producer header missing");
        using var context=CandidateContext.Create();var settings=new OutsideRailCandidateSettings(1e-7,1e-7,1e-7);
        using(var graph=new CandidateRailGraphSession(context,document,settings)){
            using var circle=context.CreateCircle(4,CandidatePlane.WorldZX);
            foreach(int index in new[]{50,56,113,120,213,219,49,55,212,218}){
                graph.Generate(index);var data=graph.ReadOutput(index,0);var matrix=data.Transform??throw new Exception("Missing saved transform data");
                double sentinel=123;Check(!data.GetDouble(ref sentinel)&&sentinel==123,"Transform coerced to number");
                bool boolean=true;Check(!data.GetBoolean(ref boolean)&&boolean,"Transform coerced to Boolean");
                Check(graph.ReadOutput(index,-1).Transform is null&&!graph.ReadOutput(index,1).HasNumber,"Invalid transform output index");
                Apply(context,circle,matrix);
            }
            Check(LegacyCsg4Codec.Write(graph.Values.ExportCurrentDocument()).AsSpan().SequenceEqual(original),"Transform generations changed saved data");
            // Placement now resolves the original remaining scale dependencies.
            foreach(int index in new[]{112,119,126}){graph.Generate(index);Check(graph.ReadOutput(index,0).Transform is not null,"Original placement-dependent scale missing");}
        }
        var resolved=document;
        foreach(int index in new[]{111,118,125})resolved=resolved.WithInputLink(index,0,8,0).WithInputLink(index,1,9,0);
        using(var graph=new CandidateRailGraphSession(context,resolved,settings)){
            foreach(int index in new[]{111,118,125,112,119,126})graph.Generate(index);
            double n=0;Check(graph.ReadOutput(111,0).GetDouble(ref n),"Resolved scalar Add missing");Near(n,4.38+1.84);
            Check(graph.ReadOutput(111,1).GetDouble(ref n),"Add negated missing");Near(n,0.0-(4.38+1.84));
            Check(graph.ReadOutput(111,2).GetDouble(ref n),"Add one-minus missing");Near(n,1.0-(4.38+1.84));
            Check(!graph.ReadOutput(111,3).HasNumber,"Add invalid index must return empty");
        }
        // Real profile/curve bounds and an asymmetric curve prove scale origin and application.
        var curveScale=document.WithInputLink(49,0,13,0).WithInputLink(49,3,10,0);
        using(var graph=new CandidateRailGraphSession(context,curveScale,settings)){
            graph.Generate(49);using var curve=graph.RetainCurve(13);var m=graph.ReadOutput(49,0).Transform!.Value;
            Check(m.M14==0&&m.M24==0&&m.M34==0,"Native bounds scale translated origin");Apply(context,curve,m);
        }
        var edited=document.WithScalarInputLink(40,1,41,1);
        using(var graph=new CandidateRailGraphSession(context,edited,settings)){
            graph.Values.SetValue(41,2.5);graph.Generate(50);Near(graph.ReadOutput(50,0).Transform!.Value.M14,-2.5);
            var saved=LegacyCsg4Codec.Read(LegacyCsg4Codec.Write(graph.Values.ExportCurrentDocument()));
            using var reopened=new CandidateRailGraphSession(context,saved,settings);reopened.Generate(50);
            Same(graph.ReadOutput(50,0).Transform!.Value,reopened.ReadOutput(50,0).Transform!.Value);
        }
        var forwarding=document.WithInputLink(16,0,50,0).WithInputLink(16,1,-1,-5);
        using(var graph=new CandidateRailGraphSession(context,forwarding,settings)){
            graph.Generate(16);var previous=graph.ReadOutput(16,0).Transform!.Value;
            graph.Values.SetValue(40,17);graph.Generate(50);Same(graph.ReadOutput(16,0).Transform!.Value,previous);
            graph.Generate(16);Near(graph.ReadOutput(16,0).Transform!.Value.M14,17);
            Throws<NotSupportedException>(()=>graph.RetainCurve(16));
        }
        using(var missing=new CandidateRailGraphSession(context,document.WithInputLink(49,0,11,0),settings))Throws<NotSupportedException>(()=>missing.Generate(49));
        using(var cycle=new CandidateRailGraphSession(context,document.WithInputLink(50,0,50,0),settings))Throws<InvalidOperationException>(()=>cycle.Generate(50));
        Check(context.DiagnosticLiveShapeCount==0,"Transform generation leaked native shapes");
        return "PASS saved transform producers:"+sourceCases+" exact-source translation/scale/scalar-add cases, saved6 translations/all7 original scales via placement, all7 scale plans also on explicit resolved links, native application/source preservation, override/edit/save/reopen/forwarding/cycle/cleanup; no full transform-chain/PC parity.";
    }
}
