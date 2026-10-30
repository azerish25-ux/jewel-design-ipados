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

[CSBlock("GemOnCrvCount", "{ED9D29AA-2627-440B-922D-03618903B594}")]
public sealed class CSGemOnCrvCount : CSBlock
{
	public readonly CSInputChannel InCurve;

	public readonly CSInputChannel InSurface;

	public readonly CSInputChannel InGemShape;

	public readonly CSInputChannel InCount;

	public readonly CSInputChannel InDiameterStart;

	public readonly CSInputChannel InDiameterMiddle;

	public readonly CSInputChannel InDiameterEnd;

	public readonly CSInputChannel InPosition;

	public readonly CSInputChannel InAmountToMiddle;

	public readonly CSInputChannel InPlacementStart;

	public readonly CSInputChannel InPlacementMiddle;

	public readonly CSInputChannel InPlacementEnd;

	public readonly CSInputChannel InSpacing;

	public readonly CSInputChannel InRoll;

	public readonly CSInputChannel InTilt;

	public readonly CSInputChannel InOrientationMethod;

	public readonly CSInputChannel InPlacementMethod;

	public readonly CSInputChannel InEnableDiameterMiddle;

	public readonly CSInputChannel InEnableDiameterEnd;

	public readonly CSInputChannel InEnablePlacementMiddle;

	public readonly CSInputChannel InEnablePlacementEnd;

	public readonly CSInputChannel InDoFlip;

	public readonly CSInputChannel InDoMirror;

	public readonly CSInputChannel InDoReverseDirection;

	public readonly CSInputChannel InGemsOnOff;

	public readonly CSOutputChannel OutArray;

	public readonly CSOutputChannel OutDisplayGems;

	private IList<CSUtilGemOnCrv.CurveInfo> _crvInfos;

	private bool _isUsingCustomGemSizes;

	public CSGemOnCrvCount(string identifier)
		: base(identifier)
	{
		InCurve = new CSInputChannel(this, ChannelDataType.Curve, ChannelArrayType.Individual);
		InSurface = new CSInputChannel(this, ChannelDataType.Surface | ChannelDataType.Brep, ChannelArrayType.Array, ChannelFlag.NotRequired);
		InGemShape = new CSInputChannel(this, ChannelDataType.Gem, ChannelArrayType.Individual);
		InCount = new CSInputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual, 10);
		InDiameterStart = new CSInputChannel(this, ChannelDataType.Gem, ChannelArrayType.Individual);
		InDiameterMiddle = new CSInputChannel(this, ChannelDataType.Gem, ChannelArrayType.Individual);
		InDiameterEnd = new CSInputChannel(this, ChannelDataType.Gem, ChannelArrayType.Individual);
		InPosition = new CSInputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual, 0.0);
		InAmountToMiddle = new CSInputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual, 50);
		InPlacementStart = new CSInputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual, 0.0);
		InPlacementMiddle = new CSInputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual, 0.0);
		InPlacementEnd = new CSInputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual, 0.0);
		InSpacing = new CSInputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual, 0.0);
		InRoll = new CSInputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual, 0.0);
		InTilt = new CSInputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual, 0.0);
		InOrientationMethod = new CSInputChannel(this, ChannelDataType.Integer, ChannelArrayType.Individual, 17);
		InPlacementMethod = new CSInputChannel(this, ChannelDataType.Integer, ChannelArrayType.Individual, 30);
		InEnableDiameterMiddle = new CSInputChannel(this, ChannelDataType.Boolean, ChannelArrayType.Individual, defaultValue: false);
		InEnableDiameterEnd = new CSInputChannel(this, ChannelDataType.Boolean, ChannelArrayType.Individual, defaultValue: false);
		InEnablePlacementMiddle = new CSInputChannel(this, ChannelDataType.Boolean, ChannelArrayType.Individual, defaultValue: false);
		InEnablePlacementEnd = new CSInputChannel(this, ChannelDataType.Boolean, ChannelArrayType.Individual, defaultValue: false);
		InDoFlip = new CSInputChannel(this, ChannelDataType.Boolean, ChannelArrayType.Individual, defaultValue: false);
		InDoMirror = new CSInputChannel(this, ChannelDataType.Boolean, ChannelArrayType.Individual, defaultValue: false);
		InDoReverseDirection = new CSInputChannel(this, ChannelDataType.Boolean, ChannelArrayType.Individual, defaultValue: false);
		InGemsOnOff = new CSInputChannel(this, ChannelDataType.Boolean, ChannelArrayType.Individual, defaultValue: false);
		OutArray = new CSOutputChannel(this, ChannelDataType.Gem, ChannelArrayType.Array);
		OutDisplayGems = new CSOutputChannel(this, ChannelDataType.Gem, ChannelArrayType.Array);
		SetChannelIdentifiers(autoCheckInputs: true);
	}

	public override bool Update()
	{
		NurbsCurve nurbsCurve = InCurve.NurbsCurve;
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
		int integer = InCount.Integer;
		double num4 = InAmountToMiddle.Percentage;
		double num5 = InPlacementStart.Double;
		double placementMiddle = InPlacementEnd.Double;
		double num6 = InPlacementEnd.Double;
		double num7 = InRoll.Double;
		double num8 = InTilt.Double;
		ValueListItem valueListItem = InOrientationMethod.ValueListItem;
		ValueListItem valueListItem2 = InPlacementMethod.ValueListItem;
		bool boolean = InEnableDiameterMiddle.Boolean;
		bool boolean2 = InEnableDiameterEnd.Boolean;
		bool boolean3 = InEnablePlacementMiddle.Boolean;
		bool boolean4 = InEnablePlacementEnd.Boolean;
		bool boolean5 = InDoFlip.Boolean;
		bool boolean6 = InDoMirror.Boolean;
		bool boolean7 = InDoReverseDirection.Boolean;
		CSDataArray cSDataArray = new CSDataArray();
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
		if ((_crvInfos == null || HasThisObjectChangedSinceLastUpdate() || BeenUpdatedAfterLastUpdate(InCurve.OutputChannelBlock)) && !CSUtilGemOnCrv.PreProcessCrv(nurbsCurve, out _crvInfos))
		{
			return UpdateError("PreProcessCrv failed");
		}
		Transform transform = CSBlockUtil.CSUtilMath.Rotation(CSBlockUtil.CSUtilMath.ToRadians(num8), Vector3d.XAxis, Point3d.Origin);
		Transform xform = CSBlockUtil.CSUtilMath.Rotation(CSBlockUtil.CSUtilMath.ToRadians(num7), Vector3d.ZAxis, Point3d.Origin) * transform;
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
		if (!CSUtilGemOnCrv.GetClosestIndexToPosition(nurbsCurve, _crvInfos, InPosition.Double, out var index))
		{
			return UpdateError("GetClosestIndexToPosition failed");
		}
		int num9 = 1;
		if (boolean7)
		{
			num9 *= -1;
		}
		if (!CalculateGemInfos(integer, gem, num4, index, num9, gemXSize, gemYSize, num, heightMiddle, num2, num3, rotationalAdjustmentFactor, rotationalAdjustmentFactor2, rotationalAdjustmentFactor3, InSpacing.Double, num5, placementMiddle, num6, out var gemOnCurveInfos))
		{
			return UpdateError("CalculateGemInfos failed");
		}
		if (boolean6)
		{
			CalculateGemInfosMirror(num9, ref gemOnCurveInfos);
		}
		foreach (CSUtilGemOnCrv.GemOnCurveInfo item in gemOnCurveInfos)
		{
			Transform transform2 = CSUtilGemOnCrv.CalculateGemXform(_crvInfos, item, brep, gem, num7, num8, valueListItem, valueListItem2, boolean5);
			CSData data = gem.Duplicate();
			CSDataHelper.Transform(data, transform2);
			cSDataArray.Append(data);
		}
		OutArray.SetData(cSDataArray);
		if (InGemsOnOff.Boolean)
		{
			OutDisplayGems.SetData(cSDataArray);
		}
		return base.Update();
	}

	private bool CalculateGemInfos(int count, CSGemData sourceGem, double amountToMiddle, int indexStart, int direction, double widthStart, double heightStart, double widthMiddle, double heightMiddle, double widthEnd, double heightEnd, double rotationalAdjustmentFactorStart, double rotationalAdjustmentFactorMiddle, double rotationalAdjustmentFactorEnd, double spacing, double placementStart, double placementMiddle, double placementEnd, out List<CSUtilGemOnCrv.GemOnCurveInfo> gemOnCurveInfos)
	{
		gemOnCurveInfos = new List<CSUtilGemOnCrv.GemOnCurveInfo>();
		if (count <= 0)
		{
			return true;
		}
		int num = indexStart;
		int num2 = (int)Math.Round(amountToMiddle * (double)count, MidpointRounding.AwayFromZero);
		int num3 = 0;
		while (num3 < num2)
		{
			double num4 = ((num2 == 1) ? 0.0 : ((double)num3 / ((double)num2 - 1.0)));
			double num5 = widthStart + (widthMiddle - widthStart) * num4;
			double num6 = heightStart + (heightMiddle - heightStart) * num4;
			double widthFromTable;
			double heightFromTable;
			if (!_isUsingCustomGemSizes)
			{
				if (!CSUtilGemOnCrv.GetWidthHeightFromTable(sourceGem, num5, num6, out widthFromTable, out heightFromTable))
				{
					return UpdateError("GetWidthHeightFromTable failed");
				}
			}
			else
			{
				widthFromTable = Math.Round(num5, 1, MidpointRounding.AwayFromZero);
				heightFromTable = Math.Round(num6, 1, MidpointRounding.AwayFromZero);
			}
			double num7 = widthFromTable / 2.0;
			double num8 = rotationalAdjustmentFactorStart + (rotationalAdjustmentFactorMiddle - rotationalAdjustmentFactorStart) * num4;
			double placement = placementStart + (placementMiddle - placementStart) * num4;
			CSUtilGemOnCrv.GemOnCurveInfo gemOnCurveInfo = new CSUtilGemOnCrv.GemOnCurveInfo
			{
				Radius = num7,
				Height = heightFromTable,
				RadiusAdjusted = num7 * num8,
				Placement = placement,
				Index = num
			};
			bool flag = true;
			if (gemOnCurveInfos.Count >= 1)
			{
				CSUtilGemOnCrv.GemOnCurveInfo gemRhs = gemOnCurveInfos[gemOnCurveInfos.Count - 1];
				flag = CheckSpacing(gemOnCurveInfo, gemRhs, spacing);
			}
			if (flag)
			{
				gemOnCurveInfos.Add(gemOnCurveInfo);
				num3++;
			}
			num += direction;
		}
		int num9 = ((num2 == 0) ? count : (count - num2));
		if (num2 > 1)
		{
			num9++;
		}
		int num10 = ((num2 > 1) ? 1 : 0);
		while (num10 < num9)
		{
			double num11 = ((num9 == 1) ? 0.0 : ((double)num10 / ((double)num9 - 1.0)));
			double num12 = widthMiddle + (widthEnd - widthMiddle) * num11;
			double num13 = heightMiddle + (heightEnd - heightMiddle) * num11;
			double widthFromTable2;
			double heightFromTable2;
			if (!_isUsingCustomGemSizes)
			{
				if (!CSUtilGemOnCrv.GetWidthHeightFromTable(sourceGem, num12, num13, out widthFromTable2, out heightFromTable2))
				{
					return UpdateError("GetWidthHeightFromTable failed");
				}
			}
			else
			{
				widthFromTable2 = Math.Round(num12, 1, MidpointRounding.AwayFromZero);
				heightFromTable2 = Math.Round(num13, 1, MidpointRounding.AwayFromZero);
			}
			double num14 = widthFromTable2 / 2.0;
			double num15 = rotationalAdjustmentFactorMiddle + (rotationalAdjustmentFactorEnd - rotationalAdjustmentFactorMiddle) * num11;
			double placement2 = placementMiddle + (placementEnd - placementMiddle) * num11;
			CSUtilGemOnCrv.GemOnCurveInfo gemOnCurveInfo2 = new CSUtilGemOnCrv.GemOnCurveInfo
			{
				Radius = num14,
				Height = heightFromTable2,
				RadiusAdjusted = num14 * num15,
				Placement = placement2,
				Index = num
			};
			bool flag2 = true;
			if (gemOnCurveInfos.Count >= 1)
			{
				CSUtilGemOnCrv.GemOnCurveInfo gemRhs2 = gemOnCurveInfos[gemOnCurveInfos.Count - 1];
				flag2 = CheckSpacing(gemOnCurveInfo2, gemRhs2, spacing);
			}
			if (flag2)
			{
				gemOnCurveInfos.Add(gemOnCurveInfo2);
				num10++;
			}
			num += direction;
		}
		return true;
	}

	private void CalculateGemInfosMirror(int direction, ref List<CSUtilGemOnCrv.GemOnCurveInfo> gemOnCurveInfos)
	{
		if (gemOnCurveInfos.Count < 2)
		{
			return;
		}
		int num = gemOnCurveInfos.Count - 2;
		int num2 = gemOnCurveInfos.Count - 1;
		int num3 = CSUtilGemOnCrv.GetFixedIndex(_crvInfos, gemOnCurveInfos[num2].Index);
		int num4 = gemOnCurveInfos.Count - 1;
		int num5 = 0;
		while (num5 < num4)
		{
			CSUtilGemOnCrv.GemOnCurveInfo gemOnCurveInfo = gemOnCurveInfos[num];
			CSUtilGemOnCrv.GemOnCurveInfo gemOnCurveInfo2 = gemOnCurveInfos[num2];
			Point3d ptRc = _crvInfos[CSUtilGemOnCrv.GetFixedIndex(_crvInfos, gemOnCurveInfo.Index)].PtRc;
			Point3d ptRc2 = _crvInfos[CSUtilGemOnCrv.GetFixedIndex(_crvInfos, gemOnCurveInfo2.Index)].PtRc;
			double spacing = ptRc.DistanceTo(ptRc2) - (gemOnCurveInfo.RadiusAdjusted + gemOnCurveInfo2.RadiusAdjusted);
			CSUtilGemOnCrv.GemOnCurveInfo gemOnCurveInfo3 = gemOnCurveInfos[num];
			gemOnCurveInfo3.Index = num3;
			bool flag = true;
			if (gemOnCurveInfos.Count >= 1)
			{
				CSUtilGemOnCrv.GemOnCurveInfo gemRhs = gemOnCurveInfos[gemOnCurveInfos.Count - 1];
				flag = CheckSpacing(gemOnCurveInfo3, gemRhs, spacing);
			}
			if (flag)
			{
				gemOnCurveInfos.Add(gemOnCurveInfo3);
				num--;
				num2--;
				num5++;
			}
			num3 += direction;
		}
	}

	private bool CheckSpacing(CSUtilGemOnCrv.GemOnCurveInfo gemLhs, CSUtilGemOnCrv.GemOnCurveInfo gemRhs, double spacing)
	{
		double num = gemLhs.RadiusAdjusted + gemRhs.RadiusAdjusted + spacing;
		Point3d ptRc = _crvInfos[CSUtilGemOnCrv.GetFixedIndex(_crvInfos, gemLhs.Index)].PtRc;
		Point3d ptRc2 = _crvInfos[CSUtilGemOnCrv.GetFixedIndex(_crvInfos, gemRhs.Index)].PtRc;
		return ptRc.DistanceTo(ptRc2) >= num;
	}
}
