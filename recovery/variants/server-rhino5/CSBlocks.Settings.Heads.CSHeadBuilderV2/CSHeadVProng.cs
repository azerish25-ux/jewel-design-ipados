using System;
using CS;
using CSBlockUtil;
using CSDatas;
using GV.CounterSketch.Shared;
using Rhino.Geometry;
using Rhino.Geometry.Intersect;

namespace CSBlocks.Settings.Heads.CSHeadBuilderV2;

internal static class CSHeadVProng
{
	public static bool VProng(int prongNumber, ProngParam prongParam, Curve headTop, Curve headBase, Curve bezelProngOutline, CSGemData copyGem, ValueListItem baseStyle, double cornerBlend, out Brep outProng, out string error)
	{
		outProng = null;
		if (!CSHeadBezel.RevolveBezel(bezelProngOutline, copyGem, headBase, cornerBlend, out var bezelSurface, out error))
		{
			return false;
		}
		Brep brep = bezelSurface.ToBrep();
		if (brep == null)
		{
			error = "BezelBuilder failed";
			return false;
		}
		brep = CSUtilRhino.ReJoinBrep(brep, CSApplicationHelper.Application.Tolerance);
		Point3d pointAtStart = prongParam.ProngRail.PointAtStart;
		Point3d pointAtEnd = prongParam.ProngRail.PointAtEnd;
		LineCurve lineCurve = new LineCurve(new Point3d(0.0, 0.0, pointAtStart.Z), new Point3d(pointAtStart.X * 2.0, pointAtStart.Y * 2.0, pointAtStart.Z));
		LineCurve lineCurve2 = new LineCurve(new Point3d(0.0, 0.0, pointAtStart.Z), new Point3d(pointAtStart.X * 2.0, pointAtStart.Y * 2.0, pointAtStart.Z));
		LineCurve lineCurve3 = new LineCurve(new Point3d(0.0, 0.0, pointAtEnd.Z), new Point3d(pointAtStart.X * 2.0, pointAtStart.Y * 2.0, pointAtEnd.Z));
		LineCurve lineCurve4 = new LineCurve(new Point3d(0.0, 0.0, pointAtEnd.Z), new Point3d(pointAtStart.X * 2.0, pointAtStart.Y * 2.0, pointAtEnd.Z));
		double length = headTop.GetLength();
		double num = headBase.GetLength();
		if (num < Math.PI * 2.0)
		{
			num = Math.PI * 2.0;
		}
		double num2 = prongParam.ProngTopX - prongParam.ProngTopBlend / length * 360.0;
		double num3 = prongParam.ProngBaseX - prongParam.ProngBaseBlend / num * 360.0;
		double radiusTop = prongParam.ProngTopBlend;
		double radiusBottom = prongParam.ProngBaseBlend;
		if (num2 < 0.5)
		{
			num2 = 0.5;
			radiusTop = (prongParam.ProngTopX - num2) * length / 360.0;
		}
		if (num3 < 0.5)
		{
			num3 = 0.5;
			radiusBottom = (prongParam.ProngBaseX - num3) * num / 360.0;
		}
		Transform xform = CSBlockUtil.CSUtilMath.Rotation(CSBlockUtil.CSUtilMath.ToRadians(num2), Vector3d.ZAxis, Point3d.Origin);
		lineCurve.Transform(xform);
		xform = CSBlockUtil.CSUtilMath.Rotation(CSBlockUtil.CSUtilMath.ToRadians(0.0 - num2), Vector3d.ZAxis, Point3d.Origin);
		lineCurve2.Transform(xform);
		xform = CSBlockUtil.CSUtilMath.Rotation(CSBlockUtil.CSUtilMath.ToRadians(num3), Vector3d.ZAxis, Point3d.Origin);
		lineCurve3.Transform(xform);
		xform = CSBlockUtil.CSUtilMath.Rotation(CSBlockUtil.CSUtilMath.ToRadians(0.0 - num3), Vector3d.ZAxis, Point3d.Origin);
		lineCurve4.Transform(xform);
		CurveIntersections curveIntersections = Intersection.CurveCurve(headTop, lineCurve, CSApplicationHelper.Application.Tolerance, CSApplicationHelper.Application.Tolerance);
		CurveIntersections curveIntersections2 = Intersection.CurveCurve(headTop, lineCurve2, CSApplicationHelper.Application.Tolerance, CSApplicationHelper.Application.Tolerance);
		CurveIntersections curveIntersections3 = Intersection.CurveCurve(headBase, lineCurve3, CSApplicationHelper.Application.Tolerance, CSApplicationHelper.Application.Tolerance);
		CurveIntersections curveIntersections4 = Intersection.CurveCurve(headBase, lineCurve4, CSApplicationHelper.Application.Tolerance, CSApplicationHelper.Application.Tolerance);
		if (curveIntersections.Count < 1 || curveIntersections2.Count < 1 || curveIntersections3.Count < 1 || curveIntersections4.Count < 1)
		{
			return false;
		}
		double num4 = prongParam.ProngTopNudge * prongParam.ProngTopY + 20.0;
		double num5 = prongParam.ProngBaseNudge * prongParam.ProngBaseY + 20.0;
		double num6 = 2.0;
		Vector3d vector3d = Vector3d.CrossProduct(headTop.TangentAt(curveIntersections[0].ParameterA), Vector3d.ZAxis);
		lineCurve = new LineCurve(curveIntersections[0].PointA + vector3d * num4, curveIntersections[0].PointA - vector3d * num6);
		vector3d = Vector3d.CrossProduct(headTop.TangentAt(curveIntersections2[0].ParameterA), Vector3d.ZAxis);
		lineCurve2 = new LineCurve(curveIntersections2[0].PointA + vector3d * num4, curveIntersections2[0].PointA - vector3d * num6);
		vector3d = Vector3d.CrossProduct(headBase.TangentAt(curveIntersections3[0].ParameterA), Vector3d.ZAxis);
		lineCurve3 = new LineCurve(curveIntersections3[0].PointA + vector3d * num5, curveIntersections3[0].PointA - vector3d * num6);
		vector3d = Vector3d.CrossProduct(headBase.TangentAt(curveIntersections4[0].ParameterA), Vector3d.ZAxis);
		lineCurve4 = new LineCurve(curveIntersections4[0].PointA + vector3d * num5, curveIntersections4[0].PointA - vector3d * num6);
		lineCurve.Transform(Transform.Translation(0.0, 0.0, prongParam.ProngDomeHeight + 0.1));
		lineCurve2.Transform(Transform.Translation(0.0, 0.0, prongParam.ProngDomeHeight + 0.1));
		lineCurve3.Transform(Transform.Translation(0.0, 0.0, -0.1));
		lineCurve4.Transform(Transform.Translation(0.0, 0.0, -0.1));
		Curve prongRail = prongParam.ProngRail;
		Curve[] profiles = new Curve[2] { lineCurve, lineCurve3 };
		Curve[] profiles2 = new Curve[2] { lineCurve2, lineCurve4 };
		double[] parameters = new double[2]
		{
			prongRail.Domain.Min,
			prongRail.Domain.Max
		};
		CSUtilSweepRC.Sweep1(prongRail, profiles, parameters, out var breps, closed: false, globalShapeBlend: false);
		NurbsSurface nurbsSurface = breps[0].Faces[0].ToNurbsSurface();
		CSUtilSweepRC.Sweep1(prongRail, profiles2, parameters, out var breps2, closed: false, globalShapeBlend: false);
		NurbsSurface nurbsSurface2 = breps2[0].Faces[0].ToNurbsSurface();
		nurbsSurface2 = (NurbsSurface)nurbsSurface2.Reverse(0);
		Brep vProng = SplitBrep(brep, nurbsSurface, nurbsSurface2, pointAtStart);
		outProng = CSHeadVProngDome.VProngDome(prongNumber, vProng, headTop.GetBoundingBox(accurate: true).Center.Z, headBase.GetBoundingBox(accurate: true).Center.Z + prongParam.ProngBaseDomeHeight, nurbsSurface, nurbsSurface2, radiusTop, radiusBottom);
		return true;
	}

	private static Brep SplitBrep(Brep vProngBezel, Surface intersectSrf0, Surface intersectSrf1, Point3d topProfilePoint)
	{
		Brep[] array = vProngBezel.Split(intersectSrf0.ToBrep(), CSApplicationHelper.Application.Tolerance);
		Brep brep = null;
		double num = 999.0;
		Brep[] array2 = array;
		foreach (Brep brep2 in array2)
		{
			double length = (brep2.ClosestPoint(topProfilePoint) - topProfilePoint).Length;
			if (length < num)
			{
				num = length;
				brep = brep2;
			}
		}
		Brep[] array3 = brep.Split(intersectSrf1.ToBrep(), CSApplicationHelper.Application.Tolerance);
		brep = null;
		num = 999.0;
		array2 = array3;
		foreach (Brep brep3 in array2)
		{
			double length2 = (brep3.ClosestPoint(topProfilePoint) - topProfilePoint).Length;
			if (length2 < num)
			{
				num = length2;
				brep = brep3;
			}
		}
		return brep;
	}
}
