using System.Collections.Generic;
using System.Linq;
using CS.CSObjects.CSBlocks;
using CS.Parametric;
using CS.Parametric.Channels;
using CSBlockDataUtils.CSGemOffsetCurves;
using CSBlockUtil;
using CSBlocks.Settings.Heads.CSHeadBuilderV1;
using CSCompatibility;
using CSDatas;
using CSGemInstance;
using GV.CounterSketch.Shared;
using RMA.OpenNURBS;

namespace CSBlocks.Tools;

[CSBlock("HeadProngPoints", "{1FFFA8A1-C91D-411B-848E-5131BC4324A5}")]
public sealed class CSHeadProngPoints : CSBlock
{
	public CSInputChannel InGems { get; }

	public CSInputChannel InStyle { get; }

	public CSInputChannel InShowAll { get; }

	public CSOutputChannel OutPoints { get; }

	public CSOutputChannel OutCurves { get; }

	public CSHeadProngPoints(string identifier)
		: base(identifier)
	{
		InGems = new CSInputChannel(this, ChannelDataType.Gem, ChannelArrayType.Array);
		InStyle = new CSInputChannel(this, ChannelDataType.Integer, ChannelArrayType.Individual);
		InShowAll = new CSInputChannel(this, ChannelDataType.Boolean, ChannelArrayType.Individual);
		OutPoints = new CSOutputChannel(this, ChannelDataType.Point, ChannelArrayType.Array | ChannelArrayType.Individual);
		OutCurves = new CSOutputChannel(this, ChannelDataType.Curve, ChannelArrayType.Array | ChannelArrayType.Individual);
		SetChannelIdentifiers();
	}

	public override bool Update()
	{
		CSDataArray flattenArray = InGems.GetFlattenArray();
		if (flattenArray == null || flattenArray.GetArrayCount() <= 0)
		{
			return UpdateMissing("Gems");
		}
		HeadStyle whichHeadStyle = HeadStyle.Unknown;
		if (!InStyle.GetValueHeadStyle(ref whichHeadStyle))
		{
			return UpdateMissing("Style");
		}
		bool value = false;
		InShowAll.GetBoolean(ref value);
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
			if (!GemOffsetCurve.GemOffset(cSGemData, 0.0, out OnCurve curve, false, out string error, 0.0))
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
			if (value)
			{
				list.AddRange(prongPositions.Select((CSGemInstanceDefinition.ProngPosition prongPosition) => prongPosition.ProngParam));
			}
			else
			{
				IEnumerable<CSHeadStyles.HeadProngInfo> headProngInfo = CSHeadStyles.GetHeadProngInfo(whichHeadStyle);
				if (headProngInfo == null)
				{
					return UpdateError("Missing Prong Infos");
				}
				list.AddRange(headProngInfo.Where(delegate(CSHeadStyles.HeadProngInfo headProngInfo3)
				{
					CSHeadStyles.HeadProngInfo headProngInfo2 = headProngInfo3;
					if (headProngInfo2.ProngParamIndex >= 0)
					{
						headProngInfo2 = headProngInfo3;
						return headProngInfo2.ProngParamIndex < prongPositions.Count;
					}
					return false;
				}).Select(delegate(CSHeadStyles.HeadProngInfo headProngInfo3)
				{
					IList<CSGemInstanceDefinition.ProngPosition> list2 = prongPositions;
					CSHeadStyles.HeadProngInfo headProngInfo2 = headProngInfo3;
					return list2[headProngInfo2.ProngParamIndex].ProngParam;
				}));
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
					On3dPoint on3dPoint3 = new On3dPoint(on3dPoint2);
					on3dPoint3.Transform(xUp);
					cSDataArray3.Append(new CSPointData(on3dPoint3.Convert()));
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
			OutPoints.SetData(null);
		}
		else if (cSDataArray.GetArrayCount() == 1)
		{
			OutPoints.SetData(cSDataArray.GetArrayItem(0));
		}
		else
		{
			OutPoints.SetData(cSDataArray);
		}
		if (cSDataArray2.GetArrayCount() == 0)
		{
			OutCurves.SetData(null);
		}
		else if (cSDataArray2.GetArrayCount() == 1)
		{
			OutCurves.SetData(cSDataArray2.GetArrayItem(0));
		}
		else
		{
			OutCurves.SetData(cSDataArray2);
		}
		return base.Update();
	}
}
