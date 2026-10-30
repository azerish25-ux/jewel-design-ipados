using System;
using CS.CSObjects.CSBlocks;
using CS.Parametric;
using CS.Parametric.Channels;
using CSDatas;
using Rhino.Geometry;

namespace CSBlocks.Core;

[CSBlock("CenterTopLine", "{9B770D58-DB90-4fef-995E-5FAA6C00D6DE}")]
public sealed class CSCenterTopLine : CSBlock
{
	public readonly CSInputChannel iCurves;

	public readonly CSOutputChannel oLine;

	public CSCenterTopLine(string identifier)
		: base(identifier)
	{
		iCurves = new CSInputChannel(this, ChannelDataType.Curve, ChannelArrayType.Array | ChannelArrayType.Individual);
		oLine = new CSOutputChannel(this, ChannelDataType.Curve, ChannelArrayType.Individual);
		SetChannelIdentifiers();
	}

	public override bool Update()
	{
		CSDataArray flattenArray = iCurves.GetFlattenArray();
		BoundingBox boundingBox = BoundingBox.Unset;
		flattenArray.GetTightBoundingBox(ref boundingBox, doGrow: true, Transform.Identity);
		if (flattenArray.GetArrayCount() < 1)
		{
			return base.Update();
		}
		CSData cSData = null;
		double num = double.MaxValue;
		for (int i = 0; i < flattenArray.GetArrayCount(); i++)
		{
			BoundingBox boundingBox2 = BoundingBox.Unset;
			CSData arrayItem = flattenArray.GetArrayItem(i);
			arrayItem.GetTightBoundingBox(ref boundingBox2, doGrow: true, Transform.Identity);
			if (boundingBox.Max.Y == boundingBox2.Max.Y && boundingBox.Max.Y == boundingBox2.Min.Y)
			{
				double num2 = Math.Abs(boundingBox.Center.X - boundingBox2.Center.X);
				if (num2 < num)
				{
					num = num2;
					cSData = arrayItem;
				}
			}
		}
		if (cSData != null)
		{
			oLine.SetData(cSData.Duplicate());
		}
		return base.Update();
	}
}
