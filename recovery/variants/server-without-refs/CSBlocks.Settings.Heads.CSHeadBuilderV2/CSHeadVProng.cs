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
		//IL_0043: Unknown result type (might be due to invalid IL or missing references)
		//IL_0048: Unknown result type (might be due to invalid IL or missing references)
		//IL_004f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0054: Unknown result type (might be due to invalid IL or missing references)
		//IL_006e: Unknown result type (might be due to invalid IL or missing references)
		//IL_009c: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a1: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a8: Expected O, but got Unknown
		//IL_00c1: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ef: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f4: Unknown result type (might be due to invalid IL or missing references)
		//IL_00fb: Expected O, but got Unknown
		//IL_0114: Unknown result type (might be due to invalid IL or missing references)
		//IL_0142: Unknown result type (might be due to invalid IL or missing references)
		//IL_0147: Unknown result type (might be due to invalid IL or missing references)
		//IL_014e: Expected O, but got Unknown
		//IL_0167: Unknown result type (might be due to invalid IL or missing references)
		//IL_0195: Unknown result type (might be due to invalid IL or missing references)
		//IL_019a: Unknown result type (might be due to invalid IL or missing references)
		//IL_01a1: Expected O, but got Unknown
		//IL_0278: Unknown result type (might be due to invalid IL or missing references)
		//IL_027d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0282: Unknown result type (might be due to invalid IL or missing references)
		//IL_0287: Unknown result type (might be due to invalid IL or missing references)
		//IL_028b: Unknown result type (might be due to invalid IL or missing references)
		//IL_029b: Unknown result type (might be due to invalid IL or missing references)
		//IL_02a0: Unknown result type (might be due to invalid IL or missing references)
		//IL_02a5: Unknown result type (might be due to invalid IL or missing references)
		//IL_02aa: Unknown result type (might be due to invalid IL or missing references)
		//IL_02ae: Unknown result type (might be due to invalid IL or missing references)
		//IL_02bd: Unknown result type (might be due to invalid IL or missing references)
		//IL_02c2: Unknown result type (might be due to invalid IL or missing references)
		//IL_02c7: Unknown result type (might be due to invalid IL or missing references)
		//IL_02cc: Unknown result type (might be due to invalid IL or missing references)
		//IL_02d0: Unknown result type (might be due to invalid IL or missing references)
		//IL_02e0: Unknown result type (might be due to invalid IL or missing references)
		//IL_02e5: Unknown result type (might be due to invalid IL or missing references)
		//IL_02ea: Unknown result type (might be due to invalid IL or missing references)
		//IL_02ef: Unknown result type (might be due to invalid IL or missing references)
		//IL_02f3: Unknown result type (might be due to invalid IL or missing references)
		//IL_03e8: Unknown result type (might be due to invalid IL or missing references)
		//IL_03ed: Unknown result type (might be due to invalid IL or missing references)
		//IL_03f2: Unknown result type (might be due to invalid IL or missing references)
		//IL_03f7: Unknown result type (might be due to invalid IL or missing references)
		//IL_0401: Unknown result type (might be due to invalid IL or missing references)
		//IL_0406: Unknown result type (might be due to invalid IL or missing references)
		//IL_040a: Unknown result type (might be due to invalid IL or missing references)
		//IL_040f: Unknown result type (might be due to invalid IL or missing references)
		//IL_041c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0421: Unknown result type (might be due to invalid IL or missing references)
		//IL_0425: Unknown result type (might be due to invalid IL or missing references)
		//IL_042a: Unknown result type (might be due to invalid IL or missing references)
		//IL_042f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0436: Expected O, but got Unknown
		//IL_0444: Unknown result type (might be due to invalid IL or missing references)
		//IL_0449: Unknown result type (might be due to invalid IL or missing references)
		//IL_044e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0453: Unknown result type (might be due to invalid IL or missing references)
		//IL_045d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0462: Unknown result type (might be due to invalid IL or missing references)
		//IL_0466: Unknown result type (might be due to invalid IL or missing references)
		//IL_046b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0478: Unknown result type (might be due to invalid IL or missing references)
		//IL_047d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0481: Unknown result type (might be due to invalid IL or missing references)
		//IL_0486: Unknown result type (might be due to invalid IL or missing references)
		//IL_048b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0492: Expected O, but got Unknown
		//IL_04a0: Unknown result type (might be due to invalid IL or missing references)
		//IL_04a5: Unknown result type (might be due to invalid IL or missing references)
		//IL_04aa: Unknown result type (might be due to invalid IL or missing references)
		//IL_04af: Unknown result type (might be due to invalid IL or missing references)
		//IL_04b9: Unknown result type (might be due to invalid IL or missing references)
		//IL_04be: Unknown result type (might be due to invalid IL or missing references)
		//IL_04c2: Unknown result type (might be due to invalid IL or missing references)
		//IL_04c7: Unknown result type (might be due to invalid IL or missing references)
		//IL_04d4: Unknown result type (might be due to invalid IL or missing references)
		//IL_04d9: Unknown result type (might be due to invalid IL or missing references)
		//IL_04dd: Unknown result type (might be due to invalid IL or missing references)
		//IL_04e2: Unknown result type (might be due to invalid IL or missing references)
		//IL_04e7: Unknown result type (might be due to invalid IL or missing references)
		//IL_04ee: Expected O, but got Unknown
		//IL_04fc: Unknown result type (might be due to invalid IL or missing references)
		//IL_0501: Unknown result type (might be due to invalid IL or missing references)
		//IL_0506: Unknown result type (might be due to invalid IL or missing references)
		//IL_050b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0515: Unknown result type (might be due to invalid IL or missing references)
		//IL_051a: Unknown result type (might be due to invalid IL or missing references)
		//IL_051e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0523: Unknown result type (might be due to invalid IL or missing references)
		//IL_0530: Unknown result type (might be due to invalid IL or missing references)
		//IL_0535: Unknown result type (might be due to invalid IL or missing references)
		//IL_0539: Unknown result type (might be due to invalid IL or missing references)
		//IL_053e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0543: Unknown result type (might be due to invalid IL or missing references)
		//IL_054a: Expected O, but got Unknown
		//IL_056e: Unknown result type (might be due to invalid IL or missing references)
		//IL_059d: Unknown result type (might be due to invalid IL or missing references)
		//IL_05c5: Unknown result type (might be due to invalid IL or missing references)
		//IL_05ed: Unknown result type (might be due to invalid IL or missing references)
		//IL_062e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0633: Unknown result type (might be due to invalid IL or missing references)
		//IL_0641: Unknown result type (might be due to invalid IL or missing references)
		//IL_0646: Unknown result type (might be due to invalid IL or missing references)
		//IL_06a6: Unknown result type (might be due to invalid IL or missing references)
		//IL_06ad: Expected O, but got Unknown
		//IL_06b2: Unknown result type (might be due to invalid IL or missing references)
		//IL_06c1: Unknown result type (might be due to invalid IL or missing references)
		//IL_06c6: Unknown result type (might be due to invalid IL or missing references)
		//IL_06ca: Unknown result type (might be due to invalid IL or missing references)
		//IL_06cf: Unknown result type (might be due to invalid IL or missing references)
		//IL_06da: Unknown result type (might be due to invalid IL or missing references)
		//IL_06df: Unknown result type (might be due to invalid IL or missing references)
		//IL_06e3: Unknown result type (might be due to invalid IL or missing references)
		//IL_06e8: Unknown result type (might be due to invalid IL or missing references)
		outProng = null;
		if (!CSHeadBezel.RevolveBezel(bezelProngOutline, copyGem, headBase, cornerBlend, out var bezelSurface, out error))
		{
			return false;
		}
		Brep val = ((Surface)bezelSurface).ToBrep();
		if (val == null)
		{
			error = "BezelBuilder failed";
			return false;
		}
		val = CSUtilRhino.ReJoinBrep(val, CSApplicationHelper.Application.Tolerance);
		Point3d pointAtStart = prongParam.ProngRail.PointAtStart;
		Point3d pointAtEnd = prongParam.ProngRail.PointAtEnd;
		LineCurve val2 = new LineCurve(new Point3d(0.0, 0.0, ((Point3d)(ref pointAtStart)).Z), new Point3d(((Point3d)(ref pointAtStart)).X * 2.0, ((Point3d)(ref pointAtStart)).Y * 2.0, ((Point3d)(ref pointAtStart)).Z));
		LineCurve val3 = new LineCurve(new Point3d(0.0, 0.0, ((Point3d)(ref pointAtStart)).Z), new Point3d(((Point3d)(ref pointAtStart)).X * 2.0, ((Point3d)(ref pointAtStart)).Y * 2.0, ((Point3d)(ref pointAtStart)).Z));
		LineCurve val4 = new LineCurve(new Point3d(0.0, 0.0, ((Point3d)(ref pointAtEnd)).Z), new Point3d(((Point3d)(ref pointAtStart)).X * 2.0, ((Point3d)(ref pointAtStart)).Y * 2.0, ((Point3d)(ref pointAtEnd)).Z));
		LineCurve val5 = new LineCurve(new Point3d(0.0, 0.0, ((Point3d)(ref pointAtEnd)).Z), new Point3d(((Point3d)(ref pointAtStart)).X * 2.0, ((Point3d)(ref pointAtStart)).Y * 2.0, ((Point3d)(ref pointAtEnd)).Z));
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
		Transform val6 = CSBlockUtil.CSUtilMath.Rotation(CSBlockUtil.CSUtilMath.ToRadians(num2), Vector3d.ZAxis, Point3d.Origin);
		((GeometryBase)val2).Transform(val6);
		val6 = CSBlockUtil.CSUtilMath.Rotation(CSBlockUtil.CSUtilMath.ToRadians(0.0 - num2), Vector3d.ZAxis, Point3d.Origin);
		((GeometryBase)val3).Transform(val6);
		val6 = CSBlockUtil.CSUtilMath.Rotation(CSBlockUtil.CSUtilMath.ToRadians(num3), Vector3d.ZAxis, Point3d.Origin);
		((GeometryBase)val4).Transform(val6);
		val6 = CSBlockUtil.CSUtilMath.Rotation(CSBlockUtil.CSUtilMath.ToRadians(0.0 - num3), Vector3d.ZAxis, Point3d.Origin);
		((GeometryBase)val5).Transform(val6);
		CurveIntersections val7 = Intersection.CurveCurve(headTop, (Curve)(object)val2, CSApplicationHelper.Application.Tolerance, CSApplicationHelper.Application.Tolerance);
		CurveIntersections val8 = Intersection.CurveCurve(headTop, (Curve)(object)val3, CSApplicationHelper.Application.Tolerance, CSApplicationHelper.Application.Tolerance);
		CurveIntersections val9 = Intersection.CurveCurve(headBase, (Curve)(object)val4, CSApplicationHelper.Application.Tolerance, CSApplicationHelper.Application.Tolerance);
		CurveIntersections val10 = Intersection.CurveCurve(headBase, (Curve)(object)val5, CSApplicationHelper.Application.Tolerance, CSApplicationHelper.Application.Tolerance);
		if (val7.Count < 1 || val8.Count < 1 || val9.Count < 1 || val10.Count < 1)
		{
			return false;
		}
		double num4 = prongParam.ProngTopNudge * prongParam.ProngTopY + 20.0;
		double num5 = prongParam.ProngBaseNudge * prongParam.ProngBaseY + 20.0;
		double num6 = 2.0;
		Vector3d val11 = Vector3d.CrossProduct(headTop.TangentAt(val7[0].ParameterA), Vector3d.ZAxis);
		val2 = new LineCurve(val7[0].PointA + val11 * num4, val7[0].PointA - val11 * num6);
		val11 = Vector3d.CrossProduct(headTop.TangentAt(val8[0].ParameterA), Vector3d.ZAxis);
		val3 = new LineCurve(val8[0].PointA + val11 * num4, val8[0].PointA - val11 * num6);
		val11 = Vector3d.CrossProduct(headBase.TangentAt(val9[0].ParameterA), Vector3d.ZAxis);
		val4 = new LineCurve(val9[0].PointA + val11 * num5, val9[0].PointA - val11 * num6);
		val11 = Vector3d.CrossProduct(headBase.TangentAt(val10[0].ParameterA), Vector3d.ZAxis);
		val5 = new LineCurve(val10[0].PointA + val11 * num5, val10[0].PointA - val11 * num6);
		((GeometryBase)val2).Transform(Transform.Translation(0.0, 0.0, prongParam.ProngDomeHeight + 0.1));
		((GeometryBase)val3).Transform(Transform.Translation(0.0, 0.0, prongParam.ProngDomeHeight + 0.1));
		((GeometryBase)val4).Transform(Transform.Translation(0.0, 0.0, -0.1));
		((GeometryBase)val5).Transform(Transform.Translation(0.0, 0.0, -0.1));
		Curve prongRail = prongParam.ProngRail;
		Curve[] profiles = (Curve[])(object)new Curve[2]
		{
			(Curve)val2,
			(Curve)val4
		};
		Curve[] profiles2 = (Curve[])(object)new Curve[2]
		{
			(Curve)val3,
			(Curve)val5
		};
		double[] array = new double[2];
		Interval domain = prongRail.Domain;
		array[0] = ((Interval)(ref domain)).Min;
		domain = prongRail.Domain;
		array[1] = ((Interval)(ref domain)).Max;
		double[] parameters = array;
		CSUtilSweepRC.Sweep1(prongRail, profiles, parameters, out var breps, closed: false, globalShapeBlend: false);
		NurbsSurface val12 = ((Surface)breps[0].Faces[0]).ToNurbsSurface();
		CSUtilSweepRC.Sweep1(prongRail, profiles2, parameters, out var breps2, closed: false, globalShapeBlend: false);
		NurbsSurface val13 = ((Surface)breps2[0].Faces[0]).ToNurbsSurface();
		val13 = (NurbsSurface)((Surface)val13).Reverse(0);
		Brep vProng = SplitBrep(val, (Surface)(object)val12, (Surface)(object)val13, pointAtStart);
		BoundingBox boundingBox = ((GeometryBase)headTop).GetBoundingBox(true);
		Point3d center = ((BoundingBox)(ref boundingBox)).Center;
		double z = ((Point3d)(ref center)).Z;
		boundingBox = ((GeometryBase)headBase).GetBoundingBox(true);
		center = ((BoundingBox)(ref boundingBox)).Center;
		outProng = CSHeadVProngDome.VProngDome(prongNumber, vProng, z, ((Point3d)(ref center)).Z + prongParam.ProngBaseDomeHeight, val12, val13, radiusTop, radiusBottom);
		return true;
	}

	private static Brep SplitBrep(Brep vProngBezel, Surface intersectSrf0, Surface intersectSrf1, Point3d topProfilePoint)
	{
		//IL_002e: Unknown result type (might be due to invalid IL or missing references)
		//IL_002f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0034: Unknown result type (might be due to invalid IL or missing references)
		//IL_0035: Unknown result type (might be due to invalid IL or missing references)
		//IL_003a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0088: Unknown result type (might be due to invalid IL or missing references)
		//IL_0089: Unknown result type (might be due to invalid IL or missing references)
		//IL_008e: Unknown result type (might be due to invalid IL or missing references)
		//IL_008f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0094: Unknown result type (might be due to invalid IL or missing references)
		Brep[] array = vProngBezel.Split(intersectSrf0.ToBrep(), CSApplicationHelper.Application.Tolerance);
		Brep val = null;
		double num = 999.0;
		Brep[] array2 = array;
		Vector3d val3;
		foreach (Brep val2 in array2)
		{
			val3 = val2.ClosestPoint(topProfilePoint) - topProfilePoint;
			double length = ((Vector3d)(ref val3)).Length;
			if (length < num)
			{
				num = length;
				val = val2;
			}
		}
		Brep[] array3 = val.Split(intersectSrf1.ToBrep(), CSApplicationHelper.Application.Tolerance);
		val = null;
		num = 999.0;
		array2 = array3;
		foreach (Brep val4 in array2)
		{
			val3 = val4.ClosestPoint(topProfilePoint) - topProfilePoint;
			double length2 = ((Vector3d)(ref val3)).Length;
			if (length2 < num)
			{
				num = length2;
				val = val4;
			}
		}
		return val;
	}
}
