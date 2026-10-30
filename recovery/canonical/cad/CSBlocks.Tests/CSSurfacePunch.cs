using System;
using CS;
using CS.CSObjects.CSBlocks;
using CS.Parametric;
using CS.Parametric.Channels;
using CSBlockDataUtils;
using CSBlockUtil;
using CSCompatibility;
using CSDatas;
using GV.CounterSketch.Shared;
using RMA.OpenNURBS;
using RMA.Rhino;
using Rhino;

namespace CSBlocks.Tests;

[Obsolete]
[CSBlock("SurfacePunch", "{00BF6CFE-E3ED-4E43-8ECC-FC664C4612F2}")]
public sealed class CSSurfacePunch : CSBlock
{
	public CSInputChannel InSortedGems { get; }

	public CSInputChannel InSurface { get; }

	public CSInputChannel InChannelSideSpacing { get; }

	public CSInputChannel InChannelEndSpacingBoth { get; }

	public CSInputChannel InChannelEndSpacingStart { get; }

	public CSInputChannel InChannelEndSpacingEnd { get; }

	public CSInputChannel InChannelEndExtendType { get; }

	public CSInputChannel InBlendAmount { get; }

	public CSOutputChannel OutSurface { get; }

	public CSOutputChannel OutCurveStart { get; }

	public CSOutputChannel OutCurveEnd { get; }

	public CSOutputChannel OutCurveLeft { get; }

	public CSOutputChannel OutCurveRight { get; }

	public CSSurfacePunch(string identifier)
		: base(identifier)
	{
		InSortedGems = new CSInputChannel(GetPropertyName(() => InSortedGems), this, ChannelDataType.Gem, ChannelArrayType.Individual);
		InSurface = new CSInputChannel(GetPropertyName(() => InSurface), this, ChannelDataType.Brep, ChannelArrayType.Array | ChannelArrayType.Individual);
		InChannelSideSpacing = new CSInputChannel(GetPropertyName(() => InChannelSideSpacing), this, ChannelDataType.Double, ChannelArrayType.Individual);
		InChannelEndSpacingBoth = new CSInputChannel(GetPropertyName(() => InChannelEndSpacingBoth), this, ChannelDataType.Double, ChannelArrayType.Individual);
		InChannelEndSpacingStart = new CSInputChannel(GetPropertyName(() => InChannelEndSpacingStart), this, ChannelDataType.Double, ChannelArrayType.Individual);
		InChannelEndSpacingEnd = new CSInputChannel(GetPropertyName(() => InChannelEndSpacingEnd), this, ChannelDataType.Double, ChannelArrayType.Individual);
		InChannelEndExtendType = new CSInputChannel(GetPropertyName(() => InChannelEndExtendType), this, ChannelDataType.Integer, ChannelArrayType.Individual);
		InBlendAmount = new CSInputChannel(GetPropertyName(() => InBlendAmount), this, ChannelDataType.Double, ChannelArrayType.Individual);
		OutSurface = new CSOutputChannel(GetPropertyName(() => OutSurface), this, ChannelDataType.Surface | ChannelDataType.Brep, ChannelArrayType.Individual);
		OutCurveStart = new CSOutputChannel(GetPropertyName(() => OutCurveStart), this, ChannelDataType.Curve, ChannelArrayType.Individual);
		OutCurveEnd = new CSOutputChannel(GetPropertyName(() => OutCurveEnd), this, ChannelDataType.Curve, ChannelArrayType.Individual);
		OutCurveLeft = new CSOutputChannel(GetPropertyName(() => OutCurveLeft), this, ChannelDataType.Curve, ChannelArrayType.Individual);
		OutCurveRight = new CSOutputChannel(GetPropertyName(() => OutCurveRight), this, ChannelDataType.Curve, ChannelArrayType.Individual);
	}

	public override bool Update()
	{
		if (!(InSortedGems.Data is CSDataArray cSDataArray))
		{
			return UpdateMissing("Missing sorted gems");
		}
		if (cSDataArray.GetArrayCount() <= 0)
		{
			return UpdateMissing("Missing sorted gems");
		}
		OnBrep brep = InSurface.GetBrep();
		if (brep == null)
		{
			return UpdateMissing("Missing surface");
		}
		double value = 0.0;
		InChannelSideSpacing.GetDouble(ref value);
		double value2 = 0.0;
		InChannelEndSpacingBoth.GetDouble(ref value2);
		double value3 = 0.0;
		InChannelEndSpacingStart.GetDouble(ref value3);
		value3 += value2;
		double value4 = 0.0;
		InChannelEndSpacingEnd.GetDouble(ref value4);
		value4 += value2;
		double value5 = 0.0;
		InBlendAmount.GetDouble(ref value5);
		value += value5;
		ValueListItem whichItem = ValueListItem.ExtendCurveTypeSmooth;
		InChannelEndExtendType.GetValueListItem(ref whichItem);
		double num = -1.23432101234321E+308;
		for (int i = 0; i < cSDataArray.GetArrayCount(); i++)
		{
			double gemZMin = CSUtilGemRails.GetChannelGemData(cSDataArray, i).GemZMin;
			num = ((!RhinoMath.IsValidDouble(num)) ? gemZMin : Math.Max(num, gemZMin));
		}
		OnCurve inRail = CSUtilGemRails.Rail(cSDataArray);
		if (!CSUtilGemRails.CalculateLeft(cSDataArray.Array, inRail, value, num, out OnCurve rail))
		{
			return UpdateError("CalculateLeft failed (Upper)");
		}
		if (!CSUtilGemRails.CalculateRight(cSDataArray.Array, inRail, value, num, out OnCurve rail2))
		{
			return UpdateError("CalculateRight failed (Upper)");
		}
		if (!CSUtilGemRails.CalculateLeft(cSDataArray.Array, inRail, value, 0.0 - num, out OnCurve rail3))
		{
			return UpdateError("CalculateLeft failed (Lower)");
		}
		if (!CSUtilGemRails.CalculateRight(cSDataArray.Array, inRail, value, 0.0 - num, out OnCurve rail4))
		{
			return UpdateError("CalculateLeft failed (Lower)");
		}
		if (!CSUtilGemRails.IsClosed(cSDataArray.Array))
		{
			IRhinoExtend.Type type;
			switch (whichItem)
			{
			case ValueListItem.ExtendCurveTypeLine:
				type = IRhinoExtend.Type.Line;
				break;
			case ValueListItem.ExtendCurveTypeArc:
				type = IRhinoExtend.Type.Arc;
				break;
			case ValueListItem.ExtendCurveTypeSmooth:
				type = IRhinoExtend.Type.Smooth;
				break;
			default:
				return UpdateError("Invalid extend curve type");
			}
			if (!CSUtilGemRails.CalculateRail(cSDataArray.Array, num, out var rail5))
			{
				return UpdateError("CalculateRail failed (Upper)");
			}
			double length = -1.23432101234321E+308;
			rail5.GetLength(ref length);
			double length2 = -1.23432101234321E+308;
			rail.GetLength(ref length2);
			double length3 = -1.23432101234321E+308;
			rail2.GetLength(ref length3);
			double num2 = length2 / length;
			double num3 = length3 / length;
			if (!RhUtil.RhinoExtendCurve(ref rail, type, 0, (CSUtilGemRails.SizeStart(cSDataArray) + value3) * num2))
			{
				return UpdateError("RhinoExtendCurve failed (Left) (Upper) (0)");
			}
			if (!RhUtil.RhinoExtendCurve(ref rail, type, 1, (CSUtilGemRails.SizeEnd(cSDataArray) + value4) * num2))
			{
				return UpdateError("RhinoExtendCurve failed (Left) (Upper) (1)");
			}
			if (!RhUtil.RhinoExtendCurve(ref rail2, type, 0, (CSUtilGemRails.SizeStart(cSDataArray) + value3) * num3))
			{
				return UpdateError("RhinoExtendCurve failed (Right) (Upper) (0)");
			}
			if (!RhUtil.RhinoExtendCurve(ref rail2, type, 1, (CSUtilGemRails.SizeEnd(cSDataArray) + value4) * num3))
			{
				return UpdateError("RhinoExtendCurve failed (Right) (Upper) (1)");
			}
			if (!CSUtilGemRails.CalculateRail(cSDataArray.Array, num, out var rail6))
			{
				return UpdateError("CalculateRail failed (Lower)");
			}
			double length4 = -1.23432101234321E+308;
			rail6.GetLength(ref length4);
			double length5 = -1.23432101234321E+308;
			rail3.GetLength(ref length5);
			double length6 = -1.23432101234321E+308;
			rail4.GetLength(ref length6);
			double num4 = length5 / length4;
			double num5 = length6 / length4;
			if (!RhUtil.RhinoExtendCurve(ref rail3, type, 0, (CSUtilGemRails.SizeStart(cSDataArray) + value3) * num4))
			{
				return UpdateError("RhinoExtendCurve failed (Left) (Lower) (0)");
			}
			if (!RhUtil.RhinoExtendCurve(ref rail3, type, 1, (CSUtilGemRails.SizeEnd(cSDataArray) + value4) * num4))
			{
				return UpdateError("RhinoExtendCurve failed (Left) (Lower) (1)");
			}
			if (!RhUtil.RhinoExtendCurve(ref rail4, type, 0, (CSUtilGemRails.SizeStart(cSDataArray) + value3) * num5))
			{
				return UpdateError("RhinoExtendCurve failed (Right) (Lower) (0)");
			}
			if (!RhUtil.RhinoExtendCurve(ref rail4, type, 1, (CSUtilGemRails.SizeEnd(cSDataArray) + value4) * num5))
			{
				return UpdateError("RhinoExtendCurve failed (Right) (Lower) (1)");
			}
			OnLineCurve onLineCurve = new OnLineCurve(rail.PointAtStart(), rail2.PointAtStart());
			OnLineCurve onLineCurve2 = new OnLineCurve(rail.PointAtEnd(), rail2.PointAtEnd());
			OnLineCurve onLineCurve3 = new OnLineCurve(rail3.PointAtStart(), rail4.PointAtStart());
			OnLineCurve onLineCurve4 = new OnLineCurve(rail3.PointAtEnd(), rail4.PointAtEnd());
			if (!CSUtilCurves.MergeCurves(new IOnCurve[4] { rail, rail2, onLineCurve, onLineCurve2 }, out OnCurve mergedCrv, 0.01))
			{
				return UpdateError("MergeCurves failed (crvUpperBorder)");
			}
			if (!CSUtilCurves.MergeCurves(new IOnCurve[4] { rail3, rail4, onLineCurve3, onLineCurve4 }, out OnCurve mergedCrv2, 0.01))
			{
				return UpdateError("MergeCurves failed (crvLowerBorder)");
			}
			OnBrep splitter = CSUtilSweep.Loft(new IOnCurve[2] { mergedCrv, mergedCrv2 }, IArgsRhinoLoft.eLoftType.ltNormal);
			if (!RhUtil.RhinoBrepSplit(brep, splitter, CSApplicationHelper.Application.Tolerance, out var pieces))
			{
				return UpdateError("RhinoBrepSplit failed");
			}
			if (pieces.Length != 2)
			{
				return UpdateError("RhinoBrepSplit produced an incorrect number of results");
			}
			OnBoundingBox tight_bbox = new OnBoundingBox();
			if (!pieces[0].GetTightBoundingBox(ref tight_bbox))
			{
				return UpdateError("GetTightBoundingBox failed (0)");
			}
			OnBoundingBox tight_bbox2 = new OnBoundingBox();
			if (!pieces[1].GetTightBoundingBox(ref tight_bbox2))
			{
				return UpdateError("GetTightBoundingBox failed (1)");
			}
			OnBrep brep2 = ((tight_bbox.Volume() < tight_bbox2.Volume()) ? pieces[1] : pieces[0]);
			OnBrep srf = ((tight_bbox.Volume() < tight_bbox2.Volume()) ? pieces[0] : pieces[1]);
			CSUtilGeometry.FixBrepSolidOrientation(ref brep2);
			OnLineCurve intersector = new OnLineCurve(onLineCurve.PointAtEnd(), onLineCurve3.PointAtEnd());
			OnLineCurve intersector2 = new OnLineCurve(onLineCurve.PointAtStart(), onLineCurve3.PointAtStart());
			OnLineCurve intersector3 = new OnLineCurve(onLineCurve2.PointAtStart(), onLineCurve4.PointAtStart());
			OnLineCurve intersector4 = new OnLineCurve(onLineCurve2.PointAtEnd(), onLineCurve4.PointAtEnd());
			if (!GetCornerPt(intersector, brep, out var cornerPt))
			{
				return UpdateError("GetCornerPt failed (A)");
			}
			if (!GetCornerPt(intersector2, brep, out var cornerPt2))
			{
				return UpdateError("GetCornerPt failed (B)");
			}
			if (!GetCornerPt(intersector3, brep, out var cornerPt3))
			{
				return UpdateError("GetCornerPt failed (C)");
			}
			if (!GetCornerPt(intersector4, brep, out var cornerPt4))
			{
				return UpdateError("GetCornerPt failed (D)");
			}
			if (!CSUtilBrepCrvs.GetCurveFromBrepEdges_V2(srf, cornerPt, cornerPt2, cornerPt4, out var edge))
			{
				return UpdateError("GetCurvesFromBrepEdgeV2 failed (Start)");
			}
			if (!CSUtilBrepCrvs.GetCurveFromBrepEdges_V2(srf, cornerPt4, cornerPt3, cornerPt, out var edge2))
			{
				return UpdateError("GetCurvesFromBrepEdgeV2 failed (End)");
			}
			if (!CSUtilBrepCrvs.GetCurveFromBrepEdges_V2(srf, cornerPt, cornerPt4, cornerPt2, out var edge3))
			{
				return UpdateError("GetCurvesFromBrepEdgeV2 failed (Left)");
			}
			if (!CSUtilBrepCrvs.GetCurveFromBrepEdges_V2(srf, cornerPt2, cornerPt3, cornerPt, out var edge4))
			{
				return UpdateError("GetCurvesFromBrepEdgeV2 failed (Right)");
			}
			OutSurface.SetData(new CSOnGeometryData(brep2.Convert()));
			OutSurface.Data.SetMaterials(InSurface.Data);
			OutCurveStart.SetData(new CSOnGeometryData(edge.Convert()));
			OutCurveEnd.SetData(new CSOnGeometryData(edge2.Convert()));
			OutCurveLeft.SetData(new CSOnGeometryData(edge3.Convert()));
			OutCurveRight.SetData(new CSOnGeometryData(edge4.Convert()));
		}
		return base.Update();
	}

	private static bool GetCornerPt(IOnCurve intersector, IOnBrep srf, out On3dPoint cornerPt)
	{
		cornerPt = new On3dPoint(OnUtil.On_UNSET_POINT);
		if (!RhUtil.RhinoCurveBrepIntersect(intersector, srf, CSApplicationHelper.Application.Tolerance, out var curves, out var points))
		{
			return false;
		}
		if (points.Count() <= 0 && curves.Length == 0)
		{
			return false;
		}
		On3dPoint on3dPoint = intersector.PointAt(intersector.Domain().Mid());
		double num = -1.23432101234321E+308;
		OnCurve[] array = curves;
		foreach (OnCurve obj in array)
		{
			double t = 0.0;
			obj.GetClosestPoint(on3dPoint, ref t);
			On3dPoint on3dPoint2 = obj.PointAt(t);
			t = on3dPoint2.DistanceTo(on3dPoint);
			if (!RhinoMath.IsValidDouble(num) || t < num)
			{
				cornerPt = on3dPoint2;
				num = t;
			}
		}
		foreach (On3dPoint item in points)
		{
			double num2 = item.DistanceTo(on3dPoint);
			if (!RhinoMath.IsValidDouble(num) || num2 < num)
			{
				cornerPt = item;
				num = num2;
			}
		}
		return RhinoMath.IsValidDouble(num);
	}
}
