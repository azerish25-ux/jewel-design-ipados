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
		return BlendCurve(pointStart, pointEnd, vectorStart, vectorEnd, blendAmount, blendStart, blendEnd, doClean);
	}

	public static OnNurbsCurve BlendCurve(IOn3dPoint pointStart, IOn3dPoint pointEnd, IOn3dVector vectorStart, IOn3dVector vectorEnd, double blendAmount = 1.0, double blendStart = 1.0, double blendEnd = 1.0, bool doClean = false)
	{
		double num = pointStart.DistanceTo(pointEnd);
		On3dPointArray on3dPointArray = new On3dPointArray();
		on3dPointArray.Append(pointStart);
		if (DoAppendPoint(doClean, blendAmount, blendStart))
		{
			on3dPointArray.Append(new On3dPoint(pointStart) + new On3dVector(vectorStart) * num * 0.2 * blendAmount * blendStart);
			on3dPointArray.Append(new On3dPoint(pointStart) + new On3dVector(vectorStart) * num * 0.4 * blendAmount * blendStart);
		}
		if (DoAppendPoint(doClean, blendAmount, blendEnd))
		{
			on3dPointArray.Append(new On3dPoint(pointEnd) + new On3dVector(vectorEnd) * num * 0.4 * blendAmount * blendEnd);
			on3dPointArray.Append(new On3dPoint(pointEnd) + new On3dVector(vectorEnd) * num * 0.2 * blendAmount * blendEnd);
		}
		on3dPointArray.Append(new On3dPoint(pointEnd));
		OnBezierCurve onBezierCurve = new OnBezierCurve(on3dPointArray);
		OnNurbsCurve nurbs_curve = new OnNurbsCurve();
		onBezierCurve.GetNurbForm(ref nurbs_curve);
		return nurbs_curve;
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
		return BlendCurveAdvanced(pointStart, pointEnd, vectorStart, vectorEnd, g1, g2, blendStartA, blendStartB, blendStartC, blendEndA, blendEndB, blendEndC, doClean);
	}

	public static OnNurbsCurve BlendCurveAdvanced(IOn3dPoint pointStart, IOn3dPoint pointEnd, IOn3dVector vectorStart, IOn3dVector vectorEnd, int g1 = 1, int g2 = 1, double blendStartA = 1.0, double blendStartB = 1.0, double blendStartC = 1.0, double blendEndA = 1.0, double blendEndB = 1.0, double blendEndC = 1.0, bool doClean = false)
	{
		double num = pointStart.DistanceTo(pointEnd) / 5.0;
		On3dPointArray on3dPointArray = new On3dPointArray();
		on3dPointArray.Append(new On3dPoint(pointStart));
		if (g1 >= 1 && DoAppendPoint(doClean, blendStartA))
		{
			on3dPointArray.Append(new On3dPoint(pointStart) + new On3dVector(vectorStart) * num * blendStartA);
		}
		if (g1 >= 2 && DoAppendPoint(doClean, blendStartB))
		{
			on3dPointArray.Append(new On3dPoint(pointStart) + new On3dVector(vectorStart) * num * (blendStartA + blendStartB));
		}
		if (g1 >= 3 && DoAppendPoint(doClean, blendStartC))
		{
			on3dPointArray.Append(new On3dPoint(pointStart) + new On3dVector(vectorStart) * num * (blendStartA + blendStartB + blendStartC));
		}
		if (g2 >= 3 && DoAppendPoint(doClean, blendEndC))
		{
			on3dPointArray.Append(new On3dPoint(pointEnd) + new On3dVector(vectorEnd) * num * (blendEndA + blendEndB + blendEndC));
		}
		if (g2 >= 2 && DoAppendPoint(doClean, blendEndB))
		{
			on3dPointArray.Append(new On3dPoint(pointEnd) + new On3dVector(vectorEnd) * num * (blendEndA + blendEndB));
		}
		if (g2 >= 1 && DoAppendPoint(doClean, blendEndA))
		{
			on3dPointArray.Append(new On3dPoint(pointEnd) + new On3dVector(vectorEnd) * num * blendEndA);
		}
		on3dPointArray.Append(new On3dPoint(pointEnd));
		OnBezierCurve onBezierCurve = new OnBezierCurve(on3dPointArray);
		OnNurbsCurve nurbs_curve = new OnNurbsCurve();
		onBezierCurve.GetNurbForm(ref nurbs_curve);
		return nurbs_curve;
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
		if (crv == null)
		{
			return false;
		}
		OnNurbsCurve nurbs_curve = new OnNurbsCurve();
		crv.GetNurbForm(ref nurbs_curve);
		if (!nurbs_curve.IsValid())
		{
			return false;
		}
		List<double> list = new List<double>();
		double num = nurbs_curve.Domain().Min();
		double t = nurbs_curve.Domain().Max();
		int hint = 0;
		list.Add(num);
		while (true)
		{
			double t2 = 0.0;
			if (!nurbs_curve.GetNextDiscontinuity(IOn.continuity.C1_continuous, num, t, ref t2, ref hint))
			{
				break;
			}
			num = t2;
			list.Add(t2);
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
			double num2 = list[j];
			double num3 = num2 - nurbs_curve.Domain().Length() / 2000.0;
			double num4 = num2 + nurbs_curve.Domain().Length() / 2000.0;
			if (num4 > nurbs_curve.Domain().Max())
			{
				num4 -= nurbs_curve.Domain().Length();
			}
			if (num3 < nurbs_curve.Domain().Min())
			{
				num3 += nurbs_curve.Domain().Length();
			}
			On3dVector on3dVector = CSUtilGeometry.TangentAtForce(nurbs_curve, num3);
			On3dVector on3dVector2 = CSUtilGeometry.TangentAtForce(nurbs_curve, num4);
			on3dVector.Unitize();
			on3dVector2.Unitize();
			double num5 = Math.Acos(on3dVector * on3dVector2);
			if (num5 < minAngleRadians || num5 > maxAngleRadians)
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
		if (nurbs_curve.IsClosed())
		{
			for (int k = 0; k < list.Count; k++)
			{
				OnNurbsCurve onNurbsCurve = nurbs_curve.Duplicate();
				On3dPoint test_point = onNurbsCurve.PointAt(list[(k + 1) % list.Count]);
				if (onNurbsCurve.ChangeClosedCurveSeam(list[k]))
				{
					double t3 = onNurbsCurve.Domain().Min();
					double t4 = onNurbsCurve.Domain().Max();
					onNurbsCurve.GetClosestPoint(test_point, ref t4);
					onNurbsCurve.Trim(new OnInterval(t3, t4));
				}
				else
				{
					onNurbsCurve.Trim(new OnInterval(list[k], list[k + 1]));
				}
				list2.Add(onNurbsCurve);
			}
		}
		else
		{
			if (Math.Abs(list[0] - nurbs_curve.Domain().Min()) > 0.0001)
			{
				OnNurbsCurve onNurbsCurve2 = nurbs_curve.Duplicate();
				onNurbsCurve2.Trim(new OnInterval(onNurbsCurve2.Domain().Min(), list[0]));
				list2.Add(onNurbsCurve2);
			}
			for (int l = 0; l < list.Count - 1; l++)
			{
				OnNurbsCurve onNurbsCurve3 = nurbs_curve.Duplicate();
				onNurbsCurve3.Trim(new OnInterval(list[l], list[l + 1]));
				list2.Add(onNurbsCurve3);
			}
			if (Math.Abs(list[list.Count - 1] - nurbs_curve.Domain().Max()) > 0.0001)
			{
				OnNurbsCurve onNurbsCurve4 = nurbs_curve.Duplicate();
				onNurbsCurve4.Trim(new OnInterval(list[list.Count - 1], onNurbsCurve4.Domain().Max()));
				list2.Add(onNurbsCurve4);
			}
		}
		List<double> list3 = new List<double>();
		List<double> list4 = new List<double>();
		List<double> list5 = new List<double>();
		foreach (OnNurbsCurve item2 in list2)
		{
			double length = 0.0;
			item2.GetLength(ref length);
			list3.Add(length);
			list4.Add(0.0);
			list5.Add(0.0);
		}
		int num6 = list2.Count;
		if (!nurbs_curve.IsClosed())
		{
			num6--;
		}
		for (int m = 0; m < num6; m++)
		{
			OnNurbsCurve onNurbsCurve5 = list2[m];
			int index = (m + 1) % list2.Count;
			OnNurbsCurve onNurbsCurve6 = list2[index];
			if (onNurbsCurve5 == null || onNurbsCurve6 == null)
			{
				continue;
			}
			On3dVector on3dVector3 = CSUtilGeometry.TangentAtForce(onNurbsCurve5, onNurbsCurve5.Domain().Max());
			On3dVector on3dVector4 = CSUtilGeometry.TangentAtForce(onNurbsCurve6, onNurbsCurve6.Domain().Min());
			if (!(on3dVector3 == null) && !(on3dVector4 == null))
			{
				on3dVector3.Unitize();
				on3dVector4.Unitize();
				double num7 = Math.Acos(on3dVector3 * on3dVector4);
				if (num7 > minAngleRadians && num7 < maxAngleRadians)
				{
					list5[m] = blendSize;
					list4[index] = blendSize;
				}
			}
		}
		for (int n = 0; n < list2.Count; n++)
		{
			OnNurbsCurve onNurbsCurve7 = list2[n];
			if (!(list3[n] < 0.01))
			{
				double num8 = list3[n] * 0.9;
				double num9 = list4[n] + list5[n];
				if (num9 > num8)
				{
					double num10 = list4[n] / num9;
					double num11 = list5[n] / num9;
					list4[n] = num10 * num8;
					list5[n] = num11 * num8;
				}
				double t5 = onNurbsCurve7.Domain().Min();
				double t6 = onNurbsCurve7.Domain().Max();
				if (list4[n] > 0.01)
				{
					double t7 = onNurbsCurve7.Domain().Min();
					double s = list4[n] / list3[n];
					onNurbsCurve7.GetNormalizedArcLengthPoint(s, ref t7);
					t5 = t7;
				}
				if (list5[n] > 0.01)
				{
					double t8 = onNurbsCurve7.Domain().Max();
					double s2 = 1.0 - list5[n] / list3[n];
					onNurbsCurve7.GetNormalizedArcLengthPoint(s2, ref t8);
					t6 = t8;
				}
				onNurbsCurve7.Trim(new OnInterval(t5, t6));
				onNurbsCurve7.MakeDeformable();
			}
		}
		List<OnNurbsCurve> list6 = new List<OnNurbsCurve>();
		for (int num12 = 0; num12 < num6; num12++)
		{
			OnNurbsCurve onNurbsCurve8 = list2[num12];
			int index2 = (num12 + 1) % list2.Count;
			OnNurbsCurve onNurbsCurve9 = list2[index2];
			On3dVector on3dVector5 = onNurbsCurve8.TangentAt(onNurbsCurve8.Domain().Max());
			On3dVector on3dVector6 = onNurbsCurve9.TangentAt(onNurbsCurve9.Domain().Min());
			on3dVector6.Reverse();
			on3dVector5.Unitize();
			on3dVector6.Unitize();
			OnNurbsCurve item = null;
			if (onNurbsCurve9.PointAtEnd().DistanceTo(onNurbsCurve8.PointAtStart()) > 0.01)
			{
				item = BlendCurve(onNurbsCurve8.PointAtEnd(), onNurbsCurve9.PointAtStart(), on3dVector5, on3dVector6, blendAmount);
			}
			list6.Add(item);
		}
		List<OnNurbsCurve> list7 = new List<OnNurbsCurve>();
		for (int num13 = 0; num13 < list2.Count; num13++)
		{
			list7.Add(list2[num13]);
			if (list6.Count > num13 && list6[num13] != null)
			{
				list7.Add(list6[num13]);
			}
		}
		IOnCurve[] inputCrvs = list7.ToArray();
		if (MergeCurves(inputCrvs, out OnCurve mergedCrv, 0.01) && mergedCrv != null)
		{
			if (mergedCrv.IsClosed())
			{
				double t9 = mergedCrv.Domain().Min();
				if (mergedCrv.GetClosestPoint(crv.PointAtStart(), ref t9))
				{
					mergedCrv.ChangeClosedCurveSeam(t9);
				}
			}
			outputCurve = mergedCrv;
			return true;
		}
		return false;
	}

	public static bool BlendCurveCorners(Curve incrv, double minAngleRadians, double maxAngleRadians, double blendSize, double blendAmount, ref Curve outputCurve)
	{
		if (incrv == null)
		{
			return false;
		}
		OnCurve onCurve = (OnCurve)Interop.ToOnCurve(incrv);
		OnNurbsCurve nurbs_curve = new OnNurbsCurve();
		onCurve.GetNurbForm(ref nurbs_curve);
		if (!nurbs_curve.IsValid())
		{
			return false;
		}
		List<double> list = new List<double>();
		double num = nurbs_curve.Domain().Min();
		double t = nurbs_curve.Domain().Max();
		int hint = 0;
		list.Add(num);
		while (true)
		{
			double t2 = 0.0;
			if (!nurbs_curve.GetNextDiscontinuity(IOn.continuity.C1_continuous, num, t, ref t2, ref hint))
			{
				break;
			}
			num = t2;
			list.Add(t2);
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
			double num2 = list[j];
			double num3 = num2 - nurbs_curve.Domain().Length() / 2000.0;
			double num4 = num2 + nurbs_curve.Domain().Length() / 2000.0;
			if (num4 > nurbs_curve.Domain().Max())
			{
				num4 -= nurbs_curve.Domain().Length();
			}
			if (num3 < nurbs_curve.Domain().Min())
			{
				num3 += nurbs_curve.Domain().Length();
			}
			On3dVector on3dVector = CSUtilGeometry.TangentAtForce(nurbs_curve, num3);
			On3dVector on3dVector2 = CSUtilGeometry.TangentAtForce(nurbs_curve, num4);
			on3dVector.Unitize();
			on3dVector2.Unitize();
			double num5 = Math.Acos(on3dVector * on3dVector2);
			if (num5 < minAngleRadians || num5 > maxAngleRadians)
			{
				list.RemoveAt(j);
				j--;
			}
		}
		if (list.Count < 1)
		{
			outputCurve = Interop.FromOnCurve(onCurve.Duplicate());
			return true;
		}
		List<OnNurbsCurve> list2 = new List<OnNurbsCurve>();
		if (nurbs_curve.IsClosed())
		{
			for (int k = 0; k < list.Count; k++)
			{
				OnNurbsCurve onNurbsCurve = nurbs_curve.Duplicate();
				On3dPoint test_point = onNurbsCurve.PointAt(list[(k + 1) % list.Count]);
				if (onNurbsCurve.ChangeClosedCurveSeam(list[k]))
				{
					double t3 = onNurbsCurve.Domain().Min();
					double t4 = onNurbsCurve.Domain().Max();
					onNurbsCurve.GetClosestPoint(test_point, ref t4);
					onNurbsCurve.Trim(new OnInterval(t3, t4));
				}
				else
				{
					onNurbsCurve.Trim(new OnInterval(list[k], list[k + 1]));
				}
				list2.Add(onNurbsCurve);
			}
		}
		else
		{
			if (Math.Abs(list[0] - nurbs_curve.Domain().Min()) > 0.0001)
			{
				OnNurbsCurve onNurbsCurve2 = nurbs_curve.Duplicate();
				onNurbsCurve2.Trim(new OnInterval(onNurbsCurve2.Domain().Min(), list[0]));
				list2.Add(onNurbsCurve2);
			}
			for (int l = 0; l < list.Count - 1; l++)
			{
				OnNurbsCurve onNurbsCurve3 = nurbs_curve.Duplicate();
				onNurbsCurve3.Trim(new OnInterval(list[l], list[l + 1]));
				list2.Add(onNurbsCurve3);
			}
			if (Math.Abs(list[list.Count - 1] - nurbs_curve.Domain().Max()) > 0.0001)
			{
				OnNurbsCurve onNurbsCurve4 = nurbs_curve.Duplicate();
				onNurbsCurve4.Trim(new OnInterval(list[list.Count - 1], onNurbsCurve4.Domain().Max()));
				list2.Add(onNurbsCurve4);
			}
		}
		List<double> list3 = new List<double>();
		List<double> list4 = new List<double>();
		List<double> list5 = new List<double>();
		foreach (OnNurbsCurve item2 in list2)
		{
			double length = 0.0;
			item2.GetLength(ref length);
			list3.Add(length);
			list4.Add(0.0);
			list5.Add(0.0);
		}
		int num6 = list2.Count;
		if (!nurbs_curve.IsClosed())
		{
			num6--;
		}
		for (int m = 0; m < num6; m++)
		{
			OnNurbsCurve onNurbsCurve5 = list2[m];
			int index = (m + 1) % list2.Count;
			OnNurbsCurve onNurbsCurve6 = list2[index];
			if (onNurbsCurve5 == null || onNurbsCurve6 == null)
			{
				continue;
			}
			On3dVector on3dVector3 = CSUtilGeometry.TangentAtForce(onNurbsCurve5, onNurbsCurve5.Domain().Max());
			On3dVector on3dVector4 = CSUtilGeometry.TangentAtForce(onNurbsCurve6, onNurbsCurve6.Domain().Min());
			if (!(on3dVector3 == null) && !(on3dVector4 == null))
			{
				on3dVector3.Unitize();
				on3dVector4.Unitize();
				double num7 = Math.Acos(on3dVector3 * on3dVector4);
				if (num7 > minAngleRadians && num7 < maxAngleRadians)
				{
					list5[m] = blendSize;
					list4[index] = blendSize;
				}
			}
		}
		for (int n = 0; n < list2.Count; n++)
		{
			OnNurbsCurve onNurbsCurve7 = list2[n];
			if (!(list3[n] < 0.01))
			{
				double num8 = list3[n] * 0.9;
				double num9 = list4[n] + list5[n];
				if (num9 > num8)
				{
					double num10 = list4[n] / num9;
					double num11 = list5[n] / num9;
					list4[n] = num10 * num8;
					list5[n] = num11 * num8;
				}
				double t5 = onNurbsCurve7.Domain().Min();
				double t6 = onNurbsCurve7.Domain().Max();
				if (list4[n] > 0.01)
				{
					double t7 = onNurbsCurve7.Domain().Min();
					double s = list4[n] / list3[n];
					onNurbsCurve7.GetNormalizedArcLengthPoint(s, ref t7);
					t5 = t7;
				}
				if (list5[n] > 0.01)
				{
					double t8 = onNurbsCurve7.Domain().Max();
					double s2 = 1.0 - list5[n] / list3[n];
					onNurbsCurve7.GetNormalizedArcLengthPoint(s2, ref t8);
					t6 = t8;
				}
				onNurbsCurve7.Trim(new OnInterval(t5, t6));
				onNurbsCurve7.MakeDeformable();
			}
		}
		List<OnNurbsCurve> list6 = new List<OnNurbsCurve>();
		for (int num12 = 0; num12 < num6; num12++)
		{
			OnNurbsCurve onNurbsCurve8 = list2[num12];
			int index2 = (num12 + 1) % list2.Count;
			OnNurbsCurve onNurbsCurve9 = list2[index2];
			On3dVector on3dVector5 = onNurbsCurve8.TangentAt(onNurbsCurve8.Domain().Max());
			On3dVector on3dVector6 = onNurbsCurve9.TangentAt(onNurbsCurve9.Domain().Min());
			on3dVector6.Reverse();
			on3dVector5.Unitize();
			on3dVector6.Unitize();
			OnNurbsCurve item = null;
			if (onNurbsCurve9.PointAtEnd().DistanceTo(onNurbsCurve8.PointAtStart()) > 0.01)
			{
				item = BlendCurve(onNurbsCurve8.PointAtEnd(), onNurbsCurve9.PointAtStart(), on3dVector5, on3dVector6, blendAmount);
			}
			list6.Add(item);
		}
		List<OnNurbsCurve> list7 = new List<OnNurbsCurve>();
		for (int num13 = 0; num13 < list2.Count; num13++)
		{
			list7.Add(list2[num13]);
			if (list6.Count > num13 && list6[num13] != null)
			{
				list7.Add(list6[num13]);
			}
		}
		IOnCurve[] inputCrvs = list7.ToArray();
		if (MergeCurves(inputCrvs, out OnCurve mergedCrv, 0.01) && mergedCrv != null)
		{
			if (mergedCrv.IsClosed())
			{
				double t9 = mergedCrv.Domain().Min();
				if (mergedCrv.GetClosestPoint(onCurve.PointAtStart(), ref t9))
				{
					mergedCrv.ChangeClosedCurveSeam(t9);
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
		crv = null;
		msg = string.Empty;
		On3dVector on3dVector = new On3dVector(profileNormal);
		OnCurve onCurve = profile.DuplicateCurve();
		onCurve.Translate(on3dVector * -10.0);
		OnCurve onCurve2 = profile.DuplicateCurve();
		onCurve2.Translate(on3dVector * 10.0);
		OnNurbsSurface onNurbsSurface = new OnNurbsSurface();
		if (onNurbsSurface.CreateRuledSurface(onCurve, onCurve2) <= 0)
		{
			msg = "CreateRuledSurface failed";
			return false;
		}
		if (!RhUtilHelper.RhinoIntersectSurfaces(srf, onNurbsSurface, CSUtil.Tolerance, out var intersectionCurves, out var _))
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
		outcrv = null;
		OnCurve onCurve = null;
		OnCurve obj = (OnCurve)Interop.ToOnCurve(profileRC);
		On3dVector other = CSConvert.Vector3dCopyToOn3dVector(profileNormalRC);
		OnSurface surfaceLhs = (OnSurface)Interop.ToOnSurface(srfRC);
		msg = string.Empty;
		On3dVector on3dVector = new On3dVector(other);
		OnCurve onCurve2 = obj.DuplicateCurve();
		onCurve2.Translate(on3dVector * -10.0);
		OnCurve onCurve3 = obj.DuplicateCurve();
		onCurve3.Translate(on3dVector * 10.0);
		OnNurbsSurface onNurbsSurface = new OnNurbsSurface();
		if (onNurbsSurface.CreateRuledSurface(onCurve2, onCurve3) <= 0)
		{
			msg = "CreateRuledSurface failed";
			return false;
		}
		if (!RhUtilHelper.RhinoIntersectSurfaces(surfaceLhs, onNurbsSurface, CSUtil.Tolerance, out var intersectionCurves, out var _))
		{
			msg = "RhinoIntersectSurfaces failed";
			return false;
		}
		if (intersectionCurves.Length != 1)
		{
			msg = "RhinoIntersectSurfaces produced unexpected results";
			return false;
		}
		onCurve = intersectionCurves[0].Duplicate();
		outcrv = Interop.FromOnCurve(onCurve);
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
		double t = -1.23432101234321E+308;
		if (!crv.GetClosestPoint(ptStart, ref t))
		{
			return false;
		}
		double t2 = -1.23432101234321E+308;
		if (!crv.GetClosestPoint(ptEnd, ref t2))
		{
			return false;
		}
		return TrimCurve(crv, t, t2, out trimmedCrv);
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
				OnCurve onCurve = crv.DuplicateCurve();
				if (Math.Abs(paramStart - num2) > CSUtil.Tolerance && onCurve.Trim(new OnInterval(paramStart, num2)))
				{
					list.Add(onCurve);
				}
				OnCurve onCurve2 = crv.DuplicateCurve();
				if (Math.Abs(num - paramEnd) > CSUtil.Tolerance && onCurve2.Trim(new OnInterval(num, paramEnd)))
				{
					list.Add(onCurve2);
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
					IOnCurve[] inputCrvs = list.ToArray();
					if (!MergeCurves(inputCrvs, out trimmedCrv, CSUtil.Tolerance))
					{
						return false;
					}
				}
			}
			else
			{
				trimmedCrv = crv.DuplicateCurve();
				if (!trimmedCrv.Trim(new OnInterval(paramStart, paramEnd)))
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
			if (!trimmedCrv.Trim(new OnInterval(paramStart, paramEnd)))
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
