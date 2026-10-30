using System;
using System.Collections.Generic;
using System.Linq;
using CS;
using CS.CSObjects.CSBlocks;
using CS.Parametric;
using CS.Parametric.Channels;
using CSBlockDataUtils.CSGemOffsetCurves;
using CSBlockUtil;
using CSCompatibility;
using CSDatas;
using GV.CounterSketch.Shared;
using RMA.OpenNURBS;
using RMA.Rhino;
using Rhino;
using Rhino.Geometry;
using Rhino.Runtime;

namespace CSBlocks.GemClusters;

[CSBlock("HaloBuilder", "{03EB6BEE-9039-41D6-ABC5-32B3E789A9FB}")]
public sealed class CSHaloBuilder : CSBlock
{
	public CSInputChannel InCenterGem { get; }

	public CSInputChannel InCenterGemShapeOverrideEnabled { get; }

	public CSInputChannel InCenterGemShapeOverride { get; }

	public CSInputChannel InCenterGemSpacing { get; }

	public CSInputChannel InProfile { get; }

	public CSInputChannel InProfile2 { get; }

	public CSInputChannel InProfileWidth { get; }

	public CSInputChannel InProfileHeight { get; }

	public CSInputChannel InChannelWallThicknessInner { get; }

	public CSInputChannel InChannelWallThicknessOuter { get; }

	public CSInputChannel InChannelWallDepth { get; }

	public CSInputChannel InAngle { get; }

	public CSInputChannel InPlacement { get; }

	public CSInputChannel InChannelCornerBlendRadius { get; }

	public CSInputChannel InGemSizeZ { get; }

	public CSInputChannel InShowChannel { get; }

	public CSInputChannel InXYScaleToGem { get; }

	public CSInputChannel InCenterGemShapeOverrideMode { get; }

	public CSInputChannel InOffsetFix { get; }

	public CSInputChannel InChannelWallThicknessExtraInner { get; }

	public CSInputChannel InChannelWallThicknessExtraOuter { get; }

	public CSInputChannel InRailX { get; }

	public CSInputChannel InRailZ { get; }

	public CSInputChannel InChannelBottomOffset { get; }

	public CSInputChannel InMetalToggle { get; }

	public CSInputChannel InOffsetX { get; }

	public CSInputChannel InOffsetY { get; }

	public CSInputChannel InEmeraldCornerPoint1 { get; }

	public CSInputChannel InEmeraldCornerPoint2 { get; }

	public CSInputChannel InRotate { get; }

	public CSInputChannel InGemSizeToggle { get; }

	public CSInputChannel InGemSize { get; }

	public CSInputChannel InChannelSpacing { get; }

	public CSInputChannel InForceChannel { get; }

	public CSInputChannel InCenterGemShapeCornerBlendEnabled { get; }

	public CSInputChannel InCenterGemShapeCornerBlendRadiusInner { get; }

	public CSInputChannel InCenterGemShapeCornerBlendRadiusOuter { get; }

	public CSInputChannel InCenterGemShapeCornerBlendOffsetPercentage { get; }

	public CSInputChannel InDepthFromGems { get; }

	public CSInputChannel InGemPlacement { get; }

	public CSInputChannel InMinCuletUp { get; }

	public CSInputChannel InMinCuletSmallerFlag { get; }

	public CSInputChannel InUseMinCulet { get; }

	public CSOutputChannel OutMetal { get; }

	public CSOutputChannel OutEmeraldCorner1Point { get; }

	public CSOutputChannel OutEmeraldCorner2Point { get; }

	public CSHaloBuilder(string identifier)
		: base(identifier)
	{
		InCenterGem = new CSInputChannel(this, ChannelDataType.Any, ChannelArrayType.Array);
		InCenterGemShapeOverrideEnabled = new CSInputChannel(this, ChannelDataType.Boolean, ChannelArrayType.Individual, defaultValue: false);
		InCenterGemShapeOverride = new CSInputChannel(this, ChannelDataType.Gem, ChannelArrayType.Individual);
		InCenterGemShapeOverrideMode = new CSInputChannel(this, ChannelDataType.Integer, ChannelArrayType.Individual, ValueListItem.GemClusterCenterShapeOverrideModeDimensionSmallest);
		InCenterGemSpacing = new CSInputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual, 0.0);
		InProfile = new CSInputChannel(this, ChannelDataType.Curve, ChannelArrayType.Individual);
		InProfile2 = new CSInputChannel(this, ChannelDataType.Curve, ChannelArrayType.Individual);
		InProfileWidth = new CSInputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual, 0.0);
		InProfileHeight = new CSInputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual, 0.0);
		InChannelCornerBlendRadius = new CSInputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual, 0.0);
		InChannelBottomOffset = new CSInputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual, 0.0);
		InChannelSpacing = new CSInputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual, 0.0);
		InChannelWallThicknessInner = new CSInputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual, 0.0);
		InChannelWallThicknessOuter = new CSInputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual, 0.0);
		InChannelWallDepth = new CSInputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual, 0.0);
		InAngle = new CSInputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual, 0.0);
		InPlacement = new CSInputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual, 0.0);
		InShowChannel = new CSInputChannel(this, ChannelDataType.Boolean, ChannelArrayType.Individual, defaultValue: true);
		InXYScaleToGem = new CSInputChannel(this, ChannelDataType.Boolean, ChannelArrayType.Individual, defaultValue: false);
		InOffsetFix = new CSInputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual, 0.0);
		InChannelWallThicknessExtraInner = new CSInputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual, 0.0);
		InChannelWallThicknessExtraOuter = new CSInputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual, 0.0);
		InRailX = new CSInputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual, 0.0);
		InRailZ = new CSInputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual, 0.0);
		InMetalToggle = new CSInputChannel(this, ChannelDataType.Boolean, ChannelArrayType.Individual, defaultValue: true);
		InOffsetX = new CSInputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual, 0.0);
		InOffsetY = new CSInputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual, 0.0);
		InEmeraldCornerPoint1 = new CSInputChannel(this, ChannelDataType.Point, ChannelArrayType.Individual, Point3d.Unset);
		InEmeraldCornerPoint2 = new CSInputChannel(this, ChannelDataType.Point, ChannelArrayType.Individual, Point3d.Unset);
		InRotate = new CSInputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual, 0.0);
		InGemSizeToggle = new CSInputChannel(this, ChannelDataType.Boolean, ChannelArrayType.Individual, defaultValue: false);
		InGemSize = new CSInputChannel(this, ChannelDataType.Gem, ChannelArrayType.Individual);
		InGemSizeZ = new CSInputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual, 0.0);
		InForceChannel = new CSInputChannel(this, ChannelDataType.Boolean, ChannelArrayType.Individual, defaultValue: false);
		InCenterGemShapeCornerBlendEnabled = new CSInputChannel(this, ChannelDataType.Boolean, ChannelArrayType.Individual, defaultValue: false);
		InCenterGemShapeCornerBlendRadiusInner = new CSInputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual, 0.0);
		InCenterGemShapeCornerBlendRadiusOuter = new CSInputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual, 0.0);
		InCenterGemShapeCornerBlendOffsetPercentage = new CSInputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual, 0.0);
		InDepthFromGems = new CSInputChannel(this, ChannelDataType.Boolean, ChannelArrayType.Individual, defaultValue: false);
		InGemPlacement = new CSInputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual, 0.0);
		InMinCuletUp = new CSInputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual, 0.0);
		InMinCuletSmallerFlag = new CSInputChannel(this, ChannelDataType.Boolean, ChannelArrayType.Individual, defaultValue: false);
		InUseMinCulet = new CSInputChannel(this, ChannelDataType.Integer, ChannelArrayType.Individual, ValueListItem.NoMinCulet);
		OutMetal = new CSOutputChannel(this, ChannelDataType.Any, ChannelArrayType.Array);
		OutEmeraldCorner1Point = new CSOutputChannel(this, ChannelDataType.Point, ChannelArrayType.Individual);
		OutEmeraldCorner2Point = new CSOutputChannel(this, ChannelDataType.Point, ChannelArrayType.Individual);
		SetChannelIdentifiers(autoCheckInputs: true);
	}

	public override bool Update()
	{
		if (!InMetalToggle.Boolean)
		{
			OutMetal.SetData(null);
			return base.Update();
		}
		CSDataArray flattenArray = InCenterGem.GetFlattenArray();
		if (flattenArray.GetArrayCount() <= 0)
		{
			return UpdateMissing("CenterGem");
		}
		CSGemData cSGemData = flattenArray.GetArrayItem(0) as CSGemData;
		CSData arrayItem = flattenArray.GetArrayItem(0);
		bool centerGemShapeOverrideEnabled = InCenterGemShapeOverrideEnabled.Boolean || cSGemData == null;
		CSGemData gem = InCenterGemShapeOverride.Gem;
		ValueListItem valueListItem = InCenterGemShapeOverrideMode.ValueListItem;
		double num = InCenterGemSpacing.Double;
		double profileWidth = InProfileWidth.Double;
		double profileHeight = InProfileHeight.Double;
		double channelBottomOffset = InChannelBottomOffset.Double;
		double channelCornerBlendRadius = InChannelCornerBlendRadius.Double;
		double channelWallThicknessInner = InChannelWallThicknessInner.Double;
		double channelWallThicknessOuter = InChannelWallThicknessOuter.Double;
		double channelWallDepth = InChannelWallDepth.Double;
		double angleDegrees = InAngle.Double;
		angleDegrees = CSBlockUtil.CSUtilMath.ToRadians(angleDegrees);
		double num2 = InPlacement.Double;
		double gemSizeZ = InGemSizeZ.Double;
		double centerGemShapeCornerBlendRadiusInner = InCenterGemShapeCornerBlendRadiusInner.Double;
		double centerGemShapeCornerBlendRadiusOuter = InCenterGemShapeCornerBlendRadiusOuter.Double;
		double centerGemShapeCornerBlendOffsetPercentage = InCenterGemShapeCornerBlendOffsetPercentage.Double;
		bool boolean = InCenterGemShapeCornerBlendEnabled.Boolean;
		if (!boolean)
		{
			centerGemShapeCornerBlendRadiusInner = 0.0;
			centerGemShapeCornerBlendRadiusOuter = 0.0;
			centerGemShapeCornerBlendOffsetPercentage = 0.0;
		}
		double num3 = InOffsetFix.Double;
		num += num3;
		bool flag = InShowChannel.Boolean;
		if (InForceChannel.Boolean)
		{
			flag = true;
		}
		bool boolean2 = InXYScaleToGem.Boolean;
		double channelWallThicknessExtraInner = InChannelWallThicknessExtraInner.Double;
		double channelWallThicknessExtraOuter = InChannelWallThicknessExtraOuter.Double;
		OnNurbsCurve profile = (flag ? InProfile2.NurbsCurve.Convert().Duplicate() : InProfile.NurbsCurve.Convert().Duplicate());
		double num4 = InRailX.Double;
		num += num4;
		double num5 = InRailZ.Double;
		num2 += num5;
		double offsetX = InOffsetX.Double;
		double offsetY = InOffsetY.Double;
		Point3d point3d = InEmeraldCornerPoint1.Point3d;
		Point3d point3d2 = InEmeraldCornerPoint2.Point3d;
		double rotate = InRotate.Double;
		bool boolean3 = InGemSizeToggle.Boolean;
		CSGemData gem2 = InGemSize.Gem;
		double channelSpacing = InChannelSpacing.Double;
		bool boolean4 = InDepthFromGems.Boolean;
		double gemPlacement = InGemPlacement.Double;
		double minCuletUp = InMinCuletUp.Double;
		bool boolean5 = InMinCuletSmallerFlag.Boolean;
		ValueListItem valueListItem2 = InUseMinCulet.ValueListItem;
		if (!MakeHaloProfile(profile, profileWidth, profileHeight, channelWallThicknessInner, channelWallThicknessOuter, channelWallDepth, angleDegrees, channelCornerBlendRadius, gemSizeZ, flag, channelWallThicknessExtraInner, channelWallThicknessExtraOuter, channelBottomOffset, boolean3, gem2, channelSpacing, boolean4, gemPlacement, minCuletUp, boolean5, valueListItem2, out var haloProfile, out var haloProfileChannelWallThicknessExtraInner, out var haloProfileChannelWallThicknessInner, out var haloProfileInnerX, out var outError))
		{
			return UpdateError(outError);
		}
		if (!MakeHalo(cSGemData, arrayItem, centerGemShapeOverrideEnabled, gem, num, haloProfile, haloProfileChannelWallThicknessExtraInner, haloProfileChannelWallThicknessInner, haloProfileInnerX, num2, boolean2, valueListItem, boolean, centerGemShapeCornerBlendRadiusInner, centerGemShapeCornerBlendRadiusOuter, centerGemShapeCornerBlendOffsetPercentage, offsetX, offsetY, point3d, point3d2, rotate, out var outputMetals, out var outputTransforms, out var emeraldCorner1Point, out var emeraldCorner2Point, out outError))
		{
			return UpdateError(outError);
		}
		OutEmeraldCorner1Point.SetData(new CSPointData(emeraldCorner1Point));
		OutEmeraldCorner2Point.SetData(new CSPointData(emeraldCorner2Point));
		CSDataArray cSDataArray = new CSDataArray();
		for (int i = 0; i < outputMetals.Count; i++)
		{
			cSDataArray.Append(new CSOnGeometryData(outputMetals[i].Convert(), outputTransforms[i].Convert(), applyTransform: false));
		}
		OutMetal.SetData(cSDataArray);
		return base.Update();
	}

	private static bool GetLargestIntersectionY(IOnCurve c1, IOnCurve c2, ref double param)
	{
		ArrayOnX_EVENT x = new ArrayOnX_EVENT();
		c1.IntersectCurve(c2, ref x, CSApplicationHelper.Application.Tolerance);
		if (x.Count() == 0)
		{
			return false;
		}
		double num = double.MinValue;
		foreach (OnX_EVENT item in x)
		{
			if (item.get_m_pointA(0).y > num)
			{
				num = item.get_m_pointA(0).y;
				param = item.get_m_a(0);
			}
		}
		return true;
	}

	private static bool MakeHalo(CSGemData centerGem, CSData centerObject, bool centerGemShapeOverrideEnabled, CSGemData centerGemShapeOverride, double centerGemSpacing, IOnCurve haloProfile, double haloProfileChannelWallThicknessExtraInner, double haloProfileChannelWallThicknessInner, double haloProfileInnerX, double placement, bool doScaleToGemXY, ValueListItem centerGemShapeOverrideMode, bool isCenterGemShapeCornerBlendEnabled, double centerGemShapeCornerBlendRadiusInner, double centerGemShapeCornerBlendRadiusOuter, double centerGemShapeCornerBlendOffsetPercentage, double offsetX, double offsetY, Point3d inputCorner1, Point3d inputCorner2, double rotate, out List<OnBrep> outputMetals, out List<OnXform> outputTransforms, out Point3d emeraldCorner1Point, out Point3d emeraldCorner2Point, out string outError)
	{
		outputMetals = new List<OnBrep>();
		outputTransforms = new List<OnXform>();
		emeraldCorner1Point = Point3d.Unset;
		emeraldCorner2Point = Point3d.Unset;
		outError = string.Empty;
		BoundingBox boundingBox = BoundingBox.Unset;
		centerObject.GetTightBoundingBox(ref boundingBox);
		double num = Math.Abs(boundingBox.Max.X - boundingBox.Min.X + offsetX);
		double num2 = Math.Abs(boundingBox.Max.Y - boundingBox.Min.Y + offsetY);
		string gemShape;
		if (centerGemShapeOverrideEnabled)
		{
			if (centerGemShapeOverride == null)
			{
				centerGemShapeOverride = new CSGemData(centerGem);
			}
			else
			{
				double num3;
				double num4;
				if (centerGem != null)
				{
					switch (centerGemShapeOverrideMode)
					{
					case ValueListItem.GemClusterCenterShapeOverrideModeDimensionX:
						num3 = (centerGem.GemXSize + offsetX) / centerGemShapeOverride.GemXSize;
						num4 = num3;
						break;
					case ValueListItem.GemClusterCenterShapeOverrideModeDimensionY:
						num4 = (centerGem.GemYSize + offsetY) / centerGemShapeOverride.GemYSize;
						num3 = num4;
						break;
					case ValueListItem.GemClusterCenterShapeOverrideModeDimensionLargest:
						if (centerGem.GemXSize > centerGem.GemYSize)
						{
							num3 = (centerGem.GemXSize + offsetX) / centerGemShapeOverride.GemXSize;
							num4 = num3;
						}
						else
						{
							num4 = (centerGem.GemYSize + offsetY) / centerGemShapeOverride.GemYSize;
							num3 = num4;
						}
						break;
					case ValueListItem.GemClusterCenterShapeOverrideModeDimensionSmallest:
						if (centerGem.GemXSize < centerGem.GemYSize)
						{
							num3 = (centerGem.GemXSize + offsetX) / centerGemShapeOverride.GemXSize;
							num4 = num3;
						}
						else
						{
							num4 = (centerGem.GemYSize + offsetY) / centerGemShapeOverride.GemYSize;
							num3 = num4;
						}
						break;
					case ValueListItem.GemClusterCenterShapeOverrideModeDimensionXandY:
						num3 = (centerGem.GemXSize + offsetX) / centerGemShapeOverride.GemXSize;
						num4 = (centerGem.GemYSize + offsetY) / centerGemShapeOverride.GemYSize;
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
						num3 = num / centerGemShapeOverride.GemXSize;
						num4 = num3;
						break;
					case ValueListItem.GemClusterCenterShapeOverrideModeDimensionY:
						num4 = num2 / centerGemShapeOverride.GemYSize;
						num3 = num4;
						break;
					case ValueListItem.GemClusterCenterShapeOverrideModeDimensionLargest:
						if (num > num2)
						{
							num3 = num / centerGemShapeOverride.GemXSize;
							num4 = num3;
						}
						else
						{
							num4 = num2 / centerGemShapeOverride.GemYSize;
							num3 = num4;
						}
						break;
					case ValueListItem.GemClusterCenterShapeOverrideModeDimensionSmallest:
						if (num < num2)
						{
							num3 = num / centerGemShapeOverride.GemXSize;
							num4 = num3;
						}
						else
						{
							num4 = num2 / centerGemShapeOverride.GemYSize;
							num3 = num4;
						}
						break;
					case ValueListItem.GemClusterCenterShapeOverrideModeDimensionXandY:
						num3 = num / centerGemShapeOverride.GemXSize;
						num4 = num2 / centerGemShapeOverride.GemYSize;
						break;
					default:
						outError = "Invalid Gem Cluster Center Shape Override Mode";
						return false;
					}
				}
				centerGemShapeOverride.Scale(num3, num4);
			}
			gemShape = centerGemShapeOverride.GemShape;
		}
		else
		{
			centerGemShapeOverride = new CSGemData(centerGem);
			gemShape = centerGem.GemShape;
		}
		Transform xUp;
		Transform inverseTransform;
		if (centerGem != null)
		{
			if (!CSBlockUtil.CSUtilMath.GetUnscaledXforms(centerGem.Xform, out xUp, out inverseTransform))
			{
				outError = "Gem Xform";
				return false;
			}
		}
		else
		{
			xUp = Transform.Translation(boundingBox.Center.X, boundingBox.Center.Y, boundingBox.Center.Z) * Transform.Rotation(RhinoMath.ToRadians(rotate), Vector3d.YAxis, Point3d.Origin);
			if (!xUp.TryGetInverse(out inverseTransform))
			{
				inverseTransform = Transform.Identity;
			}
		}
		CSGemData cSGemData = null;
		On3dVector on3dVector;
		if (centerGem != null)
		{
			cSGemData = new CSGemData(centerGem);
			cSGemData.ApplyTransform(inverseTransform);
			on3dVector = new On3dVector(cSGemData.Up.Convert());
			on3dVector.Unitize();
		}
		else
		{
			Point3d point3d = new Point3d(boundingBox.Center.X, boundingBox.Center.Y, 0.0);
			Point3d point3d2 = point3d;
			point3d2.Transform(Transform.Translation(0.0, 0.0, boundingBox.Center.Z));
			Vector3d value = new Vector3d((point3d.Z >= point3d2.Z) ? (point3d - point3d2) : (point3d2 - point3d));
			value.Unitize();
			on3dVector = CSConvert.Vector3dCopyToOn3dVector(value);
		}
		double optionalEmeraldCornerXPercent = 0.0;
		double optionalEmeraldCornerYPercent = 0.0;
		if (inputCorner1 != Point3d.Unset && inputCorner2 != Point3d.Unset)
		{
			optionalEmeraldCornerXPercent = inputCorner1.X / (centerGemShapeOverride.GemXSize / 2.0);
			optionalEmeraldCornerYPercent = inputCorner2.Y / (centerGemShapeOverride.GemYSize / 2.0);
		}
		OnNurbsCurve nurbs_curve = new OnNurbsCurve();
		haloProfile.GetNurbForm(ref nurbs_curve);
		List<On3dPoint> list = new List<On3dPoint>();
		for (int i = 0; i < nurbs_curve.CVCount(); i++)
		{
			On3dPoint vertex_value = new On3dPoint();
			if (!nurbs_curve.GetCV(i, ref vertex_value))
			{
				outError = "GetCV failed on haloProfileNc";
				return false;
			}
			list.Add(vertex_value);
		}
		double distance = centerGemSpacing + list.Min((On3dPoint _) => _.x);
		double cornerBlend = (isCenterGemShapeCornerBlendEnabled ? centerGemShapeCornerBlendRadiusInner : 0.0);
		if (!GemOffsetCurve.GemOffset(gemShape, centerGemShapeOverride, distance, out OnCurve curve, false, out string error, cornerBlend, optionalEmeraldCornerXPercent, optionalEmeraldCornerYPercent))
		{
			outError = "CurveTop Inside:" + error;
			return false;
		}
		OnNurbsCurve nurbs_curve2 = new OnNurbsCurve();
		curve.GetNurbForm(ref nurbs_curve2);
		OnNurbsSurface onNurbsSurface = new OnNurbsSurface();
		onNurbsSurface.Create(3, is_rat: true, nurbs_curve.m_order, nurbs_curve2.m_order, nurbs_curve.m_cv_count, nurbs_curve2.m_cv_count);
		bool flag = false;
		List<IOnCurve> list2 = new List<IOnCurve>();
		CSGemData gem = (doScaleToGemXY ? cSGemData : centerGemShapeOverride);
		double y = curve.PointAtStart().y;
		OnCurve source = curve;
		for (int num5 = 0; num5 < list.Count; num5++)
		{
			On3dPoint on3dPoint = list[num5];
			double distance2 = centerGemSpacing + on3dPoint.x;
			double cornerBlend2;
			if (isCenterGemShapeCornerBlendEnabled)
			{
				double num6 = haloProfileChannelWallThicknessExtraInner + haloProfileChannelWallThicknessInner;
				double num7 = on3dPoint.x - haloProfileInnerX;
				if (num7 < num6)
				{
					double num8 = num7 / num6;
					cornerBlend2 = centerGemShapeCornerBlendRadiusInner + (centerGemShapeCornerBlendRadiusOuter - centerGemShapeCornerBlendRadiusInner) * num8;
				}
				else
				{
					double num9 = centerGemShapeCornerBlendRadiusOuter + (num7 - num6);
					cornerBlend2 = centerGemShapeCornerBlendRadiusOuter + (num9 - centerGemShapeCornerBlendRadiusOuter) * centerGemShapeCornerBlendOffsetPercentage;
				}
			}
			else
			{
				cornerBlend2 = 0.0;
			}
			if (!GemOffsetCurve.GemOffset(gemShape, gem, distance2, out curve, false, out error, cornerBlend2, optionalEmeraldCornerXPercent, optionalEmeraldCornerYPercent))
			{
				outError = "CurveTop Inside:" + error;
				return false;
			}
			curve.Translate(new On3dVector(on3dVector) * (placement + on3dPoint.y));
			list2.Add(curve);
			if (curve.PointAtStart().y > y)
			{
				y = curve.PointAtStart().y;
				source = curve;
			}
			OnNurbsCurve nurbs_curve3 = new OnNurbsCurve();
			curve.GetNurbForm(ref nurbs_curve3);
			if (nurbs_curve2.CVCount() != nurbs_curve3.CVCount())
			{
				flag = true;
			}
			if (!flag)
			{
				for (int num10 = 0; num10 < nurbs_curve2.CVCount(); num10++)
				{
					On3dPoint vertex_value2 = new On3dPoint();
					nurbs_curve3.GetCV(num10, ref vertex_value2);
					double num11 = nurbs_curve.Weight(num5) * nurbs_curve3.Weight(num10);
					onNurbsSurface.SetCV(num5, num10, new On4dPoint(vertex_value2.x * num11, vertex_value2.y * num11, vertex_value2.z * num11, num11));
				}
			}
		}
		if (gemShape == "Emerald")
		{
			Curve curve2 = Interop.FromOnCurve(source);
			curve2.GetNextDiscontinuity(Continuity.C2_continuous, 0.0, 10.0, out var t);
			emeraldCorner1Point = curve2.PointAt(t);
			curve2.GetNextDiscontinuity(Continuity.C2_continuous, t, 20.0, out t);
			emeraldCorner2Point = curve2.PointAt(t);
		}
		if (!flag)
		{
			for (int num12 = 0; num12 < nurbs_curve.KnotCount(); num12++)
			{
				onNurbsSurface.SetKnot(0, num12, nurbs_curve.Knot(num12));
			}
			for (int num13 = 0; num13 < nurbs_curve2.KnotCount(); num13++)
			{
				onNurbsSurface.SetKnot(1, num13, nurbs_curve2.Knot(num13));
			}
			OnBrep brep = new OnBrep();
			onNurbsSurface.BrepForm(brep);
			brep.Transform(xUp.Convert());
			CSUtilGeometry.FixBrepSolidOrientation(ref brep);
			if (!brep.IsSolid())
			{
				Brep brep2 = brep.Convert();
				brep2.JoinNakedEdges(CSApplicationHelper.Application.Tolerance);
				if (!brep2.IsSolid)
				{
					brep2.Flip();
				}
				brep = (OnBrep)Interop.ToOnBrep(brep2);
			}
			outputMetals.Add(brep);
			outputTransforms.Add(xUp.Convert());
		}
		else
		{
			OnBrep brep3 = CSUtilSweep.Loft(list2.ToArray(), IArgsRhinoLoft.eLoftType.ltLoose);
			if (brep3 != null)
			{
				brep3.Transform(xUp.Convert());
				CSUtilGeometry.FixBrepSolidOrientation(ref brep3);
				outputMetals.Add(brep3);
				outputTransforms.Add(xUp.Convert());
			}
		}
		return true;
	}

	private static bool MakeHaloProfile(OnNurbsCurve profile, double profileWidth, double profileHeight, double channelWallThicknessInner, double channelWallThicknessOuter, double channelWallDepth, double angle, double channelCornerBlendRadius, double gemSizeZ, bool doShowChannel, double channelWallThicknessExtraInner, double channelWallThicknessExtraOuter, double channelBottomOffset, bool gemSizeToggle, CSGemData gemSize, double channelSpacing, bool depthFromGems, double gemPlacement, double minCuletUp, bool minCuletSmaller, ValueListItem useMinCulet, out OnCurve haloProfile, out double haloProfileChannelWallThicknessExtraInner, out double haloProfileChannelWallThicknessInner, out double haloProfileInnerX, out string outError)
	{
		haloProfile = null;
		haloProfileChannelWallThicknessExtraInner = -1.23432101234321E+308;
		haloProfileChannelWallThicknessInner = -1.23432101234321E+308;
		haloProfileInnerX = -1.23432101234321E+308;
		outError = null;
		if (profile == null)
		{
			outError = "Missing profile";
			return false;
		}
		if (!profile.IsValid())
		{
			outError = "Invalid profile";
			return false;
		}
		OnBoundingBox tight_bbox = new OnBoundingBox();
		if (!profile.GetTightBoundingBox(ref tight_bbox))
		{
			outError = "Unable to get tight bounding box from profile";
			return false;
		}
		OnXform onXform = new OnXform();
		onXform.Translation(0.0 - tight_bbox.Min().x, 0.0 - tight_bbox.Min().y, 0.0);
		OnXform onXform2 = new OnXform();
		onXform2.Scale(1.0 / (tight_bbox.Max().x - tight_bbox.Min().x), 1.0 / (tight_bbox.Max().y - tight_bbox.Min().y), 0.0);
		OnXform onXform3 = new OnXform();
		if (gemSize != null && (gemSizeToggle || minCuletSmaller))
		{
			profileWidth = gemSize.GemXSize + channelSpacing + channelWallThicknessInner + channelWallThicknessOuter;
		}
		onXform3.Scale(profileWidth + channelWallThicknessExtraInner + channelWallThicknessExtraOuter, 1.0, 1.0);
		OnXform onXform4 = new OnXform();
		onXform4.Translation(0.0 - channelWallThicknessExtraInner, 0.0, 0.0);
		profile.Transform(onXform4 * onXform3 * onXform2 * onXform);
		OnLineCurve c = new OnLineCurve(new On3dPoint(channelWallThicknessInner, -1.0, 0.0), new On3dPoint(channelWallThicknessInner, 2.0, 0.0));
		OnLineCurve c2 = new OnLineCurve(new On3dPoint(profileWidth - channelWallThicknessOuter, -1.0, 0.0), new On3dPoint(profileWidth - channelWallThicknessOuter, 2.0, 0.0));
		double param = 0.0;
		double param2 = 0.0;
		if (!GetLargestIntersectionY(profile, c, ref param))
		{
			outError = "Intersect for channel inner failed";
			return false;
		}
		if (!GetLargestIntersectionY(profile, c2, ref param2))
		{
			outError = "Intersect for channel outer failed";
			return false;
		}
		On3dPoint on3dPoint = profile.PointAt(param);
		On3dPoint on3dPoint2 = profile.PointAt(param2);
		double num = ((on3dPoint.y > on3dPoint2.y) ? on3dPoint.y : on3dPoint2.y);
		OnXform onXform5 = new OnXform();
		onXform5.Scale(1.0, (profileHeight + ((useMinCulet == ValueListItem.MinCuletShankGrowUp) ? minCuletUp : 0.0)) / num, 1.0);
		OnNurbsCurve onNurbsCurve = profile.Duplicate();
		onNurbsCurve.Transform(onXform5);
		OnCurve onCurve = onNurbsCurve.Duplicate();
		onCurve.Trim(new OnInterval(onNurbsCurve.Domain().Min(), param));
		OnCurve onCurve2 = onNurbsCurve.Duplicate();
		onCurve2.Trim(new OnInterval(param2, onNurbsCurve.Domain().Max()));
		On3dPoint on3dPoint3 = onCurve.PointAtEnd();
		On3dPoint on3dPoint4 = onCurve2.PointAtStart();
		if (doShowChannel)
		{
			double x = channelWallThicknessInner + (profileWidth - channelWallThicknessInner - channelWallThicknessOuter) / 2.0;
			double num2 = profileHeight - gemSizeZ + channelBottomOffset + minCuletUp;
			if (depthFromGems)
			{
				num2 += gemPlacement;
			}
			bool num3 = num2 > onCurve.PointAtEnd().y - channelWallDepth;
			bool flag = 0.0 > num2;
			if (num3)
			{
				num2 = onCurve.PointAtEnd().y - channelWallDepth;
			}
			if (flag)
			{
				num2 = 0.0;
			}
			On3dPoint on3dPoint5 = new On3dPoint(x, num2, 0.0);
			OnCurve onCurve9;
			OnCurve onCurve10;
			if (channelWallDepth > channelCornerBlendRadius * 2.0)
			{
				OnCurve onCurve3 = new OnLineCurve(on3dPoint3, new On3dPoint(on3dPoint3.x, on3dPoint3.y - channelWallDepth, 0.0));
				OnCurve onCurve4 = new OnLineCurve(new On3dPoint(on3dPoint3.x, on3dPoint3.y - channelWallDepth, 0.0), on3dPoint5);
				CSUtilGeometry.TrimEndOfCrv(onCurve3, channelCornerBlendRadius);
				CSUtilGeometry.TrimStartOfCrv(onCurve4, channelCornerBlendRadius);
				OnCurve onCurve5 = CSUtilCurves.BlendCurve(onCurve3, onCurve4);
				OnCurve onCurve6 = new OnLineCurve(on3dPoint5, new On3dPoint(on3dPoint4.x, on3dPoint4.y - channelWallDepth, 0.0));
				OnCurve onCurve7 = new OnLineCurve(new On3dPoint(on3dPoint4.x, on3dPoint4.y - channelWallDepth, 0.0), on3dPoint4);
				CSUtilGeometry.TrimEndOfCrv(onCurve6, channelCornerBlendRadius);
				CSUtilGeometry.TrimStartOfCrv(onCurve7, channelCornerBlendRadius);
				OnCurve onCurve8 = CSUtilCurves.BlendCurve(onCurve6, onCurve7);
				IOnCurve[] input_curves = new IOnCurve[3] { onCurve3, onCurve5, onCurve4 };
				OnCurve[] output = null;
				RhUtil.RhinoMergeCurves(input_curves, ref output);
				onCurve9 = output[0];
				IOnCurve[] input_curves2 = new IOnCurve[3] { onCurve6, onCurve8, onCurve7 };
				output = null;
				RhUtil.RhinoMergeCurves(input_curves2, ref output);
				onCurve10 = output[0];
			}
			else
			{
				onCurve9 = new OnLineCurve(onCurve.PointAtEnd(), on3dPoint5);
				onCurve10 = new OnLineCurve(on3dPoint5, onCurve2.PointAtStart());
			}
			CSUtilGeometry.TrimEndOfCrv(onCurve, channelCornerBlendRadius);
			CSUtilGeometry.TrimStartAndEndOfCrv(onCurve9, channelCornerBlendRadius);
			CSUtilGeometry.TrimStartAndEndOfCrv(onCurve10, channelCornerBlendRadius);
			CSUtilGeometry.TrimStartOfCrv(onCurve2, channelCornerBlendRadius);
			OnCurve onCurve11 = CSUtilCurves.BlendCurve(onCurve, onCurve9);
			OnCurve onCurve12 = CSUtilCurves.BlendCurve(onCurve9, onCurve10);
			OnCurve onCurve13 = CSUtilCurves.BlendCurve(onCurve10, onCurve2);
			IOnCurve[] input_curves3 = new IOnCurve[7] { onCurve, onCurve11, onCurve9, onCurve12, onCurve10, onCurve13, onCurve2 };
			OnCurve[] output2 = null;
			if (!RhUtil.RhinoMergeCurves(input_curves3, ref output2))
			{
				outError = "channel merge failed";
				return false;
			}
			if (output2.Length != 1)
			{
				outError = "channel merge produced bad results";
				return false;
			}
			haloProfile = output2[0];
		}
		else
		{
			OnXform onXform6 = new OnXform();
			onXform6.Scale(1.0, profileHeight, 1.0);
			OnNurbsCurve onNurbsCurve2 = profile.Duplicate();
			onNurbsCurve2.Transform(onXform6);
			haloProfile = onNurbsCurve2;
		}
		OnXform onXform7 = new OnXform();
		onXform7.Translation(0.0, 0.0 - profileHeight, 0.0);
		OnXform onXform8 = new OnXform();
		onXform8.Rotation(angle, OnUtil.On_zaxis, OnUtil.On_origin);
		haloProfile.Transform(onXform8 * onXform7);
		double y = on3dPoint3.y - channelWallDepth;
		On3dPoint on3dPoint6 = new On3dPoint(channelWallThicknessInner, y, 0.0);
		on3dPoint6.Transform(onXform8 * onXform7);
		double y2 = on3dPoint3.y;
		On3dPoint on3dPoint7 = new On3dPoint(channelWallThicknessInner, y2, 0.0);
		on3dPoint7.Transform(onXform8 * onXform7);
		OnBoundingBox tight_bbox2 = new OnBoundingBox();
		if (!haloProfile.GetTightBoundingBox(ref tight_bbox2))
		{
			outError = "Unable to get tight bounding box from halo profile";
			return false;
		}
		double num4 = channelWallThicknessExtraInner + channelWallThicknessInner;
		double num5 = Math.Min(on3dPoint6.x, on3dPoint7.x) - tight_bbox2.Min().x;
		haloProfileChannelWallThicknessExtraInner = num5 * (channelWallThicknessExtraInner / num4);
		haloProfileChannelWallThicknessInner = num5 * (channelWallThicknessInner / num4);
		haloProfileInnerX = tight_bbox2.Min().x;
		return true;
	}
}
