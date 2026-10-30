using System.Collections.Generic;
using CS;
using CS.CSDataUtilities;
using CS.CSObjects.CSBlocks;
using CS.Parametric;
using CS.Parametric.Channels;
using CSDatas;
using Rhino.Geometry;

namespace CSBlocks.Curves;

[CSBlock("JoinCurvesAll", "{3EA87EFC-98E3-460C-B40E-C7D7829E1AA2}")]
public sealed class CSJoinCurvesAll : CSBlock
{
	public readonly CSInputChannel InCurves;

	public readonly CSOutputChannel OutCurves;

	public CSJoinCurvesAll(string identifier)
		: base(identifier)
	{
		InCurves = new CSInputChannel(this, ChannelDataType.Curve, ChannelArrayType.Array | ChannelArrayType.Individual);
		OutCurves = new CSOutputChannel(this, ChannelDataType.Curve, ChannelArrayType.Individual);
		SetChannelIdentifiers();
	}

	public override bool Update()
	{
		CSDataArray flattenArray = InCurves.GetFlattenArray();
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
				list.Add(value);
			}
		}
		Curve[] array = Curve.JoinCurves(list.ToArray(), CSApplicationHelper.Application.Tolerance);
		if (array.Length == 0)
		{
			return UpdateError("MergeCurves failed");
		}
		Curve curve = array[0];
		if (!curve.IsValid)
		{
			return UpdateError("MergeCurves failed");
		}
		OutCurves.SetData(new CSOnGeometryData(curve));
		return base.Update();
	}
}
