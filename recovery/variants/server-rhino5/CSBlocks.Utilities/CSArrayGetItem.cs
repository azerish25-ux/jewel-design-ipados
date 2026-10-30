using CS.CSObjects.CSBlocks;
using CS.Parametric;
using CS.Parametric.Channels;
using CSDatas;
using CSDatas.Interfaces;

namespace CSBlocks.Utilities;

[CSBlock("ArrayGetItem", "{DA6EB0BC-7229-48F0-A18A-56749C1342AE}")]
public sealed class CSArrayGetItem : CSBlock
{
	public readonly CSInputChannel InArray;

	public readonly CSInputChannel InArrayIndex;

	public readonly CSOutputChannel OutValue;

	public CSArrayGetItem(string identifier)
		: base(identifier)
	{
		InArray = new CSInputChannel(this, ChannelDataType.Any, ChannelArrayType.Array);
		InArrayIndex = new CSInputChannel(this, ChannelDataType.Integer, ChannelArrayType.Individual);
		OutValue = new CSOutputChannel(this, ChannelDataType.Any, ChannelArrayType.Individual);
		SetChannelIdentifiers();
	}

	public override bool Update()
	{
		CSData data = InArray.Data;
		if (data == null)
		{
			OutValue.SetData(new CSData());
			return base.Update();
		}
		if (!(data is ICSArray iCSArray))
		{
			OutValue.SetData(new CSData());
			return base.Update();
		}
		int value = 0;
		InArrayIndex.GetInteger(ref value);
		int arrayCount = iCSArray.GetArrayCount();
		if (value < 0 || value >= arrayCount)
		{
			OutValue.SetData(new CSData());
			return base.Update();
		}
		OutValue.SetData(iCSArray.GetArrayItem(value).Duplicate());
		return base.Update();
	}
}
