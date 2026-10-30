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
[CSBlock("LinkedProfileOn2RingRail_V2", "{848770E1-1A36-40ed-893B-394B61A2092F}")]
public sealed class CSLinkedProfileOn2RingRail2 : CSBlock
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

	public CSLinkedProfileOn2RingRail2(string identifier)
		: base(identifier)
	{
		InRail = new CSInputChannel(this, ChannelDataType.Curve, ChannelArrayType.Individual);
		InOutsideRail = new CSInputChannel(this, ChannelDataType.Curve, ChannelArrayType.Individual);
		InAOffset = new CSInputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual);
		InLastRightRailPoint = new CSInputChannel(this, ChannelDataType.Point, ChannelArrayType.Individual);
		InLastRightRailVector = new CSInputChannel(this, ChannelDataType.Vector, ChannelArrayType.Individual);
		OutXform = new CSOutputChannel(this, ChannelDataType.Transform, ChannelArrayType.Individual);
		OutParam = new CSOutputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual);
		OutParam2 = new CSOutputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual);
		OutYSize = new CSOutputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual);
		OutRightRailPoint = new CSOutputChannel("RightRailPoint", this, ChannelDataType.Point, ChannelArrayType.Individual);
		OutRightRailVector = new CSOutputChannel("RightRailVector", this, ChannelDataType.Vector, ChannelArrayType.Individual);
		SetChannelIdentifiers();
	}

	public override bool Update()
	{
		CSDataHelper.GetNurbsCurve(InRail.Data, out var value);
		if (value == null)
		{
			return UpdateMissing("Missing Rail");
		}
		CSDataHelper.GetNurbsCurve(InOutsideRail.Data, out var value2);
		double num = 1.0;
		if (InOutsideRail.Data is CSOutsideRingRailData cSOutsideRingRailData)
		{
			num = cSOutsideRingRailData.Scale;
		}
		double value3 = 0.0;
		InAOffset.GetDouble(ref value3);
		bool flag = true;
		CSDataHelper.GetPoint(InLastRightRailPoint, out var value4);
		if (value4 == Point3d.Unset)
		{
			flag = false;
		}
		CSDataHelper.GetVector(InLastRightRailVector.Data, out var value5);
		if (value5 == Vector3d.Unset)
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
		Transform transform = CSBlockUtil.CSUtilMath.Rotation(value3 / new Vector3d(point3d).Length, Vector3d.YAxis, Point3d.Origin);
		Transform transform2;
		if (flag)
		{
			double length = new Vector3d(value4).Length;
			Point3d intersectPoint = default(Point3d);
			CSBlockUtil.CSUtilMath.IntersectLineAndCircle(point3d, inputVC, length, ref intersectPoint);
			transform2 = CSBlockUtil.CSUtilMath.Orient3Point_BackwardCompatibility(Point3d.Origin, intersectPoint, new Point3d(0.0, 1.0, 0.0), Point3d.Origin, value4, new Point3d(0.0, 1.0, 0.0));
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
		Transform x2 = default(Transform);
		CSUtilGeometry.OrientProfileToCurve(value, t, ref x2, num / 2.0);
		OutXform.SetData(new CSXformData(x2));
		if (value2 != null)
		{
			LineCurve lineCurve = new LineCurve(new Point3d(0.0, 0.0, 0.0), new Point3d(0.0, 30.0, 0.0));
			lineCurve.Transform(x2);
			CurveIntersections curveIntersections = Intersection.CurveCurve(lineCurve, value2, CSApplicationHelper.Application.Tolerance, CSApplicationHelper.Application.Tolerance);
			if (curveIntersections.Count > 0)
			{
				Point3d pointAtStart = lineCurve.PointAtStart;
				Point3d pointA = curveIntersections[0].PointA;
				double num2 = pointAtStart.DistanceTo(pointA);
				if (num2 < 0.01)
				{
					num2 = 0.01;
				}
				OutYSize.SetData(new CSNumberData(num2));
				double num3 = curveIntersections[0].ParameterB;
				if (Math.Abs(t - value.Domain.Min) < 0.0001 && Math.Abs(num3 - value2.Domain.Max) < 0.0001)
				{
					num3 = value2.Domain.Min;
				}
				if (Math.Abs(t - value.Domain.Max) < 0.0001 && Math.Abs(num3 - value2.Domain.Min) < 0.0001)
				{
					num3 = value2.Domain.Max;
				}
				OutParam2.SetData(new CSNumberData(num3));
			}
		}
		OutXform.SetData(new CSXformData(transform2 * x));
		OutParam.SetData(new CSNumberData(t));
		OutRightRailPoint.SetData(new CSPointData(point3d3));
		OutRightRailVector.SetData(new CSVectorData(vectorStore));
		return base.Update();
	}
}
