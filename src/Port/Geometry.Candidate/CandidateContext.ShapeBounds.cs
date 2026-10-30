using System.Runtime.InteropServices;
namespace JewelDesign.Geometry.Candidate;
public enum CandidateShapeBoundsMode:uint {Fast=0,Accurate=1}
public sealed partial class CandidateContext
{
 /// <summary>Candidate native box policy, not a Rhino equivalence guarantee. RequestedTolerance0 means kernel policy; native margin is explicit.</summary>
 public CandidateCurveBounds GetShapeBounds(CandidateShape shape,CandidateShapeBoundsMode mode)
 {
  lock(_gate){ThrowIfDisposed();Validate(shape);var diagnostic=new byte[DiagnosticCapacity];Check(NativeMethods.ShapeBounds(_handle,shape.Handle,(uint)mode,out var x0,out var y0,out var z0,out var x1,out var y1,out var z1,out var margin,diagnostic,DiagnosticCapacity),diagnostic);
   foreach(double value in new[]{x0,y0,z0,x1,y1,z1,margin,x0-margin,y0-margin,z0-margin,x1+margin,y1+margin,z1+margin})if(!double.IsFinite(value))throw new InvalidOperationException("Invalid native shape bounds response.");
   if(x0>x1||y0>y1||z0>z1||margin<0)throw new InvalidOperationException("Invalid native shape bounds interval.");return new(new(x0,y0,z0),new(x1,y1,z1),0,margin);
  }
 }
}
internal static partial class NativeMethods
{
 [DllImport(Library,EntryPoint="jd_candidate_shape_bounds",CallingConvention=CallingConvention.Cdecl,ExactSpelling=true)]
 internal static extern int ShapeBounds(ContextHandle context,ShapeHandle shape,uint accurate,out double x0,out double y0,out double z0,out double x1,out double y1,out double z1,out double padding,[Out]byte[] diagnostic,uint capacity);
}
