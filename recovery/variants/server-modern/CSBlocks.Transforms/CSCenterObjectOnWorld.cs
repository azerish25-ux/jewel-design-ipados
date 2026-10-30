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
		//IL_001b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0020: Unknown result type (might be due to invalid IL or missing references)
		//IL_0025: Unknown result type (might be due to invalid IL or missing references)
		//IL_003f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0044: Unknown result type (might be due to invalid IL or missing references)
		//IL_0050: Unknown result type (might be due to invalid IL or missing references)
		//IL_0055: Unknown result type (might be due to invalid IL or missing references)
		//IL_0061: Unknown result type (might be due to invalid IL or missing references)
		//IL_0066: Unknown result type (might be due to invalid IL or missing references)
		//IL_0070: Unknown result type (might be due to invalid IL or missing references)
		//IL_0075: Unknown result type (might be due to invalid IL or missing references)
		//IL_007e: Unknown result type (might be due to invalid IL or missing references)
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
		Point3d center = ((BoundingBox)(ref boundingBox)).Center;
		double num = 0.0 - ((Point3d)(ref center)).X;
		center = ((BoundingBox)(ref boundingBox)).Center;
		double num2 = 0.0 - ((Point3d)(ref center)).Y;
		center = ((BoundingBox)(ref boundingBox)).Center;
		Transform transform = Transform.Translation(num, num2, 0.0 - ((Point3d)(ref center)).Z);
		CSData data2 = data.Duplicate();
		CSDataHelper.Transform(data2, transform);
		OutObject.SetData(data2);
		return base.Update();
	}
}
