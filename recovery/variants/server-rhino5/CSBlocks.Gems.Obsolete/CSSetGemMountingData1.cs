using System;
using System.Collections.Generic;
using CS.CSObjects.CSBlocks;
using CS.Parametric;
using CS.Parametric.Channels;
using CSDatas;

namespace CSBlocks.Gems.Obsolete;

[Obsolete]
[CSBlock("SetGemMountingData_V1", "{DA7709D3-D68F-48A2-A55D-92970FEB1B34}")]
public sealed class CSSetGemMountingData1 : CSBlock
{
	public readonly CSOutputChannel OutGem;

	public readonly CSInputChannel InGemDefinition;

	public readonly CSInputChannel InGem;

	public CSSetGemMountingData1(string identifier)
		: base(identifier)
	{
		InGemDefinition = new CSInputChannel(this, ChannelDataType.Gem, ChannelArrayType.Individual);
		InGem = new CSInputChannel(this, ChannelDataType.Gem, ChannelArrayType.Array);
		OutGem = new CSOutputChannel(this, ChannelDataType.Gem, ChannelArrayType.Array);
		SetChannelIdentifiers(autoCheckInputs: true);
	}

	public override bool Update()
	{
		CSGemData gem = InGemDefinition.Gem;
		List<CSGemData> gemList = InGem.GemList;
		if (gemList.Count <= 0)
		{
			return UpdateMissing("Gem");
		}
		CSDataArray cSDataArray = new CSDataArray();
		for (int i = 0; i < gemList.Count; i++)
		{
			CSGemData cSGemData = gemList[i];
			cSGemData.SettingType = gem.SettingType;
			cSDataArray.Append(cSGemData.Duplicate());
		}
		if (cSDataArray.GetArrayCount() == 1)
		{
			OutGem.SetData(cSDataArray.GetArrayItem(0));
		}
		else
		{
			OutGem.SetData(cSDataArray);
		}
		return base.Update();
	}
}
