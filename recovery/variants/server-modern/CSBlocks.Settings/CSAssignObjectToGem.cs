using System.Collections.Generic;
using CS.CSObjects.CSBlocks;
using CS.Parametric;
using CS.Parametric.Channels;
using CSDatas;
using CSDatas.Interfaces;

namespace CSBlocks.Settings;

[CSBlock("AssignObjectToGem", "{F430BEDD-C381-4943-A6B5-24477ACF7FCD}")]
public sealed class CSAssignObjectToGem : CSBlock
{
	public readonly CSInputChannel InGem;

	public readonly CSInputChannel InObjects;

	public readonly CSOutputChannel OutObjects;

	public CSAssignObjectToGem(string identifier)
		: base(identifier)
	{
		InGem = new CSInputChannel(this, ChannelDataType.Gem, ChannelArrayType.Array);
		InObjects = new CSInputChannel(this, ChannelDataType.Any, ChannelArrayType.Array);
		OutObjects = new CSOutputChannel(this, ChannelDataType.Any, ChannelArrayType.Array);
		SetChannelIdentifiers(autoCheckInputs: true);
	}

	public override bool Update()
	{
		List<CSGemData> gemList = InGem.GemList;
		if (gemList.Count < 1)
		{
			return UpdateMissing("Invalid Array");
		}
		CSDataArray cSDataArray = new CSDataArray();
		for (int i = 0; i < gemList.Count; i++)
		{
			if (!(InObjects.GetArrayItem(i) is ICSArray iCSArray))
			{
				return UpdateMissing("Invalid Array");
			}
			CSDataArray flattenArray = iCSArray.GetFlattenArray();
			if (flattenArray == null || flattenArray.GetArrayCount() < 1)
			{
				return UpdateMissing("Invalid Array");
			}
			CSData data = flattenArray.Duplicate();
			if (gemList[i] == null)
			{
				return UpdateMissing("Invalid Gem");
			}
			cSDataArray.Append(data);
		}
		OutObjects.SetData(cSDataArray);
		return base.Update();
	}
}
