using System.Collections.Generic;
using CSBlockUtil;
using CSDatas;
using GV.CounterSketch.Shared;
using Rhino.Geometry;

namespace CSBlocks.GemClusters.Obsolete;

public sealed class CSClusterParam_V1
{
	public readonly CSDataArray InCenterObject;

	public readonly bool InCenterGemShapeOverrideEnabled;

	public readonly CSGemData InCenterGemShapeOverride;

	public readonly double InCenterGemSpacing;

	public readonly CSGemData InGemDefinition;

	public readonly double InSpacing;

	public readonly double InAngle;

	public readonly double InRotation;

	public readonly double InPlacement;

	public readonly double InFixPosition;

	public readonly int InStyle;

	public readonly bool InEven;

	public readonly double InProfileWidth;

	public readonly double InChannelWallThicknessInner;

	public readonly double InChannelWallThicknessOuter;

	public readonly double InChannelSpacing;

	public readonly bool InShowChannel;

	public readonly double InGemHeightOffset;

	public readonly bool InXYScaleToGem;

	public readonly ValueListItem InCenterGemShapeOverrideMode;

	public readonly bool InFixedGemSpacing;

	public readonly double InRailX;

	public readonly double InRailZ;

	public readonly double InGemSpacingTolerance;

	public readonly ValueListItem InUseMinCuletDistance;

	public readonly double InMinCuletDistance;

	public readonly double InProfileHeight;

	public readonly double InCornerBlend;

	public readonly double InOffsetX;

	public readonly double InOffsetY;

	public readonly Point3d InEmeraldCornerPoint1;

	public readonly Point3d InEmeraldCornerPoint2;

	public readonly double InRotate;

	public readonly bool InGemSizeToggle;

	public readonly CSGemData InGemSize;

	public readonly bool InForceGems;

	public readonly bool InProngCornerUse;

	public readonly List<double> InProngCornerPosition;

	public readonly List<int> InProngCornerType;

	public readonly List<double> InProngCornerSizing;

	public CSClusterParam_V1(CSCluster11 inCluster)
	{
		InCenterObject = inCluster.InCenterObject.GetFlattenArray();
		InCenterGemShapeOverrideEnabled = inCluster.InCenterGemShapeOverrideEnabled.Boolean;
		InCenterGemShapeOverride = inCluster.InCenterGemShapeOverride.Gem;
		InCenterGemSpacing = inCluster.InCenterGemSpacing.Double;
		InGemDefinition = inCluster.InGemDefinition.Gem;
		InSpacing = inCluster.InSpacing.Double;
		InAngle = inCluster.InAngle.Double;
		InRotation = inCluster.InRotation.Double;
		InPlacement = inCluster.InPlacement.Double;
		InFixPosition = inCluster.InFixPosition.Double;
		InStyle = inCluster.InStyle.Integer;
		InEven = inCluster.InEven.Boolean;
		InProfileWidth = inCluster.InProfileWidth.Double;
		InChannelWallThicknessInner = inCluster.InChannelWallThicknessInner.Double;
		InChannelWallThicknessOuter = inCluster.InChannelWallThicknessOuter.Double;
		InChannelSpacing = inCluster.InChannelSpacing.Double;
		InShowChannel = inCluster.InShowChannel.Boolean;
		InGemHeightOffset = inCluster.InGemHeightOffset.Double;
		InXYScaleToGem = inCluster.InXYScaleToGem.Boolean;
		InCenterGemShapeOverrideMode = inCluster.InCenterGemShapeOverrideMode.ValueListItem;
		InFixedGemSpacing = inCluster.InFixedGemSpacing.Boolean;
		InRailX = inCluster.InRailX.Double;
		InRailZ = inCluster.InRailZ.Double;
		InGemSpacingTolerance = inCluster.InGemSpacingTolerance.Double;
		InUseMinCuletDistance = inCluster.InUseMinCuletDistance.ValueListItem;
		InMinCuletDistance = inCluster.InMinCuletDistance.Double;
		InProfileHeight = inCluster.InProfileHeight.Double;
		InCornerBlend = inCluster.InCornerBlend.Double;
		InOffsetX = inCluster.InOffsetX.Double;
		InOffsetY = inCluster.InOffsetY.Double;
		InEmeraldCornerPoint1 = inCluster.InEmeraldCornerPoint1.Point3d;
		InEmeraldCornerPoint2 = inCluster.InEmeraldCornerPoint2.Point3d;
		InRotate = inCluster.InRotate.Double;
		InGemSizeToggle = inCluster.InGemSizeToggle.Boolean;
		InGemSize = inCluster.InGemSize.Gem;
		InForceGems = inCluster.InForceGems.Boolean;
		InProngCornerUse = inCluster.InProngCornerUse.Boolean;
		InProngCornerPosition = inCluster.InProngCornerPosition.DoubleList;
		InProngCornerType = inCluster.InProngCornerType.IntegerList;
		InProngCornerSizing = inCluster.InProngCornerSizing.DoubleList;
		InAngle = CSBlockUtil.CSUtilMath.ToRadians(InAngle);
		InRotation = CSBlockUtil.CSUtilMath.ToRadians(InRotation);
	}
}
