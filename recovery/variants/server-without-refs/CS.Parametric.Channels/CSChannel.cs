using CS.CSObjects;
using CSDatas;

namespace CS.Parametric.Channels;

public abstract class CSChannel
{
	public bool SocketStatic;

	public string Identifier;

	protected ChannelArrayType _arrayTypeFilter;

	protected ChannelDataType _dataTypeFilter;

	public abstract ChannelArrayType ArrayTypeFilter { get; set; }

	public abstract ChannelDataType DataTypeFilter { get; set; }

	public abstract CSData Data { get; }

	public CSObject Parent { get; set; }

	public void SetParent(CSObject parent)
	{
		Parent = parent;
	}

	protected CSChannel(string identifier, CSObject parent, ChannelDataType dataTypeFilter, ChannelArrayType arrayTypeFilter)
	{
		Identifier = identifier;
		Parent = parent;
		_dataTypeFilter = dataTypeFilter;
		_arrayTypeFilter = arrayTypeFilter;
	}

	protected CSChannel(CSChannel channel)
		: this(channel.Identifier, channel.Parent, channel.DataTypeFilter, channel.ArrayTypeFilter)
	{
	}

	protected CSChannel()
		: this(string.Empty, null, ChannelDataType.Any, ChannelArrayType.Any)
	{
	}
}
