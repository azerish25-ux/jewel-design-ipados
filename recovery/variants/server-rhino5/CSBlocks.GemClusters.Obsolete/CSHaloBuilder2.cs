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
using Rhino;
using Rhino.Geometry;
using Rhino.Runtime;

namespace CSBlocks.GemClusters.Obsolete;

[Obsolete]
[CSBlock("HaloBuilder_V2", "{4921566C-AEF6-487B-BBB0-19FE609B063D}")]
public sealed class CSHaloBuilder2 : CSBlock
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

	public CSInputChannel InBlendCornerInside { get; }

	public CSInputChannel InOffsetX { get; }

	public CSInputChannel InOffsetY { get; }

	public CSInputChannel InEmeraldCornerPoint1 { get; }

	public CSInputChannel InEmeraldCornerPoint2 { get; }

	public CSInputChannel InRotate { get; }

	public CSInputChannel InBlendCornerOutside { get; }

	public CSInputChannel InToggleBlendedCorner { get; }

	public CSOutputChannel OutMetal { get; }

	public CSOutputChannel OutEmeraldCorner1Point { get; }

	public CSOutputChannel OutEmeraldCorner2Point { get; }

	public CSHaloBuilder2(string identifier)
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
		InBlendCornerInside = new CSInputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual, 0.0);
		InMetalToggle = new CSInputChannel(this, ChannelDataType.Boolean, ChannelArrayType.Individual, defaultValue: true);
		InOffsetX = new CSInputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual, 0.0);
		InOffsetY = new CSInputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual, 0.0);
		InEmeraldCornerPoint1 = new CSInputChannel(this, ChannelDataType.Point, ChannelArrayType.Individual);
		InEmeraldCornerPoint2 = new CSInputChannel(this, ChannelDataType.Point, ChannelArrayType.Individual);
		InRotate = new CSInputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual, 0);
		InBlendCornerOutside = new CSInputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual, 0.0);
		InToggleBlendedCorner = new CSInputChannel(this, ChannelDataType.Boolean, ChannelArrayType.Individual, defaultValue: false);
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
		value9 = CSBlockUtil.CSUtilMath.ToRadians(value9);
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
		if (!InBlendCornerInside.GetValue(out double value14))
		{
			return InputError();
		}
		if (!InBlendCornerOutside.GetValue(out double value15))
		{
			return InputError();
		}
		if (!InToggleBlendedCorner.GetValue(out bool value16))
		{
			return InputError();
		}
		if (!value16)
		{
			value15 = 0.0;
			value14 = 0.0;
		}
		double value17 = 0.0;
		InOffsetFix.GetDouble(ref value17);
		value3 += value17;
		bool value18 = true;
		InShowChannel.GetBoolean(ref value18);
		bool value19 = false;
		InXYScaleToGem.GetBoolean(ref value19);
		double value20 = 0.0;
		InInnerWallExtra.GetDouble(ref value20);
		double value21 = 0.0;
		InOuterWallExtra.GetDouble(ref value21);
		OnNurbsCurve profile = (value18 ? InProfile2.GetNurbsCurve().Duplicate() : InProfile.GetNurbsCurve().Duplicate());
		ValueListItem whichItem = ValueListItem.GemClusterCenterShapeOverrideModeDimensionSmallest;
		InCenterGemShapeOverrideMode.GetValueListItem(ref whichItem);
		CSDataArray flattenArray2 = InRailX.GetFlattenArray();
		double value22 = 0.0;
		if (flattenArray2.GetArrayCount() >= 1)
		{
			flattenArray2.GetArrayItem(0).GetDouble(ref value22);
		}
		value3 += value22;
		CSDataArray flattenArray3 = InRailZ.GetFlattenArray();
		double value23 = 0.0;
		if (flattenArray3.GetArrayCount() >= 1)
		{
			flattenArray3.GetArrayItem(0).GetDouble(ref value23);
		}
		value10 += value23;
		InOffsetX.GetValue(out double value24);
		InOffsetY.GetValue(out double value25);
		Point3d value26 = Point3d.Unset;
		Point3d value27 = Point3d.Unset;
		if (InEmeraldCornerPoint1.Data != null && InEmeraldCornerPoint2.Data != null)
		{
			CSDataHelper.GetPoint(InEmeraldCornerPoint1.Data, out value26);
			CSDataHelper.GetPoint(InEmeraldCornerPoint2.Data, out value27);
		}
		InRotate.GetValue(out double value28);
		if (!MakeCluster(cSGemData, centerObject, value2, centerGemShapeOverride, value3, profile, value4, value5, value6, value7, value8, value9, value10, value11, value12, value18, value19, whichItem, value20, value21, value13, value14, value15, value24, value25, value26, value27, value28, out var outputMetals, out var outputTransforms, out var emeraldCorner1Point, out var emeraldCorner2Point, out var outError))
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
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0006: Expected O, but got Unknown
		//IL_003d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0043: Expected O, but got Unknown
		ArrayOnX_EVENT val = new ArrayOnX_EVENT();
		c1.IntersectCurve(c2, ref val, CSApplicationHelper.Application.Tolerance);
		if (val.Count() == 0)
		{
			return false;
		}
		double num = -999.0;
		foreach (OnX_EVENT item in val)
		{
			OnX_EVENT val2 = item;
			if (val2[0].y > num)
			{
				num = val2[0].y;
				param = val2[0];
			}
		}
		return true;
	}

	private static bool MakeCluster(CSGemData centerGem, CSData centerObject, bool centerGemShapeOverrideEnabled, CSGemData centerGemShapeOverride, double centerGemSpacing, OnNurbsCurve profile, double profileWidth, double profileHeight, double channelWallThicknessInner, double channelWallThicknessOuter, double channelWallDepth, double angle, double placement, double cornerBlend, double gemSizeZ, bool showChannel, bool doScaleToGemXY, ValueListItem centerGemShapeOverrideMode, double innerWallExtra, double outerWallExtra, double channelBottomOffset, double blendCornerInside, double blendCornerOutside, double offsetX, double offsetY, Point3d inputCorner1, Point3d inputCorner2, double rotate, out List<OnBrep> outputMetals, out List<OnXform> outputTransforms, out Point3d emeraldCorner1Point, out Point3d emeraldCorner2Point, out string outError)
	{
		//IL_0028: Unknown result type (might be due to invalid IL or missing references)
		//IL_002e: Expected O, but got Unknown
		//IL_0038: Unknown result type (might be due to invalid IL or missing references)
		//IL_003e: Expected O, but got Unknown
		//IL_0065: Unknown result type (might be due to invalid IL or missing references)
		//IL_006b: Expected O, but got Unknown
		//IL_00bc: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c2: Expected O, but got Unknown
		//IL_00e2: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e9: Expected O, but got Unknown
		//IL_05a8: Unknown result type (might be due to invalid IL or missing references)
		//IL_05af: Expected O, but got Unknown
		//IL_013c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0155: Unknown result type (might be due to invalid IL or missing references)
		//IL_015f: Expected O, but got Unknown
		//IL_015f: Expected O, but got Unknown
		//IL_015a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0161: Expected O, but got Unknown
		//IL_0178: Unknown result type (might be due to invalid IL or missing references)
		//IL_0194: Unknown result type (might be due to invalid IL or missing references)
		//IL_019e: Expected O, but got Unknown
		//IL_019e: Expected O, but got Unknown
		//IL_0199: Unknown result type (might be due to invalid IL or missing references)
		//IL_01a0: Expected O, but got Unknown
		//IL_05d8: Unknown result type (might be due to invalid IL or missing references)
		//IL_05df: Expected O, but got Unknown
		//IL_05fb: Unknown result type (might be due to invalid IL or missing references)
		//IL_0602: Expected O, but got Unknown
		//IL_021c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0223: Expected O, but got Unknown
		//IL_0264: Unknown result type (might be due to invalid IL or missing references)
		//IL_026e: Expected O, but got Unknown
		//IL_0288: Unknown result type (might be due to invalid IL or missing references)
		//IL_0292: Expected O, but got Unknown
		//IL_094d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0954: Expected O, but got Unknown
		//IL_0301: Unknown result type (might be due to invalid IL or missing references)
		//IL_0308: Expected O, but got Unknown
		//IL_049a: Unknown result type (might be due to invalid IL or missing references)
		//IL_04a1: Expected O, but got Unknown
		//IL_04aa: Unknown result type (might be due to invalid IL or missing references)
		//IL_04b1: Expected O, but got Unknown
		//IL_0340: Unknown result type (might be due to invalid IL or missing references)
		//IL_034a: Expected O, but got Unknown
		//IL_0345: Unknown result type (might be due to invalid IL or missing references)
		//IL_034c: Expected O, but got Unknown
		//IL_0366: Unknown result type (might be due to invalid IL or missing references)
		//IL_0372: Expected O, but got Unknown
		//IL_036d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0374: Expected O, but got Unknown
		//IL_03d4: Unknown result type (might be due to invalid IL or missing references)
		//IL_03de: Expected O, but got Unknown
		//IL_03d9: Unknown result type (might be due to invalid IL or missing references)
		//IL_03e0: Expected O, but got Unknown
		//IL_03fa: Unknown result type (might be due to invalid IL or missing references)
		//IL_0406: Expected O, but got Unknown
		//IL_0401: Unknown result type (might be due to invalid IL or missing references)
		//IL_0408: Expected O, but got Unknown
		//IL_0a9d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0aa4: Expected O, but got Unknown
		//IL_0aae: Unknown result type (might be due to invalid IL or missing references)
		//IL_0ab5: Expected O, but got Unknown
		//IL_0abf: Unknown result type (might be due to invalid IL or missing references)
		//IL_0ac6: Expected O, but got Unknown
		//IL_0b1b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0b22: Expected O, but got Unknown
		//IL_0caf: Unknown result type (might be due to invalid IL or missing references)
		//IL_0cc3: Expected O, but got Unknown
		//IL_0cf4: Unknown result type (might be due to invalid IL or missing references)
		//IL_0cfb: Expected O, but got Unknown
		//IL_0e64: Unknown result type (might be due to invalid IL or missing references)
		//IL_0e6b: Expected O, but got Unknown
		//IL_0d21: Unknown result type (might be due to invalid IL or missing references)
		//IL_0d28: Expected O, but got Unknown
		//IL_0d6f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0d79: Expected O, but got Unknown
		//IL_0ec7: Unknown result type (might be due to invalid IL or missing references)
		//IL_0ece: Expected O, but got Unknown
		outputMetals = new List<OnBrep>();
		outputTransforms = new List<OnXform>();
		emeraldCorner1Point = Point3d.Unset;
		emeraldCorner2Point = Point3d.Unset;
		OnBoundingBox val = new OnBoundingBox();
		((OnGeometry)profile).GetTightBoundingBox(ref val);
		OnXform val2 = new OnXform();
		val2.Translation(0.0 - val.Min().x, 0.0 - val.Min().y, 0.0);
		OnXform val3 = new OnXform();
		val3.Scale(1.0 / (val.Max().x - val.Min().x), 1.0 / (val.Max().y - val.Min().y), 0.0);
		OnXform val4 = new OnXform();
		val4.Scale(profileWidth + innerWallExtra + outerWallExtra, 1.0, 1.0);
		OnXform val5 = new OnXform();
		val5.Translation(0.0 - innerWallExtra, 0.0, 0.0);
		profile.Transform((IOnXform)(object)(val5 * (IOnXform)(object)val4 * (IOnXform)(object)val3 * (IOnXform)(object)val2));
		OnCurve val25;
		if (showChannel)
		{
			OnLineCurve c = new OnLineCurve((IOn3dPoint)new On3dPoint(channelWallThicknessInner, -1.0, 0.0), (IOn3dPoint)new On3dPoint(channelWallThicknessInner, 2.0, 0.0));
			OnLineCurve c2 = new OnLineCurve((IOn3dPoint)new On3dPoint(profileWidth - channelWallThicknessOuter, -1.0, 0.0), (IOn3dPoint)new On3dPoint(profileWidth - channelWallThicknessOuter, 2.0, 0.0));
			double param = 0.0;
			double param2 = 0.0;
			if (!CurveIntersectHighest((IOnCurve)(object)profile, (IOnCurve)(object)c, ref param))
			{
				outError = "Intersect for channel Inner Failed";
				return false;
			}
			if (!CurveIntersectHighest((IOnCurve)(object)profile, (IOnCurve)(object)c2, ref param2))
			{
				outError = "Intersect for channel Failed";
				return false;
			}
			On3dPoint val6 = ((OnCurve)profile).PointAt(param);
			On3dPoint val7 = ((OnCurve)profile).PointAt(param2);
			double num = ((val6.y > val7.y) ? val6.y : val7.y);
			OnXform val8 = new OnXform();
			val8.Scale(1.0, profileHeight / num, 1.0);
			profile.Transform((IOnXform)(object)val8);
			OnCurve val9 = (OnCurve)(object)profile.Duplicate();
			val9.Trim((IOnInterval)new OnInterval(((OnCurve)profile).Domain().Min(), param));
			OnCurve val10 = (OnCurve)(object)profile.Duplicate();
			val10.Trim((IOnInterval)new OnInterval(param2, ((OnCurve)profile).Domain().Max()));
			double num2 = channelWallThicknessInner + (profileWidth - channelWallThicknessInner - channelWallThicknessOuter) / 2.0;
			double num3 = profileHeight - gemSizeZ + channelBottomOffset;
			bool num4 = num3 > val9.PointAtEnd().y - channelWallDepth;
			bool flag = 0.0 > num3;
			if (num4)
			{
				num3 = val9.PointAtEnd().y - channelWallDepth;
			}
			if (flag)
			{
				num3 = 0.0;
			}
			On3dPoint val11 = new On3dPoint(num2, num3, 0.0);
			OnCurve val20;
			OnCurve val21;
			if (channelWallDepth > cornerBlend * 2.0)
			{
				On3dPoint val12 = val9.PointAtEnd();
				OnCurve val13 = (OnCurve)new OnLineCurve((IOn3dPoint)(object)val12, (IOn3dPoint)new On3dPoint(val12.x, val12.y - channelWallDepth, 0.0));
				OnCurve val14 = (OnCurve)new OnLineCurve((IOn3dPoint)new On3dPoint(val12.x, val12.y - channelWallDepth, 0.0), (IOn3dPoint)(object)val11);
				CSUtilGeometry.TrimEndOfCrv(val13, cornerBlend);
				CSUtilGeometry.TrimStartOfCrv(val14, cornerBlend);
				OnCurve val15 = (OnCurve)(object)CSUtilCurves.BlendCurve((IOnCurve)(object)val13, (IOnCurve)(object)val14);
				On3dPoint val16 = val10.PointAtStart();
				OnCurve val17 = (OnCurve)new OnLineCurve((IOn3dPoint)(object)val11, (IOn3dPoint)new On3dPoint(val16.x, val16.y - channelWallDepth, 0.0));
				OnCurve val18 = (OnCurve)new OnLineCurve((IOn3dPoint)new On3dPoint(val16.x, val16.y - channelWallDepth, 0.0), (IOn3dPoint)(object)val16);
				CSUtilGeometry.TrimEndOfCrv(val17, cornerBlend);
				CSUtilGeometry.TrimStartOfCrv(val18, cornerBlend);
				OnCurve val19 = (OnCurve)(object)CSUtilCurves.BlendCurve((IOnCurve)(object)val17, (IOnCurve)(object)val18);
				IOnCurve[] obj = new IOnCurve[3]
				{
					(IOnCurve)val13,
					(IOnCurve)val15,
					(IOnCurve)val14
				};
				OnCurve[] array = null;
				RhUtil.RhinoMergeCurves((IOnCurve[])(object)obj, ref array);
				val20 = array[0];
				IOnCurve[] obj2 = new IOnCurve[3]
				{
					(IOnCurve)val17,
					(IOnCurve)val19,
					(IOnCurve)val18
				};
				array = null;
				RhUtil.RhinoMergeCurves((IOnCurve[])(object)obj2, ref array);
				val21 = array[0];
			}
			else
			{
				val20 = (OnCurve)new OnLineCurve((IOn3dPoint)(object)val9.PointAtEnd(), (IOn3dPoint)(object)val11);
				val21 = (OnCurve)new OnLineCurve((IOn3dPoint)(object)val11, (IOn3dPoint)(object)val10.PointAtStart());
			}
			CSUtilGeometry.TrimEndOfCrv(val9, cornerBlend);
			CSUtilGeometry.TrimStartAndEndOfCrv(val20, cornerBlend);
			CSUtilGeometry.TrimStartAndEndOfCrv(val21, cornerBlend);
			CSUtilGeometry.TrimStartOfCrv(val10, cornerBlend);
			OnCurve val22 = (OnCurve)(object)CSUtilCurves.BlendCurve((IOnCurve)(object)val9, (IOnCurve)(object)val20);
			OnCurve val23 = (OnCurve)(object)CSUtilCurves.BlendCurve((IOnCurve)(object)val20, (IOnCurve)(object)val21);
			OnCurve val24 = (OnCurve)(object)CSUtilCurves.BlendCurve((IOnCurve)(object)val21, (IOnCurve)(object)val10);
			IOnCurve[] obj3 = new IOnCurve[7]
			{
				(IOnCurve)val9,
				(IOnCurve)val22,
				(IOnCurve)val20,
				(IOnCurve)val23,
				(IOnCurve)val21,
				(IOnCurve)val24,
				(IOnCurve)val10
			};
			OnCurve[] array2 = null;
			if (!RhUtil.RhinoMergeCurves((IOnCurve[])(object)obj3, ref array2))
			{
				outError = "channel Merge failed";
				return false;
			}
			if (array2.Length != 1)
			{
				outError = "channel Merge failed";
				return false;
			}
			val25 = array2[0];
		}
		else
		{
			OnXform val26 = new OnXform();
			val26.Scale(1.0, profileHeight, 1.0);
			profile.Transform((IOnXform)(object)val26);
			val25 = (OnCurve)(object)profile;
		}
		OnXform val27 = new OnXform();
		val27.Translation(0.0, 0.0 - profileHeight, 0.0);
		OnXform val28 = new OnXform();
		val28.Rotation(angle, OnUtil.On_zaxis, OnUtil.On_origin);
		((OnGeometry)val25).Transform((IOnXform)(object)(val28 * (IOnXform)(object)val27));
		BoundingBox boundingBox = BoundingBox.Unset;
		centerObject.GetTightBoundingBox(ref boundingBox);
		double num5 = Math.Abs(boundingBox.Max.X - boundingBox.Min.X + offsetX);
		double num6 = Math.Abs(boundingBox.Max.Y - boundingBox.Min.Y + offsetY);
		string gemShape;
		if (centerGemShapeOverrideEnabled)
		{
			if (centerGemShapeOverride == null)
			{
				centerGemShapeOverride = new CSGemData(centerGem);
			}
			else
			{
				double num7;
				double num8;
				if (centerGem != null)
				{
					switch (centerGemShapeOverrideMode)
					{
					case ValueListItem.GemClusterCenterShapeOverrideModeDimensionX:
						num7 = (centerGem.GemXSize + offsetX) / centerGemShapeOverride.GemXSize;
						num8 = num7;
						break;
					case ValueListItem.GemClusterCenterShapeOverrideModeDimensionY:
						num8 = (centerGem.GemYSize + offsetY) / centerGemShapeOverride.GemYSize;
						num7 = num8;
						break;
					case ValueListItem.GemClusterCenterShapeOverrideModeDimensionLargest:
						if (centerGem.GemXSize > centerGem.GemYSize)
						{
							num7 = (centerGem.GemXSize + offsetX) / centerGemShapeOverride.GemXSize;
							num8 = num7;
						}
						else
						{
							num8 = (centerGem.GemYSize + offsetY) / centerGemShapeOverride.GemYSize;
							num7 = num8;
						}
						break;
					case ValueListItem.GemClusterCenterShapeOverrideModeDimensionSmallest:
						if (centerGem.GemXSize < centerGem.GemYSize)
						{
							num7 = (centerGem.GemXSize + offsetX) / centerGemShapeOverride.GemXSize;
							num8 = num7;
						}
						else
						{
							num8 = (centerGem.GemYSize + offsetY) / centerGemShapeOverride.GemYSize;
							num7 = num8;
						}
						break;
					case ValueListItem.GemClusterCenterShapeOverrideModeDimensionXandY:
						num7 = (centerGem.GemXSize + offsetX) / centerGemShapeOverride.GemXSize;
						num8 = (centerGem.GemYSize + offsetY) / centerGemShapeOverride.GemYSize;
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
						num7 = num5 / centerGemShapeOverride.GemXSize;
						num8 = num7;
						break;
					case ValueListItem.GemClusterCenterShapeOverrideModeDimensionY:
						num8 = num6 / centerGemShapeOverride.GemYSize;
						num7 = num8;
						break;
					case ValueListItem.GemClusterCenterShapeOverrideModeDimensionLargest:
						if (num5 > num6)
						{
							num7 = num5 / centerGemShapeOverride.GemXSize;
							num8 = num7;
						}
						else
						{
							num8 = num6 / centerGemShapeOverride.GemYSize;
							num7 = num8;
						}
						break;
					case ValueListItem.GemClusterCenterShapeOverrideModeDimensionSmallest:
						if (num5 < num6)
						{
							num7 = num5 / centerGemShapeOverride.GemXSize;
							num8 = num7;
						}
						else
						{
							num8 = num6 / centerGemShapeOverride.GemYSize;
							num7 = num8;
						}
						break;
					case ValueListItem.GemClusterCenterShapeOverrideModeDimensionXandY:
						num7 = num5 / centerGemShapeOverride.GemXSize;
						num8 = num6 / centerGemShapeOverride.GemYSize;
						break;
					default:
						outError = "Invalid Gem Cluster Center Shape Override Mode";
						return false;
					}
				}
				centerGemShapeOverride.Scale(num7, num8);
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
		On3dVector val29;
		if (centerGem != null)
		{
			cSGemData = new CSGemData(centerGem);
			cSGemData.ApplyTransform(inverseTransform);
			val29 = new On3dVector((IOn3dVector)(object)cSGemData.Up.Convert());
			val29.Unitize();
		}
		else
		{
			Point3d point3d = new Point3d(boundingBox.Center.X, boundingBox.Center.Y, 0.0);
			Point3d point3d2 = point3d;
			point3d2.Transform(Transform.Translation(0.0, 0.0, boundingBox.Center.Z));
			Vector3d value = new Vector3d((point3d.Z >= point3d2.Z) ? (point3d - point3d2) : (point3d2 - point3d));
			value.Unitize();
			val29 = CSConvert.Vector3dCopyToOn3dVector(value);
		}
		double optionalEmeraldCornerXPercent = 0.0;
		double optionalEmeraldCornerYPercent = 0.0;
		if (inputCorner1 != Point3d.Unset && inputCorner2 != Point3d.Unset)
		{
			double num9 = centerGemShapeOverride.GemXSize / 2.0;
			optionalEmeraldCornerXPercent = inputCorner1.X / num9;
			double num10 = centerGemShapeOverride.GemYSize / 2.0;
			optionalEmeraldCornerYPercent = inputCorner2.Y / num10;
		}
		if (!GemOffsetCurveV2.GemOffset(gemShape, centerGemShapeOverride, centerGemSpacing, out OnCurve curve, false, out string error, blendCornerInside, optionalEmeraldCornerXPercent, optionalEmeraldCornerYPercent))
		{
			outError = "CurveTop Inside:" + error;
			return false;
		}
		OnNurbsCurve val30 = new OnNurbsCurve();
		curve.GetNurbForm(ref val30);
		OnNurbsCurve val31 = new OnNurbsCurve();
		val25.GetNurbForm(ref val31);
		OnNurbsSurface val32 = new OnNurbsSurface();
		val32.Create(3, true, val31.m_order, val30.m_order, val31.m_cv_count, val30.m_cv_count);
		bool flag2 = false;
		List<IOnCurve> list = new List<IOnCurve>();
		CSGemData gem = (doScaleToGemXY ? cSGemData : centerGemShapeOverride);
		double y = curve.PointAtStart().y;
		OnCurve source = curve;
		for (int i = 0; i < val31.CVCount(); i++)
		{
			On3dPoint val33 = new On3dPoint();
			val31.GetCV(i, ref val33);
			double num11 = (double)i / (double)(val31.CVCount() - 1);
			if (i <= val31.CVCount() / 10)
			{
				double num12 = 2.0 * (double)val31.CVCount() / 10.0;
				if (!GemOffsetCurveV2.GemOffset(gemShape, gem, centerGemSpacing + val33.x, out curve, false, out error, blendCornerInside * (0.5 + (double)i / num12) + blendCornerOutside * (1.0 - (0.5 + (double)i / num12)), optionalEmeraldCornerXPercent, optionalEmeraldCornerYPercent))
				{
					outError = "CurveTop Inside:" + error;
					return false;
				}
			}
			else if (i <= val31.CVCount() / 2)
			{
				if (!GemOffsetCurveV2.GemOffset(gemShape, gem, centerGemSpacing + val33.x, out curve, false, out error, blendCornerInside, optionalEmeraldCornerXPercent, optionalEmeraldCornerYPercent))
				{
					outError = "CurveTop Inside:" + error;
					return false;
				}
			}
			else if (i < 9 * val31.CVCount() / 10)
			{
				if (!GemOffsetCurveV2.GemOffset(gemShape, gem, centerGemSpacing + val33.x, out curve, false, out error, blendCornerOutside, optionalEmeraldCornerXPercent, optionalEmeraldCornerYPercent))
				{
					outError = "CurveTop Inside:" + error;
					return false;
				}
			}
			else if (!GemOffsetCurveV2.GemOffset(gemShape, gem, centerGemSpacing + val33.x, out curve, false, out error, blendCornerInside * num11 * 0.5 + blendCornerOutside * (1.0 - num11 * 0.5), optionalEmeraldCornerXPercent, optionalEmeraldCornerYPercent))
			{
				outError = "CurveTop Inside:" + error;
				return false;
			}
			((OnGeometry)curve).Translate((IOn3dVector)(object)(new On3dVector((IOn3dVector)(object)val29) * (placement + val33.y)));
			list.Add((IOnCurve)(object)curve);
			if (curve.PointAtStart().y > y)
			{
				y = curve.PointAtStart().y;
				source = curve;
			}
			OnNurbsCurve val34 = new OnNurbsCurve();
			curve.GetNurbForm(ref val34);
			if (val30.CVCount() != val34.CVCount())
			{
				flag2 = true;
			}
			if (!flag2)
			{
				for (int j = 0; j < val30.CVCount(); j++)
				{
					On3dPoint val35 = new On3dPoint();
					val34.GetCV(j, ref val35);
					double num13 = val31.Weight(i) * val34.Weight(j);
					val32.SetCV(i, j, (IOn4dPoint)new On4dPoint(val35.x * num13, val35.y * num13, val35.z * num13, num13));
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
		if (!flag2)
		{
			for (int k = 0; k < val31.KnotCount(); k++)
			{
				val32.SetKnot(0, k, val31.Knot(k));
			}
			for (int l = 0; l < val30.KnotCount(); l++)
			{
				val32.SetKnot(1, l, val30.Knot(l));
			}
			OnBrep brep = new OnBrep();
			((OnGeometry)val32).BrepForm(brep);
			((OnGeometry)brep).Transform((IOnXform)(object)xUp.Convert());
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
			OnBrep brep3 = CSUtilSweep.Loft(list.ToArray(), (eLoftType)1);
			if (brep3 != null)
			{
				((OnGeometry)brep3).Transform((IOnXform)(object)xUp.Convert());
				CSUtilGeometry.FixBrepSolidOrientation(ref brep3);
				outputMetals.Add(brep3);
				outputTransforms.Add(xUp.Convert());
			}
		}
		outError = string.Empty;
		return true;
	}
}
