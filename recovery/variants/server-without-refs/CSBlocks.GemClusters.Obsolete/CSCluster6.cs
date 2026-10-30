using System;
using System.Collections.Generic;
using CS.CSDataUtilities;
using CS.CSObjects.CSBlocks;
using CS.Parametric;
using CS.Parametric.Channels;
using CSBlockDataUtils.CSGemOffsetCurves;
using CSBlockUtil;
using CSCompatibility;
using CSDatas;
using CSGemInstance.GemTableData;
using GV.CounterSketch.Shared;
using RMA.OpenNURBS;
using Rhino;
using Rhino.Geometry;
using Rhino.Runtime;

namespace CSBlocks.GemClusters.Obsolete;

[Obsolete]
[CSBlock("Cluster_V6", "{2A2EF475-969A-4517-8253-576CA24FB3E7}")]
public sealed class CSCluster6 : CSBlock
{
	public readonly CSInputChannel InCenterGem;

	public readonly CSInputChannel InCenterGemShapeOverrideEnabled;

	public readonly CSInputChannel InCenterGemShapeOverride;

	public readonly CSInputChannel InCenterGemSpacing;

	public readonly CSInputChannel InGemDefinition;

	public readonly CSInputChannel InSpacing;

	public readonly CSInputChannel InAngle;

	public readonly CSInputChannel InRotation;

	public readonly CSInputChannel InPlacement;

	public readonly CSInputChannel InFixPosition;

	public readonly CSInputChannel InStyle;

	public readonly CSInputChannel InEven;

	public readonly CSInputChannel InProfileWidth;

	public readonly CSInputChannel InChannelWallThicknessInner;

	public readonly CSInputChannel InChannelWallThicknessOuter;

	public readonly CSInputChannel InChannelSpacing;

	public readonly CSInputChannel InShowChannel;

	public readonly CSInputChannel InGemHeightOffset;

	public readonly CSInputChannel InXYScaleToGem;

	public readonly CSInputChannel InCenterGemShapeOverrideMode;

	public readonly CSInputChannel InFixedGemSpacing;

	public readonly CSInputChannel InRailX;

	public readonly CSInputChannel InRailZ;

	public readonly CSInputChannel InGemSpacingTolerance;

	public readonly CSOutputChannel OutGem;

	public readonly CSOutputChannel OutGemSizeZ;

	public readonly CSOutputChannel OutChannelFix;

	public CSCluster6(string identifier)
		: base(identifier)
	{
		InCenterGem = new CSInputChannel(this, ChannelDataType.Gem, ChannelArrayType.Array | ChannelArrayType.Individual);
		InCenterGemShapeOverrideEnabled = new CSInputChannel(this, ChannelDataType.Boolean, ChannelArrayType.Individual);
		InCenterGemShapeOverride = new CSInputChannel(this, ChannelDataType.Gem, ChannelArrayType.Individual);
		InCenterGemSpacing = new CSInputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual);
		InGemDefinition = new CSInputChannel(this, ChannelDataType.Gem, ChannelArrayType.Individual);
		InSpacing = new CSInputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual);
		InAngle = new CSInputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual);
		InRotation = new CSInputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual);
		InPlacement = new CSInputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual);
		InFixPosition = new CSInputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual);
		InStyle = new CSInputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual);
		InEven = new CSInputChannel(this, ChannelDataType.Boolean, ChannelArrayType.Individual);
		InProfileWidth = new CSInputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual);
		InChannelWallThicknessInner = new CSInputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual);
		InChannelWallThicknessOuter = new CSInputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual);
		InChannelSpacing = new CSInputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual);
		InShowChannel = new CSInputChannel(this, ChannelDataType.Boolean, ChannelArrayType.Individual);
		InGemHeightOffset = new CSInputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual);
		InXYScaleToGem = new CSInputChannel(this, ChannelDataType.Boolean, ChannelArrayType.Individual);
		InCenterGemShapeOverrideMode = new CSInputChannel(this, ChannelDataType.Integer, ChannelArrayType.Individual);
		InFixedGemSpacing = new CSInputChannel(this, ChannelDataType.Boolean, ChannelArrayType.Individual);
		InRailX = new CSInputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual);
		InRailZ = new CSInputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual);
		InGemSpacingTolerance = new CSInputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual);
		OutGem = new CSOutputChannel(this, ChannelDataType.Gem, ChannelArrayType.Array);
		OutGemSizeZ = new CSOutputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual);
		OutChannelFix = new CSOutputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual);
		SetChannelIdentifiers();
	}

	public override bool Update()
	{
		bool value = true;
		InShowChannel.GetBoolean(ref value);
		CSDataArray flattenArray = InCenterGem.GetFlattenArray();
		if (flattenArray.GetArrayCount() <= 0)
		{
			return UpdateMissing("CenterGem");
		}
		if (!(flattenArray.GetArrayItem(0) is CSGemData centerGem))
		{
			return UpdateMissing("CenterGem");
		}
		CSGemData centerGemShapeOverride = InCenterGemShapeOverride.Data as CSGemData;
		if (!(InGemDefinition.Data is CSGemData cSGemData))
		{
			return UpdateMissing("Gem Definition");
		}
		double value2 = 0.5;
		InCenterGemSpacing.GetDouble(ref value2);
		double value3 = 0.0;
		InSpacing.GetDouble(ref value3);
		double value4 = 0.0;
		InAngle.GetDouble(ref value4);
		value4 = CSBlockUtil.CSUtilMath.ToRadians(value4);
		double value5 = 0.0;
		InRotation.GetDouble(ref value5);
		value5 = CSBlockUtil.CSUtilMath.ToRadians(value5);
		double value6 = 0.0;
		InPlacement.GetDouble(ref value6);
		double value7 = 0.0;
		InFixPosition.GetDouble(ref value7);
		double value8 = 0.0;
		InProfileWidth.GetDouble(ref value8);
		double value9 = 0.0;
		InChannelWallThicknessInner.GetDouble(ref value9);
		double value10 = 0.0;
		InChannelWallThicknessOuter.GetDouble(ref value10);
		double value11 = 0.0;
		InChannelSpacing.GetDouble(ref value11);
		int value12 = 0;
		InStyle.GetInteger(ref value12);
		bool value13 = false;
		InCenterGemShapeOverrideEnabled.GetBoolean(ref value13);
		bool value14 = false;
		InEven.GetBoolean(ref value14);
		double value15 = 0.0;
		InGemHeightOffset.GetDouble(ref value15);
		bool value16 = false;
		InXYScaleToGem.GetBoolean(ref value16);
		ValueListItem whichItem = ValueListItem.GemClusterCenterShapeOverrideModeDimensionSmallest;
		InCenterGemShapeOverrideMode.GetValueListItem(ref whichItem);
		bool value17 = false;
		InFixedGemSpacing.GetBoolean(ref value17);
		double value18 = 0.0;
		InGemSpacingTolerance.GetDouble(ref value18);
		CSDataArray flattenArray2 = InRailX.GetFlattenArray();
		double value19 = 0.0;
		if (flattenArray2.GetArrayCount() >= 1)
		{
			flattenArray2.GetArrayItem(0).GetDouble(ref value19);
		}
		value2 += value19;
		CSDataArray flattenArray3 = InRailZ.GetFlattenArray();
		double value20 = 0.0;
		if (flattenArray3.GetArrayCount() >= 1)
		{
			flattenArray3.GetArrayItem(0).GetDouble(ref value20);
		}
		value6 += value20;
		if (!value)
		{
			OutGemSizeZ.SetData(null);
			OutGem.SetData(null);
			return base.Update();
		}
		if (!ClusterGem(centerGem, value13, centerGemShapeOverride, value2, cSGemData, value3, value4, value5, value6, value7, value12, value14, value8, value9, value10, value11, value15, value16, whichItem, value17, value18, out var outputGems, out var gemSpacingFix, out var outError))
		{
			return UpdateError(outError);
		}
		OutGem.SetData(outputGems);
		OutGemSizeZ.SetData(new CSNumberData(cSGemData.GemZSize));
		OutChannelFix.SetData(new CSNumberData(gemSpacingFix));
		return base.Update();
	}

	private static bool ClusterGem(CSGemData centerGem, bool centerGemShapeOverrideEnabled, CSGemData centerGemShapeOverride, double centerGemSpacing, CSGemData gemDefinition, double spacing, double angle, double rotation, double placement, double fixPosition, int style, bool isEven, double profileWidth, double channelWallThicknessInner, double channelWallThicknessOuter, double channelSpacing, double gemHeightOffset, bool doScaleToGemXY, ValueListItem centerGemShapeOverrideMode, bool fixedGemSpacing, double tolerance, out CSDataArray outputGems, out double gemSpacingFix, out string outError)
	{
		//IL_0014: Unknown result type (might be due to invalid IL or missing references)
		//IL_001a: Expected O, but got Unknown
		//IL_0201: Unknown result type (might be due to invalid IL or missing references)
		//IL_0208: Expected O, but got Unknown
		//IL_021c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0223: Expected O, but got Unknown
		//IL_0249: Unknown result type (might be due to invalid IL or missing references)
		//IL_0250: Expected O, but got Unknown
		//IL_0263: Unknown result type (might be due to invalid IL or missing references)
		//IL_0268: Unknown result type (might be due to invalid IL or missing references)
		//IL_026a: Unknown result type (might be due to invalid IL or missing references)
		//IL_026f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0290: Expected O, but got Unknown
		//IL_02a4: Unknown result type (might be due to invalid IL or missing references)
		//IL_02ab: Unknown result type (might be due to invalid IL or missing references)
		//IL_02b5: Expected O, but got Unknown
		//IL_00d8: Unknown result type (might be due to invalid IL or missing references)
		//IL_00df: Expected O, but got Unknown
		//IL_0107: Unknown result type (might be due to invalid IL or missing references)
		//IL_010e: Expected O, but got Unknown
		//IL_0381: Unknown result type (might be due to invalid IL or missing references)
		//IL_0397: Unknown result type (might be due to invalid IL or missing references)
		//IL_038f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0394: Unknown result type (might be due to invalid IL or missing references)
		outputGems = new CSDataArray();
		gemSpacingFix = 0.0;
		OnXform val = new OnXform();
		val.Rotation(rotation, OnUtil.On_zaxis, OnUtil.On_origin);
		double num = profileWidth - (channelWallThicknessInner + channelWallThicknessOuter);
		double num2 = num - channelSpacing * 2.0;
		double bestGemSizeX = -1.23432101234321E+308;
		double num3 = -1.23432101234321E+308;
		if (num == 0.0)
		{
			bestGemSizeX = gemDefinition.GemXSize;
			num3 = gemDefinition.GemYSize;
		}
		else
		{
			GemSize gemSize = null;
			double num4 = -1.23432101234321E+308;
			foreach (GemSize size in gemDefinition.Sizes)
			{
				double num5 = size.X / gemDefinition.GemXSize;
				double num6 = size.Y / gemDefinition.GemYSize;
				double num7 = (num5 + num6) / 2.0;
				OnXform val2 = new OnXform();
				val2.Scale(num5, num6, num7);
				OnNurbsCurve obj = gemDefinition.GetNurbsCurve().Convert();
				obj.Transform((IOnXform)(object)(val * (IOnXform)(object)val2));
				OnBoundingBox val3 = new OnBoundingBox();
				if (!((OnGeometry)obj).GetTightBoundingBox(ref val3))
				{
					outError = "gem Curve bounding Box failed";
					return false;
				}
				double num8 = Math.Abs(val3.Max().x) + Math.Abs(val3.Min().x);
				double num9 = Math.Abs(val3.Max().y) + Math.Abs(val3.Min().y);
				double num10 = Math.Abs(num2 - num9);
				if (!RhinoMath.IsValidDouble(num4) || num10 < num4)
				{
					gemSize = size;
					bestGemSizeX = num8;
					num3 = num9;
					num4 = num10;
				}
			}
			gemDefinition.ScaleToSize(gemSize.X, gemSize.Y);
		}
		double num11 = gemDefinition.GemXSize / 2.0 - gemDefinition.GemXMax;
		double num12 = gemDefinition.GemYSize / 2.0 - gemDefinition.GemYMax;
		OnXform val4 = new OnXform();
		val4.Translation(num11, num12, 0.0);
		OnXform val5 = new OnXform();
		val5.Translation(0.0, 0.0, 0.0 - Math.Abs(gemDefinition.GemZMax));
		OnXform val6 = new OnXform();
		val6.Rotation(angle, OnUtil.On_xaxis, OnUtil.On_origin);
		Transform identity = Transform.Identity;
		OnXform val7 = new OnXform();
		val7.Translation(0.0, 0.0, placement);
		OnXform xTransformLocal = val7 * (IOnXform)(object)val6 * (IOnXform)(object)val5 * (IOnXform)(object)val4 * (IOnXform)(object)val * (IOnXform)(OnXform)Interop.ToOnXform(identity);
		string gemShape;
		if (centerGemShapeOverrideEnabled)
		{
			if (centerGemShapeOverride == null)
			{
				centerGemShapeOverride = new CSGemData(centerGem);
			}
			else
			{
				double scaleFactorX;
				switch (centerGemShapeOverrideMode)
				{
				case ValueListItem.GemClusterCenterShapeOverrideModeDimensionX:
					scaleFactorX = centerGem.GemXSize / centerGemShapeOverride.GemXSize;
					break;
				case ValueListItem.GemClusterCenterShapeOverrideModeDimensionY:
					scaleFactorX = centerGem.GemYSize / centerGemShapeOverride.GemYSize;
					break;
				case ValueListItem.GemClusterCenterShapeOverrideModeDimensionLargest:
					scaleFactorX = ((!(centerGem.GemXSize > centerGem.GemYSize)) ? (centerGem.GemYSize / centerGemShapeOverride.GemYSize) : (centerGem.GemXSize / centerGemShapeOverride.GemXSize));
					break;
				case ValueListItem.GemClusterCenterShapeOverrideModeDimensionSmallest:
					scaleFactorX = ((!(centerGem.GemXSize < centerGem.GemYSize)) ? (centerGem.GemYSize / centerGemShapeOverride.GemYSize) : (centerGem.GemXSize / centerGemShapeOverride.GemXSize));
					break;
				default:
					outError = "Invalid Gem Cluster Center Shape Override Mode";
					return false;
				}
				centerGemShapeOverride.Scale(scaleFactorX);
				if (!CSBlockUtil.CSUtilMath.GetUnscaledXform(centerGem.Xform, out var xUp))
				{
					xUp = Transform.Identity;
				}
				centerGemShapeOverride.ApplyTransform(xUp);
			}
			gemShape = centerGemShapeOverride.GemShape;
		}
		else
		{
			centerGemShapeOverride = new CSGemData(centerGem);
			gemShape = centerGem.GemShape;
		}
		double offsetRailZ;
		double num13;
		if (num == 0.0)
		{
			num13 = centerGemSpacing + num3 / 2.0 - gemHeightOffset * Math.Sin(angle);
			offsetRailZ = 0.0 + gemHeightOffset * Math.Cos(angle);
		}
		else
		{
			num13 = centerGemSpacing + (channelWallThicknessInner + num / 2.0) * Math.Cos(angle) - gemHeightOffset * Math.Sin(angle);
			offsetRailZ = (channelWallThicknessInner + num / 2.0) * Math.Sin(angle) + gemHeightOffset * Math.Cos(angle);
		}
		CSGemData offsetControlShape = (doScaleToGemXY ? centerGem : centerGemShapeOverride);
		if (!fixedGemSpacing)
		{
			if (!GetGemLayout(gemShape, offsetControlShape, gemDefinition, bestGemSizeX, spacing, num13, offsetRailZ, (IOnXform)(object)xTransformLocal, fixPosition, isEven, style, out outputGems, out outError))
			{
				return false;
			}
			gemSpacingFix = 0.0;
		}
		else
		{
			double num14 = num13;
			CSDataArray cSDataArray = null;
			double num15 = -1.0;
			num13 -= 0.1;
			bool flag = false;
			bool flag2 = true;
			if (!GetGemLayout(gemShape, offsetControlShape, gemDefinition, bestGemSizeX, spacing, num13, offsetRailZ, (IOnXform)(object)xTransformLocal, fixPosition, isEven, style, out outputGems, out outError))
			{
				return false;
			}
			num15 = GetSpacing(outputGems);
			num13 += 0.1;
			while (true)
			{
				if (!GetGemLayout(gemShape, offsetControlShape, gemDefinition, bestGemSizeX, spacing, num13, offsetRailZ, (IOnXform)(object)xTransformLocal, fixPosition, isEven, style, out outputGems, out outError))
				{
					return false;
				}
				double spacing2 = GetSpacing(outputGems);
				if (flag2 && spacing2 - spacing < tolerance)
				{
					cSDataArray = outputGems;
					flag = true;
					break;
				}
				flag2 = false;
				if (spacing2 < num15)
				{
					cSDataArray = outputGems;
					num13 -= 0.01;
					num15 = spacing2;
					break;
				}
				num13 += 0.1;
				num15 = spacing2;
			}
			if (!flag)
			{
				while (true)
				{
					if (!GetGemLayout(gemShape, offsetControlShape, gemDefinition, bestGemSizeX, spacing, num13, offsetRailZ, (IOnXform)(object)xTransformLocal, fixPosition, isEven, style, out outputGems, out outError))
					{
						return false;
					}
					double spacing3 = GetSpacing(outputGems);
					if (spacing3 > num15)
					{
						break;
					}
					cSDataArray = outputGems;
					num13 -= 0.01;
					num15 = spacing3;
				}
			}
			outputGems = cSDataArray;
			gemSpacingFix = num13 - num14;
		}
		outError = "";
		return true;
	}

	private static double CosAng(On3dVector v0, On3dVector v1)
	{
		v0.Unitize();
		v1.Unitize();
		return v0.x * v1.x + v0.y * v1.y;
	}

	private static bool GetGemLayout(string centerGemShape, CSGemData offsetControlShape, CSGemData gemDefinition, double bestGemSizeX, double spacing, double offsetRailX, double offsetRailZ, IOnXform xTransformLocal, double fixPosition, bool isEven, int style, out CSDataArray outputGems, out string outError)
	{
		outputGems = new CSDataArray();
		switch (centerGemShape)
		{
		case "Cushion":
		case "Cushion Rotated 90":
		case "Oval":
		case "Oval Rotated 90":
		case "Round":
			if (!PlaceGemsRound(centerGemShape, offsetControlShape, gemDefinition, bestGemSizeX, spacing, offsetRailX, offsetRailZ, xTransformLocal, fixPosition, isEven, ref outputGems))
			{
				outError = "PlaceGemsRound failed";
				return false;
			}
			break;
		case "Cushion Square":
		case "Princess":
		case "Baguette Straight":
			if (!PlaceGemsSquare(centerGemShape, offsetControlShape, gemDefinition, bestGemSizeX, spacing, offsetRailX, offsetRailZ, xTransformLocal, style, ref outputGems))
			{
				outError = "PlaceGemsSquare failed";
				return false;
			}
			break;
		case "Cushion Square Rotated 45":
		case "Princess Rotated 45":
			if (!PlaceGemsSquareRotated(centerGemShape, offsetControlShape, gemDefinition, bestGemSizeX, spacing, offsetRailX, offsetRailZ, xTransformLocal, style, ref outputGems))
			{
				outError = "PlaceGemsSquareRotated failed";
				return false;
			}
			break;
		case "Emerald":
		case "Emerald Rotated 90":
		case "Emerald Square":
		case "Emerald Square Rotated 45":
		case "Octagon":
		case "Radiant":
		case "Radiant Square":
			if (!PlaceGemsEmerald(centerGemShape, offsetControlShape, gemDefinition, bestGemSizeX, spacing, offsetRailX, offsetRailZ, xTransformLocal, ref outputGems))
			{
				outError = "PlaceGemsEmerald failed";
				return false;
			}
			break;
		case "Heart":
			if (!PlaceGemsHeart(centerGemShape, offsetControlShape, gemDefinition, bestGemSizeX, spacing, offsetRailX, offsetRailZ, xTransformLocal, ref outputGems))
			{
				outError = "PlaceGemsHeart failed";
				return false;
			}
			break;
		case "Marquise":
			if (!PlaceGemsMarquise(centerGemShape, offsetControlShape, gemDefinition, bestGemSizeX, spacing, offsetRailX, offsetRailZ, xTransformLocal, ref outputGems))
			{
				outError = "PlaceGemsMarquise failed";
				return false;
			}
			break;
		case "Pear":
			if (!PlaceGemsPear(centerGemShape, offsetControlShape, gemDefinition, bestGemSizeX, spacing, offsetRailX, offsetRailZ, xTransformLocal, ref outputGems))
			{
				outError = "PlaceGemsPear failed";
				return false;
			}
			break;
		case "Marquise Rotated 90":
			if (!PlaceGemsMarquiseRotated(centerGemShape, offsetControlShape, gemDefinition, bestGemSizeX, spacing, offsetRailX, offsetRailZ, xTransformLocal, ref outputGems))
			{
				outError = "PlaceGemsMarquiseRotated failed";
				return false;
			}
			break;
		default:
			outError = "Unknown gem space for cluster " + centerGemShape;
			return false;
		}
		outError = "";
		return true;
	}

	private static double GetSide(double cosAng, double a, double c)
	{
		double num = 1.0;
		double num2 = -2.0 * a * cosAng;
		double num3 = 0.0 - c * c + a * a;
		return (0.0 - num2 + Math.Sqrt(num2 * num2 - 4.0 * num * num3)) / (2.0 * num);
	}

	private static double GetSpacing(CSDataArray gemarray)
	{
		//IL_001e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0024: Expected O, but got Unknown
		//IL_0032: Unknown result type (might be due to invalid IL or missing references)
		//IL_0038: Expected O, but got Unknown
		CSGemData obj = gemarray.GetArrayItem(0) as CSGemData;
		CSGemData data = gemarray.GetArrayItem(1) as CSGemData;
		On3dPoint value = new On3dPoint(OnUtil.On_origin);
		CSDataHelper.GetPoint(obj, ref value);
		On3dPoint value2 = new On3dPoint(OnUtil.On_origin);
		CSDataHelper.GetPoint(data, ref value2);
		double num = obj.GemXSize / 2.0;
		double num2 = obj.GemYSize / 2.0;
		return (value - (IOn3dPoint)(object)value2).Length() - num - num2;
	}

	private static double GetUnnormalizedParam(int gemCurrentIndex, int gemCount, int style)
	{
		if (style == 0)
		{
			return (double)gemCurrentIndex / (double)gemCount;
		}
		double num = 1.0 / (2.0 * Math.Sqrt(2.0) + (double)(2 * (gemCount - 1)));
		return num * Math.Sqrt(2.0) + num * 2.0 * (double)gemCurrentIndex;
	}

	private static bool PlaceGemCorner(IOnCurve curveLhs, IOnCurve curveRhs, CSGemData centerGem, CSData gemDefinition, IOnXform xTransformLocal, CSDataArray outputGems)
	{
		//IL_0024: Unknown result type (might be due to invalid IL or missing references)
		//IL_0029: Unknown result type (might be due to invalid IL or missing references)
		//IL_0078: Unknown result type (might be due to invalid IL or missing references)
		//IL_0095: Unknown result type (might be due to invalid IL or missing references)
		//IL_009c: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a1: Unknown result type (might be due to invalid IL or missing references)
		On3dPoint val = curveLhs.PointAtEnd();
		On3dPoint val2 = curveRhs.PointAtStart();
		if (val.DistanceTo((IOn3dPoint)(object)val2) > 0.001)
		{
			return false;
		}
		OnLineCurve val3 = new OnLineCurve((IOn3dPoint)(object)val, (IOn3dPoint)(object)val2);
		On3dPoint destination = ((OnCurve)val3).PointAt(((OnCurve)val3).Domain().Mid());
		On3dVector obj = curveLhs.TangentAt(curveLhs.Domain().Max());
		On3dVector val4 = curveRhs.TangentAt(curveRhs.Domain().Min());
		On3dVector destination2 = obj + (IOn3dVector)(object)val4;
		OnXform xform = CSBlockUtil.CSUtilMath.Orient3Point(OnUtil.On_origin, OnUtil.On_xaxis, OnUtil.On_zaxis, (IOn3dPoint)(object)destination, (IOn3dVector)(object)destination2, (IOn3dVector)(object)centerGem.Up.Convert());
		CSData data = gemDefinition.Duplicate();
		CSDataHelper.Transform(data, xform.Convert() * xTransformLocal.Convert());
		outputGems.Append(data);
		return true;
	}

	private static bool PlaceGemCorners(IList<IOnCurve> curves, CSGemData centerGem, CSData gemDefinition, IOnXform xTransformLocal, CSDataArray outputGems)
	{
		for (int i = 0; i < curves.Count; i++)
		{
			int num = i;
			int num2 = num + 1;
			if (num2 >= curves.Count)
			{
				num2 -= curves.Count;
			}
			if (!PlaceGemCorner(curves[num], curves[num2], centerGem, gemDefinition, xTransformLocal, outputGems))
			{
				return false;
			}
		}
		return true;
	}

	private static bool PlaceGemsCurveSide(IOnCurve curve, CSGemData centerGem, CSGemData gemDefinition, double gemDefinitionSizeX, double spacing, int style, IOnXform xTransformLocal, CSDataArray outputGems)
	{
		//IL_0084: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a3: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c0: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c7: Unknown result type (might be due to invalid IL or missing references)
		//IL_00cc: Unknown result type (might be due to invalid IL or missing references)
		double num = 0.0;
		if (!curve.GetLength(ref num))
		{
			return false;
		}
		int num2 = (int)(num / (gemDefinitionSizeX + spacing));
		if (num < (double)num2 * gemDefinitionSizeX + (double)Math.Max(num2 - 1, 0) * spacing)
		{
			num2--;
		}
		double[] array = new double[num2];
		double[] array2 = new double[num2];
		for (int i = 0; i < num2; i++)
		{
			array[i] = GetUnnormalizedParam(i, num2, style);
		}
		curve.GetNormalizedArcLengthPoints(array, ref array2);
		for (int j = ((style == 0) ? 1 : 0); j < num2; j++)
		{
			OnXform xform = CSBlockUtil.CSUtilMath.Orient3Point(OnUtil.On_origin, OnUtil.On_xaxis, (IOn3dVector)(object)gemDefinition.Up.Convert(), (IOn3dPoint)(object)curve.PointAt(array2[j]), (IOn3dVector)(object)curve.TangentAt(array2[j]), (IOn3dVector)(object)centerGem.Up.Convert());
			CSData data = gemDefinition.Duplicate();
			CSDataHelper.Transform(data, xform.Convert() * xTransformLocal.Convert());
			outputGems.Append(data);
		}
		return true;
	}

	private static bool PlaceGemsEmerald(string centerGemShape, CSGemData centerGem, CSGemData gemDefinition, double gemDefinitionSizeX, double spacing, double offsetRailX, double offsetRailZ, IOnXform xTransformLocal, ref CSDataArray outputGems)
	{
		//IL_003d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0047: Unknown result type (might be due to invalid IL or missing references)
		//IL_0053: Expected O, but got Unknown
		//IL_0094: Unknown result type (might be due to invalid IL or missing references)
		//IL_009e: Expected O, but got Unknown
		OnCurve[] array = GemOffsetCurveV2.GemOffsetCurvesOld(centerGemShape, centerGem, offsetRailX, doTransformUp: true);
		if (array.Length != 9)
		{
			return false;
		}
		OnCurve[] array2 = array;
		for (int i = 0; i < array2.Length; i++)
		{
			((OnGeometry)array2[i]).Translate((IOn3dVector)(object)(new On3dVector((IOn3dVector)(object)centerGem.Normal.Convert()) * offsetRailZ));
		}
		OnCurve val = array[0];
		OnCurve val2 = array[1];
		OnCurve val3 = array[2];
		OnCurve val4 = val3.Duplicate();
		val4.Trim((IOnInterval)new OnInterval(val4.Domain().Min(), val3.Domain().Mid()));
		double cosAng = CosAng(val.PointAtStart() - (IOn3dPoint)(object)val.PointAtEnd(), val2.PointAtEnd() - (IOn3dPoint)(object)val2.PointAtStart());
		double cosAng2 = CosAng(val2.PointAtStart() - (IOn3dPoint)(object)val2.PointAtEnd(), val4.PointAtEnd() - (IOn3dPoint)(object)val4.PointAtStart());
		double num = 0.0;
		val.GetLength(ref num);
		double num2 = 0.0;
		val2.GetLength(ref num2);
		double num3 = 0.0;
		val4.GetLength(ref num3);
		double num4 = 0.0;
		bool flag = true;
		int num5 = -1;
		double num6;
		double num7;
		int num8;
		double side;
		int num9;
		double side2;
		int num10;
		double num11;
		do
		{
			num6 = gemDefinitionSizeX + spacing + num4;
			num7 = 0.0;
			num8 = (int)((num - num7) / num6);
			double a = num - (num7 + num6 * (double)num8);
			side = GetSide(cosAng, a, num6);
			num9 = (int)((num2 - side) / num6);
			double a2 = num2 - (side + num6 * (double)num9);
			side2 = GetSide(cosAng2, a2, num6);
			num10 = (int)((num3 - side2) / num6);
			num11 = num3 - (side2 + num6 * (double)num10);
			if (flag)
			{
				num5 = ((num11 > num6 / 2.0) ? 1 : 0);
				if (num5 == 1)
				{
					num11 -= num6 / 2.0;
				}
				int num12 = num8 + num9 + num10 + 3 - 1;
				num4 += num11 / (double)num12 * 0.9;
				flag = false;
				continue;
			}
			double num13 = num11 / num6;
			if (num13 < 0.25)
			{
				int num14 = num8 + num9 + num10 + 3 - 1;
				num4 += num11 / (double)num14 * 0.9;
			}
			else if (num13 < 0.75)
			{
				num11 -= num6 / 2.0;
				int num15 = num8 + num9 + num10 + 3 - 1;
				num4 += num11 / (double)num15 * 0.9;
			}
			else
			{
				num11 -= num6;
				num10++;
				int num16 = num8 + num9 + num10 + 3 - 1;
				num4 += num11 / (double)num16 * 0.9;
			}
		}
		while (num11 > 0.01);
		double num17 = num3 + num6 * (1.0 - (double)num5 / 2.0);
		int num18 = num10 - 1 + num5;
		for (int j = 0; j <= num8; j++)
		{
			PlaceGemsEmeraldSide(array, 0, gemDefinitionSizeX, num7 + (double)j * num6, centerGem, gemDefinition, xTransformLocal, ref outputGems);
		}
		for (int k = 0; k <= num9; k++)
		{
			PlaceGemsEmeraldSide(array, 1, gemDefinitionSizeX, side + (double)k * num6, centerGem, gemDefinition, xTransformLocal, ref outputGems);
		}
		for (int l = 0; l <= num10; l++)
		{
			PlaceGemsEmeraldSide(array, 2, gemDefinitionSizeX, side2 + (double)l * num6, centerGem, gemDefinition, xTransformLocal, ref outputGems);
		}
		for (int m = 0; m <= num18; m++)
		{
			PlaceGemsEmeraldSide(array, 2, gemDefinitionSizeX, num17 + (double)m * num6, centerGem, gemDefinition, xTransformLocal, ref outputGems);
		}
		for (int num19 = num9; num19 >= 0; num19--)
		{
			PlaceGemsEmeraldSide(array, 3, gemDefinitionSizeX, num2 - (side + (double)num19 * num6), centerGem, gemDefinition, xTransformLocal, ref outputGems);
		}
		for (int num20 = num8; num20 >= 0; num20--)
		{
			PlaceGemsEmeraldSide(array, 4, gemDefinitionSizeX, num - (num7 + (double)num20 * num6), centerGem, gemDefinition, xTransformLocal, ref outputGems);
		}
		for (int n = 1; n <= num8; n++)
		{
			PlaceGemsEmeraldSide(array, 4, gemDefinitionSizeX, num + (num7 + (double)n * num6), centerGem, gemDefinition, xTransformLocal, ref outputGems);
		}
		for (int num21 = 0; num21 <= num9; num21++)
		{
			PlaceGemsEmeraldSide(array, 5, gemDefinitionSizeX, side + (double)num21 * num6, centerGem, gemDefinition, xTransformLocal, ref outputGems);
		}
		for (int num22 = num18; num22 >= 0; num22--)
		{
			PlaceGemsEmeraldSide(array, 6, gemDefinitionSizeX, num3 * 2.0 - (num17 + (double)num22 * num6), centerGem, gemDefinition, xTransformLocal, ref outputGems);
		}
		for (int num23 = num10; num23 >= 0; num23--)
		{
			PlaceGemsEmeraldSide(array, 6, gemDefinitionSizeX, num3 * 2.0 - (side2 + (double)num23 * num6), centerGem, gemDefinition, xTransformLocal, ref outputGems);
		}
		for (int num24 = num9; num24 >= 0; num24--)
		{
			PlaceGemsEmeraldSide(array, 7, gemDefinitionSizeX, num2 - (side + (double)num24 * num6), centerGem, gemDefinition, xTransformLocal, ref outputGems);
		}
		for (int num25 = num8; num25 >= 1; num25--)
		{
			PlaceGemsEmeraldSide(array, 8, gemDefinitionSizeX, num - (num7 + (double)num25 * num6), centerGem, gemDefinition, xTransformLocal, ref outputGems);
		}
		return true;
	}

	private static void PlaceGemsEmeraldSide(IList<OnCurve> curves, int crvIndex, double sizeX, double distance, CSGemData centerGem, CSGemData gemDefinition, IOnXform xOrientLocal, ref CSDataArray outputGems)
	{
		//IL_0028: Unknown result type (might be due to invalid IL or missing references)
		//IL_002e: Expected O, but got Unknown
		//IL_0196: Unknown result type (might be due to invalid IL or missing references)
		//IL_019d: Unknown result type (might be due to invalid IL or missing references)
		//IL_01ab: Unknown result type (might be due to invalid IL or missing references)
		//IL_01ba: Expected O, but got Unknown
		//IL_01c9: Unknown result type (might be due to invalid IL or missing references)
		//IL_01d0: Unknown result type (might be due to invalid IL or missing references)
		//IL_01d5: Unknown result type (might be due to invalid IL or missing references)
		On3dPoint val = curves[crvIndex].PointAtStart();
		On3dVector val2 = curves[crvIndex].PointAtEnd() - (IOn3dPoint)(object)val;
		val2.Unitize();
		On3dVector destination = new On3dVector((IOn3dVector)(object)val2);
		double num = sizeX / 2.0;
		if (distance < num)
		{
			int num2 = crvIndex - 1;
			if (num2 == -1)
			{
				num2 = curves.Count - 1;
			}
			On3dVector val3 = curves[num2].PointAtEnd() - (IOn3dPoint)(object)curves[num2].PointAtStart();
			val3.Unitize();
			double num3 = distance;
			if (num3 < 0.0)
			{
				num3 = 0.0;
			}
			double num4 = num3 / num;
			double num5 = 1.0 - num4;
			destination = val2 * num4 + (IOn3dVector)(object)(val3 * num5);
		}
		else
		{
			double num6 = 0.0;
			curves[crvIndex].GetLength(ref num6);
			if (num6 - distance < num)
			{
				int index = (crvIndex + 1) % curves.Count;
				On3dVector val4 = curves[index].PointAtEnd() - (IOn3dPoint)(object)curves[index].PointAtStart();
				val4.Unitize();
				double num7 = num6 - distance;
				if (num7 < 0.0)
				{
					num7 = 0.0;
				}
				double num8 = num7 / num;
				double num9 = 1.0 - num8;
				destination = val2 * num8 + (IOn3dVector)(object)(val4 * num9);
			}
		}
		val += (IOn3dVector)(object)(val2 * distance);
		OnXform xform = CSBlockUtil.CSUtilMath.Orient3Point(OnUtil.On_origin, (IOn3dVector)new On3dVector(1.0, 0.0, 0.0), (IOn3dVector)(object)gemDefinition.Normal.Convert(), (IOn3dPoint)(object)val, (IOn3dVector)(object)destination, (IOn3dVector)(object)centerGem.Normal.Convert());
		CSData data = gemDefinition.Duplicate();
		CSDataHelper.Transform(data, xform.Convert() * xOrientLocal.Convert());
		outputGems.Append(data);
	}

	private static bool PlaceGemsHeart(string centerGemShape, CSGemData centerGem, CSGemData gemDefinition, double gemDefinitionSizeX, double spacing, double offsetRailX, double offsetRailZ, IOnXform xTransformLocal, ref CSDataArray outputGems)
	{
		//IL_003f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0045: Unknown result type (might be due to invalid IL or missing references)
		//IL_004f: Unknown result type (might be due to invalid IL or missing references)
		//IL_005b: Expected O, but got Unknown
		IOnCurve[] array = (IOnCurve[])(object)GemOffsetCurveV2.GemOffsetCurvesOld(centerGemShape, centerGem, offsetRailX, doTransformUp: true);
		IOnCurve[] array2 = array;
		if (array2.Length != 6)
		{
			return false;
		}
		array = array2;
		for (int i = 0; i < array.Length; i++)
		{
			((OnGeometry)(OnCurve)array[i]).Translate((IOn3dVector)(object)(new On3dVector((IOn3dVector)(object)centerGem.Normal.Convert()) * offsetRailZ));
		}
		if (!CSUtilCurves.MergeCurves((IOnCurve[])(object)new IOnCurve[3]
		{
			array2[0],
			array2[1],
			array2[2]
		}, out OnCurve mergedCrv, 0.01))
		{
			return false;
		}
		if (!CSUtilCurves.MergeCurves((IOnCurve[])(object)new IOnCurve[3]
		{
			array2[3],
			array2[4],
			array2[5]
		}, out OnCurve mergedCrv2, 0.01))
		{
			return false;
		}
		CSDataArray cSDataArray = new CSDataArray();
		if (!PlaceGemCorners((IList<IOnCurve>)(object)new OnCurve[2] { mergedCrv2, mergedCrv }, centerGem, gemDefinition, xTransformLocal, cSDataArray))
		{
			return false;
		}
		outputGems.Append(cSDataArray.GetArrayItem(0));
		if (!PlaceGemsCurveSide((IOnCurve)(object)mergedCrv, centerGem, gemDefinition, gemDefinitionSizeX, spacing, 0, xTransformLocal, outputGems))
		{
			return false;
		}
		outputGems.Append(cSDataArray.GetArrayItem(1));
		if (!PlaceGemsCurveSide((IOnCurve)(object)mergedCrv2, centerGem, gemDefinition, gemDefinitionSizeX, spacing, 0, xTransformLocal, outputGems))
		{
			return false;
		}
		return true;
	}

	private static bool PlaceGemsMarquise(string centerGemShape, CSGemData centerGem, CSGemData gemDefinition, double gemDefinitionSizeX, double spacing, double offsetRailX, double offsetRailZ, IOnXform xTransformLocal, ref CSDataArray outputGems)
	{
		//IL_003f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0045: Unknown result type (might be due to invalid IL or missing references)
		//IL_004f: Unknown result type (might be due to invalid IL or missing references)
		//IL_005b: Expected O, but got Unknown
		IOnCurve[] array = (IOnCurve[])(object)GemOffsetCurveV2.GemOffsetCurvesOld(centerGemShape, centerGem, offsetRailX, doTransformUp: true);
		IOnCurve[] array2 = array;
		if (array2.Length != 2)
		{
			return false;
		}
		array = array2;
		for (int i = 0; i < array.Length; i++)
		{
			((OnGeometry)(OnCurve)array[i]).Translate((IOn3dVector)(object)(new On3dVector((IOn3dVector)(object)centerGem.Normal.Convert()) * offsetRailZ));
		}
		IOnCurve val = array2[0];
		IOnCurve val2 = array2[1];
		CSDataArray cSDataArray = new CSDataArray();
		if (!PlaceGemCorners((IList<IOnCurve>)(object)new IOnCurve[2] { val2, val }, centerGem, gemDefinition, xTransformLocal, cSDataArray))
		{
			return false;
		}
		outputGems.Append(cSDataArray.GetArrayItem(0));
		if (!PlaceGemsCurveSide(val, centerGem, gemDefinition, gemDefinitionSizeX, spacing, 0, xTransformLocal, outputGems))
		{
			return false;
		}
		outputGems.Append(cSDataArray.GetArrayItem(1));
		if (!PlaceGemsCurveSide(val2, centerGem, gemDefinition, gemDefinitionSizeX, spacing, 0, xTransformLocal, outputGems))
		{
			return false;
		}
		return true;
	}

	private static bool PlaceGemsMarquiseRotated(string centerGemShape, CSGemData centerGem, CSGemData gemDefinition, double gemDefinitionSizeX, double spacing, double offsetRailX, double offsetRailZ, IOnXform xTransformLocal, ref CSDataArray outputGems)
	{
		//IL_003f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0045: Unknown result type (might be due to invalid IL or missing references)
		//IL_004f: Unknown result type (might be due to invalid IL or missing references)
		//IL_005b: Expected O, but got Unknown
		IOnCurve[] array = (IOnCurve[])(object)GemOffsetCurveV2.GemOffsetCurvesOld(centerGemShape, centerGem, offsetRailX, doTransformUp: true);
		IOnCurve[] array2 = array;
		if (array2.Length != 4)
		{
			return false;
		}
		array = array2;
		for (int i = 0; i < array.Length; i++)
		{
			((OnGeometry)(OnCurve)array[i]).Translate((IOn3dVector)(object)(new On3dVector((IOn3dVector)(object)centerGem.Normal.Convert()) * offsetRailZ));
		}
		IOnCurve val = array2[0];
		IOnCurve val2 = array2[1];
		IOnCurve val3 = array2[2];
		IOnCurve val4 = array2[3];
		IOnCurve[] inputCrvs = (IOnCurve[])(object)new IOnCurve[2] { val4, val };
		IOnCurve[] inputCrvs2 = (IOnCurve[])(object)new IOnCurve[2] { val2, val3 };
		if (!CSUtilCurves.MergeCurves(inputCrvs, out OnCurve mergedCrv, 0.01))
		{
			return false;
		}
		if (!CSUtilCurves.MergeCurves(inputCrvs2, out OnCurve mergedCrv2, 0.01))
		{
			return false;
		}
		CSDataArray cSDataArray = new CSDataArray();
		if (!PlaceGemCorners((IList<IOnCurve>)(object)new OnCurve[2] { mergedCrv, mergedCrv2 }, centerGem, gemDefinition, xTransformLocal, cSDataArray))
		{
			return false;
		}
		CSDataArray cSDataArray2 = new CSDataArray();
		if (!PlaceGemsCurveSide((IOnCurve)(object)mergedCrv, centerGem, gemDefinition, gemDefinitionSizeX, spacing, 0, xTransformLocal, cSDataArray2))
		{
			return false;
		}
		int num = cSDataArray2.GetArrayCount() / 2;
		for (int j = num; j < cSDataArray2.GetArrayCount(); j++)
		{
			outputGems.Append(cSDataArray2.GetArrayItem(j));
		}
		outputGems.Append(cSDataArray.GetArrayItem(0));
		if (!PlaceGemsCurveSide((IOnCurve)(object)mergedCrv2, centerGem, gemDefinition, gemDefinitionSizeX, spacing, 0, xTransformLocal, outputGems))
		{
			return false;
		}
		outputGems.Append(cSDataArray.GetArrayItem(1));
		for (int k = 0; k < num; k++)
		{
			outputGems.Append(cSDataArray2.GetArrayItem(k));
		}
		return true;
	}

	private static bool PlaceGemsPear(string centerGemShape, CSGemData centerGem, CSGemData gemDefinition, double gemDefinitionSizeX, double spacing, double offsetRailX, double offsetRailZ, IOnXform xTransformLocal, ref CSDataArray outputGems)
	{
		//IL_003f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0045: Unknown result type (might be due to invalid IL or missing references)
		//IL_004f: Unknown result type (might be due to invalid IL or missing references)
		//IL_005b: Expected O, but got Unknown
		IOnCurve[] array = (IOnCurve[])(object)GemOffsetCurveV2.GemOffsetCurvesOld(centerGemShape, centerGem, offsetRailX, doTransformUp: true);
		IOnCurve[] array2 = array;
		if (array2.Length != 4)
		{
			return false;
		}
		array = array2;
		for (int i = 0; i < array.Length; i++)
		{
			((OnGeometry)(OnCurve)array[i]).Translate((IOn3dVector)(object)(new On3dVector((IOn3dVector)(object)centerGem.Normal.Convert()) * offsetRailZ));
		}
		if (!CSUtilCurves.MergeCurves((IOnCurve[])(object)new IOnCurve[2]
		{
			array2[0],
			array2[1]
		}, out OnCurve mergedCrv, 0.01))
		{
			return false;
		}
		if (!CSUtilCurves.MergeCurves((IOnCurve[])(object)new IOnCurve[2]
		{
			array2[2],
			array2[3]
		}, out OnCurve mergedCrv2, 0.01))
		{
			return false;
		}
		CSDataArray cSDataArray = new CSDataArray();
		if (!PlaceGemCorners((IList<IOnCurve>)(object)new OnCurve[2] { mergedCrv2, mergedCrv }, centerGem, gemDefinition, xTransformLocal, cSDataArray))
		{
			return false;
		}
		outputGems.Append(cSDataArray.GetArrayItem(0));
		if (!PlaceGemsCurveSide((IOnCurve)(object)mergedCrv, centerGem, gemDefinition, gemDefinitionSizeX, spacing, 0, xTransformLocal, outputGems))
		{
			return false;
		}
		outputGems.Append(cSDataArray.GetArrayItem(1));
		if (!PlaceGemsCurveSide((IOnCurve)(object)mergedCrv2, centerGem, gemDefinition, gemDefinitionSizeX, spacing, 0, xTransformLocal, outputGems))
		{
			return false;
		}
		return true;
	}

	private static bool PlaceGemsRound(string centerGemShape, CSGemData centerGem, CSGemData gemDefinition, double gemDefinitionSizeX, double spacing, double offsetRailX, double offsetRailZ, IOnXform xTransformLocal, double fixPosition, bool isEven, ref CSDataArray outputGems)
	{
		//IL_002f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0039: Unknown result type (might be due to invalid IL or missing references)
		//IL_0045: Expected O, but got Unknown
		//IL_00f7: Unknown result type (might be due to invalid IL or missing references)
		//IL_0118: Unknown result type (might be due to invalid IL or missing references)
		//IL_0135: Unknown result type (might be due to invalid IL or missing references)
		//IL_013c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0141: Unknown result type (might be due to invalid IL or missing references)
		if (!GemOffsetCurveV2.GemOffset(centerGemShape, centerGem, offsetRailX, out OnCurve curve, doTransformUp: true, out string _, 0.0, 0.0, 0.0))
		{
			return false;
		}
		((OnGeometry)curve).Translate((IOn3dVector)(object)(new On3dVector((IOn3dVector)(object)centerGem.Normal.Convert()) * offsetRailZ));
		double num = 0.0;
		if (!curve.GetLength(ref num))
		{
			return false;
		}
		int num2 = (int)(num / (gemDefinitionSizeX + spacing));
		if (isEven && num2 % 2 == 1)
		{
			num2--;
		}
		double[] array = new double[num2];
		double[] array2 = new double[num2];
		double num3 = 1.0 / (double)num2;
		double num4 = fixPosition % num3;
		for (int i = 0; i < num2; i++)
		{
			array[i] = num4;
			num4 += num3;
			if (num4 > 1.0)
			{
				num4 -= 1.0;
			}
		}
		Array.Sort(array);
		curve.GetNormalizedArcLengthPoints(array, ref array2);
		for (int j = 0; j < num2; j++)
		{
			OnXform xform = CSBlockUtil.CSUtilMath.Orient3Point(OnUtil.On_origin, OnUtil.On_xaxis, (IOn3dVector)(object)gemDefinition.Up.Convert(), (IOn3dPoint)(object)curve.PointAt(array2[j]), (IOn3dVector)(object)curve.TangentAt(array2[j]), (IOn3dVector)(object)centerGem.Up.Convert());
			CSData data = gemDefinition.Duplicate();
			CSDataHelper.Transform(data, xform.Convert() * xTransformLocal.Convert());
			outputGems.Append(data);
		}
		return true;
	}

	private static bool PlaceGemsSquare(string centerGemShape, CSGemData centerGem, CSGemData gemDefinition, double gemDefinitionSizeX, double spacing, double offsetRailX, double offsetRailZ, IOnXform xTransformLocal, int style, ref CSDataArray outputGems)
	{
		//IL_003c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0046: Unknown result type (might be due to invalid IL or missing references)
		//IL_0052: Expected O, but got Unknown
		OnCurve[] array = GemOffsetCurveV2.GemOffsetCurvesOld(centerGemShape, centerGem, offsetRailX, doTransformUp: true);
		if (array.Length != 5)
		{
			return false;
		}
		OnCurve[] array2 = array;
		for (int i = 0; i < array2.Length; i++)
		{
			((OnGeometry)array2[i]).Translate((IOn3dVector)(object)(new On3dVector((IOn3dVector)(object)centerGem.Normal.Convert()) * offsetRailZ));
		}
		IOnCurve val = (IOnCurve)(object)array[0];
		IOnCurve val2 = (IOnCurve)(object)array[1];
		IOnCurve val3 = (IOnCurve)(object)array[2];
		IOnCurve val4 = (IOnCurve)(object)array[3];
		IOnCurve val5 = (IOnCurve)(object)array[4];
		if (!CSUtilCurves.MergeCurves((IOnCurve[])(object)new IOnCurve[2] { val, val5 }, out OnCurve mergedCrv, 0.01))
		{
			return false;
		}
		CSDataArray cSDataArray = null;
		if (style == 0)
		{
			cSDataArray = new CSDataArray();
			if (!PlaceGemCorners((IList<IOnCurve>)(object)new IOnCurve[4]
			{
				(IOnCurve)mergedCrv,
				val2,
				val3,
				val4
			}, centerGem, gemDefinition, xTransformLocal, cSDataArray))
			{
				return false;
			}
		}
		CSDataArray cSDataArray2 = new CSDataArray();
		if (!PlaceGemsCurveSide((IOnCurve)(object)mergedCrv, centerGem, gemDefinition, gemDefinitionSizeX, spacing, style, xTransformLocal, cSDataArray2))
		{
			return false;
		}
		int num = cSDataArray2.GetArrayCount() / 2;
		for (int j = num; j < cSDataArray2.GetArrayCount(); j++)
		{
			outputGems.Append(cSDataArray2.GetArrayItem(j));
		}
		if (style == 0)
		{
			outputGems.Append(cSDataArray.GetArrayItem(0));
		}
		if (!PlaceGemsCurveSide(val2, centerGem, gemDefinition, gemDefinitionSizeX, spacing, style, xTransformLocal, outputGems))
		{
			return false;
		}
		if (style == 0)
		{
			outputGems.Append(cSDataArray.GetArrayItem(1));
		}
		if (!PlaceGemsCurveSide(val3, centerGem, gemDefinition, gemDefinitionSizeX, spacing, style, xTransformLocal, outputGems))
		{
			return false;
		}
		if (style == 0)
		{
			outputGems.Append(cSDataArray.GetArrayItem(2));
		}
		if (!PlaceGemsCurveSide(val4, centerGem, gemDefinition, gemDefinitionSizeX, spacing, style, xTransformLocal, outputGems))
		{
			return false;
		}
		if (style == 0)
		{
			outputGems.Append(cSDataArray.GetArrayItem(3));
		}
		for (int k = 0; k < num; k++)
		{
			outputGems.Append(cSDataArray2.GetArrayItem(k));
		}
		return true;
	}

	private static bool PlaceGemsSquareRotated(string centerGemShape, CSGemData centerGem, CSGemData gemDefinition, double gemDefinitionSizeX, double spacing, double offsetRailX, double offsetRailZ, IOnXform xTransformLocal, int style, ref CSDataArray outputGems)
	{
		//IL_0038: Unknown result type (might be due to invalid IL or missing references)
		//IL_0042: Unknown result type (might be due to invalid IL or missing references)
		//IL_004e: Expected O, but got Unknown
		OnCurve[] array = GemOffsetCurveV2.GemOffsetCurvesOld(centerGemShape, centerGem, offsetRailX, doTransformUp: true);
		if (array.Length != 4)
		{
			return false;
		}
		OnCurve[] array2 = array;
		for (int i = 0; i < array2.Length; i++)
		{
			((OnGeometry)array2[i]).Translate((IOn3dVector)(object)(new On3dVector((IOn3dVector)(object)centerGem.Normal.Convert()) * offsetRailZ));
		}
		CSDataArray cSDataArray = null;
		if (style == 0)
		{
			cSDataArray = new CSDataArray();
			if (!PlaceGemCorners((IList<IOnCurve>)(object)new OnCurve[4]
			{
				array[0],
				array[1],
				array[2],
				array[3]
			}, centerGem, gemDefinition, xTransformLocal, cSDataArray))
			{
				return false;
			}
		}
		if (!PlaceGemsCurveSide((IOnCurve)(object)array[0], centerGem, gemDefinition, gemDefinitionSizeX, spacing, style, xTransformLocal, outputGems))
		{
			return false;
		}
		if (style == 0)
		{
			outputGems.Append(cSDataArray.GetArrayItem(0));
		}
		if (!PlaceGemsCurveSide((IOnCurve)(object)array[1], centerGem, gemDefinition, gemDefinitionSizeX, spacing, style, xTransformLocal, outputGems))
		{
			return false;
		}
		if (style == 0)
		{
			outputGems.Append(cSDataArray.GetArrayItem(1));
		}
		if (!PlaceGemsCurveSide((IOnCurve)(object)array[2], centerGem, gemDefinition, gemDefinitionSizeX, spacing, style, xTransformLocal, outputGems))
		{
			return false;
		}
		if (style == 0)
		{
			outputGems.Append(cSDataArray.GetArrayItem(2));
		}
		if (!PlaceGemsCurveSide((IOnCurve)(object)array[3], centerGem, gemDefinition, gemDefinitionSizeX, spacing, style, xTransformLocal, outputGems))
		{
			return false;
		}
		if (style == 0)
		{
			outputGems.Append(cSDataArray.GetArrayItem(3));
		}
		return true;
	}
}
