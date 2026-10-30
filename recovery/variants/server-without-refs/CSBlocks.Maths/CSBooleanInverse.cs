using CS.CSObjects.CSBlocks;
using CS.Parametric;
using CS.Parametric.Channels;
using CSDatas;

namespace CSBlocks.Maths;

[CSBlock("BooleanInverse", "{A7D5EF4A-E9EC-4C22-89B2-52B41898974D}")]
public sealed class CSBooleanInverse : CSBlock
{
	public CSInputChannel InValue { get; }

	public CSOutputChannel OutValue { get; }

	public CSBooleanInverse(string identifier)
		: base(identifier)
	{
		InValue = new CSInputChannel(this, ChannelDataType.Boolean, ChannelArrayType.Individual);
		OutValue = new CSOutputChannel(this, ChannelDataType.Boolean, ChannelArrayType.Individual);
		SetChannelIdentifiers();
	}

	public override bool Update()
	{
		bool value = false;
		InValue.GetBoolean(ref value);
		OutValue.SetData(new CSNumberData(value ? 0.0 : 1.0));
		return base.Update();
	}
}
