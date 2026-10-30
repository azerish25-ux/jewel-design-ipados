using CS.CSObjects.CSBlocks;
using CS.Parametric;
using CS.Parametric.Channels;
using CSBlockUtil;
using CSDatas;
using Rhino.Geometry;

namespace CSBlocks.Cutters;

[CSBlock("PatternCutterOnRingRail", "{532C44A2-2D12-471b-8105-FFC0715C1479}")]
public sealed class CSPatternCutterOnRingRail : CSBlock
{
	public CSInputChannel InRailRadius { get; }

	public CSInputChannel InProfile { get; }

	public CSInputChannel InHeight { get; }

	public CSInputChannel InThickness { get; }

	public CSInputChannel InZOffset { get; }

	public CSInputChannel InCenterSpacing { get; }

	public CSOutputChannel OutSurface { get; }

	public CSPatternCutterOnRingRail(string identifier)
		: base(identifier)
	{
		InRailRadius = new CSInputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual, 0);
		InProfile = new CSInputChannel(this, ChannelDataType.Curve, ChannelArrayType.Individual);
		InHeight = new CSInputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual, 0);
		InThickness = new CSInputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual, 0);
		InZOffset = new CSInputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual, 0);
		InCenterSpacing = new CSInputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual, 0);
		OutSurface = new CSOutputChannel(this, ChannelDataType.Brep, ChannelArrayType.Individual);
		SetChannelIdentifiers(autoCheckInputs: true);
	}

	public override bool Update()
	{
		double railRadius = InRailRadius.Double;
		NurbsCurve nurbsCurve = InProfile.NurbsCurve;
		double num = InHeight.Double;
		double num2 = InThickness.Double;
		double zOffset = InZOffset.Double;
		double centerSpacing = InCenterSpacing.Double;
		if (nurbsCurve == null)
		{
			return UpdateMissing("Profile Missing");
		}
		BoundingBox boundingBox = nurbsCurve.GetBoundingBox(accurate: true);
		double xScaleFactor = num2 / boundingBox.Max.X;
		double yScaleFactor = num / boundingBox.Max.Y;
		Transform xform = Transform.Scale(Plane.WorldXY, xScaleFactor, yScaleFactor, 1.0);
		nurbsCurve.Transform(xform);
		CSOnGeometryData outputSurface = null;
		if (!PatternCutterOnRingRail(railRadius, nurbsCurve, zOffset, centerSpacing, ref outputSurface))
		{
			return UpdateError("PatternCutterOnRingRail Failed");
		}
		OutSurface.SetData(outputSurface);
		return base.Update();
	}

	private static bool PatternCutterOnRingRail(double railRadius, NurbsCurve profile, double zOffset, double centerSpacing, ref CSOnGeometryData outputSurface)
	{
		Point3d pointAtStart = profile.PointAtStart;
		Vector3d motion = new Vector3d(pointAtStart);
		motion.Reverse();
		Transform transform = Transform.Translation(motion);
		Transform transform2 = Transform.Rotation(CSBlockUtil.CSUtilMath.ToRadians(90.0), Vector3d.XAxis, Point3d.Origin);
		Transform transform3 = Transform.Rotation(CSBlockUtil.CSUtilMath.ToRadians(90.0), Vector3d.ZAxis, Point3d.Origin);
		profile.Transform(transform3 * transform2 * transform);
		ArcCurve rail = new ArcCurve(new Circle(Plane.WorldXY, Point3d.Origin, 1.0));
		transform = Transform.Translation(0.0, centerSpacing, railRadius + zOffset);
		profile.Transform(transform);
		NurbsSurface geometry = NurbsSurface.CreateRailRevolvedSurface(profile, rail, new Line(new Point3d(0.0, 0.0, -1.0), new Point3d(0.0, 0.0, 1.0)), scaleHeight: false);
		outputSurface = new CSOnGeometryData(geometry);
		return true;
	}
}
