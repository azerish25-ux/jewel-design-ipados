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

namespace CSBlocks.Settings.Heads.CSHeadBuilderV1.Obsolete;

[Obsolete]
[CSBlock("HeadUltimate_V8", "{C2C48476-928E-46E2-AEFD-983A91DE4CFD}")]
public sealed class CSHeadUltimate8 : CSBlock
{
	public CSInputChannel InAngleX { get; }

	public CSInputChannel InAngleY { get; }

	public CSInputChannel InBaseAngle { get; }

	public CSInputChannel InBaseAnglePosition { get; }

	public CSInputChannel InBaseObject { get; }

	public CSInputChannel InBaseOffset { get; }

	public CSInputChannel InBasePointRadius { get; }

	public CSInputChannel InBaseStyle { get; }

	public CSInputChannel InBezelPlacement { get; }

	public CSInputChannel InBezelSwitch { get; }

	public CSInputChannel InBuildDomeOnPlane { get; }

	public CSInputChannel InBuildVProngPlanar { get; }

	public CSInputChannel InConstructionTransform { get; }

	public CSInputChannel InCornerBlendRails { get; }

	public CSInputChannel InCornerBlendBezel { get; }

	public CSInputChannel InDomeBlendAmountInside { get; }

	public CSInputChannel InDomeBlendAmountOutside { get; }

	public CSInputChannel InDomeBlendEndInside { get; }

	public CSInputChannel InDomeBlendEndOutside { get; }

	public CSInputChannel InDomeBlendStartInside { get; }

	public CSInputChannel InDomeBlendStartOutside { get; }

	public CSInputChannel InEnableDoItYourself { get; }

	public CSInputChannel InFocalPoint { get; }

	public CSInputChannel InGem { get; }

	public CSInputChannel InGirdleSpacing { get; }

	public CSInputChannel InHeadHeight { get; }

	public CSInputChannel InMoveRails { get; }

	public CSInputChannel InOuterSrfSwitch { get; }

	public CSInputChannel InProngArrangement { get; }

	public CSInputChannel InProngBaseBlend { get; }

	public CSInputChannel InProngBaseDomeHeight { get; }

	public CSInputChannel InProngBaseNudge { get; }

	public CSInputChannel InProngBaseProfile { get; }

	public CSInputChannel InProngBaseX { get; }

	public CSInputChannel InProngBaseY { get; }

	public CSInputChannel InProngDomeHeight { get; }

	public CSInputChannel InProngDomeSettingAngle { get; }

	public CSInputChannel InProngDomeSettingDistance { get; }

	public CSInputChannel InProngHeight { get; }

	public CSInputChannel InProngOverlap { get; }

	public CSInputChannel InProngPoint { get; }

	public CSInputChannel InProngPointAngle { get; }

	public CSInputChannel InProngTopBlend { get; }

	public CSInputChannel InProngTopNudge { get; }

	public CSInputChannel InProngTopProfile { get; }

	public CSInputChannel InProngTopX { get; }

	public CSInputChannel InProngTopY { get; }

	public CSInputChannel InProngType { get; }

	public CSInputChannel InPullRailMultiplier { get; }

	public CSInputChannel InPullToBaseObject { get; }

	public CSInputChannel InRailGem { get; }

	public CSInputChannel InRailOffset { get; }

	public CSInputChannel InRailPos { get; }

	public CSInputChannel InRailProfile { get; }

	public CSInputChannel InRailShearMultiplier { get; }

	public CSInputChannel InRailX { get; }

	public CSInputChannel InRailY { get; }

	public CSInputChannel InSeatAngle { get; }

	public CSInputChannel InSeatLength { get; }

	public CSInputChannel InStraightProng { get; }

	public CSInputChannel InToggleSrfBetweenProngs { get; }

	public CSInputChannel InTopAngle { get; }

	public CSInputChannel InUpperRailPosition { get; }

	public CSOutputChannel OutBezel { get; }

	public CSOutputChannel OutCurves { get; }

	public CSOutputChannel OutDisplayCurves { get; }

	public CSOutputChannel OutGem { get; }

	public CSOutputChannel OutHeadArray { get; }

	public CSOutputChannel OutHeads { get; }

	public CSOutputChannel OutInsideSrfBetweenProngs { get; }

	public CSOutputChannel OutLowerDomes { get; }

	public CSOutputChannel OutOriginCurves { get; }

	public CSOutputChannel OutOuterSrfBeweenProngs { get; }

	public CSOutputChannel OutProngBodies { get; }

	public CSOutputChannel OutProngDomes { get; }

	public CSOutputChannel OutTopDomes { get; }

	public CSHeadUltimate8(string identifier)
		: base(identifier)
	{
		InGem = new CSInputChannel(this, ChannelDataType.Gem, ChannelArrayType.Individual);
		InBaseObject = new CSInputChannel(this, ChannelDataType.Any, ChannelArrayType.Array);
		InConstructionTransform = new CSInputChannel(this, ChannelDataType.Transform, ChannelArrayType.Individual, Transform.Unset);
		InEnableDoItYourself = new CSInputChannel(this, ChannelDataType.Boolean, ChannelArrayType.Individual, defaultValue: false);
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
		InBaseStyle = new CSInputChannel(this, ChannelDataType.Integer, ChannelArrayType.Individual, 122);
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
		InCornerBlendRails = new CSInputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual, 0.0);
		InCornerBlendBezel = new CSInputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual, 0.0);
		InBuildVProngPlanar = new CSInputChannel(this, ChannelDataType.Boolean, ChannelArrayType.Individual, defaultValue: true);
		InStraightProng = new CSInputChannel(this, ChannelDataType.Boolean, ChannelArrayType.Array, new List<bool>());
		InFocalPoint = new CSInputChannel(this, ChannelDataType.Point, ChannelArrayType.Individual, Point3d.Unset);
		InMoveRails = new CSInputChannel(this, ChannelDataType.Boolean, ChannelArrayType.Array, new List<bool>());
		OutHeads = new CSOutputChannel(this, ChannelDataType.Any, ChannelArrayType.Array);
		OutDisplayCurves = new CSOutputChannel(this, ChannelDataType.Any, ChannelArrayType.Array);
		OutProngDomes = new CSOutputChannel(this, ChannelDataType.Any, ChannelArrayType.Array);
		OutGem = new CSOutputChannel(this, ChannelDataType.Gem, ChannelArrayType.Array);
		OutOuterSrfBeweenProngs = new CSOutputChannel(this, ChannelDataType.Any, ChannelArrayType.Array);
		OutInsideSrfBetweenProngs = new CSOutputChannel(this, ChannelDataType.Any, ChannelArrayType.Array);
		OutTopDomes = new CSOutputChannel(this, ChannelDataType.Any, ChannelArrayType.Array);
		OutProngBodies = new CSOutputChannel(this, ChannelDataType.Any, ChannelArrayType.Array);
		OutLowerDomes = new CSOutputChannel(this, ChannelDataType.Any, ChannelArrayType.Array);
		OutHeadArray = new CSOutputChannel(this, ChannelDataType.Any, ChannelArrayType.Array);
		OutBezel = new CSOutputChannel(this, ChannelDataType.Any, ChannelArrayType.Array);
		OutOriginCurves = new CSOutputChannel(this, ChannelDataType.Any, ChannelArrayType.Array);
		OutCurves = new CSOutputChannel(this, ChannelDataType.Any, ChannelArrayType.Array);
		SetChannelIdentifiers(autoCheckInputs: true);
	}

	public override bool Update()
	{
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
		double num3 = InCornerBlendRails.Double;
		double num4 = InCornerBlendBezel.Double;
		Point3d point3d = InFocalPoint.Point3d;
		List<bool> booleanList3 = InMoveRails.BooleanList;
		double bezelPlacement = InBezelPlacement.Double;
		double girdleSpacing = InGirdleSpacing.Double;
		double seatAngle = InSeatAngle.Double;
		double seatLength = InSeatLength.Double;
		Transform transform = InConstructionTransform.Transform;
		if (!(gem.Duplicate() is CSGemData cSGemData))
		{
			return UpdateError("missing Gem");
		}
		if (transform != Transform.Unset)
		{
			Transform transform2 = Transform.Translation(0.0, 0.0, 0.0 - num2);
			Transform transform3 = transform * transform2;
			cSGemData.ApplyTransform(transform3);
		}
		CSDataArray originCurves = new CSDataArray();
		if (!HeadShape(cSGemData, num, num2, flattenArray, baseOffset, angleX, angleY, topAngle, baseAngle, baseAnglePosition, integer, integerList2, integerList, doubleList2, booleanList2, boolean2 ? num4 : num3, point3d, doubleList5, doubleList9, booleanList2, out var headGirdle, out var headTop, out var headBase, out var headMiddleCurve, out var outHeadSideX, out var outHeadSideY, out var headProngLines, out var _, out var _, out var surfaceBetweenProngs))
		{
			return UpdateError("HeadShape Failed");
		}
		headProngLines.ForEach(delegate(Curve _)
		{
			originCurves.Append(new CSOnGeometryData(_));
		});
		originCurves.Append(new CSOnGeometryData(headGirdle));
		originCurves.Append(new CSOnGeometryData(headTop));
		originCurves.Append(new CSOnGeometryData(headBase));
		originCurves.Append(new CSOnGeometryData(outHeadSideX));
		originCurves.Append(new CSOnGeometryData(outHeadSideY));
		List<Brep> list2 = new List<Brep>();
		List<Curve> list3 = new List<Curve>();
		CSDataArray cSDataArray = new CSDataArray();
		CSDataArray cSDataArray2 = new CSDataArray();
		CSDataArray cSDataArray3 = new CSDataArray();
		cSGemData.GetGemXformRC(out var _, out var xUp);
		CSDataArray cSDataArray4 = new CSDataArray();
		if (boolean3)
		{
			for (int num5 = 0; num5 < integerList2.Count; num5++)
			{
				int prongPointIndex = ((num5 < integerList2.Count - 1) ? integerList2[num5 + 1] : integerList2[0]);
				Brep brep = HeadOuterSurface(cSGemData, headMiddleCurve, headTop, headGirdle, headBase, outHeadSideY, headProngLines[0], integerList2[num5], prongPointIndex, num, girdleSpacing, doubleList4[0], doubleList5[0], doubleList8[0], doubleList9[0], bezelPlacement);
				brep.Transform(xUp);
				cSDataArray4.Append(new CSOnGeometryData(brep.DuplicateBrep()));
			}
		}
		if (boolean2)
		{
			Brep brep2 = BezelBuilder(cSGemData, flattenArray, boolean6, headProngLines[0], headTop, headBase, headMiddleCurve, headGirdle, outHeadSideY, bezelPlacement, girdleSpacing, seatAngle, seatLength, num, doubleList11[0], doubleList14[0], baseAnglePosition, doubleList4[0], doubleList5[0], doubleList8[0], doubleList9[0], num4);
			if (brep2 == null)
			{
				return UpdateError("BezelBuilder failed");
			}
			OutBezel.SetData(new CSOnGeometryData(brep2));
			OutGem.SetData(cSGemData);
			OutOuterSrfBeweenProngs.SetData(cSDataArray4);
			OutOriginCurves.SetData(originCurves);
		}
		else
		{
			for (int num6 = 0; num6 < integerList2.Count; num6++)
			{
				Curve iProngRail = headProngLines[num6];
				if (!HeadProng(num6, cSGemData, integerList3[num6], integerList[num6], headProngLines, iProngRail, headTop, headBase, doubleList6[num6], doubleList10[num6], curveList3[num6], doubleList3[num6], doubleList4[num6], doubleList3[num6], doubleList5[num6], curveList2[num6], doubleList7[num6], doubleList8[num6], doubleList9[num6], doubleList7[num6], doubleList11[num6], doubleList12[num6], doubleList13[num6], integer, basePointSize, boolean, boolean4, boolean5, doubleList14[num6], doubleList[num6], doubleList19[num6], doubleList20[num6], doubleList21[num6], doubleList22[num6], doubleList23[num6], doubleList24[num6], point3d, out var topDome, out var middleProng, out var lowerDome, out var prongs, out var curves))
				{
					return UpdateError("Prong Creation Failed");
				}
				if (topDome != null)
				{
					foreach (Brep item in topDome)
					{
						cSDataArray.Append(new CSOnGeometryData(item.DuplicateBrep()));
					}
				}
				if (middleProng != null)
				{
					foreach (Brep item2 in middleProng)
					{
						cSDataArray2.Append(new CSOnGeometryData(item2.DuplicateBrep()));
					}
				}
				if (lowerDome != null)
				{
					foreach (Brep item3 in lowerDome)
					{
						cSDataArray3.Append(new CSOnGeometryData(item3.DuplicateBrep()));
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
			for (int num7 = 0; num7 < doubleList15.Count; num7++)
			{
				if (!HeadRail(cSGemData, gemList[num7], outHeadSideX, outHeadSideY, doubleList15[num7], curveList[num7], list[num7], doubleList16[num7], doubleList17[num7], boolean, doubleList18[num7], booleanList[num7], num3, point3d, booleanList3[num7], out var objects, out var railCurves, out var _, out var _))
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
			InPullRailMultiplier.GetValue(out double value);
			if (!HeadFinish(cSGemData, value, list2, list3, list4, list5, boolean, boolean6, flattenArray, out var outGem, out var outProngs, out var outRails, out var head, out var prongDomes, out var displayCurves, out var constructionCurves))
			{
				return UpdateError("HeadFinish Failed");
			}
			CSDataArray cSDataArray5 = new CSDataArray();
			if (surfaceBetweenProngs.Count > 0)
			{
				foreach (Brep item8 in surfaceBetweenProngs)
				{
					cSDataArray5.Append(new CSOnGeometryData(item8));
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

	private static Brep HeadOuterSurface(CSData gem, GeometryBase headMiddle, Curve headTop, GeometryBase headGirdle, Curve headBase, Curve headSideY, Curve prongLine, int prongPointIndex1, int prongPointIndex2, double seatHeight, double girdleSpacing, double prongTopY, double prongTopNudge, double prongBaseY, double prongBaseNudge, double bezelPlacement)
	{
		if (!(gem.Duplicate() is CSGemData cSGemData))
		{
			return null;
		}
		cSGemData.ApplyTransform(CSBlockUtil.CSUtilMath.Orient3Point(cSGemData.Center, cSGemData.Up, cSGemData.East, Point3d.Origin, Vector3d.ZAxis, Vector3d.XAxis));
		Vector3d inputDirection = CSBlockUtil.CSUtilMath.AverageTangentAt(headTop, headTop.PointAtStart);
		double d = CSBlockUtil.CSUtilMath.AngleBetweenVectorsWithDirection(new Vector3d(0.0, 0.0, -1.0), headSideY.TangentAt(headSideY.Domain.Min), inputDirection);
		double num = 1.0 / Math.Cos(d);
		Vector3d inputDirection2 = CSBlockUtil.CSUtilMath.AverageTangentAt(headBase, headBase.PointAtStart);
		double d2 = CSBlockUtil.CSUtilMath.AngleBetweenVectorsWithDirection(new Vector3d(0.0, 0.0, -1.0), headSideY.TangentAt(headSideY.Domain.Min), inputDirection2);
		double num2 = 1.0 / Math.Cos(d2);
		double num3 = prongTopY * num - prongTopNudge;
		double num4 = prongBaseY * num2 - prongBaseNudge;
		BoundingBox boundingBox = headGirdle.GetBoundingBox(accurate: true);
		cSGemData.ScaleToSize(boundingBox.Diagonal.X, boundingBox.Diagonal.Y);
		Curve curve = cSGemData.GetCurve();
		if (!GemOffsetCurve.GemOffset(cSGemData, num3 + girdleSpacing, out Curve curve2, false, out string error, 0.0))
		{
			return null;
		}
		Transform xform = Transform.Translation(0.0, 0.0, bezelPlacement);
		curve2.Transform(xform);
		List<CSGemInstanceDefinition.ProngPosition> prongPositions = cSGemData.Def.ProngPositions;
		Point3d point3d = curve.PointAt(prongPositions[prongPointIndex1].ProngParam);
		Point3d point3d2 = curve.PointAt(prongPositions[prongPointIndex2].ProngParam);
		Vector3d a = CSBlockUtil.CSUtilMath.AverageTangentAt(curve, point3d);
		Vector3d a2 = CSBlockUtil.CSUtilMath.AverageTangentAt(curve, point3d2);
		Vector3d vector3d = Vector3d.CrossProduct(a, Vector3d.ZAxis);
		Vector3d vector3d2 = Vector3d.CrossProduct(a2, Vector3d.ZAxis);
		NurbsCurve nurbsCurve = new Line(point3d, -vector3d, (num3 + girdleSpacing) * 3.0).ToNurbsCurve();
		nurbsCurve.Transform(Transform.Translation(0.0, 0.0, curve2.PointAtStart.Z - nurbsCurve.PointAtStart.Z));
		NurbsCurve nurbsCurve2 = new Line(point3d2, -vector3d2, (num3 + girdleSpacing) * 3.0).ToNurbsCurve();
		nurbsCurve2.Transform(Transform.Translation(0.0, 0.0, curve2.PointAtStart.Z - nurbsCurve2.PointAtStart.Z));
		CurveIntersections curveIntersections = Intersection.CurveCurve(curve2, nurbsCurve, CSApplicationHelper.Application.Tolerance, CSApplicationHelper.Application.Tolerance);
		CurveIntersections curveIntersections2 = Intersection.CurveCurve(curve2, nurbsCurve2, CSApplicationHelper.Application.Tolerance, CSApplicationHelper.Application.Tolerance);
		curve2.ClosestPoint(curveIntersections[0].PointA, out var t);
		curve2.ClosestPoint(curveIntersections2[0].PointA, out var t2);
		curve2 = curve2.Trim(t, t2);
		Transform xform2 = Transform.Translation(0.0, 0.0, bezelPlacement + seatHeight);
		BoundingBox boundingBox2 = headTop.GetBoundingBox(accurate: true);
		cSGemData.ScaleToSize(boundingBox2.Diagonal.X, boundingBox2.Diagonal.Y);
		if (!GemOffsetCurve.GemOffset(cSGemData, num3 + girdleSpacing, out Curve curve3, false, out error, 0.0))
		{
			return null;
		}
		curve3.Transform(xform2);
		curve = cSGemData.GetCurve();
		point3d = curve.PointAt(prongPositions[prongPointIndex1].ProngParam);
		point3d2 = curve.PointAt(prongPositions[prongPointIndex2].ProngParam);
		nurbsCurve = new Line(point3d, -vector3d, (num3 + girdleSpacing) * 3.0).ToNurbsCurve();
		nurbsCurve.Transform(Transform.Translation(0.0, 0.0, curve3.PointAtStart.Z - nurbsCurve.PointAtStart.Z));
		nurbsCurve2 = new Line(point3d2, -vector3d2, (num3 + girdleSpacing) * 3.0).ToNurbsCurve();
		nurbsCurve2.Transform(Transform.Translation(0.0, 0.0, curve3.PointAtStart.Z - nurbsCurve2.PointAtStart.Z));
		curveIntersections = Intersection.CurveCurve(curve3, nurbsCurve, CSApplicationHelper.Application.Tolerance, CSApplicationHelper.Application.Tolerance);
		curveIntersections2 = Intersection.CurveCurve(curve3, nurbsCurve2, CSApplicationHelper.Application.Tolerance, CSApplicationHelper.Application.Tolerance);
		curve3.ClosestPoint(curveIntersections[0].PointA, out t);
		curve3.ClosestPoint(curveIntersections2[0].PointA, out t2);
		curve3 = curve3.Trim(t, t2);
		BoundingBox boundingBox3 = headBase.GetBoundingBox(accurate: true);
		double x = boundingBox3.Diagonal.X;
		double y = boundingBox3.Diagonal.Y;
		cSGemData.ScaleToSize(x, y);
		xform2 = Transform.Translation(0.0, 0.0, prongLine.PointAtEnd.Z + bezelPlacement);
		if (!GemOffsetCurve.GemOffset(cSGemData, num4 + girdleSpacing, out Curve curve4, false, out error, 0.0))
		{
			return null;
		}
		curve4.Transform(xform2);
		curve = cSGemData.GetCurve();
		point3d = curve.PointAt(prongPositions[prongPointIndex1].ProngParam);
		point3d2 = curve.PointAt(prongPositions[prongPointIndex2].ProngParam);
		nurbsCurve = new Line(point3d, -vector3d, (num4 + girdleSpacing) * 3.0).ToNurbsCurve();
		nurbsCurve.Transform(Transform.Translation(0.0, 0.0, curve4.PointAtStart.Z - nurbsCurve.PointAtStart.Z));
		nurbsCurve2 = new Line(point3d2, -vector3d2, (num4 + girdleSpacing) * 3.0).ToNurbsCurve();
		nurbsCurve2.Transform(Transform.Translation(0.0, 0.0, curve4.PointAtStart.Z - nurbsCurve2.PointAtStart.Z));
		curveIntersections = Intersection.CurveCurve(curve4, nurbsCurve, CSApplicationHelper.Application.Tolerance, CSApplicationHelper.Application.Tolerance);
		curveIntersections2 = Intersection.CurveCurve(curve4, nurbsCurve2, CSApplicationHelper.Application.Tolerance, CSApplicationHelper.Application.Tolerance);
		curve4.ClosestPoint(curveIntersections[0].PointA, out t);
		curve4.ClosestPoint(curveIntersections2[0].PointA, out t2);
		curve4 = curve4.Trim(t, t2);
		BoundingBox boundingBox4 = headMiddle.GetBoundingBox(accurate: true);
		cSGemData.ScaleToSize(boundingBox4.Diagonal.X, boundingBox4.Diagonal.Y);
		double z = boundingBox4.Max.Z;
		xform2 = Transform.Translation(0.0, 0.0, z + seatHeight);
		if (!GemOffsetCurve.GemOffset(cSGemData, num3 + girdleSpacing, out Curve curve5, false, out error, 0.0))
		{
			return null;
		}
		curve5.Transform(xform2);
		curve = cSGemData.GetCurve();
		point3d = curve.PointAt(prongPositions[prongPointIndex1].ProngParam);
		point3d2 = curve.PointAt(prongPositions[prongPointIndex2].ProngParam);
		nurbsCurve = new Line(point3d, -vector3d, (num3 + girdleSpacing) * 3.0).ToNurbsCurve();
		nurbsCurve.Transform(Transform.Translation(0.0, 0.0, curve5.PointAtStart.Z - nurbsCurve.PointAtStart.Z));
		nurbsCurve2 = new Line(point3d2, -vector3d2, (num3 + girdleSpacing) * 3.0).ToNurbsCurve();
		nurbsCurve2.Transform(Transform.Translation(0.0, 0.0, curve5.PointAtStart.Z - nurbsCurve2.PointAtStart.Z));
		curveIntersections = Intersection.CurveCurve(curve5, nurbsCurve, CSApplicationHelper.Application.Tolerance, CSApplicationHelper.Application.Tolerance);
		curveIntersections2 = Intersection.CurveCurve(curve5, nurbsCurve2, CSApplicationHelper.Application.Tolerance, CSApplicationHelper.Application.Tolerance);
		curve5.ClosestPoint(curveIntersections[0].PointA, out t);
		curve5.ClosestPoint(curveIntersections2[0].PointA, out t2);
		curve5 = curve5.Trim(t, t2);
		List<Curve> list = new List<Curve>();
		if (boundingBox4.Max.Z <= curve4.PointAtEnd.Z + 0.1 || boundingBox4.Min.Z >= curve2.PointAtEnd.Z - 0.6)
		{
			list.Add(curve3);
			list.Add(curve2);
			list.Add(curve4);
		}
		else
		{
			list.Add(curve3);
			list.Add(curve2);
			list.Add(curve5);
			list.Add(curve4);
		}
		if (Math.Abs(curve3.PointAtEnd.Z - curve2.PointAtEnd.Z) < CSApplicationHelper.Application.Tolerance)
		{
			list.Remove(curve3);
		}
		Brep[] array = Brep.CreateFromLoft(list, Point3d.Unset, Point3d.Unset, LoftType.Normal, closed: false);
		if (array.Length < 1)
		{
			return null;
		}
		return array[0];
	}

	private bool HeadRail(CSGemData gem, CSGemData railShapeGem, Curve headSideX, Curve headSideY, double railZPos, Curve inRailProfile, double inRailX, double inRailY, double inRailOffset, bool enableModeDoItYourSelf, double shearMultiplier, bool upperRail, double cornerBlend, Point3d focalPoint, bool moveRail, out List<Brep> objects, out List<Curve> railCurves, out List<double> zPosition, out List<double> xPosition)
	{
		objects = new List<Brep>();
		railCurves = new List<Curve>();
		zPosition = new List<double>();
		xPosition = new List<double>();
		Curve curve = inRailProfile.DuplicateCurve();
		if (curve == null)
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
		double num = headSideX.PointAtEnd.Z * (1.0 - railZPos);
		if (upperRail)
		{
			num = Point3d.Origin.Z - inRailY / 2.0 - railZPos;
		}
		zPosition.Add(num + inRailY);
		if (gem == null)
		{
			return false;
		}
		CSBlockUtil.CSUtilMath.GetUnscaledXforms(gem.Xform, out var xUp, out var xDown);
		CurveIntersections curveIntersections = Intersection.CurveCurve(new LineCurve(new Point3d(0.0, 0.0, num), new Point3d(100.0, 0.0, num)), headSideX, 0.01, 0.01);
		if (curveIntersections.Count <= 0)
		{
			return false;
		}
		CurveIntersections curveIntersections2 = Intersection.CurveCurve(new LineCurve(new Point3d(0.0, 0.0, num), new Point3d(0.0, 100.0, num)), headSideY, 0.01, 0.01);
		if (curveIntersections2.Count <= 0)
		{
			return false;
		}
		double x = curveIntersections[0].PointA.X;
		double y = curveIntersections2[0].PointA.Y;
		xPosition.Add(x - gem.GemXMax);
		double parameterB = curveIntersections[0].ParameterB;
		double num2 = (0.0 - headSideX.TangentAt(parameterB).X) * shearMultiplier;
		Transform transform = Transform.Scale(Plane.WorldXY, x / railShapeGem.GemXMax, y / railShapeGem.GemYMax, 1.0);
		CSGemData cSGemData = new CSGemData(railShapeGem);
		cSGemData.ApplyTransform(xDown);
		cSGemData.ApplyTransform(transform);
		if (!GemOffsetCurve.GemOffset(cSGemData, inRailOffset, out Curve curve2, false, out string _, cornerBlend))
		{
			return false;
		}
		if (curve2 == null)
		{
			return false;
		}
		if (!GemOffsetCurve.GemOffset(cSGemData, inRailOffset + (0.0 - inRailX), out Curve curve3, false, out string _, 0.0))
		{
			return false;
		}
		if (curve3 == null)
		{
			return false;
		}
		Transform xform = Transform.Translation(0.0, 0.0, num);
		Point3d testPoint = cSGemData.GetCurve().PointAt(railShapeGem.BezelParam);
		curve2.ClosestPoint(testPoint, out var t);
		Point3d point3d = curve2.PointAt(t);
		Vector3d vector3d = Vector3d.CrossProduct(curve2.TangentAt(t), Vector3d.ZAxis);
		vector3d.Unitize();
		vector3d *= inRailX;
		double num3 = inRailOffset + (0.0 - inRailX);
		double t2;
		if (num3 < 0.0 && (Math.Abs(num3) > cSGemData.GemXSize / 2.0 || Math.Abs(num3) > cSGemData.GemYSize / 2.0))
		{
			curve3.ClosestPoint(point3d - vector3d, out t2);
		}
		else
		{
			curve3.ClosestPoint(point3d + vector3d, out t2);
		}
		NurbsCurve nurbsCurve = curve.ToNurbsCurve();
		BoundingBox boundingBox = nurbsCurve.GetBoundingBox(accurate: true);
		Transform transform2 = Transform.Translation(0.0 - boundingBox.Min.X, 0.0 - boundingBox.Min.Y, 0.0 - boundingBox.Min.Z);
		Transform transform3 = Transform.Scale(Plane.WorldXY, inRailX / boundingBox.Diagonal.X, inRailY / boundingBox.Diagonal.Y, 0.0);
		nurbsCurve.Transform(transform3 * transform2);
		for (int i = 0; i < nurbsCurve.Points.Count; i++)
		{
			ControlPoint value = nurbsCurve.Points[i];
			Point3d location = value.Location;
			location.X += num2 * location.Y;
			value.Location = location;
			nurbsCurve.Points[i] = value;
		}
		curve2.ClosestPoint(testPoint, out var t3);
		Point3d destination = curve2.PointAt(t3);
		Transform xform2 = CSBlockUtil.CSUtilMath.Orient3Point(new Point3d(inRailX, 0.0, 0.0), new Vector3d(-1.0, 0.0, 0.0), new Vector3d(0.0, 1.0, 0.0), destination, vector3d, Vector3d.ZAxis);
		nurbsCurve.Transform(xform2);
		curve2.Transform(xform);
		curve3.Transform(xform);
		nurbsCurve.Transform(xform);
		if (focalPoint != Point3d.Unset && moveRail)
		{
			Point3d point3d2 = focalPoint;
			point3d2.Transform(xDown);
			double num4 = point3d2.X / point3d2.Z;
			double num5 = point3d2.Y / point3d2.Z;
			curve2.Transform(Transform.Translation(curve2.PointAtStart.Z * num4, curve2.PointAtStart.Z * num5, 0.0));
			curve3.Transform(Transform.Translation(curve2.PointAtStart.Z * num4, curve2.PointAtStart.Z * num5, 0.0));
			nurbsCurve.Transform(Transform.Translation(curve2.PointAtStart.Z * num4, curve2.PointAtStart.Z * num5, 0.0));
		}
		curve2.Transform(xUp);
		curve3.Transform(xUp);
		nurbsCurve.Transform(xUp);
		railCurves.Add(curve2);
		railCurves.Add(curve3);
		railCurves.Add(nurbsCurve);
		if (!enableModeDoItYourSelf)
		{
			List<Curve> profile = new List<Curve> { nurbsCurve };
			List<double> parameters = new List<double> { t };
			List<double> parameters2 = new List<double> { t2 };
			if (!CSUtilSweepRC.Sweep2(curve2, curve3, profile, parameters, parameters2, out Brep breps, true, true))
			{
				return false;
			}
			objects.Add(breps);
		}
		return true;
	}

	private static void PullToBase(ICSArray baseObject, CSData gems, ref Brep inBezel)
	{
		if (baseObject == null)
		{
			return;
		}
		Brep brep = null;
		double num = double.NaN;
		CSData cSData = baseObject.GetArrayItem(0);
		if (cSData is CSRingRailData cSRingRailData)
		{
			num = cSRingRailData.Radius;
			cSData = null;
		}
		if (cSData != null)
		{
			Curve curve = (cSData as ICSCurve)?.GetCurve();
			if (curve != null)
			{
				Curve curve2 = curve.DuplicateCurve();
				Curve curve3 = curve.DuplicateCurve();
				curve2.Translate(new Vector3d(0.0, 1.0, 0.0) * -50.0);
				curve3.Translate(new Vector3d(0.0, 1.0, 0.0) * 50.0);
				brep = NurbsSurface.CreateRuledSurface(curve2, curve3).ToBrep();
			}
			if (brep == null && cSData is ICSBrep iCSBrep)
			{
				brep = iCSBrep.GetBrep();
			}
			if (brep == null)
			{
				Surface surface = (cSData as ICSSurface)?.GetNurbsSurface();
				if (surface != null)
				{
					brep = surface.ToBrep();
				}
			}
		}
		if ((double.IsNaN(num) && brep == null) || !(gems is CSGemData cSGemData))
		{
			return;
		}
		CSBlockUtil.CSUtilMath.GetUnscaledXform(cSGemData.Xform, out var xUp);
		if (xUp.TryGetInverse(out var inverseTransform))
		{
			double angle = double.NaN;
			if (!double.IsNaN(num))
			{
				Point3d origin = Point3d.Origin;
				origin.Transform(xUp);
				Point3d point3d = new Point3d(Vector3d.ZAxis);
				point3d.Transform(xUp);
				angle = CSBlockUtil.CSUtilMath.AngleOfPointD(point3d.X - origin.X, point3d.Z - origin.Z);
				inverseTransform = PullToRailMorph.TopCenterXformFromAngle(angle).Convert();
			}
			BoundingBox boundingBox = inBezel.GetBoundingBox(inverseTransform);
			SpaceMorph spaceMorph = (double.IsNaN(num) ? ((SpaceMorph)new CSPullToBaseBrep(boundingBox, brep, xUp, inverseTransform)) : ((SpaceMorph)new CSPullToRailCenterMorph(angle, num, 1.0, boundingBox)));
			spaceMorph.Tolerance = 0.05;
			spaceMorph.PreserveStructure = true;
			Brep brep2 = inBezel.DuplicateBrep();
			spaceMorph.Morph(brep2);
			if (brep2 != null)
			{
				inBezel = brep2.DuplicateBrep();
			}
		}
	}

	private bool HeadShape(CSData iGem, double prongHeight, double headHeight, CSDataArray baseObject, double baseOffset, double angleX, double angleY, double topAngle, double baseAngle, double baseAnglePosition, int baseStyle, List<int> prongPointList, List<int> prongType, List<double> rotateAngle, List<bool> straightProng, double cornerBlend, Point3d focalPoint, List<double> nudgeTop, List<double> nudgeBot, List<bool> straightProngToggle, out Curve headGirdle, out Curve headTop, out Curve headBase, out Curve headMiddleCurve, out Curve outHeadSideX, out Curve outHeadSideY, out List<Curve> headProngLines, out CSData outBaseObject, out double gemHeight, out List<Brep> surfaceBetweenProngs)
	{
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
		curve.Transform(xDown);
		if (!GemOffsetCurve.GemOffset(cSGemData, 0.0, out Curve curve2, false, out string _, cornerBlend))
		{
			return false;
		}
		curve2.MakeDeformable();
		double num = drop * Math.Tan(CSBlockUtil.CSUtilMath.ToRadians(angleX));
		double num2 = drop * Math.Tan(CSBlockUtil.CSUtilMath.ToRadians(angleY));
		double num3 = gemXSize + num * 2.0;
		double num4 = gemYSize + num2 * 2.0;
		Transform transform = Transform.Scale(Plane.WorldXY, num3 / gemXSize, num4 / gemYSize, 1.0);
		Transform xform = Transform.Translation(0.0, 0.0, drop) * transform;
		Curve curve3 = curve2.DuplicateCurve();
		curve3.Transform(xform);
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
		Transform transform2 = Transform.Scale(Plane.WorldXY, (num3 + num5 * 2.0) / gemXSize, (num4 + num6 * 2.0) / gemYSize, 1.0);
		Transform xform2 = Transform.Translation(0.0, 0.0, drop * (1.0 - baseAnglePosition)) * transform2;
		Curve curve4 = curve2.DuplicateCurve();
		curve4.Transform(xform2);
		headMiddleCurve = curve4;
		double num7 = prongHeight * Math.Tan(CSBlockUtil.CSUtilMath.ToRadians(angleX + topAngle));
		double num8 = prongHeight * Math.Tan(CSBlockUtil.CSUtilMath.ToRadians(angleY + topAngle));
		Transform transform3 = Transform.Scale(Plane.WorldXY, (gemXSize + num7 * 2.0) / gemXSize, (gemYSize + num8 * 2.0) / gemYSize, 1.0);
		Transform xform3 = Transform.Translation(0.0, 0.0, prongHeight) * transform3;
		Curve curve5 = curve2.DuplicateCurve();
		curve5.Transform(xform3);
		Point3d item = new Point3d(gemXMax, 0.0, 0.0);
		item.Transform(xform3);
		Point3d item2 = new Point3d(gemXMax, 0.0, 0.0);
		Point3d item3 = new Point3d(gemXMax, 0.0, 0.0);
		item3.Transform(xform2);
		Point3d item4 = new Point3d(gemXMax, 0.0, 0.0);
		item4.Transform(xform);
		Curve curve6 = ProngInterpCurve(new List<Point3d> { item, item2, item3, item4 });
		Point3d item5 = new Point3d(0.0, gemYMax, 0.0);
		item5.Transform(xform3);
		Point3d item6 = new Point3d(0.0, gemYMax, 0.0);
		Point3d item7 = new Point3d(0.0, gemYMax, 0.0);
		item7.Transform(xform2);
		Point3d item8 = new Point3d(0.0, gemYMax, 0.0);
		item8.Transform(xform);
		Curve curve7 = ProngInterpCurve(new List<Point3d> { item5, item6, item7, item8 });
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
		for (int i = 0; i < prongPointList.Count; i++)
		{
			double angleDegrees = rotateAngle[num9];
			num9++;
			LineCurve lineCurve = new LineCurve(to: curve.PointAt(list[prongPointList[i]].ProngParam) * 50.0, from: Point3d.Origin);
			Transform xform4 = CSBlockUtil.CSUtilMath.Rotation(CSBlockUtil.CSUtilMath.ToRadians(angleDegrees), Vector3d.ZAxis, Point3d.Origin);
			lineCurve.Transform(xform4);
			CurveIntersections curveIntersections = Intersection.CurveCurve(curve2, lineCurve, 0.001, 0.001);
			if (curveIntersections.Count <= 0)
			{
				continue;
			}
			double parameterA = curveIntersections[0].ParameterA;
			list2.Add(parameterA);
			Point3d point3d = new Point3d(curve2.PointAt(parameterA));
			Point3d point3d2 = new Point3d(point3d);
			point3d2.Transform(xform);
			curve3.ClosestPoint(point3d2, out var t);
			list3.Add(t);
			Point3d item9 = new Point3d(point3d);
			item9.Transform(xform2);
			if (straightProng[i])
			{
				item9 = Point3d.Unset;
			}
			Point3d point3d3 = new Point3d(point3d);
			point3d3.Transform(xform3);
			if (baseStyle == 122)
			{
				Vector3d motion = new Vector3d(new Point3d(0.0, 0.0, point3d3.Z) - point3d3);
				motion.Unitize();
				motion *= nudgeTop[i];
				point3d3.Transform(Transform.Translation(motion));
				point3d.Transform(Transform.Translation(motion));
				motion = new Vector3d(new Point3d(0.0, 0.0, point3d3.Z) - point3d3);
				motion.Unitize();
				motion *= nudgeBot[i] * (1.0 - baseAnglePosition) + nudgeTop[i] * baseAnglePosition;
				item9.Transform(Transform.Translation(motion));
				motion.Unitize();
				motion *= nudgeBot[i];
				point3d2.Transform(Transform.Translation(motion));
			}
			List<Point3d> list5 = new List<Point3d> { point3d3, point3d, item9, point3d2 };
			if (focalPoint != Point3d.Unset)
			{
				Point3d point3d4 = focalPoint;
				point3d4.Transform(xDown);
				double num10 = point3d4.X / point3d4.Z;
				double num11 = point3d4.Y / point3d4.Z;
				for (int j = 0; j < list5.Count; j++)
				{
					if (!straightProngToggle[j])
					{
						Point3d value = list5[j];
						value.X = list5[j].X + list5[j].Z * num10;
						value.Y = list5[j].Y + list5[j].Z * num11;
						list5[j] = value;
					}
				}
			}
			Curve curve8 = ProngInterpCurve(list5);
			headProngLines.Add(curve8);
			curve8.ClosestPoint(point3d, out var t2);
			Curve item10 = curve8.Trim(t2, curve8.Domain.Max);
			list4.Add(item10);
		}
		headGirdle = curve2.DuplicateCurve();
		headTop = curve5;
		headBase = curve3;
		outHeadSideX = curve6;
		outHeadSideY = curve7;
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
				CSUtilSweepRC.Sweep2(headGirdle, headBase, list6, list7, list8, out Brep[] breps, false, true);
				if (breps.Length < 1)
				{
					return UpdateError("Error making inbetween surface");
				}
				breps[0].Transform(xUp);
				Brep item11 = breps[0].DuplicateBrep();
				surfaceBetweenProngs.Add(item11);
			}
		}
		return true;
	}

	private static Curve ProngInterpCurve(List<Point3d> listPoints)
	{
		for (int i = 0; i < listPoints.Count - 1; i++)
		{
			if (listPoints[i].DistanceTo(listPoints[i + 1]) < 0.01)
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
			LineCurve lineCurve = new LineCurve(listPoints[0], listPoints[1]);
			list.Add(listPoints[0]);
			list.Add(lineCurve.PointAt(0.1));
			list.Add(listPoints[1]);
		}
		return Curve.CreateInterpolatedCurve(list, 3, CurveKnotStyle.Chord) ?? new LineCurve(listPoints[0], listPoints[listPoints.Count - 1]);
	}

	private double GetDrop(double vBelowInput, CSDataArray vBaseObject, double vBaseOffset, CSGemData gem)
	{
		if (vBaseObject == null)
		{
			return vBelowInput + vBaseOffset;
		}
		CSData arrayItem = vBaseObject.GetArrayItem(0);
		Brep brep = null;
		Curve curve = (arrayItem as ICSCurve)?.GetCurve();
		if (curve != null)
		{
			Curve curve2 = curve.DuplicateCurve();
			Curve curve3 = curve.DuplicateCurve();
			curve2.Translate(Vector3d.YAxis * 50.0);
			curve3.Translate(Vector3d.YAxis * -50.0);
			brep = NurbsSurface.CreateRuledSurface(curve2, curve3).ToBrep();
		}
		if (brep == null && arrayItem is ICSBrep iCSBrep)
		{
			brep = iCSBrep.GetBrep();
		}
		if (brep == null)
		{
			return vBelowInput + vBaseOffset;
		}
		gem.GetGemXformRC(out var _, out var xUp);
		LineCurve lineCurve = new LineCurve(new Point3d(0.0, 0.0, 100.0), new Point3d(0.0, 0.0, -100.0));
		lineCurve.Transform(xUp);
		Point3d point3d = default(Point3d);
		point3d.Transform(xUp);
		Vector3d vector3d = new Vector3d(Vector3d.ZAxis);
		vector3d.Transform(xUp);
		vector3d.Unitize();
		if (!Intersection.CurveBrep(lineCurve, brep, CSApplicationHelper.Application.Tolerance, out var overlapCurves, out var intersectionPoints))
		{
			return vBelowInput + vBaseOffset;
		}
		if (intersectionPoints.Length == 0 && overlapCurves.Length == 0)
		{
			return vBelowInput + vBaseOffset;
		}
		Point3d point3d2 = Point3d.Unset;
		double num = -1.23432101234321E+308;
		Curve[] array = overlapCurves;
		foreach (Curve obj in array)
		{
			obj.ClosestPoint(point3d, out var t);
			Point3d point3d3 = obj.PointAt(t);
			t = point3d3.DistanceTo(point3d);
			if (!RhinoMath.IsValidDouble(num) || num > t)
			{
				point3d2 = point3d3;
				num = t;
			}
		}
		Point3d[] array2 = intersectionPoints;
		for (int i = 0; i < array2.Length; i++)
		{
			Point3d point3d4 = array2[i];
			double num2 = point3d4.DistanceTo(point3d);
			if (!RhinoMath.IsValidDouble(num) || num > num2)
			{
				point3d2 = point3d4;
				num = num2;
			}
		}
		if (vector3d * (point3d - point3d2) < 0.0)
		{
			num *= -1.0;
		}
		return 0.0 - num + vBaseOffset;
	}

	private static bool HeadProng(int prongNumber, CSData iGem, int prongArrangement, int prongtype, IReadOnlyList<Curve> prongLines, Curve iProngRail, Curve iHeadTop, Curve iHeadBase, double prongVTopBlendAmount, double prongVBottomBlendAmount, Curve prongTopProfile, double prongTopX, double prongTopY, double prongVTopAngle, double prongTopNudge, Curve prongBaseProfile, double prongBaseX, double prongBaseY, double prongBaseNudge, double prongVBaseAngle, double domeRadius, double domeRotateX, double domeSettingAngle, int baseStyle, double basePointSize, bool enableDoItYourself, bool planarDome, bool planarVProng, double prongBaseDomeHeight, double overlap, double domeBlendAmountInside, double domeBlendAmountOutside, double domeBlendStartInside, double domeBlendStartOtuside, double domeBlendEndInside, double domeBlendEndOutside, Point3d focalPoint, out List<Brep> topDome, out List<Brep> middleProng, out List<Brep> lowerDome, out List<Brep> prongs, out List<Curve> curves)
	{
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
		Curve curve = prongBaseProfile.DuplicateCurve();
		if (curve == null)
		{
			return false;
		}
		if (!cSGemData.GetGemXformRC(out var _, out var xUp))
		{
			return false;
		}
		if (iHeadTop == null)
		{
			return false;
		}
		Curve curve2 = iHeadTop.DuplicateCurve();
		Curve curve3 = iHeadBase.DuplicateCurve();
		if (curve3 == null)
		{
			return false;
		}
		if (!Curve.DoDirectionsMatch(curve3, curve2))
		{
			curve3.Reverse();
		}
		Curve curve4 = iProngRail.DuplicateCurve();
		if (curve4 == null)
		{
			CS.CSUtil.Print("Missing Prong Rail\n");
			return false;
		}
		curve2.ClosestPoint(curve4.PointAtStart, out var t);
		curve3.ClosestPoint(curve4.PointAtEnd, out var t2);
		double prongBaseOffset = (0.0 - prongBaseY) * prongBaseNudge;
		double prongTopOffset = (0.0 - prongTopY) * prongTopNudge;
		if (baseStyle == 122)
		{
			prongTopOffset = 0.0;
			prongBaseOffset = 0.0;
			Point3d pointAtStart = curve4.PointAtStart;
			Vector3d motion = pointAtStart - Point3d.Origin;
			pointAtStart.Transform(Transform.Translation(motion));
			curve2.ClosestPoint(pointAtStart, out t);
			Point3d pointAtEnd = curve4.PointAtEnd;
			motion = new Vector3d(pointAtEnd.X, pointAtEnd.Y, 0.0);
			pointAtEnd.Transform(Transform.Translation(motion));
			curve3.ClosestPoint(pointAtEnd, out t2);
		}
		if (prongtype == 1)
		{
			if (!MakeVProngs(curve4, curve2, t, prongVTopAngle, prongTopY, prongTopOffset, prongVTopBlendAmount, prongVBottomBlendAmount, curve3, t2, prongVBaseAngle, prongBaseY, prongBaseOffset, (ValueListItem)baseStyle, prongBaseDomeHeight, domeRadius, domeRotateX, enableDoItYourself, planarVProng, out topDome, out middleProng, out lowerDome, out var outCurves, out var outProng, out var _))
			{
				return false;
			}
			foreach (Curve item in outCurves)
			{
				item.Transform(xUp);
				curves.Add(item);
			}
			if (outProng != null)
			{
				outProng.Transform(xUp);
				prongs.Add(outProng);
			}
		}
		else
		{
			if (!MakeProfileSweepProngs(prongNumber, prongLines, curve4, curve2, prongTopProfile, t, prongTopX, prongTopY, prongTopOffset, curve3, curve, t2, prongBaseX, prongBaseY, prongBaseOffset, (ValueListItem)baseStyle, basePointSize, domeRadius, domeRotateX, domeSettingAngle, prongBaseDomeHeight, xUp, enableDoItYourself, planarDome, overlap, prongArrangement, domeBlendAmountInside, domeBlendAmountOutside, domeBlendStartInside, domeBlendStartOtuside, domeBlendEndInside, domeBlendEndOutside, focalPoint, out topDome, out middleProng, out lowerDome, out var outCurves2, out var outProngs, out var _))
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
		curve2.Transform(xUp);
		curves.Add(curve2);
		curve3.Transform(xUp);
		curves.Add(curve3);
		return true;
	}

	private static bool CreateProngExtension(double prongExtensionHeight, ref Curve prongRail, ref Curve prongTopProfile)
	{
		Vector3d vector3d = prongRail.TangentAt(prongRail.Domain.Min);
		vector3d.Unitize();
		Point3d pointAtStart = prongRail.PointAtStart;
		Point3d point3d = pointAtStart - vector3d * prongExtensionHeight;
		Transform xform = Transform.Translation(point3d - pointAtStart);
		prongTopProfile.Transform(xform);
		LineCurve lineCurve = new LineCurve(pointAtStart, point3d);
		Curve curve = Curve.JoinCurves(new Curve[2] { prongRail, lineCurve })[0];
		if ((curve.PointAtStart - point3d).Length > 0.01)
		{
			curve.Reverse();
		}
		prongRail = curve;
		return true;
	}

	private static bool ReduceProngRailLength(ref Curve prongRail, ref Curve prongBaseProfile, double trimLength)
	{
		Curve[] array = prongRail.Split(prongRail.Domain.Max - trimLength);
		prongRail = array[0];
		Curve curve = array[1];
		Vector3d translationVector = new Vector3d(curve.PointAtStart - curve.PointAtEnd);
		prongBaseProfile.Translate(translationVector);
		return true;
	}

	private static bool MakeProfileSweepProngs(int prongIndex, IReadOnlyList<Curve> prongRailArray, Curve prongRail, Curve headTop, Curve inProngProfileTop, double paramTop, double prongTopX, double prongTopY, double prongTopOffset, Curve headBase, Curve inProngProfileBase, double paramBase, double prongBaseX, double prongBaseY, double prongBaseOffset, ValueListItem baseStyle, double basePointSize, double prongDomeHeight, double domeRotateX, double prongSettingAngle, double prongBaseDomeHeight, Transform xUp, bool enableDoItYourself, bool planarDome, double overlap, int prongArrangement, double domeBlendAmountInside, double domeBlendAmountOutside, double domeBlendStartInside, double domeBlendStartOtuside, double domeBlendEndInside, double domeBlendEndOutside, Point3d focalPoint, out List<Brep> topDome, out List<Brep> middleProng, out List<Brep> lowerDome, out List<Curve> outCurves, out List<Brep> outProngs, out string error)
	{
		outCurves = new List<Curve>();
		outProngs = new List<Brep>();
		topDome = new List<Brep>();
		middleProng = new List<Brep>();
		lowerDome = new List<Brep>();
		Point3d pointAtStart = prongRail.PointAtStart;
		Point3d pointAtEnd = prongRail.PointAtEnd;
		Vector3d vector3d = CSBlockUtil.CSUtilMath.AverageTangentAt(headTop, paramTop);
		vector3d.Unitize();
		Vector3d headTopNrm = CSBlockUtil.CSUtilMath.AverageNormalAt(Vector3d.ZAxis, vector3d);
		headTopNrm.Unitize();
		BoundingBox boundingBox = inProngProfileTop.GetBoundingBox(accurate: true);
		double x = boundingBox.Diagonal.X;
		double y = boundingBox.Diagonal.Y;
		double num = CSBlockUtil.CSUtilMath.AngleBetweenVectorsWithDirection(new Vector3d(0.0, 0.0, -1.0), prongRail.TangentAt(prongRail.Domain.Min), vector3d);
		double num2 = 1.0 / Math.Cos(num);
		Transform transform = Transform.Scale(Plane.WorldXY, prongTopX / x, prongTopY / y * num2, 0.0);
		if (!planarDome)
		{
			transform = Transform.Scale(Plane.WorldXY, prongTopX / x, prongTopY / y, 0.0);
		}
		Transform transform2 = Transform.Translation(0.0, prongTopOffset, 0.0);
		Transform transform3 = CSBlockUtil.CSUtilMath.Orient3Point(Point3d.Origin, Vector3d.XAxis, Vector3d.ZAxis, pointAtStart, vector3d, Vector3d.ZAxis);
		Curve prongTopProfile = inProngProfileTop.DuplicateCurve();
		prongTopProfile.Transform(transform3 * transform2 * transform);
		Transform xform = CSBlockUtil.CSUtilMath.Rotation(Vector3d.ZAxis * -1.0, prongRail.TangentAtStart, pointAtStart);
		if (!planarDome)
		{
			prongTopProfile.Transform(xform);
		}
		Vector3d vector3d2;
		if (baseStyle != ValueListItem.HeadBuilderProngHead)
		{
			vector3d2 = vector3d;
		}
		else
		{
			vector3d2 = CSBlockUtil.CSUtilMath.AverageTangentAt(headBase, paramBase);
			vector3d2.Unitize();
		}
		BoundingBox boundingBox2 = inProngProfileBase.GetBoundingBox(accurate: true);
		double x2 = boundingBox2.Diagonal.X;
		double y2 = boundingBox2.Diagonal.Y;
		double d = CSBlockUtil.CSUtilMath.AngleBetweenVectorsWithDirection(new Vector3d(0.0, 0.0, -1.0), prongRail.TangentAt(prongRail.Domain.Max), vector3d2);
		double num3 = 1.0 / Math.Cos(d);
		if (focalPoint != Point3d.Unset)
		{
			num3 = 1.0;
		}
		Transform transform4 = Transform.Scale(Plane.WorldXY, prongBaseX / x2, prongBaseY / y2 * num3, 0.0);
		if (baseStyle == ValueListItem.HeadBuilderProngHeadCutToPoint)
		{
			prongBaseOffset = 0.0 - prongBaseY * num3 + basePointSize;
		}
		Transform transform5 = Transform.Translation(0.0, prongBaseOffset, 0.0);
		Transform transform6 = CSBlockUtil.CSUtilMath.Orient3Point(Point3d.Origin, Vector3d.XAxis, Vector3d.ZAxis, pointAtEnd, (baseStyle != ValueListItem.HeadBuilderProngHead) ? vector3d : vector3d2, Vector3d.ZAxis);
		Curve prongBaseProfile = inProngProfileBase.DuplicateCurve();
		prongBaseProfile.Transform(transform6 * transform5 * transform4);
		Brep dome = null;
		Brep dome2 = null;
		if (prongDomeHeight <= 0.0)
		{
			if (!CreateProngExtension(0.0, ref prongRail, ref prongTopProfile))
			{
				error = "Unable to create top prong cap.";
				return false;
			}
			if (!planarDome)
			{
				prongTopProfile.Transform(xform);
			}
		}
		else
		{
			if (!CreateProngDome(inProngProfileTop, prongTopX, prongTopY, num, prongDomeHeight, domeRotateX, prongSettingAngle, num2, planarDome, domeBlendAmountInside, domeBlendAmountOutside, domeBlendStartInside, domeBlendStartOtuside, domeBlendEndInside, domeBlendEndOutside, prongRail, ref dome))
			{
				error = "Unable to create top dome";
				return false;
			}
			dome.Transform(transform3 * transform2);
		}
		if (prongBaseDomeHeight > 0.0)
		{
			if (!ReduceProngRailLength(ref prongRail, ref prongBaseProfile, prongBaseDomeHeight))
			{
				error = "ReduceProngRailLength failed";
				return false;
			}
			headBase.ClosestPoint(prongRail.PointAtEnd, out paramBase);
			if (baseStyle == ValueListItem.HeadBuilderProngHead)
			{
				Point3d pointAtEnd2 = prongRail.PointAtEnd;
				Vector3d motion = new Vector3d(pointAtEnd2.X, pointAtEnd2.Y, 0.0);
				pointAtEnd2.Transform(Transform.Translation(motion));
				headBase.ClosestPoint(pointAtEnd2, out paramBase);
			}
			vector3d2 = CSBlockUtil.CSUtilMath.AverageTangentAt(headBase, paramBase);
			vector3d2.Unitize();
			d = CSBlockUtil.CSUtilMath.AngleBetweenVectorsWithDirection(new Vector3d(0.0, 0.0, -1.0), prongRail.TangentAt(prongRail.Domain.Max), vector3d2);
			if (!CreateProngDome(inProngProfileBase, prongBaseX, prongBaseY, d, 0.0 - prongBaseDomeHeight, 0.0, 0.0, num3, planarDome, domeBlendAmountInside, domeBlendAmountOutside, domeBlendStartInside, domeBlendStartOtuside, domeBlendEndInside, domeBlendEndOutside, prongRail, ref dome2))
			{
				error = "Unable to create base dome";
				return false;
			}
			pointAtEnd = prongRail.PointAtEnd;
			Transform transform7 = CSBlockUtil.CSUtilMath.Orient3Point(Point3d.Origin, Vector3d.XAxis, Vector3d.ZAxis, pointAtEnd, (baseStyle != ValueListItem.HeadBuilderProngHead) ? vector3d : vector3d2, Vector3d.ZAxis);
			prongBaseProfile = inProngProfileBase.DuplicateCurve();
			prongBaseProfile.Transform(transform7 * transform5 * transform4);
			dome2.Transform(transform7 * transform5);
		}
		GeometryBase[] arrGeometry = new GeometryBase[3] { prongRail, prongTopProfile, prongBaseProfile };
		List<GeometryBase> outputGeometries = new List<GeometryBase>();
		if (!ArrangeGeometry(prongArrangement, xUp, vector3d, headTopNrm, prongTopX, overlap, arrGeometry, null, ref outputGeometries))
		{
			error = "Invalid head style";
			return false;
		}
		foreach (Curve item17 in outputGeometries)
		{
			outCurves.Add(item17);
		}
		if (enableDoItYourself)
		{
			GeometryBase[] arrGeometry2;
			if (dome2 != null)
			{
				arrGeometry2 = new GeometryBase[2] { dome, dome2 };
				List<GeometryBase> outputGeometries2 = new List<GeometryBase>();
				GeometryBase[] arrGeometry3 = new GeometryBase[1] { dome2 };
				if (!ArrangeGeometry(prongArrangement, xUp, vector3d, headTopNrm, prongTopX, overlap, arrGeometry3, null, ref outputGeometries2))
				{
					error = "Invalid head style";
					return false;
				}
				foreach (Brep item18 in outputGeometries2)
				{
					lowerDome.Add(item18);
				}
			}
			else
			{
				arrGeometry2 = new GeometryBase[1] { dome };
			}
			List<GeometryBase> outputGeometries3 = new List<GeometryBase>();
			if (!ArrangeGeometry(prongArrangement, xUp, vector3d, headTopNrm, prongTopX, overlap, arrGeometry2, null, ref outputGeometries3))
			{
				error = "Invalid head style";
				return false;
			}
			foreach (Brep item19 in outputGeometries3)
			{
				outProngs.Add(item19);
			}
			GeometryBase[] arrGeometry4 = new GeometryBase[1] { dome };
			List<GeometryBase> outputGeometries4 = new List<GeometryBase>();
			if (!ArrangeGeometry(prongArrangement, xUp, vector3d, headTopNrm, prongTopX, overlap, arrGeometry4, null, ref outputGeometries4))
			{
				error = "Invalid head style";
				return false;
			}
			foreach (Brep item20 in outputGeometries4)
			{
				topDome.Add(item20);
			}
		}
		else
		{
			Curve[] profiles = new Curve[2] { prongTopProfile, prongBaseProfile };
			if (focalPoint != Point3d.Unset)
			{
				double num4 = Vector3d.VectorAngle(prongTopProfile.CurvatureAt(0.0), prongBaseProfile.CurvatureAt(0.0), Plane.WorldXY);
				Vector3d vector3d3 = prongBaseProfile.PointAtNormalizedLength(0.5) - prongBaseProfile.PointAtStart;
				prongBaseProfile.Transform(Transform.Rotation(0.0 - num4, prongBaseProfile.PointAtStart + vector3d3 / 2.0));
			}
			double[] parameters = new double[2]
			{
				prongRail.Domain.Min,
				prongRail.Domain.Max
			};
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
				GeometryBase[] arrGeometry5 = new GeometryBase[1] { dome };
				List<GeometryBase> outputGeometries5 = new List<GeometryBase>();
				if (!ArrangeGeometry(prongArrangement, xUp, vector3d, headTopNrm, prongTopX, overlap, arrGeometry5, null, ref outputGeometries5))
				{
					error = "Invalid head style";
					return false;
				}
				foreach (Brep item21 in outputGeometries5)
				{
					topDome.Add(item21);
				}
			}
			List<GeometryBase> outputGeometries6 = new List<GeometryBase>();
			GeometryBase[] arrGeometry6 = new GeometryBase[1] { breps[0] };
			if (!ArrangeGeometry(prongArrangement, xUp, vector3d, headTopNrm, prongTopX, overlap, arrGeometry6, null, ref outputGeometries6))
			{
				error = "Invalid head style";
				return false;
			}
			foreach (Brep item22 in outputGeometries6)
			{
				middleProng.Add(item22);
			}
			if (dome2 != null)
			{
				List<GeometryBase> outputGeometries7 = new List<GeometryBase>();
				GeometryBase[] arrGeometry7 = new GeometryBase[1] { dome2 };
				if (!ArrangeGeometry(prongArrangement, xUp, vector3d, headTopNrm, prongTopX, overlap, arrGeometry7, null, ref outputGeometries7))
				{
					error = "Invalid head style";
					return false;
				}
				foreach (Brep item23 in outputGeometries7)
				{
					lowerDome.Add(item23);
				}
			}
			list.AddRange(breps);
			Brep brep = CSUtilRhino.JoinAllBreps(list.ToArray(), 0.01);
			if (brep == null)
			{
				error = "Failed JoinBreps";
				return false;
			}
			Brep brep2;
			if (dome2 == null)
			{
				brep2 = CSUtilRhino.CapPlanarHoles(brep, 0.01);
				if (brep2 == null)
				{
					error = "Failed CapPlanarHoles";
					return false;
				}
			}
			else
			{
				brep2 = brep;
			}
			Brep brep3 = null;
			if (baseStyle == ValueListItem.HeadBuilderProngHeadCutToPoint)
			{
				List<Point3d> points;
				if (prongRailArray.Count > 2)
				{
					Curve curve = prongRailArray[(prongIndex + 1) % prongRailArray.Count];
					if (curve == null)
					{
						error = "Failed to get prongRailCrvP";
						return false;
					}
					Curve curve2 = curve.DuplicateCurve();
					Curve curve3 = prongRailArray[(prongIndex - 1 + prongRailArray.Count) % prongRailArray.Count];
					if (curve3 == null)
					{
						error = "Failed to get prongRailCrvM";
						return false;
					}
					Curve curve4 = curve3.DuplicateCurve();
					Vector3d vector3d4 = new Vector3d(curve2.PointAtStart);
					Vector3d vector3d5 = new Vector3d(prongRail.PointAtStart);
					Vector3d vector3d6 = new Vector3d(curve4.PointAtStart);
					vector3d4.Z = 0.0;
					vector3d5.Z = 0.0;
					vector3d6.Z = 0.0;
					vector3d4.Unitize();
					vector3d5.Unitize();
					vector3d6.Unitize();
					vector3d4 *= 20.0;
					vector3d5 *= 20.0;
					vector3d6 *= 20.0;
					Point3d item8 = new Point3d(vector3d4 + vector3d5);
					Point3d item9 = new Point3d(0.0, 0.0, 0.0);
					Point3d item10 = new Point3d(vector3d6 + vector3d5);
					Point3d item11 = new Point3d(vector3d5 * -2.0);
					item8.Z = 20.0;
					item9.Z = 20.0;
					item10.Z = 20.0;
					item11.Z = 20.0;
					points = new List<Point3d> { item8, item9, item10, item11, item8 };
				}
				else
				{
					Vector3d vector3d7 = new Vector3d(prongRail.PointAtStart);
					vector3d7.Z = 0.0;
					Vector3d vector3d8 = vector3d7;
					vector3d8.Unitize();
					vector3d8 *= 20.0;
					Vector3d vector3d9 = new Vector3d(vector3d8);
					vector3d9.Transform(Transform.Rotation(CSBlockUtil.CSUtilMath.ToRadians(90.0), Point3d.Origin));
					Point3d item12 = new Point3d(vector3d9);
					Point3d item13 = new Point3d(0.0, 0.0, 0.0);
					Point3d item14 = new Point3d(-vector3d9);
					Point3d item15 = new Point3d(vector3d8 * -2.0);
					item12.Z = 20.0;
					item13.Z = 20.0;
					item14.Z = 20.0;
					item15.Z = 20.0;
					points = new List<Point3d> { item12, item13, item14, item15, item12 };
				}
				PolylineCurve curveA = new PolylineCurve(points);
				PolylineCurve polylineCurve = new PolylineCurve(points);
				Transform xform2 = Transform.Translation(0.0, 0.0, -40.0);
				polylineCurve.Transform(xform2);
				brep3 = CSUtilRhino.CapPlanarHoles(NurbsSurface.CreateRuledSurface(curveA, polylineCurve).ToBrep(), 0.001);
				CSUtilGeometry.FixBrepSolidOrientation(ref brep2, doFullClean: true);
				CSUtilGeometry.FixBrepSolidOrientation(ref brep3, doFullClean: true);
			}
			List<GeometryBase> outputGeometries8 = new List<GeometryBase>();
			if (!ArrangeGeometry(prongArrangement, xUp, vector3d, headTopNrm, prongTopX, overlap, new GeometryBase[1] { brep2 }, brep3, ref outputGeometries8))
			{
				error = "Invalid head style";
				return false;
			}
			foreach (Brep item24 in outputGeometries8)
			{
				outProngs.Add(item24);
			}
		}
		error = "";
		return true;
	}

	private static bool CreateProngDome(Curve prongProfile, double prongX, double prongY, double angle, double radius, double rotateX, double prongSettingAngle, double fixScale, bool planarDome, double blendAmountInside, double blendAmountOutside, double blendStartInside, double blendStartOutside, double blendEndInside, double blendEndOutside, Curve prongRail, ref Brep dome)
	{
		BoundingBox boundingBox = prongProfile.GetBoundingBox(accurate: true);
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
		Transform transform = Transform.Scale(Plane.WorldXY, prongX / boundingBox.Diagonal.X, prongY / boundingBox.Diagonal.Y * fixScale, 0.0);
		if (!planarDome && flag)
		{
			transform = Transform.Scale(Plane.WorldXY, prongX / boundingBox.Diagonal.X, prongY / boundingBox.Diagonal.Y, 0.0);
		}
		Transform transform2 = Transform.Translation(0.0, (0.0 - prongY) / 2.0, 0.0);
		Curve curve = prongProfile.DuplicateCurve();
		curve.Transform(transform2 * transform);
		Curve curve2 = prongRail.DuplicateCurve();
		if (flag && !planarDome)
		{
			Vector3d source = new Vector3d(0.0 - prongRail.PointAtStart.X, 0.0 - prongRail.PointAtStart.Y, 0.0);
			curve2.Transform(CSBlockUtil.CSUtilMath.Orient3Point(prongRail.PointAtStart, Vector3d.ZAxis, source, Point3d.Origin, Vector3d.ZAxis, -Vector3d.YAxis));
			Transform xform = CSBlockUtil.CSUtilMath.Rotation(Vector3d.ZAxis * -1.0, curve2.TangentAtStart, curve.PointAtStart);
			curve.Transform(xform);
		}
		LineCurve lineCurve = new LineCurve(new Point3d(0.0, -50.0, 0.0), new Point3d(0.0, 50.0, 0.0));
		NurbsSurface surface = NurbsSurface.CreateFromCorners(new Point3d(0.0, -20.0, -10.0), new Point3d(0.0, 20.0, -10.0), new Point3d(0.0, 20.0, 10.0), new Point3d(0.0, -20.0, 10.0));
		CurveIntersections curveIntersections = Intersection.CurveSurface(curve, surface, 0.01, 0.01);
		if (curveIntersections.Count <= 1)
		{
			return false;
		}
		double parameterA = curveIntersections[0].ParameterA;
		double t = curveIntersections[1].ParameterA;
		if (Math.Abs(parameterA - curve.Domain.Min) > 0.0001 && Math.Abs(parameterA - curve.Domain.Max) > 0.0001)
		{
			Point3d testPoint = curve.PointAt(t);
			curve.ChangeClosedCurveSeam(parameterA);
			curve.ClosestPoint(testPoint, out t);
		}
		Curve[] array = curve.Split(t);
		Curve curve3 = array[0];
		Curve curve4 = array[1];
		if (curve3 == null || curve4 == null)
		{
			return false;
		}
		Vector3d vector3d = lineCurve.TangentAt(lineCurve.Domain.Mid);
		vector3d.Unitize();
		Vector3d vector3d2 = curve3.TangentAt(curve3.Domain.Mid);
		vector3d2.Unitize();
		if (vector3d * vector3d2 < 0.0)
		{
			curve3.Reverse();
		}
		Vector3d vector3d3 = curve4.TangentAt(curve4.Domain.Mid);
		vector3d3.Unitize();
		if (vector3d * vector3d3 < 0.0)
		{
			curve4.Reverse();
		}
		Vector3d vector3d4 = Vector3d.ZAxis;
		if (!flag)
		{
			vector3d4.Z = 0.0 - vector3d4.Z;
			radius = 0.0 - radius;
		}
		if (planarDome || !flag)
		{
			vector3d4.Rotate(0.0 - angle, Vector3d.XAxis);
		}
		vector3d4.Unitize();
		Point3d center = boundingBox.Center;
		center.Transform(transform2 * transform);
		Point3d rotationCenter = new Point3d(center);
		if (flag)
		{
			if (!planarDome)
			{
				center += vector3d4 * (radius / Math.Cos(angle));
			}
			else
			{
				center += Vector3d.ZAxis * radius;
			}
			center.Transform(CSBlockUtil.CSUtilMath.Rotation(CSBlockUtil.CSUtilMath.ToRadians(rotateX), Vector3d.XAxis, rotationCenter));
		}
		else
		{
			center += vector3d4 * (radius / Math.Cos(angle));
			center.Transform(CSBlockUtil.CSUtilMath.Rotation(CSBlockUtil.CSUtilMath.ToRadians(rotateX), Vector3d.XAxis, rotationCenter));
		}
		Vector3d vector3d5 = Vector3d.YAxis * -1.0;
		vector3d5.Rotate(CSBlockUtil.CSUtilMath.ToRadians(prongSettingAngle) + CSBlockUtil.CSUtilMath.ToRadians(rotateX), Vector3d.XAxis);
		vector3d5.Unitize();
		Vector3d vectorEnd = new Vector3d(vector3d5);
		vectorEnd.Reverse();
		if (!planarDome && flag)
		{
			vector3d4 = -curve2.TangentAtStart;
		}
		Curve curve5 = CSUtilCurves.BlendCurve(curve3.PointAtStart, center, vector3d4, vector3d5, blendAmountInside, blendStartInside, blendEndInside);
		Curve curve6 = CSUtilCurves.BlendCurve(curve3.PointAtEnd, center, vector3d4, vectorEnd, blendAmountOutside, blendStartOutside, blendEndOutside);
		Curve[] array2 = Curve.JoinCurves(new Curve[2] { curve5, curve6 });
		if (array2 == null)
		{
			return false;
		}
		if (array2.Length == 0)
		{
			return false;
		}
		NurbsSurface surface2 = NurbsSurface.CreateFromCorners(new Point3d(-20.0, 0.0, -10.0), new Point3d(20.0, 0.0, -10.0), new Point3d(20.0, 0.0, 10.0), new Point3d(-20.0, 0.0, 10.0));
		CurveIntersections curveIntersections2 = Intersection.CurveSurface(curve3, surface2, 0.01, 0.01);
		if (curveIntersections2.Count <= 0)
		{
			return false;
		}
		double parameterA2 = curveIntersections2[0].ParameterA;
		Curve[] array3 = curve3.Split(parameterA2);
		Curve curve7 = array3[0];
		Curve curve8 = array3[1];
		if (curve7 == null || curve8 == null)
		{
			return false;
		}
		curve8.Reverse();
		CurveIntersections curveIntersections3 = Intersection.CurveSurface(curve4, surface2, 0.01, 0.01);
		if (!curveIntersections3.Any())
		{
			return false;
		}
		double parameterA3 = curveIntersections3[0].ParameterA;
		Curve[] array4 = curve4.Split(parameterA3);
		Curve curve9 = array4[0];
		Curve curve10 = array4[1];
		if (curve9 == null || curve10 == null)
		{
			return false;
		}
		curve10.Reverse();
		if (!CSUtilSweepRC.Sweep2(curve7, curve8, array2, new double[1] { curve7.Domain.Min }, new double[1] { curve8.Domain.Min }, out var breps, closed: false, maintainHeight: false))
		{
			return false;
		}
		if (!CSUtilSweepRC.Sweep2(curve9, curve10, array2, new double[1] { curve9.Domain.Min }, new double[1] { curve9.Domain.Min }, out var breps2, closed: false, maintainHeight: false))
		{
			return false;
		}
		Brep brep = CSUtilRhino.JoinAllBreps_BackwardCompatibility(new List<Brep> { breps, breps2 }.ToArray(), CSApplicationHelper.Application.Tolerance);
		if (brep == null)
		{
			return false;
		}
		Transform xform2 = Transform.Translation(0.0, prongY / 2.0, 0.0);
		brep.Transform(xform2);
		dome = brep;
		return true;
	}

	private static GeometryBase CutGeometry(GeometryBase g, Brep cut, Transform T, Transform xUp)
	{
		if (!(g is Brep brep) || cut == null)
		{
			GeometryBase geometryBase = g.Duplicate();
			geometryBase.Transform(T);
			return geometryBase;
		}
		Brep brep2 = brep.DuplicateBrep();
		brep2.Transform(T);
		Brep brep3 = cut.DuplicateBrep();
		brep3.Transform(xUp);
		Brep[] array = Brep.CreateBooleanDifference(brep2, brep3, 0.001);
		if (array == null)
		{
			return brep2.DuplicateBrep();
		}
		return array[0];
	}

	private static bool ArrangeGeometry(int prongArrangement, Transform xUp, Vector3d headTopTan, Vector3d headTopNrm, double prongTopX, double overlap, GeometryBase[] arrGeometry, Brep cutter, ref List<GeometryBase> outputGeometries)
	{
		switch ((CSHeadStyles5.EHeadProngArrangment)prongArrangement)
		{
		case CSHeadStyles5.EHeadProngArrangment.Single:
		{
			GeometryBase[] array = arrGeometry;
			for (int i = 0; i < array.Length; i++)
			{
				GeometryBase item4 = CutGeometry(array[i], cutter, xUp, xUp);
				outputGeometries.Add(item4);
			}
			break;
		}
		case CSHeadStyles5.EHeadProngArrangment.Double:
		{
			double num3 = overlap * prongTopX;
			Transform transform4 = Transform.Translation(headTopTan * 1.0 * num3);
			GeometryBase[] array = arrGeometry;
			for (int i = 0; i < array.Length; i++)
			{
				GeometryBase item5 = CutGeometry(array[i], cutter, xUp * transform4, xUp);
				outputGeometries.Add(item5);
			}
			Transform transform5 = Transform.Translation(headTopTan * -1.0 * num3);
			array = arrGeometry;
			for (int i = 0; i < array.Length; i++)
			{
				GeometryBase item6 = CutGeometry(array[i], cutter, xUp * transform5, xUp);
				outputGeometries.Add(item6);
			}
			break;
		}
		case CSHeadStyles5.EHeadProngArrangment.Triple:
		{
			double num = overlap * prongTopX;
			Transform transform = Transform.Translation(headTopTan * 1.0 * num);
			GeometryBase[] array = arrGeometry;
			for (int i = 0; i < array.Length; i++)
			{
				GeometryBase item = CutGeometry(array[i], cutter, xUp * transform, xUp);
				outputGeometries.Add(item);
			}
			Transform transform2 = Transform.Translation(headTopTan * -1.0 * num);
			array = arrGeometry;
			for (int i = 0; i < array.Length; i++)
			{
				GeometryBase item2 = CutGeometry(array[i], cutter, xUp * transform2, xUp);
				outputGeometries.Add(item2);
			}
			double num2 = overlap * prongTopX * 2.0 * Math.Sqrt(3.0) / 2.0;
			Transform transform3 = Transform.Translation(headTopNrm * num2);
			array = arrGeometry;
			for (int i = 0; i < array.Length; i++)
			{
				GeometryBase item3 = CutGeometry(array[i], cutter, xUp * transform3, xUp);
				outputGeometries.Add(item3);
			}
			break;
		}
		default:
			return false;
		}
		return true;
	}

	private static bool MakeVProngs(Curve prongRail, Curve headTop, double paramTop, double prongVTopAngle, double prongTopY, double prongTopOffset, double prongVTopBlend, double prongVBottomBlend, Curve headBase, double paramBase, double prongVBaseAngle, double prongBaseY, double prongBaseOffset, ValueListItem baseStyle, double prongBaseDomeHeight, double domeRadius, double domeRotateX, bool enableDoItYourself, bool planarVProng, out List<Brep> topDome, out List<Brep> middleProng, out List<Brep> lowerDome, out List<Curve> outCurves, out Brep outProng, out string error)
	{
		outCurves = new List<Curve>();
		outProng = null;
		topDome = new List<Brep>();
		middleProng = new List<Brep>();
		lowerDome = new List<Brep>();
		Point3d pointAtStart = prongRail.PointAtStart;
		Transform xform = CSBlockUtil.CSUtilMath.Rotation(Vector3d.ZAxis * -1.0, prongRail.TangentAtStart, pointAtStart);
		Vector3d prongDir = -prongRail.TangentAt(prongRail.Domain.Min);
		prongDir.Unitize();
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
		if (baseStyle == ValueListItem.HeadBuilderProngHead && prongBaseDomeHeight > 0.0)
		{
			if (!ReduceProngRailLength(ref prongRail, ref vProng2, prongBaseDomeHeight))
			{
				error = "ReduceProngRailLength failed";
				return false;
			}
			Vector3d prongDir2 = prongRail.TangentAt(prongRail.Domain.Max);
			if (!MakeVProngDomes(prongCurves2, prongDir2, prongBaseDomeHeight, 0.0, trimPart, out vDome2))
			{
				error = "Error Making V Prong Domes";
				return false;
			}
		}
		if (!planarVProng)
		{
			vProng.Transform(xform);
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
		vProng.ClosestPoint(prongRail.PointAtStart, out var t);
		vProng.ChangeClosedCurveSeam(t);
		vProng2.ClosestPoint(prongRail.PointAtEnd, out t);
		vProng2.ChangeClosedCurveSeam(t);
		double[] array = new double[2]
		{
			prongRail.Domain.Min,
			prongRail.Domain.Max
		};
		Curve[] profiles = new Curve[2]
		{
			prongCurves[0],
			prongCurves2[0]
		};
		if (!CSUtilSweepRC.Sweep1(prongRail, profiles, array, out var breps, closed: false, globalShapeBlend: true))
		{
			error = "Failed Sweep1";
			return false;
		}
		Brep brep = breps[0];
		Curve[] profiles2 = new Curve[2]
		{
			prongCurves[2],
			prongCurves2[2]
		};
		if (!CSUtilSweepRC.Sweep1(prongRail, profiles2, array, out var breps2, closed: false, globalShapeBlend: true))
		{
			error = "Failed Sweep1";
			return false;
		}
		Brep brep2 = breps2[0];
		Curve[] profile = new Curve[2]
		{
			prongCurves[1],
			prongCurves2[1]
		};
		if (!CSUtilSweepRC.Sweep2(brep.Edges[0], brep2.Edges[0], profile, array, array, out var breps3, closed: false, maintainHeight: true))
		{
			error = "Failed Front Sweep";
			return false;
		}
		Curve[] profile2 = new Curve[2]
		{
			prongCurves2[3],
			prongCurves[3]
		};
		double[] parameters = new double[2]
		{
			brep.Edges[2].Domain.Min,
			brep.Edges[2].Domain.Max
		};
		double[] parameters2 = new double[2]
		{
			brep2.Edges[2].Domain.Min,
			brep2.Edges[2].Domain.Max
		};
		if (!CSUtilSweepRC.Sweep2(brep2.Edges[2], brep.Edges[2], profile2, parameters2, parameters, out var breps4, closed: false, maintainHeight: true))
		{
			error = "Failed Back Sweep";
			return false;
		}
		Brep[] array2 = Brep.JoinBreps(new Brep[4] { breps4, brep, breps3, brep2 }, 0.01);
		if (array2 == null || array2.Length == 0)
		{
			error = "Failed Sweep1: No results";
			return false;
		}
		Brep brep3 = array2[0];
		CSUtilGeometry.FixBrepSolidOrientation(ref brep3);
		Brep brep4 = CSUtilRhino.JoinAllBreps_BackwardCompatibility((vDome2 != null) ? new Brep[3] { brep3, vDome, vDome2 } : new Brep[2] { brep3, vDome }, 0.01);
		Brep brep5;
		if (vDome2 != null)
		{
			brep5 = brep4;
		}
		else
		{
			brep5 = CSUtilRhino.CapPlanarHoles(brep4, 0.01);
			if (brep5 == null)
			{
				error = "Failed CapPlanarHoles";
				return false;
			}
		}
		CSUtilGeometry.FixBrepSolidOrientation(ref brep5);
		outProng = brep5;
		middleProng.Add(brep3);
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
		vProng = null;
		prongCurves = null;
		trimPart = null;
		if (headProfile == null)
		{
			return false;
		}
		Point3d point3d = headProfile.PointAt(param);
		LineCurve lineCurve = new LineCurve(new Point3d(0.0, 0.0, point3d.Z), new Point3d(point3d.X * 2.0, point3d.Y * 2.0, point3d.Z));
		LineCurve lineCurve2 = new LineCurve(new Point3d(0.0, 0.0, point3d.Z), new Point3d(point3d.X * 2.0, point3d.Y * 2.0, point3d.Z));
		Transform xform = CSBlockUtil.CSUtilMath.Rotation(CSBlockUtil.CSUtilMath.ToRadians(vProngAngle), Vector3d.ZAxis, Point3d.Origin);
		lineCurve.Transform(xform);
		xform = CSBlockUtil.CSUtilMath.Rotation(CSBlockUtil.CSUtilMath.ToRadians(0.0 - vProngAngle), Vector3d.ZAxis, Point3d.Origin);
		lineCurve2.Transform(xform);
		CurveIntersections curveIntersections = Intersection.CurveCurve(headProfile, lineCurve, 0.01, 0.01);
		if (curveIntersections.Count != 1)
		{
			return false;
		}
		double parameterA = curveIntersections[0].ParameterA;
		curveIntersections = Intersection.CurveCurve(headProfile, lineCurve2, 0.01, 0.01);
		if (curveIntersections.Count != 1)
		{
			return false;
		}
		double parameterA2 = curveIntersections[0].ParameterA;
		if (parameterA < parameterA2)
		{
			trimPart = headProfile.Trim(new Interval(parameterA, parameterA2));
		}
		else
		{
			Curve curve = headProfile.Trim(new Interval(parameterA, headProfile.Domain.Max));
			Curve curve2 = headProfile.Trim(new Interval(headProfile.Domain.Min, parameterA2));
			Curve[] array = Curve.JoinCurves(new Curve[2] { curve, curve2 }, 0.01);
			trimPart = array[0];
		}
		Curve crv = CSUtilGeometry.Offset(trimPart, Vector3d.ZAxis, new Point3d(0.0, 0.0, point3d.Z), 0.0 - prongNudge, CurveOffsetCornerStyle.Round);
		Curve crv2 = CSUtilGeometry.Offset(trimPart, Vector3d.ZAxis, new Point3d(0.0, 0.0, point3d.Z), 0.0 - (prongY + prongNudge), CurveOffsetCornerStyle.Round);
		if (crv == null || crv2 == null)
		{
			return false;
		}
		crv2.NormalizedLengthParameter(0.5, out var t);
		Point3d testPoint = crv2.PointAt(t);
		Point3d pointAtStart = crv.PointAtStart;
		Point3d pointAtStart2 = crv2.PointAtStart;
		Curve crv3 = new LineCurve(pointAtStart, pointAtStart2);
		Point3d pointAtEnd = crv.PointAtEnd;
		Point3d pointAtEnd2 = crv2.PointAtEnd;
		Curve crv4 = new LineCurve(pointAtEnd, pointAtEnd2);
		double num = blendAmount;
		double num2 = blendAmount;
		double length = crv3.GetLength();
		double length2 = crv.GetLength();
		double length3 = crv2.GetLength();
		if (blendAmount > length * 0.48)
		{
			blendAmount = length * 0.48;
		}
		if (num > length2 * 0.48)
		{
			num = length2 * 0.48;
		}
		if (num2 > length3 * 0.48)
		{
			num2 = length3 * 0.48;
		}
		if (blendAmount > 0.0)
		{
			CSUtilGeometry.TrimStartOfCrv(ref crv, num);
			CSUtilGeometry.TrimStartOfCrv(ref crv2, num2);
			CSUtilGeometry.TrimEndOfCrv(ref crv3, blendAmount);
			CSUtilGeometry.TrimStartOfCrv(ref crv3, blendAmount);
			Curve curve3 = CSUtilCurves.BlendCurve(crv, crv3, doBlendFromStart1: true);
			Curve curve4 = CSUtilCurves.BlendCurve(crv3, crv2);
			CSUtilGeometry.TrimEndOfCrv(ref crv, num);
			CSUtilGeometry.TrimEndOfCrv(ref crv2, num2);
			CSUtilGeometry.TrimEndOfCrv(ref crv4, blendAmount);
			CSUtilGeometry.TrimStartOfCrv(ref crv4, blendAmount);
			Curve curve5 = CSUtilCurves.BlendCurve(crv, crv4);
			Curve curve6 = CSUtilCurves.BlendCurve(crv4, crv2, doBlendFromStart1: false, doBlendFromStart2: false);
			Curve[] array2 = Curve.JoinCurves(new Curve[3] { curve3, crv3, curve4 }, 0.01);
			if (array2.Length != 1)
			{
				return false;
			}
			crv3 = array2[0];
			array2 = Curve.JoinCurves(new Curve[3] { curve5, crv4, curve6 }, 0.01);
			if (array2.Length != 1)
			{
				return false;
			}
			crv4 = array2[0];
		}
		prongCurves = new Curve[4] { crv, crv3, crv2, crv4 };
		Curve[] array3 = Curve.JoinCurves(prongCurves, 0.01);
		if (array3.Length != 1)
		{
			return false;
		}
		Curve curve7 = array3[0];
		curve7.ClosestPoint(testPoint, out var t2);
		curve7.ChangeClosedCurveSeam(t2);
		double min = curve7.Domain.Min;
		Vector3d a = curve7.TangentAt(min);
		Vector3d b = new Vector3d(testPoint.X, testPoint.Y, 0.0);
		if (Vector3d.CrossProduct(a, b).Z < 0.0)
		{
			curve7.Reverse();
		}
		vProng = curve7;
		return true;
	}

	private static bool MakeVProngDomes(IReadOnlyList<Curve> prongCurves, Vector3d prongDir, double prongDomeHeight, double rotateX, Curve trimPart, out Brep vDome)
	{
		Curve curve = prongCurves[0];
		Curve curve2 = prongCurves[1];
		Curve curve3 = prongCurves[2];
		Curve curve4 = prongCurves[3];
		vDome = null;
		double t = trimPart.Domain.Min;
		double max = trimPart.Domain.Max;
		List<double> list = new List<double>();
		List<double> list2 = new List<double>();
		List<Curve> list3 = new List<Curve>();
		Curve curve5 = MakeProngCurve(curve2.PointAtStart, curve2.PointAtEnd, prongDir, prongDomeHeight, rotateX);
		list3.Add(curve5);
		list.Add(curve.Domain.Min);
		list2.Add(curve3.Domain.Min);
		double t2;
		while (trimPart.GetNextDiscontinuity(Continuity.C1_continuous, t, max, out t2))
		{
			Point3d point3d = trimPart.PointAt(t2);
			t = t2;
			double t3 = t2 + 0.001;
			double t4 = t2 - 0.001;
			Vector3d vector3d = trimPart.TangentAt(t3);
			Vector3d vector3d2 = trimPart.TangentAt(t4);
			Vector3d a = vector3d + vector3d2;
			a.Unitize();
			Vector3d vector3d3 = Vector3d.CrossProduct(a, Vector3d.ZAxis);
			LineCurve curveB = new LineCurve(point3d + 50.0 * vector3d3, point3d - 50.0 * vector3d3);
			CurveIntersections curveIntersections = Intersection.CurveCurve(curve, curveB, 0.01, 0.01);
			CurveIntersections curveIntersections2 = Intersection.CurveCurve(curve3, curveB, 0.01, 0.01);
			if (curveIntersections.Count != 0 && curveIntersections2.Count != 0)
			{
				Curve item = MakeProngCurve(curveIntersections[0].PointA, curveIntersections2[0].PointA, prongDir, prongDomeHeight, rotateX);
				list3.Add(item);
				list.Add(curveIntersections[0].ParameterA);
				list2.Add(curveIntersections2[0].ParameterA);
			}
		}
		Curve curve6 = MakeProngCurve(curve4.PointAtStart, curve4.PointAtEnd, prongDir, prongDomeHeight, rotateX);
		list3.Add(curve6);
		list.Add(curve.Domain.Max);
		list2.Add(curve3.Domain.Max);
		Vector3d b = curve5.PointAtEnd - curve5.PointAtStart;
		Vector3d vector3d4 = Vector3d.CrossProduct(curve5.PointAtNormalizedLength(0.5) - curve5.PointAtStart, b);
		if (prongDir.Z < 0.0)
		{
			vector3d4 = -vector3d4;
		}
		Brep brep = LoftDir(curve2, curve5, prongDir, vector3d4).ToBrep();
		b = curve6.PointAtEnd - curve6.PointAtStart;
		vector3d4 = Vector3d.CrossProduct(curve6.PointAtNormalizedLength(0.5) - curve6.PointAtStart, b);
		if (prongDir.Z < 0.0)
		{
			vector3d4 = -vector3d4;
		}
		Brep brep2 = LoftDir(curve4, curve6, prongDir, -vector3d4).ToBrep();
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
		if (!CSUtilSweepRC.Sweep2(curve, curve3, list3.ToArray(), list.ToArray(), list2.ToArray(), out var breps, closed: false, maintainHeight: true))
		{
			return false;
		}
		if (breps == null)
		{
			return false;
		}
		Brep[] inputBreps = new Brep[3] { brep, breps, brep2 };
		vDome = CSUtilRhino.JoinAllBreps_BackwardCompatibility(inputBreps, 0.01);
		if (vDome == null)
		{
			return false;
		}
		return true;
	}

	private static Curve MakeProngCurve(Point3d domeStart0, Point3d domeStart1, Vector3d prongDir, double domeRadius, double rotateX)
	{
		Point3d point3d = (domeStart0 + domeStart1) / 2.0;
		Point3d rotationCenter = new Point3d(point3d);
		point3d += prongDir * domeRadius;
		Vector3d vector3d = domeStart0 - domeStart1;
		vector3d.Unitize();
		Vector3d rotationAxis = Vector3d.CrossProduct(vector3d, Vector3d.ZAxis);
		point3d.Transform(CSBlockUtil.CSUtilMath.Rotation(CSBlockUtil.CSUtilMath.ToRadians(0.0 - rotateX), rotationAxis, rotationCenter));
		Vector3d vectorEnd = new Vector3d(vector3d);
		vectorEnd.Reverse();
		Curve curve = CSUtilCurves.BlendCurve(domeStart0, point3d, prongDir, vector3d);
		Curve curve2 = CSUtilCurves.BlendCurve(domeStart1, point3d, prongDir, vectorEnd);
		return Curve.JoinCurves(new Curve[2] { curve, curve2 })[0].Rebuild(10, 3, preserveTangents: true);
	}

	private static NurbsSurface LoftDir(Curve c0, Curve c1, Vector3d v0, Vector3d v1)
	{
		Brep brep = Brep.CreateFromLoft(new List<Curve> { c0, c1 }, Point3d.Unset, Point3d.Unset, LoftType.Tight, closed: false)[0];
		if (brep.Faces.Count != 1)
		{
			return null;
		}
		NurbsSurface nurbsSurface = brep.Faces[0].ToNurbsSurface();
		if (nurbsSurface.Points.CountU != 4)
		{
			return null;
		}
		int countV = nurbsSurface.Points.CountV;
		for (int i = 0; i < countV; i++)
		{
			Point3d location = nurbsSurface.Points.GetControlPoint(0, i).Location;
			Point3d location2 = nurbsSurface.Points.GetControlPoint(3, i).Location;
			double num = location.DistanceTo(location2);
			location += v0 * num / 3.0;
			location2 += v1 * num / 3.0;
			nurbsSurface.Points.SetControlPoint(1, i, location);
			nurbsSurface.Points.SetControlPoint(2, i, location2);
		}
		return nurbsSurface;
	}

	private bool HeadFinish(CSData inGem, double railMultiplier, List<Brep> iHeadProngObjects, List<Curve> iHeadProngCurves, List<Brep> iHeadRailObjects, List<Curve> iHeadRailCurves, bool diy, bool pullToBaseObject, CSDataArray inBaseObject, out CSDataArray outGem, out CSDataArray outProngs, out CSDataArray outRails, out CSDataArray head, out CSDataArray prongDomes, out CSDataArray displayCurves, out CSDataArray constructionCurves)
	{
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
			prongs.Append(new CSOnGeometryData(iHeadProngObject));
		}
		CSDataArray baseRails = new CSDataArray();
		foreach (Brep iHeadRailObject in iHeadRailObjects)
		{
			baseRails.Append(new CSOnGeometryData(iHeadRailObject));
		}
		CSDataArray prongCurves = new CSDataArray();
		foreach (Curve iHeadProngCurf in iHeadProngCurves)
		{
			prongCurves.Append(new CSOnGeometryData(iHeadProngCurf));
		}
		CSDataArray baseRailCurves = new CSDataArray();
		foreach (Curve iHeadRailCurf in iHeadRailCurves)
		{
			baseRailCurves.Append(new CSOnGeometryData(iHeadRailCurf));
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
		if (baseRails != null)
		{
			cSDataArray2.Append(baseRails.GetFlattenArray().Duplicate());
		}
		if (prongs != null)
		{
			cSDataArray2.Append(prongs.GetFlattenArray().Duplicate());
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
		if (baseObject == null)
		{
			return;
		}
		Brep brep = null;
		double num = double.NaN;
		CSData cSData = baseObject.GetArrayItem(0);
		if (cSData is CSRingRailData cSRingRailData)
		{
			num = cSRingRailData.Radius;
			cSData = null;
		}
		if (cSData != null)
		{
			Curve curve = (cSData as ICSCurve)?.GetCurve();
			if (curve != null)
			{
				Curve curve2 = curve.DuplicateCurve();
				Curve curve3 = curve.DuplicateCurve();
				curve2.Translate(new Vector3d(0.0, 1.0, 0.0) * -50.0);
				curve3.Translate(new Vector3d(0.0, 1.0, 0.0) * 50.0);
				brep = NurbsSurface.CreateRuledSurface(curve2, curve3).ToBrep();
			}
			if (brep == null && cSData is ICSBrep iCSBrep)
			{
				brep = iCSBrep.GetBrep();
			}
			if (brep == null)
			{
				Surface surface = (cSData as ICSSurface)?.GetNurbsSurface();
				if (surface != null)
				{
					brep = surface.ToBrep();
				}
			}
		}
		if ((double.IsNaN(num) && brep == null) || !(gems is CSGemData cSGemData))
		{
			return;
		}
		CSBlockUtil.CSUtilMath.GetUnscaledXform(cSGemData.Xform, out var xUp);
		if (!xUp.TryGetInverse(out var inverseTransform))
		{
			return;
		}
		double angle = double.NaN;
		if (!double.IsNaN(num))
		{
			Point3d origin = Point3d.Origin;
			origin.Transform(xUp);
			Point3d point3d = new Point3d(Vector3d.ZAxis);
			point3d.Transform(xUp);
			angle = CSBlockUtil.CSUtilMath.AngleOfPointD(point3d.X - origin.X, point3d.Z - origin.Z);
			inverseTransform = PullToRailMorph.TopCenterXformFromAngle(angle).Convert();
		}
		BoundingBox boundingBox = BoundingBox.Unset;
		CSDataArray flattenArray = prongs.GetFlattenArray();
		CSDataArray cSDataArray = baseRails;
		flattenArray.GetTightBoundingBoxNew(ref boundingBox, doGrow: true, inverseTransform);
		cSDataArray.GetTightBoundingBoxNew(ref boundingBox, doGrow: true, inverseTransform);
		CSPullToBaseBrep cSPullToBaseBrep = null;
		SpaceMorph spaceMorph;
		if (!double.IsNaN(num))
		{
			spaceMorph = new CSPullToRailMorph(angle, num, railMultiplier, boundingBox);
		}
		else
		{
			cSPullToBaseBrep = new CSPullToBaseBrep(boundingBox, brep, xUp, inverseTransform);
			spaceMorph = cSPullToBaseBrep;
		}
		spaceMorph.Tolerance = 0.05;
		spaceMorph.PreserveStructure = true;
		for (int i = 0; i < flattenArray.GetArrayCount(); i++)
		{
			CSData arrayItem = flattenArray.GetArrayItem(i);
			if (arrayItem == null)
			{
				continue;
			}
			CSOnGeometryData cSOnGeometryData = arrayItem as CSOnGeometryData;
			Brep brep2 = cSOnGeometryData.GetBrep();
			List<Brep> list = new List<Brep>();
			for (int j = 0; j < brep2.Faces.Count; j++)
			{
				Brep brep3 = brep2.Faces[j].ToBrep();
				if (cSPullToBaseBrep != null)
				{
					cSPullToBaseBrep.Count = 0;
				}
				spaceMorph.Morph(brep3);
				list.Add(brep3);
			}
			Brep[] array = Brep.JoinBreps(list, 0.05);
			Brep brep4 = null;
			if (array.Length != 0)
			{
				brep4 = array[0];
				if (!brep4.IsSolid)
				{
					Brep[] array2 = Brep.JoinBreps(list, 0.001);
					if (array2.Length != 0)
					{
						Brep brep5 = array2[0];
						if (brep5.IsSolid)
						{
							brep4 = brep5;
						}
					}
				}
			}
			cSOnGeometryData = new CSOnGeometryData(brep4);
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
				arrayItem2.Morph(spaceMorph);
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
				arrayItem3.Morph(spaceMorph);
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
				arrayItem4.Morph(spaceMorph);
			}
		}
	}

	private Brep BezelBuilder(CSGemData iGem, CSDataArray baseObject, bool pullToBase, Curve prongLine, Curve headTop, Curve headBase, Curve headMiddle, Curve headGirdle, Curve headSideY, double bezelPlacement, double girdleSpacing, double seatAngle, double seatLength, double seatHeight, double domeHeight, double baseHeight, double baseAnglePosition, double prongTopY, double prongTopNudge, double prongBaseY, double prongBaseNudge, double cornerBlend)
	{
		CSGemData cSGemData = iGem.Duplicate() as CSGemData;
		if (cSGemData == null)
		{
			return null;
		}
		iGem.GetGemXformRC(out var xDown, out var xUp);
		cSGemData.ApplyTransform(xDown);
		if (seatHeight < 0.01)
		{
			seatHeight = 0.01;
		}
		Vector3d inputDirection = CSBlockUtil.CSUtilMath.AverageTangentAt(headTop, headTop.PointAtStart);
		double d = CSBlockUtil.CSUtilMath.AngleBetweenVectorsWithDirection(new Vector3d(0.0, 0.0, -1.0), headSideY.TangentAt(headSideY.Domain.Min), inputDirection);
		double num = 1.0 / Math.Cos(d);
		Vector3d inputDirection2 = CSBlockUtil.CSUtilMath.AverageTangentAt(headBase, headBase.PointAtStart);
		double d2 = CSBlockUtil.CSUtilMath.AngleBetweenVectorsWithDirection(new Vector3d(0.0, 0.0, -1.0), headSideY.TangentAt(headSideY.Domain.Min), inputDirection2);
		double num2 = 1.0 / Math.Cos(d2);
		double num3 = prongTopY * num - prongTopNudge;
		double num4 = prongBaseY * num2 - prongBaseNudge;
		BoundingBox boundingBox = headGirdle.GetBoundingBox(accurate: true);
		cSGemData.ScaleToSize(boundingBox.Diagonal.X, boundingBox.Diagonal.Y);
		if (!GemOffsetCurve.GemOffset(cSGemData, num3 + girdleSpacing, out Curve curve, false, out string error, cornerBlend))
		{
			return null;
		}
		Transform xform = Transform.Translation(0.0, 0.0, bezelPlacement);
		curve.Transform(xform);
		headGirdle.Transform(xform);
		Transform xform2 = Transform.Translation(0.0, 0.0, bezelPlacement + seatHeight);
		BoundingBox boundingBox2 = headTop.GetBoundingBox(accurate: true);
		cSGemData.ScaleToSize(boundingBox2.Diagonal.X, boundingBox2.Diagonal.Y);
		if (!GemOffsetCurve.GemOffset(cSGemData, num3 + girdleSpacing, out Curve curve2, false, out error, cornerBlend))
		{
			return null;
		}
		curve2.Transform(xform2);
		if (!GemOffsetCurve.GemOffset(cSGemData, girdleSpacing, out Curve curve3, false, out error, cornerBlend))
		{
			return null;
		}
		curve3.Transform(xform2);
		BoundingBox boundingBox3 = headBase.GetBoundingBox(accurate: true);
		double x = boundingBox3.Diagonal.X;
		double y = boundingBox3.Diagonal.Y;
		cSGemData.ScaleToSize(x, y);
		if (!GemOffsetCurve.GemOffset(cSGemData, num4 + girdleSpacing, out Curve curve4, false, out error, cornerBlend))
		{
			return null;
		}
		Point3d testPoint = cSGemData.GetCurve().PointAt(cSGemData.BezelParam);
		curve2.ClosestPoint(testPoint, out var t);
		Point3d point3d = curve2.PointAt(t);
		Vector3d vector3d = Vector3d.CrossProduct(curve2.TangentAt(t), Vector3d.ZAxis);
		vector3d.Unitize();
		curve3.ClosestPoint(point3d + vector3d, out var t2);
		Point3d other = point3d;
		double num5 = curve3.PointAt(t2).DistanceTo(other) / 2.0;
		if (cSGemData.GemShape == "BaguetteTapered" || cSGemData.GemShape == "Baguette Tapered")
		{
			num5 *= 2.0;
		}
		double num6 = num5 * baseHeight;
		if (baseObject == null || baseObject.GetArrayCount() == 0)
		{
			xform2 = Transform.Translation(0.0, 0.0, prongLine.PointAtEnd.Z + bezelPlacement + num6);
		}
		else
		{
			cSGemData = iGem.Duplicate() as CSGemData;
			double drop = GetDrop(bezelPlacement, baseObject, 0.0, cSGemData);
			xform2 = Transform.Translation(0.0, 0.0, drop + num6);
		}
		curve4.Transform(xform2);
		headMiddle.Transform(Transform.Translation(0.0, 0.0, bezelPlacement));
		Brep brep = BezelOuterSurface(iGem, headMiddle, curve2, curve, curve4, seatHeight, num3, girdleSpacing, num4, baseAnglePosition, cornerBlend);
		if (brep == null)
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
		BoundingBox boundingBox4 = curve4.GetBoundingBox(accurate: true);
		x = boundingBox4.Diagonal.X;
		y = boundingBox4.Diagonal.Y;
		if (cSGemData == null)
		{
			return null;
		}
		cSGemData.ScaleToSize(x, y);
		if (!GemOffsetCurve.GemOffset(cSGemData, 0.0 - num4, out Curve curve5, false, out error, cornerBlend))
		{
			return null;
		}
		xform2 = Transform.Translation(curve4.PointAtStart.X / 2.0, 0.0, curve4.PointAtEnd.Z);
		curve5.Transform(xform2);
		List<Curve> list = new List<Curve> { curve5 };
		Curve curve6 = null;
		cSGemData = iGem.Duplicate() as CSGemData;
		cSGemData?.ApplyTransform(xDown);
		if (seatLength > 0.0)
		{
			double distance = (0.0 - Math.Sin(CSBlockUtil.CSUtilMath.ToRadians(seatAngle))) * seatLength;
			double num7 = (0.0 - Math.Cos(CSBlockUtil.CSUtilMath.ToRadians(seatAngle))) * seatLength;
			if (!GemOffsetCurve.GemOffset(cSGemData, distance, out curve6, false, out error, cornerBlend))
			{
				return null;
			}
			xform = Transform.Translation(0.0, 0.0, num7 + bezelPlacement);
			curve6.Transform(xform);
		}
		if (curve6 != null)
		{
			list.Add(curve6);
		}
		list.Add(headGirdle);
		list.Add(curve3);
		Brep[] array = Brep.CreateFromLoft(list.ToArray(), Point3d.Unset, Point3d.Unset, LoftType.Straight, closed: false);
		if (array.Length < 1)
		{
			return null;
		}
		Brep brep2 = array[0];
		if (brep2 == null)
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
		Brep[] array2 = Brep.JoinBreps(new List<Brep> { brep, srf2, brep2, srf }.ToArray(), CSApplicationHelper.Application.Tolerance);
		if (array2.Length < 1)
		{
			return null;
		}
		CSUtilGeometry.FixBrepSolidOrientation(ref array2[0]);
		Brep inBezel = array2[0];
		inBezel.Transform(xUp);
		if (pullToBase)
		{
			PullToBase(baseObject, iGem, ref inBezel);
		}
		return inBezel;
	}

	private Brep BezelOuterSurface(CSData gem, Curve headMiddle, Curve outerTop, Curve girdleCurve, Curve outerBot, double seatHeight, double topThickness, double girdleSpacing, double bottomThickness, double baseAnglePosition, double cornerBlend)
	{
		BoundingBox boundingBox = headMiddle.GetBoundingBox(accurate: true);
		if (!(gem.Duplicate() is CSGemData cSGemData))
		{
			return null;
		}
		cSGemData.ScaleToSize(boundingBox.Diagonal.X, boundingBox.Diagonal.Y);
		double z = boundingBox.Max.Z;
		Transform xform = Transform.Translation(0.0, 0.0, z + seatHeight);
		if (!GemOffsetCurve.GemOffset(cSGemData, (1.0 - baseAnglePosition) * bottomThickness + baseAnglePosition * topThickness + girdleSpacing, out Curve curve, false, out string _, cornerBlend))
		{
			return null;
		}
		curve.Transform(xform);
		List<Curve> list = new List<Curve>();
		if (boundingBox.Max.Z <= outerBot.PointAtEnd.Z + 0.1 || boundingBox.Min.Z >= girdleCurve.PointAtEnd.Z - 0.6)
		{
			list.Add(outerTop);
			list.Add(girdleCurve);
			list.Add(outerBot);
		}
		else
		{
			list.Add(outerTop);
			list.Add(girdleCurve);
			list.Add(curve);
			list.Add(outerBot);
		}
		if (Math.Abs(outerTop.PointAtEnd.Z - girdleCurve.PointAtEnd.Z) < CSApplicationHelper.Application.Tolerance)
		{
			list.Remove(outerTop);
		}
		Brep[] array = Brep.CreateFromLoft(list, Point3d.Unset, Point3d.Unset, LoftType.Normal, closed: false);
		if (array.Length < 1)
		{
			return null;
		}
		return array[0];
	}

	private static bool BezelSurfaceTop(Curve rail1, Curve rail2, Vector3d startVector, Vector3d endVector, double fillet, CSGemData gemScaled, out Brep srf)
	{
		srf = null;
		if (rail1 == null || rail2 == null)
		{
			return false;
		}
		Point3d testPoint = gemScaled.GetCurve().PointAt(gemScaled.BezelParam);
		rail1.ClosestPoint(testPoint, out var t);
		Point3d point3d = rail1.PointAt(t);
		Vector3d vector3d = Vector3d.CrossProduct(rail1.TangentAt(t), Vector3d.ZAxis);
		vector3d.Unitize();
		rail2.ClosestPoint(point3d + vector3d, out var t2);
		Point3d point3d2 = point3d;
		Point3d point3d3 = rail2.PointAt(t2);
		LineCurve lineCurve = new LineCurve(point3d2, point3d3);
		NurbsCurve nurbsCurve;
		if (Math.Abs(fillet) < CSApplicationHelper.Application.Tolerance)
		{
			nurbsCurve = lineCurve.ToNurbsCurve();
		}
		else
		{
			Point3d point3d4 = lineCurve.PointAt(lineCurve.Domain.Mid);
			double length = lineCurve.GetLength();
			if (double.IsNaN(length) || length < 0.0)
			{
				return false;
			}
			double num = length / 2.0;
			Point3d pointStart = point3d2;
			Point3d pointEnd = point3d4;
			pointEnd.Z += num * fillet;
			Curve item = CSUtilCurves.BlendCurve(pointStart, pointEnd, startVector, -vector3d);
			Curve item2 = CSUtilCurves.BlendCurve(point3d3, pointEnd, endVector, vector3d);
			nurbsCurve = Curve.JoinCurves(new List<Curve> { item, item2 }, CSApplicationHelper.Application.Tolerance)[0].ToNurbsCurve();
		}
		if (!CSUtilSweepRC.Sweep2(rail1, rail2, new Curve[1] { nurbsCurve }, new double[1] { t }, new double[1] { t2 }, out var breps, closed: true, maintainHeight: true))
		{
			CS.CSUtil.PrintLine("Failed Sweep2");
			return false;
		}
		if (breps == null)
		{
			return false;
		}
		if (!breps.IsValid)
		{
			return false;
		}
		srf = breps;
		return true;
	}
}
