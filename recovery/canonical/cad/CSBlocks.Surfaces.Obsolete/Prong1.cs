using System;
using Rhino.Geometry;

namespace CSBlocks.Surfaces.Obsolete;

public static class Prong1
{
	public static ControlPoint[,] cv;

	static Prong1()
	{
		cv = new ControlPoint[11, 15];
		cv[0, 0] = new ControlPoint(0.0, -2.46356274147353, -2.43716199288183, 1.0);
		cv[0, 1] = new ControlPoint(0.0, -2.46356274147353, -1.72287627859612, 1.0);
		cv[0, 2] = new ControlPoint(0.0, -2.46356274147353, -1.00859056431041, 1.0);
		cv[0, 3] = new ControlPoint(0.0, -2.35614659109482, -1.00859056431041, 1.0);
		cv[0, 4] = new ControlPoint(0.0, -1.46356274147353, -1.00859056431041, 1.0);
		cv[0, 5] = new ControlPoint(0.0, -0.963562741473529, -1.00859056431041, 1.0);
		cv[0, 6] = new ControlPoint(0.0, -0.463562741473529, -1.00859056431041, 1.0);
		cv[0, 7] = new ControlPoint(0.0, 0.0, -1.00859056431041, 1.0);
		cv[1, 0] = new ControlPoint(0.212, -2.46356274147353, -2.43716199288184, 1.0);
		cv[1, 1] = new ControlPoint(0.212, -2.46356274147353, -1.72287627859613, 1.0);
		cv[1, 2] = new ControlPoint(0.212, -2.46356274147353, -1.00859056431041, 1.0);
		cv[1, 3] = new ControlPoint(0.212, -2.35614659109482, -1.00859056431041, 1.0);
		cv[1, 4] = new ControlPoint(0.212, -1.46356274147353, -1.00859056431041, 1.0);
		cv[1, 5] = new ControlPoint(0.212, -0.963562741473533, -1.00859056431041, 1.0);
		cv[1, 6] = new ControlPoint(0.212, -0.463562741473535, -1.00859056431041, 1.0);
		cv[1, 7] = new ControlPoint(0.212, 0.0, -1.00859056431041, 1.0);
		cv[2, 0] = new ControlPoint(1.365, -2.46356274147353, -2.43569007868541, 1.0);
		cv[2, 1] = new ControlPoint(1.365, -2.46356274147353, -1.72140436439971, 1.0);
		cv[2, 2] = new ControlPoint(1.365, -2.46356274147353, -1.00711865011399, 1.0);
		cv[2, 3] = new ControlPoint(1.365, -2.35614659109482, -0.703109180164105, 1.0);
		cv[2, 4] = new ControlPoint(1.365, -1.46356274147353, -0.703109180164105, 1.0);
		cv[2, 5] = new ControlPoint(1.365, -0.963562741473529, -0.703109180164105, 1.0);
		cv[2, 6] = new ControlPoint(1.365, -0.463562741473531, -1.00711865011399, 1.0);
		cv[2, 7] = new ControlPoint(1.365, 0.0, -1.00711865011399, 1.0);
		cv[3, 0] = new ControlPoint(1.726, -2.46356274147353, -2.42857142857143, 1.0);
		cv[3, 1] = new ControlPoint(1.726, -2.46356274147353, -1.71428571428572, 1.0);
		cv[3, 2] = new ControlPoint(1.726, -2.46356274147353, -1.00000000000001, 1.0);
		cv[3, 3] = new ControlPoint(1.726, -2.43919065825261, 0.641613324503731, 1.0);
		cv[3, 4] = new ControlPoint(1.726, -1.46356274147354, 0.641613324503731, 1.0);
		cv[3, 5] = new ControlPoint(1.726, -0.963562741473535, 0.641613324503731, 1.0);
		cv[3, 6] = new ControlPoint(1.726, -0.463562741473535, -1.0, 1.0);
		cv[3, 7] = new ControlPoint(1.726, 0.0, -1.0, 1.0);
		cv[4, 0] = new ControlPoint(2.165, -2.46356274147353, -2.43351395287763, 1.0);
		cv[4, 1] = new ControlPoint(2.165, -2.46356274147353, -1.71922823859192, 1.0);
		cv[4, 2] = new ControlPoint(2.165, -2.46356274147353, -1.0049425243062, 1.0);
		cv[4, 3] = new ControlPoint(2.165, -2.4700922641073, 1.18384014090205, 1.0);
		cv[4, 4] = new ControlPoint(2.165, -1.46356274147354, 1.176081271134, 1.0);
		cv[4, 5] = new ControlPoint(2.165, -0.940722478920128, 1.18384014090205, 1.0);
		cv[4, 6] = new ControlPoint(2.165, -0.463562741473536, -1.00494252430619, 1.0);
		cv[4, 7] = new ControlPoint(2.165, 0.0, -1.00494252430619, 1.0);
		cv[5, 0] = new ControlPoint(2.75, -2.46356274147353, -2.42857142857143, 1.0);
		cv[5, 1] = new ControlPoint(2.75, -2.46356274147353, -1.71428571428572, 1.0);
		cv[5, 2] = new ControlPoint(2.75, -2.46356274147353, -1.00000000000001, 1.0);
		cv[5, 3] = new ControlPoint(2.75, -2.46356274147354, 1.20734734912864, 1.0);
		cv[5, 4] = new ControlPoint(2.75, -1.46356274147353, 1.20734734912864, 1.0);
		cv[5, 5] = new ControlPoint(2.75, -0.490722478920129, 1.20734734912864, 1.0);
		cv[5, 6] = new ControlPoint(2.75, -0.463562741473535, -1.0, 1.0);
		cv[5, 7] = new ControlPoint(2.75, 0.0, -1.0, 1.0);
		for (int i = 0; i < 6; i++)
		{
			for (int j = 0; j < 7; j++)
			{
				cv[i, 8 + j] = new ControlPoint(cv[i, 6 - j].Location.X, 0.0 - cv[i, 6 - j].Location.Y, cv[i, 6 - j].Location.Z, cv[i, 6 - j].Weight);
			}
		}
		for (int k = 0; k < 5; k++)
		{
			for (int l = 0; l < 15; l++)
			{
				cv[10 - k, l] = new ControlPoint(5.5 - cv[k, l].Location.X, cv[k, l].Location.Y, cv[k, l].Location.Z, cv[k, l].Weight);
			}
		}
		for (int m = 0; m < 11; m++)
		{
			for (int n = 0; n < 15; n++)
			{
				cv[m, n] = new ControlPoint(cv[m, n].Location.X / 5.5, cv[m, n].Location.Y / 2.46356274147353, cv[m, n].Location.Z - 0.3283374965306689, cv[m, n].Weight);
			}
		}
	}

	public static bool ClosestPointParam(Point3d linePoint, Vector3d lineVector, Point3d testPoint, ref double lineParam)
	{
		double length = lineVector.Length;
		if (length == 0.0)
		{
			return false;
		}
		lineParam = ((testPoint.X - linePoint.X) * lineVector.X + (testPoint.Y - linePoint.Y) * lineVector.Y + (testPoint.Z - linePoint.Z) * lineVector.Z) / length;
		return true;
	}

	public static void GetPointOnLine(Point3d linePoint, Vector3d lineVector, double lineParam, ref Point3d point)
	{
		point = linePoint + lineVector * lineParam;
	}

	public static bool PointOnLineAtDistanceFromPoint(Point3d linePoint, Vector3d lineVector, Point3d testPoint, double distanceOfPointNeeded, ref double lineParamOfClosestPoint, ref double paramAlongLine)
	{
		if (!ClosestPointParam(linePoint, lineVector, testPoint, ref lineParamOfClosestPoint))
		{
			return false;
		}
		Point3d point = default(Point3d);
		GetPointOnLine(linePoint, lineVector, lineParamOfClosestPoint, ref point);
		double num = distanceOfPointNeeded * distanceOfPointNeeded;
		double squareLength = (point - testPoint).SquareLength;
		if (num < squareLength)
		{
			return false;
		}
		paramAlongLine = Math.Sqrt(num - squareLength);
		return true;
	}
}
