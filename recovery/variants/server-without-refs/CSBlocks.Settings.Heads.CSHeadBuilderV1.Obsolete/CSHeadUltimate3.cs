using System;
using System.Collections.Generic;
using System.Linq;
using CS;
using CS.CSDataUtilities;
using CS.CSObjects.CSBlocks;
using CS.Parametric;
using CS.Parametric.Channels;
using CSBlockDataUtils.CSGemOffsetCurves;
using CSBlockUtil;
using CSBlocks.UDT;
using CSCompatibility;
using CSDatas;
using CSDatas.Interfaces;
using CSGemInstance;
using GV.CounterSketch.Shared;
using Rhino;
using Rhino.Geometry;
using Rhino.Geometry.Intersect;
using Rhino.Runtime;

namespace CSBlocks.Settings.Heads.CSHeadBuilderV1.Obsolete;

[Obsolete]
[CSBlock("HeadUltimate_V3", "{8E10AFFD-B619-4A79-A746-7CC4C5FBF80F}")]
public sealed class CSHeadUltimate3 : CSBlock
{
	public readonly CSInputChannel InAngleX;

	public readonly CSInputChannel InAngleY;

	public readonly CSInputChannel InBaseAngle;

	public readonly CSInputChannel InBaseAnglePosition;

	public readonly CSInputChannel InBaseObject;

	public readonly CSInputChannel InBaseOffset;

	public readonly CSInputChannel InBasePointRadius;

	public readonly CSInputChannel InBaseStyle;

	public readonly CSInputChannel InBezelPlacement;

	public readonly CSInputChannel InBezelSwitch;

	public readonly CSInputChannel InBuildDomeOnPlane;

	public readonly CSInputChannel InBuildVProngPlanar;

	public readonly CSInputChannel InCamModeOverride;

	public readonly CSInputChannel InConstructionTransform;

	public readonly CSInputChannel InCornerBlend;

	public readonly CSInputChannel InDomeBlendAmountInside;

	public readonly CSInputChannel InDomeBlendAmountOutside;

	public readonly CSInputChannel InDomeBlendEndInside;

	public readonly CSInputChannel InDomeBlendEndOutside;

	public readonly CSInputChannel InDomeBlendStartInside;

	public readonly CSInputChannel InDomeBlendStartOutside;

	public readonly CSInputChannel InEnableDoItYourself;

	public readonly CSInputChannel InFocalPoint;

	public readonly CSInputChannel InGem;

	public readonly CSInputChannel InGirdleSpacing;

	public readonly CSInputChannel InHeadHeight;

	public readonly CSInputChannel InMoveRails;

	public readonly CSInputChannel InOuterSrfSwitch;

	public readonly CSInputChannel InProngArrangement;

	public readonly CSInputChannel InProngBaseBlend;

	public readonly CSInputChannel InProngBaseDomeHeight;

	public readonly CSInputChannel InProngBaseNudge;

	public readonly CSInputChannel InProngBaseProfile;

	public readonly CSInputChannel InProngBaseX;

	public readonly CSInputChannel InProngBaseY;

	public readonly CSInputChannel InProngDomeHeight;

	public readonly CSInputChannel InProngDomeSettingAngle;

	public readonly CSInputChannel InProngDomeSettingDistance;

	public readonly CSInputChannel InProngHeight;

	public readonly CSInputChannel InProngOverlap;

	public readonly CSInputChannel InProngPoint;

	public readonly CSInputChannel InProngPointAngle;

	public readonly CSInputChannel InProngTopBlend;

	public readonly CSInputChannel InProngTopNudge;

	public readonly CSInputChannel InProngTopProfile;

	public readonly CSInputChannel InProngTopX;

	public readonly CSInputChannel InProngTopY;

	public readonly CSInputChannel InProngType;

	public readonly CSInputChannel InPullRailMultiplier;

	public readonly CSInputChannel InPullToBaseObject;

	public readonly CSInputChannel InRailGem;

	public readonly CSInputChannel InRailOffset;

	public readonly CSInputChannel InRailPos;

	public readonly CSInputChannel InRailProfile;

	public readonly CSInputChannel InRailShearMultiplier;

	public readonly CSInputChannel InRailX;

	public readonly CSInputChannel InRailY;

	public readonly CSInputChannel InSeatAngle;

	public readonly CSInputChannel InSeatLength;

	public readonly CSInputChannel InStraightProng;

	public readonly CSInputChannel InToggleSrfBetweenProngs;

	public readonly CSInputChannel InTopAngle;

	public readonly CSInputChannel InUpperRailPosition;

	public readonly CSOutputChannel OutBezel;

	public readonly CSOutputChannel OutCurves;

	public readonly CSOutputChannel OutDisplayCurves;

	public readonly CSOutputChannel OutGem;

	public readonly CSOutputChannel OutHeadArray;

	public readonly CSOutputChannel OutHeads;

	public readonly CSOutputChannel OutInsideSrfBetweenProngs;

	public readonly CSOutputChannel OutLowerDomes;

	public readonly CSOutputChannel OutOriginCurves;

	public readonly CSOutputChannel OutOuterSrfBeweenProngs;

	public readonly CSOutputChannel OutProngBodies;

	public readonly CSOutputChannel OutProngDomes;

	public readonly CSOutputChannel OutTopDomes;

	private bool _camMode;

	public CSHeadUltimate3(string identifier)
		: base(identifier)
	{
		//IL_002e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0484: Unknown result type (might be due to invalid IL or missing references)
		InGem = new CSInputChannel(this, ChannelDataType.Gem, ChannelArrayType.Individual);
		InBaseObject = new CSInputChannel(this, ChannelDataType.Any, ChannelArrayType.Array);
		InConstructionTransform = new CSInputChannel(this, ChannelDataType.Transform, ChannelArrayType.Individual, Transform.Unset);
		InEnableDoItYourself = new CSInputChannel(this, ChannelDataType.Boolean, ChannelArrayType.Individual, defaultValue: false);
		InCamModeOverride = new CSInputChannel(this, ChannelDataType.Boolean, ChannelArrayType.Individual, defaultValue: false);
		InBezelSwitch = new CSInputChannel(this, ChannelDataType.Boolean, ChannelArrayType.Individual, defaultValue: false);
		InOuterSrfSwitch = new CSInputChannel(this, ChannelDataType.Boolean, ChannelArrayType.Individual, defaultValue: false);
		InBuildDomeOnPlane = new CSInputChannel(this, ChannelDataType.Boolean, ChannelArrayType.Individual, defaultValue: true);
		InPullToBaseObject = new CSInputChannel(this, ChannelDataType.Boolean, ChannelArrayType.Individual, defaultValue: false);
		InToggleSrfBetweenProngs = new CSInputChannel(this, ChannelDataType.Boolean, ChannelArrayType.Individual, defaultValue: true);
		InProngHeight = new CSInputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual, 0.15);
		InHeadHeight = new CSInputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual, 5);
		InBaseOffset = new CSInputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual, 0.0);
		InAngleX = new CSInputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual, 10);
		InAngleY = new CSInputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual, 10);
		InTopAngle = new CSInputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual, 0.0);
		InBaseAngle = new CSInputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual, 0.0);
		InBaseAnglePosition = new CSInputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual, 0.0);
		InBaseStyle = new CSInputChannel(this, ChannelDataType.Integer, ChannelArrayType.Individual, ValueListItem.HeadBuilderProngHead);
		InBasePointRadius = new CSInputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual, 0.0);
		InProngType = new CSInputChannel(this, ChannelDataType.Integer, ChannelArrayType.Array, new List<int>());
		InProngOverlap = new CSInputChannel(this, ChannelDataType.Double, ChannelArrayType.Array, new List<double>());
		InProngArrangement = new CSInputChannel(this, ChannelDataType.Integer, ChannelArrayType.Array, new List<int>());
		InProngPoint = new CSInputChannel(this, ChannelDataType.Integer, ChannelArrayType.Array, new List<int>());
		InProngPointAngle = new CSInputChannel(this, ChannelDataType.Double, ChannelArrayType.Array, new List<double>());
		InProngTopProfile = new CSInputChannel(this, ChannelDataType.Any, ChannelArrayType.Array);
		InProngTopX = new CSInputChannel(this, ChannelDataType.Double, ChannelArrayType.Array, new List<double>());
		InProngTopY = new CSInputChannel(this, ChannelDataType.Double, ChannelArrayType.Array, new List<double>());
		InProngTopNudge = new CSInputChannel(this, ChannelDataType.Double, ChannelArrayType.Array, new List<double>());
		InProngTopBlend = new CSInputChannel(this, ChannelDataType.Double, ChannelArrayType.Array, new List<double>());
		InProngBaseProfile = new CSInputChannel(this, ChannelDataType.Any, ChannelArrayType.Array);
		InProngBaseX = new CSInputChannel(this, ChannelDataType.Double, ChannelArrayType.Array, new List<double>());
		InProngBaseY = new CSInputChannel(this, ChannelDataType.Double, ChannelArrayType.Array, new List<double>());
		InProngBaseNudge = new CSInputChannel(this, ChannelDataType.Double, ChannelArrayType.Array, new List<double>());
		InProngBaseBlend = new CSInputChannel(this, ChannelDataType.Double, ChannelArrayType.Array, new List<double>());
		InProngDomeHeight = new CSInputChannel(this, ChannelDataType.Double, ChannelArrayType.Array, new List<double>());
		InProngDomeSettingDistance = new CSInputChannel(this, ChannelDataType.Double, ChannelArrayType.Array, new List<double>());
		InProngDomeSettingAngle = new CSInputChannel(this, ChannelDataType.Double, ChannelArrayType.Array, new List<double>());
		InProngBaseDomeHeight = new CSInputChannel(this, ChannelDataType.Double, ChannelArrayType.Array, new List<double>());
		InRailGem = new CSInputChannel(this, ChannelDataType.Gem, ChannelArrayType.Array);
		InRailProfile = new CSInputChannel(this, ChannelDataType.Curve, ChannelArrayType.Array);
		InRailPos = new CSInputChannel(this, ChannelDataType.Double, ChannelArrayType.Array, new List<double>());
		InRailX = new CSInputChannel(this, ChannelDataType.Double, ChannelArrayType.Array, new List<double>());
		InRailY = new CSInputChannel(this, ChannelDataType.Double, ChannelArrayType.Array, new List<double>());
		InRailOffset = new CSInputChannel(this, ChannelDataType.Double, ChannelArrayType.Array, new List<double>());
		InRailShearMultiplier = new CSInputChannel(this, ChannelDataType.Double, ChannelArrayType.Array, new List<double>());
		InBezelPlacement = new CSInputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual, 0.0);
		InSeatLength = new CSInputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual, 0.6);
		InSeatAngle = new CSInputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual, 45);
		InGirdleSpacing = new CSInputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual, 0.0);
		InPullRailMultiplier = new CSInputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual, 1.2);
		InUpperRailPosition = new CSInputChannel(this, ChannelDataType.Boolean, ChannelArrayType.Array, new List<bool>());
		InDomeBlendAmountInside = new CSInputChannel(this, ChannelDataType.Double, ChannelArrayType.Array, new List<double>());
		InDomeBlendAmountOutside = new CSInputChannel(this, ChannelDataType.Double, ChannelArrayType.Array, new List<double>());
		InDomeBlendStartInside = new CSInputChannel(this, ChannelDataType.Double, ChannelArrayType.Array, new List<double>());
		InDomeBlendStartOutside = new CSInputChannel(this, ChannelDataType.Double, ChannelArrayType.Array, new List<double>());
		InDomeBlendEndInside = new CSInputChannel(this, ChannelDataType.Double, ChannelArrayType.Array, new List<double>());
		InDomeBlendEndOutside = new CSInputChannel(this, ChannelDataType.Double, ChannelArrayType.Array, new List<double>());
		InCornerBlend = new CSInputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual, 0.0);
		InBuildVProngPlanar = new CSInputChannel(this, ChannelDataType.Boolean, ChannelArrayType.Individual, defaultValue: true);
		InStraightProng = new CSInputChannel(this, ChannelDataType.Boolean, ChannelArrayType.Array, new List<bool>());
		InFocalPoint = new CSInputChannel(this, ChannelDataType.Point, ChannelArrayType.Individual, Point3d.Unset);
		InMoveRails = new CSInputChannel(this, ChannelDataType.Boolean, ChannelArrayType.Array, new List<bool>());
		OutHeads = new CSOutputChannel(this, ChannelDataType.Any, ChannelArrayType.Any);
		OutDisplayCurves = new CSOutputChannel(this, ChannelDataType.Any, ChannelArrayType.Any);
		OutProngDomes = new CSOutputChannel(this, ChannelDataType.Any, ChannelArrayType.Any);
		OutGem = new CSOutputChannel(this, ChannelDataType.Gem, ChannelArrayType.Any);
		OutOuterSrfBeweenProngs = new CSOutputChannel(this, ChannelDataType.Any, ChannelArrayType.Any);
		OutInsideSrfBetweenProngs = new CSOutputChannel(this, ChannelDataType.Any, ChannelArrayType.Any);
		OutTopDomes = new CSOutputChannel(this, ChannelDataType.Any, ChannelArrayType.Any);
		OutProngBodies = new CSOutputChannel(this, ChannelDataType.Any, ChannelArrayType.Any);
		OutLowerDomes = new CSOutputChannel(this, ChannelDataType.Any, ChannelArrayType.Any);
		OutHeadArray = new CSOutputChannel(this, ChannelDataType.Any, ChannelArrayType.Any);
		OutBezel = new CSOutputChannel(this, ChannelDataType.Any, ChannelArrayType.Any);
		OutOriginCurves = new CSOutputChannel(this, ChannelDataType.Any, ChannelArrayType.Any);
		OutCurves = new CSOutputChannel(this, ChannelDataType.Any, ChannelArrayType.Any);
		SetChannelIdentifiers(autoCheckInputs: true);
	}

	public override bool Update()
	{
		//IL_02e0: Unknown result type (might be due to invalid IL or missing references)
		//IL_02e5: Unknown result type (might be due to invalid IL or missing references)
		//IL_0326: Unknown result type (might be due to invalid IL or missing references)
		//IL_032b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0348: Unknown result type (might be due to invalid IL or missing references)
		//IL_034a: Unknown result type (might be due to invalid IL or missing references)
		//IL_03bb: Unknown result type (might be due to invalid IL or missing references)
		//IL_036b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0370: Unknown result type (might be due to invalid IL or missing references)
		//IL_0372: Unknown result type (might be due to invalid IL or missing references)
		//IL_0374: Unknown result type (might be due to invalid IL or missing references)
		//IL_0376: Unknown result type (might be due to invalid IL or missing references)
		//IL_037b: Unknown result type (might be due to invalid IL or missing references)
		//IL_037e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0718: Unknown result type (might be due to invalid IL or missing references)
		//IL_0523: Unknown result type (might be due to invalid IL or missing references)
		//IL_08ea: Unknown result type (might be due to invalid IL or missing references)
		CSGemData gem = InGem.Gem;
		CSDataArray flattenArray = InBaseObject.GetFlattenArray();
		bool boolean = InEnableDoItYourself.Boolean;
		bool boolean2 = InBezelSwitch.Boolean;
		bool boolean3 = InOuterSrfSwitch.Boolean;
		bool boolean4 = InBuildDomeOnPlane.Boolean;
		bool boolean5 = InBuildVProngPlanar.Boolean;
		bool boolean6 = InPullToBaseObject.Boolean;
		int integer = InBaseStyle.Integer;
		double num = InProngHeight.Double;
		double num2 = InHeadHeight.Double;
		num2 = 0.0 - num2;
		double baseOffset = InBaseOffset.Double;
		double angleX = InAngleX.Double;
		double angleY = InAngleY.Double;
		double topAngle = InTopAngle.Double;
		double baseAngle = InBaseAngle.Double;
		double baseAnglePosition = InBaseAnglePosition.Double;
		double basePointSize = InBasePointRadius.Double;
		List<int> integerList = InProngType.IntegerList;
		List<int> integerList2 = InProngPoint.IntegerList;
		List<int> integerList3 = InProngArrangement.IntegerList;
		List<double> doubleList = InProngOverlap.DoubleList;
		List<double> doubleList2 = InProngPointAngle.DoubleList;
		List<double> doubleList3 = InProngTopX.DoubleList;
		List<double> doubleList4 = InProngTopY.DoubleList;
		List<double> doubleList5 = InProngTopNudge.DoubleList;
		List<double> doubleList6 = InProngTopBlend.DoubleList;
		List<double> doubleList7 = InProngBaseX.DoubleList;
		List<double> doubleList8 = InProngBaseY.DoubleList;
		List<double> doubleList9 = InProngBaseNudge.DoubleList;
		List<double> doubleList10 = InProngBaseBlend.DoubleList;
		List<double> doubleList11 = InProngDomeHeight.DoubleList;
		List<double> doubleList12 = InProngDomeSettingDistance.DoubleList;
		List<double> doubleList13 = InProngDomeSettingAngle.DoubleList;
		List<double> doubleList14 = InProngBaseDomeHeight.DoubleList;
		List<double> doubleList15 = InRailPos.DoubleList;
		List<bool> booleanList = InUpperRailPosition.BooleanList;
		List<double> list = InRailX.DoubleList;
		if (list.Count < 1)
		{
			list = new List<double>(1) { 0.0 };
		}
		List<double> doubleList16 = InRailY.DoubleList;
		List<double> doubleList17 = InRailOffset.DoubleList;
		List<double> doubleList18 = InRailShearMultiplier.DoubleList;
		List<CSGemData> gemList = InRailGem.GemList;
		List<Curve> curveList = InRailProfile.CurveList;
		List<Curve> curveList2 = InProngBaseProfile.CurveList;
		List<Curve> curveList3 = InProngTopProfile.CurveList;
		List<double> doubleList19 = InDomeBlendAmountInside.DoubleList;
		List<double> doubleList20 = InDomeBlendAmountOutside.DoubleList;
		List<double> doubleList21 = InDomeBlendStartInside.DoubleList;
		List<double> doubleList22 = InDomeBlendStartOutside.DoubleList;
		List<double> doubleList23 = InDomeBlendEndInside.DoubleList;
		List<double> doubleList24 = InDomeBlendEndOutside.DoubleList;
		List<bool> booleanList2 = InStraightProng.BooleanList;
		double cornerBlend = InCornerBlend.Double;
		Point3d point3d = InFocalPoint.Point3d;
		List<bool> booleanList3 = InMoveRails.BooleanList;
		bool boolean7 = InCamModeOverride.Boolean;
		_camMode = CSApplicationHelper.Application.IsCamMode;
		if (boolean7 && _camMode)
		{
			_camMode = false;
		}
		Transform transform = InConstructionTransform.Transform;
		if (!(gem.Duplicate() is CSGemData cSGemData))
		{
			return UpdateError("missing Gem");
		}
		if (transform != Transform.Unset)
		{
			Transform val = Transform.Translation(0.0, 0.0, 0.0 - num2);
			Transform transform2 = transform * val;
			cSGemData.ApplyTransform(transform2);
		}
		List<int> integerList4 = InProngPoint.IntegerList;
		CSDataArray originCurves = new CSDataArray();
		if (!CSHeadShape(cSGemData, num, num2, flattenArray, baseOffset, angleX, angleY, topAngle, baseAngle, baseAnglePosition, integerList4, integerList, doubleList2, booleanList2, cornerBlend, point3d, booleanList2, out var headGirdle, out var headTop, out var headBase, out var headMiddleCurve, out var outHeadSideX, out var outHeadSideY, out var headProngLines, out var _, out var _, out var surfaceBetweenProngs))
		{
			return UpdateError("HeadShape Failed");
		}
		headProngLines.ForEach(delegate(Curve _)
		{
			originCurves.Append(new CSOnGeometryData((GeometryBase)(object)_));
		});
		originCurves.Append(new CSOnGeometryData((GeometryBase)(object)headGirdle));
		originCurves.Append(new CSOnGeometryData((GeometryBase)(object)headTop));
		originCurves.Append(new CSOnGeometryData((GeometryBase)(object)headBase));
		originCurves.Append(new CSOnGeometryData((GeometryBase)(object)outHeadSideX));
		originCurves.Append(new CSOnGeometryData((GeometryBase)(object)outHeadSideY));
		List<Brep> list2 = new List<Brep>();
		List<Curve> list3 = new List<Curve>();
		CSDataArray cSDataArray = new CSDataArray();
		CSDataArray cSDataArray2 = new CSDataArray();
		CSDataArray cSDataArray3 = new CSDataArray();
		cSGemData.GetGemXformRC(out var _, out var xUp);
		double bezelPlacement = InBezelPlacement.Double;
		double girdleSpacing = InGirdleSpacing.Double;
		CSDataArray cSDataArray4 = new CSDataArray();
		if (boolean3)
		{
			for (int num3 = 0; num3 < integerList4.Count; num3++)
			{
				int prongPointIndex = ((num3 < integerList4.Count - 1) ? integerList4[num3 + 1] : integerList4[0]);
				Brep val2 = HeadOuterSurface(cSGemData, headMiddleCurve, headTop, headGirdle, headBase, outHeadSideY, headProngLines[0], integerList4[num3], prongPointIndex, num, girdleSpacing, doubleList4[0], doubleList5[0], doubleList8[0], doubleList9[0], bezelPlacement);
				((GeometryBase)val2).Transform(xUp);
				cSDataArray4.Append(new CSOnGeometryData((GeometryBase)(object)val2.DuplicateBrep()));
			}
		}
		if (boolean2)
		{
			double seatAngle = InSeatAngle.Double;
			double seatLength = InSeatLength.Double;
			Brep val3 = BezelBuilder(cSGemData, flattenArray, boolean6, headProngLines[0], headTop, headBase, headMiddleCurve, headGirdle, outHeadSideY, bezelPlacement, girdleSpacing, seatAngle, seatLength, num, doubleList11[0], doubleList14[0], baseAnglePosition, doubleList4[0], doubleList5[0], doubleList8[0], doubleList9[0], cornerBlend);
			if (val3 == null)
			{
				return UpdateError("BezelBuilder failed");
			}
			OutBezel.SetData(new CSOnGeometryData((GeometryBase)(object)val3));
			OutGem.SetData(cSGemData);
			OutOuterSrfBeweenProngs.SetData(cSDataArray4);
		}
		else
		{
			for (int num4 = 0; num4 < integerList2.Count; num4++)
			{
				Curve iProngRail = headProngLines[num4];
				if (!CSHeadProng(num4, cSGemData, integerList3[num4], integerList[num4], headProngLines, iProngRail, headTop, headBase, doubleList6[num4], doubleList10[num4], curveList3[num4], doubleList3[num4], doubleList4[num4], doubleList3[num4], doubleList5[num4], curveList2[num4], doubleList7[num4], doubleList8[num4], doubleList9[num4], doubleList7[num4], doubleList11[num4], doubleList12[num4], doubleList13[num4], integer, basePointSize, boolean, boolean4, boolean5, doubleList14[num4], doubleList[num4], doubleList19[num4], doubleList20[num4], doubleList21[num4], doubleList22[num4], doubleList23[num4], doubleList24[num4], point3d, out var topDome, out var middleProng, out var lowerDome, out var prongs, out var curves))
				{
					return UpdateError("Prong Creation Failed");
				}
				if (topDome != null)
				{
					foreach (Brep item in topDome)
					{
						cSDataArray.Append(new CSOnGeometryData((GeometryBase)(object)item.DuplicateBrep()));
					}
				}
				if (middleProng != null)
				{
					foreach (Brep item2 in middleProng)
					{
						cSDataArray2.Append(new CSOnGeometryData((GeometryBase)(object)item2.DuplicateBrep()));
					}
				}
				if (lowerDome != null)
				{
					foreach (Brep item3 in lowerDome)
					{
						cSDataArray3.Append(new CSOnGeometryData((GeometryBase)(object)item3.DuplicateBrep()));
					}
				}
				foreach (Brep item4 in prongs)
				{
					list2.Add(item4);
				}
				foreach (Curve item5 in curves)
				{
					list3.Add(item5);
				}
			}
			List<Brep> list4 = new List<Brep>();
			List<Curve> list5 = new List<Curve>();
			for (int num5 = 0; num5 < doubleList15.Count; num5++)
			{
				if (!CSHeadRail(cSGemData, gemList[num5], outHeadSideX, outHeadSideY, doubleList15[num5], curveList[num5], list[num5], doubleList16[num5], doubleList17[num5], boolean, doubleList18[num5], booleanList[num5], cornerBlend, point3d, booleanList3[num5], out var objects, out var railCurves, out var _, out var _))
				{
					return UpdateError("HeadRail Failed");
				}
				if (objects != null)
				{
					foreach (Brep item6 in objects)
					{
						list4.Add(item6);
					}
				}
				if (railCurves == null)
				{
					continue;
				}
				foreach (Curve item7 in railCurves)
				{
					list5.Add(item7);
				}
			}
			double railMultiplier = InPullRailMultiplier.Double;
			if (!CSHeadFinish(cSGemData, railMultiplier, list2, list3, list4, list5, boolean, boolean6, flattenArray, out var outGem, out var outProngs, out var outRails, out var head, out var prongDomes, out var displayCurves, out var constructionCurves))
			{
				return UpdateError("HeadFinish Failed");
			}
			CSDataArray cSDataArray5 = new CSDataArray();
			if (surfaceBetweenProngs.Count > 0)
			{
				foreach (Brep item8 in surfaceBetweenProngs)
				{
					cSDataArray5.Append(new CSOnGeometryData((GeometryBase)(object)item8));
				}
			}
			CSDataArray cSDataArray6 = new CSDataArray();
			cSDataArray6.Append(outProngs.Duplicate());
			cSDataArray6.Append(outRails.Duplicate());
			OutGem.SetData(outGem);
			OutHeads.SetData(head);
			OutDisplayCurves.SetData(displayCurves);
			OutProngDomes.SetData(prongDomes);
			OutTopDomes.SetData(cSDataArray);
			OutProngBodies.SetData(cSDataArray2);
			OutLowerDomes.SetData(cSDataArray3);
			OutInsideSrfBetweenProngs.SetData(cSDataArray5);
			OutOuterSrfBeweenProngs.SetData(cSDataArray4);
			OutHeadArray.SetData(cSDataArray6);
			OutCurves.SetData(constructionCurves);
			OutOriginCurves.SetData(originCurves);
		}
		return base.Update();
	}

	private bool CSHeadRail(CSGemData gem, CSGemData railShapeGem, Curve headSideX, Curve headSideY, double railZPos, Curve inRailProfile, double inRailX, double inRailY, double inRailOffset, bool enableModeDoItYourSelf, double shearMultiplier, bool upperRail, double cornerBlend, Point3d focalPoint, bool moveRail, out List<Brep> objects, out List<Curve> railCurves, out List<double> zPosition, out List<double> xPosition)
	{
		//IL_0039: Unknown result type (might be due to invalid IL or missing references)
		//IL_003e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0058: Unknown result type (might be due to invalid IL or missing references)
		//IL_005d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0088: Unknown result type (might be due to invalid IL or missing references)
		//IL_00aa: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c2: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c7: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e4: Expected O, but got Unknown
		//IL_0105: Unknown result type (might be due to invalid IL or missing references)
		//IL_011d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0122: Unknown result type (might be due to invalid IL or missing references)
		//IL_0140: Expected O, but got Unknown
		//IL_0156: Unknown result type (might be due to invalid IL or missing references)
		//IL_015b: Unknown result type (might be due to invalid IL or missing references)
		//IL_016e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0173: Unknown result type (might be due to invalid IL or missing references)
		//IL_01a1: Unknown result type (might be due to invalid IL or missing references)
		//IL_01a6: Unknown result type (might be due to invalid IL or missing references)
		//IL_01b6: Unknown result type (might be due to invalid IL or missing references)
		//IL_01bc: Unknown result type (might be due to invalid IL or missing references)
		//IL_01c1: Unknown result type (might be due to invalid IL or missing references)
		//IL_01e1: Unknown result type (might be due to invalid IL or missing references)
		//IL_01e6: Unknown result type (might be due to invalid IL or missing references)
		//IL_01f2: Unknown result type (might be due to invalid IL or missing references)
		//IL_024c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0251: Unknown result type (might be due to invalid IL or missing references)
		//IL_025b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0268: Unknown result type (might be due to invalid IL or missing references)
		//IL_026d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0271: Unknown result type (might be due to invalid IL or missing references)
		//IL_027f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0284: Unknown result type (might be due to invalid IL or missing references)
		//IL_028a: Unknown result type (might be due to invalid IL or missing references)
		//IL_028f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0294: Unknown result type (might be due to invalid IL or missing references)
		//IL_0299: Unknown result type (might be due to invalid IL or missing references)
		//IL_02a3: Unknown result type (might be due to invalid IL or missing references)
		//IL_02a7: Unknown result type (might be due to invalid IL or missing references)
		//IL_02ac: Unknown result type (might be due to invalid IL or missing references)
		//IL_030e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0310: Unknown result type (might be due to invalid IL or missing references)
		//IL_0312: Unknown result type (might be due to invalid IL or missing references)
		//IL_032a: Unknown result type (might be due to invalid IL or missing references)
		//IL_032f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0333: Unknown result type (might be due to invalid IL or missing references)
		//IL_0338: Unknown result type (might be due to invalid IL or missing references)
		//IL_0344: Unknown result type (might be due to invalid IL or missing references)
		//IL_0349: Unknown result type (might be due to invalid IL or missing references)
		//IL_0355: Unknown result type (might be due to invalid IL or missing references)
		//IL_035a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0364: Unknown result type (might be due to invalid IL or missing references)
		//IL_0369: Unknown result type (might be due to invalid IL or missing references)
		//IL_036b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0374: Unknown result type (might be due to invalid IL or missing references)
		//IL_0379: Unknown result type (might be due to invalid IL or missing references)
		//IL_0387: Unknown result type (might be due to invalid IL or missing references)
		//IL_038c: Unknown result type (might be due to invalid IL or missing references)
		//IL_039f: Unknown result type (might be due to invalid IL or missing references)
		//IL_03a4: Unknown result type (might be due to invalid IL or missing references)
		//IL_03a8: Unknown result type (might be due to invalid IL or missing references)
		//IL_03aa: Unknown result type (might be due to invalid IL or missing references)
		//IL_03ac: Unknown result type (might be due to invalid IL or missing references)
		//IL_02f9: Unknown result type (might be due to invalid IL or missing references)
		//IL_02fb: Unknown result type (might be due to invalid IL or missing references)
		//IL_02fd: Unknown result type (might be due to invalid IL or missing references)
		//IL_03c5: Unknown result type (might be due to invalid IL or missing references)
		//IL_03ca: Unknown result type (might be due to invalid IL or missing references)
		//IL_03ce: Unknown result type (might be due to invalid IL or missing references)
		//IL_03d3: Unknown result type (might be due to invalid IL or missing references)
		//IL_03f0: Unknown result type (might be due to invalid IL or missing references)
		//IL_0400: Unknown result type (might be due to invalid IL or missing references)
		//IL_041f: Unknown result type (might be due to invalid IL or missing references)
		//IL_042d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0432: Unknown result type (might be due to invalid IL or missing references)
		//IL_0448: Unknown result type (might be due to invalid IL or missing references)
		//IL_0468: Unknown result type (might be due to invalid IL or missing references)
		//IL_0488: Unknown result type (might be due to invalid IL or missing references)
		//IL_048d: Unknown result type (might be due to invalid IL or missing references)
		//IL_048f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0491: Unknown result type (might be due to invalid IL or missing references)
		//IL_0496: Unknown result type (might be due to invalid IL or missing references)
		//IL_049b: Unknown result type (might be due to invalid IL or missing references)
		//IL_049f: Unknown result type (might be due to invalid IL or missing references)
		//IL_04a9: Unknown result type (might be due to invalid IL or missing references)
		//IL_04b3: Unknown result type (might be due to invalid IL or missing references)
		//IL_04bd: Unknown result type (might be due to invalid IL or missing references)
		//IL_04c5: Unknown result type (might be due to invalid IL or missing references)
		//IL_04c7: Unknown result type (might be due to invalid IL or missing references)
		//IL_05bd: Unknown result type (might be due to invalid IL or missing references)
		//IL_05c7: Unknown result type (might be due to invalid IL or missing references)
		//IL_05d1: Unknown result type (might be due to invalid IL or missing references)
		//IL_04d9: Unknown result type (might be due to invalid IL or missing references)
		//IL_04db: Unknown result type (might be due to invalid IL or missing references)
		//IL_04df: Unknown result type (might be due to invalid IL or missing references)
		//IL_050b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0510: Unknown result type (might be due to invalid IL or missing references)
		//IL_051e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0523: Unknown result type (might be due to invalid IL or missing references)
		//IL_0538: Unknown result type (might be due to invalid IL or missing references)
		//IL_0547: Unknown result type (might be due to invalid IL or missing references)
		//IL_054c: Unknown result type (might be due to invalid IL or missing references)
		//IL_055a: Unknown result type (might be due to invalid IL or missing references)
		//IL_055f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0574: Unknown result type (might be due to invalid IL or missing references)
		//IL_0583: Unknown result type (might be due to invalid IL or missing references)
		//IL_0588: Unknown result type (might be due to invalid IL or missing references)
		//IL_0596: Unknown result type (might be due to invalid IL or missing references)
		//IL_059b: Unknown result type (might be due to invalid IL or missing references)
		//IL_05b0: Unknown result type (might be due to invalid IL or missing references)
		objects = new List<Brep>();
		railCurves = new List<Curve>();
		zPosition = new List<double>();
		xPosition = new List<double>();
		Curve val = inRailProfile.DuplicateCurve();
		if (val == null)
		{
			return false;
		}
		if (headSideX == null)
		{
			return false;
		}
		if (headSideY == null)
		{
			return false;
		}
		Point3d val2 = headSideX.PointAtEnd;
		double num = ((Point3d)(ref val2)).Z * (1.0 - railZPos);
		if (upperRail)
		{
			val2 = Point3d.Origin;
			num = ((Point3d)(ref val2)).Z - inRailY / 2.0 - railZPos;
		}
		zPosition.Add(num + inRailY);
		if (gem == null)
		{
			return false;
		}
		CSBlockUtil.CSUtilMath.GetUnscaledXforms(gem.Xform, out var xUp, out var xDown);
		CurveIntersections val3 = Intersection.CurveCurve((Curve)new LineCurve(new Point3d(0.0, 0.0, num), new Point3d(100.0, 0.0, num)), headSideX, 0.01, 0.01);
		if (val3.Count <= 0)
		{
			return false;
		}
		CurveIntersections val4 = Intersection.CurveCurve((Curve)new LineCurve(new Point3d(0.0, 0.0, num), new Point3d(0.0, 100.0, num)), headSideY, 0.01, 0.01);
		if (val4.Count <= 0)
		{
			return false;
		}
		Point3d val5 = val3[0].PointA;
		double x = ((Point3d)(ref val5)).X;
		val5 = val4[0].PointA;
		double y = ((Point3d)(ref val5)).Y;
		xPosition.Add(x - gem.GemXMax);
		double parameterB = val3[0].ParameterB;
		Vector3d val6 = headSideX.TangentAt(parameterB);
		double num2 = (0.0 - ((Vector3d)(ref val6)).X) * shearMultiplier;
		Transform transform = Transform.Scale(new Plane(railShapeGem.Center, railShapeGem.Up), x / railShapeGem.GemXMax, y / railShapeGem.GemYMax, 1.0);
		CSGemData cSGemData = new CSGemData(railShapeGem);
		cSGemData.ApplyTransform(transform);
		if (!GemOffsetCurveV2.GemOffset(cSGemData, inRailOffset, out Curve curve, doTransformUp: false, out string _, cornerBlend))
		{
			return false;
		}
		if (curve == null)
		{
			return false;
		}
		if (!GemOffsetCurveV2.GemOffset(cSGemData, inRailOffset + (0.0 - inRailX), out Curve curve2, doTransformUp: false, out string _, 0.0))
		{
			return false;
		}
		if (curve2 == null)
		{
			return false;
		}
		Transform val7 = Transform.Translation(0.0, 0.0, num);
		Curve curve3 = cSGemData.GetCurve();
		((GeometryBase)curve3).Transform(xDown);
		Point3d val8 = curve3.PointAt(railShapeGem.BezelParam);
		double num3 = default(double);
		curve.ClosestPoint(val8, ref num3);
		Point3d val9 = curve.PointAt(num3);
		Vector3d val10 = Vector3d.CrossProduct(curve.TangentAt(num3), Vector3d.ZAxis);
		((Vector3d)(ref val10)).Unitize();
		val10 *= inRailX;
		double num4 = inRailOffset + (0.0 - inRailX);
		double item = default(double);
		if (num4 < 0.0 && (Math.Abs(num4) > cSGemData.GemXSize / 2.0 || Math.Abs(num4) > cSGemData.GemYSize / 2.0))
		{
			curve2.ClosestPoint(val9 - val10, ref item);
		}
		else
		{
			curve2.ClosestPoint(val9 + val10, ref item);
		}
		NurbsCurve val11 = val.ToNurbsCurve();
		BoundingBox boundingBox = ((GeometryBase)val11).GetBoundingBox(true);
		val5 = ((BoundingBox)(ref boundingBox)).Min;
		double num5 = 0.0 - ((Point3d)(ref val5)).X;
		val5 = ((BoundingBox)(ref boundingBox)).Min;
		double num6 = 0.0 - ((Point3d)(ref val5)).Y;
		val5 = ((BoundingBox)(ref boundingBox)).Min;
		Transform val12 = Transform.Translation(num5, num6, 0.0 - ((Point3d)(ref val5)).Z);
		Plane worldXY = Plane.WorldXY;
		Vector3d diagonal = ((BoundingBox)(ref boundingBox)).Diagonal;
		double num7 = inRailX / ((Vector3d)(ref diagonal)).X;
		diagonal = ((BoundingBox)(ref boundingBox)).Diagonal;
		Transform val13 = Transform.Scale(worldXY, num7, inRailY / ((Vector3d)(ref diagonal)).Y, 0.0);
		((GeometryBase)val11).Transform(val13 * val12);
		for (int i = 0; i < val11.Points.Count; i++)
		{
			ControlPoint val14 = val11.Points[i];
			Point3d location = ((ControlPoint)(ref val14)).Location;
			((Point3d)(ref location)).X = ((Point3d)(ref location)).X + num2 * ((Point3d)(ref location)).Y;
			((ControlPoint)(ref val14)).Location = location;
			val11.Points[i] = val14;
		}
		double num8 = default(double);
		curve.ClosestPoint(val8, ref num8);
		Point3d destination = curve.PointAt(num8);
		Transform val15 = CSBlockUtil.CSUtilMath.Orient3Point(new Point3d(inRailX, 0.0, 0.0), new Vector3d(-1.0, 0.0, 0.0), new Vector3d(0.0, 1.0, 0.0), destination, val10, Vector3d.ZAxis);
		((GeometryBase)val11).Transform(val15);
		((GeometryBase)curve).Transform(val7);
		((GeometryBase)curve2).Transform(val7);
		((GeometryBase)val11).Transform(val7);
		if (focalPoint != Point3d.Unset && moveRail)
		{
			Point3d val16 = focalPoint;
			((Point3d)(ref val16)).Transform(xDown);
			double num9 = ((Point3d)(ref val16)).X / ((Point3d)(ref val16)).Z;
			double num10 = ((Point3d)(ref val16)).Y / ((Point3d)(ref val16)).Z;
			Curve obj = curve;
			val5 = curve.PointAtStart;
			double num11 = ((Point3d)(ref val5)).Z * num9;
			val5 = curve.PointAtStart;
			((GeometryBase)obj).Transform(Transform.Translation(num11, ((Point3d)(ref val5)).Z * num10, 0.0));
			Curve obj2 = curve2;
			val5 = curve.PointAtStart;
			double num12 = ((Point3d)(ref val5)).Z * num9;
			val5 = curve.PointAtStart;
			((GeometryBase)obj2).Transform(Transform.Translation(num12, ((Point3d)(ref val5)).Z * num10, 0.0));
			val5 = curve.PointAtStart;
			double num13 = ((Point3d)(ref val5)).Z * num9;
			val5 = curve.PointAtStart;
			((GeometryBase)val11).Transform(Transform.Translation(num13, ((Point3d)(ref val5)).Z * num10, 0.0));
		}
		((GeometryBase)curve).Transform(xUp);
		((GeometryBase)curve2).Transform(xUp);
		((GeometryBase)val11).Transform(xUp);
		railCurves.Add(curve);
		railCurves.Add(curve2);
		railCurves.Add((Curve)(object)val11);
		if (!enableModeDoItYourSelf)
		{
			List<Curve> profile = new List<Curve> { (Curve)(object)val11 };
			List<double> parameters = new List<double> { num3 };
			List<double> parameters2 = new List<double> { item };
			if (!CSUtilSweepRC.Sweep2(curve, curve2, profile, parameters, parameters2, out Brep breps, closed: true, maintainHeight: true))
			{
				return false;
			}
			objects.Add(breps);
		}
		return true;
	}

	private Brep HeadOuterSurface(CSGemData gem, Curve headMiddle, Curve headTop, Curve headGirdle, Curve headBase, Curve headSideY, Curve prongLine, int prongPointIndex1, int prongPointIndex2, double seatHeight, double girdleSpacing, double prongTopY, double prongTopNudge, double prongBaseY, double prongBaseNudge, double bezelPlacement)
	{
		//IL_0013: Unknown result type (might be due to invalid IL or missing references)
		//IL_0019: Unknown result type (might be due to invalid IL or missing references)
		//IL_001f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0024: Unknown result type (might be due to invalid IL or missing references)
		//IL_0029: Unknown result type (might be due to invalid IL or missing references)
		//IL_002e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0033: Unknown result type (might be due to invalid IL or missing references)
		//IL_0040: Unknown result type (might be due to invalid IL or missing references)
		//IL_004e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0053: Unknown result type (might be due to invalid IL or missing references)
		//IL_0070: Unknown result type (might be due to invalid IL or missing references)
		//IL_0079: Unknown result type (might be due to invalid IL or missing references)
		//IL_007e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0087: Unknown result type (might be due to invalid IL or missing references)
		//IL_008c: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ac: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ba: Unknown result type (might be due to invalid IL or missing references)
		//IL_00bf: Unknown result type (might be due to invalid IL or missing references)
		//IL_00dc: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e5: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ea: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f3: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f8: Unknown result type (might be due to invalid IL or missing references)
		//IL_012b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0130: Unknown result type (might be due to invalid IL or missing references)
		//IL_0135: Unknown result type (might be due to invalid IL or missing references)
		//IL_013a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0145: Unknown result type (might be due to invalid IL or missing references)
		//IL_014a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0191: Unknown result type (might be due to invalid IL or missing references)
		//IL_0196: Unknown result type (might be due to invalid IL or missing references)
		//IL_0199: Unknown result type (might be due to invalid IL or missing references)
		//IL_01c2: Unknown result type (might be due to invalid IL or missing references)
		//IL_01c7: Unknown result type (might be due to invalid IL or missing references)
		//IL_01dd: Unknown result type (might be due to invalid IL or missing references)
		//IL_01e2: Unknown result type (might be due to invalid IL or missing references)
		//IL_01e6: Unknown result type (might be due to invalid IL or missing references)
		//IL_01f1: Unknown result type (might be due to invalid IL or missing references)
		//IL_01f6: Unknown result type (might be due to invalid IL or missing references)
		//IL_01fa: Unknown result type (might be due to invalid IL or missing references)
		//IL_0205: Unknown result type (might be due to invalid IL or missing references)
		//IL_020a: Unknown result type (might be due to invalid IL or missing references)
		//IL_020c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0211: Unknown result type (might be due to invalid IL or missing references)
		//IL_0216: Unknown result type (might be due to invalid IL or missing references)
		//IL_0218: Unknown result type (might be due to invalid IL or missing references)
		//IL_021d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0222: Unknown result type (might be due to invalid IL or missing references)
		//IL_0224: Unknown result type (might be due to invalid IL or missing references)
		//IL_0226: Unknown result type (might be due to invalid IL or missing references)
		//IL_0228: Unknown result type (might be due to invalid IL or missing references)
		//IL_023c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0241: Unknown result type (might be due to invalid IL or missing references)
		//IL_0261: Unknown result type (might be due to invalid IL or missing references)
		//IL_0266: Unknown result type (might be due to invalid IL or missing references)
		//IL_0271: Unknown result type (might be due to invalid IL or missing references)
		//IL_0276: Unknown result type (might be due to invalid IL or missing references)
		//IL_0280: Unknown result type (might be due to invalid IL or missing references)
		//IL_028b: Unknown result type (might be due to invalid IL or missing references)
		//IL_028d: Unknown result type (might be due to invalid IL or missing references)
		//IL_028f: Unknown result type (might be due to invalid IL or missing references)
		//IL_02a3: Unknown result type (might be due to invalid IL or missing references)
		//IL_02a8: Unknown result type (might be due to invalid IL or missing references)
		//IL_02c8: Unknown result type (might be due to invalid IL or missing references)
		//IL_02cd: Unknown result type (might be due to invalid IL or missing references)
		//IL_02d8: Unknown result type (might be due to invalid IL or missing references)
		//IL_02dd: Unknown result type (might be due to invalid IL or missing references)
		//IL_02e7: Unknown result type (might be due to invalid IL or missing references)
		//IL_0337: Unknown result type (might be due to invalid IL or missing references)
		//IL_034d: Unknown result type (might be due to invalid IL or missing references)
		//IL_037a: Unknown result type (might be due to invalid IL or missing references)
		//IL_037f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0383: Unknown result type (might be due to invalid IL or missing references)
		//IL_0388: Unknown result type (might be due to invalid IL or missing references)
		//IL_038d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0392: Unknown result type (might be due to invalid IL or missing references)
		//IL_039d: Unknown result type (might be due to invalid IL or missing references)
		//IL_03a2: Unknown result type (might be due to invalid IL or missing references)
		//IL_03cf: Unknown result type (might be due to invalid IL or missing references)
		//IL_03f3: Unknown result type (might be due to invalid IL or missing references)
		//IL_03f8: Unknown result type (might be due to invalid IL or missing references)
		//IL_040e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0413: Unknown result type (might be due to invalid IL or missing references)
		//IL_0415: Unknown result type (might be due to invalid IL or missing references)
		//IL_0417: Unknown result type (might be due to invalid IL or missing references)
		//IL_0419: Unknown result type (might be due to invalid IL or missing references)
		//IL_042d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0432: Unknown result type (might be due to invalid IL or missing references)
		//IL_0453: Unknown result type (might be due to invalid IL or missing references)
		//IL_0458: Unknown result type (might be due to invalid IL or missing references)
		//IL_0463: Unknown result type (might be due to invalid IL or missing references)
		//IL_0468: Unknown result type (might be due to invalid IL or missing references)
		//IL_0472: Unknown result type (might be due to invalid IL or missing references)
		//IL_047d: Unknown result type (might be due to invalid IL or missing references)
		//IL_047f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0481: Unknown result type (might be due to invalid IL or missing references)
		//IL_0495: Unknown result type (might be due to invalid IL or missing references)
		//IL_049a: Unknown result type (might be due to invalid IL or missing references)
		//IL_04bb: Unknown result type (might be due to invalid IL or missing references)
		//IL_04c0: Unknown result type (might be due to invalid IL or missing references)
		//IL_04cb: Unknown result type (might be due to invalid IL or missing references)
		//IL_04d0: Unknown result type (might be due to invalid IL or missing references)
		//IL_04da: Unknown result type (might be due to invalid IL or missing references)
		//IL_052d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0544: Unknown result type (might be due to invalid IL or missing references)
		//IL_055f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0564: Unknown result type (might be due to invalid IL or missing references)
		//IL_0568: Unknown result type (might be due to invalid IL or missing references)
		//IL_056d: Unknown result type (might be due to invalid IL or missing references)
		//IL_057a: Unknown result type (might be due to invalid IL or missing references)
		//IL_057f: Unknown result type (might be due to invalid IL or missing references)
		//IL_05a8: Unknown result type (might be due to invalid IL or missing references)
		//IL_05ad: Unknown result type (might be due to invalid IL or missing references)
		//IL_05b9: Unknown result type (might be due to invalid IL or missing references)
		//IL_05be: Unknown result type (might be due to invalid IL or missing references)
		//IL_05df: Unknown result type (might be due to invalid IL or missing references)
		//IL_0603: Unknown result type (might be due to invalid IL or missing references)
		//IL_0608: Unknown result type (might be due to invalid IL or missing references)
		//IL_061e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0623: Unknown result type (might be due to invalid IL or missing references)
		//IL_0625: Unknown result type (might be due to invalid IL or missing references)
		//IL_0627: Unknown result type (might be due to invalid IL or missing references)
		//IL_0629: Unknown result type (might be due to invalid IL or missing references)
		//IL_063d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0642: Unknown result type (might be due to invalid IL or missing references)
		//IL_0663: Unknown result type (might be due to invalid IL or missing references)
		//IL_0668: Unknown result type (might be due to invalid IL or missing references)
		//IL_0673: Unknown result type (might be due to invalid IL or missing references)
		//IL_0678: Unknown result type (might be due to invalid IL or missing references)
		//IL_0682: Unknown result type (might be due to invalid IL or missing references)
		//IL_068d: Unknown result type (might be due to invalid IL or missing references)
		//IL_068f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0691: Unknown result type (might be due to invalid IL or missing references)
		//IL_06a5: Unknown result type (might be due to invalid IL or missing references)
		//IL_06aa: Unknown result type (might be due to invalid IL or missing references)
		//IL_06cb: Unknown result type (might be due to invalid IL or missing references)
		//IL_06d0: Unknown result type (might be due to invalid IL or missing references)
		//IL_06db: Unknown result type (might be due to invalid IL or missing references)
		//IL_06e0: Unknown result type (might be due to invalid IL or missing references)
		//IL_06ea: Unknown result type (might be due to invalid IL or missing references)
		//IL_073d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0754: Unknown result type (might be due to invalid IL or missing references)
		//IL_076e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0773: Unknown result type (might be due to invalid IL or missing references)
		//IL_0778: Unknown result type (might be due to invalid IL or missing references)
		//IL_077d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0788: Unknown result type (might be due to invalid IL or missing references)
		//IL_078d: Unknown result type (might be due to invalid IL or missing references)
		//IL_079d: Unknown result type (might be due to invalid IL or missing references)
		//IL_07a2: Unknown result type (might be due to invalid IL or missing references)
		//IL_07c4: Unknown result type (might be due to invalid IL or missing references)
		//IL_07c9: Unknown result type (might be due to invalid IL or missing references)
		//IL_07ea: Unknown result type (might be due to invalid IL or missing references)
		//IL_080e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0813: Unknown result type (might be due to invalid IL or missing references)
		//IL_0829: Unknown result type (might be due to invalid IL or missing references)
		//IL_082e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0830: Unknown result type (might be due to invalid IL or missing references)
		//IL_0832: Unknown result type (might be due to invalid IL or missing references)
		//IL_0834: Unknown result type (might be due to invalid IL or missing references)
		//IL_0848: Unknown result type (might be due to invalid IL or missing references)
		//IL_084d: Unknown result type (might be due to invalid IL or missing references)
		//IL_086e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0873: Unknown result type (might be due to invalid IL or missing references)
		//IL_087e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0883: Unknown result type (might be due to invalid IL or missing references)
		//IL_088d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0898: Unknown result type (might be due to invalid IL or missing references)
		//IL_089a: Unknown result type (might be due to invalid IL or missing references)
		//IL_089c: Unknown result type (might be due to invalid IL or missing references)
		//IL_08b0: Unknown result type (might be due to invalid IL or missing references)
		//IL_08b5: Unknown result type (might be due to invalid IL or missing references)
		//IL_08d6: Unknown result type (might be due to invalid IL or missing references)
		//IL_08db: Unknown result type (might be due to invalid IL or missing references)
		//IL_08e6: Unknown result type (might be due to invalid IL or missing references)
		//IL_08eb: Unknown result type (might be due to invalid IL or missing references)
		//IL_08f5: Unknown result type (might be due to invalid IL or missing references)
		//IL_0948: Unknown result type (might be due to invalid IL or missing references)
		//IL_095f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0980: Unknown result type (might be due to invalid IL or missing references)
		//IL_0985: Unknown result type (might be due to invalid IL or missing references)
		//IL_0990: Unknown result type (might be due to invalid IL or missing references)
		//IL_0995: Unknown result type (might be due to invalid IL or missing references)
		//IL_09ac: Unknown result type (might be due to invalid IL or missing references)
		//IL_09b1: Unknown result type (might be due to invalid IL or missing references)
		//IL_09bb: Unknown result type (might be due to invalid IL or missing references)
		//IL_09c0: Unknown result type (might be due to invalid IL or missing references)
		//IL_0a16: Unknown result type (might be due to invalid IL or missing references)
		//IL_0a1b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0a25: Unknown result type (might be due to invalid IL or missing references)
		//IL_0a2a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0a51: Unknown result type (might be due to invalid IL or missing references)
		//IL_0a56: Unknown result type (might be due to invalid IL or missing references)
		if (!(gem.Duplicate() is CSGemData cSGemData))
		{
			return null;
		}
		cSGemData.ApplyTransform(CSBlockUtil.CSUtilMath.Orient3Point(cSGemData.Center, cSGemData.Up, cSGemData.East, Point3d.Origin, Vector3d.ZAxis, Vector3d.XAxis));
		Vector3d inputDirection = CSBlockUtil.CSUtilMath.AverageTangentAt(headTop, headTop.PointAtStart);
		Vector3d inputVector = new Vector3d(0.0, 0.0, -1.0);
		Interval domain = headSideY.Domain;
		double d = CSBlockUtil.CSUtilMath.AngleBetweenVectorsWithDirection(inputVector, headSideY.TangentAt(((Interval)(ref domain)).Min), inputDirection);
		double num = 1.0 / Math.Cos(d);
		Vector3d inputDirection2 = CSBlockUtil.CSUtilMath.AverageTangentAt(headBase, headBase.PointAtStart);
		Vector3d inputVector2 = new Vector3d(0.0, 0.0, -1.0);
		domain = headSideY.Domain;
		double d2 = CSBlockUtil.CSUtilMath.AngleBetweenVectorsWithDirection(inputVector2, headSideY.TangentAt(((Interval)(ref domain)).Min), inputDirection2);
		double num2 = 1.0 / Math.Cos(d2);
		double num3 = prongTopY * num - prongTopNudge;
		double num4 = prongBaseY * num2 - prongBaseNudge;
		BoundingBox boundingBox = ((GeometryBase)headGirdle).GetBoundingBox(true);
		Vector3d diagonal = ((BoundingBox)(ref boundingBox)).Diagonal;
		double x = ((Vector3d)(ref diagonal)).X;
		diagonal = ((BoundingBox)(ref boundingBox)).Diagonal;
		cSGemData.ScaleToSize(x, ((Vector3d)(ref diagonal)).Y);
		Curve curve = cSGemData.GetCurve();
		if (!GemOffsetCurveV2.GemOffset(cSGemData, num3 + girdleSpacing, out Curve curve2, doTransformUp: false, out string error, 0.0))
		{
			return null;
		}
		Transform val = Transform.Translation(0.0, 0.0, bezelPlacement);
		((GeometryBase)curve2).Transform(val);
		List<CSGemInstanceDefinition.ProngPosition> prongPositions = cSGemData.Def.ProngPositions;
		Point3d val2 = curve.PointAt(prongPositions[prongPointIndex1].ProngParam);
		Point3d val3 = curve.PointAt(prongPositions[prongPointIndex2].ProngParam);
		Vector3d val4 = CSBlockUtil.CSUtilMath.AverageTangentAt(curve, val2);
		Vector3d val5 = CSBlockUtil.CSUtilMath.AverageTangentAt(curve, val3);
		Vector3d val6 = Vector3d.CrossProduct(val4, Vector3d.ZAxis);
		Vector3d val7 = Vector3d.CrossProduct(val5, Vector3d.ZAxis);
		Line val8 = new Line(val2, -val6, (num3 + girdleSpacing) * 3.0);
		NurbsCurve val9 = ((Line)(ref val8)).ToNurbsCurve();
		NurbsCurve obj = val9;
		Point3d val10 = curve2.PointAtStart;
		double z = ((Point3d)(ref val10)).Z;
		val10 = ((Curve)val9).PointAtStart;
		((GeometryBase)obj).Transform(Transform.Translation(0.0, 0.0, z - ((Point3d)(ref val10)).Z));
		val8 = new Line(val3, -val7, (num3 + girdleSpacing) * 3.0);
		NurbsCurve val11 = ((Line)(ref val8)).ToNurbsCurve();
		NurbsCurve obj2 = val11;
		val10 = curve2.PointAtStart;
		double z2 = ((Point3d)(ref val10)).Z;
		val10 = ((Curve)val11).PointAtStart;
		((GeometryBase)obj2).Transform(Transform.Translation(0.0, 0.0, z2 - ((Point3d)(ref val10)).Z));
		CurveIntersections val12 = Intersection.CurveCurve(curve2, (Curve)(object)val9, CSApplicationHelper.Application.Tolerance, CSApplicationHelper.Application.Tolerance);
		CurveIntersections val13 = Intersection.CurveCurve(curve2, (Curve)(object)val11, CSApplicationHelper.Application.Tolerance, CSApplicationHelper.Application.Tolerance);
		double num5 = default(double);
		curve2.ClosestPoint(val12[0].PointA, ref num5);
		double num6 = default(double);
		curve2.ClosestPoint(val13[0].PointA, ref num6);
		curve2 = curve2.Trim(num5, num6);
		Transform val14 = Transform.Translation(0.0, 0.0, bezelPlacement + seatHeight);
		BoundingBox boundingBox2 = ((GeometryBase)headTop).GetBoundingBox(true);
		diagonal = ((BoundingBox)(ref boundingBox2)).Diagonal;
		double x2 = ((Vector3d)(ref diagonal)).X;
		diagonal = ((BoundingBox)(ref boundingBox2)).Diagonal;
		cSGemData.ScaleToSize(x2, ((Vector3d)(ref diagonal)).Y);
		if (!GemOffsetCurveV2.GemOffset(cSGemData, num3 + girdleSpacing, out Curve curve3, doTransformUp: false, out error, 0.0))
		{
			return null;
		}
		((GeometryBase)curve3).Transform(val14);
		curve = cSGemData.GetCurve();
		val2 = curve.PointAt(prongPositions[prongPointIndex1].ProngParam);
		val3 = curve.PointAt(prongPositions[prongPointIndex2].ProngParam);
		val8 = new Line(val2, -val6, (num3 + girdleSpacing) * 3.0);
		val9 = ((Line)(ref val8)).ToNurbsCurve();
		NurbsCurve obj3 = val9;
		val10 = curve3.PointAtStart;
		double z3 = ((Point3d)(ref val10)).Z;
		val10 = ((Curve)val9).PointAtStart;
		((GeometryBase)obj3).Transform(Transform.Translation(0.0, 0.0, z3 - ((Point3d)(ref val10)).Z));
		val8 = new Line(val3, -val7, (num3 + girdleSpacing) * 3.0);
		val11 = ((Line)(ref val8)).ToNurbsCurve();
		NurbsCurve obj4 = val11;
		val10 = curve3.PointAtStart;
		double z4 = ((Point3d)(ref val10)).Z;
		val10 = ((Curve)val11).PointAtStart;
		((GeometryBase)obj4).Transform(Transform.Translation(0.0, 0.0, z4 - ((Point3d)(ref val10)).Z));
		val12 = Intersection.CurveCurve(curve3, (Curve)(object)val9, CSApplicationHelper.Application.Tolerance, CSApplicationHelper.Application.Tolerance);
		val13 = Intersection.CurveCurve(curve3, (Curve)(object)val11, CSApplicationHelper.Application.Tolerance, CSApplicationHelper.Application.Tolerance);
		curve3.ClosestPoint(val12[0].PointA, ref num5);
		curve3.ClosestPoint(val13[0].PointA, ref num6);
		curve3 = curve3.Trim(num5, num6);
		BoundingBox boundingBox3 = ((GeometryBase)headBase).GetBoundingBox(true);
		diagonal = ((BoundingBox)(ref boundingBox3)).Diagonal;
		double x3 = ((Vector3d)(ref diagonal)).X;
		diagonal = ((BoundingBox)(ref boundingBox3)).Diagonal;
		double y = ((Vector3d)(ref diagonal)).Y;
		cSGemData.ScaleToSize(x3, y);
		val10 = prongLine.PointAtEnd;
		val14 = Transform.Translation(0.0, 0.0, ((Point3d)(ref val10)).Z + bezelPlacement);
		if (!GemOffsetCurveV2.GemOffset(cSGemData, num4 + girdleSpacing, out Curve curve4, doTransformUp: false, out error, 0.0))
		{
			return null;
		}
		((GeometryBase)curve4).Transform(val14);
		curve = cSGemData.GetCurve();
		val2 = curve.PointAt(prongPositions[prongPointIndex1].ProngParam);
		val3 = curve.PointAt(prongPositions[prongPointIndex2].ProngParam);
		val8 = new Line(val2, -val6, (num4 + girdleSpacing) * 3.0);
		val9 = ((Line)(ref val8)).ToNurbsCurve();
		NurbsCurve obj5 = val9;
		val10 = curve4.PointAtStart;
		double z5 = ((Point3d)(ref val10)).Z;
		val10 = ((Curve)val9).PointAtStart;
		((GeometryBase)obj5).Transform(Transform.Translation(0.0, 0.0, z5 - ((Point3d)(ref val10)).Z));
		val8 = new Line(val3, -val7, (num4 + girdleSpacing) * 3.0);
		val11 = ((Line)(ref val8)).ToNurbsCurve();
		NurbsCurve obj6 = val11;
		val10 = curve4.PointAtStart;
		double z6 = ((Point3d)(ref val10)).Z;
		val10 = ((Curve)val11).PointAtStart;
		((GeometryBase)obj6).Transform(Transform.Translation(0.0, 0.0, z6 - ((Point3d)(ref val10)).Z));
		val12 = Intersection.CurveCurve(curve4, (Curve)(object)val9, CSApplicationHelper.Application.Tolerance, CSApplicationHelper.Application.Tolerance);
		val13 = Intersection.CurveCurve(curve4, (Curve)(object)val11, CSApplicationHelper.Application.Tolerance, CSApplicationHelper.Application.Tolerance);
		curve4.ClosestPoint(val12[0].PointA, ref num5);
		curve4.ClosestPoint(val13[0].PointA, ref num6);
		curve4 = curve4.Trim(num5, num6);
		BoundingBox boundingBox4 = ((GeometryBase)headMiddle).GetBoundingBox(true);
		diagonal = ((BoundingBox)(ref boundingBox4)).Diagonal;
		double x4 = ((Vector3d)(ref diagonal)).X;
		diagonal = ((BoundingBox)(ref boundingBox4)).Diagonal;
		cSGemData.ScaleToSize(x4, ((Vector3d)(ref diagonal)).Y);
		val10 = ((BoundingBox)(ref boundingBox4)).Max;
		double z7 = ((Point3d)(ref val10)).Z;
		val14 = Transform.Translation(0.0, 0.0, z7 + seatHeight);
		if (!GemOffsetCurveV2.GemOffset(cSGemData, num3 + girdleSpacing, out Curve curve5, doTransformUp: false, out error, 0.0))
		{
			return null;
		}
		((GeometryBase)curve5).Transform(val14);
		curve = cSGemData.GetCurve();
		val2 = curve.PointAt(prongPositions[prongPointIndex1].ProngParam);
		val3 = curve.PointAt(prongPositions[prongPointIndex2].ProngParam);
		val8 = new Line(val2, -val6, (num3 + girdleSpacing) * 3.0);
		val9 = ((Line)(ref val8)).ToNurbsCurve();
		NurbsCurve obj7 = val9;
		val10 = curve5.PointAtStart;
		double z8 = ((Point3d)(ref val10)).Z;
		val10 = ((Curve)val9).PointAtStart;
		((GeometryBase)obj7).Transform(Transform.Translation(0.0, 0.0, z8 - ((Point3d)(ref val10)).Z));
		val8 = new Line(val3, -val7, (num3 + girdleSpacing) * 3.0);
		val11 = ((Line)(ref val8)).ToNurbsCurve();
		NurbsCurve obj8 = val11;
		val10 = curve5.PointAtStart;
		double z9 = ((Point3d)(ref val10)).Z;
		val10 = ((Curve)val11).PointAtStart;
		((GeometryBase)obj8).Transform(Transform.Translation(0.0, 0.0, z9 - ((Point3d)(ref val10)).Z));
		val12 = Intersection.CurveCurve(curve5, (Curve)(object)val9, CSApplicationHelper.Application.Tolerance, CSApplicationHelper.Application.Tolerance);
		val13 = Intersection.CurveCurve(curve5, (Curve)(object)val11, CSApplicationHelper.Application.Tolerance, CSApplicationHelper.Application.Tolerance);
		curve5.ClosestPoint(val12[0].PointA, ref num5);
		curve5.ClosestPoint(val13[0].PointA, ref num6);
		curve5 = curve5.Trim(num5, num6);
		List<Curve> list = new List<Curve>();
		val10 = ((BoundingBox)(ref boundingBox4)).Max;
		double z10 = ((Point3d)(ref val10)).Z;
		val10 = curve4.PointAtEnd;
		if (!(z10 <= ((Point3d)(ref val10)).Z + 0.1))
		{
			val10 = ((BoundingBox)(ref boundingBox4)).Min;
			double z11 = ((Point3d)(ref val10)).Z;
			val10 = curve2.PointAtEnd;
			if (!(z11 >= ((Point3d)(ref val10)).Z - 0.6))
			{
				list.Add(curve3);
				list.Add(curve2);
				list.Add(curve5);
				list.Add(curve4);
				goto IL_0a14;
			}
		}
		list.Add(curve3);
		list.Add(curve2);
		list.Add(curve4);
		goto IL_0a14;
		IL_0a14:
		val10 = curve3.PointAtEnd;
		double z12 = ((Point3d)(ref val10)).Z;
		val10 = curve2.PointAtEnd;
		if (Math.Abs(z12 - ((Point3d)(ref val10)).Z) < CSApplicationHelper.Application.Tolerance)
		{
			list.Remove(curve3);
		}
		Brep[] array = Brep.CreateFromLoft((IEnumerable<Curve>)list, Point3d.Unset, Point3d.Unset, (LoftType)0, false);
		if (array.Length < 1)
		{
			return null;
		}
		return array[0];
	}

	private void PullToBase(CSDataArray baseObject, CSData gems, ref Brep inBezel)
	{
		//IL_012e: Unknown result type (might be due to invalid IL or missing references)
		//IL_007b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0089: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b1: Unknown result type (might be due to invalid IL or missing references)
		//IL_00bf: Unknown result type (might be due to invalid IL or missing references)
		//IL_01b4: Unknown result type (might be due to invalid IL or missing references)
		//IL_01b6: Unknown result type (might be due to invalid IL or missing references)
		//IL_01bb: Unknown result type (might be due to invalid IL or missing references)
		//IL_015a: Unknown result type (might be due to invalid IL or missing references)
		//IL_015f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0163: Unknown result type (might be due to invalid IL or missing references)
		//IL_016c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0178: Unknown result type (might be due to invalid IL or missing references)
		//IL_01ab: Unknown result type (might be due to invalid IL or missing references)
		//IL_01b0: Unknown result type (might be due to invalid IL or missing references)
		//IL_01dc: Unknown result type (might be due to invalid IL or missing references)
		//IL_01df: Unknown result type (might be due to invalid IL or missing references)
		//IL_01e1: Unknown result type (might be due to invalid IL or missing references)
		//IL_01d1: Unknown result type (might be due to invalid IL or missing references)
		if (baseObject == null)
		{
			return;
		}
		Brep val = null;
		double num = double.NaN;
		CSData cSData = baseObject.GetArrayItem(0);
		if (cSData is CSRingRailData cSRingRailData)
		{
			num = cSRingRailData.Radius;
			cSData = null;
		}
		if (cSData != null)
		{
			Curve val2 = (cSData as ICSCurve)?.GetCurve();
			if (val2 != null)
			{
				Curve val3 = val2.DuplicateCurve();
				Curve val4 = val2.DuplicateCurve();
				((GeometryBase)val3).Translate(new Vector3d(0.0, 1.0, 0.0) * -50.0);
				((GeometryBase)val4).Translate(new Vector3d(0.0, 1.0, 0.0) * 50.0);
				val = ((Surface)NurbsSurface.CreateRuledSurface(val3, val4)).ToBrep();
			}
			if (val == null && cSData is ICSBrep iCSBrep)
			{
				val = iCSBrep.GetBrep();
			}
			if (val == null)
			{
				Surface val5 = (Surface)(object)(cSData as ICSSurface)?.GetNurbsSurface();
				if (val5 != null)
				{
					val = val5.ToBrep();
				}
			}
		}
		if ((double.IsNaN(num) && val == null) || !(gems is CSGemData cSGemData))
		{
			return;
		}
		CSBlockUtil.CSUtilMath.GetUnscaledXform(cSGemData.Xform, out var xUp);
		Transform val6 = default(Transform);
		if (((Transform)(ref xUp)).TryGetInverse(ref val6))
		{
			double angle = double.NaN;
			if (!double.IsNaN(num))
			{
				Point3d origin = Point3d.Origin;
				((Point3d)(ref origin)).Transform(xUp);
				Point3d val7 = default(Point3d);
				((Point3d)(ref val7))._002Ector(Vector3d.ZAxis);
				((Point3d)(ref val7)).Transform(xUp);
				angle = CSBlockUtil.CSUtilMath.AngleOfPointD(((Point3d)(ref val7)).X - ((Point3d)(ref origin)).X, ((Point3d)(ref val7)).Z - ((Point3d)(ref origin)).Z);
				val6 = PullToRailMorph.TopCenterXformFromAngle(angle).Convert();
			}
			BoundingBox boundingBox = ((GeometryBase)inBezel).GetBoundingBox(val6);
			SpaceMorph val8 = (SpaceMorph)(double.IsNaN(num) ? ((object)new CSPullToBaseBrep(boundingBox, val, xUp, val6)) : ((object)new CSPullToRailCenterMorph(angle, num, 1.0, boundingBox)));
			val8.Tolerance = 0.05;
			val8.PreserveStructure = true;
			Brep val9 = inBezel.DuplicateBrep();
			val8.Morph((GeometryBase)(object)val9);
			if (val9 != null)
			{
				inBezel = val9.DuplicateBrep();
			}
		}
	}

	private bool CSHeadShape(CSData iGem, double prongHeight, double headHeight, CSDataArray baseObject, double baseOffset, double angleX, double angleY, double topAngle, double baseAngle, double baseAnglePosition, List<int> prongPointList, List<int> prongType, List<double> rotateAngle, List<bool> straightProng, double cornerBlend, Point3d focalPoint, List<bool> straightProngToggle, out Curve headGirdle, out Curve headTop, out Curve headBase, out Curve headMiddleCurve, out Curve outHeadSideX, out Curve outHeadSideY, out List<Curve> headProngLines, out CSData outBaseObject, out double gemHeight, out List<Brep> surfaceBetweenProngs)
	{
		//IL_00b8: Unknown result type (might be due to invalid IL or missing references)
		//IL_0123: Unknown result type (might be due to invalid IL or missing references)
		//IL_013b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0140: Unknown result type (might be due to invalid IL or missing references)
		//IL_0155: Unknown result type (might be due to invalid IL or missing references)
		//IL_015a: Unknown result type (might be due to invalid IL or missing references)
		//IL_015c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0161: Unknown result type (might be due to invalid IL or missing references)
		//IL_016e: Unknown result type (might be due to invalid IL or missing references)
		//IL_01d4: Unknown result type (might be due to invalid IL or missing references)
		//IL_0206: Unknown result type (might be due to invalid IL or missing references)
		//IL_020b: Unknown result type (might be due to invalid IL or missing references)
		//IL_022d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0232: Unknown result type (might be due to invalid IL or missing references)
		//IL_0234: Unknown result type (might be due to invalid IL or missing references)
		//IL_0239: Unknown result type (might be due to invalid IL or missing references)
		//IL_0246: Unknown result type (might be due to invalid IL or missing references)
		//IL_0279: Unknown result type (might be due to invalid IL or missing references)
		//IL_02ab: Unknown result type (might be due to invalid IL or missing references)
		//IL_02b0: Unknown result type (might be due to invalid IL or missing references)
		//IL_02c5: Unknown result type (might be due to invalid IL or missing references)
		//IL_02ca: Unknown result type (might be due to invalid IL or missing references)
		//IL_02cc: Unknown result type (might be due to invalid IL or missing references)
		//IL_02d1: Unknown result type (might be due to invalid IL or missing references)
		//IL_02de: Unknown result type (might be due to invalid IL or missing references)
		//IL_0303: Unknown result type (might be due to invalid IL or missing references)
		//IL_0342: Unknown result type (might be due to invalid IL or missing references)
		//IL_0366: Unknown result type (might be due to invalid IL or missing references)
		//IL_0373: Unknown result type (might be due to invalid IL or missing references)
		//IL_037b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0383: Unknown result type (might be due to invalid IL or missing references)
		//IL_038b: Unknown result type (might be due to invalid IL or missing references)
		//IL_03b6: Unknown result type (might be due to invalid IL or missing references)
		//IL_03f5: Unknown result type (might be due to invalid IL or missing references)
		//IL_0419: Unknown result type (might be due to invalid IL or missing references)
		//IL_0426: Unknown result type (might be due to invalid IL or missing references)
		//IL_042e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0436: Unknown result type (might be due to invalid IL or missing references)
		//IL_043e: Unknown result type (might be due to invalid IL or missing references)
		//IL_04c4: Unknown result type (might be due to invalid IL or missing references)
		//IL_04c9: Unknown result type (might be due to invalid IL or missing references)
		//IL_04cb: Unknown result type (might be due to invalid IL or missing references)
		//IL_04d0: Unknown result type (might be due to invalid IL or missing references)
		//IL_04db: Unknown result type (might be due to invalid IL or missing references)
		//IL_04e0: Unknown result type (might be due to invalid IL or missing references)
		//IL_04e7: Expected O, but got Unknown
		//IL_04ec: Unknown result type (might be due to invalid IL or missing references)
		//IL_04f1: Unknown result type (might be due to invalid IL or missing references)
		//IL_04f6: Unknown result type (might be due to invalid IL or missing references)
		//IL_04fb: Unknown result type (might be due to invalid IL or missing references)
		//IL_04ff: Unknown result type (might be due to invalid IL or missing references)
		//IL_054f: Unknown result type (might be due to invalid IL or missing references)
		//IL_055b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0564: Unknown result type (might be due to invalid IL or missing references)
		//IL_056d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0582: Unknown result type (might be due to invalid IL or missing references)
		//IL_058b: Unknown result type (might be due to invalid IL or missing references)
		//IL_05a6: Unknown result type (might be due to invalid IL or missing references)
		//IL_05af: Unknown result type (might be due to invalid IL or missing references)
		//IL_05bc: Unknown result type (might be due to invalid IL or missing references)
		//IL_05c4: Unknown result type (might be due to invalid IL or missing references)
		//IL_05cc: Unknown result type (might be due to invalid IL or missing references)
		//IL_05d4: Unknown result type (might be due to invalid IL or missing references)
		//IL_05dd: Unknown result type (might be due to invalid IL or missing references)
		//IL_05df: Unknown result type (might be due to invalid IL or missing references)
		//IL_059d: Unknown result type (might be due to invalid IL or missing references)
		//IL_05a2: Unknown result type (might be due to invalid IL or missing references)
		//IL_06cc: Unknown result type (might be due to invalid IL or missing references)
		//IL_06dc: Unknown result type (might be due to invalid IL or missing references)
		//IL_06e1: Unknown result type (might be due to invalid IL or missing references)
		//IL_05ee: Unknown result type (might be due to invalid IL or missing references)
		//IL_05f0: Unknown result type (might be due to invalid IL or missing references)
		//IL_05f4: Unknown result type (might be due to invalid IL or missing references)
		//IL_0633: Unknown result type (might be due to invalid IL or missing references)
		//IL_0638: Unknown result type (might be due to invalid IL or missing references)
		//IL_0640: Unknown result type (might be due to invalid IL or missing references)
		//IL_0645: Unknown result type (might be due to invalid IL or missing references)
		//IL_0652: Unknown result type (might be due to invalid IL or missing references)
		//IL_0657: Unknown result type (might be due to invalid IL or missing references)
		//IL_066f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0674: Unknown result type (might be due to invalid IL or missing references)
		//IL_0681: Unknown result type (might be due to invalid IL or missing references)
		//IL_0686: Unknown result type (might be due to invalid IL or missing references)
		//IL_069c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0873: Unknown result type (might be due to invalid IL or missing references)
		gemHeight = 0.0;
		headGirdle = null;
		headTop = null;
		headBase = null;
		headMiddleCurve = null;
		outHeadSideX = null;
		outHeadSideY = null;
		headProngLines = new List<Curve>();
		outBaseObject = null;
		surfaceBetweenProngs = new List<Brep>();
		if (iGem == null)
		{
			return false;
		}
		if (prongHeight < 0.01)
		{
			prongHeight = 0.01;
		}
		if (!(iGem is CSGemData cSGemData))
		{
			return false;
		}
		double drop = GetDrop(headHeight, baseObject, baseOffset, cSGemData);
		if (!CSDataHelper.SetSettingType(cSGemData, GemSettingType.PR))
		{
			return false;
		}
		if (!cSGemData.GetGemXformRC(out var xDown, out var xUp))
		{
			return false;
		}
		double gemXSize = cSGemData.GemXSize;
		double gemYSize = cSGemData.GemYSize;
		double gemXMax = cSGemData.GemXMax;
		double gemYMax = cSGemData.GemYMax;
		Curve curve = cSGemData.GetCurve();
		if (curve == null)
		{
			return false;
		}
		((GeometryBase)curve).Transform(xDown);
		if (!GemOffsetCurveV2.GemOffset(cSGemData, 0.0, out Curve curve2, doTransformUp: false, out string _, cornerBlend))
		{
			return false;
		}
		((GeometryBase)curve2).MakeDeformable();
		double num = drop * Math.Tan(CSBlockUtil.CSUtilMath.ToRadians(angleX));
		double num2 = drop * Math.Tan(CSBlockUtil.CSUtilMath.ToRadians(angleY));
		double num3 = gemXSize + num * 2.0;
		double num4 = gemYSize + num2 * 2.0;
		Transform val = Transform.Scale(Plane.WorldXY, num3 / gemXSize, num4 / gemYSize, 1.0);
		Transform val2 = Transform.Translation(0.0, 0.0, drop) * val;
		Curve val3 = curve2.DuplicateCurve();
		((GeometryBase)val3).Transform(val2);
		if (baseAnglePosition < 0.0)
		{
			baseAnglePosition = 0.0;
		}
		if (baseAnglePosition > 1.0)
		{
			baseAnglePosition = 1.0;
		}
		double num5 = (0.0 - drop) * baseAnglePosition * Math.Tan(CSBlockUtil.CSUtilMath.ToRadians(angleX + baseAngle));
		double num6 = (0.0 - drop) * baseAnglePosition * Math.Tan(CSBlockUtil.CSUtilMath.ToRadians(angleY + baseAngle));
		Transform val4 = Transform.Scale(Plane.WorldXY, (num3 + num5 * 2.0) / gemXSize, (num4 + num6 * 2.0) / gemYSize, 1.0);
		Transform val5 = Transform.Translation(0.0, 0.0, drop * (1.0 - baseAnglePosition)) * val4;
		Curve val6 = curve2.DuplicateCurve();
		((GeometryBase)val6).Transform(val5);
		headMiddleCurve = val6;
		double num7 = prongHeight * Math.Tan(CSBlockUtil.CSUtilMath.ToRadians(angleX + topAngle));
		double num8 = prongHeight * Math.Tan(CSBlockUtil.CSUtilMath.ToRadians(angleY + topAngle));
		Transform val7 = Transform.Scale(Plane.WorldXY, (gemXSize + num7 * 2.0) / gemXSize, (gemYSize + num8 * 2.0) / gemYSize, 1.0);
		Transform val8 = Transform.Translation(0.0, 0.0, prongHeight) * val7;
		Curve val9 = curve2.DuplicateCurve();
		((GeometryBase)val9).Transform(val8);
		Point3d item = default(Point3d);
		((Point3d)(ref item))._002Ector(gemXMax, 0.0, 0.0);
		((Point3d)(ref item)).Transform(val8);
		Point3d item2 = default(Point3d);
		((Point3d)(ref item2))._002Ector(gemXMax, 0.0, 0.0);
		Point3d item3 = default(Point3d);
		((Point3d)(ref item3))._002Ector(gemXMax, 0.0, 0.0);
		((Point3d)(ref item3)).Transform(val5);
		Point3d item4 = default(Point3d);
		((Point3d)(ref item4))._002Ector(gemXMax, 0.0, 0.0);
		((Point3d)(ref item4)).Transform(val2);
		Curve val10 = ProngInterpCurve(new List<Point3d> { item, item2, item3, item4 });
		Point3d item5 = default(Point3d);
		((Point3d)(ref item5))._002Ector(0.0, gemYMax, 0.0);
		((Point3d)(ref item5)).Transform(val8);
		Point3d item6 = default(Point3d);
		((Point3d)(ref item6))._002Ector(0.0, gemYMax, 0.0);
		Point3d item7 = default(Point3d);
		((Point3d)(ref item7))._002Ector(0.0, gemYMax, 0.0);
		((Point3d)(ref item7)).Transform(val5);
		Point3d item8 = default(Point3d);
		((Point3d)(ref item8))._002Ector(0.0, gemYMax, 0.0);
		((Point3d)(ref item8)).Transform(val2);
		Curve val11 = ProngInterpCurve(new List<Point3d> { item5, item6, item7, item8 });
		List<CSGemInstanceDefinition.ProngPosition> list = new List<CSGemInstanceDefinition.ProngPosition>();
		if (cSGemData.Def != null)
		{
			list = cSGemData.Def.ProngPositions;
		}
		if (prongPointList.Count != prongType.Count)
		{
			return false;
		}
		int num9 = 0;
		List<double> list2 = new List<double>();
		List<double> list3 = new List<double>();
		List<Curve> list4 = new List<Curve>();
		Point3d val16 = default(Point3d);
		Point3d val17 = default(Point3d);
		double item9 = default(double);
		Point3d unset = default(Point3d);
		Point3d item10 = default(Point3d);
		double num12 = default(double);
		for (int i = 0; i < prongPointList.Count; i++)
		{
			double angleDegrees = rotateAngle[num9];
			num9++;
			Point3d val12 = curve.PointAt(list[prongPointList[i]].ProngParam);
			LineCurve val13 = new LineCurve(Point3d.Origin, val12 * 50.0);
			Transform val14 = CSBlockUtil.CSUtilMath.Rotation(CSBlockUtil.CSUtilMath.ToRadians(angleDegrees), Vector3d.ZAxis, Point3d.Origin);
			((GeometryBase)val13).Transform(val14);
			CurveIntersections val15 = Intersection.CurveCurve(curve2, (Curve)(object)val13, 0.001, 0.001);
			if (val15.Count <= 0)
			{
				continue;
			}
			double parameterA = val15[0].ParameterA;
			list2.Add(parameterA);
			((Point3d)(ref val16))._002Ector(curve2.PointAt(parameterA));
			((Point3d)(ref val17))._002Ector(val16);
			((Point3d)(ref val17)).Transform(val2);
			val3.ClosestPoint(val17, ref item9);
			list3.Add(item9);
			((Point3d)(ref unset))._002Ector(val16);
			((Point3d)(ref unset)).Transform(val5);
			if (straightProng[i])
			{
				unset = Point3d.Unset;
			}
			((Point3d)(ref item10))._002Ector(val16);
			((Point3d)(ref item10)).Transform(val8);
			List<Point3d> list5 = new List<Point3d> { item10, val16, unset, val17 };
			if (focalPoint != Point3d.Unset)
			{
				Point3d val18 = focalPoint;
				((Point3d)(ref val18)).Transform(xDown);
				double num10 = ((Point3d)(ref val18)).X / ((Point3d)(ref val18)).Z;
				double num11 = ((Point3d)(ref val18)).Y / ((Point3d)(ref val18)).Z;
				for (int j = 0; j < list5.Count; j++)
				{
					if (!straightProngToggle[j])
					{
						Point3d value = list5[j];
						Point3d val19 = list5[j];
						double x = ((Point3d)(ref val19)).X;
						val19 = list5[j];
						((Point3d)(ref value)).X = x + ((Point3d)(ref val19)).Z * num10;
						val19 = list5[j];
						double y = ((Point3d)(ref val19)).Y;
						val19 = list5[j];
						((Point3d)(ref value)).Y = y + ((Point3d)(ref val19)).Z * num11;
						list5[j] = value;
					}
				}
			}
			Curve val20 = ProngInterpCurve(list5);
			headProngLines.Add(val20);
			val20.ClosestPoint(val16, ref num12);
			double num13 = num12;
			Interval domain = val20.Domain;
			Curve item11 = val20.Trim(num13, ((Interval)(ref domain)).Max);
			list4.Add(item11);
		}
		headGirdle = curve2.DuplicateCurve();
		headTop = val9;
		headBase = val3;
		outHeadSideX = val10;
		outHeadSideY = val11;
		outBaseObject = baseObject;
		gemHeight = cSGemData.GemZMin;
		if (!InToggleSrfBetweenProngs.GetValue(out bool value2))
		{
			return InputError();
		}
		if (value2)
		{
			for (int k = 0; k < headProngLines.Count; k++)
			{
				List<Curve> list6 = new List<Curve>();
				List<double> list7 = new List<double>();
				List<double> list8 = new List<double>();
				if (k < list4.Count - 1)
				{
					list6.Add(list4[k]);
					list6.Add(list4[k + 1]);
					list7.Add(list2[k]);
					list7.Add(list2[k + 1]);
					list8.Add(list3[k]);
					list8.Add(list3[k + 1]);
				}
				else
				{
					list6.Add(list4[k]);
					list6.Add(list4[0]);
					list7.Add(list2[k]);
					list7.Add(list2[0]);
					list8.Add(list3[k]);
					list8.Add(list3[0]);
				}
				CSUtilSweepRC.Sweep2(headGirdle, headBase, list6, list7, list8, out Brep[] breps, closed: false, maintainHeight: true);
				if (breps.Length < 1)
				{
					return UpdateError("Error making inbetween surface");
				}
				((GeometryBase)breps[0]).Transform(xUp);
				Brep item12 = breps[0].DuplicateBrep();
				surfaceBetweenProngs.Add(item12);
			}
		}
		return true;
	}

	private static Curve ProngInterpCurve(List<Point3d> listPoints)
	{
		//IL_0006: Unknown result type (might be due to invalid IL or missing references)
		//IL_000b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0012: Unknown result type (might be due to invalid IL or missing references)
		//IL_005b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0062: Unknown result type (might be due to invalid IL or missing references)
		//IL_0067: Unknown result type (might be due to invalid IL or missing references)
		//IL_006d: Expected O, but got Unknown
		//IL_0070: Unknown result type (might be due to invalid IL or missing references)
		//IL_0085: Unknown result type (might be due to invalid IL or missing references)
		//IL_0092: Unknown result type (might be due to invalid IL or missing references)
		//IL_00aa: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b8: Unknown result type (might be due to invalid IL or missing references)
		//IL_00bd: Unknown result type (might be due to invalid IL or missing references)
		for (int i = 0; i < listPoints.Count - 1; i++)
		{
			Point3d val = listPoints[i];
			if (((Point3d)(ref val)).DistanceTo(listPoints[i + 1]) < 0.01)
			{
				listPoints.RemoveAt(i);
				i--;
			}
		}
		List<Point3d> list = new List<Point3d>();
		if (listPoints.Count > 2)
		{
			list.AddRange(listPoints);
		}
		else
		{
			LineCurve val2 = new LineCurve(listPoints[0], listPoints[1]);
			list.Add(listPoints[0]);
			list.Add(((Curve)val2).PointAt(0.1));
			list.Add(listPoints[1]);
		}
		return (Curve)(((object)Curve.CreateInterpolatedCurve((IEnumerable<Point3d>)list, 3, (CurveKnotStyle)1)) ?? ((object)new LineCurve(listPoints[0], listPoints[listPoints.Count - 1])));
	}

	private double GetDrop(double vBelowInput, CSDataArray vBaseObject, double vBaseOffset, CSGemData gem)
	{
		//IL_0039: Unknown result type (might be due to invalid IL or missing references)
		//IL_0047: Unknown result type (might be due to invalid IL or missing references)
		//IL_0054: Unknown result type (might be due to invalid IL or missing references)
		//IL_0062: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c1: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e1: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e6: Unknown result type (might be due to invalid IL or missing references)
		//IL_00eb: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ec: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f5: Unknown result type (might be due to invalid IL or missing references)
		//IL_00fd: Unknown result type (might be due to invalid IL or missing references)
		//IL_0105: Unknown result type (might be due to invalid IL or missing references)
		//IL_0111: Unknown result type (might be due to invalid IL or missing references)
		//IL_0133: Expected O, but got Unknown
		//IL_0147: Unknown result type (might be due to invalid IL or missing references)
		//IL_014c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0168: Unknown result type (might be due to invalid IL or missing references)
		//IL_0174: Unknown result type (might be due to invalid IL or missing references)
		//IL_0179: Unknown result type (might be due to invalid IL or missing references)
		//IL_017d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0195: Unknown result type (might be due to invalid IL or missing references)
		//IL_0197: Unknown result type (might be due to invalid IL or missing references)
		//IL_01b8: Unknown result type (might be due to invalid IL or missing references)
		//IL_01bd: Unknown result type (might be due to invalid IL or missing references)
		//IL_01c1: Unknown result type (might be due to invalid IL or missing references)
		//IL_01ef: Unknown result type (might be due to invalid IL or missing references)
		//IL_01f1: Unknown result type (might be due to invalid IL or missing references)
		//IL_01f3: Unknown result type (might be due to invalid IL or missing references)
		//IL_01f5: Unknown result type (might be due to invalid IL or missing references)
		//IL_01d9: Unknown result type (might be due to invalid IL or missing references)
		//IL_01db: Unknown result type (might be due to invalid IL or missing references)
		if (vBaseObject == null)
		{
			return vBelowInput + vBaseOffset;
		}
		CSData arrayItem = vBaseObject.GetArrayItem(0);
		Brep val = null;
		Curve val2 = (arrayItem as ICSCurve)?.GetCurve();
		if (val2 != null)
		{
			Curve val3 = val2.DuplicateCurve();
			Curve val4 = val2.DuplicateCurve();
			((GeometryBase)val3).Translate(Vector3d.YAxis * 50.0);
			((GeometryBase)val4).Translate(Vector3d.YAxis * -50.0);
			val = ((Surface)NurbsSurface.CreateRuledSurface(val3, val4)).ToBrep();
		}
		if (val == null && arrayItem is ICSBrep iCSBrep)
		{
			val = iCSBrep.GetBrep();
		}
		if (val == null)
		{
			return vBelowInput + vBaseOffset;
		}
		gem.GetGemXformRC(out var _, out var xUp);
		LineCurve val5 = new LineCurve(new Point3d(0.0, 0.0, 100.0), new Point3d(0.0, 0.0, -100.0));
		((GeometryBase)val5).Transform(xUp);
		Point3d val6 = default(Point3d);
		((Point3d)(ref val6)).Transform(xUp);
		Vector3d val7 = default(Vector3d);
		((Vector3d)(ref val7))._002Ector(Vector3d.ZAxis);
		((Vector3d)(ref val7)).Transform(xUp);
		((Vector3d)(ref val7)).Unitize();
		Curve[] array = default(Curve[]);
		Point3d[] array2 = default(Point3d[]);
		if (!Intersection.CurveBrep((Curve)val5, val, CSApplicationHelper.Application.Tolerance, ref array, ref array2))
		{
			return vBelowInput + vBaseOffset;
		}
		if (array2.Length == 0 && array.Length == 0)
		{
			return vBelowInput + vBaseOffset;
		}
		Point3d val8 = Point3d.Unset;
		double num = -1.23432101234321E+308;
		Curve[] array3 = array;
		double num2 = default(double);
		foreach (Curve obj in array3)
		{
			obj.ClosestPoint(val6, ref num2);
			Point3d val9 = obj.PointAt(num2);
			num2 = ((Point3d)(ref val9)).DistanceTo(val6);
			if (!RhinoMath.IsValidDouble(num) || num > num2)
			{
				val8 = val9;
				num = num2;
			}
		}
		Point3d[] array4 = array2;
		for (int i = 0; i < array4.Length; i++)
		{
			Point3d val10 = array4[i];
			double num3 = ((Point3d)(ref val10)).DistanceTo(val6);
			if (!RhinoMath.IsValidDouble(num) || num > num3)
			{
				val8 = val10;
				num = num3;
			}
		}
		if (val7 * (val6 - val8) < 0.0)
		{
			num *= -1.0;
		}
		return 0.0 - num + vBaseOffset;
	}

	private bool CSHeadProng(int prongNumber, CSData iGem, int prongArrangement, int prongtype, List<Curve> prongLines, Curve iProngRail, Curve iHeadTop, Curve iHeadBase, double prongVTopBlendAmount, double prongVBottomBlendAmount, Curve prongTopProfile, double prongTopX, double prongTopY, double prongVTopAngle, double prongTopNudge, Curve prongBaseProfile, double prongBaseX, double prongBaseY, double prongBaseNudge, double prongVBaseAngle, double domeRadius, double domeRotateX, double domeSettingAngle, int baseStyle, double basePointSize, bool enableDoItYourself, bool planarDome, bool planarVProng, double prongBaseDomeHeight, double overlap, double domeBlendAmountInside, double domeBlendAmountOutside, double domeBlendStartInside, double domeBlendStartOtuside, double domeBlendEndInside, double domeBlendEndOutside, Point3d focalPoint, out List<Brep> topDome, out List<Brep> middleProng, out List<Brep> lowerDome, out List<Brep> prongs, out List<Curve> curves)
	{
		//IL_00bb: Unknown result type (might be due to invalid IL or missing references)
		//IL_00cc: Unknown result type (might be due to invalid IL or missing references)
		//IL_01a8: Unknown result type (might be due to invalid IL or missing references)
		//IL_01c3: Unknown result type (might be due to invalid IL or missing references)
		//IL_0136: Unknown result type (might be due to invalid IL or missing references)
		//IL_024a: Unknown result type (might be due to invalid IL or missing references)
		//IL_025e: Unknown result type (might be due to invalid IL or missing references)
		//IL_016a: Unknown result type (might be due to invalid IL or missing references)
		prongs = new List<Brep>();
		curves = new List<Curve>();
		topDome = null;
		middleProng = null;
		lowerDome = null;
		if (!(iGem is CSGemData cSGemData))
		{
			return false;
		}
		if (prongTopProfile == null)
		{
			return false;
		}
		double prongTopOffset = 0.0 - prongTopY * prongTopNudge;
		Curve val = prongBaseProfile.DuplicateCurve();
		if (val == null)
		{
			return false;
		}
		double prongBaseOffset = 0.0 - prongBaseY * prongBaseNudge;
		if (!cSGemData.GetGemXformRC(out var _, out var xUp))
		{
			return false;
		}
		if (iHeadTop == null)
		{
			return false;
		}
		Curve val2 = iHeadTop.DuplicateCurve();
		Curve val3 = iHeadBase.DuplicateCurve();
		if (val3 == null)
		{
			return false;
		}
		if (!Curve.DoDirectionsMatch(val3, val2))
		{
			val3.Reverse();
		}
		Curve val4 = iProngRail.DuplicateCurve();
		if (val4 == null)
		{
			CS.CSUtil.Print("Missing Prong Rail\n");
			return false;
		}
		double paramTop = default(double);
		val2.ClosestPoint(val4.PointAtStart, ref paramTop);
		double paramBase = default(double);
		val3.ClosestPoint(val4.PointAtEnd, ref paramBase);
		if (prongtype == 1)
		{
			if (!MakeVProngs(val4, val2, paramTop, prongVTopAngle, prongTopY, prongTopOffset, prongVTopBlendAmount, prongVBottomBlendAmount, val3, paramBase, prongVBaseAngle, prongBaseY, prongBaseOffset, (ValueListItem)baseStyle, prongBaseDomeHeight, domeRadius, domeRotateX, enableDoItYourself, planarVProng, _camMode, out topDome, out middleProng, out lowerDome, out var outCurves, out var outProng, out var _))
			{
				return false;
			}
			foreach (Curve item in outCurves)
			{
				((GeometryBase)item).Transform(xUp);
				curves.Add(item);
			}
			if (outProng != null)
			{
				((GeometryBase)outProng).Transform(xUp);
				prongs.Add(outProng);
			}
		}
		else
		{
			if (!MakeProfileSweepProngs(prongNumber, prongLines, val4, cSGemData, val2, prongTopProfile, paramTop, prongTopX, prongTopY, prongTopOffset, val3, val, paramBase, prongBaseX, prongBaseY, prongBaseOffset, (ValueListItem)baseStyle, basePointSize, domeRadius, domeRotateX, domeSettingAngle, prongBaseDomeHeight, xUp, enableDoItYourself, planarDome, overlap, prongArrangement, _camMode, domeBlendAmountInside, domeBlendAmountOutside, domeBlendStartInside, domeBlendStartOtuside, domeBlendEndInside, domeBlendEndOutside, focalPoint, out topDome, out middleProng, out lowerDome, out var outCurves2, out var outProngs, out var _))
			{
				return false;
			}
			foreach (Curve item2 in outCurves2)
			{
				curves.Add(item2);
			}
			foreach (Brep item3 in outProngs)
			{
				prongs.Add(item3);
			}
		}
		((GeometryBase)val2).Transform(xUp);
		curves.Add(val2);
		((GeometryBase)val3).Transform(xUp);
		curves.Add(val3);
		return true;
	}

	private static bool CreateProngExtension(double prongExtensionHeight, ref Curve prongRail, ref Curve prongTopProfile)
	{
		//IL_0004: Unknown result type (might be due to invalid IL or missing references)
		//IL_0009: Unknown result type (might be due to invalid IL or missing references)
		//IL_0012: Unknown result type (might be due to invalid IL or missing references)
		//IL_0017: Unknown result type (might be due to invalid IL or missing references)
		//IL_0022: Unknown result type (might be due to invalid IL or missing references)
		//IL_0027: Unknown result type (might be due to invalid IL or missing references)
		//IL_0028: Unknown result type (might be due to invalid IL or missing references)
		//IL_0029: Unknown result type (might be due to invalid IL or missing references)
		//IL_002b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0030: Unknown result type (might be due to invalid IL or missing references)
		//IL_0035: Unknown result type (might be due to invalid IL or missing references)
		//IL_0036: Unknown result type (might be due to invalid IL or missing references)
		//IL_0037: Unknown result type (might be due to invalid IL or missing references)
		//IL_0038: Unknown result type (might be due to invalid IL or missing references)
		//IL_003d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0042: Unknown result type (might be due to invalid IL or missing references)
		//IL_0045: Unknown result type (might be due to invalid IL or missing references)
		//IL_004c: Unknown result type (might be due to invalid IL or missing references)
		//IL_004d: Unknown result type (might be due to invalid IL or missing references)
		//IL_004e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0055: Expected O, but got Unknown
		//IL_0070: Unknown result type (might be due to invalid IL or missing references)
		//IL_0075: Unknown result type (might be due to invalid IL or missing references)
		//IL_0076: Unknown result type (might be due to invalid IL or missing references)
		//IL_007b: Unknown result type (might be due to invalid IL or missing references)
		Curve obj = prongRail;
		Interval domain = prongRail.Domain;
		Vector3d val = obj.TangentAt(((Interval)(ref domain)).Min);
		((Vector3d)(ref val)).Unitize();
		Point3d pointAtStart = prongRail.PointAtStart;
		Point3d val2 = pointAtStart - val * prongExtensionHeight;
		Transform val3 = Transform.Translation(val2 - pointAtStart);
		((GeometryBase)prongTopProfile).Transform(val3);
		LineCurve val4 = new LineCurve(pointAtStart, val2);
		Curve val5 = Curve.JoinCurves((IEnumerable<Curve>)(object)new Curve[2]
		{
			prongRail,
			(Curve)val4
		})[0];
		Vector3d val6 = val5.PointAtStart - val2;
		if (((Vector3d)(ref val6)).Length > 0.01)
		{
			val5.Reverse();
		}
		prongRail = val5;
		return true;
	}

	private static bool ReduceProngRailLength(ref Curve prongRail, ref Curve prongBaseProfile, double trimLength)
	{
		//IL_0004: Unknown result type (might be due to invalid IL or missing references)
		//IL_0009: Unknown result type (might be due to invalid IL or missing references)
		//IL_0025: Unknown result type (might be due to invalid IL or missing references)
		//IL_002b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0030: Unknown result type (might be due to invalid IL or missing references)
		//IL_003c: Unknown result type (might be due to invalid IL or missing references)
		Curve obj = prongRail;
		Interval domain = prongRail.Domain;
		Curve[] array = obj.Split(((Interval)(ref domain)).Max - trimLength);
		prongRail = array[0];
		Curve val = array[1];
		Vector3d val2 = default(Vector3d);
		((Vector3d)(ref val2))._002Ector(val.PointAtStart - val.PointAtEnd);
		((GeometryBase)prongBaseProfile).Translate(val2);
		return true;
	}

	private static bool MakeProfileSweepProngs(int prongIndex, List<Curve> prongRailArray, Curve prongRail, CSGemData gem, Curve headTop, Curve inProngProfileTop, double paramTop, double prongTopX, double prongTopY, double prongTopOffset, Curve headBase, Curve inProngProfileBase, double paramBase, double prongBaseX, double prongBaseY, double prongBaseOffset, ValueListItem baseStyle, double basePointSize, double prongDomeHeight, double domeRotateX, double prongSettingAngle, double prongBaseDomeHeight, Transform xUp, bool enableDoItYourself, bool planarDome, double overlap, int prongArrangement, bool camMode, double domeBlendAmountInside, double domeBlendAmountOutside, double domeBlendStartInside, double domeBlendStartOtuside, double domeBlendEndInside, double domeBlendEndOutside, Point3d focalPoint, out List<Brep> topDome, out List<Brep> middleProng, out List<Brep> lowerDome, out List<Curve> outCurves, out List<Brep> outProngs, out string error)
	{
		//IL_0029: Unknown result type (might be due to invalid IL or missing references)
		//IL_002e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0030: Unknown result type (might be due to invalid IL or missing references)
		//IL_0035: Unknown result type (might be due to invalid IL or missing references)
		//IL_0043: Unknown result type (might be due to invalid IL or missing references)
		//IL_0048: Unknown result type (might be due to invalid IL or missing references)
		//IL_0051: Unknown result type (might be due to invalid IL or missing references)
		//IL_0056: Unknown result type (might be due to invalid IL or missing references)
		//IL_0057: Unknown result type (might be due to invalid IL or missing references)
		//IL_005c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0068: Unknown result type (might be due to invalid IL or missing references)
		//IL_006d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0071: Unknown result type (might be due to invalid IL or missing references)
		//IL_0076: Unknown result type (might be due to invalid IL or missing references)
		//IL_0083: Unknown result type (might be due to invalid IL or missing references)
		//IL_0088: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ae: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b5: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ba: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c3: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c8: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e3: Unknown result type (might be due to invalid IL or missing references)
		//IL_00fe: Unknown result type (might be due to invalid IL or missing references)
		//IL_0103: Unknown result type (might be due to invalid IL or missing references)
		//IL_013c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0141: Unknown result type (might be due to invalid IL or missing references)
		//IL_0143: Unknown result type (might be due to invalid IL or missing references)
		//IL_0148: Unknown result type (might be due to invalid IL or missing references)
		//IL_014d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0152: Unknown result type (might be due to invalid IL or missing references)
		//IL_0153: Unknown result type (might be due to invalid IL or missing references)
		//IL_0154: Unknown result type (might be due to invalid IL or missing references)
		//IL_0159: Unknown result type (might be due to invalid IL or missing references)
		//IL_015e: Unknown result type (might be due to invalid IL or missing references)
		//IL_016b: Unknown result type (might be due to invalid IL or missing references)
		//IL_016d: Unknown result type (might be due to invalid IL or missing references)
		//IL_016f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0174: Unknown result type (might be due to invalid IL or missing references)
		//IL_0176: Unknown result type (might be due to invalid IL or missing references)
		//IL_0181: Unknown result type (might be due to invalid IL or missing references)
		//IL_018f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0195: Unknown result type (might be due to invalid IL or missing references)
		//IL_019a: Unknown result type (might be due to invalid IL or missing references)
		//IL_019b: Unknown result type (might be due to invalid IL or missing references)
		//IL_01a0: Unknown result type (might be due to invalid IL or missing references)
		//IL_0109: Unknown result type (might be due to invalid IL or missing references)
		//IL_0121: Unknown result type (might be due to invalid IL or missing references)
		//IL_0126: Unknown result type (might be due to invalid IL or missing references)
		//IL_01a8: Unknown result type (might be due to invalid IL or missing references)
		//IL_01c8: Unknown result type (might be due to invalid IL or missing references)
		//IL_01cd: Unknown result type (might be due to invalid IL or missing references)
		//IL_01b6: Unknown result type (might be due to invalid IL or missing references)
		//IL_01b7: Unknown result type (might be due to invalid IL or missing references)
		//IL_01da: Unknown result type (might be due to invalid IL or missing references)
		//IL_01df: Unknown result type (might be due to invalid IL or missing references)
		//IL_01e3: Unknown result type (might be due to invalid IL or missing references)
		//IL_01e8: Unknown result type (might be due to invalid IL or missing references)
		//IL_01f5: Unknown result type (might be due to invalid IL or missing references)
		//IL_01fa: Unknown result type (might be due to invalid IL or missing references)
		//IL_0220: Unknown result type (might be due to invalid IL or missing references)
		//IL_0227: Unknown result type (might be due to invalid IL or missing references)
		//IL_022c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0235: Unknown result type (might be due to invalid IL or missing references)
		//IL_023a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0256: Unknown result type (might be due to invalid IL or missing references)
		//IL_0258: Unknown result type (might be due to invalid IL or missing references)
		//IL_026f: Unknown result type (might be due to invalid IL or missing references)
		//IL_028a: Unknown result type (might be due to invalid IL or missing references)
		//IL_028f: Unknown result type (might be due to invalid IL or missing references)
		//IL_02b6: Unknown result type (might be due to invalid IL or missing references)
		//IL_02bb: Unknown result type (might be due to invalid IL or missing references)
		//IL_02bd: Unknown result type (might be due to invalid IL or missing references)
		//IL_02c2: Unknown result type (might be due to invalid IL or missing references)
		//IL_02c7: Unknown result type (might be due to invalid IL or missing references)
		//IL_02cc: Unknown result type (might be due to invalid IL or missing references)
		//IL_02d7: Unknown result type (might be due to invalid IL or missing references)
		//IL_02d3: Unknown result type (might be due to invalid IL or missing references)
		//IL_02d8: Unknown result type (might be due to invalid IL or missing references)
		//IL_02dd: Unknown result type (might be due to invalid IL or missing references)
		//IL_02e2: Unknown result type (might be due to invalid IL or missing references)
		//IL_02ef: Unknown result type (might be due to invalid IL or missing references)
		//IL_02f1: Unknown result type (might be due to invalid IL or missing references)
		//IL_02f3: Unknown result type (might be due to invalid IL or missing references)
		//IL_02f8: Unknown result type (might be due to invalid IL or missing references)
		//IL_02fa: Unknown result type (might be due to invalid IL or missing references)
		//IL_0320: Unknown result type (might be due to invalid IL or missing references)
		//IL_0325: Unknown result type (might be due to invalid IL or missing references)
		//IL_0466: Unknown result type (might be due to invalid IL or missing references)
		//IL_0468: Unknown result type (might be due to invalid IL or missing references)
		//IL_046a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0337: Unknown result type (might be due to invalid IL or missing references)
		//IL_033c: Unknown result type (might be due to invalid IL or missing references)
		//IL_047b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0429: Unknown result type (might be due to invalid IL or missing references)
		//IL_038d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0392: Unknown result type (might be due to invalid IL or missing references)
		//IL_0393: Unknown result type (might be due to invalid IL or missing references)
		//IL_0398: Unknown result type (might be due to invalid IL or missing references)
		//IL_039d: Unknown result type (might be due to invalid IL or missing references)
		//IL_03a2: Unknown result type (might be due to invalid IL or missing references)
		//IL_03a3: Unknown result type (might be due to invalid IL or missing references)
		//IL_03a4: Unknown result type (might be due to invalid IL or missing references)
		//IL_03a9: Unknown result type (might be due to invalid IL or missing references)
		//IL_03ae: Unknown result type (might be due to invalid IL or missing references)
		//IL_03b2: Unknown result type (might be due to invalid IL or missing references)
		//IL_03b4: Unknown result type (might be due to invalid IL or missing references)
		//IL_03b6: Unknown result type (might be due to invalid IL or missing references)
		//IL_03c1: Unknown result type (might be due to invalid IL or missing references)
		//IL_03cf: Unknown result type (might be due to invalid IL or missing references)
		//IL_03d5: Unknown result type (might be due to invalid IL or missing references)
		//IL_03da: Unknown result type (might be due to invalid IL or missing references)
		//IL_03db: Unknown result type (might be due to invalid IL or missing references)
		//IL_03e0: Unknown result type (might be due to invalid IL or missing references)
		//IL_05d3: Unknown result type (might be due to invalid IL or missing references)
		//IL_05d5: Unknown result type (might be due to invalid IL or missing references)
		//IL_05d6: Unknown result type (might be due to invalid IL or missing references)
		//IL_03eb: Unknown result type (might be due to invalid IL or missing references)
		//IL_04ad: Unknown result type (might be due to invalid IL or missing references)
		//IL_04c7: Unknown result type (might be due to invalid IL or missing references)
		//IL_04cc: Unknown result type (might be due to invalid IL or missing references)
		//IL_04f1: Unknown result type (might be due to invalid IL or missing references)
		//IL_04f8: Unknown result type (might be due to invalid IL or missing references)
		//IL_04fd: Unknown result type (might be due to invalid IL or missing references)
		//IL_0506: Unknown result type (might be due to invalid IL or missing references)
		//IL_050b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0555: Unknown result type (might be due to invalid IL or missing references)
		//IL_055a: Unknown result type (might be due to invalid IL or missing references)
		//IL_055b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0560: Unknown result type (might be due to invalid IL or missing references)
		//IL_0565: Unknown result type (might be due to invalid IL or missing references)
		//IL_056a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0575: Unknown result type (might be due to invalid IL or missing references)
		//IL_0571: Unknown result type (might be due to invalid IL or missing references)
		//IL_0603: Unknown result type (might be due to invalid IL or missing references)
		//IL_060a: Expected O, but got Unknown
		//IL_0576: Unknown result type (might be due to invalid IL or missing references)
		//IL_057b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0580: Unknown result type (might be due to invalid IL or missing references)
		//IL_058d: Unknown result type (might be due to invalid IL or missing references)
		//IL_058f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0591: Unknown result type (might be due to invalid IL or missing references)
		//IL_0596: Unknown result type (might be due to invalid IL or missing references)
		//IL_0598: Unknown result type (might be due to invalid IL or missing references)
		//IL_05a5: Unknown result type (might be due to invalid IL or missing references)
		//IL_05a7: Unknown result type (might be due to invalid IL or missing references)
		//IL_05a9: Unknown result type (might be due to invalid IL or missing references)
		//IL_07bd: Unknown result type (might be due to invalid IL or missing references)
		//IL_07cd: Unknown result type (might be due to invalid IL or missing references)
		//IL_07d2: Unknown result type (might be due to invalid IL or missing references)
		//IL_07e9: Unknown result type (might be due to invalid IL or missing references)
		//IL_07f0: Unknown result type (might be due to invalid IL or missing references)
		//IL_07f5: Unknown result type (might be due to invalid IL or missing references)
		//IL_07fa: Unknown result type (might be due to invalid IL or missing references)
		//IL_0803: Unknown result type (might be due to invalid IL or missing references)
		//IL_0808: Unknown result type (might be due to invalid IL or missing references)
		//IL_0813: Unknown result type (might be due to invalid IL or missing references)
		//IL_0818: Unknown result type (might be due to invalid IL or missing references)
		//IL_081d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0831: Unknown result type (might be due to invalid IL or missing references)
		//IL_0836: Unknown result type (might be due to invalid IL or missing references)
		//IL_0843: Unknown result type (might be due to invalid IL or missing references)
		//IL_0848: Unknown result type (might be due to invalid IL or missing references)
		//IL_0663: Unknown result type (might be due to invalid IL or missing references)
		//IL_0665: Unknown result type (might be due to invalid IL or missing references)
		//IL_0666: Unknown result type (might be due to invalid IL or missing references)
		//IL_06d3: Unknown result type (might be due to invalid IL or missing references)
		//IL_06d5: Unknown result type (might be due to invalid IL or missing references)
		//IL_06d6: Unknown result type (might be due to invalid IL or missing references)
		//IL_0693: Unknown result type (might be due to invalid IL or missing references)
		//IL_069a: Expected O, but got Unknown
		//IL_092e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0930: Unknown result type (might be due to invalid IL or missing references)
		//IL_0931: Unknown result type (might be due to invalid IL or missing references)
		//IL_08bc: Unknown result type (might be due to invalid IL or missing references)
		//IL_08be: Unknown result type (might be due to invalid IL or missing references)
		//IL_08bf: Unknown result type (might be due to invalid IL or missing references)
		//IL_0703: Unknown result type (might be due to invalid IL or missing references)
		//IL_070a: Expected O, but got Unknown
		//IL_0743: Unknown result type (might be due to invalid IL or missing references)
		//IL_0745: Unknown result type (might be due to invalid IL or missing references)
		//IL_0746: Unknown result type (might be due to invalid IL or missing references)
		//IL_095e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0965: Expected O, but got Unknown
		//IL_08ec: Unknown result type (might be due to invalid IL or missing references)
		//IL_08f3: Expected O, but got Unknown
		//IL_0773: Unknown result type (might be due to invalid IL or missing references)
		//IL_077a: Expected O, but got Unknown
		//IL_09a2: Unknown result type (might be due to invalid IL or missing references)
		//IL_09a4: Unknown result type (might be due to invalid IL or missing references)
		//IL_09a5: Unknown result type (might be due to invalid IL or missing references)
		//IL_0dce: Unknown result type (might be due to invalid IL or missing references)
		//IL_0dd0: Unknown result type (might be due to invalid IL or missing references)
		//IL_0dd1: Unknown result type (might be due to invalid IL or missing references)
		//IL_09d2: Unknown result type (might be due to invalid IL or missing references)
		//IL_09d9: Expected O, but got Unknown
		//IL_0c3a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0c54: Unknown result type (might be due to invalid IL or missing references)
		//IL_0c56: Unknown result type (might be due to invalid IL or missing references)
		//IL_0c60: Unknown result type (might be due to invalid IL or missing references)
		//IL_0c6b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0c70: Unknown result type (might be due to invalid IL or missing references)
		//IL_0c74: Unknown result type (might be due to invalid IL or missing references)
		//IL_0c8b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0c90: Unknown result type (might be due to invalid IL or missing references)
		//IL_0c9c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0cc7: Unknown result type (might be due to invalid IL or missing references)
		//IL_0cc9: Unknown result type (might be due to invalid IL or missing references)
		//IL_0cd5: Unknown result type (might be due to invalid IL or missing references)
		//IL_0ce0: Unknown result type (might be due to invalid IL or missing references)
		//IL_0d30: Unknown result type (might be due to invalid IL or missing references)
		//IL_0d38: Unknown result type (might be due to invalid IL or missing references)
		//IL_0d40: Unknown result type (might be due to invalid IL or missing references)
		//IL_0d48: Unknown result type (might be due to invalid IL or missing references)
		//IL_0d50: Unknown result type (might be due to invalid IL or missing references)
		//IL_0d5b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0d62: Expected O, but got Unknown
		//IL_0d64: Unknown result type (might be due to invalid IL or missing references)
		//IL_0d6b: Expected O, but got Unknown
		//IL_0d86: Unknown result type (might be due to invalid IL or missing references)
		//IL_0d8b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0d8f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0ac7: Unknown result type (might be due to invalid IL or missing references)
		//IL_0ad4: Unknown result type (might be due to invalid IL or missing references)
		//IL_0ae2: Unknown result type (might be due to invalid IL or missing references)
		//IL_0b34: Unknown result type (might be due to invalid IL or missing references)
		//IL_0b3f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0b44: Unknown result type (might be due to invalid IL or missing references)
		//IL_0b46: Unknown result type (might be due to invalid IL or missing references)
		//IL_0b51: Unknown result type (might be due to invalid IL or missing references)
		//IL_0b56: Unknown result type (might be due to invalid IL or missing references)
		//IL_0b58: Unknown result type (might be due to invalid IL or missing references)
		//IL_0b63: Unknown result type (might be due to invalid IL or missing references)
		//IL_0b68: Unknown result type (might be due to invalid IL or missing references)
		//IL_0b6c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0b6e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0b70: Unknown result type (might be due to invalid IL or missing references)
		//IL_0b9e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0ba0: Unknown result type (might be due to invalid IL or missing references)
		//IL_0ba2: Unknown result type (might be due to invalid IL or missing references)
		//IL_0bae: Unknown result type (might be due to invalid IL or missing references)
		//IL_0bb9: Unknown result type (might be due to invalid IL or missing references)
		//IL_0c09: Unknown result type (might be due to invalid IL or missing references)
		//IL_0c11: Unknown result type (might be due to invalid IL or missing references)
		//IL_0c19: Unknown result type (might be due to invalid IL or missing references)
		//IL_0c21: Unknown result type (might be due to invalid IL or missing references)
		//IL_0c29: Unknown result type (might be due to invalid IL or missing references)
		//IL_0e08: Unknown result type (might be due to invalid IL or missing references)
		//IL_0e0f: Expected O, but got Unknown
		outCurves = new List<Curve>();
		outProngs = new List<Brep>();
		topDome = new List<Brep>();
		middleProng = new List<Brep>();
		lowerDome = new List<Brep>();
		Point3d pointAtStart = prongRail.PointAtStart;
		Point3d pointAtEnd = prongRail.PointAtEnd;
		Vector3d val = CSBlockUtil.CSUtilMath.AverageTangentAt(headTop, paramTop);
		((Vector3d)(ref val)).Unitize();
		Vector3d headTopNrm = CSBlockUtil.CSUtilMath.AverageNormalAt(Vector3d.ZAxis, val);
		((Vector3d)(ref headTopNrm)).Unitize();
		BoundingBox boundingBox = ((GeometryBase)inProngProfileTop).GetBoundingBox(true);
		Vector3d diagonal = ((BoundingBox)(ref boundingBox)).Diagonal;
		double x = ((Vector3d)(ref diagonal)).X;
		diagonal = ((BoundingBox)(ref boundingBox)).Diagonal;
		double y = ((Vector3d)(ref diagonal)).Y;
		Vector3d inputVector = new Vector3d(0.0, 0.0, -1.0);
		Curve obj = prongRail;
		Interval domain = prongRail.Domain;
		double num = CSBlockUtil.CSUtilMath.AngleBetweenVectorsWithDirection(inputVector, obj.TangentAt(((Interval)(ref domain)).Min), val);
		double num2 = 1.0 / Math.Cos(num);
		Transform val2 = Transform.Scale(Plane.WorldXY, prongTopX / x, prongTopY / y * num2, 0.0);
		if (!planarDome)
		{
			val2 = Transform.Scale(Plane.WorldXY, prongTopX / x, prongTopY / y, 0.0);
		}
		Transform val3 = Transform.Translation(0.0, prongTopOffset, 0.0);
		Transform val4 = CSBlockUtil.CSUtilMath.Orient3Point(Point3d.Origin, Vector3d.XAxis, Vector3d.ZAxis, pointAtStart, val, Vector3d.ZAxis);
		Curve prongTopProfile = inProngProfileTop.DuplicateCurve();
		((GeometryBase)prongTopProfile).Transform(val4 * val3 * val2);
		Transform val5 = CSBlockUtil.CSUtilMath.Rotation(Vector3d.ZAxis * -1.0, prongRail.TangentAtStart, pointAtStart);
		if (!planarDome)
		{
			((GeometryBase)prongTopProfile).Transform(val5);
		}
		Vector3d val6;
		if (baseStyle != ValueListItem.HeadBuilderProngHead)
		{
			val6 = val;
		}
		else
		{
			val6 = CSBlockUtil.CSUtilMath.AverageTangentAt(headBase, paramBase);
			((Vector3d)(ref val6)).Unitize();
		}
		BoundingBox boundingBox2 = ((GeometryBase)inProngProfileBase).GetBoundingBox(true);
		diagonal = ((BoundingBox)(ref boundingBox2)).Diagonal;
		double x2 = ((Vector3d)(ref diagonal)).X;
		diagonal = ((BoundingBox)(ref boundingBox2)).Diagonal;
		double y2 = ((Vector3d)(ref diagonal)).Y;
		Vector3d inputVector2 = new Vector3d(0.0, 0.0, -1.0);
		Curve obj2 = prongRail;
		domain = prongRail.Domain;
		double d = CSBlockUtil.CSUtilMath.AngleBetweenVectorsWithDirection(inputVector2, obj2.TangentAt(((Interval)(ref domain)).Max), val6);
		double num3 = 1.0 / Math.Cos(d);
		if (focalPoint != Point3d.Unset)
		{
			num3 = 1.0;
		}
		Transform val7 = Transform.Scale(Plane.WorldXY, prongBaseX / x2, prongBaseY / y2 * num3, 0.0);
		if (baseStyle == ValueListItem.HeadBuilderProngHeadCutToPoint)
		{
			prongBaseOffset = 0.0 - prongBaseY * num3 + basePointSize;
		}
		Transform val8 = Transform.Translation(0.0, prongBaseOffset, 0.0);
		Transform val9 = CSBlockUtil.CSUtilMath.Orient3Point(Point3d.Origin, Vector3d.XAxis, Vector3d.ZAxis, pointAtEnd, (baseStyle != ValueListItem.HeadBuilderProngHead) ? val : val6, Vector3d.ZAxis);
		Curve prongBaseProfile = inProngProfileBase.DuplicateCurve();
		((GeometryBase)prongBaseProfile).Transform(val9 * val8 * val7);
		Brep dome = null;
		Brep dome2 = null;
		if (camMode && !enableDoItYourself)
		{
			double gemZMax = gem.GemZMax;
			Point3d pointAtStart2 = prongRail.PointAtStart;
			if (gemZMax > ((Point3d)(ref pointAtStart2)).Z)
			{
				double gemZMax2 = gem.GemZMax;
				pointAtStart2 = prongRail.PointAtStart;
				if (!CreateProngExtension(gemZMax2 - ((Point3d)(ref pointAtStart2)).Z, ref prongRail, ref prongTopProfile))
				{
					error = "Unable to create CAM prong extension.";
					return false;
				}
			}
			if (!CreateProngDome(inProngProfileTop, prongTopX, prongTopY, num, prongDomeHeight, domeRotateX, prongSettingAngle, num2, planarDome, domeBlendAmountInside, domeBlendAmountOutside, domeBlendStartInside, domeBlendStartOtuside, domeBlendEndInside, domeBlendEndOutside, ref dome))
			{
				error = "Unable to create top dome";
				return false;
			}
			pointAtStart = prongRail.PointAtStart;
			val4 = CSBlockUtil.CSUtilMath.Orient3Point(Point3d.Origin, Vector3d.XAxis, Vector3d.ZAxis, pointAtStart, val, Vector3d.ZAxis);
			((GeometryBase)dome).Transform(val4 * val3);
			val5 = CSBlockUtil.CSUtilMath.Rotation(Vector3d.ZAxis * -1.0, prongRail.TangentAtStart, pointAtStart);
			if (!planarDome)
			{
				((GeometryBase)dome).Transform(val5);
			}
		}
		else if (prongDomeHeight <= 0.0)
		{
			if (!CreateProngExtension(0.0, ref prongRail, ref prongTopProfile))
			{
				error = "Unable to create top prong cap.";
				return false;
			}
			if (!planarDome)
			{
				((GeometryBase)prongTopProfile).Transform(val5);
			}
		}
		else
		{
			if (!CreateProngDome(inProngProfileTop, prongTopX, prongTopY, num, prongDomeHeight, domeRotateX, prongSettingAngle, num2, planarDome, domeBlendAmountInside, domeBlendAmountOutside, domeBlendStartInside, domeBlendStartOtuside, domeBlendEndInside, domeBlendEndOutside, ref dome))
			{
				error = "Unable to create top dome";
				return false;
			}
			((GeometryBase)dome).Transform(val4 * val3);
			if (!planarDome)
			{
				((GeometryBase)dome).Transform(val5);
			}
		}
		if (prongBaseDomeHeight > 0.0)
		{
			if (!ReduceProngRailLength(ref prongRail, ref prongBaseProfile, prongBaseDomeHeight))
			{
				error = "ReduceProngRailLength failed";
				return false;
			}
			headBase.ClosestPoint(prongRail.PointAtEnd, ref paramBase);
			val6 = CSBlockUtil.CSUtilMath.AverageTangentAt(headBase, paramBase);
			((Vector3d)(ref val6)).Unitize();
			Vector3d inputVector3 = new Vector3d(0.0, 0.0, -1.0);
			Curve obj3 = prongRail;
			domain = prongRail.Domain;
			d = CSBlockUtil.CSUtilMath.AngleBetweenVectorsWithDirection(inputVector3, obj3.TangentAt(((Interval)(ref domain)).Max), val6);
			if (!CreateProngDome(inProngProfileBase, prongBaseX, prongBaseY, d, 0.0 - prongBaseDomeHeight, 0.0, 0.0, num3, planarDome, domeBlendAmountInside, domeBlendAmountOutside, domeBlendStartInside, domeBlendStartOtuside, domeBlendEndInside, domeBlendEndOutside, ref dome2))
			{
				error = "Unable to create base dome";
				return false;
			}
			pointAtEnd = prongRail.PointAtEnd;
			Transform val10 = CSBlockUtil.CSUtilMath.Orient3Point(Point3d.Origin, Vector3d.XAxis, Vector3d.ZAxis, pointAtEnd, (baseStyle != ValueListItem.HeadBuilderProngHead) ? val : val6, Vector3d.ZAxis);
			prongBaseProfile = inProngProfileBase.DuplicateCurve();
			((GeometryBase)prongBaseProfile).Transform(val10 * val8 * val7);
			((GeometryBase)dome2).Transform(val10 * val8);
		}
		GeometryBase[] arrGeometry = (GeometryBase[])(object)new GeometryBase[3]
		{
			(GeometryBase)prongRail,
			(GeometryBase)prongTopProfile,
			(GeometryBase)prongBaseProfile
		};
		List<GeometryBase> outputGeometries = new List<GeometryBase>();
		if (!ArrangeGeometry(prongArrangement, xUp, val, headTopNrm, prongTopX, overlap, arrGeometry, null, ref outputGeometries))
		{
			error = "Invalid head style";
			return false;
		}
		foreach (Curve item17 in outputGeometries)
		{
			Curve item = item17;
			outCurves.Add(item);
		}
		if (enableDoItYourself)
		{
			GeometryBase[] arrGeometry2;
			if (dome2 != null)
			{
				arrGeometry2 = (GeometryBase[])(object)new GeometryBase[2]
				{
					(GeometryBase)dome,
					(GeometryBase)dome2
				};
				List<GeometryBase> outputGeometries2 = new List<GeometryBase>();
				GeometryBase[] arrGeometry3 = (GeometryBase[])(object)new GeometryBase[1] { (GeometryBase)dome2 };
				if (!ArrangeGeometry(prongArrangement, xUp, val, headTopNrm, prongTopX, overlap, arrGeometry3, null, ref outputGeometries2))
				{
					error = "Invalid head style";
					return false;
				}
				foreach (Brep item18 in outputGeometries2)
				{
					Brep item2 = item18;
					lowerDome.Add(item2);
				}
			}
			else
			{
				arrGeometry2 = (GeometryBase[])(object)new GeometryBase[1] { (GeometryBase)dome };
			}
			List<GeometryBase> outputGeometries3 = new List<GeometryBase>();
			if (!ArrangeGeometry(prongArrangement, xUp, val, headTopNrm, prongTopX, overlap, arrGeometry2, null, ref outputGeometries3))
			{
				error = "Invalid head style";
				return false;
			}
			foreach (Brep item19 in outputGeometries3)
			{
				Brep item3 = item19;
				outProngs.Add(item3);
			}
			GeometryBase[] arrGeometry4 = (GeometryBase[])(object)new GeometryBase[1] { (GeometryBase)dome };
			List<GeometryBase> outputGeometries4 = new List<GeometryBase>();
			if (!ArrangeGeometry(prongArrangement, xUp, val, headTopNrm, prongTopX, overlap, arrGeometry4, null, ref outputGeometries4))
			{
				error = "Invalid head style";
				return false;
			}
			foreach (Brep item20 in outputGeometries4)
			{
				Brep item4 = item20;
				topDome.Add(item4);
			}
		}
		else
		{
			Curve[] profiles = (Curve[])(object)new Curve[2] { prongTopProfile, prongBaseProfile };
			double num4 = Vector3d.VectorAngle(prongTopProfile.CurvatureAt(0.0), prongBaseProfile.CurvatureAt(0.0), Plane.WorldXY);
			Vector3d val11 = prongBaseProfile.PointAtNormalizedLength(0.5) - prongBaseProfile.PointAtStart;
			((GeometryBase)prongBaseProfile).Transform(Transform.Rotation(0.0 - num4, prongBaseProfile.PointAtStart + val11 / 2.0));
			double[] array = new double[2];
			domain = prongRail.Domain;
			array[0] = ((Interval)(ref domain)).Min;
			domain = prongRail.Domain;
			array[1] = ((Interval)(ref domain)).Max;
			double[] parameters = array;
			if (!CSUtilSweepRC.Sweep1(prongRail, profiles, parameters, out var breps, closed: false, globalShapeBlend: false))
			{
				error = "Failed Sweep1";
				return false;
			}
			if (breps == null || breps.Length == 0)
			{
				error = "Failed Sweep1: No results";
				return false;
			}
			List<Brep> list = new List<Brep>();
			if (dome != null)
			{
				list.Add(dome);
			}
			if (dome2 != null)
			{
				list.Add(dome2);
			}
			if (dome != null)
			{
				GeometryBase[] arrGeometry5 = (GeometryBase[])(object)new GeometryBase[1] { (GeometryBase)dome };
				List<GeometryBase> outputGeometries5 = new List<GeometryBase>();
				if (!ArrangeGeometry(prongArrangement, xUp, val, headTopNrm, prongTopX, overlap, arrGeometry5, null, ref outputGeometries5))
				{
					error = "Invalid head style";
					return false;
				}
				foreach (Brep item21 in outputGeometries5)
				{
					Brep item5 = item21;
					topDome.Add(item5);
				}
			}
			List<GeometryBase> outputGeometries6 = new List<GeometryBase>();
			GeometryBase[] arrGeometry6 = (GeometryBase[])(object)new GeometryBase[1] { (GeometryBase)breps[0] };
			if (!ArrangeGeometry(prongArrangement, xUp, val, headTopNrm, prongTopX, overlap, arrGeometry6, null, ref outputGeometries6))
			{
				error = "Invalid head style";
				return false;
			}
			foreach (Brep item22 in outputGeometries6)
			{
				Brep item6 = item22;
				middleProng.Add(item6);
			}
			if (dome2 != null)
			{
				List<GeometryBase> outputGeometries7 = new List<GeometryBase>();
				GeometryBase[] arrGeometry7 = (GeometryBase[])(object)new GeometryBase[1] { (GeometryBase)dome2 };
				if (!ArrangeGeometry(prongArrangement, xUp, val, headTopNrm, prongTopX, overlap, arrGeometry7, null, ref outputGeometries7))
				{
					error = "Invalid head style";
					return false;
				}
				foreach (Brep item23 in outputGeometries7)
				{
					Brep item7 = item23;
					lowerDome.Add(item7);
				}
			}
			list.AddRange(breps);
			Brep val12 = CSUtilRhino.JoinAllBreps(list.ToArray(), 0.01);
			if (val12 == null)
			{
				error = "Failed JoinBreps";
				return false;
			}
			Brep brep;
			if (dome2 == null)
			{
				brep = CSUtilRhino.CapPlanarHoles(val12, 0.01);
				if (brep == null)
				{
					error = "Failed CapPlanarHoles";
					return false;
				}
			}
			else
			{
				brep = val12;
			}
			Brep brep2 = null;
			if (baseStyle == ValueListItem.HeadBuilderProngHeadCutToPoint)
			{
				List<Point3d> list2;
				if (prongRailArray.Count > 2)
				{
					Curve val13 = prongRailArray[(prongIndex + 1) % prongRailArray.Count];
					if (val13 == null)
					{
						error = "Failed to get prongRailCrvP";
						return false;
					}
					Curve val14 = val13.DuplicateCurve();
					Curve val15 = prongRailArray[(prongIndex - 1 + prongRailArray.Count) % prongRailArray.Count];
					if (val15 == null)
					{
						error = "Failed to get prongRailCrvM";
						return false;
					}
					Curve val16 = val15.DuplicateCurve();
					Vector3d val17 = default(Vector3d);
					((Vector3d)(ref val17))._002Ector(val14.PointAtStart);
					Vector3d val18 = default(Vector3d);
					((Vector3d)(ref val18))._002Ector(prongRail.PointAtStart);
					Vector3d val19 = default(Vector3d);
					((Vector3d)(ref val19))._002Ector(val16.PointAtStart);
					((Vector3d)(ref val17)).Z = 0.0;
					((Vector3d)(ref val18)).Z = 0.0;
					((Vector3d)(ref val19)).Z = 0.0;
					((Vector3d)(ref val17)).Unitize();
					((Vector3d)(ref val18)).Unitize();
					((Vector3d)(ref val19)).Unitize();
					val17 *= 20.0;
					val18 *= 20.0;
					val19 *= 20.0;
					Point3d item8 = default(Point3d);
					((Point3d)(ref item8))._002Ector(val17 + val18);
					Point3d item9 = default(Point3d);
					((Point3d)(ref item9))._002Ector(0.0, 0.0, 0.0);
					Point3d item10 = default(Point3d);
					((Point3d)(ref item10))._002Ector(val19 + val18);
					Point3d item11 = default(Point3d);
					((Point3d)(ref item11))._002Ector(val18 * -2.0);
					((Point3d)(ref item8)).Z = 20.0;
					((Point3d)(ref item9)).Z = 20.0;
					((Point3d)(ref item10)).Z = 20.0;
					((Point3d)(ref item11)).Z = 20.0;
					list2 = new List<Point3d> { item8, item9, item10, item11, item8 };
				}
				else
				{
					((Vector3d)(ref diagonal))._002Ector(prongRail.PointAtStart);
					((Vector3d)(ref diagonal)).Z = 0.0;
					Vector3d val20 = diagonal;
					((Vector3d)(ref val20)).Unitize();
					val20 *= 20.0;
					Vector3d val21 = default(Vector3d);
					((Vector3d)(ref val21))._002Ector(val20);
					((Vector3d)(ref val21)).Transform(Transform.Rotation(CSBlockUtil.CSUtilMath.ToRadians(90.0), Point3d.Origin));
					Point3d item12 = default(Point3d);
					((Point3d)(ref item12))._002Ector(val21);
					Point3d item13 = default(Point3d);
					((Point3d)(ref item13))._002Ector(0.0, 0.0, 0.0);
					Point3d item14 = default(Point3d);
					((Point3d)(ref item14))._002Ector(-val21);
					Point3d item15 = default(Point3d);
					((Point3d)(ref item15))._002Ector(val20 * -2.0);
					((Point3d)(ref item12)).Z = 20.0;
					((Point3d)(ref item13)).Z = 20.0;
					((Point3d)(ref item14)).Z = 20.0;
					((Point3d)(ref item15)).Z = 20.0;
					list2 = new List<Point3d> { item12, item13, item14, item15, item12 };
				}
				PolylineCurve val22 = new PolylineCurve((IEnumerable<Point3d>)list2);
				PolylineCurve val23 = new PolylineCurve((IEnumerable<Point3d>)list2);
				Transform val24 = Transform.Translation(0.0, 0.0, -40.0);
				((GeometryBase)val23).Transform(val24);
				brep2 = CSUtilRhino.CapPlanarHoles(((Surface)NurbsSurface.CreateRuledSurface((Curve)(object)val22, (Curve)(object)val23)).ToBrep(), 0.001);
				CSUtilGeometry.FixBrepSolidOrientation(ref brep, doFullClean: true);
				CSUtilGeometry.FixBrepSolidOrientation(ref brep2, doFullClean: true);
			}
			List<GeometryBase> outputGeometries8 = new List<GeometryBase>();
			if (!ArrangeGeometry(prongArrangement, xUp, val, headTopNrm, prongTopX, overlap, (GeometryBase[])(object)new GeometryBase[1] { (GeometryBase)brep }, brep2, ref outputGeometries8))
			{
				error = "Invalid head style";
				return false;
			}
			foreach (Brep item24 in outputGeometries8)
			{
				Brep item16 = item24;
				outProngs.Add(item16);
			}
		}
		error = "";
		return true;
	}

	private static bool CreateProngDome(Curve prongProfile, double prongX, double prongY, double angle, double radius, double rotateX, double prongSettingAngle, double fixScale, bool planarDome, double blendAmountInside, double blendAmountOutside, double blendStartInside, double blendStartOutside, double blendEndInside, double blendEndOutside, ref Brep dome)
	{
		//IL_0002: Unknown result type (might be due to invalid IL or missing references)
		//IL_0007: Unknown result type (might be due to invalid IL or missing references)
		//IL_005b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0063: Unknown result type (might be due to invalid IL or missing references)
		//IL_0068: Unknown result type (might be due to invalid IL or missing references)
		//IL_0075: Unknown result type (might be due to invalid IL or missing references)
		//IL_007a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0090: Unknown result type (might be due to invalid IL or missing references)
		//IL_0095: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f5: Unknown result type (might be due to invalid IL or missing references)
		//IL_00fa: Unknown result type (might be due to invalid IL or missing references)
		//IL_0105: Unknown result type (might be due to invalid IL or missing references)
		//IL_0106: Unknown result type (might be due to invalid IL or missing references)
		//IL_0107: Unknown result type (might be due to invalid IL or missing references)
		//IL_012d: Unknown result type (might be due to invalid IL or missing references)
		//IL_014d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0152: Unknown result type (might be due to invalid IL or missing references)
		//IL_0159: Expected O, but got Unknown
		//IL_009f: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a7: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ac: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b9: Unknown result type (might be due to invalid IL or missing references)
		//IL_00be: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d1: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d6: Unknown result type (might be due to invalid IL or missing references)
		//IL_01a4: Unknown result type (might be due to invalid IL or missing references)
		//IL_01a9: Unknown result type (might be due to invalid IL or missing references)
		//IL_01c7: Unknown result type (might be due to invalid IL or missing references)
		//IL_01cc: Unknown result type (might be due to invalid IL or missing references)
		//IL_01ea: Unknown result type (might be due to invalid IL or missing references)
		//IL_01ef: Unknown result type (might be due to invalid IL or missing references)
		//IL_01fd: Unknown result type (might be due to invalid IL or missing references)
		//IL_0227: Unknown result type (might be due to invalid IL or missing references)
		//IL_022c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0235: Unknown result type (might be due to invalid IL or missing references)
		//IL_023a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0248: Unknown result type (might be due to invalid IL or missing references)
		//IL_024d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0256: Unknown result type (might be due to invalid IL or missing references)
		//IL_025b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0265: Unknown result type (might be due to invalid IL or missing references)
		//IL_0267: Unknown result type (might be due to invalid IL or missing references)
		//IL_0285: Unknown result type (might be due to invalid IL or missing references)
		//IL_028a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0293: Unknown result type (might be due to invalid IL or missing references)
		//IL_0298: Unknown result type (might be due to invalid IL or missing references)
		//IL_02a2: Unknown result type (might be due to invalid IL or missing references)
		//IL_02a4: Unknown result type (might be due to invalid IL or missing references)
		//IL_02be: Unknown result type (might be due to invalid IL or missing references)
		//IL_02c3: Unknown result type (might be due to invalid IL or missing references)
		//IL_02e7: Unknown result type (might be due to invalid IL or missing references)
		//IL_02fc: Unknown result type (might be due to invalid IL or missing references)
		//IL_0301: Unknown result type (might be due to invalid IL or missing references)
		//IL_0305: Unknown result type (might be due to invalid IL or missing references)
		//IL_0306: Unknown result type (might be due to invalid IL or missing references)
		//IL_0307: Unknown result type (might be due to invalid IL or missing references)
		//IL_0313: Unknown result type (might be due to invalid IL or missing references)
		//IL_034e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0350: Unknown result type (might be due to invalid IL or missing references)
		//IL_035b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0360: Unknown result type (might be due to invalid IL or missing references)
		//IL_0365: Unknown result type (might be due to invalid IL or missing references)
		//IL_0370: Unknown result type (might be due to invalid IL or missing references)
		//IL_0375: Unknown result type (might be due to invalid IL or missing references)
		//IL_0377: Unknown result type (might be due to invalid IL or missing references)
		//IL_031d: Unknown result type (might be due to invalid IL or missing references)
		//IL_031f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0326: Unknown result type (might be due to invalid IL or missing references)
		//IL_032b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0330: Unknown result type (might be due to invalid IL or missing references)
		//IL_033b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0340: Unknown result type (might be due to invalid IL or missing references)
		//IL_0342: Unknown result type (might be due to invalid IL or missing references)
		//IL_0381: Unknown result type (might be due to invalid IL or missing references)
		//IL_038f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0394: Unknown result type (might be due to invalid IL or missing references)
		//IL_03a7: Unknown result type (might be due to invalid IL or missing references)
		//IL_03bc: Unknown result type (might be due to invalid IL or missing references)
		//IL_03cd: Unknown result type (might be due to invalid IL or missing references)
		//IL_03d2: Unknown result type (might be due to invalid IL or missing references)
		//IL_03d4: Unknown result type (might be due to invalid IL or missing references)
		//IL_03d6: Unknown result type (might be due to invalid IL or missing references)
		//IL_03e8: Unknown result type (might be due to invalid IL or missing references)
		//IL_03ed: Unknown result type (might be due to invalid IL or missing references)
		//IL_03ef: Unknown result type (might be due to invalid IL or missing references)
		//IL_03f1: Unknown result type (might be due to invalid IL or missing references)
		//IL_0440: Unknown result type (might be due to invalid IL or missing references)
		//IL_0460: Unknown result type (might be due to invalid IL or missing references)
		//IL_0465: Unknown result type (might be due to invalid IL or missing references)
		//IL_046c: Expected O, but got Unknown
		//IL_0533: Unknown result type (might be due to invalid IL or missing references)
		//IL_0538: Unknown result type (might be due to invalid IL or missing references)
		//IL_054c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0551: Unknown result type (might be due to invalid IL or missing references)
		//IL_0578: Unknown result type (might be due to invalid IL or missing references)
		//IL_057d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0591: Unknown result type (might be due to invalid IL or missing references)
		//IL_0596: Unknown result type (might be due to invalid IL or missing references)
		//IL_05fb: Unknown result type (might be due to invalid IL or missing references)
		//IL_0600: Unknown result type (might be due to invalid IL or missing references)
		//IL_0604: Unknown result type (might be due to invalid IL or missing references)
		BoundingBox boundingBox = ((GeometryBase)prongProfile).GetBoundingBox(true);
		bool flag = radius > 0.0;
		if (!flag)
		{
			blendAmountInside = 1.0;
			blendAmountOutside = 1.0;
			blendStartInside = 1.0;
			blendStartOutside = 1.0;
			blendEndInside = 1.0;
			blendEndOutside = 1.0;
		}
		Plane worldXY = Plane.WorldXY;
		Vector3d diagonal = ((BoundingBox)(ref boundingBox)).Diagonal;
		double num = prongX / ((Vector3d)(ref diagonal)).X;
		diagonal = ((BoundingBox)(ref boundingBox)).Diagonal;
		Transform val = Transform.Scale(worldXY, num, prongY / ((Vector3d)(ref diagonal)).Y * fixScale, 0.0);
		if (!planarDome && flag)
		{
			Plane worldXY2 = Plane.WorldXY;
			diagonal = ((BoundingBox)(ref boundingBox)).Diagonal;
			double num2 = prongX / ((Vector3d)(ref diagonal)).X;
			diagonal = ((BoundingBox)(ref boundingBox)).Diagonal;
			val = Transform.Scale(worldXY2, num2, prongY / ((Vector3d)(ref diagonal)).Y, 0.0);
		}
		Transform val2 = Transform.Translation(0.0, (0.0 - prongY) / 2.0, 0.0);
		Curve val3 = prongProfile.DuplicateCurve();
		((GeometryBase)val3).Transform(val2 * val);
		LineCurve val4 = new LineCurve(new Point3d(0.0, -50.0, 0.0), new Point3d(0.0, 50.0, 0.0));
		CurveIntersections val5 = Intersection.CurveCurve(val3, (Curve)(object)val4, 0.01, 0.01);
		if (val5.Count <= 1)
		{
			return false;
		}
		double parameterA = val5[0].ParameterA;
		double parameterA2 = val5[1].ParameterA;
		Interval domain = val3.Domain;
		if (Math.Abs(parameterA - ((Interval)(ref domain)).Min) > 0.0001)
		{
			domain = val3.Domain;
			if (Math.Abs(parameterA - ((Interval)(ref domain)).Max) > 0.0001)
			{
				Point3d val6 = val3.PointAt(parameterA2);
				val3.ChangeClosedCurveSeam(parameterA);
				val3.ClosestPoint(val6, ref parameterA2);
			}
		}
		Curve[] array = val3.Split(parameterA2);
		Curve val7 = array[0];
		Curve val8 = array[1];
		if (val7 == null || val8 == null)
		{
			return false;
		}
		domain = ((Curve)val4).Domain;
		Vector3d val9 = ((Curve)val4).TangentAt(((Interval)(ref domain)).Mid);
		((Vector3d)(ref val9)).Unitize();
		domain = val7.Domain;
		Vector3d val10 = val7.TangentAt(((Interval)(ref domain)).Mid);
		((Vector3d)(ref val10)).Unitize();
		if (val9 * val10 < 0.0)
		{
			val7.Reverse();
		}
		domain = val8.Domain;
		Vector3d val11 = val8.TangentAt(((Interval)(ref domain)).Mid);
		((Vector3d)(ref val11)).Unitize();
		if (val9 * val11 < 0.0)
		{
			val8.Reverse();
		}
		Vector3d zAxis = Vector3d.ZAxis;
		if (!flag)
		{
			((Vector3d)(ref zAxis)).Z = 0.0 - ((Vector3d)(ref zAxis)).Z;
			radius = 0.0 - radius;
		}
		if (planarDome || !flag)
		{
			((Vector3d)(ref zAxis)).Rotate(0.0 - angle, Vector3d.XAxis);
		}
		((Vector3d)(ref zAxis)).Unitize();
		Point3d val12 = ((BoundingBox)(ref boundingBox)).Center;
		((Point3d)(ref val12)).Transform(val2 * val);
		Point3d rotationCenter = default(Point3d);
		((Point3d)(ref rotationCenter))._002Ector(val12);
		if (flag)
		{
			val12 += Vector3d.ZAxis * radius;
			((Point3d)(ref val12)).Transform(CSBlockUtil.CSUtilMath.Rotation(CSBlockUtil.CSUtilMath.ToRadians(rotateX), Vector3d.XAxis, rotationCenter));
		}
		else
		{
			val12 += zAxis * (radius / Math.Cos(angle));
			((Point3d)(ref val12)).Transform(CSBlockUtil.CSUtilMath.Rotation(CSBlockUtil.CSUtilMath.ToRadians(rotateX), Vector3d.XAxis, rotationCenter));
		}
		Vector3d val13 = Vector3d.YAxis * -1.0;
		((Vector3d)(ref val13)).Rotate(CSBlockUtil.CSUtilMath.ToRadians(prongSettingAngle) + CSBlockUtil.CSUtilMath.ToRadians(rotateX), Vector3d.XAxis);
		((Vector3d)(ref val13)).Unitize();
		Vector3d vectorEnd = default(Vector3d);
		((Vector3d)(ref vectorEnd))._002Ector(val13);
		((Vector3d)(ref vectorEnd)).Reverse();
		Curve val14 = (Curve)(object)CSUtilCurves.BlendCurve(val7.PointAtStart, val12, zAxis, val13, blendAmountInside, blendStartInside, blendEndInside);
		Curve val15 = (Curve)(object)CSUtilCurves.BlendCurve(val7.PointAtEnd, val12, zAxis, vectorEnd, blendAmountOutside, blendStartOutside, blendEndOutside);
		Curve[] array2 = Curve.JoinCurves((IEnumerable<Curve>)(object)new Curve[2] { val14, val15 });
		if (array2 == null)
		{
			return false;
		}
		if (array2.Length == 0)
		{
			return false;
		}
		LineCurve val16 = new LineCurve(new Point3d(-50.0, 0.0, 0.0), new Point3d(50.0, 0.0, 0.0));
		CurveIntersections val17 = Intersection.CurveCurve(val7, (Curve)(object)val16, 0.01, 0.01);
		if (val17.Count <= 0)
		{
			return false;
		}
		double parameterA3 = val17[0].ParameterA;
		Curve[] array3 = val7.Split(parameterA3);
		Curve val18 = array3[0];
		Curve val19 = array3[1];
		if (val18 == null || val19 == null)
		{
			return false;
		}
		val19.Reverse();
		CurveIntersections val20 = Intersection.CurveCurve(val8, (Curve)(object)val16, 0.01, 0.01);
		if (!((IEnumerable<IntersectionEvent>)val20).Any())
		{
			return false;
		}
		double parameterA4 = val20[0].ParameterA;
		Curve[] array4 = val8.Split(parameterA4);
		Curve val21 = array4[0];
		Curve val22 = array4[1];
		if (val21 == null || val22 == null)
		{
			return false;
		}
		val22.Reverse();
		double[] array5 = new double[1];
		domain = val18.Domain;
		array5[0] = ((Interval)(ref domain)).Min;
		double[] array6 = new double[1];
		domain = val19.Domain;
		array6[0] = ((Interval)(ref domain)).Min;
		if (!CSUtilSweepRC.Sweep2(val18, val19, array2, array5, array6, out var breps, closed: false, maintainHeight: false))
		{
			return false;
		}
		double[] array7 = new double[1];
		domain = val21.Domain;
		array7[0] = ((Interval)(ref domain)).Min;
		double[] array8 = new double[1];
		domain = val21.Domain;
		array8[0] = ((Interval)(ref domain)).Min;
		if (!CSUtilSweepRC.Sweep2(val21, val22, array2, array7, array8, out var breps2, closed: false, maintainHeight: false))
		{
			return false;
		}
		Brep val23 = CSUtilRhino.JoinAllBreps_BackwardCompatibility(new List<Brep> { breps, breps2 }.ToArray(), CSApplicationHelper.Application.Tolerance);
		if (val23 == null)
		{
			return false;
		}
		Transform val24 = Transform.Translation(0.0, prongY / 2.0, 0.0);
		((GeometryBase)val23).Transform(val24);
		dome = val23;
		return true;
	}

	private static GeometryBase CutGeometry(GeometryBase g, Brep cut, Transform T, Transform xUp)
	{
		//IL_0014: Unknown result type (might be due to invalid IL or missing references)
		//IL_0024: Unknown result type (might be due to invalid IL or missing references)
		//IL_0033: Unknown result type (might be due to invalid IL or missing references)
		Brep val = (Brep)(object)((g is Brep) ? g : null);
		if (val == null || cut == null)
		{
			GeometryBase obj = g.Duplicate();
			obj.Transform(T);
			return obj;
		}
		val = val.DuplicateBrep();
		((GeometryBase)val).Transform(T);
		Brep val2 = cut.DuplicateBrep();
		((GeometryBase)val2).Transform(xUp);
		Brep[] array = Brep.CreateBooleanDifference(val, val2, 0.001);
		if (array == null)
		{
			return (GeometryBase)(object)val.DuplicateBrep();
		}
		return (GeometryBase)(object)array[0];
	}

	private static bool ArrangeGeometry(int prongArrangement, Transform xUp, Vector3d headTopTan, Vector3d headTopNrm, double prongTopX, double overlap, GeometryBase[] arrGeometry, Brep cutter, ref List<GeometryBase> outputGeometries)
	{
		//IL_004c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0056: Unknown result type (might be due to invalid IL or missing references)
		//IL_005d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0062: Unknown result type (might be due to invalid IL or missing references)
		//IL_0067: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f2: Unknown result type (might be due to invalid IL or missing references)
		//IL_00fc: Unknown result type (might be due to invalid IL or missing references)
		//IL_0103: Unknown result type (might be due to invalid IL or missing references)
		//IL_0108: Unknown result type (might be due to invalid IL or missing references)
		//IL_010d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0025: Unknown result type (might be due to invalid IL or missing references)
		//IL_0026: Unknown result type (might be due to invalid IL or missing references)
		//IL_0075: Unknown result type (might be due to invalid IL or missing references)
		//IL_0076: Unknown result type (might be due to invalid IL or missing references)
		//IL_0078: Unknown result type (might be due to invalid IL or missing references)
		//IL_007d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0099: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a3: Unknown result type (might be due to invalid IL or missing references)
		//IL_00aa: Unknown result type (might be due to invalid IL or missing references)
		//IL_00af: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b4: Unknown result type (might be due to invalid IL or missing references)
		//IL_011b: Unknown result type (might be due to invalid IL or missing references)
		//IL_011c: Unknown result type (might be due to invalid IL or missing references)
		//IL_011e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0123: Unknown result type (might be due to invalid IL or missing references)
		//IL_013f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0149: Unknown result type (might be due to invalid IL or missing references)
		//IL_0150: Unknown result type (might be due to invalid IL or missing references)
		//IL_0155: Unknown result type (might be due to invalid IL or missing references)
		//IL_015a: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c2: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c3: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c5: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ca: Unknown result type (might be due to invalid IL or missing references)
		//IL_0168: Unknown result type (might be due to invalid IL or missing references)
		//IL_0169: Unknown result type (might be due to invalid IL or missing references)
		//IL_016b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0170: Unknown result type (might be due to invalid IL or missing references)
		//IL_01b6: Unknown result type (might be due to invalid IL or missing references)
		//IL_01b9: Unknown result type (might be due to invalid IL or missing references)
		//IL_01be: Unknown result type (might be due to invalid IL or missing references)
		//IL_01c3: Unknown result type (might be due to invalid IL or missing references)
		//IL_01d1: Unknown result type (might be due to invalid IL or missing references)
		//IL_01d2: Unknown result type (might be due to invalid IL or missing references)
		//IL_01d4: Unknown result type (might be due to invalid IL or missing references)
		//IL_01d9: Unknown result type (might be due to invalid IL or missing references)
		switch ((CSHeadStyles.EHeadProngArrangment)prongArrangement)
		{
		case CSHeadStyles.EHeadProngArrangment.Single:
		{
			GeometryBase[] array = arrGeometry;
			for (int i = 0; i < array.Length; i++)
			{
				GeometryBase item4 = CutGeometry(array[i], cutter, xUp, xUp);
				outputGeometries.Add(item4);
			}
			break;
		}
		case CSHeadStyles.EHeadProngArrangment.Double:
		{
			double num3 = overlap * prongTopX;
			Transform val4 = Transform.Translation(headTopTan * 1.0 * num3);
			GeometryBase[] array = arrGeometry;
			for (int i = 0; i < array.Length; i++)
			{
				GeometryBase item5 = CutGeometry(array[i], cutter, xUp * val4, xUp);
				outputGeometries.Add(item5);
			}
			Transform val5 = Transform.Translation(headTopTan * -1.0 * num3);
			array = arrGeometry;
			for (int i = 0; i < array.Length; i++)
			{
				GeometryBase item6 = CutGeometry(array[i], cutter, xUp * val5, xUp);
				outputGeometries.Add(item6);
			}
			break;
		}
		case CSHeadStyles.EHeadProngArrangment.Triple:
		{
			double num = overlap * prongTopX;
			Transform val = Transform.Translation(headTopTan * 1.0 * num);
			GeometryBase[] array = arrGeometry;
			for (int i = 0; i < array.Length; i++)
			{
				GeometryBase item = CutGeometry(array[i], cutter, xUp * val, xUp);
				outputGeometries.Add(item);
			}
			Transform val2 = Transform.Translation(headTopTan * -1.0 * num);
			array = arrGeometry;
			for (int i = 0; i < array.Length; i++)
			{
				GeometryBase item2 = CutGeometry(array[i], cutter, xUp * val2, xUp);
				outputGeometries.Add(item2);
			}
			double num2 = overlap * prongTopX * 2.0 * Math.Sqrt(3.0) / 2.0;
			Transform val3 = Transform.Translation(headTopNrm * num2);
			array = arrGeometry;
			for (int i = 0; i < array.Length; i++)
			{
				GeometryBase item3 = CutGeometry(array[i], cutter, xUp * val3, xUp);
				outputGeometries.Add(item3);
			}
			break;
		}
		default:
			return false;
		}
		return true;
	}

	private static bool MakeVProngs(Curve prongRail, Curve headTop, double paramTop, double prongVTopAngle, double prongTopY, double prongTopOffset, double prongVTopBlend, double prongVBottomBlend, Curve headBase, double paramBase, double prongVBaseAngle, double prongBaseY, double prongBaseOffset, ValueListItem baseStyle, double prongBaseDomeHeight, double domeRadius, double domeRotateX, bool enableDoItYourself, bool planarVProng, bool camMode, out List<Brep> topDome, out List<Brep> middleProng, out List<Brep> lowerDome, out List<Curve> outCurves, out Brep outProng, out string error)
	{
		//IL_0025: Unknown result type (might be due to invalid IL or missing references)
		//IL_002a: Unknown result type (might be due to invalid IL or missing references)
		//IL_002b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0039: Unknown result type (might be due to invalid IL or missing references)
		//IL_003f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0044: Unknown result type (might be due to invalid IL or missing references)
		//IL_0045: Unknown result type (might be due to invalid IL or missing references)
		//IL_004a: Unknown result type (might be due to invalid IL or missing references)
		//IL_004d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0052: Unknown result type (might be due to invalid IL or missing references)
		//IL_005b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0060: Unknown result type (might be due to invalid IL or missing references)
		//IL_0065: Unknown result type (might be due to invalid IL or missing references)
		//IL_0092: Unknown result type (might be due to invalid IL or missing references)
		//IL_0141: Unknown result type (might be due to invalid IL or missing references)
		//IL_017f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0199: Unknown result type (might be due to invalid IL or missing references)
		//IL_01b9: Unknown result type (might be due to invalid IL or missing references)
		//IL_01be: Unknown result type (might be due to invalid IL or missing references)
		//IL_01cb: Unknown result type (might be due to invalid IL or missing references)
		//IL_01d0: Unknown result type (might be due to invalid IL or missing references)
		//IL_0102: Unknown result type (might be due to invalid IL or missing references)
		//IL_0107: Unknown result type (might be due to invalid IL or missing references)
		//IL_0110: Unknown result type (might be due to invalid IL or missing references)
		//IL_0115: Unknown result type (might be due to invalid IL or missing references)
		//IL_0119: Unknown result type (might be due to invalid IL or missing references)
		//IL_02be: Unknown result type (might be due to invalid IL or missing references)
		//IL_02c3: Unknown result type (might be due to invalid IL or missing references)
		//IL_02dc: Unknown result type (might be due to invalid IL or missing references)
		//IL_02e1: Unknown result type (might be due to invalid IL or missing references)
		//IL_0302: Unknown result type (might be due to invalid IL or missing references)
		//IL_0307: Unknown result type (might be due to invalid IL or missing references)
		//IL_0320: Unknown result type (might be due to invalid IL or missing references)
		//IL_0325: Unknown result type (might be due to invalid IL or missing references)
		outCurves = new List<Curve>();
		outProng = null;
		topDome = new List<Brep>();
		middleProng = new List<Brep>();
		lowerDome = new List<Brep>();
		Point3d pointAtStart = prongRail.PointAtStart;
		Transform val = CSBlockUtil.CSUtilMath.Rotation(Vector3d.ZAxis * -1.0, prongRail.TangentAtStart, pointAtStart);
		Curve obj = prongRail;
		Interval domain = prongRail.Domain;
		Vector3d prongDir = -obj.TangentAt(((Interval)(ref domain)).Min);
		((Vector3d)(ref prongDir)).Unitize();
		Curve item = prongRail;
		if (!MakeVProngProfile(headTop, paramTop, prongVTopAngle, prongTopY, prongTopOffset, prongVTopBlend, out var vProng, out var prongCurves, out var trimPart))
		{
			error = "Error Making Straight Prong Profile Top";
			return false;
		}
		if (!MakeVProngDomes(prongCurves, prongDir, domeRadius, domeRotateX, trimPart, out var vDome))
		{
			error = "Error Making V Prong Domes";
			return false;
		}
		if (!MakeVProngProfile(headBase, paramBase, prongVBaseAngle, prongBaseY, prongBaseOffset, prongVBottomBlend, out var vProng2, out var prongCurves2, out trimPart))
		{
			error = "Error Making Straight Prong Profile Base";
			return false;
		}
		Brep vDome2 = null;
		if (!camMode && baseStyle == ValueListItem.HeadBuilderProngHead && prongBaseDomeHeight > 0.0)
		{
			if (!ReduceProngRailLength(ref prongRail, ref vProng2, prongBaseDomeHeight))
			{
				error = "ReduceProngRailLength failed";
				return false;
			}
			Curve obj2 = prongRail;
			domain = prongRail.Domain;
			Vector3d prongDir2 = obj2.TangentAt(((Interval)(ref domain)).Max);
			if (!MakeVProngDomes(prongCurves2, prongDir2, prongBaseDomeHeight, 0.0, trimPart, out vDome2))
			{
				error = "Error Making V Prong Domes";
				return false;
			}
		}
		if (!planarVProng)
		{
			((GeometryBase)vProng).Transform(val);
		}
		outCurves.Add(prongRail);
		outCurves.Add(item);
		outCurves.Add(vProng);
		outCurves.Add(vProng2);
		if (enableDoItYourself)
		{
			error = "";
			return true;
		}
		double num = default(double);
		vProng.ClosestPoint(prongRail.PointAtStart, ref num);
		vProng.ChangeClosedCurveSeam(num);
		vProng2.ClosestPoint(prongRail.PointAtEnd, ref num);
		vProng2.ChangeClosedCurveSeam(num);
		double[] array = new double[2];
		domain = prongRail.Domain;
		array[0] = ((Interval)(ref domain)).Min;
		domain = prongRail.Domain;
		array[1] = ((Interval)(ref domain)).Max;
		double[] array2 = array;
		Curve[] profiles = (Curve[])(object)new Curve[2]
		{
			prongCurves2[0],
			prongCurves[0]
		};
		if (!CSUtilSweepRC.Sweep1(prongRail, profiles, array2, out var breps, closed: false, globalShapeBlend: true))
		{
			error = "Failed Sweep1";
			return false;
		}
		Brep val2 = breps[0];
		Curve[] profiles2 = (Curve[])(object)new Curve[2]
		{
			prongCurves2[2],
			prongCurves[2]
		};
		if (!CSUtilSweepRC.Sweep1(prongRail, profiles2, array2, out var breps2, closed: false, globalShapeBlend: true))
		{
			error = "Failed Sweep1";
			return false;
		}
		Brep val3 = breps2[0];
		Curve[] profile = (Curve[])(object)new Curve[2]
		{
			prongCurves2[1],
			prongCurves[1]
		};
		if (!CSUtilSweepRC.Sweep2((Curve)(object)val2.Edges[0], (Curve)(object)val3.Edges[0], profile, array2, array2, out var breps3, closed: false, maintainHeight: true))
		{
			error = "Failed Front Sweep";
			return false;
		}
		Curve[] profile2 = (Curve[])(object)new Curve[2]
		{
			prongCurves[3],
			prongCurves2[3]
		};
		double[] array3 = new double[2];
		domain = ((Curve)val2.Edges[2]).Domain;
		array3[0] = ((Interval)(ref domain)).Min;
		domain = ((Curve)val2.Edges[2]).Domain;
		array3[1] = ((Interval)(ref domain)).Max;
		double[] parameters = array3;
		double[] array4 = new double[2];
		domain = ((Curve)val3.Edges[2]).Domain;
		array4[0] = ((Interval)(ref domain)).Min;
		domain = ((Curve)val3.Edges[2]).Domain;
		array4[1] = ((Interval)(ref domain)).Max;
		double[] parameters2 = array4;
		if (!CSUtilSweepRC.Sweep2((Curve)(object)val3.Edges[2], (Curve)(object)val2.Edges[2], profile2, parameters2, parameters, out var breps4, closed: false, maintainHeight: true))
		{
			error = "Failed Back Sweep";
			return false;
		}
		Brep[] array5 = Brep.JoinBreps((IEnumerable<Brep>)(object)new Brep[4] { breps4, val2, breps3, val3 }, 0.01);
		if (array5 == null || array5.Length == 0)
		{
			error = "Failed Sweep1: No results";
			return false;
		}
		Brep brep = array5[0];
		CSUtilGeometry.FixBrepSolidOrientation(ref brep);
		Brep val4 = CSUtilRhino.JoinAllBreps_BackwardCompatibility((Brep[])(object)((vDome2 != null) ? new Brep[3] { brep, vDome, vDome2 } : new Brep[2] { brep, vDome }), 0.01);
		Brep brep2;
		if (vDome2 != null)
		{
			brep2 = val4;
		}
		else
		{
			brep2 = CSUtilRhino.CapPlanarHoles(val4, 0.01);
			if (brep2 == null)
			{
				error = "Failed CapPlanarHoles";
				return false;
			}
		}
		CSUtilGeometry.FixBrepSolidOrientation(ref brep2);
		outProng = brep2;
		middleProng.Add(brep);
		topDome.Add(vDome);
		if (vDome2 != null)
		{
			lowerDome.Add(vDome2);
		}
		error = "";
		return true;
	}

	private static bool MakeVProngProfile(Curve headProfile, double param, double vProngAngle, double prongY, double prongNudge, double blendAmount, out Curve vProng, out Curve[] prongCurves, out Curve trimPart)
	{
		//IL_0013: Unknown result type (might be due to invalid IL or missing references)
		//IL_0018: Unknown result type (might be due to invalid IL or missing references)
		//IL_0032: Unknown result type (might be due to invalid IL or missing references)
		//IL_0060: Unknown result type (might be due to invalid IL or missing references)
		//IL_0065: Unknown result type (might be due to invalid IL or missing references)
		//IL_006b: Expected O, but got Unknown
		//IL_0084: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b2: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b7: Unknown result type (might be due to invalid IL or missing references)
		//IL_00bd: Expected O, but got Unknown
		//IL_00c3: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c8: Unknown result type (might be due to invalid IL or missing references)
		//IL_00cd: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d2: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d4: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e2: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e7: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ec: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f1: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f3: Unknown result type (might be due to invalid IL or missing references)
		//IL_0184: Unknown result type (might be due to invalid IL or missing references)
		//IL_0189: Unknown result type (might be due to invalid IL or missing references)
		//IL_0192: Unknown result type (might be due to invalid IL or missing references)
		//IL_01a0: Unknown result type (might be due to invalid IL or missing references)
		//IL_01a5: Unknown result type (might be due to invalid IL or missing references)
		//IL_01b0: Unknown result type (might be due to invalid IL or missing references)
		//IL_0173: Unknown result type (might be due to invalid IL or missing references)
		//IL_01e6: Unknown result type (might be due to invalid IL or missing references)
		//IL_0204: Unknown result type (might be due to invalid IL or missing references)
		//IL_0217: Unknown result type (might be due to invalid IL or missing references)
		//IL_0235: Unknown result type (might be due to invalid IL or missing references)
		//IL_0268: Unknown result type (might be due to invalid IL or missing references)
		//IL_026d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0271: Unknown result type (might be due to invalid IL or missing references)
		//IL_0278: Unknown result type (might be due to invalid IL or missing references)
		//IL_027d: Unknown result type (might be due to invalid IL or missing references)
		//IL_027f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0281: Unknown result type (might be due to invalid IL or missing references)
		//IL_0288: Expected O, but got Unknown
		//IL_028a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0291: Unknown result type (might be due to invalid IL or missing references)
		//IL_0296: Unknown result type (might be due to invalid IL or missing references)
		//IL_0298: Unknown result type (might be due to invalid IL or missing references)
		//IL_029a: Unknown result type (might be due to invalid IL or missing references)
		//IL_02a1: Expected O, but got Unknown
		//IL_04c3: Unknown result type (might be due to invalid IL or missing references)
		//IL_04d9: Unknown result type (might be due to invalid IL or missing references)
		//IL_04de: Unknown result type (might be due to invalid IL or missing references)
		//IL_04ed: Unknown result type (might be due to invalid IL or missing references)
		//IL_0510: Unknown result type (might be due to invalid IL or missing references)
		//IL_0512: Unknown result type (might be due to invalid IL or missing references)
		//IL_0517: Unknown result type (might be due to invalid IL or missing references)
		vProng = null;
		prongCurves = null;
		trimPart = null;
		if (headProfile == null)
		{
			return false;
		}
		Point3d val = headProfile.PointAt(param);
		LineCurve val2 = new LineCurve(new Point3d(0.0, 0.0, ((Point3d)(ref val)).Z), new Point3d(((Point3d)(ref val)).X * 2.0, ((Point3d)(ref val)).Y * 2.0, ((Point3d)(ref val)).Z));
		LineCurve val3 = new LineCurve(new Point3d(0.0, 0.0, ((Point3d)(ref val)).Z), new Point3d(((Point3d)(ref val)).X * 2.0, ((Point3d)(ref val)).Y * 2.0, ((Point3d)(ref val)).Z));
		Transform val4 = CSBlockUtil.CSUtilMath.Rotation(CSBlockUtil.CSUtilMath.ToRadians(vProngAngle), Vector3d.ZAxis, Point3d.Origin);
		((GeometryBase)val2).Transform(val4);
		val4 = CSBlockUtil.CSUtilMath.Rotation(CSBlockUtil.CSUtilMath.ToRadians(0.0 - vProngAngle), Vector3d.ZAxis, Point3d.Origin);
		((GeometryBase)val3).Transform(val4);
		CurveIntersections val5 = Intersection.CurveCurve(headProfile, (Curve)(object)val2, 0.01, 0.01);
		if (val5.Count != 1)
		{
			return false;
		}
		double parameterA = val5[0].ParameterA;
		val5 = Intersection.CurveCurve(headProfile, (Curve)(object)val3, 0.01, 0.01);
		if (val5.Count != 1)
		{
			return false;
		}
		double parameterA2 = val5[0].ParameterA;
		Interval domain;
		if (parameterA < parameterA2)
		{
			trimPart = headProfile.Trim(new Interval(parameterA, parameterA2));
		}
		else
		{
			domain = headProfile.Domain;
			Curve val6 = headProfile.Trim(new Interval(parameterA, ((Interval)(ref domain)).Max));
			domain = headProfile.Domain;
			Curve val7 = headProfile.Trim(new Interval(((Interval)(ref domain)).Min, parameterA2));
			Curve[] array = Curve.JoinCurves((IEnumerable<Curve>)(object)new Curve[2] { val6, val7 }, 0.01);
			trimPart = array[0];
		}
		Curve crv = CSUtilGeometry.Offset(trimPart, Vector3d.ZAxis, new Point3d(0.0, 0.0, ((Point3d)(ref val)).Z), 0.0 - prongNudge, (CurveOffsetCornerStyle)2);
		Curve crv2 = CSUtilGeometry.Offset(trimPart, Vector3d.ZAxis, new Point3d(0.0, 0.0, ((Point3d)(ref val)).Z), 0.0 - (prongY + prongNudge), (CurveOffsetCornerStyle)2);
		if (crv == null || crv2 == null)
		{
			return false;
		}
		double num = default(double);
		crv2.NormalizedLengthParameter(0.5, ref num);
		Point3d val8 = crv2.PointAt(num);
		Point3d pointAtStart = crv.PointAtStart;
		Point3d pointAtStart2 = crv2.PointAtStart;
		Curve crv3 = (Curve)new LineCurve(pointAtStart, pointAtStart2);
		Point3d pointAtEnd = crv.PointAtEnd;
		Point3d pointAtEnd2 = crv2.PointAtEnd;
		Curve crv4 = (Curve)new LineCurve(pointAtEnd, pointAtEnd2);
		double num2 = blendAmount;
		double num3 = blendAmount;
		double length = crv3.GetLength();
		double length2 = crv.GetLength();
		double length3 = crv2.GetLength();
		if (blendAmount > length * 0.48)
		{
			blendAmount = length * 0.48;
		}
		if (num2 > length2 * 0.48)
		{
			num2 = length2 * 0.48;
		}
		if (num3 > length3 * 0.48)
		{
			num3 = length3 * 0.48;
		}
		if (blendAmount > 0.0)
		{
			CSUtilGeometry.TrimStartOfCrv(ref crv, num2);
			CSUtilGeometry.TrimStartOfCrv(ref crv2, num3);
			CSUtilGeometry.TrimEndOfCrv(ref crv3, blendAmount);
			CSUtilGeometry.TrimStartOfCrv(ref crv3, blendAmount);
			Curve val9 = (Curve)(object)CSUtilCurves.BlendCurve(crv, crv3, doBlendFromStart1: true);
			Curve val10 = (Curve)(object)CSUtilCurves.BlendCurve(crv3, crv2);
			CSUtilGeometry.TrimEndOfCrv(ref crv, num2);
			CSUtilGeometry.TrimEndOfCrv(ref crv2, num3);
			CSUtilGeometry.TrimEndOfCrv(ref crv4, blendAmount);
			CSUtilGeometry.TrimStartOfCrv(ref crv4, blendAmount);
			Curve val11 = (Curve)(object)CSUtilCurves.BlendCurve(crv, crv4);
			Curve val12 = (Curve)(object)CSUtilCurves.BlendCurve(crv4, crv2, doBlendFromStart1: false, doBlendFromStart2: false);
			Curve[] array2 = Curve.JoinCurves((IEnumerable<Curve>)(object)new Curve[3] { val9, crv3, val10 }, 0.01);
			if (array2.Length != 1)
			{
				return false;
			}
			crv3 = array2[0];
			array2 = Curve.JoinCurves((IEnumerable<Curve>)(object)new Curve[3] { val11, crv4, val12 }, 0.01);
			if (array2.Length != 1)
			{
				return false;
			}
			crv4 = array2[0];
		}
		prongCurves = (Curve[])(object)new Curve[4] { crv, crv3, crv2, crv4 };
		Curve[] array3 = Curve.JoinCurves((IEnumerable<Curve>)prongCurves, 0.01);
		if (array3.Length != 1)
		{
			return false;
		}
		Curve val13 = array3[0];
		double num4 = default(double);
		val13.ClosestPoint(val8, ref num4);
		val13.ChangeClosedCurveSeam(num4);
		domain = val13.Domain;
		double min = ((Interval)(ref domain)).Min;
		Vector3d val14 = val13.TangentAt(min);
		Vector3d val15 = default(Vector3d);
		((Vector3d)(ref val15))._002Ector(((Point3d)(ref val8)).X, ((Point3d)(ref val8)).Y, 0.0);
		Vector3d val16 = Vector3d.CrossProduct(val14, val15);
		if (((Vector3d)(ref val16)).Z < 0.0)
		{
			val13.Reverse();
		}
		vProng = val13;
		return true;
	}

	private static bool MakeVProngDomes(Curve[] prongCurves, Vector3d prongDir, double prongDomeHeight, double rotateX, Curve trimPart, out Brep vDome)
	{
		//IL_0016: Unknown result type (might be due to invalid IL or missing references)
		//IL_001b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0028: Unknown result type (might be due to invalid IL or missing references)
		//IL_002d: Unknown result type (might be due to invalid IL or missing references)
		//IL_004e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0054: Unknown result type (might be due to invalid IL or missing references)
		//IL_0059: Unknown result type (might be due to invalid IL or missing references)
		//IL_006f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0074: Unknown result type (might be due to invalid IL or missing references)
		//IL_0085: Unknown result type (might be due to invalid IL or missing references)
		//IL_008a: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a1: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a6: Unknown result type (might be due to invalid IL or missing references)
		//IL_00cc: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d5: Unknown result type (might be due to invalid IL or missing references)
		//IL_00da: Unknown result type (might be due to invalid IL or missing references)
		//IL_00dc: Unknown result type (might be due to invalid IL or missing references)
		//IL_00de: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e3: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ed: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ef: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f4: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f9: Unknown result type (might be due to invalid IL or missing references)
		//IL_00fb: Unknown result type (might be due to invalid IL or missing references)
		//IL_0106: Unknown result type (might be due to invalid IL or missing references)
		//IL_0108: Unknown result type (might be due to invalid IL or missing references)
		//IL_010d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0112: Unknown result type (might be due to invalid IL or missing references)
		//IL_011d: Unknown result type (might be due to invalid IL or missing references)
		//IL_011f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0124: Unknown result type (might be due to invalid IL or missing references)
		//IL_0129: Unknown result type (might be due to invalid IL or missing references)
		//IL_0130: Expected O, but got Unknown
		//IL_01e3: Unknown result type (might be due to invalid IL or missing references)
		//IL_01e9: Unknown result type (might be due to invalid IL or missing references)
		//IL_01ee: Unknown result type (might be due to invalid IL or missing references)
		//IL_0204: Unknown result type (might be due to invalid IL or missing references)
		//IL_0209: Unknown result type (might be due to invalid IL or missing references)
		//IL_021a: Unknown result type (might be due to invalid IL or missing references)
		//IL_021f: Unknown result type (might be due to invalid IL or missing references)
		//IL_022f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0236: Unknown result type (might be due to invalid IL or missing references)
		//IL_023b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0240: Unknown result type (might be due to invalid IL or missing references)
		//IL_024d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0254: Unknown result type (might be due to invalid IL or missing references)
		//IL_0259: Unknown result type (might be due to invalid IL or missing references)
		//IL_025e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0260: Unknown result type (might be due to invalid IL or missing references)
		//IL_0265: Unknown result type (might be due to invalid IL or missing references)
		//IL_0285: Unknown result type (might be due to invalid IL or missing references)
		//IL_0286: Unknown result type (might be due to invalid IL or missing references)
		//IL_0296: Unknown result type (might be due to invalid IL or missing references)
		//IL_029d: Unknown result type (might be due to invalid IL or missing references)
		//IL_02a2: Unknown result type (might be due to invalid IL or missing references)
		//IL_02a7: Unknown result type (might be due to invalid IL or missing references)
		//IL_02b4: Unknown result type (might be due to invalid IL or missing references)
		//IL_02bb: Unknown result type (might be due to invalid IL or missing references)
		//IL_02c0: Unknown result type (might be due to invalid IL or missing references)
		//IL_02c5: Unknown result type (might be due to invalid IL or missing references)
		//IL_02c7: Unknown result type (might be due to invalid IL or missing references)
		//IL_02cc: Unknown result type (might be due to invalid IL or missing references)
		//IL_0279: Unknown result type (might be due to invalid IL or missing references)
		//IL_027b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0280: Unknown result type (might be due to invalid IL or missing references)
		//IL_0182: Unknown result type (might be due to invalid IL or missing references)
		//IL_018f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0194: Unknown result type (might be due to invalid IL or missing references)
		//IL_02ec: Unknown result type (might be due to invalid IL or missing references)
		//IL_02ed: Unknown result type (might be due to invalid IL or missing references)
		//IL_02ef: Unknown result type (might be due to invalid IL or missing references)
		//IL_02e0: Unknown result type (might be due to invalid IL or missing references)
		//IL_02e2: Unknown result type (might be due to invalid IL or missing references)
		//IL_02e7: Unknown result type (might be due to invalid IL or missing references)
		Curve val = prongCurves[0];
		Curve val2 = prongCurves[1];
		Curve val3 = prongCurves[2];
		Curve val4 = prongCurves[3];
		vDome = null;
		Interval domain = trimPart.Domain;
		double num = ((Interval)(ref domain)).Min;
		domain = trimPart.Domain;
		double max = ((Interval)(ref domain)).Max;
		List<double> list = new List<double>();
		List<double> list2 = new List<double>();
		List<Curve> list3 = new List<Curve>();
		Curve val5 = MakeProngCurve(val2.PointAtStart, val2.PointAtEnd, prongDir, prongDomeHeight, rotateX);
		list3.Add(val5);
		domain = val.Domain;
		list.Add(((Interval)(ref domain)).Min);
		domain = val3.Domain;
		list2.Add(((Interval)(ref domain)).Min);
		double num2 = default(double);
		while (trimPart.GetNextDiscontinuity((Continuity)2, num, max, ref num2))
		{
			Point3d val6 = trimPart.PointAt(num2);
			num = num2;
			double num3 = num2 + 0.001;
			double num4 = num2 - 0.001;
			Vector3d val7 = trimPart.TangentAt(num3);
			Vector3d val8 = trimPart.TangentAt(num4);
			Vector3d val9 = val7 + val8;
			((Vector3d)(ref val9)).Unitize();
			Vector3d val10 = Vector3d.CrossProduct(val9, Vector3d.ZAxis);
			LineCurve val11 = new LineCurve(val6 + 50.0 * val10, val6 - 50.0 * val10);
			CurveIntersections val12 = Intersection.CurveCurve(val, (Curve)(object)val11, 0.01, 0.01);
			CurveIntersections val13 = Intersection.CurveCurve(val3, (Curve)(object)val11, 0.01, 0.01);
			if (val12.Count != 0 && val13.Count != 0)
			{
				Curve item = MakeProngCurve(val12[0].PointA, val13[0].PointA, prongDir, prongDomeHeight, rotateX);
				list3.Add(item);
				list.Add(val12[0].ParameterA);
				list2.Add(val13[0].ParameterA);
			}
		}
		Curve val14 = MakeProngCurve(val4.PointAtStart, val4.PointAtEnd, prongDir, prongDomeHeight, rotateX);
		list3.Add(val14);
		domain = val.Domain;
		list.Add(((Interval)(ref domain)).Max);
		domain = val3.Domain;
		list2.Add(((Interval)(ref domain)).Max);
		Vector3d val15 = val5.PointAtEnd - val5.PointAtStart;
		Vector3d val16 = Vector3d.CrossProduct(val5.PointAtNormalizedLength(0.5) - val5.PointAtStart, val15);
		if (((Vector3d)(ref prongDir)).Z < 0.0)
		{
			val16 = -val16;
		}
		Brep val17 = ((Surface)LoftDir(val2, val5, prongDir, val16)).ToBrep();
		val15 = val14.PointAtEnd - val14.PointAtStart;
		val16 = Vector3d.CrossProduct(val14.PointAtNormalizedLength(0.5) - val14.PointAtStart, val15);
		if (((Vector3d)(ref prongDir)).Z < 0.0)
		{
			val16 = -val16;
		}
		Brep val18 = ((Surface)LoftDir(val4, val14, prongDir, -val16)).ToBrep();
		if (list3.Count > 2)
		{
			for (int i = 1; i < list3.Count - 2; i++)
			{
				if (Math.Abs(list[i] - list[i + 1]) < 0.001 || Math.Abs(list2[i] - list2[i + 1]) < 0.001)
				{
					list3.RemoveAt(i);
					list.RemoveAt(i);
					list2.RemoveAt(i);
					i--;
				}
			}
		}
		if (!CSUtilSweepRC.Sweep2(val, val3, list3.ToArray(), list.ToArray(), list2.ToArray(), out var breps, closed: false, maintainHeight: true))
		{
			return false;
		}
		if (breps == null)
		{
			return false;
		}
		Brep[] inputBreps = (Brep[])(object)new Brep[3] { val17, breps, val18 };
		vDome = CSUtilRhino.JoinAllBreps_BackwardCompatibility(inputBreps, 0.01);
		if (vDome == null)
		{
			return false;
		}
		return true;
	}

	private static Curve MakeProngCurve(Point3d domeStart0, Point3d domeStart1, Vector3d prongDir, double domeRadius, double rotateX)
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_0002: Unknown result type (might be due to invalid IL or missing references)
		//IL_0010: Unknown result type (might be due to invalid IL or missing references)
		//IL_0015: Unknown result type (might be due to invalid IL or missing references)
		//IL_0018: Unknown result type (might be due to invalid IL or missing references)
		//IL_001e: Unknown result type (might be due to invalid IL or missing references)
		//IL_001f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0021: Unknown result type (might be due to invalid IL or missing references)
		//IL_0026: Unknown result type (might be due to invalid IL or missing references)
		//IL_002b: Unknown result type (might be due to invalid IL or missing references)
		//IL_002c: Unknown result type (might be due to invalid IL or missing references)
		//IL_002d: Unknown result type (might be due to invalid IL or missing references)
		//IL_002e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0033: Unknown result type (might be due to invalid IL or missing references)
		//IL_003c: Unknown result type (might be due to invalid IL or missing references)
		//IL_003d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0042: Unknown result type (might be due to invalid IL or missing references)
		//IL_0047: Unknown result type (might be due to invalid IL or missing references)
		//IL_0052: Unknown result type (might be due to invalid IL or missing references)
		//IL_0053: Unknown result type (might be due to invalid IL or missing references)
		//IL_0054: Unknown result type (might be due to invalid IL or missing references)
		//IL_0060: Unknown result type (might be due to invalid IL or missing references)
		//IL_006e: Unknown result type (might be due to invalid IL or missing references)
		//IL_006f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0070: Unknown result type (might be due to invalid IL or missing references)
		//IL_0071: Unknown result type (might be due to invalid IL or missing references)
		//IL_0095: Unknown result type (might be due to invalid IL or missing references)
		//IL_0096: Unknown result type (might be due to invalid IL or missing references)
		//IL_0097: Unknown result type (might be due to invalid IL or missing references)
		//IL_0098: Unknown result type (might be due to invalid IL or missing references)
		Point3d val = (domeStart0 + domeStart1) / 2.0;
		Point3d rotationCenter = default(Point3d);
		((Point3d)(ref rotationCenter))._002Ector(val);
		val += prongDir * domeRadius;
		Vector3d val2 = domeStart0 - domeStart1;
		((Vector3d)(ref val2)).Unitize();
		Vector3d rotationAxis = Vector3d.CrossProduct(val2, Vector3d.ZAxis);
		((Point3d)(ref val)).Transform(CSBlockUtil.CSUtilMath.Rotation(CSBlockUtil.CSUtilMath.ToRadians(0.0 - rotateX), rotationAxis, rotationCenter));
		Vector3d vectorEnd = default(Vector3d);
		((Vector3d)(ref vectorEnd))._002Ector(val2);
		((Vector3d)(ref vectorEnd)).Reverse();
		Curve val3 = (Curve)(object)CSUtilCurves.BlendCurve(domeStart0, val, prongDir, val2);
		Curve val4 = (Curve)(object)CSUtilCurves.BlendCurve(domeStart1, val, prongDir, vectorEnd);
		return (Curve)(object)Curve.JoinCurves((IEnumerable<Curve>)(object)new Curve[2] { val3, val4 })[0].Rebuild(10, 3, true);
	}

	private static NurbsSurface LoftDir(Curve c0, Curve c1, Vector3d v0, Vector3d v1)
	{
		//IL_0013: Unknown result type (might be due to invalid IL or missing references)
		//IL_0018: Unknown result type (might be due to invalid IL or missing references)
		//IL_0074: Unknown result type (might be due to invalid IL or missing references)
		//IL_0079: Unknown result type (might be due to invalid IL or missing references)
		//IL_007d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0082: Unknown result type (might be due to invalid IL or missing references)
		//IL_008c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0091: Unknown result type (might be due to invalid IL or missing references)
		//IL_0095: Unknown result type (might be due to invalid IL or missing references)
		//IL_009a: Unknown result type (might be due to invalid IL or missing references)
		//IL_009e: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a7: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a9: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ac: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ba: Unknown result type (might be due to invalid IL or missing references)
		//IL_00bf: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c4: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c6: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c8: Unknown result type (might be due to invalid IL or missing references)
		//IL_00cb: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d9: Unknown result type (might be due to invalid IL or missing references)
		//IL_00de: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e3: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ed: Unknown result type (might be due to invalid IL or missing references)
		//IL_00fd: Unknown result type (might be due to invalid IL or missing references)
		Brep val = Brep.CreateFromLoft((IEnumerable<Curve>)new List<Curve> { c0, c1 }, Point3d.Unset, Point3d.Unset, (LoftType)2, false)[0];
		if (val.Faces.Count != 1)
		{
			return null;
		}
		NurbsSurface val2 = ((Surface)val.Faces[0]).ToNurbsSurface();
		if (val2.Points.CountU != 4)
		{
			return null;
		}
		int countV = val2.Points.CountV;
		for (int i = 0; i < countV; i++)
		{
			ControlPoint controlPoint = val2.Points.GetControlPoint(0, i);
			Point3d val3 = ((ControlPoint)(ref controlPoint)).Location;
			controlPoint = val2.Points.GetControlPoint(3, i);
			Point3d location = ((ControlPoint)(ref controlPoint)).Location;
			double num = ((Point3d)(ref val3)).DistanceTo(location);
			val3 += v0 * num / 3.0;
			location += v1 * num / 3.0;
			val2.Points.SetControlPoint(1, i, val3);
			val2.Points.SetControlPoint(2, i, location);
		}
		return val2;
	}

	private bool CSHeadFinish(CSData inGem, double railMultiplier, List<Brep> iHeadProngObjects, List<Curve> iHeadProngCurves, List<Brep> iHeadRailObjects, List<Curve> iHeadRailCurves, bool diy, bool pullToBaseObject, CSDataArray inBaseObject, out CSDataArray outGem, out CSDataArray outProngs, out CSDataArray outRails, out CSDataArray head, out CSDataArray prongDomes, out CSDataArray displayCurves, out CSDataArray constructionCurves)
	{
		//IL_01e8: Unknown result type (might be due to invalid IL or missing references)
		//IL_01ed: Unknown result type (might be due to invalid IL or missing references)
		//IL_023d: Unknown result type (might be due to invalid IL or missing references)
		//IL_023f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0241: Unknown result type (might be due to invalid IL or missing references)
		//IL_02a6: Unknown result type (might be due to invalid IL or missing references)
		//IL_02a8: Unknown result type (might be due to invalid IL or missing references)
		//IL_02aa: Unknown result type (might be due to invalid IL or missing references)
		head = new CSDataArray();
		prongDomes = new CSDataArray();
		displayCurves = new CSDataArray();
		outGem = new CSDataArray();
		outProngs = new CSDataArray();
		outRails = new CSDataArray();
		constructionCurves = new CSDataArray();
		bool flag = diy;
		InConstructionTransform.GetValue(out Transform value);
		if (!(inGem is CSGemData cSGemData))
		{
			return false;
		}
		InHeadHeight.GetValue(out double _);
		CSDataArray baseObject = null;
		if (inBaseObject != null)
		{
			baseObject = inBaseObject.GetFlattenArray();
		}
		CSDataArray cSDataArray = new CSDataArray();
		CSDataArray prongs = new CSDataArray();
		foreach (Brep iHeadProngObject in iHeadProngObjects)
		{
			prongs.Append(new CSOnGeometryData((GeometryBase)(object)iHeadProngObject));
		}
		CSDataArray baseRails = new CSDataArray();
		foreach (Brep iHeadRailObject in iHeadRailObjects)
		{
			baseRails.Append(new CSOnGeometryData((GeometryBase)(object)iHeadRailObject));
		}
		CSDataArray prongCurves = new CSDataArray();
		foreach (Curve iHeadProngCurf in iHeadProngCurves)
		{
			prongCurves.Append(new CSOnGeometryData((GeometryBase)(object)iHeadProngCurf));
		}
		CSDataArray baseRailCurves = new CSDataArray();
		foreach (Curve iHeadRailCurf in iHeadRailCurves)
		{
			baseRailCurves.Append(new CSOnGeometryData((GeometryBase)(object)iHeadRailCurf));
		}
		if (pullToBaseObject)
		{
			prongs = prongs.Duplicate() as CSDataArray;
			baseRails = baseRails.Duplicate() as CSDataArray;
			prongCurves = prongCurves.Duplicate() as CSDataArray;
			baseRailCurves = baseRailCurves.Duplicate() as CSDataArray;
			PullToBase(railMultiplier, baseObject, cSGemData, ref prongs, ref baseRails, ref prongCurves, ref baseRailCurves);
		}
		cSGemData.GetGemXformRC(out var xDown, out var _);
		CSDataArray cSDataArray2 = new CSDataArray();
		CSDataArray cSDataArray3 = new CSDataArray();
		if (Transform.Unset != value)
		{
			cSDataArray3.Append(cSGemData.Duplicate());
		}
		if (prongs != null)
		{
			cSDataArray2.Append(prongs.GetFlattenArray().Duplicate());
		}
		if (baseRails != null)
		{
			cSDataArray2.Append(baseRails.GetFlattenArray().Duplicate());
		}
		cSGemData.GetGemXformRC(out var _, out var xUp2);
		cSDataArray2.ApplyTransform(xUp2 * xDown);
		cSDataArray.Append(cSDataArray2.Duplicate());
		outGem.Append(cSDataArray3);
		outProngs = prongs;
		outRails = baseRails;
		head = null;
		prongDomes = cSDataArray.Duplicate() as CSDataArray;
		CSDataArray cSDataArray4 = new CSDataArray();
		cSDataArray4.Append(prongCurves);
		cSDataArray4.Append(baseRailCurves);
		CSDataArray cSDataArray5 = new CSDataArray();
		CSData data = cSDataArray4.Duplicate();
		CSDataHelper.Transform(data, xUp2 * xDown);
		cSDataArray5.Append(data);
		displayCurves = cSDataArray5;
		constructionCurves = cSDataArray5;
		if (!flag)
		{
			head = cSDataArray.GetFlattenArray();
			prongDomes = null;
			displayCurves = null;
			return true;
		}
		return true;
	}

	private void PullToBase(double railMultiplier, CSDataArray baseObject, CSData gems, ref CSDataArray prongs, ref CSDataArray baseRails, ref CSDataArray prongCurves, ref CSDataArray baseRailCurves)
	{
		//IL_012e: Unknown result type (might be due to invalid IL or missing references)
		//IL_007b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0089: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b1: Unknown result type (might be due to invalid IL or missing references)
		//IL_00bf: Unknown result type (might be due to invalid IL or missing references)
		//IL_01b4: Unknown result type (might be due to invalid IL or missing references)
		//IL_01ce: Unknown result type (might be due to invalid IL or missing references)
		//IL_01db: Unknown result type (might be due to invalid IL or missing references)
		//IL_015a: Unknown result type (might be due to invalid IL or missing references)
		//IL_015f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0163: Unknown result type (might be due to invalid IL or missing references)
		//IL_016c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0178: Unknown result type (might be due to invalid IL or missing references)
		//IL_01ab: Unknown result type (might be due to invalid IL or missing references)
		//IL_01b0: Unknown result type (might be due to invalid IL or missing references)
		//IL_01fd: Unknown result type (might be due to invalid IL or missing references)
		//IL_0200: Unknown result type (might be due to invalid IL or missing references)
		//IL_0202: Unknown result type (might be due to invalid IL or missing references)
		//IL_01f2: Unknown result type (might be due to invalid IL or missing references)
		if (baseObject == null)
		{
			return;
		}
		Brep val = null;
		double num = double.NaN;
		CSData cSData = baseObject.GetArrayItem(0);
		if (cSData is CSRingRailData cSRingRailData)
		{
			num = cSRingRailData.Radius;
			cSData = null;
		}
		if (cSData != null)
		{
			Curve val2 = (cSData as ICSCurve)?.GetCurve();
			if (val2 != null)
			{
				Curve val3 = val2.DuplicateCurve();
				Curve val4 = val2.DuplicateCurve();
				((GeometryBase)val3).Translate(new Vector3d(0.0, 1.0, 0.0) * -50.0);
				((GeometryBase)val4).Translate(new Vector3d(0.0, 1.0, 0.0) * 50.0);
				val = ((Surface)NurbsSurface.CreateRuledSurface(val3, val4)).ToBrep();
			}
			if (val == null && cSData is ICSBrep iCSBrep)
			{
				val = iCSBrep.GetBrep();
			}
			if (val == null)
			{
				Surface val5 = (Surface)(object)(cSData as ICSSurface)?.GetNurbsSurface();
				if (val5 != null)
				{
					val = val5.ToBrep();
				}
			}
		}
		if ((double.IsNaN(num) && val == null) || !(gems is CSGemData cSGemData))
		{
			return;
		}
		CSBlockUtil.CSUtilMath.GetUnscaledXform(cSGemData.Xform, out var xUp);
		Transform val6 = default(Transform);
		if (!((Transform)(ref xUp)).TryGetInverse(ref val6))
		{
			return;
		}
		double angle = double.NaN;
		if (!double.IsNaN(num))
		{
			Point3d origin = Point3d.Origin;
			((Point3d)(ref origin)).Transform(xUp);
			Point3d val7 = default(Point3d);
			((Point3d)(ref val7))._002Ector(Vector3d.ZAxis);
			((Point3d)(ref val7)).Transform(xUp);
			angle = CSBlockUtil.CSUtilMath.AngleOfPointD(((Point3d)(ref val7)).X - ((Point3d)(ref origin)).X, ((Point3d)(ref val7)).Z - ((Point3d)(ref origin)).Z);
			val6 = PullToRailMorph.TopCenterXformFromAngle(angle).Convert();
		}
		BoundingBox boundingBox = default(BoundingBox);
		CSDataArray flattenArray = prongs.GetFlattenArray();
		CSDataArray cSDataArray = baseRails;
		flattenArray.GetTightBoundingBox(ref boundingBox, doGrow: true, val6);
		cSDataArray.GetTightBoundingBox(ref boundingBox, doGrow: true, val6);
		CSPullToBaseBrep cSPullToBaseBrep = null;
		SpaceMorph val8;
		if (!double.IsNaN(num))
		{
			val8 = (SpaceMorph)(object)new CSPullToRailMorph(angle, num, railMultiplier, boundingBox);
		}
		else
		{
			cSPullToBaseBrep = new CSPullToBaseBrep(boundingBox, val, xUp, val6);
			val8 = (SpaceMorph)(object)cSPullToBaseBrep;
		}
		val8.Tolerance = 0.05;
		val8.PreserveStructure = true;
		for (int i = 0; i < flattenArray.GetArrayCount(); i++)
		{
			CSData arrayItem = flattenArray.GetArrayItem(i);
			if (arrayItem == null)
			{
				continue;
			}
			CSOnGeometryData cSOnGeometryData = arrayItem as CSOnGeometryData;
			Brep brep = cSOnGeometryData.GetBrep();
			List<Brep> list = new List<Brep>();
			for (int j = 0; j < brep.Faces.Count; j++)
			{
				Brep val9 = ((Surface)brep.Faces[j]).ToBrep();
				if (cSPullToBaseBrep != null)
				{
					cSPullToBaseBrep.Count = 0;
				}
				val8.Morph((GeometryBase)(object)val9);
				list.Add(val9);
			}
			Brep[] array = Brep.JoinBreps((IEnumerable<Brep>)list, 0.05);
			Brep val10 = null;
			if (array.Length != 0)
			{
				val10 = array[0];
				if (!val10.IsSolid)
				{
					Brep[] array2 = Brep.JoinBreps((IEnumerable<Brep>)list, 0.001);
					if (array2.Length != 0)
					{
						Brep val11 = array2[0];
						if (val11.IsSolid)
						{
							val10 = val11;
						}
					}
				}
			}
			cSOnGeometryData = new CSOnGeometryData((GeometryBase)(object)val10);
			flattenArray.Array[i] = cSOnGeometryData;
		}
		prongs = flattenArray;
		for (int k = 0; k < cSDataArray.GetArrayCount(); k++)
		{
			CSData arrayItem2 = cSDataArray.GetArrayItem(k);
			if (arrayItem2 != null)
			{
				if (cSPullToBaseBrep != null)
				{
					cSPullToBaseBrep.Count = 0;
				}
				arrayItem2.Morph(val8);
			}
		}
		for (int l = 0; l < prongCurves.GetArrayCount(); l++)
		{
			CSData arrayItem3 = prongCurves.GetArrayItem(l);
			if (arrayItem3 != null)
			{
				if (cSPullToBaseBrep != null)
				{
					cSPullToBaseBrep.Count = 0;
				}
				arrayItem3.Morph(val8);
			}
		}
		for (int m = 0; m < baseRailCurves.GetArrayCount(); m++)
		{
			CSData arrayItem4 = baseRailCurves.GetArrayItem(m);
			if (arrayItem4 != null)
			{
				if (cSPullToBaseBrep != null)
				{
					cSPullToBaseBrep.Count = 0;
				}
				arrayItem4.Morph(val8);
			}
		}
	}

	private Brep BezelBuilder(CSGemData iGem, CSDataArray baseObject, bool pullToBase, Curve prongLine, Curve headTop, Curve headBase, Curve headMiddle, Curve headGirdle, Curve headSideY, double bezelPlacement, double girdleSpacing, double seatAngle, double seatLength, double seatHeight, double domeHeight, double baseHeight, double baseAnglePosition, double prongTopY, double prongTopNudge, double prongBaseY, double prongBaseNudge, double cornerBlend)
	{
		//IL_002d: Unknown result type (might be due to invalid IL or missing references)
		//IL_003b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0040: Unknown result type (might be due to invalid IL or missing references)
		//IL_005c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0065: Unknown result type (might be due to invalid IL or missing references)
		//IL_006a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0073: Unknown result type (might be due to invalid IL or missing references)
		//IL_0078: Unknown result type (might be due to invalid IL or missing references)
		//IL_0097: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a5: Unknown result type (might be due to invalid IL or missing references)
		//IL_00aa: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c7: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d0: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d5: Unknown result type (might be due to invalid IL or missing references)
		//IL_00de: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e3: Unknown result type (might be due to invalid IL or missing references)
		//IL_0116: Unknown result type (might be due to invalid IL or missing references)
		//IL_011b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0120: Unknown result type (might be due to invalid IL or missing references)
		//IL_0125: Unknown result type (might be due to invalid IL or missing references)
		//IL_0130: Unknown result type (might be due to invalid IL or missing references)
		//IL_0135: Unknown result type (might be due to invalid IL or missing references)
		//IL_016d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0172: Unknown result type (might be due to invalid IL or missing references)
		//IL_0175: Unknown result type (might be due to invalid IL or missing references)
		//IL_017f: Unknown result type (might be due to invalid IL or missing references)
		//IL_019e: Unknown result type (might be due to invalid IL or missing references)
		//IL_01a3: Unknown result type (might be due to invalid IL or missing references)
		//IL_01a8: Unknown result type (might be due to invalid IL or missing references)
		//IL_01ad: Unknown result type (might be due to invalid IL or missing references)
		//IL_01b2: Unknown result type (might be due to invalid IL or missing references)
		//IL_01b7: Unknown result type (might be due to invalid IL or missing references)
		//IL_01c2: Unknown result type (might be due to invalid IL or missing references)
		//IL_01c7: Unknown result type (might be due to invalid IL or missing references)
		//IL_01ed: Unknown result type (might be due to invalid IL or missing references)
		//IL_020a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0215: Unknown result type (might be due to invalid IL or missing references)
		//IL_021a: Unknown result type (might be due to invalid IL or missing references)
		//IL_021e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0223: Unknown result type (might be due to invalid IL or missing references)
		//IL_0230: Unknown result type (might be due to invalid IL or missing references)
		//IL_0235: Unknown result type (might be due to invalid IL or missing references)
		//IL_027f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0284: Unknown result type (might be due to invalid IL or missing references)
		//IL_0290: Unknown result type (might be due to invalid IL or missing references)
		//IL_0295: Unknown result type (might be due to invalid IL or missing references)
		//IL_02d7: Unknown result type (might be due to invalid IL or missing references)
		//IL_02f5: Unknown result type (might be due to invalid IL or missing references)
		//IL_02ce: Unknown result type (might be due to invalid IL or missing references)
		//IL_02d3: Unknown result type (might be due to invalid IL or missing references)
		//IL_0328: Unknown result type (might be due to invalid IL or missing references)
		//IL_032d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0332: Unknown result type (might be due to invalid IL or missing references)
		//IL_0336: Unknown result type (might be due to invalid IL or missing references)
		//IL_033b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0340: Unknown result type (might be due to invalid IL or missing references)
		//IL_0346: Unknown result type (might be due to invalid IL or missing references)
		//IL_0348: Unknown result type (might be due to invalid IL or missing references)
		//IL_035b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0360: Unknown result type (might be due to invalid IL or missing references)
		//IL_0364: Unknown result type (might be due to invalid IL or missing references)
		//IL_0369: Unknown result type (might be due to invalid IL or missing references)
		//IL_0376: Unknown result type (might be due to invalid IL or missing references)
		//IL_037b: Unknown result type (might be due to invalid IL or missing references)
		//IL_03ab: Unknown result type (might be due to invalid IL or missing references)
		//IL_03b0: Unknown result type (might be due to invalid IL or missing references)
		//IL_03c4: Unknown result type (might be due to invalid IL or missing references)
		//IL_03c9: Unknown result type (might be due to invalid IL or missing references)
		//IL_03d2: Unknown result type (might be due to invalid IL or missing references)
		//IL_03d7: Unknown result type (might be due to invalid IL or missing references)
		//IL_03db: Unknown result type (might be due to invalid IL or missing references)
		//IL_0493: Unknown result type (might be due to invalid IL or missing references)
		//IL_0498: Unknown result type (might be due to invalid IL or missing references)
		//IL_045c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0461: Unknown result type (might be due to invalid IL or missing references)
		//IL_0465: Unknown result type (might be due to invalid IL or missing references)
		//IL_04c2: Unknown result type (might be due to invalid IL or missing references)
		//IL_04c7: Unknown result type (might be due to invalid IL or missing references)
		//IL_04cb: Unknown result type (might be due to invalid IL or missing references)
		//IL_04d0: Unknown result type (might be due to invalid IL or missing references)
		//IL_04d6: Unknown result type (might be due to invalid IL or missing references)
		//IL_04d8: Unknown result type (might be due to invalid IL or missing references)
		//IL_054d: Unknown result type (might be due to invalid IL or missing references)
		CSGemData cSGemData = iGem.Duplicate() as CSGemData;
		if (cSGemData == null)
		{
			return null;
		}
		if (seatHeight < 0.01)
		{
			seatHeight = 0.01;
		}
		Vector3d inputDirection = CSBlockUtil.CSUtilMath.AverageTangentAt(headTop, headTop.PointAtStart);
		Vector3d inputVector = new Vector3d(0.0, 0.0, -1.0);
		Interval domain = headSideY.Domain;
		double d = CSBlockUtil.CSUtilMath.AngleBetweenVectorsWithDirection(inputVector, headSideY.TangentAt(((Interval)(ref domain)).Min), inputDirection);
		double num = 1.0 / Math.Cos(d);
		Vector3d inputDirection2 = CSBlockUtil.CSUtilMath.AverageTangentAt(headBase, headBase.PointAtStart);
		Vector3d inputVector2 = new Vector3d(0.0, 0.0, -1.0);
		domain = headSideY.Domain;
		double d2 = CSBlockUtil.CSUtilMath.AngleBetweenVectorsWithDirection(inputVector2, headSideY.TangentAt(((Interval)(ref domain)).Min), inputDirection2);
		double num2 = 1.0 / Math.Cos(d2);
		double num3 = prongTopY * num - prongTopNudge;
		double num4 = prongBaseY * num2 - prongBaseNudge;
		BoundingBox boundingBox = ((GeometryBase)headGirdle).GetBoundingBox(true);
		CSGemData cSGemData2 = cSGemData;
		Vector3d diagonal = ((BoundingBox)(ref boundingBox)).Diagonal;
		double x = ((Vector3d)(ref diagonal)).X;
		diagonal = ((BoundingBox)(ref boundingBox)).Diagonal;
		cSGemData2.ScaleToSize(x, ((Vector3d)(ref diagonal)).Y);
		if (!GemOffsetCurveV2.GemOffset(cSGemData, num3 + girdleSpacing, out Curve curve, doTransformUp: false, out string error, cornerBlend))
		{
			return null;
		}
		Transform val = Transform.Translation(0.0, 0.0, bezelPlacement);
		((GeometryBase)curve).Transform(val);
		((GeometryBase)headGirdle).Transform(val);
		Transform val2 = Transform.Translation(0.0, 0.0, bezelPlacement + seatHeight);
		BoundingBox boundingBox2 = ((GeometryBase)headTop).GetBoundingBox(true);
		CSGemData cSGemData3 = cSGemData;
		diagonal = ((BoundingBox)(ref boundingBox2)).Diagonal;
		double x2 = ((Vector3d)(ref diagonal)).X;
		diagonal = ((BoundingBox)(ref boundingBox2)).Diagonal;
		cSGemData3.ScaleToSize(x2, ((Vector3d)(ref diagonal)).Y);
		if (!GemOffsetCurveV2.GemOffset(cSGemData, num3 + girdleSpacing, out Curve curve2, doTransformUp: false, out error, cornerBlend))
		{
			return null;
		}
		((GeometryBase)curve2).Transform(val2);
		if (!GemOffsetCurveV2.GemOffset(cSGemData, girdleSpacing, out Curve curve3, doTransformUp: false, out error, cornerBlend))
		{
			return null;
		}
		((GeometryBase)curve3).Transform(val2);
		BoundingBox boundingBox3 = ((GeometryBase)headBase).GetBoundingBox(true);
		diagonal = ((BoundingBox)(ref boundingBox3)).Diagonal;
		double x3 = ((Vector3d)(ref diagonal)).X;
		diagonal = ((BoundingBox)(ref boundingBox3)).Diagonal;
		double y = ((Vector3d)(ref diagonal)).Y;
		cSGemData.ScaleToSize(x3, y);
		if (!GemOffsetCurveV2.GemOffset(cSGemData, num4 + girdleSpacing, out Curve curve4, doTransformUp: false, out error, cornerBlend))
		{
			return null;
		}
		Point3d val3;
		if (baseObject == null || baseObject.GetArrayCount() == 0)
		{
			val3 = prongLine.PointAtEnd;
			val2 = Transform.Translation(0.0, 0.0, ((Point3d)(ref val3)).Z + bezelPlacement);
		}
		else
		{
			cSGemData = iGem.Duplicate() as CSGemData;
			double drop = GetDrop(bezelPlacement, baseObject, 0.0, cSGemData);
			val2 = Transform.Translation(0.0, 0.0, drop);
		}
		((GeometryBase)curve4).Transform(val2);
		((GeometryBase)headMiddle).Transform(Transform.Translation(0.0, 0.0, bezelPlacement));
		Brep val4 = BezelOuterSurface(iGem, headMiddle, curve2, curve, curve4, seatHeight, num3, girdleSpacing, num4, baseAnglePosition, cornerBlend);
		if (val4 == null)
		{
			return null;
		}
		double fillet = domeHeight;
		Vector3d startVector = -prongLine.TangentAtStart;
		Vector3d endVector = -prongLine.TangentAtStart;
		if (!BezelSurfaceTop(curve2, curve3, startVector, endVector, fillet, cSGemData, out var srf))
		{
			return null;
		}
		BoundingBox boundingBox4 = ((GeometryBase)curve4).GetBoundingBox(true);
		diagonal = ((BoundingBox)(ref boundingBox4)).Diagonal;
		x3 = ((Vector3d)(ref diagonal)).X;
		diagonal = ((BoundingBox)(ref boundingBox4)).Diagonal;
		y = ((Vector3d)(ref diagonal)).Y;
		if (cSGemData == null)
		{
			return null;
		}
		cSGemData.ScaleToSize(x3, y);
		if (!GemOffsetCurveV2.GemOffset(cSGemData, 0.0 - num4, out Curve curve5, doTransformUp: false, out error, cornerBlend))
		{
			return null;
		}
		val3 = curve4.PointAtStart;
		double x4 = ((Point3d)(ref val3)).X;
		val3 = curve4.PointAtEnd;
		val2 = Transform.Translation(x4, 0.0, ((Point3d)(ref val3)).Z);
		((GeometryBase)curve5).Transform(val2);
		List<Curve> list = new List<Curve> { curve5 };
		Curve curve6 = null;
		cSGemData = iGem.Duplicate() as CSGemData;
		if (seatLength > 0.0)
		{
			double distance = (0.0 - Math.Sin(CSBlockUtil.CSUtilMath.ToRadians(seatAngle))) * seatLength;
			double num5 = (0.0 - Math.Cos(CSBlockUtil.CSUtilMath.ToRadians(seatAngle))) * seatLength;
			if (!GemOffsetCurveV2.GemOffset(cSGemData, distance, out curve6, doTransformUp: false, out error, cornerBlend))
			{
				return null;
			}
			val = Transform.Translation(0.0, 0.0, num5 + bezelPlacement);
			((GeometryBase)curve6).Transform(val);
		}
		if (curve6 != null)
		{
			list.Add(curve6);
		}
		list.Add(headGirdle);
		list.Add(curve3);
		Brep[] array = Brep.CreateFromLoft((IEnumerable<Curve>)list.ToArray(), Point3d.Unset, Point3d.Unset, (LoftType)3, false);
		if (array.Length < 1)
		{
			return null;
		}
		Brep val5 = array[0];
		if (val5 == null)
		{
			return null;
		}
		fillet = 0.0 - baseHeight;
		startVector = prongLine.TangentAtEnd;
		endVector = prongLine.TangentAtEnd;
		if (!BezelSurfaceTop(curve4, curve5, startVector, endVector, fillet, cSGemData, out var srf2))
		{
			return null;
		}
		Brep[] array2 = Brep.JoinBreps((IEnumerable<Brep>)new List<Brep> { val4, srf2, val5, srf }.ToArray(), CSApplicationHelper.Application.Tolerance);
		if (array2.Length < 1)
		{
			return null;
		}
		CSUtilGeometry.FixBrepSolidOrientation(ref array2[0]);
		Brep inBezel = array2[0];
		iGem.GetGemXformRC(out var _, out var xUp);
		((GeometryBase)inBezel).Transform(xUp);
		if (pullToBase)
		{
			PullToBase(baseObject, iGem, ref inBezel);
		}
		return inBezel;
	}

	private Brep BezelOuterSurface(CSData gem, Curve headMiddle, Curve outerTop, Curve girdleCurve, Curve outerBot, double seatHeight, double topThickness, double girdleSpacing, double bottomThickness, double baseAnglePosition, double cornerBlend)
	{
		//IL_0002: Unknown result type (might be due to invalid IL or missing references)
		//IL_0007: Unknown result type (might be due to invalid IL or missing references)
		//IL_001c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0021: Unknown result type (might be due to invalid IL or missing references)
		//IL_002c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0031: Unknown result type (might be due to invalid IL or missing references)
		//IL_0041: Unknown result type (might be due to invalid IL or missing references)
		//IL_0046: Unknown result type (might be due to invalid IL or missing references)
		//IL_0066: Unknown result type (might be due to invalid IL or missing references)
		//IL_006b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0097: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a7: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ac: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b7: Unknown result type (might be due to invalid IL or missing references)
		//IL_00bc: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d3: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d8: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e3: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e8: Unknown result type (might be due to invalid IL or missing references)
		//IL_013d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0142: Unknown result type (might be due to invalid IL or missing references)
		//IL_014d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0152: Unknown result type (might be due to invalid IL or missing references)
		//IL_0178: Unknown result type (might be due to invalid IL or missing references)
		//IL_017d: Unknown result type (might be due to invalid IL or missing references)
		BoundingBox boundingBox = ((GeometryBase)headMiddle).GetBoundingBox(true);
		if (!(gem.Duplicate() is CSGemData cSGemData))
		{
			return null;
		}
		Vector3d diagonal = ((BoundingBox)(ref boundingBox)).Diagonal;
		double x = ((Vector3d)(ref diagonal)).X;
		diagonal = ((BoundingBox)(ref boundingBox)).Diagonal;
		cSGemData.ScaleToSize(x, ((Vector3d)(ref diagonal)).Y);
		Point3d val = ((BoundingBox)(ref boundingBox)).Max;
		double z = ((Point3d)(ref val)).Z;
		Transform val2 = Transform.Translation(0.0, 0.0, z + seatHeight);
		if (!GemOffsetCurveV2.GemOffset(cSGemData, (1.0 - baseAnglePosition) * bottomThickness + baseAnglePosition * topThickness + girdleSpacing, out Curve curve, doTransformUp: false, out string _, cornerBlend))
		{
			return null;
		}
		((GeometryBase)curve).Transform(val2);
		List<Curve> list = new List<Curve>();
		val = ((BoundingBox)(ref boundingBox)).Max;
		double z2 = ((Point3d)(ref val)).Z;
		val = outerBot.PointAtEnd;
		if (!(z2 <= ((Point3d)(ref val)).Z + 0.1))
		{
			val = ((BoundingBox)(ref boundingBox)).Min;
			double z3 = ((Point3d)(ref val)).Z;
			val = girdleCurve.PointAtEnd;
			if (!(z3 >= ((Point3d)(ref val)).Z - 0.6))
			{
				list.Add(outerTop);
				list.Add(girdleCurve);
				list.Add(curve);
				list.Add(outerBot);
				goto IL_013c;
			}
		}
		list.Add(outerTop);
		list.Add(girdleCurve);
		list.Add(outerBot);
		goto IL_013c;
		IL_013c:
		val = outerTop.PointAtEnd;
		double z4 = ((Point3d)(ref val)).Z;
		val = girdleCurve.PointAtEnd;
		if (Math.Abs(z4 - ((Point3d)(ref val)).Z) < CSApplicationHelper.Application.Tolerance)
		{
			list.Remove(outerTop);
		}
		Brep[] array = Brep.CreateFromLoft((IEnumerable<Curve>)list, Point3d.Unset, Point3d.Unset, (LoftType)0, false);
		if (array.Length < 1)
		{
			return null;
		}
		return array[0];
	}

	private static bool BezelSurfaceTop(Curve rail1, Curve rail2, Vector3d startVector, Vector3d endVector, double fillet, CSGemData gemScaled, out Brep srf)
	{
		//IL_001a: Unknown result type (might be due to invalid IL or missing references)
		//IL_001f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0021: Unknown result type (might be due to invalid IL or missing references)
		//IL_002c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0031: Unknown result type (might be due to invalid IL or missing references)
		//IL_0034: Unknown result type (might be due to invalid IL or missing references)
		//IL_0039: Unknown result type (might be due to invalid IL or missing references)
		//IL_003e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0043: Unknown result type (might be due to invalid IL or missing references)
		//IL_004d: Unknown result type (might be due to invalid IL or missing references)
		//IL_004e: Unknown result type (might be due to invalid IL or missing references)
		//IL_004f: Unknown result type (might be due to invalid IL or missing references)
		//IL_005c: Unknown result type (might be due to invalid IL or missing references)
		//IL_005d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0062: Unknown result type (might be due to invalid IL or missing references)
		//IL_0067: Unknown result type (might be due to invalid IL or missing references)
		//IL_0069: Unknown result type (might be due to invalid IL or missing references)
		//IL_006b: Unknown result type (might be due to invalid IL or missing references)
		//IL_006d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0074: Expected O, but got Unknown
		//IL_0099: Unknown result type (might be due to invalid IL or missing references)
		//IL_009e: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a7: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ac: Unknown result type (might be due to invalid IL or missing references)
		//IL_00dd: Unknown result type (might be due to invalid IL or missing references)
		//IL_00df: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e1: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e3: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f8: Unknown result type (might be due to invalid IL or missing references)
		//IL_00fa: Unknown result type (might be due to invalid IL or missing references)
		//IL_00fc: Unknown result type (might be due to invalid IL or missing references)
		//IL_00fe: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ff: Unknown result type (might be due to invalid IL or missing references)
		//IL_0127: Unknown result type (might be due to invalid IL or missing references)
		//IL_0129: Unknown result type (might be due to invalid IL or missing references)
		//IL_012a: Unknown result type (might be due to invalid IL or missing references)
		//IL_012f: Unknown result type (might be due to invalid IL or missing references)
		srf = null;
		if (rail1 == null || rail2 == null)
		{
			return false;
		}
		Point3d val = gemScaled.GetCurve().PointAt(gemScaled.BezelParam);
		double num = default(double);
		rail1.ClosestPoint(val, ref num);
		Point3d val2 = rail1.PointAt(num);
		Vector3d val3 = Vector3d.CrossProduct(rail1.TangentAt(num), Vector3d.ZAxis);
		((Vector3d)(ref val3)).Unitize();
		double num2 = default(double);
		rail2.ClosestPoint(val2 + val3, ref num2);
		Point3d val4 = val2;
		Point3d val5 = rail2.PointAt(num2);
		LineCurve val6 = new LineCurve(val4, val5);
		NurbsCurve val7;
		if (Math.Abs(fillet) < CSApplicationHelper.Application.Tolerance)
		{
			val7 = ((Curve)val6).ToNurbsCurve();
		}
		else
		{
			Interval domain = ((Curve)val6).Domain;
			Point3d val8 = ((Curve)val6).PointAt(((Interval)(ref domain)).Mid);
			double length = ((Curve)val6).GetLength();
			if (double.IsNaN(length) || length < 0.0)
			{
				return false;
			}
			double num3 = length / 2.0;
			Point3d pointStart = val4;
			Point3d pointEnd = val8;
			((Point3d)(ref pointEnd)).Z = ((Point3d)(ref pointEnd)).Z + num3 * fillet;
			Curve item = (Curve)(object)CSUtilCurves.BlendCurve(pointStart, pointEnd, startVector, Vector3d.XAxis);
			Curve item2 = (Curve)(object)CSUtilCurves.BlendCurve(val5, pointEnd, endVector, -Vector3d.XAxis);
			val7 = Curve.JoinCurves((IEnumerable<Curve>)new List<Curve> { item, item2 }, CSApplicationHelper.Application.Tolerance)[0].ToNurbsCurve();
		}
		if (!CSUtilSweepRC.Sweep2(rail1, rail2, (Curve[])(object)new Curve[1] { (Curve)val7 }, new double[1] { num }, new double[1] { num2 }, out var breps, closed: true, maintainHeight: true))
		{
			CS.CSUtil.PrintLine("Failed Sweep2");
			return false;
		}
		if (breps == null)
		{
			return false;
		}
		if (!((CommonObject)breps).IsValid)
		{
			return false;
		}
		srf = breps;
		return true;
	}
}
