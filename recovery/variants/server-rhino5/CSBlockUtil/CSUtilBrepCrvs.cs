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
		//IL_000b: Unknown result type (might be due to invalid IL or missing references)
		//IL_002b: Expected O, but got Unknown
		if (srf == null)
		{
			return null;
		}
		return Interop.FromOnCurve(GetCurveFromBrepEdges((IOnBrep)(OnBrep)Interop.ToOnBrep(srf), CSConvert.Point3dCopyToOn3dPoint(ptStart), CSConvert.Point3dCopyToOn3dPoint(ptEnd), CSConvert.Point3dCopyToOn3dPoint(ptThird), isThirdPointMid, tolerance));
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
		On3dPoint val = ptStart;
		On3dPoint val2 = ptStart;
		double num2 = 0.0;
		int num3 = 0;
		while (crvs.Count > 0)
		{
			num3 %= crvs.Count;
			if (!CheckTolerance((IOn3dPoint)(object)val, (IOn3dPoint)(object)ptEnd, tolerance))
			{
				if (CheckTolerance((IOn3dPoint)(object)crvs[num3].PointAtStart(), (IOn3dPoint)(object)val, tolerance))
				{
					list.Add(crvs[num3]);
					if (crvs[num3].GetClosestPoint((IOn3dPoint)(object)ptThird, ref num2, tolerance))
					{
						num = 1;
					}
					val = crvs[num3].PointAtEnd();
					crvs.RemoveAt(num3);
					continue;
				}
				if (CheckTolerance((IOn3dPoint)(object)crvs[num3].PointAtEnd(), (IOn3dPoint)(object)val, tolerance))
				{
					list.Add(crvs[num3]);
					if (crvs[num3].GetClosestPoint((IOn3dPoint)(object)ptThird, ref num2, tolerance))
					{
						num = 1;
					}
					val = crvs[num3].PointAtStart();
					crvs.RemoveAt(num3);
					continue;
				}
			}
			if (!CheckTolerance((IOn3dPoint)(object)val2, (IOn3dPoint)(object)ptEnd, tolerance))
			{
				if (CheckTolerance((IOn3dPoint)(object)crvs[num3].PointAtStart(), (IOn3dPoint)(object)val2, tolerance))
				{
					list2.Add(crvs[num3]);
					if (crvs[num3].GetClosestPoint((IOn3dPoint)(object)ptThird, ref num2, tolerance))
					{
						num = 2;
					}
					val2 = crvs[num3].PointAtEnd();
					crvs.RemoveAt(num3);
					continue;
				}
				if (CheckTolerance((IOn3dPoint)(object)crvs[num3].PointAtEnd(), (IOn3dPoint)(object)val2, tolerance))
				{
					list2.Add(crvs[num3]);
					if (crvs[num3].GetClosestPoint((IOn3dPoint)(object)ptThird, ref num2, tolerance))
					{
						num = 2;
					}
					val2 = crvs[num3].PointAtStart();
					crvs.RemoveAt(num3);
					continue;
				}
			}
			num3++;
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
		OnCurve val3;
		if (array.Length == 1)
		{
			val3 = list[0];
		}
		else
		{
			OnCurve[] array2 = null;
			IOnCurve[] array3 = (IOnCurve[])(object)list.ToArray();
			RhUtil.RhinoMergeCurves(array3, ref array2);
			val3 = array2[0];
		}
		if (!CheckTolerance((IOn3dPoint)(object)val3.PointAtStart(), (IOn3dPoint)(object)ptStart))
		{
			val3.Reverse();
		}
		return val3;
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
			IOnCurve[] inputCrvs = (IOnCurve[])(object)crvs.ToArray();
			if (!CSUtilCurves.MergeCurves(inputCrvs, out edge, CSUtil.Tolerance))
			{
				return false;
			}
		}
		double num = -1.23432101234321E+308;
		if (!edge.GetClosestPoint((IOn3dPoint)(object)ptStart, ref num))
		{
			return false;
		}
		if (!edge.ChangeClosedCurveSeam(num))
		{
			return false;
		}
		double num2 = -1.23432101234321E+308;
		if (!edge.GetClosestPoint((IOn3dPoint)(object)ptEnd, ref num2))
		{
			return false;
		}
		double num3 = -1.23432101234321E+308;
		if (!edge.GetClosestPoint((IOn3dPoint)(object)ptThird, ref num3))
		{
			return false;
		}
		if (num3 < num2)
		{
			if (!CSUtilCurves.TrimCurve((IOnCurve)(object)edge, num2, edge.Domain().Max(), out edge))
			{
				return false;
			}
			edge.Reverse();
		}
		else if (!CSUtilCurves.TrimCurve((IOnCurve)(object)edge, edge.Domain().Min(), num2, out edge))
		{
			return false;
		}
		return true;
	}
}
