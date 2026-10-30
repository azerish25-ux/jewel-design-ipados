using System;
using Rhino.Geometry;

namespace CSBlocks.Surfaces;

public static class MicroPaveProng
{
	public static readonly ControlPoint[,] Cv;

	static MicroPaveProng()
	{
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
		for (int i = 0; i < 6; i++)
		{
			for (int j = 0; j < 7; j++)
			{
				Cv[i, 8 + j] = new ControlPoint(Cv[i, 6 - j].Location.X, 0.0 - Cv[i, 6 - j].Location.Y, Cv[i, 6 - j].Location.Z, Cv[i, 6 - j].Weight);
			}
		}
		for (int k = 0; k < 5; k++)
		{
			for (int l = 0; l < 15; l++)
			{
				Cv[10 - k, l] = new ControlPoint(5.5 - Cv[k, l].Location.X, Cv[k, l].Location.Y, Cv[k, l].Location.Z, Cv[k, l].Weight);
			}
		}
		for (int m = 0; m < 11; m++)
		{
			for (int n = 0; n < 15; n++)
			{
				Cv[m, n] = new ControlPoint(Cv[m, n].Location.X / 5.5, Cv[m, n].Location.Y / 2.46356274147353, (Cv[m, n].Location.Z - 1.183) / 3.603, Cv[m, n].Weight);
			}
		}
	}

	public static bool PointOnLineAtDistanceFromPoint(Point3d linePoint, Vector3d lineVector, Point3d testPoint, double distanceOfPointNeeded, ref double lineParamOfClosestPoint, ref double paramAlongLine)
	{
		if (!ClosestPointParam(linePoint, lineVector, testPoint, ref lineParamOfClosestPoint))
		{
			return false;
		}
		Point3d point3d = linePoint + lineVector * lineParamOfClosestPoint;
		double num = distanceOfPointNeeded * distanceOfPointNeeded;
		double squareLength = (point3d - testPoint).SquareLength;
		if (num < squareLength)
		{
			return false;
		}
		paramAlongLine = Math.Sqrt(num - squareLength);
		return true;
	}

	private static bool ClosestPointParam(Point3d linePoint, Vector3d lineVector, Point3d testPoint, ref double lineParam)
	{
		double length = lineVector.Length;
		if (length == 0.0)
		{
			return false;
		}
		lineParam = ((testPoint.X - linePoint.X) * lineVector.X + (testPoint.Y - linePoint.Y) * lineVector.Y + (testPoint.Z - linePoint.Z) * lineVector.Z) / length;
		return true;
	}
}
