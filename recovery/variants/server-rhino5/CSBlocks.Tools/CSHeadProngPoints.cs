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
		//IL_00e1: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e8: Expected O, but got Unknown
		//IL_01e2: Unknown result type (might be due to invalid IL or missing references)
		//IL_01e9: Expected O, but got Unknown
		//IL_01e9: Unknown result type (might be due to invalid IL or missing references)
		//IL_01f0: Expected O, but got Unknown
		//IL_0222: Unknown result type (might be due to invalid IL or missing references)
		//IL_0229: Expected O, but got Unknown
		//IL_0249: Unknown result type (might be due to invalid IL or missing references)
		//IL_0250: Expected O, but got Unknown
		//IL_0271: Unknown result type (might be due to invalid IL or missing references)
		//IL_0278: Expected O, but got Unknown
		//IL_0282: Unknown result type (might be due to invalid IL or missing references)
		//IL_029c: Expected O, but got Unknown
		//IL_02a2: Unknown result type (might be due to invalid IL or missing references)
		//IL_02a9: Expected O, but got Unknown
		//IL_02ce: Unknown result type (might be due to invalid IL or missing references)
		//IL_02d5: Expected O, but got Unknown
		//IL_02df: Unknown result type (might be due to invalid IL or missing references)
		//IL_02f9: Expected O, but got Unknown
		//IL_02ff: Unknown result type (might be due to invalid IL or missing references)
		//IL_0306: Expected O, but got Unknown
		//IL_0340: Unknown result type (might be due to invalid IL or missing references)
		//IL_035a: Expected O, but got Unknown
		//IL_0360: Unknown result type (might be due to invalid IL or missing references)
		//IL_0367: Expected O, but got Unknown
		//IL_038e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0395: Expected O, but got Unknown
		//IL_039e: Unknown result type (might be due to invalid IL or missing references)
		//IL_03b8: Expected O, but got Unknown
		//IL_03be: Unknown result type (might be due to invalid IL or missing references)
		//IL_03c5: Expected O, but got Unknown
		//IL_03ec: Unknown result type (might be due to invalid IL or missing references)
		//IL_03f3: Expected O, but got Unknown
		//IL_03fc: Unknown result type (might be due to invalid IL or missing references)
		//IL_0416: Expected O, but got Unknown
		//IL_041c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0423: Expected O, but got Unknown
		//IL_044a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0451: Expected O, but got Unknown
		//IL_045a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0474: Expected O, but got Unknown
		//IL_047a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0481: Expected O, but got Unknown
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
			CSBlockUtil.CSUtilMath.GetUnscaledXforms((IOnXform)(object)cSGemData.Xform.Convert(), out var xUp, out var xDown);
			OnNurbsCurve val = cSGemData.GetNurbsCurve().Convert();
			if (val == null)
			{
				continue;
			}
			val.Transform((IOnXform)(object)xDown);
			if (!GemOffsetCurve.GemOffset(cSGemData, 0.0, out OnCurve curve, false, out string error, 0.0))
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
					cSDataArray4.Append(new CSOnGeometryData(((OnCurve)(object)val10).Convert(), xUp.Convert(), applyTransform: false));
					On3dVector val11 = new On3dVector((IOn3dVector)(object)obj);
					val11.Unitize();
					On3dPoint val12 = new On3dPoint((IOn3dPoint)(object)val6) + (IOn3dVector)(object)(val11 * 0.5);
					OnLineCurve val13 = new OnLineCurve((IOn3dPoint)(object)val6, (IOn3dPoint)(object)val12);
					val13.Transform((IOnXform)(object)xUp);
					cSDataArray4.Append(new CSOnGeometryData(((OnCurve)(object)val13).Convert(), xUp.Convert(), applyTransform: false));
					On3dVector val14 = OnUtil.ON_CrossProduct((IOn3dVector)(object)val11, (IOn3dVector)(object)val8);
					val14.Unitize();
					On3dPoint val15 = new On3dPoint((IOn3dPoint)(object)val6) + (IOn3dVector)(object)(val14 * 0.5);
					OnLineCurve val16 = new OnLineCurve((IOn3dPoint)(object)val6, (IOn3dPoint)(object)val15);
					val16.Transform((IOnXform)(object)xUp);
					cSDataArray4.Append(new CSOnGeometryData(((OnCurve)(object)val16).Convert(), xUp.Convert(), applyTransform: false));
					On3dVector val17 = new On3dVector((IOn3dVector)(object)val8);
					val17.Reverse();
					On3dPoint val18 = new On3dPoint((IOn3dPoint)(object)val6) + (IOn3dVector)(object)(val17 * 0.5);
					OnLineCurve val19 = new OnLineCurve((IOn3dPoint)(object)val6, (IOn3dPoint)(object)val18);
					val19.Transform((IOnXform)(object)xUp);
					cSDataArray4.Append(new CSOnGeometryData(((OnCurve)(object)val19).Convert(), xUp.Convert(), applyTransform: false));
					On3dVector val20 = new On3dVector((IOn3dVector)(object)val11);
					val20.Reverse();
					On3dPoint val21 = new On3dPoint((IOn3dPoint)(object)val6) + (IOn3dVector)(object)(val20 * 0.5);
					OnLineCurve val22 = new OnLineCurve((IOn3dPoint)(object)val6, (IOn3dPoint)(object)val21);
					val22.Transform((IOnXform)(object)xUp);
					cSDataArray4.Append(new CSOnGeometryData(((OnCurve)(object)val22).Convert(), xUp.Convert(), applyTransform: false));
					On3dVector val23 = new On3dVector((IOn3dVector)(object)val14);
					val23.Reverse();
					On3dPoint val24 = new On3dPoint((IOn3dPoint)(object)val6) + (IOn3dVector)(object)(val23 * 0.5);
					OnLineCurve val25 = new OnLineCurve((IOn3dPoint)(object)val6, (IOn3dPoint)(object)val24);
					val25.Transform((IOnXform)(object)xUp);
					cSDataArray4.Append(new CSOnGeometryData(((OnCurve)(object)val25).Convert(), xUp.Convert(), applyTransform: false));
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
