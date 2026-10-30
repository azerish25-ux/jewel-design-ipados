using System;

namespace OctTree;

internal sealed class TriBoxOriginal
{
	private const int X = 0;

	private const int Y = 1;

	private const int Z = 2;

	private double[] v0;

	private double[] v1;

	private double[] v2;

	private double min;

	private double max;

	private double p0;

	private double p1;

	private double p2;

	private double rad;

	private double[] boxhalfsize;

	private void CROSS(out double[] dest, double[] v1, double[] v2)
	{
		dest = new double[3];
		dest[0] = v1[1] * v2[2] - v1[2] * v2[1];
		dest[1] = v1[2] * v2[0] - v1[0] * v2[2];
		dest[2] = v1[0] * v2[1] - v1[1] * v2[0];
	}

	private double DOT(double[] v1, double[] v2)
	{
		return v1[0] * v2[0] + v1[1] * v2[1] + v1[2] * v2[2];
	}

	private void SUB(out double[] dest, double[] v1, double[] v2)
	{
		dest = new double[3];
		dest[0] = v1[0] - v2[0];
		dest[1] = v1[1] - v2[1];
		dest[2] = v1[2] - v2[2];
	}

	private void FINDMINMAX(double x0, double x1, double x2)
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

	private bool planeBoxOverlap(double[] normal, double[] vert, double[] maxbox)
	{
		double[] array = new double[3];
		double[] array2 = new double[3];
		for (int i = 0; i <= 2; i++)
		{
			double num = vert[i];
			if (normal[i] > 0.0)
			{
				array[i] = 0.0 - maxbox[i] - num;
				array2[i] = maxbox[i] - num;
			}
			else
			{
				array[i] = maxbox[i] - num;
				array2[i] = 0.0 - maxbox[i] - num;
			}
		}
		if (DOT(normal, array) > 0.0)
		{
			return false;
		}
		if (DOT(normal, array2) >= 0.0)
		{
			return true;
		}
		return false;
	}

	private bool AXISTEST_X01(double a, double b, double fa, double fb)
	{
		p0 = a * v0[1] - b * v0[2];
		p2 = a * v2[1] - b * v2[2];
		if (p0 < p2)
		{
			min = p0;
			max = p2;
		}
		else
		{
			min = p2;
			max = p0;
		}
		rad = fa * boxhalfsize[1] + fb * boxhalfsize[2];
		if (!(min > rad))
		{
			return max < 0.0 - rad;
		}
		return true;
	}

	private bool AXISTEST_X2(double a, double b, double fa, double fb)
	{
		p0 = a * v0[1] - b * v0[2];
		p1 = a * v1[1] - b * v1[2];
		if (p0 < p1)
		{
			min = p0;
			max = p1;
		}
		else
		{
			min = p1;
			max = p0;
		}
		rad = fa * boxhalfsize[1] + fb * boxhalfsize[2];
		if (!(min > rad))
		{
			return max < 0.0 - rad;
		}
		return true;
	}

	private bool AXISTEST_Y02(double a, double b, double fa, double fb)
	{
		p0 = (0.0 - a) * v0[0] + b * v0[2];
		p2 = (0.0 - a) * v2[0] + b * v2[2];
		if (p0 < p2)
		{
			min = p0;
			max = p2;
		}
		else
		{
			min = p2;
			max = p0;
		}
		rad = fa * boxhalfsize[0] + fb * boxhalfsize[2];
		if (!(min > rad))
		{
			return max < 0.0 - rad;
		}
		return true;
	}

	private bool AXISTEST_Y1(double a, double b, double fa, double fb)
	{
		p0 = (0.0 - a) * v0[0] + b * v0[2];
		p1 = (0.0 - a) * v1[0] + b * v1[2];
		if (p0 < p1)
		{
			min = p0;
			max = p1;
		}
		else
		{
			min = p1;
			max = p0;
		}
		rad = fa * boxhalfsize[0] + fb * boxhalfsize[2];
		if (!(min > rad))
		{
			return max < 0.0 - rad;
		}
		return true;
	}

	private bool AXISTEST_Z12(double a, double b, double fa, double fb)
	{
		p1 = a * v1[0] - b * v1[1];
		p2 = a * v2[0] - b * v2[1];
		if (p2 < p1)
		{
			min = p2;
			max = p1;
		}
		else
		{
			min = p1;
			max = p2;
		}
		rad = fa * boxhalfsize[0] + fb * boxhalfsize[1];
		if (!(min > rad))
		{
			return max < 0.0 - rad;
		}
		return true;
	}

	private bool AXISTEST_Z0(double a, double b, double fa, double fb)
	{
		p0 = a * v0[0] - b * v0[1];
		p1 = a * v1[0] - b * v1[1];
		if (p0 < p1)
		{
			min = p0;
			max = p1;
		}
		else
		{
			min = p1;
			max = p0;
		}
		rad = fa * boxhalfsize[0] + fb * boxhalfsize[1];
		if (!(min > rad))
		{
			return max < 0.0 - rad;
		}
		return true;
	}

	public bool triBoxOverlap(double[] boxcenter, double[] boxhalfsizeI, double[][] triverts)
	{
		boxhalfsize = boxhalfsizeI;
		SUB(out v0, triverts[0], boxcenter);
		SUB(out v1, triverts[1], boxcenter);
		SUB(out v2, triverts[2], boxcenter);
		SUB(out var dest, v1, v0);
		SUB(out var dest2, v2, v1);
		SUB(out var dest3, v0, v2);
		double fb = Math.Abs(dest[0]);
		double num = Math.Abs(dest[1]);
		double fa = Math.Abs(dest[2]);
		if (AXISTEST_X01(dest[2], dest[1], fa, num))
		{
			return false;
		}
		if (AXISTEST_Y02(dest[2], dest[0], fa, fb))
		{
			return false;
		}
		if (AXISTEST_Z12(dest[1], dest[0], num, fb))
		{
			return false;
		}
		fb = Math.Abs(dest2[0]);
		num = Math.Abs(dest2[1]);
		fa = Math.Abs(dest2[2]);
		if (AXISTEST_X01(dest2[2], dest2[1], fa, num))
		{
			return false;
		}
		if (AXISTEST_Y02(dest2[2], dest2[0], fa, fb))
		{
			return false;
		}
		if (AXISTEST_Z0(dest2[1], dest2[0], num, fb))
		{
			return false;
		}
		fb = Math.Abs(dest3[0]);
		num = Math.Abs(dest3[1]);
		fa = Math.Abs(dest3[2]);
		if (AXISTEST_X2(dest3[2], dest3[1], fa, num))
		{
			return false;
		}
		if (AXISTEST_Y1(dest3[2], dest3[0], fa, fb))
		{
			return false;
		}
		if (AXISTEST_Z12(dest3[1], dest3[0], num, fb))
		{
			return false;
		}
		FINDMINMAX(v0[0], v1[0], v2[0]);
		if (min > boxhalfsize[0] || max < 0.0 - boxhalfsize[0])
		{
			return false;
		}
		FINDMINMAX(v0[1], v1[1], v2[1]);
		if (min > boxhalfsize[1] || max < 0.0 - boxhalfsize[1])
		{
			return false;
		}
		FINDMINMAX(v0[2], v1[2], v2[2]);
		if (min > boxhalfsize[2] || max < 0.0 - boxhalfsize[2])
		{
			return false;
		}
		CROSS(out var dest4, dest, dest2);
		return planeBoxOverlap(dest4, v0, boxhalfsize);
	}
}
