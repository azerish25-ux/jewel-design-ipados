using System;
using CS;
using CS.CSDataUtilities;
using CS.CSObjects.CSBlocks;
using CS.Parametric;
using CS.Parametric.Channels;
using CSBlockUtil;
using CSCompatibility;
using CSDatas;
using RMA.OpenNURBS;
using Rhino.Geometry;
using Rhino.Geometry.Intersect;
using Rhino.Runtime;

namespace CSBlocks.Rails.Obsolete;

[Obsolete]
[CSBlock("OutsideRail_V1", "{F9F6400F-67C2-4883-8C44-A91F411F2EE3}")]
public sealed class CSOutsideRail1 : CSBlock
{
	public readonly CSInputChannel iRingRadius;

	public readonly CSInputChannel iOutsideCurve;

	public readonly CSInputChannel iTopThickness;

	public readonly CSInputChannel iSideThickness;

	public readonly CSInputChannel iBottomThickness;

	public readonly CSInputChannel iLockOn;

	public readonly CSOutputChannel oOutsideRingRail;

	public readonly CSOutputChannel oOutsideScale;

	public CSOutsideRail1(string identifier)
		: base(identifier)
	{
		iRingRadius = new CSInputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual);
		iOutsideCurve = new CSInputChannel(this, ChannelDataType.Curve, ChannelArrayType.Individual);
		iTopThickness = new CSInputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual);
		iSideThickness = new CSInputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual);
		iBottomThickness = new CSInputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual);
		iLockOn = new CSInputChannel(this, ChannelDataType.Boolean, ChannelArrayType.Individual);
		oOutsideRingRail = new CSOutputChannel(this, ChannelDataType.RingRail, ChannelArrayType.Individual);
		oOutsideScale = new CSOutputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual);
		SetChannelIdentifiers();
	}

	public override bool Update()
	{
		CSDataHelper.GetNurbsCurve(iOutsideCurve.Data, out var value);
		if (value == null)
		{
			return UpdateError("Outside Ring Rail: Missing Custom Rail (No Curve)");
		}
		double value2 = 3.0;
		double value3 = 2.0;
		double value4 = 1.0;
		bool value5 = false;
		iTopThickness.GetDouble(ref value2);
		iSideThickness.GetDouble(ref value3);
		iBottomThickness.GetDouble(ref value4);
		iLockOn.GetBoolean(ref value5);
		double value6 = 0.0;
		if (!iRingRadius.GetDouble(ref value6))
		{
			CSDataHelper.GetNurbsCurve(iRingRadius.Data, out var value7);
			if (value7 == null)
			{
				return UpdateError("Missing Ring Rail");
			}
			value6 = value7.GetBoundingBox(accurate: true).Max.Z;
		}
		double num = value6;
		double num2 = 0.0 - value6;
		double num3 = value6;
		NurbsCurve nurbsCurve = value.DuplicateCurve().ToNurbsCurve();
		nurbsCurve.MakeDeformable();
		CSUtilGeometry.GetMinMax(nurbsCurve, Point3d.Origin, 50, out var min, out var _);
		if (min == 0.0 || min == double.MaxValue)
		{
			return UpdateError("Intersect of custom Rail Failed");
		}
		if (num == 0.0)
		{
			return UpdateError("Intersect of custom Rail Failed");
		}
		Transform xform = Transform.Scale(Point3d.Origin, num / min);
		nurbsCurve.Transform(xform);
		if (!CSUtilRingRail.GetRingRailPointExtents(nurbsCurve, out var pointExtentTop, out var pointExtentBottom, out var pointExtentRight, out var pointExtentLeft))
		{
			return UpdateError("Failed to get rail curve size");
		}
		if (!value5)
		{
			double num4 = pointExtentTop - pointExtentBottom;
			double num5 = num + value2 - pointExtentBottom;
			Point3d origin = new Point3d(0.0, 0.0, pointExtentBottom);
			Point3d origin2 = new Point3d(0.0, 0.0, pointExtentTop);
			Transform xform2 = Transform.Scale(new Plane(origin, Vector3d.XAxis, Vector3d.ZAxis), 1.0, num5 / num4, 1.0);
			nurbsCurve.Transform(xform2);
			if (!CSUtilRingRail.GetRingRailPointExtents(nurbsCurve, out pointExtentTop, out pointExtentBottom, out pointExtentRight, out pointExtentLeft))
			{
				return UpdateError("Failed to get rail curve size");
			}
			origin.Z = pointExtentBottom;
			origin2.Z = pointExtentTop;
			num4 = pointExtentTop - pointExtentBottom;
			num5 = pointExtentTop - (num2 - value4);
			Transform xform3 = Transform.Scale(new Plane(origin2, Vector3d.XAxis, Vector3d.ZAxis), 1.0, num5 / num4, 1.0);
			nurbsCurve.Transform(xform3);
			if (!CSUtilRingRail.GetRingRailPointExtents(nurbsCurve, out pointExtentTop, out pointExtentBottom, out pointExtentRight, out pointExtentLeft))
			{
				return UpdateError("Failed to get rail curve size");
			}
			num4 = pointExtentRight;
			num5 = num3 + value3;
			Transform xform4 = Transform.Scale(Plane.WorldXY, num5 / num4, 1.0, 1.0);
			nurbsCurve.Transform(xform4);
		}
		else
		{
			LineCurve curveB = new LineCurve(new Point3d(-200.0, 0.0, 0.0), new Point3d(200.0, 0.0, 0.0));
			double num6 = pointExtentRight;
			double num7 = num3 + value3;
			Transform xform5 = Transform.Scale(Plane.WorldXY, num7 / num6, 1.0, 1.0);
			nurbsCurve.Transform(xform5);
			CurveIntersections curveIntersections = Intersection.CurveCurve(nurbsCurve, curveB, CSApplicationHelper.Application.Tolerance, CSApplicationHelper.Application.Tolerance);
			if (curveIntersections.Count != 2)
			{
				return UpdateError("Intersect of custom Rail Failed");
			}
			NurbsCurve nurbsCurve2 = nurbsCurve.DuplicateCurve().ToNurbsCurve();
			nurbsCurve2.ChangeClosedCurveSeam(curveIntersections[0].ParameterA);
			NurbsCurve nurbsCurve3 = nurbsCurve2.DuplicateCurve().ToNurbsCurve();
			nurbsCurve3.Reverse();
			double t = 0.0;
			nurbsCurve2.ClosestPoint(curveIntersections[1].PointA, out t);
			nurbsCurve2 = nurbsCurve2.Trim(new Interval(nurbsCurve2.Domain.Min, t)).ToNurbsCurve();
			nurbsCurve3.ClosestPoint(curveIntersections[1].PointA, out t);
			nurbsCurve3 = nurbsCurve3.Trim(new Interval(nurbsCurve3.Domain.Min, t)).ToNurbsCurve();
			if (nurbsCurve2.PointAt(nurbsCurve2.Domain.Mid).Z < 0.0)
			{
				NurbsCurve nurbsCurve4 = nurbsCurve2;
				nurbsCurve2 = nurbsCurve3;
				nurbsCurve3 = nurbsCurve4;
			}
			Transform xform6 = Transform.Scale(Plane.WorldXY, 1.0, 1.0, (value2 + num) / pointExtentTop);
			Transform xform7 = Transform.Scale(Plane.WorldXY, 1.0, 1.0, (0.0 - value4 + num2) / pointExtentBottom);
			nurbsCurve2.Transform(xform6);
			nurbsCurve3.Transform(xform7);
			Curve[] array = Curve.JoinCurves(new Curve[2] { nurbsCurve2, nurbsCurve3 });
			if (array.Length != 0)
			{
				nurbsCurve = array[0].ToNurbsCurve();
			}
			LineCurve curveB2 = new LineCurve(new Point3d(0.0, 0.0, 0.0), new Point3d(0.0, 0.0, 100.0));
			CurveIntersections curveIntersections2 = Intersection.CurveCurve(nurbsCurve, curveB2, CSApplicationHelper.Application.Tolerance, CSApplicationHelper.Application.Tolerance);
			if (curveIntersections2.Count > 0)
			{
				t = curveIntersections2[0].ParameterA;
			}
			else
			{
				nurbsCurve.ClosestPoint(new Point3d(0.0, 0.0, 100.0), out t);
			}
			nurbsCurve.ChangeClosedCurveSeam(t);
			if (nurbsCurve.TangentAt(nurbsCurve.Domain.Min) * Vector3d.XAxis < 0.0)
			{
				nurbsCurve.Reverse();
			}
		}
		OnCurve obj = (OnCurve)Interop.ToOnCurve(nurbsCurve);
		OnNurbsCurve nurbs_curve = new OnNurbsCurve();
		obj.GetNurbForm(ref nurbs_curve);
		oOutsideRingRail.SetData(new CSOutsideRingRailData(nurbs_curve.Convert()));
		oOutsideScale.SetData(new CSNumberData(0.0));
		return base.Update();
	}
}
