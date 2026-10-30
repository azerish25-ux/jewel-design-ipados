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
[CSBlock("HeadShape_V1", "{5F41C682-99B9-43e5-AF5C-9868D9DDEA1F}")]
public sealed class CSHeadShape1 : CSBlock
{
	public CSInputChannel IGem { get; }

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

	public CSHeadShape1(string identifier)
		: base(identifier)
	{
		IGem = new CSInputChannel(GetPropertyName(() => IGem), this, ChannelDataType.Gem, ChannelArrayType.Array | ChannelArrayType.Individual);
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
		//IL_00f9: Unknown result type (might be due to invalid IL or missing references)
		//IL_0163: Unknown result type (might be due to invalid IL or missing references)
		//IL_016a: Expected O, but got Unknown
		//IL_0174: Unknown result type (might be due to invalid IL or missing references)
		//IL_017b: Expected O, but got Unknown
		//IL_01af: Unknown result type (might be due to invalid IL or missing references)
		//IL_01b6: Expected O, but got Unknown
		//IL_0223: Unknown result type (might be due to invalid IL or missing references)
		//IL_022a: Expected O, but got Unknown
		//IL_027d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0284: Expected O, but got Unknown
		//IL_0298: Unknown result type (might be due to invalid IL or missing references)
		//IL_029f: Expected O, but got Unknown
		//IL_02b3: Unknown result type (might be due to invalid IL or missing references)
		//IL_02ba: Expected O, but got Unknown
		//IL_02cc: Unknown result type (might be due to invalid IL or missing references)
		//IL_02d3: Expected O, but got Unknown
		//IL_030f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0316: Expected O, but got Unknown
		//IL_032a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0331: Expected O, but got Unknown
		//IL_0345: Unknown result type (might be due to invalid IL or missing references)
		//IL_034c: Expected O, but got Unknown
		//IL_035e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0365: Expected O, but got Unknown
		//IL_05e6: Unknown result type (might be due to invalid IL or missing references)
		//IL_0605: Unknown result type (might be due to invalid IL or missing references)
		//IL_0542: Unknown result type (might be due to invalid IL or missing references)
		//IL_0549: Expected O, but got Unknown
		//IL_054b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0552: Expected O, but got Unknown
		//IL_0554: Unknown result type (might be due to invalid IL or missing references)
		//IL_055b: Expected O, but got Unknown
		//IL_056d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0574: Expected O, but got Unknown
		//IL_048f: Unknown result type (might be due to invalid IL or missing references)
		//IL_04a9: Expected O, but got Unknown
		//IL_04a4: Unknown result type (might be due to invalid IL or missing references)
		//IL_04ab: Expected O, but got Unknown
		//IL_04ab: Unknown result type (might be due to invalid IL or missing references)
		//IL_04b2: Expected O, but got Unknown
		//IL_04e2: Unknown result type (might be due to invalid IL or missing references)
		//IL_0502: Unknown result type (might be due to invalid IL or missing references)
		//IL_050c: Expected O, but got Unknown
		//IL_050c: Expected O, but got Unknown
		//IL_0516: Unknown result type (might be due to invalid IL or missing references)
		//IL_051d: Expected O, but got Unknown
		CSDataArray flattenArray = IGem.GetFlattenArray();
		if (flattenArray == null || flattenArray.GetArrayCount() < 1)
		{
			return UpdateMissing("Gems");
		}
		if (!(flattenArray.GetArrayItem(0) is CSGemData cSGemData))
		{
			return UpdateMissing("Gems");
		}
		CSDataHelper.SetSettingType(cSGemData, GemSettingType.PR);
		int value = 0;
		IStyle.GetInteger(ref value);
		double value2 = 0.0;
		IAbove.GetDouble(ref value2);
		double value3 = 0.0;
		IBelow.GetDouble(ref value3);
		double value4 = 10.0;
		IAngle.GetDouble(ref value4);
		double value5 = 0.0;
		ITopAngle.GetDouble(ref value5);
		double value6 = 0.0;
		IProngAngle.GetDouble(ref value6);
		double num = value3 * Math.Tan(CSBlockUtil.CSUtilMath.ToRadians(value4));
		double num2 = value2 * Math.Tan(CSBlockUtil.CSUtilMath.ToRadians(value4 + value5));
		CSBlockUtil.CSUtilMath.GetUnscaledXforms((IOnXform)(object)cSGemData.Xform.Convert(), out var _, out var xDown);
		double gemXSize = cSGemData.GemXSize;
		double gemYSize = cSGemData.GemYSize;
		double gemXMax = cSGemData.GemXMax;
		double gemYMax = cSGemData.GemYMax;
		OnCurve val = (OnCurve)(object)cSGemData.GetNurbsCurve().Convert();
		((OnGeometry)val).Transform((IOnXform)(object)xDown);
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
		CSDataArray cSDataArray = new CSDataArray();
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
			return UpdateError("Missing POS");
		}
		for (int i = 0; i < position.Length; i++)
		{
			double num3 = 0.0;
			if (value6 == 0.0)
			{
				if (list.Count > 0 && position[i] < list.Count)
				{
					On3dPoint val12 = val.PointAt(list[position[i]].ProngParam);
					((OnCurve)val2).GetClosestPoint((IOn3dPoint)(object)val12, ref num3);
				}
			}
			else if (list.Count > 0 && position[i] < list.Count)
			{
				On3dPoint val13 = val.PointAt(list[position[i]].ProngParam);
				OnLineCurve val14 = new OnLineCurve((IOn3dPoint)new On3dPoint(0.0, 0.0, 0.0), (IOn3dPoint)(object)(val13 * 20.0));
				OnXform val15 = new OnXform();
				val15.Rotation(CSBlockUtil.CSUtilMath.ToRadians(value6 * (double)Math.Sign(mirror[i])), (IOn3dVector)new On3dVector(0.0, 0.0, 1.0), (IOn3dPoint)new On3dPoint(0.0, 0.0, 0.0));
				val14.Transform((IOnXform)(object)val15);
				ArrayOnX_EVENT val16 = new ArrayOnX_EVENT();
				((OnCurve)val2).IntersectCurve((IOnCurve)(object)val14, ref val16);
				num3 = val16[0][0];
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
			cSDataArray.Append(new CSOnGeometryData((GeometryBase)(object)nCrv3.Convert()));
		}
		OHeadGirdle.SetData(new CSOnGeometryData((GeometryBase)(object)val2.Convert()));
		OHeadTop.SetData(new CSOnGeometryData((GeometryBase)(object)val7.Convert(), val6.Convert(), applyTransform: false));
		OHeadBase.SetData(new CSOnGeometryData((GeometryBase)(object)val5.Convert(), val4.Convert(), applyTransform: false));
		OHeadSideX.SetData(new CSOnGeometryData((GeometryBase)(object)nCrv.Convert()));
		OHeadSideY.SetData(new CSOnGeometryData((GeometryBase)(object)nCrv2.Convert()));
		OHeadProngLines.SetData(cSDataArray);
		return base.Update();
	}
}
