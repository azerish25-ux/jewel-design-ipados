using System;
using Rhino.Geometry;

namespace CSBlocks.Surfaces;

public static class MicroPaveProng
{
	public static readonly ControlPoint[,] Cv;

	static MicroPaveProng()
	{
		//IL_0039: Unknown result type (might be due to invalid IL or missing references)
		//IL_003e: Unknown result type (might be due to invalid IL or missing references)
		//IL_006e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0073: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a3: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a8: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d8: Unknown result type (might be due to invalid IL or missing references)
		//IL_00dd: Unknown result type (might be due to invalid IL or missing references)
		//IL_010d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0112: Unknown result type (might be due to invalid IL or missing references)
		//IL_0142: Unknown result type (might be due to invalid IL or missing references)
		//IL_0147: Unknown result type (might be due to invalid IL or missing references)
		//IL_0177: Unknown result type (might be due to invalid IL or missing references)
		//IL_017c: Unknown result type (might be due to invalid IL or missing references)
		//IL_01ac: Unknown result type (might be due to invalid IL or missing references)
		//IL_01b1: Unknown result type (might be due to invalid IL or missing references)
		//IL_01e1: Unknown result type (might be due to invalid IL or missing references)
		//IL_01e6: Unknown result type (might be due to invalid IL or missing references)
		//IL_0216: Unknown result type (might be due to invalid IL or missing references)
		//IL_021b: Unknown result type (might be due to invalid IL or missing references)
		//IL_024b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0250: Unknown result type (might be due to invalid IL or missing references)
		//IL_0280: Unknown result type (might be due to invalid IL or missing references)
		//IL_0285: Unknown result type (might be due to invalid IL or missing references)
		//IL_02b5: Unknown result type (might be due to invalid IL or missing references)
		//IL_02ba: Unknown result type (might be due to invalid IL or missing references)
		//IL_02ea: Unknown result type (might be due to invalid IL or missing references)
		//IL_02ef: Unknown result type (might be due to invalid IL or missing references)
		//IL_031f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0324: Unknown result type (might be due to invalid IL or missing references)
		//IL_0354: Unknown result type (might be due to invalid IL or missing references)
		//IL_0359: Unknown result type (might be due to invalid IL or missing references)
		//IL_0389: Unknown result type (might be due to invalid IL or missing references)
		//IL_038e: Unknown result type (might be due to invalid IL or missing references)
		//IL_03be: Unknown result type (might be due to invalid IL or missing references)
		//IL_03c3: Unknown result type (might be due to invalid IL or missing references)
		//IL_03f3: Unknown result type (might be due to invalid IL or missing references)
		//IL_03f8: Unknown result type (might be due to invalid IL or missing references)
		//IL_0428: Unknown result type (might be due to invalid IL or missing references)
		//IL_042d: Unknown result type (might be due to invalid IL or missing references)
		//IL_045d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0462: Unknown result type (might be due to invalid IL or missing references)
		//IL_0492: Unknown result type (might be due to invalid IL or missing references)
		//IL_0497: Unknown result type (might be due to invalid IL or missing references)
		//IL_04c7: Unknown result type (might be due to invalid IL or missing references)
		//IL_04cc: Unknown result type (might be due to invalid IL or missing references)
		//IL_04fc: Unknown result type (might be due to invalid IL or missing references)
		//IL_0501: Unknown result type (might be due to invalid IL or missing references)
		//IL_0531: Unknown result type (might be due to invalid IL or missing references)
		//IL_0536: Unknown result type (might be due to invalid IL or missing references)
		//IL_0566: Unknown result type (might be due to invalid IL or missing references)
		//IL_056b: Unknown result type (might be due to invalid IL or missing references)
		//IL_059b: Unknown result type (might be due to invalid IL or missing references)
		//IL_05a0: Unknown result type (might be due to invalid IL or missing references)
		//IL_05d0: Unknown result type (might be due to invalid IL or missing references)
		//IL_05d5: Unknown result type (might be due to invalid IL or missing references)
		//IL_0605: Unknown result type (might be due to invalid IL or missing references)
		//IL_060a: Unknown result type (might be due to invalid IL or missing references)
		//IL_063a: Unknown result type (might be due to invalid IL or missing references)
		//IL_063f: Unknown result type (might be due to invalid IL or missing references)
		//IL_066f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0674: Unknown result type (might be due to invalid IL or missing references)
		//IL_06a4: Unknown result type (might be due to invalid IL or missing references)
		//IL_06a9: Unknown result type (might be due to invalid IL or missing references)
		//IL_06d9: Unknown result type (might be due to invalid IL or missing references)
		//IL_06de: Unknown result type (might be due to invalid IL or missing references)
		//IL_070e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0713: Unknown result type (might be due to invalid IL or missing references)
		//IL_0743: Unknown result type (might be due to invalid IL or missing references)
		//IL_0748: Unknown result type (might be due to invalid IL or missing references)
		//IL_0778: Unknown result type (might be due to invalid IL or missing references)
		//IL_077d: Unknown result type (might be due to invalid IL or missing references)
		//IL_07ad: Unknown result type (might be due to invalid IL or missing references)
		//IL_07b2: Unknown result type (might be due to invalid IL or missing references)
		//IL_07e2: Unknown result type (might be due to invalid IL or missing references)
		//IL_07e7: Unknown result type (might be due to invalid IL or missing references)
		//IL_0817: Unknown result type (might be due to invalid IL or missing references)
		//IL_081c: Unknown result type (might be due to invalid IL or missing references)
		//IL_084c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0851: Unknown result type (might be due to invalid IL or missing references)
		//IL_0881: Unknown result type (might be due to invalid IL or missing references)
		//IL_0886: Unknown result type (might be due to invalid IL or missing references)
		//IL_08b6: Unknown result type (might be due to invalid IL or missing references)
		//IL_08bb: Unknown result type (might be due to invalid IL or missing references)
		//IL_08eb: Unknown result type (might be due to invalid IL or missing references)
		//IL_08f0: Unknown result type (might be due to invalid IL or missing references)
		//IL_0920: Unknown result type (might be due to invalid IL or missing references)
		//IL_0925: Unknown result type (might be due to invalid IL or missing references)
		//IL_0955: Unknown result type (might be due to invalid IL or missing references)
		//IL_095a: Unknown result type (might be due to invalid IL or missing references)
		//IL_098a: Unknown result type (might be due to invalid IL or missing references)
		//IL_098f: Unknown result type (might be due to invalid IL or missing references)
		//IL_09bf: Unknown result type (might be due to invalid IL or missing references)
		//IL_09c4: Unknown result type (might be due to invalid IL or missing references)
		//IL_09f4: Unknown result type (might be due to invalid IL or missing references)
		//IL_09f9: Unknown result type (might be due to invalid IL or missing references)
		//IL_0a20: Unknown result type (might be due to invalid IL or missing references)
		//IL_0a25: Unknown result type (might be due to invalid IL or missing references)
		//IL_0a3b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0a40: Unknown result type (might be due to invalid IL or missing references)
		//IL_0a57: Unknown result type (might be due to invalid IL or missing references)
		//IL_0a5c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0a77: Unknown result type (might be due to invalid IL or missing references)
		//IL_0a7c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0ac7: Unknown result type (might be due to invalid IL or missing references)
		//IL_0acc: Unknown result type (might be due to invalid IL or missing references)
		//IL_0ae2: Unknown result type (might be due to invalid IL or missing references)
		//IL_0ae7: Unknown result type (might be due to invalid IL or missing references)
		//IL_0afc: Unknown result type (might be due to invalid IL or missing references)
		//IL_0b01: Unknown result type (might be due to invalid IL or missing references)
		//IL_0b1b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0b20: Unknown result type (might be due to invalid IL or missing references)
		//IL_0b66: Unknown result type (might be due to invalid IL or missing references)
		//IL_0b6b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0b8b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0b90: Unknown result type (might be due to invalid IL or missing references)
		//IL_0bb0: Unknown result type (might be due to invalid IL or missing references)
		//IL_0bb5: Unknown result type (might be due to invalid IL or missing references)
		//IL_0be4: Unknown result type (might be due to invalid IL or missing references)
		//IL_0be9: Unknown result type (might be due to invalid IL or missing references)
		Cv = new ControlPoint[11, 15];
		Cv[0, 0] = new ControlPoint(0.0, -2.46356274147353, -2.43716199288183, 1.0);
		Cv[0, 1] = new ControlPoint(0.0, -2.46356274147353, -1.72287627859612, 1.0);
		Cv[0, 2] = new ControlPoint(0.0, -2.46356274147353, -1.00859056431041, 1.0);
		Cv[0, 3] = new ControlPoint(0.0, -2.35614659109482, -1.00859056431041, 1.0);
		Cv[0, 4] = new ControlPoint(0.0, -1.46356274147353, -1.00859056431041, 1.0);
		Cv[0, 5] = new ControlPoint(0.0, -0.963562741473529, -1.00859056431041, 1.0);
		Cv[0, 6] = new ControlPoint(0.0, -0.463562741473529, -1.00859056431041, 1.0);
		Cv[0, 7] = new ControlPoint(0.0, 0.0, -1.00859056431041, 1.0);
		Cv[1, 0] = new ControlPoint(0.212, -2.46356274147353, -2.43716199288184, 1.0);
		Cv[1, 1] = new ControlPoint(0.212, -2.46356274147353, -1.72287627859613, 1.0);
		Cv[1, 2] = new ControlPoint(0.212, -2.46356274147353, -1.00859056431041, 1.0);
		Cv[1, 3] = new ControlPoint(0.212, -2.35614659109482, -1.00859056431041, 1.0);
		Cv[1, 4] = new ControlPoint(0.212, -1.46356274147353, -1.00859056431041, 1.0);
		Cv[1, 5] = new ControlPoint(0.212, -0.963562741473533, -1.00859056431041, 1.0);
		Cv[1, 6] = new ControlPoint(0.212, -0.463562741473535, -1.00859056431041, 1.0);
		Cv[1, 7] = new ControlPoint(0.212, 0.0, -1.00859056431041, 1.0);
		Cv[2, 0] = new ControlPoint(1.365, -2.46356274147353, -2.43569007868541, 1.0);
		Cv[2, 1] = new ControlPoint(1.365, -2.46356274147353, -1.72140436439971, 1.0);
		Cv[2, 2] = new ControlPoint(1.365, -2.46356274147353, -1.00711865011399, 1.0);
		Cv[2, 3] = new ControlPoint(1.365, -2.35614659109482, -0.703109180164105, 1.0);
		Cv[2, 4] = new ControlPoint(1.365, -1.46356274147353, -0.703109180164105, 1.0);
		Cv[2, 5] = new ControlPoint(1.365, -0.963562741473529, -0.703109180164105, 1.0);
		Cv[2, 6] = new ControlPoint(1.365, -0.463562741473531, -1.00711865011399, 1.0);
		Cv[2, 7] = new ControlPoint(1.365, 0.0, -1.00711865011399, 1.0);
		Cv[3, 0] = new ControlPoint(1.726, -2.46356274147353, -2.42857142857143, 1.0);
		Cv[3, 1] = new ControlPoint(1.726, -2.46356274147353, -1.71428571428572, 1.0);
		Cv[3, 2] = new ControlPoint(1.726, -2.46356274147353, -1.00000000000001, 1.0);
		Cv[3, 3] = new ControlPoint(1.726, -2.43919065825261, 0.641613324503731, 1.0);
		Cv[3, 4] = new ControlPoint(1.726, -1.46356274147354, 0.641613324503731, 1.0);
		Cv[3, 5] = new ControlPoint(1.726, -0.963562741473535, 0.641613324503731, 1.0);
		Cv[3, 6] = new ControlPoint(1.726, -0.463562741473535, -1.0, 1.0);
		Cv[3, 7] = new ControlPoint(1.726, 0.0, -1.0, 1.0);
		Cv[4, 0] = new ControlPoint(2.165, -2.46356274147353, -2.43351395287763, 1.0);
		Cv[4, 1] = new ControlPoint(2.165, -2.46356274147353, -1.71922823859192, 1.0);
		Cv[4, 2] = new ControlPoint(2.165, -2.46356274147353, -1.0049425243062, 1.0);
		Cv[4, 3] = new ControlPoint(2.165, -2.4700922641073, 1.18384014090205, 1.0);
		Cv[4, 4] = new ControlPoint(2.165, -1.46356274147354, 1.176081271134, 1.0);
		Cv[4, 5] = new ControlPoint(2.165, -0.940722478920128, 1.18384014090205, 1.0);
		Cv[4, 6] = new ControlPoint(2.165, -0.463562741473536, -1.00494252430619, 1.0);
		Cv[4, 7] = new ControlPoint(2.165, 0.0, -1.00494252430619, 1.0);
		Cv[5, 0] = new ControlPoint(2.75, -2.46356274147353, -2.42857142857143, 1.0);
		Cv[5, 1] = new ControlPoint(2.75, -2.46356274147353, -1.71428571428572, 1.0);
		Cv[5, 2] = new ControlPoint(2.75, -2.46356274147353, -1.00000000000001, 1.0);
		Cv[5, 3] = new ControlPoint(2.75, -2.46356274147354, 1.20734734912864, 1.0);
		Cv[5, 4] = new ControlPoint(2.75, -1.46356274147353, 1.20734734912864, 1.0);
		Cv[5, 5] = new ControlPoint(2.75, -0.490722478920129, 1.20734734912864, 1.0);
		Cv[5, 6] = new ControlPoint(2.75, -0.463562741473535, -1.0, 1.0);
		Cv[5, 7] = new ControlPoint(2.75, 0.0, -1.0, 1.0);
		Point3d location;
		for (int i = 0; i < 6; i++)
		{
			for (int j = 0; j < 7; j++)
			{
				ControlPoint[,] cv = Cv;
				int num = i;
				int num2 = 8 + j;
				location = ((ControlPoint)(ref Cv[i, 6 - j])).Location;
				double x = ((Point3d)(ref location)).X;
				location = ((ControlPoint)(ref Cv[i, 6 - j])).Location;
				double num3 = 0.0 - ((Point3d)(ref location)).Y;
				location = ((ControlPoint)(ref Cv[i, 6 - j])).Location;
				cv[num, num2] = new ControlPoint(x, num3, ((Point3d)(ref location)).Z, ((ControlPoint)(ref Cv[i, 6 - j])).Weight);
			}
		}
		for (int k = 0; k < 5; k++)
		{
			for (int l = 0; l < 15; l++)
			{
				ControlPoint[,] cv2 = Cv;
				int num4 = 10 - k;
				int num5 = l;
				location = ((ControlPoint)(ref Cv[k, l])).Location;
				double num6 = 5.5 - ((Point3d)(ref location)).X;
				location = ((ControlPoint)(ref Cv[k, l])).Location;
				double y = ((Point3d)(ref location)).Y;
				location = ((ControlPoint)(ref Cv[k, l])).Location;
				cv2[num4, num5] = new ControlPoint(num6, y, ((Point3d)(ref location)).Z, ((ControlPoint)(ref Cv[k, l])).Weight);
			}
		}
		for (int m = 0; m < 11; m++)
		{
			for (int n = 0; n < 15; n++)
			{
				ControlPoint[,] cv3 = Cv;
				int num7 = m;
				int num8 = n;
				location = ((ControlPoint)(ref Cv[m, n])).Location;
				double num9 = ((Point3d)(ref location)).X / 5.5;
				location = ((ControlPoint)(ref Cv[m, n])).Location;
				double num10 = ((Point3d)(ref location)).Y / 2.46356274147353;
				location = ((ControlPoint)(ref Cv[m, n])).Location;
				cv3[num7, num8] = new ControlPoint(num9, num10, (((Point3d)(ref location)).Z - 1.183) / 3.603, ((ControlPoint)(ref Cv[m, n])).Weight);
			}
		}
	}

	public static bool PointOnLineAtDistanceFromPoint(Point3d linePoint, Vector3d lineVector, Point3d testPoint, double distanceOfPointNeeded, ref double lineParamOfClosestPoint, ref double paramAlongLine)
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_0002: Unknown result type (might be due to invalid IL or missing references)
		//IL_000e: Unknown result type (might be due to invalid IL or missing references)
		//IL_000f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0013: Unknown result type (might be due to invalid IL or missing references)
		//IL_0018: Unknown result type (might be due to invalid IL or missing references)
		//IL_0021: Unknown result type (might be due to invalid IL or missing references)
		//IL_0022: Unknown result type (might be due to invalid IL or missing references)
		//IL_0027: Unknown result type (might be due to invalid IL or missing references)
		if (!ClosestPointParam(linePoint, lineVector, testPoint, ref lineParamOfClosestPoint))
		{
			return false;
		}
		Point3d val = linePoint + lineVector * lineParamOfClosestPoint;
		double num = distanceOfPointNeeded * distanceOfPointNeeded;
		Vector3d val2 = val - testPoint;
		double squareLength = ((Vector3d)(ref val2)).SquareLength;
		if (num < squareLength)
		{
			return false;
		}
		paramAlongLine = Math.Sqrt(num - squareLength);
		return true;
	}

	private static bool ClosestPointParam(Point3d linePoint, Vector3d lineVector, Point3d testPoint, ref double lineParam)
	{
		double length = ((Vector3d)(ref lineVector)).Length;
		if (length == 0.0)
		{
			return false;
		}
		lineParam = ((((Point3d)(ref testPoint)).X - ((Point3d)(ref linePoint)).X) * ((Vector3d)(ref lineVector)).X + (((Point3d)(ref testPoint)).Y - ((Point3d)(ref linePoint)).Y) * ((Vector3d)(ref lineVector)).Y + (((Point3d)(ref testPoint)).Z - ((Point3d)(ref linePoint)).Z) * ((Vector3d)(ref lineVector)).Z) / length;
		return true;
	}
}
