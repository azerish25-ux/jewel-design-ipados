using System.Globalization;
using System.Text.Json;
using JewelDesign.Rendering.Candidate;
namespace CandidateGeometryContractChecks;

public static class FrustumAssertions
{
    private static double Number(string value)=>value switch{"nan"=>double.NaN,"inf"=>double.PositiveInfinity,"-inf"=>double.NegativeInfinity,_=>double.Parse(value,CultureInfo.InvariantCulture)};
    private static double[] Numbers(JsonElement array)=>array.EnumerateArray().Select(x=>Number(x.GetString()!)).ToArray();
    private static CandidateFrustumBounds Bounds(double[] v)=>new(v[0],v[1],v[2],v[3],v[4],v[5]);
    private static bool Same(double a,double b)
    {
        if(double.IsNaN(a)||double.IsNaN(b))return double.IsNaN(a)&&double.IsNaN(b);
        if(a==0&&b==0)return BitConverter.DoubleToInt64Bits(a)==BitConverter.DoubleToInt64Bits(b);
        if(a==b)return true;
        return double.IsFinite(a)&&double.IsFinite(b)&&Math.Abs(a-b)<=1e-13*Math.Max(1,Math.Max(Math.Abs(a),Math.Abs(b)));
    }
    private static void Check(bool value,string message){if(!value)throw new Exception(message);}
    public static string Run()
    {
        using var stream=typeof(FrustumAssertions).Assembly.GetManifestResourceStream("FrustumSourceCases")!;
        using var input=JsonDocument.Parse(stream);int cases=0;
        foreach(var row in input.RootElement.GetProperty("cases").EnumerateArray())
        {
            int flags=row.GetProperty("flags").GetInt32(),op=row.GetProperty("op").GetInt32(),id=row.GetProperty("id").GetInt32();
            var model=new CandidateViewportFrustum(Bounds(Numbers(row.GetProperty("before"))),(flags&1)!=0,new((flags&2)!=0,(flags&4)!=0,(flags&8)!=0,(flags&16)!=0));
            var args=Numbers(row.GetProperty("args"));double scalar=0;
            bool ok=op switch{0=>model.TrySetBounds(Bounds(args)),1=>model.TrySetLens(args[0]),2=>model.TrySetHalfAngle(args[0]),3=>model.TryGetLens(out scalar),4=>model.TryGetAspect(out scalar),_=>throw new Exception("Unknown frustum operation")};
            var expected=row.GetProperty("expected");string prefix="Native frustum case "+id+":";
            Check(ok==expected.GetProperty("ok").GetBoolean(),prefix+"return");
            Check(Same(scalar,Number(expected.GetProperty("scalar").GetString()!)),prefix+"scalar");
            Check(model.IsValid==expected.GetProperty("valid").GetBoolean()&&(model.ProjectionInvalidations>0)==expected.GetProperty("cacheReset").GetBoolean(),prefix+"validity/cache");
            Check(model.DiagnosticCount==expected.GetProperty("diagnostics").GetInt32()&&model.IsTwoPointPerspective==expected.GetProperty("twoPoint").GetBoolean(),prefix+"diagnostic/two-point");
            var actual=new[]{model.Bounds.Left,model.Bounds.Right,model.Bounds.Bottom,model.Bounds.Top,model.Bounds.Near,model.Bounds.Far};var wanted=Numbers(expected.GetProperty("bounds"));
            for(int i=0;i<6;i++)Check(Same(actual[i],wanted[i]),prefix+"bounds"+i);
            cases++;
        }
        Check(cases==1245,"Native frustum fixture count differs");
        return "PASS published frustum/lens:"+cases+" independently compiled-source cases, symmetry/two-point/deadband/raw-invalid/sentinel/world-bound/depth-diagnostic/state retention; private Rhino5/Metal projection/PC parity separate.";
    }
}
