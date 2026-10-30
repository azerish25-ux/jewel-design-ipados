using JewelDesign.Geometry.Candidate;
namespace CandidateGeometryContractChecks.Recovered.DomeProfile;
internal readonly record struct Point3d(double X,double Y,double Z){internal static Point3d Origin=>new(0,0,0);}
internal readonly record struct Point4d(double X,double Y,double Z,double W);
internal sealed class PointList{internal readonly CandidatePoint3[] Points;internal readonly double[] Weights;internal PointList(int count){Points=new CandidatePoint3[count];Weights=Enumerable.Repeat(1.0,count).ToArray();}internal void SetPoint(int i,Point4d p){Points[i]=new(p.X,p.Y,p.Z);Weights[i]=p.W;}}
internal sealed class NurbsCurve{internal readonly int Order;internal readonly bool Rational;internal PointList Points{get;}internal double[] Knots{get;}internal NurbsCurve(int dimension,bool rational,int order,int count){Order=order;Rational=rational;Points=new(count);Knots=new double[count+order-2];}internal CandidateNurbsDefinition Definition(){var full=new double[Knots.Length+2];full[0]=Knots[0];full[^1]=Knots[^1];System.Array.Copy(Knots,0,full,1,Knots.Length);return new((uint)(Order-1),Points.Points,full,Rational?Points.Weights:null);}}
internal sealed class LineCurve{private readonly Point3d a,b;internal LineCurve(Point3d a,Point3d b){this.a=a;this.b=b;}internal NurbsCurve ToNurbsCurve(){var c=new NurbsCurve(3,false,2,2);c.Points.SetPoint(0,new(a.X,a.Y,a.Z,1));c.Points.SetPoint(1,new(b.X,b.Y,b.Z,1));double length=Math.Sqrt((a.X-b.X)*(a.X-b.X)+(a.Y-b.Y)*(a.Y-b.Y)+(a.Z-b.Z)*(a.Z-b.Z));c.Knots[0]=0;c.Knots[1]=length<=2.3283064365386962890625e-10?1:length;return c;}}
internal static class SourceDome{
public static NurbsCurve CreateDomeRC(double domeAmount, double radius = 0.5)
	{
		if (Math.Abs(domeAmount) < 0.005)
		{
			return new LineCurve(new Point3d(radius, 0.0, 0.0), Point3d.Origin).ToNurbsCurve();
		}
		NurbsCurve nurbsCurve = new NurbsCurve(3, rational: true, 3, 5);
		nurbsCurve.Points.SetPoint(0, new Point4d(radius, 0.0, 0.0, 1.0));
		nurbsCurve.Points.SetPoint(1, new Point4d(radius, 0.0, domeAmount, 0.70710678));
		nurbsCurve.Points.SetPoint(2, new Point4d(radius / 2.0, 0.0, domeAmount, 1.0));
		nurbsCurve.Points.SetPoint(3, new Point4d(radius / 3.0, 0.0, domeAmount, 1.0));
		nurbsCurve.Points.SetPoint(4, new Point4d(0.0, 0.0, domeAmount, 1.0));
		nurbsCurve.Knots[0] = 0.0;
		nurbsCurve.Knots[1] = 0.0;
		nurbsCurve.Knots[2] = 1.0;
		nurbsCurve.Knots[3] = 1.0;
		nurbsCurve.Knots[4] = 2.0;
		nurbsCurve.Knots[5] = 2.0;
		return nurbsCurve;
	}
}
