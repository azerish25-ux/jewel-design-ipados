using CS.CSDataUtilities;
using CS.CSObjects.CSBlocks;
using CS.Parametric;
using CS.Parametric.Channels;
using CSDatas;
using Rhino.Geometry;

namespace CSBlocks.Curves;

[CSBlock("RebuildCurves", "{86A0EC57-6D7B-4817-86BE-0920F246802A}")]
public sealed class CSRebuildCurves : CSBlock
{
	public readonly CSInputChannel InCurves;

	public readonly CSInputChannel InDegree;

	public readonly CSInputChannel InPointCount;

	public readonly CSOutputChannel OutCurves;

	public CSRebuildCurves(string identifier)
		: base(identifier)
	{
		InCurves = new CSInputChannel(this, ChannelDataType.Curve, ChannelArrayType.Array | ChannelArrayType.Individual);
		InDegree = new CSInputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual);
		InPointCount = new CSInputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual);
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
		int value = 3;
		InDegree.GetInteger(ref value);
		int value2 = 25;
		InPointCount.GetInteger(ref value2);
		if (value2 <= value)
		{
			value2 = value + 1;
		}
		CSDataArray cSDataArray = new CSDataArray();
		for (int i = 0; i < flattenArray.GetArrayCount(); i++)
		{
			CSData arrayItem = flattenArray.GetArrayItem(i);
			CSDataHelper.GetNurbsCurve(arrayItem, out var value3);
			if (value3 != null)
			{
				CSDataHelper.GetTransformNoArray(arrayItem, out var value4);
				if (!value4.IsValid)
				{
					value4 = Transform.Identity;
				}
				value4.TryGetInverse(out var inverseTransform);
				value3.Transform(inverseTransform);
				NurbsCurve nurbsCurve = value3.Rebuild(value2, value, preserveTangents: false);
				nurbsCurve.Transform(value4);
				cSDataArray.Append(new CSOnGeometryData(nurbsCurve, value4, applyTransform: false));
			}
		}
		OutCurves.SetData((cSDataArray.GetArrayCount() == 1) ? cSDataArray.GetArrayItem(0) : cSDataArray);
		return base.Update();
	}
}
