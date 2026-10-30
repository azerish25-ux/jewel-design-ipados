using CS.Parametric.Channels;
using CSDatas;

namespace CS.CSObjects.CSBlocks;

public abstract class CSGroup : CSBlock
{
	public override int OutputChannelCount => InputChannelCount;

	public CSGroup(string identifier)
		: base(identifier)
	{
	}

	public override CSOutputChannel GetOutputChannel(int outputChannelIndex)
	{
		if (outputChannelIndex > base.InputChannels.Count || base.InputChannels[outputChannelIndex].OutputChannelBlock == null || base.InputChannels[outputChannelIndex].OutputChannelBlock == this)
		{
			return null;
		}
		return base.InputChannels[outputChannelIndex].OutputChannelBlock.GetOutputChannel(outputChannelIndex);
	}

	public override CSData GetOutputChannelData(int outputChannelIndex)
	{
		if (outputChannelIndex > base.InputChannels.Count || base.InputChannels[outputChannelIndex].OutputChannelBlock == null || base.InputChannels[outputChannelIndex].OutputChannelBlock == this)
		{
			return new CSData();
		}
		return base.InputChannels[outputChannelIndex].Data;
	}

	public override string GetOutputChannelIdentifier(int outputChannelIndex)
	{
		if (outputChannelIndex > base.InputChannels.Count)
		{
			return "Invalid Channel";
		}
		return $"{outputChannelIndex}_LegacyGroup";
	}
}
