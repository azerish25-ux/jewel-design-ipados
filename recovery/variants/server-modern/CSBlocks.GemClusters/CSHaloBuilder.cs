using System;
using System.Collections.Generic;
using System.Linq;
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

namespace CSBlocks.GemClusters;

[CSBlock("HaloBuilder", "{03EB6BEE-9039-41D6-ABC5-32B3E789A9FB}")]
public sealed class CSHaloBuilder : CSBlock
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

	public CSInputChannel InChannelCornerBlendRadius { get; }

	public CSInputChannel InGemSizeZ { get; }

	public CSInputChannel InShowChannel { get; }

	public CSInputChannel InXYScaleToGem { get; }

	public CSInputChannel InCenterGemShapeOverrideMode { get; }

	public CSInputChannel InOffsetFix { get; }

	public CSInputChannel InChannelWallThicknessExtraInner { get; }

	public CSInputChannel InChannelWallThicknessExtraOuter { get; }

	public CSInputChannel InRailX { get; }

	public CSInputChannel InRailZ { get; }

	public CSInputChannel InChannelBottomOffset { get; }

	public CSInputChannel InMetalToggle { get; }

	public CSInputChannel InOffsetX { get; }

	public CSInputChannel InOffsetY { get; }

	public CSInputChannel InEmeraldCornerPoint1 { get; }

	public CSInputChannel InEmeraldCornerPoint2 { get; }

	public CSInputChannel InRotate { get; }

	public CSInputChannel InGemSizeToggle { get; }

	public CSInputChannel InGemSize { get; }

	public CSInputChannel InChannelSpacing { get; }

	public CSInputChannel InForceChannel { get; }

	public CSInputChannel InCenterGemShapeCornerBlendEnabled { get; }

	public CSInputChannel InCenterGemShapeCornerBlendRadiusInner { get; }

	public CSInputChannel InCenterGemShapeCornerBlendRadiusOuter { get; }

	public CSInputChannel InCenterGemShapeCornerBlendOffsetPercentage { get; }

	public CSInputChannel InDepthFromGems { get; }

	public CSInputChannel InGemPlacement { get; }

	public CSInputChannel InMinCuletUp { get; }

	public CSInputChannel InMinCuletSmallerFlag { get; }

	public CSInputChannel InUseMinCulet { get; }

	public CSOutputChannel OutMetal { get; }

	public CSOutputChannel OutEmeraldCorner1Point { get; }

	public CSOutputChannel OutEmeraldCorner2Point { get; }

	public CSHaloBuilder(string identifier)
		: base(identifier)
	{
		//IL_023e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0252: Unknown result type (might be due to invalid IL or missing references)
		InCenterGem = new CSInputChannel(this, ChannelDataType.Any, ChannelArrayType.Array);
		InCenterGemShapeOverrideEnabled = new CSInputChannel(this, ChannelDataType.Boolean, ChannelArrayType.Individual, defaultValue: false);
		InCenterGemShapeOverride = new CSInputChannel(this, ChannelDataType.Gem, ChannelArrayType.Individual);
		InCenterGemShapeOverrideMode = new CSInputChannel(this, ChannelDataType.Integer, ChannelArrayType.Individual, ValueListItem.GemClusterCenterShapeOverrideModeDimensionSmallest);
		InCenterGemSpacing = new CSInputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual, 0.0);
		InProfile = new CSInputChannel(this, ChannelDataType.Curve, ChannelArrayType.Individual);
		InProfile2 = new CSInputChannel(this, ChannelDataType.Curve, ChannelArrayType.Individual);
		InProfileWidth = new CSInputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual, 0.0);
		InProfileHeight = new CSInputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual, 0.0);
		InChannelCornerBlendRadius = new CSInputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual, 0.0);
		InChannelBottomOffset = new CSInputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual, 0.0);
		InChannelSpacing = new CSInputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual, 0.0);
		InChannelWallThicknessInner = new CSInputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual, 0.0);
		InChannelWallThicknessOuter = new CSInputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual, 0.0);
		InChannelWallDepth = new CSInputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual, 0.0);
		InAngle = new CSInputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual, 0.0);
		InPlacement = new CSInputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual, 0.0);
		InShowChannel = new CSInputChannel(this, ChannelDataType.Boolean, ChannelArrayType.Individual, defaultValue: true);
		InXYScaleToGem = new CSInputChannel(this, ChannelDataType.Boolean, ChannelArrayType.Individual, defaultValue: false);
		InOffsetFix = new CSInputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual, 0.0);
		InChannelWallThicknessExtraInner = new CSInputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual, 0.0);
		InChannelWallThicknessExtraOuter = new CSInputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual, 0.0);
		InRailX = new CSInputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual, 0.0);
		InRailZ = new CSInputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual, 0.0);
		InMetalToggle = new CSInputChannel(this, ChannelDataType.Boolean, ChannelArrayType.Individual, defaultValue: true);
		InOffsetX = new CSInputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual, 0.0);
		InOffsetY = new CSInputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual, 0.0);
		InEmeraldCornerPoint1 = new CSInputChannel(this, ChannelDataType.Point, ChannelArrayType.Individual, Point3d.Unset);
		InEmeraldCornerPoint2 = new CSInputChannel(this, ChannelDataType.Point, ChannelArrayType.Individual, Point3d.Unset);
		InRotate = new CSInputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual, 0.0);
		InGemSizeToggle = new CSInputChannel(this, ChannelDataType.Boolean, ChannelArrayType.Individual, defaultValue: false);
		InGemSize = new CSInputChannel(this, ChannelDataType.Gem, ChannelArrayType.Individual);
		InGemSizeZ = new CSInputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual, 0.0);
		InForceChannel = new CSInputChannel(this, ChannelDataType.Boolean, ChannelArrayType.Individual, defaultValue: false);
		InCenterGemShapeCornerBlendEnabled = new CSInputChannel(this, ChannelDataType.Boolean, ChannelArrayType.Individual, defaultValue: false);
		InCenterGemShapeCornerBlendRadiusInner = new CSInputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual, 0.0);
		InCenterGemShapeCornerBlendRadiusOuter = new CSInputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual, 0.0);
		InCenterGemShapeCornerBlendOffsetPercentage = new CSInputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual, 0.0);
		InDepthFromGems = new CSInputChannel(this, ChannelDataType.Boolean, ChannelArrayType.Individual, defaultValue: false);
		InGemPlacement = new CSInputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual, 0.0);
		InMinCuletUp = new CSInputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual, 0.0);
		InMinCuletSmallerFlag = new CSInputChannel(this, ChannelDataType.Boolean, ChannelArrayType.Individual, defaultValue: false);
		InUseMinCulet = new CSInputChannel(this, ChannelDataType.Integer, ChannelArrayType.Individual, ValueListItem.NoMinCulet);
		OutMetal = new CSOutputChannel(this, ChannelDataType.Any, ChannelArrayType.Array);
		OutEmeraldCorner1Point = new CSOutputChannel(this, ChannelDataType.Point, ChannelArrayType.Individual);
		OutEmeraldCorner2Point = new CSOutputChannel(this, ChannelDataType.Point, ChannelArrayType.Individual);
		SetChannelIdentifiers(autoCheckInputs: true);
	}

	public override bool Update()
	{
		//IL_0246: Unknown result type (might be due to invalid IL or missing references)
		//IL_024b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0253: Unknown result type (might be due to invalid IL or missing references)
		//IL_0258: Unknown result type (might be due to invalid IL or missing references)
		//IL_0332: Unknown result type (might be due to invalid IL or missing references)
		//IL_0334: Unknown result type (might be due to invalid IL or missing references)
		//IL_0358: Unknown result type (might be due to invalid IL or missing references)
		//IL_036a: Unknown result type (might be due to invalid IL or missing references)
		//IL_039b: Unknown result type (might be due to invalid IL or missing references)
		if (!InMetalToggle.Boolean)
		{
			OutMetal.SetData(null);
			return base.Update();
		}
		CSDataArray flattenArray = InCenterGem.GetFlattenArray();
		if (flattenArray.GetArrayCount() <= 0)
		{
			return UpdateMissing("CenterGem");
		}
		CSGemData cSGemData = flattenArray.GetArrayItem(0) as CSGemData;
		CSData arrayItem = flattenArray.GetArrayItem(0);
		bool centerGemShapeOverrideEnabled = InCenterGemShapeOverrideEnabled.Boolean || cSGemData == null;
		CSGemData gem = InCenterGemShapeOverride.Gem;
		ValueListItem valueListItem = InCenterGemShapeOverrideMode.ValueListItem;
		double num = InCenterGemSpacing.Double;
		double profileWidth = InProfileWidth.Double;
		double profileHeight = InProfileHeight.Double;
		double channelBottomOffset = InChannelBottomOffset.Double;
		double channelCornerBlendRadius = InChannelCornerBlendRadius.Double;
		double channelWallThicknessInner = InChannelWallThicknessInner.Double;
		double channelWallThicknessOuter = InChannelWallThicknessOuter.Double;
		double channelWallDepth = InChannelWallDepth.Double;
		double angleDegrees = InAngle.Double;
		angleDegrees = CSBlockUtil.CSUtilMath.ToRadians(angleDegrees);
		double num2 = InPlacement.Double;
		double gemSizeZ = InGemSizeZ.Double;
		double centerGemShapeCornerBlendRadiusInner = InCenterGemShapeCornerBlendRadiusInner.Double;
		double centerGemShapeCornerBlendRadiusOuter = InCenterGemShapeCornerBlendRadiusOuter.Double;
		double centerGemShapeCornerBlendOffsetPercentage = InCenterGemShapeCornerBlendOffsetPercentage.Double;
		bool boolean = InCenterGemShapeCornerBlendEnabled.Boolean;
		if (!boolean)
		{
			centerGemShapeCornerBlendRadiusInner = 0.0;
			centerGemShapeCornerBlendRadiusOuter = 0.0;
			centerGemShapeCornerBlendOffsetPercentage = 0.0;
		}
		double num3 = InOffsetFix.Double;
		num += num3;
		bool flag = InShowChannel.Boolean;
		if (InForceChannel.Boolean)
		{
			flag = true;
		}
		bool boolean2 = InXYScaleToGem.Boolean;
		double channelWallThicknessExtraInner = InChannelWallThicknessExtraInner.Double;
		double channelWallThicknessExtraOuter = InChannelWallThicknessExtraOuter.Double;
		OnNurbsCurve profile = (flag ? InProfile2.NurbsCurve.Convert().Duplicate() : InProfile.NurbsCurve.Convert().Duplicate());
		double num4 = InRailX.Double;
		num += num4;
		double num5 = InRailZ.Double;
		num2 += num5;
		double offsetX = InOffsetX.Double;
		double offsetY = InOffsetY.Double;
		Point3d point3d = InEmeraldCornerPoint1.Point3d;
		Point3d point3d2 = InEmeraldCornerPoint2.Point3d;
		double rotate = InRotate.Double;
		bool boolean3 = InGemSizeToggle.Boolean;
		CSGemData gem2 = InGemSize.Gem;
		double channelSpacing = InChannelSpacing.Double;
		bool boolean4 = InDepthFromGems.Boolean;
		double gemPlacement = InGemPlacement.Double;
		double minCuletUp = InMinCuletUp.Double;
		bool boolean5 = InMinCuletSmallerFlag.Boolean;
		ValueListItem valueListItem2 = InUseMinCulet.ValueListItem;
		if (!MakeHaloProfile(profile, profileWidth, profileHeight, channelWallThicknessInner, channelWallThicknessOuter, channelWallDepth, angleDegrees, channelCornerBlendRadius, gemSizeZ, flag, channelWallThicknessExtraInner, channelWallThicknessExtraOuter, channelBottomOffset, boolean3, gem2, channelSpacing, boolean4, gemPlacement, minCuletUp, boolean5, valueListItem2, out var haloProfile, out var haloProfileChannelWallThicknessExtraInner, out var haloProfileChannelWallThicknessInner, out var haloProfileInnerX, out var outError))
		{
			return UpdateError(outError);
		}
		if (!MakeHalo(cSGemData, arrayItem, centerGemShapeOverrideEnabled, gem, num, (IOnCurve)(object)haloProfile, haloProfileChannelWallThicknessExtraInner, haloProfileChannelWallThicknessInner, haloProfileInnerX, num2, boolean2, valueListItem, boolean, centerGemShapeCornerBlendRadiusInner, centerGemShapeCornerBlendRadiusOuter, centerGemShapeCornerBlendOffsetPercentage, offsetX, offsetY, point3d, point3d2, rotate, out var outputMetals, out var outputTransforms, out var emeraldCorner1Point, out var emeraldCorner2Point, out outError))
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

	private static bool GetLargestIntersectionY(IOnCurve c1, IOnCurve c2, ref double param)
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
		double num = double.MinValue;
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

	private static bool MakeHalo(CSGemData centerGem, CSData centerObject, bool centerGemShapeOverrideEnabled, CSGemData centerGemShapeOverride, double centerGemSpacing, IOnCurve haloProfile, double haloProfileChannelWallThicknessExtraInner, double haloProfileChannelWallThicknessInner, double haloProfileInnerX, double placement, bool doScaleToGemXY, ValueListItem centerGemShapeOverrideMode, bool isCenterGemShapeCornerBlendEnabled, double centerGemShapeCornerBlendRadiusInner, double centerGemShapeCornerBlendRadiusOuter, double centerGemShapeCornerBlendOffsetPercentage, double offsetX, double offsetY, Point3d inputCorner1, Point3d inputCorner2, double rotate, out List<OnBrep> outputMetals, out List<OnXform> outputTransforms, out Point3d emeraldCorner1Point, out Point3d emeraldCorner2Point, out string outError)
	{
		//IL_0012: Unknown result type (might be due to invalid IL or missing references)
		//IL_0017: Unknown result type (might be due to invalid IL or missing references)
		//IL_001e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0023: Unknown result type (might be due to invalid IL or missing references)
		//IL_0030: Unknown result type (might be due to invalid IL or missing references)
		//IL_0035: Unknown result type (might be due to invalid IL or missing references)
		//IL_0041: Unknown result type (might be due to invalid IL or missing references)
		//IL_0046: Unknown result type (might be due to invalid IL or missing references)
		//IL_0051: Unknown result type (might be due to invalid IL or missing references)
		//IL_0056: Unknown result type (might be due to invalid IL or missing references)
		//IL_006b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0070: Unknown result type (might be due to invalid IL or missing references)
		//IL_007b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0080: Unknown result type (might be due to invalid IL or missing references)
		//IL_02ba: Unknown result type (might be due to invalid IL or missing references)
		//IL_02bf: Unknown result type (might be due to invalid IL or missing references)
		//IL_02ca: Unknown result type (might be due to invalid IL or missing references)
		//IL_02cf: Unknown result type (might be due to invalid IL or missing references)
		//IL_02da: Unknown result type (might be due to invalid IL or missing references)
		//IL_02df: Unknown result type (might be due to invalid IL or missing references)
		//IL_02e8: Unknown result type (might be due to invalid IL or missing references)
		//IL_02f4: Unknown result type (might be due to invalid IL or missing references)
		//IL_02f9: Unknown result type (might be due to invalid IL or missing references)
		//IL_02fe: Unknown result type (might be due to invalid IL or missing references)
		//IL_0303: Unknown result type (might be due to invalid IL or missing references)
		//IL_0308: Unknown result type (might be due to invalid IL or missing references)
		//IL_029e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0315: Unknown result type (might be due to invalid IL or missing references)
		//IL_031a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0358: Unknown result type (might be due to invalid IL or missing references)
		//IL_035d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0368: Unknown result type (might be due to invalid IL or missing references)
		//IL_036d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0384: Unknown result type (might be due to invalid IL or missing references)
		//IL_0386: Unknown result type (might be due to invalid IL or missing references)
		//IL_039e: Unknown result type (might be due to invalid IL or missing references)
		//IL_03a3: Unknown result type (might be due to invalid IL or missing references)
		//IL_03ac: Unknown result type (might be due to invalid IL or missing references)
		//IL_032c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0336: Unknown result type (might be due to invalid IL or missing references)
		//IL_0340: Unknown result type (might be due to invalid IL or missing references)
		//IL_0347: Expected O, but got Unknown
		//IL_03d3: Unknown result type (might be due to invalid IL or missing references)
		//IL_03d5: Unknown result type (might be due to invalid IL or missing references)
		//IL_03d7: Unknown result type (might be due to invalid IL or missing references)
		//IL_03c8: Unknown result type (might be due to invalid IL or missing references)
		//IL_03ca: Unknown result type (might be due to invalid IL or missing references)
		//IL_03cc: Unknown result type (might be due to invalid IL or missing references)
		//IL_0408: Unknown result type (might be due to invalid IL or missing references)
		//IL_040a: Unknown result type (might be due to invalid IL or missing references)
		//IL_03e9: Unknown result type (might be due to invalid IL or missing references)
		//IL_0458: Unknown result type (might be due to invalid IL or missing references)
		//IL_045f: Expected O, but got Unknown
		//IL_0416: Unknown result type (might be due to invalid IL or missing references)
		//IL_0418: Unknown result type (might be due to invalid IL or missing references)
		//IL_0475: Unknown result type (might be due to invalid IL or missing references)
		//IL_047c: Expected O, but got Unknown
		//IL_0512: Unknown result type (might be due to invalid IL or missing references)
		//IL_0519: Expected O, but got Unknown
		//IL_0523: Unknown result type (might be due to invalid IL or missing references)
		//IL_052a: Expected O, but got Unknown
		//IL_074e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0753: Unknown result type (might be due to invalid IL or missing references)
		//IL_0774: Unknown result type (might be due to invalid IL or missing references)
		//IL_0779: Unknown result type (might be due to invalid IL or missing references)
		//IL_0625: Unknown result type (might be due to invalid IL or missing references)
		//IL_0639: Expected O, but got Unknown
		//IL_0873: Unknown result type (might be due to invalid IL or missing references)
		//IL_0895: Unknown result type (might be due to invalid IL or missing references)
		//IL_066a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0671: Expected O, but got Unknown
		//IL_07d9: Unknown result type (might be due to invalid IL or missing references)
		//IL_07e0: Expected O, but got Unknown
		//IL_07ec: Unknown result type (might be due to invalid IL or missing references)
		//IL_0850: Unknown result type (might be due to invalid IL or missing references)
		//IL_0697: Unknown result type (might be due to invalid IL or missing references)
		//IL_069e: Expected O, but got Unknown
		//IL_06e5: Unknown result type (might be due to invalid IL or missing references)
		//IL_06ef: Expected O, but got Unknown
		//IL_083c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0843: Expected O, but got Unknown
		outputMetals = new List<OnBrep>();
		outputTransforms = new List<OnXform>();
		emeraldCorner1Point = Point3d.Unset;
		emeraldCorner2Point = Point3d.Unset;
		outError = string.Empty;
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
			optionalEmeraldCornerXPercent = ((Point3d)(ref inputCorner1)).X / (centerGemShapeOverride.GemXSize / 2.0);
			optionalEmeraldCornerYPercent = ((Point3d)(ref inputCorner2)).Y / (centerGemShapeOverride.GemYSize / 2.0);
		}
		OnNurbsCurve val5 = new OnNurbsCurve();
		haloProfile.GetNurbForm(ref val5);
		List<On3dPoint> list = new List<On3dPoint>();
		for (int i = 0; i < val5.CVCount(); i++)
		{
			On3dPoint item = new On3dPoint();
			if (!val5.GetCV(i, ref item))
			{
				outError = "GetCV failed on haloProfileNc";
				return false;
			}
			list.Add(item);
		}
		double distance = centerGemSpacing + list.Min((On3dPoint _) => _.x);
		double cornerBlend = (isCenterGemShapeCornerBlendEnabled ? centerGemShapeCornerBlendRadiusInner : 0.0);
		if (!GemOffsetCurve.GemOffset(gemShape, centerGemShapeOverride, distance, out OnCurve curve, false, out string error, cornerBlend, optionalEmeraldCornerXPercent, optionalEmeraldCornerYPercent))
		{
			outError = "CurveTop Inside:" + error;
			return false;
		}
		OnNurbsCurve val6 = new OnNurbsCurve();
		curve.GetNurbForm(ref val6);
		OnNurbsSurface val7 = new OnNurbsSurface();
		val7.Create(3, true, val5.m_order, val6.m_order, val5.m_cv_count, val6.m_cv_count);
		bool flag = false;
		List<IOnCurve> list2 = new List<IOnCurve>();
		CSGemData gem = (doScaleToGemXY ? cSGemData : centerGemShapeOverride);
		double y3 = curve.PointAtStart().y;
		OnCurve val8 = curve;
		for (int num5 = 0; num5 < list.Count; num5++)
		{
			On3dPoint val9 = list[num5];
			double distance2 = centerGemSpacing + val9.x;
			double cornerBlend2;
			if (isCenterGemShapeCornerBlendEnabled)
			{
				double num6 = haloProfileChannelWallThicknessExtraInner + haloProfileChannelWallThicknessInner;
				double num7 = val9.x - haloProfileInnerX;
				if (num7 < num6)
				{
					double num8 = num7 / num6;
					cornerBlend2 = centerGemShapeCornerBlendRadiusInner + (centerGemShapeCornerBlendRadiusOuter - centerGemShapeCornerBlendRadiusInner) * num8;
				}
				else
				{
					double num9 = centerGemShapeCornerBlendRadiusOuter + (num7 - num6);
					cornerBlend2 = centerGemShapeCornerBlendRadiusOuter + (num9 - centerGemShapeCornerBlendRadiusOuter) * centerGemShapeCornerBlendOffsetPercentage;
				}
			}
			else
			{
				cornerBlend2 = 0.0;
			}
			if (!GemOffsetCurve.GemOffset(gemShape, gem, distance2, out curve, false, out error, cornerBlend2, optionalEmeraldCornerXPercent, optionalEmeraldCornerYPercent))
			{
				outError = "CurveTop Inside:" + error;
				return false;
			}
			((OnGeometry)curve).Translate((IOn3dVector)(object)(new On3dVector((IOn3dVector)(object)val2) * (placement + val9.y)));
			list2.Add((IOnCurve)(object)curve);
			if (curve.PointAtStart().y > y3)
			{
				y3 = curve.PointAtStart().y;
				val8 = curve;
			}
			OnNurbsCurve val10 = new OnNurbsCurve();
			curve.GetNurbForm(ref val10);
			if (val6.CVCount() != val10.CVCount())
			{
				flag = true;
			}
			if (!flag)
			{
				for (int num10 = 0; num10 < val6.CVCount(); num10++)
				{
					On3dPoint val11 = new On3dPoint();
					val10.GetCV(num10, ref val11);
					double num11 = val5.Weight(num5) * val10.Weight(num10);
					val7.SetCV(num5, num10, (IOn4dPoint)new On4dPoint(val11.x * num11, val11.y * num11, val11.z * num11, num11));
				}
			}
		}
		if (gemShape == "Emerald")
		{
			Curve val12 = Interop.FromOnCurve((object)val8);
			double num12 = default(double);
			val12.GetNextDiscontinuity((Continuity)3, 0.0, 10.0, ref num12);
			emeraldCorner1Point = val12.PointAt(num12);
			val12.GetNextDiscontinuity((Continuity)3, num12, 20.0, ref num12);
			emeraldCorner2Point = val12.PointAt(num12);
		}
		if (!flag)
		{
			for (int num13 = 0; num13 < val5.KnotCount(); num13++)
			{
				val7.SetKnot(0, num13, val5.Knot(num13));
			}
			for (int num14 = 0; num14 < val6.KnotCount(); num14++)
			{
				val7.SetKnot(1, num14, val6.Knot(num14));
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
			OnBrep brep2 = CSUtilSweep.Loft(list2.ToArray(), (eLoftType)1);
			if (brep2 != null)
			{
				((OnGeometry)brep2).Transform((IOnXform)(object)xUp.Convert());
				CSUtilGeometry.FixBrepSolidOrientation(ref brep2);
				outputMetals.Add(brep2);
				outputTransforms.Add(xUp.Convert());
			}
		}
		return true;
	}

	private static bool MakeHaloProfile(OnNurbsCurve profile, double profileWidth, double profileHeight, double channelWallThicknessInner, double channelWallThicknessOuter, double channelWallDepth, double angle, double channelCornerBlendRadius, double gemSizeZ, bool doShowChannel, double channelWallThicknessExtraInner, double channelWallThicknessExtraOuter, double channelBottomOffset, bool gemSizeToggle, CSGemData gemSize, double channelSpacing, bool depthFromGems, double gemPlacement, double minCuletUp, bool minCuletSmaller, ValueListItem useMinCulet, out OnCurve haloProfile, out double haloProfileChannelWallThicknessExtraInner, out double haloProfileChannelWallThicknessInner, out double haloProfileInnerX, out string outError)
	{
		//IL_004b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0051: Expected O, but got Unknown
		//IL_0065: Unknown result type (might be due to invalid IL or missing references)
		//IL_006b: Expected O, but got Unknown
		//IL_0092: Unknown result type (might be due to invalid IL or missing references)
		//IL_0098: Expected O, but got Unknown
		//IL_00e9: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ef: Expected O, but got Unknown
		//IL_012a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0131: Expected O, but got Unknown
		//IL_017b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0193: Unknown result type (might be due to invalid IL or missing references)
		//IL_019d: Expected O, but got Unknown
		//IL_019d: Expected O, but got Unknown
		//IL_0198: Unknown result type (might be due to invalid IL or missing references)
		//IL_019f: Expected O, but got Unknown
		//IL_01b5: Unknown result type (might be due to invalid IL or missing references)
		//IL_01d0: Unknown result type (might be due to invalid IL or missing references)
		//IL_01da: Expected O, but got Unknown
		//IL_01da: Expected O, but got Unknown
		//IL_01d5: Unknown result type (might be due to invalid IL or missing references)
		//IL_01dc: Expected O, but got Unknown
		//IL_0254: Unknown result type (might be due to invalid IL or missing references)
		//IL_025b: Expected O, but got Unknown
		//IL_02ba: Unknown result type (might be due to invalid IL or missing references)
		//IL_02c4: Expected O, but got Unknown
		//IL_02de: Unknown result type (might be due to invalid IL or missing references)
		//IL_02e8: Expected O, but got Unknown
		//IL_0610: Unknown result type (might be due to invalid IL or missing references)
		//IL_0617: Expected O, but got Unknown
		//IL_0648: Unknown result type (might be due to invalid IL or missing references)
		//IL_064f: Expected O, but got Unknown
		//IL_066a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0671: Expected O, but got Unknown
		//IL_06ae: Unknown result type (might be due to invalid IL or missing references)
		//IL_06b5: Expected O, but got Unknown
		//IL_06da: Unknown result type (might be due to invalid IL or missing references)
		//IL_06e1: Expected O, but got Unknown
		//IL_06f1: Unknown result type (might be due to invalid IL or missing references)
		//IL_06f8: Expected O, but got Unknown
		//IL_037a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0381: Expected O, but got Unknown
		//IL_0501: Unknown result type (might be due to invalid IL or missing references)
		//IL_0508: Expected O, but got Unknown
		//IL_0511: Unknown result type (might be due to invalid IL or missing references)
		//IL_0518: Expected O, but got Unknown
		//IL_03b0: Unknown result type (might be due to invalid IL or missing references)
		//IL_03ba: Expected O, but got Unknown
		//IL_03b5: Unknown result type (might be due to invalid IL or missing references)
		//IL_03bc: Expected O, but got Unknown
		//IL_03d6: Unknown result type (might be due to invalid IL or missing references)
		//IL_03e2: Expected O, but got Unknown
		//IL_03dd: Unknown result type (might be due to invalid IL or missing references)
		//IL_03e4: Expected O, but got Unknown
		//IL_043b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0445: Expected O, but got Unknown
		//IL_0440: Unknown result type (might be due to invalid IL or missing references)
		//IL_0447: Expected O, but got Unknown
		//IL_0461: Unknown result type (might be due to invalid IL or missing references)
		//IL_046d: Expected O, but got Unknown
		//IL_0468: Unknown result type (might be due to invalid IL or missing references)
		//IL_046f: Expected O, but got Unknown
		haloProfile = null;
		haloProfileChannelWallThicknessExtraInner = -1.23432101234321E+308;
		haloProfileChannelWallThicknessInner = -1.23432101234321E+308;
		haloProfileInnerX = -1.23432101234321E+308;
		outError = null;
		if (profile == null)
		{
			outError = "Missing profile";
			return false;
		}
		if (!((OnObject)profile).IsValid())
		{
			outError = "Invalid profile";
			return false;
		}
		OnBoundingBox val = new OnBoundingBox();
		if (!((OnGeometry)profile).GetTightBoundingBox(ref val))
		{
			outError = "Unable to get tight bounding box from profile";
			return false;
		}
		OnXform val2 = new OnXform();
		val2.Translation(0.0 - val.Min().x, 0.0 - val.Min().y, 0.0);
		OnXform val3 = new OnXform();
		val3.Scale(1.0 / (val.Max().x - val.Min().x), 1.0 / (val.Max().y - val.Min().y), 0.0);
		OnXform val4 = new OnXform();
		if (gemSize != null && (gemSizeToggle || minCuletSmaller))
		{
			profileWidth = gemSize.GemXSize + channelSpacing + channelWallThicknessInner + channelWallThicknessOuter;
		}
		val4.Scale(profileWidth + channelWallThicknessExtraInner + channelWallThicknessExtraOuter, 1.0, 1.0);
		OnXform val5 = new OnXform();
		val5.Translation(0.0 - channelWallThicknessExtraInner, 0.0, 0.0);
		profile.Transform((IOnXform)(object)(val5 * (IOnXform)(object)val4 * (IOnXform)(object)val3 * (IOnXform)(object)val2));
		OnLineCurve c = new OnLineCurve((IOn3dPoint)new On3dPoint(channelWallThicknessInner, -1.0, 0.0), (IOn3dPoint)new On3dPoint(channelWallThicknessInner, 2.0, 0.0));
		OnLineCurve c2 = new OnLineCurve((IOn3dPoint)new On3dPoint(profileWidth - channelWallThicknessOuter, -1.0, 0.0), (IOn3dPoint)new On3dPoint(profileWidth - channelWallThicknessOuter, 2.0, 0.0));
		double param = 0.0;
		double param2 = 0.0;
		if (!GetLargestIntersectionY((IOnCurve)(object)profile, (IOnCurve)(object)c, ref param))
		{
			outError = "Intersect for channel inner failed";
			return false;
		}
		if (!GetLargestIntersectionY((IOnCurve)(object)profile, (IOnCurve)(object)c2, ref param2))
		{
			outError = "Intersect for channel outer failed";
			return false;
		}
		On3dPoint val6 = ((OnCurve)profile).PointAt(param);
		On3dPoint val7 = ((OnCurve)profile).PointAt(param2);
		double num = ((val6.y > val7.y) ? val6.y : val7.y);
		OnXform val8 = new OnXform();
		val8.Scale(1.0, (profileHeight + ((useMinCulet == ValueListItem.MinCuletShankGrowUp) ? minCuletUp : 0.0)) / num, 1.0);
		OnNurbsCurve val9 = profile.Duplicate();
		val9.Transform((IOnXform)(object)val8);
		OnCurve val10 = (OnCurve)(object)val9.Duplicate();
		val10.Trim((IOnInterval)new OnInterval(((OnCurve)val9).Domain().Min(), param));
		OnCurve val11 = (OnCurve)(object)val9.Duplicate();
		val11.Trim((IOnInterval)new OnInterval(param2, ((OnCurve)val9).Domain().Max()));
		On3dPoint val12 = val10.PointAtEnd();
		On3dPoint val13 = val11.PointAtStart();
		if (doShowChannel)
		{
			double num2 = channelWallThicknessInner + (profileWidth - channelWallThicknessInner - channelWallThicknessOuter) / 2.0;
			double num3 = profileHeight - gemSizeZ + channelBottomOffset + minCuletUp;
			if (depthFromGems)
			{
				num3 += gemPlacement;
			}
			bool num4 = num3 > val10.PointAtEnd().y - channelWallDepth;
			bool flag = 0.0 > num3;
			if (num4)
			{
				num3 = val10.PointAtEnd().y - channelWallDepth;
			}
			if (flag)
			{
				num3 = 0.0;
			}
			On3dPoint val14 = new On3dPoint(num2, num3, 0.0);
			OnCurve val21;
			OnCurve val22;
			if (channelWallDepth > channelCornerBlendRadius * 2.0)
			{
				OnCurve val15 = (OnCurve)new OnLineCurve((IOn3dPoint)(object)val12, (IOn3dPoint)new On3dPoint(val12.x, val12.y - channelWallDepth, 0.0));
				OnCurve val16 = (OnCurve)new OnLineCurve((IOn3dPoint)new On3dPoint(val12.x, val12.y - channelWallDepth, 0.0), (IOn3dPoint)(object)val14);
				CSUtilGeometry.TrimEndOfCrv(val15, channelCornerBlendRadius);
				CSUtilGeometry.TrimStartOfCrv(val16, channelCornerBlendRadius);
				OnCurve val17 = (OnCurve)(object)CSUtilCurves.BlendCurve((IOnCurve)(object)val15, (IOnCurve)(object)val16);
				OnCurve val18 = (OnCurve)new OnLineCurve((IOn3dPoint)(object)val14, (IOn3dPoint)new On3dPoint(val13.x, val13.y - channelWallDepth, 0.0));
				OnCurve val19 = (OnCurve)new OnLineCurve((IOn3dPoint)new On3dPoint(val13.x, val13.y - channelWallDepth, 0.0), (IOn3dPoint)(object)val13);
				CSUtilGeometry.TrimEndOfCrv(val18, channelCornerBlendRadius);
				CSUtilGeometry.TrimStartOfCrv(val19, channelCornerBlendRadius);
				OnCurve val20 = (OnCurve)(object)CSUtilCurves.BlendCurve((IOnCurve)(object)val18, (IOnCurve)(object)val19);
				IOnCurve[] obj = new IOnCurve[3]
				{
					(IOnCurve)val15,
					(IOnCurve)val17,
					(IOnCurve)val16
				};
				OnCurve[] array = null;
				RhUtil.RhinoMergeCurves((IOnCurve[])(object)obj, ref array);
				val21 = array[0];
				IOnCurve[] obj2 = new IOnCurve[3]
				{
					(IOnCurve)val18,
					(IOnCurve)val20,
					(IOnCurve)val19
				};
				array = null;
				RhUtil.RhinoMergeCurves((IOnCurve[])(object)obj2, ref array);
				val22 = array[0];
			}
			else
			{
				val21 = (OnCurve)new OnLineCurve((IOn3dPoint)(object)val10.PointAtEnd(), (IOn3dPoint)(object)val14);
				val22 = (OnCurve)new OnLineCurve((IOn3dPoint)(object)val14, (IOn3dPoint)(object)val11.PointAtStart());
			}
			CSUtilGeometry.TrimEndOfCrv(val10, channelCornerBlendRadius);
			CSUtilGeometry.TrimStartAndEndOfCrv(val21, channelCornerBlendRadius);
			CSUtilGeometry.TrimStartAndEndOfCrv(val22, channelCornerBlendRadius);
			CSUtilGeometry.TrimStartOfCrv(val11, channelCornerBlendRadius);
			OnCurve val23 = (OnCurve)(object)CSUtilCurves.BlendCurve((IOnCurve)(object)val10, (IOnCurve)(object)val21);
			OnCurve val24 = (OnCurve)(object)CSUtilCurves.BlendCurve((IOnCurve)(object)val21, (IOnCurve)(object)val22);
			OnCurve val25 = (OnCurve)(object)CSUtilCurves.BlendCurve((IOnCurve)(object)val22, (IOnCurve)(object)val11);
			IOnCurve[] obj3 = new IOnCurve[7]
			{
				(IOnCurve)val10,
				(IOnCurve)val23,
				(IOnCurve)val21,
				(IOnCurve)val24,
				(IOnCurve)val22,
				(IOnCurve)val25,
				(IOnCurve)val11
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
			haloProfile = array2[0];
		}
		else
		{
			OnXform val26 = new OnXform();
			val26.Scale(1.0, profileHeight, 1.0);
			OnNurbsCurve val27 = profile.Duplicate();
			val27.Transform((IOnXform)(object)val26);
			haloProfile = (OnCurve)(object)val27;
		}
		OnXform val28 = new OnXform();
		val28.Translation(0.0, 0.0 - profileHeight, 0.0);
		OnXform val29 = new OnXform();
		val29.Rotation(angle, OnUtil.On_zaxis, OnUtil.On_origin);
		((OnGeometry)haloProfile).Transform((IOnXform)(object)(val29 * (IOnXform)(object)val28));
		double num5 = val12.y - channelWallDepth;
		On3dPoint val30 = new On3dPoint(channelWallThicknessInner, num5, 0.0);
		val30.Transform((IOnXform)(object)(val29 * (IOnXform)(object)val28));
		double y = val12.y;
		On3dPoint val31 = new On3dPoint(channelWallThicknessInner, y, 0.0);
		val31.Transform((IOnXform)(object)(val29 * (IOnXform)(object)val28));
		OnBoundingBox val32 = new OnBoundingBox();
		if (!((OnGeometry)haloProfile).GetTightBoundingBox(ref val32))
		{
			outError = "Unable to get tight bounding box from halo profile";
			return false;
		}
		double num6 = channelWallThicknessExtraInner + channelWallThicknessInner;
		double num7 = Math.Min(val30.x, val31.x) - val32.Min().x;
		haloProfileChannelWallThicknessExtraInner = num7 * (channelWallThicknessExtraInner / num6);
		haloProfileChannelWallThicknessInner = num7 * (channelWallThicknessInner / num6);
		haloProfileInnerX = val32.Min().x;
		return true;
	}
}
