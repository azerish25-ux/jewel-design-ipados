using CS.CSObjects.CSBlocks;
using CS.Parametric;
using CS.Parametric.Channels;
using CSDatas;
using Rhino.Geometry;

namespace CSBlocks.Core;

[CSBlock("FindGeometry", "{1BF21C9F-0C7F-4407-AE37-FC23C4103C3F}")]
public sealed class CSFindGeometry : CSBlock
{
	public CSInputChannel iArray { get; }

	public CSInputChannel iName { get; }

	public CSInputChannel iLayerName { get; }

	public CSOutputChannel oArray { get; }

	public CSFindGeometry(string identifier)
		: base(identifier)
	{
		iArray = new CSInputChannel(GetPropertyName(() => iArray), this, ChannelDataType.Any, ChannelArrayType.Array | ChannelArrayType.Individual);
		iName = new CSInputChannel(GetPropertyName(() => iName), this, ChannelDataType.String, ChannelArrayType.Individual);
		iLayerName = new CSInputChannel(GetPropertyName(() => iLayerName), this, ChannelDataType.String, ChannelArrayType.Individual);
		oArray = new CSOutputChannel(GetPropertyName(() => oArray), this, ChannelDataType.Any, ChannelArrayType.Array | ChannelArrayType.Individual);
	}

	public override bool Update()
	{
		if (iArray == null)
		{
			return UpdateMissing("Array missing");
		}
		CSDataArray flattenArray = iArray.GetFlattenArray();
		string output = string.Empty;
		string output2 = string.Empty;
		if (iName != null)
		{
			iName.GetString(ref output);
		}
		if (iLayerName != null)
		{
			iLayerName.GetString(ref output2);
		}
		CSDataArray cSDataArray = new CSDataArray();
		for (int i = 0; i < flattenArray.GetArrayCount(); i++)
		{
			CSData arrayItem = flattenArray.GetArrayItem(i);
			GeometryBase geometry = arrayItem.GetGeometry();
			if (geometry == null)
			{
				continue;
			}
			bool flag = false;
			string userString = geometry.GetUserString("CSName");
			if (!string.IsNullOrWhiteSpace(userString) && userString.CompareTo(output) == 0)
			{
				flag = true;
			}
			if (!flag)
			{
				userString = geometry.GetUserString("CSLayer");
				if (!string.IsNullOrWhiteSpace(userString) && userString.CompareTo(output2) == 0)
				{
					flag = true;
				}
			}
			if (flag)
			{
				cSDataArray.Append(arrayItem);
			}
		}
		if (cSDataArray.GetArrayCount() == 1)
		{
			oArray.SetData(cSDataArray.GetArrayItem(0));
		}
		else
		{
			oArray.SetData(cSDataArray);
		}
		return base.Update();
	}
}
