using System.Collections.Generic;
using CS.CSObjects.CSBlocks;
using CS.Parametric;
using CS.Parametric.Channels;
using CSDatas;
using Rhino.Geometry;

namespace CSBlocks.Gems;

[CSBlock("GemRemoveIntersecting", "{B728251B-78BB-45F3-8491-07026F32223B}")]
public sealed class CSGemRemoveIntersecting : CSBlock
{
	public readonly CSInputChannel InGems;

	public readonly CSInputChannel InTolerance;

	public readonly CSOutputChannel OutGems;

	public CSGemRemoveIntersecting(string identifier)
		: base(identifier)
	{
		InGems = new CSInputChannel(this, ChannelDataType.Any, ChannelArrayType.Array);
		InTolerance = new CSInputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual, 0.01);
		OutGems = new CSOutputChannel(this, ChannelDataType.Any, ChannelArrayType.Array);
		SetChannelIdentifiers(autoCheckInputs: true);
	}

	public override bool BlankOutBeforeUpdate()
	{
		return false;
	}

	public override bool Update()
	{
		List<CSGemData> gemList = InGems.GemList;
		CSDataArray cSDataArray = new CSDataArray();
		double num = InTolerance.Double;
		for (int i = 0; i < gemList.Count; i++)
		{
			CSGemData cSGemData = gemList[i];
			bool flag = true;
			if (cSGemData != null)
			{
				Point3d center = cSGemData.Center;
				for (int j = 0; j < i; j++)
				{
					CSGemData cSGemData2 = gemList[j];
					if (cSGemData2 != null)
					{
						Point3d center2 = cSGemData2.Center;
						if ((center - center2).Length < num)
						{
							flag = false;
							break;
						}
					}
				}
			}
			if (flag)
			{
				cSDataArray.Append(gemList[i]);
			}
		}
		OutGems.SetData(cSDataArray);
		return base.Update();
	}
}
