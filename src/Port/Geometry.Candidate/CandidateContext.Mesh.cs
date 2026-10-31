using System.Runtime.InteropServices;
namespace JewelDesign.Geometry.Candidate;
[StructLayout(LayoutKind.Sequential)]
public readonly record struct CandidateMeshVertex(CandidatePoint3 Position,CandidateVector3 Normal);
[StructLayout(LayoutKind.Sequential)]
public readonly record struct CandidateMeshTriangle(uint A,uint B,uint C,uint Face);
/// <summary>Detached immutable native tessellation. Face ordinals are snapshot-local, not original project identities.</summary>
public sealed class CandidateMeshSnapshot
{
 public IReadOnlyList<CandidateMeshVertex> Vertices {get;}public IReadOnlyList<CandidateMeshTriangle> Triangles {get;}
 public uint FaceCount {get;}public uint NativeStatusFlags {get;}public double RequestedDeflection {get;}public double RequestedAngle {get;}public double MaximumReportedDeflection {get;}
 internal CandidateMeshSnapshot(CandidateMeshVertex[] vertices,CandidateMeshTriangle[] triangles,uint faces,uint flags,double deflection,double angle,double maximum){Vertices=Array.AsReadOnly(vertices);Triangles=Array.AsReadOnly(triangles);FaceCount=faces;NativeStatusFlags=flags;RequestedDeflection=deflection;RequestedAngle=angle;MaximumReportedDeflection=maximum;}
}
public sealed partial class CandidateContext
{
 /// <summary>Mesh an actual deep BRep copy with absolute chord/radian angle; does not use project previews.</summary>
 public CandidateMeshSnapshot TessellateBrep(CandidateShape shape,double deflection,double angle)
 {
  lock(_gate){
   ThrowIfDisposed();Validate(shape);if(Marshal.SizeOf<CandidateMeshVertex>()!=48||Marshal.SizeOf<CandidateMeshTriangle>()!=16||Marshal.SizeOf<NativeMeshInfo>()!=24)throw new PlatformNotSupportedException("Unsupported native mesh layout.");
   var diagnostic=new byte[DiagnosticCapacity];Check(NativeMethods.Mesh(_handle,shape.Handle,deflection,angle,null,0,null,0,out var info,diagnostic,DiagnosticCapacity),diagnostic);
   if(info.Vertices>1000000||info.Triangles>2000000||info.Faces>65536||!double.IsFinite(info.Maximum)||info.Maximum<0||(info.Vertices==0)!=(info.Triangles==0)||(info.Faces==0)!=(info.Vertices==0))throw new InvalidOperationException("Invalid native mesh counts/deflection.");
   if(info.Vertices==0)return new(Array.Empty<CandidateMeshVertex>(),Array.Empty<CandidateMeshTriangle>(),0,info.Flags,deflection,angle,info.Maximum);
   var vertices=new CandidateMeshVertex[checked((int)info.Vertices)];var triangles=new CandidateMeshTriangle[checked((int)info.Triangles)];Check(NativeMethods.Mesh(_handle,shape.Handle,deflection,angle,vertices,info.Vertices,triangles,info.Triangles,out var filled,diagnostic,DiagnosticCapacity),diagnostic);
   if(filled.Vertices!=info.Vertices||filled.Triangles!=info.Triangles||filled.Faces!=info.Faces||filled.Flags!=info.Flags)throw new InvalidOperationException("Native mesh dimensions/status changed between passes.");
   foreach(var vertex in vertices){var p=vertex.Position;var n=vertex.Normal;double norm=n.X*n.X+n.Y*n.Y+n.Z*n.Z;if(!double.IsFinite(p.X)||!double.IsFinite(p.Y)||!double.IsFinite(p.Z)||!double.IsFinite(norm)||Math.Abs(norm-1)>1e-10)throw new InvalidOperationException("Invalid native mesh position/normal.");}
   foreach(var t in triangles)if(t.A>=info.Vertices||t.B>=info.Vertices||t.C>=info.Vertices||t.Face>=info.Faces)throw new InvalidOperationException("Invalid native mesh indices.");if(!double.IsFinite(filled.Maximum)||filled.Maximum<0)throw new InvalidOperationException("Invalid filled mesh deflection.");return new(vertices,triangles,info.Faces,info.Flags,deflection,angle,filled.Maximum);
  }
 }
}
[StructLayout(LayoutKind.Sequential)]
internal struct NativeMeshInfo {internal uint Vertices,Triangles,Faces,Flags;internal double Maximum;}
internal static partial class NativeMethods
{
 [DllImport(Library,EntryPoint="jd_candidate_mesh",CallingConvention=CallingConvention.Cdecl,ExactSpelling=true)]
 internal static extern int Mesh(ContextHandle context,ShapeHandle shape,double deflection,double angle,[Out]CandidateMeshVertex[]? vertices,uint vertexCapacity,[Out]CandidateMeshTriangle[]? triangles,uint triangleCapacity,out NativeMeshInfo info,[Out]byte[] diagnostic,uint capacity);
}
