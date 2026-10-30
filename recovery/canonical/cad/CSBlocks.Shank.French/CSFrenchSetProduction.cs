using System;
using System.Collections.Generic;
using CS;
using CS.CSObjects.CSBlocks;
using CS.Parametric;
using CS.Parametric.Channels;
using CSBlockUtil;
using CSBlocks.Shank.Helpers;
using CSDatas;
using Rhino.Geometry;
using Rhino.Geometry.Intersect;

namespace CSBlocks.Shank.French;

[CSBlock("FrenchSetProduction", "{19AB7AA6-167B-4E7B-9601-DA9EF3999C02}")]
public sealed class CSFrenchSetProduction : CSBlock
{
	public readonly CSInputChannel InCurve;

	public readonly CSInputChannel InBaseCurve;

	public readonly CSInputChannel InGemDataArray;

	public readonly CSInputChannel InStartProfileLocation;

	public readonly CSInputChannel InEndProfileLocation;

	public readonly CSInputChannel InShankTopProfile;

	public readonly CSInputChannel InChannelCutter;

	public readonly CSInputChannel InGraverCutter;

	public readonly CSInputChannel InTemplateGem;

	public readonly CSInputChannel InScallopCutterProfile;

	public readonly CSInputChannel InGemCutter;

	public readonly CSInputChannel InPilotHoleCutter;

	public readonly CSInputChannel InBooleanToggle;

	public readonly CSInputChannel InProductionShow;

	public readonly CSInputChannel InUnderLyingSurfacesToggle;

	public readonly CSInputChannel InGemClosedLoop;

	public readonly CSInputChannel InUseDivotCutter;

	public readonly CSInputChannel InDivotRadius;

	public readonly CSInputChannel InMetalThicknessStart;

	public readonly CSInputChannel InMetalThicknessMiddleEnabled;

	public readonly CSInputChannel InMetalThicknessMiddle;

	public readonly CSInputChannel InMetalThicknessMiddlePosition;

	public readonly CSInputChannel InMetalThicknessEndEnabled;

	public readonly CSInputChannel InMetalThicknessEnd;

	public readonly CSInputChannel InDivotCutterTranslate;

	public readonly CSOutputChannel OutChannelProfiles;

	public readonly CSOutputChannel OutShankProfiles;

	public readonly CSOutputChannel OutBaseSurface;

	public readonly CSOutputChannel OutChannelLoft;

	public readonly CSOutputChannel OutScallopCutters;

	public readonly CSOutputChannel OutGraverCutters;

	public readonly CSOutputChannel OutFrenchShank;

	public readonly CSOutputChannel OutGemCutters;

	public readonly CSOutputChannel OutPilotHoleCutters;

	public readonly CSOutputChannel OutStartProfiles;

	public readonly CSOutputChannel OutEndProfiles;

	public CSFrenchSetProduction(string identifier)
		: base(identifier)
	{
		InCurve = new CSInputChannel(this, ChannelDataType.Curve, ChannelArrayType.Individual);
		InBaseCurve = new CSInputChannel(this, ChannelDataType.Curve, ChannelArrayType.Individual);
		InGemDataArray = new CSInputChannel(this, ChannelDataType.Any, ChannelArrayType.Array);
		InStartProfileLocation = new CSInputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual);
		InEndProfileLocation = new CSInputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual);
		InShankTopProfile = new CSInputChannel(this, ChannelDataType.Curve, ChannelArrayType.Array);
		InChannelCutter = new CSInputChannel(this, ChannelDataType.Curve, ChannelArrayType.Individual);
		InScallopCutterProfile = new CSInputChannel(this, ChannelDataType.Curve, ChannelArrayType.Individual);
		InGraverCutter = new CSInputChannel(this, ChannelDataType.Brep, ChannelArrayType.Individual);
		InTemplateGem = new CSInputChannel(this, ChannelDataType.Brep, ChannelArrayType.Individual);
		InGemCutter = new CSInputChannel(this, ChannelDataType.Brep, ChannelArrayType.Individual);
		InPilotHoleCutter = new CSInputChannel(this, ChannelDataType.Brep, ChannelArrayType.Individual);
		InBooleanToggle = new CSInputChannel(this, ChannelDataType.Boolean, ChannelArrayType.Individual, defaultValue: true);
		InProductionShow = new CSInputChannel(this, ChannelDataType.Boolean, ChannelArrayType.Individual, defaultValue: true);
		InUnderLyingSurfacesToggle = new CSInputChannel(this, ChannelDataType.Boolean, ChannelArrayType.Individual, defaultValue: false);
		InGemClosedLoop = new CSInputChannel(this, ChannelDataType.Boolean, ChannelArrayType.Individual, defaultValue: false);
		InUseDivotCutter = new CSInputChannel(this, ChannelDataType.Boolean, ChannelArrayType.Individual, defaultValue: false);
		InDivotRadius = new CSInputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual, 0.5);
		InMetalThicknessStart = new CSInputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual, 0);
		InMetalThicknessMiddleEnabled = new CSInputChannel(this, ChannelDataType.Boolean, ChannelArrayType.Individual, 0);
		InMetalThicknessMiddle = new CSInputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual, 0);
		InMetalThicknessMiddlePosition = new CSInputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual, 0);
		InMetalThicknessEndEnabled = new CSInputChannel(this, ChannelDataType.Boolean, ChannelArrayType.Individual, 0);
		InMetalThicknessEnd = new CSInputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual, 0);
		InDivotCutterTranslate = new CSInputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual, 0);
		OutChannelProfiles = new CSOutputChannel(this, ChannelDataType.Curve, ChannelArrayType.Array);
		OutShankProfiles = new CSOutputChannel(this, ChannelDataType.Curve, ChannelArrayType.Array);
		OutBaseSurface = new CSOutputChannel(this, ChannelDataType.Brep, ChannelArrayType.Individual);
		OutChannelLoft = new CSOutputChannel(this, ChannelDataType.Brep, ChannelArrayType.Individual);
		OutScallopCutters = new CSOutputChannel(this, ChannelDataType.Brep, ChannelArrayType.Array);
		OutGraverCutters = new CSOutputChannel(this, ChannelDataType.Brep, ChannelArrayType.Array);
		OutFrenchShank = new CSOutputChannel(this, ChannelDataType.Brep, ChannelArrayType.Individual);
		OutGemCutters = new CSOutputChannel(this, ChannelDataType.Brep, ChannelArrayType.Array);
		OutPilotHoleCutters = new CSOutputChannel(this, ChannelDataType.Brep, ChannelArrayType.Array);
		OutStartProfiles = new CSOutputChannel(this, ChannelDataType.Curve, ChannelArrayType.Array);
		OutEndProfiles = new CSOutputChannel(this, ChannelDataType.Curve, ChannelArrayType.Array);
		SetChannelIdentifiers(autoCheckInputs: true);
	}

	public override bool Update()
	{
		bool flag = InBooleanToggle.Boolean;
		bool boolean = InProductionShow.Boolean;
		bool boolean2 = InGemClosedLoop.Boolean;
		if (!InUnderLyingSurfacesToggle.Boolean && !boolean)
		{
			return base.Update();
		}
		Curve curve = InCurve.Curve;
		Curve curve2 = InBaseCurve.Curve;
		List<UltimateGemData> list = new List<UltimateGemData>();
		if (!(InGemDataArray.Data is CSDataArray cSDataArray))
		{
			return UpdateMissing("Ultimate GemData Array");
		}
		for (int i = 0; i < cSDataArray.GetArrayCount(); i++)
		{
			if (!(cSDataArray.GetArrayItem(i) is CSDataArray gemInfo))
			{
				return UpdateMissing("Ultimate GemData GemArray");
			}
			UltimateGemData item = UltimateGemData.ReadFromArray(gemInfo);
			list.Add(item);
		}
		BoundingBox boundingBox = InTemplateGem.Brep.GetBoundingBox(accurate: true);
		Curve curve3 = InChannelCutter.Curve;
		Curve curve4 = InScallopCutterProfile.Curve;
		Brep brep = InGraverCutter.Brep;
		Brep brep2 = InGemCutter.Brep;
		Brep brep3 = InPilotHoleCutter.Brep;
		bool boolean3 = InUseDivotCutter.Boolean;
		double num = InDivotRadius.Double;
		double num2 = InMetalThicknessStart.Double;
		bool boolean4 = InMetalThicknessMiddleEnabled.Boolean;
		double num3 = InMetalThicknessMiddle.Double;
		double num4 = InMetalThicknessMiddlePosition.Double;
		bool boolean5 = InMetalThicknessEndEnabled.Boolean;
		double num5 = InMetalThicknessEnd.Double;
		if (!boolean4)
		{
			num4 = 0.0;
			num3 = num2;
		}
		if (!boolean5)
		{
			num5 = num2;
		}
		double num6 = InDivotCutterTranslate.Double;
		InShankTopProfile.GetValues(out List<Curve> value);
		Curve curve5 = null;
		Curve curve6 = null;
		List<Curve> list2 = new List<Curve>();
		foreach (Curve item3 in value)
		{
			string text = item3.GetUserString("CSName").ToLower();
			if (text == "top")
			{
				curve5 = item3.DuplicateCurve();
			}
			if (text == "side")
			{
				list2.Add(item3.DuplicateCurve());
			}
			if (text == "bottom")
			{
				curve6 = item3.DuplicateCurve();
			}
		}
		if (curve5 == null || list2.Count != 2 || curve6 == null)
		{
			return UpdateError("Error shank profile Curves");
		}
		double num7 = InStartProfileLocation.Double;
		double num8 = InEndProfileLocation.Double;
		while (num7 > 1.0 || num7 < 0.0)
		{
			if (curve.IsClosed)
			{
				if (num7 > 1.0)
				{
					num7 -= 1.0;
				}
				if (num7 < 0.0)
				{
					num7 += 1.0;
				}
			}
			else
			{
				if (num7 > 1.0)
				{
					num7 = 1.0;
				}
				if (num7 < 0.0)
				{
					num7 = 0.0;
				}
			}
		}
		while (num8 > 1.0 || num8 < 0.0)
		{
			if (curve.IsClosed)
			{
				if (num8 > 1.0)
				{
					num8 -= 1.0;
				}
				if (num8 < 0.0)
				{
					num8 += 1.0;
				}
			}
			else
			{
				if (num8 > 1.0)
				{
					num8 = 1.0;
				}
				if (num8 < 0.0)
				{
					num8 = 0.0;
				}
			}
		}
		double num9 = double.MinValue;
		foreach (UltimateGemData item4 in list)
		{
			if (item4.SizeX > num9)
			{
				num9 = item4.SizeX;
			}
		}
		List<Curve> list3 = new List<Curve>();
		List<Curve> list4 = new List<Curve>();
		List<Brep> list5 = new List<Brep>();
		List<Brep> list6 = new List<Brep>();
		List<Brep> list7 = new List<Brep>();
		List<Curve> list8 = new List<Curve>();
		List<Curve> list9 = new List<Curve>();
		List<Curve> list10 = new List<Curve>();
		List<Curve> list11 = new List<Curve>();
		CSDataArray cSDataArray2 = new CSDataArray();
		CSDataArray cSDataArray3 = new CSDataArray();
		if (!curve.IsClosed && curve2.IsClosed)
		{
			Plane plane = new Plane(curve.PointAtStart, curve.TangentAtStart);
			CurveIntersections curveIntersections = Intersection.CurvePlane(curve2, plane, 0.01);
			if (curveIntersections == null || curveIntersections.Count < 1)
			{
				UpdateError("Failed plane and base Crv Intersection");
			}
			double num10 = double.MaxValue;
			double t = 0.0;
			for (int j = 0; j < curveIntersections.Count; j++)
			{
				if (curveIntersections[j].PointA.DistanceTo(curve.PointAtStart) < num10)
				{
					num10 = curveIntersections[j].PointA.DistanceTo(curve.PointAtStart);
					t = curveIntersections[j].ParameterA;
				}
			}
			curve2.ChangeClosedCurveSeam(t);
		}
		if (!curve2.IsClosed && curve.IsClosed)
		{
			curve.ClosestPoint(curve2.PointAtStart, out var t2);
			curve.ChangeClosedCurveSeam(t2);
		}
		MakeShankProfile(endGem: true, num7, num2, list[0], boundingBox, curve, curve2, curve5, curve6, list2, curve3, out var sweepParam, out var sweepParam2, out var outTopProfile, out var outBotProfile, out var outSide1Profile, out var outSide2Profile, out var outShankProfile, out var _);
		MakeEndGravers(start: true, num7, list[0], list[1], boundingBox, curve, curve2, brep, curve3, out var endGraverBrep, out var endChannelProfile2, out var channelCap);
		MakeEndGravers(start: false, num8, list[list.Count - 1], list[list.Count - 2], boundingBox, curve, curve2, brep, curve3, out var endGraverBrep2, out var endChannelProfile3, out var channelCap2);
		if (!boolean2)
		{
			list3.Add(endChannelProfile2);
			list4.Add(outShankProfile);
			list8.Add(outTopProfile);
			list9.Add(outBotProfile);
			list10.Add(outSide1Profile);
			list11.Add(outSide2Profile);
			list5.Add(endGraverBrep);
			cSDataArray2.Append(new CSOnGeometryData(outShankProfile.DuplicateCurve()));
		}
		MakeShankProfile(endGem: true, num8, num5, list[list.Count - 1], boundingBox, curve, curve2, curve5, curve6, list2, curve3, out var sweepParam3, out var sweepParam4, out var outTopProfile2, out var outBotProfile2, out var outSide1Profile2, out var outSide2Profile2, out var outShankProfile2, out var _);
		List<double> list12 = new List<double>();
		foreach (UltimateGemData item5 in list)
		{
			list12.Add(item5.SizeY);
		}
		bool[] array = CSFourRailSweep.FindMinMax(list12.ToArray());
		List<double> list13 = new List<double>();
		List<double> list14 = new List<double>();
		List<double> list15 = new List<double>();
		if (!boolean2)
		{
			list14.Add(sweepParam);
			list15.Add(sweepParam2);
			list13.Add(list[0].SizeY + num2 * 2.0);
		}
		curve.ClosestPoint(curve.PointAtStart, out var _);
		double num11 = curve.GetLength(new Interval(sweepParam, sweepParam3)) / curve.GetLength();
		double num12 = num4 * num11;
		for (int k = 0; k < list.Count; k++)
		{
			if (array[k])
			{
				Curve curve7 = curve3.DuplicateCurve();
				double xScaleFactor = list[k].SizeX / boundingBox.Diagonal.X;
				double yScaleFactor = list[k].SizeY / boundingBox.Diagonal.Y;
				double zScaleFactor = list[k].SizeZ / boundingBox.Diagonal.Z;
				curve.ClosestPoint(list[k].GemPoint, out var t4);
				Vector3d destination = curve.PointAt(t4) - list[k].GemPoint;
				Transform xform = Transform.Scale(Plane.WorldXY, xScaleFactor, yScaleFactor, zScaleFactor);
				Transform xform2 = CSBlockUtil.CSUtilMath.Orient3Point(Point3d.Origin, Vector3d.ZAxis, Vector3d.XAxis, list[k].GemPoint, destination, curve.TangentAt(t4));
				curve.NormalizedLengthParameter(t4, out var _);
				double normParam = list[0].NormParam;
				double normParam2 = list[list.Count - 1].NormParam;
				t4 = list[k].NormParam - normParam;
				double num14;
				if (t4 < num12)
				{
					double num13 = t4 / num12;
					num14 = num2 + (num3 - num2) * num13;
				}
				else
				{
					t4 -= num12;
					double num15 = t4 / (normParam2 - num12);
					num14 = num3 + (num5 - num3) * num15;
				}
				curve7.Transform(xform);
				curve7.Transform(xform2);
				list3.Add(curve7);
				MakeShankProfile(endGem: false, 0.0, num14, list[k], boundingBox, curve, curve2, curve5, curve6, list2, curve3, out var sweepParam5, out var sweepParam6, out var outTopProfile3, out var outBotProfile3, out var outSide1Profile3, out var outSide2Profile3, out var outShankProfile3, out var _);
				list14.Add(sweepParam5);
				list15.Add(sweepParam6);
				list4.Add(outShankProfile3);
				list13.Add(list[k].SizeY + num14 * 2.0);
				list8.Add(outTopProfile3);
				list9.Add(outBotProfile3);
				list10.Add(outSide1Profile3);
				list11.Add(outSide2Profile3);
			}
		}
		if (!boolean2)
		{
			list8.Add(outTopProfile2);
			list9.Add(outBotProfile2);
			list10.Add(outSide1Profile2);
			list11.Add(outSide2Profile2);
			list14.Add(sweepParam3);
			list15.Add(sweepParam4);
			list13.Add(list[list.Count - 1].SizeY + num5 * 2.0);
			list4.Add(outShankProfile2);
			list3.Add(endChannelProfile3);
			list5.Add(endGraverBrep2);
			cSDataArray3.Append(new CSOnGeometryData(outShankProfile2.DuplicateCurve()));
		}
		Brep breps;
		Brep breps2;
		Brep breps3;
		Brep breps4;
		Brep breps5;
		if (boolean2)
		{
			if (!CSUtilSweepRC.Sweep2(curve, curve2, list8, list14, list15, out breps, true, true))
			{
				return UpdateError("Failed Sweep Top");
			}
			if (!CSUtilSweepRC.Sweep2(curve, curve2, list9, list14, list15, out breps2, true, true))
			{
				return UpdateError("Failed Sweep Top");
			}
			if (!CSUtilSweepRC.Sweep2(curve, curve2, list10, list14, list15, out breps3, true, true))
			{
				return UpdateError("Failed Sweep Top");
			}
			if (!CSUtilSweepRC.Sweep2(curve, curve2, list11, list14, list15, out breps4, true, true))
			{
				return UpdateError("Failed Sweep Top");
			}
			if (!CSUtilSweepRC.Sweep2(curve, curve2, list3, list14, list15, out breps5, true, true))
			{
				return UpdateError("Failed Sweep Channel");
			}
		}
		else
		{
			CSFourRailSweep.Create4Rails(curve, curve2, 0.0, list13, list14, list15, startShankProfileEnabled: false, startShankProfileTangent: false, startChannelProfileTangent: false, endShankProfileEnabled: false, endShankProfileTangent: false, endChannelProfileTangent: false, out var baseCrv, out var baseCrv2, out var gemCrv, out var gemCrv2, out var baseCrv0Param, out var baseCrv1Param, out var gemCrv0Param, out var gemCrv1Param);
			if (!CSUtilSweepRC.Sweep2(gemCrv2, gemCrv, list8, gemCrv1Param, gemCrv0Param, out breps, false, true))
			{
				return UpdateError("Failed Sweep Top");
			}
			if (!CSUtilSweepRC.Sweep2(baseCrv, baseCrv2, list9, baseCrv0Param, baseCrv1Param, out breps2, false, true))
			{
				return UpdateError("Failed Sweep base");
			}
			Curve curveFromBrepEdges = CSUtilBrepCrvs.GetCurveFromBrepEdges(breps, list8[0].PointAtStart, list8[list8.Count - 1].PointAtStart, list8[0].PointAtEnd, isThirdPointMid: false);
			Curve curveFromBrepEdges2 = CSUtilBrepCrvs.GetCurveFromBrepEdges(breps2, list9[0].PointAtEnd, list9[list9.Count - 1].PointAtEnd, list9[0].PointAtStart, isThirdPointMid: false);
			Curve curveFromBrepEdges3 = CSUtilBrepCrvs.GetCurveFromBrepEdges(breps, list8[0].PointAtEnd, list8[list8.Count - 1].PointAtEnd, list8[0].PointAtStart, isThirdPointMid: false);
			Curve curveFromBrepEdges4 = CSUtilBrepCrvs.GetCurveFromBrepEdges(breps2, list9[0].PointAtStart, list9[list9.Count - 1].PointAtStart, list9[0].PointAtEnd, isThirdPointMid: false);
			List<double> list16 = new List<double>();
			List<double> list17 = new List<double>();
			List<double> list18 = new List<double>();
			List<double> list19 = new List<double>();
			for (int l = 0; l < list14.Count; l++)
			{
				list11[l].Reverse();
				curveFromBrepEdges.ClosestPoint(list10[l].PointAtEnd, out var t6);
				list16.Add(t6);
				curveFromBrepEdges2.ClosestPoint(list10[l].PointAtStart, out t6);
				list17.Add(t6);
				curveFromBrepEdges3.ClosestPoint(list11[l].PointAtEnd, out t6);
				list18.Add(t6);
				curveFromBrepEdges4.ClosestPoint(list11[l].PointAtStart, out t6);
				list19.Add(t6);
			}
			if (!CSUtilSweepRC.Sweep2(curveFromBrepEdges2, curveFromBrepEdges, list10, list17, list16, out breps3, false, true))
			{
				return UpdateError("Failed Sweep side1");
			}
			if (!CSUtilSweepRC.Sweep2(curveFromBrepEdges4, curveFromBrepEdges3, list11, list19, list18, out breps4, false, true))
			{
				return UpdateError("Failed Sweep side2");
			}
			gemCrv2.ClosestPoints(list3[0], out var pointOnThisCurve, out var pointOnOtherCurve);
			gemCrv2.ClosestPoint(pointOnThisCurve, out var t7);
			gemCrv1Param[0] = t7;
			gemCrv.ClosestPoints(list3[0], out var pointOnThisCurve2, out var pointOnOtherCurve2);
			gemCrv.ClosestPoint(pointOnThisCurve2, out var t8);
			gemCrv0Param[0] = t8;
			gemCrv2.ClosestPoints(list3[list3.Count - 1], out pointOnThisCurve, out pointOnOtherCurve);
			gemCrv2.ClosestPoint(pointOnThisCurve, out t7);
			gemCrv1Param[gemCrv1Param.Count - 1] = t7;
			gemCrv.ClosestPoints(list3[list3.Count - 1], out pointOnThisCurve2, out pointOnOtherCurve2);
			gemCrv.ClosestPoint(pointOnThisCurve2, out t8);
			gemCrv0Param[gemCrv0Param.Count - 1] = t8;
			if (!CSUtilSweepRC.Sweep2(gemCrv2, gemCrv, list3, gemCrv1Param, gemCrv0Param, out breps5, false, true))
			{
				return UpdateError("Failed Sweep Channel");
			}
			Brep[] array2 = Brep.JoinBreps(new List<Brep> { channelCap, breps5, channelCap2 }, 0.01);
			if (array2.Length != 1)
			{
				return UpdateError("Channel Sweep messed up");
			}
			breps5 = array2[0].DuplicateBrep();
		}
		Brep[] array3 = Brep.JoinBreps(new List<Brep> { breps, breps3, breps2, breps4 }, 0.01);
		if (array3.Length != 1)
		{
			return UpdateError("Failed join of sweeps");
		}
		Brep brep4 = array3[0];
		for (int m = 0; m < list.Count; m++)
		{
			UltimateGemData ultimateGemData = list[m];
			curve.ClosestPoint(ultimateGemData.GemPoint, out var t9);
			Vector3d vector3d = curve.PointAt(t9) - ultimateGemData.GemPoint;
			vector3d.Unitize();
			Brep brep5 = brep2.DuplicateBrep();
			Brep brep6 = brep3.DuplicateBrep();
			double xScaleFactor2 = ultimateGemData.SizeX / boundingBox.Diagonal.X;
			double yScaleFactor2 = ultimateGemData.SizeY / boundingBox.Diagonal.Y;
			double zScaleFactor2 = ultimateGemData.SizeZ / boundingBox.Diagonal.Z;
			Transform xform3 = Transform.Scale(Plane.WorldXY, xScaleFactor2, yScaleFactor2, zScaleFactor2);
			brep5.Transform(xform3);
			brep6.Transform(xform3);
			if (boolean3)
			{
				Curve curve8 = new Circle(Plane.WorldXY, Point3d.Origin, num).ToNurbsCurve();
				Point3d center = curve8.GetBoundingBox(accurate: true).Center;
				Vector3d motion = -Vector3d.ZAxis;
				motion.Unitize();
				motion *= num;
				center.Transform(Transform.Translation(motion));
				NurbsCurve nurbsCurve = CSUtilCurves.BlendCurve(curve8.PointAtStart, center, -Vector3d.ZAxis, Vector3d.XAxis);
				curve8.ClosestPoint(curve8.PointAtStart, out var t10);
				Curve[] profiles = new Curve[1] { nurbsCurve };
				CSUtilSweepRC.Sweep1(curve8, profiles, new double[1] { t10 }, out var breps6, closed: true, globalShapeBlend: false);
				Brep item2 = breps6[0];
				Point3d destination2 = curve.PointAt(t9) - vector3d * ultimateGemData.SizeZ;
				Extrusion extrusion = Extrusion.Create(curve8, ultimateGemData.SizeZ * 2.0, cap: false);
				item2 = Brep.JoinBreps(new List<Brep>
				{
					extrusion.ToBrep(),
					item2
				}, CSApplicationHelper.Application.Tolerance)[0];
				item2 = item2.CapPlanarHoles(0.01);
				if (item2.SolidOrientation == BrepSolidOrientation.Inward)
				{
					item2.Flip();
				}
				Transform xform4 = CSBlockUtil.CSUtilMath.Orient3Point(Point3d.Origin, Vector3d.ZAxis, Vector3d.XAxis, destination2, vector3d, curve.TangentAt(t9));
				item2.Transform(xform4);
				Transform xform5 = Transform.Translation(vector3d * num6);
				item2.Transform(xform5);
				list7.Add(item2);
			}
			Transform xform6 = CSBlockUtil.CSUtilMath.Orient3Point(Point3d.Origin, Vector3d.ZAxis, Vector3d.XAxis, ultimateGemData.GemPoint, vector3d, curve.TangentAt(t9));
			brep5.Transform(xform6);
			list6.Add(brep5);
			if (!boolean3)
			{
				brep6.Transform(xform6);
				list7.Add(brep6);
			}
			if (m < list.Count - 1)
			{
				UltimateGemData ultimateGemData2 = list[m + 1];
				double num16;
				double num17;
				if (ultimateGemData.SizeX < ultimateGemData2.SizeX)
				{
					num16 = ultimateGemData.SizeX / ultimateGemData2.SizeX;
					num17 = 1.0 - num16;
				}
				else if (ultimateGemData.SizeX > ultimateGemData2.SizeX)
				{
					num17 = ultimateGemData2.SizeX / ultimateGemData.SizeX;
					num16 = 1.0 - num17;
				}
				else
				{
					num17 = 1.0;
					num16 = 1.0;
				}
				xScaleFactor2 = (ultimateGemData.SizeX / boundingBox.Diagonal.X + ultimateGemData2.SizeX / boundingBox.Diagonal.X) / 2.0;
				yScaleFactor2 = (ultimateGemData.SizeY / boundingBox.Diagonal.Y + ultimateGemData2.SizeY / boundingBox.Diagonal.Y) / 2.0;
				zScaleFactor2 = (ultimateGemData.SizeZ / boundingBox.Diagonal.Z + ultimateGemData2.SizeZ / boundingBox.Diagonal.Z) / 2.0;
				Brep brep7 = brep.DuplicateBrep();
				xform3 = Transform.Scale(Plane.WorldXY, xScaleFactor2, yScaleFactor2, zScaleFactor2);
				brep7.Transform(xform3);
				curve.ClosestPoint(ultimateGemData.GemPoint, out var t11);
				curve.ClosestPoint(ultimateGemData2.GemPoint, out var t12);
				Curve curve9 = curve.Trim(t11, t12);
				curve9 = curve9.Trim(CurveEnd.Start, ultimateGemData.SizeX / 2.0);
				curve9 = curve9.Trim(CurveEnd.End, ultimateGemData2.SizeX / 2.0);
				curve.ClosestPoint(curve9.PointAtNormalizedLength(0.5), out var t13);
				Vector3d vector3d2 = (ultimateGemData.GemPoint - curve.PointAt(t11) + (ultimateGemData2.GemPoint - curve.PointAt(t12))) / 2.0;
				Vector3d vector3d3 = (curve.PointAt(t9) - ultimateGemData.GemPoint) * num16 + (curve.PointAt(t12) - ultimateGemData2.GemPoint) * num17;
				vector3d3.Unitize();
				Point3d destination3 = curve.PointAt(t13) - vector3d3 * vector3d2.Length;
				Vector3d destination4 = curve.TangentAt(t13);
				xform6 = CSBlockUtil.CSUtilMath.Orient3Point(Point3d.Origin, Vector3d.ZAxis, Vector3d.XAxis, destination3, vector3d3, destination4);
				brep7.Transform(xform6);
				list5.Add(brep7);
			}
			else if (boolean2)
			{
				UltimateGemData ultimateGemData3 = list[0];
				double num18;
				double num19;
				if (ultimateGemData.SizeX < ultimateGemData3.SizeX)
				{
					num18 = ultimateGemData.SizeX / ultimateGemData3.SizeX;
					num19 = 1.0 - num18;
				}
				else if (ultimateGemData.SizeX > ultimateGemData3.SizeX)
				{
					num19 = ultimateGemData3.SizeX / ultimateGemData.SizeX;
					num18 = 1.0 - num19;
				}
				else
				{
					num19 = 1.0;
					num18 = 1.0;
				}
				xScaleFactor2 = (ultimateGemData.SizeX / boundingBox.Diagonal.X + ultimateGemData3.SizeX / boundingBox.Diagonal.X) / 2.0;
				yScaleFactor2 = (ultimateGemData.SizeY / boundingBox.Diagonal.Y + ultimateGemData3.SizeY / boundingBox.Diagonal.Y) / 2.0;
				zScaleFactor2 = (ultimateGemData.SizeZ / boundingBox.Diagonal.Z + ultimateGemData3.SizeZ / boundingBox.Diagonal.Z) / 2.0;
				Brep brep8 = brep.DuplicateBrep();
				xform3 = Transform.Scale(Plane.WorldXY, xScaleFactor2, yScaleFactor2, zScaleFactor2);
				brep8.Transform(xform3);
				curve.ClosestPoint(ultimateGemData.GemPoint, out var t14);
				curve.ClosestPoint(ultimateGemData3.GemPoint, out var t15);
				Curve curve10 = curve.Trim(t14, t15);
				curve10 = curve10.Trim(CurveEnd.Start, ultimateGemData.SizeX / 2.0);
				curve10 = curve10.Trim(CurveEnd.End, ultimateGemData3.SizeX / 2.0);
				curve.ClosestPoint(curve10.PointAtNormalizedLength(0.5), out var t16);
				Vector3d vector3d4 = (ultimateGemData.GemPoint - curve.PointAt(t14) + (ultimateGemData3.GemPoint - curve.PointAt(t15))) / 2.0;
				Vector3d vector3d5 = (curve.PointAt(t9) - ultimateGemData.GemPoint) * num18 + (curve.PointAt(t15) - ultimateGemData3.GemPoint) * num19;
				vector3d5.Unitize();
				Point3d destination5 = curve.PointAt(t16) - vector3d5 * vector3d4.Length;
				Vector3d destination6 = curve.TangentAt(t16);
				xform6 = CSBlockUtil.CSUtilMath.Orient3Point(Point3d.Origin, Vector3d.ZAxis, Vector3d.XAxis, destination5, vector3d5, destination6);
				brep8.Transform(xform6);
				list5.Add(brep8);
			}
		}
		list3.Add(endChannelProfile3);
		Brep brep9;
		if (brep4 != null)
		{
			brep9 = brep4.CapPlanarHoles(0.01);
			if (brep9 == null)
			{
				brep9 = brep4;
			}
			if (brep9.SolidOrientation == BrepSolidOrientation.Inward)
			{
				brep9.Flip();
			}
			if (breps5.SolidOrientation == BrepSolidOrientation.Inward)
			{
				breps5.Flip();
			}
		}
		else
		{
			flag = false;
			brep9 = null;
		}
		MakeScallopCutters(curve, list, boundingBox, curve4, num9 + num2, out var scallopCutters);
		CSDataArray outChannelProfiles = new CSDataArray();
		CSDataArray outTopProfile4 = new CSDataArray();
		CSDataArray outScallopCutters = new CSDataArray();
		CSDataArray outGraverCutters = new CSDataArray();
		CSDataArray outGemCutters = new CSDataArray();
		CSDataArray outPilotHoleCutters = new CSDataArray();
		list3.ForEach(delegate(Curve _)
		{
			outChannelProfiles.Append(new CSOnGeometryData(_));
		});
		list4.ForEach(delegate(Curve _)
		{
			outTopProfile4.Append(new CSOnGeometryData(_));
		});
		list5.ForEach(delegate(Brep _)
		{
			outGraverCutters.Append(new CSOnGeometryData(_));
		});
		scallopCutters.ForEach(delegate(Brep _)
		{
			outScallopCutters.Append(new CSOnGeometryData(_));
		});
		list6.ForEach(delegate(Brep _)
		{
			outGemCutters.Append(new CSOnGeometryData(_));
		});
		list7.ForEach(delegate(Brep _)
		{
			outPilotHoleCutters.Append(new CSOnGeometryData(_));
		});
		if (boolean)
		{
			if (flag)
			{
				Brep[] array4 = Brep.CreateBooleanDifference(brep9, breps5, 0.01);
				if (array4 == null || array4.Length != 1)
				{
					return UpdateError("Failed Channel Intersection");
				}
				Brep[] array5 = Brep.CreateBooleanDifference(array4, scallopCutters, 0.01);
				if (array5 == null || array5.Length != 1)
				{
					return UpdateError("Failed Intersection Scallops");
				}
				Brep[] array6 = Brep.CreateBooleanDifference(array5[0], list5[0], 0.01);
				if (array6 == null || array6.Length != 1)
				{
					return UpdateError("Failed Boolean First Graver ");
				}
				for (int num20 = 1; num20 < list5.Count; num20++)
				{
					array6 = Brep.CreateBooleanDifference(array6[0], list5[num20], 0.01);
					if (array6 == null || array6.Length == 0)
					{
						return UpdateError("Failed Boolean Gravers ");
					}
				}
				array6 = Brep.CreateBooleanDifference(array6, list6, 0.01);
				if (array6 == null || array6.Length != 1)
				{
					return UpdateError("Failed Boolean GemCutters ");
				}
				array6 = Brep.CreateBooleanDifference(array6, list7, 0.01);
				if (array6 == null || array6.Length != 1)
				{
					return UpdateError("Failed Boolean PilotHoles ");
				}
				OutFrenchShank.SetData(new CSOnGeometryData(array6[0]));
			}
			else
			{
				CSDataArray cSDataArray4 = new CSDataArray();
				cSDataArray4.Append(new CSOnGeometryData(brep9));
				cSDataArray4.Append(new CSOnGeometryData(breps5));
				cSDataArray4.Append(outScallopCutters);
				cSDataArray4.Append(outGraverCutters);
				cSDataArray4.Append(outGemCutters);
				cSDataArray4.Append(outPilotHoleCutters);
				OutFrenchShank.SetData(cSDataArray4);
			}
			OutStartProfiles.SetData(cSDataArray2);
			OutEndProfiles.SetData(cSDataArray3);
		}
		OutBaseSurface.SetData(new CSOnGeometryData(brep9));
		OutChannelLoft.SetData(new CSOnGeometryData(breps5));
		OutScallopCutters.SetData(outScallopCutters);
		OutGraverCutters.SetData(outGraverCutters);
		OutGemCutters.SetData(outGemCutters);
		OutChannelProfiles.SetData(outChannelProfiles);
		OutShankProfiles.SetData(outTopProfile4);
		return base.Update();
	}

	private void MakeEndGravers(bool start, double location, UltimateGemData endGem, UltimateGemData nextGem, BoundingBox templateGemBB, Curve crv, Curve baseCrv, Brep graverBrep, Curve channelProfile, out Brep endGraverBrep, out Curve endChannelProfile, out Brep channelCap)
	{
		Brep brep = graverBrep.DuplicateBrep();
		Curve curve = channelProfile.DuplicateCurve();
		double xScaleFactor = endGem.SizeX / templateGemBB.Diagonal.X;
		double yScaleFactor = endGem.SizeY / templateGemBB.Diagonal.Y;
		double zScaleFactor = endGem.SizeZ / templateGemBB.Diagonal.Z;
		crv.ClosestPoint(crv.PointAtNormalizedLength(location), out var _);
		baseCrv.ClosestPoint(crv.PointAtNormalizedLength(location), out var t2);
		crv.ClosestPoint(endGem.GemPoint, out var t3);
		baseCrv.ClosestPoint(crv.PointAt(t3), out var _);
		crv.ClosestPoint(nextGem.GemPoint, out var t5);
		Curve curve2;
		if (start)
		{
			curve2 = crv.Trim(t3, t5);
			curve2 = curve2.Trim(CurveEnd.Start, endGem.SizeX / 2.0);
			curve2 = curve2.Trim(CurveEnd.End, nextGem.SizeX / 2.0);
		}
		else
		{
			curve2 = crv.Trim(t5, t3);
			curve2 = curve2.Trim(CurveEnd.Start, nextGem.SizeX / 2.0);
			curve2 = curve2.Trim(CurveEnd.End, endGem.SizeX / 2.0);
		}
		Point3d testPoint = curve2.PointAtNormalizedLength(0.5);
		crv.ClosestPoint(testPoint, out var t6);
		Point3d testPoint2;
		if (start)
		{
			double length = crv.GetLength(new Interval(t3, t6));
			curve2 = crv.Trim(crv.Domain.Min, t3);
			curve2 = curve2.Trim(CurveEnd.End, length);
			testPoint2 = curve2.PointAtEnd;
		}
		else
		{
			double length2 = crv.GetLength(new Interval(t6, t3));
			curve2 = crv.Trim(t3, crv.Domain.Max);
			curve2 = curve2.Trim(CurveEnd.Start, length2);
			testPoint2 = curve2.PointAtStart;
		}
		crv.ClosestPoint(testPoint2, out var t7);
		Plane plane = new Plane(crv.PointAt(t7), crv.TangentAt(t7));
		CurveIntersections curveIntersections = Intersection.CurvePlane(baseCrv, plane, 0.01);
		if (curveIntersections == null || curveIntersections.Count < 1)
		{
			UpdateError("Failed plane and base Crv Intersection");
		}
		double num = double.MaxValue;
		for (int i = 0; i < curveIntersections.Count; i++)
		{
			if (curveIntersections[i].PointA.DistanceTo(crv.PointAt(t7)) < num)
			{
				num = curveIntersections[i].PointA.DistanceTo(crv.PointAt(t7));
				t2 = curveIntersections[i].ParameterA;
			}
		}
		Vector3d vector3d = crv.PointAt(t7) - baseCrv.PointAt(t2);
		vector3d.Unitize();
		Vector3d vector3d2 = endGem.GemPoint - crv.PointAt(t3);
		Point3d destination = crv.PointAt(t7) - vector3d * vector3d2.Length;
		Vector3d destination2 = vector3d;
		Vector3d destination3 = crv.TangentAt(t7);
		Transform xform = CSBlockUtil.CSUtilMath.Orient3Point(Point3d.Origin, Vector3d.ZAxis, Vector3d.XAxis, destination, destination2, destination3);
		Transform xform2 = Transform.Scale(Plane.WorldXY, xScaleFactor, yScaleFactor, zScaleFactor);
		brep.Transform(xform2);
		brep.Transform(xform);
		curve.Transform(xform2);
		endChannelProfile = curve.DuplicateCurve();
		endChannelProfile.Transform(xform);
		BoundingBox boundingBox = curve.GetBoundingBox(accurate: true);
		Point3d corner = new Point3d(-1.0, 0.0, 5.0);
		Point3d corner2 = new Point3d(1.0, 0.0, 5.0);
		Point3d corner3 = new Point3d(1.0, 0.0, -5.0);
		Point3d corner4 = new Point3d(-1.0, 0.0, -5.0);
		NurbsSurface cutter = NurbsSurface.CreateFromCorners(corner, corner2, corner3, corner4);
		double num2 = 0.0 - boundingBox.Min.Z;
		Transform xform3 = Transform.Translation(0.0, 0.0, num2);
		curve.Transform(xform3);
		NurbsCurve profile = curve.Split(cutter, 0.01)[0].ToNurbsCurve();
		Point3d point3d = new Point3d(0.0, 1.0, 0.0);
		Point3d point3d2 = new Point3d(-1.5, 0.0, 0.0);
		Point3d point3d3 = new Point3d(0.0, -1.0, 0.0);
		Curve curve3 = Curve.CreateInterpolatedCurve(new Point3d[3] { point3d, point3d2, point3d3 }, 3);
		NurbsSurface nurbsSurface = CSUtilRailRevolve.RailRevolveRC(profile, curve3.ToNurbsCurve(), 1.0, 0);
		xform3 = Transform.Translation(0.0, 0.0, 0.0 - num2);
		nurbsSurface.Transform(xform3);
		if (!start)
		{
			nurbsSurface.Transform(Transform.Rotation(CSBlockUtil.CSUtilMath.ToRadians(180.0), Point3d.Origin));
		}
		nurbsSurface.Transform(xform);
		channelCap = nurbsSurface.ToBrep().DuplicateBrep();
		endGraverBrep = brep.DuplicateBrep();
	}

	private void MakeScallopCutters(Curve crv, List<UltimateGemData> gemList, BoundingBox templateGemBB, Curve scallopProfile, double scallopLength, out List<Brep> scallopCutters)
	{
		scallopCutters = new List<Brep>();
		for (int i = 0; i < gemList.Count; i++)
		{
			UltimateGemData ultimateGemData = gemList[i];
			Point3d gemPoint = ultimateGemData.GemPoint;
			crv.ClosestPoint(gemPoint, out var t);
			double xScaleFactor = ultimateGemData.SizeX / templateGemBB.Diagonal.X;
			double yScaleFactor = ultimateGemData.SizeY / templateGemBB.Diagonal.Y;
			double zScaleFactor = ultimateGemData.SizeZ / templateGemBB.Diagonal.Z;
			Transform xform = Transform.Scale(Plane.WorldXY, xScaleFactor, yScaleFactor, zScaleFactor);
			Curve curve = scallopProfile.DuplicateCurve();
			curve.Transform(xform);
			Vector3d vector3d;
			Vector3d vector3d2;
			if (i > 0 && i < gemList.Count - 1)
			{
				Point3d gemPoint2 = gemList[i - 1].GemPoint;
				Point3d gemPoint3 = gemList[i + 1].GemPoint;
				vector3d = gemPoint2 - gemPoint;
				vector3d2 = gemPoint3 - gemPoint;
			}
			else if (i == 0)
			{
				vector3d2 = gemList[i + 1].GemPoint - gemPoint;
				vector3d = -vector3d2;
			}
			else
			{
				vector3d = gemList[i - 1].GemPoint - gemPoint;
				vector3d2 = -vector3d;
			}
			Vector3d vector3d3 = crv.PointAt(t) - ultimateGemData.GemPoint;
			vector3d3.Unitize();
			Transform transformation = CSBlockUtil.CSUtilMath.Orient3Point(gemPoint, vector3d3, vector3d, Point3d.Origin, Vector3d.ZAxis, Vector3d.XAxis);
			vector3d.Transform(transformation);
			vector3d2.Transform(transformation);
			vector3d.Z = 0.0;
			vector3d2.Z = 0.0;
			double num = CSBlockUtil.CSUtilMath.AngleBetweenVectors(vector3d, vector3d2);
			num = Math.PI - num;
			double x = curve.GetBoundingBox(accurate: true).Diagonal.X;
			double num2 = x - Math.Tan(num / 2.0) * scallopLength;
			double num3 = x + Math.Tan(num / 2.0) * scallopLength;
			Curve curve2 = curve.DuplicateCurve();
			Curve curve3 = curve.DuplicateCurve();
			curve2.Transform(Transform.Scale(Plane.WorldXY, num2 / x, 1.0, 1.0));
			curve3.Transform(Transform.Scale(Plane.WorldXY, num3 / x, 1.0, 1.0));
			Transform xform2 = CSBlockUtil.CSUtilMath.Orient3Point(Point3d.Origin, Vector3d.ZAxis, Vector3d.XAxis, gemPoint, vector3d3, crv.TangentAt(t));
			curve2.Transform(xform2);
			curve3.Transform(xform2);
			Vector3d vector3d4 = Vector3d.CrossProduct(crv.TangentAt(t), vector3d3);
			if (vector3d2.Y > 0.0)
			{
				curve2.Transform(Transform.Translation(vector3d4 * scallopLength));
				curve3.Transform(Transform.Translation(vector3d4 * (0.0 - scallopLength)));
			}
			else
			{
				curve2.Transform(Transform.Translation(vector3d4 * (0.0 - scallopLength)));
				curve3.Transform(Transform.Translation(vector3d4 * scallopLength));
			}
			Brep brep = CSUtilSweepRC.Loft(new List<Curve> { curve2, curve3 }, LoftType.Normal, doCapped: true);
			if (brep == null)
			{
				UpdateError("Scallop Loft Error");
			}
			if (brep.SolidOrientation == BrepSolidOrientation.Inward)
			{
				brep.Flip();
			}
			scallopCutters.Add(brep);
		}
	}

	private void MakeShankProfile(bool endGem, double location, double shankWidthAdd, UltimateGemData gem, BoundingBox templateGemBB, Curve crv, Curve baseCrv, Curve shankTopProfile, Curve shankBotProfile, List<Curve> shankSideProfiles, Curve channelProfile, out double sweepParam1, out double sweepParam2, out Curve outTopProfile, out Curve outBotProfile, out Curve outSide1Profile, out Curve outSide2Profile, out Curve outShankProfile, out Curve endChannelProfile)
	{
		Curve curve = shankTopProfile.DuplicateCurve();
		Curve curve2 = shankBotProfile.DuplicateCurve();
		Point3d pointAtStart = curve.PointAtStart;
		Curve curve3;
		Curve curve4;
		if (shankSideProfiles[0].PointAtEnd.DistanceTo(pointAtStart) < shankSideProfiles[1].PointAtEnd.DistanceTo(pointAtStart))
		{
			curve3 = shankSideProfiles[0].DuplicateCurve();
			curve4 = shankSideProfiles[1].DuplicateCurve();
		}
		else
		{
			curve3 = shankSideProfiles[1].DuplicateCurve();
			curve4 = shankSideProfiles[0].DuplicateCurve();
		}
		Curve curve5 = channelProfile.DuplicateCurve();
		double num = shankTopProfile.GetBoundingBox(accurate: true).Diagonal.Y - templateGemBB.Diagonal.Y;
		double xScaleFactor = gem.SizeX / templateGemBB.Diagonal.X;
		double yScaleFactor = (gem.SizeY + shankWidthAdd * 2.0) / (templateGemBB.Diagonal.Y + num);
		double zScaleFactor = gem.SizeZ / templateGemBB.Diagonal.Z;
		Transform xform = Transform.Scale(Plane.WorldXY, xScaleFactor, yScaleFactor, zScaleFactor);
		curve5.Transform(xform);
		curve.Transform(xform);
		curve2.Transform(xform);
		curve3.Transform(xform);
		curve4.Transform(xform);
		crv.ClosestPoint(gem.GemPoint, out var t);
		baseCrv.ClosestPoint(gem.GemPoint, out var t2);
		Point3d point3d = gem.GemPoint;
		Plane plane = new Plane(point3d, crv.TangentAt(t));
		CurveIntersections curveIntersections = Intersection.CurvePlane(baseCrv, plane, 0.01);
		if (curveIntersections == null || curveIntersections.Count < 1)
		{
			UpdateError("Failed plane and base Crv Intersection");
		}
		double num2 = double.MaxValue;
		for (int i = 0; i < curveIntersections.Count; i++)
		{
			if (curveIntersections[i].PointA.DistanceTo(point3d) < num2)
			{
				num2 = curveIntersections[i].PointA.DistanceTo(point3d);
				t2 = curveIntersections[i].ParameterA;
			}
		}
		Vector3d vector3d = crv.PointAt(t) - gem.GemPoint;
		vector3d.Unitize();
		Vector3d inputVector = crv.PointAt(t) - gem.GemPoint;
		inputVector.Unitize();
		if (CSBlockUtil.CSUtilMath.AngleBetweenVectors(vector3d, inputVector) > Math.PI / 2.0)
		{
			vector3d.Reverse();
		}
		if (endGem)
		{
			crv.ClosestPoint(crv.PointAtNormalizedLength(location), out t);
			plane = new Plane(crv.PointAt(t), crv.TangentAt(t));
			curveIntersections = Intersection.CurvePlane(baseCrv, plane, 0.01);
			if (curveIntersections == null || curveIntersections.Count < 1)
			{
				UpdateError("Failed plane and base Crv Intersection");
			}
			num2 = double.MaxValue;
			for (int j = 0; j < curveIntersections.Count; j++)
			{
				if (curveIntersections[j].PointA.DistanceTo(point3d) < num2)
				{
					num2 = curveIntersections[j].PointA.DistanceTo(point3d);
					t2 = curveIntersections[j].ParameterA;
				}
			}
			if (baseCrv.IsClosed && !crv.IsClosed && Math.Abs(t2 - baseCrv.Domain.Max) < 0.001)
			{
				t2 = baseCrv.Domain.Min;
			}
			vector3d = crv.PointAt(t) - baseCrv.PointAt(t2);
			vector3d.Unitize();
			if (CSBlockUtil.CSUtilMath.AngleBetweenVectors(vector3d, inputVector) > Math.PI / 2.0)
			{
				vector3d.Reverse();
			}
			crv.ClosestPoint(gem.GemPoint, out var t3);
			Vector3d vector3d2 = crv.PointAt(t3) - gem.GemPoint;
			point3d = crv.PointAtNormalizedLength(location) - vector3d * vector3d2.Length;
		}
		double num3 = curve.GetBoundingBox(accurate: true).Diagonal.Z + curve2.GetBoundingBox(accurate: true).Diagonal.Z;
		BoundingBox boundingBox = curve2.GetBoundingBox(accurate: true);
		double num4 = baseCrv.PointAt(t2).DistanceTo(crv.PointAt(t));
		if (num3 > num4)
		{
			Transform xform2 = Transform.Scale(Plane.WorldXY, 1.0, 1.0, num4 * 0.75 / num3);
			curve.Transform(xform2);
			curve2.Transform(xform2);
			boundingBox = curve2.GetBoundingBox(accurate: true);
		}
		Transform xform3 = CSBlockUtil.CSUtilMath.Orient3Point(Point3d.Origin, Vector3d.ZAxis, Vector3d.XAxis, point3d, vector3d, crv.TangentAt(t));
		curve5.Transform(xform3);
		Curve curve6 = curve.DuplicateCurve();
		curve6.Transform(xform3);
		Curve curve7 = curve2.DuplicateCurve();
		curve7.Transform(Transform.Translation(0.0, 0.0, 0.0 - boundingBox.Min.Z));
		Point3d destination = crv.PointAt(t) - vector3d * num4;
		Transform xform4 = CSBlockUtil.CSUtilMath.Orient3Point(Point3d.Origin, Vector3d.ZAxis, Vector3d.XAxis, destination, vector3d, crv.TangentAt(t));
		curve7.Transform(xform4);
		double num5 = curve7.PointAtStart.DistanceTo(curve6.PointAtEnd);
		Curve curve8 = curve3.DuplicateCurve();
		BoundingBox boundingBox2 = curve8.GetBoundingBox(accurate: true);
		curve8.Transform(Transform.Scale(Plane.WorldXY, 1.0, 1.0, num5 / boundingBox2.Diagonal.Z));
		Transform xform5 = CSBlockUtil.CSUtilMath.Orient3Point(curve8.PointAtEnd, Vector3d.ZAxis, Vector3d.XAxis, curve6.PointAtStart, vector3d, crv.TangentAt(t));
		curve8.Transform(xform5);
		Curve curve9 = curve4.DuplicateCurve();
		BoundingBox boundingBox3 = curve9.GetBoundingBox(accurate: true);
		curve9.Transform(Transform.Scale(Plane.WorldXY, 1.0, 1.0, num5 / boundingBox3.Diagonal.Z));
		Transform xform6 = CSBlockUtil.CSUtilMath.Orient3Point(curve9.PointAtStart, Vector3d.ZAxis, Vector3d.XAxis, curve6.PointAtEnd, vector3d, crv.TangentAt(t));
		curve9.Transform(xform6);
		outTopProfile = curve6;
		outBotProfile = curve7;
		outSide1Profile = curve8;
		outSide2Profile = curve9;
		CSUtilCurves.MergeCurves((IEnumerable<Curve>)new List<Curve> { curve6, curve8, curve7, curve9 }, out Curve mergedCrv, 0.01);
		endChannelProfile = curve5.DuplicateCurve();
		if (mergedCrv == null)
		{
			UpdateError("Shank Profile Failed To Merge");
		}
		mergedCrv.ClosestPoint(baseCrv.PointAt(t2), out var t4);
		mergedCrv.ChangeClosedCurveSeam(t4);
		outShankProfile = mergedCrv;
		sweepParam1 = t;
		sweepParam2 = t2;
	}
}
