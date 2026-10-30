using JewelDesign.Geometry.Candidate;
namespace CandidateGeometryContractChecks;
internal static class NurbsSurfaceAssertions
{
 private static void Check(bool v,string m){if(!v)throw new Exception(m);}
 private static void Near(CandidatePoint3 a,CandidatePoint3 b){Check(double.IsFinite(a.X)&&double.IsFinite(a.Y)&&double.IsFinite(a.Z)&&Math.Abs(a.X-b.X)<=1e-8&&Math.Abs(a.Y-b.Y)<=1e-8&&Math.Abs(a.Z-b.Z)<=1e-8,"Native tensor surface differs from independent homogeneous evaluation");}
 private static void Throws<T>(Action f)where T:Exception{try{f();}catch(T){return;}throw new Exception("Expected "+typeof(T).Name);}
 private static double[] Curve(double[][] values,int degree,double[] knots,double t){int n=values.Length-1,span=degree;if(t>=knots[n+1])span=n;else while(span<n&&t>=knots[span+1])span++;var d=new double[degree+1][];for(int j=0;j<=degree;j++)d[j]=(double[])values[span-degree+j].Clone();for(int r=1;r<=degree;r++)for(int j=degree;j>=r;j--){int k=span-degree+j;double alpha=(t-knots[k])/(knots[k+degree-r+1]-knots[k]);for(int c=0;c<4;c++)d[j][c]=(1-alpha)*d[j-1][c]+alpha*d[j][c];}return d[degree];}
 private static CandidatePoint3 Evaluate(CandidatePoint3[] points,double[]? weights,int nu,int nv,int du,int dv,double[] ku,double[] kv,double u,double v){var rows=new double[nu][];for(int i=0;i<nu;i++){var row=new double[nv][];for(int j=0;j<nv;j++){int k=i*nv+j;double w=weights?[k]??1;row[j]=new[]{points[k].X*w,points[k].Y*w,points[k].Z*w,w};}rows[i]=Curve(row,dv,kv,v);}var p=Curve(rows,du,ku,u);return new(p[0]/p[3],p[1]/p[3],p[2]/p[3]);}
 public static string Run(){
  using var context=CandidateContext.Create();int samples=0;
  void Case(int nu,int nv,int du,int dv,CandidatePoint3[] p,double[] ku,double[] kv,double[]? w=null){
   var original=p.ToArray();var knotsU=ku.ToArray();var knotsV=kv.ToArray();var weights=w?.ToArray();
   var definition=new CandidateNurbsSurfaceDefinition((uint)du,(uint)dv,(uint)nu,(uint)nv,p,ku,kv,w);
   p[0]=new(999,999,999);ku[0]=-999;if(w is not null)w[0]=99;
   using var face=context.CreateNurbsSurface(definition,1e-9);Check(context.Inspect(face).IsValid,"Tensor NURBS face invalid");var bounds=context.InspectSurface(face);Check(Math.Abs(bounds.FirstU-knotsU[du])<1e-12&&Math.Abs(bounds.LastU-knotsU[nu])<1e-12&&Math.Abs(bounds.FirstV-knotsV[dv])<1e-12&&Math.Abs(bounds.LastV-knotsV[nv])<1e-12,"U/V active domains changed");
   for(int i=0;i<=16;i++)for(int j=0;j<=16;j++){double u=knotsU[du]+(knotsU[nu]-knotsU[du])*i/16,v=knotsV[dv]+(knotsV[nv]-knotsV[dv])*j/16;Near(context.PointAtSurface(face,u,v),Evaluate(original,weights,nu,nv,du,dv,knotsU,knotsV,u,v));samples++;}
   using var moved=context.Translate(face,1,-2,3);double x=(bounds.FirstU+bounds.LastU)*.5,y=(bounds.FirstV+bounds.LastV)*.5;var a=context.PointAtSurface(face,x,y);Near(context.PointAtSurface(moved,x,y),new(a.X+1,a.Y-2,a.Z+3));
  }
  Case(2,2,1,1,new[]{new CandidatePoint3(0,0,0),new(0,3,1),new(2,0,0),new(2,3,2)},new[]{2.0,2,5,5},new[]{-1.0,-1,3,3});
  Case(2,3,1,2,new[]{new CandidatePoint3(0,2,0),new(0,2,2),new(0,0,2),new(10,2,0),new(10,2,2),new(10,0,2)},new[]{0.0,0,1,1},new[]{0.0,0,0,1,1,1},new[]{1.0,Math.Sqrt(.5),1,1,Math.Sqrt(.5),1});
  var points=new CandidatePoint3[12];var weights=new double[12];for(int i=0;i<3;i++)for(int j=0;j<4;j++){int k=i*4+j;points[k]=new(i,j,.1*i*j+.04*j*j);weights[k]=.5+.1*i+.07*j;}
  Case(3,4,2,2,points,new[]{0.0,0,0,1,1,1},new[]{-2.0,-2,-2,.5,3,3,3},weights);
  var unclamped=new CandidatePoint3[8];for(int i=0;i<4;i++)for(int j=0;j<2;j++)unclamped[i*2+j]=new(i,j,.2*i*i+.1*j);
  Case(4,2,2,1,unclamped,new[]{-2.0,-1,0,1,2,3,4},new[]{0.0,0,1,1});
  Case(2,3,1,2,new[]{new CandidatePoint3(2,0,0),new(2,2,0),new(0,2,0),new(0,0,3),new(0,0,3),new(0,0,3)},new[]{0.0,0,1,1},new[]{0.0,0,0,1,1,1},new[]{1.0,Math.Sqrt(.5),1,1,Math.Sqrt(.5),1});
  Check(context.DiagnosticLiveShapeCount==0,"Tensor surface cases leaked entries");
  var p2=new[]{new CandidatePoint3(0,0,0),new(0,1,0),new(1,0,0),new(1,1,0)};var k2=new[]{0.0,0,1,1};
  Throws<CandidateGeometryException>(()=>context.CreateNurbsSurface(new(1,1,2,2,p2,k2,k2,new[]{1.0,-1,1,1}),1e-9));
  Throws<CandidateGeometryException>(()=>context.CreateNurbsSurface(new(1,1,2,3,p2,k2,k2),1e-9));
  Throws<CandidateGeometryException>(()=>context.CreateNurbsSurface(new(1,1,2,2,p2,new[]{0.0,0,-1,1},k2),1e-9));
  Throws<CandidateGeometryException>(()=>context.CreateNurbsSurface(new(1,1,2,2,p2,k2,k2),double.NaN));
  Throws<CandidateGeometryException>(()=>context.CreateNurbsSurface(new(1,1,2,2,new[]{new CandidatePoint3(double.NaN,0,0),p2[1],p2[2],p2[3]},k2,k2),1e-9));
  Check(context.DiagnosticLiveShapeCount==0,"Invalid tensor surface inputs leaked entries");
  return "PASS rational NURBS surfaces:"+samples+" independent homogeneous tensor points, U-major/rational/nonuniform/nonclamped active domains/collapsed apex, immutable definitions, native face/affine/error/disposal; cap algorithm/PC parity separate.";
 }
}
