using System.Runtime.InteropServices;
using JewelDesign.Documents;
namespace CandidateGeometryContractChecks;
internal static class NativeRotationReference
{
    [DllImport("__Internal",EntryPoint="jd_test_rotation_axis",CallingConvention=CallingConvention.Cdecl,ExactSpelling=true)]
    private static extern int AxisRaw(double radians,uint axis,[Out] double[] output,uint count);
    [DllImport("__Internal",EntryPoint="jd_test_rotation_xyz",CallingConvention=CallingConvention.Cdecl,ExactSpelling=true)]
    private static extern int XyzRaw(double x,double y,double z,double mirror,[Out] double[] output,uint count);
    private static LegacyAffineTransform Matrix(double[] a)=>new(a[0],a[1],a[2],a[3],a[4],a[5],a[6],a[7],a[8],a[9],a[10],a[11]);
    internal static LegacyAffineTransform Axis(double radians,uint axis)
    {var a=new double[12];if(AxisRaw(radians,axis,a,12)!=0)throw new NotSupportedException("Native published comparison rejected angle/axis.");return Matrix(a);}
    internal static LegacyAffineTransform Xyz(double x,double y,double z,double mirror)
    {var a=new double[12];if(XyzRaw(x,y,z,mirror,a,12)!=0)throw new NotSupportedException("Native published comparison rejected angles.");return Matrix(a);}
}
