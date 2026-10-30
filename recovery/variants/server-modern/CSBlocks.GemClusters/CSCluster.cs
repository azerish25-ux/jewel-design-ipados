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
		//IL_0273: Unknown result type (might be due to invalid IL or missing references)
		//IL_0287: Unknown result type (might be due to invalid IL or missing references)
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
		//IL_008e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0093: Unknown result type (might be due to invalid IL or missing references)
		//IL_0098: Unknown result type (might be due to invalid IL or missing references)
		//IL_009d: Unknown result type (might be due to invalid IL or missing references)
		//IL_03d3: Unknown result type (might be due to invalid IL or missing references)
		//IL_03d8: Unknown result type (might be due to invalid IL or missing references)
		//IL_03f9: Unknown result type (might be due to invalid IL or missing references)
		//IL_03fe: Unknown result type (might be due to invalid IL or missing references)
		//IL_0406: Unknown result type (might be due to invalid IL or missing references)
		//IL_040b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0410: Unknown result type (might be due to invalid IL or missing references)
		//IL_0415: Unknown result type (might be due to invalid IL or missing references)
		//IL_042e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0433: Unknown result type (might be due to invalid IL or missing references)
		//IL_0435: Unknown result type (might be due to invalid IL or missing references)
		//IL_043a: Unknown result type (might be due to invalid IL or missing references)
		//IL_043c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0441: Unknown result type (might be due to invalid IL or missing references)
		//IL_0443: Unknown result type (might be due to invalid IL or missing references)
		//IL_0448: Unknown result type (might be due to invalid IL or missing references)
		//IL_044a: Unknown result type (might be due to invalid IL or missing references)
		//IL_044f: Unknown result type (might be due to invalid IL or missing references)
		//IL_033f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0179: Unknown result type (might be due to invalid IL or missing references)
		//IL_0184: Unknown result type (might be due to invalid IL or missing references)
		//IL_0189: Unknown result type (might be due to invalid IL or missing references)
		//IL_0193: Unknown result type (might be due to invalid IL or missing references)
		//IL_0195: Unknown result type (might be due to invalid IL or missing references)
		//IL_0197: Unknown result type (might be due to invalid IL or missing references)
		//IL_01a3: Unknown result type (might be due to invalid IL or missing references)
		//IL_01a8: Unknown result type (might be due to invalid IL or missing references)
		//IL_01ac: Unknown result type (might be due to invalid IL or missing references)
		//IL_01b1: Unknown result type (might be due to invalid IL or missing references)
		//IL_01c1: Unknown result type (might be due to invalid IL or missing references)
		//IL_01c6: Unknown result type (might be due to invalid IL or missing references)
		//IL_01d9: Unknown result type (might be due to invalid IL or missing references)
		//IL_01de: Unknown result type (might be due to invalid IL or missing references)
		//IL_01ee: Unknown result type (might be due to invalid IL or missing references)
		//IL_01f3: Unknown result type (might be due to invalid IL or missing references)
		//IL_0471: Unknown result type (might be due to invalid IL or missing references)
		//IL_0476: Unknown result type (might be due to invalid IL or missing references)
		//IL_0484: Unknown result type (might be due to invalid IL or missing references)
		//IL_0489: Unknown result type (might be due to invalid IL or missing references)
		//IL_0494: Unknown result type (might be due to invalid IL or missing references)
		//IL_0499: Unknown result type (might be due to invalid IL or missing references)
		//IL_04b3: Unknown result type (might be due to invalid IL or missing references)
		//IL_04b8: Unknown result type (might be due to invalid IL or missing references)
		//IL_04c3: Unknown result type (might be due to invalid IL or missing references)
		//IL_04c8: Unknown result type (might be due to invalid IL or missing references)
		//IL_0730: Unknown result type (might be due to invalid IL or missing references)
		//IL_0735: Unknown result type (might be due to invalid IL or missing references)
		//IL_0740: Unknown result type (might be due to invalid IL or missing references)
		//IL_0745: Unknown result type (might be due to invalid IL or missing references)
		//IL_0750: Unknown result type (might be due to invalid IL or missing references)
		//IL_0755: Unknown result type (might be due to invalid IL or missing references)
		//IL_075e: Unknown result type (might be due to invalid IL or missing references)
		//IL_076e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0773: Unknown result type (might be due to invalid IL or missing references)
		//IL_0778: Unknown result type (might be due to invalid IL or missing references)
		//IL_077d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0782: Unknown result type (might be due to invalid IL or missing references)
		//IL_0717: Unknown result type (might be due to invalid IL or missing references)
		//IL_0897: Unknown result type (might be due to invalid IL or missing references)
		//IL_089c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0786: Unknown result type (might be due to invalid IL or missing references)
		//IL_0725: Unknown result type (might be due to invalid IL or missing references)
		//IL_072a: Unknown result type (might be due to invalid IL or missing references)
		//IL_08a9: Unknown result type (might be due to invalid IL or missing references)
		//IL_08ae: Unknown result type (might be due to invalid IL or missing references)
		//IL_096c: Unknown result type (might be due to invalid IL or missing references)
		//IL_091d: Unknown result type (might be due to invalid IL or missing references)
		//IL_08ce: Unknown result type (might be due to invalid IL or missing references)
		//IL_08d3: Unknown result type (might be due to invalid IL or missing references)
		//IL_08f5: Unknown result type (might be due to invalid IL or missing references)
		//IL_08fa: Unknown result type (might be due to invalid IL or missing references)
		//IL_09aa: Unknown result type (might be due to invalid IL or missing references)
		//IL_0a34: Unknown result type (might be due to invalid IL or missing references)
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
		Transform val = Transform.Rotation(cSClusterParam.InRotation, Vector3d.ZAxis, Point3d.Origin);
		double num3 = cSClusterParam.InProfileWidth - (cSClusterParam.InChannelWallThicknessInner + cSClusterParam.InChannelWallThicknessOuter);
		double num4 = num3 - cSClusterParam.InChannelSpacing * 2.0;
		double bestGemSizeX = -1.23432101234321E+308;
		double num5 = -1.23432101234321E+308;
		CSGemData inGemDefinition = cSClusterParam.InGemDefinition;
		Point3d val3;
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
					double num9 = (num7 + num8) / 2.0;
					Transform val2 = Transform.Scale(Plane.WorldXY, num7, num8, num9);
					NurbsCurve nurbsCurve = inGemDefinition.GetNurbsCurve();
					((GeometryBase)nurbsCurve).Transform(val * val2);
					BoundingBox boundingBox = ((GeometryBase)nurbsCurve).GetBoundingBox(true);
					val3 = ((BoundingBox)(ref boundingBox)).Max;
					double num10 = Math.Abs(((Point3d)(ref val3)).X);
					val3 = ((BoundingBox)(ref boundingBox)).Min;
					double num11 = num10 + Math.Abs(((Point3d)(ref val3)).X);
					val3 = ((BoundingBox)(ref boundingBox)).Max;
					double num12 = Math.Abs(((Point3d)(ref val3)).Y);
					val3 = ((BoundingBox)(ref boundingBox)).Min;
					double num13 = num12 + Math.Abs(((Point3d)(ref val3)).Y);
					double num14 = Math.Abs(num4 - num13);
					if (!RhinoMath.IsValidDouble(num6) || num14 < num6)
					{
						gemSize = size;
						bestGemSizeX = num11;
						num5 = num13;
						num6 = num14;
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
		double num15 = 0.0;
		if (cSClusterParam.InUseMinCuletDistance == ValueListItem.MinCuletGrowUp)
		{
			if (inGemDefinition.GemZSize > cSClusterParam.InProfileHeight - cSClusterParam.InMinCuletDistance)
			{
				num15 = inGemDefinition.GemZSize - (cSClusterParam.InProfileHeight - cSClusterParam.InMinCuletDistance);
			}
			number = num15;
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
		double num16 = inGemDefinition.GemXSize / 2.0 - inGemDefinition.GemXMax;
		double num17 = inGemDefinition.GemYSize / 2.0 - inGemDefinition.GemYMax;
		Transform val4 = Transform.Translation(num16, num17, 0.0);
		Transform val5 = Transform.Translation(0.0, 0.0, 0.0 - Math.Abs(inGemDefinition.GemZMax));
		Transform val6 = Transform.Rotation(cSClusterParam.InAngle, Vector3d.XAxis, Point3d.Origin);
		Transform xTransformLocal = Transform.Translation(0.0, 0.0, num2 + num15) * val6 * val5 * val4 * val;
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
				val3 = ((BoundingBox)(ref boundingBox2)).Max;
				double x = ((Point3d)(ref val3)).X;
				val3 = ((BoundingBox)(ref boundingBox2)).Min;
				double num18 = Math.Abs(x - ((Point3d)(ref val3)).X + cSClusterParam.InOffsetX);
				val3 = ((BoundingBox)(ref boundingBox2)).Max;
				double y = ((Point3d)(ref val3)).Y;
				val3 = ((BoundingBox)(ref boundingBox2)).Min;
				double num19 = Math.Abs(y - ((Point3d)(ref val3)).Y + cSClusterParam.InOffsetY);
				double num20;
				double num21;
				if (cSGemData != null)
				{
					switch (cSClusterParam.InCenterGemShapeOverrideMode)
					{
					case ValueListItem.GemClusterCenterShapeOverrideModeDimensionX:
						num20 = (cSGemData.GemXSize + cSClusterParam.InOffsetX) / cSGemData2.GemXSize;
						num21 = num20;
						break;
					case ValueListItem.GemClusterCenterShapeOverrideModeDimensionY:
						num21 = (cSGemData.GemYSize + cSClusterParam.InOffsetY) / cSGemData2.GemYSize;
						num20 = num21;
						break;
					case ValueListItem.GemClusterCenterShapeOverrideModeDimensionLargest:
						if (cSGemData.GemXSize > cSGemData.GemYSize)
						{
							num20 = (cSGemData.GemXSize + cSClusterParam.InOffsetX) / cSGemData2.GemXSize;
							num21 = num20;
						}
						else
						{
							num21 = (cSGemData.GemYSize + cSClusterParam.InOffsetY) / cSGemData2.GemYSize;
							num20 = num21;
						}
						break;
					case ValueListItem.GemClusterCenterShapeOverrideModeDimensionSmallest:
						if (cSGemData.GemXSize < cSGemData.GemYSize)
						{
							num20 = (cSGemData.GemXSize + cSClusterParam.InOffsetX) / cSGemData2.GemXSize;
							num21 = num20;
						}
						else
						{
							num21 = (cSGemData.GemYSize + cSClusterParam.InOffsetY) / cSGemData2.GemYSize;
							num20 = num21;
						}
						break;
					case ValueListItem.GemClusterCenterShapeOverrideModeDimensionXandY:
						num20 = (cSGemData.GemXSize + cSClusterParam.InOffsetX) / cSGemData2.GemXSize;
						num21 = (cSGemData.GemYSize + cSClusterParam.InOffsetY) / cSGemData2.GemYSize;
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
						num20 = num18 / cSGemData2.GemXSize;
						num21 = num20;
						break;
					case ValueListItem.GemClusterCenterShapeOverrideModeDimensionY:
						num21 = num19 / cSGemData2.GemYSize;
						num20 = num21;
						break;
					case ValueListItem.GemClusterCenterShapeOverrideModeDimensionLargest:
						if (num18 > num19)
						{
							num20 = num18 / cSGemData2.GemXSize;
							num21 = num20;
						}
						else
						{
							num21 = num19 / cSGemData2.GemYSize;
							num20 = num21;
						}
						break;
					case ValueListItem.GemClusterCenterShapeOverrideModeDimensionSmallest:
						if (num18 < num19)
						{
							num20 = num18 / cSGemData2.GemXSize;
							num21 = num20;
						}
						else
						{
							num21 = num19 / cSGemData2.GemYSize;
							num20 = num21;
						}
						break;
					case ValueListItem.GemClusterCenterShapeOverrideModeDimensionXandY:
						num20 = num18 / cSGemData2.GemXSize;
						num21 = num19 / cSGemData2.GemYSize;
						break;
					default:
						return UpdateError("Invalid Gem Cluster Center Shape Override Mode");
					}
				}
				cSGemData2.Scale(num20, num21);
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
					val3 = ((BoundingBox)(ref boundingBox2)).Center;
					double x2 = ((Point3d)(ref val3)).X;
					val3 = ((BoundingBox)(ref boundingBox2)).Center;
					double y2 = ((Point3d)(ref val3)).Y;
					val3 = ((BoundingBox)(ref boundingBox2)).Center;
					xUp = Transform.Translation(x2, y2, ((Point3d)(ref val3)).Z) * Transform.Rotation(RhinoMath.ToRadians(cSClusterParam.InRotate), Vector3d.YAxis, Point3d.Origin);
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
		double num22;
		if (Math.Abs(num3) < 0.001)
		{
			num22 = num + num5 / 2.0 - cSClusterParam.InGemHeightOffset * Math.Sin(cSClusterParam.InAngle);
			offsetRailZ = 0.0 + cSClusterParam.InGemHeightOffset * Math.Cos(cSClusterParam.InAngle);
		}
		else
		{
			num22 = num + (cSClusterParam.InChannelWallThicknessInner + num3 / 2.0) * Math.Cos(cSClusterParam.InAngle) - cSClusterParam.InGemHeightOffset * Math.Sin(cSClusterParam.InAngle);
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
			double num23 = cSGemData3.GemXSize / 2.0;
			val3 = cSClusterParam.InEmeraldCornerPoint1;
			optionalEmeraldCornerXPercent = ((Point3d)(ref val3)).X / num23;
			double num24 = cSGemData3.GemYSize / 2.0;
			val3 = cSClusterParam.InEmeraldCornerPoint2;
			optionalEmeraldCornerYPercent = ((Point3d)(ref val3)).Y / num24;
		}
		CSDataArray outputGems;
		double number2;
		if (!cSClusterParam.InFixedGemSpacing)
		{
			if (!GetGemLayout(cSClusterParam, gemShape, cSGemData3, inGemDefinition, bestGemSizeX, num22, offsetRailZ, xTransformLocal, optionalEmeraldCornerXPercent, optionalEmeraldCornerYPercent, out outputGems, out var outError))
			{
				return UpdateError(outError);
			}
			number2 = 0.0;
		}
		else
		{
			double num25 = num22;
			num22 -= 0.1;
			bool flag4 = false;
			bool flag5 = true;
			if (!GetGemLayout(cSClusterParam, gemShape, cSGemData3, inGemDefinition, bestGemSizeX, num22, offsetRailZ, xTransformLocal, optionalEmeraldCornerXPercent, optionalEmeraldCornerYPercent, out outputGems, out var outError2))
			{
				return UpdateError(outError2);
			}
			double num26 = GetSpacing(outputGems);
			num22 += 0.1;
			CSDataArray cSDataArray;
			while (true)
			{
				if (!GetGemLayout(cSClusterParam, gemShape, cSGemData3, inGemDefinition, bestGemSizeX, num22, offsetRailZ, xTransformLocal, optionalEmeraldCornerXPercent, optionalEmeraldCornerYPercent, out outputGems, out outError2))
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
				if (spacing < num26)
				{
					cSDataArray = outputGems;
					num22 -= 0.01;
					num26 = spacing;
					break;
				}
				num22 += 0.1;
				num26 = spacing;
			}
			if (!flag4)
			{
				while (true)
				{
					if (!GetGemLayout(cSClusterParam, gemShape, cSGemData3, inGemDefinition, bestGemSizeX, num22, offsetRailZ, xTransformLocal, optionalEmeraldCornerXPercent, optionalEmeraldCornerYPercent, out outputGems, out outError2))
					{
						return UpdateError(outError2);
					}
					double spacing2 = GetSpacing(outputGems);
					if (spacing2 > num26)
					{
						break;
					}
					cSDataArray = outputGems;
					num22 -= 0.01;
					num26 = spacing2;
				}
			}
			outputGems = cSDataArray;
			number2 = num22 - num25;
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
		//IL_0620: Unknown result type (might be due to invalid IL or missing references)
		//IL_0537: Unknown result type (might be due to invalid IL or missing references)
		//IL_050c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0436: Unknown result type (might be due to invalid IL or missing references)
		//IL_0650: Unknown result type (might be due to invalid IL or missing references)
		//IL_049f: Unknown result type (might be due to invalid IL or missing references)
		//IL_05c0: Unknown result type (might be due to invalid IL or missing references)
		//IL_04d8: Unknown result type (might be due to invalid IL or missing references)
		//IL_0593: Unknown result type (might be due to invalid IL or missing references)
		//IL_0680: Unknown result type (might be due to invalid IL or missing references)
		//IL_046a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0565: Unknown result type (might be due to invalid IL or missing references)
		//IL_05ed: Unknown result type (might be due to invalid IL or missing references)
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
		//IL_0060: Unknown result type (might be due to invalid IL or missing references)
		//IL_0061: Unknown result type (might be due to invalid IL or missing references)
		//IL_0062: Unknown result type (might be due to invalid IL or missing references)
		//IL_0067: Unknown result type (might be due to invalid IL or missing references)
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
		Vector3d val = value - value2;
		return ((Vector3d)(ref val)).Length - num - num2;
	}

	private static bool PlaceGemsCalf(string centerGemShape, CSGemData centerGem, CSGemData gemDefinition, double gemDefinitionSizeX, double spacing, double offsetRailX, double offsetRailZ, Transform xTransformLocal, int style, double angle, ref CSDataArray outputGems)
	{
		//IL_003c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0043: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f4: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d5: Unknown result type (might be due to invalid IL or missing references)
		//IL_0150: Unknown result type (might be due to invalid IL or missing references)
		//IL_017a: Unknown result type (might be due to invalid IL or missing references)
		//IL_01a5: Unknown result type (might be due to invalid IL or missing references)
		//IL_01d0: Unknown result type (might be due to invalid IL or missing references)
		//IL_01fb: Unknown result type (might be due to invalid IL or missing references)
		Curve[] array = GemOffsetCurve.GemOffsetCurves(centerGemShape, centerGem, offsetRailX, doTransformUp: true);
		if (array.Length != 7)
		{
			return false;
		}
		Curve[] array2 = array;
		for (int i = 0; i < array2.Length; i++)
		{
			((GeometryBase)array2[i]).Translate(centerGem.Normal * offsetRailZ);
		}
		Curve val = array[0];
		Curve val2 = array[1];
		Curve val3 = array[2];
		Curve val4 = array[3];
		Curve val5 = array[4];
		Curve val6 = array[5];
		Curve val7 = array[6];
		if (!CSUtilCurves.MergeCurves((IEnumerable<Curve>)(object)new Curve[2] { val, val7 }, out Curve mergedCrv, 0.01))
		{
			return false;
		}
		CSDataArray cSDataArray = new CSDataArray();
		if (style == 0)
		{
			cSDataArray = new CSDataArray();
			if (!CSClusterGemUtils.PlaceGemCorners((IList<Curve>)(object)new Curve[6] { mergedCrv, val2, val3, val4, val5, val6 }, centerGem, gemDefinition, xTransformLocal, cSDataArray, angle))
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
		if (!CSClusterGemUtils.PlaceGemsCurveSide(val2, centerGem, gemDefinition, gemDefinitionSizeX, spacing, style, xTransformLocal, outputGems))
		{
			return false;
		}
		if (style == 0)
		{
			outputGems.Append(cSDataArray.GetArrayItem(1));
		}
		if (!CSClusterGemUtils.PlaceGemsCurveSide(val3, centerGem, gemDefinition, gemDefinitionSizeX, spacing, style, xTransformLocal, outputGems))
		{
			return false;
		}
		if (style == 0)
		{
			outputGems.Append(cSDataArray.GetArrayItem(2));
		}
		if (!CSClusterGemUtils.PlaceGemsCurveSide(val4, centerGem, gemDefinition, gemDefinitionSizeX, spacing, style, xTransformLocal, outputGems))
		{
			return false;
		}
		if (style == 0)
		{
			outputGems.Append(cSDataArray.GetArrayItem(3));
		}
		if (!CSClusterGemUtils.PlaceGemsCurveSide(val5, centerGem, gemDefinition, gemDefinitionSizeX, spacing, style, xTransformLocal, outputGems))
		{
			return false;
		}
		if (style == 0)
		{
			outputGems.Append(cSDataArray.GetArrayItem(4));
		}
		if (!CSClusterGemUtils.PlaceGemsCurveSide(val6, centerGem, gemDefinition, gemDefinitionSizeX, spacing, style, xTransformLocal, outputGems))
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
		//IL_003c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0043: Unknown result type (might be due to invalid IL or missing references)
		//IL_00cd: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b4: Unknown result type (might be due to invalid IL or missing references)
		//IL_00fd: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e4: Unknown result type (might be due to invalid IL or missing references)
		Curve[] array = GemOffsetCurve.GemOffsetCurves(centerGemShape, centerGem, offsetRailX, doTransformUp: true);
		if (array.Length != 4)
		{
			return false;
		}
		Curve[] array2 = array;
		for (int i = 0; i < array2.Length; i++)
		{
			((GeometryBase)array2[i]).Translate(centerGem.Normal * offsetRailZ);
		}
		Curve val = array[0];
		Curve val2 = array[1];
		Curve val3 = array[2];
		Curve val4 = array[3];
		CSUtilCurves.MergeCurves((IEnumerable<Curve>)(object)new Curve[2] { val, val4 }, out Curve mergedCrv, 0.01);
		CSUtilCurves.MergeCurves((IEnumerable<Curve>)(object)new Curve[2] { val2, val3 }, out Curve mergedCrv2, 0.01);
		if (style == 0 && !CSClusterGemUtils.PlaceGemCorner(val, val2, centerGem, gemDefinition, xTransformLocal, angle, outputGems))
		{
			return false;
		}
		if (!CSClusterGemUtils.PlaceGemsCurveSide(mergedCrv, centerGem, gemDefinition, gemDefinitionSizeX, spacing, style, xTransformLocal, outputGems))
		{
			return false;
		}
		if (style == 0 && !CSClusterGemUtils.PlaceGemCorner(val3, val4, centerGem, gemDefinition, xTransformLocal, angle, outputGems))
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
		//IL_003c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0043: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ca: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ee: Unknown result type (might be due to invalid IL or missing references)
		//IL_0112: Unknown result type (might be due to invalid IL or missing references)
		Curve[] array = GemOffsetCurve.GemOffsetCurves(centerGemShape, centerGem, offsetRailX, doTransformUp: true);
		if (array.Length != 6)
		{
			return false;
		}
		Curve[] array2 = array;
		for (int i = 0; i < array2.Length; i++)
		{
			((GeometryBase)array2[i]).Translate(centerGem.Normal * offsetRailZ);
		}
		if (!CSUtilCurves.MergeCurves((IEnumerable<Curve>)(object)new Curve[3]
		{
			array[0],
			array[1],
			array[2]
		}, out Curve mergedCrv, 0.01))
		{
			return false;
		}
		if (!CSUtilCurves.MergeCurves((IEnumerable<Curve>)(object)new Curve[3]
		{
			array[3],
			array[4],
			array[5]
		}, out Curve mergedCrv2, 0.01))
		{
			return false;
		}
		CSDataArray cSDataArray = new CSDataArray();
		if (!CSClusterGemUtils.PlaceGemCorners((IList<Curve>)(object)new Curve[2] { mergedCrv2, mergedCrv }, centerGem, gemDefinition, xTransformLocal, cSDataArray, angle))
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
		//IL_003c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0043: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d3: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f1: Unknown result type (might be due to invalid IL or missing references)
		//IL_0149: Unknown result type (might be due to invalid IL or missing references)
		Curve[] array = GemOffsetCurve.GemOffsetCurves(centerGemShape, centerGem, offsetRailX, doTransformUp: true);
		if (array.Length != 4)
		{
			return false;
		}
		Curve[] array2 = array;
		for (int i = 0; i < array2.Length; i++)
		{
			((GeometryBase)array2[i]).Translate(centerGem.Normal * offsetRailZ);
		}
		Curve val = array[0];
		Curve val2 = array[1];
		Curve val3 = array[2];
		Curve val4 = array[3];
		Curve[] inputCrvs = (Curve[])(object)new Curve[2] { val4, val };
		Curve[] inputCrvs2 = (Curve[])(object)new Curve[2] { val2, val3 };
		if (!CSUtilCurves.MergeCurves((IEnumerable<Curve>)inputCrvs, out Curve mergedCrv, 0.01))
		{
			return false;
		}
		if (!CSUtilCurves.MergeCurves((IEnumerable<Curve>)inputCrvs2, out Curve mergedCrv2, 0.01))
		{
			return false;
		}
		CSDataArray cSDataArray = new CSDataArray();
		if (!CSClusterGemUtils.PlaceGemCorners((IList<Curve>)(object)new Curve[2] { mergedCrv, mergedCrv2 }, centerGem, gemDefinition, xTransformLocal, cSDataArray, angle))
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
		//IL_0038: Unknown result type (might be due to invalid IL or missing references)
		//IL_003f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0096: Unknown result type (might be due to invalid IL or missing references)
		//IL_007e: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c1: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ec: Unknown result type (might be due to invalid IL or missing references)
		//IL_0117: Unknown result type (might be due to invalid IL or missing references)
		Curve[] array = GemOffsetCurve.GemOffsetCurves(centerGemShape, centerGem, offsetRailX, doTransformUp: true);
		if (array.Length != 4)
		{
			return false;
		}
		Curve[] array2 = array;
		for (int i = 0; i < array2.Length; i++)
		{
			((GeometryBase)array2[i]).Translate(centerGem.Normal * offsetRailZ);
		}
		CSDataArray cSDataArray = new CSDataArray();
		if (style == 0 && !CSClusterGemUtils.PlaceGemCorners((IList<Curve>)(object)new Curve[4]
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
		//IL_003c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0043: Unknown result type (might be due to invalid IL or missing references)
		//IL_00cf: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b0: Unknown result type (might be due to invalid IL or missing references)
		//IL_012b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0155: Unknown result type (might be due to invalid IL or missing references)
		Curve[] array = GemOffsetCurve.GemOffsetCurves(centerGemShape, centerGem, offsetRailX, doTransformUp: true);
		if (array.Length != 4)
		{
			return false;
		}
		Curve[] array2 = array;
		for (int i = 0; i < array2.Length; i++)
		{
			((GeometryBase)array2[i]).Translate(centerGem.Normal * offsetRailZ);
		}
		Curve val = array[0];
		Curve val2 = array[1];
		Curve val3 = array[2];
		Curve val4 = array[3];
		if (!CSUtilCurves.MergeCurves((IEnumerable<Curve>)(object)new Curve[2] { val, val4 }, out Curve mergedCrv, 0.01))
		{
			return false;
		}
		CSDataArray cSDataArray = new CSDataArray();
		if (style == 0 && !CSClusterGemUtils.PlaceGemCorners((IList<Curve>)(object)new Curve[3] { mergedCrv, val2, val3 }, centerGem, gemDefinition, xTransformLocal, cSDataArray, angle))
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
		if (!CSClusterGemUtils.PlaceGemsCurveSide(val2, centerGem, gemDefinition, gemDefinitionSizeX, spacing, style, xTransformLocal, outputGems))
		{
			return false;
		}
		if (style == 0)
		{
			outputGems.Append(cSDataArray.GetArrayItem(1));
		}
		if (!CSClusterGemUtils.PlaceGemsCurveSide(val3, centerGem, gemDefinition, gemDefinitionSizeX, spacing, style, xTransformLocal, outputGems))
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
		//IL_003d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0044: Unknown result type (might be due to invalid IL or missing references)
		//IL_016d: Unknown result type (might be due to invalid IL or missing references)
		//IL_014e: Unknown result type (might be due to invalid IL or missing references)
		//IL_01ca: Unknown result type (might be due to invalid IL or missing references)
		//IL_01f5: Unknown result type (might be due to invalid IL or missing references)
		Curve[] array = GemOffsetCurve.GemOffsetCurves(centerGemShape, centerGem, offsetRailX, doTransformUp: true);
		if (array.Length != 12)
		{
			return false;
		}
		Curve[] array2 = array;
		for (int i = 0; i < array2.Length; i++)
		{
			((GeometryBase)array2[i]).Translate(centerGem.Normal * offsetRailZ);
		}
		Curve val = array[0];
		Curve val2 = array[1];
		Curve val3 = array[2];
		Curve val4 = array[3];
		Curve val5 = array[4];
		Curve val6 = array[5];
		Curve val7 = array[6];
		Curve val8 = array[7];
		Curve val9 = array[8];
		Curve val10 = array[9];
		Curve val11 = array[10];
		Curve val12 = array[11];
		Curve[] inputCrvs = (Curve[])(object)new Curve[4] { val, val12, val2, val11 };
		Curve[] inputCrvs2 = (Curve[])(object)new Curve[4] { val3, val4, val5, val6 };
		Curve[] inputCrvs3 = (Curve[])(object)new Curve[4] { val7, val8, val9, val10 };
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
		if (style == 0 && !CSClusterGemUtils.PlaceGemCorners((IList<Curve>)(object)new Curve[3] { mergedCrv, mergedCrv2, mergedCrv3 }, centerGem, gemDefinition, xTransformLocal, cSDataArray, angle))
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
