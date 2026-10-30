using System;
using CS;
using CS.CSDataUtilities;
using CS.CSObjects.CSBlocks;
using CS.Parametric;
using CS.Parametric.Channels;
using CSBlockUtil;
using CSDatas;
using Rhino.Geometry;
using Rhino.Geometry.Intersect;

namespace CSBlocks.Linked.Obsolete;

[Obsolete]
[CSBlock("LinkedProfileOn2RingRail_V1", "{310FF09C-A24C-447f-A169-D36556CDDBEF}")]
public sealed class CSLinkedProfileOn2RingRail1 : CSBlock
{
	public readonly CSInputChannel iRail;

	public readonly CSInputChannel iOutsideRail;

	public readonly CSInputChannel iOutsideRailScale;

	public readonly CSInputChannel iAOffset;

	public readonly CSInputChannel iLastRightRailPoint;

	public readonly CSInputChannel iLastRightRailVector;

	public readonly CSOutputChannel oXform;

	public readonly CSOutputChannel oParam;

	public readonly CSOutputChannel oParam2;

	public readonly CSOutputChannel oYSize;

	public readonly CSOutputChannel oRightRailPoint;

	public readonly CSOutputChannel oRightRailVector;

	public CSLinkedProfileOn2RingRail1(string identifier)
		: base(identifier)
	{
		iRail = new CSInputChannel(this, ChannelDataType.Curve, ChannelArrayType.Individual);
		iOutsideRail = new CSInputChannel(this, ChannelDataType.Curve, ChannelArrayType.Individual);
		iOutsideRailScale = new CSInputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual);
		iAOffset = new CSInputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual);
		iLastRightRailPoint = new CSInputChannel(this, ChannelDataType.Point, ChannelArrayType.Individual);
		iLastRightRailVector = new CSInputChannel(this, ChannelDataType.Vector, ChannelArrayType.Individual);
		oXform = new CSOutputChannel(this, ChannelDataType.Transform, ChannelArrayType.Individual);
		oParam = new CSOutputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual);
		oParam2 = new CSOutputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual);
		oYSize = new CSOutputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual);
		oRightRailPoint = new CSOutputChannel("RightRailPoint", this, ChannelDataType.Point, ChannelArrayType.Individual);
		oRightRailVector = new CSOutputChannel("RightRailVector", this, ChannelDataType.Vector, ChannelArrayType.Individual);
		SetChannelIdentifiers();
	}

	public override bool Update()
	{
		CSDataHelper.GetNurbsCurve(iRail.Data, out var value);
		if (value == null)
		{
			return UpdateMissing("Profile Placer Simple: Missing Nurbs Curve");
		}
		CSDataHelper.GetNurbsCurve(iOutsideRail.Data, out var value2);
		double value3 = 1.0;
		iOutsideRailScale.GetDouble(ref value3);
		double value4 = 0.0;
		iAOffset.GetDouble(ref value4);
		bool flag = true;
		Point3d value5 = Point3d.Unset;
		CSDataHelper.GetPoint(iLastRightRailPoint.Data, out value5);
		if (value5 == Point3d.Unset)
		{
			flag = false;
		}
		Vector3d value6 = Vector3d.Unset;
		CSDataHelper.GetVector(iLastRightRailVector.Data, out value6);
		if (value6 == Vector3d.Unset)
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
		Vector3d vector3d = default(Vector3d);
		vector3d = point3d2 - point3d;
		vector3d.Unitize();
		Transform transform = CSBlockUtil.CSUtilMath.Rotation(value4 / new Vector3d(point3d).Length, Vector3d.YAxis, Point3d.Origin);
		Transform transform2;
		if (flag)
		{
			double length = new Vector3d(value5).Length;
			Point3d intersectPoint = default(Point3d);
			CSBlockUtil.CSUtilMath.IntersectLineAndCircle(point3d, vector3d, length, ref intersectPoint);
			transform2 = CSBlockUtil.CSUtilMath.Orient3Point_BackwardCompatibility(Point3d.Origin, intersectPoint, new Point3d(0.0, 1.0, 0.0), Point3d.Origin, value5, new Point3d(0.0, 1.0, 0.0));
		}
		else
		{
			transform2 = Transform.Identity;
		}
		transform2 *= transform;
		point3d3.Transform(transform2);
		point3d4.Transform(transform2);
		Vector3d vector3d2 = default(Vector3d);
		vector3d2 = point3d4 - point3d3;
		vector3d2.Unitize();
		double t = 0.0;
		value.ClosestPoint(point3d3, out t);
		Transform x2 = default(Transform);
		CSUtilGeometry.OrientProfileToCurve(value, t, ref x2, value3 / 2.0);
		oXform.SetData(new CSXformData(x2));
		if (value2 != null)
		{
			LineCurve lineCurve = new LineCurve(new Point3d(0.0, 0.0, 0.0), new Point3d(0.0, 30.0, 0.0));
			lineCurve.Transform(x2);
			CurveIntersections curveIntersections = Intersection.CurveCurve(lineCurve, value2, CSApplicationHelper.Application.Tolerance, CSApplicationHelper.Application.Tolerance);
			if (curveIntersections.Count > 0)
			{
				Point3d point3d5 = default(Point3d);
				Point3d pointAtStart = lineCurve.PointAtStart;
				point3d5 = curveIntersections[0].PointA;
				double num = pointAtStart.DistanceTo(point3d5);
				if (num < 0.01)
				{
					num = 0.01;
				}
				oYSize.SetData(new CSNumberData(num));
				double num2 = 0.0;
				num2 = curveIntersections[0].ParameterB;
				if (Math.Abs(t - value.Domain.Min) < 0.0001 && Math.Abs(num2 - value2.Domain.Max) < 0.0001)
				{
					num2 = value2.Domain.Min;
				}
				if (Math.Abs(t - value.Domain.Max) < 0.0001 && Math.Abs(num2 - value2.Domain.Min) < 0.0001)
				{
					num2 = value2.Domain.Max;
				}
				oParam2.SetData(new CSNumberData(num2));
			}
		}
		oXform.SetData(new CSXformData(transform2 * x));
		oParam.SetData(new CSNumberData(t));
		oRightRailPoint.SetData(new CSPointData(point3d3));
		oRightRailVector.SetData(new CSVectorData(vector3d2));
		return base.Update();
	}
}
