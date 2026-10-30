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
		outputGems = new CSDataArray();
		gemSpacingFix = 0.0;
		OnXform onXform = new OnXform();
		onXform.Rotation(rotation, OnUtil.On_zaxis, OnUtil.On_origin);
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
					double z_scale_factor = (num5 + num6) / 2.0;
					OnXform onXform2 = new OnXform();
					onXform2.Scale(num5, num6, z_scale_factor);
					OnNurbsCurve onNurbsCurve = gemDefinition.GetNurbsCurve().Convert();
					onNurbsCurve.Transform(onXform * onXform2);
					OnBoundingBox tight_bbox = new OnBoundingBox();
					if (!onNurbsCurve.GetTightBoundingBox(ref tight_bbox))
					{
						outError = "gem Curve bounding Box failed";
						return false;
					}
					double num7 = Math.Abs(tight_bbox.Max().x) + Math.Abs(tight_bbox.Min().x);
					double num8 = Math.Abs(tight_bbox.Max().y) + Math.Abs(tight_bbox.Min().y);
					double num9 = Math.Abs(num2 - num8);
					if (!RhinoMath.IsValidDouble(num4) || num9 < num4)
					{
						gemSize2 = size;
						bestGemSizeX = num7;
						num3 = num8;
						num4 = num9;
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
		double dx = gemDefinition.GemXSize / 2.0 - gemDefinition.GemXMax;
		double dy = gemDefinition.GemYSize / 2.0 - gemDefinition.GemYMax;
		OnXform onXform3 = new OnXform();
		onXform3.Translation(dx, dy, 0.0);
		OnXform onXform4 = new OnXform();
		onXform4.Translation(0.0, 0.0, 0.0 - Math.Abs(gemDefinition.GemZMax));
		OnXform onXform5 = new OnXform();
		onXform5.Rotation(angle, OnUtil.On_xaxis, OnUtil.On_origin);
		double num10 = 0.0;
		if (useMinCuletDistance == ValueListItem.MinCuletGrowUp && gemDefinition.GemZSize > profileHeight - minCuletDistance)
		{
			num10 = gemDefinition.GemZSize - (profileHeight - minCuletDistance);
		}
		Transform xBestScale = Transform.Identity;
		if (useMinCuletDistance == ValueListItem.MinCuletGrowSmaller)
		{
			CSUtilGem.GetGemScaleFromTableWithMaxZ(gemDefinition, gemDefinition.GemXSize, gemDefinition.GemYSize, profileHeight - minCuletDistance, useLargestPossible: true, useIterationOnly: false, out xBestScale);
		}
		OnXform onXform6 = new OnXform();
		onXform6.Translation(0.0, 0.0, placement + num10);
		OnXform xTransformLocal = onXform6 * onXform5 * onXform4 * onXform3 * onXform * (OnXform)Interop.ToOnXform(xBestScale);
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
				double num11 = Math.Abs(boundingBox.Max.X - boundingBox.Min.X + offsetX);
				double num12 = Math.Abs(boundingBox.Max.Y - boundingBox.Min.Y + offsetY);
				double num13;
				double num14;
				if (centerGem != null)
				{
					switch (centerGemShapeOverrideMode)
					{
					case ValueListItem.GemClusterCenterShapeOverrideModeDimensionX:
						num13 = (centerGem.GemXSize + offsetX) / centerGemShapeOverride.GemXSize;
						num14 = num13;
						break;
					case ValueListItem.GemClusterCenterShapeOverrideModeDimensionY:
						num14 = (centerGem.GemYSize + offsetY) / centerGemShapeOverride.GemYSize;
						num13 = num14;
						break;
					case ValueListItem.GemClusterCenterShapeOverrideModeDimensionLargest:
						if (centerGem.GemXSize > centerGem.GemYSize)
						{
							num13 = (centerGem.GemXSize + offsetX) / centerGemShapeOverride.GemXSize;
							num14 = num13;
						}
						else
						{
							num14 = (centerGem.GemYSize + offsetY) / centerGemShapeOverride.GemYSize;
							num13 = num14;
						}
						break;
					case ValueListItem.GemClusterCenterShapeOverrideModeDimensionSmallest:
						if (centerGem.GemXSize < centerGem.GemYSize)
						{
							num13 = (centerGem.GemXSize + offsetX) / centerGemShapeOverride.GemXSize;
							num14 = num13;
						}
						else
						{
							num14 = (centerGem.GemYSize + offsetY) / centerGemShapeOverride.GemYSize;
							num13 = num14;
						}
						break;
					case ValueListItem.GemClusterCenterShapeOverrideModeDimensionXandY:
						num13 = (centerGem.GemXSize + offsetX) / centerGemShapeOverride.GemXSize;
						num14 = (centerGem.GemYSize + offsetY) / centerGemShapeOverride.GemYSize;
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
						num13 = num11 / centerGemShapeOverride.GemXSize;
						num14 = num13;
						break;
					case ValueListItem.GemClusterCenterShapeOverrideModeDimensionY:
						num14 = num12 / centerGemShapeOverride.GemYSize;
						num13 = num14;
						break;
					case ValueListItem.GemClusterCenterShapeOverrideModeDimensionLargest:
						if (num11 > num12)
						{
							num13 = num11 / centerGemShapeOverride.GemXSize;
							num14 = num13;
						}
						else
						{
							num14 = num12 / centerGemShapeOverride.GemYSize;
							num13 = num14;
						}
						break;
					case ValueListItem.GemClusterCenterShapeOverrideModeDimensionSmallest:
						if (num11 < num12)
						{
							num13 = num11 / centerGemShapeOverride.GemXSize;
							num14 = num13;
						}
						else
						{
							num14 = num12 / centerGemShapeOverride.GemYSize;
							num13 = num14;
						}
						break;
					case ValueListItem.GemClusterCenterShapeOverrideModeDimensionXandY:
						num13 = num11 / centerGemShapeOverride.GemXSize;
						num14 = num12 / centerGemShapeOverride.GemYSize;
						break;
					default:
						outError = "Invalid Gem Cluster Center Shape Override Mode";
						return false;
					}
				}
				centerGemShapeOverride.Scale(num13, num14);
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
		double num15;
		if (num == 0.0)
		{
			num15 = centerGemSpacing + num3 / 2.0 - gemHeightOffset * Math.Sin(angle);
			offsetRailZ = 0.0 + gemHeightOffset * Math.Cos(angle);
		}
		else
		{
			num15 = centerGemSpacing + (channelWallThicknessInner + num / 2.0) * Math.Cos(angle) - gemHeightOffset * Math.Sin(angle);
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
			double num16 = cSGemData.GemXSize / 2.0;
			optionalEmeraldCornerXPercent = inputCorner1.X / num16;
			double num17 = cSGemData.GemYSize / 2.0;
			optionalEmeraldCornerYPercent = inputCorner2.Y / num17;
		}
		if (!fixedGemSpacing)
		{
			if (!GetGemLayout(gemShape, cSGemData, gemDefinition, bestGemSizeX, spacing, num15, offsetRailZ, xTransformLocal, fixPosition, isEven, style, cornerBlend, optionalEmeraldCornerXPercent, optionalEmeraldCornerYPercent, angle, out outputGems, out outError))
			{
				return false;
			}
			gemSpacingFix = 0.0;
		}
		else
		{
			double num18 = num15;
			CSDataArray cSDataArray = null;
			double num19 = -1.0;
			num15 -= 0.1;
			bool flag = false;
			bool flag2 = true;
			if (!GetGemLayout(gemShape, cSGemData, gemDefinition, bestGemSizeX, spacing, num15, offsetRailZ, xTransformLocal, fixPosition, isEven, style, cornerBlend, optionalEmeraldCornerXPercent, optionalEmeraldCornerYPercent, angle, out outputGems, out outError))
			{
				return false;
			}
			num19 = GetSpacing(outputGems);
			num15 += 0.1;
			while (true)
			{
				if (!GetGemLayout(gemShape, cSGemData, gemDefinition, bestGemSizeX, spacing, num15, offsetRailZ, xTransformLocal, fixPosition, isEven, style, cornerBlend, optionalEmeraldCornerXPercent, optionalEmeraldCornerYPercent, angle, out outputGems, out outError))
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
				if (spacing2 < num19)
				{
					cSDataArray = outputGems;
					num15 -= 0.01;
					num19 = spacing2;
					break;
				}
				num15 += 0.1;
				num19 = spacing2;
			}
			if (!flag)
			{
				while (true)
				{
					if (!GetGemLayout(gemShape, cSGemData, gemDefinition, bestGemSizeX, spacing, num15, offsetRailZ, xTransformLocal, fixPosition, isEven, style, cornerBlend, optionalEmeraldCornerXPercent, optionalEmeraldCornerYPercent, angle, out outputGems, out outError))
					{
						return false;
					}
					double spacing3 = GetSpacing(outputGems);
					if (spacing3 > num19)
					{
						break;
					}
					cSDataArray = outputGems;
					num15 -= 0.01;
					num19 = spacing3;
				}
			}
			outputGems = cSDataArray;
			gemSpacingFix = num15 - num18;
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
		CSGemData obj = gemarray.GetArrayItem(0) as CSGemData;
		CSGemData data = gemarray.GetArrayItem(1) as CSGemData;
		On3dPoint value = new On3dPoint(OnUtil.On_origin);
		CSDataHelper.GetPoint(obj, ref value);
		On3dPoint value2 = new On3dPoint(OnUtil.On_origin);
		CSDataHelper.GetPoint(data, ref value2);
		double num = obj.GemXSize / 2.0;
		double num2 = obj.GemYSize / 2.0;
		return (value - value2).Length() - num - num2;
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
		On3dPoint on3dPoint = curveLhs.PointAtEnd();
		On3dPoint on3dPoint2 = curveRhs.PointAtStart();
		if (on3dPoint.DistanceTo(on3dPoint2) > 0.001)
		{
			return false;
		}
		OnLineCurve onLineCurve = new OnLineCurve(on3dPoint, on3dPoint2);
		On3dPoint destination = onLineCurve.PointAt(onLineCurve.Domain().Mid());
		On3dVector on3dVector = curveLhs.TangentAt(curveLhs.Domain().Max());
		On3dVector on3dVector2 = curveRhs.TangentAt(curveRhs.Domain().Min());
		On3dVector destination2 = on3dVector + on3dVector2;
		OnXform xform = CSBlockUtil.CSUtilMath.Orient3Point(OnUtil.On_origin, OnUtil.On_xaxis, OnUtil.On_zaxis, destination, destination2, centerGem.Up.Convert());
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
			array2[i].Translate(centerGem.Normal.Convert() * offsetRailZ);
		}
		OnCurve onCurve = array[0];
		OnCurve onCurve2 = array[1];
		OnCurve onCurve3 = array[2];
		OnCurve onCurve4 = array[3];
		OnCurve onCurve5 = array[4];
		OnCurve onCurve6 = array[5];
		OnCurve onCurve7 = array[6];
		if (!CSUtilCurves.MergeCurves(new IOnCurve[2] { onCurve, onCurve7 }, out OnCurve mergedCrv, 0.01))
		{
			return false;
		}
		CSDataArray cSDataArray = new CSDataArray();
		if (style == 0)
		{
			cSDataArray = new CSDataArray();
			if (!PlaceGemCorners(new OnCurve[6] { mergedCrv, onCurve2, onCurve3, onCurve4, onCurve5, onCurve6 }, centerGem, gemDefinition, xTransformLocal, cSDataArray, angle))
			{
				return false;
			}
		}
		CSDataArray cSDataArray2 = new CSDataArray();
		if (!PlaceGemsCurveSide(mergedCrv, centerGem, gemDefinition, gemDefinitionSizeX, spacing, style, xTransformLocal, cSDataArray2))
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
		if (!PlaceGemsCurveSide(onCurve2, centerGem, gemDefinition, gemDefinitionSizeX, spacing, style, xTransformLocal, outputGems))
		{
			return false;
		}
		if (style == 0)
		{
			outputGems.Append(cSDataArray.GetArrayItem(1));
		}
		if (!PlaceGemsCurveSide(onCurve3, centerGem, gemDefinition, gemDefinitionSizeX, spacing, style, xTransformLocal, outputGems))
		{
			return false;
		}
		if (style == 0)
		{
			outputGems.Append(cSDataArray.GetArrayItem(2));
		}
		if (!PlaceGemsCurveSide(onCurve4, centerGem, gemDefinition, gemDefinitionSizeX, spacing, style, xTransformLocal, outputGems))
		{
			return false;
		}
		if (style == 0)
		{
			outputGems.Append(cSDataArray.GetArrayItem(3));
		}
		if (!PlaceGemsCurveSide(onCurve5, centerGem, gemDefinition, gemDefinitionSizeX, spacing, style, xTransformLocal, outputGems))
		{
			return false;
		}
		if (style == 0)
		{
			outputGems.Append(cSDataArray.GetArrayItem(4));
		}
		if (!PlaceGemsCurveSide(onCurve6, centerGem, gemDefinition, gemDefinitionSizeX, spacing, style, xTransformLocal, outputGems))
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
		double length = 0.0;
		if (!curve.GetLength(ref length))
		{
			return false;
		}
		if (style == 1 && centerGem.GemShape == "Princess")
		{
			length -= 3.0 * gemDefinitionSizeX / 7.0;
		}
		int num = (int)(length / (gemDefinitionSizeX + spacing));
		if (length < (double)num * gemDefinitionSizeX + (double)Math.Max(num - 1, 0) * spacing)
		{
			num--;
		}
		double[] array = new double[num];
		double[] t = new double[num];
		for (int i = 0; i < num; i++)
		{
			array[i] = GetUnnormalizedParam(i, num, style);
		}
		curve.GetNormalizedArcLengthPoints(array, ref t);
		for (int j = ((style == 0) ? 1 : 0); j < num; j++)
		{
			OnXform xform = CSBlockUtil.CSUtilMath.Orient3Point(OnUtil.On_origin, OnUtil.On_xaxis, gemDefinition.Up.Convert(), curve.PointAt(t[j]), curve.TangentAt(t[j]), centerGem.Up.Convert());
			CSData data = gemDefinition.Duplicate();
			CSDataHelper.Transform(data, xform.Convert() * xTransformLocal.Convert());
			outputGems.Append(data);
		}
		return true;
	}

	private static bool PlaceGemsEmerald(string centerGemShape, CSGemData centerGem, CSGemData gemDefinition, double gemDefinitionSizeX, double spacing, double offsetRailX, double offsetRailZ, IOnXform xTransformLocal, double optionalEmeraldCornerXPercent, double optionalEmeraldCornerYPercent, ref CSDataArray outputGems)
	{
		OnCurve[] array = GemOffsetCurve.GemOffsetCurvesOld(centerGemShape, centerGem, offsetRailX, doTransformUp: true, 0.0, optionalEmeraldCornerXPercent, optionalEmeraldCornerYPercent);
		if (array.Length != 9)
		{
			return false;
		}
		OnCurve[] array2 = array;
		for (int i = 0; i < array2.Length; i++)
		{
			array2[i].Translate(new On3dVector(centerGem.Normal.Convert()) * offsetRailZ);
		}
		OnCurve onCurve = array[0];
		OnCurve onCurve2 = array[1];
		OnCurve onCurve3 = array[2];
		OnCurve onCurve4 = onCurve3.Duplicate();
		onCurve4.Trim(new OnInterval(onCurve4.Domain().Min(), onCurve3.Domain().Mid()));
		double cosAng = CosAng(onCurve.PointAtStart() - onCurve.PointAtEnd(), onCurve2.PointAtEnd() - onCurve2.PointAtStart());
		double cosAng2 = CosAng(onCurve2.PointAtStart() - onCurve2.PointAtEnd(), onCurve4.PointAtEnd() - onCurve4.PointAtStart());
		double length = 0.0;
		onCurve.GetLength(ref length);
		double length2 = 0.0;
		onCurve2.GetLength(ref length2);
		double length3 = 0.0;
		onCurve4.GetLength(ref length3);
		double num = 0.0;
		bool flag = true;
		int num2 = -1;
		double num3;
		double num4;
		int num5;
		double side;
		int num6;
		double side2;
		int num7;
		double num8;
		do
		{
			num3 = gemDefinitionSizeX + spacing + num;
			num4 = 0.0;
			num5 = (int)((length - num4) / num3);
			double a = length - (num4 + num3 * (double)num5);
			side = GetSide(cosAng, a, num3);
			num6 = (int)((length2 - side) / num3);
			double a2 = length2 - (side + num3 * (double)num6);
			side2 = GetSide(cosAng2, a2, num3);
			num7 = (int)((length3 - side2) / num3);
			num8 = length3 - (side2 + num3 * (double)num7);
			if (flag)
			{
				num2 = ((num8 > num3 / 2.0) ? 1 : 0);
				if (num2 == 1)
				{
					num8 -= num3 / 2.0;
				}
				int num9 = num5 + num6 + num7 + 3 - 1;
				num += num8 / (double)num9 * 0.9;
				flag = false;
				continue;
			}
			double num10 = num8 / num3;
			if (num10 < 0.25)
			{
				int num11 = num5 + num6 + num7 + 3 - 1;
				num += num8 / (double)num11 * 0.9;
			}
			else if (num10 < 0.75)
			{
				num8 -= num3 / 2.0;
				int num12 = num5 + num6 + num7 + 3 - 1;
				num += num8 / (double)num12 * 0.9;
			}
			else
			{
				num8 -= num3;
				num7++;
				int num13 = num5 + num6 + num7 + 3 - 1;
				num += num8 / (double)num13 * 0.9;
			}
		}
		while (num8 > 0.01);
		double num14 = length3 + num3 * (1.0 - (double)num2 / 2.0);
		int num15 = num7 - 1 + num2;
		for (int j = 0; j <= num5; j++)
		{
			PlaceGemsEmeraldSide(array, 0, gemDefinitionSizeX, num4 + (double)j * num3, centerGem, gemDefinition, xTransformLocal, ref outputGems);
		}
		for (int k = 0; k <= num6; k++)
		{
			PlaceGemsEmeraldSide(array, 1, gemDefinitionSizeX, side + (double)k * num3, centerGem, gemDefinition, xTransformLocal, ref outputGems);
		}
		for (int l = 0; l <= num7; l++)
		{
			PlaceGemsEmeraldSide(array, 2, gemDefinitionSizeX, side2 + (double)l * num3, centerGem, gemDefinition, xTransformLocal, ref outputGems);
		}
		for (int m = 0; m <= num15; m++)
		{
			PlaceGemsEmeraldSide(array, 2, gemDefinitionSizeX, num14 + (double)m * num3, centerGem, gemDefinition, xTransformLocal, ref outputGems);
		}
		for (int num16 = num6; num16 >= 0; num16--)
		{
			PlaceGemsEmeraldSide(array, 3, gemDefinitionSizeX, length2 - (side + (double)num16 * num3), centerGem, gemDefinition, xTransformLocal, ref outputGems);
		}
		for (int num17 = num5; num17 >= 0; num17--)
		{
			PlaceGemsEmeraldSide(array, 4, gemDefinitionSizeX, length - (num4 + (double)num17 * num3), centerGem, gemDefinition, xTransformLocal, ref outputGems);
		}
		for (int n = 1; n <= num5; n++)
		{
			PlaceGemsEmeraldSide(array, 4, gemDefinitionSizeX, length + (num4 + (double)n * num3), centerGem, gemDefinition, xTransformLocal, ref outputGems);
		}
		for (int num18 = 0; num18 <= num6; num18++)
		{
			PlaceGemsEmeraldSide(array, 5, gemDefinitionSizeX, side + (double)num18 * num3, centerGem, gemDefinition, xTransformLocal, ref outputGems);
		}
		for (int num19 = num15; num19 >= 0; num19--)
		{
			PlaceGemsEmeraldSide(array, 6, gemDefinitionSizeX, length3 * 2.0 - (num14 + (double)num19 * num3), centerGem, gemDefinition, xTransformLocal, ref outputGems);
		}
		for (int num20 = num7; num20 >= 0; num20--)
		{
			PlaceGemsEmeraldSide(array, 6, gemDefinitionSizeX, length3 * 2.0 - (side2 + (double)num20 * num3), centerGem, gemDefinition, xTransformLocal, ref outputGems);
		}
		for (int num21 = num6; num21 >= 0; num21--)
		{
			PlaceGemsEmeraldSide(array, 7, gemDefinitionSizeX, length2 - (side + (double)num21 * num3), centerGem, gemDefinition, xTransformLocal, ref outputGems);
		}
		for (int num22 = num5; num22 >= 1; num22--)
		{
			PlaceGemsEmeraldSide(array, 8, gemDefinitionSizeX, length - (num4 + (double)num22 * num3), centerGem, gemDefinition, xTransformLocal, ref outputGems);
		}
		return true;
	}

	private static void PlaceGemsEmeraldSide(IList<OnCurve> curves, int crvIndex, double sizeX, double distance, CSGemData centerGem, CSGemData gemDefinition, IOnXform xOrientLocal, ref CSDataArray outputGems)
	{
		On3dPoint on3dPoint = curves[crvIndex].PointAtStart();
		On3dVector on3dVector = curves[crvIndex].PointAtEnd() - on3dPoint;
		on3dVector.Unitize();
		On3dVector destination = new On3dVector(on3dVector);
		double num = sizeX / 2.0;
		if (distance < num)
		{
			int num2 = crvIndex - 1;
			if (num2 == -1)
			{
				num2 = curves.Count - 1;
			}
			On3dVector on3dVector2 = curves[num2].PointAtEnd() - curves[num2].PointAtStart();
			on3dVector2.Unitize();
			double num3 = distance;
			if (num3 < 0.0)
			{
				num3 = 0.0;
			}
			double num4 = num3 / num;
			double num5 = 1.0 - num4;
			destination = on3dVector * num4 + on3dVector2 * num5;
		}
		else
		{
			double length = 0.0;
			curves[crvIndex].GetLength(ref length);
			if (length - distance < num)
			{
				int index = (crvIndex + 1) % curves.Count;
				On3dVector on3dVector3 = curves[index].PointAtEnd() - curves[index].PointAtStart();
				on3dVector3.Unitize();
				double num6 = length - distance;
				if (num6 < 0.0)
				{
					num6 = 0.0;
				}
				double num7 = num6 / num;
				double num8 = 1.0 - num7;
				destination = on3dVector * num7 + on3dVector3 * num8;
			}
		}
		on3dPoint += (IOn3dVector)(on3dVector * distance);
		OnXform xform = CSBlockUtil.CSUtilMath.Orient3Point(OnUtil.On_origin, new On3dVector(1.0, 0.0, 0.0), gemDefinition.Normal.Convert(), on3dPoint, destination, centerGem.Normal.Convert());
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
			array2[i].Translate(centerGem.Normal.Convert() * offsetRailZ);
		}
		OnCurve onCurve = array[0];
		OnCurve onCurve2 = array[1];
		OnCurve onCurve3 = array[2];
		OnCurve onCurve4 = array[3];
		IOnCurve[] inputCrvs = new OnCurve[2] { onCurve, onCurve4 };
		CSUtilCurves.MergeCurves(inputCrvs, out OnCurve mergedCrv, 0.01);
		inputCrvs = new OnCurve[2] { onCurve2, onCurve3 };
		CSUtilCurves.MergeCurves(inputCrvs, out OnCurve mergedCrv2, 0.01);
		if (style == 0 && !PlaceGemCorner(onCurve, onCurve2, centerGem, gemDefinition, xTransformLocal, angle, outputGems))
		{
			return false;
		}
		if (!PlaceGemsCurveSide(mergedCrv, centerGem, gemDefinition, gemDefinitionSizeX, spacing, style, xTransformLocal, outputGems))
		{
			return false;
		}
		if (style == 0 && !PlaceGemCorner(onCurve3, onCurve4, centerGem, gemDefinition, xTransformLocal, angle, outputGems))
		{
			return false;
		}
		if (!PlaceGemsCurveSide(mergedCrv2, centerGem, gemDefinition, gemDefinitionSizeX, spacing, style, xTransformLocal, outputGems))
		{
			return false;
		}
		return true;
	}

	private static bool PlaceGemsHeart(string centerGemShape, CSGemData centerGem, CSGemData gemDefinition, double gemDefinitionSizeX, double spacing, double offsetRailX, double offsetRailZ, IOnXform xTransformLocal, double angle, ref CSDataArray outputGems)
	{
		OnCurve[] array = GemOffsetCurve.GemOffsetCurvesOld(centerGemShape, centerGem, offsetRailX, doTransformUp: true);
		if (array.Length != 6)
		{
			return false;
		}
		OnCurve[] array2 = array;
		for (int i = 0; i < array2.Length; i++)
		{
			array2[i].Translate(new On3dVector(centerGem.Normal.Convert()) * offsetRailZ);
		}
		if (!CSUtilCurves.MergeCurves(new IOnCurve[3]
		{
			array[0],
			array[1],
			array[2]
		}, out OnCurve mergedCrv, 0.01))
		{
			return false;
		}
		if (!CSUtilCurves.MergeCurves(new IOnCurve[3]
		{
			array[3],
			array[4],
			array[5]
		}, out OnCurve mergedCrv2, 0.01))
		{
			return false;
		}
		CSDataArray cSDataArray = new CSDataArray();
		if (!PlaceGemCorners(new OnCurve[2] { mergedCrv2, mergedCrv }, centerGem, gemDefinition, xTransformLocal, cSDataArray, angle))
		{
			return false;
		}
		outputGems.Append(cSDataArray.GetArrayItem(0));
		if (!PlaceGemsCurveSide(mergedCrv, centerGem, gemDefinition, gemDefinitionSizeX, spacing, 0, xTransformLocal, outputGems))
		{
			return false;
		}
		outputGems.Append(cSDataArray.GetArrayItem(1));
		if (!PlaceGemsCurveSide(mergedCrv2, centerGem, gemDefinition, gemDefinitionSizeX, spacing, 0, xTransformLocal, outputGems))
		{
			return false;
		}
		return true;
	}

	private static bool PlaceGemsMarquise(string centerGemShape, CSGemData centerGem, CSGemData gemDefinition, double gemDefinitionSizeX, double spacing, double offsetRailX, double offsetRailZ, IOnXform xTransformLocal, double angle, ref CSDataArray outputGems)
	{
		OnCurve[] array = GemOffsetCurve.GemOffsetCurvesOld(centerGemShape, centerGem, offsetRailX, doTransformUp: true);
		if (array.Length != 2)
		{
			return false;
		}
		OnCurve[] array2 = array;
		for (int i = 0; i < array2.Length; i++)
		{
			array2[i].Translate(new On3dVector(centerGem.Normal.Convert()) * offsetRailZ);
		}
		IOnCurve onCurve = array[0];
		IOnCurve onCurve2 = array[1];
		CSDataArray cSDataArray = new CSDataArray();
		if (!PlaceGemCorners(new IOnCurve[2] { onCurve2, onCurve }, centerGem, gemDefinition, xTransformLocal, cSDataArray, angle))
		{
			return false;
		}
		outputGems.Append(cSDataArray.GetArrayItem(0));
		if (!PlaceGemsCurveSide(onCurve, centerGem, gemDefinition, gemDefinitionSizeX, spacing, 0, xTransformLocal, outputGems))
		{
			return false;
		}
		outputGems.Append(cSDataArray.GetArrayItem(1));
		if (!PlaceGemsCurveSide(onCurve2, centerGem, gemDefinition, gemDefinitionSizeX, spacing, 0, xTransformLocal, outputGems))
		{
			return false;
		}
		return true;
	}

	private static bool PlaceGemsMarquiseRotated(string centerGemShape, CSGemData centerGem, CSGemData gemDefinition, double gemDefinitionSizeX, double spacing, double offsetRailX, double offsetRailZ, IOnXform xTransformLocal, double angle, ref CSDataArray outputGems)
	{
		OnCurve[] array = GemOffsetCurve.GemOffsetCurvesOld(centerGemShape, centerGem, offsetRailX, doTransformUp: true);
		if (array.Length != 4)
		{
			return false;
		}
		OnCurve[] array2 = array;
		for (int i = 0; i < array2.Length; i++)
		{
			array2[i].Translate(new On3dVector(centerGem.Normal.Convert()) * offsetRailZ);
		}
		IOnCurve onCurve = array[0];
		IOnCurve onCurve2 = array[1];
		IOnCurve onCurve3 = array[2];
		IOnCurve onCurve4 = array[3];
		IOnCurve[] inputCrvs = new IOnCurve[2] { onCurve4, onCurve };
		IOnCurve[] inputCrvs2 = new IOnCurve[2] { onCurve2, onCurve3 };
		if (!CSUtilCurves.MergeCurves(inputCrvs, out OnCurve mergedCrv, 0.01))
		{
			return false;
		}
		if (!CSUtilCurves.MergeCurves(inputCrvs2, out OnCurve mergedCrv2, 0.01))
		{
			return false;
		}
		CSDataArray cSDataArray = new CSDataArray();
		if (!PlaceGemCorners(new OnCurve[2] { mergedCrv, mergedCrv2 }, centerGem, gemDefinition, xTransformLocal, cSDataArray, angle))
		{
			return false;
		}
		CSDataArray cSDataArray2 = new CSDataArray();
		if (!PlaceGemsCurveSide(mergedCrv, centerGem, gemDefinition, gemDefinitionSizeX, spacing, 0, xTransformLocal, cSDataArray2))
		{
			return false;
		}
		int num = cSDataArray2.GetArrayCount() / 2;
		for (int j = num; j < cSDataArray2.GetArrayCount(); j++)
		{
			outputGems.Append(cSDataArray2.GetArrayItem(j));
		}
		outputGems.Append(cSDataArray.GetArrayItem(0));
		if (!PlaceGemsCurveSide(mergedCrv2, centerGem, gemDefinition, gemDefinitionSizeX, spacing, 0, xTransformLocal, outputGems))
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
		OnCurve[] array = GemOffsetCurve.GemOffsetCurvesOld(centerGemShape, centerGem, offsetRailX, doTransformUp: true);
		if (array.Length != 4)
		{
			return false;
		}
		OnCurve[] array2 = array;
		for (int i = 0; i < array2.Length; i++)
		{
			array2[i].Translate(new On3dVector(centerGem.Normal.Convert()) * offsetRailZ);
		}
		if (!CSUtilCurves.MergeCurves(new IOnCurve[2]
		{
			array[0],
			array[1]
		}, out OnCurve mergedCrv, 0.01))
		{
			return false;
		}
		if (!CSUtilCurves.MergeCurves(new IOnCurve[2]
		{
			array[2],
			array[3]
		}, out OnCurve mergedCrv2, 0.01))
		{
			return false;
		}
		CSDataArray cSDataArray = new CSDataArray();
		if (!PlaceGemCorners(new OnCurve[2] { mergedCrv2, mergedCrv }, centerGem, gemDefinition, xTransformLocal, cSDataArray, angle))
		{
			return false;
		}
		outputGems.Append(cSDataArray.GetArrayItem(0));
		if (!PlaceGemsCurveSide(mergedCrv, centerGem, gemDefinition, gemDefinitionSizeX, spacing, 0, xTransformLocal, outputGems))
		{
			return false;
		}
		outputGems.Append(cSDataArray.GetArrayItem(1));
		if (!PlaceGemsCurveSide(mergedCrv2, centerGem, gemDefinition, gemDefinitionSizeX, spacing, 0, xTransformLocal, outputGems))
		{
			return false;
		}
		return true;
	}

	private static bool PlaceGemsRound(string centerGemShape, CSGemData centerGem, CSGemData gemDefinition, double gemDefinitionSizeX, double spacing, double offsetRailX, double offsetRailZ, IOnXform xTransformLocal, double fixPosition, bool isEven, ref CSDataArray outputGems)
	{
		if (!GemOffsetCurve.GemOffset(centerGemShape, centerGem, offsetRailX, out OnCurve curve, true, out string _, 0.0, 0.0, 0.0))
		{
			return false;
		}
		curve.Translate(new On3dVector(centerGem.Normal.Convert()) * offsetRailZ);
		double length = 0.0;
		if (!curve.GetLength(ref length))
		{
			return false;
		}
		int num = (int)(length / (gemDefinitionSizeX + spacing));
		if (isEven && num % 2 == 1)
		{
			num--;
		}
		double[] array = new double[num];
		double[] t = new double[num];
		double num2 = 1.0 / (double)num;
		double num3 = fixPosition % num2;
		for (int i = 0; i < num; i++)
		{
			array[i] = num3;
			num3 += num2;
			if (num3 > 1.0)
			{
				num3 -= 1.0;
			}
		}
		Array.Sort(array);
		curve.GetNormalizedArcLengthPoints(array, ref t);
		for (int j = 0; j < num; j++)
		{
			OnXform xform = CSBlockUtil.CSUtilMath.Orient3Point(OnUtil.On_origin, OnUtil.On_xaxis, gemDefinition.Up.Convert(), curve.PointAt(t[j]), curve.TangentAt(t[j]), centerGem.Up.Convert());
			CSData data = gemDefinition.Duplicate();
			CSDataHelper.Transform(data, xform.Convert() * xTransformLocal.Convert());
			outputGems.Append(data);
		}
		return true;
	}

	private static bool PlaceGemsSquare(string centerGemShape, CSGemData centerGem, CSGemData gemDefinition, double gemDefinitionSizeX, double spacing, double offsetRailX, double offsetRailZ, IOnXform xTransformLocal, int style, double cornerBlend, double angle, ref CSDataArray outputGems)
	{
		OnCurve[] array = GemOffsetCurve.GemOffsetCurvesOld(centerGemShape, centerGem, offsetRailX, doTransformUp: true, cornerBlend);
		if (array.Length != 5)
		{
			return false;
		}
		OnCurve[] array2 = array;
		for (int i = 0; i < array2.Length; i++)
		{
			array2[i].Translate(new On3dVector(centerGem.Normal.Convert()) * offsetRailZ);
		}
		IOnCurve onCurve = array[0];
		IOnCurve onCurve2 = array[1];
		IOnCurve onCurve3 = array[2];
		IOnCurve onCurve4 = array[3];
		IOnCurve onCurve5 = array[4];
		if (!CSUtilCurves.MergeCurves(new IOnCurve[2] { onCurve, onCurve5 }, out OnCurve mergedCrv, 0.01))
		{
			return false;
		}
		CSDataArray cSDataArray = null;
		if (style == 0)
		{
			cSDataArray = new CSDataArray();
			if (!PlaceGemCorners(new IOnCurve[4] { mergedCrv, onCurve2, onCurve3, onCurve4 }, centerGem, gemDefinition, xTransformLocal, cSDataArray, angle))
			{
				return false;
			}
		}
		CSDataArray cSDataArray2 = new CSDataArray();
		if (!PlaceGemsCurveSide(mergedCrv, centerGem, gemDefinition, gemDefinitionSizeX, spacing, style, xTransformLocal, cSDataArray2))
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
		if (!PlaceGemsCurveSide(onCurve2, centerGem, gemDefinition, gemDefinitionSizeX, spacing, style, xTransformLocal, outputGems))
		{
			return false;
		}
		if (style == 0)
		{
			outputGems.Append(cSDataArray.GetArrayItem(1));
		}
		if (!PlaceGemsCurveSide(onCurve3, centerGem, gemDefinition, gemDefinitionSizeX, spacing, style, xTransformLocal, outputGems))
		{
			return false;
		}
		if (style == 0)
		{
			outputGems.Append(cSDataArray.GetArrayItem(2));
		}
		if (!PlaceGemsCurveSide(onCurve4, centerGem, gemDefinition, gemDefinitionSizeX, spacing, style, xTransformLocal, outputGems))
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
		OnCurve[] array = GemOffsetCurve.GemOffsetCurvesOld(centerGemShape, centerGem, offsetRailX, doTransformUp: true);
		if (array.Length != 4)
		{
			return false;
		}
		OnCurve[] array2 = array;
		for (int i = 0; i < array2.Length; i++)
		{
			array2[i].Translate(new On3dVector(centerGem.Normal.Convert()) * offsetRailZ);
		}
		CSDataArray cSDataArray = null;
		if (style == 0)
		{
			cSDataArray = new CSDataArray();
			if (!PlaceGemCorners(new OnCurve[4]
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
		if (!PlaceGemsCurveSide(array[0], centerGem, gemDefinition, gemDefinitionSizeX, spacing, style, xTransformLocal, outputGems))
		{
			return false;
		}
		if (style == 0)
		{
			outputGems.Append(cSDataArray.GetArrayItem(0));
		}
		if (!PlaceGemsCurveSide(array[1], centerGem, gemDefinition, gemDefinitionSizeX, spacing, style, xTransformLocal, outputGems))
		{
			return false;
		}
		if (style == 0)
		{
			outputGems.Append(cSDataArray.GetArrayItem(1));
		}
		if (!PlaceGemsCurveSide(array[2], centerGem, gemDefinition, gemDefinitionSizeX, spacing, style, xTransformLocal, outputGems))
		{
			return false;
		}
		if (style == 0)
		{
			outputGems.Append(cSDataArray.GetArrayItem(2));
		}
		if (!PlaceGemsCurveSide(array[3], centerGem, gemDefinition, gemDefinitionSizeX, spacing, style, xTransformLocal, outputGems))
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
			array2[i].Translate(centerGem.Normal.Convert() * offsetRailZ);
		}
		IOnCurve onCurve = array[0];
		IOnCurve onCurve2 = array[1];
		IOnCurve onCurve3 = array[2];
		IOnCurve onCurve4 = array[3];
		if (!CSUtilCurves.MergeCurves(new IOnCurve[2] { onCurve, onCurve4 }, out OnCurve mergedCrv, 0.01))
		{
			return false;
		}
		CSDataArray cSDataArray = null;
		if (style == 0)
		{
			cSDataArray = new CSDataArray();
			if (!PlaceGemCorners(new IOnCurve[3] { mergedCrv, onCurve2, onCurve3 }, centerGem, gemDefinition, xTransformLocal, cSDataArray, angle))
			{
				return false;
			}
		}
		CSDataArray cSDataArray2 = new CSDataArray();
		if (!PlaceGemsCurveSide(mergedCrv, centerGem, gemDefinition, gemDefinitionSizeX, spacing, style, xTransformLocal, cSDataArray2))
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
		if (!PlaceGemsCurveSide(onCurve2, centerGem, gemDefinition, gemDefinitionSizeX, spacing, style, xTransformLocal, outputGems))
		{
			return false;
		}
		if (style == 0)
		{
			outputGems.Append(cSDataArray.GetArrayItem(1));
		}
		if (!PlaceGemsCurveSide(onCurve3, centerGem, gemDefinition, gemDefinitionSizeX, spacing, style, xTransformLocal, outputGems))
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
			array2[i].Translate(centerGem.Normal.Convert() * offsetRailZ);
		}
		IOnCurve onCurve = array[0];
		IOnCurve onCurve2 = array[1];
		IOnCurve onCurve3 = array[2];
		IOnCurve onCurve4 = array[3];
		IOnCurve onCurve5 = array[4];
		IOnCurve onCurve6 = array[5];
		IOnCurve onCurve7 = array[6];
		IOnCurve onCurve8 = array[7];
		IOnCurve onCurve9 = array[8];
		IOnCurve onCurve10 = array[9];
		IOnCurve onCurve11 = array[10];
		IOnCurve onCurve12 = array[11];
		IOnCurve[] inputCrvs = new IOnCurve[4] { onCurve, onCurve12, onCurve2, onCurve11 };
		IOnCurve[] inputCrvs2 = new IOnCurve[4] { onCurve3, onCurve4, onCurve5, onCurve6 };
		IOnCurve[] inputCrvs3 = new IOnCurve[4] { onCurve7, onCurve8, onCurve9, onCurve10 };
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
			if (!PlaceGemCorners(new OnCurve[3] { mergedCrv, mergedCrv2, mergedCrv3 }, centerGem, gemDefinition, xTransformLocal, cSDataArray, angle))
			{
				return false;
			}
		}
		CSDataArray cSDataArray2 = new CSDataArray();
		if (!PlaceGemsCurveSide(mergedCrv, centerGem, gemDefinition, gemDefinitionSizeX, spacing, style, xTransformLocal, cSDataArray2))
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
		if (!PlaceGemsCurveSide(mergedCrv2, centerGem, gemDefinition, gemDefinitionSizeX, spacing, style, xTransformLocal, outputGems))
		{
			return false;
		}
		if (style == 0)
		{
			outputGems.Append(cSDataArray.GetArrayItem(1));
		}
		if (!PlaceGemsCurveSide(mergedCrv3, centerGem, gemDefinition, gemDefinitionSizeX, spacing, style, xTransformLocal, outputGems))
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
