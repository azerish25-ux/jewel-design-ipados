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
		//IL_00a4: Unknown result type (might be due to invalid IL or missing references)
		//IL_010f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0127: Unknown result type (might be due to invalid IL or missing references)
		//IL_012c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0141: Unknown result type (might be due to invalid IL or missing references)
		//IL_0146: Unknown result type (might be due to invalid IL or missing references)
		//IL_0148: Unknown result type (might be due to invalid IL or missing references)
		//IL_014d: Unknown result type (might be due to invalid IL or missing references)
		//IL_015a: Unknown result type (might be due to invalid IL or missing references)
		//IL_01c0: Unknown result type (might be due to invalid IL or missing references)
		//IL_01f2: Unknown result type (might be due to invalid IL or missing references)
		//IL_01f7: Unknown result type (might be due to invalid IL or missing references)
		//IL_0219: Unknown result type (might be due to invalid IL or missing references)
		//IL_021e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0220: Unknown result type (might be due to invalid IL or missing references)
		//IL_0225: Unknown result type (might be due to invalid IL or missing references)
		//IL_0232: Unknown result type (might be due to invalid IL or missing references)
		//IL_0265: Unknown result type (might be due to invalid IL or missing references)
		//IL_0297: Unknown result type (might be due to invalid IL or missing references)
		//IL_029c: Unknown result type (might be due to invalid IL or missing references)
		//IL_02b1: Unknown result type (might be due to invalid IL or missing references)
		//IL_02b6: Unknown result type (might be due to invalid IL or missing references)
		//IL_02b8: Unknown result type (might be due to invalid IL or missing references)
		//IL_02bd: Unknown result type (might be due to invalid IL or missing references)
		//IL_02ca: Unknown result type (might be due to invalid IL or missing references)
		//IL_02ef: Unknown result type (might be due to invalid IL or missing references)
		//IL_032e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0352: Unknown result type (might be due to invalid IL or missing references)
		//IL_035f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0367: Unknown result type (might be due to invalid IL or missing references)
		//IL_036f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0377: Unknown result type (might be due to invalid IL or missing references)
		//IL_03a2: Unknown result type (might be due to invalid IL or missing references)
		//IL_03e1: Unknown result type (might be due to invalid IL or missing references)
		//IL_0405: Unknown result type (might be due to invalid IL or missing references)
		//IL_0412: Unknown result type (might be due to invalid IL or missing references)
		//IL_041a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0422: Unknown result type (might be due to invalid IL or missing references)
		//IL_042a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0486: Unknown result type (might be due to invalid IL or missing references)
		//IL_048b: Unknown result type (might be due to invalid IL or missing references)
		//IL_048d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0492: Unknown result type (might be due to invalid IL or missing references)
		//IL_049d: Unknown result type (might be due to invalid IL or missing references)
		//IL_04a2: Unknown result type (might be due to invalid IL or missing references)
		//IL_04a9: Expected O, but got Unknown
		//IL_04ae: Unknown result type (might be due to invalid IL or missing references)
		//IL_04b3: Unknown result type (might be due to invalid IL or missing references)
		//IL_04b8: Unknown result type (might be due to invalid IL or missing references)
		//IL_04bd: Unknown result type (might be due to invalid IL or missing references)
		//IL_04c1: Unknown result type (might be due to invalid IL or missing references)
		//IL_0783: Unknown result type (might be due to invalid IL or missing references)
		//IL_0788: Unknown result type (might be due to invalid IL or missing references)
		//IL_078a: Unknown result type (might be due to invalid IL or missing references)
		//IL_078f: Unknown result type (might be due to invalid IL or missing references)
		//IL_079a: Unknown result type (might be due to invalid IL or missing references)
		//IL_079f: Unknown result type (might be due to invalid IL or missing references)
		//IL_07a6: Expected O, but got Unknown
		//IL_07ab: Unknown result type (might be due to invalid IL or missing references)
		//IL_07b0: Unknown result type (might be due to invalid IL or missing references)
		//IL_07b5: Unknown result type (might be due to invalid IL or missing references)
		//IL_07ba: Unknown result type (might be due to invalid IL or missing references)
		//IL_07be: Unknown result type (might be due to invalid IL or missing references)
		//IL_0508: Unknown result type (might be due to invalid IL or missing references)
		//IL_0514: Unknown result type (might be due to invalid IL or missing references)
		//IL_051d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0526: Unknown result type (might be due to invalid IL or missing references)
		//IL_052f: Unknown result type (might be due to invalid IL or missing references)
		//IL_080c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0818: Unknown result type (might be due to invalid IL or missing references)
		//IL_0821: Unknown result type (might be due to invalid IL or missing references)
		//IL_082a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0833: Unknown result type (might be due to invalid IL or missing references)
		//IL_083c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0845: Unknown result type (might be due to invalid IL or missing references)
		//IL_0548: Unknown result type (might be due to invalid IL or missing references)
		//IL_0551: Unknown result type (might be due to invalid IL or missing references)
		//IL_053f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0544: Unknown result type (might be due to invalid IL or missing references)
		//IL_094f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0957: Unknown result type (might be due to invalid IL or missing references)
		//IL_095f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0967: Unknown result type (might be due to invalid IL or missing references)
		//IL_0970: Unknown result type (might be due to invalid IL or missing references)
		//IL_0972: Unknown result type (might be due to invalid IL or missing references)
		//IL_0870: Unknown result type (might be due to invalid IL or missing references)
		//IL_0875: Unknown result type (might be due to invalid IL or missing references)
		//IL_0877: Unknown result type (might be due to invalid IL or missing references)
		//IL_0889: Unknown result type (might be due to invalid IL or missing references)
		//IL_0893: Unknown result type (might be due to invalid IL or missing references)
		//IL_0898: Unknown result type (might be due to invalid IL or missing references)
		//IL_089c: Unknown result type (might be due to invalid IL or missing references)
		//IL_089e: Unknown result type (might be due to invalid IL or missing references)
		//IL_08aa: Unknown result type (might be due to invalid IL or missing references)
		//IL_08ac: Unknown result type (might be due to invalid IL or missing references)
		//IL_08d1: Unknown result type (might be due to invalid IL or missing references)
		//IL_08d6: Unknown result type (might be due to invalid IL or missing references)
		//IL_08d8: Unknown result type (might be due to invalid IL or missing references)
		//IL_08ea: Unknown result type (might be due to invalid IL or missing references)
		//IL_090d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0912: Unknown result type (might be due to invalid IL or missing references)
		//IL_0916: Unknown result type (might be due to invalid IL or missing references)
		//IL_0918: Unknown result type (might be due to invalid IL or missing references)
		//IL_092a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0934: Unknown result type (might be due to invalid IL or missing references)
		//IL_0939: Unknown result type (might be due to invalid IL or missing references)
		//IL_093d: Unknown result type (might be due to invalid IL or missing references)
		//IL_093f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0657: Unknown result type (might be due to invalid IL or missing references)
		//IL_065f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0667: Unknown result type (might be due to invalid IL or missing references)
		//IL_066f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0678: Unknown result type (might be due to invalid IL or missing references)
		//IL_067a: Unknown result type (might be due to invalid IL or missing references)
		//IL_057c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0581: Unknown result type (might be due to invalid IL or missing references)
		//IL_0583: Unknown result type (might be due to invalid IL or missing references)
		//IL_0595: Unknown result type (might be due to invalid IL or missing references)
		//IL_059e: Unknown result type (might be due to invalid IL or missing references)
		//IL_05a3: Unknown result type (might be due to invalid IL or missing references)
		//IL_05a7: Unknown result type (might be due to invalid IL or missing references)
		//IL_05a9: Unknown result type (might be due to invalid IL or missing references)
		//IL_05b5: Unknown result type (might be due to invalid IL or missing references)
		//IL_05b7: Unknown result type (might be due to invalid IL or missing references)
		//IL_05dc: Unknown result type (might be due to invalid IL or missing references)
		//IL_05e1: Unknown result type (might be due to invalid IL or missing references)
		//IL_05e3: Unknown result type (might be due to invalid IL or missing references)
		//IL_05f5: Unknown result type (might be due to invalid IL or missing references)
		//IL_0616: Unknown result type (might be due to invalid IL or missing references)
		//IL_061b: Unknown result type (might be due to invalid IL or missing references)
		//IL_061f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0621: Unknown result type (might be due to invalid IL or missing references)
		//IL_0633: Unknown result type (might be due to invalid IL or missing references)
		//IL_063c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0641: Unknown result type (might be due to invalid IL or missing references)
		//IL_0645: Unknown result type (might be due to invalid IL or missing references)
		//IL_0647: Unknown result type (might be due to invalid IL or missing references)
		//IL_0981: Unknown result type (might be due to invalid IL or missing references)
		//IL_0983: Unknown result type (might be due to invalid IL or missing references)
		//IL_0987: Unknown result type (might be due to invalid IL or missing references)
		//IL_0689: Unknown result type (might be due to invalid IL or missing references)
		//IL_068b: Unknown result type (might be due to invalid IL or missing references)
		//IL_068f: Unknown result type (might be due to invalid IL or missing references)
		//IL_09c5: Unknown result type (might be due to invalid IL or missing references)
		//IL_09ca: Unknown result type (might be due to invalid IL or missing references)
		//IL_09d2: Unknown result type (might be due to invalid IL or missing references)
		//IL_09d7: Unknown result type (might be due to invalid IL or missing references)
		//IL_09e4: Unknown result type (might be due to invalid IL or missing references)
		//IL_09e9: Unknown result type (might be due to invalid IL or missing references)
		//IL_0a01: Unknown result type (might be due to invalid IL or missing references)
		//IL_0a06: Unknown result type (might be due to invalid IL or missing references)
		//IL_0a13: Unknown result type (might be due to invalid IL or missing references)
		//IL_0a18: Unknown result type (might be due to invalid IL or missing references)
		//IL_0a2e: Unknown result type (might be due to invalid IL or missing references)
		//IL_06cc: Unknown result type (might be due to invalid IL or missing references)
		//IL_06d1: Unknown result type (might be due to invalid IL or missing references)
		//IL_06d9: Unknown result type (might be due to invalid IL or missing references)
		//IL_06de: Unknown result type (might be due to invalid IL or missing references)
		//IL_06eb: Unknown result type (might be due to invalid IL or missing references)
		//IL_06f0: Unknown result type (might be due to invalid IL or missing references)
		//IL_0708: Unknown result type (might be due to invalid IL or missing references)
		//IL_070d: Unknown result type (might be due to invalid IL or missing references)
		//IL_071a: Unknown result type (might be due to invalid IL or missing references)
		//IL_071f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0735: Unknown result type (might be due to invalid IL or missing references)
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
		((GeometryBase)curve).Transform(xDown);
		if (!GemOffsetCurve.GemOffset(cSGemData, 0.0, out Curve curve2, false, out error, cornerBlend))
		{
			return false;
		}
		((GeometryBase)curve2).MakeDeformable();
		double num = drop * Math.Tan(CSBlockUtil.CSUtilMath.ToRadians(angleX));
		double num2 = drop * Math.Tan(CSBlockUtil.CSUtilMath.ToRadians(angleY));
		double num3 = gemXSize + num * 2.0;
		double num4 = gemYSize + num2 * 2.0;
		Transform val = Transform.Scale(Plane.WorldXY, num3 / gemXSize, num4 / gemYSize, 1.0);
		Transform val2 = Transform.Translation(0.0, 0.0, drop) * val;
		Curve val3 = curve2.DuplicateCurve();
		((GeometryBase)val3).Transform(val2);
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
		Transform val4 = Transform.Scale(Plane.WorldXY, (num3 + num5 * 2.0) / gemXSize, (num4 + num6 * 2.0) / gemYSize, 1.0);
		Transform val5 = Transform.Translation(0.0, 0.0, drop * (1.0 - baseAnglePosition)) * val4;
		Curve val6 = curve2.DuplicateCurve();
		((GeometryBase)val6).Transform(val5);
		headMiddleCurve = val6;
		double num7 = prongHeight * Math.Tan(CSBlockUtil.CSUtilMath.ToRadians(angleX + topAngle));
		double num8 = prongHeight * Math.Tan(CSBlockUtil.CSUtilMath.ToRadians(angleY + topAngle));
		Transform val7 = Transform.Scale(Plane.WorldXY, (gemXSize + num7 * 2.0) / gemXSize, (gemYSize + num8 * 2.0) / gemYSize, 1.0);
		Transform val8 = Transform.Translation(0.0, 0.0, prongHeight) * val7;
		Curve val9 = curve2.DuplicateCurve();
		((GeometryBase)val9).Transform(val8);
		Point3d item = default(Point3d);
		((Point3d)(ref item))._002Ector(gemXMax, 0.0, 0.0);
		((Point3d)(ref item)).Transform(val8);
		Point3d item2 = default(Point3d);
		((Point3d)(ref item2))._002Ector(gemXMax, 0.0, 0.0);
		Point3d item3 = default(Point3d);
		((Point3d)(ref item3))._002Ector(gemXMax, 0.0, 0.0);
		((Point3d)(ref item3)).Transform(val5);
		Point3d item4 = default(Point3d);
		((Point3d)(ref item4))._002Ector(gemXMax, 0.0, 0.0);
		((Point3d)(ref item4)).Transform(val2);
		Curve val10 = ProngInterpCurve(new List<Point3d> { item, item2, item3, item4 });
		Point3d item5 = default(Point3d);
		((Point3d)(ref item5))._002Ector(0.0, gemYMax, 0.0);
		((Point3d)(ref item5)).Transform(val8);
		Point3d item6 = default(Point3d);
		((Point3d)(ref item6))._002Ector(0.0, gemYMax, 0.0);
		Point3d item7 = default(Point3d);
		((Point3d)(ref item7))._002Ector(0.0, gemYMax, 0.0);
		((Point3d)(ref item7)).Transform(val5);
		Point3d item8 = default(Point3d);
		((Point3d)(ref item8))._002Ector(0.0, gemYMax, 0.0);
		((Point3d)(ref item8)).Transform(val2);
		Curve val11 = ProngInterpCurve(new List<Point3d> { item5, item6, item7, item8 });
		List<CSGemInstanceDefinition.ProngPosition> list = new List<CSGemInstanceDefinition.ProngPosition>();
		if (cSGemData.Def != null)
		{
			list = cSGemData.Def.ProngPositions;
		}
		Point3d val16 = default(Point3d);
		Point3d item9 = default(Point3d);
		Point3d unset = default(Point3d);
		Point3d val17 = default(Point3d);
		Vector3d val18 = default(Vector3d);
		Point3d val20;
		foreach (ProngParam prongParam in prongParams)
		{
			double prongPointAngle = prongParam.ProngPointAngle;
			Point3d val12 = curve.PointAt(list[prongParam.ProngPoint].ProngParam);
			LineCurve val13 = new LineCurve(Point3d.Origin, val12 * 50.0);
			Transform val14 = CSBlockUtil.CSUtilMath.Rotation(CSBlockUtil.CSUtilMath.ToRadians(prongPointAngle), Vector3d.ZAxis, Point3d.Origin);
			((GeometryBase)val13).Transform(val14);
			CurveIntersections val15 = Intersection.CurveCurve(curve2, (Curve)(object)val13, 0.001, 0.001);
			if (val15.Count <= 0)
			{
				continue;
			}
			double parameterA = val15[0].ParameterA;
			((Point3d)(ref val16))._002Ector(curve2.PointAt(parameterA));
			((Point3d)(ref item9))._002Ector(val16);
			((Point3d)(ref item9)).Transform(val2);
			((Point3d)(ref unset))._002Ector(val16);
			((Point3d)(ref unset)).Transform(val5);
			if (prongParam.StraightProng)
			{
				unset = Point3d.Unset;
			}
			((Point3d)(ref val17))._002Ector(val16);
			((Point3d)(ref val17)).Transform(val8);
			if (baseStyle == ValueListItem.HeadBuilderProngHead)
			{
				((Vector3d)(ref val18))._002Ector(new Point3d(0.0, 0.0, ((Point3d)(ref val17)).Z) - val17);
				((Vector3d)(ref val18)).Unitize();
				val18 *= prongParam.ProngTopNudge;
				((Point3d)(ref val17)).Transform(Transform.Translation(val18));
				((Point3d)(ref val16)).Transform(Transform.Translation(val18));
				((Vector3d)(ref val18))._002Ector(new Point3d(0.0, 0.0, ((Point3d)(ref val17)).Z) - val17);
				((Vector3d)(ref val18)).Unitize();
				val18 *= prongParam.ProngBaseNudge * (1.0 - baseAnglePosition) + prongParam.ProngTopNudge * baseAnglePosition;
				((Point3d)(ref unset)).Transform(Transform.Translation(val18));
				((Vector3d)(ref val18)).Unitize();
				val18 *= prongParam.ProngBaseNudge;
				((Point3d)(ref item9)).Transform(Transform.Translation(val18));
			}
			List<Point3d> list2 = new List<Point3d> { val17, val16, unset, item9 };
			if (focalPoint != Point3d.Unset)
			{
				Point3d val19 = focalPoint;
				((Point3d)(ref val19)).Transform(xDown);
				double num9 = ((Point3d)(ref val19)).X / ((Point3d)(ref val19)).Z;
				double num10 = ((Point3d)(ref val19)).Y / ((Point3d)(ref val19)).Z;
				for (int j = 0; j < list2.Count; j++)
				{
					if (!prongParam.StraightProng)
					{
						Point3d value = list2[j];
						val20 = list2[j];
						double x = ((Point3d)(ref val20)).X;
						val20 = list2[j];
						((Point3d)(ref value)).X = x + ((Point3d)(ref val20)).Z * num9;
						val20 = list2[j];
						double y = ((Point3d)(ref val20)).Y;
						val20 = list2[j];
						((Point3d)(ref value)).Y = y + ((Point3d)(ref val20)).Z * num10;
						list2[j] = value;
					}
				}
			}
			Curve prongRail = ProngInterpCurve(list2);
			prongParam.ProngRail = prongRail;
		}
		double prongPointAngle2 = prongParams[0].ProngPointAngle;
		Point3d val21 = curve.PointAt(cSGemData.BezelParam);
		LineCurve val22 = new LineCurve(Point3d.Origin, val21 * 50.0);
		Transform val23 = CSBlockUtil.CSUtilMath.Rotation(CSBlockUtil.CSUtilMath.ToRadians(prongPointAngle2), Vector3d.ZAxis, Point3d.Origin);
		((GeometryBase)val22).Transform(val23);
		CurveIntersections val24 = Intersection.CurveCurve(curve2, (Curve)(object)val22, 0.001, 0.001);
		if (val24.Count <= 0)
		{
			error = "Bezel Point Prong Failed";
			return false;
		}
		double parameterA2 = val24[0].ParameterA;
		Point3d val25 = default(Point3d);
		((Point3d)(ref val25))._002Ector(curve2.PointAt(parameterA2));
		Point3d item10 = default(Point3d);
		((Point3d)(ref item10))._002Ector(val25);
		((Point3d)(ref item10)).Transform(val2);
		Point3d item11 = default(Point3d);
		((Point3d)(ref item11))._002Ector(val25);
		((Point3d)(ref item11)).Transform(val5);
		Point3d val26 = default(Point3d);
		((Point3d)(ref val26))._002Ector(val25);
		((Point3d)(ref val26)).Transform(val8);
		if (baseStyle == ValueListItem.HeadBuilderProngHead)
		{
			Vector3d val27 = default(Vector3d);
			((Vector3d)(ref val27))._002Ector(new Point3d(0.0, 0.0, ((Point3d)(ref val26)).Z) - val26);
			((Vector3d)(ref val27)).Unitize();
			val27 *= prongParams[0].ProngTopNudge;
			((Point3d)(ref val26)).Transform(Transform.Translation(val27));
			((Point3d)(ref val25)).Transform(Transform.Translation(val27));
			((Vector3d)(ref val27))._002Ector(new Point3d(0.0, 0.0, ((Point3d)(ref val26)).Z) - val26);
			((Vector3d)(ref val27)).Unitize();
			val27 *= prongParams[0].ProngBaseNudge * (1.0 - baseAnglePosition) + prongParams[0].ProngTopNudge * baseAnglePosition;
			((Point3d)(ref item11)).Transform(Transform.Translation(val27));
			((Vector3d)(ref val27)).Unitize();
			val27 *= prongParams[0].ProngBaseNudge;
			((Point3d)(ref item10)).Transform(Transform.Translation(val27));
		}
		List<Point3d> list3 = new List<Point3d> { val26, val25, item11, item10 };
		if (focalPoint != Point3d.Unset)
		{
			Point3d val28 = focalPoint;
			((Point3d)(ref val28)).Transform(xDown);
			double num11 = ((Point3d)(ref val28)).X / ((Point3d)(ref val28)).Z;
			double num12 = ((Point3d)(ref val28)).Y / ((Point3d)(ref val28)).Z;
			for (int k = 0; k < list3.Count; k++)
			{
				if (!prongParams[0].StraightProng)
				{
					Point3d value2 = list3[k];
					val20 = list3[k];
					double x2 = ((Point3d)(ref val20)).X;
					val20 = list3[k];
					((Point3d)(ref value2)).X = x2 + ((Point3d)(ref val20)).Z * num11;
					val20 = list3[k];
					double y2 = ((Point3d)(ref val20)).Y;
					val20 = list3[k];
					((Point3d)(ref value2)).Y = y2 + ((Point3d)(ref val20)).Z * num12;
					list3[k] = value2;
				}
			}
		}
		bezelPointProngRail = ProngInterpCurve(list3);
		headGirdle = curve2.DuplicateCurve();
		headTop = val9;
		headBase = val3;
		outHeadSideX = val10;
		outHeadSideY = val11;
		gemHeight = cSGemData.GemZMin;
		return true;
	}

	private static double GetDrop(double vBelowInput, CSDataArray vBaseObject, double vBaseOffset, CSGemData gem)
	{
		//IL_0039: Unknown result type (might be due to invalid IL or missing references)
		//IL_0047: Unknown result type (might be due to invalid IL or missing references)
		//IL_0054: Unknown result type (might be due to invalid IL or missing references)
		//IL_0062: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c0: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e0: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e5: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ea: Unknown result type (might be due to invalid IL or missing references)
		//IL_00eb: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f4: Unknown result type (might be due to invalid IL or missing references)
		//IL_00fc: Unknown result type (might be due to invalid IL or missing references)
		//IL_0104: Unknown result type (might be due to invalid IL or missing references)
		//IL_0110: Unknown result type (might be due to invalid IL or missing references)
		//IL_0132: Expected O, but got Unknown
		//IL_0146: Unknown result type (might be due to invalid IL or missing references)
		//IL_014b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0167: Unknown result type (might be due to invalid IL or missing references)
		//IL_0173: Unknown result type (might be due to invalid IL or missing references)
		//IL_0178: Unknown result type (might be due to invalid IL or missing references)
		//IL_017c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0194: Unknown result type (might be due to invalid IL or missing references)
		//IL_0196: Unknown result type (might be due to invalid IL or missing references)
		//IL_01b7: Unknown result type (might be due to invalid IL or missing references)
		//IL_01bc: Unknown result type (might be due to invalid IL or missing references)
		//IL_01c0: Unknown result type (might be due to invalid IL or missing references)
		//IL_01ee: Unknown result type (might be due to invalid IL or missing references)
		//IL_01f0: Unknown result type (might be due to invalid IL or missing references)
		//IL_01f2: Unknown result type (might be due to invalid IL or missing references)
		//IL_01f4: Unknown result type (might be due to invalid IL or missing references)
		//IL_01d8: Unknown result type (might be due to invalid IL or missing references)
		//IL_01da: Unknown result type (might be due to invalid IL or missing references)
		if (vBaseObject == null)
		{
			return vBelowInput + vBaseOffset;
		}
		CSData arrayItem = vBaseObject.GetArrayItem(0);
		Brep val = null;
		Curve val2 = (arrayItem as ICSCurve)?.GetCurve();
		if (val2 != null)
		{
			Curve val3 = val2.DuplicateCurve();
			Curve val4 = val2.DuplicateCurve();
			((GeometryBase)val3).Translate(Vector3d.YAxis * 50.0);
			((GeometryBase)val4).Translate(Vector3d.YAxis * -50.0);
			val = ((Surface)NurbsSurface.CreateRuledSurface(val3, val4)).ToBrep();
		}
		if (val == null && arrayItem is ICSBrep iCSBrep)
		{
			val = iCSBrep.GetBrep();
		}
		if (val == null)
		{
			return vBelowInput + vBaseOffset;
		}
		gem.GetGemXformRC(out var _, out var xUp);
		LineCurve val5 = new LineCurve(new Point3d(0.0, 0.0, 100.0), new Point3d(0.0, 0.0, -100.0));
		((GeometryBase)val5).Transform(xUp);
		Point3d val6 = default(Point3d);
		((Point3d)(ref val6)).Transform(xUp);
		Vector3d val7 = default(Vector3d);
		((Vector3d)(ref val7))._002Ector(Vector3d.ZAxis);
		((Vector3d)(ref val7)).Transform(xUp);
		((Vector3d)(ref val7)).Unitize();
		Curve[] array = default(Curve[]);
		Point3d[] array2 = default(Point3d[]);
		if (!Intersection.CurveBrep((Curve)val5, val, CSApplicationHelper.Application.Tolerance, ref array, ref array2))
		{
			return vBelowInput + vBaseOffset;
		}
		if (array2.Length == 0 && array.Length == 0)
		{
			return vBelowInput + vBaseOffset;
		}
		Point3d val8 = Point3d.Unset;
		double num = -1.23432101234321E+308;
		Curve[] array3 = array;
		double num2 = default(double);
		foreach (Curve obj in array3)
		{
			obj.ClosestPoint(val6, ref num2);
			Point3d val9 = obj.PointAt(num2);
			num2 = ((Point3d)(ref val9)).DistanceTo(val6);
			if (!RhinoMath.IsValidDouble(num) || num > num2)
			{
				val8 = val9;
				num = num2;
			}
		}
		Point3d[] array4 = array2;
		for (int i = 0; i < array4.Length; i++)
		{
			Point3d val10 = array4[i];
			double num3 = ((Point3d)(ref val10)).DistanceTo(val6);
			if (!RhinoMath.IsValidDouble(num) || num > num3)
			{
				val8 = val10;
				num = num3;
			}
		}
		if (val7 * (val6 - val8) < 0.0)
		{
			num *= -1.0;
		}
		return 0.0 - num + vBaseOffset;
	}

	private static Curve ProngInterpCurve(List<Point3d> listPoints)
	{
		//IL_0006: Unknown result type (might be due to invalid IL or missing references)
		//IL_000b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0012: Unknown result type (might be due to invalid IL or missing references)
		//IL_005b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0062: Unknown result type (might be due to invalid IL or missing references)
		//IL_0067: Unknown result type (might be due to invalid IL or missing references)
		//IL_006d: Expected O, but got Unknown
		//IL_0070: Unknown result type (might be due to invalid IL or missing references)
		//IL_0085: Unknown result type (might be due to invalid IL or missing references)
		//IL_0092: Unknown result type (might be due to invalid IL or missing references)
		//IL_00aa: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b8: Unknown result type (might be due to invalid IL or missing references)
		//IL_00bd: Unknown result type (might be due to invalid IL or missing references)
		for (int i = 0; i < listPoints.Count - 1; i++)
		{
			Point3d val = listPoints[i];
			if (((Point3d)(ref val)).DistanceTo(listPoints[i + 1]) < 0.01)
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
			LineCurve val2 = new LineCurve(listPoints[0], listPoints[1]);
			list.Add(listPoints[0]);
			list.Add(((Curve)val2).PointAt(0.1));
			list.Add(listPoints[1]);
		}
		return (Curve)(((object)Curve.CreateInterpolatedCurve((IEnumerable<Point3d>)list, 3, (CurveKnotStyle)1)) ?? ((object)new LineCurve(listPoints[0], listPoints[listPoints.Count - 1])));
	}
}
