using System;
using System.Collections.Generic;
using CS;
using CSBlockDataUtils.CSGemOffsetCurves;
using CSBlockUtil;
using CSDatas;
using GV.CounterSketch.Shared;
using Rhino.Geometry;
using Rhino.Geometry.Intersect;

namespace CSBlocks.Settings.Heads.CSHeadBuilderV2;

internal static class CSHeadBetweenProngSrf
{
	public static void BetweenProngSrf(Curve headBase, Curve bezelPointProng, CSGemData gem, ProngParam[] prongParams, ValueListItem baseStyle, bool planarDome, bool buildInner, bool buildOuter, out List<Brep> innerSrf, out List<Brep> outerSrf)
	{
		innerSrf = new List<Brep>();
		outerSrf = new List<Brep>();
		if (!(gem.Duplicate() is CSGemData cSGemData))
		{
			return;
		}
		cSGemData.GetTransform(out var value);
		value.TryGetInverse(out var inverseTransform);
		cSGemData.ApplyTransform(inverseTransform);
		if (!BezelRevolveInnerOuter(bezelPointProng, prongParams[0].ProngTopY, 0.0, prongParams[0].ProngBaseY, 0.0, baseStyle, prongParams[0].ProngBaseDomeHeight, planarDome, out var innerRailCrv, out var outerRailCrv, out var error) || !RevolveStartEnd(innerRailCrv, cSGemData, headBase, out var startCrv, out var endCrv, out error) || !RevolveStartEnd(outerRailCrv, cSGemData, headBase, out var startCrv2, out var endCrv2, out error))
		{
			return;
		}
		List<Curve> list = new List<Curve>();
		List<Curve> list2 = new List<Curve>();
		List<Point3d> list3 = new List<Point3d>();
		List<Point3d> list4 = new List<Point3d>();
		List<Point3d> list5 = new List<Point3d>();
		List<Point3d> list6 = new List<Point3d>();
		for (int i = 0; i < prongParams.Length; i++)
		{
			BezelRevolveInnerOuter(prongParams[i].ProngRail, prongParams[i].ProngTopY, 0.0, prongParams[i].ProngBaseY, 0.0, baseStyle, prongParams[i].ProngBaseDomeHeight, planarDome, out var innerRailCrv2, out var outerRailCrv2, out error);
			list.Add(innerRailCrv2);
			list2.Add(outerRailCrv2);
			Point3d pointAtStart = innerRailCrv2.PointAtStart;
			Point3d pointAtStart2 = outerRailCrv2.PointAtStart;
			Point3d pointAtEnd = innerRailCrv2.PointAtEnd;
			Point3d pointAtEnd2 = outerRailCrv2.PointAtEnd;
			Vector3d vector3d = pointAtStart2 - pointAtStart;
			Vector3d vector3d2 = pointAtEnd2 - pointAtEnd;
			Point3d point3d = pointAtStart - vector3d;
			Point3d to = pointAtStart2 + vector3d * 5.0;
			Point3d point3d2 = pointAtEnd - vector3d2;
			Point3d to2 = pointAtEnd2 + vector3d2 * 5.0;
			LineCurve intersectCrv = new LineCurve(new Line(point3d, to));
			LineCurve intersectCrv2 = new LineCurve(new Line(point3d2, to2));
			if (buildInner)
			{
				Point3d point3d3 = HeadIntersect(startCrv, intersectCrv, pointAtStart);
				Point3d point3d4 = HeadIntersect(endCrv, intersectCrv2, pointAtEnd);
				list3.Add(point3d3);
				list5.Add(point3d4);
				list[i] = FixCurve(list[i], point3d3, point3d4);
			}
			if (buildOuter)
			{
				Point3d point3d5 = HeadIntersect(startCrv2, intersectCrv, pointAtStart2);
				Point3d point3d6 = HeadIntersect(endCrv2, intersectCrv2, pointAtEnd2);
				list4.Add(point3d5);
				list6.Add(point3d6);
				list2[i] = FixCurve(list2[i], point3d5, point3d6);
			}
		}
		int num = prongParams.Length;
		for (int j = 0; j < num; j++)
		{
			int index = (j + 1) % num;
			if (buildInner)
			{
				Point3d p = list3[j];
				Point3d p2 = list5[j];
				Point3d p3 = list3[index];
				Point3d p4 = list5[index];
				Curve curve = list[j];
				Curve curve2 = list[index];
				Curve curve3 = TrimToPoints(startCrv, p, p3);
				Curve curve4 = TrimToPoints(endCrv, p2, p4);
				CSUtilSweepRC.Sweep2(curve3, curve4, new Curve[2] { curve, curve2 }, new double[2]
				{
					curve3.Domain.Min,
					curve3.Domain.Max
				}, new double[2]
				{
					curve4.Domain.Min,
					curve4.Domain.Max
				}, out var breps, closed: false, maintainHeight: false);
				innerSrf.Add(breps);
			}
			if (buildOuter)
			{
				Point3d p5 = list4[j];
				Point3d p6 = list6[j];
				Point3d p7 = list4[index];
				Point3d p8 = list6[index];
				Curve curve5 = list2[j];
				Curve curve6 = list2[index];
				Curve curve7 = TrimToPoints(startCrv2, p5, p7);
				Curve curve8 = TrimToPoints(endCrv2, p6, p8);
				CSUtilSweepRC.Sweep2(curve7, curve8, new Curve[2] { curve5, curve6 }, new double[2]
				{
					curve7.Domain.Min,
					curve7.Domain.Max
				}, new double[2]
				{
					curve8.Domain.Min,
					curve8.Domain.Max
				}, out var breps2, closed: false, maintainHeight: false);
				outerSrf.Add(breps2);
			}
		}
	}

	private static bool BezelRevolveInnerOuter(Curve prongRail, double prongTopY, double prongTopOffset, double prongBaseY, double prongBaseOffset, ValueListItem baseStyle, double prongBaseDomeHeight, bool planarDome, out Curve innerRailCrv, out Curve outerRailCrv, out string error)
	{
		innerRailCrv = null;
		outerRailCrv = null;
		PolylineCurve polylineCurve = new PolylineCurve(new List<Point3d>
		{
			new Point3d(0.0, 0.0, 0.0),
			new Point3d(-0.5, 0.0, 0.0),
			new Point3d(-0.5, 1.0, 0.0),
			new Point3d(0.5, 1.0, 0.0),
			new Point3d(0.5, 0.0, 0.0),
			new Point3d(0.0, 0.0, 0.0)
		});
		Curve curve = new PolylineCurve(new List<Point3d>
		{
			new Point3d(0.0, 0.0, 0.0),
			new Point3d(-0.5, 0.0, 0.0),
			new Point3d(-0.5, 1.0, 0.0),
			new Point3d(0.5, 1.0, 0.0),
			new Point3d(0.5, 0.0, 0.0),
			new Point3d(0.0, 0.0, 0.0)
		});
		Point3d pointAtStart = prongRail.PointAtStart;
		pointAtStart.Z = 0.0;
		Vector3d vector3d = new Vector3d(pointAtStart);
		vector3d.Unitize();
		Point3d point3d = new Point3d(5.0, 0.0, 0.0);
		Transform xform = CSBlockUtil.CSUtilMath.Orient3Point(prongRail.PointAtStart, vector3d, Vector3d.ZAxis, point3d, Vector3d.XAxis, Vector3d.ZAxis);
		Transform xform2 = CSBlockUtil.CSUtilMath.Orient3Point(point3d, Vector3d.XAxis, Vector3d.ZAxis, prongRail.PointAtStart, vector3d, Vector3d.ZAxis);
		prongRail = prongRail.DuplicateCurve();
		prongRail.Transform(xform);
		double d = CSBlockUtil.CSUtilMath.AngleBetweenVectorsWithDirection(new Vector3d(0.0, 0.0, -1.0), prongRail.TangentAt(prongRail.Domain.Min), -Vector3d.YAxis);
		Transform transform = Transform.Scale(yScaleFactor: prongTopY * (planarDome ? (1.0 / Math.Cos(d)) : 1.0), plane: Plane.WorldXY, xScaleFactor: 1.0, zScaleFactor: 0.0);
		Transform transform2 = Transform.Translation(0.0, prongTopOffset, 0.0);
		Transform transform3 = CSBlockUtil.CSUtilMath.Orient3Point(Point3d.Origin, Vector3d.XAxis, Vector3d.ZAxis, point3d, -Vector3d.YAxis, Vector3d.ZAxis);
		Curve curve2 = polylineCurve.DuplicateCurve();
		curve2.Transform(transform3 * transform2 * transform);
		Transform xform3 = CSBlockUtil.CSUtilMath.Rotation(Vector3d.ZAxis * -1.0, prongRail.TangentAtStart, point3d);
		if (!planarDome)
		{
			curve2.Transform(xform3);
		}
		double z = prongRail.PointAtEnd.Z + prongBaseDomeHeight;
		double num = CSBlockUtil.CSUtilMath.AngleBetweenVectorsWithDirection(new Vector3d(0.0, 0.0, -1.0), prongRail.TangentAtEnd, -Vector3d.YAxis);
		double length = ((num < 0.0) ? (Math.Tan(0.0 - num) * prongBaseY + 0.05) : 0.05);
		if (baseStyle != ValueListItem.HeadBuilderProngHead)
		{
			length = ((num < 0.0) ? (Math.Tan(0.0 - num) * prongBaseY + 0.05) : (Math.Tan(num) * prongBaseY + 0.05));
		}
		prongRail = prongRail.Extend(CurveEnd.End, length, CurveExtensionStyle.Line);
		Point3d pointAtEnd = prongRail.PointAtEnd;
		Transform transform4 = Transform.Scale(Plane.WorldXY, 1.0, prongBaseY, 0.0);
		Transform transform5 = Transform.Translation(0.0, prongBaseOffset, 0.0);
		Vector3d tangentAtEnd = prongRail.TangentAtEnd;
		Transform transform6 = CSBlockUtil.CSUtilMath.Orient3Point(Point3d.Origin, Vector3d.XAxis, Vector3d.ZAxis, pointAtEnd, -Vector3d.YAxis, -tangentAtEnd);
		Curve curve3 = curve.DuplicateCurve();
		curve3.Transform(transform6 * transform5 * transform4);
		Curve[] profiles = new Curve[2] { curve2, curve3 };
		double[] parameters = new double[2]
		{
			prongRail.Domain.Min,
			prongRail.Domain.Max
		};
		if (!CSUtilSweepRC.Sweep1(prongRail, profiles, parameters, out var breps, closed: false, globalShapeBlend: false))
		{
			error = "Failed Sweep1";
			return false;
		}
		if (breps == null || breps.Length == 0)
		{
			error = "Failed Sweep1: No results";
			return false;
		}
		Brep splitter = NurbsSurface.CreateFromCorners(new Point3d(20.0, 20.0, z), new Point3d(-20.0, 20.0, z), new Point3d(-20.0, -20.0, z), new Point3d(20.0, -20.0, z)).ToBrep();
		Brep[] array = breps[0].Split(splitter, 0.01);
		if (array.Length != 2)
		{
			error = "Prong split failed";
			return false;
		}
		BoundingBox boundingBox = array[0].GetBoundingBox(accurate: true);
		BoundingBox boundingBox2 = array[1].GetBoundingBox(accurate: true);
		Brep brep = ((boundingBox.Center.Z > boundingBox2.Center.Z) ? array[0] : array[1]);
		Point3d pointAtStart2 = prongRail.PointAtStart;
		Point3d pointAtEnd2 = prongRail.PointAtEnd;
		NurbsSurface surface = NurbsSurface.CreateFromCorners(pointAtEnd2 + new Point3d(-4.0, 0.0, -3.0), pointAtEnd2 + new Point3d(8.0, 0.0, -3.0), pointAtStart2 + new Point3d(8.0, 0.0, 3.0), pointAtStart2 + new Point3d(-4.0, 0.0, 3.0));
		Intersection.BrepSurface(brep, surface, CSApplicationHelper.Application.Tolerance, out var intersectionCurves, out var _);
		innerRailCrv = ((intersectionCurves[0].PointAtStart.X < intersectionCurves[1].PointAtStart.X) ? intersectionCurves[0] : intersectionCurves[1]);
		outerRailCrv = ((intersectionCurves[0].PointAtStart.X < intersectionCurves[1].PointAtStart.X) ? intersectionCurves[1] : intersectionCurves[0]);
		if (innerRailCrv.PointAtStart.Z < innerRailCrv.PointAtEnd.Z)
		{
			innerRailCrv.Reverse();
		}
		if (outerRailCrv.PointAtStart.Z < outerRailCrv.PointAtEnd.Z)
		{
			outerRailCrv.Reverse();
		}
		innerRailCrv.Transform(xform2);
		outerRailCrv.Transform(xform2);
		error = "";
		return true;
	}

	private static Curve FixCurve(Curve crvIn, Point3d p0, Point3d p1)
	{
		NurbsCurve nurbsCurve = crvIn.ToNurbsCurve();
		Vector3d vector3d = p0 - nurbsCurve.PointAtStart;
		Vector3d vector3d2 = p1 - nurbsCurve.PointAtEnd;
		double z = nurbsCurve.PointAtEnd.Z;
		for (int i = 0; i < nurbsCurve.Points.Count; i++)
		{
			ControlPoint value = nurbsCurve.Points[i];
			Point3d location = value.Location;
			double z2 = location.Z;
			location += vector3d * (1.0 - z2 / z);
			location += vector3d2 * (z2 / z);
			value.Location = location;
			nurbsCurve.Points[i] = value;
		}
		return nurbsCurve;
	}

	private static Point3d HeadIntersect(Curve headCrv, Curve intersectCrv, Point3d closestPt)
	{
		CurveIntersections curveIntersections = Intersection.CurveCurve(headCrv, intersectCrv, CSApplicationHelper.Application.Tolerance, CSApplicationHelper.Application.Tolerance);
		if (curveIntersections.Count == 0)
		{
			return Point3d.Unset;
		}
		double num = 999.0;
		Point3d result = Point3d.Unset;
		foreach (IntersectionEvent item in curveIntersections)
		{
			double length = (closestPt - item.PointA).Length;
			if (length < num)
			{
				num = length;
				result = item.PointA;
			}
		}
		return result;
	}

	private static bool RevolveStartEnd(Curve bezelProngOutline1, CSGemData copyGem, Curve headBase, out Curve startCrv, out Curve endCrv, out string error)
	{
		startCrv = null;
		endCrv = null;
		if (!GemOffsetCurve.GemOffset(copyGem.GemShape, copyGem, 0.0, out Curve curve, false, out error, 0.0, 0.0, 0.0))
		{
			error = "CurveTop Inside:" + error;
			return false;
		}
		NurbsCurve nurbsCurve = bezelProngOutline1.ToNurbsCurve();
		Point3d point3d = copyGem.GetCurve().PointAt(copyGem.BezelParam);
		BoundingBox boundingBox = headBase.GetBoundingBox(accurate: true);
		double z = boundingBox.Center.Z;
		double gemXSize = copyGem.GemXSize;
		double gemYSize = copyGem.GemYSize;
		double x = boundingBox.Diagonal.X;
		double y = boundingBox.Diagonal.Y;
		double num = gemXSize - x;
		double num2 = gemYSize - y;
		for (int i = 0; i < 2; i++)
		{
			Point3d point3d2 = ((i == 0) ? nurbsCurve.PointAtStart : nurbsCurve.PointAtEnd);
			double num3 = point3d2.Z / z;
			double num4 = (gemXSize - num * num3) / gemXSize;
			double num5 = (gemYSize - num2 * num3) / gemYSize;
			Transform transform = Transform.Scale(Plane.WorldXY, num4, num5, 1.0);
			Transform transform2 = Transform.Scale(Plane.WorldXY, 1.0 / num4, 1.0 / num5, 1.0);
			copyGem.ApplyTransform(transform);
			double distance = point3d2.X - point3d.X + num / 2.0 * num3;
			if (copyGem.GemShape == "BaguetteTapered")
			{
				distance = -1.0 * (point3d2.Y - point3d.Y) + num2 / 2.0 * num3;
			}
			if (!GemOffsetCurve.GemOffset(copyGem.GemShape, copyGem, distance, out curve, false, out error, 0.0, 0.0, 0.0))
			{
				error = "CurveTop Inside:" + error;
				return false;
			}
			copyGem.ApplyTransform(transform2);
			NurbsCurve nurbsCurve2 = curve.ToNurbsCurve();
			nurbsCurve2.Translate(Vector3d.ZAxis * point3d2.Z);
			if (i == 0)
			{
				startCrv = nurbsCurve2;
			}
			else
			{
				endCrv = nurbsCurve2;
			}
		}
		error = "";
		return true;
	}

	private static Curve TrimToPoints(Curve crv, Point3d p0, Point3d p1)
	{
		Curve curve = crv.DuplicateCurve();
		curve.ClosestPoint(p0, out var t);
		curve.ChangeClosedCurveSeam(t);
		curve.ClosestPoint(p1, out t);
		return curve.Trim(curve.Domain.Min, t);
	}
}
