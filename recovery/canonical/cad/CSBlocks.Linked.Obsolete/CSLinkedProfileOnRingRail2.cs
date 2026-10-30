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
[CSBlock("LinkedProfileOnRingRail_V2", "{86E5FE71-5DC5-4427-9613-11E9B93291C6}")]
public sealed class CSLinkedProfileOnRingRail2 : CSBlock
{
	public readonly CSInputChannel InRail;

	public readonly CSInputChannel InAOffset;

	public readonly CSInputChannel InLastRightRailPoint;

	public readonly CSInputChannel InLastRightRailVector;

	public readonly CSOutputChannel OutXform;

	public readonly CSOutputChannel OutParam;

	public readonly CSOutputChannel OutRightRailPoint;

	public readonly CSOutputChannel OutRightRailVector;

	public CSLinkedProfileOnRingRail2(string identifier)
		: base(identifier)
	{
		InRail = new CSInputChannel(this, ChannelDataType.Curve, ChannelArrayType.Individual);
		InAOffset = new CSInputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual);
		InLastRightRailPoint = new CSInputChannel(this, ChannelDataType.Point, ChannelArrayType.Individual);
		InLastRightRailVector = new CSInputChannel(this, ChannelDataType.Vector, ChannelArrayType.Individual);
		OutXform = new CSOutputChannel(this, ChannelDataType.Transform, ChannelArrayType.Individual);
		OutParam = new CSOutputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual);
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
		double value2 = 0.0;
		InAOffset.GetDouble(ref value2);
		bool flag = true;
		CSDataHelper.GetPoint(InLastRightRailPoint.Data, out var value3);
		if (value3 == Point3d.Unset)
		{
			flag = false;
		}
		CSDataHelper.GetVector(InLastRightRailVector.Data, out var value4);
		if (value4 == Vector3d.Unset)
		{
			flag = false;
		}
		Point3d point3d = new Point3d(Point3d.Origin);
		Point3d point3d2 = new Point3d(Vector3d.YAxis);
		Point3d point3d3 = new Point3d(Point3d.Origin);
		Point3d point3d4 = new Point3d(Vector3d.YAxis);
		Transform x = default(Transform);
		CSUtilGeometry.OrientProfileToCurveNalp(value, 0.0, ref x, 0.0);
		point3d.Transform(x);
		point3d2.Transform(x);
		point3d3.Transform(x);
		point3d4.Transform(x);
		Vector3d inputVC = point3d2 - point3d;
		inputVC.Unitize();
		Transform transform = CSBlockUtil.CSUtilMath.Rotation(value2 / new Vector3d(point3d).Length, Vector3d.YAxis, Point3d.Origin);
		Transform transform2;
		if (flag)
		{
			double length = new Vector3d(value3).Length;
			Point3d intersectPoint = default(Point3d);
			CSBlockUtil.CSUtilMath.IntersectLineAndCircle(point3d, inputVC, length, ref intersectPoint);
			transform2 = CSBlockUtil.CSUtilMath.Orient3Point_BackwardCompatibility(Point3d.Origin, intersectPoint, new Point3d(0.0, 1.0, 0.0), Point3d.Origin, value3, new Point3d(0.0, 1.0, 0.0));
		}
		else
		{
			transform2 = Transform.Identity;
		}
		transform2 *= transform;
		point3d3.Transform(transform2);
		point3d4.Transform(transform2);
		Vector3d vectorStore = point3d4 - point3d3;
		vectorStore.Unitize();
		value.ClosestPoint(point3d3, out var t);
		OutXform.SetData(new CSXformData(transform2 * x));
		OutParam.SetData(new CSNumberData(t));
		OutRightRailPoint.SetData(new CSPointData(point3d3));
		OutRightRailVector.SetData(new CSVectorData(vectorStore));
		return base.Update();
	}
}
