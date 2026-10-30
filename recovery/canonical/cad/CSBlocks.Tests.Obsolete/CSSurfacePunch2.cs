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

namespace CSBlocks.Tests.Obsolete;

[Obsolete]
[CSBlock("SurfacePunch_V2", "{BBD8B6A6-6459-4808-9390-65ED4B12B1D2}")]
public sealed class CSSurfacePunch2 : CSBlock
{
	public CSInputChannel InSortedGems { get; }

	public CSInputChannel InSurface { get; }

	public CSInputChannel InChannelSideSpacing { get; }

	public CSInputChannel InChannelEndProfile { get; }

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

	public CSSurfacePunch2(string identifier)
		: base(identifier)
	{
		InSortedGems = new CSInputChannel(GetPropertyName(() => InSortedGems), this, ChannelDataType.Gem, ChannelArrayType.Individual);
		InSurface = new CSInputChannel(GetPropertyName(() => InSurface), this, ChannelDataType.Brep, ChannelArrayType.Array | ChannelArrayType.Individual);
		InChannelSideSpacing = new CSInputChannel(GetPropertyName(() => InChannelSideSpacing), this, ChannelDataType.Double, ChannelArrayType.Individual);
		InChannelEndProfile = new CSInputChannel(GetPropertyName(() => InChannelEndProfile), this, ChannelDataType.Curve, ChannelArrayType.Individual);
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
		OnCurve onCurve = InChannelEndProfile.GetNurbsCurve();
		if (onCurve == null)
		{
			On3dPoint arg = new On3dPoint(-5.0, 0.0, 0.0);
			On3dPoint arg2 = new On3dPoint(5.0, 0.0, 0.0);
			onCurve = new OnLineCurve(arg, arg2);
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
		if (!CalculateChannelBorderCurves(cSDataArray, onCurve, value, value3, value4, whichItem, num, out var crvStart, out var crvEnd, out var crvLeft, out var crvRight, out var msg))
		{
			return UpdateError(msg + "(Upper)");
		}
		if (!CalculateChannelBorderCurves(cSDataArray, onCurve, value, value3, value4, whichItem, 0.0 - num, out var crvStart2, out var crvEnd2, out var crvLeft2, out var crvRight2, out var msg2))
		{
			return UpdateError(msg2 + "(Lower)");
		}
		if (!CSUtilCurves.MergeCurves(new IOnCurve[4] { crvLeft, crvRight, crvStart, crvEnd }, out OnCurve mergedCrv, 0.01))
		{
			UpdateError("MergeCurves failed (crvBorderUpper)");
		}
		if (!CSUtilCurves.MergeCurves(new IOnCurve[4] { crvLeft2, crvRight2, crvStart2, crvEnd2 }, out OnCurve mergedCrv2, 0.01))
		{
			UpdateError("MergeCurves failed (crvBorderLower)");
		}
		On3dVector arg3 = mergedCrv.TangentAt(mergedCrv.Domain().Min());
		On3dVector on3dVector = mergedCrv2.TangentAt(mergedCrv2.Domain().Min());
		if (OnUtil.ON_DotProduct(arg3, on3dVector) < 0.0)
		{
			on3dVector.Reverse();
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
		for (int j = 0; j < pieces.Length; j++)
		{
			CSUtilGeometry.FixBrepSolidOrientation(ref pieces[j]);
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
		OnLineCurve intersector = new OnLineCurve(crvStart.PointAtStart(), crvStart2.PointAtStart());
		OnLineCurve intersector2 = new OnLineCurve(crvStart.PointAtEnd(), crvStart2.PointAtEnd());
		OnLineCurve intersector3 = new OnLineCurve(crvEnd.PointAtEnd(), crvEnd2.PointAtEnd());
		OnLineCurve intersector4 = new OnLineCurve(crvEnd.PointAtStart(), crvEnd2.PointAtStart());
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
		return base.Update();
	}

	private static bool CalculateChannelBorderCurves(CSDataArray channelGemDatas, IOnCurve channelEndProfile, double channelSideSpacing, double channelEndSpacingStart, double channelEndSpacingEnd, ValueListItem channelEndExtendType, double splitDistance, out OnCurve crvStart, out OnCurve crvEnd, out OnCurve crvLeft, out OnCurve crvRight, out string msg)
	{
		crvStart = null;
		crvEnd = null;
		crvLeft = null;
		crvRight = null;
		msg = string.Empty;
		OnCurve inRail = CSUtilGemRails.Rail(channelGemDatas);
		if (!CSUtilGemRails.CalculateLeft(channelGemDatas.Array, inRail, channelSideSpacing, splitDistance, out crvLeft))
		{
			msg = "CalculateLeft failed";
			return false;
		}
		if (!CSUtilGemRails.CalculateRight(channelGemDatas.Array, inRail, channelSideSpacing, splitDistance, out crvRight))
		{
			msg = "CalculateRight failed";
			return false;
		}
		if (CSUtilGemRails.IsClosed(channelGemDatas.Array))
		{
			return true;
		}
		IRhinoExtend.Type type;
		switch (channelEndExtendType)
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
			msg = "Invalid extend curve type";
			return false;
		}
		if (!CSUtilGemRails.CalculateRail(channelGemDatas.Array, splitDistance, out var rail))
		{
			msg = "CalculateRail failed";
			return false;
		}
		double length = -1.23432101234321E+308;
		rail.GetLength(ref length);
		double length2 = -1.23432101234321E+308;
		crvLeft.GetLength(ref length2);
		double length3 = -1.23432101234321E+308;
		crvRight.GetLength(ref length3);
		double num = length2 / length;
		double num2 = length3 / length;
		if (!RhUtil.RhinoExtendCurve(ref crvLeft, type, 0, (CSUtilGemRails.SizeStart(channelGemDatas) + channelEndSpacingStart) * num))
		{
			msg = "RhinoExtendCurve failed (Left Start)";
			return false;
		}
		if (!RhUtil.RhinoExtendCurve(ref crvLeft, type, 1, (CSUtilGemRails.SizeEnd(channelGemDatas) + channelEndSpacingEnd) * num))
		{
			msg = "RhinoExtendCurve failed (Left End)";
			return false;
		}
		if (!RhUtil.RhinoExtendCurve(ref crvRight, type, 0, (CSUtilGemRails.SizeStart(channelGemDatas) + channelEndSpacingStart) * num2))
		{
			msg = "RhinoExtendCurve failed (Right Start)";
			return false;
		}
		if (!RhUtil.RhinoExtendCurve(ref crvRight, type, 1, (CSUtilGemRails.SizeEnd(channelGemDatas) + channelEndSpacingEnd) * num2))
		{
			msg = "RhinoExtendCurve failed (Right End)";
			return false;
		}
		OnBoundingBox tight_bbox = new OnBoundingBox();
		if (!channelEndProfile.GetTightBoundingBox(ref tight_bbox))
		{
			msg = "GetTightBoundingBox failed";
			return false;
		}
		double num3 = tight_bbox.Max().x - tight_bbox.Min().x;
		double num4 = crvLeft.PointAtStart().DistanceTo(crvRight.PointAtStart());
		double num5 = num4 / num3;
		OnXform onXform = new OnXform();
		onXform.Scale(num5, num5, num5);
		double num6 = crvLeft.PointAtEnd().DistanceTo(crvRight.PointAtEnd());
		double num7 = num6 / num3;
		OnXform onXform2 = new OnXform();
		onXform2.Scale(num7, num7, num7);
		On3dPoint source = new On3dPoint(0.0 - num4 / 2.0, 0.0, 0.0);
		IOn3dVector on_xaxis = OnUtil.On_xaxis;
		IOn3dVector on_yaxis = OnUtil.On_yaxis;
		On3dPoint destination = new On3dPoint(crvRight.PointAtStart());
		On3dVector on3dVector = new On3dVector(crvLeft.PointAtStart() - crvRight.PointAtStart());
		on3dVector.Unitize();
		On3dVector on3dVector2 = new On3dVector(crvLeft.TangentAt(crvLeft.Domain().Min()) + crvRight.TangentAt(crvRight.Domain().Min()));
		on3dVector2.Reverse();
		on3dVector2.Unitize();
		OnXform onXform3 = CSBlockUtil.CSUtilMath.Orient3Point(source, on_xaxis, on_yaxis, destination, on3dVector, on3dVector2);
		On3dPoint source2 = new On3dPoint(0.0 - num6 / 2.0, 0.0, 0.0);
		IOn3dVector on_xaxis2 = OnUtil.On_xaxis;
		IOn3dVector on_yaxis2 = OnUtil.On_yaxis;
		On3dPoint destination2 = new On3dPoint(crvRight.PointAtEnd());
		On3dVector on3dVector3 = new On3dVector(crvLeft.PointAtEnd() - crvRight.PointAtEnd());
		on3dVector3.Unitize();
		On3dVector on3dVector4 = new On3dVector(crvLeft.TangentAt(crvLeft.Domain().Max()) + crvRight.TangentAt(crvRight.Domain().Max()));
		on3dVector4.Unitize();
		OnXform onXform4 = CSBlockUtil.CSUtilMath.Orient3Point(source2, on_xaxis2, on_yaxis2, destination2, on3dVector3, on3dVector4);
		OnCurve onCurve = channelEndProfile.DuplicateCurve();
		onCurve.Transform(onXform3 * onXform);
		OnCurve onCurve2 = channelEndProfile.DuplicateCurve();
		onCurve2.Reverse();
		onCurve2.Transform(onXform4 * onXform2);
		if (!CSUtilGemRails.CalculateLeft(channelGemDatas.Array, inRail, channelSideSpacing + 1.0, splitDistance, out OnCurve rail2))
		{
			msg = "CalculateLeft failed";
			return false;
		}
		if (!CSUtilGemRails.CalculateRight(channelGemDatas.Array, inRail, channelSideSpacing + 1.0, splitDistance, out OnCurve rail3))
		{
			msg = "CalculateRight failed";
			return false;
		}
		double length4 = -1.23432101234321E+308;
		rail2.GetLength(ref length4);
		double length5 = -1.23432101234321E+308;
		rail3.GetLength(ref length5);
		double num8 = length4 / length;
		double num9 = length5 / length;
		OnBoundingBox onBoundingBox = new OnBoundingBox(tight_bbox);
		onBoundingBox.Transform(onXform);
		double num10 = onBoundingBox.Max().y - onBoundingBox.Min().y;
		OnBoundingBox onBoundingBox2 = new OnBoundingBox(tight_bbox);
		onBoundingBox2.Transform(onXform2);
		double num11 = onBoundingBox2.Max().y - onBoundingBox2.Min().y;
		if (!RhUtil.RhinoExtendCurve(ref rail2, type, 0, (CSUtilGemRails.SizeStart(channelGemDatas) + channelEndSpacingStart + num10 + 1.0) * num8))
		{
			msg = "RhinoExtendCurve failed (Left Extra Start)";
			return false;
		}
		if (!RhUtil.RhinoExtendCurve(ref rail2, type, 1, (CSUtilGemRails.SizeEnd(channelGemDatas) + channelEndSpacingEnd + num11 + 1.0) * num8))
		{
			msg = "RhinoExtendCurve failed (Left Extra End)";
			return false;
		}
		if (!RhUtil.RhinoExtendCurve(ref rail3, type, 0, (CSUtilGemRails.SizeStart(channelGemDatas) + channelEndSpacingStart + num10 + 1.0) * num9))
		{
			msg = "RhinoExtendCurve failed (Right Extra Start)";
			return false;
		}
		if (!RhUtil.RhinoExtendCurve(ref rail3, type, 1, (CSUtilGemRails.SizeEnd(channelGemDatas) + channelEndSpacingEnd + num11 + 1.0) * num9))
		{
			msg = "RhinoExtendCurve failed (Right Extra End)";
			return false;
		}
		OnNurbsSurface onNurbsSurface = new OnNurbsSurface();
		if ((double)onNurbsSurface.CreateRuledSurface(rail2, rail3) <= 0.0)
		{
			msg = "CreateRuledSurface failed";
			return false;
		}
		IOn3dVector profileNormal = OnUtil.ON_CrossProduct(on3dVector, on3dVector2);
		if (!CSUtilCurves.CurveFrom2ViewSimple(onCurve, profileNormal, onNurbsSurface, out crvStart, out var msg2))
		{
			msg = msg2 + " (Start)";
			return false;
		}
		IOn3dVector profileNormal2 = OnUtil.ON_CrossProduct(on3dVector3, on3dVector4);
		if (!CSUtilCurves.CurveFrom2ViewSimple(onCurve2, profileNormal2, onNurbsSurface, out crvEnd, out var msg3))
		{
			msg = msg3 + " (End)";
			return false;
		}
		return true;
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
