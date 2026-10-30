using System.Collections.Generic;
using CS.CSObjects.CSBlocks;
using CS.Parametric;
using CS.Parametric.Channels;
using CSDatas;

namespace CSBlocks.Core;

[CSBlock("SortGeometryBy", "{A062FB87-F1C5-4376-9BCE-798D1B030D64}")]
public sealed class CSSortGeometryBy : CSBlock
{
	public readonly CSInputChannel iArray;

	public readonly CSInputChannel iSearch;

	public CSSortGeometryBy(string identifier)
		: base(identifier)
	{
		iArray = new CSInputChannel(this, ChannelDataType.Any, ChannelArrayType.Array | ChannelArrayType.Individual);
		iSearch = new CSInputChannel(this, ChannelDataType.Boolean, ChannelArrayType.Individual);
		SetChannelIdentifiers();
	}

	public override bool Update()
	{
		if (iArray == null)
		{
			return UpdateMissing("Array missing");
		}
		string key = "CSName";
		bool value = false;
		iSearch.GetBoolean(ref value);
		if (value)
		{
			key = "CSLayer";
		}
		CSDataArray flattenArray = iArray.GetFlattenArray();
		List<string> list = new List<string>();
		for (int i = 0; i < flattenArray.GetArrayCount(); i++)
		{
			CSData arrayItem = flattenArray.GetArrayItem(i);
			string name = string.Empty;
			if (arrayItem.GetUserString(key, ref name) && !list.Exists((string check) => name.CompareTo(check) == 0))
			{
				list.Add(name);
			}
		}
		list.Sort();
		base.OutputChannels.Clear();
		for (int num = 0; num < list.Count; num++)
		{
			CSDataArray cSDataArray = new CSDataArray();
			for (int num2 = 0; num2 < flattenArray.GetArrayCount(); num2++)
			{
				CSData arrayItem2 = flattenArray.GetArrayItem(num2);
				string value2 = string.Empty;
				if (arrayItem2.GetUserString(key, ref value2) && value2.CompareTo(list[num]) == 0)
				{
					cSDataArray.Append(arrayItem2);
				}
			}
			new CSOutputChannel($"{base.Identifier}_{num}", this, ChannelDataType.Any, ChannelArrayType.Any).SetData((cSDataArray.GetArrayCount() == 1) ? cSDataArray.GetArrayItem(0) : cSDataArray);
		}
		return base.Update();
	}
}
