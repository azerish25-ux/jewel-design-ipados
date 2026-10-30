using System;
using GemvisionGeometry.Geometry;

namespace OctTree;

internal static class TriangleBoxIntersector
{
	private static void Findminmax(double x0, double x1, double x2, out double min, out double max)
	{
		min = (max = x0);
		if (x1 < min)
		{
			min = x1;
		}
		if (x1 > max)
		{
			max = x1;
		}
		if (x2 < min)
		{
			min = x2;
		}
		if (x2 > max)
		{
			max = x2;
		}
	}

	private static bool PlaneBoxOverlap(GVector3D normal, GVector3D vert, double maxbox)
	{
		GVector3D gVector3D = new GVector3D(0.0, 0.0, 0.0);
		GVector3D gVector3D2 = new GVector3D(0.0, 0.0, 0.0);
		if (normal.X > 0.0)
		{
			gVector3D.X = 0.0 - maxbox - vert.X;
			gVector3D2.X = maxbox - vert.X;
		}
		else
		{
			gVector3D.X = maxbox - vert.X;
			gVector3D2.X = 0.0 - maxbox - vert.X;
		}
		if (normal.Y > 0.0)
		{
			gVector3D.Y = 0.0 - maxbox - vert.Y;
			gVector3D2.Y = maxbox - vert.Y;
		}
		else
		{
			gVector3D.Y = maxbox - vert.Y;
			gVector3D2.Y = 0.0 - maxbox - vert.Y;
		}
		if (normal.Z > 0.0)
		{
			gVector3D.Z = 0.0 - maxbox - vert.Z;
			gVector3D2.Z = maxbox - vert.Z;
		}
		else
		{
			gVector3D.Z = maxbox - vert.Z;
			gVector3D2.Z = 0.0 - maxbox - vert.Z;
		}
		if (normal * gVector3D > 0.0)
		{
			return false;
		}
		if (normal * gVector3D2 >= 0.0)
		{
			return true;
		}
		return false;
	}

	private static bool AXISTEST_X01(double a, double b, double fa, double fb, double boxhalfsize, GVector3D v0, GVector3D v2)
	{
		double num = a * v0.Y - b * v0.Z;
		double num2 = a * v2.Y - b * v2.Z;
		double num3;
		double num4;
		if (num < num2)
		{
			num3 = num;
			num4 = num2;
		}
		else
		{
			num3 = num2;
			num4 = num;
		}
		double num5 = fa * boxhalfsize + fb * boxhalfsize;
		if (!(num3 > num5))
		{
			return num4 < 0.0 - num5;
		}
		return true;
	}

	private static bool AXISTEST_X2(double a, double b, double fa, double fb, double boxhalfsize, GVector3D v0, GVector3D v1)
	{
		double num = a * v0.Y - b * v0.Z;
		double num2 = a * v1.Y - b * v1.Z;
		double num3;
		double num4;
		if (num < num2)
		{
			num3 = num;
			num4 = num2;
		}
		else
		{
			num3 = num2;
			num4 = num;
		}
		double num5 = fa * boxhalfsize + fb * boxhalfsize;
		if (!(num3 > num5))
		{
			return num4 < 0.0 - num5;
		}
		return true;
	}

	private static bool AXISTEST_Y02(double a, double b, double fa, double fb, double boxhalfsize, GVector3D v0, GVector3D v2)
	{
		double num = (0.0 - a) * v0.X + b * v0.Z;
		double num2 = (0.0 - a) * v2.X + b * v2.Z;
		double num3;
		double num4;
		if (num < num2)
		{
			num3 = num;
			num4 = num2;
		}
		else
		{
			num3 = num2;
			num4 = num;
		}
		double num5 = fa * boxhalfsize + fb * boxhalfsize;
		if (!(num3 > num5))
		{
			return num4 < 0.0 - num5;
		}
		return true;
	}

	private static bool AXISTEST_Y1(double a, double b, double fa, double fb, double boxhalfsize, GVector3D v0, GVector3D v1)
	{
		double num = (0.0 - a) * v0.X + b * v0.Z;
		double num2 = (0.0 - a) * v1.X + b * v1.Z;
		double num3;
		double num4;
		if (num < num2)
		{
			num3 = num;
			num4 = num2;
		}
		else
		{
			num3 = num2;
			num4 = num;
		}
		double num5 = fa * boxhalfsize + fb * boxhalfsize;
		if (!(num3 > num5))
		{
			return num4 < 0.0 - num5;
		}
		return true;
	}

	private static bool AXISTEST_Z12(double a, double b, double fa, double fb, double boxhalfsize, GVector3D v1, GVector3D v2)
	{
		double num = a * v1.X - b * v1.Y;
		double num2 = a * v2.X - b * v2.Y;
		double num3;
		double num4;
		if (num2 < num)
		{
			num3 = num2;
			num4 = num;
		}
		else
		{
			num3 = num;
			num4 = num2;
		}
		double num5 = fa * boxhalfsize + fb * boxhalfsize;
		if (!(num3 > num5))
		{
			return num4 < 0.0 - num5;
		}
		return true;
	}

	private static bool AXISTEST_Z0(double a, double b, double fa, double fb, double boxhalfsize, GVector3D v0, GVector3D v1)
	{
		double num = a * v0.X - b * v0.Y;
		double num2 = a * v1.X - b * v1.Y;
		double num3;
		double num4;
		if (num < num2)
		{
			num3 = num;
			num4 = num2;
		}
		else
		{
			num3 = num2;
			num4 = num;
		}
		double num5 = fa * boxhalfsize + fb * boxhalfsize;
		if (!(num3 > num5))
		{
			return num4 < 0.0 - num5;
		}
		return true;
	}

	public static bool TriBoxOverlap(GPoint3D boxcenter, double boxhalfsize, GTriangle triverts)
	{
		GVector3D gVector3D = triverts.Vert0 - boxcenter;
		GVector3D gVector3D2 = triverts.Vert1 - boxcenter;
		GVector3D gVector3D3 = triverts.Vert2 - boxcenter;
		GVector3D v = gVector3D2 - gVector3D;
		GVector3D v2 = gVector3D3 - gVector3D2;
		GVector3D gVector3D4 = gVector3D - gVector3D3;
		double fb = Math.Abs(v.X);
		double num = Math.Abs(v.Y);
		double fa = Math.Abs(v.Z);
		if (AXISTEST_X01(v.Z, v.Y, fa, num, boxhalfsize, gVector3D, gVector3D3))
		{
			return false;
		}
		if (AXISTEST_Y02(v.Z, v.X, fa, fb, boxhalfsize, gVector3D, gVector3D3))
		{
			return false;
		}
		if (AXISTEST_Z12(v.Y, v.X, num, fb, boxhalfsize, gVector3D2, gVector3D3))
		{
			return false;
		}
		fb = Math.Abs(v2.X);
		num = Math.Abs(v2.Y);
		fa = Math.Abs(v2.Z);
		if (AXISTEST_X01(v2.Z, v2.Y, fa, num, boxhalfsize, gVector3D, gVector3D3))
		{
			return false;
		}
		if (AXISTEST_Y02(v2.Z, v2.X, fa, fb, boxhalfsize, gVector3D, gVector3D3))
		{
			return false;
		}
		if (AXISTEST_Z0(v2.Y, v2.X, num, fb, boxhalfsize, gVector3D, gVector3D2))
		{
			return false;
		}
		fb = Math.Abs(gVector3D4.X);
		num = Math.Abs(gVector3D4.Y);
		fa = Math.Abs(gVector3D4.Z);
		if (AXISTEST_X2(gVector3D4.Z, gVector3D4.Y, fa, num, boxhalfsize, gVector3D, gVector3D2))
		{
			return false;
		}
		if (AXISTEST_Y1(gVector3D4.Z, gVector3D4.X, fa, fb, boxhalfsize, gVector3D, gVector3D2))
		{
			return false;
		}
		if (AXISTEST_Z12(gVector3D4.Y, gVector3D4.X, num, fb, boxhalfsize, gVector3D2, gVector3D3))
		{
			return false;
		}
		Findminmax(gVector3D.X, gVector3D2.X, gVector3D3.X, out var min, out var max);
		if (min > boxhalfsize || max < 0.0 - boxhalfsize)
		{
			return false;
		}
		Findminmax(gVector3D.Y, gVector3D2.Y, gVector3D3.Y, out min, out max);
		if (min > boxhalfsize || max < 0.0 - boxhalfsize)
		{
			return false;
		}
		Findminmax(gVector3D.Z, gVector3D2.Z, gVector3D3.Z, out min, out max);
		if (min > boxhalfsize || max < 0.0 - boxhalfsize)
		{
			return false;
		}
		return PlaneBoxOverlap(GVector3D.CrossProduct(v, v2), gVector3D, boxhalfsize);
	}
}
