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
		//IL_000c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0011: Unknown result type (might be due to invalid IL or missing references)
		//IL_0016: Unknown result type (might be due to invalid IL or missing references)
		//IL_0045: Unknown result type (might be due to invalid IL or missing references)
		//IL_004a: Unknown result type (might be due to invalid IL or missing references)
		//IL_005b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0068: Unknown result type (might be due to invalid IL or missing references)
		//IL_006d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0078: Unknown result type (might be due to invalid IL or missing references)
		//IL_007d: Unknown result type (might be due to invalid IL or missing references)
		//IL_008a: Unknown result type (might be due to invalid IL or missing references)
		//IL_008f: Unknown result type (might be due to invalid IL or missing references)
		//IL_009a: Unknown result type (might be due to invalid IL or missing references)
		//IL_009f: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ac: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b1: Unknown result type (might be due to invalid IL or missing references)
		//IL_00bc: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c1: Unknown result type (might be due to invalid IL or missing references)
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
			Point3d val = ((BoundingBox)(ref boundingBox)).Max;
			double y = ((Point3d)(ref val)).Y;
			val = ((BoundingBox)(ref boundingBox2)).Max;
			if (y != ((Point3d)(ref val)).Y)
			{
				continue;
			}
			val = ((BoundingBox)(ref boundingBox)).Max;
			double y2 = ((Point3d)(ref val)).Y;
			val = ((BoundingBox)(ref boundingBox2)).Min;
			if (y2 == ((Point3d)(ref val)).Y)
			{
				val = ((BoundingBox)(ref boundingBox)).Center;
				double x = ((Point3d)(ref val)).X;
				val = ((BoundingBox)(ref boundingBox2)).Center;
				double num2 = Math.Abs(x - ((Point3d)(ref val)).X);
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
