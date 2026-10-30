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
		List<double> list = new List<double>(gemCrvParamIn);
		List<double> list2 = new List<double>(baseCrvParamIn);
		Curve curve = crv.DuplicateCurve();
		Curve curve2 = baseCrv.DuplicateCurve();
		if (curve.IsClosed && curve2.IsClosed)
		{
			curve.ChangeClosedCurveSeam(list[0]);
			curve2.ChangeClosedCurveSeam(list2[0]);
			list[0] = curve.Domain.Min;
			list2[0] = curve2.Domain.Min;
			for (int i = 1; i < shankWidthatGem.Count; i++)
			{
				Point3d testPoint = crv.PointAt(list[i]);
				curve.ClosestPoint(testPoint, out var t);
				list[i] = t;
				testPoint = baseCrv.PointAt(list2[i]);
				curve2.ClosestPoint(testPoint, out t);
				list2[i] = t;
			}
		}
		double num = 0.0;
		foreach (double item3 in shankWidthatGem)
		{
			if (item3 > num)
			{
				num = item3;
			}
		}
		num = (num + channelWallWidth) / 2.0;
		num *= 1.5;
		List<Curve> list3 = new List<Curve>();
		List<Curve> list4 = new List<Curve>();
		List<Point3d> list5 = new List<Point3d>();
		List<Point3d> list6 = new List<Point3d>();
		for (int j = 0; j < shankWidthatGem.Count; j++)
		{
			ProfilePositionScale(curve, curve2, list[j], list2[j], 0.0, out var pScale, out var pRail);
			LineCurve lineCurve = new LineCurve(new Point3d(0.0 - num, 0.0, 0.0), new Point3d(num, 0.0, 0.0));
			lineCurve.Transform(pRail);
			list3.Add(lineCurve);
			LineCurve lineCurve2 = new LineCurve(new Point3d(0.0 - num, 1.0, 0.0), new Point3d(num, 1.0, 0.0));
			lineCurve2.Transform(pScale);
			lineCurve2.Transform(pRail);
			list4.Add(lineCurve2);
			Point3d item = new Point3d(0.0, 0.0, 0.0);
			item.Transform(pRail);
			list5.Add(item);
			Point3d item2 = new Point3d(0.0, 1.0, 0.0);
			item2.Transform(pScale);
			item2.Transform(pRail);
			list6.Add(item2);
		}
		SweepTwoRail obj = new SweepTwoRail
		{
			SweepTolerance = CSApplicationHelper.Application.Tolerance,
			AngleToleranceRadians = RhinoDoc.ActiveDoc.ModelAngleToleranceRadians,
			MaintainHeight = true,
			ClosedSweep = false
		};
		Brep[] array = obj.PerformSweep(curve, curve2, list3, list, list2);
		Brep[] array2 = obj.PerformSweep(curve, curve2, list4, list, list2);
		NurbsSurface nurbsSurface = null;
		if (array[0].Faces.Count > 0)
		{
			nurbsSurface = array[0].Faces[0].ToNurbsSurface();
		}
		NurbsSurface nurbsSurface2 = null;
		if (array2[0].Faces.Count > 0)
		{
			nurbsSurface2 = array2[0].Faces[0].ToNurbsSurface();
		}
		if (nurbsSurface == null || nurbsSurface2 == null)
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
		for (int k = 0; k < shankWidthatGem.Count; k++)
		{
			double num2 = shankWidthatGem[k];
			nurbsSurface.ClosestPoint(list5[k], out var u, out var v);
			list7.Add(new Point3d(u, v + num2, 0.0));
			list8.Add(new Point3d(u, v - num2, 0.0));
			nurbsSurface2.ClosestPoint(list6[k], out u, out v);
			list9.Add(new Point3d(u, v + num2, 0.0));
			list10.Add(new Point3d(u, v - num2, 0.0));
		}
		List<Point3d> list11 = new List<Point3d>(list7);
		List<Point3d> list12 = new List<Point3d>(list8);
		List<Point3d> list13 = new List<Point3d>(list9);
		List<Point3d> list14 = new List<Point3d>(list10);
		Vector3d startTangent = Vector3d.Unset;
		Vector3d endTangent = Vector3d.Unset;
		if ((startShankProfileTangent && startShankProfileEnabled) || (startChannelProfileTangent && !startShankProfileEnabled))
		{
			startTangent = Vector3d.XAxis;
		}
		if (startChannelProfileTangent && startShankProfileEnabled)
		{
			list11.Insert(1, new Point3d(list11[1].X - 0.2, list11[1].Y, 0.0));
			list12.Insert(1, new Point3d(list12[1].X - 0.2, list12[1].Y, 0.0));
			list13.Insert(1, new Point3d(list13[1].X - 0.2, list13[1].Y, 0.0));
			list14.Insert(1, new Point3d(list14[1].X - 0.2, list14[1].Y, 0.0));
			list11.Insert(1, new Point3d(list11[1].X - 0.2, list11[1].Y, 0.0));
			list12.Insert(1, new Point3d(list12[1].X - 0.2, list12[1].Y, 0.0));
			list13.Insert(1, new Point3d(list13[1].X - 0.2, list13[1].Y, 0.0));
			list14.Insert(1, new Point3d(list14[1].X - 0.2, list14[1].Y, 0.0));
		}
		if ((endShankProfileTangent && endShankProfileEnabled) || (endChannelProfileTangent && !endShankProfileEnabled))
		{
			endTangent = Vector3d.XAxis;
		}
		if (endChannelProfileTangent && endShankProfileEnabled)
		{
			int num3 = list11.Count - 2;
			list11.Insert(num3 + 1, new Point3d(list11[num3].X + 0.2, list11[num3].Y, 0.0));
			list12.Insert(num3 + 1, new Point3d(list12[num3].X + 0.2, list12[num3].Y, 0.0));
			list13.Insert(num3 + 1, new Point3d(list13[num3].X + 0.2, list13[num3].Y, 0.0));
			list14.Insert(num3 + 1, new Point3d(list14[num3].X + 0.2, list14[num3].Y, 0.0));
			num3 = list11.Count - 2;
			list11.Insert(num3 + 1, new Point3d(list11[num3].X + 0.2, list11[num3].Y, 0.0));
			list12.Insert(num3 + 1, new Point3d(list12[num3].X + 0.2, list12[num3].Y, 0.0));
			list13.Insert(num3 + 1, new Point3d(list13[num3].X + 0.2, list13[num3].Y, 0.0));
			list14.Insert(num3 + 1, new Point3d(list14[num3].X + 0.2, list14[num3].Y, 0.0));
		}
		Curve curve2d = Curve.CreateInterpolatedCurve(list11, 3, CurveKnotStyle.Chord, startTangent, endTangent);
		Curve curve2d2 = Curve.CreateInterpolatedCurve(list12, 3, CurveKnotStyle.Chord, startTangent, endTangent);
		Curve curve2d3 = Curve.CreateInterpolatedCurve(list13, 3, CurveKnotStyle.Chord, startTangent, endTangent);
		Curve curve2d4 = Curve.CreateInterpolatedCurve(list14, 3, CurveKnotStyle.Chord, startTangent, endTangent);
		baseCrv0 = nurbsSurface.Pushup(curve2d, 0.01);
		baseCrv1 = nurbsSurface.Pushup(curve2d2, 0.01);
		gemCrv0 = nurbsSurface2.Pushup(curve2d3, 0.01);
		gemCrv1 = nurbsSurface2.Pushup(curve2d4, 0.01);
		baseCrv0Param = new List<double>();
		baseCrv1Param = new List<double>();
		gemCrv0Param = new List<double>();
		gemCrv1Param = new List<double>();
		baseCrv0Param.Add(baseCrv0.Domain.Min);
		baseCrv1Param.Add(baseCrv1.Domain.Min);
		gemCrv0Param.Add(gemCrv0.Domain.Min);
		gemCrv1Param.Add(gemCrv1.Domain.Min);
		for (int l = 1; l < shankWidthatGem.Count; l++)
		{
			Point3d testPoint2 = nurbsSurface.PointAt(list7[l].X, list7[l].Y);
			baseCrv0.ClosestPoint(testPoint2, out var t2, 0.01);
			baseCrv0Param.Add(t2);
			testPoint2 = nurbsSurface.PointAt(list8[l].X, list8[l].Y);
			baseCrv1.ClosestPoint(testPoint2, out t2, 0.01);
			baseCrv1Param.Add(t2);
			testPoint2 = nurbsSurface2.PointAt(list9[l].X, list9[l].Y);
			gemCrv0.ClosestPoint(testPoint2, out t2, 0.01);
			gemCrv0Param.Add(t2);
			testPoint2 = nurbsSurface2.PointAt(list10[l].X, list10[l].Y);
			gemCrv1.ClosestPoint(testPoint2, out t2, 0.01);
			gemCrv1Param.Add(t2);
		}
	}

	private static void ProfilePositionScale(Curve gemCrv, Curve baseCrv, double gemParam, double gemBaseParam, double minCuletGrowUp, out Transform pScale, out Transform pRail)
	{
		Point3d point3d = gemCrv.PointAt(gemParam);
		Vector3d destination = gemCrv.TangentAt(gemParam);
		Point3d point3d2 = baseCrv.PointAt(gemBaseParam);
		Vector3d destination2 = point3d - point3d2;
		double yScaleFactor = destination2.Length + minCuletGrowUp;
		pScale = Transform.Scale(Plane.WorldXY, 1.0, yScaleFactor, 1.0);
		pRail = CSUtilMath.Orient3Point(new Point3d(0.0, 0.0, 0.0), Vector3d.YAxis, Vector3d.ZAxis, point3d2, destination2, destination);
		point3d2.Transform(pRail);
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
