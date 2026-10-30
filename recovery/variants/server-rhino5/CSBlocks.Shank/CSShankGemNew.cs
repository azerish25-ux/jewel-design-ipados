using System;
using System.Collections.Generic;
using CS.CSDataUtilities;
using CS.CSObjects.CSBlocks;
using CS.Parametric;
using CS.Parametric.Channels;
using CSBlockUtil;
using CSBlocks.Shank.Helpers;
using CSDatas;
using CSGemInstance.GemTableData;
using GV.CounterSketch.Shared;
using Rhino;
using Rhino.Geometry;
using Rhino.Geometry.Intersect;

namespace CSBlocks.Shank;

[CSBlock("ShankGemNew", "{4134705C-830C-4141-9434-A7A81336A86D}")]
public sealed class CSShankGemNew : CSBlock
{
	public readonly CSInputChannel InCurve;

	public readonly CSInputChannel InBaseObject;

	public readonly CSInputChannel InFrenchObj;

	public readonly CSInputChannel InGemShape;

	public readonly CSInputChannel InDiameterStart;

	public readonly CSInputChannel InDiameterMiddle;

	public readonly CSInputChannel InDiameterEnd;

	public readonly CSInputChannel InPositionStart;

	public readonly CSInputChannel InPositionEnd;

	public readonly CSInputChannel InPositionMiddle;

	public readonly CSInputChannel InPlacementStart;

	public readonly CSInputChannel InPlacementMiddle;

	public readonly CSInputChannel InPlacementEnd;

	public readonly CSInputChannel InRoll;

	public readonly CSInputChannel InTilt;

	public readonly CSInputChannel InOrientationMethod;

	public readonly CSInputChannel InOrientationFollowCurve;

	public readonly CSInputChannel InPlacementMethod;

	public readonly CSInputChannel InSpacingMethod;

	public readonly CSInputChannel InEnableDiameterMiddle;

	public readonly CSInputChannel InEnableDiameterEnd;

	public readonly CSInputChannel InEnablePlacementMiddle;

	public readonly CSInputChannel InEnablePlacementEnd;

	public readonly CSInputChannel InDoFlip;

	public readonly CSInputChannel InDoMirror;

	public readonly CSInputChannel InDoReverseDirection;

	public readonly CSInputChannel InProfile;

	public readonly CSInputChannel InProfile2;

	public readonly CSInputChannel InChannelEnabled;

	public readonly CSInputChannel InMirrorMode;

	public readonly CSInputChannel InChannelClosed;

	public readonly CSInputChannel InUseMinCuletDistance;

	public readonly CSInputChannel InMinCuletDistance;

	public readonly CSInputChannel InShowGems;

	public readonly CSInputChannel InSpacingStart;

	public readonly CSInputChannel InSpacingMiddle;

	public readonly CSInputChannel InSpacingEnd;

	public readonly CSInputChannel InSpacingToleranceStart;

	public readonly CSInputChannel InSpacingToleranceMiddle;

	public readonly CSInputChannel InSpacingToleranceEnd;

	public readonly CSInputChannel InWallWidth;

	public readonly CSInputChannel InChannelWallGemOverlap;

	public readonly CSInputChannel InChannelStartSlopeSpacing;

	public readonly CSInputChannel InChannelEndSlopeSpacing;

	public readonly CSInputChannel InMoveGemsStart;

	public readonly CSInputChannel InSideBuild;

	public readonly CSInputChannel InFront;

	public readonly CSInputChannel InEuroGemToggle;

	public readonly CSInputChannel InEuroGemAngle;

	public readonly CSInputChannel InGemCount;

	public readonly CSInputChannel InGemCountToggle;

	public readonly CSInputChannel InMetalWidthToggle;

	public readonly CSInputChannel InProfileWidthStart;

	public readonly CSInputChannel InProfileWidthMiddleToggle;

	public readonly CSInputChannel InProfileWidthMiddle;

	public readonly CSInputChannel InProfileWidthEndToggle;

	public readonly CSInputChannel InProfileWidthEnd;

	public readonly CSInputChannel InForceGems;

	public readonly CSInputChannel InRotate90;

	public readonly CSInputChannel InMoveGemsEndOverride;

	public readonly CSInputChannel InMoveGemsEnd;

	public readonly CSOutputChannel OutDisplayGemArray;

	public readonly CSOutputChannel OutGemArray;

	public readonly CSOutputChannel OutGems;

	public readonly CSOutputChannel OutChannelYStart;

	public readonly CSOutputChannel OutChannelYEnd;

	public readonly CSOutputChannel OutPositionStart;

	public readonly CSOutputChannel OutPositionMiddle;

	public readonly CSOutputChannel OutPositionEnd;

	public readonly CSOutputChannel OutChannelEnabled;

	public readonly CSOutputChannel OutChannelStartSlopeLength;

	public readonly CSOutputChannel OutChannelEndSlopeLength;

	public readonly CSOutputChannel OutGemsClosedLoop;

	public readonly CSOutputChannel OutDirection;

	public readonly CSOutputChannel OutGemCount;

	public readonly CSOutputChannel OutGem0SizeX;

	public readonly CSOutputChannel OutGem0SizeY;

	public readonly CSOutputChannel OutGem1SizeY;

	public readonly CSOutputChannel OutGem0Position;

	public readonly CSOutputChannel OutGem1Position;

	public readonly CSOutputChannel OutGemLast0SizeX;

	public readonly CSOutputChannel OutGemLast0SizeY;

	public readonly CSOutputChannel OutGemLast1SizeY;

	public readonly CSOutputChannel OutGemLast0Position;

	public readonly CSOutputChannel OutGemLast1Position;

	public readonly CSOutputChannel OutWallWidth;

	public readonly CSOutputChannel OutChannelWallGemOverlap;

	public readonly CSOutputChannel OutCurveLength;

	public readonly CSOutputChannel OutProfile;

	public readonly CSOutputChannel OutStartGemXSize;

	public readonly CSOutputChannel OutStartGemYSize;

	public readonly CSOutputChannel OutStartGemZSize;

	public readonly CSOutputChannel OutStartGemZMin;

	public readonly CSOutputChannel OutStartGemZMax;

	public readonly CSOutputChannel OutMiddleGemXSize;

	public readonly CSOutputChannel OutMiddleGemYSize;

	public readonly CSOutputChannel OutMiddleGemZSize;

	public readonly CSOutputChannel OutMiddleGemZMin;

	public readonly CSOutputChannel OutMiddleGemZMax;

	public readonly CSOutputChannel OutEndGemXSize;

	public readonly CSOutputChannel OutEndGemYSize;

	public readonly CSOutputChannel OutEndGemZSize;

	public readonly CSOutputChannel OutEndGemZMin;

	public readonly CSOutputChannel OutEndGemZMax;

	public readonly CSOutputChannel OutGrewSmaller;

	public CSShankGemNew(string identifier)
		: base(identifier)
	{
		InCurve = new CSInputChannel(this, ChannelDataType.Curve, ChannelArrayType.Individual);
		InBaseObject = new CSInputChannel(this, ChannelDataType.Curve | ChannelDataType.Surface, ChannelArrayType.Any, ChannelFlag.NotRequired);
		InFrenchObj = new CSInputChannel(this, ChannelDataType.Brep | ChannelDataType.Mesh, ChannelArrayType.Array, ChannelFlag.NotRequired);
		InGemShape = new CSInputChannel(this, ChannelDataType.Gem, ChannelArrayType.Individual);
		InDiameterStart = new CSInputChannel(this, ChannelDataType.Gem, ChannelArrayType.Individual);
		InDiameterMiddle = new CSInputChannel(this, ChannelDataType.Gem, ChannelArrayType.Individual);
		InDiameterEnd = new CSInputChannel(this, ChannelDataType.Gem, ChannelArrayType.Individual);
		InPositionStart = new CSInputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual, 0.0);
		InPositionEnd = new CSInputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual, 1.0);
		InPositionMiddle = new CSInputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual, 50);
		InPlacementStart = new CSInputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual, 0.0);
		InPlacementMiddle = new CSInputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual, 0.0);
		InPlacementEnd = new CSInputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual, 0.0);
		InRoll = new CSInputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual, 0.0);
		InTilt = new CSInputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual, 0.0);
		InOrientationMethod = new CSInputChannel(this, ChannelDataType.Integer, ChannelArrayType.Individual, ValueListItem.GemOrientationMethodAxisY);
		InOrientationFollowCurve = new CSInputChannel(this, ChannelDataType.Boolean, ChannelArrayType.Individual, defaultValue: true);
		InPlacementMethod = new CSInputChannel(this, ChannelDataType.Integer, ChannelArrayType.Individual, ValueListItem.GemPlacementMethodGirdle);
		InSpacingMethod = new CSInputChannel(this, ChannelDataType.Integer, ChannelArrayType.Individual, ValueListItem.GemSpacingMethodMinimum);
		InEnableDiameterMiddle = new CSInputChannel(this, ChannelDataType.Boolean, ChannelArrayType.Individual, defaultValue: false);
		InEnableDiameterEnd = new CSInputChannel(this, ChannelDataType.Boolean, ChannelArrayType.Individual, defaultValue: false);
		InEnablePlacementMiddle = new CSInputChannel(this, ChannelDataType.Boolean, ChannelArrayType.Individual, defaultValue: false);
		InEnablePlacementEnd = new CSInputChannel(this, ChannelDataType.Boolean, ChannelArrayType.Individual, defaultValue: false);
		InDoFlip = new CSInputChannel(this, ChannelDataType.Boolean, ChannelArrayType.Individual, defaultValue: false);
		InDoMirror = new CSInputChannel(this, ChannelDataType.Boolean, ChannelArrayType.Individual, defaultValue: false);
		InDoReverseDirection = new CSInputChannel(this, ChannelDataType.Boolean, ChannelArrayType.Individual, defaultValue: false);
		InProfile = new CSInputChannel(this, ChannelDataType.Curve, ChannelArrayType.Any);
		InProfile2 = new CSInputChannel(this, ChannelDataType.Curve, ChannelArrayType.Any);
		InChannelEnabled = new CSInputChannel(this, ChannelDataType.Boolean, ChannelArrayType.Individual, defaultValue: false);
		InMirrorMode = new CSInputChannel(this, ChannelDataType.Integer, ChannelArrayType.Individual, ValueListItem.GemOnCrvMirrorOddAndEven);
		InChannelClosed = new CSInputChannel(this, ChannelDataType.Boolean, ChannelArrayType.Individual, defaultValue: false);
		InUseMinCuletDistance = new CSInputChannel(this, ChannelDataType.Integer, ChannelArrayType.Individual, ValueListItem.NoMinCulet);
		InMinCuletDistance = new CSInputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual, 0.0);
		InShowGems = new CSInputChannel(this, ChannelDataType.Boolean, ChannelArrayType.Individual, defaultValue: true);
		InSpacingStart = new CSInputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual, 0.0);
		InSpacingMiddle = new CSInputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual, 0.0);
		InSpacingEnd = new CSInputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual, 0.0);
		InSpacingToleranceStart = new CSInputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual, 0.0);
		InSpacingToleranceMiddle = new CSInputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual, 0.0);
		InSpacingToleranceEnd = new CSInputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual, 0.0);
		InWallWidth = new CSInputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual, 0.5);
		InChannelWallGemOverlap = new CSInputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual, 0.0);
		InChannelStartSlopeSpacing = new CSInputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual, 0.0);
		InChannelEndSlopeSpacing = new CSInputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual, 0.0);
		InMoveGemsStart = new CSInputChannel(this, ChannelDataType.Integer, ChannelArrayType.Individual, ValueListItem.ShankEdge);
		InSideBuild = new CSInputChannel(this, ChannelDataType.Boolean, ChannelArrayType.Individual, defaultValue: false);
		InFront = new CSInputChannel(this, ChannelDataType.Boolean, ChannelArrayType.Individual, defaultValue: true);
		InEuroGemToggle = new CSInputChannel(this, ChannelDataType.Boolean, ChannelArrayType.Individual, defaultValue: false);
		InEuroGemAngle = new CSInputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual, 135.0);
		InGemCount = new CSInputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual, 5);
		InGemCountToggle = new CSInputChannel(this, ChannelDataType.Boolean, ChannelArrayType.Individual, defaultValue: false);
		InMetalWidthToggle = new CSInputChannel(this, ChannelDataType.Boolean, ChannelArrayType.Individual, defaultValue: false);
		InProfileWidthStart = new CSInputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual, 3);
		InProfileWidthMiddleToggle = new CSInputChannel(this, ChannelDataType.Boolean, ChannelArrayType.Individual, defaultValue: false);
		InProfileWidthMiddle = new CSInputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual, 3);
		InProfileWidthEndToggle = new CSInputChannel(this, ChannelDataType.Boolean, ChannelArrayType.Individual, defaultValue: false);
		InProfileWidthEnd = new CSInputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual, 3);
		InForceGems = new CSInputChannel(this, ChannelDataType.Boolean, ChannelArrayType.Individual, defaultValue: false);
		InRotate90 = new CSInputChannel(this, ChannelDataType.Boolean, ChannelArrayType.Individual, defaultValue: false);
		InMoveGemsEnd = new CSInputChannel(this, ChannelDataType.Integer, ChannelArrayType.Individual, ValueListItem.ShankEdge);
		InMoveGemsEndOverride = new CSInputChannel(this, ChannelDataType.Boolean, ChannelArrayType.Individual, defaultValue: false);
		OutDisplayGemArray = new CSOutputChannel(this, ChannelDataType.Any, ChannelArrayType.Array);
		OutGemArray = new CSOutputChannel(this, ChannelDataType.Any, ChannelArrayType.Array);
		OutGems = new CSOutputChannel(this, ChannelDataType.Any, ChannelArrayType.Array);
		OutChannelYStart = new CSOutputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual);
		OutChannelYEnd = new CSOutputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual);
		OutPositionStart = new CSOutputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual);
		OutPositionMiddle = new CSOutputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual);
		OutPositionEnd = new CSOutputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual);
		OutChannelStartSlopeLength = new CSOutputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual);
		OutChannelEndSlopeLength = new CSOutputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual);
		OutDirection = new CSOutputChannel(this, ChannelDataType.Integer, ChannelArrayType.Individual);
		OutGemCount = new CSOutputChannel(this, ChannelDataType.Integer, ChannelArrayType.Individual);
		OutGem0SizeX = new CSOutputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual);
		OutGem0SizeY = new CSOutputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual);
		OutGem1SizeY = new CSOutputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual);
		OutGem0Position = new CSOutputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual);
		OutGem1Position = new CSOutputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual);
		OutGemLast0SizeX = new CSOutputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual);
		OutGemLast0SizeY = new CSOutputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual);
		OutGemLast1SizeY = new CSOutputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual);
		OutGemLast0Position = new CSOutputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual);
		OutGemLast1Position = new CSOutputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual);
		OutCurveLength = new CSOutputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual);
		OutWallWidth = new CSOutputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual);
		OutChannelWallGemOverlap = new CSOutputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual);
		OutChannelEnabled = new CSOutputChannel(this, ChannelDataType.Boolean, ChannelArrayType.Individual);
		OutGemsClosedLoop = new CSOutputChannel(this, ChannelDataType.Boolean, ChannelArrayType.Individual);
		OutProfile = new CSOutputChannel(this, ChannelDataType.Curve, ChannelArrayType.Individual);
		OutStartGemXSize = new CSOutputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual);
		OutStartGemYSize = new CSOutputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual);
		OutStartGemZSize = new CSOutputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual);
		OutStartGemZMin = new CSOutputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual);
		OutStartGemZMax = new CSOutputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual);
		OutMiddleGemXSize = new CSOutputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual);
		OutMiddleGemYSize = new CSOutputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual);
		OutMiddleGemZSize = new CSOutputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual);
		OutMiddleGemZMin = new CSOutputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual);
		OutMiddleGemZMax = new CSOutputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual);
		OutEndGemXSize = new CSOutputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual);
		OutEndGemYSize = new CSOutputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual);
		OutEndGemZSize = new CSOutputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual);
		OutEndGemZMin = new CSOutputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual);
		OutEndGemZMax = new CSOutputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual);
		OutGrewSmaller = new CSOutputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual);
		SetChannelIdentifiers(autoCheckInputs: true);
	}

	public override bool Update()
	{
		CSShankGemParam cSShankGemParam = new CSShankGemParam(this);
		double channelEndSlopeLength = cSShankGemParam.InChannelEndSlopeSpacing;
		double channelStartSlopeLength = cSShankGemParam.InChannelStartSlopeSpacing;
		double positionStartInput = cSShankGemParam.InPositionStart;
		double positionEndInput = cSShankGemParam.InPositionEnd;
		double inPositionMiddle = cSShankGemParam.InPositionMiddle;
		if (cSShankGemParam.InEuroGemToggle)
		{
			double length = new Vector3d(cSShankGemParam.InCurve.PointAtStart - cSShankGemParam.InCurve.PointAtNormalizedLength(0.5)).Length;
			Surface surface = NurbsSurface.CreateFromCorners(new Point3d(0.0, -1.0, 0.0), new Point3d(0.0, -1.0, length), new Point3d(0.0, 1.0, length), new Point3d(0.0, 1.0, 0.0));
			surface.Transform(Transform.Rotation(RhinoMath.ToRadians(cSShankGemParam.InEuroGemAngle), Vector3d.YAxis, Point3d.Origin));
			CurveIntersections curveIntersections = Intersection.CurveSurface(cSShankGemParam.InCurve, surface, 0.01, 0.01);
			cSShankGemParam.InCurve.ClosestPoint(curveIntersections[0].PointA, out var t);
			cSShankGemParam.InCurve.ClosestPoint(cSShankGemParam.InCurve.PointAtStart, out var t2);
			double length2 = cSShankGemParam.InCurve.GetLength();
			double length3 = cSShankGemParam.InCurve.GetLength(new Interval(t2, t));
			if (positionEndInput > length3 / length2)
			{
				positionEndInput = length3 / length2;
			}
		}
		double rollAngle = CSBlockUtil.CSUtilMath.ToRadians(cSShankGemParam.InRoll);
		double tiltAngle = CSBlockUtil.CSUtilMath.ToRadians(cSShankGemParam.InTilt);
		ValueListItem spacingMethod = cSShankGemParam.InSpacingMethod;
		if (cSShankGemParam.InGemCountToggle)
		{
			spacingMethod = ValueListItem.GemSpacingMethodFixed;
		}
		bool flag = cSShankGemParam.InShowGems;
		Curve profile;
		if (flag)
		{
			profile = cSShankGemParam.InProfile2;
			OutProfile.SetData(InProfile2.Data?.Duplicate());
		}
		else
		{
			profile = cSShankGemParam.InProfile;
			OutProfile.SetData(InProfile.Data?.Duplicate());
		}
		double inProfileWidthMiddle = cSShankGemParam.InProfileWidthMiddle;
		double inProfileWidthEnd = cSShankGemParam.InProfileWidthEnd;
		if (cSShankGemParam.InForceGems)
		{
			flag = true;
		}
		bool grewSmaller = false;
		UltGem(cSShankGemParam, profile, cSShankGemParam.InDiameterStart, cSShankGemParam.InDiameterMiddle, cSShankGemParam.InDiameterEnd, rollAngle, tiltAngle, inProfileWidthMiddle, inProfileWidthEnd, spacingMethod, ref channelStartSlopeLength, ref channelEndSlopeLength, ref positionStartInput, ref positionEndInput, ref grewSmaller, out var yStart, out var yEnd, out var gemsClosedLoopSoNoChannelEnds, out var direction, out var outputGems, out var gemList);
		foreach (UltimateGemData item in gemList)
		{
			if (cSShankGemParam.InRotate90)
			{
				double sizeY = item.SizeY;
				item.SizeY = item.SizeX;
				item.SizeX = sizeY;
			}
		}
		double length4 = cSShankGemParam.InCurve.GetLength();
		int count = gemList.Count;
		ShankMath(gemList, count, cSShankGemParam.InRotate90, grewSmaller, out var gem0SizeX, out var gem0SizeY, out var gem1SizeY, out var gem0Position, out var gem1Position, out var gemLast0SizeX, out var gemLast0SizeY, out var gemLast1SizeY, out var gemLast0Position, out var gemLast1Position);
		if (flag)
		{
			OutDisplayGemArray.SetData(outputGems);
		}
		OutGemArray.SetData(outputGems);
		CSDataArray cSDataArray = new CSDataArray();
		foreach (UltimateGemData item2 in gemList)
		{
			cSDataArray.Append(item2.AddToArray());
		}
		OutGems.SetData(cSDataArray);
		OutChannelYStart.SetData(new CSNumberData(yStart));
		OutChannelYEnd.SetData(new CSNumberData(yEnd));
		OutPositionStart.SetData(new CSNumberData(positionStartInput));
		OutPositionMiddle.SetData(new CSNumberData(inPositionMiddle));
		OutPositionEnd.SetData(new CSNumberData(positionEndInput));
		OutChannelEnabled.SetData(new CSNumberData(cSShankGemParam.InChannelEnabled ? 1 : 0));
		OutChannelStartSlopeLength.SetData(new CSNumberData(channelStartSlopeLength));
		OutChannelEndSlopeLength.SetData(new CSNumberData(channelEndSlopeLength));
		OutGemsClosedLoop.SetData(new CSNumberData(gemsClosedLoopSoNoChannelEnds ? 1 : 0));
		OutDirection.SetData(new CSNumberData(direction));
		OutGemCount.SetData(new CSNumberData(count));
		OutGem0SizeX.SetData(new CSNumberData(gem0SizeX));
		OutGem0SizeY.SetData(new CSNumberData(gem0SizeY));
		OutGem1SizeY.SetData(new CSNumberData(gem1SizeY));
		OutGem0Position.SetData(new CSNumberData(gem0Position));
		OutGem1Position.SetData(new CSNumberData(gem1Position));
		OutGemLast0SizeX.SetData(new CSNumberData(gemLast0SizeX));
		OutGemLast0SizeY.SetData(new CSNumberData(gemLast0SizeY));
		OutGemLast1SizeY.SetData(new CSNumberData(gemLast1SizeY));
		OutGemLast0Position.SetData(new CSNumberData(gemLast0Position));
		OutGemLast1Position.SetData(new CSNumberData(gemLast1Position));
		OutCurveLength.SetData(new CSNumberData(length4));
		OutChannelWallGemOverlap.SetData(new CSNumberData(cSShankGemParam.InChannelWallGemOverlap));
		OutWallWidth.SetData(new CSNumberData(cSShankGemParam.InWallWidth));
		OutGrewSmaller.SetData(new CSNumberData(grewSmaller));
		if (count > 0)
		{
			OutStartGemXSize.SetData(new CSNumberData(gemList[0].SizeX));
			OutStartGemYSize.SetData(new CSNumberData(gemList[0].SizeY));
			OutStartGemZSize.SetData(new CSNumberData(gemList[0].SizeZ));
			OutStartGemZMax.SetData(new CSNumberData(gemList[0].GemZMax));
			OutStartGemZMin.SetData(new CSNumberData(gemList[0].GemZMin));
			OutMiddleGemXSize.SetData(new CSNumberData(gemList[count / 2].SizeX));
			OutMiddleGemYSize.SetData(new CSNumberData(gemList[count / 2].SizeY));
			OutMiddleGemZSize.SetData(new CSNumberData(gemList[count / 2].SizeZ));
			OutMiddleGemZMax.SetData(new CSNumberData(gemList[count / 2].GemZMax));
			OutMiddleGemZMin.SetData(new CSNumberData(gemList[count / 2].GemZMin));
			OutEndGemXSize.SetData(new CSNumberData(gemList[count - 1].SizeX));
			OutEndGemYSize.SetData(new CSNumberData(gemList[count - 1].SizeY));
			OutEndGemZSize.SetData(new CSNumberData(gemList[count - 1].SizeZ));
			OutEndGemZMax.SetData(new CSNumberData(gemList[count - 1].GemZMax));
			OutEndGemZMin.SetData(new CSNumberData(gemList[count - 1].GemZMin));
		}
		return base.Update();
	}

	private static void ShankMath(List<UltimateGemData> gemList, int gemCount, bool rotated90, bool grewSmaller, out double gem0SizeX, out double gem0SizeY, out double gem1SizeY, out double gem0Position, out double gem1Position, out double gemLast0SizeX, out double gemLast0SizeY, out double gemLast1SizeY, out double gemLast0Position, out double gemLast1Position)
	{
		gem0SizeX = 0.0;
		gem0SizeY = 0.0;
		gem1SizeY = 0.0;
		gem0Position = 0.0;
		gem1Position = 0.0;
		gemLast0SizeX = 0.0;
		gemLast0SizeY = 0.0;
		gemLast1SizeY = 0.0;
		gemLast0Position = 0.0;
		gemLast1Position = 0.0;
		if (gemCount > 0)
		{
			UltimateGemData ultimateGemData = gemList[0];
			gem0SizeX = ultimateGemData.SizeX;
			gem0SizeY = ((rotated90 || grewSmaller) ? ultimateGemData.SizeY : ultimateGemData.vY);
			gem0Position = ultimateGemData.NormParam;
			ultimateGemData = gemList[gemCount - 1];
			gemLast0SizeX = ultimateGemData.SizeX;
			gemLast0SizeY = ((rotated90 || grewSmaller) ? ultimateGemData.SizeY : ultimateGemData.vY);
			gemLast0Position = ultimateGemData.NormParam;
		}
		if (gemCount > 1)
		{
			UltimateGemData ultimateGemData2 = gemList[1];
			gem1SizeY = ((rotated90 || grewSmaller) ? ultimateGemData2.SizeY : ultimateGemData2.vY);
			gem1Position = ultimateGemData2.NormParam;
			ultimateGemData2 = gemList[gemCount - 2];
			gemLast1SizeY = ((rotated90 || grewSmaller) ? ultimateGemData2.SizeY : ultimateGemData2.vY);
			gemLast1Position = ultimateGemData2.NormParam;
		}
	}

	private void UltGem(CSShankGemParam inChannels, Curve profile, CSGemData gemDiameterStart, CSGemData gemDiameterMiddle, CSGemData gemDiameterEnd, double rollAngle, double tiltAngle, double profileWidthMiddle, double profileWidthEnd, ValueListItem spacingMethod, ref double channelStartSlopeLength, ref double channelEndSlopeLength, ref double positionStartInput, ref double positionEndInput, ref bool grewSmaller, out double yStart, out double yEnd, out bool gemsClosedLoopSoNoChannelEnds, out int direction, out CSDataArray outputGems, out List<UltimateGemData> gemList)
	{
		outputGems = new CSDataArray();
		if (profile != null)
		{
			BoundingBox boundingBox = profile.GetBoundingBox(accurate: true);
			double x = boundingBox.Diagonal.X;
			double y = boundingBox.Diagonal.Y;
			Transform xform = Transform.Translation((0.0 - (boundingBox.Max.X + boundingBox.Min.X)) / 2.0, 0.0 - boundingBox.Min.Y, 0.0);
			profile.Transform(xform);
			Transform xform2 = Transform.Scale(Plane.WorldXY, 1.0 / x, 1.0 / y, 1.0);
			profile.Transform(xform2);
		}
		ValueListItem valueListItem = inChannels.InOrientationMethod;
		if (inChannels.InBaseCurve == null && valueListItem == ValueListItem.GemOrientationMethodSurface)
		{
			valueListItem = ValueListItem.GemOrientationMethodAxisY;
		}
		if (inChannels.InBaseCurve == null && (valueListItem == ValueListItem.OrientationMethodClosestPointOnCurve || valueListItem == ValueListItem.OrientationMethodIntersectPointOnCurve))
		{
			valueListItem = ValueListItem.GemOrientationMethodAxisY;
		}
		if (inChannels.InMetalWidthToggle)
		{
			GemSize gemSize = null;
			GemSize gemSize2 = null;
			GemSize gemSize3 = null;
			double num = -1.23432101234321E+308;
			double num2 = -1.23432101234321E+308;
			double num3 = -1.23432101234321E+308;
			double num4 = inChannels.InProfileWidthStart - inChannels.InWallWidth * 2.0;
			double num5 = profileWidthMiddle - inChannels.InWallWidth * 2.0;
			double num6 = profileWidthEnd - inChannels.InWallWidth * 2.0;
			foreach (GemSize size in gemDiameterStart.Sizes)
			{
				double num7 = size.X / gemDiameterStart.GemXSize;
				double num8 = size.Y / gemDiameterStart.GemYSize;
				double zScaleFactor = (num7 + num8) / 2.0;
				Transform xform3 = Transform.Scale(Plane.WorldXY, num7, num8, zScaleFactor);
				NurbsCurve nurbsCurve = gemDiameterStart.GetNurbsCurve();
				nurbsCurve.Transform(xform3);
				BoundingBox boundingBox2 = nurbsCurve.GetBoundingBox(accurate: true);
				double num9 = Math.Abs(boundingBox2.Max.Y) + Math.Abs(boundingBox2.Min.Y);
				double num10 = Math.Abs(num4 - num9);
				if (!RhinoMath.IsValidDouble(num) || num10 < num)
				{
					gemSize = size;
					num = num10;
				}
				double num11 = size.X / gemDiameterMiddle.GemXSize;
				double num12 = size.Y / gemDiameterMiddle.GemYSize;
				double zScaleFactor2 = (num11 + num12) / 2.0;
				Transform xform4 = Transform.Scale(Plane.WorldXY, num11, num12, zScaleFactor2);
				NurbsCurve nurbsCurve2 = gemDiameterMiddle.GetNurbsCurve();
				nurbsCurve2.Transform(xform4);
				BoundingBox boundingBox3 = nurbsCurve2.GetBoundingBox(accurate: true);
				double num13 = Math.Abs(boundingBox3.Max.Y) + Math.Abs(boundingBox3.Min.Y);
				double num14 = Math.Abs(num5 - num13);
				if (!RhinoMath.IsValidDouble(num2) || num14 < num2)
				{
					gemSize2 = size;
					num2 = num14;
				}
				double num15 = size.X / gemDiameterEnd.GemXSize;
				double num16 = size.Y / gemDiameterEnd.GemYSize;
				double zScaleFactor3 = (num15 + num16) / 2.0;
				Transform xform5 = Transform.Scale(Plane.WorldXY, num15, num16, zScaleFactor3);
				NurbsCurve nurbsCurve3 = gemDiameterEnd.GetNurbsCurve();
				nurbsCurve3.Transform(xform5);
				BoundingBox boundingBox4 = nurbsCurve3.GetBoundingBox(accurate: true);
				double num17 = Math.Abs(boundingBox4.Max.Y) + Math.Abs(boundingBox4.Min.Y);
				double num18 = Math.Abs(num6 - num17);
				if (!RhinoMath.IsValidDouble(num3) || num18 < num3)
				{
					gemSize3 = size;
					num3 = num18;
				}
			}
			if (gemSize != null)
			{
				gemDiameterStart.ScaleToSize(gemSize.X, gemSize.Y);
			}
			if (gemSize2 != null)
			{
				gemDiameterMiddle.ScaleToSize(gemSize2.X, gemSize2.Y);
			}
			if (gemSize3 != null)
			{
				gemDiameterEnd.ScaleToSize(gemSize3.X, gemSize3.Y);
			}
		}
		double num19 = (inChannels.InRotate90 ? gemDiameterStart.GemYSize : gemDiameterStart.GemXSize);
		double num20 = (inChannels.InRotate90 ? gemDiameterMiddle.GemYSize : gemDiameterMiddle.GemXSize);
		double num21 = (inChannels.InRotate90 ? gemDiameterEnd.GemYSize : gemDiameterEnd.GemXSize);
		yStart = (inChannels.InRotate90 ? gemDiameterStart.GemXSize : gemDiameterStart.GemYSize);
		double num22 = (inChannels.InRotate90 ? gemDiameterMiddle.GemXSize : gemDiameterMiddle.GemYSize);
		yEnd = (inChannels.InRotate90 ? gemDiameterEnd.GemXSize : gemDiameterEnd.GemYSize);
		if (inChannels.InMetalWidthToggle)
		{
			if (!inChannels.InProfileWidthEndToggle)
			{
				num21 = num19;
				yEnd = yStart;
				profileWidthEnd = inChannels.InProfileWidthStart;
			}
		}
		else if (!inChannels.InEnableDiameterEnd)
		{
			num21 = num19;
			yEnd = yStart;
		}
		if (inChannels.InMetalWidthToggle)
		{
			if (!inChannels.InProfileWidthMiddleToggle)
			{
				num20 = num19 + (num21 - num19) * inChannels.InPositionMiddle;
				num22 = yStart + (yEnd - yStart) * inChannels.InPositionMiddle;
				profileWidthMiddle = (inChannels.InProfileWidthStart + profileWidthEnd) / 2.0;
			}
		}
		else if (!inChannels.InEnableDiameterMiddle)
		{
			num20 = num19 + (num21 - num19) * inChannels.InPositionMiddle;
			num22 = yStart + (yEnd - yStart) * inChannels.InPositionMiddle;
		}
		double inPlacementStart = inChannels.InPlacementStart;
		double placementMiddle = inChannels.InPlacementMiddle;
		double num23 = inChannels.InPlacementEnd;
		if (!inChannels.InEnablePlacementEnd)
		{
			num23 = inPlacementStart;
		}
		if (!inChannels.InEnablePlacementMiddle)
		{
			placementMiddle = inPlacementStart + (num23 - inPlacementStart) * inChannels.InPositionMiddle;
		}
		Transform transform = CSBlockUtil.CSUtilMath.Rotation(rollAngle, Vector3d.ZAxis, Point3d.Origin);
		Transform transform2 = CSBlockUtil.CSUtilMath.Rotation(tiltAngle, Vector3d.XAxis, Point3d.Origin);
		Transform xTrans = transform * transform2;
		double xSize = (inChannels.InRotate90 ? yStart : num19);
		double ySize = (inChannels.InRotate90 ? num19 : yStart);
		if (!CSGemPosition.CalculateRotateTestGem(inChannels.InGemShape, xSize, ySize, rollAngle, inChannels.InRotate90, out var xSizeOut))
		{
			UpdateError("GetGemScaleFromX failed");
		}
		double xSize2 = (inChannels.InRotate90 ? num22 : num20);
		double ySize2 = (inChannels.InRotate90 ? num20 : num22);
		if (!CSGemPosition.CalculateRotateTestGem(inChannels.InGemShape, xSize2, ySize2, rollAngle, inChannels.InRotate90, out var xSizeOut2))
		{
			UpdateError("GetGemScaleFromX failed");
		}
		double xSize3 = (inChannels.InRotate90 ? yEnd : num21);
		double ySize3 = (inChannels.InRotate90 ? num21 : yEnd);
		if (!CSGemPosition.CalculateRotateTestGem(inChannels.InGemShape, xSize3, ySize3, rollAngle, inChannels.InRotate90, out var xSizeOut3))
		{
			UpdateError("GetGemScaleFromX failed");
		}
		direction = ((!(positionStartInput > positionEndInput)) ? 1 : (-1));
		if (inChannels.InGemCountToggle)
		{
			direction = 1;
		}
		if (inChannels.InCurve.IsClosed && inChannels.InDoReverseDirection)
		{
			positionStartInput += direction;
			direction = -direction;
		}
		gemsClosedLoopSoNoChannelEnds = inChannels.InChannelClosed;
		if (inChannels.InDoMirror && inChannels.InCurve.IsClosed)
		{
			double num24 = positionEndInput - positionStartInput;
			if (num24 > 0.499 && !inChannels.InGemCountToggle)
			{
				positionEndInput = positionStartInput + 0.5;
				gemsClosedLoopSoNoChannelEnds = true;
				channelStartSlopeLength = 0.0;
				channelEndSlopeLength = 0.0;
			}
			else if (num24 < -0.499 && !inChannels.InGemCountToggle)
			{
				positionEndInput = positionStartInput - 0.5;
				gemsClosedLoopSoNoChannelEnds = true;
				channelStartSlopeLength = 0.0;
				channelEndSlopeLength = 0.0;
			}
		}
		double num25 = 0.0;
		double num26 = 0.0;
		if (!gemsClosedLoopSoNoChannelEnds)
		{
			if (inChannels.SObj != null)
			{
				num25 = inChannels.SObj.GAll.GetBoundingBox(accurate: true).Diagonal.X;
			}
			if (inChannels.EObj != null)
			{
				num26 = inChannels.EObj.GAll.GetBoundingBox(accurate: true).Diagonal.X;
			}
		}
		double length = inChannels.InCurve.GetLength();
		double num27 = positionStartInput;
		if (!inChannels.InDoMirror)
		{
			if (inChannels.InChannelEnabled && inChannels.InMoveGemsStart == ValueListItem.ShankEdge)
			{
				num27 = ((inChannels.SObj == null) ? (num27 + (num19 / 2.0 + channelStartSlopeLength) / length * (double)direction) : (num27 + num19 * num25 / length * (double)direction));
			}
			else if (inChannels.InMoveGemsStart == ValueListItem.GemEdge)
			{
				num27 = positionStartInput + num19 / 2.0 / length * (double)direction;
			}
		}
		double num28 = positionEndInput;
		ValueListItem valueListItem2 = (inChannels.InMoveGemsEndOverride ? inChannels.InMoveGemsEnd : inChannels.InMoveGemsStart);
		if (inChannels.InChannelEnabled && valueListItem2 == ValueListItem.ShankEdge)
		{
			num28 = ((inChannels.EObj == null) ? (num28 - (num21 / 2.0 + channelEndSlopeLength) / length * (double)direction) : (num28 - num21 * num26 / length * (double)direction));
		}
		else if (valueListItem2 == ValueListItem.GemEdge)
		{
			num28 = positionEndInput - num21 / 2.0 / length * (double)direction;
		}
		double paramPosition = num27;
		double profileWidth;
		if (!inChannels.InDoMirror)
		{
			CSGemPosition.CalcGemPos(inChannels, paramPosition, xSizeOut, xSizeOut2, xSizeOut3, num19, num20, num21, yStart, num22, yEnd, doMirror: false, mirrorModeEven: false, num27, num28, xTrans, direction, valueListItem, profile, profileWidthMiddle, profileWidthEnd, inPlacementStart, placementMiddle, num23, ref grewSmaller, out gemList, out var _, out profileWidth);
			if (spacingMethod == ValueListItem.GemSpacingMethodMinimum)
			{
				CSGemPosition.AdjustGemPosEnd(inChannels, profile, profileWidth, profileWidthMiddle, profileWidthEnd, xSizeOut, xSizeOut2, xSizeOut3, inPlacementStart, placementMiddle, num23, direction, num27, num28, valueListItem, num19, num20, num21, yStart, num22, yEnd, xTrans, ref grewSmaller, ref gemList);
			}
		}
		else
		{
			List<UltimateGemData> gemList2 = new List<UltimateGemData>();
			List<UltimateGemData> gemListMirror2 = new List<UltimateGemData>();
			double num29 = 999.0;
			double num30 = 999.0;
			if (inChannels.InMirrorMode == ValueListItem.GemOnCrvMirrorEven || inChannels.InMirrorMode == ValueListItem.GemOnCrvMirrorOddAndEven)
			{
				CSGemPosition.CalcGemPos(inChannels, paramPosition, xSizeOut, xSizeOut2, xSizeOut3, num19, num20, num21, yStart, num22, yEnd, doMirror: true, mirrorModeEven: true, num27, num28, xTrans, direction, valueListItem, profile, profileWidthMiddle, profileWidthEnd, inPlacementStart, placementMiddle, num23, ref grewSmaller, out gemList2, out gemListMirror2, out profileWidth);
				if (spacingMethod == ValueListItem.GemSpacingMethodMinimum)
				{
					CSGemPosition.AdjustGemPosEndMirror(inChannels, profile, profileWidth, profileWidthMiddle, profileWidthEnd, xSizeOut, xSizeOut2, xSizeOut3, inPlacementStart, placementMiddle, num23, direction, num27, num28, valueListItem, num19, num20, num21, yStart, num22, yEnd, xTrans, ref grewSmaller, ref gemList2, ref gemListMirror2);
				}
				int count = gemList2.Count;
				if (count > 1)
				{
					num29 = (gemList2[0].GemPoint - gemList2[1].GemPoint).Length - gemList2[0].SizeXRotated / 2.0 - gemList2[1].SizeXRotated / 2.0;
				}
				if (count == 1 && gemListMirror2.Count == 1)
				{
					num29 = (gemList2[0].GemPoint - gemListMirror2[0].GemPoint).Length - gemList2[0].SizeXRotated / 2.0 - gemListMirror2[0].SizeXRotated / 2.0;
				}
				if (count >= 1)
				{
					num30 = Math.Abs(num28 - gemList2[count - 1].NormParam);
				}
				if (num29 < 0.0)
				{
					num29 = 999.0;
					num30 = 999.0;
				}
			}
			List<UltimateGemData> gemList3 = new List<UltimateGemData>();
			List<UltimateGemData> gemListMirror3 = new List<UltimateGemData>();
			double num31 = 999.0;
			double num32 = 999.0;
			if (inChannels.InMirrorMode == ValueListItem.GemOnCrvMirrorOdd || inChannels.InMirrorMode == ValueListItem.GemOnCrvMirrorOddAndEven)
			{
				CSGemPosition.CalcGemPos(inChannels, paramPosition, xSizeOut, xSizeOut2, xSizeOut3, num19, num20, num21, yStart, num22, yEnd, doMirror: true, mirrorModeEven: false, num27, num28, xTrans, direction, valueListItem, profile, profileWidthMiddle, profileWidthEnd, inPlacementStart, placementMiddle, num23, ref grewSmaller, out gemList3, out gemListMirror3, out profileWidth);
				if (spacingMethod == ValueListItem.GemSpacingMethodMinimum)
				{
					CSGemPosition.AdjustGemPosEndMirror(inChannels, profile, profileWidth, profileWidthMiddle, profileWidthEnd, xSizeOut, xSizeOut2, xSizeOut3, inPlacementStart, placementMiddle, num23, direction, num27, num28, valueListItem, num19, num20, num21, yStart, num22, yEnd, xTrans, ref grewSmaller, ref gemList3, ref gemListMirror3);
				}
				int count2 = gemList3.Count;
				if (count2 > 1)
				{
					num31 = (gemList3[0].GemPoint - gemList3[1].GemPoint).Length - gemList3[0].SizeXRotated / 2.0 - gemList3[1].SizeXRotated / 2.0;
				}
				if (count2 >= 1)
				{
					num32 = Math.Abs(num28 - gemList3[count2 - 1].NormParam);
				}
			}
			List<UltimateGemData> gemListMirror;
			if (num32 < 0.01 && num30 < 0.01)
			{
				if (num31 <= num29)
				{
					gemList = gemList3;
					gemListMirror = gemListMirror3;
				}
				else
				{
					gemList = gemList2;
					gemListMirror = gemListMirror2;
				}
			}
			else if (num32 <= num30)
			{
				gemList = gemList3;
				gemListMirror = gemListMirror3;
			}
			else
			{
				gemList = gemList2;
				gemListMirror = gemListMirror2;
			}
			for (int i = 0; i < gemListMirror.Count; i++)
			{
				gemList.Insert(0, gemListMirror[i]);
			}
		}
		List<Transform> list = new List<Transform>();
		foreach (UltimateGemData gem in gemList)
		{
			list.Add(gem.GemTransform);
		}
		if (list != null)
		{
			for (int j = 0; j < list.Count; j++)
			{
				CSGemData cSGemData = inChannels.InGemShape.Duplicate() as CSGemData;
				CSDataHelper.Transform(cSGemData, list[j]);
				if (inChannels.InRotate90)
				{
					CSDataHelper.Transform(cSGemData, Transform.Rotation(Math.PI / 2.0, cSGemData.Up, cSGemData.Center));
				}
				outputGems.Append(cSGemData);
				gemList[j].GemZMax = cSGemData.GemZMax;
				gemList[j].GemZMin = cSGemData.GemZMin;
			}
		}
		if (inChannels.InGemCountToggle && inChannels.InDoMirror)
		{
			inChannels.InCurve.NormalizedLengthParameter(positionStartInput, out var t);
			if (inChannels.InCurve.DuplicateCurve().Trim(t, gemList[gemList.Count - 1].Param).GetLength() >= inChannels.InCurve.GetLength() * 0.48)
			{
				gemsClosedLoopSoNoChannelEnds = true;
				channelStartSlopeLength = 0.0;
				channelEndSlopeLength = 0.0;
			}
		}
		if (gemList.Count > 0)
		{
			positionStartInput = gemList[0].NormParam - num19 * num25 / length * (double)direction;
			positionEndInput = gemList[gemList.Count - 1].NormParam + num21 * num26 / length * (double)direction;
		}
	}
}
