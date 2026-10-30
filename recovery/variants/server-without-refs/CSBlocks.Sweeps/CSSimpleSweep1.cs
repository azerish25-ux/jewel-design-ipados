using System.Collections.Generic;
using CS.CSObjects.CSBlocks;
using CS.Parametric;
using CS.Parametric.Channels;
using CSBlockUtil;
using CSCompatibility;
using CSDatas;
using RMA.OpenNURBS;
using Rhino.Geometry;

namespace CSBlocks.Sweeps;

[CSBlock("SimpleSweep1", "{34529E1F-22E9-4b80-A513-9B777B1BE789}")]
public sealed class CSSimpleSweep1 : CSBlock
{
	public CSInputChannel iRail { get; }

	public CSInputChannel iCurves { get; }

	public CSInputChannel iParams { get; }

	public CSInputChannel iClosed { get; }

	public CSOutputChannel oBrep { get; }

	public CSSimpleSweep1(string identifier)
		: base(identifier)
	{
		iRail = new CSInputChannel(GetPropertyName(() => iRail), this, ChannelDataType.Curve, ChannelArrayType.Array | ChannelArrayType.Individual);
		iCurves = new CSInputChannel(GetPropertyName(() => iCurves), this, ChannelDataType.Curve, ChannelArrayType.Array | ChannelArrayType.Individual);
		iParams = new CSInputChannel(GetPropertyName(() => iParams), this, ChannelDataType.Any, ChannelArrayType.Array | ChannelArrayType.Individual);
		iClosed = new CSInputChannel(GetPropertyName(() => iClosed), this, ChannelDataType.Boolean, ChannelArrayType.Individual);
		oBrep = new CSOutputChannel(GetPropertyName(() => oBrep), this, ChannelDataType.Brep, ChannelArrayType.Individual);
	}

	public override bool Update()
	{
		OnNurbsCurve val = iRail.GetFlattenArray().GetArrayItem(0).GetNurbsCurve()
			.Convert();
		if (val == null)
		{
			return UpdateMissing("Missing Rail");
		}
		List<IOnCurve> list = new List<IOnCurve>();
		CSDataArray flattenArray = iCurves.GetFlattenArray();
		for (int i = 0; i < flattenArray.GetArrayCount(); i++)
		{
			OnCurve item = (OnCurve)(object)flattenArray.GetArrayItem(i).GetNurbsCurve().Convert();
			list.Add((IOnCurve)(object)item);
			if (list[i] == null)
			{
				return UpdateMissing("Missing curve in array (No NurbsCurve):" + i);
			}
		}
		List<double> list2 = new List<double>();
		CSDataArray flattenArray2 = iParams.GetFlattenArray();
		for (int j = 0; j < flattenArray2.GetArrayCount(); j++)
		{
			double value = 0.0;
			if (flattenArray2.GetArrayItem(j).GetDouble(ref value))
			{
				list2.Add(value);
				continue;
			}
			return UpdateMissing("Missing param in array (No Param):" + j);
		}
		bool value2 = false;
		iClosed.GetBoolean(ref value2);
		if (!CSUtilSweep.Sweep1((IOnCurve)(object)val, list.ToArray(), list2.ToArray(), out var breps, value2) || breps.Length == 0)
		{
			return UpdateError("Failed Sweep");
		}
		base.OutputChannels.Clear();
		for (int k = 0; k < breps.Length; k++)
		{
			OnBrep val2 = breps[k];
			if (val2 != null)
			{
				new CSOutputChannel($"{base.Identifier}_{k}", this, ChannelDataType.Any, ChannelArrayType.Individual).SetData(new CSOnGeometryData((GeometryBase)(object)val2.Convert()));
			}
		}
		return base.Update();
	}
}
