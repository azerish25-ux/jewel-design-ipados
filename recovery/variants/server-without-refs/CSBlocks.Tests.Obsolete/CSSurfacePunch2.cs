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
using Rhino.Geometry;

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
		//IL_007a: Unknown result type (might be due to invalid IL or missing references)
		//IL_009a: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a1: Expected O, but got Unknown
		//IL_00a8: Expected O, but got Unknown
		//IL_00a3: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a9: Expected O, but got Unknown
		//IL_0310: Unknown result type (might be due to invalid IL or missing references)
		//IL_0317: Expected O, but got Unknown
		//IL_0330: Unknown result type (might be due to invalid IL or missing references)
		//IL_0337: Expected O, but got Unknown
		//IL_0396: Unknown result type (might be due to invalid IL or missing references)
		//IL_03a9: Unknown result type (might be due to invalid IL or missing references)
		//IL_03b0: Expected O, but got Unknown
		//IL_03be: Unknown result type (might be due to invalid IL or missing references)
		//IL_03c5: Expected O, but got Unknown
		//IL_03d3: Unknown result type (might be due to invalid IL or missing references)
		//IL_03da: Expected O, but got Unknown
		//IL_03e2: Expected O, but got Unknown
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
		OnCurve val = (OnCurve)(object)InChannelEndProfile.GetNurbsCurve();
		if (val == null)
		{
			On3dPoint val2 = new On3dPoint(-5.0, 0.0, 0.0);
			On3dPoint val3 = new On3dPoint(5.0, 0.0, 0.0);
			val = (OnCurve)new OnLineCurve((IOn3dPoint)val2, (IOn3dPoint)(object)val3);
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
		if (!CalculateChannelBorderCurves(cSDataArray, (IOnCurve)(object)val, value, value3, value4, whichItem, num, out var crvStart, out var crvEnd, out var crvLeft, out var crvRight, out var msg))
		{
			return UpdateError(msg + "(Upper)");
		}
		if (!CalculateChannelBorderCurves(cSDataArray, (IOnCurve)(object)val, value, value3, value4, whichItem, 0.0 - num, out var crvStart2, out var crvEnd2, out var crvLeft2, out var crvRight2, out var msg2))
		{
			return UpdateError(msg2 + "(Lower)");
		}
		if (!CSUtilCurves.MergeCurves((IOnCurve[])(object)new IOnCurve[4]
		{
			(IOnCurve)crvLeft,
			(IOnCurve)crvRight,
			(IOnCurve)crvStart,
			(IOnCurve)crvEnd
		}, out OnCurve mergedCrv, 0.01))
		{
			UpdateError("MergeCurves failed (crvBorderUpper)");
		}
		if (!CSUtilCurves.MergeCurves((IOnCurve[])(object)new IOnCurve[4]
		{
			(IOnCurve)crvLeft2,
			(IOnCurve)crvRight2,
			(IOnCurve)crvStart2,
			(IOnCurve)crvEnd2
		}, out OnCurve mergedCrv2, 0.01))
		{
			UpdateError("MergeCurves failed (crvBorderLower)");
		}
		On3dVector obj = mergedCrv.TangentAt(mergedCrv.Domain().Min());
		On3dVector val4 = mergedCrv2.TangentAt(mergedCrv2.Domain().Min());
		if (OnUtil.ON_DotProduct((IOn3dVector)(object)obj, (IOn3dVector)(object)val4) < 0.0)
		{
			val4.Reverse();
		}
		OnBrep val5 = CSUtilSweep.Loft((IOnCurve[])(object)new IOnCurve[2]
		{
			(IOnCurve)mergedCrv,
			(IOnCurve)mergedCrv2
		}, (eLoftType)0);
		OnBrep[] array = default(OnBrep[]);
		if (!RhUtil.RhinoBrepSplit((IOnBrep)(object)brep, (IOnBrep)(object)val5, CSApplicationHelper.Application.Tolerance, ref array))
		{
			return UpdateError("RhinoBrepSplit failed");
		}
		if (array.Length != 2)
		{
			return UpdateError("RhinoBrepSplit produced an incorrect number of results");
		}
		for (int j = 0; j < array.Length; j++)
		{
			CSUtilGeometry.FixBrepSolidOrientation(ref array[j]);
		}
		OnBoundingBox val6 = new OnBoundingBox();
		if (!((OnGeometry)array[0]).GetTightBoundingBox(ref val6))
		{
			return UpdateError("GetTightBoundingBox failed (0)");
		}
		OnBoundingBox val7 = new OnBoundingBox();
		if (!((OnGeometry)array[1]).GetTightBoundingBox(ref val7))
		{
			return UpdateError("GetTightBoundingBox failed (1)");
		}
		OnBrep brep2 = ((val6.Volume() < val7.Volume()) ? array[1] : array[0]);
		OnBrep srf = ((val6.Volume() < val7.Volume()) ? array[0] : array[1]);
		OnLineCurve val8 = new OnLineCurve((IOn3dPoint)(object)crvStart.PointAtStart(), (IOn3dPoint)(object)crvStart2.PointAtStart());
		OnLineCurve intersector = new OnLineCurve((IOn3dPoint)(object)crvStart.PointAtEnd(), (IOn3dPoint)(object)crvStart2.PointAtEnd());
		OnLineCurve intersector2 = new OnLineCurve((IOn3dPoint)(object)crvEnd.PointAtEnd(), (IOn3dPoint)(object)crvEnd2.PointAtEnd());
		OnLineCurve intersector3 = new OnLineCurve((IOn3dPoint)(object)crvEnd.PointAtStart(), (IOn3dPoint)(object)crvEnd2.PointAtStart());
		if (!GetCornerPt((IOnCurve)val8, (IOnBrep)(object)brep, out var cornerPt))
		{
			return UpdateError("GetCornerPt failed (A)");
		}
		if (!GetCornerPt((IOnCurve)(object)intersector, (IOnBrep)(object)brep, out var cornerPt2))
		{
			return UpdateError("GetCornerPt failed (B)");
		}
		if (!GetCornerPt((IOnCurve)(object)intersector2, (IOnBrep)(object)brep, out var cornerPt3))
		{
			return UpdateError("GetCornerPt failed (C)");
		}
		if (!GetCornerPt((IOnCurve)(object)intersector3, (IOnBrep)(object)brep, out var cornerPt4))
		{
			return UpdateError("GetCornerPt failed (D)");
		}
		if (!CSUtilBrepCrvs.GetCurveFromBrepEdges_V2((IOnBrep)(object)srf, cornerPt, cornerPt2, cornerPt4, out var edge))
		{
			return UpdateError("GetCurvesFromBrepEdgeV2 failed (Start)");
		}
		if (!CSUtilBrepCrvs.GetCurveFromBrepEdges_V2((IOnBrep)(object)srf, cornerPt4, cornerPt3, cornerPt, out var edge2))
		{
			return UpdateError("GetCurvesFromBrepEdgeV2 failed (End)");
		}
		if (!CSUtilBrepCrvs.GetCurveFromBrepEdges_V2((IOnBrep)(object)srf, cornerPt, cornerPt4, cornerPt2, out var edge3))
		{
			return UpdateError("GetCurvesFromBrepEdgeV2 failed (Left)");
		}
		if (!CSUtilBrepCrvs.GetCurveFromBrepEdges_V2((IOnBrep)(object)srf, cornerPt2, cornerPt3, cornerPt, out var edge4))
		{
			return UpdateError("GetCurvesFromBrepEdgeV2 failed (Right)");
		}
		OutSurface.SetData(new CSOnGeometryData((GeometryBase)(object)brep2.Convert()));
		OutSurface.Data.SetMaterials(InSurface.Data);
		OutCurveStart.SetData(new CSOnGeometryData((GeometryBase)(object)edge.Convert()));
		OutCurveEnd.SetData(new CSOnGeometryData((GeometryBase)(object)edge2.Convert()));
		OutCurveLeft.SetData(new CSOnGeometryData((GeometryBase)(object)edge3.Convert()));
		OutCurveRight.SetData(new CSOnGeometryData((GeometryBase)(object)edge4.Convert()));
		return base.Update();
	}

	private static bool CalculateChannelBorderCurves(CSDataArray channelGemDatas, IOnCurve channelEndProfile, double channelSideSpacing, double channelEndSpacingStart, double channelEndSpacingEnd, ValueListItem channelEndExtendType, double splitDistance, out OnCurve crvStart, out OnCurve crvEnd, out OnCurve crvLeft, out OnCurve crvRight, out string msg)
	{
		//IL_0081: Unknown result type (might be due to invalid IL or missing references)
		//IL_0085: Unknown result type (might be due to invalid IL or missing references)
		//IL_0089: Unknown result type (might be due to invalid IL or missing references)
		//IL_00fe: Unknown result type (might be due to invalid IL or missing references)
		//IL_011e: Unknown result type (might be due to invalid IL or missing references)
		//IL_013f: Unknown result type (might be due to invalid IL or missing references)
		//IL_015f: Unknown result type (might be due to invalid IL or missing references)
		//IL_017e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0185: Expected O, but got Unknown
		//IL_01cf: Unknown result type (might be due to invalid IL or missing references)
		//IL_01d6: Expected O, but got Unknown
		//IL_0201: Unknown result type (might be due to invalid IL or missing references)
		//IL_0208: Expected O, but got Unknown
		//IL_0232: Unknown result type (might be due to invalid IL or missing references)
		//IL_024d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0254: Expected O, but got Unknown
		//IL_0269: Unknown result type (might be due to invalid IL or missing references)
		//IL_0270: Expected O, but got Unknown
		//IL_02a7: Unknown result type (might be due to invalid IL or missing references)
		//IL_02ae: Expected O, but got Unknown
		//IL_02cc: Expected O, but got Unknown
		//IL_02ed: Unknown result type (might be due to invalid IL or missing references)
		//IL_0308: Unknown result type (might be due to invalid IL or missing references)
		//IL_030f: Expected O, but got Unknown
		//IL_0324: Unknown result type (might be due to invalid IL or missing references)
		//IL_032b: Expected O, but got Unknown
		//IL_0362: Unknown result type (might be due to invalid IL or missing references)
		//IL_0369: Expected O, but got Unknown
		//IL_0380: Expected O, but got Unknown
		//IL_0442: Unknown result type (might be due to invalid IL or missing references)
		//IL_0449: Expected O, but got Unknown
		//IL_0470: Unknown result type (might be due to invalid IL or missing references)
		//IL_0477: Expected O, but got Unknown
		//IL_049e: Unknown result type (might be due to invalid IL or missing references)
		//IL_04cb: Unknown result type (might be due to invalid IL or missing references)
		//IL_04f9: Unknown result type (might be due to invalid IL or missing references)
		//IL_0526: Unknown result type (might be due to invalid IL or missing references)
		//IL_0552: Unknown result type (might be due to invalid IL or missing references)
		//IL_0559: Expected O, but got Unknown
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
		Type val;
		switch (channelEndExtendType)
		{
		case ValueListItem.ExtendCurveTypeLine:
			val = (Type)0;
			break;
		case ValueListItem.ExtendCurveTypeArc:
			val = (Type)1;
			break;
		case ValueListItem.ExtendCurveTypeSmooth:
			val = (Type)2;
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
		double num = -1.23432101234321E+308;
		rail.GetLength(ref num);
		double num2 = -1.23432101234321E+308;
		crvLeft.GetLength(ref num2);
		double num3 = -1.23432101234321E+308;
		crvRight.GetLength(ref num3);
		double num4 = num2 / num;
		double num5 = num3 / num;
		if (!RhUtil.RhinoExtendCurve(ref crvLeft, val, 0, (CSUtilGemRails.SizeStart(channelGemDatas) + channelEndSpacingStart) * num4))
		{
			msg = "RhinoExtendCurve failed (Left Start)";
			return false;
		}
		if (!RhUtil.RhinoExtendCurve(ref crvLeft, val, 1, (CSUtilGemRails.SizeEnd(channelGemDatas) + channelEndSpacingEnd) * num4))
		{
			msg = "RhinoExtendCurve failed (Left End)";
			return false;
		}
		if (!RhUtil.RhinoExtendCurve(ref crvRight, val, 0, (CSUtilGemRails.SizeStart(channelGemDatas) + channelEndSpacingStart) * num5))
		{
			msg = "RhinoExtendCurve failed (Right Start)";
			return false;
		}
		if (!RhUtil.RhinoExtendCurve(ref crvRight, val, 1, (CSUtilGemRails.SizeEnd(channelGemDatas) + channelEndSpacingEnd) * num5))
		{
			msg = "RhinoExtendCurve failed (Right End)";
			return false;
		}
		OnBoundingBox val2 = new OnBoundingBox();
		if (!((IOnGeometry)channelEndProfile).GetTightBoundingBox(ref val2))
		{
			msg = "GetTightBoundingBox failed";
			return false;
		}
		double num6 = val2.Max().x - val2.Min().x;
		double num7 = crvLeft.PointAtStart().DistanceTo((IOn3dPoint)(object)crvRight.PointAtStart());
		double num8 = num7 / num6;
		OnXform val3 = new OnXform();
		val3.Scale(num8, num8, num8);
		double num9 = crvLeft.PointAtEnd().DistanceTo((IOn3dPoint)(object)crvRight.PointAtEnd());
		double num10 = num9 / num6;
		OnXform val4 = new OnXform();
		val4.Scale(num10, num10, num10);
		On3dPoint val5 = new On3dPoint(0.0 - num7 / 2.0, 0.0, 0.0);
		IOn3dVector on_xaxis = OnUtil.On_xaxis;
		IOn3dVector on_yaxis = OnUtil.On_yaxis;
		On3dPoint destination = new On3dPoint((IOn3dPoint)(object)crvRight.PointAtStart());
		On3dVector val6 = new On3dVector((IOn3dVector)(object)(crvLeft.PointAtStart() - (IOn3dPoint)(object)crvRight.PointAtStart()));
		val6.Unitize();
		On3dVector val7 = new On3dVector((IOn3dVector)(object)(crvLeft.TangentAt(crvLeft.Domain().Min()) + (IOn3dVector)(object)crvRight.TangentAt(crvRight.Domain().Min())));
		val7.Reverse();
		val7.Unitize();
		OnXform val8 = CSBlockUtil.CSUtilMath.Orient3Point((IOn3dPoint)val5, on_xaxis, on_yaxis, (IOn3dPoint)(object)destination, (IOn3dVector)(object)val6, (IOn3dVector)(object)val7);
		On3dPoint val9 = new On3dPoint(0.0 - num9 / 2.0, 0.0, 0.0);
		IOn3dVector on_xaxis2 = OnUtil.On_xaxis;
		IOn3dVector on_yaxis2 = OnUtil.On_yaxis;
		On3dPoint destination2 = new On3dPoint((IOn3dPoint)(object)crvRight.PointAtEnd());
		On3dVector val10 = new On3dVector((IOn3dVector)(object)(crvLeft.PointAtEnd() - (IOn3dPoint)(object)crvRight.PointAtEnd()));
		val10.Unitize();
		On3dVector val11 = new On3dVector((IOn3dVector)(object)(crvLeft.TangentAt(crvLeft.Domain().Max()) + (IOn3dVector)(object)crvRight.TangentAt(crvRight.Domain().Max())));
		val11.Unitize();
		OnXform val12 = CSBlockUtil.CSUtilMath.Orient3Point((IOn3dPoint)val9, on_xaxis2, on_yaxis2, (IOn3dPoint)(object)destination2, (IOn3dVector)(object)val10, (IOn3dVector)(object)val11);
		OnCurve val13 = channelEndProfile.DuplicateCurve();
		((OnGeometry)val13).Transform((IOnXform)(object)(val8 * (IOnXform)(object)val3));
		OnCurve val14 = channelEndProfile.DuplicateCurve();
		val14.Reverse();
		((OnGeometry)val14).Transform((IOnXform)(object)(val12 * (IOnXform)(object)val4));
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
		double num11 = -1.23432101234321E+308;
		rail2.GetLength(ref num11);
		double num12 = -1.23432101234321E+308;
		rail3.GetLength(ref num12);
		double num13 = num11 / num;
		double num14 = num12 / num;
		OnBoundingBox val15 = new OnBoundingBox((IOnBoundingBox)(object)val2);
		val15.Transform((IOnXform)(object)val3);
		double num15 = val15.Max().y - val15.Min().y;
		OnBoundingBox val16 = new OnBoundingBox((IOnBoundingBox)(object)val2);
		val16.Transform((IOnXform)(object)val4);
		double num16 = val16.Max().y - val16.Min().y;
		if (!RhUtil.RhinoExtendCurve(ref rail2, val, 0, (CSUtilGemRails.SizeStart(channelGemDatas) + channelEndSpacingStart + num15 + 1.0) * num13))
		{
			msg = "RhinoExtendCurve failed (Left Extra Start)";
			return false;
		}
		if (!RhUtil.RhinoExtendCurve(ref rail2, val, 1, (CSUtilGemRails.SizeEnd(channelGemDatas) + channelEndSpacingEnd + num16 + 1.0) * num13))
		{
			msg = "RhinoExtendCurve failed (Left Extra End)";
			return false;
		}
		if (!RhUtil.RhinoExtendCurve(ref rail3, val, 0, (CSUtilGemRails.SizeStart(channelGemDatas) + channelEndSpacingStart + num15 + 1.0) * num14))
		{
			msg = "RhinoExtendCurve failed (Right Extra Start)";
			return false;
		}
		if (!RhUtil.RhinoExtendCurve(ref rail3, val, 1, (CSUtilGemRails.SizeEnd(channelGemDatas) + channelEndSpacingEnd + num16 + 1.0) * num14))
		{
			msg = "RhinoExtendCurve failed (Right Extra End)";
			return false;
		}
		OnNurbsSurface val17 = new OnNurbsSurface();
		if ((double)val17.CreateRuledSurface((IOnCurve)(object)rail2, (IOnCurve)(object)rail3) <= 0.0)
		{
			msg = "CreateRuledSurface failed";
			return false;
		}
		IOn3dVector profileNormal = (IOn3dVector)(object)OnUtil.ON_CrossProduct((IOn3dVector)(object)val6, (IOn3dVector)(object)val7);
		if (!CSUtilCurves.CurveFrom2ViewSimple((IOnCurve)(object)val13, profileNormal, (IOnSurface)(object)val17, out crvStart, out var msg2))
		{
			msg = msg2 + " (Start)";
			return false;
		}
		IOn3dVector profileNormal2 = (IOn3dVector)(object)OnUtil.ON_CrossProduct((IOn3dVector)(object)val10, (IOn3dVector)(object)val11);
		if (!CSUtilCurves.CurveFrom2ViewSimple((IOnCurve)(object)val14, profileNormal2, (IOnSurface)(object)val17, out crvEnd, out var msg3))
		{
			msg = msg3 + " (End)";
			return false;
		}
		return true;
	}

	private static bool GetCornerPt(IOnCurve intersector, IOnBrep srf, out On3dPoint cornerPt)
	{
		//IL_0006: Unknown result type (might be due to invalid IL or missing references)
		//IL_000c: Expected O, but got Unknown
		//IL_00b8: Unknown result type (might be due to invalid IL or missing references)
		//IL_00bf: Expected O, but got Unknown
		cornerPt = new On3dPoint(OnUtil.On_UNSET_POINT);
		OnCurve[] array = default(OnCurve[]);
		On3dPointArray val = default(On3dPointArray);
		if (!RhUtil.RhinoCurveBrepIntersect(intersector, srf, CSApplicationHelper.Application.Tolerance, ref array, ref val))
		{
			return false;
		}
		if (((ArrayOn3dPoint)val).Count() <= 0 && array.Length == 0)
		{
			return false;
		}
		On3dPoint val2 = intersector.PointAt(intersector.Domain().Mid());
		double num = -1.23432101234321E+308;
		OnCurve[] array2 = array;
		foreach (OnCurve obj in array2)
		{
			double num2 = 0.0;
			obj.GetClosestPoint((IOn3dPoint)(object)val2, ref num2);
			On3dPoint val3 = obj.PointAt(num2);
			num2 = val3.DistanceTo((IOn3dPoint)(object)val2);
			if (!RhinoMath.IsValidDouble(num) || num2 < num)
			{
				cornerPt = val3;
				num = num2;
			}
		}
		foreach (On3dPoint item in (ArrayOn3dPoint)val)
		{
			On3dPoint val4 = item;
			double num3 = val4.DistanceTo((IOn3dPoint)(object)val2);
			if (!RhinoMath.IsValidDouble(num) || num3 < num)
			{
				cornerPt = val4;
				num = num3;
			}
		}
		return RhinoMath.IsValidDouble(num);
	}
}
