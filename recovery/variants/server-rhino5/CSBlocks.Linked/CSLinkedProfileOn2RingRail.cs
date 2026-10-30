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

namespace CSBlocks.Linked;

[CSBlock("LinkedProfileOn2RingRail", "{D0E33A43-794E-4FE4-9318-D2424ABAC382}")]
public sealed class CSLinkedProfileOn2RingRail : CSBlock
{
	public CSInputChannel InRail;

	public CSInputChannel InOutsideRail;

	public CSInputChannel InAOffset;

	public CSInputChannel InLastRightRailPoint;

	public CSInputChannel InLastRightRailVector;

	public CSOutputChannel OutXform;

	public CSOutputChannel OutParam;

	public CSOutputChannel OutParam2;

	public CSOutputChannel OutYSize;

	public CSOutputChannel OutRightRailPoint;

	public CSOutputChannel OutRightRailVector;

	public CSLinkedProfileOn2RingRail(string identifier)
		: base(identifier)
	{
		InRail = new CSInputChannel(this, ChannelDataType.Curve, ChannelArrayType.Individual);
		InOutsideRail = new CSInputChannel(this, ChannelDataType.Curve, ChannelArrayType.Individual);
		InAOffset = new CSInputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual, 0.0);
		InLastRightRailPoint = new CSInputChannel(this, ChannelDataType.Point, ChannelArrayType.Individual, Point3d.Unset);
		InLastRightRailVector = new CSInputChannel(this, ChannelDataType.Vector, ChannelArrayType.Individual);
		OutXform = new CSOutputChannel(this, ChannelDataType.Transform, ChannelArrayType.Individual);
		OutParam = new CSOutputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual);
		OutParam2 = new CSOutputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual);
		OutYSize = new CSOutputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual);
		OutRightRailPoint = new CSOutputChannel("RightRailPoint", this, ChannelDataType.Point, ChannelArrayType.Individual);
		OutRightRailVector = new CSOutputChannel("RightRailVector", this, ChannelDataType.Vector, ChannelArrayType.Individual);
		SetChannelIdentifiers(autoCheckInputs: true);
	}

	public override bool Update()
	{
		NurbsCurve nurbsCurve = InRail.NurbsCurve;
		if (nurbsCurve == null)
		{
			return UpdateMissing("Missing Rail");
		}
		NurbsCurve nurbsCurve2 = InOutsideRail.NurbsCurve;
		double num = 1.0;
		if (InOutsideRail.Data is CSOutsideRingRailData cSOutsideRingRailData)
		{
			num = cSOutsideRingRailData.Scale;
		}
		double num2 = InAOffset.Double;
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
		Transform transform = CSBlockUtil.CSUtilMath.Rotation(num2 / new Vector3d(point3d2).Length, Vector3d.YAxis, Point3d.Origin);
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
		Transform x2 = default(Transform);
		CSUtilGeometry.OrientProfileToCurve(nurbsCurve, t, ref x2, num / 2.0);
		OutXform.SetData(new CSXformData(x2));
		if (nurbsCurve2 != null)
		{
			LineCurve lineCurve = new LineCurve(new Point3d(0.0, 0.0, 0.0), new Point3d(0.0, 30.0, 0.0));
			lineCurve.Transform(x2);
			CurveIntersections curveIntersections = Intersection.CurveCurve(lineCurve, nurbsCurve2, CSApplicationHelper.Application.Tolerance, CSApplicationHelper.Application.Tolerance);
			if (curveIntersections.Count > 0)
			{
				Point3d pointAtStart = lineCurve.PointAtStart;
				Point3d pointA = curveIntersections[0].PointA;
				double num3 = pointAtStart.DistanceTo(pointA);
				if (num3 < 0.01)
				{
					num3 = 0.01;
				}
				OutYSize.SetData(new CSNumberData(num3));
				double num4 = curveIntersections[0].ParameterB;
				if (Math.Abs(t - nurbsCurve.Domain.Min) < 0.0001 && Math.Abs(num4 - nurbsCurve2.Domain.Max) < 0.0001)
				{
					num4 = nurbsCurve2.Domain.Min;
				}
				if (Math.Abs(t - nurbsCurve.Domain.Max) < 0.0001 && Math.Abs(num4 - nurbsCurve2.Domain.Min) < 0.0001)
				{
					num4 = nurbsCurve2.Domain.Max;
				}
				OutParam2.SetData(new CSNumberData(num4));
			}
		}
		OutXform.SetData(new CSXformData(transform2 * x));
		OutParam.SetData(new CSNumberData(t));
		OutRightRailPoint.SetData(new CSPointData(point3d4));
		OutRightRailVector.SetData(new CSVectorData(vectorStore));
		return base.Update();
	}
}
