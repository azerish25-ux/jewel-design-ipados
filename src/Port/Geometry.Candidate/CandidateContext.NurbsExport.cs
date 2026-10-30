using System.Runtime.InteropServices;
namespace JewelDesign.Geometry.Candidate;
public sealed partial class CandidateContext
{
 /// <summary>Export owned native nonperiodic controls, weights and full knots for the actual edge interval.</summary>
 public CandidateNurbsDefinition ExportNurbs(CandidateShape shape)
 {
  lock(_gate){
   ThrowIfDisposed();Validate(shape);var diagnostic=new byte[DiagnosticCapacity];
   Check(NativeMethods.CurveNurbsExport(_handle,shape.Handle,out var degree,out var count,out var knotCount,out var rational,null,0,null,0,null,0,diagnostic,DiagnosticCapacity),diagnostic);
   if(degree==0||degree>64||count<=degree||count>65536||knotCount!=count+degree+1||rational>1)throw new InvalidOperationException("Invalid native NURBS export dimensions.");
   var coordinates=new double[checked((int)count*3)];var knots=new double[checked((int)knotCount)];var weights=rational==1?new double[checked((int)count)]:null;
   Check(NativeMethods.CurveNurbsExport(_handle,shape.Handle,out var d2,out var n2,out var k2,out var r2,coordinates,(uint)coordinates.Length,weights,(uint)(weights?.Length??0),knots,knotCount,diagnostic,DiagnosticCapacity),diagnostic);
   if(degree!=d2||count!=n2||knotCount!=k2||rational!=r2)throw new InvalidOperationException("Native NURBS export changed between passes.");
   if(coordinates.Any(x=>!double.IsFinite(x))||knots.Any(x=>!double.IsFinite(x))||(weights?.Any(x=>!double.IsFinite(x)||x<=0)??false))throw new InvalidOperationException("Invalid native NURBS export coefficients.");
   var points=new CandidatePoint3[checked((int)count)];for(int i=0;i<points.Length;i++)points[i]=new(coordinates[i*3],coordinates[i*3+1],coordinates[i*3+2]);
   return new(degree,points,knots,weights);
  }
 }
}
internal static partial class NativeMethods
{
 [DllImport(Library,EntryPoint="jd_candidate_curve_nurbs_export",CallingConvention=CallingConvention.Cdecl,ExactSpelling=true)]
 internal static extern int CurveNurbsExport(ContextHandle context,ShapeHandle shape,out uint degree,out uint count,out uint knotCount,out uint rational,[Out]double[]? coordinates,uint coordinateCapacity,[Out]double[]? weights,uint weightCapacity,[Out]double[]? knots,uint knotCapacity,[Out]byte[] diagnostic,uint capacity);
}
