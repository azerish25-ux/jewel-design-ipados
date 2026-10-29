using System;
using GemvisionGeometry.Geometry;

namespace GemvisionGeometry.Helpers;

public static class GMath
{
	public const double UnsetDouble = -1.23432101234321E+308;

	public const float UnsetSingle = -1.234321E+38f;

	public static double AngleBetweenVectors(GVector3D inputVector1, GVector3D inputVector2)
	{
		GVector3D gVector3D = new GVector3D(inputVector1);
		GVector3D gVector3D2 = new GVector3D(inputVector2);
		gVector3D.Unitize();
		gVector3D2.Unitize();
		return Math.Acos(Clamp(gVector3D * gVector3D2, -1.0, 1.0));
	}

	public static float Clamp(float value, float min, float max)
	{
		if (value < min)
		{
			return min;
		}
		if (value > max)
		{
			return max;
		}
		return value;
	}

	public static double Clamp(double value, double min, double max)
	{
		if (value < min)
		{
			return min;
		}
		if (value > max)
		{
			return max;
		}
		return value;
	}

	public static int Clamp(int value, int min, int max)
	{
		if (value < min)
		{
			return min;
		}
		if (value > max)
		{
			return max;
		}
		return value;
	}

	public static double CrossProduct2D(GPoint3D pointA, GPoint3D pointB, GPoint3D pointC)
	{
		double[] array = new double[2];
		double[] array2 = new double[2];
		array[0] = pointB.X - pointA.X;
		array[1] = pointB.Y - pointA.Y;
		array2[0] = pointC.X - pointA.X;
		array2[1] = pointC.Y - pointA.Y;
		return array[0] * array2[1] - array[1] * array2[0];
	}

	public static double DotProduct2D(GPoint3D pointA, GPoint3D pointB, GPoint3D pointC)
	{
		double[] array = new double[2];
		double[] array2 = new double[2];
		array[0] = pointB.X - pointA.X;
		array[1] = pointB.Y - pointA.Y;
		array2[0] = pointC.X - pointB.X;
		array2[1] = pointC.Y - pointB.Y;
		return array[0] * array2[0] + array[1] * array2[1];
	}

	public static int Factorial(int value)
	{
		if (value == 0)
		{
			return 1;
		}
		int num = value;
		for (int num2 = value - 1; num2 >= 1; num2--)
		{
			num *= num2;
		}
		return num;
	}

	public static GPoint3F GetClosestPointToLine(GPoint3F a, GPoint3F b, GPoint3F p, bool finite)
	{
		GVector3F gVector3F = b - a;
		double squaredLength = gVector3F.GetSquaredLength();
		double num = (double)(gVector3F * (p - a)) / squaredLength;
		if (finite && num < 0.0)
		{
			return a.Duplicate();
		}
		if (finite && num > squaredLength)
		{
			return b.Duplicate();
		}
		return new GPoint3F((float)((double)a.X + (double)gVector3F.X * num), (float)((double)a.Y + (double)gVector3F.Y * num), 0f);
	}

	public static bool IsValidDouble(double x)
	{
		if (Math.Abs(x - -1.23432101234321E+308) > double.Epsilon && !double.IsInfinity(x))
		{
			return !double.IsNaN(x);
		}
		return false;
	}

	public static bool IsValidSingle(double x)
	{
		if (Math.Abs(x - -1.2343210235867887E+38) > double.Epsilon && !double.IsInfinity(x))
		{
			return !double.IsNaN(x);
		}
		return false;
	}

	public static GPoint3D LineInt(GPoint3D point0, GVector3D vector0, GPoint3D point1, GVector3D vector1)
	{
		double num = (vector0.X * (point0.Y - point1.Y) + vector0.Y * (point1.X - point0.X)) / (vector1.Y * vector0.X - vector0.Y * vector1.X);
		if (!double.IsNaN(num) && !double.IsInfinity(num))
		{
			return new GPoint3D(point0.X + vector0.X * num, point0.Y + vector0.Y * num, point0.Z + vector0.Z * num);
		}
		return GPoint3D.Unset;
	}

	public static double LineToPointDistance2D(GPoint3D lineStart, GPoint3D lineEnd, GPoint3D testPt, bool finiteLine)
	{
		double value = CrossProduct2D(lineStart, lineEnd, testPt) / lineStart.Distance(lineEnd);
		if (!finiteLine)
		{
			return Math.Abs(value);
		}
		if (DotProduct2D(lineStart, lineEnd, testPt) > 0.0)
		{
			return lineEnd.Distance(testPt);
		}
		if (DotProduct2D(lineEnd, lineStart, testPt) > 0.0)
		{
			return lineStart.Distance(testPt);
		}
		return Math.Abs(value);
	}

	public static double ToDegrees(double angle)
	{
		return angle / Math.PI * 180.0;
	}

	public static float ToDegrees(float angle)
	{
		return (float)ToDegrees((double)angle);
	}

	public static double ToRadians(double angle)
	{
		return angle / 180.0 * Math.PI;
	}

	public static float ToRadians(float angle)
	{
		return (float)ToRadians((double)angle);
	}
}
