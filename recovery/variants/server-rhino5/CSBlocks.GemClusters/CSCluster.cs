using System;
using System.Collections.Generic;
using CS.CSDataUtilities;
using CS.CSObjects.CSBlocks;
using CS.Parametric;
using CS.Parametric.Channels;
using CSBlockDataUtils;
using CSBlockDataUtils.CSGemOffsetCurves;
using CSBlockUtil;
using CSBlocks.GemClusters.CSClusterGemShapes;
using CSDatas;
using CSDatas.Interfaces;
using CSGemInstance.GemTableData;
using GV.CounterSketch.Shared;
using Rhino;
using Rhino.Geometry;

namespace CSBlocks.GemClusters;

[CSBlock("Cluster", "{69AA0AA3-2835-4829-94E3-77D48F68DACE}")]
public sealed class CSCluster : CSBlock
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

	public readonly CSInputChannel InProngCornerUse;

	public readonly CSInputChannel InProngCornerPosition;

	public readonly CSInputChannel InProngCornerGemType;

	public readonly CSInputChannel InProngCornerMetalType;

	public readonly CSInputChannel InProngCornerSizing;

	public readonly CSOutputChannel OutGem;

	public readonly CSOutputChannel OutGemSizeZ;

	public readonly CSOutputChannel OutChannelFix;

	public readonly CSOutputChannel OutMinCuletUp;

	public readonly CSOutputChannel OutMinCuletSmallerFlag;

	public CSCluster(string identifier)
		: base(identifier)
	{
		InCenterObject = new CSInputChannel(this, ChannelDataType.Any, ChannelArrayType.Array);
		InCenterGemShapeOverrideEnabled = new CSInputChannel(this, ChannelDataType.Boolean, ChannelArrayType.Individual, defaultValue: false);
		InCenterGemShapeOverride = new CSInputChannel(this, ChannelDataType.Gem, ChannelArrayType.Individual, ChannelFlag.NotRequired);
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
		InProngCornerUse = new CSInputChannel(this, ChannelDataType.Boolean, ChannelArrayType.Individual, defaultValue: false);
		InProngCornerPosition = new CSInputChannel(this, ChannelDataType.Double, ChannelArrayType.Array, new List<double>());
		InProngCornerGemType = new CSInputChannel(this, ChannelDataType.Integer, ChannelArrayType.Array, new List<int>());
		InProngCornerMetalType = new CSInputChannel(this, ChannelDataType.Integer, ChannelArrayType.Array, new List<int>());
		InProngCornerSizing = new CSInputChannel(this, ChannelDataType.Double, ChannelArrayType.Array, new List<double>());
		OutGem = new CSOutputChannel(this, ChannelDataType.Gem, ChannelArrayType.Array);
		OutGemSizeZ = new CSOutputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual);
		OutChannelFix = new CSOutputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual);
		OutMinCuletUp = new CSOutputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual);
		OutMinCuletSmallerFlag = new CSOutputChannel(this, ChannelDataType.Boolean, ChannelArrayType.Individual);
		SetChannelIdentifiers(autoCheckInputs: true);
	}

	public override bool Update()
	{
		CSClusterParam cSClusterParam = new CSClusterParam(this);
		bool flag = cSClusterParam.InShowChannel || cSClusterParam.InForceGems;
		CSDataArray inCenterObject = cSClusterParam.InCenterObject;
		if (inCenterObject.GetArrayCount() <= 0)
		{
			return UpdateMissing("CenterGem");
		}
		CSGemData cSGemData = inCenterObject.GetArrayItem(0) as CSGemData;
		CSData arrayItem = inCenterObject.GetArrayItem(0);
		bool flag2 = cSClusterParam.InCenterGemShapeOverrideEnabled || cSGemData == null;
		double num = cSClusterParam.InCenterGemSpacing + cSClusterParam.InRailX;
		double num2 = cSClusterParam.InPlacement + cSClusterParam.InRailZ;
		double number = 0.0;
		bool flag3 = false;
		Transform transform = Transform.Rotation(cSClusterParam.InRotation, Vector3d.ZAxis, Point3d.Origin);
		double num3 = cSClusterParam.InProfileWidth - (cSClusterParam.InChannelWallThicknessInner + cSClusterParam.InChannelWallThicknessOuter);
		double num4 = num3 - cSClusterParam.InChannelSpacing * 2.0;
		double bestGemSizeX = -1.23432101234321E+308;
		double num5 = -1.23432101234321E+308;
		CSGemData inGemDefinition = cSClusterParam.InGemDefinition;
		if (!cSClusterParam.InGemSizeToggle)
		{
			if (Math.Abs(num3) < 0.001)
			{
				bestGemSizeX = inGemDefinition.GemXSize;
				num5 = inGemDefinition.GemYSize;
			}
			else
			{
				GemSize gemSize = null;
				double num6 = -1.23432101234321E+308;
				foreach (GemSize size in inGemDefinition.Sizes)
				{
					double num7 = size.X / inGemDefinition.GemXSize;
					double num8 = size.Y / inGemDefinition.GemYSize;
					double zScaleFactor = (num7 + num8) / 2.0;
					Transform transform2 = Transform.Scale(Plane.WorldXY, num7, num8, zScaleFactor);
					NurbsCurve nurbsCurve = inGemDefinition.GetNurbsCurve();
					nurbsCurve.Transform(transform * transform2);
					BoundingBox boundingBox = nurbsCurve.GetBoundingBox(accurate: true);
					double num9 = Math.Abs(boundingBox.Max.X) + Math.Abs(boundingBox.Min.X);
					double num10 = Math.Abs(boundingBox.Max.Y) + Math.Abs(boundingBox.Min.Y);
					double num11 = Math.Abs(num4 - num10);
					if (!RhinoMath.IsValidDouble(num6) || num11 < num6)
					{
						gemSize = size;
						bestGemSizeX = num9;
						num5 = num10;
						num6 = num11;
					}
				}
				inGemDefinition.ScaleToSize(gemSize.X, gemSize.Y);
			}
		}
		else
		{
			inGemDefinition.ScaleToSize(cSClusterParam.InGemSize.GemXSize, cSClusterParam.InGemSize.GemYSize);
			num5 = cSClusterParam.InGemSize.GemYSize;
			bestGemSizeX = cSClusterParam.InGemSize.GemXSize;
			num3 = cSClusterParam.InGemSize.GemXSize + cSClusterParam.InChannelSpacing;
		}
		double num12 = 0.0;
		if (cSClusterParam.InUseMinCuletDistance == ValueListItem.MinCuletGrowUp)
		{
			if (inGemDefinition.GemZSize > cSClusterParam.InProfileHeight - cSClusterParam.InMinCuletDistance)
			{
				num12 = inGemDefinition.GemZSize - (cSClusterParam.InProfileHeight - cSClusterParam.InMinCuletDistance);
			}
			number = num12;
		}
		if (cSClusterParam.InUseMinCuletDistance == ValueListItem.MinCuletGrowSmaller)
		{
			double gemXSize = inGemDefinition.GemXSize;
			double gemYSize = inGemDefinition.GemYSize;
			CSUtilGem.GetGemScaleFromTableWithMaxZ(inGemDefinition, inGemDefinition.GemXSize, inGemDefinition.GemYSize, cSClusterParam.InProfileHeight - cSClusterParam.InMinCuletDistance, useLargestPossible: true, useIterationOnly: false, out var xBestScale);
			inGemDefinition.ApplyTransform(xBestScale);
			if (gemXSize - inGemDefinition.GemXSize > 0.1 || gemYSize - inGemDefinition.GemYSize > 0.1)
			{
				bestGemSizeX = inGemDefinition.GemXSize;
				num5 = inGemDefinition.GemYSize;
				num3 = inGemDefinition.GemXSize + cSClusterParam.InChannelSpacing;
				flag3 = true;
			}
		}
		double dx = inGemDefinition.GemXSize / 2.0 - inGemDefinition.GemXMax;
		double dy = inGemDefinition.GemYSize / 2.0 - inGemDefinition.GemYMax;
		Transform transform3 = Transform.Translation(dx, dy, 0.0);
		Transform transform4 = Transform.Translation(0.0, 0.0, 0.0 - Math.Abs(inGemDefinition.GemZMax));
		Transform transform5 = Transform.Rotation(cSClusterParam.InAngle, Vector3d.XAxis, Point3d.Origin);
		Transform xTransformLocal = Transform.Translation(0.0, 0.0, num2 + num12) * transform5 * transform4 * transform3 * transform;
		CSGemData cSGemData2 = cSClusterParam.InCenterGemShapeOverride;
		string gemShape;
		if (flag2)
		{
			if (cSGemData2 == null)
			{
				cSGemData2 = new CSGemData(cSGemData);
			}
			else
			{
				BoundingBox boundingBox2 = BoundingBox.Unset;
				arrayItem.GetTightBoundingBox(ref boundingBox2);
				double num13 = Math.Abs(boundingBox2.Max.X - boundingBox2.Min.X + cSClusterParam.InOffsetX);
				double num14 = Math.Abs(boundingBox2.Max.Y - boundingBox2.Min.Y + cSClusterParam.InOffsetY);
				double num15;
				double num16;
				if (cSGemData != null)
				{
					switch (cSClusterParam.InCenterGemShapeOverrideMode)
					{
					case ValueListItem.GemClusterCenterShapeOverrideModeDimensionX:
						num15 = (cSGemData.GemXSize + cSClusterParam.InOffsetX) / cSGemData2.GemXSize;
						num16 = num15;
						break;
					case ValueListItem.GemClusterCenterShapeOverrideModeDimensionY:
						num16 = (cSGemData.GemYSize + cSClusterParam.InOffsetY) / cSGemData2.GemYSize;
						num15 = num16;
						break;
					case ValueListItem.GemClusterCenterShapeOverrideModeDimensionLargest:
						if (cSGemData.GemXSize > cSGemData.GemYSize)
						{
							num15 = (cSGemData.GemXSize + cSClusterParam.InOffsetX) / cSGemData2.GemXSize;
							num16 = num15;
						}
						else
						{
							num16 = (cSGemData.GemYSize + cSClusterParam.InOffsetY) / cSGemData2.GemYSize;
							num15 = num16;
						}
						break;
					case ValueListItem.GemClusterCenterShapeOverrideModeDimensionSmallest:
						if (cSGemData.GemXSize < cSGemData.GemYSize)
						{
							num15 = (cSGemData.GemXSize + cSClusterParam.InOffsetX) / cSGemData2.GemXSize;
							num16 = num15;
						}
						else
						{
							num16 = (cSGemData.GemYSize + cSClusterParam.InOffsetY) / cSGemData2.GemYSize;
							num15 = num16;
						}
						break;
					case ValueListItem.GemClusterCenterShapeOverrideModeDimensionXandY:
						num15 = (cSGemData.GemXSize + cSClusterParam.InOffsetX) / cSGemData2.GemXSize;
						num16 = (cSGemData.GemYSize + cSClusterParam.InOffsetY) / cSGemData2.GemYSize;
						break;
					default:
						return UpdateError("Invalid Gem Cluster Center Shape Override Mode");
					}
				}
				else
				{
					switch (cSClusterParam.InCenterGemShapeOverrideMode)
					{
					case ValueListItem.GemClusterCenterShapeOverrideModeDimensionX:
						num15 = num13 / cSGemData2.GemXSize;
						num16 = num15;
						break;
					case ValueListItem.GemClusterCenterShapeOverrideModeDimensionY:
						num16 = num14 / cSGemData2.GemYSize;
						num15 = num16;
						break;
					case ValueListItem.GemClusterCenterShapeOverrideModeDimensionLargest:
						if (num13 > num14)
						{
							num15 = num13 / cSGemData2.GemXSize;
							num16 = num15;
						}
						else
						{
							num16 = num14 / cSGemData2.GemYSize;
							num15 = num16;
						}
						break;
					case ValueListItem.GemClusterCenterShapeOverrideModeDimensionSmallest:
						if (num13 < num14)
						{
							num15 = num13 / cSGemData2.GemXSize;
							num16 = num15;
						}
						else
						{
							num16 = num14 / cSGemData2.GemYSize;
							num15 = num16;
						}
						break;
					case ValueListItem.GemClusterCenterShapeOverrideModeDimensionXandY:
						num15 = num13 / cSGemData2.GemXSize;
						num16 = num14 / cSGemData2.GemYSize;
						break;
					default:
						return UpdateError("Invalid Gem Cluster Center Shape Override Mode");
					}
				}
				cSGemData2.Scale(num15, num16);
				Transform xUp;
				if (cSGemData != null)
				{
					if (!CSBlockUtil.CSUtilMath.GetUnscaledXform(cSGemData.Xform, out xUp))
					{
						xUp = Transform.Identity;
					}
				}
				else
				{
					xUp = Transform.Translation(boundingBox2.Center.X, boundingBox2.Center.Y, boundingBox2.Center.Z) * Transform.Rotation(RhinoMath.ToRadians(cSClusterParam.InRotate), Vector3d.YAxis, Point3d.Origin);
				}
				cSGemData2.ApplyTransform(xUp);
			}
			gemShape = cSGemData2.GemShape;
		}
		else
		{
			cSGemData2 = new CSGemData(cSGemData);
			gemShape = cSGemData.GemShape;
		}
		double offsetRailZ;
		double num17;
		if (Math.Abs(num3) < 0.001)
		{
			num17 = num + num5 / 2.0 - cSClusterParam.InGemHeightOffset * Math.Sin(cSClusterParam.InAngle);
			offsetRailZ = 0.0 + cSClusterParam.InGemHeightOffset * Math.Cos(cSClusterParam.InAngle);
		}
		else
		{
			num17 = num + (cSClusterParam.InChannelWallThicknessInner + num3 / 2.0) * Math.Cos(cSClusterParam.InAngle) - cSClusterParam.InGemHeightOffset * Math.Sin(cSClusterParam.InAngle);
			offsetRailZ = (cSClusterParam.InChannelWallThicknessInner + num3 / 2.0) * Math.Sin(cSClusterParam.InAngle) + cSClusterParam.InGemHeightOffset * Math.Cos(cSClusterParam.InAngle);
		}
		CSGemData cSGemData3 = (cSClusterParam.InXYScaleToGem ? cSGemData : cSGemData2);
		if (cSGemData == null)
		{
			cSGemData3 = cSGemData2;
		}
		double optionalEmeraldCornerXPercent = 0.0;
		double optionalEmeraldCornerYPercent = 0.0;
		if (cSClusterParam.InEmeraldCornerPoint1 != Point3d.Unset && cSClusterParam.InEmeraldCornerPoint2 != Point3d.Unset)
		{
			double num18 = cSGemData3.GemXSize / 2.0;
			optionalEmeraldCornerXPercent = cSClusterParam.InEmeraldCornerPoint1.X / num18;
			double num19 = cSGemData3.GemYSize / 2.0;
			optionalEmeraldCornerYPercent = cSClusterParam.InEmeraldCornerPoint2.Y / num19;
		}
		CSDataArray outputGems;
		double number2;
		if (!cSClusterParam.InFixedGemSpacing)
		{
			if (!GetGemLayout(cSClusterParam, gemShape, cSGemData3, inGemDefinition, bestGemSizeX, num17, offsetRailZ, xTransformLocal, optionalEmeraldCornerXPercent, optionalEmeraldCornerYPercent, out outputGems, out var outError))
			{
				return UpdateError(outError);
			}
			number2 = 0.0;
		}
		else
		{
			double num20 = num17;
			num17 -= 0.1;
			bool flag4 = false;
			bool flag5 = true;
			if (!GetGemLayout(cSClusterParam, gemShape, cSGemData3, inGemDefinition, bestGemSizeX, num17, offsetRailZ, xTransformLocal, optionalEmeraldCornerXPercent, optionalEmeraldCornerYPercent, out outputGems, out var outError2))
			{
				return UpdateError(outError2);
			}
			double num21 = GetSpacing(outputGems);
			num17 += 0.1;
			CSDataArray cSDataArray;
			while (true)
			{
				if (!GetGemLayout(cSClusterParam, gemShape, cSGemData3, inGemDefinition, bestGemSizeX, num17, offsetRailZ, xTransformLocal, optionalEmeraldCornerXPercent, optionalEmeraldCornerYPercent, out outputGems, out outError2))
				{
					return UpdateError(outError2);
				}
				double spacing = GetSpacing(outputGems);
				if (flag5 && spacing - cSClusterParam.InSpacing < cSClusterParam.InGemSpacingTolerance)
				{
					cSDataArray = outputGems;
					flag4 = true;
					break;
				}
				flag5 = false;
				if (spacing < num21)
				{
					cSDataArray = outputGems;
					num17 -= 0.01;
					num21 = spacing;
					break;
				}
				num17 += 0.1;
				num21 = spacing;
			}
			if (!flag4)
			{
				while (true)
				{
					if (!GetGemLayout(cSClusterParam, gemShape, cSGemData3, inGemDefinition, bestGemSizeX, num17, offsetRailZ, xTransformLocal, optionalEmeraldCornerXPercent, optionalEmeraldCornerYPercent, out outputGems, out outError2))
					{
						return UpdateError(outError2);
					}
					double spacing2 = GetSpacing(outputGems);
					if (spacing2 > num21)
					{
						break;
					}
					cSDataArray = outputGems;
					num17 -= 0.01;
					num21 = spacing2;
				}
			}
			outputGems = cSDataArray;
			number2 = num17 - num20;
		}
		if (!flag)
		{
			OutGemSizeZ.SetData(null);
			OutGem.SetData(outputGems);
			OutMinCuletUp.SetData(null);
			OutMinCuletSmallerFlag.SetData(null);
		}
		else
		{
			OutGem.SetData(outputGems);
			OutGemSizeZ.SetData(new CSNumberData(cSClusterParam.InGemDefinition.GemZSize));
			OutChannelFix.SetData(new CSNumberData(number2));
			OutMinCuletUp.SetData(new CSNumberData(number));
			OutMinCuletSmallerFlag.SetData(new CSNumberData(flag3 ? 1 : 0));
		}
		return base.Update();
	}

	private static bool GetGemLayout(CSClusterParam inCluster, string centerGemShape, CSGemData offsetControlShape, CSGemData gemDefinition, double bestGemSizeX, double offsetRailX, double offsetRailZ, Transform xTransformLocal, double optionalEmeraldCornerXPercent, double optionalEmeraldCornerYPercent, out CSDataArray outputGems, out string outError)
	{
		outputGems = new CSDataArray();
		switch (centerGemShape)
		{
		case "Cushion":
		case "Cushion Rotated 90":
		case "Oval":
		case "Oval Rotated 90":
		case "Round":
			if (!CSClusterGemRound.PlaceGems(inCluster, centerGemShape, offsetControlShape, gemDefinition, bestGemSizeX, inCluster.InSpacing, offsetRailX, offsetRailZ, xTransformLocal, inCluster.InFixPosition, inCluster.InEven, ref outputGems))
			{
				outError = "PlaceGemsRound failed";
				return false;
			}
			break;
		case "Cushion Square":
			if (!CSClusterGemSquare.PlaceGems(inCluster, centerGemShape, offsetControlShape, gemDefinition, bestGemSizeX, inCluster.InSpacing, offsetRailX, offsetRailZ, xTransformLocal, 0, inCluster.InCornerBlend, inCluster.InAngle, ref outputGems))
			{
				outError = "PlaceGemsSquare failed";
				return false;
			}
			break;
		case "Princess":
		case "Baguette Straight":
		case "Baguette Tapered":
		case "BaguetteTapered":
			if (!CSClusterGemSquare.PlaceGems(inCluster, centerGemShape, offsetControlShape, gemDefinition, bestGemSizeX, inCluster.InSpacing, offsetRailX, offsetRailZ, xTransformLocal, inCluster.InStyle, inCluster.InCornerBlend, inCluster.InAngle, ref outputGems))
			{
				outError = "PlaceGemsSquare failed";
				return false;
			}
			break;
		case "Cushion Square Rotated 45":
		case "Princess Rotated 45":
			if (!PlaceGemsSquareRotated(centerGemShape, offsetControlShape, gemDefinition, bestGemSizeX, inCluster.InSpacing, offsetRailX, offsetRailZ, xTransformLocal, inCluster.InStyle, inCluster.InAngle, ref outputGems))
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
			if (!CSClusterGemEmerald.PlaceGems(inCluster, centerGemShape, offsetControlShape, gemDefinition, bestGemSizeX, inCluster.InSpacing, offsetRailX, offsetRailZ, xTransformLocal, optionalEmeraldCornerXPercent, optionalEmeraldCornerYPercent, ref outputGems))
			{
				outError = "PlaceGemsEmerald failed";
				return false;
			}
			break;
		case "Heart":
			if (!PlaceGemsHeart(centerGemShape, offsetControlShape, gemDefinition, bestGemSizeX, inCluster.InSpacing, offsetRailX, offsetRailZ, xTransformLocal, inCluster.InAngle, ref outputGems))
			{
				outError = "PlaceGemsHeart failed";
				return false;
			}
			break;
		case "Marquise":
			if (!CSClusterGemMarquise.PlaceGems(inCluster, centerGemShape, offsetControlShape, gemDefinition, bestGemSizeX, inCluster.InSpacing, offsetRailX, offsetRailZ, xTransformLocal, inCluster.InAngle, ref outputGems))
			{
				outError = "PlaceGemsMarquise failed";
				return false;
			}
			break;
		case "Pear":
			if (!CSClusterGemPear.PlaceGems(inCluster, centerGemShape, offsetControlShape, gemDefinition, bestGemSizeX, inCluster.InSpacing, offsetRailX, offsetRailZ, xTransformLocal, inCluster.InAngle, ref outputGems))
			{
				outError = "PlaceGemsPear failed";
				return false;
			}
			break;
		case "Marquise Rotated 90":
			if (!PlaceGemsMarquiseRotated(centerGemShape, offsetControlShape, gemDefinition, bestGemSizeX, inCluster.InSpacing, offsetRailX, offsetRailZ, xTransformLocal, inCluster.InAngle, ref outputGems))
			{
				outError = "PlaceGemsMarquiseRotated failed";
				return false;
			}
			break;
		case "Calf":
			if (!PlaceGemsCalf(centerGemShape, offsetControlShape, gemDefinition, bestGemSizeX, inCluster.InSpacing, offsetRailX, offsetRailZ, xTransformLocal, inCluster.InStyle, inCluster.InAngle, ref outputGems))
			{
				outError = "PlaceGemsCalf failed";
				return false;
			}
			break;
		case "Triangle":
			if (!PlaceGemsTriangle(centerGemShape, offsetControlShape, gemDefinition, bestGemSizeX, inCluster.InSpacing, offsetRailX, offsetRailZ, xTransformLocal, inCluster.InStyle, inCluster.InAngle, ref outputGems))
			{
				outError = "PlaceGemsTriangle failed";
				return false;
			}
			break;
		case "Trillion Curved":
		case "Trillion Straight":
			if (!PlaceGemsTrillionCurved(centerGemShape, offsetControlShape, gemDefinition, bestGemSizeX, inCluster.InSpacing, offsetRailX, offsetRailZ, xTransformLocal, inCluster.InStyle, inCluster.InAngle, ref outputGems))
			{
				outError = "PlaceGemsTrillion failed";
				return false;
			}
			break;
		case "Half Moon":
			if (!PlaceGemsHalfMoon(centerGemShape, offsetControlShape, gemDefinition, bestGemSizeX, inCluster.InSpacing, offsetRailX, offsetRailZ, xTransformLocal, inCluster.InStyle, inCluster.InAngle, ref outputGems))
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

	private static double GetSpacing(ICSArray gemarray)
	{
		CSGemData cSGemData = gemarray.GetArrayItem(0) as CSGemData;
		CSGemData cSGemData2 = gemarray.GetArrayItem(1) as CSGemData;
		if (cSGemData == null || cSGemData2 == null)
		{
			return double.NaN;
		}
		CSDataHelper.GetPoint(cSGemData, out var value);
		CSDataHelper.GetPoint(cSGemData2, out var value2);
		double num = cSGemData.GemXSize / 2.0;
		double num2 = cSGemData.GemYSize / 2.0;
		return (value - value2).Length - num - num2;
	}

	private static bool PlaceGemsCalf(string centerGemShape, CSGemData centerGem, CSGemData gemDefinition, double gemDefinitionSizeX, double spacing, double offsetRailX, double offsetRailZ, Transform xTransformLocal, int style, double angle, ref CSDataArray outputGems)
	{
		Curve[] array = GemOffsetCurve.GemOffsetCurves(centerGemShape, centerGem, offsetRailX, doTransformUp: true);
		if (array.Length != 7)
		{
			return false;
		}
		Curve[] array2 = array;
		for (int i = 0; i < array2.Length; i++)
		{
			array2[i].Translate(centerGem.Normal * offsetRailZ);
		}
		Curve curve = array[0];
		Curve curve2 = array[1];
		Curve curve3 = array[2];
		Curve curve4 = array[3];
		Curve curve5 = array[4];
		Curve curve6 = array[5];
		Curve curve7 = array[6];
		if (!CSUtilCurves.MergeCurves((IEnumerable<Curve>)new Curve[2] { curve, curve7 }, out Curve mergedCrv, 0.01))
		{
			return false;
		}
		CSDataArray cSDataArray = new CSDataArray();
		if (style == 0)
		{
			cSDataArray = new CSDataArray();
			if (!CSClusterGemUtils.PlaceGemCorners(new Curve[6] { mergedCrv, curve2, curve3, curve4, curve5, curve6 }, centerGem, gemDefinition, xTransformLocal, cSDataArray, angle))
			{
				return false;
			}
		}
		CSDataArray cSDataArray2 = new CSDataArray();
		if (!CSClusterGemUtils.PlaceGemsCurveSide(mergedCrv, centerGem, gemDefinition, gemDefinitionSizeX, spacing, style, xTransformLocal, cSDataArray2))
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
		if (!CSClusterGemUtils.PlaceGemsCurveSide(curve2, centerGem, gemDefinition, gemDefinitionSizeX, spacing, style, xTransformLocal, outputGems))
		{
			return false;
		}
		if (style == 0)
		{
			outputGems.Append(cSDataArray.GetArrayItem(1));
		}
		if (!CSClusterGemUtils.PlaceGemsCurveSide(curve3, centerGem, gemDefinition, gemDefinitionSizeX, spacing, style, xTransformLocal, outputGems))
		{
			return false;
		}
		if (style == 0)
		{
			outputGems.Append(cSDataArray.GetArrayItem(2));
		}
		if (!CSClusterGemUtils.PlaceGemsCurveSide(curve4, centerGem, gemDefinition, gemDefinitionSizeX, spacing, style, xTransformLocal, outputGems))
		{
			return false;
		}
		if (style == 0)
		{
			outputGems.Append(cSDataArray.GetArrayItem(3));
		}
		if (!CSClusterGemUtils.PlaceGemsCurveSide(curve5, centerGem, gemDefinition, gemDefinitionSizeX, spacing, style, xTransformLocal, outputGems))
		{
			return false;
		}
		if (style == 0)
		{
			outputGems.Append(cSDataArray.GetArrayItem(4));
		}
		if (!CSClusterGemUtils.PlaceGemsCurveSide(curve6, centerGem, gemDefinition, gemDefinitionSizeX, spacing, style, xTransformLocal, outputGems))
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

	private static bool PlaceGemsHalfMoon(string centerGemShape, CSGemData centerGem, CSGemData gemDefinition, double gemDefinitionSizeX, double spacing, double offsetRailX, double offsetRailZ, Transform xTransformLocal, int style, double angle, ref CSDataArray outputGems)
	{
		Curve[] array = GemOffsetCurve.GemOffsetCurves(centerGemShape, centerGem, offsetRailX, doTransformUp: true);
		if (array.Length != 4)
		{
			return false;
		}
		Curve[] array2 = array;
		for (int i = 0; i < array2.Length; i++)
		{
			array2[i].Translate(centerGem.Normal * offsetRailZ);
		}
		Curve curve = array[0];
		Curve curve2 = array[1];
		Curve curve3 = array[2];
		Curve curve4 = array[3];
		CSUtilCurves.MergeCurves((IEnumerable<Curve>)new Curve[2] { curve, curve4 }, out Curve mergedCrv, 0.01);
		CSUtilCurves.MergeCurves((IEnumerable<Curve>)new Curve[2] { curve2, curve3 }, out Curve mergedCrv2, 0.01);
		if (style == 0 && !CSClusterGemUtils.PlaceGemCorner(curve, curve2, centerGem, gemDefinition, xTransformLocal, angle, outputGems))
		{
			return false;
		}
		if (!CSClusterGemUtils.PlaceGemsCurveSide(mergedCrv, centerGem, gemDefinition, gemDefinitionSizeX, spacing, style, xTransformLocal, outputGems))
		{
			return false;
		}
		if (style == 0 && !CSClusterGemUtils.PlaceGemCorner(curve3, curve4, centerGem, gemDefinition, xTransformLocal, angle, outputGems))
		{
			return false;
		}
		if (!CSClusterGemUtils.PlaceGemsCurveSide(mergedCrv2, centerGem, gemDefinition, gemDefinitionSizeX, spacing, style, xTransformLocal, outputGems))
		{
			return false;
		}
		return true;
	}

	private static bool PlaceGemsHeart(string centerGemShape, CSGemData centerGem, CSGemData gemDefinition, double gemDefinitionSizeX, double spacing, double offsetRailX, double offsetRailZ, Transform xTransformLocal, double angle, ref CSDataArray outputGems)
	{
		Curve[] array = GemOffsetCurve.GemOffsetCurves(centerGemShape, centerGem, offsetRailX, doTransformUp: true);
		if (array.Length != 6)
		{
			return false;
		}
		Curve[] array2 = array;
		for (int i = 0; i < array2.Length; i++)
		{
			array2[i].Translate(centerGem.Normal * offsetRailZ);
		}
		if (!CSUtilCurves.MergeCurves((IEnumerable<Curve>)new Curve[3]
		{
			array[0],
			array[1],
			array[2]
		}, out Curve mergedCrv, 0.01))
		{
			return false;
		}
		if (!CSUtilCurves.MergeCurves((IEnumerable<Curve>)new Curve[3]
		{
			array[3],
			array[4],
			array[5]
		}, out Curve mergedCrv2, 0.01))
		{
			return false;
		}
		CSDataArray cSDataArray = new CSDataArray();
		if (!CSClusterGemUtils.PlaceGemCorners(new Curve[2] { mergedCrv2, mergedCrv }, centerGem, gemDefinition, xTransformLocal, cSDataArray, angle))
		{
			return false;
		}
		outputGems.Append(cSDataArray.GetArrayItem(0));
		if (!CSClusterGemUtils.PlaceGemsCurveSide(mergedCrv, centerGem, gemDefinition, gemDefinitionSizeX, spacing, 0, xTransformLocal, outputGems))
		{
			return false;
		}
		outputGems.Append(cSDataArray.GetArrayItem(1));
		if (!CSClusterGemUtils.PlaceGemsCurveSide(mergedCrv2, centerGem, gemDefinition, gemDefinitionSizeX, spacing, 0, xTransformLocal, outputGems))
		{
			return false;
		}
		return true;
	}

	private static bool PlaceGemsMarquiseRotated(string centerGemShape, CSGemData centerGem, CSGemData gemDefinition, double gemDefinitionSizeX, double spacing, double offsetRailX, double offsetRailZ, Transform xTransformLocal, double angle, ref CSDataArray outputGems)
	{
		Curve[] array = GemOffsetCurve.GemOffsetCurves(centerGemShape, centerGem, offsetRailX, doTransformUp: true);
		if (array.Length != 4)
		{
			return false;
		}
		Curve[] array2 = array;
		for (int i = 0; i < array2.Length; i++)
		{
			array2[i].Translate(centerGem.Normal * offsetRailZ);
		}
		Curve curve = array[0];
		Curve curve2 = array[1];
		Curve curve3 = array[2];
		Curve curve4 = array[3];
		Curve[] inputCrvs = new Curve[2] { curve4, curve };
		Curve[] inputCrvs2 = new Curve[2] { curve2, curve3 };
		if (!CSUtilCurves.MergeCurves((IEnumerable<Curve>)inputCrvs, out Curve mergedCrv, 0.01))
		{
			return false;
		}
		if (!CSUtilCurves.MergeCurves((IEnumerable<Curve>)inputCrvs2, out Curve mergedCrv2, 0.01))
		{
			return false;
		}
		CSDataArray cSDataArray = new CSDataArray();
		if (!CSClusterGemUtils.PlaceGemCorners(new Curve[2] { mergedCrv, mergedCrv2 }, centerGem, gemDefinition, xTransformLocal, cSDataArray, angle))
		{
			return false;
		}
		CSDataArray cSDataArray2 = new CSDataArray();
		if (!CSClusterGemUtils.PlaceGemsCurveSide(mergedCrv, centerGem, gemDefinition, gemDefinitionSizeX, spacing, 0, xTransformLocal, cSDataArray2))
		{
			return false;
		}
		int num = cSDataArray2.GetArrayCount() / 2;
		for (int j = num; j < cSDataArray2.GetArrayCount(); j++)
		{
			outputGems.Append(cSDataArray2.GetArrayItem(j));
		}
		outputGems.Append(cSDataArray.GetArrayItem(0));
		if (!CSClusterGemUtils.PlaceGemsCurveSide(mergedCrv2, centerGem, gemDefinition, gemDefinitionSizeX, spacing, 0, xTransformLocal, outputGems))
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

	private static bool PlaceGemsSquareRotated(string centerGemShape, CSGemData centerGem, CSGemData gemDefinition, double gemDefinitionSizeX, double spacing, double offsetRailX, double offsetRailZ, Transform xTransformLocal, int style, double angle, ref CSDataArray outputGems)
	{
		Curve[] array = GemOffsetCurve.GemOffsetCurves(centerGemShape, centerGem, offsetRailX, doTransformUp: true);
		if (array.Length != 4)
		{
			return false;
		}
		Curve[] array2 = array;
		for (int i = 0; i < array2.Length; i++)
		{
			array2[i].Translate(centerGem.Normal * offsetRailZ);
		}
		CSDataArray cSDataArray = new CSDataArray();
		if (style == 0 && !CSClusterGemUtils.PlaceGemCorners(new Curve[4]
		{
			array[0],
			array[1],
			array[2],
			array[3]
		}, centerGem, gemDefinition, xTransformLocal, cSDataArray, angle))
		{
			return false;
		}
		if (!CSClusterGemUtils.PlaceGemsCurveSide(array[0], centerGem, gemDefinition, gemDefinitionSizeX, spacing, style, xTransformLocal, outputGems))
		{
			return false;
		}
		if (style == 0)
		{
			outputGems.Append(cSDataArray.GetArrayItem(0));
		}
		if (!CSClusterGemUtils.PlaceGemsCurveSide(array[1], centerGem, gemDefinition, gemDefinitionSizeX, spacing, style, xTransformLocal, outputGems))
		{
			return false;
		}
		if (style == 0)
		{
			outputGems.Append(cSDataArray.GetArrayItem(1));
		}
		if (!CSClusterGemUtils.PlaceGemsCurveSide(array[2], centerGem, gemDefinition, gemDefinitionSizeX, spacing, style, xTransformLocal, outputGems))
		{
			return false;
		}
		if (style == 0)
		{
			outputGems.Append(cSDataArray.GetArrayItem(2));
		}
		if (!CSClusterGemUtils.PlaceGemsCurveSide(array[3], centerGem, gemDefinition, gemDefinitionSizeX, spacing, style, xTransformLocal, outputGems))
		{
			return false;
		}
		if (style == 0)
		{
			outputGems.Append(cSDataArray.GetArrayItem(3));
		}
		return true;
	}

	private static bool PlaceGemsTriangle(string centerGemShape, CSGemData centerGem, CSGemData gemDefinition, double gemDefinitionSizeX, double spacing, double offsetRailX, double offsetRailZ, Transform xTransformLocal, int style, double angle, ref CSDataArray outputGems)
	{
		Curve[] array = GemOffsetCurve.GemOffsetCurves(centerGemShape, centerGem, offsetRailX, doTransformUp: true);
		if (array.Length != 4)
		{
			return false;
		}
		Curve[] array2 = array;
		for (int i = 0; i < array2.Length; i++)
		{
			array2[i].Translate(centerGem.Normal * offsetRailZ);
		}
		Curve curve = array[0];
		Curve curve2 = array[1];
		Curve curve3 = array[2];
		Curve curve4 = array[3];
		if (!CSUtilCurves.MergeCurves((IEnumerable<Curve>)new Curve[2] { curve, curve4 }, out Curve mergedCrv, 0.01))
		{
			return false;
		}
		CSDataArray cSDataArray = new CSDataArray();
		if (style == 0 && !CSClusterGemUtils.PlaceGemCorners(new Curve[3] { mergedCrv, curve2, curve3 }, centerGem, gemDefinition, xTransformLocal, cSDataArray, angle))
		{
			return false;
		}
		CSDataArray cSDataArray2 = new CSDataArray();
		if (!CSClusterGemUtils.PlaceGemsCurveSide(mergedCrv, centerGem, gemDefinition, gemDefinitionSizeX, spacing, style, xTransformLocal, cSDataArray2))
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
		if (!CSClusterGemUtils.PlaceGemsCurveSide(curve2, centerGem, gemDefinition, gemDefinitionSizeX, spacing, style, xTransformLocal, outputGems))
		{
			return false;
		}
		if (style == 0)
		{
			outputGems.Append(cSDataArray.GetArrayItem(1));
		}
		if (!CSClusterGemUtils.PlaceGemsCurveSide(curve3, centerGem, gemDefinition, gemDefinitionSizeX, spacing, style, xTransformLocal, outputGems))
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

	private static bool PlaceGemsTrillionCurved(string centerGemShape, CSGemData centerGem, CSGemData gemDefinition, double gemDefinitionSizeX, double spacing, double offsetRailX, double offsetRailZ, Transform xTransformLocal, int style, double angle, ref CSDataArray outputGems)
	{
		Curve[] array = GemOffsetCurve.GemOffsetCurves(centerGemShape, centerGem, offsetRailX, doTransformUp: true);
		if (array.Length != 12)
		{
			return false;
		}
		Curve[] array2 = array;
		for (int i = 0; i < array2.Length; i++)
		{
			array2[i].Translate(centerGem.Normal * offsetRailZ);
		}
		Curve curve = array[0];
		Curve curve2 = array[1];
		Curve curve3 = array[2];
		Curve curve4 = array[3];
		Curve curve5 = array[4];
		Curve curve6 = array[5];
		Curve curve7 = array[6];
		Curve curve8 = array[7];
		Curve curve9 = array[8];
		Curve curve10 = array[9];
		Curve curve11 = array[10];
		Curve curve12 = array[11];
		Curve[] inputCrvs = new Curve[4] { curve, curve12, curve2, curve11 };
		Curve[] inputCrvs2 = new Curve[4] { curve3, curve4, curve5, curve6 };
		Curve[] inputCrvs3 = new Curve[4] { curve7, curve8, curve9, curve10 };
		if (!CSUtilCurves.MergeCurves((IEnumerable<Curve>)inputCrvs, out Curve mergedCrv, 0.01))
		{
			return false;
		}
		if (!CSUtilCurves.MergeCurves((IEnumerable<Curve>)inputCrvs2, out Curve mergedCrv2, 0.01))
		{
			return false;
		}
		if (!CSUtilCurves.MergeCurves((IEnumerable<Curve>)inputCrvs3, out Curve mergedCrv3, 0.01))
		{
			return false;
		}
		CSDataArray cSDataArray = new CSDataArray();
		if (style == 0 && !CSClusterGemUtils.PlaceGemCorners(new Curve[3] { mergedCrv, mergedCrv2, mergedCrv3 }, centerGem, gemDefinition, xTransformLocal, cSDataArray, angle))
		{
			return false;
		}
		CSDataArray cSDataArray2 = new CSDataArray();
		if (!CSClusterGemUtils.PlaceGemsCurveSide(mergedCrv, centerGem, gemDefinition, gemDefinitionSizeX, spacing, style, xTransformLocal, cSDataArray2))
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
		if (!CSClusterGemUtils.PlaceGemsCurveSide(mergedCrv2, centerGem, gemDefinition, gemDefinitionSizeX, spacing, style, xTransformLocal, outputGems))
		{
			return false;
		}
		if (style == 0)
		{
			outputGems.Append(cSDataArray.GetArrayItem(1));
		}
		if (!CSClusterGemUtils.PlaceGemsCurveSide(mergedCrv3, centerGem, gemDefinition, gemDefinitionSizeX, spacing, style, xTransformLocal, outputGems))
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
