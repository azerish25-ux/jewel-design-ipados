using System.Collections.Generic;
using CS.CSDataUtilities;
using CS.CSObjects.CSBlocks;
using CS.Parametric;
using CS.Parametric.Channels;
using CSDatas;

namespace CSBlocks.Gems;

[CSBlock("RemoveIntersectionGems", "{12941952-592D-4EC3-9891-257262B609AC}")]
public sealed class CSRemoveIntersectionGems : CSBlock
{
	public readonly CSInputChannel InGems;

	public readonly CSInputChannel InObjects;

	public readonly CSInputChannel InTolerance;

	public readonly CSInputChannel InShowGems;

	public readonly CSOutputChannel OutGems;

	public readonly CSOutputChannel OutDisplayGems;

	public CSRemoveIntersectionGems(string identifier)
		: base(identifier)
	{
		InGems = new CSInputChannel(this, ChannelDataType.Gem, ChannelArrayType.Array);
		InObjects = new CSInputChannel(this, ChannelDataType.Any, ChannelArrayType.Array);
		InTolerance = new CSInputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual, 0.01);
		InShowGems = new CSInputChannel(this, ChannelDataType.Boolean, ChannelArrayType.Individual, defaultValue: true);
		OutGems = new CSOutputChannel(this, ChannelDataType.Gem, ChannelArrayType.Array);
		OutDisplayGems = new CSOutputChannel(this, ChannelDataType.Gem, ChannelArrayType.Array);
		SetChannelIdentifiers(autoCheckInputs: true);
	}

	public override bool BlankOutBeforeUpdate()
	{
		return false;
	}

	public override bool Update()
	{
		List<CSGemData> gemList = InGems.GemList;
		CSDataArray flattenArray = InObjects.GetFlattenArray();
		CSDataArray cSDataArray = new CSDataArray();
		double num = InTolerance.Double;
		bool boolean = InShowGems.Boolean;
		for (int i = 0; i < gemList.Count; i++)
		{
			CSGemData cSGemData = gemList[i];
			bool flag = true;
			if (flattenArray != null || flattenArray.GetArrayCount() < 1)
			{
				foreach (CSData flattened in flattenArray.GetFlattenedList())
				{
					CSDataHelper.GetBrep(flattened, out var value);
					if (value.ClosestPoint(cSGemData.Center).DistanceTo(cSGemData.Center) - cSGemData.GemXSize / 2.0 - num <= 0.0)
					{
						flag = false;
					}
					if (value.IsPointInside(cSGemData.Center, 0.01, strictlyIn: true))
					{
						flag = false;
					}
				}
			}
			if (flag)
			{
				cSDataArray.Append(gemList[i]);
			}
		}
		OutGems.SetData(cSDataArray);
		if (boolean)
		{
			OutDisplayGems.SetData(cSDataArray);
		}
		else
		{
			OutDisplayGems.SetData(null);
		}
		return base.Update();
	}
}
