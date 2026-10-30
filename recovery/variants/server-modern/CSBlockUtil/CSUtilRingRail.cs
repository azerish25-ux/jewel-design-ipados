using System;
using RMA.OpenNURBS;
using Rhino.Geometry;
using Rhino.Geometry.Intersect;
using Rhino.Runtime;

namespace CSBlockUtil;

public static class CSUtilRingRail
{
	public static bool CreateOutsideRingRail(double radius, IOnCurve ringRailOuterProfile, double thicknessTop, double thicknessBottom, double thicknessSide, out OnNurbsCurve ringRailOuter)
	{
		//IL_000c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0012: Expected O, but got Unknown
		//IL_002b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0032: Expected O, but got Unknown
		//IL_006d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0074: Expected O, but got Unknown
		//IL_008a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0094: Expected O, but got Unknown
		//IL_00d3: Unknown result type (might be due to invalid IL or missing references)
		//IL_00da: Expected O, but got Unknown
		//IL_0115: Unknown result type (might be due to invalid IL or missing references)
		//IL_011c: Expected O, but got Unknown
		//IL_0132: Unknown result type (might be due to invalid IL or missing references)
		//IL_013c: Expected O, but got Unknown
		//IL_017b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0182: Expected O, but got Unknown
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
		OnXform val = new OnXform();
		try
		{
			if (!GetRingRailPointExtentTop((IOnCurve)(object)ringRailOuter, out var pointExtent))
			{
				return false;
			}
			if (!GetRingRailPointExtentBottom((IOnCurve)(object)ringRailOuter, out var pointExtent2))
			{
				return false;
			}
			double num3 = (thicknessTop + radius - pointExtent2) / (pointExtent - pointExtent2);
			OnPlane val2 = new OnPlane(OnUtil.On_xy_plane);
			val2.SetOrigin((IOn3dPoint)new On3dPoint(0.0, 0.0, pointExtent2));
			val.Scale((IOnPlane)(object)val2, 1.0, 1.0, num3);
			if (!ringRailOuter.Transform((IOnXform)(object)val))
			{
				return false;
			}
		}
		finally
		{
			((IDisposable)val)?.Dispose();
		}
		OnXform val3 = new OnXform();
		try
		{
			if (!GetRingRailPointExtentTop((IOnCurve)(object)ringRailOuter, out var pointExtent3))
			{
				return false;
			}
			if (!GetRingRailPointExtentBottom((IOnCurve)(object)ringRailOuter, out var pointExtent4))
			{
				return false;
			}
			double num4 = (thicknessBottom + pointExtent3 - num) / (pointExtent3 - pointExtent4);
			OnPlane val4 = new OnPlane(OnUtil.On_xy_plane);
			val4.SetOrigin((IOn3dPoint)new On3dPoint(0.0, 0.0, pointExtent3));
			val3.Scale((IOnPlane)(object)val4, 1.0, 1.0, num4);
			if (!ringRailOuter.Transform((IOnXform)(object)val3))
			{
				return false;
			}
		}
		finally
		{
			((IDisposable)val3)?.Dispose();
		}
		OnXform val5 = new OnXform();
		try
		{
			if (!GetRingRailPointExtentRight((IOnCurve)(object)ringRailOuter, out var pointExtent5))
			{
				return false;
			}
			if (!GetRingRailPointExtentLeft((IOnCurve)(object)ringRailOuter, out var pointExtent6))
			{
				return false;
			}
			double num5 = (thicknessSide * 2.0 + radius - num2) / (pointExtent5 - pointExtent6);
			val5.Scale(num5, 1.0, 1.0);
			if (!ringRailOuter.Transform((IOnXform)(object)val5))
			{
				return false;
			}
		}
		finally
		{
			((IDisposable)val5)?.Dispose();
		}
		if (!GetRingRailPointTop((IOnCurve)(object)ringRailOuter, out var point))
		{
			return false;
		}
		double num6 = -1.23432101234321E+308;
		if (!((OnCurve)ringRailOuter).GetClosestPoint(point, ref num6))
		{
			return false;
		}
		if (!((OnCurve)ringRailOuter).ChangeClosedCurveSeam(num6))
		{
			return false;
		}
		double num7 = OnUtil.ON_DotProduct((IOn3dVector)(object)((OnCurve)ringRailOuter).TangentAt(((OnCurve)ringRailOuter).Domain().Min()), OnUtil.On_xaxis);
		if (num7 == 0.0)
		{
			return false;
		}
		if (num7 < 0.0)
		{
			((OnCurve)ringRailOuter).Reverse();
		}
		return true;
	}

	public static bool CreateOutsideRingRail(double radius, Curve ringRailOuterProfile, double thicknessTop, double thicknessBottom, double thicknessSide, out NurbsCurve ringRailOuter)
	{
		//IL_002c: Unknown result type (might be due to invalid IL or missing references)
		//IL_005c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0061: Unknown result type (might be due to invalid IL or missing references)
		//IL_0079: Unknown result type (might be due to invalid IL or missing references)
		//IL_0083: Unknown result type (might be due to invalid IL or missing references)
		//IL_0099: Unknown result type (might be due to invalid IL or missing references)
		//IL_009e: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a3: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ae: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b3: Unknown result type (might be due to invalid IL or missing references)
		//IL_00df: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e4: Unknown result type (might be due to invalid IL or missing references)
		//IL_00fc: Unknown result type (might be due to invalid IL or missing references)
		//IL_0106: Unknown result type (might be due to invalid IL or missing references)
		//IL_011c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0121: Unknown result type (might be due to invalid IL or missing references)
		//IL_0126: Unknown result type (might be due to invalid IL or missing references)
		//IL_0133: Unknown result type (might be due to invalid IL or missing references)
		//IL_016d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0186: Unknown result type (might be due to invalid IL or missing references)
		//IL_018b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0190: Unknown result type (might be due to invalid IL or missing references)
		//IL_01b7: Unknown result type (might be due to invalid IL or missing references)
		//IL_01d8: Unknown result type (might be due to invalid IL or missing references)
		//IL_01dd: Unknown result type (might be due to invalid IL or missing references)
		//IL_01e6: Unknown result type (might be due to invalid IL or missing references)
		//IL_01eb: Unknown result type (might be due to invalid IL or missing references)
		double num = 0.0 - radius;
		double num2 = 0.0 - radius;
		ringRailOuter = null;
		ringRailOuter = ringRailOuterProfile.ToNurbsCurve();
		if (ringRailOuter == null)
		{
			return false;
		}
		if (!((GeometryBase)ringRailOuter).MakeDeformable())
		{
			return false;
		}
		Transform val = default(Transform);
		if (!GetRingRailPointExtentTop((Curve)(object)ringRailOuter, out var pointExtent))
		{
			return false;
		}
		if (!GetRingRailPointExtentBottom((Curve)(object)ringRailOuter, out var pointExtent2))
		{
			return false;
		}
		double num3 = (thicknessTop + radius - pointExtent2) / (pointExtent - pointExtent2);
		Plane worldXY = Plane.WorldXY;
		((Plane)(ref worldXY)).Origin = new Point3d(0.0, 0.0, pointExtent2);
		val = Transform.Scale(worldXY, 1.0, 1.0, num3);
		if (!((GeometryBase)ringRailOuter).Transform(val))
		{
			return false;
		}
		Transform unset = Transform.Unset;
		if (!GetRingRailPointExtentTop((Curve)(object)ringRailOuter, out var pointExtent3))
		{
			return false;
		}
		if (!GetRingRailPointExtentBottom((Curve)(object)ringRailOuter, out var pointExtent4))
		{
			return false;
		}
		double num4 = (thicknessBottom + pointExtent3 - num) / (pointExtent3 - pointExtent4);
		Plane worldXY2 = Plane.WorldXY;
		((Plane)(ref worldXY2)).Origin = new Point3d(0.0, 0.0, pointExtent3);
		unset = Transform.Scale(worldXY2, 1.0, 1.0, num4);
		if (!((GeometryBase)ringRailOuter).Transform(unset))
		{
			return false;
		}
		Transform val2 = default(Transform);
		if (!GetRingRailPointExtentRight((Curve)(object)ringRailOuter, out var pointExtent5))
		{
			return false;
		}
		if (!GetRingRailPointExtentLeft((Curve)(object)ringRailOuter, out var pointExtent6))
		{
			return false;
		}
		double num5 = (thicknessSide * 2.0 + radius - num2) / (pointExtent5 - pointExtent6);
		val2 = Transform.Scale(Plane.WorldXY, num5, 1.0, 1.0);
		if (!((GeometryBase)ringRailOuter).Transform(val2))
		{
			return false;
		}
		if (!GetRingRailPointTop((Curve)(object)ringRailOuter, out var point))
		{
			return false;
		}
		double num6 = -1.23432101234321E+308;
		if (!((Curve)ringRailOuter).ClosestPoint(point, ref num6))
		{
			return false;
		}
		if (!((Curve)ringRailOuter).ChangeClosedCurveSeam(num6))
		{
			return false;
		}
		NurbsCurve obj = ringRailOuter;
		Interval domain = ((Curve)ringRailOuter).Domain;
		double num7 = Vector3d.Multiply(((Curve)obj).TangentAt(((Interval)(ref domain)).Min), Vector3d.XAxis);
		if (num7 == 0.0)
		{
			return false;
		}
		if (num7 < 0.0)
		{
			((Curve)ringRailOuter).Reverse();
		}
		return true;
	}

	public static bool CreateRingRail(double radius, out OnNurbsCurve ringRail)
	{
		//IL_0009: Unknown result type (might be due to invalid IL or missing references)
		//IL_000f: Expected O, but got Unknown
		//IL_001a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0020: Expected O, but got Unknown
		//IL_0099: Unknown result type (might be due to invalid IL or missing references)
		//IL_009f: Expected O, but got Unknown
		ringRail = null;
		OnCircle val = new OnCircle(OnUtil.On_zx_plane, radius);
		if (!val.IsValid())
		{
			return false;
		}
		OnArcCurve val2 = new OnArcCurve((IOnCircle)(object)val);
		if (!((OnObject)val2).IsValid())
		{
			return false;
		}
		if (!GetRingRailPointTop((IOnCurve)(object)val2, out var point))
		{
			return false;
		}
		double num = -1.23432101234321E+308;
		if (!((OnCurve)val2).GetClosestPoint(point, ref num))
		{
			return false;
		}
		if (!((OnCurve)val2).ChangeClosedCurveSeam(num))
		{
			return false;
		}
		double num2 = OnUtil.ON_DotProduct((IOn3dVector)(object)((OnCurve)val2).TangentAt(((OnCurve)val2).Domain().Min()), OnUtil.On_xaxis);
		if (num2 == 0.0)
		{
			return false;
		}
		if (num2 < 0.0)
		{
			((OnCurve)val2).Reverse();
		}
		ringRail = new OnNurbsCurve();
		return ((OnCurve)val2).GetNurbForm(ref ringRail) > 0;
	}

	public static bool CreateRingRail(double radius, out NurbsCurve ringRail)
	{
		//IL_0009: Unknown result type (might be due to invalid IL or missing references)
		//IL_000f: Expected O, but got Unknown
		//IL_001a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0020: Expected O, but got Unknown
		//IL_0098: Unknown result type (might be due to invalid IL or missing references)
		//IL_009f: Expected O, but got Unknown
		ringRail = null;
		OnCircle val = new OnCircle(OnUtil.On_zx_plane, radius);
		if (!val.IsValid())
		{
			return false;
		}
		OnArcCurve val2 = new OnArcCurve((IOnCircle)(object)val);
		if (!((OnObject)val2).IsValid())
		{
			return false;
		}
		if (!GetRingRailPointTop((IOnCurve)(object)val2, out var point))
		{
			return false;
		}
		double num = -1.23432101234321E+308;
		if (!((OnCurve)val2).GetClosestPoint(point, ref num))
		{
			return false;
		}
		if (!((OnCurve)val2).ChangeClosedCurveSeam(num))
		{
			return false;
		}
		double num2 = OnUtil.ON_DotProduct((IOn3dVector)(object)((OnCurve)val2).TangentAt(((OnCurve)val2).Domain().Min()), OnUtil.On_xaxis);
		if (num2 == 0.0)
		{
			return false;
		}
		if (num2 < 0.0)
		{
			((OnCurve)val2).Reverse();
		}
		OnNurbsCurve val3 = new OnNurbsCurve();
		((OnCurve)val2).GetNurbForm(ref val3);
		ringRail = Interop.FromOnCurve((object)val3).ToNurbsCurve();
		return ((Curve)ringRail).GetLength() > 0.0;
	}

	public static bool GetIntersectPoint(IOnCurve curveLhs, IOnCurve curveRhs, out IOn3dPoint pt)
	{
		//IL_0007: Unknown result type (might be due to invalid IL or missing references)
		//IL_000d: Expected O, but got Unknown
		pt = OnUtil.On_UNSET_POINT;
		ArrayOnX_EVENT val = new ArrayOnX_EVENT();
		curveLhs.IntersectCurve(curveRhs, ref val);
		if ((double)val.Count() <= 0.0)
		{
			return false;
		}
		pt = (IOn3dPoint)(object)val[0][0];
		return true;
	}

	public static bool GetRingRailPointBottom(IOnCurve curve, out IOn3dPoint point)
	{
		//IL_0020: Unknown result type (might be due to invalid IL or missing references)
		//IL_002a: Expected O, but got Unknown
		//IL_0025: Unknown result type (might be due to invalid IL or missing references)
		//IL_002b: Expected O, but got Unknown
		OnLineCurve curveRhs = new OnLineCurve(OnUtil.On_origin, (IOn3dPoint)new On3dPoint(0.0, 0.0, -100.0));
		return GetIntersectPoint(curve, (IOnCurve)(object)curveRhs, out point);
	}

	public static bool GetRingRailPointBottom(Curve curve, out Point3d point)
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0020: Unknown result type (might be due to invalid IL or missing references)
		//IL_0025: Unknown result type (might be due to invalid IL or missing references)
		//IL_002b: Expected O, but got Unknown
		//IL_0045: Unknown result type (might be due to invalid IL or missing references)
		//IL_004a: Unknown result type (might be due to invalid IL or missing references)
		LineCurve val = new LineCurve(Point3d.Origin, new Point3d(0.0, 0.0, -100.0));
		CurveIntersections val2 = Intersection.CurveCurve(curve, (Curve)(object)val, CSUtil.Tolerance, CSUtil.Tolerance);
		point = val2[0].PointA;
		return ((Point3d)(ref point)).IsValid;
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
		pointExtent = ((Point3d)(ref point)).Z;
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
		pointExtent = ((Point3d)(ref point)).X;
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
		pointExtent = ((Point3d)(ref point)).X;
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
		pointExtent = ((Point3d)(ref point)).Z;
		return true;
	}

	public static bool GetRingRailPointLeft(IOnCurve curve, out IOn3dPoint point)
	{
		//IL_0020: Unknown result type (might be due to invalid IL or missing references)
		//IL_002a: Expected O, but got Unknown
		//IL_0025: Unknown result type (might be due to invalid IL or missing references)
		//IL_002b: Expected O, but got Unknown
		OnLineCurve curveRhs = new OnLineCurve(OnUtil.On_origin, (IOn3dPoint)new On3dPoint(-100.0, 0.0, 0.0));
		return GetIntersectPoint(curve, (IOnCurve)(object)curveRhs, out point);
	}

	public static bool GetRingRailPointLeft(Curve curve, out Point3d point)
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0020: Unknown result type (might be due to invalid IL or missing references)
		//IL_0025: Unknown result type (might be due to invalid IL or missing references)
		//IL_002b: Expected O, but got Unknown
		//IL_0045: Unknown result type (might be due to invalid IL or missing references)
		//IL_004a: Unknown result type (might be due to invalid IL or missing references)
		LineCurve val = new LineCurve(Point3d.Origin, new Point3d(-100.0, 0.0, 0.0));
		CurveIntersections val2 = Intersection.CurveCurve(curve, (Curve)(object)val, CSUtil.Tolerance, CSUtil.Tolerance);
		point = val2[0].PointA;
		return ((Point3d)(ref point)).IsValid;
	}

	public static bool GetRingRailPointRight(IOnCurve curve, out IOn3dPoint point)
	{
		//IL_0020: Unknown result type (might be due to invalid IL or missing references)
		//IL_002a: Expected O, but got Unknown
		//IL_0025: Unknown result type (might be due to invalid IL or missing references)
		//IL_002b: Expected O, but got Unknown
		OnLineCurve curveRhs = new OnLineCurve(OnUtil.On_origin, (IOn3dPoint)new On3dPoint(100.0, 0.0, 0.0));
		return GetIntersectPoint(curve, (IOnCurve)(object)curveRhs, out point);
	}

	public static bool GetRingRailPointRight(Curve curve, out Point3d point)
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0020: Unknown result type (might be due to invalid IL or missing references)
		//IL_0025: Unknown result type (might be due to invalid IL or missing references)
		//IL_002b: Expected O, but got Unknown
		//IL_0045: Unknown result type (might be due to invalid IL or missing references)
		//IL_004a: Unknown result type (might be due to invalid IL or missing references)
		LineCurve val = new LineCurve(Point3d.Origin, new Point3d(100.0, 0.0, 0.0));
		CurveIntersections val2 = Intersection.CurveCurve(curve, (Curve)(object)val, CSUtil.Tolerance, CSUtil.Tolerance);
		point = val2[0].PointA;
		return ((Point3d)(ref point)).IsValid;
	}

	public static bool GetRingRailPoints(IOnCurve curve, out IOn3dPoint pointTop, out IOn3dPoint pointBottom, out IOn3dPoint pointRight, out IOn3dPoint pointLeft)
	{
		return (byte)(1u & (GetRingRailPointTop(curve, out pointTop) ? 1u : 0u) & (GetRingRailPointBottom(curve, out pointBottom) ? 1u : 0u) & (GetRingRailPointRight(curve, out pointRight) ? 1u : 0u) & (GetRingRailPointLeft(curve, out pointLeft) ? 1u : 0u)) != 0;
	}

	public static bool GetRingRailPointTop(IOnCurve curve, out IOn3dPoint point)
	{
		//IL_0020: Unknown result type (might be due to invalid IL or missing references)
		//IL_002a: Expected O, but got Unknown
		//IL_0025: Unknown result type (might be due to invalid IL or missing references)
		//IL_002b: Expected O, but got Unknown
		OnLineCurve curveRhs = new OnLineCurve(OnUtil.On_origin, (IOn3dPoint)new On3dPoint(0.0, 0.0, 100.0));
		return GetIntersectPoint(curve, (IOnCurve)(object)curveRhs, out point);
	}

	public static bool GetRingRailPointTop(Curve curve, out Point3d point)
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0020: Unknown result type (might be due to invalid IL or missing references)
		//IL_0025: Unknown result type (might be due to invalid IL or missing references)
		//IL_002b: Expected O, but got Unknown
		//IL_0045: Unknown result type (might be due to invalid IL or missing references)
		//IL_004a: Unknown result type (might be due to invalid IL or missing references)
		LineCurve val = new LineCurve(Point3d.Origin, new Point3d(0.0, 0.0, 100.0));
		CurveIntersections val2 = Intersection.CurveCurve(curve, (Curve)(object)val, CSUtil.Tolerance, CSUtil.Tolerance);
		point = val2[0].PointA;
		return ((Point3d)(ref point)).IsValid;
	}
}
