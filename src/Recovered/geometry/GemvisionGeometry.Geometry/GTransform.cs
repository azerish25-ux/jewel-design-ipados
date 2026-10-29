using System;

namespace GemvisionGeometry.Geometry;

public struct GTransform
{
	internal double M00;

	internal double M01;

	internal double M02;

	internal double M03;

	internal double M10;

	internal double M11;

	internal double M12;

	internal double M13;

	internal double M20;

	internal double M21;

	internal double M22;

	internal double M23;

	internal double M30;

	internal double M31;

	internal double M32;

	internal double M33;

	public static GTransform Identity => new GTransform
	{
		M00 = 1.0,
		M11 = 1.0,
		M22 = 1.0,
		M33 = 1.0
	};

	public static GTransform Unset => new GTransform
	{
		M00 = -1.23432101234321E+308,
		M01 = -1.23432101234321E+308,
		M02 = -1.23432101234321E+308,
		M03 = -1.23432101234321E+308,
		M10 = -1.23432101234321E+308,
		M11 = -1.23432101234321E+308,
		M12 = -1.23432101234321E+308,
		M13 = -1.23432101234321E+308,
		M20 = -1.23432101234321E+308,
		M21 = -1.23432101234321E+308,
		M22 = -1.23432101234321E+308,
		M23 = -1.23432101234321E+308,
		M30 = -1.23432101234321E+308,
		M31 = -1.23432101234321E+308,
		M32 = -1.23432101234321E+308,
		M33 = -1.23432101234321E+308
	};

	public static GTransform Orient(GPoint3D o, GVector3D x, GVector3D y, GVector3D z)
	{
		GTransform identity = Identity;
		identity.M00 = x.X;
		identity.M01 = x.Y;
		identity.M02 = x.Z;
		identity.M03 = o.X;
		identity.M10 = y.X;
		identity.M11 = y.Y;
		identity.M12 = y.Z;
		identity.M13 = o.Y;
		identity.M20 = z.X;
		identity.M21 = z.Y;
		identity.M22 = z.Z;
		identity.M23 = o.Z;
		return identity;
	}

	public static GTransform operator *(GTransform a, GTransform b)
	{
		return new GTransform
		{
			M00 = a.M00 * b.M00 + a.M01 * b.M10 + a.M02 * b.M20 + a.M03 * b.M30,
			M01 = a.M00 * b.M01 + a.M01 * b.M11 + a.M02 * b.M21 + a.M03 * b.M31,
			M02 = a.M00 * b.M02 + a.M01 * b.M12 + a.M02 * b.M22 + a.M03 * b.M32,
			M03 = a.M00 * b.M03 + a.M01 * b.M13 + a.M02 * b.M23 + a.M03 * b.M33,
			M10 = a.M10 * b.M00 + a.M11 * b.M10 + a.M12 * b.M20 + a.M13 * b.M30,
			M11 = a.M10 * b.M01 + a.M11 * b.M11 + a.M12 * b.M21 + a.M13 * b.M31,
			M12 = a.M10 * b.M02 + a.M11 * b.M12 + a.M12 * b.M22 + a.M13 * b.M32,
			M13 = a.M10 * b.M03 + a.M11 * b.M13 + a.M12 * b.M23 + a.M13 * b.M33,
			M20 = a.M20 * b.M00 + a.M21 * b.M10 + a.M22 * b.M20 + a.M23 * b.M30,
			M21 = a.M20 * b.M01 + a.M21 * b.M11 + a.M22 * b.M21 + a.M23 * b.M31,
			M22 = a.M20 * b.M02 + a.M21 * b.M12 + a.M22 * b.M22 + a.M23 * b.M32,
			M23 = a.M20 * b.M03 + a.M21 * b.M13 + a.M22 * b.M23 + a.M23 * b.M33,
			M30 = a.M30 * b.M00 + a.M31 * b.M10 + a.M32 * b.M20 + a.M33 * b.M30,
			M31 = a.M30 * b.M01 + a.M31 * b.M11 + a.M32 * b.M21 + a.M33 * b.M31,
			M32 = a.M30 * b.M02 + a.M31 * b.M12 + a.M32 * b.M22 + a.M33 * b.M32,
			M33 = a.M30 * b.M03 + a.M31 * b.M13 + a.M32 * b.M23 + a.M33 * b.M33
		};
	}

	public static GTransform Translation(double dx, double dy, double dz)
	{
		GTransform identity = Identity;
		identity.M03 = dx;
		identity.M13 = dy;
		identity.M23 = dz;
		identity.M33 = 1.0;
		return identity;
	}

	public static GTransform Translation(GPoint3D d)
	{
		GTransform identity = Identity;
		identity.M03 = d.X;
		identity.M13 = d.Y;
		identity.M23 = d.Z;
		identity.M33 = 1.0;
		return identity;
	}

	public static GTransform ScaleOrigin(GVector3D p)
	{
		return ScaleOrigin(p.X, p.Y, p.Z);
	}

	public static GTransform ScaleOrigin(double dx, double dy, double dz)
	{
		return new GTransform
		{
			M00 = dx,
			M11 = dy,
			M22 = dz,
			M33 = 1.0
		};
	}

	public static GTransform Rotation(double angle, GVector3D v1)
	{
		double num = Math.Cos(angle);
		double num2 = Math.Sin(angle);
		double num3 = 1.0 - num;
		v1.Unitize();
		double x = v1.X;
		double y = v1.Y;
		double z = v1.Z;
		return new GTransform
		{
			M00 = num3 * x * x + num,
			M10 = num3 * x * y - num2 * z,
			M20 = num3 * x * z + num2 * y,
			M01 = num3 * x * y + num2 * z,
			M11 = num3 * y * y + num,
			M21 = num3 * y * z - num2 * x,
			M02 = num3 * x * z - num2 * y,
			M12 = num3 * y * z + num2 * x,
			M22 = num3 * z * z + num,
			M33 = 1.0
		};
	}
}
