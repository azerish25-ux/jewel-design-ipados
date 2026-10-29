using System.Runtime.Serialization;

namespace GV.CounterSketch.Shared;

[DataContract]
public class ListItemIndexPair
{
	[DataMember]
	public int Index { get; set; }

	[DataMember]
	public bool UIDisabled { get; set; }

	[DataMember]
	public ValueListItem ValueListItem { get; set; }

	public ListItemIndexPair()
	{
	}

	public ListItemIndexPair(int index, bool isUIDisabled, ValueListItem whichValueListItem)
	{
		Index = index;
		UIDisabled = isUIDisabled;
		ValueListItem = whichValueListItem;
	}

	public ListItemIndexPair(int index, ValueListItem whichValueListItem)
		: this(index, isUIDisabled: false, whichValueListItem)
	{
	}

	public ListItemIndexPair(ValueListItem whichValueListItem)
		: this((int)whichValueListItem, isUIDisabled: false, whichValueListItem)
	{
	}
}
