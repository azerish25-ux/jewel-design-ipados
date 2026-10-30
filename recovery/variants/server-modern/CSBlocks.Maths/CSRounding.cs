using System;
using CS.CSObjects.CSBlocks;
using CS.Parametric;
using CS.Parametric.Channels;
using CSDatas;

namespace CSBlocks.Maths;

[CSBlock("Rounding", "{C201C4F2-2D71-454A-AE1E-4E2C43AA9459}")]
public sealed class CSRounding : CSBlock
{
	public readonly CSInputChannel InValue;

	public readonly CSInputChannel InDecimalPlaces;

	public readonly CSOutputChannel OutValue;

	public CSRounding(string identifier)
		: base(identifier)
	{
		InValue = new CSInputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual);
		InDecimalPlaces = new CSInputChannel(this, ChannelDataType.Integer, ChannelArrayType.Individual);
		OutValue = new CSOutputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual);
		SetChannelIdentifiers();
	}

	public override bool Update()
	{
		double value = 0.0;
		InValue.GetDouble(ref value);
		int value2 = 0;
		InDecimalPlaces.GetInteger(ref value2);
		OutValue.SetData(new CSNumberData(Math.Round(value, value2)));
		return base.Update();
	}
}
