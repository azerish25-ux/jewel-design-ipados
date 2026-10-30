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
		foreach (CSInputChannel inputChannel in base.InputChannels)
		{
			if (!inputChannel.CheckInputValid())
			{
				return UpdateMissing(inputChannel.Identifier + " is missing inputs");
			}
		}
		UltimateGemInChannels ultimateGemInChannels = new UltimateGemInChannels(this);
		Curve inProfile = ultimateGemInChannels.InProfile;
		if (inProfile != null)
		{
			BoundingBox boundingBox = inProfile.GetBoundingBox(accurate: true);
			double x = boundingBox.Diagonal.X;
			double y = boundingBox.Diagonal.Y;
			Transform xform = Transform.Translation((0.0 - (boundingBox.Max.X + boundingBox.Min.X)) / 2.0, 0.0 - boundingBox.Min.Y, 0.0);
			inProfile.Transform(xform);
			Transform xform2 = Transform.Scale(Plane.WorldXY, 1.0 / x, 1.0 / y, 1.0);
			inProfile.Transform(xform2);
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
		double num = ultimateGemInChannels.InDiameterMiddle.GemXSize;
		double num2 = ultimateGemInChannels.InDiameterEnd.GemXSize;
		double gemYSize = ultimateGemInChannels.InDiameterStart.GemYSize;
		double num3 = ultimateGemInChannels.InDiameterMiddle.GemYSize;
		double num4 = ultimateGemInChannels.InDiameterEnd.GemYSize;
		if (!ultimateGemInChannels.InEnableDiameterEnd)
		{
			num2 = gemXSize;
			num4 = gemYSize;
		}
		if (!ultimateGemInChannels.InEnableDiameterMiddle)
		{
			num = gemXSize + (num2 - gemXSize) * ultimateGemInChannels.InPositionMiddle;
			num3 = gemYSize + (num4 - gemYSize) * ultimateGemInChannels.InPositionMiddle;
		}
		double inPlacementStart = ultimateGemInChannels.InPlacementStart;
		double placementMiddle = ultimateGemInChannels.InPlacementMiddle;
		double num5 = ultimateGemInChannels.InPlacementEnd;
		if (!ultimateGemInChannels.InEnablePlacementEnd)
		{
			num5 = inPlacementStart;
		}
		if (!ultimateGemInChannels.InEnablePlacementMiddle)
		{
			placementMiddle = inPlacementStart + (num5 - inPlacementStart) * ultimateGemInChannels.InPositionMiddle;
		}
		double num6 = CSBlockUtil.CSUtilMath.ToRadians(ultimateGemInChannels.InRoll);
		double angleRadians = CSBlockUtil.CSUtilMath.ToRadians(ultimateGemInChannels.InTilt);
		Transform transform = CSBlockUtil.CSUtilMath.Rotation(num6, Vector3d.ZAxis, Point3d.Origin);
		Transform transform2 = CSBlockUtil.CSUtilMath.Rotation(angleRadians, Vector3d.XAxis, Point3d.Origin);
		Transform xTrans = transform * transform2;
		if (!CalculateRotateTestGem(ultimateGemInChannels.InGemShape, gemXSize, gemYSize, num6, out var xSizeOut))
		{
			UpdateError("GetGemScaleFromX failed");
		}
		if (!CalculateRotateTestGem(ultimateGemInChannels.InGemShape, num, num3, num6, out var xSizeOut2))
		{
			UpdateError("GetGemScaleFromX failed");
		}
		if (!CalculateRotateTestGem(ultimateGemInChannels.InGemShape, num2, num4, num6, out var xSizeOut3))
		{
			UpdateError("GetGemScaleFromX failed");
		}
		double num7 = ((!ultimateGemInChannels.InOverrideStartPosition) ? ultimateGemInChannels.InPositionStart : ultimateGemInChannels.InOverrideStart);
		double num8 = ((!ultimateGemInChannels.InOverrideEndPosition) ? ultimateGemInChannels.InPositionEnd : ultimateGemInChannels.InOverrideEnd);
		int direction = ((!(num7 > num8)) ? 1 : (-1));
		if (ultimateGemInChannels.InCurve.IsClosed && ultimateGemInChannels.InDoReverseDirection)
		{
			num7 += (double)direction;
			direction = -direction;
		}
		double num9 = ultimateGemInChannels.InChannelStartDistance;
		bool flag = ultimateGemInChannels.InChannelClosed;
		double length = ultimateGemInChannels.InCurve.GetLength();
		if (ultimateGemInChannels.InDoMirror && ultimateGemInChannels.InCurve.IsClosed)
		{
			double num10 = num8 - num7;
			if (num10 > 0.499)
			{
				num8 = num7 + 0.5;
				flag = true;
				num9 = 0.0;
			}
			else if (num10 < -0.499)
			{
				num8 = num7 - 0.5;
				flag = true;
				num9 = 0.0;
			}
		}
		double num11 = num7;
		if (!ultimateGemInChannels.InDoMirror && ultimateGemInChannels.InChannelEnabled && ultimateGemInChannels.InMoveGems)
		{
			num11 += (gemXSize / 2.0 + num9) / length * (double)direction;
		}
		double num12 = num8;
		if (ultimateGemInChannels.InChannelEnabled && ultimateGemInChannels.InMoveGems)
		{
			num12 -= (num2 / 2.0 + num9) / length * (double)direction;
		}
		double paramPosition = num11;
		List<UltimateGemData> gemList;
		if (!ultimateGemInChannels.InDoMirror)
		{
			CalcGemPos(ultimateGemInChannels, paramPosition, xSizeOut, xSizeOut2, xSizeOut3, gemXSize, num, num2, gemYSize, num3, num4, inPlacementStart, placementMiddle, num5, doMirror: false, mirrorModeEven: false, num11, num12, inProfile, xTrans, direction, valueListItem, out gemList, out var _);
			if (ultimateGemInChannels.InSpacingMethod == ValueListItem.GemSpacingMethodMinimum)
			{
				AdjustGemPosEnd(ultimateGemInChannels, num11, num12, inProfile, xTrans, valueListItem, ref gemList, ref direction);
			}
		}
		else
		{
			List<UltimateGemData> gemList2 = new List<UltimateGemData>();
			List<UltimateGemData> gemListMirror2 = new List<UltimateGemData>();
			double num13 = 999.0;
			double num14 = 999.0;
			if (ultimateGemInChannels.InMirrorMode == ValueListItem.GemOnCrvMirrorEven || ultimateGemInChannels.InMirrorMode == ValueListItem.GemOnCrvMirrorOddAndEven)
			{
				CalcGemPos(ultimateGemInChannels, paramPosition, xSizeOut, xSizeOut2, xSizeOut3, gemXSize, num, num2, gemYSize, num3, num4, inPlacementStart, placementMiddle, num5, doMirror: true, mirrorModeEven: true, num11, num12, inProfile, xTrans, direction, valueListItem, out gemList2, out gemListMirror2);
				if (ultimateGemInChannels.InSpacingMethod == ValueListItem.GemSpacingMethodMinimum)
				{
					AdjustGemPosEndMirror(ultimateGemInChannels, num11, num12, inProfile, xTrans, direction, valueListItem, ref gemList2, ref gemListMirror2);
				}
				int count = gemList2.Count;
				if (count > 1)
				{
					num13 = (gemList2[0].GemPoint - gemList2[1].GemPoint).Length - gemList2[0].SizeXRotated / 2.0 - gemList2[1].SizeXRotated / 2.0;
				}
				if (count == 1 && gemListMirror2.Count == 1)
				{
					num13 = (gemList2[0].GemPoint - gemListMirror2[0].GemPoint).Length - gemList2[0].SizeXRotated / 2.0 - gemListMirror2[0].SizeXRotated / 2.0;
				}
				if (count >= 1)
				{
					num14 = Math.Abs(num12 - gemList2[count - 1].NormParam);
				}
				if (num13 < 0.0)
				{
					num13 = 999.0;
					num14 = 999.0;
				}
			}
			List<UltimateGemData> gemList3 = new List<UltimateGemData>();
			List<UltimateGemData> gemListMirror3 = new List<UltimateGemData>();
			double num15 = 999.0;
			double num16 = 999.0;
			if (ultimateGemInChannels.InMirrorMode == ValueListItem.GemOnCrvMirrorOdd || ultimateGemInChannels.InMirrorMode == ValueListItem.GemOnCrvMirrorOddAndEven)
			{
				CalcGemPos(ultimateGemInChannels, paramPosition, xSizeOut, xSizeOut2, xSizeOut3, gemXSize, num, num2, gemYSize, num3, num4, inPlacementStart, placementMiddle, num5, doMirror: true, mirrorModeEven: false, num11, num12, inProfile, xTrans, direction, valueListItem, out gemList3, out gemListMirror3);
				if (ultimateGemInChannels.InSpacingMethod == ValueListItem.GemSpacingMethodMinimum)
				{
					AdjustGemPosEndMirror(ultimateGemInChannels, num11, num12, inProfile, xTrans, direction, valueListItem, ref gemList3, ref gemListMirror3);
				}
				int count2 = gemList3.Count;
				if (count2 > 1)
				{
					num15 = (gemList3[0].GemPoint - gemList3[1].GemPoint).Length - gemList3[0].SizeXRotated / 2.0 - gemList3[1].SizeXRotated / 2.0;
				}
				if (count2 >= 1)
				{
					num16 = Math.Abs(num12 - gemList3[count2 - 1].NormParam);
				}
			}
			List<UltimateGemData> gemListMirror;
			if (num16 < 0.01 && num14 < 0.01)
			{
				if (num15 <= num13)
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
			else if (num16 <= num14)
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
		OutChannelYEnd.SetData(new CSNumberData(num4));
		OutPositionStart.SetData(new CSNumberData(num7));
		OutPositionMiddle.SetData(new CSNumberData(ultimateGemInChannels.InPositionMiddle));
		OutPositionEnd.SetData(new CSNumberData(num8));
		OutChannelEnabled.SetData(new CSNumberData(ultimateGemInChannels.InChannelEnabled ? 1 : 0));
		OutChannelSlopeLength.SetData(new CSNumberData(num9));
		OutGemsClosedLoop.SetData(new CSNumberData(flag ? 1 : 0));
		OutDirection.SetData(new CSNumberData(direction));
		return base.Update();
	}

	private static void AdjustEndErrorSoGemSpacingLessGemTollerance(UltimateGemInChannels inChannels, List<UltimateGemData> gemList, double vLen, double positionStartGem, Curve profile, Transform xTrans, ValueListItem vOrientationMethod, ref double endError)
	{
		if (gemList.Count >= 1)
		{
			UltimateGemData ultimateGemData = gemList[gemList.Count - 1].Duplicate();
			CalculateGemXform(inChannels, profile, xTrans, vOrientationMethod, ultimateGemData);
			Point3d origin = Point3d.Origin;
			origin.Transform(ultimateGemData.GemTransform);
			MoveAGem(inChannels, ultimateGemData, positionStartGem, endError, vLen, profile, xTrans, vOrientationMethod);
			Point3d origin2 = Point3d.Origin;
			origin2.Transform(ultimateGemData.GemTransform);
			double length = (origin2 - origin).Length;
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
			if ((gemPoint - gemPoint2).Length - ultimateGemData.SizeXRotated / 2.0 - ultimateGemData2.SizeXRotated / 2.0 - inChannels.InSpacing * 0.8 < 0.0)
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
		gemList = new List<UltimateGemData>();
		gemListMirror = new List<UltimateGemData>();
		Point3d point3d = Point3d.Unset;
		Point3d point3d2 = Point3d.Unset;
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
					point3d = ultimateGemData2.GemPoint;
					point3d2 = point3d;
					num = ultimateGemData.SizeXRotated;
				}
				double length2 = (ultimateGemData.GemPoint - point3d).Length;
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
				point3d = ultimateGemData.GemPoint;
			}
			if (doMirror)
			{
				num4 = 999.0;
				num5 = 20;
				if (point3d2 == Point3d.Unset)
				{
					point3d2 = point3d;
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
						double length3 = (ultimateGemData2.GemPoint - point3d2).Length;
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
						point3d2 = ultimateGemData2.GemPoint;
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
				point3d = gemList[gemList.Count - 1].GemPoint;
				if ((gemPoint - point3d).Length - gemList[0].SizeXRotated / 2.0 - gemList[gemList.Count - 1].SizeXRotated / 2.0 - inChannels.InSpacing < 0.0)
				{
					gemList.RemoveAt(gemList.Count - 1);
					flag = true;
				}
			}
		}
	}

	private static UltimateGemData CalcGemSize(UltimateGemInChannels inChannels, double paramPosition, double gemRotatedStartXSize, double gemRotatedMiddleXSize, double gemRotatedEndXSize, double xStart, double xMiddle, double xEnd, double yStart, double yMiddle, double yEnd, double placementStart, double placementMiddle, double placementEnd, ValueListItem vOrientationMethod, double positionStartGem, double positionEndGem, Curve profile)
	{
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
			Point3d point3d = inChannels.InCurve.PointAtNormalizedLength(paramPosition);
			Point3d point3d2 = inChannels.InBaseCurve.PointAt(ultimateGemData.BaseParam);
			double length = (point3d - point3d2).Length;
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
			CSUtilGem.GetGemScaleFromTable(inChannels.InGemShape, num2, num3, true, false, out xBestScale);
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
		Point3d origin = Point3d.Origin;
		origin.Transform(firstGem.GemTransform);
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
			double length2 = (ultimateGemData.GemPoint - origin).Length;
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
		double num = gemOnCurveInfo.SizeX / inChannels.InGemShape.GemXSize;
		Transform transform;
		if (gemOnCurveInfo.SizeY > 0.0)
		{
			double yScaleFactor = gemOnCurveInfo.SizeY / inChannels.InGemShape.GemYSize;
			transform = Transform.Scale(Plane.WorldXY, num, yScaleFactor, num);
		}
		else
		{
			transform = Transform.Scale(Plane.WorldXY, num, num, num);
		}
		CSGemData cSGemData = new CSGemData(inChannels.InGemShape);
		cSGemData.ApplyTransform(transform);
		double zOffset = 0.0;
		CSUtilGem.GetGemZOffset(cSGemData, inChannels.InPlacementMethod, ref zOffset);
		CalculatePointsOnCrv(inChannels.InCurve, inChannels.InBaseCurve, vOrientationMethod, gemOnCurveInfo);
		Point3d point3d = Point3d.Unset;
		if (inChannels.InBaseCurve != null)
		{
			point3d = inChannels.InBaseCurve.PointAt(gemOnCurveInfo.BaseParam);
		}
		Point3d point3d2 = inChannels.InCurve.PointAt(gemOnCurveInfo.Param);
		Vector3d vector3d = inChannels.InCurve.TangentAt(gemOnCurveInfo.Param);
		double num2 = 0.0;
		if (inChannels.InChannelEnabled && profile != null)
		{
			double num3 = ProfileCut(inChannels.InChannelWallGemOverlap, inChannels.InChannelWallWidth, profile, gemOnCurveInfo);
			ProfilePositionScale(gemOnCurveInfo, inChannels.InCurve, inChannels.InBaseCurve, out var pScale, out var pRail);
			num2 = (point3d2 - point3d).Length * (1.0 - num3);
			gemOnCurveInfo.ProfileCurve.Transform(pScale);
			gemOnCurveInfo.ProfileTransform = pRail;
		}
		Vector3d vector3d2 = (inChannels.InDoFlip ? new Vector3d(0.0, 0.0, -1.0) : new Vector3d(0.0, 0.0, 1.0));
		Vector3d vector3d3 = (inChannels.InDoFlip ? new Vector3d(-1.0, 0.0, 0.0) : new Vector3d(1.0, 0.0, 0.0));
		Vector3d brepNormal;
		switch (vOrientationMethod)
		{
		case ValueListItem.GemOrientationMethodSurface:
			brepNormal = Vector3d.ZAxis;
			CSUtilGeometry.GetClosestNormalToBrep(inChannels.InBaseSurface, point3d2, ref brepNormal);
			break;
		case ValueListItem.GemOrientationMethodOrigin:
			brepNormal = new Vector3d(point3d2);
			break;
		case ValueListItem.GemOrientationMethodAxisY:
			brepNormal = new Vector3d(point3d2.X, 0.0, point3d2.Z);
			break;
		case ValueListItem.GemOrientationMethodDown:
			brepNormal = Vector3d.ZAxis;
			break;
		case ValueListItem.OrientationMethodClosestPointOnCurve:
		case ValueListItem.OrientationMethodIntersectPointOnCurve:
			brepNormal = ((point3d != Point3d.Unset) ? (point3d2 - point3d) : new Vector3d(point3d2));
			brepNormal.Unitize();
			break;
		default:
			brepNormal = Vector3d.ZAxis;
			break;
		}
		brepNormal.Unitize();
		Transform transform2 = (inChannels.InOrientationFollowCurve ? CSBlockUtil.CSUtilMath.Orient3Point(Point3d.Origin, vector3d3, vector3d2, point3d2, vector3d, brepNormal) : CSBlockUtil.CSUtilMath.Orient3Point(Point3d.Origin, vector3d2, vector3d3, point3d2, brepNormal, vector3d));
		double num4 = zOffset + gemOnCurveInfo.ZPlacement - num2;
		Transform transform3 = Transform.Translation(brepNormal * num4);
		gemOnCurveInfo.GemTransform = transform3 * transform2 * xTrans * transform;
	}

	private static void CalculatePointsOnCrv(Curve crv, Curve baseCrv, ValueListItem vOrientationMethod, UltimateGemData gem)
	{
		double num;
		for (num = gem.NormParam; num > 1.0; num -= 1.0)
		{
		}
		for (; num < 0.0; num += 1.0)
		{
		}
		crv.NormalizedLengthParameter(num, out gem.Param);
		Point3d point3d = crv.PointAt(gem.Param);
		Vector3d normal = crv.TangentAt(gem.Param);
		gem.BaseParam = 0.0;
		if (baseCrv == null)
		{
			return;
		}
		if (vOrientationMethod == ValueListItem.OrientationMethodClosestPointOnCurve)
		{
			baseCrv.ClosestPoint(point3d, out gem.BaseParam);
			return;
		}
		Plane plane = new Plane(point3d, normal);
		CurveIntersections curveIntersections = Intersection.CurvePlane(baseCrv, plane, 0.001);
		double num2 = 9999.0;
		foreach (IntersectionEvent item in curveIntersections)
		{
			double length = (item.PointA - point3d).Length;
			if (length < num2)
			{
				num2 = length;
				gem.BaseParam = item.ParameterA;
			}
		}
	}

	private static bool CalculateRotateTestGem(CSGemData gemShape, double xSize, double ySize, double rollAngle, out double xSizeOut)
	{
		Transform transform = CSBlockUtil.CSUtilMath.Rotation(rollAngle, Vector3d.ZAxis, Point3d.Origin);
		if (!CSUtilGem.GetGemScaleFromTable(gemShape, xSize, ySize, true, false, out Transform xBestScale))
		{
			xSizeOut = 0.0;
			return false;
		}
		CSGemData cSGemData = new CSGemData(gemShape);
		cSGemData.ApplyTransform(transform * xBestScale);
		if (!(cSGemData.GetCurve() is NurbsCurve nurbsCurve))
		{
			xSizeOut = 0.0;
			return false;
		}
		for (int i = 0; i < nurbsCurve.Points.Count; i++)
		{
			ControlPoint value = nurbsCurve.Points[i];
			value.Location = new Point3d(value.Location.X, value.Location.Y, 0.0);
			nurbsCurve.Points[i] = value;
		}
		BoundingBox boundingBox = nurbsCurve.GetBoundingBox(accurate: true);
		double y = boundingBox.Min.Y;
		double num = boundingBox.Max.Y - boundingBox.Min.Y;
		double num2 = 0.0;
		for (int j = 1; j < 20; j++)
		{
			CurveIntersections curveIntersections = Intersection.CurveCurve(new LineCurve(new Point3d(20.0, y + num * (double)j / 20.0, 0.0), new Point3d(-20.0, y + num * (double)j / 20.0, 0.0)), nurbsCurve, 0.001, 0.001);
			if (curveIntersections.Count < 2)
			{
				continue;
			}
			double num3 = 999.0;
			double num4 = -999.0;
			foreach (IntersectionEvent item in curveIntersections)
			{
				Point3d pointA = item.PointA;
				if (pointA.X > num4)
				{
					num4 = pointA.X;
				}
				if (pointA.X < num3)
				{
					num3 = pointA.X;
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
		double num = (gemPos.NormParam - positionStartGem) * endError / gemRowLength;
		gemPos.NormParam += num;
		CalculateGemXform(inChannels, profile, xTrans, vOrientationMethod, gemPos);
	}

	private static double ProfileCut(double channelWallGemOverlap, double wallWidth, Curve profile, UltimateGemData gemOnCurveInfo)
	{
		double sizeY = gemOnCurveInfo.SizeY;
		LineCurve curveB = new LineCurve(new Point3d((0.0 - sizeY) / 2.0 + channelWallGemOverlap, 2.0, 0.0), new Point3d((0.0 - sizeY) / 2.0 + channelWallGemOverlap, -2.0, 0.0));
		LineCurve curveB2 = new LineCurve(new Point3d(sizeY / 2.0 - channelWallGemOverlap, 2.0, 0.0), new Point3d(sizeY / 2.0 - channelWallGemOverlap, -2.0, 0.0));
		gemOnCurveInfo.ProfileCurve = profile.DuplicateCurve();
		Transform xform = Transform.Scale(Plane.WorldXY, wallWidth + gemOnCurveInfo.SizeY, 1.0, 1.0);
		gemOnCurveInfo.ProfileCurve.Transform(xform);
		CurveIntersections curveIntersections = Intersection.CurveCurve(gemOnCurveInfo.ProfileCurve, curveB, 0.001, 0.001);
		CurveIntersections curveIntersections2 = Intersection.CurveCurve(gemOnCurveInfo.ProfileCurve, curveB2, 0.001, 0.001);
		double num = -1.0;
		foreach (IntersectionEvent item in curveIntersections)
		{
			Point3d pointA = item.PointA;
			if (pointA.Y > num)
			{
				num = pointA.Y;
				gemOnCurveInfo.ParamLeft = item.ParameterA;
			}
		}
		double num2 = -1.0;
		foreach (IntersectionEvent item2 in curveIntersections2)
		{
			Point3d pointA2 = item2.PointA;
			if (pointA2.Y > num2)
			{
				num2 = pointA2.Y;
				gemOnCurveInfo.ParamRight = item2.ParameterA;
			}
		}
		return (num + num2) / 2.0;
	}

	private static void ProfilePositionScale(UltimateGemData gem, Curve crv, Curve baseCrv, out Transform pScale, out Transform pRail)
	{
		Point3d point3d = crv.PointAt(gem.Param);
		Vector3d destination = crv.TangentAt(gem.Param);
		Point3d point3d2 = baseCrv.PointAt(gem.BaseParam);
		Vector3d destination2 = point3d - point3d2;
		double length = destination2.Length;
		pScale = Transform.Scale(Plane.WorldXY, 1.0, length, 1.0);
		pRail = CSBlockUtil.CSUtilMath.Orient3Point(new Point3d(0.0, length, 0.0), Vector3d.YAxis, Vector3d.ZAxis, point3d, destination2, destination);
	}
}
