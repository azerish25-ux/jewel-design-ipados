using System;
using GemvisionGeometry.Helpers;

namespace GemvisionGeometry.Geometry;

public struct GVector3F
{
	internal float Mx;

	internal float My;

	internal float Mz;

	public static GVector3F XAxis => new GVector3F(1f, 0f, 0f);

	public static GVector3F YAxis => new GVector3F(0f, 1f, 0f);

	public static GVector3F ZAxis => new GVector3F(0f, 0f, 1f);

	public bool IsValid
	{
		get
		{
			if (GMath.IsValidSingle(Mx) && GMath.IsValidSingle(My) && GMath.IsValidSingle(Mz))
			{
				return Math.Abs(GetSquaredLength()) > double.Epsilon;
			}
			return false;
		}
	}

	public float X
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

	public float Y
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

	public float Z
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

	public GVector3F(float x, float y, float z)
	{
		Mx = x;
		My = y;
		Mz = z;
	}

	public GVector3F(GPoint3F pt)
	{
		Mx = pt.X;
		My = pt.Y;
		Mz = pt.Z;
	}

	public GVector3F(GVector3F vector)
	{
		Mx = vector.X;
		My = vector.Y;
		Mz = vector.Z;
	}

	public static double Angle(GVector3F vec1, GVector3F vec2)
	{
		double num = Math.Atan2(vec1.Y, vec1.X);
		double num2 = Math.Atan2(vec2.Y, vec2.X);
		double num3 = Math.Abs(num - num2);
		return Math.Min(num3, Math.Abs(180.0 - num3));
	}

	public static bool AntiParallel(GVector3F vec1, GVector3F vec2)
	{
		return Math.Abs(Angle(vec1, vec2) - Math.PI) < double.Epsilon;
	}

	public static GVector3F CrossProduct(GVector3F v1, GVector3F v2)
	{
		return new GVector3F(v1.My * v2.Mz - v1.Mz * v2.My, v1.Mz * v2.Mx - v1.Mx * v2.Mz, v1.Mx * v2.My - v1.My * v2.Mx);
	}

	public static GVector3F operator +(GVector3F vector1, GVector3F vector2)
	{
		return new GVector3F(vector1.Mx + vector2.Mx, vector1.My + vector2.My, vector1.Mz + vector2.Mz);
	}

	public static GVector3F operator -(GVector3F vec)
	{
		return new GVector3F(0f - vec.Mx, 0f - vec.My, 0f - vec.Mz);
	}

	public static GVector3F operator -(GVector3F vector1, GVector3F vector2)
	{
		return new GVector3F(vector1.Mx - vector2.Mx, vector1.My - vector2.My, vector1.Mz - vector2.Mz);
	}

	public static float operator *(GVector3F vector1, GVector3F vector2)
	{
		return vector1.Mx * vector2.Mx + vector1.My * vector2.My + vector1.Mz * vector2.Mz;
	}

	public static GVector3F operator *(GVector3F vector1, float factor)
	{
		return new GVector3F(vector1.Mx * factor, vector1.My * factor, vector1.Mz * factor);
	}

	public static bool Parallel(GVector3F vec1, GVector3F vec2)
	{
		double num = Angle(vec1, vec2);
		if (!(num < double.Epsilon))
		{
			return Math.Abs(num - Math.PI * 2.0) < double.Epsilon;
		}
		return true;
	}

	public static GVector3F Rotate2D(GVector3F targetPt, double angle)
	{
		double num = Math.Cos(angle);
		double num2 = Math.Sin(angle);
		return new GVector3F
		{
			X = (float)(num * (double)targetPt.X - num2 * (double)targetPt.Y),
			Y = (float)(num2 * (double)targetPt.X + num * (double)targetPt.Y)
		};
	}

	public double Angle(GVector3F other)
	{
		return Angle(this, other);
	}

	public GVector3F Duplicate()
	{
		return new GVector3F(this);
	}

	public double GetLength()
	{
		return Math.Sqrt(GetSquaredLength());
	}

	public double GetSquaredLength()
	{
		return Math.Pow(Mx, 2.0) + Math.Pow(My, 2.0) + Math.Pow(Mz, 2.0);
	}

	public bool IsAntiParallel(GVector3F other)
	{
		return AntiParallel(this, other);
	}

	public bool IsParallel(GVector3F other)
	{
		return Parallel(this, other);
	}

	public GVector3F Perpendicular(bool unitize)
	{
		return Perpendicular(this, unitize);
	}

	public bool Reverse()
	{
		if (!IsValid)
		{
			return false;
		}
		this *= -1f;
		return true;
	}

	public void Rotate2D(double angle)
	{
		GVector3F gVector3F = Rotate2D(this, angle);
		X = gVector3F.X;
		Y = gVector3F.Y;
	}

	public bool Unitize()
	{
		if (!IsValid)
		{
			return false;
		}
		float num = (float)GetLength();
		Mx /= num;
		My /= num;
		Mz /= num;
		return true;
	}

	private static GVector3F Perpendicular(GVector3F vector, bool unitize)
	{
		GVector3F result = new GVector3F(vector.Y, 0f - vector.X, 0f);
		if (unitize)
		{
			result.Unitize();
		}
		return result;
	}
}
