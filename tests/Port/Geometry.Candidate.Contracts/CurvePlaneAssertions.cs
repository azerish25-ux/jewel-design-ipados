using Fallback=CandidateGeometryContractChecks.Recovered.AveragePlane;
using JewelDesign.Documents;
using JewelDesign.Features.Candidate;
using JewelDesign.Geometry.Candidate;
namespace CandidateGeometryContractChecks;
internal static class CurvePlaneAssertions
{
 static void Check(bool v,string message){if(!v)throw new Exception(message);}
 static double Dot(CandidateVector3 a,CandidateVector3 b)=>a.X*b.X+a.Y*b.Y+a.Z*b.Z;
 private static int comparisons;private static double maximumError;
 static bool Compared(CandidateContext context,CandidateShape curve,out CandidatePlane plane){
  comparisons++;bool actual=CurvePlaneCandidate.TryPlanar(context,curve,out plane),expected=NativePlaneReference.Try(context,curve,out var fields);Check(actual==expected,"Published planar decision differs");if(actual){var x=plane.XDirection;var z=plane.Normal;var y=new CandidateVector3(z.Y*x.Z-z.Z*x.Y,z.Z*x.X-z.X*x.Z,z.X*x.Y-z.Y*x.X);var values=new[]{plane.Origin.X,plane.Origin.Y,plane.Origin.Z,x.X,x.Y,x.Z,y.X,y.Y,y.Z,z.X,z.Y,z.Z};for(int i=0;i<12;i++){double delta=Math.Abs(values[i]-fields[i]);maximumError=Math.Max(maximumError,delta);Check(delta<=1e-10,"Published plane axis/origin differs at"+i+": "+values[i]+" / "+fields[i]+" comparison="+comparisons+" actual="+string.Join(",",values)+" expected="+string.Join(",",fields));}}return actual;
 }
 public static string Run(){comparisons=0;maximumError=0;using var context=CandidateContext.Create();int planes=0,boundary=0;
  foreach(var frame in new[]{CandidatePlane.WorldXY,CandidatePlane.WorldZX,new CandidatePlane(new(5,-4,3),new(0,.6,.8),new(1,0,0))}){
   using var circle=context.CreateCircle(2,frame);foreach(bool reverse in new[]{false,true}){using var curve=reverse?context.ReverseCurve(circle):context.Clone(circle);Check(Compared(context,curve,out var plane),"Circle planar test failed");Check(Dot(plane.Normal,frame.Normal)*(reverse?-1:1)>1-1e-10,"Circle normal orientation changed");var info=context.InspectCurve(curve);Check(Dot(plane.XDirection,context.TangentAt(curve,info.FirstParameter))>1-1e-10,"Plane X axis lost initial tangent");planes++;}
  }
  foreach(var end in new[]{new CandidatePoint3(1,0,0),new(0,1,0),new(0,0,1),new(1,2,3)}){using var line=context.CreateNurbs(new(1,new[]{new CandidatePoint3(0,0,0),end},new[]{0.0,0,1,1}));Check(Compared(context,line,out _),"Line plane failed");planes++;}
  foreach(double height in new[]{-.5,-.1,.1,.5})using(var curve=context.CreateNurbs(new(1,new[]{new CandidatePoint3(1,0,0),new(1,1,height),new(0,1,height*.5),new(-1,1,-height*.5),new(-1,0,0)},new[]{0.0,0,1,2,3,4,4}))){Check(!Compared(context,curve,out _),"Nonplanar curve accepted");Check(CurvePlaneCandidate.TryAveragePlane(context,curve,out var fallback),"Recovered fallback failed");Check(Fallback.SourceAverage.AveragePlaneOfCurve(new(context,curve),out var source),"Source fallback failed");Check(Math.Abs(fallback.Normal.X-source.Normal.X)<1e-12&&Math.Abs(fallback.Normal.Y-source.Normal.Y)<1e-12&&Math.Abs(fallback.Normal.Z-source.Normal.Z)<1e-12,"Fallback normal differs from source body");planes++;}
  using(var stream=typeof(CurvePlaneAssertions).Assembly.GetManifestResourceStream("RingRailLibraryFixture")!)using(var memory=new MemoryStream()){
   stream.CopyTo(memory);var document=LegacyCsg4Codec.Read(memory.ToArray());using var graph=new CandidateRailGraphSession(context,document,new(1e-7,1e-7,1e-7));
   void Run(CandidateRailGraphSession g){g.Generate(115,122);foreach(int index in new[]{115,122}){using var curve=g.RetainCurve(index);Compared(context,curve,out _);Check(CurvePlaneCandidate.TryAveragePlane(context,curve,out var plane),"Generated cap profile plane missing");using var cap=ProfileCapWithPlaneCandidate.Build(context,curve,plane,index==115?.5:-.5);var d=context.InspectCurve(curve);var s=context.InspectSurface(cap.Shape);for(int i=0;i<=32;i++){double t=i/32.0;var a=context.PointAt(curve,d.FirstParameter+(d.LastParameter-d.FirstParameter)*t);var b=context.PointAtSurface(cap.Shape,s.FirstU,s.FirstV+(s.LastV-s.FirstV)*t);Check(Math.Abs(a.X-b.X)<1e-8&&Math.Abs(a.Y-b.Y)<1e-8&&Math.Abs(a.Z-b.Z)<1e-8,"Actual cap profile boundary lost");boundary++;}planes++;}}
   Run(graph);graph.Values.SetValue(2,document.Records[2].Scalar!.Value+.25);Run(graph);using var reopened=new CandidateRailGraphSession(context,LegacyCsg4Codec.Read(LegacyCsg4Codec.Write(graph.Values.ExportCurrentDocument())),new(1e-7,1e-7,1e-7));Run(reopened);
  }

  var random=new Random(20261030);
  for(int index=0;index<1200;index++){
   double a=random.NextDouble()*6-3,b=random.NextDouble()*2-1;var x=new CandidateVector3(Math.Cos(a),0,-Math.Sin(a));var n=new CandidateVector3(Math.Sin(a)*Math.Cos(b),Math.Sin(b),Math.Cos(a)*Math.Cos(b));var y=new CandidateVector3(n.Y*x.Z-n.Z*x.Y,n.Z*x.X-n.X*x.Z,n.X*x.Y-n.Y*x.X);double offset=index%7==0?100000:10;var origin=new CandidatePoint3(offset*random.NextDouble(),-offset*random.NextDouble(),offset*random.NextDouble());
   var points=new CandidatePoint3[4];var weights=new double[4];for(int j=0;j<4;j++){double u=j+.2*random.NextDouble(),v=(random.NextDouble()*2-1)*(index%10==0?1e-8:1),w=index%4==0&&j==2?.01:0;points[j]=new(origin.X+x.X*u+y.X*v+n.X*w,origin.Y+x.Y*u+y.Y*v+n.Y*w,origin.Z+x.Z*u+y.Z*v+n.Z*w);weights[j]=.5+random.NextDouble();}
   var knots=index%3==0?new[]{-2.0,-1,0,1,2,3,4}:new[]{0.0,0,0,.4,1,1,1};using var curve=context.CreateNurbs(new(2,points,knots,weights));Compared(context,curve,out _);
  }

  foreach(int count in new[]{64,65,127,128,129,193,257}){
   var points=Enumerable.Range(0,count).Select(i=>{double t=1.5*Math.PI*i/(count-1),x=2*Math.Cos(t),y=3*Math.Sin(t);return new CandidatePoint3(10+x,-3+y,5+.3*x-.2*y);}).ToArray();var knots=Enumerable.Range(0,count+3).Select(i=>i<=2?0.0:i>=count?1.0:(i-2)/(double)(count-2)).ToArray();using var curve=context.CreateNurbs(new(2,points,knots));Compared(context,curve,out _);
  }
  using(var backwards=context.CreateNurbs(new(2,new[]{new CandidatePoint3(0,0,0),new(2,0,0),new(1,0,0),new(3,0,0)},new[]{0.0,0,0,.5,1,1,1})))Check(!Compared(context,backwards,out _),"Backtracking collinear source policy changed");
  Check(context.DiagnosticLiveShapeCount==0,"Plane/cap cases leaked native entries");return "PASS automatic cap plane candidate:"+planes+" oriented/linear/fallback/generated cases and"+boundary+" actual-profile cap boundary points; published-plane-comparisons="+comparisons+"; max-axis-error="+maximumError.ToString("R",System.Globalization.CultureInfo.InvariantCulture)+"; published6 policy, original-PC/full graph parity separate.";
 }
}
