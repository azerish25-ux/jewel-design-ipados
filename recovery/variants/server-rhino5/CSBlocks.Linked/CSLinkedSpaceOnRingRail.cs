using CS.CSDataUtilities;
using CS.CSObjects.CSBlocks;
using CS.Parametric;
using CS.Parametric.Channels;
using CSBlockUtil;
using CSDatas;
using Rhino.Geometry;

namespace CSBlocks.Linked;

[CSBlock("LinkedSpaceOnRingRail", "{23C6F7FD-E44C-4B5A-93CC-D273D098A470}")]
public sealed class CSLinkedSpaceOnRingRail : CSBlock
{
	public readonly CSInputChannel InRail;

	public readonly CSInputChannel InAOffset;

	public readonly CSInputChannel InLastRightRailPoint;

	public readonly CSInputChannel InLastRightRailVector;

	public readonly CSOutputChannel OutRightRailPoint;

	public readonly CSOutputChannel OutRightRailVector;

	public CSLinkedSpaceOnRingRail(string identifier)
		: base(identifier)
	{
		InRail = new CSInputChannel(this, ChannelDataType.Curve, ChannelArrayType.Individual);
		InAOffset = new CSInputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual, 0.0);
		InLastRightRailPoint = new CSInputChannel(this, ChannelDataType.Point, ChannelArrayType.Individual, Point3d.Origin);
		InLastRightRailVector = new CSInputChannel(this, ChannelDataType.Vector, ChannelArrayType.Individual);
		OutRightRailPoint = new CSOutputChannel("RightRailPoint", this, ChannelDataType.Point, ChannelArrayType.Individual);
		OutRightRailVector = new CSOutputChannel("RightRailVector", this, ChannelDataType.Vector, ChannelArrayType.Individual);
		SetChannelIdentifiers();
	}

	public override bool Update()
	{
		NurbsCurve nurbsCurve = InRail.NurbsCurve;
		if (nurbsCurve == null)
		{
			return UpdateMissing("Rail Input Missing");
		}
		double num = InAOffset.Double;
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
		Point3d point3d2 = new Point3d(0.0, 0.0, 0.0);
		Point3d point3d3 = new Point3d(0.0, 1.0, 0.0);
		Point3d point3d4 = new Point3d(0.0, 0.0, 0.0);
		Point3d point3d5 = new Point3d(0.0, 1.0, 0.0);
		Transform x = default(Transform);
		CSUtilGeometry.OrientProfileToCurveNalp(nurbsCurve, 0.0, ref x, 0.0);
		point3d2.Transform(x);
		point3d3.Transform(x);
		point3d4.Transform(x);
		point3d5.Transform(x);
		Vector3d inputVC = point3d3 - point3d2;
		inputVC.Unitize();
		Transform transform = CSBlockUtil.CSUtilMath.Rotation(num / new Vector3d(point3d2).Length, new Vector3d(0.0, 1.0, 0.0), new Point3d(0.0, 0.0, 0.0));
		Transform xform;
		if (flag)
		{
			double length = new Vector3d(point3d).Length;
			Point3d intersectPoint = default(Point3d);
			CSBlockUtil.CSUtilMath.IntersectLineAndCircle(point3d2, inputVC, length, ref intersectPoint);
			xform = CSBlockUtil.CSUtilMath.Orient3Point(new Point3d(0.0, 0.0, 0.0), intersectPoint, new Point3d(0.0, 1.0, 0.0), new Point3d(0.0, 0.0, 0.0), point3d, new Point3d(0.0, 1.0, 0.0));
		}
		else
		{
			xform = Transform.Identity;
		}
		xform *= transform;
		point3d4.Transform(xform);
		point3d5.Transform(xform);
		Vector3d vectorStore = point3d5 - point3d4;
		vectorStore.Unitize();
		OutRightRailPoint.SetData(new CSPointData(point3d4));
		OutRightRailVector.SetData(new CSVectorData(vectorStore));
		return base.Update();
	}
}
