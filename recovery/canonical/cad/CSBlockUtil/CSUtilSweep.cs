using System;
using System.Collections.Generic;
using System.Linq;
using CSCompatibility;
using RMA.OpenNURBS;
using RMA.Rhino;
using Rhino.Geometry;
using Rhino.Runtime;

namespace CSBlockUtil;

public static class CSUtilSweep
{
	public static bool GetClosestPoints(OnCurve curve1, OnCurve curve2, int pointCount, ref double ct1, ref double ct2)
	{
		On3dPointArray on3dPointArray = new On3dPointArray();
		On3dPointArray on3dPointArray2 = new On3dPointArray();
		double[] t = new double[pointCount];
		double[] t2 = new double[pointCount];
		for (int i = 0; i < pointCount; i++)
		{
			t[i] = (double)i / (double)pointCount;
			t2[i] = (double)i / (double)pointCount;
		}
		curve1.GetNormalizedArcLengthPoints(t, ref t);
		curve2.GetNormalizedArcLengthPoints(t2, ref t2);
		for (int j = 0; j < t.Length; j++)
		{
			on3dPointArray.Append(curve1.PointAt(t[j]));
			on3dPointArray2.Append(curve2.PointAt(t2[j]));
		}
		int num = 0;
		int num2 = 0;
		double num3 = double.MaxValue;
		for (int k = 0; k < pointCount; k++)
		{
			for (int l = 0; l < pointCount; l++)
			{
				double num4 = on3dPointArray[k].DistanceTo(on3dPointArray2[l]);
				if (num4 < num3)
				{
					num = k;
					num2 = l;
					num3 = num4;
				}
			}
		}
		ct1 = t[num];
		ct2 = t2[num2];
		return true;
	}

	public static OnBrep Loft(IOnCurve[] profiles, IArgsRhinoLoft.eLoftType loftType)
	{
		return Loft(profiles, loftType, capped: false);
	}

	public static OnBrep Loft(IOnCurve[] profiles, IArgsRhinoLoft.eLoftType loftType, bool capped)
	{
		MArgsRhinoLoft mArgsRhinoLoft = new MArgsRhinoLoft
		{
			m_bUseEndpoint = false,
			m_bUseStartpoint = false,
			m_bAllowStartTangent = false,
			m_bAllowEndTangent = false,
			m_bClosed = false,
			m_loft_type = (int)loftType,
			m_simplify_method = 0,
			m_start_condition = 3,
			m_end_condition = 3,
			m_refit_tolerance = CSUtil.Tolerance
		};
		int num = profiles.Length;
		MRhinoLoftCurve[] array = new MRhinoLoftCurve[num];
		for (int i = 0; i < num; i++)
		{
			IOnCurve onCurve = profiles[i];
			if (onCurve != null)
			{
				array[i] = new MRhinoLoftCurve
				{
					m_curve = onCurve.DuplicateCurve()
				};
				array[i].m_curve.RemoveShortSegments(OnUtil.On_ZERO_TOLERANCE, bRemoveShortSegments: true);
				array[i].m_pick_point = new On3dPoint(OnUtil.On_UNSET_POINT);
				array[i].m_pick_t = -1.23432101234321E+308;
				array[i].m_trim = null;
				array[i].m_bClosed = array[i].m_curve.IsClosed();
				array[i].m_bPlanar = array[i].m_curve.IsPlanar(array[i].m_plane);
			}
		}
		IRhinoLoftCurve[] loftcurves = array;
		mArgsRhinoLoft.m_loftcurves = loftcurves;
		if (!RhUtil.RhinoSdkLoftSurface(mArgsRhinoLoft, out var srfs))
		{
			return null;
		}
		if (srfs == null || srfs.Length == 0)
		{
			return null;
		}
		OnBrep onBrep;
		if (srfs.Length == 1)
		{
			onBrep = srfs[0].BrepForm();
		}
		else
		{
			List<OnBrep> list = new List<OnBrep>();
			OnNurbsSurface[] array2 = srfs;
			foreach (OnNurbsSurface onNurbsSurface in array2)
			{
				if (onNurbsSurface.IsValid())
				{
					OnBrep onBrep2 = new OnBrep();
					onBrep2.NewFace(onNurbsSurface);
					list.Add(onBrep2);
				}
			}
			onBrep = CSUtilRhino.JoinAllBreps(list.ToArray(), CSUtil.Tolerance);
		}
		if (onBrep != null && capped)
		{
			onBrep = CSUtilRhino.CapPlanarHoles(onBrep, CSUtil.Tolerance);
		}
		return onBrep;
	}

	public static OnBrep LoftMatchCurves(IOnCurve[] profiles, IArgsRhinoLoft.eLoftType loftType, bool capped)
	{
		List<OnCurve> list = new List<OnCurve>();
		OnCurve onCurve = null;
		IOnCurve[] array = profiles;
		foreach (IOnCurve onCurve2 in array)
		{
			if (onCurve2 != null)
			{
				OnCurve onCurve3 = onCurve2.DuplicateCurve();
				if (onCurve != null)
				{
					MatchCurveDirection2(onCurve, onCurve3, 50);
				}
				list.Add(onCurve3);
				onCurve = onCurve3;
			}
		}
		array = list.ToArray();
		return Loft(array, loftType, capped);
	}

	public static bool MatchCurveDirection(OnCurve curve1, OnCurve curve2, int pointCount)
	{
		double[] t = new double[pointCount];
		double[] t2 = new double[pointCount];
		for (int i = 0; i < pointCount; i++)
		{
			t[i] = (double)i / (double)(curve1.IsClosed() ? pointCount : (pointCount - 1));
			t2[i] = (double)i / (double)(curve2.IsClosed() ? pointCount : (pointCount - 1));
		}
		curve1.GetNormalizedArcLengthPoints(t, ref t);
		curve2.GetNormalizedArcLengthPoints(t2, ref t2);
		On3dPointArray on3dPointArray = new On3dPointArray();
		On3dPointArray on3dPointArray2 = new On3dPointArray();
		for (int j = 0; j < t.Length; j++)
		{
			on3dPointArray.Append(curve1.PointAt(t[j]));
			on3dPointArray2.Append(curve2.PointAt(t2[j]));
		}
		double num = 0.0;
		double num2 = 0.0;
		int num3 = 0;
		int num4 = 0;
		double num5 = OnUtil.On_DBL_MAX;
		if (curve1.IsClosed() || curve2.IsClosed())
		{
			for (int k = 0; k < on3dPointArray2.Count(); k++)
			{
				if (on3dPointArray[0].DistanceTo(on3dPointArray2[k]) < num5)
				{
					num5 = on3dPointArray[0].DistanceTo(on3dPointArray2[k]);
					num4 = k;
				}
			}
		}
		else
		{
			num4 = 0;
		}
		for (int l = 0; l < on3dPointArray.Count(); l++)
		{
			num += on3dPointArray[(l + num3) % pointCount].DistanceTo(on3dPointArray2[(l + num4) % pointCount]);
			num2 += on3dPointArray[(l + num3) % pointCount].DistanceTo(on3dPointArray2[(pointCount - l - 1 + num4) % pointCount]);
		}
		if (!(num2 < num))
		{
			return false;
		}
		curve2.Reverse();
		return true;
	}

	public static bool MatchCurveDirection2(OnCurve curve1, OnCurve curve2, int pointCount)
	{
		On3dPointArray on3dPointArray = new On3dPointArray();
		On3dPointArray on3dPointArray2 = new On3dPointArray();
		List<double> list = new List<double>();
		List<double> list2 = new List<double>();
		for (int i = 0; i < pointCount; i++)
		{
			list.Add((double)i / (double)pointCount);
			list2.Add((double)i / (double)pointCount);
		}
		double[] t = list.ToArray();
		double[] t2 = list2.ToArray();
		curve1.GetNormalizedArcLengthPoints(t, ref t);
		curve2.GetNormalizedArcLengthPoints(t2, ref t2);
		for (int j = 0; j < t.Length; j++)
		{
			on3dPointArray.Append(curve1.PointAt(t[j]));
			on3dPointArray2.Append(curve2.PointAt(t2[j]));
		}
		double num = double.MaxValue;
		int num2 = 0;
		bool flag = false;
		for (int k = 0; k < 2; k++)
		{
			bool flag2 = k == 1;
			int offset = 0;
			for (int l = 0; l < pointCount; l++)
			{
				double num3 = CalculateMatchDirectionValue(on3dPointArray, on3dPointArray2, offset, l, flag2);
				if (num3 < num)
				{
					num2 = l;
					flag = flag2;
					num = num3;
				}
			}
		}
		curve2.ChangeClosedCurveSeam(t2[num2]);
		if (flag)
		{
			curve2.Reverse();
		}
		return flag;
	}

	public static OnMesh MeshPipe(IOnCurve curve, IOn3dPoint[] points, double[] radii, double tolerance)
	{
		double length = 0.0;
		curve.GetLength(ref length);
		if (length < tolerance)
		{
			return null;
		}
		if (points.Length < 2)
		{
			return null;
		}
		if (points.Length != radii.Length)
		{
			return null;
		}
		double num = radii[0];
		double num2 = radii[radii.Length - 1];
		List<double> list = new List<double>();
		foreach (IOn3dPoint arg in points)
		{
			double t = 0.0;
			curve.GetClosestPoint(arg, ref t);
			list.Add(t);
		}
		OnCircle onCircle = new OnCircle(new OnPlane(OnUtil.On_origin, OnUtil.On_zaxis), num);
		double num3 = Math.Max(num, num2 / 2.0) * 2.0 * OnUtil.On_PI;
		OnNurbsCurve nurbs_curve = new OnNurbsCurve();
		onCircle.GetNurbForm(ref nurbs_curve);
		nurbs_curve.SetDomain(new OnInterval(0.0, 1.0));
		double num4 = (int)(num3 / tolerance);
		List<double> list2 = new List<double>();
		for (int j = 0; (double)j < num4; j++)
		{
			list2.Add((double)j / num4);
		}
		double[] t2 = new double[list2.Count];
		nurbs_curve.GetNormalizedArcLengthPoints(list2.ToArray(), ref t2);
		OnMesh onMesh = new OnMesh((list.Count - 1) * t2.Length, list.Count * t2.Length, has_vertex_normals: true, has_texture_coords: false);
		OnXform onXform = new OnXform();
		OnXform onXform2 = new OnXform();
		On3fPoint arg2 = new On3fPoint();
		On3fVector on3fVector = new On3fVector();
		On3fPoint on3fPoint = new On3fPoint();
		int count = list.Count;
		int num5 = t2.Length;
		On3fPointArray on3fPointArray = new On3fPointArray(num5);
		for (int k = 0; k < num5; k++)
		{
			on3fPointArray.Append(nurbs_curve.PointAt(t2[k]));
		}
		onMesh.m_V.SetCount(list.Count * num5);
		onMesh.m_N.SetCount(list.Count * num5);
		for (int l = 0; l < count; l++)
		{
			double t3 = list[l];
			On3dPoint on3dPoint = curve.PointAt(t3);
			on3fPoint.x = (float)on3dPoint.x;
			on3fPoint.y = (float)on3dPoint.y;
			on3fPoint.z = (float)on3dPoint.z;
			onXform.ChangeBasis(new OnPlane(on3dPoint, curve.TangentAt(t3)), OnPlane.World_xy);
			double num6 = radii[l] / num;
			onXform2.Scale(num6, num6, num6);
			OnXform arg3 = onXform * onXform2;
			int num7 = 0;
			double num8 = double.MaxValue;
			if (l > 0)
			{
				for (int m = 0; m < num5; m++)
				{
					On3fPoint on3fPoint2 = new On3fPoint(on3fPointArray[m]);
					on3fPoint2.Transform(arg3);
					double num9 = on3fPoint2.DistanceTo(arg2);
					if (num9 < num8)
					{
						num8 = num9;
						num7 = m;
					}
				}
			}
			for (int n = 0; n < num5; n++)
			{
				On3fPoint on3fPoint2 = new On3fPoint(on3fPointArray[(n + num7) % num5]);
				on3fPoint2.Transform(arg3);
				int index = l * num5 + n;
				onMesh.m_V[index] = on3fPoint2;
				on3fVector.x = on3fPoint2.x - on3fPoint.x;
				on3fVector.y = on3fPoint2.y - on3fPoint.y;
				on3fVector.z = on3fPoint2.z - on3fPoint.z;
				on3fVector.Unitize();
				onMesh.m_N[index] = on3fVector;
				if (n == 0)
				{
					arg2 = on3fPoint2;
				}
			}
		}
		for (int num10 = 1; num10 < count; num10++)
		{
			for (int num11 = 1; num11 <= num5; num11++)
			{
				onMesh.m_F.AppendQuad((num10 - 1) * num5 + (num11 - 1), (num10 - 1) * num5 + num11 % num5, num10 * num5 + num11 % num5, num10 * num5 + num11 - 1);
			}
		}
		int num12 = num5 / 4;
		OnMesh onMesh2 = new OnMesh();
		On3dPoint on3dPoint2 = curve.PointAt(list[0]);
		on3fPoint.x = (float)on3dPoint2.x;
		on3fPoint.y = (float)on3dPoint2.y;
		on3fPoint.z = (float)on3dPoint2.z;
		onXform.ChangeBasis(new OnPlane(on3dPoint2, -curve.TangentAt(list[0])), OnPlane.World_xy);
		double num13 = radii[0] / num;
		onXform2.Scale(num13, num13, num13);
		for (int num14 = 0; num14 <= num5; num14++)
		{
			OnXform onXform3 = new OnXform();
			On3dVector on3dVector = nurbs_curve.PointAt(t2[num14 % num5]) - OnUtil.On_origin;
			on3dVector.Unitize();
			onXform3.Rotation(OnUtil.On_PI / 2.0, on3dVector, OnUtil.On_origin);
			OnNurbsCurve onNurbsCurve = nurbs_curve.Duplicate();
			if (!onNurbsCurve.ChangeClosedCurveSeam(t2[num14 % num5]))
			{
				continue;
			}
			onNurbsCurve.SetDomain(new OnInterval(0.0, 1.0));
			OnXform arg4 = onXform3;
			for (int num15 = 0; num15 < num12 + 1; num15++)
			{
				On3dPoint on3dPoint3;
				if (num15 != num12)
				{
					on3dPoint3 = onNurbsCurve.PointAt(t2[num15]);
					on3dPoint3.Transform(arg4);
				}
				else
				{
					on3dPoint3 = new On3dPoint(0.0, 0.0, num);
				}
				onMesh2.m_V.Append(on3dPoint3);
				On3dVector on3dVector2 = on3dPoint3 - OnUtil.On_origin;
				on3dVector2.Unitize();
				on3dVector2.Reverse();
				onMesh2.m_N.Append(on3dVector2);
			}
			if (num14 > 0)
			{
				for (int num16 = 1; num16 < num12 + 1; num16++)
				{
					onMesh2.m_F.AppendQuad((num14 - 1 + num5) % num5 * (num12 + 1) + (num16 - 1), (num14 - 1 + num5) % num5 * (num12 + 1) + num16, num14 % num5 * (num12 + 1) + num16, num14 % num5 * (num12 + 1) + num16 - 1);
				}
			}
		}
		OnMesh onMesh3 = onMesh2.Duplicate();
		onMesh2.Transform(onXform);
		num13 = radii[radii.Length - 1] / num;
		onXform2.Scale(num13, num13, num13);
		onXform.ChangeBasis(new OnPlane(curve.PointAt(list[list.Count - 1]), curve.TangentAt(list[list.Count - 1])), OnPlane.World_xy);
		onMesh3.Transform(onXform * onXform2);
		onMesh.Append(onMesh2);
		onMesh.Append(onMesh3);
		return onMesh;
	}

	public static OnMesh MeshPipe(IOnCurve curve, double startRadius, double endRadius, double tolerance)
	{
		double length = 0.0;
		curve.GetLength(ref length);
		if (length < tolerance)
		{
			return null;
		}
		if (startRadius < tolerance || endRadius < tolerance)
		{
			return null;
		}
		double num = Math.Max(10, (int)(length / tolerance));
		List<double> list = new List<double>();
		for (int i = 0; (double)i < num; i++)
		{
			list.Add((double)i / num);
		}
		list.Add(1.0);
		double[] t = new double[list.Count];
		curve.GetNormalizedArcLengthPoints(list.ToArray(), ref t);
		OnCircle onCircle = new OnCircle(new OnPlane(OnUtil.On_origin, OnUtil.On_zaxis), startRadius);
		double num2 = Math.Max(startRadius, endRadius) * 2.0 * OnUtil.On_PI;
		double num3 = (endRadius / startRadius - 1.0) / (double)(list.Count - 1);
		OnNurbsCurve nurbs_curve = new OnNurbsCurve();
		onCircle.GetNurbForm(ref nurbs_curve);
		nurbs_curve.SetDomain(new OnInterval(0.0, 1.0));
		double num4 = (int)(num2 / tolerance);
		List<double> list2 = new List<double>();
		for (int j = 0; (double)j < num4; j++)
		{
			list2.Add((double)j / num4);
		}
		double[] t2 = new double[list2.Count];
		nurbs_curve.GetNormalizedArcLengthPoints(list2.ToArray(), ref t2);
		OnMesh onMesh = new OnMesh((t.Length - 1) * t2.Length, t.Length * t2.Length, has_vertex_normals: true, has_texture_coords: false);
		OnXform onXform = new OnXform();
		OnXform onXform2 = new OnXform();
		On3fPoint arg = new On3fPoint();
		On3fVector on3fVector = new On3fVector();
		On3fPoint on3fPoint = new On3fPoint();
		int num5 = t.Length;
		int num6 = t2.Length;
		On3fPointArray on3fPointArray = new On3fPointArray(num6);
		for (int k = 0; k < num6; k++)
		{
			on3fPointArray.Append(nurbs_curve.PointAt(t2[k]));
		}
		onMesh.m_V.SetCount(t.Length * num6);
		onMesh.m_N.SetCount(t.Length * num6);
		for (int l = 0; l < num5; l++)
		{
			double t3 = t[l];
			On3dPoint on3dPoint = curve.PointAt(t3);
			on3fPoint.x = (float)on3dPoint.x;
			on3fPoint.y = (float)on3dPoint.y;
			on3fPoint.z = (float)on3dPoint.z;
			onXform.ChangeBasis(new OnPlane(on3dPoint, curve.TangentAt(t3)), OnPlane.World_xy);
			onXform2.Scale(1.0 + num3 * (double)l, 1.0 + num3 * (double)l, 1.0 + num3 * (double)l);
			OnXform arg2 = onXform * onXform2;
			int num7 = 0;
			double num8 = double.MaxValue;
			if (l > 0)
			{
				for (int m = 0; m < num6; m++)
				{
					On3fPoint on3fPoint2 = new On3fPoint(on3fPointArray[m]);
					on3fPoint2.Transform(arg2);
					double num9 = on3fPoint2.DistanceTo(arg);
					if (num9 < num8)
					{
						num8 = num9;
						num7 = m;
					}
				}
			}
			for (int n = 0; n < num6; n++)
			{
				On3fPoint on3fPoint2 = new On3fPoint(on3fPointArray[(n + num7) % num6]);
				on3fPoint2.Transform(arg2);
				int index = l * num6 + n;
				onMesh.m_V[index] = on3fPoint2;
				on3fVector.x = on3fPoint2.x - on3fPoint.x;
				on3fVector.y = on3fPoint2.y - on3fPoint.y;
				on3fVector.z = on3fPoint2.z - on3fPoint.z;
				on3fVector.Unitize();
				onMesh.m_N[index] = on3fVector;
				if (n == 0)
				{
					arg = on3fPoint2;
				}
			}
		}
		for (int num10 = 1; num10 < num5; num10++)
		{
			for (int num11 = 1; num11 <= num6; num11++)
			{
				onMesh.m_F.AppendQuad((num10 - 1) * num6 + (num11 - 1), (num10 - 1) * num6 + num11 % num6, num10 * num6 + num11 % num6, num10 * num6 + num11 - 1);
			}
		}
		int num12 = num6 / 4;
		OnMesh onMesh2 = new OnMesh();
		On3dPoint on3dPoint2 = curve.PointAtStart();
		on3fPoint.x = (float)on3dPoint2.x;
		on3fPoint.y = (float)on3dPoint2.y;
		on3fPoint.z = (float)on3dPoint2.z;
		onXform.ChangeBasis(new OnPlane(on3dPoint2, -curve.TangentAt(curve.Domain().Min())), OnPlane.World_xy);
		onXform2.Scale(1.0 + num3 * 0.0, 1.0 + num3 * 0.0, 1.0 + num3 * 0.0);
		for (int num13 = 0; num13 <= num6; num13++)
		{
			OnXform onXform3 = new OnXform();
			On3dVector on3dVector = nurbs_curve.PointAt(t2[num13 % num6]) - OnUtil.On_origin;
			on3dVector.Unitize();
			onXform3.Rotation(OnUtil.On_PI / 2.0, on3dVector, OnUtil.On_origin);
			OnNurbsCurve onNurbsCurve = nurbs_curve.Duplicate();
			if (!onNurbsCurve.ChangeClosedCurveSeam(t2[num13 % num6]))
			{
				continue;
			}
			onNurbsCurve.SetDomain(new OnInterval(0.0, 1.0));
			OnXform arg3 = onXform3;
			for (int num14 = 0; num14 < num12 + 1; num14++)
			{
				On3dPoint on3dPoint3;
				if (num14 != num12)
				{
					on3dPoint3 = onNurbsCurve.PointAt(t2[num14]);
					on3dPoint3.Transform(arg3);
				}
				else
				{
					on3dPoint3 = new On3dPoint(0.0, 0.0, startRadius);
				}
				onMesh2.m_V.Append(on3dPoint3);
				On3dVector on3dVector2 = on3dPoint3 - OnUtil.On_origin;
				on3dVector2.Unitize();
				on3dVector2.Reverse();
				onMesh2.m_N.Append(on3dVector2);
			}
			if (num13 > 0)
			{
				for (int num15 = 1; num15 < num12 + 1; num15++)
				{
					onMesh2.m_F.AppendQuad((num13 - 1 + num6) % num6 * (num12 + 1) + (num15 - 1), (num13 - 1 + num6) % num6 * (num12 + 1) + num15, num13 % num6 * (num12 + 1) + num15, num13 % num6 * (num12 + 1) + num15 - 1);
				}
			}
		}
		OnMesh onMesh3 = onMesh2.Duplicate();
		onMesh2.Transform(onXform);
		onXform2.Scale(endRadius / startRadius, endRadius / startRadius, endRadius / startRadius);
		onXform.ChangeBasis(new OnPlane(curve.PointAtEnd(), curve.TangentAt(curve.Domain().Max())), OnPlane.World_xy);
		onMesh3.Transform(onXform * onXform2);
		onMesh.Append(onMesh2);
		onMesh.Append(onMesh3);
		return onMesh;
	}

	public static Mesh MeshPipe(Curve icurve, Point3d[] ipoints, double[] radii, double tolerance)
	{
		IOnCurve onCurve = (IOnCurve)Interop.ToOnCurve(icurve);
		IOn3dPoint[] array = new IOn3dPoint[ipoints.Length];
		for (int i = 0; i < ipoints.Length; i++)
		{
			array[i] = CSConvert.Point3dCopyToOn3dPoint(ipoints[i]);
		}
		double length = 0.0;
		onCurve.GetLength(ref length);
		if (length < tolerance)
		{
			return null;
		}
		if (array.Length < 2)
		{
			return null;
		}
		if (array.Length != radii.Length)
		{
			return null;
		}
		double num = radii[0];
		double num2 = radii[radii.Length - 1];
		List<double> list = new List<double>();
		IOn3dPoint[] array2 = array;
		foreach (IOn3dPoint arg in array2)
		{
			double t = 0.0;
			onCurve.GetClosestPoint(arg, ref t);
			list.Add(t);
		}
		OnCircle onCircle = new OnCircle(new OnPlane(OnUtil.On_origin, OnUtil.On_zaxis), num);
		double num3 = Math.Max(num, num2 / 2.0) * 2.0 * OnUtil.On_PI;
		OnNurbsCurve nurbs_curve = new OnNurbsCurve();
		onCircle.GetNurbForm(ref nurbs_curve);
		nurbs_curve.SetDomain(new OnInterval(0.0, 1.0));
		double num4 = (int)(num3 / tolerance);
		List<double> list2 = new List<double>();
		for (int k = 0; (double)k < num4; k++)
		{
			list2.Add((double)k / num4);
		}
		double[] t2 = new double[list2.Count];
		nurbs_curve.GetNormalizedArcLengthPoints(list2.ToArray(), ref t2);
		OnMesh onMesh = new OnMesh((list.Count - 1) * t2.Length, list.Count * t2.Length, has_vertex_normals: true, has_texture_coords: false);
		OnXform onXform = new OnXform();
		OnXform onXform2 = new OnXform();
		On3fPoint arg2 = new On3fPoint();
		On3fVector on3fVector = new On3fVector();
		On3fPoint on3fPoint = new On3fPoint();
		int count = list.Count;
		int num5 = t2.Length;
		On3fPointArray on3fPointArray = new On3fPointArray(num5);
		for (int l = 0; l < num5; l++)
		{
			on3fPointArray.Append(nurbs_curve.PointAt(t2[l]));
		}
		onMesh.m_V.SetCount(list.Count * num5);
		onMesh.m_N.SetCount(list.Count * num5);
		for (int m = 0; m < count; m++)
		{
			double t3 = list[m];
			On3dPoint on3dPoint = onCurve.PointAt(t3);
			on3fPoint.x = (float)on3dPoint.x;
			on3fPoint.y = (float)on3dPoint.y;
			on3fPoint.z = (float)on3dPoint.z;
			onXform.ChangeBasis(new OnPlane(on3dPoint, onCurve.TangentAt(t3)), OnPlane.World_xy);
			double num6 = radii[m] / num;
			onXform2.Scale(num6, num6, num6);
			OnXform arg3 = onXform * onXform2;
			int num7 = 0;
			double num8 = double.MaxValue;
			if (m > 0)
			{
				for (int n = 0; n < num5; n++)
				{
					On3fPoint on3fPoint2 = new On3fPoint(on3fPointArray[n]);
					on3fPoint2.Transform(arg3);
					double num9 = on3fPoint2.DistanceTo(arg2);
					if (num9 < num8)
					{
						num8 = num9;
						num7 = n;
					}
				}
			}
			for (int num10 = 0; num10 < num5; num10++)
			{
				On3fPoint on3fPoint2 = new On3fPoint(on3fPointArray[(num10 + num7) % num5]);
				on3fPoint2.Transform(arg3);
				int index = m * num5 + num10;
				onMesh.m_V[index] = on3fPoint2;
				on3fVector.x = on3fPoint2.x - on3fPoint.x;
				on3fVector.y = on3fPoint2.y - on3fPoint.y;
				on3fVector.z = on3fPoint2.z - on3fPoint.z;
				on3fVector.Unitize();
				onMesh.m_N[index] = on3fVector;
				if (num10 == 0)
				{
					arg2 = on3fPoint2;
				}
			}
		}
		for (int num11 = 1; num11 < count; num11++)
		{
			for (int num12 = 1; num12 <= num5; num12++)
			{
				onMesh.m_F.AppendQuad((num11 - 1) * num5 + (num12 - 1), (num11 - 1) * num5 + num12 % num5, num11 * num5 + num12 % num5, num11 * num5 + num12 - 1);
			}
		}
		int num13 = num5 / 4;
		OnMesh onMesh2 = new OnMesh();
		On3dPoint on3dPoint2 = onCurve.PointAt(list[0]);
		on3fPoint.x = (float)on3dPoint2.x;
		on3fPoint.y = (float)on3dPoint2.y;
		on3fPoint.z = (float)on3dPoint2.z;
		onXform.ChangeBasis(new OnPlane(on3dPoint2, -onCurve.TangentAt(list[0])), OnPlane.World_xy);
		double num14 = radii[0] / num;
		onXform2.Scale(num14, num14, num14);
		for (int num15 = 0; num15 <= num5; num15++)
		{
			OnXform onXform3 = new OnXform();
			On3dVector on3dVector = nurbs_curve.PointAt(t2[num15 % num5]) - OnUtil.On_origin;
			on3dVector.Unitize();
			onXform3.Rotation(OnUtil.On_PI / 2.0, on3dVector, OnUtil.On_origin);
			OnNurbsCurve onNurbsCurve = nurbs_curve.Duplicate();
			if (!onNurbsCurve.ChangeClosedCurveSeam(t2[num15 % num5]))
			{
				continue;
			}
			onNurbsCurve.SetDomain(new OnInterval(0.0, 1.0));
			OnXform arg4 = onXform3;
			for (int num16 = 0; num16 < num13 + 1; num16++)
			{
				On3dPoint on3dPoint3;
				if (num16 != num13)
				{
					on3dPoint3 = onNurbsCurve.PointAt(t2[num16]);
					on3dPoint3.Transform(arg4);
				}
				else
				{
					on3dPoint3 = new On3dPoint(0.0, 0.0, num);
				}
				onMesh2.m_V.Append(on3dPoint3);
				On3dVector on3dVector2 = on3dPoint3 - OnUtil.On_origin;
				on3dVector2.Unitize();
				on3dVector2.Reverse();
				onMesh2.m_N.Append(on3dVector2);
			}
			if (num15 > 0)
			{
				for (int num17 = 1; num17 < num13 + 1; num17++)
				{
					onMesh2.m_F.AppendQuad((num15 - 1 + num5) % num5 * (num13 + 1) + (num17 - 1), (num15 - 1 + num5) % num5 * (num13 + 1) + num17, num15 % num5 * (num13 + 1) + num17, num15 % num5 * (num13 + 1) + num17 - 1);
				}
			}
		}
		OnMesh onMesh3 = onMesh2.Duplicate();
		onMesh2.Transform(onXform);
		num14 = radii[radii.Length - 1] / num;
		onXform2.Scale(num14, num14, num14);
		onXform.ChangeBasis(new OnPlane(onCurve.PointAt(list[list.Count - 1]), onCurve.TangentAt(list[list.Count - 1])), OnPlane.World_xy);
		onMesh3.Transform(onXform * onXform2);
		onMesh.Append(onMesh2);
		onMesh.Append(onMesh3);
		return Interop.FromOnMesh(onMesh);
	}

	public static OnMesh MeshPipe(IOnCurve curve, double widthStart, double heightStart, double widthEnd, double heightEnd, double tolerance)
	{
		double length = 0.0;
		curve.GetLength(ref length);
		if (length < tolerance)
		{
			return null;
		}
		widthStart /= 2.0;
		widthEnd /= 2.0;
		heightStart /= 2.0;
		heightEnd /= 2.0;
		if (widthStart < tolerance)
		{
			widthStart = tolerance;
		}
		if (heightStart < tolerance)
		{
			heightStart = tolerance;
		}
		if (widthEnd < tolerance)
		{
			widthEnd = tolerance;
		}
		if (heightEnd < tolerance)
		{
			heightEnd = tolerance;
		}
		double num = Math.Max(widthStart, heightStart);
		double num2 = Math.Max(widthEnd, heightEnd);
		double num3 = Math.Max(10, (int)(length / tolerance));
		List<double> list = new List<double>();
		for (int i = 0; (double)i < num3; i++)
		{
			list.Add((double)i / num3);
		}
		list.Add(1.0);
		double[] t = new double[list.Count];
		curve.GetNormalizedArcLengthPoints(list.ToArray(), ref t);
		OnEllipse onEllipse = new OnEllipse(new OnPlane(OnUtil.On_origin, OnUtil.On_zaxis), widthStart, heightStart);
		double num4 = Math.Max(num, num2) * 2.0 * OnUtil.On_PI;
		double num5 = (widthEnd / widthStart - 1.0) / (double)(list.Count - 1);
		double num6 = (heightEnd / heightStart - 1.0) / (double)(list.Count - 1);
		OnNurbsCurve arg = new OnNurbsCurve();
		onEllipse.GetNurbForm(ref arg);
		arg.SetDomain(new OnInterval(0.0, 1.0));
		double num7 = (int)(num4 / tolerance);
		List<double> list2 = new List<double>();
		for (int j = 0; (double)j < num7; j++)
		{
			list2.Add((double)j / num7);
		}
		double[] t2 = new double[list2.Count];
		arg.GetNormalizedArcLengthPoints(list2.ToArray(), ref t2);
		_ = t.Length;
		_ = t2.Length;
		_ = t2.Length;
		_ = t2.Length;
		OnMesh onMesh = new OnMesh((t.Length - 1) * t2.Length, t.Length * t2.Length, has_vertex_normals: true, has_texture_coords: false);
		OnXform onXform = new OnXform();
		OnXform onXform2 = new OnXform();
		On3fPoint on3fPoint = new On3fPoint();
		On3fPoint arg2 = new On3fPoint();
		On3fVector on3fVector = new On3fVector();
		On3fPoint on3fPoint2 = new On3fPoint();
		int num8 = t.Length;
		int num9 = t2.Length;
		On3fPointArray on3fPointArray = new On3fPointArray(num9);
		for (int k = 0; k < num9; k++)
		{
			on3fPointArray.Append(arg.PointAt(t2[k]));
		}
		onMesh.m_V.SetCount(t.Length * num9);
		onMesh.m_N.SetCount(t.Length * num9);
		for (int l = 0; l < num8; l++)
		{
			double t3 = t[l];
			On3dPoint on3dPoint = curve.PointAt(t3);
			on3fPoint2.x = (float)on3dPoint.x;
			on3fPoint2.y = (float)on3dPoint.y;
			on3fPoint2.z = (float)on3dPoint.z;
			On3dVector on3dVector = new On3dVector(0.0, 0.0, 1.0);
			On3dVector on3dVector2 = curve.TangentAt(t3);
			if (on3dVector * on3dVector2 > 0.99)
			{
				on3dVector = new On3dVector(0.0, 1.0, 0.0);
			}
			On3dVector on3dVector3 = OnUtil.ON_CrossProduct(on3dVector, on3dVector2);
			on3dVector3.Unitize();
			On3dVector on3dVector4 = OnUtil.ON_CrossProduct(on3dVector2, on3dVector3);
			on3dVector4.Unitize();
			onXform.ChangeBasis(on3dPoint, on3dVector3, on3dVector4, on3dVector2, OnPlane.World_xy.origin, OnPlane.World_xy.xaxis, OnPlane.World_xy.yaxis, OnPlane.World_xy.zaxis);
			onXform2.Scale(1.0 + num5 * (double)l, 1.0 + num6 * (double)l, 1.0 + 0.0);
			OnXform arg3 = onXform * onXform2;
			int num10 = 0;
			double num11 = double.MaxValue;
			if (l > 0)
			{
				for (int m = 0; m < num9; m++)
				{
					on3fPoint = new On3fPoint(on3fPointArray[m]);
					on3fPoint.Transform(arg3);
					double num12 = on3fPoint.DistanceTo(arg2);
					if (num12 < num11)
					{
						num11 = num12;
						num10 = m;
					}
				}
			}
			for (int n = 0; n < num9; n++)
			{
				on3fPoint = new On3fPoint(on3fPointArray[(n + num10) % num9]);
				on3fPoint.Transform(arg3);
				int index = l * num9 + n;
				onMesh.m_V[index] = on3fPoint;
				on3fVector.x = on3fPoint.x - on3fPoint2.x;
				on3fVector.y = on3fPoint.y - on3fPoint2.y;
				on3fVector.z = on3fPoint.z - on3fPoint2.z;
				on3fVector.Unitize();
				onMesh.m_N[index] = on3fVector;
				if (n == 0)
				{
					arg2 = on3fPoint;
				}
			}
		}
		for (int num13 = 1; num13 < num8; num13++)
		{
			for (int num14 = 1; num14 <= num9; num14++)
			{
				onMesh.m_F.AppendQuad((num13 - 1) * num9 + (num14 - 1), (num13 - 1) * num9 + num14 % num9, num13 * num9 + num14 % num9, num13 * num9 + num14 - 1);
			}
		}
		OnMesh onMesh2 = new OnMesh();
		OnMesh onMesh3 = new OnMesh();
		int num15 = num9 / 4;
		if (widthStart == heightStart)
		{
			onMesh2 = new OnMesh();
			On3dPoint on3dPoint2 = curve.PointAtStart();
			on3fPoint2.x = (float)on3dPoint2.x;
			on3fPoint2.y = (float)on3dPoint2.y;
			on3fPoint2.z = (float)on3dPoint2.z;
			On3dVector on3dVector5 = new On3dVector(0.0, 0.0, 1.0);
			On3dVector on3dVector6 = curve.TangentAt(curve.Domain().Min());
			if (on3dVector5 * on3dVector6 > 0.99)
			{
				on3dVector5 = new On3dVector(0.0, 1.0, 0.0);
			}
			On3dVector on3dVector7 = OnUtil.ON_CrossProduct(on3dVector5, on3dVector6);
			On3dVector y = OnUtil.ON_CrossProduct(on3dVector7, on3dVector6);
			onXform.ChangeBasis(on3dPoint2, on3dVector7, y, -on3dVector6, OnPlane.World_xy.origin, OnPlane.World_xy.xaxis, OnPlane.World_xy.yaxis, OnPlane.World_xy.zaxis);
			onXform2.Scale(1.0 + num5 * 0.0, 1.0 + num6 * 0.0, 1.0);
			for (int num16 = 0; num16 <= num9; num16++)
			{
				OnXform onXform3 = new OnXform();
				On3dVector on3dVector8 = arg.PointAt(t2[num16 % num9]) - OnUtil.On_origin;
				on3dVector8.Unitize();
				onXform3.Rotation(OnUtil.On_PI / 2.0, on3dVector8, OnUtil.On_origin);
				OnNurbsCurve onNurbsCurve = arg.Duplicate();
				if (!onNurbsCurve.ChangeClosedCurveSeam(t2[num16 % num9]))
				{
					continue;
				}
				onNurbsCurve.SetDomain(new OnInterval(0.0, 1.0));
				OnXform arg4 = onXform3;
				for (int num17 = 0; num17 < num15 + 1; num17++)
				{
					On3dPoint on3dPoint3;
					if (num17 != num15)
					{
						on3dPoint3 = onNurbsCurve.PointAt(t2[num17]);
						on3dPoint3.Transform(arg4);
					}
					else
					{
						on3dPoint3 = new On3dPoint(0.0, 0.0, num);
					}
					onMesh2.m_V.Append(on3dPoint3);
					On3dVector on3dVector9 = on3dPoint3 - OnUtil.On_origin;
					on3dVector9.Unitize();
					on3dVector9.Reverse();
					onMesh2.m_N.Append(on3dVector9);
				}
				if (num16 > 0)
				{
					for (int num18 = 1; num18 < num15 + 1; num18++)
					{
						onMesh2.m_F.AppendQuad((num16 - 1 + num9) % num9 * (num15 + 1) + (num18 - 1), (num16 - 1 + num9) % num9 * (num15 + 1) + num18, num16 % num9 * (num15 + 1) + num18, num16 % num9 * (num15 + 1) + num18 - 1);
					}
				}
			}
			onMesh3 = onMesh2.Duplicate();
			onMesh2.Transform(onXform);
			onXform2.Scale(widthEnd / widthStart, heightEnd / heightStart, num2 / num);
			on3dVector5 = new On3dVector(0.0, 0.0, 1.0);
			on3dVector6 = curve.TangentAt(curve.Domain().Max());
			if (on3dVector5 * on3dVector6 > 0.99)
			{
				on3dVector5 = new On3dVector(0.0, 1.0, 0.0);
			}
			on3dVector7 = OnUtil.ON_CrossProduct(on3dVector5, on3dVector6);
			on3dVector7.Unitize();
			y = OnUtil.ON_CrossProduct(on3dVector7, on3dVector6);
			y.Unitize();
			onXform.ChangeBasis(curve.PointAtEnd(), on3dVector7, y, on3dVector6, OnPlane.World_xy.origin, OnPlane.World_xy.xaxis, OnPlane.World_xy.yaxis, OnPlane.World_xy.zaxis);
			onMesh3.Transform(onXform * onXform2);
		}
		else
		{
			onMesh2 = new OnMesh();
			onMesh3 = new OnMesh();
		}
		if (onMesh2 != null)
		{
			onMesh.Append(onMesh2);
		}
		if (onMesh3 != null)
		{
			onMesh.Append(onMesh3);
		}
		return onMesh;
	}

	public static OnBrep Sweep1(IOnCurve railCurve, IOnCurve profile)
	{
		IOnCurve[] profiles = new IOnCurve[1] { profile };
		double[] array = new double[1];
		On3dPoint on3dPoint = profile.PointAtStart();
		On3dPoint on3dPoint2 = profile.PointAtEnd();
		railCurve.GetClosestPoint((on3dPoint + on3dPoint2) / 2.0, ref array[0]);
		Sweep1(railCurve, profiles, array, out var breps, railCurve.IsClosed());
		if (breps == null)
		{
			return null;
		}
		if (breps.Length < 1)
		{
			return null;
		}
		return breps[0];
	}

	public static bool Sweep1(IOnCurve railCurve, IOnCurve profile, IOnCurve profile2, out OnBrep[] breps)
	{
		IOnCurve[] profiles = new IOnCurve[2] { profile, profile2 };
		double[] array = new double[2];
		On3dPoint on3dPoint = profile.PointAtStart();
		On3dPoint on3dPoint2 = profile.PointAtEnd();
		railCurve.GetClosestPoint((on3dPoint + on3dPoint2) / 2.0, ref array[0]);
		on3dPoint = profile2.PointAtStart();
		on3dPoint2 = profile2.PointAtEnd();
		railCurve.GetClosestPoint((on3dPoint + on3dPoint2) / 2.0, ref array[1]);
		return Sweep1(railCurve, profiles, array, out breps, railCurve.IsClosed());
	}

	public static bool Sweep1(IOnCurve inputRailCurve, IOnCurve[] profiles, double[] inputParameters, out OnBrep[] breps, bool closed)
	{
		breps = new OnBrep[0];
		if (inputRailCurve == null)
		{
			CSUtil.PrintLine("Missing Rail");
			return false;
		}
		OnCurve onCurve = inputRailCurve.DuplicateCurve();
		if (profiles == null)
		{
			CSUtil.PrintLine("Missing Profiles");
			return false;
		}
		if (inputParameters == null)
		{
			CSUtil.PrintLine("Missing Parameters");
			return false;
		}
		double[] array = (double[])inputParameters.Clone();
		if (profiles.Length == 0)
		{
			CSUtil.PrintLine("No Parameters");
			return false;
		}
		if (profiles.Length != array.Length)
		{
			CSUtil.PrintLine("Parameters  Profiles do not match");
			return false;
		}
		IOnCurve[] array2 = profiles;
		for (int i = 0; i < array2.Length; i++)
		{
			if (array2[i] == null)
			{
				CSUtil.PrintLine("Missing a profile");
				return false;
			}
		}
		if (onCurve.IsClosed())
		{
			On3dPointArray on3dPointArray = new On3dPointArray();
			double[] array3 = array;
			foreach (double t in array3)
			{
				on3dPointArray.Append(onCurve.PointAt(t));
			}
			onCurve.ChangeClosedCurveSeam(array[0]);
			array[0] = onCurve.Domain().Min();
			for (int j = 1; j < on3dPointArray.Count(); j++)
			{
				onCurve.GetClosestPoint(on3dPointArray[j], ref array[j]);
			}
		}
		MRhinoCurveObject mRhinoCurveObject = new MRhinoCurveObject();
		mRhinoCurveObject.SetCurve(onCurve);
		MRhinoPolyEdge mRhinoPolyEdge = new MRhinoPolyEdge();
		mRhinoPolyEdge.Create(mRhinoCurveObject.Curve(), mRhinoCurveObject);
		MArgsRhinoSweep1 args = new MArgsRhinoSweep1
		{
			m_angle_tolerance = CSUtil.AngleToleranceRadians,
			m_bClosed = (onCurve.IsClosed() && closed),
			m_bHaveRailPickPoint = false,
			m_bUsePivotPoint = false,
			m_miter_type = 0,
			m_planar_up = new On3dVector(OnUtil.On_zaxis),
			m_rail_curve = mRhinoPolyEdge,
			m_rebuild_count = -1,
			m_refit_tolerance = CSUtil.Tolerance,
			m_simplify = 0,
			m_style = 0,
			m_sweep_tolerance = CSUtil.Tolerance
		};
		args.set_m_bUsePoints(0, 0);
		args.set_m_bUsePoints(1, 0);
		List<OnCurve> list = new List<OnCurve>();
		int[] array4 = new int[profiles.Length];
		for (int k = 0; k < profiles.Length; k++)
		{
			array4[k] = k;
		}
		if (profiles.Length > 1)
		{
			array2 = profiles;
			for (int i = 0; i < array2.Length; i++)
			{
				_ = array2[i];
				for (int l = 0; l < profiles.Length - 1; l++)
				{
					if (array[array4[l]] > array[array4[l + 1]])
					{
						int num = array4[l];
						array4[l] = array4[l + 1];
						array4[l + 1] = num;
					}
				}
			}
		}
		for (int m = 0; m < profiles.Length; m++)
		{
			OnCurve onCurve2 = profiles[array4[m]].DuplicateCurve();
			if (onCurve2 == null)
			{
				CSUtil.PrintLine("Error with duplicate curve");
				return false;
			}
			args.m_rail_params.Append(array[array4[m]]);
			list.Add(onCurve2);
			MRhinoCurveObject mRhinoCurveObject2 = new MRhinoCurveObject();
			mRhinoCurveObject2.SetCurve(onCurve2);
			args.m_shape_objrefs.Append(new MRhinoObjRef(mRhinoCurveObject2));
		}
		args.m_shape_curves = list.ToArray();
		Array.Clear(breps, 0, breps.Length);
		return RhUtil.RhinoSweep1(ref args, out breps);
	}

	public static List<OnBrep> Sweep1C4Simple(OnCurve railCurve1, IOnCurve[] profiles, double[] param1, bool closed)
	{
		if (profiles.Length % 4 != 0)
		{
			return null;
		}
		for (int i = 0; i < profiles.Length; i++)
		{
			if (profiles[i] == null)
			{
				return null;
			}
		}
		OnBrep[] breps = null;
		OnBrep[] breps2 = null;
		OnBrep[] breps3 = null;
		OnBrep[] breps4 = null;
		IOnCurve[] array = new IOnCurve[profiles.Length / 4];
		IOnCurve[] array2 = new IOnCurve[profiles.Length / 4];
		IOnCurve[] array3 = new IOnCurve[profiles.Length / 4];
		IOnCurve[] array4 = new IOnCurve[profiles.Length / 4];
		for (int j = 0; j < profiles.Length / 4; j++)
		{
			array[j] = profiles[j * 4].DuplicateCurve();
			array2[j] = profiles[j * 4 + 1].DuplicateCurve();
			array3[j] = profiles[j * 4 + 2].DuplicateCurve();
			array4[j] = profiles[j * 4 + 3].DuplicateCurve();
		}
		if (!Sweep1(railCurve1, array, param1, out breps, railCurve1.IsClosed() && closed))
		{
			return null;
		}
		if (!Sweep1(railCurve1, array2, param1, out breps2, railCurve1.IsClosed() && closed))
		{
			return null;
		}
		if (!Sweep1(railCurve1, array3, param1, out breps3, railCurve1.IsClosed() && closed))
		{
			return null;
		}
		if (!Sweep1(railCurve1, array4, param1, out breps4, railCurve1.IsClosed() && closed))
		{
			return null;
		}
		List<OnBrep> list = breps.ToList();
		list.AddRange(breps2);
		list.AddRange(breps3);
		list.AddRange(breps4);
		return list;
	}

	public static bool Sweep2(IOnCurve railCurve, IOnCurve railCurve2, IOnCurve profile1, IOnCurve profile2, out OnBrep[] breps)
	{
		MArgsRhinoSweep2 args = new MArgsRhinoSweep2();
		MRhinoCurveObject mRhinoCurveObject = new MRhinoCurveObject();
		mRhinoCurveObject.SetCurve(railCurve.DuplicateCurve());
		MRhinoPolyEdge mRhinoPolyEdge = new MRhinoPolyEdge();
		mRhinoPolyEdge.Create(mRhinoCurveObject.Curve(), mRhinoCurveObject);
		args.set_m_rail_curves(0, (IRhinoPolyEdge)mRhinoPolyEdge.Duplicate());
		MRhinoCurveObject mRhinoCurveObject2 = new MRhinoCurveObject();
		mRhinoCurveObject2.SetCurve(railCurve2.DuplicateCurve());
		MRhinoPolyEdge mRhinoPolyEdge2 = new MRhinoPolyEdge();
		mRhinoPolyEdge2.Create(mRhinoCurveObject2.Curve(), mRhinoCurveObject2);
		args.set_m_rail_curves(1, (IRhinoPolyEdge)mRhinoPolyEdge2.Duplicate());
		List<IOnCurve> list = new List<IOnCurve>();
		if (profile1 != null)
		{
			list.Add(profile1);
		}
		if (profile2 != null)
		{
			list.Add(profile2);
		}
		for (int i = 0; i < list.Count; i++)
		{
			Arraydouble arraydouble = new Arraydouble();
			Arraydouble arraydouble2 = new Arraydouble();
			OnCurve onCurve = list[i].DuplicateCurve();
			double t = 0.0;
			double t2 = 0.0;
			On3dPoint on3dPoint = onCurve.PointAtStart();
			On3dPoint on3dPoint2 = onCurve.PointAtEnd();
			mRhinoPolyEdge.GetClosestPoint(on3dPoint, ref t);
			mRhinoPolyEdge.GetClosestPoint(on3dPoint2, ref t2);
			On3dPoint arg = mRhinoPolyEdge.PointAt(t);
			On3dPoint arg2 = mRhinoPolyEdge.PointAt(t2);
			if (on3dPoint.DistanceTo(arg) < on3dPoint2.DistanceTo(arg2))
			{
				arraydouble.Append(t);
			}
			else
			{
				arraydouble2.Append(t2);
			}
			mRhinoPolyEdge2.GetClosestPoint(on3dPoint, ref t);
			mRhinoPolyEdge2.GetClosestPoint(on3dPoint2, ref t2);
			arg = mRhinoPolyEdge2.PointAt(t);
			arg2 = mRhinoPolyEdge2.PointAt(t2);
			if (on3dPoint.DistanceTo(arg) < on3dPoint2.DistanceTo(arg2))
			{
				arraydouble.Append(t);
			}
			else
			{
				arraydouble2.Append(t2);
			}
			args.set_m_rail_params(0, arraydouble);
			args.set_m_rail_params(1, arraydouble2);
		}
		args.m_shape_curves = list.ToArray();
		args.m_bClosed = false;
		args.m_simplify = 0;
		args.m_rebuild_count = -1;
		args.m_refit_tolerance = CSUtil.Tolerance;
		args.m_sweep_tolerance = CSUtil.Tolerance;
		args.m_angle_tolerance = CSUtil.AngleToleranceRadians;
		args.m_bSameHeight = true;
		return RhUtil.RhinoSweep2(ref args, out breps);
	}

	public static bool Sweep2(IOnCurve railCurve1, IOnCurve railCurve2, IOnCurve[] profiles, double[] parameters1, double[] parameters2, out OnBrep[] breps)
	{
		return Sweep2(railCurve1, railCurve2, profiles, parameters1, parameters2, out breps, closed: false, maintainHeight: false);
	}

	public static bool Sweep2(IOnCurve railCurve1, IOnCurve railCurve2, IOnCurve[] profiles, double[] parameters1, double[] parameters2, out OnBrep[] breps, bool closed)
	{
		return Sweep2(railCurve1, railCurve2, profiles, parameters1, parameters2, out breps, closed, maintainHeight: false);
	}

	public static bool Sweep2(IOnCurve railCurve1, IOnCurve railCurve2, IOnCurve[] profiles, double[] parameters1In, double[] parameters2In, out OnBrep[] breps, bool closed, bool maintainHeight)
	{
		breps = new OnBrep[0];
		if (profiles == null)
		{
			CSUtil.PrintLine("Missing Profiles");
			return false;
		}
		if (parameters1In == null)
		{
			CSUtil.PrintLine("Missing Parameters");
			return false;
		}
		if (parameters2In == null)
		{
			CSUtil.PrintLine("Missing Parameters");
			return false;
		}
		if (profiles.Length == 0)
		{
			CSUtil.PrintLine("No Profiles");
			return false;
		}
		if (profiles.Length != parameters1In.Length)
		{
			CSUtil.PrintLine("No param1");
			return false;
		}
		if (profiles.Length != parameters2In.Length)
		{
			CSUtil.PrintLine("No param2");
			return false;
		}
		if (railCurve1 == null)
		{
			CSUtil.PrintLine("Missing Rail1");
			return false;
		}
		if (railCurve2 == null)
		{
			CSUtil.PrintLine("Missing Rail2");
			return false;
		}
		if (profiles.Length == 0)
		{
			CSUtil.PrintLine("No Parameters");
			return false;
		}
		OnCurve onCurve = railCurve1.DuplicateCurve();
		OnCurve onCurve2 = railCurve2.DuplicateCurve();
		double[] array = new double[parameters1In.Length];
		double[] array2 = new double[parameters2In.Length];
		if (onCurve.IsClosed() && onCurve2.IsClosed())
		{
			onCurve.ChangeClosedCurveSeam(parameters1In[0]);
			onCurve2.ChangeClosedCurveSeam(parameters2In[0]);
			array[0] = onCurve.Domain().Min();
			array2[0] = onCurve2.Domain().Min();
			if (profiles.Length > 1)
			{
				for (int i = 1; i < profiles.Length; i++)
				{
					double t = 0.0;
					onCurve.GetClosestPoint(railCurve1.PointAt(parameters1In[i]), ref t);
					array[i] = t;
					t = 0.0;
					onCurve2.GetClosestPoint(railCurve2.PointAt(parameters2In[i]), ref t);
					array2[i] = t;
				}
			}
		}
		else
		{
			for (int j = 0; j < profiles.Length; j++)
			{
				array[j] = parameters1In[j];
				array2[j] = parameters2In[j];
				if (array[j] < onCurve.Domain().Min())
				{
					array[j] = onCurve.Domain().Min();
				}
				if (array[j] > onCurve.Domain().Max())
				{
					array[j] = onCurve.Domain().Max();
				}
				if (array2[j] < onCurve2.Domain().Min())
				{
					array2[j] = onCurve2.Domain().Min();
				}
				if (array2[j] > onCurve2.Domain().Max())
				{
					array2[j] = onCurve2.Domain().Max();
				}
			}
		}
		int[] array3 = new int[profiles.Length];
		for (int k = 0; k < profiles.Length; k++)
		{
			array3[k] = k;
		}
		IOnCurve[] array4;
		if (profiles.Length > 1)
		{
			array4 = profiles;
			for (int l = 0; l < array4.Length; l++)
			{
				_ = array4[l];
				for (int m = 0; m < profiles.Length - 1; m++)
				{
					if (array[array3[m]] > array[array3[m + 1]])
					{
						int num = array3[m];
						array3[m] = array3[m + 1];
						array3[m + 1] = num;
					}
				}
			}
		}
		MRhinoCurveObject mRhinoCurveObject = new MRhinoCurveObject();
		mRhinoCurveObject.SetCurve(onCurve);
		MRhinoPolyEdge mRhinoPolyEdge = new MRhinoPolyEdge();
		mRhinoPolyEdge.Create(mRhinoCurveObject.Curve(), mRhinoCurveObject);
		MRhinoCurveObject mRhinoCurveObject2 = new MRhinoCurveObject();
		mRhinoCurveObject2.SetCurve(onCurve2);
		MRhinoPolyEdge mRhinoPolyEdge2 = new MRhinoPolyEdge();
		mRhinoPolyEdge2.Create(mRhinoCurveObject2.Curve(), mRhinoCurveObject2);
		MArgsRhinoSweep2 args = new MArgsRhinoSweep2
		{
			m_angle_tolerance = CSUtil.AngleToleranceRadians,
			m_bClosed = (railCurve1.IsClosed() && railCurve2.IsClosed() && closed),
			m_bSameHeight = maintainHeight
		};
		args.set_m_rail_curves(0, (IRhinoPolyEdge)mRhinoPolyEdge);
		args.set_m_rail_curves(1, (IRhinoPolyEdge)mRhinoPolyEdge2);
		args.m_rebuild_count = -1;
		args.m_refit_tolerance = CSUtil.Tolerance;
		args.m_simplify = 0;
		args.m_sweep_tolerance = CSUtil.Tolerance;
		List<OnCurve> list = new List<OnCurve>();
		Arraydouble arraydouble = new Arraydouble();
		Arraydouble arraydouble2 = new Arraydouble();
		for (int n = 0; n < profiles.Length; n++)
		{
			OnCurve onCurve3 = profiles[array3[n]].DuplicateCurve();
			if (onCurve3 != null)
			{
				arraydouble.Append(array[array3[n]]);
				arraydouble2.Append(array2[array3[n]]);
				list.Add(onCurve3);
			}
			else
			{
				CSUtil.PrintLine("Unable to duplicate curve");
			}
		}
		args.set_m_rail_params(0, arraydouble);
		args.set_m_rail_params(1, arraydouble2);
		MArgsRhinoSweep2 mArgsRhinoSweep = args;
		array4 = list.ToArray();
		mArgsRhinoSweep.m_shape_curves = array4;
		return RhUtil.RhinoSweep2(ref args, out breps);
	}

	public static OnBrep Sweep2C4(OnCurve railCurve1, OnCurve railCurve2, IOnCurve[] profiles, double[] param1, double[] param2, bool closed)
	{
		OnCurve rail = null;
		OnCurve rail2 = null;
		OnCurve rail3 = null;
		OnCurve rail4 = null;
		return Sweep2C4(railCurve1, railCurve2, profiles, param1, param2, closed, ref rail, ref rail2, ref rail3, ref rail4);
	}

	public static OnBrep Sweep2C4(OnCurve railCurve1, OnCurve railCurve2, IOnCurve[] profiles, double[] param1, double[] param2, bool closed, ref OnCurve rail11, ref OnCurve rail12, ref OnCurve rail21, ref OnCurve rail22)
	{
		if (profiles.Length % 4 != 0)
		{
			return null;
		}
		if (profiles.Any((IOnCurve profile) => profile == null))
		{
			return null;
		}
		IOnCurve[] array = new IOnCurve[profiles.Length / 4];
		IOnCurve[] array2 = new IOnCurve[profiles.Length / 4];
		for (int num = 0; num < profiles.Length / 4; num++)
		{
			array[num] = profiles[num * 4].DuplicateCurve();
			array2[num] = profiles[num * 4 + 2].DuplicateCurve();
		}
		if (Math.Abs(param2[0] - railCurve2.Domain().Min()) < 0.001)
		{
			param2[0] = railCurve2.Domain().Min();
		}
		if (!Sweep2(railCurve1, railCurve2, array, param1, param2, out var breps, railCurve1.IsClosed() && closed, maintainHeight: true))
		{
			return null;
		}
		if (!Sweep2(railCurve1, railCurve2, array2, param1, param2, out var breps2, railCurve1.IsClosed() && closed, maintainHeight: true))
		{
			return null;
		}
		OnBrep onBrep = CSUtilRhino.JoinAllBreps(breps, CSUtil.Tolerance);
		OnBrep onBrep2 = CSUtilRhino.JoinAllBreps(breps2, CSUtil.Tolerance);
		List<OnCurve> list = new List<OnCurve>();
		if (onBrep != null)
		{
			for (int num2 = 0; num2 < onBrep.m_T.Count(); num2++)
			{
				if (onBrep.m_T[num2].Edge() != null && !onBrep.m_T[num2].Edge().IsSmoothManifoldEdge(OnUtil.On_PI) && Math.Abs(onBrep.m_T[num2].TangentAt(onBrep.m_T[num2].Domain().Mid()).x) > 0.001)
				{
					list.Add(onBrep.m_T[num2].Edge().DuplicateCurve());
				}
			}
		}
		if (onBrep2 != null)
		{
			for (int num3 = 0; num3 < onBrep2.m_T.Count(); num3++)
			{
				if (onBrep2.m_T[num3].Edge() != null && !onBrep2.m_T[num3].Edge().IsSmoothManifoldEdge(OnUtil.On_PI) && Math.Abs(onBrep2.m_T[num3].TangentAt(onBrep2.m_T[num3].Domain().Mid()).x) > 0.001)
				{
					list.Add(onBrep2.m_T[num3].Edge().DuplicateCurve());
				}
			}
		}
		On3dPoint on3dPoint = profiles[1].PointAtStart();
		On3dPoint on3dPoint2 = profiles[3].PointAtStart();
		On3dPoint on3dPoint3 = profiles[1].PointAtEnd();
		On3dPoint on3dPoint4 = profiles[3].PointAtEnd();
		foreach (OnCurve item in list)
		{
			double t = 0.0;
			item.GetClosestPoint(on3dPoint, ref t);
			if (item.PointAt(t).DistanceTo(on3dPoint) < 0.0001)
			{
				rail11 = item;
			}
			item.GetClosestPoint(on3dPoint3, ref t);
			if (item.PointAt(t).DistanceTo(on3dPoint3) < 0.0001)
			{
				rail12 = item;
			}
			item.GetClosestPoint(on3dPoint2, ref t);
			if (item.PointAt(t).DistanceTo(on3dPoint2) < 0.0001)
			{
				rail21 = item;
			}
			item.GetClosestPoint(on3dPoint4, ref t);
			if (item.PointAt(t).DistanceTo(on3dPoint4) < 0.0001)
			{
				rail22 = item;
			}
		}
		if (rail11 == null || rail12 == null || rail22 == null || rail21 == null)
		{
			CSUtil.Print("Error creating Multirail sweep");
			return null;
		}
		OnBrep[] breps3 = null;
		OnBrep[] breps4 = null;
		IOnCurve[] array3 = new IOnCurve[profiles.Length / 4];
		IOnCurve[] array4 = new IOnCurve[profiles.Length / 4];
		for (int num4 = 0; num4 < profiles.Length / 4; num4++)
		{
			array3[num4] = profiles[num4 * 4 + 1];
			array4[num4] = profiles[num4 * 4 + 3];
		}
		Sweep2NoParam(rail11, rail12, array3, out breps3, maintainHeight: true, rail11.IsClosed(), rail11.IsClosed(), flipProfiles: true);
		rail21.Reverse();
		rail22.Reverse();
		Sweep2NoParam(rail22, rail21, array4, out breps4, maintainHeight: true, rail11.IsClosed(), rail11.IsClosed(), flipProfiles: true);
		List<OnBrep> list2 = breps3.ToList();
		list2.AddRange(breps4);
		list2.Add(onBrep);
		list2.Add(onBrep2);
		return CSUtilRhino.JoinAllBreps(list2.ToArray(), CSUtil.Tolerance * 2.0);
	}

	public static OnBrep Sweep2C4(OnCurve railCurve1, OnCurve railCurve2, IOnCurve[] profiles, bool closed)
	{
		if (profiles.Length % 4 != 0)
		{
			return null;
		}
		if (profiles.Any((IOnCurve onCurve6) => onCurve6 == null))
		{
			return null;
		}
		IOnCurve[] array = new IOnCurve[profiles.Length / 4];
		IOnCurve[] array2 = new IOnCurve[profiles.Length / 4];
		for (int num = 0; num < profiles.Length / 4; num++)
		{
			array[num] = profiles[num * 4].DuplicateCurve();
			OnCurve onCurve = profiles[num * 4 + 2].DuplicateCurve();
			if (array[num].PointAtStart().DistanceTo(onCurve.PointAtStart()) > array[num].PointAtEnd().DistanceTo(onCurve.PointAtStart()))
			{
				onCurve.Reverse();
			}
			array2[num] = onCurve;
		}
		Sweep2NoParam(railCurve1, railCurve2, array, out var breps, maintainHeight: true, railCurve1.IsClosed(), railCurve1.IsClosed() && closed, flipProfiles: false);
		Sweep2NoParam(railCurve1, railCurve2, array2, out var breps2, maintainHeight: true, railCurve1.IsClosed(), railCurve1.IsClosed() && closed, flipProfiles: false);
		OnBrep onBrep = CSUtilRhino.JoinAllBreps(breps, CSUtil.Tolerance);
		OnBrep onBrep2 = CSUtilRhino.JoinAllBreps(breps2, CSUtil.Tolerance);
		List<OnCurve> list = new List<OnCurve>();
		if (onBrep != null)
		{
			for (int num2 = 0; num2 < onBrep.m_T.Count(); num2++)
			{
				if (onBrep.m_T[num2].Edge() != null && !onBrep.m_T[num2].Edge().IsSmoothManifoldEdge(OnUtil.On_PI) && Math.Abs(onBrep.m_T[num2].TangentAt(onBrep.m_T[num2].Domain().Mid()).x) > 0.001)
				{
					list.Add(onBrep.m_T[num2].Edge().DuplicateCurve());
				}
			}
		}
		if (onBrep2 != null)
		{
			for (int num3 = 0; num3 < onBrep2.m_T.Count(); num3++)
			{
				if (onBrep2.m_T[num3].Edge() != null && !onBrep2.m_T[num3].Edge().IsSmoothManifoldEdge(OnUtil.On_PI) && Math.Abs(onBrep2.m_T[num3].TangentAt(onBrep2.m_T[num3].Domain().Mid()).x) > 0.001)
				{
					list.Add(onBrep2.m_T[num3].Edge().DuplicateCurve());
				}
			}
		}
		OnCurve onCurve2 = null;
		OnCurve onCurve3 = null;
		OnCurve onCurve4 = null;
		OnCurve onCurve5 = null;
		On3dPoint on3dPoint = profiles[1].PointAtStart();
		On3dPoint on3dPoint2 = profiles[3].PointAtStart();
		On3dPoint on3dPoint3 = profiles[1].PointAtEnd();
		On3dPoint on3dPoint4 = profiles[3].PointAtEnd();
		foreach (OnCurve item in list)
		{
			double t = 0.0;
			item.GetClosestPoint(on3dPoint, ref t);
			if (item.PointAt(t).DistanceTo(on3dPoint) < 0.0001)
			{
				onCurve2 = item;
			}
			item.GetClosestPoint(on3dPoint3, ref t);
			if (item.PointAt(t).DistanceTo(on3dPoint3) < 0.0001)
			{
				onCurve3 = item;
			}
			item.GetClosestPoint(on3dPoint2, ref t);
			if (item.PointAt(t).DistanceTo(on3dPoint2) < 0.0001)
			{
				onCurve4 = item;
			}
			item.GetClosestPoint(on3dPoint4, ref t);
			if (item.PointAt(t).DistanceTo(on3dPoint4) < 0.0001)
			{
				onCurve5 = item;
			}
		}
		if (onCurve2 == null || onCurve3 == null || onCurve5 == null || onCurve4 == null)
		{
			CSUtil.Print("Error creating Multirail sweep");
			return null;
		}
		IOnCurve[] array3 = new IOnCurve[profiles.Length / 4];
		IOnCurve[] array4 = new IOnCurve[profiles.Length / 4];
		for (int num4 = 0; num4 < profiles.Length / 4; num4++)
		{
			array3[num4] = profiles[num4 * 4 + 1];
			array4[num4] = profiles[num4 * 4 + 3];
		}
		Sweep2NoParam(onCurve2, onCurve3, array3, out var breps3, maintainHeight: true, onCurve2.IsClosed(), onCurve2.IsClosed(), flipProfiles: true);
		onCurve4.Reverse();
		onCurve5.Reverse();
		Sweep2NoParam(onCurve5, onCurve4, array4, out var breps4, maintainHeight: true, onCurve2.IsClosed(), onCurve2.IsClosed(), flipProfiles: true);
		List<OnBrep> list2 = breps3.ToList();
		list2.AddRange(breps4);
		list2.Add(onBrep);
		list2.Add(onBrep2);
		return CSUtilRhino.JoinAllBreps(list2.ToArray(), CSUtil.Tolerance * 2.0);
	}

	public static List<OnBrep> Sweep2C4Simple(OnCurve railCurve1, OnCurve railCurve2, IOnCurve[] profiles, double[] param1, double[] param2, bool closed)
	{
		if (profiles.Length % 4 != 0)
		{
			return null;
		}
		if (profiles.Any((IOnCurve t) => t == null))
		{
			return null;
		}
		IOnCurve[] array = new IOnCurve[profiles.Length / 4];
		IOnCurve[] array2 = new IOnCurve[profiles.Length / 4];
		IOnCurve[] array3 = new IOnCurve[profiles.Length / 4];
		IOnCurve[] array4 = new IOnCurve[profiles.Length / 4];
		for (int num = 0; num < profiles.Length / 4; num++)
		{
			array[num] = profiles[num * 4].DuplicateCurve();
			array2[num] = profiles[num * 4 + 1].DuplicateCurve();
			array3[num] = profiles[num * 4 + 2].DuplicateCurve();
			array4[num] = profiles[num * 4 + 3].DuplicateCurve();
		}
		if (Math.Abs(param2[0] - railCurve2.Domain().Min()) < 0.001)
		{
			param2[0] = 0.0;
		}
		if (!Sweep2(railCurve1, railCurve2, array, param1, param2, out var breps, railCurve1.IsClosed() && closed, maintainHeight: true))
		{
			return null;
		}
		if (!Sweep2(railCurve1, railCurve2, array2, param1, param2, out var breps2, railCurve1.IsClosed() && closed, maintainHeight: true))
		{
			return null;
		}
		if (!Sweep2(railCurve1, railCurve2, array3, param1, param2, out var breps3, railCurve1.IsClosed() && closed, maintainHeight: true))
		{
			return null;
		}
		if (!Sweep2(railCurve1, railCurve2, array4, param1, param2, out var breps4, railCurve1.IsClosed() && closed, maintainHeight: true))
		{
			return null;
		}
		List<OnBrep> list = breps.ToList();
		list.AddRange(breps2);
		list.AddRange(breps3);
		list.AddRange(breps4);
		return list;
	}

	public static bool Sweep2NoParam(IOnCurve railCurve1, IOnCurve railCurve2, IOnCurve[] profiles, out OnBrep[] breps, bool maintainHeight, bool firstAtStart, bool closed, bool flipProfiles)
	{
		OnCurve onCurve = railCurve1.DuplicateCurve();
		OnCurve onCurve2 = railCurve2.DuplicateCurve();
		List<OnCurve> list = (from profile in profiles
			where profile != null
			select profile.DuplicateCurve()).ToList();
		MatchCurveDirection(onCurve, onCurve2, 20);
		List<double> list2 = new List<double>();
		List<double> list3 = new List<double>();
		for (int num = 0; num < list.Count; num++)
		{
			double t = 0.0;
			double t2 = 0.0;
			bool flag = false;
			if (!list[num].IsClosed())
			{
				t = 0.0;
				t2 = 0.0;
				On3dPoint on3dPoint = list[num].PointAtStart();
				On3dPoint on3dPoint2 = list[num].PointAtEnd();
				onCurve.GetClosestPoint(on3dPoint, ref t);
				onCurve.GetClosestPoint(on3dPoint2, ref t2);
				On3dPoint arg = onCurve.PointAt(t);
				On3dPoint arg2 = onCurve.PointAt(t2);
				double num2 = on3dPoint.DistanceTo(arg);
				double num3 = on3dPoint2.DistanceTo(arg2);
				if (num2 < 0.01 || num3 < 0.01)
				{
					flag = true;
				}
				if (flipProfiles && num2 > num3)
				{
					list[num].Reverse();
				}
				if (flag)
				{
					onCurve.GetClosestPoint(list[num].PointAtStart(), ref t);
					onCurve2.GetClosestPoint(list[num].PointAtEnd(), ref t2);
				}
			}
			if (!flag)
			{
				On3dPointArray on3dPointArray = new On3dPointArray();
				List<double> obj = new List<double> { 0.0, 0.25, 0.33, 0.5, 0.66, 0.75 };
				double t3 = 0.0;
				foreach (double item in obj)
				{
					list[num].GetNormalizedArcLengthPoint(item, ref t3);
					on3dPointArray.Append(list[num].PointAt(t3));
				}
				double num4 = OnUtil.On_DBL_MAX;
				double num5 = OnUtil.On_DBL_MAX;
				for (int num6 = 0; num6 < on3dPointArray.Count(); num6++)
				{
					onCurve.GetClosestPoint(on3dPointArray[num6], ref t3);
					double num7 = onCurve.PointAt(t3).DistanceTo(on3dPointArray[num6]);
					if (num7 < num4)
					{
						t = t3;
						num4 = num7;
					}
					onCurve2.GetClosestPoint(on3dPointArray[num6], ref t3);
					num7 = onCurve2.PointAt(t3).DistanceTo(on3dPointArray[num6]);
					if (num7 < num5)
					{
						t2 = t3;
						num5 = num7;
					}
				}
			}
			if (num == 0 && firstAtStart)
			{
				if (onCurve.IsClosed())
				{
					onCurve.ChangeClosedCurveSeam(t);
					t = onCurve.Domain().Min();
				}
				if (onCurve2.IsClosed())
				{
					onCurve2.ChangeClosedCurveSeam(t2);
					t2 = onCurve2.Domain().Min();
				}
			}
			list2.Add(t);
			list3.Add(t2);
		}
		for (int num8 = 0; num8 < list.Count; num8++)
		{
			for (int num9 = num8 + 1; num9 < list.Count; num9++)
			{
				if (list2[num9] < list2[num8])
				{
					OnCurve value = list[num8];
					double value2 = list2[num8];
					double value3 = list3[num8];
					list[num8] = list[num9];
					list2[num8] = list2[num9];
					list3[num8] = list3[num9];
					list[num9] = value;
					list2[num9] = value2;
					list3[num9] = value3;
				}
			}
		}
		if (onCurve.IsClosed() && !closed && list2.Count > 0)
		{
			onCurve.Trim(new OnInterval(onCurve.Domain().Min(), list2[list2.Count - 1]));
		}
		if (onCurve2.IsClosed() && !closed && list3.Count > 0)
		{
			onCurve2.Trim(new OnInterval(onCurve2.Domain().Min(), list3[list3.Count - 1]));
		}
		IOnCurve[] array = new IOnCurve[list.Count];
		for (int num10 = 0; num10 < list.Count; num10++)
		{
			array[num10] = list[num10];
		}
		return Sweep2(onCurve, onCurve2, array, list2.ToArray(), list3.ToArray(), out breps, closed, maintainHeight);
	}

	public static bool Sweep2NoProfile(IOnCurve railCurve1, IOnCurve railCurve2, out OnBrep[] breps)
	{
		return Sweep2NoProfile(railCurve1, railCurve2, out breps, closed: false, 1);
	}

	public static bool Sweep2NoProfile(IOnCurve railCurve1, IOnCurve railCurve2, out OnBrep[] breps, bool closed, int profileCount)
	{
		breps = new OnBrep[0];
		if (railCurve1 == null || railCurve2 == null)
		{
			CSUtil.PrintLine("Missing rail");
			return false;
		}
		OnNurbsCurve onNurbsCurve = railCurve1.NurbsCurve();
		OnNurbsCurve onNurbsCurve2 = railCurve2.NurbsCurve();
		if (!RhUtil.RhinoDoCurveDirectionsMatch(onNurbsCurve, onNurbsCurve2))
		{
			onNurbsCurve2.Reverse();
		}
		List<IOnCurve> list = new List<IOnCurve>();
		List<double> list2 = new List<double>();
		List<double> list3 = new List<double>();
		if (profileCount < 1)
		{
			profileCount = 1;
		}
		for (int i = 0; i < profileCount; i++)
		{
			double t = onNurbsCurve.Domain().Min();
			double t2 = onNurbsCurve2.Domain().Min();
			onNurbsCurve.GetNormalizedArcLengthPoint((double)i / (double)profileCount, ref t);
			onNurbsCurve2.GetNormalizedArcLengthPoint((double)i / (double)profileCount, ref t2);
			list2.Add(t);
			list3.Add(t2);
			OnLineCurve item = new OnLineCurve(onNurbsCurve.PointAt(t), onNurbsCurve2.PointAt(t2));
			list.Add(item);
		}
		return Sweep2(onNurbsCurve, onNurbsCurve2, list.ToArray(), list2.ToArray(), list3.ToArray(), out breps, closed, maintainHeight: false);
	}

	private static double CalculateMatchDirectionValue(ArrayOn3dPoint p1, ArrayOn3dPoint p2, int offset1, int offset2, bool reverse)
	{
		if (p1.Count() != p2.Count())
		{
			return 0.0;
		}
		int num = p1.Count();
		double num2 = 0.0;
		if (!reverse)
		{
			for (int i = 0; i < p1.Count(); i++)
			{
				num2 += p1[(i + offset1) % num].DistanceTo(p2[(i + offset2) % num]);
			}
		}
		else
		{
			for (int j = 0; j < p1.Count(); j++)
			{
				num2 += p1[(j + offset1) % num].DistanceTo(p2[(num - j - 1 + offset2) % num]);
			}
		}
		return num2;
	}
}
