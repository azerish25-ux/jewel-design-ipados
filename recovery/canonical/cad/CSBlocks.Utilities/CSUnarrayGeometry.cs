using CS.CSObjects.CSBlocks;
using CS.Parametric;
using CS.Parametric.Channels;
using CSDatas;

namespace CSBlocks.Utilities;

[CSBlock("UnarrayGeometry", "{BFCC64BC-1E82-4AE5-BEB7-F698622035AD}")]
public sealed class CSUnarrayGeometry : CSBlockExpandableInput
{
	protected override string ExpandableInputChannelIdentifier => "InObject";

	protected override ChannelDataType ExpandableInputChannelDataType => ChannelDataType.Any;

	protected override ChannelArrayType ExpandableInputChannelArrayType => ChannelArrayType.Any;

	public CSUnarrayGeometry(string identifier)
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
			if (data != null && data.GetType() == typeof(CSOnGeometryData))
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
