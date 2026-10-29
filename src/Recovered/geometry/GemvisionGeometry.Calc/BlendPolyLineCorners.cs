using System.Collections.Generic;
using GemvisionGeometry.Geometry;
using GemvisionGeometry.Helpers;

namespace GemvisionGeometry.Calc;

public static class BlendPolyLineCorners
{
	public static GNurbsCurve MakeBlendedCurve(GPolyline InLine, double cornerBlend, bool testStart)
	{
		bool flag = true;
		int num = InLine.PointCount - 1;
		GPoint3D[] array = new GPoint3D[num];
		GPoint3D[] array2 = new GPoint3D[num];
		for (int i = 0; i < num; i++)
		{
			GPoint3D gPoint3D = InLine[i];
			GPoint3D gPoint3D2 = InLine[i];
			GVector3D gVector3D = gPoint3D2 - gPoint3D;
			double length = gVector3D.Length;
			gVector3D.Unitize();
			double num2 = cornerBlend;
			if (flag && (i == 0 || i == num - 1))
			{
				if (num2 > length * 2.0 / 3.0)
				{
					num2 = length * 2.0 / 3.0;
				}
			}
			else if (num2 > length / 3.0)
			{
				num2 = length / 3.0;
			}
			gVector3D *= num2;
			array[i] = ((!flag || i != 0) ? (gPoint3D + gVector3D) : gPoint3D);
			array2[i] = ((!flag || i != num - 1) ? (gPoint3D2 - gVector3D) : gPoint3D2);
		}
		List<GNurbsCurve> list = new List<GNurbsCurve>();
		for (int j = 0; j < num; j++)
		{
			GVector3D gVector3D2 = array2[j] - array[j];
			list.Add(GCurveHelper.PointsToNurbs(new GPoint3D[4]
			{
				array[j],
				array[j] + gVector3D2 * 0.33333,
				array[j] + gVector3D2 * 0.66666,
				array2[j]
			}));
			if (j < num - 1)
			{
				GVector3D v = array[j + 1] - array2[j + 1];
				list.Add(BlendCurve.GetBlendCurve(array2[j], array[j + 1], gVector3D2, v));
			}
		}
		return GCurveHelper.JoinCurves(list);
	}
}
