using System.Collections.Generic;
using CS.CSDataUtilities;
using CS.CSObjects.CSBlocks;
using CS.Parametric;
using CS.Parametric.Channels;
using CSDatas;

namespace CSBlocks.Core;

[CSBlock("ReorderArray", "{EFCAC754-44F7-4a46-80CA-01655ADB5316}")]
public sealed class CSReorderArray : CSBlockExpandableInput
{
	public readonly CSInputChannel InArray;

	public readonly CSInputChannel InFirst;

	public readonly CSOutputChannel OutArray;

	protected override string ExpandableInputChannelIdentifier => "InReorderIndex";

	protected override ChannelDataType ExpandableInputChannelDataType => ChannelDataType.Integer;

	protected override ChannelArrayType ExpandableInputChannelArrayType => ChannelArrayType.Individual;

	public CSReorderArray(string identifier)
		: base(identifier)
	{
		InArray = new CSInputChannel(this, ChannelDataType.Any, ChannelArrayType.Any);
		InFirst = new CSInputChannel(this, ChannelDataType.Any, ChannelArrayType.Any);
		CreateExpansionInputChannel();
		OutArray = new CSOutputChannel(this, ChannelDataType.Gem, ChannelArrayType.Array);
		SetChannelIdentifiers();
	}

	public override bool Update()
	{
		if (InArray == null)
		{
			return UpdateMissing("Missing Input");
		}
		CSData data = InArray.Data;
		List<int> list = new List<int>();
		for (int i = 0; i < CSDataHelper.GetArrayCount(data); i++)
		{
			list.Add(i);
		}
		CSDataArray cSDataArray = new CSDataArray();
		for (int j = 1; j < base.NonExpandableInputChannels.Count; j++)
		{
			if (base.NonExpandableInputChannels[j] != null)
			{
				int value = 0;
				if (base.NonExpandableInputChannels[j].GetInteger(ref value) && value >= 0 && value < CSDataHelper.GetArrayCount(data))
				{
					cSDataArray.Append(CSDataHelper.GetArrayItem(data, value));
					list.Remove(value);
				}
			}
		}
		foreach (int item in list)
		{
			cSDataArray.Append(CSDataHelper.GetArrayItem(data, item));
		}
		OutArray.SetData(cSDataArray);
		return base.Update();
	}
}
