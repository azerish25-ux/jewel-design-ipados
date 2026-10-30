using System.Collections.Generic;
using CSBlockUtil;
using CSCompatibility;
using CSDatas;
using RMA.OpenNURBS;

namespace CSBlockDataUtils;

public static class CSUtilProngPlacer
{
	public static bool CalculateCircleEdgeLeft(CSChannelGemData gemPrevious, CSChannelGemData gemCurrent, IOnCurve rail, double edgeNudge, out OnCircle circleEdgeLeft)
	{
		circleEdgeLeft = new OnCircle();
		if (!TrimRail(rail, gemPrevious.Left.Convert(), gemCurrent.Left.Convert(), out var railTrimmed))
		{
			return false;
		}
		On3dPoint on3dPoint = railTrimmed.PointAt(railTrimmed.Domain().Mid());
		On3dVector on3dVector = gemPrevious.LeftVector.Convert() + gemCurrent.LeftVector.Convert();
		on3dVector.Unitize();
		On3dPoint on3dPoint2 = on3dPoint + on3dVector * 10.0;
		On3dVector on3dVector2 = gemPrevious.Normal.Convert() + gemCurrent.Normal.Convert();
		on3dVector2.Unitize();
		OnPlane plane = new OnPlane(on3dPoint2, on3dVector2);
		double radius = on3dPoint.DistanceTo(on3dPoint2);
		circleEdgeLeft = new OnCircle(plane, on3dPoint2, radius);
		circleEdgeLeft.radius -= edgeNudge;
		return true;
	}

	public static bool CalculateCircleEdgeRight(CSChannelGemData gemPrevious, CSChannelGemData gemCurrent, IOnCurve rail, double edgeNudge, out OnCircle circleEdgeRight)
	{
		circleEdgeRight = new OnCircle();
		if (!TrimRail(rail, gemPrevious.Right.Convert(), gemCurrent.Right.Convert(), out var railTrimmed))
		{
			return false;
		}
		On3dPoint on3dPoint = railTrimmed.PointAt(railTrimmed.Domain().Mid());
		On3dVector on3dVector = gemPrevious.RightVector.Convert() + gemCurrent.RightVector.Convert();
		on3dVector.Unitize();
		On3dPoint on3dPoint2 = on3dPoint + on3dVector * 10.0;
		On3dVector on3dVector2 = gemPrevious.Normal.Convert() + gemCurrent.Normal.Convert();
		on3dVector2.Unitize();
		OnPlane plane = new OnPlane(on3dPoint2, on3dVector2);
		double radius = on3dPoint.DistanceTo(on3dPoint2);
		circleEdgeRight = new OnCircle(plane, on3dPoint2, radius);
		circleEdgeRight.radius -= edgeNudge;
		return true;
	}

	private static bool TrimRail(IOnCurve rail, IOn3dPoint pt0, IOn3dPoint pt1, out OnCurve railTrimmed)
	{
		railTrimmed = rail.DuplicateCurve();
		railTrimmed.Domain().MakeIncreasing();
		double t = -1.23432101234321E+308;
		if (!railTrimmed.GetClosestPoint(pt0, ref t))
		{
			return false;
		}
		double t2 = -1.23432101234321E+308;
		if (!railTrimmed.GetClosestPoint(pt1, ref t2))
		{
			return false;
		}
		if (t < t2)
		{
			return railTrimmed.Trim(new OnInterval(t, t2));
		}
		if (!railTrimmed.IsClosed())
		{
			return false;
		}
		List<IOnCurve> list = new List<IOnCurve>();
		OnCurve onCurve = railTrimmed.Duplicate();
		if (onCurve.Trim(new OnInterval(t, onCurve.Domain().Max())))
		{
			list.Add(onCurve);
		}
		OnCurve onCurve2 = railTrimmed.Duplicate();
		if (onCurve2.Trim(new OnInterval(onCurve2.Domain().Min(), t2)))
		{
			list.Add(onCurve2);
		}
		return CSUtilCurves.MergeCurves(list.ToArray(), out railTrimmed, 0.01);
	}
}
