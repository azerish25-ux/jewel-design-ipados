using System;
using System.Collections.Generic;
using CS.CSDataUtilities;
using CS.CSObjects.CSBlocks;
using CS.Parametric;
using CS.Parametric.Channels;
using CSBlockDataUtils;
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
[CSBlock("Cluster_V10", "{3D66135F-6C57-4E92-9219-74E84472E206}")]
public sealed class CSCluster10 : CSBlock
{
	public readonly CSInputChannel InCenterObject;

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

	public readonly CSInputChannel InUseMinCuletDistance;

	public readonly CSInputChannel InMinCuletDistance;

	public readonly CSInputChannel InProfileHeight;

	public readonly CSInputChannel InCornerBlend;

	public readonly CSInputChannel InOffsetX;

	public readonly CSInputChannel InOffsetY;

	public readonly CSInputChannel InEmeraldCornerPoint1;

	public readonly CSInputChannel InEmeraldCornerPoint2;

	public readonly CSInputChannel InRotate;

	public readonly CSInputChannel InGemSizeToggle;

	public readonly CSInputChannel InGemSize;

	public readonly CSInputChannel InForceGems;

	public readonly CSOutputChannel OutGem;

	public readonly CSOutputChannel OutGemSizeZ;

	public readonly CSOutputChannel OutChannelFix;

	public readonly CSOutputChannel OutDisplayGem;

	public CSCluster10(string identifier)
		: base(identifier)
	{
		InCenterObject = new CSInputChannel(this, ChannelDataType.Any, ChannelArrayType.Array);
		InCenterGemShapeOverrideEnabled = new CSInputChannel(this, ChannelDataType.Boolean, ChannelArrayType.Individual, defaultValue: false);
		InCenterGemShapeOverride = new CSInputChannel(this, ChannelDataType.Gem, ChannelArrayType.Individual);
		InCenterGemSpacing = new CSInputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual, 0.5);
		InGemDefinition = new CSInputChannel(this, ChannelDataType.Gem, ChannelArrayType.Individual);
		InSpacing = new CSInputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual, 0.0);
		InAngle = new CSInputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual, 0.0);
		InRotation = new CSInputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual, 0.0);
		InPlacement = new CSInputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual, 0.0);
		InFixPosition = new CSInputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual, 0.0);
		InStyle = new CSInputChannel(this, ChannelDataType.Integer, ChannelArrayType.Individual, 0);
		InEven = new CSInputChannel(this, ChannelDataType.Boolean, ChannelArrayType.Individual, defaultValue: false);
		InProfileWidth = new CSInputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual, 0.0);
		InChannelWallThicknessInner = new CSInputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual, 0.0);
		InChannelWallThicknessOuter = new CSInputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual, 0.0);
		InChannelSpacing = new CSInputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual, 0.0);
		InShowChannel = new CSInputChannel(this, ChannelDataType.Boolean, ChannelArrayType.Individual, defaultValue: true);
		InGemHeightOffset = new CSInputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual, 0.0);
		InXYScaleToGem = new CSInputChannel(this, ChannelDataType.Boolean, ChannelArrayType.Individual, defaultValue: false);
		InCenterGemShapeOverrideMode = new CSInputChannel(this, ChannelDataType.Integer, ChannelArrayType.Individual, ValueListItem.GemClusterCenterShapeOverrideModeDimensionSmallest);
		InFixedGemSpacing = new CSInputChannel(this, ChannelDataType.Boolean, ChannelArrayType.Individual, defaultValue: false);
		InRailX = new CSInputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual, 0.0);
		InRailZ = new CSInputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual, 0.0);
		InGemSpacingTolerance = new CSInputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual, 0.0);
		InUseMinCuletDistance = new CSInputChannel(this, ChannelDataType.Integer, ChannelArrayType.Individual, ValueListItem.NoMinCulet);
		InMinCuletDistance = new CSInputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual, 0.0);
		InProfileHeight = new CSInputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual, 3.0);
		InCornerBlend = new CSInputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual, 0.0);
		InOffsetX = new CSInputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual, 0.0);
		InOffsetY = new CSInputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual, 0.0);
		InEmeraldCornerPoint1 = new CSInputChannel(this, ChannelDataType.Point, ChannelArrayType.Individual, Point3d.Unset);
		InEmeraldCornerPoint2 = new CSInputChannel(this, ChannelDataType.Point, ChannelArrayType.Individual, Point3d.Unset);
		InRotate = new CSInputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual, 0.0);
		InGemSizeToggle = new CSInputChannel(this, ChannelDataType.Boolean, ChannelArrayType.Individual, defaultValue: false);
		InGemSize = new CSInputChannel(this, ChannelDataType.Gem, ChannelArrayType.Individual);
		InForceGems = new CSInputChannel(this, ChannelDataType.Boolean, ChannelArrayType.Individual, defaultValue: false);
		OutGem = new CSOutputChannel(this, ChannelDataType.Gem, ChannelArrayType.Array);
		OutDisplayGem = new CSOutputChannel(this, ChannelDataType.Gem, ChannelArrayType.Array);
		OutGemSizeZ = new CSOutputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual);
		OutChannelFix = new CSOutputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual);
		SetChannelIdentifiers(autoCheckInputs: true);
	}

	public override bool Update()
	{
		bool flag = InShowChannel.Boolean;
		if (InForceGems.Boolean)
		{
			flag = true;
		}
		ValueListItem valueListItem = InUseMinCuletDistance.ValueListItem;
		double minCuletDistance = InMinCuletDistance.Double;
		double profileHeight = InProfileHeight.Double;
		List<CSGemData> gemList = InCenterObject.GemList;
		if (gemList.Count <= 0)
		{
			return UpdateMissing("CenterGem");
		}
		CSGemData cSGemData = gemList[0];
		CSData centerObject = gemList[0];
		CSGemData gem = InCenterGemShapeOverride.Gem;
		bool centerGemShapeOverrideEnabled = InCenterGemShapeOverrideEnabled.Boolean;
		if (cSGemData == null)
		{
			centerGemShapeOverrideEnabled = true;
		}
		CSGemData gem2 = InGemDefinition.Gem;
		double num = InCenterGemSpacing.Double;
		double spacing = InSpacing.Double;
		double angleDegrees = InAngle.Double;
		angleDegrees = CSBlockUtil.CSUtilMath.ToRadians(angleDegrees);
		double angleDegrees2 = InRotation.Double;
		angleDegrees2 = CSBlockUtil.CSUtilMath.ToRadians(angleDegrees2);
		double num2 = InPlacement.Double;
		double fixPosition = InFixPosition.Double;
		double profileWidth = InProfileWidth.Double;
		double channelWallThicknessInner = InChannelWallThicknessInner.Double;
		double channelWallThicknessOuter = InChannelWallThicknessOuter.Double;
		double channelSpacing = InChannelSpacing.Double;
		int integer = InStyle.Integer;
		double cornerBlend = InCornerBlend.Double;
		bool boolean = InEven.Boolean;
		double gemHeightOffset = InGemHeightOffset.Double;
		bool boolean2 = InXYScaleToGem.Boolean;
		ValueListItem valueListItem2 = InCenterGemShapeOverrideMode.ValueListItem;
		bool boolean3 = InFixedGemSpacing.Boolean;
		double tolerance = InGemSpacingTolerance.Double;
		double num3 = InRailX.Double;
		num += num3;
		double num4 = InRailZ.Double;
		num2 += num4;
		double offsetX = InOffsetX.Double;
		double offsetY = InOffsetY.Double;
		Point3d inputCorner = Point3d.Unset;
		Point3d inputCorner2 = Point3d.Unset;
		if (InEmeraldCornerPoint1.Data != null && InEmeraldCornerPoint2.Data != null)
		{
			inputCorner = InEmeraldCornerPoint1.Point3d;
			inputCorner2 = InEmeraldCornerPoint2.Point3d;
		}
		double rotate = InRotate.Double;
		bool boolean4 = InGemSizeToggle.Boolean;
		CSGemData gem3 = InGemSize.Gem;
		if (!ClusterGem(cSGemData, centerObject, centerGemShapeOverrideEnabled, gem, num, gem2, spacing, angleDegrees, angleDegrees2, num2, fixPosition, integer, cornerBlend, boolean, profileWidth, channelWallThicknessInner, channelWallThicknessOuter, channelSpacing, gemHeightOffset, boolean2, valueListItem2, boolean3, tolerance, valueListItem, minCuletDistance, profileHeight, offsetX, offsetY, inputCorner, inputCorner2, rotate, boolean4, gem3, out var outputGems, out var gemSpacingFix, out var outError))
		{
			return UpdateError(outError);
		}
		if (!flag)
		{
			OutDisplayGem.SetData(null);
			OutGemSizeZ.SetData(null);
			OutGem.SetData(outputGems);
		}
		else
		{
			OutGem.SetData(outputGems);
			OutDisplayGem.SetData(outputGems);
			OutGemSizeZ.SetData(new CSNumberData(gem2.GemZSize));
			OutChannelFix.SetData(new CSNumberData(gemSpacingFix));
		}
		return base.Update();
	}

	private static bool ClusterGem(CSGemData centerGem, CSData centerObject, bool centerGemShapeOverrideEnabled, CSGemData centerGemShapeOverride, double centerGemSpacing, CSGemData gemDefinition, double spacing, double angle, double rotation, double placement, double fixPosition, int style, double cornerBlend, bool isEven, double profileWidth, double channelWallThicknessInner, double channelWallThicknessOuter, double channelSpacing, double gemHeightOffset, bool doScaleToGemXY, ValueListItem centerGemShapeOverrideMode, bool fixedGemSpacing, double tolerance, ValueListItem useMinCuletDistance, double minCuletDistance, double profileHeight, double offsetX, double offsetY, Point3d inputCorner1, Point3d inputCorner2, double rotate, bool gemSizeToggle, CSGemData gemSize, out CSDataArray outputGems, out double gemSpacingFix, out string outError)
	{
		//IL_0014: Unknown result type (might be due to invalid IL or missing references)
		//IL_001a: Expected O, but got Unknown
		//IL_023b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0242: Expected O, but got Unknown
		//IL_0256: Unknown result type (might be due to invalid IL or missing references)
		//IL_025d: Expected O, but got Unknown
		//IL_0283: Unknown result type (might be due to invalid IL or missing references)
		//IL_028a: Expected O, but got Unknown
		//IL_02fd: Unknown result type (might be due to invalid IL or missing references)
		//IL_0302: Unknown result type (might be due to invalid IL or missing references)
		//IL_0326: Expected O, but got Unknown
		//IL_0341: Unknown result type (might be due to invalid IL or missing references)
		//IL_034b: Expected O, but got Unknown
		//IL_00df: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e6: Expected O, but got Unknown
		//IL_010e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0115: Expected O, but got Unknown
		outputGems = new CSDataArray();
		gemSpacingFix = 0.0;
		OnXform val = new OnXform();
		val.Rotation(rotation, OnUtil.On_zaxis, OnUtil.On_origin);
		double num = profileWidth - (channelWallThicknessInner + channelWallThicknessOuter);
		double num2 = num - channelSpacing * 2.0;
		double bestGemSizeX = -1.23432101234321E+308;
		double num3 = -1.23432101234321E+308;
		if (!gemSizeToggle)
		{
			if (num == 0.0)
			{
				bestGemSizeX = gemDefinition.GemXSize;
				num3 = gemDefinition.GemYSize;
			}
			else
			{
				GemSize gemSize2 = null;
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
						gemSize2 = size;
						bestGemSizeX = num8;
						num3 = num9;
						num4 = num10;
					}
				}
				gemDefinition.ScaleToSize(gemSize2.X, gemSize2.Y);
			}
		}
		else
		{
			gemDefinition.ScaleToSize(gemSize.GemXSize, gemSize.GemYSize);
			num3 = gemSize.GemYSize;
			bestGemSizeX = gemSize.GemXSize;
			num = gemSize.GemXSize + channelSpacing;
		}
		double num11 = gemDefinition.GemXSize / 2.0 - gemDefinition.GemXMax;
		double num12 = gemDefinition.GemYSize / 2.0 - gemDefinition.GemYMax;
		OnXform val4 = new OnXform();
		val4.Translation(num11, num12, 0.0);
		OnXform val5 = new OnXform();
		val5.Translation(0.0, 0.0, 0.0 - Math.Abs(gemDefinition.GemZMax));
		OnXform val6 = new OnXform();
		val6.Rotation(angle, OnUtil.On_xaxis, OnUtil.On_origin);
		double num13 = 0.0;
		if (useMinCuletDistance == ValueListItem.MinCuletGrowUp && gemDefinition.GemZSize > profileHeight - minCuletDistance)
		{
			num13 = gemDefinition.GemZSize - (profileHeight - minCuletDistance);
		}
		Transform xBestScale = Transform.Identity;
		if (useMinCuletDistance == ValueListItem.MinCuletGrowSmaller)
		{
			CSUtilGem.GetGemScaleFromTableWithMaxZ(gemDefinition, gemDefinition.GemXSize, gemDefinition.GemYSize, profileHeight - minCuletDistance, useLargestPossible: true, useIterationOnly: false, out xBestScale);
		}
		OnXform val7 = new OnXform();
		val7.Translation(0.0, 0.0, placement + num13);
		OnXform xTransformLocal = val7 * (IOnXform)(object)val6 * (IOnXform)(object)val5 * (IOnXform)(object)val4 * (IOnXform)(object)val * (IOnXform)(OnXform)Interop.ToOnXform(xBestScale);
		string gemShape;
		if (centerGemShapeOverrideEnabled)
		{
			if (centerGemShapeOverride == null)
			{
				centerGemShapeOverride = new CSGemData(centerGem);
			}
			else
			{
				BoundingBox boundingBox = BoundingBox.Unset;
				centerObject.GetTightBoundingBox(ref boundingBox);
				double num14 = Math.Abs(boundingBox.Max.X - boundingBox.Min.X + offsetX);
				double num15 = Math.Abs(boundingBox.Max.Y - boundingBox.Min.Y + offsetY);
				double num16;
				double num17;
				if (centerGem != null)
				{
					switch (centerGemShapeOverrideMode)
					{
					case ValueListItem.GemClusterCenterShapeOverrideModeDimensionX:
						num16 = (centerGem.GemXSize + offsetX) / centerGemShapeOverride.GemXSize;
						num17 = num16;
						break;
					case ValueListItem.GemClusterCenterShapeOverrideModeDimensionY:
						num17 = (centerGem.GemYSize + offsetY) / centerGemShapeOverride.GemYSize;
						num16 = num17;
						break;
					case ValueListItem.GemClusterCenterShapeOverrideModeDimensionLargest:
						if (centerGem.GemXSize > centerGem.GemYSize)
						{
							num16 = (centerGem.GemXSize + offsetX) / centerGemShapeOverride.GemXSize;
							num17 = num16;
						}
						else
						{
							num17 = (centerGem.GemYSize + offsetY) / centerGemShapeOverride.GemYSize;
							num16 = num17;
						}
						break;
					case ValueListItem.GemClusterCenterShapeOverrideModeDimensionSmallest:
						if (centerGem.GemXSize < centerGem.GemYSize)
						{
							num16 = (centerGem.GemXSize + offsetX) / centerGemShapeOverride.GemXSize;
							num17 = num16;
						}
						else
						{
							num17 = (centerGem.GemYSize + offsetY) / centerGemShapeOverride.GemYSize;
							num16 = num17;
						}
						break;
					case ValueListItem.GemClusterCenterShapeOverrideModeDimensionXandY:
						num16 = (centerGem.GemXSize + offsetX) / centerGemShapeOverride.GemXSize;
						num17 = (centerGem.GemYSize + offsetY) / centerGemShapeOverride.GemYSize;
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
						num16 = num14 / centerGemShapeOverride.GemXSize;
						num17 = num16;
						break;
					case ValueListItem.GemClusterCenterShapeOverrideModeDimensionY:
						num17 = num15 / centerGemShapeOverride.GemYSize;
						num16 = num17;
						break;
					case ValueListItem.GemClusterCenterShapeOverrideModeDimensionLargest:
						if (num14 > num15)
						{
							num16 = num14 / centerGemShapeOverride.GemXSize;
							num17 = num16;
						}
						else
						{
							num17 = num15 / centerGemShapeOverride.GemYSize;
							num16 = num17;
						}
						break;
					case ValueListItem.GemClusterCenterShapeOverrideModeDimensionSmallest:
						if (num14 < num15)
						{
							num16 = num14 / centerGemShapeOverride.GemXSize;
							num17 = num16;
						}
						else
						{
							num17 = num15 / centerGemShapeOverride.GemYSize;
							num16 = num17;
						}
						break;
					case ValueListItem.GemClusterCenterShapeOverrideModeDimensionXandY:
						num16 = num14 / centerGemShapeOverride.GemXSize;
						num17 = num15 / centerGemShapeOverride.GemYSize;
						break;
					default:
						outError = "Invalid Gem Cluster Center Shape Override Mode";
						return false;
					}
				}
				centerGemShapeOverride.Scale(num16, num17);
				Transform xUp;
				if (centerGem != null)
				{
					if (!CSBlockUtil.CSUtilMath.GetUnscaledXform(centerGem.Xform, out xUp))
					{
						xUp = Transform.Identity;
					}
				}
				else
				{
					xUp = Transform.Translation(boundingBox.Center.X, boundingBox.Center.Y, boundingBox.Center.Z) * Transform.Rotation(RhinoMath.ToRadians(rotate), Vector3d.YAxis, Point3d.Origin);
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
		double num18;
		if (num == 0.0)
		{
			num18 = centerGemSpacing + num3 / 2.0 - gemHeightOffset * Math.Sin(angle);
			offsetRailZ = 0.0 + gemHeightOffset * Math.Cos(angle);
		}
		else
		{
			num18 = centerGemSpacing + (channelWallThicknessInner + num / 2.0) * Math.Cos(angle) - gemHeightOffset * Math.Sin(angle);
			offsetRailZ = (channelWallThicknessInner + num / 2.0) * Math.Sin(angle) + gemHeightOffset * Math.Cos(angle);
		}
		CSGemData cSGemData = (doScaleToGemXY ? centerGem : centerGemShapeOverride);
		if (centerGem == null)
		{
			cSGemData = centerGemShapeOverride;
		}
		double optionalEmeraldCornerXPercent = 0.0;
		double optionalEmeraldCornerYPercent = 0.0;
		if (inputCorner1 != Point3d.Unset && inputCorner2 != Point3d.Unset)
		{
			double num19 = cSGemData.GemXSize / 2.0;
			optionalEmeraldCornerXPercent = inputCorner1.X / num19;
			double num20 = cSGemData.GemYSize / 2.0;
			optionalEmeraldCornerYPercent = inputCorner2.Y / num20;
		}
		if (!fixedGemSpacing)
		{
			if (!GetGemLayout(gemShape, cSGemData, gemDefinition, bestGemSizeX, spacing, num18, offsetRailZ, (IOnXform)(object)xTransformLocal, fixPosition, isEven, style, cornerBlend, optionalEmeraldCornerXPercent, optionalEmeraldCornerYPercent, angle, out outputGems, out outError))
			{
				return false;
			}
			gemSpacingFix = 0.0;
		}
		else
		{
			double num21 = num18;
			CSDataArray cSDataArray = null;
			double num22 = -1.0;
			num18 -= 0.1;
			bool flag = false;
			bool flag2 = true;
			if (!GetGemLayout(gemShape, cSGemData, gemDefinition, bestGemSizeX, spacing, num18, offsetRailZ, (IOnXform)(object)xTransformLocal, fixPosition, isEven, style, cornerBlend, optionalEmeraldCornerXPercent, optionalEmeraldCornerYPercent, angle, out outputGems, out outError))
			{
				return false;
			}
			num22 = GetSpacing(outputGems);
			num18 += 0.1;
			while (true)
			{
				if (!GetGemLayout(gemShape, cSGemData, gemDefinition, bestGemSizeX, spacing, num18, offsetRailZ, (IOnXform)(object)xTransformLocal, fixPosition, isEven, style, cornerBlend, optionalEmeraldCornerXPercent, optionalEmeraldCornerYPercent, angle, out outputGems, out outError))
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
				if (spacing2 < num22)
				{
					cSDataArray = outputGems;
					num18 -= 0.01;
					num22 = spacing2;
					break;
				}
				num18 += 0.1;
				num22 = spacing2;
			}
			if (!flag)
			{
				while (true)
				{
					if (!GetGemLayout(gemShape, cSGemData, gemDefinition, bestGemSizeX, spacing, num18, offsetRailZ, (IOnXform)(object)xTransformLocal, fixPosition, isEven, style, cornerBlend, optionalEmeraldCornerXPercent, optionalEmeraldCornerYPercent, angle, out outputGems, out outError))
					{
						return false;
					}
					double spacing3 = GetSpacing(outputGems);
					if (spacing3 > num22)
					{
						break;
					}
					cSDataArray = outputGems;
					num18 -= 0.01;
					num22 = spacing3;
				}
			}
			outputGems = cSDataArray;
			gemSpacingFix = num18 - num21;
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

	private static bool GetGemLayout(string centerGemShape, CSGemData offsetControlShape, CSGemData gemDefinition, double bestGemSizeX, double spacing, double offsetRailX, double offsetRailZ, IOnXform xTransformLocal, double fixPosition, bool isEven, int style, double cornerBlend, double optionalEmeraldCornerXPercent, double optionalEmeraldCornerYPercent, double angle, out CSDataArray outputGems, out string outError)
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
		case "Baguette Tapered":
		case "BaguetteTapered":
			if (!PlaceGemsSquare(centerGemShape, offsetControlShape, gemDefinition, bestGemSizeX, spacing, offsetRailX, offsetRailZ, xTransformLocal, style, cornerBlend, angle, ref outputGems))
			{
				outError = "PlaceGemsSquare failed";
				return false;
			}
			break;
		case "Cushion Square Rotated 45":
		case "Princess Rotated 45":
			if (!PlaceGemsSquareRotated(centerGemShape, offsetControlShape, gemDefinition, bestGemSizeX, spacing, offsetRailX, offsetRailZ, xTransformLocal, style, angle, ref outputGems))
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
			if (!PlaceGemsEmerald(centerGemShape, offsetControlShape, gemDefinition, bestGemSizeX, spacing, offsetRailX, offsetRailZ, xTransformLocal, optionalEmeraldCornerXPercent, optionalEmeraldCornerYPercent, ref outputGems))
			{
				outError = "PlaceGemsEmerald failed";
				return false;
			}
			break;
		case "Heart":
			if (!PlaceGemsHeart(centerGemShape, offsetControlShape, gemDefinition, bestGemSizeX, spacing, offsetRailX, offsetRailZ, xTransformLocal, angle, ref outputGems))
			{
				outError = "PlaceGemsHeart failed";
				return false;
			}
			break;
		case "Marquise":
			if (!PlaceGemsMarquise(centerGemShape, offsetControlShape, gemDefinition, bestGemSizeX, spacing, offsetRailX, offsetRailZ, xTransformLocal, angle, ref outputGems))
			{
				outError = "PlaceGemsMarquise failed";
				return false;
			}
			break;
		case "Pear":
			if (!PlaceGemsPear(centerGemShape, offsetControlShape, gemDefinition, bestGemSizeX, spacing, offsetRailX, offsetRailZ, xTransformLocal, angle, ref outputGems))
			{
				outError = "PlaceGemsPear failed";
				return false;
			}
			break;
		case "Marquise Rotated 90":
			if (!PlaceGemsMarquiseRotated(centerGemShape, offsetControlShape, gemDefinition, bestGemSizeX, spacing, offsetRailX, offsetRailZ, xTransformLocal, angle, ref outputGems))
			{
				outError = "PlaceGemsMarquiseRotated failed";
				return false;
			}
			break;
		case "Calf":
			if (!PlaceGemsCalf(centerGemShape, offsetControlShape, gemDefinition, bestGemSizeX, spacing, offsetRailX, offsetRailZ, xTransformLocal, style, angle, ref outputGems))
			{
				outError = "PlaceGemsCalf failed";
				return false;
			}
			break;
		case "Triangle":
			if (!PlaceGemsTriangle(centerGemShape, offsetControlShape, gemDefinition, bestGemSizeX, spacing, offsetRailX, offsetRailZ, xTransformLocal, style, angle, ref outputGems))
			{
				outError = "PlaceGemsTriangle failed";
				return false;
			}
			break;
		case "Trillion Curved":
		case "Trillion Straight":
			if (!PlaceGemsTrillionCurved(centerGemShape, offsetControlShape, gemDefinition, bestGemSizeX, spacing, offsetRailX, offsetRailZ, xTransformLocal, style, angle, ref outputGems))
			{
				outError = "PlaceGemsTrillion failed";
				return false;
			}
			break;
		case "Half Moon":
			if (!PlaceGemsHalfMoon(centerGemShape, offsetControlShape, gemDefinition, bestGemSizeX, spacing, offsetRailX, offsetRailZ, xTransformLocal, style, angle, ref outputGems))
			{
				outError = "PlaceGemsHalfMoon failed";
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

	private static bool PlaceGemCorner(IOnCurve curveLhs, IOnCurve curveRhs, CSGemData centerGem, CSData gemDefinition, IOnXform xTransformLocal, double angle, CSDataArray outputGems)
	{
		//IL_0024: Unknown result type (might be due to invalid IL or missing references)
		//IL_0029: Unknown result type (might be due to invalid IL or missing references)
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
		Vector3d up = centerGem.Up;
		up.Unitize();
		up = up * Math.Sin(angle) / 2.0;
		Transform transform = Transform.Translation(up);
		CSDataHelper.Transform(data, xform.Convert() * xTransformLocal.Convert() * transform);
		outputGems.Append(data);
		return true;
	}

	private static bool PlaceGemCorners(IList<IOnCurve> curves, CSGemData centerGem, CSData gemDefinition, IOnXform xTransformLocal, CSDataArray outputGems, double angle)
	{
		for (int i = 0; i < curves.Count; i++)
		{
			int num = i;
			int num2 = num + 1;
			if (num2 >= curves.Count)
			{
				num2 -= curves.Count;
			}
			if (!PlaceGemCorner(curves[num], curves[num2], centerGem, gemDefinition, xTransformLocal, angle, outputGems))
			{
				return false;
			}
		}
		return true;
	}

	private static bool PlaceGemsCalf(string centerGemShape, CSGemData centerGem, CSGemData gemDefinition, double gemDefinitionSizeX, double spacing, double offsetRailX, double offsetRailZ, IOnXform xTransformLocal, int style, double angle, ref CSDataArray outputGems)
	{
		OnCurve[] array = GemOffsetCurve.GemOffsetCurvesOld(centerGemShape, centerGem, offsetRailX, doTransformUp: true);
		if (array.Length != 7)
		{
			return false;
		}
		OnCurve[] array2 = array;
		for (int i = 0; i < array2.Length; i++)
		{
			((OnGeometry)array2[i]).Translate((IOn3dVector)(object)(centerGem.Normal.Convert() * offsetRailZ));
		}
		OnCurve val = array[0];
		OnCurve val2 = array[1];
		OnCurve val3 = array[2];
		OnCurve val4 = array[3];
		OnCurve val5 = array[4];
		OnCurve val6 = array[5];
		OnCurve val7 = array[6];
		if (!CSUtilCurves.MergeCurves((IOnCurve[])(object)new IOnCurve[2]
		{
			(IOnCurve)val,
			(IOnCurve)val7
		}, out OnCurve mergedCrv, 0.01))
		{
			return false;
		}
		CSDataArray cSDataArray = new CSDataArray();
		if (style == 0)
		{
			cSDataArray = new CSDataArray();
			if (!PlaceGemCorners((IList<IOnCurve>)(object)new OnCurve[6] { mergedCrv, val2, val3, val4, val5, val6 }, centerGem, gemDefinition, xTransformLocal, cSDataArray, angle))
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
		if (!PlaceGemsCurveSide((IOnCurve)(object)val2, centerGem, gemDefinition, gemDefinitionSizeX, spacing, style, xTransformLocal, outputGems))
		{
			return false;
		}
		if (style == 0)
		{
			outputGems.Append(cSDataArray.GetArrayItem(1));
		}
		if (!PlaceGemsCurveSide((IOnCurve)(object)val3, centerGem, gemDefinition, gemDefinitionSizeX, spacing, style, xTransformLocal, outputGems))
		{
			return false;
		}
		if (style == 0)
		{
			outputGems.Append(cSDataArray.GetArrayItem(2));
		}
		if (!PlaceGemsCurveSide((IOnCurve)(object)val4, centerGem, gemDefinition, gemDefinitionSizeX, spacing, style, xTransformLocal, outputGems))
		{
			return false;
		}
		if (style == 0)
		{
			outputGems.Append(cSDataArray.GetArrayItem(3));
		}
		if (!PlaceGemsCurveSide((IOnCurve)(object)val5, centerGem, gemDefinition, gemDefinitionSizeX, spacing, style, xTransformLocal, outputGems))
		{
			return false;
		}
		if (style == 0)
		{
			outputGems.Append(cSDataArray.GetArrayItem(4));
		}
		if (!PlaceGemsCurveSide((IOnCurve)(object)val6, centerGem, gemDefinition, gemDefinitionSizeX, spacing, style, xTransformLocal, outputGems))
		{
			return false;
		}
		if (style == 0)
		{
			outputGems.Append(cSDataArray.GetArrayItem(5));
		}
		for (int k = 0; k < num; k++)
		{
			outputGems.Append(cSDataArray2.GetArrayItem(k));
		}
		return true;
	}

	private static bool PlaceGemsCurveSide(IOnCurve curve, CSGemData centerGem, CSGemData gemDefinition, double gemDefinitionSizeX, double spacing, int style, IOnXform xTransformLocal, CSDataArray outputGems)
	{
		double num = 0.0;
		if (!curve.GetLength(ref num))
		{
			return false;
		}
		if (style == 1 && centerGem.GemShape == "Princess")
		{
			num -= 3.0 * gemDefinitionSizeX / 7.0;
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

	private static bool PlaceGemsEmerald(string centerGemShape, CSGemData centerGem, CSGemData gemDefinition, double gemDefinitionSizeX, double spacing, double offsetRailX, double offsetRailZ, IOnXform xTransformLocal, double optionalEmeraldCornerXPercent, double optionalEmeraldCornerYPercent, ref CSDataArray outputGems)
	{
		//IL_0039: Unknown result type (might be due to invalid IL or missing references)
		//IL_0045: Expected O, but got Unknown
		//IL_0086: Unknown result type (might be due to invalid IL or missing references)
		//IL_0090: Expected O, but got Unknown
		OnCurve[] array = GemOffsetCurve.GemOffsetCurvesOld(centerGemShape, centerGem, offsetRailX, doTransformUp: true, 0.0, optionalEmeraldCornerXPercent, optionalEmeraldCornerYPercent);
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
		//IL_01ba: Expected O, but got Unknown
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

	private static bool PlaceGemsHalfMoon(string centerGemShape, CSGemData centerGem, CSGemData gemDefinition, double gemDefinitionSizeX, double spacing, double offsetRailX, double offsetRailZ, IOnXform xTransformLocal, int style, double angle, ref CSDataArray outputGems)
	{
		OnCurve[] array = GemOffsetCurve.GemOffsetCurvesOld(centerGemShape, centerGem, offsetRailX, doTransformUp: true);
		if (array.Length != 4)
		{
			return false;
		}
		OnCurve[] array2 = array;
		for (int i = 0; i < array2.Length; i++)
		{
			((OnGeometry)array2[i]).Translate((IOn3dVector)(object)(centerGem.Normal.Convert() * offsetRailZ));
		}
		OnCurve val = array[0];
		OnCurve val2 = array[1];
		OnCurve val3 = array[2];
		OnCurve val4 = array[3];
		IOnCurve[] inputCrvs = (IOnCurve[])(object)new OnCurve[2] { val, val4 };
		CSUtilCurves.MergeCurves(inputCrvs, out OnCurve mergedCrv, 0.01);
		inputCrvs = (IOnCurve[])(object)new OnCurve[2] { val2, val3 };
		CSUtilCurves.MergeCurves(inputCrvs, out OnCurve mergedCrv2, 0.01);
		if (style == 0 && !PlaceGemCorner((IOnCurve)(object)val, (IOnCurve)(object)val2, centerGem, gemDefinition, xTransformLocal, angle, outputGems))
		{
			return false;
		}
		if (!PlaceGemsCurveSide((IOnCurve)(object)mergedCrv, centerGem, gemDefinition, gemDefinitionSizeX, spacing, style, xTransformLocal, outputGems))
		{
			return false;
		}
		if (style == 0 && !PlaceGemCorner((IOnCurve)(object)val3, (IOnCurve)(object)val4, centerGem, gemDefinition, xTransformLocal, angle, outputGems))
		{
			return false;
		}
		if (!PlaceGemsCurveSide((IOnCurve)(object)mergedCrv2, centerGem, gemDefinition, gemDefinitionSizeX, spacing, style, xTransformLocal, outputGems))
		{
			return false;
		}
		return true;
	}

	private static bool PlaceGemsHeart(string centerGemShape, CSGemData centerGem, CSGemData gemDefinition, double gemDefinitionSizeX, double spacing, double offsetRailX, double offsetRailZ, IOnXform xTransformLocal, double angle, ref CSDataArray outputGems)
	{
		//IL_0046: Unknown result type (might be due to invalid IL or missing references)
		//IL_0052: Expected O, but got Unknown
		OnCurve[] array = GemOffsetCurve.GemOffsetCurvesOld(centerGemShape, centerGem, offsetRailX, doTransformUp: true);
		if (array.Length != 6)
		{
			return false;
		}
		OnCurve[] array2 = array;
		for (int i = 0; i < array2.Length; i++)
		{
			((OnGeometry)array2[i]).Translate((IOn3dVector)(object)(new On3dVector((IOn3dVector)(object)centerGem.Normal.Convert()) * offsetRailZ));
		}
		if (!CSUtilCurves.MergeCurves((IOnCurve[])(object)new IOnCurve[3]
		{
			(IOnCurve)array[0],
			(IOnCurve)array[1],
			(IOnCurve)array[2]
		}, out OnCurve mergedCrv, 0.01))
		{
			return false;
		}
		if (!CSUtilCurves.MergeCurves((IOnCurve[])(object)new IOnCurve[3]
		{
			(IOnCurve)array[3],
			(IOnCurve)array[4],
			(IOnCurve)array[5]
		}, out OnCurve mergedCrv2, 0.01))
		{
			return false;
		}
		CSDataArray cSDataArray = new CSDataArray();
		if (!PlaceGemCorners((IList<IOnCurve>)(object)new OnCurve[2] { mergedCrv2, mergedCrv }, centerGem, gemDefinition, xTransformLocal, cSDataArray, angle))
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

	private static bool PlaceGemsMarquise(string centerGemShape, CSGemData centerGem, CSGemData gemDefinition, double gemDefinitionSizeX, double spacing, double offsetRailX, double offsetRailZ, IOnXform xTransformLocal, double angle, ref CSDataArray outputGems)
	{
		//IL_0046: Unknown result type (might be due to invalid IL or missing references)
		//IL_0052: Expected O, but got Unknown
		OnCurve[] array = GemOffsetCurve.GemOffsetCurvesOld(centerGemShape, centerGem, offsetRailX, doTransformUp: true);
		if (array.Length != 2)
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
		CSDataArray cSDataArray = new CSDataArray();
		if (!PlaceGemCorners((IList<IOnCurve>)(object)new IOnCurve[2] { val2, val }, centerGem, gemDefinition, xTransformLocal, cSDataArray, angle))
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

	private static bool PlaceGemsMarquiseRotated(string centerGemShape, CSGemData centerGem, CSGemData gemDefinition, double gemDefinitionSizeX, double spacing, double offsetRailX, double offsetRailZ, IOnXform xTransformLocal, double angle, ref CSDataArray outputGems)
	{
		//IL_0046: Unknown result type (might be due to invalid IL or missing references)
		//IL_0052: Expected O, but got Unknown
		OnCurve[] array = GemOffsetCurve.GemOffsetCurvesOld(centerGemShape, centerGem, offsetRailX, doTransformUp: true);
		if (array.Length != 4)
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
		if (!PlaceGemCorners((IList<IOnCurve>)(object)new OnCurve[2] { mergedCrv, mergedCrv2 }, centerGem, gemDefinition, xTransformLocal, cSDataArray, angle))
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

	private static bool PlaceGemsPear(string centerGemShape, CSGemData centerGem, CSGemData gemDefinition, double gemDefinitionSizeX, double spacing, double offsetRailX, double offsetRailZ, IOnXform xTransformLocal, double angle, ref CSDataArray outputGems)
	{
		//IL_0046: Unknown result type (might be due to invalid IL or missing references)
		//IL_0052: Expected O, but got Unknown
		OnCurve[] array = GemOffsetCurve.GemOffsetCurvesOld(centerGemShape, centerGem, offsetRailX, doTransformUp: true);
		if (array.Length != 4)
		{
			return false;
		}
		OnCurve[] array2 = array;
		for (int i = 0; i < array2.Length; i++)
		{
			((OnGeometry)array2[i]).Translate((IOn3dVector)(object)(new On3dVector((IOn3dVector)(object)centerGem.Normal.Convert()) * offsetRailZ));
		}
		if (!CSUtilCurves.MergeCurves((IOnCurve[])(object)new IOnCurve[2]
		{
			(IOnCurve)array[0],
			(IOnCurve)array[1]
		}, out OnCurve mergedCrv, 0.01))
		{
			return false;
		}
		if (!CSUtilCurves.MergeCurves((IOnCurve[])(object)new IOnCurve[2]
		{
			(IOnCurve)array[2],
			(IOnCurve)array[3]
		}, out OnCurve mergedCrv2, 0.01))
		{
			return false;
		}
		CSDataArray cSDataArray = new CSDataArray();
		if (!PlaceGemCorners((IList<IOnCurve>)(object)new OnCurve[2] { mergedCrv2, mergedCrv }, centerGem, gemDefinition, xTransformLocal, cSDataArray, angle))
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
		//IL_0039: Unknown result type (might be due to invalid IL or missing references)
		//IL_0045: Expected O, but got Unknown
		if (!GemOffsetCurve.GemOffset(centerGemShape, centerGem, offsetRailX, out OnCurve curve, true, out string _, 0.0, 0.0, 0.0))
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

	private static bool PlaceGemsSquare(string centerGemShape, CSGemData centerGem, CSGemData gemDefinition, double gemDefinitionSizeX, double spacing, double offsetRailX, double offsetRailZ, IOnXform xTransformLocal, int style, double cornerBlend, double angle, ref CSDataArray outputGems)
	{
		//IL_003f: Unknown result type (might be due to invalid IL or missing references)
		//IL_004b: Expected O, but got Unknown
		OnCurve[] array = GemOffsetCurve.GemOffsetCurvesOld(centerGemShape, centerGem, offsetRailX, doTransformUp: true, cornerBlend);
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
			}, centerGem, gemDefinition, xTransformLocal, cSDataArray, angle))
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

	private static bool PlaceGemsSquareRotated(string centerGemShape, CSGemData centerGem, CSGemData gemDefinition, double gemDefinitionSizeX, double spacing, double offsetRailX, double offsetRailZ, IOnXform xTransformLocal, int style, double angle, ref CSDataArray outputGems)
	{
		//IL_0042: Unknown result type (might be due to invalid IL or missing references)
		//IL_004e: Expected O, but got Unknown
		OnCurve[] array = GemOffsetCurve.GemOffsetCurvesOld(centerGemShape, centerGem, offsetRailX, doTransformUp: true);
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
			}, centerGem, gemDefinition, xTransformLocal, cSDataArray, angle))
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

	private static bool PlaceGemsTriangle(string centerGemShape, CSGemData centerGem, CSGemData gemDefinition, double gemDefinitionSizeX, double spacing, double offsetRailX, double offsetRailZ, IOnXform xTransformLocal, int style, double angle, ref CSDataArray outputGems)
	{
		OnCurve[] array = GemOffsetCurve.GemOffsetCurvesOld(centerGemShape, centerGem, offsetRailX, doTransformUp: true);
		if (array.Length != 4)
		{
			return false;
		}
		OnCurve[] array2 = array;
		for (int i = 0; i < array2.Length; i++)
		{
			((OnGeometry)array2[i]).Translate((IOn3dVector)(object)(centerGem.Normal.Convert() * offsetRailZ));
		}
		IOnCurve val = (IOnCurve)(object)array[0];
		IOnCurve val2 = (IOnCurve)(object)array[1];
		IOnCurve val3 = (IOnCurve)(object)array[2];
		IOnCurve val4 = (IOnCurve)(object)array[3];
		if (!CSUtilCurves.MergeCurves((IOnCurve[])(object)new IOnCurve[2] { val, val4 }, out OnCurve mergedCrv, 0.01))
		{
			return false;
		}
		CSDataArray cSDataArray = null;
		if (style == 0)
		{
			cSDataArray = new CSDataArray();
			if (!PlaceGemCorners((IList<IOnCurve>)(object)new IOnCurve[3]
			{
				(IOnCurve)mergedCrv,
				val2,
				val3
			}, centerGem, gemDefinition, xTransformLocal, cSDataArray, angle))
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
		for (int k = 0; k < num; k++)
		{
			outputGems.Append(cSDataArray2.GetArrayItem(k));
		}
		return true;
	}

	private static bool PlaceGemsTrillionCurved(string centerGemShape, CSGemData centerGem, CSGemData gemDefinition, double gemDefinitionSizeX, double spacing, double offsetRailX, double offsetRailZ, IOnXform xTransformLocal, int style, double angle, ref CSDataArray outputGems)
	{
		OnCurve[] array = GemOffsetCurve.GemOffsetCurvesOld(centerGemShape, centerGem, offsetRailX, doTransformUp: true);
		if (array.Length != 12)
		{
			return false;
		}
		OnCurve[] array2 = array;
		for (int i = 0; i < array2.Length; i++)
		{
			((OnGeometry)array2[i]).Translate((IOn3dVector)(object)(centerGem.Normal.Convert() * offsetRailZ));
		}
		IOnCurve val = (IOnCurve)(object)array[0];
		IOnCurve val2 = (IOnCurve)(object)array[1];
		IOnCurve val3 = (IOnCurve)(object)array[2];
		IOnCurve val4 = (IOnCurve)(object)array[3];
		IOnCurve val5 = (IOnCurve)(object)array[4];
		IOnCurve val6 = (IOnCurve)(object)array[5];
		IOnCurve val7 = (IOnCurve)(object)array[6];
		IOnCurve val8 = (IOnCurve)(object)array[7];
		IOnCurve val9 = (IOnCurve)(object)array[8];
		IOnCurve val10 = (IOnCurve)(object)array[9];
		IOnCurve val11 = (IOnCurve)(object)array[10];
		IOnCurve val12 = (IOnCurve)(object)array[11];
		IOnCurve[] inputCrvs = (IOnCurve[])(object)new IOnCurve[4] { val, val12, val2, val11 };
		IOnCurve[] inputCrvs2 = (IOnCurve[])(object)new IOnCurve[4] { val3, val4, val5, val6 };
		IOnCurve[] inputCrvs3 = (IOnCurve[])(object)new IOnCurve[4] { val7, val8, val9, val10 };
		if (!CSUtilCurves.MergeCurves(inputCrvs, out OnCurve mergedCrv, 0.01))
		{
			return false;
		}
		if (!CSUtilCurves.MergeCurves(inputCrvs2, out OnCurve mergedCrv2, 0.01))
		{
			return false;
		}
		if (!CSUtilCurves.MergeCurves(inputCrvs3, out OnCurve mergedCrv3, 0.01))
		{
			return false;
		}
		CSDataArray cSDataArray = null;
		if (style == 0)
		{
			cSDataArray = new CSDataArray();
			if (!PlaceGemCorners((IList<IOnCurve>)(object)new OnCurve[3] { mergedCrv, mergedCrv2, mergedCrv3 }, centerGem, gemDefinition, xTransformLocal, cSDataArray, angle))
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
		if (!PlaceGemsCurveSide((IOnCurve)(object)mergedCrv2, centerGem, gemDefinition, gemDefinitionSizeX, spacing, style, xTransformLocal, outputGems))
		{
			return false;
		}
		if (style == 0)
		{
			outputGems.Append(cSDataArray.GetArrayItem(1));
		}
		if (!PlaceGemsCurveSide((IOnCurve)(object)mergedCrv3, centerGem, gemDefinition, gemDefinitionSizeX, spacing, style, xTransformLocal, outputGems))
		{
			return false;
		}
		if (style == 0)
		{
			outputGems.Append(cSDataArray.GetArrayItem(2));
		}
		for (int k = 0; k < num; k++)
		{
			outputGems.Append(cSDataArray2.GetArrayItem(k));
		}
		return true;
	}
}
