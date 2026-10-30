using System;
using GemvisionGeometry.Helpers;

namespace GemvisionGeometry.Geometry;

public struct GVector3D
{
	internal double Mx;

	internal double My;

	internal double Mz;

	public static GVector3D XAxis => new GVector3D(1.0, 0.0, 0.0);

	public static GVector3D YAxis => new GVector3D(0.0, 1.0, 0.0);

	public static GVector3D ZAxis => new GVector3D(0.0, 0.0, 1.0);

	public bool IsValid
	{
		get
		{
			if (GMath.IsValidDouble(Mx) && GMath.IsValidDouble(My) && GMath.IsValidDouble(Mz))
			{
				return Math.Abs(GetSquaredLength()) > double.Epsilon;
			}
			return false;
		}
	}

	public double X
	{
		get
		{
			return Mx;
		}
		set
		{
			Mx = value;
		}
	}

	public double Y
	{
		get
		{
			return My;
		}
		set
		{
			My = value;
		}
	}

	public double Z
	{
		get
		{
			return Mz;
		}
		set
		{
			Mz = value;
		}
	}

	public double Length => Math.Sqrt(GetSquaredLength());

	public GVector3D(double x, double y, double z)
	{
		Mx = x;
		My = y;
		Mz = z;
	}

	public GVector3D(GPoint3D point)
	{
		Mx = point.X;
		My = point.Y;
		Mz = point.Z;
	}

	public GVector3D(GVector3D vector)
	{
		Mx = vector.X;
		My = vector.Y;
		Mz = vector.Z;
	}

	public static double Angle(GVector3D vec1, GVector3D vec2)
	{
		double num = Math.Atan2(vec1.Y, vec1.X);
		double num2 = Math.Atan2(vec2.Y, vec2.X);
		double num3 = Math.Abs(num - num2);
		return Math.Min(num3, Math.Abs(180.0 - num3));
	}

	public static bool AntiParallel(GVector3D vec1, GVector3D vec2)
	{
		return Math.Abs(Angle(vec1, vec2) - Math.PI) < double.Epsilon;
	}

	public static GVector3D CrossProduct(GVector3D v1, GVector3D v2)
	{
		return new GVector3D(v1.My * v2.Mz - v1.Mz * v2.My, v1.Mz * v2.Mx - v1.Mx * v2.Mz, v1.Mx * v2.My - v1.My * v2.Mx);
	}

	public static GVector3D operator +(GVector3D vector1, GVector3D vector2)
	{
		return new GVector3D(vector1.Mx + vector2.Mx, vector1.My + vector2.My, vector1.Mz + vector2.Mz);
	}

	public static GVector3D operator -(GVector3D vector1, GVector3D vector2)
	{
		return new GVector3D(vector1.Mx - vector2.Mx, vector1.My - vector2.My, vector1.Mz - vector2.Mz);
	}

	public static GVector3D operator -(GVector3D vec)
	{
		return new GVector3D(0.0 - vec.Mx, 0.0 - vec.My, 0.0 - vec.Mz);
	}

	public static double operator *(GVector3D vector1, GVector3D vector2)
	{
		return vector1.Mx * vector2.Mx + vector1.My * vector2.My + vector1.Mz * vector2.Mz;
	}

	public static GVector3D operator *(GVector3D vector1, double factor)
	{
		return new GVector3D(vector1.Mx * factor, vector1.My * factor, vector1.Mz * factor);
	}

	public static bool Parallel(GVector3D vec1, GVector3D vec2)
	{
		double num = Angle(vec1, vec2);
		if (!(num < double.Epsilon))
		{
			return Math.Abs(num - Math.PI * 2.0) < double.Epsilon;
		}
		return true;
	}

	public static GVector3D Perpendicular(GVector3D vector, bool unitize)
	{
		GVector3D result = new GVector3D(vector.Y, 0.0 - vector.X, 0.0);
		if (unitize)
		{
			result.Unitize();
		}
		return result;
	}

	public static GVector3D Rotate2D(GVector3D targetPt, double angle)
	{
		double num = Math.Cos(angle);
		double num2 = Math.Sin(angle);
		return new GVector3D
		{
			X = (float)(num * targetPt.X - num2 * targetPt.Y),
			Y = (float)(num2 * targetPt.X + num * targetPt.Y)
		};
	}

	public double Angle(GVector3D other)
	{
		return Angle(this, other);
	}

	public GVector3D Duplicate()
	{
		return new GVector3D(this);
	}

	public double GetSquaredLength()
	{
		return Math.Pow(Mx, 2.0) + Math.Pow(My, 2.0) + Math.Pow(Mz, 2.0);
	}

	public bool IsAntiParallel(GVector3D other)
	{
		return AntiParallel(this, other);
	}

	public bool IsParallel(GVector3D other)
	{
		return Parallel(this, other);
	}

	public GVector3D Perpendicular(bool unitize)
	{
		return Perpendicular(this, unitize);
	}

	public bool Reverse()
	{
		if (!IsValid)
		{
			return false;
		}
		this *= -1.0;
		return true;
	}

	public void Rotate2D(double angle)
	{
		GVector3D gVector3D = Rotate2D(this, angle);
		X = gVector3D.X;
		Y = gVector3D.Y;
	}

	public bool Unitize()
	{
		if (!IsValid)
		{
			return false;
		}
		double length = Length;
		Mx /= length;
		My /= length;
		Mz /= length;
		return true;
	}
}
