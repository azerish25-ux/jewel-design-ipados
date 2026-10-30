using System;
using System.Collections.Generic;
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
using Rhino.Geometry;

namespace CSBlocks.Tools.Obsolete;

[Obsolete]
[CSBlock("HeadProngPoints_V1", "{77479DCE-BFD1-4D45-88D0-469DE8838B4E}")]
public sealed class CSHeadProngPoints1 : CSBlock
{
	public CSInputChannel iGems { get; }

	public CSInputChannel iStyle { get; }

	public CSInputChannel iProngAngle { get; }

	public CSInputChannel iGameChanger { get; }

	public CSOutputChannel oPoints { get; }

	public CSOutputChannel oCurves { get; }

	public CSHeadProngPoints1(string identifier)
		: base(identifier)
	{
		iGems = new CSInputChannel(GetPropertyName(() => iGems), this, ChannelDataType.Gem, ChannelArrayType.Array | ChannelArrayType.Individual);
		iStyle = new CSInputChannel(GetPropertyName(() => iStyle), this, ChannelDataType.Double, ChannelArrayType.Individual);
		iProngAngle = new CSInputChannel(GetPropertyName(() => iProngAngle), this, ChannelDataType.Double, ChannelArrayType.Individual);
		iGameChanger = new CSInputChannel(GetPropertyName(() => iGameChanger), this, ChannelDataType.Boolean, ChannelArrayType.Individual);
		oPoints = new CSOutputChannel(GetPropertyName(() => oPoints), this, ChannelDataType.Point, ChannelArrayType.Array | ChannelArrayType.Individual);
		oCurves = new CSOutputChannel(GetPropertyName(() => oCurves), this, ChannelDataType.Curve, ChannelArrayType.Array | ChannelArrayType.Individual);
	}

	public override bool Update()
	{
		//IL_008a: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e6: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ed: Expected O, but got Unknown
		//IL_01db: Unknown result type (might be due to invalid IL or missing references)
		//IL_01e2: Expected O, but got Unknown
		//IL_020a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0211: Expected O, but got Unknown
		//IL_021b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0222: Expected O, but got Unknown
		//IL_0254: Unknown result type (might be due to invalid IL or missing references)
		//IL_0334: Expected O, but got Unknown
		//IL_0335: Unknown result type (might be due to invalid IL or missing references)
		//IL_033c: Expected O, but got Unknown
		//IL_0349: Unknown result type (might be due to invalid IL or missing references)
		//IL_035d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0364: Expected O, but got Unknown
		//IL_036d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0387: Expected O, but got Unknown
		//IL_038c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0393: Expected O, but got Unknown
		//IL_03a8: Unknown result type (might be due to invalid IL or missing references)
		//IL_03ba: Unknown result type (might be due to invalid IL or missing references)
		//IL_03c1: Expected O, but got Unknown
		//IL_03ca: Unknown result type (might be due to invalid IL or missing references)
		//IL_03e4: Expected O, but got Unknown
		//IL_03e9: Unknown result type (might be due to invalid IL or missing references)
		//IL_03f0: Expected O, but got Unknown
		//IL_0405: Unknown result type (might be due to invalid IL or missing references)
		//IL_0429: Unknown result type (might be due to invalid IL or missing references)
		//IL_0443: Expected O, but got Unknown
		//IL_0448: Unknown result type (might be due to invalid IL or missing references)
		//IL_044f: Expected O, but got Unknown
		//IL_0464: Unknown result type (might be due to invalid IL or missing references)
		//IL_0476: Unknown result type (might be due to invalid IL or missing references)
		//IL_047d: Expected O, but got Unknown
		//IL_0485: Unknown result type (might be due to invalid IL or missing references)
		//IL_049f: Expected O, but got Unknown
		//IL_04a4: Unknown result type (might be due to invalid IL or missing references)
		//IL_04ab: Expected O, but got Unknown
		//IL_04c0: Unknown result type (might be due to invalid IL or missing references)
		//IL_04d2: Unknown result type (might be due to invalid IL or missing references)
		//IL_04d9: Expected O, but got Unknown
		//IL_04e1: Unknown result type (might be due to invalid IL or missing references)
		//IL_04fb: Expected O, but got Unknown
		//IL_0500: Unknown result type (might be due to invalid IL or missing references)
		//IL_0507: Expected O, but got Unknown
		//IL_051c: Unknown result type (might be due to invalid IL or missing references)
		//IL_052e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0535: Expected O, but got Unknown
		//IL_053d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0557: Expected O, but got Unknown
		//IL_055b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0562: Expected O, but got Unknown
		//IL_0577: Unknown result type (might be due to invalid IL or missing references)
		CSDataArray flattenArray = iGems.GetFlattenArray();
		if (flattenArray == null || flattenArray.GetArrayCount() <= 0)
		{
			return UpdateMissing("Gems");
		}
		int value = 0;
		iStyle.GetInteger(ref value);
		double value2 = 0.0;
		iProngAngle.GetDouble(ref value2);
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
			IList<CSGemInstanceDefinition.ProngPosition> list = new List<CSGemInstanceDefinition.ProngPosition>();
			if (cSGemData.Def != null)
			{
				list = cSGemData.Def.ProngPositions;
			}
			List<CSHeadStyles2.HeadProngInfo> list2 = (value3 ? CSHeadStyles2.GetHeadStyleGameChanger(cSGemData.GemShape, value, cSGemData.VersionMajor) : CSHeadStyles2.GetHeadStyle(cSGemData.GemShape, value, cSGemData.VersionMajor));
			if (list2 == null)
			{
				return UpdateError("Missing Prong Infos");
			}
			CSDataArray cSDataArray3 = new CSDataArray();
			CSDataArray cSDataArray4 = new CSDataArray();
			foreach (CSHeadStyles2.HeadProngInfo item in list2)
			{
				if (item.ProngParamIndex < 0 || item.ProngParamIndex >= list.Count)
				{
					continue;
				}
				double prongParam = list[item.ProngParamIndex].ProngParam;
				On3dPoint val3 = ((OnCurve)val).PointAt(prongParam);
				double num = CSBlockUtil.CSUtilMath.ToRadians(item.ProngParamAngleOffset + value2 * (double)Math.Sign(item.Mirror));
				OnXform val4 = new OnXform();
				val4.Rotation(num, OnUtil.On_zaxis, OnUtil.On_origin);
				OnLineCurve val5 = new OnLineCurve(OnUtil.On_origin, (IOn3dPoint)(object)(val3 * 20.0));
				val5.Transform((IOnXform)(object)val4);
				ArrayOnX_EVENT val6 = new ArrayOnX_EVENT();
				((OnCurve)val2).IntersectCurve((IOnCurve)(object)val5, ref val6);
				if (val6.Count() > 0)
				{
					double num2 = val6[0][0];
					On3dPoint val7 = new On3dPoint((IOn3dPoint)(object)((OnCurve)val2).PointAt(num2));
					double num3 = num2 - 0.01;
					if (num3 < ((OnCurve)val2).Domain().Min())
					{
						num3 += ((OnCurve)val2).Domain().Max() - ((OnCurve)val2).Domain().Min();
					}
					On3dVector val8 = ((OnCurve)val2).TangentAt(num3);
					On3dVector obj = OnUtil.ON_CrossProduct(OnUtil.On_zaxis, (IOn3dVector)(object)val8);
					obj.Unitize();
					double num4 = num2 + 0.01;
					if (num4 > ((OnCurve)val2).Domain().Max())
					{
						num4 -= ((OnCurve)val2).Domain().Max() - ((OnCurve)val2).Domain().Min();
					}
					On3dVector val9 = ((OnCurve)val2).TangentAt(num4);
					On3dVector val10 = OnUtil.ON_CrossProduct(OnUtil.On_zaxis, (IOn3dVector)(object)val9);
					val10.Unitize();
					On3dVector val11 = obj + (IOn3dVector)(object)val10;
					val11.Unitize();
					On3dPoint obj2 = val7 + (IOn3dVector)(object)(val11 * item.Nudge);
					On3dPoint val12 = new On3dPoint((IOn3dPoint)(object)obj2);
					val12.Transform((IOnXform)(object)xUp);
					cSDataArray3.Append(new CSPointData(val12.Convert()));
					On3dVector val13 = new On3dVector(OnUtil.On_zaxis);
					val13.Unitize();
					On3dPoint val14 = new On3dPoint((IOn3dPoint)(object)obj2) + (IOn3dVector)(object)(val13 * 0.5);
					OnLineCurve val15 = new OnLineCurve((IOn3dPoint)(object)obj2, (IOn3dPoint)(object)val14);
					val15.Transform((IOnXform)(object)xUp);
					cSDataArray4.Append(new CSOnGeometryData((GeometryBase)(object)((OnCurve)(object)val15).Convert(), xUp.Convert(), applyTransform: false));
					On3dVector val16 = new On3dVector((IOn3dVector)(object)val11);
					val16.Unitize();
					On3dPoint val17 = new On3dPoint((IOn3dPoint)(object)obj2) + (IOn3dVector)(object)(val16 * 0.5);
					OnLineCurve val18 = new OnLineCurve((IOn3dPoint)(object)obj2, (IOn3dPoint)(object)val17);
					val18.Transform((IOnXform)(object)xUp);
					cSDataArray4.Append(new CSOnGeometryData((GeometryBase)(object)((OnCurve)(object)val18).Convert(), xUp.Convert(), applyTransform: false));
					On3dVector val19 = OnUtil.ON_CrossProduct((IOn3dVector)(object)val16, (IOn3dVector)(object)val13);
					val19.Unitize();
					On3dPoint val20 = new On3dPoint((IOn3dPoint)(object)obj2) + (IOn3dVector)(object)(val19 * 0.5);
					OnLineCurve val21 = new OnLineCurve((IOn3dPoint)(object)obj2, (IOn3dPoint)(object)val20);
					val21.Transform((IOnXform)(object)xUp);
					cSDataArray4.Append(new CSOnGeometryData((GeometryBase)(object)((OnCurve)(object)val21).Convert(), xUp.Convert(), applyTransform: false));
					On3dVector val22 = new On3dVector((IOn3dVector)(object)val13);
					val22.Reverse();
					On3dPoint val23 = new On3dPoint((IOn3dPoint)(object)obj2) + (IOn3dVector)(object)(val22 * 0.5);
					OnLineCurve val24 = new OnLineCurve((IOn3dPoint)(object)obj2, (IOn3dPoint)(object)val23);
					val24.Transform((IOnXform)(object)xUp);
					cSDataArray4.Append(new CSOnGeometryData((GeometryBase)(object)((OnCurve)(object)val24).Convert(), xUp.Convert(), applyTransform: false));
					On3dVector val25 = new On3dVector((IOn3dVector)(object)val16);
					val25.Reverse();
					On3dPoint val26 = new On3dPoint((IOn3dPoint)(object)obj2) + (IOn3dVector)(object)(val25 * 0.5);
					OnLineCurve val27 = new OnLineCurve((IOn3dPoint)(object)obj2, (IOn3dPoint)(object)val26);
					val27.Transform((IOnXform)(object)xUp);
					cSDataArray4.Append(new CSOnGeometryData((GeometryBase)(object)((OnCurve)(object)val27).Convert(), xUp.Convert(), applyTransform: false));
					On3dVector val28 = new On3dVector((IOn3dVector)(object)val19);
					val28.Reverse();
					On3dPoint val29 = new On3dPoint((IOn3dPoint)(object)obj2) + (IOn3dVector)(object)(val28 * 0.5);
					OnLineCurve val30 = new OnLineCurve((IOn3dPoint)(object)obj2, (IOn3dPoint)(object)val29);
					val30.Transform((IOnXform)(object)xUp);
					cSDataArray4.Append(new CSOnGeometryData((GeometryBase)(object)((OnCurve)(object)val30).Convert(), xUp.Convert(), applyTransform: false));
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
