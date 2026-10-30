using CS.CSObjects.CSBlocks;
using CS.Parametric;
using CS.Parametric.Channels;
using CSCompatibility;
using CSDatas;
using RMA.OpenNURBS;
using Rhino.Geometry;

namespace CSBlocks.Curves;

[CSBlock("FlattenCurve", "{7657D565-BC4F-488C-A3A8-994669ADF8BE}")]
public sealed class CSFlattenCurve : CSBlock
{
	public CSInputChannel InCurve { get; }

	public CSInputChannel InWhichAxis { get; }

	public CSOutputChannel OutCurve { get; }

	public CSFlattenCurve(string identifier)
		: base(identifier)
	{
		InCurve = new CSInputChannel(GetPropertyName(() => InCurve), this, ChannelDataType.Curve, ChannelArrayType.Individual);
		InWhichAxis = new CSInputChannel(GetPropertyName(() => InWhichAxis), this, ChannelDataType.Double, ChannelArrayType.Individual);
		OutCurve = new CSOutputChannel(GetPropertyName(() => OutCurve), this, ChannelDataType.Curve, ChannelArrayType.Array | ChannelArrayType.Individual);
	}

	public override bool Update()
	{
		//IL_0057: Unknown result type (might be due to invalid IL or missing references)
		//IL_005e: Expected O, but got Unknown
		CSDataArray flattenArray = InCurve.GetFlattenArray();
		if (flattenArray == null)
		{
			return UpdateMissing("Curves");
		}
		int value = 0;
		InWhichAxis.GetInteger(ref value);
		CSDataArray cSDataArray = new CSDataArray();
		for (int i = 0; i < flattenArray.GetArrayCount(); i++)
		{
			OnNurbsCurve val = flattenArray.GetArrayItem(i).GetNurbsCurve().Convert();
			if (val == null)
			{
				continue;
			}
			for (int j = 0; j < val.CVCount(); j++)
			{
				On4dPoint val2 = new On4dPoint();
				val.GetCV(j, ref val2);
				switch (value)
				{
				case 0:
					val2.x = 0.0;
					break;
				case 1:
					val2.y = 0.0;
					break;
				case 2:
					val2.z = 0.0;
					break;
				}
				val.SetCV(j, (IOn4dPoint)(object)val2);
			}
			cSDataArray.Append(new CSOnGeometryData((GeometryBase)(object)val.Convert()));
		}
		switch (cSDataArray.Array.Count)
		{
		case 0:
			OutCurve.SetData(new CSData());
			break;
		case 1:
			OutCurve.SetData(cSDataArray.Array[0]);
			break;
		default:
			OutCurve.SetData(cSDataArray);
			break;
		}
		return base.Update();
	}
}
