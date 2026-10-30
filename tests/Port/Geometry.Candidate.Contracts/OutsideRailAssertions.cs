using System.Text.Json;
using System.Security.Cryptography;
using JewelDesign.Documents;
using JewelDesign.Features.Candidate;
using JewelDesign.Geometry.Candidate;
namespace CandidateGeometryContractChecks;

internal static class OutsideRailAssertions
{
    private static void Check(bool value,string message){if(!value)throw new Exception(message);}
    private static void Near(double actual,double expected,double tolerance=1e-5)=>Check(double.IsFinite(actual)&&Math.Abs(actual-expected)<tolerance,"Outside rail mismatch: "+actual.ToString("R")+" expected "+expected.ToString("R"));
    private static void Point(CandidatePoint3 actual,double x,double y,double z){Near(actual.X,x);Near(actual.Y,y);Near(actual.Z,z);}
    private static readonly OutsideRailCandidateSettings Settings=new(1e-7,1e-7,1e-7);
    private static void Inspect(CandidateContext context,OutsideRailCandidate rail,double radius,OutsideRailV1Parameters p)
    {
        Near(rail.ResolvedRadius,radius);Check(rail.OutsideScale==0,"Source OutsideScale literal changed");
        Check(context.IsCurveClosed(rail.Curve),"Actual output is not closed");
        Check(rail.Diagnostics.Count==0,"Analytic normal case used kernel fallback");
        double center=(p.TopThickness-p.BottomThickness)/2,vertical=radius+(p.TopThickness+p.BottomThickness)/2;
        double horizontal=p.LockOn?radius+p.SideThickness:(radius+p.SideThickness)/Math.Sqrt(1-center*center/(vertical*vertical));
        for(int i=0;i<40;i++){
            double theta=2*Math.PI*(i+.23)/40,s=Math.Sin(theta),c=Math.Cos(theta);
            var expected=new CandidatePoint3(horizontal*s,0,p.LockOn?(c>=0?radius+p.TopThickness:radius+p.BottomThickness)*c:center+vertical*c);
            var projection=context.ClosestPoint(rail.Curve,expected);Near(projection.Distance,0,2e-5);
        }
        if(p.LockOn){
            Check(rail.JoinDisposition==OutsideRailJoinDisposition.Merged,"Locked halves were not merged");
            var info=context.InspectCurve(rail.Curve);Point(context.PointAt(rail.Curve,info.FirstParameter),0,0,radius+p.TopThickness);
            Check(context.TangentAt(rail.Curve,info.FirstParameter).X>0,"Final tangent must face +X");
        }
    }

    private static void LibraryCase(CandidateContext context,CandidateShape explicitProfile)
    {
        using var fixture=typeof(OutsideRailAssertions).Assembly.GetManifestResourceStream("RingRailLibraryFixture")!;
        using var memory=new MemoryStream();fixture.CopyTo(memory);var bytes=memory.ToArray();
        using var oracleStream=typeof(OutsideRailAssertions).Assembly.GetManifestResourceStream("OutsideRailInputOracle")!;
        using var oracle=JsonDocument.Parse(oracleStream);
        Check(Convert.ToHexString(SHA256.HashData(bytes)).ToLowerInvariant()==oracle.RootElement.GetProperty("fixtureSha256").GetString(),"Outside fixture hash changed");
        var header=oracle.RootElement.GetProperty("rails")[0];
        Check(header.GetProperty("index").GetInt32()==13&&header.GetProperty("fullyConsumed").GetBoolean(),"Outside header boundary");
        int[] expectedObjects={2,6,8,9,10,11},expectedChannels={0,3,0,0,0,0};
        for(int i=0;i<6;i++)Check(header.GetProperty("objects")[i].GetInt32()==expectedObjects[i]&&header.GetProperty("channels")[i].GetInt32()==expectedChannels[i],"Outside source links");
        var document=LegacyCsg4Codec.Read(bytes);
        Check(document.Records[13].TypeGuid==OutsideRailV1Candidate.TypeGuid&&document.Records[13].Version==new LegacyCsg4Version(1,0),"Outside record profile");
        static OutsideRailV1Parameters Read(LegacyCsg4Document doc)=>new(doc.Records[8].Scalar!.Value,doc.Records[9].Scalar!.Value,doc.Records[10].Scalar!.Value,doc.Records[11].Scalar!.Value>.5);
        var parameters=Read(document);double radius=document.Records[2].Scalar!.Value;
        Near(parameters.TopThickness,4.38);Near(parameters.SideThickness,1.84);Near(parameters.BottomThickness,1.7);Check(parameters.LockOn,"Stored lock candidate");
        using(var original=OutsideRailV1Candidate.Build(context,explicitProfile,radius,Settings,parameters)){
            Inspect(context,original,radius,parameters);
            var editor=new LegacyCsg4EditSession(document);editor.BeginEditTransaction();editor.SetValue(editor.GetParameter(8),parameters.TopThickness+.1);editor.EndEditTransaction();
            var reopened=LegacyCsg4Codec.Read(editor.ExportArchiveData());var changedParameters=Read(reopened);Near(changedParameters.TopThickness,4.48);
            using(var changed=OutsideRailV1Candidate.Build(context,explicitProfile,radius,Settings,changedParameters))Inspect(context,changed,radius,changedParameters);
            Inspect(context,original,radius,parameters);
            Check(editor.Undo(),"Outside parameter undo missing");Check(editor.ExportArchiveData().AsSpan().SequenceEqual(bytes),"Outside parameter undo archive changed");
            using(var restored=OutsideRailV1Candidate.Build(context,explicitProfile,radius,Settings,Read(editor.Document)))Inspect(context,restored,radius,parameters);
        }
    }
    private static void EllipseCase(CandidateContext context)
    {
        using var source=context.CreateCircle(1,CandidatePlane.WorldZX);
        using var ellipse=context.TransformAffine(source,new(3,0,0,0,0,1,0,0,0,0,7,0));
        using var oracleStream=typeof(OutsideRailAssertions).Assembly.GetManifestResourceStream("OutsideRailInputOracle")!;
        using var oracle=JsonDocument.Parse(oracleStream);double sampled=oracle.RootElement.GetProperty("analyticEllipse").GetProperty("sampledMinimum").GetDouble();
        Check(sampled>3.01,"Independent sampled minimum must differ from exact closest3");
        foreach(bool locked in new[]{false,true}){
            var parameters=new OutsideRailV1Parameters(3,2,1,locked);
            using var result=OutsideRailV1Candidate.Build(context,ellipse,7,new(1e-9,1e-7,1e-7),parameters);
            Near(result.MinimumSampledDistance,sampled,1e-6);Inspect(context,result,7,parameters);
        }
    }
    public static string Run()
    {
        using var context=CandidateContext.Create();
        Check(OutsideRailV1Candidate.TypeGuid==new Guid("f9f6400f-67c2-4883-8c44-a91f411f2ee3"),"Legacy outside registration changed");
        using(var circle=context.CreateCircle(7,CandidatePlane.WorldZX)){
            var defaultParameters=new OutsideRailV1Parameters();
            using(var defaults=OutsideRailV1Candidate.Build(context,circle,7,Settings))Inspect(context,defaults,7,defaultParameters);
            foreach(bool mode in new[]{false,true}){
                var p=new OutsideRailV1Parameters(LockOn:mode);
                using var typical=OutsideRailV1Candidate.Build(context,circle,7,new(.001,.001,.001),p);Inspect(context,typical,7,p);
            }
            foreach(bool locked in new[]{false,true}){
                var p=new OutsideRailV1Parameters(4,2.5,1.5,locked);
                using(var result=OutsideRailV1Candidate.Build(context,circle,8,Settings,p)){Inspect(context,result,8,p);Near(result.MinimumSampledDistance,7);}
                using var reversed=context.ReverseCurve(circle);
                using(var result=OutsideRailV1Candidate.Build(context,reversed,8,Settings,p))Inspect(context,result,8,p);
                using var moved=context.RelocateClosedCurveSeam(circle,2);
                using(var result=OutsideRailV1Candidate.Build(context,moved,8,Settings,p))Inspect(context,result,8,p);
            }
            using(var radiusBase=context.CreateCircle(5,CandidatePlane.WorldZX))
            using(var radiusCurve=context.Translate(radiusBase,0,0,2)){
                using(var fallback=OutsideRailV1Candidate.Build(context,circle,null,Settings,radiusCurve:radiusCurve))Inspect(context,fallback,7,defaultParameters);
                using(var priority=OutsideRailV1Candidate.Build(context,circle,9,Settings,radiusCurve:radiusCurve))Inspect(context,priority,9,defaultParameters);
            }
            using(var negative=OutsideRailV1Candidate.Build(context,circle,7,Settings,new(-1,-.25,.5,false)))Inspect(context,negative,7,new(-1,-.25,.5,false));

            // Finite final ray misses this tall profile: source uses closest-to-Z100 seam.
            using(var tall=OutsideRailV1Candidate.Build(context,circle,7,Settings,new(143,2,1,true))){
                Check(context.IntersectCurveLine(tall.Curve,new(0,0,0),new(0,0,100),Settings.ApplicationTolerance).Events.Count==0,"Tall final ray unexpectedly intersects");
                var info=context.InspectCurve(tall.Curve);var start=context.PointAt(tall.Curve,info.FirstParameter);
                double u=150.0*100/(150.0*150-9.0*9);
                Near(Math.Abs(start.X),9*Math.Sqrt(1-u*u));Near(start.Z,150*u);
                Check(context.TangentAt(tall.Curve,info.FirstParameter).X>0,"Closest fallback orientation");
                Check(tall.Diagnostics.Count==0,"Source closest fallback is not a native failure");
            }
            var differentTolerances=new OutsideRailCandidateSettings(1e-7,100,1e-7);
            using(var unlocked=OutsideRailV1Candidate.Build(context,circle,7,differentTolerances))Inspect(context,unlocked,7,new());
            try{using var locked=OutsideRailV1Candidate.Build(context,circle,7,differentTolerances,new(LockOn:true));throw new Exception("Direct application tolerance was ignored");}
            catch(OutsideRailCandidateException error){Check(error.Message=="Intersect of custom Rail Failed","Application intersection tolerance policy");}
            try{using var missingExtent=OutsideRailV1Candidate.Build(context,circle,7,Settings,new(143,2,1,false));throw new Exception("Finite helper ray was extended");}
            catch(ArgumentOutOfRangeException){}
            Check(context.DiagnosticLiveShapeCount==1,"Branch failure cleanup retained curves");
            Point(context.PointAt(circle,0),0,0,7);Check(context.IsCurveClosed(circle),"Source closure changed");
            foreach(var bad in new[]{0.0}){
                try{using var result=OutsideRailV1Candidate.Build(context,circle,bad,Settings);throw new Exception("Zero radius accepted");}
                catch(OutsideRailCandidateException error){Check(error.Message=="Intersect of custom Rail Failed","Recovered zero-radius message changed");}
            }
            try{using var missing=OutsideRailV1Candidate.Build(context,circle,null,Settings);throw new Exception("Missing radius accepted");}
            catch(OutsideRailCandidateException error){Check(error.Message=="Missing Ring Rail","Missing radius message");}
            Check(context.DiagnosticLiveShapeCount==1,"Failure cleanup retained temporary curves");
        }
        try{using var missing=OutsideRailV1Candidate.Build(context,null,7,Settings);throw new Exception("Missing curve accepted");}
        catch(OutsideRailCandidateException error){Check(error.Message=="Outside Ring Rail: Missing Custom Rail (No Curve)","Missing curve message");}
        using(var line=context.CreateLine(new(0,0,0),new(1,0,0)))Check(!context.IsCurveClosed(line),"Open line closure query");
        using(var profile=context.CreateCircle(7,CandidatePlane.WorldZX))LibraryCase(context,profile);
        EllipseCase(context);
        using(var zeroMinimum=context.CreateLine(new(0,0,0),new(1,0,0))){
            try{using var result=OutsideRailV1Candidate.Build(context,zeroMinimum,7,Settings);throw new Exception("Zero sampled minimum accepted");}
            catch(OutsideRailCandidateException error){Check(error.Message=="Intersect of custom Rail Failed","Sample-minimum source message");}
            Check(context.DiagnosticLiveShapeCount==1,"Zero-minimum cleanup");
        }
        Check(context.DiagnosticLiveShapeCount==0,"Outside builder retained native shape entries");
        return "PASS outside rail candidate: actual source-ordered unlocked/locked deformation, split/join/seam/direction, defaults/radius fallback/priority/scale0, independent sampled ellipse, stored-input edit/save/reopen/rebuild/undo and ownership/errors; no actual chooser/graph/PC claim.";
    }
}
