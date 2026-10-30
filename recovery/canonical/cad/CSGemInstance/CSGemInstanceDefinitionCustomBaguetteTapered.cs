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
		gemInstanceDefinition = null;
		GeometryBase brep = GetBrep();
		int num = RhinoDoc.ActiveDoc.Layers.Find("Gem", ignoreDeletedLayers: true);
		if (num == -1)
		{
			Layer layer = new Layer
			{
				Name = "Gem",
				Color = Color.FromArgb(0, 0, 128)
			};
			num = RhinoDoc.ActiveDoc.Layers.Add(layer);
			if (num == -1)
			{
				num = RhinoDoc.ActiveDoc.Layers.CurrentLayerIndex;
			}
		}
		ObjectAttributes attributes = new ObjectAttributes
		{
			ColorSource = ObjectColorSource.ColorFromParent,
			LayerIndex = num,
			Name = _shape,
			MaterialSource = ObjectMaterialSource.MaterialFromParent,
			PlotColorSource = ObjectPlotColorSource.PlotColorFromParent,
			PlotWeightSource = ObjectPlotWeightSource.PlotWeightFromParent
		};
		Guid guid = RhinoDoc.ActiveDoc.Objects.Add(brep, attributes);
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
		RhinoObject rhinoObject = RhinoDoc.ActiveDoc.Objects.Find(guid);
		int num2 = RhinoDoc.ActiveDoc.InstanceDefinitions.Add(BlockName, string.Empty, Point3d.Origin, rhinoObject.Geometry, attributes);
		RhinoDoc.ActiveDoc.Objects.Delete(guid, quiet: true);
		if (num2 >= 0)
		{
			gemInstanceDefinition = RhinoDoc.ActiveDoc.InstanceDefinitions[num2];
		}
		return gemInstanceDefinition != null;
	}

	private void CalculateVertices(Point3dList profilePoints, double halfGirdleThickness, double topThickness, double bottomThickness)
	{
		_vertices.Clear();
		double num = 0.6;
		double num2 = 0.8;
		double num3 = 1.0;
		_vertices.Add(new Point3d(num * profilePoints[1].X, num2 * profilePoints[1].Y, num3 * topThickness + halfGirdleThickness));
		_vertices.Add(new Point3d(num * profilePoints[2].X, num2 * profilePoints[2].Y, num3 * topThickness + halfGirdleThickness));
		_vertices.Add(new Point3d(num * profilePoints[3].X, num2 * profilePoints[3].Y, num3 * topThickness + halfGirdleThickness));
		_vertices.Add(new Point3d(num * profilePoints[4].X, num2 * profilePoints[4].Y, num3 * topThickness + halfGirdleThickness));
		num = 1.0;
		num2 = 1.0;
		num3 = 0.0;
		_vertices.Add(new Point3d(num * profilePoints[1].X, num2 * profilePoints[1].Y, num3 * topThickness + halfGirdleThickness));
		_vertices.Add(new Point3d(num * profilePoints[2].X, num2 * profilePoints[2].Y, num3 * topThickness + halfGirdleThickness));
		_vertices.Add(new Point3d(num * profilePoints[3].X, num2 * profilePoints[3].Y, num3 * topThickness + halfGirdleThickness));
		_vertices.Add(new Point3d(num * profilePoints[4].X, num2 * profilePoints[4].Y, num3 * topThickness + halfGirdleThickness));
		num = 1.0;
		num2 = 1.0;
		num3 = 0.0;
		_vertices.Add(new Point3d(num * profilePoints[1].X, num2 * profilePoints[1].Y, 0.0 - (num3 * bottomThickness + halfGirdleThickness)));
		_vertices.Add(new Point3d(num * profilePoints[2].X, num2 * profilePoints[2].Y, 0.0 - (num3 * bottomThickness + halfGirdleThickness)));
		_vertices.Add(new Point3d(num * profilePoints[3].X, num2 * profilePoints[3].Y, 0.0 - (num3 * bottomThickness + halfGirdleThickness)));
		_vertices.Add(new Point3d(num * profilePoints[4].X, num2 * profilePoints[4].Y, 0.0 - (num3 * bottomThickness + halfGirdleThickness)));
		num = 0.6;
		num2 = 0.8;
		num3 = 0.5;
		_vertices.Add(new Point3d(num * profilePoints[1].X, num2 * profilePoints[1].Y, 0.0 - (num3 * bottomThickness + halfGirdleThickness)));
		_vertices.Add(new Point3d(num * profilePoints[2].X, num2 * profilePoints[2].Y, 0.0 - (num3 * bottomThickness + halfGirdleThickness)));
		_vertices.Add(new Point3d(num * profilePoints[3].X, num2 * profilePoints[3].Y, 0.0 - (num3 * bottomThickness + halfGirdleThickness)));
		_vertices.Add(new Point3d(num * profilePoints[4].X, num2 * profilePoints[4].Y, 0.0 - (num3 * bottomThickness + halfGirdleThickness)));
		num = 0.5;
		num2 = 0.5;
		num3 = 1.0;
		double num4 = (_widthBottom + _widthTop) / 2.0;
		if (_length > num4)
		{
			_vertices.Add(new Point3d(0.0, num2 * profilePoints[1].Y, 0.0 - (num3 * bottomThickness + halfGirdleThickness)));
			_vertices.Add(new Point3d(0.0, num2 * profilePoints[3].Y, 0.0 - (num3 * bottomThickness + halfGirdleThickness)));
		}
		else
		{
			_vertices.Add(new Point3d((num * profilePoints[1].X + num * profilePoints[2].X) / 2.0, 0.0, 0.0 - (num3 * bottomThickness + halfGirdleThickness)));
			_vertices.Add(new Point3d(0.0 - (num * profilePoints[1].X + num * profilePoints[2].X) / 2.0, 0.0, 0.0 - (num3 * bottomThickness + halfGirdleThickness)));
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
		Point3dList point3dList = new Point3dList
		{
			{ 0.0, num, 0.0 },
			{ num3, num, 0.0 },
			{
				num2,
				0.0 - num,
				0.0
			},
			{
				0.0 - num2,
				0.0 - num,
				0.0
			},
			{
				0.0 - num3,
				num,
				0.0
			},
			{ 0.0, num, 0.0 }
		};
		_curve = new PolylineCurve(point3dList).ToNurbsCurve();
		_bezelParameter = _curve.Domain.Mid;
		List<Point3d> list = new List<Point3d>();
		list.AddRange(CSUtilCustomGems.CalculateProngPoints(point3dList[0], point3dList[1], 3));
		list.AddRange(CSUtilCustomGems.CalculateProngPoints(point3dList[1], point3dList[2], 6));
		list.AddRange(CSUtilCustomGems.CalculateProngPoints(point3dList[2], point3dList[3], 6));
		list.AddRange(CSUtilCustomGems.CalculateProngPoints(point3dList[3], point3dList[4], 6));
		list.AddRange(CSUtilCustomGems.CalculateProngPoints(point3dList[4], point3dList[5], 3));
		base.ProngPositions.Clear();
		foreach (Point3d item in list)
		{
			_curve.ClosestPoint(item, out var t);
			base.ProngPositions.Add(new ProngPosition(1, t));
		}
		CalculateVertices(point3dList, num5, num6, num7);
		CalculateFaceCornerSets();
		_mesh = GetMesh();
	}
}
