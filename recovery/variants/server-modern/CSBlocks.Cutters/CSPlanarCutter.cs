using CS.CSObjects.CSBlocks;
using CS.Parametric;
using CS.Parametric.Channels;
using CSDatas;
using GV.CounterSketch.Shared;
using Rhino.Geometry;

namespace CSBlocks.Cutters;

[CSBlock("PlanarCutter", "{EDDF22CD-BC48-4416-A8DF-6EEED10E374B}")]
public sealed class CSPlanarCutter : CSBlock
{
	public CSInputChannel InNormalMode { get; }

	public CSOutputChannel OutCutter { get; }

	public CSPlanarCutter(string identifier)
		: base(identifier)
	{
		InNormalMode = new CSInputChannel(this, ChannelDataType.Integer, ChannelArrayType.Individual, ValueListItem.PlanarCutterNormalModePosX);
		OutCutter = new CSOutputChannel(this, ChannelDataType.Surface, ChannelArrayType.Individual);
		SetChannelIdentifiers(autoCheckInputs: true);
	}

	public override bool Update()
	{
		//IL_0032: Unknown result type (might be due to invalid IL or missing references)
		//IL_0037: Unknown result type (might be due to invalid IL or missing references)
		//IL_003a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0048: Unknown result type (might be due to invalid IL or missing references)
		//IL_004d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0050: Unknown result type (might be due to invalid IL or missing references)
		//IL_0055: Unknown result type (might be due to invalid IL or missing references)
		//IL_0058: Unknown result type (might be due to invalid IL or missing references)
		//IL_0066: Unknown result type (might be due to invalid IL or missing references)
		//IL_006b: Unknown result type (might be due to invalid IL or missing references)
		//IL_006e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0073: Unknown result type (might be due to invalid IL or missing references)
		//IL_0076: Unknown result type (might be due to invalid IL or missing references)
		//IL_0084: Unknown result type (might be due to invalid IL or missing references)
		//IL_0089: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b1: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b6: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b7: Unknown result type (might be due to invalid IL or missing references)
		//IL_00bc: Unknown result type (might be due to invalid IL or missing references)
		//IL_00bd: Unknown result type (might be due to invalid IL or missing references)
		//IL_00be: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c4: Expected O, but got Unknown
		Vector3d val;
		switch (InNormalMode.ValueListItem)
		{
		case ValueListItem.PlanarCutterNormalModePosX:
			val = Vector3d.XAxis;
			break;
		case ValueListItem.PlanarCutterNormalModeNegX:
			val = Vector3d.XAxis * -1.0;
			break;
		case ValueListItem.PlanarCutterNormalModePosY:
			val = Vector3d.YAxis;
			break;
		case ValueListItem.PlanarCutterNormalModeNegY:
			val = Vector3d.YAxis * -1.0;
			break;
		case ValueListItem.PlanarCutterNormalModePosZ:
			val = Vector3d.ZAxis;
			break;
		case ValueListItem.PlanarCutterNormalModeNegZ:
			val = Vector3d.ZAxis * -1.0;
			break;
		default:
			return UpdateError("Invalid mode");
		}
		Interval val2 = default(Interval);
		((Interval)(ref val2))._002Ector(-200.0, 200.0);
		PlaneSurface geometry = new PlaneSurface(new Plane(Point3d.Origin, val), val2, val2);
		OutCutter.SetData(new CSOnGeometryData((GeometryBase)(object)geometry));
		return base.Update();
	}
}
