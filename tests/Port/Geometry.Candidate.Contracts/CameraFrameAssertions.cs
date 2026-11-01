using System.Globalization;
using System.Text.Json;
using JewelDesign.Geometry.Candidate;
using JewelDesign.Rendering.Candidate;
namespace CandidateGeometryContractChecks;

public static class CameraFrameAssertions
{
    private static double Number(string s)=>s switch {"nan"=>double.NaN,"inf"=>double.PositiveInfinity,"-inf"=>double.NegativeInfinity,_=>double.Parse(s,CultureInfo.InvariantCulture)};
    private static bool Same(double a,double b,bool exact)
    {
        if(double.IsNaN(a)||double.IsNaN(b)) return double.IsNaN(a)&&double.IsNaN(b);
        if(a==0&&b==0) return BitConverter.DoubleToInt64Bits(a)==BitConverter.DoubleToInt64Bits(b);
        return a==b || !exact&&double.IsFinite(a)&&double.IsFinite(b)&&Math.Abs(a-b)<=1e-13*Math.Max(1,Math.Max(Math.Abs(a),Math.Abs(b)));
    }
    private static void Check(bool condition,string message){if(!condition)throw new Exception(message);}
    private static CandidateVector3 Vector(double[] a,int i)=>new(a[i],a[i+1],a[i+2]);
    private static CandidateCameraFrameState State(double[] a,int flags)=>new(new(a[0],a[1],a[2]),Vector(a,3),Vector(a,6),Vector(a,9),Vector(a,12),Vector(a,15),(flags&1)!=0,(flags&2)!=0,(flags&4)!=0,(flags&8)!=0,(flags&16)!=0);
    private static double[] Values(CandidateCameraFrameState s)=>new[]{s.Location.X,s.Location.Y,s.Location.Z,s.Direction.X,s.Direction.Y,s.Direction.Z,s.Up.X,s.Up.Y,s.Up.Z,s.X.X,s.X.Y,s.X.Z,s.Y.X,s.Y.Y,s.Y.Z,s.Z.X,s.Z.Y,s.Z.Z};
    public static string Run()
    {
        using var stream=typeof(CameraFrameAssertions).Assembly.GetManifestResourceStream("CameraFrameSourceCases")!;
        using var document=JsonDocument.Parse(stream);int cases=0,steps=0;
        foreach(var row in document.RootElement.GetProperty("cases").EnumerateArray())
        {
            var raw=row.GetProperty("state").EnumerateArray().Select(v=>Number(v.GetString()!)).ToArray();
            var frame=new CandidateCameraFrame(State(raw,row.GetProperty("flags").GetInt32()));
            var expected=row.GetProperty("expected").EnumerateArray().ToArray();int step=0;
            foreach(var operation in row.GetProperty("steps").EnumerateArray())
            {
                var v=operation.EnumerateArray().Select(x=>Number(x.GetString()!)).ToArray();
                bool ok=(int)v[0] switch{0=>frame.TrySetFrame(),1=>frame.TrySetLocation(new(v[1],v[2],v[3])),2=>frame.TrySetDirection(new(v[1],v[2],v[3])),3=>frame.TrySetUp(new(v[1],v[2],v[3])),_=>throw new Exception("Unknown frame operation")};
                var e=expected[step];string label="Frame case"+row.GetProperty("id")+" step"+step;
                Check(ok==e.GetProperty("ok").GetBoolean(),label+" return differs");
                Check(frame.Current.IsValidCamera==e.GetProperty("validCamera").GetBoolean()&&frame.Current.IsValidFrame==e.GetProperty("validFrame").GetBoolean(),label+" validity differs");
                Check(frame.ProjectionInvalidations==e.GetProperty("invalidations").GetInt32()&&frame.DiagnosticCount==e.GetProperty("diagnostics").GetInt32(),label+" cache/diagnostic differs");
                var actual=Values(frame.Current);var wanted=e.GetProperty("values").EnumerateArray().Select(x=>Number(x.GetString()!)).ToArray();
                for(int i=0;i<18;i++)Check(Same(actual[i],wanted[i],i<9),label+" component"+i+" differs: "+actual[i].ToString("R",CultureInfo.InvariantCulture)+" expected "+wanted[i].ToString("R",CultureInfo.InvariantCulture));
                step++;steps++;
            }
            Check(step==expected.Length,"Frame expected step count differs");cases++;
        }
        Check(cases==730&&steps==2978,"Frame source fixture count stale");
        int composed=0;
        foreach(var preset in new[]{(new CandidatePoint3(0,0,80),new CandidateVector3(0,0,-80),new CandidateVector3(0,1,0)),(new CandidatePoint3(0,-80,0),new CandidateVector3(0,80,0),new CandidateVector3(0,0,1)),(new CandidatePoint3(80,0,0),new CandidateVector3(-80,0,0),new CandidateVector3(0,0,1)),(new CandidatePoint3(45,-75,50),new CandidateVector3(-45,75,-50),new CandidateVector3(0,0,1))})
        {
            var f=new CandidateCameraFrame(new(preset.Item1,preset.Item2,preset.Item3,default,default,default,false,false,false,false,false));
            Check(f.TrySetFrame(),"Source preset frame rejected");var s=f.Current;
            var view=new CandidateCameraTransform(new double[16]);view.SetWorldToCamera(s.Location,s.X,s.Y,s.Z);var m=view.CopyCoefficients();
            for(int r=0;r<3;r++)Check(Math.Abs(m[4*r]*s.Location.X+m[4*r+1]*s.Location.Y+m[4*r+2]*s.Location.Z+m[4*r+3])<1e-12,"Camera eye does not map to origin");
            Check(m[11]<0,"Preset origin not in front of camera");
            var world=new CandidateCameraTransform(new double[16]);world.SetCameraToWorld(s.Location,s.X,s.Y,s.Z);var w=world.CopyCoefficients();
            for(int r=0;r<4;r++)for(int c=0;c<4;c++){double value=0;for(int k=0;k<4;k++)value+=m[4*r+k]*w[4*k+c];Check(Math.Abs(value-(r==c?1:0))<1e-12,"Frame world/camera inverse differs");}
            composed++;
        }
        return "PASS published camera frames:"+cases+" sequences/"+steps+" source steps; locks/validity/setter order/unset/threshold/degenerate state retention, cache/diagnostics and"+composed+" preset matrix compositions; rendered preset/private Rhino5/PC parity separate.";
    }
}
