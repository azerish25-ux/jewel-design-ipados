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
		OnNurbsCurve onNurbsCurve = cSRingRailData.GetNurbsCurve().Convert();
		OnNurbsCurve onNurbsCurve2 = cSOutsideRingRailData.GetNurbsCurve().Convert();
		OnXform onXform = new OnXform();
		onXform.Mirror(OnUtil.On_origin, OnUtil.On_xaxis);
		if (!CSUtilRingRail.GetRingRailPointExtentRight(onNurbsCurve2, out var pointExtent))
		{
			return UpdateError("GetRingRailPointExtentRight failed");
		}
		if (!CSUtilRingRail.GetRingRailPointExtentTop(onNurbsCurve2, out var pointExtent2))
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
		On3dPoint on3dPoint;
		On3dPoint on3dPoint3;
		On3dPoint on3dPoint2;
		switch (whichItem)
		{
		case ValueListItem.RingRailFlexShoulderStyleInverted:
			on3dPoint = new On3dPoint(num + value2, 0.0, num2 + value3);
			on3dPoint3 = new On3dPoint(OnUtil.On_origin);
			on3dPoint2 = new On3dPoint(pointExtent, 0.0, 0.0);
			break;
		case ValueListItem.RingRailFlexShoulderStyleRegular:
			on3dPoint = new On3dPoint(num + value2, 0.0, num2 + value3);
			on3dPoint2 = new On3dPoint(pointExtent, 0.0, 0.0);
			on3dPoint3 = new On3dPoint(on3dPoint2.x, 0.0, on3dPoint.z);
			break;
		default:
			return UpdateError("Invalid Shoulder Style");
		}
		On3dVector on3dVector;
		On3dVector on3dVector3;
		switch (whichItem)
		{
		case ValueListItem.RingRailFlexShoulderStyleInverted:
		{
			double num4 = on3dPoint.DistanceTo(on3dPoint2);
			On3dVector on3dVector4 = on3dPoint - on3dPoint2;
			on3dVector4.Unitize();
			on3dVector = on3dPoint2 + on3dVector4 * num4 * value6 - on3dPoint3;
			on3dVector.Unitize();
			double num5 = on3dPoint3.DistanceTo(on3dPoint2);
			On3dVector on3dVector5 = on3dPoint2 - on3dPoint3;
			on3dVector5.Unitize();
			On3dPoint on3dPoint5 = on3dPoint3 + on3dVector5 * num5 * value4;
			on3dVector3 = on3dPoint - on3dPoint5;
			on3dVector3.Unitize();
			break;
		}
		case ValueListItem.RingRailFlexShoulderStyleRegular:
		{
			on3dVector = new On3dVector(OnUtil.On_xaxis);
			double num3 = on3dPoint3.DistanceTo(on3dPoint2);
			On3dVector on3dVector2 = on3dPoint3 - on3dPoint2;
			on3dVector2.Unitize();
			On3dPoint on3dPoint4 = on3dPoint2 + on3dVector2 * num3 * value4;
			on3dVector3 = on3dPoint - on3dPoint4;
			on3dVector3.Unitize();
			break;
		}
		default:
			return UpdateError("Invalid Shoulder Style");
		}
		double num6 = CSBlockUtil.CSUtilMath.ToDegrees(CSBlockUtil.CSUtilMath.AngleBetweenVectors(on3dVector, OnUtil.On_zaxis)) - value7;
		if (num6 <= 0.0)
		{
			num6 = 0.0 + CSApplicationHelper.Application.Tolerance / 2.0;
		}
		if (num6 >= 180.0)
		{
			num6 = 180.0 - CSApplicationHelper.Application.Tolerance / 2.0;
		}
		OnBoundingBox bbox = new OnBoundingBox();
		if (!onNurbsCurve2.GetBoundingBox(ref bbox))
		{
			return UpdateError("GetBoundingBox failed");
		}
		double radiusMax = bbox.Diagonal().LengthSquared();
		OnNurbsCurve onNurbsCurve3 = onNurbsCurve2.Duplicate();
		if (!TrimRingRail(onNurbsCurve3, radiusMax, num6))
		{
			return UpdateError("TrimRingRail failed");
		}
		double t = onNurbsCurve3.Domain().Min();
		IOn3dPoint pointEnd = onNurbsCurve3.PointAt(t);
		On3dVector on3dVector6 = onNurbsCurve3.TangentAt(t);
		on3dVector6.Unitize();
		double x = num + value2;
		double z = num2 + value3;
		On3dPoint on3dPoint6 = new On3dPoint(x, 0.0, z);
		On3dVector on3dVector7 = new On3dVector(on3dVector3);
		on3dVector7.Rotate(CSBlockUtil.CSUtilMath.ToRadians(0.0 - value5), OnUtil.On_yaxis);
		on3dVector7.Unitize();
		OnNurbsCurve onNurbsCurve4 = CSUtilCurves.BlendCurveAdvanced(on3dPoint6, pointEnd, on3dVector7 * -1.0, on3dVector6 * -1.0, 3, 3, 0.1, 0.53, value8, 0.35, 0.7, 1.386, doClean: true);
		OnNurbsCurve onNurbsCurve5 = onNurbsCurve4.Duplicate();
		onNurbsCurve5.Reverse();
		onNurbsCurve5.Transform(onXform);
		if (!CSUtilCurves.MergeCurves(new List<IOnCurve> { onNurbsCurve3, onNurbsCurve4, onNurbsCurve5 }.ToArray(), out OnCurve mergedCrv, CSApplicationHelper.Application.Tolerance))
		{
			return UpdateError("MergeCurves failed");
		}
		OnNurbsCurve onNurbsCurve6 = onNurbsCurve.Duplicate();
		if (!TrimRingRail(onNurbsCurve6, radiusMax, num6))
		{
			return UpdateError("TrimRingRail failed");
		}
		double t2 = onNurbsCurve6.Domain().Min();
		IOn3dPoint pointEnd2 = onNurbsCurve6.PointAt(t2);
		On3dVector on3dVector8 = onNurbsCurve6.TangentAt(t2);
		on3dVector8.Unitize();
		On3dVector on3dVector9 = OnUtil.ON_CrossProduct(on3dVector7, new On3dVector(OnUtil.On_yaxis) * 1.0);
		On3dPoint pointStart = on3dPoint6 + on3dVector9 * value;
		On3dVector on3dVector10 = on3dVector7;
		OnNurbsCurve onNurbsCurve7 = CSUtilCurves.BlendCurveAdvanced(pointStart, pointEnd2, on3dVector10 * -1.0, on3dVector8 * -1.0, 3, 3, 0.1, 0.53, value8, 0.35, 0.7, 1.386, doClean: true);
		OnNurbsCurve onNurbsCurve8 = onNurbsCurve7.Duplicate();
		onNurbsCurve8.Reverse();
		onNurbsCurve8.Transform(onXform);
		if (!CSUtilCurves.MergeCurves(new List<IOnCurve> { onNurbsCurve6, onNurbsCurve7, onNurbsCurve8 }.ToArray(), out OnCurve mergedCrv2, CSApplicationHelper.Application.Tolerance))
		{
			return UpdateError("MergeCurves failed");
		}
		OutRingRailFlexInside.SetData(new CSOnGeometryData(mergedCrv2.Convert()));
		OutRingRailFlexOutside.SetData(new CSOnGeometryData(mergedCrv.Convert()));
		return base.Update();
	}

	private bool GetIntersection(IOnCurve ringRail, double rotationAngle, double radiusMax, out double param)
	{
		OnXform onXform = new OnXform();
		onXform.Rotation(rotationAngle, OnUtil.On_yaxis, OnUtil.On_origin);
		OnLineCurve onLineCurve = new OnLineCurve(OnUtil.On_origin, new On3dPoint(0.0, 0.0, radiusMax * radiusMax));
		onLineCurve.Transform(onXform);
		if (CSBlockUtil.CSUtilIntersect.GetIntersectParam(ringRail, onLineCurve, out param))
		{
			return true;
		}
		SetMessage("Intersection failed");
		return false;
	}

	private bool TrimRingRail(OnCurve ringRail, double radiusMax, double shoulderBottomAngle)
	{
		double num = CSBlockUtil.CSUtilMath.ToRadians(shoulderBottomAngle);
		if (!GetIntersection(ringRail, num, radiusMax, out var param))
		{
			SetMessage("GetIntersection failed");
			return false;
		}
		if (!GetIntersection(ringRail, 0.0 - num, radiusMax, out var param2))
		{
			SetMessage("GetIntersection failed");
			return false;
		}
		if (!ringRail.Trim(new OnInterval(param, param2)))
		{
			SetMessage("Trim failed");
			return false;
		}
		return true;
	}
}
