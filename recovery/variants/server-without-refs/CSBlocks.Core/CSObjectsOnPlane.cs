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
		//IL_0032: Unknown result type (might be due to invalid IL or missing references)
		//IL_0037: Unknown result type (might be due to invalid IL or missing references)
		//IL_0044: Unknown result type (might be due to invalid IL or missing references)
		//IL_0049: Unknown result type (might be due to invalid IL or missing references)
		//IL_0064: Unknown result type (might be due to invalid IL or missing references)
		//IL_0069: Unknown result type (might be due to invalid IL or missing references)
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
			Point3d val = ((BoundingBox)(ref boundingBox)).Max;
			if (Math.Abs(((Point3d)(ref val)).Z) < 0.001)
			{
				val = ((BoundingBox)(ref boundingBox)).Min;
				if (Math.Abs(((Point3d)(ref val)).Z) < 0.001)
				{
					cSDataArray.Append(arrayItem);
				}
			}
		}
		OutObjects.SetData(cSDataArray);
		return base.Update();
	}
}
