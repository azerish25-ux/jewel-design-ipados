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
		//IL_019f: Unknown result type (might be due to invalid IL or missing references)
		//IL_01a4: Unknown result type (might be due to invalid IL or missing references)
		//IL_01be: Unknown result type (might be due to invalid IL or missing references)
		//IL_01c3: Unknown result type (might be due to invalid IL or missing references)
		//IL_01d3: Unknown result type (might be due to invalid IL or missing references)
		//IL_01d8: Unknown result type (might be due to invalid IL or missing references)
		//IL_01ef: Unknown result type (might be due to invalid IL or missing references)
		//IL_01f4: Unknown result type (might be due to invalid IL or missing references)
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
			Point3d val4 = ((BoundingBox)(ref boundingBox)).Max;
			double val5 = Math.Abs(((Point3d)(ref val4)).X);
			val4 = ((BoundingBox)(ref boundingBox)).Min;
			num = Math.Max(val5, Math.Abs(((Point3d)(ref val4)).X));
			val4 = ((BoundingBox)(ref boundingBox)).Center;
			num2 = ((Point3d)(ref val4)).Z;
		}
		On3dPoint val6;
		On3dPoint val8;
		On3dPoint val7;
		switch (whichItem)
		{
		case ValueListItem.RingRailFlexShoulderStyleInverted:
			val6 = new On3dPoint(num + value2, 0.0, num2 + value3);
			val8 = new On3dPoint(OnUtil.On_origin);
			val7 = new On3dPoint(pointExtent, 0.0, 0.0);
			break;
		case ValueListItem.RingRailFlexShoulderStyleRegular:
			val6 = new On3dPoint(num + value2, 0.0, num2 + value3);
			val7 = new On3dPoint(pointExtent, 0.0, 0.0);
			val8 = new On3dPoint(val7.x, 0.0, val6.z);
			break;
		default:
			return UpdateError("Invalid Shoulder Style");
		}
		On3dVector val9;
		On3dVector val12;
		switch (whichItem)
		{
		case ValueListItem.RingRailFlexShoulderStyleInverted:
		{
			double num4 = val6.DistanceTo((IOn3dPoint)(object)val7);
			On3dVector val13 = val6 - (IOn3dPoint)(object)val7;
			val13.Unitize();
			val9 = val7 + (IOn3dVector)(object)(val13 * num4 * value6) - (IOn3dPoint)(object)val8;
			val9.Unitize();
			double num5 = val8.DistanceTo((IOn3dPoint)(object)val7);
			On3dVector val14 = val7 - (IOn3dPoint)(object)val8;
			val14.Unitize();
			On3dPoint val15 = val8 + (IOn3dVector)(object)(val14 * num5 * value4);
			val12 = val6 - (IOn3dPoint)(object)val15;
			val12.Unitize();
			break;
		}
		case ValueListItem.RingRailFlexShoulderStyleRegular:
		{
			val9 = new On3dVector(OnUtil.On_xaxis);
			double num3 = val8.DistanceTo((IOn3dPoint)(object)val7);
			On3dVector val10 = val8 - (IOn3dPoint)(object)val7;
			val10.Unitize();
			On3dPoint val11 = val7 + (IOn3dVector)(object)(val10 * num3 * value4);
			val12 = val6 - (IOn3dPoint)(object)val11;
			val12.Unitize();
			break;
		}
		default:
			return UpdateError("Invalid Shoulder Style");
		}
		double num6 = CSBlockUtil.CSUtilMath.ToDegrees(CSBlockUtil.CSUtilMath.AngleBetweenVectors((IOn3dVector)(object)val9, OnUtil.On_zaxis)) - value7;
		if (num6 <= 0.0)
		{
			num6 = 0.0 + CSApplicationHelper.Application.Tolerance / 2.0;
		}
		if (num6 >= 180.0)
		{
			num6 = 180.0 - CSApplicationHelper.Application.Tolerance / 2.0;
		}
		OnBoundingBox val16 = new OnBoundingBox();
		if (!((OnGeometry)val2).GetBoundingBox(ref val16))
		{
			return UpdateError("GetBoundingBox failed");
		}
		double radiusMax = val16.Diagonal().LengthSquared();
		OnNurbsCurve val17 = val2.Duplicate();
		if (!TrimRingRail((OnCurve)(object)val17, radiusMax, num6))
		{
			return UpdateError("TrimRingRail failed");
		}
		double num7 = ((OnCurve)val17).Domain().Min();
		IOn3dPoint pointEnd = (IOn3dPoint)(object)((OnCurve)val17).PointAt(num7);
		On3dVector val18 = ((OnCurve)val17).TangentAt(num7);
		val18.Unitize();
		double num8 = num + value2;
		double num9 = num2 + value3;
		On3dPoint val19 = new On3dPoint(num8, 0.0, num9);
		On3dVector val20 = new On3dVector((IOn3dVector)(object)val12);
		val20.Rotate(CSBlockUtil.CSUtilMath.ToRadians(0.0 - value5), OnUtil.On_yaxis);
		val20.Unitize();
		OnNurbsCurve val21 = CSUtilCurves.BlendCurveAdvanced((IOn3dPoint)(object)val19, pointEnd, (IOn3dVector)(object)(val20 * -1.0), (IOn3dVector)(object)(val18 * -1.0), 3, 3, 0.1, 0.53, value8, 0.35, 0.7, 1.386, doClean: true);
		OnNurbsCurve val22 = val21.Duplicate();
		((OnCurve)val22).Reverse();
		val22.Transform((IOnXform)(object)val3);
		if (!CSUtilCurves.MergeCurves(new List<IOnCurve>
		{
			(IOnCurve)(object)val17,
			(IOnCurve)(object)val21,
			(IOnCurve)(object)val22
		}.ToArray(), out OnCurve mergedCrv, CSApplicationHelper.Application.Tolerance))
		{
			return UpdateError("MergeCurves failed");
		}
		OnNurbsCurve val23 = val.Duplicate();
		if (!TrimRingRail((OnCurve)(object)val23, radiusMax, num6))
		{
			return UpdateError("TrimRingRail failed");
		}
		double num10 = ((OnCurve)val23).Domain().Min();
		IOn3dPoint pointEnd2 = (IOn3dPoint)(object)((OnCurve)val23).PointAt(num10);
		On3dVector val24 = ((OnCurve)val23).TangentAt(num10);
		val24.Unitize();
		On3dVector val25 = OnUtil.ON_CrossProduct((IOn3dVector)(object)val20, (IOn3dVector)(object)(new On3dVector(OnUtil.On_yaxis) * 1.0));
		On3dPoint pointStart = val19 + (IOn3dVector)(object)(val25 * value);
		On3dVector val26 = val20;
		OnNurbsCurve val27 = CSUtilCurves.BlendCurveAdvanced((IOn3dPoint)(object)pointStart, pointEnd2, (IOn3dVector)(object)(val26 * -1.0), (IOn3dVector)(object)(val24 * -1.0), 3, 3, 0.1, 0.53, value8, 0.35, 0.7, 1.386, doClean: true);
		OnNurbsCurve val28 = val27.Duplicate();
		((OnCurve)val28).Reverse();
		val28.Transform((IOnXform)(object)val3);
		if (!CSUtilCurves.MergeCurves(new List<IOnCurve>
		{
			(IOnCurve)(object)val23,
			(IOnCurve)(object)val27,
			(IOnCurve)(object)val28
		}.ToArray(), out OnCurve mergedCrv2, CSApplicationHelper.Application.Tolerance))
		{
			return UpdateError("MergeCurves failed");
		}
		OutRingRailFlexInside.SetData(new CSOnGeometryData((GeometryBase)(object)mergedCrv2.Convert()));
		OutRingRailFlexOutside.SetData(new CSOnGeometryData((GeometryBase)(object)mergedCrv.Convert()));
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
