using RMA.OpenNURBS;
using Rhino.Geometry;
using Rhino.Geometry.Intersect;
using Rhino.Runtime;

namespace CSBlockUtil;

public static class CSUtilRingRail
{
	public static bool CreateOutsideRingRail(double radius, IOnCurve ringRailOuterProfile, double thicknessTop, double thicknessBottom, double thicknessSide, out OnNurbsCurve ringRailOuter)
	{
		double num = 0.0 - radius;
		double num2 = 0.0 - radius;
		ringRailOuter = new OnNurbsCurve();
		if (ringRailOuterProfile.GetNurbForm(ref ringRailOuter) <= 0)
		{
			return false;
		}
		if (!ringRailOuter.MakeDeformable())
		{
			return false;
		}
		using (OnXform onXform = new OnXform())
		{
			if (!GetRingRailPointExtentTop(ringRailOuter, out var pointExtent))
			{
				return false;
			}
			if (!GetRingRailPointExtentBottom(ringRailOuter, out var pointExtent2))
			{
				return false;
			}
			double z_scale_factor = (thicknessTop + radius - pointExtent2) / (pointExtent - pointExtent2);
			OnPlane onPlane = new OnPlane(OnUtil.On_xy_plane);
			onPlane.SetOrigin(new On3dPoint(0.0, 0.0, pointExtent2));
			onXform.Scale(onPlane, 1.0, 1.0, z_scale_factor);
			if (!ringRailOuter.Transform(onXform))
			{
				return false;
			}
		}
		using (OnXform onXform2 = new OnXform())
		{
			if (!GetRingRailPointExtentTop(ringRailOuter, out var pointExtent3))
			{
				return false;
			}
			if (!GetRingRailPointExtentBottom(ringRailOuter, out var pointExtent4))
			{
				return false;
			}
			double z_scale_factor2 = (thicknessBottom + pointExtent3 - num) / (pointExtent3 - pointExtent4);
			OnPlane onPlane2 = new OnPlane(OnUtil.On_xy_plane);
			onPlane2.SetOrigin(new On3dPoint(0.0, 0.0, pointExtent3));
			onXform2.Scale(onPlane2, 1.0, 1.0, z_scale_factor2);
			if (!ringRailOuter.Transform(onXform2))
			{
				return false;
			}
		}
		using (OnXform onXform3 = new OnXform())
		{
			if (!GetRingRailPointExtentRight(ringRailOuter, out var pointExtent5))
			{
				return false;
			}
			if (!GetRingRailPointExtentLeft(ringRailOuter, out var pointExtent6))
			{
				return false;
			}
			double x_scale_factor = (thicknessSide * 2.0 + radius - num2) / (pointExtent5 - pointExtent6);
			onXform3.Scale(x_scale_factor, 1.0, 1.0);
			if (!ringRailOuter.Transform(onXform3))
			{
				return false;
			}
		}
		if (!GetRingRailPointTop(ringRailOuter, out var point))
		{
			return false;
		}
		double t = -1.23432101234321E+308;
		if (!ringRailOuter.GetClosestPoint(point, ref t))
		{
			return false;
		}
		if (!ringRailOuter.ChangeClosedCurveSeam(t))
		{
			return false;
		}
		double num3 = OnUtil.ON_DotProduct(ringRailOuter.TangentAt(ringRailOuter.Domain().Min()), OnUtil.On_xaxis);
		if (num3 == 0.0)
		{
			return false;
		}
		if (num3 < 0.0)
		{
			ringRailOuter.Reverse();
		}
		return true;
	}

	public static bool CreateOutsideRingRail(double radius, Curve ringRailOuterProfile, double thicknessTop, double thicknessBottom, double thicknessSide, out NurbsCurve ringRailOuter)
	{
		double num = 0.0 - radius;
		double num2 = 0.0 - radius;
		ringRailOuter = null;
		ringRailOuter = ringRailOuterProfile.ToNurbsCurve();
		if (ringRailOuter == null)
		{
			return false;
		}
		if (!ringRailOuter.MakeDeformable())
		{
			return false;
		}
		Transform transform = default(Transform);
		if (!GetRingRailPointExtentTop(ringRailOuter, out var pointExtent))
		{
			return false;
		}
		if (!GetRingRailPointExtentBottom(ringRailOuter, out var pointExtent2))
		{
			return false;
		}
		double zScaleFactor = (thicknessTop + radius - pointExtent2) / (pointExtent - pointExtent2);
		Plane worldXY = Plane.WorldXY;
		worldXY.Origin = new Point3d(0.0, 0.0, pointExtent2);
		transform = Transform.Scale(worldXY, 1.0, 1.0, zScaleFactor);
		if (!ringRailOuter.Transform(transform))
		{
			return false;
		}
		Transform unset = Transform.Unset;
		if (!GetRingRailPointExtentTop(ringRailOuter, out var pointExtent3))
		{
			return false;
		}
		if (!GetRingRailPointExtentBottom(ringRailOuter, out var pointExtent4))
		{
			return false;
		}
		double zScaleFactor2 = (thicknessBottom + pointExtent3 - num) / (pointExtent3 - pointExtent4);
		Plane worldXY2 = Plane.WorldXY;
		worldXY2.Origin = new Point3d(0.0, 0.0, pointExtent3);
		unset = Transform.Scale(worldXY2, 1.0, 1.0, zScaleFactor2);
		if (!ringRailOuter.Transform(unset))
		{
			return false;
		}
		Transform transform2 = default(Transform);
		if (!GetRingRailPointExtentRight(ringRailOuter, out var pointExtent5))
		{
			return false;
		}
		if (!GetRingRailPointExtentLeft(ringRailOuter, out var pointExtent6))
		{
			return false;
		}
		double xScaleFactor = (thicknessSide * 2.0 + radius - num2) / (pointExtent5 - pointExtent6);
		transform2 = Transform.Scale(Plane.WorldXY, xScaleFactor, 1.0, 1.0);
		if (!ringRailOuter.Transform(transform2))
		{
			return false;
		}
		if (!GetRingRailPointTop(ringRailOuter, out var point))
		{
			return false;
		}
		double t = -1.23432101234321E+308;
		if (!ringRailOuter.ClosestPoint(point, out t))
		{
			return false;
		}
		if (!ringRailOuter.ChangeClosedCurveSeam(t))
		{
			return false;
		}
		double num3 = Vector3d.Multiply(ringRailOuter.TangentAt(ringRailOuter.Domain.Min), Vector3d.XAxis);
		if (num3 == 0.0)
		{
			return false;
		}
		if (num3 < 0.0)
		{
			ringRailOuter.Reverse();
		}
		return true;
	}

	public static bool CreateRingRail(double radius, out OnNurbsCurve ringRail)
	{
		ringRail = null;
		OnCircle onCircle = new OnCircle(OnUtil.On_zx_plane, radius);
		if (!onCircle.IsValid())
		{
			return false;
		}
		OnArcCurve onArcCurve = new OnArcCurve(onCircle);
		if (!onArcCurve.IsValid())
		{
			return false;
		}
		if (!GetRingRailPointTop(onArcCurve, out var point))
		{
			return false;
		}
		double t = -1.23432101234321E+308;
		if (!onArcCurve.GetClosestPoint(point, ref t))
		{
			return false;
		}
		if (!onArcCurve.ChangeClosedCurveSeam(t))
		{
			return false;
		}
		double num = OnUtil.ON_DotProduct(onArcCurve.TangentAt(onArcCurve.Domain().Min()), OnUtil.On_xaxis);
		if (num == 0.0)
		{
			return false;
		}
		if (num < 0.0)
		{
			onArcCurve.Reverse();
		}
		ringRail = new OnNurbsCurve();
		return onArcCurve.GetNurbForm(ref ringRail) > 0;
	}

	public static bool CreateRingRail(double radius, out NurbsCurve ringRail)
	{
		ringRail = null;
		OnCircle onCircle = new OnCircle(OnUtil.On_zx_plane, radius);
		if (!onCircle.IsValid())
		{
			return false;
		}
		OnArcCurve onArcCurve = new OnArcCurve(onCircle);
		if (!onArcCurve.IsValid())
		{
			return false;
		}
		if (!GetRingRailPointTop(onArcCurve, out var point))
		{
			return false;
		}
		double t = -1.23432101234321E+308;
		if (!onArcCurve.GetClosestPoint(point, ref t))
		{
			return false;
		}
		if (!onArcCurve.ChangeClosedCurveSeam(t))
		{
			return false;
		}
		double num = OnUtil.ON_DotProduct(onArcCurve.TangentAt(onArcCurve.Domain().Min()), OnUtil.On_xaxis);
		if (num == 0.0)
		{
			return false;
		}
		if (num < 0.0)
		{
			onArcCurve.Reverse();
		}
		OnNurbsCurve nurbs_curve = new OnNurbsCurve();
		onArcCurve.GetNurbForm(ref nurbs_curve);
		ringRail = Interop.FromOnCurve(nurbs_curve).ToNurbsCurve();
		return ringRail.GetLength() > 0.0;
	}

	public static bool GetIntersectPoint(IOnCurve curveLhs, IOnCurve curveRhs, out IOn3dPoint pt)
	{
		pt = OnUtil.On_UNSET_POINT;
		ArrayOnX_EVENT x = new ArrayOnX_EVENT();
		curveLhs.IntersectCurve(curveRhs, ref x);
		if ((double)x.Count() <= 0.0)
		{
			return false;
		}
		pt = x[0].get_m_pointA(0);
		return true;
	}

	public static bool GetRingRailPointBottom(IOnCurve curve, out IOn3dPoint point)
	{
		OnLineCurve curveRhs = new OnLineCurve(OnUtil.On_origin, new On3dPoint(0.0, 0.0, -100.0));
		return GetIntersectPoint(curve, curveRhs, out point);
	}

	public static bool GetRingRailPointBottom(Curve curve, out Point3d point)
	{
		LineCurve curveB = new LineCurve(Point3d.Origin, new Point3d(0.0, 0.0, -100.0));
		CurveIntersections curveIntersections = Intersection.CurveCurve(curve, curveB, CSUtil.Tolerance, CSUtil.Tolerance);
		point = curveIntersections[0].PointA;
		return point.IsValid;
	}

	public static bool GetRingRailPointExtentBottom(IOnCurve curve, out double pointExtent)
	{
		pointExtent = -1.23432101234321E+308;
		if (!GetRingRailPointBottom(curve, out var point))
		{
			return false;
		}
		pointExtent = point.z;
		return true;
	}

	public static bool GetRingRailPointExtentBottom(Curve curve, out double pointExtent)
	{
		pointExtent = double.NaN;
		if (!GetRingRailPointBottom(curve, out var point))
		{
			return false;
		}
		pointExtent = point.Z;
		return true;
	}

	public static bool GetRingRailPointExtentLeft(IOnCurve curve, out double pointExtent)
	{
		pointExtent = -1.23432101234321E+308;
		if (!GetRingRailPointLeft(curve, out var point))
		{
			return false;
		}
		pointExtent = point.x;
		return true;
	}

	public static bool GetRingRailPointExtentLeft(Curve curve, out double pointExtent)
	{
		pointExtent = double.NaN;
		if (!GetRingRailPointLeft(curve, out var point))
		{
			return false;
		}
		pointExtent = point.X;
		return true;
	}

	public static bool GetRingRailPointExtentRight(IOnCurve curve, out double pointExtent)
	{
		pointExtent = -1.23432101234321E+308;
		if (!GetRingRailPointRight(curve, out var point))
		{
			return false;
		}
		pointExtent = point.x;
		return true;
	}

	public static bool GetRingRailPointExtentRight(Curve curve, out double pointExtent)
	{
		pointExtent = double.NaN;
		if (!GetRingRailPointRight(curve, out var point))
		{
			return false;
		}
		pointExtent = point.X;
		return true;
	}

	public static bool GetRingRailPointExtents(IOnCurve curve, out double pointExtentTop, out double pointExtentBottom, out double pointExtentRight, out double pointExtentLeft)
	{
		return (byte)(1u & (GetRingRailPointExtentTop(curve, out pointExtentTop) ? 1u : 0u) & (GetRingRailPointExtentBottom(curve, out pointExtentBottom) ? 1u : 0u) & (GetRingRailPointExtentRight(curve, out pointExtentRight) ? 1u : 0u) & (GetRingRailPointExtentLeft(curve, out pointExtentLeft) ? 1u : 0u)) != 0;
	}

	public static bool GetRingRailPointExtents(Curve curve, out double pointExtentTop, out double pointExtentBottom, out double pointExtentRight, out double pointExtentLeft)
	{
		return (byte)(1u & (GetRingRailPointExtentTop(curve, out pointExtentTop) ? 1u : 0u) & (GetRingRailPointExtentBottom(curve, out pointExtentBottom) ? 1u : 0u) & (GetRingRailPointExtentRight(curve, out pointExtentRight) ? 1u : 0u) & (GetRingRailPointExtentLeft(curve, out pointExtentLeft) ? 1u : 0u)) != 0;
	}

	public static bool GetRingRailPointExtentTop(IOnCurve curve, out double pointExtent)
	{
		pointExtent = -1.23432101234321E+308;
		if (!GetRingRailPointTop(curve, out var point))
		{
			return false;
		}
		pointExtent = point.z;
		return true;
	}

	public static bool GetRingRailPointExtentTop(Curve curve, out double pointExtent)
	{
		pointExtent = double.NaN;
		if (!GetRingRailPointTop(curve, out var point))
		{
			return false;
		}
		pointExtent = point.Z;
		return true;
	}

	public static bool GetRingRailPointLeft(IOnCurve curve, out IOn3dPoint point)
	{
		OnLineCurve curveRhs = new OnLineCurve(OnUtil.On_origin, new On3dPoint(-100.0, 0.0, 0.0));
		return GetIntersectPoint(curve, curveRhs, out point);
	}

	public static bool GetRingRailPointLeft(Curve curve, out Point3d point)
	{
		LineCurve curveB = new LineCurve(Point3d.Origin, new Point3d(-100.0, 0.0, 0.0));
		CurveIntersections curveIntersections = Intersection.CurveCurve(curve, curveB, CSUtil.Tolerance, CSUtil.Tolerance);
		point = curveIntersections[0].PointA;
		return point.IsValid;
	}

	public static bool GetRingRailPointRight(IOnCurve curve, out IOn3dPoint point)
	{
		OnLineCurve curveRhs = new OnLineCurve(OnUtil.On_origin, new On3dPoint(100.0, 0.0, 0.0));
		return GetIntersectPoint(curve, curveRhs, out point);
	}

	public static bool GetRingRailPointRight(Curve curve, out Point3d point)
	{
		LineCurve curveB = new LineCurve(Point3d.Origin, new Point3d(100.0, 0.0, 0.0));
		CurveIntersections curveIntersections = Intersection.CurveCurve(curve, curveB, CSUtil.Tolerance, CSUtil.Tolerance);
		point = curveIntersections[0].PointA;
		return point.IsValid;
	}

	public static bool GetRingRailPoints(IOnCurve curve, out IOn3dPoint pointTop, out IOn3dPoint pointBottom, out IOn3dPoint pointRight, out IOn3dPoint pointLeft)
	{
		return (byte)(1u & (GetRingRailPointTop(curve, out pointTop) ? 1u : 0u) & (GetRingRailPointBottom(curve, out pointBottom) ? 1u : 0u) & (GetRingRailPointRight(curve, out pointRight) ? 1u : 0u) & (GetRingRailPointLeft(curve, out pointLeft) ? 1u : 0u)) != 0;
	}

	public static bool GetRingRailPointTop(IOnCurve curve, out IOn3dPoint point)
	{
		OnLineCurve curveRhs = new OnLineCurve(OnUtil.On_origin, new On3dPoint(0.0, 0.0, 100.0));
		return GetIntersectPoint(curve, curveRhs, out point);
	}

	public static bool GetRingRailPointTop(Curve curve, out Point3d point)
	{
		LineCurve curveB = new LineCurve(Point3d.Origin, new Point3d(0.0, 0.0, 100.0));
		CurveIntersections curveIntersections = Intersection.CurveCurve(curve, curveB, CSUtil.Tolerance, CSUtil.Tolerance);
		point = curveIntersections[0].PointA;
		return point.IsValid;
	}
}
