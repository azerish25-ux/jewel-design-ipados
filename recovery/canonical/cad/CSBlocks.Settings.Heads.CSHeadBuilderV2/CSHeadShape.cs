using System;
using System.Collections.Generic;
using CS;
using CSBlockDataUtils.CSGemOffsetCurves;
using CSBlockUtil;
using CSDatas;
using CSDatas.Interfaces;
using CSGemInstance;
using GV.CounterSketch.Shared;
using Rhino;
using Rhino.Geometry;
using Rhino.Geometry.Intersect;

namespace CSBlocks.Settings.Heads.CSHeadBuilderV2;

internal static class CSHeadShape
{
	public static bool HeadShape(CSData iGem, ProngParam[] prongParams, double prongHeight, double headHeight, CSDataArray baseObject, double baseOffset, double angleX, double angleY, double topAngle, double baseAngle, double baseAnglePosition, ValueListItem baseStyle, double cornerBlend, Point3d focalPoint, out Curve headGirdle, out Curve headTop, out Curve headBase, out Curve headMiddleCurve, out Curve outHeadSideX, out Curve outHeadSideY, out Curve bezelPointProngRail, out double gemHeight, out string error)
	{
		error = "";
		gemHeight = 0.0;
		headGirdle = null;
		headTop = null;
		headBase = null;
		headMiddleCurve = null;
		outHeadSideX = null;
		outHeadSideY = null;
		bezelPointProngRail = null;
		if (iGem == null)
		{
			return false;
		}
		if (prongHeight < 0.01)
		{
			prongHeight = 0.01;
		}
		if (!(iGem is CSGemData cSGemData))
		{
			return false;
		}
		double drop = GetDrop(headHeight, baseObject, baseOffset, cSGemData);
		if (!cSGemData.GetGemXformRC(out var xDown, out var _))
		{
			return false;
		}
		double gemXSize = cSGemData.GemXSize;
		double gemYSize = cSGemData.GemYSize;
		double gemXMax = cSGemData.GemXMax;
		double gemYMax = cSGemData.GemYMax;
		Curve curve = cSGemData.GetCurve();
		if (curve == null)
		{
			return false;
		}
		curve.Transform(xDown);
		if (!GemOffsetCurve.GemOffset(cSGemData, 0.0, out Curve curve2, false, out error, cornerBlend))
		{
			return false;
		}
		curve2.MakeDeformable();
		double num = drop * Math.Tan(CSBlockUtil.CSUtilMath.ToRadians(angleX));
		double num2 = drop * Math.Tan(CSBlockUtil.CSUtilMath.ToRadians(angleY));
		double num3 = gemXSize + num * 2.0;
		double num4 = gemYSize + num2 * 2.0;
		Transform transform = Transform.Scale(Plane.WorldXY, num3 / gemXSize, num4 / gemYSize, 1.0);
		Transform xform = Transform.Translation(0.0, 0.0, drop) * transform;
		Curve curve3 = curve2.DuplicateCurve();
		curve3.Transform(xform);
		if (baseAnglePosition < 0.0)
		{
			baseAnglePosition = 0.0;
		}
		if (baseAnglePosition > 1.0)
		{
			baseAnglePosition = 1.0;
		}
		double num5 = (0.0 - drop) * baseAnglePosition * Math.Tan(CSBlockUtil.CSUtilMath.ToRadians(angleX + baseAngle));
		double num6 = (0.0 - drop) * baseAnglePosition * Math.Tan(CSBlockUtil.CSUtilMath.ToRadians(angleY + baseAngle));
		Transform transform2 = Transform.Scale(Plane.WorldXY, (num3 + num5 * 2.0) / gemXSize, (num4 + num6 * 2.0) / gemYSize, 1.0);
		Transform xform2 = Transform.Translation(0.0, 0.0, drop * (1.0 - baseAnglePosition)) * transform2;
		Curve curve4 = curve2.DuplicateCurve();
		curve4.Transform(xform2);
		headMiddleCurve = curve4;
		double num7 = prongHeight * Math.Tan(CSBlockUtil.CSUtilMath.ToRadians(angleX + topAngle));
		double num8 = prongHeight * Math.Tan(CSBlockUtil.CSUtilMath.ToRadians(angleY + topAngle));
		Transform transform3 = Transform.Scale(Plane.WorldXY, (gemXSize + num7 * 2.0) / gemXSize, (gemYSize + num8 * 2.0) / gemYSize, 1.0);
		Transform xform3 = Transform.Translation(0.0, 0.0, prongHeight) * transform3;
		Curve curve5 = curve2.DuplicateCurve();
		curve5.Transform(xform3);
		Point3d item = new Point3d(gemXMax, 0.0, 0.0);
		item.Transform(xform3);
		Point3d item2 = new Point3d(gemXMax, 0.0, 0.0);
		Point3d item3 = new Point3d(gemXMax, 0.0, 0.0);
		item3.Transform(xform2);
		Point3d item4 = new Point3d(gemXMax, 0.0, 0.0);
		item4.Transform(xform);
		Curve curve6 = ProngInterpCurve(new List<Point3d> { item, item2, item3, item4 });
		Point3d item5 = new Point3d(0.0, gemYMax, 0.0);
		item5.Transform(xform3);
		Point3d item6 = new Point3d(0.0, gemYMax, 0.0);
		Point3d item7 = new Point3d(0.0, gemYMax, 0.0);
		item7.Transform(xform2);
		Point3d item8 = new Point3d(0.0, gemYMax, 0.0);
		item8.Transform(xform);
		Curve curve7 = ProngInterpCurve(new List<Point3d> { item5, item6, item7, item8 });
		List<CSGemInstanceDefinition.ProngPosition> list = new List<CSGemInstanceDefinition.ProngPosition>();
		if (cSGemData.Def != null)
		{
			list = cSGemData.Def.ProngPositions;
		}
		foreach (ProngParam prongParam in prongParams)
		{
			double prongPointAngle = prongParam.ProngPointAngle;
			LineCurve lineCurve = new LineCurve(to: curve.PointAt(list[prongParam.ProngPoint].ProngParam) * 50.0, from: Point3d.Origin);
			Transform xform4 = CSBlockUtil.CSUtilMath.Rotation(CSBlockUtil.CSUtilMath.ToRadians(prongPointAngle), Vector3d.ZAxis, Point3d.Origin);
			lineCurve.Transform(xform4);
			CurveIntersections curveIntersections = Intersection.CurveCurve(curve2, lineCurve, 0.001, 0.001);
			if (curveIntersections.Count <= 0)
			{
				continue;
			}
			double parameterA = curveIntersections[0].ParameterA;
			Point3d point3d = new Point3d(curve2.PointAt(parameterA));
			Point3d item9 = new Point3d(point3d);
			item9.Transform(xform);
			Point3d item10 = new Point3d(point3d);
			item10.Transform(xform2);
			if (prongParam.StraightProng)
			{
				item10 = Point3d.Unset;
			}
			Point3d point3d2 = new Point3d(point3d);
			point3d2.Transform(xform3);
			if (baseStyle == ValueListItem.HeadBuilderProngHead)
			{
				Vector3d motion = new Vector3d(new Point3d(0.0, 0.0, point3d2.Z) - point3d2);
				motion.Unitize();
				motion *= prongParam.ProngTopNudge;
				point3d2.Transform(Transform.Translation(motion));
				point3d.Transform(Transform.Translation(motion));
				motion = new Vector3d(new Point3d(0.0, 0.0, point3d2.Z) - point3d2);
				motion.Unitize();
				motion *= prongParam.ProngBaseNudge * (1.0 - baseAnglePosition) + prongParam.ProngTopNudge * baseAnglePosition;
				item10.Transform(Transform.Translation(motion));
				motion.Unitize();
				motion *= prongParam.ProngBaseNudge;
				item9.Transform(Transform.Translation(motion));
			}
			List<Point3d> list2 = new List<Point3d> { point3d2, point3d, item10, item9 };
			if (focalPoint != Point3d.Unset)
			{
				Point3d point3d3 = focalPoint;
				point3d3.Transform(xDown);
				double num9 = point3d3.X / point3d3.Z;
				double num10 = point3d3.Y / point3d3.Z;
				for (int j = 0; j < list2.Count; j++)
				{
					if (!prongParam.StraightProng)
					{
						Point3d value = list2[j];
						value.X = list2[j].X + list2[j].Z * num9;
						value.Y = list2[j].Y + list2[j].Z * num10;
						list2[j] = value;
					}
				}
			}
			Curve prongRail = ProngInterpCurve(list2);
			prongParam.ProngRail = prongRail;
		}
		double prongPointAngle2 = prongParams[0].ProngPointAngle;
		LineCurve lineCurve2 = new LineCurve(to: curve.PointAt(cSGemData.BezelParam) * 50.0, from: Point3d.Origin);
		Transform xform5 = CSBlockUtil.CSUtilMath.Rotation(CSBlockUtil.CSUtilMath.ToRadians(prongPointAngle2), Vector3d.ZAxis, Point3d.Origin);
		lineCurve2.Transform(xform5);
		CurveIntersections curveIntersections2 = Intersection.CurveCurve(curve2, lineCurve2, 0.001, 0.001);
		if (curveIntersections2.Count <= 0)
		{
			error = "Bezel Point Prong Failed";
			return false;
		}
		double parameterA2 = curveIntersections2[0].ParameterA;
		Point3d point3d4 = new Point3d(curve2.PointAt(parameterA2));
		Point3d item11 = new Point3d(point3d4);
		item11.Transform(xform);
		Point3d item12 = new Point3d(point3d4);
		item12.Transform(xform2);
		Point3d point3d5 = new Point3d(point3d4);
		point3d5.Transform(xform3);
		if (baseStyle == ValueListItem.HeadBuilderProngHead)
		{
			Vector3d motion2 = new Vector3d(new Point3d(0.0, 0.0, point3d5.Z) - point3d5);
			motion2.Unitize();
			motion2 *= prongParams[0].ProngTopNudge;
			point3d5.Transform(Transform.Translation(motion2));
			point3d4.Transform(Transform.Translation(motion2));
			motion2 = new Vector3d(new Point3d(0.0, 0.0, point3d5.Z) - point3d5);
			motion2.Unitize();
			motion2 *= prongParams[0].ProngBaseNudge * (1.0 - baseAnglePosition) + prongParams[0].ProngTopNudge * baseAnglePosition;
			item12.Transform(Transform.Translation(motion2));
			motion2.Unitize();
			motion2 *= prongParams[0].ProngBaseNudge;
			item11.Transform(Transform.Translation(motion2));
		}
		List<Point3d> list3 = new List<Point3d> { point3d5, point3d4, item12, item11 };
		if (focalPoint != Point3d.Unset)
		{
			Point3d point3d6 = focalPoint;
			point3d6.Transform(xDown);
			double num11 = point3d6.X / point3d6.Z;
			double num12 = point3d6.Y / point3d6.Z;
			for (int k = 0; k < list3.Count; k++)
			{
				if (!prongParams[0].StraightProng)
				{
					Point3d value2 = list3[k];
					value2.X = list3[k].X + list3[k].Z * num11;
					value2.Y = list3[k].Y + list3[k].Z * num12;
					list3[k] = value2;
				}
			}
		}
		bezelPointProngRail = ProngInterpCurve(list3);
		headGirdle = curve2.DuplicateCurve();
		headTop = curve5;
		headBase = curve3;
		outHeadSideX = curve6;
		outHeadSideY = curve7;
		gemHeight = cSGemData.GemZMin;
		return true;
	}

	private static double GetDrop(double vBelowInput, CSDataArray vBaseObject, double vBaseOffset, CSGemData gem)
	{
		if (vBaseObject == null)
		{
			return vBelowInput + vBaseOffset;
		}
		CSData arrayItem = vBaseObject.GetArrayItem(0);
		Brep brep = null;
		Curve curve = (arrayItem as ICSCurve)?.GetCurve();
		if (curve != null)
		{
			Curve curve2 = curve.DuplicateCurve();
			Curve curve3 = curve.DuplicateCurve();
			curve2.Translate(Vector3d.YAxis * 50.0);
			curve3.Translate(Vector3d.YAxis * -50.0);
			brep = NurbsSurface.CreateRuledSurface(curve2, curve3).ToBrep();
		}
		if (brep == null && arrayItem is ICSBrep iCSBrep)
		{
			brep = iCSBrep.GetBrep();
		}
		if (brep == null)
		{
			return vBelowInput + vBaseOffset;
		}
		gem.GetGemXformRC(out var _, out var xUp);
		LineCurve lineCurve = new LineCurve(new Point3d(0.0, 0.0, 100.0), new Point3d(0.0, 0.0, -100.0));
		lineCurve.Transform(xUp);
		Point3d point3d = default(Point3d);
		point3d.Transform(xUp);
		Vector3d vector3d = new Vector3d(Vector3d.ZAxis);
		vector3d.Transform(xUp);
		vector3d.Unitize();
		if (!Intersection.CurveBrep(lineCurve, brep, CSApplicationHelper.Application.Tolerance, out var overlapCurves, out var intersectionPoints))
		{
			return vBelowInput + vBaseOffset;
		}
		if (intersectionPoints.Length == 0 && overlapCurves.Length == 0)
		{
			return vBelowInput + vBaseOffset;
		}
		Point3d point3d2 = Point3d.Unset;
		double num = -1.23432101234321E+308;
		Curve[] array = overlapCurves;
		foreach (Curve obj in array)
		{
			obj.ClosestPoint(point3d, out var t);
			Point3d point3d3 = obj.PointAt(t);
			t = point3d3.DistanceTo(point3d);
			if (!RhinoMath.IsValidDouble(num) || num > t)
			{
				point3d2 = point3d3;
				num = t;
			}
		}
		Point3d[] array2 = intersectionPoints;
		for (int i = 0; i < array2.Length; i++)
		{
			Point3d point3d4 = array2[i];
			double num2 = point3d4.DistanceTo(point3d);
			if (!RhinoMath.IsValidDouble(num) || num > num2)
			{
				point3d2 = point3d4;
				num = num2;
			}
		}
		if (vector3d * (point3d - point3d2) < 0.0)
		{
			num *= -1.0;
		}
		return 0.0 - num + vBaseOffset;
	}

	private static Curve ProngInterpCurve(List<Point3d> listPoints)
	{
		for (int i = 0; i < listPoints.Count - 1; i++)
		{
			if (listPoints[i].DistanceTo(listPoints[i + 1]) < 0.01)
			{
				listPoints.RemoveAt(i);
				i--;
			}
		}
		List<Point3d> list = new List<Point3d>();
		if (listPoints.Count > 2)
		{
			list.AddRange(listPoints);
		}
		else
		{
			LineCurve lineCurve = new LineCurve(listPoints[0], listPoints[1]);
			list.Add(listPoints[0]);
			list.Add(lineCurve.PointAt(0.1));
			list.Add(listPoints[1]);
		}
		return Curve.CreateInterpolatedCurve(list, 3, CurveKnotStyle.Chord) ?? new LineCurve(listPoints[0], listPoints[listPoints.Count - 1]);
	}
}
