using CS.CSObjects.CSBlocks;
using CS.Parametric;
using CS.Parametric.Channels;

namespace CSBlocks.Utilities;

[CSBlock("IfElse", "{6E2AD6B9-F9BB-4f5c-A2F2-83D5BC6C8DA9}")]
public sealed class CSIfElse : CSBlock
{
	public readonly CSInputChannel InCondition;

	public readonly CSInputChannel InIfConditionTrue;

	public readonly CSInputChannel InIfConditionFalse;

	public readonly CSOutputChannel OutOutput;

	public CSIfElse(string identifier)
		: base(identifier)
	{
		InCondition = new CSInputChannel(this, ChannelDataType.Boolean, ChannelArrayType.Individual);
		InIfConditionTrue = new CSInputChannel(this, ChannelDataType.Any, ChannelArrayType.Any);
		InIfConditionFalse = new CSInputChannel(this, ChannelDataType.Any, ChannelArrayType.Any);
		OutOutput = new CSOutputChannel(this, ChannelDataType.Any, ChannelArrayType.Any);
		SetChannelIdentifiers();
	}

	public override bool BlankOutBeforeUpdate()
	{
		return false;
	}

	public override bool Update()
	{
		bool value = true;
		InCondition.GetBoolean(ref value);
		OutOutput.SetData(value ? InIfConditionTrue.Data : InIfConditionFalse.Data);
		return base.Update();
	}
}
