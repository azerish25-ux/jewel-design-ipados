using CS.CSDataUtilities;
using CS.CSObjects.CSBlocks;
using CS.Parametric;
using CS.Parametric.Channels;
using CSBlockUtil;
using CSDatas;
using Rhino.Geometry;

namespace CSBlocks.Linked;

[CSBlock("LinkedCollarOnRingRail", "{55488183-91D6-4BD7-B167-923291361BEF}")]
public sealed class CSLinkedCollarOnRingRail : CSBlock
{
	public readonly CSInputChannel InRail;

	public readonly CSInputChannel InProfile;

	public readonly CSInputChannel InCollarX;

	public readonly CSInputChannel InCollarY;

	public readonly CSInputChannel InCollarZ;

	public readonly CSInputChannel InCollarDome;

	public readonly CSInputChannel InAOffset;

	public readonly CSInputChannel InLastRightRailPoint;

	public readonly CSInputChannel InLastRightRailVector;

	public readonly CSOutputChannel OutCollar;

	public readonly CSOutputChannel OutRightRailPoint;

	public readonly CSOutputChannel OutRightRailVector;

	public CSLinkedCollarOnRingRail(string identifier)
		: base(identifier)
	{
		InRail = new CSInputChannel(this, ChannelDataType.Curve, ChannelArrayType.Individual);
		InProfile = new CSInputChannel(this, ChannelDataType.Curve, ChannelArrayType.Individual);
		InCollarX = new CSInputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual, 0.0);
		InCollarY = new CSInputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual, 0.0);
		InCollarZ = new CSInputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual, 0.0);
		InCollarDome = new CSInputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual, 0.0);
		InAOffset = new CSInputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual, 0.0);
		InLastRightRailPoint = new CSInputChannel(this, ChannelDataType.Point, ChannelArrayType.Individual, Point3d.Unset);
		InLastRightRailVector = new CSInputChannel(this, ChannelDataType.Vector, ChannelArrayType.Individual);
		OutCollar = new CSOutputChannel(this, ChannelDataType.Brep, ChannelArrayType.Individual);
		OutRightRailPoint = new CSOutputChannel("RightRailPoint", this, ChannelDataType.Point, ChannelArrayType.Individual);
		OutRightRailVector = new CSOutputChannel("RightRailVector", this, ChannelDataType.Vector, ChannelArrayType.Individual);
		SetChannelIdentifiers(autoCheckInputs: true);
	}

	public override bool Update()
	{
		NurbsCurve nurbsCurve = InRail.NurbsCurve;
		if (nurbsCurve == null)
		{
			return UpdateMissing("Rail Input Missing");
		}
		NurbsCurve nurbsCurve2 = InProfile.NurbsCurve;
		if (nurbsCurve2 == null)
		{
			return UpdateMissing("Profile Missing");
		}
		double num = InCollarX.Double;
		double num2 = InCollarY.Double;
		double num3 = InCollarZ.Double;
		double num4 = InCollarDome.Double;
		double num5 = InAOffset.Double;
		bool flag = true;
		Point3d point3d = InLastRightRailPoint.Point3d;
		if (point3d == Point3d.Unset)
		{
			flag = false;
		}
		CSDataHelper.GetVector(InLastRightRailVector.Data, out var value);
		if (value == Vector3d.Unset)
		{
			flag = false;
		}
		BoundingBox boundingBox = nurbsCurve2.GetBoundingBox(accurate: true);
		Transform transform = Transform.Translation(0.0 - boundingBox.Center.X, 0.0 - boundingBox.Center.Y, 0.0);
		Transform transform2 = Transform.Scale(Plane.WorldXY, num / boundingBox.Diagonal.X, num2 / boundingBox.Diagonal.Y, 0.0);
		nurbsCurve2.Transform(transform2 * transform);
		Transform xform = Transform.Translation(0.0, 0.0, num3 / 2.0);
		NurbsCurve nurbsCurve3 = CSUtilGeometry.CreateDomeRC(num4);
		nurbsCurve3.Transform(xform);
		Transform xform2 = Transform.Scale(Plane.WorldXY, 1.0, 1.0, -1.0);
		NurbsCurve nurbsCurve4 = nurbsCurve3.DuplicateCurve().ToNurbsCurve();
		nurbsCurve4.Transform(xform2);
		LineCurve lineCurve = new LineCurve(nurbsCurve3.PointAtStart, nurbsCurve4.PointAtStart);
		Curve[] array = Curve.JoinCurves(new Curve[3] { nurbsCurve3, lineCurve, nurbsCurve4 }, 0.01);
		if (array == null)
		{
			return UpdateError("Error Merging Curves");
		}
		NurbsCurve inprofile = array[0].ToNurbsCurve();
		xform = Transform.Translation(0.0, num2 / 2.0, 0.0);
		NurbsSurface nurbsSurface = CSUtilRailRevolve.RailRevolve(inprofile, nurbsCurve2, 0.5, 0);
		nurbsSurface.Transform(xform);
		Point3d point3d2 = new Point3d(0.0, 0.0, (0.0 - num3) / 2.0 - num4);
		Point3d point3d3 = new Point3d(0.0, 1.0, (0.0 - num3) / 2.0 - num4);
		Point3d point3d4 = new Point3d(0.0, 0.0, num3 / 2.0 + num4);
		Point3d point3d5 = new Point3d(0.0, 1.0, num3 / 2.0 + num4);
		Transform x = default(Transform);
		CSUtilGeometry.OrientProfileToCurveNalp(nurbsCurve, 0.0, ref x, 0.0);
		nurbsSurface.Transform(x);
		point3d2.Transform(x);
		point3d3.Transform(x);
		point3d4.Transform(x);
		point3d5.Transform(x);
		Vector3d inputVC = point3d3 - point3d2;
		inputVC.Unitize();
		Transform transform3 = CSBlockUtil.CSUtilMath.Rotation(num5 / new Vector3d(point3d2).Length, new Vector3d(0.0, 1.0, 0.0), new Point3d(0.0, 0.0, 0.0));
		Transform transform4;
		if (flag)
		{
			double length = new Vector3d(point3d).Length;
			Point3d intersectPoint = default(Point3d);
			CSBlockUtil.CSUtilMath.IntersectLineAndCircle(point3d2, inputVC, length, ref intersectPoint);
			transform4 = CSBlockUtil.CSUtilMath.Orient3Point(new Point3d(0.0, 0.0, 0.0), intersectPoint, new Point3d(0.0, 1.0, 0.0), new Point3d(0.0, 0.0, 0.0), point3d, new Point3d(0.0, 1.0, 0.0));
		}
		else
		{
			transform4 = Transform.Identity;
		}
		Transform transform5 = transform4 * transform3;
		nurbsSurface.Transform(transform5);
		point3d4.Transform(transform5);
		point3d5.Transform(transform5);
		Vector3d vectorStore = point3d5 - point3d4;
		vectorStore.Unitize();
		OutCollar.SetData(new CSOnGeometryData(nurbsSurface.ToBrep(), transform5, applyTransform: false));
		OutRightRailPoint.SetData(new CSPointData(point3d4));
		OutRightRailVector.SetData(new CSVectorData(vectorStore));
		return base.Update();
	}
}
