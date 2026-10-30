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
		//IL_00dd: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e4: Expected O, but got Unknown
		//IL_02bc: Unknown result type (might be due to invalid IL or missing references)
		//IL_02c3: Expected O, but got Unknown
		//IL_02d1: Unknown result type (might be due to invalid IL or missing references)
		//IL_02d8: Expected O, but got Unknown
		//IL_02e6: Unknown result type (might be due to invalid IL or missing references)
		//IL_02ed: Expected O, but got Unknown
		//IL_02fb: Unknown result type (might be due to invalid IL or missing references)
		//IL_0302: Expected O, but got Unknown
		//IL_03c3: Unknown result type (might be due to invalid IL or missing references)
		//IL_03ca: Expected O, but got Unknown
		//IL_03e3: Unknown result type (might be due to invalid IL or missing references)
		//IL_03ea: Expected O, but got Unknown
		//IL_0449: Unknown result type (might be due to invalid IL or missing references)
		//IL_045c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0463: Expected O, but got Unknown
		//IL_0471: Unknown result type (might be due to invalid IL or missing references)
		//IL_0478: Expected O, but got Unknown
		//IL_0486: Unknown result type (might be due to invalid IL or missing references)
		//IL_048d: Expected O, but got Unknown
		//IL_0495: Expected O, but got Unknown
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
		if (!CSUtilGeometry.GetClosestPointToBrep((IOnBrep)(object)brep, (IOn3dPoint)(object)channelGemData.Center.Convert(), ref brepPoint))
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
		double num = -1.23432101234321E+308;
		rail.GetLength(ref num);
		double num2 = -1.23432101234321E+308;
		rail3.GetLength(ref num2);
		double num3 = num2 / num;
		double num4 = -1.23432101234321E+308;
		rail2.GetLength(ref num4);
		double num5 = -1.23432101234321E+308;
		rail4.GetLength(ref num5);
		double num6 = num5 / num4;
		RhUtil.RhinoExtendCurve(ref rail, (Type)2, 0, CSUtilGemRails.SizeStart(cSDataArray) + value3);
		RhUtil.RhinoExtendCurve(ref rail, (Type)2, 1, CSUtilGemRails.SizeEnd(cSDataArray) + value4);
		RhUtil.RhinoExtendCurve(ref rail2, (Type)2, 0, CSUtilGemRails.SizeStart(cSDataArray) + value3);
		RhUtil.RhinoExtendCurve(ref rail2, (Type)2, 1, CSUtilGemRails.SizeEnd(cSDataArray) + value4);
		RhUtil.RhinoExtendCurve(ref rail3, (Type)2, 0, (CSUtilGemRails.SizeStart(cSDataArray) + value3) * num3);
		RhUtil.RhinoExtendCurve(ref rail3, (Type)2, 1, (CSUtilGemRails.SizeEnd(cSDataArray) + value4) * num3);
		RhUtil.RhinoExtendCurve(ref rail4, (Type)2, 0, (CSUtilGemRails.SizeStart(cSDataArray) + value3) * num6);
		RhUtil.RhinoExtendCurve(ref rail4, (Type)2, 1, (CSUtilGemRails.SizeEnd(cSDataArray) + value4) * num6);
		if (!rail.IsClosed() || !rail2.IsClosed())
		{
			OnLineCurve val = new OnLineCurve((IOn3dPoint)(object)rail.PointAtStart(), (IOn3dPoint)(object)rail2.PointAtStart());
			OnLineCurve val2 = new OnLineCurve((IOn3dPoint)(object)rail.PointAtEnd(), (IOn3dPoint)(object)rail2.PointAtEnd());
			OnLineCurve val3 = new OnLineCurve((IOn3dPoint)(object)rail3.PointAtStart(), (IOn3dPoint)(object)rail4.PointAtStart());
			OnLineCurve val4 = new OnLineCurve((IOn3dPoint)(object)rail3.PointAtEnd(), (IOn3dPoint)(object)rail4.PointAtEnd());
			if (!CSUtilCurves.MergeCurves((IOnCurve[])(object)new IOnCurve[4]
			{
				(IOnCurve)rail,
				(IOnCurve)rail2,
				(IOnCurve)val,
				(IOnCurve)val2
			}, out OnCurve mergedCrv, 0.01))
			{
				UpdateError("MergeCurves failed (crvBorder)");
			}
			if (!CSUtilCurves.MergeCurves((IOnCurve[])(object)new IOnCurve[4]
			{
				(IOnCurve)rail3,
				(IOnCurve)rail4,
				(IOnCurve)val3,
				(IOnCurve)val4
			}, out OnCurve mergedCrv2, 0.01))
			{
				UpdateError("MergeCurves failed (offsetCrvBorder)");
			}
			IOnCurve[] profiles = (IOnCurve[])(object)new OnCurve[2] { mergedCrv, mergedCrv2 };
			OnBrep val5 = CSUtilSweep.Loft(profiles, (eLoftType)0);
			OnBrep[] array = default(OnBrep[]);
			if (!RhUtil.RhinoBrepSplit((IOnBrep)(object)brep, (IOnBrep)(object)val5, CSApplicationHelper.Application.Tolerance, ref array))
			{
				return UpdateError("RhinoBrepSplit failed");
			}
			if (array.Length != 2)
			{
				return UpdateError("RhinoBrepSplit produced an incorrect number of results");
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
			OnLineCurve val8 = new OnLineCurve((IOn3dPoint)(object)((OnCurve)val).PointAtEnd(), (IOn3dPoint)(object)((OnCurve)val3).PointAtEnd());
			OnLineCurve intersector = new OnLineCurve((IOn3dPoint)(object)((OnCurve)val).PointAtStart(), (IOn3dPoint)(object)((OnCurve)val3).PointAtStart());
			OnLineCurve intersector2 = new OnLineCurve((IOn3dPoint)(object)((OnCurve)val2).PointAtStart(), (IOn3dPoint)(object)((OnCurve)val4).PointAtStart());
			OnLineCurve intersector3 = new OnLineCurve((IOn3dPoint)(object)((OnCurve)val2).PointAtEnd(), (IOn3dPoint)(object)((OnCurve)val4).PointAtEnd());
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
