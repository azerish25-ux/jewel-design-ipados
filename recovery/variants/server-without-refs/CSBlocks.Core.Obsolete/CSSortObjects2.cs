using System;
using CS.CSObjects.CSBlocks;
using CS.Parametric;
using CS.Parametric.Channels;
using CSDatas;
using Rhino.Geometry;

namespace CSBlocks.Core.Obsolete;

[Obsolete]
[CSBlock("SortObjects_V2", "{2F824805-8A58-4E8B-9384-DCCB445CF6C7}")]
public sealed class CSSortObjects2 : CSBlock
{
	public readonly CSInputChannel iUnsortedObjects;

	public readonly CSInputChannel iSortX;

	public readonly CSInputChannel iSortY;

	public readonly CSInputChannel iSortZ;

	public readonly CSInputChannel iSortDir;

	public readonly CSInputChannel iOutputMethod;

	public readonly CSOutputChannel oSortedObjects;

	public CSSortObjects2(string identifier)
		: base(identifier)
	{
		iUnsortedObjects = new CSInputChannel(this, ChannelDataType.Any, ChannelArrayType.Array | ChannelArrayType.Individual);
		iSortX = new CSInputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual);
		iSortY = new CSInputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual);
		iSortZ = new CSInputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual);
		iSortDir = new CSInputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual);
		iOutputMethod = new CSInputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual);
		oSortedObjects = new CSOutputChannel(this, ChannelDataType.Any, ChannelArrayType.Array | ChannelArrayType.Individual);
		SetChannelIdentifiers();
	}

	public override bool Update()
	{
		//IL_0155: Unknown result type (might be due to invalid IL or missing references)
		//IL_015a: Unknown result type (might be due to invalid IL or missing references)
		//IL_016e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0173: Unknown result type (might be due to invalid IL or missing references)
		if (iUnsortedObjects == null)
		{
			return UpdateMissing("Missing unsorted objects channel");
		}
		if (!(iUnsortedObjects.GetFlattenArray().Duplicate() is CSDataArray cSDataArray))
		{
			return UpdateMissing("Missing unsorted objects");
		}
		double value = 0.0;
		double value2 = 0.0;
		double value3 = 0.0;
		if (iSortDir == null)
		{
			if (iSortX != null)
			{
				iSortX.GetDouble(ref value);
			}
			if (iSortY != null)
			{
				iSortY.GetDouble(ref value2);
			}
			if (iSortZ != null)
			{
				iSortZ.GetDouble(ref value3);
			}
		}
		else
		{
			int value4 = 0;
			iSortDir.GetInteger(ref value4);
			switch (value4)
			{
			case 0:
				value = 1.0;
				break;
			case 1:
				value = -1.0;
				break;
			case 2:
				value2 = 1.0;
				break;
			case 3:
				value2 = -1.0;
				break;
			case 4:
				value3 = 1.0;
				break;
			case 5:
				value3 = -1.0;
				break;
			default:
				value = -1.0;
				break;
			}
		}
		CSDataArray cSDataArray2 = new CSDataArray();
		while (cSDataArray.GetArrayCount() > 0)
		{
			int index = 0;
			double num = double.MinValue;
			if (cSDataArray.GetArrayCount() > 1)
			{
				for (int i = 0; i < cSDataArray.GetArrayCount(); i++)
				{
					BoundingBox boundingBox = BoundingBox.Empty;
					cSDataArray.GetArrayItem(i).GetBoundingBox(ref boundingBox);
					Point3d center = ((BoundingBox)(ref boundingBox)).Center;
					double num2 = ((Point3d)(ref center)).X * value + ((Point3d)(ref center)).Y * value2 + ((Point3d)(ref center)).Z * value3;
					if (num2 > num)
					{
						num = num2;
						index = i;
					}
				}
			}
			else
			{
				index = 0;
			}
			cSDataArray2.Append(cSDataArray.GetArrayItem(index));
			cSDataArray.RemoveAt(index);
		}
		int value5 = 0;
		iOutputMethod.GetInteger(ref value5);
		switch (value5)
		{
		case 0:
			oSortedObjects.SetData(cSDataArray2);
			break;
		case 1:
		{
			base.OutputChannels.Clear();
			for (int j = 0; j < cSDataArray2.GetArrayCount(); j++)
			{
				new CSOutputChannel("OutSortedObjects_" + j, this, ChannelDataType.Any, ChannelArrayType.Any).SetData(cSDataArray2.GetArrayItem(j));
			}
			break;
		}
		case 2:
			oSortedObjects.SetData((cSDataArray2.GetArrayCount() > 0) ? cSDataArray2.GetArrayItem(0) : null);
			break;
		default:
			oSortedObjects.SetData(cSDataArray2);
			break;
		}
		return base.Update();
	}
}
