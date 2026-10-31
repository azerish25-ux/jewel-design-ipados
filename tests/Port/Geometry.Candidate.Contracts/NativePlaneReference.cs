using System.Runtime.InteropServices;
using JewelDesign.Geometry.Candidate;
namespace CandidateGeometryContractChecks;
internal static class NativePlaneReference
{
 [DllImport("__Internal",EntryPoint="jd_test_nurbs_plane",CallingConvention=CallingConvention.Cdecl,ExactSpelling=true)]
 private static extern int Query([In]double[] coordinates,uint coordinateCount,uint degree,[In]double[] knots,uint knotCount,[In]double[] endData,uint endCount,[Out]double[] output,uint outputCount);
 internal static bool Try(CandidateContext context,CandidateShape curve,out double[] result){
  var definition=context.ExportNurbs(curve);var d=context.InspectCurve(curve);var a=context.PointAt(curve,d.FirstParameter);var b=context.PointAt(curve,d.LastParameter);var tangent=context.TangentAt(curve,d.FirstParameter);
  var points=definition.Poles.SelectMany(p=>new[]{p.X,p.Y,p.Z}).ToArray();var knots=definition.KnotVector.ToArray();result=new double[12];var status=Query(points,(uint)points.Length,definition.Degree,knots,(uint)knots.Length,new[]{a.X,a.Y,a.Z,b.X,b.Y,b.Z,tangent.X,tangent.Y,tangent.Z},9,result,12);if(status>1)throw new Exception("Native plane reference input/shim failure: "+status);return status==0;
 }
}
