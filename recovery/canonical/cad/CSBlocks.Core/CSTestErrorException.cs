using CS.CSObjects;
using CS.CSObjects.CSBlocks;
using CS.Parametric;
using CS.Parametric.Channels;

namespace CSBlocks.Core;

[CSBlock("TestErrorException", "{78841694-65B0-48e4-A7EF-36D6467AF7C2}")]
public sealed class CSTestErrorException : CSBlock
{
	public readonly CSInputChannel InTest;

	public readonly CSOutputChannel OutOutput;

	public CSTestErrorException(string identifier)
		: base(identifier)
	{
		InTest = new CSInputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual);
		OutOutput = new CSOutputChannel(this, ChannelDataType.Any, ChannelArrayType.Any);
		SetChannelIdentifiers();
	}

	public override bool Update()
	{
		double value = 0.0;
		InTest.GetDouble(ref value);
		switch ((int)value)
		{
		case 1:
			return UpdateMissing("Missing Input Test");
		case 2:
			return UpdateError("Test Update Errror");
		case 3:
			((CSObject)null).Update();
			return base.Update();
		case 4:
			return false;
		default:
			return base.Update();
		}
	}
}
