using System.Collections.Generic;
using CSCompatibility;
using RMA.OpenNURBS;
using RMA.Rhino;
using Rhino.Geometry;
using Rhino.Runtime;

namespace CSBlockUtil;

public static class CSUtilBrepCrvs
{
	public static Curve GetCurveFromBrepEdges(Brep srf, Point3d ptStart, Point3d ptEnd, Point3d ptThird, bool isThirdPointMid = true, double tolerance = 0.001)
	{
		if (srf == null)
		{
			return null;
		}
		return Interop.FromOnCurve(GetCurveFromBrepEdges((OnBrep)Interop.ToOnBrep(srf), CSConvert.Point3dCopyToOn3dPoint(ptStart), CSConvert.Point3dCopyToOn3dPoint(ptEnd), CSConvert.Point3dCopyToOn3dPoint(ptThird), isThirdPointMid, tolerance));
	}

	public static OnCurve GetCurveFromBrepEdges(IOnBrep srf, On3dPoint ptStart, On3dPoint ptEnd, On3dPoint ptThird, bool isThirdPointMid = true, double tolerance = 0.001)
	{
		return GetCurveFromCurveList(new List<OnCurve>(CSUtilGeometry.NakedEdges(srf)), ptStart, ptEnd, ptThird, isThirdPointMid, tolerance);
	}

	public static bool GetCurveFromBrepEdges_V2(IOnBrep srf, On3dPoint ptStart, On3dPoint ptEnd, On3dPoint ptThird, out OnCurve edge)
	{
		return GetCurveFromCurveList_V2(new List<OnCurve>(CSUtilGeometry.NakedEdges(srf)), ptStart, ptEnd, ptThird, out edge);
	}

	private static bool CheckTolerance(IOn3dPoint pt1, IOn3dPoint pt2, double tolerance = 0.001)
	{
		return pt1.DistanceTo(pt2) < tolerance;
	}

	private static OnCurve GetCurveFromCurveList(List<OnCurve> crvs, On3dPoint ptStart, On3dPoint ptEnd, On3dPoint ptThird, bool isThirdPointMid = true, double tolerance = 0.001)
	{
		List<OnCurve> list = new List<OnCurve>();
		List<OnCurve> list2 = new List<OnCurve>();
		int num = -1;
		On3dPoint on3dPoint = ptStart;
		On3dPoint on3dPoint2 = ptStart;
		double t = 0.0;
		int num2 = 0;
		while (crvs.Count > 0)
		{
			num2 %= crvs.Count;
			if (!CheckTolerance(on3dPoint, ptEnd, tolerance))
			{
				if (CheckTolerance(crvs[num2].PointAtStart(), on3dPoint, tolerance))
				{
					list.Add(crvs[num2]);
					if (crvs[num2].GetClosestPoint(ptThird, ref t, tolerance))
					{
						num = 1;
					}
					on3dPoint = crvs[num2].PointAtEnd();
					crvs.RemoveAt(num2);
					continue;
				}
				if (CheckTolerance(crvs[num2].PointAtEnd(), on3dPoint, tolerance))
				{
					list.Add(crvs[num2]);
					if (crvs[num2].GetClosestPoint(ptThird, ref t, tolerance))
					{
						num = 1;
					}
					on3dPoint = crvs[num2].PointAtStart();
					crvs.RemoveAt(num2);
					continue;
				}
			}
			if (!CheckTolerance(on3dPoint2, ptEnd, tolerance))
			{
				if (CheckTolerance(crvs[num2].PointAtStart(), on3dPoint2, tolerance))
				{
					list2.Add(crvs[num2]);
					if (crvs[num2].GetClosestPoint(ptThird, ref t, tolerance))
					{
						num = 2;
					}
					on3dPoint2 = crvs[num2].PointAtEnd();
					crvs.RemoveAt(num2);
					continue;
				}
				if (CheckTolerance(crvs[num2].PointAtEnd(), on3dPoint2, tolerance))
				{
					list2.Add(crvs[num2]);
					if (crvs[num2].GetClosestPoint(ptThird, ref t, tolerance))
					{
						num = 2;
					}
					on3dPoint2 = crvs[num2].PointAtStart();
					crvs.RemoveAt(num2);
					continue;
				}
			}
			num2++;
		}
		OnCurve[] array = null;
		if (isThirdPointMid)
		{
			if (num == 1)
			{
				array = list.ToArray();
			}
			if (num == 2)
			{
				array = list2.ToArray();
			}
		}
		else
		{
			if (num == 1)
			{
				array = list2.ToArray();
			}
			if (num == 2)
			{
				array = list.ToArray();
			}
		}
		if (array == null)
		{
			return null;
		}
		OnCurve onCurve;
		if (array.Length == 1)
		{
			onCurve = list[0];
		}
		else
		{
			OnCurve[] output = null;
			IOnCurve[] input_curves = list.ToArray();
			RhUtil.RhinoMergeCurves(input_curves, ref output);
			onCurve = output[0];
		}
		if (!CheckTolerance(onCurve.PointAtStart(), ptStart))
		{
			onCurve.Reverse();
		}
		return onCurve;
	}

	private static bool GetCurveFromCurveList_V2(List<OnCurve> crvs, On3dPoint ptStart, On3dPoint ptEnd, On3dPoint ptThird, out OnCurve edge)
	{
		edge = null;
		if (crvs.Count <= 0)
		{
			return false;
		}
		if (crvs.Count == 1)
		{
			edge = crvs[0].DuplicateCurve();
		}
		else
		{
			IOnCurve[] inputCrvs = crvs.ToArray();
			if (!CSUtilCurves.MergeCurves(inputCrvs, out edge, CSUtil.Tolerance))
			{
				return false;
			}
		}
		double t = -1.23432101234321E+308;
		if (!edge.GetClosestPoint(ptStart, ref t))
		{
			return false;
		}
		if (!edge.ChangeClosedCurveSeam(t))
		{
			return false;
		}
		double t2 = -1.23432101234321E+308;
		if (!edge.GetClosestPoint(ptEnd, ref t2))
		{
			return false;
		}
		double t3 = -1.23432101234321E+308;
		if (!edge.GetClosestPoint(ptThird, ref t3))
		{
			return false;
		}
		if (t3 < t2)
		{
			if (!CSUtilCurves.TrimCurve(edge, t2, edge.Domain().Max(), out edge))
			{
				return false;
			}
			edge.Reverse();
		}
		else if (!CSUtilCurves.TrimCurve(edge, edge.Domain().Min(), t2, out edge))
		{
			return false;
		}
		return true;
	}
}
