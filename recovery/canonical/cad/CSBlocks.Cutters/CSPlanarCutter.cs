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
		Vector3d normal;
		switch (InNormalMode.ValueListItem)
		{
		case ValueListItem.PlanarCutterNormalModePosX:
			normal = Vector3d.XAxis;
			break;
		case ValueListItem.PlanarCutterNormalModeNegX:
			normal = Vector3d.XAxis * -1.0;
			break;
		case ValueListItem.PlanarCutterNormalModePosY:
			normal = Vector3d.YAxis;
			break;
		case ValueListItem.PlanarCutterNormalModeNegY:
			normal = Vector3d.YAxis * -1.0;
			break;
		case ValueListItem.PlanarCutterNormalModePosZ:
			normal = Vector3d.ZAxis;
			break;
		case ValueListItem.PlanarCutterNormalModeNegZ:
			normal = Vector3d.ZAxis * -1.0;
			break;
		default:
			return UpdateError("Invalid mode");
		}
		Interval interval = new Interval(-200.0, 200.0);
		PlaneSurface geometry = new PlaneSurface(new Plane(Point3d.Origin, normal), interval, interval);
		OutCutter.SetData(new CSOnGeometryData(geometry));
		return base.Update();
	}
}
