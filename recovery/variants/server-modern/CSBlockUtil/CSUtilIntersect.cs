using System;
using RMA.OpenNURBS;
using Rhino.Geometry;
using Rhino.Geometry.Intersect;
using Rhino.Runtime;

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
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0005: Unknown result type (might be due to invalid IL or missing references)
		//IL_0008: Unknown result type (might be due to invalid IL or missing references)
		//IL_000f: Unknown result type (might be due to invalid IL or missing references)
		Plane worldXY = Plane.WorldXY;
		((Plane)(ref worldXY)).Transform(transformation);
		return GetIntersectSurface(worldXY, out brep);
	}

	public static bool GetIntersectSurface(Curve nurbsCurve, out Brep brep)
	{
		//IL_0014: Unknown result type (might be due to invalid IL or missing references)
		brep = null;
		if (nurbsCurve == null)
		{
			return false;
		}
		Plane plane = default(Plane);
		if (!nurbsCurve.TryGetPlane(ref plane))
		{
			return false;
		}
		return GetIntersectSurface(plane, out brep);
	}

	public static bool GetIntersectSurface(Plane plane, out Brep brep)
	{
		//IL_0017: Unknown result type (might be due to invalid IL or missing references)
		//IL_0025: Unknown result type (might be due to invalid IL or missing references)
		//IL_002a: Unknown result type (might be due to invalid IL or missing references)
		//IL_002b: Unknown result type (might be due to invalid IL or missing references)
		//IL_003e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0055: Unknown result type (might be due to invalid IL or missing references)
		//IL_005a: Unknown result type (might be due to invalid IL or missing references)
		//IL_005f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0060: Unknown result type (might be due to invalid IL or missing references)
		brep = null;
		Transform val = Transform.Scale(((Plane)(ref plane)).PointAt(0.5, 0.5), 1000.0);
		PlaneSurface val2 = new PlaneSurface(plane, new Interval(0.0, 1.0), new Interval(0.0, 1.0));
		((GeometryBase)val2).Transform(val);
		NurbsSurface val3 = ((Surface)val2).ToNurbsSurface();
		if (val3 != null)
		{
			return GetIntersectSurface((GeometryBase)(object)val3, out brep);
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
		//IL_002d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0052: Unknown result type (might be due to invalid IL or missing references)
		//IL_0057: Unknown result type (might be due to invalid IL or missing references)
		//IL_0059: Unknown result type (might be due to invalid IL or missing references)
		//IL_0066: Unknown result type (might be due to invalid IL or missing references)
		//IL_006b: Unknown result type (might be due to invalid IL or missing references)
		//IL_006f: Unknown result type (might be due to invalid IL or missing references)
		bestIntersectionEvent = null;
		if (crv == null)
		{
			return false;
		}
		if (!((CommonObject)crv).IsValid)
		{
			return false;
		}
		if (!((Plane)(ref pln)).IsValid)
		{
			return false;
		}
		if (crvTarget == null)
		{
			return false;
		}
		if (!((CommonObject)crvTarget).IsValid)
		{
			return false;
		}
		CurveIntersections obj = Intersection.CurvePlane(crv, pln, CSUtil.Tolerance);
		double num = double.NaN;
		double num2 = default(double);
		foreach (IntersectionEvent item in obj)
		{
			Point3d pointA = item.PointA;
			if (crvTarget.ClosestPoint(pointA, ref num2))
			{
				Point3d val = crvTarget.PointAt(num2);
				double num3 = ((Point3d)(ref val)).DistanceTo(pointA);
				if (bestIntersectionEvent == null || num3 < num)
				{
					num = num3;
					bestIntersectionEvent = item;
				}
			}
		}
		return bestIntersectionEvent != null;
	}

	public static bool GetClosestIntersection(Curve crv, Plane pln, Point3d ptTarget, out IntersectionEvent bestIntersectionEvent)
	{
		//IL_0029: Unknown result type (might be due to invalid IL or missing references)
		//IL_004e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0053: Unknown result type (might be due to invalid IL or missing references)
		//IL_0056: Unknown result type (might be due to invalid IL or missing references)
		bestIntersectionEvent = null;
		if (crv == null)
		{
			return false;
		}
		if (!((CommonObject)crv).IsValid)
		{
			return false;
		}
		if (!((Plane)(ref pln)).IsValid)
		{
			return false;
		}
		if (!((Point3d)(ref ptTarget)).IsValid)
		{
			return false;
		}
		CurveIntersections obj = Intersection.CurvePlane(crv, pln, CSUtil.Tolerance);
		double num = double.NaN;
		foreach (IntersectionEvent item in obj)
		{
			Point3d pointA = item.PointA;
			double num2 = ((Point3d)(ref ptTarget)).DistanceTo(pointA);
			if (bestIntersectionEvent == null || num2 < num)
			{
				num = num2;
				bestIntersectionEvent = item;
			}
		}
		return bestIntersectionEvent != null;
	}
}
