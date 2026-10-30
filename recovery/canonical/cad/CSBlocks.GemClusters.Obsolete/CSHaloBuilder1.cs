using System;
using System.Collections.Generic;
using CS;
using CS.CSDataUtilities;
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
using Rhino.Geometry;
using Rhino.Runtime;

namespace CSBlocks.GemClusters.Obsolete;

[Obsolete]
[CSBlock("HaloBuilder_V1", "{88465C7C-6E4C-4AFE-98D2-98596E90DA70}")]
public sealed class CSHaloBuilder1 : CSBlock
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

	public CSInputChannel InChannelSpacing { get; }

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

	public CSInputChannel InBlendCornerGemShape { get; }

	public CSInputChannel InOffsetX { get; }

	public CSInputChannel InOffsetY { get; }

	public CSInputChannel InEmeraldCornerPoint1 { get; }

	public CSInputChannel InEmeraldCornerPoint2 { get; }

	public CSOutputChannel OutMetal { get; }

	public CSOutputChannel OutEmeraldCorner1Point { get; }

	public CSOutputChannel OutEmeraldCorner2Point { get; }

	public CSHaloBuilder1(string identifier)
		: base(identifier)
	{
		InCenterGem = new CSInputChannel(this, ChannelDataType.Gem, ChannelArrayType.Array | ChannelArrayType.Individual);
		InCenterGemShapeOverrideEnabled = new CSInputChannel(this, ChannelDataType.Boolean, ChannelArrayType.Individual);
		InCenterGemShapeOverride = new CSInputChannel(this, ChannelDataType.Gem, ChannelArrayType.Individual);
		InCenterGemSpacing = new CSInputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual);
		InProfile = new CSInputChannel(this, ChannelDataType.Curve, ChannelArrayType.Individual);
		InProfile2 = new CSInputChannel(this, ChannelDataType.Curve, ChannelArrayType.Individual);
		InProfileWidth = new CSInputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual);
		InProfileHeight = new CSInputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual);
		InChannelWallThicknessInner = new CSInputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual);
		InChannelWallThicknessOuter = new CSInputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual);
		InChannelSpacing = new CSInputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual);
		InChannelWallDepth = new CSInputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual);
		InAngle = new CSInputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual);
		InPlacement = new CSInputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual);
		InCornerBlend = new CSInputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual);
		InGemSizeZ = new CSInputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual);
		InShowChannel = new CSInputChannel(this, ChannelDataType.Boolean, ChannelArrayType.Individual);
		InXYScaleToGem = new CSInputChannel(this, ChannelDataType.Boolean, ChannelArrayType.Individual);
		InCenterGemShapeOverrideMode = new CSInputChannel(this, ChannelDataType.Integer, ChannelArrayType.Individual);
		InOffsetFix = new CSInputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual);
		InInnerWallExtra = new CSInputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual);
		InOuterWallExtra = new CSInputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual);
		InRailX = new CSInputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual);
		InRailZ = new CSInputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual);
		InChannelBottomOffset = new CSInputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual, 0.0);
		InBlendCornerGemShape = new CSInputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual, 0.0);
		InMetalToggle = new CSInputChannel(this, ChannelDataType.Boolean, ChannelArrayType.Individual, defaultValue: true);
		InOffsetX = new CSInputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual, 0.0);
		InOffsetY = new CSInputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual, 0.0);
		InEmeraldCornerPoint1 = new CSInputChannel(this, ChannelDataType.Point, ChannelArrayType.Individual);
		InEmeraldCornerPoint2 = new CSInputChannel(this, ChannelDataType.Point, ChannelArrayType.Individual);
		OutMetal = new CSOutputChannel(this, ChannelDataType.Any, ChannelArrayType.Array);
		OutEmeraldCorner1Point = new CSOutputChannel(this, ChannelDataType.Point, ChannelArrayType.Individual);
		OutEmeraldCorner2Point = new CSOutputChannel(this, ChannelDataType.Point, ChannelArrayType.Individual);
		SetChannelIdentifiers();
	}

	public override bool Update()
	{
		InMetalToggle.GetValue(out bool value);
		if (!value)
		{
			OutMetal.SetData(null);
			return base.Update();
		}
		CSDataArray flattenArray = InCenterGem.GetFlattenArray();
		if (flattenArray.GetArrayCount() <= 0)
		{
			return UpdateMissing("CenterGem");
		}
		CSGemData cSGemData = flattenArray.GetArrayItem(0).Duplicate() as CSGemData;
		CSData centerObject = flattenArray.GetArrayItem(0).Duplicate();
		bool value2 = false;
		InCenterGemShapeOverrideEnabled.GetBoolean(ref value2);
		if (cSGemData == null)
		{
			value2 = true;
		}
		CSGemData centerGemShapeOverride = InCenterGemShapeOverride.Data as CSGemData;
		double value3 = 0.0;
		InCenterGemSpacing.GetDouble(ref value3);
		double value4 = 0.0;
		InProfileWidth.GetDouble(ref value4);
		double value5 = 0.0;
		InProfileHeight.GetDouble(ref value5);
		double value6 = 0.0;
		InChannelWallThicknessInner.GetDouble(ref value6);
		double value7 = 0.0;
		InChannelWallThicknessOuter.GetDouble(ref value7);
		double value8 = 0.0;
		InChannelWallDepth.GetDouble(ref value8);
		double value9 = 0.0;
		InAngle.GetDouble(ref value9);
		value9 *= OnUtil.On_DEGREES_TO_RADIANS;
		double value10 = 0.0;
		InPlacement.GetDouble(ref value10);
		double value11 = 0.0;
		InCornerBlend.GetDouble(ref value11);
		double value12 = 0.0;
		InGemSizeZ.GetDouble(ref value12);
		if (!InChannelBottomOffset.GetValue(out double value13))
		{
			return InputError();
		}
		if (!InBlendCornerGemShape.GetValue(out double value14))
		{
			return InputError();
		}
		double value15 = 0.0;
		InOffsetFix.GetDouble(ref value15);
		value3 += value15;
		bool value16 = true;
		InShowChannel.GetBoolean(ref value16);
		bool value17 = false;
		InXYScaleToGem.GetBoolean(ref value17);
		double value18 = 0.0;
		InInnerWallExtra.GetDouble(ref value18);
		double value19 = 0.0;
		InOuterWallExtra.GetDouble(ref value19);
		OnNurbsCurve profile = (value16 ? InProfile2.GetNurbsCurve().Duplicate() : InProfile.GetNurbsCurve().Duplicate());
		ValueListItem whichItem = ValueListItem.GemClusterCenterShapeOverrideModeDimensionSmallest;
		InCenterGemShapeOverrideMode.GetValueListItem(ref whichItem);
		CSDataArray flattenArray2 = InRailX.GetFlattenArray();
		double value20 = 0.0;
		if (flattenArray2.GetArrayCount() >= 1)
		{
			flattenArray2.GetArrayItem(0).GetDouble(ref value20);
		}
		value3 += value20;
		CSDataArray flattenArray3 = InRailZ.GetFlattenArray();
		double value21 = 0.0;
		if (flattenArray3.GetArrayCount() >= 1)
		{
			flattenArray3.GetArrayItem(0).GetDouble(ref value21);
		}
		value10 += value21;
		InOffsetX.GetValue(out double value22);
		InOffsetY.GetValue(out double value23);
		Point3d value24 = Point3d.Unset;
		Point3d value25 = Point3d.Unset;
		if (InEmeraldCornerPoint1.Data != null && InEmeraldCornerPoint2.Data != null)
		{
			CSDataHelper.GetPoint(InEmeraldCornerPoint1.Data, out value24);
			CSDataHelper.GetPoint(InEmeraldCornerPoint2.Data, out value25);
		}
		if (!MakeCluster(cSGemData, centerObject, value2, centerGemShapeOverride, value3, profile, value4, value5, value6, value7, value8, value9, value10, value11, value12, value16, value17, whichItem, value18, value19, value13, value14, value22, value23, value24, value25, out var outputMetals, out var outputTransforms, out var emeraldCorner1Point, out var emeraldCorner2Point, out var outError))
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

	private static bool MakeCluster(CSGemData centerGem, CSData centerObject, bool centerGemShapeOverrideEnabled, CSGemData centerGemShapeOverride, double centerGemSpacing, OnNurbsCurve profile, double profileWidth, double profileHeight, double channelWallThicknessInner, double channelWallThicknessOuter, double channelWallDepth, double angle, double placement, double cornerBlend, double gemSizeZ, bool showChannel, bool doScaleToGemXY, ValueListItem centerGemShapeOverrideMode, double innerWallExtra, double outerWallExtra, double channelBottomOffset, double blendCornerGemShape, double offsetX, double offsetY, Point3d inputCorner1, Point3d inputCorner2, out List<OnBrep> outputMetals, out List<OnXform> outputTransforms, out Point3d emeraldCorner1Point, out Point3d emeraldCorner2Point, out string outError)
	{
		outputMetals = new List<OnBrep>();
		outputTransforms = new List<OnXform>();
		emeraldCorner1Point = Point3d.Unset;
		emeraldCorner2Point = Point3d.Unset;
		OnBoundingBox tight_bbox = new OnBoundingBox();
		profile.GetTightBoundingBox(ref tight_bbox);
		OnXform onXform = new OnXform();
		onXform.Translation(0.0 - tight_bbox.Min().x, 0.0 - tight_bbox.Min().y, 0.0);
		OnXform onXform2 = new OnXform();
		onXform2.Scale(1.0 / (tight_bbox.Max().x - tight_bbox.Min().x), 1.0 / (tight_bbox.Max().y - tight_bbox.Min().y), 0.0);
		OnXform onXform3 = new OnXform();
		onXform3.Scale(profileWidth + innerWallExtra + outerWallExtra, 1.0, 1.0);
		OnXform onXform4 = new OnXform();
		onXform4.Translation(0.0 - innerWallExtra, 0.0, 0.0);
		profile.Transform(onXform4 * onXform3 * onXform2 * onXform);
		OnCurve onCurve14;
		if (showChannel)
		{
			OnLineCurve c = new OnLineCurve(new On3dPoint(channelWallThicknessInner, -1.0, 0.0), new On3dPoint(channelWallThicknessInner, 2.0, 0.0));
			OnLineCurve c2 = new OnLineCurve(new On3dPoint(profileWidth - channelWallThicknessOuter, -1.0, 0.0), new On3dPoint(profileWidth - channelWallThicknessOuter, 2.0, 0.0));
			double param = 0.0;
			double param2 = 0.0;
			if (!CurveIntersectHighest(profile, c, ref param))
			{
				outError = "Intersect for channel Inner Failed";
				return false;
			}
			if (!CurveIntersectHighest(profile, c2, ref param2))
			{
				outError = "Intersect for channel Failed";
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
				outError = "channel Merge failed";
				return false;
			}
			if (output2.Length != 1)
			{
				outError = "channel Merge failed";
				return false;
			}
			onCurve14 = output2[0];
		}
		else
		{
			OnXform onXform6 = new OnXform();
			onXform6.Scale(1.0, profileHeight, 1.0);
			profile.Transform(onXform6);
			onCurve14 = profile;
		}
		OnXform onXform7 = new OnXform();
		onXform7.Translation(0.0, 0.0 - profileHeight, 0.0);
		OnXform onXform8 = new OnXform();
		onXform8.Rotation(angle, OnUtil.On_zaxis, OnUtil.On_origin);
		onCurve14.Transform(onXform8 * onXform7);
		BoundingBox boundingBox = BoundingBox.Unset;
		centerObject.GetTightBoundingBox(ref boundingBox);
		double num4 = Math.Abs(boundingBox.Max.X - boundingBox.Min.X + offsetX);
		double num5 = Math.Abs(boundingBox.Max.Y - boundingBox.Min.Y + offsetY);
		string gemShape;
		if (centerGemShapeOverrideEnabled)
		{
			if (centerGemShapeOverride == null)
			{
				centerGemShapeOverride = new CSGemData(centerGem);
			}
			else
			{
				double num6;
				double num7;
				if (centerGem != null)
				{
					switch (centerGemShapeOverrideMode)
					{
					case ValueListItem.GemClusterCenterShapeOverrideModeDimensionX:
						num6 = (centerGem.GemXSize + offsetX) / centerGemShapeOverride.GemXSize;
						num7 = num6;
						break;
					case ValueListItem.GemClusterCenterShapeOverrideModeDimensionY:
						num7 = (centerGem.GemYSize + offsetY) / centerGemShapeOverride.GemYSize;
						num6 = num7;
						break;
					case ValueListItem.GemClusterCenterShapeOverrideModeDimensionLargest:
						if (centerGem.GemXSize > centerGem.GemYSize)
						{
							num6 = (centerGem.GemXSize + offsetX) / centerGemShapeOverride.GemXSize;
							num7 = num6;
						}
						else
						{
							num7 = (centerGem.GemYSize + offsetY) / centerGemShapeOverride.GemYSize;
							num6 = num7;
						}
						break;
					case ValueListItem.GemClusterCenterShapeOverrideModeDimensionSmallest:
						if (centerGem.GemXSize < centerGem.GemYSize)
						{
							num6 = (centerGem.GemXSize + offsetX) / centerGemShapeOverride.GemXSize;
							num7 = num6;
						}
						else
						{
							num7 = (centerGem.GemYSize + offsetY) / centerGemShapeOverride.GemYSize;
							num6 = num7;
						}
						break;
					case ValueListItem.GemClusterCenterShapeOverrideModeDimensionXandY:
						num6 = (centerGem.GemXSize + offsetX) / centerGemShapeOverride.GemXSize;
						num7 = (centerGem.GemYSize + offsetY) / centerGemShapeOverride.GemYSize;
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
						num6 = num4 / centerGemShapeOverride.GemXSize;
						num7 = num6;
						break;
					case ValueListItem.GemClusterCenterShapeOverrideModeDimensionY:
						num7 = num5 / centerGemShapeOverride.GemYSize;
						num6 = num7;
						break;
					case ValueListItem.GemClusterCenterShapeOverrideModeDimensionLargest:
						if (num4 > num5)
						{
							num6 = num4 / centerGemShapeOverride.GemXSize;
							num7 = num6;
						}
						else
						{
							num7 = num5 / centerGemShapeOverride.GemYSize;
							num6 = num7;
						}
						break;
					case ValueListItem.GemClusterCenterShapeOverrideModeDimensionSmallest:
						if (num4 < num5)
						{
							num6 = num4 / centerGemShapeOverride.GemXSize;
							num7 = num6;
						}
						else
						{
							num7 = num5 / centerGemShapeOverride.GemYSize;
							num6 = num7;
						}
						break;
					case ValueListItem.GemClusterCenterShapeOverrideModeDimensionXandY:
						num6 = num4 / centerGemShapeOverride.GemXSize;
						num7 = num5 / centerGemShapeOverride.GemYSize;
						break;
					default:
						outError = "Invalid Gem Cluster Center Shape Override Mode";
						return false;
					}
				}
				centerGemShapeOverride.Scale(num6, num7);
			}
			gemShape = centerGemShapeOverride.GemShape;
		}
		else
		{
			centerGemShapeOverride = new CSGemData(centerGem);
			gemShape = centerGem.GemShape;
		}
		Transform inverseTransform;
		Transform xDown;
		if (centerGem != null)
		{
			if (!CSBlockUtil.CSUtilMath.GetUnscaledXforms(centerGem.Xform, out inverseTransform, out xDown))
			{
				outError = "Gem X form";
				return false;
			}
		}
		else
		{
			xDown = Transform.Translation(boundingBox.Center.X, boundingBox.Center.Y, boundingBox.Center.Z);
			if (!xDown.TryGetInverse(out inverseTransform))
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
			Point3d origin = Point3d.Origin;
			origin.Transform(Transform.Translation(boundingBox.Center.X, boundingBox.Center.Y, boundingBox.Center.Z));
			Vector3d value = new Vector3d(Point3d.Origin - origin);
			value.Unitize();
			on3dVector = CSConvert.Vector3dCopyToOn3dVector(value);
		}
		double optionalEmeraldCornerXPercent = 0.0;
		double optionalEmeraldCornerYPercent = 0.0;
		if (inputCorner1 != Point3d.Unset && inputCorner2 != Point3d.Unset)
		{
			double num8 = centerGemShapeOverride.GemXSize / 2.0;
			optionalEmeraldCornerXPercent = inputCorner1.X / num8;
			double num9 = centerGemShapeOverride.GemYSize / 2.0;
			optionalEmeraldCornerYPercent = inputCorner2.Y / num9;
		}
		if (!GemOffsetCurveV2.GemOffset(gemShape, centerGemShapeOverride, centerGemSpacing, out OnCurve curve, false, out string error, blendCornerGemShape, optionalEmeraldCornerXPercent, optionalEmeraldCornerYPercent))
		{
			outError = "CurveTop Inside:" + error;
			return false;
		}
		OnNurbsCurve nurbs_curve = new OnNurbsCurve();
		curve.GetNurbForm(ref nurbs_curve);
		OnNurbsCurve nurbs_curve2 = new OnNurbsCurve();
		onCurve14.GetNurbForm(ref nurbs_curve2);
		OnNurbsSurface onNurbsSurface = new OnNurbsSurface();
		onNurbsSurface.Create(3, is_rat: true, nurbs_curve2.m_order, nurbs_curve.m_order, nurbs_curve2.m_cv_count, nurbs_curve.m_cv_count);
		bool flag2 = false;
		List<IOnCurve> list = new List<IOnCurve>();
		CSGemData gem = (doScaleToGemXY ? cSGemData : centerGemShapeOverride);
		double y = curve.PointAtStart().y;
		OnCurve source = curve;
		for (int i = 0; i < nurbs_curve2.CVCount(); i++)
		{
			On3dPoint vertex_value = new On3dPoint();
			nurbs_curve2.GetCV(i, ref vertex_value);
			if (!GemOffsetCurveV2.GemOffset(gemShape, gem, centerGemSpacing + vertex_value.x, out curve, false, out error, blendCornerGemShape, optionalEmeraldCornerXPercent, optionalEmeraldCornerYPercent))
			{
				outError = "CurveTop Inside:" + error;
				return false;
			}
			curve.Translate(new On3dVector(on3dVector) * (placement + vertex_value.y));
			list.Add(curve);
			Curve curve2 = Interop.FromOnCurve(curve);
			if (curve2.PointAtStart.Y > y)
			{
				y = curve2.PointAtStart.Y;
				source = curve;
			}
			OnNurbsCurve nurbs_curve3 = new OnNurbsCurve();
			curve.GetNurbForm(ref nurbs_curve3);
			if (nurbs_curve.CVCount() != nurbs_curve3.CVCount())
			{
				flag2 = true;
			}
			if (!flag2)
			{
				for (int j = 0; j < nurbs_curve.CVCount(); j++)
				{
					On3dPoint vertex_value2 = new On3dPoint();
					nurbs_curve3.GetCV(j, ref vertex_value2);
					double num10 = nurbs_curve2.Weight(i) * nurbs_curve3.Weight(j);
					onNurbsSurface.SetCV(i, j, new On4dPoint(vertex_value2.x * num10, vertex_value2.y * num10, vertex_value2.z * num10, num10));
				}
			}
		}
		if (gemShape == "Emerald")
		{
			Curve curve3 = Interop.FromOnCurve(source);
			curve3.GetNextDiscontinuity(Continuity.C2_continuous, 0.0, 10.0, out var t);
			emeraldCorner1Point = curve3.PointAt(t);
			curve3.GetNextDiscontinuity(Continuity.C2_continuous, t, 20.0, out t);
			emeraldCorner2Point = curve3.PointAt(t);
		}
		if (!flag2)
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
			brep.Transform(xDown.Convert());
			CSUtilGeometry.FixBrepSolidOrientation(ref brep);
			if (!brep.IsSolid())
			{
				Brep brep2 = Interop.FromOnBrep(brep);
				brep2.JoinNakedEdges(CSApplicationHelper.Application.Tolerance);
				if (!brep2.IsSolid)
				{
					brep2.Flip();
				}
				brep = (OnBrep)Interop.ToOnBrep(brep2);
			}
			outputMetals.Add(brep);
			outputTransforms.Add(xDown.Convert());
		}
		else
		{
			OnBrep brep3 = CSUtilSweep.Loft(list.ToArray(), IArgsRhinoLoft.eLoftType.ltLoose);
			if (brep3 != null)
			{
				brep3.Transform(xDown.Convert());
				CSUtilGeometry.FixBrepSolidOrientation(ref brep3);
				outputMetals.Add(brep3);
				outputTransforms.Add(xDown.Convert());
			}
		}
		outError = string.Empty;
		return true;
	}
}
