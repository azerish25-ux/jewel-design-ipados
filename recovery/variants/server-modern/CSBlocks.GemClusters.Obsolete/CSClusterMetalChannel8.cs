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
using GV.CounterSketch.Shared;
using RMA.OpenNURBS;
using RMA.Rhino;
using Rhino.Geometry;
using Rhino.Runtime;

namespace CSBlocks.GemClusters.Obsolete;

[Obsolete]
[CSBlock("ClusterMetalChannel_V8", "{470BEBB7-3303-4DAE-8061-46236587EFB0}")]
public sealed class CSClusterMetalChannel8 : CSBlock
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

	public CSInputChannel iChannelSpacing { get; private set; }

	public CSInputChannel iChannelWallDepth { get; }

	public CSInputChannel iAngle { get; }

	public CSInputChannel iPlacement { get; }

	public CSInputChannel iCornerBlend { get; }

	public CSInputChannel iGemSizeZ { get; }

	public CSInputChannel iShowChannel { get; }

	public CSInputChannel iXYScaleToGem { get; }

	public CSInputChannel iCenterGemShapeOverrideMode { get; }

	public CSInputChannel iOffsetFix { get; }

	public CSInputChannel iInnerWallExtra { get; }

	public CSInputChannel iOuterWallExtra { get; }

	public CSInputChannel iRailX { get; }

	public CSInputChannel iRailZ { get; }

	public CSClusterMetalChannel8(string identifier)
		: base(identifier)
	{
		iCenterGem = new CSInputChannel(this, ChannelDataType.Gem, ChannelArrayType.Array | ChannelArrayType.Individual);
		iCenterGemShapeOverrideEnabled = new CSInputChannel(this, ChannelDataType.Boolean, ChannelArrayType.Individual);
		iCenterGemShapeOverride = new CSInputChannel(GetPropertyName(() => iCenterGemShapeOverride), this, ChannelDataType.Gem, ChannelArrayType.Individual);
		iCenterGemSpacing = new CSInputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual);
		iProfile = new CSInputChannel(GetPropertyName(() => iProfile), this, ChannelDataType.Curve, ChannelArrayType.Individual);
		iProfileWidth = new CSInputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual);
		iProfileHeight = new CSInputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual);
		iChannelWallThicknessInner = new CSInputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual);
		iChannelWallThicknessOuter = new CSInputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual);
		iChannelSpacing = new CSInputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual);
		iChannelWallDepth = new CSInputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual);
		iAngle = new CSInputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual);
		iPlacement = new CSInputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual);
		iCornerBlend = new CSInputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual);
		iGemSizeZ = new CSInputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual);
		iShowChannel = new CSInputChannel(this, ChannelDataType.Boolean, ChannelArrayType.Individual);
		iXYScaleToGem = new CSInputChannel(this, ChannelDataType.Boolean, ChannelArrayType.Individual);
		iCenterGemShapeOverrideMode = new CSInputChannel(this, ChannelDataType.Integer, ChannelArrayType.Individual);
		iOffsetFix = new CSInputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual);
		iInnerWallExtra = new CSInputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual);
		iOuterWallExtra = new CSInputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual);
		iRailX = new CSInputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual);
		iRailZ = new CSInputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual);
		oMetal = new CSOutputChannel(GetPropertyName(() => oMetal), this, ChannelDataType.Any, ChannelArrayType.Array);
		SetChannelIdentifiers();
	}

	public override bool Update()
	{
		//IL_02ce: Unknown result type (might be due to invalid IL or missing references)
		CSDataArray flattenArray = iCenterGem.GetFlattenArray();
		if (flattenArray.GetArrayCount() <= 0)
		{
			return UpdateMissing("CenterGem");
		}
		if (!(flattenArray.GetArrayItem(0).Duplicate() is CSGemData centerGem))
		{
			return UpdateMissing("CenterGem");
		}
		CSGemData centerGemShapeOverride = iCenterGemShapeOverride.Data as CSGemData;
		OnNurbsCurve profile = iProfile.GetNurbsCurve().Duplicate();
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
		iChannelWallDepth.GetDouble(ref value6);
		double value7 = 0.0;
		iAngle.GetDouble(ref value7);
		value7 = CSBlockUtil.CSUtilMath.ToRadians(value7);
		double value8 = 0.0;
		iPlacement.GetDouble(ref value8);
		double value9 = 0.0;
		iCornerBlend.GetDouble(ref value9);
		double value10 = 0.0;
		iGemSizeZ.GetDouble(ref value10);
		double value11 = 0.0;
		iOffsetFix.GetDouble(ref value11);
		value += value11;
		bool value12 = false;
		iCenterGemShapeOverrideEnabled.GetBoolean(ref value12);
		bool value13 = true;
		iShowChannel.GetBoolean(ref value13);
		bool value14 = false;
		iXYScaleToGem.GetBoolean(ref value14);
		double value15 = 0.0;
		iInnerWallExtra.GetDouble(ref value15);
		double value16 = 0.0;
		iOuterWallExtra.GetDouble(ref value16);
		ValueListItem whichItem = ValueListItem.GemClusterCenterShapeOverrideModeDimensionSmallest;
		iCenterGemShapeOverrideMode.GetValueListItem(ref whichItem);
		CSDataArray flattenArray2 = iRailX.GetFlattenArray();
		double value17 = 0.0;
		if (flattenArray2.GetArrayCount() >= 1)
		{
			flattenArray2.GetArrayItem(0).GetDouble(ref value17);
		}
		value += value17;
		CSDataArray flattenArray3 = iRailZ.GetFlattenArray();
		double value18 = 0.0;
		if (flattenArray3.GetArrayCount() >= 1)
		{
			flattenArray3.GetArrayItem(0).GetDouble(ref value18);
		}
		value8 += value18;
		if (!MakeCluster(centerGem, value12, centerGemShapeOverride, value, profile, value2, value3, value4, value5, value6, value7, value8, value9, value10, value13, value14, whichItem, value15, value16, out var outputMetals, out var outputTransforms, out var outError))
		{
			return UpdateError(outError);
		}
		CSDataArray cSDataArray = new CSDataArray();
		for (int i = 0; i < outputMetals.Count; i++)
		{
			cSDataArray.Append(new CSOnGeometryData((GeometryBase)(object)outputMetals[i].Convert(), outputTransforms[i].Convert(), applyTransform: false));
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

	private static bool MakeCluster(CSGemData centerGem, bool centerGemShapeOverrideEnabled, CSGemData centerGemShapeOverride, double centerGemSpacing, OnNurbsCurve profile, double profileWidth, double profileHeight, double channelWallThicknessInner, double channelWallThicknessOuter, double channelWallDepth, double angle, double placement, double cornerBlend, double gemSizeZ, bool showChannel, bool doScaleToGemXY, ValueListItem centerGemShapeOverrideMode, double innerWallExtra, double outerWallExtra, out List<OnBrep> outputMetals, out List<OnXform> outputTransforms, out string outError)
	{
		//IL_0010: Unknown result type (might be due to invalid IL or missing references)
		//IL_0016: Expected O, but got Unknown
		//IL_0020: Unknown result type (might be due to invalid IL or missing references)
		//IL_0026: Expected O, but got Unknown
		//IL_004d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0053: Expected O, but got Unknown
		//IL_00a4: Unknown result type (might be due to invalid IL or missing references)
		//IL_00aa: Expected O, but got Unknown
		//IL_00ca: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d1: Expected O, but got Unknown
		//IL_0588: Unknown result type (might be due to invalid IL or missing references)
		//IL_058f: Expected O, but got Unknown
		//IL_0124: Unknown result type (might be due to invalid IL or missing references)
		//IL_013d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0147: Expected O, but got Unknown
		//IL_0147: Expected O, but got Unknown
		//IL_0142: Unknown result type (might be due to invalid IL or missing references)
		//IL_0149: Expected O, but got Unknown
		//IL_0160: Unknown result type (might be due to invalid IL or missing references)
		//IL_017c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0186: Expected O, but got Unknown
		//IL_0186: Expected O, but got Unknown
		//IL_0181: Unknown result type (might be due to invalid IL or missing references)
		//IL_0188: Expected O, but got Unknown
		//IL_05b8: Unknown result type (might be due to invalid IL or missing references)
		//IL_05bf: Expected O, but got Unknown
		//IL_05db: Unknown result type (might be due to invalid IL or missing references)
		//IL_05e2: Expected O, but got Unknown
		//IL_06ea: Unknown result type (might be due to invalid IL or missing references)
		//IL_070e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0716: Unknown result type (might be due to invalid IL or missing references)
		//IL_071b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0204: Unknown result type (might be due to invalid IL or missing references)
		//IL_020b: Expected O, but got Unknown
		//IL_024c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0256: Expected O, but got Unknown
		//IL_0270: Unknown result type (might be due to invalid IL or missing references)
		//IL_027a: Expected O, but got Unknown
		//IL_02a2: Unknown result type (might be due to invalid IL or missing references)
		//IL_02a9: Expected O, but got Unknown
		//IL_0734: Unknown result type (might be due to invalid IL or missing references)
		//IL_073e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0745: Expected O, but got Unknown
		//IL_045d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0464: Expected O, but got Unknown
		//IL_046d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0474: Expected O, but got Unknown
		//IL_02e1: Unknown result type (might be due to invalid IL or missing references)
		//IL_02eb: Expected O, but got Unknown
		//IL_02e6: Unknown result type (might be due to invalid IL or missing references)
		//IL_02ed: Expected O, but got Unknown
		//IL_0307: Unknown result type (might be due to invalid IL or missing references)
		//IL_0313: Expected O, but got Unknown
		//IL_030e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0315: Expected O, but got Unknown
		//IL_0375: Unknown result type (might be due to invalid IL or missing references)
		//IL_037f: Expected O, but got Unknown
		//IL_037a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0381: Expected O, but got Unknown
		//IL_039b: Unknown result type (might be due to invalid IL or missing references)
		//IL_03a7: Expected O, but got Unknown
		//IL_03a2: Unknown result type (might be due to invalid IL or missing references)
		//IL_03a9: Expected O, but got Unknown
		//IL_0791: Unknown result type (might be due to invalid IL or missing references)
		//IL_0798: Expected O, but got Unknown
		//IL_07a2: Unknown result type (might be due to invalid IL or missing references)
		//IL_07a9: Expected O, but got Unknown
		//IL_07b3: Unknown result type (might be due to invalid IL or missing references)
		//IL_07ba: Expected O, but got Unknown
		//IL_07fd: Unknown result type (might be due to invalid IL or missing references)
		//IL_0804: Expected O, but got Unknown
		//IL_085a: Unknown result type (might be due to invalid IL or missing references)
		//IL_086e: Expected O, but got Unknown
		//IL_087d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0884: Expected O, but got Unknown
		//IL_0a18: Unknown result type (might be due to invalid IL or missing references)
		//IL_0a3a: Unknown result type (might be due to invalid IL or missing references)
		//IL_08aa: Unknown result type (might be due to invalid IL or missing references)
		//IL_08b1: Expected O, but got Unknown
		//IL_08f8: Unknown result type (might be due to invalid IL or missing references)
		//IL_0902: Expected O, but got Unknown
		//IL_0983: Unknown result type (might be due to invalid IL or missing references)
		//IL_098a: Expected O, but got Unknown
		//IL_0996: Unknown result type (might be due to invalid IL or missing references)
		//IL_09f5: Unknown result type (might be due to invalid IL or missing references)
		outputMetals = new List<OnBrep>();
		outputTransforms = new List<OnXform>();
		OnBoundingBox val = new OnBoundingBox();
		((OnGeometry)profile).GetTightBoundingBox(ref val);
		OnXform val2 = new OnXform();
		val2.Translation(0.0 - val.Min().x, 0.0 - val.Min().y, 0.0);
		OnXform val3 = new OnXform();
		val3.Scale(1.0 / (val.Max().x - val.Min().x), 1.0 / (val.Max().y - val.Min().y), 0.0);
		OnXform val4 = new OnXform();
		val4.Scale(profileWidth + innerWallExtra + outerWallExtra, 1.0, 1.0);
		OnXform val5 = new OnXform();
		val5.Translation(0.0 - innerWallExtra, 0.0, 0.0);
		profile.Transform((IOnXform)(object)(val5 * (IOnXform)(object)val4 * (IOnXform)(object)val3 * (IOnXform)(object)val2));
		OnCurve val20;
		if (showChannel)
		{
			OnLineCurve c = new OnLineCurve((IOn3dPoint)new On3dPoint(channelWallThicknessInner, -1.0, 0.0), (IOn3dPoint)new On3dPoint(channelWallThicknessInner, 2.0, 0.0));
			OnLineCurve c2 = new OnLineCurve((IOn3dPoint)new On3dPoint(profileWidth - channelWallThicknessOuter, -1.0, 0.0), (IOn3dPoint)new On3dPoint(profileWidth - channelWallThicknessOuter, 2.0, 0.0));
			double param = 0.0;
			double param2 = 0.0;
			if (!CurveIntersectHighest((IOnCurve)(object)profile, (IOnCurve)(object)c, ref param))
			{
				outError = "Intersect for channel Inner Failed";
				return false;
			}
			if (!CurveIntersectHighest((IOnCurve)(object)profile, (IOnCurve)(object)c2, ref param2))
			{
				outError = "Intersect for channel Failed";
				return false;
			}
			On3dPoint val6 = ((OnCurve)profile).PointAt(param);
			On3dPoint val7 = ((OnCurve)profile).PointAt(param2);
			double num = ((val6.y > val7.y) ? val6.y : val7.y);
			OnXform val8 = new OnXform();
			val8.Scale(1.0, profileHeight / num, 1.0);
			profile.Transform((IOnXform)(object)val8);
			OnCurve val9 = (OnCurve)(object)profile.Duplicate();
			val9.Trim((IOnInterval)new OnInterval(((OnCurve)profile).Domain().Min(), param));
			OnCurve val10 = (OnCurve)(object)profile.Duplicate();
			val10.Trim((IOnInterval)new OnInterval(param2, ((OnCurve)profile).Domain().Max()));
			double num2 = channelWallThicknessInner + (profileWidth - channelWallThicknessInner - channelWallThicknessOuter) / 2.0;
			double num3 = profileHeight - gemSizeZ;
			On3dPoint val11 = new On3dPoint(num2, num3, 0.0);
			OnCurve val18;
			OnCurve val19;
			IOnCurve[] array2;
			if (channelWallDepth > cornerBlend * 2.0)
			{
				On3dPoint val12 = val9.PointAtEnd();
				OnCurve val13 = (OnCurve)new OnLineCurve((IOn3dPoint)(object)val12, (IOn3dPoint)new On3dPoint(val12.x, val12.y - channelWallDepth, 0.0));
				OnCurve val14 = (OnCurve)new OnLineCurve((IOn3dPoint)new On3dPoint(val12.x, val12.y - channelWallDepth, 0.0), (IOn3dPoint)(object)val11);
				CSUtilGeometry.TrimEndOfCrv(val13, cornerBlend);
				CSUtilGeometry.TrimStartOfCrv(val14, cornerBlend);
				OnCurve item = (OnCurve)(object)CSUtilCurves.BlendCurve((IOnCurve)(object)val13, (IOnCurve)(object)val14);
				On3dPoint val15 = val10.PointAtStart();
				OnCurve val16 = (OnCurve)new OnLineCurve((IOn3dPoint)(object)val11, (IOn3dPoint)new On3dPoint(val15.x, val15.y - channelWallDepth, 0.0));
				OnCurve val17 = (OnCurve)new OnLineCurve((IOn3dPoint)new On3dPoint(val15.x, val15.y - channelWallDepth, 0.0), (IOn3dPoint)(object)val15);
				CSUtilGeometry.TrimEndOfCrv(val16, cornerBlend);
				CSUtilGeometry.TrimStartOfCrv(val17, cornerBlend);
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
			CSUtilGeometry.TrimEndOfCrv(val9, cornerBlend);
			CSUtilGeometry.TrimStartAndEndOfCrv(val18, cornerBlend);
			CSUtilGeometry.TrimStartAndEndOfCrv(val19, cornerBlend);
			CSUtilGeometry.TrimStartOfCrv(val10, cornerBlend);
			OnCurve item3 = (OnCurve)(object)CSUtilCurves.BlendCurve((IOnCurve)(object)val9, (IOnCurve)(object)val18);
			OnCurve item4 = (OnCurve)(object)CSUtilCurves.BlendCurve((IOnCurve)(object)val18, (IOnCurve)(object)val19);
			OnCurve item5 = (OnCurve)(object)CSUtilCurves.BlendCurve((IOnCurve)(object)val19, (IOnCurve)(object)val10);
			List<OnCurve> obj3 = new List<OnCurve> { val9, item3, val18, item4, val19, item5, val10 };
			OnCurve[] array3 = null;
			array2 = (IOnCurve[])(object)obj3.ToArray();
			if (!RhUtil.RhinoMergeCurves(array2, ref array3))
			{
				outError = "channel Merge failed";
				return false;
			}
			if (array3.Length != 1)
			{
				outError = "channel Merge failed";
				return false;
			}
			val20 = array3[0];
		}
		else
		{
			OnXform val21 = new OnXform();
			val21.Scale(1.0, profileHeight, 1.0);
			profile.Transform((IOnXform)(object)val21);
			val20 = (OnCurve)(object)profile;
		}
		OnXform val22 = new OnXform();
		val22.Translation(0.0, 0.0 - profileHeight, 0.0);
		OnXform val23 = new OnXform();
		val23.Rotation(angle, OnUtil.On_zaxis, OnUtil.On_origin);
		((OnGeometry)val20).Transform((IOnXform)(object)(val23 * (IOnXform)(object)val22));
		string gemShape;
		if (centerGemShapeOverrideEnabled)
		{
			if (centerGemShapeOverride == null)
			{
				centerGemShapeOverride = new CSGemData(centerGem);
			}
			else
			{
				double scaleFactorX;
				switch (centerGemShapeOverrideMode)
				{
				case ValueListItem.GemClusterCenterShapeOverrideModeDimensionX:
					scaleFactorX = centerGem.GemXSize / centerGemShapeOverride.GemXSize;
					break;
				case ValueListItem.GemClusterCenterShapeOverrideModeDimensionY:
					scaleFactorX = centerGem.GemYSize / centerGemShapeOverride.GemYSize;
					break;
				case ValueListItem.GemClusterCenterShapeOverrideModeDimensionLargest:
					scaleFactorX = ((!(centerGem.GemXSize > centerGem.GemYSize)) ? (centerGem.GemYSize / centerGemShapeOverride.GemYSize) : (centerGem.GemXSize / centerGemShapeOverride.GemXSize));
					break;
				case ValueListItem.GemClusterCenterShapeOverrideModeDimensionSmallest:
					scaleFactorX = ((!(centerGem.GemXSize < centerGem.GemYSize)) ? (centerGem.GemYSize / centerGemShapeOverride.GemYSize) : (centerGem.GemXSize / centerGemShapeOverride.GemXSize));
					break;
				default:
					outError = "Invalid Gem Cluster Center Shape Override Mode";
					return false;
				}
				centerGemShapeOverride.Scale(scaleFactorX);
			}
			gemShape = centerGemShapeOverride.GemShape;
		}
		else
		{
			centerGemShapeOverride = new CSGemData(centerGem);
			gemShape = centerGem.GemShape;
		}
		if (!CSBlockUtil.CSUtilMath.GetUnscaledXforms(centerGem.Xform, out var xUp, out var xDown))
		{
			outError = "Gem X form";
			return false;
		}
		CSGemData cSGemData = new CSGemData(centerGem);
		cSGemData.ApplyTransform(xDown);
		BoundingBox boundingBox = BoundingBox.Unset;
		if (!cSGemData.GetTightBoundingBox(ref boundingBox))
		{
			outError = "Gem Invalid Bounding Box";
			return false;
		}
		On3dVector val24 = new On3dVector((IOn3dVector)(object)cSGemData.Up.Convert());
		val24.Unitize();
		if (!GemOffsetCurveV2.GemOffset(gemShape, centerGemShapeOverride, centerGemSpacing, out OnCurve curve, false, out string error, 0.0, 0.0, 0.0))
		{
			outError = "CurveTop Inside:" + error;
			return false;
		}
		OnNurbsCurve val25 = new OnNurbsCurve();
		curve.GetNurbForm(ref val25);
		OnNurbsCurve val26 = new OnNurbsCurve();
		val20.GetNurbForm(ref val26);
		OnNurbsSurface val27 = new OnNurbsSurface();
		val27.Create(3, true, val26.m_order, val25.m_order, val26.m_cv_count, val25.m_cv_count);
		bool flag = false;
		List<IOnCurve> list = new List<IOnCurve>();
		CSGemData gem = (doScaleToGemXY ? cSGemData : centerGemShapeOverride);
		for (int i = 0; i < val26.CVCount(); i++)
		{
			On3dPoint val28 = new On3dPoint();
			val26.GetCV(i, ref val28);
			if (!GemOffsetCurveV2.GemOffset(gemShape, gem, centerGemSpacing + val28.x, out curve, false, out error, 0.0, 0.0, 0.0))
			{
				outError = "CurveTop Inside:" + error;
				return false;
			}
			((OnGeometry)curve).Translate((IOn3dVector)(object)(new On3dVector((IOn3dVector)(object)val24) * (placement + val28.y)));
			list.Add((IOnCurve)(object)curve);
			OnNurbsCurve val29 = new OnNurbsCurve();
			curve.GetNurbForm(ref val29);
			if (val25.CVCount() != val29.CVCount())
			{
				flag = true;
			}
			if (!flag)
			{
				for (int j = 0; j < val25.CVCount(); j++)
				{
					On3dPoint val30 = new On3dPoint();
					val29.GetCV(j, ref val30);
					double num4 = val26.Weight(i) * val29.Weight(j);
					val27.SetCV(i, j, (IOn4dPoint)new On4dPoint(val30.x * num4, val30.y * num4, val30.z * num4, num4));
				}
			}
		}
		if (!flag)
		{
			for (int k = 0; k < val26.KnotCount(); k++)
			{
				val27.SetKnot(0, k, val26.Knot(k));
			}
			for (int l = 0; l < val25.KnotCount(); l++)
			{
				val27.SetKnot(1, l, val25.Knot(l));
			}
			OnBrep brep = new OnBrep();
			((OnGeometry)val27).BrepForm(brep);
			((OnGeometry)brep).Transform((IOnXform)(object)xUp.Convert());
			CSUtilGeometry.FixBrepSolidOrientation(ref brep);
			if (!brep.IsSolid())
			{
				Brep val31 = Interop.FromOnBrep((object)brep);
				val31.JoinNakedEdges(CSApplicationHelper.Application.Tolerance);
				if (!val31.IsSolid)
				{
					val31.Flip();
				}
				brep = val31.Convert();
			}
			outputMetals.Add(brep);
			outputTransforms.Add(xUp.Convert());
		}
		else
		{
			OnBrep brep2 = CSUtilSweep.Loft(list.ToArray(), (eLoftType)1);
			if (brep2 != null)
			{
				((OnGeometry)brep2).Transform((IOnXform)(object)xUp.Convert());
				CSUtilGeometry.FixBrepSolidOrientation(ref brep2);
				outputMetals.Add(brep2);
				outputTransforms.Add(xUp.Convert());
			}
		}
		outError = string.Empty;
		return true;
	}
}
