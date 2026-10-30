using System.Collections.Generic;
using CS;
using CS.CSDataUtilities;
using CS.CSObjects.CSBlocks;
using CS.Parametric;
using CS.Parametric.Channels;
using CSBlockUtil;
using CSDatas;
using Rhino.Geometry;

namespace CSBlocks.Curves;

[CSBlock("JoinCurves", "{73754992-F29A-4403-B7CF-ABD97C63207C}")]
public sealed class CSJoinCurves : CSBlock
{
	public readonly CSInputChannel InCurves;

	public readonly CSOutputChannel OutCurves;

	public CSJoinCurves(string identifier)
		: base(identifier)
	{
		InCurves = new CSInputChannel(this, ChannelDataType.Curve, ChannelArrayType.Array | ChannelArrayType.Individual);
		OutCurves = new CSOutputChannel(this, ChannelDataType.Curve, ChannelArrayType.Array | ChannelArrayType.Individual);
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
				list.Add((Curve)(object)value);
			}
		}
		if (!CSUtilCurves.MergeCurves((IEnumerable<Curve>)list.ToArray(), out Curve[] mergedCrvs, CSApplicationHelper.Application.Tolerance))
		{
			return UpdateError("MergeCurves failed");
		}
		CSDataArray cSDataArray = new CSDataArray();
		Curve[] array = mergedCrvs;
		foreach (Curve geometry in array)
		{
			cSDataArray.Append(new CSOnGeometryData((GeometryBase)(object)geometry));
		}
		switch (cSDataArray.GetArrayCount())
		{
		case 0:
			OutCurves.SetData(null);
			break;
		case 1:
			OutCurves.SetData(cSDataArray.GetArrayItem(0));
			break;
		default:
			OutCurves.SetData(cSDataArray);
			break;
		}
		return base.Update();
	}
}
