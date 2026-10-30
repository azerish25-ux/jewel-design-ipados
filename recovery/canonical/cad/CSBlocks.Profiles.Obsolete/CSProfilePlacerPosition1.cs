using System;
using CS.CSObjects.CSBlocks;
using CS.Parametric;
using CS.Parametric.Channels;
using CSDatas;

namespace CSBlocks.Profiles.Obsolete;

[Obsolete]
[CSBlock("ProfilePlacerPosition_V1", "{F5E77AC0-E09B-4d02-B50A-D6CAA88D2174}")]
public sealed class CSProfilePlacerPosition1 : CSBlock
{
	public CSInputChannel iPosition { get; }

	public CSInputChannel iOffset { get; }

	public CSOutputChannel oPosition { get; }

	public CSProfilePlacerPosition1(string identifier)
		: base(identifier)
	{
		iPosition = new CSInputChannel(GetPropertyName(() => iPosition), this, ChannelDataType.Double, ChannelArrayType.Individual);
		iOffset = new CSInputChannel(GetPropertyName(() => iOffset), this, ChannelDataType.Double, ChannelArrayType.Individual);
		oPosition = new CSOutputChannel(GetPropertyName(() => oPosition), this, ChannelDataType.Double, ChannelArrayType.Individual);
	}

	public override bool Update()
	{
		double value = 0.0;
		iPosition.GetDouble(ref value);
		double value2 = 0.0;
		iOffset.GetDouble(ref value2);
		value += value2;
		if (value < 0.0)
		{
			value += 1.0;
		}
		if (value > 1.0)
		{
			value -= 1.0;
		}
		if (value == 1.0)
		{
			value = 0.0;
		}
		oPosition.SetData(new CSNumberData(value));
		return base.Update();
	}
}
