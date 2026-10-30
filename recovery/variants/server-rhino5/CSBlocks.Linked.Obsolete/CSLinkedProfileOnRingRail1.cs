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
[CSBlock("LinkedProfileOnRingRail_V1", "{77268A3B-891E-4e8e-9BD3-8A4760F853B3}")]
public sealed class CSLinkedProfileOnRingRail1 : CSBlock
{
	public readonly CSInputChannel iRail;

	public readonly CSInputChannel iProfile;

	public readonly CSInputChannel iProfileX;

	public readonly CSInputChannel iProfileY;

	public readonly CSInputChannel iAOffset;

	public readonly CSInputChannel iLastRightRailPoint;

	public readonly CSInputChannel iLastRightRailVector;

	public readonly CSOutputChannel oProfile;

	public readonly CSOutputChannel oRightRailPoint;

	public readonly CSOutputChannel oRightRailVector;

	public CSLinkedProfileOnRingRail1(string identifier)
		: base(identifier)
	{
		iRail = new CSInputChannel(this, ChannelDataType.Curve, ChannelArrayType.Individual);
		iProfile = new CSInputChannel(this, ChannelDataType.Curve, ChannelArrayType.Individual);
		iProfileX = new CSInputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual);
		iProfileY = new CSInputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual);
		iAOffset = new CSInputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual);
		iLastRightRailPoint = new CSInputChannel(this, ChannelDataType.Point, ChannelArrayType.Individual);
		iLastRightRailVector = new CSInputChannel(this, ChannelDataType.Vector, ChannelArrayType.Individual);
		oProfile = new CSOutputChannel(this, ChannelDataType.Curve, ChannelArrayType.Individual);
		oRightRailPoint = new CSOutputChannel("RightRailPoint", this, ChannelDataType.Point, ChannelArrayType.Individual);
		oRightRailVector = new CSOutputChannel("RightRailVector", this, ChannelDataType.Vector, ChannelArrayType.Individual);
		SetChannelIdentifiers();
	}

	public override bool Update()
	{
		CSDataHelper.GetNurbsCurve(iRail.Data, out var value);
		if (value == null)
		{
			return UpdateMissing("Rail Input Missing");
		}
		CSDataHelper.GetNurbsCurve(iProfile.Data, out var value2);
		if (value2 == null)
		{
			return UpdateMissing("Profile Missing");
		}
		double value3 = 0.0;
		iProfileX.GetDouble(ref value3);
		double value4 = 0.0;
		iProfileY.GetDouble(ref value4);
		double value5 = 0.0;
		iAOffset.GetDouble(ref value5);
		bool flag = true;
		Point3d value6 = Point3d.Unset;
		CSDataHelper.GetPoint(iLastRightRailPoint.Data, out value6);
		if (value6 == Point3d.Unset)
		{
			flag = false;
		}
		Vector3d value7 = Vector3d.Unset;
		CSDataHelper.GetVector(iLastRightRailVector.Data, out value7);
		BoundingBox boundingBox = value2.GetBoundingBox(accurate: true);
		Transform transform = Transform.Scale(Plane.WorldXY, value3 / boundingBox.Diagonal.X, value4 / boundingBox.Diagonal.Y, 0.0);
		Point3d point3d = new Point3d(0.0, 0.0, 0.0);
		Point3d point3d2 = new Point3d(0.0, 1.0, 0.0);
		Point3d point3d3 = new Point3d(0.0, 0.0, 0.0);
		Point3d point3d4 = new Point3d(0.0, 1.0, 0.0);
		Transform x = default(Transform);
		CSUtilGeometry.OrientProfileToCurveNalp(value, 0.0, ref x, 0.0);
		point3d.Transform(x);
		point3d2.Transform(x);
		point3d3.Transform(x);
		point3d4.Transform(x);
		Vector3d vector3d = default(Vector3d);
		vector3d = point3d2 - point3d;
		vector3d.Unitize();
		Transform transform2 = CSBlockUtil.CSUtilMath.Rotation(value5 / new Vector3d(point3d).Length, Vector3d.YAxis, Point3d.Origin);
		Transform transform3;
		if (flag)
		{
			double length = new Vector3d(value6).Length;
			Point3d intersectPoint = default(Point3d);
			CSBlockUtil.CSUtilMath.IntersectLineAndCircle(point3d, vector3d, length, ref intersectPoint);
			transform3 = CSBlockUtil.CSUtilMath.Orient3Point_BackwardCompatibility(Point3d.Origin, intersectPoint, new Point3d(0.0, 1.0, 0.0), Point3d.Origin, value6, new Point3d(0.0, 1.0, 0.0));
		}
		else
		{
			transform3 = Transform.Identity;
		}
		transform3 *= transform2;
		point3d3.Transform(transform3);
		point3d4.Transform(transform3);
		Vector3d vector3d2 = default(Vector3d);
		vector3d2 = point3d4 - point3d3;
		vector3d2.Unitize();
		CSData data = iProfile.Data.Duplicate();
		CSDataHelper.Transform(data, transform3 * x * transform);
		oProfile.SetData(data);
		oRightRailPoint.SetData(new CSPointData(point3d3));
		oRightRailVector.SetData(new CSVectorData(vector3d2));
		return base.Update();
	}
}
