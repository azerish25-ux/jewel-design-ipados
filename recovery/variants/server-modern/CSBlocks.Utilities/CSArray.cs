using CS.CSObjects.CSBlocks;
using CS.Parametric;
using CS.Parametric.Channels;
using CSDatas;

namespace CSBlocks.Utilities;

[CSBlock("Array", "{8D8B262F-F2BB-43c5-9486-FBD563C4759B}")]
public sealed class CSArray : CSBlockExpandableInput
{
	public readonly CSOutputChannel OutArray;

	protected override string ExpandableInputChannelIdentifier => "InObject";

	protected override ChannelDataType ExpandableInputChannelDataType => ChannelDataType.Any;

	protected override ChannelArrayType ExpandableInputChannelArrayType => ChannelArrayType.Any;

	public CSArray(string identifier)
		: base(identifier)
	{
		CreateExpansionInputChannel();
		OutArray = new CSOutputChannel(this, ChannelDataType.Any, ChannelArrayType.Array);
		SetChannelIdentifiers();
	}

	public override bool BlankOutBeforeUpdate()
	{
		return false;
	}

	public override bool Update()
	{
		CSDataArray cSDataArray = new CSDataArray();
		foreach (CSInputChannel nonExpandableInputChannel in base.NonExpandableInputChannels)
		{
			if (nonExpandableInputChannel == null)
			{
				continue;
			}
			if (nonExpandableInputChannel.OutputChannelIndex == -1)
			{
				if (nonExpandableInputChannel.OutputChannelBlock != null)
				{
					for (int i = 0; i < nonExpandableInputChannel.OutputChannelBlock.OutputChannelCount; i++)
					{
						cSDataArray.Append(nonExpandableInputChannel.OutputChannelBlock.GetOutputChannelData(i));
					}
				}
			}
			else
			{
				cSDataArray.Append(nonExpandableInputChannel.Data);
			}
		}
		OutArray.SetData(cSDataArray);
		return base.Update();
	}
}
