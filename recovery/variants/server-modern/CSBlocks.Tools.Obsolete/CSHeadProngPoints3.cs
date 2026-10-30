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
using GV.CounterSketch.Shared;
using RMA.OpenNURBS;
using Rhino.Geometry;

namespace CSBlocks.Tools.Obsolete;

[Obsolete]
[CSBlock("HeadProngPoints_V3", "{8CECDC28-48FD-481A-93F6-68D721FE5916}")]
public sealed class CSHeadProngPoints3 : CSBlock
{
	public CSInputChannel InGems { get; }

	public CSInputChannel InStyle { get; }

	public CSInputChannel InShowAll { get; }

	public CSOutputChannel OutPoints { get; }

	public CSOutputChannel OutCurves { get; }

	public CSHeadProngPoints3(string identifier)
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
		//IL_0079: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d5: Unknown result type (might be due to invalid IL or missing references)
		//IL_00dc: Expected O, but got Unknown
		//IL_01dd: Unknown result type (might be due to invalid IL or missing references)
		//IL_01e4: Expected O, but got Unknown
		//IL_01e4: Unknown result type (might be due to invalid IL or missing references)
		//IL_01eb: Expected O, but got Unknown
		//IL_021d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0224: Expected O, but got Unknown
		//IL_0244: Unknown result type (might be due to invalid IL or missing references)
		//IL_024b: Expected O, but got Unknown
		//IL_0258: Unknown result type (might be due to invalid IL or missing references)
		//IL_026c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0273: Expected O, but got Unknown
		//IL_027d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0297: Expected O, but got Unknown
		//IL_029d: Unknown result type (might be due to invalid IL or missing references)
		//IL_02a4: Expected O, but got Unknown
		//IL_02b9: Unknown result type (might be due to invalid IL or missing references)
		//IL_02c9: Unknown result type (might be due to invalid IL or missing references)
		//IL_02d0: Expected O, but got Unknown
		//IL_02da: Unknown result type (might be due to invalid IL or missing references)
		//IL_02f4: Expected O, but got Unknown
		//IL_02fa: Unknown result type (might be due to invalid IL or missing references)
		//IL_0301: Expected O, but got Unknown
		//IL_0316: Unknown result type (might be due to invalid IL or missing references)
		//IL_033b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0355: Expected O, but got Unknown
		//IL_035b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0362: Expected O, but got Unknown
		//IL_0377: Unknown result type (might be due to invalid IL or missing references)
		//IL_0389: Unknown result type (might be due to invalid IL or missing references)
		//IL_0390: Expected O, but got Unknown
		//IL_0399: Unknown result type (might be due to invalid IL or missing references)
		//IL_03b3: Expected O, but got Unknown
		//IL_03b9: Unknown result type (might be due to invalid IL or missing references)
		//IL_03c0: Expected O, but got Unknown
		//IL_03d5: Unknown result type (might be due to invalid IL or missing references)
		//IL_03e7: Unknown result type (might be due to invalid IL or missing references)
		//IL_03ee: Expected O, but got Unknown
		//IL_03f7: Unknown result type (might be due to invalid IL or missing references)
		//IL_0411: Expected O, but got Unknown
		//IL_0417: Unknown result type (might be due to invalid IL or missing references)
		//IL_041e: Expected O, but got Unknown
		//IL_0433: Unknown result type (might be due to invalid IL or missing references)
		//IL_0445: Unknown result type (might be due to invalid IL or missing references)
		//IL_044c: Expected O, but got Unknown
		//IL_0455: Unknown result type (might be due to invalid IL or missing references)
		//IL_046f: Expected O, but got Unknown
		//IL_0475: Unknown result type (might be due to invalid IL or missing references)
		//IL_047c: Expected O, but got Unknown
		//IL_0491: Unknown result type (might be due to invalid IL or missing references)
		CSDataArray flattenArray = InGems.GetFlattenArray();
		if (flattenArray == null || flattenArray.GetArrayCount() <= 0)
		{
			return UpdateMissing("Gems");
		}
		ValueListItem whichItem = ValueListItem.HeadStyleCenter1;
		InStyle.GetValueListItem(ref whichItem);
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
			CSBlockUtil.CSUtilMath.GetUnscaledXforms((IOnXform)(object)cSGemData.Xform.Convert(), out var xUp, out var xDown);
			OnNurbsCurve val = cSGemData.GetNurbsCurve().Convert();
			if (val == null)
			{
				continue;
			}
			val.Transform((IOnXform)(object)xDown);
			if (!GemOffsetCurveV2.GemOffset(cSGemData, 0.0, out OnCurve curve, false, out string error, 0.0))
			{
				return UpdateError(error);
			}
			OnNurbsCurve val2 = new OnNurbsCurve();
			curve.GetNurbForm(ref val2);
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
				List<CSHeadStyles5.HeadProngInfo> headStyle = CSHeadStyles5.GetHeadStyle(cSGemData.GemShape, whichItem);
				if (headStyle == null)
				{
					return UpdateError("Missing Prong Infos");
				}
				list.AddRange(headStyle.Where(delegate(CSHeadStyles5.HeadProngInfo headProngInfo)
				{
					CSHeadStyles5.HeadProngInfo headProngInfo2 = headProngInfo;
					if (headProngInfo2.ProngParamIndex >= 0)
					{
						headProngInfo2 = headProngInfo;
						return headProngInfo2.ProngParamIndex < prongPositions.Count;
					}
					return false;
				}).Select(delegate(CSHeadStyles5.HeadProngInfo headProngInfo)
				{
					IList<CSGemInstanceDefinition.ProngPosition> list2 = prongPositions;
					CSHeadStyles5.HeadProngInfo headProngInfo2 = headProngInfo;
					return list2[headProngInfo2.ProngParamIndex].ProngParam;
				}));
			}
			CSDataArray cSDataArray3 = new CSDataArray();
			CSDataArray cSDataArray4 = new CSDataArray();
			foreach (double item in list)
			{
				On3dPoint val3 = ((OnCurve)val).PointAt(item);
				OnLineCurve val4 = new OnLineCurve(OnUtil.On_origin, (IOn3dPoint)(object)(val3 * 50.0));
				ArrayOnX_EVENT val5 = new ArrayOnX_EVENT();
				((OnCurve)val2).IntersectCurve((IOnCurve)(object)val4, ref val5);
				if (val5.Count() > 0)
				{
					double num = val5[0][0];
					On3dPoint val6 = new On3dPoint((IOn3dPoint)(object)((OnCurve)val2).PointAt(num));
					On3dVector obj = CSBlockUtil.CSUtilMath.AverageNormalAt((IOnCurve)(object)val2, OnUtil.On_zaxis, (IOn3dPoint)(object)val6);
					obj.Unitize();
					On3dPoint val7 = new On3dPoint((IOn3dPoint)(object)val6);
					val7.Transform((IOnXform)(object)xUp);
					cSDataArray3.Append(new CSPointData(val7.Convert()));
					On3dVector val8 = new On3dVector(OnUtil.On_zaxis);
					val8.Unitize();
					On3dPoint val9 = new On3dPoint((IOn3dPoint)(object)val6) + (IOn3dVector)(object)(val8 * 0.5);
					OnLineCurve val10 = new OnLineCurve((IOn3dPoint)(object)val6, (IOn3dPoint)(object)val9);
					val10.Transform((IOnXform)(object)xUp);
					cSDataArray4.Append(new CSOnGeometryData((GeometryBase)(object)((OnCurve)(object)val10).Convert(), xUp.Convert(), applyTransform: false));
					On3dVector val11 = new On3dVector((IOn3dVector)(object)obj);
					val11.Unitize();
					On3dPoint val12 = new On3dPoint((IOn3dPoint)(object)val6) + (IOn3dVector)(object)(val11 * 0.5);
					OnLineCurve val13 = new OnLineCurve((IOn3dPoint)(object)val6, (IOn3dPoint)(object)val12);
					val13.Transform((IOnXform)(object)xUp);
					cSDataArray4.Append(new CSOnGeometryData((GeometryBase)(object)((OnCurve)(object)val13).Convert(), xUp.Convert(), applyTransform: false));
					On3dVector val14 = OnUtil.ON_CrossProduct((IOn3dVector)(object)val11, (IOn3dVector)(object)val8);
					val14.Unitize();
					On3dPoint val15 = new On3dPoint((IOn3dPoint)(object)val6) + (IOn3dVector)(object)(val14 * 0.5);
					OnLineCurve val16 = new OnLineCurve((IOn3dPoint)(object)val6, (IOn3dPoint)(object)val15);
					val16.Transform((IOnXform)(object)xUp);
					cSDataArray4.Append(new CSOnGeometryData((GeometryBase)(object)((OnCurve)(object)val16).Convert(), xUp.Convert(), applyTransform: false));
					On3dVector val17 = new On3dVector((IOn3dVector)(object)val8);
					val17.Reverse();
					On3dPoint val18 = new On3dPoint((IOn3dPoint)(object)val6) + (IOn3dVector)(object)(val17 * 0.5);
					OnLineCurve val19 = new OnLineCurve((IOn3dPoint)(object)val6, (IOn3dPoint)(object)val18);
					val19.Transform((IOnXform)(object)xUp);
					cSDataArray4.Append(new CSOnGeometryData((GeometryBase)(object)((OnCurve)(object)val19).Convert(), xUp.Convert(), applyTransform: false));
					On3dVector val20 = new On3dVector((IOn3dVector)(object)val11);
					val20.Reverse();
					On3dPoint val21 = new On3dPoint((IOn3dPoint)(object)val6) + (IOn3dVector)(object)(val20 * 0.5);
					OnLineCurve val22 = new OnLineCurve((IOn3dPoint)(object)val6, (IOn3dPoint)(object)val21);
					val22.Transform((IOnXform)(object)xUp);
					cSDataArray4.Append(new CSOnGeometryData((GeometryBase)(object)((OnCurve)(object)val22).Convert(), xUp.Convert(), applyTransform: false));
					On3dVector val23 = new On3dVector((IOn3dVector)(object)val14);
					val23.Reverse();
					On3dPoint val24 = new On3dPoint((IOn3dPoint)(object)val6) + (IOn3dVector)(object)(val23 * 0.5);
					OnLineCurve val25 = new OnLineCurve((IOn3dPoint)(object)val6, (IOn3dPoint)(object)val24);
					val25.Transform((IOnXform)(object)xUp);
					cSDataArray4.Append(new CSOnGeometryData((GeometryBase)(object)((OnCurve)(object)val25).Convert(), xUp.Convert(), applyTransform: false));
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
