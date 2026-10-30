using System;
using System.Collections.Generic;
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

namespace CSBlocks.GemClusters.Obsolete;

[Obsolete]
[CSBlock("HaloBuilder_V3", "{A2DA2C3D-F1B2-463E-8F6C-D56DA090D492}")]
public sealed class CSHaloBuilder3 : CSBlock
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

	public CSInputChannel InCornerBlend { get; }

	public CSInputChannel InGemSizeZ { get; }

	public CSInputChannel InShowChannel { get; }

	public CSInputChannel InXYScaleToGem { get; }

	public CSInputChannel InCenterGemShapeOverrideMode { get; }

	public CSInputChannel InOffsetFix { get; }

	public CSInputChannel InInnerWallExtra { get; }

	public CSInputChannel InOuterWallExtra { get; }

	public CSInputChannel InRailX { get; }

	public CSInputChannel InRailZ { get; }

	public CSInputChannel InChannelBottomOffset { get; }

	public CSInputChannel InMetalToggle { get; }

	public CSInputChannel InBlendCornerInside { get; }

	public CSInputChannel InOffsetX { get; }

	public CSInputChannel InOffsetY { get; }

	public CSInputChannel InEmeraldCornerPoint1 { get; }

	public CSInputChannel InEmeraldCornerPoint2 { get; }

	public CSInputChannel InRotate { get; }

	public CSInputChannel InBlendCornerOutside { get; }

	public CSInputChannel InToggleBlendedCorner { get; }

	public CSInputChannel InGemSizeToggle { get; }

	public CSInputChannel InGemSize { get; }

	public CSInputChannel InChannelSpacing { get; }

	public CSInputChannel InForceChannel { get; }

	public CSOutputChannel OutMetal { get; }

	public CSOutputChannel OutEmeraldCorner1Point { get; }

	public CSOutputChannel OutEmeraldCorner2Point { get; }

	public CSHaloBuilder3(string identifier)
		: base(identifier)
	{
		InCenterGem = new CSInputChannel(this, ChannelDataType.Gem, ChannelArrayType.Array);
		InCenterGemShapeOverrideEnabled = new CSInputChannel(this, ChannelDataType.Boolean, ChannelArrayType.Individual, defaultValue: false);
		InCenterGemShapeOverride = new CSInputChannel(this, ChannelDataType.Gem, ChannelArrayType.Individual);
		InCenterGemSpacing = new CSInputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual, 0.0);
		InProfile = new CSInputChannel(this, ChannelDataType.Curve, ChannelArrayType.Individual);
		InProfile2 = new CSInputChannel(this, ChannelDataType.Curve, ChannelArrayType.Individual);
		InProfileWidth = new CSInputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual, 0.0);
		InProfileHeight = new CSInputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual, 0.0);
		InChannelWallThicknessInner = new CSInputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual, 0.0);
		InChannelWallThicknessOuter = new CSInputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual, 0.0);
		InChannelWallDepth = new CSInputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual, 0.0);
		InAngle = new CSInputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual, 0.0);
		InPlacement = new CSInputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual, 0.0);
		InCornerBlend = new CSInputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual, 0.0);
		InGemSizeZ = new CSInputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual, 0.0);
		InShowChannel = new CSInputChannel(this, ChannelDataType.Boolean, ChannelArrayType.Individual, defaultValue: true);
		InXYScaleToGem = new CSInputChannel(this, ChannelDataType.Boolean, ChannelArrayType.Individual, defaultValue: false);
		InCenterGemShapeOverrideMode = new CSInputChannel(this, ChannelDataType.Integer, ChannelArrayType.Individual, ValueListItem.GemClusterCenterShapeOverrideModeDimensionSmallest);
		InOffsetFix = new CSInputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual, 0.0);
		InInnerWallExtra = new CSInputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual, 0.0);
		InOuterWallExtra = new CSInputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual, 0.0);
		InRailX = new CSInputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual, 0.0);
		InRailZ = new CSInputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual, 0.0);
		InChannelBottomOffset = new CSInputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual, 0.0);
		InBlendCornerInside = new CSInputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual, 0.0);
		InMetalToggle = new CSInputChannel(this, ChannelDataType.Boolean, ChannelArrayType.Individual, defaultValue: true);
		InOffsetX = new CSInputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual, 0.0);
		InOffsetY = new CSInputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual, 0.0);
		InEmeraldCornerPoint1 = new CSInputChannel(this, ChannelDataType.Point, ChannelArrayType.Individual, Point3d.Unset);
		InEmeraldCornerPoint2 = new CSInputChannel(this, ChannelDataType.Point, ChannelArrayType.Individual, Point3d.Unset);
		InRotate = new CSInputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual, 0.0);
		InBlendCornerOutside = new CSInputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual, 0.0);
		InToggleBlendedCorner = new CSInputChannel(this, ChannelDataType.Boolean, ChannelArrayType.Individual, defaultValue: false);
		InGemSizeToggle = new CSInputChannel(this, ChannelDataType.Boolean, ChannelArrayType.Individual, defaultValue: false);
		InGemSize = new CSInputChannel(this, ChannelDataType.Gem, ChannelArrayType.Individual);
		InChannelSpacing = new CSInputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual, 0.0);
		InForceChannel = new CSInputChannel(this, ChannelDataType.Boolean, ChannelArrayType.Individual, defaultValue: false);
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
		List<CSGemData> gemList = InCenterGem.GemList;
		if (gemList.Count <= 0)
		{
			return UpdateMissing("CenterGem");
		}
		CSGemData cSGemData = gemList[0];
		CSData centerObject = gemList[0];
		bool centerGemShapeOverrideEnabled = InCenterGemShapeOverrideEnabled.Boolean || cSGemData == null;
		CSGemData gem = InCenterGemShapeOverride.Gem;
		double num = InCenterGemSpacing.Double;
		double profileWidth = InProfileWidth.Double;
		double profileHeight = InProfileHeight.Double;
		double channelWallThicknessInner = InChannelWallThicknessInner.Double;
		double channelWallThicknessOuter = InChannelWallThicknessOuter.Double;
		double channelWallDepth = InChannelWallDepth.Double;
		double angleDegrees = InAngle.Double;
		angleDegrees = CSBlockUtil.CSUtilMath.ToRadians(angleDegrees);
		double num2 = InPlacement.Double;
		double cornerBlend = InCornerBlend.Double;
		double gemSizeZ = InGemSizeZ.Double;
		double channelBottomOffset = InChannelBottomOffset.Double;
		double blendCornerInside = InBlendCornerInside.Double;
		double blendCornerOutside = InBlendCornerOutside.Double;
		if (!InToggleBlendedCorner.Boolean)
		{
			blendCornerOutside = 0.0;
			blendCornerInside = 0.0;
		}
		double num3 = InOffsetFix.Double;
		num += num3;
		bool flag = InShowChannel.Boolean;
		if (InForceChannel.Boolean)
		{
			flag = true;
		}
		bool boolean = InXYScaleToGem.Boolean;
		double innerWallExtra = InInnerWallExtra.Double;
		double outerWallExtra = InOuterWallExtra.Double;
		OnNurbsCurve profile = (flag ? InProfile2.NurbsCurve.Convert().Duplicate() : InProfile.NurbsCurve.Convert().Duplicate());
		ValueListItem valueListItem = InCenterGemShapeOverrideMode.ValueListItem;
		double num4 = InRailX.Double;
		num += num4;
		double num5 = InRailZ.Double;
		num2 += num5;
		double offsetX = InOffsetX.Double;
		double offsetY = InOffsetY.Double;
		Point3d point3d = InEmeraldCornerPoint1.Point3d;
		Point3d point3d2 = InEmeraldCornerPoint2.Point3d;
		double rotate = InRotate.Double;
		bool boolean2 = InGemSizeToggle.Boolean;
		CSGemData gem2 = InGemSize.Gem;
		double channelSpacing = InChannelSpacing.Double;
		if (!MakeClusterMetalProfile(profile, profileWidth, profileHeight, channelWallThicknessInner, channelWallThicknessOuter, channelWallDepth, angleDegrees, cornerBlend, gemSizeZ, flag, innerWallExtra, outerWallExtra, channelBottomOffset, boolean2, gem2, channelSpacing, out var channelProfile, out var outError))
		{
			return UpdateError(outError);
		}
		if (!MakeClusterMetal(cSGemData, centerObject, centerGemShapeOverrideEnabled, gem, num, channelProfile, num2, boolean, valueListItem, blendCornerInside, blendCornerOutside, offsetX, offsetY, point3d, point3d2, rotate, out var outputMetals, out var outputTransforms, out var emeraldCorner1Point, out var emeraldCorner2Point, out outError))
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

	private static bool CurveIntersectHighest(IOnCurve c1, IOnCurve c2, ref double param)
	{
		ArrayOnX_EVENT x = new ArrayOnX_EVENT();
		c1.IntersectCurve(c2, ref x, CSApplicationHelper.Application.Tolerance);
		if (x.Count() == 0)
		{
			return false;
		}
		double num = -999.0;
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

	private static bool MakeClusterMetalProfile(OnNurbsCurve profile, double profileWidth, double profileHeight, double channelWallThicknessInner, double channelWallThicknessOuter, double channelWallDepth, double angle, double cornerBlend, double gemSizeZ, bool showChannel, double innerWallExtra, double outerWallExtra, double channelBottomOffset, bool gemSizeToggle, CSGemData gemSize, double channelSpacing, out OnCurve channelProfile, out string outError)
	{
		channelProfile = null;
		outError = null;
		OnBoundingBox tight_bbox = new OnBoundingBox();
		profile.GetTightBoundingBox(ref tight_bbox);
		OnXform onXform = new OnXform();
		onXform.Translation(0.0 - tight_bbox.Min().x, 0.0 - tight_bbox.Min().y, 0.0);
		OnXform onXform2 = new OnXform();
		onXform2.Scale(1.0 / (tight_bbox.Max().x - tight_bbox.Min().x), 1.0 / (tight_bbox.Max().y - tight_bbox.Min().y), 0.0);
		OnXform onXform3 = new OnXform();
		if (gemSizeToggle && gemSize != null)
		{
			profileWidth = gemSize.GemXSize + channelSpacing + channelWallThicknessInner + channelWallThicknessOuter;
		}
		onXform3.Scale(profileWidth + innerWallExtra + outerWallExtra, 1.0, 1.0);
		OnXform onXform4 = new OnXform();
		onXform4.Translation(0.0 - innerWallExtra, 0.0, 0.0);
		profile.Transform(onXform4 * onXform3 * onXform2 * onXform);
		if (showChannel)
		{
			OnLineCurve c = new OnLineCurve(new On3dPoint(channelWallThicknessInner, -1.0, 0.0), new On3dPoint(channelWallThicknessInner, 2.0, 0.0));
			OnLineCurve c2 = new OnLineCurve(new On3dPoint(profileWidth - channelWallThicknessOuter, -1.0, 0.0), new On3dPoint(profileWidth - channelWallThicknessOuter, 2.0, 0.0));
			double param = 0.0;
			double param2 = 0.0;
			if (!CurveIntersectHighest(profile, c, ref param))
			{
				outError = "Intersect for channel inner failed";
				return false;
			}
			if (!CurveIntersectHighest(profile, c2, ref param2))
			{
				outError = "Intersect for channel outer failed";
				return false;
			}
			On3dPoint on3dPoint = profile.PointAt(param);
			On3dPoint on3dPoint2 = profile.PointAt(param2);
			double num = ((on3dPoint.y > on3dPoint2.y) ? on3dPoint.y : on3dPoint2.y);
			OnXform onXform5 = new OnXform();
			onXform5.Scale(1.0, profileHeight / num, 1.0);
			profile.Transform(onXform5);
			OnCurve onCurve = profile.Duplicate();
			onCurve.Trim(new OnInterval(profile.Domain().Min(), param));
			OnCurve onCurve2 = profile.Duplicate();
			onCurve2.Trim(new OnInterval(param2, profile.Domain().Max()));
			double x = channelWallThicknessInner + (profileWidth - channelWallThicknessInner - channelWallThicknessOuter) / 2.0;
			double num2 = profileHeight - gemSizeZ + channelBottomOffset;
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
			On3dPoint on3dPoint3 = new On3dPoint(x, num2, 0.0);
			OnCurve onCurve9;
			OnCurve onCurve10;
			if (channelWallDepth > cornerBlend * 2.0)
			{
				On3dPoint on3dPoint4 = onCurve.PointAtEnd();
				OnCurve onCurve3 = new OnLineCurve(on3dPoint4, new On3dPoint(on3dPoint4.x, on3dPoint4.y - channelWallDepth, 0.0));
				OnCurve onCurve4 = new OnLineCurve(new On3dPoint(on3dPoint4.x, on3dPoint4.y - channelWallDepth, 0.0), on3dPoint3);
				CSUtilGeometry.TrimEndOfCrv(onCurve3, cornerBlend);
				CSUtilGeometry.TrimStartOfCrv(onCurve4, cornerBlend);
				OnCurve onCurve5 = CSUtilCurves.BlendCurve(onCurve3, onCurve4);
				On3dPoint on3dPoint5 = onCurve2.PointAtStart();
				OnCurve onCurve6 = new OnLineCurve(on3dPoint3, new On3dPoint(on3dPoint5.x, on3dPoint5.y - channelWallDepth, 0.0));
				OnCurve onCurve7 = new OnLineCurve(new On3dPoint(on3dPoint5.x, on3dPoint5.y - channelWallDepth, 0.0), on3dPoint5);
				CSUtilGeometry.TrimEndOfCrv(onCurve6, cornerBlend);
				CSUtilGeometry.TrimStartOfCrv(onCurve7, cornerBlend);
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
				onCurve9 = new OnLineCurve(onCurve.PointAtEnd(), on3dPoint3);
				onCurve10 = new OnLineCurve(on3dPoint3, onCurve2.PointAtStart());
			}
			CSUtilGeometry.TrimEndOfCrv(onCurve, cornerBlend);
			CSUtilGeometry.TrimStartAndEndOfCrv(onCurve9, cornerBlend);
			CSUtilGeometry.TrimStartAndEndOfCrv(onCurve10, cornerBlend);
			CSUtilGeometry.TrimStartOfCrv(onCurve2, cornerBlend);
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
			channelProfile = output2[0];
		}
		else
		{
			OnXform onXform6 = new OnXform();
			onXform6.Scale(1.0, profileHeight, 1.0);
			profile.Transform(onXform6);
			channelProfile = profile;
		}
		OnXform onXform7 = new OnXform();
		onXform7.Translation(0.0, 0.0 - profileHeight, 0.0);
		OnXform onXform8 = new OnXform();
		onXform8.Rotation(angle, OnUtil.On_zaxis, OnUtil.On_origin);
		channelProfile.Transform(onXform8 * onXform7);
		return true;
	}

	private static bool MakeClusterMetal(CSGemData centerGem, CSData centerObject, bool centerGemShapeOverrideEnabled, CSGemData centerGemShapeOverride, double centerGemSpacing, IOnCurve channelProfile, double placement, bool doScaleToGemXY, ValueListItem centerGemShapeOverrideMode, double blendCornerInside, double blendCornerOutside, double offsetX, double offsetY, Point3d inputCorner1, Point3d inputCorner2, double rotate, out List<OnBrep> outputMetals, out List<OnXform> outputTransforms, out Point3d emeraldCorner1Point, out Point3d emeraldCorner2Point, out string outError)
	{
		outputMetals = new List<OnBrep>();
		outputTransforms = new List<OnXform>();
		emeraldCorner1Point = Point3d.Unset;
		emeraldCorner2Point = Point3d.Unset;
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
			double num5 = centerGemShapeOverride.GemXSize / 2.0;
			optionalEmeraldCornerXPercent = inputCorner1.X / num5;
			double num6 = centerGemShapeOverride.GemYSize / 2.0;
			optionalEmeraldCornerYPercent = inputCorner2.Y / num6;
		}
		if (!GemOffsetCurve.GemOffset(gemShape, centerGemShapeOverride, centerGemSpacing, out OnCurve curve, false, out string error, blendCornerInside, optionalEmeraldCornerXPercent, optionalEmeraldCornerYPercent))
		{
			outError = "CurveTop Inside:" + error;
			return false;
		}
		OnNurbsCurve nurbs_curve = new OnNurbsCurve();
		curve.GetNurbForm(ref nurbs_curve);
		OnNurbsCurve nurbs_curve2 = new OnNurbsCurve();
		channelProfile.GetNurbForm(ref nurbs_curve2);
		OnNurbsSurface onNurbsSurface = new OnNurbsSurface();
		onNurbsSurface.Create(3, is_rat: true, nurbs_curve2.m_order, nurbs_curve.m_order, nurbs_curve2.m_cv_count, nurbs_curve.m_cv_count);
		bool flag = false;
		List<IOnCurve> list = new List<IOnCurve>();
		CSGemData gem = (doScaleToGemXY ? cSGemData : centerGemShapeOverride);
		double y = curve.PointAtStart().y;
		OnCurve source = curve;
		for (int i = 0; i < nurbs_curve2.CVCount(); i++)
		{
			On3dPoint vertex_value = new On3dPoint();
			nurbs_curve2.GetCV(i, ref vertex_value);
			double num7 = (double)i / (double)(nurbs_curve2.CVCount() - 1);
			if (i <= nurbs_curve2.CVCount() / 10)
			{
				double num8 = 2.0 * (double)nurbs_curve2.CVCount() / 10.0;
				if (!GemOffsetCurve.GemOffset(gemShape, gem, centerGemSpacing + vertex_value.x, out curve, false, out error, blendCornerInside * (0.5 + (double)i / num8) + blendCornerOutside * (1.0 - (0.5 + (double)i / num8)), optionalEmeraldCornerXPercent, optionalEmeraldCornerYPercent))
				{
					outError = "CurveTop Inside:" + error;
					return false;
				}
			}
			else if (i <= nurbs_curve2.CVCount() / 2)
			{
				if (!GemOffsetCurve.GemOffset(gemShape, gem, centerGemSpacing + vertex_value.x, out curve, false, out error, blendCornerInside, optionalEmeraldCornerXPercent, optionalEmeraldCornerYPercent))
				{
					outError = "CurveTop Inside:" + error;
					return false;
				}
			}
			else if (i < 9 * nurbs_curve2.CVCount() / 10)
			{
				if (!GemOffsetCurve.GemOffset(gemShape, gem, centerGemSpacing + vertex_value.x, out curve, false, out error, blendCornerOutside, optionalEmeraldCornerXPercent, optionalEmeraldCornerYPercent))
				{
					outError = "CurveTop Inside:" + error;
					return false;
				}
			}
			else if (!GemOffsetCurve.GemOffset(gemShape, gem, centerGemSpacing + vertex_value.x, out curve, false, out error, blendCornerInside * num7 * 0.5 + blendCornerOutside * (1.0 - num7 * 0.5), optionalEmeraldCornerXPercent, optionalEmeraldCornerYPercent))
			{
				outError = "CurveTop Inside:" + error;
				return false;
			}
			curve.Translate(new On3dVector(on3dVector) * (placement + vertex_value.y));
			list.Add(curve);
			if (curve.PointAtStart().y > y)
			{
				y = curve.PointAtStart().y;
				source = curve;
			}
			OnNurbsCurve nurbs_curve3 = new OnNurbsCurve();
			curve.GetNurbForm(ref nurbs_curve3);
			if (nurbs_curve.CVCount() != nurbs_curve3.CVCount())
			{
				flag = true;
			}
			if (!flag)
			{
				for (int j = 0; j < nurbs_curve.CVCount(); j++)
				{
					On3dPoint vertex_value2 = new On3dPoint();
					nurbs_curve3.GetCV(j, ref vertex_value2);
					double num9 = nurbs_curve2.Weight(i) * nurbs_curve3.Weight(j);
					onNurbsSurface.SetCV(i, j, new On4dPoint(vertex_value2.x * num9, vertex_value2.y * num9, vertex_value2.z * num9, num9));
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
			for (int k = 0; k < nurbs_curve2.KnotCount(); k++)
			{
				onNurbsSurface.SetKnot(0, k, nurbs_curve2.Knot(k));
			}
			for (int l = 0; l < nurbs_curve.KnotCount(); l++)
			{
				onNurbsSurface.SetKnot(1, l, nurbs_curve.Knot(l));
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
			OnBrep brep3 = CSUtilSweep.Loft(list.ToArray(), IArgsRhinoLoft.eLoftType.ltLoose);
			if (brep3 != null)
			{
				brep3.Transform(xUp.Convert());
				CSUtilGeometry.FixBrepSolidOrientation(ref brep3);
				outputMetals.Add(brep3);
				outputTransforms.Add(xUp.Convert());
			}
		}
		outError = string.Empty;
		return true;
	}
}
