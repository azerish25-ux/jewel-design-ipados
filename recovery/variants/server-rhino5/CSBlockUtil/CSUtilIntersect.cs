using System;
using RMA.OpenNURBS;
using Rhino.Geometry;
using Rhino.Geometry.Intersect;

namespace CSBlockUtil;

public static class CSUtilIntersect
{
	[Obsolete]
	public static bool GetIntersect(IOnCurve curveLhs, IOnCurve curveRhs, out double param, out IOn3dPoint pt)
	{
		//IL_0012: Unknown result type (might be due to invalid IL or missing references)
		//IL_0018: Expected O, but got Unknown
		param = -1.23432101234321E+308;
		pt = OnUtil.On_UNSET_POINT;
		ArrayOnX_EVENT val = new ArrayOnX_EVENT();
		curveLhs.IntersectCurve(curveRhs, ref val);
		if ((double)val.Count() <= 0.0)
		{
			return false;
		}
		param = val[0][0];
		pt = (IOn3dPoint)(object)val[0][0];
		return true;
	}

	public static bool GetIntersectParam(IOnCurve curveLhs, IOnCurve curveRhs, out double param)
	{
		IOn3dPoint pt;
		return GetIntersect(curveLhs, curveRhs, out param, out pt);
	}

	public static bool GetIntersectPoint(IOnCurve curveLhs, IOnCurve curveRhs, out IOn3dPoint pt)
	{
		double param;
		return GetIntersect(curveLhs, curveRhs, out param, out pt);
	}

	public static bool GetIntersectSurface(Transform transformation, out Brep brep)
	{
		Plane worldXY = Plane.WorldXY;
		worldXY.Transform(transformation);
		return GetIntersectSurface(worldXY, out brep);
	}

	public static bool GetIntersectSurface(Curve nurbsCurve, out Brep brep)
	{
		brep = null;
		if (nurbsCurve == null)
		{
			return false;
		}
		if (!nurbsCurve.TryGetPlane(out var plane))
		{
			return false;
		}
		return GetIntersectSurface(plane, out brep);
	}

	public static bool GetIntersectSurface(Plane plane, out Brep brep)
	{
		brep = null;
		Transform xform = Transform.Scale(plane.PointAt(0.5, 0.5), 1000.0);
		PlaneSurface planeSurface = new PlaneSurface(plane, new Interval(0.0, 1.0), new Interval(0.0, 1.0));
		planeSurface.Transform(xform);
		NurbsSurface nurbsSurface = planeSurface.ToNurbsSurface();
		if (nurbsSurface != null)
		{
			return GetIntersectSurface(nurbsSurface, out brep);
		}
		return false;
	}

	public static bool GetIntersectSurface(GeometryBase geo, out Brep brep)
	{
		brep = null;
		if (geo == null)
		{
			return false;
		}
		if (!geo.HasBrepForm)
		{
			return false;
		}
		brep = Brep.TryConvertBrep(geo);
		return brep != null;
	}

	public static bool GetClosestIntersection(Curve crv, Plane pln, Curve crvTarget, out IntersectionEvent bestIntersectionEvent)
	{
		bestIntersectionEvent = null;
		if (crv == null)
		{
			return false;
		}
		if (!crv.IsValid)
		{
			return false;
		}
		if (!pln.IsValid)
		{
			return false;
		}
		if (crvTarget == null)
		{
			return false;
		}
		if (!crvTarget.IsValid)
		{
			return false;
		}
		CurveIntersections curveIntersections = Intersection.CurvePlane(crv, pln, CSUtil.Tolerance);
		double num = double.NaN;
		foreach (IntersectionEvent item in curveIntersections)
		{
			Point3d pointA = item.PointA;
			if (crvTarget.ClosestPoint(pointA, out var t))
			{
				double num2 = crvTarget.PointAt(t).DistanceTo(pointA);
				if (bestIntersectionEvent == null || num2 < num)
				{
					num = num2;
					bestIntersectionEvent = item;
				}
			}
		}
		return bestIntersectionEvent != null;
	}

	public static bool GetClosestIntersection(Curve crv, Plane pln, Point3d ptTarget, out IntersectionEvent bestIntersectionEvent)
	{
		bestIntersectionEvent = null;
		if (crv == null)
		{
			return false;
		}
		if (!crv.IsValid)
		{
			return false;
		}
		if (!pln.IsValid)
		{
			return false;
		}
		if (!ptTarget.IsValid)
		{
			return false;
		}
		CurveIntersections curveIntersections = Intersection.CurvePlane(crv, pln, CSUtil.Tolerance);
		double num = double.NaN;
		foreach (IntersectionEvent item in curveIntersections)
		{
			Point3d pointA = item.PointA;
			double num2 = ptTarget.DistanceTo(pointA);
			if (bestIntersectionEvent == null || num2 < num)
			{
				num = num2;
				bestIntersectionEvent = item;
			}
		}
		return bestIntersectionEvent != null;
	}
}
