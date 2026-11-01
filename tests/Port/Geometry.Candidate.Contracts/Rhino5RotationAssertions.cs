using System.Globalization;
using System.Text.Json;
using JewelDesign.Geometry.Candidate;
using JewelDesign.Rendering.Candidate;
namespace CandidateGeometryContractChecks;
public static class Rhino5RotationAssertions
{
    private static int checks;
    private static void Check(bool ok,string message){checks++;if(!ok)throw new Exception(message);}
    private static bool Near(double a,double b)=>Math.Abs(a-b)<1e-11;
    private static void Vector(CandidateVector3 a,CandidateVector3 b,string message)=>Check(Near(a.X,b.X)&&Near(a.Y,b.Y)&&Near(a.Z,b.Z),message);
    public static string Run()
    {
        checks=0;
        Vector(Rhino5VectorRotation.FromAngle(Math.PI/2,new(0,0,7)).Apply(new(1,0,0)),new(0,1,0),"Right handed quarter turn/nonunit axis");
        Vector(Rhino5VectorRotation.FromSinCos(3,4,new(0,0,1)).Apply(new(1,0,0)),new(.8,.6,0),"Scaled sin/cos normalization");
        Vector(Rhino5VectorRotation.FromSinCos(.6,.8,new(0,0,0)).Apply(new(5,10,15)),new(4,8,12),"Zero axis preserves native cosine scaling");
        var failed=Rhino5VectorRotation.FromSinCos(0,0,new(1,2,3));
        Check(failed.DiagnosticCount==1,"Failed pair normalization diagnostic");Vector(failed.Apply(new(2,3,4)),new(2,3,4),"Failed pair returns identity");
        Vector(Rhino5VectorRotation.FromSinCos(1,1e-10,new(0,0,1)).Apply(new(1,0,0)),new(0,1,0),"Cardinal cleanup");
        foreach(var axis in new[]{new CandidateVector3(1,2,3),new(1e200,2e200,3e200),new(1e-300,2e-300,3e-300)})
        {
            var r=Rhino5VectorRotation.FromAngle(.731,axis);var v=new CandidateVector3(2,-3,5);var a=r.Apply(v);
            Check(Near(a.X*a.X+a.Y*a.Y+a.Z*a.Z,38),"General-axis length preservation");
            Vector(Rhino5VectorRotation.FromAngle(-.731,axis).Apply(a),v,"Inverse rotation");
            double len=Math.Sqrt(14);var u=new CandidateVector3(1/len,2/len,3/len);Vector(r.Apply(u),u,"Axis fixed point");
        }
        using var stream=typeof(Rhino5RotationAssertions).Assembly.GetManifestResourceStream("CameraRotationSourceCases")!;
        using var json=JsonDocument.Parse(stream);int cases=0;
        foreach(var row in json.RootElement.GetProperty("cases").EnumerateArray())
        {
            var a=row.GetProperty("args").EnumerateArray().Select(x=>double.Parse(x.GetString()!,CultureInfo.InvariantCulture)).ToArray();
            var axis=new CandidateVector3(a[3],a[4],a[5]);var r=a[0]==0?Rhino5VectorRotation.FromSinCos(a[1],a[2],axis):Rhino5VectorRotation.FromAngle(a[1],axis);
            Check(r.DiagnosticCount==row.GetProperty("diagnostics").GetInt32(),"Compiled source diagnostic "+cases);
            var actual=r.Apply(new(a[6],a[7],a[8]));var values=new[]{actual.X,actual.Y,actual.Z};int j=0;
            foreach(var n in row.GetProperty("vector").EnumerateArray())
            {
                string token=n.GetString()!;double expected=token=="nan"?double.NaN:token=="inf"?double.PositiveInfinity:token=="-inf"?double.NegativeInfinity:double.Parse(token,CultureInfo.InvariantCulture);double got=values[j++];
                Check((double.IsNaN(got)&&double.IsNaN(expected))||got==expected||(double.IsFinite(got)&&double.IsFinite(expected)&&Math.Abs(got-expected)<=2e-13*Math.Max(1,Math.Max(Math.Abs(got),Math.Abs(expected)))),"Compiled source vector "+cases+":"+j);
            }
            cases++;
        }
        Check(cases==608,"Compiled rotation fixture count");
        return "PASS original-derived Rhino5 vector rotation:"+checks+" checks including "+cases+" compiled published-source comparisons and15 analytical/edge checks; original runtime/PC parity separate.";
    }
}
