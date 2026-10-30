using CS.CSDataUtilities;
using CS.CSObjects.CSBlocks;
using CS.Parametric;
using CS.Parametric.Channels;
using CSDatas;
using Rhino.Geometry;

namespace CSBlocks.Transforms;

[CSBlock("CenterObjectOnWorld", "{2B56A2E2-5691-4310-8483-49DA01B37765}")]
public sealed class CSCenterObjectOnWorld : CSBlock
{
	public readonly CSInputChannel InObject;

	public readonly CSOutputChannel OutObject;

	public CSCenterObjectOnWorld(string identifier)
		: base(identifier)
	{
		InObject = new CSInputChannel(this, ChannelDataType.Any, ChannelArrayType.Any);
		OutObject = new CSOutputChannel(this, ChannelDataType.Any, ChannelArrayType.Any);
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
		if (!data.GetTightBoundingBox(ref boundingBox, doGrow: true, Transform.Identity))
		{
			return UpdateError("Bounding Box");
		}
		Transform transform = Transform.Translation(0.0 - boundingBox.Center.X, 0.0 - boundingBox.Center.Y, 0.0 - boundingBox.Center.Z);
		CSData data2 = data.Duplicate();
		CSDataHelper.Transform(data2, transform);
		OutObject.SetData(data2);
		return base.Update();
	}
}
