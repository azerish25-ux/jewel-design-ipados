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
[CSBlock("LinkedSpaceOnRingRail_V1", "{C5FDFEA3-8069-4122-99A1-992B8E2F4A97}")]
public sealed class CSLinkedSpaceOnRingRail1 : CSBlock
{
	public readonly CSInputChannel InRail;

	public readonly CSInputChannel InAOffset;

	public readonly CSInputChannel InLastRightRailPoint;

	public readonly CSInputChannel InLastRightRailVector;

	public readonly CSOutputChannel OutRightRailPoint;

	public readonly CSOutputChannel OutRightRailVector;

	public CSLinkedSpaceOnRingRail1(string identifier)
		: base(identifier)
	{
		InRail = new CSInputChannel(this, ChannelDataType.Curve, ChannelArrayType.Individual);
		InAOffset = new CSInputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual);
		InLastRightRailPoint = new CSInputChannel(this, ChannelDataType.Point, ChannelArrayType.Individual);
		InLastRightRailVector = new CSInputChannel(this, ChannelDataType.Vector, ChannelArrayType.Individual);
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
		Vector3d inputVC = point3d2 - point3d;
		inputVC.Unitize();
		Transform transform = CSBlockUtil.CSUtilMath.Rotation(value2 / new Vector3d(point3d).Length, new Vector3d(0.0, 1.0, 0.0), new Point3d(0.0, 0.0, 0.0));
		Transform xform;
		if (flag)
		{
			double length = new Vector3d(value3).Length;
			Point3d intersectPoint = default(Point3d);
			CSBlockUtil.CSUtilMath.IntersectLineAndCircle(point3d, inputVC, length, ref intersectPoint);
			xform = CSBlockUtil.CSUtilMath.Orient3Point_BackwardCompatibility(new Point3d(0.0, 0.0, 0.0), intersectPoint, new Point3d(0.0, 1.0, 0.0), new Point3d(0.0, 0.0, 0.0), value3, new Point3d(0.0, 1.0, 0.0));
		}
		else
		{
			xform = Transform.Identity;
		}
		xform *= transform;
		point3d3.Transform(xform);
		point3d4.Transform(xform);
		Vector3d vectorStore = point3d4 - point3d3;
		vectorStore.Unitize();
		OutRightRailPoint.SetData(new CSPointData(point3d3));
		OutRightRailVector.SetData(new CSVectorData(vectorStore));
		return base.Update();
	}
}
