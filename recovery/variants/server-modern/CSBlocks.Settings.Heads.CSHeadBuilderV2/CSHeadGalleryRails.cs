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
		//IL_0032: Unknown result type (might be due to invalid IL or missing references)
		//IL_0037: Unknown result type (might be due to invalid IL or missing references)
		//IL_0059: Unknown result type (might be due to invalid IL or missing references)
		//IL_005e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0085: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a7: Unknown result type (might be due to invalid IL or missing references)
		//IL_00bf: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c4: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e1: Expected O, but got Unknown
		//IL_0102: Unknown result type (might be due to invalid IL or missing references)
		//IL_011a: Unknown result type (might be due to invalid IL or missing references)
		//IL_011f: Unknown result type (might be due to invalid IL or missing references)
		//IL_013c: Expected O, but got Unknown
		//IL_0152: Unknown result type (might be due to invalid IL or missing references)
		//IL_0157: Unknown result type (might be due to invalid IL or missing references)
		//IL_016a: Unknown result type (might be due to invalid IL or missing references)
		//IL_016f: Unknown result type (might be due to invalid IL or missing references)
		//IL_018c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0191: Unknown result type (might be due to invalid IL or missing references)
		//IL_01a4: Unknown result type (might be due to invalid IL or missing references)
		//IL_01ce: Unknown result type (might be due to invalid IL or missing references)
		//IL_01d3: Unknown result type (might be due to invalid IL or missing references)
		//IL_01e4: Unknown result type (might be due to invalid IL or missing references)
		//IL_01ed: Unknown result type (might be due to invalid IL or missing references)
		//IL_0253: Unknown result type (might be due to invalid IL or missing references)
		//IL_0258: Unknown result type (might be due to invalid IL or missing references)
		//IL_026c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0271: Unknown result type (might be due to invalid IL or missing references)
		//IL_0275: Unknown result type (might be due to invalid IL or missing references)
		//IL_0283: Unknown result type (might be due to invalid IL or missing references)
		//IL_0288: Unknown result type (might be due to invalid IL or missing references)
		//IL_028e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0293: Unknown result type (might be due to invalid IL or missing references)
		//IL_0298: Unknown result type (might be due to invalid IL or missing references)
		//IL_029d: Unknown result type (might be due to invalid IL or missing references)
		//IL_02a7: Unknown result type (might be due to invalid IL or missing references)
		//IL_02af: Unknown result type (might be due to invalid IL or missing references)
		//IL_02b4: Unknown result type (might be due to invalid IL or missing references)
		//IL_031e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0320: Unknown result type (might be due to invalid IL or missing references)
		//IL_0322: Unknown result type (might be due to invalid IL or missing references)
		//IL_033a: Unknown result type (might be due to invalid IL or missing references)
		//IL_033f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0343: Unknown result type (might be due to invalid IL or missing references)
		//IL_0348: Unknown result type (might be due to invalid IL or missing references)
		//IL_0354: Unknown result type (might be due to invalid IL or missing references)
		//IL_0359: Unknown result type (might be due to invalid IL or missing references)
		//IL_0365: Unknown result type (might be due to invalid IL or missing references)
		//IL_036a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0374: Unknown result type (might be due to invalid IL or missing references)
		//IL_0379: Unknown result type (might be due to invalid IL or missing references)
		//IL_037b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0388: Unknown result type (might be due to invalid IL or missing references)
		//IL_038d: Unknown result type (might be due to invalid IL or missing references)
		//IL_039f: Unknown result type (might be due to invalid IL or missing references)
		//IL_03a4: Unknown result type (might be due to invalid IL or missing references)
		//IL_03b7: Unknown result type (might be due to invalid IL or missing references)
		//IL_03bc: Unknown result type (might be due to invalid IL or missing references)
		//IL_03c0: Unknown result type (might be due to invalid IL or missing references)
		//IL_03c2: Unknown result type (might be due to invalid IL or missing references)
		//IL_03c4: Unknown result type (might be due to invalid IL or missing references)
		//IL_0309: Unknown result type (might be due to invalid IL or missing references)
		//IL_030b: Unknown result type (might be due to invalid IL or missing references)
		//IL_030d: Unknown result type (might be due to invalid IL or missing references)
		//IL_03dd: Unknown result type (might be due to invalid IL or missing references)
		//IL_03e2: Unknown result type (might be due to invalid IL or missing references)
		//IL_03e6: Unknown result type (might be due to invalid IL or missing references)
		//IL_03eb: Unknown result type (might be due to invalid IL or missing references)
		//IL_0408: Unknown result type (might be due to invalid IL or missing references)
		//IL_0418: Unknown result type (might be due to invalid IL or missing references)
		//IL_0437: Unknown result type (might be due to invalid IL or missing references)
		//IL_0445: Unknown result type (might be due to invalid IL or missing references)
		//IL_044a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0464: Unknown result type (might be due to invalid IL or missing references)
		//IL_0484: Unknown result type (might be due to invalid IL or missing references)
		//IL_04a4: Unknown result type (might be due to invalid IL or missing references)
		//IL_04a9: Unknown result type (might be due to invalid IL or missing references)
		//IL_04ab: Unknown result type (might be due to invalid IL or missing references)
		//IL_04ad: Unknown result type (might be due to invalid IL or missing references)
		//IL_04b2: Unknown result type (might be due to invalid IL or missing references)
		//IL_04b7: Unknown result type (might be due to invalid IL or missing references)
		//IL_04bb: Unknown result type (might be due to invalid IL or missing references)
		//IL_04c5: Unknown result type (might be due to invalid IL or missing references)
		//IL_04cf: Unknown result type (might be due to invalid IL or missing references)
		//IL_04d9: Unknown result type (might be due to invalid IL or missing references)
		//IL_04e1: Unknown result type (might be due to invalid IL or missing references)
		//IL_04e3: Unknown result type (might be due to invalid IL or missing references)
		//IL_05e1: Unknown result type (might be due to invalid IL or missing references)
		//IL_05eb: Unknown result type (might be due to invalid IL or missing references)
		//IL_05f5: Unknown result type (might be due to invalid IL or missing references)
		//IL_04fd: Unknown result type (might be due to invalid IL or missing references)
		//IL_04ff: Unknown result type (might be due to invalid IL or missing references)
		//IL_0503: Unknown result type (might be due to invalid IL or missing references)
		//IL_052f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0534: Unknown result type (might be due to invalid IL or missing references)
		//IL_0542: Unknown result type (might be due to invalid IL or missing references)
		//IL_0547: Unknown result type (might be due to invalid IL or missing references)
		//IL_055c: Unknown result type (might be due to invalid IL or missing references)
		//IL_056b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0570: Unknown result type (might be due to invalid IL or missing references)
		//IL_057e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0583: Unknown result type (might be due to invalid IL or missing references)
		//IL_0598: Unknown result type (might be due to invalid IL or missing references)
		//IL_05a7: Unknown result type (might be due to invalid IL or missing references)
		//IL_05ac: Unknown result type (might be due to invalid IL or missing references)
		//IL_05ba: Unknown result type (might be due to invalid IL or missing references)
		//IL_05bf: Unknown result type (might be due to invalid IL or missing references)
		//IL_05d4: Unknown result type (might be due to invalid IL or missing references)
		railParam.RailObjects = new List<Brep>();
		railParam.RailCurves = new List<Curve>();
		Curve val = railParam.RailProfile.DuplicateCurve();
		if (val == null)
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
		Point3d val2 = headSideX.PointAtEnd;
		double num = ((Point3d)(ref val2)).Z * (1.0 - railParam.RailPos);
		if (railParam.RailUpperToggle)
		{
			val2 = Point3d.Origin;
			num = ((Point3d)(ref val2)).Z - railParam.RailY / 2.0 - railParam.RailPos;
		}
		if (gem == null)
		{
			return false;
		}
		CSBlockUtil.CSUtilMath.GetUnscaledXforms(gem.Xform, out var xUp, out var xDown);
		CurveIntersections val3 = Intersection.CurveCurve((Curve)new LineCurve(new Point3d(0.0, 0.0, num), new Point3d(100.0, 0.0, num)), headSideX, 0.01, 0.01);
		if (val3.Count <= 0)
		{
			return false;
		}
		CurveIntersections val4 = Intersection.CurveCurve((Curve)new LineCurve(new Point3d(0.0, 0.0, num), new Point3d(0.0, 100.0, num)), headSideY, 0.01, 0.01);
		if (val4.Count <= 0)
		{
			return false;
		}
		Point3d val5 = val3[0].PointA;
		double x = ((Point3d)(ref val5)).X;
		val5 = val4[0].PointA;
		double y = ((Point3d)(ref val5)).Y;
		double parameterB = val3[0].ParameterB;
		Vector3d val6 = headSideX.TangentAt(parameterB);
		double num2 = (0.0 - ((Vector3d)(ref val6)).X) * railParam.RailShearMultiplier;
		Transform transform = Transform.Scale(Plane.WorldXY, x / railParam.RailGem.GemXMax, y / railParam.RailGem.GemYMax, 1.0);
		CSGemData cSGemData = new CSGemData(railParam.RailGem);
		cSGemData.ApplyTransform(xDown);
		cSGemData.ApplyTransform(transform);
		if (!GemOffsetCurve.GemOffset(cSGemData, railParam.RailOffset, out Curve curve, false, out string _, cornerBlend))
		{
			return false;
		}
		if (curve == null)
		{
			return false;
		}
		if (!GemOffsetCurve.GemOffset(cSGemData, railParam.RailOffset + (0.0 - railParam.RailX), out Curve curve2, false, out string _, 0.0))
		{
			return false;
		}
		if (curve2 == null)
		{
			return false;
		}
		Transform val7 = Transform.Translation(0.0, 0.0, num);
		Point3d val8 = cSGemData.GetCurve().PointAt(railParam.RailGem.BezelParam);
		double num3 = default(double);
		curve.ClosestPoint(val8, ref num3);
		Point3d val9 = curve.PointAt(num3);
		Vector3d val10 = Vector3d.CrossProduct(curve.TangentAt(num3), Vector3d.ZAxis);
		((Vector3d)(ref val10)).Unitize();
		val10 *= railParam.RailX;
		double num4 = railParam.RailOffset + (0.0 - railParam.RailX);
		double item = default(double);
		if (num4 < 0.0 && (Math.Abs(num4) > cSGemData.GemXSize / 2.0 || Math.Abs(num4) > cSGemData.GemYSize / 2.0))
		{
			curve2.ClosestPoint(val9 - val10, ref item);
		}
		else
		{
			curve2.ClosestPoint(val9 + val10, ref item);
		}
		NurbsCurve val11 = val.ToNurbsCurve();
		BoundingBox boundingBox = ((GeometryBase)val11).GetBoundingBox(true);
		val5 = ((BoundingBox)(ref boundingBox)).Min;
		double num5 = 0.0 - ((Point3d)(ref val5)).X;
		val5 = ((BoundingBox)(ref boundingBox)).Min;
		double num6 = 0.0 - ((Point3d)(ref val5)).Y;
		val5 = ((BoundingBox)(ref boundingBox)).Min;
		Transform val12 = Transform.Translation(num5, num6, 0.0 - ((Point3d)(ref val5)).Z);
		Plane worldXY = Plane.WorldXY;
		double railX = railParam.RailX;
		Vector3d diagonal = ((BoundingBox)(ref boundingBox)).Diagonal;
		double num7 = railX / ((Vector3d)(ref diagonal)).X;
		double railY = railParam.RailY;
		diagonal = ((BoundingBox)(ref boundingBox)).Diagonal;
		Transform val13 = Transform.Scale(worldXY, num7, railY / ((Vector3d)(ref diagonal)).Y, 0.0);
		((GeometryBase)val11).Transform(val13 * val12);
		for (int i = 0; i < val11.Points.Count; i++)
		{
			ControlPoint val14 = val11.Points[i];
			Point3d location = ((ControlPoint)(ref val14)).Location;
			((Point3d)(ref location)).X = ((Point3d)(ref location)).X + num2 * ((Point3d)(ref location)).Y;
			((ControlPoint)(ref val14)).Location = location;
			val11.Points[i] = val14;
		}
		double num8 = default(double);
		curve.ClosestPoint(val8, ref num8);
		Point3d destination = curve.PointAt(num8);
		Transform val15 = CSBlockUtil.CSUtilMath.Orient3Point(new Point3d(railParam.RailX, 0.0, 0.0), new Vector3d(-1.0, 0.0, 0.0), new Vector3d(0.0, 1.0, 0.0), destination, val10, Vector3d.ZAxis);
		((GeometryBase)val11).Transform(val15);
		((GeometryBase)curve).Transform(val7);
		((GeometryBase)curve2).Transform(val7);
		((GeometryBase)val11).Transform(val7);
		if (focalPoint != Point3d.Unset && railParam.RailMove)
		{
			Point3d val16 = focalPoint;
			((Point3d)(ref val16)).Transform(xDown);
			double num9 = ((Point3d)(ref val16)).X / ((Point3d)(ref val16)).Z;
			double num10 = ((Point3d)(ref val16)).Y / ((Point3d)(ref val16)).Z;
			Curve obj = curve;
			val5 = curve.PointAtStart;
			double num11 = ((Point3d)(ref val5)).Z * num9;
			val5 = curve.PointAtStart;
			((GeometryBase)obj).Transform(Transform.Translation(num11, ((Point3d)(ref val5)).Z * num10, 0.0));
			Curve obj2 = curve2;
			val5 = curve.PointAtStart;
			double num12 = ((Point3d)(ref val5)).Z * num9;
			val5 = curve.PointAtStart;
			((GeometryBase)obj2).Transform(Transform.Translation(num12, ((Point3d)(ref val5)).Z * num10, 0.0));
			val5 = curve.PointAtStart;
			double num13 = ((Point3d)(ref val5)).Z * num9;
			val5 = curve.PointAtStart;
			((GeometryBase)val11).Transform(Transform.Translation(num13, ((Point3d)(ref val5)).Z * num10, 0.0));
		}
		((GeometryBase)curve).Transform(xUp);
		((GeometryBase)curve2).Transform(xUp);
		((GeometryBase)val11).Transform(xUp);
		railParam.RailCurves.Add(curve);
		railParam.RailCurves.Add(curve2);
		railParam.RailCurves.Add((Curve)(object)val11);
		if (!enableModeDoItYourSelf)
		{
			List<Curve> profile = new List<Curve> { (Curve)(object)val11 };
			List<double> parameters = new List<double> { num3 };
			List<double> parameters2 = new List<double> { item };
			if (!CSUtilSweepRC.Sweep2(curve, curve2, profile, parameters, parameters2, out Brep breps, true, true))
			{
				return false;
			}
			railParam.RailObjects.Add(breps);
		}
		return true;
	}
}
