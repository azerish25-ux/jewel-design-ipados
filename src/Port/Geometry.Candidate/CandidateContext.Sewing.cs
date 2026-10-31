using System.Runtime.InteropServices;
namespace JewelDesign.Geometry.Candidate;
public readonly record struct CandidateSewingInfo(uint Parts,uint Faces,uint FreeEdges,uint MultipleEdges,uint ContiguousEdges,uint DeletedFaces,uint ClosedShells,double RequestedTolerance,double NativeTolerance,double NativeMinimumTolerance,double NativeMaximumTolerance);
public sealed class CandidateSewingResult:IDisposable
{
 public CandidateShape Shape {get;}public CandidateSewingInfo Info {get;}
 internal CandidateSewingResult(CandidateShape shape,CandidateSewingInfo info){Shape=shape;Info=info;}
 public void Dispose()=>Shape.Dispose();
}
public sealed partial class CandidateContext
{
 /// <summary>Sew copied native BRep inputs and expose actual topology diagnostics. This does not implement legacy Rhino repair/filter policy.</summary>
 public CandidateSewingResult SewBreps(IReadOnlyList<CandidateShape> shapes,double tolerance)
 {
  ArgumentNullException.ThrowIfNull(shapes);if(shapes.Count==0||shapes.Count>65536)throw new ArgumentOutOfRangeException(nameof(shapes));
  var inputs=new CandidateShape[shapes.Count];for(int i=0;i<inputs.Length;i++)inputs[i]=shapes[i];NativeSewingInfo native=default;
  var result=CreateShape(diagnostic=>{var ids=inputs.Select(x=>x.Handle.RawId).ToArray();var status=NativeMethods.Sew(_handle,ids,(uint)ids.Length,tolerance,out var id,out native,diagnostic,DiagnosticCapacity);return(status,id);},inputs);
  try{
   if(native.Reserved!=0||native.Parts==0||native.Faces==0||native.Parts>int.MaxValue||native.Faces>int.MaxValue||native.FreeEdges>int.MaxValue||native.MultipleEdges>int.MaxValue||native.ContiguousEdges>int.MaxValue||native.DeletedFaces>int.MaxValue||native.ClosedShells>int.MaxValue||!double.IsFinite(native.Tolerance)||!double.IsFinite(native.Minimum)||!double.IsFinite(native.Maximum)||native.Tolerance<=0||native.Minimum<=0||native.Maximum<native.Minimum)throw new InvalidOperationException("Invalid native sewing diagnostics.");
   return new(result,new(native.Parts,native.Faces,native.FreeEdges,native.MultipleEdges,native.ContiguousEdges,native.DeletedFaces,native.ClosedShells,tolerance,native.Tolerance,native.Minimum,native.Maximum));
  }catch{result.Dispose();throw;}
 }
}
[StructLayout(LayoutKind.Sequential)]
internal struct NativeSewingInfo
{
 internal uint Parts,Faces,FreeEdges,MultipleEdges,ContiguousEdges,DeletedFaces,ClosedShells,Reserved;
 internal double Tolerance,Minimum,Maximum;
}
internal static partial class NativeMethods
{
 [DllImport(Library,EntryPoint="jd_candidate_sew",CallingConvention=CallingConvention.Cdecl,ExactSpelling=true)]
 internal static extern int Sew(ContextHandle context,[In]ulong[] shapes,uint count,double tolerance,out ulong output,out NativeSewingInfo info,[Out]byte[] diagnostic,uint capacity);
}
