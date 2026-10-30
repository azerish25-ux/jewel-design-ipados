using System;
using CS.CSObjects.CSBlocks;
using CS.Parametric;
using CS.Parametric.Channels;
using CSDatas;
using CSDatas.Interfaces;
using Rhino.Geometry;

namespace CSBlocks.Core;

[CSBlock("ObjectsOnPlane", "{49C328B2-F1FB-476e-85CB-1E5C47BB8898}")]
public sealed class CSObjectsOnPlane : CSBlock
{
	public readonly CSInputChannel InObjects;

	public readonly CSOutputChannel OutObjects;

	public CSObjectsOnPlane(string identifier)
		: base(identifier)
	{
		InObjects = new CSInputChannel(this, ChannelDataType.Any, ChannelArrayType.Any);
		OutObjects = new CSOutputChannel(this, ChannelDataType.Any, ChannelArrayType.Any);
		SetChannelIdentifiers();
	}

	public override bool Update()
	{
		if (!(InObjects.Data is ICSArray iCSArray))
		{
			return UpdateMissing("Missing objects");
		}
		CSDataArray cSDataArray = new CSDataArray();
		for (int i = 0; i < iCSArray.GetArrayCount(); i++)
		{
			CSData arrayItem = iCSArray.GetArrayItem(i);
			BoundingBox boundingBox = BoundingBox.Unset;
			arrayItem.GetTightBoundingBox(ref boundingBox);
			if (Math.Abs(boundingBox.Max.Z) < 0.001 && Math.Abs(boundingBox.Min.Z) < 0.001)
			{
				cSDataArray.Append(arrayItem);
			}
		}
		OutObjects.SetData(cSDataArray);
		return base.Update();
	}
}
