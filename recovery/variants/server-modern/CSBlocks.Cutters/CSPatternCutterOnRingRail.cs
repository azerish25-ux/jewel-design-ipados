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
		//IL_005b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0060: Unknown result type (might be due to invalid IL or missing references)
		//IL_0065: Unknown result type (might be due to invalid IL or missing references)
		//IL_006a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0079: Unknown result type (might be due to invalid IL or missing references)
		//IL_007e: Unknown result type (might be due to invalid IL or missing references)
		//IL_008a: Unknown result type (might be due to invalid IL or missing references)
		//IL_009c: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a1: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a4: Unknown result type (might be due to invalid IL or missing references)
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
		BoundingBox boundingBox = ((GeometryBase)nurbsCurve).GetBoundingBox(true);
		Point3d max = ((BoundingBox)(ref boundingBox)).Max;
		double num3 = num2 / ((Point3d)(ref max)).X;
		max = ((BoundingBox)(ref boundingBox)).Max;
		double num4 = num / ((Point3d)(ref max)).Y;
		Transform val = Transform.Scale(Plane.WorldXY, num3, num4, 1.0);
		((GeometryBase)nurbsCurve).Transform(val);
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
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_0006: Unknown result type (might be due to invalid IL or missing references)
		//IL_0009: Unknown result type (might be due to invalid IL or missing references)
		//IL_0017: Unknown result type (might be due to invalid IL or missing references)
		//IL_0018: Unknown result type (might be due to invalid IL or missing references)
		//IL_001d: Unknown result type (might be due to invalid IL or missing references)
		//IL_002c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0031: Unknown result type (might be due to invalid IL or missing references)
		//IL_0036: Unknown result type (might be due to invalid IL or missing references)
		//IL_003b: Unknown result type (might be due to invalid IL or missing references)
		//IL_004a: Unknown result type (might be due to invalid IL or missing references)
		//IL_004f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0054: Unknown result type (might be due to invalid IL or missing references)
		//IL_0059: Unknown result type (might be due to invalid IL or missing references)
		//IL_005c: Unknown result type (might be due to invalid IL or missing references)
		//IL_005e: Unknown result type (might be due to invalid IL or missing references)
		//IL_005f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0064: Unknown result type (might be due to invalid IL or missing references)
		//IL_0065: Unknown result type (might be due to invalid IL or missing references)
		//IL_0070: Unknown result type (might be due to invalid IL or missing references)
		//IL_0075: Unknown result type (might be due to invalid IL or missing references)
		//IL_0083: Unknown result type (might be due to invalid IL or missing references)
		//IL_0088: Unknown result type (might be due to invalid IL or missing references)
		//IL_008f: Expected O, but got Unknown
		//IL_009c: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a1: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a3: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c8: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e8: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ed: Unknown result type (might be due to invalid IL or missing references)
		Point3d pointAtStart = ((Curve)profile).PointAtStart;
		Vector3d val = default(Vector3d);
		((Vector3d)(ref val))._002Ector(pointAtStart);
		((Vector3d)(ref val)).Reverse();
		Transform val2 = Transform.Translation(val);
		Transform val3 = Transform.Rotation(CSBlockUtil.CSUtilMath.ToRadians(90.0), Vector3d.XAxis, Point3d.Origin);
		Transform val4 = Transform.Rotation(CSBlockUtil.CSUtilMath.ToRadians(90.0), Vector3d.ZAxis, Point3d.Origin);
		((GeometryBase)profile).Transform(val4 * val3 * val2);
		ArcCurve val5 = new ArcCurve(new Circle(Plane.WorldXY, Point3d.Origin, 1.0));
		val2 = Transform.Translation(0.0, centerSpacing, railRadius + zOffset);
		((GeometryBase)profile).Transform(val2);
		NurbsSurface geometry = NurbsSurface.CreateRailRevolvedSurface((Curve)(object)profile, (Curve)(object)val5, new Line(new Point3d(0.0, 0.0, -1.0), new Point3d(0.0, 0.0, 1.0)), false);
		outputSurface = new CSOnGeometryData((GeometryBase)(object)geometry);
		return true;
	}
}
