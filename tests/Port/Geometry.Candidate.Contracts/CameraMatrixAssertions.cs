using System.Globalization;
using System.Numerics;
using System.Text.Json;
using JewelDesign.Geometry.Candidate;
using JewelDesign.Rendering.Candidate;
namespace CandidateGeometryContractChecks;

public static class CameraMatrixAssertions
{
    private static double Number(string s)=>s switch{"nan"=>double.NaN,"inf"=>double.PositiveInfinity,"-inf"=>double.NegativeInfinity,_=>double.Parse(s,CultureInfo.InvariantCulture)};
    private static bool Same(double x,double y){if(double.IsNaN(x)||double.IsNaN(y))return double.IsNaN(x)&&double.IsNaN(y);if(x==0&&y==0)return BitConverter.DoubleToInt64Bits(x)==BitConverter.DoubleToInt64Bits(y);if(x==y)return true;return double.IsFinite(x)&&double.IsFinite(y)&&Math.Abs(x-y)<=1e-13*Math.Max(1,Math.Max(Math.Abs(x),Math.Abs(y)));}
    private static void Check(bool value,string message){if(!value)throw new Exception(message);}
    private static CandidatePoint3 Point(double[] v,int start)=>new(v[start],v[start+1],v[start+2]);
    private static CandidateVector3 Vector(double[] v,int start)=>new(v[start],v[start+1],v[start+2]);
    public static string Run()
    {
        using var stream=typeof(CameraMatrixAssertions).Assembly.GetManifestResourceStream("CameraMatrixSourceCases")!;using var document=JsonDocument.Parse(stream);int cases=0;
        foreach(var row in document.RootElement.GetProperty("cases").EnumerateArray())
        {
            var transform=new CandidateCameraTransform(Enumerable.Range(1,16).Select(i=>(double)i).ToArray());
            var v=row.GetProperty("values").EnumerateArray().Select(x=>Number(x.GetString()!)).ToArray();int op=row.GetProperty("op").GetInt32();bool ok=true;
            if(op==0)ok=transform.TrySetCameraToClip(row.GetProperty("perspective").GetBoolean(),new(v[0],v[1],v[2],v[3],v[4],v[5]));
            else if(op==1)transform.SetWorldToCamera(Point(v,0),Vector(v,3),Vector(v,6),Vector(v,9));
            else if(op==2)transform.SetCameraToWorld(Point(v,0),Vector(v,3),Vector(v,6),Vector(v,9));else throw new Exception("Unknown matrix case");
            var expected=row.GetProperty("expected");Check(ok==expected.GetProperty("ok").GetBoolean(),"Source camera matrix return differs");
            var actual=transform.CopyCoefficients();var wanted=expected.GetProperty("matrix").EnumerateArray().Select(x=>Number(x.GetString()!)).ToArray();
            for(int i=0;i<16;i++)Check(Same(actual[i],wanted[i]),"Source camera matrix coefficient differs:case"+row.GetProperty("id")+" index"+i);
            cases++;
        }
        Check(cases==118,"Camera matrix native fixture count stale");int endpoints=0;
        foreach(bool perspective in new[]{false,true})
        {
            var projection=new CandidateCameraTransform(new double[16]);Check(projection.TrySetCameraToClip(perspective,new(-2,4,-1,3,1,100)),"Valid source projection rejected");
            Check(projection.TryGetMetalClipMatrix(out var metal),"Valid Metal projection rejected");
            foreach(float depth in new[]{1f,100f})foreach(float x in new[]{-2f,4f})foreach(float y in new[]{-1f,3f})
            {
                float scale=perspective?depth:1;var clip=Vector4.Transform(new Vector4(x*scale,y*scale,-depth,1),metal);
                Check(Math.Abs(clip.X/clip.W-(x==-2?-1:1))<2e-5&&Math.Abs(clip.Y/clip.W-(y==-1?-1:1))<2e-5&&Math.Abs(clip.Z/clip.W-(depth==1?0:1))<2e-5,"Source-to-Metal corner/depth convention differs");endpoints++;
            }
            if(perspective)Check(Vector4.Transform(new Vector4(0,0,1,1),metal).W<0,"Behind-camera homogeneous sign changed");
        }
        var packing=new CandidateCameraTransform(Enumerable.Range(1,16).Select(i=>(double)i).ToArray());Check(packing.TryGetMetalClipMatrix(out var packed),"Finite matrix packing failed");
        Check(Vector4.Transform(new Vector4(2,3,4,1),packed)==new Vector4(24,64,20,144),"Metal row/column/depth packing differs");
        foreach(double bad in new[]{double.NaN,double.PositiveInfinity,double.MaxValue}){var values=Enumerable.Range(1,16).Select(i=>(double)i).ToArray();values[0]=bad;Check(!new CandidateCameraTransform(values).TryGetMetalClipMatrix(out _),"Invalid/nonrepresentable GPU matrix accepted");}
        return "PASS published camera matrices:"+cases+" compiled-source cases and"+endpoints+" independent Metal clip endpoints; source coefficients/retained failure/frame basis, depth0..1/transpose and invalid float guards; full preset/private Rhino5/PC parity separate.";
    }
}
