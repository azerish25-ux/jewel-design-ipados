using System;
using System.Collections.Generic;
using CSCompatibility;
using RMA.OpenNURBS;
using RMA.Rhino;
using Rhino;
using Rhino.Collections;
using Rhino.Geometry;
using Rhino.Geometry.Intersect;
using Rhino.Runtime;

namespace CSBlockUtil;

public static class CSUtilGeometry
{
	public static bool AdjustRailFromCurve(OnNurbsCurve crv, ref OnNurbsCurve rail, ref double lengthParameter, ref bool flip)
	{
		if (crv == null)
		{
			return false;
		}
		double t = rail.Domain().Min();
		bool flag = false;
		OnPlane onPlane = new OnPlane();
		if (crv.IsPlanar(onPlane))
		{
			OnPlaneSurface onPlaneSurface = new OnPlaneSurface(onPlane);
			On3dPoint fixed_point = onPlane.PointAt(0.5, 0.5);
			OnXform onXform = new OnXform();
			onXform.Scale(fixed_point, 1000.0);
			onPlaneSurface.Transform(onXform);
			ArrayOnX_EVENT x = new ArrayOnX_EVENT();
			rail.IntersectSurface(onPlaneSurface, ref x);
			if (x.Count() > 0)
			{
				int num = -1;
				double num2 = 1000.0;
				for (int i = 0; i < x.Count(); i++)
				{
					if (!x[i].IsOverlapEvent())
					{
						double num3 = x[i].get_m_pointA(0).DistanceTo(crv.PointAtStart());
						if (num3 < num2)
						{
							num = i;
							num2 = num3;
						}
					}
				}
				if (num >= 0)
				{
					t = x[num].get_m_a(0);
					flag = true;
				}
			}
		}
		if (!flag)
		{
			rail.GetClosestPoint(crv.PointAtStart(), ref t);
		}
		if (rail.IsClosed())
		{
			rail.ChangeClosedCurveSeam(t);
			if (lengthParameter < 0.0)
			{
				rail.Reverse();
				lengthParameter = 0.0 - lengthParameter;
				flip = true;
			}
		}
		else
		{
			double length = 1.0;
			rail.GetLength(ref length);
			double length2 = 1.0;
			if (lengthParameter > 0.0)
			{
				if (rail.Domain().Max() != t)
				{
					rail.Trim(new OnInterval(t, rail.Domain().Max()));
				}
				else
				{
					lengthParameter = 1.0;
				}
			}
			else if (rail.Domain().Min() != t)
			{
				rail.Trim(new OnInterval(rail.Domain().Min(), t));
				lengthParameter = 0.0 - lengthParameter;
				rail.Reverse();
				flip = true;
			}
			else
			{
				lengthParameter = 0.0;
			}
			rail.GetLength(ref length2);
			lengthParameter /= length2 / length;
		}
		return true;
	}

	public static bool AdjustRailFromCurve(NurbsCurve crv, ref NurbsCurve rail, ref double lengthParameter, ref bool flip)
	{
		if (crv == null)
		{
			return false;
		}
		double t = rail.Domain.Min;
		bool flag = false;
		if (crv.TryGetPlane(out var plane))
		{
			Transform xform = Transform.Scale(plane.PointAt(0.5, 0.5), 1000.0);
			PlaneSurface planeSurface = new PlaneSurface(plane, new Interval(0.0, 1.0), new Interval(0.0, 1.0));
			planeSurface.Transform(xform);
			CurveIntersections curveIntersections = Intersection.CurveSurface(rail, planeSurface, CSUtil.Tolerance, CSUtil.Tolerance);
			if (curveIntersections.Count > 0)
			{
				int num = -1;
				double num2 = 1000.0;
				for (int i = 0; i < curveIntersections.Count; i++)
				{
					if (!curveIntersections[i].IsOverlap)
					{
						double num3 = curveIntersections[i].PointA.DistanceTo(crv.PointAtStart);
						if (num3 < num2)
						{
							num = i;
							num2 = num3;
						}
					}
				}
				if (num >= 0)
				{
					t = curveIntersections[num].ParameterA;
					flag = true;
				}
			}
		}
		if (!flag)
		{
			rail.ClosestPoint(crv.PointAtStart, out t);
		}
		if (rail.IsClosed)
		{
			rail.ChangeClosedCurveSeam(t);
			if (lengthParameter < 0.0)
			{
				rail.Reverse();
				lengthParameter = 0.0 - lengthParameter;
				flip = true;
			}
		}
		else
		{
			double length = rail.GetLength();
			if (lengthParameter > 0.0)
			{
				if (rail.Domain.Max != t)
				{
					rail = rail.Trim(new Interval(t, rail.Domain.Max)).ToNurbsCurve();
				}
				else
				{
					lengthParameter = 1.0;
				}
			}
			else if (rail.Domain.Min != t)
			{
				rail = rail.Trim(new Interval(rail.Domain.Min, t)).ToNurbsCurve();
				lengthParameter = 0.0 - lengthParameter;
				rail.Reverse();
				flip = true;
			}
			else
			{
				lengthParameter = 0.0;
			}
			double length2 = rail.GetLength();
			lengthParameter /= length2 / length;
		}
		return true;
	}

	public static bool BuildProngDomeProfile(double diameter, double filletPer, double taperPer, out OnNurbsCurve profile)
	{
		if (filletPer > 0.99)
		{
			filletPer = 0.99;
		}
		if (filletPer < 0.01)
		{
			filletPer = 0.01;
		}
		double num = diameter / 2.0;
		double num2 = num * filletPer;
		profile = new OnNurbsCurve();
		if (!profile.Create(3, bIsRational: true, 3, 5))
		{
			return false;
		}
		profile.SetCV(0, new On4dPoint(num, 0.0, 0.0, 1.0));
		double num3 = 0.7071067811;
		double num4 = num * (1.0 - taperPer);
		profile.SetCV(1, new On4dPoint(num4 * num3, 0.0, num2 * num3, num3));
		profile.SetCV(2, new On4dPoint(num - num2, 0.0, num2, 1.0));
		profile.SetCV(3, new On4dPoint((num - num2) / 2.0, 0.0, num2, 1.0));
		profile.SetCV(4, new On4dPoint(0.0, 0.0, num2, 1.0));
		profile.SetKnot(0, 0.0);
		profile.SetKnot(1, 0.0);
		profile.SetKnot(2, 0.5);
		profile.SetKnot(3, 0.5);
		profile.SetKnot(4, 1.0);
		profile.SetKnot(5, 1.0);
		return true;
	}

	public static bool BuildProngProfile(double diameter, double height, double filletPer, double taperPer, out OnNurbsCurve profile)
	{
		if (filletPer > 0.99)
		{
			filletPer = 0.99;
		}
		if (filletPer < 0.01)
		{
			filletPer = 0.01;
		}
		double num = diameter / 2.0;
		double num2 = num * (1.0 + taperPer);
		double num3 = num * filletPer;
		if (num3 > height)
		{
			height = num3 + 0.01;
		}
		double num4 = height - num3;
		profile = new OnNurbsCurve();
		if (!profile.Create(3, bIsRational: true, 3, 9))
		{
			return false;
		}
		profile.SetCV(0, new On4dPoint(0.0, 0.0, 0.0, 1.0));
		profile.SetCV(1, new On4dPoint(num2 / 2.0, 0.0, 0.0, 1.0));
		profile.SetCV(2, new On4dPoint(num2, 0.0, 0.0, 1.0));
		profile.SetCV(3, new On4dPoint((num2 + num) / 2.0, 0.0, num4 / 2.0, 1.0));
		profile.SetCV(4, new On4dPoint(num, 0.0, num4, 1.0));
		double num5 = 0.7071067811;
		double num6 = num2 + (num - num2) / num4 * height;
		double num7 = height;
		profile.SetCV(5, new On4dPoint(num6 * num5, 0.0, num7 * num5, num5));
		profile.SetCV(6, new On4dPoint(num - num3, 0.0, height, 1.0));
		profile.SetCV(7, new On4dPoint((num - num3) / 2.0, 0.0, height, 1.0));
		profile.SetCV(8, new On4dPoint(0.0, 0.0, height, 1.0));
		profile.SetKnot(0, 0.0);
		profile.SetKnot(1, 0.0);
		profile.SetKnot(2, 0.25);
		profile.SetKnot(3, 0.25);
		profile.SetKnot(4, 0.5);
		profile.SetKnot(5, 0.5);
		profile.SetKnot(6, 0.75);
		profile.SetKnot(7, 0.75);
		profile.SetKnot(8, 1.0);
		profile.SetKnot(9, 1.0);
		return true;
	}

	public static bool BuildProngProfileNew(double diameter, double height, double filletPer, double taperPer, out OnNurbsCurve profile, double bottomPercent = 0.0)
	{
		if (filletPer > 0.99)
		{
			filletPer = 0.99;
		}
		if (filletPer < 0.01)
		{
			filletPer = 0.01;
		}
		double num = diameter / 2.0;
		double num2 = num * (1.0 + taperPer);
		double num3 = num * filletPer;
		if (num3 > height)
		{
			height = num3 + 0.01;
		}
		double num4 = height - num3;
		double num5 = num3;
		profile = new OnNurbsCurve();
		if (!profile.Create(3, bIsRational: true, 3, 11))
		{
			return false;
		}
		double num6 = 0.7071067811;
		double num7 = num2 + (num - num2) / num4 * height;
		double num8 = height;
		profile.SetCV(0, new On4dPoint(0.0, 0.0, 0.0, 1.0));
		profile.SetCV(1, new On4dPoint((num2 - num3) / 2.0, 0.0, 0.0, 1.0));
		profile.SetCV(2, new On4dPoint(num2 - num3, 0.0, 0.0, 1.0));
		profile.SetCV(3, new On4dPoint(num2 * num6, 0.0, 0.0, num6));
		profile.SetCV(4, new On4dPoint(num2, 0.0, num5 * bottomPercent, 1.0));
		profile.SetCV(5, new On4dPoint((num2 + num) / 2.0, 0.0, (num5 + num4) / 2.0, 1.0));
		profile.SetCV(6, new On4dPoint(num, 0.0, num4, 1.0));
		profile.SetCV(7, new On4dPoint(num7 * num6, 0.0, num8 * num6, num6));
		profile.SetCV(8, new On4dPoint(num - num3, 0.0, height, 1.0));
		profile.SetCV(9, new On4dPoint((num - num3) / 2.0, 0.0, height, 1.0));
		profile.SetCV(10, new On4dPoint(0.0, 0.0, height, 1.0));
		profile.SetKnot(0, 0.0);
		profile.SetKnot(1, 0.0);
		profile.SetKnot(2, 0.25);
		profile.SetKnot(3, 0.25);
		profile.SetKnot(4, 0.5);
		profile.SetKnot(5, 0.5);
		profile.SetKnot(6, 0.75);
		profile.SetKnot(7, 0.75);
		profile.SetKnot(8, 1.0);
		profile.SetKnot(9, 1.0);
		profile.SetKnot(10, 1.25);
		profile.SetKnot(11, 1.25);
		return true;
	}

	public static OnCircle CalculateApolloniusCircle(IOnCircle c1, IOnCircle c2, IOnCircle c3)
	{
		OnPlane onPlane = new OnPlane(c1.Center(), c2.Center(), c3.Center());
		double num = 1.0;
		if (new On3dVector(c1.Normal()) * onPlane.Normal() < 0.0)
		{
			num = -1.0;
		}
		double u = 0.0;
		double v = 0.0;
		onPlane.ClosestPointTo(c1.Center(), ref u, ref v);
		On3dPoint on3dPoint = new On3dPoint(u, v, 0.0);
		onPlane.ClosestPointTo(c2.Center(), ref u, ref v);
		On3dPoint on3dPoint2 = new On3dPoint(u, v, 0.0);
		onPlane.ClosestPointTo(c3.Center(), ref u, ref v);
		On3dPoint on3dPoint3 = new On3dPoint(u, v, 0.0);
		double x = on3dPoint.x;
		double y = on3dPoint.y;
		double x2 = on3dPoint2.x;
		double y2 = on3dPoint2.y;
		double x3 = on3dPoint3.x;
		double y3 = on3dPoint3.y;
		double radius = c1.radius;
		double radius2 = c2.radius;
		double radius3 = c3.radius;
		double num2 = 2.0 * (x - x2);
		double num3 = 2.0 * (y - y2);
		double num4 = 2.0 * (radius - radius2);
		double num5 = x * x + y * y - radius * radius - x2 * x2 - y2 * y2 + radius2 * radius2;
		double num6 = 2.0 * (x - x3);
		double num7 = 2.0 * (y - y3);
		double num8 = 2.0 * (radius - radius3);
		double num9 = x * x + y * y - radius * radius - x3 * x3 - y3 * y3 + radius3 * radius3;
		double num10 = num2 * num7 - num3 * num6;
		double num11 = (num7 * num5 - num3 * num9) / num10 - x;
		double num12 = ((0.0 - num7) * num4 + num3 * num8) / num10;
		double num13 = ((0.0 - num6) * num5 + num2 * num9) / num10 - y;
		double num14 = (num6 * num4 - num2 * num8) / num10;
		double num15 = num11 * num11 + num13 * num13 - radius * radius;
		double num16 = 2.0 * num11 * num12 + 2.0 * num13 * num14 - 2.0 * radius;
		double num17 = num12 * num12 + num14 * num14 - 1.0;
		double num18 = (0.0 - num16 - Math.Sqrt(num16 * num16 - 4.0 * (num17 * num15))) / (2.0 * num17);
		double u2 = (num7 * num5 - num3 * num9 - num7 * num4 * num18 + num3 * num8 * num18) / (num2 * num7 - num3 * num6);
		double v2 = ((0.0 - num6) * num5 + num2 * num9 + num6 * num4 * num18 - num2 * num8 * num18) / (num2 * num7 - num6 * num3);
		return new OnCircle(new OnPlane(onPlane.PointAt(u2, v2), new On3dVector(onPlane.Normal()) * num), onPlane.PointAt(u2, v2), num18);
	}

	public static OnNurbsCurve ChannelAdjustEndProfile(OnCurve curve, double amount)
	{
		OnNurbsCurve nurbs_curve = new OnNurbsCurve();
		curve.GetNurbForm(ref nurbs_curve);
		nurbs_curve.MakeDeformable();
		OnBoundingBox tight_bbox = new OnBoundingBox();
		nurbs_curve.GetTightBoundingBox(ref tight_bbox);
		On3dPoint vertex_value = new On3dPoint();
		if (tight_bbox.Center().x < 0.0)
		{
			for (int i = 1; i < nurbs_curve.CVCount() - 1; i++)
			{
				nurbs_curve.GetCV(i, ref vertex_value);
				double num = 1.0 - (vertex_value.x - tight_bbox.Min().x) / (tight_bbox.Max().x - tight_bbox.Min().x);
				vertex_value.y += (tight_bbox.Max().y - vertex_value.y) * num * amount;
				nurbs_curve.SetCV(i, vertex_value);
			}
		}
		else
		{
			for (int j = 1; j < nurbs_curve.CVCount() - 1; j++)
			{
				nurbs_curve.GetCV(j, ref vertex_value);
				double num2 = (vertex_value.x - tight_bbox.Min().x) / (tight_bbox.Max().x - tight_bbox.Min().x);
				vertex_value.y += (tight_bbox.Max().y - vertex_value.y) * num2 * amount;
				nurbs_curve.SetCV(j, vertex_value);
			}
		}
		nurbs_curve.RemoveShortSegments(0.001, bRemoveShortSegments: true);
		return nurbs_curve;
	}

	public static OnNurbsCurve CreateDome(double domeAmount, double radius = 0.5)
	{
		OnNurbsCurve nurbs_curve = new OnNurbsCurve();
		if (Math.Abs(domeAmount) < 0.005)
		{
			new OnLineCurve(new On3dPoint(radius, 0.0, 0.0), OnUtil.On_origin).GetNurbForm(ref nurbs_curve);
		}
		else
		{
			nurbs_curve.Create(3, bIsRational: true, 3, 5);
			nurbs_curve.SetCV(0, new On4dPoint(radius, 0.0, 0.0, 1.0));
			nurbs_curve.SetCV(1, new On4dPoint(radius * 0.70710678, 0.0, domeAmount * 0.70710678, 0.70710678));
			nurbs_curve.SetCV(2, new On4dPoint(radius / 2.0, 0.0, domeAmount, 1.0));
			nurbs_curve.SetCV(3, new On4dPoint(radius / 3.0, 0.0, domeAmount, 1.0));
			nurbs_curve.SetCV(4, new On4dPoint(0.0, 0.0, domeAmount, 1.0));
			nurbs_curve.SetKnot(0, 0.0);
			nurbs_curve.SetKnot(1, 0.0);
			nurbs_curve.SetKnot(2, 1.0);
			nurbs_curve.SetKnot(3, 1.0);
			nurbs_curve.SetKnot(4, 2.0);
			nurbs_curve.SetKnot(5, 2.0);
		}
		return nurbs_curve;
	}

	public static NurbsCurve CreateDomeRC(double domeAmount, double radius = 0.5)
	{
		if (Math.Abs(domeAmount) < 0.005)
		{
			return new LineCurve(new Point3d(radius, 0.0, 0.0), Point3d.Origin).ToNurbsCurve();
		}
		NurbsCurve nurbsCurve = new NurbsCurve(3, rational: true, 3, 5);
		nurbsCurve.Points.SetPoint(0, new Point4d(radius, 0.0, 0.0, 1.0));
		nurbsCurve.Points.SetPoint(1, new Point4d(radius, 0.0, domeAmount, 0.70710678));
		nurbsCurve.Points.SetPoint(2, new Point4d(radius / 2.0, 0.0, domeAmount, 1.0));
		nurbsCurve.Points.SetPoint(3, new Point4d(radius / 3.0, 0.0, domeAmount, 1.0));
		nurbsCurve.Points.SetPoint(4, new Point4d(0.0, 0.0, domeAmount, 1.0));
		nurbsCurve.Knots[0] = 0.0;
		nurbsCurve.Knots[1] = 0.0;
		nurbsCurve.Knots[2] = 1.0;
		nurbsCurve.Knots[3] = 1.0;
		nurbsCurve.Knots[4] = 2.0;
		nurbsCurve.Knots[5] = 2.0;
		return nurbsCurve;
	}

	[Obsolete]
	public static bool CreateMeshProng(IOnNurbsCurve rail, double diameter, double height, double filletPer, double taperPer, out OnMesh meshSurface)
	{
		meshSurface = null;
		if (!BuildProngProfile(diameter, height, filletPer, taperPer, out var profile))
		{
			return false;
		}
		BuildProngRail(rail, out var railActual);
		RhUtilHelper.RhinoConvertCurveToPolyline(profile, 5, 5, 3.14, 1, 1.0, 0.02, 0.02, 0.4, out var polylineCurve);
		RhUtilHelper.RhinoConvertCurveToPolyline(railActual, 5, 5, 3.14, 1, 1.0, 0.02, 0.02, 0.4, out var polylineCurve2);
		int num = polylineCurve.PointCount();
		int num2 = polylineCurve2.PointCount() - 1;
		int num3 = (num - 2) * num2 + 2;
		int num4 = num2 * (num - 1);
		OnMesh onMesh = new OnMesh(num4, num3, has_vertex_normals: false, has_texture_coords: false);
		int num5 = 0;
		for (int i = 1; i < num - 1; i++)
		{
			On3dPoint on3dPoint = new On3dPoint(polylineCurve.m_pline[i]);
			double num6 = new On3dVector(on3dPoint)
			{
				z = 0.0
			}.Length();
			for (int j = 0; j < num2; j++)
			{
				On3dPoint on3dPoint2 = new On3dPoint(polylineCurve2.m_pline[j]);
				on3dPoint2.x *= num6;
				on3dPoint2.y *= num6;
				on3dPoint2.z = on3dPoint.z;
				onMesh.SetVertex(num5++, on3dPoint2);
			}
		}
		int vertex_index = num5;
		onMesh.SetVertex(num5++, polylineCurve.m_pline[0]);
		int vertex_index2 = num5;
		onMesh.SetVertex(num5++, polylineCurve.m_pline[num - 1]);
		int num7 = 0;
		for (int k = 0; k < num2; k++)
		{
			onMesh.SetTriangle(num7++, vertex_index, k, (k + 1) % num2);
		}
		for (int l = 0; l < num - 3; l++)
		{
			int num8 = num2 * l;
			int num9 = num2 * (l + 1);
			for (int m = 0; m < num2; m++)
			{
				onMesh.SetQuad(num7++, num8 + m, num9 + m, num9 + (m + 1) % num2, num8 + (m + 1) % num2);
			}
		}
		int num10 = num2 * (num - 3);
		for (int n = 0; n < num2; n++)
		{
			onMesh.SetTriangle(num7++, num10 + n, vertex_index2, num10 + (n + 1) % num2);
		}
		if (num5 != num3)
		{
			RhinoApp.Write("Mesh Vertex Count incorrect. Expected " + num3 + " made " + num5 + "\n");
		}
		if (num7 != num4)
		{
			RhinoApp.Write("Mesh face Count incorrect. Expected " + num4 + " made " + num7 + "\n");
		}
		onMesh.ComputeVertexNormals();
		onMesh.Compact();
		OnTextureMapping onTextureMapping = new OnTextureMapping();
		onTextureMapping.EvaluateSphereMapping(onMesh.BoundingBox().Center(), OnUtil.On_xaxis, new On3dPoint(OnUtil.On_origin));
		onMesh.SetTextureCoordinates(onTextureMapping);
		if (!onMesh.IsValid())
		{
			return false;
		}
		meshSurface = onMesh;
		return true;
	}

	public static bool CreateMeshProng(IOnNurbsCurve rail, double diameter, double height, double filletPer, double taperPer, out OnMesh meshSurface, out OnPolyline outRailLine)
	{
		meshSurface = null;
		outRailLine = null;
		if (!BuildProngProfile(diameter, height, filletPer, taperPer, out var profile))
		{
			return false;
		}
		outRailLine = new OnPolyline();
		BuildProngRail(rail, out var railActual);
		RhUtilHelper.RhinoConvertCurveToPolyline(profile, 5, 5, 3.14, 1, 1.0, 0.02, 0.02, 0.4, out var polylineCurve);
		RhUtilHelper.RhinoConvertCurveToPolyline(railActual, 5, 5, 3.14, 1, 1.0, 0.02, 0.02, 0.4, out var polylineCurve2);
		int num = polylineCurve.PointCount();
		int num2 = polylineCurve2.PointCount() - 1;
		int initial_vertex_capacity = (num - 2) * num2 + 2;
		OnMesh onMesh = new OnMesh(num2 * (num - 1), initial_vertex_capacity, has_vertex_normals: false, has_texture_coords: false);
		int num3 = 0;
		for (int i = 1; i < num - 1; i++)
		{
			On3dPoint on3dPoint = new On3dPoint(polylineCurve.m_pline[i]);
			double num4 = new On3dVector(on3dPoint)
			{
				z = 0.0
			}.Length();
			bool flag = false;
			if (outRailLine.Count() < 1 && Math.Abs(num4 - diameter / 2.0) < 0.01)
			{
				flag = true;
			}
			for (int j = 0; j < num2; j++)
			{
				On3dPoint on3dPoint2 = new On3dPoint(polylineCurve2.m_pline[j]) * num4;
				on3dPoint2.z = on3dPoint.z;
				onMesh.SetVertex(num3++, on3dPoint2);
				if (flag)
				{
					outRailLine.Append(on3dPoint2);
				}
			}
			if (flag && polylineCurve2.IsClosed())
			{
				outRailLine.Append(outRailLine[0]);
			}
		}
		int vertex_index = num3;
		onMesh.SetVertex(num3++, polylineCurve.m_pline[0]);
		int vertex_index2 = num3;
		onMesh.SetVertex(num3++, polylineCurve.m_pline[num - 1]);
		int num5 = 0;
		for (int k = 0; k < num2; k++)
		{
			onMesh.SetTriangle(num5++, vertex_index, k, (k + 1) % num2);
		}
		for (int l = 0; l < num - 3; l++)
		{
			int num6 = num2 * l;
			int num7 = num2 * (l + 1);
			for (int m = 0; m < num2; m++)
			{
				onMesh.SetQuad(num5++, num6 + m, num7 + m, num7 + (m + 1) % num2, num6 + (m + 1) % num2);
			}
		}
		int num8 = num2 * (num - 3);
		for (int n = 0; n < num2; n++)
		{
			onMesh.SetTriangle(num5++, num8 + n, vertex_index2, num8 + (n + 1) % num2);
		}
		onMesh.ComputeVertexNormals();
		onMesh.Compact();
		meshSurface = onMesh;
		return true;
	}

	public static bool CreateMeshProngNew(IOnNurbsCurve rail, double diameter, double height, double filletPer, double taperPer, out OnMesh meshSurface, out OnPolyline outRailLine, double bottomPercent = 0.0)
	{
		meshSurface = null;
		outRailLine = null;
		if (!BuildProngProfileNew(diameter, height, filletPer, taperPer, out var profile, bottomPercent))
		{
			return false;
		}
		outRailLine = new OnPolyline();
		BuildProngRail(rail, out var railActual);
		RhUtilHelper.RhinoConvertCurveToPolyline(profile, 5, 5, 3.14, 1, 1.0, 0.02, 0.02, 0.4, out var polylineCurve);
		RhUtilHelper.RhinoConvertCurveToPolyline(railActual, 5, 5, 3.14, 1, 1.0, 0.02, 0.02, 0.4, out var polylineCurve2);
		int num = polylineCurve.PointCount();
		int num2 = polylineCurve2.PointCount() - 1;
		int initial_vertex_capacity = (num - 2) * num2 + 2;
		OnMesh onMesh = new OnMesh(num2 * (num - 1), initial_vertex_capacity, has_vertex_normals: false, has_texture_coords: false);
		int num3 = 0;
		for (int i = 1; i < num - 1; i++)
		{
			On3dPoint on3dPoint = new On3dPoint(polylineCurve.m_pline[i]);
			double num4 = new On3dVector(on3dPoint)
			{
				z = 0.0
			}.Length();
			bool flag = false;
			if (outRailLine.Count() < 1 && Math.Abs(num4 - diameter / 2.0) < 0.01)
			{
				flag = true;
			}
			for (int j = 0; j < num2; j++)
			{
				On3dPoint on3dPoint2 = new On3dPoint(polylineCurve2.m_pline[j]) * num4;
				on3dPoint2.z = on3dPoint.z;
				onMesh.SetVertex(num3++, on3dPoint2);
				if (flag)
				{
					outRailLine.Append(on3dPoint2);
				}
			}
			if (flag && polylineCurve2.IsClosed())
			{
				outRailLine.Append(outRailLine[0]);
			}
		}
		int vertex_index = num3;
		onMesh.SetVertex(num3++, polylineCurve.m_pline[0]);
		int vertex_index2 = num3;
		onMesh.SetVertex(num3++, polylineCurve.m_pline[num - 1]);
		int num5 = 0;
		for (int k = 0; k < num2; k++)
		{
			onMesh.SetTriangle(num5++, vertex_index, k, (k + 1) % num2);
		}
		for (int l = 0; l < num - 3; l++)
		{
			int num6 = num2 * l;
			int num7 = num2 * (l + 1);
			for (int m = 0; m < num2; m++)
			{
				onMesh.SetQuad(num5++, num6 + m, num7 + m, num7 + (m + 1) % num2, num6 + (m + 1) % num2);
			}
		}
		int num8 = num2 * (num - 3);
		for (int n = 0; n < num2; n++)
		{
			onMesh.SetTriangle(num5++, num8 + n, vertex_index2, num8 + (n + 1) % num2);
		}
		onMesh.ComputeVertexNormals();
		onMesh.Compact();
		meshSurface = onMesh;
		return true;
	}

	[Obsolete]
	public static bool CreateProng(IOnNurbsCurve rail, double diameter, double height, double filletPer, double taperPer, out OnNurbsSurface nurbsSurface)
	{
		nurbsSurface = null;
		if (!BuildProngProfile(diameter, height, filletPer, taperPer, out var profile))
		{
			return false;
		}
		BuildProngRail(rail, out var railActual);
		nurbsSurface = CSUtilRailRevolve.RailRevolve(profile, railActual, 1.0, 1);
		return true;
	}

	public static bool CreateProng(IOnNurbsCurve rail, double diameter, double height, double filletPer, double taperPer, out OnNurbsSurface nurbsSurface, out OnNurbsCurve outputRail)
	{
		nurbsSurface = null;
		outputRail = null;
		if (!BuildProngProfile(diameter, height, filletPer, taperPer, out var profile))
		{
			return false;
		}
		BuildProngRail(rail, out var railActual);
		nurbsSurface = CSUtilRailRevolve.RailRevolve(profile, railActual, 1.0, 1, out outputRail);
		return true;
	}

	[Obsolete]
	public static bool CreateProng_Old_V1(double diameter, double height, double filletPer, double taperPer, ref OnNurbsSurface nurbsSurface)
	{
		return CreateProng_Old_V1(new On3dVector(OnUtil.On_zaxis), diameter, height, filletPer, taperPer, ref nurbsSurface);
	}

	[Obsolete]
	public static bool CreateProng_Old_V1(IOn3dVector direction, double diameter, double height, double filletPer, double taperPer, ref OnNurbsSurface nurbsSurface)
	{
		if (filletPer > 0.99)
		{
			filletPer = 0.99;
		}
		if (filletPer < 0.01)
		{
			filletPer = 0.01;
		}
		double num = diameter / 2.0;
		double num2 = num * (1.0 - taperPer);
		double num3 = num2 * filletPer;
		if (num3 > height)
		{
			height = num3 + 0.01;
		}
		double num4 = height - num3;
		double[,] array = new double[9, 2]
		{
			{ 0.0, 0.0 },
			{
				num / 2.0,
				0.0
			},
			{ num, 0.0 },
			{
				(num + num2) / 2.0,
				num4 / 2.0
			},
			{ num2, num4 },
			{
				num + (num2 - num) / num4 * height,
				height
			},
			{
				num2 - num3,
				height
			},
			{
				(num2 - num3) / 2.0,
				height
			},
			{ 0.0, height }
		};
		nurbsSurface = new OnNurbsSurface();
		if (!nurbsSurface.Create(3, is_rat: true, 3, 3, 9, 9))
		{
			return false;
		}
		for (int i = 0; i < 9; i++)
		{
			double num5;
			double num6;
			if (i == 5)
			{
				num5 = 0.7071067811;
				num6 = 0.5;
			}
			else
			{
				num5 = 1.0;
				num6 = 0.7071067811;
			}
			On4dPoint cv = new On4dPoint((0.0 - array[i, 0]) * num5, 0.0 * num5, array[i, 1] * num5, num5);
			nurbsSurface.SetCV(0, i, cv);
			cv = new On4dPoint((0.0 - array[i, 0]) * num6, (0.0 - array[i, 0]) * num6, array[i, 1] * num6, num6);
			nurbsSurface.SetCV(1, i, cv);
			cv = new On4dPoint(0.0 * num5, (0.0 - array[i, 0]) * num5, array[i, 1] * num5, num5);
			nurbsSurface.SetCV(2, i, cv);
			cv = new On4dPoint(array[i, 0] * num6, (0.0 - array[i, 0]) * num6, array[i, 1] * num6, num6);
			nurbsSurface.SetCV(3, i, cv);
			cv = new On4dPoint(array[i, 0] * num5, 0.0 * num5, array[i, 1] * num5, num5);
			nurbsSurface.SetCV(4, i, cv);
			cv = new On4dPoint(array[i, 0] * num6, array[i, 0] * num6, array[i, 1] * num6, num6);
			nurbsSurface.SetCV(5, i, cv);
			cv = new On4dPoint(0.0 * num5, array[i, 0] * num5, array[i, 1] * num5, num5);
			nurbsSurface.SetCV(6, i, cv);
			cv = new On4dPoint((0.0 - array[i, 0]) * num6, array[i, 0] * num6, array[i, 1] * num6, num6);
			nurbsSurface.SetCV(7, i, cv);
			cv = new On4dPoint((0.0 - array[i, 0]) * num5, 0.0 * num5, array[i, 1] * num5, num5);
			nurbsSurface.SetCV(8, i, cv);
		}
		nurbsSurface.SetKnot(0, 0, 0.0);
		nurbsSurface.SetKnot(0, 1, 0.0);
		nurbsSurface.SetKnot(0, 2, 0.25);
		nurbsSurface.SetKnot(0, 3, 0.25);
		nurbsSurface.SetKnot(0, 4, 0.5);
		nurbsSurface.SetKnot(0, 5, 0.5);
		nurbsSurface.SetKnot(0, 6, 0.75);
		nurbsSurface.SetKnot(0, 7, 0.75);
		nurbsSurface.SetKnot(0, 8, 1.0);
		nurbsSurface.SetKnot(0, 9, 1.0);
		nurbsSurface.SetKnot(1, 0, 0.0);
		nurbsSurface.SetKnot(1, 1, 0.0);
		nurbsSurface.SetKnot(1, 2, 0.25);
		nurbsSurface.SetKnot(1, 3, 0.25);
		nurbsSurface.SetKnot(1, 4, 0.5);
		nurbsSurface.SetKnot(1, 5, 0.5);
		nurbsSurface.SetKnot(1, 6, 0.75);
		nurbsSurface.SetKnot(1, 7, 0.75);
		nurbsSurface.SetKnot(1, 8, 1.0);
		nurbsSurface.SetKnot(1, 9, 1.0);
		OnPlane plane = new OnPlane(OnUtil.On_origin, OnUtil.On_zaxis);
		OnPlane plane2 = new OnPlane(OnUtil.On_origin, direction);
		OnXform onXform = new OnXform();
		onXform.ChangeBasis(plane2, plane);
		nurbsSurface.Transform(onXform);
		return true;
	}

	public static bool CreateProngDome(IOnNurbsCurve rail, double diameter, double filletPer, double taperPer, out OnNurbsSurface dome)
	{
		dome = null;
		if (!BuildProngDomeProfile(diameter, filletPer, taperPer, out var profile))
		{
			return false;
		}
		BuildProngRail(rail, out var railActual);
		dome = CSUtilRailRevolve.RailRevolve(profile, railActual, 1.0, 1);
		return true;
	}

	public static bool CreateProngNew(IOnNurbsCurve rail, double diameter, double height, double filletPer, double taperPer, double bottomPercentage, out OnNurbsSurface nurbsSurface, out OnNurbsCurve outputRail)
	{
		nurbsSurface = null;
		outputRail = null;
		if (!BuildProngProfileNew(diameter, height, filletPer, taperPer, out var profile, bottomPercentage))
		{
			return false;
		}
		BuildProngRail(rail, out var railActual);
		nurbsSurface = CSUtilRailRevolve.RailRevolve(profile, railActual, 1.0, 1, out outputRail);
		return true;
	}

	public static OnNurbsSurface ExtrudeSurface(OnNurbsCurve crv, IOn3dVector vector, double extrudeDistance, bool bothSides)
	{
		OnNurbsCurve onNurbsCurve = crv.Duplicate();
		OnNurbsCurve onNurbsCurve2 = crv.Duplicate();
		onNurbsCurve.Translate(new On3dVector(vector) * extrudeDistance);
		if (bothSides)
		{
			onNurbsCurve2.Translate(new On3dVector(vector) * (0.0 - extrudeDistance));
		}
		OnNurbsSurface onNurbsSurface = new OnNurbsSurface();
		onNurbsSurface.CreateRuledSurface(onNurbsCurve, onNurbsCurve2);
		return onNurbsSurface;
	}

	public static void FixBrepSolidOrientation(ref Brep brep, bool doFullClean = false)
	{
		OnBrep brep2 = brep.Convert();
		FixBrepSolidOrientation(ref brep2, doFullClean);
		brep = brep2.Convert();
	}

	public static void FixBrepSolidOrientation(ref OnBrep brep, bool doFullClean = false)
	{
		if (brep != null && brep.IsValid())
		{
			brep.SplitKinkyFaces();
			if (doFullClean)
			{
				brep.FlipReversedSurfaces();
				brep.ShrinkSurfaces();
				brep.Compact();
			}
			if (-1 == brep.SolidOrientation())
			{
				brep.Flip();
			}
			if (doFullClean)
			{
				brep.ClearBoundingBox();
				brep.SetVertexTolerances();
				brep.SetTrimIsoFlags();
				brep.SetTrimTypeFlags();
				brep.SetTrimTolerances();
				brep.SetTrimBoundingBoxes();
			}
		}
	}

	public static bool GetClosestNormalToBrep(IOnBrep brep, IOn3dPoint point, ref On3dVector brepNormal)
	{
		if (!GetClosestPointToBrep(brep, point, out var faceIndex, out var s, out var t))
		{
			return false;
		}
		brepNormal = brep.Face(faceIndex).NormalAt(s, t);
		if (brep.m_F[faceIndex].m_bRev)
		{
			brepNormal.Reverse();
		}
		return brepNormal.IsValid();
	}

	public static bool GetClosestNormalToBrep(Brep brep, Point3d testPoint, ref Vector3d brepNormal)
	{
		if (!GetClosestPointToBrep(brep, testPoint, out var faceIndex, out var u, out var v))
		{
			return false;
		}
		brepNormal = brep.Faces[faceIndex].NormalAt(u, v);
		OnBrep brep2 = (OnBrep)Interop.ToOnBrep(brep);
		On3dPoint point = CSConvert.Point3dCopyToOn3dPoint(testPoint);
		On3dVector brepNormal2 = new On3dVector(OnUtil.On_UNSET_VECTOR);
		GetClosestNormalToBrep(brep2, point, ref brepNormal2);
		Vector3d other = CSConvert.On3dVectorCopyToVector3d(brepNormal2);
		if (!brepNormal.EpsilonEquals(other, CSUtil.Tolerance))
		{
			CSUtil.PrintLine("GetClosestNormalToBrep ERROR");
		}
		return brepNormal.IsValid;
	}

	public static bool GetClosestPointToBrep(IOnBrep brep, IOn3dPoint point, ref On3dPoint brepPoint, ref On3dVector brepNormal)
	{
		if (!GetClosestPointToBrep(brep, point, out var faceIndex, out var s, out var t))
		{
			return false;
		}
		brepPoint = brep.Face(faceIndex).PointAt(s, t);
		brepNormal = brep.Face(faceIndex).NormalAt(s, t);
		if (brep.m_F[faceIndex].m_bRev)
		{
			brepNormal.Reverse();
		}
		if (brepPoint.IsValid())
		{
			return brepNormal.IsValid();
		}
		return false;
	}

	public static bool GetClosestPointToBrep(Brep brep, Point3d testPoint, ref Point3d brepPoint, ref Vector3d brepNormal)
	{
		if (!GetClosestPointToBrep(brep, testPoint, out var faceIndex, out var u, out var v))
		{
			return false;
		}
		brepPoint = brep.Faces[faceIndex].PointAt(u, v);
		brepNormal = brep.Faces[faceIndex].NormalAt(u, v);
		OnBrep brep2 = (OnBrep)Interop.ToOnBrep(brep);
		On3dPoint point = CSConvert.Point3dCopyToOn3dPoint(testPoint);
		On3dVector brepNormal2 = new On3dVector(OnUtil.On_UNSET_VECTOR);
		GetClosestNormalToBrep(brep2, point, ref brepNormal2);
		Vector3d other = CSConvert.On3dVectorCopyToVector3d(brepNormal2);
		if (!brepNormal.EpsilonEquals(other, CSUtil.Tolerance))
		{
			CSUtil.PrintLine("GetClosestPointToBrep ERROR");
		}
		if (brepPoint.IsValid)
		{
			return brepNormal.IsValid;
		}
		return false;
	}

	public static bool GetClosestPointToBrep(IOnBrep brep, IOn3dPoint point, ref On3dPoint brepPoint)
	{
		if (!GetClosestPointToBrep(brep, point, out var faceIndex, out var s, out var t))
		{
			return false;
		}
		brepPoint = brep.Face(faceIndex).PointAt(s, t);
		return brepPoint.IsValid();
	}

	public static bool GetClosestPointToBrep(Brep brep, Point3d testPoint, ref Point3d brepPoint)
	{
		if (!GetClosestPointToBrep(brep, testPoint, out var faceIndex, out var u, out var v))
		{
			return false;
		}
		brepPoint = brep.Faces[faceIndex].PointAt(u, v);
		return brepPoint.IsValid;
	}

	public static bool GetClosestPointToBrep(IOnBrep brep, IOn3dPoint point, out int faceIndex, out double s, out double t)
	{
		faceIndex = -1;
		s = -1.23432101234321E+308;
		t = -1.23432101234321E+308;
		if (brep == null)
		{
			return false;
		}
		double num = -1.23432101234321E+308;
		for (int i = 0; i < brep.m_F.Count(); i++)
		{
			IOnBrepFace onBrepFace = brep.m_F[i];
			double s2 = -1.23432101234321E+308;
			double t2 = -1.23432101234321E+308;
			if (onBrepFace.GetClosestPoint(point, ref s2, ref t2, num))
			{
				double num2 = onBrepFace.PointAt(s2, t2).DistanceTo(point);
				if (!RhinoMath.IsValidDouble(num) || num2 < num)
				{
					num = num2;
					faceIndex = i;
					s = s2;
					t = t2;
				}
			}
		}
		return RhinoMath.IsValidDouble(num);
	}

	public static bool GetClosestPointToBrep(Brep brep, Point3d pt, out int faceIndex, out double u, out double v)
	{
		faceIndex = -1;
		u = double.NaN;
		v = double.NaN;
		if (brep == null)
		{
			return false;
		}
		double num = double.NaN;
		for (int i = 0; i < brep.Faces.Count; i++)
		{
			BrepFace brepFace = brep.Faces[i];
			if (brepFace.ClosestPoint(pt, out var u2, out var v2))
			{
				double num2 = brepFace.PointAt(u2, v2).DistanceTo(pt);
				if (double.IsNaN(num) || num2 < num)
				{
					num = num2;
					faceIndex = i;
					u = u2;
					v = v2;
				}
			}
		}
		return !double.IsNaN(num);
	}

	public static double GetDotProduct(IOn3dPoint point1, IOn3dPoint point2, IOn3dPoint point3)
	{
		On3dVector on3dVector = new On3dPoint(point1) - point2;
		On3dVector on3dVector2 = new On3dPoint(point3) - point2;
		on3dVector.Unitize();
		on3dVector2.Unitize();
		return on3dVector * on3dVector2;
	}

	public static void GetMinMax(IOnCurve curve, IOn3dPoint testPoint, int pointCount, out double min, out double max)
	{
		if (pointCount < 1)
		{
			min = double.MaxValue;
			max = 0.0;
			return;
		}
		On3dPointArray on3dPointArray = new On3dPointArray();
		double[] t = new double[pointCount];
		for (int i = 0; i < pointCount; i++)
		{
			t[i] = (double)i / (double)pointCount;
		}
		curve.GetNormalizedArcLengthPoints(t, ref t);
		double[] array = t;
		foreach (double t2 in array)
		{
			on3dPointArray.Append(curve.PointAt(t2));
		}
		GetMinMax(on3dPointArray, testPoint, out min, out max);
	}

	public static void GetMinMax(Curve curve, Point3d testPoint, int pointCount, out double min, out double max)
	{
		if (pointCount < 1)
		{
			min = double.MaxValue;
			max = 0.0;
			return;
		}
		Point3dList point3dList = new Point3dList();
		double[] array = new double[pointCount];
		for (int i = 0; i < pointCount; i++)
		{
			array[i] = (double)i / (double)pointCount;
		}
		array = curve.NormalizedLengthParameters(array, CSUtil.Tolerance);
		double[] array2 = array;
		foreach (double t in array2)
		{
			point3dList.Add(curve.PointAt(t));
		}
		GetMinMax(point3dList, testPoint, out min, out max);
	}

	[Obsolete]
	public static OnXform GetObjectDisplayXform(OnBoundingBox boundingBox)
	{
		OnXform onXform = new OnXform();
		onXform.Identity();
		if (boundingBox == null)
		{
			return onXform;
		}
		OnXform onXform2 = new OnXform();
		onXform2.Rotation(CSUtilMath.ToRadians(-90.0), OnUtil.On_xaxis, boundingBox.Center());
		OnXform onXform3 = new OnXform();
		onXform3.Rotation(CSUtilMath.ToRadians(-90.0), OnUtil.On_zaxis, boundingBox.Center());
		On3dVector on3dVector = boundingBox.Diagonal();
		if (on3dVector.z > 0.1 && on3dVector.x > on3dVector.y)
		{
			onXform = onXform2;
		}
		else if (on3dVector.z > 0.1 && on3dVector.y > on3dVector.x)
		{
			onXform = onXform2 * onXform3;
		}
		return onXform;
	}

	public static Transform GetObjectDisplayXform(BoundingBox boundingBox)
	{
		Transform result = Transform.Identity;
		if (!boundingBox.IsValid)
		{
			return result;
		}
		Transform transform = CSUtilMath.Rotation(CSUtilMath.ToRadians(-90.0), Vector3d.XAxis, boundingBox.Center);
		Transform transform2 = CSUtilMath.Rotation(CSUtilMath.ToRadians(-90.0), Vector3d.ZAxis, boundingBox.Center);
		Vector3d diagonal = boundingBox.Diagonal;
		if (diagonal.Z > 0.1 && diagonal.X > diagonal.Y)
		{
			result = transform;
		}
		else if (diagonal.Z > 0.1 && diagonal.Y > diagonal.X)
		{
			result = transform * transform2;
		}
		return result;
	}

	public static bool IntersectionSurfaces(IOnSurface surfaceLhs, IOnSurface surfaceRhs, out OnCurve outputCurve)
	{
		outputCurve = null;
		if (!RhUtilHelper.RhinoIntersectSurfaces(surfaceLhs, surfaceRhs, CSUtil.Tolerance, out var intersectionCurves, out var _))
		{
			return false;
		}
		if (intersectionCurves == null)
		{
			return false;
		}
		if (intersectionCurves.Length < 1)
		{
			return false;
		}
		IOnCurve[] inputCrvs = intersectionCurves;
		if (!CSUtilCurves.MergeCurves(inputCrvs, out OnCurve mergedCrv, 0.01))
		{
			return false;
		}
		if (mergedCrv == null)
		{
			return false;
		}
		outputCurve = mergedCrv;
		return true;
	}

	public static bool LoftBrepEdge(List<Curve> bEdge, bool flip1, bool flip2, out Brep surface)
	{
		surface = null;
		MArgsRhinoLoft mArgsRhinoLoft = new MArgsRhinoLoft();
		mArgsRhinoLoft.m_bAllowStartTangent = true;
		mArgsRhinoLoft.m_bAllowEndTangent = true;
		mArgsRhinoLoft.m_bUseStartpoint = false;
		mArgsRhinoLoft.m_bUseEndpoint = false;
		mArgsRhinoLoft.m_bClosed = false;
		mArgsRhinoLoft.m_loft_type = 0;
		mArgsRhinoLoft.m_simplify_method = 0;
		mArgsRhinoLoft.m_start_condition = 4;
		mArgsRhinoLoft.m_end_condition = 4;
		mArgsRhinoLoft.m_rebuild_point_count = 10;
		mArgsRhinoLoft.m_refit_tolerance = 0.001;
		int count = bEdge.Count;
		MRhinoLoftCurve[] array = new MRhinoLoftCurve[count];
		for (int i = 0; i < count; i++)
		{
			OnCurve onCurve;
			IOnBrepTrim trim;
			if (bEdge[i] is BrepEdge brepEdge)
			{
				OnBrep obj = (OnBrep)Interop.ToOnBrep(brepEdge.Brep);
				OnBrepEdge onBrepEdge = obj.Edge(brepEdge.EdgeIndex);
				onCurve = obj.Edge(onBrepEdge.m_edge_index);
				trim = obj.Trim(onBrepEdge.m_ti[0]);
			}
			else
			{
				onCurve = (OnCurve)Interop.ToOnCurve(bEdge[i]);
				trim = null;
			}
			if (onCurve != null)
			{
				OnCurve onCurve2 = onCurve.DuplicateCurve();
				if (i == 0 && flip1)
				{
					onCurve2.Reverse();
				}
				if (i == 1 && flip2)
				{
					onCurve2.Reverse();
				}
				array[i] = new MRhinoLoftCurve();
				array[i].m_curve = onCurve2;
				array[i].m_pick_point = new On3dPoint(OnUtil.On_UNSET_POINT);
				array[i].m_pick_t = -1.23432101234321E+308;
				array[i].m_trim = trim;
				array[i].m_bClosed = array[i].m_curve.IsClosed();
				array[i].m_bPlanar = array[i].m_curve.IsPlanar(array[i].m_plane);
			}
		}
		IRhinoLoftCurve[] loftcurves = array;
		mArgsRhinoLoft.m_loftcurves = loftcurves;
		OnNurbsSurface[] srfs = null;
		RhUtil.RhinoSdkLoftSurface(mArgsRhinoLoft, out srfs);
		if ((srfs == null) | (srfs.Length == 0))
		{
			return false;
		}
		if (srfs.Length == 1)
		{
			OnBrep source = srfs[0].BrepForm();
			surface = Interop.FromOnBrep(source);
			return true;
		}
		List<OnBrep> list = new List<OnBrep>();
		for (int j = 0; j < srfs.Length; j++)
		{
			if (srfs[j].IsValid())
			{
				OnBrep onBrep = new OnBrep();
				onBrep.NewFace(srfs[j]);
				list.Add(onBrep);
			}
		}
		OnBrep onBrep2 = RhUtil.RhinoJoinBreps(list.ToArray(), 0.001);
		if (onBrep2 != null)
		{
			surface = Interop.FromOnBrep(onBrep2);
			return true;
		}
		return false;
	}

	public static IEnumerable<OnCurve> NakedEdges(IOnBrep brep)
	{
		List<OnCurve> list = new List<OnCurve>();
		for (int i = 0; i < brep.m_E.Count(); i++)
		{
			if (brep.m_E[i].m_ti.m_count == 1)
			{
				list.Add(brep.m_E[i].DuplicateCurve());
			}
		}
		return list.ToArray();
	}

	public static Curve Offset(Curve crv, Vector3d normalDir, Point3d origin, double distance, CurveOffsetCornerStyle cornerStyle = CurveOffsetCornerStyle.Sharp)
	{
		if (crv == null)
		{
			return null;
		}
		if (!crv.IsValid)
		{
			return null;
		}
		if (Math.Abs(distance) <= CSUtil.Tolerance)
		{
			return crv.DuplicateCurve();
		}
		Curve[] array = crv.Offset(origin, normalDir, distance, 0.01, cornerStyle);
		if (array.Length == 1)
		{
			return array[0];
		}
		Curve[] array2 = Curve.JoinCurves(array);
		if (array2.Length == 1)
		{
			return array2[0];
		}
		return null;
	}

	public static OnCurve OffsetOld(IOnCurve crv, IOn3dVector normalDir, IOn3dPoint origin, double distance, bool closed)
	{
		if (crv == null)
		{
			return null;
		}
		if (!crv.IsValid())
		{
			return null;
		}
		if (Math.Abs(distance) <= CSUtil.Tolerance)
		{
			return crv.DuplicateCurve();
		}
		if (!RhUtilHelper.RhinoOffsetCurve(crv, normalDir, origin, distance, 0.0001, 0.01, 1, out var offsetCurves))
		{
			CSUtil.PrintLine("Error Creating Offset");
			return null;
		}
		for (int i = 0; i < offsetCurves.Length - 1; i++)
		{
			RhUtilHelper.RhinoMakeCurveEndsMeet(offsetCurves[i], 1, offsetCurves[i + 1], 0);
		}
		if (offsetCurves.Length > 1 && closed)
		{
			RhUtilHelper.RhinoMakeCurveEndsMeet(offsetCurves[0], 0, offsetCurves[offsetCurves.Length - 1], 1);
		}
		IOnCurve[] array = new IOnCurve[offsetCurves.Length];
		for (int j = 0; j < offsetCurves.Length; j++)
		{
			array[j] = offsetCurves[j];
		}
		OnCurve[] joinRet = new OnCurve[1];
		if (!RhUtilHelper.RhinoJoinCurves(array, ref joinRet, CSUtil.Tolerance))
		{
			CSUtil.PrintLine("Error RhinoJoinCurves");
			return null;
		}
		if (joinRet.Length == 1)
		{
			return joinRet[0];
		}
		CSUtil.PrintLine("Error Merging Curves");
		return null;
	}

	public static Curve OffsetOld(Curve crv, Vector3d normalDir, Point3d origin, double distance, bool closed)
	{
		if (crv == null)
		{
			return null;
		}
		if (!crv.IsValid)
		{
			return null;
		}
		if (Math.Abs(distance) <= CSUtil.Tolerance)
		{
			return crv.DuplicateCurve();
		}
		if (!RhUtilHelper.RhinoOffsetCurve(crv, normalDir, origin, distance, 0.0001, 0.01, CurveOffsetCornerStyle.Sharp, out var offsetCurves))
		{
			CSUtil.PrintLine("Error Creating Offset");
			return null;
		}
		for (int i = 0; i < offsetCurves.Length - 1; i++)
		{
			RhUtilHelper.RhinoMakeCurveEndsMeet(offsetCurves[i], doAdjustCurveA: false, offsetCurves[i + 1], doAdjustCurveB: true);
		}
		if (offsetCurves.Length > 1 && closed)
		{
			RhUtilHelper.RhinoMakeCurveEndsMeet(offsetCurves[0], doAdjustCurveA: true, offsetCurves[offsetCurves.Length - 1], doAdjustCurveB: false);
		}
		Curve[] array = RhUtilHelper.RhinoJoinCurves(offsetCurves, CSUtil.Tolerance);
		if (array.Length == 1)
		{
			return array[0];
		}
		CSUtil.PrintLine("Error Merging Curves");
		return null;
	}

	public static bool OrientProfileToCurve(IOnCurve rail, double param, ref OnXform x, double yScale)
	{
		On3dPoint point = new On3dPoint();
		On3dVector first_derivative = new On3dVector();
		if (!rail.Ev1Der(param, ref point, ref first_derivative))
		{
			return false;
		}
		On3dVector on3dVector = point - OnUtil.On_origin;
		on3dVector.y *= yScale;
		on3dVector.Unitize();
		x = CSUtilMath.Orient3Point(new On3dPoint(0.0, 0.0, 0.0), new On3dVector(0.0, 1.0, 0.0), new On3dVector(0.0, 0.0, 1.0), point, on3dVector, first_derivative);
		return true;
	}

	public static bool OrientProfileToCurve(Curve rail, double param, ref Transform x, double yScale)
	{
		Point3d unset = Point3d.Unset;
		Vector3d unset2 = Vector3d.Unset;
		unset = rail.PointAt(param);
		unset2 = rail.TangentAt(param);
		if (unset == Point3d.Unset || unset2 == Vector3d.Unset)
		{
			return false;
		}
		Vector3d destination = unset - Point3d.Origin;
		destination.Y *= yScale;
		destination.Unitize();
		x = CSUtilMath.Orient3Point(new Point3d(0.0, 0.0, 0.0), new Vector3d(0.0, 1.0, 0.0), new Vector3d(0.0, 0.0, 1.0), unset, destination, unset2);
		return true;
	}

	public static bool OrientProfileToCurveNalp(IOnCurve rail, double param, ref OnXform x, double yScale)
	{
		if (!rail.GetNormalizedArcLengthPoint(param, ref param))
		{
			return false;
		}
		On3dPoint point = new On3dPoint();
		On3dVector first_derivative = new On3dVector();
		if (!rail.Ev1Der(param, ref point, ref first_derivative))
		{
			return false;
		}
		first_derivative.Unitize();
		On3dVector on3dVector = point - OnUtil.On_origin;
		on3dVector.y *= yScale;
		on3dVector.Unitize();
		x = CSUtilMath.Orient3Point(OnUtil.On_origin, OnUtil.On_yaxis, OnUtil.On_zaxis, point, on3dVector, first_derivative);
		return true;
	}

	public static bool OrientProfileToCurveNalp(Curve rail, double param, ref Transform x, double yScale)
	{
		if (!rail.NormalizedLengthParameter(param, out param))
		{
			return false;
		}
		Point3d unset = Point3d.Unset;
		Vector3d unset2 = Vector3d.Unset;
		unset = rail.PointAt(param);
		unset2 = rail.TangentAt(param);
		if (unset == Point3d.Unset || unset2 == Vector3d.Unset)
		{
			return false;
		}
		unset2.Unitize();
		Vector3d destination = unset - Point3d.Origin;
		destination.Y *= yScale;
		destination.Unitize();
		x = CSUtilMath.Orient3Point(Point3d.Origin, Vector3d.YAxis, Vector3d.ZAxis, unset, destination, unset2);
		return true;
	}

	public static bool OrientProfileToRailNalp(IOnCurve rail, double param, ref OnXform xOrient)
	{
		if (!rail.GetNormalizedArcLengthPoint(param, ref param))
		{
			return false;
		}
		On3dPoint point = new On3dPoint();
		On3dVector first_derivative = new On3dVector();
		if (!rail.Ev1Der(param, ref point, ref first_derivative))
		{
			return false;
		}
		first_derivative.Unitize();
		On3dVector on3dVector = new On3dVector(point.x, 0.0, point.z);
		on3dVector.Unitize();
		xOrient = CSUtilMath.Orient3Point(OnUtil.On_origin, OnUtil.On_yaxis, OnUtil.On_zaxis, point, on3dVector, first_derivative);
		return true;
	}

	public static bool OrientToCurveNalp(IOnCurve rail, double param, ref OnXform x)
	{
		if (!rail.GetNormalizedArcLengthPoint(param, ref param))
		{
			return false;
		}
		On3dPoint point = new On3dPoint();
		On3dVector first_derivative = new On3dVector();
		if (!rail.Ev1Der(param, ref point, ref first_derivative))
		{
			return false;
		}
		On3dVector destination = point - OnUtil.On_origin;
		x = CSUtilMath.Orient3Point(new On3dPoint(0.0, 0.0, 0.0), new On3dVector(0.0, 0.0, 1.0), new On3dVector(1.0, 0.0, 0.0), point, destination, first_derivative);
		return true;
	}

	public static bool OrientToCurveNalp(Curve rail, double param, ref Transform x)
	{
		if (!rail.NormalizedLengthParameter(param, out param))
		{
			return false;
		}
		Point3d unset = Point3d.Unset;
		Vector3d unset2 = Vector3d.Unset;
		unset2 = rail.TangentAt(param);
		unset = rail.PointAt(param);
		if (unset2 == Vector3d.Unset || unset == Point3d.Unset)
		{
			return false;
		}
		Vector3d destination = unset - Point3d.Origin;
		x = CSUtilMath.Orient3Point(new Point3d(0.0, 0.0, 0.0), new Vector3d(0.0, 0.0, 1.0), new Vector3d(1.0, 0.0, 0.0), unset, destination, unset2);
		return true;
	}

	public static OnCurve OriginOffset(IOnCurve crv, IOn3dVector normalDir, double distance, bool closed)
	{
		return OffsetOld(crv, normalDir, OnUtil.On_origin, distance, closed);
	}

	public static Curve OriginOffset(Curve crv, Vector3d normalDir, double distance)
	{
		return Offset(crv, normalDir, Point3d.Origin, distance);
	}

	public static Curve OriginOffsetOldXY(Curve crv, double distance, bool closed)
	{
		return OffsetOld(crv, Vector3d.ZAxis, Point3d.Origin, distance, closed);
	}

	public static OnCurve OriginOffsetXY(IOnCurve crv, double distance, bool closed)
	{
		return OffsetOld(crv, OnUtil.On_zaxis, OnUtil.On_origin, distance, closed);
	}

	public static Curve OriginOffsetXY(Curve crv, double distance)
	{
		return Offset(crv, Vector3d.ZAxis, Point3d.Origin, distance);
	}

	public static bool SplitProfileInHalf(IOnCurve curve, ref OnCurve leftSide, ref OnCurve rightSide)
	{
		OnPlane onPlane = new OnPlane();
		if (!curve.IsPlanar(onPlane))
		{
			return false;
		}
		OnLineCurve curveB = ((Math.Abs(OnUtil.ON_DotProduct(onPlane.Normal(), OnUtil.On_yaxis)) > 0.9) ? new OnLineCurve(new On3dPoint(0.0, 0.0, -100.0), new On3dPoint(0.0, 0.0, 100.0)) : new OnLineCurve(new On3dPoint(0.0, -100.0, 0.0), new On3dPoint(0.0, 100.0, 0.0)));
		ArrayOnX_EVENT x = new ArrayOnX_EVENT();
		curve.IntersectCurve(curveB, ref x);
		if (x.Count() < 2)
		{
			return false;
		}
		OnCurve onCurve = curve.DuplicateCurve();
		onCurve.ChangeClosedCurveSeam(x[0].get_m_a(0));
		double t = 0.0;
		onCurve.GetClosestPoint(x[1].get_m_pointA(0), ref t);
		return onCurve.Split(t, ref leftSide, ref rightSide);
	}

	public static On3dVector TangentAtForce(OnNurbsCurve crv, double t)
	{
		On3dVector on3dVector = crv.TangentAt(t);
		if (on3dVector != null && on3dVector.IsValid() && !on3dVector.IsZero())
		{
			return on3dVector;
		}
		double num = crv.Domain().Length();
		if (Math.Abs(t - crv.Domain().Min()) < Math.Abs(t - crv.Domain().Max()))
		{
			for (int i = 0; i < 1000; i++)
			{
				on3dVector = crv.TangentAt(t + num * ((double)i / 2000.0));
				if (on3dVector != null && on3dVector.IsValid())
				{
					return on3dVector;
				}
			}
		}
		else
		{
			for (int j = 0; j < 1000; j++)
			{
				on3dVector = crv.TangentAt(t - num * ((double)j / 2000.0));
				if (on3dVector != null && on3dVector.IsValid())
				{
					return on3dVector;
				}
			}
		}
		return null;
	}

	public static Curve Trim2dLineCurveToSurface(Curve curve2D, Surface srf3D)
	{
		Curve result = curve2D.DuplicateCurve();
		Interval interval = srf3D.Domain(0);
		Interval interval2 = srf3D.Domain(1);
		Point3d corner = new Point3d(interval.Min, interval2.Min, 0.0);
		Point3d corner2 = new Point3d(interval.Min, interval2.Max, 0.0);
		Point3d corner3 = new Point3d(interval.Max, interval2.Max, 0.0);
		Point3d corner4 = new Point3d(interval.Max, interval2.Min, 0.0);
		Surface surface = NurbsSurface.CreateFromCorners(corner, corner2, corner3, corner4);
		CurveIntersections curveIntersections = Intersection.CurveSurface(curve2D, surface, CSUtil.Tolerance, CSUtil.Tolerance);
		if (curveIntersections != null && curveIntersections.Count > 0)
		{
			result = new LineCurve(curveIntersections[0].PointB, curveIntersections[0].PointB2);
		}
		return result;
	}

	public static void TrimEndOfCrv(OnCurve crv, double len)
	{
		double length = 0.0;
		crv.GetLength(ref length);
		double s = (length - len) / length;
		double t = 0.0;
		crv.GetNormalizedArcLengthPoint(s, ref t);
		crv.Trim(new OnInterval(crv.Domain().Min(), t));
	}

	public static void TrimEndOfCrv(ref Curve crv, double len)
	{
		double length = crv.GetLength();
		double s = (length - len) / length;
		crv.NormalizedLengthParameter(s, out var t);
		crv = crv.Trim(crv.Domain.Min, t);
	}

	public static void TrimStartAndEndOfCrv(OnCurve crv, double len)
	{
		double length = 0.0;
		crv.GetLength(ref length);
		double s = len / length;
		double t = 0.0;
		crv.GetNormalizedArcLengthPoint(s, ref t);
		double s2 = (length - len) / length;
		double t2 = 0.0;
		crv.GetNormalizedArcLengthPoint(s2, ref t2);
		crv.Trim(new OnInterval(t, t2));
	}

	public static void TrimStartAndEndOfCrv(ref Curve crv, double len)
	{
		double length = crv.GetLength();
		double s = len / length;
		crv.NormalizedLengthParameter(s, out var t);
		double s2 = (length - len) / length;
		crv.NormalizedLengthParameter(s2, out var t2);
		crv = crv.Trim(t, t2);
	}

	public static void TrimStartOfCrv(OnCurve crv, double len)
	{
		double length = 0.0;
		crv.GetLength(ref length);
		double s = len / length;
		double t = 0.0;
		crv.GetNormalizedArcLengthPoint(s, ref t);
		crv.Trim(new OnInterval(t, crv.Domain().Max()));
	}

	public static void TrimStartOfCrv(ref Curve crv, double len)
	{
		double length = crv.GetLength();
		double s = len / length;
		double t = 0.0;
		crv.NormalizedLengthParameter(s, out t);
		crv = crv.Trim(t, crv.Domain.Max);
	}

	private static void AddLine(OnMesh m, int v0, int v1, int v2)
	{
		Point3f point3f = new Point3f(m.m_V[v0].x, m.m_V[v0].y, m.m_V[v0].z);
		Point3f point3f2 = new Point3f(m.m_V[v1].x, m.m_V[v1].y, m.m_V[v1].z);
		Point3f point3f3 = new Point3f(m.m_V[v2].x, m.m_V[v2].y, m.m_V[v2].z);
		Point3d point3d = new Point3d(point3f.X, point3f.Y, point3f.Z);
		Point3d point3d2 = new Point3d(point3f2.X, point3f2.Y, point3f2.Z);
		Point3d point3d3 = new Point3d(point3f3.X, point3f3.Y, point3f3.Z);
		RhinoDoc.ActiveDoc.Objects.AddLine(new Line(point3d, point3d2));
		RhinoDoc.ActiveDoc.Objects.AddLine(new Line(point3d2, point3d3));
		RhinoDoc.ActiveDoc.Objects.AddLine(new Line(point3d3, point3d));
	}

	private static void AddLine(OnMesh m, int v0, int v1, int v2, int v3)
	{
		Point3f point3f = new Point3f(m.m_V[v0].x, m.m_V[v0].y, m.m_V[v0].z);
		Point3f point3f2 = new Point3f(m.m_V[v1].x, m.m_V[v1].y, m.m_V[v1].z);
		Point3f point3f3 = new Point3f(m.m_V[v2].x, m.m_V[v2].y, m.m_V[v2].z);
		Point3f point3f4 = new Point3f(m.m_V[v3].x, m.m_V[v3].y, m.m_V[v3].z);
		Point3d point3d = new Point3d(point3f.X, point3f.Y, point3f.Z);
		Point3d point3d2 = new Point3d(point3f2.X, point3f2.Y, point3f2.Z);
		Point3d point3d3 = new Point3d(point3f3.X, point3f3.Y, point3f3.Z);
		Point3d point3d4 = new Point3d(point3f4.X, point3f4.Y, point3f4.Z);
		RhinoDoc.ActiveDoc.Objects.AddLine(new Line(point3d, point3d2));
		RhinoDoc.ActiveDoc.Objects.AddLine(new Line(point3d2, point3d3));
		RhinoDoc.ActiveDoc.Objects.AddLine(new Line(point3d3, point3d4));
		RhinoDoc.ActiveDoc.Objects.AddLine(new Line(point3d4, point3d));
	}

	private static void BuildProngRail(IOnNurbsCurve rail, out OnNurbsCurve railActual)
	{
		OnBoundingBox tight_bbox = new OnBoundingBox();
		rail.GetTightBoundingBox(ref tight_bbox);
		double num = tight_bbox.Max().x - tight_bbox.Min().x;
		double num2 = tight_bbox.Max().y - tight_bbox.Min().y;
		double num3 = 4.0 / (num + num2);
		OnXform onXform = new OnXform();
		onXform.Scale(num3, num3, 1.0);
		OnXform onXform2 = new OnXform();
		onXform2.Translation(new On3dPoint(OnUtil.On_origin) - tight_bbox.Center());
		railActual = OnNurbsCurve.New(rail);
		railActual.Transform(onXform * onXform2);
	}

	private static void GetMinMax(IArrayOn3dPoint points, IOn3dPoint testPoint, out double min, out double max)
	{
		max = 0.0;
		min = double.MaxValue;
		for (int i = 0; i < points.Count(); i++)
		{
			double num = points[i].DistanceTo(testPoint);
			if (num < min)
			{
				min = num;
			}
			if (num > max)
			{
				max = num;
			}
		}
	}

	private static void GetMinMax(Point3dList points, Point3d testPoint, out double min, out double max)
	{
		max = 0.0;
		min = double.MaxValue;
		for (int i = 0; i < points.Count; i++)
		{
			double num = points[i].DistanceTo(testPoint);
			if (num < min)
			{
				min = num;
			}
			if (num > max)
			{
				max = num;
			}
		}
	}
}
