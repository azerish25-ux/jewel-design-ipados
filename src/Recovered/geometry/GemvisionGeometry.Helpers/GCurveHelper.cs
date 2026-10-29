using System.Collections.Generic;
using GemvisionGeometry.Geometry;

namespace GemvisionGeometry.Helpers;

public static class GCurveHelper
{
	public static List<GNurbsCurve> Explode(GNurbsCurve originalCurve)
	{
		List<GNurbsCurve> list = new List<GNurbsCurve>();
		GNurbsCurve gNurbsCurve = new GNurbsCurve(originalCurve.Points.Count, originalCurve.Degree);
		int num = 0;
		foreach (GPoint4D point in originalCurve.Points)
		{
			gNurbsCurve.SetPoint(num, point);
			num++;
		}
		num = 0;
		foreach (double knot in originalCurve.Knots)
		{
			gNurbsCurve.SetKnot(num, knot);
			num++;
		}
		while (true)
		{
			double num2 = -1.0;
			int num3 = 1;
			int num4 = -1;
			for (int i = 0; i < gNurbsCurve.Knots.Count; i++)
			{
				if (i >= gNurbsCurve.Degree && i < gNurbsCurve.Knots.Count - gNurbsCurve.Degree)
				{
					num3 = ((num2 != gNurbsCurve.Knots[i]) ? 1 : (num3 + 1));
					if (num3 >= gNurbsCurve.Degree)
					{
						num4 = i;
						break;
					}
					num2 = gNurbsCurve.Knots[i];
				}
				else
				{
					num4 = -1;
				}
			}
			if (num4 <= 0)
			{
				break;
			}
			GNurbsCurve secondHalf;
			GNurbsCurve item = ExplodeCurve(num4, gNurbsCurve, out secondHalf);
			list.Add(item);
			gNurbsCurve = secondHalf;
		}
		list.Add(gNurbsCurve);
		return list;
	}

	private static GNurbsCurve ExplodeCurve(int splitIndex, GNurbsCurve nurbsCrv, out GNurbsCurve secondHalf)
	{
		GNurbsCurve gNurbsCurve = new GNurbsCurve(splitIndex - (nurbsCrv.Degree - 1), nurbsCrv.Degree);
		for (int i = 0; i < gNurbsCurve.Points.Count; i++)
		{
			gNurbsCurve.SetPoint(i, nurbsCrv.Points[i]);
		}
		for (int j = 0; j <= splitIndex; j++)
		{
			gNurbsCurve.SetKnot(j, nurbsCrv.Knots[j]);
		}
		gNurbsCurve.SetKnot(splitIndex + 1, nurbsCrv.Knots[splitIndex]);
		GNurbsCurve gNurbsCurve2 = new GNurbsCurve(nurbsCrv.Points.Count - (splitIndex - nurbsCrv.Degree), nurbsCrv.Degree);
		for (int k = 0; k < gNurbsCurve2.Points.Count; k++)
		{
			gNurbsCurve2.SetPoint(k, nurbsCrv.Points[splitIndex - nurbsCrv.Degree + k]);
		}
		gNurbsCurve2.SetKnot(0, nurbsCrv.Knots[splitIndex]);
		for (int l = 1; l < nurbsCrv.Knots.Count - splitIndex + nurbsCrv.Degree; l++)
		{
			gNurbsCurve2.SetKnot(l, nurbsCrv.Knots[splitIndex + l - nurbsCrv.Degree]);
		}
		secondHalf = gNurbsCurve2;
		return gNurbsCurve;
	}

	public static GNurbsCurve JoinCurves(List<GNurbsCurve> curveList)
	{
		List<GPoint4D> list = new List<GPoint4D>();
		int num = 0;
		double num2 = 0.0;
		List<double> list2 = new List<double>();
		bool flag = true;
		bool flag2 = false;
		for (int i = 0; i < curveList.Count; i++)
		{
			if (i == curveList.Count - 1)
			{
				flag2 = true;
			}
			GNurbsCurve gNurbsCurve = curveList[i];
			num = gNurbsCurve.Degree;
			for (int j = 0; j < gNurbsCurve.Points.Count - 1; j++)
			{
				GPoint4D item = gNurbsCurve.Points[j];
				list.Add(item);
			}
			if (flag2)
			{
				list.Add(gNurbsCurve.Points[gNurbsCurve.Points.Count - 1]);
			}
			if (flag)
			{
				for (int k = 0; k < num - 1; k++)
				{
					list2.Add(gNurbsCurve.Knots[0]);
				}
			}
			for (int l = num - 1; l < gNurbsCurve.Knots.Count - num; l++)
			{
				list2.Add(gNurbsCurve.Knots[l] + num2);
			}
			if (flag2)
			{
				for (int m = 0; m < num; m++)
				{
					list2.Add(gNurbsCurve.Knots[gNurbsCurve.Knots.Count - 1] + num2);
				}
			}
			if (flag)
			{
				_ = gNurbsCurve.Domain.Min;
			}
			num2 += gNurbsCurve.Domain.Max;
			flag = false;
		}
		GNurbsCurve gNurbsCurve2 = new GNurbsCurve(list.Count, num);
		for (int n = 0; n < list.Count; n++)
		{
			gNurbsCurve2.SetPoint(n, list[n]);
		}
		for (int num3 = 0; num3 < list2.Count; num3++)
		{
			gNurbsCurve2.SetKnot(num3, list2[num3]);
		}
		return gNurbsCurve2;
	}

	public static GNurbsCurve PointsToNurbs(GPoint3D[] points)
	{
		int num = points.Length;
		GNurbsCurve gNurbsCurve = new GNurbsCurve(num, 3);
		for (int i = 0; i < num; i++)
		{
			gNurbsCurve.SetPoint(i, new GPoint4D(points[i]));
		}
		gNurbsCurve.SetKnot(0, 0.0);
		gNurbsCurve.SetKnot(1, 0.0);
		gNurbsCurve.SetKnot(2, 0.0);
		gNurbsCurve.SetKnot(3, 0.0);
		int num2 = 0;
		for (int j = 0; j < num - 4; j++)
		{
			num2++;
			gNurbsCurve.SetKnot(j + 4, num2);
		}
		num2++;
		gNurbsCurve.SetKnot(num, num2);
		gNurbsCurve.SetKnot(num + 1, num2);
		gNurbsCurve.SetKnot(num + 2, num2);
		gNurbsCurve.SetKnot(num + 3, num2);
		return gNurbsCurve;
	}
}
