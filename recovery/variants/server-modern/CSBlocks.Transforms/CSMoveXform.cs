using CS.CSDataUtilities;
using CS.CSObjects.CSBlocks;
using CS.Parametric;
using CS.Parametric.Channels;
using CSDatas;
using Rhino.Geometry;

namespace CSBlocks.Transforms;

[CSBlock("MoveXform", "{CEAA8189-8052-44d0-9672-68D9DC30FA42}")]
public sealed class CSMoveXform : CSBlock
{
	public readonly CSInputChannel iSource;

	public readonly CSInputChannel iDestination;

	public readonly CSOutputChannel oXform;

	public CSMoveXform(string identifier)
		: base(identifier)
	{
		iSource = new CSInputChannel(this, ChannelDataType.Point, ChannelArrayType.Individual);
		iDestination = new CSInputChannel(this, ChannelDataType.Point, ChannelArrayType.Individual);
		oXform = new CSOutputChannel(this, ChannelDataType.Transform, ChannelArrayType.Individual);
		SetChannelIdentifiers();
	}

	public override bool Update()
	{
		//IL_0002: Unknown result type (might be due to invalid IL or missing references)
		//IL_0018: Unknown result type (might be due to invalid IL or missing references)
		//IL_002c: Unknown result type (might be due to invalid IL or missing references)
		//IL_002d: Unknown result type (might be due to invalid IL or missing references)
		//IL_002e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0033: Unknown result type (might be due to invalid IL or missing references)
		//IL_0038: Unknown result type (might be due to invalid IL or missing references)
		//IL_003d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0044: Unknown result type (might be due to invalid IL or missing references)
		Point3d value = default(Point3d);
		CSDataHelper.GetPoint(iSource, out value);
		Point3d value2 = default(Point3d);
		CSDataHelper.GetPoint(iDestination, out value2);
		Transform transform = Transform.Translation(new Vector3d(value2 - value));
		oXform.SetData(new CSXformData(transform));
		return base.Update();
	}
}
