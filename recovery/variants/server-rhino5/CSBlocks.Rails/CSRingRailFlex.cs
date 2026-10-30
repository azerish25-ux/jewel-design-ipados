using System;
using System.Collections.Generic;
using CS;
using CS.CSObjects.CSBlocks;
using CS.Parametric;
using CS.Parametric.Channels;
using CSBlockUtil;
using CSCompatibility;
using CSDatas;
using GV.CounterSketch.Shared;
using RMA.OpenNURBS;
using Rhino.Geometry;

namespace CSBlocks.Rails;

[CSBlock("RingRailFlex", "{41C74FFD-B2D0-4C4D-BAFE-018FC015AE42}")]
public sealed class CSRingRailFlex : CSBlock
{
	public CSInputChannel InPlanarRingRail { get; }

	public CSInputChannel InPlanarOutsideRingRail { get; }

	public CSInputChannel InCenterFeatureObjects { get; }

	public CSInputChannel InShoulderStyle { get; }

	public CSInputChannel InShoulderTopThickness { get; }

	public CSInputChannel InShoulderTopOffsetX { get; }

	public CSInputChannel InShoulderTopOffsetZ { get; }

	public CSInputChannel InShoulderTopMeanCurveAmount { get; }

	public CSInputChannel InShoulderTopAngleOffset { get; }

	public CSInputChannel InShoulderBottomMeanCurveAmount { get; }

	public CSInputChannel InShoulderBottomAngleOffset { get; }

	public CSInputChannel InShoulderBlendAmount { get; }

	public CSOutputChannel OutRingRailFlexInside { get; }

	public CSOutputChannel OutRingRailFlexOutside { get; }

	public CSRingRailFlex(string identifier)
		: base(identifier)
	{
		InPlanarRingRail = new CSInputChannel(GetPropertyName(() => InPlanarRingRail), this, ChannelDataType.RingRail, ChannelArrayType.Individual);
		InPlanarOutsideRingRail = new CSInputChannel(GetPropertyName(() => InPlanarOutsideRingRail), this, ChannelDataType.OutsideRingRail, ChannelArrayType.Individual);
		InCenterFeatureObjects = new CSInputChannel(GetPropertyName(() => InCenterFeatureObjects), this, ChannelDataType.Any, ChannelArrayType.Any);
		InShoulderStyle = new CSInputChannel(GetPropertyName(() => InShoulderStyle), this, ChannelDataType.Integer, ChannelArrayType.Individual);
		InShoulderTopThickness = new CSInputChannel(GetPropertyName(() => InShoulderTopThickness), this, ChannelDataType.Double, ChannelArrayType.Individual);
		InShoulderTopOffsetX = new CSInputChannel(GetPropertyName(() => InShoulderTopOffsetX), this, ChannelDataType.Double, ChannelArrayType.Individual);
		InShoulderTopOffsetZ = new CSInputChannel(GetPropertyName(() => InShoulderTopOffsetZ), this, ChannelDataType.Double, ChannelArrayType.Individual);
		InShoulderTopMeanCurveAmount = new CSInputChannel(GetPropertyName(() => InShoulderTopMeanCurveAmount), this, ChannelDataType.Double, ChannelArrayType.Individual);
		InShoulderTopAngleOffset = new CSInputChannel(GetPropertyName(() => InShoulderTopAngleOffset), this, ChannelDataType.Double, ChannelArrayType.Individual);
		InShoulderBottomMeanCurveAmount = new CSInputChannel(GetPropertyName(() => InShoulderBottomMeanCurveAmount), this, ChannelDataType.Double, ChannelArrayType.Individual);
		InShoulderBottomAngleOffset = new CSInputChannel(GetPropertyName(() => InShoulderBottomAngleOffset), this, ChannelDataType.Double, ChannelArrayType.Individual);
		InShoulderBlendAmount = new CSInputChannel(GetPropertyName(() => InShoulderBlendAmount), this, ChannelDataType.Double, ChannelArrayType.Individual);
		OutRingRailFlexInside = new CSOutputChannel(GetPropertyName(() => OutRingRailFlexInside), this, ChannelDataType.Curve, ChannelArrayType.Individual);
		OutRingRailFlexOutside = new CSOutputChannel(GetPropertyName(() => OutRingRailFlexOutside), this, ChannelDataType.Curve, ChannelArrayType.Individual);
	}

	public override bool Update()
	{
		//IL_013f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0146: Expected O, but got Unknown
		//IL_0262: Unknown result type (might be due to invalid IL or missing references)
		//IL_0269: Expected O, but got Unknown
		//IL_027d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0284: Expected O, but got Unknown
		//IL_029b: Unknown result type (might be due to invalid IL or missing references)
		//IL_02a2: Expected O, but got Unknown
		//IL_021f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0226: Expected O, but got Unknown
		//IL_022b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0232: Expected O, but got Unknown
		//IL_0246: Unknown result type (might be due to invalid IL or missing references)
		//IL_024d: Expected O, but got Unknown
		//IL_0357: Unknown result type (might be due to invalid IL or missing references)
		//IL_035e: Expected O, but got Unknown
		//IL_0426: Unknown result type (might be due to invalid IL or missing references)
		//IL_042d: Expected O, but got Unknown
		//IL_04b8: Unknown result type (might be due to invalid IL or missing references)
		//IL_04bf: Expected O, but got Unknown
		//IL_04c1: Unknown result type (might be due to invalid IL or missing references)
		//IL_04c8: Expected O, but got Unknown
		//IL_05f3: Unknown result type (might be due to invalid IL or missing references)
		//IL_0606: Expected O, but got Unknown
		if (!(InPlanarRingRail.Data is CSRingRailData cSRingRailData))
		{
			return UpdateMissing("Missing Planar Ring Rail");
		}
		if (!(InPlanarOutsideRingRail.Data is CSOutsideRingRailData cSOutsideRingRailData))
		{
			return UpdateMissing("Missing Planar Outside Ring Rail");
		}
		CSDataArray flattenArray = InCenterFeatureObjects.GetFlattenArray();
		ValueListItem whichItem = ValueListItem.RingRailFlexShoulderStyleRegular;
		InShoulderStyle.GetValueListItem(ref whichItem);
		double value = 1.8;
		InShoulderTopThickness.GetDouble(ref value);
		double value2 = 2.0;
		InShoulderTopOffsetX.GetDouble(ref value2);
		double value3 = 2.0;
		InShoulderTopOffsetZ.GetDouble(ref value3);
		double value4 = 0.5;
		InShoulderTopMeanCurveAmount.GetDouble(ref value4);
		double value5 = 0.0;
		InShoulderTopAngleOffset.GetDouble(ref value5);
		double value6 = 0.5;
		InShoulderBottomMeanCurveAmount.GetDouble(ref value6);
		double value7 = 0.0;
		InShoulderBottomAngleOffset.GetDouble(ref value7);
		double value8 = 1.0;
		InShoulderBlendAmount.GetDouble(ref value8);
		OnNurbsCurve val = cSRingRailData.GetNurbsCurve().Convert();
		OnNurbsCurve val2 = cSOutsideRingRailData.GetNurbsCurve().Convert();
		OnXform val3 = new OnXform();
		val3.Mirror(OnUtil.On_origin, OnUtil.On_xaxis);
		if (!CSUtilRingRail.GetRingRailPointExtentRight((IOnCurve)(object)val2, out var pointExtent))
		{
			return UpdateError("GetRingRailPointExtentRight failed");
		}
		if (!CSUtilRingRail.GetRingRailPointExtentTop((IOnCurve)(object)val2, out var pointExtent2))
		{
			return UpdateError("GetRingRailPointExtentTop failed");
		}
		double num;
		double num2;
		if (flattenArray.GetArrayCount() <= 0)
		{
			num = 0.0;
			num2 = pointExtent2;
		}
		else
		{
			BoundingBox boundingBox = BoundingBox.Unset;
			if (!flattenArray.GetBoundingBox(ref boundingBox))
			{
				return UpdateError("GetBoundingBox failed");
			}
			num = Math.Max(Math.Abs(boundingBox.Max.X), Math.Abs(boundingBox.Min.X));
			num2 = boundingBox.Center.Z;
		}
		On3dPoint val4;
		On3dPoint val6;
		On3dPoint val5;
		switch (whichItem)
		{
		case ValueListItem.RingRailFlexShoulderStyleInverted:
			val4 = new On3dPoint(num + value2, 0.0, num2 + value3);
			val6 = new On3dPoint(OnUtil.On_origin);
			val5 = new On3dPoint(pointExtent, 0.0, 0.0);
			break;
		case ValueListItem.RingRailFlexShoulderStyleRegular:
			val4 = new On3dPoint(num + value2, 0.0, num2 + value3);
			val5 = new On3dPoint(pointExtent, 0.0, 0.0);
			val6 = new On3dPoint(val5.x, 0.0, val4.z);
			break;
		default:
			return UpdateError("Invalid Shoulder Style");
		}
		On3dVector val7;
		On3dVector val10;
		switch (whichItem)
		{
		case ValueListItem.RingRailFlexShoulderStyleInverted:
		{
			double num4 = val4.DistanceTo((IOn3dPoint)(object)val5);
			On3dVector val11 = val4 - (IOn3dPoint)(object)val5;
			val11.Unitize();
			val7 = val5 + (IOn3dVector)(object)(val11 * num4 * value6) - (IOn3dPoint)(object)val6;
			val7.Unitize();
			double num5 = val6.DistanceTo((IOn3dPoint)(object)val5);
			On3dVector val12 = val5 - (IOn3dPoint)(object)val6;
			val12.Unitize();
			On3dPoint val13 = val6 + (IOn3dVector)(object)(val12 * num5 * value4);
			val10 = val4 - (IOn3dPoint)(object)val13;
			val10.Unitize();
			break;
		}
		case ValueListItem.RingRailFlexShoulderStyleRegular:
		{
			val7 = new On3dVector(OnUtil.On_xaxis);
			double num3 = val6.DistanceTo((IOn3dPoint)(object)val5);
			On3dVector val8 = val6 - (IOn3dPoint)(object)val5;
			val8.Unitize();
			On3dPoint val9 = val5 + (IOn3dVector)(object)(val8 * num3 * value4);
			val10 = val4 - (IOn3dPoint)(object)val9;
			val10.Unitize();
			break;
		}
		default:
			return UpdateError("Invalid Shoulder Style");
		}
		double num6 = CSBlockUtil.CSUtilMath.ToDegrees(CSBlockUtil.CSUtilMath.AngleBetweenVectors((IOn3dVector)(object)val7, OnUtil.On_zaxis)) - value7;
		if (num6 <= 0.0)
		{
			num6 = 0.0 + CSApplicationHelper.Application.Tolerance / 2.0;
		}
		if (num6 >= 180.0)
		{
			num6 = 180.0 - CSApplicationHelper.Application.Tolerance / 2.0;
		}
		OnBoundingBox val14 = new OnBoundingBox();
		if (!((OnGeometry)val2).GetBoundingBox(ref val14))
		{
			return UpdateError("GetBoundingBox failed");
		}
		double radiusMax = val14.Diagonal().LengthSquared();
		OnNurbsCurve val15 = val2.Duplicate();
		if (!TrimRingRail((OnCurve)(object)val15, radiusMax, num6))
		{
			return UpdateError("TrimRingRail failed");
		}
		double num7 = ((OnCurve)val15).Domain().Min();
		IOn3dPoint pointEnd = (IOn3dPoint)(object)((OnCurve)val15).PointAt(num7);
		On3dVector val16 = ((OnCurve)val15).TangentAt(num7);
		val16.Unitize();
		double num8 = num + value2;
		double num9 = num2 + value3;
		On3dPoint val17 = new On3dPoint(num8, 0.0, num9);
		On3dVector val18 = new On3dVector((IOn3dVector)(object)val10);
		val18.Rotate(CSBlockUtil.CSUtilMath.ToRadians(0.0 - value5), OnUtil.On_yaxis);
		val18.Unitize();
		OnNurbsCurve val19 = CSUtilCurves.BlendCurveAdvanced((IOn3dPoint)(object)val17, pointEnd, (IOn3dVector)(object)(val18 * -1.0), (IOn3dVector)(object)(val16 * -1.0), 3, 3, 0.1, 0.53, value8, 0.35, 0.7, 1.386, doClean: true);
		OnNurbsCurve val20 = val19.Duplicate();
		((OnCurve)val20).Reverse();
		val20.Transform((IOnXform)(object)val3);
		if (!CSUtilCurves.MergeCurves(new List<IOnCurve>
		{
			(IOnCurve)(object)val15,
			(IOnCurve)(object)val19,
			(IOnCurve)(object)val20
		}.ToArray(), out OnCurve mergedCrv, CSApplicationHelper.Application.Tolerance))
		{
			return UpdateError("MergeCurves failed");
		}
		OnNurbsCurve val21 = val.Duplicate();
		if (!TrimRingRail((OnCurve)(object)val21, radiusMax, num6))
		{
			return UpdateError("TrimRingRail failed");
		}
		double num10 = ((OnCurve)val21).Domain().Min();
		IOn3dPoint pointEnd2 = (IOn3dPoint)(object)((OnCurve)val21).PointAt(num10);
		On3dVector val22 = ((OnCurve)val21).TangentAt(num10);
		val22.Unitize();
		On3dVector val23 = OnUtil.ON_CrossProduct((IOn3dVector)(object)val18, (IOn3dVector)(object)(new On3dVector(OnUtil.On_yaxis) * 1.0));
		On3dPoint pointStart = val17 + (IOn3dVector)(object)(val23 * value);
		On3dVector val24 = val18;
		OnNurbsCurve val25 = CSUtilCurves.BlendCurveAdvanced((IOn3dPoint)(object)pointStart, pointEnd2, (IOn3dVector)(object)(val24 * -1.0), (IOn3dVector)(object)(val22 * -1.0), 3, 3, 0.1, 0.53, value8, 0.35, 0.7, 1.386, doClean: true);
		OnNurbsCurve val26 = val25.Duplicate();
		((OnCurve)val26).Reverse();
		val26.Transform((IOnXform)(object)val3);
		if (!CSUtilCurves.MergeCurves(new List<IOnCurve>
		{
			(IOnCurve)(object)val21,
			(IOnCurve)(object)val25,
			(IOnCurve)(object)val26
		}.ToArray(), out OnCurve mergedCrv2, CSApplicationHelper.Application.Tolerance))
		{
			return UpdateError("MergeCurves failed");
		}
		OutRingRailFlexInside.SetData(new CSOnGeometryData(mergedCrv2.Convert()));
		OutRingRailFlexOutside.SetData(new CSOnGeometryData(mergedCrv.Convert()));
		return base.Update();
	}

	private bool GetIntersection(IOnCurve ringRail, double rotationAngle, double radiusMax, out double param)
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0006: Expected O, but got Unknown
		//IL_0031: Unknown result type (might be due to invalid IL or missing references)
		//IL_003b: Expected O, but got Unknown
		//IL_0036: Unknown result type (might be due to invalid IL or missing references)
		//IL_003c: Expected O, but got Unknown
		OnXform val = new OnXform();
		val.Rotation(rotationAngle, OnUtil.On_yaxis, OnUtil.On_origin);
		OnLineCurve val2 = new OnLineCurve(OnUtil.On_origin, (IOn3dPoint)new On3dPoint(0.0, 0.0, radiusMax * radiusMax));
		val2.Transform((IOnXform)(object)val);
		if (CSBlockUtil.CSUtilIntersect.GetIntersectParam(ringRail, (IOnCurve)(object)val2, out param))
		{
			return true;
		}
		SetMessage("Intersection failed");
		return false;
	}

	private bool TrimRingRail(OnCurve ringRail, double radiusMax, double shoulderBottomAngle)
	{
		//IL_003f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0049: Expected O, but got Unknown
		double num = CSBlockUtil.CSUtilMath.ToRadians(shoulderBottomAngle);
		if (!GetIntersection((IOnCurve)(object)ringRail, num, radiusMax, out var param))
		{
			SetMessage("GetIntersection failed");
			return false;
		}
		if (!GetIntersection((IOnCurve)(object)ringRail, 0.0 - num, radiusMax, out var param2))
		{
			SetMessage("GetIntersection failed");
			return false;
		}
		if (!ringRail.Trim((IOnInterval)new OnInterval(param, param2)))
		{
			SetMessage("Trim failed");
			return false;
		}
		return true;
	}
}
