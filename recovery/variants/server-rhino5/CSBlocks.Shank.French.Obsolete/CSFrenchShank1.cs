using System;
using System.Collections.Generic;
using System.Linq;
using CS;
using CS.CSObjects.CSBlocks;
using CS.Parametric;
using CS.Parametric.Channels;
using CSBlocks.Shank.Helpers;
using CSDatas;
using CSDatas.Interfaces;
using Rhino.Geometry;
using Rhino.Geometry.Intersect;

namespace CSBlocks.Shank.French.Obsolete;

[Obsolete]
[CSBlock("FrenchShank", "{4c719d6d-3587-434c-b6a0-2c5159eb30db}")]
public sealed class CSFrenchShank1 : CSBlock
{
	public readonly CSInputChannel InCurve;

	public readonly CSInputChannel InBaseCurve;

	public readonly CSInputChannel InFrenchObj;

	public readonly CSInputChannel InGemDataArray;

	public readonly CSInputChannel InGemClosedLoop;

	public readonly CSInputChannel InShowProduction;

	public readonly CSInputChannel InMetalEdgeThicknessStart;

	public readonly CSInputChannel InMetalEdgeThicknessMidEnabled;

	public readonly CSInputChannel InMetalEdgeThicknessMid;

	public readonly CSInputChannel InMetalEdgeThicknessMidPosition;

	public readonly CSInputChannel InMetalEdgeThicknessEndEnabled;

	public readonly CSInputChannel InMetalEdgeThicknessEnd;

	public readonly CSInputChannel InAverageProfileWidth;

	public readonly CSInputChannel InProfileWidthToggle;

	public readonly CSInputChannel InProfileWidthStart;

	public readonly CSInputChannel InProfileWidthMiddle;

	public readonly CSInputChannel InProfileWidthMiddleToggle;

	public readonly CSInputChannel InProfileWidthEnd;

	public readonly CSInputChannel InProfileWidthEndToggle;

	public readonly CSOutputChannel OutChannel;

	public readonly CSOutputChannel OutEdgeCrvStart;

	public readonly CSOutputChannel OutEdgeCrvEnd;

	public CSFrenchShank1(string identifier)
		: base(identifier)
	{
		InCurve = new CSInputChannel(this, ChannelDataType.Curve, ChannelArrayType.Individual);
		InBaseCurve = new CSInputChannel(this, ChannelDataType.Curve, ChannelArrayType.Individual);
		InFrenchObj = new CSInputChannel(this, ChannelDataType.Brep | ChannelDataType.Mesh, ChannelArrayType.Array);
		InGemDataArray = new CSInputChannel(this, ChannelDataType.Any, ChannelArrayType.Array);
		InGemClosedLoop = new CSInputChannel(this, ChannelDataType.Boolean, ChannelArrayType.Individual, defaultValue: false);
		InShowProduction = new CSInputChannel(this, ChannelDataType.Boolean, ChannelArrayType.Individual);
		InMetalEdgeThicknessStart = new CSInputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual, 0.2);
		InMetalEdgeThicknessMidEnabled = new CSInputChannel(this, ChannelDataType.Boolean, ChannelArrayType.Individual, defaultValue: false);
		InMetalEdgeThicknessMid = new CSInputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual, 0.2);
		InMetalEdgeThicknessMidPosition = new CSInputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual, 0.5);
		InMetalEdgeThicknessEndEnabled = new CSInputChannel(this, ChannelDataType.Boolean, ChannelArrayType.Individual, defaultValue: false);
		InMetalEdgeThicknessEnd = new CSInputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual, 0.2);
		InAverageProfileWidth = new CSInputChannel(this, ChannelDataType.Boolean, ChannelArrayType.Individual, defaultValue: false);
		InProfileWidthToggle = new CSInputChannel(this, ChannelDataType.Boolean, ChannelArrayType.Individual, defaultValue: false);
		InProfileWidthStart = new CSInputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual, 3.0);
		InProfileWidthMiddle = new CSInputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual, 3.0);
		InProfileWidthMiddleToggle = new CSInputChannel(this, ChannelDataType.Boolean, ChannelArrayType.Individual, defaultValue: false);
		InProfileWidthEnd = new CSInputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual, 3.0);
		InProfileWidthEndToggle = new CSInputChannel(this, ChannelDataType.Boolean, ChannelArrayType.Individual, defaultValue: false);
		OutChannel = new CSOutputChannel(this, ChannelDataType.Mesh, ChannelArrayType.Array);
		OutEdgeCrvStart = new CSOutputChannel(this, ChannelDataType.Curve, ChannelArrayType.Individual);
		OutEdgeCrvEnd = new CSOutputChannel(this, ChannelDataType.Curve, ChannelArrayType.Individual);
		SetChannelIdentifiers();
	}

	public override bool Update()
	{
		if (!InCurve.GetValue(out Curve value))
		{
			return InputError();
		}
		if (!InBaseCurve.GetValue(out Curve value2))
		{
			return InputError();
		}
		bool boolean = InGemClosedLoop.Boolean;
		bool isCamMode = CSApplicationHelper.Application.IsCamMode;
		if (InShowProduction.Boolean)
		{
			return true;
		}
		double num = InMetalEdgeThicknessStart.Double;
		bool boolean2 = InMetalEdgeThicknessMidEnabled.Boolean;
		double num2 = InMetalEdgeThicknessMid.Double;
		double num3 = InMetalEdgeThicknessMidPosition.Double;
		bool boolean3 = InMetalEdgeThicknessEndEnabled.Boolean;
		double num4 = InMetalEdgeThicknessEnd.Double;
		if (!boolean2)
		{
			num3 = 0.0;
			num2 = num;
		}
		if (!boolean3)
		{
			num4 = num;
		}
		CSFrenchGeometry cSFrenchGeometry = null;
		CSFrenchGeometry cSFrenchGeometry2 = null;
		CSFrenchGeometry cSFrenchGeometry3 = null;
		bool isMesh = true;
		List<Brep> list = new List<Brep>();
		List<Mesh> list2 = new List<Mesh>();
		List<Curve> list3 = new List<Curve>();
		if (!(InFrenchObj.Data is ICSArray iCSArray))
		{
			return UpdateMissing("French Geometry missing");
		}
		CSDataArray flattenArray = iCSArray.GetFlattenArray();
		int count = flattenArray.Array.Count;
		for (int i = 0; i < count; i++)
		{
			Brep brep = (flattenArray.Array[i] as ICSBrep)?.GetBrep();
			if (brep != null)
			{
				list.Add(brep);
				continue;
			}
			Mesh mesh = (flattenArray.Array[i] as ICSMesh)?.GetMesh();
			if (mesh != null)
			{
				list2.Add(mesh);
				continue;
			}
			Curve curve = (flattenArray.Array[i] as ICSCurve)?.GetCurve();
			if (curve != null)
			{
				list3.Add(curve);
			}
		}
		if (list.Count > 0)
		{
			if (!boolean)
			{
				cSFrenchGeometry = new CSFrenchGeometry(list, list3, "s", isMesh: false, isCamMode);
			}
			cSFrenchGeometry2 = new CSFrenchGeometry(list, null, "c", isMesh: false, isCamMode);
			if (!boolean)
			{
				cSFrenchGeometry3 = new CSFrenchGeometry(list, list3, "e", isMesh: false, isCamMode);
			}
			isMesh = false;
		}
		else if (list2.Count > 0)
		{
			if (!boolean)
			{
				cSFrenchGeometry = new CSFrenchGeometry(list2, list3, "s", isMesh: true, isCamMode);
			}
			cSFrenchGeometry2 = new CSFrenchGeometry(list2, null, "c", isMesh: true, isCamMode);
			if (!boolean)
			{
				cSFrenchGeometry3 = new CSFrenchGeometry(list2, list3, "e", isMesh: true, isCamMode);
			}
		}
		if (cSFrenchGeometry2 == null)
		{
			return UpdateMissing("Parts not found");
		}
		if (cSFrenchGeometry2.GTop == null)
		{
			return UpdateMissing("Top mesh part not found");
		}
		if (cSFrenchGeometry2.GBase == null)
		{
			return UpdateMissing("Base mesh part not found");
		}
		if (cSFrenchGeometry2.GSide.Count == 0)
		{
			return UpdateMissing("Side mesh parts not found");
		}
		List<UltimateGemData> list4 = new List<UltimateGemData>();
		if (!(InGemDataArray.Data is CSDataArray cSDataArray))
		{
			return UpdateMissing("Ultimate GemData Array");
		}
		for (int j = 0; j < cSDataArray.GetArrayCount(); j++)
		{
			if (!(cSDataArray.GetArrayItem(j) is CSDataArray gemInfo))
			{
				return UpdateMissing("Ultimate GemData GemArray");
			}
			UltimateGemData item = UltimateGemData.ReadFromArray(gemInfo);
			list4.Add(item);
		}
		int count2 = list4.Count;
		double normParam = list4[0].NormParam;
		double normParam2 = list4[count2 - 1].NormParam;
		double num5 = normParam2 - normParam;
		double num6 = num3 * num5;
		for (int k = 0; k < count2; k++)
		{
			double num7 = list4[k].NormParam - normParam;
			double num9;
			if (num7 < num6)
			{
				double num8 = num7 / num6;
				num9 = num + (num2 - num) * num8;
			}
			else
			{
				num7 -= num6;
				double num10 = num7 / (normParam2 - num6);
				num9 = num2 + (num4 - num2) * num10;
			}
			list4[k].SizeYPlusWall = list4[k].SizeY / 2.0 + num9;
		}
		if (!cSFrenchGeometry2.SetUpGeometry(halo: false, out var bottomSize, out var midSize, out var topSize, out var _, out var _))
		{
			return UpdateError("Bounding Box not found");
		}
		double bottomSize2 = 0.0;
		double midSize2 = 0.0;
		double topSize2 = 0.0;
		double xSize2 = 0.0;
		double ySize2 = 0.0;
		if (!boolean && !cSFrenchGeometry3.SetUpGeometry(halo: false, out bottomSize2, out midSize2, out topSize2, out xSize2, out ySize2))
		{
			return UpdateError("Bounding Box not found");
		}
		double bottomSize3 = 0.0;
		double midSize3 = 0.0;
		double topSize3 = 0.0;
		double xSize3 = 0.0;
		double ySize3 = 0.0;
		if (!boolean && !cSFrenchGeometry.SetUpGeometry(halo: false, out bottomSize3, out midSize3, out topSize3, out xSize3, out ySize3))
		{
			return UpdateError("Bounding Box not found");
		}
		List<Point3d> edgepoints;
		List<Point3d> zScale;
		List<Point3d> gemCenterPoints;
		List<double> normParamOnCrv;
		int gemZeroIndex;
		if (!boolean)
		{
			CalcGemCurvePoints(list4, value, out edgepoints, out zScale, out gemCenterPoints, out normParamOnCrv);
			gemZeroIndex = 0;
			AddEndCurvePoints(list4, value, ref edgepoints, ref zScale, ref gemCenterPoints, ref normParamOnCrv, xSize3, xSize2, ref gemZeroIndex);
		}
		else
		{
			CalcGemCurvePointsClosed(list4, value, out edgepoints, out zScale, out gemCenterPoints, out normParamOnCrv);
			gemZeroIndex = 3;
		}
		List<Point3d> list5 = new List<Point3d>();
		if (InAverageProfileWidth.Boolean)
		{
			bool[] array = CSFourRailSweep.FindMinMax(list4.Select((UltimateGemData g) => g.SizeYPlusWall).ToArray());
			if (array.Length != edgepoints.Count)
			{
				int count3 = edgepoints.Count;
				int num11 = (count3 - array.Length) / 2;
				bool[] array2 = new bool[count3];
				for (int num12 = 0; num12 < count3; num12++)
				{
					array2[num12] = true;
				}
				for (int num13 = 0; num13 < array.Length; num13++)
				{
					array2[num13 + num11] = array[num13];
				}
				array = array2;
			}
			for (int num14 = 0; num14 < array.Length; num14++)
			{
				if (array[num14])
				{
					list5.Add(edgepoints[num14]);
				}
			}
		}
		else
		{
			list5.AddRange(edgepoints);
		}
		Curve edgecrv = Curve.CreateControlPointCurve(list5, 3);
		Curve zCrv = Curve.CreateControlPointCurve(zScale, 3);
		Curve gemCrv = Curve.CreateInterpolatedCurve(gemCenterPoints, 3);
		double maxIntersectLength = Math.Max(edgepoints[edgepoints.Count - 1].Y, gemCenterPoints[gemCenterPoints.Count - 1].Y);
		CSDataArray cSDataArray2 = new CSDataArray();
		int num15 = count2 - 1;
		if (boolean)
		{
			num15++;
		}
		double baseParamStart;
		double baseParamEnd;
		for (int num16 = 0; num16 < num15; num16++)
		{
			DoMainMorph(cSFrenchGeometry2, isMesh, topSize, midSize, bottomSize, value, value2, edgecrv, zCrv, gemCrv, normParamOnCrv[gemZeroIndex + num16], normParamOnCrv[gemZeroIndex + num16 + 1], edgepoints[gemZeroIndex + num16].X, edgepoints[gemZeroIndex + num16 + 1].X, maxIntersectLength, out var nextm, out baseParamStart, out baseParamEnd, out var _);
			foreach (GeometryBase item2 in nextm)
			{
				cSDataArray2.Append(new CSOnGeometryData(item2));
			}
		}
		Curve edgeCrv2 = null;
		Curve edgeCrv3 = null;
		if (!boolean)
		{
			DoMainMorph(cSFrenchGeometry, isMesh, topSize3, midSize3, bottomSize3, value, value2, edgecrv, zCrv, gemCrv, normParamOnCrv[0], normParamOnCrv[1], edgepoints[0].X, edgepoints[1].X, maxIntersectLength, out var nextm2, out var _, out baseParamEnd, out edgeCrv2);
			foreach (GeometryBase item3 in nextm2)
			{
				cSDataArray2.Append(new CSOnGeometryData(item3));
			}
		}
		if (!boolean)
		{
			DoMainMorph(cSFrenchGeometry3, isMesh, topSize2, midSize2, bottomSize2, value, value2, edgecrv, zCrv, gemCrv, normParamOnCrv[count2], normParamOnCrv[count2 + 1], edgepoints[count2].X, edgepoints[count2 + 1].X, maxIntersectLength, out var nextm3, out baseParamStart, out var _, out edgeCrv3);
			foreach (GeometryBase item4 in nextm3)
			{
				cSDataArray2.Append(new CSOnGeometryData(item4));
			}
		}
		OutChannel.SetData(new CSDataArray(cSDataArray2));
		OutEdgeCrvStart.SetData(new CSOnGeometryData(edgeCrv2));
		OutEdgeCrvEnd.SetData(new CSOnGeometryData(edgeCrv3));
		return base.Update();
	}

	private static void AddEndCurvePoints(List<UltimateGemData> gemList, Curve crv, ref List<Point3d> edgePoints, ref List<Point3d> zScalePoints, ref List<Point3d> gemCenterPoints, ref List<double> normParamOnCrv, double startXSize, double endXSize, ref int gemZeroIndex)
	{
		int count = gemList.Count;
		double length = crv.GetLength();
		startXSize *= gemList[0].SizeX;
		Vector3d vector3d = ((count > 1) ? (edgePoints[1] - edgePoints[0]) : new Vector3d(1.0, 0.0, 0.0));
		vector3d /= vector3d.X;
		Point3d item = new Point3d(edgePoints[0]);
		item.X -= vector3d.X * startXSize;
		item.Y -= vector3d.Y * startXSize;
		edgePoints.Insert(0, item);
		gemCenterPoints.Insert(0, new Point3d(item.X, 0.0, 0.0));
		vector3d = ((count > 1) ? (zScalePoints[1] - zScalePoints[0]) : new Vector3d(1.0, 0.0, 0.0));
		vector3d /= vector3d.X;
		item = new Point3d(zScalePoints[0]);
		item.X -= vector3d.X * startXSize;
		item.Y -= vector3d.Y * startXSize;
		zScalePoints.Insert(0, item);
		double num = normParamOnCrv[0] - startXSize / length;
		if (!crv.IsClosed && num < 0.0)
		{
			num = 0.0;
		}
		normParamOnCrv.Insert(0, num);
		gemZeroIndex++;
		endXSize *= gemList[count - 1].SizeX;
		vector3d = ((count > 1) ? (edgePoints[count] - edgePoints[count - 1]) : new Vector3d(1.0, 0.0, 0.0));
		vector3d /= vector3d.X;
		item = new Point3d(edgePoints[count]);
		item.X += vector3d.X * endXSize;
		item.Y += vector3d.Y * endXSize;
		edgePoints.Add(item);
		gemCenterPoints.Add(new Point3d(item.X, count + 1, 0.0));
		vector3d = ((count > 1) ? (zScalePoints[count] - zScalePoints[count - 1]) : new Vector3d(1.0, 0.0, 0.0));
		vector3d /= vector3d.X;
		item = new Point3d(zScalePoints[count]);
		item.X += vector3d.X * endXSize;
		item.Y += vector3d.Y * endXSize;
		zScalePoints.Add(item);
		double num2 = normParamOnCrv[count] + endXSize / length;
		if (!crv.IsClosed && num2 > 1.0)
		{
			num2 = 1.0;
		}
		normParamOnCrv.Add(num2);
	}

	private static void CalcGemCurvePoints(List<UltimateGemData> gemList, Curve crv, out List<Point3d> edgepoints, out List<Point3d> zScale, out List<Point3d> gemCenterPoints, out List<double> normParamOnCrv)
	{
		int count = gemList.Count;
		double[] array = new double[count];
		normParamOnCrv = new List<double>();
		double length = crv.GetLength();
		double lastNormParam = 0.0;
		double paramEndFix = 0.0;
		double num = 0.0;
		double num2 = 0.0;
		edgepoints = new List<Point3d>();
		zScale = new List<Point3d>();
		gemCenterPoints = new List<Point3d>();
		bool flag = true;
		for (int i = 0; i < count; i++)
		{
			Point3d gemPoint = gemList[i].GemPoint;
			crv.ClosestPoint(gemPoint, out var t);
			array[i] = t;
			double normParam = crv.GetLength(new Interval(crv.Domain.Min, t)) / length;
			ParamClosedFix(flag, ref normParam, ref lastNormParam, ref paramEndFix);
			normParamOnCrv.Add(normParam);
			if (flag)
			{
				flag = false;
				num2 = 0.0;
				edgepoints.Add(new Point3d(num2, gemList[i].SizeYPlusWall, 0.0));
				zScale.Add(new Point3d(num2, gemList[i].SizeY, 0.0));
				gemCenterPoints.Add(new Point3d(num2, 1.0, 0.0));
				continue;
			}
			double num3 = array[i - 1];
			double num4 = array[i];
			if (num3 > num4)
			{
				double num5 = num4;
				num4 = num3;
				num3 = num5;
			}
			double num6 = crv.GetLength(new Interval(num3, num4));
			if (paramEndFix != num)
			{
				num6 = crv.GetLength() - num6;
				num = paramEndFix;
			}
			double num7 = gemList[i - 1].SizeX / 2.0;
			double num8 = gemList[i].SizeX / 2.0;
			double num9 = num6 - (num7 + num8);
			double num10 = (num7 + num9 / 2.0) / num6;
			double x = num2 + num6 / 2.0;
			gemCenterPoints.Add(new Point3d(x, (double)i + num10, 0.0));
			num2 += num6;
			edgepoints.Add(new Point3d(num2, gemList[i].SizeYPlusWall, 0.0));
			zScale.Add(new Point3d(num2, gemList[i].SizeY, 0.0));
			gemCenterPoints.Add(new Point3d(num2, i + 1, 0.0));
		}
	}

	private static void CalcGemCurvePointsClosed(List<UltimateGemData> gemList, Curve crv, out List<Point3d> edgepoints, out List<Point3d> zScale, out List<Point3d> gemCenterPoints, out List<double> normParamOnCrv)
	{
		int count = gemList.Count;
		double[] array = new double[count];
		normParamOnCrv = new List<double>();
		double length = crv.GetLength();
		double lastNormParam = 0.0;
		double paramEndFix = 0.0;
		double num = 0.0;
		double num2 = 0.0;
		edgepoints = new List<Point3d>();
		zScale = new List<Point3d>();
		gemCenterPoints = new List<Point3d>();
		bool flag = true;
		int num3 = 0;
		for (int i = -3; i < count + 3; i++)
		{
			int num4 = (i + count) % count;
			int num5 = (i - 1 + count) % count;
			Point3d gemPoint = gemList[num4].GemPoint;
			crv.ClosestPoint(gemPoint, out var t);
			array[num4] = t;
			double normParam = crv.GetLength(new Interval(crv.Domain.Min, t)) / length;
			ParamClosedFix(flag, ref normParam, ref lastNormParam, ref paramEndFix);
			normParamOnCrv.Add(normParam);
			if (flag)
			{
				flag = false;
				num2 = 0.0;
				edgepoints.Add(new Point3d(num2, gemList[num4].SizeYPlusWall, 0.0));
				zScale.Add(new Point3d(num2, gemList[num4].SizeY, 0.0));
				gemCenterPoints.Add(new Point3d(num2, 1.0, 0.0));
				num3++;
				continue;
			}
			double num6 = array[num5];
			double num7 = array[num4];
			if (num6 > num7)
			{
				double num8 = num7;
				num7 = num6;
				num6 = num8;
			}
			double num9 = crv.GetLength(new Interval(num6, num7));
			if (paramEndFix != num)
			{
				num9 = crv.GetLength() - num9;
				num = paramEndFix;
			}
			double num10 = gemList[num5].SizeX / 2.0;
			double num11 = gemList[num4].SizeX / 2.0;
			double num12 = num9 - (num10 + num11);
			double num13 = (num10 + num12 / 2.0) / num9;
			double x = num2 + num9 / 2.0;
			gemCenterPoints.Add(new Point3d(x, (double)num3 + num13, 0.0));
			num2 += num9;
			edgepoints.Add(new Point3d(num2, gemList[num4].SizeYPlusWall, 0.0));
			zScale.Add(new Point3d(num2, gemList[num4].SizeY, 0.0));
			gemCenterPoints.Add(new Point3d(num2, num3 + 1, 0.0));
			num3++;
		}
	}

	private static double ClosestPointTo(Curve baseCrv, Point3d p, Vector3d v)
	{
		double result = 0.0;
		Plane plane = new Plane(p, v);
		CurveIntersections curveIntersections = Intersection.CurvePlane(baseCrv, plane, 0.001);
		double num = 999.0;
		foreach (IntersectionEvent item in curveIntersections)
		{
			if (item.IsPoint)
			{
				double length = (item.PointA - p).Length;
				if (length < num)
				{
					num = length;
					result = item.ParameterA;
				}
			}
		}
		return result;
	}

	private static void DoMainMorph(CSFrenchGeometry cObj, bool isMesh, double inTopSize, double inMidSize, double inBottomSize, Curve crv, Curve baseCrv, Curve edgecrv, Curve zCrv, Curve gemCrv, double normParam0, double normParam1, double xPos0, double xPos1, double maxIntersectLength, out List<GeometryBase> nextm, out double baseParamStart, out double baseParamEnd, out Curve edgeCrv)
	{
		CSFrenchMorph1 cSFrenchMorph = new CSFrenchMorph1(inBottomSize, inMidSize, inTopSize);
		int num = 0;
		for (int i = 0; i <= 100; i++)
		{
			double num2 = FixNormParam(normParam0 + (normParam1 - normParam0) * (double)i / 100.0);
			crv.NormalizedLengthParameter(num2, out var t);
			cSFrenchMorph.CrvParam[i] = num2;
			cSFrenchMorph.CrvPoints[i] = crv.PointAt(t);
			cSFrenchMorph.CrvTangent[i] = crv.TangentAt(t);
			double x = xPos0 + (xPos1 - xPos0) * (double)i / 100.0;
			LineCurve curveB = new LineCurve(new Point3d(x, 0.0, 0.0), new Point3d(x, maxIntersectLength + 1.0, 0.0));
			CurveIntersections curveIntersections = Intersection.CurveCurve(edgecrv, curveB, 0.001, 0.001);
			if (curveIntersections.Count == 1)
			{
				cSFrenchMorph.YScale[i] = curveIntersections[0].PointA.Y;
			}
			curveIntersections = Intersection.CurveCurve(zCrv, curveB, 0.001, 0.001);
			if (curveIntersections.Count == 1)
			{
				cSFrenchMorph.ZScale[i] = curveIntersections[0].PointA.Y;
			}
			curveIntersections = Intersection.CurveCurve(gemCrv, curveB, 0.001, 0.001);
			if (curveIntersections.Count == 1)
			{
				if (i == 0)
				{
					num = (int)Math.Round(curveIntersections[0].PointA.Y);
				}
				cSFrenchMorph.XShift[i] = curveIntersections[0].PointA.Y - (double)num;
			}
		}
		baseParamStart = ClosestPointTo(baseCrv, cSFrenchMorph.CrvPoints[0], cSFrenchMorph.CrvTangent[0]);
		baseParamEnd = ClosestPointTo(baseCrv, cSFrenchMorph.CrvPoints[100], cSFrenchMorph.CrvTangent[100]);
		baseParamStart = baseCrv.GetLength(new Interval(baseCrv.Domain.Min, baseParamStart)) / baseCrv.GetLength();
		baseParamEnd = baseCrv.GetLength(new Interval(baseCrv.Domain.Min, baseParamEnd)) / baseCrv.GetLength();
		if (baseParamEnd > 0.75 && baseParamStart < 0.25)
		{
			baseParamStart += 1.0;
		}
		else if (baseParamEnd < 0.25 && baseParamStart > 0.75)
		{
			baseParamStart -= 1.0;
		}
		cSFrenchMorph.BaseCrvPoints = new Point3d[101];
		for (int j = 0; j <= 100; j++)
		{
			double v = baseParamStart + (baseParamEnd - baseParamStart) * (double)j / 100.0;
			baseCrv.NormalizedLengthParameter(FixNormParam(v), out var t2);
			cSFrenchMorph.BaseCrvPoints[j] = baseCrv.PointAt(t2);
		}
		for (int k = 0; k <= 100; k++)
		{
			Vector3d a = cSFrenchMorph.CrvPoints[k] - cSFrenchMorph.BaseCrvPoints[k];
			cSFrenchMorph.CrvTangent[k] = Vector3d.CrossProduct(a, cSFrenchMorph.CrvTangent[k]);
			cSFrenchMorph.CrvTangent[k].Unitize();
		}
		nextm = new List<GeometryBase>();
		cSFrenchMorph.WhichPart = 1;
		GeometryBase geometryBase = (isMesh ? ((Mesh)cObj.GBase).DuplicateMesh() : ((Brep)cObj.GBase).Duplicate());
		cSFrenchMorph.Morph(geometryBase);
		FixNorms(geometryBase);
		nextm.Add(geometryBase);
		cSFrenchMorph.WhichPart = 2;
		foreach (GeometryBase item in cObj.GSide)
		{
			GeometryBase geometryBase2 = (isMesh ? ((Mesh)item).DuplicateMesh() : ((Brep)item).Duplicate());
			cSFrenchMorph.Morph(geometryBase2);
			FixNorms(geometryBase2);
			nextm.Add(geometryBase2);
		}
		cSFrenchMorph.WhichPart = 3;
		GeometryBase geometryBase3 = (isMesh ? ((Mesh)cObj.GTop).DuplicateMesh() : ((Brep)cObj.GTop).Duplicate());
		cSFrenchMorph.Morph(geometryBase3);
		FixNorms(geometryBase3);
		nextm.Add(geometryBase3);
		if (cObj.ECrv != null)
		{
			cSFrenchMorph.WhichPart = 0;
			Curve curve = cObj.ECrv.DuplicateCurve();
			cSFrenchMorph.Morph(curve);
			edgeCrv = curve;
		}
		else
		{
			edgeCrv = null;
		}
	}

	private static double FixNormParam(double v)
	{
		while (v < 0.0 || v > 1.0)
		{
			if (v < 0.0)
			{
				v += 1.0;
			}
			if (v > 1.0)
			{
				v -= 1.0;
			}
		}
		return v;
	}

	private static void FixNorms(GeometryBase gb)
	{
		(gb as Mesh)?.Normals.ComputeNormals();
	}

	private static void ParamClosedFix(bool first, ref double normParam, ref double lastNormParam, ref double paramEndFix)
	{
		if (first)
		{
			lastNormParam = normParam;
			return;
		}
		normParam += paramEndFix;
		if (paramEndFix == 0.0)
		{
			if (lastNormParam < 0.25 && normParam > 0.75)
			{
				paramEndFix = -1.0;
				normParam += paramEndFix;
			}
			else if (lastNormParam > 0.75 && normParam < 0.25)
			{
				paramEndFix = 1.0;
				normParam += paramEndFix;
			}
			lastNormParam = normParam;
		}
	}
}
