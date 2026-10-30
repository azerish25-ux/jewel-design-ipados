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
		//IL_0014: Unknown result type (might be due to invalid IL or missing references)
		//IL_001a: Expected O, but got Unknown
		//IL_0027: Unknown result type (might be due to invalid IL or missing references)
		//IL_002d: Expected O, but got Unknown
		//IL_0047: Unknown result type (might be due to invalid IL or missing references)
		//IL_004e: Expected O, but got Unknown
		//IL_0069: Unknown result type (might be due to invalid IL or missing references)
		//IL_0070: Expected O, but got Unknown
		//IL_01c7: Unknown result type (might be due to invalid IL or missing references)
		//IL_01d1: Expected O, but got Unknown
		//IL_018f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0199: Expected O, but got Unknown
		if (crv == null)
		{
			return false;
		}
		double num = ((OnCurve)rail).Domain().Min();
		bool flag = false;
		OnPlane val = new OnPlane();
		if (((OnCurve)crv).IsPlanar(val))
		{
			OnPlaneSurface val2 = new OnPlaneSurface((IOnPlane)(object)val);
			On3dPoint val3 = val.PointAt(0.5, 0.5);
			OnXform val4 = new OnXform();
			val4.Scale((IOn3dPoint)(object)val3, 1000.0);
			val2.Transform((IOnXform)(object)val4);
			ArrayOnX_EVENT val5 = new ArrayOnX_EVENT();
			((OnCurve)rail).IntersectSurface((IOnSurface)(object)val2, ref val5);
			if (val5.Count() > 0)
			{
				int num2 = -1;
				double num3 = 1000.0;
				for (int i = 0; i < val5.Count(); i++)
				{
					if (!val5[i].IsOverlapEvent())
					{
						double num4 = val5[i][0].DistanceTo((IOn3dPoint)(object)((OnCurve)crv).PointAtStart());
						if (num4 < num3)
						{
							num2 = i;
							num3 = num4;
						}
					}
				}
				if (num2 >= 0)
				{
					num = val5[num2][0];
					flag = true;
				}
			}
		}
		if (!flag)
		{
			((OnCurve)rail).GetClosestPoint((IOn3dPoint)(object)((OnCurve)crv).PointAtStart(), ref num);
		}
		if (((OnCurve)rail).IsClosed())
		{
			((OnCurve)rail).ChangeClosedCurveSeam(num);
			if (lengthParameter < 0.0)
			{
				((OnCurve)rail).Reverse();
				lengthParameter = 0.0 - lengthParameter;
				flip = true;
			}
		}
		else
		{
			double num5 = 1.0;
			((OnCurve)rail).GetLength(ref num5);
			double num6 = 1.0;
			if (lengthParameter > 0.0)
			{
				if (((OnCurve)rail).Domain().Max() != num)
				{
					((OnCurve)rail).Trim((IOnInterval)new OnInterval(num, ((OnCurve)rail).Domain().Max()));
				}
				else
				{
					lengthParameter = 1.0;
				}
			}
			else if (((OnCurve)rail).Domain().Min() != num)
			{
				((OnCurve)rail).Trim((IOnInterval)new OnInterval(((OnCurve)rail).Domain().Min(), num));
				lengthParameter = 0.0 - lengthParameter;
				((OnCurve)rail).Reverse();
				flip = true;
			}
			else
			{
				lengthParameter = 0.0;
			}
			((OnCurve)rail).GetLength(ref num6);
			lengthParameter /= num6 / num5;
		}
		return true;
	}

	public static bool AdjustRailFromCurve(NurbsCurve crv, ref NurbsCurve rail, ref double lengthParameter, ref bool flip)
	{
		//IL_0007: Unknown result type (might be due to invalid IL or missing references)
		//IL_000c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0038: Unknown result type (might be due to invalid IL or missing references)
		//IL_0046: Unknown result type (might be due to invalid IL or missing references)
		//IL_004b: Unknown result type (might be due to invalid IL or missing references)
		//IL_004d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0060: Unknown result type (might be due to invalid IL or missing references)
		//IL_0077: Unknown result type (might be due to invalid IL or missing references)
		//IL_007c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0083: Expected O, but got Unknown
		//IL_0085: Unknown result type (might be due to invalid IL or missing references)
		//IL_0129: Unknown result type (might be due to invalid IL or missing references)
		//IL_01c9: Unknown result type (might be due to invalid IL or missing references)
		//IL_01ce: Unknown result type (might be due to invalid IL or missing references)
		//IL_0185: Unknown result type (might be due to invalid IL or missing references)
		//IL_018a: Unknown result type (might be due to invalid IL or missing references)
		//IL_01de: Unknown result type (might be due to invalid IL or missing references)
		//IL_01e3: Unknown result type (might be due to invalid IL or missing references)
		//IL_01ec: Unknown result type (might be due to invalid IL or missing references)
		//IL_019b: Unknown result type (might be due to invalid IL or missing references)
		//IL_01a0: Unknown result type (might be due to invalid IL or missing references)
		//IL_01a8: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d8: Unknown result type (might be due to invalid IL or missing references)
		//IL_00dd: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e2: Unknown result type (might be due to invalid IL or missing references)
		if (crv == null)
		{
			return false;
		}
		Interval domain = ((Curve)rail).Domain;
		double num = ((Interval)(ref domain)).Min;
		bool flag = false;
		Plane val = default(Plane);
		if (((Curve)crv).TryGetPlane(ref val))
		{
			Transform val2 = Transform.Scale(((Plane)(ref val)).PointAt(0.5, 0.5), 1000.0);
			PlaneSurface val3 = new PlaneSurface(val, new Interval(0.0, 1.0), new Interval(0.0, 1.0));
			((GeometryBase)val3).Transform(val2);
			CurveIntersections val4 = Intersection.CurveSurface((Curve)(object)rail, (Surface)(object)val3, CSUtil.Tolerance, CSUtil.Tolerance);
			if (val4.Count > 0)
			{
				int num2 = -1;
				double num3 = 1000.0;
				for (int i = 0; i < val4.Count; i++)
				{
					if (!val4[i].IsOverlap)
					{
						Point3d pointA = val4[i].PointA;
						double num4 = ((Point3d)(ref pointA)).DistanceTo(((Curve)crv).PointAtStart);
						if (num4 < num3)
						{
							num2 = i;
							num3 = num4;
						}
					}
				}
				if (num2 >= 0)
				{
					num = val4[num2].ParameterA;
					flag = true;
				}
			}
		}
		if (!flag)
		{
			((Curve)rail).ClosestPoint(((Curve)crv).PointAtStart, ref num);
		}
		if (((Curve)rail).IsClosed)
		{
			((Curve)rail).ChangeClosedCurveSeam(num);
			if (lengthParameter < 0.0)
			{
				((Curve)rail).Reverse();
				lengthParameter = 0.0 - lengthParameter;
				flip = true;
			}
		}
		else
		{
			double length = ((Curve)rail).GetLength();
			if (lengthParameter > 0.0)
			{
				domain = ((Curve)rail).Domain;
				if (((Interval)(ref domain)).Max != num)
				{
					NurbsCurve obj = rail;
					double num5 = num;
					domain = ((Curve)rail).Domain;
					rail = ((Curve)obj).Trim(new Interval(num5, ((Interval)(ref domain)).Max)).ToNurbsCurve();
				}
				else
				{
					lengthParameter = 1.0;
				}
			}
			else
			{
				domain = ((Curve)rail).Domain;
				if (((Interval)(ref domain)).Min != num)
				{
					NurbsCurve obj2 = rail;
					domain = ((Curve)rail).Domain;
					rail = ((Curve)obj2).Trim(new Interval(((Interval)(ref domain)).Min, num)).ToNurbsCurve();
					lengthParameter = 0.0 - lengthParameter;
					((Curve)rail).Reverse();
					flip = true;
				}
				else
				{
					lengthParameter = 0.0;
				}
			}
			double length2 = ((Curve)rail).GetLength();
			lengthParameter /= length2 / length;
		}
		return true;
	}

	public static bool BuildProngDomeProfile(double diameter, double filletPer, double taperPer, out OnNurbsCurve profile)
	{
		//IL_003f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0045: Expected O, but got Unknown
		//IL_0073: Unknown result type (might be due to invalid IL or missing references)
		//IL_007d: Expected O, but got Unknown
		//IL_00a9: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b3: Expected O, but got Unknown
		//IL_00cd: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d7: Expected O, but got Unknown
		//IL_00fb: Unknown result type (might be due to invalid IL or missing references)
		//IL_0105: Expected O, but got Unknown
		//IL_0125: Unknown result type (might be due to invalid IL or missing references)
		//IL_012f: Expected O, but got Unknown
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
		if (!profile.Create(3, true, 3, 5))
		{
			return false;
		}
		profile.SetCV(0, (IOn4dPoint)new On4dPoint(num, 0.0, 0.0, 1.0));
		double num3 = 0.7071067811;
		double num4 = num * (1.0 - taperPer);
		profile.SetCV(1, (IOn4dPoint)new On4dPoint(num4 * num3, 0.0, num2 * num3, num3));
		profile.SetCV(2, (IOn4dPoint)new On4dPoint(num - num2, 0.0, num2, 1.0));
		profile.SetCV(3, (IOn4dPoint)new On4dPoint((num - num2) / 2.0, 0.0, num2, 1.0));
		profile.SetCV(4, (IOn4dPoint)new On4dPoint(0.0, 0.0, num2, 1.0));
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
		//IL_0063: Unknown result type (might be due to invalid IL or missing references)
		//IL_0069: Expected O, but got Unknown
		//IL_00a2: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ac: Expected O, but got Unknown
		//IL_00d7: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e1: Expected O, but got Unknown
		//IL_0102: Unknown result type (might be due to invalid IL or missing references)
		//IL_010c: Expected O, but got Unknown
		//IL_013b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0145: Expected O, but got Unknown
		//IL_015e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0168: Expected O, but got Unknown
		//IL_019b: Unknown result type (might be due to invalid IL or missing references)
		//IL_01a5: Expected O, but got Unknown
		//IL_01c0: Unknown result type (might be due to invalid IL or missing references)
		//IL_01ca: Expected O, but got Unknown
		//IL_01ef: Unknown result type (might be due to invalid IL or missing references)
		//IL_01f9: Expected O, but got Unknown
		//IL_021a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0224: Expected O, but got Unknown
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
		if (!profile.Create(3, true, 3, 9))
		{
			return false;
		}
		profile.SetCV(0, (IOn4dPoint)new On4dPoint(0.0, 0.0, 0.0, 1.0));
		profile.SetCV(1, (IOn4dPoint)new On4dPoint(num2 / 2.0, 0.0, 0.0, 1.0));
		profile.SetCV(2, (IOn4dPoint)new On4dPoint(num2, 0.0, 0.0, 1.0));
		profile.SetCV(3, (IOn4dPoint)new On4dPoint((num2 + num) / 2.0, 0.0, num4 / 2.0, 1.0));
		profile.SetCV(4, (IOn4dPoint)new On4dPoint(num, 0.0, num4, 1.0));
		double num5 = 0.7071067811;
		double num6 = num2 + (num - num2) / num4 * height;
		double num7 = height;
		profile.SetCV(5, (IOn4dPoint)new On4dPoint(num6 * num5, 0.0, num7 * num5, num5));
		profile.SetCV(6, (IOn4dPoint)new On4dPoint(num - num3, 0.0, height, 1.0));
		profile.SetCV(7, (IOn4dPoint)new On4dPoint((num - num3) / 2.0, 0.0, height, 1.0));
		profile.SetCV(8, (IOn4dPoint)new On4dPoint(0.0, 0.0, height, 1.0));
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
		//IL_0066: Unknown result type (might be due to invalid IL or missing references)
		//IL_006c: Expected O, but got Unknown
		//IL_00be: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c8: Expected O, but got Unknown
		//IL_00f5: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ff: Expected O, but got Unknown
		//IL_0122: Unknown result type (might be due to invalid IL or missing references)
		//IL_012c: Expected O, but got Unknown
		//IL_0149: Unknown result type (might be due to invalid IL or missing references)
		//IL_0153: Expected O, but got Unknown
		//IL_0170: Unknown result type (might be due to invalid IL or missing references)
		//IL_017a: Expected O, but got Unknown
		//IL_01ac: Unknown result type (might be due to invalid IL or missing references)
		//IL_01b6: Expected O, but got Unknown
		//IL_01cf: Unknown result type (might be due to invalid IL or missing references)
		//IL_01d9: Expected O, but got Unknown
		//IL_01f3: Unknown result type (might be due to invalid IL or missing references)
		//IL_01fd: Expected O, but got Unknown
		//IL_0218: Unknown result type (might be due to invalid IL or missing references)
		//IL_0222: Expected O, but got Unknown
		//IL_0248: Unknown result type (might be due to invalid IL or missing references)
		//IL_0252: Expected O, but got Unknown
		//IL_0274: Unknown result type (might be due to invalid IL or missing references)
		//IL_027e: Expected O, but got Unknown
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
		if (!profile.Create(3, true, 3, 11))
		{
			return false;
		}
		double num6 = 0.7071067811;
		double num7 = num2 + (num - num2) / num4 * height;
		double num8 = height;
		profile.SetCV(0, (IOn4dPoint)new On4dPoint(0.0, 0.0, 0.0, 1.0));
		profile.SetCV(1, (IOn4dPoint)new On4dPoint((num2 - num3) / 2.0, 0.0, 0.0, 1.0));
		profile.SetCV(2, (IOn4dPoint)new On4dPoint(num2 - num3, 0.0, 0.0, 1.0));
		profile.SetCV(3, (IOn4dPoint)new On4dPoint(num2 * num6, 0.0, 0.0, num6));
		profile.SetCV(4, (IOn4dPoint)new On4dPoint(num2, 0.0, num5 * bottomPercent, 1.0));
		profile.SetCV(5, (IOn4dPoint)new On4dPoint((num2 + num) / 2.0, 0.0, (num5 + num4) / 2.0, 1.0));
		profile.SetCV(6, (IOn4dPoint)new On4dPoint(num, 0.0, num4, 1.0));
		profile.SetCV(7, (IOn4dPoint)new On4dPoint(num7 * num6, 0.0, num8 * num6, num6));
		profile.SetCV(8, (IOn4dPoint)new On4dPoint(num - num3, 0.0, height, 1.0));
		profile.SetCV(9, (IOn4dPoint)new On4dPoint((num - num3) / 2.0, 0.0, height, 1.0));
		profile.SetCV(10, (IOn4dPoint)new On4dPoint(0.0, 0.0, height, 1.0));
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
		//IL_0012: Unknown result type (might be due to invalid IL or missing references)
		//IL_0018: Expected O, but got Unknown
		//IL_0028: Unknown result type (might be due to invalid IL or missing references)
		//IL_0038: Expected O, but got Unknown
		//IL_007d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0084: Expected O, but got Unknown
		//IL_00a0: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a7: Expected O, but got Unknown
		//IL_00c3: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ec: Unknown result type (might be due to invalid IL or missing references)
		//IL_0304: Unknown result type (might be due to invalid IL or missing references)
		//IL_030f: Expected O, but got Unknown
		//IL_030f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0325: Expected O, but got Unknown
		//IL_0320: Unknown result type (might be due to invalid IL or missing references)
		//IL_0326: Expected O, but got Unknown
		OnPlane val = new OnPlane(c1.Center(), c2.Center(), c3.Center());
		double num = 1.0;
		if (new On3dVector(c1.Normal()) * val.Normal() < 0.0)
		{
			num = -1.0;
		}
		double num2 = 0.0;
		double num3 = 0.0;
		val.ClosestPointTo(c1.Center(), ref num2, ref num3);
		On3dPoint val2 = new On3dPoint(num2, num3, 0.0);
		val.ClosestPointTo(c2.Center(), ref num2, ref num3);
		On3dPoint val3 = new On3dPoint(num2, num3, 0.0);
		val.ClosestPointTo(c3.Center(), ref num2, ref num3);
		On3dPoint val4 = new On3dPoint(num2, num3, 0.0);
		double x = val2.x;
		double y = val2.y;
		double x2 = val3.x;
		double y2 = val3.y;
		double x3 = val4.x;
		double y3 = val4.y;
		double radius = c1.radius;
		double radius2 = c2.radius;
		double radius3 = c3.radius;
		double num4 = 2.0 * (x - x2);
		double num5 = 2.0 * (y - y2);
		double num6 = 2.0 * (radius - radius2);
		double num7 = x * x + y * y - radius * radius - x2 * x2 - y2 * y2 + radius2 * radius2;
		double num8 = 2.0 * (x - x3);
		double num9 = 2.0 * (y - y3);
		double num10 = 2.0 * (radius - radius3);
		double num11 = x * x + y * y - radius * radius - x3 * x3 - y3 * y3 + radius3 * radius3;
		double num12 = num4 * num9 - num5 * num8;
		double num13 = (num9 * num7 - num5 * num11) / num12 - x;
		double num14 = ((0.0 - num9) * num6 + num5 * num10) / num12;
		double num15 = ((0.0 - num8) * num7 + num4 * num11) / num12 - y;
		double num16 = (num8 * num6 - num4 * num10) / num12;
		double num17 = num13 * num13 + num15 * num15 - radius * radius;
		double num18 = 2.0 * num13 * num14 + 2.0 * num15 * num16 - 2.0 * radius;
		double num19 = num14 * num14 + num16 * num16 - 1.0;
		double num20 = (0.0 - num18 - Math.Sqrt(num18 * num18 - 4.0 * (num19 * num17))) / (2.0 * num19);
		double num21 = (num9 * num7 - num5 * num11 - num9 * num6 * num20 + num5 * num10 * num20) / (num4 * num9 - num5 * num8);
		double num22 = ((0.0 - num8) * num7 + num4 * num11 + num8 * num6 * num20 - num4 * num10 * num20) / (num4 * num9 - num8 * num5);
		return new OnCircle((IOnPlane)new OnPlane((IOn3dPoint)(object)val.PointAt(num21, num22), (IOn3dVector)(object)(new On3dVector(val.Normal()) * num)), (IOn3dPoint)(object)val.PointAt(num21, num22), num20);
	}

	public static OnNurbsCurve ChannelAdjustEndProfile(OnCurve curve, double amount)
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0006: Expected O, but got Unknown
		//IL_0016: Unknown result type (might be due to invalid IL or missing references)
		//IL_001c: Expected O, but got Unknown
		//IL_0025: Unknown result type (might be due to invalid IL or missing references)
		//IL_002b: Expected O, but got Unknown
		OnNurbsCurve val = new OnNurbsCurve();
		curve.GetNurbForm(ref val);
		val.MakeDeformable();
		OnBoundingBox val2 = new OnBoundingBox();
		((OnGeometry)val).GetTightBoundingBox(ref val2);
		On3dPoint val3 = new On3dPoint();
		if (val2.Center().x < 0.0)
		{
			for (int i = 1; i < val.CVCount() - 1; i++)
			{
				val.GetCV(i, ref val3);
				double num = 1.0 - (val3.x - val2.Min().x) / (val2.Max().x - val2.Min().x);
				On3dPoint obj = val3;
				obj.y += (val2.Max().y - val3.y) * num * amount;
				val.SetCV(i, (IOn3dPoint)(object)val3);
			}
		}
		else
		{
			for (int j = 1; j < val.CVCount() - 1; j++)
			{
				val.GetCV(j, ref val3);
				double num2 = (val3.x - val2.Min().x) / (val2.Max().x - val2.Min().x);
				On3dPoint obj2 = val3;
				obj2.y += (val2.Max().y - val3.y) * num2 * amount;
				val.SetCV(j, (IOn3dPoint)(object)val3);
			}
		}
		val.RemoveShortSegments(0.001, true);
		return val;
	}

	public static OnNurbsCurve CreateDome(double domeAmount, double radius = 0.5)
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0006: Expected O, but got Unknown
		//IL_006f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0079: Expected O, but got Unknown
		//IL_00a4: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ae: Expected O, but got Unknown
		//IL_00cf: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d9: Expected O, but got Unknown
		//IL_00fa: Unknown result type (might be due to invalid IL or missing references)
		//IL_0104: Expected O, but got Unknown
		//IL_0123: Unknown result type (might be due to invalid IL or missing references)
		//IL_012d: Expected O, but got Unknown
		//IL_002a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0039: Expected O, but got Unknown
		//IL_0034: Unknown result type (might be due to invalid IL or missing references)
		OnNurbsCurve val = new OnNurbsCurve();
		if (Math.Abs(domeAmount) < 0.005)
		{
			((OnCurve)new OnLineCurve((IOn3dPoint)new On3dPoint(radius, 0.0, 0.0), OnUtil.On_origin)).GetNurbForm(ref val);
		}
		else
		{
			val.Create(3, true, 3, 5);
			val.SetCV(0, (IOn4dPoint)new On4dPoint(radius, 0.0, 0.0, 1.0));
			val.SetCV(1, (IOn4dPoint)new On4dPoint(radius * 0.70710678, 0.0, domeAmount * 0.70710678, 0.70710678));
			val.SetCV(2, (IOn4dPoint)new On4dPoint(radius / 2.0, 0.0, domeAmount, 1.0));
			val.SetCV(3, (IOn4dPoint)new On4dPoint(radius / 3.0, 0.0, domeAmount, 1.0));
			val.SetCV(4, (IOn4dPoint)new On4dPoint(0.0, 0.0, domeAmount, 1.0));
			val.SetKnot(0, 0.0);
			val.SetKnot(1, 0.0);
			val.SetKnot(2, 1.0);
			val.SetKnot(3, 1.0);
			val.SetKnot(4, 2.0);
			val.SetKnot(5, 2.0);
		}
		return val;
	}

	public static NurbsCurve CreateDomeRC(double domeAmount, double radius = 0.5)
	{
		//IL_003d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0042: Unknown result type (might be due to invalid IL or missing references)
		//IL_0065: Unknown result type (might be due to invalid IL or missing references)
		//IL_0070: Unknown result type (might be due to invalid IL or missing references)
		//IL_008b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0096: Unknown result type (might be due to invalid IL or missing references)
		//IL_00bb: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c6: Unknown result type (might be due to invalid IL or missing references)
		//IL_00eb: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f6: Unknown result type (might be due to invalid IL or missing references)
		//IL_0119: Unknown result type (might be due to invalid IL or missing references)
		//IL_0124: Unknown result type (might be due to invalid IL or missing references)
		//IL_0139: Unknown result type (might be due to invalid IL or missing references)
		//IL_014e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0163: Unknown result type (might be due to invalid IL or missing references)
		//IL_0178: Unknown result type (might be due to invalid IL or missing references)
		//IL_018d: Unknown result type (might be due to invalid IL or missing references)
		//IL_01a3: Expected O, but got Unknown
		//IL_0024: Unknown result type (might be due to invalid IL or missing references)
		//IL_0029: Unknown result type (might be due to invalid IL or missing references)
		//IL_002e: Unknown result type (might be due to invalid IL or missing references)
		if (Math.Abs(domeAmount) < 0.005)
		{
			return ((Curve)new LineCurve(new Point3d(radius, 0.0, 0.0), Point3d.Origin)).ToNurbsCurve();
		}
		NurbsCurve val = new NurbsCurve(3, true, 3, 5);
		val.Points.SetPoint(0, new Point4d(radius, 0.0, 0.0, 1.0));
		val.Points.SetPoint(1, new Point4d(radius, 0.0, domeAmount, 0.70710678));
		val.Points.SetPoint(2, new Point4d(radius / 2.0, 0.0, domeAmount, 1.0));
		val.Points.SetPoint(3, new Point4d(radius / 3.0, 0.0, domeAmount, 1.0));
		val.Points.SetPoint(4, new Point4d(0.0, 0.0, domeAmount, 1.0));
		val.Knots[0] = 0.0;
		val.Knots[1] = 0.0;
		val.Knots[2] = 1.0;
		val.Knots[3] = 1.0;
		val.Knots[4] = 2.0;
		val.Knots[5] = 2.0;
		return val;
	}

	[Obsolete]
	public static bool CreateMeshProng(IOnNurbsCurve rail, double diameter, double height, double filletPer, double taperPer, out OnMesh meshSurface)
	{
		//IL_00b8: Unknown result type (might be due to invalid IL or missing references)
		//IL_00bf: Expected O, but got Unknown
		//IL_00d7: Unknown result type (might be due to invalid IL or missing references)
		//IL_00de: Expected O, but got Unknown
		//IL_00e0: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e5: Unknown result type (might be due to invalid IL or missing references)
		//IL_010d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0114: Expected O, but got Unknown
		//IL_030e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0315: Expected O, but got Unknown
		//IL_032d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0337: Expected O, but got Unknown
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
		OnMesh val = new OnMesh(num4, num3, false, false);
		int num5 = 0;
		for (int i = 1; i < num - 1; i++)
		{
			On3dPoint val2 = new On3dPoint((IOn3dPoint)(object)((ArrayOn3dPoint)polylineCurve.m_pline)[i]);
			double num6 = new On3dVector((IOn3dPoint)(object)val2)
			{
				z = 0.0
			}.Length();
			for (int j = 0; j < num2; j++)
			{
				On3dPoint val3 = new On3dPoint((IOn3dPoint)(object)((ArrayOn3dPoint)polylineCurve2.m_pline)[j]);
				val3.x *= num6;
				val3.y *= num6;
				val3.z = val2.z;
				val.SetVertex(num5++, (IOn3dPoint)(object)val3);
			}
		}
		int num7 = num5;
		val.SetVertex(num5++, (IOn3dPoint)(object)((ArrayOn3dPoint)polylineCurve.m_pline)[0]);
		int num8 = num5;
		val.SetVertex(num5++, (IOn3dPoint)(object)((ArrayOn3dPoint)polylineCurve.m_pline)[num - 1]);
		int num9 = 0;
		for (int k = 0; k < num2; k++)
		{
			val.SetTriangle(num9++, num7, k, (k + 1) % num2);
		}
		for (int l = 0; l < num - 3; l++)
		{
			int num10 = num2 * l;
			int num11 = num2 * (l + 1);
			for (int m = 0; m < num2; m++)
			{
				val.SetQuad(num9++, num10 + m, num11 + m, num11 + (m + 1) % num2, num10 + (m + 1) % num2);
			}
		}
		int num12 = num2 * (num - 3);
		for (int n = 0; n < num2; n++)
		{
			val.SetTriangle(num9++, num12 + n, num8, num12 + (n + 1) % num2);
		}
		if (num5 != num3)
		{
			RhinoApp.Write("Mesh Vertex Count incorrect. Expected " + num3 + " made " + num5 + "\n");
		}
		if (num9 != num4)
		{
			RhinoApp.Write("Mesh face Count incorrect. Expected " + num4 + " made " + num9 + "\n");
		}
		val.ComputeVertexNormals();
		val.Compact();
		OnTextureMapping val4 = new OnTextureMapping();
		val4.EvaluateSphereMapping((IOn3dPoint)(object)((OnGeometry)val).BoundingBox().Center(), OnUtil.On_xaxis, new On3dPoint(OnUtil.On_origin));
		val.SetTextureCoordinates((IOnTextureMapping)(object)val4);
		if (!((OnObject)val).IsValid())
		{
			return false;
		}
		meshSurface = val;
		return true;
	}

	public static bool CreateMeshProng(IOnNurbsCurve rail, double diameter, double height, double filletPer, double taperPer, out OnMesh meshSurface, out OnPolyline outRailLine)
	{
		//IL_001a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0020: Expected O, but got Unknown
		//IL_00c0: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c7: Expected O, but got Unknown
		//IL_00df: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e6: Expected O, but got Unknown
		//IL_00e8: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ed: Unknown result type (might be due to invalid IL or missing references)
		//IL_0144: Unknown result type (might be due to invalid IL or missing references)
		//IL_0150: Expected O, but got Unknown
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
		int num3 = (num - 2) * num2 + 2;
		OnMesh val = new OnMesh(num2 * (num - 1), num3, false, false);
		int num4 = 0;
		for (int i = 1; i < num - 1; i++)
		{
			On3dPoint val2 = new On3dPoint((IOn3dPoint)(object)((ArrayOn3dPoint)polylineCurve.m_pline)[i]);
			double num5 = new On3dVector((IOn3dPoint)(object)val2)
			{
				z = 0.0
			}.Length();
			bool flag = false;
			if (((ArrayOn3dPoint)outRailLine).Count() < 1 && Math.Abs(num5 - diameter / 2.0) < 0.01)
			{
				flag = true;
			}
			for (int j = 0; j < num2; j++)
			{
				On3dPoint val3 = new On3dPoint((IOn3dPoint)(object)((ArrayOn3dPoint)polylineCurve2.m_pline)[j]) * num5;
				val3.z = val2.z;
				val.SetVertex(num4++, (IOn3dPoint)(object)val3);
				if (flag)
				{
					((ArrayOn3dPoint)outRailLine).Append((IOn3dPoint)(object)val3);
				}
			}
			if (flag && ((OnCurve)polylineCurve2).IsClosed())
			{
				((ArrayOn3dPoint)outRailLine).Append((IOn3dPoint)(object)((ArrayOn3dPoint)outRailLine)[0]);
			}
		}
		int num6 = num4;
		val.SetVertex(num4++, (IOn3dPoint)(object)((ArrayOn3dPoint)polylineCurve.m_pline)[0]);
		int num7 = num4;
		val.SetVertex(num4++, (IOn3dPoint)(object)((ArrayOn3dPoint)polylineCurve.m_pline)[num - 1]);
		int num8 = 0;
		for (int k = 0; k < num2; k++)
		{
			val.SetTriangle(num8++, num6, k, (k + 1) % num2);
		}
		for (int l = 0; l < num - 3; l++)
		{
			int num9 = num2 * l;
			int num10 = num2 * (l + 1);
			for (int m = 0; m < num2; m++)
			{
				val.SetQuad(num8++, num9 + m, num10 + m, num10 + (m + 1) % num2, num9 + (m + 1) % num2);
			}
		}
		int num11 = num2 * (num - 3);
		for (int n = 0; n < num2; n++)
		{
			val.SetTriangle(num8++, num11 + n, num7, num11 + (n + 1) % num2);
		}
		val.ComputeVertexNormals();
		val.Compact();
		meshSurface = val;
		return true;
	}

	public static bool CreateMeshProngNew(IOnNurbsCurve rail, double diameter, double height, double filletPer, double taperPer, out OnMesh meshSurface, out OnPolyline outRailLine, double bottomPercent = 0.0)
	{
		//IL_001c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0022: Expected O, but got Unknown
		//IL_00c2: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c9: Expected O, but got Unknown
		//IL_00e1: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e8: Expected O, but got Unknown
		//IL_00ea: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ef: Unknown result type (might be due to invalid IL or missing references)
		//IL_0146: Unknown result type (might be due to invalid IL or missing references)
		//IL_0152: Expected O, but got Unknown
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
		int num3 = (num - 2) * num2 + 2;
		OnMesh val = new OnMesh(num2 * (num - 1), num3, false, false);
		int num4 = 0;
		for (int i = 1; i < num - 1; i++)
		{
			On3dPoint val2 = new On3dPoint((IOn3dPoint)(object)((ArrayOn3dPoint)polylineCurve.m_pline)[i]);
			double num5 = new On3dVector((IOn3dPoint)(object)val2)
			{
				z = 0.0
			}.Length();
			bool flag = false;
			if (((ArrayOn3dPoint)outRailLine).Count() < 1 && Math.Abs(num5 - diameter / 2.0) < 0.01)
			{
				flag = true;
			}
			for (int j = 0; j < num2; j++)
			{
				On3dPoint val3 = new On3dPoint((IOn3dPoint)(object)((ArrayOn3dPoint)polylineCurve2.m_pline)[j]) * num5;
				val3.z = val2.z;
				val.SetVertex(num4++, (IOn3dPoint)(object)val3);
				if (flag)
				{
					((ArrayOn3dPoint)outRailLine).Append((IOn3dPoint)(object)val3);
				}
			}
			if (flag && ((OnCurve)polylineCurve2).IsClosed())
			{
				((ArrayOn3dPoint)outRailLine).Append((IOn3dPoint)(object)((ArrayOn3dPoint)outRailLine)[0]);
			}
		}
		int num6 = num4;
		val.SetVertex(num4++, (IOn3dPoint)(object)((ArrayOn3dPoint)polylineCurve.m_pline)[0]);
		int num7 = num4;
		val.SetVertex(num4++, (IOn3dPoint)(object)((ArrayOn3dPoint)polylineCurve.m_pline)[num - 1]);
		int num8 = 0;
		for (int k = 0; k < num2; k++)
		{
			val.SetTriangle(num8++, num6, k, (k + 1) % num2);
		}
		for (int l = 0; l < num - 3; l++)
		{
			int num9 = num2 * l;
			int num10 = num2 * (l + 1);
			for (int m = 0; m < num2; m++)
			{
				val.SetQuad(num8++, num9 + m, num10 + m, num10 + (m + 1) % num2, num9 + (m + 1) % num2);
			}
		}
		int num11 = num2 * (num - 3);
		for (int n = 0; n < num2; n++)
		{
			val.SetTriangle(num8++, num11 + n, num7, num11 + (n + 1) % num2);
		}
		val.ComputeVertexNormals();
		val.Compact();
		meshSurface = val;
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
		nurbsSurface = CSUtilRailRevolve.RailRevolve((IOnNurbsCurve)(object)profile, (IOnNurbsCurve)(object)railActual, 1.0, 1);
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
		nurbsSurface = CSUtilRailRevolve.RailRevolve((IOnNurbsCurve)(object)profile, (IOnNurbsCurve)(object)railActual, 1.0, 1, out outputRail);
		return true;
	}

	[Obsolete]
	public static bool CreateProng_Old_V1(double diameter, double height, double filletPer, double taperPer, ref OnNurbsSurface nurbsSurface)
	{
		//IL_0005: Unknown result type (might be due to invalid IL or missing references)
		//IL_0015: Expected O, but got Unknown
		return CreateProng_Old_V1((IOn3dVector)new On3dVector(OnUtil.On_zaxis), diameter, height, filletPer, taperPer, ref nurbsSurface);
	}

	[Obsolete]
	public static bool CreateProng_Old_V1(IOn3dVector direction, double diameter, double height, double filletPer, double taperPer, ref OnNurbsSurface nurbsSurface)
	{
		//IL_0180: Unknown result type (might be due to invalid IL or missing references)
		//IL_0186: Expected O, but got Unknown
		//IL_05b0: Unknown result type (might be due to invalid IL or missing references)
		//IL_05b7: Expected O, but got Unknown
		//IL_05bd: Unknown result type (might be due to invalid IL or missing references)
		//IL_05c4: Expected O, but got Unknown
		//IL_05c4: Unknown result type (might be due to invalid IL or missing references)
		//IL_05cb: Expected O, but got Unknown
		//IL_01fe: Unknown result type (might be due to invalid IL or missing references)
		//IL_0205: Expected O, but got Unknown
		//IL_023e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0245: Expected O, but got Unknown
		//IL_027c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0283: Expected O, but got Unknown
		//IL_02bb: Unknown result type (might be due to invalid IL or missing references)
		//IL_02c2: Expected O, but got Unknown
		//IL_02f8: Unknown result type (might be due to invalid IL or missing references)
		//IL_02ff: Expected O, but got Unknown
		//IL_0336: Unknown result type (might be due to invalid IL or missing references)
		//IL_033d: Expected O, but got Unknown
		//IL_0373: Unknown result type (might be due to invalid IL or missing references)
		//IL_037a: Expected O, but got Unknown
		//IL_03b2: Unknown result type (might be due to invalid IL or missing references)
		//IL_03b9: Expected O, but got Unknown
		//IL_03f0: Unknown result type (might be due to invalid IL or missing references)
		//IL_03f7: Expected O, but got Unknown
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
		if (!nurbsSurface.Create(3, true, 3, 3, 9, 9))
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
			On4dPoint val = new On4dPoint((0.0 - array[i, 0]) * num5, 0.0 * num5, array[i, 1] * num5, num5);
			nurbsSurface.SetCV(0, i, (IOn4dPoint)(object)val);
			val = new On4dPoint((0.0 - array[i, 0]) * num6, (0.0 - array[i, 0]) * num6, array[i, 1] * num6, num6);
			nurbsSurface.SetCV(1, i, (IOn4dPoint)(object)val);
			val = new On4dPoint(0.0 * num5, (0.0 - array[i, 0]) * num5, array[i, 1] * num5, num5);
			nurbsSurface.SetCV(2, i, (IOn4dPoint)(object)val);
			val = new On4dPoint(array[i, 0] * num6, (0.0 - array[i, 0]) * num6, array[i, 1] * num6, num6);
			nurbsSurface.SetCV(3, i, (IOn4dPoint)(object)val);
			val = new On4dPoint(array[i, 0] * num5, 0.0 * num5, array[i, 1] * num5, num5);
			nurbsSurface.SetCV(4, i, (IOn4dPoint)(object)val);
			val = new On4dPoint(array[i, 0] * num6, array[i, 0] * num6, array[i, 1] * num6, num6);
			nurbsSurface.SetCV(5, i, (IOn4dPoint)(object)val);
			val = new On4dPoint(0.0 * num5, array[i, 0] * num5, array[i, 1] * num5, num5);
			nurbsSurface.SetCV(6, i, (IOn4dPoint)(object)val);
			val = new On4dPoint((0.0 - array[i, 0]) * num6, array[i, 0] * num6, array[i, 1] * num6, num6);
			nurbsSurface.SetCV(7, i, (IOn4dPoint)(object)val);
			val = new On4dPoint((0.0 - array[i, 0]) * num5, 0.0 * num5, array[i, 1] * num5, num5);
			nurbsSurface.SetCV(8, i, (IOn4dPoint)(object)val);
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
		OnPlane val2 = new OnPlane(OnUtil.On_origin, OnUtil.On_zaxis);
		OnPlane val3 = new OnPlane(OnUtil.On_origin, direction);
		OnXform val4 = new OnXform();
		val4.ChangeBasis((IOnPlane)(object)val3, (IOnPlane)(object)val2);
		nurbsSurface.Transform((IOnXform)(object)val4);
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
		dome = CSUtilRailRevolve.RailRevolve((IOnNurbsCurve)(object)profile, (IOnNurbsCurve)(object)railActual, 1.0, 1);
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
		nurbsSurface = CSUtilRailRevolve.RailRevolve((IOnNurbsCurve)(object)profile, (IOnNurbsCurve)(object)railActual, 1.0, 1, out outputRail);
		return true;
	}

	public static OnNurbsSurface ExtrudeSurface(OnNurbsCurve crv, IOn3dVector vector, double extrudeDistance, bool bothSides)
	{
		//IL_0010: Unknown result type (might be due to invalid IL or missing references)
		//IL_001b: Expected O, but got Unknown
		//IL_0038: Unknown result type (might be due to invalid IL or missing references)
		//IL_003d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0047: Expected O, but got Unknown
		//IL_0026: Unknown result type (might be due to invalid IL or missing references)
		//IL_0032: Expected O, but got Unknown
		OnNurbsCurve val = crv.Duplicate();
		OnNurbsCurve val2 = crv.Duplicate();
		((OnGeometry)val).Translate((IOn3dVector)(object)(new On3dVector(vector) * extrudeDistance));
		if (bothSides)
		{
			((OnGeometry)val2).Translate((IOn3dVector)(object)(new On3dVector(vector) * (0.0 - extrudeDistance)));
		}
		OnNurbsSurface val3 = new OnNurbsSurface();
		val3.CreateRuledSurface((IOnCurve)(object)val, (IOnCurve)(object)val2);
		return val3;
	}

	public static void FixBrepSolidOrientation(ref Brep brep, bool doFullClean = false)
	{
		OnBrep brep2 = brep.Convert();
		FixBrepSolidOrientation(ref brep2, doFullClean);
		brep = brep2.Convert();
	}

	public static void FixBrepSolidOrientation(ref OnBrep brep, bool doFullClean = false)
	{
		if (brep != null && ((OnObject)brep).IsValid())
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
				((OnGeometry)brep).ClearBoundingBox();
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
		brepNormal = ((OnSurface)brep.Face(faceIndex)).NormalAt(s, t);
		if (brep.m_F[faceIndex].m_bRev)
		{
			brepNormal.Reverse();
		}
		return brepNormal.IsValid();
	}

	public static bool GetClosestNormalToBrep(Brep brep, Point3d testPoint, ref Vector3d brepNormal)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_0020: Unknown result type (might be due to invalid IL or missing references)
		//IL_0025: Unknown result type (might be due to invalid IL or missing references)
		//IL_0030: Unknown result type (might be due to invalid IL or missing references)
		//IL_0035: Unknown result type (might be due to invalid IL or missing references)
		//IL_0041: Unknown result type (might be due to invalid IL or missing references)
		//IL_0048: Expected O, but got Unknown
		//IL_0050: Expected O, but got Unknown
		//IL_0053: Unknown result type (might be due to invalid IL or missing references)
		//IL_0058: Unknown result type (might be due to invalid IL or missing references)
		//IL_005b: Unknown result type (might be due to invalid IL or missing references)
		if (!GetClosestPointToBrep(brep, testPoint, out var faceIndex, out var u, out var v))
		{
			return false;
		}
		brepNormal = ((Surface)brep.Faces[faceIndex]).NormalAt(u, v);
		OnBrep val = (OnBrep)Interop.ToOnBrep(brep);
		On3dPoint point = CSConvert.Point3dCopyToOn3dPoint(testPoint);
		On3dVector brepNormal2 = new On3dVector(OnUtil.On_UNSET_VECTOR);
		GetClosestNormalToBrep((IOnBrep)val, (IOn3dPoint)(object)point, ref brepNormal2);
		Vector3d val2 = CSConvert.On3dVectorCopyToVector3d(brepNormal2);
		if (!((Vector3d)(ref brepNormal)).EpsilonEquals(val2, CSUtil.Tolerance))
		{
			CSUtil.PrintLine("GetClosestNormalToBrep ERROR");
		}
		return ((Vector3d)(ref brepNormal)).IsValid;
	}

	public static bool GetClosestPointToBrep(IOnBrep brep, IOn3dPoint point, ref On3dPoint brepPoint, ref On3dVector brepNormal)
	{
		if (!GetClosestPointToBrep(brep, point, out var faceIndex, out var s, out var t))
		{
			return false;
		}
		brepPoint = ((OnSurface)brep.Face(faceIndex)).PointAt(s, t);
		brepNormal = ((OnSurface)brep.Face(faceIndex)).NormalAt(s, t);
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
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_0020: Unknown result type (might be due to invalid IL or missing references)
		//IL_0025: Unknown result type (might be due to invalid IL or missing references)
		//IL_0039: Unknown result type (might be due to invalid IL or missing references)
		//IL_003e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0049: Unknown result type (might be due to invalid IL or missing references)
		//IL_004e: Unknown result type (might be due to invalid IL or missing references)
		//IL_005a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0061: Expected O, but got Unknown
		//IL_0069: Expected O, but got Unknown
		//IL_006c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0071: Unknown result type (might be due to invalid IL or missing references)
		//IL_0074: Unknown result type (might be due to invalid IL or missing references)
		if (!GetClosestPointToBrep(brep, testPoint, out var faceIndex, out var u, out var v))
		{
			return false;
		}
		brepPoint = ((Surface)brep.Faces[faceIndex]).PointAt(u, v);
		brepNormal = ((Surface)brep.Faces[faceIndex]).NormalAt(u, v);
		OnBrep val = (OnBrep)Interop.ToOnBrep(brep);
		On3dPoint point = CSConvert.Point3dCopyToOn3dPoint(testPoint);
		On3dVector brepNormal2 = new On3dVector(OnUtil.On_UNSET_VECTOR);
		GetClosestNormalToBrep((IOnBrep)val, (IOn3dPoint)(object)point, ref brepNormal2);
		Vector3d val2 = CSConvert.On3dVectorCopyToVector3d(brepNormal2);
		if (!((Vector3d)(ref brepNormal)).EpsilonEquals(val2, CSUtil.Tolerance))
		{
			CSUtil.PrintLine("GetClosestPointToBrep ERROR");
		}
		if (((Point3d)(ref brepPoint)).IsValid)
		{
			return ((Vector3d)(ref brepNormal)).IsValid;
		}
		return false;
	}

	public static bool GetClosestPointToBrep(IOnBrep brep, IOn3dPoint point, ref On3dPoint brepPoint)
	{
		if (!GetClosestPointToBrep(brep, point, out var faceIndex, out var s, out var t))
		{
			return false;
		}
		brepPoint = ((OnSurface)brep.Face(faceIndex)).PointAt(s, t);
		return brepPoint.IsValid();
	}

	public static bool GetClosestPointToBrep(Brep brep, Point3d testPoint, ref Point3d brepPoint)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_0020: Unknown result type (might be due to invalid IL or missing references)
		//IL_0025: Unknown result type (might be due to invalid IL or missing references)
		if (!GetClosestPointToBrep(brep, testPoint, out var faceIndex, out var u, out var v))
		{
			return false;
		}
		brepPoint = ((Surface)brep.Faces[faceIndex]).PointAt(u, v);
		return ((Point3d)(ref brepPoint)).IsValid;
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
			IOnBrepFace val = brep.m_F[i];
			double num2 = -1.23432101234321E+308;
			double num3 = -1.23432101234321E+308;
			if (((IOnSurface)val).GetClosestPoint(point, ref num2, ref num3, num))
			{
				double num4 = ((IOnSurface)val).PointAt(num2, num3).DistanceTo(point);
				if (!RhinoMath.IsValidDouble(num) || num4 < num)
				{
					num = num4;
					faceIndex = i;
					s = num2;
					t = num3;
				}
			}
		}
		return RhinoMath.IsValidDouble(num);
	}

	public static bool GetClosestPointToBrep(Brep brep, Point3d pt, out int faceIndex, out double u, out double v)
	{
		//IL_003b: Unknown result type (might be due to invalid IL or missing references)
		//IL_004b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0050: Unknown result type (might be due to invalid IL or missing references)
		//IL_0054: Unknown result type (might be due to invalid IL or missing references)
		faceIndex = -1;
		u = double.NaN;
		v = double.NaN;
		if (brep == null)
		{
			return false;
		}
		double num = double.NaN;
		double num2 = default(double);
		double num3 = default(double);
		for (int i = 0; i < brep.Faces.Count; i++)
		{
			BrepFace val = brep.Faces[i];
			if (((Surface)val).ClosestPoint(pt, ref num2, ref num3))
			{
				Point3d val2 = ((Surface)val).PointAt(num2, num3);
				double num4 = ((Point3d)(ref val2)).DistanceTo(pt);
				if (double.IsNaN(num) || num4 < num)
				{
					num = num4;
					faceIndex = i;
					u = num2;
					v = num3;
				}
			}
		}
		return !double.IsNaN(num);
	}

	public static double GetDotProduct(IOn3dPoint point1, IOn3dPoint point2, IOn3dPoint point3)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_000c: Expected O, but got Unknown
		//IL_000d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0018: Expected O, but got Unknown
		On3dVector obj = new On3dPoint(point1) - point2;
		On3dVector val = new On3dPoint(point3) - point2;
		obj.Unitize();
		val.Unitize();
		return obj * (IOn3dVector)(object)val;
	}

	public static void GetMinMax(IOnCurve curve, IOn3dPoint testPoint, int pointCount, out double min, out double max)
	{
		//IL_001c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0022: Expected O, but got Unknown
		if (pointCount < 1)
		{
			min = double.MaxValue;
			max = 0.0;
			return;
		}
		On3dPointArray val = new On3dPointArray();
		double[] array = new double[pointCount];
		for (int i = 0; i < pointCount; i++)
		{
			array[i] = (double)i / (double)pointCount;
		}
		curve.GetNormalizedArcLengthPoints(array, ref array);
		double[] array2 = array;
		foreach (double num in array2)
		{
			((ArrayOn3dPoint)val).Append((IOn3dPoint)(object)curve.PointAt(num));
		}
		GetMinMax((IArrayOn3dPoint)(object)val, testPoint, out min, out max);
	}

	public static void GetMinMax(Curve curve, Point3d testPoint, int pointCount, out double min, out double max)
	{
		//IL_001c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0022: Expected O, but got Unknown
		//IL_005b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0073: Unknown result type (might be due to invalid IL or missing references)
		if (pointCount < 1)
		{
			min = double.MaxValue;
			max = 0.0;
			return;
		}
		Point3dList val = new Point3dList();
		double[] array = new double[pointCount];
		for (int i = 0; i < pointCount; i++)
		{
			array[i] = (double)i / (double)pointCount;
		}
		array = curve.NormalizedLengthParameters(array, CSUtil.Tolerance);
		double[] array2 = array;
		foreach (double num in array2)
		{
			((RhinoList<Point3d>)(object)val).Add(curve.PointAt(num));
		}
		GetMinMax(val, testPoint, out min, out max);
	}

	[Obsolete]
	public static OnXform GetObjectDisplayXform(OnBoundingBox boundingBox)
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0006: Expected O, but got Unknown
		//IL_0011: Unknown result type (might be due to invalid IL or missing references)
		//IL_0017: Expected O, but got Unknown
		//IL_0036: Unknown result type (might be due to invalid IL or missing references)
		//IL_003c: Expected O, but got Unknown
		OnXform val = new OnXform();
		val.Identity();
		if (boundingBox == null)
		{
			return val;
		}
		OnXform val2 = new OnXform();
		val2.Rotation(CSUtilMath.ToRadians(-90.0), OnUtil.On_xaxis, (IOn3dPoint)(object)boundingBox.Center());
		OnXform val3 = new OnXform();
		val3.Rotation(CSUtilMath.ToRadians(-90.0), OnUtil.On_zaxis, (IOn3dPoint)(object)boundingBox.Center());
		On3dVector val4 = boundingBox.Diagonal();
		if (val4.z > 0.1 && val4.x > val4.y)
		{
			val = val2;
		}
		else if (val4.z > 0.1 && val4.y > val4.x)
		{
			val = val2 * (IOnXform)(object)val3;
		}
		return val;
	}

	public static Transform GetObjectDisplayXform(BoundingBox boundingBox)
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0005: Unknown result type (might be due to invalid IL or missing references)
		//IL_001f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0026: Unknown result type (might be due to invalid IL or missing references)
		//IL_002b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0030: Unknown result type (might be due to invalid IL or missing references)
		//IL_003f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0046: Unknown result type (might be due to invalid IL or missing references)
		//IL_004b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0050: Unknown result type (might be due to invalid IL or missing references)
		//IL_0053: Unknown result type (might be due to invalid IL or missing references)
		//IL_0058: Unknown result type (might be due to invalid IL or missing references)
		//IL_000f: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a9: Unknown result type (might be due to invalid IL or missing references)
		//IL_007b: Unknown result type (might be due to invalid IL or missing references)
		//IL_007c: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a1: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a2: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a3: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a8: Unknown result type (might be due to invalid IL or missing references)
		Transform result = Transform.Identity;
		if (!((BoundingBox)(ref boundingBox)).IsValid)
		{
			return result;
		}
		Transform val = CSUtilMath.Rotation(CSUtilMath.ToRadians(-90.0), Vector3d.XAxis, ((BoundingBox)(ref boundingBox)).Center);
		Transform val2 = CSUtilMath.Rotation(CSUtilMath.ToRadians(-90.0), Vector3d.ZAxis, ((BoundingBox)(ref boundingBox)).Center);
		Vector3d diagonal = ((BoundingBox)(ref boundingBox)).Diagonal;
		if (((Vector3d)(ref diagonal)).Z > 0.1 && ((Vector3d)(ref diagonal)).X > ((Vector3d)(ref diagonal)).Y)
		{
			result = val;
		}
		else if (((Vector3d)(ref diagonal)).Z > 0.1 && ((Vector3d)(ref diagonal)).Y > ((Vector3d)(ref diagonal)).X)
		{
			result = val * val2;
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
		IOnCurve[] inputCrvs = (IOnCurve[])(object)intersectionCurves;
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
		//IL_0003: Unknown result type (might be due to invalid IL or missing references)
		//IL_0009: Expected O, but got Unknown
		//IL_00da: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e1: Expected O, but got Unknown
		//IL_0094: Unknown result type (might be due to invalid IL or missing references)
		//IL_0099: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a8: Unknown result type (might be due to invalid IL or missing references)
		//IL_0119: Unknown result type (might be due to invalid IL or missing references)
		//IL_011f: Expected O, but got Unknown
		//IL_0133: Unknown result type (might be due to invalid IL or missing references)
		//IL_013d: Expected O, but got Unknown
		//IL_01f5: Unknown result type (might be due to invalid IL or missing references)
		//IL_01fc: Expected O, but got Unknown
		surface = null;
		MArgsRhinoLoft val = new MArgsRhinoLoft();
		val.m_bAllowStartTangent = true;
		val.m_bAllowEndTangent = true;
		val.m_bUseStartpoint = false;
		val.m_bUseEndpoint = false;
		val.m_bClosed = false;
		val.m_loft_type = 0;
		val.m_simplify_method = 0;
		val.m_start_condition = 4;
		val.m_end_condition = 4;
		val.m_rebuild_point_count = 10;
		val.m_refit_tolerance = 0.001;
		int count = bEdge.Count;
		MRhinoLoftCurve[] array = (MRhinoLoftCurve[])(object)new MRhinoLoftCurve[count];
		for (int i = 0; i < count; i++)
		{
			Curve obj = bEdge[i];
			BrepEdge val2 = (BrepEdge)(object)((obj is BrepEdge) ? obj : null);
			OnCurve val5;
			IOnBrepTrim trim;
			if (val2 != null)
			{
				OnBrep val3 = (OnBrep)Interop.ToOnBrep(val2.Brep);
				OnBrepEdge val4 = val3.Edge(val2.EdgeIndex);
				val5 = (OnCurve)(object)val3.Edge(val4.m_edge_index);
				trim = (IOnBrepTrim)(object)val3.Trim(val4.m_ti[0]);
			}
			else
			{
				val5 = (OnCurve)Interop.ToOnCurve(bEdge[i]);
				trim = null;
			}
			if (val5 != null)
			{
				OnCurve val6 = val5.DuplicateCurve();
				if (i == 0 && flip1)
				{
					val6.Reverse();
				}
				if (i == 1 && flip2)
				{
					val6.Reverse();
				}
				array[i] = new MRhinoLoftCurve();
				array[i].m_curve = val6;
				array[i].m_pick_point = new On3dPoint(OnUtil.On_UNSET_POINT);
				array[i].m_pick_t = -1.23432101234321E+308;
				array[i].m_trim = trim;
				array[i].m_bClosed = array[i].m_curve.IsClosed();
				array[i].m_bPlanar = array[i].m_curve.IsPlanar(array[i].m_plane);
			}
		}
		IRhinoLoftCurve[] loftcurves = (IRhinoLoftCurve[])(object)array;
		val.m_loftcurves = loftcurves;
		OnNurbsSurface[] array2 = null;
		RhUtil.RhinoSdkLoftSurface(val, ref array2);
		if ((array2 == null) | (array2.Length == 0))
		{
			return false;
		}
		if (array2.Length == 1)
		{
			OnBrep val7 = ((OnGeometry)array2[0]).BrepForm();
			surface = Interop.FromOnBrep((object)val7);
			return true;
		}
		List<OnBrep> list = new List<OnBrep>();
		for (int j = 0; j < array2.Length; j++)
		{
			if (((OnObject)array2[j]).IsValid())
			{
				OnBrep val8 = new OnBrep();
				val8.NewFace((IOnSurface)(object)array2[j]);
				list.Add(val8);
			}
		}
		OnBrep val9 = RhUtil.RhinoJoinBreps(list.ToArray(), 0.001);
		if (val9 != null)
		{
			surface = Interop.FromOnBrep((object)val9);
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
				list.Add(((IOnCurve)brep.m_E[i]).DuplicateCurve());
			}
		}
		return list.ToArray();
	}

	public static Curve Offset(Curve crv, Vector3d normalDir, Point3d origin, double distance, CurveOffsetCornerStyle cornerStyle = (CurveOffsetCornerStyle)1)
	{
		//IL_0024: Unknown result type (might be due to invalid IL or missing references)
		//IL_0025: Unknown result type (might be due to invalid IL or missing references)
		//IL_0030: Unknown result type (might be due to invalid IL or missing references)
		if (crv == null)
		{
			return null;
		}
		if (!((CommonObject)crv).IsValid)
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
		Curve[] array2 = Curve.JoinCurves((IEnumerable<Curve>)array);
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
		if (!((IOnObject)crv).IsValid())
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
		IOnCurve[] array = (IOnCurve[])(object)new IOnCurve[offsetCurves.Length];
		for (int j = 0; j < offsetCurves.Length; j++)
		{
			array[j] = (IOnCurve)(object)offsetCurves[j];
		}
		OnCurve[] joinRet = (OnCurve[])(object)new OnCurve[1];
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
		//IL_0024: Unknown result type (might be due to invalid IL or missing references)
		//IL_0025: Unknown result type (might be due to invalid IL or missing references)
		if (crv == null)
		{
			return null;
		}
		if (!((CommonObject)crv).IsValid)
		{
			return null;
		}
		if (Math.Abs(distance) <= CSUtil.Tolerance)
		{
			return crv.DuplicateCurve();
		}
		if (!RhUtilHelper.RhinoOffsetCurve(crv, normalDir, origin, distance, 0.0001, 0.01, (CurveOffsetCornerStyle)1, out var offsetCurves))
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
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0006: Expected O, but got Unknown
		//IL_0006: Unknown result type (might be due to invalid IL or missing references)
		//IL_000c: Expected O, but got Unknown
		//IL_0058: Unknown result type (might be due to invalid IL or missing references)
		//IL_0078: Unknown result type (might be due to invalid IL or missing references)
		//IL_0098: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a5: Expected O, but got Unknown
		//IL_00a5: Expected O, but got Unknown
		//IL_00a5: Expected O, but got Unknown
		On3dPoint val = new On3dPoint();
		On3dVector destination = new On3dVector();
		if (!rail.Ev1Der(param, ref val, ref destination))
		{
			return false;
		}
		On3dVector val2 = val - OnUtil.On_origin;
		val2.y *= yScale;
		val2.Unitize();
		x = CSUtilMath.Orient3Point((IOn3dPoint)new On3dPoint(0.0, 0.0, 0.0), (IOn3dVector)new On3dVector(0.0, 1.0, 0.0), (IOn3dVector)new On3dVector(0.0, 0.0, 1.0), (IOn3dPoint)(object)val, (IOn3dVector)(object)val2, (IOn3dVector)(object)destination);
		return true;
	}

	public static bool OrientProfileToCurve(Curve rail, double param, ref Transform x, double yScale)
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0005: Unknown result type (might be due to invalid IL or missing references)
		//IL_0006: Unknown result type (might be due to invalid IL or missing references)
		//IL_000b: Unknown result type (might be due to invalid IL or missing references)
		//IL_000e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0013: Unknown result type (might be due to invalid IL or missing references)
		//IL_0016: Unknown result type (might be due to invalid IL or missing references)
		//IL_001b: Unknown result type (might be due to invalid IL or missing references)
		//IL_001c: Unknown result type (might be due to invalid IL or missing references)
		//IL_001d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0029: Unknown result type (might be due to invalid IL or missing references)
		//IL_002a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0038: Unknown result type (might be due to invalid IL or missing references)
		//IL_0039: Unknown result type (might be due to invalid IL or missing references)
		//IL_003e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0043: Unknown result type (might be due to invalid IL or missing references)
		//IL_0078: Unknown result type (might be due to invalid IL or missing references)
		//IL_0098: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b8: Unknown result type (might be due to invalid IL or missing references)
		//IL_00bd: Unknown result type (might be due to invalid IL or missing references)
		//IL_00be: Unknown result type (might be due to invalid IL or missing references)
		//IL_00bf: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c0: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c5: Unknown result type (might be due to invalid IL or missing references)
		Point3d unset = Point3d.Unset;
		Vector3d unset2 = Vector3d.Unset;
		unset = rail.PointAt(param);
		unset2 = rail.TangentAt(param);
		if (unset == Point3d.Unset || unset2 == Vector3d.Unset)
		{
			return false;
		}
		Vector3d destination = unset - Point3d.Origin;
		((Vector3d)(ref destination)).Y = ((Vector3d)(ref destination)).Y * yScale;
		((Vector3d)(ref destination)).Unitize();
		x = CSUtilMath.Orient3Point(new Point3d(0.0, 0.0, 0.0), new Vector3d(0.0, 1.0, 0.0), new Vector3d(0.0, 0.0, 1.0), unset, destination, unset2);
		return true;
	}

	public static bool OrientProfileToCurveNalp(IOnCurve rail, double param, ref OnXform x, double yScale)
	{
		//IL_000d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0013: Expected O, but got Unknown
		//IL_0013: Unknown result type (might be due to invalid IL or missing references)
		//IL_0019: Expected O, but got Unknown
		if (!rail.GetNormalizedArcLengthPoint(param, ref param))
		{
			return false;
		}
		On3dPoint val = new On3dPoint();
		On3dVector val2 = new On3dVector();
		if (!rail.Ev1Der(param, ref val, ref val2))
		{
			return false;
		}
		val2.Unitize();
		On3dVector val3 = val - OnUtil.On_origin;
		val3.y *= yScale;
		val3.Unitize();
		x = CSUtilMath.Orient3Point(OnUtil.On_origin, OnUtil.On_yaxis, OnUtil.On_zaxis, (IOn3dPoint)(object)val, (IOn3dVector)(object)val3, (IOn3dVector)(object)val2);
		return true;
	}

	public static bool OrientProfileToCurveNalp(Curve rail, double param, ref Transform x, double yScale)
	{
		//IL_000d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0012: Unknown result type (might be due to invalid IL or missing references)
		//IL_0013: Unknown result type (might be due to invalid IL or missing references)
		//IL_0018: Unknown result type (might be due to invalid IL or missing references)
		//IL_001b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0020: Unknown result type (might be due to invalid IL or missing references)
		//IL_0023: Unknown result type (might be due to invalid IL or missing references)
		//IL_0028: Unknown result type (might be due to invalid IL or missing references)
		//IL_0029: Unknown result type (might be due to invalid IL or missing references)
		//IL_002a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0036: Unknown result type (might be due to invalid IL or missing references)
		//IL_0037: Unknown result type (might be due to invalid IL or missing references)
		//IL_004d: Unknown result type (might be due to invalid IL or missing references)
		//IL_004e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0053: Unknown result type (might be due to invalid IL or missing references)
		//IL_0058: Unknown result type (might be due to invalid IL or missing references)
		//IL_0072: Unknown result type (might be due to invalid IL or missing references)
		//IL_0077: Unknown result type (might be due to invalid IL or missing references)
		//IL_007c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0081: Unknown result type (might be due to invalid IL or missing references)
		//IL_0082: Unknown result type (might be due to invalid IL or missing references)
		//IL_0083: Unknown result type (might be due to invalid IL or missing references)
		//IL_0084: Unknown result type (might be due to invalid IL or missing references)
		//IL_0089: Unknown result type (might be due to invalid IL or missing references)
		if (!rail.NormalizedLengthParameter(param, ref param))
		{
			return false;
		}
		Point3d unset = Point3d.Unset;
		Vector3d val = Vector3d.Unset;
		unset = rail.PointAt(param);
		val = rail.TangentAt(param);
		if (unset == Point3d.Unset || val == Vector3d.Unset)
		{
			return false;
		}
		((Vector3d)(ref val)).Unitize();
		Vector3d destination = unset - Point3d.Origin;
		((Vector3d)(ref destination)).Y = ((Vector3d)(ref destination)).Y * yScale;
		((Vector3d)(ref destination)).Unitize();
		x = CSUtilMath.Orient3Point(Point3d.Origin, Vector3d.YAxis, Vector3d.ZAxis, unset, destination, val);
		return true;
	}

	public static bool OrientProfileToRailNalp(IOnCurve rail, double param, ref OnXform xOrient)
	{
		//IL_000d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0013: Expected O, but got Unknown
		//IL_0013: Unknown result type (might be due to invalid IL or missing references)
		//IL_0019: Expected O, but got Unknown
		//IL_0044: Unknown result type (might be due to invalid IL or missing references)
		//IL_004a: Expected O, but got Unknown
		if (!rail.GetNormalizedArcLengthPoint(param, ref param))
		{
			return false;
		}
		On3dPoint val = new On3dPoint();
		On3dVector val2 = new On3dVector();
		if (!rail.Ev1Der(param, ref val, ref val2))
		{
			return false;
		}
		val2.Unitize();
		On3dVector val3 = new On3dVector(val.x, 0.0, val.z);
		val3.Unitize();
		xOrient = CSUtilMath.Orient3Point(OnUtil.On_origin, OnUtil.On_yaxis, OnUtil.On_zaxis, (IOn3dPoint)(object)val, (IOn3dVector)(object)val3, (IOn3dVector)(object)val2);
		return true;
	}

	public static bool OrientToCurveNalp(IOnCurve rail, double param, ref OnXform x)
	{
		//IL_000d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0013: Expected O, but got Unknown
		//IL_0013: Unknown result type (might be due to invalid IL or missing references)
		//IL_0019: Expected O, but got Unknown
		//IL_0050: Unknown result type (might be due to invalid IL or missing references)
		//IL_0070: Unknown result type (might be due to invalid IL or missing references)
		//IL_0090: Unknown result type (might be due to invalid IL or missing references)
		//IL_009d: Expected O, but got Unknown
		//IL_009d: Expected O, but got Unknown
		//IL_009d: Expected O, but got Unknown
		if (!rail.GetNormalizedArcLengthPoint(param, ref param))
		{
			return false;
		}
		On3dPoint val = new On3dPoint();
		On3dVector destination = new On3dVector();
		if (!rail.Ev1Der(param, ref val, ref destination))
		{
			return false;
		}
		On3dVector destination2 = val - OnUtil.On_origin;
		x = CSUtilMath.Orient3Point((IOn3dPoint)new On3dPoint(0.0, 0.0, 0.0), (IOn3dVector)new On3dVector(0.0, 0.0, 1.0), (IOn3dVector)new On3dVector(1.0, 0.0, 0.0), (IOn3dPoint)(object)val, (IOn3dVector)(object)destination2, (IOn3dVector)(object)destination);
		return true;
	}

	public static bool OrientToCurveNalp(Curve rail, double param, ref Transform x)
	{
		//IL_000d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0012: Unknown result type (might be due to invalid IL or missing references)
		//IL_0013: Unknown result type (might be due to invalid IL or missing references)
		//IL_0018: Unknown result type (might be due to invalid IL or missing references)
		//IL_001b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0020: Unknown result type (might be due to invalid IL or missing references)
		//IL_0023: Unknown result type (might be due to invalid IL or missing references)
		//IL_0028: Unknown result type (might be due to invalid IL or missing references)
		//IL_0029: Unknown result type (might be due to invalid IL or missing references)
		//IL_002a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0036: Unknown result type (might be due to invalid IL or missing references)
		//IL_0037: Unknown result type (might be due to invalid IL or missing references)
		//IL_0045: Unknown result type (might be due to invalid IL or missing references)
		//IL_0046: Unknown result type (might be due to invalid IL or missing references)
		//IL_004b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0050: Unknown result type (might be due to invalid IL or missing references)
		//IL_006d: Unknown result type (might be due to invalid IL or missing references)
		//IL_008d: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ad: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b2: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b3: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b4: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b5: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ba: Unknown result type (might be due to invalid IL or missing references)
		if (!rail.NormalizedLengthParameter(param, ref param))
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
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_0002: Unknown result type (might be due to invalid IL or missing references)
		return Offset(crv, normalDir, Point3d.Origin, distance, (CurveOffsetCornerStyle)1);
	}

	public static Curve OriginOffsetOldXY(Curve crv, double distance, bool closed)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_0006: Unknown result type (might be due to invalid IL or missing references)
		return OffsetOld(crv, Vector3d.ZAxis, Point3d.Origin, distance, closed);
	}

	public static OnCurve OriginOffsetXY(IOnCurve crv, double distance, bool closed)
	{
		return OffsetOld(crv, OnUtil.On_zaxis, OnUtil.On_origin, distance, closed);
	}

	public static Curve OriginOffsetXY(Curve crv, double distance)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_0006: Unknown result type (might be due to invalid IL or missing references)
		return Offset(crv, Vector3d.ZAxis, Point3d.Origin, distance, (CurveOffsetCornerStyle)1);
	}

	public static bool SplitProfileInHalf(IOnCurve curve, ref OnCurve leftSide, ref OnCurve rightSide)
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0006: Expected O, but got Unknown
		//IL_0093: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b3: Unknown result type (might be due to invalid IL or missing references)
		//IL_00bd: Expected O, but got Unknown
		//IL_00bd: Expected O, but got Unknown
		//IL_00b8: Unknown result type (might be due to invalid IL or missing references)
		//IL_004c: Unknown result type (might be due to invalid IL or missing references)
		//IL_006c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0076: Expected O, but got Unknown
		//IL_0076: Expected O, but got Unknown
		//IL_0071: Unknown result type (might be due to invalid IL or missing references)
		//IL_00be: Expected O, but got Unknown
		//IL_00be: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c4: Expected O, but got Unknown
		OnPlane val = new OnPlane();
		if (!curve.IsPlanar(val))
		{
			return false;
		}
		OnLineCurve val2 = ((Math.Abs(OnUtil.ON_DotProduct(val.Normal(), OnUtil.On_yaxis)) > 0.9) ? new OnLineCurve((IOn3dPoint)new On3dPoint(0.0, 0.0, -100.0), (IOn3dPoint)new On3dPoint(0.0, 0.0, 100.0)) : new OnLineCurve((IOn3dPoint)new On3dPoint(0.0, -100.0, 0.0), (IOn3dPoint)new On3dPoint(0.0, 100.0, 0.0)));
		ArrayOnX_EVENT val3 = new ArrayOnX_EVENT();
		curve.IntersectCurve((IOnCurve)(object)val2, ref val3);
		if (val3.Count() < 2)
		{
			return false;
		}
		OnCurve obj = curve.DuplicateCurve();
		obj.ChangeClosedCurveSeam(val3[0][0]);
		double num = 0.0;
		obj.GetClosestPoint((IOn3dPoint)(object)val3[1][0], ref num);
		return obj.Split(num, ref leftSide, ref rightSide);
	}

	public static On3dVector TangentAtForce(OnNurbsCurve crv, double t)
	{
		On3dVector val = ((OnCurve)crv).TangentAt(t);
		if (val != (IOn3dVector)null && val.IsValid() && !val.IsZero())
		{
			return val;
		}
		double num = ((OnCurve)crv).Domain().Length();
		if (Math.Abs(t - ((OnCurve)crv).Domain().Min()) < Math.Abs(t - ((OnCurve)crv).Domain().Max()))
		{
			for (int i = 0; i < 1000; i++)
			{
				val = ((OnCurve)crv).TangentAt(t + num * ((double)i / 2000.0));
				if (val != (IOn3dVector)null && val.IsValid())
				{
					return val;
				}
			}
		}
		else
		{
			for (int j = 0; j < 1000; j++)
			{
				val = ((OnCurve)crv).TangentAt(t - num * ((double)j / 2000.0));
				if (val != (IOn3dVector)null && val.IsValid())
				{
					return val;
				}
			}
		}
		return null;
	}

	public static Curve Trim2dLineCurveToSurface(Curve curve2D, Surface srf3D)
	{
		//IL_0009: Unknown result type (might be due to invalid IL or missing references)
		//IL_000e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0011: Unknown result type (might be due to invalid IL or missing references)
		//IL_0016: Unknown result type (might be due to invalid IL or missing references)
		//IL_002e: Unknown result type (might be due to invalid IL or missing references)
		//IL_008d: Unknown result type (might be due to invalid IL or missing references)
		//IL_008e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0090: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c3: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d0: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d5: Unknown result type (might be due to invalid IL or missing references)
		//IL_00db: Expected O, but got Unknown
		Curve result = curve2D.DuplicateCurve();
		Interval val = srf3D.Domain(0);
		Interval val2 = srf3D.Domain(1);
		Point3d val3 = new Point3d(((Interval)(ref val)).Min, ((Interval)(ref val2)).Min, 0.0);
		Point3d val4 = default(Point3d);
		((Point3d)(ref val4))._002Ector(((Interval)(ref val)).Min, ((Interval)(ref val2)).Max, 0.0);
		Point3d val5 = default(Point3d);
		((Point3d)(ref val5))._002Ector(((Interval)(ref val)).Max, ((Interval)(ref val2)).Max, 0.0);
		Point3d val6 = default(Point3d);
		((Point3d)(ref val6))._002Ector(((Interval)(ref val)).Max, ((Interval)(ref val2)).Min, 0.0);
		Surface val7 = (Surface)(object)NurbsSurface.CreateFromCorners(val3, val4, val5, val6);
		CurveIntersections val8 = Intersection.CurveSurface(curve2D, val7, CSUtil.Tolerance, CSUtil.Tolerance);
		if (val8 != null && val8.Count > 0)
		{
			result = (Curve)new LineCurve(val8[0].PointB, val8[0].PointB2);
		}
		return result;
	}

	public static void TrimEndOfCrv(OnCurve crv, double len)
	{
		//IL_003a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0044: Expected O, but got Unknown
		double num = 0.0;
		crv.GetLength(ref num);
		double num2 = (num - len) / num;
		double num3 = 0.0;
		crv.GetNormalizedArcLengthPoint(num2, ref num3);
		crv.Trim((IOnInterval)new OnInterval(crv.Domain().Min(), num3));
	}

	public static void TrimEndOfCrv(ref Curve crv, double len)
	{
		//IL_001e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0023: Unknown result type (might be due to invalid IL or missing references)
		double length = crv.GetLength();
		double num = (length - len) / length;
		double num2 = default(double);
		crv.NormalizedLengthParameter(num, ref num2);
		Curve obj = crv;
		Interval domain = crv.Domain;
		crv = obj.Trim(((Interval)(ref domain)).Min, num2);
	}

	public static void TrimStartAndEndOfCrv(OnCurve crv, double len)
	{
		//IL_004a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0054: Expected O, but got Unknown
		double num = 0.0;
		crv.GetLength(ref num);
		double num2 = len / num;
		double num3 = 0.0;
		crv.GetNormalizedArcLengthPoint(num2, ref num3);
		double num4 = (num - len) / num;
		double num5 = 0.0;
		crv.GetNormalizedArcLengthPoint(num4, ref num5);
		crv.Trim((IOnInterval)new OnInterval(num3, num5));
	}

	public static void TrimStartAndEndOfCrv(ref Curve crv, double len)
	{
		double length = crv.GetLength();
		double num = len / length;
		double num2 = default(double);
		crv.NormalizedLengthParameter(num, ref num2);
		double num3 = (length - len) / length;
		double num4 = default(double);
		crv.NormalizedLengthParameter(num3, ref num4);
		crv = crv.Trim(num2, num4);
	}

	public static void TrimStartOfCrv(OnCurve crv, double len)
	{
		//IL_0038: Unknown result type (might be due to invalid IL or missing references)
		//IL_0042: Expected O, but got Unknown
		double num = 0.0;
		crv.GetLength(ref num);
		double num2 = len / num;
		double num3 = 0.0;
		crv.GetNormalizedArcLengthPoint(num2, ref num3);
		crv.Trim((IOnInterval)new OnInterval(num3, crv.Domain().Max()));
	}

	public static void TrimStartOfCrv(ref Curve crv, double len)
	{
		//IL_0027: Unknown result type (might be due to invalid IL or missing references)
		//IL_002c: Unknown result type (might be due to invalid IL or missing references)
		double length = crv.GetLength();
		double num = len / length;
		double num2 = 0.0;
		crv.NormalizedLengthParameter(num, ref num2);
		Curve obj = crv;
		double num3 = num2;
		Interval domain = crv.Domain;
		crv = obj.Trim(num3, ((Interval)(ref domain)).Max);
	}

	private static void AddLine(OnMesh m, int v0, int v1, int v2)
	{
		//IL_0115: Unknown result type (might be due to invalid IL or missing references)
		//IL_0116: Unknown result type (might be due to invalid IL or missing references)
		//IL_0118: Unknown result type (might be due to invalid IL or missing references)
		//IL_012d: Unknown result type (might be due to invalid IL or missing references)
		//IL_012f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0131: Unknown result type (might be due to invalid IL or missing references)
		//IL_0146: Unknown result type (might be due to invalid IL or missing references)
		//IL_0148: Unknown result type (might be due to invalid IL or missing references)
		//IL_0149: Unknown result type (might be due to invalid IL or missing references)
		Point3f val = default(Point3f);
		((Point3f)(ref val))._002Ector(((ArrayOn3fPoint)m.m_V)[v0].x, ((ArrayOn3fPoint)m.m_V)[v0].y, ((ArrayOn3fPoint)m.m_V)[v0].z);
		Point3f val2 = default(Point3f);
		((Point3f)(ref val2))._002Ector(((ArrayOn3fPoint)m.m_V)[v1].x, ((ArrayOn3fPoint)m.m_V)[v1].y, ((ArrayOn3fPoint)m.m_V)[v1].z);
		Point3f val3 = default(Point3f);
		((Point3f)(ref val3))._002Ector(((ArrayOn3fPoint)m.m_V)[v2].x, ((ArrayOn3fPoint)m.m_V)[v2].y, ((ArrayOn3fPoint)m.m_V)[v2].z);
		Point3d val4 = default(Point3d);
		((Point3d)(ref val4))._002Ector((double)((Point3f)(ref val)).X, (double)((Point3f)(ref val)).Y, (double)((Point3f)(ref val)).Z);
		Point3d val5 = default(Point3d);
		((Point3d)(ref val5))._002Ector((double)((Point3f)(ref val2)).X, (double)((Point3f)(ref val2)).Y, (double)((Point3f)(ref val2)).Z);
		Point3d val6 = default(Point3d);
		((Point3d)(ref val6))._002Ector((double)((Point3f)(ref val3)).X, (double)((Point3f)(ref val3)).Y, (double)((Point3f)(ref val3)).Z);
		RhinoDoc.ActiveDoc.Objects.AddLine(new Line(val4, val5));
		RhinoDoc.ActiveDoc.Objects.AddLine(new Line(val5, val6));
		RhinoDoc.ActiveDoc.Objects.AddLine(new Line(val6, val4));
	}

	private static void AddLine(OnMesh m, int v0, int v1, int v2, int v3)
	{
		//IL_0171: Unknown result type (might be due to invalid IL or missing references)
		//IL_0173: Unknown result type (might be due to invalid IL or missing references)
		//IL_0175: Unknown result type (might be due to invalid IL or missing references)
		//IL_018a: Unknown result type (might be due to invalid IL or missing references)
		//IL_018c: Unknown result type (might be due to invalid IL or missing references)
		//IL_018e: Unknown result type (might be due to invalid IL or missing references)
		//IL_01a3: Unknown result type (might be due to invalid IL or missing references)
		//IL_01a5: Unknown result type (might be due to invalid IL or missing references)
		//IL_01a7: Unknown result type (might be due to invalid IL or missing references)
		//IL_01bc: Unknown result type (might be due to invalid IL or missing references)
		//IL_01be: Unknown result type (might be due to invalid IL or missing references)
		//IL_01c0: Unknown result type (might be due to invalid IL or missing references)
		Point3f val = default(Point3f);
		((Point3f)(ref val))._002Ector(((ArrayOn3fPoint)m.m_V)[v0].x, ((ArrayOn3fPoint)m.m_V)[v0].y, ((ArrayOn3fPoint)m.m_V)[v0].z);
		Point3f val2 = default(Point3f);
		((Point3f)(ref val2))._002Ector(((ArrayOn3fPoint)m.m_V)[v1].x, ((ArrayOn3fPoint)m.m_V)[v1].y, ((ArrayOn3fPoint)m.m_V)[v1].z);
		Point3f val3 = default(Point3f);
		((Point3f)(ref val3))._002Ector(((ArrayOn3fPoint)m.m_V)[v2].x, ((ArrayOn3fPoint)m.m_V)[v2].y, ((ArrayOn3fPoint)m.m_V)[v2].z);
		Point3f val4 = default(Point3f);
		((Point3f)(ref val4))._002Ector(((ArrayOn3fPoint)m.m_V)[v3].x, ((ArrayOn3fPoint)m.m_V)[v3].y, ((ArrayOn3fPoint)m.m_V)[v3].z);
		Point3d val5 = default(Point3d);
		((Point3d)(ref val5))._002Ector((double)((Point3f)(ref val)).X, (double)((Point3f)(ref val)).Y, (double)((Point3f)(ref val)).Z);
		Point3d val6 = default(Point3d);
		((Point3d)(ref val6))._002Ector((double)((Point3f)(ref val2)).X, (double)((Point3f)(ref val2)).Y, (double)((Point3f)(ref val2)).Z);
		Point3d val7 = default(Point3d);
		((Point3d)(ref val7))._002Ector((double)((Point3f)(ref val3)).X, (double)((Point3f)(ref val3)).Y, (double)((Point3f)(ref val3)).Z);
		Point3d val8 = default(Point3d);
		((Point3d)(ref val8))._002Ector((double)((Point3f)(ref val4)).X, (double)((Point3f)(ref val4)).Y, (double)((Point3f)(ref val4)).Z);
		RhinoDoc.ActiveDoc.Objects.AddLine(new Line(val5, val6));
		RhinoDoc.ActiveDoc.Objects.AddLine(new Line(val6, val7));
		RhinoDoc.ActiveDoc.Objects.AddLine(new Line(val7, val8));
		RhinoDoc.ActiveDoc.Objects.AddLine(new Line(val8, val5));
	}

	private static void BuildProngRail(IOnNurbsCurve rail, out OnNurbsCurve railActual)
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0006: Expected O, but got Unknown
		//IL_004d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0054: Expected O, but got Unknown
		//IL_0066: Unknown result type (might be due to invalid IL or missing references)
		//IL_006d: Expected O, but got Unknown
		//IL_0074: Unknown result type (might be due to invalid IL or missing references)
		//IL_0084: Expected O, but got Unknown
		OnBoundingBox val = new OnBoundingBox();
		((IOnGeometry)rail).GetTightBoundingBox(ref val);
		double num = val.Max().x - val.Min().x;
		double num2 = val.Max().y - val.Min().y;
		double num3 = 4.0 / (num + num2);
		OnXform val2 = new OnXform();
		val2.Scale(num3, num3, 1.0);
		OnXform val3 = new OnXform();
		val3.Translation((IOn3dVector)(object)(new On3dPoint(OnUtil.On_origin) - (IOn3dPoint)(object)val.Center()));
		railActual = OnNurbsCurve.New(rail);
		railActual.Transform((IOnXform)(object)(val2 * (IOnXform)(object)val3));
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
		//IL_001c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0021: Unknown result type (might be due to invalid IL or missing references)
		//IL_0024: Unknown result type (might be due to invalid IL or missing references)
		max = 0.0;
		min = double.MaxValue;
		for (int i = 0; i < ((RhinoList<Point3d>)(object)points).Count; i++)
		{
			Point3d val = ((RhinoList<Point3d>)(object)points)[i];
			double num = ((Point3d)(ref val)).DistanceTo(testPoint);
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
