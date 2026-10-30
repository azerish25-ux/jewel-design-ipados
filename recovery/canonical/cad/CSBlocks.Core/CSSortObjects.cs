using System.Collections.Generic;
using CS.CSDataUtilities;
using CS.CSObjects.CSBlocks;
using CS.Parametric;
using CS.Parametric.Channels;
using CSDatas;
using CSDatas.Interfaces;
using GV.CounterSketch.Shared;
using Rhino.Geometry;

namespace CSBlocks.Core;

[CSBlock("SortObjects", "{E170A296-6B3E-44E8-BD87-6D208BB0DCC5}")]
public sealed class CSSortObjects : CSBlock
{
	public CSInputChannel InUnsortedObjects { get; }

	public CSInputChannel InSortDirection { get; }

	public CSInputChannel InSortSide { get; }

	public CSInputChannel InSortPoint { get; }

	public CSInputChannel InOutputMethod { get; }

	public CSOutputChannel OutSortedObjects { get; }

	public CSSortObjects(string identifier)
		: base(identifier)
	{
		InUnsortedObjects = new CSInputChannel(GetPropertyName(() => InUnsortedObjects), this, ChannelDataType.Any, ChannelArrayType.Array | ChannelArrayType.Individual);
		InSortDirection = new CSInputChannel(GetPropertyName(() => InSortDirection), this, ChannelDataType.Integer, ChannelArrayType.Individual);
		InSortSide = new CSInputChannel(GetPropertyName(() => InSortSide), this, ChannelDataType.Integer, ChannelArrayType.Individual);
		InSortPoint = new CSInputChannel(GetPropertyName(() => InSortPoint), this, ChannelDataType.Point, ChannelArrayType.Individual);
		InOutputMethod = new CSInputChannel(GetPropertyName(() => InOutputMethod), this, ChannelDataType.Integer, ChannelArrayType.Individual);
		OutSortedObjects = new CSOutputChannel(GetPropertyName(() => OutSortedObjects), this, ChannelDataType.Any, ChannelArrayType.Array | ChannelArrayType.Individual);
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
		ValueListItem whichItem3 = ValueListItem.SortOnCenterPoint;
		InSortSide.GetValueListItem(ref whichItem3);
		if (!CSDataHelper.GetPoint(InSortPoint, out var value))
		{
			value = Point3d.Unset;
		}
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
		List<double> list = new List<double>();
		if (!value.IsValid)
		{
			for (int i = 0; i < cSDataArray.GetArrayCount(); i++)
			{
				BoundingBox boundingBox = BoundingBox.Empty;
				cSDataArray.GetArrayItem(i).GetBoundingBox(ref boundingBox);
				Point3d point3d;
				switch (whichItem3)
				{
				case ValueListItem.SortOnMaxPoint:
					point3d = boundingBox.Max;
					break;
				case ValueListItem.SortOnMinPoint:
					point3d = boundingBox.Min;
					break;
				case ValueListItem.SortOnCenterPoint:
					point3d = boundingBox.Center;
					break;
				default:
					return UpdateError("Invalid Side Method");
				}
				double item = point3d.X * num + point3d.Y * num2 + point3d.Z * num3;
				list.Add(item);
			}
		}
		else
		{
			for (int j = 0; j < cSDataArray.GetArrayCount(); j++)
			{
				Point3d value2 = Point3d.Unset;
				CSData arrayItem = cSDataArray.GetArrayItem(j);
				if (!value2.IsValid && arrayItem is ICSBrep iCSBrep)
				{
					Brep brep = iCSBrep.GetBrep();
					if (brep != null)
					{
						value2 = brep.ClosestPoint(value);
					}
				}
				if (!value2.IsValid && arrayItem is ICSCurve iCSCurve)
				{
					Curve curve = iCSCurve.GetCurve();
					if (curve != null && curve.ClosestPoint(value, out var t))
					{
						value2 = curve.PointAt(t);
					}
				}
				if (!value2.IsValid && arrayItem is ICSPoint iCSPoint)
				{
					iCSPoint.GetPoint(out value2);
				}
				if (!value2.IsValid)
				{
					cSDataArray.RemoveAt(j);
					j--;
				}
				else
				{
					double length = (value2 - value).Length;
					list.Add(0.0 - length);
				}
			}
		}
		CSDataArray cSDataArray2 = new CSDataArray();
		while (cSDataArray.GetArrayCount() > 0)
		{
			int index = 0;
			double num4 = double.MinValue;
			if (cSDataArray.GetArrayCount() > 1)
			{
				for (int k = 0; k < cSDataArray.GetArrayCount(); k++)
				{
					double num5 = list[k];
					if (num5 > num4)
					{
						num4 = num5;
						index = k;
					}
				}
			}
			else
			{
				index = 0;
			}
			cSDataArray2.Append(cSDataArray.GetArrayItem(index));
			cSDataArray.RemoveAt(index);
			list.RemoveAt(index);
		}
		switch (whichItem2)
		{
		case ValueListItem.SortAxisOutputArray:
			OutSortedObjects.SetData(cSDataArray2);
			break;
		case ValueListItem.SortAxisOutputOutputs:
		{
			base.OutputChannels.Clear();
			for (int l = 0; l < cSDataArray2.GetArrayCount(); l++)
			{
				new CSOutputChannel("OutSortedObjects_" + l, this, ChannelDataType.Any, ChannelArrayType.Any).SetData(cSDataArray2.GetArrayItem(l));
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
