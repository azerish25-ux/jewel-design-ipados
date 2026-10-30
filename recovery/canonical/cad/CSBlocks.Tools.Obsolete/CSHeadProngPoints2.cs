using System;
using System.Collections.Generic;
using System.Linq;
using CS.CSObjects.CSBlocks;
using CS.Parametric;
using CS.Parametric.Channels;
using CSBlockDataUtils.CSGemOffsetCurves;
using CSBlockUtil;
using CSBlocks.Settings.Heads.CSHeadBuilderV1.Obsolete;
using CSCompatibility;
using CSDatas;
using CSGemInstance;
using RMA.OpenNURBS;

namespace CSBlocks.Tools.Obsolete;

[Obsolete]
[CSBlock("HeadProngPoints_V2", "{020858C4-ACC1-4D36-916B-C8C2431C306F}")]
public sealed class CSHeadProngPoints2 : CSBlock
{
	public CSInputChannel iGems { get; }

	public CSInputChannel iStyle { get; }

	public CSInputChannel iShowAll { get; }

	public CSInputChannel iGameChanger { get; }

	public CSOutputChannel oPoints { get; }

	public CSOutputChannel oCurves { get; }

	public CSHeadProngPoints2(string identifier)
		: base(identifier)
	{
		iGems = new CSInputChannel(GetPropertyName(() => iGems), this, ChannelDataType.Gem, ChannelArrayType.Array | ChannelArrayType.Individual);
		iStyle = new CSInputChannel(GetPropertyName(() => iStyle), this, ChannelDataType.Double, ChannelArrayType.Individual);
		iShowAll = new CSInputChannel(GetPropertyName(() => iShowAll), this, ChannelDataType.Boolean, ChannelArrayType.Individual);
		iGameChanger = new CSInputChannel(GetPropertyName(() => iGameChanger), this, ChannelDataType.Boolean, ChannelArrayType.Individual);
		oPoints = new CSOutputChannel(GetPropertyName(() => oPoints), this, ChannelDataType.Point, ChannelArrayType.Array | ChannelArrayType.Individual);
		oCurves = new CSOutputChannel(GetPropertyName(() => oCurves), this, ChannelDataType.Curve, ChannelArrayType.Array | ChannelArrayType.Individual);
	}

	public override bool Update()
	{
		CSDataArray flattenArray = iGems.GetFlattenArray();
		if (flattenArray == null || flattenArray.GetArrayCount() <= 0)
		{
			return UpdateMissing("Gems");
		}
		int value = 0;
		iStyle.GetInteger(ref value);
		bool value2 = false;
		iShowAll.GetBoolean(ref value2);
		bool value3 = false;
		iGameChanger.GetBoolean(ref value3);
		CSDataArray cSDataArray = new CSDataArray();
		CSDataArray cSDataArray2 = new CSDataArray();
		for (int i = 0; i < flattenArray.GetArrayCount(); i++)
		{
			if (!(flattenArray.GetArrayItem(i) is CSGemData cSGemData))
			{
				continue;
			}
			CSBlockUtil.CSUtilMath.GetUnscaledXforms(cSGemData.Xform.Convert(), out var xUp, out var xDown);
			OnNurbsCurve onNurbsCurve = cSGemData.GetNurbsCurve().Convert();
			if (onNurbsCurve == null)
			{
				continue;
			}
			onNurbsCurve.Transform(xDown);
			if (!GemOffsetCurveV2.GemOffset(cSGemData, 0.0, out OnCurve curve, false, out string error, 0.0))
			{
				return UpdateError(error);
			}
			OnNurbsCurve nurbs_curve = new OnNurbsCurve();
			curve.GetNurbForm(ref nurbs_curve);
			IList<CSGemInstanceDefinition.ProngPosition> prongPositions = new List<CSGemInstanceDefinition.ProngPosition>();
			if (cSGemData.Def != null)
			{
				prongPositions = cSGemData.Def.ProngPositions;
			}
			List<double> list = new List<double>();
			if (value2)
			{
				list.AddRange(prongPositions.Select((CSGemInstanceDefinition.ProngPosition prongPosition) => prongPosition.ProngParam));
			}
			else
			{
				List<CSHeadStyles3.HeadProngInfo> list2 = (value3 ? CSHeadStyles3.GetHeadStyleGameChanger(cSGemData.GemShape, value) : CSHeadStyles3.GetHeadStyle(cSGemData.GemShape, value));
				if (list2 == null)
				{
					return UpdateError("Missing Prong Infos");
				}
				list.AddRange(from headProngInfo in list2
					where headProngInfo.ProngParamIndex >= 0 && headProngInfo.ProngParamIndex < prongPositions.Count
					select prongPositions[headProngInfo.ProngParamIndex].ProngParam);
			}
			CSDataArray cSDataArray3 = new CSDataArray();
			CSDataArray cSDataArray4 = new CSDataArray();
			foreach (double item in list)
			{
				On3dPoint on3dPoint = onNurbsCurve.PointAt(item);
				OnLineCurve curveB = new OnLineCurve(OnUtil.On_origin, on3dPoint * 50.0);
				ArrayOnX_EVENT x = new ArrayOnX_EVENT();
				nurbs_curve.IntersectCurve(curveB, ref x);
				if (x.Count() > 0)
				{
					double t = x[0].get_m_a(0);
					On3dPoint on3dPoint2 = new On3dPoint(nurbs_curve.PointAt(t));
					On3dVector on3dVector = CSBlockUtil.CSUtilMath.AverageNormalAt(nurbs_curve, OnUtil.On_zaxis, on3dPoint2);
					on3dVector.Unitize();
					new On3dPoint(on3dVector).Transform(xUp);
					cSDataArray3.Append(new CSPointData(on3dPoint2.Convert()));
					On3dVector on3dVector2 = new On3dVector(OnUtil.On_zaxis);
					on3dVector2.Unitize();
					On3dPoint arg = new On3dPoint(on3dPoint2) + on3dVector2 * 0.5;
					OnLineCurve onLineCurve = new OnLineCurve(on3dPoint2, arg);
					onLineCurve.Transform(xUp);
					cSDataArray4.Append(new CSOnGeometryData(onLineCurve.Convert(), xUp.Convert(), applyTransform: false));
					On3dVector on3dVector3 = new On3dVector(on3dVector);
					on3dVector3.Unitize();
					On3dPoint arg2 = new On3dPoint(on3dPoint2) + on3dVector3 * 0.5;
					OnLineCurve onLineCurve2 = new OnLineCurve(on3dPoint2, arg2);
					onLineCurve2.Transform(xUp);
					cSDataArray4.Append(new CSOnGeometryData(onLineCurve2.Convert(), xUp.Convert(), applyTransform: false));
					On3dVector on3dVector4 = OnUtil.ON_CrossProduct(on3dVector3, on3dVector2);
					on3dVector4.Unitize();
					On3dPoint arg3 = new On3dPoint(on3dPoint2) + on3dVector4 * 0.5;
					OnLineCurve onLineCurve3 = new OnLineCurve(on3dPoint2, arg3);
					onLineCurve3.Transform(xUp);
					cSDataArray4.Append(new CSOnGeometryData(onLineCurve3.Convert(), xUp.Convert(), applyTransform: false));
					On3dVector on3dVector5 = new On3dVector(on3dVector2);
					on3dVector5.Reverse();
					On3dPoint arg4 = new On3dPoint(on3dPoint2) + on3dVector5 * 0.5;
					OnLineCurve onLineCurve4 = new OnLineCurve(on3dPoint2, arg4);
					onLineCurve4.Transform(xUp);
					cSDataArray4.Append(new CSOnGeometryData(onLineCurve4.Convert(), xUp.Convert(), applyTransform: false));
					On3dVector on3dVector6 = new On3dVector(on3dVector3);
					on3dVector6.Reverse();
					On3dPoint arg5 = new On3dPoint(on3dPoint2) + on3dVector6 * 0.5;
					OnLineCurve onLineCurve5 = new OnLineCurve(on3dPoint2, arg5);
					onLineCurve5.Transform(xUp);
					cSDataArray4.Append(new CSOnGeometryData(onLineCurve5.Convert(), xUp.Convert(), applyTransform: false));
					On3dVector on3dVector7 = new On3dVector(on3dVector4);
					on3dVector7.Reverse();
					On3dPoint arg6 = new On3dPoint(on3dPoint2) + on3dVector7 * 0.5;
					OnLineCurve onLineCurve6 = new OnLineCurve(on3dPoint2, arg6);
					onLineCurve6.Transform(xUp);
					cSDataArray4.Append(new CSOnGeometryData(onLineCurve6.Convert(), xUp.Convert(), applyTransform: false));
				}
			}
			cSDataArray.Append(cSDataArray3);
			cSDataArray2.Append(cSDataArray4);
		}
		if (cSDataArray.GetArrayCount() == 0)
		{
			oPoints.SetData(null);
		}
		else if (cSDataArray.GetArrayCount() == 1)
		{
			oPoints.SetData(cSDataArray.GetArrayItem(0));
		}
		else
		{
			oPoints.SetData(cSDataArray);
		}
		if (cSDataArray2.GetArrayCount() == 0)
		{
			oCurves.SetData(null);
		}
		else if (cSDataArray2.GetArrayCount() == 1)
		{
			oCurves.SetData(cSDataArray2.GetArrayItem(0));
		}
		else
		{
			oCurves.SetData(cSDataArray2);
		}
		return base.Update();
	}
}
