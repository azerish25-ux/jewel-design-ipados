using CS.CSObjects.CSBlocks;
using CS.Parametric;
using CS.Parametric.Channels;
using CSDatas;

namespace CSBlocks.SMI;

[CSBlock("SMIInterval", "{8CCACB93-58B3-43f0-85DE-34859B93C074}")]
public sealed class CSSMIInterval : CSBlock
{
	public CSInputChannel iCount { get; }

	public CSOutputChannel oCount { get; }

	public CSOutputChannel oAngle { get; }

	public CSSMIInterval(string identifier)
		: base(identifier)
	{
		iCount = new CSInputChannel(GetPropertyName(() => iCount), this, ChannelDataType.Double, ChannelArrayType.Individual);
		oCount = new CSOutputChannel(GetPropertyName(() => oCount), this, ChannelDataType.Integer, ChannelArrayType.Individual);
		oAngle = new CSOutputChannel(GetPropertyName(() => oAngle), this, ChannelDataType.Double, ChannelArrayType.Individual);
	}

	public override bool Update()
	{
		int value = 0;
		iCount.GetInteger(ref value);
		double number = 360.0 / (double)value;
		oCount.SetData(new CSNumberData(value));
		oAngle.SetData(new CSNumberData(number));
		return base.Update();
	}
}
