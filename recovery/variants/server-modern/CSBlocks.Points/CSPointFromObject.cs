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
		//IL_000d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0031: Unknown result type (might be due to invalid IL or missing references)
		//IL_0036: Unknown result type (might be due to invalid IL or missing references)
		//IL_006c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0071: Unknown result type (might be due to invalid IL or missing references)
		//IL_0076: Unknown result type (might be due to invalid IL or missing references)
		//IL_007b: Unknown result type (might be due to invalid IL or missing references)
		//IL_009e: Unknown result type (might be due to invalid IL or missing references)
		//IL_00aa: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ab: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b0: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ba: Unknown result type (might be due to invalid IL or missing references)
		//IL_00bf: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e2: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ee: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ef: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f4: Unknown result type (might be due to invalid IL or missing references)
		//IL_00fc: Unknown result type (might be due to invalid IL or missing references)
		//IL_0101: Unknown result type (might be due to invalid IL or missing references)
		//IL_010f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0114: Unknown result type (might be due to invalid IL or missing references)
		//IL_012e: Unknown result type (might be due to invalid IL or missing references)
		//IL_013a: Unknown result type (might be due to invalid IL or missing references)
		//IL_013b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0140: Unknown result type (might be due to invalid IL or missing references)
		//IL_0148: Unknown result type (might be due to invalid IL or missing references)
		//IL_014d: Unknown result type (might be due to invalid IL or missing references)
		//IL_015b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0160: Unknown result type (might be due to invalid IL or missing references)
		//IL_017a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0186: Unknown result type (might be due to invalid IL or missing references)
		//IL_0187: Unknown result type (might be due to invalid IL or missing references)
		//IL_018c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0194: Unknown result type (might be due to invalid IL or missing references)
		//IL_0199: Unknown result type (might be due to invalid IL or missing references)
		//IL_01b0: Unknown result type (might be due to invalid IL or missing references)
		//IL_01b5: Unknown result type (might be due to invalid IL or missing references)
		//IL_01c6: Unknown result type (might be due to invalid IL or missing references)
		//IL_01d2: Unknown result type (might be due to invalid IL or missing references)
		//IL_01d3: Unknown result type (might be due to invalid IL or missing references)
		//IL_01d8: Unknown result type (might be due to invalid IL or missing references)
		//IL_01dd: Unknown result type (might be due to invalid IL or missing references)
		//IL_01e2: Unknown result type (might be due to invalid IL or missing references)
		//IL_01f9: Unknown result type (might be due to invalid IL or missing references)
		//IL_01fe: Unknown result type (might be due to invalid IL or missing references)
		//IL_020f: Unknown result type (might be due to invalid IL or missing references)
		//IL_021b: Unknown result type (might be due to invalid IL or missing references)
		//IL_021c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0221: Unknown result type (might be due to invalid IL or missing references)
		//IL_022c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0224: Unknown result type (might be due to invalid IL or missing references)
		//IL_0225: Unknown result type (might be due to invalid IL or missing references)
		//IL_0063: Unknown result type (might be due to invalid IL or missing references)
		//IL_0064: Unknown result type (might be due to invalid IL or missing references)
		CSData data = InObject.Data;
		BoundingBox boundingBox = default(BoundingBox);
		data.GetTightBoundingBox(ref boundingBox);
		ValueListItem whichItem = ValueListItem.SortOnCenterPoint;
		InPointSelection.GetValueListItem(ref whichItem);
		Point3d center = ((BoundingBox)(ref boundingBox)).Center;
		Point3d pointStore;
		Point3d val;
		switch (whichItem)
		{
		case ValueListItem.SortOnCenterPoint:
			pointStore = center;
			break;
		case ValueListItem.SortAxisXPos:
			center = ((BoundingBox)(ref boundingBox)).Center;
			val = ((BoundingBox)(ref boundingBox)).Max;
			((Point3d)(ref center)).Transform(Transform.Translation(((Point3d)(ref val)).X - ((Point3d)(ref center)).X, 0.0, 0.0));
			pointStore = ((BoundingBox)(ref boundingBox)).ClosestPoint(center);
			break;
		case ValueListItem.SortAxisXNeg:
			val = ((BoundingBox)(ref boundingBox)).Min;
			((Point3d)(ref center)).Transform(Transform.Translation(((Point3d)(ref val)).X - ((Point3d)(ref center)).X, 0.0, 0.0));
			pointStore = ((BoundingBox)(ref boundingBox)).ClosestPoint(center);
			break;
		case ValueListItem.SortAxisYPos:
			center = ((BoundingBox)(ref boundingBox)).Center;
			val = ((BoundingBox)(ref boundingBox)).Max;
			((Point3d)(ref center)).Transform(Transform.Translation(0.0, ((Point3d)(ref val)).Y - ((Point3d)(ref center)).Y, 0.0));
			pointStore = ((BoundingBox)(ref boundingBox)).ClosestPoint(center);
			break;
		case ValueListItem.SortAxisYNeg:
			center = ((BoundingBox)(ref boundingBox)).Center;
			val = ((BoundingBox)(ref boundingBox)).Min;
			((Point3d)(ref center)).Transform(Transform.Translation(0.0, ((Point3d)(ref val)).Y - ((Point3d)(ref center)).Y, 0.0));
			pointStore = ((BoundingBox)(ref boundingBox)).ClosestPoint(center);
			break;
		case ValueListItem.SortAxisZPos:
			center = ((BoundingBox)(ref boundingBox)).Center;
			val = ((BoundingBox)(ref boundingBox)).Max;
			((Point3d)(ref center)).Transform(Transform.Translation(0.0, 0.0, ((Point3d)(ref val)).Z - ((Point3d)(ref center)).Z));
			pointStore = ((BoundingBox)(ref boundingBox)).ClosestPoint(center);
			break;
		case ValueListItem.SortAxisZNeg:
			center = ((BoundingBox)(ref boundingBox)).Center;
			val = ((BoundingBox)(ref boundingBox)).Min;
			((Point3d)(ref center)).Transform(Transform.Translation(0.0, 0.0, ((Point3d)(ref val)).Z - ((Point3d)(ref center)).Z));
			pointStore = ((BoundingBox)(ref boundingBox)).ClosestPoint(center);
			break;
		default:
			pointStore = center;
			break;
		}
		OutPoint.SetData(new CSPointData(pointStore));
		return base.Update();
	}
}
