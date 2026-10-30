using System;

namespace GemvisionGeometry.Geometry;

public struct GPoint4D : IEquatable<GPoint4D>
{
	internal double Mx;

	internal double My;

	internal double Mz;

	internal double Mw;

	public static GPoint4D Unset => new GPoint4D(-1.23432101234321E+308, -1.23432101234321E+308, -1.23432101234321E+308, -1.23432101234321E+308);

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

	public double W
	{
		get
		{
			return Mw;
		}
		set
		{
			Mw = value;
		}
	}

	public GPoint4D(double x, double y, double z, double w)
	{
		Mx = x;
		My = y;
		Mz = z;
		Mw = w;
	}

	public GPoint4D(GPoint3D point)
	{
		Mx = point.X;
		My = point.Y;
		Mz = point.Z;
		Mw = 1.0;
	}

	public GPoint4D(GPoint4D point)
	{
		Mx = point.X;
		My = point.Y;
		Mz = point.Z;
		Mw = point.W;
	}

	public static GPoint4D Rotate2D(GPoint4D targetPt, GPoint4D pivotPt, double angle)
	{
		double num = Math.Cos(angle);
		double num2 = Math.Sin(angle);
		return new GPoint4D
		{
			X = (float)(num * (targetPt.X - pivotPt.X) - num2 * (targetPt.Y - pivotPt.Y) + pivotPt.X),
			Y = (float)(num2 * (targetPt.X - pivotPt.X) + num * (targetPt.Y - pivotPt.Y) + pivotPt.Y)
		};
	}

	public double Distance(GPoint4D otherPt)
	{
		return Math.Sqrt(SquareDistance(new GPoint3D(otherPt.X, otherPt.Y, otherPt.Z)));
	}

	public GPoint4D Duplicate()
	{
		return new GPoint4D(this);
	}

	public bool Equals(GPoint4D other)
	{
		if (Mx.Equals(other.Mx) && My.Equals(other.My) && Mz.Equals(other.Mz))
		{
			return Mw.Equals(other.Mw);
		}
		return false;
	}

	public override bool Equals(object obj)
	{
		if (obj == null)
		{
			return false;
		}
		if (obj is GPoint4D other)
		{
			return Equals(other);
		}
		return false;
	}

	public override int GetHashCode()
	{
		return (((((Mx.GetHashCode() * 397) ^ My.GetHashCode()) * 397) ^ Mz.GetHashCode()) * 397) ^ Mw.GetHashCode();
	}

	public bool IsUnset()
	{
		if (Math.Abs(Mx - -1.23432101234321E+308) < double.Epsilon && Math.Abs(My - -1.23432101234321E+308) < double.Epsilon && Math.Abs(Mz - -1.23432101234321E+308) < double.Epsilon)
		{
			return Math.Abs(Mw - -1.23432101234321E+308) < double.Epsilon;
		}
		return false;
	}

	public void Rotate2D(GPoint4D pivotPt, double angle)
	{
		GPoint4D gPoint4D = Rotate2D(this, pivotPt, angle);
		X = gPoint4D.X;
		Y = gPoint4D.Y;
	}

	public void Scale(double s)
	{
		Mx *= s;
		My *= s;
		Mz *= s;
	}

	public double SquareDistance(GPoint3D otherPt)
	{
		return (otherPt - new GPoint3D(X, Y, Z)).GetSquaredLength();
	}

	public GPoint3D Location()
	{
		return new GPoint3D((float)Mx, (float)My, (float)Mz);
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

	public void Translate(GPoint4D t)
	{
		Translate(new GVector3D(t.X, t.Y, t.Z));
	}

	public void Translate(GVector3D t)
	{
		Mx += t.Mx;
		My += t.My;
		Mz += t.Mz;
	}
}
