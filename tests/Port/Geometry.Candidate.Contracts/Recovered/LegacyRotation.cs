#nullable disable
// Exact recovered Update/ToRadians with test observation storage and pinned published6 native axis math.
using JewelDesign.Documents;
namespace CandidateGeometryContractChecks.Recovered.Rotation;
internal readonly record struct Vector3d(uint Axis){internal static Vector3d XAxis=>new(0);internal static Vector3d YAxis=>new(1);internal static Vector3d ZAxis=>new(2);}
internal readonly record struct Point3d{internal static Point3d Origin=>default;}
internal readonly record struct Transform(LegacyAffineTransform Matrix)
{
    private static double At(double[] a,int r,int c)=>r==3?(c==3?1:0):a[4*r+c];
    private static double[] Fields(LegacyAffineTransform m)=>new[]{m.M11,m.M12,m.M13,m.M14,m.M21,m.M22,m.M23,m.M24,m.M31,m.M32,m.M33,m.M34};
    public static Transform operator*(Transform left,Transform right){
        var a=Fields(left.Matrix);var b=Fields(right.Matrix);var c=new double[12];
        for(int i=0;i<3;i++)for(int j=0;j<4;j++)for(int k=0;k<4;k++)c[4*i+j]+=At(a,i,k)*At(b,k,j);
        return new(new(c[0],c[1],c[2],c[3],c[4],c[5],c[6],c[7],c[8],c[9],c[10],c[11]));
    }
}
internal static class CSBlockUtil{internal static class CSUtilMath{
internal static double On_DEGREES_TO_RADIANS=>Math.PI/180.0;
internal static Transform Rotation(double angle,Vector3d axis,Point3d origin)=>new(NativeRotationReference.Axis(angle,axis.Axis));
public static double ToRadians(double angleDegrees)
	{
		return On_DEGREES_TO_RADIANS * angleDegrees;
	}
}}
internal sealed class Input{private readonly LegacyChannelValue value;internal Input(LegacyChannelValue value){this.value=value;}internal bool GetDouble(ref double number)=>value.GetDouble(ref number);}
internal sealed class CSXformData{internal Transform Transform;internal CSXformData(Transform transform){Transform=transform;}}
internal sealed class Output{internal CSXformData Data;internal void SetData(CSXformData data){Data=data;}}
internal class CSBlock{public virtual bool Update()=>true;}
internal sealed class CSRotateObjectDegXform:CSBlock{
private readonly Input iX,iY,iZ,iMirror;internal readonly Output oXform=new();
internal CSRotateObjectDegXform(LegacyChannelValue x,LegacyChannelValue y,LegacyChannelValue z,LegacyChannelValue mirror){iX=new(x);iY=new(y);iZ=new(z);iMirror=new(mirror);}
public override bool Update()
	{
		double value = 0.0;
		iX.GetDouble(ref value);
		double value2 = 0.0;
		iY.GetDouble(ref value2);
		double value3 = 0.0;
		iZ.GetDouble(ref value3);
		double value4 = 0.0;
		iMirror.GetDouble(ref value4);
		if (value4 > 0.5)
		{
			value = 0.0 - value;
			value3 = 0.0 - value3;
		}
		Transform transform = CSBlockUtil.CSUtilMath.Rotation(CSBlockUtil.CSUtilMath.ToRadians(value), Vector3d.XAxis, Point3d.Origin);
		Transform transform2 = CSBlockUtil.CSUtilMath.Rotation(CSBlockUtil.CSUtilMath.ToRadians(value2), Vector3d.YAxis, Point3d.Origin);
		Transform transform3 = CSBlockUtil.CSUtilMath.Rotation(CSBlockUtil.CSUtilMath.ToRadians(value3), Vector3d.ZAxis, Point3d.Origin);
		Transform transform4 = transform * transform2 * transform3;
		oXform.SetData(new CSXformData(transform4));
		return base.Update();
	}
}
