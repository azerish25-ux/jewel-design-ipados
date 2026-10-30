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
		//IL_0002: Unknown result type (might be due to invalid IL or missing references)
		//IL_0008: Expected O, but got Unknown
		//IL_000a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0015: Unknown result type (might be due to invalid IL or missing references)
		//IL_003c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0047: Unknown result type (might be due to invalid IL or missing references)
		//IL_0075: Unknown result type (might be due to invalid IL or missing references)
		//IL_0080: Unknown result type (might be due to invalid IL or missing references)
		//IL_0099: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a0: Expected O, but got Unknown
		//IL_00af: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b5: Expected O, but got Unknown
		circleEdgeLeft = new OnCircle();
		if (!TrimRail(rail, (IOn3dPoint)(object)gemPrevious.Left.Convert(), (IOn3dPoint)(object)gemCurrent.Left.Convert(), out var railTrimmed))
		{
			return false;
		}
		On3dPoint obj = railTrimmed.PointAt(railTrimmed.Domain().Mid());
		On3dVector val = gemPrevious.LeftVector.Convert() + (IOn3dVector)(object)gemCurrent.LeftVector.Convert();
		val.Unitize();
		On3dPoint val2 = obj + (IOn3dVector)(object)(val * 10.0);
		On3dVector val3 = gemPrevious.Normal.Convert() + (IOn3dVector)(object)gemCurrent.Normal.Convert();
		val3.Unitize();
		OnPlane val4 = new OnPlane((IOn3dPoint)(object)val2, (IOn3dVector)(object)val3);
		double num = obj.DistanceTo((IOn3dPoint)(object)val2);
		circleEdgeLeft = new OnCircle((IOnPlane)(object)val4, (IOn3dPoint)(object)val2, num);
		circleEdgeLeft.radius -= edgeNudge;
		return true;
	}

	public static bool CalculateCircleEdgeRight(CSChannelGemData gemPrevious, CSChannelGemData gemCurrent, IOnCurve rail, double edgeNudge, out OnCircle circleEdgeRight)
	{
		//IL_0002: Unknown result type (might be due to invalid IL or missing references)
		//IL_0008: Expected O, but got Unknown
		//IL_000a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0015: Unknown result type (might be due to invalid IL or missing references)
		//IL_003c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0047: Unknown result type (might be due to invalid IL or missing references)
		//IL_0075: Unknown result type (might be due to invalid IL or missing references)
		//IL_0080: Unknown result type (might be due to invalid IL or missing references)
		//IL_0099: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a0: Expected O, but got Unknown
		//IL_00af: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b5: Expected O, but got Unknown
		circleEdgeRight = new OnCircle();
		if (!TrimRail(rail, (IOn3dPoint)(object)gemPrevious.Right.Convert(), (IOn3dPoint)(object)gemCurrent.Right.Convert(), out var railTrimmed))
		{
			return false;
		}
		On3dPoint obj = railTrimmed.PointAt(railTrimmed.Domain().Mid());
		On3dVector val = gemPrevious.RightVector.Convert() + (IOn3dVector)(object)gemCurrent.RightVector.Convert();
		val.Unitize();
		On3dPoint val2 = obj + (IOn3dVector)(object)(val * 10.0);
		On3dVector val3 = gemPrevious.Normal.Convert() + (IOn3dVector)(object)gemCurrent.Normal.Convert();
		val3.Unitize();
		OnPlane val4 = new OnPlane((IOn3dPoint)(object)val2, (IOn3dVector)(object)val3);
		double num = obj.DistanceTo((IOn3dPoint)(object)val2);
		circleEdgeRight = new OnCircle((IOnPlane)(object)val4, (IOn3dPoint)(object)val2, num);
		circleEdgeRight.radius -= edgeNudge;
		return true;
	}

	private static bool TrimRail(IOnCurve rail, IOn3dPoint pt0, IOn3dPoint pt1, out OnCurve railTrimmed)
	{
		//IL_004d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0057: Expected O, but got Unknown
		//IL_0088: Unknown result type (might be due to invalid IL or missing references)
		//IL_0092: Expected O, but got Unknown
		//IL_00b4: Unknown result type (might be due to invalid IL or missing references)
		//IL_00be: Expected O, but got Unknown
		railTrimmed = rail.DuplicateCurve();
		railTrimmed.Domain().MakeIncreasing();
		double num = -1.23432101234321E+308;
		if (!railTrimmed.GetClosestPoint(pt0, ref num))
		{
			return false;
		}
		double num2 = -1.23432101234321E+308;
		if (!railTrimmed.GetClosestPoint(pt1, ref num2))
		{
			return false;
		}
		if (num < num2)
		{
			return railTrimmed.Trim((IOnInterval)new OnInterval(num, num2));
		}
		if (!railTrimmed.IsClosed())
		{
			return false;
		}
		List<IOnCurve> list = new List<IOnCurve>();
		OnCurve val = railTrimmed.Duplicate();
		if (val.Trim((IOnInterval)new OnInterval(num, val.Domain().Max())))
		{
			list.Add((IOnCurve)(object)val);
		}
		OnCurve val2 = railTrimmed.Duplicate();
		if (val2.Trim((IOnInterval)new OnInterval(val2.Domain().Min(), num2)))
		{
			list.Add((IOnCurve)(object)val2);
		}
		return CSUtilCurves.MergeCurves(list.ToArray(), out railTrimmed, 0.01);
	}
}
