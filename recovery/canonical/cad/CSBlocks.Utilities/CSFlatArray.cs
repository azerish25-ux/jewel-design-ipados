using CS.CSDataUtilities;
using CS.CSObjects.CSBlocks;
using CS.Parametric;
using CS.Parametric.Channels;
using CSDatas;

namespace CSBlocks.Utilities;

[CSBlock("FlatArray", "{24A007AF-8AC5-4341-BFCD-02240BA997FD}")]
public sealed class CSFlatArray : CSBlockExpandableInput
{
	public readonly CSOutputChannel OutArray;

	public readonly CSOutputChannel OutFirst;

	protected override string ExpandableInputChannelIdentifier => "InObject";

	protected override ChannelDataType ExpandableInputChannelDataType => ChannelDataType.Any;

	protected override ChannelArrayType ExpandableInputChannelArrayType => ChannelArrayType.Any;

	public CSFlatArray(string identifier)
		: base(identifier)
	{
		CreateExpansionInputChannel();
		OutArray = new CSOutputChannel(this, ChannelDataType.Any, ChannelArrayType.Array);
		OutFirst = new CSOutputChannel(this, ChannelDataType.Any, ChannelArrayType.Any);
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
				if (nonExpandableInputChannel.OutputChannelBlock == null)
				{
					continue;
				}
				for (int i = 0; i < nonExpandableInputChannel.OutputChannelBlock.OutputChannelCount; i++)
				{
					CSDataArray flattenArray = CSDataHelper.GetFlattenArray(nonExpandableInputChannel.OutputChannelBlock.GetOutputChannelData(i));
					for (int j = 0; j < flattenArray.GetArrayCount(); j++)
					{
						cSDataArray.Append(flattenArray.GetArrayItem(j));
					}
				}
			}
			else
			{
				CSDataArray flattenArray2 = nonExpandableInputChannel.GetFlattenArray();
				for (int k = 0; k < flattenArray2.GetArrayCount(); k++)
				{
					cSDataArray.Append(flattenArray2.GetArrayItem(k));
				}
			}
		}
		OutArray.SetData(cSDataArray);
		if (cSDataArray.GetArrayCount() > 0)
		{
			OutFirst.SetData(cSDataArray.GetArrayItem(0));
		}
		return base.Update();
	}
}
