using System;
using GemvisionGeometry.Geometry;
using GemvisionGeometry.Helpers;

namespace GemvisionGeometry.Calc;

public static class BlendCurve
{
	public static GPoint3D[] GetBlendCurv(GPoint3D p0, GPoint3D p1, GVector3D v0, GVector3D v1)
	{
		double num = calcVLen(p1, p0, v1, v0);
		double num2 = calcVLen(p0, p1, v0, v1);
		v0 *= num;
		v1 *= num2;
		return new GPoint3D[4]
		{
			p0,
			p0 + v0,
			p1 + v1,
			p1
		};
	}

	public static GNurbsCurve GetBlendCurve(GPoint3D p0, GPoint3D p1, GVector3D v0, GVector3D v1)
	{
		return GCurveHelper.PointsToNurbs(GetBlendCurv(p0, p1, v0, v1));
	}

	private static double calcVLen(GPoint3D P0, GPoint3D P1, GVector3D V0, GVector3D V1)
	{
		double num = (P0 - P1).Length / 10.0;
		GVector3D gVector3D = P1 - P0;
		gVector3D.Unitize();
		V0.Unitize();
		V1.Unitize();
		double num2 = Math.Acos(gVector3D * V1);
		if (num2 > Math.PI / 2.0)
		{
			num2 = Math.PI - num2;
		}
		double num3 = Math.Acos(-gVector3D * V0);
		if (num3 > Math.PI / 2.0)
		{
			num3 = Math.PI - num3;
		}
		num2 /= Math.PI / 2.0;
		num3 /= Math.PI / 2.0;
		double num4 = quadVal(1.0 - num3, new double[4] { 0.0, 0.11111111, 0.7777777777, 1.0 }, new double[4] { 4.6666666, 4.797685, 2.976183, 2.66666666 });
		double num5 = quadVal(1.0 - num3, new double[4] { 0.0, 0.11111111, 0.7777777777, 1.0 }, new double[4] { 9.333333333, 8.885525, 4.510541, 4.0 });
		return (num4 + Math.Pow(num2, 1.8) * (num5 - num4)) * num;
	}

	private static double quadVal(double v, double[] x, double[] y)
	{
		int num = x.Length;
		double num2 = 0.0;
		for (int i = 0; i < num; i++)
		{
			double num3 = 1.0;
			double num4 = 1.0;
			for (int j = 0; j < num; j++)
			{
				if (i != j)
				{
					num4 *= x[i] - x[j];
					num3 *= v - x[j];
				}
			}
			num2 += num3 * y[i] / num4;
		}
		return num2;
	}
}
