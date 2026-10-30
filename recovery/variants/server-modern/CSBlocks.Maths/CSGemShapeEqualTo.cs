using CS.CSObjects.CSBlocks;
using CS.Parametric;
using CS.Parametric.Channels;
using CSDatas;

namespace CSBlocks.Maths;

[CSBlock("GemShapeEqualTo", "{6EE65B85-B91D-4DAF-82A3-79157A34FB5C}")]
public sealed class CSGemShapeEqualTo : CSBlock
{
	public readonly CSInputChannel iValueLhs;

	public readonly CSInputChannel iValueRhs;

	public readonly CSInputChannel iOutputIfFalse;

	public readonly CSInputChannel iOutputIfTrue;

	public readonly CSOutputChannel oValue;

	public readonly CSOutputChannel oOutput;

	public CSGemShapeEqualTo(string identifier)
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
		if (!(iValueLhs.Data is CSGemData cSGemData))
		{
			return UpdateMissing("ValueLhs");
		}
		if (!(iValueRhs.Data is CSGemData cSGemData2))
		{
			return UpdateMissing("ValueRhs");
		}
		bool flag = cSGemData.GemShape == cSGemData2.GemShape;
		oValue.SetData(new CSNumberData(flag ? 1.0 : 0.0));
		oOutput.SetData(flag ? iOutputIfTrue.Data : iOutputIfFalse.Data);
		return base.Update();
	}
}
