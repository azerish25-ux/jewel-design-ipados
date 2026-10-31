using JewelDesign.Geometry.Candidate;
namespace JewelDesign.Rendering.Candidate;
/// <summary>Owned GPU upload data centered/scaled in double before float conversion. Original coordinates remain in the CAD snapshot.</summary>
public sealed class CadViewportMesh
{
 private readonly float[] vertices;private readonly uint[] indices,faceOrdinals;
 public CandidatePoint3 Center {get;}public double Radius {get;}public int VertexCount=>vertices.Length/6;public int TriangleCount=>indices.Length/3;
 private CadViewportMesh(float[] vertices,uint[] indices,uint[] faces,CandidatePoint3 center,double radius){this.vertices=vertices;this.indices=indices;faceOrdinals=faces;Center=center;Radius=radius;}
 public float[] CopyInterleavedVertices()=>(float[])vertices.Clone();
 public uint[] CopyIndices()=>(uint[])indices.Clone();
 public uint[] CopyTriangleFaceOrdinals()=>(uint[])faceOrdinals.Clone();
 public static CadViewportMesh FromSnapshot(CandidateMeshSnapshot mesh)
 {
  ArgumentNullException.ThrowIfNull(mesh);if(mesh.Vertices.Count==0)return new(Array.Empty<float>(),Array.Empty<uint>(),Array.Empty<uint>(),new(0,0,0),1);
  var center=new CandidatePoint3(mesh.Vertices.Min(v=>v.Position.X)*.5+mesh.Vertices.Max(v=>v.Position.X)*.5,mesh.Vertices.Min(v=>v.Position.Y)*.5+mesh.Vertices.Max(v=>v.Position.Y)*.5,mesh.Vertices.Min(v=>v.Position.Z)*.5+mesh.Vertices.Max(v=>v.Position.Z)*.5);
  double extent=0;foreach(var vertex in mesh.Vertices){var p=vertex.Position;extent=Math.Max(extent,Math.Max(Math.Abs(p.X-center.X),Math.Max(Math.Abs(p.Y-center.Y),Math.Abs(p.Z-center.Z))));}
  if(!double.IsFinite(extent)||extent<=0)throw new NotSupportedException("Degenerate/nonfinite viewport extent.");
  double radius=0;foreach(var vertex in mesh.Vertices){var p=vertex.Position;double x=(p.X-center.X)/extent,y=(p.Y-center.Y)/extent,z=(p.Z-center.Z)/extent;radius=Math.Max(radius,Math.Sqrt(x*x+y*y+z*z));}radius*=extent;
  if(!double.IsFinite(radius)||radius<=0)throw new NotSupportedException("Viewport radius is not representable.");
  var data=new float[checked(mesh.Vertices.Count*6)];for(int i=0;i<mesh.Vertices.Count;i++){var v=mesh.Vertices[i];data[6*i]=(float)((v.Position.X-center.X)/radius);data[6*i+1]=(float)((v.Position.Y-center.Y)/radius);data[6*i+2]=(float)((v.Position.Z-center.Z)/radius);data[6*i+3]=(float)v.Normal.X;data[6*i+4]=(float)v.Normal.Y;data[6*i+5]=(float)v.Normal.Z;}
  if(data.Any(v=>!float.IsFinite(v)))throw new NotSupportedException("Viewport buffer contains nonfinite data.");
  var indices=new uint[checked(mesh.Triangles.Count*3)];var faces=new uint[mesh.Triangles.Count];for(int i=0;i<mesh.Triangles.Count;i++){var t=mesh.Triangles[i];indices[3*i]=t.A;indices[3*i+1]=t.B;indices[3*i+2]=t.C;faces[i]=t.Face;}
  return new(data,indices,faces,center,radius);
 }
}
