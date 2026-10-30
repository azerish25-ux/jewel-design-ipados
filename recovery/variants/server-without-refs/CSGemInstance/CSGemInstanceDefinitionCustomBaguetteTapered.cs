using System;
using System.Collections.Generic;
using System.Drawing;
using CSCompatibility;
using CSGemInstance.GemTableData;
using Rhino;
using Rhino.Collections;
using Rhino.DocObjects;
using Rhino.Geometry;

namespace CSGemInstance;

public sealed class CSGemInstanceDefinitionCustomBaguetteTapered : CSGemInstanceDefinitionCustom
{
	private readonly double _length;

	private readonly double _widthBottom;

	private readonly double _widthTop;

	private readonly double _thickness;

	private readonly double _girdleThickness;

	public override string BlockName { get; }

	public CSGemInstanceDefinitionCustomBaguetteTapered(double length, double widthBottom, double widthTop, double thickness, double girdleThickness, bool useOldShapeName = false)
	{
		_length = length;
		_widthBottom = widthBottom;
		_widthTop = widthTop;
		_thickness = thickness;
		_girdleThickness = girdleThickness;
		_gemTableType = GemTableType.Diamond;
		_gemTableShape = GemTableShape.BaguetteTapered;
		_gemTableLayer = GemTableHelper.GetLayer(_gemTableType, _gemTableShape);
		_gemTableVersion = GemTableHelper.GetCurrentGemTableVersion(_gemTableType, _gemTableShape);
		_type = GemTableHelper.GetDescriptionFromValue(_gemTableType);
		_shape = (useOldShapeName ? $"Baguette_Length{length}_WidthBottom{widthBottom}_WidthTop{widthTop}_Depth{thickness}_GirdleThickness{girdleThickness}" : GemTableHelper.GetDescriptionFromValue(_gemTableShape));
		_layer = GemTableHelper.GetDescriptionFromValue(_gemTableLayer);
		_fudgefactor = GemTableHelper.GetGem(_gemTableType, _gemTableShape).FudgeFactor;
		base.Sizes.Clear();
		base.Sizes.AddRange(GemTableHelper.GetGem(_gemTableType, _gemTableShape).SizeTable());
		_versionMajor = CSGemInstanceDefinitionHelper.CurrentVersionMajor;
		_versionMinor = CSGemInstanceDefinitionHelper.CurrentVersionMinor;
		_isGeneratedGem = true;
		BlockName = $"Custom_Baguette_Length{_length}_WidthBottom{_widthBottom}_WidthTop{_widthTop}_Height{_thickness}_Girdle{_girdleThickness}";
		Baguette();
	}

	protected override bool CreateGemInstanceDefinition(out InstanceDefinition gemInstanceDefinition)
	{
		//IL_006e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0073: Unknown result type (might be due to invalid IL or missing references)
		//IL_007a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0081: Unknown result type (might be due to invalid IL or missing references)
		//IL_008d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0094: Unknown result type (might be due to invalid IL or missing references)
		//IL_009b: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a3: Expected O, but got Unknown
		//IL_0024: Unknown result type (might be due to invalid IL or missing references)
		//IL_0029: Unknown result type (might be due to invalid IL or missing references)
		//IL_0034: Unknown result type (might be due to invalid IL or missing references)
		//IL_0048: Expected O, but got Unknown
		//IL_0481: Unknown result type (might be due to invalid IL or missing references)
		gemInstanceDefinition = null;
		GeometryBase brep = (GeometryBase)(object)GetBrep();
		int num = RhinoDoc.ActiveDoc.Layers.Find("Gem", true);
		if (num == -1)
		{
			Layer val = new Layer
			{
				Name = "Gem",
				Color = Color.FromArgb(0, 0, 128)
			};
			num = RhinoDoc.ActiveDoc.Layers.Add(val);
			if (num == -1)
			{
				num = RhinoDoc.ActiveDoc.Layers.CurrentLayerIndex;
			}
		}
		ObjectAttributes val2 = new ObjectAttributes
		{
			ColorSource = (ObjectColorSource)3,
			LayerIndex = num,
			Name = _shape,
			MaterialSource = (ObjectMaterialSource)3,
			PlotColorSource = (ObjectPlotColorSource)3,
			PlotWeightSource = (ObjectPlotWeightSource)3
		};
		Guid guid = RhinoDoc.ActiveDoc.Objects.Add(brep, val2);
		if (guid == Guid.Empty)
		{
			return false;
		}
		RhinoScriptInterface.SetObjectData(guid, "MatrixICGem", "ID", "Gem");
		RhinoScriptInterface.SetObjectData(guid, "MatrixICGem", "GemType", "Custom");
		RhinoScriptInterface.SetObjectData(guid, "MatrixICGem", "GemShape", "Baguette");
		RhinoScriptInterface.SetObjectData(guid, "MatrixICGem", "Version", "1.1");
		RhinoScriptInterface.SetObjectData(guid, "MatrixICGem", "GemLayer", _layer);
		RhinoScriptInterface.SetObjectData(guid, "MatrixICGem", "GemXMin", _minX);
		RhinoScriptInterface.SetObjectData(guid, "MatrixICGem", "GemYMin", _minY);
		RhinoScriptInterface.SetObjectData(guid, "MatrixICGem", "GemZMin", _minZ);
		RhinoScriptInterface.SetObjectData(guid, "MatrixICGem", "GemXMax", _maxX);
		RhinoScriptInterface.SetObjectData(guid, "MatrixICGem", "GemYMax", _maxY);
		RhinoScriptInterface.SetObjectData(guid, "MatrixICGem", "GemZMax", _maxZ);
		RhinoScriptInterface.SetObjectData(guid, "MatrixICGem", "FFactor", _fudgefactor);
		RhinoScriptInterface.SetObjectData(guid, "MatrixICGem", "HeightPercent", Math.Round(Math.Max(_widthBottom, _widthTop) / _length, 2));
		if (!CSUtilCpt.GetCptStr(_curve, out var strPoints, out var strKnots, out var strWeights, out var strDegree, out var strDomain, out var strBoundingBox))
		{
			return false;
		}
		RhinoScriptInterface.SetObjectData(guid, "MatrixICGem", "CPT0", strPoints);
		RhinoScriptInterface.SetObjectData(guid, "MatrixICGem", "CPT1", strKnots);
		RhinoScriptInterface.SetObjectData(guid, "MatrixICGem", "CPT2", strWeights);
		RhinoScriptInterface.SetObjectData(guid, "MatrixICGem", "CPT3", strDegree);
		RhinoScriptInterface.SetObjectData(guid, "MatrixICGem", "CPT4", strDomain);
		RhinoScriptInterface.SetObjectData(guid, "MatrixICGem", "CPT6", strBoundingBox);
		RhinoScriptInterface.SetObjectData(guid, "MatrixICGem", "ProngPointCount", base.ProngPositions.Count);
		for (int i = 0; i < base.ProngPositions.Count; i++)
		{
			ProngPosition prongPosition = base.ProngPositions[i];
			RhinoScriptInterface.SetObjectData(guid, "MatrixICGem", $"ProngPoint{i}", prongPosition.ProngParam);
			RhinoScriptInterface.SetObjectData(guid, "MatrixICGem", $"ProngPointLayer{i}", prongPosition.ProngLayer);
		}
		RhinoScriptInterface.SetObjectData(guid, "MatrixICGem", "SizeCount", 1);
		RhinoScriptInterface.SetObjectData(guid, "MatrixICGem", "SizeX0", _maxX - _minX);
		RhinoScriptInterface.SetObjectData(guid, "MatrixICGem", "SizeY0", _maxY - _minY);
		RhinoScriptInterface.SetObjectData(guid, "MatrixICGem", "ShowX", true);
		RhinoScriptInterface.SetObjectData(guid, "MatrixICGem", "ShowY", true);
		RhinoScriptInterface.SetObjectData(guid, "MatrixICGem", "LabelX", "Width");
		RhinoScriptInterface.SetObjectData(guid, "MatrixICGem", "LabelY", "Length");
		RhinoScriptInterface.SetObjectData(guid, "MatrixICGem", "Bottom", _widthBottom);
		RhinoScriptInterface.SetObjectData(guid, "MatrixICGem", "Top", _widthTop);
		RhinoScriptInterface.SetObjectData(guid, "MatrixICGem", "Depth", _thickness);
		RhinoScriptInterface.SetObjectData(guid, "MatrixICGem", "Girdle", _girdleThickness);
		RhinoObject val3 = RhinoDoc.ActiveDoc.Objects.Find(guid);
		int num2 = RhinoDoc.ActiveDoc.InstanceDefinitions.Add(BlockName, string.Empty, Point3d.Origin, val3.Geometry, val2);
		RhinoDoc.ActiveDoc.Objects.Delete(guid, true);
		if (num2 >= 0)
		{
			gemInstanceDefinition = RhinoDoc.ActiveDoc.InstanceDefinitions[num2];
		}
		return gemInstanceDefinition != null;
	}

	private void CalculateVertices(Point3dList profilePoints, double halfGirdleThickness, double topThickness, double bottomThickness)
	{
		//IL_0032: Unknown result type (might be due to invalid IL or missing references)
		//IL_0037: Unknown result type (might be due to invalid IL or missing references)
		//IL_0044: Unknown result type (might be due to invalid IL or missing references)
		//IL_0049: Unknown result type (might be due to invalid IL or missing references)
		//IL_0058: Unknown result type (might be due to invalid IL or missing references)
		//IL_006b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0070: Unknown result type (might be due to invalid IL or missing references)
		//IL_007d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0082: Unknown result type (might be due to invalid IL or missing references)
		//IL_0091: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a4: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a9: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b6: Unknown result type (might be due to invalid IL or missing references)
		//IL_00bb: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ca: Unknown result type (might be due to invalid IL or missing references)
		//IL_00dd: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e2: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ef: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f4: Unknown result type (might be due to invalid IL or missing references)
		//IL_0103: Unknown result type (might be due to invalid IL or missing references)
		//IL_0134: Unknown result type (might be due to invalid IL or missing references)
		//IL_0139: Unknown result type (might be due to invalid IL or missing references)
		//IL_0146: Unknown result type (might be due to invalid IL or missing references)
		//IL_014b: Unknown result type (might be due to invalid IL or missing references)
		//IL_015a: Unknown result type (might be due to invalid IL or missing references)
		//IL_016d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0172: Unknown result type (might be due to invalid IL or missing references)
		//IL_017f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0184: Unknown result type (might be due to invalid IL or missing references)
		//IL_0193: Unknown result type (might be due to invalid IL or missing references)
		//IL_01a6: Unknown result type (might be due to invalid IL or missing references)
		//IL_01ab: Unknown result type (might be due to invalid IL or missing references)
		//IL_01b8: Unknown result type (might be due to invalid IL or missing references)
		//IL_01bd: Unknown result type (might be due to invalid IL or missing references)
		//IL_01cc: Unknown result type (might be due to invalid IL or missing references)
		//IL_01df: Unknown result type (might be due to invalid IL or missing references)
		//IL_01e4: Unknown result type (might be due to invalid IL or missing references)
		//IL_01f1: Unknown result type (might be due to invalid IL or missing references)
		//IL_01f6: Unknown result type (might be due to invalid IL or missing references)
		//IL_0205: Unknown result type (might be due to invalid IL or missing references)
		//IL_0236: Unknown result type (might be due to invalid IL or missing references)
		//IL_023b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0248: Unknown result type (might be due to invalid IL or missing references)
		//IL_024d: Unknown result type (might be due to invalid IL or missing references)
		//IL_025e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0271: Unknown result type (might be due to invalid IL or missing references)
		//IL_0276: Unknown result type (might be due to invalid IL or missing references)
		//IL_0283: Unknown result type (might be due to invalid IL or missing references)
		//IL_0288: Unknown result type (might be due to invalid IL or missing references)
		//IL_0299: Unknown result type (might be due to invalid IL or missing references)
		//IL_02ac: Unknown result type (might be due to invalid IL or missing references)
		//IL_02b1: Unknown result type (might be due to invalid IL or missing references)
		//IL_02be: Unknown result type (might be due to invalid IL or missing references)
		//IL_02c3: Unknown result type (might be due to invalid IL or missing references)
		//IL_02d4: Unknown result type (might be due to invalid IL or missing references)
		//IL_02e7: Unknown result type (might be due to invalid IL or missing references)
		//IL_02ec: Unknown result type (might be due to invalid IL or missing references)
		//IL_02f9: Unknown result type (might be due to invalid IL or missing references)
		//IL_02fe: Unknown result type (might be due to invalid IL or missing references)
		//IL_030f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0340: Unknown result type (might be due to invalid IL or missing references)
		//IL_0345: Unknown result type (might be due to invalid IL or missing references)
		//IL_0352: Unknown result type (might be due to invalid IL or missing references)
		//IL_0357: Unknown result type (might be due to invalid IL or missing references)
		//IL_0368: Unknown result type (might be due to invalid IL or missing references)
		//IL_037b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0380: Unknown result type (might be due to invalid IL or missing references)
		//IL_038d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0392: Unknown result type (might be due to invalid IL or missing references)
		//IL_03a3: Unknown result type (might be due to invalid IL or missing references)
		//IL_03b6: Unknown result type (might be due to invalid IL or missing references)
		//IL_03bb: Unknown result type (might be due to invalid IL or missing references)
		//IL_03c8: Unknown result type (might be due to invalid IL or missing references)
		//IL_03cd: Unknown result type (might be due to invalid IL or missing references)
		//IL_03de: Unknown result type (might be due to invalid IL or missing references)
		//IL_03f1: Unknown result type (might be due to invalid IL or missing references)
		//IL_03f6: Unknown result type (might be due to invalid IL or missing references)
		//IL_0403: Unknown result type (might be due to invalid IL or missing references)
		//IL_0408: Unknown result type (might be due to invalid IL or missing references)
		//IL_0419: Unknown result type (might be due to invalid IL or missing references)
		//IL_04d0: Unknown result type (might be due to invalid IL or missing references)
		//IL_04d5: Unknown result type (might be due to invalid IL or missing references)
		//IL_04e2: Unknown result type (might be due to invalid IL or missing references)
		//IL_04e7: Unknown result type (might be due to invalid IL or missing references)
		//IL_050c: Unknown result type (might be due to invalid IL or missing references)
		//IL_051f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0524: Unknown result type (might be due to invalid IL or missing references)
		//IL_0531: Unknown result type (might be due to invalid IL or missing references)
		//IL_0536: Unknown result type (might be due to invalid IL or missing references)
		//IL_055c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0474: Unknown result type (might be due to invalid IL or missing references)
		//IL_0479: Unknown result type (might be due to invalid IL or missing references)
		//IL_048a: Unknown result type (might be due to invalid IL or missing references)
		//IL_04a6: Unknown result type (might be due to invalid IL or missing references)
		//IL_04ab: Unknown result type (might be due to invalid IL or missing references)
		//IL_04bc: Unknown result type (might be due to invalid IL or missing references)
		((RhinoList<Point3d>)(object)_vertices).Clear();
		double num = 0.6;
		double num2 = 0.8;
		double num3 = 1.0;
		Point3dList vertices = _vertices;
		double num4 = num;
		Point3d val = ((RhinoList<Point3d>)(object)profilePoints)[1];
		double num5 = num4 * ((Point3d)(ref val)).X;
		double num6 = num2;
		val = ((RhinoList<Point3d>)(object)profilePoints)[1];
		((RhinoList<Point3d>)(object)vertices).Add(new Point3d(num5, num6 * ((Point3d)(ref val)).Y, num3 * topThickness + halfGirdleThickness));
		Point3dList vertices2 = _vertices;
		double num7 = num;
		val = ((RhinoList<Point3d>)(object)profilePoints)[2];
		double num8 = num7 * ((Point3d)(ref val)).X;
		double num9 = num2;
		val = ((RhinoList<Point3d>)(object)profilePoints)[2];
		((RhinoList<Point3d>)(object)vertices2).Add(new Point3d(num8, num9 * ((Point3d)(ref val)).Y, num3 * topThickness + halfGirdleThickness));
		Point3dList vertices3 = _vertices;
		double num10 = num;
		val = ((RhinoList<Point3d>)(object)profilePoints)[3];
		double num11 = num10 * ((Point3d)(ref val)).X;
		double num12 = num2;
		val = ((RhinoList<Point3d>)(object)profilePoints)[3];
		((RhinoList<Point3d>)(object)vertices3).Add(new Point3d(num11, num12 * ((Point3d)(ref val)).Y, num3 * topThickness + halfGirdleThickness));
		Point3dList vertices4 = _vertices;
		double num13 = num;
		val = ((RhinoList<Point3d>)(object)profilePoints)[4];
		double num14 = num13 * ((Point3d)(ref val)).X;
		double num15 = num2;
		val = ((RhinoList<Point3d>)(object)profilePoints)[4];
		((RhinoList<Point3d>)(object)vertices4).Add(new Point3d(num14, num15 * ((Point3d)(ref val)).Y, num3 * topThickness + halfGirdleThickness));
		num = 1.0;
		num2 = 1.0;
		num3 = 0.0;
		Point3dList vertices5 = _vertices;
		double num16 = num;
		val = ((RhinoList<Point3d>)(object)profilePoints)[1];
		double num17 = num16 * ((Point3d)(ref val)).X;
		double num18 = num2;
		val = ((RhinoList<Point3d>)(object)profilePoints)[1];
		((RhinoList<Point3d>)(object)vertices5).Add(new Point3d(num17, num18 * ((Point3d)(ref val)).Y, num3 * topThickness + halfGirdleThickness));
		Point3dList vertices6 = _vertices;
		double num19 = num;
		val = ((RhinoList<Point3d>)(object)profilePoints)[2];
		double num20 = num19 * ((Point3d)(ref val)).X;
		double num21 = num2;
		val = ((RhinoList<Point3d>)(object)profilePoints)[2];
		((RhinoList<Point3d>)(object)vertices6).Add(new Point3d(num20, num21 * ((Point3d)(ref val)).Y, num3 * topThickness + halfGirdleThickness));
		Point3dList vertices7 = _vertices;
		double num22 = num;
		val = ((RhinoList<Point3d>)(object)profilePoints)[3];
		double num23 = num22 * ((Point3d)(ref val)).X;
		double num24 = num2;
		val = ((RhinoList<Point3d>)(object)profilePoints)[3];
		((RhinoList<Point3d>)(object)vertices7).Add(new Point3d(num23, num24 * ((Point3d)(ref val)).Y, num3 * topThickness + halfGirdleThickness));
		Point3dList vertices8 = _vertices;
		double num25 = num;
		val = ((RhinoList<Point3d>)(object)profilePoints)[4];
		double num26 = num25 * ((Point3d)(ref val)).X;
		double num27 = num2;
		val = ((RhinoList<Point3d>)(object)profilePoints)[4];
		((RhinoList<Point3d>)(object)vertices8).Add(new Point3d(num26, num27 * ((Point3d)(ref val)).Y, num3 * topThickness + halfGirdleThickness));
		num = 1.0;
		num2 = 1.0;
		num3 = 0.0;
		Point3dList vertices9 = _vertices;
		double num28 = num;
		val = ((RhinoList<Point3d>)(object)profilePoints)[1];
		double num29 = num28 * ((Point3d)(ref val)).X;
		double num30 = num2;
		val = ((RhinoList<Point3d>)(object)profilePoints)[1];
		((RhinoList<Point3d>)(object)vertices9).Add(new Point3d(num29, num30 * ((Point3d)(ref val)).Y, 0.0 - (num3 * bottomThickness + halfGirdleThickness)));
		Point3dList vertices10 = _vertices;
		double num31 = num;
		val = ((RhinoList<Point3d>)(object)profilePoints)[2];
		double num32 = num31 * ((Point3d)(ref val)).X;
		double num33 = num2;
		val = ((RhinoList<Point3d>)(object)profilePoints)[2];
		((RhinoList<Point3d>)(object)vertices10).Add(new Point3d(num32, num33 * ((Point3d)(ref val)).Y, 0.0 - (num3 * bottomThickness + halfGirdleThickness)));
		Point3dList vertices11 = _vertices;
		double num34 = num;
		val = ((RhinoList<Point3d>)(object)profilePoints)[3];
		double num35 = num34 * ((Point3d)(ref val)).X;
		double num36 = num2;
		val = ((RhinoList<Point3d>)(object)profilePoints)[3];
		((RhinoList<Point3d>)(object)vertices11).Add(new Point3d(num35, num36 * ((Point3d)(ref val)).Y, 0.0 - (num3 * bottomThickness + halfGirdleThickness)));
		Point3dList vertices12 = _vertices;
		double num37 = num;
		val = ((RhinoList<Point3d>)(object)profilePoints)[4];
		double num38 = num37 * ((Point3d)(ref val)).X;
		double num39 = num2;
		val = ((RhinoList<Point3d>)(object)profilePoints)[4];
		((RhinoList<Point3d>)(object)vertices12).Add(new Point3d(num38, num39 * ((Point3d)(ref val)).Y, 0.0 - (num3 * bottomThickness + halfGirdleThickness)));
		num = 0.6;
		num2 = 0.8;
		num3 = 0.5;
		Point3dList vertices13 = _vertices;
		double num40 = num;
		val = ((RhinoList<Point3d>)(object)profilePoints)[1];
		double num41 = num40 * ((Point3d)(ref val)).X;
		double num42 = num2;
		val = ((RhinoList<Point3d>)(object)profilePoints)[1];
		((RhinoList<Point3d>)(object)vertices13).Add(new Point3d(num41, num42 * ((Point3d)(ref val)).Y, 0.0 - (num3 * bottomThickness + halfGirdleThickness)));
		Point3dList vertices14 = _vertices;
		double num43 = num;
		val = ((RhinoList<Point3d>)(object)profilePoints)[2];
		double num44 = num43 * ((Point3d)(ref val)).X;
		double num45 = num2;
		val = ((RhinoList<Point3d>)(object)profilePoints)[2];
		((RhinoList<Point3d>)(object)vertices14).Add(new Point3d(num44, num45 * ((Point3d)(ref val)).Y, 0.0 - (num3 * bottomThickness + halfGirdleThickness)));
		Point3dList vertices15 = _vertices;
		double num46 = num;
		val = ((RhinoList<Point3d>)(object)profilePoints)[3];
		double num47 = num46 * ((Point3d)(ref val)).X;
		double num48 = num2;
		val = ((RhinoList<Point3d>)(object)profilePoints)[3];
		((RhinoList<Point3d>)(object)vertices15).Add(new Point3d(num47, num48 * ((Point3d)(ref val)).Y, 0.0 - (num3 * bottomThickness + halfGirdleThickness)));
		Point3dList vertices16 = _vertices;
		double num49 = num;
		val = ((RhinoList<Point3d>)(object)profilePoints)[4];
		double num50 = num49 * ((Point3d)(ref val)).X;
		double num51 = num2;
		val = ((RhinoList<Point3d>)(object)profilePoints)[4];
		((RhinoList<Point3d>)(object)vertices16).Add(new Point3d(num50, num51 * ((Point3d)(ref val)).Y, 0.0 - (num3 * bottomThickness + halfGirdleThickness)));
		num = 0.5;
		num2 = 0.5;
		num3 = 1.0;
		double num52 = (_widthBottom + _widthTop) / 2.0;
		if (_length > num52)
		{
			Point3dList vertices17 = _vertices;
			double num53 = num2;
			val = ((RhinoList<Point3d>)(object)profilePoints)[1];
			((RhinoList<Point3d>)(object)vertices17).Add(new Point3d(0.0, num53 * ((Point3d)(ref val)).Y, 0.0 - (num3 * bottomThickness + halfGirdleThickness)));
			Point3dList vertices18 = _vertices;
			double num54 = num2;
			val = ((RhinoList<Point3d>)(object)profilePoints)[3];
			((RhinoList<Point3d>)(object)vertices18).Add(new Point3d(0.0, num54 * ((Point3d)(ref val)).Y, 0.0 - (num3 * bottomThickness + halfGirdleThickness)));
		}
		else
		{
			Point3dList vertices19 = _vertices;
			double num55 = num;
			val = ((RhinoList<Point3d>)(object)profilePoints)[1];
			double num56 = num55 * ((Point3d)(ref val)).X;
			double num57 = num;
			val = ((RhinoList<Point3d>)(object)profilePoints)[2];
			((RhinoList<Point3d>)(object)vertices19).Add(new Point3d((num56 + num57 * ((Point3d)(ref val)).X) / 2.0, 0.0, 0.0 - (num3 * bottomThickness + halfGirdleThickness)));
			Point3dList vertices20 = _vertices;
			double num58 = num;
			val = ((RhinoList<Point3d>)(object)profilePoints)[1];
			double num59 = num58 * ((Point3d)(ref val)).X;
			double num60 = num;
			val = ((RhinoList<Point3d>)(object)profilePoints)[2];
			((RhinoList<Point3d>)(object)vertices20).Add(new Point3d(0.0 - (num59 + num60 * ((Point3d)(ref val)).X) / 2.0, 0.0, 0.0 - (num3 * bottomThickness + halfGirdleThickness)));
		}
	}

	private void CalculateFaceCornerSets()
	{
		_faceCornerSets.Clear();
		_faceCornerSets.Add(new int[4] { 3, 2, 1, 0 });
		_faceCornerSets.Add(new int[4] { 0, 4, 7, 3 });
		_faceCornerSets.Add(new int[4] { 1, 5, 4, 0 });
		_faceCornerSets.Add(new int[4] { 2, 6, 5, 1 });
		_faceCornerSets.Add(new int[4] { 3, 7, 6, 2 });
		_faceCornerSets.Add(new int[4] { 4, 8, 11, 7 });
		_faceCornerSets.Add(new int[4] { 5, 9, 8, 4 });
		_faceCornerSets.Add(new int[4] { 6, 10, 9, 5 });
		_faceCornerSets.Add(new int[4] { 7, 11, 10, 6 });
		_faceCornerSets.Add(new int[4] { 8, 12, 15, 11 });
		_faceCornerSets.Add(new int[4] { 9, 13, 12, 8 });
		_faceCornerSets.Add(new int[4] { 10, 14, 13, 9 });
		_faceCornerSets.Add(new int[4] { 11, 15, 14, 10 });
		double num = (_widthBottom + _widthTop) / 2.0;
		if (_length > num)
		{
			_faceCornerSets.Add(new int[3] { 12, 16, 15 });
			_faceCornerSets.Add(new int[4] { 13, 17, 16, 12 });
			_faceCornerSets.Add(new int[3] { 14, 17, 13 });
			_faceCornerSets.Add(new int[4] { 15, 16, 17, 14 });
		}
		else
		{
			_faceCornerSets.Add(new int[4] { 12, 16, 17, 15 });
			_faceCornerSets.Add(new int[3] { 13, 16, 12 });
			_faceCornerSets.Add(new int[4] { 14, 17, 16, 13 });
			_faceCornerSets.Add(new int[3] { 15, 17, 14 });
		}
	}

	private void Baguette()
	{
		//IL_00b4: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b9: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d2: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e3: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f5: Unknown result type (might be due to invalid IL or missing references)
		//IL_0108: Unknown result type (might be due to invalid IL or missing references)
		//IL_011a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0135: Expected O, but got Unknown
		//IL_0138: Unknown result type (might be due to invalid IL or missing references)
		//IL_014e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0153: Unknown result type (might be due to invalid IL or missing references)
		//IL_016a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0172: Unknown result type (might be due to invalid IL or missing references)
		//IL_0186: Unknown result type (might be due to invalid IL or missing references)
		//IL_018e: Unknown result type (might be due to invalid IL or missing references)
		//IL_01a2: Unknown result type (might be due to invalid IL or missing references)
		//IL_01aa: Unknown result type (might be due to invalid IL or missing references)
		//IL_01be: Unknown result type (might be due to invalid IL or missing references)
		//IL_01c6: Unknown result type (might be due to invalid IL or missing references)
		//IL_01da: Unknown result type (might be due to invalid IL or missing references)
		//IL_01e2: Unknown result type (might be due to invalid IL or missing references)
		//IL_0208: Unknown result type (might be due to invalid IL or missing references)
		//IL_020d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0215: Unknown result type (might be due to invalid IL or missing references)
		double num = _length / 2.0;
		double num2 = _widthBottom / 2.0;
		double num3 = _widthTop / 2.0;
		double num4 = Math.Max(num3, num2);
		double num5 = _girdleThickness / 2.0;
		double num6 = (_thickness - _girdleThickness) * (1.0 / 3.0);
		double num7 = (_thickness - _girdleThickness) * (2.0 / 3.0);
		_minX = 0.0 - num4;
		_maxX = num4;
		_minY = 0.0 - num;
		_maxY = num;
		_minZ = 0.0 - (num7 + num5);
		_maxZ = num6 + num5;
		Point3dList val = new Point3dList();
		val.Add(0.0, num, 0.0);
		val.Add(num3, num, 0.0);
		val.Add(num2, 0.0 - num, 0.0);
		val.Add(0.0 - num2, 0.0 - num, 0.0);
		val.Add(0.0 - num3, num, 0.0);
		val.Add(0.0, num, 0.0);
		Point3dList val2 = val;
		_curve = ((Curve)new PolylineCurve((IEnumerable<Point3d>)val2)).ToNurbsCurve();
		Interval domain = ((Curve)_curve).Domain;
		_bezelParameter = ((Interval)(ref domain)).Mid;
		List<Point3d> list = new List<Point3d>();
		list.AddRange(CSUtilCustomGems.CalculateProngPoints(((RhinoList<Point3d>)(object)val2)[0], ((RhinoList<Point3d>)(object)val2)[1], 3));
		list.AddRange(CSUtilCustomGems.CalculateProngPoints(((RhinoList<Point3d>)(object)val2)[1], ((RhinoList<Point3d>)(object)val2)[2], 6));
		list.AddRange(CSUtilCustomGems.CalculateProngPoints(((RhinoList<Point3d>)(object)val2)[2], ((RhinoList<Point3d>)(object)val2)[3], 6));
		list.AddRange(CSUtilCustomGems.CalculateProngPoints(((RhinoList<Point3d>)(object)val2)[3], ((RhinoList<Point3d>)(object)val2)[4], 6));
		list.AddRange(CSUtilCustomGems.CalculateProngPoints(((RhinoList<Point3d>)(object)val2)[4], ((RhinoList<Point3d>)(object)val2)[5], 3));
		base.ProngPositions.Clear();
		double prongParam = default(double);
		foreach (Point3d item in list)
		{
			((Curve)_curve).ClosestPoint(item, ref prongParam);
			base.ProngPositions.Add(new ProngPosition(1, prongParam));
		}
		CalculateVertices(val2, num5, num6, num7);
		CalculateFaceCornerSets();
		_mesh = GetMesh();
	}
}
