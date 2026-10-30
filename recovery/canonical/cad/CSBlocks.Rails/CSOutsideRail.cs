using CS;
using CS.CSDataUtilities;
using CS.CSObjects.CSBlocks;
using CS.Parametric;
using CS.Parametric.Channels;
using CSBlockUtil;
using CSDatas;
using Rhino.Geometry;
using Rhino.Geometry.Intersect;

namespace CSBlocks.Rails;

[CSBlock("OutsideRail", "{594560C4-6033-4FAC-B990-68FBACEB1E0F}")]
public sealed class CSOutsideRail : CSBlock
{
	public readonly CSInputChannel InRingRailRadius;

	public readonly CSInputChannel InShape;

	public readonly CSInputChannel InThicknessTop;

	public readonly CSInputChannel InThicknessBottom;

	public readonly CSInputChannel InThicknessSide;

	public readonly CSInputChannel InEnableLock;

	public readonly CSOutputChannel OutOutsideRingRail;

	public CSOutsideRail(string identifier)
		: base(identifier)
	{
		InRingRailRadius = new CSInputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual);
		InShape = new CSInputChannel(this, ChannelDataType.Curve, ChannelArrayType.Individual);
		InThicknessTop = new CSInputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual);
		InThicknessBottom = new CSInputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual);
		InThicknessSide = new CSInputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual);
		InEnableLock = new CSInputChannel(this, ChannelDataType.Boolean, ChannelArrayType.Individual);
		OutOutsideRingRail = new CSOutputChannel(this, ChannelDataType.OutsideRingRail, ChannelArrayType.Individual);
		SetChannelIdentifiers();
	}

	public override bool Update()
	{
		CSDataHelper.GetNurbsCurve(InShape.Data, out var value);
		if (value == null)
		{
			return UpdateMissing("Shape");
		}
		if (!value.IsPlanar())
		{
			return UpdateError("Shape must be planar");
		}
		double value2 = 8.667196228921199;
		InRingRailRadius.GetDouble(ref value2);
		double value3 = 3.0;
		InThicknessTop.GetDouble(ref value3);
		double value4 = 1.0;
		InThicknessBottom.GetDouble(ref value4);
		double value5 = 2.0;
		InThicknessSide.GetDouble(ref value5);
		bool value6 = false;
		InEnableLock.GetBoolean(ref value6);
		NurbsCurve ringRailOuter = value.DuplicateCurve().ToNurbsCurve();
		ringRailOuter.MakeDeformable();
		if (value6)
		{
			double num = value2;
			double num2 = 0.0 - value2;
			double num3 = value2;
			if (!CSUtilRingRail.GetRingRailPointExtents(ringRailOuter, out var pointExtentTop, out var pointExtentBottom, out var pointExtentRight, out var _))
			{
				return UpdateError("GetRingRailPointExtents failed");
			}
			LineCurve curveB = new LineCurve(new Point3d(-200.0, 0.0, 0.0), new Point3d(200.0, 0.0, 0.0));
			double num4 = pointExtentRight;
			double num5 = num3 + value5;
			Transform xform = Transform.Scale(Plane.WorldXY, num5 / num4, 1.0, 1.0);
			ringRailOuter.Transform(xform);
			CurveIntersections curveIntersections = Intersection.CurveCurve(ringRailOuter, curveB, CSApplicationHelper.Application.Tolerance, CSApplicationHelper.Application.Tolerance);
			if (curveIntersections.Count != 2)
			{
				return UpdateError("Intersect of custom Rail Failed\n");
			}
			NurbsCurve nurbsCurve = ringRailOuter.DuplicateCurve().ToNurbsCurve();
			nurbsCurve.ChangeClosedCurveSeam(curveIntersections[0].ParameterA);
			NurbsCurve nurbsCurve2 = nurbsCurve.DuplicateCurve().ToNurbsCurve();
			nurbsCurve2.Reverse();
			double t = 0.0;
			nurbsCurve.ClosestPoint(curveIntersections[1].PointA, out t);
			Curve curve = nurbsCurve.Trim(new Interval(nurbsCurve.Domain.Min, t)).ToNurbsCurve();
			nurbsCurve2.ClosestPoint(curveIntersections[1].PointA, out t);
			Curve curve2 = nurbsCurve2.Trim(new Interval(nurbsCurve2.Domain.Min, t)).ToNurbsCurve();
			if (curve.PointAt(curve.Domain.Mid).Z < 0.0)
			{
				Curve curve3 = curve;
				curve = curve2;
				curve2 = curve3;
			}
			Transform xform2 = Transform.Scale(Plane.WorldXY, 1.0, 1.0, (value3 + num) / pointExtentTop);
			Transform xform3 = Transform.Scale(Plane.WorldXY, 1.0, 1.0, (0.0 - value4 + num2) / pointExtentBottom);
			curve.Transform(xform2);
			curve2.Transform(xform3);
			Curve[] array = Curve.JoinCurves(new Curve[2] { curve, curve2 });
			if (array.Length != 0)
			{
				ringRailOuter = array[0].ToNurbsCurve();
			}
			LineCurve curveB2 = new LineCurve(new Point3d(0.0, 0.0, 0.0), new Point3d(0.0, 0.0, 100.0));
			CurveIntersections curveIntersections2 = Intersection.CurveCurve(ringRailOuter, curveB2, CSApplicationHelper.Application.Tolerance, CSApplicationHelper.Application.Tolerance);
			if (curveIntersections2.Count > 0)
			{
				t = curveIntersections2[0].ParameterA;
			}
			else
			{
				ringRailOuter.ClosestPoint(new Point3d(0.0, 0.0, 100.0), out t);
			}
			ringRailOuter.ChangeClosedCurveSeam(t);
			if (ringRailOuter.TangentAt(ringRailOuter.Domain.Min) * Vector3d.XAxis < 0.0)
			{
				ringRailOuter.Reverse();
			}
		}
		else if (!CSUtilRingRail.CreateOutsideRingRail(value2, value, value3, value4, value5, out ringRailOuter))
		{
			return UpdateError("CreateOutsideRingRail failed");
		}
		OutOutsideRingRail.SetData(new CSOutsideRingRailData(ringRailOuter));
		return base.Update();
	}
}
