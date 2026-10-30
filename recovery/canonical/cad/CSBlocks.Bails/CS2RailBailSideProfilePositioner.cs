using CS.CSObjects.CSBlocks;
using CS.Parametric;
using CS.Parametric.Channels;
using CSBlockUtil;
using CSDatas;
using Rhino.Geometry;

namespace CSBlocks.Bails;

[CSBlock("2RailBailSideProfilePositioner", "{AF8FC9F8-5C64-414a-83E5-6F818049491B}")]
public sealed class CS2RailBailSideProfilePositioner : CSBlock
{
	public readonly CSInputChannel InBailRail;

	public readonly CSInputChannel InSideProfile;

	public readonly CSInputChannel InDistanceBottom;

	public readonly CSInputChannel InDistanceTop;

	public readonly CSOutputChannel OutProfile;

	public CS2RailBailSideProfilePositioner(string identifier)
		: base(identifier)
	{
		InBailRail = new CSInputChannel(this, ChannelDataType.Curve, ChannelArrayType.Individual);
		InSideProfile = new CSInputChannel(this, ChannelDataType.Curve, ChannelArrayType.Individual);
		InDistanceBottom = new CSInputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual, 1);
		InDistanceTop = new CSInputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual, 1);
		OutProfile = new CSOutputChannel(this, ChannelDataType.Curve, ChannelArrayType.Individual);
		SetChannelIdentifiers(autoCheckInputs: true);
	}

	public override bool Update()
	{
		CSData data = InBailRail.Data;
		NurbsCurve nurbsCurve = InSideProfile.NurbsCurve;
		if (data == null || nurbsCurve == null)
		{
			return UpdateMissing("SideProfile Missing");
		}
		double num = InDistanceBottom.Double;
		double num2 = InDistanceTop.Double;
		BoundingBox boundingBox = BoundingBox.Unset;
		data.GetTightBoundingBox(ref boundingBox, doGrow: true, Transform.Identity);
		Point3d pointAtStart = nurbsCurve.PointAtStart;
		Point3d pointAtEnd = nurbsCurve.PointAtEnd;
		double x = boundingBox.Max.X;
		if (num < 0.0)
		{
			x = boundingBox.Min.X;
		}
		Point3d point3d = new Point3d(x + num, boundingBox.Min.Y, boundingBox.Min.Z);
		Point3d point3d2 = new Point3d(x + num2, boundingBox.Max.Y, boundingBox.Min.Z);
		double num3 = point3d.DistanceTo(point3d2);
		double num4 = pointAtStart.DistanceTo(pointAtEnd);
		Transform transform = Transform.Scale(pointAtStart, num3 / num4);
		Transform transform2 = CSBlockUtil.CSUtilMath.Orient3Point(pointAtStart, pointAtEnd - pointAtStart, Vector3d.ZAxis, point3d, point3d2 - point3d, Vector3d.ZAxis);
		nurbsCurve.Transform(transform2 * transform);
		OutProfile.SetData(new CSOnGeometryData(nurbsCurve, transform2 * transform, applyTransform: false));
		return base.Update();
	}
}
