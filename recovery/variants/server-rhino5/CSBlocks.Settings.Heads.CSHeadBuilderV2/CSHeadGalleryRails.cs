using System;
using System.Collections.Generic;
using CSBlockDataUtils.CSGemOffsetCurves;
using CSBlockUtil;
using CSDatas;
using Rhino.Geometry;
using Rhino.Geometry.Intersect;

namespace CSBlocks.Settings.Heads.CSHeadBuilderV2;

internal static class CSHeadGalleryRails
{
	public static bool GalleryRail(CSGemData gem, RailParam railParam, Curve headSideX, Curve headSideY, bool enableModeDoItYourSelf, double cornerBlend, Point3d focalPoint)
	{
		railParam.RailObjects = new List<Brep>();
		railParam.RailCurves = new List<Curve>();
		Curve curve = railParam.RailProfile.DuplicateCurve();
		if (curve == null)
		{
			return false;
		}
		if (headSideX == null)
		{
			return false;
		}
		if (headSideY == null)
		{
			return false;
		}
		double num = headSideX.PointAtEnd.Z * (1.0 - railParam.RailPos);
		if (railParam.RailUpperToggle)
		{
			num = Point3d.Origin.Z - railParam.RailY / 2.0 - railParam.RailPos;
		}
		if (gem == null)
		{
			return false;
		}
		CSBlockUtil.CSUtilMath.GetUnscaledXforms(gem.Xform, out var xUp, out var xDown);
		CurveIntersections curveIntersections = Intersection.CurveCurve(new LineCurve(new Point3d(0.0, 0.0, num), new Point3d(100.0, 0.0, num)), headSideX, 0.01, 0.01);
		if (curveIntersections.Count <= 0)
		{
			return false;
		}
		CurveIntersections curveIntersections2 = Intersection.CurveCurve(new LineCurve(new Point3d(0.0, 0.0, num), new Point3d(0.0, 100.0, num)), headSideY, 0.01, 0.01);
		if (curveIntersections2.Count <= 0)
		{
			return false;
		}
		double x = curveIntersections[0].PointA.X;
		double y = curveIntersections2[0].PointA.Y;
		double parameterB = curveIntersections[0].ParameterB;
		double num2 = (0.0 - headSideX.TangentAt(parameterB).X) * railParam.RailShearMultiplier;
		Transform transform = Transform.Scale(Plane.WorldXY, x / railParam.RailGem.GemXMax, y / railParam.RailGem.GemYMax, 1.0);
		CSGemData cSGemData = new CSGemData(railParam.RailGem);
		cSGemData.ApplyTransform(xDown);
		cSGemData.ApplyTransform(transform);
		if (!GemOffsetCurve.GemOffset(cSGemData, railParam.RailOffset, out Curve curve2, false, out string _, cornerBlend))
		{
			return false;
		}
		if (curve2 == null)
		{
			return false;
		}
		if (!GemOffsetCurve.GemOffset(cSGemData, railParam.RailOffset + (0.0 - railParam.RailX), out Curve curve3, false, out string _, 0.0))
		{
			return false;
		}
		if (curve3 == null)
		{
			return false;
		}
		Transform xform = Transform.Translation(0.0, 0.0, num);
		Point3d testPoint = cSGemData.GetCurve().PointAt(railParam.RailGem.BezelParam);
		curve2.ClosestPoint(testPoint, out var t);
		Point3d point3d = curve2.PointAt(t);
		Vector3d vector3d = Vector3d.CrossProduct(curve2.TangentAt(t), Vector3d.ZAxis);
		vector3d.Unitize();
		vector3d *= railParam.RailX;
		double num3 = railParam.RailOffset + (0.0 - railParam.RailX);
		double t2;
		if (num3 < 0.0 && (Math.Abs(num3) > cSGemData.GemXSize / 2.0 || Math.Abs(num3) > cSGemData.GemYSize / 2.0))
		{
			curve3.ClosestPoint(point3d - vector3d, out t2);
		}
		else
		{
			curve3.ClosestPoint(point3d + vector3d, out t2);
		}
		NurbsCurve nurbsCurve = curve.ToNurbsCurve();
		BoundingBox boundingBox = nurbsCurve.GetBoundingBox(accurate: true);
		Transform transform2 = Transform.Translation(0.0 - boundingBox.Min.X, 0.0 - boundingBox.Min.Y, 0.0 - boundingBox.Min.Z);
		Transform transform3 = Transform.Scale(Plane.WorldXY, railParam.RailX / boundingBox.Diagonal.X, railParam.RailY / boundingBox.Diagonal.Y, 0.0);
		nurbsCurve.Transform(transform3 * transform2);
		for (int i = 0; i < nurbsCurve.Points.Count; i++)
		{
			ControlPoint value = nurbsCurve.Points[i];
			Point3d location = value.Location;
			location.X += num2 * location.Y;
			value.Location = location;
			nurbsCurve.Points[i] = value;
		}
		curve2.ClosestPoint(testPoint, out var t3);
		Point3d destination = curve2.PointAt(t3);
		Transform xform2 = CSBlockUtil.CSUtilMath.Orient3Point(new Point3d(railParam.RailX, 0.0, 0.0), new Vector3d(-1.0, 0.0, 0.0), new Vector3d(0.0, 1.0, 0.0), destination, vector3d, Vector3d.ZAxis);
		nurbsCurve.Transform(xform2);
		curve2.Transform(xform);
		curve3.Transform(xform);
		nurbsCurve.Transform(xform);
		if (focalPoint != Point3d.Unset && railParam.RailMove)
		{
			Point3d point3d2 = focalPoint;
			point3d2.Transform(xDown);
			double num4 = point3d2.X / point3d2.Z;
			double num5 = point3d2.Y / point3d2.Z;
			curve2.Transform(Transform.Translation(curve2.PointAtStart.Z * num4, curve2.PointAtStart.Z * num5, 0.0));
			curve3.Transform(Transform.Translation(curve2.PointAtStart.Z * num4, curve2.PointAtStart.Z * num5, 0.0));
			nurbsCurve.Transform(Transform.Translation(curve2.PointAtStart.Z * num4, curve2.PointAtStart.Z * num5, 0.0));
		}
		curve2.Transform(xUp);
		curve3.Transform(xUp);
		nurbsCurve.Transform(xUp);
		railParam.RailCurves.Add(curve2);
		railParam.RailCurves.Add(curve3);
		railParam.RailCurves.Add(nurbsCurve);
		if (!enableModeDoItYourSelf)
		{
			List<Curve> profile = new List<Curve> { nurbsCurve };
			List<double> parameters = new List<double> { t };
			List<double> parameters2 = new List<double> { t2 };
			if (!CSUtilSweepRC.Sweep2(curve2, curve3, profile, parameters, parameters2, out Brep breps, true, true))
			{
				return false;
			}
			railParam.RailObjects.Add(breps);
		}
		return true;
	}
}
