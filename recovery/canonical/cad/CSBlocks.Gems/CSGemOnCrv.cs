using System;
using System.Collections.Generic;
using CS.CSDataUtilities;
using CS.CSObjects.CSBlocks;
using CS.CSValues.Interfaces;
using CS.Parametric;
using CS.Parametric.Channels;
using CSBlockDataUtils;
using CSBlockUtil;
using CSDatas;
using GV.CounterSketch.Shared;
using Rhino.Geometry;

namespace CSBlocks.Gems;

[CSBlock("GemOnCrv", "{74F8BEDE-A992-4EA9-8676-92F5ACBD28E7}")]
public sealed class CSGemOnCrv : CSBlock
{
	public readonly CSInputChannel InCurve;

	public readonly CSInputChannel InSurface;

	public readonly CSInputChannel InGemShape;

	public readonly CSInputChannel InDiameterStart;

	public readonly CSInputChannel InDiameterMiddle;

	public readonly CSInputChannel InDiameterEnd;

	public readonly CSInputChannel InPositionStart;

	public readonly CSInputChannel InPositionEnd;

	public readonly CSInputChannel InAmountToMiddle;

	public readonly CSInputChannel InPlacementStart;

	public readonly CSInputChannel InPlacementMiddle;

	public readonly CSInputChannel InPlacementEnd;

	public readonly CSInputChannel InSpacing;

	public readonly CSInputChannel InRoll;

	public readonly CSInputChannel InTilt;

	public readonly CSInputChannel InOrientationMethod;

	public readonly CSInputChannel InPlacementMethod;

	public readonly CSInputChannel InSpacingMethod;

	public readonly CSInputChannel InEnableDiameterMiddle;

	public readonly CSInputChannel InEnableDiameterEnd;

	public readonly CSInputChannel InEnablePlacementMiddle;

	public readonly CSInputChannel InEnablePlacementEnd;

	public readonly CSInputChannel InDoFlip;

	public readonly CSInputChannel InDoMirror;

	public readonly CSInputChannel InDoReverseDirection;

	public readonly CSInputChannel InGemOnOff;

	public readonly CSOutputChannel OutArray;

	public readonly CSOutputChannel OutDisplayGems;

	private IList<CSUtilGemOnCrv.CurveInfo> _crvInfos;

	private bool _isUsingCustomGemSizes;

	public CSGemOnCrv(string identifier)
		: base(identifier)
	{
		InCurve = new CSInputChannel(this, ChannelDataType.Curve, ChannelArrayType.Individual);
		InSurface = new CSInputChannel(this, ChannelDataType.Brep, ChannelArrayType.Array, ChannelFlag.NotRequired);
		InGemShape = new CSInputChannel(this, ChannelDataType.Gem, ChannelArrayType.Individual);
		InDiameterStart = new CSInputChannel(this, ChannelDataType.Gem, ChannelArrayType.Individual);
		InDiameterMiddle = new CSInputChannel(this, ChannelDataType.Gem, ChannelArrayType.Individual);
		InDiameterEnd = new CSInputChannel(this, ChannelDataType.Gem, ChannelArrayType.Individual);
		InPositionStart = new CSInputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual, 0.0);
		InPositionEnd = new CSInputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual, 0.0);
		InAmountToMiddle = new CSInputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual, 50);
		InPlacementStart = new CSInputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual, 0.0);
		InPlacementMiddle = new CSInputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual, 0.0);
		InPlacementEnd = new CSInputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual, 0.0);
		InSpacing = new CSInputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual, 0.0);
		InRoll = new CSInputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual, 0.0);
		InTilt = new CSInputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual, 0.0);
		InOrientationMethod = new CSInputChannel(this, ChannelDataType.Integer, ChannelArrayType.Individual, 17);
		InPlacementMethod = new CSInputChannel(this, ChannelDataType.Integer, ChannelArrayType.Individual, 30);
		InSpacingMethod = new CSInputChannel(this, ChannelDataType.Integer, ChannelArrayType.Individual, 10);
		InEnableDiameterMiddle = new CSInputChannel(this, ChannelDataType.Boolean, ChannelArrayType.Individual, defaultValue: false);
		InEnableDiameterEnd = new CSInputChannel(this, ChannelDataType.Boolean, ChannelArrayType.Individual, defaultValue: false);
		InEnablePlacementMiddle = new CSInputChannel(this, ChannelDataType.Boolean, ChannelArrayType.Individual, defaultValue: false);
		InEnablePlacementEnd = new CSInputChannel(this, ChannelDataType.Boolean, ChannelArrayType.Individual, defaultValue: false);
		InDoFlip = new CSInputChannel(this, ChannelDataType.Boolean, ChannelArrayType.Individual, defaultValue: false);
		InDoMirror = new CSInputChannel(this, ChannelDataType.Boolean, ChannelArrayType.Individual, defaultValue: false);
		InDoReverseDirection = new CSInputChannel(this, ChannelDataType.Boolean, ChannelArrayType.Individual, defaultValue: false);
		InGemOnOff = new CSInputChannel(this, ChannelDataType.Boolean, ChannelArrayType.Individual, defaultValue: false);
		OutArray = new CSOutputChannel(this, ChannelDataType.Gem, ChannelArrayType.Array);
		OutDisplayGems = new CSOutputChannel(this, ChannelDataType.Gem, ChannelArrayType.Array);
		SetChannelIdentifiers(autoCheckInputs: true);
	}

	public override bool Update()
	{
		NurbsCurve nurbsCurve = InCurve.NurbsCurve;
		if ((_crvInfos == null || HasThisObjectChangedSinceLastUpdate() || BeenUpdatedAfterLastUpdate(InCurve.OutputChannelBlock)) && !CSUtilGemOnCrv.PreProcessCrv(nurbsCurve, out _crvInfos))
		{
			return UpdateError("PreProcessCrv failed");
		}
		Brep brep = InSurface.Brep;
		CSGemData gem = InGemShape.Gem;
		CSGemData gem2 = InDiameterStart.Gem;
		CSGemData gem3 = InDiameterMiddle.Gem;
		CSGemData gem4 = InDiameterEnd.Gem;
		double gemXSize = gem2.GemXSize;
		double num = gem3.GemXSize;
		double num2 = gem4.GemXSize;
		double gemYSize = gem2.GemYSize;
		double heightMiddle = gem3.GemYSize;
		double num3 = gem4.GemYSize;
		double pos = InPositionStart.Double;
		double pos2 = InPositionEnd.Double;
		double num4 = InAmountToMiddle.Percentage;
		double num5 = InPlacementStart.Double;
		double placementMiddle = InPlacementMiddle.Double;
		double num6 = InPlacementEnd.Double;
		double num7 = InSpacing.Double;
		double num8 = InRoll.Double;
		double num9 = InTilt.Double;
		ValueListItem valueListItem = InOrientationMethod.ValueListItem;
		ValueListItem valueListItem2 = InPlacementMethod.ValueListItem;
		ValueListItem valueListItem3 = InSpacingMethod.ValueListItem;
		bool boolean = InEnableDiameterMiddle.Boolean;
		bool boolean2 = InEnableDiameterEnd.Boolean;
		bool boolean3 = InEnablePlacementMiddle.Boolean;
		bool boolean4 = InEnablePlacementEnd.Boolean;
		bool boolean5 = InDoFlip.Boolean;
		bool boolean6 = InDoMirror.Boolean;
		bool boolean7 = InDoReverseDirection.Boolean;
		if (brep == null && valueListItem == ValueListItem.GemOrientationMethodSurface)
		{
			valueListItem = ValueListItem.GemOrientationMethodAxisY;
		}
		if (!boolean)
		{
			num4 = 1.0;
		}
		if (!boolean2)
		{
			num2 = gemXSize;
			num3 = gemYSize;
		}
		if (!boolean)
		{
			num = gemXSize + (num2 - gemXSize) * num4;
			heightMiddle = gemYSize + (num3 - gemYSize) * num4;
		}
		if (!boolean4)
		{
			num6 = num5;
		}
		if (!boolean3)
		{
			placementMiddle = num5 + (num6 - num5) * num4;
		}
		ICSValueGemSize iCSValueGemSize = InDiameterStart.OutputChannelBlock as ICSValueGemSize;
		ICSValueGemSize iCSValueGemSize2 = InDiameterEnd.OutputChannelBlock as ICSValueGemSize;
		ICSValueGemSize iCSValueGemSize3 = InDiameterMiddle.OutputChannelBlock as ICSValueGemSize;
		if (iCSValueGemSize != null && iCSValueGemSize2 != null && iCSValueGemSize3 != null)
		{
			_isUsingCustomGemSizes = iCSValueGemSize.IsCustomGemSize || (boolean2 && iCSValueGemSize2.IsCustomGemSize) || (boolean && iCSValueGemSize3.IsCustomGemSize);
		}
		Transform transform = CSBlockUtil.CSUtilMath.Rotation(CSBlockUtil.CSUtilMath.ToRadians(num9), Vector3d.XAxis, Point3d.Origin);
		Transform xform = CSBlockUtil.CSUtilMath.Rotation(CSBlockUtil.CSUtilMath.ToRadians(num8), Vector3d.ZAxis, Point3d.Origin) * transform;
		double radius = 0.0;
		double rotationalAdjustmentFactor = 0.0;
		if (!CSUtilGemOnCrv.CalculateTestGem(gem, gemXSize, xform, ref radius, ref rotationalAdjustmentFactor))
		{
			UpdateError("GetGemScaleFromX failed");
		}
		double radius2 = 0.0;
		double rotationalAdjustmentFactor2 = 0.0;
		if (!CSUtilGemOnCrv.CalculateTestGem(gem, num, xform, ref radius2, ref rotationalAdjustmentFactor2))
		{
			UpdateError("GetGemScaleFromX failed");
		}
		double radius3 = 0.0;
		double rotationalAdjustmentFactor3 = 0.0;
		if (!CSUtilGemOnCrv.CalculateTestGem(gem, num2, xform, ref radius3, ref rotationalAdjustmentFactor3))
		{
			UpdateError("GetGemScaleFromX failed");
		}
		double num10 = (radius * rotationalAdjustmentFactor + num7 / 2.0) / radius * rotationalAdjustmentFactor;
		double num11 = (radius2 * rotationalAdjustmentFactor2 + num7 / 2.0) / radius2 * rotationalAdjustmentFactor2;
		double num12 = (radius2 * rotationalAdjustmentFactor3 + num7 / 2.0) / radius2 * rotationalAdjustmentFactor3;
		double spacingScaleFactor = (num10 + num11 + num12) / 3.0;
		if (!CSUtilGemOnCrv.GetClosestIndexToPosition(nurbsCurve, _crvInfos, pos, out var index))
		{
			return UpdateError("GetClosestIndexToPosition failed");
		}
		if (!CSUtilGemOnCrv.GetClosestIndexToPosition(nurbsCurve, _crvInfos, pos2, out var index2))
		{
			return UpdateError("GetClosestIndexToPosition failed");
		}
		int num13 = ((index < index2) ? 1 : (-1));
		if (boolean7)
		{
			num13 *= -1;
		}
		if (!CalculateGemCountEstimate(gem, num4, index, index2, num13, boolean7, gemXSize, num, num2, rotationalAdjustmentFactor, rotationalAdjustmentFactor2, rotationalAdjustmentFactor3, valueListItem3, num7, spacingScaleFactor, out var countStartToMiddle, out var countMiddleToEnd))
		{
			return UpdateError("CalculateGemCountEstimate failed");
		}
		if (!CalculateGemInfos(countStartToMiddle, countMiddleToEnd, gem, num4, index, index2, num13, boolean7, gemXSize, gemYSize, num, heightMiddle, num2, num3, rotationalAdjustmentFactor, rotationalAdjustmentFactor2, rotationalAdjustmentFactor3, valueListItem3, num7, spacingScaleFactor, num5, placementMiddle, num6, out var gemOnCurveInfos))
		{
			return UpdateError("CalculateGemInfos failed");
		}
		if (boolean6)
		{
			CalculateGemInfosMirror(index, index2, num13, boolean7, ref gemOnCurveInfos);
		}
		CSDataArray cSDataArray = new CSDataArray();
		foreach (CSUtilGemOnCrv.GemOnCurveInfo item in gemOnCurveInfos)
		{
			Transform transform2 = CSUtilGemOnCrv.CalculateGemXform(_crvInfos, item, brep, gem, num8, num9, valueListItem, valueListItem2, boolean5);
			CSData data = gem.Duplicate();
			CSDataHelper.Transform(data, transform2);
			cSDataArray.Append(data);
		}
		OutArray.SetData(cSDataArray);
		InGemOnOff.GetValue(out bool value);
		if (value)
		{
			OutDisplayGems.SetData(cSDataArray);
		}
		return base.Update();
	}

	private void AdjustSpacing(int indexEnd, int direction, bool doReverseDirection, ValueListItem whichSpacingMethod, double spacing, double spacingScaleFactor, IReadOnlyList<CSUtilGemOnCrv.GemOnCurveInfo> gemOnCurveInfos, ref List<CSUtilGemOnCrv.GemOnCurveInfo> gemOnCurveInfoSet)
	{
		if (whichSpacingMethod != ValueListItem.GemSpacingMethodMinimum || gemOnCurveInfoSet.Count <= 1)
		{
			return;
		}
		CSUtilGemOnCrv.GemOnCurveInfo gemOnCurveInfo = ((gemOnCurveInfos.Count >= 1) ? gemOnCurveInfos[0] : gemOnCurveInfoSet[0]);
		CSUtilGemOnCrv.GemOnCurveInfo gemOnCurveInfo2 = gemOnCurveInfoSet[gemOnCurveInfoSet.Count - 1];
		int index = gemOnCurveInfo.Index;
		int i = gemOnCurveInfo2.Index;
		double num = gemOnCurveInfo.RadiusAdjusted + gemOnCurveInfo2.RadiusAdjusted;
		num = ((whichSpacingMethod != ValueListItem.GemSpacingMethodMinimumScaled) ? (num + spacing) : (num * spacingScaleFactor));
		bool flag = false;
		for (; CSUtilGemOnCrv.GetFixedIndex(_crvInfos, i) != CSUtilGemOnCrv.GetFixedIndex(_crvInfos, indexEnd); i += direction)
		{
			Point3d ptRc = _crvInfos[CSUtilGemOnCrv.GetFixedIndex(_crvInfos, i)].PtRc;
			Point3d ptRc2 = _crvInfos[CSUtilGemOnCrv.GetFixedIndex(_crvInfos, index)].PtRc;
			if (ptRc.DistanceTo(ptRc2) < num)
			{
				flag = true;
				break;
			}
		}
		double num2 = Math.Abs((flag ? i : indexEnd) - gemOnCurveInfoSet[gemOnCurveInfoSet.Count - 1].Index);
		if (doReverseDirection)
		{
			num2 = (double)_crvInfos.Count - num2;
		}
		int num3 = (flag ? gemOnCurveInfoSet.Count : (gemOnCurveInfoSet.Count - 1));
		double num4 = num2 / (double)num3 * (double)direction;
		for (int j = 0; j < gemOnCurveInfoSet.Count; j++)
		{
			int index2 = gemOnCurveInfoSet[j].Index + (int)(num4 * (double)j);
			CSUtilGemOnCrv.GemOnCurveInfo value = gemOnCurveInfoSet[j];
			value.Index = index2;
			gemOnCurveInfoSet[j] = value;
		}
	}

	private bool CalculateGemCountEstimate(CSGemData sourceGem, double amountToMiddle, int indexStart, int indexEnd, int direction, bool doReverseDirection, double diameterStart, double diameterMiddle, double diameterEnd, double rotationalAdjustmentFactorStart, double rotationalAdjustmentFactorMiddle, double rotationalAdjustmentFactorEnd, ValueListItem whichSpacingMethod, double spacing, double spacingScaleFactor, out int countStartToMiddle, out int countMiddleToEnd)
	{
		countStartToMiddle = 0;
		countMiddleToEnd = 0;
		int num = Math.Abs(indexStart - indexEnd);
		if (doReverseDirection)
		{
			num = _crvInfos.Count - num;
		}
		int fixedIndex = CSUtilGemOnCrv.GetFixedIndex(_crvInfos, indexStart + direction * (int)Math.Round((double)num * amountToMiddle, MidpointRounding.AwayFromZero));
		int num2 = ((direction != 1) ? ((!(diameterStart < diameterMiddle)) ? 1 : (-1)) : ((diameterStart < diameterMiddle) ? 1 : (-1)));
		List<CSUtilGemOnCrv.GemOnCurveInfo> list = new List<CSUtilGemOnCrv.GemOnCurveInfo>();
		int num3 = ((diameterStart < diameterMiddle) ? indexStart : fixedIndex);
		int i = 0;
		int num4 = (int)Math.Round((double)num * amountToMiddle, MidpointRounding.AwayFromZero);
		int num5 = 0;
		for (; i <= num4; i++)
		{
			double num6 = (double)i / (double)num4;
			double num7 = diameterStart + (diameterMiddle - diameterStart) * num6;
			double diameterFromTable;
			if (!_isUsingCustomGemSizes)
			{
				if (!CSUtilGemOnCrv.GetDiameterFromTable(sourceGem, num7, out diameterFromTable))
				{
					return UpdateError("GetDiameterFromTable failed");
				}
			}
			else
			{
				diameterFromTable = Math.Round(num7, 1, MidpointRounding.AwayFromZero);
			}
			double num8 = diameterFromTable / 2.0;
			double num9 = rotationalAdjustmentFactorStart + (rotationalAdjustmentFactorMiddle - rotationalAdjustmentFactorStart) * num6;
			CSUtilGemOnCrv.GemOnCurveInfo gemOnCurveInfo = new CSUtilGemOnCrv.GemOnCurveInfo
			{
				Radius = num8,
				RadiusAdjusted = num8 * num9,
				Index = num3
			};
			bool flag = true;
			if (list.Count >= 1)
			{
				CSUtilGemOnCrv.GemOnCurveInfo gemRhs = list[list.Count - 1];
				flag = CheckSpacing(gemOnCurveInfo, gemRhs, whichSpacingMethod, spacing, spacingScaleFactor);
			}
			if (flag)
			{
				list.Add(gemOnCurveInfo);
				num5++;
			}
			num3 += num2;
		}
		countStartToMiddle = num5;
		int num10 = ((direction != 1) ? ((!(diameterMiddle < diameterEnd)) ? 1 : (-1)) : ((diameterMiddle < diameterEnd) ? 1 : (-1)));
		List<CSUtilGemOnCrv.GemOnCurveInfo> list2 = new List<CSUtilGemOnCrv.GemOnCurveInfo>();
		int num11 = ((diameterMiddle < diameterEnd) ? fixedIndex : indexEnd);
		int j = 0;
		int num12 = num - i;
		int num13 = 0;
		for (; j <= num12; j++)
		{
			double num14 = (double)j / (double)num12;
			double num15 = diameterMiddle + (diameterEnd - diameterMiddle) * num14;
			double diameterFromTable2;
			if (!_isUsingCustomGemSizes)
			{
				if (!CSUtilGemOnCrv.GetDiameterFromTable(sourceGem, num15, out diameterFromTable2))
				{
					return UpdateError("GetDiameterFromTable failed");
				}
			}
			else
			{
				diameterFromTable2 = Math.Round(num15, 1, MidpointRounding.AwayFromZero);
			}
			double num16 = diameterFromTable2 / 2.0;
			double num17 = rotationalAdjustmentFactorMiddle + (rotationalAdjustmentFactorEnd - rotationalAdjustmentFactorMiddle) * num14;
			CSUtilGemOnCrv.GemOnCurveInfo gemOnCurveInfo2 = new CSUtilGemOnCrv.GemOnCurveInfo
			{
				Radius = num16,
				RadiusAdjusted = num16 * num17,
				Index = num11
			};
			bool flag2 = true;
			if (list2.Count >= 1)
			{
				CSUtilGemOnCrv.GemOnCurveInfo gemRhs2 = list2[list2.Count - 1];
				flag2 = CheckSpacing(gemOnCurveInfo2, gemRhs2, whichSpacingMethod, spacing, spacingScaleFactor);
			}
			if (flag2)
			{
				list2.Add(gemOnCurveInfo2);
				num13++;
			}
			num11 += num10;
		}
		countMiddleToEnd = num13;
		return true;
	}

	private bool CalculateGemInfos(int countStartToMiddle, int countMiddleToEnd, CSGemData sourceGem, double amountToMiddle, int indexStart, int indexEnd, int direction, bool doReverseDirection, double widthStart, double heightStart, double widthMiddle, double heightMiddle, double widthEnd, double heightEnd, double rotationalAdjustmentFactorStart, double rotationalAdjustmentFactorMiddle, double rotationalAdjustmentFactorEnd, ValueListItem whichSpacingMethod, double spacing, double spacingScaleFactor, double placementStart, double placementMiddle, double placementEnd, out List<CSUtilGemOnCrv.GemOnCurveInfo> gemOnCurveInfos)
	{
		gemOnCurveInfos = new List<CSUtilGemOnCrv.GemOnCurveInfo>();
		if (countStartToMiddle + countMiddleToEnd <= 0)
		{
			return true;
		}
		int num = indexStart;
		int num2 = Math.Abs(indexStart - indexEnd);
		if (doReverseDirection)
		{
			num2 = _crvInfos.Count - num2;
		}
		int i = 0;
		int num3 = (int)Math.Round((double)num2 * amountToMiddle, MidpointRounding.AwayFromZero);
		int num4 = 0;
		for (; i <= num3; i++)
		{
			if (num4 >= countStartToMiddle)
			{
				break;
			}
			double num5 = ((countStartToMiddle == 1) ? 0.0 : ((double)num4 / ((double)countStartToMiddle - 1.0)));
			double num6 = widthStart + (widthMiddle - widthStart) * num5;
			double num7 = heightStart + (heightMiddle - heightStart) * num5;
			double widthFromTable;
			double heightFromTable;
			if (!_isUsingCustomGemSizes)
			{
				if (!CSUtilGemOnCrv.GetWidthHeightFromTable(sourceGem, num6, num7, out widthFromTable, out heightFromTable))
				{
					return UpdateError("GetWidthHeightFromTable failed");
				}
			}
			else
			{
				widthFromTable = Math.Round(num6, 1, MidpointRounding.AwayFromZero);
				heightFromTable = Math.Round(num7, 1, MidpointRounding.AwayFromZero);
			}
			double num8 = widthFromTable / 2.0;
			double num9 = rotationalAdjustmentFactorStart + (rotationalAdjustmentFactorMiddle - rotationalAdjustmentFactorStart) * num5;
			double placement = placementStart + (placementMiddle - placementStart) * num5;
			CSUtilGemOnCrv.GemOnCurveInfo gemOnCurveInfo = new CSUtilGemOnCrv.GemOnCurveInfo
			{
				Radius = num8,
				Height = heightFromTable,
				RadiusAdjusted = num8 * num9,
				Placement = placement,
				Index = num
			};
			bool flag = true;
			bool flag2 = true;
			if (gemOnCurveInfos.Count >= 1)
			{
				CSUtilGemOnCrv.GemOnCurveInfo gemRhs = gemOnCurveInfos[0];
				flag = CheckSpacing(gemOnCurveInfo, gemRhs, whichSpacingMethod, spacing, spacingScaleFactor);
			}
			if (gemOnCurveInfos.Count >= 1)
			{
				CSUtilGemOnCrv.GemOnCurveInfo gemRhs2 = gemOnCurveInfos[gemOnCurveInfos.Count - 1];
				flag2 = CheckSpacing(gemOnCurveInfo, gemRhs2, whichSpacingMethod, spacing, spacingScaleFactor);
			}
			if (flag && flag2)
			{
				gemOnCurveInfos.Add(gemOnCurveInfo);
				num4++;
			}
			num += direction;
		}
		if (num4 < countStartToMiddle)
		{
			return CalculateGemInfos(countStartToMiddle - 1, countMiddleToEnd, sourceGem, amountToMiddle, indexStart, indexEnd, direction, doReverseDirection, widthStart, heightStart, widthMiddle, heightMiddle, widthEnd, heightEnd, rotationalAdjustmentFactorStart, rotationalAdjustmentFactorMiddle, rotationalAdjustmentFactorEnd, whichSpacingMethod, spacing, spacingScaleFactor, placementStart, placementMiddle, placementEnd, out gemOnCurveInfos);
		}
		int j = 0;
		int num10 = num2 - num3;
		int num11 = 1;
		for (; j <= num10; j++)
		{
			if (num11 >= countMiddleToEnd)
			{
				break;
			}
			double num12 = ((countMiddleToEnd == 1) ? 0.0 : ((double)num11 / ((double)countMiddleToEnd - 1.0)));
			double num13 = widthMiddle + (widthEnd - widthMiddle) * num12;
			double num14 = heightMiddle + (heightEnd - heightMiddle) * num12;
			double widthFromTable2;
			double heightFromTable2;
			if (!_isUsingCustomGemSizes)
			{
				if (!CSUtilGemOnCrv.GetWidthHeightFromTable(sourceGem, num13, num14, out widthFromTable2, out heightFromTable2))
				{
					return UpdateError("GetWidthHeightFromTable failed");
				}
			}
			else
			{
				widthFromTable2 = Math.Round(num13, 1, MidpointRounding.AwayFromZero);
				heightFromTable2 = Math.Round(num14, 1, MidpointRounding.AwayFromZero);
			}
			double num15 = widthFromTable2 / 2.0;
			double num16 = rotationalAdjustmentFactorMiddle + (rotationalAdjustmentFactorEnd - rotationalAdjustmentFactorMiddle) * num12;
			double placement2 = placementMiddle + (placementEnd - placementMiddle) * num12;
			CSUtilGemOnCrv.GemOnCurveInfo gemOnCurveInfo2 = new CSUtilGemOnCrv.GemOnCurveInfo
			{
				Radius = num15,
				Height = heightFromTable2,
				RadiusAdjusted = num15 * num16,
				Placement = placement2,
				Index = num
			};
			bool flag3 = true;
			bool flag4 = true;
			if (gemOnCurveInfos.Count >= 1)
			{
				CSUtilGemOnCrv.GemOnCurveInfo gemRhs3 = gemOnCurveInfos[0];
				flag3 = CheckSpacing(gemOnCurveInfo2, gemRhs3, whichSpacingMethod, spacing, spacingScaleFactor);
			}
			if (gemOnCurveInfos.Count >= 1)
			{
				CSUtilGemOnCrv.GemOnCurveInfo gemRhs4 = gemOnCurveInfos[gemOnCurveInfos.Count - 1];
				flag4 = CheckSpacing(gemOnCurveInfo2, gemRhs4, whichSpacingMethod, spacing, spacingScaleFactor);
			}
			if (flag3 && flag4)
			{
				gemOnCurveInfos.Add(gemOnCurveInfo2);
				num11++;
			}
			num += direction;
		}
		if (num11 < countMiddleToEnd)
		{
			return CalculateGemInfos(countStartToMiddle, countMiddleToEnd - 1, sourceGem, amountToMiddle, indexStart, indexEnd, direction, doReverseDirection, widthStart, heightStart, widthMiddle, heightMiddle, widthEnd, heightEnd, rotationalAdjustmentFactorStart, rotationalAdjustmentFactorMiddle, rotationalAdjustmentFactorEnd, whichSpacingMethod, spacing, spacingScaleFactor, placementStart, placementMiddle, placementEnd, out gemOnCurveInfos);
		}
		AdjustSpacing(indexEnd, direction, doReverseDirection, whichSpacingMethod, spacing, spacingScaleFactor, gemOnCurveInfos, ref gemOnCurveInfos);
		return true;
	}

	private void CalculateGemInfosMirror(int indexStart, int indexEnd, int direction, bool doReverseDirection, ref List<CSUtilGemOnCrv.GemOnCurveInfo> gemOnCurveInfos)
	{
		if (gemOnCurveInfos.Count < 2)
		{
			return;
		}
		int num = gemOnCurveInfos.Count - 2;
		int num2 = gemOnCurveInfos.Count - 1;
		int num3 = CSUtilGemOnCrv.GetFixedIndex(_crvInfos, gemOnCurveInfos[num2].Index);
		int i = 0;
		for (int num4 = (doReverseDirection ? Math.Abs(indexStart - indexEnd) : (_crvInfos.Count - Math.Abs(indexStart - indexEnd))); i <= num4; i++)
		{
			if (num < 0)
			{
				break;
			}
			CSUtilGemOnCrv.GemOnCurveInfo gemOnCurveInfo = gemOnCurveInfos[num];
			CSUtilGemOnCrv.GemOnCurveInfo gemOnCurveInfo2 = gemOnCurveInfos[num2];
			Point3d ptRc = _crvInfos[CSUtilGemOnCrv.GetFixedIndex(_crvInfos, gemOnCurveInfo.Index)].PtRc;
			Point3d ptRc2 = _crvInfos[CSUtilGemOnCrv.GetFixedIndex(_crvInfos, gemOnCurveInfo2.Index)].PtRc;
			double spacing = ptRc.DistanceTo(ptRc2) - (gemOnCurveInfo.RadiusAdjusted + gemOnCurveInfo2.RadiusAdjusted);
			CSUtilGemOnCrv.GemOnCurveInfo gemOnCurveInfo3 = gemOnCurveInfos[num];
			gemOnCurveInfo3.Index = num3;
			bool flag = true;
			bool flag2 = true;
			if (gemOnCurveInfos.Count >= 1)
			{
				CSUtilGemOnCrv.GemOnCurveInfo gemRhs = gemOnCurveInfos[0];
				flag = CheckSpacing(gemOnCurveInfo3, gemRhs, spacing);
			}
			if (gemOnCurveInfos.Count >= 1)
			{
				CSUtilGemOnCrv.GemOnCurveInfo gemRhs2 = gemOnCurveInfos[gemOnCurveInfos.Count - 1];
				flag2 = CheckSpacing(gemOnCurveInfo3, gemRhs2, spacing);
			}
			if (flag && flag2)
			{
				gemOnCurveInfos.Add(gemOnCurveInfo3);
				num--;
				num2--;
			}
			num3 += direction;
		}
	}

	private bool CheckSpacing(CSUtilGemOnCrv.GemOnCurveInfo gemLhs, CSUtilGemOnCrv.GemOnCurveInfo gemRhs, ValueListItem whichSpacingMethod, double spacing, double spacingScaleFactor)
	{
		double num = gemLhs.RadiusAdjusted + gemRhs.RadiusAdjusted;
		num = ((whichSpacingMethod != ValueListItem.GemSpacingMethodMinimumScaled) ? (num + spacing) : (num * spacingScaleFactor));
		Point3d ptRc = _crvInfos[CSUtilGemOnCrv.GetFixedIndex(_crvInfos, gemLhs.Index)].PtRc;
		Point3d ptRc2 = _crvInfos[CSUtilGemOnCrv.GetFixedIndex(_crvInfos, gemRhs.Index)].PtRc;
		return ptRc.DistanceTo(ptRc2) >= num;
	}

	private bool CheckSpacing(CSUtilGemOnCrv.GemOnCurveInfo gemLhs, CSUtilGemOnCrv.GemOnCurveInfo gemRhs, double spacing)
	{
		double num = gemLhs.RadiusAdjusted + gemRhs.RadiusAdjusted + spacing;
		Point3d ptRc = _crvInfos[CSUtilGemOnCrv.GetFixedIndex(_crvInfos, gemLhs.Index)].PtRc;
		Point3d ptRc2 = _crvInfos[CSUtilGemOnCrv.GetFixedIndex(_crvInfos, gemRhs.Index)].PtRc;
		return ptRc.DistanceTo(ptRc2) >= num;
	}
}
