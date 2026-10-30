using JewelDesign.Geometry.Candidate;
namespace CandidateGeometryContractChecks.Recovered.AveragePlane;
internal readonly record struct Point3d(double X,double Y,double Z){public static Vector3d operator-(Point3d a,Point3d b)=>new(a.X-b.X,a.Y-b.Y,a.Z-b.Z);}
internal struct Vector3d{internal double X,Y,Z;internal Vector3d(double x,double y,double z){X=x;Y=y;Z=z;}public static Vector3d operator+(Vector3d a,Vector3d b)=>new(a.X+b.X,a.Y+b.Y,a.Z+b.Z);public static Vector3d operator/(Vector3d a,double b)=>new(a.X/b,a.Y/b,a.Z/b);public static double operator*(Vector3d a,Vector3d b)=>a.X*b.X+a.Y*b.Y+a.Z*b.Z;internal static Vector3d CrossProduct(Vector3d a,Vector3d b)=>new(a.Y*b.Z-a.Z*b.Y,a.Z*b.X-a.X*b.Z,a.X*b.Y-a.Y*b.X);internal bool Unitize(){double scale=Math.Max(Math.Abs(X),Math.Max(Math.Abs(Y),Math.Abs(Z)));if(scale==0)return false;double x=X/scale,y=Y/scale,z=Z/scale,n=Math.Sqrt(x*x+y*y+z*z);X=x/n;Y=y/n;Z=z/n;return true;}}
internal readonly struct Plane{internal readonly Point3d Origin;internal readonly Vector3d Normal;internal Plane(Point3d p,Vector3d normal){normal.Unitize();Origin=p;Normal=normal;}}
internal sealed class Curve{private readonly CandidateContext context;private readonly CandidateShape shape;internal Curve(CandidateContext context,CandidateShape shape){this.context=context;this.shape=shape;}internal bool TryGetPlane(out Plane p){p=default;return false;}internal Point3d PointAtStart{get{var d=context.InspectCurve(shape);var p=context.PointAt(shape,d.FirstParameter);return new(p.X,p.Y,p.Z);}}internal Point3d PointAtNormalizedLength(double fraction){var p=context.PointAt(shape,context.ParameterAtNormalizedLength(shape,fraction,1e-9));return new(p.X,p.Y,p.Z);}}
internal static class SourceAverage{
public static bool AveragePlaneOfCurve(Curve curve, out Plane plane)
	{
		if (curve.TryGetPlane(out plane))
		{
			return true;
		}
		Point3d pointAtStart = curve.PointAtStart;
		double[] array = new double[5] { 0.25, 0.33, 0.5, 0.66, 0.75 };
		List<Point3d> list = array.Select(curve.PointAtNormalizedLength).ToList();
		Vector3d normal = new Vector3d(0.0, 0.0, 0.0);
		int num = 0;
		for (int i = 0; i < array.Length - 1; i++)
		{
			Vector3d vector3d = list[i] - pointAtStart;
			vector3d.Unitize();
			Vector3d vector3d2 = list[i + 1] - pointAtStart;
			vector3d2.Unitize();
			double num2 = vector3d * vector3d2;
			if (!(num2 < 0.01) && !(num2 > 3.14))
			{
				Vector3d vector3d3 = Vector3d.CrossProduct(vector3d, vector3d2);
				vector3d3.Unitize();
				normal += vector3d3;
				num++;
			}
		}
		if (num == 0)
		{
			return false;
		}
		normal /= (double)num;
		plane = new Plane(pointAtStart, normal);
		return true;
	}
}
