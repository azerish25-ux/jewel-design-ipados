using System;
using System.Collections.Generic;
using CSBlockUtil;
using CSCompatibility;
using CSDatas;
using RMA.OpenNURBS;
using Rhino.Geometry;

namespace CSBlockDataUtils.CSGemOffsetCurves;

[Obsolete]
public static class GemOffsetCurveV1
{
	[Obsolete]
	public static bool GemOffset(CSGemData gem, double distance, out OnCurve curve, bool doTransformUp, out string error)
	{
		Curve curve2;
		bool result = GemOffset(gem, distance, out curve2, doTransformUp, out error);
		curve = curve2.Convert();
		return result;
	}

	public static bool GemOffset(CSGemData gem, double distance, out Curve curve, bool doTransformUp, out string error)
	{
		return GemOffset(gem.GemShape, gem, distance, out curve, doTransformUp, out error);
	}

	[Obsolete]
	public static bool GemOffset(string shape, CSGemData gem, double distance, out OnCurve curve, bool doTransformUp, out string error)
	{
		Curve curve2;
		bool result = GemOffset(shape, gem, distance, out curve2, doTransformUp, out error);
		curve = curve2.Convert();
		return result;
	}

	public static bool GemOffset(string shape, CSGemData gem, double distance, out Curve curve, bool doTransformUp, out string error)
	{
		curve = null;
		error = string.Empty;
		Curve[] array = GemOffsetCurves(shape, gem, distance, doTransformUp);
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

	[Obsolete]
	public static OnCurve[] GemOffsetCurvesOld(string shape, CSGemData gem, double distance, bool doTransformUp)
	{
		Curve[] array = GemOffsetCurves(shape, gem, distance, doTransformUp);
		OnCurve[] array2 = (OnCurve[])(object)new OnCurve[array.Length];
		for (int i = 0; i < array2.Length; i++)
		{
			array2[i] = array[i].Convert();
		}
		return array2;
	}

	public static Curve[] GemOffsetCurves(string shape, CSGemData gem, double distance, bool doTransformUp)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_0205: Unknown result type (might be due to invalid IL or missing references)
		//IL_0231: Unknown result type (might be due to invalid IL or missing references)
		CSBlockUtil.CSUtilMath.GetUnscaledXforms(gem.Xform, out var xUp, out var xDown);
		Curve[] array;
		switch (shape)
		{
		case "Round":
		case "Oval":
			array = GemOffsetCurvesRound(gem, distance);
			break;
		case "Baguette Straight":
		case "Princess":
			array = GemOffsetCurvesSquare(gem, distance);
			break;
		case "Cushion":
			array = GemOffsetCurvesCushion(gem, distance);
			break;
		case "Emerald":
		case "Radiant":
			array = GemOffsetCurvesEmerald(gem, distance);
			break;
		case "Emerald Square":
			array = GemOffsetCurvesEmeraldSquare(gem, distance);
			break;
		case "Heart":
			array = GemOffsetCurvesHeart(gem, distance);
			break;
		case "Marquise":
			array = GemOffsetCurvesMarquise(gem, distance);
			break;
		case "Pear":
			array = GemOffsetCurvesPear(gem, distance);
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

	private static Curve[] GemOffsetCurvesCushion(CSGemData gem, double distance)
	{
		//IL_00aa: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ac: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ad: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b6: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ba: Unknown result type (might be due to invalid IL or missing references)
		//IL_00bf: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d3: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d6: Unknown result type (might be due to invalid IL or missing references)
		//IL_00db: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ef: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f3: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f8: Unknown result type (might be due to invalid IL or missing references)
		//IL_010c: Unknown result type (might be due to invalid IL or missing references)
		//IL_010e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0111: Unknown result type (might be due to invalid IL or missing references)
		//IL_0116: Unknown result type (might be due to invalid IL or missing references)
		//IL_011b: Unknown result type (might be due to invalid IL or missing references)
		//IL_011c: Unknown result type (might be due to invalid IL or missing references)
		//IL_011f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0124: Unknown result type (might be due to invalid IL or missing references)
		//IL_0129: Unknown result type (might be due to invalid IL or missing references)
		//IL_012b: Unknown result type (might be due to invalid IL or missing references)
		//IL_012e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0133: Unknown result type (might be due to invalid IL or missing references)
		//IL_01af: Unknown result type (might be due to invalid IL or missing references)
		//IL_01b1: Unknown result type (might be due to invalid IL or missing references)
		//IL_01b3: Unknown result type (might be due to invalid IL or missing references)
		//IL_01bc: Unknown result type (might be due to invalid IL or missing references)
		//IL_01c0: Unknown result type (might be due to invalid IL or missing references)
		//IL_01c5: Unknown result type (might be due to invalid IL or missing references)
		//IL_01d9: Unknown result type (might be due to invalid IL or missing references)
		//IL_01dd: Unknown result type (might be due to invalid IL or missing references)
		//IL_01e2: Unknown result type (might be due to invalid IL or missing references)
		//IL_01f6: Unknown result type (might be due to invalid IL or missing references)
		//IL_01fa: Unknown result type (might be due to invalid IL or missing references)
		//IL_01ff: Unknown result type (might be due to invalid IL or missing references)
		//IL_0213: Unknown result type (might be due to invalid IL or missing references)
		//IL_0215: Unknown result type (might be due to invalid IL or missing references)
		//IL_0218: Unknown result type (might be due to invalid IL or missing references)
		//IL_021d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0222: Unknown result type (might be due to invalid IL or missing references)
		//IL_0224: Unknown result type (might be due to invalid IL or missing references)
		//IL_0227: Unknown result type (might be due to invalid IL or missing references)
		//IL_022c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0231: Unknown result type (might be due to invalid IL or missing references)
		//IL_0233: Unknown result type (might be due to invalid IL or missing references)
		//IL_0236: Unknown result type (might be due to invalid IL or missing references)
		//IL_023b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0247: Unknown result type (might be due to invalid IL or missing references)
		//IL_024c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0257: Unknown result type (might be due to invalid IL or missing references)
		//IL_025c: Unknown result type (might be due to invalid IL or missing references)
		//IL_028a: Unknown result type (might be due to invalid IL or missing references)
		//IL_02a3: Unknown result type (might be due to invalid IL or missing references)
		//IL_0269: Unknown result type (might be due to invalid IL or missing references)
		//IL_026e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0279: Unknown result type (might be due to invalid IL or missing references)
		//IL_027e: Unknown result type (might be due to invalid IL or missing references)
		//IL_02be: Unknown result type (might be due to invalid IL or missing references)
		//IL_02de: Unknown result type (might be due to invalid IL or missing references)
		//IL_02e3: Unknown result type (might be due to invalid IL or missing references)
		//IL_02e5: Unknown result type (might be due to invalid IL or missing references)
		//IL_02e7: Unknown result type (might be due to invalid IL or missing references)
		//IL_02eb: Unknown result type (might be due to invalid IL or missing references)
		//IL_02f3: Unknown result type (might be due to invalid IL or missing references)
		//IL_02f5: Unknown result type (might be due to invalid IL or missing references)
		//IL_02f9: Unknown result type (might be due to invalid IL or missing references)
		//IL_0303: Unknown result type (might be due to invalid IL or missing references)
		//IL_030a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0313: Unknown result type (might be due to invalid IL or missing references)
		//IL_0318: Unknown result type (might be due to invalid IL or missing references)
		//IL_0321: Unknown result type (might be due to invalid IL or missing references)
		//IL_032a: Unknown result type (might be due to invalid IL or missing references)
		//IL_032f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0338: Unknown result type (might be due to invalid IL or missing references)
		//IL_033d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0367: Unknown result type (might be due to invalid IL or missing references)
		//IL_036e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0377: Unknown result type (might be due to invalid IL or missing references)
		//IL_037c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0385: Unknown result type (might be due to invalid IL or missing references)
		//IL_038e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0393: Unknown result type (might be due to invalid IL or missing references)
		//IL_039c: Unknown result type (might be due to invalid IL or missing references)
		//IL_03a1: Unknown result type (might be due to invalid IL or missing references)
		//IL_03cb: Unknown result type (might be due to invalid IL or missing references)
		//IL_03d2: Unknown result type (might be due to invalid IL or missing references)
		//IL_03db: Unknown result type (might be due to invalid IL or missing references)
		//IL_03e0: Unknown result type (might be due to invalid IL or missing references)
		//IL_03e9: Unknown result type (might be due to invalid IL or missing references)
		//IL_03f2: Unknown result type (might be due to invalid IL or missing references)
		//IL_03f7: Unknown result type (might be due to invalid IL or missing references)
		//IL_0400: Unknown result type (might be due to invalid IL or missing references)
		//IL_0405: Unknown result type (might be due to invalid IL or missing references)
		//IL_042f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0436: Unknown result type (might be due to invalid IL or missing references)
		//IL_043f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0444: Unknown result type (might be due to invalid IL or missing references)
		//IL_044d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0456: Unknown result type (might be due to invalid IL or missing references)
		//IL_045b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0464: Unknown result type (might be due to invalid IL or missing references)
		//IL_0469: Unknown result type (might be due to invalid IL or missing references)
		//IL_0491: Unknown result type (might be due to invalid IL or missing references)
		//IL_0493: Unknown result type (might be due to invalid IL or missing references)
		//IL_049a: Expected O, but got Unknown
		//IL_049a: Unknown result type (might be due to invalid IL or missing references)
		//IL_049c: Unknown result type (might be due to invalid IL or missing references)
		//IL_04a3: Expected O, but got Unknown
		//IL_04a7: Unknown result type (might be due to invalid IL or missing references)
		//IL_04ac: Unknown result type (might be due to invalid IL or missing references)
		//IL_04b7: Unknown result type (might be due to invalid IL or missing references)
		//IL_04bc: Unknown result type (might be due to invalid IL or missing references)
		//IL_04c5: Unknown result type (might be due to invalid IL or missing references)
		//IL_04d5: Unknown result type (might be due to invalid IL or missing references)
		//IL_04da: Unknown result type (might be due to invalid IL or missing references)
		//IL_04e5: Unknown result type (might be due to invalid IL or missing references)
		//IL_04ea: Unknown result type (might be due to invalid IL or missing references)
		//IL_04f3: Unknown result type (might be due to invalid IL or missing references)
		//IL_0512: Unknown result type (might be due to invalid IL or missing references)
		//IL_0514: Unknown result type (might be due to invalid IL or missing references)
		//IL_051a: Expected O, but got Unknown
		//IL_0521: Unknown result type (might be due to invalid IL or missing references)
		//IL_0523: Unknown result type (might be due to invalid IL or missing references)
		//IL_0529: Expected O, but got Unknown
		//IL_0530: Unknown result type (might be due to invalid IL or missing references)
		//IL_0532: Unknown result type (might be due to invalid IL or missing references)
		//IL_0538: Expected O, but got Unknown
		double num = gem.GemXSize / 2.0;
		double num2 = gem.GemYSize / 2.0;
		double num3 = num / 4.0;
		double num4 = num2 / 5.0;
		Point3d val = default(Point3d);
		((Point3d)(ref val))._002Ector(0.0, 5.0 * num4, 0.0);
		Point3d val2 = default(Point3d);
		((Point3d)(ref val2))._002Ector(-2.18319 * num3, 4.54508 * num4, 0.0);
		Point3d val3 = default(Point3d);
		((Point3d)(ref val3))._002Ector(2.18319 * num3, 4.54508 * num4, 0.0);
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
		((Arc)(ref val8))._002Ector(val2 + val5 * distance, val + val6 * distance, val3 + val7 * distance);
		Point3d val9 = default(Point3d);
		((Point3d)(ref val9))._002Ector(4.0 * num3, 0.0, 0.0);
		Point3d val10 = default(Point3d);
		((Point3d)(ref val10))._002Ector(3.63684 * num3, 2.72661 * num4, 0.0);
		Point3d val11 = default(Point3d);
		((Point3d)(ref val11))._002Ector(3.63684 * num3, -2.72661 * num4, 0.0);
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
		((Arc)(ref val16))._002Ector(val10 + val13 * distance, val9 + val14 * distance, val11 + val15 * distance);
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
		CSBlockUtil.CSUtilMath.GetUnscaledXforms(gem.Xform, out var _, out var xDown);
		NurbsCurve nurbsCurve = gem.GetNurbsCurve();
		((GeometryBase)nurbsCurve).Transform(xDown);
		return (Curve[])(object)new Curve[1] { CSUtilGeometry.OriginOffsetOldXY((Curve)(object)nurbsCurve, distance, closed: true) };
	}

	private static Curve[] GemOffsetCurvesEmerald(CSGemData gem, double distance, double cornerPercentX = 0.769277, double cornerPercentY = 0.8462)
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
		//IL_03f6: Unknown result type (might be due to invalid IL or missing references)
		//IL_03f8: Unknown result type (might be due to invalid IL or missing references)
		//IL_03fa: Unknown result type (might be due to invalid IL or missing references)
		//IL_0400: Expected O, but got Unknown
		//IL_0402: Unknown result type (might be due to invalid IL or missing references)
		//IL_0404: Unknown result type (might be due to invalid IL or missing references)
		//IL_0406: Unknown result type (might be due to invalid IL or missing references)
		//IL_040c: Expected O, but got Unknown
		//IL_040e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0410: Unknown result type (might be due to invalid IL or missing references)
		//IL_0412: Unknown result type (might be due to invalid IL or missing references)
		//IL_0418: Expected O, but got Unknown
		//IL_041a: Unknown result type (might be due to invalid IL or missing references)
		//IL_041c: Unknown result type (might be due to invalid IL or missing references)
		//IL_041e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0424: Expected O, but got Unknown
		//IL_0426: Unknown result type (might be due to invalid IL or missing references)
		//IL_0428: Unknown result type (might be due to invalid IL or missing references)
		//IL_042a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0430: Expected O, but got Unknown
		//IL_0432: Unknown result type (might be due to invalid IL or missing references)
		//IL_0434: Unknown result type (might be due to invalid IL or missing references)
		//IL_0436: Unknown result type (might be due to invalid IL or missing references)
		//IL_043c: Expected O, but got Unknown
		//IL_043e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0440: Unknown result type (might be due to invalid IL or missing references)
		//IL_0442: Unknown result type (might be due to invalid IL or missing references)
		//IL_0448: Expected O, but got Unknown
		//IL_044a: Unknown result type (might be due to invalid IL or missing references)
		//IL_044c: Unknown result type (might be due to invalid IL or missing references)
		//IL_044e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0454: Expected O, but got Unknown
		//IL_0456: Unknown result type (might be due to invalid IL or missing references)
		//IL_0458: Unknown result type (might be due to invalid IL or missing references)
		//IL_045a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0460: Expected O, but got Unknown
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
		return (Curve[])(object)new Curve[9]
		{
			(Curve)new LineCurve(val11, val12),
			(Curve)new LineCurve(val12, val13),
			(Curve)new LineCurve(val13, val14),
			(Curve)new LineCurve(val14, val15),
			(Curve)new LineCurve(val15, val16),
			(Curve)new LineCurve(val16, val17),
			(Curve)new LineCurve(val17, val18),
			(Curve)new LineCurve(val18, val19),
			(Curve)new LineCurve(val19, val11)
		};
	}

	private static Curve[] GemOffsetCurvesEmeraldSquare(CSGemData gem, double distance)
	{
		return GemOffsetCurvesEmerald(gem, distance, 0.7688, 0.7688);
	}

	private static Curve[] GemOffsetCurvesHeart(CSGemData gem, double distance)
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
		//IL_05b4: Unknown result type (might be due to invalid IL or missing references)
		//IL_05b9: Unknown result type (might be due to invalid IL or missing references)
		//IL_05bb: Unknown result type (might be due to invalid IL or missing references)
		//IL_05e1: Unknown result type (might be due to invalid IL or missing references)
		//IL_05e6: Unknown result type (might be due to invalid IL or missing references)
		//IL_05ea: Unknown result type (might be due to invalid IL or missing references)
		//IL_05eb: Unknown result type (might be due to invalid IL or missing references)
		//IL_05ed: Unknown result type (might be due to invalid IL or missing references)
		//IL_05f2: Unknown result type (might be due to invalid IL or missing references)
		//IL_05f4: Unknown result type (might be due to invalid IL or missing references)
		//IL_0603: Unknown result type (might be due to invalid IL or missing references)
		//IL_060a: Expected O, but got Unknown
		//IL_0636: Unknown result type (might be due to invalid IL or missing references)
		//IL_066c: Unknown result type (might be due to invalid IL or missing references)
		//IL_06a2: Unknown result type (might be due to invalid IL or missing references)
		//IL_06d8: Unknown result type (might be due to invalid IL or missing references)
		//IL_07b5: Unknown result type (might be due to invalid IL or missing references)
		//IL_07ba: Unknown result type (might be due to invalid IL or missing references)
		//IL_07bc: Unknown result type (might be due to invalid IL or missing references)
		//IL_07d5: Unknown result type (might be due to invalid IL or missing references)
		//IL_07da: Unknown result type (might be due to invalid IL or missing references)
		//IL_07fa: Unknown result type (might be due to invalid IL or missing references)
		//IL_07ff: Unknown result type (might be due to invalid IL or missing references)
		//IL_0803: Unknown result type (might be due to invalid IL or missing references)
		//IL_0804: Unknown result type (might be due to invalid IL or missing references)
		//IL_0806: Unknown result type (might be due to invalid IL or missing references)
		//IL_080b: Unknown result type (might be due to invalid IL or missing references)
		//IL_080d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0812: Unknown result type (might be due to invalid IL or missing references)
		//IL_0814: Unknown result type (might be due to invalid IL or missing references)
		//IL_081f: Unknown result type (might be due to invalid IL or missing references)
		//IL_083f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0844: Unknown result type (might be due to invalid IL or missing references)
		//IL_0851: Unknown result type (might be due to invalid IL or missing references)
		//IL_086c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0887: Unknown result type (might be due to invalid IL or missing references)
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
		if (num9 < 0.04)
		{
			num5 = -2.7953078235558855;
			distance = num5 / num3;
			return GemOffsetCurvesHeart(gem, distance);
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

	private static Curve[] GemOffsetCurvesMarquise(CSGemData gem, double distance)
	{
		//IL_00d0: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d2: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d4: Unknown result type (might be due to invalid IL or missing references)
		//IL_00db: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ff: Unknown result type (might be due to invalid IL or missing references)
		//IL_0104: Unknown result type (might be due to invalid IL or missing references)
		//IL_0111: Unknown result type (might be due to invalid IL or missing references)
		//IL_0119: Unknown result type (might be due to invalid IL or missing references)
		//IL_0139: Unknown result type (might be due to invalid IL or missing references)
		//IL_013e: Unknown result type (might be due to invalid IL or missing references)
		//IL_014b: Unknown result type (might be due to invalid IL or missing references)
		double num = gem.GemXSize / 2.0;
		double num2 = gem.GemYSize / 2.0;
		double num3 = 2.5 / num;
		double num4 = distance * num3;
		double num5 = num4 + 6.25;
		double num6 = Math.Sqrt(num5 * num5 - 14.0625);
		if (num5 < 3.75)
		{
			num4 = -2.46;
			distance = num4 / num3;
			return GemOffsetCurvesMarquise(gem, distance);
		}
		Point3d val = default(Point3d);
		((Point3d)(ref val))._002Ector(0.0, num6, 0.0);
		Point3d val2 = default(Point3d);
		((Point3d)(ref val2))._002Ector(2.5 + num4, 0.0, 0.0);
		Point3d val3 = default(Point3d);
		((Point3d)(ref val3))._002Ector(0.0, 0.0 - num6, 0.0);
		Arc val4 = default(Arc);
		((Arc)(ref val4))._002Ector(val, val2, val3);
		Transform val5 = Transform.Scale(Plane.WorldXY, num / 2.5, num2 / 5.0, 1.0);
		NurbsCurve val6 = ((Arc)(ref val4)).ToNurbsCurve();
		((GeometryBase)val6).Transform(val5);
		Transform val7 = Transform.Scale(Plane.WorldXY, -1.0, -1.0, 1.0);
		NurbsCurve val8 = ((Curve)val6).ToNurbsCurve();
		((GeometryBase)val8).Transform(val7);
		return (Curve[])(object)new Curve[2]
		{
			(Curve)val6,
			(Curve)val8
		};
	}

	private static Curve[] GemOffsetCurvesPear(CSGemData gem, double distance)
	{
		//IL_0157: Unknown result type (might be due to invalid IL or missing references)
		//IL_015e: Expected O, but got Unknown
		//IL_018f: Unknown result type (might be due to invalid IL or missing references)
		//IL_01d9: Unknown result type (might be due to invalid IL or missing references)
		//IL_0223: Unknown result type (might be due to invalid IL or missing references)
		//IL_026d: Unknown result type (might be due to invalid IL or missing references)
		//IL_02b7: Unknown result type (might be due to invalid IL or missing references)
		//IL_0301: Unknown result type (might be due to invalid IL or missing references)
		//IL_034b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0390: Unknown result type (might be due to invalid IL or missing references)
		//IL_0477: Unknown result type (might be due to invalid IL or missing references)
		//IL_0497: Unknown result type (might be due to invalid IL or missing references)
		//IL_049c: Unknown result type (might be due to invalid IL or missing references)
		//IL_04a9: Unknown result type (might be due to invalid IL or missing references)
		double num = gem.GemXSize / 2.0;
		double num2 = gem.GemYSize / 2.0;
		double num3 = num / 3.5;
		double num4 = num2 / 5.0;
		double num5 = 3.5 / num;
		double num6 = 5.0 / num2;
		double num7 = distance * num5;
		double num8 = distance * num6;
		double num9 = 3.5 + num7;
		double num10 = 3.86 + num8;
		double num11 = 8.08108 + num8;
		double num12 = Math.Sqrt(num11 * num11 - 20.742103922500004) - 0.540207;
		if (2.53693 * num9 * num3 < 0.04)
		{
			num9 = 0.04 / num3 / 2.53693;
			num7 = num9 - 3.5;
			distance = num7 / num5 + 0.0001;
			return GemOffsetCurvesPear(gem, distance);
		}
		if ((num10 - 0.329747) * num4 < 0.04)
		{
			num10 = 0.04 / num4 + 0.329747;
			num8 = num10 - 3.86;
			distance = num8 / num6 + 0.0001;
			return GemOffsetCurvesPear(gem, distance);
		}
		NurbsCurve val = new NurbsCurve(3, false, 4, 8);
		val.Points[0] = new ControlPoint(0.0, (num10 - 0.329747) * num4, 0.0, 1.0);
		val.Points[1] = new ControlPoint(0.20354483588156877 * num9 * num3, (0.9940940639751765 * num10 - 0.329747) * num4, 0.0, 1.0);
		val.Points[2] = new ControlPoint(0.6123362632073628 * num9 * num3, (0.8879139319627639 * num10 - 0.329747) * num4, 0.0, 1.0);
		val.Points[3] = new ControlPoint(1.012658540190205 * num9 * num3, (0.3703501564029002 * num10 - 0.329747) * num4, 0.0, 1.0);
		val.Points[4] = new ControlPoint(1.016703337103465 * num9 * num3, (-0.2872952282636081 * num12 - 0.329747) * num4, 0.0, 1.0);
		val.Points[5] = new ControlPoint(0.5443786297392439 * num9 * num3, (-0.7315634760021571 * num12 - 0.329747) * num4, 0.0, 1.0);
		val.Points[6] = new ControlPoint(0.19128191296704555 * num9 * num3, (-0.9245961035017445 * num12 - 0.329747) * num4, 0.0, 1.0);
		val.Points[7] = new ControlPoint(0.0, (-1.0 * num12 - 0.329747) * num4, 0.0, 1.0);
		val.Knots[0] = 0.0;
		val.Knots[1] = 0.0;
		val.Knots[2] = 0.0;
		val.Knots[3] = 0.2;
		val.Knots[4] = 0.4;
		val.Knots[5] = 0.6;
		val.Knots[6] = 0.8;
		val.Knots[7] = 1.0;
		val.Knots[8] = 1.0;
		val.Knots[9] = 1.0;
		Transform val2 = Transform.Scale(Plane.WorldXY, -1.0, 1.0, 1.0);
		NurbsCurve val3 = ((Curve)val).ToNurbsCurve();
		((GeometryBase)val3).Transform(val2);
		((Curve)val3).Reverse();
		return (Curve[])(object)new Curve[2]
		{
			(Curve)val,
			(Curve)val3
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

	private static Curve[] GemOffsetCurvesSquare(CSGemData gem, double distance)
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
}
