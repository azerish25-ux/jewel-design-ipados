using System.Collections.Generic;
using CS.CSObjects.CSBlocks;
using CS.Parametric;
using CS.Parametric.Channels;
using CSDatas;

namespace CSBlocks.Gems;

[CSBlock("SetGemMountingDataFromGem", "{DCFA1D37-29D3-419E-93A2-BE386B2ED68C}")]
public sealed class CSSetGemMountingDataFromGem : CSBlock
{
	public readonly CSInputChannel InGemDefinition;

	public readonly CSInputChannel InGem;

	public readonly CSOutputChannel OutGem;

	public CSSetGemMountingDataFromGem(string identifier)
		: base(identifier)
	{
		InGemDefinition = new CSInputChannel(this, ChannelDataType.Gem, ChannelArrayType.Individual);
		InGem = new CSInputChannel(this, ChannelDataType.Gem, ChannelArrayType.Array);
		OutGem = new CSOutputChannel(this, ChannelDataType.Gem, ChannelArrayType.Array);
		SetChannelIdentifiers(autoCheckInputs: true);
	}

	public override bool Update()
	{
		List<CSGemData> gemList = InGem.GemList;
		CSGemData gem = InGemDefinition.Gem;
		CSDataArray cSDataArray = new CSDataArray();
		foreach (CSGemData item in gemList)
		{
			if (item != null)
			{
				CSGemData data = new CSGemData(item)
				{
					SettingType = gem.SettingType
				};
				cSDataArray.Append(data);
			}
		}
		OutGem.SetData((cSDataArray.GetArrayCount() == 1) ? cSDataArray.GetArrayItem(0) : cSDataArray);
		return base.Update();
	}
}
