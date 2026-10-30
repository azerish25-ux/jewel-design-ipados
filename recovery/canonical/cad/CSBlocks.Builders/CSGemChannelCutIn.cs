using System.Collections.Generic;
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

namespace CSBlocks.Builders;

[CSBlock("GemChannelCutIn", "{50E6FF06-BC97-4A76-B8BA-78F2DB6EEFAB}")]
public sealed class CSGemChannelCutIn : CSBlock
{
	public readonly CSInputChannel InCurveEnd1;

	public readonly CSInputChannel InCurveEnd2;

	public readonly CSInputChannel InCurveSide1;

	public readonly CSInputChannel InCurveSide2;

	public readonly CSInputChannel InSortedGems;

	public readonly CSInputChannel InOriginalSurface;

	public readonly CSInputChannel InChannelWallHeight;

	public readonly CSInputChannel InChannelSideSpacing;

	public readonly CSInputChannel InChannelBlend;

	public readonly CSInputChannel InPullDownDistance;

	public readonly CSInputChannel InSplitSurface;

	public readonly CSInputChannel InJoin;

	public readonly CSOutputChannel OutSurface;

	public readonly CSOutputChannel OutChannel;

	public CSGemChannelCutIn(string identifier)
		: base(identifier)
	{
		InCurveEnd1 = new CSInputChannel(this, ChannelDataType.Curve, ChannelArrayType.Individual);
		InCurveEnd2 = new CSInputChannel(this, ChannelDataType.Curve, ChannelArrayType.Individual);
		InCurveSide1 = new CSInputChannel(this, ChannelDataType.Curve, ChannelArrayType.Individual);
		InCurveSide2 = new CSInputChannel(this, ChannelDataType.Curve, ChannelArrayType.Individual);
		InSortedGems = new CSInputChannel(this, ChannelDataType.Gem, ChannelArrayType.Individual);
		InOriginalSurface = new CSInputChannel(this, ChannelDataType.Brep, ChannelArrayType.Individual);
		InChannelWallHeight = new CSInputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual);
		InChannelSideSpacing = new CSInputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual);
		InChannelBlend = new CSInputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual);
		InPullDownDistance = new CSInputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual);
		InSplitSurface = new CSInputChannel(this, ChannelDataType.Any, ChannelArrayType.Any);
		InJoin = new CSInputChannel(this, ChannelDataType.Boolean, ChannelArrayType.Individual);
		OutSurface = new CSOutputChannel(this, ChannelDataType.Surface | ChannelDataType.Brep, ChannelArrayType.Individual);
		OutChannel = new CSOutputChannel(this, ChannelDataType.Surface | ChannelDataType.Brep, ChannelArrayType.Individual);
		SetChannelIdentifiers();
	}

	public override bool Update()
	{
		CSData data = InCurveEnd1.Data;
		if (data == null)
		{
			return UpdateMissing("Curve End 1 missing.");
		}
		OnCurve endCurve = data.GetNurbsCurve().Convert();
		CSData data2 = InCurveEnd2.Data;
		if (data2 == null)
		{
			return UpdateMissing("Curve End 2 missing.");
		}
		OnCurve endCurve2 = data2.GetNurbsCurve().Convert();
		CSData data3 = InCurveSide1.Data;
		if (data3 == null)
		{
			return UpdateMissing("Curve Side 1 missing.");
		}
		OnCurve onCurve = data3.GetNurbsCurve().Convert();
		CSData data4 = InCurveSide2.Data;
		if (data4 == null)
		{
			return UpdateMissing("Curve Side 2 missing.");
		}
		OnCurve onCurve2 = data4.GetNurbsCurve().Convert();
		if (!(InSortedGems.Data is CSDataArray cSDataArray))
		{
			return UpdateMissing("Missing sorted gems");
		}
		if (cSDataArray.GetArrayCount() <= 0)
		{
			return UpdateMissing("Missing sorted gems");
		}
		IList<CSChannelGemData> keyChannelGems = CSUtilGemRails.GetKeyChannelGems(cSDataArray.Array);
		CSGemData cSGemData = keyChannelGems[0];
		if (cSGemData == null)
		{
			return UpdateMissing("Gem Start");
		}
		CSGemData cSGemData2 = keyChannelGems[keyChannelGems.Count - 1];
		if (cSGemData2 == null)
		{
			return UpdateMissing("Gem End");
		}
		CSData data5 = InOriginalSurface.Data;
		if (data5 == null)
		{
			return UpdateMissing("Original Surface not supplied");
		}
		OnBrep onBrep = data5.GetBrep().Convert();
		if (onBrep == null)
		{
			return UpdateMissing("Original Surface Brep not supplied");
		}
		double value = 0.0;
		InChannelWallHeight.GetDouble(ref value);
		double value2 = 0.0;
		InChannelSideSpacing.GetDouble(ref value2);
		double value3 = 0.0;
		InChannelBlend.GetDouble(ref value3);
		double value4 = 0.0;
		InPullDownDistance.GetDouble(ref value4);
		MakeGemCurves(cSGemData, value, value3, onCurve, onCurve2, onBrep, out var crvLeft, out var crvBase, out var crvRight, out var crvLeftParam, out var crvRightParam);
		MakeGemCurves(cSGemData2, value, value3, onCurve, onCurve2, onBrep, out var crvLeft2, out var crvBase2, out var crvRight2, out var crvLeftParam2, out var crvRightParam2);
		MakeEndCurves(endCurve, onCurve, onCurve2, value3, out var crvLeft3, out var crvBase3, out var crvRight3, out var crvLeftParam3, out var crvRightParam3, workingOnStart: true);
		MakeEndCurves(endCurve2, onCurve, onCurve2, value3, out var crvLeft4, out var crvBase4, out var crvRight4, out var crvLeftParam4, out var crvRightParam4, workingOnStart: false);
		List<IOnCurve> list = new List<IOnCurve>();
		list.Add(crvLeft3);
		list.Add(crvLeft);
		list.Add(crvLeft2);
		list.Add(crvLeft4);
		List<double> list2 = new List<double>();
		list2.Add(crvLeftParam3);
		list2.Add(crvLeftParam);
		list2.Add(crvLeftParam2);
		list2.Add(crvLeftParam4);
		List<double> list3 = new List<double>();
		list3.Add(crvRightParam3);
		list3.Add(crvRightParam);
		list3.Add(crvRightParam2);
		list3.Add(crvRightParam4);
		bool closed = false;
		bool maintainHeight = true;
		if (!CSUtilSweep.Sweep2(onCurve, onCurve2, list.ToArray(), list2.ToArray(), list3.ToArray(), out var breps, closed, maintainHeight))
		{
			return UpdateError("Sweep2");
		}
		if (breps == null || breps.Length != 1)
		{
			return UpdateError("NoReturnFromSweep2");
		}
		OnBrep onBrep2 = breps[0];
		list.Clear();
		list.Add(crvRight3);
		list.Add(crvRight);
		list.Add(crvRight2);
		list.Add(crvRight4);
		if (!CSUtilSweep.Sweep2(onCurve, onCurve2, list.ToArray(), list2.ToArray(), list3.ToArray(), out breps, closed, maintainHeight))
		{
			return UpdateError("Sweep2");
		}
		if (breps == null || breps.Length != 1)
		{
			return UpdateError("NoReturnFromSweep2");
		}
		OnBrep onBrep3 = breps[0];
		OnCurve curveFromBrepEdges = CSUtilBrepCrvs.GetCurveFromBrepEdges(onBrep2, crvLeft3.PointAtEnd(), crvLeft4.PointAtEnd(), crvLeft.PointAtEnd());
		OnCurve curveFromBrepEdges2 = CSUtilBrepCrvs.GetCurveFromBrepEdges(onBrep3, crvRight3.PointAtStart(), crvRight4.PointAtStart(), crvRight.PointAtStart());
		double length = 0.0;
		curveFromBrepEdges.GetLength(ref length);
		double length2 = 0.0;
		curveFromBrepEdges2.GetLength(ref length2);
		double t = 0.0;
		curveFromBrepEdges.GetNormalizedArcLengthPoint(value4 / length, ref t);
		double t2 = 0.0;
		curveFromBrepEdges2.GetNormalizedArcLengthPoint(value4 / length2, ref t2);
		On3dPoint arg = curveFromBrepEdges.PointAt(t);
		On3dPoint arg2 = curveFromBrepEdges2.PointAt(t2);
		OnCurve item = new OnLineCurve(arg, arg2);
		double t3 = 0.0;
		curveFromBrepEdges.GetNormalizedArcLengthPoint((length - value4) / length, ref t3);
		double t4 = 0.0;
		curveFromBrepEdges2.GetNormalizedArcLengthPoint((length2 - value4) / length2, ref t4);
		On3dPoint arg3 = curveFromBrepEdges.PointAt(t3);
		On3dPoint arg4 = curveFromBrepEdges2.PointAt(t4);
		OnCurve item2 = new OnLineCurve(arg3, arg4);
		list2.Clear();
		double t5 = 0.0;
		curveFromBrepEdges.GetClosestPoint(crvBase3.PointAtStart(), ref t5, CSApplicationHelper.Application.Tolerance);
		list2.Add(t5);
		if (value4 > 0.01)
		{
			list2.Add(t);
		}
		curveFromBrepEdges.GetClosestPoint(crvBase.PointAtStart(), ref t5, CSApplicationHelper.Application.Tolerance);
		list2.Add(t5);
		curveFromBrepEdges.GetClosestPoint(crvBase2.PointAtStart(), ref t5, CSApplicationHelper.Application.Tolerance);
		list2.Add(t5);
		if (value4 > 0.01)
		{
			list2.Add(t3);
		}
		curveFromBrepEdges.GetClosestPoint(crvBase4.PointAtStart(), ref t5, CSApplicationHelper.Application.Tolerance);
		list2.Add(t5);
		list3.Clear();
		curveFromBrepEdges2.GetClosestPoint(crvBase3.PointAtEnd(), ref t5, CSApplicationHelper.Application.Tolerance);
		list3.Add(t5);
		if (value4 > 0.01)
		{
			list3.Add(t2);
		}
		curveFromBrepEdges2.GetClosestPoint(crvBase.PointAtEnd(), ref t5, CSApplicationHelper.Application.Tolerance);
		list3.Add(t5);
		curveFromBrepEdges2.GetClosestPoint(crvBase2.PointAtEnd(), ref t5, CSApplicationHelper.Application.Tolerance);
		list3.Add(t5);
		if (value4 > 0.01)
		{
			list3.Add(t4);
		}
		curveFromBrepEdges2.GetClosestPoint(crvBase4.PointAtEnd(), ref t5, CSApplicationHelper.Application.Tolerance);
		list3.Add(t5);
		list = new List<IOnCurve> { crvBase3 };
		if (value4 > 0.01)
		{
			list.Add(item);
		}
		list.Add(crvBase);
		list.Add(crvBase2);
		if (value4 > 0.01)
		{
			list.Add(item2);
		}
		list.Add(crvBase4);
		if (!CSUtilSweep.Sweep2(curveFromBrepEdges, curveFromBrepEdges2, list.ToArray(), list2.ToArray(), list3.ToArray(), out breps, closed, maintainHeight))
		{
			return UpdateError("Sweep2");
		}
		if (breps == null || breps.Length != 1)
		{
			return UpdateError("NoReturnFromSweep2");
		}
		OnBrep onBrep4 = breps[0];
		CSData data6 = InSplitSurface.Data;
		bool value5 = true;
		InJoin.GetBoolean(ref value5);
		if (value5)
		{
			List<OnBrep> list4 = new List<OnBrep> { onBrep2, onBrep4, onBrep3 };
			if (data6 != null)
			{
				OnBrep item3 = data6.GetBrep().Convert();
				list4.Add(item3);
			}
			CSUtilRhino.JoinBreps(list4.ToArray(), out var outputBreps, CSApplicationHelper.Application.Tolerance);
			if (outputBreps.Length != 1)
			{
				return UpdateError("channel failed to join");
			}
			OutSurface.SetData(new CSOnGeometryData(outputBreps[0].Convert()));
			OutSurface.Data.SetMaterials(data6);
		}
		else
		{
			CSUtilRhino.JoinBreps(new OnBrep[3] { onBrep2, onBrep4, onBrep3 }, out var outputBreps2, CSApplicationHelper.Application.Tolerance);
			if (outputBreps2.Length != 1)
			{
				return UpdateError("channel failed to join");
			}
			OutSurface.SetData(data6);
			OutChannel.SetData(new CSOnGeometryData(outputBreps2[0].Convert()));
			OutChannel.Data.SetMaterials(data6);
		}
		return base.Update();
	}

	private static void MakeEndCurves(IOnCurve endCurve, IOnCurve leftCurve, IOnCurve rightcurve, double channelBlend, out OnCurve crvLeft, out OnCurve crvBase, out OnCurve crvRight, out double crvLeftParam, out double crvRightParam, bool workingOnStart)
	{
		crvBase = null;
		crvLeft = null;
		crvRight = null;
		crvLeftParam = 0.0;
		crvRightParam = 0.0;
		if (endCurve != null && leftCurve != null && rightcurve != null)
		{
			double length = 0.0;
			endCurve.GetLength(ref length);
			double num = channelBlend / length;
			double s = 1.0 - num;
			double t = 0.0;
			endCurve.GetNormalizedArcLengthPoint(num, ref t);
			double t2 = 0.0;
			endCurve.GetNormalizedArcLengthPoint(s, ref t2);
			crvLeft = endCurve.DuplicateCurve();
			crvLeft.Trim(new OnInterval(crvLeft.Domain().Min(), t));
			crvBase = endCurve.DuplicateCurve();
			crvBase.Trim(new OnInterval(t, t2));
			crvRight = endCurve.DuplicateCurve();
			crvRight.Trim(new OnInterval(t2, crvRight.Domain().Max()));
			if (workingOnStart)
			{
				crvLeftParam = leftCurve.Domain().Min();
				crvRightParam = rightcurve.Domain().Min();
			}
			else
			{
				crvLeftParam = leftCurve.Domain().Max();
				crvRightParam = rightcurve.Domain().Max();
			}
		}
	}

	private static void MakeGemCurves(CSGemData gem, double channelWallHeight, double channelBlend, OnCurve leftCurve, OnCurve rightCurve, OnBrep surface, out OnCurve crvLeft, out OnCurve crvBase, out OnCurve crvRight, out double crvLeftParam, out double crvRightParam)
	{
		IOn3dPoint test_point = gem.YMaxPoint.Convert();
		IOn3dPoint test_point2 = gem.YMinPoint.Convert();
		IOn3dPoint on3dPoint = gem.ZMinPoint.Convert();
		On3dVector on3dVector = new On3dVector(gem.Down.Convert());
		crvLeftParam = 0.0;
		crvRightParam = 0.0;
		crvBase = null;
		crvLeft = null;
		crvRight = null;
		if (leftCurve != null && rightCurve != null)
		{
			leftCurve.GetClosestPoint(test_point2, ref crvLeftParam);
			rightCurve.GetClosestPoint(test_point, ref crvRightParam);
			On3dPoint point = new On3dPoint();
			On3dPoint point2 = new On3dPoint();
			On3dVector tangent = new On3dVector();
			On3dVector tangent2 = new On3dVector();
			leftCurve.EvTangent(crvLeftParam, ref point, ref tangent);
			rightCurve.EvTangent(crvRightParam, ref point2, ref tangent2);
			On3dVector on3dVector2 = point2 - point;
			on3dVector2.Unitize();
			On3dPoint on3dPoint2 = point + on3dVector2 * channelBlend;
			On3dPoint on3dPoint3 = on3dPoint2 + on3dVector * channelWallHeight;
			On3dPoint on3dPoint4 = point2 - on3dVector2 * channelBlend;
			On3dPoint on3dPoint5 = on3dPoint4 + on3dVector * channelWallHeight;
			List<IOnCurve> list = new List<IOnCurve>();
			OnCurve[] output = null;
			OnCurve onCurve3;
			OnCurve onCurve6;
			if (channelWallHeight > channelBlend)
			{
				OnCurve onCurve = new OnLineCurve(on3dPoint2, on3dPoint3);
				OnCurve onCurve2 = new OnLineCurve(on3dPoint3, on3dPoint);
				CSUtilGeometry.TrimEndOfCrv(onCurve, channelBlend);
				CSUtilGeometry.TrimStartOfCrv(onCurve2, channelBlend);
				OnCurve item = CSUtilCurves.BlendCurve(onCurve, onCurve2);
				list.Clear();
				list.Add(onCurve);
				list.Add(item);
				list.Add(onCurve2);
				RhUtil.RhinoMergeCurves(list.ToArray(), ref output, CSApplicationHelper.Application.Tolerance);
				onCurve3 = output[0];
				OnCurve onCurve4 = new OnLineCurve(on3dPoint5, on3dPoint4);
				OnCurve onCurve5 = new OnLineCurve(on3dPoint, on3dPoint5);
				CSUtilGeometry.TrimEndOfCrv(onCurve5, channelBlend);
				CSUtilGeometry.TrimStartOfCrv(onCurve4, channelBlend);
				OnCurve item2 = CSUtilCurves.BlendCurve(onCurve5, onCurve4);
				list.Clear();
				list.Add(onCurve5);
				list.Add(item2);
				list.Add(onCurve4);
				output = null;
				RhUtil.RhinoMergeCurves(list.ToArray(), ref output, CSApplicationHelper.Application.Tolerance);
				onCurve6 = output[0];
			}
			else
			{
				onCurve3 = new OnLineCurve(on3dPoint3, on3dPoint);
				onCurve6 = new OnLineCurve(on3dPoint, on3dPoint5);
			}
			CSUtilGeometry.TrimStartAndEndOfCrv(onCurve3, channelBlend);
			CSUtilGeometry.TrimStartAndEndOfCrv(onCurve6, channelBlend);
			On3dVector brepNormal = new On3dVector();
			On3dVector brepNormal2 = new On3dVector();
			CSUtilGeometry.GetClosestNormalToBrep(surface, point, ref brepNormal);
			CSUtilGeometry.GetClosestNormalToBrep(surface, point2, ref brepNormal2);
			On3dVector on3dVector3 = OnUtil.ON_CrossProduct(tangent, brepNormal);
			On3dVector on3dVector4 = OnUtil.ON_CrossProduct(tangent2, brepNormal2);
			OnLineCurve crv = new OnLineCurve(point + on3dVector3, point);
			OnLineCurve crv2 = new OnLineCurve(point2, point2 - on3dVector4);
			OnCurve item3 = CSUtilCurves.BlendCurve(crv, onCurve3);
			OnCurve item4 = CSUtilCurves.BlendCurve(onCurve6, crv2);
			list.Clear();
			list.Add(item3);
			list.Add(onCurve3);
			output = null;
			RhUtil.RhinoMergeCurves(list.ToArray(), ref output, CSApplicationHelper.Application.Tolerance);
			crvLeft = output[0];
			list.Clear();
			list.Add(onCurve6);
			list.Add(item4);
			output = null;
			RhUtil.RhinoMergeCurves(list.ToArray(), ref output, CSApplicationHelper.Application.Tolerance);
			crvRight = output[0];
			crvBase = CSUtilCurves.BlendCurve(onCurve3, onCurve6);
		}
	}
}
