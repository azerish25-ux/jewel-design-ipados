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
		//IL_0034: Unknown result type (might be due to invalid IL or missing references)
		//IL_0128: Unknown result type (might be due to invalid IL or missing references)
		//IL_012d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0131: Unknown result type (might be due to invalid IL or missing references)
		//IL_0136: Unknown result type (might be due to invalid IL or missing references)
		//IL_013a: Unknown result type (might be due to invalid IL or missing references)
		//IL_013f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0143: Unknown result type (might be due to invalid IL or missing references)
		//IL_0148: Unknown result type (might be due to invalid IL or missing references)
		//IL_014a: Unknown result type (might be due to invalid IL or missing references)
		//IL_014c: Unknown result type (might be due to invalid IL or missing references)
		//IL_014e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0153: Unknown result type (might be due to invalid IL or missing references)
		//IL_0155: Unknown result type (might be due to invalid IL or missing references)
		//IL_0157: Unknown result type (might be due to invalid IL or missing references)
		//IL_0159: Unknown result type (might be due to invalid IL or missing references)
		//IL_015e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0160: Unknown result type (might be due to invalid IL or missing references)
		//IL_0162: Unknown result type (might be due to invalid IL or missing references)
		//IL_0164: Unknown result type (might be due to invalid IL or missing references)
		//IL_0169: Unknown result type (might be due to invalid IL or missing references)
		//IL_016b: Unknown result type (might be due to invalid IL or missing references)
		//IL_016d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0178: Unknown result type (might be due to invalid IL or missing references)
		//IL_017d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0182: Unknown result type (might be due to invalid IL or missing references)
		//IL_0184: Unknown result type (might be due to invalid IL or missing references)
		//IL_0186: Unknown result type (might be due to invalid IL or missing references)
		//IL_0188: Unknown result type (might be due to invalid IL or missing references)
		//IL_018d: Unknown result type (might be due to invalid IL or missing references)
		//IL_018f: Unknown result type (might be due to invalid IL or missing references)
		//IL_019a: Unknown result type (might be due to invalid IL or missing references)
		//IL_019f: Unknown result type (might be due to invalid IL or missing references)
		//IL_01a4: Unknown result type (might be due to invalid IL or missing references)
		//IL_01a6: Unknown result type (might be due to invalid IL or missing references)
		//IL_01a8: Unknown result type (might be due to invalid IL or missing references)
		//IL_01aa: Unknown result type (might be due to invalid IL or missing references)
		//IL_01af: Unknown result type (might be due to invalid IL or missing references)
		//IL_01b6: Expected O, but got Unknown
		//IL_01b6: Unknown result type (might be due to invalid IL or missing references)
		//IL_01b8: Unknown result type (might be due to invalid IL or missing references)
		//IL_01bd: Unknown result type (might be due to invalid IL or missing references)
		//IL_01c4: Expected O, but got Unknown
		//IL_01cc: Unknown result type (might be due to invalid IL or missing references)
		//IL_01ce: Unknown result type (might be due to invalid IL or missing references)
		//IL_01d3: Unknown result type (might be due to invalid IL or missing references)
		//IL_01d9: Unknown result type (might be due to invalid IL or missing references)
		//IL_01db: Unknown result type (might be due to invalid IL or missing references)
		//IL_01e0: Unknown result type (might be due to invalid IL or missing references)
		//IL_01e4: Unknown result type (might be due to invalid IL or missing references)
		//IL_01ed: Unknown result type (might be due to invalid IL or missing references)
		//IL_0201: Unknown result type (might be due to invalid IL or missing references)
		//IL_0203: Unknown result type (might be due to invalid IL or missing references)
		//IL_0217: Unknown result type (might be due to invalid IL or missing references)
		//IL_0219: Unknown result type (might be due to invalid IL or missing references)
		//IL_021e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0224: Unknown result type (might be due to invalid IL or missing references)
		//IL_0226: Unknown result type (might be due to invalid IL or missing references)
		//IL_022b: Unknown result type (might be due to invalid IL or missing references)
		//IL_022f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0238: Unknown result type (might be due to invalid IL or missing references)
		//IL_024c: Unknown result type (might be due to invalid IL or missing references)
		//IL_024e: Unknown result type (might be due to invalid IL or missing references)
		//IL_028b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0290: Unknown result type (might be due to invalid IL or missing references)
		//IL_0296: Unknown result type (might be due to invalid IL or missing references)
		//IL_029b: Unknown result type (might be due to invalid IL or missing references)
		//IL_02a1: Unknown result type (might be due to invalid IL or missing references)
		//IL_02a6: Unknown result type (might be due to invalid IL or missing references)
		//IL_02ac: Unknown result type (might be due to invalid IL or missing references)
		//IL_02b1: Unknown result type (might be due to invalid IL or missing references)
		//IL_02cb: Unknown result type (might be due to invalid IL or missing references)
		//IL_02cd: Unknown result type (might be due to invalid IL or missing references)
		//IL_02d8: Unknown result type (might be due to invalid IL or missing references)
		//IL_02da: Unknown result type (might be due to invalid IL or missing references)
		//IL_0301: Unknown result type (might be due to invalid IL or missing references)
		//IL_0306: Unknown result type (might be due to invalid IL or missing references)
		//IL_0314: Unknown result type (might be due to invalid IL or missing references)
		//IL_0319: Unknown result type (might be due to invalid IL or missing references)
		//IL_032d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0332: Unknown result type (might be due to invalid IL or missing references)
		//IL_0340: Unknown result type (might be due to invalid IL or missing references)
		//IL_0345: Unknown result type (might be due to invalid IL or missing references)
		//IL_036e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0373: Unknown result type (might be due to invalid IL or missing references)
		//IL_0379: Unknown result type (might be due to invalid IL or missing references)
		//IL_037e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0384: Unknown result type (might be due to invalid IL or missing references)
		//IL_0389: Unknown result type (might be due to invalid IL or missing references)
		//IL_038f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0394: Unknown result type (might be due to invalid IL or missing references)
		//IL_03ae: Unknown result type (might be due to invalid IL or missing references)
		//IL_03b0: Unknown result type (might be due to invalid IL or missing references)
		//IL_03bb: Unknown result type (might be due to invalid IL or missing references)
		//IL_03bd: Unknown result type (might be due to invalid IL or missing references)
		//IL_03e4: Unknown result type (might be due to invalid IL or missing references)
		//IL_03e9: Unknown result type (might be due to invalid IL or missing references)
		//IL_03f7: Unknown result type (might be due to invalid IL or missing references)
		//IL_03fc: Unknown result type (might be due to invalid IL or missing references)
		//IL_0410: Unknown result type (might be due to invalid IL or missing references)
		//IL_0415: Unknown result type (might be due to invalid IL or missing references)
		//IL_0423: Unknown result type (might be due to invalid IL or missing references)
		//IL_0428: Unknown result type (might be due to invalid IL or missing references)
		innerSrf = new List<Brep>();
		outerSrf = new List<Brep>();
		if (!(gem.Duplicate() is CSGemData cSGemData))
		{
			return;
		}
		cSGemData.GetTransform(out var value);
		Transform transform = default(Transform);
		((Transform)(ref value)).TryGetInverse(ref transform);
		cSGemData.ApplyTransform(transform);
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
			Vector3d val = pointAtStart2 - pointAtStart;
			Vector3d val2 = pointAtEnd2 - pointAtEnd;
			Point3d val3 = pointAtStart - val;
			Point3d val4 = pointAtStart2 + val * 5.0;
			Point3d val5 = pointAtEnd - val2;
			Point3d val6 = pointAtEnd2 + val2 * 5.0;
			LineCurve intersectCrv = new LineCurve(new Line(val3, val4));
			LineCurve intersectCrv2 = new LineCurve(new Line(val5, val6));
			if (buildInner)
			{
				Point3d val7 = HeadIntersect(startCrv, (Curve)(object)intersectCrv, pointAtStart);
				Point3d val8 = HeadIntersect(endCrv, (Curve)(object)intersectCrv2, pointAtEnd);
				list3.Add(val7);
				list5.Add(val8);
				list[i] = FixCurve(list[i], val7, val8);
			}
			if (buildOuter)
			{
				Point3d val9 = HeadIntersect(startCrv2, (Curve)(object)intersectCrv, pointAtStart2);
				Point3d val10 = HeadIntersect(endCrv2, (Curve)(object)intersectCrv2, pointAtEnd2);
				list4.Add(val9);
				list6.Add(val10);
				list2[i] = FixCurve(list2[i], val9, val10);
			}
		}
		int num = prongParams.Length;
		for (int j = 0; j < num; j++)
		{
			int index = (j + 1) % num;
			Interval domain;
			if (buildInner)
			{
				Point3d p = list3[j];
				Point3d p2 = list5[j];
				Point3d p3 = list3[index];
				Point3d p4 = list5[index];
				Curve val11 = list[j];
				Curve val12 = list[index];
				Curve val13 = TrimToPoints(startCrv, p, p3);
				Curve val14 = TrimToPoints(endCrv, p2, p4);
				Curve[] profile = new Curve[2] { val11, val12 };
				double[] array = new double[2];
				domain = val13.Domain;
				array[0] = ((Interval)(ref domain)).Min;
				domain = val13.Domain;
				array[1] = ((Interval)(ref domain)).Max;
				double[] array2 = new double[2];
				domain = val14.Domain;
				array2[0] = ((Interval)(ref domain)).Min;
				domain = val14.Domain;
				array2[1] = ((Interval)(ref domain)).Max;
				CSUtilSweepRC.Sweep2(val13, val14, (Curve[])(object)profile, array, array2, out var breps, closed: false, maintainHeight: false);
				innerSrf.Add(breps);
			}
			if (buildOuter)
			{
				Point3d p5 = list4[j];
				Point3d p6 = list6[j];
				Point3d p7 = list4[index];
				Point3d p8 = list6[index];
				Curve val15 = list2[j];
				Curve val16 = list2[index];
				Curve val17 = TrimToPoints(startCrv2, p5, p7);
				Curve val18 = TrimToPoints(endCrv2, p6, p8);
				Curve[] profile2 = new Curve[2] { val15, val16 };
				double[] array3 = new double[2];
				domain = val17.Domain;
				array3[0] = ((Interval)(ref domain)).Min;
				domain = val17.Domain;
				array3[1] = ((Interval)(ref domain)).Max;
				double[] array4 = new double[2];
				domain = val18.Domain;
				array4[0] = ((Interval)(ref domain)).Min;
				domain = val18.Domain;
				array4[1] = ((Interval)(ref domain)).Max;
				CSUtilSweepRC.Sweep2(val17, val18, (Curve[])(object)profile2, array3, array4, out var breps2, closed: false, maintainHeight: false);
				outerSrf.Add(breps2);
			}
		}
	}

	private static bool BezelRevolveInnerOuter(Curve prongRail, double prongTopY, double prongTopOffset, double prongBaseY, double prongBaseOffset, ValueListItem baseStyle, double prongBaseDomeHeight, bool planarDome, out Curve innerRailCrv, out Curve outerRailCrv, out string error)
	{
		//IL_0029: Unknown result type (might be due to invalid IL or missing references)
		//IL_004f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0075: Unknown result type (might be due to invalid IL or missing references)
		//IL_009b: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c1: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e7: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f1: Unknown result type (might be due to invalid IL or missing references)
		//IL_0117: Unknown result type (might be due to invalid IL or missing references)
		//IL_013d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0163: Unknown result type (might be due to invalid IL or missing references)
		//IL_0189: Unknown result type (might be due to invalid IL or missing references)
		//IL_01af: Unknown result type (might be due to invalid IL or missing references)
		//IL_01d5: Unknown result type (might be due to invalid IL or missing references)
		//IL_01df: Unknown result type (might be due to invalid IL or missing references)
		//IL_01e5: Expected O, but got Unknown
		//IL_01e6: Unknown result type (might be due to invalid IL or missing references)
		//IL_01eb: Unknown result type (might be due to invalid IL or missing references)
		//IL_01fe: Unknown result type (might be due to invalid IL or missing references)
		//IL_022f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0234: Unknown result type (might be due to invalid IL or missing references)
		//IL_0235: Unknown result type (might be due to invalid IL or missing references)
		//IL_023a: Unknown result type (might be due to invalid IL or missing references)
		//IL_023b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0240: Unknown result type (might be due to invalid IL or missing references)
		//IL_0245: Unknown result type (might be due to invalid IL or missing references)
		//IL_024a: Unknown result type (might be due to invalid IL or missing references)
		//IL_024c: Unknown result type (might be due to invalid IL or missing references)
		//IL_024d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0252: Unknown result type (might be due to invalid IL or missing references)
		//IL_0258: Unknown result type (might be due to invalid IL or missing references)
		//IL_025d: Unknown result type (might be due to invalid IL or missing references)
		//IL_025e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0263: Unknown result type (might be due to invalid IL or missing references)
		//IL_0268: Unknown result type (might be due to invalid IL or missing references)
		//IL_0273: Unknown result type (might be due to invalid IL or missing references)
		//IL_0296: Unknown result type (might be due to invalid IL or missing references)
		//IL_029d: Unknown result type (might be due to invalid IL or missing references)
		//IL_02a2: Unknown result type (might be due to invalid IL or missing references)
		//IL_02ab: Unknown result type (might be due to invalid IL or missing references)
		//IL_02b0: Unknown result type (might be due to invalid IL or missing references)
		//IL_02b5: Unknown result type (might be due to invalid IL or missing references)
		//IL_02e3: Unknown result type (might be due to invalid IL or missing references)
		//IL_02fe: Unknown result type (might be due to invalid IL or missing references)
		//IL_0303: Unknown result type (might be due to invalid IL or missing references)
		//IL_0318: Unknown result type (might be due to invalid IL or missing references)
		//IL_031d: Unknown result type (might be due to invalid IL or missing references)
		//IL_031f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0324: Unknown result type (might be due to invalid IL or missing references)
		//IL_0329: Unknown result type (might be due to invalid IL or missing references)
		//IL_032e: Unknown result type (might be due to invalid IL or missing references)
		//IL_032f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0334: Unknown result type (might be due to invalid IL or missing references)
		//IL_0339: Unknown result type (might be due to invalid IL or missing references)
		//IL_033e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0343: Unknown result type (might be due to invalid IL or missing references)
		//IL_034e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0350: Unknown result type (might be due to invalid IL or missing references)
		//IL_0352: Unknown result type (might be due to invalid IL or missing references)
		//IL_0357: Unknown result type (might be due to invalid IL or missing references)
		//IL_0359: Unknown result type (might be due to invalid IL or missing references)
		//IL_0364: Unknown result type (might be due to invalid IL or missing references)
		//IL_0372: Unknown result type (might be due to invalid IL or missing references)
		//IL_0378: Unknown result type (might be due to invalid IL or missing references)
		//IL_037d: Unknown result type (might be due to invalid IL or missing references)
		//IL_037e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0383: Unknown result type (might be due to invalid IL or missing references)
		//IL_0394: Unknown result type (might be due to invalid IL or missing references)
		//IL_0399: Unknown result type (might be due to invalid IL or missing references)
		//IL_03c2: Unknown result type (might be due to invalid IL or missing references)
		//IL_03c8: Unknown result type (might be due to invalid IL or missing references)
		//IL_03cd: Unknown result type (might be due to invalid IL or missing references)
		//IL_03d2: Unknown result type (might be due to invalid IL or missing references)
		//IL_038b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0457: Unknown result type (might be due to invalid IL or missing references)
		//IL_045c: Unknown result type (might be due to invalid IL or missing references)
		//IL_045e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0476: Unknown result type (might be due to invalid IL or missing references)
		//IL_047b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0491: Unknown result type (might be due to invalid IL or missing references)
		//IL_0496: Unknown result type (might be due to invalid IL or missing references)
		//IL_0499: Unknown result type (might be due to invalid IL or missing references)
		//IL_049e: Unknown result type (might be due to invalid IL or missing references)
		//IL_04a0: Unknown result type (might be due to invalid IL or missing references)
		//IL_04a5: Unknown result type (might be due to invalid IL or missing references)
		//IL_04aa: Unknown result type (might be due to invalid IL or missing references)
		//IL_04af: Unknown result type (might be due to invalid IL or missing references)
		//IL_04b1: Unknown result type (might be due to invalid IL or missing references)
		//IL_04b6: Unknown result type (might be due to invalid IL or missing references)
		//IL_04bb: Unknown result type (might be due to invalid IL or missing references)
		//IL_04bd: Unknown result type (might be due to invalid IL or missing references)
		//IL_04c2: Unknown result type (might be due to invalid IL or missing references)
		//IL_04c7: Unknown result type (might be due to invalid IL or missing references)
		//IL_04d3: Unknown result type (might be due to invalid IL or missing references)
		//IL_04d5: Unknown result type (might be due to invalid IL or missing references)
		//IL_04d7: Unknown result type (might be due to invalid IL or missing references)
		//IL_04dc: Unknown result type (might be due to invalid IL or missing references)
		//IL_04de: Unknown result type (might be due to invalid IL or missing references)
		//IL_0504: Unknown result type (might be due to invalid IL or missing references)
		//IL_0509: Unknown result type (might be due to invalid IL or missing references)
		//IL_0516: Unknown result type (might be due to invalid IL or missing references)
		//IL_051b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0568: Unknown result type (might be due to invalid IL or missing references)
		//IL_0581: Unknown result type (might be due to invalid IL or missing references)
		//IL_059a: Unknown result type (might be due to invalid IL or missing references)
		//IL_05b3: Unknown result type (might be due to invalid IL or missing references)
		//IL_05f0: Unknown result type (might be due to invalid IL or missing references)
		//IL_05f5: Unknown result type (might be due to invalid IL or missing references)
		//IL_05fc: Unknown result type (might be due to invalid IL or missing references)
		//IL_0601: Unknown result type (might be due to invalid IL or missing references)
		//IL_0605: Unknown result type (might be due to invalid IL or missing references)
		//IL_060a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0615: Unknown result type (might be due to invalid IL or missing references)
		//IL_061a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0630: Unknown result type (might be due to invalid IL or missing references)
		//IL_0635: Unknown result type (might be due to invalid IL or missing references)
		//IL_0638: Unknown result type (might be due to invalid IL or missing references)
		//IL_063d: Unknown result type (might be due to invalid IL or missing references)
		//IL_063f: Unknown result type (might be due to invalid IL or missing references)
		//IL_065c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0661: Unknown result type (might be due to invalid IL or missing references)
		//IL_0666: Unknown result type (might be due to invalid IL or missing references)
		//IL_0683: Unknown result type (might be due to invalid IL or missing references)
		//IL_0688: Unknown result type (might be due to invalid IL or missing references)
		//IL_068d: Unknown result type (might be due to invalid IL or missing references)
		//IL_06aa: Unknown result type (might be due to invalid IL or missing references)
		//IL_06af: Unknown result type (might be due to invalid IL or missing references)
		//IL_06b4: Unknown result type (might be due to invalid IL or missing references)
		//IL_06d1: Unknown result type (might be due to invalid IL or missing references)
		//IL_06d6: Unknown result type (might be due to invalid IL or missing references)
		//IL_06fe: Unknown result type (might be due to invalid IL or missing references)
		//IL_0703: Unknown result type (might be due to invalid IL or missing references)
		//IL_0710: Unknown result type (might be due to invalid IL or missing references)
		//IL_0715: Unknown result type (might be due to invalid IL or missing references)
		//IL_0731: Unknown result type (might be due to invalid IL or missing references)
		//IL_0736: Unknown result type (might be due to invalid IL or missing references)
		//IL_0743: Unknown result type (might be due to invalid IL or missing references)
		//IL_0748: Unknown result type (might be due to invalid IL or missing references)
		//IL_0761: Unknown result type (might be due to invalid IL or missing references)
		//IL_0766: Unknown result type (might be due to invalid IL or missing references)
		//IL_0772: Unknown result type (might be due to invalid IL or missing references)
		//IL_0777: Unknown result type (might be due to invalid IL or missing references)
		//IL_078e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0793: Unknown result type (might be due to invalid IL or missing references)
		//IL_079f: Unknown result type (might be due to invalid IL or missing references)
		//IL_07a4: Unknown result type (might be due to invalid IL or missing references)
		//IL_07bb: Unknown result type (might be due to invalid IL or missing references)
		//IL_07c6: Unknown result type (might be due to invalid IL or missing references)
		innerRailCrv = null;
		outerRailCrv = null;
		PolylineCurve val = new PolylineCurve((IEnumerable<Point3d>)new List<Point3d>
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
		double d = CSBlockUtil.CSUtilMath.AngleBetweenVectorsWithDirection(inputVector, obj.TangentAt(((Interval)(ref domain)).Min), -Vector3d.YAxis);
		double num = (planarDome ? (1.0 / Math.Cos(d)) : 1.0);
		Transform val7 = Transform.Scale(Plane.WorldXY, 1.0, prongTopY * num, 0.0);
		Transform val8 = Transform.Translation(0.0, prongTopOffset, 0.0);
		Transform val9 = CSBlockUtil.CSUtilMath.Orient3Point(Point3d.Origin, Vector3d.XAxis, Vector3d.ZAxis, val4, -Vector3d.YAxis, Vector3d.ZAxis);
		Curve val10 = ((Curve)val).DuplicateCurve();
		((GeometryBase)val10).Transform(val9 * val8 * val7);
		Transform val11 = CSBlockUtil.CSUtilMath.Rotation(Vector3d.ZAxis * -1.0, prongRail.TangentAtStart, val4);
		if (!planarDome)
		{
			((GeometryBase)val10).Transform(val11);
		}
		Point3d val12 = prongRail.PointAtEnd;
		double num2 = ((Point3d)(ref val12)).Z + prongBaseDomeHeight;
		double num3 = CSBlockUtil.CSUtilMath.AngleBetweenVectorsWithDirection(new Vector3d(0.0, 0.0, -1.0), prongRail.TangentAtEnd, -Vector3d.YAxis);
		double num4 = ((num3 < 0.0) ? (Math.Tan(0.0 - num3) * prongBaseY + 0.05) : 0.05);
		if (baseStyle != ValueListItem.HeadBuilderProngHead)
		{
			num4 = ((num3 < 0.0) ? (Math.Tan(0.0 - num3) * prongBaseY + 0.05) : (Math.Tan(num3) * prongBaseY + 0.05));
		}
		prongRail = prongRail.Extend((CurveEnd)2, num4, (CurveExtensionStyle)0);
		Point3d pointAtEnd = prongRail.PointAtEnd;
		Transform val13 = Transform.Scale(Plane.WorldXY, 1.0, prongBaseY, 0.0);
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
		Brep val17 = ((Surface)NurbsSurface.CreateFromCorners(new Point3d(20.0, 20.0, num2), new Point3d(-20.0, 20.0, num2), new Point3d(-20.0, -20.0, num2), new Point3d(20.0, -20.0, num2))).ToBrep();
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
		val12 = array3[0].PointAtStart;
		double x = ((Point3d)(ref val12)).X;
		val12 = array3[1].PointAtStart;
		innerRailCrv = ((x < ((Point3d)(ref val12)).X) ? array3[0] : array3[1]);
		val12 = array3[0].PointAtStart;
		double x2 = ((Point3d)(ref val12)).X;
		val12 = array3[1].PointAtStart;
		outerRailCrv = ((x2 < ((Point3d)(ref val12)).X) ? array3[1] : array3[0]);
		val12 = innerRailCrv.PointAtStart;
		double z2 = ((Point3d)(ref val12)).Z;
		val12 = innerRailCrv.PointAtEnd;
		if (z2 < ((Point3d)(ref val12)).Z)
		{
			innerRailCrv.Reverse();
		}
		val12 = outerRailCrv.PointAtStart;
		double z3 = ((Point3d)(ref val12)).Z;
		val12 = outerRailCrv.PointAtEnd;
		if (z3 < ((Point3d)(ref val12)).Z)
		{
			outerRailCrv.Reverse();
		}
		((GeometryBase)innerRailCrv).Transform(val6);
		((GeometryBase)outerRailCrv).Transform(val6);
		error = "";
		return true;
	}

	private static Curve FixCurve(Curve crvIn, Point3d p0, Point3d p1)
	{
		//IL_0007: Unknown result type (might be due to invalid IL or missing references)
		//IL_0009: Unknown result type (might be due to invalid IL or missing references)
		//IL_000e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0013: Unknown result type (might be due to invalid IL or missing references)
		//IL_0014: Unknown result type (might be due to invalid IL or missing references)
		//IL_0016: Unknown result type (might be due to invalid IL or missing references)
		//IL_001b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0020: Unknown result type (might be due to invalid IL or missing references)
		//IL_0022: Unknown result type (might be due to invalid IL or missing references)
		//IL_0027: Unknown result type (might be due to invalid IL or missing references)
		//IL_003e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0043: Unknown result type (might be due to invalid IL or missing references)
		//IL_0047: Unknown result type (might be due to invalid IL or missing references)
		//IL_004c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0057: Unknown result type (might be due to invalid IL or missing references)
		//IL_0059: Unknown result type (might be due to invalid IL or missing references)
		//IL_0068: Unknown result type (might be due to invalid IL or missing references)
		//IL_006d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0072: Unknown result type (might be due to invalid IL or missing references)
		//IL_0074: Unknown result type (might be due to invalid IL or missing references)
		//IL_0076: Unknown result type (might be due to invalid IL or missing references)
		//IL_007b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0080: Unknown result type (might be due to invalid IL or missing references)
		//IL_0085: Unknown result type (might be due to invalid IL or missing references)
		//IL_0089: Unknown result type (might be due to invalid IL or missing references)
		//IL_0098: Unknown result type (might be due to invalid IL or missing references)
		NurbsCurve val = crvIn.ToNurbsCurve();
		Vector3d val2 = p0 - ((Curve)val).PointAtStart;
		Vector3d val3 = p1 - ((Curve)val).PointAtEnd;
		Point3d pointAtEnd = ((Curve)val).PointAtEnd;
		double z = ((Point3d)(ref pointAtEnd)).Z;
		for (int i = 0; i < val.Points.Count; i++)
		{
			ControlPoint val4 = val.Points[i];
			Point3d val5 = ((ControlPoint)(ref val4)).Location;
			double z2 = ((Point3d)(ref val5)).Z;
			val5 += val2 * (1.0 - z2 / z);
			val5 = (((ControlPoint)(ref val4)).Location = val5 + val3 * (z2 / z));
			val.Points[i] = val4;
		}
		return (Curve)(object)val;
	}

	private static Point3d HeadIntersect(Curve headCrv, Curve intersectCrv, Point3d closestPt)
	{
		//IL_0034: Unknown result type (might be due to invalid IL or missing references)
		//IL_0039: Unknown result type (might be due to invalid IL or missing references)
		//IL_0024: Unknown result type (might be due to invalid IL or missing references)
		//IL_004b: Unknown result type (might be due to invalid IL or missing references)
		//IL_004e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0053: Unknown result type (might be due to invalid IL or missing references)
		//IL_0058: Unknown result type (might be due to invalid IL or missing references)
		//IL_006d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0072: Unknown result type (might be due to invalid IL or missing references)
		//IL_0087: Unknown result type (might be due to invalid IL or missing references)
		CurveIntersections val = Intersection.CurveCurve(headCrv, intersectCrv, CSApplicationHelper.Application.Tolerance, CSApplicationHelper.Application.Tolerance);
		if (val.Count == 0)
		{
			return Point3d.Unset;
		}
		double num = 999.0;
		Point3d result = Point3d.Unset;
		foreach (IntersectionEvent item in val)
		{
			Vector3d val2 = closestPt - item.PointA;
			double length = ((Vector3d)(ref val2)).Length;
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
		//IL_0063: Unknown result type (might be due to invalid IL or missing references)
		//IL_0068: Unknown result type (might be due to invalid IL or missing references)
		//IL_006b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0070: Unknown result type (might be due to invalid IL or missing references)
		//IL_0073: Unknown result type (might be due to invalid IL or missing references)
		//IL_0078: Unknown result type (might be due to invalid IL or missing references)
		//IL_0095: Unknown result type (might be due to invalid IL or missing references)
		//IL_009a: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a7: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ac: Unknown result type (might be due to invalid IL or missing references)
		//IL_00da: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d2: Unknown result type (might be due to invalid IL or missing references)
		//IL_00df: Unknown result type (might be due to invalid IL or missing references)
		//IL_0107: Unknown result type (might be due to invalid IL or missing references)
		//IL_0119: Unknown result type (might be due to invalid IL or missing references)
		//IL_011e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0120: Unknown result type (might be due to invalid IL or missing references)
		//IL_0146: Unknown result type (might be due to invalid IL or missing references)
		//IL_014b: Unknown result type (might be due to invalid IL or missing references)
		//IL_014e: Unknown result type (might be due to invalid IL or missing references)
		//IL_01f7: Unknown result type (might be due to invalid IL or missing references)
		//IL_0209: Unknown result type (might be due to invalid IL or missing references)
		//IL_0215: Unknown result type (might be due to invalid IL or missing references)
		startCrv = null;
		endCrv = null;
		if (!GemOffsetCurve.GemOffset(copyGem.GemShape, copyGem, 0.0, out Curve curve, false, out error, 0.0, 0.0, 0.0))
		{
			error = "CurveTop Inside:" + error;
			return false;
		}
		NurbsCurve val = bezelProngOutline1.ToNurbsCurve();
		Point3d val2 = copyGem.GetCurve().PointAt(copyGem.BezelParam);
		BoundingBox boundingBox = ((GeometryBase)headBase).GetBoundingBox(true);
		Point3d center = ((BoundingBox)(ref boundingBox)).Center;
		double z = ((Point3d)(ref center)).Z;
		double gemXSize = copyGem.GemXSize;
		double gemYSize = copyGem.GemYSize;
		Vector3d diagonal = ((BoundingBox)(ref boundingBox)).Diagonal;
		double x = ((Vector3d)(ref diagonal)).X;
		diagonal = ((BoundingBox)(ref boundingBox)).Diagonal;
		double y = ((Vector3d)(ref diagonal)).Y;
		double num = gemXSize - x;
		double num2 = gemYSize - y;
		for (int i = 0; i < 2; i++)
		{
			Point3d val3 = ((i == 0) ? ((Curve)val).PointAtStart : ((Curve)val).PointAtEnd);
			double num3 = ((Point3d)(ref val3)).Z / z;
			double num4 = (gemXSize - num * num3) / gemXSize;
			double num5 = (gemYSize - num2 * num3) / gemYSize;
			Transform transform = Transform.Scale(Plane.WorldXY, num4, num5, 1.0);
			Transform transform2 = Transform.Scale(Plane.WorldXY, 1.0 / num4, 1.0 / num5, 1.0);
			copyGem.ApplyTransform(transform);
			double distance = ((Point3d)(ref val3)).X - ((Point3d)(ref val2)).X + num / 2.0 * num3;
			if (copyGem.GemShape == "BaguetteTapered")
			{
				distance = -1.0 * (((Point3d)(ref val3)).Y - ((Point3d)(ref val2)).Y) + num2 / 2.0 * num3;
			}
			if (!GemOffsetCurve.GemOffset(copyGem.GemShape, copyGem, distance, out curve, false, out error, 0.0, 0.0, 0.0))
			{
				error = "CurveTop Inside:" + error;
				return false;
			}
			copyGem.ApplyTransform(transform2);
			NurbsCurve val4 = curve.ToNurbsCurve();
			((GeometryBase)val4).Translate(Vector3d.ZAxis * ((Point3d)(ref val3)).Z);
			if (i == 0)
			{
				startCrv = (Curve)(object)val4;
			}
			else
			{
				endCrv = (Curve)(object)val4;
			}
		}
		error = "";
		return true;
	}

	private static Curve TrimToPoints(Curve crv, Point3d p0, Point3d p1)
	{
		//IL_0007: Unknown result type (might be due to invalid IL or missing references)
		//IL_0019: Unknown result type (might be due to invalid IL or missing references)
		//IL_0023: Unknown result type (might be due to invalid IL or missing references)
		//IL_0028: Unknown result type (might be due to invalid IL or missing references)
		Curve obj = crv.DuplicateCurve();
		double num = default(double);
		obj.ClosestPoint(p0, ref num);
		obj.ChangeClosedCurveSeam(num);
		obj.ClosestPoint(p1, ref num);
		Interval domain = obj.Domain;
		return obj.Trim(((Interval)(ref domain)).Min, num);
	}
}
