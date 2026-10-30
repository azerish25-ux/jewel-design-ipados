using CS.CSObjects.CSBlocks;
using CS.Parametric;
using CS.Parametric.Channels;
using CSDatas;
using GV.CounterSketch.Shared;
using Rhino.Geometry;

namespace CSBlocks.Points;

[CSBlock("PointFromObject", "{589666A3-9C8B-4D77-945A-CDBA78828BC3}")]
public sealed class CSPointFromObject : CSBlock
{
	public readonly CSOutputChannel OutPoint;

	public readonly CSInputChannel InObject;

	public readonly CSInputChannel InPointSelection;

	public CSPointFromObject(string identifier)
		: base(identifier)
	{
		InObject = new CSInputChannel(this, ChannelDataType.Any, ChannelArrayType.Individual);
		InPointSelection = new CSInputChannel(this, ChannelDataType.Integer, ChannelArrayType.Individual);
		OutPoint = new CSOutputChannel(this, ChannelDataType.Point, ChannelArrayType.Individual);
		SetChannelIdentifiers();
	}

	public override bool Update()
	{
		CSData data = InObject.Data;
		BoundingBox boundingBox = default(BoundingBox);
		data.GetTightBoundingBox(ref boundingBox);
		ValueListItem whichItem = ValueListItem.SortOnCenterPoint;
		InPointSelection.GetValueListItem(ref whichItem);
		Point3d center = boundingBox.Center;
		Point3d pointStore;
		switch (whichItem)
		{
		case ValueListItem.SortOnCenterPoint:
			pointStore = center;
			break;
		case ValueListItem.SortAxisXPos:
			center = boundingBox.Center;
			center.Transform(Transform.Translation(boundingBox.Max.X - center.X, 0.0, 0.0));
			pointStore = boundingBox.ClosestPoint(center);
			break;
		case ValueListItem.SortAxisXNeg:
			center.Transform(Transform.Translation(boundingBox.Min.X - center.X, 0.0, 0.0));
			pointStore = boundingBox.ClosestPoint(center);
			break;
		case ValueListItem.SortAxisYPos:
			center = boundingBox.Center;
			center.Transform(Transform.Translation(0.0, boundingBox.Max.Y - center.Y, 0.0));
			pointStore = boundingBox.ClosestPoint(center);
			break;
		case ValueListItem.SortAxisYNeg:
			center = boundingBox.Center;
			center.Transform(Transform.Translation(0.0, boundingBox.Min.Y - center.Y, 0.0));
			pointStore = boundingBox.ClosestPoint(center);
			break;
		case ValueListItem.SortAxisZPos:
			center = boundingBox.Center;
			center.Transform(Transform.Translation(0.0, 0.0, boundingBox.Max.Z - center.Z));
			pointStore = boundingBox.ClosestPoint(center);
			break;
		case ValueListItem.SortAxisZNeg:
			center = boundingBox.Center;
			center.Transform(Transform.Translation(0.0, 0.0, boundingBox.Min.Z - center.Z));
			pointStore = boundingBox.ClosestPoint(center);
			break;
		default:
			pointStore = center;
			break;
		}
		OutPoint.SetData(new CSPointData(pointStore));
		return base.Update();
	}
}
