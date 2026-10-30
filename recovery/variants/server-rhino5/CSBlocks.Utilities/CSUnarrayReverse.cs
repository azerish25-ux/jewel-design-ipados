using CS.CSObjects.CSBlocks;
using CS.Parametric;
using CS.Parametric.Channels;
using CSDatas;

namespace CSBlocks.Utilities;

[CSBlock("UnarrayReverse", "{9A6332CA-205C-4C57-91D3-8DD5222EBD0E}")]
public sealed class CSUnarrayReverse : CSBlockExpandableInput
{
	protected override string ExpandableInputChannelIdentifier => "InObject";

	protected override ChannelDataType ExpandableInputChannelDataType => ChannelDataType.Any;

	protected override ChannelArrayType ExpandableInputChannelArrayType => ChannelArrayType.Any;

	public CSUnarrayReverse(string identifier)
		: base(identifier)
	{
		CreateExpansionInputChannel();
	}

	public override bool BlankOutBeforeUpdate()
	{
		return false;
	}

	public override bool Update()
	{
		base.OutputChannels.Clear();
		CSDataArray data = new CSDataArray();
		int num = 0;
		foreach (CSInputChannel nonExpandableInputChannel in base.NonExpandableInputChannels)
		{
			CSData data2 = nonExpandableInputChannel.Data;
			if (data2.GetType() == typeof(CSDataArray))
			{
				foreach (CSData item in ((CSDataArray)data2).Array)
				{
					new CSOutputChannel($"{base.Identifier}_{num}", this, ChannelDataType.Any, ChannelArrayType.Any).SetData(item);
					num++;
				}
			}
			else
			{
				new CSOutputChannel($"{base.Identifier}_{num}", this, ChannelDataType.Any, ChannelArrayType.Individual).SetData(data2);
				num++;
			}
		}
		base.OutputChannels.Reverse();
		new CSOutputChannel($"{base.Identifier}_{num}", this, ChannelDataType.Any, ChannelArrayType.Any).SetData(data);
		return base.Update();
	}
}
