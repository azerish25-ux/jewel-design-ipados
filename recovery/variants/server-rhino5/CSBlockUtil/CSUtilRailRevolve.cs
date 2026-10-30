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
		//IL_0006: Unknown result type (might be due to invalid IL or missing references)
		//IL_000b: Unknown result type (might be due to invalid IL or missing references)
		double s = new On3dPoint((IOn3dPoint)(object)((IOnCurve)profile).PointAtStart())
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
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0006: Expected O, but got Unknown
		//IL_006f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0076: Expected O, but got Unknown
		//IL_0083: Unknown result type (might be due to invalid IL or missing references)
		//IL_0088: Unknown result type (might be due to invalid IL or missing references)
		//IL_0097: Unknown result type (might be due to invalid IL or missing references)
		//IL_009d: Expected O, but got Unknown
		//IL_0098: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ef: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f6: Expected O, but got Unknown
		//IL_01ad: Unknown result type (might be due to invalid IL or missing references)
		//IL_01b7: Expected O, but got Unknown
		//IL_017b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0185: Expected O, but got Unknown
		OnNurbsSurface val = new OnNurbsSurface();
		if (direction == 0)
		{
			val.Create(3, true, profile.m_order, rail.m_order, profile.m_cv_count, rail.m_cv_count);
		}
		else
		{
			val.Create(3, true, rail.m_order, profile.m_order, rail.m_cv_count, profile.m_cv_count);
		}
		int num = rail.CVCount();
		int num2 = profile.CVCount();
		double z = ((IOnCurve)profile).PointAtEnd().z;
		for (int i = 0; i < num2; i++)
		{
			On3dPoint val2 = new On3dPoint();
			profile.GetCV(i, ref val2);
			On3dPoint val3 = new On3dPoint((IOn3dPoint)(object)val2)
			{
				z = 0.0
			};
			new On3dVector((IOn3dPoint)val3).Unitize();
			double num3 = val3.DistanceTo(OnUtil.On_origin);
			double num4 = (zAdjust ? (1.0 - val2.z / z) : 0.0);
			if (double.IsNaN(num4))
			{
				num4 = 0.0;
			}
			for (int j = 0; j < num; j++)
			{
				On3dPoint val4 = new On3dPoint();
				rail.GetCV(j, ref val4);
				val4.x = val4.x * num3 / s1;
				val4.y = val4.y * num3 / s1;
				val4.z = val2.z + num4 * val4.z;
				double num5 = profile.Weight(i) * rail.Weight(j);
				if (direction == 0)
				{
					val.SetCV(i, j, (IOn4dPoint)new On4dPoint(val4.x * num5, val4.y * num5, val4.z * num5, num5));
				}
				else
				{
					val.SetCV(j, i, (IOn4dPoint)new On4dPoint(val4.x * num5, val4.y * num5, val4.z * num5, num5));
				}
			}
		}
		for (int k = 0; k < profile.KnotCount(); k++)
		{
			val.SetKnot(direction, k, profile.Knot(k));
		}
		for (int l = 0; l < rail.KnotCount(); l++)
		{
			val.SetKnot(1 - direction, l, rail.Knot(l));
		}
		return val;
	}

	public static NurbsSurface RailRevolve(NurbsCurve inprofile, NurbsCurve inrail, double s1, int direction, bool zAdjust = false)
	{
		//IL_0006: Unknown result type (might be due to invalid IL or missing references)
		//IL_0019: Unknown result type (might be due to invalid IL or missing references)
		//IL_0026: Unknown result type (might be due to invalid IL or missing references)
		//IL_002c: Expected O, but got Unknown
		//IL_0097: Unknown result type (might be due to invalid IL or missing references)
		//IL_009e: Expected O, but got Unknown
		//IL_00ab: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b0: Unknown result type (might be due to invalid IL or missing references)
		//IL_00bf: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c5: Expected O, but got Unknown
		//IL_00c0: Unknown result type (might be due to invalid IL or missing references)
		//IL_0118: Unknown result type (might be due to invalid IL or missing references)
		//IL_011f: Expected O, but got Unknown
		//IL_01d6: Unknown result type (might be due to invalid IL or missing references)
		//IL_01e0: Expected O, but got Unknown
		//IL_01a4: Unknown result type (might be due to invalid IL or missing references)
		//IL_01ae: Expected O, but got Unknown
		IOnCurve val = (IOnCurve)Interop.ToOnCurve(inprofile);
		IOnNurbsCurve val2 = null;
		val2 = (IOnNurbsCurve)(object)val.NurbsCurve();
		IOnCurve val3 = (IOnCurve)Interop.ToOnCurve(inrail);
		OnNurbsCurve val4 = null;
		val4 = val3.NurbsCurve();
		OnNurbsSurface val5 = new OnNurbsSurface();
		if (direction == 0)
		{
			val5.Create(3, true, val2.m_order, val4.m_order, val2.m_cv_count, val4.m_cv_count);
		}
		else
		{
			val5.Create(3, true, val4.m_order, val2.m_order, val4.m_cv_count, val2.m_cv_count);
		}
		int num = val4.CVCount();
		int num2 = val2.CVCount();
		double z = ((IOnCurve)val2).PointAtEnd().z;
		for (int i = 0; i < num2; i++)
		{
			On3dPoint val6 = new On3dPoint();
			val2.GetCV(i, ref val6);
			On3dPoint val7 = new On3dPoint((IOn3dPoint)(object)val6)
			{
				z = 0.0
			};
			new On3dVector((IOn3dPoint)val7).Unitize();
			double num3 = val7.DistanceTo(OnUtil.On_origin);
			double num4 = (zAdjust ? (1.0 - val6.z / z) : 0.0);
			if (double.IsNaN(num4))
			{
				num4 = 0.0;
			}
			for (int j = 0; j < num; j++)
			{
				On3dPoint val8 = new On3dPoint();
				val4.GetCV(j, ref val8);
				val8.x = val8.x * num3 / s1;
				val8.y = val8.y * num3 / s1;
				val8.z = val6.z + num4 * val8.z;
				double num5 = val2.Weight(i) * val4.Weight(j);
				if (direction == 0)
				{
					val5.SetCV(i, j, (IOn4dPoint)new On4dPoint(val8.x * num5, val8.y * num5, val8.z * num5, num5));
				}
				else
				{
					val5.SetCV(j, i, (IOn4dPoint)new On4dPoint(val8.x * num5, val8.y * num5, val8.z * num5, num5));
				}
			}
		}
		for (int k = 0; k < val2.KnotCount(); k++)
		{
			val5.SetKnot(direction, k, val2.Knot(k));
		}
		for (int l = 0; l < val4.KnotCount(); l++)
		{
			val5.SetKnot(1 - direction, l, val4.Knot(l));
		}
		return Interop.FromOnSurface(val5).ToNurbsSurface();
	}

	public static OnNurbsSurface RailRevolve(IOnNurbsCurve profile, IOnNurbsCurve rail, double s1, int direction, out OnNurbsCurve baseCurve)
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0006: Expected O, but got Unknown
		//IL_005d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0063: Expected O, but got Unknown
		//IL_0071: Unknown result type (might be due to invalid IL or missing references)
		//IL_0078: Expected O, but got Unknown
		//IL_0084: Unknown result type (might be due to invalid IL or missing references)
		//IL_0089: Unknown result type (might be due to invalid IL or missing references)
		//IL_009a: Expected O, but got Unknown
		//IL_00c4: Unknown result type (might be due to invalid IL or missing references)
		//IL_00cb: Expected O, but got Unknown
		//IL_00d8: Unknown result type (might be due to invalid IL or missing references)
		//IL_00dd: Unknown result type (might be due to invalid IL or missing references)
		//IL_011f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0126: Expected O, but got Unknown
		//IL_0198: Unknown result type (might be due to invalid IL or missing references)
		//IL_019f: Expected O, but got Unknown
		//IL_01c9: Unknown result type (might be due to invalid IL or missing references)
		OnNurbsSurface val = new OnNurbsSurface();
		if (direction == 0)
		{
			val.Create(3, true, profile.m_order, rail.m_order, profile.m_cv_count, rail.m_cv_count);
		}
		else
		{
			val.Create(3, true, rail.m_order, profile.m_order, rail.m_cv_count, profile.m_cv_count);
		}
		baseCurve = new OnNurbsCurve(3, true, rail.m_order, rail.m_cv_count);
		double num = 0.0;
		for (int i = 0; i < profile.CVCount(); i++)
		{
			On3dPoint val2 = new On3dPoint();
			profile.GetCV(i, ref val2);
			On3dPoint val3 = new On3dPoint((IOn3dPoint)(object)val2)
			{
				z = 0.0
			};
			num = Math.Max(num, val3.DistanceTo(OnUtil.On_origin));
		}
		bool flag = false;
		for (int j = 0; j < profile.CVCount(); j++)
		{
			On3dPoint val4 = new On3dPoint();
			profile.GetCV(j, ref val4);
			On3dPoint val5 = new On3dPoint((IOn3dPoint)(object)val4)
			{
				z = 0.0
			};
			bool flag2 = false;
			double num2 = val5.DistanceTo(OnUtil.On_origin);
			if (!flag && Math.Abs(num2 - num) < 0.01)
			{
				flag2 = true;
				flag = true;
			}
			for (int k = 0; k < rail.CVCount(); k++)
			{
				On3dPoint val6 = new On3dPoint();
				rail.GetCV(k, ref val6);
				val6.x = val6.x * num2 / s1;
				val6.y = val6.y * num2 / s1;
				val6.z = val4.z;
				double num3 = profile.Weight(j) * rail.Weight(k);
				On4dPoint val7 = new On4dPoint(val6.x * num3, val6.y * num3, val6.z * num3, num3);
				double num4 = rail.Weight(k);
				new On4dPoint(val6.x * num4, val6.y * num4, val6.z * num4, num4);
				if (direction == 0)
				{
					val.SetCV(j, k, (IOn4dPoint)(object)val7);
				}
				else
				{
					val.SetCV(k, j, (IOn4dPoint)(object)val7);
				}
				if (flag2)
				{
					baseCurve.SetCV(k, (IOn4dPoint)(object)val7);
				}
			}
		}
		for (int l = 0; l < rail.KnotCount(); l++)
		{
			baseCurve.SetKnot(l, rail.Knot(l));
		}
		for (int m = 0; m < profile.KnotCount(); m++)
		{
			val.SetKnot(direction, m, profile.Knot(m));
		}
		for (int n = 0; n < rail.KnotCount(); n++)
		{
			val.SetKnot(1 - direction, n, rail.Knot(n));
		}
		return val;
	}

	public static OnNurbsSurface RailRevolveGen(IOnNurbsCurve iprofile, IOnNurbsCurve rail, int direction, bool zAdjust = false)
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0006: Expected O, but got Unknown
		//IL_0015: Unknown result type (might be due to invalid IL or missing references)
		//IL_001a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0070: Unknown result type (might be due to invalid IL or missing references)
		//IL_0090: Unknown result type (might be due to invalid IL or missing references)
		//IL_009a: Expected O, but got Unknown
		//IL_009a: Expected O, but got Unknown
		//IL_0095: Unknown result type (might be due to invalid IL or missing references)
		//IL_009b: Expected O, but got Unknown
		//IL_009b: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a2: Expected O, but got Unknown
		//IL_01b3: Unknown result type (might be due to invalid IL or missing references)
		//IL_01ba: Expected O, but got Unknown
		//IL_012d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0134: Expected O, but got Unknown
		//IL_013d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0144: Expected O, but got Unknown
		//IL_015c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0166: Expected O, but got Unknown
		//IL_0176: Unknown result type (might be due to invalid IL or missing references)
		//IL_0180: Expected O, but got Unknown
		//IL_0228: Unknown result type (might be due to invalid IL or missing references)
		//IL_022f: Expected O, but got Unknown
		//IL_023c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0241: Unknown result type (might be due to invalid IL or missing references)
		//IL_029c: Unknown result type (might be due to invalid IL or missing references)
		//IL_02a3: Expected O, but got Unknown
		//IL_035c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0366: Expected O, but got Unknown
		//IL_0329: Unknown result type (might be due to invalid IL or missing references)
		//IL_0333: Expected O, but got Unknown
		OnNurbsCurve val = new OnNurbsCurve();
		((IOnCurve)iprofile).GetNurbForm(ref val);
		double num = new On3dPoint((IOn3dPoint)(object)((IOnCurve)rail).PointAtStart())
		{
			z = 0.0
		}.DistanceTo(OnUtil.On_origin);
		double num2 = 0.0;
		((OnCurve)val).GetClosestPoint((IOn3dPoint)(object)((IOnCurve)rail).PointAtStart(), ref num2);
		((OnCurve)val).ChangeClosedCurveSeam(num2);
		OnLineCurve val2 = new OnLineCurve((IOn3dPoint)new On3dPoint(0.0, 0.0, -100.0), (IOn3dPoint)new On3dPoint(0.0, 0.0, 100.0));
		ArrayOnX_EVENT val3 = new ArrayOnX_EVENT();
		((OnCurve)val).IntersectCurve((IOnCurve)(object)val2, ref val3);
		if (val3.Count() > 1)
		{
			double num3 = 1000.0;
			double num4 = -1000.0;
			for (int i = 0; i < val3.Count(); i++)
			{
				if (val3[i][0] < num3)
				{
					num3 = val3[i][0];
				}
				if (val3[i][0] > num4)
				{
					num4 = val3[i][0];
				}
			}
			OnNurbsCurve val4 = new OnNurbsCurve();
			((OnCurve)val).GetNurbForm(ref val4);
			OnNurbsCurve val5 = new OnNurbsCurve();
			((OnCurve)val).GetNurbForm(ref val5);
			((OnCurve)val4).Trim((IOnInterval)new OnInterval(((OnCurve)val).Domain().Min(), num3));
			((OnCurve)val5).Trim((IOnInterval)new OnInterval(num4, ((OnCurve)val).Domain().Max()));
			OnCurve[] obj = new OnCurve[2]
			{
				(OnCurve)val4,
				(OnCurve)val5
			};
			OnCurve[] array = null;
			IOnCurve[] array2 = (IOnCurve[])(object)obj;
			RhUtil.RhinoMergeCurves(array2, ref array);
			if (array.Length == 0)
			{
				return null;
			}
			array[0].GetNurbForm(ref val);
		}
		OnNurbsSurface val6 = new OnNurbsSurface();
		if (direction == 0)
		{
			val6.Create(3, true, val.m_order, rail.m_order, val.m_cv_count, rail.m_cv_count);
		}
		else
		{
			val6.Create(3, true, rail.m_order, val.m_order, rail.m_cv_count, val.m_cv_count);
		}
		int num5 = rail.CVCount();
		int num6 = val.CVCount();
		double z = ((OnCurve)val).PointAtEnd().z;
		for (int j = 0; j < num6; j++)
		{
			On3dPoint val7 = new On3dPoint();
			val.GetCV(j, ref val7);
			double num7 = new On3dPoint((IOn3dPoint)(object)val7)
			{
				z = 0.0
			}.DistanceTo(OnUtil.On_origin);
			double num8 = (zAdjust ? (1.0 - val7.z / z) : 0.0);
			if (double.IsNaN(num8))
			{
				num8 = 0.0;
			}
			for (int k = 0; k < num5; k++)
			{
				On3dPoint val8 = new On3dPoint();
				rail.GetCV(k, ref val8);
				val8.x = val8.x * num7 / num;
				val8.y = val8.y * num7 / num;
				val8.z = val7.z + num8 * val8.z;
				double num9 = val.Weight(j) * rail.Weight(k);
				if (direction == 0)
				{
					val6.SetCV(j, k, (IOn4dPoint)new On4dPoint(val8.x * num9, val8.y * num9, val8.z * num9, num9));
				}
				else
				{
					val6.SetCV(k, j, (IOn4dPoint)new On4dPoint(val8.x * num9, val8.y * num9, val8.z * num9, num9));
				}
			}
		}
		for (int l = 0; l < val.KnotCount(); l++)
		{
			val6.SetKnot(direction, l, val.Knot(l));
		}
		for (int m = 0; m < rail.KnotCount(); m++)
		{
			val6.SetKnot(1 - direction, m, rail.Knot(m));
		}
		return val6;
	}

	public static NurbsSurface RailRevolveGen(NurbsCurve iprofile, NurbsCurve irail, int direction, bool zAdjust = false)
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0006: Expected O, but got Unknown
		//IL_000c: Unknown result type (might be due to invalid IL or missing references)
		//IL_001f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0024: Unknown result type (might be due to invalid IL or missing references)
		//IL_002a: Expected O, but got Unknown
		//IL_0038: Unknown result type (might be due to invalid IL or missing references)
		//IL_003d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0093: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b3: Unknown result type (might be due to invalid IL or missing references)
		//IL_00bd: Expected O, but got Unknown
		//IL_00bd: Expected O, but got Unknown
		//IL_00b8: Unknown result type (might be due to invalid IL or missing references)
		//IL_00bf: Expected O, but got Unknown
		//IL_00bf: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c6: Expected O, but got Unknown
		//IL_01d8: Unknown result type (might be due to invalid IL or missing references)
		//IL_01df: Expected O, but got Unknown
		//IL_0152: Unknown result type (might be due to invalid IL or missing references)
		//IL_0159: Expected O, but got Unknown
		//IL_0162: Unknown result type (might be due to invalid IL or missing references)
		//IL_0169: Expected O, but got Unknown
		//IL_0181: Unknown result type (might be due to invalid IL or missing references)
		//IL_018b: Expected O, but got Unknown
		//IL_019b: Unknown result type (might be due to invalid IL or missing references)
		//IL_01a5: Expected O, but got Unknown
		//IL_024d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0254: Expected O, but got Unknown
		//IL_0261: Unknown result type (might be due to invalid IL or missing references)
		//IL_0266: Unknown result type (might be due to invalid IL or missing references)
		//IL_02c1: Unknown result type (might be due to invalid IL or missing references)
		//IL_02c8: Expected O, but got Unknown
		//IL_0381: Unknown result type (might be due to invalid IL or missing references)
		//IL_038b: Expected O, but got Unknown
		//IL_034e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0358: Expected O, but got Unknown
		OnNurbsCurve val = new OnNurbsCurve();
		((OnCurve)Interop.ToOnCurve(iprofile)).GetNurbForm(ref val);
		OnCurve val2 = (OnCurve)Interop.ToOnCurve(irail);
		OnNurbsCurve val3 = new OnNurbsCurve();
		val2.GetNurbForm(ref val3);
		double num = new On3dPoint((IOn3dPoint)(object)((OnCurve)val3).PointAtStart())
		{
			z = 0.0
		}.DistanceTo(OnUtil.On_origin);
		double num2 = 0.0;
		((OnCurve)val).GetClosestPoint((IOn3dPoint)(object)((OnCurve)val3).PointAtStart(), ref num2);
		((OnCurve)val).ChangeClosedCurveSeam(num2);
		OnLineCurve val4 = new OnLineCurve((IOn3dPoint)new On3dPoint(0.0, 0.0, -100.0), (IOn3dPoint)new On3dPoint(0.0, 0.0, 100.0));
		ArrayOnX_EVENT val5 = new ArrayOnX_EVENT();
		((OnCurve)val).IntersectCurve((IOnCurve)(object)val4, ref val5);
		if (val5.Count() > 1)
		{
			double num3 = 1000.0;
			double num4 = -1000.0;
			for (int i = 0; i < val5.Count(); i++)
			{
				if (val5[i][0] < num3)
				{
					num3 = val5[i][0];
				}
				if (val5[i][0] > num4)
				{
					num4 = val5[i][0];
				}
			}
			OnNurbsCurve val6 = new OnNurbsCurve();
			((OnCurve)val).GetNurbForm(ref val6);
			OnNurbsCurve val7 = new OnNurbsCurve();
			((OnCurve)val).GetNurbForm(ref val7);
			((OnCurve)val6).Trim((IOnInterval)new OnInterval(((OnCurve)val).Domain().Min(), num3));
			((OnCurve)val7).Trim((IOnInterval)new OnInterval(num4, ((OnCurve)val).Domain().Max()));
			OnCurve[] obj = new OnCurve[2]
			{
				(OnCurve)val6,
				(OnCurve)val7
			};
			OnCurve[] array = null;
			IOnCurve[] array2 = (IOnCurve[])(object)obj;
			RhUtil.RhinoMergeCurves(array2, ref array);
			if (array.Length == 0)
			{
				return null;
			}
			array[0].GetNurbForm(ref val);
		}
		OnNurbsSurface val8 = new OnNurbsSurface();
		if (direction == 0)
		{
			val8.Create(3, true, val.m_order, val3.m_order, val.m_cv_count, val3.m_cv_count);
		}
		else
		{
			val8.Create(3, true, val3.m_order, val.m_order, val3.m_cv_count, val.m_cv_count);
		}
		int num5 = val3.CVCount();
		int num6 = val.CVCount();
		double z = ((OnCurve)val).PointAtEnd().z;
		for (int j = 0; j < num6; j++)
		{
			On3dPoint val9 = new On3dPoint();
			val.GetCV(j, ref val9);
			double num7 = new On3dPoint((IOn3dPoint)(object)val9)
			{
				z = 0.0
			}.DistanceTo(OnUtil.On_origin);
			double num8 = (zAdjust ? (1.0 - val9.z / z) : 0.0);
			if (double.IsNaN(num8))
			{
				num8 = 0.0;
			}
			for (int k = 0; k < num5; k++)
			{
				On3dPoint val10 = new On3dPoint();
				val3.GetCV(k, ref val10);
				val10.x = val10.x * num7 / num;
				val10.y = val10.y * num7 / num;
				val10.z = val9.z + num8 * val10.z;
				double num9 = val.Weight(j) * val3.Weight(k);
				if (direction == 0)
				{
					val8.SetCV(j, k, (IOn4dPoint)new On4dPoint(val10.x * num9, val10.y * num9, val10.z * num9, num9));
				}
				else
				{
					val8.SetCV(k, j, (IOn4dPoint)new On4dPoint(val10.x * num9, val10.y * num9, val10.z * num9, num9));
				}
			}
		}
		for (int l = 0; l < val.KnotCount(); l++)
		{
			val8.SetKnot(direction, l, val.Knot(l));
		}
		for (int m = 0; m < val3.KnotCount(); m++)
		{
			val8.SetKnot(1 - direction, m, val3.Knot(m));
		}
		return Interop.FromOnSurface(val8).ToNurbsSurface();
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
		//IL_0006: Unknown result type (might be due to invalid IL or missing references)
		//IL_000b: Unknown result type (might be due to invalid IL or missing references)
		double s = new On3dPoint((IOn3dPoint)(object)((IOnCurve)rail).PointAtStart())
		{
			z = 0.0
		}.DistanceTo(OnUtil.On_origin);
		return RailRevolve(profile, rail, s, 1);
	}
}
