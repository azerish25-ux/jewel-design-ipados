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
		//IL_05ed: Unknown result type (might be due to invalid IL or missing references)
		//IL_05f2: Unknown result type (might be due to invalid IL or missing references)
		//IL_0606: Unknown result type (might be due to invalid IL or missing references)
		//IL_060b: Unknown result type (might be due to invalid IL or missing references)
		//IL_066c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0671: Unknown result type (might be due to invalid IL or missing references)
		//IL_0683: Unknown result type (might be due to invalid IL or missing references)
		//IL_0688: Unknown result type (might be due to invalid IL or missing references)
		//IL_071b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0720: Unknown result type (might be due to invalid IL or missing references)
		//IL_072c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0731: Unknown result type (might be due to invalid IL or missing references)
		//IL_05a1: Unknown result type (might be due to invalid IL or missing references)
		//IL_07b4: Unknown result type (might be due to invalid IL or missing references)
		//IL_07b9: Unknown result type (might be due to invalid IL or missing references)
		//IL_07c8: Unknown result type (might be due to invalid IL or missing references)
		//IL_07cd: Unknown result type (might be due to invalid IL or missing references)
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
		bool flag = true;
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
			Brep val = (flattenArray.Array[i] as ICSBrep)?.GetBrep();
			if (val != null)
			{
				list.Add(val);
				continue;
			}
			Mesh val2 = (flattenArray.Array[i] as ICSMesh)?.GetMesh();
			if (val2 != null)
			{
				list2.Add(val2);
				continue;
			}
			Curve val3 = (flattenArray.Array[i] as ICSCurve)?.GetCurve();
			if (val3 != null)
			{
				list3.Add(val3);
			}
		}
		if (list.Count > 0)
		{
			if (!boolean)
			{
				cSFrenchGeometry = new CSFrenchGeometry((IEnumerable<GeometryBase>)list, list3, "s", isMesh: false, isCamMode);
			}
			cSFrenchGeometry2 = new CSFrenchGeometry((IEnumerable<GeometryBase>)list, null, "c", isMesh: false, isCamMode);
			if (!boolean)
			{
				cSFrenchGeometry3 = new CSFrenchGeometry((IEnumerable<GeometryBase>)list, list3, "e", isMesh: false, isCamMode);
			}
			flag = false;
		}
		else if (list2.Count > 0)
		{
			if (!boolean)
			{
				cSFrenchGeometry = new CSFrenchGeometry((IEnumerable<GeometryBase>)list2, list3, "s", isMesh: true, isCamMode);
			}
			cSFrenchGeometry2 = new CSFrenchGeometry((IEnumerable<GeometryBase>)list2, null, "c", isMesh: true, isCamMode);
			if (!boolean)
			{
				cSFrenchGeometry3 = new CSFrenchGeometry((IEnumerable<GeometryBase>)list2, list3, "e", isMesh: true, isCamMode);
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
		Curve edgecrv = Curve.CreateControlPointCurve((IEnumerable<Point3d>)list5, 3);
		Curve zCrv = Curve.CreateControlPointCurve((IEnumerable<Point3d>)zScale, 3);
		Curve gemCrv = Curve.CreateInterpolatedCurve((IEnumerable<Point3d>)gemCenterPoints, 3);
		Point3d val4 = edgepoints[edgepoints.Count - 1];
		double y = ((Point3d)(ref val4)).Y;
		val4 = gemCenterPoints[gemCenterPoints.Count - 1];
		double maxIntersectLength = Math.Max(y, ((Point3d)(ref val4)).Y);
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
			CSFrenchGeometry cObj = cSFrenchGeometry2;
			bool isMesh = flag;
			double inTopSize = topSize;
			double inMidSize = midSize;
			double inBottomSize = bottomSize;
			Curve crv = value;
			Curve baseCrv = value2;
			double normParam3 = normParamOnCrv[gemZeroIndex + num16];
			double normParam4 = normParamOnCrv[gemZeroIndex + num16 + 1];
			val4 = edgepoints[gemZeroIndex + num16];
			double x = ((Point3d)(ref val4)).X;
			val4 = edgepoints[gemZeroIndex + num16 + 1];
			DoMainMorph(cObj, isMesh, inTopSize, inMidSize, inBottomSize, crv, baseCrv, edgecrv, zCrv, gemCrv, normParam3, normParam4, x, ((Point3d)(ref val4)).X, maxIntersectLength, out var nextm, out baseParamStart, out baseParamEnd, out var _);
			foreach (GeometryBase item2 in nextm)
			{
				cSDataArray2.Append(new CSOnGeometryData(item2));
			}
		}
		Curve edgeCrv2 = null;
		Curve edgeCrv3 = null;
		if (!boolean)
		{
			CSFrenchGeometry cObj2 = cSFrenchGeometry;
			bool isMesh2 = flag;
			double inTopSize2 = topSize3;
			double inMidSize2 = midSize3;
			double inBottomSize2 = bottomSize3;
			Curve crv2 = value;
			Curve baseCrv2 = value2;
			double normParam5 = normParamOnCrv[0];
			double normParam6 = normParamOnCrv[1];
			val4 = edgepoints[0];
			double x2 = ((Point3d)(ref val4)).X;
			val4 = edgepoints[1];
			DoMainMorph(cObj2, isMesh2, inTopSize2, inMidSize2, inBottomSize2, crv2, baseCrv2, edgecrv, zCrv, gemCrv, normParam5, normParam6, x2, ((Point3d)(ref val4)).X, maxIntersectLength, out var nextm2, out var _, out baseParamEnd, out edgeCrv2);
			foreach (GeometryBase item3 in nextm2)
			{
				cSDataArray2.Append(new CSOnGeometryData(item3));
			}
		}
		if (!boolean)
		{
			CSFrenchGeometry cObj3 = cSFrenchGeometry3;
			bool isMesh3 = flag;
			double inTopSize3 = topSize2;
			double inMidSize3 = midSize2;
			double inBottomSize3 = bottomSize2;
			Curve crv3 = value;
			Curve baseCrv3 = value2;
			double normParam7 = normParamOnCrv[count2];
			double normParam8 = normParamOnCrv[count2 + 1];
			val4 = edgepoints[count2];
			double x3 = ((Point3d)(ref val4)).X;
			val4 = edgepoints[count2 + 1];
			DoMainMorph(cObj3, isMesh3, inTopSize3, inMidSize3, inBottomSize3, crv3, baseCrv3, edgecrv, zCrv, gemCrv, normParam7, normParam8, x3, ((Point3d)(ref val4)).X, maxIntersectLength, out var nextm3, out baseParamStart, out var _, out edgeCrv3);
			foreach (GeometryBase item4 in nextm3)
			{
				cSDataArray2.Append(new CSOnGeometryData(item4));
			}
		}
		OutChannel.SetData(new CSDataArray(cSDataArray2));
		OutEdgeCrvStart.SetData(new CSOnGeometryData((GeometryBase)(object)edgeCrv2));
		OutEdgeCrvEnd.SetData(new CSOnGeometryData((GeometryBase)(object)edgeCrv3));
		return base.Update();
	}

	private static void AddEndCurvePoints(List<UltimateGemData> gemList, Curve crv, ref List<Point3d> edgePoints, ref List<Point3d> zScalePoints, ref List<Point3d> gemCenterPoints, ref List<double> normParamOnCrv, double startXSize, double endXSize, ref int gemZeroIndex)
	{
		//IL_0048: Unknown result type (might be due to invalid IL or missing references)
		//IL_0050: Unknown result type (might be due to invalid IL or missing references)
		//IL_0055: Unknown result type (might be due to invalid IL or missing references)
		//IL_003e: Unknown result type (might be due to invalid IL or missing references)
		//IL_005a: Unknown result type (might be due to invalid IL or missing references)
		//IL_005b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0063: Unknown result type (might be due to invalid IL or missing references)
		//IL_0068: Unknown result type (might be due to invalid IL or missing references)
		//IL_006e: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ad: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d0: Unknown result type (might be due to invalid IL or missing references)
		//IL_0103: Unknown result type (might be due to invalid IL or missing references)
		//IL_010b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0110: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f9: Unknown result type (might be due to invalid IL or missing references)
		//IL_0115: Unknown result type (might be due to invalid IL or missing references)
		//IL_0116: Unknown result type (might be due to invalid IL or missing references)
		//IL_011e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0123: Unknown result type (might be due to invalid IL or missing references)
		//IL_0129: Unknown result type (might be due to invalid IL or missing references)
		//IL_0168: Unknown result type (might be due to invalid IL or missing references)
		//IL_01ed: Unknown result type (might be due to invalid IL or missing references)
		//IL_01f7: Unknown result type (might be due to invalid IL or missing references)
		//IL_01fc: Unknown result type (might be due to invalid IL or missing references)
		//IL_01e3: Unknown result type (might be due to invalid IL or missing references)
		//IL_0201: Unknown result type (might be due to invalid IL or missing references)
		//IL_0202: Unknown result type (might be due to invalid IL or missing references)
		//IL_020a: Unknown result type (might be due to invalid IL or missing references)
		//IL_020f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0215: Unknown result type (might be due to invalid IL or missing references)
		//IL_0253: Unknown result type (might be due to invalid IL or missing references)
		//IL_0270: Unknown result type (might be due to invalid IL or missing references)
		//IL_02a3: Unknown result type (might be due to invalid IL or missing references)
		//IL_02ad: Unknown result type (might be due to invalid IL or missing references)
		//IL_02b2: Unknown result type (might be due to invalid IL or missing references)
		//IL_0299: Unknown result type (might be due to invalid IL or missing references)
		//IL_02b7: Unknown result type (might be due to invalid IL or missing references)
		//IL_02b8: Unknown result type (might be due to invalid IL or missing references)
		//IL_02c0: Unknown result type (might be due to invalid IL or missing references)
		//IL_02c5: Unknown result type (might be due to invalid IL or missing references)
		//IL_02cb: Unknown result type (might be due to invalid IL or missing references)
		//IL_0309: Unknown result type (might be due to invalid IL or missing references)
		int count = gemList.Count;
		double length = crv.GetLength();
		startXSize *= gemList[0].SizeX;
		Vector3d val = (Vector3d)((count > 1) ? (edgePoints[1] - edgePoints[0]) : new Vector3d(1.0, 0.0, 0.0));
		val /= ((Vector3d)(ref val)).X;
		Point3d item = default(Point3d);
		((Point3d)(ref item))._002Ector(edgePoints[0]);
		((Point3d)(ref item)).X = ((Point3d)(ref item)).X - ((Vector3d)(ref val)).X * startXSize;
		((Point3d)(ref item)).Y = ((Point3d)(ref item)).Y - ((Vector3d)(ref val)).Y * startXSize;
		edgePoints.Insert(0, item);
		gemCenterPoints.Insert(0, new Point3d(((Point3d)(ref item)).X, 0.0, 0.0));
		val = (Vector3d)((count > 1) ? (zScalePoints[1] - zScalePoints[0]) : new Vector3d(1.0, 0.0, 0.0));
		val /= ((Vector3d)(ref val)).X;
		((Point3d)(ref item))._002Ector(zScalePoints[0]);
		((Point3d)(ref item)).X = ((Point3d)(ref item)).X - ((Vector3d)(ref val)).X * startXSize;
		((Point3d)(ref item)).Y = ((Point3d)(ref item)).Y - ((Vector3d)(ref val)).Y * startXSize;
		zScalePoints.Insert(0, item);
		double num = normParamOnCrv[0] - startXSize / length;
		if (!crv.IsClosed && num < 0.0)
		{
			num = 0.0;
		}
		normParamOnCrv.Insert(0, num);
		gemZeroIndex++;
		endXSize *= gemList[count - 1].SizeX;
		val = (Vector3d)((count > 1) ? (edgePoints[count] - edgePoints[count - 1]) : new Vector3d(1.0, 0.0, 0.0));
		val /= ((Vector3d)(ref val)).X;
		((Point3d)(ref item))._002Ector(edgePoints[count]);
		((Point3d)(ref item)).X = ((Point3d)(ref item)).X + ((Vector3d)(ref val)).X * endXSize;
		((Point3d)(ref item)).Y = ((Point3d)(ref item)).Y + ((Vector3d)(ref val)).Y * endXSize;
		edgePoints.Add(item);
		gemCenterPoints.Add(new Point3d(((Point3d)(ref item)).X, (double)(count + 1), 0.0));
		val = (Vector3d)((count > 1) ? (zScalePoints[count] - zScalePoints[count - 1]) : new Vector3d(1.0, 0.0, 0.0));
		val /= ((Vector3d)(ref val)).X;
		((Point3d)(ref item))._002Ector(zScalePoints[count]);
		((Point3d)(ref item)).X = ((Point3d)(ref item)).X + ((Vector3d)(ref val)).X * endXSize;
		((Point3d)(ref item)).Y = ((Point3d)(ref item)).Y + ((Vector3d)(ref val)).Y * endXSize;
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
		//IL_0071: Unknown result type (might be due to invalid IL or missing references)
		//IL_0076: Unknown result type (might be due to invalid IL or missing references)
		//IL_0079: Unknown result type (might be due to invalid IL or missing references)
		//IL_008b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0090: Unknown result type (might be due to invalid IL or missing references)
		//IL_009b: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ec: Unknown result type (might be due to invalid IL or missing references)
		//IL_0110: Unknown result type (might be due to invalid IL or missing references)
		//IL_0131: Unknown result type (might be due to invalid IL or missing references)
		//IL_0161: Unknown result type (might be due to invalid IL or missing references)
		//IL_01f9: Unknown result type (might be due to invalid IL or missing references)
		//IL_0224: Unknown result type (might be due to invalid IL or missing references)
		//IL_0248: Unknown result type (might be due to invalid IL or missing references)
		//IL_0265: Unknown result type (might be due to invalid IL or missing references)
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
		double num3 = default(double);
		for (int i = 0; i < count; i++)
		{
			Point3d gemPoint = gemList[i].GemPoint;
			crv.ClosestPoint(gemPoint, ref num3);
			array[i] = num3;
			Interval domain = crv.Domain;
			double normParam = crv.GetLength(new Interval(((Interval)(ref domain)).Min, num3)) / length;
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
			double num4 = array[i - 1];
			double num5 = array[i];
			if (num4 > num5)
			{
				double num6 = num5;
				num5 = num4;
				num4 = num6;
			}
			double num7 = crv.GetLength(new Interval(num4, num5));
			if (paramEndFix != num)
			{
				num7 = crv.GetLength() - num7;
				num = paramEndFix;
			}
			double num8 = gemList[i - 1].SizeX / 2.0;
			double num9 = gemList[i].SizeX / 2.0;
			double num10 = num7 - (num8 + num9);
			double num11 = (num8 + num10 / 2.0) / num7;
			double num12 = num2 + num7 / 2.0;
			gemCenterPoints.Add(new Point3d(num12, (double)i + num11, 0.0));
			num2 += num7;
			edgepoints.Add(new Point3d(num2, gemList[i].SizeYPlusWall, 0.0));
			zScale.Add(new Point3d(num2, gemList[i].SizeY, 0.0));
			gemCenterPoints.Add(new Point3d(num2, (double)(i + 1), 0.0));
		}
	}

	private static void CalcGemCurvePointsClosed(List<UltimateGemData> gemList, Curve crv, out List<Point3d> edgepoints, out List<Point3d> zScale, out List<Point3d> gemCenterPoints, out List<double> normParamOnCrv)
	{
		//IL_0087: Unknown result type (might be due to invalid IL or missing references)
		//IL_008c: Unknown result type (might be due to invalid IL or missing references)
		//IL_008f: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a1: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a6: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b1: Unknown result type (might be due to invalid IL or missing references)
		//IL_0105: Unknown result type (might be due to invalid IL or missing references)
		//IL_0129: Unknown result type (might be due to invalid IL or missing references)
		//IL_014a: Unknown result type (might be due to invalid IL or missing references)
		//IL_017e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0214: Unknown result type (might be due to invalid IL or missing references)
		//IL_023f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0263: Unknown result type (might be due to invalid IL or missing references)
		//IL_0280: Unknown result type (might be due to invalid IL or missing references)
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
		double num6 = default(double);
		for (int i = -3; i < count + 3; i++)
		{
			int num4 = (i + count) % count;
			int num5 = (i - 1 + count) % count;
			Point3d gemPoint = gemList[num4].GemPoint;
			crv.ClosestPoint(gemPoint, ref num6);
			array[num4] = num6;
			Interval domain = crv.Domain;
			double normParam = crv.GetLength(new Interval(((Interval)(ref domain)).Min, num6)) / length;
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
			double num7 = array[num5];
			double num8 = array[num4];
			if (num7 > num8)
			{
				double num9 = num8;
				num8 = num7;
				num7 = num9;
			}
			double num10 = crv.GetLength(new Interval(num7, num8));
			if (paramEndFix != num)
			{
				num10 = crv.GetLength() - num10;
				num = paramEndFix;
			}
			double num11 = gemList[num5].SizeX / 2.0;
			double num12 = gemList[num4].SizeX / 2.0;
			double num13 = num10 - (num11 + num12);
			double num14 = (num11 + num13 / 2.0) / num10;
			double num15 = num2 + num10 / 2.0;
			gemCenterPoints.Add(new Point3d(num15, (double)num3 + num14, 0.0));
			num2 += num10;
			edgepoints.Add(new Point3d(num2, gemList[num4].SizeYPlusWall, 0.0));
			zScale.Add(new Point3d(num2, gemList[num4].SizeY, 0.0));
			gemCenterPoints.Add(new Point3d(num2, (double)(num3 + 1), 0.0));
			num3++;
		}
	}

	private static double ClosestPointTo(Curve baseCrv, Point3d p, Vector3d v)
	{
		//IL_000c: Unknown result type (might be due to invalid IL or missing references)
		//IL_000d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0014: Unknown result type (might be due to invalid IL or missing references)
		//IL_0048: Unknown result type (might be due to invalid IL or missing references)
		//IL_004d: Unknown result type (might be due to invalid IL or missing references)
		//IL_004e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0053: Unknown result type (might be due to invalid IL or missing references)
		double result = 0.0;
		Plane val = default(Plane);
		((Plane)(ref val))._002Ector(p, v);
		CurveIntersections obj = Intersection.CurvePlane(baseCrv, val, 0.001);
		double num = 999.0;
		foreach (IntersectionEvent item in obj)
		{
			if (item.IsPoint)
			{
				Vector3d val2 = item.PointA - p;
				double length = ((Vector3d)(ref val2)).Length;
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
		//IL_0054: Unknown result type (might be due to invalid IL or missing references)
		//IL_0059: Unknown result type (might be due to invalid IL or missing references)
		//IL_006a: Unknown result type (might be due to invalid IL or missing references)
		//IL_006f: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a0: Unknown result type (might be due to invalid IL or missing references)
		//IL_00bc: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c1: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c8: Expected O, but got Unknown
		//IL_01d8: Unknown result type (might be due to invalid IL or missing references)
		//IL_01e4: Unknown result type (might be due to invalid IL or missing references)
		//IL_01fb: Unknown result type (might be due to invalid IL or missing references)
		//IL_0208: Unknown result type (might be due to invalid IL or missing references)
		//IL_0219: Unknown result type (might be due to invalid IL or missing references)
		//IL_021e: Unknown result type (might be due to invalid IL or missing references)
		//IL_022a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0243: Unknown result type (might be due to invalid IL or missing references)
		//IL_0248: Unknown result type (might be due to invalid IL or missing references)
		//IL_0254: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ff: Unknown result type (might be due to invalid IL or missing references)
		//IL_0104: Unknown result type (might be due to invalid IL or missing references)
		//IL_0145: Unknown result type (might be due to invalid IL or missing references)
		//IL_014a: Unknown result type (might be due to invalid IL or missing references)
		//IL_01ac: Unknown result type (might be due to invalid IL or missing references)
		//IL_01b1: Unknown result type (might be due to invalid IL or missing references)
		//IL_0187: Unknown result type (might be due to invalid IL or missing references)
		//IL_018c: Unknown result type (might be due to invalid IL or missing references)
		//IL_030b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0310: Unknown result type (might be due to invalid IL or missing references)
		//IL_032e: Unknown result type (might be due to invalid IL or missing references)
		//IL_033b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0340: Unknown result type (might be due to invalid IL or missing references)
		//IL_0345: Unknown result type (might be due to invalid IL or missing references)
		//IL_034f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0359: Unknown result type (might be due to invalid IL or missing references)
		//IL_035e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0363: Unknown result type (might be due to invalid IL or missing references)
		//IL_03b1: Unknown result type (might be due to invalid IL or missing references)
		//IL_039f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0405: Unknown result type (might be due to invalid IL or missing references)
		//IL_03f7: Unknown result type (might be due to invalid IL or missing references)
		//IL_0466: Unknown result type (might be due to invalid IL or missing references)
		//IL_0454: Unknown result type (might be due to invalid IL or missing references)
		CSFrenchMorph1 cSFrenchMorph = new CSFrenchMorph1(inBottomSize, inMidSize, inTopSize);
		int num = 0;
		double num3 = default(double);
		for (int i = 0; i <= 100; i++)
		{
			double num2 = FixNormParam(normParam0 + (normParam1 - normParam0) * (double)i / 100.0);
			crv.NormalizedLengthParameter(num2, ref num3);
			cSFrenchMorph.CrvParam[i] = num2;
			cSFrenchMorph.CrvPoints[i] = crv.PointAt(num3);
			cSFrenchMorph.CrvTangent[i] = crv.TangentAt(num3);
			double num4 = xPos0 + (xPos1 - xPos0) * (double)i / 100.0;
			LineCurve val = new LineCurve(new Point3d(num4, 0.0, 0.0), new Point3d(num4, maxIntersectLength + 1.0, 0.0));
			CurveIntersections val2 = Intersection.CurveCurve(edgecrv, (Curve)(object)val, 0.001, 0.001);
			Point3d pointA;
			if (val2.Count == 1)
			{
				double[] yScale = cSFrenchMorph.YScale;
				int num5 = i;
				pointA = val2[0].PointA;
				yScale[num5] = ((Point3d)(ref pointA)).Y;
			}
			val2 = Intersection.CurveCurve(zCrv, (Curve)(object)val, 0.001, 0.001);
			if (val2.Count == 1)
			{
				double[] zScale = cSFrenchMorph.ZScale;
				int num6 = i;
				pointA = val2[0].PointA;
				zScale[num6] = ((Point3d)(ref pointA)).Y;
			}
			val2 = Intersection.CurveCurve(gemCrv, (Curve)(object)val, 0.001, 0.001);
			if (val2.Count == 1)
			{
				if (i == 0)
				{
					pointA = val2[0].PointA;
					num = (int)Math.Round(((Point3d)(ref pointA)).Y);
				}
				double[] xShift = cSFrenchMorph.XShift;
				int num7 = i;
				pointA = val2[0].PointA;
				xShift[num7] = ((Point3d)(ref pointA)).Y - (double)num;
			}
		}
		baseParamStart = ClosestPointTo(baseCrv, cSFrenchMorph.CrvPoints[0], cSFrenchMorph.CrvTangent[0]);
		baseParamEnd = ClosestPointTo(baseCrv, cSFrenchMorph.CrvPoints[100], cSFrenchMorph.CrvTangent[100]);
		Interval domain = baseCrv.Domain;
		baseParamStart = baseCrv.GetLength(new Interval(((Interval)(ref domain)).Min, baseParamStart)) / baseCrv.GetLength();
		domain = baseCrv.Domain;
		baseParamEnd = baseCrv.GetLength(new Interval(((Interval)(ref domain)).Min, baseParamEnd)) / baseCrv.GetLength();
		if (baseParamEnd > 0.75 && baseParamStart < 0.25)
		{
			baseParamStart += 1.0;
		}
		else if (baseParamEnd < 0.25 && baseParamStart > 0.75)
		{
			baseParamStart -= 1.0;
		}
		cSFrenchMorph.BaseCrvPoints = (Point3d[])(object)new Point3d[101];
		double num8 = default(double);
		for (int j = 0; j <= 100; j++)
		{
			double v = baseParamStart + (baseParamEnd - baseParamStart) * (double)j / 100.0;
			baseCrv.NormalizedLengthParameter(FixNormParam(v), ref num8);
			cSFrenchMorph.BaseCrvPoints[j] = baseCrv.PointAt(num8);
		}
		for (int k = 0; k <= 100; k++)
		{
			Vector3d val3 = cSFrenchMorph.CrvPoints[k] - cSFrenchMorph.BaseCrvPoints[k];
			cSFrenchMorph.CrvTangent[k] = Vector3d.CrossProduct(val3, cSFrenchMorph.CrvTangent[k]);
			((Vector3d)(ref cSFrenchMorph.CrvTangent[k])).Unitize();
		}
		nextm = new List<GeometryBase>();
		cSFrenchMorph.WhichPart = 1;
		GeometryBase val4 = (GeometryBase)(isMesh ? ((object)((Mesh)cObj.GBase).DuplicateMesh()) : ((object)((GeometryBase)(Brep)cObj.GBase).Duplicate()));
		((SpaceMorph)cSFrenchMorph).Morph(val4);
		FixNorms(val4);
		nextm.Add(val4);
		cSFrenchMorph.WhichPart = 2;
		foreach (GeometryBase item in cObj.GSide)
		{
			GeometryBase val5 = (GeometryBase)(isMesh ? ((object)((Mesh)item).DuplicateMesh()) : ((object)((GeometryBase)(Brep)item).Duplicate()));
			((SpaceMorph)cSFrenchMorph).Morph(val5);
			FixNorms(val5);
			nextm.Add(val5);
		}
		cSFrenchMorph.WhichPart = 3;
		GeometryBase val6 = (GeometryBase)(isMesh ? ((object)((Mesh)cObj.GTop).DuplicateMesh()) : ((object)((GeometryBase)(Brep)cObj.GTop).Duplicate()));
		((SpaceMorph)cSFrenchMorph).Morph(val6);
		FixNorms(val6);
		nextm.Add(val6);
		if (cObj.ECrv != null)
		{
			cSFrenchMorph.WhichPart = 0;
			Curve val7 = cObj.ECrv.DuplicateCurve();
			((SpaceMorph)cSFrenchMorph).Morph((GeometryBase)(object)val7);
			edgeCrv = val7;
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
		GeometryBase obj = ((gb is Mesh) ? gb : null);
		if (obj != null)
		{
			((Mesh)obj).Normals.ComputeNormals();
		}
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
