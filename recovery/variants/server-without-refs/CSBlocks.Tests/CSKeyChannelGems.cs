using System;
using System.Collections.Generic;
using CS.CSObjects.CSBlocks;
using CS.Parametric;
using CS.Parametric.Channels;
using CSBlockDataUtils;
using CSDatas;

namespace CSBlocks.Tests;

[Obsolete]
[CSBlock("KeyChannelGems", "{A2A18371-2543-4ECE-BFF2-5265878A99CE}")]
public sealed class CSKeyChannelGems : CSBlock
{
	public readonly CSInputChannel InSortedGems;

	public readonly CSOutputChannel OutGems;

	public CSKeyChannelGems(string identifier)
		: base(identifier)
	{
		InSortedGems = new CSInputChannel(this, ChannelDataType.Gem, ChannelArrayType.Individual);
		OutGems = new CSOutputChannel(this, ChannelDataType.Gem, ChannelArrayType.Array);
		SetChannelIdentifiers();
	}

	public override bool Update()
	{
		if (!(InSortedGems.Data is CSDataArray cSDataArray))
		{
			return UpdateMissing("Missing sorted gems");
		}
		if (cSDataArray.GetArrayCount() <= 0)
		{
			return UpdateMissing("Missing sorted gems");
		}
		IEnumerable<CSChannelGemData> keyChannelGems = CSUtilGemRails.GetKeyChannelGems(cSDataArray.Array);
		OutGems.SetData(new CSDataArray(keyChannelGems));
		return base.Update();
	}
}
