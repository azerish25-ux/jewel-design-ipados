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
using RMA.OpenNURBS;
using RMA.Rhino;
using Rhino.Geometry;

namespace CSBlocks.Settings.Heads.CSHeadBuilderV1.Obsolete;

[Obsolete]
[CSBlock("HeadShape_V4", "{6014D50C-BF30-498d-BB4C-E1B475A29B50}")]
public sealed class CSHeadShape4 : CSBlock
{
	public CSInputChannel iGems { get; }

	public CSInputChannel iStyle { get; }

	public CSInputChannel iAbove { get; }

	public CSInputChannel iBelow { get; }

	public CSInputChannel iAngleX { get; }

	public CSInputChannel iTopAngle { get; }

	public CSInputChannel iProngAngle { get; }

	public CSInputChannel iEnableModeGameChanger { get; }

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

	public CSHeadShape4(string identifier)
		: base(identifier)
	{
		iGems = new CSInputChannel(GetPropertyName(() => iGems), this, ChannelDataType.Gem, ChannelArrayType.Array | ChannelArrayType.Individual);
		iStyle = new CSInputChannel(GetPropertyName(() => iStyle), this, ChannelDataType.Double, ChannelArrayType.Individual);
		iAbove = new CSInputChannel(GetPropertyName(() => iAbove), this, ChannelDataType.Double, ChannelArrayType.Individual);
		iBelow = new CSInputChannel(GetPropertyName(() => iBelow), this, ChannelDataType.Double, ChannelArrayType.Individual);
		iAngleX = new CSInputChannel(GetPropertyName(() => iAngleX), this, ChannelDataType.Double, ChannelArrayType.Individual);
		iTopAngle = new CSInputChannel(GetPropertyName(() => iTopAngle), this, ChannelDataType.Double, ChannelArrayType.Individual);
		iProngAngle = new CSInputChannel(GetPropertyName(() => iProngAngle), this, ChannelDataType.Double, ChannelArrayType.Individual);
		iEnableModeGameChanger = new CSInputChannel(GetPropertyName(() => iEnableModeGameChanger), this, ChannelDataType.Boolean, ChannelArrayType.Individual);
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
		//IL_021f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0255: Unknown result type (might be due to invalid IL or missing references)
		//IL_025c: Expected O, but got Unknown
		//IL_0306: Unknown result type (might be due to invalid IL or missing references)
		//IL_030d: Expected O, but got Unknown
		//IL_0327: Unknown result type (might be due to invalid IL or missing references)
		//IL_032e: Expected O, but got Unknown
		//IL_038d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0394: Expected O, but got Unknown
		//IL_03c8: Unknown result type (might be due to invalid IL or missing references)
		//IL_03cd: Unknown result type (might be due to invalid IL or missing references)
		//IL_03ed: Expected O, but got Unknown
		//IL_0416: Unknown result type (might be due to invalid IL or missing references)
		//IL_041d: Expected O, but got Unknown
		//IL_0431: Unknown result type (might be due to invalid IL or missing references)
		//IL_0438: Expected O, but got Unknown
		//IL_0455: Unknown result type (might be due to invalid IL or missing references)
		//IL_045a: Unknown result type (might be due to invalid IL or missing references)
		//IL_046b: Expected O, but got Unknown
		//IL_0481: Unknown result type (might be due to invalid IL or missing references)
		//IL_0488: Expected O, but got Unknown
		//IL_049c: Unknown result type (might be due to invalid IL or missing references)
		//IL_04a3: Expected O, but got Unknown
		//IL_04c0: Unknown result type (might be due to invalid IL or missing references)
		//IL_04c5: Unknown result type (might be due to invalid IL or missing references)
		//IL_04d6: Expected O, but got Unknown
		//IL_070d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0728: Unknown result type (might be due to invalid IL or missing references)
		//IL_05b7: Unknown result type (might be due to invalid IL or missing references)
		//IL_05be: Expected O, but got Unknown
		//IL_05e6: Unknown result type (might be due to invalid IL or missing references)
		//IL_05ed: Expected O, but got Unknown
		//IL_05f7: Unknown result type (might be due to invalid IL or missing references)
		//IL_05fe: Expected O, but got Unknown
		//IL_0630: Unknown result type (might be due to invalid IL or missing references)
		//IL_0669: Expected O, but got Unknown
		//IL_0696: Unknown result type (might be due to invalid IL or missing references)
		//IL_069d: Expected O, but got Unknown
		//IL_0671: Unknown result type (might be due to invalid IL or missing references)
		//IL_0678: Expected O, but got Unknown
		//IL_06a8: Unknown result type (might be due to invalid IL or missing references)
		//IL_06ad: Unknown result type (might be due to invalid IL or missing references)
		//IL_06be: Expected O, but got Unknown
		CSDataArray flattenArray = iGems.GetFlattenArray();
		if (flattenArray == null || flattenArray.GetArrayCount() < 1)
		{
			return UpdateMissing("Missing Gem");
		}
		int value = 0;
		iStyle.GetInteger(ref value);
		double value2 = 0.0;
		iAbove.GetDouble(ref value2);
		if (value2 < 0.01)
		{
			value2 = 0.01;
		}
		CSData cSData = null;
		double value3 = 0.0;
		if (!iBelow.GetDouble(ref value3))
		{
			cSData = iBelow.Data;
		}
		double value4 = 10.0;
		iAngleX.GetDouble(ref value4);
		double value5 = value4;
		iAngleY.GetDouble(ref value5);
		double value6 = 0.0;
		iTopAngle.GetDouble(ref value6);
		double value7 = 0.0;
		iProngAngle.GetDouble(ref value7);
		double value8 = 0.3;
		iRailX.GetDouble(ref value8);
		double value9 = 0.3;
		iRailX2.GetDouble(ref value9);
		bool value10 = false;
		iEnableSecondControl.GetBoolean(ref value10);
		if (value10)
		{
			value8 = value9;
		}
		value8 = value8 * 2.0 + 0.1;
		bool value11 = false;
		iEnableModeGameChanger.GetBoolean(ref value11);
		bool value12 = false;
		iBaseToPoint.GetBoolean(ref value12);
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
				CSDataHelper.GetArrayItem(cSData, i).GetDouble(ref value3);
			}
			if (!CSDataHelper.SetSettingType(cSGemData, GemSettingType.PR) || !cSGemData.GetGemXformRC(out var xDown, out var _))
			{
				continue;
			}
			double gemXSize = cSGemData.GemXSize;
			double gemYSize = cSGemData.GemYSize;
			double gemXMax = cSGemData.GemXMax;
			double gemYMax = cSGemData.GemYMax;
			OnNurbsCurve val = cSGemData.GetNurbsCurve().Convert();
			if (val == null)
			{
				continue;
			}
			val.Transform((IOnXform)(object)xDown.Convert());
			if (!GemOffsetCurveV2.GemOffset(cSGemData, 0.0, out OnCurve curve, false, out string error, 0.0))
			{
				return UpdateError(error);
			}
			OnNurbsCurve val2 = new OnNurbsCurve();
			curve.GetNurbForm(ref val2);
			double num = value3 * Math.Tan(CSBlockUtil.CSUtilMath.ToRadians(value4));
			double num2 = value3 * Math.Tan(CSBlockUtil.CSUtilMath.ToRadians(value5));
			double num3 = gemXSize + num * 2.0;
			double num4;
			if (num3 < value8)
			{
				num3 = value8;
				num4 = CSBlockUtil.CSUtilMath.ToDegrees(Math.Atan((num3 - gemXSize) / 2.0 / value3));
			}
			else
			{
				num4 = value4;
			}
			double num5 = gemYSize + num2 * 2.0;
			double num6;
			if (num5 < value8)
			{
				num5 = value8;
				num6 = CSBlockUtil.CSUtilMath.ToDegrees(Math.Atan((num5 - gemYSize) / 2.0 / value3));
			}
			else
			{
				num6 = value5;
			}
			OnXform val3 = new OnXform();
			val3.Scale(num3 / gemXSize, num5 / gemYSize, 1.0);
			OnXform val4 = new OnXform();
			val4.Translation(0.0, 0.0, value3);
			OnXform val5 = val4 * (IOnXform)(object)val3;
			OnNurbsCurve val6 = val2.Duplicate();
			val6.Transform((IOnXform)(object)val5);
			double num7 = value2 * Math.Tan(CSBlockUtil.CSUtilMath.ToRadians(num4 + value6));
			double num8 = value2 * Math.Tan(CSBlockUtil.CSUtilMath.ToRadians(num6 + value6));
			OnXform val7 = new OnXform();
			val7.Scale((gemXSize + num7 * 2.0) / gemXSize, (gemYSize + num8 * 2.0) / gemYSize, 1.0);
			OnXform val8 = new OnXform();
			val8.Translation(0.0, 0.0, value2);
			OnXform val9 = val8 * (IOnXform)(object)val7;
			OnNurbsCurve val10 = val2.Duplicate();
			val10.Transform((IOnXform)(object)val9);
			On3dPoint pMiddle = new On3dPoint(gemXMax, 0.0, 0.0);
			On3dPoint val11 = new On3dPoint(gemXMax, 0.0, 0.0);
			val11.Transform((IOnXform)(object)val5);
			On3dPoint val12 = new On3dPoint(gemXMax, 0.0, 0.0);
			val12.Transform((IOnXform)(object)val9);
			OnCurve crv = ProngInterpCurve((IOn3dPoint)val12, (IOn3dPoint)(object)pMiddle, (IOn3dPoint)(object)val11);
			On3dPoint pMiddle2 = new On3dPoint(0.0, gemYMax, 0.0);
			On3dPoint val13 = new On3dPoint(0.0, gemYMax, 0.0);
			val13.Transform((IOnXform)(object)val5);
			On3dPoint val14 = new On3dPoint(0.0, gemYMax, 0.0);
			val14.Transform((IOnXform)(object)val9);
			OnCurve crv2 = ProngInterpCurve((IOn3dPoint)val14, (IOn3dPoint)(object)pMiddle2, (IOn3dPoint)(object)val13);
			IList<CSGemInstanceDefinition.ProngPosition> list = new List<CSGemInstanceDefinition.ProngPosition>();
			if (cSGemData.Def != null)
			{
				list = cSGemData.Def.ProngPositions;
			}
			List<CSHeadStyles2.HeadProngInfo> list2 = (value11 ? CSHeadStyles2.GetHeadStyleGameChanger(cSGemData.GemShape, value, cSGemData.VersionMajor) : CSHeadStyles2.GetHeadStyle(cSGemData.GemShape, value, cSGemData.VersionMajor));
			if (list2 == null)
			{
				return UpdateError("Prong Info Missing");
			}
			CSDataArray cSDataArray7 = new CSDataArray();
			foreach (CSHeadStyles2.HeadProngInfo item in list2)
			{
				if (item.ProngParamIndex < 0 || item.ProngParamIndex >= list.Count)
				{
					continue;
				}
				double prongParam = list[item.ProngParamIndex].ProngParam;
				On3dPoint val15 = ((OnCurve)val).PointAt(prongParam);
				double num9 = CSBlockUtil.CSUtilMath.ToRadians(item.ProngParamAngleOffset + value7 * (double)Math.Sign(item.Mirror));
				OnXform val16 = new OnXform();
				val16.Rotation(num9, OnUtil.On_zaxis, OnUtil.On_origin);
				OnLineCurve val17 = new OnLineCurve(OnUtil.On_origin, (IOn3dPoint)(object)(val15 * 20.0));
				val17.Transform((IOnXform)(object)val16);
				ArrayOnX_EVENT val18 = new ArrayOnX_EVENT();
				((OnCurve)val2).IntersectCurve((IOnCurve)(object)val17, ref val18);
				if (val18.Count() > 0)
				{
					double num10 = val18[0][0];
					On3dPoint val19 = new On3dPoint((IOn3dPoint)(object)((OnCurve)val2).PointAt(num10));
					On3dVector val20 = CSBlockUtil.CSUtilMath.AverageNormalAt((IOnCurve)(object)val2, OnUtil.On_zaxis, num10);
					val20.Unitize();
					On3dPoint val21 = val19 + (IOn3dVector)(object)(val20 * item.Nudge);
					On3dPoint val22;
					if (!value12)
					{
						val22 = new On3dPoint((IOn3dPoint)(object)val21);
						val22.Transform((IOnXform)(object)val5);
					}
					else
					{
						val22 = new On3dPoint((IOn3dPoint)(object)On3dPoint.op_Implicit((IOn3dVector)(object)(val20 * item.Nudge)));
						val22.Transform((IOnXform)(object)val4);
					}
					On3dPoint val23 = new On3dPoint((IOn3dPoint)(object)val21);
					val23.Transform((IOnXform)(object)val9);
					OnCurve crv3 = ProngInterpCurve((IOn3dPoint)val23, (IOn3dPoint)(object)val21, (IOn3dPoint)(object)val22);
					cSDataArray7.Append(new CSOnGeometryData((GeometryBase)(object)crv3.Convert()));
				}
			}
			cSDataArray.Append(new CSOnGeometryData((GeometryBase)(object)val2.Convert()));
			cSDataArray2.Append(new CSOnGeometryData((GeometryBase)(object)val10.Convert(), val9.Convert(), applyTransform: false));
			cSDataArray3.Append(new CSOnGeometryData((GeometryBase)(object)val6.Convert(), val4.Convert(), applyTransform: false));
			cSDataArray4.Append(new CSOnGeometryData((GeometryBase)(object)crv.Convert()));
			cSDataArray5.Append(new CSOnGeometryData((GeometryBase)(object)crv2.Convert()));
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
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0006: Expected O, but got Unknown
		//IL_0031: Unknown result type (might be due to invalid IL or missing references)
		//IL_0037: Expected O, but got Unknown
		//IL_006c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0072: Expected O, but got Unknown
		ArrayOn3dPoint val = new ArrayOn3dPoint();
		if (pTop.DistanceTo(pMiddle) > 0.01)
		{
			val.Append(pTop);
			val.Append(pMiddle);
			val.Append(pBase);
		}
		else
		{
			OnLine val2 = new OnLine(pTop, pBase);
			val.Append(pTop);
			val.Append((IOn3dPoint)(object)val2.PointAt(0.1));
			val.Append(pBase);
		}
		OnNurbsCurve val3 = RhUtil.RhinoInterpCurve(3, (IArrayOn3dPoint)(object)val, (IOn3dVector)null, (IOn3dVector)null, 1);
		if (val3 != null)
		{
			return (OnCurve)(object)val3;
		}
		return (OnCurve)new OnLineCurve(pTop, pBase);
	}
}
