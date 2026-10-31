using System.Numerics;
namespace JewelDesign.Rendering.Candidate;
/// <summary>Fitted candidate camera for a unit bounding sphere; not asserted original CounterSketch default camera behavior.</summary>
public static class CadViewportCamera
{
 public static Matrix4x4 Fit(int width,int height,float yaw,float pitch)
 {
  if(width<1||height<1||width>4096||height>4096||!float.IsFinite(yaw)||!float.IsFinite(pitch)||Math.Abs(pitch)>=1.55)throw new ArgumentOutOfRangeException(nameof(width),"Invalid bounded viewport/camera pose.");
  float aspect=width/(float)height,fov=MathF.PI/4;float limiting=MathF.Min(fov/2,MathF.Atan(MathF.Tan(fov/2)*aspect)),distance=1.2f/MathF.Sin(limiting);
  var eye=distance*new Vector3(MathF.Cos(pitch)*MathF.Sin(yaw),MathF.Sin(pitch),MathF.Cos(pitch)*MathF.Cos(yaw));
  // System.Numerics row-vector memory is interpreted as its transpose by Metal column-major multiplication.
  return Matrix4x4.CreateLookAt(eye,Vector3.Zero,Vector3.UnitY)*Matrix4x4.CreatePerspectiveFieldOfView(fov,aspect,.01f,distance*10);
 }
 public static float[] MatrixScalars(Matrix4x4 m)=>new[]{m.M11,m.M12,m.M13,m.M14,m.M21,m.M22,m.M23,m.M24,m.M31,m.M32,m.M33,m.M34,m.M41,m.M42,m.M43,m.M44};
}
