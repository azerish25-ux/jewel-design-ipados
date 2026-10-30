using System.Collections.Generic;
using CS.CSDataUtilities;
using CS.CSObjects.CSBlocks;
using CS.Parametric;
using CS.Parametric.Channels;
using CSBlockUtil;
using CSCompatibility;
using CSDatas;
using CSDatas.Interfaces;
using RMA.OpenNURBS;
using Rhino.Geometry;

namespace CSBlocks.Sweeps;

[CSBlock("Sweep2C4Param", "{6E0E665C-30EB-4082-859B-717DA65ADB25}")]
public sealed class CSSweep2C4Param : CSBlock
{
	public CSInputChannel iRail { get; }

	public CSInputChannel iRail2 { get; }

	public CSInputChannel iCurves { get; }

	public CSInputChannel iParams { get; }

	public CSInputChannel iParams2 { get; }

	public CSInputChannel iClosed { get; }

	public CSOutputChannel oBrep { get; }

	public CSOutputChannel oRail11 { get; }

	public CSOutputChannel oRail12 { get; }

	public CSOutputChannel oRail21 { get; }

	public CSOutputChannel oRail22 { get; }

	public CSSweep2C4Param(string identifier)
		: base(identifier)
	{
		iRail = new CSInputChannel(GetPropertyName(() => iRail), this, ChannelDataType.Curve, ChannelArrayType.Individual);
		iRail2 = new CSInputChannel(GetPropertyName(() => iRail2), this, ChannelDataType.Curve, ChannelArrayType.Individual);
		iCurves = new CSInputChannel(GetPropertyName(() => iCurves), this, ChannelDataType.Curve, ChannelArrayType.Individual);
		iParams = new CSInputChannel(GetPropertyName(() => iParams), this, ChannelDataType.Any, ChannelArrayType.Any);
		iParams2 = new CSInputChannel(GetPropertyName(() => iParams2), this, ChannelDataType.Any, ChannelArrayType.Any);
		iClosed = new CSInputChannel(GetPropertyName(() => iClosed), this, ChannelDataType.Boolean, ChannelArrayType.Individual);
		oBrep = new CSOutputChannel(GetPropertyName(() => oBrep), this, ChannelDataType.Brep, ChannelArrayType.Individual);
		oRail11 = new CSOutputChannel(GetPropertyName(() => oRail11), this, ChannelDataType.Curve, ChannelArrayType.Individual);
		oRail12 = new CSOutputChannel(GetPropertyName(() => oRail12), this, ChannelDataType.Curve, ChannelArrayType.Individual);
		oRail21 = new CSOutputChannel(GetPropertyName(() => oRail21), this, ChannelDataType.Curve, ChannelArrayType.Individual);
		oRail22 = new CSOutputChannel(GetPropertyName(() => oRail22), this, ChannelDataType.Curve, ChannelArrayType.Individual);
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
				list.Add((IOnCurve)(object)arrayItem.GetCurveSegment(0, 4, fromStart: false).Convert());
				list.Add((IOnCurve)(object)arrayItem.GetCurveSegment(1, 4, fromStart: false).Convert());
				list.Add((IOnCurve)(object)arrayItem.GetCurveSegment(2, 4, fromStart: false).Convert());
				list.Add((IOnCurve)(object)arrayItem.GetCurveSegment(3, 4, fromStart: false).Convert());
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
		OnCurve rail = null;
		OnCurve rail2 = null;
		OnCurve rail3 = null;
		OnCurve rail4 = null;
		OnBrep brep = CSUtilSweep.Sweep2C4((OnCurve)(object)nurbsCurve, (OnCurve)(object)nurbsCurve2, list.ToArray(), list2.ToArray(), list3.ToArray(), value, ref rail, ref rail2, ref rail3, ref rail4);
		oRail11.SetData(new CSOnGeometryData((GeometryBase)(object)rail.Convert()));
		oRail12.SetData(new CSOnGeometryData((GeometryBase)(object)rail2.Convert()));
		oRail21.SetData(new CSOnGeometryData((GeometryBase)(object)rail3.Convert()));
		oRail22.SetData(new CSOnGeometryData((GeometryBase)(object)rail4.Convert()));
		if (brep != null)
		{
			CSUtilGeometry.FixBrepSolidOrientation(ref brep);
			oBrep.SetData(new CSOnGeometryData((GeometryBase)(object)brep.Convert()));
			return base.Update();
		}
		oBrep.SetData(new CSData());
		return UpdateError("Error Sweep");
	}
}
