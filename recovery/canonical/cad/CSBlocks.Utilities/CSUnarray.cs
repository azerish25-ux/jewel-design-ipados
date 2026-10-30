using CS.CSObjects.CSBlocks;
using CS.Parametric;
using CS.Parametric.Channels;
using CSDatas;

namespace CSBlocks.Utilities;

[CSBlock("Unarray", "{41C78ECB-FF87-45DA-96EC-E3744C2C62A0}")]
public sealed class CSUnarray : CSBlockExpandableInput
{
	protected override string ExpandableInputChannelIdentifier => "InObject";

	protected override ChannelDataType ExpandableInputChannelDataType => ChannelDataType.Any;

	protected override ChannelArrayType ExpandableInputChannelArrayType => ChannelArrayType.Any;

	public CSUnarray(string identifier)
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
		int num = 0;
		foreach (CSInputChannel nonExpandableInputChannel in base.NonExpandableInputChannels)
		{
			CSData data = nonExpandableInputChannel.Data;
			if (data != null && data.GetType() == typeof(CSDataArray))
			{
				foreach (CSData item in ((CSDataArray)data).Array)
				{
					new CSOutputChannel($"{base.Identifier}_{num}", this, ChannelDataType.Any, ChannelArrayType.Any).SetData(item);
					num++;
				}
			}
			else
			{
				new CSOutputChannel($"{base.Identifier}_{num}", this, ChannelDataType.Any, ChannelArrayType.Any).SetData(data);
				num++;
			}
		}
		if (OutputChannelCount > 0)
		{
			return base.Update();
		}
		return UpdateMissing("Missing output");
	}
}
