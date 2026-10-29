using System.Xml.Serialization;

namespace CS.Pattern;

public struct CompareChannelConnections(int iBlockInputNumber, int iChannelObjOffset, int iChannelOutput)
{
	[XmlAttribute]
	public int blockInputNumber = iBlockInputNumber;

	[XmlAttribute]
	public int channelObjOffset = iChannelObjOffset;

	[XmlAttribute]
	public int channelOutput = iChannelOutput;

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
