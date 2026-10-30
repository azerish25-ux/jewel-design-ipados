using JewelDesign.Documents;
using JewelDesign.Features.Candidate;
using JewelDesign.Geometry.Candidate;
using Oracle=CandidateGeometryContractChecks.Recovered.Rotation;
namespace CandidateGeometryContractChecks;

internal static class RotationProducerAssertions
{
    private static void Check(bool condition,string message){if(!condition)throw new Exception(message);}
    private static void Near(double a,double b,double tolerance=1e-12){if(!double.IsFinite(a)||!double.IsFinite(b)||Math.Abs(a-b)>tolerance)throw new Exception("Rotation mismatch "+a+" vs "+b);}
    private static void Throws<T>(Action action)where T:Exception{try{action();}catch(T){return;}throw new Exception("Expected "+typeof(T).Name);}
    private static double[] Fields(LegacyAffineTransform m)=>new[]{m.M11,m.M12,m.M13,m.M14,m.M21,m.M22,m.M23,m.M24,m.M31,m.M32,m.M33,m.M34};
    private static void Same(LegacyAffineTransform a,LegacyAffineTransform b){var x=Fields(a);var y=Fields(b);for(int i=0;i<x.Length;i++)Near(x[i],y[i]);}
    private static LegacyChannelValue Number(double x)=>LegacyChannelValue.FromNumber(x);
    private static double Numeric(LegacyChannelValue x){double n=0;x.GetDouble(ref n);return n;}
    private static LegacyAffineTransform Build(double x,double y,double z,double mirror=0)=>RotationProducerCandidate.Build(Number(x),Number(y),Number(z),Number(mirror));
    private static CandidateAffine Native(LegacyAffineTransform m)=>new(m.M11,m.M12,m.M13,m.M14,m.M21,m.M22,m.M23,m.M24,m.M31,m.M32,m.M33,m.M34);
    private static CandidatePoint3 Apply(LegacyAffineTransform m,CandidatePoint3 p)=>new(m.M11*p.X+m.M12*p.Y+m.M13*p.Z+m.M14,m.M21*p.X+m.M22*p.Y+m.M23*p.Z+m.M24,m.M31*p.X+m.M32*p.Y+m.M33*p.Z+m.M34);
    private static void Point(CandidatePoint3 p,double x,double y,double z){Near(p.X,x);Near(p.Y,y);Near(p.Z,z);}
    private static int SourceCases()
    {
        var values=new[]{LegacyChannelValue.Missing,LegacyChannelValue.Empty,LegacyChannelValue.FromCurve(4,0),LegacyChannelValue.FromTransform(LegacyAffineTransform.Identity)}
            .Concat(new[]{-720.0,-180,-90,-45,BitConverter.Int64BitsToDouble(long.MinValue),0,1e-9,17.25,90,180,270,360,720}.Select(Number)).ToArray();
        var controls=new[]{LegacyChannelValue.Missing,LegacyChannelValue.Empty}.Concat(new[]{0.0,.5,Math.BitIncrement(.5),1,double.NaN,double.PositiveInfinity,double.NegativeInfinity}.Select(Number)).ToArray();
        int count=0;
        foreach(var x in values)foreach(var y in values)foreach(var z in values)foreach(var mirror in controls){
            var source=new Oracle.CSRotateObjectDegXform(x,y,z,mirror);Check(source.Update(),"Recovered rotation wrapper failed");
            var actual=RotationProducerCandidate.Build(x,y,z,mirror);
            Same(actual,source.oXform.Data.Transform.Matrix);
            Same(actual,NativeRotationReference.Xyz(Numeric(x),Numeric(y),Numeric(z),Numeric(mirror)));count++;
        }
        return count;
    }
    public static string Run()
    {
        int sourceCases=SourceCases();Check(sourceCases==44217,"Missing recovered/native rotation cases");
        var point=new CandidatePoint3(1,2,3);
        Point(Apply(Build(90,0,0),point),1,-3,2);
        Point(Apply(Build(0,90,0),point),3,2,-1);
        Point(Apply(Build(0,0,90),point),-2,1,3);
        Point(Apply(Build(90,90,90),point),3,-2,1);
        Point(Apply(Build(90,0,0,1),point),1,3,-2);
        Point(Apply(Build(0,90,0,1),point),3,2,-1);
        Point(Apply(Build(0,0,90,1),point),2,-1,3);
        Check(Build(1e-8,0,0)==LegacyAffineTransform.Identity,"Published small-angle noise cleanup missing");
        Check(Build(90+1e-7,0,0)==Build(90,0,0),"Published near-cardinal cleanup missing");
        double threshold=1.490116119385e-8/(Math.PI/180.0);
        foreach(double center in new[]{0.0,90,180,270,360})
        foreach(double sign in new[]{-1.0,1.0})
        foreach(double factor in new[]{.5,1.0,2.0}){
            double boundary=center+sign*factor*threshold;
            foreach(double angle in new[]{Math.BitDecrement(boundary),boundary,Math.BitIncrement(boundary)}){
                Same(Build(angle,0,0),NativeRotationReference.Xyz(angle,0,0,0));
                Same(Build(0,angle,0),NativeRotationReference.Xyz(0,angle,0,0));
                Same(Build(0,0,angle),NativeRotationReference.Xyz(0,0,angle,0));
            }
        }
        foreach(double angle in new[]{1e6,-1e6,1e12,-1e12,1e100,-1e100,1e308,-1e308})Same(Build(angle,angle,angle,1),NativeRotationReference.Xyz(angle,angle,angle,1));
        foreach(double invalid in new[]{double.NaN,double.PositiveInfinity,double.NegativeInfinity}){
            Throws<NotSupportedException>(()=>Build(invalid,0,0));
            Throws<NotSupportedException>(()=>NativeRotationReference.Xyz(invalid,0,0,0));
        }
        var translation=LegacyAffineTransform.Translation(10,20,30);var scale=LegacyAffineTransform.Scale(2,3,4);
        Point(Apply(translation*scale,point),12,26,42);Point(Apply(scale*translation,point),22,66,132);
        Same(LegacyAffineTransform.Identity*translation,translation);Same(translation*LegacyAffineTransform.Identity,translation);
        Throws<NotSupportedException>(()=>LegacyAffineTransform.Multiply(LegacyAffineTransform.Translation(double.NaN,0,0),scale));
        using var stream=typeof(RotationProducerAssertions).Assembly.GetManifestResourceStream("RingRailLibraryFixture")!;
        using var memory=new MemoryStream();stream.CopyTo(memory);var original=memory.ToArray();var document=LegacyCsg4Codec.Read(original);
        using var context=CandidateContext.Create();var settings=new OutsideRailCandidateSettings(1e-7,1e-7,1e-7);
        using(var graph=new CandidateRailGraphSession(context,document,settings))
        using(var curve=context.CreateCircle(7,CandidatePlane.WorldZX)){
            var before=context.InspectCurve(curve);var start=context.PointAt(curve,before.FirstParameter);
            foreach(int index in new[]{51,57,114,121,214,220}){
                graph.Generate(index);var m=graph.ReadOutput(index,0).Transform??throw new Exception("Saved rotation transform missing");
                var links=graph.Inputs(index);
                double Read(int slot)=>links[slot].ObjectIndex<0?0:Numeric(graph.Values.ReadOutput(links[slot].ObjectIndex,links[slot].ChannelIndex));
                Same(m,NativeRotationReference.Xyz(Read(0),Read(1),Read(2),Read(3)));
                using var rotated=context.TransformAffine(curve,Native(m));Near(context.InspectCurve(rotated).Length,before.Length,1e-8);
                for(int i=0;i<=16;i++){double t=before.FirstParameter+(before.LastParameter-before.FirstParameter)*i/16;var p=context.PointAt(curve,t);var q=context.PointAt(rotated,t);var expected=Apply(m,p);Near(q.X,expected.X,1e-8);Near(q.Y,expected.Y,1e-8);Near(q.Z,expected.Z,1e-8);}
                double sentinel=123;Check(!graph.ReadOutput(index,0).GetDouble(ref sentinel)&&sentinel==123&&!graph.ReadOutput(index,1).HasNumber,"Rotation data conversion or invalid index");
            }
            Check(context.PointAt(curve,before.FirstParameter)==start,"Rotations mutated native input");
            using(var scaled=context.TransformAffine(curve,Native(scale)))
            using(var moved=context.TransformAffine(scaled,Native(translation)))
            using(var combined=context.TransformAffine(curve,Native(translation*scale))){
                for(int i=0;i<=16;i++){double t=before.FirstParameter+(before.LastParameter-before.FirstParameter)*i/16;var a=context.PointAt(moved,t);var b=context.PointAt(combined,t);Near(a.X,b.X,1e-8);Near(a.Y,b.Y,1e-8);Near(a.Z,b.Z,1e-8);}
            }
            Check(LegacyCsg4Codec.Write(graph.Values.ExportCurrentDocument()).AsSpan().SequenceEqual(original),"Rotation generation changed saved archive");
            graph.Values.SetValue(43,17);graph.Values.SetValue(44,23);graph.Values.SetValue(45,31);graph.Values.SetValue(46,1);graph.Generate(51,57);
            Same(graph.ReadOutput(51,0).Transform!.Value,NativeRotationReference.Xyz(17,23,31,0));Same(graph.ReadOutput(57,0).Transform!.Value,NativeRotationReference.Xyz(17,23,31,1));
            var saved=LegacyCsg4Codec.Read(LegacyCsg4Codec.Write(graph.Values.ExportCurrentDocument()));
            using(var reopened=new CandidateRailGraphSession(context,saved,settings)){reopened.Generate(51,57);Same(graph.ReadOutput(51,0).Transform!.Value,reopened.ReadOutput(51,0).Transform!.Value);Same(graph.ReadOutput(57,0).Transform!.Value,reopened.ReadOutput(57,0).Transform!.Value);}
            var previous=graph.ReadOutput(51,0).Transform!.Value;graph.Values.SetValue(43,double.NaN);Throws<NotSupportedException>(()=>graph.Generate(51));Same(graph.ReadOutput(51,0).Transform!.Value,previous);
        }
        using(var graph=new CandidateRailGraphSession(context,document.WithInputLink(16,0,51,0).WithInputLink(16,1,-1,-5),settings)){
            graph.Generate(16);var old=graph.ReadOutput(16,0).Transform!.Value;graph.Values.SetValue(43,33);graph.Generate(51);Same(graph.ReadOutput(16,0).Transform!.Value,old);graph.Generate(16);Same(graph.ReadOutput(16,0).Transform!.Value,graph.ReadOutput(51,0).Transform!.Value);
        }
        using(var graph=new CandidateRailGraphSession(context,document.WithInputLink(51,0,51,0),settings))Throws<InvalidOperationException>(()=>graph.Generate(51));
        Check(context.DiagnosticLiveShapeCount==0,"Rotation tests leaked native entries");
        return "PASS saved rotation producers:"+sourceCases+" recovered-wrapper/published-native cases, actual6 saved nodes, cardinal/noise/threshold/large-angle/order/mirror/default/finite guards, affine products/native application/source preservation, edit/save/reopen/forwarding/failure/cycle/cleanup; published6 comparison only, no original Rhino5/PC parity.";
    }
}
