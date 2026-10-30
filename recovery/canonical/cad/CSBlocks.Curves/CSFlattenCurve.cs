using CS.CSObjects.CSBlocks;
using CS.Parametric;
using CS.Parametric.Channels;
using CSCompatibility;
using CSDatas;
using RMA.OpenNURBS;

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
			OnNurbsCurve onNurbsCurve = flattenArray.GetArrayItem(i).GetNurbsCurve().Convert();
			if (onNurbsCurve == null)
			{
				continue;
			}
			for (int j = 0; j < onNurbsCurve.CVCount(); j++)
			{
				On4dPoint vertex_value = new On4dPoint();
				onNurbsCurve.GetCV(j, ref vertex_value);
				switch (value)
				{
				case 0:
					vertex_value.x = 0.0;
					break;
				case 1:
					vertex_value.y = 0.0;
					break;
				case 2:
					vertex_value.z = 0.0;
					break;
				}
				onNurbsCurve.SetCV(j, vertex_value);
			}
			cSDataArray.Append(new CSOnGeometryData(onNurbsCurve.Convert()));
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
