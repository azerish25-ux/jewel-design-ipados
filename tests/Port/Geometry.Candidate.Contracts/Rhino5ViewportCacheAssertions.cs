using System.Globalization;
using System.Text.Json;
using JewelDesign.Geometry.Candidate;
using JewelDesign.Rendering.Candidate;
namespace CandidateGeometryContractChecks;
public static class Rhino5ViewportCacheAssertions
{
    private static int checks;
    private static void Check(bool ok,string message){checks++;if(!ok)throw new Exception(message);}
    private static Rhino5ViewportState Seed()=>new(new(new(0,0,80),new(0,0,-1),new(0,1,0),new(1,0,0),new(0,1,0),new(0,0,1),true,false,false,false),new(-1,1,-1,1,1,1000),true,Rhino5Viewport.Perspective,3,new(0,0,0));
    private static double Number(string s)=>s switch{"nan"=>double.NaN,"inf"=>double.PositiveInfinity,"-inf"=>double.NegativeInfinity,_=>double.Parse(s,CultureInfo.InvariantCulture)};
    public static string Run()
    {
        checks=0;var seed=Seed();var identity=Rhino5Matrix4.Identity;
        Check(identity.IsIdentity(),"Exact identity");var copy=identity.CopyCoefficients();copy[0]=100;Check(identity[0,0]==1,"Matrix ownership");
        copy=identity.CopyCoefficients();copy[3]=double.NaN;Check(!new Rhino5Matrix4(copy).IsIdentity(),"Unordered coefficient rejects identity");
        foreach(var raw in new[]{seed with {Frame=seed.Frame with {CameraValid=false}},seed with {FrustumValid=false}})
            Check(!Rhino5Matrix4.TryWorldToClip(raw,identity,out var retained)&&retained.IsIdentity(),"Invalid flags reset output identity and return false");
        Check(Rhino5Matrix4.TryWorldToClip(seed,identity,out var clip),"World-to-clip succeeds");
        foreach(var pair in new[]{(79d,1d),(-920d,-1d)})
        {double z=pair.Item1;double a=clip[2,2]*z+clip[2,3],w=clip[3,2]*z+clip[3,3];Check(Math.Abs(a/w-pair.Item2)<1e-12,"Analytical source near/far clip endpoint");}
        var degenerate=seed with {Bounds=new(1,1,-1,1,1,1000)};
        Check(Rhino5Matrix4.TryWorldToClip(degenerate,identity,out var fallback)&&fallback[0,0]==0&&fallback[1,1]==0&&fallback[2,2]==0&&fallback[3,3]==1,"Ignored projection failure retains constructor fallback but GetXform succeeds");
        using var stream=typeof(Rhino5ViewportCacheAssertions).Assembly.GetManifestResourceStream("ViewportCacheSourceCases")!;
        using var input=JsonDocument.Parse(stream);int cases=0;
        foreach(var row in input.RootElement.GetProperty("cases").EnumerateArray())
        {
            var a=row.GetProperty("args").EnumerateArray().Select(x=>Number(x.GetString()!)).ToArray();int flags=(int)a[0];
            var state=seed with {Frame=seed.Frame with {CameraValid=(flags&1)!=0,Location=new(a[2],a[3],a[4]),X=new(a[5],a[6],a[7]),Y=new(a[8],a[9],a[10]),Z=new(a[11],a[12],a[13])},FrustumValid=(flags&2)!=0,Projection=(uint)a[1],Bounds=new(a[14],a[15],a[16],a[17],a[18],a[19])};
            var mod=new Rhino5Matrix4(a.AsSpan(20,16));var extra=new Rhino5Matrix4(a.AsSpan(36,16));
            bool ok=Rhino5Matrix4.TryWorldToClip(state,mod,out var m);Check(ok==row.GetProperty("ok").GetBoolean(),"Compiled GetXform return "+cases);
            Check(extra.IsIdentity()==row.GetProperty("identity").GetBoolean(),"Compiled zero-tolerance identity "+cases);
            if(ok&&!extra.IsIdentity())m=m.Multiply(extra);var expected=row.GetProperty("values").EnumerateArray().Select(x=>Number(x.GetString()!)).ToArray();var actual=m.CopyCoefficients();
            for(int i=0;i<16;i++){double x=actual[i],y=expected[i];Check((double.IsNaN(x)&&double.IsNaN(y))||x==y||(double.IsFinite(x)&&double.IsFinite(y)&&Math.Abs(x-y)<=2e-13*Math.Max(1,Math.Max(Math.Abs(x),Math.Abs(y)))),"Compiled cache matrix "+cases+":"+i);}
            cases++;
        }
        Check(cases==1084,"Independent cache fixture count");
        return "PASS original-derived Rhino5 viewport cache:"+checks+" checks including "+cases+" compiled published GetXform/cache cases and9 analytical/ownership checks; native commit/history/PC parity separate.";
    }
}
