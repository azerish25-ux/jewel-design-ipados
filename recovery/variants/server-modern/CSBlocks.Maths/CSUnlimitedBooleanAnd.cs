using System.Linq;
using CS.CSObjects.CSBlocks;
using CS.Parametric;
using CS.Parametric.Channels;
using CSDatas;

namespace CSBlocks.Maths;

[CSBlock("UnlimitedBooleanAnd", "{C2A86BA3-CE2D-4216-9E6A-759B77820FEF}")]
public sealed class CSUnlimitedBooleanAnd : CSBlockExpandableInput
{
	public CSOutputChannel OutValue { get; }

	protected override string ExpandableInputChannelIdentifier => "InValue";

	protected override ChannelDataType ExpandableInputChannelDataType => ChannelDataType.Boolean;

	protected override ChannelArrayType ExpandableInputChannelArrayType => ChannelArrayType.Individual;

	public CSUnlimitedBooleanAnd(string identifier)
		: base(identifier)
	{
		CreateExpansionInputChannel();
		OutValue = new CSOutputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual);
		SetChannelIdentifiers();
	}

	public override bool Update()
	{
		bool flag = base.NonExpandableInputChannels.All(delegate(CSInputChannel inputObject)
		{
			bool value = false;
			inputObject.GetBoolean(ref value);
			return value;
		});
		OutValue.SetData(new CSNumberData(flag ? 1.0 : 0.0));
		return base.Update();
	}
}
