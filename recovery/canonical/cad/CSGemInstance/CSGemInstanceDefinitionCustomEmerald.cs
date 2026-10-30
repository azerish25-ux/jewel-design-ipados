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

public sealed class CSGemInstanceDefinitionCustomEmerald : CSGemInstanceDefinitionCustom
{
	private readonly double _length;

	private readonly double _width;

	private readonly double _thickness;

	private readonly double _girdleThickness;

	private readonly double _corner;

	public override string BlockName { get; }

	public CSGemInstanceDefinitionCustomEmerald(double length, double width, double thickness, double girdleThickness, double corner, bool useOldShapeName = false)
	{
		_length = length;
		_width = width;
		_thickness = thickness;
		_girdleThickness = girdleThickness;
		_corner = corner;
		_gemTableType = GemTableType.Diamond;
		_gemTableShape = GemTableShape.EmeraldCustom;
		_gemTableLayer = GemTableHelper.GetLayer(_gemTableType, _gemTableShape);
		_gemTableVersion = GemTableHelper.GetCurrentGemTableVersion(_gemTableType, _gemTableShape);
		_type = GemTableHelper.GetDescriptionFromValue(_gemTableType);
		_shape = (useOldShapeName ? $"Emerald_Length{length}_Width{width}_Depth{thickness}_GirdleThickness{girdleThickness}_Corner{corner}" : GemTableHelper.GetDescriptionFromValue(_gemTableShape));
		_layer = GemTableHelper.GetDescriptionFromValue(_gemTableLayer);
		_fudgefactor = GemTableHelper.GetGem(_gemTableType, _gemTableShape).FudgeFactor;
		base.Sizes.Clear();
		base.Sizes.AddRange(GemTableHelper.GetGem(_gemTableType, _gemTableShape).SizeTable());
		_versionMajor = CSGemInstanceDefinitionHelper.CurrentVersionMajor;
		_versionMinor = CSGemInstanceDefinitionHelper.CurrentVersionMinor;
		_isGeneratedGem = true;
		BlockName = $"Custom_Emerald_Length{_length}_Width{_width}_Height{_thickness}_Corner{_corner}_Girdle{_girdleThickness}";
		Emerald();
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
		RhinoScriptInterface.SetObjectData(guid, "MatrixICGem", "GemShape", "Emerald");
		RhinoScriptInterface.SetObjectData(guid, "MatrixICGem", "Version", "1.1");
		RhinoScriptInterface.SetObjectData(guid, "MatrixICGem", "GemLayer", _layer);
		RhinoScriptInterface.SetObjectData(guid, "MatrixICGem", "GemXMin", _minX);
		RhinoScriptInterface.SetObjectData(guid, "MatrixICGem", "GemYMin", _minY);
		RhinoScriptInterface.SetObjectData(guid, "MatrixICGem", "GemZMin", _minZ);
		RhinoScriptInterface.SetObjectData(guid, "MatrixICGem", "GemXMax", _maxX);
		RhinoScriptInterface.SetObjectData(guid, "MatrixICGem", "GemYMax", _maxY);
		RhinoScriptInterface.SetObjectData(guid, "MatrixICGem", "GemZMax", _maxZ);
		RhinoScriptInterface.SetObjectData(guid, "MatrixICGem", "FFactor", _fudgefactor);
		RhinoScriptInterface.SetObjectData(guid, "MatrixICGem", "HeightPercent", Math.Round(_length / _width));
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
		RhinoScriptInterface.SetObjectData(guid, "MatrixICGem", "Corner", _corner);
		RhinoScriptInterface.SetObjectData(guid, "MatrixICGem", "Height", _thickness);
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

	private void CalculateFaceCornerSets()
	{
		_faceCornerSets.Clear();
		if (_length > _width)
		{
			_faceCornerSets.Add(new int[4] { 0, 3, 2, 1 });
			_faceCornerSets.Add(new int[4] { 7, 4, 3, 0 });
			_faceCornerSets.Add(new int[4] { 6, 5, 4, 7 });
		}
		else
		{
			_faceCornerSets.Add(new int[4] { 6, 1, 0, 7 });
			_faceCornerSets.Add(new int[4] { 5, 2, 1, 6 });
			_faceCornerSets.Add(new int[4] { 4, 3, 2, 5 });
		}
		_faceCornerSets.Add(new int[4] { 0, 8, 15, 7 });
		_faceCornerSets.Add(new int[4] { 1, 9, 8, 0 });
		_faceCornerSets.Add(new int[4] { 2, 10, 9, 1 });
		_faceCornerSets.Add(new int[4] { 3, 11, 10, 2 });
		_faceCornerSets.Add(new int[4] { 4, 12, 11, 3 });
		_faceCornerSets.Add(new int[4] { 5, 13, 12, 4 });
		_faceCornerSets.Add(new int[4] { 6, 14, 13, 5 });
		_faceCornerSets.Add(new int[4] { 7, 15, 14, 6 });
		_faceCornerSets.Add(new int[4] { 8, 16, 23, 15 });
		_faceCornerSets.Add(new int[4] { 9, 17, 16, 8 });
		_faceCornerSets.Add(new int[4] { 10, 18, 17, 9 });
		_faceCornerSets.Add(new int[4] { 11, 19, 18, 10 });
		_faceCornerSets.Add(new int[4] { 12, 20, 19, 11 });
		_faceCornerSets.Add(new int[4] { 13, 21, 20, 12 });
		_faceCornerSets.Add(new int[4] { 14, 22, 21, 13 });
		_faceCornerSets.Add(new int[4] { 15, 23, 22, 14 });
		_faceCornerSets.Add(new int[4] { 16, 24, 31, 23 });
		_faceCornerSets.Add(new int[4] { 17, 25, 24, 16 });
		_faceCornerSets.Add(new int[4] { 18, 26, 25, 17 });
		_faceCornerSets.Add(new int[4] { 19, 27, 26, 18 });
		_faceCornerSets.Add(new int[4] { 20, 28, 27, 19 });
		_faceCornerSets.Add(new int[4] { 21, 29, 28, 20 });
		_faceCornerSets.Add(new int[4] { 22, 30, 29, 21 });
		_faceCornerSets.Add(new int[4] { 23, 31, 30, 22 });
		_faceCornerSets.Add(new int[4] { 24, 32, 39, 31 });
		_faceCornerSets.Add(new int[4] { 25, 33, 32, 24 });
		_faceCornerSets.Add(new int[4] { 26, 34, 33, 25 });
		_faceCornerSets.Add(new int[4] { 27, 35, 34, 26 });
		_faceCornerSets.Add(new int[4] { 28, 36, 35, 27 });
		_faceCornerSets.Add(new int[4] { 29, 37, 36, 28 });
		_faceCornerSets.Add(new int[4] { 30, 38, 37, 29 });
		_faceCornerSets.Add(new int[4] { 31, 39, 38, 30 });
		_faceCornerSets.Add(new int[4] { 32, 40, 47, 39 });
		_faceCornerSets.Add(new int[4] { 33, 41, 40, 32 });
		_faceCornerSets.Add(new int[4] { 34, 42, 41, 33 });
		_faceCornerSets.Add(new int[4] { 35, 43, 42, 34 });
		_faceCornerSets.Add(new int[4] { 36, 44, 43, 35 });
		_faceCornerSets.Add(new int[4] { 37, 45, 44, 36 });
		_faceCornerSets.Add(new int[4] { 38, 46, 45, 37 });
		_faceCornerSets.Add(new int[4] { 39, 47, 46, 38 });
		if (_length > _width)
		{
			_faceCornerSets.Add(new int[3] { 40, 48, 47 });
			_faceCornerSets.Add(new int[3] { 41, 48, 40 });
			_faceCornerSets.Add(new int[4] { 42, 49, 48, 41 });
			_faceCornerSets.Add(new int[3] { 43, 49, 42 });
			_faceCornerSets.Add(new int[3] { 44, 49, 43 });
			_faceCornerSets.Add(new int[3] { 45, 49, 44 });
			_faceCornerSets.Add(new int[4] { 46, 48, 49, 45 });
			_faceCornerSets.Add(new int[3] { 47, 48, 46 });
		}
		else
		{
			_faceCornerSets.Add(new int[4] { 40, 48, 49, 47 });
			_faceCornerSets.Add(new int[3] { 41, 48, 40 });
			_faceCornerSets.Add(new int[3] { 42, 48, 41 });
			_faceCornerSets.Add(new int[3] { 43, 48, 42 });
			_faceCornerSets.Add(new int[4] { 44, 49, 48, 43 });
			_faceCornerSets.Add(new int[3] { 45, 49, 44 });
			_faceCornerSets.Add(new int[3] { 46, 49, 45 });
			_faceCornerSets.Add(new int[3] { 47, 49, 46 });
		}
	}

	private void CalculateVertices(Point3dList profilePoints, double halfGirdleThickness, double topThickness, double bottomThickness)
	{
		_vertices.Clear();
		double num = 0.53;
		double num2 = 0.68;
		double num3 = 1.0;
		_vertices.Add(new Point3d(num * profilePoints[1].X, num2 * profilePoints[1].Y, num3 * topThickness + halfGirdleThickness));
		_vertices.Add(new Point3d(num * profilePoints[2].X, num2 * profilePoints[2].Y, num3 * topThickness + halfGirdleThickness));
		_vertices.Add(new Point3d(num * profilePoints[3].X, num2 * profilePoints[3].Y, num3 * topThickness + halfGirdleThickness));
		_vertices.Add(new Point3d(num * profilePoints[4].X, num2 * profilePoints[4].Y, num3 * topThickness + halfGirdleThickness));
		_vertices.Add(new Point3d(num * profilePoints[5].X, num2 * profilePoints[5].Y, num3 * topThickness + halfGirdleThickness));
		_vertices.Add(new Point3d(num * profilePoints[6].X, num2 * profilePoints[6].Y, num3 * topThickness + halfGirdleThickness));
		_vertices.Add(new Point3d(num * profilePoints[7].X, num2 * profilePoints[7].Y, num3 * topThickness + halfGirdleThickness));
		_vertices.Add(new Point3d(num * profilePoints[8].X, num2 * profilePoints[8].Y, num3 * topThickness + halfGirdleThickness));
		num = 0.67;
		num2 = 0.755;
		num3 = 0.9;
		_vertices.Add(new Point3d(num * profilePoints[1].X, num2 * profilePoints[1].Y, num3 * topThickness + halfGirdleThickness));
		_vertices.Add(new Point3d(num * profilePoints[2].X, num2 * profilePoints[2].Y, num3 * topThickness + halfGirdleThickness));
		_vertices.Add(new Point3d(num * profilePoints[3].X, num2 * profilePoints[3].Y, num3 * topThickness + halfGirdleThickness));
		_vertices.Add(new Point3d(num * profilePoints[4].X, num2 * profilePoints[4].Y, num3 * topThickness + halfGirdleThickness));
		_vertices.Add(new Point3d(num * profilePoints[5].X, num2 * profilePoints[5].Y, num3 * topThickness + halfGirdleThickness));
		_vertices.Add(new Point3d(num * profilePoints[6].X, num2 * profilePoints[6].Y, num3 * topThickness + halfGirdleThickness));
		_vertices.Add(new Point3d(num * profilePoints[7].X, num2 * profilePoints[7].Y, num3 * topThickness + halfGirdleThickness));
		_vertices.Add(new Point3d(num * profilePoints[8].X, num2 * profilePoints[8].Y, num3 * topThickness + halfGirdleThickness));
		num = 0.83;
		num2 = 0.87;
		num3 = 0.6;
		_vertices.Add(new Point3d(num * profilePoints[1].X, num2 * profilePoints[1].Y, num3 * topThickness + halfGirdleThickness));
		_vertices.Add(new Point3d(num * profilePoints[2].X, num2 * profilePoints[2].Y, num3 * topThickness + halfGirdleThickness));
		_vertices.Add(new Point3d(num * profilePoints[3].X, num2 * profilePoints[3].Y, num3 * topThickness + halfGirdleThickness));
		_vertices.Add(new Point3d(num * profilePoints[4].X, num2 * profilePoints[4].Y, num3 * topThickness + halfGirdleThickness));
		_vertices.Add(new Point3d(num * profilePoints[5].X, num2 * profilePoints[5].Y, num3 * topThickness + halfGirdleThickness));
		_vertices.Add(new Point3d(num * profilePoints[6].X, num2 * profilePoints[6].Y, num3 * topThickness + halfGirdleThickness));
		_vertices.Add(new Point3d(num * profilePoints[7].X, num2 * profilePoints[7].Y, num3 * topThickness + halfGirdleThickness));
		_vertices.Add(new Point3d(num * profilePoints[8].X, num2 * profilePoints[8].Y, num3 * topThickness + halfGirdleThickness));
		num = 1.0;
		num2 = 1.0;
		num3 = 0.0;
		_vertices.Add(new Point3d(num * profilePoints[1].X, num2 * profilePoints[1].Y, num3 * topThickness + halfGirdleThickness));
		_vertices.Add(new Point3d(num * profilePoints[2].X, num2 * profilePoints[2].Y, num3 * topThickness + halfGirdleThickness));
		_vertices.Add(new Point3d(num * profilePoints[3].X, num2 * profilePoints[3].Y, num3 * topThickness + halfGirdleThickness));
		_vertices.Add(new Point3d(num * profilePoints[4].X, num2 * profilePoints[4].Y, num3 * topThickness + halfGirdleThickness));
		_vertices.Add(new Point3d(num * profilePoints[5].X, num2 * profilePoints[5].Y, num3 * topThickness + halfGirdleThickness));
		_vertices.Add(new Point3d(num * profilePoints[6].X, num2 * profilePoints[6].Y, num3 * topThickness + halfGirdleThickness));
		_vertices.Add(new Point3d(num * profilePoints[7].X, num2 * profilePoints[7].Y, num3 * topThickness + halfGirdleThickness));
		_vertices.Add(new Point3d(num * profilePoints[8].X, num2 * profilePoints[8].Y, num3 * topThickness + halfGirdleThickness));
		num = 1.0;
		num2 = 1.0;
		num3 = 0.0;
		_vertices.Add(new Point3d(num * profilePoints[1].X, num2 * profilePoints[1].Y, 0.0 - (num3 * bottomThickness + halfGirdleThickness)));
		_vertices.Add(new Point3d(num * profilePoints[2].X, num2 * profilePoints[2].Y, 0.0 - (num3 * bottomThickness + halfGirdleThickness)));
		_vertices.Add(new Point3d(num * profilePoints[3].X, num2 * profilePoints[3].Y, 0.0 - (num3 * bottomThickness + halfGirdleThickness)));
		_vertices.Add(new Point3d(num * profilePoints[4].X, num2 * profilePoints[4].Y, 0.0 - (num3 * bottomThickness + halfGirdleThickness)));
		_vertices.Add(new Point3d(num * profilePoints[5].X, num2 * profilePoints[5].Y, 0.0 - (num3 * bottomThickness + halfGirdleThickness)));
		_vertices.Add(new Point3d(num * profilePoints[6].X, num2 * profilePoints[6].Y, 0.0 - (num3 * bottomThickness + halfGirdleThickness)));
		_vertices.Add(new Point3d(num * profilePoints[7].X, num2 * profilePoints[7].Y, 0.0 - (num3 * bottomThickness + halfGirdleThickness)));
		_vertices.Add(new Point3d(num * profilePoints[8].X, num2 * profilePoints[8].Y, 0.0 - (num3 * bottomThickness + halfGirdleThickness)));
		num = 0.68;
		num2 = 0.745;
		num3 = 0.55;
		_vertices.Add(new Point3d(num * profilePoints[1].X, num2 * profilePoints[1].Y, 0.0 - (num3 * bottomThickness + halfGirdleThickness)));
		_vertices.Add(new Point3d(num * profilePoints[2].X, num2 * profilePoints[2].Y, 0.0 - (num3 * bottomThickness + halfGirdleThickness)));
		_vertices.Add(new Point3d(num * profilePoints[3].X, num2 * profilePoints[3].Y, 0.0 - (num3 * bottomThickness + halfGirdleThickness)));
		_vertices.Add(new Point3d(num * profilePoints[4].X, num2 * profilePoints[4].Y, 0.0 - (num3 * bottomThickness + halfGirdleThickness)));
		_vertices.Add(new Point3d(num * profilePoints[5].X, num2 * profilePoints[5].Y, 0.0 - (num3 * bottomThickness + halfGirdleThickness)));
		_vertices.Add(new Point3d(num * profilePoints[6].X, num2 * profilePoints[6].Y, 0.0 - (num3 * bottomThickness + halfGirdleThickness)));
		_vertices.Add(new Point3d(num * profilePoints[7].X, num2 * profilePoints[7].Y, 0.0 - (num3 * bottomThickness + halfGirdleThickness)));
		_vertices.Add(new Point3d(num * profilePoints[8].X, num2 * profilePoints[8].Y, 0.0 - (num3 * bottomThickness + halfGirdleThickness)));
		num = 0.5;
		num2 = 0.38;
		num3 = 1.0;
		if (_length > _width)
		{
			_vertices.Add(new Point3d(0.0, num2 * profilePoints[1].Y, 0.0 - (num3 * bottomThickness + halfGirdleThickness)));
			_vertices.Add(new Point3d(0.0, num2 * profilePoints[5].Y, 0.0 - (num3 * bottomThickness + halfGirdleThickness)));
		}
		else
		{
			_vertices.Add(new Point3d(num * profilePoints[2].X, 0.0, 0.0 - (num3 * bottomThickness + halfGirdleThickness)));
			_vertices.Add(new Point3d(num * profilePoints[6].X, 0.0, 0.0 - (num3 * bottomThickness + halfGirdleThickness)));
		}
	}

	private void Emerald()
	{
		double num = _length / 2.0;
		double num2 = _width / 2.0;
		double num3 = _girdleThickness / 2.0;
		double num4 = _corner * Math.Cos(Math.PI / 4.0);
		double num5 = (_thickness - _girdleThickness) * 0.29;
		double num6 = (_thickness - _girdleThickness) * 0.71;
		_minX = 0.0 - num2;
		_maxX = num2;
		_minY = 0.0 - num;
		_maxY = num;
		_minZ = 0.0 - (num6 + num3);
		_maxZ = num5 + num3;
		Point3dList point3dList = new Point3dList
		{
			{ 0.0, num, 0.0 },
			{
				num2 - num4,
				num,
				0.0
			},
			{
				num2,
				num - num4,
				0.0
			},
			{
				num2,
				0.0 - num + num4,
				0.0
			},
			{
				num2 - num4,
				0.0 - num,
				0.0
			},
			{
				0.0 - num2 + num4,
				0.0 - num,
				0.0
			},
			{
				0.0 - num2,
				0.0 - num + num4,
				0.0
			},
			{
				0.0 - num2,
				num - num4,
				0.0
			},
			{
				0.0 - num2 + num4,
				num,
				0.0
			},
			{ 0.0, num, 0.0 }
		};
		_curve = new PolylineCurve(point3dList).ToNurbsCurve();
		_bezelParameter = _curve.Domain.Mid;
		List<Point3d> list = new List<Point3d>();
		list.AddRange(CSUtilCustomGems.CalculateProngPoints(point3dList[0], point3dList[1], 2));
		list.AddRange(CSUtilCustomGems.CalculateProngPoints(point3dList[1], point3dList[2], 2));
		list.AddRange(CSUtilCustomGems.CalculateProngPoints(point3dList[2], point3dList[3], 4));
		list.AddRange(CSUtilCustomGems.CalculateProngPoints(point3dList[3], point3dList[4], 2));
		list.AddRange(CSUtilCustomGems.CalculateProngPoints(point3dList[4], point3dList[5], 4));
		list.AddRange(CSUtilCustomGems.CalculateProngPoints(point3dList[5], point3dList[6], 2));
		list.AddRange(CSUtilCustomGems.CalculateProngPoints(point3dList[6], point3dList[7], 4));
		list.AddRange(CSUtilCustomGems.CalculateProngPoints(point3dList[7], point3dList[8], 2));
		list.AddRange(CSUtilCustomGems.CalculateProngPoints(point3dList[8], point3dList[9], 2));
		base.ProngPositions.Clear();
		foreach (Point3d item in list)
		{
			_curve.ClosestPoint(item, out var t);
			base.ProngPositions.Add(new ProngPosition(1, t));
		}
		CalculateVertices(point3dList, num3, num5, num6);
		CalculateFaceCornerSets();
		_mesh = GetMesh();
	}
}
