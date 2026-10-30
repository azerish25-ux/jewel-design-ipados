using System.Collections.Generic;
using CS;
using CS.Parametric.Channels;
using CSBlocks.Shank.French;
using CSDatas;
using GV.CounterSketch.Shared;
using Rhino.Geometry;

namespace CSBlocks.Shank.Helpers;

public sealed class CSShankGemParam
{
	public readonly Curve InCurve;

	public readonly Curve InBaseCurve;

	public readonly Brep InBaseSurface;

	public readonly CSGemData InGemShape;

	public readonly CSGemData InDiameterStart;

	public readonly CSGemData InDiameterMiddle;

	public readonly CSGemData InDiameterEnd;

	public readonly double InPositionStart;

	public readonly double InPositionEnd;

	public readonly double InPositionMiddle;

	public readonly double InPlacementStart;

	public readonly double InPlacementMiddle;

	public readonly double InPlacementEnd;

	public readonly double InRoll;

	public readonly double InTilt;

	public readonly ValueListItem InOrientationMethod;

	public readonly bool InOrientationFollowCurve;

	public readonly ValueListItem InPlacementMethod;

	public readonly ValueListItem InSpacingMethod;

	public readonly bool InEnableDiameterMiddle;

	public readonly bool InEnableDiameterEnd;

	public readonly bool InEnablePlacementMiddle;

	public readonly bool InEnablePlacementEnd;

	public readonly bool InDoFlip;

	public readonly bool InDoMirror;

	public readonly bool InDoReverseDirection;

	public readonly Curve InProfile;

	public readonly Curve InProfile2;

	public readonly bool InChannelEnabled;

	public readonly double InChannelWallGemOverlap;

	public readonly ValueListItem InMirrorMode;

	public readonly bool InChannelClosed;

	public readonly ValueListItem InUseMinCuletDistance;

	public readonly double InMinCuletDistance;

	public readonly ValueListItem InMoveGemsStart;

	public readonly bool InMoveGemsEndOverride;

	public readonly ValueListItem InMoveGemsEnd;

	public readonly bool InShowGems;

	public readonly double InChannelStartSlopeSpacing;

	public readonly double InChannelEndSlopeSpacing;

	public readonly double InSpacingEnd;

	public readonly double InSpacingToleranceEnd;

	public readonly bool InFront;

	public readonly double InSpacingMiddle;

	public readonly double InSpacingToleranceMiddle;

	public readonly bool InSideBuild;

	public readonly double InSpacingStart;

	public readonly double InSpacingToleranceStart;

	public readonly double InWallWidth;

	public readonly bool InRotate90;

	public readonly double InGemCount;

	public readonly bool InGemCountToggle;

	public readonly bool InEuroGemToggle;

	public readonly double InEuroGemAngle;

	public readonly bool InMetalWidthToggle;

	public readonly double InProfileWidthStart;

	public readonly double InProfileWidthMiddle;

	public readonly bool InProfileWidthMiddleToggle;

	public readonly double InProfileWidthEnd;

	public readonly bool InProfileWidthEndToggle;

	public readonly bool InForceGems;

	public readonly CSFrenchGeometry SObj;

	public readonly CSFrenchGeometry EObj;

	public CSShankGemParam(CSShankGemNew inShankGem)
	{
		InCurve = inShankGem.InCurve.Curve;
		InBaseCurve = inShankGem.InBaseObject.Curve;
		InBaseSurface = inShankGem.InBaseObject.Brep;
		InGemShape = inShankGem.InGemShape.Gem;
		InDiameterStart = inShankGem.InDiameterStart.Gem;
		InDiameterMiddle = inShankGem.InDiameterMiddle.Gem;
		InDiameterEnd = inShankGem.InDiameterEnd.Gem;
		InSpacingEnd = inShankGem.InSpacingEnd.Double;
		InSpacingToleranceEnd = inShankGem.InSpacingToleranceEnd.Double;
		InPositionStart = inShankGem.InPositionStart.Double;
		InPositionEnd = inShankGem.InPositionEnd.Double;
		InPositionMiddle = inShankGem.InPositionMiddle.Percentage;
		InPlacementStart = inShankGem.InPlacementStart.Double;
		InPlacementMiddle = inShankGem.InPlacementMiddle.Double;
		InPlacementEnd = inShankGem.InPlacementEnd.Double;
		InRoll = inShankGem.InRoll.Double;
		InTilt = inShankGem.InTilt.Double;
		InOrientationMethod = inShankGem.InOrientationMethod.ValueListItem;
		InOrientationFollowCurve = inShankGem.InOrientationFollowCurve.Boolean;
		InPlacementMethod = inShankGem.InPlacementMethod.ValueListItem;
		InSpacingMethod = inShankGem.InSpacingMethod.ValueListItem;
		InEnableDiameterMiddle = inShankGem.InEnableDiameterMiddle.Boolean;
		InEnableDiameterEnd = inShankGem.InEnableDiameterEnd.Boolean;
		InEnablePlacementMiddle = inShankGem.InEnablePlacementMiddle.Boolean;
		InEnablePlacementEnd = inShankGem.InEnablePlacementEnd.Boolean;
		InDoFlip = inShankGem.InDoFlip.Boolean;
		InDoMirror = inShankGem.InDoMirror.Boolean;
		InDoReverseDirection = inShankGem.InDoReverseDirection.Boolean;
		InProfile = inShankGem.InProfile.Curve;
		InProfile2 = inShankGem.InProfile2.Curve;
		InChannelEnabled = inShankGem.InChannelEnabled.Boolean;
		InChannelWallGemOverlap = inShankGem.InChannelWallGemOverlap.Double;
		InChannelStartSlopeSpacing = inShankGem.InChannelStartSlopeSpacing.Double;
		InChannelEndSlopeSpacing = inShankGem.InChannelEndSlopeSpacing.Double;
		InMirrorMode = inShankGem.InMirrorMode.ValueListItem;
		InChannelClosed = inShankGem.InChannelClosed.Boolean;
		InUseMinCuletDistance = inShankGem.InUseMinCuletDistance.ValueListItem;
		InMinCuletDistance = inShankGem.InMinCuletDistance.Double;
		InShowGems = inShankGem.InShowGems.Boolean;
		InMoveGemsStart = inShankGem.InMoveGemsStart.ValueListItem;
		InMoveGemsEnd = inShankGem.InMoveGemsEnd.ValueListItem;
		InMoveGemsEndOverride = inShankGem.InMoveGemsEndOverride.Boolean;
		InFront = inShankGem.InFront.Boolean;
		InSpacingMiddle = inShankGem.InSpacingMiddle.Double;
		InSpacingToleranceMiddle = inShankGem.InSpacingToleranceMiddle.Double;
		InSideBuild = inShankGem.InSideBuild.Boolean;
		InSpacingStart = inShankGem.InSpacingStart.Double;
		InSpacingToleranceStart = inShankGem.InSpacingToleranceStart.Double;
		InWallWidth = inShankGem.InWallWidth.Double;
		InRotate90 = inShankGem.InRotate90.Boolean;
		InGemCount = inShankGem.InGemCount.Double;
		InGemCountToggle = inShankGem.InGemCountToggle.Boolean;
		InEuroGemToggle = inShankGem.InEuroGemToggle.Boolean;
		InEuroGemAngle = inShankGem.InEuroGemAngle.Double;
		InMetalWidthToggle = inShankGem.InMetalWidthToggle.Boolean;
		InProfileWidthStart = inShankGem.InProfileWidthStart.Double;
		InProfileWidthMiddle = inShankGem.InProfileWidthMiddle.Double;
		InProfileWidthMiddleToggle = inShankGem.InProfileWidthMiddleToggle.Boolean;
		InProfileWidthEnd = inShankGem.InProfileWidthEnd.Double;
		InProfileWidthEndToggle = inShankGem.InProfileWidthEndToggle.Boolean;
		InForceGems = inShankGem.InForceGems.Boolean;
		bool isCamMode = CSApplicationHelper.Application.IsCamMode;
		CSInputChannel inFrenchObj = inShankGem.InFrenchObj;
		if (inFrenchObj != null)
		{
			List<Mesh> value2;
			if (inFrenchObj.GetValues(out List<Brep> value) && value != null && value.Count > 0)
			{
				SObj = new CSFrenchGeometry((IEnumerable<GeometryBase>)value, null, "s", isMesh: false, isCamMode);
				EObj = new CSFrenchGeometry((IEnumerable<GeometryBase>)value, null, "e", isMesh: false, isCamMode);
			}
			else if (inFrenchObj.GetValues(out value2) && value2 != null && value2.Count > 0)
			{
				SObj = new CSFrenchGeometry((IEnumerable<GeometryBase>)value2, null, "s", isMesh: true, isCamMode);
				EObj = new CSFrenchGeometry((IEnumerable<GeometryBase>)value2, null, "e", isMesh: true, isCamMode);
			}
		}
	}
}
