using System.Collections.Generic;
using CS.CSDataUtilities;
using CS.CSObjects.CSBlocks;
using CS.Parametric;
using CS.Parametric.Channels;
using CSBlockDataUtils;
using CSDatas;
using GV.CounterSketch.Shared;
using Rhino.Geometry;

namespace CSBlocks.Gems;

[CSBlock("ResizeGem", "{CD81A878-58F7-4484-8917-78C647996935}")]
public sealed class CSResizeGem : CSBlock
{
	public CSOutputChannel OutResizedGem { get; }

	public CSInputChannel InGem { get; }

	public CSInputChannel InSizeX { get; }

	public CSInputChannel InSizeY { get; }

	public CSInputChannel InResizeMode { get; }

	public CSInputChannel InUseLargestSizePossible { get; }

	public CSResizeGem(string identifier)
		: base(identifier)
	{
		InGem = new CSInputChannel(this, ChannelDataType.Gem, ChannelArrayType.Array);
		InSizeX = new CSInputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual, -1.23432101234321E+308);
		InSizeY = new CSInputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual, -1.23432101234321E+308);
		InResizeMode = new CSInputChannel(this, ChannelDataType.Integer, ChannelArrayType.Individual, ValueListItem.ResizeGemModeUnconstrained);
		InUseLargestSizePossible = new CSInputChannel(this, ChannelDataType.Boolean, ChannelArrayType.Individual, defaultValue: false);
		OutResizedGem = new CSOutputChannel(this, ChannelDataType.Gem, ChannelArrayType.Array);
		SetChannelIdentifiers(autoCheckInputs: true);
	}

	public override bool Update()
	{
		List<CSGemData> gemList = InGem.GemList;
		if (gemList.Count <= 0)
		{
			return UpdateMissing("Missing Gem");
		}
		double requestedSizeX = InSizeX.Double;
		double requestedSizeY = InSizeY.Double;
		ValueListItem valueListItem = InResizeMode.ValueListItem;
		bool boolean = InUseLargestSizePossible.Boolean;
		CSDataArray cSDataArray = new CSDataArray();
		foreach (CSGemData item in gemList)
		{
			Transform xBestScale;
			switch (valueListItem)
			{
			case ValueListItem.ResizeGemModeUnconstrained:
				if (!CSUtilGem.GetGemScale(item, requestedSizeX, requestedSizeY, out xBestScale))
				{
					return UpdateError("GetGemSize failed");
				}
				break;
			case ValueListItem.ResizeGemModeConstrainedToFullGemSizeTable:
				if (!CSUtilGem.GetGemScaleFromTable(item, requestedSizeX, requestedSizeY, boolean, false, out xBestScale))
				{
					return UpdateError("GetGemSizeFromTable failed");
				}
				break;
			case ValueListItem.ResizeGemModeConstrainedToIterationGemSizeTable:
				if (!CSUtilGem.GetGemScaleFromTable(item, requestedSizeX, requestedSizeY, boolean, true, out xBestScale))
				{
					return UpdateError("GetGemSizeFromTable failed");
				}
				break;
			default:
				return UpdateError("Invalid resize mode");
			}
			CSData data = item.Duplicate();
			CSDataHelper.Transform(data, xBestScale);
			cSDataArray.Append(data);
		}
		OutResizedGem.SetData((cSDataArray.GetArrayCount() == 1) ? cSDataArray.GetArrayItem(0) : cSDataArray);
		return base.Update();
	}
}
