using System;
using System.Collections.Generic;
using CS.CSDataUtilities;
using CS.CSObjects.CSBlocks;
using CS.CSValues.Interfaces;
using CS.Parametric;
using CS.Parametric.Channels;
using CSBlockDataUtils;
using CSBlockUtil;
using CSCompatibility;
using CSDatas;
using GV.CounterSketch.Shared;
using RMA.OpenNURBS;

namespace CSBlocks.Gems.Obsolete;

[Obsolete]
[CSBlock("GemOnCrvCount_V3", "{57627795-F43D-4769-8A4E-0357A79A3FEA}")]
public sealed class CSGemOnCrvCount3 : CSBlock
{
	private IList<CSUtilGemOnCrv.CurveInfo> _crvInfos;

	private bool _isUsingCustomGemSizes;

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

	public readonly CSOutputChannel OutArray;

	public CSGemOnCrvCount3(string identifier)
		: base(identifier)
	{
		InCurve = new CSInputChannel(this, ChannelDataType.Curve, ChannelArrayType.Individual);
		InSurface = new CSInputChannel(this, ChannelDataType.Brep, ChannelArrayType.Array | ChannelArrayType.Individual, ChannelFlag.NotRequired);
		InGemShape = new CSInputChannel(this, ChannelDataType.Gem, ChannelArrayType.Individual);
		InCount = new CSInputChannel(this, ChannelDataType.Integer, ChannelArrayType.Individual, 10);
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
		InOrientationMethod = new CSInputChannel(this, ChannelDataType.Integer, ChannelArrayType.Individual, ValueListItem.GemOrientationMethodAxisY);
		InPlacementMethod = new CSInputChannel(this, ChannelDataType.Integer, ChannelArrayType.Individual, ValueListItem.GemPlacementMethodGirdle);
		InEnableDiameterMiddle = new CSInputChannel(this, ChannelDataType.Boolean, ChannelArrayType.Individual, defaultValue: false);
		InEnableDiameterEnd = new CSInputChannel(this, ChannelDataType.Boolean, ChannelArrayType.Individual, defaultValue: false);
		InEnablePlacementMiddle = new CSInputChannel(this, ChannelDataType.Boolean, ChannelArrayType.Individual, defaultValue: false);
		InEnablePlacementEnd = new CSInputChannel(this, ChannelDataType.Boolean, ChannelArrayType.Individual, defaultValue: false);
		InDoFlip = new CSInputChannel(this, ChannelDataType.Boolean, ChannelArrayType.Individual, defaultValue: false);
		InDoMirror = new CSInputChannel(this, ChannelDataType.Boolean, ChannelArrayType.Individual, defaultValue: false);
		InDoReverseDirection = new CSInputChannel(this, ChannelDataType.Boolean, ChannelArrayType.Individual, defaultValue: false);
		OutArray = new CSOutputChannel(this, ChannelDataType.Gem, ChannelArrayType.Array);
		SetChannelIdentifiers(autoCheckInputs: true);
	}

	public override bool Update()
	{
		//IL_0276: Unknown result type (might be due to invalid IL or missing references)
		//IL_027d: Expected O, but got Unknown
		//IL_0295: Unknown result type (might be due to invalid IL or missing references)
		//IL_029a: Unknown result type (might be due to invalid IL or missing references)
		//IL_02b8: Expected O, but got Unknown
		//IL_02b8: Unknown result type (might be due to invalid IL or missing references)
		//IL_02bf: Expected O, but got Unknown
		OnCurve val = (OnCurve)(object)InCurve.NurbsCurve.Convert();
		if (val == null)
		{
			return UpdateMissing("Curve");
		}
		OnBrep val2 = null;
		CSDataArray flattenArray = InSurface.GetFlattenArray();
		if (flattenArray.GetArrayCount() > 0 && flattenArray.GetArrayItem(0) != null)
		{
			val2 = InSurface.Brep.Convert();
		}
		CSGemData gem = InGemShape.Gem;
		CSGemData gem2 = InDiameterStart.Gem;
		CSGemData gem3 = InDiameterMiddle.Gem;
		CSGemData gem4 = InDiameterEnd.Gem;
		double gemXSize = gem2.GemXSize;
		double num = gem3.GemXSize;
		double num2 = gem4.GemXSize;
		int integer = InCount.Integer;
		double pos = InPosition.Double;
		double num3 = InAmountToMiddle.Percentage;
		double num4 = InPlacementStart.Double;
		double placementMiddle = InPlacementEnd.Double;
		double num5 = InPlacementEnd.Double;
		double spacing = InSpacing.Double;
		double num6 = InRoll.Double;
		double num7 = InTilt.Double;
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
		if (val2 == null && valueListItem == ValueListItem.GemOrientationMethodSurface)
		{
			valueListItem = ValueListItem.GemOrientationMethodAxisY;
		}
		if (!boolean)
		{
			num3 = 1.0;
		}
		if (!boolean2)
		{
			num2 = gemXSize;
		}
		if (!boolean)
		{
			num = gemXSize + (num2 - gemXSize) * num3;
		}
		if (!boolean4)
		{
			num5 = num4;
		}
		if (!boolean3)
		{
			placementMiddle = num4 + (num5 - num4) * num3;
		}
		ICSValueGemSize iCSValueGemSize = InDiameterStart.OutputChannelBlock as ICSValueGemSize;
		ICSValueGemSize iCSValueGemSize2 = InDiameterEnd.OutputChannelBlock as ICSValueGemSize;
		ICSValueGemSize iCSValueGemSize3 = InDiameterMiddle.OutputChannelBlock as ICSValueGemSize;
		if (iCSValueGemSize != null && iCSValueGemSize2 != null && iCSValueGemSize3 != null)
		{
			_isUsingCustomGemSizes = iCSValueGemSize.IsCustomGemSize || (boolean2 && iCSValueGemSize2.IsCustomGemSize) || (boolean && iCSValueGemSize3.IsCustomGemSize);
		}
		if ((BeenUpdatedAfterLastUpdate(InCurve.OutputChannelBlock) || _crvInfos == null) && !CSUtilGemOnCrv.PreProcessCrv((IOnCurve)(object)val, out _crvInfos))
		{
			return UpdateError("PreProcessCrv failed");
		}
		OnXform val3 = new OnXform();
		val3.Rotation(CSBlockUtil.CSUtilMath.ToRadians(num7), OnUtil.On_xaxis, OnUtil.On_origin);
		OnXform val4 = new OnXform();
		val4.Rotation(CSBlockUtil.CSUtilMath.ToRadians(num6), OnUtil.On_zaxis, OnUtil.On_origin);
		OnXform xform = new OnXform((IOnXform)(object)(val4 * (IOnXform)(object)val3));
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
		if (!CSUtilGemOnCrv.GetClosestIndexToPosition((IOnCurve)(object)val, _crvInfos, pos, out var index))
		{
			return UpdateError("GetClosestIndexToPosition failed");
		}
		int num8 = 1;
		if (boolean7)
		{
			num8 *= -1;
		}
		if (!CalculateGemInfos(integer, gem, num3, index, num8, gemXSize, num, num2, rotationalAdjustmentFactor, rotationalAdjustmentFactor2, rotationalAdjustmentFactor3, spacing, num4, placementMiddle, num5, out var gemOnCurveInfos))
		{
			return UpdateError("CalculateGemInfos failed");
		}
		if (boolean6)
		{
			CalculateGemInfosMirror(num8, ref gemOnCurveInfos);
		}
		foreach (CSUtilGemOnCrv.GemOnCurveInfo item in gemOnCurveInfos)
		{
			OnXform xform2 = CSUtilGemOnCrv.CalculateGemXform(_crvInfos, item, (IOnBrep)(object)val2, gem, num6, num7, valueListItem, valueListItem2, boolean5);
			CSData data = gem.Duplicate();
			CSDataHelper.Transform(data, xform2.Convert());
			cSDataArray.Append(data);
		}
		OutArray.SetData(cSDataArray);
		return base.Update();
	}

	private bool CalculateGemInfos(int count, CSGemData sourceGem, double amountToMiddle, int indexStart, int direction, double diameterStart, double diameterMiddle, double diameterEnd, double rotationalAdjustmentFactorStart, double rotationalAdjustmentFactorMiddle, double rotationalAdjustmentFactorEnd, double spacing, double placementStart, double placementMiddle, double placementEnd, out List<CSUtilGemOnCrv.GemOnCurveInfo> gemOnCurveInfos)
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
			double num5 = diameterStart + (diameterMiddle - diameterStart) * num4;
			double diameterFromTable;
			if (!_isUsingCustomGemSizes)
			{
				if (!CSUtilGemOnCrv.GetDiameterFromTable(sourceGem, num5, out diameterFromTable))
				{
					return UpdateError("GetDiameterFromTable failed");
				}
			}
			else
			{
				diameterFromTable = Math.Round(num5, 1, MidpointRounding.AwayFromZero);
			}
			double num6 = diameterFromTable / 2.0;
			double num7 = rotationalAdjustmentFactorStart + (rotationalAdjustmentFactorMiddle - rotationalAdjustmentFactorStart) * num4;
			double placement = placementStart + (placementMiddle - placementStart) * num4;
			CSUtilGemOnCrv.GemOnCurveInfo gemOnCurveInfo = new CSUtilGemOnCrv.GemOnCurveInfo
			{
				Radius = num6,
				RadiusAdjusted = num6 * num7,
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
		int num8 = ((num2 == 0) ? count : (count - num2));
		if (num2 > 1)
		{
			num8++;
		}
		int num9 = ((num2 > 1) ? 1 : 0);
		while (num9 < num8)
		{
			double num10 = ((num8 == 1) ? 0.0 : ((double)num9 / ((double)num8 - 1.0)));
			double num11 = diameterMiddle + (diameterEnd - diameterMiddle) * num10;
			double diameterFromTable2;
			if (!_isUsingCustomGemSizes)
			{
				if (!CSUtilGemOnCrv.GetDiameterFromTable(sourceGem, num11, out diameterFromTable2))
				{
					return UpdateError("GetDiameterFromTable failed");
				}
			}
			else
			{
				diameterFromTable2 = Math.Round(num11, 1, MidpointRounding.AwayFromZero);
			}
			double num12 = diameterFromTable2 / 2.0;
			double num13 = rotationalAdjustmentFactorMiddle + (rotationalAdjustmentFactorEnd - rotationalAdjustmentFactorMiddle) * num10;
			double placement2 = placementMiddle + (placementEnd - placementMiddle) * num10;
			CSUtilGemOnCrv.GemOnCurveInfo gemOnCurveInfo2 = new CSUtilGemOnCrv.GemOnCurveInfo
			{
				Radius = num12,
				RadiusAdjusted = num12 * num13,
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
				num9++;
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
			IOn3dPoint pt = _crvInfos[CSUtilGemOnCrv.GetFixedIndex(_crvInfos, gemOnCurveInfo.Index)].Pt;
			IOn3dPoint pt2 = _crvInfos[CSUtilGemOnCrv.GetFixedIndex(_crvInfos, gemOnCurveInfo2.Index)].Pt;
			double spacing = pt.DistanceTo(pt2) - (gemOnCurveInfo.RadiusAdjusted + gemOnCurveInfo2.RadiusAdjusted);
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
		IOn3dPoint pt = _crvInfos[CSUtilGemOnCrv.GetFixedIndex(_crvInfos, gemLhs.Index)].Pt;
		IOn3dPoint pt2 = _crvInfos[CSUtilGemOnCrv.GetFixedIndex(_crvInfos, gemRhs.Index)].Pt;
		return pt.DistanceTo(pt2) >= num;
	}
}
