using System;
using System.Collections.Generic;
using CS.CSDataUtilities;
using CS.CSObjects.CSBlocks;
using CS.Parametric;
using CS.Parametric.Channels;
using CSBlockUtil;
using CSBlocks.UDT;
using CSCompatibility;
using CSDatas;
using CSDatas.Interfaces;
using GV.CounterSketch.Shared;
using Rhino.Geometry;

namespace CSBlocks.Settings.Heads.CSHeadBuilderV2;

[CSBlock("HeadBuilder", "{7B5187EA-9D40-49CE-A2DC-E995B59401E2}")]
public sealed class CSHeadBuilder : CSBlock
{
	public readonly CSInputChannel InAngleX;

	public readonly CSInputChannel InAngleY;

	public readonly CSInputChannel InBaseAngle;

	public readonly CSInputChannel InBaseAnglePosition;

	public readonly CSInputChannel InBaseObject;

	public readonly CSInputChannel InBaseOffset;

	public readonly CSInputChannel InBasePointRadius;

	public readonly CSInputChannel InBaseStyle;

	public readonly CSInputChannel InBezelSeatHeight;

	public readonly CSInputChannel InBezelSwitch;

	public readonly CSInputChannel InBuildDomeOnPlane;

	public readonly CSInputChannel InBuildVProngPlanar;

	public readonly CSInputChannel InConstructionTransform;

	public readonly CSInputChannel InCornerBlendRails;

	public readonly CSInputChannel InCornerBlendBezel;

	public readonly CSInputChannel InDomeBlendAmountInside;

	public readonly CSInputChannel InDomeBlendAmountOutside;

	public readonly CSInputChannel InDomeBlendEndInside;

	public readonly CSInputChannel InDomeBlendEndOutside;

	public readonly CSInputChannel InDomeBlendStartInside;

	public readonly CSInputChannel InDomeBlendStartOutside;

	public readonly CSInputChannel InEnableDoItYourself;

	public readonly CSInputChannel InFocalPoint;

	public readonly CSInputChannel InGem;

	public readonly CSInputChannel InBezelBlendPercentage;

	public readonly CSInputChannel InHeadHeight;

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

	public readonly CSInputChannel InRailMove;

	public readonly CSInputChannel InRailPulltoRingRail;

	public readonly CSInputChannel InBezelSeatAngle;

	public readonly CSInputChannel InBezelSeatLength;

	public readonly CSInputChannel InStraightProng;

	public readonly CSInputChannel InToggleSrfBetweenProngs;

	public readonly CSInputChannel InTopAngle;

	public readonly CSInputChannel InUpperRailPosition;

	public readonly CSOutputChannel OutBezel;

	public readonly CSOutputChannel OutHeadCurves;

	public readonly CSOutputChannel OutProngCurves;

	public readonly CSOutputChannel OutRailCurves;

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

	public readonly CSOutputChannel OutProngParts;

	public readonly CSOutputChannel OutRails;

	public CSHeadBuilder(string identifier)
		: base(identifier)
	{
		InGem = new CSInputChannel(this, ChannelDataType.Gem, ChannelArrayType.Individual);
		InBaseObject = new CSInputChannel(this, ChannelDataType.Any, ChannelArrayType.Individual, ChannelFlag.NotRequired);
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
		InRailMove = new CSInputChannel(this, ChannelDataType.Boolean, ChannelArrayType.Array, new List<bool>());
		InRailPulltoRingRail = new CSInputChannel(this, ChannelDataType.Boolean, ChannelArrayType.Array, new List<bool>());
		InBezelSeatHeight = new CSInputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual, 0.3);
		InBezelSeatLength = new CSInputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual, 0.3);
		InBezelSeatAngle = new CSInputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual, 45);
		InBezelBlendPercentage = new CSInputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual, 0.5);
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
		OutProngParts = new CSOutputChannel(this, ChannelDataType.Any, ChannelArrayType.Array);
		OutRails = new CSOutputChannel(this, ChannelDataType.Any, ChannelArrayType.Array);
		OutOriginCurves = new CSOutputChannel(this, ChannelDataType.Any, ChannelArrayType.Array);
		OutProngCurves = new CSOutputChannel(this, ChannelDataType.Any, ChannelArrayType.Array);
		OutRailCurves = new CSOutputChannel(this, ChannelDataType.Any, ChannelArrayType.Array);
		OutHeadCurves = new CSOutputChannel(this, ChannelDataType.Any, ChannelArrayType.Array);
		SetChannelIdentifiers(autoCheckInputs: true);
	}

	public override bool Update()
	{
		CSGemData gem = InGem.Gem;
		CSDataArray flattenArray = InBaseObject.GetFlattenArray();
		bool boolean = InEnableDoItYourself.Boolean;
		bool boolean2 = InBezelSwitch.Boolean;
		bool boolean3 = InBuildDomeOnPlane.Boolean;
		_ = InBuildVProngPlanar.Boolean;
		bool boolean4 = InPullToBaseObject.Boolean;
		ValueListItem integer = (ValueListItem)InBaseStyle.Integer;
		double prongHeight = InProngHeight.Double;
		double num = InHeadHeight.Double;
		num = 0.0 - num;
		double baseOffset = InBaseOffset.Double;
		double angleX = InAngleX.Double;
		double angleY = InAngleY.Double;
		double topAngle = InTopAngle.Double;
		double baseAngle = InBaseAngle.Double;
		double baseAnglePosition = InBaseAnglePosition.Double;
		double basePointSize = InBasePointRadius.Double;
		ProngParam[] array = ProngParam.SetProngParams(InProngType, InProngPoint, InProngArrangement, InProngOverlap, InProngPointAngle, InProngTopX, InProngTopY, InProngTopNudge, InProngTopBlend, InProngBaseX, InProngBaseY, InProngBaseNudge, InProngBaseBlend, InProngDomeHeight, InProngDomeSettingDistance, InProngDomeSettingAngle, InProngBaseDomeHeight, InProngTopProfile, InProngBaseProfile, InDomeBlendAmountInside, InDomeBlendAmountOutside, InDomeBlendStartInside, InDomeBlendStartOutside, InDomeBlendEndInside, InDomeBlendEndOutside, InStraightProng);
		RailParam[] array2 = RailParam.SetRailParam(InRailPos, InUpperRailPosition, InRailX, InRailY, InRailOffset, InRailShearMultiplier, InRailGem, InRailProfile, InRailMove, InRailPulltoRingRail);
		double num2 = InCornerBlendRails.Double;
		double num3 = InCornerBlendBezel.Double;
		Point3d point3d = InFocalPoint.Point3d;
		double bezelSeatHeight = InBezelSeatHeight.Double;
		double bezelSeatLength = InBezelSeatLength.Double;
		double bezelSeatAngle = InBezelSeatAngle.Double;
		double bezelBlendPercentage = InBezelBlendPercentage.Double;
		bool boolean5 = InOuterSrfSwitch.Boolean;
		bool boolean6 = InToggleSrfBetweenProngs.Boolean;
		Transform transform = InConstructionTransform.Transform;
		if (!(gem.Duplicate() is CSGemData cSGemData))
		{
			return UpdateError("missing Gem");
		}
		if (transform != Transform.Unset)
		{
			Transform transform2 = Transform.Translation(0.0, 0.0, 0.0 - num);
			Transform transform3 = transform * transform2;
			cSGemData.ApplyTransform(transform3);
		}
		CSDataArray cSDataArray = new CSDataArray();
		double cornerBlend = (boolean2 ? num3 : num2);
		if (!CSHeadShape.HeadShape(cSGemData, array, prongHeight, num, flattenArray, baseOffset, angleX, angleY, topAngle, baseAngle, baseAnglePosition, integer, cornerBlend, point3d, out var headGirdle, out var headTop, out var headBase, out var _, out var outHeadSideX, out var outHeadSideY, out var bezelPointProngRail, out var _, out var error))
		{
			return UpdateError("HeadShape Failed " + error);
		}
		ProngParam[] array3 = array;
		foreach (ProngParam prongParam in array3)
		{
			cSDataArray.Append(new CSOnGeometryData(prongParam.ProngRail));
		}
		cSDataArray.Append(new CSOnGeometryData(headGirdle));
		cSDataArray.Append(new CSOnGeometryData(headTop));
		cSDataArray.Append(new CSOnGeometryData(headBase));
		cSDataArray.Append(new CSOnGeometryData(outHeadSideX));
		cSDataArray.Append(new CSOnGeometryData(outHeadSideY));
		List<Brep> list = new List<Brep>();
		List<Curve> list2 = new List<Curve>();
		CSDataArray cSDataArray2 = new CSDataArray();
		CSDataArray cSDataArray3 = new CSDataArray();
		CSDataArray cSDataArray4 = new CSDataArray();
		CSDataArray cSDataArray5 = new CSDataArray();
		CSDataArray cSDataArray6 = new CSDataArray();
		cSGemData.GetGemXformRC(out var xDown, out var xUp);
		Curve curve = headGirdle.DuplicateCurve();
		Curve curve2 = headTop.DuplicateCurve();
		Curve curve3 = headBase.DuplicateCurve();
		curve.Transform(xUp);
		curve2.Transform(xUp);
		curve3.Transform(xUp);
		list2.Add(curve);
		list2.Add(curve2);
		list2.Add(curve3);
		if (boolean2)
		{
			CSGemData cSGemData2 = cSGemData.Duplicate() as CSGemData;
			cSGemData2.ApplyTransform(xDown);
			if (!CSHeadBezel.BezelProfile(bezelPointProngRail, array[0], 0.0, 0.0, integer, boolean3, bezelSeatHeight, bezelSeatLength, bezelSeatAngle, bezelBlendPercentage, out var bezelProngOutline, out error))
			{
				UpdateError(error);
				return false;
			}
			if (!CSHeadBezel.RevolveBezel(bezelProngOutline, cSGemData2, headBase, num3, out var bezelSurface, out error))
			{
				UpdateError(error);
				return false;
			}
			bezelSurface.Transform(xUp);
			Brep brep = bezelSurface.ToBrep();
			if (brep == null)
			{
				return UpdateError("BezelBuilder failed");
			}
			CSUtilGeometry.FixBrepSolidOrientation(ref brep);
			if (boolean4)
			{
				PullToBase(flattenArray, cSGemData, ref brep);
			}
			OutBezel.SetData(new CSOnGeometryData(brep));
		}
		else
		{
			for (int j = 0; j < array.Length; j++)
			{
				List<Curve> list3 = new List<Curve>();
				CSDataArray tempArrayCurves = new CSDataArray();
				CSDataArray cSDataArray7 = new CSDataArray();
				ProngParam prongParam2 = array[j];
				if (!CSHeadProng.Prong(j, cSGemData, array, headTop, headBase, bezelPointProngRail, integer, basePointSize, boolean, boolean3, point3d, num3, out error))
				{
					return UpdateError(error);
				}
				if (prongParam2.TopDome != null)
				{
					foreach (Brep item in prongParam2.TopDome)
					{
						cSDataArray4.Append(new CSOnGeometryData(item.DuplicateBrep()));
						cSDataArray7.Append(new CSOnGeometryData(item.DuplicateBrep()));
					}
				}
				if (prongParam2.MiddleProng != null)
				{
					foreach (Brep item2 in prongParam2.MiddleProng)
					{
						cSDataArray5.Append(new CSOnGeometryData(item2.DuplicateBrep()));
						cSDataArray7.Append(new CSOnGeometryData(item2.DuplicateBrep()));
					}
				}
				if (prongParam2.LowerDome != null)
				{
					foreach (Brep item3 in prongParam2.LowerDome)
					{
						cSDataArray6.Append(new CSOnGeometryData(item3.DuplicateBrep()));
						cSDataArray7.Append(new CSOnGeometryData(item3.DuplicateBrep()));
					}
				}
				list.AddRange(prongParam2.OutProngs);
				list3.AddRange(prongParam2.OutCurves);
				list3.ForEach(delegate(Curve _)
				{
					tempArrayCurves.Append(new CSOnGeometryData(_));
				});
				cSDataArray2.Append(tempArrayCurves);
				cSDataArray3.Append(cSDataArray7);
			}
			foreach (RailParam railParam in array2)
			{
				if (!CSHeadGalleryRails.GalleryRail(cSGemData, railParam, outHeadSideX, outHeadSideY, boolean, num2, point3d))
				{
					return UpdateError("HeadRail Failed");
				}
			}
			InPullRailMultiplier.GetValue(out double value);
			if (!CSHeadFinish(cSGemData, value, list, list2, cSDataArray2, cSDataArray3, array2, boolean, boolean4, flattenArray, out var outProngs, out var outRails, out var head, out var prongDomes, out var prongParts, out var displayCurves, out var constructionProngCurves, out var constructionHeadCurves, out var constructionRailCurves))
			{
				return UpdateError("HeadFinish Failed");
			}
			CSDataArray cSDataArray8 = new CSDataArray();
			cSDataArray8.Append(outProngs.Duplicate());
			cSDataArray8.Append(outRails.Duplicate());
			OutHeads.SetData(head);
			OutDisplayCurves.SetData(displayCurves);
			OutProngDomes.SetData(prongDomes);
			OutTopDomes.SetData(cSDataArray4);
			OutProngBodies.SetData(cSDataArray5);
			OutLowerDomes.SetData(cSDataArray6);
			OutHeadArray.SetData(cSDataArray8);
			OutProngCurves.SetData(constructionProngCurves);
			OutRailCurves.SetData(constructionRailCurves);
			OutHeadCurves.SetData(constructionHeadCurves);
			OutProngParts.SetData(prongParts);
			OutRails.SetData(outRails);
		}
		OutGem.SetData(cSGemData);
		OutOriginCurves.SetData(cSDataArray);
		if (!boolean6 && !boolean5)
		{
			return base.Update();
		}
		CSHeadBetweenProngSrf.BetweenProngSrf(headBase, bezelPointProngRail, cSGemData, array, integer, boolean3, boolean6, boolean5, out var innerSrf, out var outerSrf);
		CSDataArray cSDataArray9 = new CSDataArray();
		foreach (Brep item4 in innerSrf)
		{
			item4.Transform(xUp);
			cSDataArray9.Append(new CSOnGeometryData(item4));
		}
		OutInsideSrfBetweenProngs.SetData(cSDataArray9);
		CSDataArray cSDataArray10 = new CSDataArray();
		foreach (Brep item5 in outerSrf)
		{
			item5.Transform(xUp);
			cSDataArray10.Append(new CSOnGeometryData(item5));
		}
		OutOuterSrfBeweenProngs.SetData(cSDataArray10);
		return base.Update();
	}

	private static bool CSHeadFinish(CSData inGem, double railMultiplier, List<Brep> iHeadProngObjects, List<Curve> headCurves, CSDataArray prongCurves, CSDataArray inProngParts, RailParam[] rails, bool diy, bool pullToBaseObject, CSDataArray inBaseObject, out CSDataArray outProngs, out CSDataArray outRails, out CSDataArray head, out CSDataArray prongDomes, out CSDataArray prongParts, out CSDataArray displayCurves, out CSDataArray constructionProngCurves, out CSDataArray constructionHeadCurves, out CSDataArray constructionRailCurves)
	{
		head = new CSDataArray();
		prongDomes = new CSDataArray();
		displayCurves = new CSDataArray();
		outProngs = new CSDataArray();
		outRails = new CSDataArray();
		constructionProngCurves = new CSDataArray();
		constructionRailCurves = new CSDataArray();
		constructionHeadCurves = new CSDataArray();
		prongParts = new CSDataArray();
		bool flag = diy;
		if (!(inGem is CSGemData cSGemData))
		{
			return false;
		}
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
		CSDataArray headCurves2 = new CSDataArray();
		foreach (Curve headCurf in headCurves)
		{
			headCurves2.Append(new CSOnGeometryData(headCurf));
		}
		CSDataArray prongCurves2 = prongCurves;
		CSDataArray baseRails = new CSDataArray();
		CSDataArray baseRailCurves = new CSDataArray();
		CSDataArray cSDataArray2 = new CSDataArray();
		CSDataArray cSDataArray3 = new CSDataArray();
		foreach (RailParam railParam in rails)
		{
			if (railParam.RailPullToRingRail)
			{
				foreach (Brep railObject in railParam.RailObjects)
				{
					baseRails.Append(new CSOnGeometryData(railObject.DuplicateBrep()));
				}
				foreach (Curve railCurf in railParam.RailCurves)
				{
					baseRailCurves.Append(new CSOnGeometryData(railCurf.DuplicateCurve()));
				}
				continue;
			}
			foreach (Brep railObject2 in railParam.RailObjects)
			{
				cSDataArray2.Append(new CSOnGeometryData(railObject2));
			}
			foreach (Curve railCurf2 in railParam.RailCurves)
			{
				cSDataArray3.Append(new CSOnGeometryData(railCurf2));
			}
		}
		if (pullToBaseObject)
		{
			PullToBase(railMultiplier, baseObject, cSGemData, cSDataArray2, ref prongs, ref inProngParts, ref baseRails, ref headCurves2, ref prongCurves2, ref baseRailCurves);
		}
		foreach (CSOnGeometryData item in cSDataArray2.Array)
		{
			baseRails.Append(item);
		}
		foreach (CSOnGeometryData item2 in cSDataArray3.Array)
		{
			baseRailCurves.Append(item2);
		}
		cSGemData.GetGemXformRC(out var xDown0, out var _);
		CSDataArray cSDataArray4 = new CSDataArray();
		if (baseRails != null)
		{
			cSDataArray4.Append(baseRails.GetFlattenArray().Duplicate());
		}
		if (prongs != null)
		{
			cSDataArray4.Append(prongs.GetFlattenArray().Duplicate());
		}
		cSGemData.GetGemXformRC(out var _, out var xUpL);
		cSDataArray4.ApplyTransform(xUpL * xDown0);
		cSDataArray.Append(cSDataArray4.Duplicate());
		outProngs = prongs;
		outRails = baseRails;
		prongParts = inProngParts;
		head = null;
		prongDomes = cSDataArray.Duplicate() as CSDataArray;
		CSDataArray cSDataArray5 = new CSDataArray();
		cSDataArray5.Append(prongCurves2);
		cSDataArray5.Append(baseRailCurves);
		cSDataArray5.Append(headCurves2);
		CSDataArray cSDataArray6 = new CSDataArray();
		CSData data3 = cSDataArray5.Duplicate();
		CSDataHelper.Transform(data3, xUpL * xDown0);
		cSDataArray6.Append(data3);
		displayCurves = cSDataArray6;
		prongCurves2.Array.ForEach(delegate(CSData _)
		{
			CSDataHelper.Transform(_, xUpL * xDown0);
		});
		baseRailCurves.Array.ForEach(delegate(CSData _)
		{
			CSDataHelper.Transform(_, xUpL * xDown0);
		});
		headCurves2.Array.ForEach(delegate(CSData _)
		{
			CSDataHelper.Transform(_, xUpL * xDown0);
		});
		constructionProngCurves = prongCurves2;
		constructionRailCurves = baseRailCurves;
		constructionHeadCurves = headCurves2;
		if (!flag)
		{
			head = cSDataArray.GetFlattenArray();
			prongDomes = null;
			displayCurves = null;
			return true;
		}
		return true;
	}

	private static void PullToBase(double railMultiplier, CSDataArray baseObject, CSData gems, CSDataArray railsNotPulled, ref CSDataArray prongs, ref CSDataArray prongParts, ref CSDataArray baseRails, ref CSDataArray headCurves, ref CSDataArray prongCurves, ref CSDataArray baseRailCurves)
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
		if (cSData != null && cSData is ICSCurve iCSCurve)
		{
			Curve curve = iCSCurve.GetCurve();
			if (curve.IsCircle())
			{
				num = curve.GetLength() / (Math.PI * 2.0);
				cSData = null;
			}
		}
		if (cSData != null)
		{
			Curve curve2 = (cSData as ICSCurve)?.GetCurve();
			if (curve2 != null)
			{
				Curve curve3 = curve2.DuplicateCurve();
				Curve curve4 = curve2.DuplicateCurve();
				curve3.Translate(new Vector3d(0.0, 1.0, 0.0) * -50.0);
				curve4.Translate(new Vector3d(0.0, 1.0, 0.0) * 50.0);
				brep = NurbsSurface.CreateRuledSurface(curve3, curve4).ToBrep();
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
		if (railsNotPulled.Array.Count > 0)
		{
			railsNotPulled.GetTightBoundingBoxNew(ref boundingBox, doGrow: true, inverseTransform);
		}
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
			if (flattenArray.GetArrayItem(i) is CSOnGeometryData cSOnGeometryData)
			{
				Brep brep2 = cSOnGeometryData.GetBrep();
				spaceMorph.Morph(brep2);
				CSOnGeometryData value = new CSOnGeometryData(brep2);
				flattenArray.Array[i] = value;
			}
		}
		prongs = flattenArray;
		for (int j = 0; j < prongParts.GetArrayCount(); j++)
		{
			CSData arrayItem = prongParts.GetArrayItem(j);
			if (arrayItem is CSDataArray cSDataArray2)
			{
				for (int k = 0; k < cSDataArray2.GetArrayCount(); k++)
				{
					if (cSDataArray2.GetArrayItem(k) is CSOnGeometryData cSOnGeometryData2)
					{
						Brep brep3 = cSOnGeometryData2.GetBrep();
						spaceMorph.Morph(brep3);
						CSOnGeometryData value2 = new CSOnGeometryData(brep3);
						cSDataArray2.Array[k] = value2;
					}
				}
			}
			if (arrayItem is CSOnGeometryData cSOnGeometryData3)
			{
				Brep brep4 = cSOnGeometryData3.GetBrep();
				spaceMorph.Morph(brep4);
				CSOnGeometryData value3 = new CSOnGeometryData(brep4);
				prongParts.Array[j] = value3;
			}
		}
		for (int l = 0; l < cSDataArray.GetArrayCount(); l++)
		{
			CSData arrayItem2 = cSDataArray.GetArrayItem(l);
			if (arrayItem2 != null)
			{
				if (cSPullToBaseBrep != null)
				{
					cSPullToBaseBrep.Count = 0;
				}
				arrayItem2.Morph(spaceMorph);
			}
		}
		for (int m = 0; m < headCurves.GetArrayCount(); m++)
		{
			CSData arrayItem3 = headCurves.GetArrayItem(m);
			if (arrayItem3 != null)
			{
				if (cSPullToBaseBrep != null)
				{
					cSPullToBaseBrep.Count = 0;
				}
				arrayItem3.Morph(spaceMorph);
			}
		}
		for (int n = 0; n < prongCurves.GetArrayCount(); n++)
		{
			CSData arrayItem4 = prongCurves.GetArrayItem(n);
			if (arrayItem4 != null)
			{
				if (cSPullToBaseBrep != null)
				{
					cSPullToBaseBrep.Count = 0;
				}
				arrayItem4.Morph(spaceMorph);
			}
		}
		for (int num2 = 0; num2 < baseRailCurves.GetArrayCount(); num2++)
		{
			CSData arrayItem5 = baseRailCurves.GetArrayItem(num2);
			if (arrayItem5 != null)
			{
				if (cSPullToBaseBrep != null)
				{
					cSPullToBaseBrep.Count = 0;
				}
				arrayItem5.Morph(spaceMorph);
			}
		}
	}

	private static void PullToBase(CSDataArray baseObject, CSData gems, ref Brep inBezel)
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
			brep2.MakeDeformable();
			spaceMorph.Morph(brep2);
			inBezel = brep2.DuplicateBrep();
		}
	}
}
