using System;

namespace GemvisionGeometry.Geometry;

public struct GPoint3F : IEquatable<GPoint3F>
{
	internal float Mx;

	internal float My;

	internal float Mz;

	public static GPoint3F Unset => new GPoint3F(-1.234321E+38f, -1.234321E+38f, -1.234321E+38f);

	public bool IsUnset => this == Unset;

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

	public GPoint3F(float x, float y, float z)
	{
		Mx = x;
		My = y;
		Mz = z;
	}

	public GPoint3F(GPoint3F point)
	{
		Mx = point.X;
		My = point.Y;
		Mz = point.Z;
	}

	public GPoint3F(GPoint3D point)
	{
		Mx = (float)point.Mx;
		My = (float)point.My;
		Mz = (float)point.Mz;
	}

	public static GPoint3F Rotate2D(GPoint3F targetPt, GPoint3F pivotPt, double angle)
	{
		double num = Math.Cos(angle);
		double num2 = Math.Sin(angle);
		return new GPoint3F
		{
			X = (float)(num * (double)(targetPt.X - pivotPt.X) - num2 * (double)(targetPt.Y - pivotPt.Y) + (double)pivotPt.X),
			Y = (float)(num2 * (double)(targetPt.X - pivotPt.X) + num * (double)(targetPt.Y - pivotPt.Y) + (double)pivotPt.Y)
		};
	}

	public double Distance(GPoint3F otherPt)
	{
		return Math.Sqrt(SquareDistance(otherPt));
	}

	public GPoint3F Duplicate()
	{
		return new GPoint3F(this);
	}

	public bool Equals(GPoint3F other)
	{
		if (Mx.Equals(other.Mx) && My.Equals(other.My))
		{
			return Mz.Equals(other.Mz);
		}
		return false;
	}

	public override bool Equals(object obj)
	{
		if (obj == null)
		{
			return false;
		}
		if (obj is GPoint3F other)
		{
			return Equals(other);
		}
		return false;
	}

	public override int GetHashCode()
	{
		return (((Mx.GetHashCode() * 397) ^ My.GetHashCode()) * 397) ^ Mz.GetHashCode();
	}

	public void Rotate2D(GPoint3F pivotPt, double angle)
	{
		GPoint3F gPoint3F = Rotate2D(this, pivotPt, angle);
		X = gPoint3F.X;
		Y = gPoint3F.Y;
	}

	public double SquareDistance(GPoint3F otherPt)
	{
		return (otherPt - this).GetSquaredLength();
	}

	public void Translate(GPoint3F t)
	{
		Translate(new GVector3F(t));
	}

	public void Translate(GVector3F t)
	{
		Mx += t.Mx;
		My += t.My;
		Mz += t.Mz;
	}

	public static GPoint3F operator -(GPoint3F pt)
	{
		return new GPoint3F(0f - pt.Mx, 0f - pt.My, 0f - pt.Mz);
	}

	public static GVector3F operator -(GPoint3F p0, GPoint3F p1)
	{
		return new GVector3F(p0.Mx - p1.Mx, p0.My - p1.My, p0.Mz - p1.Mz);
	}

	public static bool operator !=(GPoint3F obj1, GPoint3F obj2)
	{
		return !(obj1 == obj2);
	}

	public static GPoint3F operator *(GPoint3F p, float factor)
	{
		return new GPoint3F(p.Mx * factor, p.My * factor, p.Mz * factor);
	}

	public static GPoint3F operator /(GPoint3F p, float divisor)
	{
		return new GPoint3F(p.Mx / divisor, p.My / divisor, p.Mz / divisor);
	}

	public static GPoint3F operator +(GPoint3F p0, GPoint3F p1)
	{
		return new GPoint3F(p0.Mx + p1.Mx, p0.My + p1.My, p0.Mz + p1.Mz);
	}

	public static GPoint3F operator +(GPoint3F p0, GVector3F v1)
	{
		return new GPoint3F(p0.Mx + v1.Mx, p0.My + v1.My, p0.Mz + v1.Mz);
	}

	public static bool operator ==(GPoint3F obj1, GPoint3F obj2)
	{
		if (Math.Abs(obj1.X - obj2.X) < float.Epsilon && Math.Abs(obj1.Y - obj2.Y) < float.Epsilon)
		{
			return Math.Abs(obj1.Z - obj2.Z) < float.Epsilon;
		}
		return false;
	}
}
