using System;
using System.Collections.Generic;
using CSCompatibility;
using RMA.OpenNURBS;
using Rhino.Geometry;
using Rhino.Runtime;

namespace CSBlockUtil;

public static class CSUtilCurves
{
	public static OnNurbsCurve BlendCurve(IOnCurve crv1, IOnCurve crv2, bool doBlendFromStart1 = false, bool doBlendFromStart2 = true, double blendAmount = 1.0, double blendStart = 1.0, double blendEnd = 1.0, bool doClean = false)
	{
		GetBlendPoints(crv1, crv2, doBlendFromStart1, doBlendFromStart2, out var pointStart, out var pointEnd, out var vectorStart, out var vectorEnd);
		return BlendCurve((IOn3dPoint)(object)pointStart, (IOn3dPoint)(object)pointEnd, (IOn3dVector)(object)vectorStart, (IOn3dVector)(object)vectorEnd, blendAmount, blendStart, blendEnd, doClean);
	}

	public static OnNurbsCurve BlendCurve(IOn3dPoint pointStart, IOn3dPoint pointEnd, IOn3dVector vectorStart, IOn3dVector vectorEnd, double blendAmount = 1.0, double blendStart = 1.0, double blendEnd = 1.0, bool doClean = false)
	{
		//IL_0008: Unknown result type (might be due to invalid IL or missing references)
		//IL_000e: Expected O, but got Unknown
		//IL_002d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0033: Unknown result type (might be due to invalid IL or missing references)
		//IL_003e: Expected O, but got Unknown
		//IL_005f: Expected O, but got Unknown
		//IL_0066: Unknown result type (might be due to invalid IL or missing references)
		//IL_006c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0077: Expected O, but got Unknown
		//IL_0098: Expected O, but got Unknown
		//IL_0127: Unknown result type (might be due to invalid IL or missing references)
		//IL_0131: Expected O, but got Unknown
		//IL_0132: Unknown result type (might be due to invalid IL or missing references)
		//IL_0137: Unknown result type (might be due to invalid IL or missing references)
		//IL_013d: Expected O, but got Unknown
		//IL_00b5: Unknown result type (might be due to invalid IL or missing references)
		//IL_00bb: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c6: Expected O, but got Unknown
		//IL_00e7: Expected O, but got Unknown
		//IL_00ee: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f4: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ff: Expected O, but got Unknown
		//IL_0120: Expected O, but got Unknown
		double num = pointStart.DistanceTo(pointEnd);
		On3dPointArray val = new On3dPointArray();
		((ArrayOn3dPoint)val).Append(pointStart);
		if (DoAppendPoint(doClean, blendAmount, blendStart))
		{
			((ArrayOn3dPoint)val).Append((IOn3dPoint)(object)(new On3dPoint(pointStart) + (IOn3dVector)(object)(new On3dVector(vectorStart) * num * 0.2 * blendAmount * blendStart)));
			((ArrayOn3dPoint)val).Append((IOn3dPoint)(object)(new On3dPoint(pointStart) + (IOn3dVector)(object)(new On3dVector(vectorStart) * num * 0.4 * blendAmount * blendStart)));
		}
		if (DoAppendPoint(doClean, blendAmount, blendEnd))
		{
			((ArrayOn3dPoint)val).Append((IOn3dPoint)(object)(new On3dPoint(pointEnd) + (IOn3dVector)(object)(new On3dVector(vectorEnd) * num * 0.4 * blendAmount * blendEnd)));
			((ArrayOn3dPoint)val).Append((IOn3dPoint)(object)(new On3dPoint(pointEnd) + (IOn3dVector)(object)(new On3dVector(vectorEnd) * num * 0.2 * blendAmount * blendEnd)));
		}
		((ArrayOn3dPoint)val).Append((IOn3dPoint)new On3dPoint(pointEnd));
		OnBezierCurve val2 = new OnBezierCurve((IOn3dPointArray)(object)val);
		OnNurbsCurve result = new OnNurbsCurve();
		val2.GetNurbForm(ref result);
		return result;
	}

	public static NurbsCurve BlendCurve(Curve crv1, Curve crv2, bool doBlendFromStart1 = false, bool doBlendFromStart2 = true, double blendAmount = 1.0, double blendStart = 1.0, double blendEnd = 1.0, bool doClean = false)
	{
		GetBlendPoints(crv1, crv2, doBlendFromStart1, doBlendFromStart2, out var pointStart, out var pointEnd, out var vectorStart, out var vectorEnd);
		return BlendCurve(pointStart, pointEnd, vectorStart, vectorEnd, blendAmount, blendStart, blendEnd, doClean);
	}

	public static NurbsCurve BlendCurve(Point3d pointStart, Point3d pointEnd, Vector3d vectorStart, Vector3d vectorEnd, double blendAmount = 1.0, double blendStart = 1.0, double blendEnd = 1.0, bool doClean = false)
	{
		double num = pointStart.DistanceTo(pointEnd);
		List<Point3d> list = new List<Point3d>();
		list.Add(pointStart);
		if (!doClean || (blendAmount > 0.0 && blendStart > 0.0))
		{
			list.Add(pointStart + vectorStart * num * 0.2 * blendAmount * blendStart);
			list.Add(pointStart + vectorStart * num * 0.4 * blendAmount * blendStart);
		}
		if (!doClean || (blendAmount > 0.0 && blendEnd > 0.0))
		{
			list.Add(pointEnd + vectorEnd * num * 0.4 * blendAmount * blendEnd);
			list.Add(pointEnd + vectorEnd * num * 0.2 * blendAmount * blendEnd);
		}
		list.Add(pointEnd);
		return new BezierCurve(list).ToNurbsCurve();
	}

	public static OnNurbsCurve BlendCurveAdvanced(IOnCurve crv1, IOnCurve crv2, bool doBlendFromStart1 = false, bool doBlendFromStart2 = true, int g1 = 1, int g2 = 1, double blendStartA = 1.0, double blendStartB = 1.0, double blendStartC = 1.0, double blendEndA = 1.0, double blendEndB = 1.0, double blendEndC = 1.0, bool doClean = false)
	{
		GetBlendPoints(crv1, crv2, doBlendFromStart1, doBlendFromStart2, out var pointStart, out var pointEnd, out var vectorStart, out var vectorEnd);
		return BlendCurveAdvanced((IOn3dPoint)(object)pointStart, (IOn3dPoint)(object)pointEnd, (IOn3dVector)(object)vectorStart, (IOn3dVector)(object)vectorEnd, g1, g2, blendStartA, blendStartB, blendStartC, blendEndA, blendEndB, blendEndC, doClean);
	}

	public static OnNurbsCurve BlendCurveAdvanced(IOn3dPoint pointStart, IOn3dPoint pointEnd, IOn3dVector vectorStart, IOn3dVector vectorEnd, int g1 = 1, int g2 = 1, double blendStartA = 1.0, double blendStartB = 1.0, double blendStartC = 1.0, double blendEndA = 1.0, double blendEndB = 1.0, double blendEndC = 1.0, bool doClean = false)
	{
		//IL_0012: Unknown result type (might be due to invalid IL or missing references)
		//IL_0018: Expected O, but got Unknown
		//IL_001a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0024: Expected O, but got Unknown
		//IL_0048: Unknown result type (might be due to invalid IL or missing references)
		//IL_004e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0059: Expected O, but got Unknown
		//IL_0065: Expected O, but got Unknown
		//IL_008e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0094: Unknown result type (might be due to invalid IL or missing references)
		//IL_009f: Expected O, but got Unknown
		//IL_00ae: Expected O, but got Unknown
		//IL_00d7: Unknown result type (might be due to invalid IL or missing references)
		//IL_00dd: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e8: Expected O, but got Unknown
		//IL_00fa: Expected O, but got Unknown
		//IL_0123: Unknown result type (might be due to invalid IL or missing references)
		//IL_0129: Unknown result type (might be due to invalid IL or missing references)
		//IL_0134: Expected O, but got Unknown
		//IL_0146: Expected O, but got Unknown
		//IL_01dc: Unknown result type (might be due to invalid IL or missing references)
		//IL_01e6: Expected O, but got Unknown
		//IL_01e7: Unknown result type (might be due to invalid IL or missing references)
		//IL_01ec: Unknown result type (might be due to invalid IL or missing references)
		//IL_01f2: Expected O, but got Unknown
		//IL_016f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0175: Unknown result type (might be due to invalid IL or missing references)
		//IL_0180: Expected O, but got Unknown
		//IL_018f: Expected O, but got Unknown
		//IL_01b8: Unknown result type (might be due to invalid IL or missing references)
		//IL_01be: Unknown result type (might be due to invalid IL or missing references)
		//IL_01c9: Expected O, but got Unknown
		//IL_01d5: Expected O, but got Unknown
		double num = pointStart.DistanceTo(pointEnd) / 5.0;
		On3dPointArray val = new On3dPointArray();
		((ArrayOn3dPoint)val).Append((IOn3dPoint)new On3dPoint(pointStart));
		if (g1 >= 1 && DoAppendPoint(doClean, blendStartA))
		{
			((ArrayOn3dPoint)val).Append((IOn3dPoint)(object)(new On3dPoint(pointStart) + (IOn3dVector)(object)(new On3dVector(vectorStart) * num * blendStartA)));
		}
		if (g1 >= 2 && DoAppendPoint(doClean, blendStartB))
		{
			((ArrayOn3dPoint)val).Append((IOn3dPoint)(object)(new On3dPoint(pointStart) + (IOn3dVector)(object)(new On3dVector(vectorStart) * num * (blendStartA + blendStartB))));
		}
		if (g1 >= 3 && DoAppendPoint(doClean, blendStartC))
		{
			((ArrayOn3dPoint)val).Append((IOn3dPoint)(object)(new On3dPoint(pointStart) + (IOn3dVector)(object)(new On3dVector(vectorStart) * num * (blendStartA + blendStartB + blendStartC))));
		}
		if (g2 >= 3 && DoAppendPoint(doClean, blendEndC))
		{
			((ArrayOn3dPoint)val).Append((IOn3dPoint)(object)(new On3dPoint(pointEnd) + (IOn3dVector)(object)(new On3dVector(vectorEnd) * num * (blendEndA + blendEndB + blendEndC))));
		}
		if (g2 >= 2 && DoAppendPoint(doClean, blendEndB))
		{
			((ArrayOn3dPoint)val).Append((IOn3dPoint)(object)(new On3dPoint(pointEnd) + (IOn3dVector)(object)(new On3dVector(vectorEnd) * num * (blendEndA + blendEndB))));
		}
		if (g2 >= 1 && DoAppendPoint(doClean, blendEndA))
		{
			((ArrayOn3dPoint)val).Append((IOn3dPoint)(object)(new On3dPoint(pointEnd) + (IOn3dVector)(object)(new On3dVector(vectorEnd) * num * blendEndA)));
		}
		((ArrayOn3dPoint)val).Append((IOn3dPoint)new On3dPoint(pointEnd));
		OnBezierCurve val2 = new OnBezierCurve((IOn3dPointArray)(object)val);
		OnNurbsCurve result = new OnNurbsCurve();
		val2.GetNurbForm(ref result);
		return result;
	}

	public static NurbsCurve BlendCurveAdvanced(Curve crv1, Curve crv2, bool doBlendFromStart1 = false, bool doBlendFromStart2 = true, int g1 = 3, int g2 = 3, double blendStartA = 1.0, double blendStartB = 1.0, double blendStartC = 1.0, double blendEndA = 1.0, double blendEndB = 1.0, double blendEndC = 1.0, bool doClean = false)
	{
		GetBlendPoints(crv1, crv2, doBlendFromStart1, doBlendFromStart2, out var pointStart, out var pointEnd, out var vectorStart, out var vectorEnd);
		return BlendCurveAdvanced(pointStart, pointEnd, vectorStart, vectorEnd, g1, g2, blendStartA, blendStartB, blendStartC, blendEndA, blendEndB, blendEndC, doClean);
	}

	public static NurbsCurve BlendCurveAdvanced(Point3d pointStart, Point3d pointEnd, Vector3d vectorStart, Vector3d vectorEnd, int g1 = 3, int g2 = 3, double blendStartA = 1.0, double blendStartB = 1.0, double blendStartC = 1.0, double blendEndA = 1.0, double blendEndB = 1.0, double blendEndC = 1.0, bool doClean = false)
	{
		double num = pointStart.DistanceTo(pointEnd) / 5.0;
		List<Point3d> list = new List<Point3d>();
		list.Add(pointStart);
		if (g1 >= 1 && DoAppendPoint(doClean, blendStartA))
		{
			list.Add(pointStart + vectorStart * num * blendStartA);
		}
		if (g1 >= 2 && DoAppendPoint(doClean, blendStartB))
		{
			list.Add(pointStart + vectorStart * num * (blendStartA + blendStartB));
		}
		if (g1 >= 3 && DoAppendPoint(doClean, blendStartC))
		{
			list.Add(pointStart + vectorStart * num * (blendStartA + blendStartB + blendStartC));
		}
		if (g2 >= 3 && DoAppendPoint(doClean, blendEndC))
		{
			list.Add(pointEnd + vectorEnd * num * (blendEndA + blendEndB + blendEndC));
		}
		if (g2 >= 2 && DoAppendPoint(doClean, blendEndB))
		{
			list.Add(pointEnd + vectorEnd * num * (blendEndA + blendEndB));
		}
		if (g2 >= 1 && DoAppendPoint(doClean, blendEndA))
		{
			list.Add(pointEnd + vectorEnd * num * blendEndA);
		}
		list.Add(pointEnd);
		return new BezierCurve(list).ToNurbsCurve();
	}

	public static bool BlendCurveCorners(OnCurve crv, double minAngleRadians, double maxAngleRadians, double blendSize, double blendAmount, ref OnCurve outputCurve)
	{
		//IL_0005: Unknown result type (might be due to invalid IL or missing references)
		//IL_000b: Expected O, but got Unknown
		//IL_02fc: Unknown result type (might be due to invalid IL or missing references)
		//IL_0306: Expected O, but got Unknown
		//IL_0331: Unknown result type (might be due to invalid IL or missing references)
		//IL_033b: Expected O, but got Unknown
		//IL_0290: Unknown result type (might be due to invalid IL or missing references)
		//IL_029a: Expected O, but got Unknown
		//IL_026f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0279: Expected O, but got Unknown
		//IL_03a5: Unknown result type (might be due to invalid IL or missing references)
		//IL_03af: Expected O, but got Unknown
		//IL_065a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0664: Expected O, but got Unknown
		if (crv == null)
		{
			return false;
		}
		OnNurbsCurve val = new OnNurbsCurve();
		crv.GetNurbForm(ref val);
		if (!((OnObject)val).IsValid())
		{
			return false;
		}
		List<double> list = new List<double>();
		double num = ((OnCurve)val).Domain().Min();
		double num2 = ((OnCurve)val).Domain().Max();
		int num3 = 0;
		list.Add(num);
		while (true)
		{
			double num4 = 0.0;
			if (!((OnCurve)val).GetNextDiscontinuity((continuity)2, num, num2, ref num4, ref num3))
			{
				break;
			}
			num = num4;
			list.Add(num4);
		}
		for (int i = 0; i < list.Count - 2; i++)
		{
			if (Math.Abs(list[i] - list[i + 1]) < 1E-05)
			{
				list.RemoveAt(i + 1);
				i--;
			}
		}
		if (list.Count > 2 && Math.Abs(list[list.Count - 2] - list[list.Count - 1]) < 1E-05)
		{
			list.RemoveAt(list.Count - 2);
		}
		for (int j = 0; j < list.Count; j++)
		{
			if (list.Count == 1)
			{
				break;
			}
			double num5 = list[j];
			double num6 = num5 - ((OnCurve)val).Domain().Length() / 2000.0;
			double num7 = num5 + ((OnCurve)val).Domain().Length() / 2000.0;
			if (num7 > ((OnCurve)val).Domain().Max())
			{
				num7 -= ((OnCurve)val).Domain().Length();
			}
			if (num6 < ((OnCurve)val).Domain().Min())
			{
				num6 += ((OnCurve)val).Domain().Length();
			}
			On3dVector obj = CSUtilGeometry.TangentAtForce(val, num6);
			On3dVector val2 = CSUtilGeometry.TangentAtForce(val, num7);
			obj.Unitize();
			val2.Unitize();
			double num8 = Math.Acos(obj * (IOn3dVector)(object)val2);
			if (num8 < minAngleRadians || num8 > maxAngleRadians)
			{
				list.RemoveAt(j);
				j--;
			}
		}
		if (list.Count < 1)
		{
			outputCurve = crv.Duplicate();
			return true;
		}
		List<OnNurbsCurve> list2 = new List<OnNurbsCurve>();
		if (((OnCurve)val).IsClosed())
		{
			for (int k = 0; k < list.Count; k++)
			{
				OnNurbsCurve val3 = val.Duplicate();
				On3dPoint val4 = ((OnCurve)val3).PointAt(list[(k + 1) % list.Count]);
				if (((OnCurve)val3).ChangeClosedCurveSeam(list[k]))
				{
					double num9 = ((OnCurve)val3).Domain().Min();
					double num10 = ((OnCurve)val3).Domain().Max();
					((OnCurve)val3).GetClosestPoint((IOn3dPoint)(object)val4, ref num10);
					((OnCurve)val3).Trim((IOnInterval)new OnInterval(num9, num10));
				}
				else
				{
					((OnCurve)val3).Trim((IOnInterval)new OnInterval(list[k], list[k + 1]));
				}
				list2.Add(val3);
			}
		}
		else
		{
			if (Math.Abs(list[0] - ((OnCurve)val).Domain().Min()) > 0.0001)
			{
				OnNurbsCurve val5 = val.Duplicate();
				((OnCurve)val5).Trim((IOnInterval)new OnInterval(((OnCurve)val5).Domain().Min(), list[0]));
				list2.Add(val5);
			}
			for (int l = 0; l < list.Count - 1; l++)
			{
				OnNurbsCurve val6 = val.Duplicate();
				((OnCurve)val6).Trim((IOnInterval)new OnInterval(list[l], list[l + 1]));
				list2.Add(val6);
			}
			if (Math.Abs(list[list.Count - 1] - ((OnCurve)val).Domain().Max()) > 0.0001)
			{
				OnNurbsCurve val7 = val.Duplicate();
				((OnCurve)val7).Trim((IOnInterval)new OnInterval(list[list.Count - 1], ((OnCurve)val7).Domain().Max()));
				list2.Add(val7);
			}
		}
		List<double> list3 = new List<double>();
		List<double> list4 = new List<double>();
		List<double> list5 = new List<double>();
		foreach (OnNurbsCurve item3 in list2)
		{
			double item = 0.0;
			((OnCurve)item3).GetLength(ref item);
			list3.Add(item);
			list4.Add(0.0);
			list5.Add(0.0);
		}
		int num11 = list2.Count;
		if (!((OnCurve)val).IsClosed())
		{
			num11--;
		}
		for (int m = 0; m < num11; m++)
		{
			OnNurbsCurve val8 = list2[m];
			int index = (m + 1) % list2.Count;
			OnNurbsCurve val9 = list2[index];
			if (val8 == null || val9 == null)
			{
				continue;
			}
			On3dVector val10 = CSUtilGeometry.TangentAtForce(val8, ((OnCurve)val8).Domain().Max());
			On3dVector val11 = CSUtilGeometry.TangentAtForce(val9, ((OnCurve)val9).Domain().Min());
			if (!(val10 == (IOn3dVector)null) && !(val11 == (IOn3dVector)null))
			{
				val10.Unitize();
				val11.Unitize();
				double num12 = Math.Acos(val10 * (IOn3dVector)(object)val11);
				if (num12 > minAngleRadians && num12 < maxAngleRadians)
				{
					list5[m] = blendSize;
					list4[index] = blendSize;
				}
			}
		}
		for (int n = 0; n < list2.Count; n++)
		{
			OnNurbsCurve val12 = list2[n];
			if (!(list3[n] < 0.01))
			{
				double num13 = list3[n] * 0.9;
				double num14 = list4[n] + list5[n];
				if (num14 > num13)
				{
					double num15 = list4[n] / num14;
					double num16 = list5[n] / num14;
					list4[n] = num15 * num13;
					list5[n] = num16 * num13;
				}
				double num17 = ((OnCurve)val12).Domain().Min();
				double num18 = ((OnCurve)val12).Domain().Max();
				if (list4[n] > 0.01)
				{
					double num19 = ((OnCurve)val12).Domain().Min();
					double num20 = list4[n] / list3[n];
					((OnCurve)val12).GetNormalizedArcLengthPoint(num20, ref num19);
					num17 = num19;
				}
				if (list5[n] > 0.01)
				{
					double num21 = ((OnCurve)val12).Domain().Max();
					double num22 = 1.0 - list5[n] / list3[n];
					((OnCurve)val12).GetNormalizedArcLengthPoint(num22, ref num21);
					num18 = num21;
				}
				((OnCurve)val12).Trim((IOnInterval)new OnInterval(num17, num18));
				val12.MakeDeformable();
			}
		}
		List<OnNurbsCurve> list6 = new List<OnNurbsCurve>();
		for (int num23 = 0; num23 < num11; num23++)
		{
			OnNurbsCurve val13 = list2[num23];
			int index2 = (num23 + 1) % list2.Count;
			OnNurbsCurve val14 = list2[index2];
			On3dVector val15 = ((OnCurve)val13).TangentAt(((OnCurve)val13).Domain().Max());
			On3dVector val16 = ((OnCurve)val14).TangentAt(((OnCurve)val14).Domain().Min());
			val16.Reverse();
			val15.Unitize();
			val16.Unitize();
			OnNurbsCurve item2 = null;
			if (((OnCurve)val14).PointAtEnd().DistanceTo((IOn3dPoint)(object)((OnCurve)val13).PointAtStart()) > 0.01)
			{
				item2 = BlendCurve((IOn3dPoint)(object)((OnCurve)val13).PointAtEnd(), (IOn3dPoint)(object)((OnCurve)val14).PointAtStart(), (IOn3dVector)(object)val15, (IOn3dVector)(object)val16, blendAmount);
			}
			list6.Add(item2);
		}
		List<OnNurbsCurve> list7 = new List<OnNurbsCurve>();
		for (int num24 = 0; num24 < list2.Count; num24++)
		{
			list7.Add(list2[num24]);
			if (list6.Count > num24 && list6[num24] != null)
			{
				list7.Add(list6[num24]);
			}
		}
		IOnCurve[] inputCrvs = (IOnCurve[])(object)list7.ToArray();
		if (MergeCurves(inputCrvs, out OnCurve mergedCrv, 0.01) && mergedCrv != null)
		{
			if (mergedCrv.IsClosed())
			{
				double num25 = mergedCrv.Domain().Min();
				if (mergedCrv.GetClosestPoint((IOn3dPoint)(object)crv.PointAtStart(), ref num25))
				{
					mergedCrv.ChangeClosedCurveSeam(num25);
				}
			}
			outputCurve = mergedCrv;
			return true;
		}
		return false;
	}

	public static bool BlendCurveCorners(Curve incrv, double minAngleRadians, double maxAngleRadians, double blendSize, double blendAmount, ref Curve outputCurve)
	{
		//IL_000b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0011: Expected O, but got Unknown
		//IL_0011: Unknown result type (might be due to invalid IL or missing references)
		//IL_0017: Expected O, but got Unknown
		//IL_030f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0319: Expected O, but got Unknown
		//IL_0344: Unknown result type (might be due to invalid IL or missing references)
		//IL_034e: Expected O, but got Unknown
		//IL_02a3: Unknown result type (might be due to invalid IL or missing references)
		//IL_02ad: Expected O, but got Unknown
		//IL_0282: Unknown result type (might be due to invalid IL or missing references)
		//IL_028c: Expected O, but got Unknown
		//IL_03b8: Unknown result type (might be due to invalid IL or missing references)
		//IL_03c2: Expected O, but got Unknown
		//IL_066d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0677: Expected O, but got Unknown
		if (incrv == null)
		{
			return false;
		}
		OnCurve val = (OnCurve)Interop.ToOnCurve(incrv);
		OnNurbsCurve val2 = new OnNurbsCurve();
		val.GetNurbForm(ref val2);
		if (!((OnObject)val2).IsValid())
		{
			return false;
		}
		List<double> list = new List<double>();
		double num = ((OnCurve)val2).Domain().Min();
		double num2 = ((OnCurve)val2).Domain().Max();
		int num3 = 0;
		list.Add(num);
		while (true)
		{
			double num4 = 0.0;
			if (!((OnCurve)val2).GetNextDiscontinuity((continuity)2, num, num2, ref num4, ref num3))
			{
				break;
			}
			num = num4;
			list.Add(num4);
		}
		for (int i = 0; i < list.Count - 2; i++)
		{
			if (Math.Abs(list[i] - list[i + 1]) < 1E-05)
			{
				list.RemoveAt(i + 1);
				i--;
			}
		}
		if (list.Count > 2 && Math.Abs(list[list.Count - 2] - list[list.Count - 1]) < 1E-05)
		{
			list.RemoveAt(list.Count - 2);
		}
		for (int j = 0; j < list.Count; j++)
		{
			if (list.Count == 1)
			{
				break;
			}
			double num5 = list[j];
			double num6 = num5 - ((OnCurve)val2).Domain().Length() / 2000.0;
			double num7 = num5 + ((OnCurve)val2).Domain().Length() / 2000.0;
			if (num7 > ((OnCurve)val2).Domain().Max())
			{
				num7 -= ((OnCurve)val2).Domain().Length();
			}
			if (num6 < ((OnCurve)val2).Domain().Min())
			{
				num6 += ((OnCurve)val2).Domain().Length();
			}
			On3dVector obj = CSUtilGeometry.TangentAtForce(val2, num6);
			On3dVector val3 = CSUtilGeometry.TangentAtForce(val2, num7);
			obj.Unitize();
			val3.Unitize();
			double num8 = Math.Acos(obj * (IOn3dVector)(object)val3);
			if (num8 < minAngleRadians || num8 > maxAngleRadians)
			{
				list.RemoveAt(j);
				j--;
			}
		}
		if (list.Count < 1)
		{
			outputCurve = Interop.FromOnCurve(val.Duplicate());
			return true;
		}
		List<OnNurbsCurve> list2 = new List<OnNurbsCurve>();
		if (((OnCurve)val2).IsClosed())
		{
			for (int k = 0; k < list.Count; k++)
			{
				OnNurbsCurve val4 = val2.Duplicate();
				On3dPoint val5 = ((OnCurve)val4).PointAt(list[(k + 1) % list.Count]);
				if (((OnCurve)val4).ChangeClosedCurveSeam(list[k]))
				{
					double num9 = ((OnCurve)val4).Domain().Min();
					double num10 = ((OnCurve)val4).Domain().Max();
					((OnCurve)val4).GetClosestPoint((IOn3dPoint)(object)val5, ref num10);
					((OnCurve)val4).Trim((IOnInterval)new OnInterval(num9, num10));
				}
				else
				{
					((OnCurve)val4).Trim((IOnInterval)new OnInterval(list[k], list[k + 1]));
				}
				list2.Add(val4);
			}
		}
		else
		{
			if (Math.Abs(list[0] - ((OnCurve)val2).Domain().Min()) > 0.0001)
			{
				OnNurbsCurve val6 = val2.Duplicate();
				((OnCurve)val6).Trim((IOnInterval)new OnInterval(((OnCurve)val6).Domain().Min(), list[0]));
				list2.Add(val6);
			}
			for (int l = 0; l < list.Count - 1; l++)
			{
				OnNurbsCurve val7 = val2.Duplicate();
				((OnCurve)val7).Trim((IOnInterval)new OnInterval(list[l], list[l + 1]));
				list2.Add(val7);
			}
			if (Math.Abs(list[list.Count - 1] - ((OnCurve)val2).Domain().Max()) > 0.0001)
			{
				OnNurbsCurve val8 = val2.Duplicate();
				((OnCurve)val8).Trim((IOnInterval)new OnInterval(list[list.Count - 1], ((OnCurve)val8).Domain().Max()));
				list2.Add(val8);
			}
		}
		List<double> list3 = new List<double>();
		List<double> list4 = new List<double>();
		List<double> list5 = new List<double>();
		foreach (OnNurbsCurve item3 in list2)
		{
			double item = 0.0;
			((OnCurve)item3).GetLength(ref item);
			list3.Add(item);
			list4.Add(0.0);
			list5.Add(0.0);
		}
		int num11 = list2.Count;
		if (!((OnCurve)val2).IsClosed())
		{
			num11--;
		}
		for (int m = 0; m < num11; m++)
		{
			OnNurbsCurve val9 = list2[m];
			int index = (m + 1) % list2.Count;
			OnNurbsCurve val10 = list2[index];
			if (val9 == null || val10 == null)
			{
				continue;
			}
			On3dVector val11 = CSUtilGeometry.TangentAtForce(val9, ((OnCurve)val9).Domain().Max());
			On3dVector val12 = CSUtilGeometry.TangentAtForce(val10, ((OnCurve)val10).Domain().Min());
			if (!(val11 == (IOn3dVector)null) && !(val12 == (IOn3dVector)null))
			{
				val11.Unitize();
				val12.Unitize();
				double num12 = Math.Acos(val11 * (IOn3dVector)(object)val12);
				if (num12 > minAngleRadians && num12 < maxAngleRadians)
				{
					list5[m] = blendSize;
					list4[index] = blendSize;
				}
			}
		}
		for (int n = 0; n < list2.Count; n++)
		{
			OnNurbsCurve val13 = list2[n];
			if (!(list3[n] < 0.01))
			{
				double num13 = list3[n] * 0.9;
				double num14 = list4[n] + list5[n];
				if (num14 > num13)
				{
					double num15 = list4[n] / num14;
					double num16 = list5[n] / num14;
					list4[n] = num15 * num13;
					list5[n] = num16 * num13;
				}
				double num17 = ((OnCurve)val13).Domain().Min();
				double num18 = ((OnCurve)val13).Domain().Max();
				if (list4[n] > 0.01)
				{
					double num19 = ((OnCurve)val13).Domain().Min();
					double num20 = list4[n] / list3[n];
					((OnCurve)val13).GetNormalizedArcLengthPoint(num20, ref num19);
					num17 = num19;
				}
				if (list5[n] > 0.01)
				{
					double num21 = ((OnCurve)val13).Domain().Max();
					double num22 = 1.0 - list5[n] / list3[n];
					((OnCurve)val13).GetNormalizedArcLengthPoint(num22, ref num21);
					num18 = num21;
				}
				((OnCurve)val13).Trim((IOnInterval)new OnInterval(num17, num18));
				val13.MakeDeformable();
			}
		}
		List<OnNurbsCurve> list6 = new List<OnNurbsCurve>();
		for (int num23 = 0; num23 < num11; num23++)
		{
			OnNurbsCurve val14 = list2[num23];
			int index2 = (num23 + 1) % list2.Count;
			OnNurbsCurve val15 = list2[index2];
			On3dVector val16 = ((OnCurve)val14).TangentAt(((OnCurve)val14).Domain().Max());
			On3dVector val17 = ((OnCurve)val15).TangentAt(((OnCurve)val15).Domain().Min());
			val17.Reverse();
			val16.Unitize();
			val17.Unitize();
			OnNurbsCurve item2 = null;
			if (((OnCurve)val15).PointAtEnd().DistanceTo((IOn3dPoint)(object)((OnCurve)val14).PointAtStart()) > 0.01)
			{
				item2 = BlendCurve((IOn3dPoint)(object)((OnCurve)val14).PointAtEnd(), (IOn3dPoint)(object)((OnCurve)val15).PointAtStart(), (IOn3dVector)(object)val16, (IOn3dVector)(object)val17, blendAmount);
			}
			list6.Add(item2);
		}
		List<OnNurbsCurve> list7 = new List<OnNurbsCurve>();
		for (int num24 = 0; num24 < list2.Count; num24++)
		{
			list7.Add(list2[num24]);
			if (list6.Count > num24 && list6[num24] != null)
			{
				list7.Add(list6[num24]);
			}
		}
		IOnCurve[] inputCrvs = (IOnCurve[])(object)list7.ToArray();
		if (MergeCurves(inputCrvs, out OnCurve mergedCrv, 0.01) && mergedCrv != null)
		{
			if (mergedCrv.IsClosed())
			{
				double num25 = mergedCrv.Domain().Min();
				if (mergedCrv.GetClosestPoint((IOn3dPoint)(object)val.PointAtStart(), ref num25))
				{
					mergedCrv.ChangeClosedCurveSeam(num25);
				}
			}
			outputCurve = Interop.FromOnCurve(mergedCrv);
			return true;
		}
		return false;
	}

	public static bool ChangeClosedCurveSeam(ref Curve crv, ref double[] crvParams, double tSeam)
	{
		if (crv == null)
		{
			return false;
		}
		if (!crv.IsValid)
		{
			return false;
		}
		if (!crv.IsClosed)
		{
			return true;
		}
		List<Point3d> list = new List<Point3d>();
		double[] array = crvParams;
		foreach (double t in array)
		{
			Point3d item = crv.PointAt(t);
			if (!item.IsValid)
			{
				return false;
			}
			list.Add(item);
		}
		if (list.Count != crvParams.Length)
		{
			return false;
		}
		if (!crv.ChangeClosedCurveSeam(tSeam))
		{
			return false;
		}
		List<double> list2 = new List<double>();
		foreach (Point3d item2 in list)
		{
			if (!crv.ClosestPoint(item2, out var t2))
			{
				return false;
			}
			list2.Add(t2);
		}
		if (list2.Count != crvParams.Length)
		{
			return false;
		}
		crvParams = list2.ToArray();
		return true;
	}

	public static bool CurveFrom2ViewSimple(IOnCurve profile, IOn3dVector profileNormal, IOnSurface srf, out OnCurve crv, out string msg)
	{
		//IL_000c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0012: Expected O, but got Unknown
		//IL_004c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0052: Expected O, but got Unknown
		crv = null;
		msg = string.Empty;
		On3dVector val = new On3dVector(profileNormal);
		OnCurve val2 = profile.DuplicateCurve();
		((OnGeometry)val2).Translate((IOn3dVector)(object)(val * -10.0));
		OnCurve val3 = profile.DuplicateCurve();
		((OnGeometry)val3).Translate((IOn3dVector)(object)(val * 10.0));
		OnNurbsSurface val4 = new OnNurbsSurface();
		if (val4.CreateRuledSurface((IOnCurve)(object)val2, (IOnCurve)(object)val3) <= 0)
		{
			msg = "CreateRuledSurface failed";
			return false;
		}
		if (!RhUtilHelper.RhinoIntersectSurfaces(srf, (IOnSurface)(object)val4, CSUtil.Tolerance, out var intersectionCurves, out var _))
		{
			msg = "RhinoIntersectSurfaces failed";
			return false;
		}
		if (intersectionCurves.Length != 1)
		{
			msg = "RhinoIntersectSurfaces produced unexpected results";
			return false;
		}
		crv = intersectionCurves[0].Duplicate();
		return true;
	}

	public static bool CurveFrom2ViewSimple(Curve profileRC, Vector3d profileNormalRC, Surface srfRC, out Curve outcrv, out string msg)
	{
		//IL_000b: Unknown result type (might be due to invalid IL or missing references)
		//IL_001c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0022: Expected O, but got Unknown
		//IL_002a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0030: Expected O, but got Unknown
		//IL_0030: Unknown result type (might be due to invalid IL or missing references)
		//IL_006b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0072: Expected O, but got Unknown
		outcrv = null;
		OnCurve val = null;
		OnCurve val2 = (OnCurve)Interop.ToOnCurve(profileRC);
		On3dVector obj = CSConvert.Vector3dCopyToOn3dVector(profileNormalRC);
		OnSurface surfaceLhs = (OnSurface)Interop.ToOnSurface(srfRC);
		msg = string.Empty;
		On3dVector val3 = new On3dVector((IOn3dVector)(object)obj);
		OnCurve val4 = val2.DuplicateCurve();
		((OnGeometry)val4).Translate((IOn3dVector)(object)(val3 * -10.0));
		OnCurve val5 = val2.DuplicateCurve();
		((OnGeometry)val5).Translate((IOn3dVector)(object)(val3 * 10.0));
		OnNurbsSurface val6 = new OnNurbsSurface();
		if (val6.CreateRuledSurface((IOnCurve)(object)val4, (IOnCurve)(object)val5) <= 0)
		{
			msg = "CreateRuledSurface failed";
			return false;
		}
		if (!RhUtilHelper.RhinoIntersectSurfaces((IOnSurface)(object)surfaceLhs, (IOnSurface)(object)val6, CSUtil.Tolerance, out var intersectionCurves, out var _))
		{
			msg = "RhinoIntersectSurfaces failed";
			return false;
		}
		if (intersectionCurves.Length != 1)
		{
			msg = "RhinoIntersectSurfaces produced unexpected results";
			return false;
		}
		val = intersectionCurves[0].Duplicate();
		outcrv = Interop.FromOnCurve(val);
		return true;
	}

	public static bool MergeCurves(IOnCurve[] inputCrvs, out OnCurve mergedCrv, double tolerance = 0.01)
	{
		mergedCrv = null;
		OnCurve[] joinRet = null;
		if (!RhUtilHelper.RhinoJoinCurves(inputCrvs, ref joinRet, tolerance))
		{
			return false;
		}
		if (joinRet == null)
		{
			return false;
		}
		if (joinRet.Length != 1)
		{
			return false;
		}
		mergedCrv = joinRet[0];
		return true;
	}

	public static bool MergeCurves(IEnumerable<Curve> inputCrvs, out Curve mergedCrv, double tolerance = 0.01)
	{
		mergedCrv = null;
		Curve[] array = Curve.JoinCurves(inputCrvs, tolerance);
		if (array == null)
		{
			return false;
		}
		if (array.Length != 1)
		{
			return false;
		}
		mergedCrv = array[0];
		return true;
	}

	public static bool MergeCurves(IOnCurve[] inputCrvs, out OnCurve[] mergedCrvs, double tolerance = 0.01)
	{
		mergedCrvs = null;
		if (!RhUtilHelper.RhinoJoinCurves(inputCrvs, ref mergedCrvs, tolerance))
		{
			return false;
		}
		if (mergedCrvs == null)
		{
			return false;
		}
		return mergedCrvs.Length != 0;
	}

	public static bool MergeCurves(IEnumerable<Curve> inputCrvs, out Curve[] mergedCrvs, double tolerance = 0.01)
	{
		mergedCrvs = Curve.JoinCurves(inputCrvs, tolerance);
		if (mergedCrvs == null)
		{
			return false;
		}
		return mergedCrvs.Length != 0;
	}

	public static bool TrimCurve(IOnCurve crv, IOn3dPoint ptStart, IOn3dPoint ptEnd, out OnCurve trimmedCrv)
	{
		trimmedCrv = null;
		if (ptStart.DistanceTo(ptEnd) < CSUtil.Tolerance)
		{
			return false;
		}
		double paramStart = -1.23432101234321E+308;
		if (!crv.GetClosestPoint(ptStart, ref paramStart))
		{
			return false;
		}
		double paramEnd = -1.23432101234321E+308;
		if (!crv.GetClosestPoint(ptEnd, ref paramEnd))
		{
			return false;
		}
		return TrimCurve(crv, paramStart, paramEnd, out trimmedCrv);
	}

	public static bool TrimCurve(Curve crv, Point3d ptStart, Point3d ptEnd, out Curve trimmedCrv)
	{
		trimmedCrv = null;
		if (ptStart.DistanceTo(ptEnd) < CSUtil.Tolerance)
		{
			return false;
		}
		if (!crv.ClosestPoint(ptStart, out var t))
		{
			return false;
		}
		if (!crv.ClosestPoint(ptEnd, out var t2))
		{
			return false;
		}
		return TrimCurve(crv, t, t2, out trimmedCrv);
	}

	public static bool TrimCurve(IOnCurve crv, double paramStart, double paramEnd, out OnCurve trimmedCrv)
	{
		//IL_00fc: Unknown result type (might be due to invalid IL or missing references)
		//IL_0106: Expected O, but got Unknown
		//IL_00d8: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e2: Expected O, but got Unknown
		//IL_0052: Unknown result type (might be due to invalid IL or missing references)
		//IL_005c: Expected O, but got Unknown
		//IL_0080: Unknown result type (might be due to invalid IL or missing references)
		//IL_008a: Expected O, but got Unknown
		trimmedCrv = null;
		if (paramStart == paramEnd)
		{
			return false;
		}
		if (crv.IsClosed())
		{
			if (paramStart > paramEnd)
			{
				double num = crv.Domain().Min();
				double num2 = crv.Domain().Max();
				List<OnCurve> list = new List<OnCurve>();
				OnCurve val = crv.DuplicateCurve();
				if (Math.Abs(paramStart - num2) > CSUtil.Tolerance && val.Trim((IOnInterval)new OnInterval(paramStart, num2)))
				{
					list.Add(val);
				}
				OnCurve val2 = crv.DuplicateCurve();
				if (Math.Abs(num - paramEnd) > CSUtil.Tolerance && val2.Trim((IOnInterval)new OnInterval(num, paramEnd)))
				{
					list.Add(val2);
				}
				if (list.Count <= 0)
				{
					return false;
				}
				if (list.Count == 1)
				{
					trimmedCrv = list[0];
				}
				else
				{
					IOnCurve[] inputCrvs = (IOnCurve[])(object)list.ToArray();
					if (!MergeCurves(inputCrvs, out trimmedCrv, CSUtil.Tolerance))
					{
						return false;
					}
				}
			}
			else
			{
				trimmedCrv = crv.DuplicateCurve();
				if (!trimmedCrv.Trim((IOnInterval)new OnInterval(paramStart, paramEnd)))
				{
					return false;
				}
			}
		}
		else
		{
			if (paramStart > paramEnd)
			{
				double num3 = paramStart;
				paramStart = paramEnd;
				paramEnd = num3;
			}
			trimmedCrv = crv.DuplicateCurve();
			if (!trimmedCrv.Trim((IOnInterval)new OnInterval(paramStart, paramEnd)))
			{
				return false;
			}
		}
		return true;
	}

	public static bool TrimCurve(Curve crv, double paramStart, double paramEnd, out Curve trimmedCrv)
	{
		trimmedCrv = null;
		if (paramStart == paramEnd)
		{
			return false;
		}
		if (crv.IsClosed)
		{
			if (paramStart > paramEnd)
			{
				double min = crv.Domain.Min;
				double max = crv.Domain.Max;
				List<Curve> list = new List<Curve>();
				if (Math.Abs(paramStart - max) > CSUtil.Tolerance)
				{
					Curve curve = crv.Trim(new Interval(paramStart, max));
					if (curve != null)
					{
						list.Add(curve);
					}
				}
				if (Math.Abs(min - paramEnd) > CSUtil.Tolerance)
				{
					Curve curve2 = crv.Trim(new Interval(min, paramEnd));
					if (curve2 != null)
					{
						list.Add(curve2);
					}
				}
				if (list.Count <= 0)
				{
					return false;
				}
				if (list.Count == 1)
				{
					trimmedCrv = list[0];
				}
				else if (!MergeCurves((IEnumerable<Curve>)list.ToArray(), out trimmedCrv, CSUtil.Tolerance))
				{
					return false;
				}
				return true;
			}
			trimmedCrv = crv.Trim(new Interval(paramStart, paramEnd));
			if (trimmedCrv != null)
			{
				return trimmedCrv.IsValid;
			}
			return false;
		}
		if (paramStart > paramEnd)
		{
			double num = paramStart;
			paramStart = paramEnd;
			paramEnd = num;
		}
		trimmedCrv = crv.Trim(new Interval(paramStart, paramEnd));
		if (trimmedCrv != null)
		{
			return trimmedCrv.IsValid;
		}
		return false;
	}

	private static bool DoAppendPoint(bool doClean, double blendAmount1 = 1.0, double blendAmount2 = 1.0, double blendAmount3 = 1.0)
	{
		if (doClean)
		{
			return blendAmount1 > 0.0 && blendAmount2 > 0.0 && blendAmount3 > 0.0;
		}
		return true;
	}

	private static void GetBlendPoints(IOnCurve crv1, IOnCurve crv2, bool doBlendFromStart1, bool doBlendFromStart2, out On3dPoint pointStart, out On3dPoint pointEnd, out On3dVector vectorStart, out On3dVector vectorEnd)
	{
		if (doBlendFromStart1)
		{
			pointStart = crv1.PointAtStart();
			vectorStart = crv1.TangentAt(crv1.Domain().Min()) * -1.0;
		}
		else
		{
			pointStart = crv1.PointAtEnd();
			vectorStart = crv1.TangentAt(crv1.Domain().Max());
		}
		if (doBlendFromStart2)
		{
			pointEnd = crv2.PointAtStart();
			vectorEnd = crv2.TangentAt(crv2.Domain().Min()) * -1.0;
		}
		else
		{
			pointEnd = crv2.PointAtEnd();
			vectorEnd = crv2.TangentAt(crv2.Domain().Max());
		}
	}

	private static void GetBlendPoints(Curve crv1, Curve crv2, bool doBlendFromStart1, bool doBlendFromStart2, out Point3d pointStart, out Point3d pointEnd, out Vector3d vectorStart, out Vector3d vectorEnd)
	{
		if (doBlendFromStart1)
		{
			pointStart = crv1.PointAtStart;
			vectorStart = crv1.TangentAtStart * -1.0;
		}
		else
		{
			pointStart = crv1.PointAtEnd;
			vectorStart = crv1.TangentAtEnd;
		}
		if (doBlendFromStart2)
		{
			pointEnd = crv2.PointAtStart;
			vectorEnd = crv2.TangentAtStart * -1.0;
		}
		else
		{
			pointEnd = crv2.PointAtEnd;
			vectorEnd = crv2.TangentAtEnd;
		}
	}
}
