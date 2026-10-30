using System.Xml.Serialization;

namespace CS.Pattern;

public struct CompareChannelConnections
{
	[XmlAttribute]
	public int blockInputNumber;

	[XmlAttribute]
	public int channelObjOffset;

	[XmlAttribute]
	public int channelOutput;

	public CompareChannelConnections(int iBlockInputNumber, int iChannelObjOffset, int iChannelOutput)
	{
		blockInputNumber = iBlockInputNumber;
		channelObjOffset = iChannelObjOffset;
		channelOutput = iChannelOutput;
	}

	public bool Equals(CompareChannelConnections obj)
	{
		if (obj.blockInputNumber != blockInputNumber)
		{
			return false;
		}
		if (obj.channelObjOffset != channelObjOffset)
		{
			return false;
		}
		if (obj.channelOutput != channelOutput)
		{
			return false;
		}
		return true;
	}
}
