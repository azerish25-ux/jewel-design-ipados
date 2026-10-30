using CS.CSObjects.CSBlocks;
using CS.Parametric;
using CS.Parametric.Channels;
using CSDatas;
using CSDatas.Interfaces;

namespace CSBlocks.Utilities;

[CSBlock("ArrayCount", "{302358AD-ECEF-4E08-A49C-CDC0C4EF9A96}")]
public sealed class CSArrayCount : CSBlock
{
	public readonly CSInputChannel InArray;

	public readonly CSOutputChannel OutCount;

	public readonly CSOutputChannel OutFlatCount;

	public CSArrayCount(string identifier)
		: base(identifier)
	{
		InArray = new CSInputChannel(this, ChannelDataType.Any, ChannelArrayType.Array);
		OutCount = new CSOutputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual);
		OutFlatCount = new CSOutputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual);
		SetChannelIdentifiers();
	}

	public override bool Update()
	{
		CSData data = InArray.Data;
		if (data == null)
		{
			OutCount.SetData(new CSNumberData(0.0));
			OutFlatCount.SetData(new CSNumberData(0.0));
			return base.Update();
		}
		if (!(data is ICSArray iCSArray))
		{
			OutCount.SetData(new CSNumberData(0.0));
			OutFlatCount.SetData(new CSNumberData(0.0));
			return base.Update();
		}
		CSDataArray flattenArray = iCSArray.GetFlattenArray();
		OutCount.SetData(new CSNumberData(iCSArray.GetArrayCount()));
		OutFlatCount.SetData(new CSNumberData(flattenArray.GetArrayCount()));
		return base.Update();
	}
}
