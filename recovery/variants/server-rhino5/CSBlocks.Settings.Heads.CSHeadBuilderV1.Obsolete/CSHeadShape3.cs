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
[CSBlock("HeadShape_V3", "{EEB400EA-EF26-4fb3-85B2-6E3C6A211616}")]
public sealed class CSHeadShape3 : CSBlock
{
	public CSInputChannel IGems { get; }

	public CSInputChannel IStyle { get; }

	public CSInputChannel IAbove { get; }

	public CSInputChannel IBelow { get; }

	public CSInputChannel IAngle { get; }

	public CSInputChannel ITopAngle { get; }

	public CSInputChannel IProngAngle { get; }

	public CSOutputChannel OHeadGirdle { get; }

	public CSOutputChannel OHeadTop { get; }

	public CSOutputChannel OHeadBase { get; }

	public CSOutputChannel OHeadSideX { get; }

	public CSOutputChannel OHeadSideY { get; }

	public CSOutputChannel OHeadProngLines { get; }

	public CSHeadShape3(string identifier)
		: base(identifier)
	{
		IGems = new CSInputChannel(GetPropertyName(() => IGems), this, ChannelDataType.Gem, ChannelArrayType.Array | ChannelArrayType.Individual);
		IStyle = new CSInputChannel(GetPropertyName(() => IStyle), this, ChannelDataType.Double, ChannelArrayType.Individual);
		IAbove = new CSInputChannel(GetPropertyName(() => IAbove), this, ChannelDataType.Double, ChannelArrayType.Individual);
		IBelow = new CSInputChannel(GetPropertyName(() => IBelow), this, ChannelDataType.Double, ChannelArrayType.Individual);
		IAngle = new CSInputChannel(GetPropertyName(() => IAngle), this, ChannelDataType.Double, ChannelArrayType.Individual);
		ITopAngle = new CSInputChannel(GetPropertyName(() => ITopAngle), this, ChannelDataType.Double, ChannelArrayType.Individual);
		IProngAngle = new CSInputChannel(GetPropertyName(() => IProngAngle), this, ChannelDataType.Double, ChannelArrayType.Individual);
		OHeadGirdle = new CSOutputChannel(GetPropertyName(() => OHeadGirdle), this, ChannelDataType.Curve, ChannelArrayType.Array);
		OHeadTop = new CSOutputChannel(GetPropertyName(() => OHeadTop), this, ChannelDataType.Curve, ChannelArrayType.Array);
		OHeadBase = new CSOutputChannel(GetPropertyName(() => OHeadBase), this, ChannelDataType.Curve, ChannelArrayType.Array);
		OHeadSideX = new CSOutputChannel(GetPropertyName(() => OHeadSideX), this, ChannelDataType.Curve, ChannelArrayType.Array);
		OHeadSideY = new CSOutputChannel(GetPropertyName(() => OHeadSideY), this, ChannelDataType.Curve, ChannelArrayType.Array);
		OHeadProngLines = new CSOutputChannel(GetPropertyName(() => OHeadProngLines), this, ChannelDataType.Curve, ChannelArrayType.Array);
	}

	public override bool Update()
	{
		//IL_01b4: Unknown result type (might be due to invalid IL or missing references)
		//IL_01bb: Expected O, but got Unknown
		//IL_01c5: Unknown result type (might be due to invalid IL or missing references)
		//IL_01cc: Expected O, but got Unknown
		//IL_0200: Unknown result type (might be due to invalid IL or missing references)
		//IL_0207: Expected O, but got Unknown
		//IL_0274: Unknown result type (might be due to invalid IL or missing references)
		//IL_027b: Expected O, but got Unknown
		//IL_02ce: Unknown result type (might be due to invalid IL or missing references)
		//IL_02d5: Expected O, but got Unknown
		//IL_02e9: Unknown result type (might be due to invalid IL or missing references)
		//IL_02f0: Expected O, but got Unknown
		//IL_0304: Unknown result type (might be due to invalid IL or missing references)
		//IL_030b: Expected O, but got Unknown
		//IL_031d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0324: Expected O, but got Unknown
		//IL_0360: Unknown result type (might be due to invalid IL or missing references)
		//IL_0367: Expected O, but got Unknown
		//IL_037b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0382: Expected O, but got Unknown
		//IL_0396: Unknown result type (might be due to invalid IL or missing references)
		//IL_039d: Expected O, but got Unknown
		//IL_03af: Unknown result type (might be due to invalid IL or missing references)
		//IL_03b6: Expected O, but got Unknown
		//IL_059c: Unknown result type (might be due to invalid IL or missing references)
		//IL_05a3: Expected O, but got Unknown
		//IL_05a5: Unknown result type (might be due to invalid IL or missing references)
		//IL_05ac: Expected O, but got Unknown
		//IL_05ae: Unknown result type (might be due to invalid IL or missing references)
		//IL_05b5: Expected O, but got Unknown
		//IL_05c7: Unknown result type (might be due to invalid IL or missing references)
		//IL_05ce: Expected O, but got Unknown
		//IL_04df: Unknown result type (might be due to invalid IL or missing references)
		//IL_04f9: Expected O, but got Unknown
		//IL_04f4: Unknown result type (might be due to invalid IL or missing references)
		//IL_04fb: Expected O, but got Unknown
		//IL_04fb: Unknown result type (might be due to invalid IL or missing references)
		//IL_0502: Expected O, but got Unknown
		//IL_0532: Unknown result type (might be due to invalid IL or missing references)
		//IL_0552: Unknown result type (might be due to invalid IL or missing references)
		//IL_055c: Expected O, but got Unknown
		//IL_055c: Expected O, but got Unknown
		//IL_0566: Unknown result type (might be due to invalid IL or missing references)
		//IL_056d: Expected O, but got Unknown
		CSDataArray flattenArray = IGems.GetFlattenArray();
		if (flattenArray == null || flattenArray.GetArrayCount() < 1)
		{
			return UpdateMissing("Gems");
		}
		int value = 0;
		IStyle.GetInteger(ref value);
		double value2 = 0.0;
		IAbove.GetDouble(ref value2);
		CSData cSData = null;
		double value3 = 0.0;
		if (!IBelow.GetDouble(ref value3))
		{
			cSData = IBelow.Data;
		}
		double value4 = 10.0;
		IAngle.GetDouble(ref value4);
		double value5 = 0.0;
		ITopAngle.GetDouble(ref value5);
		double value6 = 0.0;
		IProngAngle.GetDouble(ref value6);
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
			double num = value3 * Math.Tan(CSBlockUtil.CSUtilMath.ToRadians(value4));
			double num2 = value2 * Math.Tan(CSBlockUtil.CSUtilMath.ToRadians(value4 + value5));
			CSDataHelper.SetSettingType(cSGemData, GemSettingType.PR);
			cSGemData.GetGemXformRC(out var xDown, out var _);
			double gemXSize = cSGemData.GemXSize;
			double gemYSize = cSGemData.GemYSize;
			double gemXMax = cSGemData.GemXMax;
			double gemYMax = cSGemData.GemYMax;
			OnCurve val = (OnCurve)(object)cSGemData.GetNurbsCurve().Convert();
			((OnGeometry)val).Transform((IOnXform)(object)xDown.Convert());
			if (!GemOffsetCurveV1.GemOffset(cSGemData, 0.0, out OnCurve curve, false, out string error))
			{
				return UpdateError(error);
			}
			OnNurbsCurve val2 = new OnNurbsCurve();
			curve.GetNurbForm(ref val2);
			OnXform val3 = new OnXform();
			val3.Scale((gemXSize + num * 2.0) / gemXSize, (gemYSize + num * 2.0) / gemYSize, 1.0);
			OnXform val4 = new OnXform();
			val4.Translation(0.0, 0.0, value3);
			val4 *= (IOnXform)(object)val3;
			OnNurbsCurve val5 = val2.Duplicate();
			val5.Transform((IOnXform)(object)val4);
			val3.Scale((gemXSize + num2 * 2.0) / gemXSize, (gemYSize + num2 * 2.0) / gemYSize, 1.0);
			OnXform val6 = new OnXform();
			val6.Translation(0.0, 0.0, value2);
			val6 *= (IOnXform)(object)val3;
			OnNurbsCurve val7 = val2.Duplicate();
			val7.Transform((IOnXform)(object)val6);
			CSDataArray cSDataArray7 = new CSDataArray();
			On3dPoint val8 = new On3dPoint(gemXMax, 0.0, 0.0);
			On3dPoint val9 = new On3dPoint(gemXMax, 0.0, 0.0);
			On3dPoint val10 = new On3dPoint(gemXMax, 0.0, 0.0);
			val9.Transform((IOnXform)(object)val4);
			val10.Transform((IOnXform)(object)val6);
			ArrayOn3dPoint val11 = new ArrayOn3dPoint();
			val11.Append((IOn3dPoint)(object)val10);
			val11.Append((IOn3dPoint)(object)val8);
			val11.Append((IOn3dPoint)(object)val9);
			OnNurbsCurve nCrv = RhUtil.RhinoInterpCurve(3, (IArrayOn3dPoint)(object)val11, (IOn3dVector)null, (IOn3dVector)null, 1);
			val8 = new On3dPoint(0.0, gemYMax, 0.0);
			val9 = new On3dPoint(0.0, gemYMax, 0.0);
			val10 = new On3dPoint(0.0, gemYMax, 0.0);
			val9.Transform((IOnXform)(object)val4);
			val10.Transform((IOnXform)(object)val6);
			val11 = new ArrayOn3dPoint();
			val11.Append((IOn3dPoint)(object)val10);
			val11.Append((IOn3dPoint)(object)val8);
			val11.Append((IOn3dPoint)(object)val9);
			OnNurbsCurve nCrv2 = RhUtil.RhinoInterpCurve(3, (IArrayOn3dPoint)(object)val11, (IOn3dVector)null, (IOn3dVector)null, 1);
			IList<CSGemInstanceDefinition.ProngPosition> list = new List<CSGemInstanceDefinition.ProngPosition>();
			if (cSGemData.Def != null)
			{
				list = cSGemData.Def.ProngPositions;
			}
			CSHeadStyles1.GetHeadData1(cSGemData.GemShape, value, out var position, out var _, out var mirror);
			if (position == null)
			{
				return UpdateError("Missing Pos");
			}
			for (int j = 0; j < position.Length; j++)
			{
				double num3 = 0.0;
				if (value6 == 0.0)
				{
					if (position[j] < list.Count && position[j] >= 0)
					{
						On3dPoint val12 = val.PointAt(list[position[j]].ProngParam);
						((OnCurve)val2).GetClosestPoint((IOn3dPoint)(object)val12, ref num3);
					}
				}
				else if (position[j] < list.Count && position[j] >= 0)
				{
					On3dPoint val13 = val.PointAt(list[position[j]].ProngParam);
					OnLineCurve val14 = new OnLineCurve((IOn3dPoint)new On3dPoint(0.0, 0.0, 0.0), (IOn3dPoint)(object)(val13 * 20.0));
					OnXform val15 = new OnXform();
					val15.Rotation(CSBlockUtil.CSUtilMath.ToRadians(value6 * (double)Math.Sign(mirror[j])), (IOn3dVector)new On3dVector(0.0, 0.0, 1.0), (IOn3dPoint)new On3dPoint(0.0, 0.0, 0.0));
					val14.Transform((IOnXform)(object)val15);
					ArrayOnX_EVENT val16 = new ArrayOnX_EVENT();
					((OnCurve)val2).IntersectCurve((IOnCurve)(object)val14, ref val16);
					if (val16.Count() > 0)
					{
						num3 = val16[0][0];
					}
				}
				val8 = new On3dPoint((IOn3dPoint)(object)((OnCurve)val2).PointAt(num3));
				val9 = new On3dPoint((IOn3dPoint)(object)val8);
				val10 = new On3dPoint((IOn3dPoint)(object)val8);
				val9.Transform((IOnXform)(object)val4);
				val10.Transform((IOnXform)(object)val6);
				val11 = new ArrayOn3dPoint();
				val11.Append((IOn3dPoint)(object)val10);
				val11.Append((IOn3dPoint)(object)val8);
				val11.Append((IOn3dPoint)(object)val9);
				OnNurbsCurve nCrv3 = RhUtil.RhinoInterpCurve(3, (IArrayOn3dPoint)(object)val11, (IOn3dVector)null, (IOn3dVector)null, 1);
				cSDataArray7.Append(new CSOnGeometryData(nCrv3.Convert()));
			}
			cSDataArray.Append(new CSOnGeometryData(val2.Convert()));
			cSDataArray2.Append(new CSOnGeometryData(val7.Convert(), val6.Convert(), applyTransform: false));
			cSDataArray3.Append(new CSOnGeometryData(val5.Convert(), val4.Convert(), applyTransform: false));
			cSDataArray4.Append(new CSOnGeometryData(nCrv.Convert()));
			cSDataArray5.Append(new CSOnGeometryData(nCrv2.Convert()));
			cSDataArray6.Append(cSDataArray7);
		}
		OHeadGirdle.SetData(cSDataArray);
		OHeadTop.SetData(cSDataArray2);
		OHeadBase.SetData(cSDataArray3);
		OHeadSideX.SetData(cSDataArray4);
		OHeadSideY.SetData(cSDataArray5);
		OHeadProngLines.SetData(cSDataArray6);
		return base.Update();
	}
}
