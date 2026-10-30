using System.Runtime.InteropServices;
namespace JewelDesign.Geometry.Candidate;
public sealed partial class CandidateContext
{
 public CandidateShape CreateNurbsSurface(CandidateNurbsSurfaceDefinition definition,double tolerance){
  ArgumentNullException.ThrowIfNull(definition);
  return CreateShape(diagnostic=>{var status=NativeMethods.NurbsSurface(_handle,definition.DegreeU,definition.DegreeV,definition.CountU,definition.CountV,definition.Weights is null?0u:1u,definition.Coordinates,checked((uint)definition.Coordinates.Length),definition.Weights,checked((uint)(definition.Weights?.Length??0)),definition.KnotsU,checked((uint)definition.KnotsU.Length),definition.KnotsV,checked((uint)definition.KnotsV.Length),tolerance,out var id,diagnostic,DiagnosticCapacity);return(status,id);});
 }
}
internal static partial class NativeMethods
{
 [DllImport(Library,EntryPoint="jd_candidate_nurbs_surface",CallingConvention=CallingConvention.Cdecl,ExactSpelling=true)]
 internal static extern int NurbsSurface(ContextHandle context,uint degreeU,uint degreeV,uint countU,uint countV,uint rational,[In]double[] coordinates,uint coordinateCount,[In]double[]? weights,uint weightCount,[In]double[] knotsU,uint knotCountU,[In]double[] knotsV,uint knotCountV,double tolerance,out ulong output,[Out]byte[] diagnostic,uint capacity);
}
