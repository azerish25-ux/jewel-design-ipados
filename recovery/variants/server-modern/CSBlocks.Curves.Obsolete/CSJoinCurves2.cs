using System;
using System.Collections.Generic;
using CS.CSDataUtilities;
using CS.CSObjects.CSBlocks;
using CS.Parametric;
using CS.Parametric.Channels;
using CSDatas;
using Rhino.Geometry;

namespace CSBlocks.Curves.Obsolete;

[Obsolete]
[CSBlock("JoinCurves_V2", "{9DF57963-4508-4a41-A9F0-B13F1F8BDD18}")]
public sealed class CSJoinCurves2 : CSBlock
{
	public readonly CSInputChannel iCurves;

	public readonly CSOutputChannel oCurves;

	public CSJoinCurves2(string identifier)
		: base(identifier)
	{
		iCurves = new CSInputChannel(this, ChannelDataType.Curve, ChannelArrayType.Array | ChannelArrayType.Individual);
		oCurves = new CSOutputChannel(this, ChannelDataType.Curve, ChannelArrayType.Array | ChannelArrayType.Individual);
		SetChannelIdentifiers();
	}

	public override bool Update()
	{
		CSDataArray flattenArray = iCurves.GetFlattenArray();
		if (flattenArray.GetArrayCount() <= 0)
		{
			return UpdateMissing("Curves");
		}
		List<Curve> list = new List<Curve>();
		for (int i = 0; i < flattenArray.GetArrayCount(); i++)
		{
			CSDataHelper.GetNurbsCurve(flattenArray.GetArrayItem(i), out var value);
			if (value != null)
			{
				list.Add((Curve)(object)value);
			}
		}
		Curve[] array = Curve.JoinCurves((IEnumerable<Curve>)list.ToArray());
		if (array == null)
		{
			return UpdateError("Error RhinoMergeCurve Failed");
		}
		if (array.Length < 1)
		{
			return UpdateError("Error with Merge Curves");
		}
		CSDataArray cSDataArray = new CSDataArray();
		Curve[] array2 = array;
		foreach (Curve geometry in array2)
		{
			cSDataArray.Append(new CSOnGeometryData((GeometryBase)(object)geometry));
		}
		oCurves.SetData((cSDataArray.GetArrayCount() == 1) ? cSDataArray.GetArrayItem(0) : cSDataArray);
		return base.Update();
	}
}
