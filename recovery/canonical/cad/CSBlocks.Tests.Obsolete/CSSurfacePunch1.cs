using System;
using CS;
using CS.CSObjects.CSBlocks;
using CS.Parametric;
using CS.Parametric.Channels;
using CSBlockDataUtils;
using CSBlockUtil;
using CSCompatibility;
using CSDatas;
using RMA.OpenNURBS;
using RMA.Rhino;
using Rhino;

namespace CSBlocks.Tests.Obsolete;

[Obsolete]
[CSBlock("SurfacePunch_V1", "{42305A48-6DD0-4214-BCBA-1AD89A012CAF}")]
public sealed class CSSurfacePunch1 : CSBlock
{
	public CSInputChannel InSortedGems { get; }

	public CSInputChannel InSurface { get; }

	public CSInputChannel InChannelSpacing { get; }

	public CSInputChannel InChannelSpacing2 { get; }

	public CSInputChannel InChannelSpacingStart { get; }

	public CSInputChannel InChannelSpacingEnd { get; }

	public CSInputChannel InSplitDistance { get; }

	public CSOutputChannel OutSurface { get; }

	public CSOutputChannel OutCurveStart { get; }

	public CSOutputChannel OutCurveEnd { get; }

	public CSOutputChannel OutCurveLeft { get; }

	public CSOutputChannel OutCurveRight { get; }

	public CSSurfacePunch1(string identifier)
		: base(identifier)
	{
		InSortedGems = new CSInputChannel(GetPropertyName(() => InSortedGems), this, ChannelDataType.Gem, ChannelArrayType.Individual);
		InSurface = new CSInputChannel(GetPropertyName(() => InSurface), this, ChannelDataType.Brep, ChannelArrayType.Array | ChannelArrayType.Individual);
		InChannelSpacing = new CSInputChannel(GetPropertyName(() => InChannelSpacing), this, ChannelDataType.Double, ChannelArrayType.Individual);
		InChannelSpacing2 = new CSInputChannel(GetPropertyName(() => InChannelSpacing2), this, ChannelDataType.Double, ChannelArrayType.Individual);
		InChannelSpacingStart = new CSInputChannel(GetPropertyName(() => InChannelSpacingStart), this, ChannelDataType.Double, ChannelArrayType.Individual);
		InChannelSpacingEnd = new CSInputChannel(GetPropertyName(() => InChannelSpacingEnd), this, ChannelDataType.Double, ChannelArrayType.Individual);
		InSplitDistance = new CSInputChannel(GetPropertyName(() => InSplitDistance), this, ChannelDataType.Double, ChannelArrayType.Individual);
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
		InChannelSpacing.GetDouble(ref value);
		double value2 = 0.0;
		InChannelSpacing2.GetDouble(ref value2);
		value += value2;
		double value3 = 0.0;
		InChannelSpacingStart.GetDouble(ref value3);
		double value4 = 0.0;
		InChannelSpacingEnd.GetDouble(ref value4);
		double value5 = 2.0;
		InSplitDistance.GetDouble(ref value5);
		CSChannelGemData channelGemData = CSUtilGemRails.GetChannelGemData(cSDataArray, 0);
		On3dPoint brepPoint = new On3dPoint(OnUtil.On_UNSET_POINT);
		if (!CSUtilGeometry.GetClosestPointToBrep(brep, channelGemData.Center.Convert(), ref brepPoint))
		{
			return UpdateError("GetClosestPointToBrep failed");
		}
		OnCurve inRail = CSUtilGemRails.Rail(cSDataArray);
		if (!CSUtilGemRails.CalculateLeft(cSDataArray.Array, inRail, value, value5, out OnCurve rail))
		{
			return UpdateError("CalculateLeft failed");
		}
		if (!CSUtilGemRails.CalculateRight(cSDataArray.Array, inRail, value, value5, out OnCurve rail2))
		{
			return UpdateError("CalculateRight failed");
		}
		if (!CSUtilGemRails.CalculateLeft(cSDataArray.Array, inRail, value, 0.0 - value5, out OnCurve rail3))
		{
			return UpdateError("CalculateLeft failed (Offset)");
		}
		if (!CSUtilGemRails.CalculateRight(cSDataArray.Array, inRail, value, 0.0 - value5, out OnCurve rail4))
		{
			return UpdateError("CalculateLeft failed (Offset)");
		}
		double length = -1.23432101234321E+308;
		rail.GetLength(ref length);
		double length2 = -1.23432101234321E+308;
		rail3.GetLength(ref length2);
		double num = length2 / length;
		double length3 = -1.23432101234321E+308;
		rail2.GetLength(ref length3);
		double length4 = -1.23432101234321E+308;
		rail4.GetLength(ref length4);
		double num2 = length4 / length3;
		RhUtil.RhinoExtendCurve(ref rail, IRhinoExtend.Type.Smooth, 0, CSUtilGemRails.SizeStart(cSDataArray) + value3);
		RhUtil.RhinoExtendCurve(ref rail, IRhinoExtend.Type.Smooth, 1, CSUtilGemRails.SizeEnd(cSDataArray) + value4);
		RhUtil.RhinoExtendCurve(ref rail2, IRhinoExtend.Type.Smooth, 0, CSUtilGemRails.SizeStart(cSDataArray) + value3);
		RhUtil.RhinoExtendCurve(ref rail2, IRhinoExtend.Type.Smooth, 1, CSUtilGemRails.SizeEnd(cSDataArray) + value4);
		RhUtil.RhinoExtendCurve(ref rail3, IRhinoExtend.Type.Smooth, 0, (CSUtilGemRails.SizeStart(cSDataArray) + value3) * num);
		RhUtil.RhinoExtendCurve(ref rail3, IRhinoExtend.Type.Smooth, 1, (CSUtilGemRails.SizeEnd(cSDataArray) + value4) * num);
		RhUtil.RhinoExtendCurve(ref rail4, IRhinoExtend.Type.Smooth, 0, (CSUtilGemRails.SizeStart(cSDataArray) + value3) * num2);
		RhUtil.RhinoExtendCurve(ref rail4, IRhinoExtend.Type.Smooth, 1, (CSUtilGemRails.SizeEnd(cSDataArray) + value4) * num2);
		if (!rail.IsClosed() || !rail2.IsClosed())
		{
			OnLineCurve onLineCurve = new OnLineCurve(rail.PointAtStart(), rail2.PointAtStart());
			OnLineCurve onLineCurve2 = new OnLineCurve(rail.PointAtEnd(), rail2.PointAtEnd());
			OnLineCurve onLineCurve3 = new OnLineCurve(rail3.PointAtStart(), rail4.PointAtStart());
			OnLineCurve onLineCurve4 = new OnLineCurve(rail3.PointAtEnd(), rail4.PointAtEnd());
			if (!CSUtilCurves.MergeCurves(new IOnCurve[4] { rail, rail2, onLineCurve, onLineCurve2 }, out OnCurve mergedCrv, 0.01))
			{
				UpdateError("MergeCurves failed (crvBorder)");
			}
			if (!CSUtilCurves.MergeCurves(new IOnCurve[4] { rail3, rail4, onLineCurve3, onLineCurve4 }, out OnCurve mergedCrv2, 0.01))
			{
				UpdateError("MergeCurves failed (offsetCrvBorder)");
			}
			IOnCurve[] profiles = new OnCurve[2] { mergedCrv, mergedCrv2 };
			OnBrep splitter = CSUtilSweep.Loft(profiles, IArgsRhinoLoft.eLoftType.ltNormal);
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
