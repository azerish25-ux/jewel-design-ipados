using System;
using System.Collections.Generic;
using CS;
using CSBlockUtil;
using Rhino;
using Rhino.Geometry;

namespace CSBlocks.Shank.Helpers;

internal static class CSFourRailSweep
{
	public static void Create4Rails(Curve crv, Curve baseCrv, double channelWallWidth, List<double> shankWidthatGem, List<double> gemCrvParamIn, List<double> baseCrvParamIn, bool startShankProfileEnabled, bool startShankProfileTangent, bool startChannelProfileTangent, bool endShankProfileEnabled, bool endShankProfileTangent, bool endChannelProfileTangent, out Curve baseCrv0, out Curve baseCrv1, out Curve gemCrv0, out Curve gemCrv1, out List<double> baseCrv0Param, out List<double> baseCrv1Param, out List<double> gemCrv0Param, out List<double> gemCrv1Param)
	{
		//IL_0053: Unknown result type (might be due to invalid IL or missing references)
		//IL_0058: Unknown result type (might be due to invalid IL or missing references)
		//IL_0069: Unknown result type (might be due to invalid IL or missing references)
		//IL_006e: Unknown result type (might be due to invalid IL or missing references)
		//IL_008a: Unknown result type (might be due to invalid IL or missing references)
		//IL_008f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0092: Unknown result type (might be due to invalid IL or missing references)
		//IL_00af: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b4: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b7: Unknown result type (might be due to invalid IL or missing references)
		//IL_0197: Unknown result type (might be due to invalid IL or missing references)
		//IL_01b0: Unknown result type (might be due to invalid IL or missing references)
		//IL_01b5: Unknown result type (might be due to invalid IL or missing references)
		//IL_01bc: Expected O, but got Unknown
		//IL_01be: Unknown result type (might be due to invalid IL or missing references)
		//IL_01e4: Unknown result type (might be due to invalid IL or missing references)
		//IL_01fd: Unknown result type (might be due to invalid IL or missing references)
		//IL_0202: Unknown result type (might be due to invalid IL or missing references)
		//IL_0209: Expected O, but got Unknown
		//IL_020b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0215: Unknown result type (might be due to invalid IL or missing references)
		//IL_024a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0253: Unknown result type (might be due to invalid IL or missing references)
		//IL_027e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0287: Unknown result type (might be due to invalid IL or missing references)
		//IL_0290: Unknown result type (might be due to invalid IL or missing references)
		//IL_02aa: Unknown result type (might be due to invalid IL or missing references)
		//IL_02af: Unknown result type (might be due to invalid IL or missing references)
		//IL_02bf: Unknown result type (might be due to invalid IL or missing references)
		//IL_02cf: Unknown result type (might be due to invalid IL or missing references)
		//IL_02d6: Unknown result type (might be due to invalid IL or missing references)
		//IL_02dd: Unknown result type (might be due to invalid IL or missing references)
		//IL_03a9: Unknown result type (might be due to invalid IL or missing references)
		//IL_03ca: Unknown result type (might be due to invalid IL or missing references)
		//IL_03e6: Unknown result type (might be due to invalid IL or missing references)
		//IL_03f6: Unknown result type (might be due to invalid IL or missing references)
		//IL_0417: Unknown result type (might be due to invalid IL or missing references)
		//IL_0433: Unknown result type (might be due to invalid IL or missing references)
		//IL_0474: Unknown result type (might be due to invalid IL or missing references)
		//IL_0479: Unknown result type (might be due to invalid IL or missing references)
		//IL_047b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0480: Unknown result type (might be due to invalid IL or missing references)
		//IL_0491: Unknown result type (might be due to invalid IL or missing references)
		//IL_0496: Unknown result type (might be due to invalid IL or missing references)
		//IL_04a8: Unknown result type (might be due to invalid IL or missing references)
		//IL_04ad: Unknown result type (might be due to invalid IL or missing references)
		//IL_04c3: Unknown result type (might be due to invalid IL or missing references)
		//IL_04c8: Unknown result type (might be due to invalid IL or missing references)
		//IL_04da: Unknown result type (might be due to invalid IL or missing references)
		//IL_04ea: Unknown result type (might be due to invalid IL or missing references)
		//IL_04ef: Unknown result type (might be due to invalid IL or missing references)
		//IL_0505: Unknown result type (might be due to invalid IL or missing references)
		//IL_050a: Unknown result type (might be due to invalid IL or missing references)
		//IL_051c: Unknown result type (might be due to invalid IL or missing references)
		//IL_052c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0531: Unknown result type (might be due to invalid IL or missing references)
		//IL_0547: Unknown result type (might be due to invalid IL or missing references)
		//IL_054c: Unknown result type (might be due to invalid IL or missing references)
		//IL_055e: Unknown result type (might be due to invalid IL or missing references)
		//IL_056e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0573: Unknown result type (might be due to invalid IL or missing references)
		//IL_0589: Unknown result type (might be due to invalid IL or missing references)
		//IL_058e: Unknown result type (might be due to invalid IL or missing references)
		//IL_05a0: Unknown result type (might be due to invalid IL or missing references)
		//IL_05b0: Unknown result type (might be due to invalid IL or missing references)
		//IL_05b5: Unknown result type (might be due to invalid IL or missing references)
		//IL_05cb: Unknown result type (might be due to invalid IL or missing references)
		//IL_05d0: Unknown result type (might be due to invalid IL or missing references)
		//IL_05e2: Unknown result type (might be due to invalid IL or missing references)
		//IL_05f2: Unknown result type (might be due to invalid IL or missing references)
		//IL_05f7: Unknown result type (might be due to invalid IL or missing references)
		//IL_060d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0612: Unknown result type (might be due to invalid IL or missing references)
		//IL_0624: Unknown result type (might be due to invalid IL or missing references)
		//IL_0634: Unknown result type (might be due to invalid IL or missing references)
		//IL_0639: Unknown result type (might be due to invalid IL or missing references)
		//IL_064f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0654: Unknown result type (might be due to invalid IL or missing references)
		//IL_0666: Unknown result type (might be due to invalid IL or missing references)
		//IL_0676: Unknown result type (might be due to invalid IL or missing references)
		//IL_067b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0691: Unknown result type (might be due to invalid IL or missing references)
		//IL_0696: Unknown result type (might be due to invalid IL or missing references)
		//IL_06a8: Unknown result type (might be due to invalid IL or missing references)
		//IL_06c1: Unknown result type (might be due to invalid IL or missing references)
		//IL_06c6: Unknown result type (might be due to invalid IL or missing references)
		//IL_0924: Unknown result type (might be due to invalid IL or missing references)
		//IL_0926: Unknown result type (might be due to invalid IL or missing references)
		//IL_0933: Unknown result type (might be due to invalid IL or missing references)
		//IL_0935: Unknown result type (might be due to invalid IL or missing references)
		//IL_0942: Unknown result type (might be due to invalid IL or missing references)
		//IL_0944: Unknown result type (might be due to invalid IL or missing references)
		//IL_0951: Unknown result type (might be due to invalid IL or missing references)
		//IL_0953: Unknown result type (might be due to invalid IL or missing references)
		//IL_09d6: Unknown result type (might be due to invalid IL or missing references)
		//IL_09db: Unknown result type (might be due to invalid IL or missing references)
		//IL_09ef: Unknown result type (might be due to invalid IL or missing references)
		//IL_09f4: Unknown result type (might be due to invalid IL or missing references)
		//IL_0a08: Unknown result type (might be due to invalid IL or missing references)
		//IL_0a0d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0a21: Unknown result type (might be due to invalid IL or missing references)
		//IL_0a26: Unknown result type (might be due to invalid IL or missing references)
		//IL_06e7: Unknown result type (might be due to invalid IL or missing references)
		//IL_06ec: Unknown result type (might be due to invalid IL or missing references)
		//IL_0703: Unknown result type (might be due to invalid IL or missing references)
		//IL_0708: Unknown result type (might be due to invalid IL or missing references)
		//IL_071a: Unknown result type (might be due to invalid IL or missing references)
		//IL_072e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0733: Unknown result type (might be due to invalid IL or missing references)
		//IL_074a: Unknown result type (might be due to invalid IL or missing references)
		//IL_074f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0761: Unknown result type (might be due to invalid IL or missing references)
		//IL_0775: Unknown result type (might be due to invalid IL or missing references)
		//IL_077a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0791: Unknown result type (might be due to invalid IL or missing references)
		//IL_0796: Unknown result type (might be due to invalid IL or missing references)
		//IL_07a8: Unknown result type (might be due to invalid IL or missing references)
		//IL_07bc: Unknown result type (might be due to invalid IL or missing references)
		//IL_07c1: Unknown result type (might be due to invalid IL or missing references)
		//IL_07d8: Unknown result type (might be due to invalid IL or missing references)
		//IL_07dd: Unknown result type (might be due to invalid IL or missing references)
		//IL_07ef: Unknown result type (might be due to invalid IL or missing references)
		//IL_080e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0813: Unknown result type (might be due to invalid IL or missing references)
		//IL_082a: Unknown result type (might be due to invalid IL or missing references)
		//IL_082f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0841: Unknown result type (might be due to invalid IL or missing references)
		//IL_0855: Unknown result type (might be due to invalid IL or missing references)
		//IL_085a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0871: Unknown result type (might be due to invalid IL or missing references)
		//IL_0876: Unknown result type (might be due to invalid IL or missing references)
		//IL_0888: Unknown result type (might be due to invalid IL or missing references)
		//IL_089c: Unknown result type (might be due to invalid IL or missing references)
		//IL_08a1: Unknown result type (might be due to invalid IL or missing references)
		//IL_08b8: Unknown result type (might be due to invalid IL or missing references)
		//IL_08bd: Unknown result type (might be due to invalid IL or missing references)
		//IL_08cf: Unknown result type (might be due to invalid IL or missing references)
		//IL_08e3: Unknown result type (might be due to invalid IL or missing references)
		//IL_08e8: Unknown result type (might be due to invalid IL or missing references)
		//IL_08ff: Unknown result type (might be due to invalid IL or missing references)
		//IL_0904: Unknown result type (might be due to invalid IL or missing references)
		//IL_0916: Unknown result type (might be due to invalid IL or missing references)
		//IL_0a42: Unknown result type (might be due to invalid IL or missing references)
		//IL_0a47: Unknown result type (might be due to invalid IL or missing references)
		//IL_0a54: Unknown result type (might be due to invalid IL or missing references)
		//IL_0a59: Unknown result type (might be due to invalid IL or missing references)
		//IL_0a62: Unknown result type (might be due to invalid IL or missing references)
		//IL_0a67: Unknown result type (might be due to invalid IL or missing references)
		//IL_0a6c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0a8f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0a94: Unknown result type (might be due to invalid IL or missing references)
		//IL_0aa1: Unknown result type (might be due to invalid IL or missing references)
		//IL_0aa6: Unknown result type (might be due to invalid IL or missing references)
		//IL_0aaf: Unknown result type (might be due to invalid IL or missing references)
		//IL_0ab4: Unknown result type (might be due to invalid IL or missing references)
		//IL_0ab9: Unknown result type (might be due to invalid IL or missing references)
		//IL_0adc: Unknown result type (might be due to invalid IL or missing references)
		//IL_0ae1: Unknown result type (might be due to invalid IL or missing references)
		//IL_0aee: Unknown result type (might be due to invalid IL or missing references)
		//IL_0af3: Unknown result type (might be due to invalid IL or missing references)
		//IL_0afc: Unknown result type (might be due to invalid IL or missing references)
		//IL_0b01: Unknown result type (might be due to invalid IL or missing references)
		//IL_0b06: Unknown result type (might be due to invalid IL or missing references)
		//IL_0b29: Unknown result type (might be due to invalid IL or missing references)
		//IL_0b2e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0b3b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0b40: Unknown result type (might be due to invalid IL or missing references)
		//IL_0b49: Unknown result type (might be due to invalid IL or missing references)
		//IL_0b4e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0b53: Unknown result type (might be due to invalid IL or missing references)
		List<double> list = new List<double>(gemCrvParamIn);
		List<double> list2 = new List<double>(baseCrvParamIn);
		Curve val = crv.DuplicateCurve();
		Curve val2 = baseCrv.DuplicateCurve();
		Interval domain;
		if (val.IsClosed && val2.IsClosed)
		{
			val.ChangeClosedCurveSeam(list[0]);
			val2.ChangeClosedCurveSeam(list2[0]);
			domain = val.Domain;
			list[0] = ((Interval)(ref domain)).Min;
			domain = val2.Domain;
			list2[0] = ((Interval)(ref domain)).Min;
			double value = default(double);
			for (int i = 1; i < shankWidthatGem.Count; i++)
			{
				Point3d val3 = crv.PointAt(list[i]);
				val.ClosestPoint(val3, ref value);
				list[i] = value;
				val3 = baseCrv.PointAt(list2[i]);
				val2.ClosestPoint(val3, ref value);
				list2[i] = value;
			}
		}
		double num = 0.0;
		foreach (double item4 in shankWidthatGem)
		{
			if (item4 > num)
			{
				num = item4;
			}
		}
		num = (num + channelWallWidth) / 2.0;
		num *= 1.5;
		List<Curve> list3 = new List<Curve>();
		List<Curve> list4 = new List<Curve>();
		List<Point3d> list5 = new List<Point3d>();
		List<Point3d> list6 = new List<Point3d>();
		Point3d item = default(Point3d);
		Point3d item2 = default(Point3d);
		for (int j = 0; j < shankWidthatGem.Count; j++)
		{
			ProfilePositionScale(val, val2, list[j], list2[j], 0.0, out var pScale, out var pRail);
			LineCurve val4 = new LineCurve(new Point3d(0.0 - num, 0.0, 0.0), new Point3d(num, 0.0, 0.0));
			((GeometryBase)val4).Transform(pRail);
			list3.Add((Curve)(object)val4);
			LineCurve val5 = new LineCurve(new Point3d(0.0 - num, 1.0, 0.0), new Point3d(num, 1.0, 0.0));
			((GeometryBase)val5).Transform(pScale);
			((GeometryBase)val5).Transform(pRail);
			list4.Add((Curve)(object)val5);
			((Point3d)(ref item))._002Ector(0.0, 0.0, 0.0);
			((Point3d)(ref item)).Transform(pRail);
			list5.Add(item);
			((Point3d)(ref item2))._002Ector(0.0, 1.0, 0.0);
			((Point3d)(ref item2)).Transform(pScale);
			((Point3d)(ref item2)).Transform(pRail);
			list6.Add(item2);
		}
		SweepTwoRail val6 = new SweepTwoRail
		{
			SweepTolerance = CSApplicationHelper.Application.Tolerance,
			AngleToleranceRadians = RhinoDoc.ActiveDoc.ModelAngleToleranceRadians,
			MaintainHeight = true,
			ClosedSweep = false
		};
		Brep[] array = val6.PerformSweep(val, val2, (IEnumerable<Curve>)list3, (IEnumerable<double>)list, (IEnumerable<double>)list2);
		Brep[] array2 = val6.PerformSweep(val, val2, (IEnumerable<Curve>)list4, (IEnumerable<double>)list, (IEnumerable<double>)list2);
		NurbsSurface val7 = null;
		if (array[0].Faces.Count > 0)
		{
			val7 = ((Surface)array[0].Faces[0]).ToNurbsSurface();
		}
		NurbsSurface val8 = null;
		if (array2[0].Faces.Count > 0)
		{
			val8 = ((Surface)array2[0].Faces[0]).ToNurbsSurface();
		}
		if (val7 == null || val8 == null)
		{
			baseCrv0 = null;
			baseCrv1 = null;
			gemCrv0 = null;
			gemCrv1 = null;
			baseCrv0Param = null;
			baseCrv1Param = null;
			gemCrv0Param = null;
			gemCrv1Param = null;
			return;
		}
		List<Point3d> list7 = new List<Point3d>();
		List<Point3d> list8 = new List<Point3d>();
		List<Point3d> list9 = new List<Point3d>();
		List<Point3d> list10 = new List<Point3d>();
		double num3 = default(double);
		double num4 = default(double);
		for (int k = 0; k < shankWidthatGem.Count; k++)
		{
			double num2 = shankWidthatGem[k];
			((Surface)val7).ClosestPoint(list5[k], ref num3, ref num4);
			list7.Add(new Point3d(num3, num4 + num2, 0.0));
			list8.Add(new Point3d(num3, num4 - num2, 0.0));
			((Surface)val8).ClosestPoint(list6[k], ref num3, ref num4);
			list9.Add(new Point3d(num3, num4 + num2, 0.0));
			list10.Add(new Point3d(num3, num4 - num2, 0.0));
		}
		List<Point3d> list11 = new List<Point3d>(list7);
		List<Point3d> list12 = new List<Point3d>(list8);
		List<Point3d> list13 = new List<Point3d>(list9);
		List<Point3d> list14 = new List<Point3d>(list10);
		Vector3d val9 = Vector3d.Unset;
		Vector3d val10 = Vector3d.Unset;
		if ((startShankProfileTangent && startShankProfileEnabled) || (startChannelProfileTangent && !startShankProfileEnabled))
		{
			val9 = Vector3d.XAxis;
		}
		Point3d val11;
		if (startChannelProfileTangent && startShankProfileEnabled)
		{
			val11 = list11[1];
			double num5 = ((Point3d)(ref val11)).X - 0.2;
			val11 = list11[1];
			list11.Insert(1, new Point3d(num5, ((Point3d)(ref val11)).Y, 0.0));
			val11 = list12[1];
			double num6 = ((Point3d)(ref val11)).X - 0.2;
			val11 = list12[1];
			list12.Insert(1, new Point3d(num6, ((Point3d)(ref val11)).Y, 0.0));
			val11 = list13[1];
			double num7 = ((Point3d)(ref val11)).X - 0.2;
			val11 = list13[1];
			list13.Insert(1, new Point3d(num7, ((Point3d)(ref val11)).Y, 0.0));
			val11 = list14[1];
			double num8 = ((Point3d)(ref val11)).X - 0.2;
			val11 = list14[1];
			list14.Insert(1, new Point3d(num8, ((Point3d)(ref val11)).Y, 0.0));
			val11 = list11[1];
			double num9 = ((Point3d)(ref val11)).X - 0.2;
			val11 = list11[1];
			list11.Insert(1, new Point3d(num9, ((Point3d)(ref val11)).Y, 0.0));
			val11 = list12[1];
			double num10 = ((Point3d)(ref val11)).X - 0.2;
			val11 = list12[1];
			list12.Insert(1, new Point3d(num10, ((Point3d)(ref val11)).Y, 0.0));
			val11 = list13[1];
			double num11 = ((Point3d)(ref val11)).X - 0.2;
			val11 = list13[1];
			list13.Insert(1, new Point3d(num11, ((Point3d)(ref val11)).Y, 0.0));
			val11 = list14[1];
			double num12 = ((Point3d)(ref val11)).X - 0.2;
			val11 = list14[1];
			list14.Insert(1, new Point3d(num12, ((Point3d)(ref val11)).Y, 0.0));
		}
		if ((endShankProfileTangent && endShankProfileEnabled) || (endChannelProfileTangent && !endShankProfileEnabled))
		{
			val10 = Vector3d.XAxis;
		}
		if (endChannelProfileTangent && endShankProfileEnabled)
		{
			int num13 = list11.Count - 2;
			int index = num13 + 1;
			val11 = list11[num13];
			double num14 = ((Point3d)(ref val11)).X + 0.2;
			val11 = list11[num13];
			list11.Insert(index, new Point3d(num14, ((Point3d)(ref val11)).Y, 0.0));
			int index2 = num13 + 1;
			val11 = list12[num13];
			double num15 = ((Point3d)(ref val11)).X + 0.2;
			val11 = list12[num13];
			list12.Insert(index2, new Point3d(num15, ((Point3d)(ref val11)).Y, 0.0));
			int index3 = num13 + 1;
			val11 = list13[num13];
			double num16 = ((Point3d)(ref val11)).X + 0.2;
			val11 = list13[num13];
			list13.Insert(index3, new Point3d(num16, ((Point3d)(ref val11)).Y, 0.0));
			int index4 = num13 + 1;
			val11 = list14[num13];
			double num17 = ((Point3d)(ref val11)).X + 0.2;
			val11 = list14[num13];
			list14.Insert(index4, new Point3d(num17, ((Point3d)(ref val11)).Y, 0.0));
			num13 = list11.Count - 2;
			int index5 = num13 + 1;
			val11 = list11[num13];
			double num18 = ((Point3d)(ref val11)).X + 0.2;
			val11 = list11[num13];
			list11.Insert(index5, new Point3d(num18, ((Point3d)(ref val11)).Y, 0.0));
			int index6 = num13 + 1;
			val11 = list12[num13];
			double num19 = ((Point3d)(ref val11)).X + 0.2;
			val11 = list12[num13];
			list12.Insert(index6, new Point3d(num19, ((Point3d)(ref val11)).Y, 0.0));
			int index7 = num13 + 1;
			val11 = list13[num13];
			double num20 = ((Point3d)(ref val11)).X + 0.2;
			val11 = list13[num13];
			list13.Insert(index7, new Point3d(num20, ((Point3d)(ref val11)).Y, 0.0));
			int index8 = num13 + 1;
			val11 = list14[num13];
			double num21 = ((Point3d)(ref val11)).X + 0.2;
			val11 = list14[num13];
			list14.Insert(index8, new Point3d(num21, ((Point3d)(ref val11)).Y, 0.0));
		}
		Curve val12 = Curve.CreateInterpolatedCurve((IEnumerable<Point3d>)list11, 3, (CurveKnotStyle)1, val9, val10);
		Curve val13 = Curve.CreateInterpolatedCurve((IEnumerable<Point3d>)list12, 3, (CurveKnotStyle)1, val9, val10);
		Curve val14 = Curve.CreateInterpolatedCurve((IEnumerable<Point3d>)list13, 3, (CurveKnotStyle)1, val9, val10);
		Curve val15 = Curve.CreateInterpolatedCurve((IEnumerable<Point3d>)list14, 3, (CurveKnotStyle)1, val9, val10);
		baseCrv0 = ((Surface)val7).Pushup(val12, 0.01);
		baseCrv1 = ((Surface)val7).Pushup(val13, 0.01);
		gemCrv0 = ((Surface)val8).Pushup(val14, 0.01);
		gemCrv1 = ((Surface)val8).Pushup(val15, 0.01);
		baseCrv0Param = new List<double>();
		baseCrv1Param = new List<double>();
		gemCrv0Param = new List<double>();
		gemCrv1Param = new List<double>();
		List<double> obj = baseCrv0Param;
		domain = baseCrv0.Domain;
		obj.Add(((Interval)(ref domain)).Min);
		List<double> obj2 = baseCrv1Param;
		domain = baseCrv1.Domain;
		obj2.Add(((Interval)(ref domain)).Min);
		List<double> obj3 = gemCrv0Param;
		domain = gemCrv0.Domain;
		obj3.Add(((Interval)(ref domain)).Min);
		List<double> obj4 = gemCrv1Param;
		domain = gemCrv1.Domain;
		obj4.Add(((Interval)(ref domain)).Min);
		double item3 = default(double);
		for (int l = 1; l < shankWidthatGem.Count; l++)
		{
			NurbsSurface obj5 = val7;
			val11 = list7[l];
			double x = ((Point3d)(ref val11)).X;
			val11 = list7[l];
			Point3d val16 = ((Surface)obj5).PointAt(x, ((Point3d)(ref val11)).Y);
			baseCrv0.ClosestPoint(val16, ref item3, 0.01);
			baseCrv0Param.Add(item3);
			NurbsSurface obj6 = val7;
			val11 = list8[l];
			double x2 = ((Point3d)(ref val11)).X;
			val11 = list8[l];
			val16 = ((Surface)obj6).PointAt(x2, ((Point3d)(ref val11)).Y);
			baseCrv1.ClosestPoint(val16, ref item3, 0.01);
			baseCrv1Param.Add(item3);
			NurbsSurface obj7 = val8;
			val11 = list9[l];
			double x3 = ((Point3d)(ref val11)).X;
			val11 = list9[l];
			val16 = ((Surface)obj7).PointAt(x3, ((Point3d)(ref val11)).Y);
			gemCrv0.ClosestPoint(val16, ref item3, 0.01);
			gemCrv0Param.Add(item3);
			NurbsSurface obj8 = val8;
			val11 = list10[l];
			double x4 = ((Point3d)(ref val11)).X;
			val11 = list10[l];
			val16 = ((Surface)obj8).PointAt(x4, ((Point3d)(ref val11)).Y);
			gemCrv1.ClosestPoint(val16, ref item3, 0.01);
			gemCrv1Param.Add(item3);
		}
	}

	private static void ProfilePositionScale(Curve gemCrv, Curve baseCrv, double gemParam, double gemBaseParam, double minCuletGrowUp, out Transform pScale, out Transform pRail)
	{
		//IL_0002: Unknown result type (might be due to invalid IL or missing references)
		//IL_0009: Unknown result type (might be due to invalid IL or missing references)
		//IL_000e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0011: Unknown result type (might be due to invalid IL or missing references)
		//IL_0016: Unknown result type (might be due to invalid IL or missing references)
		//IL_0017: Unknown result type (might be due to invalid IL or missing references)
		//IL_0018: Unknown result type (might be due to invalid IL or missing references)
		//IL_001d: Unknown result type (might be due to invalid IL or missing references)
		//IL_002b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0043: Unknown result type (might be due to invalid IL or missing references)
		//IL_0048: Unknown result type (might be due to invalid IL or missing references)
		//IL_006a: Unknown result type (might be due to invalid IL or missing references)
		//IL_006f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0074: Unknown result type (might be due to invalid IL or missing references)
		//IL_0079: Unknown result type (might be due to invalid IL or missing references)
		//IL_007a: Unknown result type (might be due to invalid IL or missing references)
		//IL_007b: Unknown result type (might be due to invalid IL or missing references)
		//IL_007c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0081: Unknown result type (might be due to invalid IL or missing references)
		//IL_008a: Unknown result type (might be due to invalid IL or missing references)
		Point3d val = gemCrv.PointAt(gemParam);
		Vector3d destination = gemCrv.TangentAt(gemParam);
		Point3d val2 = baseCrv.PointAt(gemBaseParam);
		Vector3d destination2 = val - val2;
		double num = ((Vector3d)(ref destination2)).Length + minCuletGrowUp;
		pScale = Transform.Scale(Plane.WorldXY, 1.0, num, 1.0);
		pRail = CSUtilMath.Orient3Point(new Point3d(0.0, 0.0, 0.0), Vector3d.YAxis, Vector3d.ZAxis, val2, destination2, destination);
		((Point3d)(ref val2)).Transform(pRail);
	}

	public static bool[] FindMinMax(IReadOnlyList<double> vals)
	{
		bool flag = true;
		int num = (vals.Count + 1) / 2;
		for (int i = 0; i < num; i++)
		{
			if (!Deq(vals[i], vals[vals.Count - 1 - i]))
			{
				flag = false;
				break;
			}
		}
		int num2 = (flag ? num : vals.Count);
		bool[] array = new bool[vals.Count];
		array[0] = true;
		array[num2 - 1] = true;
		for (int j = 0; j < num2 - 2; j++)
		{
			if (vals[j] > vals[j + 1] && vals[j + 1] < vals[j + 2])
			{
				array[j + 1] = true;
			}
			if (vals[j] < vals[j + 1] && vals[j + 1] > vals[j + 2])
			{
				array[j + 1] = true;
			}
		}
		for (int k = 0; k < num2 - 3; k++)
		{
			if (vals[k] > vals[k + 1] && Deq(vals[k + 1], vals[k + 2]) && vals[k + 2] < vals[k + 3])
			{
				array[k + 1] = true;
				array[k + 2] = true;
			}
			if (vals[k] < vals[k + 1] && Deq(vals[k + 1], vals[k + 2]) && vals[k + 2] > vals[k + 3])
			{
				array[k + 1] = true;
				array[k + 2] = true;
			}
		}
		for (int l = 0; l < num2 - 4; l++)
		{
			if (vals[l] > vals[l + 1] && Deq(vals[l + 1], vals[l + 2]) && Deq(vals[l + 2], vals[l + 3]) && vals[l + 3] < vals[l + 4])
			{
				array[l + 2] = true;
			}
			if (vals[l] < vals[l + 1] && Deq(vals[l + 1], vals[l + 2]) && Deq(vals[l + 2], vals[l + 3]) && vals[l + 3] > vals[l + 4])
			{
				array[l + 2] = true;
			}
		}
		for (int m = 0; m < num2 - 3; m++)
		{
			if (Deq(vals[m], vals[m + 1]) && Deq(vals[m], vals[m + 2]) && Deq(vals[m], vals[m + 3]))
			{
				array[m + 1] = true;
				array[m + 2] = true;
			}
		}
		int num3 = 0;
		for (int n = 0; n < num2; n++)
		{
			if (!array[n])
			{
				continue;
			}
			int num4 = n - num3;
			if (n - num3 > 4)
			{
				int num5 = num4 / 4;
				int num6 = num3 + (num4 - num5) / 2 - 1;
				for (int num7 = 0; num7 < num5; num7++)
				{
					if (num6 < num2 - 1)
					{
						array[num6] = true;
						num6 += 4;
					}
				}
			}
			num3 = n;
		}
		if (flag)
		{
			for (int num8 = 0; num8 < num; num8++)
			{
				array[array.Length - 1 - num8] = array[num8];
			}
		}
		return array;
	}

	private static bool Deq(double v0, double v1)
	{
		return Math.Abs(v0 - v1) < 0.001;
	}
}
