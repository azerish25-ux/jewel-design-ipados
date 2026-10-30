using System;
using CS.CSObjects.CSBlocks;
using CS.Parametric;
using CS.Parametric.Channels;
using CSDatas;
using GV.CounterSketch.Shared;
using Rhino.Geometry;

namespace CSBlocks.Core.Obsolete;

[Obsolete]
[CSBlock("SortObjects_V3", "{76EEE2A6-DFA7-43C3-80AA-EDA32F6BB8CA}")]
public sealed class CSSortObjects3 : CSBlock
{
	public readonly CSInputChannel InUnsortedObjects;

	public readonly CSInputChannel InSortDirection;

	public readonly CSInputChannel InOutputMethod;

	public readonly CSOutputChannel OutSortedObjects;

	public CSSortObjects3(string identifier)
		: base(identifier)
	{
		InUnsortedObjects = new CSInputChannel(this, ChannelDataType.Any, ChannelArrayType.Array | ChannelArrayType.Individual);
		InSortDirection = new CSInputChannel(this, ChannelDataType.Integer, ChannelArrayType.Individual);
		InOutputMethod = new CSInputChannel(this, ChannelDataType.Integer, ChannelArrayType.Individual);
		OutSortedObjects = new CSOutputChannel(this, ChannelDataType.Any, ChannelArrayType.Array | ChannelArrayType.Individual);
		SetChannelIdentifiers();
	}

	public override bool Update()
	{
		if (!(InUnsortedObjects.GetFlattenArray().Duplicate() is CSDataArray cSDataArray))
		{
			return UpdateMissing("Missing unsorted objects");
		}
		ValueListItem whichItem = ValueListItem.SortAxisXNeg;
		InSortDirection.GetValueListItem(ref whichItem);
		ValueListItem whichItem2 = ValueListItem.SortAxisOutputFirstOnly;
		InOutputMethod.GetValueListItem(ref whichItem2);
		double num = 0.0;
		double num2 = 0.0;
		double num3 = 0.0;
		switch (whichItem)
		{
		case ValueListItem.SortAxisXPos:
			num = 1.0;
			break;
		case ValueListItem.SortAxisXNeg:
			num = -1.0;
			break;
		case ValueListItem.SortAxisYPos:
			num2 = 1.0;
			break;
		case ValueListItem.SortAxisYNeg:
			num2 = -1.0;
			break;
		case ValueListItem.SortAxisZPos:
			num3 = 1.0;
			break;
		case ValueListItem.SortAxisZNeg:
			num3 = -1.0;
			break;
		default:
			return UpdateError("Invalid Sort Direction");
		}
		CSDataArray cSDataArray2 = new CSDataArray();
		while (cSDataArray.GetArrayCount() > 0)
		{
			int index = 0;
			double num4 = double.MinValue;
			if (cSDataArray.GetArrayCount() > 1)
			{
				for (int i = 0; i < cSDataArray.GetArrayCount(); i++)
				{
					BoundingBox boundingBox = BoundingBox.Empty;
					cSDataArray.GetArrayItem(i).GetBoundingBox(ref boundingBox);
					Point3d center = boundingBox.Center;
					double num5 = center.X * num + center.Y * num2 + center.Z * num3;
					if (num5 > num4)
					{
						num4 = num5;
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
		switch (whichItem2)
		{
		case ValueListItem.SortAxisOutputArray:
			OutSortedObjects.SetData(cSDataArray2);
			break;
		case ValueListItem.SortAxisOutputOutputs:
		{
			base.OutputChannels.Clear();
			for (int j = 0; j < cSDataArray2.GetArrayCount(); j++)
			{
				new CSOutputChannel("OutSortedObjects_" + j, this, ChannelDataType.Any, ChannelArrayType.Any).SetData(cSDataArray2.GetArrayItem(j));
			}
			break;
		}
		case ValueListItem.SortAxisOutputFirstOnly:
			OutSortedObjects.SetData((cSDataArray2.GetArrayCount() > 0) ? cSDataArray2.GetArrayItem(0) : null);
			break;
		default:
			return UpdateError("Invalid Output Method");
		}
		return base.Update();
	}
}
