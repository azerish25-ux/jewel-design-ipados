using System.Runtime.InteropServices;
namespace JewelDesign.Geometry.Candidate;
internal static partial class NativeMethods
{
 [DllImport(Library,EntryPoint="jd_candidate_paired_sweep",CallingConvention=CallingConvention.Cdecl,ExactSpelling=true)]
 internal static extern int PairedSweep(ContextHandle context,ShapeHandle rail1,ShapeHandle rail2,[In]ulong[] profiles,uint count,[In]double[] a,uint countA,[In]double[] b,uint countB,uint maintainHeight,uint correspondence,double tolerance,out ulong output,[Out]byte[] diagnostic,uint capacity);
 [DllImport(Library,EntryPoint="jd_candidate_surface_bounds",CallingConvention=CallingConvention.Cdecl,ExactSpelling=true)]
 internal static extern int SurfaceBounds(ContextHandle context,ShapeHandle shape,out double firstU,out double lastU,out double firstV,out double lastV,[Out]byte[] diagnostic,uint capacity);
 [DllImport(Library,EntryPoint="jd_candidate_surface_point",CallingConvention=CallingConvention.Cdecl,ExactSpelling=true)]
 internal static extern int SurfacePoint(ContextHandle context,ShapeHandle shape,double u,double v,out double x,out double y,out double z,[Out]byte[] diagnostic,uint capacity);
}
