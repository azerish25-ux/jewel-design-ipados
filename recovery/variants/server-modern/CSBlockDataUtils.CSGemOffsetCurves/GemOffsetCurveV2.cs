using System;
using System.Collections.Generic;
using CSBlockUtil;
using CSCompatibility;
using CSDatas;
using CSGemInstance.GemTableData;
using RMA.OpenNURBS;
using Rhino.Geometry;

namespace CSBlockDataUtils.CSGemOffsetCurves;

[Obsolete]
public static class GemOffsetCurveV2
{
	[Obsolete]
	public static bool GemOffset(CSGemData gem, double distance, out OnCurve curve, bool doTransformUp, out string error, double cornerBlend = 0.0)
	{
		Curve curve2;
		bool result = GemOffset(gem, distance, out curve2, doTransformUp, out error, cornerBlend);
		curve = curve2.Convert();
		return result;
	}

	public static bool GemOffset(CSGemData gem, double distance, out Curve curve, bool doTransformUp, out string error, double cornerBlend = 0.0)
	{
		return GemOffset(gem.GemShape, gem, distance, out curve, doTransformUp, out error, cornerBlend, 0.0, 0.0);
	}

	[Obsolete]
	public static bool GemOffset(string shape, CSGemData gem, double distance, out OnCurve curve, bool doTransformUp, out string error, double cornerBlend = 0.0, double optionalEmeraldCornerXPercent = 0.0, double optionalEmeraldCornerYPercent = 0.0)
	{
		Curve curve2;
		bool result = GemOffset(shape, gem, distance, out curve2, doTransformUp, out error, cornerBlend, optionalEmeraldCornerXPercent, optionalEmeraldCornerYPercent);
		curve = curve2.Convert();
		return result;
	}

	public static bool GemOffset(string shape, CSGemData gem, double distance, out Curve curve, bool doTransformUp, out string error, double cornerBlend = 0.0, double optionalEmeraldCornerXPercent = 0.0, double optionalEmeraldCornerYPercent = 0.0)
	{
		curve = null;
		error = string.Empty;
		Curve[] array = GemOffsetCurves(shape, gem, distance, doTransformUp, cornerBlend, optionalEmeraldCornerXPercent, optionalEmeraldCornerYPercent);
		if (array.Length != 1)
		{
			if (!CSUtilCurves.MergeCurves((IEnumerable<Curve>)array, out Curve mergedCrv, 0.01))
			{
				error = $"Gem Offset : {shape} Error Merging Curves";
				return false;
			}
			curve = mergedCrv;
		}
		else
		{
			curve = array[0];
		}
		if (curve != null)
		{
			return true;
		}
		error = $"Gem Offset : {shape} Error Curve";
		return false;
	}

	public static Curve[] GemOffsetCurves(string shape, CSGemData gem, double distance, bool doTransformUp, double cornerBlend = 0.0, double optionalEmeraldCornerXPercent = 0.0, double optionalEmeraldCornerYPercent = 0.0)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_056d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0599: Unknown result type (might be due to invalid IL or missing references)
		CSBlockUtil.CSUtilMath.GetUnscaledXforms(gem.Xform, out var xUp, out var xDown);
		Curve[] array;
		switch (shape)
		{
		case "Round":
		case "Oval":
		case "Oval Rotated 90":
			array = GemOffsetCurvesRound(gem, distance);
			break;
		case "Baguette Straight":
		case "Princess":
			array = GemOffsetCurvesSquare(gem, distance, cornerBlend);
			break;
		case "Baguette Tapered":
		case "BaguetteTapered":
			array = GemOffsetCurvesBaguetteTapered(gem, distance);
			break;
		case "Princess Rotated 45":
			array = GemOffsetCurvesSquareRotate45(gem, distance);
			break;
		case "Cushion Square":
			array = GemOffsetCurvesCushionSquare(gem, distance);
			break;
		case "Cushion Square Rotated 45":
			array = GemOffsetCurvesCushionSquareRotated45(gem, distance);
			break;
		case "Cushion":
			array = GemOffsetCurvesCushion(gem, distance);
			break;
		case "Cushion Rotated 90":
			array = GemOffsetCurvesCushionRotated90(gem, distance);
			break;
		case "Emerald":
		{
			double cornerPercentX = 0.6399272014559708;
			double cornerPercentY = 0.7429;
			if (optionalEmeraldCornerXPercent != 0.0 && optionalEmeraldCornerYPercent != 0.0)
			{
				cornerPercentX = optionalEmeraldCornerXPercent;
				cornerPercentY = optionalEmeraldCornerYPercent;
			}
			array = GemOffsetCurvesEmerald(gem, distance, cornerPercentX, cornerPercentY);
			break;
		}
		case "Emerald Rotated 90":
			array = GemOffsetCurvesEmerald(gem, distance, 0.7429, 0.6399272014559708);
			break;
		case "Emerald Square":
			array = GemOffsetCurvesEmerald(gem, distance, 0.64, 0.64);
			break;
		case "Emerald Square Rotated 45":
			array = GemOffsetCurvesEmerald(gem, distance, 0.21939999999999998, 0.21939999999999998);
			break;
		case "Heart":
			array = GemOffsetCurvesHeart(gem, distance);
			break;
		case "Marquise":
			array = GemOffsetCurvesMarquise(gem, distance);
			break;
		case "Marquise Rotated 90":
			array = GemOffsetCurvesMarquiseRotated90(gem, distance);
			break;
		case "Octagon":
			array = GemOffsetCurvesEmerald(gem, distance, 0.4142, 0.4142);
			break;
		case "Pear":
			array = GemOffsetCurvesPear(gem, distance);
			break;
		case "Radiant":
			array = GemOffsetCurvesEmerald(gem, distance, 0.7691855336665687, 0.8462);
			break;
		case "Radiant Square":
			array = GemOffsetCurvesEmerald(gem, distance, 0.5603, 0.5603);
			break;
		default:
		{
			CSUtil.PrintLine($"Gem Offset Shape Used Default Offset : {shape}");
			NurbsCurve nurbsCurve = gem.GetNurbsCurve();
			((GeometryBase)nurbsCurve).Transform(xDown);
			array = (Curve[])(object)new Curve[1] { CSUtilGeometry.OriginOffsetOldXY((Curve)(object)nurbsCurve, distance, closed: true) };
			break;
		}
		}
		if (!doTransformUp)
		{
			return array;
		}
		Curve[] array2 = array;
		for (int i = 0; i < array2.Length; i++)
		{
			((GeometryBase)array2[i]).Transform(xUp);
		}
		return array;
	}

	[Obsolete]
	public static OnCurve[] GemOffsetCurvesOld(string shape, CSGemData gem, double distance, bool doTransformUp, double cornerBlend = 0.0, double optionalEmeraldCornerXPercent = 0.0, double optionalEmeraldCornerYPercent = 0.0)
	{
		Curve[] array = GemOffsetCurves(shape, gem, distance, doTransformUp, cornerBlend, optionalEmeraldCornerXPercent, optionalEmeraldCornerYPercent);
		OnCurve[] array2 = (OnCurve[])(object)new OnCurve[array.Length];
		for (int i = 0; i < array2.Length; i++)
		{
			array2[i] = array[i].Convert();
		}
		return array2;
	}

	private static double AngleA(double a, double b, double c)
	{
		double num = (b * b + c * c - a * a) / (2.0 * b * c);
		if (Math.Abs(num) > 1.0)
		{
			return 0.0;
		}
		return Math.Acos(num);
	}

	private static void BlendCorner(ref Curve curve1, ref Curve curve2)
	{
		//IL_003b: Unknown result type (might be due to invalid IL or missing references)
		NurbsCurve val = CSUtilCurves.BlendCurve(curve1, curve2);
		val = ((Curve)val).Trim(new Interval(0.0, 0.5)).ToNurbsCurve();
		CSUtilCurves.MergeCurves((IEnumerable<Curve>)(object)new Curve[2]
		{
			curve1,
			(Curve)val
		}, out Curve[] mergedCrvs, 0.01);
		curve1 = mergedCrvs[0];
		NurbsCurve val2 = CSUtilCurves.BlendCurve(curve1, curve2);
		CSUtilCurves.MergeCurves((IEnumerable<Curve>)(object)new Curve[2]
		{
			curve2,
			(Curve)val2
		}, out Curve[] mergedCrvs2, 0.01);
		curve2 = mergedCrvs2[0];
	}

	private static Curve[] GemOffsetCurvesBaguetteTapered(CSGemData gem, double distance)
	{
		//IL_00ff: Unknown result type (might be due to invalid IL or missing references)
		//IL_0101: Unknown result type (might be due to invalid IL or missing references)
		//IL_0103: Unknown result type (might be due to invalid IL or missing references)
		//IL_010a: Expected O, but got Unknown
		//IL_010a: Unknown result type (might be due to invalid IL or missing references)
		//IL_010c: Unknown result type (might be due to invalid IL or missing references)
		//IL_010e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0115: Expected O, but got Unknown
		//IL_0115: Unknown result type (might be due to invalid IL or missing references)
		//IL_0117: Unknown result type (might be due to invalid IL or missing references)
		//IL_0119: Unknown result type (might be due to invalid IL or missing references)
		//IL_0120: Expected O, but got Unknown
		//IL_0120: Unknown result type (might be due to invalid IL or missing references)
		//IL_0122: Unknown result type (might be due to invalid IL or missing references)
		//IL_0124: Unknown result type (might be due to invalid IL or missing references)
		//IL_012b: Expected O, but got Unknown
		//IL_012b: Unknown result type (might be due to invalid IL or missing references)
		//IL_012d: Unknown result type (might be due to invalid IL or missing references)
		//IL_012f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0136: Expected O, but got Unknown
		GemSize gemTableSizeForShapeTypeIndex = GemTableHelper.GetGemTableSizeForShapeTypeIndex(GemTableType.Diamond, GemTableShape.BaguetteTapered, gem.CurrentSizeTableIndex);
		if (gemTableSizeForShapeTypeIndex == null)
		{
			return null;
		}
		double num = gemTableSizeForShapeTypeIndex.Width / 2.0;
		double num2 = ((GemSizeTapered)gemTableSizeForShapeTypeIndex).Width2 / 2.0;
		double num3 = gemTableSizeForShapeTypeIndex.Height / 2.0;
		num += distance;
		num2 += distance;
		num3 += distance;
		if (num < 0.04)
		{
			num = 0.04;
		}
		if (num2 < 0.04)
		{
			num2 = 0.04;
		}
		if (num3 < 0.04)
		{
			num3 = 0.04;
		}
		Point3d val = default(Point3d);
		((Point3d)(ref val))._002Ector(0.0, num3, 0.0);
		Point3d val2 = default(Point3d);
		((Point3d)(ref val2))._002Ector(num, num3, 0.0);
		Point3d val3 = default(Point3d);
		((Point3d)(ref val3))._002Ector(num2, 0.0 - num3, 0.0);
		Point3d val4 = default(Point3d);
		((Point3d)(ref val4))._002Ector(0.0 - num2, 0.0 - num3, 0.0);
		Point3d val5 = default(Point3d);
		((Point3d)(ref val5))._002Ector(0.0 - num, num3, 0.0);
		LineCurve val6 = new LineCurve(val, val2);
		LineCurve val7 = new LineCurve(val2, val3);
		LineCurve val8 = new LineCurve(val3, val4);
		LineCurve val9 = new LineCurve(val4, val5);
		LineCurve val10 = new LineCurve(val5, val);
		return (Curve[])(object)new Curve[5]
		{
			(Curve)val6,
			(Curve)val7,
			(Curve)val8,
			(Curve)val9,
			(Curve)val10
		};
	}

	private static Curve[] GemOffsetCurvesCushion(CSGemData iGem, double iDistance)
	{
		//IL_0158: Unknown result type (might be due to invalid IL or missing references)
		//IL_015a: Unknown result type (might be due to invalid IL or missing references)
		//IL_015c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0165: Unknown result type (might be due to invalid IL or missing references)
		//IL_0169: Unknown result type (might be due to invalid IL or missing references)
		//IL_016e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0182: Unknown result type (might be due to invalid IL or missing references)
		//IL_0186: Unknown result type (might be due to invalid IL or missing references)
		//IL_018b: Unknown result type (might be due to invalid IL or missing references)
		//IL_019f: Unknown result type (might be due to invalid IL or missing references)
		//IL_01a3: Unknown result type (might be due to invalid IL or missing references)
		//IL_01a8: Unknown result type (might be due to invalid IL or missing references)
		//IL_01bc: Unknown result type (might be due to invalid IL or missing references)
		//IL_01be: Unknown result type (might be due to invalid IL or missing references)
		//IL_01c1: Unknown result type (might be due to invalid IL or missing references)
		//IL_01c6: Unknown result type (might be due to invalid IL or missing references)
		//IL_01cb: Unknown result type (might be due to invalid IL or missing references)
		//IL_01cd: Unknown result type (might be due to invalid IL or missing references)
		//IL_01d0: Unknown result type (might be due to invalid IL or missing references)
		//IL_01d5: Unknown result type (might be due to invalid IL or missing references)
		//IL_01da: Unknown result type (might be due to invalid IL or missing references)
		//IL_01dc: Unknown result type (might be due to invalid IL or missing references)
		//IL_01df: Unknown result type (might be due to invalid IL or missing references)
		//IL_01e4: Unknown result type (might be due to invalid IL or missing references)
		//IL_0262: Unknown result type (might be due to invalid IL or missing references)
		//IL_0264: Unknown result type (might be due to invalid IL or missing references)
		//IL_0266: Unknown result type (might be due to invalid IL or missing references)
		//IL_026f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0273: Unknown result type (might be due to invalid IL or missing references)
		//IL_0278: Unknown result type (might be due to invalid IL or missing references)
		//IL_028c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0290: Unknown result type (might be due to invalid IL or missing references)
		//IL_0295: Unknown result type (might be due to invalid IL or missing references)
		//IL_02a9: Unknown result type (might be due to invalid IL or missing references)
		//IL_02ad: Unknown result type (might be due to invalid IL or missing references)
		//IL_02b2: Unknown result type (might be due to invalid IL or missing references)
		//IL_02c6: Unknown result type (might be due to invalid IL or missing references)
		//IL_02c8: Unknown result type (might be due to invalid IL or missing references)
		//IL_02cb: Unknown result type (might be due to invalid IL or missing references)
		//IL_02d0: Unknown result type (might be due to invalid IL or missing references)
		//IL_02d5: Unknown result type (might be due to invalid IL or missing references)
		//IL_02d7: Unknown result type (might be due to invalid IL or missing references)
		//IL_02da: Unknown result type (might be due to invalid IL or missing references)
		//IL_02df: Unknown result type (might be due to invalid IL or missing references)
		//IL_02e4: Unknown result type (might be due to invalid IL or missing references)
		//IL_02e6: Unknown result type (might be due to invalid IL or missing references)
		//IL_02e9: Unknown result type (might be due to invalid IL or missing references)
		//IL_02ee: Unknown result type (might be due to invalid IL or missing references)
		//IL_02fa: Unknown result type (might be due to invalid IL or missing references)
		//IL_02ff: Unknown result type (might be due to invalid IL or missing references)
		//IL_030a: Unknown result type (might be due to invalid IL or missing references)
		//IL_030f: Unknown result type (might be due to invalid IL or missing references)
		//IL_033d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0356: Unknown result type (might be due to invalid IL or missing references)
		//IL_031c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0321: Unknown result type (might be due to invalid IL or missing references)
		//IL_032c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0331: Unknown result type (might be due to invalid IL or missing references)
		//IL_0370: Unknown result type (might be due to invalid IL or missing references)
		//IL_0390: Unknown result type (might be due to invalid IL or missing references)
		//IL_0395: Unknown result type (might be due to invalid IL or missing references)
		//IL_0397: Unknown result type (might be due to invalid IL or missing references)
		//IL_0399: Unknown result type (might be due to invalid IL or missing references)
		//IL_039d: Unknown result type (might be due to invalid IL or missing references)
		//IL_03a5: Unknown result type (might be due to invalid IL or missing references)
		//IL_03a7: Unknown result type (might be due to invalid IL or missing references)
		//IL_03ab: Unknown result type (might be due to invalid IL or missing references)
		//IL_03b5: Unknown result type (might be due to invalid IL or missing references)
		//IL_03bc: Unknown result type (might be due to invalid IL or missing references)
		//IL_03c5: Unknown result type (might be due to invalid IL or missing references)
		//IL_03ca: Unknown result type (might be due to invalid IL or missing references)
		//IL_03d3: Unknown result type (might be due to invalid IL or missing references)
		//IL_03dc: Unknown result type (might be due to invalid IL or missing references)
		//IL_03e1: Unknown result type (might be due to invalid IL or missing references)
		//IL_03ea: Unknown result type (might be due to invalid IL or missing references)
		//IL_03ef: Unknown result type (might be due to invalid IL or missing references)
		//IL_0419: Unknown result type (might be due to invalid IL or missing references)
		//IL_0420: Unknown result type (might be due to invalid IL or missing references)
		//IL_0429: Unknown result type (might be due to invalid IL or missing references)
		//IL_042e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0437: Unknown result type (might be due to invalid IL or missing references)
		//IL_0440: Unknown result type (might be due to invalid IL or missing references)
		//IL_0445: Unknown result type (might be due to invalid IL or missing references)
		//IL_044e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0453: Unknown result type (might be due to invalid IL or missing references)
		//IL_047d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0484: Unknown result type (might be due to invalid IL or missing references)
		//IL_048d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0492: Unknown result type (might be due to invalid IL or missing references)
		//IL_049b: Unknown result type (might be due to invalid IL or missing references)
		//IL_04a4: Unknown result type (might be due to invalid IL or missing references)
		//IL_04a9: Unknown result type (might be due to invalid IL or missing references)
		//IL_04b2: Unknown result type (might be due to invalid IL or missing references)
		//IL_04b7: Unknown result type (might be due to invalid IL or missing references)
		//IL_04e1: Unknown result type (might be due to invalid IL or missing references)
		//IL_04e8: Unknown result type (might be due to invalid IL or missing references)
		//IL_04f1: Unknown result type (might be due to invalid IL or missing references)
		//IL_04f6: Unknown result type (might be due to invalid IL or missing references)
		//IL_04ff: Unknown result type (might be due to invalid IL or missing references)
		//IL_0508: Unknown result type (might be due to invalid IL or missing references)
		//IL_050d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0516: Unknown result type (might be due to invalid IL or missing references)
		//IL_051b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0543: Unknown result type (might be due to invalid IL or missing references)
		//IL_0545: Unknown result type (might be due to invalid IL or missing references)
		//IL_054c: Expected O, but got Unknown
		//IL_054c: Unknown result type (might be due to invalid IL or missing references)
		//IL_054e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0555: Expected O, but got Unknown
		//IL_0559: Unknown result type (might be due to invalid IL or missing references)
		//IL_055e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0569: Unknown result type (might be due to invalid IL or missing references)
		//IL_056e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0577: Unknown result type (might be due to invalid IL or missing references)
		//IL_0587: Unknown result type (might be due to invalid IL or missing references)
		//IL_058c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0597: Unknown result type (might be due to invalid IL or missing references)
		//IL_059c: Unknown result type (might be due to invalid IL or missing references)
		//IL_05a5: Unknown result type (might be due to invalid IL or missing references)
		//IL_05c4: Unknown result type (might be due to invalid IL or missing references)
		//IL_05c6: Unknown result type (might be due to invalid IL or missing references)
		//IL_05cc: Expected O, but got Unknown
		//IL_05d3: Unknown result type (might be due to invalid IL or missing references)
		//IL_05d5: Unknown result type (might be due to invalid IL or missing references)
		//IL_05db: Expected O, but got Unknown
		//IL_05e2: Unknown result type (might be due to invalid IL or missing references)
		//IL_05e4: Unknown result type (might be due to invalid IL or missing references)
		//IL_05ea: Expected O, but got Unknown
		double num = iDistance;
		CSGemData cSGemData = new CSGemData(iGem);
		if (iDistance < 0.0)
		{
			if (iGem.GemXSize + iDistance * 2.0 < 0.0)
			{
				num = (0.0 - iGem.GemXSize) / 2.0 + 0.05;
			}
			if (iGem.GemYSize + iDistance * 2.0 < 0.0)
			{
				num = (0.0 - iGem.GemYSize) / 2.0 + 0.05;
			}
			cSGemData.ScaleToSize(iGem.GemXSize + num * 2.0, iGem.GemYSize + num * 2.0);
			num = 0.0;
		}
		double gemXSize = cSGemData.GemXSize;
		double gemYSize = cSGemData.GemYSize;
		double num2 = gemXSize / 8.0;
		double num3 = gemYSize / 10.0;
		Point3d val = default(Point3d);
		((Point3d)(ref val))._002Ector(0.0, 5.0 * num3, 0.0);
		Point3d val2 = default(Point3d);
		((Point3d)(ref val2))._002Ector(-2.18319 * num2, 4.54508 * num3, 0.0);
		Point3d val3 = default(Point3d);
		((Point3d)(ref val3))._002Ector(2.18319 * num2, 4.54508 * num3, 0.0);
		Arc val4 = default(Arc);
		((Arc)(ref val4))._002Ector(val2, val, val3);
		Vector3d val5 = default(Vector3d);
		((Vector3d)(ref val5))._002Ector(val2 - ((Arc)(ref val4)).Center);
		((Vector3d)(ref val5)).Unitize();
		Vector3d val6 = default(Vector3d);
		((Vector3d)(ref val6))._002Ector(val - ((Arc)(ref val4)).Center);
		((Vector3d)(ref val6)).Unitize();
		Vector3d val7 = default(Vector3d);
		((Vector3d)(ref val7))._002Ector(val3 - ((Arc)(ref val4)).Center);
		((Vector3d)(ref val7)).Unitize();
		Arc val8 = default(Arc);
		((Arc)(ref val8))._002Ector(val2 + val5 * num, val + val6 * num, val3 + val7 * num);
		Point3d val9 = default(Point3d);
		((Point3d)(ref val9))._002Ector(4.0 * num2, 0.0, 0.0);
		Point3d val10 = default(Point3d);
		((Point3d)(ref val10))._002Ector(3.63684 * num2, 2.72661 * num3, 0.0);
		Point3d val11 = default(Point3d);
		((Point3d)(ref val11))._002Ector(3.63684 * num2, -2.72661 * num3, 0.0);
		Arc val12 = default(Arc);
		((Arc)(ref val12))._002Ector(val10, val9, val11);
		Vector3d val13 = default(Vector3d);
		((Vector3d)(ref val13))._002Ector(val10 - ((Arc)(ref val12)).Center);
		((Vector3d)(ref val13)).Unitize();
		Vector3d val14 = default(Vector3d);
		((Vector3d)(ref val14))._002Ector(val9 - ((Arc)(ref val12)).Center);
		((Vector3d)(ref val14)).Unitize();
		Vector3d val15 = default(Vector3d);
		((Vector3d)(ref val15))._002Ector(val11 - ((Arc)(ref val12)).Center);
		((Vector3d)(ref val15)).Unitize();
		Arc val16 = default(Arc);
		((Arc)(ref val16))._002Ector(val10 + val13 * num, val9 + val14 * num, val11 + val15 * num);
		Point3d val17 = ((Arc)(ref val8)).EndPoint;
		double x = ((Point3d)(ref val17)).X;
		val17 = ((Arc)(ref val16)).StartPoint;
		if (!(x > ((Point3d)(ref val17)).X))
		{
			val17 = ((Arc)(ref val16)).StartPoint;
			double y = ((Point3d)(ref val17)).Y;
			val17 = ((Arc)(ref val8)).EndPoint;
			if (!(y > ((Point3d)(ref val17)).Y))
			{
				Transform val18 = Transform.Scale(Plane.WorldXY, -1.0, -1.0, 0.0);
				Arc val19 = val8;
				((Arc)(ref val19)).Transform(val18);
				Arc val20 = val16;
				((Arc)(ref val20)).Transform(val18);
				Point3d endPoint = ((Arc)(ref val8)).EndPoint;
				Point3d startPoint = ((Arc)(ref val16)).StartPoint;
				Interval val21 = ((Arc)(ref val8)).AngleDomain;
				Vector3d vectorStart = ((Arc)(ref val8)).TangentAt(((Interval)(ref val21)).Max);
				val21 = ((Arc)(ref val16)).AngleDomain;
				NurbsCurve val22 = CSUtilCurves.BlendCurve(endPoint, startPoint, vectorStart, -((Arc)(ref val16)).TangentAt(((Interval)(ref val21)).Min), 0.9);
				Point3d endPoint2 = ((Arc)(ref val16)).EndPoint;
				Point3d startPoint2 = ((Arc)(ref val19)).StartPoint;
				val21 = ((Arc)(ref val16)).AngleDomain;
				Vector3d vectorStart2 = ((Arc)(ref val16)).TangentAt(((Interval)(ref val21)).Max);
				val21 = ((Arc)(ref val19)).AngleDomain;
				NurbsCurve val23 = CSUtilCurves.BlendCurve(endPoint2, startPoint2, vectorStart2, -((Arc)(ref val19)).TangentAt(((Interval)(ref val21)).Min), 0.9);
				Point3d endPoint3 = ((Arc)(ref val19)).EndPoint;
				Point3d startPoint3 = ((Arc)(ref val20)).StartPoint;
				val21 = ((Arc)(ref val19)).AngleDomain;
				Vector3d vectorStart3 = ((Arc)(ref val19)).TangentAt(((Interval)(ref val21)).Max);
				val21 = ((Arc)(ref val20)).AngleDomain;
				NurbsCurve val24 = CSUtilCurves.BlendCurve(endPoint3, startPoint3, vectorStart3, -((Arc)(ref val20)).TangentAt(((Interval)(ref val21)).Min), 0.9);
				Point3d endPoint4 = ((Arc)(ref val20)).EndPoint;
				Point3d startPoint4 = ((Arc)(ref val8)).StartPoint;
				val21 = ((Arc)(ref val20)).AngleDomain;
				Vector3d vectorStart4 = ((Arc)(ref val20)).TangentAt(((Interval)(ref val21)).Max);
				val21 = ((Arc)(ref val8)).AngleDomain;
				NurbsCurve val25 = CSUtilCurves.BlendCurve(endPoint4, startPoint4, vectorStart4, -((Arc)(ref val8)).TangentAt(((Interval)(ref val21)).Min), 0.9);
				ArcCurve val26 = new ArcCurve(val8);
				ArcCurve val27 = new ArcCurve(val8);
				val21 = ((Curve)val26).Domain;
				double mid = ((Interval)(ref val21)).Mid;
				val21 = ((Curve)val26).Domain;
				Curve val28 = ((Curve)val26).Trim(new Interval(mid, ((Interval)(ref val21)).Max));
				val21 = ((Curve)val27).Domain;
				double min = ((Interval)(ref val21)).Min;
				val21 = ((Curve)val27).Domain;
				Curve val29 = ((Curve)val27).Trim(new Interval(min, ((Interval)(ref val21)).Mid));
				return (Curve[])(object)new Curve[9]
				{
					val28,
					(Curve)val22,
					(Curve)new ArcCurve(val16),
					(Curve)val23,
					(Curve)new ArcCurve(val19),
					(Curve)val24,
					(Curve)new ArcCurve(val20),
					(Curve)val25,
					val29
				};
			}
		}
		CSBlockUtil.CSUtilMath.GetUnscaledXforms(iGem.Xform, out var _, out var xDown);
		NurbsCurve nurbsCurve = iGem.GetNurbsCurve();
		((GeometryBase)nurbsCurve).Transform(xDown);
		return (Curve[])(object)new Curve[1] { CSUtilGeometry.OriginOffsetXY((Curve)(object)nurbsCurve, iDistance) };
	}

	private static Curve[] GemOffsetCurvesCushionRotated90(CSGemData iGem, double iDistance)
	{
		//IL_0158: Unknown result type (might be due to invalid IL or missing references)
		//IL_015a: Unknown result type (might be due to invalid IL or missing references)
		//IL_015c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0165: Unknown result type (might be due to invalid IL or missing references)
		//IL_0169: Unknown result type (might be due to invalid IL or missing references)
		//IL_016e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0182: Unknown result type (might be due to invalid IL or missing references)
		//IL_0186: Unknown result type (might be due to invalid IL or missing references)
		//IL_018b: Unknown result type (might be due to invalid IL or missing references)
		//IL_019f: Unknown result type (might be due to invalid IL or missing references)
		//IL_01a3: Unknown result type (might be due to invalid IL or missing references)
		//IL_01a8: Unknown result type (might be due to invalid IL or missing references)
		//IL_01bc: Unknown result type (might be due to invalid IL or missing references)
		//IL_01be: Unknown result type (might be due to invalid IL or missing references)
		//IL_01c1: Unknown result type (might be due to invalid IL or missing references)
		//IL_01c6: Unknown result type (might be due to invalid IL or missing references)
		//IL_01cb: Unknown result type (might be due to invalid IL or missing references)
		//IL_01cd: Unknown result type (might be due to invalid IL or missing references)
		//IL_01d0: Unknown result type (might be due to invalid IL or missing references)
		//IL_01d5: Unknown result type (might be due to invalid IL or missing references)
		//IL_01da: Unknown result type (might be due to invalid IL or missing references)
		//IL_01dc: Unknown result type (might be due to invalid IL or missing references)
		//IL_01df: Unknown result type (might be due to invalid IL or missing references)
		//IL_01e4: Unknown result type (might be due to invalid IL or missing references)
		//IL_0262: Unknown result type (might be due to invalid IL or missing references)
		//IL_0264: Unknown result type (might be due to invalid IL or missing references)
		//IL_0266: Unknown result type (might be due to invalid IL or missing references)
		//IL_026f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0273: Unknown result type (might be due to invalid IL or missing references)
		//IL_0278: Unknown result type (might be due to invalid IL or missing references)
		//IL_028c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0290: Unknown result type (might be due to invalid IL or missing references)
		//IL_0295: Unknown result type (might be due to invalid IL or missing references)
		//IL_02a9: Unknown result type (might be due to invalid IL or missing references)
		//IL_02ad: Unknown result type (might be due to invalid IL or missing references)
		//IL_02b2: Unknown result type (might be due to invalid IL or missing references)
		//IL_02c6: Unknown result type (might be due to invalid IL or missing references)
		//IL_02c8: Unknown result type (might be due to invalid IL or missing references)
		//IL_02cb: Unknown result type (might be due to invalid IL or missing references)
		//IL_02d0: Unknown result type (might be due to invalid IL or missing references)
		//IL_02d5: Unknown result type (might be due to invalid IL or missing references)
		//IL_02d7: Unknown result type (might be due to invalid IL or missing references)
		//IL_02da: Unknown result type (might be due to invalid IL or missing references)
		//IL_02df: Unknown result type (might be due to invalid IL or missing references)
		//IL_02e4: Unknown result type (might be due to invalid IL or missing references)
		//IL_02e6: Unknown result type (might be due to invalid IL or missing references)
		//IL_02e9: Unknown result type (might be due to invalid IL or missing references)
		//IL_02ee: Unknown result type (might be due to invalid IL or missing references)
		//IL_02fa: Unknown result type (might be due to invalid IL or missing references)
		//IL_02ff: Unknown result type (might be due to invalid IL or missing references)
		//IL_030a: Unknown result type (might be due to invalid IL or missing references)
		//IL_030f: Unknown result type (might be due to invalid IL or missing references)
		//IL_033d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0356: Unknown result type (might be due to invalid IL or missing references)
		//IL_031c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0321: Unknown result type (might be due to invalid IL or missing references)
		//IL_032c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0331: Unknown result type (might be due to invalid IL or missing references)
		//IL_0370: Unknown result type (might be due to invalid IL or missing references)
		//IL_0390: Unknown result type (might be due to invalid IL or missing references)
		//IL_0395: Unknown result type (might be due to invalid IL or missing references)
		//IL_0397: Unknown result type (might be due to invalid IL or missing references)
		//IL_0399: Unknown result type (might be due to invalid IL or missing references)
		//IL_039d: Unknown result type (might be due to invalid IL or missing references)
		//IL_03a5: Unknown result type (might be due to invalid IL or missing references)
		//IL_03a7: Unknown result type (might be due to invalid IL or missing references)
		//IL_03ab: Unknown result type (might be due to invalid IL or missing references)
		//IL_03b5: Unknown result type (might be due to invalid IL or missing references)
		//IL_03bc: Unknown result type (might be due to invalid IL or missing references)
		//IL_03c5: Unknown result type (might be due to invalid IL or missing references)
		//IL_03ca: Unknown result type (might be due to invalid IL or missing references)
		//IL_03d3: Unknown result type (might be due to invalid IL or missing references)
		//IL_03dc: Unknown result type (might be due to invalid IL or missing references)
		//IL_03e1: Unknown result type (might be due to invalid IL or missing references)
		//IL_03ea: Unknown result type (might be due to invalid IL or missing references)
		//IL_03ef: Unknown result type (might be due to invalid IL or missing references)
		//IL_0419: Unknown result type (might be due to invalid IL or missing references)
		//IL_0420: Unknown result type (might be due to invalid IL or missing references)
		//IL_0429: Unknown result type (might be due to invalid IL or missing references)
		//IL_042e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0437: Unknown result type (might be due to invalid IL or missing references)
		//IL_0440: Unknown result type (might be due to invalid IL or missing references)
		//IL_0445: Unknown result type (might be due to invalid IL or missing references)
		//IL_044e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0453: Unknown result type (might be due to invalid IL or missing references)
		//IL_047d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0484: Unknown result type (might be due to invalid IL or missing references)
		//IL_048d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0492: Unknown result type (might be due to invalid IL or missing references)
		//IL_049b: Unknown result type (might be due to invalid IL or missing references)
		//IL_04a4: Unknown result type (might be due to invalid IL or missing references)
		//IL_04a9: Unknown result type (might be due to invalid IL or missing references)
		//IL_04b2: Unknown result type (might be due to invalid IL or missing references)
		//IL_04b7: Unknown result type (might be due to invalid IL or missing references)
		//IL_04e1: Unknown result type (might be due to invalid IL or missing references)
		//IL_04e8: Unknown result type (might be due to invalid IL or missing references)
		//IL_04f1: Unknown result type (might be due to invalid IL or missing references)
		//IL_04f6: Unknown result type (might be due to invalid IL or missing references)
		//IL_04ff: Unknown result type (might be due to invalid IL or missing references)
		//IL_0508: Unknown result type (might be due to invalid IL or missing references)
		//IL_050d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0516: Unknown result type (might be due to invalid IL or missing references)
		//IL_051b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0543: Unknown result type (might be due to invalid IL or missing references)
		//IL_0545: Unknown result type (might be due to invalid IL or missing references)
		//IL_054c: Expected O, but got Unknown
		//IL_054c: Unknown result type (might be due to invalid IL or missing references)
		//IL_054e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0555: Expected O, but got Unknown
		//IL_0559: Unknown result type (might be due to invalid IL or missing references)
		//IL_055e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0569: Unknown result type (might be due to invalid IL or missing references)
		//IL_056e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0577: Unknown result type (might be due to invalid IL or missing references)
		//IL_0587: Unknown result type (might be due to invalid IL or missing references)
		//IL_058c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0597: Unknown result type (might be due to invalid IL or missing references)
		//IL_059c: Unknown result type (might be due to invalid IL or missing references)
		//IL_05a5: Unknown result type (might be due to invalid IL or missing references)
		//IL_05c4: Unknown result type (might be due to invalid IL or missing references)
		//IL_05c6: Unknown result type (might be due to invalid IL or missing references)
		//IL_05cc: Expected O, but got Unknown
		//IL_05d3: Unknown result type (might be due to invalid IL or missing references)
		//IL_05d5: Unknown result type (might be due to invalid IL or missing references)
		//IL_05db: Expected O, but got Unknown
		//IL_05e2: Unknown result type (might be due to invalid IL or missing references)
		//IL_05e4: Unknown result type (might be due to invalid IL or missing references)
		//IL_05ea: Expected O, but got Unknown
		double num = iDistance;
		CSGemData cSGemData = new CSGemData(iGem);
		if (iDistance < 0.0)
		{
			if (iGem.GemXSize + iDistance * 2.0 < 0.0)
			{
				num = (0.0 - iGem.GemXSize) / 2.0 + 0.05;
			}
			if (iGem.GemYSize + iDistance * 2.0 < 0.0)
			{
				num = (0.0 - iGem.GemYSize) / 2.0 + 0.05;
			}
			cSGemData.ScaleToSize(iGem.GemXSize + num * 2.0, iGem.GemYSize + num * 2.0);
			num = 0.0;
		}
		double gemXSize = cSGemData.GemXSize;
		double gemYSize = cSGemData.GemYSize;
		double num2 = gemXSize / 10.0;
		double num3 = gemYSize / 8.0;
		Point3d val = default(Point3d);
		((Point3d)(ref val))._002Ector(0.0, 4.0 * num3, 0.0);
		Point3d val2 = default(Point3d);
		((Point3d)(ref val2))._002Ector(-2.72661 * num2, 3.63684 * num3, 0.0);
		Point3d val3 = default(Point3d);
		((Point3d)(ref val3))._002Ector(2.72661 * num2, 3.63684 * num3, 0.0);
		Arc val4 = default(Arc);
		((Arc)(ref val4))._002Ector(val2, val, val3);
		Vector3d val5 = default(Vector3d);
		((Vector3d)(ref val5))._002Ector(val2 - ((Arc)(ref val4)).Center);
		((Vector3d)(ref val5)).Unitize();
		Vector3d val6 = default(Vector3d);
		((Vector3d)(ref val6))._002Ector(val - ((Arc)(ref val4)).Center);
		((Vector3d)(ref val6)).Unitize();
		Vector3d val7 = default(Vector3d);
		((Vector3d)(ref val7))._002Ector(val3 - ((Arc)(ref val4)).Center);
		((Vector3d)(ref val7)).Unitize();
		Arc val8 = default(Arc);
		((Arc)(ref val8))._002Ector(val2 + val5 * num, val + val6 * num, val3 + val7 * num);
		Point3d val9 = default(Point3d);
		((Point3d)(ref val9))._002Ector(5.0 * num2, 0.0, 0.0);
		Point3d val10 = default(Point3d);
		((Point3d)(ref val10))._002Ector(4.54508 * num2, 2.18319 * num3, 0.0);
		Point3d val11 = default(Point3d);
		((Point3d)(ref val11))._002Ector(4.54508 * num2, -2.18319 * num3, 0.0);
		Arc val12 = default(Arc);
		((Arc)(ref val12))._002Ector(val10, val9, val11);
		Vector3d val13 = default(Vector3d);
		((Vector3d)(ref val13))._002Ector(val10 - ((Arc)(ref val12)).Center);
		((Vector3d)(ref val13)).Unitize();
		Vector3d val14 = default(Vector3d);
		((Vector3d)(ref val14))._002Ector(val9 - ((Arc)(ref val12)).Center);
		((Vector3d)(ref val14)).Unitize();
		Vector3d val15 = default(Vector3d);
		((Vector3d)(ref val15))._002Ector(val11 - ((Arc)(ref val12)).Center);
		((Vector3d)(ref val15)).Unitize();
		Arc val16 = default(Arc);
		((Arc)(ref val16))._002Ector(val10 + val13 * num, val9 + val14 * num, val11 + val15 * num);
		Point3d val17 = ((Arc)(ref val8)).EndPoint;
		double x = ((Point3d)(ref val17)).X;
		val17 = ((Arc)(ref val16)).StartPoint;
		if (!(x > ((Point3d)(ref val17)).X))
		{
			val17 = ((Arc)(ref val16)).StartPoint;
			double y = ((Point3d)(ref val17)).Y;
			val17 = ((Arc)(ref val8)).EndPoint;
			if (!(y > ((Point3d)(ref val17)).Y))
			{
				Transform val18 = Transform.Scale(Plane.WorldXY, -1.0, -1.0, 0.0);
				Arc val19 = val8;
				((Arc)(ref val19)).Transform(val18);
				Arc val20 = val16;
				((Arc)(ref val20)).Transform(val18);
				Point3d endPoint = ((Arc)(ref val8)).EndPoint;
				Point3d startPoint = ((Arc)(ref val16)).StartPoint;
				Interval val21 = ((Arc)(ref val8)).AngleDomain;
				Vector3d vectorStart = ((Arc)(ref val8)).TangentAt(((Interval)(ref val21)).Max);
				val21 = ((Arc)(ref val16)).AngleDomain;
				NurbsCurve val22 = CSUtilCurves.BlendCurve(endPoint, startPoint, vectorStart, -((Arc)(ref val16)).TangentAt(((Interval)(ref val21)).Min), 0.9);
				Point3d endPoint2 = ((Arc)(ref val16)).EndPoint;
				Point3d startPoint2 = ((Arc)(ref val19)).StartPoint;
				val21 = ((Arc)(ref val16)).AngleDomain;
				Vector3d vectorStart2 = ((Arc)(ref val16)).TangentAt(((Interval)(ref val21)).Max);
				val21 = ((Arc)(ref val19)).AngleDomain;
				NurbsCurve val23 = CSUtilCurves.BlendCurve(endPoint2, startPoint2, vectorStart2, -((Arc)(ref val19)).TangentAt(((Interval)(ref val21)).Min), 0.9);
				Point3d endPoint3 = ((Arc)(ref val19)).EndPoint;
				Point3d startPoint3 = ((Arc)(ref val20)).StartPoint;
				val21 = ((Arc)(ref val19)).AngleDomain;
				Vector3d vectorStart3 = ((Arc)(ref val19)).TangentAt(((Interval)(ref val21)).Max);
				val21 = ((Arc)(ref val20)).AngleDomain;
				NurbsCurve val24 = CSUtilCurves.BlendCurve(endPoint3, startPoint3, vectorStart3, -((Arc)(ref val20)).TangentAt(((Interval)(ref val21)).Min), 0.9);
				Point3d endPoint4 = ((Arc)(ref val20)).EndPoint;
				Point3d startPoint4 = ((Arc)(ref val8)).StartPoint;
				val21 = ((Arc)(ref val20)).AngleDomain;
				Vector3d vectorStart4 = ((Arc)(ref val20)).TangentAt(((Interval)(ref val21)).Max);
				val21 = ((Arc)(ref val8)).AngleDomain;
				NurbsCurve val25 = CSUtilCurves.BlendCurve(endPoint4, startPoint4, vectorStart4, -((Arc)(ref val8)).TangentAt(((Interval)(ref val21)).Min), 0.9);
				ArcCurve val26 = new ArcCurve(val8);
				ArcCurve val27 = new ArcCurve(val8);
				val21 = ((Curve)val26).Domain;
				double mid = ((Interval)(ref val21)).Mid;
				val21 = ((Curve)val26).Domain;
				Curve val28 = ((Curve)val26).Trim(new Interval(mid, ((Interval)(ref val21)).Max));
				val21 = ((Curve)val27).Domain;
				double min = ((Interval)(ref val21)).Min;
				val21 = ((Curve)val27).Domain;
				Curve val29 = ((Curve)val27).Trim(new Interval(min, ((Interval)(ref val21)).Mid));
				return (Curve[])(object)new Curve[9]
				{
					val28,
					(Curve)val22,
					(Curve)new ArcCurve(val16),
					(Curve)val23,
					(Curve)new ArcCurve(val19),
					(Curve)val24,
					(Curve)new ArcCurve(val20),
					(Curve)val25,
					val29
				};
			}
		}
		CSBlockUtil.CSUtilMath.GetUnscaledXforms(iGem.Xform, out var _, out var xDown);
		NurbsCurve nurbsCurve = iGem.GetNurbsCurve();
		((GeometryBase)nurbsCurve).Transform(xDown);
		return (Curve[])(object)new Curve[1] { CSUtilGeometry.OriginOffsetXY((Curve)(object)nurbsCurve, iDistance) };
	}

	private static Curve[] GemOffsetCurvesCushionSquare(CSGemData gem, double distance, double cornerBlend = 0.0)
	{
		//IL_00ac: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ae: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b0: Unknown result type (might be due to invalid IL or missing references)
		//IL_00bb: Unknown result type (might be due to invalid IL or missing references)
		//IL_0140: Unknown result type (might be due to invalid IL or missing references)
		//IL_0142: Unknown result type (might be due to invalid IL or missing references)
		//IL_0144: Unknown result type (might be due to invalid IL or missing references)
		//IL_014f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0168: Unknown result type (might be due to invalid IL or missing references)
		//IL_016a: Unknown result type (might be due to invalid IL or missing references)
		//IL_016c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0171: Unknown result type (might be due to invalid IL or missing references)
		//IL_0187: Unknown result type (might be due to invalid IL or missing references)
		//IL_0189: Unknown result type (might be due to invalid IL or missing references)
		//IL_018b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0190: Unknown result type (might be due to invalid IL or missing references)
		//IL_019e: Unknown result type (might be due to invalid IL or missing references)
		//IL_01a3: Unknown result type (might be due to invalid IL or missing references)
		//IL_01ae: Unknown result type (might be due to invalid IL or missing references)
		//IL_01b2: Unknown result type (might be due to invalid IL or missing references)
		//IL_01b7: Unknown result type (might be due to invalid IL or missing references)
		//IL_01b9: Unknown result type (might be due to invalid IL or missing references)
		//IL_01bb: Unknown result type (might be due to invalid IL or missing references)
		//IL_01bd: Unknown result type (might be due to invalid IL or missing references)
		//IL_01c2: Unknown result type (might be due to invalid IL or missing references)
		//IL_01ff: Unknown result type (might be due to invalid IL or missing references)
		//IL_0201: Unknown result type (might be due to invalid IL or missing references)
		//IL_0205: Unknown result type (might be due to invalid IL or missing references)
		//IL_0207: Unknown result type (might be due to invalid IL or missing references)
		//IL_0209: Unknown result type (might be due to invalid IL or missing references)
		//IL_0212: Unknown result type (might be due to invalid IL or missing references)
		//IL_0214: Unknown result type (might be due to invalid IL or missing references)
		//IL_0216: Unknown result type (might be due to invalid IL or missing references)
		//IL_0221: Unknown result type (might be due to invalid IL or missing references)
		//IL_0226: Unknown result type (might be due to invalid IL or missing references)
		//IL_0231: Unknown result type (might be due to invalid IL or missing references)
		//IL_0236: Unknown result type (might be due to invalid IL or missing references)
		//IL_023f: Unknown result type (might be due to invalid IL or missing references)
		//IL_024e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0253: Unknown result type (might be due to invalid IL or missing references)
		//IL_025e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0263: Unknown result type (might be due to invalid IL or missing references)
		//IL_026c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0277: Unknown result type (might be due to invalid IL or missing references)
		//IL_0279: Unknown result type (might be due to invalid IL or missing references)
		//IL_02b8: Unknown result type (might be due to invalid IL or missing references)
		//IL_02ba: Unknown result type (might be due to invalid IL or missing references)
		//IL_02bc: Unknown result type (might be due to invalid IL or missing references)
		//IL_0321: Unknown result type (might be due to invalid IL or missing references)
		//IL_0323: Unknown result type (might be due to invalid IL or missing references)
		//IL_0325: Unknown result type (might be due to invalid IL or missing references)
		//IL_0388: Unknown result type (might be due to invalid IL or missing references)
		//IL_038a: Unknown result type (might be due to invalid IL or missing references)
		//IL_038c: Unknown result type (might be due to invalid IL or missing references)
		//IL_038e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0393: Unknown result type (might be due to invalid IL or missing references)
		//IL_0395: Unknown result type (might be due to invalid IL or missing references)
		//IL_039c: Expected O, but got Unknown
		//IL_039c: Unknown result type (might be due to invalid IL or missing references)
		//IL_039e: Unknown result type (might be due to invalid IL or missing references)
		//IL_03a5: Expected O, but got Unknown
		//IL_03a5: Unknown result type (might be due to invalid IL or missing references)
		//IL_03a7: Unknown result type (might be due to invalid IL or missing references)
		//IL_03ae: Expected O, but got Unknown
		//IL_03ae: Unknown result type (might be due to invalid IL or missing references)
		//IL_03b5: Expected O, but got Unknown
		//IL_03b5: Unknown result type (might be due to invalid IL or missing references)
		//IL_03b7: Unknown result type (might be due to invalid IL or missing references)
		//IL_03be: Expected O, but got Unknown
		double num = gem.GemXSize / 2.0;
		double num2 = gem.GemYSize / 2.0;
		double num3 = num / 5.0;
		double num4 = num2 / 5.0;
		Point3d val = default(Point3d);
		((Point3d)(ref val))._002Ector(5.0 * num3, 0.0, 0.0);
		Point3d val2 = default(Point3d);
		((Point3d)(ref val2))._002Ector(3.94733 * num3, 3.94733 * num4, 0.0);
		Point3d val3 = default(Point3d);
		((Point3d)(ref val3))._002Ector(3.94733 * num3, -3.94733 * num4, 0.0);
		Arc val4 = default(Arc);
		((Arc)(ref val4))._002Ector(val2, val, val3);
		Point3d val5 = default(Point3d);
		((Point3d)(ref val5))._002Ector(((Arc)(ref val4)).Center);
		double radius = ((Arc)(ref val4)).Radius;
		((Point3d)(ref val))._002Ector(0.0, 5.0 * num4, 0.0);
		((Point3d)(ref val2))._002Ector(-3.94733 * num3, 3.94733 * num4, 0.0);
		((Point3d)(ref val3))._002Ector(3.94733 * num3, 3.94733 * num4, 0.0);
		Arc val6 = default(Arc);
		((Arc)(ref val6))._002Ector(val2, val, val3);
		Point3d val7 = default(Point3d);
		((Point3d)(ref val7))._002Ector(((Arc)(ref val6)).Center);
		double radius2 = ((Arc)(ref val6)).Radius;
		double num5 = radius + distance;
		double a = radius2 + distance;
		Vector3d val8 = val5 - val7;
		double length = ((Vector3d)(ref val8)).Length;
		double num6 = AngleA(a, num5, length);
		Vector3d val9 = val7 - val5;
		((Vector3d)(ref val9)).Unitize();
		((Vector3d)(ref val9)).Rotate(num6, new Vector3d(Vector3d.ZAxis));
		val9 *= num5;
		Point3d val10 = val5 + val9;
		((Point3d)(ref val2))._002Ector(0.0 - ((Point3d)(ref val10)).X, ((Point3d)(ref val10)).Y, 0.0);
		((Point3d)(ref val))._002Ector(0.0, num2 + distance, 0.0);
		val3 = val10;
		Arc val11 = default(Arc);
		((Arc)(ref val11))._002Ector(val2, val, val3);
		Arc val12 = default(Arc);
		((Arc)(ref val12))._002Ector(val2, val, val3);
		Interval angleDomain = ((Arc)(ref val11)).AngleDomain;
		double mid = ((Interval)(ref angleDomain)).Mid;
		angleDomain = ((Arc)(ref val11)).AngleDomain;
		((Arc)(ref val11)).Trim(new Interval(mid, ((Interval)(ref angleDomain)).Max));
		angleDomain = ((Arc)(ref val12)).AngleDomain;
		double min = ((Interval)(ref angleDomain)).Min;
		angleDomain = ((Arc)(ref val12)).AngleDomain;
		((Arc)(ref val12)).Trim(new Interval(min, ((Interval)(ref angleDomain)).Mid));
		val2 = val10;
		((Point3d)(ref val))._002Ector(num + distance, 0.0, 0.0);
		((Point3d)(ref val3))._002Ector(((Point3d)(ref val10)).X, 0.0 - ((Point3d)(ref val10)).Y, 0.0);
		Arc val13 = default(Arc);
		((Arc)(ref val13))._002Ector(val2, val, val3);
		((Point3d)(ref val2))._002Ector(((Point3d)(ref val10)).X, 0.0 - ((Point3d)(ref val10)).Y, 0.0);
		((Point3d)(ref val))._002Ector(0.0, 0.0 - num2 - distance, 0.0);
		((Point3d)(ref val3))._002Ector(0.0 - ((Point3d)(ref val10)).X, 0.0 - ((Point3d)(ref val10)).Y, 0.0);
		Arc val14 = default(Arc);
		((Arc)(ref val14))._002Ector(val2, val, val3);
		((Point3d)(ref val2))._002Ector(0.0 - ((Point3d)(ref val10)).X, 0.0 - ((Point3d)(ref val10)).Y, 0.0);
		((Point3d)(ref val))._002Ector(0.0 - num - distance, 0.0, 0.0);
		((Point3d)(ref val3))._002Ector(0.0 - ((Point3d)(ref val10)).X, ((Point3d)(ref val10)).Y, 0.0);
		Arc val15 = new Arc(val2, val, val3);
		Curve curve1A = (Curve)new ArcCurve(val11);
		Curve curve = (Curve)new ArcCurve(val13);
		Curve curve2 = (Curve)new ArcCurve(val14);
		Curve curve3 = (Curve)new ArcCurve(val15);
		Curve curve1B = (Curve)new ArcCurve(val12);
		if (cornerBlend > 0.0)
		{
			Trim5Curves(cornerBlend, ref curve1A, ref curve, ref curve2, ref curve3, ref curve1B);
			BlendCorner(ref curve1A, ref curve);
			BlendCorner(ref curve, ref curve2);
			BlendCorner(ref curve2, ref curve3);
			BlendCorner(ref curve3, ref curve1B);
		}
		return (Curve[])(object)new Curve[5] { curve1A, curve, curve2, curve3, curve1B };
	}

	private static Curve[] GemOffsetCurvesCushionSquareRotated45(CSGemData gem, double distance)
	{
		//IL_0084: Unknown result type (might be due to invalid IL or missing references)
		//IL_0086: Unknown result type (might be due to invalid IL or missing references)
		//IL_0087: Unknown result type (might be due to invalid IL or missing references)
		//IL_008c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0090: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a6: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a8: Unknown result type (might be due to invalid IL or missing references)
		//IL_013e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0140: Unknown result type (might be due to invalid IL or missing references)
		//IL_0142: Unknown result type (might be due to invalid IL or missing references)
		//IL_0147: Unknown result type (might be due to invalid IL or missing references)
		//IL_014b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0161: Unknown result type (might be due to invalid IL or missing references)
		//IL_0163: Unknown result type (might be due to invalid IL or missing references)
		//IL_01ad: Unknown result type (might be due to invalid IL or missing references)
		//IL_01af: Unknown result type (might be due to invalid IL or missing references)
		//IL_01b0: Unknown result type (might be due to invalid IL or missing references)
		//IL_01b5: Unknown result type (might be due to invalid IL or missing references)
		//IL_01b7: Unknown result type (might be due to invalid IL or missing references)
		//IL_01b9: Unknown result type (might be due to invalid IL or missing references)
		//IL_01bb: Unknown result type (might be due to invalid IL or missing references)
		//IL_01c0: Unknown result type (might be due to invalid IL or missing references)
		//IL_01d2: Unknown result type (might be due to invalid IL or missing references)
		//IL_01d3: Unknown result type (might be due to invalid IL or missing references)
		//IL_01d5: Unknown result type (might be due to invalid IL or missing references)
		//IL_01d7: Unknown result type (might be due to invalid IL or missing references)
		//IL_0229: Unknown result type (might be due to invalid IL or missing references)
		//IL_022b: Unknown result type (might be due to invalid IL or missing references)
		//IL_022c: Unknown result type (might be due to invalid IL or missing references)
		//IL_022e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0280: Unknown result type (might be due to invalid IL or missing references)
		//IL_0281: Unknown result type (might be due to invalid IL or missing references)
		//IL_0283: Unknown result type (might be due to invalid IL or missing references)
		//IL_0285: Unknown result type (might be due to invalid IL or missing references)
		//IL_02d7: Unknown result type (might be due to invalid IL or missing references)
		//IL_02d9: Unknown result type (might be due to invalid IL or missing references)
		//IL_02da: Unknown result type (might be due to invalid IL or missing references)
		//IL_02dc: Unknown result type (might be due to invalid IL or missing references)
		double num = gem.GemXSize / 2.0;
		double num2 = gem.GemYSize / 2.0;
		double num3 = num / 5.58236;
		double num4 = num2 / 5.58236;
		Point3d val = default(Point3d);
		((Point3d)(ref val))._002Ector(0.0 * num3, 5.58236 * num4, 0.0);
		Point3d val2 = default(Point3d);
		((Point3d)(ref val2))._002Ector(1.055 * num3, 5.30386 * num4, 0.0);
		Vector3d val3 = val2 - val;
		Vector3d inputVector = default(Vector3d);
		((Vector3d)(ref inputVector))._002Ector(val3);
		((Vector3d)(ref inputVector)).X = 0.0 - ((Vector3d)(ref inputVector)).X;
		double num5 = CSBlockUtil.CSUtilMath.AngleBetweenVectors(val3, inputVector);
		double num6 = 1.0 / Math.Sin(num5 / 2.0);
		((Point3d)(ref val)).Y = ((Point3d)(ref val)).Y + num6 * distance;
		((Point3d)(ref val2)).Y = ((Point3d)(ref val2)).Y + num6 * distance;
		Point3d val4 = default(Point3d);
		((Point3d)(ref val4))._002Ector(5.58236 * num3, 0.0 * num4, 0.0);
		Point3d val5 = default(Point3d);
		((Point3d)(ref val5))._002Ector(5.30386 * num3, 1.055 * num4, 0.0);
		Vector3d val6 = val5 - val4;
		Vector3d inputVector2 = default(Vector3d);
		((Vector3d)(ref inputVector2))._002Ector(val6);
		((Vector3d)(ref inputVector2)).Y = 0.0 - ((Vector3d)(ref inputVector2)).Y;
		double num7 = CSBlockUtil.CSUtilMath.AngleBetweenVectors(val6, inputVector2);
		double num8 = 1.0 / Math.Sin(num7 / 2.0);
		((Point3d)(ref val4)).X = ((Point3d)(ref val4)).X + num8 * distance;
		((Point3d)(ref val5)).X = ((Point3d)(ref val5)).X + num8 * distance;
		val3 = val2 - val;
		val6 = val5 - val4;
		((Vector3d)(ref val3)).Unitize();
		((Vector3d)(ref val6)).Unitize();
		NurbsCurve val7 = CSUtilCurves.BlendCurve(val, val4, val3, val6, 0.85);
		((Point3d)(ref val)).Y = 0.0 - ((Point3d)(ref val)).Y;
		((Vector3d)(ref val3)).Y = 0.0 - ((Vector3d)(ref val3)).Y;
		((Vector3d)(ref val6)).Y = 0.0 - ((Vector3d)(ref val6)).Y;
		NurbsCurve val8 = CSUtilCurves.BlendCurve(val4, val, val6, val3, 0.85);
		((Point3d)(ref val4)).X = 0.0 - ((Point3d)(ref val4)).X;
		((Vector3d)(ref val3)).X = 0.0 - ((Vector3d)(ref val3)).X;
		((Vector3d)(ref val6)).X = 0.0 - ((Vector3d)(ref val6)).X;
		NurbsCurve val9 = CSUtilCurves.BlendCurve(val, val4, val3, val6, 0.85);
		((Point3d)(ref val)).Y = 0.0 - ((Point3d)(ref val)).Y;
		((Vector3d)(ref val3)).Y = 0.0 - ((Vector3d)(ref val3)).Y;
		((Vector3d)(ref val6)).Y = 0.0 - ((Vector3d)(ref val6)).Y;
		NurbsCurve val10 = CSUtilCurves.BlendCurve(val4, val, val6, val3, 0.85);
		return (Curve[])(object)new Curve[4]
		{
			(Curve)val7,
			(Curve)val8,
			(Curve)val9,
			(Curve)val10
		};
	}

	private static Curve[] GemOffsetCurvesEmerald(CSGemData gem, double distance, double cornerPercentX, double cornerPercentY, double cornerBlend = 0.0)
	{
		//IL_00a3: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a5: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a7: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ac: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c1: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c5: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ca: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ce: Unknown result type (might be due to invalid IL or missing references)
		//IL_00cf: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d1: Unknown result type (might be due to invalid IL or missing references)
		//IL_00fd: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ff: Unknown result type (might be due to invalid IL or missing references)
		//IL_0101: Unknown result type (might be due to invalid IL or missing references)
		//IL_0106: Unknown result type (might be due to invalid IL or missing references)
		//IL_011b: Unknown result type (might be due to invalid IL or missing references)
		//IL_011f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0124: Unknown result type (might be due to invalid IL or missing references)
		//IL_0128: Unknown result type (might be due to invalid IL or missing references)
		//IL_0129: Unknown result type (might be due to invalid IL or missing references)
		//IL_012b: Unknown result type (might be due to invalid IL or missing references)
		//IL_03ed: Unknown result type (might be due to invalid IL or missing references)
		//IL_03ef: Unknown result type (might be due to invalid IL or missing references)
		//IL_03f1: Unknown result type (might be due to invalid IL or missing references)
		//IL_03f8: Expected O, but got Unknown
		//IL_03f8: Unknown result type (might be due to invalid IL or missing references)
		//IL_03fa: Unknown result type (might be due to invalid IL or missing references)
		//IL_03fc: Unknown result type (might be due to invalid IL or missing references)
		//IL_0403: Expected O, but got Unknown
		//IL_0403: Unknown result type (might be due to invalid IL or missing references)
		//IL_0405: Unknown result type (might be due to invalid IL or missing references)
		//IL_0407: Unknown result type (might be due to invalid IL or missing references)
		//IL_040e: Expected O, but got Unknown
		//IL_040e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0410: Unknown result type (might be due to invalid IL or missing references)
		//IL_0412: Unknown result type (might be due to invalid IL or missing references)
		//IL_0419: Expected O, but got Unknown
		//IL_0419: Unknown result type (might be due to invalid IL or missing references)
		//IL_041b: Unknown result type (might be due to invalid IL or missing references)
		//IL_041d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0424: Expected O, but got Unknown
		//IL_0424: Unknown result type (might be due to invalid IL or missing references)
		//IL_0426: Unknown result type (might be due to invalid IL or missing references)
		//IL_0428: Unknown result type (might be due to invalid IL or missing references)
		//IL_042f: Expected O, but got Unknown
		//IL_042f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0431: Unknown result type (might be due to invalid IL or missing references)
		//IL_0433: Unknown result type (might be due to invalid IL or missing references)
		//IL_043a: Expected O, but got Unknown
		//IL_043a: Unknown result type (might be due to invalid IL or missing references)
		//IL_043c: Unknown result type (might be due to invalid IL or missing references)
		//IL_043e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0445: Expected O, but got Unknown
		//IL_0445: Unknown result type (might be due to invalid IL or missing references)
		//IL_0447: Unknown result type (might be due to invalid IL or missing references)
		//IL_0449: Unknown result type (might be due to invalid IL or missing references)
		//IL_0450: Expected O, but got Unknown
		double num = gem.GemXSize / 2.0;
		double num2 = gem.GemYSize / 2.0;
		Point3d val = default(Point3d);
		((Point3d)(ref val))._002Ector(num * cornerPercentX, num2, 0.0);
		Point3d val2 = default(Point3d);
		((Point3d)(ref val2))._002Ector(num, num2 * cornerPercentY, 0.0);
		Vector3d val3 = default(Vector3d);
		((Vector3d)(ref val3))._002Ector(((Point3d)(ref val)).Y - ((Point3d)(ref val2)).Y, 0.0 - (((Point3d)(ref val)).X - ((Point3d)(ref val2)).X), 0.0);
		((Vector3d)(ref val3)).Unitize();
		Vector3d val4 = default(Vector3d);
		((Vector3d)(ref val4))._002Ector(1.0, 0.0, 0.0);
		val4 += val3;
		((Vector3d)(ref val4)).Unitize();
		double num3 = distance / ((Vector3d)(ref val4)).X;
		val4 *= num3;
		Point3d val5 = default(Point3d);
		((Point3d)(ref val5))._002Ector(val2 + val4);
		Vector3d val6 = default(Vector3d);
		((Vector3d)(ref val6))._002Ector(0.0, 1.0, 0.0);
		val6 += val3;
		((Vector3d)(ref val6)).Unitize();
		double num4 = distance / ((Vector3d)(ref val6)).Y;
		val6 *= num4;
		Point3d val7 = default(Point3d);
		((Point3d)(ref val7))._002Ector(val + val6);
		if (((Point3d)(ref val7)).X + 0.02 > ((Point3d)(ref val5)).X)
		{
			Point3d val8 = default(Point3d);
			((Point3d)(ref val8))._002Ector(((Point3d)(ref val5)).X, ((Point3d)(ref val7)).Y, 0.0);
			((Point3d)(ref val5))._002Ector(((Point3d)(ref val8)).X, ((Point3d)(ref val8)).Y - 0.02, 0.0);
			((Point3d)(ref val7))._002Ector(((Point3d)(ref val8)).X - 0.02, ((Point3d)(ref val8)).Y, 0.0);
		}
		if (((Point3d)(ref val7)).X - 0.04 <= 0.0)
		{
			Point3d val9 = default(Point3d);
			((Point3d)(ref val9))._002Ector(0.04, ((Point3d)(ref val7)).Y, 0.0);
			((Point3d)(ref val5))._002Ector(((Point3d)(ref val9)).X, ((Point3d)(ref val9)).Y - 0.02, 0.0);
			((Point3d)(ref val7))._002Ector(((Point3d)(ref val9)).X - 0.02, ((Point3d)(ref val9)).Y, 0.0);
		}
		if (((Point3d)(ref val7)).Y - 0.04 <= 0.0)
		{
			Point3d val10 = default(Point3d);
			((Point3d)(ref val10))._002Ector(((Point3d)(ref val5)).X, 0.04, 0.0);
			((Point3d)(ref val5))._002Ector(((Point3d)(ref val10)).X, ((Point3d)(ref val10)).Y - 0.02, 0.0);
			((Point3d)(ref val7))._002Ector(((Point3d)(ref val10)).X - 0.02, ((Point3d)(ref val10)).Y, 0.0);
		}
		Point3d val11 = default(Point3d);
		((Point3d)(ref val11))._002Ector(0.0, ((Point3d)(ref val7)).Y, 0.0);
		Point3d val12 = default(Point3d);
		((Point3d)(ref val12))._002Ector(((Point3d)(ref val7)).X, ((Point3d)(ref val7)).Y, 0.0);
		Point3d val13 = default(Point3d);
		((Point3d)(ref val13))._002Ector(((Point3d)(ref val5)).X, ((Point3d)(ref val5)).Y, 0.0);
		Point3d val14 = default(Point3d);
		((Point3d)(ref val14))._002Ector(((Point3d)(ref val5)).X, 0.0 - ((Point3d)(ref val5)).Y, 0.0);
		Point3d val15 = default(Point3d);
		((Point3d)(ref val15))._002Ector(((Point3d)(ref val7)).X, 0.0 - ((Point3d)(ref val7)).Y, 0.0);
		Point3d val16 = default(Point3d);
		((Point3d)(ref val16))._002Ector(0.0 - ((Point3d)(ref val7)).X, 0.0 - ((Point3d)(ref val7)).Y, 0.0);
		Point3d val17 = default(Point3d);
		((Point3d)(ref val17))._002Ector(0.0 - ((Point3d)(ref val5)).X, 0.0 - ((Point3d)(ref val5)).Y, 0.0);
		Point3d val18 = default(Point3d);
		((Point3d)(ref val18))._002Ector(0.0 - ((Point3d)(ref val5)).X, ((Point3d)(ref val5)).Y, 0.0);
		Point3d val19 = default(Point3d);
		((Point3d)(ref val19))._002Ector(0.0 - ((Point3d)(ref val7)).X, ((Point3d)(ref val7)).Y, 0.0);
		Curve curve = (Curve)new LineCurve(val11, val12);
		Curve curve2 = (Curve)new LineCurve(val12, val13);
		Curve curve3 = (Curve)new LineCurve(val13, val14);
		Curve curve4 = (Curve)new LineCurve(val14, val15);
		Curve curve5 = (Curve)new LineCurve(val15, val16);
		Curve curve6 = (Curve)new LineCurve(val16, val17);
		Curve curve7 = (Curve)new LineCurve(val17, val18);
		Curve curve8 = (Curve)new LineCurve(val18, val19);
		Curve curve9 = (Curve)new LineCurve(val19, val11);
		if (cornerBlend > 0.0)
		{
			Trim9Curves(cornerBlend, ref curve, ref curve2, ref curve3, ref curve4, ref curve5, ref curve6, ref curve7, ref curve8, ref curve9);
			BlendCorner(ref curve, ref curve2);
			BlendCorner(ref curve2, ref curve3);
			BlendCorner(ref curve3, ref curve4);
			BlendCorner(ref curve4, ref curve5);
			BlendCorner(ref curve5, ref curve6);
			BlendCorner(ref curve6, ref curve7);
			BlendCorner(ref curve7, ref curve8);
			BlendCorner(ref curve8, ref curve9);
			BlendCorner(ref curve9, ref curve);
		}
		return (Curve[])(object)new Curve[9] { curve, curve2, curve3, curve4, curve5, curve6, curve7, curve8, curve9 };
	}

	private static Curve[] GemOffsetCurvesHeart(CSGemData gem, double distance, double cornerBlend = 0.0)
	{
		//IL_0022: Unknown result type (might be due to invalid IL or missing references)
		//IL_0046: Unknown result type (might be due to invalid IL or missing references)
		//IL_004b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0074: Unknown result type (might be due to invalid IL or missing references)
		//IL_007b: Expected O, but got Unknown
		//IL_00a7: Unknown result type (might be due to invalid IL or missing references)
		//IL_00dd: Unknown result type (might be due to invalid IL or missing references)
		//IL_0113: Unknown result type (might be due to invalid IL or missing references)
		//IL_0149: Unknown result type (might be due to invalid IL or missing references)
		//IL_017f: Unknown result type (might be due to invalid IL or missing references)
		//IL_01b5: Unknown result type (might be due to invalid IL or missing references)
		//IL_01eb: Unknown result type (might be due to invalid IL or missing references)
		//IL_030d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0312: Unknown result type (might be due to invalid IL or missing references)
		//IL_0314: Unknown result type (might be due to invalid IL or missing references)
		//IL_033a: Unknown result type (might be due to invalid IL or missing references)
		//IL_033f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0343: Unknown result type (might be due to invalid IL or missing references)
		//IL_0344: Unknown result type (might be due to invalid IL or missing references)
		//IL_0346: Unknown result type (might be due to invalid IL or missing references)
		//IL_034b: Unknown result type (might be due to invalid IL or missing references)
		//IL_034d: Unknown result type (might be due to invalid IL or missing references)
		//IL_035c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0363: Expected O, but got Unknown
		//IL_038f: Unknown result type (might be due to invalid IL or missing references)
		//IL_03c5: Unknown result type (might be due to invalid IL or missing references)
		//IL_03fb: Unknown result type (might be due to invalid IL or missing references)
		//IL_0431: Unknown result type (might be due to invalid IL or missing references)
		//IL_0467: Unknown result type (might be due to invalid IL or missing references)
		//IL_049d: Unknown result type (might be due to invalid IL or missing references)
		//IL_05c0: Unknown result type (might be due to invalid IL or missing references)
		//IL_05c5: Unknown result type (might be due to invalid IL or missing references)
		//IL_05c7: Unknown result type (might be due to invalid IL or missing references)
		//IL_05ed: Unknown result type (might be due to invalid IL or missing references)
		//IL_05f2: Unknown result type (might be due to invalid IL or missing references)
		//IL_05f6: Unknown result type (might be due to invalid IL or missing references)
		//IL_05f7: Unknown result type (might be due to invalid IL or missing references)
		//IL_05f9: Unknown result type (might be due to invalid IL or missing references)
		//IL_05fe: Unknown result type (might be due to invalid IL or missing references)
		//IL_0600: Unknown result type (might be due to invalid IL or missing references)
		//IL_060f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0616: Expected O, but got Unknown
		//IL_0642: Unknown result type (might be due to invalid IL or missing references)
		//IL_0678: Unknown result type (might be due to invalid IL or missing references)
		//IL_06ae: Unknown result type (might be due to invalid IL or missing references)
		//IL_06e4: Unknown result type (might be due to invalid IL or missing references)
		//IL_07c1: Unknown result type (might be due to invalid IL or missing references)
		//IL_07c6: Unknown result type (might be due to invalid IL or missing references)
		//IL_07c8: Unknown result type (might be due to invalid IL or missing references)
		//IL_07e1: Unknown result type (might be due to invalid IL or missing references)
		//IL_07e6: Unknown result type (might be due to invalid IL or missing references)
		//IL_0806: Unknown result type (might be due to invalid IL or missing references)
		//IL_080b: Unknown result type (might be due to invalid IL or missing references)
		//IL_080f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0810: Unknown result type (might be due to invalid IL or missing references)
		//IL_0812: Unknown result type (might be due to invalid IL or missing references)
		//IL_0817: Unknown result type (might be due to invalid IL or missing references)
		//IL_0819: Unknown result type (might be due to invalid IL or missing references)
		//IL_081e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0820: Unknown result type (might be due to invalid IL or missing references)
		//IL_082b: Unknown result type (might be due to invalid IL or missing references)
		//IL_084b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0850: Unknown result type (might be due to invalid IL or missing references)
		//IL_085d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0878: Unknown result type (might be due to invalid IL or missing references)
		//IL_0893: Unknown result type (might be due to invalid IL or missing references)
		//IL_08b4: Unknown result type (might be due to invalid IL or missing references)
		//IL_08b9: Unknown result type (might be due to invalid IL or missing references)
		//IL_08bf: Unknown result type (might be due to invalid IL or missing references)
		//IL_08c6: Unknown result type (might be due to invalid IL or missing references)
		//IL_08cb: Unknown result type (might be due to invalid IL or missing references)
		//IL_08dd: Unknown result type (might be due to invalid IL or missing references)
		//IL_08df: Unknown result type (might be due to invalid IL or missing references)
		//IL_08e2: Unknown result type (might be due to invalid IL or missing references)
		//IL_08e7: Unknown result type (might be due to invalid IL or missing references)
		//IL_08ec: Unknown result type (might be due to invalid IL or missing references)
		//IL_08f0: Unknown result type (might be due to invalid IL or missing references)
		//IL_08fe: Unknown result type (might be due to invalid IL or missing references)
		//IL_0903: Unknown result type (might be due to invalid IL or missing references)
		//IL_090e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0921: Unknown result type (might be due to invalid IL or missing references)
		//IL_0926: Unknown result type (might be due to invalid IL or missing references)
		//IL_092c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0933: Unknown result type (might be due to invalid IL or missing references)
		//IL_0938: Unknown result type (might be due to invalid IL or missing references)
		//IL_094a: Unknown result type (might be due to invalid IL or missing references)
		//IL_094c: Unknown result type (might be due to invalid IL or missing references)
		//IL_094f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0954: Unknown result type (might be due to invalid IL or missing references)
		//IL_0959: Unknown result type (might be due to invalid IL or missing references)
		//IL_095d: Unknown result type (might be due to invalid IL or missing references)
		//IL_096d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0972: Unknown result type (might be due to invalid IL or missing references)
		//IL_097b: Unknown result type (might be due to invalid IL or missing references)
		//IL_09b1: Unknown result type (might be due to invalid IL or missing references)
		//IL_09b6: Unknown result type (might be due to invalid IL or missing references)
		//IL_09bc: Unknown result type (might be due to invalid IL or missing references)
		//IL_09c3: Unknown result type (might be due to invalid IL or missing references)
		//IL_09c8: Unknown result type (might be due to invalid IL or missing references)
		//IL_09da: Unknown result type (might be due to invalid IL or missing references)
		//IL_09dc: Unknown result type (might be due to invalid IL or missing references)
		//IL_09df: Unknown result type (might be due to invalid IL or missing references)
		//IL_09e4: Unknown result type (might be due to invalid IL or missing references)
		//IL_09e9: Unknown result type (might be due to invalid IL or missing references)
		//IL_09ed: Unknown result type (might be due to invalid IL or missing references)
		//IL_09fb: Unknown result type (might be due to invalid IL or missing references)
		//IL_0a00: Unknown result type (might be due to invalid IL or missing references)
		//IL_0a0b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0a1e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0a23: Unknown result type (might be due to invalid IL or missing references)
		//IL_0a29: Unknown result type (might be due to invalid IL or missing references)
		//IL_0a30: Unknown result type (might be due to invalid IL or missing references)
		//IL_0a35: Unknown result type (might be due to invalid IL or missing references)
		//IL_0a47: Unknown result type (might be due to invalid IL or missing references)
		//IL_0a49: Unknown result type (might be due to invalid IL or missing references)
		//IL_0a4c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0a51: Unknown result type (might be due to invalid IL or missing references)
		//IL_0a56: Unknown result type (might be due to invalid IL or missing references)
		//IL_0a5a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0a6a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0a6f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0a78: Unknown result type (might be due to invalid IL or missing references)
		double num = gem.GemXSize / 2.0;
		double num2 = gem.GemYSize / 2.0;
		Transform val = Transform.Scale(Plane.WorldXY, num / 5.0, num2 / 5.0, 1.0);
		double num3 = 5.0 / num;
		double num4 = 5.0 / num2;
		double num5 = distance * num3;
		double num6 = distance * num4;
		NurbsCurve val2 = new NurbsCurve(3, false, 4, 7);
		val2.Points[0] = new ControlPoint(5.0066166789220405, 0.0, 0.0, 1.0);
		val2.Points[1] = new ControlPoint(5.006616678922161, -0.6563961048979948, 0.0, 1.0);
		val2.Points[2] = new ControlPoint(4.649296555283862, -2.0660402223759666, 0.0, 1.0);
		val2.Points[3] = new ControlPoint(3.3947011721913873, -3.8722073371279846, 0.0, 1.0);
		val2.Points[4] = new ControlPoint(1.870281543227221, -5.609876323245201, 0.0, 1.0);
		val2.Points[5] = new ControlPoint(0.6112959541798841, -6.50455766084459, 0.0, 1.0);
		val2.Points[6] = new ControlPoint(0.0, -6.966404742505439, 0.0, 1.0);
		val2.Knots[0] = 0.0;
		val2.Knots[1] = 0.0;
		val2.Knots[2] = 0.0;
		val2.Knots[3] = 0.22119745803368646;
		val2.Knots[4] = 0.480798305355791;
		val2.Knots[5] = 0.7403991526778955;
		val2.Knots[6] = 1.0;
		val2.Knots[7] = 1.0;
		val2.Knots[8] = 1.0;
		double num7 = 12.9866 + num6;
		double num8 = Math.Sqrt(num7 * num7 - 57.072887715600004) - 3.59672;
		double num9 = 5.0 + num5;
		Transform val3 = Transform.Translation(0.0, 1.4744, 0.0);
		Transform val4 = Transform.Scale(Plane.WorldXY, num9 / 5.0, num8 / 6.9664, 1.0);
		((GeometryBase)val2).Transform(val * val3 * val4);
		NurbsCurve val5 = new NurbsCurve(3, false, 4, 6);
		val5.Points[0] = new ControlPoint(0.0, 3.0365333791772997, 0.0, 1.0);
		val5.Points[1] = new ControlPoint(0.6449175426389968, 3.0365333791772997, 0.0, 1.0);
		val5.Points[2] = new ControlPoint(2.144578282320578, 2.6159447841997165, 0.0, 1.0);
		val5.Points[3] = new ControlPoint(2.796856932501917, 1.0001195190754388, 0.0, 1.0);
		val5.Points[4] = new ControlPoint(2.835307823555864, 0.11396078132127507, 0.0, 1.0);
		val5.Points[5] = new ControlPoint(2.8353078235558855, 0.0, 0.0, 1.0);
		val5.Knots[0] = 0.0;
		val5.Knots[1] = 0.0;
		val5.Knots[2] = 0.0;
		val5.Knots[3] = 0.45097144418550456;
		val5.Knots[4] = 0.9292474576898094;
		val5.Knots[5] = 1.0;
		val5.Knots[6] = 1.0;
		val5.Knots[7] = 1.0;
		num8 = 3.0365333791772997 + num6;
		num9 = 2.8353078235558855 + num5;
		num9 = Math.Round(num9, 2, MidpointRounding.AwayFromZero);
		if (num9 < 0.04)
		{
			num5 = -2.7953078235558855;
			distance = num5 / num3;
			return GemOffsetCurvesHeart(gem, distance, cornerBlend);
		}
		val3 = Transform.Translation(2.17131, 1.4744, 0.0);
		val4 = Transform.Scale(Plane.WorldXY, num9 / 2.8353078235558855, num8 / 3.0365333791772997, 1.0);
		((GeometryBase)val5).Transform(val * val3 * val4);
		NurbsCurve val6 = new NurbsCurve(3, false, 4, 4);
		val6.Points[0] = new ControlPoint(0.0, 1.5786909734483108, 0.0, 1.0);
		val6.Points[1] = new ControlPoint(0.3572666485958004, 2.6632323152819315, 0.0, 1.0);
		val6.Points[2] = new ControlPoint(1.2783305169636177, 3.0286525140927982, 0.0, 1.0);
		val6.Points[3] = new ControlPoint(2.171308855366155, 3.0365333791772997, 0.0, 1.0);
		val6.Knots[0] = 0.0;
		val6.Knots[1] = 0.0;
		val6.Knots[2] = 0.0;
		val6.Knots[3] = 1.0;
		val6.Knots[4] = 1.0;
		val6.Knots[5] = 1.0;
		double num10 = num6;
		if (num10 < 0.0)
		{
			num10 = 0.0;
		}
		double num11 = 1.0 / (num10 + 1.0);
		Transform val7 = Transform.Translation(0.0, -3.0365333791772997, 0.0);
		val4 = Transform.Scale(Plane.WorldXY, 1.0, num11, 1.0);
		val3 = Transform.Translation(0.0, 4.5109333791773 + num6, 0.0);
		((GeometryBase)val6).Transform(val * val3 * val4 * val7);
		Transform val8 = Transform.Scale(Plane.WorldXY, -1.0, 1.0, 1.0);
		NurbsCurve val9 = ((Curve)val2).ToNurbsCurve();
		((GeometryBase)val9).Transform(val8);
		((Curve)val9).Reverse();
		NurbsCurve val10 = ((Curve)val5).ToNurbsCurve();
		((GeometryBase)val10).Transform(val8);
		((Curve)val10).Reverse();
		NurbsCurve val11 = ((Curve)val6).ToNurbsCurve();
		((GeometryBase)val11).Transform(val8);
		((Curve)val11).Reverse();
		if (cornerBlend > 0.0)
		{
			Point3d pointAtEnd = ((Curve)val2).PointAtEnd;
			Vector3d val12 = default(Vector3d);
			((Vector3d)(ref val12))._002Ector(((Curve)val2).PointAtEnd - ((Curve)val2).PointAtStart);
			((Vector3d)(ref val12)).Unitize();
			pointAtEnd -= val12 * cornerBlend;
			double num12 = default(double);
			((Curve)val2).ClosestPoint(pointAtEnd, ref num12);
			NurbsCurve obj = val2;
			Interval domain = ((Curve)val2).Domain;
			val2 = ((Curve)obj).Trim(new Interval(((Interval)(ref domain)).Min, num12)).ToNurbsCurve();
			pointAtEnd = ((Curve)val9).PointAtStart;
			((Vector3d)(ref val12))._002Ector(((Curve)val9).PointAtEnd - ((Curve)val9).PointAtStart);
			((Vector3d)(ref val12)).Unitize();
			pointAtEnd += val12 * cornerBlend;
			((Curve)val9).ClosestPoint(pointAtEnd, ref num12);
			NurbsCurve obj2 = val9;
			double num13 = num12;
			domain = ((Curve)val9).Domain;
			val9 = ((Curve)obj2).Trim(new Interval(num13, ((Interval)(ref domain)).Max)).ToNurbsCurve();
			Curve curve = (Curve)(object)val2;
			Curve curve2 = (Curve)(object)val9;
			BlendCorner(ref curve, ref curve2);
			val2 = curve.ToNurbsCurve();
			val9 = curve2.ToNurbsCurve();
			pointAtEnd = ((Curve)val11).PointAtEnd;
			((Vector3d)(ref val12))._002Ector(((Curve)val11).PointAtEnd - ((Curve)val11).PointAtStart);
			((Vector3d)(ref val12)).Unitize();
			pointAtEnd -= val12 * cornerBlend;
			((Curve)val11).ClosestPoint(pointAtEnd, ref num12);
			NurbsCurve obj3 = val11;
			domain = ((Curve)val11).Domain;
			val11 = ((Curve)obj3).Trim(new Interval(((Interval)(ref domain)).Min, num12)).ToNurbsCurve();
			pointAtEnd = ((Curve)val6).PointAtStart;
			((Vector3d)(ref val12))._002Ector(((Curve)val6).PointAtEnd - ((Curve)val6).PointAtStart);
			((Vector3d)(ref val12)).Unitize();
			pointAtEnd += val12 * cornerBlend;
			((Curve)val6).ClosestPoint(pointAtEnd, ref num12);
			NurbsCurve obj4 = val6;
			double num14 = num12;
			domain = ((Curve)val6).Domain;
			val6 = ((Curve)obj4).Trim(new Interval(num14, ((Interval)(ref domain)).Max)).ToNurbsCurve();
			Curve curve3 = (Curve)(object)val6;
			Curve curve4 = (Curve)(object)val11;
			BlendCorner(ref curve4, ref curve3);
			val6 = curve3.ToNurbsCurve();
			val11 = curve4.ToNurbsCurve();
		}
		return (Curve[])(object)new Curve[6]
		{
			(Curve)val6,
			(Curve)val5,
			(Curve)val2,
			(Curve)val9,
			(Curve)val10,
			(Curve)val11
		};
	}

	private static Curve[] GemOffsetCurvesMarquise(CSGemData gem, double distance, double cornerBlend = 0.0)
	{
		//IL_00ca: Unknown result type (might be due to invalid IL or missing references)
		//IL_00cc: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ce: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d5: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f9: Unknown result type (might be due to invalid IL or missing references)
		//IL_00fe: Unknown result type (might be due to invalid IL or missing references)
		//IL_010b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0113: Unknown result type (might be due to invalid IL or missing references)
		//IL_0133: Unknown result type (might be due to invalid IL or missing references)
		//IL_0138: Unknown result type (might be due to invalid IL or missing references)
		//IL_0145: Unknown result type (might be due to invalid IL or missing references)
		//IL_015e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0163: Unknown result type (might be due to invalid IL or missing references)
		//IL_0165: Unknown result type (might be due to invalid IL or missing references)
		//IL_017a: Unknown result type (might be due to invalid IL or missing references)
		//IL_017f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0184: Unknown result type (might be due to invalid IL or missing references)
		//IL_0188: Unknown result type (might be due to invalid IL or missing references)
		//IL_0198: Unknown result type (might be due to invalid IL or missing references)
		//IL_019d: Unknown result type (might be due to invalid IL or missing references)
		//IL_01a6: Unknown result type (might be due to invalid IL or missing references)
		//IL_01b9: Unknown result type (might be due to invalid IL or missing references)
		//IL_01be: Unknown result type (might be due to invalid IL or missing references)
		//IL_01c0: Unknown result type (might be due to invalid IL or missing references)
		//IL_01d6: Unknown result type (might be due to invalid IL or missing references)
		//IL_01db: Unknown result type (might be due to invalid IL or missing references)
		//IL_01e0: Unknown result type (might be due to invalid IL or missing references)
		//IL_01e4: Unknown result type (might be due to invalid IL or missing references)
		//IL_01f2: Unknown result type (might be due to invalid IL or missing references)
		//IL_01f7: Unknown result type (might be due to invalid IL or missing references)
		//IL_0202: Unknown result type (might be due to invalid IL or missing references)
		//IL_0215: Unknown result type (might be due to invalid IL or missing references)
		//IL_021a: Unknown result type (might be due to invalid IL or missing references)
		//IL_021c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0232: Unknown result type (might be due to invalid IL or missing references)
		//IL_0237: Unknown result type (might be due to invalid IL or missing references)
		//IL_023c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0240: Unknown result type (might be due to invalid IL or missing references)
		//IL_0250: Unknown result type (might be due to invalid IL or missing references)
		//IL_0255: Unknown result type (might be due to invalid IL or missing references)
		//IL_025e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0271: Unknown result type (might be due to invalid IL or missing references)
		//IL_0276: Unknown result type (might be due to invalid IL or missing references)
		//IL_0278: Unknown result type (might be due to invalid IL or missing references)
		//IL_028d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0292: Unknown result type (might be due to invalid IL or missing references)
		//IL_0297: Unknown result type (might be due to invalid IL or missing references)
		//IL_029b: Unknown result type (might be due to invalid IL or missing references)
		//IL_02a9: Unknown result type (might be due to invalid IL or missing references)
		//IL_02ae: Unknown result type (might be due to invalid IL or missing references)
		//IL_02b9: Unknown result type (might be due to invalid IL or missing references)
		double gemXSize = gem.GemXSize;
		double gemYSize = gem.GemYSize;
		double num = 5.0 / gemXSize;
		if (distance < 0.0 - gemXSize / 2.0 + 0.01)
		{
			distance = 0.0 - gemXSize / 2.0 + 0.01;
		}
		double num2 = distance * num;
		double num3 = num2 + 6.25;
		double num4 = Math.Sqrt(num3 * num3 - 14.0625);
		Point3d val = default(Point3d);
		((Point3d)(ref val))._002Ector(0.0, num4, 0.0);
		Point3d val2 = default(Point3d);
		((Point3d)(ref val2))._002Ector(2.5 + num2, 0.0, 0.0);
		Point3d val3 = default(Point3d);
		((Point3d)(ref val3))._002Ector(0.0, 0.0 - num4, 0.0);
		Arc val4 = default(Arc);
		((Arc)(ref val4))._002Ector(val, val2, val3);
		Transform val5 = Transform.Scale(Plane.WorldXY, gemXSize / 5.0, gemYSize / 10.0, 1.0);
		NurbsCurve val6 = ((Arc)(ref val4)).ToNurbsCurve();
		((GeometryBase)val6).Transform(val5);
		Transform val7 = Transform.Scale(Plane.WorldXY, -1.0, -1.0, 1.0);
		NurbsCurve val8 = ((Curve)val6).ToNurbsCurve();
		((GeometryBase)val8).Transform(val7);
		if (cornerBlend > 0.0)
		{
			Point3d pointAtStart = ((Curve)val8).PointAtStart;
			pointAtStart += new Vector3d(0.0, cornerBlend, 0.0);
			double num5 = default(double);
			((Curve)val8).ClosestPoint(pointAtStart, ref num5);
			NurbsCurve obj = val8;
			double num6 = num5;
			Interval domain = ((Curve)val8).Domain;
			val8 = ((Curve)obj).Trim(new Interval(num6, ((Interval)(ref domain)).Max)).ToNurbsCurve();
			pointAtStart = ((Curve)val8).PointAtEnd;
			pointAtStart += new Vector3d(0.0, 0.0 - cornerBlend, 0.0);
			((Curve)val8).ClosestPoint(pointAtStart, ref num5);
			NurbsCurve obj2 = val8;
			domain = ((Curve)val8).Domain;
			val8 = ((Curve)obj2).Trim(new Interval(((Interval)(ref domain)).Min, num5)).ToNurbsCurve();
			pointAtStart = ((Curve)val6).PointAtStart;
			pointAtStart += new Vector3d(0.0, 0.0 - cornerBlend, 0.0);
			((Curve)val6).ClosestPoint(pointAtStart, ref num5);
			NurbsCurve obj3 = val6;
			double num7 = num5;
			domain = ((Curve)val6).Domain;
			val6 = ((Curve)obj3).Trim(new Interval(num7, ((Interval)(ref domain)).Max)).ToNurbsCurve();
			pointAtStart = ((Curve)val6).PointAtEnd;
			pointAtStart += new Vector3d(0.0, cornerBlend, 0.0);
			((Curve)val6).ClosestPoint(pointAtStart, ref num5);
			NurbsCurve obj4 = val6;
			domain = ((Curve)val6).Domain;
			val6 = ((Curve)obj4).Trim(new Interval(((Interval)(ref domain)).Min, num5)).ToNurbsCurve();
			Curve curve = (Curve)(object)val6;
			Curve curve2 = (Curve)(object)val8;
			BlendCorner(ref curve, ref curve2);
			BlendCorner(ref curve2, ref curve);
			val6 = curve.ToNurbsCurve();
			val8 = curve2.ToNurbsCurve();
		}
		return (Curve[])(object)new Curve[2]
		{
			(Curve)val6,
			(Curve)val8
		};
	}

	private static Curve[] GemOffsetCurvesMarquiseRotated90(CSGemData gem, double distance)
	{
		//IL_00ca: Unknown result type (might be due to invalid IL or missing references)
		//IL_00cc: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ce: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d5: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f9: Unknown result type (might be due to invalid IL or missing references)
		//IL_00fe: Unknown result type (might be due to invalid IL or missing references)
		//IL_010b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0117: Unknown result type (might be due to invalid IL or missing references)
		//IL_011c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0127: Unknown result type (might be due to invalid IL or missing references)
		//IL_012c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0135: Unknown result type (might be due to invalid IL or missing references)
		//IL_0145: Unknown result type (might be due to invalid IL or missing references)
		//IL_014a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0155: Unknown result type (might be due to invalid IL or missing references)
		//IL_015a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0163: Unknown result type (might be due to invalid IL or missing references)
		//IL_016f: Unknown result type (might be due to invalid IL or missing references)
		//IL_018f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0194: Unknown result type (might be due to invalid IL or missing references)
		//IL_01a1: Unknown result type (might be due to invalid IL or missing references)
		//IL_01bc: Unknown result type (might be due to invalid IL or missing references)
		double gemXSize = gem.GemXSize;
		double gemYSize = gem.GemYSize;
		double num = 5.0 / gemYSize;
		if (distance < 0.0 - gemYSize / 2.0 + 0.01)
		{
			distance = 0.0 - gemYSize / 2.0 + 0.01;
		}
		double num2 = distance * num;
		double num3 = num2 + 6.25;
		double num4 = Math.Sqrt(num3 * num3 - 14.0625);
		Point3d val = default(Point3d);
		((Point3d)(ref val))._002Ector(0.0 - num4, 0.0, 0.0);
		Point3d val2 = default(Point3d);
		((Point3d)(ref val2))._002Ector(0.0, 2.5 + num2, 0.0);
		Point3d val3 = default(Point3d);
		((Point3d)(ref val3))._002Ector(num4, 0.0, 0.0);
		Arc val4 = default(Arc);
		((Arc)(ref val4))._002Ector(val, val2, val3);
		Transform val5 = Transform.Scale(Plane.WorldXY, gemXSize / 10.0, gemYSize / 5.0, 1.0);
		NurbsCurve val6 = ((Arc)(ref val4)).ToNurbsCurve();
		((GeometryBase)val6).Transform(val5);
		Interval domain = ((Curve)val6).Domain;
		double mid = ((Interval)(ref domain)).Mid;
		domain = ((Curve)val6).Domain;
		Curve val7 = ((Curve)val6).Trim(new Interval(mid, ((Interval)(ref domain)).Max));
		domain = ((Curve)val6).Domain;
		double min = ((Interval)(ref domain)).Min;
		domain = ((Curve)val6).Domain;
		Curve val8 = ((Curve)val6).Trim(new Interval(min, ((Interval)(ref domain)).Mid));
		Transform val9 = Transform.Scale(Plane.WorldXY, 1.0, -1.0, 1.0);
		Curve val10 = val7.DuplicateCurve();
		((GeometryBase)val10).Transform(val9);
		val10.Reverse();
		Curve val11 = val8.DuplicateCurve();
		((GeometryBase)val11).Transform(val9);
		val11.Reverse();
		return (Curve[])(object)new Curve[4] { val7, val10, val11, val8 };
	}

	private static Curve[] GemOffsetCurvesPear(CSGemData gem, double distance, double cornerBlend = 0.0)
	{
		//IL_0075: Unknown result type (might be due to invalid IL or missing references)
		//IL_0087: Unknown result type (might be due to invalid IL or missing references)
		//IL_008c: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ab: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b0: Unknown result type (might be due to invalid IL or missing references)
		//IL_00cf: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ef: Unknown result type (might be due to invalid IL or missing references)
		//IL_010f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0124: Unknown result type (might be due to invalid IL or missing references)
		//IL_0126: Unknown result type (might be due to invalid IL or missing references)
		//IL_0128: Unknown result type (might be due to invalid IL or missing references)
		//IL_0135: Unknown result type (might be due to invalid IL or missing references)
		//IL_01ae: Unknown result type (might be due to invalid IL or missing references)
		//IL_01b0: Unknown result type (might be due to invalid IL or missing references)
		//IL_01b2: Unknown result type (might be due to invalid IL or missing references)
		//IL_01be: Unknown result type (might be due to invalid IL or missing references)
		//IL_01c0: Unknown result type (might be due to invalid IL or missing references)
		//IL_01c2: Unknown result type (might be due to invalid IL or missing references)
		//IL_01dc: Unknown result type (might be due to invalid IL or missing references)
		//IL_01de: Unknown result type (might be due to invalid IL or missing references)
		//IL_0257: Unknown result type (might be due to invalid IL or missing references)
		//IL_0274: Unknown result type (might be due to invalid IL or missing references)
		//IL_0279: Unknown result type (might be due to invalid IL or missing references)
		//IL_029e: Unknown result type (might be due to invalid IL or missing references)
		//IL_02be: Unknown result type (might be due to invalid IL or missing references)
		//IL_02c3: Unknown result type (might be due to invalid IL or missing references)
		//IL_02d0: Unknown result type (might be due to invalid IL or missing references)
		//IL_02eb: Unknown result type (might be due to invalid IL or missing references)
		//IL_0328: Unknown result type (might be due to invalid IL or missing references)
		//IL_0352: Unknown result type (might be due to invalid IL or missing references)
		//IL_037c: Unknown result type (might be due to invalid IL or missing references)
		//IL_03a6: Unknown result type (might be due to invalid IL or missing references)
		//IL_03c2: Unknown result type (might be due to invalid IL or missing references)
		//IL_03c7: Unknown result type (might be due to invalid IL or missing references)
		//IL_03cd: Unknown result type (might be due to invalid IL or missing references)
		//IL_03d4: Unknown result type (might be due to invalid IL or missing references)
		//IL_03d9: Unknown result type (might be due to invalid IL or missing references)
		//IL_03eb: Unknown result type (might be due to invalid IL or missing references)
		//IL_03ed: Unknown result type (might be due to invalid IL or missing references)
		//IL_03f0: Unknown result type (might be due to invalid IL or missing references)
		//IL_03f5: Unknown result type (might be due to invalid IL or missing references)
		//IL_03fa: Unknown result type (might be due to invalid IL or missing references)
		//IL_03fe: Unknown result type (might be due to invalid IL or missing references)
		//IL_040c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0411: Unknown result type (might be due to invalid IL or missing references)
		//IL_041c: Unknown result type (might be due to invalid IL or missing references)
		//IL_042f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0434: Unknown result type (might be due to invalid IL or missing references)
		//IL_043a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0441: Unknown result type (might be due to invalid IL or missing references)
		//IL_0446: Unknown result type (might be due to invalid IL or missing references)
		//IL_0458: Unknown result type (might be due to invalid IL or missing references)
		//IL_045a: Unknown result type (might be due to invalid IL or missing references)
		//IL_045d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0462: Unknown result type (might be due to invalid IL or missing references)
		//IL_0467: Unknown result type (might be due to invalid IL or missing references)
		//IL_046b: Unknown result type (might be due to invalid IL or missing references)
		//IL_047b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0480: Unknown result type (might be due to invalid IL or missing references)
		//IL_0489: Unknown result type (might be due to invalid IL or missing references)
		double gemXSize = gem.GemXSize;
		double gemYSize = gem.GemYSize;
		if (distance < (0.0 - gemXSize) / 2.0 + 0.01)
		{
			distance = (0.0 - gemXSize) / 2.0 + 0.01;
		}
		double num = gemXSize / 7.0;
		double num2 = gemYSize / 10.0;
		double num3 = 3.5 * num + distance;
		double num4 = 3.81 * num2 + distance;
		Transform val = Transform.Scale(Plane.WorldXY, num3, num4, 1.0);
		Transform val2 = Transform.Translation(0.0, -0.285806 * num2, 0.0);
		Arc val3 = default(Arc);
		((Arc)(ref val3))._002Ector(new Point3d(0.0, 1.0, 0.0), new Point3d(0.707, 0.707, 0.0), new Point3d(1.0, 0.0, 0.0));
		NurbsCurve val4 = ((Arc)(ref val3)).ToNurbsCurve();
		((GeometryBase)val4).Transform(val2 * val);
		Point3d pointAtEnd = ((Curve)val4).PointAtEnd;
		Point3d val5 = default(Point3d);
		((Point3d)(ref val5))._002Ector(0.0 * num, -6.47576 * num2, 0.0);
		Point3d val6 = default(Point3d);
		((Point3d)(ref val6))._002Ector(0.173945 * num, -6.35321 * num2, 0.0);
		Point3d val7 = default(Point3d);
		((Point3d)(ref val7))._002Ector(-0.173945 * num, -6.35321 * num2, 0.0);
		Vector3d val8 = default(Vector3d);
		((Vector3d)(ref val8))._002Ector(val6 - val5);
		Vector3d inputVector = default(Vector3d);
		((Vector3d)(ref inputVector))._002Ector(val7 - val5);
		((Vector3d)(ref val8)).Unitize();
		((Vector3d)(ref inputVector)).Unitize();
		double num5 = CSBlockUtil.CSUtilMath.AngleBetweenVectors(val8, inputVector);
		double num6 = 1.0 / Math.Sin(num5 / 2.0);
		((Point3d)(ref val5)).Y = ((Point3d)(ref val5)).Y - num6 * distance;
		if (distance < 0.0)
		{
			double num7 = distance / (gemXSize / 2.0);
			((Vector3d)(ref val8)).X = ((Vector3d)(ref val8)).X * (1.0 - num7 * num7);
			((Vector3d)(ref val8)).Unitize();
		}
		NurbsCurve val9 = CSUtilCurves.BlendCurve(pointAtEnd, val5, new Vector3d(0.0, -1.0, 0.0), val8, 0.8);
		Transform val10 = Transform.Scale(Plane.WorldXY, -1.0, 1.0, 1.0);
		Curve val11 = ((Curve)val9).DuplicateCurve();
		((GeometryBase)val11).Transform(val10);
		val11.Reverse();
		Curve val12 = ((Curve)val4).DuplicateCurve();
		((GeometryBase)val12).Transform(val10);
		val12.Reverse();
		if (gem.VersionMajor > 1)
		{
			((GeometryBase)val9).Translate(new Vector3d(0.0, 0.08 * gemYSize, 0.0));
			((GeometryBase)val4).Translate(new Vector3d(0.0, 0.08 * gemYSize, 0.0));
			((GeometryBase)val11).Translate(new Vector3d(0.0, 0.08 * gemYSize, 0.0));
			((GeometryBase)val12).Translate(new Vector3d(0.0, 0.08 * gemYSize, 0.0));
		}
		if (cornerBlend > 0.0)
		{
			Point3d pointAtEnd2 = ((Curve)val9).PointAtEnd;
			Vector3d val13 = default(Vector3d);
			((Vector3d)(ref val13))._002Ector(((Curve)val9).PointAtEnd - ((Curve)val9).PointAtStart);
			((Vector3d)(ref val13)).Unitize();
			pointAtEnd2 -= val13 * cornerBlend;
			double num8 = default(double);
			((Curve)val9).ClosestPoint(pointAtEnd2, ref num8);
			NurbsCurve obj = val9;
			Interval domain = ((Curve)val9).Domain;
			val9 = ((Curve)obj).Trim(new Interval(((Interval)(ref domain)).Min, num8)).ToNurbsCurve();
			pointAtEnd2 = val11.PointAtStart;
			((Vector3d)(ref val13))._002Ector(val11.PointAtEnd - val11.PointAtStart);
			((Vector3d)(ref val13)).Unitize();
			pointAtEnd2 += val13 * cornerBlend;
			val11.ClosestPoint(pointAtEnd2, ref num8);
			Curve obj2 = val11;
			double num9 = num8;
			domain = val11.Domain;
			val11 = (Curve)(object)obj2.Trim(new Interval(num9, ((Interval)(ref domain)).Max)).ToNurbsCurve();
			Curve curve = (Curve)(object)val9;
			Curve curve2 = val11;
			BlendCorner(ref curve, ref curve2);
			val9 = curve.ToNurbsCurve();
			val11 = (Curve)(object)curve2.ToNurbsCurve();
		}
		return (Curve[])(object)new Curve[4]
		{
			(Curve)val4,
			(Curve)val9,
			val11,
			val12
		};
	}

	private static Curve[] GemOffsetCurvesRound(CSGemData gem, double distance)
	{
		//IL_0064: Unknown result type (might be due to invalid IL or missing references)
		//IL_0069: Unknown result type (might be due to invalid IL or missing references)
		//IL_006e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0073: Unknown result type (might be due to invalid IL or missing references)
		//IL_0074: Unknown result type (might be due to invalid IL or missing references)
		//IL_0094: Unknown result type (might be due to invalid IL or missing references)
		//IL_0099: Unknown result type (might be due to invalid IL or missing references)
		//IL_009a: Unknown result type (might be due to invalid IL or missing references)
		//IL_00aa: Unknown result type (might be due to invalid IL or missing references)
		//IL_00af: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b3: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d1: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d3: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d4: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d9: Unknown result type (might be due to invalid IL or missing references)
		//IL_00da: Unknown result type (might be due to invalid IL or missing references)
		double num = gem.GemXSize / 2.0;
		double num2 = gem.GemYSize / 2.0;
		num += distance;
		num2 += distance;
		if (num < 0.04)
		{
			num = 0.04;
		}
		if (num2 < 0.04)
		{
			num2 = 0.04;
		}
		Transform val = Transform.Rotation(CSBlockUtil.CSUtilMath.ToRadians(90.0), Vector3d.ZAxis, Point3d.Origin);
		Transform val2 = Transform.Scale(Plane.WorldXY, -1.0, 1.0, 1.0);
		Transform val3 = Transform.Scale(Plane.WorldXY, num, num2, 0.0);
		Circle val4 = default(Circle);
		((Circle)(ref val4))._002Ector(Point3d.Origin, 1.0);
		NurbsCurve val5 = ((Circle)(ref val4)).ToNurbsCurve();
		((GeometryBase)val5).Transform(val3 * val2 * val);
		return (Curve[])(object)new Curve[1] { (Curve)val5 };
	}

	private static Curve[] GemOffsetCurvesSquare(CSGemData gem, double distance, double cornerBlend)
	{
		//IL_00bc: Unknown result type (might be due to invalid IL or missing references)
		//IL_00bd: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d2: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d7: Unknown result type (might be due to invalid IL or missing references)
		//IL_00dc: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e3: Expected O, but got Unknown
		//IL_00e3: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f8: Unknown result type (might be due to invalid IL or missing references)
		//IL_00fd: Unknown result type (might be due to invalid IL or missing references)
		//IL_0102: Unknown result type (might be due to invalid IL or missing references)
		//IL_0117: Unknown result type (might be due to invalid IL or missing references)
		//IL_011c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0121: Unknown result type (might be due to invalid IL or missing references)
		//IL_0128: Expected O, but got Unknown
		//IL_0128: Unknown result type (might be due to invalid IL or missing references)
		//IL_013e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0143: Unknown result type (might be due to invalid IL or missing references)
		//IL_0148: Unknown result type (might be due to invalid IL or missing references)
		//IL_015d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0162: Unknown result type (might be due to invalid IL or missing references)
		//IL_0167: Unknown result type (might be due to invalid IL or missing references)
		//IL_016e: Expected O, but got Unknown
		//IL_016e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0183: Unknown result type (might be due to invalid IL or missing references)
		//IL_0188: Unknown result type (might be due to invalid IL or missing references)
		//IL_018d: Unknown result type (might be due to invalid IL or missing references)
		//IL_01a3: Unknown result type (might be due to invalid IL or missing references)
		//IL_01a8: Unknown result type (might be due to invalid IL or missing references)
		//IL_01ad: Unknown result type (might be due to invalid IL or missing references)
		//IL_01b4: Expected O, but got Unknown
		//IL_01b4: Unknown result type (might be due to invalid IL or missing references)
		//IL_01c9: Unknown result type (might be due to invalid IL or missing references)
		//IL_01ce: Unknown result type (might be due to invalid IL or missing references)
		//IL_01d3: Unknown result type (might be due to invalid IL or missing references)
		//IL_01d4: Unknown result type (might be due to invalid IL or missing references)
		//IL_01db: Expected O, but got Unknown
		double num = gem.GemXSize / 2.0;
		double num2 = gem.GemYSize / 2.0;
		num += distance;
		num2 += distance;
		if (num < 0.04)
		{
			num = 0.04;
		}
		if (num2 < 0.04)
		{
			num2 = 0.04;
		}
		Point3d val = default(Point3d);
		((Point3d)(ref val))._002Ector(0.0, num2, 0.0);
		Point3d val2 = default(Point3d);
		((Point3d)(ref val2))._002Ector(num, num2, 0.0);
		Point3d val3 = default(Point3d);
		((Point3d)(ref val3))._002Ector(num, 0.0 - num2, 0.0);
		Point3d val4 = default(Point3d);
		((Point3d)(ref val4))._002Ector(0.0 - num, 0.0 - num2, 0.0);
		Point3d val5 = default(Point3d);
		((Point3d)(ref val5))._002Ector(0.0 - num, num2, 0.0);
		Curve curve = (Curve)new LineCurve(val, val2 + new Vector3d(0.0 - cornerBlend, 0.0, 0.0));
		Curve curve2 = (Curve)new LineCurve(val2 + new Vector3d(0.0, 0.0 - cornerBlend, 0.0), val3 + new Vector3d(0.0, cornerBlend, 0.0));
		Curve curve3 = (Curve)new LineCurve(val3 + new Vector3d(0.0 - cornerBlend, 0.0, 0.0), val4 + new Vector3d(cornerBlend, 0.0, 0.0));
		Curve curve4 = (Curve)new LineCurve(val4 + new Vector3d(0.0, cornerBlend, 0.0), val5 + new Vector3d(0.0, 0.0 - cornerBlend, 0.0));
		Curve curve5 = (Curve)new LineCurve(val5 + new Vector3d(cornerBlend, 0.0, 0.0), val);
		if (cornerBlend > 0.0)
		{
			BlendCorner(ref curve, ref curve2);
			BlendCorner(ref curve2, ref curve3);
			BlendCorner(ref curve3, ref curve4);
			BlendCorner(ref curve4, ref curve5);
		}
		return (Curve[])(object)new Curve[5] { curve, curve2, curve3, curve4, curve5 };
	}

	private static Curve[] GemOffsetCurvesSquareRotate45(CSGemData gem, double distance)
	{
		//IL_002d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0045: Unknown result type (might be due to invalid IL or missing references)
		//IL_0132: Unknown result type (might be due to invalid IL or missing references)
		//IL_0134: Unknown result type (might be due to invalid IL or missing references)
		//IL_0136: Unknown result type (might be due to invalid IL or missing references)
		//IL_013d: Expected O, but got Unknown
		//IL_013d: Unknown result type (might be due to invalid IL or missing references)
		//IL_013f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0141: Unknown result type (might be due to invalid IL or missing references)
		//IL_0148: Expected O, but got Unknown
		//IL_0148: Unknown result type (might be due to invalid IL or missing references)
		//IL_014a: Unknown result type (might be due to invalid IL or missing references)
		//IL_014c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0153: Expected O, but got Unknown
		//IL_0153: Unknown result type (might be due to invalid IL or missing references)
		//IL_0155: Unknown result type (might be due to invalid IL or missing references)
		//IL_0157: Unknown result type (might be due to invalid IL or missing references)
		//IL_015e: Expected O, but got Unknown
		double num = gem.GemXSize / 2.0;
		double num2 = gem.GemYSize / 2.0;
		Vector3d inputVector = new Vector3d(num, num2, 0.0);
		Vector3d inputVector2 = default(Vector3d);
		((Vector3d)(ref inputVector2))._002Ector(num, 0.0 - num2, 0.0);
		double num3 = CSBlockUtil.CSUtilMath.AngleBetweenVectors(inputVector, inputVector2);
		double num4 = OnUtil.On_PI - num3;
		double num5 = 1.0 / Math.Sin(num3 / 2.0);
		double num6 = 1.0 / Math.Sin(num4 / 2.0);
		num += distance * num5;
		num2 += distance * num6;
		if (num < 0.04)
		{
			num = 0.04;
		}
		if (num2 < 0.04)
		{
			num2 = 0.04;
		}
		Point3d val = default(Point3d);
		((Point3d)(ref val))._002Ector(0.0, num2, 0.0);
		Point3d val2 = default(Point3d);
		((Point3d)(ref val2))._002Ector(num, 0.0, 0.0);
		Point3d val3 = default(Point3d);
		((Point3d)(ref val3))._002Ector(0.0, 0.0 - num2, 0.0);
		Point3d val4 = default(Point3d);
		((Point3d)(ref val4))._002Ector(0.0 - num, 0.0, 0.0);
		LineCurve val5 = new LineCurve(val, val2);
		LineCurve val6 = new LineCurve(val2, val3);
		LineCurve val7 = new LineCurve(val3, val4);
		LineCurve val8 = new LineCurve(val4, val);
		return (Curve[])(object)new Curve[4]
		{
			(Curve)val5,
			(Curve)val6,
			(Curve)val7,
			(Curve)val8
		};
	}

	private static void Trim5Curves(double cornerBlend, ref Curve curve1A, ref Curve curve2, ref Curve curve3, ref Curve curve4, ref Curve curve1B)
	{
		//IL_0002: Unknown result type (might be due to invalid IL or missing references)
		//IL_0007: Unknown result type (might be due to invalid IL or missing references)
		//IL_000a: Unknown result type (might be due to invalid IL or missing references)
		//IL_000d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0012: Unknown result type (might be due to invalid IL or missing references)
		//IL_0024: Unknown result type (might be due to invalid IL or missing references)
		//IL_0026: Unknown result type (might be due to invalid IL or missing references)
		//IL_002b: Unknown result type (might be due to invalid IL or missing references)
		//IL_002c: Unknown result type (might be due to invalid IL or missing references)
		//IL_002d: Unknown result type (might be due to invalid IL or missing references)
		//IL_002e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0033: Unknown result type (might be due to invalid IL or missing references)
		//IL_0036: Unknown result type (might be due to invalid IL or missing references)
		//IL_0044: Unknown result type (might be due to invalid IL or missing references)
		//IL_0049: Unknown result type (might be due to invalid IL or missing references)
		//IL_0052: Unknown result type (might be due to invalid IL or missing references)
		//IL_005f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0064: Unknown result type (might be due to invalid IL or missing references)
		//IL_0067: Unknown result type (might be due to invalid IL or missing references)
		//IL_006a: Unknown result type (might be due to invalid IL or missing references)
		//IL_006f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0081: Unknown result type (might be due to invalid IL or missing references)
		//IL_0083: Unknown result type (might be due to invalid IL or missing references)
		//IL_0088: Unknown result type (might be due to invalid IL or missing references)
		//IL_0089: Unknown result type (might be due to invalid IL or missing references)
		//IL_008a: Unknown result type (might be due to invalid IL or missing references)
		//IL_008b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0090: Unknown result type (might be due to invalid IL or missing references)
		//IL_0093: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a2: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a7: Unknown result type (might be due to invalid IL or missing references)
		//IL_00af: Unknown result type (might be due to invalid IL or missing references)
		//IL_00bc: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c1: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c4: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c7: Unknown result type (might be due to invalid IL or missing references)
		//IL_00cc: Unknown result type (might be due to invalid IL or missing references)
		//IL_00de: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e0: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e5: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e6: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e7: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e8: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ed: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f0: Unknown result type (might be due to invalid IL or missing references)
		//IL_00fe: Unknown result type (might be due to invalid IL or missing references)
		//IL_0103: Unknown result type (might be due to invalid IL or missing references)
		//IL_010c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0119: Unknown result type (might be due to invalid IL or missing references)
		//IL_011e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0121: Unknown result type (might be due to invalid IL or missing references)
		//IL_0124: Unknown result type (might be due to invalid IL or missing references)
		//IL_0129: Unknown result type (might be due to invalid IL or missing references)
		//IL_013b: Unknown result type (might be due to invalid IL or missing references)
		//IL_013d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0142: Unknown result type (might be due to invalid IL or missing references)
		//IL_0143: Unknown result type (might be due to invalid IL or missing references)
		//IL_0144: Unknown result type (might be due to invalid IL or missing references)
		//IL_0145: Unknown result type (might be due to invalid IL or missing references)
		//IL_014a: Unknown result type (might be due to invalid IL or missing references)
		//IL_014d: Unknown result type (might be due to invalid IL or missing references)
		//IL_015c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0161: Unknown result type (might be due to invalid IL or missing references)
		//IL_0169: Unknown result type (might be due to invalid IL or missing references)
		//IL_0176: Unknown result type (might be due to invalid IL or missing references)
		//IL_017b: Unknown result type (might be due to invalid IL or missing references)
		//IL_017e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0181: Unknown result type (might be due to invalid IL or missing references)
		//IL_0186: Unknown result type (might be due to invalid IL or missing references)
		//IL_0198: Unknown result type (might be due to invalid IL or missing references)
		//IL_019a: Unknown result type (might be due to invalid IL or missing references)
		//IL_019f: Unknown result type (might be due to invalid IL or missing references)
		//IL_01a0: Unknown result type (might be due to invalid IL or missing references)
		//IL_01a1: Unknown result type (might be due to invalid IL or missing references)
		//IL_01a2: Unknown result type (might be due to invalid IL or missing references)
		//IL_01a7: Unknown result type (might be due to invalid IL or missing references)
		//IL_01aa: Unknown result type (might be due to invalid IL or missing references)
		//IL_01b8: Unknown result type (might be due to invalid IL or missing references)
		//IL_01bd: Unknown result type (might be due to invalid IL or missing references)
		//IL_01c6: Unknown result type (might be due to invalid IL or missing references)
		//IL_01d4: Unknown result type (might be due to invalid IL or missing references)
		//IL_01d9: Unknown result type (might be due to invalid IL or missing references)
		//IL_01dc: Unknown result type (might be due to invalid IL or missing references)
		//IL_01e0: Unknown result type (might be due to invalid IL or missing references)
		//IL_01e5: Unknown result type (might be due to invalid IL or missing references)
		//IL_01f7: Unknown result type (might be due to invalid IL or missing references)
		//IL_01f9: Unknown result type (might be due to invalid IL or missing references)
		//IL_01fe: Unknown result type (might be due to invalid IL or missing references)
		//IL_01ff: Unknown result type (might be due to invalid IL or missing references)
		//IL_0200: Unknown result type (might be due to invalid IL or missing references)
		//IL_0201: Unknown result type (might be due to invalid IL or missing references)
		//IL_0206: Unknown result type (might be due to invalid IL or missing references)
		//IL_020a: Unknown result type (might be due to invalid IL or missing references)
		//IL_021c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0221: Unknown result type (might be due to invalid IL or missing references)
		//IL_0229: Unknown result type (might be due to invalid IL or missing references)
		//IL_0237: Unknown result type (might be due to invalid IL or missing references)
		//IL_023c: Unknown result type (might be due to invalid IL or missing references)
		//IL_023f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0243: Unknown result type (might be due to invalid IL or missing references)
		//IL_0248: Unknown result type (might be due to invalid IL or missing references)
		//IL_025a: Unknown result type (might be due to invalid IL or missing references)
		//IL_025c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0261: Unknown result type (might be due to invalid IL or missing references)
		//IL_0262: Unknown result type (might be due to invalid IL or missing references)
		//IL_0263: Unknown result type (might be due to invalid IL or missing references)
		//IL_0264: Unknown result type (might be due to invalid IL or missing references)
		//IL_0269: Unknown result type (might be due to invalid IL or missing references)
		//IL_026d: Unknown result type (might be due to invalid IL or missing references)
		//IL_027e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0283: Unknown result type (might be due to invalid IL or missing references)
		//IL_028c: Unknown result type (might be due to invalid IL or missing references)
		//IL_029a: Unknown result type (might be due to invalid IL or missing references)
		//IL_029f: Unknown result type (might be due to invalid IL or missing references)
		//IL_02a2: Unknown result type (might be due to invalid IL or missing references)
		//IL_02a6: Unknown result type (might be due to invalid IL or missing references)
		//IL_02ab: Unknown result type (might be due to invalid IL or missing references)
		//IL_02bd: Unknown result type (might be due to invalid IL or missing references)
		//IL_02bf: Unknown result type (might be due to invalid IL or missing references)
		//IL_02c4: Unknown result type (might be due to invalid IL or missing references)
		//IL_02c5: Unknown result type (might be due to invalid IL or missing references)
		//IL_02c6: Unknown result type (might be due to invalid IL or missing references)
		//IL_02c7: Unknown result type (might be due to invalid IL or missing references)
		//IL_02cc: Unknown result type (might be due to invalid IL or missing references)
		//IL_02d0: Unknown result type (might be due to invalid IL or missing references)
		//IL_02e2: Unknown result type (might be due to invalid IL or missing references)
		//IL_02e7: Unknown result type (might be due to invalid IL or missing references)
		//IL_02ef: Unknown result type (might be due to invalid IL or missing references)
		Point3d pointAtEnd = curve1A.PointAtEnd;
		Vector3d val = default(Vector3d);
		((Vector3d)(ref val))._002Ector(pointAtEnd - curve1A.PointAtStart);
		((Vector3d)(ref val)).Unitize();
		val *= cornerBlend;
		pointAtEnd -= val;
		double num = default(double);
		curve1A.ClosestPoint(pointAtEnd, ref num);
		Curve obj = curve1A;
		Interval domain = curve1A.Domain;
		curve1A = obj.Trim(new Interval(((Interval)(ref domain)).Min, num));
		pointAtEnd = curve2.PointAtStart;
		((Vector3d)(ref val))._002Ector(pointAtEnd - curve2.PointAtEnd);
		((Vector3d)(ref val)).Unitize();
		val *= cornerBlend;
		pointAtEnd -= val;
		curve2.ClosestPoint(pointAtEnd, ref num);
		Curve obj2 = curve2;
		double num2 = num;
		domain = curve2.Domain;
		curve2 = obj2.Trim(new Interval(num2, ((Interval)(ref domain)).Max));
		pointAtEnd = curve2.PointAtEnd;
		((Vector3d)(ref val))._002Ector(pointAtEnd - curve2.PointAtStart);
		((Vector3d)(ref val)).Unitize();
		val *= cornerBlend;
		pointAtEnd -= val;
		curve2.ClosestPoint(pointAtEnd, ref num);
		Curve obj3 = curve2;
		domain = curve2.Domain;
		curve2 = obj3.Trim(new Interval(((Interval)(ref domain)).Min, num));
		pointAtEnd = curve3.PointAtStart;
		((Vector3d)(ref val))._002Ector(pointAtEnd - curve3.PointAtEnd);
		((Vector3d)(ref val)).Unitize();
		val *= cornerBlend;
		pointAtEnd -= val;
		curve3.ClosestPoint(pointAtEnd, ref num);
		Curve obj4 = curve3;
		double num3 = num;
		domain = curve3.Domain;
		curve3 = obj4.Trim(new Interval(num3, ((Interval)(ref domain)).Max));
		pointAtEnd = curve3.PointAtEnd;
		((Vector3d)(ref val))._002Ector(pointAtEnd - curve3.PointAtStart);
		((Vector3d)(ref val)).Unitize();
		val *= cornerBlend;
		pointAtEnd -= val;
		curve3.ClosestPoint(pointAtEnd, ref num);
		Curve obj5 = curve3;
		domain = curve3.Domain;
		curve3 = obj5.Trim(new Interval(((Interval)(ref domain)).Min, num));
		pointAtEnd = curve4.PointAtStart;
		((Vector3d)(ref val))._002Ector(pointAtEnd - curve4.PointAtEnd);
		((Vector3d)(ref val)).Unitize();
		val *= cornerBlend;
		pointAtEnd -= val;
		curve4.ClosestPoint(pointAtEnd, ref num);
		Curve obj6 = curve4;
		double num4 = num;
		domain = curve4.Domain;
		curve4 = obj6.Trim(new Interval(num4, ((Interval)(ref domain)).Max));
		pointAtEnd = curve4.PointAtEnd;
		((Vector3d)(ref val))._002Ector(pointAtEnd - curve4.PointAtStart);
		((Vector3d)(ref val)).Unitize();
		val *= cornerBlend;
		pointAtEnd -= val;
		curve4.ClosestPoint(pointAtEnd, ref num);
		Curve obj7 = curve4;
		domain = curve4.Domain;
		curve4 = obj7.Trim(new Interval(((Interval)(ref domain)).Min, num));
		pointAtEnd = curve1B.PointAtStart;
		((Vector3d)(ref val))._002Ector(pointAtEnd - curve1B.PointAtEnd);
		((Vector3d)(ref val)).Unitize();
		val *= cornerBlend;
		pointAtEnd -= val;
		curve1B.ClosestPoint(pointAtEnd, ref num);
		Curve obj8 = curve1B;
		double num5 = num;
		domain = curve1B.Domain;
		curve1B = obj8.Trim(new Interval(num5, ((Interval)(ref domain)).Max));
	}

	private static void Trim9Curves(double cornerBlend, ref Curve curve0, ref Curve curve1, ref Curve curve2, ref Curve curve3, ref Curve curve4, ref Curve curve5, ref Curve curve6, ref Curve curve7, ref Curve curve8)
	{
		//IL_004c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0051: Unknown result type (might be due to invalid IL or missing references)
		//IL_0057: Unknown result type (might be due to invalid IL or missing references)
		//IL_005e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0063: Unknown result type (might be due to invalid IL or missing references)
		//IL_0075: Unknown result type (might be due to invalid IL or missing references)
		//IL_0077: Unknown result type (might be due to invalid IL or missing references)
		//IL_007a: Unknown result type (might be due to invalid IL or missing references)
		//IL_007f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0084: Unknown result type (might be due to invalid IL or missing references)
		//IL_0088: Unknown result type (might be due to invalid IL or missing references)
		//IL_0097: Unknown result type (might be due to invalid IL or missing references)
		//IL_009c: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a7: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b4: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b9: Unknown result type (might be due to invalid IL or missing references)
		//IL_00bf: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c6: Unknown result type (might be due to invalid IL or missing references)
		//IL_00cb: Unknown result type (might be due to invalid IL or missing references)
		//IL_00fb: Unknown result type (might be due to invalid IL or missing references)
		//IL_00fd: Unknown result type (might be due to invalid IL or missing references)
		//IL_0101: Unknown result type (might be due to invalid IL or missing references)
		//IL_0106: Unknown result type (might be due to invalid IL or missing references)
		//IL_010b: Unknown result type (might be due to invalid IL or missing references)
		//IL_010f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0120: Unknown result type (might be due to invalid IL or missing references)
		//IL_0125: Unknown result type (might be due to invalid IL or missing references)
		//IL_012e: Unknown result type (might be due to invalid IL or missing references)
		//IL_013b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0140: Unknown result type (might be due to invalid IL or missing references)
		//IL_0146: Unknown result type (might be due to invalid IL or missing references)
		//IL_014d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0152: Unknown result type (might be due to invalid IL or missing references)
		//IL_0182: Unknown result type (might be due to invalid IL or missing references)
		//IL_0184: Unknown result type (might be due to invalid IL or missing references)
		//IL_0188: Unknown result type (might be due to invalid IL or missing references)
		//IL_018d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0192: Unknown result type (might be due to invalid IL or missing references)
		//IL_0196: Unknown result type (might be due to invalid IL or missing references)
		//IL_01a5: Unknown result type (might be due to invalid IL or missing references)
		//IL_01aa: Unknown result type (might be due to invalid IL or missing references)
		//IL_01b5: Unknown result type (might be due to invalid IL or missing references)
		//IL_01c2: Unknown result type (might be due to invalid IL or missing references)
		//IL_01c7: Unknown result type (might be due to invalid IL or missing references)
		//IL_01cd: Unknown result type (might be due to invalid IL or missing references)
		//IL_01d4: Unknown result type (might be due to invalid IL or missing references)
		//IL_01d9: Unknown result type (might be due to invalid IL or missing references)
		//IL_0209: Unknown result type (might be due to invalid IL or missing references)
		//IL_020b: Unknown result type (might be due to invalid IL or missing references)
		//IL_020f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0214: Unknown result type (might be due to invalid IL or missing references)
		//IL_0219: Unknown result type (might be due to invalid IL or missing references)
		//IL_021d: Unknown result type (might be due to invalid IL or missing references)
		//IL_022e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0233: Unknown result type (might be due to invalid IL or missing references)
		//IL_023c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0249: Unknown result type (might be due to invalid IL or missing references)
		//IL_024e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0254: Unknown result type (might be due to invalid IL or missing references)
		//IL_025b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0260: Unknown result type (might be due to invalid IL or missing references)
		//IL_0290: Unknown result type (might be due to invalid IL or missing references)
		//IL_0292: Unknown result type (might be due to invalid IL or missing references)
		//IL_0296: Unknown result type (might be due to invalid IL or missing references)
		//IL_029b: Unknown result type (might be due to invalid IL or missing references)
		//IL_02a0: Unknown result type (might be due to invalid IL or missing references)
		//IL_02a4: Unknown result type (might be due to invalid IL or missing references)
		//IL_02b3: Unknown result type (might be due to invalid IL or missing references)
		//IL_02b8: Unknown result type (might be due to invalid IL or missing references)
		//IL_02c3: Unknown result type (might be due to invalid IL or missing references)
		//IL_02d1: Unknown result type (might be due to invalid IL or missing references)
		//IL_02d6: Unknown result type (might be due to invalid IL or missing references)
		//IL_02dd: Unknown result type (might be due to invalid IL or missing references)
		//IL_02e5: Unknown result type (might be due to invalid IL or missing references)
		//IL_02ea: Unknown result type (might be due to invalid IL or missing references)
		//IL_031a: Unknown result type (might be due to invalid IL or missing references)
		//IL_031c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0320: Unknown result type (might be due to invalid IL or missing references)
		//IL_0325: Unknown result type (might be due to invalid IL or missing references)
		//IL_032a: Unknown result type (might be due to invalid IL or missing references)
		//IL_032f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0343: Unknown result type (might be due to invalid IL or missing references)
		//IL_0348: Unknown result type (might be due to invalid IL or missing references)
		//IL_0351: Unknown result type (might be due to invalid IL or missing references)
		//IL_035f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0364: Unknown result type (might be due to invalid IL or missing references)
		//IL_036b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0373: Unknown result type (might be due to invalid IL or missing references)
		//IL_0378: Unknown result type (might be due to invalid IL or missing references)
		//IL_03a8: Unknown result type (might be due to invalid IL or missing references)
		//IL_03aa: Unknown result type (might be due to invalid IL or missing references)
		//IL_03ae: Unknown result type (might be due to invalid IL or missing references)
		//IL_03b3: Unknown result type (might be due to invalid IL or missing references)
		//IL_03b8: Unknown result type (might be due to invalid IL or missing references)
		//IL_03bd: Unknown result type (might be due to invalid IL or missing references)
		//IL_03cf: Unknown result type (might be due to invalid IL or missing references)
		//IL_03d4: Unknown result type (might be due to invalid IL or missing references)
		//IL_03df: Unknown result type (might be due to invalid IL or missing references)
		//IL_03ed: Unknown result type (might be due to invalid IL or missing references)
		//IL_03f2: Unknown result type (might be due to invalid IL or missing references)
		//IL_03f9: Unknown result type (might be due to invalid IL or missing references)
		//IL_0401: Unknown result type (might be due to invalid IL or missing references)
		//IL_0406: Unknown result type (might be due to invalid IL or missing references)
		//IL_0436: Unknown result type (might be due to invalid IL or missing references)
		//IL_0438: Unknown result type (might be due to invalid IL or missing references)
		//IL_043c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0441: Unknown result type (might be due to invalid IL or missing references)
		//IL_0446: Unknown result type (might be due to invalid IL or missing references)
		//IL_044b: Unknown result type (might be due to invalid IL or missing references)
		//IL_045f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0464: Unknown result type (might be due to invalid IL or missing references)
		//IL_046d: Unknown result type (might be due to invalid IL or missing references)
		//IL_047b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0480: Unknown result type (might be due to invalid IL or missing references)
		//IL_0487: Unknown result type (might be due to invalid IL or missing references)
		//IL_048f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0494: Unknown result type (might be due to invalid IL or missing references)
		//IL_04c4: Unknown result type (might be due to invalid IL or missing references)
		//IL_04c6: Unknown result type (might be due to invalid IL or missing references)
		//IL_04ca: Unknown result type (might be due to invalid IL or missing references)
		//IL_04cf: Unknown result type (might be due to invalid IL or missing references)
		//IL_04d4: Unknown result type (might be due to invalid IL or missing references)
		//IL_04d9: Unknown result type (might be due to invalid IL or missing references)
		//IL_04eb: Unknown result type (might be due to invalid IL or missing references)
		//IL_04f0: Unknown result type (might be due to invalid IL or missing references)
		//IL_04fb: Unknown result type (might be due to invalid IL or missing references)
		//IL_0509: Unknown result type (might be due to invalid IL or missing references)
		//IL_050e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0515: Unknown result type (might be due to invalid IL or missing references)
		//IL_051d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0522: Unknown result type (might be due to invalid IL or missing references)
		//IL_0554: Unknown result type (might be due to invalid IL or missing references)
		//IL_0556: Unknown result type (might be due to invalid IL or missing references)
		//IL_055a: Unknown result type (might be due to invalid IL or missing references)
		//IL_055f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0564: Unknown result type (might be due to invalid IL or missing references)
		//IL_0569: Unknown result type (might be due to invalid IL or missing references)
		//IL_057d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0582: Unknown result type (might be due to invalid IL or missing references)
		//IL_058b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0599: Unknown result type (might be due to invalid IL or missing references)
		//IL_059e: Unknown result type (might be due to invalid IL or missing references)
		//IL_05a5: Unknown result type (might be due to invalid IL or missing references)
		//IL_05ad: Unknown result type (might be due to invalid IL or missing references)
		//IL_05b2: Unknown result type (might be due to invalid IL or missing references)
		//IL_05e4: Unknown result type (might be due to invalid IL or missing references)
		//IL_05e6: Unknown result type (might be due to invalid IL or missing references)
		//IL_05ea: Unknown result type (might be due to invalid IL or missing references)
		//IL_05ef: Unknown result type (might be due to invalid IL or missing references)
		//IL_05f4: Unknown result type (might be due to invalid IL or missing references)
		//IL_05f9: Unknown result type (might be due to invalid IL or missing references)
		//IL_060b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0610: Unknown result type (might be due to invalid IL or missing references)
		//IL_061b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0629: Unknown result type (might be due to invalid IL or missing references)
		//IL_062e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0635: Unknown result type (might be due to invalid IL or missing references)
		//IL_063d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0642: Unknown result type (might be due to invalid IL or missing references)
		//IL_0674: Unknown result type (might be due to invalid IL or missing references)
		//IL_0676: Unknown result type (might be due to invalid IL or missing references)
		//IL_067a: Unknown result type (might be due to invalid IL or missing references)
		//IL_067f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0684: Unknown result type (might be due to invalid IL or missing references)
		//IL_0689: Unknown result type (might be due to invalid IL or missing references)
		//IL_069d: Unknown result type (might be due to invalid IL or missing references)
		//IL_06a2: Unknown result type (might be due to invalid IL or missing references)
		//IL_06ab: Unknown result type (might be due to invalid IL or missing references)
		//IL_06b9: Unknown result type (might be due to invalid IL or missing references)
		//IL_06be: Unknown result type (might be due to invalid IL or missing references)
		//IL_06c5: Unknown result type (might be due to invalid IL or missing references)
		//IL_06cd: Unknown result type (might be due to invalid IL or missing references)
		//IL_06d2: Unknown result type (might be due to invalid IL or missing references)
		//IL_0704: Unknown result type (might be due to invalid IL or missing references)
		//IL_0706: Unknown result type (might be due to invalid IL or missing references)
		//IL_070a: Unknown result type (might be due to invalid IL or missing references)
		//IL_070f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0714: Unknown result type (might be due to invalid IL or missing references)
		//IL_0719: Unknown result type (might be due to invalid IL or missing references)
		//IL_072b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0730: Unknown result type (might be due to invalid IL or missing references)
		//IL_073b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0749: Unknown result type (might be due to invalid IL or missing references)
		//IL_074e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0755: Unknown result type (might be due to invalid IL or missing references)
		//IL_075d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0762: Unknown result type (might be due to invalid IL or missing references)
		//IL_0794: Unknown result type (might be due to invalid IL or missing references)
		//IL_0796: Unknown result type (might be due to invalid IL or missing references)
		//IL_079a: Unknown result type (might be due to invalid IL or missing references)
		//IL_079f: Unknown result type (might be due to invalid IL or missing references)
		//IL_07a4: Unknown result type (might be due to invalid IL or missing references)
		//IL_07a9: Unknown result type (might be due to invalid IL or missing references)
		//IL_07bd: Unknown result type (might be due to invalid IL or missing references)
		//IL_07c2: Unknown result type (might be due to invalid IL or missing references)
		//IL_07cb: Unknown result type (might be due to invalid IL or missing references)
		//IL_07d9: Unknown result type (might be due to invalid IL or missing references)
		//IL_07de: Unknown result type (might be due to invalid IL or missing references)
		//IL_07e5: Unknown result type (might be due to invalid IL or missing references)
		//IL_07ed: Unknown result type (might be due to invalid IL or missing references)
		//IL_07f2: Unknown result type (might be due to invalid IL or missing references)
		//IL_0824: Unknown result type (might be due to invalid IL or missing references)
		//IL_0826: Unknown result type (might be due to invalid IL or missing references)
		//IL_082a: Unknown result type (might be due to invalid IL or missing references)
		//IL_082f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0834: Unknown result type (might be due to invalid IL or missing references)
		//IL_0839: Unknown result type (might be due to invalid IL or missing references)
		//IL_084b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0850: Unknown result type (might be due to invalid IL or missing references)
		//IL_085b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0869: Unknown result type (might be due to invalid IL or missing references)
		//IL_086e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0875: Unknown result type (might be due to invalid IL or missing references)
		//IL_087d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0882: Unknown result type (might be due to invalid IL or missing references)
		//IL_08b4: Unknown result type (might be due to invalid IL or missing references)
		//IL_08b6: Unknown result type (might be due to invalid IL or missing references)
		//IL_08ba: Unknown result type (might be due to invalid IL or missing references)
		//IL_08bf: Unknown result type (might be due to invalid IL or missing references)
		//IL_08c4: Unknown result type (might be due to invalid IL or missing references)
		//IL_08c9: Unknown result type (might be due to invalid IL or missing references)
		//IL_08dd: Unknown result type (might be due to invalid IL or missing references)
		//IL_08e2: Unknown result type (might be due to invalid IL or missing references)
		//IL_08eb: Unknown result type (might be due to invalid IL or missing references)
		double length = curve1.GetLength();
		double length2 = curve2.GetLength();
		double length3 = curve3.GetLength();
		double length4 = curve4.GetLength();
		double length5 = curve5.GetLength();
		double length6 = curve6.GetLength();
		double length7 = curve7.GetLength();
		double length8 = curve8.GetLength();
		Point3d pointAtEnd = curve0.PointAtEnd;
		Vector3d val = default(Vector3d);
		((Vector3d)(ref val))._002Ector(curve0.PointAtEnd - curve0.PointAtStart);
		((Vector3d)(ref val)).Unitize();
		pointAtEnd -= val * cornerBlend;
		double num = default(double);
		curve0.ClosestPoint(pointAtEnd, ref num);
		Curve obj = curve0;
		Interval domain = curve0.Domain;
		curve0 = obj.Trim(new Interval(((Interval)(ref domain)).Min, num));
		pointAtEnd = curve1.PointAtStart;
		((Vector3d)(ref val))._002Ector(curve1.PointAtStart - curve1.PointAtEnd);
		((Vector3d)(ref val)).Unitize();
		double num2 = cornerBlend;
		if (cornerBlend * 2.0 > length)
		{
			num2 = length * 0.49;
		}
		pointAtEnd -= val * num2;
		curve1.ClosestPoint(pointAtEnd, ref num);
		Curve obj2 = curve1;
		double num3 = num;
		domain = curve1.Domain;
		curve1 = obj2.Trim(new Interval(num3, ((Interval)(ref domain)).Max));
		pointAtEnd = curve1.PointAtEnd;
		((Vector3d)(ref val))._002Ector(curve1.PointAtEnd - curve1.PointAtStart);
		((Vector3d)(ref val)).Unitize();
		num2 = cornerBlend;
		if (cornerBlend * 2.0 > length)
		{
			num2 = length * 0.49;
		}
		pointAtEnd -= val * num2;
		curve1.ClosestPoint(pointAtEnd, ref num);
		Curve obj3 = curve1;
		domain = curve1.Domain;
		curve1 = obj3.Trim(new Interval(((Interval)(ref domain)).Min, num));
		pointAtEnd = curve2.PointAtStart;
		((Vector3d)(ref val))._002Ector(curve2.PointAtStart - curve2.PointAtEnd);
		((Vector3d)(ref val)).Unitize();
		num2 = cornerBlend;
		if (cornerBlend * 2.0 > length2)
		{
			num2 = length2 * 0.49;
		}
		pointAtEnd -= val * num2;
		curve2.ClosestPoint(pointAtEnd, ref num);
		Curve obj4 = curve2;
		double num4 = num;
		domain = curve2.Domain;
		curve2 = obj4.Trim(new Interval(num4, ((Interval)(ref domain)).Max));
		pointAtEnd = curve2.PointAtEnd;
		((Vector3d)(ref val))._002Ector(curve2.PointAtEnd - curve2.PointAtStart);
		((Vector3d)(ref val)).Unitize();
		num2 = cornerBlend;
		if (cornerBlend * 2.0 > length2)
		{
			num2 = length2 * 0.49;
		}
		pointAtEnd -= val * num2;
		curve2.ClosestPoint(pointAtEnd, ref num);
		Curve obj5 = curve2;
		domain = curve2.Domain;
		curve2 = obj5.Trim(new Interval(((Interval)(ref domain)).Min, num));
		pointAtEnd = curve3.PointAtStart;
		((Vector3d)(ref val))._002Ector(curve3.PointAtStart - curve3.PointAtEnd);
		((Vector3d)(ref val)).Unitize();
		num2 = cornerBlend;
		if (cornerBlend * 2.0 > length3)
		{
			num2 = length3 * 0.49;
		}
		pointAtEnd -= val * num2;
		curve3.ClosestPoint(pointAtEnd, ref num);
		Curve obj6 = curve3;
		double num5 = num;
		domain = curve3.Domain;
		curve3 = obj6.Trim(new Interval(num5, ((Interval)(ref domain)).Max));
		pointAtEnd = curve3.PointAtEnd;
		((Vector3d)(ref val))._002Ector(curve3.PointAtEnd - curve3.PointAtStart);
		((Vector3d)(ref val)).Unitize();
		num2 = cornerBlend;
		if (cornerBlend * 2.0 > length3)
		{
			num2 = length3 * 0.49;
		}
		pointAtEnd -= val * num2;
		curve3.ClosestPoint(pointAtEnd, ref num);
		Curve obj7 = curve3;
		domain = curve3.Domain;
		curve3 = obj7.Trim(new Interval(((Interval)(ref domain)).Min, num));
		pointAtEnd = curve4.PointAtStart;
		((Vector3d)(ref val))._002Ector(curve4.PointAtStart - curve4.PointAtEnd);
		((Vector3d)(ref val)).Unitize();
		num2 = cornerBlend;
		if (cornerBlend * 2.0 > length4)
		{
			num2 = length4 * 0.49;
		}
		pointAtEnd -= val * num2;
		curve4.ClosestPoint(pointAtEnd, ref num);
		Curve obj8 = curve4;
		double num6 = num;
		domain = curve4.Domain;
		curve4 = obj8.Trim(new Interval(num6, ((Interval)(ref domain)).Max));
		pointAtEnd = curve4.PointAtEnd;
		((Vector3d)(ref val))._002Ector(curve4.PointAtEnd - curve4.PointAtStart);
		((Vector3d)(ref val)).Unitize();
		num2 = cornerBlend;
		if (cornerBlend * 2.0 > length4)
		{
			num2 = length4 * 0.49;
		}
		pointAtEnd -= val * num2;
		curve4.ClosestPoint(pointAtEnd, ref num);
		Curve obj9 = curve4;
		domain = curve4.Domain;
		curve4 = obj9.Trim(new Interval(((Interval)(ref domain)).Min, num));
		pointAtEnd = curve5.PointAtStart;
		((Vector3d)(ref val))._002Ector(curve5.PointAtStart - curve5.PointAtEnd);
		((Vector3d)(ref val)).Unitize();
		num2 = cornerBlend;
		if (cornerBlend * 2.0 > length5)
		{
			num2 = length5 * 0.49;
		}
		pointAtEnd -= val * num2;
		curve5.ClosestPoint(pointAtEnd, ref num);
		Curve obj10 = curve5;
		double num7 = num;
		domain = curve5.Domain;
		curve5 = obj10.Trim(new Interval(num7, ((Interval)(ref domain)).Max));
		pointAtEnd = curve5.PointAtEnd;
		((Vector3d)(ref val))._002Ector(curve5.PointAtEnd - curve5.PointAtStart);
		((Vector3d)(ref val)).Unitize();
		num2 = cornerBlend;
		if (cornerBlend * 2.0 > length5)
		{
			num2 = length5 * 0.49;
		}
		pointAtEnd -= val * num2;
		curve5.ClosestPoint(pointAtEnd, ref num);
		Curve obj11 = curve5;
		domain = curve5.Domain;
		curve5 = obj11.Trim(new Interval(((Interval)(ref domain)).Min, num));
		pointAtEnd = curve6.PointAtStart;
		((Vector3d)(ref val))._002Ector(curve6.PointAtStart - curve6.PointAtEnd);
		((Vector3d)(ref val)).Unitize();
		num2 = cornerBlend;
		if (cornerBlend * 2.0 > length6)
		{
			num2 = length6 * 0.49;
		}
		pointAtEnd -= val * num2;
		curve6.ClosestPoint(pointAtEnd, ref num);
		Curve obj12 = curve6;
		double num8 = num;
		domain = curve6.Domain;
		curve6 = obj12.Trim(new Interval(num8, ((Interval)(ref domain)).Max));
		pointAtEnd = curve6.PointAtEnd;
		((Vector3d)(ref val))._002Ector(curve6.PointAtEnd - curve6.PointAtStart);
		((Vector3d)(ref val)).Unitize();
		num2 = cornerBlend;
		if (cornerBlend * 2.0 > length6)
		{
			num2 = length6 * 0.49;
		}
		pointAtEnd -= val * num2;
		curve6.ClosestPoint(pointAtEnd, ref num);
		Curve obj13 = curve6;
		domain = curve6.Domain;
		curve6 = obj13.Trim(new Interval(((Interval)(ref domain)).Min, num));
		pointAtEnd = curve7.PointAtStart;
		((Vector3d)(ref val))._002Ector(curve7.PointAtStart - curve7.PointAtEnd);
		((Vector3d)(ref val)).Unitize();
		num2 = cornerBlend;
		if (cornerBlend * 2.0 > length7)
		{
			num2 = length7 * 0.49;
		}
		pointAtEnd -= val * num2;
		curve7.ClosestPoint(pointAtEnd, ref num);
		Curve obj14 = curve7;
		double num9 = num;
		domain = curve7.Domain;
		curve7 = obj14.Trim(new Interval(num9, ((Interval)(ref domain)).Max));
		pointAtEnd = curve7.PointAtEnd;
		((Vector3d)(ref val))._002Ector(curve7.PointAtEnd - curve7.PointAtStart);
		((Vector3d)(ref val)).Unitize();
		num2 = cornerBlend;
		if (cornerBlend * 2.0 > length7)
		{
			num2 = length7 * 0.49;
		}
		pointAtEnd -= val * num2;
		curve7.ClosestPoint(pointAtEnd, ref num);
		Curve obj15 = curve7;
		domain = curve7.Domain;
		curve7 = obj15.Trim(new Interval(((Interval)(ref domain)).Min, num));
		pointAtEnd = curve8.PointAtStart;
		((Vector3d)(ref val))._002Ector(curve8.PointAtEnd - curve8.PointAtStart);
		((Vector3d)(ref val)).Unitize();
		num2 = cornerBlend;
		if (cornerBlend * 2.0 > length8)
		{
			num2 = length8 * 0.49;
		}
		pointAtEnd += val * num2;
		curve8.ClosestPoint(pointAtEnd, ref num);
		Curve obj16 = curve8;
		double num10 = num;
		domain = curve8.Domain;
		curve8 = obj16.Trim(new Interval(num10, ((Interval)(ref domain)).Max));
	}
}
