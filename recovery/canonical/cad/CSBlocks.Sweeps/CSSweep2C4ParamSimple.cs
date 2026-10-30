using System.Collections.Generic;
using CS;
using CS.CSDataUtilities;
using CS.CSObjects.CSBlocks;
using CS.Parametric;
using CS.Parametric.Channels;
using CSBlockUtil;
using CSCompatibility;
using CSDatas;
using CSDatas.Interfaces;
using RMA.OpenNURBS;

namespace CSBlocks.Sweeps;

[CSBlock("Sweep2C4ParamSimple", "{88A5ACC8-9FEF-40f5-9C68-4AE609A29D5D}")]
public sealed class CSSweep2C4ParamSimple : CSBlock
{
	public CSInputChannel iRail { get; }

	public CSInputChannel iRail2 { get; }

	public CSInputChannel iCurves { get; }

	public CSInputChannel iParams { get; }

	public CSInputChannel iParams2 { get; }

	public CSInputChannel iClosed { get; }

	public CSOutputChannel oBrep { get; }

	public CSSweep2C4ParamSimple(string identifier)
		: base(identifier)
	{
		iRail = new CSInputChannel(GetPropertyName(() => iRail), this, ChannelDataType.Curve, ChannelArrayType.Individual);
		iRail2 = new CSInputChannel(GetPropertyName(() => iRail2), this, ChannelDataType.Curve, ChannelArrayType.Individual);
		iCurves = new CSInputChannel(GetPropertyName(() => iCurves), this, ChannelDataType.Curve, ChannelArrayType.Individual);
		iParams = new CSInputChannel(GetPropertyName(() => iParams), this, ChannelDataType.Any, ChannelArrayType.Any);
		iParams2 = new CSInputChannel(GetPropertyName(() => iParams2), this, ChannelDataType.Any, ChannelArrayType.Any);
		iClosed = new CSInputChannel(GetPropertyName(() => iClosed), this, ChannelDataType.Boolean, ChannelArrayType.Individual);
		oBrep = new CSOutputChannel(GetPropertyName(() => oBrep), this, ChannelDataType.Brep, ChannelArrayType.Individual);
	}

	public override bool Update()
	{
		OnNurbsCurve nurbsCurve = iRail.GetNurbsCurve();
		if (nurbsCurve == null)
		{
			return UpdateMissing("Missing Rail 1");
		}
		OnNurbsCurve nurbsCurve2 = iRail2.GetNurbsCurve();
		if (nurbsCurve2 == null)
		{
			return UpdateMissing("Missing Rail 2");
		}
		if (!(iCurves.Data is ICSArray iCSArray))
		{
			return UpdateMissing("Missing Curve Array");
		}
		List<IOnCurve> list = new List<IOnCurve>();
		CSData data = iParams.Data;
		CSData data2 = iParams2.Data;
		bool value = false;
		iClosed.GetBoolean(ref value);
		for (int i = 0; i < iCSArray.GetArrayCount(); i++)
		{
			if (iCSArray.GetArrayItem(i) == null)
			{
				return UpdateMissing("Missing Curve Array (No Object)");
			}
			CSData arrayItem = iCSArray.GetArrayItem(i);
			if (arrayItem != null)
			{
				list.Add(arrayItem.GetCurveSegment(0, 4, fromStart: false).Convert());
				list.Add(arrayItem.GetCurveSegment(1, 4, fromStart: false).Convert());
				list.Add(arrayItem.GetCurveSegment(2, 4, fromStart: false).Convert());
				list.Add(arrayItem.GetCurveSegment(3, 4, fromStart: false).Convert());
			}
		}
		List<double> list2 = new List<double>();
		List<double> list3 = new List<double>();
		for (int j = 0; j < CSDataHelper.GetArrayCount(data); j++)
		{
			double value2 = 0.0;
			double value3 = 0.0;
			if (CSDataHelper.GetArrayItem(data, j).GetDouble(ref value2))
			{
				list2.Add(value2);
			}
			if (CSDataHelper.GetArrayItem(data2, j).GetDouble(ref value3))
			{
				list3.Add(value3);
			}
		}
		OnBrep onBrep = CSUtilRhino.JoinAllBreps(CSUtilSweep.Sweep2C4Simple(nurbsCurve, nurbsCurve2, list.ToArray(), list2.ToArray(), list3.ToArray(), value).ToArray(), CSApplicationHelper.Application.Tolerance * 2.0);
		if (onBrep == null)
		{
			oBrep.SetData(new CSData());
			return UpdateError("Failed Sweep");
		}
		oBrep.SetData(new CSOnGeometryData(onBrep.Convert()));
		return base.Update();
	}
}
