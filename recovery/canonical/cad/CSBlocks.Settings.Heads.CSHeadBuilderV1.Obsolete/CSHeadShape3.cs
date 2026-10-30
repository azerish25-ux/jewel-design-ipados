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
			OnCurve onCurve = cSGemData.GetNurbsCurve().Convert();
			onCurve.Transform(xDown.Convert());
			if (!GemOffsetCurveV1.GemOffset(cSGemData, 0.0, out OnCurve curve, false, out string error))
			{
				return UpdateError(error);
			}
			OnNurbsCurve nurbs_curve = new OnNurbsCurve();
			curve.GetNurbForm(ref nurbs_curve);
			OnXform onXform = new OnXform();
			onXform.Scale((gemXSize + num * 2.0) / gemXSize, (gemYSize + num * 2.0) / gemYSize, 1.0);
			OnXform onXform2 = new OnXform();
			onXform2.Translation(0.0, 0.0, value3);
			onXform2 *= (IOnXform)onXform;
			OnNurbsCurve onNurbsCurve = nurbs_curve.Duplicate();
			onNurbsCurve.Transform(onXform2);
			onXform.Scale((gemXSize + num2 * 2.0) / gemXSize, (gemYSize + num2 * 2.0) / gemYSize, 1.0);
			OnXform onXform3 = new OnXform();
			onXform3.Translation(0.0, 0.0, value2);
			onXform3 *= (IOnXform)onXform;
			OnNurbsCurve onNurbsCurve2 = nurbs_curve.Duplicate();
			onNurbsCurve2.Transform(onXform3);
			CSDataArray cSDataArray7 = new CSDataArray();
			On3dPoint val = new On3dPoint(gemXMax, 0.0, 0.0);
			On3dPoint on3dPoint = new On3dPoint(gemXMax, 0.0, 0.0);
			On3dPoint on3dPoint2 = new On3dPoint(gemXMax, 0.0, 0.0);
			on3dPoint.Transform(onXform2);
			on3dPoint2.Transform(onXform3);
			ArrayOn3dPoint arrayOn3dPoint = new ArrayOn3dPoint();
			arrayOn3dPoint.Append(on3dPoint2);
			arrayOn3dPoint.Append(val);
			arrayOn3dPoint.Append(on3dPoint);
			OnNurbsCurve nCrv = RhUtil.RhinoInterpCurve(3, arrayOn3dPoint, null, null, 1);
			val = new On3dPoint(0.0, gemYMax, 0.0);
			on3dPoint = new On3dPoint(0.0, gemYMax, 0.0);
			on3dPoint2 = new On3dPoint(0.0, gemYMax, 0.0);
			on3dPoint.Transform(onXform2);
			on3dPoint2.Transform(onXform3);
			arrayOn3dPoint = new ArrayOn3dPoint();
			arrayOn3dPoint.Append(on3dPoint2);
			arrayOn3dPoint.Append(val);
			arrayOn3dPoint.Append(on3dPoint);
			OnNurbsCurve nCrv2 = RhUtil.RhinoInterpCurve(3, arrayOn3dPoint, null, null, 1);
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
				double t = 0.0;
				if (value6 == 0.0)
				{
					if (position[j] < list.Count && position[j] >= 0)
					{
						On3dPoint test_point = onCurve.PointAt(list[position[j]].ProngParam);
						nurbs_curve.GetClosestPoint(test_point, ref t);
					}
				}
				else if (position[j] < list.Count && position[j] >= 0)
				{
					On3dPoint on3dPoint3 = onCurve.PointAt(list[position[j]].ProngParam);
					OnLineCurve onLineCurve = new OnLineCurve(new On3dPoint(0.0, 0.0, 0.0), on3dPoint3 * 20.0);
					OnXform onXform4 = new OnXform();
					onXform4.Rotation(CSBlockUtil.CSUtilMath.ToRadians(value6 * (double)Math.Sign(mirror[j])), new On3dVector(0.0, 0.0, 1.0), new On3dPoint(0.0, 0.0, 0.0));
					onLineCurve.Transform(onXform4);
					ArrayOnX_EVENT x = new ArrayOnX_EVENT();
					nurbs_curve.IntersectCurve(onLineCurve, ref x);
					if (x.Count() > 0)
					{
						t = x[0].get_m_a(0);
					}
				}
				val = new On3dPoint(nurbs_curve.PointAt(t));
				on3dPoint = new On3dPoint(val);
				on3dPoint2 = new On3dPoint(val);
				on3dPoint.Transform(onXform2);
				on3dPoint2.Transform(onXform3);
				arrayOn3dPoint = new ArrayOn3dPoint();
				arrayOn3dPoint.Append(on3dPoint2);
				arrayOn3dPoint.Append(val);
				arrayOn3dPoint.Append(on3dPoint);
				OnNurbsCurve nCrv3 = RhUtil.RhinoInterpCurve(3, arrayOn3dPoint, null, null, 1);
				cSDataArray7.Append(new CSOnGeometryData(nCrv3.Convert()));
			}
			cSDataArray.Append(new CSOnGeometryData(nurbs_curve.Convert()));
			cSDataArray2.Append(new CSOnGeometryData(onNurbsCurve2.Convert(), onXform3.Convert(), applyTransform: false));
			cSDataArray3.Append(new CSOnGeometryData(onNurbsCurve.Convert(), onXform2.Convert(), applyTransform: false));
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
