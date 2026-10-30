using System.Linq;
using CS.CSObjects.CSBlocks;
using CS.Parametric;
using CS.Parametric.Channels;
using CSDatas;

namespace CSBlocks.Maths;

[CSBlock("UnlimitedBooleanOr", "{5C1C1AE0-E72E-4467-90B6-2F6E9F930CA8}")]
public sealed class CSUnlimitedBooleanOr : CSBlockExpandableInput
{
	public CSOutputChannel OutValue { get; }

	protected override string ExpandableInputChannelIdentifier => "InValue";

	protected override ChannelDataType ExpandableInputChannelDataType => ChannelDataType.Boolean;

	protected override ChannelArrayType ExpandableInputChannelArrayType => ChannelArrayType.Individual;

	public CSUnlimitedBooleanOr(string identifier)
		: base(identifier)
	{
		CreateExpansionInputChannel();
		OutValue = new CSOutputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual);
		SetChannelIdentifiers();
	}

	public override bool Update()
	{
		bool flag = base.NonExpandableInputChannels.Any(delegate(CSInputChannel inputObject)
		{
			bool value = false;
			inputObject.GetBoolean(ref value);
			return value;
		});
		OutValue.SetData(new CSNumberData(flag ? 1.0 : 0.0));
		return base.Update();
	}
}
