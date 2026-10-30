using System;
using System.Collections.Generic;
using CS;
using CS.CSObjects.CSBlocks;
using CS.Parametric;
using CS.Parametric.Channels;
using CSBlockDataUtils.CSGemOffsetCurves;
using CSBlockUtil;
using CSCompatibility;
using CSDatas;
using RMA.OpenNURBS;
using RMA.Rhino;
using Rhino.Geometry;

namespace CSBlocks.GemClusters.Obsolete;

[Obsolete]
[CSBlock("ClusterMetalChannel_V3", "{366CF44C-44D1-42CB-AB80-DFBF6FE94A00}")]
public sealed class CSClusterMetalChannel3 : CSBlock
{
	public CSOutputChannel oMetal { get; }

	public CSInputChannel iCenterGem { get; }

	public CSInputChannel iCenterGemShapeOverrideEnabled { get; }

	public CSInputChannel iCenterGemShapeOverride { get; }

	public CSInputChannel iCenterGemSpacing { get; }

	public CSInputChannel iProfile { get; }

	public CSInputChannel iProfileWidth { get; }

	public CSInputChannel iProfileHeight { get; }

	public CSInputChannel iChannelWallThicknessInner { get; }

	public CSInputChannel iChannelWallThicknessOuter { get; }

	public CSInputChannel iChannelSpacing { get; }

	public CSInputChannel iChannelWallDepth { get; }

	public CSInputChannel iAngle { get; }

	public CSInputChannel iPlacement { get; }

	public CSInputChannel iCornerBlend { get; }

	public CSInputChannel iGemSizeZ { get; }

	public CSInputChannel iShowChannel { get; }

	public CSInputChannel iXYScaleToGem { get; }

	public CSClusterMetalChannel3(string identifier)
		: base(identifier)
	{
		iCenterGem = new CSInputChannel(GetPropertyName(() => iCenterGem), this, ChannelDataType.Gem, ChannelArrayType.Array | ChannelArrayType.Individual);
		iCenterGemShapeOverrideEnabled = new CSInputChannel(GetPropertyName(() => iCenterGemShapeOverrideEnabled), this, ChannelDataType.Boolean, ChannelArrayType.Individual);
		iCenterGemShapeOverride = new CSInputChannel(GetPropertyName(() => iCenterGemShapeOverride), this, ChannelDataType.Boolean, ChannelArrayType.Individual);
		iCenterGemSpacing = new CSInputChannel(GetPropertyName(() => iCenterGemSpacing), this, ChannelDataType.Double, ChannelArrayType.Individual);
		iProfile = new CSInputChannel(GetPropertyName(() => iProfile), this, ChannelDataType.Curve, ChannelArrayType.Individual);
		iProfileWidth = new CSInputChannel(GetPropertyName(() => iProfileWidth), this, ChannelDataType.Double, ChannelArrayType.Individual);
		iProfileHeight = new CSInputChannel(GetPropertyName(() => iProfileHeight), this, ChannelDataType.Double, ChannelArrayType.Individual);
		iChannelWallThicknessInner = new CSInputChannel(GetPropertyName(() => iChannelWallThicknessInner), this, ChannelDataType.Double, ChannelArrayType.Individual);
		iChannelWallThicknessOuter = new CSInputChannel(GetPropertyName(() => iChannelWallThicknessOuter), this, ChannelDataType.Double, ChannelArrayType.Individual);
		iChannelSpacing = new CSInputChannel(GetPropertyName(() => iChannelSpacing), this, ChannelDataType.Double, ChannelArrayType.Individual);
		iChannelWallDepth = new CSInputChannel(GetPropertyName(() => iChannelWallDepth), this, ChannelDataType.Double, ChannelArrayType.Individual);
		iAngle = new CSInputChannel(GetPropertyName(() => iAngle), this, ChannelDataType.Double, ChannelArrayType.Individual);
		iPlacement = new CSInputChannel(GetPropertyName(() => iPlacement), this, ChannelDataType.Double, ChannelArrayType.Individual);
		iCornerBlend = new CSInputChannel(GetPropertyName(() => iCornerBlend), this, ChannelDataType.Double, ChannelArrayType.Individual);
		iGemSizeZ = new CSInputChannel(GetPropertyName(() => iGemSizeZ), this, ChannelDataType.Double, ChannelArrayType.Individual);
		iShowChannel = new CSInputChannel(GetPropertyName(() => iShowChannel), this, ChannelDataType.Boolean, ChannelArrayType.Individual);
		iXYScaleToGem = new CSInputChannel(GetPropertyName(() => iXYScaleToGem), this, ChannelDataType.Boolean, ChannelArrayType.Individual);
		oMetal = new CSOutputChannel(GetPropertyName(() => oMetal), this, ChannelDataType.Any, ChannelArrayType.Array);
	}

	public override bool Update()
	{
		//IL_0191: Unknown result type (might be due to invalid IL or missing references)
		//IL_0198: Expected O, but got Unknown
		//IL_01a1: Unknown result type (might be due to invalid IL or missing references)
		//IL_01a8: Expected O, but got Unknown
		//IL_01d2: Unknown result type (might be due to invalid IL or missing references)
		//IL_01d9: Expected O, but got Unknown
		//IL_022f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0236: Expected O, but got Unknown
		//IL_06e6: Unknown result type (might be due to invalid IL or missing references)
		//IL_06ed: Expected O, but got Unknown
		//IL_0283: Unknown result type (might be due to invalid IL or missing references)
		//IL_029c: Unknown result type (might be due to invalid IL or missing references)
		//IL_02a6: Expected O, but got Unknown
		//IL_02a6: Expected O, but got Unknown
		//IL_02a1: Unknown result type (might be due to invalid IL or missing references)
		//IL_02a8: Expected O, but got Unknown
		//IL_02bf: Unknown result type (might be due to invalid IL or missing references)
		//IL_02db: Unknown result type (might be due to invalid IL or missing references)
		//IL_02e5: Expected O, but got Unknown
		//IL_02e5: Expected O, but got Unknown
		//IL_02e0: Unknown result type (might be due to invalid IL or missing references)
		//IL_02e7: Expected O, but got Unknown
		//IL_0714: Unknown result type (might be due to invalid IL or missing references)
		//IL_071b: Expected O, but got Unknown
		//IL_0737: Unknown result type (might be due to invalid IL or missing references)
		//IL_073e: Expected O, but got Unknown
		//IL_0363: Unknown result type (might be due to invalid IL or missing references)
		//IL_036a: Expected O, but got Unknown
		//IL_03a8: Unknown result type (might be due to invalid IL or missing references)
		//IL_03b2: Expected O, but got Unknown
		//IL_03ca: Unknown result type (might be due to invalid IL or missing references)
		//IL_03d4: Expected O, but got Unknown
		//IL_03fc: Unknown result type (might be due to invalid IL or missing references)
		//IL_0403: Expected O, but got Unknown
		//IL_05b7: Unknown result type (might be due to invalid IL or missing references)
		//IL_05be: Expected O, but got Unknown
		//IL_05c7: Unknown result type (might be due to invalid IL or missing references)
		//IL_05ce: Expected O, but got Unknown
		//IL_043b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0445: Expected O, but got Unknown
		//IL_0440: Unknown result type (might be due to invalid IL or missing references)
		//IL_0447: Expected O, but got Unknown
		//IL_0461: Unknown result type (might be due to invalid IL or missing references)
		//IL_046d: Expected O, but got Unknown
		//IL_0468: Unknown result type (might be due to invalid IL or missing references)
		//IL_046f: Expected O, but got Unknown
		//IL_04cf: Unknown result type (might be due to invalid IL or missing references)
		//IL_04d9: Expected O, but got Unknown
		//IL_04d4: Unknown result type (might be due to invalid IL or missing references)
		//IL_04db: Expected O, but got Unknown
		//IL_04f5: Unknown result type (might be due to invalid IL or missing references)
		//IL_0501: Expected O, but got Unknown
		//IL_04fc: Unknown result type (might be due to invalid IL or missing references)
		//IL_0503: Expected O, but got Unknown
		//IL_07c5: Unknown result type (might be due to invalid IL or missing references)
		//IL_079e: Unknown result type (might be due to invalid IL or missing references)
		//IL_07e3: Unknown result type (might be due to invalid IL or missing references)
		//IL_07eb: Unknown result type (might be due to invalid IL or missing references)
		//IL_07f0: Unknown result type (might be due to invalid IL or missing references)
		//IL_080b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0815: Unknown result type (might be due to invalid IL or missing references)
		//IL_081c: Expected O, but got Unknown
		//IL_086a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0871: Expected O, but got Unknown
		//IL_087b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0882: Expected O, but got Unknown
		//IL_088c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0893: Expected O, but got Unknown
		//IL_08d6: Unknown result type (might be due to invalid IL or missing references)
		//IL_08dd: Expected O, but got Unknown
		//IL_0935: Unknown result type (might be due to invalid IL or missing references)
		//IL_0949: Expected O, but got Unknown
		//IL_0958: Unknown result type (might be due to invalid IL or missing references)
		//IL_095f: Expected O, but got Unknown
		//IL_0aaf: Unknown result type (might be due to invalid IL or missing references)
		//IL_0ac5: Unknown result type (might be due to invalid IL or missing references)
		//IL_0985: Unknown result type (might be due to invalid IL or missing references)
		//IL_098c: Expected O, but got Unknown
		//IL_09d3: Unknown result type (might be due to invalid IL or missing references)
		//IL_09dd: Expected O, but got Unknown
		//IL_0a5e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0a65: Expected O, but got Unknown
		//IL_0a71: Unknown result type (might be due to invalid IL or missing references)
		//IL_0a87: Unknown result type (might be due to invalid IL or missing references)
		CSDataArray flattenArray = iCenterGem.GetFlattenArray();
		if (flattenArray.GetArrayCount() <= 0)
		{
			return UpdateMissing("Gem");
		}
		CSGemData cSGemData = iCenterGemShapeOverride.Data as CSGemData;
		OnNurbsCurve val = iProfile.GetNurbsCurve().Duplicate();
		double value = 0.0;
		iCenterGemSpacing.GetDouble(ref value);
		double value2 = 0.0;
		iProfileWidth.GetDouble(ref value2);
		double value3 = 0.0;
		iProfileHeight.GetDouble(ref value3);
		double value4 = 0.0;
		iChannelWallThicknessInner.GetDouble(ref value4);
		double value5 = 0.0;
		iChannelWallThicknessOuter.GetDouble(ref value5);
		double value6 = 0.0;
		iCenterGemSpacing.GetDouble(ref value6);
		double value7 = 0.0;
		iChannelWallDepth.GetDouble(ref value7);
		double value8 = 0.0;
		iAngle.GetDouble(ref value8);
		value8 = CSBlockUtil.CSUtilMath.ToRadians(value8);
		double value9 = 0.0;
		iPlacement.GetDouble(ref value9);
		double value10 = 0.0;
		iCornerBlend.GetDouble(ref value10);
		double value11 = 0.0;
		iGemSizeZ.GetDouble(ref value11);
		bool value12 = false;
		iCenterGemShapeOverrideEnabled.GetBoolean(ref value12);
		bool value13 = true;
		iShowChannel.GetBoolean(ref value13);
		bool value14 = false;
		iXYScaleToGem.GetBoolean(ref value14);
		OnBoundingBox val2 = new OnBoundingBox();
		((OnGeometry)val).GetTightBoundingBox(ref val2);
		OnXform val3 = new OnXform();
		val3.Translation(0.0 - val2.Min().x, 0.0 - val2.Min().y, 0.0);
		OnXform val4 = new OnXform();
		val4.Scale(1.0 / (val2.Max().x - val2.Min().x), 1.0 / (val2.Max().y - val2.Min().y), 0.0);
		OnXform val5 = new OnXform();
		val5.Scale(value2, 1.0, 1.0);
		val.Transform((IOnXform)(object)(val5 * (IOnXform)(object)val4 * (IOnXform)(object)val3));
		OnCurve val20;
		if (value13)
		{
			OnLineCurve c = new OnLineCurve((IOn3dPoint)new On3dPoint(value4, -1.0, 0.0), (IOn3dPoint)new On3dPoint(value4, 2.0, 0.0));
			OnLineCurve c2 = new OnLineCurve((IOn3dPoint)new On3dPoint(value2 - value5, -1.0, 0.0), (IOn3dPoint)new On3dPoint(value2 - value5, 2.0, 0.0));
			double param = 0.0;
			double param2 = 0.0;
			if (!CurveIntersectHighest((IOnCurve)(object)val, (IOnCurve)(object)c, ref param))
			{
				return UpdateError("Intersect for channel Inner Failed");
			}
			if (!CurveIntersectHighest((IOnCurve)(object)val, (IOnCurve)(object)c2, ref param2))
			{
				return UpdateError("Intersect for channel Failed");
			}
			On3dPoint val6 = ((OnCurve)val).PointAt(param);
			On3dPoint val7 = ((OnCurve)val).PointAt(param2);
			double num = ((val6.y > val7.y) ? val6.y : val7.y);
			OnXform val8 = new OnXform();
			val8.Scale(1.0, value3 / num, 1.0);
			val.Transform((IOnXform)(object)val8);
			OnCurve val9 = (OnCurve)(object)val.Duplicate();
			val9.Trim((IOnInterval)new OnInterval(((OnCurve)val).Domain().Min(), param));
			OnCurve val10 = (OnCurve)(object)val.Duplicate();
			val10.Trim((IOnInterval)new OnInterval(param2, ((OnCurve)val).Domain().Max()));
			double num2 = value4 + (value2 - value4 - value5) / 2.0;
			double num3 = value3 - value11;
			On3dPoint val11 = new On3dPoint(num2, num3, 0.0);
			OnCurve val18;
			OnCurve val19;
			IOnCurve[] array2;
			if (value7 > value10 * 2.0)
			{
				On3dPoint val12 = val9.PointAtEnd();
				OnCurve val13 = (OnCurve)new OnLineCurve((IOn3dPoint)(object)val12, (IOn3dPoint)new On3dPoint(val12.x, val12.y - value7, 0.0));
				OnCurve val14 = (OnCurve)new OnLineCurve((IOn3dPoint)new On3dPoint(val12.x, val12.y - value7, 0.0), (IOn3dPoint)(object)val11);
				CSUtilGeometry.TrimEndOfCrv(val13, value10);
				CSUtilGeometry.TrimStartOfCrv(val14, value10);
				OnCurve item = (OnCurve)(object)CSUtilCurves.BlendCurve((IOnCurve)(object)val13, (IOnCurve)(object)val14);
				On3dPoint val15 = val10.PointAtStart();
				OnCurve val16 = (OnCurve)new OnLineCurve((IOn3dPoint)(object)val11, (IOn3dPoint)new On3dPoint(val15.x, val15.y - value7, 0.0));
				OnCurve val17 = (OnCurve)new OnLineCurve((IOn3dPoint)new On3dPoint(val15.x, val15.y - value7, 0.0), (IOn3dPoint)(object)val15);
				CSUtilGeometry.TrimEndOfCrv(val16, value10);
				CSUtilGeometry.TrimStartOfCrv(val17, value10);
				OnCurve item2 = (OnCurve)(object)CSUtilCurves.BlendCurve((IOnCurve)(object)val16, (IOnCurve)(object)val17);
				List<OnCurve> obj = new List<OnCurve> { val13, item, val14 };
				OnCurve[] array = null;
				array2 = (IOnCurve[])(object)obj.ToArray();
				RhUtil.RhinoMergeCurves(array2, ref array);
				val18 = array[0];
				List<OnCurve> obj2 = new List<OnCurve> { val16, item2, val17 };
				array = null;
				array2 = (IOnCurve[])(object)obj2.ToArray();
				RhUtil.RhinoMergeCurves(array2, ref array);
				val19 = array[0];
			}
			else
			{
				val18 = (OnCurve)new OnLineCurve((IOn3dPoint)(object)val9.PointAtEnd(), (IOn3dPoint)(object)val11);
				val19 = (OnCurve)new OnLineCurve((IOn3dPoint)(object)val11, (IOn3dPoint)(object)val10.PointAtStart());
			}
			CSUtilGeometry.TrimEndOfCrv(val9, value10);
			CSUtilGeometry.TrimStartAndEndOfCrv(val18, value10);
			CSUtilGeometry.TrimStartAndEndOfCrv(val19, value10);
			CSUtilGeometry.TrimStartOfCrv(val10, value10);
			OnCurve item3 = (OnCurve)(object)CSUtilCurves.BlendCurve((IOnCurve)(object)val9, (IOnCurve)(object)val18);
			OnCurve item4 = (OnCurve)(object)CSUtilCurves.BlendCurve((IOnCurve)(object)val18, (IOnCurve)(object)val19);
			OnCurve item5 = (OnCurve)(object)CSUtilCurves.BlendCurve((IOnCurve)(object)val19, (IOnCurve)(object)val10);
			List<OnCurve> obj3 = new List<OnCurve> { val9, item3, val18, item4, val19, item5, val10 };
			OnCurve[] array3 = null;
			array2 = (IOnCurve[])(object)obj3.ToArray();
			if (!RhUtil.RhinoMergeCurves(array2, ref array3))
			{
				return UpdateError("channel Merge failed");
			}
			if (array3.Length != 1)
			{
				return UpdateError("channel Merge failed");
			}
			val20 = array3[0];
		}
		else
		{
			OnXform val21 = new OnXform();
			val21.Scale(1.0, value3, 1.0);
			val.Transform((IOnXform)(object)val21);
			val20 = (OnCurve)(object)val;
		}
		OnXform val22 = new OnXform();
		val22.Translation(0.0, 0.0 - value3, 0.0);
		OnXform val23 = new OnXform();
		val23.Rotation(value8, OnUtil.On_zaxis, OnUtil.On_origin);
		((OnGeometry)val20).Transform((IOnXform)(object)(val23 * (IOnXform)(object)val22));
		CSDataArray cSDataArray = new CSDataArray();
		for (int i = 0; i < flattenArray.GetArrayCount(); i++)
		{
			if (!(flattenArray.GetArrayItem(i).Duplicate() is CSGemData cSGemData2))
			{
				continue;
			}
			string gemShape;
			if (value12)
			{
				if (cSGemData == null)
				{
					cSGemData = new CSGemData(cSGemData2);
				}
				cSGemData.Xform = cSGemData2.Xform;
				gemShape = cSGemData.GemShape;
			}
			else
			{
				cSGemData = new CSGemData(cSGemData2);
				gemShape = cSGemData2.GemShape;
			}
			if (!CSBlockUtil.CSUtilMath.GetUnscaledXforms(cSGemData2.Xform, out var xUp, out var xDown))
			{
				return UpdateMissing("Gem X form");
			}
			cSGemData2.ApplyTransform(xDown);
			BoundingBox boundingBox = BoundingBox.Unset;
			if (!cSGemData2.GetTightBoundingBox(ref boundingBox))
			{
				return UpdateMissing("Gem Invalid Bounding Box");
			}
			On3dVector val24 = new On3dVector((IOn3dVector)(object)cSGemData2.Up.Convert());
			val24.Unitize();
			double num4 = value;
			double num5 = value9;
			if (!GemOffsetCurveV2.GemOffset(gemShape, cSGemData, num4, out OnCurve curve, false, out string error, 0.0, 0.0, 0.0))
			{
				return UpdateError("CurveTop Inside:" + error);
			}
			OnNurbsCurve val25 = new OnNurbsCurve();
			curve.GetNurbForm(ref val25);
			OnNurbsCurve val26 = new OnNurbsCurve();
			val20.GetNurbForm(ref val26);
			OnNurbsSurface val27 = new OnNurbsSurface();
			val27.Create(3, true, val26.m_order, val25.m_order, val26.m_cv_count, val25.m_cv_count);
			bool flag = false;
			List<OnCurve> list = new List<OnCurve>();
			CSGemData gem = (value14 ? cSGemData2 : cSGemData);
			for (int j = 0; j < val26.CVCount(); j++)
			{
				On3dPoint val28 = new On3dPoint();
				val26.GetCV(j, ref val28);
				if (!GemOffsetCurveV2.GemOffset(gemShape, gem, num4 + val28.x, out curve, false, out error, 0.0, 0.0, 0.0))
				{
					return UpdateError("CurveTop Inside:" + error);
				}
				((OnGeometry)curve).Translate((IOn3dVector)(object)(new On3dVector((IOn3dVector)(object)val24) * (num5 + val28.y)));
				list.Add(curve);
				OnNurbsCurve val29 = new OnNurbsCurve();
				curve.GetNurbForm(ref val29);
				if (val25.CVCount() != val29.CVCount())
				{
					flag = true;
				}
				if (!flag)
				{
					for (int k = 0; k < val25.CVCount(); k++)
					{
						On3dPoint val30 = new On3dPoint();
						val29.GetCV(k, ref val30);
						double num6 = val26.Weight(j) * val29.Weight(k);
						val27.SetCV(j, k, (IOn4dPoint)new On4dPoint(val30.x * num6, val30.y * num6, val30.z * num6, num6));
					}
				}
			}
			if (!flag)
			{
				for (int l = 0; l < val26.KnotCount(); l++)
				{
					val27.SetKnot(0, l, val26.Knot(l));
				}
				for (int m = 0; m < val25.KnotCount(); m++)
				{
					val27.SetKnot(1, m, val25.Knot(m));
				}
				OnBrep val31 = new OnBrep();
				((OnGeometry)val27).BrepForm(val31);
				((OnGeometry)val31).Transform((IOnXform)(object)xUp.Convert());
				cSDataArray.Append(new CSOnGeometryData((GeometryBase)(object)val31.Convert(), xUp, applyTransform: false));
			}
			else
			{
				IOnCurve[] array2 = (IOnCurve[])(object)list.ToArray();
				OnBrep val32 = CSUtilSweep.Loft(array2, (eLoftType)1);
				if (val32 != null)
				{
					((OnGeometry)val32).Transform((IOnXform)(object)xUp.Convert());
					cSDataArray.Append(new CSOnGeometryData((GeometryBase)(object)val32.Convert(), xUp, applyTransform: false));
				}
			}
		}
		oMetal.SetData(cSDataArray);
		return base.Update();
	}

	private static bool CurveIntersectHighest(IOnCurve c1, IOnCurve c2, ref double param)
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0006: Expected O, but got Unknown
		//IL_003d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0043: Expected O, but got Unknown
		ArrayOnX_EVENT val = new ArrayOnX_EVENT();
		c1.IntersectCurve(c2, ref val, CSApplicationHelper.Application.Tolerance);
		if (val.Count() == 0)
		{
			return false;
		}
		double num = -999.0;
		foreach (OnX_EVENT item in val)
		{
			OnX_EVENT val2 = item;
			if (val2[0].y > num)
			{
				num = val2[0].y;
				param = val2[0];
			}
		}
		return true;
	}
}
