using System;
using System.Collections.Generic;
using CS;
using CS.CSDataUtilities;
using CS.CSObjects.CSBlocks;
using CS.Parametric;
using CS.Parametric.Channels;
using CSBlockDataUtils;
using CSBlockDataUtils.CSGemOffsetCurves;
using CSBlockUtil;
using CSDatas;
using CSDatas.Interfaces;
using CSGemInstance.GemTableData;
using GV.CounterSketch.Shared;
using Rhino;
using Rhino.Geometry;

namespace CSBlocks.GemClusters.Obsolete;

[Obsolete]
[CSBlock("Cluster_V11", "{0C5373B8-CE0A-4E1F-B199-E148C334ED7A}")]
public sealed class CSCluster11 : CSBlock
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

	public readonly CSInputChannel InProngCornerType;

	public readonly CSInputChannel InProngCornerSizing;

	public readonly CSOutputChannel OutGem;

	public readonly CSOutputChannel OutGemSizeZ;

	public readonly CSOutputChannel OutChannelFix;

	public readonly CSOutputChannel OutDisplayGem;

	public readonly CSOutputChannel OutMinCuletUp;

	public readonly CSOutputChannel OutMinCuletSmallerFlag;

	public CSCluster11(string identifier)
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
		InProngCornerType = new CSInputChannel(this, ChannelDataType.Integer, ChannelArrayType.Array, new List<int>());
		InProngCornerSizing = new CSInputChannel(this, ChannelDataType.Double, ChannelArrayType.Array, new List<double>());
		OutGem = new CSOutputChannel(this, ChannelDataType.Gem, ChannelArrayType.Array);
		OutDisplayGem = new CSOutputChannel(this, ChannelDataType.Gem, ChannelArrayType.Array);
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
		CSClusterParam_V1 cSClusterParam_V = new CSClusterParam_V1(this);
		bool flag = cSClusterParam_V.InShowChannel || cSClusterParam_V.InForceGems;
		CSDataArray inCenterObject = cSClusterParam_V.InCenterObject;
		if (inCenterObject.GetArrayCount() <= 0)
		{
			return UpdateMissing("CenterGem");
		}
		CSGemData cSGemData = inCenterObject.GetArrayItem(0) as CSGemData;
		CSData arrayItem = inCenterObject.GetArrayItem(0);
		bool flag2 = cSClusterParam_V.InCenterGemShapeOverrideEnabled || cSGemData == null;
		double num = cSClusterParam_V.InCenterGemSpacing + cSClusterParam_V.InRailX;
		double num2 = cSClusterParam_V.InPlacement + cSClusterParam_V.InRailZ;
		double number = 0.0;
		bool flag3 = false;
		Transform val = Transform.Rotation(cSClusterParam_V.InRotation, Vector3d.ZAxis, Point3d.Origin);
		double num3 = cSClusterParam_V.InProfileWidth - (cSClusterParam_V.InChannelWallThicknessInner + cSClusterParam_V.InChannelWallThicknessOuter);
		double num4 = num3 - cSClusterParam_V.InChannelSpacing * 2.0;
		double bestGemSizeX = -1.23432101234321E+308;
		double num5 = -1.23432101234321E+308;
		CSGemData inGemDefinition = cSClusterParam_V.InGemDefinition;
		Point3d val3;
		if (!cSClusterParam_V.InGemSizeToggle)
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
			inGemDefinition.ScaleToSize(cSClusterParam_V.InGemSize.GemXSize, cSClusterParam_V.InGemSize.GemYSize);
			num5 = cSClusterParam_V.InGemSize.GemYSize;
			bestGemSizeX = cSClusterParam_V.InGemSize.GemXSize;
			num3 = cSClusterParam_V.InGemSize.GemXSize + cSClusterParam_V.InChannelSpacing;
		}
		double num15 = 0.0;
		if (cSClusterParam_V.InUseMinCuletDistance == ValueListItem.MinCuletGrowUp)
		{
			if (inGemDefinition.GemZSize > cSClusterParam_V.InProfileHeight - cSClusterParam_V.InMinCuletDistance)
			{
				num15 = inGemDefinition.GemZSize - (cSClusterParam_V.InProfileHeight - cSClusterParam_V.InMinCuletDistance);
			}
			number = num15;
		}
		if (cSClusterParam_V.InUseMinCuletDistance == ValueListItem.MinCuletGrowSmaller)
		{
			double gemXSize = inGemDefinition.GemXSize;
			double gemYSize = inGemDefinition.GemYSize;
			CSUtilGem.GetGemScaleFromTableWithMaxZ(inGemDefinition, inGemDefinition.GemXSize, inGemDefinition.GemYSize, cSClusterParam_V.InProfileHeight - cSClusterParam_V.InMinCuletDistance, useLargestPossible: true, useIterationOnly: false, out var xBestScale);
			inGemDefinition.ApplyTransform(xBestScale);
			if (gemXSize - inGemDefinition.GemXSize > 0.1 || gemYSize - inGemDefinition.GemYSize > 0.1)
			{
				bestGemSizeX = inGemDefinition.GemXSize;
				num5 = inGemDefinition.GemYSize;
				num3 = inGemDefinition.GemXSize + cSClusterParam_V.InChannelSpacing;
				flag3 = true;
			}
		}
		double num16 = inGemDefinition.GemXSize / 2.0 - inGemDefinition.GemXMax;
		double num17 = inGemDefinition.GemYSize / 2.0 - inGemDefinition.GemYMax;
		Transform val4 = Transform.Translation(num16, num17, 0.0);
		Transform val5 = Transform.Translation(0.0, 0.0, 0.0 - Math.Abs(inGemDefinition.GemZMax));
		Transform val6 = Transform.Rotation(cSClusterParam_V.InAngle, Vector3d.XAxis, Point3d.Origin);
		Transform xTransformLocal = Transform.Translation(0.0, 0.0, num2 + num15) * val6 * val5 * val4 * val;
		CSGemData cSGemData2 = cSClusterParam_V.InCenterGemShapeOverride;
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
				double num18 = Math.Abs(x - ((Point3d)(ref val3)).X + cSClusterParam_V.InOffsetX);
				val3 = ((BoundingBox)(ref boundingBox2)).Max;
				double y = ((Point3d)(ref val3)).Y;
				val3 = ((BoundingBox)(ref boundingBox2)).Min;
				double num19 = Math.Abs(y - ((Point3d)(ref val3)).Y + cSClusterParam_V.InOffsetY);
				double num20;
				double num21;
				if (cSGemData != null)
				{
					switch (cSClusterParam_V.InCenterGemShapeOverrideMode)
					{
					case ValueListItem.GemClusterCenterShapeOverrideModeDimensionX:
						num20 = (cSGemData.GemXSize + cSClusterParam_V.InOffsetX) / cSGemData2.GemXSize;
						num21 = num20;
						break;
					case ValueListItem.GemClusterCenterShapeOverrideModeDimensionY:
						num21 = (cSGemData.GemYSize + cSClusterParam_V.InOffsetY) / cSGemData2.GemYSize;
						num20 = num21;
						break;
					case ValueListItem.GemClusterCenterShapeOverrideModeDimensionLargest:
						if (cSGemData.GemXSize > cSGemData.GemYSize)
						{
							num20 = (cSGemData.GemXSize + cSClusterParam_V.InOffsetX) / cSGemData2.GemXSize;
							num21 = num20;
						}
						else
						{
							num21 = (cSGemData.GemYSize + cSClusterParam_V.InOffsetY) / cSGemData2.GemYSize;
							num20 = num21;
						}
						break;
					case ValueListItem.GemClusterCenterShapeOverrideModeDimensionSmallest:
						if (cSGemData.GemXSize < cSGemData.GemYSize)
						{
							num20 = (cSGemData.GemXSize + cSClusterParam_V.InOffsetX) / cSGemData2.GemXSize;
							num21 = num20;
						}
						else
						{
							num21 = (cSGemData.GemYSize + cSClusterParam_V.InOffsetY) / cSGemData2.GemYSize;
							num20 = num21;
						}
						break;
					case ValueListItem.GemClusterCenterShapeOverrideModeDimensionXandY:
						num20 = (cSGemData.GemXSize + cSClusterParam_V.InOffsetX) / cSGemData2.GemXSize;
						num21 = (cSGemData.GemYSize + cSClusterParam_V.InOffsetY) / cSGemData2.GemYSize;
						break;
					default:
						return UpdateError("Invalid Gem Cluster Center Shape Override Mode");
					}
				}
				else
				{
					switch (cSClusterParam_V.InCenterGemShapeOverrideMode)
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
					xUp = Transform.Translation(x2, y2, ((Point3d)(ref val3)).Z) * Transform.Rotation(RhinoMath.ToRadians(cSClusterParam_V.InRotate), Vector3d.YAxis, Point3d.Origin);
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
			num22 = num + num5 / 2.0 - cSClusterParam_V.InGemHeightOffset * Math.Sin(cSClusterParam_V.InAngle);
			offsetRailZ = 0.0 + cSClusterParam_V.InGemHeightOffset * Math.Cos(cSClusterParam_V.InAngle);
		}
		else
		{
			num22 = num + (cSClusterParam_V.InChannelWallThicknessInner + num3 / 2.0) * Math.Cos(cSClusterParam_V.InAngle) - cSClusterParam_V.InGemHeightOffset * Math.Sin(cSClusterParam_V.InAngle);
			offsetRailZ = (cSClusterParam_V.InChannelWallThicknessInner + num3 / 2.0) * Math.Sin(cSClusterParam_V.InAngle) + cSClusterParam_V.InGemHeightOffset * Math.Cos(cSClusterParam_V.InAngle);
		}
		CSGemData cSGemData3 = (cSClusterParam_V.InXYScaleToGem ? cSGemData : cSGemData2);
		if (cSGemData == null)
		{
			cSGemData3 = cSGemData2;
		}
		double optionalEmeraldCornerXPercent = 0.0;
		double optionalEmeraldCornerYPercent = 0.0;
		if (cSClusterParam_V.InEmeraldCornerPoint1 != Point3d.Unset && cSClusterParam_V.InEmeraldCornerPoint2 != Point3d.Unset)
		{
			double num23 = cSGemData3.GemXSize / 2.0;
			val3 = cSClusterParam_V.InEmeraldCornerPoint1;
			optionalEmeraldCornerXPercent = ((Point3d)(ref val3)).X / num23;
			double num24 = cSGemData3.GemYSize / 2.0;
			val3 = cSClusterParam_V.InEmeraldCornerPoint2;
			optionalEmeraldCornerYPercent = ((Point3d)(ref val3)).Y / num24;
		}
		CSDataArray outputGems;
		double number2;
		if (!cSClusterParam_V.InFixedGemSpacing)
		{
			if (!GetGemLayout(cSClusterParam_V, gemShape, cSGemData3, inGemDefinition, bestGemSizeX, num22, offsetRailZ, xTransformLocal, optionalEmeraldCornerXPercent, optionalEmeraldCornerYPercent, out outputGems, out var outError))
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
			if (!GetGemLayout(cSClusterParam_V, gemShape, cSGemData3, inGemDefinition, bestGemSizeX, num22, offsetRailZ, xTransformLocal, optionalEmeraldCornerXPercent, optionalEmeraldCornerYPercent, out outputGems, out var outError2))
			{
				return UpdateError(outError2);
			}
			double num26 = GetSpacing(outputGems);
			num22 += 0.1;
			CSDataArray cSDataArray;
			while (true)
			{
				if (!GetGemLayout(cSClusterParam_V, gemShape, cSGemData3, inGemDefinition, bestGemSizeX, num22, offsetRailZ, xTransformLocal, optionalEmeraldCornerXPercent, optionalEmeraldCornerYPercent, out outputGems, out outError2))
				{
					return UpdateError(outError2);
				}
				double spacing = GetSpacing(outputGems);
				if (flag5 && spacing - cSClusterParam_V.InSpacing < cSClusterParam_V.InGemSpacingTolerance)
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
					if (!GetGemLayout(cSClusterParam_V, gemShape, cSGemData3, inGemDefinition, bestGemSizeX, num22, offsetRailZ, xTransformLocal, optionalEmeraldCornerXPercent, optionalEmeraldCornerYPercent, out outputGems, out outError2))
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
			OutDisplayGem.SetData(null);
			OutGemSizeZ.SetData(null);
			OutGem.SetData(outputGems);
			OutMinCuletUp.SetData(null);
			OutMinCuletSmallerFlag.SetData(null);
		}
		else
		{
			OutGem.SetData(outputGems);
			OutDisplayGem.SetData(outputGems);
			OutGemSizeZ.SetData(new CSNumberData(cSClusterParam_V.InGemDefinition.GemZSize));
			OutChannelFix.SetData(new CSNumberData(number2));
			OutMinCuletUp.SetData(new CSNumberData(number));
			OutMinCuletSmallerFlag.SetData(new CSNumberData(flag3 ? 1 : 0));
		}
		return base.Update();
	}

	private static double CosAng(Vector3d v0, Vector3d v1)
	{
		((Vector3d)(ref v0)).Unitize();
		((Vector3d)(ref v1)).Unitize();
		return ((Vector3d)(ref v0)).X * ((Vector3d)(ref v1)).X + ((Vector3d)(ref v0)).Y * ((Vector3d)(ref v1)).Y;
	}

	private static bool GetGemLayout(CSClusterParam_V1 inCluster, string centerGemShape, CSGemData offsetControlShape, CSGemData gemDefinition, double bestGemSizeX, double offsetRailX, double offsetRailZ, Transform xTransformLocal, double optionalEmeraldCornerXPercent, double optionalEmeraldCornerYPercent, out CSDataArray outputGems, out string outError)
	{
		//IL_061a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0533: Unknown result type (might be due to invalid IL or missing references)
		//IL_0508: Unknown result type (might be due to invalid IL or missing references)
		//IL_0435: Unknown result type (might be due to invalid IL or missing references)
		//IL_064a: Unknown result type (might be due to invalid IL or missing references)
		//IL_049c: Unknown result type (might be due to invalid IL or missing references)
		//IL_05ba: Unknown result type (might be due to invalid IL or missing references)
		//IL_04d5: Unknown result type (might be due to invalid IL or missing references)
		//IL_058d: Unknown result type (might be due to invalid IL or missing references)
		//IL_067a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0468: Unknown result type (might be due to invalid IL or missing references)
		//IL_0560: Unknown result type (might be due to invalid IL or missing references)
		//IL_05e7: Unknown result type (might be due to invalid IL or missing references)
		outputGems = new CSDataArray();
		switch (centerGemShape)
		{
		case "Cushion":
		case "Cushion Rotated 90":
		case "Oval":
		case "Oval Rotated 90":
		case "Round":
			if (!PlaceGemsRound(centerGemShape, offsetControlShape, gemDefinition, bestGemSizeX, inCluster.InSpacing, offsetRailX, offsetRailZ, xTransformLocal, inCluster.InFixPosition, inCluster.InEven, ref outputGems))
			{
				outError = "PlaceGemsRound failed";
				return false;
			}
			break;
		case "Cushion Square":
			if (!PlaceGemsSquare(centerGemShape, offsetControlShape, gemDefinition, bestGemSizeX, inCluster.InSpacing, offsetRailX, offsetRailZ, xTransformLocal, 0, inCluster.InCornerBlend, inCluster.InAngle, ref outputGems))
			{
				outError = "PlaceGemsSquare failed";
				return false;
			}
			break;
		case "Princess":
		case "Baguette Straight":
		case "Baguette Tapered":
		case "BaguetteTapered":
			if (!PlaceGemsSquare(centerGemShape, offsetControlShape, gemDefinition, bestGemSizeX, inCluster.InSpacing, offsetRailX, offsetRailZ, xTransformLocal, inCluster.InStyle, inCluster.InCornerBlend, inCluster.InAngle, ref outputGems))
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
			if (!PlaceGemsEmerald(centerGemShape, offsetControlShape, gemDefinition, bestGemSizeX, inCluster.InSpacing, offsetRailX, offsetRailZ, xTransformLocal, optionalEmeraldCornerXPercent, optionalEmeraldCornerYPercent, ref outputGems))
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
			if (!PlaceGemsMarquise(centerGemShape, offsetControlShape, gemDefinition, bestGemSizeX, inCluster.InSpacing, offsetRailX, offsetRailZ, xTransformLocal, inCluster.InAngle, ref outputGems))
			{
				outError = "PlaceGemsMarquise failed";
				return false;
			}
			break;
		case "Pear":
			if (!PlaceGemsPear(centerGemShape, offsetControlShape, gemDefinition, bestGemSizeX, inCluster.InSpacing, offsetRailX, offsetRailZ, xTransformLocal, inCluster.InAngle, ref outputGems))
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

	private static double GetSide(double cosAng, double a, double c)
	{
		double num = -2.0 * a * cosAng;
		double num2 = 0.0 - c * c + a * a;
		return (0.0 - num + Math.Sqrt(num * num - 4.0 * num2)) / 2.0;
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

	private static double GetUnnormalizedParam(int gemCurrentIndex, int gemCount, int style)
	{
		if (style == 0)
		{
			return (double)gemCurrentIndex / (double)gemCount;
		}
		double num = 1.0 / (2.0 * Math.Sqrt(2.0) + (double)(2 * (gemCount - 1)));
		return num * Math.Sqrt(2.0) + num * 2.0 * (double)gemCurrentIndex;
	}

	private static bool PlaceGemCorner(Curve curveLhs, Curve curveRhs, CSGemData centerGem, CSData gemDefinition, Transform xTransformLocal, double angle, CSDataArray outputGems)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_0006: Unknown result type (might be due to invalid IL or missing references)
		//IL_0008: Unknown result type (might be due to invalid IL or missing references)
		//IL_000d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0010: Unknown result type (might be due to invalid IL or missing references)
		//IL_0023: Unknown result type (might be due to invalid IL or missing references)
		//IL_0024: Unknown result type (might be due to invalid IL or missing references)
		//IL_0025: Unknown result type (might be due to invalid IL or missing references)
		//IL_002a: Unknown result type (might be due to invalid IL or missing references)
		//IL_002b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0030: Unknown result type (might be due to invalid IL or missing references)
		//IL_0039: Unknown result type (might be due to invalid IL or missing references)
		//IL_003e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0041: Unknown result type (might be due to invalid IL or missing references)
		//IL_0046: Unknown result type (might be due to invalid IL or missing references)
		//IL_004f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0056: Unknown result type (might be due to invalid IL or missing references)
		//IL_005b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0064: Unknown result type (might be due to invalid IL or missing references)
		//IL_0069: Unknown result type (might be due to invalid IL or missing references)
		//IL_006a: Unknown result type (might be due to invalid IL or missing references)
		//IL_006b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0070: Unknown result type (might be due to invalid IL or missing references)
		//IL_0072: Unknown result type (might be due to invalid IL or missing references)
		//IL_0077: Unknown result type (might be due to invalid IL or missing references)
		//IL_007c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0081: Unknown result type (might be due to invalid IL or missing references)
		//IL_0082: Unknown result type (might be due to invalid IL or missing references)
		//IL_0085: Unknown result type (might be due to invalid IL or missing references)
		//IL_008a: Unknown result type (might be due to invalid IL or missing references)
		//IL_008f: Unknown result type (might be due to invalid IL or missing references)
		//IL_009a: Unknown result type (might be due to invalid IL or missing references)
		//IL_009f: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a9: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b2: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c0: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c5: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c7: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c9: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ce: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d2: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d4: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d6: Unknown result type (might be due to invalid IL or missing references)
		//IL_00db: Unknown result type (might be due to invalid IL or missing references)
		//IL_00dd: Unknown result type (might be due to invalid IL or missing references)
		Point3d pointAtEnd = curveLhs.PointAtEnd;
		Point3d pointAtStart = curveRhs.PointAtStart;
		if (((Point3d)(ref pointAtEnd)).DistanceTo(pointAtStart) > 0.001)
		{
			return false;
		}
		LineCurve val = new LineCurve(pointAtEnd, pointAtStart);
		Interval domain = ((Curve)val).Domain;
		Point3d destination = ((Curve)val).PointAt(((Interval)(ref domain)).Mid);
		domain = curveLhs.Domain;
		Vector3d val2 = curveLhs.TangentAt(((Interval)(ref domain)).Max);
		domain = curveRhs.Domain;
		Vector3d val3 = curveRhs.TangentAt(((Interval)(ref domain)).Min);
		Vector3d destination2 = val2 + val3;
		Transform val4 = CSBlockUtil.CSUtilMath.Orient3Point(Point3d.Origin, Vector3d.XAxis, Vector3d.ZAxis, destination, destination2, centerGem.Up);
		CSData data = gemDefinition.Duplicate();
		Vector3d val5 = centerGem.Up;
		((Vector3d)(ref val5)).Unitize();
		val5 = val5 * Math.Sin(angle) / 2.0;
		Transform val6 = Transform.Translation(val5);
		CSDataHelper.Transform(data, val4 * xTransformLocal * val6);
		outputGems.Append(data);
		return true;
	}

	private static bool PlaceGemCorners(IList<Curve> curves, CSGemData centerGem, CSData gemDefinition, Transform xTransformLocal, CSDataArray outputGems, double angle)
	{
		//IL_002c: Unknown result type (might be due to invalid IL or missing references)
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
			if (!PlaceGemCorners((IList<Curve>)(object)new Curve[6] { mergedCrv, val2, val3, val4, val5, val6 }, centerGem, gemDefinition, xTransformLocal, cSDataArray, angle))
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
		if (!PlaceGemsCurveSide(val5, centerGem, gemDefinition, gemDefinitionSizeX, spacing, style, xTransformLocal, outputGems))
		{
			return false;
		}
		if (style == 0)
		{
			outputGems.Append(cSDataArray.GetArrayItem(4));
		}
		if (!PlaceGemsCurveSide(val6, centerGem, gemDefinition, gemDefinitionSizeX, spacing, style, xTransformLocal, outputGems))
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

	private static bool PlaceGemsCurveSide(Curve curve, CSGemData centerGem, CSGemData gemDefinition, double gemDefinitionSizeX, double spacing, int style, Transform xTransformLocal, CSDataArray outputGems)
	{
		//IL_009a: Unknown result type (might be due to invalid IL or missing references)
		//IL_009f: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a5: Unknown result type (might be due to invalid IL or missing references)
		//IL_00af: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b9: Unknown result type (might be due to invalid IL or missing references)
		//IL_00bf: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c4: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c9: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d5: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d7: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d9: Unknown result type (might be due to invalid IL or missing references)
		double num = curve.GetLength();
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
		for (int i = 0; i < num2; i++)
		{
			array[i] = GetUnnormalizedParam(i, num2, style);
		}
		double[] array2 = curve.NormalizedLengthParameters(array, CSApplicationHelper.Application.Tolerance);
		for (int j = ((style == 0) ? 1 : 0); j < num2; j++)
		{
			Transform val = CSBlockUtil.CSUtilMath.Orient3Point(Point3d.Origin, Vector3d.XAxis, gemDefinition.Up, curve.PointAt(array2[j]), curve.TangentAt(array2[j]), centerGem.Up);
			CSData data = gemDefinition.Duplicate();
			CSDataHelper.Transform(data, val * xTransformLocal);
			outputGems.Append(data);
		}
		return true;
	}

	private static bool PlaceGemsEmerald(string centerGemShape, CSGemData centerGem, CSGemData gemDefinition, double gemDefinitionSizeX, double spacing, double offsetRailX, double offsetRailZ, Transform xTransformLocal, double optionalEmeraldCornerXPercent, double optionalEmeraldCornerYPercent, ref CSDataArray outputGems)
	{
		//IL_002f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0036: Unknown result type (might be due to invalid IL or missing references)
		//IL_0067: Unknown result type (might be due to invalid IL or missing references)
		//IL_006c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0076: Unknown result type (might be due to invalid IL or missing references)
		//IL_007b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0084: Unknown result type (might be due to invalid IL or missing references)
		//IL_0090: Unknown result type (might be due to invalid IL or missing references)
		//IL_0096: Unknown result type (might be due to invalid IL or missing references)
		//IL_009b: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a1: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a7: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ac: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b9: Unknown result type (might be due to invalid IL or missing references)
		//IL_00bf: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c4: Unknown result type (might be due to invalid IL or missing references)
		//IL_00cb: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d2: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d7: Unknown result type (might be due to invalid IL or missing references)
		//IL_02d8: Unknown result type (might be due to invalid IL or missing references)
		//IL_0300: Unknown result type (might be due to invalid IL or missing references)
		//IL_0328: Unknown result type (might be due to invalid IL or missing references)
		//IL_0350: Unknown result type (might be due to invalid IL or missing references)
		//IL_037c: Unknown result type (might be due to invalid IL or missing references)
		//IL_03a7: Unknown result type (might be due to invalid IL or missing references)
		//IL_03d1: Unknown result type (might be due to invalid IL or missing references)
		//IL_03f9: Unknown result type (might be due to invalid IL or missing references)
		//IL_042f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0464: Unknown result type (might be due to invalid IL or missing references)
		//IL_048f: Unknown result type (might be due to invalid IL or missing references)
		//IL_04ba: Unknown result type (might be due to invalid IL or missing references)
		Curve[] array = GemOffsetCurve.GemOffsetCurves(centerGemShape, centerGem, offsetRailX, doTransformUp: true, 0.0, optionalEmeraldCornerXPercent, optionalEmeraldCornerYPercent);
		if (array.Length != 9)
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
		Curve val4 = val3.DuplicateCurve();
		Interval domain = val4.Domain;
		double min = ((Interval)(ref domain)).Min;
		domain = val3.Domain;
		val4.Trim(new Interval(min, ((Interval)(ref domain)).Mid));
		double cosAng = CosAng(val.PointAtStart - val.PointAtEnd, val2.PointAtEnd - val2.PointAtStart);
		double cosAng2 = CosAng(val2.PointAtStart - val2.PointAtEnd, val4.PointAtEnd - val4.PointAtStart);
		double length = val.GetLength();
		double length2 = val2.GetLength();
		double length3 = val4.GetLength();
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

	private static void PlaceGemsEmeraldSide(IList<Curve> curves, int crvIndex, double sizeX, double distance, CSGemData centerGem, CSGemData gemDefinition, Transform xOrientLocal, ref CSDataArray outputGems)
	{
		//IL_0007: Unknown result type (might be due to invalid IL or missing references)
		//IL_000c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0014: Unknown result type (might be due to invalid IL or missing references)
		//IL_0019: Unknown result type (might be due to invalid IL or missing references)
		//IL_001a: Unknown result type (might be due to invalid IL or missing references)
		//IL_001f: Unknown result type (might be due to invalid IL or missing references)
		//IL_002a: Unknown result type (might be due to invalid IL or missing references)
		//IL_015e: Unknown result type (might be due to invalid IL or missing references)
		//IL_015f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0161: Unknown result type (might be due to invalid IL or missing references)
		//IL_0166: Unknown result type (might be due to invalid IL or missing references)
		//IL_016b: Unknown result type (might be due to invalid IL or missing references)
		//IL_016c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0171: Unknown result type (might be due to invalid IL or missing references)
		//IL_0178: Unknown result type (might be due to invalid IL or missing references)
		//IL_017d: Unknown result type (might be due to invalid IL or missing references)
		//IL_017e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0181: Unknown result type (might be due to invalid IL or missing references)
		//IL_0186: Unknown result type (might be due to invalid IL or missing references)
		//IL_018b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0198: Unknown result type (might be due to invalid IL or missing references)
		//IL_019a: Unknown result type (might be due to invalid IL or missing references)
		//IL_019c: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f4: Unknown result type (might be due to invalid IL or missing references)
		//IL_0101: Unknown result type (might be due to invalid IL or missing references)
		//IL_0106: Unknown result type (might be due to invalid IL or missing references)
		//IL_010b: Unknown result type (might be due to invalid IL or missing references)
		//IL_005f: Unknown result type (might be due to invalid IL or missing references)
		//IL_006c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0071: Unknown result type (might be due to invalid IL or missing references)
		//IL_0076: Unknown result type (might be due to invalid IL or missing references)
		//IL_0147: Unknown result type (might be due to invalid IL or missing references)
		//IL_014a: Unknown result type (might be due to invalid IL or missing references)
		//IL_014f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0153: Unknown result type (might be due to invalid IL or missing references)
		//IL_0158: Unknown result type (might be due to invalid IL or missing references)
		//IL_015d: Unknown result type (might be due to invalid IL or missing references)
		//IL_00af: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b2: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b7: Unknown result type (might be due to invalid IL or missing references)
		//IL_00bb: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c0: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c5: Unknown result type (might be due to invalid IL or missing references)
		Point3d pointAtStart = curves[crvIndex].PointAtStart;
		Vector3d val = curves[crvIndex].PointAtEnd - pointAtStart;
		((Vector3d)(ref val)).Unitize();
		Vector3d destination = default(Vector3d);
		((Vector3d)(ref destination))._002Ector(val);
		double num = sizeX / 2.0;
		if (distance < num)
		{
			int num2 = crvIndex - 1;
			if (num2 == -1)
			{
				num2 = curves.Count - 1;
			}
			Vector3d val2 = curves[num2].PointAtEnd - curves[num2].PointAtStart;
			((Vector3d)(ref val2)).Unitize();
			double num3 = distance;
			if (num3 < 0.0)
			{
				num3 = 0.0;
			}
			double num4 = num3 / num;
			double num5 = 1.0 - num4;
			destination = val * num4 + val2 * num5;
		}
		else
		{
			double length = curves[crvIndex].GetLength();
			if (length - distance < num)
			{
				int index = (crvIndex + 1) % curves.Count;
				Vector3d val3 = curves[index].PointAtEnd - curves[index].PointAtStart;
				((Vector3d)(ref val3)).Unitize();
				double num6 = length - distance;
				if (num6 < 0.0)
				{
					num6 = 0.0;
				}
				double num7 = num6 / num;
				double num8 = 1.0 - num7;
				destination = val * num7 + val3 * num8;
			}
		}
		pointAtStart += val * distance;
		Transform val4 = CSBlockUtil.CSUtilMath.Orient3Point(Point3d.Origin, Vector3d.XAxis, gemDefinition.Normal, pointAtStart, destination, centerGem.Normal);
		CSData data = gemDefinition.Duplicate();
		CSDataHelper.Transform(data, val4 * xOrientLocal);
		outputGems.Append(data);
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
		if (style == 0 && !PlaceGemCorner(val, val2, centerGem, gemDefinition, xTransformLocal, angle, outputGems))
		{
			return false;
		}
		if (!PlaceGemsCurveSide(mergedCrv, centerGem, gemDefinition, gemDefinitionSizeX, spacing, style, xTransformLocal, outputGems))
		{
			return false;
		}
		if (style == 0 && !PlaceGemCorner(val3, val4, centerGem, gemDefinition, xTransformLocal, angle, outputGems))
		{
			return false;
		}
		if (!PlaceGemsCurveSide(mergedCrv2, centerGem, gemDefinition, gemDefinitionSizeX, spacing, style, xTransformLocal, outputGems))
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
		if (!PlaceGemCorners((IList<Curve>)(object)new Curve[2] { mergedCrv2, mergedCrv }, centerGem, gemDefinition, xTransformLocal, cSDataArray, angle))
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

	private static bool PlaceGemsMarquise(string centerGemShape, CSGemData centerGem, CSGemData gemDefinition, double gemDefinitionSizeX, double spacing, double offsetRailX, double offsetRailZ, Transform xTransformLocal, double angle, ref CSDataArray outputGems)
	{
		//IL_003c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0043: Unknown result type (might be due to invalid IL or missing references)
		//IL_007a: Unknown result type (might be due to invalid IL or missing references)
		//IL_009e: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c2: Unknown result type (might be due to invalid IL or missing references)
		Curve[] array = GemOffsetCurve.GemOffsetCurves(centerGemShape, centerGem, offsetRailX, doTransformUp: true);
		if (array.Length != 2)
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
		CSDataArray cSDataArray = new CSDataArray();
		if (!PlaceGemCorners((IList<Curve>)(object)new Curve[2] { val2, val }, centerGem, gemDefinition, xTransformLocal, cSDataArray, angle))
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
		if (!PlaceGemCorners((IList<Curve>)(object)new Curve[2] { mergedCrv, mergedCrv2 }, centerGem, gemDefinition, xTransformLocal, cSDataArray, angle))
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

	private static bool PlaceGemsPear(string centerGemShape, CSGemData centerGem, CSGemData gemDefinition, double gemDefinitionSizeX, double spacing, double offsetRailX, double offsetRailZ, Transform xTransformLocal, double angle, ref CSDataArray outputGems)
	{
		//IL_003c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0043: Unknown result type (might be due to invalid IL or missing references)
		//IL_00be: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e2: Unknown result type (might be due to invalid IL or missing references)
		//IL_0106: Unknown result type (might be due to invalid IL or missing references)
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
		if (!CSUtilCurves.MergeCurves((IEnumerable<Curve>)(object)new Curve[2]
		{
			array[0],
			array[1]
		}, out Curve mergedCrv, 0.01))
		{
			return false;
		}
		if (!CSUtilCurves.MergeCurves((IEnumerable<Curve>)(object)new Curve[2]
		{
			array[2],
			array[3]
		}, out Curve mergedCrv2, 0.01))
		{
			return false;
		}
		CSDataArray cSDataArray = new CSDataArray();
		if (!PlaceGemCorners((IList<Curve>)(object)new Curve[2] { mergedCrv2, mergedCrv }, centerGem, gemDefinition, xTransformLocal, cSDataArray, angle))
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

	public static bool PlaceGemsRound(string centerGemShape, CSGemData centerGem, CSGemData gemDefinition, double gemDefinitionSizeX, double spacing, double offsetRailX, double offsetRailZ, Transform xTransformLocal, double fixPosition, bool isEven, ref CSDataArray outputGems)
	{
		//IL_002f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0036: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d1: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d6: Unknown result type (might be due to invalid IL or missing references)
		//IL_00dc: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e7: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f2: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f8: Unknown result type (might be due to invalid IL or missing references)
		//IL_00fd: Unknown result type (might be due to invalid IL or missing references)
		//IL_0102: Unknown result type (might be due to invalid IL or missing references)
		//IL_0135: Unknown result type (might be due to invalid IL or missing references)
		//IL_0137: Unknown result type (might be due to invalid IL or missing references)
		//IL_0139: Unknown result type (might be due to invalid IL or missing references)
		if (!GemOffsetCurve.GemOffset(centerGemShape, centerGem, offsetRailX, out Curve curve, doTransformUp: true, out string _, 0.0, 0.0, 0.0))
		{
			return false;
		}
		((GeometryBase)curve).Translate(centerGem.Normal * offsetRailZ);
		int num = (int)(curve.GetLength() / (gemDefinitionSizeX + spacing));
		if (isEven && num % 2 == 1)
		{
			num--;
		}
		double[] array = new double[num];
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
		double[] array2 = curve.NormalizedLengthParameters(array, CSApplicationHelper.Application.Tolerance);
		for (int j = 0; j < num; j++)
		{
			Transform val = CSBlockUtil.CSUtilMath.Orient3Point(Point3d.Origin, Vector3d.XAxis, gemDefinition.Up, curve.PointAt(array2[j]), curve.TangentAt(array2[j]), centerGem.Up);
			CSGemData cSGemData = (CSGemData)gemDefinition.Duplicate();
			cSGemData.SetUserString("Corner90", "0");
			cSGemData.SetUserString("Corner", "0");
			CSDataHelper.Transform(cSGemData, val * xTransformLocal);
			outputGems.Append(cSGemData);
		}
		return true;
	}

	private static bool PlaceGemsSquare(string centerGemShape, CSGemData centerGem, CSGemData gemDefinition, double gemDefinitionSizeX, double spacing, double offsetRailX, double offsetRailZ, Transform xTransformLocal, int style, double cornerBlend, double angle, ref CSDataArray outputGems)
	{
		//IL_0035: Unknown result type (might be due to invalid IL or missing references)
		//IL_003c: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d2: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b3: Unknown result type (might be due to invalid IL or missing references)
		//IL_012e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0158: Unknown result type (might be due to invalid IL or missing references)
		//IL_0183: Unknown result type (might be due to invalid IL or missing references)
		Curve[] array = GemOffsetCurve.GemOffsetCurves(centerGemShape, centerGem, offsetRailX, doTransformUp: true, cornerBlend);
		if (array.Length != 5)
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
		if (!CSUtilCurves.MergeCurves((IEnumerable<Curve>)(object)new Curve[2] { val, val5 }, out Curve mergedCrv, 0.01))
		{
			return false;
		}
		CSDataArray cSDataArray = new CSDataArray();
		if (style == 0 && !PlaceGemCorners((IList<Curve>)(object)new Curve[4] { mergedCrv, val2, val3, val4 }, centerGem, gemDefinition, xTransformLocal, cSDataArray, angle))
		{
			return false;
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
		if (style == 0 && !PlaceGemCorners((IList<Curve>)(object)new Curve[4]
		{
			array[0],
			array[1],
			array[2],
			array[3]
		}, centerGem, gemDefinition, xTransformLocal, cSDataArray, angle))
		{
			return false;
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
		if (style == 0 && !PlaceGemCorners((IList<Curve>)(object)new Curve[3] { mergedCrv, val2, val3 }, centerGem, gemDefinition, xTransformLocal, cSDataArray, angle))
		{
			return false;
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
		if (style == 0 && !PlaceGemCorners((IList<Curve>)(object)new Curve[3] { mergedCrv, mergedCrv2, mergedCrv3 }, centerGem, gemDefinition, xTransformLocal, cSDataArray, angle))
		{
			return false;
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
