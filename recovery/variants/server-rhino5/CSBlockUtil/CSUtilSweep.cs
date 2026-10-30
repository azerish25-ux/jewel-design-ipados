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
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0006: Expected O, but got Unknown
		//IL_0006: Unknown result type (might be due to invalid IL or missing references)
		//IL_000c: Expected O, but got Unknown
		On3dPointArray val = new On3dPointArray();
		On3dPointArray val2 = new On3dPointArray();
		double[] array = new double[pointCount];
		double[] array2 = new double[pointCount];
		for (int i = 0; i < pointCount; i++)
		{
			array[i] = (double)i / (double)pointCount;
			array2[i] = (double)i / (double)pointCount;
		}
		curve1.GetNormalizedArcLengthPoints(array, ref array);
		curve2.GetNormalizedArcLengthPoints(array2, ref array2);
		for (int j = 0; j < array.Length; j++)
		{
			((ArrayOn3dPoint)val).Append((IOn3dPoint)(object)curve1.PointAt(array[j]));
			((ArrayOn3dPoint)val2).Append((IOn3dPoint)(object)curve2.PointAt(array2[j]));
		}
		int num = 0;
		int num2 = 0;
		double num3 = double.MaxValue;
		for (int k = 0; k < pointCount; k++)
		{
			for (int l = 0; l < pointCount; l++)
			{
				double num4 = ((ArrayOn3dPoint)val)[k].DistanceTo((IOn3dPoint)(object)((ArrayOn3dPoint)val2)[l]);
				if (num4 < num3)
				{
					num = k;
					num2 = l;
					num3 = num4;
				}
			}
		}
		ct1 = array[num];
		ct2 = array2[num2];
		return true;
	}

	public static OnBrep Loft(IOnCurve[] profiles, eLoftType loftType)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		return Loft(profiles, loftType, capped: false);
	}

	public static OnBrep Loft(IOnCurve[] profiles, eLoftType loftType, bool capped)
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0005: Unknown result type (might be due to invalid IL or missing references)
		//IL_000c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0013: Unknown result type (might be due to invalid IL or missing references)
		//IL_001a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0021: Unknown result type (might be due to invalid IL or missing references)
		//IL_0028: Unknown result type (might be due to invalid IL or missing references)
		//IL_0029: Unknown result type (might be due to invalid IL or missing references)
		//IL_002f: Expected I4, but got Unknown
		//IL_002f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0036: Unknown result type (might be due to invalid IL or missing references)
		//IL_003d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0044: Unknown result type (might be due to invalid IL or missing references)
		//IL_0050: Expected O, but got Unknown
		//IL_0073: Unknown result type (might be due to invalid IL or missing references)
		//IL_0078: Unknown result type (might be due to invalid IL or missing references)
		//IL_0086: Expected O, but got Unknown
		//IL_00a4: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ae: Expected O, but got Unknown
		//IL_0160: Unknown result type (might be due to invalid IL or missing references)
		//IL_0167: Expected O, but got Unknown
		MArgsRhinoLoft val = new MArgsRhinoLoft
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
		MRhinoLoftCurve[] array = (MRhinoLoftCurve[])(object)new MRhinoLoftCurve[num];
		for (int i = 0; i < num; i++)
		{
			IOnCurve val2 = profiles[i];
			if (val2 != null)
			{
				array[i] = new MRhinoLoftCurve
				{
					m_curve = val2.DuplicateCurve()
				};
				array[i].m_curve.RemoveShortSegments(OnUtil.On_ZERO_TOLERANCE, true);
				array[i].m_pick_point = new On3dPoint(OnUtil.On_UNSET_POINT);
				array[i].m_pick_t = -1.23432101234321E+308;
				array[i].m_trim = null;
				array[i].m_bClosed = array[i].m_curve.IsClosed();
				array[i].m_bPlanar = array[i].m_curve.IsPlanar(array[i].m_plane);
			}
		}
		IRhinoLoftCurve[] loftcurves = (IRhinoLoftCurve[])(object)array;
		val.m_loftcurves = loftcurves;
		OnNurbsSurface[] array2 = default(OnNurbsSurface[]);
		if (!RhUtil.RhinoSdkLoftSurface(val, ref array2))
		{
			return null;
		}
		if (array2 == null || array2.Length == 0)
		{
			return null;
		}
		OnBrep val3;
		if (array2.Length == 1)
		{
			val3 = ((OnGeometry)array2[0]).BrepForm();
		}
		else
		{
			List<OnBrep> list = new List<OnBrep>();
			OnNurbsSurface[] array3 = array2;
			foreach (OnNurbsSurface val4 in array3)
			{
				if (((OnObject)val4).IsValid())
				{
					OnBrep val5 = new OnBrep();
					val5.NewFace((IOnSurface)(object)val4);
					list.Add(val5);
				}
			}
			val3 = CSUtilRhino.JoinAllBreps(list.ToArray(), CSUtil.Tolerance);
		}
		if (val3 != null && capped)
		{
			val3 = CSUtilRhino.CapPlanarHoles(val3, CSUtil.Tolerance);
		}
		return val3;
	}

	public static OnBrep LoftMatchCurves(IOnCurve[] profiles, eLoftType loftType, bool capped)
	{
		//IL_004b: Unknown result type (might be due to invalid IL or missing references)
		List<OnCurve> list = new List<OnCurve>();
		OnCurve val = null;
		IOnCurve[] array = profiles;
		foreach (IOnCurve val2 in array)
		{
			if (val2 != null)
			{
				OnCurve val3 = val2.DuplicateCurve();
				if (val != null)
				{
					MatchCurveDirection2(val, val3, 50);
				}
				list.Add(val3);
				val = val3;
			}
		}
		array = (IOnCurve[])(object)list.ToArray();
		return Loft(array, loftType, capped);
	}

	public static bool MatchCurveDirection(OnCurve curve1, OnCurve curve2, int pointCount)
	{
		//IL_0060: Unknown result type (might be due to invalid IL or missing references)
		//IL_0066: Expected O, but got Unknown
		//IL_0066: Unknown result type (might be due to invalid IL or missing references)
		//IL_006c: Expected O, but got Unknown
		double[] array = new double[pointCount];
		double[] array2 = new double[pointCount];
		for (int i = 0; i < pointCount; i++)
		{
			array[i] = (double)i / (double)(curve1.IsClosed() ? pointCount : (pointCount - 1));
			array2[i] = (double)i / (double)(curve2.IsClosed() ? pointCount : (pointCount - 1));
		}
		curve1.GetNormalizedArcLengthPoints(array, ref array);
		curve2.GetNormalizedArcLengthPoints(array2, ref array2);
		On3dPointArray val = new On3dPointArray();
		On3dPointArray val2 = new On3dPointArray();
		for (int j = 0; j < array.Length; j++)
		{
			((ArrayOn3dPoint)val).Append((IOn3dPoint)(object)curve1.PointAt(array[j]));
			((ArrayOn3dPoint)val2).Append((IOn3dPoint)(object)curve2.PointAt(array2[j]));
		}
		double num = 0.0;
		double num2 = 0.0;
		int num3 = 0;
		int num4 = 0;
		double num5 = OnUtil.On_DBL_MAX;
		if (curve1.IsClosed() || curve2.IsClosed())
		{
			for (int k = 0; k < ((ArrayOn3dPoint)val2).Count(); k++)
			{
				if (((ArrayOn3dPoint)val)[0].DistanceTo((IOn3dPoint)(object)((ArrayOn3dPoint)val2)[k]) < num5)
				{
					num5 = ((ArrayOn3dPoint)val)[0].DistanceTo((IOn3dPoint)(object)((ArrayOn3dPoint)val2)[k]);
					num4 = k;
				}
			}
		}
		else
		{
			num4 = 0;
		}
		for (int l = 0; l < ((ArrayOn3dPoint)val).Count(); l++)
		{
			num += ((ArrayOn3dPoint)val)[(l + num3) % pointCount].DistanceTo((IOn3dPoint)(object)((ArrayOn3dPoint)val2)[(l + num4) % pointCount]);
			num2 += ((ArrayOn3dPoint)val)[(l + num3) % pointCount].DistanceTo((IOn3dPoint)(object)((ArrayOn3dPoint)val2)[(pointCount - l - 1 + num4) % pointCount]);
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
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0006: Expected O, but got Unknown
		//IL_0006: Unknown result type (might be due to invalid IL or missing references)
		//IL_000c: Expected O, but got Unknown
		On3dPointArray val = new On3dPointArray();
		On3dPointArray val2 = new On3dPointArray();
		List<double> list = new List<double>();
		List<double> list2 = new List<double>();
		for (int i = 0; i < pointCount; i++)
		{
			list.Add((double)i / (double)pointCount);
			list2.Add((double)i / (double)pointCount);
		}
		double[] array = list.ToArray();
		double[] array2 = list2.ToArray();
		curve1.GetNormalizedArcLengthPoints(array, ref array);
		curve2.GetNormalizedArcLengthPoints(array2, ref array2);
		for (int j = 0; j < array.Length; j++)
		{
			((ArrayOn3dPoint)val).Append((IOn3dPoint)(object)curve1.PointAt(array[j]));
			((ArrayOn3dPoint)val2).Append((IOn3dPoint)(object)curve2.PointAt(array2[j]));
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
				double num3 = CalculateMatchDirectionValue((ArrayOn3dPoint)(object)val, (ArrayOn3dPoint)(object)val2, offset, l, flag2);
				if (num3 < num)
				{
					num2 = l;
					flag = flag2;
					num = num3;
				}
			}
		}
		curve2.ChangeClosedCurveSeam(array2[num2]);
		if (flag)
		{
			curve2.Reverse();
		}
		return flag;
	}

	public static OnMesh MeshPipe(IOnCurve curve, IOn3dPoint[] points, double[] radii, double tolerance)
	{
		//IL_0082: Unknown result type (might be due to invalid IL or missing references)
		//IL_008d: Expected O, but got Unknown
		//IL_0088: Unknown result type (might be due to invalid IL or missing references)
		//IL_008f: Expected O, but got Unknown
		//IL_00b0: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b7: Expected O, but got Unknown
		//IL_00d5: Unknown result type (might be due to invalid IL or missing references)
		//IL_00df: Expected O, but got Unknown
		//IL_0145: Unknown result type (might be due to invalid IL or missing references)
		//IL_014c: Expected O, but got Unknown
		//IL_014c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0153: Expected O, but got Unknown
		//IL_0153: Unknown result type (might be due to invalid IL or missing references)
		//IL_015a: Expected O, but got Unknown
		//IL_015a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0161: Expected O, but got Unknown
		//IL_0161: Unknown result type (might be due to invalid IL or missing references)
		//IL_0168: Expected O, but got Unknown
		//IL_0168: Unknown result type (might be due to invalid IL or missing references)
		//IL_016f: Expected O, but got Unknown
		//IL_017f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0186: Expected O, but got Unknown
		//IL_0229: Unknown result type (might be due to invalid IL or missing references)
		//IL_0238: Expected O, but got Unknown
		//IL_03d2: Unknown result type (might be due to invalid IL or missing references)
		//IL_03d9: Expected O, but got Unknown
		//IL_042b: Unknown result type (might be due to invalid IL or missing references)
		//IL_043a: Expected O, but got Unknown
		//IL_02c6: Unknown result type (might be due to invalid IL or missing references)
		//IL_02cd: Expected O, but got Unknown
		//IL_027a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0281: Expected O, but got Unknown
		//IL_0457: Unknown result type (might be due to invalid IL or missing references)
		//IL_045e: Expected O, but got Unknown
		//IL_0630: Unknown result type (might be due to invalid IL or missing references)
		//IL_063f: Expected O, but got Unknown
		//IL_04cf: Unknown result type (might be due to invalid IL or missing references)
		//IL_04d9: Expected O, but got Unknown
		//IL_0515: Unknown result type (might be due to invalid IL or missing references)
		//IL_051c: Expected O, but got Unknown
		double num = 0.0;
		curve.GetLength(ref num);
		if (num < tolerance)
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
		double num2 = radii[0];
		double num3 = radii[radii.Length - 1];
		List<double> list = new List<double>();
		foreach (IOn3dPoint val in points)
		{
			double item = 0.0;
			curve.GetClosestPoint(val, ref item);
			list.Add(item);
		}
		OnCircle val2 = new OnCircle((IOnPlane)new OnPlane(OnUtil.On_origin, OnUtil.On_zaxis), num2);
		double num4 = Math.Max(num2, num3 / 2.0) * 2.0 * OnUtil.On_PI;
		OnNurbsCurve val3 = new OnNurbsCurve();
		val2.GetNurbForm(ref val3);
		((OnCurve)val3).SetDomain((IOnInterval)new OnInterval(0.0, 1.0));
		double num5 = (int)(num4 / tolerance);
		List<double> list2 = new List<double>();
		for (int j = 0; (double)j < num5; j++)
		{
			list2.Add((double)j / num5);
		}
		double[] array = new double[list2.Count];
		((OnCurve)val3).GetNormalizedArcLengthPoints(list2.ToArray(), ref array);
		OnMesh val4 = new OnMesh((list.Count - 1) * array.Length, list.Count * array.Length, true, false);
		OnXform val5 = new OnXform();
		OnXform val6 = new OnXform();
		On3fPoint val7 = new On3fPoint();
		On3fVector val8 = new On3fVector();
		On3fPoint val9 = new On3fPoint();
		int count = list.Count;
		int num6 = array.Length;
		On3fPointArray val10 = new On3fPointArray(num6);
		for (int k = 0; k < num6; k++)
		{
			((ArrayOn3fPoint)val10).Append((IOn3dPoint)(object)((OnCurve)val3).PointAt(array[k]));
		}
		((ArrayOn3fPoint)val4.m_V).SetCount(list.Count * num6);
		((ArrayOn3fVector)val4.m_N).SetCount(list.Count * num6);
		for (int l = 0; l < count; l++)
		{
			double num7 = list[l];
			On3dPoint val11 = curve.PointAt(num7);
			val9.x = (float)val11.x;
			val9.y = (float)val11.y;
			val9.z = (float)val11.z;
			val5.ChangeBasis((IOnPlane)new OnPlane((IOn3dPoint)(object)val11, (IOn3dVector)(object)curve.TangentAt(num7)), OnPlane.World_xy);
			double num8 = radii[l] / num2;
			val6.Scale(num8, num8, num8);
			OnXform val12 = val5 * (IOnXform)(object)val6;
			int num9 = 0;
			double num10 = double.MaxValue;
			if (l > 0)
			{
				for (int m = 0; m < num6; m++)
				{
					On3fPoint val13 = new On3fPoint((IOn3fPoint)(object)((ArrayOn3fPoint)val10)[m]);
					val13.Transform((IOnXform)(object)val12);
					double num11 = val13.DistanceTo((IOn3fPoint)(object)val7);
					if (num11 < num10)
					{
						num10 = num11;
						num9 = m;
					}
				}
			}
			for (int n = 0; n < num6; n++)
			{
				On3fPoint val13 = new On3fPoint((IOn3fPoint)(object)((ArrayOn3fPoint)val10)[(n + num9) % num6]);
				val13.Transform((IOnXform)(object)val12);
				int num12 = l * num6 + n;
				((ArrayOn3fPoint)val4.m_V)[num12] = val13;
				val8.x = val13.x - val9.x;
				val8.y = val13.y - val9.y;
				val8.z = val13.z - val9.z;
				val8.Unitize();
				((ArrayOn3fVector)val4.m_N)[num12] = val8;
				if (n == 0)
				{
					val7 = val13;
				}
			}
		}
		for (int num13 = 1; num13 < count; num13++)
		{
			for (int num14 = 1; num14 <= num6; num14++)
			{
				val4.m_F.AppendQuad((num13 - 1) * num6 + (num14 - 1), (num13 - 1) * num6 + num14 % num6, num13 * num6 + num14 % num6, num13 * num6 + num14 - 1);
			}
		}
		int num15 = num6 / 4;
		OnMesh val14 = new OnMesh();
		On3dPoint val15 = curve.PointAt(list[0]);
		val9.x = (float)val15.x;
		val9.y = (float)val15.y;
		val9.z = (float)val15.z;
		val5.ChangeBasis((IOnPlane)new OnPlane((IOn3dPoint)(object)val15, (IOn3dVector)(object)(-curve.TangentAt(list[0]))), OnPlane.World_xy);
		double num16 = radii[0] / num2;
		val6.Scale(num16, num16, num16);
		for (int num17 = 0; num17 <= num6; num17++)
		{
			OnXform val16 = new OnXform();
			On3dVector val17 = ((OnCurve)val3).PointAt(array[num17 % num6]) - OnUtil.On_origin;
			val17.Unitize();
			val16.Rotation(OnUtil.On_PI / 2.0, (IOn3dVector)(object)val17, OnUtil.On_origin);
			OnNurbsCurve val18 = val3.Duplicate();
			if (!((OnCurve)val18).ChangeClosedCurveSeam(array[num17 % num6]))
			{
				continue;
			}
			((OnCurve)val18).SetDomain((IOnInterval)new OnInterval(0.0, 1.0));
			OnXform val19 = val16;
			for (int num18 = 0; num18 < num15 + 1; num18++)
			{
				On3dPoint val20;
				if (num18 != num15)
				{
					val20 = ((OnCurve)val18).PointAt(array[num18]);
					val20.Transform((IOnXform)(object)val19);
				}
				else
				{
					val20 = new On3dPoint(0.0, 0.0, num2);
				}
				((ArrayOn3fPoint)val14.m_V).Append((IOn3dPoint)(object)val20);
				On3dVector val21 = val20 - OnUtil.On_origin;
				val21.Unitize();
				val21.Reverse();
				((ArrayOn3fVector)val14.m_N).Append((IOn3dVector)(object)val21);
			}
			if (num17 > 0)
			{
				for (int num19 = 1; num19 < num15 + 1; num19++)
				{
					val14.m_F.AppendQuad((num17 - 1 + num6) % num6 * (num15 + 1) + (num19 - 1), (num17 - 1 + num6) % num6 * (num15 + 1) + num19, num17 % num6 * (num15 + 1) + num19, num17 % num6 * (num15 + 1) + num19 - 1);
				}
			}
		}
		OnMesh val22 = val14.Duplicate();
		((OnGeometry)val14).Transform((IOnXform)(object)val5);
		num16 = radii[radii.Length - 1] / num2;
		val6.Scale(num16, num16, num16);
		val5.ChangeBasis((IOnPlane)new OnPlane((IOn3dPoint)(object)curve.PointAt(list[list.Count - 1]), (IOn3dVector)(object)curve.TangentAt(list[list.Count - 1])), OnPlane.World_xy);
		((OnGeometry)val22).Transform((IOnXform)(object)(val5 * (IOnXform)(object)val6));
		val4.Append((IOnMesh)(object)val14);
		val4.Append((IOnMesh)(object)val22);
		return val4;
	}

	public static OnMesh MeshPipe(IOnCurve curve, double startRadius, double endRadius, double tolerance)
	{
		//IL_0086: Unknown result type (might be due to invalid IL or missing references)
		//IL_0091: Expected O, but got Unknown
		//IL_008c: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c3: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ca: Expected O, but got Unknown
		//IL_00e6: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f0: Expected O, but got Unknown
		//IL_0152: Unknown result type (might be due to invalid IL or missing references)
		//IL_0159: Expected O, but got Unknown
		//IL_0159: Unknown result type (might be due to invalid IL or missing references)
		//IL_0160: Expected O, but got Unknown
		//IL_0160: Unknown result type (might be due to invalid IL or missing references)
		//IL_0167: Expected O, but got Unknown
		//IL_0167: Unknown result type (might be due to invalid IL or missing references)
		//IL_016e: Expected O, but got Unknown
		//IL_016e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0175: Expected O, but got Unknown
		//IL_0175: Unknown result type (might be due to invalid IL or missing references)
		//IL_017c: Expected O, but got Unknown
		//IL_0189: Unknown result type (might be due to invalid IL or missing references)
		//IL_0190: Expected O, but got Unknown
		//IL_0229: Unknown result type (might be due to invalid IL or missing references)
		//IL_0238: Expected O, but got Unknown
		//IL_03f4: Unknown result type (might be due to invalid IL or missing references)
		//IL_03fb: Expected O, but got Unknown
		//IL_044a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0459: Expected O, but got Unknown
		//IL_02e8: Unknown result type (might be due to invalid IL or missing references)
		//IL_02ef: Expected O, but got Unknown
		//IL_029c: Unknown result type (might be due to invalid IL or missing references)
		//IL_02a3: Expected O, but got Unknown
		//IL_04ab: Unknown result type (might be due to invalid IL or missing references)
		//IL_04b2: Expected O, but got Unknown
		//IL_066b: Unknown result type (might be due to invalid IL or missing references)
		//IL_067a: Expected O, but got Unknown
		//IL_0523: Unknown result type (might be due to invalid IL or missing references)
		//IL_052d: Expected O, but got Unknown
		//IL_0569: Unknown result type (might be due to invalid IL or missing references)
		//IL_0570: Expected O, but got Unknown
		double num = 0.0;
		curve.GetLength(ref num);
		if (num < tolerance)
		{
			return null;
		}
		if (startRadius < tolerance || endRadius < tolerance)
		{
			return null;
		}
		double num2 = Math.Max(10, (int)(num / tolerance));
		List<double> list = new List<double>();
		for (int i = 0; (double)i < num2; i++)
		{
			list.Add((double)i / num2);
		}
		list.Add(1.0);
		double[] array = new double[list.Count];
		curve.GetNormalizedArcLengthPoints(list.ToArray(), ref array);
		OnCircle val = new OnCircle((IOnPlane)new OnPlane(OnUtil.On_origin, OnUtil.On_zaxis), startRadius);
		double num3 = Math.Max(startRadius, endRadius) * 2.0 * OnUtil.On_PI;
		double num4 = (endRadius / startRadius - 1.0) / (double)(list.Count - 1);
		OnNurbsCurve val2 = new OnNurbsCurve();
		val.GetNurbForm(ref val2);
		((OnCurve)val2).SetDomain((IOnInterval)new OnInterval(0.0, 1.0));
		double num5 = (int)(num3 / tolerance);
		List<double> list2 = new List<double>();
		for (int j = 0; (double)j < num5; j++)
		{
			list2.Add((double)j / num5);
		}
		double[] array2 = new double[list2.Count];
		((OnCurve)val2).GetNormalizedArcLengthPoints(list2.ToArray(), ref array2);
		OnMesh val3 = new OnMesh((array.Length - 1) * array2.Length, array.Length * array2.Length, true, false);
		OnXform val4 = new OnXform();
		OnXform val5 = new OnXform();
		On3fPoint val6 = new On3fPoint();
		On3fVector val7 = new On3fVector();
		On3fPoint val8 = new On3fPoint();
		int num6 = array.Length;
		int num7 = array2.Length;
		On3fPointArray val9 = new On3fPointArray(num7);
		for (int k = 0; k < num7; k++)
		{
			((ArrayOn3fPoint)val9).Append((IOn3dPoint)(object)((OnCurve)val2).PointAt(array2[k]));
		}
		((ArrayOn3fPoint)val3.m_V).SetCount(array.Length * num7);
		((ArrayOn3fVector)val3.m_N).SetCount(array.Length * num7);
		for (int l = 0; l < num6; l++)
		{
			double num8 = array[l];
			On3dPoint val10 = curve.PointAt(num8);
			val8.x = (float)val10.x;
			val8.y = (float)val10.y;
			val8.z = (float)val10.z;
			val4.ChangeBasis((IOnPlane)new OnPlane((IOn3dPoint)(object)val10, (IOn3dVector)(object)curve.TangentAt(num8)), OnPlane.World_xy);
			val5.Scale(1.0 + num4 * (double)l, 1.0 + num4 * (double)l, 1.0 + num4 * (double)l);
			OnXform val11 = val4 * (IOnXform)(object)val5;
			int num9 = 0;
			double num10 = double.MaxValue;
			if (l > 0)
			{
				for (int m = 0; m < num7; m++)
				{
					On3fPoint val12 = new On3fPoint((IOn3fPoint)(object)((ArrayOn3fPoint)val9)[m]);
					val12.Transform((IOnXform)(object)val11);
					double num11 = val12.DistanceTo((IOn3fPoint)(object)val6);
					if (num11 < num10)
					{
						num10 = num11;
						num9 = m;
					}
				}
			}
			for (int n = 0; n < num7; n++)
			{
				On3fPoint val12 = new On3fPoint((IOn3fPoint)(object)((ArrayOn3fPoint)val9)[(n + num9) % num7]);
				val12.Transform((IOnXform)(object)val11);
				int num12 = l * num7 + n;
				((ArrayOn3fPoint)val3.m_V)[num12] = val12;
				val7.x = val12.x - val8.x;
				val7.y = val12.y - val8.y;
				val7.z = val12.z - val8.z;
				val7.Unitize();
				((ArrayOn3fVector)val3.m_N)[num12] = val7;
				if (n == 0)
				{
					val6 = val12;
				}
			}
		}
		for (int num13 = 1; num13 < num6; num13++)
		{
			for (int num14 = 1; num14 <= num7; num14++)
			{
				val3.m_F.AppendQuad((num13 - 1) * num7 + (num14 - 1), (num13 - 1) * num7 + num14 % num7, num13 * num7 + num14 % num7, num13 * num7 + num14 - 1);
			}
		}
		int num15 = num7 / 4;
		OnMesh val13 = new OnMesh();
		On3dPoint val14 = curve.PointAtStart();
		val8.x = (float)val14.x;
		val8.y = (float)val14.y;
		val8.z = (float)val14.z;
		val4.ChangeBasis((IOnPlane)new OnPlane((IOn3dPoint)(object)val14, (IOn3dVector)(object)(-curve.TangentAt(curve.Domain().Min()))), OnPlane.World_xy);
		val5.Scale(1.0 + num4 * 0.0, 1.0 + num4 * 0.0, 1.0 + num4 * 0.0);
		for (int num16 = 0; num16 <= num7; num16++)
		{
			OnXform val15 = new OnXform();
			On3dVector val16 = ((OnCurve)val2).PointAt(array2[num16 % num7]) - OnUtil.On_origin;
			val16.Unitize();
			val15.Rotation(OnUtil.On_PI / 2.0, (IOn3dVector)(object)val16, OnUtil.On_origin);
			OnNurbsCurve val17 = val2.Duplicate();
			if (!((OnCurve)val17).ChangeClosedCurveSeam(array2[num16 % num7]))
			{
				continue;
			}
			((OnCurve)val17).SetDomain((IOnInterval)new OnInterval(0.0, 1.0));
			OnXform val18 = val15;
			for (int num17 = 0; num17 < num15 + 1; num17++)
			{
				On3dPoint val19;
				if (num17 != num15)
				{
					val19 = ((OnCurve)val17).PointAt(array2[num17]);
					val19.Transform((IOnXform)(object)val18);
				}
				else
				{
					val19 = new On3dPoint(0.0, 0.0, startRadius);
				}
				((ArrayOn3fPoint)val13.m_V).Append((IOn3dPoint)(object)val19);
				On3dVector val20 = val19 - OnUtil.On_origin;
				val20.Unitize();
				val20.Reverse();
				((ArrayOn3fVector)val13.m_N).Append((IOn3dVector)(object)val20);
			}
			if (num16 > 0)
			{
				for (int num18 = 1; num18 < num15 + 1; num18++)
				{
					val13.m_F.AppendQuad((num16 - 1 + num7) % num7 * (num15 + 1) + (num18 - 1), (num16 - 1 + num7) % num7 * (num15 + 1) + num18, num16 % num7 * (num15 + 1) + num18, num16 % num7 * (num15 + 1) + num18 - 1);
				}
			}
		}
		OnMesh val21 = val13.Duplicate();
		((OnGeometry)val13).Transform((IOnXform)(object)val4);
		val5.Scale(endRadius / startRadius, endRadius / startRadius, endRadius / startRadius);
		val4.ChangeBasis((IOnPlane)new OnPlane((IOn3dPoint)(object)curve.PointAtEnd(), (IOn3dVector)(object)curve.TangentAt(curve.Domain().Max())), OnPlane.World_xy);
		((OnGeometry)val21).Transform((IOnXform)(object)(val4 * (IOnXform)(object)val5));
		val3.Append((IOnMesh)(object)val13);
		val3.Append((IOnMesh)(object)val21);
		return val3;
	}

	public static Mesh MeshPipe(Curve icurve, Point3d[] ipoints, double[] radii, double tolerance)
	{
		//IL_0006: Unknown result type (might be due to invalid IL or missing references)
		//IL_000c: Expected O, but got Unknown
		//IL_00bd: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c8: Expected O, but got Unknown
		//IL_00c3: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ca: Expected O, but got Unknown
		//IL_00ec: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f3: Expected O, but got Unknown
		//IL_0111: Unknown result type (might be due to invalid IL or missing references)
		//IL_011b: Expected O, but got Unknown
		//IL_0183: Unknown result type (might be due to invalid IL or missing references)
		//IL_018a: Expected O, but got Unknown
		//IL_018a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0191: Expected O, but got Unknown
		//IL_0191: Unknown result type (might be due to invalid IL or missing references)
		//IL_0198: Expected O, but got Unknown
		//IL_0198: Unknown result type (might be due to invalid IL or missing references)
		//IL_019f: Expected O, but got Unknown
		//IL_019f: Unknown result type (might be due to invalid IL or missing references)
		//IL_01a6: Expected O, but got Unknown
		//IL_01a6: Unknown result type (might be due to invalid IL or missing references)
		//IL_01ad: Expected O, but got Unknown
		//IL_01be: Unknown result type (might be due to invalid IL or missing references)
		//IL_01c5: Expected O, but got Unknown
		//IL_026b: Unknown result type (might be due to invalid IL or missing references)
		//IL_027a: Expected O, but got Unknown
		//IL_0414: Unknown result type (might be due to invalid IL or missing references)
		//IL_041b: Expected O, but got Unknown
		//IL_046f: Unknown result type (might be due to invalid IL or missing references)
		//IL_047e: Expected O, but got Unknown
		//IL_0308: Unknown result type (might be due to invalid IL or missing references)
		//IL_030f: Expected O, but got Unknown
		//IL_02bc: Unknown result type (might be due to invalid IL or missing references)
		//IL_02c3: Expected O, but got Unknown
		//IL_049b: Unknown result type (might be due to invalid IL or missing references)
		//IL_04a2: Expected O, but got Unknown
		//IL_0678: Unknown result type (might be due to invalid IL or missing references)
		//IL_0687: Expected O, but got Unknown
		//IL_0513: Unknown result type (might be due to invalid IL or missing references)
		//IL_051d: Expected O, but got Unknown
		//IL_0559: Unknown result type (might be due to invalid IL or missing references)
		//IL_0560: Expected O, but got Unknown
		IOnCurve val = (IOnCurve)Interop.ToOnCurve(icurve);
		IOn3dPoint[] array = (IOn3dPoint[])(object)new IOn3dPoint[ipoints.Length];
		for (int i = 0; i < ipoints.Length; i++)
		{
			array[i] = (IOn3dPoint)(object)CSConvert.Point3dCopyToOn3dPoint(ipoints[i]);
		}
		double num = 0.0;
		val.GetLength(ref num);
		if (num < tolerance)
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
		double num2 = radii[0];
		double num3 = radii[radii.Length - 1];
		List<double> list = new List<double>();
		IOn3dPoint[] array2 = array;
		foreach (IOn3dPoint val2 in array2)
		{
			double item = 0.0;
			val.GetClosestPoint(val2, ref item);
			list.Add(item);
		}
		OnCircle val3 = new OnCircle((IOnPlane)new OnPlane(OnUtil.On_origin, OnUtil.On_zaxis), num2);
		double num4 = Math.Max(num2, num3 / 2.0) * 2.0 * OnUtil.On_PI;
		OnNurbsCurve val4 = new OnNurbsCurve();
		val3.GetNurbForm(ref val4);
		((OnCurve)val4).SetDomain((IOnInterval)new OnInterval(0.0, 1.0));
		double num5 = (int)(num4 / tolerance);
		List<double> list2 = new List<double>();
		for (int k = 0; (double)k < num5; k++)
		{
			list2.Add((double)k / num5);
		}
		double[] array3 = new double[list2.Count];
		((OnCurve)val4).GetNormalizedArcLengthPoints(list2.ToArray(), ref array3);
		OnMesh val5 = new OnMesh((list.Count - 1) * array3.Length, list.Count * array3.Length, true, false);
		OnXform val6 = new OnXform();
		OnXform val7 = new OnXform();
		On3fPoint val8 = new On3fPoint();
		On3fVector val9 = new On3fVector();
		On3fPoint val10 = new On3fPoint();
		int count = list.Count;
		int num6 = array3.Length;
		On3fPointArray val11 = new On3fPointArray(num6);
		for (int l = 0; l < num6; l++)
		{
			((ArrayOn3fPoint)val11).Append((IOn3dPoint)(object)((OnCurve)val4).PointAt(array3[l]));
		}
		((ArrayOn3fPoint)val5.m_V).SetCount(list.Count * num6);
		((ArrayOn3fVector)val5.m_N).SetCount(list.Count * num6);
		for (int m = 0; m < count; m++)
		{
			double num7 = list[m];
			On3dPoint val12 = val.PointAt(num7);
			val10.x = (float)val12.x;
			val10.y = (float)val12.y;
			val10.z = (float)val12.z;
			val6.ChangeBasis((IOnPlane)new OnPlane((IOn3dPoint)(object)val12, (IOn3dVector)(object)val.TangentAt(num7)), OnPlane.World_xy);
			double num8 = radii[m] / num2;
			val7.Scale(num8, num8, num8);
			OnXform val13 = val6 * (IOnXform)(object)val7;
			int num9 = 0;
			double num10 = double.MaxValue;
			if (m > 0)
			{
				for (int n = 0; n < num6; n++)
				{
					On3fPoint val14 = new On3fPoint((IOn3fPoint)(object)((ArrayOn3fPoint)val11)[n]);
					val14.Transform((IOnXform)(object)val13);
					double num11 = val14.DistanceTo((IOn3fPoint)(object)val8);
					if (num11 < num10)
					{
						num10 = num11;
						num9 = n;
					}
				}
			}
			for (int num12 = 0; num12 < num6; num12++)
			{
				On3fPoint val14 = new On3fPoint((IOn3fPoint)(object)((ArrayOn3fPoint)val11)[(num12 + num9) % num6]);
				val14.Transform((IOnXform)(object)val13);
				int num13 = m * num6 + num12;
				((ArrayOn3fPoint)val5.m_V)[num13] = val14;
				val9.x = val14.x - val10.x;
				val9.y = val14.y - val10.y;
				val9.z = val14.z - val10.z;
				val9.Unitize();
				((ArrayOn3fVector)val5.m_N)[num13] = val9;
				if (num12 == 0)
				{
					val8 = val14;
				}
			}
		}
		for (int num14 = 1; num14 < count; num14++)
		{
			for (int num15 = 1; num15 <= num6; num15++)
			{
				val5.m_F.AppendQuad((num14 - 1) * num6 + (num15 - 1), (num14 - 1) * num6 + num15 % num6, num14 * num6 + num15 % num6, num14 * num6 + num15 - 1);
			}
		}
		int num16 = num6 / 4;
		OnMesh val15 = new OnMesh();
		On3dPoint val16 = val.PointAt(list[0]);
		val10.x = (float)val16.x;
		val10.y = (float)val16.y;
		val10.z = (float)val16.z;
		val6.ChangeBasis((IOnPlane)new OnPlane((IOn3dPoint)(object)val16, (IOn3dVector)(object)(-val.TangentAt(list[0]))), OnPlane.World_xy);
		double num17 = radii[0] / num2;
		val7.Scale(num17, num17, num17);
		for (int num18 = 0; num18 <= num6; num18++)
		{
			OnXform val17 = new OnXform();
			On3dVector val18 = ((OnCurve)val4).PointAt(array3[num18 % num6]) - OnUtil.On_origin;
			val18.Unitize();
			val17.Rotation(OnUtil.On_PI / 2.0, (IOn3dVector)(object)val18, OnUtil.On_origin);
			OnNurbsCurve val19 = val4.Duplicate();
			if (!((OnCurve)val19).ChangeClosedCurveSeam(array3[num18 % num6]))
			{
				continue;
			}
			((OnCurve)val19).SetDomain((IOnInterval)new OnInterval(0.0, 1.0));
			OnXform val20 = val17;
			for (int num19 = 0; num19 < num16 + 1; num19++)
			{
				On3dPoint val21;
				if (num19 != num16)
				{
					val21 = ((OnCurve)val19).PointAt(array3[num19]);
					val21.Transform((IOnXform)(object)val20);
				}
				else
				{
					val21 = new On3dPoint(0.0, 0.0, num2);
				}
				((ArrayOn3fPoint)val15.m_V).Append((IOn3dPoint)(object)val21);
				On3dVector val22 = val21 - OnUtil.On_origin;
				val22.Unitize();
				val22.Reverse();
				((ArrayOn3fVector)val15.m_N).Append((IOn3dVector)(object)val22);
			}
			if (num18 > 0)
			{
				for (int num20 = 1; num20 < num16 + 1; num20++)
				{
					val15.m_F.AppendQuad((num18 - 1 + num6) % num6 * (num16 + 1) + (num20 - 1), (num18 - 1 + num6) % num6 * (num16 + 1) + num20, num18 % num6 * (num16 + 1) + num20, num18 % num6 * (num16 + 1) + num20 - 1);
				}
			}
		}
		OnMesh val23 = val15.Duplicate();
		((OnGeometry)val15).Transform((IOnXform)(object)val6);
		num17 = radii[radii.Length - 1] / num2;
		val7.Scale(num17, num17, num17);
		val6.ChangeBasis((IOnPlane)new OnPlane((IOn3dPoint)(object)val.PointAt(list[list.Count - 1]), (IOn3dVector)(object)val.TangentAt(list[list.Count - 1])), OnPlane.World_xy);
		((OnGeometry)val23).Transform((IOnXform)(object)(val6 * (IOnXform)(object)val7));
		val5.Append((IOnMesh)(object)val15);
		val5.Append((IOnMesh)(object)val23);
		return Interop.FromOnMesh(val5);
	}

	public static OnMesh MeshPipe(IOnCurve curve, double widthStart, double heightStart, double widthEnd, double heightEnd, double tolerance)
	{
		//IL_00ef: Unknown result type (might be due to invalid IL or missing references)
		//IL_00fb: Expected O, but got Unknown
		//IL_00f6: Unknown result type (might be due to invalid IL or missing references)
		//IL_0149: Unknown result type (might be due to invalid IL or missing references)
		//IL_0150: Expected O, but got Unknown
		//IL_016c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0176: Expected O, but got Unknown
		//IL_01ef: Unknown result type (might be due to invalid IL or missing references)
		//IL_01f6: Expected O, but got Unknown
		//IL_01f6: Unknown result type (might be due to invalid IL or missing references)
		//IL_01fd: Expected O, but got Unknown
		//IL_01fd: Unknown result type (might be due to invalid IL or missing references)
		//IL_0204: Expected O, but got Unknown
		//IL_0204: Unknown result type (might be due to invalid IL or missing references)
		//IL_020b: Expected O, but got Unknown
		//IL_020b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0212: Expected O, but got Unknown
		//IL_0212: Unknown result type (might be due to invalid IL or missing references)
		//IL_0219: Expected O, but got Unknown
		//IL_0219: Unknown result type (might be due to invalid IL or missing references)
		//IL_0220: Expected O, but got Unknown
		//IL_022e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0235: Expected O, but got Unknown
		//IL_02e0: Unknown result type (might be due to invalid IL or missing references)
		//IL_02e7: Expected O, but got Unknown
		//IL_0320: Unknown result type (might be due to invalid IL or missing references)
		//IL_0327: Expected O, but got Unknown
		//IL_053d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0544: Expected O, but got Unknown
		//IL_0544: Unknown result type (might be due to invalid IL or missing references)
		//IL_054b: Expected O, but got Unknown
		//IL_091e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0925: Expected O, but got Unknown
		//IL_0925: Unknown result type (might be due to invalid IL or missing references)
		//IL_092c: Expected O, but got Unknown
		//IL_0558: Unknown result type (might be due to invalid IL or missing references)
		//IL_055f: Expected O, but got Unknown
		//IL_05af: Unknown result type (might be due to invalid IL or missing references)
		//IL_05b6: Expected O, but got Unknown
		//IL_0437: Unknown result type (might be due to invalid IL or missing references)
		//IL_043e: Expected O, but got Unknown
		//IL_03eb: Unknown result type (might be due to invalid IL or missing references)
		//IL_03f2: Expected O, but got Unknown
		//IL_05f8: Unknown result type (might be due to invalid IL or missing references)
		//IL_05ff: Expected O, but got Unknown
		//IL_0696: Unknown result type (might be due to invalid IL or missing references)
		//IL_069d: Expected O, but got Unknown
		//IL_0859: Unknown result type (might be due to invalid IL or missing references)
		//IL_0860: Expected O, but got Unknown
		//IL_070e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0718: Expected O, but got Unknown
		//IL_08a2: Unknown result type (might be due to invalid IL or missing references)
		//IL_08a9: Expected O, but got Unknown
		//IL_0754: Unknown result type (might be due to invalid IL or missing references)
		//IL_075b: Expected O, but got Unknown
		double num = 0.0;
		curve.GetLength(ref num);
		if (num < tolerance)
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
		double num2 = Math.Max(widthStart, heightStart);
		double num3 = Math.Max(widthEnd, heightEnd);
		double num4 = Math.Max(10, (int)(num / tolerance));
		List<double> list = new List<double>();
		for (int i = 0; (double)i < num4; i++)
		{
			list.Add((double)i / num4);
		}
		list.Add(1.0);
		double[] array = new double[list.Count];
		curve.GetNormalizedArcLengthPoints(list.ToArray(), ref array);
		OnEllipse val = new OnEllipse((IOnPlane)new OnPlane(OnUtil.On_origin, OnUtil.On_zaxis), widthStart, heightStart);
		double num5 = Math.Max(num2, num3) * 2.0 * OnUtil.On_PI;
		double num6 = (widthEnd / widthStart - 1.0) / (double)(list.Count - 1);
		double num7 = (heightEnd / heightStart - 1.0) / (double)(list.Count - 1);
		OnNurbsCurve val2 = new OnNurbsCurve();
		val.GetNurbForm(ref val2);
		((OnCurve)val2).SetDomain((IOnInterval)new OnInterval(0.0, 1.0));
		double num8 = (int)(num5 / tolerance);
		List<double> list2 = new List<double>();
		for (int j = 0; (double)j < num8; j++)
		{
			list2.Add((double)j / num8);
		}
		double[] array2 = new double[list2.Count];
		((OnCurve)val2).GetNormalizedArcLengthPoints(list2.ToArray(), ref array2);
		_ = array.Length;
		_ = array2.Length;
		_ = array2.Length;
		_ = array2.Length;
		OnMesh val3 = new OnMesh((array.Length - 1) * array2.Length, array.Length * array2.Length, true, false);
		OnXform val4 = new OnXform();
		OnXform val5 = new OnXform();
		On3fPoint val6 = new On3fPoint();
		On3fPoint val7 = new On3fPoint();
		On3fVector val8 = new On3fVector();
		On3fPoint val9 = new On3fPoint();
		int num9 = array.Length;
		int num10 = array2.Length;
		On3fPointArray val10 = new On3fPointArray(num10);
		for (int k = 0; k < num10; k++)
		{
			((ArrayOn3fPoint)val10).Append((IOn3dPoint)(object)((OnCurve)val2).PointAt(array2[k]));
		}
		((ArrayOn3fPoint)val3.m_V).SetCount(array.Length * num10);
		((ArrayOn3fVector)val3.m_N).SetCount(array.Length * num10);
		for (int l = 0; l < num9; l++)
		{
			double num11 = array[l];
			On3dPoint val11 = curve.PointAt(num11);
			val9.x = (float)val11.x;
			val9.y = (float)val11.y;
			val9.z = (float)val11.z;
			On3dVector val12 = new On3dVector(0.0, 0.0, 1.0);
			On3dVector val13 = curve.TangentAt(num11);
			if (val12 * (IOn3dVector)(object)val13 > 0.99)
			{
				val12 = new On3dVector(0.0, 1.0, 0.0);
			}
			On3dVector val14 = OnUtil.ON_CrossProduct((IOn3dVector)(object)val12, (IOn3dVector)(object)val13);
			val14.Unitize();
			On3dVector val15 = OnUtil.ON_CrossProduct((IOn3dVector)(object)val13, (IOn3dVector)(object)val14);
			val15.Unitize();
			val4.ChangeBasis((IOn3dPoint)(object)val11, (IOn3dVector)(object)val14, (IOn3dVector)(object)val15, (IOn3dVector)(object)val13, OnPlane.World_xy.origin, OnPlane.World_xy.xaxis, OnPlane.World_xy.yaxis, OnPlane.World_xy.zaxis);
			val5.Scale(1.0 + num6 * (double)l, 1.0 + num7 * (double)l, 1.0 + 0.0);
			OnXform val16 = val4 * (IOnXform)(object)val5;
			int num12 = 0;
			double num13 = double.MaxValue;
			if (l > 0)
			{
				for (int m = 0; m < num10; m++)
				{
					val6 = new On3fPoint((IOn3fPoint)(object)((ArrayOn3fPoint)val10)[m]);
					val6.Transform((IOnXform)(object)val16);
					double num14 = val6.DistanceTo((IOn3fPoint)(object)val7);
					if (num14 < num13)
					{
						num13 = num14;
						num12 = m;
					}
				}
			}
			for (int n = 0; n < num10; n++)
			{
				val6 = new On3fPoint((IOn3fPoint)(object)((ArrayOn3fPoint)val10)[(n + num12) % num10]);
				val6.Transform((IOnXform)(object)val16);
				int num15 = l * num10 + n;
				((ArrayOn3fPoint)val3.m_V)[num15] = val6;
				val8.x = val6.x - val9.x;
				val8.y = val6.y - val9.y;
				val8.z = val6.z - val9.z;
				val8.Unitize();
				((ArrayOn3fVector)val3.m_N)[num15] = val8;
				if (n == 0)
				{
					val7 = val6;
				}
			}
		}
		for (int num16 = 1; num16 < num9; num16++)
		{
			for (int num17 = 1; num17 <= num10; num17++)
			{
				val3.m_F.AppendQuad((num16 - 1) * num10 + (num17 - 1), (num16 - 1) * num10 + num17 % num10, num16 * num10 + num17 % num10, num16 * num10 + num17 - 1);
			}
		}
		OnMesh val17 = new OnMesh();
		OnMesh val18 = new OnMesh();
		int num18 = num10 / 4;
		if (widthStart == heightStart)
		{
			val17 = new OnMesh();
			On3dPoint val19 = curve.PointAtStart();
			val9.x = (float)val19.x;
			val9.y = (float)val19.y;
			val9.z = (float)val19.z;
			On3dVector val20 = new On3dVector(0.0, 0.0, 1.0);
			On3dVector val21 = curve.TangentAt(curve.Domain().Min());
			if (val20 * (IOn3dVector)(object)val21 > 0.99)
			{
				val20 = new On3dVector(0.0, 1.0, 0.0);
			}
			On3dVector val22 = OnUtil.ON_CrossProduct((IOn3dVector)(object)val20, (IOn3dVector)(object)val21);
			On3dVector val23 = OnUtil.ON_CrossProduct((IOn3dVector)(object)val22, (IOn3dVector)(object)val21);
			val4.ChangeBasis((IOn3dPoint)(object)val19, (IOn3dVector)(object)val22, (IOn3dVector)(object)val23, (IOn3dVector)(object)(-val21), OnPlane.World_xy.origin, OnPlane.World_xy.xaxis, OnPlane.World_xy.yaxis, OnPlane.World_xy.zaxis);
			val5.Scale(1.0 + num6 * 0.0, 1.0 + num7 * 0.0, 1.0);
			for (int num19 = 0; num19 <= num10; num19++)
			{
				OnXform val24 = new OnXform();
				On3dVector val25 = ((OnCurve)val2).PointAt(array2[num19 % num10]) - OnUtil.On_origin;
				val25.Unitize();
				val24.Rotation(OnUtil.On_PI / 2.0, (IOn3dVector)(object)val25, OnUtil.On_origin);
				OnNurbsCurve val26 = val2.Duplicate();
				if (!((OnCurve)val26).ChangeClosedCurveSeam(array2[num19 % num10]))
				{
					continue;
				}
				((OnCurve)val26).SetDomain((IOnInterval)new OnInterval(0.0, 1.0));
				OnXform val27 = val24;
				for (int num20 = 0; num20 < num18 + 1; num20++)
				{
					On3dPoint val28;
					if (num20 != num18)
					{
						val28 = ((OnCurve)val26).PointAt(array2[num20]);
						val28.Transform((IOnXform)(object)val27);
					}
					else
					{
						val28 = new On3dPoint(0.0, 0.0, num2);
					}
					((ArrayOn3fPoint)val17.m_V).Append((IOn3dPoint)(object)val28);
					On3dVector val29 = val28 - OnUtil.On_origin;
					val29.Unitize();
					val29.Reverse();
					((ArrayOn3fVector)val17.m_N).Append((IOn3dVector)(object)val29);
				}
				if (num19 > 0)
				{
					for (int num21 = 1; num21 < num18 + 1; num21++)
					{
						val17.m_F.AppendQuad((num19 - 1 + num10) % num10 * (num18 + 1) + (num21 - 1), (num19 - 1 + num10) % num10 * (num18 + 1) + num21, num19 % num10 * (num18 + 1) + num21, num19 % num10 * (num18 + 1) + num21 - 1);
					}
				}
			}
			val18 = val17.Duplicate();
			((OnGeometry)val17).Transform((IOnXform)(object)val4);
			val5.Scale(widthEnd / widthStart, heightEnd / heightStart, num3 / num2);
			val20 = new On3dVector(0.0, 0.0, 1.0);
			val21 = curve.TangentAt(curve.Domain().Max());
			if (val20 * (IOn3dVector)(object)val21 > 0.99)
			{
				val20 = new On3dVector(0.0, 1.0, 0.0);
			}
			val22 = OnUtil.ON_CrossProduct((IOn3dVector)(object)val20, (IOn3dVector)(object)val21);
			val22.Unitize();
			val23 = OnUtil.ON_CrossProduct((IOn3dVector)(object)val22, (IOn3dVector)(object)val21);
			val23.Unitize();
			val4.ChangeBasis((IOn3dPoint)(object)curve.PointAtEnd(), (IOn3dVector)(object)val22, (IOn3dVector)(object)val23, (IOn3dVector)(object)val21, OnPlane.World_xy.origin, OnPlane.World_xy.xaxis, OnPlane.World_xy.yaxis, OnPlane.World_xy.zaxis);
			((OnGeometry)val18).Transform((IOnXform)(object)(val4 * (IOnXform)(object)val5));
		}
		else
		{
			val17 = new OnMesh();
			val18 = new OnMesh();
		}
		if (val17 != null)
		{
			val3.Append((IOnMesh)(object)val17);
		}
		if (val18 != null)
		{
			val3.Append((IOnMesh)(object)val18);
		}
		return val3;
	}

	public static OnBrep Sweep1(IOnCurve railCurve, IOnCurve profile)
	{
		IOnCurve[] profiles = (IOnCurve[])(object)new IOnCurve[1] { profile };
		double[] array = new double[1];
		On3dPoint val = profile.PointAtStart();
		On3dPoint val2 = profile.PointAtEnd();
		railCurve.GetClosestPoint((IOn3dPoint)(object)((val + (IOn3dPoint)(object)val2) / 2.0), ref array[0]);
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
		IOnCurve[] profiles = (IOnCurve[])(object)new IOnCurve[2] { profile, profile2 };
		double[] array = new double[2];
		On3dPoint val = profile.PointAtStart();
		On3dPoint val2 = profile.PointAtEnd();
		railCurve.GetClosestPoint((IOn3dPoint)(object)((val + (IOn3dPoint)(object)val2) / 2.0), ref array[0]);
		val = profile2.PointAtStart();
		val2 = profile2.PointAtEnd();
		railCurve.GetClosestPoint((IOn3dPoint)(object)((val + (IOn3dPoint)(object)val2) / 2.0), ref array[1]);
		return Sweep1(railCurve, profiles, array, out breps, railCurve.IsClosed());
	}

	public static bool Sweep1(IOnCurve inputRailCurve, IOnCurve[] profiles, double[] inputParameters, out OnBrep[] breps, bool closed)
	{
		//IL_0116: Unknown result type (might be due to invalid IL or missing references)
		//IL_011c: Expected O, but got Unknown
		//IL_0123: Unknown result type (might be due to invalid IL or missing references)
		//IL_0129: Expected O, but got Unknown
		//IL_0137: Unknown result type (might be due to invalid IL or missing references)
		//IL_013c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0147: Unknown result type (might be due to invalid IL or missing references)
		//IL_0156: Unknown result type (might be due to invalid IL or missing references)
		//IL_015d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0164: Unknown result type (might be due to invalid IL or missing references)
		//IL_016b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0171: Unknown result type (might be due to invalid IL or missing references)
		//IL_017b: Expected O, but got Unknown
		//IL_017b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0182: Unknown result type (might be due to invalid IL or missing references)
		//IL_0189: Unknown result type (might be due to invalid IL or missing references)
		//IL_0194: Unknown result type (might be due to invalid IL or missing references)
		//IL_019b: Unknown result type (might be due to invalid IL or missing references)
		//IL_01a2: Unknown result type (might be due to invalid IL or missing references)
		//IL_01af: Expected O, but got Unknown
		//IL_009d: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a4: Expected O, but got Unknown
		//IL_028e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0295: Expected O, but got Unknown
		//IL_02a7: Unknown result type (might be due to invalid IL or missing references)
		//IL_02b1: Expected O, but got Unknown
		breps = (OnBrep[])(object)new OnBrep[0];
		if (inputRailCurve == null)
		{
			CSUtil.PrintLine("Missing Rail");
			return false;
		}
		OnCurve val = inputRailCurve.DuplicateCurve();
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
		if (val.IsClosed())
		{
			On3dPointArray val2 = new On3dPointArray();
			double[] array3 = array;
			foreach (double num in array3)
			{
				((ArrayOn3dPoint)val2).Append((IOn3dPoint)(object)val.PointAt(num));
			}
			val.ChangeClosedCurveSeam(array[0]);
			array[0] = val.Domain().Min();
			for (int j = 1; j < ((ArrayOn3dPoint)val2).Count(); j++)
			{
				val.GetClosestPoint((IOn3dPoint)(object)((ArrayOn3dPoint)val2)[j], ref array[j]);
			}
		}
		MRhinoCurveObject val3 = new MRhinoCurveObject();
		val3.SetCurve(val);
		MRhinoPolyEdge val4 = new MRhinoPolyEdge();
		val4.Create(val3.Curve(), (IRhinoObject)(object)val3);
		MArgsRhinoSweep1 val5 = new MArgsRhinoSweep1
		{
			m_angle_tolerance = CSUtil.AngleToleranceRadians,
			m_bClosed = (val.IsClosed() && closed),
			m_bHaveRailPickPoint = false,
			m_bUsePivotPoint = false,
			m_miter_type = 0,
			m_planar_up = new On3dVector(OnUtil.On_zaxis),
			m_rail_curve = (IRhinoPolyEdge)(object)val4,
			m_rebuild_count = -1,
			m_refit_tolerance = CSUtil.Tolerance,
			m_simplify = 0,
			m_style = 0,
			m_sweep_tolerance = CSUtil.Tolerance
		};
		val5[0] = 0;
		val5[1] = 0;
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
						int num2 = array4[l];
						array4[l] = array4[l + 1];
						array4[l + 1] = num2;
					}
				}
			}
		}
		for (int m = 0; m < profiles.Length; m++)
		{
			OnCurve val6 = profiles[array4[m]].DuplicateCurve();
			if (val6 == null)
			{
				CSUtil.PrintLine("Error with duplicate curve");
				return false;
			}
			val5.m_rail_params.Append(array[array4[m]]);
			list.Add(val6);
			MRhinoCurveObject val7 = new MRhinoCurveObject();
			val7.SetCurve(val6);
			val5.m_shape_objrefs.Append((IRhinoObjRef)new MRhinoObjRef((IRhinoObject)(object)val7));
		}
		val5.m_shape_curves = list.ToArray();
		Array.Clear(breps, 0, breps.Length);
		return RhUtil.RhinoSweep1(ref val5, ref breps);
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
		IOnCurve[] array = (IOnCurve[])(object)new IOnCurve[profiles.Length / 4];
		IOnCurve[] array2 = (IOnCurve[])(object)new IOnCurve[profiles.Length / 4];
		IOnCurve[] array3 = (IOnCurve[])(object)new IOnCurve[profiles.Length / 4];
		IOnCurve[] array4 = (IOnCurve[])(object)new IOnCurve[profiles.Length / 4];
		for (int j = 0; j < profiles.Length / 4; j++)
		{
			array[j] = (IOnCurve)(object)profiles[j * 4].DuplicateCurve();
			array2[j] = (IOnCurve)(object)profiles[j * 4 + 1].DuplicateCurve();
			array3[j] = (IOnCurve)(object)profiles[j * 4 + 2].DuplicateCurve();
			array4[j] = (IOnCurve)(object)profiles[j * 4 + 3].DuplicateCurve();
		}
		if (!Sweep1((IOnCurve)(object)railCurve1, array, param1, out breps, railCurve1.IsClosed() && closed))
		{
			return null;
		}
		if (!Sweep1((IOnCurve)(object)railCurve1, array2, param1, out breps2, railCurve1.IsClosed() && closed))
		{
			return null;
		}
		if (!Sweep1((IOnCurve)(object)railCurve1, array3, param1, out breps3, railCurve1.IsClosed() && closed))
		{
			return null;
		}
		if (!Sweep1((IOnCurve)(object)railCurve1, array4, param1, out breps4, railCurve1.IsClosed() && closed))
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
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0006: Expected O, but got Unknown
		//IL_0006: Unknown result type (might be due to invalid IL or missing references)
		//IL_000c: Expected O, but got Unknown
		//IL_0018: Unknown result type (might be due to invalid IL or missing references)
		//IL_001e: Expected O, but got Unknown
		//IL_0039: Unknown result type (might be due to invalid IL or missing references)
		//IL_003f: Expected O, but got Unknown
		//IL_004b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0052: Expected O, but got Unknown
		//IL_0094: Unknown result type (might be due to invalid IL or missing references)
		//IL_009b: Expected O, but got Unknown
		//IL_009b: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a2: Expected O, but got Unknown
		MArgsRhinoSweep2 val = new MArgsRhinoSweep2();
		MRhinoCurveObject val2 = new MRhinoCurveObject();
		val2.SetCurve(railCurve.DuplicateCurve());
		MRhinoPolyEdge val3 = new MRhinoPolyEdge();
		val3.Create(val2.Curve(), (IRhinoObject)(object)val2);
		val[0] = (IRhinoPolyEdge)(object)val3.Duplicate();
		MRhinoCurveObject val4 = new MRhinoCurveObject();
		val4.SetCurve(railCurve2.DuplicateCurve());
		MRhinoPolyEdge val5 = new MRhinoPolyEdge();
		val5.Create(val4.Curve(), (IRhinoObject)(object)val4);
		val[1] = (IRhinoPolyEdge)(object)val5.Duplicate();
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
			Arraydouble val6 = new Arraydouble();
			Arraydouble val7 = new Arraydouble();
			OnCurve obj = list[i].DuplicateCurve();
			double num = 0.0;
			double num2 = 0.0;
			On3dPoint val8 = obj.PointAtStart();
			On3dPoint val9 = obj.PointAtEnd();
			((OnCurve)val3).GetClosestPoint((IOn3dPoint)(object)val8, ref num);
			((OnCurve)val3).GetClosestPoint((IOn3dPoint)(object)val9, ref num2);
			On3dPoint val10 = ((OnCurve)val3).PointAt(num);
			On3dPoint val11 = ((OnCurve)val3).PointAt(num2);
			if (val8.DistanceTo((IOn3dPoint)(object)val10) < val9.DistanceTo((IOn3dPoint)(object)val11))
			{
				val6.Append(num);
			}
			else
			{
				val7.Append(num2);
			}
			((OnCurve)val5).GetClosestPoint((IOn3dPoint)(object)val8, ref num);
			((OnCurve)val5).GetClosestPoint((IOn3dPoint)(object)val9, ref num2);
			val10 = ((OnCurve)val5).PointAt(num);
			val11 = ((OnCurve)val5).PointAt(num2);
			if (val8.DistanceTo((IOn3dPoint)(object)val10) < val9.DistanceTo((IOn3dPoint)(object)val11))
			{
				val6.Append(num);
			}
			else
			{
				val7.Append(num2);
			}
			val[0] = val6;
			val[1] = val7;
		}
		val.m_shape_curves = list.ToArray();
		val.m_bClosed = false;
		val.m_simplify = 0;
		val.m_rebuild_count = -1;
		val.m_refit_tolerance = CSUtil.Tolerance;
		val.m_sweep_tolerance = CSUtil.Tolerance;
		val.m_angle_tolerance = CSUtil.AngleToleranceRadians;
		val.m_bSameHeight = true;
		return RhUtil.RhinoSweep2(ref val, ref breps);
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
		//IL_029f: Unknown result type (might be due to invalid IL or missing references)
		//IL_02a6: Expected O, but got Unknown
		//IL_02ae: Unknown result type (might be due to invalid IL or missing references)
		//IL_02b5: Expected O, but got Unknown
		//IL_02c6: Unknown result type (might be due to invalid IL or missing references)
		//IL_02cd: Expected O, but got Unknown
		//IL_02d5: Unknown result type (might be due to invalid IL or missing references)
		//IL_02dc: Expected O, but got Unknown
		//IL_02ed: Unknown result type (might be due to invalid IL or missing references)
		//IL_02f2: Unknown result type (might be due to invalid IL or missing references)
		//IL_02fd: Unknown result type (might be due to invalid IL or missing references)
		//IL_0317: Unknown result type (might be due to invalid IL or missing references)
		//IL_0321: Expected O, but got Unknown
		//IL_0364: Unknown result type (might be due to invalid IL or missing references)
		//IL_036b: Expected O, but got Unknown
		//IL_036b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0372: Expected O, but got Unknown
		breps = (OnBrep[])(object)new OnBrep[0];
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
		OnCurve val = railCurve1.DuplicateCurve();
		OnCurve val2 = railCurve2.DuplicateCurve();
		double[] array = new double[parameters1In.Length];
		double[] array2 = new double[parameters2In.Length];
		if (val.IsClosed() && val2.IsClosed())
		{
			val.ChangeClosedCurveSeam(parameters1In[0]);
			val2.ChangeClosedCurveSeam(parameters2In[0]);
			array[0] = val.Domain().Min();
			array2[0] = val2.Domain().Min();
			if (profiles.Length > 1)
			{
				for (int i = 1; i < profiles.Length; i++)
				{
					double num = 0.0;
					val.GetClosestPoint((IOn3dPoint)(object)railCurve1.PointAt(parameters1In[i]), ref num);
					array[i] = num;
					num = 0.0;
					val2.GetClosestPoint((IOn3dPoint)(object)railCurve2.PointAt(parameters2In[i]), ref num);
					array2[i] = num;
				}
			}
		}
		else
		{
			for (int j = 0; j < profiles.Length; j++)
			{
				array[j] = parameters1In[j];
				array2[j] = parameters2In[j];
				if (array[j] < val.Domain().Min())
				{
					array[j] = val.Domain().Min();
				}
				if (array[j] > val.Domain().Max())
				{
					array[j] = val.Domain().Max();
				}
				if (array2[j] < val2.Domain().Min())
				{
					array2[j] = val2.Domain().Min();
				}
				if (array2[j] > val2.Domain().Max())
				{
					array2[j] = val2.Domain().Max();
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
						int num2 = array3[m];
						array3[m] = array3[m + 1];
						array3[m + 1] = num2;
					}
				}
			}
		}
		MRhinoCurveObject val3 = new MRhinoCurveObject();
		val3.SetCurve(val);
		MRhinoPolyEdge val4 = new MRhinoPolyEdge();
		val4.Create(val3.Curve(), (IRhinoObject)(object)val3);
		MRhinoCurveObject val5 = new MRhinoCurveObject();
		val5.SetCurve(val2);
		MRhinoPolyEdge val6 = new MRhinoPolyEdge();
		val6.Create(val5.Curve(), (IRhinoObject)(object)val5);
		MArgsRhinoSweep2 val7 = new MArgsRhinoSweep2
		{
			m_angle_tolerance = CSUtil.AngleToleranceRadians,
			m_bClosed = (railCurve1.IsClosed() && railCurve2.IsClosed() && closed),
			m_bSameHeight = maintainHeight
		};
		val7[0] = (IRhinoPolyEdge)(object)val4;
		val7[1] = (IRhinoPolyEdge)(object)val6;
		val7.m_rebuild_count = -1;
		val7.m_refit_tolerance = CSUtil.Tolerance;
		val7.m_simplify = 0;
		val7.m_sweep_tolerance = CSUtil.Tolerance;
		List<OnCurve> list = new List<OnCurve>();
		Arraydouble val8 = new Arraydouble();
		Arraydouble val9 = new Arraydouble();
		for (int n = 0; n < profiles.Length; n++)
		{
			OnCurve val10 = profiles[array3[n]].DuplicateCurve();
			if (val10 != null)
			{
				val8.Append(array[array3[n]]);
				val9.Append(array2[array3[n]]);
				list.Add(val10);
			}
			else
			{
				CSUtil.PrintLine("Unable to duplicate curve");
			}
		}
		val7[0] = val8;
		val7[1] = val9;
		MArgsRhinoSweep2 obj = val7;
		array4 = (IOnCurve[])(object)list.ToArray();
		obj.m_shape_curves = array4;
		return RhUtil.RhinoSweep2(ref val7, ref breps);
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
		IOnCurve[] array = (IOnCurve[])(object)new IOnCurve[profiles.Length / 4];
		IOnCurve[] array2 = (IOnCurve[])(object)new IOnCurve[profiles.Length / 4];
		for (int num = 0; num < profiles.Length / 4; num++)
		{
			array[num] = (IOnCurve)(object)profiles[num * 4].DuplicateCurve();
			array2[num] = (IOnCurve)(object)profiles[num * 4 + 2].DuplicateCurve();
		}
		if (Math.Abs(param2[0] - railCurve2.Domain().Min()) < 0.001)
		{
			param2[0] = railCurve2.Domain().Min();
		}
		if (!Sweep2((IOnCurve)(object)railCurve1, (IOnCurve)(object)railCurve2, array, param1, param2, out var breps, railCurve1.IsClosed() && closed, maintainHeight: true))
		{
			return null;
		}
		if (!Sweep2((IOnCurve)(object)railCurve1, (IOnCurve)(object)railCurve2, array2, param1, param2, out var breps2, railCurve1.IsClosed() && closed, maintainHeight: true))
		{
			return null;
		}
		OnBrep val = CSUtilRhino.JoinAllBreps(breps, CSUtil.Tolerance);
		OnBrep val2 = CSUtilRhino.JoinAllBreps(breps2, CSUtil.Tolerance);
		List<OnCurve> list = new List<OnCurve>();
		if (val != null)
		{
			for (int num2 = 0; num2 < val.m_T.Count(); num2++)
			{
				if (val.m_T[num2].Edge() != null && !val.m_T[num2].Edge().IsSmoothManifoldEdge(OnUtil.On_PI) && Math.Abs(((OnCurve)val.m_T[num2]).TangentAt(((OnCurve)val.m_T[num2]).Domain().Mid()).x) > 0.001)
				{
					list.Add(((OnCurve)val.m_T[num2].Edge()).DuplicateCurve());
				}
			}
		}
		if (val2 != null)
		{
			for (int num3 = 0; num3 < val2.m_T.Count(); num3++)
			{
				if (val2.m_T[num3].Edge() != null && !val2.m_T[num3].Edge().IsSmoothManifoldEdge(OnUtil.On_PI) && Math.Abs(((OnCurve)val2.m_T[num3]).TangentAt(((OnCurve)val2.m_T[num3]).Domain().Mid()).x) > 0.001)
				{
					list.Add(((OnCurve)val2.m_T[num3].Edge()).DuplicateCurve());
				}
			}
		}
		On3dPoint val3 = profiles[1].PointAtStart();
		On3dPoint val4 = profiles[3].PointAtStart();
		On3dPoint val5 = profiles[1].PointAtEnd();
		On3dPoint val6 = profiles[3].PointAtEnd();
		foreach (OnCurve item in list)
		{
			double num4 = 0.0;
			item.GetClosestPoint((IOn3dPoint)(object)val3, ref num4);
			if (item.PointAt(num4).DistanceTo((IOn3dPoint)(object)val3) < 0.0001)
			{
				rail11 = item;
			}
			item.GetClosestPoint((IOn3dPoint)(object)val5, ref num4);
			if (item.PointAt(num4).DistanceTo((IOn3dPoint)(object)val5) < 0.0001)
			{
				rail12 = item;
			}
			item.GetClosestPoint((IOn3dPoint)(object)val4, ref num4);
			if (item.PointAt(num4).DistanceTo((IOn3dPoint)(object)val4) < 0.0001)
			{
				rail21 = item;
			}
			item.GetClosestPoint((IOn3dPoint)(object)val6, ref num4);
			if (item.PointAt(num4).DistanceTo((IOn3dPoint)(object)val6) < 0.0001)
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
		IOnCurve[] array3 = (IOnCurve[])(object)new IOnCurve[profiles.Length / 4];
		IOnCurve[] array4 = (IOnCurve[])(object)new IOnCurve[profiles.Length / 4];
		for (int num5 = 0; num5 < profiles.Length / 4; num5++)
		{
			array3[num5] = profiles[num5 * 4 + 1];
			array4[num5] = profiles[num5 * 4 + 3];
		}
		Sweep2NoParam((IOnCurve)(object)rail11, (IOnCurve)(object)rail12, array3, out breps3, maintainHeight: true, rail11.IsClosed(), rail11.IsClosed(), flipProfiles: true);
		rail21.Reverse();
		rail22.Reverse();
		Sweep2NoParam((IOnCurve)(object)rail22, (IOnCurve)(object)rail21, array4, out breps4, maintainHeight: true, rail11.IsClosed(), rail11.IsClosed(), flipProfiles: true);
		List<OnBrep> list2 = breps3.ToList();
		list2.AddRange(breps4);
		list2.Add(val);
		list2.Add(val2);
		return CSUtilRhino.JoinAllBreps(list2.ToArray(), CSUtil.Tolerance * 2.0);
	}

	public static OnBrep Sweep2C4(OnCurve railCurve1, OnCurve railCurve2, IOnCurve[] profiles, bool closed)
	{
		if (profiles.Length % 4 != 0)
		{
			return null;
		}
		if (profiles.Any((IOnCurve t) => t == null))
		{
			return null;
		}
		IOnCurve[] array = (IOnCurve[])(object)new IOnCurve[profiles.Length / 4];
		IOnCurve[] array2 = (IOnCurve[])(object)new IOnCurve[profiles.Length / 4];
		for (int num = 0; num < profiles.Length / 4; num++)
		{
			array[num] = (IOnCurve)(object)profiles[num * 4].DuplicateCurve();
			OnCurve val = profiles[num * 4 + 2].DuplicateCurve();
			if (array[num].PointAtStart().DistanceTo((IOn3dPoint)(object)val.PointAtStart()) > array[num].PointAtEnd().DistanceTo((IOn3dPoint)(object)val.PointAtStart()))
			{
				val.Reverse();
			}
			array2[num] = (IOnCurve)(object)val;
		}
		Sweep2NoParam((IOnCurve)(object)railCurve1, (IOnCurve)(object)railCurve2, array, out var breps, maintainHeight: true, railCurve1.IsClosed(), railCurve1.IsClosed() && closed, flipProfiles: false);
		Sweep2NoParam((IOnCurve)(object)railCurve1, (IOnCurve)(object)railCurve2, array2, out var breps2, maintainHeight: true, railCurve1.IsClosed(), railCurve1.IsClosed() && closed, flipProfiles: false);
		OnBrep val2 = CSUtilRhino.JoinAllBreps(breps, CSUtil.Tolerance);
		OnBrep val3 = CSUtilRhino.JoinAllBreps(breps2, CSUtil.Tolerance);
		List<OnCurve> list = new List<OnCurve>();
		if (val2 != null)
		{
			for (int num2 = 0; num2 < val2.m_T.Count(); num2++)
			{
				if (val2.m_T[num2].Edge() != null && !val2.m_T[num2].Edge().IsSmoothManifoldEdge(OnUtil.On_PI) && Math.Abs(((OnCurve)val2.m_T[num2]).TangentAt(((OnCurve)val2.m_T[num2]).Domain().Mid()).x) > 0.001)
				{
					list.Add(((OnCurve)val2.m_T[num2].Edge()).DuplicateCurve());
				}
			}
		}
		if (val3 != null)
		{
			for (int num3 = 0; num3 < val3.m_T.Count(); num3++)
			{
				if (val3.m_T[num3].Edge() != null && !val3.m_T[num3].Edge().IsSmoothManifoldEdge(OnUtil.On_PI) && Math.Abs(((OnCurve)val3.m_T[num3]).TangentAt(((OnCurve)val3.m_T[num3]).Domain().Mid()).x) > 0.001)
				{
					list.Add(((OnCurve)val3.m_T[num3].Edge()).DuplicateCurve());
				}
			}
		}
		OnCurve val4 = null;
		OnCurve val5 = null;
		OnCurve val6 = null;
		OnCurve val7 = null;
		On3dPoint val8 = profiles[1].PointAtStart();
		On3dPoint val9 = profiles[3].PointAtStart();
		On3dPoint val10 = profiles[1].PointAtEnd();
		On3dPoint val11 = profiles[3].PointAtEnd();
		foreach (OnCurve item in list)
		{
			double num4 = 0.0;
			item.GetClosestPoint((IOn3dPoint)(object)val8, ref num4);
			if (item.PointAt(num4).DistanceTo((IOn3dPoint)(object)val8) < 0.0001)
			{
				val4 = item;
			}
			item.GetClosestPoint((IOn3dPoint)(object)val10, ref num4);
			if (item.PointAt(num4).DistanceTo((IOn3dPoint)(object)val10) < 0.0001)
			{
				val5 = item;
			}
			item.GetClosestPoint((IOn3dPoint)(object)val9, ref num4);
			if (item.PointAt(num4).DistanceTo((IOn3dPoint)(object)val9) < 0.0001)
			{
				val6 = item;
			}
			item.GetClosestPoint((IOn3dPoint)(object)val11, ref num4);
			if (item.PointAt(num4).DistanceTo((IOn3dPoint)(object)val11) < 0.0001)
			{
				val7 = item;
			}
		}
		if (val4 == null || val5 == null || val7 == null || val6 == null)
		{
			CSUtil.Print("Error creating Multirail sweep");
			return null;
		}
		IOnCurve[] array3 = (IOnCurve[])(object)new IOnCurve[profiles.Length / 4];
		IOnCurve[] array4 = (IOnCurve[])(object)new IOnCurve[profiles.Length / 4];
		for (int num5 = 0; num5 < profiles.Length / 4; num5++)
		{
			array3[num5] = profiles[num5 * 4 + 1];
			array4[num5] = profiles[num5 * 4 + 3];
		}
		Sweep2NoParam((IOnCurve)(object)val4, (IOnCurve)(object)val5, array3, out var breps3, maintainHeight: true, val4.IsClosed(), val4.IsClosed(), flipProfiles: true);
		val6.Reverse();
		val7.Reverse();
		Sweep2NoParam((IOnCurve)(object)val7, (IOnCurve)(object)val6, array4, out var breps4, maintainHeight: true, val4.IsClosed(), val4.IsClosed(), flipProfiles: true);
		List<OnBrep> list2 = breps3.ToList();
		list2.AddRange(breps4);
		list2.Add(val2);
		list2.Add(val3);
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
		IOnCurve[] array = (IOnCurve[])(object)new IOnCurve[profiles.Length / 4];
		IOnCurve[] array2 = (IOnCurve[])(object)new IOnCurve[profiles.Length / 4];
		IOnCurve[] array3 = (IOnCurve[])(object)new IOnCurve[profiles.Length / 4];
		IOnCurve[] array4 = (IOnCurve[])(object)new IOnCurve[profiles.Length / 4];
		for (int num = 0; num < profiles.Length / 4; num++)
		{
			array[num] = (IOnCurve)(object)profiles[num * 4].DuplicateCurve();
			array2[num] = (IOnCurve)(object)profiles[num * 4 + 1].DuplicateCurve();
			array3[num] = (IOnCurve)(object)profiles[num * 4 + 2].DuplicateCurve();
			array4[num] = (IOnCurve)(object)profiles[num * 4 + 3].DuplicateCurve();
		}
		if (Math.Abs(param2[0] - railCurve2.Domain().Min()) < 0.001)
		{
			param2[0] = 0.0;
		}
		if (!Sweep2((IOnCurve)(object)railCurve1, (IOnCurve)(object)railCurve2, array, param1, param2, out var breps, railCurve1.IsClosed() && closed, maintainHeight: true))
		{
			return null;
		}
		if (!Sweep2((IOnCurve)(object)railCurve1, (IOnCurve)(object)railCurve2, array2, param1, param2, out var breps2, railCurve1.IsClosed() && closed, maintainHeight: true))
		{
			return null;
		}
		if (!Sweep2((IOnCurve)(object)railCurve1, (IOnCurve)(object)railCurve2, array3, param1, param2, out var breps3, railCurve1.IsClosed() && closed, maintainHeight: true))
		{
			return null;
		}
		if (!Sweep2((IOnCurve)(object)railCurve1, (IOnCurve)(object)railCurve2, array4, param1, param2, out var breps4, railCurve1.IsClosed() && closed, maintainHeight: true))
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
		//IL_0187: Unknown result type (might be due to invalid IL or missing references)
		//IL_018e: Expected O, but got Unknown
		//IL_0436: Unknown result type (might be due to invalid IL or missing references)
		//IL_0440: Expected O, but got Unknown
		//IL_0473: Unknown result type (might be due to invalid IL or missing references)
		//IL_047d: Expected O, but got Unknown
		OnCurve val = railCurve1.DuplicateCurve();
		OnCurve val2 = railCurve2.DuplicateCurve();
		List<OnCurve> list = (from profile in profiles
			where profile != null
			select profile.DuplicateCurve()).ToList();
		MatchCurveDirection(val, val2, 20);
		List<double> list2 = new List<double>();
		List<double> list3 = new List<double>();
		for (int num = 0; num < list.Count; num++)
		{
			double num2 = 0.0;
			double num3 = 0.0;
			bool flag = false;
			if (!list[num].IsClosed())
			{
				num2 = 0.0;
				num3 = 0.0;
				On3dPoint val3 = list[num].PointAtStart();
				On3dPoint val4 = list[num].PointAtEnd();
				val.GetClosestPoint((IOn3dPoint)(object)val3, ref num2);
				val.GetClosestPoint((IOn3dPoint)(object)val4, ref num3);
				On3dPoint val5 = val.PointAt(num2);
				On3dPoint val6 = val.PointAt(num3);
				double num4 = val3.DistanceTo((IOn3dPoint)(object)val5);
				double num5 = val4.DistanceTo((IOn3dPoint)(object)val6);
				if (num4 < 0.01 || num5 < 0.01)
				{
					flag = true;
				}
				if (flipProfiles && num4 > num5)
				{
					list[num].Reverse();
				}
				if (flag)
				{
					val.GetClosestPoint((IOn3dPoint)(object)list[num].PointAtStart(), ref num2);
					val2.GetClosestPoint((IOn3dPoint)(object)list[num].PointAtEnd(), ref num3);
				}
			}
			if (!flag)
			{
				On3dPointArray val7 = new On3dPointArray();
				List<double> obj = new List<double> { 0.0, 0.25, 0.33, 0.5, 0.66, 0.75 };
				double num6 = 0.0;
				foreach (double item in obj)
				{
					list[num].GetNormalizedArcLengthPoint(item, ref num6);
					((ArrayOn3dPoint)val7).Append((IOn3dPoint)(object)list[num].PointAt(num6));
				}
				double num7 = OnUtil.On_DBL_MAX;
				double num8 = OnUtil.On_DBL_MAX;
				for (int num9 = 0; num9 < ((ArrayOn3dPoint)val7).Count(); num9++)
				{
					val.GetClosestPoint((IOn3dPoint)(object)((ArrayOn3dPoint)val7)[num9], ref num6);
					double num10 = val.PointAt(num6).DistanceTo((IOn3dPoint)(object)((ArrayOn3dPoint)val7)[num9]);
					if (num10 < num7)
					{
						num2 = num6;
						num7 = num10;
					}
					val2.GetClosestPoint((IOn3dPoint)(object)((ArrayOn3dPoint)val7)[num9], ref num6);
					num10 = val2.PointAt(num6).DistanceTo((IOn3dPoint)(object)((ArrayOn3dPoint)val7)[num9]);
					if (num10 < num8)
					{
						num3 = num6;
						num8 = num10;
					}
				}
			}
			if (num == 0 && firstAtStart)
			{
				if (val.IsClosed())
				{
					val.ChangeClosedCurveSeam(num2);
					num2 = val.Domain().Min();
				}
				if (val2.IsClosed())
				{
					val2.ChangeClosedCurveSeam(num3);
					num3 = val2.Domain().Min();
				}
			}
			list2.Add(num2);
			list3.Add(num3);
		}
		for (int num11 = 0; num11 < list.Count; num11++)
		{
			for (int num12 = num11 + 1; num12 < list.Count; num12++)
			{
				if (list2[num12] < list2[num11])
				{
					OnCurve value = list[num11];
					double value2 = list2[num11];
					double value3 = list3[num11];
					list[num11] = list[num12];
					list2[num11] = list2[num12];
					list3[num11] = list3[num12];
					list[num12] = value;
					list2[num12] = value2;
					list3[num12] = value3;
				}
			}
		}
		if (val.IsClosed() && !closed && list2.Count > 0)
		{
			val.Trim((IOnInterval)new OnInterval(val.Domain().Min(), list2[list2.Count - 1]));
		}
		if (val2.IsClosed() && !closed && list3.Count > 0)
		{
			val2.Trim((IOnInterval)new OnInterval(val2.Domain().Min(), list3[list3.Count - 1]));
		}
		IOnCurve[] array = (IOnCurve[])(object)new IOnCurve[list.Count];
		for (int num13 = 0; num13 < list.Count; num13++)
		{
			array[num13] = (IOnCurve)(object)list[num13];
		}
		return Sweep2((IOnCurve)(object)val, (IOnCurve)(object)val2, array, list2.ToArray(), list3.ToArray(), out breps, closed, maintainHeight);
	}

	public static bool Sweep2NoProfile(IOnCurve railCurve1, IOnCurve railCurve2, out OnBrep[] breps)
	{
		return Sweep2NoProfile(railCurve1, railCurve2, out breps, closed: false, 1);
	}

	public static bool Sweep2NoProfile(IOnCurve railCurve1, IOnCurve railCurve2, out OnBrep[] breps, bool closed, int profileCount)
	{
		//IL_00b3: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ba: Expected O, but got Unknown
		breps = (OnBrep[])(object)new OnBrep[0];
		if (railCurve1 == null || railCurve2 == null)
		{
			CSUtil.PrintLine("Missing rail");
			return false;
		}
		OnNurbsCurve val = railCurve1.NurbsCurve();
		OnNurbsCurve val2 = railCurve2.NurbsCurve();
		if (!RhUtil.RhinoDoCurveDirectionsMatch((IOnCurve)(object)val, (IOnCurve)(object)val2))
		{
			((OnCurve)val2).Reverse();
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
			double num = ((OnCurve)val).Domain().Min();
			double num2 = ((OnCurve)val2).Domain().Min();
			((OnCurve)val).GetNormalizedArcLengthPoint((double)i / (double)profileCount, ref num);
			((OnCurve)val2).GetNormalizedArcLengthPoint((double)i / (double)profileCount, ref num2);
			list2.Add(num);
			list3.Add(num2);
			OnLineCurve item = new OnLineCurve((IOn3dPoint)(object)((OnCurve)val).PointAt(num), (IOn3dPoint)(object)((OnCurve)val2).PointAt(num2));
			list.Add((IOnCurve)(object)item);
		}
		return Sweep2((IOnCurve)(object)val, (IOnCurve)(object)val2, list.ToArray(), list2.ToArray(), list3.ToArray(), out breps, closed, maintainHeight: false);
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
				num2 += p1[(i + offset1) % num].DistanceTo((IOn3dPoint)(object)p2[(i + offset2) % num]);
			}
		}
		else
		{
			for (int j = 0; j < p1.Count(); j++)
			{
				num2 += p1[(j + offset1) % num].DistanceTo((IOn3dPoint)(object)p2[(num - j - 1 + offset2) % num]);
			}
		}
		return num2;
	}
}
