using System.Collections.Generic;
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
		//IL_0052: Unknown result type (might be due to invalid IL or missing references)
		//IL_0057: Unknown result type (might be due to invalid IL or missing references)
		//IL_0079: Unknown result type (might be due to invalid IL or missing references)
		//IL_007e: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a0: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a5: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c7: Unknown result type (might be due to invalid IL or missing references)
		//IL_00cc: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ee: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f3: Unknown result type (might be due to invalid IL or missing references)
		//IL_0115: Unknown result type (might be due to invalid IL or missing references)
		//IL_011a: Unknown result type (might be due to invalid IL or missing references)
		//IL_013c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0141: Unknown result type (might be due to invalid IL or missing references)
		//IL_0163: Unknown result type (might be due to invalid IL or missing references)
		//IL_0168: Unknown result type (might be due to invalid IL or missing references)
		//IL_0196: Unknown result type (might be due to invalid IL or missing references)
		//IL_019b: Unknown result type (might be due to invalid IL or missing references)
		//IL_01bd: Unknown result type (might be due to invalid IL or missing references)
		//IL_01c2: Unknown result type (might be due to invalid IL or missing references)
		//IL_01e4: Unknown result type (might be due to invalid IL or missing references)
		//IL_01e9: Unknown result type (might be due to invalid IL or missing references)
		//IL_020b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0210: Unknown result type (might be due to invalid IL or missing references)
		//IL_0232: Unknown result type (might be due to invalid IL or missing references)
		//IL_0237: Unknown result type (might be due to invalid IL or missing references)
		//IL_0259: Unknown result type (might be due to invalid IL or missing references)
		//IL_025e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0280: Unknown result type (might be due to invalid IL or missing references)
		//IL_0285: Unknown result type (might be due to invalid IL or missing references)
		//IL_02a7: Unknown result type (might be due to invalid IL or missing references)
		//IL_02ac: Unknown result type (might be due to invalid IL or missing references)
		//IL_02da: Unknown result type (might be due to invalid IL or missing references)
		//IL_02df: Unknown result type (might be due to invalid IL or missing references)
		//IL_0301: Unknown result type (might be due to invalid IL or missing references)
		//IL_0306: Unknown result type (might be due to invalid IL or missing references)
		//IL_0328: Unknown result type (might be due to invalid IL or missing references)
		//IL_032d: Unknown result type (might be due to invalid IL or missing references)
		//IL_034f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0354: Unknown result type (might be due to invalid IL or missing references)
		//IL_0376: Unknown result type (might be due to invalid IL or missing references)
		//IL_037b: Unknown result type (might be due to invalid IL or missing references)
		//IL_039d: Unknown result type (might be due to invalid IL or missing references)
		//IL_03a2: Unknown result type (might be due to invalid IL or missing references)
		//IL_03c4: Unknown result type (might be due to invalid IL or missing references)
		//IL_03c9: Unknown result type (might be due to invalid IL or missing references)
		//IL_03eb: Unknown result type (might be due to invalid IL or missing references)
		//IL_03f0: Unknown result type (might be due to invalid IL or missing references)
		//IL_041e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0423: Unknown result type (might be due to invalid IL or missing references)
		//IL_0445: Unknown result type (might be due to invalid IL or missing references)
		//IL_044a: Unknown result type (might be due to invalid IL or missing references)
		//IL_046c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0471: Unknown result type (might be due to invalid IL or missing references)
		//IL_0493: Unknown result type (might be due to invalid IL or missing references)
		//IL_0498: Unknown result type (might be due to invalid IL or missing references)
		//IL_04ba: Unknown result type (might be due to invalid IL or missing references)
		//IL_04bf: Unknown result type (might be due to invalid IL or missing references)
		//IL_04e1: Unknown result type (might be due to invalid IL or missing references)
		//IL_04e6: Unknown result type (might be due to invalid IL or missing references)
		//IL_0508: Unknown result type (might be due to invalid IL or missing references)
		//IL_050d: Unknown result type (might be due to invalid IL or missing references)
		//IL_052f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0534: Unknown result type (might be due to invalid IL or missing references)
		//IL_0562: Unknown result type (might be due to invalid IL or missing references)
		//IL_0567: Unknown result type (might be due to invalid IL or missing references)
		//IL_0589: Unknown result type (might be due to invalid IL or missing references)
		//IL_058e: Unknown result type (might be due to invalid IL or missing references)
		//IL_05b0: Unknown result type (might be due to invalid IL or missing references)
		//IL_05b5: Unknown result type (might be due to invalid IL or missing references)
		//IL_05d7: Unknown result type (might be due to invalid IL or missing references)
		//IL_05dc: Unknown result type (might be due to invalid IL or missing references)
		//IL_05fe: Unknown result type (might be due to invalid IL or missing references)
		//IL_0603: Unknown result type (might be due to invalid IL or missing references)
		//IL_0625: Unknown result type (might be due to invalid IL or missing references)
		//IL_062a: Unknown result type (might be due to invalid IL or missing references)
		//IL_064c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0651: Unknown result type (might be due to invalid IL or missing references)
		//IL_0673: Unknown result type (might be due to invalid IL or missing references)
		//IL_0678: Unknown result type (might be due to invalid IL or missing references)
		//IL_06a6: Unknown result type (might be due to invalid IL or missing references)
		//IL_06ab: Unknown result type (might be due to invalid IL or missing references)
		//IL_06cd: Unknown result type (might be due to invalid IL or missing references)
		//IL_06d2: Unknown result type (might be due to invalid IL or missing references)
		//IL_06f4: Unknown result type (might be due to invalid IL or missing references)
		//IL_06f9: Unknown result type (might be due to invalid IL or missing references)
		//IL_071b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0720: Unknown result type (might be due to invalid IL or missing references)
		//IL_0742: Unknown result type (might be due to invalid IL or missing references)
		//IL_0747: Unknown result type (might be due to invalid IL or missing references)
		//IL_0769: Unknown result type (might be due to invalid IL or missing references)
		//IL_076e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0790: Unknown result type (might be due to invalid IL or missing references)
		//IL_0795: Unknown result type (might be due to invalid IL or missing references)
		//IL_07b7: Unknown result type (might be due to invalid IL or missing references)
		//IL_07bc: Unknown result type (might be due to invalid IL or missing references)
		Point3d[] array;
		switch (InMode.Integer)
		{
		case 0:
			array = (Point3d[])(object)new Point3d[8]
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
			array = (Point3d[])(object)new Point3d[8]
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
			array = (Point3d[])(object)new Point3d[8]
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
			array = (Point3d[])(object)new Point3d[8]
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
			array = (Point3d[])(object)new Point3d[8]
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
			array = (Point3d[])(object)new Point3d[8]
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
		BoundingBox val = default(BoundingBox);
		((BoundingBox)(ref val))._002Ector((IEnumerable<Point3d>)array);
		if (!((BoundingBox)(ref val)).MakeValid())
		{
			return UpdateError("Unable to create box from bounding box corners");
		}
		Brep geometry = ((BoundingBox)(ref val)).ToBrep();
		OutCutter.SetData(new CSOnGeometryData((GeometryBase)(object)geometry));
		return base.Update();
	}
}
