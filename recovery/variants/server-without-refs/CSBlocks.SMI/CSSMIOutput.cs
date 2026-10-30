using System;
using CS.CSObjects.CSBlocks;
using CS.Parametric;
using CS.Parametric.Channels;
using CSDatas;

namespace CSBlocks.SMI;

[CSBlock("SMIOutput", "{89B077CB-8F1C-42a5-8C10-763F7E880E8B}")]
public sealed class CSSMIOutput : CSBlockExpandableInput
{
	public CSOutputChannel OutString { get; }

	protected override string ExpandableInputChannelIdentifier => "InReorderIndex";

	protected override ChannelDataType ExpandableInputChannelDataType => ChannelDataType.Double | ChannelDataType.Profile;

	protected override ChannelArrayType ExpandableInputChannelArrayType => ChannelArrayType.Array | ChannelArrayType.Individual;

	public CSSMIOutput(string identifier)
		: base(identifier)
	{
		CreateExpansionInputChannel();
		OutString = new CSOutputChannel(GetPropertyName(() => OutString), this, ChannelDataType.String, ChannelArrayType.Individual);
	}

	public override bool Update()
	{
		string text = "[" + base.Identifier + "]" + Environment.NewLine;
		foreach (CSInputChannel nonExpandableInputChannel in base.NonExpandableInputChannels)
		{
			if (nonExpandableInputChannel == null || string.IsNullOrEmpty(nonExpandableInputChannel.OutputChannelBlock.Identifier))
			{
				continue;
			}
			string text2 = null;
			CSDataArray flattenArray = nonExpandableInputChannel.GetFlattenArray();
			for (int i = 0; i < flattenArray.GetArrayCount(); i++)
			{
				CSData arrayItem = flattenArray.GetArrayItem(i);
				if (arrayItem == null)
				{
					continue;
				}
				if (arrayItem is CSNumberData)
				{
					double value = 0.0;
					if (arrayItem.GetDouble(ref value))
					{
						if (!string.IsNullOrEmpty(text2))
						{
							text2 += ", ";
						}
						text2 += value;
					}
				}
				else if (arrayItem is CSProfileData)
				{
					CSProfileData cSProfileData = arrayItem as CSProfileData;
					text2 += cSProfileData.Name;
				}
			}
			if (!string.IsNullOrEmpty(text2))
			{
				text = text + base.Identifier + " = " + text2 + Environment.NewLine;
			}
		}
		OutString.SetData(new CSStringData(text));
		return base.Update();
	}
}
