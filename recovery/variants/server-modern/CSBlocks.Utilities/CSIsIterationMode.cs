using CS;
using CS.CSObjects.CSBlocks;
using CS.Parametric;
using CS.Parametric.Channels;
using CSDatas;

namespace CSBlocks.Utilities;

[CSBlock("IsIterationMode", "{8FA385DE-776A-483F-BC54-04BBF17BAF2B}")]
public sealed class CSIsIterationMode : CSBlock
{
	public readonly CSOutputChannel OutValue;

	public CSIsIterationMode(string identifier)
		: base(identifier)
	{
		OutValue = new CSOutputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual);
		SetChannelIdentifiers();
	}

	public override bool Update()
	{
		OutValue.SetData(new CSNumberData(CSApplicationHelper.Application.IsIterationMode ? 1.0 : 0.0));
		return base.Update();
	}
}
