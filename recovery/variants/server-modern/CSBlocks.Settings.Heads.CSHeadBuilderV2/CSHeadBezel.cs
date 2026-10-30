using System;
using System.Collections.Generic;
using CS;
using CSBlockDataUtils.CSGemOffsetCurves;
using CSBlockUtil;
using CSDatas;
using GV.CounterSketch.Shared;
using Rhino;
using Rhino.Geometry;
using Rhino.Geometry.Collections;
using Rhino.Geometry.Intersect;

namespace CSBlocks.Settings.Heads.CSHeadBuilderV2;

internal static class CSHeadBezel
{
	public static bool BezelProfile(Curve prongRail, ProngParam prongParam, double prongTopOffset, double prongBaseOffset, ValueListItem baseStyle, bool planarDome, double bezelSeatHeight, double bezelSeatLength, double bezelSeatAngle, double bezelBlendPercentage, out Curve bezelProngOutline, out string error)
	{
		//IL_0025: Unknown result type (might be due to invalid IL or missing references)
		//IL_004b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0071: Unknown result type (might be due to invalid IL or missing references)
		//IL_0097: Unknown result type (might be due to invalid IL or missing references)
		//IL_00bd: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e3: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ed: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f3: Expected O, but got Unknown
		//IL_0114: Unknown result type (might be due to invalid IL or missing references)
		//IL_013a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0160: Unknown result type (might be due to invalid IL or missing references)
		//IL_0186: Unknown result type (might be due to invalid IL or missing references)
		//IL_01ac: Unknown result type (might be due to invalid IL or missing references)
		//IL_01d2: Unknown result type (might be due to invalid IL or missing references)
		//IL_01dc: Unknown result type (might be due to invalid IL or missing references)
		//IL_01e2: Expected O, but got Unknown
		//IL_01e3: Unknown result type (might be due to invalid IL or missing references)
		//IL_01e8: Unknown result type (might be due to invalid IL or missing references)
		//IL_01fb: Unknown result type (might be due to invalid IL or missing references)
		//IL_022c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0231: Unknown result type (might be due to invalid IL or missing references)
		//IL_0232: Unknown result type (might be due to invalid IL or missing references)
		//IL_0237: Unknown result type (might be due to invalid IL or missing references)
		//IL_0239: Unknown result type (might be due to invalid IL or missing references)
		//IL_023e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0243: Unknown result type (might be due to invalid IL or missing references)
		//IL_0248: Unknown result type (might be due to invalid IL or missing references)
		//IL_024a: Unknown result type (might be due to invalid IL or missing references)
		//IL_024c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0251: Unknown result type (might be due to invalid IL or missing references)
		//IL_0257: Unknown result type (might be due to invalid IL or missing references)
		//IL_025c: Unknown result type (might be due to invalid IL or missing references)
		//IL_025d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0262: Unknown result type (might be due to invalid IL or missing references)
		//IL_0267: Unknown result type (might be due to invalid IL or missing references)
		//IL_0272: Unknown result type (might be due to invalid IL or missing references)
		//IL_0295: Unknown result type (might be due to invalid IL or missing references)
		//IL_029c: Unknown result type (might be due to invalid IL or missing references)
		//IL_02a1: Unknown result type (might be due to invalid IL or missing references)
		//IL_02aa: Unknown result type (might be due to invalid IL or missing references)
		//IL_02af: Unknown result type (might be due to invalid IL or missing references)
		//IL_02b4: Unknown result type (might be due to invalid IL or missing references)
		//IL_02e2: Unknown result type (might be due to invalid IL or missing references)
		//IL_02ff: Unknown result type (might be due to invalid IL or missing references)
		//IL_0304: Unknown result type (might be due to invalid IL or missing references)
		//IL_0319: Unknown result type (might be due to invalid IL or missing references)
		//IL_031e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0320: Unknown result type (might be due to invalid IL or missing references)
		//IL_0325: Unknown result type (might be due to invalid IL or missing references)
		//IL_032a: Unknown result type (might be due to invalid IL or missing references)
		//IL_032f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0331: Unknown result type (might be due to invalid IL or missing references)
		//IL_0336: Unknown result type (might be due to invalid IL or missing references)
		//IL_033b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0340: Unknown result type (might be due to invalid IL or missing references)
		//IL_0345: Unknown result type (might be due to invalid IL or missing references)
		//IL_0351: Unknown result type (might be due to invalid IL or missing references)
		//IL_0353: Unknown result type (might be due to invalid IL or missing references)
		//IL_0355: Unknown result type (might be due to invalid IL or missing references)
		//IL_035a: Unknown result type (might be due to invalid IL or missing references)
		//IL_035c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0367: Unknown result type (might be due to invalid IL or missing references)
		//IL_0375: Unknown result type (might be due to invalid IL or missing references)
		//IL_037b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0380: Unknown result type (might be due to invalid IL or missing references)
		//IL_0382: Unknown result type (might be due to invalid IL or missing references)
		//IL_0387: Unknown result type (might be due to invalid IL or missing references)
		//IL_038f: Unknown result type (might be due to invalid IL or missing references)
		//IL_041a: Unknown result type (might be due to invalid IL or missing references)
		//IL_041f: Unknown result type (might be due to invalid IL or missing references)
		//IL_044c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0452: Unknown result type (might be due to invalid IL or missing references)
		//IL_0457: Unknown result type (might be due to invalid IL or missing references)
		//IL_045c: Unknown result type (might be due to invalid IL or missing references)
		//IL_040a: Unknown result type (might be due to invalid IL or missing references)
		//IL_040c: Unknown result type (might be due to invalid IL or missing references)
		//IL_040e: Unknown result type (might be due to invalid IL or missing references)
		//IL_04f0: Unknown result type (might be due to invalid IL or missing references)
		//IL_04f5: Unknown result type (might be due to invalid IL or missing references)
		//IL_04f7: Unknown result type (might be due to invalid IL or missing references)
		//IL_0511: Unknown result type (might be due to invalid IL or missing references)
		//IL_0516: Unknown result type (might be due to invalid IL or missing references)
		//IL_052b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0530: Unknown result type (might be due to invalid IL or missing references)
		//IL_0533: Unknown result type (might be due to invalid IL or missing references)
		//IL_0538: Unknown result type (might be due to invalid IL or missing references)
		//IL_053a: Unknown result type (might be due to invalid IL or missing references)
		//IL_053f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0544: Unknown result type (might be due to invalid IL or missing references)
		//IL_0549: Unknown result type (might be due to invalid IL or missing references)
		//IL_054b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0550: Unknown result type (might be due to invalid IL or missing references)
		//IL_0555: Unknown result type (might be due to invalid IL or missing references)
		//IL_0557: Unknown result type (might be due to invalid IL or missing references)
		//IL_055c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0561: Unknown result type (might be due to invalid IL or missing references)
		//IL_056d: Unknown result type (might be due to invalid IL or missing references)
		//IL_056f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0571: Unknown result type (might be due to invalid IL or missing references)
		//IL_0576: Unknown result type (might be due to invalid IL or missing references)
		//IL_0578: Unknown result type (might be due to invalid IL or missing references)
		//IL_059e: Unknown result type (might be due to invalid IL or missing references)
		//IL_05a3: Unknown result type (might be due to invalid IL or missing references)
		//IL_05b0: Unknown result type (might be due to invalid IL or missing references)
		//IL_05b5: Unknown result type (might be due to invalid IL or missing references)
		//IL_0602: Unknown result type (might be due to invalid IL or missing references)
		//IL_061b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0634: Unknown result type (might be due to invalid IL or missing references)
		//IL_064d: Unknown result type (might be due to invalid IL or missing references)
		//IL_068a: Unknown result type (might be due to invalid IL or missing references)
		//IL_068f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0696: Unknown result type (might be due to invalid IL or missing references)
		//IL_069b: Unknown result type (might be due to invalid IL or missing references)
		//IL_069f: Unknown result type (might be due to invalid IL or missing references)
		//IL_06a4: Unknown result type (might be due to invalid IL or missing references)
		//IL_06af: Unknown result type (might be due to invalid IL or missing references)
		//IL_06b4: Unknown result type (might be due to invalid IL or missing references)
		//IL_06ca: Unknown result type (might be due to invalid IL or missing references)
		//IL_06cf: Unknown result type (might be due to invalid IL or missing references)
		//IL_06d2: Unknown result type (might be due to invalid IL or missing references)
		//IL_06d7: Unknown result type (might be due to invalid IL or missing references)
		//IL_06d9: Unknown result type (might be due to invalid IL or missing references)
		//IL_06f6: Unknown result type (might be due to invalid IL or missing references)
		//IL_06fb: Unknown result type (might be due to invalid IL or missing references)
		//IL_0700: Unknown result type (might be due to invalid IL or missing references)
		//IL_071d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0722: Unknown result type (might be due to invalid IL or missing references)
		//IL_0727: Unknown result type (might be due to invalid IL or missing references)
		//IL_0744: Unknown result type (might be due to invalid IL or missing references)
		//IL_0749: Unknown result type (might be due to invalid IL or missing references)
		//IL_074e: Unknown result type (might be due to invalid IL or missing references)
		//IL_076b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0770: Unknown result type (might be due to invalid IL or missing references)
		//IL_079d: Unknown result type (might be due to invalid IL or missing references)
		//IL_07a2: Unknown result type (might be due to invalid IL or missing references)
		//IL_07af: Unknown result type (might be due to invalid IL or missing references)
		//IL_07b4: Unknown result type (might be due to invalid IL or missing references)
		//IL_07cf: Unknown result type (might be due to invalid IL or missing references)
		//IL_07d4: Unknown result type (might be due to invalid IL or missing references)
		//IL_07e1: Unknown result type (might be due to invalid IL or missing references)
		//IL_07e6: Unknown result type (might be due to invalid IL or missing references)
		//IL_07ff: Unknown result type (might be due to invalid IL or missing references)
		//IL_0804: Unknown result type (might be due to invalid IL or missing references)
		//IL_080f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0814: Unknown result type (might be due to invalid IL or missing references)
		//IL_0829: Unknown result type (might be due to invalid IL or missing references)
		//IL_082e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0839: Unknown result type (might be due to invalid IL or missing references)
		//IL_083e: Unknown result type (might be due to invalid IL or missing references)
		//IL_08d4: Unknown result type (might be due to invalid IL or missing references)
		//IL_08db: Unknown result type (might be due to invalid IL or missing references)
		//IL_08e0: Unknown result type (might be due to invalid IL or missing references)
		//IL_08e7: Expected O, but got Unknown
		//IL_087d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0884: Unknown result type (might be due to invalid IL or missing references)
		//IL_0889: Unknown result type (might be due to invalid IL or missing references)
		//IL_088e: Unknown result type (might be due to invalid IL or missing references)
		//IL_089c: Unknown result type (might be due to invalid IL or missing references)
		//IL_08a3: Unknown result type (might be due to invalid IL or missing references)
		//IL_08a8: Unknown result type (might be due to invalid IL or missing references)
		//IL_08ad: Unknown result type (might be due to invalid IL or missing references)
		//IL_08e7: Unknown result type (might be due to invalid IL or missing references)
		//IL_08ec: Unknown result type (might be due to invalid IL or missing references)
		//IL_08f1: Unknown result type (might be due to invalid IL or missing references)
		//IL_08f8: Unknown result type (might be due to invalid IL or missing references)
		//IL_08fd: Unknown result type (might be due to invalid IL or missing references)
		//IL_0902: Unknown result type (might be due to invalid IL or missing references)
		//IL_0907: Unknown result type (might be due to invalid IL or missing references)
		//IL_090c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0911: Unknown result type (might be due to invalid IL or missing references)
		//IL_0915: Unknown result type (might be due to invalid IL or missing references)
		//IL_0c64: Unknown result type (might be due to invalid IL or missing references)
		//IL_093d: Unknown result type (might be due to invalid IL or missing references)
		//IL_093f: Unknown result type (might be due to invalid IL or missing references)
		//IL_09a5: Unknown result type (might be due to invalid IL or missing references)
		//IL_09ad: Unknown result type (might be due to invalid IL or missing references)
		//IL_09b5: Unknown result type (might be due to invalid IL or missing references)
		//IL_09bc: Unknown result type (might be due to invalid IL or missing references)
		//IL_09c3: Expected O, but got Unknown
		//IL_0a4b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0a50: Unknown result type (might be due to invalid IL or missing references)
		//IL_0a5b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0a60: Unknown result type (might be due to invalid IL or missing references)
		//IL_0a8c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0aab: Unknown result type (might be due to invalid IL or missing references)
		//IL_0ab5: Unknown result type (might be due to invalid IL or missing references)
		//IL_0abc: Expected O, but got Unknown
		//IL_09d2: Unknown result type (might be due to invalid IL or missing references)
		//IL_09d7: Unknown result type (might be due to invalid IL or missing references)
		//IL_09d9: Unknown result type (might be due to invalid IL or missing references)
		//IL_09de: Unknown result type (might be due to invalid IL or missing references)
		//IL_09e8: Unknown result type (might be due to invalid IL or missing references)
		//IL_09ec: Unknown result type (might be due to invalid IL or missing references)
		//IL_09f1: Unknown result type (might be due to invalid IL or missing references)
		//IL_09f5: Unknown result type (might be due to invalid IL or missing references)
		//IL_0a03: Unknown result type (might be due to invalid IL or missing references)
		//IL_0b00: Unknown result type (might be due to invalid IL or missing references)
		//IL_0b05: Unknown result type (might be due to invalid IL or missing references)
		//IL_0b17: Unknown result type (might be due to invalid IL or missing references)
		//IL_0b1c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0b1e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0b23: Unknown result type (might be due to invalid IL or missing references)
		//IL_0b2d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0b31: Unknown result type (might be due to invalid IL or missing references)
		//IL_0b36: Unknown result type (might be due to invalid IL or missing references)
		//IL_0b3a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0b48: Unknown result type (might be due to invalid IL or missing references)
		bezelProngOutline = null;
		Curve val = (Curve)new PolylineCurve((IEnumerable<Point3d>)new List<Point3d>
		{
			new Point3d(0.0, 0.0, 0.0),
			new Point3d(-0.5, 0.0, 0.0),
			new Point3d(-0.5, 1.0, 0.0),
			new Point3d(0.5, 1.0, 0.0),
			new Point3d(0.5, 0.0, 0.0),
			new Point3d(0.0, 0.0, 0.0)
		});
		Curve val2 = (Curve)new PolylineCurve((IEnumerable<Point3d>)new List<Point3d>
		{
			new Point3d(0.0, 0.0, 0.0),
			new Point3d(-0.5, 0.0, 0.0),
			new Point3d(-0.5, 1.0, 0.0),
			new Point3d(0.5, 1.0, 0.0),
			new Point3d(0.5, 0.0, 0.0),
			new Point3d(0.0, 0.0, 0.0)
		});
		Point3d pointAtStart = prongRail.PointAtStart;
		((Point3d)(ref pointAtStart)).Z = 0.0;
		Vector3d val3 = default(Vector3d);
		((Vector3d)(ref val3))._002Ector(pointAtStart);
		((Vector3d)(ref val3)).Unitize();
		Point3d val4 = default(Point3d);
		((Point3d)(ref val4))._002Ector(5.0, 0.0, 0.0);
		Transform val5 = CSBlockUtil.CSUtilMath.Orient3Point(prongRail.PointAtStart, val3, Vector3d.ZAxis, val4, Vector3d.XAxis, Vector3d.ZAxis);
		Transform val6 = CSBlockUtil.CSUtilMath.Orient3Point(val4, Vector3d.XAxis, Vector3d.ZAxis, prongRail.PointAtStart, val3, Vector3d.ZAxis);
		prongRail = prongRail.DuplicateCurve();
		((GeometryBase)prongRail).Transform(val5);
		Vector3d inputVector = new Vector3d(0.0, 0.0, -1.0);
		Curve obj = prongRail;
		Interval domain = prongRail.Domain;
		double num = CSBlockUtil.CSUtilMath.AngleBetweenVectorsWithDirection(inputVector, obj.TangentAt(((Interval)(ref domain)).Min), -Vector3d.YAxis);
		double num2 = (planarDome ? (1.0 / Math.Cos(num)) : 1.0);
		Transform val7 = Transform.Scale(Plane.WorldXY, prongParam.ProngTopX, prongParam.ProngTopY * num2, 0.0);
		Transform val8 = Transform.Translation(0.0, prongTopOffset, 0.0);
		Transform val9 = CSBlockUtil.CSUtilMath.Orient3Point(Point3d.Origin, Vector3d.XAxis, Vector3d.ZAxis, val4, -Vector3d.YAxis, Vector3d.ZAxis);
		Curve val10 = val.DuplicateCurve();
		((GeometryBase)val10).Transform(val9 * val8 * val7);
		Transform val11 = CSBlockUtil.CSUtilMath.Rotation(Vector3d.ZAxis * -1.0, prongRail.TangentAtStart, val4);
		if (!planarDome)
		{
			((GeometryBase)val10).Transform(val11);
		}
		Curve domeCurve = null;
		if (!(prongParam.ProngDomeHeight <= 0.0))
		{
			if (!ProngDomeCurve(val, prongParam.ProngTopX, prongParam.ProngTopY, num, prongParam.ProngDomeHeight, prongParam.ProngDomeSettingDistance, prongParam.ProngDomeSettingAngle, num2, planarDome, prongParam.DomeBlendAmountInside, prongParam.DomeBlendAmountOutside, prongParam.DomeBlendStartInside, prongParam.DomeBlendStartOutside, prongParam.DomeBlendEndInside, prongParam.DomeBlendEndOutside, prongRail, ref domeCurve))
			{
				error = "Error Creating Prong Dome Curve";
				return false;
			}
			((GeometryBase)domeCurve).Transform(val9 * val8);
		}
		Point3d val12 = prongRail.PointAtEnd;
		double num3 = ((Point3d)(ref val12)).Z + prongParam.ProngBaseDomeHeight;
		double num4 = CSBlockUtil.CSUtilMath.AngleBetweenVectorsWithDirection(new Vector3d(0.0, 0.0, -1.0), prongRail.TangentAtEnd, -Vector3d.YAxis);
		double num5 = ((num4 < 0.0) ? (Math.Tan(0.0 - num4) * prongParam.ProngBaseY + 0.05) : 0.05);
		if (baseStyle != ValueListItem.HeadBuilderProngHead)
		{
			num5 = ((num4 < 0.0) ? (Math.Tan(0.0 - num4) * prongParam.ProngBaseY + 0.05) : (Math.Tan(num4) * prongParam.ProngBaseY + 0.05));
		}
		prongRail = prongRail.Extend((CurveEnd)2, num5, (CurveExtensionStyle)0);
		Point3d pointAtEnd = prongRail.PointAtEnd;
		Transform val13 = Transform.Scale(Plane.WorldXY, prongParam.ProngBaseX, prongParam.ProngBaseY, 0.0);
		Transform val14 = Transform.Translation(0.0, prongBaseOffset, 0.0);
		Vector3d tangentAtEnd = prongRail.TangentAtEnd;
		Transform val15 = CSBlockUtil.CSUtilMath.Orient3Point(Point3d.Origin, Vector3d.XAxis, Vector3d.ZAxis, pointAtEnd, -Vector3d.YAxis, -tangentAtEnd);
		Curve val16 = val2.DuplicateCurve();
		((GeometryBase)val16).Transform(val15 * val14 * val13);
		Curve[] profiles = (Curve[])(object)new Curve[2] { val10, val16 };
		double[] array = new double[2];
		domain = prongRail.Domain;
		array[0] = ((Interval)(ref domain)).Min;
		domain = prongRail.Domain;
		array[1] = ((Interval)(ref domain)).Max;
		double[] parameters = array;
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
		Brep val17 = ((Surface)NurbsSurface.CreateFromCorners(new Point3d(20.0, 20.0, num3), new Point3d(-20.0, 20.0, num3), new Point3d(-20.0, -20.0, num3), new Point3d(20.0, -20.0, num3))).ToBrep();
		Brep[] array2 = breps[0].Split(val17, 0.01);
		if (array2.Length != 2)
		{
			error = "Prong split failed";
			return false;
		}
		BoundingBox boundingBox = ((GeometryBase)array2[0]).GetBoundingBox(true);
		BoundingBox boundingBox2 = ((GeometryBase)array2[1]).GetBoundingBox(true);
		val12 = ((BoundingBox)(ref boundingBox)).Center;
		double z = ((Point3d)(ref val12)).Z;
		val12 = ((BoundingBox)(ref boundingBox2)).Center;
		Brep obj2 = ((z > ((Point3d)(ref val12)).Z) ? array2[0] : array2[1]);
		Point3d pointAtStart2 = prongRail.PointAtStart;
		Point3d pointAtEnd2 = prongRail.PointAtEnd;
		NurbsSurface val18 = NurbsSurface.CreateFromCorners(pointAtEnd2 + new Point3d(-4.0, 0.0, -3.0), pointAtEnd2 + new Point3d(8.0, 0.0, -3.0), pointAtStart2 + new Point3d(8.0, 0.0, 3.0), pointAtStart2 + new Point3d(-4.0, 0.0, 3.0));
		Curve[] array3 = default(Curve[]);
		Point3d[] array4 = default(Point3d[]);
		Intersection.BrepSurface(obj2, (Surface)(object)val18, CSApplicationHelper.Application.Tolerance, ref array3, ref array4);
		List<Curve> list = new List<Curve>();
		val12 = array3[0].PointAtStart;
		double x = ((Point3d)(ref val12)).X;
		val12 = array3[1].PointAtStart;
		Curve val19 = ((x < ((Point3d)(ref val12)).X) ? array3[0] : array3[1]);
		val12 = array3[0].PointAtStart;
		double x2 = ((Point3d)(ref val12)).X;
		val12 = array3[1].PointAtStart;
		Curve val20 = ((x2 < ((Point3d)(ref val12)).X) ? array3[1] : array3[0]);
		val12 = val19.PointAtStart;
		double z2 = ((Point3d)(ref val12)).Z;
		val12 = val19.PointAtEnd;
		if (z2 < ((Point3d)(ref val12)).Z)
		{
			val19.Reverse();
		}
		val12 = val20.PointAtStart;
		double z3 = ((Point3d)(ref val12)).Z;
		val12 = val20.PointAtEnd;
		if (z3 < ((Point3d)(ref val12)).Z)
		{
			val20.Reverse();
		}
		Curve domeCurve2;
		if (prongParam.ProngBaseDomeHeight > 0.0)
		{
			double angle = CSBlockUtil.CSUtilMath.AngleBetweenVectorsWithDirection(new Vector3d(0.0, 0.0, -1.0), val19.TangentAtEnd, -Vector3d.YAxis);
			Vector3d val21 = val19.PointAtEnd - val20.PointAtEnd;
			if (!ProngBaseDomeCurve(((Vector3d)(ref val21)).Length, 0.0 - prongParam.ProngBaseDomeHeight, angle, out domeCurve2))
			{
				error = "Failed prong base";
				return false;
			}
		}
		else
		{
			domeCurve2 = (Curve)new LineCurve(val19.PointAtEnd, val20.PointAtEnd);
		}
		Transform val22 = CSBlockUtil.CSUtilMath.Orient3Point(Point3d.Origin, Vector3d.XAxis, Vector3d.ZAxis, val19.PointAtEnd, -Vector3d.YAxis, Vector3d.ZAxis);
		((GeometryBase)domeCurve2).Transform(val22);
		if (bezelSeatHeight > 0.0 && bezelSeatLength > 0.0)
		{
			Point3d item = val4;
			((Point3d)(ref item)).Z = ((Point3d)(ref item)).Z - bezelSeatHeight;
			double num6 = RhinoMath.ToRadians(bezelSeatAngle + 180.0);
			double num7 = ((Point3d)(ref item)).X + bezelSeatLength * Math.Cos(num6);
			double num8 = ((Point3d)(ref item)).Z + bezelSeatLength * Math.Sin(num6);
			Point3d val23 = default(Point3d);
			((Point3d)(ref val23))._002Ector(num7, ((Point3d)(ref item)).Y, num8);
			PolylineCurve val24 = new PolylineCurve((IEnumerable<Point3d>)new List<Point3d> { val4, item, val23 });
			if (bezelBlendPercentage > 0.99)
			{
				Vector3d vectorStart = val19.PointAtEnd - val23;
				((Vector3d)(ref vectorStart)).Unitize();
				Curve val25 = (Curve)(object)CSUtilCurves.BlendCurve(val23, val19.PointAtEnd, vectorStart, val19.TangentAtEnd * -1.0);
				val19 = Curve.JoinCurves((IEnumerable<Curve>)(object)new Curve[2]
				{
					(Curve)val24,
					val25
				})[0];
			}
			else
			{
				val12 = val19.PointAtEnd;
				double z4 = ((Point3d)(ref val12)).Z;
				val12 = val19.PointAtEnd;
				double num9 = z4 - (((Point3d)(ref val12)).Z - num8) * bezelBlendPercentage;
				PolylineCurve val26 = new PolylineCurve((IEnumerable<Point3d>)new List<Point3d>
				{
					new Point3d(0.0, 0.0, num9),
					new Point3d(50.0, 0.0, num9)
				});
				CurveIntersections val27 = Intersection.CurveCurve(val19, (Curve)(object)val26, CSApplicationHelper.Application.Tolerance, CSApplicationHelper.Application.Tolerance);
				if (val27.Count != 1)
				{
					error = "Bezel seat Intersection Error";
					return false;
				}
				Curve obj3 = val19;
				double parameterA = val27[0].ParameterA;
				domain = val19.Domain;
				val19 = obj3.Trim(parameterA, ((Interval)(ref domain)).Max);
				Vector3d vectorStart2 = val19.PointAtStart - val23;
				((Vector3d)(ref vectorStart2)).Unitize();
				Curve val28 = (Curve)(object)CSUtilCurves.BlendCurve(val23, val19.PointAtStart, vectorStart2, val19.TangentAtStart * -1.0);
				val28 = (Curve)(object)val28.Rebuild(5, 3, false);
				val19 = Curve.JoinCurves((IEnumerable<Curve>)(object)new Curve[3]
				{
					(Curve)val24,
					val28,
					val19
				})[0];
			}
		}
		list.Add(val19);
		list.Add(val20);
		if (domeCurve == null)
		{
			error = "Missing prongTopDomeCrv";
			return false;
		}
		list.Add((Curve)(object)domeCurve.ToNurbsCurve());
		list.Add(domeCurve2);
		NurbsCurve val29 = Curve.JoinCurves((IEnumerable<Curve>)new List<NurbsCurve>
		{
			list[0].ToNurbsCurve(),
			list[1].Rebuild(5, 3, false),
			list[2].Rebuild(8, 3, false),
			list[3].Rebuild(8, 3, false)
		}, CSApplicationHelper.Application.Tolerance)[0].ToNurbsCurve();
		((GeometryBase)val29).MakeDeformable();
		((Curve)val29).MakeClosed(0.01);
		bezelProngOutline = (Curve)(object)val29;
		((GeometryBase)bezelProngOutline).Transform(val6);
		error = "";
		return true;
	}

	public static bool RevolveBezel(Curve bezelProngOutline1, CSGemData copyGem, Curve headBase, double cornerBlend, out NurbsSurface bezelSurface, out string error)
	{
		//IL_005f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0064: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c3: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c8: Unknown result type (might be due to invalid IL or missing references)
		//IL_00cc: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d1: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ee: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f3: Unknown result type (might be due to invalid IL or missing references)
		//IL_0100: Unknown result type (might be due to invalid IL or missing references)
		//IL_0105: Unknown result type (might be due to invalid IL or missing references)
		//IL_007a: Unknown result type (might be due to invalid IL or missing references)
		//IL_007f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0084: Unknown result type (might be due to invalid IL or missing references)
		//IL_0085: Unknown result type (might be due to invalid IL or missing references)
		//IL_008a: Unknown result type (might be due to invalid IL or missing references)
		//IL_008d: Unknown result type (might be due to invalid IL or missing references)
		//IL_012e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0133: Unknown result type (might be due to invalid IL or missing references)
		//IL_0137: Unknown result type (might be due to invalid IL or missing references)
		//IL_013c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0164: Unknown result type (might be due to invalid IL or missing references)
		//IL_0176: Unknown result type (might be due to invalid IL or missing references)
		//IL_017b: Unknown result type (might be due to invalid IL or missing references)
		//IL_017d: Unknown result type (might be due to invalid IL or missing references)
		//IL_01a3: Unknown result type (might be due to invalid IL or missing references)
		//IL_01a8: Unknown result type (might be due to invalid IL or missing references)
		//IL_01ab: Unknown result type (might be due to invalid IL or missing references)
		//IL_01b5: Unknown result type (might be due to invalid IL or missing references)
		//IL_01ba: Unknown result type (might be due to invalid IL or missing references)
		//IL_0218: Unknown result type (might be due to invalid IL or missing references)
		//IL_022a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0231: Unknown result type (might be due to invalid IL or missing references)
		//IL_0236: Unknown result type (might be due to invalid IL or missing references)
		//IL_023f: Unknown result type (might be due to invalid IL or missing references)
		//IL_025b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0260: Unknown result type (might be due to invalid IL or missing references)
		//IL_026a: Unknown result type (might be due to invalid IL or missing references)
		//IL_026f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0281: Unknown result type (might be due to invalid IL or missing references)
		//IL_0286: Unknown result type (might be due to invalid IL or missing references)
		//IL_02a0: Unknown result type (might be due to invalid IL or missing references)
		//IL_02a5: Unknown result type (might be due to invalid IL or missing references)
		//IL_02b0: Unknown result type (might be due to invalid IL or missing references)
		//IL_02b5: Unknown result type (might be due to invalid IL or missing references)
		//IL_02c0: Unknown result type (might be due to invalid IL or missing references)
		//IL_02c5: Unknown result type (might be due to invalid IL or missing references)
		//IL_02d0: Unknown result type (might be due to invalid IL or missing references)
		if (!GemOffsetCurve.GemOffset(copyGem.GemShape, copyGem, 0.0, out Curve curve, false, out error, cornerBlend, 0.0, 0.0))
		{
			bezelSurface = null;
			error = "CurveTop Inside:" + error;
			return false;
		}
		NurbsCurve val = bezelProngOutline1.ToNurbsCurve();
		NurbsCurve val2 = curve.ToNurbsCurve();
		Point3d val3 = copyGem.GetCurve().PointAt(copyGem.BezelParam);
		Plane val4 = default(Plane);
		if (((Curve)val).TryGetPlane(ref val4, 0.1))
		{
			Transform val5 = Transform.Rotation(((Plane)(ref val4)).Normal, Vector3d.YAxis, val3);
			((GeometryBase)val).Transform(val5);
		}
		bezelSurface = NurbsSurface.Create(3, true, val.Order, val2.Order, val.Points.Count, val2.Points.Count);
		BoundingBox boundingBox = ((GeometryBase)headBase).GetBoundingBox(true);
		Point3d val6 = ((BoundingBox)(ref boundingBox)).Center;
		double z = ((Point3d)(ref val6)).Z;
		double gemXSize = copyGem.GemXSize;
		double gemYSize = copyGem.GemYSize;
		Vector3d diagonal = ((BoundingBox)(ref boundingBox)).Diagonal;
		double x = ((Vector3d)(ref diagonal)).X;
		diagonal = ((BoundingBox)(ref boundingBox)).Diagonal;
		double y = ((Vector3d)(ref diagonal)).Y;
		double num = gemXSize - x;
		double num2 = gemYSize - y;
		for (int i = 0; i < val.Points.Count; i++)
		{
			ControlPoint val7 = val.Points[i];
			val6 = ((ControlPoint)(ref val7)).Location;
			double num3 = ((Point3d)(ref val6)).Z / z;
			double num4 = (gemXSize - num * num3) / gemXSize;
			double num5 = (gemYSize - num2 * num3) / gemYSize;
			Transform transform = Transform.Scale(Plane.WorldXY, num4, num5, 1.0);
			Transform transform2 = Transform.Scale(Plane.WorldXY, 1.0 / num4, 1.0 / num5, 1.0);
			copyGem.ApplyTransform(transform);
			val6 = ((ControlPoint)(ref val7)).Location;
			double distance = ((Point3d)(ref val6)).X - ((Point3d)(ref val3)).X + num / 2.0 * num3;
			if (!GemOffsetCurve.GemOffset(copyGem.GemShape, copyGem, distance, out curve, false, out error, cornerBlend, 0.0, 0.0))
			{
				error = "CurveTop Inside:" + error;
				return false;
			}
			copyGem.ApplyTransform(transform2);
			NurbsCurve val8 = curve.ToNurbsCurve();
			Vector3d zAxis = Vector3d.ZAxis;
			val6 = ((ControlPoint)(ref val7)).Location;
			((GeometryBase)val8).Translate(zAxis * ((Point3d)(ref val6)).Z);
			for (int j = 0; j < val8.Points.Count; j++)
			{
				ControlPoint val9 = val8.Points[j];
				ControlPoint val10 = val.Points[i];
				double weight = ((ControlPoint)(ref val10)).Weight;
				val10 = val8.Points[j];
				double num6 = weight * ((ControlPoint)(ref val10)).Weight;
				NurbsSurfacePointList points = bezelSurface.Points;
				int num7 = i;
				int num8 = j;
				val6 = ((ControlPoint)(ref val9)).Location;
				double x2 = ((Point3d)(ref val6)).X;
				val6 = ((ControlPoint)(ref val9)).Location;
				double y2 = ((Point3d)(ref val6)).Y;
				val6 = ((ControlPoint)(ref val9)).Location;
				points.SetControlPoint(num7, num8, new ControlPoint(x2, y2, ((Point3d)(ref val6)).Z, num6));
			}
		}
		for (int k = 0; k < val.Knots.Count; k++)
		{
			bezelSurface.KnotsU[k] = val.Knots[k];
		}
		for (int l = 0; l < val2.Knots.Count; l++)
		{
			bezelSurface.KnotsV[l] = val2.Knots[l];
		}
		error = "";
		return true;
	}

	private static bool ProngBaseDomeCurve(double prongYSize, double radius, double angle, out Curve domeCurve)
	{
		//IL_001c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0021: Unknown result type (might be due to invalid IL or missing references)
		//IL_0026: Unknown result type (might be due to invalid IL or missing references)
		//IL_0042: Unknown result type (might be due to invalid IL or missing references)
		//IL_0047: Unknown result type (might be due to invalid IL or missing references)
		//IL_004c: Unknown result type (might be due to invalid IL or missing references)
		//IL_009e: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a3: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a9: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ae: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b9: Unknown result type (might be due to invalid IL or missing references)
		//IL_00be: Unknown result type (might be due to invalid IL or missing references)
		//IL_00cb: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d0: Unknown result type (might be due to invalid IL or missing references)
		//IL_00dd: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e2: Unknown result type (might be due to invalid IL or missing references)
		//IL_00eb: Unknown result type (might be due to invalid IL or missing references)
		//IL_00fd: Unknown result type (might be due to invalid IL or missing references)
		//IL_013a: Unknown result type (might be due to invalid IL or missing references)
		Curve val = (Curve)(object)CSUtilGeometry.CreateDomeRC(radius, prongYSize / 2.0);
		((GeometryBase)val).Transform(Transform.Rotation(Math.PI / 2.0, Vector3d.ZAxis, Point3d.Origin));
		Curve val2 = val.DuplicateCurve();
		((GeometryBase)val2).Transform(Transform.Rotation(Math.PI, Vector3d.ZAxis, Point3d.Origin));
		Curve[] array = Curve.JoinCurves((IEnumerable<Curve>)(object)new Curve[2] { val, val2 }, CSApplicationHelper.Application.Tolerance);
		if (array.Length != 1)
		{
			domeCurve = null;
			return false;
		}
		NurbsCurve val3 = array[0].ToNurbsCurve();
		double num = Math.Tan(angle);
		for (int i = 0; i < val3.Points.Count; i++)
		{
			ControlPoint val4 = val3.Points[i];
			Point3d location = ((ControlPoint)(ref val4)).Location;
			double x = ((Point3d)(ref location)).X;
			location = ((ControlPoint)(ref val4)).Location;
			double y = ((Point3d)(ref location)).Y;
			location = ((ControlPoint)(ref val4)).Location;
			double num2 = y + num * ((Point3d)(ref location)).Z;
			location = ((ControlPoint)(ref val4)).Location;
			((ControlPoint)(ref val4)).Location = new Point3d(x, num2, ((Point3d)(ref location)).Z);
			val3.Points[i] = val4;
		}
		((GeometryBase)val3).Translate(new Vector3d(0.0, prongYSize / 2.0, 0.0));
		domeCurve = (Curve)(object)val3;
		return true;
	}

	private static bool ProngDomeCurve(Curve prongProfile, double prongX, double prongY, double angle, double radius, double rotateX, double prongSettingAngle, double fixScale, bool planarDome, double blendAmountInside, double blendAmountOutside, double blendStartInside, double blendStartOutside, double blendEndInside, double blendEndOutside, Curve prongRail, ref Curve domeCurve)
	{
		//IL_0002: Unknown result type (might be due to invalid IL or missing references)
		//IL_0007: Unknown result type (might be due to invalid IL or missing references)
		//IL_005b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0063: Unknown result type (might be due to invalid IL or missing references)
		//IL_0068: Unknown result type (might be due to invalid IL or missing references)
		//IL_0075: Unknown result type (might be due to invalid IL or missing references)
		//IL_007a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0090: Unknown result type (might be due to invalid IL or missing references)
		//IL_0095: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f5: Unknown result type (might be due to invalid IL or missing references)
		//IL_00fa: Unknown result type (might be due to invalid IL or missing references)
		//IL_0105: Unknown result type (might be due to invalid IL or missing references)
		//IL_0106: Unknown result type (might be due to invalid IL or missing references)
		//IL_0107: Unknown result type (might be due to invalid IL or missing references)
		//IL_009f: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a7: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ac: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b9: Unknown result type (might be due to invalid IL or missing references)
		//IL_00be: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d1: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d6: Unknown result type (might be due to invalid IL or missing references)
		//IL_01d6: Unknown result type (might be due to invalid IL or missing references)
		//IL_01f6: Unknown result type (might be due to invalid IL or missing references)
		//IL_01fb: Unknown result type (might be due to invalid IL or missing references)
		//IL_0202: Expected O, but got Unknown
		//IL_021d: Unknown result type (might be due to invalid IL or missing references)
		//IL_023d: Unknown result type (might be due to invalid IL or missing references)
		//IL_025d: Unknown result type (might be due to invalid IL or missing references)
		//IL_027d: Unknown result type (might be due to invalid IL or missing references)
		//IL_02d4: Unknown result type (might be due to invalid IL or missing references)
		//IL_02d9: Unknown result type (might be due to invalid IL or missing references)
		//IL_012c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0131: Unknown result type (might be due to invalid IL or missing references)
		//IL_013d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0142: Unknown result type (might be due to invalid IL or missing references)
		//IL_015e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0163: Unknown result type (might be due to invalid IL or missing references)
		//IL_0168: Unknown result type (might be due to invalid IL or missing references)
		//IL_016a: Unknown result type (might be due to invalid IL or missing references)
		//IL_016f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0174: Unknown result type (might be due to invalid IL or missing references)
		//IL_0179: Unknown result type (might be due to invalid IL or missing references)
		//IL_017e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0189: Unknown result type (might be due to invalid IL or missing references)
		//IL_0197: Unknown result type (might be due to invalid IL or missing references)
		//IL_019e: Unknown result type (might be due to invalid IL or missing references)
		//IL_01a5: Unknown result type (might be due to invalid IL or missing references)
		//IL_01aa: Unknown result type (might be due to invalid IL or missing references)
		//IL_01af: Unknown result type (might be due to invalid IL or missing references)
		//IL_01b3: Unknown result type (might be due to invalid IL or missing references)
		//IL_02f7: Unknown result type (might be due to invalid IL or missing references)
		//IL_02fc: Unknown result type (might be due to invalid IL or missing references)
		//IL_031a: Unknown result type (might be due to invalid IL or missing references)
		//IL_031f: Unknown result type (might be due to invalid IL or missing references)
		//IL_032d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0357: Unknown result type (might be due to invalid IL or missing references)
		//IL_035c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0365: Unknown result type (might be due to invalid IL or missing references)
		//IL_036a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0378: Unknown result type (might be due to invalid IL or missing references)
		//IL_037d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0386: Unknown result type (might be due to invalid IL or missing references)
		//IL_038b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0395: Unknown result type (might be due to invalid IL or missing references)
		//IL_0397: Unknown result type (might be due to invalid IL or missing references)
		//IL_03b5: Unknown result type (might be due to invalid IL or missing references)
		//IL_03ba: Unknown result type (might be due to invalid IL or missing references)
		//IL_03c3: Unknown result type (might be due to invalid IL or missing references)
		//IL_03c8: Unknown result type (might be due to invalid IL or missing references)
		//IL_03d2: Unknown result type (might be due to invalid IL or missing references)
		//IL_03d4: Unknown result type (might be due to invalid IL or missing references)
		//IL_03ee: Unknown result type (might be due to invalid IL or missing references)
		//IL_03f3: Unknown result type (might be due to invalid IL or missing references)
		//IL_0417: Unknown result type (might be due to invalid IL or missing references)
		//IL_042c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0431: Unknown result type (might be due to invalid IL or missing references)
		//IL_0435: Unknown result type (might be due to invalid IL or missing references)
		//IL_0436: Unknown result type (might be due to invalid IL or missing references)
		//IL_0437: Unknown result type (might be due to invalid IL or missing references)
		//IL_0443: Unknown result type (might be due to invalid IL or missing references)
		//IL_049d: Unknown result type (might be due to invalid IL or missing references)
		//IL_049f: Unknown result type (might be due to invalid IL or missing references)
		//IL_04aa: Unknown result type (might be due to invalid IL or missing references)
		//IL_04af: Unknown result type (might be due to invalid IL or missing references)
		//IL_04b4: Unknown result type (might be due to invalid IL or missing references)
		//IL_04bf: Unknown result type (might be due to invalid IL or missing references)
		//IL_04c4: Unknown result type (might be due to invalid IL or missing references)
		//IL_04c6: Unknown result type (might be due to invalid IL or missing references)
		//IL_04d0: Unknown result type (might be due to invalid IL or missing references)
		//IL_04de: Unknown result type (might be due to invalid IL or missing references)
		//IL_04e3: Unknown result type (might be due to invalid IL or missing references)
		//IL_04f6: Unknown result type (might be due to invalid IL or missing references)
		//IL_050b: Unknown result type (might be due to invalid IL or missing references)
		//IL_046c: Unknown result type (might be due to invalid IL or missing references)
		//IL_046e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0475: Unknown result type (might be due to invalid IL or missing references)
		//IL_047a: Unknown result type (might be due to invalid IL or missing references)
		//IL_047f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0451: Unknown result type (might be due to invalid IL or missing references)
		//IL_0453: Unknown result type (might be due to invalid IL or missing references)
		//IL_045e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0463: Unknown result type (might be due to invalid IL or missing references)
		//IL_0468: Unknown result type (might be due to invalid IL or missing references)
		//IL_0533: Unknown result type (might be due to invalid IL or missing references)
		//IL_0538: Unknown result type (might be due to invalid IL or missing references)
		//IL_053a: Unknown result type (might be due to invalid IL or missing references)
		//IL_053c: Unknown result type (might be due to invalid IL or missing references)
		//IL_054e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0553: Unknown result type (might be due to invalid IL or missing references)
		//IL_0555: Unknown result type (might be due to invalid IL or missing references)
		//IL_0557: Unknown result type (might be due to invalid IL or missing references)
		//IL_0525: Unknown result type (might be due to invalid IL or missing references)
		//IL_052a: Unknown result type (might be due to invalid IL or missing references)
		//IL_052f: Unknown result type (might be due to invalid IL or missing references)
		//IL_048a: Unknown result type (might be due to invalid IL or missing references)
		//IL_048f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0491: Unknown result type (might be due to invalid IL or missing references)
		//IL_05af: Unknown result type (might be due to invalid IL or missing references)
		//IL_05b4: Unknown result type (might be due to invalid IL or missing references)
		//IL_05b9: Unknown result type (might be due to invalid IL or missing references)
		BoundingBox boundingBox = ((GeometryBase)prongProfile).GetBoundingBox(true);
		bool flag = radius > 0.0;
		if (!flag)
		{
			blendAmountInside = 1.0;
			blendAmountOutside = 1.0;
			blendStartInside = 1.0;
			blendStartOutside = 1.0;
			blendEndInside = 1.0;
			blendEndOutside = 1.0;
		}
		Plane worldXY = Plane.WorldXY;
		Vector3d diagonal = ((BoundingBox)(ref boundingBox)).Diagonal;
		double num = prongX / ((Vector3d)(ref diagonal)).X;
		diagonal = ((BoundingBox)(ref boundingBox)).Diagonal;
		Transform val = Transform.Scale(worldXY, num, prongY / ((Vector3d)(ref diagonal)).Y * fixScale, 0.0);
		if (!planarDome && flag)
		{
			Plane worldXY2 = Plane.WorldXY;
			diagonal = ((BoundingBox)(ref boundingBox)).Diagonal;
			double num2 = prongX / ((Vector3d)(ref diagonal)).X;
			diagonal = ((BoundingBox)(ref boundingBox)).Diagonal;
			val = Transform.Scale(worldXY2, num2, prongY / ((Vector3d)(ref diagonal)).Y, 0.0);
		}
		Transform val2 = Transform.Translation(0.0, (0.0 - prongY) / 2.0, 0.0);
		Curve val3 = prongProfile.DuplicateCurve();
		((GeometryBase)val3).Transform(val2 * val);
		Curve val4 = prongRail.DuplicateCurve();
		if (flag && !planarDome)
		{
			Point3d pointAtStart = prongRail.PointAtStart;
			double num3 = 0.0 - ((Point3d)(ref pointAtStart)).X;
			pointAtStart = prongRail.PointAtStart;
			Vector3d source = default(Vector3d);
			((Vector3d)(ref source))._002Ector(num3, 0.0 - ((Point3d)(ref pointAtStart)).Y, 0.0);
			((GeometryBase)val4).Transform(CSBlockUtil.CSUtilMath.Orient3Point(prongRail.PointAtStart, Vector3d.ZAxis, source, Point3d.Origin, Vector3d.ZAxis, -Vector3d.YAxis));
			Transform val5 = CSBlockUtil.CSUtilMath.Rotation(Vector3d.ZAxis * -1.0, val4.TangentAtStart, val3.PointAtStart);
			((GeometryBase)val3).Transform(val5);
		}
		LineCurve val6 = new LineCurve(new Point3d(0.0, -50.0, 0.0), new Point3d(0.0, 50.0, 0.0));
		NurbsSurface val7 = NurbsSurface.CreateFromCorners(new Point3d(0.0, -20.0, -10.0), new Point3d(0.0, 20.0, -10.0), new Point3d(0.0, 20.0, 10.0), new Point3d(0.0, -20.0, 10.0));
		CurveIntersections val8 = Intersection.CurveSurface(val3, (Surface)(object)val7, 0.01, 0.01);
		if (val8.Count <= 1)
		{
			return false;
		}
		double parameterA = val8[0].ParameterA;
		double parameterA2 = val8[1].ParameterA;
		Interval domain = val3.Domain;
		if (Math.Abs(parameterA - ((Interval)(ref domain)).Min) > 0.0001)
		{
			domain = val3.Domain;
			if (Math.Abs(parameterA - ((Interval)(ref domain)).Max) > 0.0001)
			{
				Point3d val9 = val3.PointAt(parameterA2);
				val3.ChangeClosedCurveSeam(parameterA);
				val3.ClosestPoint(val9, ref parameterA2);
			}
		}
		Curve[] array = val3.Split(parameterA2);
		Curve val10 = array[0];
		Curve val11 = array[1];
		if (val10 == null || val11 == null)
		{
			return false;
		}
		domain = ((Curve)val6).Domain;
		Vector3d val12 = ((Curve)val6).TangentAt(((Interval)(ref domain)).Mid);
		((Vector3d)(ref val12)).Unitize();
		domain = val10.Domain;
		Vector3d val13 = val10.TangentAt(((Interval)(ref domain)).Mid);
		((Vector3d)(ref val13)).Unitize();
		if (val12 * val13 < 0.0)
		{
			val10.Reverse();
		}
		domain = val11.Domain;
		Vector3d val14 = val11.TangentAt(((Interval)(ref domain)).Mid);
		((Vector3d)(ref val14)).Unitize();
		if (val12 * val14 < 0.0)
		{
			val11.Reverse();
		}
		Vector3d val15 = Vector3d.ZAxis;
		if (!flag)
		{
			((Vector3d)(ref val15)).Z = 0.0 - ((Vector3d)(ref val15)).Z;
			radius = 0.0 - radius;
		}
		if (planarDome || !flag)
		{
			((Vector3d)(ref val15)).Rotate(0.0 - angle, Vector3d.XAxis);
		}
		((Vector3d)(ref val15)).Unitize();
		Point3d val16 = ((BoundingBox)(ref boundingBox)).Center;
		((Point3d)(ref val16)).Transform(val2 * val);
		Point3d rotationCenter = default(Point3d);
		((Point3d)(ref rotationCenter))._002Ector(val16);
		if (flag)
		{
			val16 = (planarDome ? (val16 + Vector3d.ZAxis * radius) : (val16 + val15 * (radius / Math.Cos(angle))));
			((Point3d)(ref val16)).Transform(CSBlockUtil.CSUtilMath.Rotation(CSBlockUtil.CSUtilMath.ToRadians(rotateX), Vector3d.XAxis, rotationCenter));
		}
		else
		{
			val16 += val15 * (radius / Math.Cos(angle));
			((Point3d)(ref val16)).Transform(CSBlockUtil.CSUtilMath.Rotation(CSBlockUtil.CSUtilMath.ToRadians(rotateX), Vector3d.XAxis, rotationCenter));
		}
		Vector3d val17 = Vector3d.YAxis * -1.0;
		((Vector3d)(ref val17)).Rotate(CSBlockUtil.CSUtilMath.ToRadians(prongSettingAngle) + CSBlockUtil.CSUtilMath.ToRadians(rotateX), Vector3d.XAxis);
		((Vector3d)(ref val17)).Unitize();
		Vector3d vectorEnd = default(Vector3d);
		((Vector3d)(ref vectorEnd))._002Ector(val17);
		((Vector3d)(ref vectorEnd)).Reverse();
		if (!planarDome && flag)
		{
			val15 = -val4.TangentAtStart;
		}
		Curve val18 = (Curve)(object)CSUtilCurves.BlendCurve(val10.PointAtStart, val16, val15, val17, blendAmountInside, blendStartInside, blendEndInside);
		Curve val19 = (Curve)(object)CSUtilCurves.BlendCurve(val10.PointAtEnd, val16, val15, vectorEnd, blendAmountOutside, blendStartOutside, blendEndOutside);
		Curve[] array2 = Curve.JoinCurves((IEnumerable<Curve>)(object)new Curve[2] { val18, val19 });
		if (array2 == null)
		{
			return false;
		}
		if (array2.Length == 0)
		{
			return false;
		}
		domeCurve = array2[0];
		Transform val20 = Transform.Translation(0.0, prongY / 2.0, 0.0);
		((GeometryBase)domeCurve).Transform(val20);
		return true;
	}
}
