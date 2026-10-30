using System;
using CS.CSObjects.CSBlocks;
using CS.Parametric;
using CS.Parametric.Channels;
using CSDatas;

namespace CSBlocks.Utilities.Obsolete;

[Obsolete]
[CSBlock("Unarray_V1", "{E147E20F-A225-4005-AB5E-2F37D149E81C}")]
public sealed class CSUnarray1 : CSBlockExpandableInput
{
	protected override string ExpandableInputChannelIdentifier => "InObject";

	protected override ChannelDataType ExpandableInputChannelDataType => ChannelDataType.Any;

	protected override ChannelArrayType ExpandableInputChannelArrayType => ChannelArrayType.Any;

	public CSUnarray1(string identifier)
		: base(identifier)
	{
		CreateExpansionInputChannel();
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
		new CSOutputChannel($"{base.Identifier}_{num}", this, ChannelDataType.Any, ChannelArrayType.Any).SetData(data);
		return base.Update();
	}
}
