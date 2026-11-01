using System.Globalization;
using System.Text.Json;
using JewelDesign.Geometry.Candidate;
using JewelDesign.Rendering.Candidate;
namespace CandidateGeometryContractChecks;

public static class ViewportProjectionAssertions
{
    private static double Number(string s)=>s switch {"nan"=>double.NaN,"inf"=>double.PositiveInfinity,"-inf"=>double.NegativeInfinity,_=>double.Parse(s,CultureInfo.InvariantCulture)};
    private static bool Same(double a,double b,bool exact=false)
    {
        if(double.IsNaN(a)||double.IsNaN(b))return double.IsNaN(a)&&double.IsNaN(b);
        if(a==0&&b==0)return BitConverter.DoubleToInt64Bits(a)==BitConverter.DoubleToInt64Bits(b);
        return a==b||!exact&&double.IsFinite(a)&&double.IsFinite(b)&&Math.Abs(a-b)<=1e-13*Math.Max(1,Math.Max(Math.Abs(a),Math.Abs(b)));
    }
    private static void Check(bool value,string message){if(!value)throw new Exception(message);}
    private static CandidatePoint3 Point(double[] a,int i)=>new(a[i],a[i+1],a[i+2]);
    private static CandidateVector3 Vector(double[] a,int i)=>new(a[i],a[i+1],a[i+2]);
    private static CandidateViewportProjectionState State(double[] a,int flags,uint projection,byte symmetry)=>new(
        new(Point(a,0),Vector(a,3),Vector(a,6),Vector(a,9),Vector(a,12),Vector(a,15),(flags&1)!=0,(flags&2)!=0,(flags&4)!=0,(flags&8)!=0,(flags&16)!=0),
        new(a[18],a[19],a[20],a[21],a[22],a[23]),(flags&32)!=0,projection,symmetry,Point(a,24));
    private static double[] Values(CandidateViewportProjectionState s)
    {
        var f=s.Frame;var b=s.Bounds;
        return new[]{f.Location.X,f.Location.Y,f.Location.Z,f.Direction.X,f.Direction.Y,f.Direction.Z,f.Up.X,f.Up.Y,f.Up.Z,f.X.X,f.X.Y,f.X.Z,f.Y.X,f.Y.Y,f.Y.Z,f.Z.X,f.Z.Y,f.Z.Z,b.Left,b.Right,b.Bottom,b.Top,b.Near,b.Far,s.Target.X,s.Target.Y,s.Target.Z};
    }
    private static int Flags(CandidateViewportProjectionState s)=>(s.Frame.IsValidCamera?1:0)|(s.Frame.IsValidFrame?2:0)|(s.Frame.LocationLocked?4:0)|(s.Frame.DirectionLocked?8:0)|(s.Frame.UpLocked?16:0)|(s.IsValidFrustum?32:0);
    private static bool Retained(int op,int i)=>i<3?op!=0&&op!=1&&op!=5:i<6?op!=6:i<9?op!=7:i<18?op!=6&&op!=7&&op!=12:i<24?op!=0&&op!=1&&op!=8&&op!=9:op!=0&&op!=4;
    public static string Run()
    {
        using var stream=typeof(ViewportProjectionAssertions).Assembly.GetManifestResourceStream("ViewportProjectionSourceCases")!;
        using var document=JsonDocument.Parse(stream);int sequences=0,steps=0;
        foreach(var row in document.RootElement.GetProperty("cases").EnumerateArray())
        {
            var raw=row.GetProperty("state").EnumerateArray().Select(x=>Number(x.GetString()!)).ToArray();
            var viewport=new CandidateViewportProjection(State(raw,row.GetProperty("flags").GetInt32(),row.GetProperty("projection").GetUInt32(),row.GetProperty("symmetry").GetByte()));
            var expected=row.GetProperty("expected").EnumerateArray().ToArray();int index=0;
            foreach(var operation in row.GetProperty("steps").EnumerateArray())
            {
                var a=operation.EnumerateArray().Select(x=>Number(x.GetString()!)).ToArray();int op=(int)a[0];bool ok=true;double result=0;var before=Values(viewport.Current);
                switch(op)
                {
                    case 0:ok=viewport.TryChangeToParallel(a[1]!=0);break;
                    case 1:ok=viewport.TryChangeToSymmetric(a[1]!=0,a[2]!=0,a[3]);break;
                    case 2:result=viewport.TargetDistance(a[1]!=0);break;
                    case 3:ok=viewport.TrySetProjection((uint)a[1]);break;
                    case 4:ok=viewport.TrySetTarget(Point(a,1));break;
                    case 5:ok=viewport.TrySetLocation(Point(a,1));break;
                    case 6:ok=viewport.TrySetDirection(Vector(a,1));break;
                    case 7:ok=viewport.TrySetUp(Vector(a,1));break;
                    case 8:ok=viewport.TrySetLens(a[1]);break;
                    case 9:ok=viewport.TrySetBounds(new(a[1],a[2],a[3],a[4],a[5],a[6]));break;
                    case 10:viewport.SetCameraLocks(a[1]!=0,a[2]!=0,a[3]!=0);break;
                    case 11:viewport.SetFrustumSymmetry(a[1]!=0,a[2]!=0);break;
                    case 12:ok=viewport.TrySetFrame();break;
                    default:throw new Exception("Unknown viewport operation");
                }
                var e=expected[index];string label="Viewport case"+row.GetProperty("id")+" step"+index;
                Check(ok==e.GetProperty("ok").GetBoolean()&&Same(result,Number(e.GetProperty("result").GetString()!)),label+" return/query differs");
                Check(Flags(viewport.Current)==e.GetProperty("flags").GetInt32()&&viewport.Current.Projection==e.GetProperty("projection").GetUInt32()&&viewport.Current.SymmetryFlags==e.GetProperty("symmetry").GetByte(),label+" flags/projection/symmetry differ");
                Check(viewport.ProjectionInvalidations==e.GetProperty("invalidations").GetInt32()&&viewport.DiagnosticCount==e.GetProperty("diagnostics").GetInt32(),label+" invalidations/diagnostics differ");
                var actual=Values(viewport.Current);var wanted=e.GetProperty("values").EnumerateArray().Select(x=>Number(x.GetString()!)).ToArray();
                for(int i=0;i<27;i++)Check(Same(actual[i],wanted[i],i>=3&&i<9||op==5&&i<3||op==4&&i>=24),label+" component"+i+" differs: "+actual[i].ToString("R",CultureInfo.InvariantCulture)+" expected "+wanted[i].ToString("R",CultureInfo.InvariantCulture));
                for(int i=0;i<27;i++)if(Retained(op,i))Check(BitConverter.DoubleToInt64Bits(actual[i])==BitConverter.DoubleToInt64Bits(before[i]),label+" unexpectedly changed retained component"+i);
                index++;steps++;
            }
            Check(index==expected.Length,"Projection fixture step count stale");sequences++;
        }
        Check(sequences==961&&steps==6675,"Projection source case count stale");
        return "PASS published viewport transitions:"+sequences+" sequences/"+steps+" source steps; joint frame/frustum/target state, lock clearing/unknown symmetry bits, dolly/parallel scale, fallback/jitter and failure mutation; source-preset rendering/private Rhino5/PC parity separate.";
    }
}
