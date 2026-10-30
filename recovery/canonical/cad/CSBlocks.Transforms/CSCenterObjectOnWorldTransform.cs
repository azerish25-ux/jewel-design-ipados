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
		Transform transform = Transform.Translation(0.0 - boundingBox.Center.X, 0.0 - boundingBox.Center.Y, 0.0 - boundingBox.Center.Z);
		OutTransform.SetData(new CSXformData(transform));
		return base.Update();
	}
}
