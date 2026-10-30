using System.Collections.Generic;
using CS.CSObjects.CSBlocks;
using CS.Parametric;
using CS.Parametric.Channels;
using CSBlockUtil;
using CSCompatibility;
using CSDatas;
using CSDatas.Interfaces;
using RMA.OpenNURBS;

namespace CSBlocks.Sweeps;

[CSBlock("Sweep2C4", "{60E95145-810A-4910-A734-99126E1BB440}")]
public sealed class CSSweep2C4 : CSBlock
{
	public CSInputChannel iRail { get; }

	public CSInputChannel iRail2 { get; }

	public CSInputChannel iCurves { get; }

	public CSOutputChannel oBrep { get; }

	public CSSweep2C4(string identifier)
		: base(identifier)
	{
		iRail = new CSInputChannel(GetPropertyName(() => iRail), this, ChannelDataType.Curve, ChannelArrayType.Individual);
		iRail2 = new CSInputChannel(GetPropertyName(() => iRail2), this, ChannelDataType.Curve, ChannelArrayType.Individual);
		iCurves = new CSInputChannel(GetPropertyName(() => iCurves), this, ChannelDataType.Curve, ChannelArrayType.Individual);
		oBrep = new CSOutputChannel(GetPropertyName(() => oBrep), this, ChannelDataType.Brep, ChannelArrayType.Individual);
	}

	public override bool Update()
	{
		CSData data = iRail.Data;
		if (data == null)
		{
			return UpdateMissing("Missing Rail");
		}
		OnNurbsCurve onNurbsCurve = data.GetNurbsCurve().Convert();
		if (onNurbsCurve == null)
		{
			return UpdateMissing("Missing Rail");
		}
		CSData data2 = iRail2.Data;
		if (data2 == null)
		{
			return UpdateMissing("Missing Rail 2");
		}
		OnNurbsCurve onNurbsCurve2 = data2.GetNurbsCurve().Convert();
		if (onNurbsCurve2 == null)
		{
			return UpdateMissing("Missing Rail 2");
		}
		if (!(iCurves.Data is ICSArray iCSArray))
		{
			return UpdateMissing("Missing Curve Array");
		}
		double num = 0.0;
		List<IOnCurve> list = new List<IOnCurve>();
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
		OnBrep onBrep = CSUtilSweep.Sweep2C4(onNurbsCurve, onNurbsCurve2, list.ToArray(), num > 0.5);
		if (onBrep != null)
		{
			oBrep.SetData(new CSOnGeometryData(onBrep.Convert()));
			return base.Update();
		}
		oBrep.SetData(new CSData());
		return UpdateError("Failed Sweep");
	}
}
