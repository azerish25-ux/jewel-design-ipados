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
		//IL_045a: Unknown result type (might be due to invalid IL or missing references)
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
		RhinoObject val3 = RhinoDoc.ActiveDoc.Objects.Find(guid);
		int num2 = RhinoDoc.ActiveDoc.InstanceDefinitions.Add(BlockName, string.Empty, Point3d.Origin, val3.Geometry, val2);
		RhinoDoc.ActiveDoc.Objects.Delete(guid, true);
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
		//IL_0032: Unknown result type (might be due to invalid IL or missing references)
		//IL_0037: Unknown result type (might be due to invalid IL or missing references)
		//IL_0043: Unknown result type (might be due to invalid IL or missing references)
		//IL_0048: Unknown result type (might be due to invalid IL or missing references)
		//IL_0056: Unknown result type (might be due to invalid IL or missing references)
		//IL_0069: Unknown result type (might be due to invalid IL or missing references)
		//IL_006e: Unknown result type (might be due to invalid IL or missing references)
		//IL_007a: Unknown result type (might be due to invalid IL or missing references)
		//IL_007f: Unknown result type (might be due to invalid IL or missing references)
		//IL_008d: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a0: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a5: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b1: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b6: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c4: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d7: Unknown result type (might be due to invalid IL or missing references)
		//IL_00dc: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e8: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ed: Unknown result type (might be due to invalid IL or missing references)
		//IL_00fb: Unknown result type (might be due to invalid IL or missing references)
		//IL_010e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0113: Unknown result type (might be due to invalid IL or missing references)
		//IL_011f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0124: Unknown result type (might be due to invalid IL or missing references)
		//IL_0132: Unknown result type (might be due to invalid IL or missing references)
		//IL_0145: Unknown result type (might be due to invalid IL or missing references)
		//IL_014a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0156: Unknown result type (might be due to invalid IL or missing references)
		//IL_015b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0169: Unknown result type (might be due to invalid IL or missing references)
		//IL_017c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0181: Unknown result type (might be due to invalid IL or missing references)
		//IL_018d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0192: Unknown result type (might be due to invalid IL or missing references)
		//IL_01a0: Unknown result type (might be due to invalid IL or missing references)
		//IL_01b3: Unknown result type (might be due to invalid IL or missing references)
		//IL_01b8: Unknown result type (might be due to invalid IL or missing references)
		//IL_01c4: Unknown result type (might be due to invalid IL or missing references)
		//IL_01c9: Unknown result type (might be due to invalid IL or missing references)
		//IL_01d7: Unknown result type (might be due to invalid IL or missing references)
		//IL_0208: Unknown result type (might be due to invalid IL or missing references)
		//IL_020d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0219: Unknown result type (might be due to invalid IL or missing references)
		//IL_021e: Unknown result type (might be due to invalid IL or missing references)
		//IL_022c: Unknown result type (might be due to invalid IL or missing references)
		//IL_023f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0244: Unknown result type (might be due to invalid IL or missing references)
		//IL_0250: Unknown result type (might be due to invalid IL or missing references)
		//IL_0255: Unknown result type (might be due to invalid IL or missing references)
		//IL_0263: Unknown result type (might be due to invalid IL or missing references)
		//IL_0276: Unknown result type (might be due to invalid IL or missing references)
		//IL_027b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0287: Unknown result type (might be due to invalid IL or missing references)
		//IL_028c: Unknown result type (might be due to invalid IL or missing references)
		//IL_029a: Unknown result type (might be due to invalid IL or missing references)
		//IL_02ad: Unknown result type (might be due to invalid IL or missing references)
		//IL_02b2: Unknown result type (might be due to invalid IL or missing references)
		//IL_02be: Unknown result type (might be due to invalid IL or missing references)
		//IL_02c3: Unknown result type (might be due to invalid IL or missing references)
		//IL_02d1: Unknown result type (might be due to invalid IL or missing references)
		//IL_02e4: Unknown result type (might be due to invalid IL or missing references)
		//IL_02e9: Unknown result type (might be due to invalid IL or missing references)
		//IL_02f5: Unknown result type (might be due to invalid IL or missing references)
		//IL_02fa: Unknown result type (might be due to invalid IL or missing references)
		//IL_0308: Unknown result type (might be due to invalid IL or missing references)
		//IL_031b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0320: Unknown result type (might be due to invalid IL or missing references)
		//IL_032c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0331: Unknown result type (might be due to invalid IL or missing references)
		//IL_033f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0352: Unknown result type (might be due to invalid IL or missing references)
		//IL_0357: Unknown result type (might be due to invalid IL or missing references)
		//IL_0363: Unknown result type (might be due to invalid IL or missing references)
		//IL_0368: Unknown result type (might be due to invalid IL or missing references)
		//IL_0376: Unknown result type (might be due to invalid IL or missing references)
		//IL_0389: Unknown result type (might be due to invalid IL or missing references)
		//IL_038e: Unknown result type (might be due to invalid IL or missing references)
		//IL_039a: Unknown result type (might be due to invalid IL or missing references)
		//IL_039f: Unknown result type (might be due to invalid IL or missing references)
		//IL_03ad: Unknown result type (might be due to invalid IL or missing references)
		//IL_03de: Unknown result type (might be due to invalid IL or missing references)
		//IL_03e3: Unknown result type (might be due to invalid IL or missing references)
		//IL_03ef: Unknown result type (might be due to invalid IL or missing references)
		//IL_03f4: Unknown result type (might be due to invalid IL or missing references)
		//IL_0402: Unknown result type (might be due to invalid IL or missing references)
		//IL_0415: Unknown result type (might be due to invalid IL or missing references)
		//IL_041a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0426: Unknown result type (might be due to invalid IL or missing references)
		//IL_042b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0439: Unknown result type (might be due to invalid IL or missing references)
		//IL_044c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0451: Unknown result type (might be due to invalid IL or missing references)
		//IL_045d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0462: Unknown result type (might be due to invalid IL or missing references)
		//IL_0470: Unknown result type (might be due to invalid IL or missing references)
		//IL_0483: Unknown result type (might be due to invalid IL or missing references)
		//IL_0488: Unknown result type (might be due to invalid IL or missing references)
		//IL_0494: Unknown result type (might be due to invalid IL or missing references)
		//IL_0499: Unknown result type (might be due to invalid IL or missing references)
		//IL_04a7: Unknown result type (might be due to invalid IL or missing references)
		//IL_04ba: Unknown result type (might be due to invalid IL or missing references)
		//IL_04bf: Unknown result type (might be due to invalid IL or missing references)
		//IL_04cb: Unknown result type (might be due to invalid IL or missing references)
		//IL_04d0: Unknown result type (might be due to invalid IL or missing references)
		//IL_04de: Unknown result type (might be due to invalid IL or missing references)
		//IL_04f1: Unknown result type (might be due to invalid IL or missing references)
		//IL_04f6: Unknown result type (might be due to invalid IL or missing references)
		//IL_0502: Unknown result type (might be due to invalid IL or missing references)
		//IL_0507: Unknown result type (might be due to invalid IL or missing references)
		//IL_0515: Unknown result type (might be due to invalid IL or missing references)
		//IL_0528: Unknown result type (might be due to invalid IL or missing references)
		//IL_052d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0539: Unknown result type (might be due to invalid IL or missing references)
		//IL_053e: Unknown result type (might be due to invalid IL or missing references)
		//IL_054c: Unknown result type (might be due to invalid IL or missing references)
		//IL_055f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0564: Unknown result type (might be due to invalid IL or missing references)
		//IL_0570: Unknown result type (might be due to invalid IL or missing references)
		//IL_0575: Unknown result type (might be due to invalid IL or missing references)
		//IL_0583: Unknown result type (might be due to invalid IL or missing references)
		//IL_05b4: Unknown result type (might be due to invalid IL or missing references)
		//IL_05b9: Unknown result type (might be due to invalid IL or missing references)
		//IL_05c5: Unknown result type (might be due to invalid IL or missing references)
		//IL_05ca: Unknown result type (might be due to invalid IL or missing references)
		//IL_05d8: Unknown result type (might be due to invalid IL or missing references)
		//IL_05eb: Unknown result type (might be due to invalid IL or missing references)
		//IL_05f0: Unknown result type (might be due to invalid IL or missing references)
		//IL_05fc: Unknown result type (might be due to invalid IL or missing references)
		//IL_0601: Unknown result type (might be due to invalid IL or missing references)
		//IL_060f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0622: Unknown result type (might be due to invalid IL or missing references)
		//IL_0627: Unknown result type (might be due to invalid IL or missing references)
		//IL_0633: Unknown result type (might be due to invalid IL or missing references)
		//IL_0638: Unknown result type (might be due to invalid IL or missing references)
		//IL_0646: Unknown result type (might be due to invalid IL or missing references)
		//IL_0659: Unknown result type (might be due to invalid IL or missing references)
		//IL_065e: Unknown result type (might be due to invalid IL or missing references)
		//IL_066a: Unknown result type (might be due to invalid IL or missing references)
		//IL_066f: Unknown result type (might be due to invalid IL or missing references)
		//IL_067d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0690: Unknown result type (might be due to invalid IL or missing references)
		//IL_0695: Unknown result type (might be due to invalid IL or missing references)
		//IL_06a1: Unknown result type (might be due to invalid IL or missing references)
		//IL_06a6: Unknown result type (might be due to invalid IL or missing references)
		//IL_06b4: Unknown result type (might be due to invalid IL or missing references)
		//IL_06c7: Unknown result type (might be due to invalid IL or missing references)
		//IL_06cc: Unknown result type (might be due to invalid IL or missing references)
		//IL_06d8: Unknown result type (might be due to invalid IL or missing references)
		//IL_06dd: Unknown result type (might be due to invalid IL or missing references)
		//IL_06eb: Unknown result type (might be due to invalid IL or missing references)
		//IL_06fe: Unknown result type (might be due to invalid IL or missing references)
		//IL_0703: Unknown result type (might be due to invalid IL or missing references)
		//IL_070f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0714: Unknown result type (might be due to invalid IL or missing references)
		//IL_0722: Unknown result type (might be due to invalid IL or missing references)
		//IL_0735: Unknown result type (might be due to invalid IL or missing references)
		//IL_073a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0746: Unknown result type (might be due to invalid IL or missing references)
		//IL_074b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0759: Unknown result type (might be due to invalid IL or missing references)
		//IL_078a: Unknown result type (might be due to invalid IL or missing references)
		//IL_078f: Unknown result type (might be due to invalid IL or missing references)
		//IL_079b: Unknown result type (might be due to invalid IL or missing references)
		//IL_07a0: Unknown result type (might be due to invalid IL or missing references)
		//IL_07b0: Unknown result type (might be due to invalid IL or missing references)
		//IL_07c3: Unknown result type (might be due to invalid IL or missing references)
		//IL_07c8: Unknown result type (might be due to invalid IL or missing references)
		//IL_07d4: Unknown result type (might be due to invalid IL or missing references)
		//IL_07d9: Unknown result type (might be due to invalid IL or missing references)
		//IL_07e9: Unknown result type (might be due to invalid IL or missing references)
		//IL_07fc: Unknown result type (might be due to invalid IL or missing references)
		//IL_0801: Unknown result type (might be due to invalid IL or missing references)
		//IL_080d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0812: Unknown result type (might be due to invalid IL or missing references)
		//IL_0822: Unknown result type (might be due to invalid IL or missing references)
		//IL_0835: Unknown result type (might be due to invalid IL or missing references)
		//IL_083a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0846: Unknown result type (might be due to invalid IL or missing references)
		//IL_084b: Unknown result type (might be due to invalid IL or missing references)
		//IL_085b: Unknown result type (might be due to invalid IL or missing references)
		//IL_086e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0873: Unknown result type (might be due to invalid IL or missing references)
		//IL_087f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0884: Unknown result type (might be due to invalid IL or missing references)
		//IL_0894: Unknown result type (might be due to invalid IL or missing references)
		//IL_08a7: Unknown result type (might be due to invalid IL or missing references)
		//IL_08ac: Unknown result type (might be due to invalid IL or missing references)
		//IL_08b8: Unknown result type (might be due to invalid IL or missing references)
		//IL_08bd: Unknown result type (might be due to invalid IL or missing references)
		//IL_08cd: Unknown result type (might be due to invalid IL or missing references)
		//IL_08e0: Unknown result type (might be due to invalid IL or missing references)
		//IL_08e5: Unknown result type (might be due to invalid IL or missing references)
		//IL_08f1: Unknown result type (might be due to invalid IL or missing references)
		//IL_08f6: Unknown result type (might be due to invalid IL or missing references)
		//IL_0906: Unknown result type (might be due to invalid IL or missing references)
		//IL_0919: Unknown result type (might be due to invalid IL or missing references)
		//IL_091e: Unknown result type (might be due to invalid IL or missing references)
		//IL_092a: Unknown result type (might be due to invalid IL or missing references)
		//IL_092f: Unknown result type (might be due to invalid IL or missing references)
		//IL_093f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0970: Unknown result type (might be due to invalid IL or missing references)
		//IL_0975: Unknown result type (might be due to invalid IL or missing references)
		//IL_0981: Unknown result type (might be due to invalid IL or missing references)
		//IL_0986: Unknown result type (might be due to invalid IL or missing references)
		//IL_0996: Unknown result type (might be due to invalid IL or missing references)
		//IL_09a9: Unknown result type (might be due to invalid IL or missing references)
		//IL_09ae: Unknown result type (might be due to invalid IL or missing references)
		//IL_09ba: Unknown result type (might be due to invalid IL or missing references)
		//IL_09bf: Unknown result type (might be due to invalid IL or missing references)
		//IL_09cf: Unknown result type (might be due to invalid IL or missing references)
		//IL_09e2: Unknown result type (might be due to invalid IL or missing references)
		//IL_09e7: Unknown result type (might be due to invalid IL or missing references)
		//IL_09f3: Unknown result type (might be due to invalid IL or missing references)
		//IL_09f8: Unknown result type (might be due to invalid IL or missing references)
		//IL_0a08: Unknown result type (might be due to invalid IL or missing references)
		//IL_0a1b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0a20: Unknown result type (might be due to invalid IL or missing references)
		//IL_0a2c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0a31: Unknown result type (might be due to invalid IL or missing references)
		//IL_0a41: Unknown result type (might be due to invalid IL or missing references)
		//IL_0a54: Unknown result type (might be due to invalid IL or missing references)
		//IL_0a59: Unknown result type (might be due to invalid IL or missing references)
		//IL_0a65: Unknown result type (might be due to invalid IL or missing references)
		//IL_0a6a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0a7a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0a8d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0a92: Unknown result type (might be due to invalid IL or missing references)
		//IL_0a9e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0aa3: Unknown result type (might be due to invalid IL or missing references)
		//IL_0ab3: Unknown result type (might be due to invalid IL or missing references)
		//IL_0ac6: Unknown result type (might be due to invalid IL or missing references)
		//IL_0acb: Unknown result type (might be due to invalid IL or missing references)
		//IL_0ad7: Unknown result type (might be due to invalid IL or missing references)
		//IL_0adc: Unknown result type (might be due to invalid IL or missing references)
		//IL_0aec: Unknown result type (might be due to invalid IL or missing references)
		//IL_0aff: Unknown result type (might be due to invalid IL or missing references)
		//IL_0b04: Unknown result type (might be due to invalid IL or missing references)
		//IL_0b10: Unknown result type (might be due to invalid IL or missing references)
		//IL_0b15: Unknown result type (might be due to invalid IL or missing references)
		//IL_0b25: Unknown result type (might be due to invalid IL or missing references)
		//IL_0bc7: Unknown result type (might be due to invalid IL or missing references)
		//IL_0bcc: Unknown result type (might be due to invalid IL or missing references)
		//IL_0be5: Unknown result type (might be due to invalid IL or missing references)
		//IL_0bf8: Unknown result type (might be due to invalid IL or missing references)
		//IL_0bfd: Unknown result type (might be due to invalid IL or missing references)
		//IL_0c16: Unknown result type (might be due to invalid IL or missing references)
		//IL_0b6d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0b72: Unknown result type (might be due to invalid IL or missing references)
		//IL_0b82: Unknown result type (might be due to invalid IL or missing references)
		//IL_0b9e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0ba3: Unknown result type (might be due to invalid IL or missing references)
		//IL_0bb3: Unknown result type (might be due to invalid IL or missing references)
		((RhinoList<Point3d>)(object)_vertices).Clear();
		double num = 0.53;
		double num2 = 0.68;
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
		Point3dList vertices5 = _vertices;
		double num16 = num;
		val = ((RhinoList<Point3d>)(object)profilePoints)[5];
		double num17 = num16 * ((Point3d)(ref val)).X;
		double num18 = num2;
		val = ((RhinoList<Point3d>)(object)profilePoints)[5];
		((RhinoList<Point3d>)(object)vertices5).Add(new Point3d(num17, num18 * ((Point3d)(ref val)).Y, num3 * topThickness + halfGirdleThickness));
		Point3dList vertices6 = _vertices;
		double num19 = num;
		val = ((RhinoList<Point3d>)(object)profilePoints)[6];
		double num20 = num19 * ((Point3d)(ref val)).X;
		double num21 = num2;
		val = ((RhinoList<Point3d>)(object)profilePoints)[6];
		((RhinoList<Point3d>)(object)vertices6).Add(new Point3d(num20, num21 * ((Point3d)(ref val)).Y, num3 * topThickness + halfGirdleThickness));
		Point3dList vertices7 = _vertices;
		double num22 = num;
		val = ((RhinoList<Point3d>)(object)profilePoints)[7];
		double num23 = num22 * ((Point3d)(ref val)).X;
		double num24 = num2;
		val = ((RhinoList<Point3d>)(object)profilePoints)[7];
		((RhinoList<Point3d>)(object)vertices7).Add(new Point3d(num23, num24 * ((Point3d)(ref val)).Y, num3 * topThickness + halfGirdleThickness));
		Point3dList vertices8 = _vertices;
		double num25 = num;
		val = ((RhinoList<Point3d>)(object)profilePoints)[8];
		double num26 = num25 * ((Point3d)(ref val)).X;
		double num27 = num2;
		val = ((RhinoList<Point3d>)(object)profilePoints)[8];
		((RhinoList<Point3d>)(object)vertices8).Add(new Point3d(num26, num27 * ((Point3d)(ref val)).Y, num3 * topThickness + halfGirdleThickness));
		num = 0.67;
		num2 = 0.755;
		num3 = 0.9;
		Point3dList vertices9 = _vertices;
		double num28 = num;
		val = ((RhinoList<Point3d>)(object)profilePoints)[1];
		double num29 = num28 * ((Point3d)(ref val)).X;
		double num30 = num2;
		val = ((RhinoList<Point3d>)(object)profilePoints)[1];
		((RhinoList<Point3d>)(object)vertices9).Add(new Point3d(num29, num30 * ((Point3d)(ref val)).Y, num3 * topThickness + halfGirdleThickness));
		Point3dList vertices10 = _vertices;
		double num31 = num;
		val = ((RhinoList<Point3d>)(object)profilePoints)[2];
		double num32 = num31 * ((Point3d)(ref val)).X;
		double num33 = num2;
		val = ((RhinoList<Point3d>)(object)profilePoints)[2];
		((RhinoList<Point3d>)(object)vertices10).Add(new Point3d(num32, num33 * ((Point3d)(ref val)).Y, num3 * topThickness + halfGirdleThickness));
		Point3dList vertices11 = _vertices;
		double num34 = num;
		val = ((RhinoList<Point3d>)(object)profilePoints)[3];
		double num35 = num34 * ((Point3d)(ref val)).X;
		double num36 = num2;
		val = ((RhinoList<Point3d>)(object)profilePoints)[3];
		((RhinoList<Point3d>)(object)vertices11).Add(new Point3d(num35, num36 * ((Point3d)(ref val)).Y, num3 * topThickness + halfGirdleThickness));
		Point3dList vertices12 = _vertices;
		double num37 = num;
		val = ((RhinoList<Point3d>)(object)profilePoints)[4];
		double num38 = num37 * ((Point3d)(ref val)).X;
		double num39 = num2;
		val = ((RhinoList<Point3d>)(object)profilePoints)[4];
		((RhinoList<Point3d>)(object)vertices12).Add(new Point3d(num38, num39 * ((Point3d)(ref val)).Y, num3 * topThickness + halfGirdleThickness));
		Point3dList vertices13 = _vertices;
		double num40 = num;
		val = ((RhinoList<Point3d>)(object)profilePoints)[5];
		double num41 = num40 * ((Point3d)(ref val)).X;
		double num42 = num2;
		val = ((RhinoList<Point3d>)(object)profilePoints)[5];
		((RhinoList<Point3d>)(object)vertices13).Add(new Point3d(num41, num42 * ((Point3d)(ref val)).Y, num3 * topThickness + halfGirdleThickness));
		Point3dList vertices14 = _vertices;
		double num43 = num;
		val = ((RhinoList<Point3d>)(object)profilePoints)[6];
		double num44 = num43 * ((Point3d)(ref val)).X;
		double num45 = num2;
		val = ((RhinoList<Point3d>)(object)profilePoints)[6];
		((RhinoList<Point3d>)(object)vertices14).Add(new Point3d(num44, num45 * ((Point3d)(ref val)).Y, num3 * topThickness + halfGirdleThickness));
		Point3dList vertices15 = _vertices;
		double num46 = num;
		val = ((RhinoList<Point3d>)(object)profilePoints)[7];
		double num47 = num46 * ((Point3d)(ref val)).X;
		double num48 = num2;
		val = ((RhinoList<Point3d>)(object)profilePoints)[7];
		((RhinoList<Point3d>)(object)vertices15).Add(new Point3d(num47, num48 * ((Point3d)(ref val)).Y, num3 * topThickness + halfGirdleThickness));
		Point3dList vertices16 = _vertices;
		double num49 = num;
		val = ((RhinoList<Point3d>)(object)profilePoints)[8];
		double num50 = num49 * ((Point3d)(ref val)).X;
		double num51 = num2;
		val = ((RhinoList<Point3d>)(object)profilePoints)[8];
		((RhinoList<Point3d>)(object)vertices16).Add(new Point3d(num50, num51 * ((Point3d)(ref val)).Y, num3 * topThickness + halfGirdleThickness));
		num = 0.83;
		num2 = 0.87;
		num3 = 0.6;
		Point3dList vertices17 = _vertices;
		double num52 = num;
		val = ((RhinoList<Point3d>)(object)profilePoints)[1];
		double num53 = num52 * ((Point3d)(ref val)).X;
		double num54 = num2;
		val = ((RhinoList<Point3d>)(object)profilePoints)[1];
		((RhinoList<Point3d>)(object)vertices17).Add(new Point3d(num53, num54 * ((Point3d)(ref val)).Y, num3 * topThickness + halfGirdleThickness));
		Point3dList vertices18 = _vertices;
		double num55 = num;
		val = ((RhinoList<Point3d>)(object)profilePoints)[2];
		double num56 = num55 * ((Point3d)(ref val)).X;
		double num57 = num2;
		val = ((RhinoList<Point3d>)(object)profilePoints)[2];
		((RhinoList<Point3d>)(object)vertices18).Add(new Point3d(num56, num57 * ((Point3d)(ref val)).Y, num3 * topThickness + halfGirdleThickness));
		Point3dList vertices19 = _vertices;
		double num58 = num;
		val = ((RhinoList<Point3d>)(object)profilePoints)[3];
		double num59 = num58 * ((Point3d)(ref val)).X;
		double num60 = num2;
		val = ((RhinoList<Point3d>)(object)profilePoints)[3];
		((RhinoList<Point3d>)(object)vertices19).Add(new Point3d(num59, num60 * ((Point3d)(ref val)).Y, num3 * topThickness + halfGirdleThickness));
		Point3dList vertices20 = _vertices;
		double num61 = num;
		val = ((RhinoList<Point3d>)(object)profilePoints)[4];
		double num62 = num61 * ((Point3d)(ref val)).X;
		double num63 = num2;
		val = ((RhinoList<Point3d>)(object)profilePoints)[4];
		((RhinoList<Point3d>)(object)vertices20).Add(new Point3d(num62, num63 * ((Point3d)(ref val)).Y, num3 * topThickness + halfGirdleThickness));
		Point3dList vertices21 = _vertices;
		double num64 = num;
		val = ((RhinoList<Point3d>)(object)profilePoints)[5];
		double num65 = num64 * ((Point3d)(ref val)).X;
		double num66 = num2;
		val = ((RhinoList<Point3d>)(object)profilePoints)[5];
		((RhinoList<Point3d>)(object)vertices21).Add(new Point3d(num65, num66 * ((Point3d)(ref val)).Y, num3 * topThickness + halfGirdleThickness));
		Point3dList vertices22 = _vertices;
		double num67 = num;
		val = ((RhinoList<Point3d>)(object)profilePoints)[6];
		double num68 = num67 * ((Point3d)(ref val)).X;
		double num69 = num2;
		val = ((RhinoList<Point3d>)(object)profilePoints)[6];
		((RhinoList<Point3d>)(object)vertices22).Add(new Point3d(num68, num69 * ((Point3d)(ref val)).Y, num3 * topThickness + halfGirdleThickness));
		Point3dList vertices23 = _vertices;
		double num70 = num;
		val = ((RhinoList<Point3d>)(object)profilePoints)[7];
		double num71 = num70 * ((Point3d)(ref val)).X;
		double num72 = num2;
		val = ((RhinoList<Point3d>)(object)profilePoints)[7];
		((RhinoList<Point3d>)(object)vertices23).Add(new Point3d(num71, num72 * ((Point3d)(ref val)).Y, num3 * topThickness + halfGirdleThickness));
		Point3dList vertices24 = _vertices;
		double num73 = num;
		val = ((RhinoList<Point3d>)(object)profilePoints)[8];
		double num74 = num73 * ((Point3d)(ref val)).X;
		double num75 = num2;
		val = ((RhinoList<Point3d>)(object)profilePoints)[8];
		((RhinoList<Point3d>)(object)vertices24).Add(new Point3d(num74, num75 * ((Point3d)(ref val)).Y, num3 * topThickness + halfGirdleThickness));
		num = 1.0;
		num2 = 1.0;
		num3 = 0.0;
		Point3dList vertices25 = _vertices;
		double num76 = num;
		val = ((RhinoList<Point3d>)(object)profilePoints)[1];
		double num77 = num76 * ((Point3d)(ref val)).X;
		double num78 = num2;
		val = ((RhinoList<Point3d>)(object)profilePoints)[1];
		((RhinoList<Point3d>)(object)vertices25).Add(new Point3d(num77, num78 * ((Point3d)(ref val)).Y, num3 * topThickness + halfGirdleThickness));
		Point3dList vertices26 = _vertices;
		double num79 = num;
		val = ((RhinoList<Point3d>)(object)profilePoints)[2];
		double num80 = num79 * ((Point3d)(ref val)).X;
		double num81 = num2;
		val = ((RhinoList<Point3d>)(object)profilePoints)[2];
		((RhinoList<Point3d>)(object)vertices26).Add(new Point3d(num80, num81 * ((Point3d)(ref val)).Y, num3 * topThickness + halfGirdleThickness));
		Point3dList vertices27 = _vertices;
		double num82 = num;
		val = ((RhinoList<Point3d>)(object)profilePoints)[3];
		double num83 = num82 * ((Point3d)(ref val)).X;
		double num84 = num2;
		val = ((RhinoList<Point3d>)(object)profilePoints)[3];
		((RhinoList<Point3d>)(object)vertices27).Add(new Point3d(num83, num84 * ((Point3d)(ref val)).Y, num3 * topThickness + halfGirdleThickness));
		Point3dList vertices28 = _vertices;
		double num85 = num;
		val = ((RhinoList<Point3d>)(object)profilePoints)[4];
		double num86 = num85 * ((Point3d)(ref val)).X;
		double num87 = num2;
		val = ((RhinoList<Point3d>)(object)profilePoints)[4];
		((RhinoList<Point3d>)(object)vertices28).Add(new Point3d(num86, num87 * ((Point3d)(ref val)).Y, num3 * topThickness + halfGirdleThickness));
		Point3dList vertices29 = _vertices;
		double num88 = num;
		val = ((RhinoList<Point3d>)(object)profilePoints)[5];
		double num89 = num88 * ((Point3d)(ref val)).X;
		double num90 = num2;
		val = ((RhinoList<Point3d>)(object)profilePoints)[5];
		((RhinoList<Point3d>)(object)vertices29).Add(new Point3d(num89, num90 * ((Point3d)(ref val)).Y, num3 * topThickness + halfGirdleThickness));
		Point3dList vertices30 = _vertices;
		double num91 = num;
		val = ((RhinoList<Point3d>)(object)profilePoints)[6];
		double num92 = num91 * ((Point3d)(ref val)).X;
		double num93 = num2;
		val = ((RhinoList<Point3d>)(object)profilePoints)[6];
		((RhinoList<Point3d>)(object)vertices30).Add(new Point3d(num92, num93 * ((Point3d)(ref val)).Y, num3 * topThickness + halfGirdleThickness));
		Point3dList vertices31 = _vertices;
		double num94 = num;
		val = ((RhinoList<Point3d>)(object)profilePoints)[7];
		double num95 = num94 * ((Point3d)(ref val)).X;
		double num96 = num2;
		val = ((RhinoList<Point3d>)(object)profilePoints)[7];
		((RhinoList<Point3d>)(object)vertices31).Add(new Point3d(num95, num96 * ((Point3d)(ref val)).Y, num3 * topThickness + halfGirdleThickness));
		Point3dList vertices32 = _vertices;
		double num97 = num;
		val = ((RhinoList<Point3d>)(object)profilePoints)[8];
		double num98 = num97 * ((Point3d)(ref val)).X;
		double num99 = num2;
		val = ((RhinoList<Point3d>)(object)profilePoints)[8];
		((RhinoList<Point3d>)(object)vertices32).Add(new Point3d(num98, num99 * ((Point3d)(ref val)).Y, num3 * topThickness + halfGirdleThickness));
		num = 1.0;
		num2 = 1.0;
		num3 = 0.0;
		Point3dList vertices33 = _vertices;
		double num100 = num;
		val = ((RhinoList<Point3d>)(object)profilePoints)[1];
		double num101 = num100 * ((Point3d)(ref val)).X;
		double num102 = num2;
		val = ((RhinoList<Point3d>)(object)profilePoints)[1];
		((RhinoList<Point3d>)(object)vertices33).Add(new Point3d(num101, num102 * ((Point3d)(ref val)).Y, 0.0 - (num3 * bottomThickness + halfGirdleThickness)));
		Point3dList vertices34 = _vertices;
		double num103 = num;
		val = ((RhinoList<Point3d>)(object)profilePoints)[2];
		double num104 = num103 * ((Point3d)(ref val)).X;
		double num105 = num2;
		val = ((RhinoList<Point3d>)(object)profilePoints)[2];
		((RhinoList<Point3d>)(object)vertices34).Add(new Point3d(num104, num105 * ((Point3d)(ref val)).Y, 0.0 - (num3 * bottomThickness + halfGirdleThickness)));
		Point3dList vertices35 = _vertices;
		double num106 = num;
		val = ((RhinoList<Point3d>)(object)profilePoints)[3];
		double num107 = num106 * ((Point3d)(ref val)).X;
		double num108 = num2;
		val = ((RhinoList<Point3d>)(object)profilePoints)[3];
		((RhinoList<Point3d>)(object)vertices35).Add(new Point3d(num107, num108 * ((Point3d)(ref val)).Y, 0.0 - (num3 * bottomThickness + halfGirdleThickness)));
		Point3dList vertices36 = _vertices;
		double num109 = num;
		val = ((RhinoList<Point3d>)(object)profilePoints)[4];
		double num110 = num109 * ((Point3d)(ref val)).X;
		double num111 = num2;
		val = ((RhinoList<Point3d>)(object)profilePoints)[4];
		((RhinoList<Point3d>)(object)vertices36).Add(new Point3d(num110, num111 * ((Point3d)(ref val)).Y, 0.0 - (num3 * bottomThickness + halfGirdleThickness)));
		Point3dList vertices37 = _vertices;
		double num112 = num;
		val = ((RhinoList<Point3d>)(object)profilePoints)[5];
		double num113 = num112 * ((Point3d)(ref val)).X;
		double num114 = num2;
		val = ((RhinoList<Point3d>)(object)profilePoints)[5];
		((RhinoList<Point3d>)(object)vertices37).Add(new Point3d(num113, num114 * ((Point3d)(ref val)).Y, 0.0 - (num3 * bottomThickness + halfGirdleThickness)));
		Point3dList vertices38 = _vertices;
		double num115 = num;
		val = ((RhinoList<Point3d>)(object)profilePoints)[6];
		double num116 = num115 * ((Point3d)(ref val)).X;
		double num117 = num2;
		val = ((RhinoList<Point3d>)(object)profilePoints)[6];
		((RhinoList<Point3d>)(object)vertices38).Add(new Point3d(num116, num117 * ((Point3d)(ref val)).Y, 0.0 - (num3 * bottomThickness + halfGirdleThickness)));
		Point3dList vertices39 = _vertices;
		double num118 = num;
		val = ((RhinoList<Point3d>)(object)profilePoints)[7];
		double num119 = num118 * ((Point3d)(ref val)).X;
		double num120 = num2;
		val = ((RhinoList<Point3d>)(object)profilePoints)[7];
		((RhinoList<Point3d>)(object)vertices39).Add(new Point3d(num119, num120 * ((Point3d)(ref val)).Y, 0.0 - (num3 * bottomThickness + halfGirdleThickness)));
		Point3dList vertices40 = _vertices;
		double num121 = num;
		val = ((RhinoList<Point3d>)(object)profilePoints)[8];
		double num122 = num121 * ((Point3d)(ref val)).X;
		double num123 = num2;
		val = ((RhinoList<Point3d>)(object)profilePoints)[8];
		((RhinoList<Point3d>)(object)vertices40).Add(new Point3d(num122, num123 * ((Point3d)(ref val)).Y, 0.0 - (num3 * bottomThickness + halfGirdleThickness)));
		num = 0.68;
		num2 = 0.745;
		num3 = 0.55;
		Point3dList vertices41 = _vertices;
		double num124 = num;
		val = ((RhinoList<Point3d>)(object)profilePoints)[1];
		double num125 = num124 * ((Point3d)(ref val)).X;
		double num126 = num2;
		val = ((RhinoList<Point3d>)(object)profilePoints)[1];
		((RhinoList<Point3d>)(object)vertices41).Add(new Point3d(num125, num126 * ((Point3d)(ref val)).Y, 0.0 - (num3 * bottomThickness + halfGirdleThickness)));
		Point3dList vertices42 = _vertices;
		double num127 = num;
		val = ((RhinoList<Point3d>)(object)profilePoints)[2];
		double num128 = num127 * ((Point3d)(ref val)).X;
		double num129 = num2;
		val = ((RhinoList<Point3d>)(object)profilePoints)[2];
		((RhinoList<Point3d>)(object)vertices42).Add(new Point3d(num128, num129 * ((Point3d)(ref val)).Y, 0.0 - (num3 * bottomThickness + halfGirdleThickness)));
		Point3dList vertices43 = _vertices;
		double num130 = num;
		val = ((RhinoList<Point3d>)(object)profilePoints)[3];
		double num131 = num130 * ((Point3d)(ref val)).X;
		double num132 = num2;
		val = ((RhinoList<Point3d>)(object)profilePoints)[3];
		((RhinoList<Point3d>)(object)vertices43).Add(new Point3d(num131, num132 * ((Point3d)(ref val)).Y, 0.0 - (num3 * bottomThickness + halfGirdleThickness)));
		Point3dList vertices44 = _vertices;
		double num133 = num;
		val = ((RhinoList<Point3d>)(object)profilePoints)[4];
		double num134 = num133 * ((Point3d)(ref val)).X;
		double num135 = num2;
		val = ((RhinoList<Point3d>)(object)profilePoints)[4];
		((RhinoList<Point3d>)(object)vertices44).Add(new Point3d(num134, num135 * ((Point3d)(ref val)).Y, 0.0 - (num3 * bottomThickness + halfGirdleThickness)));
		Point3dList vertices45 = _vertices;
		double num136 = num;
		val = ((RhinoList<Point3d>)(object)profilePoints)[5];
		double num137 = num136 * ((Point3d)(ref val)).X;
		double num138 = num2;
		val = ((RhinoList<Point3d>)(object)profilePoints)[5];
		((RhinoList<Point3d>)(object)vertices45).Add(new Point3d(num137, num138 * ((Point3d)(ref val)).Y, 0.0 - (num3 * bottomThickness + halfGirdleThickness)));
		Point3dList vertices46 = _vertices;
		double num139 = num;
		val = ((RhinoList<Point3d>)(object)profilePoints)[6];
		double num140 = num139 * ((Point3d)(ref val)).X;
		double num141 = num2;
		val = ((RhinoList<Point3d>)(object)profilePoints)[6];
		((RhinoList<Point3d>)(object)vertices46).Add(new Point3d(num140, num141 * ((Point3d)(ref val)).Y, 0.0 - (num3 * bottomThickness + halfGirdleThickness)));
		Point3dList vertices47 = _vertices;
		double num142 = num;
		val = ((RhinoList<Point3d>)(object)profilePoints)[7];
		double num143 = num142 * ((Point3d)(ref val)).X;
		double num144 = num2;
		val = ((RhinoList<Point3d>)(object)profilePoints)[7];
		((RhinoList<Point3d>)(object)vertices47).Add(new Point3d(num143, num144 * ((Point3d)(ref val)).Y, 0.0 - (num3 * bottomThickness + halfGirdleThickness)));
		Point3dList vertices48 = _vertices;
		double num145 = num;
		val = ((RhinoList<Point3d>)(object)profilePoints)[8];
		double num146 = num145 * ((Point3d)(ref val)).X;
		double num147 = num2;
		val = ((RhinoList<Point3d>)(object)profilePoints)[8];
		((RhinoList<Point3d>)(object)vertices48).Add(new Point3d(num146, num147 * ((Point3d)(ref val)).Y, 0.0 - (num3 * bottomThickness + halfGirdleThickness)));
		num = 0.5;
		num2 = 0.38;
		num3 = 1.0;
		if (_length > _width)
		{
			Point3dList vertices49 = _vertices;
			double num148 = num2;
			val = ((RhinoList<Point3d>)(object)profilePoints)[1];
			((RhinoList<Point3d>)(object)vertices49).Add(new Point3d(0.0, num148 * ((Point3d)(ref val)).Y, 0.0 - (num3 * bottomThickness + halfGirdleThickness)));
			Point3dList vertices50 = _vertices;
			double num149 = num2;
			val = ((RhinoList<Point3d>)(object)profilePoints)[5];
			((RhinoList<Point3d>)(object)vertices50).Add(new Point3d(0.0, num149 * ((Point3d)(ref val)).Y, 0.0 - (num3 * bottomThickness + halfGirdleThickness)));
		}
		else
		{
			Point3dList vertices51 = _vertices;
			double num150 = num;
			val = ((RhinoList<Point3d>)(object)profilePoints)[2];
			((RhinoList<Point3d>)(object)vertices51).Add(new Point3d(num150 * ((Point3d)(ref val)).X, 0.0, 0.0 - (num3 * bottomThickness + halfGirdleThickness)));
			Point3dList vertices52 = _vertices;
			double num151 = num;
			val = ((RhinoList<Point3d>)(object)profilePoints)[6];
			((RhinoList<Point3d>)(object)vertices52).Add(new Point3d(num151 * ((Point3d)(ref val)).X, 0.0, 0.0 - (num3 * bottomThickness + halfGirdleThickness)));
		}
	}

	private void Emerald()
	{
		//IL_00ae: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b3: Unknown result type (might be due to invalid IL or missing references)
		//IL_00cc: Unknown result type (might be due to invalid IL or missing references)
		//IL_00df: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f2: Unknown result type (might be due to invalid IL or missing references)
		//IL_0106: Unknown result type (might be due to invalid IL or missing references)
		//IL_011a: Unknown result type (might be due to invalid IL or missing references)
		//IL_012f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0144: Unknown result type (might be due to invalid IL or missing references)
		//IL_0158: Unknown result type (might be due to invalid IL or missing references)
		//IL_016c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0187: Expected O, but got Unknown
		//IL_018a: Unknown result type (might be due to invalid IL or missing references)
		//IL_01a0: Unknown result type (might be due to invalid IL or missing references)
		//IL_01a5: Unknown result type (might be due to invalid IL or missing references)
		//IL_01bc: Unknown result type (might be due to invalid IL or missing references)
		//IL_01c4: Unknown result type (might be due to invalid IL or missing references)
		//IL_01d8: Unknown result type (might be due to invalid IL or missing references)
		//IL_01e0: Unknown result type (might be due to invalid IL or missing references)
		//IL_01f4: Unknown result type (might be due to invalid IL or missing references)
		//IL_01fc: Unknown result type (might be due to invalid IL or missing references)
		//IL_0210: Unknown result type (might be due to invalid IL or missing references)
		//IL_0218: Unknown result type (might be due to invalid IL or missing references)
		//IL_022c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0234: Unknown result type (might be due to invalid IL or missing references)
		//IL_0248: Unknown result type (might be due to invalid IL or missing references)
		//IL_0250: Unknown result type (might be due to invalid IL or missing references)
		//IL_0264: Unknown result type (might be due to invalid IL or missing references)
		//IL_026c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0280: Unknown result type (might be due to invalid IL or missing references)
		//IL_0288: Unknown result type (might be due to invalid IL or missing references)
		//IL_029c: Unknown result type (might be due to invalid IL or missing references)
		//IL_02a5: Unknown result type (might be due to invalid IL or missing references)
		//IL_02cb: Unknown result type (might be due to invalid IL or missing references)
		//IL_02d0: Unknown result type (might be due to invalid IL or missing references)
		//IL_02d8: Unknown result type (might be due to invalid IL or missing references)
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
		Point3dList val = new Point3dList();
		val.Add(0.0, num, 0.0);
		val.Add(num2 - num4, num, 0.0);
		val.Add(num2, num - num4, 0.0);
		val.Add(num2, 0.0 - num + num4, 0.0);
		val.Add(num2 - num4, 0.0 - num, 0.0);
		val.Add(0.0 - num2 + num4, 0.0 - num, 0.0);
		val.Add(0.0 - num2, 0.0 - num + num4, 0.0);
		val.Add(0.0 - num2, num - num4, 0.0);
		val.Add(0.0 - num2 + num4, num, 0.0);
		val.Add(0.0, num, 0.0);
		Point3dList val2 = val;
		_curve = ((Curve)new PolylineCurve((IEnumerable<Point3d>)val2)).ToNurbsCurve();
		Interval domain = ((Curve)_curve).Domain;
		_bezelParameter = ((Interval)(ref domain)).Mid;
		List<Point3d> list = new List<Point3d>();
		list.AddRange(CSUtilCustomGems.CalculateProngPoints(((RhinoList<Point3d>)(object)val2)[0], ((RhinoList<Point3d>)(object)val2)[1], 2));
		list.AddRange(CSUtilCustomGems.CalculateProngPoints(((RhinoList<Point3d>)(object)val2)[1], ((RhinoList<Point3d>)(object)val2)[2], 2));
		list.AddRange(CSUtilCustomGems.CalculateProngPoints(((RhinoList<Point3d>)(object)val2)[2], ((RhinoList<Point3d>)(object)val2)[3], 4));
		list.AddRange(CSUtilCustomGems.CalculateProngPoints(((RhinoList<Point3d>)(object)val2)[3], ((RhinoList<Point3d>)(object)val2)[4], 2));
		list.AddRange(CSUtilCustomGems.CalculateProngPoints(((RhinoList<Point3d>)(object)val2)[4], ((RhinoList<Point3d>)(object)val2)[5], 4));
		list.AddRange(CSUtilCustomGems.CalculateProngPoints(((RhinoList<Point3d>)(object)val2)[5], ((RhinoList<Point3d>)(object)val2)[6], 2));
		list.AddRange(CSUtilCustomGems.CalculateProngPoints(((RhinoList<Point3d>)(object)val2)[6], ((RhinoList<Point3d>)(object)val2)[7], 4));
		list.AddRange(CSUtilCustomGems.CalculateProngPoints(((RhinoList<Point3d>)(object)val2)[7], ((RhinoList<Point3d>)(object)val2)[8], 2));
		list.AddRange(CSUtilCustomGems.CalculateProngPoints(((RhinoList<Point3d>)(object)val2)[8], ((RhinoList<Point3d>)(object)val2)[9], 2));
		base.ProngPositions.Clear();
		double prongParam = default(double);
		foreach (Point3d item in list)
		{
			((Curve)_curve).ClosestPoint(item, ref prongParam);
			base.ProngPositions.Add(new ProngPosition(1, prongParam));
		}
		CalculateVertices(val2, num3, num5, num6);
		CalculateFaceCornerSets();
		_mesh = GetMesh();
	}
}
