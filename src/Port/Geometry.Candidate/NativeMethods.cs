using System.Runtime.InteropServices;
namespace JewelDesign.Geometry.Candidate;

internal static class NativeMethods
{
    private const string Library="__Internal";
    [DllImport(Library,EntryPoint="jd_candidate_abi_version",CallingConvention=CallingConvention.Cdecl,ExactSpelling=true)]
    internal static extern uint AbiVersion();
    [DllImport(Library,EntryPoint="jd_candidate_context_create",CallingConvention=CallingConvention.Cdecl,ExactSpelling=true)]
    internal static extern int ContextCreate(uint version,out ulong output,[Out]byte[] diagnostic,uint capacity);
    [DllImport(Library,EntryPoint="jd_candidate_context_release",CallingConvention=CallingConvention.Cdecl,ExactSpelling=true)]
    internal static extern int ContextReleaseRaw(ulong context,[Out]byte[]? diagnostic,uint capacity);
    [DllImport(Library,EntryPoint="jd_candidate_context_live_shapes",CallingConvention=CallingConvention.Cdecl,ExactSpelling=true)]
    internal static extern int ContextLiveShapes(ContextHandle context,out ulong count,[Out]byte[] diagnostic,uint capacity);
    [DllImport(Library,EntryPoint="jd_candidate_context_live_shapes",CallingConvention=CallingConvention.Cdecl,ExactSpelling=true)]
    internal static extern int ContextLiveShapesRaw(ulong context,out ulong count,[Out]byte[] diagnostic,uint capacity);
    [DllImport(Library,EntryPoint="jd_candidate_box",CallingConvention=CallingConvention.Cdecl,ExactSpelling=true)]
    internal static extern int Box(ContextHandle context,double x,double y,double z,out ulong output,[Out]byte[] diagnostic,uint capacity);
    [DllImport(Library,EntryPoint="jd_candidate_translate",CallingConvention=CallingConvention.Cdecl,ExactSpelling=true)]
    internal static extern int Translate(ContextHandle context,ShapeHandle shape,double x,double y,double z,out ulong output,[Out]byte[] diagnostic,uint capacity);
    [DllImport(Library,EntryPoint="jd_candidate_shape_clone",CallingConvention=CallingConvention.Cdecl,ExactSpelling=true)]
    internal static extern int ShapeClone(ContextHandle context,ShapeHandle shape,out ulong output,[Out]byte[] diagnostic,uint capacity);
    [DllImport(Library,EntryPoint="jd_candidate_boolean",CallingConvention=CallingConvention.Cdecl,ExactSpelling=true)]
    internal static extern int Boolean(ContextHandle context,int operation,ShapeHandle lhs,ShapeHandle rhs,double fuzzy,out ulong output,[Out]byte[] diagnostic,uint capacity);
    [DllImport(Library,EntryPoint="jd_candidate_shape_retain",CallingConvention=CallingConvention.Cdecl,ExactSpelling=true)]
    internal static extern int ShapeRetain(ContextHandle context,ShapeHandle shape,[Out]byte[] diagnostic,uint capacity);
    [DllImport(Library,EntryPoint="jd_candidate_shape_release",CallingConvention=CallingConvention.Cdecl,ExactSpelling=true)]
    internal static extern int ShapeReleaseRaw(ulong context,ulong shape,[Out]byte[]? diagnostic,uint capacity);
    [DllImport(Library,EntryPoint="jd_candidate_shape_inspect",CallingConvention=CallingConvention.Cdecl,ExactSpelling=true)]
    internal static extern int ShapeInspect(ContextHandle context,ShapeHandle shape,out double volume,out uint solids,out uint valid,[Out]byte[] diagnostic,uint capacity);
    [DllImport(Library,EntryPoint="jd_candidate_circle",CallingConvention=CallingConvention.Cdecl,ExactSpelling=true)]
    internal static extern int Circle(ContextHandle context,double radius,double ox,double oy,double oz,double nx,double ny,double nz,double xx,double xy,double xz,out ulong output,[Out]byte[] diagnostic,uint capacity);
    [DllImport(Library,EntryPoint="jd_candidate_curve_inspect",CallingConvention=CallingConvention.Cdecl,ExactSpelling=true)]
    internal static extern int CurveInspect(ContextHandle context,ShapeHandle shape,out double first,out double last,out double length,out uint degree,out uint poles,out uint rational,[Out]byte[] diagnostic,uint capacity);
    [DllImport(Library,EntryPoint="jd_candidate_curve_point",CallingConvention=CallingConvention.Cdecl,ExactSpelling=true)]
    internal static extern int CurvePoint(ContextHandle context,ShapeHandle shape,double parameter,out double x,out double y,out double z,[Out]byte[] diagnostic,uint capacity);
    [DllImport(Library,EntryPoint="jd_candidate_curve_length_parameter",CallingConvention=CallingConvention.Cdecl,ExactSpelling=true)]
    internal static extern int CurveLengthParameter(ContextHandle context,ShapeHandle shape,double fraction,double tolerance,out double parameter,[Out]byte[] diagnostic,uint capacity);
    [DllImport(Library,EntryPoint="jd_candidate_affine",CallingConvention=CallingConvention.Cdecl,ExactSpelling=true)]
    internal static extern int Affine(ContextHandle context,ShapeHandle shape,[In]double[] matrix,uint count,out ulong output,[Out]byte[] diagnostic,uint capacity);
    [DllImport(Library,EntryPoint="jd_candidate_curve_closest",CallingConvention=CallingConvention.Cdecl,ExactSpelling=true)]
    internal static extern int CurveClosest(ContextHandle context,ShapeHandle shape,double px,double py,double pz,out double parameter,out double x,out double y,out double z,out double distance,[Out]byte[] diagnostic,uint capacity);
}
