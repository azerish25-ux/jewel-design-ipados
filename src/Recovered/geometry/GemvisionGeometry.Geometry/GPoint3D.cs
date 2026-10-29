using System;
using GemvisionGeometry.Helpers;

namespace GemvisionGeometry.Geometry;

public struct GPoint3D : IEquatable<GPoint3D>
{
	internal double Mx;

	internal double My;

	internal double Mz;

	public static GPoint3D Origin => new GPoint3D(0.0, 0.0, 0.0);

	public static GPoint3D Unset => new GPoint3D(-1.23432101234321E+308, -1.23432101234321E+308, -1.23432101234321E+308);

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

	public bool IsValid
	{
		get
		{
			if (GMath.IsValidDouble(Mx) && GMath.IsValidDouble(My))
			{
				return GMath.IsValidDouble(Mz);
			}
			return false;
		}
	}

	public GPoint3D(double x, double y, double z)
	{
		Mx = x;
		My = y;
		Mz = z;
	}

	public GPoint3D(GPoint3D point)
	{
		Mx = point.X;
		My = point.Y;
		Mz = point.Z;
	}

	public GPoint3D(GVector3D vector)
	{
		Mx = vector.X;
		My = vector.Y;
		Mz = vector.Z;
	}

	public GPoint3D(GPoint3F point)
	{
		Mx = point.X;
		My = point.Y;
		Mz = point.Z;
	}

	public static GPoint3D operator -(GPoint3D p0, double val)
	{
		return p0 + (0.0 - val);
	}

	public static GVector3D operator -(GPoint3D p0, GPoint3D p1)
	{
		return new GVector3D(p0.Mx - p1.Mx, p0.My - p1.My, p0.Mz - p1.Mz);
	}

	public static GPoint3D operator -(GPoint3D point, GVector3D vector)
	{
		return new GPoint3D(point.Mx - vector.Mx, point.My - vector.My, point.Mz - vector.Mz);
	}

	public static GPoint3D operator -(GPoint3D p0)
	{
		return new GPoint3D(0.0 - p0.Mx, 0.0 - p0.My, 0.0 - p0.Mz);
	}

	public static GPoint3D operator *(GPoint3D p0, double s)
	{
		return new GPoint3D(p0.Mx * s, p0.My * s, p0.Mz * s);
	}

	public static GPoint3D operator /(GPoint3D p0, double s)
	{
		return new GPoint3D(p0.Mx / s, p0.My / s, p0.Mz / s);
	}

	public static GPoint3D operator +(GPoint3D p0, double val)
	{
		return new GPoint3D(p0.Mx + val, p0.My + val, p0.Mz + val);
	}

	public static GPoint3D operator +(GPoint3D p0, GPoint3D p1)
	{
		return new GPoint3D(p0.Mx + p1.Mx, p0.My + p1.My, p0.Mz + p1.Mz);
	}

	public static GPoint3D operator +(GPoint3D point, GVector3D vector)
	{
		return new GPoint3D(point.Mx + vector.Mx, point.My + vector.My, point.Mz + vector.Mz);
	}

	public static bool operator ==(GPoint3D a, GPoint3D b)
	{
		if (a.Mx == b.Mx && a.My == b.My)
		{
			return a.Mz == b.Mz;
		}
		return false;
	}

	public static bool operator !=(GPoint3D a, GPoint3D b)
	{
		if (a.Mx == b.Mx && a.My == b.My)
		{
			return a.Mz != b.Mz;
		}
		return true;
	}

	public static GPoint3D Rotate2D(GPoint3D targetPt, GPoint3D pivotPt, double angle)
	{
		double num = Math.Cos(angle);
		double num2 = Math.Sin(angle);
		return new GPoint3D
		{
			X = (float)(num * (targetPt.X - pivotPt.X) - num2 * (targetPt.Y - pivotPt.Y) + pivotPt.X),
			Y = (float)(num2 * (targetPt.X - pivotPt.X) + num * (targetPt.Y - pivotPt.Y) + pivotPt.Y)
		};
	}

	public double Distance(GPoint3D otherPt)
	{
		return Math.Sqrt(SquareDistance(otherPt));
	}

	public GPoint3D Duplicate()
	{
		return new GPoint3D(this);
	}

	public bool Equals(GPoint3D other)
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
		if (obj is GPoint3D other)
		{
			return Equals(other);
		}
		return false;
	}

	public override int GetHashCode()
	{
		return (((Mx.GetHashCode() * 397) ^ My.GetHashCode()) * 397) ^ Mz.GetHashCode();
	}

	public bool IsUnset()
	{
		if (Mx != -1.23432101234321E+308 && My != -1.23432101234321E+308)
		{
			return Mz != -1.23432101234321E+308;
		}
		return false;
	}

	public void Rotate2D(GPoint3D pivotPt, double angle)
	{
		GPoint3D gPoint3D = Rotate2D(this, pivotPt, angle);
		X = gPoint3D.X;
		Y = gPoint3D.Y;
	}

	public void Scale(double s)
	{
		Mx *= s;
		My *= s;
		Mz *= s;
	}

	public double SquareDistance(GPoint3D otherPt)
	{
		return (otherPt - this).GetSquaredLength();
	}

	public GPoint3F ToGPoint3F()
	{
		return new GPoint3F((float)Mx, (float)My, (float)Mz);
	}

	public void Transform(GTransform trans)
	{
		double num = trans.M30 * Mx + trans.M31 * My + trans.M32 * Mz + trans.M33;
		if (num != 0.0)
		{
			num = 1.0 / num;
		}
		double mx = num * (trans.M00 * Mx + trans.M01 * My + trans.M02 * Mz + trans.M03);
		double my = num * (trans.M10 * Mx + trans.M11 * My + trans.M12 * Mz + trans.M13);
		double mz = num * (trans.M20 * Mx + trans.M21 * My + trans.M22 * Mz + trans.M23);
		Mx = mx;
		My = my;
		Mz = mz;
	}

	public void Translate(GPoint3D t)
	{
		Translate(new GVector3D(t));
	}

	public void Translate(GVector3D t)
	{
		Mx += t.Mx;
		My += t.My;
		Mz += t.Mz;
	}
}
