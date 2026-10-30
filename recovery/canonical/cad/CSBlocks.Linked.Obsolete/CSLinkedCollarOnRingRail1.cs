using System;
using CS.CSDataUtilities;
using CS.CSObjects.CSBlocks;
using CS.Parametric;
using CS.Parametric.Channels;
using CSBlockUtil;
using CSDatas;
using Rhino.Geometry;

namespace CSBlocks.Linked.Obsolete;

[Obsolete]
[CSBlock("LinkedCollarOnRingRail_V1", "{2F80F6CF-4CBC-4782-B387-87EA0C73DA70}")]
public sealed class CSLinkedCollarOnRingRail1 : CSBlock
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

	public CSLinkedCollarOnRingRail1(string identifier)
		: base(identifier)
	{
		InRail = new CSInputChannel(this, ChannelDataType.Curve, ChannelArrayType.Individual);
		InProfile = new CSInputChannel(this, ChannelDataType.Curve, ChannelArrayType.Individual);
		InCollarX = new CSInputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual);
		InCollarY = new CSInputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual);
		InCollarZ = new CSInputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual);
		InCollarDome = new CSInputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual);
		InAOffset = new CSInputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual);
		InLastRightRailPoint = new CSInputChannel(this, ChannelDataType.Point, ChannelArrayType.Individual);
		InLastRightRailVector = new CSInputChannel(this, ChannelDataType.Vector, ChannelArrayType.Individual);
		OutCollar = new CSOutputChannel(this, ChannelDataType.Brep, ChannelArrayType.Individual);
		OutRightRailPoint = new CSOutputChannel("RightRailPoint", this, ChannelDataType.Point, ChannelArrayType.Individual);
		OutRightRailVector = new CSOutputChannel("RightRailVector", this, ChannelDataType.Vector, ChannelArrayType.Individual);
		SetChannelIdentifiers();
	}

	public override bool Update()
	{
		CSDataHelper.GetNurbsCurve(InRail.Data, out var value);
		if (value == null)
		{
			return UpdateMissing("Rail Input Missing");
		}
		CSDataHelper.GetNurbsCurve(InProfile.Data, out var value2);
		if (value2 == null)
		{
			return UpdateMissing("Profile Missing");
		}
		double value3 = 0.0;
		InCollarX.GetDouble(ref value3);
		double value4 = 0.0;
		InCollarY.GetDouble(ref value4);
		double value5 = 0.0;
		InCollarZ.GetDouble(ref value5);
		double value6 = 0.0;
		InCollarDome.GetDouble(ref value6);
		double value7 = 0.0;
		InAOffset.GetDouble(ref value7);
		bool flag = true;
		CSDataHelper.GetPoint(InLastRightRailPoint, out var value8);
		if (value8 == Point3d.Unset)
		{
			flag = false;
		}
		CSDataHelper.GetVector(InLastRightRailVector.Data, out var value9);
		if (value9 == Vector3d.Unset)
		{
			flag = false;
		}
		BoundingBox boundingBox = value2.GetBoundingBox(accurate: true);
		Transform transform = Transform.Translation(0.0 - boundingBox.Center.X, 0.0 - boundingBox.Center.Y, 0.0);
		Transform transform2 = Transform.Scale(Plane.WorldXY, value3 / boundingBox.Diagonal.X, value4 / boundingBox.Diagonal.Y, 0.0);
		value2.Transform(transform2 * transform);
		Transform xform = Transform.Translation(0.0, 0.0, value5 / 2.0);
		NurbsCurve nurbsCurve = CSUtilGeometry.CreateDomeRC(value6);
		nurbsCurve.Transform(xform);
		Transform xform2 = Transform.Scale(Plane.WorldXY, 1.0, 1.0, -1.0);
		NurbsCurve nurbsCurve2 = nurbsCurve.DuplicateCurve().ToNurbsCurve();
		nurbsCurve2.Transform(xform2);
		LineCurve lineCurve = new LineCurve(nurbsCurve.PointAtStart, nurbsCurve2.PointAtStart);
		Curve[] array = Curve.JoinCurves(new Curve[3] { nurbsCurve, lineCurve, nurbsCurve2 }, 0.01);
		if (array == null)
		{
			return UpdateError("Error Merging Curves");
		}
		NurbsCurve inprofile = array[0].ToNurbsCurve();
		xform = Transform.Translation(0.0, value4 / 2.0, 0.0);
		NurbsSurface nurbsSurface = CSUtilRailRevolve.RailRevolve(inprofile, value2, 0.5, 0);
		nurbsSurface.Transform(xform);
		Point3d point3d = new Point3d(0.0, 0.0, (0.0 - value5) / 2.0 - value6);
		Point3d point3d2 = new Point3d(0.0, 1.0, (0.0 - value5) / 2.0 - value6);
		Point3d point3d3 = new Point3d(0.0, 0.0, value5 / 2.0 + value6);
		Point3d point3d4 = new Point3d(0.0, 1.0, value5 / 2.0 + value6);
		Transform x = default(Transform);
		CSUtilGeometry.OrientProfileToCurveNalp(value, 0.0, ref x, 0.0);
		nurbsSurface.Transform(x);
		point3d.Transform(x);
		point3d2.Transform(x);
		point3d3.Transform(x);
		point3d4.Transform(x);
		Vector3d inputVC = point3d2 - point3d;
		inputVC.Unitize();
		Transform transform3 = CSBlockUtil.CSUtilMath.Rotation(value7 / new Vector3d(point3d).Length, new Vector3d(0.0, 1.0, 0.0), new Point3d(0.0, 0.0, 0.0));
		Transform transform4;
		if (flag)
		{
			double length = new Vector3d(value8).Length;
			Point3d intersectPoint = default(Point3d);
			CSBlockUtil.CSUtilMath.IntersectLineAndCircle(point3d, inputVC, length, ref intersectPoint);
			transform4 = CSBlockUtil.CSUtilMath.Orient3Point_BackwardCompatibility(new Point3d(0.0, 0.0, 0.0), intersectPoint, new Point3d(0.0, 1.0, 0.0), new Point3d(0.0, 0.0, 0.0), value8, new Point3d(0.0, 1.0, 0.0));
		}
		else
		{
			transform4 = Transform.Identity;
		}
		Transform transform5 = transform4 * transform3;
		nurbsSurface.Transform(transform5);
		point3d3.Transform(transform5);
		point3d4.Transform(transform5);
		Vector3d vectorStore = point3d4 - point3d3;
		vectorStore.Unitize();
		OutCollar.SetData(new CSOnGeometryData(nurbsSurface.ToBrep(), transform5, applyTransform: false));
		OutRightRailPoint.SetData(new CSPointData(point3d3));
		OutRightRailVector.SetData(new CSVectorData(vectorStore));
		return base.Update();
	}
}
