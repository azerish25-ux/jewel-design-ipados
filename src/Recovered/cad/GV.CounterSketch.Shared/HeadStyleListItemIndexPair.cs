using System.Runtime.Serialization;

namespace GV.CounterSketch.Shared;

[DataContract]
public sealed class HeadStyleListItemIndexPair
{
	[DataMember]
	public int Index { get; set; }

	[DataMember]
	public bool UIDisabled { get; set; }

	[DataMember]
	public HeadStyle HeadStyleListItem { get; set; }

	public HeadStyleListItemIndexPair(int index, bool isUIDisabled, HeadStyle whichHeadStyle)
	{
		Index = index;
		UIDisabled = isUIDisabled;
		HeadStyleListItem = whichHeadStyle;
	}

	public HeadStyleListItemIndexPair(int index, HeadStyle whichHeadStyle)
		: this(index, isUIDisabled: false, whichHeadStyle)
	{
	}

	public HeadStyleListItemIndexPair(HeadStyle whichHeadStyle)
		: this((int)whichHeadStyle, isUIDisabled: false, whichHeadStyle)
	{
	}
}
