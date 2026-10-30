using System;
using System.Collections.Generic;
using CS.CSDataUtilities;
using CS.CSObjects.CSBlocks;
using CS.Parametric;
using CS.Parametric.Channels;
using CSBlockDataUtils;
using CSCompatibility;
using CSDatas;
using RMA.OpenNURBS;

namespace CSBlocks.Gems.Obsolete;

[Obsolete]
[CSBlock("ResizeGem_V1", "{314A379E-6852-4B20-92B6-BC101FA45D53}")]
public sealed class CSResizeGem1 : CSBlock
{
	public CSOutputChannel OutResizedGem;

	public readonly CSInputChannel InGem;

	public readonly CSInputChannel InSizeX;

	public readonly CSInputChannel InSizeY;

	public readonly CSInputChannel InUseTable;

	public readonly CSInputChannel InUseLargestSizePossible;

	public CSResizeGem1(string identifier)
		: base(identifier)
	{
		InGem = new CSInputChannel(this, ChannelDataType.Gem, ChannelArrayType.Array);
		InSizeX = new CSInputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual, -1.23432101234321E+308);
		InSizeY = new CSInputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual, -1.23432101234321E+308);
		InUseTable = new CSInputChannel(this, ChannelDataType.Boolean, ChannelArrayType.Individual, defaultValue: false);
		InUseLargestSizePossible = new CSInputChannel(this, ChannelDataType.Boolean, ChannelArrayType.Individual, defaultValue: false);
		OutResizedGem = new CSOutputChannel(this, ChannelDataType.Gem, ChannelArrayType.Array);
		SetChannelIdentifiers(autoCheckInputs: true);
	}

	public override bool Update()
	{
		//IL_00b1: Unknown result type (might be due to invalid IL or missing references)
		List<CSGemData> gemList = InGem.GemList;
		if (gemList.Count <= 0)
		{
			return UpdateMissing("Missing Gem");
		}
		double requestedSizeX = InSizeX.Double;
		double requestedSizeY = InSizeY.Double;
		bool boolean = InUseTable.Boolean;
		bool boolean2 = InUseLargestSizePossible.Boolean;
		CSDataArray cSDataArray = new CSDataArray();
		for (int i = 0; i < gemList.Count; i++)
		{
			CSGemData cSGemData = gemList[i];
			if (cSGemData == null)
			{
				continue;
			}
			OnXform xBestScale;
			if (boolean)
			{
				if (!CSUtilGem.GetGemScaleFromTable(cSGemData, requestedSizeX, requestedSizeY, boolean2, false, out xBestScale))
				{
					return UpdateError("GetGemSizeFromTable failed");
				}
			}
			else if (!CSUtilGem.GetGemScale(cSGemData, requestedSizeX, requestedSizeY, out xBestScale))
			{
				return UpdateError("GetGemSize failed");
			}
			CSData data = cSGemData.Duplicate();
			CSDataHelper.Transform(data, xBestScale.Convert());
			cSDataArray.Append(data);
		}
		if (cSDataArray.GetArrayCount() == 1)
		{
			OutResizedGem.SetData(cSDataArray.GetArrayItem(0));
		}
		else
		{
			OutResizedGem.SetData(cSDataArray);
		}
		return base.Update();
	}
}
