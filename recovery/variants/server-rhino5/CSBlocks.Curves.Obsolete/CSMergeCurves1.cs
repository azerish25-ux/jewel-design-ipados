using System;
using System.Collections.Generic;
using CS;
using CS.CSDataUtilities;
using CS.CSObjects.CSBlocks;
using CS.Parametric;
using CS.Parametric.Channels;
using CSDatas;
using Rhino.Geometry;

namespace CSBlocks.Curves.Obsolete;

[Obsolete]
[CSBlock("MergeCurves_V1", "{9122BC33-3D61-421d-9205-F645E564C429}")]
public sealed class CSMergeCurves1 : CSBlock
{
	public readonly CSInputChannel iCurves;

	public readonly CSOutputChannel oArrayOfCurves;

	public CSMergeCurves1(string identifier)
		: base(identifier)
	{
		iCurves = new CSInputChannel(this, ChannelDataType.Curve, ChannelArrayType.Individual);
		oArrayOfCurves = new CSOutputChannel(this, ChannelDataType.Curve, ChannelArrayType.Array);
		SetChannelIdentifiers();
	}

	public override bool Update()
	{
		if (iCurves == null)
		{
			return UpdateMissing("Missing Curve");
		}
		List<Curve> list = new List<Curve>();
		CSData data = iCurves.Data;
		if (data == null)
		{
			return UpdateMissing("Missing curve array");
		}
		CSDataArray flattenArray = CSDataHelper.GetFlattenArray(data);
		for (int i = 0; i < flattenArray.GetArrayCount(); i++)
		{
			if (flattenArray.GetArrayItem(i) == null)
			{
				return UpdateError("Missing curve in array (No Object)");
			}
			CSDataHelper.GetNurbsCurve(flattenArray.GetArrayItem(i), out var value);
			if (value == null)
			{
				return UpdateError("Missing curve in array (No Object)");
			}
			list.Add(value);
		}
		Curve[] array = new Curve[0];
		array = Curve.JoinCurves(list.ToArray(), CSApplicationHelper.Application.Tolerance);
		if (array == null)
		{
			array = list.ToArray();
		}
		CSDataArray cSDataArray = new CSDataArray();
		for (int j = 0; j < array.Length; j++)
		{
			cSDataArray.Append(new CSOnGeometryData(array[j]));
		}
		oArrayOfCurves.SetData(cSDataArray);
		return base.Update();
	}
}
