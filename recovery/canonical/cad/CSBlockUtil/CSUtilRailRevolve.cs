using System;
using RMA.OpenNURBS;
using RMA.Rhino;
using Rhino.Geometry;
using Rhino.Runtime;

namespace CSBlockUtil;

public static class CSUtilRailRevolve
{
	public static OnNurbsSurface RailRevolve(IOnNurbsCurve profile, IOnNurbsCurve rail, int direction, bool zAdjust = false)
	{
		double s = new On3dPoint(profile.PointAtStart())
		{
			z = 0.0
		}.DistanceTo(OnUtil.On_origin);
		return RailRevolve(profile, rail, s, direction, zAdjust);
	}

	public static NurbsSurface RailRevolve(NurbsCurve profile, NurbsCurve rail, int direction, bool zAdjust = false)
	{
		Point3d point3d = new Point3d(profile.PointAtStart);
		point3d.Z = 0.0;
		Point3d point3d2 = point3d;
		double s = point3d2.DistanceTo(Point3d.Origin);
		return RailRevolve(profile, rail, s, direction, zAdjust);
	}

	public static OnNurbsSurface RailRevolve(IOnNurbsCurve profile, IOnNurbsCurve rail, double s1, int direction, bool zAdjust = false)
	{
		OnNurbsSurface onNurbsSurface = new OnNurbsSurface();
		if (direction == 0)
		{
			onNurbsSurface.Create(3, is_rat: true, profile.m_order, rail.m_order, profile.m_cv_count, rail.m_cv_count);
		}
		else
		{
			onNurbsSurface.Create(3, is_rat: true, rail.m_order, profile.m_order, rail.m_cv_count, profile.m_cv_count);
		}
		int num = rail.CVCount();
		int num2 = profile.CVCount();
		double z = profile.PointAtEnd().z;
		for (int i = 0; i < num2; i++)
		{
			On3dPoint vertex_value = new On3dPoint();
			profile.GetCV(i, ref vertex_value);
			On3dPoint on3dPoint = new On3dPoint(vertex_value);
			on3dPoint.z = 0.0;
			new On3dVector(on3dPoint).Unitize();
			double num3 = on3dPoint.DistanceTo(OnUtil.On_origin);
			double num4 = (zAdjust ? (1.0 - vertex_value.z / z) : 0.0);
			if (double.IsNaN(num4))
			{
				num4 = 0.0;
			}
			for (int j = 0; j < num; j++)
			{
				On3dPoint vertex_value2 = new On3dPoint();
				rail.GetCV(j, ref vertex_value2);
				vertex_value2.x = vertex_value2.x * num3 / s1;
				vertex_value2.y = vertex_value2.y * num3 / s1;
				vertex_value2.z = vertex_value.z + num4 * vertex_value2.z;
				double num5 = profile.Weight(i) * rail.Weight(j);
				if (direction == 0)
				{
					onNurbsSurface.SetCV(i, j, new On4dPoint(vertex_value2.x * num5, vertex_value2.y * num5, vertex_value2.z * num5, num5));
				}
				else
				{
					onNurbsSurface.SetCV(j, i, new On4dPoint(vertex_value2.x * num5, vertex_value2.y * num5, vertex_value2.z * num5, num5));
				}
			}
		}
		for (int k = 0; k < profile.KnotCount(); k++)
		{
			onNurbsSurface.SetKnot(direction, k, profile.Knot(k));
		}
		for (int l = 0; l < rail.KnotCount(); l++)
		{
			onNurbsSurface.SetKnot(1 - direction, l, rail.Knot(l));
		}
		return onNurbsSurface;
	}

	public static NurbsSurface RailRevolve(NurbsCurve inprofile, NurbsCurve inrail, double s1, int direction, bool zAdjust = false)
	{
		IOnCurve obj = (IOnCurve)Interop.ToOnCurve(inprofile);
		IOnNurbsCurve onNurbsCurve = null;
		onNurbsCurve = obj.NurbsCurve();
		IOnCurve obj2 = (IOnCurve)Interop.ToOnCurve(inrail);
		OnNurbsCurve onNurbsCurve2 = null;
		onNurbsCurve2 = obj2.NurbsCurve();
		OnNurbsSurface onNurbsSurface = new OnNurbsSurface();
		if (direction == 0)
		{
			onNurbsSurface.Create(3, is_rat: true, onNurbsCurve.m_order, onNurbsCurve2.m_order, onNurbsCurve.m_cv_count, onNurbsCurve2.m_cv_count);
		}
		else
		{
			onNurbsSurface.Create(3, is_rat: true, onNurbsCurve2.m_order, onNurbsCurve.m_order, onNurbsCurve2.m_cv_count, onNurbsCurve.m_cv_count);
		}
		int num = onNurbsCurve2.CVCount();
		int num2 = onNurbsCurve.CVCount();
		double z = onNurbsCurve.PointAtEnd().z;
		for (int i = 0; i < num2; i++)
		{
			On3dPoint vertex_value = new On3dPoint();
			onNurbsCurve.GetCV(i, ref vertex_value);
			On3dPoint on3dPoint = new On3dPoint(vertex_value);
			on3dPoint.z = 0.0;
			new On3dVector(on3dPoint).Unitize();
			double num3 = on3dPoint.DistanceTo(OnUtil.On_origin);
			double num4 = (zAdjust ? (1.0 - vertex_value.z / z) : 0.0);
			if (double.IsNaN(num4))
			{
				num4 = 0.0;
			}
			for (int j = 0; j < num; j++)
			{
				On3dPoint vertex_value2 = new On3dPoint();
				onNurbsCurve2.GetCV(j, ref vertex_value2);
				vertex_value2.x = vertex_value2.x * num3 / s1;
				vertex_value2.y = vertex_value2.y * num3 / s1;
				vertex_value2.z = vertex_value.z + num4 * vertex_value2.z;
				double num5 = onNurbsCurve.Weight(i) * onNurbsCurve2.Weight(j);
				if (direction == 0)
				{
					onNurbsSurface.SetCV(i, j, new On4dPoint(vertex_value2.x * num5, vertex_value2.y * num5, vertex_value2.z * num5, num5));
				}
				else
				{
					onNurbsSurface.SetCV(j, i, new On4dPoint(vertex_value2.x * num5, vertex_value2.y * num5, vertex_value2.z * num5, num5));
				}
			}
		}
		for (int k = 0; k < onNurbsCurve.KnotCount(); k++)
		{
			onNurbsSurface.SetKnot(direction, k, onNurbsCurve.Knot(k));
		}
		for (int l = 0; l < onNurbsCurve2.KnotCount(); l++)
		{
			onNurbsSurface.SetKnot(1 - direction, l, onNurbsCurve2.Knot(l));
		}
		return Interop.FromOnSurface(onNurbsSurface).ToNurbsSurface();
	}

	public static OnNurbsSurface RailRevolve(IOnNurbsCurve profile, IOnNurbsCurve rail, double s1, int direction, out OnNurbsCurve baseCurve)
	{
		OnNurbsSurface onNurbsSurface = new OnNurbsSurface();
		if (direction == 0)
		{
			onNurbsSurface.Create(3, is_rat: true, profile.m_order, rail.m_order, profile.m_cv_count, rail.m_cv_count);
		}
		else
		{
			onNurbsSurface.Create(3, is_rat: true, rail.m_order, profile.m_order, rail.m_cv_count, profile.m_cv_count);
		}
		baseCurve = new OnNurbsCurve(3, bIsRational: true, rail.m_order, rail.m_cv_count);
		double num = 0.0;
		for (int i = 0; i < profile.CVCount(); i++)
		{
			On3dPoint vertex_value = new On3dPoint();
			profile.GetCV(i, ref vertex_value);
			On3dPoint on3dPoint = new On3dPoint(vertex_value)
			{
				z = 0.0
			};
			num = Math.Max(num, on3dPoint.DistanceTo(OnUtil.On_origin));
		}
		bool flag = false;
		for (int j = 0; j < profile.CVCount(); j++)
		{
			On3dPoint vertex_value2 = new On3dPoint();
			profile.GetCV(j, ref vertex_value2);
			On3dPoint obj = new On3dPoint(vertex_value2)
			{
				z = 0.0
			};
			bool flag2 = false;
			double num2 = obj.DistanceTo(OnUtil.On_origin);
			if (!flag && Math.Abs(num2 - num) < 0.01)
			{
				flag2 = true;
				flag = true;
			}
			for (int k = 0; k < rail.CVCount(); k++)
			{
				On3dPoint vertex_value3 = new On3dPoint();
				rail.GetCV(k, ref vertex_value3);
				vertex_value3.x = vertex_value3.x * num2 / s1;
				vertex_value3.y = vertex_value3.y * num2 / s1;
				vertex_value3.z = vertex_value2.z;
				double num3 = profile.Weight(j) * rail.Weight(k);
				On4dPoint on4dPoint = new On4dPoint(vertex_value3.x * num3, vertex_value3.y * num3, vertex_value3.z * num3, num3);
				double num4 = rail.Weight(k);
				new On4dPoint(vertex_value3.x * num4, vertex_value3.y * num4, vertex_value3.z * num4, num4);
				if (direction == 0)
				{
					onNurbsSurface.SetCV(j, k, on4dPoint);
				}
				else
				{
					onNurbsSurface.SetCV(k, j, on4dPoint);
				}
				if (flag2)
				{
					baseCurve.SetCV(k, on4dPoint);
				}
			}
		}
		for (int l = 0; l < rail.KnotCount(); l++)
		{
			baseCurve.SetKnot(l, rail.Knot(l));
		}
		for (int m = 0; m < profile.KnotCount(); m++)
		{
			onNurbsSurface.SetKnot(direction, m, profile.Knot(m));
		}
		for (int n = 0; n < rail.KnotCount(); n++)
		{
			onNurbsSurface.SetKnot(1 - direction, n, rail.Knot(n));
		}
		return onNurbsSurface;
	}

	public static OnNurbsSurface RailRevolveGen(IOnNurbsCurve iprofile, IOnNurbsCurve rail, int direction, bool zAdjust = false)
	{
		OnNurbsCurve nurbs_curve = new OnNurbsCurve();
		iprofile.GetNurbForm(ref nurbs_curve);
		double num = new On3dPoint(rail.PointAtStart())
		{
			z = 0.0
		}.DistanceTo(OnUtil.On_origin);
		double t = 0.0;
		nurbs_curve.GetClosestPoint(rail.PointAtStart(), ref t);
		nurbs_curve.ChangeClosedCurveSeam(t);
		OnLineCurve curveB = new OnLineCurve(new On3dPoint(0.0, 0.0, -100.0), new On3dPoint(0.0, 0.0, 100.0));
		ArrayOnX_EVENT x = new ArrayOnX_EVENT();
		nurbs_curve.IntersectCurve(curveB, ref x);
		if (x.Count() > 1)
		{
			double num2 = 1000.0;
			double num3 = -1000.0;
			for (int i = 0; i < x.Count(); i++)
			{
				if (x[i].get_m_a(0) < num2)
				{
					num2 = x[i].get_m_a(0);
				}
				if (x[i].get_m_a(0) > num3)
				{
					num3 = x[i].get_m_a(0);
				}
			}
			OnNurbsCurve nurbs_curve2 = new OnNurbsCurve();
			nurbs_curve.GetNurbForm(ref nurbs_curve2);
			OnNurbsCurve nurbs_curve3 = new OnNurbsCurve();
			nurbs_curve.GetNurbForm(ref nurbs_curve3);
			nurbs_curve2.Trim(new OnInterval(nurbs_curve.Domain().Min(), num2));
			nurbs_curve3.Trim(new OnInterval(num3, nurbs_curve.Domain().Max()));
			OnCurve[] obj = new OnCurve[2] { nurbs_curve2, nurbs_curve3 };
			OnCurve[] output = null;
			IOnCurve[] input_curves = obj;
			RhUtil.RhinoMergeCurves(input_curves, ref output);
			if (output.Length == 0)
			{
				return null;
			}
			output[0].GetNurbForm(ref nurbs_curve);
		}
		OnNurbsSurface onNurbsSurface = new OnNurbsSurface();
		if (direction == 0)
		{
			onNurbsSurface.Create(3, is_rat: true, nurbs_curve.m_order, rail.m_order, nurbs_curve.m_cv_count, rail.m_cv_count);
		}
		else
		{
			onNurbsSurface.Create(3, is_rat: true, rail.m_order, nurbs_curve.m_order, rail.m_cv_count, nurbs_curve.m_cv_count);
		}
		int num4 = rail.CVCount();
		int num5 = nurbs_curve.CVCount();
		double z = nurbs_curve.PointAtEnd().z;
		for (int j = 0; j < num5; j++)
		{
			On3dPoint vertex_value = new On3dPoint();
			nurbs_curve.GetCV(j, ref vertex_value);
			double num6 = new On3dPoint(vertex_value)
			{
				z = 0.0
			}.DistanceTo(OnUtil.On_origin);
			double num7 = (zAdjust ? (1.0 - vertex_value.z / z) : 0.0);
			if (double.IsNaN(num7))
			{
				num7 = 0.0;
			}
			for (int k = 0; k < num4; k++)
			{
				On3dPoint vertex_value2 = new On3dPoint();
				rail.GetCV(k, ref vertex_value2);
				vertex_value2.x = vertex_value2.x * num6 / num;
				vertex_value2.y = vertex_value2.y * num6 / num;
				vertex_value2.z = vertex_value.z + num7 * vertex_value2.z;
				double num8 = nurbs_curve.Weight(j) * rail.Weight(k);
				if (direction == 0)
				{
					onNurbsSurface.SetCV(j, k, new On4dPoint(vertex_value2.x * num8, vertex_value2.y * num8, vertex_value2.z * num8, num8));
				}
				else
				{
					onNurbsSurface.SetCV(k, j, new On4dPoint(vertex_value2.x * num8, vertex_value2.y * num8, vertex_value2.z * num8, num8));
				}
			}
		}
		for (int l = 0; l < nurbs_curve.KnotCount(); l++)
		{
			onNurbsSurface.SetKnot(direction, l, nurbs_curve.Knot(l));
		}
		for (int m = 0; m < rail.KnotCount(); m++)
		{
			onNurbsSurface.SetKnot(1 - direction, m, rail.Knot(m));
		}
		return onNurbsSurface;
	}

	public static NurbsSurface RailRevolveGen(NurbsCurve iprofile, NurbsCurve irail, int direction, bool zAdjust = false)
	{
		OnNurbsCurve nurbs_curve = new OnNurbsCurve();
		((OnCurve)Interop.ToOnCurve(iprofile)).GetNurbForm(ref nurbs_curve);
		OnCurve obj = (OnCurve)Interop.ToOnCurve(irail);
		OnNurbsCurve nurbs_curve2 = new OnNurbsCurve();
		obj.GetNurbForm(ref nurbs_curve2);
		double num = new On3dPoint(nurbs_curve2.PointAtStart())
		{
			z = 0.0
		}.DistanceTo(OnUtil.On_origin);
		double t = 0.0;
		nurbs_curve.GetClosestPoint(nurbs_curve2.PointAtStart(), ref t);
		nurbs_curve.ChangeClosedCurveSeam(t);
		OnLineCurve curveB = new OnLineCurve(new On3dPoint(0.0, 0.0, -100.0), new On3dPoint(0.0, 0.0, 100.0));
		ArrayOnX_EVENT x = new ArrayOnX_EVENT();
		nurbs_curve.IntersectCurve(curveB, ref x);
		if (x.Count() > 1)
		{
			double num2 = 1000.0;
			double num3 = -1000.0;
			for (int i = 0; i < x.Count(); i++)
			{
				if (x[i].get_m_a(0) < num2)
				{
					num2 = x[i].get_m_a(0);
				}
				if (x[i].get_m_a(0) > num3)
				{
					num3 = x[i].get_m_a(0);
				}
			}
			OnNurbsCurve nurbs_curve3 = new OnNurbsCurve();
			nurbs_curve.GetNurbForm(ref nurbs_curve3);
			OnNurbsCurve nurbs_curve4 = new OnNurbsCurve();
			nurbs_curve.GetNurbForm(ref nurbs_curve4);
			nurbs_curve3.Trim(new OnInterval(nurbs_curve.Domain().Min(), num2));
			nurbs_curve4.Trim(new OnInterval(num3, nurbs_curve.Domain().Max()));
			OnCurve[] obj2 = new OnCurve[2] { nurbs_curve3, nurbs_curve4 };
			OnCurve[] output = null;
			IOnCurve[] input_curves = obj2;
			RhUtil.RhinoMergeCurves(input_curves, ref output);
			if (output.Length == 0)
			{
				return null;
			}
			output[0].GetNurbForm(ref nurbs_curve);
		}
		OnNurbsSurface onNurbsSurface = new OnNurbsSurface();
		if (direction == 0)
		{
			onNurbsSurface.Create(3, is_rat: true, nurbs_curve.m_order, nurbs_curve2.m_order, nurbs_curve.m_cv_count, nurbs_curve2.m_cv_count);
		}
		else
		{
			onNurbsSurface.Create(3, is_rat: true, nurbs_curve2.m_order, nurbs_curve.m_order, nurbs_curve2.m_cv_count, nurbs_curve.m_cv_count);
		}
		int num4 = nurbs_curve2.CVCount();
		int num5 = nurbs_curve.CVCount();
		double z = nurbs_curve.PointAtEnd().z;
		for (int j = 0; j < num5; j++)
		{
			On3dPoint vertex_value = new On3dPoint();
			nurbs_curve.GetCV(j, ref vertex_value);
			double num6 = new On3dPoint(vertex_value)
			{
				z = 0.0
			}.DistanceTo(OnUtil.On_origin);
			double num7 = (zAdjust ? (1.0 - vertex_value.z / z) : 0.0);
			if (double.IsNaN(num7))
			{
				num7 = 0.0;
			}
			for (int k = 0; k < num4; k++)
			{
				On3dPoint vertex_value2 = new On3dPoint();
				nurbs_curve2.GetCV(k, ref vertex_value2);
				vertex_value2.x = vertex_value2.x * num6 / num;
				vertex_value2.y = vertex_value2.y * num6 / num;
				vertex_value2.z = vertex_value.z + num7 * vertex_value2.z;
				double num8 = nurbs_curve.Weight(j) * nurbs_curve2.Weight(k);
				if (direction == 0)
				{
					onNurbsSurface.SetCV(j, k, new On4dPoint(vertex_value2.x * num8, vertex_value2.y * num8, vertex_value2.z * num8, num8));
				}
				else
				{
					onNurbsSurface.SetCV(k, j, new On4dPoint(vertex_value2.x * num8, vertex_value2.y * num8, vertex_value2.z * num8, num8));
				}
			}
		}
		for (int l = 0; l < nurbs_curve.KnotCount(); l++)
		{
			onNurbsSurface.SetKnot(direction, l, nurbs_curve.Knot(l));
		}
		for (int m = 0; m < nurbs_curve2.KnotCount(); m++)
		{
			onNurbsSurface.SetKnot(1 - direction, m, nurbs_curve2.Knot(m));
		}
		return Interop.FromOnSurface(onNurbsSurface).ToNurbsSurface();
	}

	public static NurbsSurface RailRevolveRC(NurbsCurve profile, NurbsCurve rail, int direction, bool zAdjust = false)
	{
		Point3d point3d = new Point3d(profile.PointAtStart);
		point3d.Z = 0.0;
		Point3d point3d2 = point3d;
		double s = point3d2.DistanceTo(Point3d.Origin);
		return RailRevolveRC(profile, rail, s, direction, zAdjust);
	}

	public static NurbsSurface RailRevolveRC(NurbsCurve profile, NurbsCurve rail, double s1, int direction, bool zAdjust = false)
	{
		NurbsSurface nurbsSurface = ((direction != 0) ? NurbsSurface.Create(3, isRational: true, rail.Order, profile.Order, rail.Points.Count, profile.Points.Count) : NurbsSurface.Create(3, isRational: true, profile.Order, rail.Order, profile.Points.Count, rail.Points.Count));
		int count = rail.Points.Count;
		int count2 = profile.Points.Count;
		double z = profile.PointAtEnd.Z;
		for (int i = 0; i < count2; i++)
		{
			Point3d location = profile.Points[i].Location;
			Point3d point3d = new Point3d(location);
			point3d.Z = 0.0;
			Point3d point = point3d;
			new Vector3d(point).Unitize();
			double num = point.DistanceTo(Point3d.Origin);
			double num2 = (zAdjust ? (1.0 - location.Z / z) : 0.0);
			if (double.IsNaN(num2))
			{
				num2 = 0.0;
			}
			for (int j = 0; j < count; j++)
			{
				Point3d location2 = rail.Points[j].Location;
				location2.X = location2.X * num / s1;
				location2.Y = location2.Y * num / s1;
				location2.Z = location.Z + num2 * location2.Z;
				double weight = profile.Points[i].Weight * rail.Points[j].Weight;
				if (direction == 0)
				{
					nurbsSurface.Points.SetControlPoint(i, j, new ControlPoint(location2, weight));
				}
				else
				{
					nurbsSurface.Points.SetControlPoint(i, j, new ControlPoint(location2, weight));
				}
			}
		}
		if (direction == 0)
		{
			for (int k = 0; k < profile.Knots.Count; k++)
			{
				nurbsSurface.KnotsU[k] = profile.Knots[k];
			}
			for (int l = 0; l < rail.Knots.Count; l++)
			{
				nurbsSurface.KnotsV[l] = rail.Knots[l];
			}
		}
		else
		{
			for (int m = 0; m < profile.Knots.Count; m++)
			{
				nurbsSurface.KnotsV[m] = profile.Knots[m];
			}
			for (int n = 0; n < rail.Knots.Count; n++)
			{
				nurbsSurface.KnotsU[n] = rail.Knots[n];
			}
		}
		return nurbsSurface;
	}

	public static OnNurbsSurface RailRevolveRhino4(IOnNurbsCurve profile, IOnNurbsCurve rail)
	{
		double s = new On3dPoint(rail.PointAtStart())
		{
			z = 0.0
		}.DistanceTo(OnUtil.On_origin);
		return RailRevolve(profile, rail, s, 1);
	}
}
