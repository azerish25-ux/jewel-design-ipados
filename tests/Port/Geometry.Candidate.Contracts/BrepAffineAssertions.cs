using JewelDesign.Documents;
using JewelDesign.Features.Candidate;
using JewelDesign.Geometry.Candidate;
namespace CandidateGeometryContractChecks;
internal static class BrepAffineAssertions
{
 static void Check(bool value,string message){if(!value)throw new Exception(message);}
 static CandidateAffine Native(LegacyAffineTransform m)=>new(m.M11,m.M12,m.M13,m.M14,m.M21,m.M22,m.M23,m.M24,m.M31,m.M32,m.M33,m.M34);
 static CandidatePoint3 Apply(CandidateAffine m,CandidatePoint3 p)=>new(m.M11*p.X+m.M12*p.Y+m.M13*p.Z+m.M14,m.M21*p.X+m.M22*p.Y+m.M23*p.Z+m.M24,m.M31*p.X+m.M32*p.Y+m.M33*p.Z+m.M34);
 static double Determinant(CandidateAffine m)=>m.M11*(m.M22*m.M33-m.M23*m.M32)-m.M12*(m.M21*m.M33-m.M23*m.M31)+m.M13*(m.M21*m.M32-m.M22*m.M31);
 public static string Run(){using var context=CandidateContext.Create();int cases=0,points=0;
  void FaceCase(CandidateShape face,CandidateAffine matrix){var d=context.InspectSurface(face);var before=context.GetShapeBounds(face,CandidateShapeBoundsMode.Accurate);using var moved=context.TransformAffine(face,matrix);Check(context.Inspect(moved).IsValid,"Affine collapsed-apex face invalid");Check(context.InspectSurface(moved)==d,"Affine surface parameter domain changed");for(int i=0;i<=4;i++)for(int j=0;j<=4;j++){double u=d.FirstU+(d.LastU-d.FirstU)*i/4,v=d.FirstV+(d.LastV-d.FirstV)*j/4;var expected=Apply(matrix,context.PointAtSurface(face,u,v));var actual=context.PointAtSurface(moved,u,v);Check(Math.Abs(actual.X-expected.X)<1e-8&&Math.Abs(actual.Y-expected.Y)<1e-8&&Math.Abs(actual.Z-expected.Z)<1e-8,"Native affine changed requested surface point mapping");points++;}Check(context.GetShapeBounds(face,CandidateShapeBoundsMode.Accurate)==before,"Affine mutated source BRep/tolerances");cases++;}
  double s=Math.Sin(.37),c=Math.Cos(.37);var transforms=new[]{CandidateAffine.Identity,new CandidateAffine(c,-s,0,1,s,c,0,-2,0,0,1,.5),new CandidateAffine(2*c,-2*s,0,1,2*s,2*c,0,-2,0,0,2,.5),new CandidateAffine(1.2,0,0,1,0,.8,0,-2,0,0,1.5,.5),new CandidateAffine(1,.2,.1,1,0,1,.15,-2,0,0,1,.5),new CandidateAffine(-1,0,0,1,0,1,0,-2,0,0,1,.5)};
  using(var stream=typeof(BrepAffineAssertions).Assembly.GetManifestResourceStream("RingRailLibraryFixture")!)using(var memory=new MemoryStream()){
   stream.CopyTo(memory);var document=LegacyCsg4Codec.Read(memory.ToArray());using var graph=new CandidateRailGraphSession(context,document,new(1e-7,1e-7,1e-7));graph.Generate(140);
   foreach(int index in new[]{138,139}){
    using var source=graph.RetainCurve(index==138?115:122);Check(CurvePlaneCandidate.TryAveragePlane(context,source,out var plane),"Actual cap plane missing");var x=plane.XDirection;var n=plane.Normal;var p=plane.Origin;var y=new CandidateVector3(n.Y*x.Z-n.Z*x.Y,n.Z*x.X-n.X*x.Z,n.X*x.Y-n.Y*x.X);using var inPlane=context.TransformAffine(source,new(x.X,x.Y,x.Z,-(x.X*p.X+x.Y*p.Y+x.Z*p.Z),y.X,y.Y,y.Z,-(y.X*p.X+y.Y*p.Y+y.Z*p.Z),n.X,n.Y,n.Z,-(n.X*p.X+n.Y*p.Y+n.Z*p.Z)));var poles=context.ExportNurbs(inPlane).Poles;double cx=(poles.Min(q=>q.X)+poles.Max(q=>q.X))*.5,cy=(poles.Min(q=>q.Y)+poles.Max(q=>q.Y))*.5;using var centered=context.TransformAffine(inPlane,new(1,0,0,-cx,0,1,0,-cy,0,0,1,0));var tensor=RailRevolveTensorCandidate.Build(context,RailRevolveTensorCandidate.CreateDomeRhino5(index==138?.3:-.3),context.ExportNurbs(centered),0,true);using var local=context.CreateNurbsSurface(tensor,1e-7);using var snapshot=graph.CaptureOutput(index);FaceCase(local,Native(snapshot.ReadTransform(snapshot.Value)));
    using var world=graph.RetainBrep(index);foreach(var matrix in transforms)FaceCase(world,matrix);
   }
   using var joined=graph.RetainBrep(140);double volume=context.Inspect(joined).Volume;var bounds=context.GetShapeBounds(joined,CandidateShapeBoundsMode.Accurate);foreach(var matrix in transforms){using var moved=context.TransformAffine(joined,matrix);Check(context.Inspect(moved).IsValid,"Affine joined shell invalid");Check(Math.Abs(Math.Abs(context.Inspect(moved).Volume)-Math.Abs(volume*Determinant(matrix)))<1e-6,"Affine closed volume violates determinant law");cases++;}Check(context.GetShapeBounds(joined,CandidateShapeBoundsMode.Accurate)==bounds,"Affine mutated joined input");
  }
  Check(context.DiagnosticLiveShapeCount==0,"BRep affine tests leaked native entries");return "PASS collapsed-apex BRep affine:"+cases+" native cases and"+points+" coefficient-mapped points; actual local/world caps and joined design, rigid/uniform/nonuniform/shear/reflection, volume/source/ownership; original-PC/full affine-family parity separate.";
 }
}
