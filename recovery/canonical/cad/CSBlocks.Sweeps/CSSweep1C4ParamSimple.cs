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

[CSBlock("Sweep1C4ParamSimple", "{7BD36869-184E-4f23-B909-E25A9C4A5762}")]
public sealed class CSSweep1C4ParamSimple : CSBlock
{
	public CSInputChannel iRail { get; }

	public CSInputChannel iCurves { get; }

	public CSInputChannel iParams { get; }

	public CSInputChannel iClosed { get; }

	public CSOutputChannel oBrep { get; }

	public CSSweep1C4ParamSimple(string identifier)
		: base(identifier)
	{
		iRail = new CSInputChannel(GetPropertyName(() => iRail), this, ChannelDataType.Curve, ChannelArrayType.Array | ChannelArrayType.Individual);
		iCurves = new CSInputChannel(GetPropertyName(() => iCurves), this, ChannelDataType.Curve, ChannelArrayType.Individual);
		iParams = new CSInputChannel(GetPropertyName(() => iParams), this, ChannelDataType.Any, ChannelArrayType.Any);
		iClosed = new CSInputChannel(GetPropertyName(() => iClosed), this, ChannelDataType.Boolean, ChannelArrayType.Individual);
		oBrep = new CSOutputChannel(GetPropertyName(() => oBrep), this, ChannelDataType.Brep, ChannelArrayType.Individual);
	}

	public override bool Update()
	{
		OnNurbsCurve onNurbsCurve = iRail.GetFlattenArray().GetArrayItem(0).GetNurbsCurve()
			.Convert();
		if (onNurbsCurve == null)
		{
			return UpdateMissing("Rail");
		}
		if (!(iCurves.Data is ICSArray iCSArray))
		{
			return UpdateMissing("Curve Array");
		}
		CSData data = iParams.Data;
		bool value = false;
		iClosed.GetBoolean(ref value);
		List<IOnCurve> list = new List<IOnCurve>();
		for (int i = 0; i < iCSArray.GetArrayCount(); i++)
		{
			if (iCSArray.GetArrayItem(i) == null)
			{
				return UpdateMissing("Curve Array (No Object)");
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
		for (int j = 0; j < CSDataHelper.GetArrayCount(data); j++)
		{
			double value2 = 0.0;
			if (CSDataHelper.GetArrayItem(data, j).GetDouble(ref value2))
			{
				list2.Add(value2);
			}
		}
		OnBrep onBrep = CSUtilRhino.JoinAllBreps(CSUtilSweep.Sweep1C4Simple(onNurbsCurve, list.ToArray(), list2.ToArray(), value).ToArray(), CSApplicationHelper.Application.Tolerance * 2.0);
		if (onBrep != null)
		{
			oBrep.SetData(new CSOnGeometryData(onBrep.Convert()));
			return base.Update();
		}
		return UpdateError("Failed Sweep");
	}
}
