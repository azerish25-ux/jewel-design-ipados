using System.Collections.Generic;
using CS.CSObjects.CSBlocks;
using CS.Parametric;
using CS.Parametric.Channels;
using CSDatas;
using GV.CounterSketch.Shared;

namespace CSBlocks.Gems;

[CSBlock("SetGemMountingData", "{EC32A38A-57B5-49E2-BF3F-59B686B06F5F}")]
public sealed class CSSetGemMountingData : CSBlock
{
	public readonly CSInputChannel InSettingType;

	public readonly CSInputChannel InGems;

	public readonly CSOutputChannel OutGems;

	public CSSetGemMountingData(string identifier)
		: base(identifier)
	{
		InGems = new CSInputChannel(this, ChannelDataType.Gem, ChannelArrayType.Array | ChannelArrayType.Individual);
		InSettingType = new CSInputChannel(this, ChannelDataType.Integer, ChannelArrayType.Individual, ValueListItem.SettingTypeXX);
		OutGems = new CSOutputChannel(this, ChannelDataType.Gem, ChannelArrayType.Array);
		SetChannelIdentifiers(autoCheckInputs: true);
	}

	public override bool Update()
	{
		List<CSGemData> gemList = InGems.GemList;
		ValueListItem valueListItem = InSettingType.ValueListItem;
		GemSettingType gemSettingType = GetGemSettingType(valueListItem);
		CSDataArray cSDataArray = new CSDataArray();
		foreach (CSGemData item in gemList)
		{
			if (item != null)
			{
				CSGemData data = new CSGemData(item)
				{
					SettingType = gemSettingType
				};
				cSDataArray.Append(data);
			}
		}
		OutGems.SetData((cSDataArray.GetArrayCount() == 1) ? cSDataArray.GetArrayItem(0) : cSDataArray);
		return base.Update();
	}

	private GemSettingType GetGemSettingType(ValueListItem whichValueListItem)
	{
		return whichValueListItem switch
		{
			ValueListItem.SettingTypeXX => GemSettingType.XX, 
			ValueListItem.SettingTypeBC => GemSettingType.BC, 
			ValueListItem.SettingTypeFB => GemSettingType.FB, 
			ValueListItem.SettingTypeBR => GemSettingType.BR, 
			ValueListItem.SettingTypeBZ => GemSettingType.BZ, 
			ValueListItem.SettingTypeHB => GemSettingType.HB, 
			ValueListItem.SettingTypePR => GemSettingType.PR, 
			ValueListItem.SettingTypeVP => GemSettingType.VP, 
			ValueListItem.SettingTypeCH => GemSettingType.CH, 
			ValueListItem.SettingTypePC => GemSettingType.PC, 
			ValueListItem.SettingTypeCM => GemSettingType.CM, 
			ValueListItem.SettingTypeFP => GemSettingType.FP, 
			ValueListItem.SettingTypePV => GemSettingType.PV, 
			ValueListItem.SettingTypeFT => GemSettingType.FT, 
			ValueListItem.SettingTypeGP => GemSettingType.GP, 
			ValueListItem.SettingTypeMB => GemSettingType.MB, 
			ValueListItem.SettingTypePT => GemSettingType.PT, 
			ValueListItem.SettingTypeSH => GemSettingType.SH, 
			ValueListItem.SettingTypeFS => GemSettingType.FC, 
			ValueListItem.SettingTypeCW => GemSettingType.CW, 
			_ => GemSettingType.XX, 
		};
	}
}
