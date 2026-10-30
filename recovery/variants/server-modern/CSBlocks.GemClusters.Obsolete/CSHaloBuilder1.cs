using System;
using System.Collections.Generic;
using CS;
using CS.CSDataUtilities;
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
[CSBlock("HaloBuilder_V1", "{88465C7C-6E4C-4AFE-98D2-98596E90DA70}")]
public sealed class CSHaloBuilder1 : CSBlock
{
	public CSInputChannel InCenterGem { get; }

	public CSInputChannel InCenterGemShapeOverrideEnabled { get; }

	public CSInputChannel InCenterGemShapeOverride { get; }

	public CSInputChannel InCenterGemSpacing { get; }

	public CSInputChannel InProfile { get; }

	public CSInputChannel InProfile2 { get; }

	public CSInputChannel InProfileWidth { get; }

	public CSInputChannel InProfileHeight { get; }

	public CSInputChannel InChannelWallThicknessInner { get; }

	public CSInputChannel InChannelWallThicknessOuter { get; }

	public CSInputChannel InChannelSpacing { get; }

	public CSInputChannel InChannelWallDepth { get; }

	public CSInputChannel InAngle { get; }

	public CSInputChannel InPlacement { get; }

	public CSInputChannel InCornerBlend { get; }

	public CSInputChannel InGemSizeZ { get; }

	public CSInputChannel InShowChannel { get; }

	public CSInputChannel InXYScaleToGem { get; }

	public CSInputChannel InCenterGemShapeOverrideMode { get; }

	public CSInputChannel InOffsetFix { get; }

	public CSInputChannel InInnerWallExtra { get; }

	public CSInputChannel InOuterWallExtra { get; }

	public CSInputChannel InRailX { get; }

	public CSInputChannel InRailZ { get; }

	public CSInputChannel InChannelBottomOffset { get; }

	public CSInputChannel InMetalToggle { get; }

	public CSInputChannel InBlendCornerGemShape { get; }

	public CSInputChannel InOffsetX { get; }

	public CSInputChannel InOffsetY { get; }

	public CSInputChannel InEmeraldCornerPoint1 { get; }

	public CSInputChannel InEmeraldCornerPoint2 { get; }

	public CSOutputChannel OutMetal { get; }

	public CSOutputChannel OutEmeraldCorner1Point { get; }

	public CSOutputChannel OutEmeraldCorner2Point { get; }

	public CSHaloBuilder1(string identifier)
		: base(identifier)
	{
		InCenterGem = new CSInputChannel(this, ChannelDataType.Gem, ChannelArrayType.Array | ChannelArrayType.Individual);
		InCenterGemShapeOverrideEnabled = new CSInputChannel(this, ChannelDataType.Boolean, ChannelArrayType.Individual);
		InCenterGemShapeOverride = new CSInputChannel(this, ChannelDataType.Gem, ChannelArrayType.Individual);
		InCenterGemSpacing = new CSInputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual);
		InProfile = new CSInputChannel(this, ChannelDataType.Curve, ChannelArrayType.Individual);
		InProfile2 = new CSInputChannel(this, ChannelDataType.Curve, ChannelArrayType.Individual);
		InProfileWidth = new CSInputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual);
		InProfileHeight = new CSInputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual);
		InChannelWallThicknessInner = new CSInputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual);
		InChannelWallThicknessOuter = new CSInputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual);
		InChannelSpacing = new CSInputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual);
		InChannelWallDepth = new CSInputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual);
		InAngle = new CSInputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual);
		InPlacement = new CSInputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual);
		InCornerBlend = new CSInputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual);
		InGemSizeZ = new CSInputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual);
		InShowChannel = new CSInputChannel(this, ChannelDataType.Boolean, ChannelArrayType.Individual);
		InXYScaleToGem = new CSInputChannel(this, ChannelDataType.Boolean, ChannelArrayType.Individual);
		InCenterGemShapeOverrideMode = new CSInputChannel(this, ChannelDataType.Integer, ChannelArrayType.Individual);
		InOffsetFix = new CSInputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual);
		InInnerWallExtra = new CSInputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual);
		InOuterWallExtra = new CSInputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual);
		InRailX = new CSInputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual);
		InRailZ = new CSInputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual);
		InChannelBottomOffset = new CSInputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual, 0.0);
		InBlendCornerGemShape = new CSInputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual, 0.0);
		InMetalToggle = new CSInputChannel(this, ChannelDataType.Boolean, ChannelArrayType.Individual, defaultValue: true);
		InOffsetX = new CSInputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual, 0.0);
		InOffsetY = new CSInputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual, 0.0);
		InEmeraldCornerPoint1 = new CSInputChannel(this, ChannelDataType.Point, ChannelArrayType.Individual);
		InEmeraldCornerPoint2 = new CSInputChannel(this, ChannelDataType.Point, ChannelArrayType.Individual);
		OutMetal = new CSOutputChannel(this, ChannelDataType.Any, ChannelArrayType.Array);
		OutEmeraldCorner1Point = new CSOutputChannel(this, ChannelDataType.Point, ChannelArrayType.Individual);
		OutEmeraldCorner2Point = new CSOutputChannel(this, ChannelDataType.Point, ChannelArrayType.Individual);
		SetChannelIdentifiers();
	}

	public override bool Update()
	{
		//IL_02f9: Unknown result type (might be due to invalid IL or missing references)
		//IL_02fe: Unknown result type (might be due to invalid IL or missing references)
		//IL_0300: Unknown result type (might be due to invalid IL or missing references)
		//IL_0305: Unknown result type (might be due to invalid IL or missing references)
		//IL_0375: Unknown result type (might be due to invalid IL or missing references)
		//IL_0377: Unknown result type (might be due to invalid IL or missing references)
		//IL_0399: Unknown result type (might be due to invalid IL or missing references)
		//IL_03ab: Unknown result type (might be due to invalid IL or missing references)
		//IL_03dc: Unknown result type (might be due to invalid IL or missing references)
		InMetalToggle.GetValue(out bool value);
		if (!value)
		{
			OutMetal.SetData(null);
			return base.Update();
		}
		CSDataArray flattenArray = InCenterGem.GetFlattenArray();
		if (flattenArray.GetArrayCount() <= 0)
		{
			return UpdateMissing("CenterGem");
		}
		CSGemData cSGemData = flattenArray.GetArrayItem(0).Duplicate() as CSGemData;
		CSData centerObject = flattenArray.GetArrayItem(0).Duplicate();
		bool value2 = false;
		InCenterGemShapeOverrideEnabled.GetBoolean(ref value2);
		if (cSGemData == null)
		{
			value2 = true;
		}
		CSGemData centerGemShapeOverride = InCenterGemShapeOverride.Data as CSGemData;
		double value3 = 0.0;
		InCenterGemSpacing.GetDouble(ref value3);
		double value4 = 0.0;
		InProfileWidth.GetDouble(ref value4);
		double value5 = 0.0;
		InProfileHeight.GetDouble(ref value5);
		double value6 = 0.0;
		InChannelWallThicknessInner.GetDouble(ref value6);
		double value7 = 0.0;
		InChannelWallThicknessOuter.GetDouble(ref value7);
		double value8 = 0.0;
		InChannelWallDepth.GetDouble(ref value8);
		double value9 = 0.0;
		InAngle.GetDouble(ref value9);
		value9 *= OnUtil.On_DEGREES_TO_RADIANS;
		double value10 = 0.0;
		InPlacement.GetDouble(ref value10);
		double value11 = 0.0;
		InCornerBlend.GetDouble(ref value11);
		double value12 = 0.0;
		InGemSizeZ.GetDouble(ref value12);
		if (!InChannelBottomOffset.GetValue(out double value13))
		{
			return InputError();
		}
		if (!InBlendCornerGemShape.GetValue(out double value14))
		{
			return InputError();
		}
		double value15 = 0.0;
		InOffsetFix.GetDouble(ref value15);
		value3 += value15;
		bool value16 = true;
		InShowChannel.GetBoolean(ref value16);
		bool value17 = false;
		InXYScaleToGem.GetBoolean(ref value17);
		double value18 = 0.0;
		InInnerWallExtra.GetDouble(ref value18);
		double value19 = 0.0;
		InOuterWallExtra.GetDouble(ref value19);
		OnNurbsCurve profile = (value16 ? InProfile2.GetNurbsCurve().Duplicate() : InProfile.GetNurbsCurve().Duplicate());
		ValueListItem whichItem = ValueListItem.GemClusterCenterShapeOverrideModeDimensionSmallest;
		InCenterGemShapeOverrideMode.GetValueListItem(ref whichItem);
		CSDataArray flattenArray2 = InRailX.GetFlattenArray();
		double value20 = 0.0;
		if (flattenArray2.GetArrayCount() >= 1)
		{
			flattenArray2.GetArrayItem(0).GetDouble(ref value20);
		}
		value3 += value20;
		CSDataArray flattenArray3 = InRailZ.GetFlattenArray();
		double value21 = 0.0;
		if (flattenArray3.GetArrayCount() >= 1)
		{
			flattenArray3.GetArrayItem(0).GetDouble(ref value21);
		}
		value10 += value21;
		InOffsetX.GetValue(out double value22);
		InOffsetY.GetValue(out double value23);
		Point3d value24 = Point3d.Unset;
		Point3d value25 = Point3d.Unset;
		if (InEmeraldCornerPoint1.Data != null && InEmeraldCornerPoint2.Data != null)
		{
			CSDataHelper.GetPoint(InEmeraldCornerPoint1.Data, out value24);
			CSDataHelper.GetPoint(InEmeraldCornerPoint2.Data, out value25);
		}
		if (!MakeCluster(cSGemData, centerObject, value2, centerGemShapeOverride, value3, profile, value4, value5, value6, value7, value8, value9, value10, value11, value12, value16, value17, whichItem, value18, value19, value13, value14, value22, value23, value24, value25, out var outputMetals, out var outputTransforms, out var emeraldCorner1Point, out var emeraldCorner2Point, out var outError))
		{
			return UpdateError(outError);
		}
		OutEmeraldCorner1Point.SetData(new CSPointData(emeraldCorner1Point));
		OutEmeraldCorner2Point.SetData(new CSPointData(emeraldCorner2Point));
		CSDataArray cSDataArray = new CSDataArray();
		for (int i = 0; i < outputMetals.Count; i++)
		{
			cSDataArray.Append(new CSOnGeometryData((GeometryBase)(object)outputMetals[i].Convert(), outputTransforms[i].Convert(), applyTransform: false));
		}
		OutMetal.SetData(cSDataArray);
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

	private static bool MakeCluster(CSGemData centerGem, CSData centerObject, bool centerGemShapeOverrideEnabled, CSGemData centerGemShapeOverride, double centerGemSpacing, OnNurbsCurve profile, double profileWidth, double profileHeight, double channelWallThicknessInner, double channelWallThicknessOuter, double channelWallDepth, double angle, double placement, double cornerBlend, double gemSizeZ, bool showChannel, bool doScaleToGemXY, ValueListItem centerGemShapeOverrideMode, double innerWallExtra, double outerWallExtra, double channelBottomOffset, double blendCornerGemShape, double offsetX, double offsetY, Point3d inputCorner1, Point3d inputCorner2, out List<OnBrep> outputMetals, out List<OnXform> outputTransforms, out Point3d emeraldCorner1Point, out Point3d emeraldCorner2Point, out string outError)
	{
		//IL_0012: Unknown result type (might be due to invalid IL or missing references)
		//IL_0017: Unknown result type (might be due to invalid IL or missing references)
		//IL_001e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0023: Unknown result type (might be due to invalid IL or missing references)
		//IL_0028: Unknown result type (might be due to invalid IL or missing references)
		//IL_002e: Expected O, but got Unknown
		//IL_0038: Unknown result type (might be due to invalid IL or missing references)
		//IL_003e: Expected O, but got Unknown
		//IL_0065: Unknown result type (might be due to invalid IL or missing references)
		//IL_006b: Expected O, but got Unknown
		//IL_00bc: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c2: Expected O, but got Unknown
		//IL_00e2: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e9: Expected O, but got Unknown
		//IL_05a8: Unknown result type (might be due to invalid IL or missing references)
		//IL_05af: Expected O, but got Unknown
		//IL_013c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0155: Unknown result type (might be due to invalid IL or missing references)
		//IL_015f: Expected O, but got Unknown
		//IL_015f: Expected O, but got Unknown
		//IL_015a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0161: Expected O, but got Unknown
		//IL_0178: Unknown result type (might be due to invalid IL or missing references)
		//IL_0194: Unknown result type (might be due to invalid IL or missing references)
		//IL_019e: Expected O, but got Unknown
		//IL_019e: Expected O, but got Unknown
		//IL_0199: Unknown result type (might be due to invalid IL or missing references)
		//IL_01a0: Expected O, but got Unknown
		//IL_05d8: Unknown result type (might be due to invalid IL or missing references)
		//IL_05df: Expected O, but got Unknown
		//IL_05fb: Unknown result type (might be due to invalid IL or missing references)
		//IL_0602: Expected O, but got Unknown
		//IL_0626: Unknown result type (might be due to invalid IL or missing references)
		//IL_062b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0638: Unknown result type (might be due to invalid IL or missing references)
		//IL_063d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0648: Unknown result type (might be due to invalid IL or missing references)
		//IL_064d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0663: Unknown result type (might be due to invalid IL or missing references)
		//IL_0668: Unknown result type (might be due to invalid IL or missing references)
		//IL_0673: Unknown result type (might be due to invalid IL or missing references)
		//IL_0678: Unknown result type (might be due to invalid IL or missing references)
		//IL_08c7: Unknown result type (might be due to invalid IL or missing references)
		//IL_08cc: Unknown result type (might be due to invalid IL or missing references)
		//IL_08d7: Unknown result type (might be due to invalid IL or missing references)
		//IL_08dc: Unknown result type (might be due to invalid IL or missing references)
		//IL_08e7: Unknown result type (might be due to invalid IL or missing references)
		//IL_08ec: Unknown result type (might be due to invalid IL or missing references)
		//IL_08f5: Unknown result type (might be due to invalid IL or missing references)
		//IL_08fa: Unknown result type (might be due to invalid IL or missing references)
		//IL_08ab: Unknown result type (might be due to invalid IL or missing references)
		//IL_021c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0223: Expected O, but got Unknown
		//IL_0264: Unknown result type (might be due to invalid IL or missing references)
		//IL_026e: Expected O, but got Unknown
		//IL_0288: Unknown result type (might be due to invalid IL or missing references)
		//IL_0292: Expected O, but got Unknown
		//IL_0907: Unknown result type (might be due to invalid IL or missing references)
		//IL_090c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0943: Unknown result type (might be due to invalid IL or missing references)
		//IL_0948: Unknown result type (might be due to invalid IL or missing references)
		//IL_094e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0953: Unknown result type (might be due to invalid IL or missing references)
		//IL_095e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0963: Unknown result type (might be due to invalid IL or missing references)
		//IL_096e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0973: Unknown result type (might be due to invalid IL or missing references)
		//IL_097c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0988: Unknown result type (might be due to invalid IL or missing references)
		//IL_098d: Unknown result type (might be due to invalid IL or missing references)
		//IL_098f: Unknown result type (might be due to invalid IL or missing references)
		//IL_09a1: Unknown result type (might be due to invalid IL or missing references)
		//IL_091e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0928: Unknown result type (might be due to invalid IL or missing references)
		//IL_0932: Unknown result type (might be due to invalid IL or missing references)
		//IL_0939: Expected O, but got Unknown
		//IL_0301: Unknown result type (might be due to invalid IL or missing references)
		//IL_0308: Expected O, but got Unknown
		//IL_09c8: Unknown result type (might be due to invalid IL or missing references)
		//IL_09ca: Unknown result type (might be due to invalid IL or missing references)
		//IL_049a: Unknown result type (might be due to invalid IL or missing references)
		//IL_04a1: Expected O, but got Unknown
		//IL_04aa: Unknown result type (might be due to invalid IL or missing references)
		//IL_04b1: Expected O, but got Unknown
		//IL_0340: Unknown result type (might be due to invalid IL or missing references)
		//IL_034a: Expected O, but got Unknown
		//IL_0345: Unknown result type (might be due to invalid IL or missing references)
		//IL_034c: Expected O, but got Unknown
		//IL_0366: Unknown result type (might be due to invalid IL or missing references)
		//IL_0372: Expected O, but got Unknown
		//IL_036d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0374: Expected O, but got Unknown
		//IL_03d4: Unknown result type (might be due to invalid IL or missing references)
		//IL_03de: Expected O, but got Unknown
		//IL_03d9: Unknown result type (might be due to invalid IL or missing references)
		//IL_03e0: Expected O, but got Unknown
		//IL_03fa: Unknown result type (might be due to invalid IL or missing references)
		//IL_0406: Expected O, but got Unknown
		//IL_0401: Unknown result type (might be due to invalid IL or missing references)
		//IL_0408: Expected O, but got Unknown
		//IL_09d6: Unknown result type (might be due to invalid IL or missing references)
		//IL_09d8: Unknown result type (might be due to invalid IL or missing references)
		//IL_0a48: Unknown result type (might be due to invalid IL or missing references)
		//IL_0a4f: Expected O, but got Unknown
		//IL_0a59: Unknown result type (might be due to invalid IL or missing references)
		//IL_0a60: Expected O, but got Unknown
		//IL_0a6a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0a71: Expected O, but got Unknown
		//IL_0ac6: Unknown result type (might be due to invalid IL or missing references)
		//IL_0acd: Expected O, but got Unknown
		//IL_0b0e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0b22: Expected O, but got Unknown
		//IL_0b3c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0b41: Unknown result type (might be due to invalid IL or missing references)
		//IL_0c49: Unknown result type (might be due to invalid IL or missing references)
		//IL_0c4e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0c6f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0c74: Unknown result type (might be due to invalid IL or missing references)
		//IL_0b64: Unknown result type (might be due to invalid IL or missing references)
		//IL_0b6b: Expected O, but got Unknown
		//IL_0b50: Unknown result type (might be due to invalid IL or missing references)
		//IL_0b55: Unknown result type (might be due to invalid IL or missing references)
		//IL_0d6e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0d90: Unknown result type (might be due to invalid IL or missing references)
		//IL_0b91: Unknown result type (might be due to invalid IL or missing references)
		//IL_0b98: Expected O, but got Unknown
		//IL_0bdf: Unknown result type (might be due to invalid IL or missing references)
		//IL_0be9: Expected O, but got Unknown
		//IL_0cd4: Unknown result type (might be due to invalid IL or missing references)
		//IL_0cdb: Expected O, but got Unknown
		//IL_0ce7: Unknown result type (might be due to invalid IL or missing references)
		//IL_0d4b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0d37: Unknown result type (might be due to invalid IL or missing references)
		//IL_0d3e: Expected O, but got Unknown
		outputMetals = new List<OnBrep>();
		outputTransforms = new List<OnXform>();
		emeraldCorner1Point = Point3d.Unset;
		emeraldCorner2Point = Point3d.Unset;
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
		OnCurve val25;
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
			double num3 = profileHeight - gemSizeZ + channelBottomOffset;
			bool num4 = num3 > val9.PointAtEnd().y - channelWallDepth;
			bool flag = 0.0 > num3;
			if (num4)
			{
				num3 = val9.PointAtEnd().y - channelWallDepth;
			}
			if (flag)
			{
				num3 = 0.0;
			}
			On3dPoint val11 = new On3dPoint(num2, num3, 0.0);
			OnCurve val20;
			OnCurve val21;
			if (channelWallDepth > cornerBlend * 2.0)
			{
				On3dPoint val12 = val9.PointAtEnd();
				OnCurve val13 = (OnCurve)new OnLineCurve((IOn3dPoint)(object)val12, (IOn3dPoint)new On3dPoint(val12.x, val12.y - channelWallDepth, 0.0));
				OnCurve val14 = (OnCurve)new OnLineCurve((IOn3dPoint)new On3dPoint(val12.x, val12.y - channelWallDepth, 0.0), (IOn3dPoint)(object)val11);
				CSUtilGeometry.TrimEndOfCrv(val13, cornerBlend);
				CSUtilGeometry.TrimStartOfCrv(val14, cornerBlend);
				OnCurve val15 = (OnCurve)(object)CSUtilCurves.BlendCurve((IOnCurve)(object)val13, (IOnCurve)(object)val14);
				On3dPoint val16 = val10.PointAtStart();
				OnCurve val17 = (OnCurve)new OnLineCurve((IOn3dPoint)(object)val11, (IOn3dPoint)new On3dPoint(val16.x, val16.y - channelWallDepth, 0.0));
				OnCurve val18 = (OnCurve)new OnLineCurve((IOn3dPoint)new On3dPoint(val16.x, val16.y - channelWallDepth, 0.0), (IOn3dPoint)(object)val16);
				CSUtilGeometry.TrimEndOfCrv(val17, cornerBlend);
				CSUtilGeometry.TrimStartOfCrv(val18, cornerBlend);
				OnCurve val19 = (OnCurve)(object)CSUtilCurves.BlendCurve((IOnCurve)(object)val17, (IOnCurve)(object)val18);
				IOnCurve[] obj = new IOnCurve[3]
				{
					(IOnCurve)val13,
					(IOnCurve)val15,
					(IOnCurve)val14
				};
				OnCurve[] array = null;
				RhUtil.RhinoMergeCurves((IOnCurve[])(object)obj, ref array);
				val20 = array[0];
				IOnCurve[] obj2 = new IOnCurve[3]
				{
					(IOnCurve)val17,
					(IOnCurve)val19,
					(IOnCurve)val18
				};
				array = null;
				RhUtil.RhinoMergeCurves((IOnCurve[])(object)obj2, ref array);
				val21 = array[0];
			}
			else
			{
				val20 = (OnCurve)new OnLineCurve((IOn3dPoint)(object)val9.PointAtEnd(), (IOn3dPoint)(object)val11);
				val21 = (OnCurve)new OnLineCurve((IOn3dPoint)(object)val11, (IOn3dPoint)(object)val10.PointAtStart());
			}
			CSUtilGeometry.TrimEndOfCrv(val9, cornerBlend);
			CSUtilGeometry.TrimStartAndEndOfCrv(val20, cornerBlend);
			CSUtilGeometry.TrimStartAndEndOfCrv(val21, cornerBlend);
			CSUtilGeometry.TrimStartOfCrv(val10, cornerBlend);
			OnCurve val22 = (OnCurve)(object)CSUtilCurves.BlendCurve((IOnCurve)(object)val9, (IOnCurve)(object)val20);
			OnCurve val23 = (OnCurve)(object)CSUtilCurves.BlendCurve((IOnCurve)(object)val20, (IOnCurve)(object)val21);
			OnCurve val24 = (OnCurve)(object)CSUtilCurves.BlendCurve((IOnCurve)(object)val21, (IOnCurve)(object)val10);
			IOnCurve[] obj3 = new IOnCurve[7]
			{
				(IOnCurve)val9,
				(IOnCurve)val22,
				(IOnCurve)val20,
				(IOnCurve)val23,
				(IOnCurve)val21,
				(IOnCurve)val24,
				(IOnCurve)val10
			};
			OnCurve[] array2 = null;
			if (!RhUtil.RhinoMergeCurves((IOnCurve[])(object)obj3, ref array2))
			{
				outError = "channel Merge failed";
				return false;
			}
			if (array2.Length != 1)
			{
				outError = "channel Merge failed";
				return false;
			}
			val25 = array2[0];
		}
		else
		{
			OnXform val26 = new OnXform();
			val26.Scale(1.0, profileHeight, 1.0);
			profile.Transform((IOnXform)(object)val26);
			val25 = (OnCurve)(object)profile;
		}
		OnXform val27 = new OnXform();
		val27.Translation(0.0, 0.0 - profileHeight, 0.0);
		OnXform val28 = new OnXform();
		val28.Rotation(angle, OnUtil.On_zaxis, OnUtil.On_origin);
		((OnGeometry)val25).Transform((IOnXform)(object)(val28 * (IOnXform)(object)val27));
		BoundingBox boundingBox = BoundingBox.Unset;
		centerObject.GetTightBoundingBox(ref boundingBox);
		Point3d val29 = ((BoundingBox)(ref boundingBox)).Max;
		double x = ((Point3d)(ref val29)).X;
		val29 = ((BoundingBox)(ref boundingBox)).Min;
		double num5 = Math.Abs(x - ((Point3d)(ref val29)).X + offsetX);
		val29 = ((BoundingBox)(ref boundingBox)).Max;
		double y = ((Point3d)(ref val29)).Y;
		val29 = ((BoundingBox)(ref boundingBox)).Min;
		double num6 = Math.Abs(y - ((Point3d)(ref val29)).Y + offsetY);
		string gemShape;
		if (centerGemShapeOverrideEnabled)
		{
			if (centerGemShapeOverride == null)
			{
				centerGemShapeOverride = new CSGemData(centerGem);
			}
			else
			{
				double num7;
				double num8;
				if (centerGem != null)
				{
					switch (centerGemShapeOverrideMode)
					{
					case ValueListItem.GemClusterCenterShapeOverrideModeDimensionX:
						num7 = (centerGem.GemXSize + offsetX) / centerGemShapeOverride.GemXSize;
						num8 = num7;
						break;
					case ValueListItem.GemClusterCenterShapeOverrideModeDimensionY:
						num8 = (centerGem.GemYSize + offsetY) / centerGemShapeOverride.GemYSize;
						num7 = num8;
						break;
					case ValueListItem.GemClusterCenterShapeOverrideModeDimensionLargest:
						if (centerGem.GemXSize > centerGem.GemYSize)
						{
							num7 = (centerGem.GemXSize + offsetX) / centerGemShapeOverride.GemXSize;
							num8 = num7;
						}
						else
						{
							num8 = (centerGem.GemYSize + offsetY) / centerGemShapeOverride.GemYSize;
							num7 = num8;
						}
						break;
					case ValueListItem.GemClusterCenterShapeOverrideModeDimensionSmallest:
						if (centerGem.GemXSize < centerGem.GemYSize)
						{
							num7 = (centerGem.GemXSize + offsetX) / centerGemShapeOverride.GemXSize;
							num8 = num7;
						}
						else
						{
							num8 = (centerGem.GemYSize + offsetY) / centerGemShapeOverride.GemYSize;
							num7 = num8;
						}
						break;
					case ValueListItem.GemClusterCenterShapeOverrideModeDimensionXandY:
						num7 = (centerGem.GemXSize + offsetX) / centerGemShapeOverride.GemXSize;
						num8 = (centerGem.GemYSize + offsetY) / centerGemShapeOverride.GemYSize;
						break;
					default:
						outError = "Invalid Gem Cluster Center Shape Override Mode";
						return false;
					}
				}
				else
				{
					switch (centerGemShapeOverrideMode)
					{
					case ValueListItem.GemClusterCenterShapeOverrideModeDimensionX:
						num7 = num5 / centerGemShapeOverride.GemXSize;
						num8 = num7;
						break;
					case ValueListItem.GemClusterCenterShapeOverrideModeDimensionY:
						num8 = num6 / centerGemShapeOverride.GemYSize;
						num7 = num8;
						break;
					case ValueListItem.GemClusterCenterShapeOverrideModeDimensionLargest:
						if (num5 > num6)
						{
							num7 = num5 / centerGemShapeOverride.GemXSize;
							num8 = num7;
						}
						else
						{
							num8 = num6 / centerGemShapeOverride.GemYSize;
							num7 = num8;
						}
						break;
					case ValueListItem.GemClusterCenterShapeOverrideModeDimensionSmallest:
						if (num5 < num6)
						{
							num7 = num5 / centerGemShapeOverride.GemXSize;
							num8 = num7;
						}
						else
						{
							num8 = num6 / centerGemShapeOverride.GemYSize;
							num7 = num8;
						}
						break;
					case ValueListItem.GemClusterCenterShapeOverrideModeDimensionXandY:
						num7 = num5 / centerGemShapeOverride.GemXSize;
						num8 = num6 / centerGemShapeOverride.GemYSize;
						break;
					default:
						outError = "Invalid Gem Cluster Center Shape Override Mode";
						return false;
					}
				}
				centerGemShapeOverride.Scale(num7, num8);
			}
			gemShape = centerGemShapeOverride.GemShape;
		}
		else
		{
			centerGemShapeOverride = new CSGemData(centerGem);
			gemShape = centerGem.GemShape;
		}
		Transform xUp = default(Transform);
		Transform xDown;
		if (centerGem != null)
		{
			if (!CSBlockUtil.CSUtilMath.GetUnscaledXforms(centerGem.Xform, out xUp, out xDown))
			{
				outError = "Gem X form";
				return false;
			}
		}
		else
		{
			val29 = ((BoundingBox)(ref boundingBox)).Center;
			double x2 = ((Point3d)(ref val29)).X;
			val29 = ((BoundingBox)(ref boundingBox)).Center;
			double y2 = ((Point3d)(ref val29)).Y;
			val29 = ((BoundingBox)(ref boundingBox)).Center;
			xDown = Transform.Translation(x2, y2, ((Point3d)(ref val29)).Z);
			if (!((Transform)(ref xDown)).TryGetInverse(ref xUp))
			{
				xUp = Transform.Identity;
			}
		}
		CSGemData cSGemData = null;
		On3dVector val30;
		if (centerGem != null)
		{
			cSGemData = new CSGemData(centerGem);
			cSGemData.ApplyTransform(xUp);
			val30 = new On3dVector((IOn3dVector)(object)cSGemData.Up.Convert());
			val30.Unitize();
		}
		else
		{
			Point3d origin = Point3d.Origin;
			val29 = ((BoundingBox)(ref boundingBox)).Center;
			double x3 = ((Point3d)(ref val29)).X;
			val29 = ((BoundingBox)(ref boundingBox)).Center;
			double y3 = ((Point3d)(ref val29)).Y;
			val29 = ((BoundingBox)(ref boundingBox)).Center;
			((Point3d)(ref origin)).Transform(Transform.Translation(x3, y3, ((Point3d)(ref val29)).Z));
			Vector3d value = default(Vector3d);
			((Vector3d)(ref value))._002Ector(Point3d.Origin - origin);
			((Vector3d)(ref value)).Unitize();
			val30 = CSConvert.Vector3dCopyToOn3dVector(value);
		}
		double optionalEmeraldCornerXPercent = 0.0;
		double optionalEmeraldCornerYPercent = 0.0;
		if (inputCorner1 != Point3d.Unset && inputCorner2 != Point3d.Unset)
		{
			double num9 = centerGemShapeOverride.GemXSize / 2.0;
			optionalEmeraldCornerXPercent = ((Point3d)(ref inputCorner1)).X / num9;
			double num10 = centerGemShapeOverride.GemYSize / 2.0;
			optionalEmeraldCornerYPercent = ((Point3d)(ref inputCorner2)).Y / num10;
		}
		if (!GemOffsetCurveV2.GemOffset(gemShape, centerGemShapeOverride, centerGemSpacing, out OnCurve curve, false, out string error, blendCornerGemShape, optionalEmeraldCornerXPercent, optionalEmeraldCornerYPercent))
		{
			outError = "CurveTop Inside:" + error;
			return false;
		}
		OnNurbsCurve val31 = new OnNurbsCurve();
		curve.GetNurbForm(ref val31);
		OnNurbsCurve val32 = new OnNurbsCurve();
		val25.GetNurbForm(ref val32);
		OnNurbsSurface val33 = new OnNurbsSurface();
		val33.Create(3, true, val32.m_order, val31.m_order, val32.m_cv_count, val31.m_cv_count);
		bool flag2 = false;
		List<IOnCurve> list = new List<IOnCurve>();
		CSGemData gem = (doScaleToGemXY ? cSGemData : centerGemShapeOverride);
		double y4 = curve.PointAtStart().y;
		OnCurve val34 = curve;
		for (int i = 0; i < val32.CVCount(); i++)
		{
			On3dPoint val35 = new On3dPoint();
			val32.GetCV(i, ref val35);
			if (!GemOffsetCurveV2.GemOffset(gemShape, gem, centerGemSpacing + val35.x, out curve, false, out error, blendCornerGemShape, optionalEmeraldCornerXPercent, optionalEmeraldCornerYPercent))
			{
				outError = "CurveTop Inside:" + error;
				return false;
			}
			((OnGeometry)curve).Translate((IOn3dVector)(object)(new On3dVector((IOn3dVector)(object)val30) * (placement + val35.y)));
			list.Add((IOnCurve)(object)curve);
			Curve val36 = Interop.FromOnCurve((object)curve);
			val29 = val36.PointAtStart;
			if (((Point3d)(ref val29)).Y > y4)
			{
				val29 = val36.PointAtStart;
				y4 = ((Point3d)(ref val29)).Y;
				val34 = curve;
			}
			OnNurbsCurve val37 = new OnNurbsCurve();
			curve.GetNurbForm(ref val37);
			if (val31.CVCount() != val37.CVCount())
			{
				flag2 = true;
			}
			if (!flag2)
			{
				for (int j = 0; j < val31.CVCount(); j++)
				{
					On3dPoint val38 = new On3dPoint();
					val37.GetCV(j, ref val38);
					double num11 = val32.Weight(i) * val37.Weight(j);
					val33.SetCV(i, j, (IOn4dPoint)new On4dPoint(val38.x * num11, val38.y * num11, val38.z * num11, num11));
				}
			}
		}
		if (gemShape == "Emerald")
		{
			Curve val39 = Interop.FromOnCurve((object)val34);
			double num12 = default(double);
			val39.GetNextDiscontinuity((Continuity)3, 0.0, 10.0, ref num12);
			emeraldCorner1Point = val39.PointAt(num12);
			val39.GetNextDiscontinuity((Continuity)3, num12, 20.0, ref num12);
			emeraldCorner2Point = val39.PointAt(num12);
		}
		if (!flag2)
		{
			for (int k = 0; k < val32.KnotCount(); k++)
			{
				val33.SetKnot(0, k, val32.Knot(k));
			}
			for (int l = 0; l < val31.KnotCount(); l++)
			{
				val33.SetKnot(1, l, val31.Knot(l));
			}
			OnBrep brep = new OnBrep();
			((OnGeometry)val33).BrepForm(brep);
			((OnGeometry)brep).Transform((IOnXform)(object)xDown.Convert());
			CSUtilGeometry.FixBrepSolidOrientation(ref brep);
			if (!brep.IsSolid())
			{
				Brep val40 = Interop.FromOnBrep((object)brep);
				val40.JoinNakedEdges(CSApplicationHelper.Application.Tolerance);
				if (!val40.IsSolid)
				{
					val40.Flip();
				}
				brep = (OnBrep)Interop.ToOnBrep(val40);
			}
			outputMetals.Add(brep);
			outputTransforms.Add(xDown.Convert());
		}
		else
		{
			OnBrep brep2 = CSUtilSweep.Loft(list.ToArray(), (eLoftType)1);
			if (brep2 != null)
			{
				((OnGeometry)brep2).Transform((IOnXform)(object)xDown.Convert());
				CSUtilGeometry.FixBrepSolidOrientation(ref brep2);
				outputMetals.Add(brep2);
				outputTransforms.Add(xDown.Convert());
			}
		}
		outError = string.Empty;
		return true;
	}
}
