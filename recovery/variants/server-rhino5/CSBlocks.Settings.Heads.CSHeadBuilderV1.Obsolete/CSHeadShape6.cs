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

namespace CSBlocks.Settings.Heads.CSHeadBuilderV1.Obsolete;

[Obsolete]
[CSBlock("HeadShape_V6", "{C367D439-BEBC-42C2-860B-115B9B355F35}")]
public sealed class CSHeadShape6 : CSBlock
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

	public CSHeadShape6(string identifier)
		: base(identifier)
	{
		iGems = new CSInputChannel(GetPropertyName(() => iGems), this, ChannelDataType.Gem, ChannelArrayType.Array | ChannelArrayType.Individual);
		iStyle = new CSInputChannel(GetPropertyName(() => iStyle), this, ChannelDataType.Double, ChannelArrayType.Individual);
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
		//IL_0244: Unknown result type (might be due to invalid IL or missing references)
		//IL_024b: Expected O, but got Unknown
		//IL_02f5: Unknown result type (might be due to invalid IL or missing references)
		//IL_02fc: Expected O, but got Unknown
		//IL_0316: Unknown result type (might be due to invalid IL or missing references)
		//IL_031d: Expected O, but got Unknown
		//IL_037c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0383: Expected O, but got Unknown
		//IL_03b7: Unknown result type (might be due to invalid IL or missing references)
		//IL_03bc: Unknown result type (might be due to invalid IL or missing references)
		//IL_03dc: Expected O, but got Unknown
		//IL_0405: Unknown result type (might be due to invalid IL or missing references)
		//IL_040c: Expected O, but got Unknown
		//IL_0420: Unknown result type (might be due to invalid IL or missing references)
		//IL_0427: Expected O, but got Unknown
		//IL_0444: Unknown result type (might be due to invalid IL or missing references)
		//IL_0449: Unknown result type (might be due to invalid IL or missing references)
		//IL_045a: Expected O, but got Unknown
		//IL_0470: Unknown result type (might be due to invalid IL or missing references)
		//IL_0477: Expected O, but got Unknown
		//IL_048b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0492: Expected O, but got Unknown
		//IL_04af: Unknown result type (might be due to invalid IL or missing references)
		//IL_04b4: Unknown result type (might be due to invalid IL or missing references)
		//IL_04c5: Expected O, but got Unknown
		//IL_0577: Unknown result type (might be due to invalid IL or missing references)
		//IL_057e: Expected O, but got Unknown
		//IL_057e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0585: Expected O, but got Unknown
		//IL_05b4: Unknown result type (might be due to invalid IL or missing references)
		//IL_05be: Expected O, but got Unknown
		//IL_05b9: Unknown result type (might be due to invalid IL or missing references)
		//IL_05c0: Expected O, but got Unknown
		//IL_05dd: Unknown result type (might be due to invalid IL or missing references)
		//IL_05e4: Expected O, but got Unknown
		//IL_05c9: Unknown result type (might be due to invalid IL or missing references)
		//IL_05d0: Expected O, but got Unknown
		//IL_05ef: Unknown result type (might be due to invalid IL or missing references)
		//IL_05f4: Unknown result type (might be due to invalid IL or missing references)
		//IL_0605: Expected O, but got Unknown
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
		iBaseToPoint.GetBoolean(ref value11);
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
			List<CSHeadStyles4.HeadProngInfo> headStyle = CSHeadStyles4.GetHeadStyle(cSGemData.GemShape, value);
			if (headStyle == null)
			{
				return UpdateError("Prong Info Missing");
			}
			CSDataArray cSDataArray7 = new CSDataArray();
			foreach (CSHeadStyles4.HeadProngInfo item in headStyle)
			{
				if (item.ProngParamIndex < 0 || item.ProngParamIndex >= list.Count)
				{
					continue;
				}
				On3dPoint val15 = ((OnCurve)val).PointAt(list[item.ProngParamIndex].ProngParam);
				OnLineCurve val16 = new OnLineCurve(OnUtil.On_origin, (IOn3dPoint)(object)(val15 * 50.0));
				ArrayOnX_EVENT val17 = new ArrayOnX_EVENT();
				((OnCurve)val2).IntersectCurve((IOnCurve)(object)val16, ref val17);
				if (val17.Count() > 0)
				{
					double num9 = val17[0][0];
					On3dPoint val18 = new On3dPoint((IOn3dPoint)new On3dPoint((IOn3dPoint)(object)((OnCurve)val2).PointAt(num9)));
					On3dPoint val19;
					if (value11)
					{
						val19 = new On3dPoint(OnUtil.On_origin);
						val19.Transform((IOnXform)(object)val4);
					}
					else
					{
						val19 = new On3dPoint((IOn3dPoint)(object)val18);
						val19.Transform((IOnXform)(object)val5);
					}
					On3dPoint val20 = new On3dPoint((IOn3dPoint)(object)val18);
					val20.Transform((IOnXform)(object)val9);
					OnCurve crv3 = ProngInterpCurve((IOn3dPoint)val20, (IOn3dPoint)(object)val18, (IOn3dPoint)(object)val19);
					cSDataArray7.Append(new CSOnGeometryData(crv3.Convert()));
				}
			}
			cSDataArray.Append(new CSOnGeometryData(val2.Convert()));
			cSDataArray2.Append(new CSOnGeometryData(val10.Convert(), val9.Convert(), applyTransform: false));
			cSDataArray3.Append(new CSOnGeometryData(val6.Convert(), val5.Convert(), applyTransform: false));
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
