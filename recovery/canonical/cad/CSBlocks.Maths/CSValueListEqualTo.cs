using CS.CSObjects.CSBlocks;
using CS.Parametric;
using CS.Parametric.Channels;
using CSDatas;
using GV.CounterSketch.Shared;

namespace CSBlocks.Maths;

[CSBlock("ValueListEqualTo", "{39638E1B-2D9D-400E-BDA9-A3B76AAFEA32}")]
public sealed class CSValueListEqualTo : CSBlock
{
	public readonly CSInputChannel iValueLhs;

	public readonly CSInputChannel iValueRhs;

	public readonly CSInputChannel iOutputIfFalse;

	public readonly CSInputChannel iOutputIfTrue;

	public readonly CSOutputChannel oValue;

	public readonly CSOutputChannel oOutput;

	public CSValueListEqualTo(string identifier)
		: base(identifier)
	{
		iValueLhs = new CSInputChannel(this, ChannelDataType.Integer, ChannelArrayType.Individual);
		iValueRhs = new CSInputChannel(this, ChannelDataType.Integer, ChannelArrayType.Individual);
		iOutputIfFalse = new CSInputChannel(this, ChannelDataType.Any, ChannelArrayType.Any);
		iOutputIfTrue = new CSInputChannel(this, ChannelDataType.Any, ChannelArrayType.Any);
		oValue = new CSOutputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual);
		oOutput = new CSOutputChannel(this, ChannelDataType.Any, ChannelArrayType.Any);
		SetChannelIdentifiers();
	}

	public override bool Update()
	{
		ValueListItem whichItem = ValueListItem.Unknown;
		if (!iValueLhs.GetValueListItem(ref whichItem))
		{
			return UpdateMissing("ValueLhs");
		}
		ValueListItem whichItem2 = ValueListItem.Unknown;
		if (!iValueRhs.GetValueListItem(ref whichItem2))
		{
			return UpdateMissing("ValueRhs");
		}
		bool flag = whichItem == whichItem2;
		oValue.SetData(new CSNumberData(flag ? 1.0 : 0.0));
		oOutput.SetData(flag ? iOutputIfTrue.Data : iOutputIfFalse.Data);
		return base.Update();
	}
}
