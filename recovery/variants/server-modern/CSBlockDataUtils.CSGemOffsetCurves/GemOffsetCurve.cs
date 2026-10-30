using System;
using System.Collections.Generic;
using CSBlockUtil;
using CSCompatibility;
using CSDatas;
using RMA.OpenNURBS;
using Rhino.Geometry;

namespace CSBlockDataUtils.CSGemOffsetCurves;

public static class GemOffsetCurve
{
	public static double AngleBetweenVectors(Vector3d inputVector1, Vector3d inputVector2)
	{
		//IL_0002: Unknown result type (might be due to invalid IL or missing references)
		//IL_000a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0020: Unknown result type (might be due to invalid IL or missing references)
		//IL_0021: Unknown result type (might be due to invalid IL or missing references)
		Vector3d val = default(Vector3d);
		((Vector3d)(ref val))._002Ector(inputVector1);
		Vector3d val2 = default(Vector3d);
		((Vector3d)(ref val2))._002Ector(inputVector2);
		((Vector3d)(ref val)).Unitize();
		((Vector3d)(ref val2)).Unitize();
		double num = Vector3d.Multiply(val2, val);
		if (num < -1.0)
		{
			num = -1.0;
		}
		else if (num > 1.0)
		{
			num = 1.0;
		}
		return Math.Acos(num);
	}

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
		//IL_075d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0788: Unknown result type (might be due to invalid IL or missing references)
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
			array = GemOffsetCurvesBaguetteTapered(gem, distance, cornerBlend);
			break;
		case "Princess Rotated 45":
			array = GemOffsetCurvesSquareRotate45(gem, distance);
			break;
		case "Cushion Square":
			array = GemOffsetCurvesCushionSquare(gem, distance, cornerBlend);
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
			double cornerPercentX;
			double cornerPercentY;
			if (gem.VersionMajor > 3 || (gem.VersionMajor == 3 && (double)gem.VersionMinor >= 3.4))
			{
				cornerPercentX = 0.6931989360212797;
				cornerPercentY = 0.78106;
			}
			else
			{
				cornerPercentX = 0.6399272014559708;
				cornerPercentY = 0.7429;
			}
			if (optionalEmeraldCornerXPercent != 0.0 && optionalEmeraldCornerYPercent != 0.0)
			{
				cornerPercentX = optionalEmeraldCornerXPercent;
				cornerPercentY = optionalEmeraldCornerYPercent;
			}
			array = GemOffsetCurvesEmerald(gem, distance, cornerPercentX, cornerPercentY, cornerBlend);
			break;
		}
		case "Emerald Rotated 90":
		{
			double cornerPercentX3;
			double cornerPercentY3;
			if (gem.VersionMajor > 3 || (gem.VersionMajor == 3 && (double)gem.VersionMinor >= 3.4))
			{
				cornerPercentX3 = 0.78106;
				cornerPercentY3 = 0.6931989360212797;
			}
			else
			{
				cornerPercentX3 = 0.7429;
				cornerPercentY3 = 0.6399272014559708;
			}
			array = GemOffsetCurvesEmerald(gem, distance, cornerPercentX3, cornerPercentY3, cornerBlend);
			break;
		}
		case "Emerald Square":
		{
			double num2 = ((gem.VersionMajor <= 3 && (gem.VersionMajor != 3 || !((double)gem.VersionMinor >= 3.4))) ? 0.64 : 0.6985);
			array = GemOffsetCurvesEmerald(gem, distance, num2, num2, cornerBlend);
			break;
		}
		case "Emerald Square Rotated 45":
			array = GemOffsetCurvesEmerald(gem, distance, 0.21939999999999998, 0.21939999999999998, cornerBlend);
			break;
		case "Heart":
			array = GemOffsetCurvesHeart(gem, distance, cornerBlend);
			break;
		case "Marquise":
			array = GemOffsetCurvesMarquise(gem, distance, cornerBlend);
			break;
		case "Marquise Rotated 90":
			array = GemOffsetCurvesMarquiseRotated90(gem, distance);
			break;
		case "Octagon":
			array = GemOffsetCurvesEmerald(gem, distance, 0.4142, 0.4142, cornerBlend);
			break;
		case "Pear":
			array = GemOffsetCurvesPear(gem, distance, cornerBlend);
			break;
		case "Radiant":
		{
			double cornerPercentX2;
			double cornerPercentY2;
			if (gem.VersionMajor > 3 || (gem.VersionMajor == 3 && (double)gem.VersionMinor >= 3.4))
			{
				cornerPercentX2 = 0.7647058823529412;
				cornerPercentY2 = 0.8400000000000001;
			}
			else
			{
				cornerPercentX2 = 0.7691855336665687;
				cornerPercentY2 = 0.8462;
			}
			array = GemOffsetCurvesEmerald(gem, distance, cornerPercentX2, cornerPercentY2, cornerBlend);
			break;
		}
		case "Radiant Square":
		{
			double num = ((gem.VersionMajor <= 3 && (gem.VersionMajor != 3 || !((double)gem.VersionMinor >= 3.4))) ? 0.5603 : 0.5599999999999999);
			array = GemOffsetCurvesEmerald(gem, distance, num, num, cornerBlend);
			break;
		}
		case "Calf":
			array = GemOffsetCurvesCalf(gem, distance, cornerBlend);
			break;
		case "Triangle":
			array = GemOffsetCurvesTriangle(gem, distance, cornerBlend);
			break;
		case "Trillion Straight":
			array = GemOffsetCurvesTrillionStraight(gem, distance, cornerBlend);
			break;
		case "Trillion Curved":
			array = GemOffsetCurvesTrillionCurved(gem, distance, cornerBlend);
			break;
		case "Half Moon":
			array = GemOffsetCurvesHalfMoon(gem, distance, cornerBlend);
			break;
		default:
		{
			CSUtil.PrintLine($"Gem Offset Shape Used Default Offset : {shape}");
			NurbsCurve nurbsCurve = gem.GetNurbsCurve();
			((GeometryBase)nurbsCurve).Transform(xDown);
			array = (Curve[])(object)new Curve[1] { CSUtilGeometry.OriginOffsetXY((Curve)(object)nurbsCurve, distance) };
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
		if (!(Math.Abs(num) > 1.0))
		{
			return Math.Acos(num);
		}
		return 0.0;
	}

	public static bool BlendCorner(ref Curve crv0, ref Curve crv1)
	{
		//IL_002a: Unknown result type (might be due to invalid IL or missing references)
		//IL_002f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0039: Unknown result type (might be due to invalid IL or missing references)
		//IL_003e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0047: Unknown result type (might be due to invalid IL or missing references)
		//IL_0077: Unknown result type (might be due to invalid IL or missing references)
		//IL_007c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0086: Unknown result type (might be due to invalid IL or missing references)
		//IL_008b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0094: Unknown result type (might be due to invalid IL or missing references)
		NurbsCurve val = CSUtilCurves.BlendCurve(crv0, crv1);
		Interval domain = ((Curve)val).Domain;
		double min = ((Interval)(ref domain)).Min;
		domain = ((Curve)val).Domain;
		Curve val2 = ((Curve)val).Trim(new Interval(min, ((Interval)(ref domain)).Mid));
		if (!CSUtilCurves.MergeCurves((IEnumerable<Curve>)(object)new Curve[2] { crv0, val2 }, out Curve mergedCrv, 0.01))
		{
			return false;
		}
		domain = ((Curve)val).Domain;
		double mid = ((Interval)(ref domain)).Mid;
		domain = ((Curve)val).Domain;
		Curve val3 = ((Curve)val).Trim(new Interval(mid, ((Interval)(ref domain)).Max));
		if (!CSUtilCurves.MergeCurves((IEnumerable<Curve>)(object)new Curve[2] { crv1, val3 }, out Curve mergedCrv2, 0.01))
		{
			return false;
		}
		crv0 = mergedCrv;
		crv1 = mergedCrv2;
		return true;
	}

	private static Point3d cornerOffset(double offset, Point3d p0, Point3d p1, Point3d p2)
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_0002: Unknown result type (might be due to invalid IL or missing references)
		//IL_0007: Unknown result type (might be due to invalid IL or missing references)
		//IL_0008: Unknown result type (might be due to invalid IL or missing references)
		//IL_0009: Unknown result type (might be due to invalid IL or missing references)
		//IL_000a: Unknown result type (might be due to invalid IL or missing references)
		//IL_000f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0020: Unknown result type (might be due to invalid IL or missing references)
		//IL_0021: Unknown result type (might be due to invalid IL or missing references)
		//IL_0028: Unknown result type (might be due to invalid IL or missing references)
		//IL_0029: Unknown result type (might be due to invalid IL or missing references)
		//IL_002a: Unknown result type (might be due to invalid IL or missing references)
		//IL_002f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0038: Unknown result type (might be due to invalid IL or missing references)
		//IL_003a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0059: Unknown result type (might be due to invalid IL or missing references)
		//IL_005e: Unknown result type (might be due to invalid IL or missing references)
		//IL_005f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0060: Unknown result type (might be due to invalid IL or missing references)
		//IL_0061: Unknown result type (might be due to invalid IL or missing references)
		Vector3d val = p1 - p0;
		Vector3d val2 = p2 - p1;
		((Vector3d)(ref val)).Unitize();
		((Vector3d)(ref val2)).Unitize();
		double num = AngleBetweenVectors(val, val2);
		Vector3d val3 = val2 - val;
		((Vector3d)(ref val3)).Unitize();
		val3 = val3 * offset / Math.Cos(Math.PI - num / 2.0);
		return p1 + val3;
	}

	private static Curve[] GemOffsetCurvesBaguetteTapered(CSGemData gem, double distance, double cornerBlend = 0.0)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_0007: Unknown result type (might be due to invalid IL or missing references)
		//IL_000d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0012: Unknown result type (might be due to invalid IL or missing references)
		//IL_0017: Unknown result type (might be due to invalid IL or missing references)
		//IL_001c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0021: Unknown result type (might be due to invalid IL or missing references)
		//IL_0026: Unknown result type (might be due to invalid IL or missing references)
		//IL_0032: Unknown result type (might be due to invalid IL or missing references)
		//IL_0057: Unknown result type (might be due to invalid IL or missing references)
		//IL_005c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0060: Unknown result type (might be due to invalid IL or missing references)
		//IL_0065: Unknown result type (might be due to invalid IL or missing references)
		//IL_006d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0072: Unknown result type (might be due to invalid IL or missing references)
		//IL_0076: Unknown result type (might be due to invalid IL or missing references)
		//IL_007b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0084: Unknown result type (might be due to invalid IL or missing references)
		//IL_0089: Unknown result type (might be due to invalid IL or missing references)
		//IL_008d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0092: Unknown result type (might be due to invalid IL or missing references)
		//IL_009a: Unknown result type (might be due to invalid IL or missing references)
		//IL_009f: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a3: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a8: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ab: Unknown result type (might be due to invalid IL or missing references)
		//IL_00cf: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d1: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d2: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d4: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d9: Unknown result type (might be due to invalid IL or missing references)
		//IL_00dc: Unknown result type (might be due to invalid IL or missing references)
		//IL_00dd: Unknown result type (might be due to invalid IL or missing references)
		//IL_00df: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e1: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e6: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e9: Unknown result type (might be due to invalid IL or missing references)
		//IL_00eb: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ed: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ef: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f4: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f7: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f9: Unknown result type (might be due to invalid IL or missing references)
		//IL_00fb: Unknown result type (might be due to invalid IL or missing references)
		//IL_00fc: Unknown result type (might be due to invalid IL or missing references)
		//IL_0101: Unknown result type (might be due to invalid IL or missing references)
		//IL_018b: Unknown result type (might be due to invalid IL or missing references)
		//IL_018d: Unknown result type (might be due to invalid IL or missing references)
		//IL_018f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0196: Expected O, but got Unknown
		//IL_0196: Unknown result type (might be due to invalid IL or missing references)
		//IL_0198: Unknown result type (might be due to invalid IL or missing references)
		//IL_019a: Unknown result type (might be due to invalid IL or missing references)
		//IL_01a1: Expected O, but got Unknown
		//IL_01a1: Unknown result type (might be due to invalid IL or missing references)
		//IL_01a3: Unknown result type (might be due to invalid IL or missing references)
		//IL_01a5: Unknown result type (might be due to invalid IL or missing references)
		//IL_01ac: Expected O, but got Unknown
		//IL_01ac: Unknown result type (might be due to invalid IL or missing references)
		//IL_01ae: Unknown result type (might be due to invalid IL or missing references)
		//IL_01b0: Unknown result type (might be due to invalid IL or missing references)
		//IL_01b7: Expected O, but got Unknown
		//IL_01b7: Unknown result type (might be due to invalid IL or missing references)
		//IL_01b9: Unknown result type (might be due to invalid IL or missing references)
		//IL_01bb: Unknown result type (might be due to invalid IL or missing references)
		//IL_01c2: Expected O, but got Unknown
		Transform transform = CSBlockUtil.CSUtilMath.Orient3Point(gem.Center, gem.Up, gem.North, Point3d.Origin, Vector3d.ZAxis, Vector3d.YAxis);
		Transform transform2 = default(Transform);
		((Transform)(ref transform)).TryGetInverse(ref transform2);
		gem.ApplyTransform(transform);
		NurbsCurve nurbsCurve = gem.GetNurbsCurve();
		double num = gem.GemYSize / 2.0;
		ControlPoint val = nurbsCurve.Points[1];
		Point3d location = ((ControlPoint)(ref val)).Location;
		val = nurbsCurve.Points[2];
		Point3d location2 = ((ControlPoint)(ref val)).Location;
		val = nurbsCurve.Points[3];
		Point3d location3 = ((ControlPoint)(ref val)).Location;
		val = nurbsCurve.Points[4];
		Point3d location4 = ((ControlPoint)(ref val)).Location;
		gem.ApplyTransform(transform2);
		Point3d val2 = default(Point3d);
		((Point3d)(ref val2))._002Ector(0.0, num + distance, 0.0);
		Point3d val3 = cornerOffset(distance, location4, location, location2);
		Point3d val4 = cornerOffset(distance, location, location2, location3);
		Point3d val5 = cornerOffset(distance, location2, location3, location4);
		Point3d val6 = cornerOffset(distance, location3, location4, location);
		if (((Point3d)(ref val3)).X < 0.1)
		{
			((Point3d)(ref val3)).X = 0.1;
		}
		if (((Point3d)(ref val4)).X < 0.1)
		{
			((Point3d)(ref val4)).X = 0.1;
		}
		if (((Point3d)(ref val5)).X > -0.1)
		{
			((Point3d)(ref val5)).X = -0.1;
		}
		if (((Point3d)(ref val6)).X > -0.1)
		{
			((Point3d)(ref val6)).X = -0.1;
		}
		Curve val7 = (Curve)new LineCurve(val2, val3);
		Curve val8 = (Curve)new LineCurve(val3, val4);
		Curve val9 = (Curve)new LineCurve(val4, val5);
		Curve val10 = (Curve)new LineCurve(val5, val6);
		Curve val11 = (Curve)new LineCurve(val6, val2);
		if (cornerBlend > 0.0)
		{
			TrimCurve(val7, cornerBlend, (CurveEnd)2, out var crvTrimmed);
			TrimCurve(val8, cornerBlend, (CurveEnd)3, out var crvTrimmed2);
			TrimCurve(val9, cornerBlend, (CurveEnd)3, out var crvTrimmed3);
			TrimCurve(val10, cornerBlend, (CurveEnd)3, out var crvTrimmed4);
			TrimCurve(val11, cornerBlend, (CurveEnd)1, out var crvTrimmed5);
			BlendCorner(ref crvTrimmed, ref crvTrimmed2);
			BlendCorner(ref crvTrimmed2, ref crvTrimmed3);
			BlendCorner(ref crvTrimmed3, ref crvTrimmed4);
			BlendCorner(ref crvTrimmed4, ref crvTrimmed5);
			val7 = crvTrimmed;
			val8 = crvTrimmed2;
			val9 = crvTrimmed3;
			val10 = crvTrimmed4;
			val11 = crvTrimmed5;
		}
		return (Curve[])(object)new Curve[5] { val7, val8, val9, val10, val11 };
	}

	private static Curve[] GemOffsetCurvesCalf(CSGemData gem, double distance, double cornerBlend = 0.0)
	{
		//IL_011b: Unknown result type (might be due to invalid IL or missing references)
		//IL_011d: Unknown result type (might be due to invalid IL or missing references)
		//IL_011f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0124: Unknown result type (might be due to invalid IL or missing references)
		//IL_0139: Unknown result type (might be due to invalid IL or missing references)
		//IL_013d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0142: Unknown result type (might be due to invalid IL or missing references)
		//IL_0146: Unknown result type (might be due to invalid IL or missing references)
		//IL_0147: Unknown result type (might be due to invalid IL or missing references)
		//IL_0149: Unknown result type (might be due to invalid IL or missing references)
		//IL_0175: Unknown result type (might be due to invalid IL or missing references)
		//IL_0177: Unknown result type (might be due to invalid IL or missing references)
		//IL_0179: Unknown result type (might be due to invalid IL or missing references)
		//IL_017e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0193: Unknown result type (might be due to invalid IL or missing references)
		//IL_0197: Unknown result type (might be due to invalid IL or missing references)
		//IL_019c: Unknown result type (might be due to invalid IL or missing references)
		//IL_01a0: Unknown result type (might be due to invalid IL or missing references)
		//IL_01a2: Unknown result type (might be due to invalid IL or missing references)
		//IL_01a4: Unknown result type (might be due to invalid IL or missing references)
		//IL_01b9: Unknown result type (might be due to invalid IL or missing references)
		//IL_01cf: Unknown result type (might be due to invalid IL or missing references)
		//IL_01dc: Unknown result type (might be due to invalid IL or missing references)
		//IL_01de: Unknown result type (might be due to invalid IL or missing references)
		//IL_01e0: Unknown result type (might be due to invalid IL or missing references)
		//IL_01e2: Unknown result type (might be due to invalid IL or missing references)
		//IL_02d9: Unknown result type (might be due to invalid IL or missing references)
		//IL_02db: Unknown result type (might be due to invalid IL or missing references)
		//IL_02dd: Unknown result type (might be due to invalid IL or missing references)
		//IL_02e4: Expected O, but got Unknown
		//IL_02e4: Unknown result type (might be due to invalid IL or missing references)
		//IL_02e6: Unknown result type (might be due to invalid IL or missing references)
		//IL_02e8: Unknown result type (might be due to invalid IL or missing references)
		//IL_02ef: Expected O, but got Unknown
		//IL_02ef: Unknown result type (might be due to invalid IL or missing references)
		//IL_02f1: Unknown result type (might be due to invalid IL or missing references)
		//IL_02f3: Unknown result type (might be due to invalid IL or missing references)
		//IL_02fa: Expected O, but got Unknown
		//IL_02fa: Unknown result type (might be due to invalid IL or missing references)
		//IL_02fc: Unknown result type (might be due to invalid IL or missing references)
		//IL_02fe: Unknown result type (might be due to invalid IL or missing references)
		//IL_0305: Expected O, but got Unknown
		//IL_0305: Unknown result type (might be due to invalid IL or missing references)
		//IL_0307: Unknown result type (might be due to invalid IL or missing references)
		//IL_0309: Unknown result type (might be due to invalid IL or missing references)
		//IL_0310: Expected O, but got Unknown
		//IL_0310: Unknown result type (might be due to invalid IL or missing references)
		//IL_0312: Unknown result type (might be due to invalid IL or missing references)
		//IL_0314: Unknown result type (might be due to invalid IL or missing references)
		//IL_031b: Expected O, but got Unknown
		//IL_031b: Unknown result type (might be due to invalid IL or missing references)
		//IL_031d: Unknown result type (might be due to invalid IL or missing references)
		//IL_031f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0326: Expected O, but got Unknown
		//IL_02d1: Unknown result type (might be due to invalid IL or missing references)
		//IL_02d3: Unknown result type (might be due to invalid IL or missing references)
		//IL_02d5: Unknown result type (might be due to invalid IL or missing references)
		//IL_02d7: Unknown result type (might be due to invalid IL or missing references)
		double num = gem.GemXSize / 2.0;
		double num2 = gem.GemYSize / 2.0;
		Point3d val = default(Point3d);
		((Point3d)(ref val))._002Ector(num * 0.8, num2 * 0.8, 0.0);
		Point3d val2 = default(Point3d);
		((Point3d)(ref val2))._002Ector(num, num2 * 0.2, 0.0);
		Point3d val3 = default(Point3d);
		((Point3d)(ref val3))._002Ector(num * 0.3, (0.0 - num2) * 1.2, 0.0);
		Vector3d val4 = default(Vector3d);
		((Vector3d)(ref val4))._002Ector(((Point3d)(ref val)).Y - ((Point3d)(ref val2)).Y, 0.0 - (((Point3d)(ref val)).X - ((Point3d)(ref val2)).X), 0.0);
		((Vector3d)(ref val4)).Unitize();
		Vector3d val5 = default(Vector3d);
		((Vector3d)(ref val5))._002Ector(((Point3d)(ref val3)).Y - ((Point3d)(ref val2)).Y, 0.0 - (((Point3d)(ref val3)).X - ((Point3d)(ref val2)).X), 0.0);
		((Vector3d)(ref val5)).Unitize();
		Vector3d val6 = default(Vector3d);
		((Vector3d)(ref val6))._002Ector(0.0, 1.0, 0.0);
		val6 += val4;
		((Vector3d)(ref val6)).Unitize();
		double num3 = distance / ((Vector3d)(ref val6)).Y;
		val6 *= num3;
		Point3d point = default(Point3d);
		((Point3d)(ref point))._002Ector(val + val6);
		Vector3d val7 = default(Vector3d);
		((Vector3d)(ref val7))._002Ector(0.0, 1.0, 0.0);
		val7 += val5;
		((Vector3d)(ref val7)).Unitize();
		num3 = distance / ((Vector3d)(ref val7)).Y;
		val7 *= num3;
		Point3d point2 = default(Point3d);
		((Point3d)(ref point2))._002Ector(val3 - val7);
		((Vector3d)(ref val4)).Rotate(Math.PI / 2.0, Vector3d.ZAxis);
		((Vector3d)(ref val5)).Rotate(Math.PI / 2.0, Vector3d.ZAxis);
		CSBlockUtil.CSUtilMath.LineInt(out var outpoint, point, val4, point2, val5);
		Point3d val8 = default(Point3d);
		((Point3d)(ref val8))._002Ector(((Point3d)(ref point)).X, ((Point3d)(ref point)).Y, 0.0);
		Point3d val9 = default(Point3d);
		((Point3d)(ref val9))._002Ector(((Point3d)(ref outpoint)).X, ((Point3d)(ref outpoint)).Y, 0.0);
		Point3d val10 = default(Point3d);
		((Point3d)(ref val10))._002Ector(((Point3d)(ref point2)).X, ((Point3d)(ref point2)).Y, 0.0);
		Point3d val11 = default(Point3d);
		((Point3d)(ref val11))._002Ector(0.0 - ((Point3d)(ref point2)).X, ((Point3d)(ref point2)).Y, 0.0);
		Point3d val12 = default(Point3d);
		((Point3d)(ref val12))._002Ector(0.0 - ((Point3d)(ref outpoint)).X, ((Point3d)(ref outpoint)).Y, 0.0);
		Point3d val13 = default(Point3d);
		((Point3d)(ref val13))._002Ector(0.0 - ((Point3d)(ref point)).X, ((Point3d)(ref point)).Y, 0.0);
		Point3d val14 = default(Point3d);
		((Point3d)(ref val14))._002Ector(0.0, ((Point3d)(ref point)).Y, 0.0);
		if (((Point3d)(ref val10)).X < ((Point3d)(ref val11)).X)
		{
			Point3d val15 = val10;
			val10 = val11;
			val11 = val15;
		}
		Curve val16 = (Curve)new LineCurve(val14, val8);
		Curve val17 = (Curve)new LineCurve(val8, val9);
		Curve val18 = (Curve)new LineCurve(val9, val10);
		Curve val19 = (Curve)new LineCurve(val10, val11);
		Curve val20 = (Curve)new LineCurve(val11, val12);
		Curve val21 = (Curve)new LineCurve(val12, val13);
		Curve val22 = (Curve)new LineCurve(val13, val14);
		if (cornerBlend > 0.0)
		{
			TrimCurve(val16, cornerBlend, (CurveEnd)2, out var crvTrimmed);
			TrimCurve(val17, cornerBlend, (CurveEnd)3, out var crvTrimmed2);
			TrimCurve(val18, cornerBlend, (CurveEnd)3, out var crvTrimmed3);
			TrimCurve(val19, cornerBlend, (CurveEnd)3, out var crvTrimmed4);
			TrimCurve(val20, cornerBlend, (CurveEnd)3, out var crvTrimmed5);
			TrimCurve(val21, cornerBlend, (CurveEnd)3, out var crvTrimmed6);
			TrimCurve(val22, cornerBlend, (CurveEnd)1, out var crvTrimmed7);
			BlendCorner(ref crvTrimmed, ref crvTrimmed2);
			BlendCorner(ref crvTrimmed2, ref crvTrimmed3);
			BlendCorner(ref crvTrimmed3, ref crvTrimmed4);
			BlendCorner(ref crvTrimmed4, ref crvTrimmed5);
			BlendCorner(ref crvTrimmed5, ref crvTrimmed6);
			BlendCorner(ref crvTrimmed6, ref crvTrimmed7);
			val16 = crvTrimmed;
			val17 = crvTrimmed2;
			val18 = crvTrimmed3;
			val19 = crvTrimmed4;
			val20 = crvTrimmed5;
			val21 = crvTrimmed6;
			val22 = crvTrimmed7;
		}
		return (Curve[])(object)new Curve[7] { val16, val17, val18, val19, val20, val21, val22 };
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
		//IL_0214: Unknown result type (might be due to invalid IL or missing references)
		//IL_0219: Unknown result type (might be due to invalid IL or missing references)
		//IL_0224: Unknown result type (might be due to invalid IL or missing references)
		//IL_0229: Unknown result type (might be due to invalid IL or missing references)
		//IL_0232: Unknown result type (might be due to invalid IL or missing references)
		//IL_023f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0241: Unknown result type (might be due to invalid IL or missing references)
		//IL_0243: Unknown result type (might be due to invalid IL or missing references)
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
		Interval angleDomain = ((Arc)(ref val11)).AngleDomain;
		double mid = ((Interval)(ref angleDomain)).Mid;
		angleDomain = ((Arc)(ref val11)).AngleDomain;
		((Arc)(ref val11)).Trim(new Interval(mid, ((Interval)(ref angleDomain)).Max));
		Arc val12 = default(Arc);
		((Arc)(ref val12))._002Ector(val2, val, val3);
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
		Curve val16 = (Curve)new ArcCurve(val11);
		Curve val17 = (Curve)new ArcCurve(val13);
		Curve val18 = (Curve)new ArcCurve(val14);
		Curve val19 = (Curve)new ArcCurve(val15);
		Curve val20 = (Curve)new ArcCurve(val12);
		if (cornerBlend > 0.0)
		{
			TrimCurve(val16, cornerBlend, (CurveEnd)2, out var crvTrimmed);
			TrimCurve(val17, cornerBlend, (CurveEnd)3, out var crvTrimmed2);
			TrimCurve(val18, cornerBlend, (CurveEnd)3, out var crvTrimmed3);
			TrimCurve(val19, cornerBlend, (CurveEnd)3, out var crvTrimmed4);
			TrimCurve(val20, cornerBlend, (CurveEnd)1, out var crvTrimmed5);
			BlendCorner(ref crvTrimmed, ref crvTrimmed2);
			BlendCorner(ref crvTrimmed2, ref crvTrimmed3);
			BlendCorner(ref crvTrimmed3, ref crvTrimmed4);
			BlendCorner(ref crvTrimmed4, ref crvTrimmed5);
			val16 = crvTrimmed;
			val17 = crvTrimmed2;
			val18 = crvTrimmed3;
			val19 = crvTrimmed4;
			val20 = crvTrimmed5;
		}
		return (Curve[])(object)new Curve[5] { val16, val17, val18, val19, val20 };
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
		Curve val20 = (Curve)new LineCurve(val11, val12);
		Curve val21 = (Curve)new LineCurve(val12, val13);
		Curve val22 = (Curve)new LineCurve(val13, val14);
		Curve val23 = (Curve)new LineCurve(val14, val15);
		Curve val24 = (Curve)new LineCurve(val15, val16);
		Curve val25 = (Curve)new LineCurve(val16, val17);
		Curve val26 = (Curve)new LineCurve(val17, val18);
		Curve val27 = (Curve)new LineCurve(val18, val19);
		Curve val28 = (Curve)new LineCurve(val19, val11);
		if (cornerBlend > 0.0)
		{
			if (cornerBlend < 0.11)
			{
				cornerBlend = 0.11;
			}
			TrimCurve(val20, cornerBlend, (CurveEnd)2, out var crvTrimmed);
			TrimCurve(val21, cornerBlend, (CurveEnd)3, out var crvTrimmed2);
			TrimCurve(val22, cornerBlend, (CurveEnd)3, out var crvTrimmed3);
			TrimCurve(val23, cornerBlend, (CurveEnd)3, out var crvTrimmed4);
			TrimCurve(val24, cornerBlend, (CurveEnd)3, out var crvTrimmed5);
			TrimCurve(val25, cornerBlend, (CurveEnd)3, out var crvTrimmed6);
			TrimCurve(val26, cornerBlend, (CurveEnd)3, out var crvTrimmed7);
			TrimCurve(val27, cornerBlend, (CurveEnd)3, out var crvTrimmed8);
			TrimCurve(val28, cornerBlend, (CurveEnd)1, out var crvTrimmed9);
			BlendCorner(ref crvTrimmed, ref crvTrimmed2);
			BlendCorner(ref crvTrimmed2, ref crvTrimmed3);
			BlendCorner(ref crvTrimmed3, ref crvTrimmed4);
			BlendCorner(ref crvTrimmed4, ref crvTrimmed5);
			BlendCorner(ref crvTrimmed5, ref crvTrimmed6);
			BlendCorner(ref crvTrimmed6, ref crvTrimmed7);
			BlendCorner(ref crvTrimmed7, ref crvTrimmed8);
			BlendCorner(ref crvTrimmed8, ref crvTrimmed9);
			val20 = crvTrimmed;
			val21 = crvTrimmed2;
			val22 = crvTrimmed3;
			val23 = crvTrimmed4;
			val24 = crvTrimmed5;
			val25 = crvTrimmed6;
			val26 = crvTrimmed7;
			val27 = crvTrimmed8;
			val28 = crvTrimmed9;
		}
		return (Curve[])(object)new Curve[9] { val20, val21, val22, val23, val24, val25, val26, val27, val28 };
	}

	private static Curve[] GemOffsetCurvesHalfMoon(CSGemData gem, double distance, double cornerBlend = 0.0)
	{
		//IL_001b: Unknown result type (might be due to invalid IL or missing references)
		//IL_008d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0092: Unknown result type (might be due to invalid IL or missing references)
		//IL_0097: Unknown result type (might be due to invalid IL or missing references)
		//IL_009c: Unknown result type (might be due to invalid IL or missing references)
		//IL_009e: Unknown result type (might be due to invalid IL or missing references)
		//IL_00be: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c3: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c5: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d6: Unknown result type (might be due to invalid IL or missing references)
		//IL_00db: Unknown result type (might be due to invalid IL or missing references)
		//IL_00dd: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e2: Unknown result type (might be due to invalid IL or missing references)
		//IL_0113: Unknown result type (might be due to invalid IL or missing references)
		//IL_0131: Unknown result type (might be due to invalid IL or missing references)
		//IL_0133: Unknown result type (might be due to invalid IL or missing references)
		//IL_0135: Unknown result type (might be due to invalid IL or missing references)
		//IL_013a: Unknown result type (might be due to invalid IL or missing references)
		//IL_013c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0149: Unknown result type (might be due to invalid IL or missing references)
		//IL_015e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0163: Unknown result type (might be due to invalid IL or missing references)
		//IL_016c: Unknown result type (might be due to invalid IL or missing references)
		//IL_018e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0193: Unknown result type (might be due to invalid IL or missing references)
		//IL_019c: Unknown result type (might be due to invalid IL or missing references)
		//IL_01b5: Unknown result type (might be due to invalid IL or missing references)
		//IL_01ba: Unknown result type (might be due to invalid IL or missing references)
		//IL_01c5: Unknown result type (might be due to invalid IL or missing references)
		//IL_01ca: Unknown result type (might be due to invalid IL or missing references)
		//IL_01d3: Unknown result type (might be due to invalid IL or missing references)
		//IL_01e1: Unknown result type (might be due to invalid IL or missing references)
		//IL_01e8: Unknown result type (might be due to invalid IL or missing references)
		//IL_01ef: Unknown result type (might be due to invalid IL or missing references)
		//IL_01f4: Unknown result type (might be due to invalid IL or missing references)
		//IL_0202: Unknown result type (might be due to invalid IL or missing references)
		//IL_0207: Unknown result type (might be due to invalid IL or missing references)
		//IL_020c: Unknown result type (might be due to invalid IL or missing references)
		//IL_020e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0212: Unknown result type (might be due to invalid IL or missing references)
		//IL_0217: Unknown result type (might be due to invalid IL or missing references)
		//IL_021e: Expected O, but got Unknown
		//IL_0220: Unknown result type (might be due to invalid IL or missing references)
		//IL_0225: Unknown result type (might be due to invalid IL or missing references)
		//IL_0227: Unknown result type (might be due to invalid IL or missing references)
		//IL_022e: Expected O, but got Unknown
		//IL_00e8: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ed: Unknown result type (might be due to invalid IL or missing references)
		//IL_010a: Unknown result type (might be due to invalid IL or missing references)
		//IL_010f: Unknown result type (might be due to invalid IL or missing references)
		CSGemData cSGemData = gem.Duplicate() as CSGemData;
		gem.GetGemXformRC(out var xDown, out var _);
		cSGemData?.ApplyTransform(xDown);
		double gemXSize = gem.GemXSize;
		double num = (gem.GemYSize + distance) / 2.0;
		gemXSize += distance * 2.0;
		num += distance;
		if (gemXSize < 0.04)
		{
			gemXSize = 0.04;
		}
		if (num < 0.04)
		{
			num = 0.04;
		}
		Transform val = Transform.Rotation(CSBlockUtil.CSUtilMath.ToRadians(90.0), Vector3d.ZAxis, Point3d.Origin);
		Transform val2 = Transform.Scale(Plane.WorldXY, -1.0, 1.0, 1.0);
		Transform val3 = Transform.Scale(Plane.WorldXY, gemXSize, num, 0.0);
		Transform val4 = Transform.Identity;
		if (cSGemData != null)
		{
			Point3d xMinPoint = cSGemData.XMinPoint;
			val4 = Transform.Translation(((Point3d)(ref xMinPoint)).X - distance, 0.0, 0.0);
		}
		Circle val5 = default(Circle);
		((Circle)(ref val5))._002Ector(Point3d.Origin, 1.0);
		NurbsCurve val6 = ((Circle)(ref val5)).ToNurbsCurve();
		((GeometryBase)val6).Transform(val3 * val2 * val);
		((GeometryBase)val6).Transform(val4);
		Interval domain = ((Curve)val6).Domain;
		Curve val7 = ((Curve)val6).Trim(new Interval(0.0, ((Interval)(ref domain)).Mid));
		Curve val8 = val7.DuplicateCurve();
		Curve obj = val8;
		domain = val7.Domain;
		val8 = obj.Trim(new Interval(0.0, ((Interval)(ref domain)).Mid));
		Curve val9 = val7.DuplicateCurve();
		Curve obj2 = val9;
		domain = val7.Domain;
		double mid = ((Interval)(ref domain)).Mid;
		domain = val7.Domain;
		val9 = obj2.Trim(new Interval(mid, ((Interval)(ref domain)).Max));
		Point3d val10 = val7.PointAtEnd + (val7.PointAtStart - val7.PointAtEnd) / 2.0;
		Curve val11 = (Curve)new LineCurve(val10, val7.PointAtStart);
		Curve val12 = (Curve)new LineCurve(val7.PointAtEnd, val10);
		if (cornerBlend > 0.0)
		{
			TrimCurve(val11, cornerBlend, (CurveEnd)2, out var crvTrimmed);
			TrimCurve(val8, cornerBlend, (CurveEnd)1, out var crvTrimmed2);
			TrimCurve(val9, cornerBlend, (CurveEnd)2, out var crvTrimmed3);
			TrimCurve(val12, cornerBlend, (CurveEnd)1, out var crvTrimmed4);
			BlendCorner(ref crvTrimmed, ref crvTrimmed2);
			BlendCorner(ref crvTrimmed3, ref crvTrimmed4);
			val11 = crvTrimmed;
			val8 = crvTrimmed2;
			val9 = crvTrimmed3;
			val12 = crvTrimmed4;
		}
		return (Curve[])(object)new Curve[4] { val11, val8, val9, val12 };
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
		//IL_060d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0612: Unknown result type (might be due to invalid IL or missing references)
		//IL_0618: Unknown result type (might be due to invalid IL or missing references)
		//IL_0631: Unknown result type (might be due to invalid IL or missing references)
		//IL_0638: Expected O, but got Unknown
		//IL_0664: Unknown result type (might be due to invalid IL or missing references)
		//IL_069a: Unknown result type (might be due to invalid IL or missing references)
		//IL_06d0: Unknown result type (might be due to invalid IL or missing references)
		//IL_0706: Unknown result type (might be due to invalid IL or missing references)
		//IL_07e3: Unknown result type (might be due to invalid IL or missing references)
		//IL_07e8: Unknown result type (might be due to invalid IL or missing references)
		//IL_07ea: Unknown result type (might be due to invalid IL or missing references)
		//IL_0803: Unknown result type (might be due to invalid IL or missing references)
		//IL_0808: Unknown result type (might be due to invalid IL or missing references)
		//IL_0828: Unknown result type (might be due to invalid IL or missing references)
		//IL_082d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0831: Unknown result type (might be due to invalid IL or missing references)
		//IL_0832: Unknown result type (might be due to invalid IL or missing references)
		//IL_0834: Unknown result type (might be due to invalid IL or missing references)
		//IL_0839: Unknown result type (might be due to invalid IL or missing references)
		//IL_083b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0840: Unknown result type (might be due to invalid IL or missing references)
		//IL_0842: Unknown result type (might be due to invalid IL or missing references)
		//IL_0859: Unknown result type (might be due to invalid IL or missing references)
		//IL_0879: Unknown result type (might be due to invalid IL or missing references)
		//IL_087e: Unknown result type (might be due to invalid IL or missing references)
		//IL_088b: Unknown result type (might be due to invalid IL or missing references)
		//IL_08a6: Unknown result type (might be due to invalid IL or missing references)
		//IL_08c1: Unknown result type (might be due to invalid IL or missing references)
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
		Point3d pointAtEnd = ((Curve)val5).PointAtEnd;
		((Point3d)(ref pointAtEnd)).DistanceTo(((Curve)val2).PointAtStart);
		_ = 0.01;
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
		RhUtilHelper.RhinoMakeCurveEndsMeet((Curve)(object)val5, doAdjustCurveA: false, (Curve)(object)val2, doAdjustCurveB: true);
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
			TrimCurve((Curve)(object)val6, cornerBlend, (CurveEnd)1, out var crvTrimmed);
			TrimCurve((Curve)(object)val2, cornerBlend, (CurveEnd)2, out var crvTrimmed2);
			TrimCurve((Curve)(object)val9, cornerBlend, (CurveEnd)1, out var crvTrimmed3);
			TrimCurve((Curve)(object)val11, cornerBlend, (CurveEnd)2, out var crvTrimmed4);
			BlendCorner(ref crvTrimmed2, ref crvTrimmed3);
			BlendCorner(ref crvTrimmed4, ref crvTrimmed);
			val6 = crvTrimmed.ToNurbsCurve();
			val2 = crvTrimmed2.ToNurbsCurve();
			val9 = crvTrimmed3.ToNurbsCurve();
			val11 = crvTrimmed4.ToNurbsCurve();
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
			TrimCurve((Curve)(object)val6, cornerBlend, (CurveEnd)3, out var crvTrimmed);
			TrimCurve((Curve)(object)val8, cornerBlend, (CurveEnd)3, out var crvTrimmed2);
			BlendCorner(ref crvTrimmed, ref crvTrimmed2);
			BlendCorner(ref crvTrimmed2, ref crvTrimmed);
			val6 = crvTrimmed.ToNurbsCurve();
			val8 = crvTrimmed2.ToNurbsCurve();
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
		//IL_0174: Unknown result type (might be due to invalid IL or missing references)
		//IL_0179: Unknown result type (might be due to invalid IL or missing references)
		//IL_0186: Unknown result type (might be due to invalid IL or missing references)
		//IL_01a1: Unknown result type (might be due to invalid IL or missing references)
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
		Transform val9 = Transform.Mirror(Plane.WorldZX);
		NurbsCurve val10 = val7.ToNurbsCurve();
		((GeometryBase)val10).Transform(val9);
		((Curve)val10).Reverse();
		NurbsCurve val11 = val8.ToNurbsCurve();
		((GeometryBase)val11).Transform(val9);
		((Curve)val11).Reverse();
		return (Curve[])(object)new Curve[4]
		{
			val7,
			(Curve)val10,
			(Curve)val11,
			val8
		};
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
		//IL_01c6: Unknown result type (might be due to invalid IL or missing references)
		//IL_01c8: Unknown result type (might be due to invalid IL or missing references)
		//IL_01ca: Unknown result type (might be due to invalid IL or missing references)
		//IL_01dc: Unknown result type (might be due to invalid IL or missing references)
		//IL_01de: Unknown result type (might be due to invalid IL or missing references)
		//IL_0257: Unknown result type (might be due to invalid IL or missing references)
		//IL_0274: Unknown result type (might be due to invalid IL or missing references)
		//IL_0279: Unknown result type (might be due to invalid IL or missing references)
		//IL_02b5: Unknown result type (might be due to invalid IL or missing references)
		//IL_02ba: Unknown result type (might be due to invalid IL or missing references)
		//IL_02bf: Unknown result type (might be due to invalid IL or missing references)
		//IL_02cc: Unknown result type (might be due to invalid IL or missing references)
		//IL_02e7: Unknown result type (might be due to invalid IL or missing references)
		//IL_0324: Unknown result type (might be due to invalid IL or missing references)
		//IL_034e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0378: Unknown result type (might be due to invalid IL or missing references)
		//IL_03a2: Unknown result type (might be due to invalid IL or missing references)
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
		((Vector3d)(ref val8)).Unitize();
		Vector3d inputVector = default(Vector3d);
		((Vector3d)(ref inputVector))._002Ector(val7 - val5);
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
		val9 = ((Curve)val9).Rebuild(val9.Points.Count, 3, true);
		Transform val10 = Transform.Mirror(Plane.WorldYZ);
		NurbsCurve val11 = ((Curve)val9).ToNurbsCurve();
		((GeometryBase)val11).Transform(val10);
		((Curve)val11).Reverse();
		NurbsCurve val12 = ((Curve)val4).ToNurbsCurve();
		((GeometryBase)val12).Transform(val10);
		((Curve)val12).Reverse();
		if (gem.VersionMajor > 1)
		{
			((GeometryBase)val9).Translate(new Vector3d(0.0, 0.08 * gemYSize, 0.0));
			((GeometryBase)val4).Translate(new Vector3d(0.0, 0.08 * gemYSize, 0.0));
			((GeometryBase)val11).Translate(new Vector3d(0.0, 0.08 * gemYSize, 0.0));
			((GeometryBase)val12).Translate(new Vector3d(0.0, 0.08 * gemYSize, 0.0));
		}
		if (cornerBlend > 0.0)
		{
			TrimCurve((Curve)(object)val9, cornerBlend, (CurveEnd)2, out var crvTrimmed);
			TrimCurve((Curve)(object)val11, cornerBlend, (CurveEnd)1, out var crvTrimmed2);
			BlendCorner(ref crvTrimmed, ref crvTrimmed2);
			val9 = crvTrimmed.ToNurbsCurve();
			val11 = crvTrimmed2.ToNurbsCurve();
		}
		return (Curve[])(object)new Curve[4]
		{
			(Curve)val4,
			(Curve)val9,
			(Curve)val11,
			(Curve)val12
		};
	}

	private static Curve[] GemOffsetCurvesRound(CSGemData gem, double distance)
	{
		//IL_0064: Unknown result type (might be due to invalid IL or missing references)
		//IL_0069: Unknown result type (might be due to invalid IL or missing references)
		//IL_006e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0073: Unknown result type (might be due to invalid IL or missing references)
		//IL_0074: Unknown result type (might be due to invalid IL or missing references)
		//IL_0079: Unknown result type (might be due to invalid IL or missing references)
		//IL_007e: Unknown result type (might be due to invalid IL or missing references)
		//IL_007f: Unknown result type (might be due to invalid IL or missing references)
		//IL_008f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0094: Unknown result type (might be due to invalid IL or missing references)
		//IL_0098: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b6: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b8: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b9: Unknown result type (might be due to invalid IL or missing references)
		//IL_00be: Unknown result type (might be due to invalid IL or missing references)
		//IL_00bf: Unknown result type (might be due to invalid IL or missing references)
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
		Transform val2 = Transform.Mirror(Plane.WorldYZ);
		Transform val3 = Transform.Scale(Plane.WorldXY, num, num2, 0.0);
		Circle val4 = default(Circle);
		((Circle)(ref val4))._002Ector(Point3d.Origin, 1.0);
		NurbsCurve val5 = ((Circle)(ref val4)).ToNurbsCurve();
		((GeometryBase)val5).Transform(val3 * val2 * val);
		return (Curve[])(object)new Curve[1] { (Curve)val5 };
	}

	private static Curve[] GemOffsetCurvesSquare(CSGemData gem, double distance, double cornerBlend = 0.0)
	{
		//IL_00bc: Unknown result type (might be due to invalid IL or missing references)
		//IL_00bd: Unknown result type (might be due to invalid IL or missing references)
		//IL_00be: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c5: Expected O, but got Unknown
		//IL_00c5: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c6: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c8: Unknown result type (might be due to invalid IL or missing references)
		//IL_00cf: Expected O, but got Unknown
		//IL_00cf: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d1: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d3: Unknown result type (might be due to invalid IL or missing references)
		//IL_00da: Expected O, but got Unknown
		//IL_00da: Unknown result type (might be due to invalid IL or missing references)
		//IL_00dc: Unknown result type (might be due to invalid IL or missing references)
		//IL_00de: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e5: Expected O, but got Unknown
		//IL_00e5: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e7: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e8: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ef: Expected O, but got Unknown
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
		Curve val6 = (Curve)new LineCurve(val, val2);
		Curve val7 = (Curve)new LineCurve(val2, val3);
		Curve val8 = (Curve)new LineCurve(val3, val4);
		Curve val9 = (Curve)new LineCurve(val4, val5);
		Curve val10 = (Curve)new LineCurve(val5, val);
		if (cornerBlend > 0.0)
		{
			TrimCurve(val6, cornerBlend, (CurveEnd)2, out var crvTrimmed);
			TrimCurve(val7, cornerBlend, (CurveEnd)3, out var crvTrimmed2);
			TrimCurve(val8, cornerBlend, (CurveEnd)3, out var crvTrimmed3);
			TrimCurve(val9, cornerBlend, (CurveEnd)3, out var crvTrimmed4);
			TrimCurve(val10, cornerBlend, (CurveEnd)1, out var crvTrimmed5);
			BlendCorner(ref crvTrimmed, ref crvTrimmed2);
			BlendCorner(ref crvTrimmed2, ref crvTrimmed3);
			BlendCorner(ref crvTrimmed3, ref crvTrimmed4);
			BlendCorner(ref crvTrimmed4, ref crvTrimmed5);
			val6 = crvTrimmed;
			val7 = crvTrimmed2;
			val8 = crvTrimmed3;
			val9 = crvTrimmed4;
			val10 = crvTrimmed5;
		}
		return (Curve[])(object)new Curve[5] { val6, val7, val8, val9, val10 };
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

	private static Curve[] GemOffsetCurvesTriangle(CSGemData gem, double distance, double cornerBlend = 0.0)
	{
		//IL_00bc: Unknown result type (might be due to invalid IL or missing references)
		//IL_00be: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c0: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c5: Unknown result type (might be due to invalid IL or missing references)
		//IL_00da: Unknown result type (might be due to invalid IL or missing references)
		//IL_00de: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e3: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e7: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e8: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ea: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f6: Unknown result type (might be due to invalid IL or missing references)
		//IL_018e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0190: Unknown result type (might be due to invalid IL or missing references)
		//IL_0192: Unknown result type (might be due to invalid IL or missing references)
		//IL_0199: Expected O, but got Unknown
		//IL_0199: Unknown result type (might be due to invalid IL or missing references)
		//IL_019b: Unknown result type (might be due to invalid IL or missing references)
		//IL_019d: Unknown result type (might be due to invalid IL or missing references)
		//IL_01a4: Expected O, but got Unknown
		//IL_01a4: Unknown result type (might be due to invalid IL or missing references)
		//IL_01a6: Unknown result type (might be due to invalid IL or missing references)
		//IL_01a8: Unknown result type (might be due to invalid IL or missing references)
		//IL_01af: Expected O, but got Unknown
		//IL_01af: Unknown result type (might be due to invalid IL or missing references)
		//IL_01b1: Unknown result type (might be due to invalid IL or missing references)
		//IL_01b3: Unknown result type (might be due to invalid IL or missing references)
		//IL_01ba: Expected O, but got Unknown
		double num = gem.GemXSize / 2.0;
		double num2 = gem.GemYSize / 2.0;
		Point3d val = default(Point3d);
		((Point3d)(ref val))._002Ector(num, num2 * (2.0 / 3.0), 0.0);
		Point3d val2 = default(Point3d);
		((Point3d)(ref val2))._002Ector(0.0, (0.0 - num2) * 1.3333333333333333, 0.0);
		Vector3d val3 = default(Vector3d);
		((Vector3d)(ref val3))._002Ector(((Point3d)(ref val)).Y - ((Point3d)(ref val2)).Y, 0.0 - (((Point3d)(ref val)).X - ((Point3d)(ref val2)).X), 0.0);
		((Vector3d)(ref val3)).Unitize();
		Vector3d val4 = default(Vector3d);
		((Vector3d)(ref val4))._002Ector(0.0, 1.0, 0.0);
		val4 += val3;
		((Vector3d)(ref val4)).Unitize();
		double num3 = distance / ((Vector3d)(ref val4)).Y;
		val4 *= num3;
		Point3d val5 = default(Point3d);
		((Point3d)(ref val5))._002Ector(val + val4);
		Point3d val6 = default(Point3d);
		((Point3d)(ref val6))._002Ector(val2);
		((Point3d)(ref val6)).Y = ((Point3d)(ref val6)).Y + distance / ((Vector3d)(ref val3)).Y;
		Point3d val7 = default(Point3d);
		((Point3d)(ref val7))._002Ector(0.0, ((Point3d)(ref val5)).Y, 0.0);
		Point3d val8 = default(Point3d);
		((Point3d)(ref val8))._002Ector(((Point3d)(ref val5)).X, ((Point3d)(ref val5)).Y, 0.0);
		Point3d val9 = default(Point3d);
		((Point3d)(ref val9))._002Ector(((Point3d)(ref val6)).X, ((Point3d)(ref val6)).Y, 0.0);
		Point3d val10 = default(Point3d);
		((Point3d)(ref val10))._002Ector(0.0 - ((Point3d)(ref val5)).X, ((Point3d)(ref val5)).Y, 0.0);
		Curve val11 = (Curve)new LineCurve(val7, val8);
		Curve val12 = (Curve)new LineCurve(val8, val9);
		Curve val13 = (Curve)new LineCurve(val9, val10);
		Curve val14 = (Curve)new LineCurve(val10, val7);
		if (cornerBlend > 0.0)
		{
			TrimCurve(val11, cornerBlend, (CurveEnd)2, out var crvTrimmed);
			TrimCurve(val12, cornerBlend, (CurveEnd)3, out var crvTrimmed2);
			TrimCurve(val13, cornerBlend, (CurveEnd)3, out var crvTrimmed3);
			TrimCurve(val14, cornerBlend, (CurveEnd)1, out var crvTrimmed4);
			BlendCorner(ref crvTrimmed, ref crvTrimmed2);
			BlendCorner(ref crvTrimmed2, ref crvTrimmed3);
			BlendCorner(ref crvTrimmed3, ref crvTrimmed4);
			val11 = crvTrimmed;
			val12 = crvTrimmed2;
			val13 = crvTrimmed3;
			val14 = crvTrimmed4;
		}
		return (Curve[])(object)new Curve[4] { val11, val12, val13, val14 };
	}

	private static Curve[] GemOffsetCurvesTrillionCurved(CSGemData gem, double distance, double cornerBlend = 0.0)
	{
		//IL_04aa: Unknown result type (might be due to invalid IL or missing references)
		//IL_04b2: Unknown result type (might be due to invalid IL or missing references)
		//IL_04e9: Unknown result type (might be due to invalid IL or missing references)
		//IL_04f2: Unknown result type (might be due to invalid IL or missing references)
		//IL_04fb: Unknown result type (might be due to invalid IL or missing references)
		//IL_0504: Unknown result type (might be due to invalid IL or missing references)
		//IL_050d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0516: Unknown result type (might be due to invalid IL or missing references)
		//IL_051f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0528: Unknown result type (might be due to invalid IL or missing references)
		//IL_0531: Unknown result type (might be due to invalid IL or missing references)
		//IL_053a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0543: Unknown result type (might be due to invalid IL or missing references)
		//IL_0555: Unknown result type (might be due to invalid IL or missing references)
		//IL_056b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0581: Unknown result type (might be due to invalid IL or missing references)
		//IL_0597: Unknown result type (might be due to invalid IL or missing references)
		//IL_05ad: Unknown result type (might be due to invalid IL or missing references)
		//IL_05c3: Unknown result type (might be due to invalid IL or missing references)
		//IL_05d9: Unknown result type (might be due to invalid IL or missing references)
		//IL_05ef: Unknown result type (might be due to invalid IL or missing references)
		//IL_0605: Unknown result type (might be due to invalid IL or missing references)
		//IL_061b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0631: Unknown result type (might be due to invalid IL or missing references)
		//IL_063e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0640: Unknown result type (might be due to invalid IL or missing references)
		//IL_0642: Unknown result type (might be due to invalid IL or missing references)
		//IL_0643: Unknown result type (might be due to invalid IL or missing references)
		//IL_0646: Unknown result type (might be due to invalid IL or missing references)
		//IL_064b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0650: Unknown result type (might be due to invalid IL or missing references)
		//IL_065a: Unknown result type (might be due to invalid IL or missing references)
		//IL_065c: Unknown result type (might be due to invalid IL or missing references)
		//IL_065e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0660: Unknown result type (might be due to invalid IL or missing references)
		//IL_0663: Unknown result type (might be due to invalid IL or missing references)
		//IL_0668: Unknown result type (might be due to invalid IL or missing references)
		//IL_066d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0677: Unknown result type (might be due to invalid IL or missing references)
		//IL_0679: Unknown result type (might be due to invalid IL or missing references)
		//IL_067b: Unknown result type (might be due to invalid IL or missing references)
		//IL_067d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0680: Unknown result type (might be due to invalid IL or missing references)
		//IL_0685: Unknown result type (might be due to invalid IL or missing references)
		//IL_068a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0694: Unknown result type (might be due to invalid IL or missing references)
		//IL_0696: Unknown result type (might be due to invalid IL or missing references)
		//IL_0698: Unknown result type (might be due to invalid IL or missing references)
		//IL_069a: Unknown result type (might be due to invalid IL or missing references)
		//IL_069d: Unknown result type (might be due to invalid IL or missing references)
		//IL_06a2: Unknown result type (might be due to invalid IL or missing references)
		//IL_06a7: Unknown result type (might be due to invalid IL or missing references)
		//IL_06b1: Unknown result type (might be due to invalid IL or missing references)
		//IL_06b3: Unknown result type (might be due to invalid IL or missing references)
		//IL_06b5: Unknown result type (might be due to invalid IL or missing references)
		//IL_06b7: Unknown result type (might be due to invalid IL or missing references)
		//IL_06ba: Unknown result type (might be due to invalid IL or missing references)
		//IL_06bf: Unknown result type (might be due to invalid IL or missing references)
		//IL_06c4: Unknown result type (might be due to invalid IL or missing references)
		//IL_06ce: Unknown result type (might be due to invalid IL or missing references)
		//IL_06d0: Unknown result type (might be due to invalid IL or missing references)
		//IL_06d2: Unknown result type (might be due to invalid IL or missing references)
		//IL_06d4: Unknown result type (might be due to invalid IL or missing references)
		//IL_06d7: Unknown result type (might be due to invalid IL or missing references)
		//IL_06dc: Unknown result type (might be due to invalid IL or missing references)
		//IL_06e1: Unknown result type (might be due to invalid IL or missing references)
		//IL_06eb: Unknown result type (might be due to invalid IL or missing references)
		//IL_06ed: Unknown result type (might be due to invalid IL or missing references)
		//IL_06ef: Unknown result type (might be due to invalid IL or missing references)
		//IL_06f1: Unknown result type (might be due to invalid IL or missing references)
		//IL_06f4: Unknown result type (might be due to invalid IL or missing references)
		//IL_06f9: Unknown result type (might be due to invalid IL or missing references)
		//IL_06fe: Unknown result type (might be due to invalid IL or missing references)
		//IL_0708: Unknown result type (might be due to invalid IL or missing references)
		//IL_070a: Unknown result type (might be due to invalid IL or missing references)
		//IL_070c: Unknown result type (might be due to invalid IL or missing references)
		//IL_070e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0711: Unknown result type (might be due to invalid IL or missing references)
		//IL_0716: Unknown result type (might be due to invalid IL or missing references)
		//IL_071b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0725: Unknown result type (might be due to invalid IL or missing references)
		//IL_0727: Unknown result type (might be due to invalid IL or missing references)
		//IL_0729: Unknown result type (might be due to invalid IL or missing references)
		//IL_072b: Unknown result type (might be due to invalid IL or missing references)
		//IL_072e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0733: Unknown result type (might be due to invalid IL or missing references)
		//IL_0738: Unknown result type (might be due to invalid IL or missing references)
		//IL_0742: Unknown result type (might be due to invalid IL or missing references)
		//IL_0744: Unknown result type (might be due to invalid IL or missing references)
		//IL_0746: Unknown result type (might be due to invalid IL or missing references)
		//IL_0748: Unknown result type (might be due to invalid IL or missing references)
		//IL_074b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0750: Unknown result type (might be due to invalid IL or missing references)
		//IL_0755: Unknown result type (might be due to invalid IL or missing references)
		//IL_08dd: Unknown result type (might be due to invalid IL or missing references)
		//IL_08fb: Unknown result type (might be due to invalid IL or missing references)
		//IL_0919: Unknown result type (might be due to invalid IL or missing references)
		//IL_0937: Unknown result type (might be due to invalid IL or missing references)
		//IL_0955: Unknown result type (might be due to invalid IL or missing references)
		//IL_0973: Unknown result type (might be due to invalid IL or missing references)
		//IL_0991: Unknown result type (might be due to invalid IL or missing references)
		//IL_09af: Unknown result type (might be due to invalid IL or missing references)
		//IL_09cd: Unknown result type (might be due to invalid IL or missing references)
		//IL_09f4: Unknown result type (might be due to invalid IL or missing references)
		//IL_09f6: Unknown result type (might be due to invalid IL or missing references)
		//IL_09f8: Unknown result type (might be due to invalid IL or missing references)
		//IL_0a0c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0a11: Unknown result type (might be due to invalid IL or missing references)
		//IL_0a26: Unknown result type (might be due to invalid IL or missing references)
		//IL_0a2b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0a34: Unknown result type (might be due to invalid IL or missing references)
		//IL_0a44: Unknown result type (might be due to invalid IL or missing references)
		//IL_0a49: Unknown result type (might be due to invalid IL or missing references)
		//IL_0a54: Unknown result type (might be due to invalid IL or missing references)
		//IL_0a59: Unknown result type (might be due to invalid IL or missing references)
		//IL_0a6c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0a7a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0a7c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0a7e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0a90: Unknown result type (might be due to invalid IL or missing references)
		//IL_0a92: Unknown result type (might be due to invalid IL or missing references)
		//IL_0a94: Unknown result type (might be due to invalid IL or missing references)
		//IL_0aa6: Unknown result type (might be due to invalid IL or missing references)
		//IL_0aa8: Unknown result type (might be due to invalid IL or missing references)
		//IL_0aaa: Unknown result type (might be due to invalid IL or missing references)
		//IL_0abc: Unknown result type (might be due to invalid IL or missing references)
		//IL_0abe: Unknown result type (might be due to invalid IL or missing references)
		//IL_0ac0: Unknown result type (might be due to invalid IL or missing references)
		//IL_0ad2: Unknown result type (might be due to invalid IL or missing references)
		//IL_0ad4: Unknown result type (might be due to invalid IL or missing references)
		//IL_0ad6: Unknown result type (might be due to invalid IL or missing references)
		//IL_0ae6: Unknown result type (might be due to invalid IL or missing references)
		//IL_0aeb: Unknown result type (might be due to invalid IL or missing references)
		//IL_0af0: Unknown result type (might be due to invalid IL or missing references)
		//IL_0afd: Unknown result type (might be due to invalid IL or missing references)
		//IL_0b18: Unknown result type (might be due to invalid IL or missing references)
		//IL_0b33: Unknown result type (might be due to invalid IL or missing references)
		//IL_0b4e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0b69: Unknown result type (might be due to invalid IL or missing references)
		double num = gem.GemXSize / 2.0;
		double num2 = gem.GemYSize / 2.0;
		if (distance < 0.0)
		{
			num = gem.GemXSize / 2.0 + distance;
			num2 = gem.GemYSize / 2.0 + distance;
			distance = 0.0;
		}
		if (num < 0.04)
		{
			num = 0.04;
		}
		if (num2 < 0.04)
		{
			num2 = 0.04;
		}
		Point3d val = default(Point3d);
		((Point3d)(ref val))._002Ector(0.0, num2 * 0.7969, 0.0);
		Point3d val2 = default(Point3d);
		((Point3d)(ref val2))._002Ector(num * 0.314, num2 * 0.7887, 0.0);
		Point3d val3 = default(Point3d);
		((Point3d)(ref val3))._002Ector(num * 0.6668, num2 * 0.7276, 0.0);
		Point3d val4 = default(Point3d);
		((Point3d)(ref val4))._002Ector(num, num2 * 0.5969, 0.0);
		Point3d val5 = default(Point3d);
		((Point3d)(ref val5))._002Ector(num * 0.9713, num2 * 0.4106, 0.0);
		Point3d val6 = default(Point3d);
		((Point3d)(ref val6))._002Ector(num * 0.9266, num2 * 0.2275, 0.0);
		Point3d val7 = default(Point3d);
		((Point3d)(ref val7))._002Ector(num * 0.7308, (0.0 - num2) * 0.2561, 0.0);
		Point3d val8 = default(Point3d);
		((Point3d)(ref val8))._002Ector(num * 0.4705, (0.0 - num2) * 0.7083, 0.0);
		Point3d val9 = default(Point3d);
		((Point3d)(ref val9))._002Ector(num * 0.3472, (0.0 - num2) * 0.877, 0.0);
		Point3d val10 = default(Point3d);
		((Point3d)(ref val10))._002Ector(num * 0.2091, (0.0 - num2) * 1.0337, 0.0);
		Point3d val11 = default(Point3d);
		((Point3d)(ref val11))._002Ector(num * 0.1081, (0.0 - num2) * 1.1228, 0.0);
		Point3d val12 = default(Point3d);
		((Point3d)(ref val12))._002Ector(0.0, (0.0 - num2) * 1.2031, 0.0);
		Vector3d val13 = default(Vector3d);
		((Vector3d)(ref val13))._002Ector(((Point3d)(ref val)).Y - ((Point3d)(ref val2)).Y, 0.0 - (((Point3d)(ref val)).X - ((Point3d)(ref val2)).X), 0.0);
		((Vector3d)(ref val13)).Unitize();
		Vector3d val14 = default(Vector3d);
		((Vector3d)(ref val14))._002Ector(((Point3d)(ref val2)).Y - ((Point3d)(ref val3)).Y, 0.0 - (((Point3d)(ref val2)).X - ((Point3d)(ref val3)).X), 0.0);
		((Vector3d)(ref val14)).Unitize();
		Vector3d val15 = default(Vector3d);
		((Vector3d)(ref val15))._002Ector(((Point3d)(ref val3)).Y - ((Point3d)(ref val4)).Y, 0.0 - (((Point3d)(ref val3)).X - ((Point3d)(ref val4)).X), 0.0);
		((Vector3d)(ref val15)).Unitize();
		Vector3d val16 = default(Vector3d);
		((Vector3d)(ref val16))._002Ector(((Point3d)(ref val4)).Y - ((Point3d)(ref val5)).Y, 0.0 - (((Point3d)(ref val4)).X - ((Point3d)(ref val5)).X), 0.0);
		((Vector3d)(ref val16)).Unitize();
		Vector3d val17 = default(Vector3d);
		((Vector3d)(ref val17))._002Ector(((Point3d)(ref val5)).Y - ((Point3d)(ref val6)).Y, 0.0 - (((Point3d)(ref val5)).X - ((Point3d)(ref val6)).X), 0.0);
		((Vector3d)(ref val17)).Unitize();
		Vector3d val18 = default(Vector3d);
		((Vector3d)(ref val18))._002Ector(((Point3d)(ref val6)).Y - ((Point3d)(ref val7)).Y, 0.0 - (((Point3d)(ref val6)).X - ((Point3d)(ref val7)).X), 0.0);
		((Vector3d)(ref val18)).Unitize();
		Vector3d val19 = default(Vector3d);
		((Vector3d)(ref val19))._002Ector(((Point3d)(ref val7)).Y - ((Point3d)(ref val8)).Y, 0.0 - (((Point3d)(ref val7)).X - ((Point3d)(ref val8)).X), 0.0);
		((Vector3d)(ref val19)).Unitize();
		Vector3d val20 = default(Vector3d);
		((Vector3d)(ref val20))._002Ector(((Point3d)(ref val8)).Y - ((Point3d)(ref val9)).Y, 0.0 - (((Point3d)(ref val8)).X - ((Point3d)(ref val9)).X), 0.0);
		((Vector3d)(ref val20)).Unitize();
		Vector3d val21 = default(Vector3d);
		((Vector3d)(ref val21))._002Ector(((Point3d)(ref val9)).Y - ((Point3d)(ref val10)).Y, 0.0 - (((Point3d)(ref val9)).X - ((Point3d)(ref val10)).X), 0.0);
		((Vector3d)(ref val21)).Unitize();
		Vector3d val22 = default(Vector3d);
		((Vector3d)(ref val22))._002Ector(((Point3d)(ref val10)).Y - ((Point3d)(ref val11)).Y, 0.0 - (((Point3d)(ref val10)).X - ((Point3d)(ref val11)).X), 0.0);
		((Vector3d)(ref val22)).Unitize();
		Vector3d val23 = default(Vector3d);
		((Vector3d)(ref val23))._002Ector(((Point3d)(ref val11)).Y - ((Point3d)(ref val12)).Y, 0.0 - (((Point3d)(ref val11)).X - ((Point3d)(ref val12)).X), 0.0);
		((Vector3d)(ref val23)).Unitize();
		Point3d point = default(Point3d);
		((Point3d)(ref point))._002Ector(val);
		Point3d val24 = default(Point3d);
		((Point3d)(ref val24))._002Ector(val12);
		((Point3d)(ref point)).Y = ((Point3d)(ref point)).Y + distance / ((Vector3d)(ref val13)).Y;
		((Point3d)(ref val24)).Y = ((Point3d)(ref val24)).Y + distance / ((Vector3d)(ref val23)).Y;
		Vector3d vector = default(Vector3d);
		((Vector3d)(ref vector))._002Ector(val13);
		Vector3d val25 = default(Vector3d);
		((Vector3d)(ref val25))._002Ector(val14);
		Vector3d val26 = default(Vector3d);
		((Vector3d)(ref val26))._002Ector(val15);
		Vector3d val27 = default(Vector3d);
		((Vector3d)(ref val27))._002Ector(val16);
		Vector3d val28 = default(Vector3d);
		((Vector3d)(ref val28))._002Ector(val17);
		Vector3d val29 = default(Vector3d);
		((Vector3d)(ref val29))._002Ector(val18);
		Vector3d val30 = default(Vector3d);
		((Vector3d)(ref val30))._002Ector(val19);
		Vector3d val31 = default(Vector3d);
		((Vector3d)(ref val31))._002Ector(val20);
		Vector3d val32 = default(Vector3d);
		((Vector3d)(ref val32))._002Ector(val21);
		Vector3d val33 = default(Vector3d);
		((Vector3d)(ref val33))._002Ector(val22);
		Vector3d vector2 = default(Vector3d);
		((Vector3d)(ref vector2))._002Ector(val23);
		((Vector3d)(ref vector)).Rotate(Math.PI / 2.0, Vector3d.ZAxis);
		((Vector3d)(ref val25)).Rotate(Math.PI / 2.0, Vector3d.ZAxis);
		((Vector3d)(ref val26)).Rotate(Math.PI / 2.0, Vector3d.ZAxis);
		((Vector3d)(ref val27)).Rotate(Math.PI / 2.0, Vector3d.ZAxis);
		((Vector3d)(ref val28)).Rotate(Math.PI / 2.0, Vector3d.ZAxis);
		((Vector3d)(ref val29)).Rotate(Math.PI / 2.0, Vector3d.ZAxis);
		((Vector3d)(ref val30)).Rotate(Math.PI / 2.0, Vector3d.ZAxis);
		((Vector3d)(ref val31)).Rotate(Math.PI / 2.0, Vector3d.ZAxis);
		((Vector3d)(ref val32)).Rotate(Math.PI / 2.0, Vector3d.ZAxis);
		((Vector3d)(ref val33)).Rotate(Math.PI / 2.0, Vector3d.ZAxis);
		((Vector3d)(ref vector2)).Rotate(Math.PI / 2.0, Vector3d.ZAxis);
		CSBlockUtil.CSUtilMath.LineInt(out var outpoint, point, vector, val2 + val14 * distance, val25);
		CSBlockUtil.CSUtilMath.LineInt(out var outpoint2, outpoint, val25, val3 + val15 * distance, val26);
		CSBlockUtil.CSUtilMath.LineInt(out var outpoint3, outpoint2, val26, val4 + val16 * distance, val27);
		CSBlockUtil.CSUtilMath.LineInt(out var outpoint4, outpoint3, val27, val5 + val17 * distance, val28);
		CSBlockUtil.CSUtilMath.LineInt(out var outpoint5, outpoint4, val28, val6 + val18 * distance, val29);
		CSBlockUtil.CSUtilMath.LineInt(out var outpoint6, outpoint5, val29, val7 + val19 * distance, val30);
		CSBlockUtil.CSUtilMath.LineInt(out var outpoint7, outpoint6, val30, val8 + val20 * distance, val31);
		CSBlockUtil.CSUtilMath.LineInt(out var outpoint8, outpoint7, val31, val9 + val21 * distance, val32);
		CSBlockUtil.CSUtilMath.LineInt(out var outpoint9, outpoint8, val32, val10 + val22 * distance, val33);
		CSBlockUtil.CSUtilMath.LineInt(out var outpoint10, outpoint9, val33, val11 + val23 * distance, vector2);
		Point3d val34 = default(Point3d);
		((Point3d)(ref val34))._002Ector(((Point3d)(ref point)).X, ((Point3d)(ref point)).Y, 0.0);
		Point3d val35 = default(Point3d);
		((Point3d)(ref val35))._002Ector(((Point3d)(ref outpoint)).X, ((Point3d)(ref outpoint)).Y, 0.0);
		Point3d val36 = default(Point3d);
		((Point3d)(ref val36))._002Ector(((Point3d)(ref outpoint2)).X, ((Point3d)(ref outpoint2)).Y, 0.0);
		Point3d val37 = default(Point3d);
		((Point3d)(ref val37))._002Ector(((Point3d)(ref outpoint3)).X, ((Point3d)(ref outpoint3)).Y, 0.0);
		Point3d val38 = default(Point3d);
		((Point3d)(ref val38))._002Ector(((Point3d)(ref outpoint4)).X, ((Point3d)(ref outpoint4)).Y, 0.0);
		Point3d val39 = default(Point3d);
		((Point3d)(ref val39))._002Ector(((Point3d)(ref outpoint5)).X, ((Point3d)(ref outpoint5)).Y, 0.0);
		Point3d val40 = default(Point3d);
		((Point3d)(ref val40))._002Ector(((Point3d)(ref outpoint6)).X, ((Point3d)(ref outpoint6)).Y, 0.0);
		Point3d val41 = default(Point3d);
		((Point3d)(ref val41))._002Ector(((Point3d)(ref outpoint7)).X, ((Point3d)(ref outpoint7)).Y, 0.0);
		Point3d val42 = default(Point3d);
		((Point3d)(ref val42))._002Ector(((Point3d)(ref outpoint8)).X, ((Point3d)(ref outpoint8)).Y, 0.0);
		Point3d val43 = default(Point3d);
		((Point3d)(ref val43))._002Ector(((Point3d)(ref outpoint9)).X, ((Point3d)(ref outpoint9)).Y, 0.0);
		Point3d val44 = default(Point3d);
		((Point3d)(ref val44))._002Ector(((Point3d)(ref outpoint10)).X, ((Point3d)(ref outpoint10)).Y, 0.0);
		Point3d val45 = default(Point3d);
		((Point3d)(ref val45))._002Ector(((Point3d)(ref val24)).X, ((Point3d)(ref val24)).Y, 0.0);
		new Point3d(0.0 - ((Point3d)(ref outpoint10)).X, ((Point3d)(ref outpoint10)).Y, 0.0);
		new Point3d(0.0 - ((Point3d)(ref outpoint9)).X, ((Point3d)(ref outpoint9)).Y, 0.0);
		new Point3d(0.0 - ((Point3d)(ref outpoint8)).X, ((Point3d)(ref outpoint8)).Y, 0.0);
		new Point3d(0.0 - ((Point3d)(ref outpoint7)).X, ((Point3d)(ref outpoint7)).Y, 0.0);
		new Point3d(0.0 - ((Point3d)(ref outpoint6)).X, ((Point3d)(ref outpoint6)).Y, 0.0);
		new Point3d(0.0 - ((Point3d)(ref outpoint5)).X, ((Point3d)(ref outpoint5)).Y, 0.0);
		new Point3d(0.0 - ((Point3d)(ref outpoint4)).X, ((Point3d)(ref outpoint4)).Y, 0.0);
		new Point3d(0.0 - ((Point3d)(ref outpoint3)).X, ((Point3d)(ref outpoint3)).Y, 0.0);
		new Point3d(0.0 - ((Point3d)(ref outpoint2)).X, ((Point3d)(ref outpoint2)).Y, 0.0);
		Point3d val46 = default(Point3d);
		((Point3d)(ref val46))._002Ector(0.0 - ((Point3d)(ref outpoint)).X, ((Point3d)(ref outpoint)).Y, 0.0);
		Arc val47 = default(Arc);
		((Arc)(ref val47))._002Ector(val46, val34, val35);
		NurbsCurve val48 = ((Arc)(ref val47)).ToNurbsCurve();
		Interval domain = ((Curve)val48).Domain;
		double num3 = ((Interval)(ref domain)).Max / 2.0;
		domain = ((Curve)val48).Domain;
		Curve val49 = ((Curve)val48).Trim(new Interval(num3, ((Interval)(ref domain)).Max));
		domain = ((Curve)val48).Domain;
		double min = ((Interval)(ref domain)).Min;
		domain = ((Curve)val48).Domain;
		Curve val50 = ((Curve)val48).Trim(new Interval(min, ((Interval)(ref domain)).Max / 2.0));
		((Arc)(ref val47))._002Ector(val35, val36, val37);
		NurbsCurve val51 = ((Arc)(ref val47)).ToNurbsCurve();
		((Arc)(ref val47))._002Ector(val37, val38, val39);
		NurbsCurve val52 = ((Arc)(ref val47)).ToNurbsCurve();
		((Arc)(ref val47))._002Ector(val39, val40, val41);
		NurbsCurve val53 = ((Arc)(ref val47)).ToNurbsCurve();
		((Arc)(ref val47))._002Ector(val41, val42, val43);
		NurbsCurve val54 = ((Arc)(ref val47)).ToNurbsCurve();
		((Arc)(ref val47))._002Ector(val43, val44, val45);
		NurbsCurve val55 = ((Arc)(ref val47)).ToNurbsCurve();
		Transform val56 = Transform.Mirror(Plane.WorldYZ);
		NurbsCurve val57 = ((Curve)val51).ToNurbsCurve();
		((GeometryBase)val51).Transform(val56);
		((Curve)val51).Reverse();
		NurbsCurve val58 = ((Curve)val52).ToNurbsCurve();
		((GeometryBase)val58).Transform(val56);
		((Curve)val58).Reverse();
		NurbsCurve val59 = ((Curve)val53).ToNurbsCurve();
		((GeometryBase)val59).Transform(val56);
		((Curve)val59).Reverse();
		NurbsCurve val60 = ((Curve)val54).ToNurbsCurve();
		((GeometryBase)val60).Transform(val56);
		((Curve)val60).Reverse();
		NurbsCurve val61 = ((Curve)val55).ToNurbsCurve();
		((GeometryBase)val61).Transform(val56);
		((Curve)val61).Reverse();
		if (cornerBlend > 0.0)
		{
			TrimCurve((Curve)(object)val57, cornerBlend, (CurveEnd)2, out var crvTrimmed);
			TrimCurve((Curve)(object)val52, cornerBlend, (CurveEnd)1, out var crvTrimmed2);
			TrimCurve((Curve)(object)val55, cornerBlend, (CurveEnd)2, out var crvTrimmed3);
			TrimCurve((Curve)(object)val61, cornerBlend, (CurveEnd)1, out var crvTrimmed4);
			TrimCurve((Curve)(object)val58, cornerBlend, (CurveEnd)2, out var crvTrimmed5);
			TrimCurve((Curve)(object)val51, cornerBlend, (CurveEnd)1, out var crvTrimmed6);
			BlendCorner(ref crvTrimmed, ref crvTrimmed2);
			BlendCorner(ref crvTrimmed3, ref crvTrimmed4);
			BlendCorner(ref crvTrimmed5, ref crvTrimmed6);
			val57 = crvTrimmed.ToNurbsCurve();
			val52 = crvTrimmed2.ToNurbsCurve();
			val55 = crvTrimmed3.ToNurbsCurve();
			val61 = crvTrimmed4.ToNurbsCurve();
			val58 = crvTrimmed5.ToNurbsCurve();
			val51 = crvTrimmed6.ToNurbsCurve();
		}
		return (Curve[])(object)new Curve[12]
		{
			val49,
			(Curve)val57,
			(Curve)val52,
			(Curve)val53,
			(Curve)val54,
			(Curve)val55,
			(Curve)val61,
			(Curve)val60,
			(Curve)val59,
			(Curve)val58,
			(Curve)val51,
			val50
		};
	}

	private static Curve[] GemOffsetCurvesTrillionStraight(CSGemData gem, double distance, double cornerBlend = 0.0)
	{
		//IL_0285: Unknown result type (might be due to invalid IL or missing references)
		//IL_028d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0296: Unknown result type (might be due to invalid IL or missing references)
		//IL_029f: Unknown result type (might be due to invalid IL or missing references)
		//IL_02a8: Unknown result type (might be due to invalid IL or missing references)
		//IL_02b1: Unknown result type (might be due to invalid IL or missing references)
		//IL_02ba: Unknown result type (might be due to invalid IL or missing references)
		//IL_02c3: Unknown result type (might be due to invalid IL or missing references)
		//IL_02d5: Unknown result type (might be due to invalid IL or missing references)
		//IL_02eb: Unknown result type (might be due to invalid IL or missing references)
		//IL_0301: Unknown result type (might be due to invalid IL or missing references)
		//IL_0317: Unknown result type (might be due to invalid IL or missing references)
		//IL_032d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0343: Unknown result type (might be due to invalid IL or missing references)
		//IL_0350: Unknown result type (might be due to invalid IL or missing references)
		//IL_0352: Unknown result type (might be due to invalid IL or missing references)
		//IL_0354: Unknown result type (might be due to invalid IL or missing references)
		//IL_0355: Unknown result type (might be due to invalid IL or missing references)
		//IL_035f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0361: Unknown result type (might be due to invalid IL or missing references)
		//IL_0363: Unknown result type (might be due to invalid IL or missing references)
		//IL_0365: Unknown result type (might be due to invalid IL or missing references)
		//IL_036f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0371: Unknown result type (might be due to invalid IL or missing references)
		//IL_0373: Unknown result type (might be due to invalid IL or missing references)
		//IL_0375: Unknown result type (might be due to invalid IL or missing references)
		//IL_037f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0381: Unknown result type (might be due to invalid IL or missing references)
		//IL_0383: Unknown result type (might be due to invalid IL or missing references)
		//IL_0385: Unknown result type (might be due to invalid IL or missing references)
		//IL_038f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0391: Unknown result type (might be due to invalid IL or missing references)
		//IL_0393: Unknown result type (might be due to invalid IL or missing references)
		//IL_0395: Unknown result type (might be due to invalid IL or missing references)
		//IL_050a: Unknown result type (might be due to invalid IL or missing references)
		//IL_050c: Unknown result type (might be due to invalid IL or missing references)
		//IL_050e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0515: Expected O, but got Unknown
		//IL_0515: Unknown result type (might be due to invalid IL or missing references)
		//IL_0517: Unknown result type (might be due to invalid IL or missing references)
		//IL_0519: Unknown result type (might be due to invalid IL or missing references)
		//IL_0520: Expected O, but got Unknown
		//IL_0520: Unknown result type (might be due to invalid IL or missing references)
		//IL_0522: Unknown result type (might be due to invalid IL or missing references)
		//IL_0524: Unknown result type (might be due to invalid IL or missing references)
		//IL_052b: Expected O, but got Unknown
		//IL_052b: Unknown result type (might be due to invalid IL or missing references)
		//IL_052d: Unknown result type (might be due to invalid IL or missing references)
		//IL_052f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0536: Expected O, but got Unknown
		//IL_0536: Unknown result type (might be due to invalid IL or missing references)
		//IL_0538: Unknown result type (might be due to invalid IL or missing references)
		//IL_053a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0541: Expected O, but got Unknown
		//IL_0541: Unknown result type (might be due to invalid IL or missing references)
		//IL_0543: Unknown result type (might be due to invalid IL or missing references)
		//IL_0545: Unknown result type (might be due to invalid IL or missing references)
		//IL_054c: Expected O, but got Unknown
		//IL_054c: Unknown result type (might be due to invalid IL or missing references)
		//IL_054e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0550: Unknown result type (might be due to invalid IL or missing references)
		//IL_0557: Expected O, but got Unknown
		//IL_0557: Unknown result type (might be due to invalid IL or missing references)
		//IL_0559: Unknown result type (might be due to invalid IL or missing references)
		//IL_055b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0562: Expected O, but got Unknown
		//IL_0562: Unknown result type (might be due to invalid IL or missing references)
		//IL_0564: Unknown result type (might be due to invalid IL or missing references)
		//IL_0566: Unknown result type (might be due to invalid IL or missing references)
		//IL_056d: Expected O, but got Unknown
		//IL_056d: Unknown result type (might be due to invalid IL or missing references)
		//IL_056f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0571: Unknown result type (might be due to invalid IL or missing references)
		//IL_0578: Expected O, but got Unknown
		//IL_0578: Unknown result type (might be due to invalid IL or missing references)
		//IL_057a: Unknown result type (might be due to invalid IL or missing references)
		//IL_057c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0583: Expected O, but got Unknown
		//IL_0583: Unknown result type (might be due to invalid IL or missing references)
		//IL_0585: Unknown result type (might be due to invalid IL or missing references)
		//IL_0587: Unknown result type (might be due to invalid IL or missing references)
		//IL_058e: Expected O, but got Unknown
		double num = gem.GemXSize / 2.0 + distance * 1.4;
		double num2 = gem.GemYSize / 2.0 + distance * 1.4;
		Point3d val = default(Point3d);
		((Point3d)(ref val))._002Ector(0.0, num2 * 0.7576, 0.0);
		Point3d point = default(Point3d);
		((Point3d)(ref point))._002Ector(num * 0.7708, num2 * 0.7378, 0.0);
		Point3d point2 = default(Point3d);
		((Point3d)(ref point2))._002Ector(num, num2 * 0.6212, 0.0);
		Point3d point3 = default(Point3d);
		((Point3d)(ref point3))._002Ector(num * 0.9792, num2 * 0.3493, 0.0);
		Point3d point4 = default(Point3d);
		((Point3d)(ref point4))._002Ector(num * 0.6103, (0.0 - num2) * 0.378, 0.0);
		Point3d val2 = default(Point3d);
		((Point3d)(ref val2))._002Ector(num * 0.2085, (0.0 - num2) * 1.0871, 0.0);
		Point3d val3 = default(Point3d);
		((Point3d)(ref val3))._002Ector(0.0, (0.0 - num2) * 1.2424, 0.0);
		Vector3d val4 = default(Vector3d);
		((Vector3d)(ref val4))._002Ector(((Point3d)(ref val)).Y - ((Point3d)(ref point)).Y, 0.0 - (((Point3d)(ref val)).X - ((Point3d)(ref point)).X), 0.0);
		((Vector3d)(ref val4)).Unitize();
		Vector3d val5 = default(Vector3d);
		((Vector3d)(ref val5))._002Ector(((Point3d)(ref point)).Y - ((Point3d)(ref point2)).Y, 0.0 - (((Point3d)(ref point)).X - ((Point3d)(ref point2)).X), 0.0);
		((Vector3d)(ref val5)).Unitize();
		Vector3d val6 = default(Vector3d);
		((Vector3d)(ref val6))._002Ector(((Point3d)(ref point2)).Y - ((Point3d)(ref point3)).Y, 0.0 - (((Point3d)(ref point2)).X - ((Point3d)(ref point3)).X), 0.0);
		((Vector3d)(ref val6)).Unitize();
		Vector3d val7 = default(Vector3d);
		((Vector3d)(ref val7))._002Ector(((Point3d)(ref point3)).Y - ((Point3d)(ref point4)).Y, 0.0 - (((Point3d)(ref point3)).X - ((Point3d)(ref point4)).X), 0.0);
		((Vector3d)(ref val7)).Unitize();
		Vector3d val8 = default(Vector3d);
		((Vector3d)(ref val8))._002Ector(((Point3d)(ref point4)).Y - ((Point3d)(ref val2)).Y, 0.0 - (((Point3d)(ref point4)).X - ((Point3d)(ref val2)).X), 0.0);
		((Vector3d)(ref val8)).Unitize();
		Vector3d val9 = default(Vector3d);
		((Vector3d)(ref val9))._002Ector(((Point3d)(ref val2)).Y - ((Point3d)(ref val3)).Y, 0.0 - (((Point3d)(ref val2)).X - ((Point3d)(ref val3)).X), 0.0);
		((Vector3d)(ref val9)).Unitize();
		Point3d point5 = default(Point3d);
		((Point3d)(ref point5))._002Ector(val);
		Point3d point6 = default(Point3d);
		((Point3d)(ref point6))._002Ector(val3);
		Vector3d vector = default(Vector3d);
		((Vector3d)(ref vector))._002Ector(val4);
		Vector3d val10 = default(Vector3d);
		((Vector3d)(ref val10))._002Ector(val5);
		Vector3d val11 = default(Vector3d);
		((Vector3d)(ref val11))._002Ector(val6);
		Vector3d val12 = default(Vector3d);
		((Vector3d)(ref val12))._002Ector(val7);
		Vector3d val13 = default(Vector3d);
		((Vector3d)(ref val13))._002Ector(val8);
		Vector3d vector2 = default(Vector3d);
		((Vector3d)(ref vector2))._002Ector(val9);
		((Vector3d)(ref vector)).Rotate(Math.PI / 2.0, Vector3d.ZAxis);
		((Vector3d)(ref val10)).Rotate(Math.PI / 2.0, Vector3d.ZAxis);
		((Vector3d)(ref val11)).Rotate(Math.PI / 2.0, Vector3d.ZAxis);
		((Vector3d)(ref val12)).Rotate(Math.PI / 2.0, Vector3d.ZAxis);
		((Vector3d)(ref val13)).Rotate(Math.PI / 2.0, Vector3d.ZAxis);
		((Vector3d)(ref vector2)).Rotate(Math.PI / 2.0, Vector3d.ZAxis);
		CSBlockUtil.CSUtilMath.LineInt(out var outpoint, point5, vector, point, val10);
		CSBlockUtil.CSUtilMath.LineInt(out var outpoint2, outpoint, val10, point2, val11);
		CSBlockUtil.CSUtilMath.LineInt(out var outpoint3, outpoint2, val11, point3, val12);
		CSBlockUtil.CSUtilMath.LineInt(out var outpoint4, outpoint3, val12, point4, val13);
		CSBlockUtil.CSUtilMath.LineInt(out var outpoint5, outpoint4, val13, point6, vector2);
		Point3d val14 = default(Point3d);
		((Point3d)(ref val14))._002Ector(((Point3d)(ref point5)).X, ((Point3d)(ref point5)).Y, 0.0);
		Point3d val15 = default(Point3d);
		((Point3d)(ref val15))._002Ector(((Point3d)(ref outpoint)).X, ((Point3d)(ref outpoint)).Y, 0.0);
		Point3d val16 = default(Point3d);
		((Point3d)(ref val16))._002Ector(((Point3d)(ref outpoint2)).X, ((Point3d)(ref outpoint2)).Y, 0.0);
		Point3d val17 = default(Point3d);
		((Point3d)(ref val17))._002Ector(((Point3d)(ref outpoint3)).X, ((Point3d)(ref outpoint3)).Y, 0.0);
		Point3d val18 = default(Point3d);
		((Point3d)(ref val18))._002Ector(((Point3d)(ref outpoint4)).X, ((Point3d)(ref outpoint4)).Y, 0.0);
		Point3d val19 = default(Point3d);
		((Point3d)(ref val19))._002Ector(((Point3d)(ref outpoint5)).X, ((Point3d)(ref outpoint5)).Y, 0.0);
		Point3d val20 = default(Point3d);
		((Point3d)(ref val20))._002Ector(((Point3d)(ref point6)).X, ((Point3d)(ref point6)).Y, 0.0);
		Point3d val21 = default(Point3d);
		((Point3d)(ref val21))._002Ector(0.0 - ((Point3d)(ref outpoint5)).X, ((Point3d)(ref outpoint5)).Y, 0.0);
		Point3d val22 = default(Point3d);
		((Point3d)(ref val22))._002Ector(0.0 - ((Point3d)(ref outpoint4)).X, ((Point3d)(ref outpoint4)).Y, 0.0);
		Point3d val23 = default(Point3d);
		((Point3d)(ref val23))._002Ector(0.0 - ((Point3d)(ref outpoint3)).X, ((Point3d)(ref outpoint3)).Y, 0.0);
		Point3d val24 = default(Point3d);
		((Point3d)(ref val24))._002Ector(0.0 - ((Point3d)(ref outpoint2)).X, ((Point3d)(ref outpoint2)).Y, 0.0);
		Point3d val25 = default(Point3d);
		((Point3d)(ref val25))._002Ector(0.0 - ((Point3d)(ref outpoint)).X, ((Point3d)(ref outpoint)).Y, 0.0);
		Curve val26 = (Curve)new LineCurve(val14, val15);
		Curve val27 = (Curve)new LineCurve(val15, val16);
		Curve val28 = (Curve)new LineCurve(val16, val17);
		Curve val29 = (Curve)new LineCurve(val17, val18);
		Curve val30 = (Curve)new LineCurve(val18, val19);
		Curve val31 = (Curve)new LineCurve(val19, val20);
		Curve val32 = (Curve)new LineCurve(val20, val21);
		Curve val33 = (Curve)new LineCurve(val21, val22);
		Curve val34 = (Curve)new LineCurve(val22, val23);
		Curve val35 = (Curve)new LineCurve(val23, val24);
		Curve val36 = (Curve)new LineCurve(val24, val25);
		Curve val37 = (Curve)new LineCurve(val25, val14);
		if (cornerBlend > 0.0)
		{
			TrimCurve(val26, cornerBlend, (CurveEnd)2, out var crvTrimmed);
			TrimCurve(val27, cornerBlend, (CurveEnd)3, out var crvTrimmed2);
			TrimCurve(val28, cornerBlend, (CurveEnd)3, out var crvTrimmed3);
			TrimCurve(val29, cornerBlend, (CurveEnd)3, out var crvTrimmed4);
			TrimCurve(val30, cornerBlend, (CurveEnd)3, out var crvTrimmed5);
			TrimCurve(val31, cornerBlend, (CurveEnd)3, out var crvTrimmed6);
			TrimCurve(val32, cornerBlend, (CurveEnd)3, out var crvTrimmed7);
			TrimCurve(val33, cornerBlend, (CurveEnd)3, out var crvTrimmed8);
			TrimCurve(val34, cornerBlend, (CurveEnd)3, out var crvTrimmed9);
			TrimCurve(val35, cornerBlend, (CurveEnd)3, out var crvTrimmed10);
			TrimCurve(val36, cornerBlend, (CurveEnd)3, out var crvTrimmed11);
			TrimCurve(val37, cornerBlend, (CurveEnd)1, out var crvTrimmed12);
			BlendCorner(ref crvTrimmed, ref crvTrimmed2);
			BlendCorner(ref crvTrimmed2, ref crvTrimmed3);
			BlendCorner(ref crvTrimmed3, ref crvTrimmed4);
			BlendCorner(ref crvTrimmed4, ref crvTrimmed5);
			BlendCorner(ref crvTrimmed5, ref crvTrimmed6);
			BlendCorner(ref crvTrimmed6, ref crvTrimmed7);
			BlendCorner(ref crvTrimmed7, ref crvTrimmed8);
			BlendCorner(ref crvTrimmed8, ref crvTrimmed9);
			BlendCorner(ref crvTrimmed9, ref crvTrimmed10);
			BlendCorner(ref crvTrimmed10, ref crvTrimmed11);
			BlendCorner(ref crvTrimmed11, ref crvTrimmed12);
			val26 = crvTrimmed;
			val27 = crvTrimmed2;
			val28 = crvTrimmed3;
			val29 = crvTrimmed4;
			val30 = crvTrimmed5;
			val31 = crvTrimmed6;
			val32 = crvTrimmed7;
			val33 = crvTrimmed8;
			val34 = crvTrimmed9;
			val35 = crvTrimmed10;
			val36 = crvTrimmed11;
			val37 = crvTrimmed12;
		}
		return (Curve[])(object)new Curve[12]
		{
			val26, val27, val28, val29, val30, val31, val32, val33, val34, val35,
			val36, val37
		};
	}

	private static bool TrimCurve(Curve crv, double trimDistance, CurveEnd whichCurveEnd, out Curve crvTrimmed)
	{
		//IL_0038: Unknown result type (might be due to invalid IL or missing references)
		//IL_003c: Unknown result type (might be due to invalid IL or missing references)
		crvTrimmed = null;
		if (crv == null)
		{
			return false;
		}
		double length = crv.GetLength();
		if (trimDistance * 2.0 > length)
		{
			trimDistance = (length - CSUtil.Tolerance) / 2.0;
		}
		Curve val = crv.DuplicateCurve();
		crvTrimmed = (((int)whichCurveEnd == 0) ? val : val.Trim(whichCurveEnd, trimDistance));
		return true;
	}
}
