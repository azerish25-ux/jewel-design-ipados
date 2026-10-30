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
using Rhino;
using Rhino.Geometry;
using Rhino.Runtime;

namespace CSBlocks.GemClusters.Obsolete;

[Obsolete]
[CSBlock("HaloBuilder_V3", "{A2DA2C3D-F1B2-463E-8F6C-D56DA090D492}")]
public sealed class CSHaloBuilder3 : CSBlock
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

	public CSInputChannel InBlendCornerInside { get; }

	public CSInputChannel InOffsetX { get; }

	public CSInputChannel InOffsetY { get; }

	public CSInputChannel InEmeraldCornerPoint1 { get; }

	public CSInputChannel InEmeraldCornerPoint2 { get; }

	public CSInputChannel InRotate { get; }

	public CSInputChannel InBlendCornerOutside { get; }

	public CSInputChannel InToggleBlendedCorner { get; }

	public CSInputChannel InGemSizeToggle { get; }

	public CSInputChannel InGemSize { get; }

	public CSInputChannel InChannelSpacing { get; }

	public CSInputChannel InForceChannel { get; }

	public CSOutputChannel OutMetal { get; }

	public CSOutputChannel OutEmeraldCorner1Point { get; }

	public CSOutputChannel OutEmeraldCorner2Point { get; }

	public CSHaloBuilder3(string identifier)
		: base(identifier)
	{
		//IL_0259: Unknown result type (might be due to invalid IL or missing references)
		//IL_026d: Unknown result type (might be due to invalid IL or missing references)
		InCenterGem = new CSInputChannel(this, ChannelDataType.Gem, ChannelArrayType.Array);
		InCenterGemShapeOverrideEnabled = new CSInputChannel(this, ChannelDataType.Boolean, ChannelArrayType.Individual, defaultValue: false);
		InCenterGemShapeOverride = new CSInputChannel(this, ChannelDataType.Gem, ChannelArrayType.Individual);
		InCenterGemSpacing = new CSInputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual, 0.0);
		InProfile = new CSInputChannel(this, ChannelDataType.Curve, ChannelArrayType.Individual);
		InProfile2 = new CSInputChannel(this, ChannelDataType.Curve, ChannelArrayType.Individual);
		InProfileWidth = new CSInputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual, 0.0);
		InProfileHeight = new CSInputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual, 0.0);
		InChannelWallThicknessInner = new CSInputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual, 0.0);
		InChannelWallThicknessOuter = new CSInputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual, 0.0);
		InChannelWallDepth = new CSInputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual, 0.0);
		InAngle = new CSInputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual, 0.0);
		InPlacement = new CSInputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual, 0.0);
		InCornerBlend = new CSInputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual, 0.0);
		InGemSizeZ = new CSInputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual, 0.0);
		InShowChannel = new CSInputChannel(this, ChannelDataType.Boolean, ChannelArrayType.Individual, defaultValue: true);
		InXYScaleToGem = new CSInputChannel(this, ChannelDataType.Boolean, ChannelArrayType.Individual, defaultValue: false);
		InCenterGemShapeOverrideMode = new CSInputChannel(this, ChannelDataType.Integer, ChannelArrayType.Individual, ValueListItem.GemClusterCenterShapeOverrideModeDimensionSmallest);
		InOffsetFix = new CSInputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual, 0.0);
		InInnerWallExtra = new CSInputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual, 0.0);
		InOuterWallExtra = new CSInputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual, 0.0);
		InRailX = new CSInputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual, 0.0);
		InRailZ = new CSInputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual, 0.0);
		InChannelBottomOffset = new CSInputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual, 0.0);
		InBlendCornerInside = new CSInputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual, 0.0);
		InMetalToggle = new CSInputChannel(this, ChannelDataType.Boolean, ChannelArrayType.Individual, defaultValue: true);
		InOffsetX = new CSInputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual, 0.0);
		InOffsetY = new CSInputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual, 0.0);
		InEmeraldCornerPoint1 = new CSInputChannel(this, ChannelDataType.Point, ChannelArrayType.Individual, Point3d.Unset);
		InEmeraldCornerPoint2 = new CSInputChannel(this, ChannelDataType.Point, ChannelArrayType.Individual, Point3d.Unset);
		InRotate = new CSInputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual, 0.0);
		InBlendCornerOutside = new CSInputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual, 0.0);
		InToggleBlendedCorner = new CSInputChannel(this, ChannelDataType.Boolean, ChannelArrayType.Individual, defaultValue: false);
		InGemSizeToggle = new CSInputChannel(this, ChannelDataType.Boolean, ChannelArrayType.Individual, defaultValue: false);
		InGemSize = new CSInputChannel(this, ChannelDataType.Gem, ChannelArrayType.Individual);
		InChannelSpacing = new CSInputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual, 0.0);
		InForceChannel = new CSInputChannel(this, ChannelDataType.Boolean, ChannelArrayType.Individual, defaultValue: false);
		OutMetal = new CSOutputChannel(this, ChannelDataType.Any, ChannelArrayType.Array);
		OutEmeraldCorner1Point = new CSOutputChannel(this, ChannelDataType.Point, ChannelArrayType.Individual);
		OutEmeraldCorner2Point = new CSOutputChannel(this, ChannelDataType.Point, ChannelArrayType.Individual);
		SetChannelIdentifiers(autoCheckInputs: true);
	}

	public override bool Update()
	{
		//IL_0225: Unknown result type (might be due to invalid IL or missing references)
		//IL_022a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0232: Unknown result type (might be due to invalid IL or missing references)
		//IL_0237: Unknown result type (might be due to invalid IL or missing references)
		//IL_02b6: Unknown result type (might be due to invalid IL or missing references)
		//IL_02b8: Unknown result type (might be due to invalid IL or missing references)
		//IL_02dc: Unknown result type (might be due to invalid IL or missing references)
		//IL_02ee: Unknown result type (might be due to invalid IL or missing references)
		//IL_031f: Unknown result type (might be due to invalid IL or missing references)
		if (!InMetalToggle.Boolean)
		{
			OutMetal.SetData(null);
			return base.Update();
		}
		List<CSGemData> gemList = InCenterGem.GemList;
		if (gemList.Count <= 0)
		{
			return UpdateMissing("CenterGem");
		}
		CSGemData cSGemData = gemList[0];
		CSData centerObject = gemList[0];
		bool centerGemShapeOverrideEnabled = InCenterGemShapeOverrideEnabled.Boolean || cSGemData == null;
		CSGemData gem = InCenterGemShapeOverride.Gem;
		double num = InCenterGemSpacing.Double;
		double profileWidth = InProfileWidth.Double;
		double profileHeight = InProfileHeight.Double;
		double channelWallThicknessInner = InChannelWallThicknessInner.Double;
		double channelWallThicknessOuter = InChannelWallThicknessOuter.Double;
		double channelWallDepth = InChannelWallDepth.Double;
		double angleDegrees = InAngle.Double;
		angleDegrees = CSBlockUtil.CSUtilMath.ToRadians(angleDegrees);
		double num2 = InPlacement.Double;
		double cornerBlend = InCornerBlend.Double;
		double gemSizeZ = InGemSizeZ.Double;
		double channelBottomOffset = InChannelBottomOffset.Double;
		double blendCornerInside = InBlendCornerInside.Double;
		double blendCornerOutside = InBlendCornerOutside.Double;
		if (!InToggleBlendedCorner.Boolean)
		{
			blendCornerOutside = 0.0;
			blendCornerInside = 0.0;
		}
		double num3 = InOffsetFix.Double;
		num += num3;
		bool flag = InShowChannel.Boolean;
		if (InForceChannel.Boolean)
		{
			flag = true;
		}
		bool boolean = InXYScaleToGem.Boolean;
		double innerWallExtra = InInnerWallExtra.Double;
		double outerWallExtra = InOuterWallExtra.Double;
		OnNurbsCurve profile = (flag ? InProfile2.NurbsCurve.Convert().Duplicate() : InProfile.NurbsCurve.Convert().Duplicate());
		ValueListItem valueListItem = InCenterGemShapeOverrideMode.ValueListItem;
		double num4 = InRailX.Double;
		num += num4;
		double num5 = InRailZ.Double;
		num2 += num5;
		double offsetX = InOffsetX.Double;
		double offsetY = InOffsetY.Double;
		Point3d point3d = InEmeraldCornerPoint1.Point3d;
		Point3d point3d2 = InEmeraldCornerPoint2.Point3d;
		double rotate = InRotate.Double;
		bool boolean2 = InGemSizeToggle.Boolean;
		CSGemData gem2 = InGemSize.Gem;
		double channelSpacing = InChannelSpacing.Double;
		if (!MakeClusterMetalProfile(profile, profileWidth, profileHeight, channelWallThicknessInner, channelWallThicknessOuter, channelWallDepth, angleDegrees, cornerBlend, gemSizeZ, flag, innerWallExtra, outerWallExtra, channelBottomOffset, boolean2, gem2, channelSpacing, out var channelProfile, out var outError))
		{
			return UpdateError(outError);
		}
		if (!MakeClusterMetal(cSGemData, centerObject, centerGemShapeOverrideEnabled, gem, num, (IOnCurve)(object)channelProfile, num2, boolean, valueListItem, blendCornerInside, blendCornerOutside, offsetX, offsetY, point3d, point3d2, rotate, out var outputMetals, out var outputTransforms, out var emeraldCorner1Point, out var emeraldCorner2Point, out outError))
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

	private static bool MakeClusterMetalProfile(OnNurbsCurve profile, double profileWidth, double profileHeight, double channelWallThicknessInner, double channelWallThicknessOuter, double channelWallDepth, double angle, double cornerBlend, double gemSizeZ, bool showChannel, double innerWallExtra, double outerWallExtra, double channelBottomOffset, bool gemSizeToggle, CSGemData gemSize, double channelSpacing, out OnCurve channelProfile, out string outError)
	{
		//IL_0008: Unknown result type (might be due to invalid IL or missing references)
		//IL_000e: Expected O, but got Unknown
		//IL_0017: Unknown result type (might be due to invalid IL or missing references)
		//IL_001d: Expected O, but got Unknown
		//IL_0044: Unknown result type (might be due to invalid IL or missing references)
		//IL_004a: Expected O, but got Unknown
		//IL_009b: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a1: Expected O, but got Unknown
		//IL_00d9: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e0: Expected O, but got Unknown
		//IL_058d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0594: Expected O, but got Unknown
		//IL_0131: Unknown result type (might be due to invalid IL or missing references)
		//IL_0149: Unknown result type (might be due to invalid IL or missing references)
		//IL_0153: Expected O, but got Unknown
		//IL_0153: Expected O, but got Unknown
		//IL_014e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0155: Expected O, but got Unknown
		//IL_016b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0186: Unknown result type (might be due to invalid IL or missing references)
		//IL_0190: Expected O, but got Unknown
		//IL_0190: Expected O, but got Unknown
		//IL_018b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0192: Expected O, but got Unknown
		//IL_05bb: Unknown result type (might be due to invalid IL or missing references)
		//IL_05c2: Expected O, but got Unknown
		//IL_05dd: Unknown result type (might be due to invalid IL or missing references)
		//IL_05e4: Expected O, but got Unknown
		//IL_020a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0211: Expected O, but got Unknown
		//IL_024e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0258: Expected O, but got Unknown
		//IL_0270: Unknown result type (might be due to invalid IL or missing references)
		//IL_027a: Expected O, but got Unknown
		//IL_02e5: Unknown result type (might be due to invalid IL or missing references)
		//IL_02ec: Expected O, but got Unknown
		//IL_047e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0485: Expected O, but got Unknown
		//IL_048e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0495: Expected O, but got Unknown
		//IL_0324: Unknown result type (might be due to invalid IL or missing references)
		//IL_032e: Expected O, but got Unknown
		//IL_0329: Unknown result type (might be due to invalid IL or missing references)
		//IL_0330: Expected O, but got Unknown
		//IL_034a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0356: Expected O, but got Unknown
		//IL_0351: Unknown result type (might be due to invalid IL or missing references)
		//IL_0358: Expected O, but got Unknown
		//IL_03b8: Unknown result type (might be due to invalid IL or missing references)
		//IL_03c2: Expected O, but got Unknown
		//IL_03bd: Unknown result type (might be due to invalid IL or missing references)
		//IL_03c4: Expected O, but got Unknown
		//IL_03de: Unknown result type (might be due to invalid IL or missing references)
		//IL_03ea: Expected O, but got Unknown
		//IL_03e5: Unknown result type (might be due to invalid IL or missing references)
		//IL_03ec: Expected O, but got Unknown
		channelProfile = null;
		outError = null;
		OnBoundingBox val = new OnBoundingBox();
		((OnGeometry)profile).GetTightBoundingBox(ref val);
		OnXform val2 = new OnXform();
		val2.Translation(0.0 - val.Min().x, 0.0 - val.Min().y, 0.0);
		OnXform val3 = new OnXform();
		val3.Scale(1.0 / (val.Max().x - val.Min().x), 1.0 / (val.Max().y - val.Min().y), 0.0);
		OnXform val4 = new OnXform();
		if (gemSizeToggle && gemSize != null)
		{
			profileWidth = gemSize.GemXSize + channelSpacing + channelWallThicknessInner + channelWallThicknessOuter;
		}
		val4.Scale(profileWidth + innerWallExtra + outerWallExtra, 1.0, 1.0);
		OnXform val5 = new OnXform();
		val5.Translation(0.0 - innerWallExtra, 0.0, 0.0);
		profile.Transform((IOnXform)(object)(val5 * (IOnXform)(object)val4 * (IOnXform)(object)val3 * (IOnXform)(object)val2));
		if (showChannel)
		{
			OnLineCurve c = new OnLineCurve((IOn3dPoint)new On3dPoint(channelWallThicknessInner, -1.0, 0.0), (IOn3dPoint)new On3dPoint(channelWallThicknessInner, 2.0, 0.0));
			OnLineCurve c2 = new OnLineCurve((IOn3dPoint)new On3dPoint(profileWidth - channelWallThicknessOuter, -1.0, 0.0), (IOn3dPoint)new On3dPoint(profileWidth - channelWallThicknessOuter, 2.0, 0.0));
			double param = 0.0;
			double param2 = 0.0;
			if (!CurveIntersectHighest((IOnCurve)(object)profile, (IOnCurve)(object)c, ref param))
			{
				outError = "Intersect for channel inner failed";
				return false;
			}
			if (!CurveIntersectHighest((IOnCurve)(object)profile, (IOnCurve)(object)c2, ref param2))
			{
				outError = "Intersect for channel outer failed";
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
				outError = "channel merge failed";
				return false;
			}
			if (array2.Length != 1)
			{
				outError = "channel merge produced bad results";
				return false;
			}
			channelProfile = array2[0];
		}
		else
		{
			OnXform val25 = new OnXform();
			val25.Scale(1.0, profileHeight, 1.0);
			profile.Transform((IOnXform)(object)val25);
			channelProfile = (OnCurve)(object)profile;
		}
		OnXform val26 = new OnXform();
		val26.Translation(0.0, 0.0 - profileHeight, 0.0);
		OnXform val27 = new OnXform();
		val27.Rotation(angle, OnUtil.On_zaxis, OnUtil.On_origin);
		((OnGeometry)channelProfile).Transform((IOnXform)(object)(val27 * (IOnXform)(object)val26));
		return true;
	}

	private static bool MakeClusterMetal(CSGemData centerGem, CSData centerObject, bool centerGemShapeOverrideEnabled, CSGemData centerGemShapeOverride, double centerGemSpacing, IOnCurve channelProfile, double placement, bool doScaleToGemXY, ValueListItem centerGemShapeOverrideMode, double blendCornerInside, double blendCornerOutside, double offsetX, double offsetY, Point3d inputCorner1, Point3d inputCorner2, double rotate, out List<OnBrep> outputMetals, out List<OnXform> outputTransforms, out Point3d emeraldCorner1Point, out Point3d emeraldCorner2Point, out string outError)
	{
		//IL_0012: Unknown result type (might be due to invalid IL or missing references)
		//IL_0017: Unknown result type (might be due to invalid IL or missing references)
		//IL_001e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0023: Unknown result type (might be due to invalid IL or missing references)
		//IL_0028: Unknown result type (might be due to invalid IL or missing references)
		//IL_002d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0039: Unknown result type (might be due to invalid IL or missing references)
		//IL_003e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0049: Unknown result type (might be due to invalid IL or missing references)
		//IL_004e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0063: Unknown result type (might be due to invalid IL or missing references)
		//IL_0068: Unknown result type (might be due to invalid IL or missing references)
		//IL_0073: Unknown result type (might be due to invalid IL or missing references)
		//IL_0078: Unknown result type (might be due to invalid IL or missing references)
		//IL_02b2: Unknown result type (might be due to invalid IL or missing references)
		//IL_02b7: Unknown result type (might be due to invalid IL or missing references)
		//IL_02c2: Unknown result type (might be due to invalid IL or missing references)
		//IL_02c7: Unknown result type (might be due to invalid IL or missing references)
		//IL_02d2: Unknown result type (might be due to invalid IL or missing references)
		//IL_02d7: Unknown result type (might be due to invalid IL or missing references)
		//IL_02e0: Unknown result type (might be due to invalid IL or missing references)
		//IL_02ec: Unknown result type (might be due to invalid IL or missing references)
		//IL_02f1: Unknown result type (might be due to invalid IL or missing references)
		//IL_02f6: Unknown result type (might be due to invalid IL or missing references)
		//IL_02fb: Unknown result type (might be due to invalid IL or missing references)
		//IL_0300: Unknown result type (might be due to invalid IL or missing references)
		//IL_0296: Unknown result type (might be due to invalid IL or missing references)
		//IL_030d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0312: Unknown result type (might be due to invalid IL or missing references)
		//IL_0350: Unknown result type (might be due to invalid IL or missing references)
		//IL_0355: Unknown result type (might be due to invalid IL or missing references)
		//IL_0360: Unknown result type (might be due to invalid IL or missing references)
		//IL_0365: Unknown result type (might be due to invalid IL or missing references)
		//IL_037c: Unknown result type (might be due to invalid IL or missing references)
		//IL_037e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0396: Unknown result type (might be due to invalid IL or missing references)
		//IL_039b: Unknown result type (might be due to invalid IL or missing references)
		//IL_03a4: Unknown result type (might be due to invalid IL or missing references)
		//IL_0324: Unknown result type (might be due to invalid IL or missing references)
		//IL_032e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0338: Unknown result type (might be due to invalid IL or missing references)
		//IL_033f: Expected O, but got Unknown
		//IL_03cb: Unknown result type (might be due to invalid IL or missing references)
		//IL_03cd: Unknown result type (might be due to invalid IL or missing references)
		//IL_03cf: Unknown result type (might be due to invalid IL or missing references)
		//IL_03c0: Unknown result type (might be due to invalid IL or missing references)
		//IL_03c2: Unknown result type (might be due to invalid IL or missing references)
		//IL_03c4: Unknown result type (might be due to invalid IL or missing references)
		//IL_0408: Unknown result type (might be due to invalid IL or missing references)
		//IL_040a: Unknown result type (might be due to invalid IL or missing references)
		//IL_03e1: Unknown result type (might be due to invalid IL or missing references)
		//IL_0416: Unknown result type (might be due to invalid IL or missing references)
		//IL_0418: Unknown result type (might be due to invalid IL or missing references)
		//IL_0487: Unknown result type (might be due to invalid IL or missing references)
		//IL_048e: Expected O, but got Unknown
		//IL_0498: Unknown result type (might be due to invalid IL or missing references)
		//IL_049f: Expected O, but got Unknown
		//IL_04a9: Unknown result type (might be due to invalid IL or missing references)
		//IL_04b0: Expected O, but got Unknown
		//IL_0505: Unknown result type (might be due to invalid IL or missing references)
		//IL_050c: Expected O, but got Unknown
		//IL_07be: Unknown result type (might be due to invalid IL or missing references)
		//IL_07c3: Unknown result type (might be due to invalid IL or missing references)
		//IL_07e4: Unknown result type (might be due to invalid IL or missing references)
		//IL_07e9: Unknown result type (might be due to invalid IL or missing references)
		//IL_0695: Unknown result type (might be due to invalid IL or missing references)
		//IL_06a9: Expected O, but got Unknown
		//IL_06da: Unknown result type (might be due to invalid IL or missing references)
		//IL_06e1: Expected O, but got Unknown
		//IL_08e3: Unknown result type (might be due to invalid IL or missing references)
		//IL_0905: Unknown result type (might be due to invalid IL or missing references)
		//IL_0849: Unknown result type (might be due to invalid IL or missing references)
		//IL_0850: Expected O, but got Unknown
		//IL_085c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0707: Unknown result type (might be due to invalid IL or missing references)
		//IL_070e: Expected O, but got Unknown
		//IL_0755: Unknown result type (might be due to invalid IL or missing references)
		//IL_075f: Expected O, but got Unknown
		//IL_08c0: Unknown result type (might be due to invalid IL or missing references)
		//IL_08ac: Unknown result type (might be due to invalid IL or missing references)
		//IL_08b3: Expected O, but got Unknown
		outputMetals = new List<OnBrep>();
		outputTransforms = new List<OnXform>();
		emeraldCorner1Point = Point3d.Unset;
		emeraldCorner2Point = Point3d.Unset;
		BoundingBox boundingBox = BoundingBox.Unset;
		centerObject.GetTightBoundingBox(ref boundingBox);
		Point3d val = ((BoundingBox)(ref boundingBox)).Max;
		double x = ((Point3d)(ref val)).X;
		val = ((BoundingBox)(ref boundingBox)).Min;
		double num = Math.Abs(x - ((Point3d)(ref val)).X + offsetX);
		val = ((BoundingBox)(ref boundingBox)).Max;
		double y = ((Point3d)(ref val)).Y;
		val = ((BoundingBox)(ref boundingBox)).Min;
		double num2 = Math.Abs(y - ((Point3d)(ref val)).Y + offsetY);
		string gemShape;
		if (centerGemShapeOverrideEnabled)
		{
			if (centerGemShapeOverride == null)
			{
				centerGemShapeOverride = new CSGemData(centerGem);
			}
			else
			{
				double num3;
				double num4;
				if (centerGem != null)
				{
					switch (centerGemShapeOverrideMode)
					{
					case ValueListItem.GemClusterCenterShapeOverrideModeDimensionX:
						num3 = (centerGem.GemXSize + offsetX) / centerGemShapeOverride.GemXSize;
						num4 = num3;
						break;
					case ValueListItem.GemClusterCenterShapeOverrideModeDimensionY:
						num4 = (centerGem.GemYSize + offsetY) / centerGemShapeOverride.GemYSize;
						num3 = num4;
						break;
					case ValueListItem.GemClusterCenterShapeOverrideModeDimensionLargest:
						if (centerGem.GemXSize > centerGem.GemYSize)
						{
							num3 = (centerGem.GemXSize + offsetX) / centerGemShapeOverride.GemXSize;
							num4 = num3;
						}
						else
						{
							num4 = (centerGem.GemYSize + offsetY) / centerGemShapeOverride.GemYSize;
							num3 = num4;
						}
						break;
					case ValueListItem.GemClusterCenterShapeOverrideModeDimensionSmallest:
						if (centerGem.GemXSize < centerGem.GemYSize)
						{
							num3 = (centerGem.GemXSize + offsetX) / centerGemShapeOverride.GemXSize;
							num4 = num3;
						}
						else
						{
							num4 = (centerGem.GemYSize + offsetY) / centerGemShapeOverride.GemYSize;
							num3 = num4;
						}
						break;
					case ValueListItem.GemClusterCenterShapeOverrideModeDimensionXandY:
						num3 = (centerGem.GemXSize + offsetX) / centerGemShapeOverride.GemXSize;
						num4 = (centerGem.GemYSize + offsetY) / centerGemShapeOverride.GemYSize;
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
						num3 = num / centerGemShapeOverride.GemXSize;
						num4 = num3;
						break;
					case ValueListItem.GemClusterCenterShapeOverrideModeDimensionY:
						num4 = num2 / centerGemShapeOverride.GemYSize;
						num3 = num4;
						break;
					case ValueListItem.GemClusterCenterShapeOverrideModeDimensionLargest:
						if (num > num2)
						{
							num3 = num / centerGemShapeOverride.GemXSize;
							num4 = num3;
						}
						else
						{
							num4 = num2 / centerGemShapeOverride.GemYSize;
							num3 = num4;
						}
						break;
					case ValueListItem.GemClusterCenterShapeOverrideModeDimensionSmallest:
						if (num < num2)
						{
							num3 = num / centerGemShapeOverride.GemXSize;
							num4 = num3;
						}
						else
						{
							num4 = num2 / centerGemShapeOverride.GemYSize;
							num3 = num4;
						}
						break;
					case ValueListItem.GemClusterCenterShapeOverrideModeDimensionXandY:
						num3 = num / centerGemShapeOverride.GemXSize;
						num4 = num2 / centerGemShapeOverride.GemYSize;
						break;
					default:
						outError = "Invalid Gem Cluster Center Shape Override Mode";
						return false;
					}
				}
				centerGemShapeOverride.Scale(num3, num4);
			}
			gemShape = centerGemShapeOverride.GemShape;
		}
		else
		{
			centerGemShapeOverride = new CSGemData(centerGem);
			gemShape = centerGem.GemShape;
		}
		Transform xUp;
		Transform xDown = default(Transform);
		if (centerGem != null)
		{
			if (!CSBlockUtil.CSUtilMath.GetUnscaledXforms(centerGem.Xform, out xUp, out xDown))
			{
				outError = "Gem Xform";
				return false;
			}
		}
		else
		{
			val = ((BoundingBox)(ref boundingBox)).Center;
			double x2 = ((Point3d)(ref val)).X;
			val = ((BoundingBox)(ref boundingBox)).Center;
			double y2 = ((Point3d)(ref val)).Y;
			val = ((BoundingBox)(ref boundingBox)).Center;
			xUp = Transform.Translation(x2, y2, ((Point3d)(ref val)).Z) * Transform.Rotation(RhinoMath.ToRadians(rotate), Vector3d.YAxis, Point3d.Origin);
			if (!((Transform)(ref xUp)).TryGetInverse(ref xDown))
			{
				xDown = Transform.Identity;
			}
		}
		CSGemData cSGemData = null;
		On3dVector val2;
		if (centerGem != null)
		{
			cSGemData = new CSGemData(centerGem);
			cSGemData.ApplyTransform(xDown);
			val2 = new On3dVector((IOn3dVector)(object)cSGemData.Up.Convert());
			val2.Unitize();
		}
		else
		{
			val = ((BoundingBox)(ref boundingBox)).Center;
			double x3 = ((Point3d)(ref val)).X;
			val = ((BoundingBox)(ref boundingBox)).Center;
			Point3d val3 = default(Point3d);
			((Point3d)(ref val3))._002Ector(x3, ((Point3d)(ref val)).Y, 0.0);
			Point3d val4 = val3;
			val = ((BoundingBox)(ref boundingBox)).Center;
			((Point3d)(ref val4)).Transform(Transform.Translation(0.0, 0.0, ((Point3d)(ref val)).Z));
			Vector3d value = default(Vector3d);
			((Vector3d)(ref value))._002Ector((((Point3d)(ref val3)).Z >= ((Point3d)(ref val4)).Z) ? (val3 - val4) : (val4 - val3));
			((Vector3d)(ref value)).Unitize();
			val2 = CSConvert.Vector3dCopyToOn3dVector(value);
		}
		double optionalEmeraldCornerXPercent = 0.0;
		double optionalEmeraldCornerYPercent = 0.0;
		if (inputCorner1 != Point3d.Unset && inputCorner2 != Point3d.Unset)
		{
			double num5 = centerGemShapeOverride.GemXSize / 2.0;
			optionalEmeraldCornerXPercent = ((Point3d)(ref inputCorner1)).X / num5;
			double num6 = centerGemShapeOverride.GemYSize / 2.0;
			optionalEmeraldCornerYPercent = ((Point3d)(ref inputCorner2)).Y / num6;
		}
		if (!GemOffsetCurve.GemOffset(gemShape, centerGemShapeOverride, centerGemSpacing, out OnCurve curve, doTransformUp: false, out string error, blendCornerInside, optionalEmeraldCornerXPercent, optionalEmeraldCornerYPercent))
		{
			outError = "CurveTop Inside:" + error;
			return false;
		}
		OnNurbsCurve val5 = new OnNurbsCurve();
		curve.GetNurbForm(ref val5);
		OnNurbsCurve val6 = new OnNurbsCurve();
		channelProfile.GetNurbForm(ref val6);
		OnNurbsSurface val7 = new OnNurbsSurface();
		val7.Create(3, true, val6.m_order, val5.m_order, val6.m_cv_count, val5.m_cv_count);
		bool flag = false;
		List<IOnCurve> list = new List<IOnCurve>();
		CSGemData gem = (doScaleToGemXY ? cSGemData : centerGemShapeOverride);
		double y3 = curve.PointAtStart().y;
		OnCurve val8 = curve;
		for (int i = 0; i < val6.CVCount(); i++)
		{
			On3dPoint val9 = new On3dPoint();
			val6.GetCV(i, ref val9);
			double num7 = (double)i / (double)(val6.CVCount() - 1);
			if (i <= val6.CVCount() / 10)
			{
				double num8 = 2.0 * (double)val6.CVCount() / 10.0;
				if (!GemOffsetCurve.GemOffset(gemShape, gem, centerGemSpacing + val9.x, out curve, doTransformUp: false, out error, blendCornerInside * (0.5 + (double)i / num8) + blendCornerOutside * (1.0 - (0.5 + (double)i / num8)), optionalEmeraldCornerXPercent, optionalEmeraldCornerYPercent))
				{
					outError = "CurveTop Inside:" + error;
					return false;
				}
			}
			else if (i <= val6.CVCount() / 2)
			{
				if (!GemOffsetCurve.GemOffset(gemShape, gem, centerGemSpacing + val9.x, out curve, doTransformUp: false, out error, blendCornerInside, optionalEmeraldCornerXPercent, optionalEmeraldCornerYPercent))
				{
					outError = "CurveTop Inside:" + error;
					return false;
				}
			}
			else if (i < 9 * val6.CVCount() / 10)
			{
				if (!GemOffsetCurve.GemOffset(gemShape, gem, centerGemSpacing + val9.x, out curve, doTransformUp: false, out error, blendCornerOutside, optionalEmeraldCornerXPercent, optionalEmeraldCornerYPercent))
				{
					outError = "CurveTop Inside:" + error;
					return false;
				}
			}
			else if (!GemOffsetCurve.GemOffset(gemShape, gem, centerGemSpacing + val9.x, out curve, doTransformUp: false, out error, blendCornerInside * num7 * 0.5 + blendCornerOutside * (1.0 - num7 * 0.5), optionalEmeraldCornerXPercent, optionalEmeraldCornerYPercent))
			{
				outError = "CurveTop Inside:" + error;
				return false;
			}
			((OnGeometry)curve).Translate((IOn3dVector)(object)(new On3dVector((IOn3dVector)(object)val2) * (placement + val9.y)));
			list.Add((IOnCurve)(object)curve);
			if (curve.PointAtStart().y > y3)
			{
				y3 = curve.PointAtStart().y;
				val8 = curve;
			}
			OnNurbsCurve val10 = new OnNurbsCurve();
			curve.GetNurbForm(ref val10);
			if (val5.CVCount() != val10.CVCount())
			{
				flag = true;
			}
			if (!flag)
			{
				for (int j = 0; j < val5.CVCount(); j++)
				{
					On3dPoint val11 = new On3dPoint();
					val10.GetCV(j, ref val11);
					double num9 = val6.Weight(i) * val10.Weight(j);
					val7.SetCV(i, j, (IOn4dPoint)new On4dPoint(val11.x * num9, val11.y * num9, val11.z * num9, num9));
				}
			}
		}
		if (gemShape == "Emerald")
		{
			Curve val12 = Interop.FromOnCurve((object)val8);
			double num10 = default(double);
			val12.GetNextDiscontinuity((Continuity)3, 0.0, 10.0, ref num10);
			emeraldCorner1Point = val12.PointAt(num10);
			val12.GetNextDiscontinuity((Continuity)3, num10, 20.0, ref num10);
			emeraldCorner2Point = val12.PointAt(num10);
		}
		if (!flag)
		{
			for (int k = 0; k < val6.KnotCount(); k++)
			{
				val7.SetKnot(0, k, val6.Knot(k));
			}
			for (int l = 0; l < val5.KnotCount(); l++)
			{
				val7.SetKnot(1, l, val5.Knot(l));
			}
			OnBrep brep = new OnBrep();
			((OnGeometry)val7).BrepForm(brep);
			((OnGeometry)brep).Transform((IOnXform)(object)xUp.Convert());
			CSUtilGeometry.FixBrepSolidOrientation(ref brep);
			if (!brep.IsSolid())
			{
				Brep val13 = brep.Convert();
				val13.JoinNakedEdges(CSApplicationHelper.Application.Tolerance);
				if (!val13.IsSolid)
				{
					val13.Flip();
				}
				brep = (OnBrep)Interop.ToOnBrep(val13);
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
