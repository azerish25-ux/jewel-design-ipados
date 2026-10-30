using CS.CSObjects.CSBlocks;
using CS.Parametric;
using CS.Parametric.Channels;
using CSDatas;
using Rhino.Geometry;

namespace CSBlocks.Transforms;

[CSBlock("CenterObjectOnWorldTransform", "{396CBBE8-964D-4E02-926F-FC01114830CA}")]
public sealed class CSCenterObjectOnWorldTransform : CSBlock
{
	public readonly CSInputChannel InObject;

	public readonly CSOutputChannel OutTransform;

	public CSCenterObjectOnWorldTransform(string identifier)
		: base(identifier)
	{
		InObject = new CSInputChannel(this, ChannelDataType.Any, ChannelArrayType.Any);
		OutTransform = new CSOutputChannel(this, ChannelDataType.Transform, ChannelArrayType.Individual);
		SetChannelIdentifiers();
	}

	public override bool Update()
	{
		//IL_001b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0020: Unknown result type (might be due to invalid IL or missing references)
		//IL_0039: Unknown result type (might be due to invalid IL or missing references)
		//IL_003e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0049: Unknown result type (might be due to invalid IL or missing references)
		//IL_004e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0059: Unknown result type (might be due to invalid IL or missing references)
		//IL_005e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0067: Unknown result type (might be due to invalid IL or missing references)
		//IL_006c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0073: Unknown result type (might be due to invalid IL or missing references)
		CSData data = InObject.Data;
		if (data == null)
		{
			return UpdateMissing("Object to scale");
		}
		BoundingBox boundingBox = BoundingBox.Unset;
		if (!data.GetTightBoundingBox(ref boundingBox))
		{
			return UpdateError("Bounding Box");
		}
		Point3d center = ((BoundingBox)(ref boundingBox)).Center;
		double num = 0.0 - ((Point3d)(ref center)).X;
		center = ((BoundingBox)(ref boundingBox)).Center;
		double num2 = 0.0 - ((Point3d)(ref center)).Y;
		center = ((BoundingBox)(ref boundingBox)).Center;
		Transform transform = Transform.Translation(num, num2, 0.0 - ((Point3d)(ref center)).Z);
		OutTransform.SetData(new CSXformData(transform));
		return base.Update();
	}
}
