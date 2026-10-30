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
		//IL_01e8: Unknown result type (might be due to invalid IL or missing references)
		//IL_01ed: Unknown result type (might be due to invalid IL or missing references)
		//IL_01f2: Unknown result type (might be due to invalid IL or missing references)
		//IL_026e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0292: Unknown result type (might be due to invalid IL or missing references)
		//IL_02b6: Unknown result type (might be due to invalid IL or missing references)
		//IL_02da: Unknown result type (might be due to invalid IL or missing references)
		//IL_0368: Unknown result type (might be due to invalid IL or missing references)
		//IL_038c: Unknown result type (might be due to invalid IL or missing references)
		//IL_03b0: Unknown result type (might be due to invalid IL or missing references)
		//IL_03d4: Unknown result type (might be due to invalid IL or missing references)
		//IL_0404: Unknown result type (might be due to invalid IL or missing references)
		//IL_040b: Expected O, but got Unknown
		//IL_0419: Unknown result type (might be due to invalid IL or missing references)
		//IL_0420: Expected O, but got Unknown
		//IL_042e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0435: Expected O, but got Unknown
		//IL_0443: Unknown result type (might be due to invalid IL or missing references)
		//IL_044a: Expected O, but got Unknown
		//IL_0507: Unknown result type (might be due to invalid IL or missing references)
		//IL_050e: Expected O, but got Unknown
		//IL_0527: Unknown result type (might be due to invalid IL or missing references)
		//IL_052e: Expected O, but got Unknown
		//IL_0595: Unknown result type (might be due to invalid IL or missing references)
		//IL_05a8: Unknown result type (might be due to invalid IL or missing references)
		//IL_05af: Expected O, but got Unknown
		//IL_05bd: Unknown result type (might be due to invalid IL or missing references)
		//IL_05c4: Expected O, but got Unknown
		//IL_05d2: Unknown result type (might be due to invalid IL or missing references)
		//IL_05d9: Expected O, but got Unknown
		//IL_05e1: Expected O, but got Unknown
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
			Type val;
			switch (whichItem)
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
				return UpdateError("Invalid extend curve type");
			}
			if (!CSUtilGemRails.CalculateRail(cSDataArray.Array, num, out var rail5))
			{
				return UpdateError("CalculateRail failed (Upper)");
			}
			double num2 = -1.23432101234321E+308;
			rail5.GetLength(ref num2);
			double num3 = -1.23432101234321E+308;
			rail.GetLength(ref num3);
			double num4 = -1.23432101234321E+308;
			rail2.GetLength(ref num4);
			double num5 = num3 / num2;
			double num6 = num4 / num2;
			if (!RhUtil.RhinoExtendCurve(ref rail, val, 0, (CSUtilGemRails.SizeStart(cSDataArray) + value3) * num5))
			{
				return UpdateError("RhinoExtendCurve failed (Left) (Upper) (0)");
			}
			if (!RhUtil.RhinoExtendCurve(ref rail, val, 1, (CSUtilGemRails.SizeEnd(cSDataArray) + value4) * num5))
			{
				return UpdateError("RhinoExtendCurve failed (Left) (Upper) (1)");
			}
			if (!RhUtil.RhinoExtendCurve(ref rail2, val, 0, (CSUtilGemRails.SizeStart(cSDataArray) + value3) * num6))
			{
				return UpdateError("RhinoExtendCurve failed (Right) (Upper) (0)");
			}
			if (!RhUtil.RhinoExtendCurve(ref rail2, val, 1, (CSUtilGemRails.SizeEnd(cSDataArray) + value4) * num6))
			{
				return UpdateError("RhinoExtendCurve failed (Right) (Upper) (1)");
			}
			if (!CSUtilGemRails.CalculateRail(cSDataArray.Array, num, out var rail6))
			{
				return UpdateError("CalculateRail failed (Lower)");
			}
			double num7 = -1.23432101234321E+308;
			rail6.GetLength(ref num7);
			double num8 = -1.23432101234321E+308;
			rail3.GetLength(ref num8);
			double num9 = -1.23432101234321E+308;
			rail4.GetLength(ref num9);
			double num10 = num8 / num7;
			double num11 = num9 / num7;
			if (!RhUtil.RhinoExtendCurve(ref rail3, val, 0, (CSUtilGemRails.SizeStart(cSDataArray) + value3) * num10))
			{
				return UpdateError("RhinoExtendCurve failed (Left) (Lower) (0)");
			}
			if (!RhUtil.RhinoExtendCurve(ref rail3, val, 1, (CSUtilGemRails.SizeEnd(cSDataArray) + value4) * num10))
			{
				return UpdateError("RhinoExtendCurve failed (Left) (Lower) (1)");
			}
			if (!RhUtil.RhinoExtendCurve(ref rail4, val, 0, (CSUtilGemRails.SizeStart(cSDataArray) + value3) * num11))
			{
				return UpdateError("RhinoExtendCurve failed (Right) (Lower) (0)");
			}
			if (!RhUtil.RhinoExtendCurve(ref rail4, val, 1, (CSUtilGemRails.SizeEnd(cSDataArray) + value4) * num11))
			{
				return UpdateError("RhinoExtendCurve failed (Right) (Lower) (1)");
			}
			OnLineCurve val2 = new OnLineCurve((IOn3dPoint)(object)rail.PointAtStart(), (IOn3dPoint)(object)rail2.PointAtStart());
			OnLineCurve val3 = new OnLineCurve((IOn3dPoint)(object)rail.PointAtEnd(), (IOn3dPoint)(object)rail2.PointAtEnd());
			OnLineCurve val4 = new OnLineCurve((IOn3dPoint)(object)rail3.PointAtStart(), (IOn3dPoint)(object)rail4.PointAtStart());
			OnLineCurve val5 = new OnLineCurve((IOn3dPoint)(object)rail3.PointAtEnd(), (IOn3dPoint)(object)rail4.PointAtEnd());
			if (!CSUtilCurves.MergeCurves((IOnCurve[])(object)new IOnCurve[4]
			{
				(IOnCurve)rail,
				(IOnCurve)rail2,
				(IOnCurve)val2,
				(IOnCurve)val3
			}, out OnCurve mergedCrv, 0.01))
			{
				return UpdateError("MergeCurves failed (crvUpperBorder)");
			}
			if (!CSUtilCurves.MergeCurves((IOnCurve[])(object)new IOnCurve[4]
			{
				(IOnCurve)rail3,
				(IOnCurve)rail4,
				(IOnCurve)val4,
				(IOnCurve)val5
			}, out OnCurve mergedCrv2, 0.01))
			{
				return UpdateError("MergeCurves failed (crvLowerBorder)");
			}
			OnBrep val6 = CSUtilSweep.Loft((IOnCurve[])(object)new IOnCurve[2]
			{
				(IOnCurve)mergedCrv,
				(IOnCurve)mergedCrv2
			}, (eLoftType)0);
			OnBrep[] array = default(OnBrep[]);
			if (!RhUtil.RhinoBrepSplit((IOnBrep)(object)brep, (IOnBrep)(object)val6, CSApplicationHelper.Application.Tolerance, ref array))
			{
				return UpdateError("RhinoBrepSplit failed");
			}
			if (array.Length != 2)
			{
				return UpdateError("RhinoBrepSplit produced an incorrect number of results");
			}
			OnBoundingBox val7 = new OnBoundingBox();
			if (!((OnGeometry)array[0]).GetTightBoundingBox(ref val7))
			{
				return UpdateError("GetTightBoundingBox failed (0)");
			}
			OnBoundingBox val8 = new OnBoundingBox();
			if (!((OnGeometry)array[1]).GetTightBoundingBox(ref val8))
			{
				return UpdateError("GetTightBoundingBox failed (1)");
			}
			OnBrep brep2 = ((val7.Volume() < val8.Volume()) ? array[1] : array[0]);
			OnBrep srf = ((val7.Volume() < val8.Volume()) ? array[0] : array[1]);
			CSUtilGeometry.FixBrepSolidOrientation(ref brep2);
			OnLineCurve val9 = new OnLineCurve((IOn3dPoint)(object)((OnCurve)val2).PointAtEnd(), (IOn3dPoint)(object)((OnCurve)val4).PointAtEnd());
			OnLineCurve intersector = new OnLineCurve((IOn3dPoint)(object)((OnCurve)val2).PointAtStart(), (IOn3dPoint)(object)((OnCurve)val4).PointAtStart());
			OnLineCurve intersector2 = new OnLineCurve((IOn3dPoint)(object)((OnCurve)val3).PointAtStart(), (IOn3dPoint)(object)((OnCurve)val5).PointAtStart());
			OnLineCurve intersector3 = new OnLineCurve((IOn3dPoint)(object)((OnCurve)val3).PointAtEnd(), (IOn3dPoint)(object)((OnCurve)val5).PointAtEnd());
			if (!GetCornerPt((IOnCurve)val9, (IOnBrep)(object)brep, out var cornerPt))
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
