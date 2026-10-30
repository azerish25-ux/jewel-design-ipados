using CS.CSDataUtilities;
using CS.CSObjects.CSBlocks;
using CS.Parametric;
using CS.Parametric.Channels;
using CSBlockUtil;
using CSDatas;
using Rhino.Geometry;

namespace CSBlocks.Linked;

[CSBlock("LinkedProfileOnRingRail", "{C82BFF71-4BD2-451F-A6EF-CFA51BCEB1F4}")]
public sealed class CSLinkedProfileOnRingRail : CSBlock
{
	public readonly CSInputChannel InRail;

	public readonly CSInputChannel InAOffset;

	public readonly CSInputChannel InLastRightRailPoint;

	public readonly CSInputChannel InLastRightRailVector;

	public readonly CSOutputChannel OutXform;

	public readonly CSOutputChannel OutParam;

	public readonly CSOutputChannel OutRightRailPoint;

	public readonly CSOutputChannel OutRightRailVector;

	public CSLinkedProfileOnRingRail(string identifier)
		: base(identifier)
	{
		InRail = new CSInputChannel(this, ChannelDataType.Curve, ChannelArrayType.Individual);
		InAOffset = new CSInputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual, 0.0);
		InLastRightRailPoint = new CSInputChannel(this, ChannelDataType.Point, ChannelArrayType.Individual, Point3d.Unset);
		InLastRightRailVector = new CSInputChannel(this, ChannelDataType.Vector, ChannelArrayType.Individual);
		OutXform = new CSOutputChannel(this, ChannelDataType.Transform, ChannelArrayType.Individual);
		OutParam = new CSOutputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual);
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
		Point3d point3d2 = new Point3d(Point3d.Origin);
		Point3d point3d3 = new Point3d(Vector3d.YAxis);
		Point3d point3d4 = new Point3d(Point3d.Origin);
		Point3d point3d5 = new Point3d(Vector3d.YAxis);
		Transform x = default(Transform);
		CSUtilGeometry.OrientProfileToCurveNalp(nurbsCurve, 0.0, ref x, 0.0);
		point3d2.Transform(x);
		point3d3.Transform(x);
		point3d4.Transform(x);
		point3d5.Transform(x);
		Vector3d inputVC = point3d3 - point3d2;
		inputVC.Unitize();
		Transform transform = CSBlockUtil.CSUtilMath.Rotation(num / new Vector3d(point3d2).Length, Vector3d.YAxis, Point3d.Origin);
		Transform transform2;
		if (flag)
		{
			double length = new Vector3d(point3d).Length;
			Point3d intersectPoint = default(Point3d);
			CSBlockUtil.CSUtilMath.IntersectLineAndCircle(point3d2, inputVC, length, ref intersectPoint);
			transform2 = CSBlockUtil.CSUtilMath.Orient3Point(Point3d.Origin, intersectPoint, new Point3d(0.0, 1.0, 0.0), Point3d.Origin, point3d, new Point3d(0.0, 1.0, 0.0));
		}
		else
		{
			transform2 = Transform.Identity;
		}
		transform2 *= transform;
		point3d4.Transform(transform2);
		point3d5.Transform(transform2);
		Vector3d vectorStore = point3d5 - point3d4;
		vectorStore.Unitize();
		nurbsCurve.ClosestPoint(point3d4, out var t);
		OutXform.SetData(new CSXformData(transform2 * x));
		OutParam.SetData(new CSNumberData(t));
		OutRightRailPoint.SetData(new CSPointData(point3d4));
		OutRightRailVector.SetData(new CSVectorData(vectorStore));
		return base.Update();
	}
}
