using CS.CSObjects.CSBlocks;
using CS.Parametric;
using CS.Parametric.Channels;
using CSDatas;
using Rhino.Geometry;

namespace CSBlocks.Cutters;

[CSBlock("BoxCutter", "{E24D1263-26E0-4A9E-924F-0F2E08A5EF8A}")]
public sealed class CSBoxCutter : CSBlock
{
	public readonly CSInputChannel InMode;

	public readonly CSOutputChannel OutCutter;

	public CSBoxCutter(string identifier)
		: base(identifier)
	{
		InMode = new CSInputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual, 0);
		OutCutter = new CSOutputChannel(this, ChannelDataType.Surface, ChannelArrayType.Individual);
		SetChannelIdentifiers(autoCheckInputs: true);
	}

	public override bool Update()
	{
		Point3d[] points;
		switch (InMode.Integer)
		{
		case 0:
			points = new Point3d[8]
			{
				new Point3d(0.0, -100.0, -100.0),
				new Point3d(200.0, -100.0, -100.0),
				new Point3d(200.0, 100.0, -100.0),
				new Point3d(0.0, 100.0, -100.0),
				new Point3d(0.0, -100.0, 100.0),
				new Point3d(200.0, -100.0, 100.0),
				new Point3d(200.0, 100.0, 100.0),
				new Point3d(0.0, 100.0, 100.0)
			};
			break;
		case 1:
			points = new Point3d[8]
			{
				new Point3d(-200.0, -100.0, -100.0),
				new Point3d(0.0, -100.0, -100.0),
				new Point3d(0.0, 100.0, -100.0),
				new Point3d(-200.0, 100.0, -100.0),
				new Point3d(-200.0, -100.0, 100.0),
				new Point3d(0.0, -100.0, 100.0),
				new Point3d(0.0, 100.0, 100.0),
				new Point3d(-200.0, 100.0, 100.0)
			};
			break;
		case 2:
			points = new Point3d[8]
			{
				new Point3d(-100.0, 0.0, -100.0),
				new Point3d(-100.0, 200.0, -100.0),
				new Point3d(100.0, 200.0, -100.0),
				new Point3d(100.0, 0.0, -100.0),
				new Point3d(-100.0, 0.0, 100.0),
				new Point3d(-100.0, 200.0, 100.0),
				new Point3d(100.0, 200.0, 100.0),
				new Point3d(100.0, 0.0, 100.0)
			};
			break;
		case 3:
			points = new Point3d[8]
			{
				new Point3d(-100.0, -200.0, -100.0),
				new Point3d(-100.0, 0.0, -100.0),
				new Point3d(100.0, 0.0, -100.0),
				new Point3d(100.0, -200.0, -100.0),
				new Point3d(-100.0, -200.0, 100.0),
				new Point3d(-100.0, 0.0, 100.0),
				new Point3d(100.0, 0.0, 100.0),
				new Point3d(100.0, -200.0, 100.0)
			};
			break;
		case 4:
			points = new Point3d[8]
			{
				new Point3d(-100.0, -100.0, 0.0),
				new Point3d(-100.0, -100.0, 200.0),
				new Point3d(100.0, -100.0, 200.0),
				new Point3d(100.0, -100.0, 0.0),
				new Point3d(-100.0, 100.0, 0.0),
				new Point3d(-100.0, 100.0, 200.0),
				new Point3d(100.0, 100.0, 200.0),
				new Point3d(100.0, 100.0, 0.0)
			};
			break;
		case 5:
			points = new Point3d[8]
			{
				new Point3d(-100.0, -100.0, -200.0),
				new Point3d(-100.0, -100.0, 0.0),
				new Point3d(100.0, -100.0, 0.0),
				new Point3d(100.0, -100.0, -200.0),
				new Point3d(-100.0, 100.0, -200.0),
				new Point3d(-100.0, 100.0, 0.0),
				new Point3d(100.0, 100.0, 0.0),
				new Point3d(100.0, 100.0, -200.0)
			};
			break;
		default:
			return UpdateError("Invalid mode");
		}
		BoundingBox boundingBox = new BoundingBox(points);
		if (!boundingBox.MakeValid())
		{
			return UpdateError("Unable to create box from bounding box corners");
		}
		Brep geometry = boundingBox.ToBrep();
		OutCutter.SetData(new CSOnGeometryData(geometry));
		return base.Update();
	}
}
