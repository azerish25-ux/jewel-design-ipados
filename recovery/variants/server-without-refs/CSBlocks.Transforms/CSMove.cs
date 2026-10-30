using CS.CSDataUtilities;
using CS.CSObjects.CSBlocks;
using CS.Parametric;
using CS.Parametric.Channels;
using CSDatas;
using Rhino.Geometry;

namespace CSBlocks.Transforms;

[CSBlock("Move", "{F8924313-7F66-4082-9216-C6F683138A2D}")]
public sealed class CSMove : CSBlock
{
	public readonly CSInputChannel iObject;

	public readonly CSInputChannel iSource;

	public readonly CSInputChannel iDestination;

	public readonly CSOutputChannel oObject;

	public CSMove(string identifier)
		: base(identifier)
	{
		iObject = new CSInputChannel(this, ChannelDataType.Any, ChannelArrayType.Any);
		iSource = new CSInputChannel(this, ChannelDataType.Point, ChannelArrayType.Individual);
		iDestination = new CSInputChannel(this, ChannelDataType.Point, ChannelArrayType.Individual);
		oObject = new CSOutputChannel(this, ChannelDataType.Any, ChannelArrayType.Any);
		SetChannelIdentifiers();
	}

	public override bool Update()
	{
		//IL_001d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0033: Unknown result type (might be due to invalid IL or missing references)
		//IL_0047: Unknown result type (might be due to invalid IL or missing references)
		//IL_0048: Unknown result type (might be due to invalid IL or missing references)
		//IL_0049: Unknown result type (might be due to invalid IL or missing references)
		//IL_004e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0053: Unknown result type (might be due to invalid IL or missing references)
		//IL_0058: Unknown result type (might be due to invalid IL or missing references)
		//IL_0063: Unknown result type (might be due to invalid IL or missing references)
		CSData data = iObject.Data;
		if (data == null)
		{
			return UpdateMissing("Missing Object");
		}
		Point3d value = default(Point3d);
		CSDataHelper.GetPoint(iSource, out value);
		Point3d value2 = default(Point3d);
		CSDataHelper.GetPoint(iDestination, out value2);
		Transform transform = Transform.Translation(new Vector3d(value2 - value));
		CSData data2 = data.Duplicate();
		CSDataHelper.Transform(data2, transform);
		oObject.SetData(data2);
		return base.Update();
	}
}
