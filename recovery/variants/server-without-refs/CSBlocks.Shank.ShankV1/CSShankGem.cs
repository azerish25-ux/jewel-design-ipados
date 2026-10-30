using System;
using System.Collections.Generic;
using CS.CSObjects.CSBlocks;
using CS.Parametric;
using CS.Parametric.Channels;
using CSBlockDataUtils;
using CSBlockUtil;
using CSBlocks.Shank.Helpers;
using CSDatas;
using GV.CounterSketch.Shared;
using Rhino.Geometry;
using Rhino.Geometry.Intersect;

namespace CSBlocks.Shank.ShankV1;

[CSBlock("ShankGem", "{C9D9D4CA-9172-4E63-949B-0410B992E797}")]
public sealed class CSShankGem : CSBlock
{
	private sealed class UltimateGemInChannels
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

		public readonly double InSpacing;

		public readonly double InSpacingTollerance;

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

		public readonly double InChannelWallWidth;

		public readonly bool InChannelEnabled;

		public readonly double InChannelWallGemOverlap;

		public readonly double InChannelStartDistance;

		public readonly ValueListItem InMirrorMode;

		public readonly bool InChannelClosed;

		public readonly bool InUseMinCuletDistance;

		public readonly double InMinCuletDistance;

		public readonly bool InMoveGems;

		public readonly bool InOverrideStartPosition;

		public readonly bool InOverrideEndPosition;

		public readonly double InOverrideStart;

		public readonly double InOverrideEnd;

		public UltimateGemInChannels(CSShankGem In)
		{
			InCurve = In.InCurve.Curve;
			InBaseCurve = In.InBaseObject.Curve;
			InBaseSurface = In.InBaseObject.Brep;
			InGemShape = In.InGemShape.Gem;
			InDiameterStart = In.InDiameterStart.Gem;
			InDiameterMiddle = In.InDiameterMiddle.Gem;
			InDiameterEnd = In.InDiameterEnd.Gem;
			InPositionStart = In.InPositionStart.Double;
			InPositionEnd = In.InPositionEnd.Double;
			InPositionMiddle = In.InPositionMiddle.Percentage;
			InPlacementStart = In.InPlacementStart.Double;
			InPlacementMiddle = In.InPlacementMiddle.Double;
			InPlacementEnd = In.InPlacementEnd.Double;
			InSpacing = In.InSpacing.Double;
			InSpacingTollerance = In.InSpacingTollerance.Double;
			InRoll = In.InRoll.Double;
			InTilt = In.InTilt.Double;
			InOrientationMethod = In.InOrientationMethod.ValueListItem;
			InOrientationFollowCurve = In.InOrientationFollowCurve.Boolean;
			InPlacementMethod = In.InPlacementMethod.ValueListItem;
			InSpacingMethod = In.InSpacingMethod.ValueListItem;
			InEnableDiameterMiddle = In.InEnableDiameterMiddle.Boolean;
			InEnableDiameterEnd = In.InEnableDiameterEnd.Boolean;
			InEnablePlacementMiddle = In.InEnablePlacementMiddle.Boolean;
			InEnablePlacementEnd = In.InEnablePlacementEnd.Boolean;
			InDoFlip = In.InDoFlip.Boolean;
			InDoMirror = In.InDoMirror.Boolean;
			InDoReverseDirection = In.InDoReverseDirection.Boolean;
			InProfile = In.InProfile.Curve;
			InChannelWallWidth = In.InChannelWallWidth.Double;
			InChannelEnabled = In.InChannelEnabled.Boolean;
			InChannelWallGemOverlap = In.InChannelWallGemOverlap.Double;
			InChannelStartDistance = In.InChannelStartDistance.Double;
			InMirrorMode = In.InMirrorMode.ValueListItem;
			InChannelClosed = In.InChannelClosed.Boolean;
			InUseMinCuletDistance = In.InUseMinCuletDistance.Boolean;
			InMinCuletDistance = In.InMinCuletDistance.Double;
			InMoveGems = In.InMoveGems.Boolean;
			InOverrideStartPosition = In.InOverrideStartPosition.Boolean;
			InOverrideEndPosition = In.InOverrideEndPosition.Boolean;
			InOverrideStart = In.InOverrideStart.Double;
			InOverrideEnd = In.InOverrideEnd.Double;
		}
	}

	public readonly CSInputChannel InCurve;

	public readonly CSInputChannel InBaseObject;

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

	public readonly CSInputChannel InSpacing;

	public readonly CSInputChannel InSpacingTollerance;

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

	public readonly CSInputChannel InChannelWallWidth;

	public readonly CSInputChannel InChannelEnabled;

	public readonly CSInputChannel InChannelWallGemOverlap;

	public readonly CSInputChannel InChannelStartDistance;

	public readonly CSInputChannel InMirrorMode;

	public readonly CSInputChannel InChannelClosed;

	public readonly CSInputChannel InUseMinCuletDistance;

	public readonly CSInputChannel InMinCuletDistance;

	public readonly CSInputChannel InMoveGems;

	public readonly CSInputChannel InOverrideStartPosition;

	public readonly CSInputChannel InOverrideEndPosition;

	public readonly CSInputChannel InOverrideStart;

	public readonly CSInputChannel InOverrideEnd;

	public readonly CSOutputChannel OutGemXFormArray;

	public readonly CSOutputChannel OutGems;

	public readonly CSOutputChannel OutChannelYStart;

	public readonly CSOutputChannel OutChannelYEnd;

	public readonly CSOutputChannel OutPositionStart;

	public readonly CSOutputChannel OutPositionMiddle;

	public readonly CSOutputChannel OutPositionEnd;

	public readonly CSOutputChannel OutChannelEnabled;

	public readonly CSOutputChannel OutChannelSlopeLength;

	public readonly CSOutputChannel OutGemsClosedLoop;

	public readonly CSOutputChannel OutDirection;

	public CSShankGem(string identifier)
		: base(identifier)
	{
		InCurve = new CSInputChannel(this, ChannelDataType.Curve, ChannelArrayType.Individual);
		InBaseObject = new CSInputChannel(this, ChannelDataType.Curve | ChannelDataType.Surface, ChannelArrayType.Individual, ChannelFlag.NotRequired);
		InGemShape = new CSInputChannel(this, ChannelDataType.Gem, ChannelArrayType.Individual);
		InDiameterStart = new CSInputChannel(this, ChannelDataType.Gem, ChannelArrayType.Individual);
		InDiameterMiddle = new CSInputChannel(this, ChannelDataType.Gem, ChannelArrayType.Individual);
		InDiameterEnd = new CSInputChannel(this, ChannelDataType.Gem, ChannelArrayType.Individual);
		InPositionStart = new CSInputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual, 0.0);
		InPositionEnd = new CSInputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual, 1);
		InPositionMiddle = new CSInputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual, 50.0);
		InPlacementStart = new CSInputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual, 0.0);
		InPlacementMiddle = new CSInputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual, 0.0);
		InPlacementEnd = new CSInputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual, 0.0);
		InSpacing = new CSInputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual, 0.0);
		InSpacingTollerance = new CSInputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual, 0.0);
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
		InProfile = new CSInputChannel(this, ChannelDataType.Curve, ChannelArrayType.Individual);
		InChannelWallWidth = new CSInputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual, 0.0);
		InChannelEnabled = new CSInputChannel(this, ChannelDataType.Boolean, ChannelArrayType.Individual, defaultValue: false);
		InChannelWallGemOverlap = new CSInputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual, 0.0);
		InChannelStartDistance = new CSInputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual, 0.0);
		InMirrorMode = new CSInputChannel(this, ChannelDataType.Integer, ChannelArrayType.Individual, ValueListItem.GemOnCrvMirrorOddAndEven);
		InChannelClosed = new CSInputChannel(this, ChannelDataType.Boolean, ChannelArrayType.Individual, defaultValue: false);
		InUseMinCuletDistance = new CSInputChannel(this, ChannelDataType.Boolean, ChannelArrayType.Individual, defaultValue: false);
		InMinCuletDistance = new CSInputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual, 0);
		InMoveGems = new CSInputChannel(this, ChannelDataType.Boolean, ChannelArrayType.Individual, defaultValue: true);
		InOverrideStartPosition = new CSInputChannel(this, ChannelDataType.Boolean, ChannelArrayType.Individual, defaultValue: false);
		InOverrideEndPosition = new CSInputChannel(this, ChannelDataType.Boolean, ChannelArrayType.Individual, defaultValue: false);
		InOverrideStart = new CSInputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual, 0.3);
		InOverrideEnd = new CSInputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual, 0.6);
		OutGemXFormArray = new CSOutputChannel(this, ChannelDataType.Any, ChannelArrayType.Array);
		OutGems = new CSOutputChannel(this, ChannelDataType.Any, ChannelArrayType.Array);
		OutChannelYStart = new CSOutputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual);
		OutChannelYEnd = new CSOutputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual);
		OutPositionStart = new CSOutputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual);
		OutPositionMiddle = new CSOutputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual);
		OutPositionEnd = new CSOutputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual);
		OutChannelEnabled = new CSOutputChannel(this, ChannelDataType.Boolean, ChannelArrayType.Individual);
		OutChannelSlopeLength = new CSOutputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual);
		OutGemsClosedLoop = new CSOutputChannel(this, ChannelDataType.Boolean, ChannelArrayType.Individual);
		OutDirection = new CSOutputChannel(this, ChannelDataType.Integer, ChannelArrayType.Individual);
		SetChannelIdentifiers();
	}

	public override bool Update()
	{
		//IL_006e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0073: Unknown result type (might be due to invalid IL or missing references)
		//IL_0077: Unknown result type (might be due to invalid IL or missing references)
		//IL_007c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0089: Unknown result type (might be due to invalid IL or missing references)
		//IL_008e: Unknown result type (might be due to invalid IL or missing references)
		//IL_009b: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a0: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ab: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b0: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c7: Unknown result type (might be due to invalid IL or missing references)
		//IL_00cc: Unknown result type (might be due to invalid IL or missing references)
		//IL_00df: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e4: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e7: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ef: Unknown result type (might be due to invalid IL or missing references)
		//IL_0115: Unknown result type (might be due to invalid IL or missing references)
		//IL_011a: Unknown result type (might be due to invalid IL or missing references)
		//IL_011d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0232: Unknown result type (might be due to invalid IL or missing references)
		//IL_0237: Unknown result type (might be due to invalid IL or missing references)
		//IL_023c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0241: Unknown result type (might be due to invalid IL or missing references)
		//IL_0243: Unknown result type (might be due to invalid IL or missing references)
		//IL_0248: Unknown result type (might be due to invalid IL or missing references)
		//IL_024d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0252: Unknown result type (might be due to invalid IL or missing references)
		//IL_0254: Unknown result type (might be due to invalid IL or missing references)
		//IL_0256: Unknown result type (might be due to invalid IL or missing references)
		//IL_0258: Unknown result type (might be due to invalid IL or missing references)
		//IL_025d: Unknown result type (might be due to invalid IL or missing references)
		//IL_043e: Unknown result type (might be due to invalid IL or missing references)
		//IL_04cd: Unknown result type (might be due to invalid IL or missing references)
		//IL_0460: Unknown result type (might be due to invalid IL or missing references)
		//IL_04ec: Unknown result type (might be due to invalid IL or missing references)
		//IL_0510: Unknown result type (might be due to invalid IL or missing references)
		//IL_051d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0522: Unknown result type (might be due to invalid IL or missing references)
		//IL_0527: Unknown result type (might be due to invalid IL or missing references)
		//IL_0669: Unknown result type (might be due to invalid IL or missing references)
		//IL_0688: Unknown result type (might be due to invalid IL or missing references)
		//IL_07bf: Unknown result type (might be due to invalid IL or missing references)
		//IL_0579: Unknown result type (might be due to invalid IL or missing references)
		//IL_0586: Unknown result type (might be due to invalid IL or missing references)
		//IL_058b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0590: Unknown result type (might be due to invalid IL or missing references)
		//IL_06ac: Unknown result type (might be due to invalid IL or missing references)
		//IL_06b9: Unknown result type (might be due to invalid IL or missing references)
		//IL_06be: Unknown result type (might be due to invalid IL or missing references)
		//IL_06c3: Unknown result type (might be due to invalid IL or missing references)
		foreach (CSInputChannel inputChannel in base.InputChannels)
		{
			if (!inputChannel.CheckInputValid())
			{
				return UpdateMissing(inputChannel.Identifier + " is missing inputs");
			}
		}
		UltimateGemInChannels ultimateGemInChannels = new UltimateGemInChannels(this);
		Curve inProfile = ultimateGemInChannels.InProfile;
		Vector3d val;
		if (inProfile != null)
		{
			BoundingBox boundingBox = ((GeometryBase)inProfile).GetBoundingBox(true);
			val = ((BoundingBox)(ref boundingBox)).Diagonal;
			double x = ((Vector3d)(ref val)).X;
			val = ((BoundingBox)(ref boundingBox)).Diagonal;
			double y = ((Vector3d)(ref val)).Y;
			Point3d val2 = ((BoundingBox)(ref boundingBox)).Max;
			double x2 = ((Point3d)(ref val2)).X;
			val2 = ((BoundingBox)(ref boundingBox)).Min;
			double num = (0.0 - (x2 + ((Point3d)(ref val2)).X)) / 2.0;
			val2 = ((BoundingBox)(ref boundingBox)).Min;
			Transform val3 = Transform.Translation(num, 0.0 - ((Point3d)(ref val2)).Y, 0.0);
			((GeometryBase)inProfile).Transform(val3);
			Transform val4 = Transform.Scale(Plane.WorldXY, 1.0 / x, 1.0 / y, 1.0);
			((GeometryBase)inProfile).Transform(val4);
		}
		ValueListItem valueListItem = ultimateGemInChannels.InOrientationMethod;
		if (ultimateGemInChannels.InBaseSurface == null && valueListItem == ValueListItem.GemOrientationMethodSurface)
		{
			valueListItem = ValueListItem.GemOrientationMethodAxisY;
		}
		if (ultimateGemInChannels.InBaseCurve == null && (valueListItem == ValueListItem.OrientationMethodClosestPointOnCurve || valueListItem == ValueListItem.OrientationMethodIntersectPointOnCurve))
		{
			valueListItem = ValueListItem.GemOrientationMethodAxisY;
		}
		double gemXSize = ultimateGemInChannels.InDiameterStart.GemXSize;
		double num2 = ultimateGemInChannels.InDiameterMiddle.GemXSize;
		double num3 = ultimateGemInChannels.InDiameterEnd.GemXSize;
		double gemYSize = ultimateGemInChannels.InDiameterStart.GemYSize;
		double num4 = ultimateGemInChannels.InDiameterMiddle.GemYSize;
		double num5 = ultimateGemInChannels.InDiameterEnd.GemYSize;
		if (!ultimateGemInChannels.InEnableDiameterEnd)
		{
			num3 = gemXSize;
			num5 = gemYSize;
		}
		if (!ultimateGemInChannels.InEnableDiameterMiddle)
		{
			num2 = gemXSize + (num3 - gemXSize) * ultimateGemInChannels.InPositionMiddle;
			num4 = gemYSize + (num5 - gemYSize) * ultimateGemInChannels.InPositionMiddle;
		}
		double inPlacementStart = ultimateGemInChannels.InPlacementStart;
		double placementMiddle = ultimateGemInChannels.InPlacementMiddle;
		double num6 = ultimateGemInChannels.InPlacementEnd;
		if (!ultimateGemInChannels.InEnablePlacementEnd)
		{
			num6 = inPlacementStart;
		}
		if (!ultimateGemInChannels.InEnablePlacementMiddle)
		{
			placementMiddle = inPlacementStart + (num6 - inPlacementStart) * ultimateGemInChannels.InPositionMiddle;
		}
		double num7 = CSBlockUtil.CSUtilMath.ToRadians(ultimateGemInChannels.InRoll);
		double angleRadians = CSBlockUtil.CSUtilMath.ToRadians(ultimateGemInChannels.InTilt);
		Transform val5 = CSBlockUtil.CSUtilMath.Rotation(num7, Vector3d.ZAxis, Point3d.Origin);
		Transform val6 = CSBlockUtil.CSUtilMath.Rotation(angleRadians, Vector3d.XAxis, Point3d.Origin);
		Transform xTrans = val5 * val6;
		if (!CalculateRotateTestGem(ultimateGemInChannels.InGemShape, gemXSize, gemYSize, num7, out var xSizeOut))
		{
			UpdateError("GetGemScaleFromX failed");
		}
		if (!CalculateRotateTestGem(ultimateGemInChannels.InGemShape, num2, num4, num7, out var xSizeOut2))
		{
			UpdateError("GetGemScaleFromX failed");
		}
		if (!CalculateRotateTestGem(ultimateGemInChannels.InGemShape, num3, num5, num7, out var xSizeOut3))
		{
			UpdateError("GetGemScaleFromX failed");
		}
		double num8 = ((!ultimateGemInChannels.InOverrideStartPosition) ? ultimateGemInChannels.InPositionStart : ultimateGemInChannels.InOverrideStart);
		double num9 = ((!ultimateGemInChannels.InOverrideEndPosition) ? ultimateGemInChannels.InPositionEnd : ultimateGemInChannels.InOverrideEnd);
		int direction = ((!(num8 > num9)) ? 1 : (-1));
		if (ultimateGemInChannels.InCurve.IsClosed && ultimateGemInChannels.InDoReverseDirection)
		{
			num8 += (double)direction;
			direction = -direction;
		}
		double num10 = ultimateGemInChannels.InChannelStartDistance;
		bool flag = ultimateGemInChannels.InChannelClosed;
		double length = ultimateGemInChannels.InCurve.GetLength();
		if (ultimateGemInChannels.InDoMirror && ultimateGemInChannels.InCurve.IsClosed)
		{
			double num11 = num9 - num8;
			if (num11 > 0.499)
			{
				num9 = num8 + 0.5;
				flag = true;
				num10 = 0.0;
			}
			else if (num11 < -0.499)
			{
				num9 = num8 - 0.5;
				flag = true;
				num10 = 0.0;
			}
		}
		double num12 = num8;
		if (!ultimateGemInChannels.InDoMirror && ultimateGemInChannels.InChannelEnabled && ultimateGemInChannels.InMoveGems)
		{
			num12 += (gemXSize / 2.0 + num10) / length * (double)direction;
		}
		double num13 = num9;
		if (ultimateGemInChannels.InChannelEnabled && ultimateGemInChannels.InMoveGems)
		{
			num13 -= (num3 / 2.0 + num10) / length * (double)direction;
		}
		double paramPosition = num12;
		List<UltimateGemData> gemList;
		if (!ultimateGemInChannels.InDoMirror)
		{
			CalcGemPos(ultimateGemInChannels, paramPosition, xSizeOut, xSizeOut2, xSizeOut3, gemXSize, num2, num3, gemYSize, num4, num5, inPlacementStart, placementMiddle, num6, doMirror: false, mirrorModeEven: false, num12, num13, inProfile, xTrans, direction, valueListItem, out gemList, out var _);
			if (ultimateGemInChannels.InSpacingMethod == ValueListItem.GemSpacingMethodMinimum)
			{
				AdjustGemPosEnd(ultimateGemInChannels, num12, num13, inProfile, xTrans, valueListItem, ref gemList, ref direction);
			}
		}
		else
		{
			List<UltimateGemData> gemList2 = new List<UltimateGemData>();
			List<UltimateGemData> gemListMirror2 = new List<UltimateGemData>();
			double num14 = 999.0;
			double num15 = 999.0;
			if (ultimateGemInChannels.InMirrorMode == ValueListItem.GemOnCrvMirrorEven || ultimateGemInChannels.InMirrorMode == ValueListItem.GemOnCrvMirrorOddAndEven)
			{
				CalcGemPos(ultimateGemInChannels, paramPosition, xSizeOut, xSizeOut2, xSizeOut3, gemXSize, num2, num3, gemYSize, num4, num5, inPlacementStart, placementMiddle, num6, doMirror: true, mirrorModeEven: true, num12, num13, inProfile, xTrans, direction, valueListItem, out gemList2, out gemListMirror2);
				if (ultimateGemInChannels.InSpacingMethod == ValueListItem.GemSpacingMethodMinimum)
				{
					AdjustGemPosEndMirror(ultimateGemInChannels, num12, num13, inProfile, xTrans, direction, valueListItem, ref gemList2, ref gemListMirror2);
				}
				int count = gemList2.Count;
				if (count > 1)
				{
					val = gemList2[0].GemPoint - gemList2[1].GemPoint;
					num14 = ((Vector3d)(ref val)).Length - gemList2[0].SizeXRotated / 2.0 - gemList2[1].SizeXRotated / 2.0;
				}
				if (count == 1 && gemListMirror2.Count == 1)
				{
					val = gemList2[0].GemPoint - gemListMirror2[0].GemPoint;
					num14 = ((Vector3d)(ref val)).Length - gemList2[0].SizeXRotated / 2.0 - gemListMirror2[0].SizeXRotated / 2.0;
				}
				if (count >= 1)
				{
					num15 = Math.Abs(num13 - gemList2[count - 1].NormParam);
				}
				if (num14 < 0.0)
				{
					num14 = 999.0;
					num15 = 999.0;
				}
			}
			List<UltimateGemData> gemList3 = new List<UltimateGemData>();
			List<UltimateGemData> gemListMirror3 = new List<UltimateGemData>();
			double num16 = 999.0;
			double num17 = 999.0;
			if (ultimateGemInChannels.InMirrorMode == ValueListItem.GemOnCrvMirrorOdd || ultimateGemInChannels.InMirrorMode == ValueListItem.GemOnCrvMirrorOddAndEven)
			{
				CalcGemPos(ultimateGemInChannels, paramPosition, xSizeOut, xSizeOut2, xSizeOut3, gemXSize, num2, num3, gemYSize, num4, num5, inPlacementStart, placementMiddle, num6, doMirror: true, mirrorModeEven: false, num12, num13, inProfile, xTrans, direction, valueListItem, out gemList3, out gemListMirror3);
				if (ultimateGemInChannels.InSpacingMethod == ValueListItem.GemSpacingMethodMinimum)
				{
					AdjustGemPosEndMirror(ultimateGemInChannels, num12, num13, inProfile, xTrans, direction, valueListItem, ref gemList3, ref gemListMirror3);
				}
				int count2 = gemList3.Count;
				if (count2 > 1)
				{
					val = gemList3[0].GemPoint - gemList3[1].GemPoint;
					num16 = ((Vector3d)(ref val)).Length - gemList3[0].SizeXRotated / 2.0 - gemList3[1].SizeXRotated / 2.0;
				}
				if (count2 >= 1)
				{
					num17 = Math.Abs(num13 - gemList3[count2 - 1].NormParam);
				}
			}
			List<UltimateGemData> gemListMirror;
			if (num17 < 0.01 && num15 < 0.01)
			{
				if (num16 <= num14)
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
			else if (num17 <= num15)
			{
				gemList = gemList3;
				gemListMirror = gemListMirror3;
			}
			else
			{
				gemList = gemList2;
				gemListMirror = gemListMirror2;
			}
			foreach (UltimateGemData item in gemListMirror)
			{
				gemList.Insert(0, item);
			}
		}
		CSDataArray cSDataArray = new CSDataArray();
		foreach (UltimateGemData item2 in gemList)
		{
			cSDataArray.Append(new CSXformData(item2.GemTransform));
		}
		OutGemXFormArray.SetData(cSDataArray);
		CSDataArray cSDataArray2 = new CSDataArray();
		foreach (UltimateGemData item3 in gemList)
		{
			cSDataArray2.Append(item3.AddToArray());
		}
		OutGems.SetData(cSDataArray2);
		OutChannelYStart.SetData(new CSNumberData(gemYSize));
		OutChannelYEnd.SetData(new CSNumberData(num5));
		OutPositionStart.SetData(new CSNumberData(num8));
		OutPositionMiddle.SetData(new CSNumberData(ultimateGemInChannels.InPositionMiddle));
		OutPositionEnd.SetData(new CSNumberData(num9));
		OutChannelEnabled.SetData(new CSNumberData(ultimateGemInChannels.InChannelEnabled ? 1 : 0));
		OutChannelSlopeLength.SetData(new CSNumberData(num10));
		OutGemsClosedLoop.SetData(new CSNumberData(flag ? 1 : 0));
		OutDirection.SetData(new CSNumberData(direction));
		return base.Update();
	}

	private static void AdjustEndErrorSoGemSpacingLessGemTollerance(UltimateGemInChannels inChannels, List<UltimateGemData> gemList, double vLen, double positionStartGem, Curve profile, Transform xTrans, ValueListItem vOrientationMethod, ref double endError)
	{
		//IL_0023: Unknown result type (might be due to invalid IL or missing references)
		//IL_002d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0032: Unknown result type (might be due to invalid IL or missing references)
		//IL_0036: Unknown result type (might be due to invalid IL or missing references)
		//IL_0049: Unknown result type (might be due to invalid IL or missing references)
		//IL_0052: Unknown result type (might be due to invalid IL or missing references)
		//IL_0057: Unknown result type (might be due to invalid IL or missing references)
		//IL_005b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0065: Unknown result type (might be due to invalid IL or missing references)
		//IL_0066: Unknown result type (might be due to invalid IL or missing references)
		//IL_0067: Unknown result type (might be due to invalid IL or missing references)
		//IL_006c: Unknown result type (might be due to invalid IL or missing references)
		if (gemList.Count >= 1)
		{
			UltimateGemData ultimateGemData = gemList[gemList.Count - 1].Duplicate();
			CalculateGemXform(inChannels, profile, xTrans, vOrientationMethod, ultimateGemData);
			Point3d origin = Point3d.Origin;
			((Point3d)(ref origin)).Transform(ultimateGemData.GemTransform);
			MoveAGem(inChannels, ultimateGemData, positionStartGem, endError, vLen, profile, xTrans, vOrientationMethod);
			Point3d origin2 = Point3d.Origin;
			((Point3d)(ref origin2)).Transform(ultimateGemData.GemTransform);
			Vector3d val = origin2 - origin;
			double length = ((Vector3d)(ref val)).Length;
			double num = gemList.Count - 1;
			if (num == 0.0)
			{
				num = 1.0;
			}
			length /= num;
			if (length > inChannels.InSpacingTollerance)
			{
				endError *= inChannels.InSpacingTollerance / length;
			}
		}
	}

	private static void AdjustGemPosEnd(UltimateGemInChannels inChannels, double positionStartGem, double positionEndGem, Curve profile, Transform xTrans, ValueListItem vOrientationMethod, ref List<UltimateGemData> gemList, ref int direction)
	{
		//IL_00ac: Unknown result type (might be due to invalid IL or missing references)
		//IL_0040: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d3: Unknown result type (might be due to invalid IL or missing references)
		if (gemList.Count <= 1)
		{
			return;
		}
		double paramPosition = positionEndGem;
		if (inChannels.InCurve.IsClosed)
		{
			direction = -direction;
			CalcLastLoopingGemPos(inChannels, gemList[0], gemList[gemList.Count - 1], positionEndGem, profile, xTrans, direction, vOrientationMethod, out paramPosition);
			if (positionEndGem != paramPosition)
			{
				double num = (paramPosition - gemList[gemList.Count - 1].NormParam) / (double)(gemList.Count - 1);
				paramPosition -= num;
			}
			direction = -direction;
		}
		double normParam = gemList[gemList.Count - 1].NormParam;
		double endError = paramPosition - normParam;
		double num2 = normParam - positionStartGem;
		AdjustEndErrorSoGemSpacingLessGemTollerance(inChannels, gemList, num2, positionStartGem, profile, xTrans, vOrientationMethod, ref endError);
		foreach (UltimateGemData gem in gemList)
		{
			MoveAGem(inChannels, gem, positionStartGem, endError, num2, profile, xTrans, vOrientationMethod);
		}
	}

	private static void AdjustGemPosEndMirror(UltimateGemInChannels inChannels, double positionStartGem, double positionEndGem, Curve profile, Transform xTrans, int direction, ValueListItem vOrientationMethod, ref List<UltimateGemData> gemList, ref List<UltimateGemData> gemListMirror)
	{
		//IL_003e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0095: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a6: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b1: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b8: Unknown result type (might be due to invalid IL or missing references)
		//IL_00bd: Unknown result type (might be due to invalid IL or missing references)
		//IL_00bf: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c1: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c6: Unknown result type (might be due to invalid IL or missing references)
		//IL_0158: Unknown result type (might be due to invalid IL or missing references)
		//IL_0197: Unknown result type (might be due to invalid IL or missing references)
		if (gemList.Count <= 1 && gemListMirror.Count <= 0)
		{
			return;
		}
		double normParam = gemList[gemList.Count - 1].NormParam;
		double endError = positionEndGem - normParam;
		double num = normParam - positionStartGem;
		AdjustEndErrorSoGemSpacingLessGemTollerance(inChannels, gemList, num, positionStartGem, profile, xTrans, vOrientationMethod, ref endError);
		double endError2 = endError;
		double gemRowLength = num;
		if (gemList.Count > 1)
		{
			UltimateGemData ultimateGemData = gemList[gemList.Count - 1].Duplicate();
			UltimateGemData ultimateGemData2 = gemListMirror[gemListMirror.Count - 1].Duplicate();
			MoveAGem(inChannels, ultimateGemData, positionStartGem, endError, num, profile, xTrans, vOrientationMethod);
			MoveAGem(inChannels, ultimateGemData2, positionStartGem, endError2, gemRowLength, profile, xTrans, vOrientationMethod);
			Point3d gemPoint = ultimateGemData.GemPoint;
			Point3d gemPoint2 = ultimateGemData2.GemPoint;
			Vector3d val = gemPoint - gemPoint2;
			if (((Vector3d)(ref val)).Length - ultimateGemData.SizeXRotated / 2.0 - ultimateGemData2.SizeXRotated / 2.0 - inChannels.InSpacing * 0.8 < 0.0)
			{
				endError = positionStartGem + 0.5 * (double)direction - normParam;
				num = normParam - positionStartGem;
				endError2 = endError;
				gemRowLength = num;
				gemList.RemoveAt(gemList.Count - 1);
			}
		}
		foreach (UltimateGemData gem in gemList)
		{
			MoveAGem(inChannels, gem, positionStartGem, endError, num, profile, xTrans, vOrientationMethod);
		}
		foreach (UltimateGemData item in gemListMirror)
		{
			MoveAGem(inChannels, item, positionStartGem, endError2, gemRowLength, profile, xTrans, vOrientationMethod);
		}
	}

	private static void CalcGemPos(UltimateGemInChannels inChannels, double paramPosition, double gemRotatedStartXSize, double gemRotatedMiddleXSize, double gemRotatedEndXSize, double xStart, double xMiddle, double xEnd, double yStart, double yMiddle, double yEnd, double placementStart, double placementMiddle, double placementEnd, bool doMirror, bool mirrorModeEven, double positionStartGem, double positionEndGem, Curve profile, Transform xTrans, double direction, ValueListItem vOrientationMethod, out List<UltimateGemData> gemList, out List<UltimateGemData> gemListMirror)
	{
		//IL_0010: Unknown result type (might be due to invalid IL or missing references)
		//IL_0015: Unknown result type (might be due to invalid IL or missing references)
		//IL_0016: Unknown result type (might be due to invalid IL or missing references)
		//IL_001b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0237: Unknown result type (might be due to invalid IL or missing references)
		//IL_023c: Unknown result type (might be due to invalid IL or missing references)
		//IL_008a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0253: Unknown result type (might be due to invalid IL or missing references)
		//IL_0254: Unknown result type (might be due to invalid IL or missing references)
		//IL_0142: Unknown result type (might be due to invalid IL or missing references)
		//IL_0147: Unknown result type (might be due to invalid IL or missing references)
		//IL_0148: Unknown result type (might be due to invalid IL or missing references)
		//IL_014d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0260: Unknown result type (might be due to invalid IL or missing references)
		//IL_0261: Unknown result type (might be due to invalid IL or missing references)
		//IL_0111: Unknown result type (might be due to invalid IL or missing references)
		//IL_011e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0123: Unknown result type (might be due to invalid IL or missing references)
		//IL_0124: Unknown result type (might be due to invalid IL or missing references)
		//IL_0125: Unknown result type (might be due to invalid IL or missing references)
		//IL_0496: Unknown result type (might be due to invalid IL or missing references)
		//IL_04ad: Unknown result type (might be due to invalid IL or missing references)
		//IL_04b2: Unknown result type (might be due to invalid IL or missing references)
		//IL_04b3: Unknown result type (might be due to invalid IL or missing references)
		//IL_04b4: Unknown result type (might be due to invalid IL or missing references)
		//IL_04b9: Unknown result type (might be due to invalid IL or missing references)
		//IL_0439: Unknown result type (might be due to invalid IL or missing references)
		//IL_043e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0353: Unknown result type (might be due to invalid IL or missing references)
		//IL_0360: Unknown result type (might be due to invalid IL or missing references)
		//IL_0365: Unknown result type (might be due to invalid IL or missing references)
		//IL_0366: Unknown result type (might be due to invalid IL or missing references)
		//IL_036b: Unknown result type (might be due to invalid IL or missing references)
		gemList = new List<UltimateGemData>();
		gemListMirror = new List<UltimateGemData>();
		Point3d val = Point3d.Unset;
		Point3d val2 = Point3d.Unset;
		double num = 0.0;
		double length = inChannels.InCurve.GetLength();
		double num2 = paramPosition;
		double num3 = paramPosition;
		bool flag = false;
		while (paramPosition * direction < positionEndGem * direction && !flag)
		{
			double num4 = 999.0;
			int num5 = 20;
			UltimateGemData ultimateGemData = null;
			UltimateGemData ultimateGemData2 = null;
			Vector3d val3;
			while (num4 > 0.01 && num5 > 0)
			{
				num5--;
				ultimateGemData = CalcGemSize(inChannels, paramPosition, gemRotatedStartXSize, gemRotatedMiddleXSize, gemRotatedEndXSize, xStart, xMiddle, xEnd, yStart, yMiddle, yEnd, placementStart, placementMiddle, placementEnd, vOrientationMethod, positionStartGem, positionEndGem, profile);
				CalculateGemXform(inChannels, profile, xTrans, vOrientationMethod, ultimateGemData);
				if (gemList.Count == 0)
				{
					if (!(doMirror && mirrorModeEven))
					{
						num4 = 0.0;
						continue;
					}
					ultimateGemData2 = new UltimateGemData
					{
						NormParam = num3 - (paramPosition - num3),
						SizeX = ultimateGemData.SizeX,
						SizeY = ultimateGemData.SizeY,
						SizeZ = ultimateGemData.SizeZ,
						ZPlacement = ultimateGemData.ZPlacement,
						Spacing = ultimateGemData.Spacing,
						SizeXRotated = ultimateGemData.SizeXRotated
					};
					CalculateGemXform(inChannels, profile, xTrans, vOrientationMethod, ultimateGemData2);
					val = ultimateGemData2.GemPoint;
					val2 = val;
					num = ultimateGemData.SizeXRotated;
				}
				val3 = ultimateGemData.GemPoint - val;
				double length2 = ((Vector3d)(ref val3)).Length;
				length2 = length2 - ultimateGemData.SizeXRotated / 2.0 - num / 2.0;
				num4 = Math.Abs(length2 - ultimateGemData.Spacing);
				if (!(num4 > 0.01))
				{
					continue;
				}
				double num6 = (inChannels.InSpacing - length2) * direction;
				if (doMirror && mirrorModeEven && gemList.Count == 0)
				{
					num6 /= 2.0;
				}
				paramPosition += num6 / length * 0.8;
				if (!inChannels.InCurve.IsClosed)
				{
					if (paramPosition > 1.0)
					{
						paramPosition = 1.0;
					}
					if (paramPosition < 0.0)
					{
						paramPosition = 0.0;
					}
				}
			}
			if (ultimateGemData != null)
			{
				gemList.Add(ultimateGemData);
				val = ultimateGemData.GemPoint;
			}
			if (doMirror)
			{
				num4 = 999.0;
				num5 = 20;
				if (val2 == Point3d.Unset)
				{
					val2 = val;
				}
				if (gemList.Count <= 1)
				{
					num4 = 0.0;
				}
				if (inChannels.InCurve.IsClosed || (num2 < 1.01 && num2 > -0.01))
				{
					if (!inChannels.InCurve.IsClosed)
					{
						if (num2 > 1.0)
						{
							num2 = 1.0;
						}
						if (num2 < 0.0)
						{
							num2 = 0.0;
						}
					}
					while (num4 > 0.01 && num5 > 0)
					{
						num5--;
						ultimateGemData2 = new UltimateGemData
						{
							NormParam = num2,
							SizeX = ultimateGemData.SizeX,
							SizeY = ultimateGemData.SizeY,
							SizeZ = ultimateGemData.SizeZ,
							ZPlacement = ultimateGemData.ZPlacement,
							Spacing = ultimateGemData.Spacing,
							SizeXRotated = ultimateGemData.SizeXRotated
						};
						CalculateGemXform(inChannels, profile, xTrans, vOrientationMethod, ultimateGemData2);
						val3 = ultimateGemData2.GemPoint - val2;
						double length3 = ((Vector3d)(ref val3)).Length;
						length3 = length3 - ultimateGemData2.SizeXRotated / 2.0 - num / 2.0;
						num4 = Math.Abs(length3 - ultimateGemData2.Spacing);
						if (!(num4 > 0.01))
						{
							continue;
						}
						double num7 = (inChannels.InSpacing - length3) * direction;
						num2 -= num7 / length * 0.8;
						if (!inChannels.InCurve.IsClosed)
						{
							if (num2 > 1.0)
							{
								num2 = 1.0;
							}
							if (num2 < 0.0)
							{
								num2 = 0.0;
							}
						}
					}
					if (ultimateGemData2 != null)
					{
						gemListMirror.Add(ultimateGemData2);
						val2 = ultimateGemData2.GemPoint;
					}
				}
			}
			num = ultimateGemData.SizeXRotated;
			double num8 = num + inChannels.InSpacing;
			paramPosition += num8 / length * direction;
			num2 -= num8 / length * direction;
			if (!doMirror && inChannels.InCurve.IsClosed && gemList.Count >= 3)
			{
				Point3d gemPoint = gemList[0].GemPoint;
				val = gemList[gemList.Count - 1].GemPoint;
				val3 = gemPoint - val;
				if (((Vector3d)(ref val3)).Length - gemList[0].SizeXRotated / 2.0 - gemList[gemList.Count - 1].SizeXRotated / 2.0 - inChannels.InSpacing < 0.0)
				{
					gemList.RemoveAt(gemList.Count - 1);
					flag = true;
				}
			}
		}
	}

	private static UltimateGemData CalcGemSize(UltimateGemInChannels inChannels, double paramPosition, double gemRotatedStartXSize, double gemRotatedMiddleXSize, double gemRotatedEndXSize, double xStart, double xMiddle, double xEnd, double yStart, double yMiddle, double yEnd, double placementStart, double placementMiddle, double placementEnd, ValueListItem vOrientationMethod, double positionStartGem, double positionEndGem, Curve profile)
	{
		//IL_0118: Unknown result type (might be due to invalid IL or missing references)
		//IL_0129: Unknown result type (might be due to invalid IL or missing references)
		//IL_012e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0130: Unknown result type (might be due to invalid IL or missing references)
		//IL_0132: Unknown result type (might be due to invalid IL or missing references)
		//IL_0137: Unknown result type (might be due to invalid IL or missing references)
		//IL_0206: Unknown result type (might be due to invalid IL or missing references)
		//IL_0185: Unknown result type (might be due to invalid IL or missing references)
		UltimateGemData ultimateGemData = new UltimateGemData
		{
			NormParam = paramPosition
		};
		double num = (paramPosition - positionStartGem) / (positionEndGem - positionStartGem);
		double zPlacement;
		double num2;
		double num3;
		double num4;
		if (num <= inChannels.InPositionMiddle)
		{
			num /= inChannels.InPositionMiddle;
			zPlacement = placementStart * (1.0 - num) + placementMiddle * num;
			num2 = xStart * (1.0 - num) + xMiddle * num;
			num3 = yStart * (1.0 - num) + yMiddle * num;
			num4 = gemRotatedStartXSize * (1.0 - num) + gemRotatedMiddleXSize * num;
		}
		else
		{
			num = (num - inChannels.InPositionMiddle) / (1.0 - inChannels.InPositionMiddle);
			zPlacement = placementMiddle * (1.0 - num) + placementEnd * num;
			num2 = xMiddle * (1.0 - num) + xEnd * num;
			num3 = yMiddle * (1.0 - num) + yEnd * num;
			num4 = gemRotatedMiddleXSize * (1.0 - num) + gemRotatedEndXSize * num;
		}
		Transform xBestScale;
		if (inChannels.InUseMinCuletDistance && inChannels.InBaseCurve != null)
		{
			CalculatePointsOnCrv(inChannels.InCurve, inChannels.InBaseCurve, vOrientationMethod, ultimateGemData);
			Point3d val = inChannels.InCurve.PointAtNormalizedLength(paramPosition);
			Point3d val2 = inChannels.InBaseCurve.PointAt(ultimateGemData.BaseParam);
			Vector3d val3 = val - val2;
			double length = ((Vector3d)(ref val3)).Length;
			double num5 = length - inChannels.InMinCuletDistance;
			CSUtilGem.GetGemScaleFromTableWithMaxZ(inChannels.InGemShape, num2, num3, num5, useLargestPossible: true, useIterationOnly: false, out xBestScale);
			if (inChannels.InChannelEnabled && profile != null)
			{
				CSGemData cSGemData = inChannels.InGemShape.Duplicate() as CSGemData;
				cSGemData.ApplyTransform(xBestScale);
				ultimateGemData.SizeY = cSGemData.GemYSize;
				double num6 = ProfileCut(inChannels.InChannelWallGemOverlap, inChannels.InChannelWallWidth, profile, ultimateGemData);
				double num7 = length * (1.0 - num6);
				CSUtilGem.GetGemScaleFromTableWithMaxZ(inChannels.InGemShape, num2, num3, num5 - num7, useLargestPossible: true, useIterationOnly: false, out xBestScale);
			}
		}
		else
		{
			CSUtilGem.GetGemScaleFromTable(inChannels.InGemShape, num2, num3, useLargestPossible: true, useIterationOnly: false, out xBestScale);
		}
		if (inChannels.InGemShape.Duplicate() is CSGemData cSGemData2)
		{
			cSGemData2.ApplyTransform(xBestScale);
			double num8 = (cSGemData2.GemXSize + cSGemData2.GemYSize) / (num2 + num3);
			ultimateGemData.SizeX = cSGemData2.GemXSize;
			ultimateGemData.SizeY = cSGemData2.GemYSize;
			ultimateGemData.SizeZ = cSGemData2.GemZSize;
			ultimateGemData.SizeXRotated = num4 * num8;
			ultimateGemData.ZPlacement = zPlacement;
			ultimateGemData.Spacing = inChannels.InSpacing;
		}
		return ultimateGemData;
	}

	private static void CalcLastLoopingGemPos(UltimateGemInChannels inChannels, UltimateGemData firstGem, UltimateGemData lastGem, double neededGemPos, Curve profile, Transform xTrans, int direction, ValueListItem vOrientationMethod, out double paramPosition)
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0005: Unknown result type (might be due to invalid IL or missing references)
		//IL_0009: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a0: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ad: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b2: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b3: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b8: Unknown result type (might be due to invalid IL or missing references)
		Point3d origin = Point3d.Origin;
		((Point3d)(ref origin)).Transform(firstGem.GemTransform);
		double length = inChannels.InCurve.GetLength();
		double num = 999.0;
		int num2 = 20;
		paramPosition = neededGemPos;
		bool flag = true;
		while (num > 0.01 && num2 > 0)
		{
			num2--;
			UltimateGemData ultimateGemData = new UltimateGemData
			{
				NormParam = paramPosition,
				SizeX = lastGem.SizeX,
				SizeY = lastGem.SizeY,
				SizeZ = lastGem.SizeZ,
				ZPlacement = lastGem.ZPlacement,
				Spacing = lastGem.Spacing,
				SizeXRotated = lastGem.SizeXRotated
			};
			CalculateGemXform(inChannels, profile, xTrans, vOrientationMethod, ultimateGemData);
			Vector3d val = ultimateGemData.GemPoint - origin;
			double length2 = ((Vector3d)(ref val)).Length;
			length2 = length2 - ultimateGemData.SizeXRotated / 2.0 - firstGem.SizeXRotated / 2.0;
			if (flag)
			{
				if (length2 - ultimateGemData.Spacing > 0.0)
				{
					paramPosition = neededGemPos;
					break;
				}
				flag = false;
				paramPosition = lastGem.NormParam;
			}
			else
			{
				num = Math.Abs(length2 - ultimateGemData.Spacing);
				if (num > 0.01)
				{
					double num3 = (inChannels.InSpacing - length2) * (double)direction;
					paramPosition += num3 / length * 0.8;
				}
			}
		}
	}

	private static void CalculateGemXform(UltimateGemInChannels inChannels, Curve profile, Transform xTrans, ValueListItem vOrientationMethod, UltimateGemData gemOnCurveInfo)
	{
		//IL_004c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0054: Unknown result type (might be due to invalid IL or missing references)
		//IL_0059: Unknown result type (might be due to invalid IL or missing references)
		//IL_003b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0044: Unknown result type (might be due to invalid IL or missing references)
		//IL_0049: Unknown result type (might be due to invalid IL or missing references)
		//IL_0066: Unknown result type (might be due to invalid IL or missing references)
		//IL_0099: Unknown result type (might be due to invalid IL or missing references)
		//IL_009e: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c7: Unknown result type (might be due to invalid IL or missing references)
		//IL_00cc: Unknown result type (might be due to invalid IL or missing references)
		//IL_00db: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e0: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b4: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b9: Unknown result type (might be due to invalid IL or missing references)
		//IL_01a2: Unknown result type (might be due to invalid IL or missing references)
		//IL_0180: Unknown result type (might be due to invalid IL or missing references)
		//IL_0125: Unknown result type (might be due to invalid IL or missing references)
		//IL_0127: Unknown result type (might be due to invalid IL or missing references)
		//IL_0128: Unknown result type (might be due to invalid IL or missing references)
		//IL_012d: Unknown result type (might be due to invalid IL or missing references)
		//IL_014c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0156: Unknown result type (might be due to invalid IL or missing references)
		//IL_0158: Unknown result type (might be due to invalid IL or missing references)
		//IL_01a7: Unknown result type (might be due to invalid IL or missing references)
		//IL_01ee: Unknown result type (might be due to invalid IL or missing references)
		//IL_01cc: Unknown result type (might be due to invalid IL or missing references)
		//IL_01f3: Unknown result type (might be due to invalid IL or missing references)
		//IL_0222: Unknown result type (might be due to invalid IL or missing references)
		//IL_0227: Unknown result type (might be due to invalid IL or missing references)
		//IL_022f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0299: Unknown result type (might be due to invalid IL or missing references)
		//IL_029e: Unknown result type (might be due to invalid IL or missing references)
		//IL_023d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0266: Unknown result type (might be due to invalid IL or missing references)
		//IL_026b: Unknown result type (might be due to invalid IL or missing references)
		//IL_026f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0270: Unknown result type (might be due to invalid IL or missing references)
		//IL_02c6: Unknown result type (might be due to invalid IL or missing references)
		//IL_02cb: Unknown result type (might be due to invalid IL or missing references)
		//IL_02cd: Unknown result type (might be due to invalid IL or missing references)
		//IL_02cf: Unknown result type (might be due to invalid IL or missing references)
		//IL_02d1: Unknown result type (might be due to invalid IL or missing references)
		//IL_02d3: Unknown result type (might be due to invalid IL or missing references)
		//IL_02d5: Unknown result type (might be due to invalid IL or missing references)
		//IL_02b0: Unknown result type (might be due to invalid IL or missing references)
		//IL_02b5: Unknown result type (might be due to invalid IL or missing references)
		//IL_02b7: Unknown result type (might be due to invalid IL or missing references)
		//IL_02b9: Unknown result type (might be due to invalid IL or missing references)
		//IL_02bb: Unknown result type (might be due to invalid IL or missing references)
		//IL_02bd: Unknown result type (might be due to invalid IL or missing references)
		//IL_02bf: Unknown result type (might be due to invalid IL or missing references)
		//IL_0285: Unknown result type (might be due to invalid IL or missing references)
		//IL_0287: Unknown result type (might be due to invalid IL or missing references)
		//IL_0288: Unknown result type (might be due to invalid IL or missing references)
		//IL_027c: Unknown result type (might be due to invalid IL or missing references)
		//IL_027e: Unknown result type (might be due to invalid IL or missing references)
		//IL_02da: Unknown result type (might be due to invalid IL or missing references)
		//IL_02ea: Unknown result type (might be due to invalid IL or missing references)
		//IL_02ee: Unknown result type (might be due to invalid IL or missing references)
		//IL_02f3: Unknown result type (might be due to invalid IL or missing references)
		//IL_02f8: Unknown result type (might be due to invalid IL or missing references)
		//IL_02fc: Unknown result type (might be due to invalid IL or missing references)
		//IL_02fe: Unknown result type (might be due to invalid IL or missing references)
		//IL_0300: Unknown result type (might be due to invalid IL or missing references)
		//IL_0305: Unknown result type (might be due to invalid IL or missing references)
		//IL_0306: Unknown result type (might be due to invalid IL or missing references)
		//IL_030b: Unknown result type (might be due to invalid IL or missing references)
		//IL_030c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0311: Unknown result type (might be due to invalid IL or missing references)
		//IL_028d: Unknown result type (might be due to invalid IL or missing references)
		double num = gemOnCurveInfo.SizeX / inChannels.InGemShape.GemXSize;
		Transform val;
		if (gemOnCurveInfo.SizeY > 0.0)
		{
			double num2 = gemOnCurveInfo.SizeY / inChannels.InGemShape.GemYSize;
			val = Transform.Scale(Plane.WorldXY, num, num2, num);
		}
		else
		{
			val = Transform.Scale(Plane.WorldXY, num, num, num);
		}
		CSGemData cSGemData = new CSGemData(inChannels.InGemShape);
		cSGemData.ApplyTransform(val);
		double zOffset = 0.0;
		CSUtilGem.GetGemZOffset(cSGemData, inChannels.InPlacementMethod, ref zOffset);
		CalculatePointsOnCrv(inChannels.InCurve, inChannels.InBaseCurve, vOrientationMethod, gemOnCurveInfo);
		Point3d val2 = Point3d.Unset;
		if (inChannels.InBaseCurve != null)
		{
			val2 = inChannels.InBaseCurve.PointAt(gemOnCurveInfo.BaseParam);
		}
		Point3d val3 = inChannels.InCurve.PointAt(gemOnCurveInfo.Param);
		Vector3d val4 = inChannels.InCurve.TangentAt(gemOnCurveInfo.Param);
		double num3 = 0.0;
		if (inChannels.InChannelEnabled && profile != null)
		{
			double num4 = ProfileCut(inChannels.InChannelWallGemOverlap, inChannels.InChannelWallWidth, profile, gemOnCurveInfo);
			ProfilePositionScale(gemOnCurveInfo, inChannels.InCurve, inChannels.InBaseCurve, out var pScale, out var pRail);
			Vector3d val5 = val3 - val2;
			num3 = ((Vector3d)(ref val5)).Length * (1.0 - num4);
			((GeometryBase)gemOnCurveInfo.ProfileCurve).Transform(pScale);
			gemOnCurveInfo.ProfileTransform = pRail;
		}
		Vector3d val6 = (inChannels.InDoFlip ? new Vector3d(0.0, 0.0, -1.0) : new Vector3d(0.0, 0.0, 1.0));
		Vector3d val7 = (inChannels.InDoFlip ? new Vector3d(-1.0, 0.0, 0.0) : new Vector3d(1.0, 0.0, 0.0));
		Vector3d brepNormal = default(Vector3d);
		switch (vOrientationMethod)
		{
		case ValueListItem.GemOrientationMethodSurface:
			brepNormal = Vector3d.ZAxis;
			CSUtilGeometry.GetClosestNormalToBrep(inChannels.InBaseSurface, val3, ref brepNormal);
			break;
		case ValueListItem.GemOrientationMethodOrigin:
			((Vector3d)(ref brepNormal))._002Ector(val3);
			break;
		case ValueListItem.GemOrientationMethodAxisY:
			((Vector3d)(ref brepNormal))._002Ector(((Point3d)(ref val3)).X, 0.0, ((Point3d)(ref val3)).Z);
			break;
		case ValueListItem.GemOrientationMethodDown:
			brepNormal = Vector3d.ZAxis;
			break;
		case ValueListItem.OrientationMethodClosestPointOnCurve:
		case ValueListItem.OrientationMethodIntersectPointOnCurve:
			brepNormal = (Vector3d)((val2 != Point3d.Unset) ? (val3 - val2) : new Vector3d(val3));
			((Vector3d)(ref brepNormal)).Unitize();
			break;
		default:
			brepNormal = Vector3d.ZAxis;
			break;
		}
		((Vector3d)(ref brepNormal)).Unitize();
		Transform val8 = (inChannels.InOrientationFollowCurve ? CSBlockUtil.CSUtilMath.Orient3Point(Point3d.Origin, val7, val6, val3, val4, brepNormal) : CSBlockUtil.CSUtilMath.Orient3Point(Point3d.Origin, val6, val7, val3, brepNormal, val4));
		double num5 = zOffset + gemOnCurveInfo.ZPlacement - num3;
		Transform val9 = Transform.Translation(brepNormal * num5);
		gemOnCurveInfo.GemTransform = val9 * val8 * xTrans * val;
	}

	private static void CalculatePointsOnCrv(Curve crv, Curve baseCrv, ValueListItem vOrientationMethod, UltimateGemData gem)
	{
		//IL_0050: Unknown result type (might be due to invalid IL or missing references)
		//IL_0055: Unknown result type (might be due to invalid IL or missing references)
		//IL_005d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0062: Unknown result type (might be due to invalid IL or missing references)
		//IL_008f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0090: Unknown result type (might be due to invalid IL or missing references)
		//IL_0097: Unknown result type (might be due to invalid IL or missing references)
		//IL_007f: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c5: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ca: Unknown result type (might be due to invalid IL or missing references)
		//IL_00cb: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d0: Unknown result type (might be due to invalid IL or missing references)
		double num;
		for (num = gem.NormParam; num > 1.0; num -= 1.0)
		{
		}
		for (; num < 0.0; num += 1.0)
		{
		}
		crv.NormalizedLengthParameter(num, ref gem.Param);
		Point3d val = crv.PointAt(gem.Param);
		Vector3d val2 = crv.TangentAt(gem.Param);
		gem.BaseParam = 0.0;
		if (baseCrv == null)
		{
			return;
		}
		if (vOrientationMethod == ValueListItem.OrientationMethodClosestPointOnCurve)
		{
			baseCrv.ClosestPoint(val, ref gem.BaseParam);
			return;
		}
		Plane val3 = default(Plane);
		((Plane)(ref val3))._002Ector(val, val2);
		CurveIntersections obj = Intersection.CurvePlane(baseCrv, val3, 0.001);
		double num2 = 9999.0;
		foreach (IntersectionEvent item in obj)
		{
			Vector3d val4 = item.PointA - val;
			double length = ((Vector3d)(ref val4)).Length;
			if (length < num2)
			{
				num2 = length;
				gem.BaseParam = item.ParameterA;
			}
		}
	}

	private static bool CalculateRotateTestGem(CSGemData gemShape, double xSize, double ySize, double rollAngle, out double xSizeOut)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_0006: Unknown result type (might be due to invalid IL or missing references)
		//IL_000b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0010: Unknown result type (might be due to invalid IL or missing references)
		//IL_0034: Unknown result type (might be due to invalid IL or missing references)
		//IL_0035: Unknown result type (might be due to invalid IL or missing references)
		//IL_0036: Unknown result type (might be due to invalid IL or missing references)
		//IL_006a: Unknown result type (might be due to invalid IL or missing references)
		//IL_006f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0075: Unknown result type (might be due to invalid IL or missing references)
		//IL_007a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0085: Unknown result type (might be due to invalid IL or missing references)
		//IL_008a: Unknown result type (might be due to invalid IL or missing references)
		//IL_009c: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ae: Unknown result type (might be due to invalid IL or missing references)
		//IL_00cc: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d1: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d4: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d9: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e6: Unknown result type (might be due to invalid IL or missing references)
		//IL_00eb: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f6: Unknown result type (might be due to invalid IL or missing references)
		//IL_00fb: Unknown result type (might be due to invalid IL or missing references)
		//IL_013f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0169: Unknown result type (might be due to invalid IL or missing references)
		//IL_016e: Unknown result type (might be due to invalid IL or missing references)
		//IL_018b: Expected O, but got Unknown
		//IL_01bf: Unknown result type (might be due to invalid IL or missing references)
		//IL_01c4: Unknown result type (might be due to invalid IL or missing references)
		Transform val = CSBlockUtil.CSUtilMath.Rotation(rollAngle, Vector3d.ZAxis, Point3d.Origin);
		if (!CSUtilGem.GetGemScaleFromTable(gemShape, xSize, ySize, useLargestPossible: true, useIterationOnly: false, out Transform xBestScale))
		{
			xSizeOut = 0.0;
			return false;
		}
		CSGemData cSGemData = new CSGemData(gemShape);
		cSGemData.ApplyTransform(val * xBestScale);
		Curve curve = cSGemData.GetCurve();
		NurbsCurve val2 = (NurbsCurve)(object)((curve is NurbsCurve) ? curve : null);
		if (val2 == null)
		{
			xSizeOut = 0.0;
			return false;
		}
		Point3d val4;
		for (int i = 0; i < val2.Points.Count; i++)
		{
			ControlPoint val3 = val2.Points[i];
			val4 = ((ControlPoint)(ref val3)).Location;
			double x = ((Point3d)(ref val4)).X;
			val4 = ((ControlPoint)(ref val3)).Location;
			((ControlPoint)(ref val3)).Location = new Point3d(x, ((Point3d)(ref val4)).Y, 0.0);
			val2.Points[i] = val3;
		}
		BoundingBox boundingBox = ((GeometryBase)val2).GetBoundingBox(true);
		val4 = ((BoundingBox)(ref boundingBox)).Min;
		double y = ((Point3d)(ref val4)).Y;
		val4 = ((BoundingBox)(ref boundingBox)).Max;
		double y2 = ((Point3d)(ref val4)).Y;
		val4 = ((BoundingBox)(ref boundingBox)).Min;
		double num = y2 - ((Point3d)(ref val4)).Y;
		double num2 = 0.0;
		for (int j = 1; j < 20; j++)
		{
			CurveIntersections val5 = Intersection.CurveCurve((Curve)new LineCurve(new Point3d(20.0, y + num * (double)j / 20.0, 0.0), new Point3d(-20.0, y + num * (double)j / 20.0, 0.0)), (Curve)(object)val2, 0.001, 0.001);
			if (val5.Count < 2)
			{
				continue;
			}
			double num3 = 999.0;
			double num4 = -999.0;
			foreach (IntersectionEvent item in val5)
			{
				Point3d pointA = item.PointA;
				if (((Point3d)(ref pointA)).X > num4)
				{
					num4 = ((Point3d)(ref pointA)).X;
				}
				if (((Point3d)(ref pointA)).X < num3)
				{
					num3 = ((Point3d)(ref pointA)).X;
				}
			}
			if (num4 - num3 > num2)
			{
				num2 = num4 - num3;
			}
		}
		xSizeOut = num2;
		return true;
	}

	private static void MoveAGem(UltimateGemInChannels inChannels, UltimateGemData gemPos, double positionStartGem, double endError, double gemRowLength, Curve profile, Transform xTrans, ValueListItem vOrientationMethod)
	{
		//IL_001f: Unknown result type (might be due to invalid IL or missing references)
		double num = (gemPos.NormParam - positionStartGem) * endError / gemRowLength;
		gemPos.NormParam += num;
		CalculateGemXform(inChannels, profile, xTrans, vOrientationMethod, gemPos);
	}

	private static double ProfileCut(double channelWallGemOverlap, double wallWidth, Curve profile, UltimateGemData gemOnCurveInfo)
	{
		//IL_0027: Unknown result type (might be due to invalid IL or missing references)
		//IL_004c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0051: Unknown result type (might be due to invalid IL or missing references)
		//IL_0057: Expected O, but got Unknown
		//IL_0076: Unknown result type (might be due to invalid IL or missing references)
		//IL_009a: Unknown result type (might be due to invalid IL or missing references)
		//IL_009f: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a5: Expected O, but got Unknown
		//IL_00b1: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d0: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d5: Unknown result type (might be due to invalid IL or missing references)
		//IL_00dc: Unknown result type (might be due to invalid IL or missing references)
		//IL_0140: Unknown result type (might be due to invalid IL or missing references)
		//IL_0145: Unknown result type (might be due to invalid IL or missing references)
		//IL_01a0: Unknown result type (might be due to invalid IL or missing references)
		//IL_01a5: Unknown result type (might be due to invalid IL or missing references)
		double sizeY = gemOnCurveInfo.SizeY;
		LineCurve val = new LineCurve(new Point3d((0.0 - sizeY) / 2.0 + channelWallGemOverlap, 2.0, 0.0), new Point3d((0.0 - sizeY) / 2.0 + channelWallGemOverlap, -2.0, 0.0));
		LineCurve val2 = new LineCurve(new Point3d(sizeY / 2.0 - channelWallGemOverlap, 2.0, 0.0), new Point3d(sizeY / 2.0 - channelWallGemOverlap, -2.0, 0.0));
		gemOnCurveInfo.ProfileCurve = profile.DuplicateCurve();
		Transform val3 = Transform.Scale(Plane.WorldXY, wallWidth + gemOnCurveInfo.SizeY, 1.0, 1.0);
		((GeometryBase)gemOnCurveInfo.ProfileCurve).Transform(val3);
		CurveIntersections obj = Intersection.CurveCurve(gemOnCurveInfo.ProfileCurve, (Curve)(object)val, 0.001, 0.001);
		CurveIntersections val4 = Intersection.CurveCurve(gemOnCurveInfo.ProfileCurve, (Curve)(object)val2, 0.001, 0.001);
		double num = -1.0;
		foreach (IntersectionEvent item in obj)
		{
			Point3d pointA = item.PointA;
			if (((Point3d)(ref pointA)).Y > num)
			{
				num = ((Point3d)(ref pointA)).Y;
				gemOnCurveInfo.ParamLeft = item.ParameterA;
			}
		}
		double num2 = -1.0;
		foreach (IntersectionEvent item2 in val4)
		{
			Point3d pointA2 = item2.PointA;
			if (((Point3d)(ref pointA2)).Y > num2)
			{
				num2 = ((Point3d)(ref pointA2)).Y;
				gemOnCurveInfo.ParamRight = item2.ParameterA;
			}
		}
		return (num + num2) / 2.0;
	}

	private static void ProfilePositionScale(UltimateGemData gem, Curve crv, Curve baseCrv, out Transform pScale, out Transform pRail)
	{
		//IL_0007: Unknown result type (might be due to invalid IL or missing references)
		//IL_000c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0014: Unknown result type (might be due to invalid IL or missing references)
		//IL_0019: Unknown result type (might be due to invalid IL or missing references)
		//IL_0021: Unknown result type (might be due to invalid IL or missing references)
		//IL_0026: Unknown result type (might be due to invalid IL or missing references)
		//IL_0027: Unknown result type (might be due to invalid IL or missing references)
		//IL_0028: Unknown result type (might be due to invalid IL or missing references)
		//IL_0029: Unknown result type (might be due to invalid IL or missing references)
		//IL_002e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0039: Unknown result type (might be due to invalid IL or missing references)
		//IL_0052: Unknown result type (might be due to invalid IL or missing references)
		//IL_0057: Unknown result type (might be due to invalid IL or missing references)
		//IL_0072: Unknown result type (might be due to invalid IL or missing references)
		//IL_0077: Unknown result type (might be due to invalid IL or missing references)
		//IL_007c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0081: Unknown result type (might be due to invalid IL or missing references)
		//IL_0082: Unknown result type (might be due to invalid IL or missing references)
		//IL_0083: Unknown result type (might be due to invalid IL or missing references)
		//IL_0084: Unknown result type (might be due to invalid IL or missing references)
		//IL_0089: Unknown result type (might be due to invalid IL or missing references)
		Point3d val = crv.PointAt(gem.Param);
		Vector3d destination = crv.TangentAt(gem.Param);
		Point3d val2 = baseCrv.PointAt(gem.BaseParam);
		Vector3d destination2 = val - val2;
		double length = ((Vector3d)(ref destination2)).Length;
		pScale = Transform.Scale(Plane.WorldXY, 1.0, length, 1.0);
		pRail = CSBlockUtil.CSUtilMath.Orient3Point(new Point3d(0.0, length, 0.0), Vector3d.YAxis, Vector3d.ZAxis, val, destination2, destination);
	}
}
