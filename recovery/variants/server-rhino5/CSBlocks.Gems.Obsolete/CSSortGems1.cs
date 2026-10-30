using System;
using System.Collections.Generic;
using CS.CSObjects.CSBlocks;
using CS.Parametric;
using CS.Parametric.Channels;
using CSBlockDataUtils;
using CSDatas;

namespace CSBlocks.Gems.Obsolete;

[Obsolete]
[CSBlock("SortGems_V1", "{0B55DB1B-E05C-470f-87C1-723E9C81A30A}")]
public sealed class CSSortGems1 : CSBlock
{
	public CSInputChannel iGemArray { get; }

	public CSOutputChannel oSortedGemArray { get; }

	public CSSortGems1(string identifier)
		: base(identifier)
	{
		iGemArray = new CSInputChannel(this, ChannelDataType.Gem, ChannelArrayType.Array);
		oSortedGemArray = new CSOutputChannel(this, ChannelDataType.Gem, ChannelArrayType.Array);
		SetChannelIdentifiers(autoCheckInputs: true);
	}

	public override bool Update()
	{
		CSDataArray cSDataArray = new CSDataArray();
		List<CSGemData> gemList = iGemArray.GemList;
		if (gemList.Count <= 0)
		{
			return UpdateMissing("Gems");
		}
		for (int i = 0; i < gemList.Count; i++)
		{
			CSGemData cSGemData = gemList[i];
			if (cSGemData != null)
			{
				cSDataArray.Append(new CSChannelGemData(cSGemData));
				CSUtilGemRails.SortGems(cSDataArray);
			}
		}
		CSUtilGemRails.CreateRails(cSDataArray);
		oSortedGemArray.SetData(cSDataArray);
		return base.Update();
	}
}
