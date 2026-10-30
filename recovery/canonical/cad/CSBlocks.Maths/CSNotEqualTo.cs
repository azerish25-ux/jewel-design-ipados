using CS.CSObjects.CSBlocks;
using CS.Parametric;
using CS.Parametric.Channels;
using CSDatas;

namespace CSBlocks.Maths;

[CSBlock("NotEqualTo", "{C73DC056-9CEC-4C18-93F6-BC785B1C0ED6}")]
public sealed class CSNotEqualTo : CSBlock
{
	public readonly CSInputChannel iValueLhs;

	public readonly CSInputChannel iValueRhs;

	public readonly CSInputChannel iOutputIfFalse;

	public readonly CSInputChannel iOutputIfTrue;

	public readonly CSOutputChannel oValue;

	public readonly CSOutputChannel oOutput;

	public CSNotEqualTo(string identifier)
		: base(identifier)
	{
		iValueLhs = new CSInputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual);
		iValueRhs = new CSInputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual);
		iOutputIfFalse = new CSInputChannel(this, ChannelDataType.Any, ChannelArrayType.Any);
		iOutputIfTrue = new CSInputChannel(this, ChannelDataType.Any, ChannelArrayType.Any);
		oValue = new CSOutputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual);
		oOutput = new CSOutputChannel(this, ChannelDataType.Any, ChannelArrayType.Any);
		SetChannelIdentifiers();
	}

	public override bool Update()
	{
		double value = 0.0;
		iValueLhs.GetDouble(ref value);
		double value2 = 0.0;
		iValueRhs.GetDouble(ref value2);
		bool flag = value != value2;
		oValue.SetData(new CSNumberData(flag ? 1.0 : 0.0));
		oOutput.SetData(flag ? iOutputIfTrue.Data : iOutputIfFalse.Data);
		return base.Update();
	}
}
