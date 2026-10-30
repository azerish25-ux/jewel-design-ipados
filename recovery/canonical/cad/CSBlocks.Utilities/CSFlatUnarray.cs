using CS.CSDataUtilities;
using CS.CSObjects.CSBlocks;
using CS.Parametric;
using CS.Parametric.Channels;
using CSDatas;

namespace CSBlocks.Utilities;

[CSBlock("FlatUnarray", "{AFB40CCD-E20E-4CEC-A0B7-E07AA214C838}")]
public sealed class CSFlatUnarray : CSBlockExpandableInput
{
	protected override string ExpandableInputChannelIdentifier => "InObject";

	protected override ChannelDataType ExpandableInputChannelDataType => ChannelDataType.Any;

	protected override ChannelArrayType ExpandableInputChannelArrayType => ChannelArrayType.Any;

	public CSFlatUnarray(string identifier)
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
		base.OutputChannels.Clear();
		for (int l = 0; l < cSDataArray.GetArrayCount(); l++)
		{
			new CSOutputChannel($"{base.Identifier}_{l}", this, ChannelDataType.Any, ChannelArrayType.Individual).SetData(cSDataArray.GetArrayItem(l));
		}
		return base.Update();
	}
}
