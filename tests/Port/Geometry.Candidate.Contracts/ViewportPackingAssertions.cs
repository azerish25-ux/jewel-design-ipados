using System.Numerics;
using JewelDesign.Geometry.Candidate;
using JewelDesign.Rendering.Candidate;
namespace CandidateGeometryContractChecks;
internal static class ViewportPackingAssertions
{
 static void Check(bool value,string message){if(!value)throw new Exception(message);}
 static void Throws<T>(Action action)where T:Exception{try{action();}catch(T){return;}throw new Exception("Expected "+typeof(T).Name);}
 public static string Run(){using var context=CandidateContext.Create();using var box=context.CreateBox(2,3,4);using var far=context.Translate(box,1e9,-1e9,1e9);var mesh=context.TessellateBrep(far,.05,.35);var upload=CadViewportMesh.FromSnapshot(mesh);var vertices=upload.CopyInterleavedVertices();var indices=upload.CopyIndices();Check(vertices.Length==mesh.Vertices.Count*6&&indices.Length==mesh.Triangles.Count*3,"GPU mesh stride/index count differs");Check(Math.Abs(upload.Center.X-(1e9+1))<1e-7&&Math.Abs(upload.Center.Y-(-1e9+1.5))<1e-7&&Math.Abs(upload.Center.Z-(1e9+2))<1e-7,"Double center precision lost");
  for(int i=0;i<mesh.Vertices.Count;i++){var p=mesh.Vertices[i].Position;Check(Math.Abs(vertices[i*6]-(p.X-upload.Center.X)/upload.Radius)<1e-7&&Math.Abs(vertices[i*6+1]-(p.Y-upload.Center.Y)/upload.Radius)<1e-7&&Math.Abs(vertices[i*6+2]-(p.Z-upload.Center.Z)/upload.Radius)<1e-7,"Large-world coordinates were cast before centering");Check(Math.Abs(vertices[i*6+3]-mesh.Vertices[i].Normal.X)<1e-7,"GPU normal layout differs");}
  var faceIds=upload.CopyTriangleFaceOrdinals();for(int i=0;i<mesh.Triangles.Count;i++)Check(indices[3*i]==mesh.Triangles[i].A&&faceIds[i]==mesh.Triangles[i].Face,"Mesh-local selection identity/order lost");float original=vertices[0];vertices[0]=99;Check(upload.CopyInterleavedVertices()[0]==original,"GPU upload snapshot exposes mutable storage");int clips=0;
  foreach(var size in new[]{(320,320),(320,640),(640,320)})foreach(float yaw in new[]{-.7f,.6f}){var matrix=CadViewportCamera.Fit(size.Item1,size.Item2,yaw,.35f);var data=upload.CopyInterleavedVertices();for(int i=0;i<upload.VertexCount;i++){var p=Vector4.Transform(new Vector4(data[i*6],data[i*6+1],data[i*6+2],1),matrix);Check(float.IsFinite(p.W)&&p.W>0&&Math.Abs(p.X/p.W)<1&&Math.Abs(p.Y/p.W)<1&&p.Z/p.W>=0&&p.Z/p.W<=1,"Fitted Metal camera clipped normalized CAD bounds");clips++;}}
  Throws<ArgumentOutOfRangeException>(()=>CadViewportCamera.Fit(0,320,0,0));Throws<ArgumentOutOfRangeException>(()=>CadViewportCamera.Fit(320,320,float.NaN,0));Throws<ArgumentOutOfRangeException>(()=>CadViewportCamera.Fit(320,320,0,1.6f));Check(context.DiagnosticLiveShapeCount==2,"GPU packing unexpectedly retained native entries");return "PASS viewport upload/camera:"+clips+" clip points, large-world double centering, float6/index/face identity and owned buffers; Metal raster/UI/source-camera parity separate.";
 }
}
