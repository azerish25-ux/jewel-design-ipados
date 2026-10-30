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
		//IL_00e5: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ec: Expected O, but got Unknown
		//IL_01ff: Unknown result type (might be due to invalid IL or missing references)
		//IL_0206: Expected O, but got Unknown
		//IL_0206: Unknown result type (might be due to invalid IL or missing references)
		//IL_020d: Expected O, but got Unknown
		//IL_023f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0246: Expected O, but got Unknown
		//IL_0265: Unknown result type (might be due to invalid IL or missing references)
		//IL_0289: Unknown result type (might be due to invalid IL or missing references)
		//IL_0290: Expected O, but got Unknown
		//IL_029a: Unknown result type (might be due to invalid IL or missing references)
		//IL_02b4: Expected O, but got Unknown
		//IL_02ba: Unknown result type (might be due to invalid IL or missing references)
		//IL_02c1: Expected O, but got Unknown
		//IL_02e6: Unknown result type (might be due to invalid IL or missing references)
		//IL_02ed: Expected O, but got Unknown
		//IL_02f7: Unknown result type (might be due to invalid IL or missing references)
		//IL_0311: Expected O, but got Unknown
		//IL_0317: Unknown result type (might be due to invalid IL or missing references)
		//IL_031e: Expected O, but got Unknown
		//IL_0358: Unknown result type (might be due to invalid IL or missing references)
		//IL_0372: Expected O, but got Unknown
		//IL_0378: Unknown result type (might be due to invalid IL or missing references)
		//IL_037f: Expected O, but got Unknown
		//IL_03a6: Unknown result type (might be due to invalid IL or missing references)
		//IL_03ad: Expected O, but got Unknown
		//IL_03b6: Unknown result type (might be due to invalid IL or missing references)
		//IL_03d0: Expected O, but got Unknown
		//IL_03d6: Unknown result type (might be due to invalid IL or missing references)
		//IL_03dd: Expected O, but got Unknown
		//IL_0404: Unknown result type (might be due to invalid IL or missing references)
		//IL_040b: Expected O, but got Unknown
		//IL_0414: Unknown result type (might be due to invalid IL or missing references)
		//IL_042e: Expected O, but got Unknown
		//IL_0434: Unknown result type (might be due to invalid IL or missing references)
		//IL_043b: Expected O, but got Unknown
		//IL_0462: Unknown result type (might be due to invalid IL or missing references)
		//IL_0469: Expected O, but got Unknown
		//IL_0472: Unknown result type (might be due to invalid IL or missing references)
		//IL_048c: Expected O, but got Unknown
		//IL_0492: Unknown result type (might be due to invalid IL or missing references)
		//IL_0499: Expected O, but got Unknown
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
					new On3dPoint((IOn3dVector)(object)obj).Transform((IOnXform)(object)xUp);
					cSDataArray3.Append(new CSPointData(val6.Convert()));
					On3dVector val7 = new On3dVector(OnUtil.On_zaxis);
					val7.Unitize();
					On3dPoint val8 = new On3dPoint((IOn3dPoint)(object)val6) + (IOn3dVector)(object)(val7 * 0.5);
					OnLineCurve val9 = new OnLineCurve((IOn3dPoint)(object)val6, (IOn3dPoint)(object)val8);
					val9.Transform((IOnXform)(object)xUp);
					cSDataArray4.Append(new CSOnGeometryData(((OnCurve)(object)val9).Convert(), xUp.Convert(), applyTransform: false));
					On3dVector val10 = new On3dVector((IOn3dVector)(object)obj);
					val10.Unitize();
					On3dPoint val11 = new On3dPoint((IOn3dPoint)(object)val6) + (IOn3dVector)(object)(val10 * 0.5);
					OnLineCurve val12 = new OnLineCurve((IOn3dPoint)(object)val6, (IOn3dPoint)(object)val11);
					val12.Transform((IOnXform)(object)xUp);
					cSDataArray4.Append(new CSOnGeometryData(((OnCurve)(object)val12).Convert(), xUp.Convert(), applyTransform: false));
					On3dVector val13 = OnUtil.ON_CrossProduct((IOn3dVector)(object)val10, (IOn3dVector)(object)val7);
					val13.Unitize();
					On3dPoint val14 = new On3dPoint((IOn3dPoint)(object)val6) + (IOn3dVector)(object)(val13 * 0.5);
					OnLineCurve val15 = new OnLineCurve((IOn3dPoint)(object)val6, (IOn3dPoint)(object)val14);
					val15.Transform((IOnXform)(object)xUp);
					cSDataArray4.Append(new CSOnGeometryData(((OnCurve)(object)val15).Convert(), xUp.Convert(), applyTransform: false));
					On3dVector val16 = new On3dVector((IOn3dVector)(object)val7);
					val16.Reverse();
					On3dPoint val17 = new On3dPoint((IOn3dPoint)(object)val6) + (IOn3dVector)(object)(val16 * 0.5);
					OnLineCurve val18 = new OnLineCurve((IOn3dPoint)(object)val6, (IOn3dPoint)(object)val17);
					val18.Transform((IOnXform)(object)xUp);
					cSDataArray4.Append(new CSOnGeometryData(((OnCurve)(object)val18).Convert(), xUp.Convert(), applyTransform: false));
					On3dVector val19 = new On3dVector((IOn3dVector)(object)val10);
					val19.Reverse();
					On3dPoint val20 = new On3dPoint((IOn3dPoint)(object)val6) + (IOn3dVector)(object)(val19 * 0.5);
					OnLineCurve val21 = new OnLineCurve((IOn3dPoint)(object)val6, (IOn3dPoint)(object)val20);
					val21.Transform((IOnXform)(object)xUp);
					cSDataArray4.Append(new CSOnGeometryData(((OnCurve)(object)val21).Convert(), xUp.Convert(), applyTransform: false));
					On3dVector val22 = new On3dVector((IOn3dVector)(object)val13);
					val22.Reverse();
					On3dPoint val23 = new On3dPoint((IOn3dPoint)(object)val6) + (IOn3dVector)(object)(val22 * 0.5);
					OnLineCurve val24 = new OnLineCurve((IOn3dPoint)(object)val6, (IOn3dPoint)(object)val23);
					val24.Transform((IOnXform)(object)xUp);
					cSDataArray4.Append(new CSOnGeometryData(((OnCurve)(object)val24).Convert(), xUp.Convert(), applyTransform: false));
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
