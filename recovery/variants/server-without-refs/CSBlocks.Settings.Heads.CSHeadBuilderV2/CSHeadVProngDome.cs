using System.Collections.Generic;
using CS;
using CSBlockUtil;
using Rhino.Geometry;
using Rhino.Geometry.Intersect;

namespace CSBlocks.Settings.Heads.CSHeadBuilderV2;

internal static class CSHeadVProngDome
{
	public static Brep VProngDome(int prongNumber, Brep vProng, double topZ, double baseZ, NurbsSurface intSrf0, NurbsSurface intSrf1, double radiusTop, double radiusBottom)
	{
		//IL_0019: Unknown result type (might be due to invalid IL or missing references)
		//IL_0099: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b2: Unknown result type (might be due to invalid IL or missing references)
		//IL_00cb: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e4: Unknown result type (might be due to invalid IL or missing references)
		//IL_0118: Unknown result type (might be due to invalid IL or missing references)
		//IL_011d: Unknown result type (might be due to invalid IL or missing references)
		//IL_005e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0063: Unknown result type (might be due to invalid IL or missing references)
		//IL_0066: Unknown result type (might be due to invalid IL or missing references)
		//IL_006b: Unknown result type (might be due to invalid IL or missing references)
		//IL_006e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0073: Unknown result type (might be due to invalid IL or missing references)
		//IL_0076: Unknown result type (might be due to invalid IL or missing references)
		//IL_007b: Unknown result type (might be due to invalid IL or missing references)
		//IL_018b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0191: Unknown result type (might be due to invalid IL or missing references)
		//IL_0197: Unknown result type (might be due to invalid IL or missing references)
		//IL_01a5: Unknown result type (might be due to invalid IL or missing references)
		//IL_01ab: Unknown result type (might be due to invalid IL or missing references)
		//IL_01b9: Unknown result type (might be due to invalid IL or missing references)
		//IL_01c8: Unknown result type (might be due to invalid IL or missing references)
		//IL_01ce: Unknown result type (might be due to invalid IL or missing references)
		//IL_01d4: Unknown result type (might be due to invalid IL or missing references)
		//IL_01da: Unknown result type (might be due to invalid IL or missing references)
		//IL_01e8: Unknown result type (might be due to invalid IL or missing references)
		//IL_01eb: Unknown result type (might be due to invalid IL or missing references)
		//IL_01f0: Unknown result type (might be due to invalid IL or missing references)
		//IL_01fb: Unknown result type (might be due to invalid IL or missing references)
		//IL_0201: Unknown result type (might be due to invalid IL or missing references)
		//IL_020f: Unknown result type (might be due to invalid IL or missing references)
		//IL_021d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0220: Unknown result type (might be due to invalid IL or missing references)
		//IL_0225: Unknown result type (might be due to invalid IL or missing references)
		//IL_0228: Unknown result type (might be due to invalid IL or missing references)
		//IL_0239: Unknown result type (might be due to invalid IL or missing references)
		//IL_023e: Unknown result type (might be due to invalid IL or missing references)
		//IL_024a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0250: Unknown result type (might be due to invalid IL or missing references)
		//IL_027a: Unknown result type (might be due to invalid IL or missing references)
		//IL_027f: Unknown result type (might be due to invalid IL or missing references)
		//IL_028d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0292: Unknown result type (might be due to invalid IL or missing references)
		//IL_02a6: Unknown result type (might be due to invalid IL or missing references)
		//IL_02ab: Unknown result type (might be due to invalid IL or missing references)
		//IL_02b9: Unknown result type (might be due to invalid IL or missing references)
		//IL_02be: Unknown result type (might be due to invalid IL or missing references)
		//IL_02d5: Unknown result type (might be due to invalid IL or missing references)
		//IL_02db: Unknown result type (might be due to invalid IL or missing references)
		//IL_02e9: Unknown result type (might be due to invalid IL or missing references)
		//IL_02f7: Unknown result type (might be due to invalid IL or missing references)
		//IL_02fe: Unknown result type (might be due to invalid IL or missing references)
		//IL_030c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0311: Unknown result type (might be due to invalid IL or missing references)
		//IL_0316: Unknown result type (might be due to invalid IL or missing references)
		//IL_0321: Unknown result type (might be due to invalid IL or missing references)
		//IL_032f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0335: Unknown result type (might be due to invalid IL or missing references)
		//IL_0343: Unknown result type (might be due to invalid IL or missing references)
		//IL_0348: Unknown result type (might be due to invalid IL or missing references)
		//IL_034d: Unknown result type (might be due to invalid IL or missing references)
		//IL_035b: Unknown result type (might be due to invalid IL or missing references)
		//IL_035d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0368: Unknown result type (might be due to invalid IL or missing references)
		//IL_036b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0379: Unknown result type (might be due to invalid IL or missing references)
		//IL_0380: Unknown result type (might be due to invalid IL or missing references)
		//IL_0385: Unknown result type (might be due to invalid IL or missing references)
		//IL_038a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0394: Unknown result type (might be due to invalid IL or missing references)
		//IL_039f: Unknown result type (might be due to invalid IL or missing references)
		//IL_03a5: Unknown result type (might be due to invalid IL or missing references)
		//IL_03b3: Unknown result type (might be due to invalid IL or missing references)
		//IL_03b8: Unknown result type (might be due to invalid IL or missing references)
		//IL_03bd: Unknown result type (might be due to invalid IL or missing references)
		//IL_03cb: Unknown result type (might be due to invalid IL or missing references)
		//IL_03cd: Unknown result type (might be due to invalid IL or missing references)
		//IL_03d9: Unknown result type (might be due to invalid IL or missing references)
		//IL_03de: Unknown result type (might be due to invalid IL or missing references)
		//IL_03ea: Unknown result type (might be due to invalid IL or missing references)
		//IL_03f0: Unknown result type (might be due to invalid IL or missing references)
		//IL_041a: Unknown result type (might be due to invalid IL or missing references)
		//IL_041f: Unknown result type (might be due to invalid IL or missing references)
		//IL_042d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0432: Unknown result type (might be due to invalid IL or missing references)
		//IL_0446: Unknown result type (might be due to invalid IL or missing references)
		//IL_044b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0459: Unknown result type (might be due to invalid IL or missing references)
		//IL_045e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0475: Unknown result type (might be due to invalid IL or missing references)
		//IL_047b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0489: Unknown result type (might be due to invalid IL or missing references)
		//IL_0497: Unknown result type (might be due to invalid IL or missing references)
		//IL_049e: Unknown result type (might be due to invalid IL or missing references)
		//IL_04ac: Unknown result type (might be due to invalid IL or missing references)
		//IL_04b1: Unknown result type (might be due to invalid IL or missing references)
		//IL_04b6: Unknown result type (might be due to invalid IL or missing references)
		//IL_04c1: Unknown result type (might be due to invalid IL or missing references)
		//IL_04c7: Unknown result type (might be due to invalid IL or missing references)
		//IL_04cc: Unknown result type (might be due to invalid IL or missing references)
		//IL_04d1: Unknown result type (might be due to invalid IL or missing references)
		//IL_04df: Unknown result type (might be due to invalid IL or missing references)
		//IL_04e1: Unknown result type (might be due to invalid IL or missing references)
		//IL_04ec: Unknown result type (might be due to invalid IL or missing references)
		//IL_04ef: Unknown result type (might be due to invalid IL or missing references)
		//IL_04fd: Unknown result type (might be due to invalid IL or missing references)
		//IL_0504: Unknown result type (might be due to invalid IL or missing references)
		//IL_0509: Unknown result type (might be due to invalid IL or missing references)
		//IL_050e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0518: Unknown result type (might be due to invalid IL or missing references)
		//IL_051b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0520: Unknown result type (might be due to invalid IL or missing references)
		//IL_0525: Unknown result type (might be due to invalid IL or missing references)
		//IL_0533: Unknown result type (might be due to invalid IL or missing references)
		//IL_0535: Unknown result type (might be due to invalid IL or missing references)
		//IL_0133: Unknown result type (might be due to invalid IL or missing references)
		//IL_0138: Unknown result type (might be due to invalid IL or missing references)
		//IL_013d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0154: Unknown result type (might be due to invalid IL or missing references)
		//IL_0159: Unknown result type (might be due to invalid IL or missing references)
		//IL_0172: Unknown result type (might be due to invalid IL or missing references)
		//IL_0174: Unknown result type (might be due to invalid IL or missing references)
		//IL_0176: Unknown result type (might be due to invalid IL or missing references)
		//IL_0178: Unknown result type (might be due to invalid IL or missing references)
		//IL_017b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0180: Unknown result type (might be due to invalid IL or missing references)
		//IL_0183: Unknown result type (might be due to invalid IL or missing references)
		//IL_0188: Unknown result type (might be due to invalid IL or missing references)
		Transform.Translation((double)((prongNumber + 1) * 20), 40.0, 0.0);
		if (!CutBrep(vProng, topZ, intSrf0, out var crvInner, out var crvOuter))
		{
			return null;
		}
		if (!CutBrep(vProng, baseZ, intSrf0, out var crvInner2, out var crvOuter2))
		{
			return null;
		}
		GetEdgeCurves(vProng, crvInner, intSrf0, intSrf1, out var edgeCurveStart, out var edgeCurveEnd);
		if (edgeCurveStart == null || edgeCurveEnd == null)
		{
			return null;
		}
		Point3d val;
		Point3d val2;
		Vector3d tangentAtStart;
		Vector3d tangentAtEnd;
		if (crvInner2 != null)
		{
			val = crvInner2.PointAtStart;
			val2 = crvInner2.PointAtEnd;
			tangentAtStart = crvInner2.TangentAtStart;
			tangentAtEnd = crvInner2.TangentAtEnd;
		}
		else
		{
			NurbsSurface val3 = NurbsSurface.CreateFromCorners(new Point3d(40.0, 40.0, baseZ), new Point3d(40.0, -40.0, baseZ), new Point3d(-40.0, -40.0, baseZ), new Point3d(-40.0, 40.0, baseZ));
			CurveIntersections obj = Intersection.CurveSurface(edgeCurveStart, (Surface)(object)val3, CSApplicationHelper.Application.Tolerance, CSApplicationHelper.Application.Tolerance);
			double num = 9999.0;
			Point3d val4 = Point3d.Unset;
			foreach (IntersectionEvent item in obj)
			{
				Vector3d val5 = new Vector3d(item.PointA);
				double length = ((Vector3d)(ref val5)).Length;
				if (length < num)
				{
					num = length;
					val4 = item.PointA;
				}
			}
			val = val4;
			val2 = val4;
			tangentAtStart = crvOuter2.TangentAtStart;
			tangentAtEnd = crvOuter2.TangentAtEnd;
		}
		Curve val6 = ProngCurve(crvInner.PointAtStart, crvOuter.PointAtStart, crvInner.TangentAtStart * -1.0, crvOuter.TangentAtStart * -1.0, radiusTop);
		Curve val7 = ProngCurve(crvInner.PointAtEnd, crvOuter.PointAtEnd, crvInner.TangentAtEnd, crvOuter.TangentAtEnd, radiusTop);
		Curve val8 = ProngCurve(val, crvOuter2.PointAtStart, tangentAtStart * -1.0, crvOuter2.TangentAtStart * -1.0, radiusBottom);
		Curve val9 = ProngCurve(val2, crvOuter2.PointAtEnd, tangentAtEnd, crvOuter2.TangentAtEnd, radiusBottom);
		Curve val10 = CurveBetween2Points(edgeCurveStart, crvInner.PointAtStart, val);
		Curve val11 = CurveBetween2Points(edgeCurveStart, crvOuter.PointAtStart, crvOuter2.PointAtStart);
		Curve railCurve = val10;
		Curve railCurve2 = val11;
		Curve[] profile = new Curve[2] { val6, val8 };
		double[] array = new double[2];
		Interval domain = val10.Domain;
		array[0] = ((Interval)(ref domain)).Min;
		domain = val10.Domain;
		array[1] = ((Interval)(ref domain)).Max;
		double[] array2 = new double[2];
		domain = val11.Domain;
		array2[0] = ((Interval)(ref domain)).Min;
		domain = val11.Domain;
		array2[1] = ((Interval)(ref domain)).Max;
		CSUtilSweepRC.Sweep2(railCurve, railCurve2, (Curve[])(object)profile, array, array2, out var breps, closed: false, maintainHeight: true);
		Curve c = CurveBetween2Points(edgeCurveStart, crvInner.PointAtStart, crvOuter.PointAtStart);
		Vector3d v = val10.TangentAtStart * -1.0 + val11.TangentAtStart * -1.0;
		((Vector3d)(ref v)).Unitize();
		Vector3d v2 = crvInner.TangentAtStart * -1.0 + crvOuter.TangentAtStart * -1.0;
		((Vector3d)(ref v2)).Unitize();
		NurbsSurface val12 = LoftDir(val6, c, v, v2);
		Curve c2 = CurveBetween2Points(edgeCurveStart, val, crvOuter2.PointAtStart);
		v = val10.TangentAtEnd + val11.TangentAtEnd;
		((Vector3d)(ref v)).Unitize();
		v2 = tangentAtStart * -1.0 + crvOuter2.TangentAtStart * -1.0;
		((Vector3d)(ref v2)).Unitize();
		NurbsSurface val13 = LoftDir(val8, c2, v, v2);
		val10 = CurveBetween2Points(edgeCurveEnd, crvInner.PointAtEnd, val2);
		val11 = CurveBetween2Points(edgeCurveEnd, crvOuter.PointAtEnd, crvOuter2.PointAtEnd);
		Curve railCurve3 = val10;
		Curve railCurve4 = val11;
		Curve[] profile2 = new Curve[2] { val7, val9 };
		double[] array3 = new double[2];
		domain = val10.Domain;
		array3[0] = ((Interval)(ref domain)).Min;
		domain = val10.Domain;
		array3[1] = ((Interval)(ref domain)).Max;
		double[] array4 = new double[2];
		domain = val11.Domain;
		array4[0] = ((Interval)(ref domain)).Min;
		domain = val11.Domain;
		array4[1] = ((Interval)(ref domain)).Max;
		CSUtilSweepRC.Sweep2(railCurve3, railCurve4, (Curve[])(object)profile2, array3, array4, out var breps2, closed: false, maintainHeight: true);
		Curve c3 = CurveBetween2Points(edgeCurveEnd, crvInner.PointAtEnd, crvOuter.PointAtEnd);
		v = val10.TangentAtStart * -1.0 + val11.TangentAtStart * -1.0;
		((Vector3d)(ref v)).Unitize();
		v2 = crvInner.TangentAtEnd + crvOuter.TangentAtEnd;
		((Vector3d)(ref v2)).Unitize();
		NurbsSurface val14 = LoftDir(val7, c3, v, v2);
		Curve c4 = CurveBetween2Points(edgeCurveEnd, val2, crvOuter2.PointAtEnd);
		v = val10.TangentAtEnd + val11.TangentAtEnd;
		((Vector3d)(ref v)).Unitize();
		v2 = tangentAtEnd + crvOuter2.TangentAtEnd;
		((Vector3d)(ref v2)).Unitize();
		NurbsSurface val15 = LoftDir(val9, c4, v, v2);
		return Brep.JoinBreps((IEnumerable<Brep>)(object)new Brep[7]
		{
			vProng,
			breps,
			breps2,
			((Surface)val12).ToBrep(),
			((Surface)val13).ToBrep(),
			((Surface)val14).ToBrep(),
			((Surface)val15).ToBrep()
		}, CSApplicationHelper.Application.Tolerance)[0];
	}

	private static Curve CurveBetween2Points(Curve crv, Point3d point0, Point3d point1)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_0006: Unknown result type (might be due to invalid IL or missing references)
		//IL_0007: Unknown result type (might be due to invalid IL or missing references)
		//IL_000c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0035: Unknown result type (might be due to invalid IL or missing references)
		//IL_0022: Unknown result type (might be due to invalid IL or missing references)
		Vector3d val = crv.PointAtStart - point0;
		if (((Vector3d)(ref val)).Length > CSApplicationHelper.Application.Tolerance)
		{
			double num = default(double);
			crv.ClosestPoint(point0, ref num);
			crv.ChangeClosedCurveSeam(num);
		}
		double num2 = default(double);
		crv.ClosestPoint(point1, ref num2);
		Curve[] array = crv.Split(num2);
		if (array.Length != 2)
		{
			return null;
		}
		double length = array[0].GetLength();
		double length2 = array[1].GetLength();
		if (length < length2)
		{
			return array[0];
		}
		Curve obj = array[1];
		obj.Reverse();
		return obj;
	}

	private static bool CutBrep(Brep vProng, double zCut, NurbsSurface intSrf, out Curve crvInner, out Curve crvOuter)
	{
		//IL_0013: Unknown result type (might be due to invalid IL or missing references)
		//IL_002b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0043: Unknown result type (might be due to invalid IL or missing references)
		//IL_005b: Unknown result type (might be due to invalid IL or missing references)
		//IL_00bc: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c1: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c2: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c7: Unknown result type (might be due to invalid IL or missing references)
		//IL_00de: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e3: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e4: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e9: Unknown result type (might be due to invalid IL or missing references)
		NurbsSurface val = NurbsSurface.CreateFromCorners(new Point3d(40.0, 40.0, zCut), new Point3d(40.0, -40.0, zCut), new Point3d(-40.0, -40.0, zCut), new Point3d(-40.0, 40.0, zCut));
		Point3d[] array2 = default(Point3d[]);
		Curve[] array = default(Curve[]);
		Intersection.BrepSurface(vProng, (Surface)(object)val, CSApplicationHelper.Application.Tolerance, ref array, ref array2);
		array = Curve.JoinCurves((IEnumerable<Curve>)array, CSApplicationHelper.Application.Tolerance);
		if (array.Length == 2)
		{
			Point3d val2 = default(Point3d);
			((Point3d)(ref val2))._002Ector(0.0, 0.0, zCut);
			Vector3d val3 = array[0].PointAtNormalizedLength(0.5) - val2;
			double length = ((Vector3d)(ref val3)).Length;
			val3 = array[1].PointAtNormalizedLength(0.5) - val2;
			double length2 = ((Vector3d)(ref val3)).Length;
			crvInner = ((length < length2) ? array[0] : array[1]);
			crvOuter = ((length < length2) ? array[1] : array[0]);
			TestFlip(ref crvInner, intSrf);
			TestFlip(ref crvOuter, intSrf);
			return true;
		}
		if (array.Length == 1)
		{
			crvInner = null;
			crvOuter = array[0];
			TestFlip(ref crvOuter, intSrf);
			return true;
		}
		crvInner = null;
		crvOuter = null;
		return false;
	}

	private static void GetEdgeCurves(Brep vProng, Curve crvTopInner, NurbsSurface srf0, NurbsSurface srf1, out Curve edgeCurveStart, out Curve edgeCurveEnd)
	{
		//IL_01ee: Unknown result type (might be due to invalid IL or missing references)
		//IL_01f7: Unknown result type (might be due to invalid IL or missing references)
		//IL_01fc: Unknown result type (might be due to invalid IL or missing references)
		//IL_0201: Unknown result type (might be due to invalid IL or missing references)
		//IL_0207: Expected O, but got Unknown
		//IL_0238: Unknown result type (might be due to invalid IL or missing references)
		//IL_023d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0241: Unknown result type (might be due to invalid IL or missing references)
		//IL_024e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0253: Unknown result type (might be due to invalid IL or missing references)
		//IL_0257: Unknown result type (might be due to invalid IL or missing references)
		//IL_0264: Unknown result type (might be due to invalid IL or missing references)
		//IL_0269: Unknown result type (might be due to invalid IL or missing references)
		//IL_026a: Unknown result type (might be due to invalid IL or missing references)
		//IL_026b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0270: Unknown result type (might be due to invalid IL or missing references)
		//IL_027b: Unknown result type (might be due to invalid IL or missing references)
		//IL_027c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0281: Unknown result type (might be due to invalid IL or missing references)
		//IL_006e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0073: Unknown result type (might be due to invalid IL or missing references)
		//IL_0076: Unknown result type (might be due to invalid IL or missing references)
		//IL_0087: Unknown result type (might be due to invalid IL or missing references)
		//IL_008c: Unknown result type (might be due to invalid IL or missing references)
		//IL_008f: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a0: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a5: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a9: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ab: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ad: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b2: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c0: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c2: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c4: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c9: Unknown result type (might be due to invalid IL or missing references)
		Curve[] array = vProng.DuplicateEdgeCurves(true);
		Vector3d val5;
		if (array.Length != 2)
		{
			double[] array2 = new double[6] { 0.0, 0.2, 0.4, 0.6, 0.8, 1.0 };
			List<Curve> list = new List<Curve>();
			List<Curve> list2 = new List<Curve>();
			Curve[] array3 = array;
			double num4 = default(double);
			double num5 = default(double);
			foreach (Curve val in array3)
			{
				double num = 0.0;
				double num2 = 0.0;
				double[] array4 = array2;
				foreach (double num3 in array4)
				{
					Point3d val2 = val.PointAtNormalizedLength(num3);
					((Surface)srf0).ClosestPoint(val2, ref num4, ref num5);
					Point3d val3 = ((Surface)srf0).PointAt(num4, num5);
					((Surface)srf1).ClosestPoint(val2, ref num4, ref num5);
					Point3d val4 = ((Surface)srf1).PointAt(num4, num5);
					double num6 = num;
					val5 = val2 - val3;
					num = num6 + ((Vector3d)(ref val5)).Length;
					double num7 = num2;
					val5 = val2 - val4;
					num2 = num7 + ((Vector3d)(ref val5)).Length;
				}
				if (num < num2)
				{
					list.Add(val);
				}
				else
				{
					list2.Add(val);
				}
			}
			array = (Curve[])(object)new Curve[2];
			if (list.Count == 1)
			{
				array[0] = list[0];
			}
			else
			{
				Curve[] array5 = Curve.JoinCurves((IEnumerable<Curve>)list, CSApplicationHelper.Application.Tolerance);
				if (array5.Length == 1)
				{
					array[0] = array5[0];
				}
				else
				{
					array[0] = null;
				}
			}
			if (list2.Count == 1)
			{
				array[1] = list2[0];
			}
			else
			{
				Curve[] array6 = Curve.JoinCurves((IEnumerable<Curve>)list2, CSApplicationHelper.Application.Tolerance);
				if (array6.Length == 1)
				{
					array[1] = array6[0];
				}
				else
				{
					array[1] = null;
				}
			}
		}
		for (int k = 0; k < 2; k++)
		{
			if (array[k].IsClosed)
			{
				continue;
			}
			if (array[k].IsClosable(CSApplicationHelper.Application.Tolerance))
			{
				array[k].MakeClosed(CSApplicationHelper.Application.Tolerance);
				if (array[k].IsClosed)
				{
					continue;
				}
			}
			Curve[] array7 = Curve.JoinCurves((IEnumerable<Curve>)(object)new Curve[2]
			{
				array[k],
				(Curve)new LineCurve(new Line(array[k].PointAtStart, array[k].PointAtEnd))
			});
			array[k] = array7[0];
		}
		if (array[0] == null || array[1] == null)
		{
			edgeCurveStart = null;
			edgeCurveEnd = null;
			return;
		}
		Point3d pointAtStart = crvTopInner.PointAtStart;
		double num8 = default(double);
		array[0].ClosestPoint(pointAtStart, ref num8);
		Point3d val6 = array[0].PointAt(num8);
		array[1].ClosestPoint(pointAtStart, ref num8);
		Point3d val7 = array[1].PointAt(num8);
		val5 = val6 - pointAtStart;
		double length = ((Vector3d)(ref val5)).Length;
		val5 = val7 - pointAtStart;
		double length2 = ((Vector3d)(ref val5)).Length;
		edgeCurveStart = ((length < length2) ? array[0] : array[1]);
		edgeCurveEnd = ((length < length2) ? array[1] : array[0]);
	}

	private static NurbsSurface LoftDir(Curve c0, Curve c1, Vector3d v0, Vector3d v1)
	{
		//IL_000e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0013: Unknown result type (might be due to invalid IL or missing references)
		//IL_006f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0074: Unknown result type (might be due to invalid IL or missing references)
		//IL_0078: Unknown result type (might be due to invalid IL or missing references)
		//IL_007d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0087: Unknown result type (might be due to invalid IL or missing references)
		//IL_008c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0090: Unknown result type (might be due to invalid IL or missing references)
		//IL_0095: Unknown result type (might be due to invalid IL or missing references)
		//IL_0099: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a2: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a4: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a7: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b5: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ba: Unknown result type (might be due to invalid IL or missing references)
		//IL_00bf: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c1: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c3: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c6: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d4: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d9: Unknown result type (might be due to invalid IL or missing references)
		//IL_00de: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e8: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f8: Unknown result type (might be due to invalid IL or missing references)
		Brep val = Brep.CreateFromLoft((IEnumerable<Curve>)(object)new Curve[2] { c0, c1 }, Point3d.Unset, Point3d.Unset, (LoftType)2, false)[0];
		if (val.Faces.Count != 1)
		{
			return null;
		}
		NurbsSurface val2 = ((Surface)val.Faces[0]).ToNurbsSurface();
		if (val2.Points.CountU != 4)
		{
			return null;
		}
		int countV = val2.Points.CountV;
		for (int i = 0; i < countV; i++)
		{
			ControlPoint controlPoint = val2.Points.GetControlPoint(0, i);
			Point3d val3 = ((ControlPoint)(ref controlPoint)).Location;
			controlPoint = val2.Points.GetControlPoint(3, i);
			Point3d location = ((ControlPoint)(ref controlPoint)).Location;
			double num = ((Point3d)(ref val3)).DistanceTo(location);
			val3 += v0 * num / 3.0;
			location += v1 * num / 3.0;
			val2.Points.SetControlPoint(1, i, val3);
			val2.Points.SetControlPoint(2, i, location);
		}
		return val2;
	}

	private static Curve ProngCurve(Point3d domeStart0, Point3d domeStart1, Vector3d prongDir0, Vector3d prongDir1, double domeRadius)
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_0002: Unknown result type (might be due to invalid IL or missing references)
		//IL_0010: Unknown result type (might be due to invalid IL or missing references)
		//IL_0015: Unknown result type (might be due to invalid IL or missing references)
		//IL_0016: Unknown result type (might be due to invalid IL or missing references)
		//IL_0017: Unknown result type (might be due to invalid IL or missing references)
		//IL_0018: Unknown result type (might be due to invalid IL or missing references)
		//IL_001d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0026: Unknown result type (might be due to invalid IL or missing references)
		//IL_0027: Unknown result type (might be due to invalid IL or missing references)
		//IL_002a: Unknown result type (might be due to invalid IL or missing references)
		//IL_002f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0034: Unknown result type (might be due to invalid IL or missing references)
		//IL_0035: Unknown result type (might be due to invalid IL or missing references)
		//IL_0036: Unknown result type (might be due to invalid IL or missing references)
		//IL_0037: Unknown result type (might be due to invalid IL or missing references)
		//IL_003c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0047: Unknown result type (might be due to invalid IL or missing references)
		//IL_0055: Unknown result type (might be due to invalid IL or missing references)
		//IL_0056: Unknown result type (might be due to invalid IL or missing references)
		//IL_0057: Unknown result type (might be due to invalid IL or missing references)
		//IL_0058: Unknown result type (might be due to invalid IL or missing references)
		//IL_007c: Unknown result type (might be due to invalid IL or missing references)
		//IL_007d: Unknown result type (might be due to invalid IL or missing references)
		//IL_007e: Unknown result type (might be due to invalid IL or missing references)
		//IL_007f: Unknown result type (might be due to invalid IL or missing references)
		Point3d val = (domeStart0 + domeStart1) / 2.0;
		Vector3d val2 = prongDir0 + prongDir1;
		((Vector3d)(ref val2)).Unitize();
		val += val2 * domeRadius;
		Vector3d val3 = domeStart0 - domeStart1;
		((Vector3d)(ref val3)).Unitize();
		Vector3d vectorEnd = default(Vector3d);
		((Vector3d)(ref vectorEnd))._002Ector(val3);
		((Vector3d)(ref vectorEnd)).Reverse();
		Curve val4 = (Curve)(object)CSUtilCurves.BlendCurve(domeStart0, val, prongDir0, val3);
		Curve val5 = (Curve)(object)CSUtilCurves.BlendCurve(domeStart1, val, prongDir1, vectorEnd);
		return (Curve)(object)Curve.JoinCurves((IEnumerable<Curve>)(object)new Curve[2] { val4, val5 })[0].Rebuild(10, 3, true);
	}

	private static void TestFlip(ref Curve crv, NurbsSurface srf)
	{
		//IL_0003: Unknown result type (might be due to invalid IL or missing references)
		//IL_0015: Unknown result type (might be due to invalid IL or missing references)
		//IL_001d: Unknown result type (might be due to invalid IL or missing references)
		//IL_002f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0034: Unknown result type (might be due to invalid IL or missing references)
		//IL_0037: Unknown result type (might be due to invalid IL or missing references)
		//IL_003c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0041: Unknown result type (might be due to invalid IL or missing references)
		//IL_004a: Unknown result type (might be due to invalid IL or missing references)
		//IL_004d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0052: Unknown result type (might be due to invalid IL or missing references)
		//IL_0057: Unknown result type (might be due to invalid IL or missing references)
		double num = default(double);
		double num2 = default(double);
		((Surface)srf).ClosestPoint(crv.PointAtStart, ref num, ref num2);
		Point3d val = ((Surface)srf).PointAt(num, num2);
		((Surface)srf).ClosestPoint(crv.PointAtEnd, ref num, ref num2);
		Point3d val2 = ((Surface)srf).PointAt(num, num2);
		Vector3d val3 = val - crv.PointAtStart;
		double length = ((Vector3d)(ref val3)).Length;
		val3 = val2 - crv.PointAtEnd;
		double length2 = ((Vector3d)(ref val3)).Length;
		if (length > length2)
		{
			crv.Reverse();
		}
	}
}
