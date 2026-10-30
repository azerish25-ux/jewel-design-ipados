using System;
using System.Collections.Generic;
using CS.CSDataUtilities;
using CS.CSObjects.CSBlocks;
using CS.Parametric;
using CS.Parametric.Channels;
using CSBlockDataUtils.CSGemOffsetCurves;
using CSBlockUtil;
using CSCompatibility;
using CSDatas;
using CSGemInstance;
using GV.CounterSketch.Shared;
using RMA.OpenNURBS;
using RMA.Rhino;

namespace CSBlocks.Settings.Heads.CSHeadBuilderV1.Obsolete;

[Obsolete]
[CSBlock("HeadShape_V7", "{07937C9A-2660-4BD5-AED4-F1B794D87640}")]
public sealed class CSHeadShape7 : CSBlock
{
	public CSInputChannel iGems { get; }

	public CSInputChannel iStyle { get; }

	public CSInputChannel iAbove { get; }

	public CSInputChannel iBelow { get; }

	public CSInputChannel iAngleX { get; }

	public CSInputChannel iTopAngle { get; }

	public CSInputChannel iProngAngle { get; }

	public CSInputChannel iBaseToPoint { get; }

	public CSInputChannel iAngleY { get; }

	public CSInputChannel iRailX { get; }

	public CSInputChannel iRailX2 { get; }

	public CSInputChannel iEnableSecondControl { get; }

	public CSOutputChannel oHeadGirdle { get; }

	public CSOutputChannel oHeadTop { get; }

	public CSOutputChannel oHeadBase { get; }

	public CSOutputChannel oHeadSideX { get; }

	public CSOutputChannel oHeadSideY { get; }

	public CSOutputChannel oHeadProngLines { get; }

	public CSHeadShape7(string identifier)
		: base(identifier)
	{
		iGems = new CSInputChannel(GetPropertyName(() => iGems), this, ChannelDataType.Gem, ChannelArrayType.Array);
		iStyle = new CSInputChannel(GetPropertyName(() => iStyle), this, ChannelDataType.Integer, ChannelArrayType.Individual);
		iAbove = new CSInputChannel(GetPropertyName(() => iAbove), this, ChannelDataType.Double, ChannelArrayType.Individual);
		iBelow = new CSInputChannel(GetPropertyName(() => iBelow), this, ChannelDataType.Double, ChannelArrayType.Individual);
		iAngleX = new CSInputChannel(GetPropertyName(() => iAngleX), this, ChannelDataType.Double, ChannelArrayType.Individual);
		iTopAngle = new CSInputChannel(GetPropertyName(() => iTopAngle), this, ChannelDataType.Double, ChannelArrayType.Individual);
		iProngAngle = new CSInputChannel(GetPropertyName(() => iProngAngle), this, ChannelDataType.Double, ChannelArrayType.Individual);
		iBaseToPoint = new CSInputChannel(GetPropertyName(() => iBaseToPoint), this, ChannelDataType.Boolean, ChannelArrayType.Individual);
		iAngleY = new CSInputChannel(GetPropertyName(() => iAngleY), this, ChannelDataType.Double, ChannelArrayType.Individual);
		iRailX = new CSInputChannel(GetPropertyName(() => iRailX), this, ChannelDataType.Double, ChannelArrayType.Individual);
		iRailX2 = new CSInputChannel(GetPropertyName(() => iRailX2), this, ChannelDataType.Double, ChannelArrayType.Individual);
		iEnableSecondControl = new CSInputChannel(GetPropertyName(() => iEnableSecondControl), this, ChannelDataType.Boolean, ChannelArrayType.Individual);
		oHeadGirdle = new CSOutputChannel(GetPropertyName(() => oHeadGirdle), this, ChannelDataType.Curve, ChannelArrayType.Array);
		oHeadTop = new CSOutputChannel(GetPropertyName(() => oHeadTop), this, ChannelDataType.Curve, ChannelArrayType.Array);
		oHeadBase = new CSOutputChannel(GetPropertyName(() => oHeadBase), this, ChannelDataType.Curve, ChannelArrayType.Array);
		oHeadSideX = new CSOutputChannel(GetPropertyName(() => oHeadSideX), this, ChannelDataType.Curve, ChannelArrayType.Array);
		oHeadSideY = new CSOutputChannel(GetPropertyName(() => oHeadSideY), this, ChannelDataType.Curve, ChannelArrayType.Array);
		oHeadProngLines = new CSOutputChannel(GetPropertyName(() => oHeadProngLines), this, ChannelDataType.Curve, ChannelArrayType.Array);
	}

	public override bool Update()
	{
		CSDataArray flattenArray = iGems.GetFlattenArray();
		if (flattenArray == null || flattenArray.GetArrayCount() < 1)
		{
			return UpdateMissing("Missing Gem");
		}
		ValueListItem whichItem = ValueListItem.HeadStyleCenter1;
		iStyle.GetValueListItem(ref whichItem);
		double value = 0.0;
		iAbove.GetDouble(ref value);
		if (value < 0.01)
		{
			value = 0.01;
		}
		CSData cSData = null;
		double value2 = 0.0;
		if (!iBelow.GetDouble(ref value2))
		{
			cSData = iBelow.Data;
		}
		double value3 = 10.0;
		iAngleX.GetDouble(ref value3);
		double value4 = value3;
		iAngleY.GetDouble(ref value4);
		double value5 = 0.0;
		iTopAngle.GetDouble(ref value5);
		double value6 = 0.0;
		iProngAngle.GetDouble(ref value6);
		double value7 = 0.3;
		iRailX.GetDouble(ref value7);
		double value8 = 0.3;
		iRailX2.GetDouble(ref value8);
		bool value9 = false;
		iEnableSecondControl.GetBoolean(ref value9);
		if (value9)
		{
			value7 = value8;
		}
		value7 = value7 * 2.0 + 0.1;
		bool value10 = false;
		iBaseToPoint.GetBoolean(ref value10);
		CSDataArray cSDataArray = new CSDataArray();
		CSDataArray cSDataArray2 = new CSDataArray();
		CSDataArray cSDataArray3 = new CSDataArray();
		CSDataArray cSDataArray4 = new CSDataArray();
		CSDataArray cSDataArray5 = new CSDataArray();
		CSDataArray cSDataArray6 = new CSDataArray();
		for (int i = 0; i < flattenArray.GetArrayCount(); i++)
		{
			if (!(flattenArray.GetArrayItem(i) is CSGemData cSGemData))
			{
				continue;
			}
			if (cSData != null)
			{
				CSDataHelper.GetArrayItem(cSData, i).GetDouble(ref value2);
			}
			if (!CSDataHelper.SetSettingType(cSGemData, GemSettingType.PR) || !cSGemData.GetGemXformRC(out var xDown, out var _))
			{
				continue;
			}
			double gemXSize = cSGemData.GemXSize;
			double gemYSize = cSGemData.GemYSize;
			double gemXMax = cSGemData.GemXMax;
			double gemYMax = cSGemData.GemYMax;
			OnNurbsCurve onNurbsCurve = cSGemData.GetNurbsCurve().Convert();
			if (onNurbsCurve == null)
			{
				continue;
			}
			onNurbsCurve.Transform(xDown.Convert());
			if (!GemOffsetCurveV2.GemOffset(cSGemData, 0.0, out OnCurve curve, false, out string error, 0.0))
			{
				return UpdateError(error);
			}
			OnNurbsCurve nurbs_curve = new OnNurbsCurve();
			curve.GetNurbForm(ref nurbs_curve);
			double num = value2 * Math.Tan(CSBlockUtil.CSUtilMath.ToRadians(value3));
			double num2 = value2 * Math.Tan(CSBlockUtil.CSUtilMath.ToRadians(value4));
			double num3 = gemXSize + num * 2.0;
			double num4;
			if (num3 < value7)
			{
				num3 = value7;
				num4 = CSBlockUtil.CSUtilMath.ToDegrees(Math.Atan((num3 - gemXSize) / 2.0 / value2));
			}
			else
			{
				num4 = value3;
			}
			double num5 = gemYSize + num2 * 2.0;
			double num6;
			if (num5 < value7)
			{
				num5 = value7;
				num6 = CSBlockUtil.CSUtilMath.ToDegrees(Math.Atan((num5 - gemYSize) / 2.0 / value2));
			}
			else
			{
				num6 = value4;
			}
			OnXform onXform = new OnXform();
			onXform.Scale(num3 / gemXSize, num5 / gemYSize, 1.0);
			OnXform onXform2 = new OnXform();
			onXform2.Translation(0.0, 0.0, value2);
			OnXform onXform3 = onXform2 * onXform;
			OnNurbsCurve onNurbsCurve2 = nurbs_curve.Duplicate();
			onNurbsCurve2.Transform(onXform3);
			double num7 = value * Math.Tan(CSBlockUtil.CSUtilMath.ToRadians(num4 + value5));
			double num8 = value * Math.Tan(CSBlockUtil.CSUtilMath.ToRadians(num6 + value5));
			OnXform onXform4 = new OnXform();
			onXform4.Scale((gemXSize + num7 * 2.0) / gemXSize, (gemYSize + num8 * 2.0) / gemYSize, 1.0);
			OnXform onXform5 = new OnXform();
			onXform5.Translation(0.0, 0.0, value);
			OnXform onXform6 = onXform5 * onXform4;
			OnNurbsCurve onNurbsCurve3 = nurbs_curve.Duplicate();
			onNurbsCurve3.Transform(onXform6);
			On3dPoint pMiddle = new On3dPoint(gemXMax, 0.0, 0.0);
			On3dPoint on3dPoint = new On3dPoint(gemXMax, 0.0, 0.0);
			on3dPoint.Transform(onXform3);
			On3dPoint on3dPoint2 = new On3dPoint(gemXMax, 0.0, 0.0);
			on3dPoint2.Transform(onXform6);
			OnCurve crv = ProngInterpCurve(on3dPoint2, pMiddle, on3dPoint);
			On3dPoint pMiddle2 = new On3dPoint(0.0, gemYMax, 0.0);
			On3dPoint on3dPoint3 = new On3dPoint(0.0, gemYMax, 0.0);
			on3dPoint3.Transform(onXform3);
			On3dPoint on3dPoint4 = new On3dPoint(0.0, gemYMax, 0.0);
			on3dPoint4.Transform(onXform6);
			OnCurve crv2 = ProngInterpCurve(on3dPoint4, pMiddle2, on3dPoint3);
			IList<CSGemInstanceDefinition.ProngPosition> list = new List<CSGemInstanceDefinition.ProngPosition>();
			if (cSGemData.Def != null)
			{
				list = cSGemData.Def.ProngPositions;
			}
			List<CSHeadStyles5.HeadProngInfo> headStyle = CSHeadStyles5.GetHeadStyle(cSGemData.GemShape, whichItem);
			if (headStyle == null)
			{
				return UpdateError("Prong Info Missing");
			}
			CSDataArray cSDataArray7 = new CSDataArray();
			foreach (CSHeadStyles5.HeadProngInfo item in headStyle)
			{
				if (item.ProngParamIndex < 0 || item.ProngParamIndex >= list.Count)
				{
					continue;
				}
				On3dPoint on3dPoint5 = onNurbsCurve.PointAt(list[item.ProngParamIndex].ProngParam);
				OnLineCurve curveB = new OnLineCurve(OnUtil.On_origin, on3dPoint5 * 50.0);
				ArrayOnX_EVENT x = new ArrayOnX_EVENT();
				nurbs_curve.IntersectCurve(curveB, ref x);
				if (x.Count() > 0)
				{
					double t = x[0].get_m_a(0);
					On3dPoint on3dPoint6 = new On3dPoint(new On3dPoint(nurbs_curve.PointAt(t)));
					On3dPoint on3dPoint7;
					if (value10)
					{
						on3dPoint7 = new On3dPoint(OnUtil.On_origin);
						on3dPoint7.Transform(onXform2);
					}
					else
					{
						on3dPoint7 = new On3dPoint(on3dPoint6);
						on3dPoint7.Transform(onXform3);
					}
					On3dPoint on3dPoint8 = new On3dPoint(on3dPoint6);
					on3dPoint8.Transform(onXform6);
					OnCurve crv3 = ProngInterpCurve(on3dPoint8, on3dPoint6, on3dPoint7);
					cSDataArray7.Append(new CSOnGeometryData(crv3.Convert()));
				}
			}
			cSDataArray.Append(new CSOnGeometryData(nurbs_curve.Convert()));
			cSDataArray2.Append(new CSOnGeometryData(onNurbsCurve3.Convert(), onXform6.Convert(), applyTransform: false));
			cSDataArray3.Append(new CSOnGeometryData(onNurbsCurve2.Convert(), onXform3.Convert(), applyTransform: false));
			cSDataArray4.Append(new CSOnGeometryData(crv.Convert()));
			cSDataArray5.Append(new CSOnGeometryData(crv2.Convert()));
			cSDataArray6.Append(cSDataArray7);
		}
		oHeadGirdle.SetData(cSDataArray);
		oHeadTop.SetData(cSDataArray2);
		oHeadBase.SetData(cSDataArray3);
		oHeadSideX.SetData(cSDataArray4);
		oHeadSideY.SetData(cSDataArray5);
		oHeadProngLines.SetData(cSDataArray6);
		return base.Update();
	}

	private static OnCurve ProngInterpCurve(IOn3dPoint pTop, IOn3dPoint pMiddle, IOn3dPoint pBase)
	{
		ArrayOn3dPoint arrayOn3dPoint = new ArrayOn3dPoint();
		if (pTop.DistanceTo(pMiddle) > 0.01)
		{
			arrayOn3dPoint.Append(pTop);
			arrayOn3dPoint.Append(pMiddle);
			arrayOn3dPoint.Append(pBase);
		}
		else
		{
			OnLine onLine = new OnLine(pTop, pBase);
			arrayOn3dPoint.Append(pTop);
			arrayOn3dPoint.Append(onLine.PointAt(0.1));
			arrayOn3dPoint.Append(pBase);
		}
		OnNurbsCurve onNurbsCurve = RhUtil.RhinoInterpCurve(3, arrayOn3dPoint, null, null, 1);
		if (onNurbsCurve != null)
		{
			return onNurbsCurve;
		}
		return new OnLineCurve(pTop, pBase);
	}
}
