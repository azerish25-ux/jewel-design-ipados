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
		//IL_0438: Unknown result type (might be due to invalid IL or missing references)
		//IL_043f: Expected O, but got Unknown
		//IL_048d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0494: Expected O, but got Unknown
		CSData data = InCurveEnd1.Data;
		if (data == null)
		{
			return UpdateMissing("Curve End 1 missing.");
		}
		OnCurve endCurve = (OnCurve)(object)data.GetNurbsCurve().Convert();
		CSData data2 = InCurveEnd2.Data;
		if (data2 == null)
		{
			return UpdateMissing("Curve End 2 missing.");
		}
		OnCurve endCurve2 = (OnCurve)(object)data2.GetNurbsCurve().Convert();
		CSData data3 = InCurveSide1.Data;
		if (data3 == null)
		{
			return UpdateMissing("Curve Side 1 missing.");
		}
		OnCurve val = (OnCurve)(object)data3.GetNurbsCurve().Convert();
		CSData data4 = InCurveSide2.Data;
		if (data4 == null)
		{
			return UpdateMissing("Curve Side 2 missing.");
		}
		OnCurve val2 = (OnCurve)(object)data4.GetNurbsCurve().Convert();
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
		OnBrep val3 = data5.GetBrep().Convert();
		if (val3 == null)
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
		MakeGemCurves(cSGemData, value, value3, val, val2, val3, out var crvLeft, out var crvBase, out var crvRight, out var crvLeftParam, out var crvRightParam);
		MakeGemCurves(cSGemData2, value, value3, val, val2, val3, out var crvLeft2, out var crvBase2, out var crvRight2, out var crvLeftParam2, out var crvRightParam2);
		MakeEndCurves((IOnCurve)(object)endCurve, (IOnCurve)(object)val, (IOnCurve)(object)val2, value3, out var crvLeft3, out var crvBase3, out var crvRight3, out var crvLeftParam3, out var crvRightParam3, workingOnStart: true);
		MakeEndCurves((IOnCurve)(object)endCurve2, (IOnCurve)(object)val, (IOnCurve)(object)val2, value3, out var crvLeft4, out var crvBase4, out var crvRight4, out var crvLeftParam4, out var crvRightParam4, workingOnStart: false);
		List<IOnCurve> list = new List<IOnCurve>();
		list.Add((IOnCurve)(object)crvLeft3);
		list.Add((IOnCurve)(object)crvLeft);
		list.Add((IOnCurve)(object)crvLeft2);
		list.Add((IOnCurve)(object)crvLeft4);
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
		if (!CSUtilSweep.Sweep2((IOnCurve)(object)val, (IOnCurve)(object)val2, list.ToArray(), list2.ToArray(), list3.ToArray(), out var breps, closed, maintainHeight))
		{
			return UpdateError("Sweep2");
		}
		if (breps == null || breps.Length != 1)
		{
			return UpdateError("NoReturnFromSweep2");
		}
		OnBrep val4 = breps[0];
		list.Clear();
		list.Add((IOnCurve)(object)crvRight3);
		list.Add((IOnCurve)(object)crvRight);
		list.Add((IOnCurve)(object)crvRight2);
		list.Add((IOnCurve)(object)crvRight4);
		if (!CSUtilSweep.Sweep2((IOnCurve)(object)val, (IOnCurve)(object)val2, list.ToArray(), list2.ToArray(), list3.ToArray(), out breps, closed, maintainHeight))
		{
			return UpdateError("Sweep2");
		}
		if (breps == null || breps.Length != 1)
		{
			return UpdateError("NoReturnFromSweep2");
		}
		OnBrep val5 = breps[0];
		OnCurve curveFromBrepEdges = CSUtilBrepCrvs.GetCurveFromBrepEdges((IOnBrep)(object)val4, crvLeft3.PointAtEnd(), crvLeft4.PointAtEnd(), crvLeft.PointAtEnd());
		OnCurve curveFromBrepEdges2 = CSUtilBrepCrvs.GetCurveFromBrepEdges((IOnBrep)(object)val5, crvRight3.PointAtStart(), crvRight4.PointAtStart(), crvRight.PointAtStart());
		double num = 0.0;
		curveFromBrepEdges.GetLength(ref num);
		double num2 = 0.0;
		curveFromBrepEdges2.GetLength(ref num2);
		double num3 = 0.0;
		curveFromBrepEdges.GetNormalizedArcLengthPoint(value4 / num, ref num3);
		double num4 = 0.0;
		curveFromBrepEdges2.GetNormalizedArcLengthPoint(value4 / num2, ref num4);
		On3dPoint obj = curveFromBrepEdges.PointAt(num3);
		On3dPoint val6 = curveFromBrepEdges2.PointAt(num4);
		OnCurve item = (OnCurve)new OnLineCurve((IOn3dPoint)(object)obj, (IOn3dPoint)(object)val6);
		double num5 = 0.0;
		curveFromBrepEdges.GetNormalizedArcLengthPoint((num - value4) / num, ref num5);
		double num6 = 0.0;
		curveFromBrepEdges2.GetNormalizedArcLengthPoint((num2 - value4) / num2, ref num6);
		On3dPoint obj2 = curveFromBrepEdges.PointAt(num5);
		On3dPoint val7 = curveFromBrepEdges2.PointAt(num6);
		OnCurve item2 = (OnCurve)new OnLineCurve((IOn3dPoint)(object)obj2, (IOn3dPoint)(object)val7);
		list2.Clear();
		double item3 = 0.0;
		curveFromBrepEdges.GetClosestPoint((IOn3dPoint)(object)crvBase3.PointAtStart(), ref item3, CSApplicationHelper.Application.Tolerance);
		list2.Add(item3);
		if (value4 > 0.01)
		{
			list2.Add(num3);
		}
		curveFromBrepEdges.GetClosestPoint((IOn3dPoint)(object)crvBase.PointAtStart(), ref item3, CSApplicationHelper.Application.Tolerance);
		list2.Add(item3);
		curveFromBrepEdges.GetClosestPoint((IOn3dPoint)(object)crvBase2.PointAtStart(), ref item3, CSApplicationHelper.Application.Tolerance);
		list2.Add(item3);
		if (value4 > 0.01)
		{
			list2.Add(num5);
		}
		curveFromBrepEdges.GetClosestPoint((IOn3dPoint)(object)crvBase4.PointAtStart(), ref item3, CSApplicationHelper.Application.Tolerance);
		list2.Add(item3);
		list3.Clear();
		curveFromBrepEdges2.GetClosestPoint((IOn3dPoint)(object)crvBase3.PointAtEnd(), ref item3, CSApplicationHelper.Application.Tolerance);
		list3.Add(item3);
		if (value4 > 0.01)
		{
			list3.Add(num4);
		}
		curveFromBrepEdges2.GetClosestPoint((IOn3dPoint)(object)crvBase.PointAtEnd(), ref item3, CSApplicationHelper.Application.Tolerance);
		list3.Add(item3);
		curveFromBrepEdges2.GetClosestPoint((IOn3dPoint)(object)crvBase2.PointAtEnd(), ref item3, CSApplicationHelper.Application.Tolerance);
		list3.Add(item3);
		if (value4 > 0.01)
		{
			list3.Add(num6);
		}
		curveFromBrepEdges2.GetClosestPoint((IOn3dPoint)(object)crvBase4.PointAtEnd(), ref item3, CSApplicationHelper.Application.Tolerance);
		list3.Add(item3);
		list = new List<IOnCurve> { (IOnCurve)(object)crvBase3 };
		if (value4 > 0.01)
		{
			list.Add((IOnCurve)(object)item);
		}
		list.Add((IOnCurve)(object)crvBase);
		list.Add((IOnCurve)(object)crvBase2);
		if (value4 > 0.01)
		{
			list.Add((IOnCurve)(object)item2);
		}
		list.Add((IOnCurve)(object)crvBase4);
		if (!CSUtilSweep.Sweep2((IOnCurve)(object)curveFromBrepEdges, (IOnCurve)(object)curveFromBrepEdges2, list.ToArray(), list2.ToArray(), list3.ToArray(), out breps, closed, maintainHeight))
		{
			return UpdateError("Sweep2");
		}
		if (breps == null || breps.Length != 1)
		{
			return UpdateError("NoReturnFromSweep2");
		}
		OnBrep val8 = breps[0];
		CSData data6 = InSplitSurface.Data;
		bool value5 = true;
		InJoin.GetBoolean(ref value5);
		if (value5)
		{
			List<OnBrep> list4 = new List<OnBrep> { val4, val8, val5 };
			if (data6 != null)
			{
				OnBrep item4 = data6.GetBrep().Convert();
				list4.Add(item4);
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
			CSUtilRhino.JoinBreps((OnBrep[])(object)new OnBrep[3] { val4, val8, val5 }, out var outputBreps2, CSApplicationHelper.Application.Tolerance);
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
		//IL_0096: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a0: Expected O, but got Unknown
		//IL_00b0: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ba: Expected O, but got Unknown
		//IL_00d6: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e0: Expected O, but got Unknown
		crvBase = null;
		crvLeft = null;
		crvRight = null;
		crvLeftParam = 0.0;
		crvRightParam = 0.0;
		if (endCurve != null && leftCurve != null && rightcurve != null)
		{
			double num = 0.0;
			endCurve.GetLength(ref num);
			double num2 = channelBlend / num;
			double num3 = 1.0 - num2;
			double num4 = 0.0;
			endCurve.GetNormalizedArcLengthPoint(num2, ref num4);
			double num5 = 0.0;
			endCurve.GetNormalizedArcLengthPoint(num3, ref num5);
			crvLeft = endCurve.DuplicateCurve();
			crvLeft.Trim((IOnInterval)new OnInterval(crvLeft.Domain().Min(), num4));
			crvBase = endCurve.DuplicateCurve();
			crvBase.Trim((IOnInterval)new OnInterval(num4, num5));
			crvRight = endCurve.DuplicateCurve();
			crvRight.Trim((IOnInterval)new OnInterval(num5, crvRight.Domain().Max()));
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
		//IL_002f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0035: Expected O, but got Unknown
		//IL_0077: Unknown result type (might be due to invalid IL or missing references)
		//IL_007e: Expected O, but got Unknown
		//IL_007e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0085: Expected O, but got Unknown
		//IL_0085: Unknown result type (might be due to invalid IL or missing references)
		//IL_008c: Expected O, but got Unknown
		//IL_008c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0093: Expected O, but got Unknown
		//IL_023c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0243: Expected O, but got Unknown
		//IL_0246: Unknown result type (might be due to invalid IL or missing references)
		//IL_024d: Expected O, but got Unknown
		//IL_011a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0121: Expected O, but got Unknown
		//IL_0124: Unknown result type (might be due to invalid IL or missing references)
		//IL_012b: Expected O, but got Unknown
		//IL_01a9: Unknown result type (might be due to invalid IL or missing references)
		//IL_01b0: Expected O, but got Unknown
		//IL_01b3: Unknown result type (might be due to invalid IL or missing references)
		//IL_01ba: Expected O, but got Unknown
		//IL_025d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0264: Expected O, but got Unknown
		//IL_0264: Unknown result type (might be due to invalid IL or missing references)
		//IL_026b: Expected O, but got Unknown
		//IL_02a4: Unknown result type (might be due to invalid IL or missing references)
		//IL_02b4: Unknown result type (might be due to invalid IL or missing references)
		//IL_02bb: Expected O, but got Unknown
		//IL_02e0: Expected O, but got Unknown
		IOn3dPoint val = (IOn3dPoint)(object)gem.YMaxPoint.Convert();
		IOn3dPoint val2 = (IOn3dPoint)(object)gem.YMinPoint.Convert();
		IOn3dPoint val3 = (IOn3dPoint)(object)gem.ZMinPoint.Convert();
		On3dVector val4 = new On3dVector((IOn3dVector)(object)gem.Down.Convert());
		crvLeftParam = 0.0;
		crvRightParam = 0.0;
		crvBase = null;
		crvLeft = null;
		crvRight = null;
		if (leftCurve != null && rightCurve != null)
		{
			leftCurve.GetClosestPoint(val2, ref crvLeftParam);
			rightCurve.GetClosestPoint(val, ref crvRightParam);
			On3dPoint val5 = new On3dPoint();
			On3dPoint val6 = new On3dPoint();
			On3dVector val7 = new On3dVector();
			On3dVector val8 = new On3dVector();
			leftCurve.EvTangent(crvLeftParam, ref val5, ref val7);
			rightCurve.EvTangent(crvRightParam, ref val6, ref val8);
			On3dVector val9 = val6 - (IOn3dPoint)(object)val5;
			val9.Unitize();
			On3dPoint val10 = val5 + (IOn3dVector)(object)(val9 * channelBlend);
			On3dPoint val11 = val10 + (IOn3dVector)(object)(val4 * channelWallHeight);
			On3dPoint val12 = val6 - (IOn3dVector)(object)(val9 * channelBlend);
			On3dPoint val13 = val12 + (IOn3dVector)(object)(val4 * channelWallHeight);
			List<IOnCurve> list = new List<IOnCurve>();
			OnCurve[] array = null;
			OnCurve val16;
			OnCurve val19;
			if (channelWallHeight > channelBlend)
			{
				OnCurve val14 = (OnCurve)new OnLineCurve((IOn3dPoint)(object)val10, (IOn3dPoint)(object)val11);
				OnCurve val15 = (OnCurve)new OnLineCurve((IOn3dPoint)(object)val11, val3);
				CSUtilGeometry.TrimEndOfCrv(val14, channelBlend);
				CSUtilGeometry.TrimStartOfCrv(val15, channelBlend);
				OnCurve item = (OnCurve)(object)CSUtilCurves.BlendCurve((IOnCurve)(object)val14, (IOnCurve)(object)val15);
				list.Clear();
				list.Add((IOnCurve)(object)val14);
				list.Add((IOnCurve)(object)item);
				list.Add((IOnCurve)(object)val15);
				RhUtil.RhinoMergeCurves(list.ToArray(), ref array, CSApplicationHelper.Application.Tolerance);
				val16 = array[0];
				OnCurve val17 = (OnCurve)new OnLineCurve((IOn3dPoint)(object)val13, (IOn3dPoint)(object)val12);
				OnCurve val18 = (OnCurve)new OnLineCurve(val3, (IOn3dPoint)(object)val13);
				CSUtilGeometry.TrimEndOfCrv(val18, channelBlend);
				CSUtilGeometry.TrimStartOfCrv(val17, channelBlend);
				OnCurve item2 = (OnCurve)(object)CSUtilCurves.BlendCurve((IOnCurve)(object)val18, (IOnCurve)(object)val17);
				list.Clear();
				list.Add((IOnCurve)(object)val18);
				list.Add((IOnCurve)(object)item2);
				list.Add((IOnCurve)(object)val17);
				array = null;
				RhUtil.RhinoMergeCurves(list.ToArray(), ref array, CSApplicationHelper.Application.Tolerance);
				val19 = array[0];
			}
			else
			{
				val16 = (OnCurve)new OnLineCurve((IOn3dPoint)(object)val11, val3);
				val19 = (OnCurve)new OnLineCurve(val3, (IOn3dPoint)(object)val13);
			}
			CSUtilGeometry.TrimStartAndEndOfCrv(val16, channelBlend);
			CSUtilGeometry.TrimStartAndEndOfCrv(val19, channelBlend);
			On3dVector brepNormal = new On3dVector();
			On3dVector brepNormal2 = new On3dVector();
			CSUtilGeometry.GetClosestNormalToBrep((IOnBrep)(object)surface, (IOn3dPoint)(object)val5, ref brepNormal);
			CSUtilGeometry.GetClosestNormalToBrep((IOnBrep)(object)surface, (IOn3dPoint)(object)val6, ref brepNormal2);
			On3dVector val20 = OnUtil.ON_CrossProduct((IOn3dVector)(object)val7, (IOn3dVector)(object)brepNormal);
			On3dVector val21 = OnUtil.ON_CrossProduct((IOn3dVector)(object)val8, (IOn3dVector)(object)brepNormal2);
			OnLineCurve val22 = new OnLineCurve((IOn3dPoint)(object)(val5 + (IOn3dVector)(object)val20), (IOn3dPoint)(object)val5);
			OnLineCurve crv = new OnLineCurve((IOn3dPoint)(object)val6, (IOn3dPoint)(object)(val6 - (IOn3dVector)(object)val21));
			OnCurve item3 = (OnCurve)(object)CSUtilCurves.BlendCurve((IOnCurve)val22, (IOnCurve)(object)val16);
			OnCurve item4 = (OnCurve)(object)CSUtilCurves.BlendCurve((IOnCurve)(object)val19, (IOnCurve)(object)crv);
			list.Clear();
			list.Add((IOnCurve)(object)item3);
			list.Add((IOnCurve)(object)val16);
			array = null;
			RhUtil.RhinoMergeCurves(list.ToArray(), ref array, CSApplicationHelper.Application.Tolerance);
			crvLeft = array[0];
			list.Clear();
			list.Add((IOnCurve)(object)val19);
			list.Add((IOnCurve)(object)item4);
			array = null;
			RhUtil.RhinoMergeCurves(list.ToArray(), ref array, CSApplicationHelper.Application.Tolerance);
			crvRight = array[0];
			crvBase = (OnCurve)(object)CSUtilCurves.BlendCurve((IOnCurve)(object)val16, (IOnCurve)(object)val19);
		}
	}
}
