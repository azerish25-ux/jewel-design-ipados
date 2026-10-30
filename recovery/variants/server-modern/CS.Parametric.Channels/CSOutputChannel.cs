using CS.CSObjects;
using CS.CSObjects.CSBlocks;
using CSDatas;

namespace CS.Parametric.Channels;

public sealed class CSOutputChannel : CSChannel
{
	private CSData _data;

	public override ChannelArrayType ArrayTypeFilter
	{
		get
		{
			return _arrayTypeFilter;
		}
		set
		{
			ChannelArrayType channelArrayType = value;
			if (channelArrayType.HasFlag(ChannelArrayType.Array))
			{
				channelArrayType |= ChannelArrayType.Array;
			}
			_arrayTypeFilter = channelArrayType;
		}
	}

	public override ChannelDataType DataTypeFilter
	{
		get
		{
			return _dataTypeFilter;
		}
		set
		{
			_dataTypeFilter = value;
		}
	}

	public override CSData Data => _data;

	public void SetData(CSData value)
	{
		_data = value;
	}

	public CSOutputChannel(string identifier, CSObject parent, ChannelDataType dataType, ChannelArrayType arrayType)
		: base(identifier, parent, dataType, arrayType)
	{
		_data = new CSData();
		if (base.Parent is CSBlock cSBlock)
		{
			cSBlock.AddOutputChannelToParent(this);
		}
	}

	public CSOutputChannel(CSBlock parent, ChannelDataType dataType, ChannelArrayType arrayType)
		: this(string.Empty, parent, dataType, arrayType)
	{
	}

	public CSOutputChannel(CSChannel copy)
		: base(copy)
	{
	}
}
