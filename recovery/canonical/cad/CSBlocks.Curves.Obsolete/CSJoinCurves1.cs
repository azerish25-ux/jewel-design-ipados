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
[CSBlock("JoinCurves_V1", "{E4A97A98-2EF6-4453-BB69-2EE9ABC51023}")]
public sealed class CSJoinCurves1 : CSBlock
{
	public readonly CSInputChannel iCurves;

	public readonly CSOutputChannel oCurve;

	public CSJoinCurves1(string identifier)
		: base(identifier)
	{
		iCurves = new CSInputChannel(this, ChannelDataType.Curve, ChannelArrayType.Array | ChannelArrayType.Individual);
		oCurve = new CSOutputChannel(this, ChannelDataType.Curve, ChannelArrayType.Individual);
		SetChannelIdentifiers();
	}

	public override bool Update()
	{
		CSDataArray flattenArray = iCurves.GetFlattenArray();
		if (flattenArray.GetArrayCount() <= 0)
		{
			return UpdateMissing("Input Curves");
		}
		List<Curve> list = new List<Curve>();
		for (int i = 0; i < flattenArray.GetArrayCount(); i++)
		{
			CSDataHelper.GetNurbsCurve(flattenArray.GetArrayItem(i), out var value);
			if (value != null)
			{
				list.Add(value);
			}
		}
		Curve[] array = Curve.JoinCurves(list.ToArray());
		if (array == null)
		{
			return UpdateError("RhinoMergeCurve Error");
		}
		if (array.Length < 1)
		{
			return UpdateError("RhinoMergeCurve returned null");
		}
		base.OutputChannels.Clear();
		for (int j = 0; j < array.Length; j++)
		{
			Curve curve = array[j];
			if (curve != null)
			{
				NurbsCurve nurbsCurve = curve.ToNurbsCurve();
				if (nurbsCurve != null)
				{
					new CSOutputChannel($"{base.Identifier}_{j}", this, ChannelDataType.Any, ChannelArrayType.Individual).SetData(new CSOnGeometryData(nurbsCurve));
				}
			}
		}
		return base.Update();
	}
}
