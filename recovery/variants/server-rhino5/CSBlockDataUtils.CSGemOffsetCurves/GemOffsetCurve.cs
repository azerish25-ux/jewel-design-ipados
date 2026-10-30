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
		Vector3d vector = new Vector3d(inputVector1);
		Vector3d vector2 = new Vector3d(inputVector2);
		vector.Unitize();
		vector2.Unitize();
		double num = Vector3d.Multiply(vector2, vector);
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
			nurbsCurve.Transform(xDown);
			array = new Curve[1] { CSUtilGeometry.OriginOffsetXY(nurbsCurve, distance) };
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
			array2[i].Transform(xUp);
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
		NurbsCurve nurbsCurve = CSUtilCurves.BlendCurve(crv0, crv1);
		Curve curve = nurbsCurve.Trim(new Interval(nurbsCurve.Domain.Min, nurbsCurve.Domain.Mid));
		if (!CSUtilCurves.MergeCurves((IEnumerable<Curve>)new Curve[2] { crv0, curve }, out Curve mergedCrv, 0.01))
		{
			return false;
		}
		Curve curve2 = nurbsCurve.Trim(new Interval(nurbsCurve.Domain.Mid, nurbsCurve.Domain.Max));
		if (!CSUtilCurves.MergeCurves((IEnumerable<Curve>)new Curve[2] { crv1, curve2 }, out Curve mergedCrv2, 0.01))
		{
			return false;
		}
		crv0 = mergedCrv;
		crv1 = mergedCrv2;
		return true;
	}

	private static Point3d cornerOffset(double offset, Point3d p0, Point3d p1, Point3d p2)
	{
		Vector3d vector3d = p1 - p0;
		Vector3d vector3d2 = p2 - p1;
		vector3d.Unitize();
		vector3d2.Unitize();
		double num = AngleBetweenVectors(vector3d, vector3d2);
		Vector3d vector3d3 = vector3d2 - vector3d;
		vector3d3.Unitize();
		vector3d3 = vector3d3 * offset / Math.Cos(Math.PI - num / 2.0);
		return p1 + vector3d3;
	}

	private static Curve[] GemOffsetCurvesBaguetteTapered(CSGemData gem, double distance, double cornerBlend = 0.0)
	{
		Transform transform = CSBlockUtil.CSUtilMath.Orient3Point(gem.Center, gem.Up, gem.North, Point3d.Origin, Vector3d.ZAxis, Vector3d.YAxis);
		transform.TryGetInverse(out var inverseTransform);
		gem.ApplyTransform(transform);
		NurbsCurve nurbsCurve = gem.GetNurbsCurve();
		double num = gem.GemYSize / 2.0;
		Point3d location = nurbsCurve.Points[1].Location;
		Point3d location2 = nurbsCurve.Points[2].Location;
		Point3d location3 = nurbsCurve.Points[3].Location;
		Point3d location4 = nurbsCurve.Points[4].Location;
		gem.ApplyTransform(inverseTransform);
		Point3d point3d = new Point3d(0.0, num + distance, 0.0);
		Point3d point3d2 = cornerOffset(distance, location4, location, location2);
		Point3d point3d3 = cornerOffset(distance, location, location2, location3);
		Point3d point3d4 = cornerOffset(distance, location2, location3, location4);
		Point3d point3d5 = cornerOffset(distance, location3, location4, location);
		if (point3d2.X < 0.1)
		{
			point3d2.X = 0.1;
		}
		if (point3d3.X < 0.1)
		{
			point3d3.X = 0.1;
		}
		if (point3d4.X > -0.1)
		{
			point3d4.X = -0.1;
		}
		if (point3d5.X > -0.1)
		{
			point3d5.X = -0.1;
		}
		Curve curve = new LineCurve(point3d, point3d2);
		Curve curve2 = new LineCurve(point3d2, point3d3);
		Curve curve3 = new LineCurve(point3d3, point3d4);
		Curve curve4 = new LineCurve(point3d4, point3d5);
		Curve curve5 = new LineCurve(point3d5, point3d);
		if (cornerBlend > 0.0)
		{
			TrimCurve(curve, cornerBlend, CurveEnd.End, out var crvTrimmed);
			TrimCurve(curve2, cornerBlend, CurveEnd.Both, out var crvTrimmed2);
			TrimCurve(curve3, cornerBlend, CurveEnd.Both, out var crvTrimmed3);
			TrimCurve(curve4, cornerBlend, CurveEnd.Both, out var crvTrimmed4);
			TrimCurve(curve5, cornerBlend, CurveEnd.Start, out var crvTrimmed5);
			BlendCorner(ref crvTrimmed, ref crvTrimmed2);
			BlendCorner(ref crvTrimmed2, ref crvTrimmed3);
			BlendCorner(ref crvTrimmed3, ref crvTrimmed4);
			BlendCorner(ref crvTrimmed4, ref crvTrimmed5);
			curve = crvTrimmed;
			curve2 = crvTrimmed2;
			curve3 = crvTrimmed3;
			curve4 = crvTrimmed4;
			curve5 = crvTrimmed5;
		}
		return new Curve[5] { curve, curve2, curve3, curve4, curve5 };
	}

	private static Curve[] GemOffsetCurvesCalf(CSGemData gem, double distance, double cornerBlend = 0.0)
	{
		double num = gem.GemXSize / 2.0;
		double num2 = gem.GemYSize / 2.0;
		Point3d point3d = new Point3d(num * 0.8, num2 * 0.8, 0.0);
		Point3d point3d2 = new Point3d(num, num2 * 0.2, 0.0);
		Point3d point3d3 = new Point3d(num * 0.3, (0.0 - num2) * 1.2, 0.0);
		Vector3d vector3d = new Vector3d(point3d.Y - point3d2.Y, 0.0 - (point3d.X - point3d2.X), 0.0);
		vector3d.Unitize();
		Vector3d vector3d2 = new Vector3d(point3d3.Y - point3d2.Y, 0.0 - (point3d3.X - point3d2.X), 0.0);
		vector3d2.Unitize();
		Vector3d vector3d3 = new Vector3d(0.0, 1.0, 0.0);
		vector3d3 += vector3d;
		vector3d3.Unitize();
		double num3 = distance / vector3d3.Y;
		vector3d3 *= num3;
		Point3d point = new Point3d(point3d + vector3d3);
		Vector3d vector3d4 = new Vector3d(0.0, 1.0, 0.0);
		vector3d4 += vector3d2;
		vector3d4.Unitize();
		num3 = distance / vector3d4.Y;
		vector3d4 *= num3;
		Point3d point2 = new Point3d(point3d3 - vector3d4);
		vector3d.Rotate(Math.PI / 2.0, Vector3d.ZAxis);
		vector3d2.Rotate(Math.PI / 2.0, Vector3d.ZAxis);
		CSBlockUtil.CSUtilMath.LineInt(out var outpoint, point, vector3d, point2, vector3d2);
		Point3d point3d4 = new Point3d(point.X, point.Y, 0.0);
		Point3d point3d5 = new Point3d(outpoint.X, outpoint.Y, 0.0);
		Point3d point3d6 = new Point3d(point2.X, point2.Y, 0.0);
		Point3d point3d7 = new Point3d(0.0 - point2.X, point2.Y, 0.0);
		Point3d point3d8 = new Point3d(0.0 - outpoint.X, outpoint.Y, 0.0);
		Point3d point3d9 = new Point3d(0.0 - point.X, point.Y, 0.0);
		Point3d point3d10 = new Point3d(0.0, point.Y, 0.0);
		if (point3d6.X < point3d7.X)
		{
			Point3d point3d11 = point3d6;
			point3d6 = point3d7;
			point3d7 = point3d11;
		}
		Curve curve = new LineCurve(point3d10, point3d4);
		Curve curve2 = new LineCurve(point3d4, point3d5);
		Curve curve3 = new LineCurve(point3d5, point3d6);
		Curve curve4 = new LineCurve(point3d6, point3d7);
		Curve curve5 = new LineCurve(point3d7, point3d8);
		Curve curve6 = new LineCurve(point3d8, point3d9);
		Curve curve7 = new LineCurve(point3d9, point3d10);
		if (cornerBlend > 0.0)
		{
			TrimCurve(curve, cornerBlend, CurveEnd.End, out var crvTrimmed);
			TrimCurve(curve2, cornerBlend, CurveEnd.Both, out var crvTrimmed2);
			TrimCurve(curve3, cornerBlend, CurveEnd.Both, out var crvTrimmed3);
			TrimCurve(curve4, cornerBlend, CurveEnd.Both, out var crvTrimmed4);
			TrimCurve(curve5, cornerBlend, CurveEnd.Both, out var crvTrimmed5);
			TrimCurve(curve6, cornerBlend, CurveEnd.Both, out var crvTrimmed6);
			TrimCurve(curve7, cornerBlend, CurveEnd.Start, out var crvTrimmed7);
			BlendCorner(ref crvTrimmed, ref crvTrimmed2);
			BlendCorner(ref crvTrimmed2, ref crvTrimmed3);
			BlendCorner(ref crvTrimmed3, ref crvTrimmed4);
			BlendCorner(ref crvTrimmed4, ref crvTrimmed5);
			BlendCorner(ref crvTrimmed5, ref crvTrimmed6);
			BlendCorner(ref crvTrimmed6, ref crvTrimmed7);
			curve = crvTrimmed;
			curve2 = crvTrimmed2;
			curve3 = crvTrimmed3;
			curve4 = crvTrimmed4;
			curve5 = crvTrimmed5;
			curve6 = crvTrimmed6;
			curve7 = crvTrimmed7;
		}
		return new Curve[7] { curve, curve2, curve3, curve4, curve5, curve6, curve7 };
	}

	private static Curve[] GemOffsetCurvesCushion(CSGemData iGem, double iDistance)
	{
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
		Point3d point3d = new Point3d(0.0, 5.0 * num3, 0.0);
		Point3d point3d2 = new Point3d(-2.18319 * num2, 4.54508 * num3, 0.0);
		Point3d point3d3 = new Point3d(2.18319 * num2, 4.54508 * num3, 0.0);
		Arc arc = new Arc(point3d2, point3d, point3d3);
		Vector3d vector3d = new Vector3d(point3d2 - arc.Center);
		vector3d.Unitize();
		Vector3d vector3d2 = new Vector3d(point3d - arc.Center);
		vector3d2.Unitize();
		Vector3d vector3d3 = new Vector3d(point3d3 - arc.Center);
		vector3d3.Unitize();
		Arc arc2 = new Arc(point3d2 + vector3d * num, point3d + vector3d2 * num, point3d3 + vector3d3 * num);
		Point3d point3d4 = new Point3d(4.0 * num2, 0.0, 0.0);
		Point3d point3d5 = new Point3d(3.63684 * num2, 2.72661 * num3, 0.0);
		Point3d point3d6 = new Point3d(3.63684 * num2, -2.72661 * num3, 0.0);
		Arc arc3 = new Arc(point3d5, point3d4, point3d6);
		Vector3d vector3d4 = new Vector3d(point3d5 - arc3.Center);
		vector3d4.Unitize();
		Vector3d vector3d5 = new Vector3d(point3d4 - arc3.Center);
		vector3d5.Unitize();
		Vector3d vector3d6 = new Vector3d(point3d6 - arc3.Center);
		vector3d6.Unitize();
		Arc arc4 = new Arc(point3d5 + vector3d4 * num, point3d4 + vector3d5 * num, point3d6 + vector3d6 * num);
		if (arc2.EndPoint.X > arc4.StartPoint.X || arc4.StartPoint.Y > arc2.EndPoint.Y)
		{
			CSBlockUtil.CSUtilMath.GetUnscaledXforms(iGem.Xform, out var _, out var xDown);
			NurbsCurve nurbsCurve = iGem.GetNurbsCurve();
			nurbsCurve.Transform(xDown);
			return new Curve[1] { CSUtilGeometry.OriginOffsetXY(nurbsCurve, iDistance) };
		}
		Transform xform = Transform.Scale(Plane.WorldXY, -1.0, -1.0, 0.0);
		Arc arc5 = arc2;
		arc5.Transform(xform);
		Arc arc6 = arc4;
		arc6.Transform(xform);
		NurbsCurve nurbsCurve2 = CSUtilCurves.BlendCurve(arc2.EndPoint, arc4.StartPoint, arc2.TangentAt(arc2.AngleDomain.Max), -arc4.TangentAt(arc4.AngleDomain.Min), 0.9);
		NurbsCurve nurbsCurve3 = CSUtilCurves.BlendCurve(arc4.EndPoint, arc5.StartPoint, arc4.TangentAt(arc4.AngleDomain.Max), -arc5.TangentAt(arc5.AngleDomain.Min), 0.9);
		NurbsCurve nurbsCurve4 = CSUtilCurves.BlendCurve(arc5.EndPoint, arc6.StartPoint, arc5.TangentAt(arc5.AngleDomain.Max), -arc6.TangentAt(arc6.AngleDomain.Min), 0.9);
		NurbsCurve nurbsCurve5 = CSUtilCurves.BlendCurve(arc6.EndPoint, arc2.StartPoint, arc6.TangentAt(arc6.AngleDomain.Max), -arc2.TangentAt(arc2.AngleDomain.Min), 0.9);
		ArcCurve arcCurve = new ArcCurve(arc2);
		ArcCurve arcCurve2 = new ArcCurve(arc2);
		Curve curve = arcCurve.Trim(new Interval(arcCurve.Domain.Mid, arcCurve.Domain.Max));
		Curve curve2 = arcCurve2.Trim(new Interval(arcCurve2.Domain.Min, arcCurve2.Domain.Mid));
		return new Curve[9]
		{
			curve,
			nurbsCurve2,
			new ArcCurve(arc4),
			nurbsCurve3,
			new ArcCurve(arc5),
			nurbsCurve4,
			new ArcCurve(arc6),
			nurbsCurve5,
			curve2
		};
	}

	private static Curve[] GemOffsetCurvesCushionRotated90(CSGemData iGem, double iDistance)
	{
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
		Point3d point3d = new Point3d(0.0, 4.0 * num3, 0.0);
		Point3d point3d2 = new Point3d(-2.72661 * num2, 3.63684 * num3, 0.0);
		Point3d point3d3 = new Point3d(2.72661 * num2, 3.63684 * num3, 0.0);
		Arc arc = new Arc(point3d2, point3d, point3d3);
		Vector3d vector3d = new Vector3d(point3d2 - arc.Center);
		vector3d.Unitize();
		Vector3d vector3d2 = new Vector3d(point3d - arc.Center);
		vector3d2.Unitize();
		Vector3d vector3d3 = new Vector3d(point3d3 - arc.Center);
		vector3d3.Unitize();
		Arc arc2 = new Arc(point3d2 + vector3d * num, point3d + vector3d2 * num, point3d3 + vector3d3 * num);
		Point3d point3d4 = new Point3d(5.0 * num2, 0.0, 0.0);
		Point3d point3d5 = new Point3d(4.54508 * num2, 2.18319 * num3, 0.0);
		Point3d point3d6 = new Point3d(4.54508 * num2, -2.18319 * num3, 0.0);
		Arc arc3 = new Arc(point3d5, point3d4, point3d6);
		Vector3d vector3d4 = new Vector3d(point3d5 - arc3.Center);
		vector3d4.Unitize();
		Vector3d vector3d5 = new Vector3d(point3d4 - arc3.Center);
		vector3d5.Unitize();
		Vector3d vector3d6 = new Vector3d(point3d6 - arc3.Center);
		vector3d6.Unitize();
		Arc arc4 = new Arc(point3d5 + vector3d4 * num, point3d4 + vector3d5 * num, point3d6 + vector3d6 * num);
		if (arc2.EndPoint.X > arc4.StartPoint.X || arc4.StartPoint.Y > arc2.EndPoint.Y)
		{
			CSBlockUtil.CSUtilMath.GetUnscaledXforms(iGem.Xform, out var _, out var xDown);
			NurbsCurve nurbsCurve = iGem.GetNurbsCurve();
			nurbsCurve.Transform(xDown);
			return new Curve[1] { CSUtilGeometry.OriginOffsetXY(nurbsCurve, iDistance) };
		}
		Transform xform = Transform.Scale(Plane.WorldXY, -1.0, -1.0, 0.0);
		Arc arc5 = arc2;
		arc5.Transform(xform);
		Arc arc6 = arc4;
		arc6.Transform(xform);
		NurbsCurve nurbsCurve2 = CSUtilCurves.BlendCurve(arc2.EndPoint, arc4.StartPoint, arc2.TangentAt(arc2.AngleDomain.Max), -arc4.TangentAt(arc4.AngleDomain.Min), 0.9);
		NurbsCurve nurbsCurve3 = CSUtilCurves.BlendCurve(arc4.EndPoint, arc5.StartPoint, arc4.TangentAt(arc4.AngleDomain.Max), -arc5.TangentAt(arc5.AngleDomain.Min), 0.9);
		NurbsCurve nurbsCurve4 = CSUtilCurves.BlendCurve(arc5.EndPoint, arc6.StartPoint, arc5.TangentAt(arc5.AngleDomain.Max), -arc6.TangentAt(arc6.AngleDomain.Min), 0.9);
		NurbsCurve nurbsCurve5 = CSUtilCurves.BlendCurve(arc6.EndPoint, arc2.StartPoint, arc6.TangentAt(arc6.AngleDomain.Max), -arc2.TangentAt(arc2.AngleDomain.Min), 0.9);
		ArcCurve arcCurve = new ArcCurve(arc2);
		ArcCurve arcCurve2 = new ArcCurve(arc2);
		Curve curve = arcCurve.Trim(new Interval(arcCurve.Domain.Mid, arcCurve.Domain.Max));
		Curve curve2 = arcCurve2.Trim(new Interval(arcCurve2.Domain.Min, arcCurve2.Domain.Mid));
		return new Curve[9]
		{
			curve,
			nurbsCurve2,
			new ArcCurve(arc4),
			nurbsCurve3,
			new ArcCurve(arc5),
			nurbsCurve4,
			new ArcCurve(arc6),
			nurbsCurve5,
			curve2
		};
	}

	private static Curve[] GemOffsetCurvesCushionSquare(CSGemData gem, double distance, double cornerBlend = 0.0)
	{
		double num = gem.GemXSize / 2.0;
		double num2 = gem.GemYSize / 2.0;
		double num3 = num / 5.0;
		double num4 = num2 / 5.0;
		Point3d pointOnInterior = new Point3d(5.0 * num3, 0.0, 0.0);
		Point3d startPoint = new Point3d(3.94733 * num3, 3.94733 * num4, 0.0);
		Point3d endPoint = new Point3d(3.94733 * num3, -3.94733 * num4, 0.0);
		Arc arc = new Arc(startPoint, pointOnInterior, endPoint);
		Point3d point3d = new Point3d(arc.Center);
		double radius = arc.Radius;
		pointOnInterior = new Point3d(0.0, 5.0 * num4, 0.0);
		startPoint = new Point3d(-3.94733 * num3, 3.94733 * num4, 0.0);
		endPoint = new Point3d(3.94733 * num3, 3.94733 * num4, 0.0);
		Arc arc2 = new Arc(startPoint, pointOnInterior, endPoint);
		Point3d point3d2 = new Point3d(arc2.Center);
		double radius2 = arc2.Radius;
		double num5 = radius + distance;
		double a = radius2 + distance;
		double length = (point3d - point3d2).Length;
		double angleRadians = AngleA(a, num5, length);
		Vector3d vector3d = point3d2 - point3d;
		vector3d.Unitize();
		vector3d.Rotate(angleRadians, new Vector3d(Vector3d.ZAxis));
		vector3d *= num5;
		Point3d point3d3 = point3d + vector3d;
		startPoint = new Point3d(0.0 - point3d3.X, point3d3.Y, 0.0);
		pointOnInterior = new Point3d(0.0, num2 + distance, 0.0);
		endPoint = point3d3;
		Arc arc3 = new Arc(startPoint, pointOnInterior, endPoint);
		arc3.Trim(new Interval(arc3.AngleDomain.Mid, arc3.AngleDomain.Max));
		Arc arc4 = new Arc(startPoint, pointOnInterior, endPoint);
		arc4.Trim(new Interval(arc4.AngleDomain.Min, arc4.AngleDomain.Mid));
		startPoint = point3d3;
		pointOnInterior = new Point3d(num + distance, 0.0, 0.0);
		endPoint = new Point3d(point3d3.X, 0.0 - point3d3.Y, 0.0);
		Arc arc5 = new Arc(startPoint, pointOnInterior, endPoint);
		startPoint = new Point3d(point3d3.X, 0.0 - point3d3.Y, 0.0);
		pointOnInterior = new Point3d(0.0, 0.0 - num2 - distance, 0.0);
		endPoint = new Point3d(0.0 - point3d3.X, 0.0 - point3d3.Y, 0.0);
		Arc arc6 = new Arc(startPoint, pointOnInterior, endPoint);
		startPoint = new Point3d(0.0 - point3d3.X, 0.0 - point3d3.Y, 0.0);
		pointOnInterior = new Point3d(0.0 - num - distance, 0.0, 0.0);
		endPoint = new Point3d(0.0 - point3d3.X, point3d3.Y, 0.0);
		Arc arc7 = new Arc(startPoint, pointOnInterior, endPoint);
		Curve curve = new ArcCurve(arc3);
		Curve curve2 = new ArcCurve(arc5);
		Curve curve3 = new ArcCurve(arc6);
		Curve curve4 = new ArcCurve(arc7);
		Curve curve5 = new ArcCurve(arc4);
		if (cornerBlend > 0.0)
		{
			TrimCurve(curve, cornerBlend, CurveEnd.End, out var crvTrimmed);
			TrimCurve(curve2, cornerBlend, CurveEnd.Both, out var crvTrimmed2);
			TrimCurve(curve3, cornerBlend, CurveEnd.Both, out var crvTrimmed3);
			TrimCurve(curve4, cornerBlend, CurveEnd.Both, out var crvTrimmed4);
			TrimCurve(curve5, cornerBlend, CurveEnd.Start, out var crvTrimmed5);
			BlendCorner(ref crvTrimmed, ref crvTrimmed2);
			BlendCorner(ref crvTrimmed2, ref crvTrimmed3);
			BlendCorner(ref crvTrimmed3, ref crvTrimmed4);
			BlendCorner(ref crvTrimmed4, ref crvTrimmed5);
			curve = crvTrimmed;
			curve2 = crvTrimmed2;
			curve3 = crvTrimmed3;
			curve4 = crvTrimmed4;
			curve5 = crvTrimmed5;
		}
		return new Curve[5] { curve, curve2, curve3, curve4, curve5 };
	}

	private static Curve[] GemOffsetCurvesCushionSquareRotated45(CSGemData gem, double distance)
	{
		double num = gem.GemXSize / 2.0;
		double num2 = gem.GemYSize / 2.0;
		double num3 = num / 5.58236;
		double num4 = num2 / 5.58236;
		Point3d point3d = new Point3d(0.0 * num3, 5.58236 * num4, 0.0);
		Point3d point3d2 = new Point3d(1.055 * num3, 5.30386 * num4, 0.0);
		Vector3d vector3d = point3d2 - point3d;
		Vector3d inputVector = new Vector3d(vector3d);
		inputVector.X = 0.0 - inputVector.X;
		double num5 = CSBlockUtil.CSUtilMath.AngleBetweenVectors(vector3d, inputVector);
		double num6 = 1.0 / Math.Sin(num5 / 2.0);
		point3d.Y += num6 * distance;
		point3d2.Y += num6 * distance;
		Point3d point3d3 = new Point3d(5.58236 * num3, 0.0 * num4, 0.0);
		Point3d point3d4 = new Point3d(5.30386 * num3, 1.055 * num4, 0.0);
		Vector3d vector3d2 = point3d4 - point3d3;
		Vector3d inputVector2 = new Vector3d(vector3d2);
		inputVector2.Y = 0.0 - inputVector2.Y;
		double num7 = CSBlockUtil.CSUtilMath.AngleBetweenVectors(vector3d2, inputVector2);
		double num8 = 1.0 / Math.Sin(num7 / 2.0);
		point3d3.X += num8 * distance;
		point3d4.X += num8 * distance;
		vector3d = point3d2 - point3d;
		vector3d2 = point3d4 - point3d3;
		vector3d.Unitize();
		vector3d2.Unitize();
		NurbsCurve nurbsCurve = CSUtilCurves.BlendCurve(point3d, point3d3, vector3d, vector3d2, 0.85);
		point3d.Y = 0.0 - point3d.Y;
		vector3d.Y = 0.0 - vector3d.Y;
		vector3d2.Y = 0.0 - vector3d2.Y;
		NurbsCurve nurbsCurve2 = CSUtilCurves.BlendCurve(point3d3, point3d, vector3d2, vector3d, 0.85);
		point3d3.X = 0.0 - point3d3.X;
		vector3d.X = 0.0 - vector3d.X;
		vector3d2.X = 0.0 - vector3d2.X;
		NurbsCurve nurbsCurve3 = CSUtilCurves.BlendCurve(point3d, point3d3, vector3d, vector3d2, 0.85);
		point3d.Y = 0.0 - point3d.Y;
		vector3d.Y = 0.0 - vector3d.Y;
		vector3d2.Y = 0.0 - vector3d2.Y;
		NurbsCurve nurbsCurve4 = CSUtilCurves.BlendCurve(point3d3, point3d, vector3d2, vector3d, 0.85);
		return new Curve[4] { nurbsCurve, nurbsCurve2, nurbsCurve3, nurbsCurve4 };
	}

	private static Curve[] GemOffsetCurvesEmerald(CSGemData gem, double distance, double cornerPercentX, double cornerPercentY, double cornerBlend = 0.0)
	{
		double num = gem.GemXSize / 2.0;
		double num2 = gem.GemYSize / 2.0;
		Point3d point3d = new Point3d(num * cornerPercentX, num2, 0.0);
		Point3d point3d2 = new Point3d(num, num2 * cornerPercentY, 0.0);
		Vector3d vector3d = new Vector3d(point3d.Y - point3d2.Y, 0.0 - (point3d.X - point3d2.X), 0.0);
		vector3d.Unitize();
		Vector3d vector3d2 = new Vector3d(1.0, 0.0, 0.0);
		vector3d2 += vector3d;
		vector3d2.Unitize();
		double num3 = distance / vector3d2.X;
		vector3d2 *= num3;
		Point3d point3d3 = new Point3d(point3d2 + vector3d2);
		Vector3d vector3d3 = new Vector3d(0.0, 1.0, 0.0);
		vector3d3 += vector3d;
		vector3d3.Unitize();
		double num4 = distance / vector3d3.Y;
		vector3d3 *= num4;
		Point3d point3d4 = new Point3d(point3d + vector3d3);
		if (point3d4.X + 0.02 > point3d3.X)
		{
			Point3d point3d5 = new Point3d(point3d3.X, point3d4.Y, 0.0);
			point3d3 = new Point3d(point3d5.X, point3d5.Y - 0.02, 0.0);
			point3d4 = new Point3d(point3d5.X - 0.02, point3d5.Y, 0.0);
		}
		if (point3d4.X - 0.04 <= 0.0)
		{
			Point3d point3d6 = new Point3d(0.04, point3d4.Y, 0.0);
			point3d3 = new Point3d(point3d6.X, point3d6.Y - 0.02, 0.0);
			point3d4 = new Point3d(point3d6.X - 0.02, point3d6.Y, 0.0);
		}
		if (point3d4.Y - 0.04 <= 0.0)
		{
			Point3d point3d7 = new Point3d(point3d3.X, 0.04, 0.0);
			point3d3 = new Point3d(point3d7.X, point3d7.Y - 0.02, 0.0);
			point3d4 = new Point3d(point3d7.X - 0.02, point3d7.Y, 0.0);
		}
		Point3d point3d8 = new Point3d(0.0, point3d4.Y, 0.0);
		Point3d point3d9 = new Point3d(point3d4.X, point3d4.Y, 0.0);
		Point3d point3d10 = new Point3d(point3d3.X, point3d3.Y, 0.0);
		Point3d point3d11 = new Point3d(point3d3.X, 0.0 - point3d3.Y, 0.0);
		Point3d point3d12 = new Point3d(point3d4.X, 0.0 - point3d4.Y, 0.0);
		Point3d point3d13 = new Point3d(0.0 - point3d4.X, 0.0 - point3d4.Y, 0.0);
		Point3d point3d14 = new Point3d(0.0 - point3d3.X, 0.0 - point3d3.Y, 0.0);
		Point3d point3d15 = new Point3d(0.0 - point3d3.X, point3d3.Y, 0.0);
		Point3d point3d16 = new Point3d(0.0 - point3d4.X, point3d4.Y, 0.0);
		Curve curve = new LineCurve(point3d8, point3d9);
		Curve curve2 = new LineCurve(point3d9, point3d10);
		Curve curve3 = new LineCurve(point3d10, point3d11);
		Curve curve4 = new LineCurve(point3d11, point3d12);
		Curve curve5 = new LineCurve(point3d12, point3d13);
		Curve curve6 = new LineCurve(point3d13, point3d14);
		Curve curve7 = new LineCurve(point3d14, point3d15);
		Curve curve8 = new LineCurve(point3d15, point3d16);
		Curve curve9 = new LineCurve(point3d16, point3d8);
		if (cornerBlend > 0.0)
		{
			if (cornerBlend < 0.11)
			{
				cornerBlend = 0.11;
			}
			TrimCurve(curve, cornerBlend, CurveEnd.End, out var crvTrimmed);
			TrimCurve(curve2, cornerBlend, CurveEnd.Both, out var crvTrimmed2);
			TrimCurve(curve3, cornerBlend, CurveEnd.Both, out var crvTrimmed3);
			TrimCurve(curve4, cornerBlend, CurveEnd.Both, out var crvTrimmed4);
			TrimCurve(curve5, cornerBlend, CurveEnd.Both, out var crvTrimmed5);
			TrimCurve(curve6, cornerBlend, CurveEnd.Both, out var crvTrimmed6);
			TrimCurve(curve7, cornerBlend, CurveEnd.Both, out var crvTrimmed7);
			TrimCurve(curve8, cornerBlend, CurveEnd.Both, out var crvTrimmed8);
			TrimCurve(curve9, cornerBlend, CurveEnd.Start, out var crvTrimmed9);
			BlendCorner(ref crvTrimmed, ref crvTrimmed2);
			BlendCorner(ref crvTrimmed2, ref crvTrimmed3);
			BlendCorner(ref crvTrimmed3, ref crvTrimmed4);
			BlendCorner(ref crvTrimmed4, ref crvTrimmed5);
			BlendCorner(ref crvTrimmed5, ref crvTrimmed6);
			BlendCorner(ref crvTrimmed6, ref crvTrimmed7);
			BlendCorner(ref crvTrimmed7, ref crvTrimmed8);
			BlendCorner(ref crvTrimmed8, ref crvTrimmed9);
			curve = crvTrimmed;
			curve2 = crvTrimmed2;
			curve3 = crvTrimmed3;
			curve4 = crvTrimmed4;
			curve5 = crvTrimmed5;
			curve6 = crvTrimmed6;
			curve7 = crvTrimmed7;
			curve8 = crvTrimmed8;
			curve9 = crvTrimmed9;
		}
		return new Curve[9] { curve, curve2, curve3, curve4, curve5, curve6, curve7, curve8, curve9 };
	}

	private static Curve[] GemOffsetCurvesHalfMoon(CSGemData gem, double distance, double cornerBlend = 0.0)
	{
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
		Transform transform = Transform.Rotation(CSBlockUtil.CSUtilMath.ToRadians(90.0), Vector3d.ZAxis, Point3d.Origin);
		Transform transform2 = Transform.Scale(Plane.WorldXY, -1.0, 1.0, 1.0);
		Transform transform3 = Transform.Scale(Plane.WorldXY, gemXSize, num, 0.0);
		Transform xform = Transform.Identity;
		if (cSGemData != null)
		{
			xform = Transform.Translation(cSGemData.XMinPoint.X - distance, 0.0, 0.0);
		}
		NurbsCurve nurbsCurve = new Circle(Point3d.Origin, 1.0).ToNurbsCurve();
		nurbsCurve.Transform(transform3 * transform2 * transform);
		nurbsCurve.Transform(xform);
		Curve curve = nurbsCurve.Trim(new Interval(0.0, nurbsCurve.Domain.Mid));
		Curve curve2 = curve.DuplicateCurve();
		curve2 = curve2.Trim(new Interval(0.0, curve.Domain.Mid));
		Curve curve3 = curve.DuplicateCurve();
		curve3 = curve3.Trim(new Interval(curve.Domain.Mid, curve.Domain.Max));
		Point3d point3d = curve.PointAtEnd + (curve.PointAtStart - curve.PointAtEnd) / 2.0;
		Curve curve4 = new LineCurve(point3d, curve.PointAtStart);
		Curve curve5 = new LineCurve(curve.PointAtEnd, point3d);
		if (cornerBlend > 0.0)
		{
			TrimCurve(curve4, cornerBlend, CurveEnd.End, out var crvTrimmed);
			TrimCurve(curve2, cornerBlend, CurveEnd.Start, out var crvTrimmed2);
			TrimCurve(curve3, cornerBlend, CurveEnd.End, out var crvTrimmed3);
			TrimCurve(curve5, cornerBlend, CurveEnd.Start, out var crvTrimmed4);
			BlendCorner(ref crvTrimmed, ref crvTrimmed2);
			BlendCorner(ref crvTrimmed3, ref crvTrimmed4);
			curve4 = crvTrimmed;
			curve2 = crvTrimmed2;
			curve3 = crvTrimmed3;
			curve5 = crvTrimmed4;
		}
		return new Curve[4] { curve4, curve2, curve3, curve5 };
	}

	private static Curve[] GemOffsetCurvesHeart(CSGemData gem, double distance, double cornerBlend = 0.0)
	{
		double num = gem.GemXSize / 2.0;
		double num2 = gem.GemYSize / 2.0;
		Transform transform = Transform.Scale(Plane.WorldXY, num / 5.0, num2 / 5.0, 1.0);
		double num3 = 5.0 / num;
		double num4 = 5.0 / num2;
		double num5 = distance * num3;
		double num6 = distance * num4;
		NurbsCurve nurbsCurve = new NurbsCurve(3, rational: false, 4, 7);
		nurbsCurve.Points[0] = new ControlPoint(5.0066166789220405, 0.0, 0.0, 1.0);
		nurbsCurve.Points[1] = new ControlPoint(5.006616678922161, -0.6563961048979948, 0.0, 1.0);
		nurbsCurve.Points[2] = new ControlPoint(4.649296555283862, -2.0660402223759666, 0.0, 1.0);
		nurbsCurve.Points[3] = new ControlPoint(3.3947011721913873, -3.8722073371279846, 0.0, 1.0);
		nurbsCurve.Points[4] = new ControlPoint(1.870281543227221, -5.609876323245201, 0.0, 1.0);
		nurbsCurve.Points[5] = new ControlPoint(0.6112959541798841, -6.50455766084459, 0.0, 1.0);
		nurbsCurve.Points[6] = new ControlPoint(0.0, -6.966404742505439, 0.0, 1.0);
		nurbsCurve.Knots[0] = 0.0;
		nurbsCurve.Knots[1] = 0.0;
		nurbsCurve.Knots[2] = 0.0;
		nurbsCurve.Knots[3] = 0.22119745803368646;
		nurbsCurve.Knots[4] = 0.480798305355791;
		nurbsCurve.Knots[5] = 0.7403991526778955;
		nurbsCurve.Knots[6] = 1.0;
		nurbsCurve.Knots[7] = 1.0;
		nurbsCurve.Knots[8] = 1.0;
		double num7 = 12.9866 + num6;
		double num8 = Math.Sqrt(num7 * num7 - 57.072887715600004) - 3.59672;
		double num9 = 5.0 + num5;
		Transform transform2 = Transform.Translation(0.0, 1.4744, 0.0);
		Transform transform3 = Transform.Scale(Plane.WorldXY, num9 / 5.0, num8 / 6.9664, 1.0);
		nurbsCurve.Transform(transform * transform2 * transform3);
		NurbsCurve nurbsCurve2 = new NurbsCurve(3, rational: false, 4, 6);
		nurbsCurve2.Points[0] = new ControlPoint(0.0, 3.0365333791772997, 0.0, 1.0);
		nurbsCurve2.Points[1] = new ControlPoint(0.6449175426389968, 3.0365333791772997, 0.0, 1.0);
		nurbsCurve2.Points[2] = new ControlPoint(2.144578282320578, 2.6159447841997165, 0.0, 1.0);
		nurbsCurve2.Points[3] = new ControlPoint(2.796856932501917, 1.0001195190754388, 0.0, 1.0);
		nurbsCurve2.Points[4] = new ControlPoint(2.835307823555864, 0.11396078132127507, 0.0, 1.0);
		nurbsCurve2.Points[5] = new ControlPoint(2.8353078235558855, 0.0, 0.0, 1.0);
		nurbsCurve2.Knots[0] = 0.0;
		nurbsCurve2.Knots[1] = 0.0;
		nurbsCurve2.Knots[2] = 0.0;
		nurbsCurve2.Knots[3] = 0.45097144418550456;
		nurbsCurve2.Knots[4] = 0.9292474576898094;
		nurbsCurve2.Knots[5] = 1.0;
		nurbsCurve2.Knots[6] = 1.0;
		nurbsCurve2.Knots[7] = 1.0;
		num8 = 3.0365333791772997 + num6;
		num9 = 2.8353078235558855 + num5;
		num9 = Math.Round(num9, 2, MidpointRounding.AwayFromZero);
		if (num9 < 0.04)
		{
			num5 = -2.7953078235558855;
			distance = num5 / num3;
			return GemOffsetCurvesHeart(gem, distance, cornerBlend);
		}
		transform2 = Transform.Translation(2.17131, 1.4744, 0.0);
		transform3 = Transform.Scale(Plane.WorldXY, num9 / 2.8353078235558855, num8 / 3.0365333791772997, 1.0);
		nurbsCurve2.Transform(transform * transform2 * transform3);
		nurbsCurve2.PointAtEnd.DistanceTo(nurbsCurve.PointAtStart);
		_ = 0.01;
		NurbsCurve nurbsCurve3 = new NurbsCurve(3, rational: false, 4, 4);
		nurbsCurve3.Points[0] = new ControlPoint(0.0, 1.5786909734483108, 0.0, 1.0);
		nurbsCurve3.Points[1] = new ControlPoint(0.3572666485958004, 2.6632323152819315, 0.0, 1.0);
		nurbsCurve3.Points[2] = new ControlPoint(1.2783305169636177, 3.0286525140927982, 0.0, 1.0);
		nurbsCurve3.Points[3] = new ControlPoint(2.171308855366155, 3.0365333791772997, 0.0, 1.0);
		nurbsCurve3.Knots[0] = 0.0;
		nurbsCurve3.Knots[1] = 0.0;
		nurbsCurve3.Knots[2] = 0.0;
		nurbsCurve3.Knots[3] = 1.0;
		nurbsCurve3.Knots[4] = 1.0;
		nurbsCurve3.Knots[5] = 1.0;
		double num10 = num6;
		if (num10 < 0.0)
		{
			num10 = 0.0;
		}
		double yScaleFactor = 1.0 / (num10 + 1.0);
		Transform transform4 = Transform.Translation(0.0, -3.0365333791772997, 0.0);
		transform3 = Transform.Scale(Plane.WorldXY, 1.0, yScaleFactor, 1.0);
		transform2 = Transform.Translation(0.0, 4.5109333791773 + num6, 0.0);
		nurbsCurve3.Transform(transform * transform2 * transform3 * transform4);
		RhUtilHelper.RhinoMakeCurveEndsMeet(nurbsCurve2, doAdjustCurveA: false, nurbsCurve, doAdjustCurveB: true);
		Transform xform = Transform.Scale(Plane.WorldXY, -1.0, 1.0, 1.0);
		NurbsCurve nurbsCurve4 = nurbsCurve.ToNurbsCurve();
		nurbsCurve4.Transform(xform);
		nurbsCurve4.Reverse();
		NurbsCurve nurbsCurve5 = nurbsCurve2.ToNurbsCurve();
		nurbsCurve5.Transform(xform);
		nurbsCurve5.Reverse();
		NurbsCurve nurbsCurve6 = nurbsCurve3.ToNurbsCurve();
		nurbsCurve6.Transform(xform);
		nurbsCurve6.Reverse();
		if (cornerBlend > 0.0)
		{
			TrimCurve(nurbsCurve3, cornerBlend, CurveEnd.Start, out var crvTrimmed);
			TrimCurve(nurbsCurve, cornerBlend, CurveEnd.End, out var crvTrimmed2);
			TrimCurve(nurbsCurve4, cornerBlend, CurveEnd.Start, out var crvTrimmed3);
			TrimCurve(nurbsCurve6, cornerBlend, CurveEnd.End, out var crvTrimmed4);
			BlendCorner(ref crvTrimmed2, ref crvTrimmed3);
			BlendCorner(ref crvTrimmed4, ref crvTrimmed);
			nurbsCurve3 = crvTrimmed.ToNurbsCurve();
			nurbsCurve = crvTrimmed2.ToNurbsCurve();
			nurbsCurve4 = crvTrimmed3.ToNurbsCurve();
			nurbsCurve6 = crvTrimmed4.ToNurbsCurve();
		}
		return new Curve[6] { nurbsCurve3, nurbsCurve2, nurbsCurve, nurbsCurve4, nurbsCurve5, nurbsCurve6 };
	}

	private static Curve[] GemOffsetCurvesMarquise(CSGemData gem, double distance, double cornerBlend = 0.0)
	{
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
		Point3d startPoint = new Point3d(0.0, num4, 0.0);
		Point3d pointOnInterior = new Point3d(2.5 + num2, 0.0, 0.0);
		Point3d endPoint = new Point3d(0.0, 0.0 - num4, 0.0);
		Arc arc = new Arc(startPoint, pointOnInterior, endPoint);
		Transform xform = Transform.Scale(Plane.WorldXY, gemXSize / 5.0, gemYSize / 10.0, 1.0);
		NurbsCurve nurbsCurve = arc.ToNurbsCurve();
		nurbsCurve.Transform(xform);
		Transform xform2 = Transform.Scale(Plane.WorldXY, -1.0, -1.0, 1.0);
		NurbsCurve nurbsCurve2 = nurbsCurve.ToNurbsCurve();
		nurbsCurve2.Transform(xform2);
		if (cornerBlend > 0.0)
		{
			TrimCurve(nurbsCurve, cornerBlend, CurveEnd.Both, out var crvTrimmed);
			TrimCurve(nurbsCurve2, cornerBlend, CurveEnd.Both, out var crvTrimmed2);
			BlendCorner(ref crvTrimmed, ref crvTrimmed2);
			BlendCorner(ref crvTrimmed2, ref crvTrimmed);
			nurbsCurve = crvTrimmed.ToNurbsCurve();
			nurbsCurve2 = crvTrimmed2.ToNurbsCurve();
		}
		return new Curve[2] { nurbsCurve, nurbsCurve2 };
	}

	private static Curve[] GemOffsetCurvesMarquiseRotated90(CSGemData gem, double distance)
	{
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
		Point3d startPoint = new Point3d(0.0 - num4, 0.0, 0.0);
		Point3d pointOnInterior = new Point3d(0.0, 2.5 + num2, 0.0);
		Point3d endPoint = new Point3d(num4, 0.0, 0.0);
		Arc arc = new Arc(startPoint, pointOnInterior, endPoint);
		Transform xform = Transform.Scale(Plane.WorldXY, gemXSize / 10.0, gemYSize / 5.0, 1.0);
		NurbsCurve nurbsCurve = arc.ToNurbsCurve();
		nurbsCurve.Transform(xform);
		Curve curve = nurbsCurve.Trim(new Interval(nurbsCurve.Domain.Mid, nurbsCurve.Domain.Max));
		Curve curve2 = nurbsCurve.Trim(new Interval(nurbsCurve.Domain.Min, nurbsCurve.Domain.Mid));
		Transform xform2 = Transform.Mirror(Plane.WorldZX);
		NurbsCurve nurbsCurve2 = curve.ToNurbsCurve();
		nurbsCurve2.Transform(xform2);
		nurbsCurve2.Reverse();
		NurbsCurve nurbsCurve3 = curve2.ToNurbsCurve();
		nurbsCurve3.Transform(xform2);
		nurbsCurve3.Reverse();
		return new Curve[4] { curve, nurbsCurve2, nurbsCurve3, curve2 };
	}

	private static Curve[] GemOffsetCurvesPear(CSGemData gem, double distance, double cornerBlend = 0.0)
	{
		double gemXSize = gem.GemXSize;
		double gemYSize = gem.GemYSize;
		if (distance < (0.0 - gemXSize) / 2.0 + 0.01)
		{
			distance = (0.0 - gemXSize) / 2.0 + 0.01;
		}
		double num = gemXSize / 7.0;
		double num2 = gemYSize / 10.0;
		double xScaleFactor = 3.5 * num + distance;
		double yScaleFactor = 3.81 * num2 + distance;
		Transform transform = Transform.Scale(Plane.WorldXY, xScaleFactor, yScaleFactor, 1.0);
		Transform transform2 = Transform.Translation(0.0, -0.285806 * num2, 0.0);
		NurbsCurve nurbsCurve = new Arc(new Point3d(0.0, 1.0, 0.0), new Point3d(0.707, 0.707, 0.0), new Point3d(1.0, 0.0, 0.0)).ToNurbsCurve();
		nurbsCurve.Transform(transform2 * transform);
		Point3d pointAtEnd = nurbsCurve.PointAtEnd;
		Point3d point3d = new Point3d(0.0 * num, -6.47576 * num2, 0.0);
		Point3d point3d2 = new Point3d(0.173945 * num, -6.35321 * num2, 0.0);
		Point3d point3d3 = new Point3d(-0.173945 * num, -6.35321 * num2, 0.0);
		Vector3d vector3d = new Vector3d(point3d2 - point3d);
		vector3d.Unitize();
		Vector3d inputVector = new Vector3d(point3d3 - point3d);
		inputVector.Unitize();
		double num3 = CSBlockUtil.CSUtilMath.AngleBetweenVectors(vector3d, inputVector);
		double num4 = 1.0 / Math.Sin(num3 / 2.0);
		point3d.Y -= num4 * distance;
		if (distance < 0.0)
		{
			double num5 = distance / (gemXSize / 2.0);
			vector3d.X *= 1.0 - num5 * num5;
			vector3d.Unitize();
		}
		NurbsCurve nurbsCurve2 = CSUtilCurves.BlendCurve(pointAtEnd, point3d, new Vector3d(0.0, -1.0, 0.0), vector3d, 0.8);
		nurbsCurve2 = nurbsCurve2.Rebuild(nurbsCurve2.Points.Count, 3, preserveTangents: true);
		Transform xform = Transform.Mirror(Plane.WorldYZ);
		NurbsCurve nurbsCurve3 = nurbsCurve2.ToNurbsCurve();
		nurbsCurve3.Transform(xform);
		nurbsCurve3.Reverse();
		NurbsCurve nurbsCurve4 = nurbsCurve.ToNurbsCurve();
		nurbsCurve4.Transform(xform);
		nurbsCurve4.Reverse();
		if (gem.VersionMajor > 1)
		{
			nurbsCurve2.Translate(new Vector3d(0.0, 0.08 * gemYSize, 0.0));
			nurbsCurve.Translate(new Vector3d(0.0, 0.08 * gemYSize, 0.0));
			nurbsCurve3.Translate(new Vector3d(0.0, 0.08 * gemYSize, 0.0));
			nurbsCurve4.Translate(new Vector3d(0.0, 0.08 * gemYSize, 0.0));
		}
		if (cornerBlend > 0.0)
		{
			TrimCurve(nurbsCurve2, cornerBlend, CurveEnd.End, out var crvTrimmed);
			TrimCurve(nurbsCurve3, cornerBlend, CurveEnd.Start, out var crvTrimmed2);
			BlendCorner(ref crvTrimmed, ref crvTrimmed2);
			nurbsCurve2 = crvTrimmed.ToNurbsCurve();
			nurbsCurve3 = crvTrimmed2.ToNurbsCurve();
		}
		return new Curve[4] { nurbsCurve, nurbsCurve2, nurbsCurve3, nurbsCurve4 };
	}

	private static Curve[] GemOffsetCurvesRound(CSGemData gem, double distance)
	{
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
		Transform transform = Transform.Rotation(CSBlockUtil.CSUtilMath.ToRadians(90.0), Vector3d.ZAxis, Point3d.Origin);
		Transform transform2 = Transform.Mirror(Plane.WorldYZ);
		Transform transform3 = Transform.Scale(Plane.WorldXY, num, num2, 0.0);
		NurbsCurve nurbsCurve = new Circle(Point3d.Origin, 1.0).ToNurbsCurve();
		nurbsCurve.Transform(transform3 * transform2 * transform);
		return new Curve[1] { nurbsCurve };
	}

	private static Curve[] GemOffsetCurvesSquare(CSGemData gem, double distance, double cornerBlend = 0.0)
	{
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
		Point3d point3d = new Point3d(0.0, num2, 0.0);
		Point3d point3d2 = new Point3d(num, num2, 0.0);
		Point3d point3d3 = new Point3d(num, 0.0 - num2, 0.0);
		Point3d point3d4 = new Point3d(0.0 - num, 0.0 - num2, 0.0);
		Point3d point3d5 = new Point3d(0.0 - num, num2, 0.0);
		Curve curve = new LineCurve(point3d, point3d2);
		Curve curve2 = new LineCurve(point3d2, point3d3);
		Curve curve3 = new LineCurve(point3d3, point3d4);
		Curve curve4 = new LineCurve(point3d4, point3d5);
		Curve curve5 = new LineCurve(point3d5, point3d);
		if (cornerBlend > 0.0)
		{
			TrimCurve(curve, cornerBlend, CurveEnd.End, out var crvTrimmed);
			TrimCurve(curve2, cornerBlend, CurveEnd.Both, out var crvTrimmed2);
			TrimCurve(curve3, cornerBlend, CurveEnd.Both, out var crvTrimmed3);
			TrimCurve(curve4, cornerBlend, CurveEnd.Both, out var crvTrimmed4);
			TrimCurve(curve5, cornerBlend, CurveEnd.Start, out var crvTrimmed5);
			BlendCorner(ref crvTrimmed, ref crvTrimmed2);
			BlendCorner(ref crvTrimmed2, ref crvTrimmed3);
			BlendCorner(ref crvTrimmed3, ref crvTrimmed4);
			BlendCorner(ref crvTrimmed4, ref crvTrimmed5);
			curve = crvTrimmed;
			curve2 = crvTrimmed2;
			curve3 = crvTrimmed3;
			curve4 = crvTrimmed4;
			curve5 = crvTrimmed5;
		}
		return new Curve[5] { curve, curve2, curve3, curve4, curve5 };
	}

	private static Curve[] GemOffsetCurvesSquareRotate45(CSGemData gem, double distance)
	{
		double num = gem.GemXSize / 2.0;
		double num2 = gem.GemYSize / 2.0;
		Vector3d inputVector = new Vector3d(num, num2, 0.0);
		Vector3d inputVector2 = new Vector3d(num, 0.0 - num2, 0.0);
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
		Point3d point3d = new Point3d(0.0, num2, 0.0);
		Point3d point3d2 = new Point3d(num, 0.0, 0.0);
		Point3d point3d3 = new Point3d(0.0, 0.0 - num2, 0.0);
		Point3d point3d4 = new Point3d(0.0 - num, 0.0, 0.0);
		LineCurve lineCurve = new LineCurve(point3d, point3d2);
		LineCurve lineCurve2 = new LineCurve(point3d2, point3d3);
		LineCurve lineCurve3 = new LineCurve(point3d3, point3d4);
		LineCurve lineCurve4 = new LineCurve(point3d4, point3d);
		return new Curve[4] { lineCurve, lineCurve2, lineCurve3, lineCurve4 };
	}

	private static Curve[] GemOffsetCurvesTriangle(CSGemData gem, double distance, double cornerBlend = 0.0)
	{
		double x = gem.GemXSize / 2.0;
		double num = gem.GemYSize / 2.0;
		Point3d point3d = new Point3d(x, num * (2.0 / 3.0), 0.0);
		Point3d point = new Point3d(0.0, (0.0 - num) * 1.3333333333333333, 0.0);
		Vector3d vector3d = new Vector3d(point3d.Y - point.Y, 0.0 - (point3d.X - point.X), 0.0);
		vector3d.Unitize();
		Vector3d vector3d2 = new Vector3d(0.0, 1.0, 0.0);
		vector3d2 += vector3d;
		vector3d2.Unitize();
		double num2 = distance / vector3d2.Y;
		vector3d2 *= num2;
		Point3d point3d2 = new Point3d(point3d + vector3d2);
		Point3d point3d3 = new Point3d(point);
		point3d3.Y += distance / vector3d.Y;
		Point3d point3d4 = new Point3d(0.0, point3d2.Y, 0.0);
		Point3d point3d5 = new Point3d(point3d2.X, point3d2.Y, 0.0);
		Point3d point3d6 = new Point3d(point3d3.X, point3d3.Y, 0.0);
		Point3d point3d7 = new Point3d(0.0 - point3d2.X, point3d2.Y, 0.0);
		Curve curve = new LineCurve(point3d4, point3d5);
		Curve curve2 = new LineCurve(point3d5, point3d6);
		Curve curve3 = new LineCurve(point3d6, point3d7);
		Curve curve4 = new LineCurve(point3d7, point3d4);
		if (cornerBlend > 0.0)
		{
			TrimCurve(curve, cornerBlend, CurveEnd.End, out var crvTrimmed);
			TrimCurve(curve2, cornerBlend, CurveEnd.Both, out var crvTrimmed2);
			TrimCurve(curve3, cornerBlend, CurveEnd.Both, out var crvTrimmed3);
			TrimCurve(curve4, cornerBlend, CurveEnd.Start, out var crvTrimmed4);
			BlendCorner(ref crvTrimmed, ref crvTrimmed2);
			BlendCorner(ref crvTrimmed2, ref crvTrimmed3);
			BlendCorner(ref crvTrimmed3, ref crvTrimmed4);
			curve = crvTrimmed;
			curve2 = crvTrimmed2;
			curve3 = crvTrimmed3;
			curve4 = crvTrimmed4;
		}
		return new Curve[4] { curve, curve2, curve3, curve4 };
	}

	private static Curve[] GemOffsetCurvesTrillionCurved(CSGemData gem, double distance, double cornerBlend = 0.0)
	{
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
		Point3d point = new Point3d(0.0, num2 * 0.7969, 0.0);
		Point3d point3d = new Point3d(num * 0.314, num2 * 0.7887, 0.0);
		Point3d point3d2 = new Point3d(num * 0.6668, num2 * 0.7276, 0.0);
		Point3d point3d3 = new Point3d(num, num2 * 0.5969, 0.0);
		Point3d point3d4 = new Point3d(num * 0.9713, num2 * 0.4106, 0.0);
		Point3d point3d5 = new Point3d(num * 0.9266, num2 * 0.2275, 0.0);
		Point3d point3d6 = new Point3d(num * 0.7308, (0.0 - num2) * 0.2561, 0.0);
		Point3d point3d7 = new Point3d(num * 0.4705, (0.0 - num2) * 0.7083, 0.0);
		Point3d point3d8 = new Point3d(num * 0.3472, (0.0 - num2) * 0.877, 0.0);
		Point3d point3d9 = new Point3d(num * 0.2091, (0.0 - num2) * 1.0337, 0.0);
		Point3d point3d10 = new Point3d(num * 0.1081, (0.0 - num2) * 1.1228, 0.0);
		Point3d point2 = new Point3d(0.0, (0.0 - num2) * 1.2031, 0.0);
		Vector3d vector = new Vector3d(point.Y - point3d.Y, 0.0 - (point.X - point3d.X), 0.0);
		vector.Unitize();
		Vector3d vector3d = new Vector3d(point3d.Y - point3d2.Y, 0.0 - (point3d.X - point3d2.X), 0.0);
		vector3d.Unitize();
		Vector3d vector3d2 = new Vector3d(point3d2.Y - point3d3.Y, 0.0 - (point3d2.X - point3d3.X), 0.0);
		vector3d2.Unitize();
		Vector3d vector3d3 = new Vector3d(point3d3.Y - point3d4.Y, 0.0 - (point3d3.X - point3d4.X), 0.0);
		vector3d3.Unitize();
		Vector3d vector3d4 = new Vector3d(point3d4.Y - point3d5.Y, 0.0 - (point3d4.X - point3d5.X), 0.0);
		vector3d4.Unitize();
		Vector3d vector3d5 = new Vector3d(point3d5.Y - point3d6.Y, 0.0 - (point3d5.X - point3d6.X), 0.0);
		vector3d5.Unitize();
		Vector3d vector3d6 = new Vector3d(point3d6.Y - point3d7.Y, 0.0 - (point3d6.X - point3d7.X), 0.0);
		vector3d6.Unitize();
		Vector3d vector3d7 = new Vector3d(point3d7.Y - point3d8.Y, 0.0 - (point3d7.X - point3d8.X), 0.0);
		vector3d7.Unitize();
		Vector3d vector3d8 = new Vector3d(point3d8.Y - point3d9.Y, 0.0 - (point3d8.X - point3d9.X), 0.0);
		vector3d8.Unitize();
		Vector3d vector3d9 = new Vector3d(point3d9.Y - point3d10.Y, 0.0 - (point3d9.X - point3d10.X), 0.0);
		vector3d9.Unitize();
		Vector3d vector3d10 = new Vector3d(point3d10.Y - point2.Y, 0.0 - (point3d10.X - point2.X), 0.0);
		vector3d10.Unitize();
		Point3d point3 = new Point3d(point);
		Point3d point3d11 = new Point3d(point2);
		point3.Y += distance / vector.Y;
		point3d11.Y += distance / vector3d10.Y;
		Vector3d vector2 = new Vector3d(vector);
		Vector3d vector3d11 = new Vector3d(vector3d);
		Vector3d vector3d12 = new Vector3d(vector3d2);
		Vector3d vector3d13 = new Vector3d(vector3d3);
		Vector3d vector3d14 = new Vector3d(vector3d4);
		Vector3d vector3d15 = new Vector3d(vector3d5);
		Vector3d vector3d16 = new Vector3d(vector3d6);
		Vector3d vector3d17 = new Vector3d(vector3d7);
		Vector3d vector3d18 = new Vector3d(vector3d8);
		Vector3d vector3d19 = new Vector3d(vector3d9);
		Vector3d vector3 = new Vector3d(vector3d10);
		vector2.Rotate(Math.PI / 2.0, Vector3d.ZAxis);
		vector3d11.Rotate(Math.PI / 2.0, Vector3d.ZAxis);
		vector3d12.Rotate(Math.PI / 2.0, Vector3d.ZAxis);
		vector3d13.Rotate(Math.PI / 2.0, Vector3d.ZAxis);
		vector3d14.Rotate(Math.PI / 2.0, Vector3d.ZAxis);
		vector3d15.Rotate(Math.PI / 2.0, Vector3d.ZAxis);
		vector3d16.Rotate(Math.PI / 2.0, Vector3d.ZAxis);
		vector3d17.Rotate(Math.PI / 2.0, Vector3d.ZAxis);
		vector3d18.Rotate(Math.PI / 2.0, Vector3d.ZAxis);
		vector3d19.Rotate(Math.PI / 2.0, Vector3d.ZAxis);
		vector3.Rotate(Math.PI / 2.0, Vector3d.ZAxis);
		CSBlockUtil.CSUtilMath.LineInt(out var outpoint, point3, vector2, point3d + vector3d * distance, vector3d11);
		CSBlockUtil.CSUtilMath.LineInt(out var outpoint2, outpoint, vector3d11, point3d2 + vector3d2 * distance, vector3d12);
		CSBlockUtil.CSUtilMath.LineInt(out var outpoint3, outpoint2, vector3d12, point3d3 + vector3d3 * distance, vector3d13);
		CSBlockUtil.CSUtilMath.LineInt(out var outpoint4, outpoint3, vector3d13, point3d4 + vector3d4 * distance, vector3d14);
		CSBlockUtil.CSUtilMath.LineInt(out var outpoint5, outpoint4, vector3d14, point3d5 + vector3d5 * distance, vector3d15);
		CSBlockUtil.CSUtilMath.LineInt(out var outpoint6, outpoint5, vector3d15, point3d6 + vector3d6 * distance, vector3d16);
		CSBlockUtil.CSUtilMath.LineInt(out var outpoint7, outpoint6, vector3d16, point3d7 + vector3d7 * distance, vector3d17);
		CSBlockUtil.CSUtilMath.LineInt(out var outpoint8, outpoint7, vector3d17, point3d8 + vector3d8 * distance, vector3d18);
		CSBlockUtil.CSUtilMath.LineInt(out var outpoint9, outpoint8, vector3d18, point3d9 + vector3d9 * distance, vector3d19);
		CSBlockUtil.CSUtilMath.LineInt(out var outpoint10, outpoint9, vector3d19, point3d10 + vector3d10 * distance, vector3);
		Point3d pointOnInterior = new Point3d(point3.X, point3.Y, 0.0);
		Point3d point3d12 = new Point3d(outpoint.X, outpoint.Y, 0.0);
		Point3d pointOnInterior2 = new Point3d(outpoint2.X, outpoint2.Y, 0.0);
		Point3d point3d13 = new Point3d(outpoint3.X, outpoint3.Y, 0.0);
		Point3d pointOnInterior3 = new Point3d(outpoint4.X, outpoint4.Y, 0.0);
		Point3d point3d14 = new Point3d(outpoint5.X, outpoint5.Y, 0.0);
		Point3d pointOnInterior4 = new Point3d(outpoint6.X, outpoint6.Y, 0.0);
		Point3d point3d15 = new Point3d(outpoint7.X, outpoint7.Y, 0.0);
		Point3d pointOnInterior5 = new Point3d(outpoint8.X, outpoint8.Y, 0.0);
		Point3d point3d16 = new Point3d(outpoint9.X, outpoint9.Y, 0.0);
		Point3d pointOnInterior6 = new Point3d(outpoint10.X, outpoint10.Y, 0.0);
		Point3d endPoint = new Point3d(point3d11.X, point3d11.Y, 0.0);
		new Point3d(0.0 - outpoint10.X, outpoint10.Y, 0.0);
		new Point3d(0.0 - outpoint9.X, outpoint9.Y, 0.0);
		new Point3d(0.0 - outpoint8.X, outpoint8.Y, 0.0);
		new Point3d(0.0 - outpoint7.X, outpoint7.Y, 0.0);
		new Point3d(0.0 - outpoint6.X, outpoint6.Y, 0.0);
		new Point3d(0.0 - outpoint5.X, outpoint5.Y, 0.0);
		new Point3d(0.0 - outpoint4.X, outpoint4.Y, 0.0);
		new Point3d(0.0 - outpoint3.X, outpoint3.Y, 0.0);
		new Point3d(0.0 - outpoint2.X, outpoint2.Y, 0.0);
		Point3d startPoint = new Point3d(0.0 - outpoint.X, outpoint.Y, 0.0);
		NurbsCurve nurbsCurve = new Arc(startPoint, pointOnInterior, point3d12).ToNurbsCurve();
		Curve curve = nurbsCurve.Trim(new Interval(nurbsCurve.Domain.Max / 2.0, nurbsCurve.Domain.Max));
		Curve curve2 = nurbsCurve.Trim(new Interval(nurbsCurve.Domain.Min, nurbsCurve.Domain.Max / 2.0));
		NurbsCurve nurbsCurve2 = new Arc(point3d12, pointOnInterior2, point3d13).ToNurbsCurve();
		NurbsCurve nurbsCurve3 = new Arc(point3d13, pointOnInterior3, point3d14).ToNurbsCurve();
		NurbsCurve nurbsCurve4 = new Arc(point3d14, pointOnInterior4, point3d15).ToNurbsCurve();
		NurbsCurve nurbsCurve5 = new Arc(point3d15, pointOnInterior5, point3d16).ToNurbsCurve();
		NurbsCurve nurbsCurve6 = new Arc(point3d16, pointOnInterior6, endPoint).ToNurbsCurve();
		Transform xform = Transform.Mirror(Plane.WorldYZ);
		NurbsCurve nurbsCurve7 = nurbsCurve2.ToNurbsCurve();
		nurbsCurve2.Transform(xform);
		nurbsCurve2.Reverse();
		NurbsCurve nurbsCurve8 = nurbsCurve3.ToNurbsCurve();
		nurbsCurve8.Transform(xform);
		nurbsCurve8.Reverse();
		NurbsCurve nurbsCurve9 = nurbsCurve4.ToNurbsCurve();
		nurbsCurve9.Transform(xform);
		nurbsCurve9.Reverse();
		NurbsCurve nurbsCurve10 = nurbsCurve5.ToNurbsCurve();
		nurbsCurve10.Transform(xform);
		nurbsCurve10.Reverse();
		NurbsCurve nurbsCurve11 = nurbsCurve6.ToNurbsCurve();
		nurbsCurve11.Transform(xform);
		nurbsCurve11.Reverse();
		if (cornerBlend > 0.0)
		{
			TrimCurve(nurbsCurve7, cornerBlend, CurveEnd.End, out var crvTrimmed);
			TrimCurve(nurbsCurve3, cornerBlend, CurveEnd.Start, out var crvTrimmed2);
			TrimCurve(nurbsCurve6, cornerBlend, CurveEnd.End, out var crvTrimmed3);
			TrimCurve(nurbsCurve11, cornerBlend, CurveEnd.Start, out var crvTrimmed4);
			TrimCurve(nurbsCurve8, cornerBlend, CurveEnd.End, out var crvTrimmed5);
			TrimCurve(nurbsCurve2, cornerBlend, CurveEnd.Start, out var crvTrimmed6);
			BlendCorner(ref crvTrimmed, ref crvTrimmed2);
			BlendCorner(ref crvTrimmed3, ref crvTrimmed4);
			BlendCorner(ref crvTrimmed5, ref crvTrimmed6);
			nurbsCurve7 = crvTrimmed.ToNurbsCurve();
			nurbsCurve3 = crvTrimmed2.ToNurbsCurve();
			nurbsCurve6 = crvTrimmed3.ToNurbsCurve();
			nurbsCurve11 = crvTrimmed4.ToNurbsCurve();
			nurbsCurve8 = crvTrimmed5.ToNurbsCurve();
			nurbsCurve2 = crvTrimmed6.ToNurbsCurve();
		}
		return new Curve[12]
		{
			curve, nurbsCurve7, nurbsCurve3, nurbsCurve4, nurbsCurve5, nurbsCurve6, nurbsCurve11, nurbsCurve10, nurbsCurve9, nurbsCurve8,
			nurbsCurve2, curve2
		};
	}

	private static Curve[] GemOffsetCurvesTrillionStraight(CSGemData gem, double distance, double cornerBlend = 0.0)
	{
		double num = gem.GemXSize / 2.0 + distance * 1.4;
		double num2 = gem.GemYSize / 2.0 + distance * 1.4;
		Point3d point = new Point3d(0.0, num2 * 0.7576, 0.0);
		Point3d point2 = new Point3d(num * 0.7708, num2 * 0.7378, 0.0);
		Point3d point3 = new Point3d(num, num2 * 0.6212, 0.0);
		Point3d point4 = new Point3d(num * 0.9792, num2 * 0.3493, 0.0);
		Point3d point5 = new Point3d(num * 0.6103, (0.0 - num2) * 0.378, 0.0);
		Point3d point3d = new Point3d(num * 0.2085, (0.0 - num2) * 1.0871, 0.0);
		Point3d point6 = new Point3d(0.0, (0.0 - num2) * 1.2424, 0.0);
		Vector3d vector = new Vector3d(point.Y - point2.Y, 0.0 - (point.X - point2.X), 0.0);
		vector.Unitize();
		Vector3d vector2 = new Vector3d(point2.Y - point3.Y, 0.0 - (point2.X - point3.X), 0.0);
		vector2.Unitize();
		Vector3d vector3 = new Vector3d(point3.Y - point4.Y, 0.0 - (point3.X - point4.X), 0.0);
		vector3.Unitize();
		Vector3d vector4 = new Vector3d(point4.Y - point5.Y, 0.0 - (point4.X - point5.X), 0.0);
		vector4.Unitize();
		Vector3d vector5 = new Vector3d(point5.Y - point3d.Y, 0.0 - (point5.X - point3d.X), 0.0);
		vector5.Unitize();
		Vector3d vector6 = new Vector3d(point3d.Y - point6.Y, 0.0 - (point3d.X - point6.X), 0.0);
		vector6.Unitize();
		Point3d point7 = new Point3d(point);
		Point3d point8 = new Point3d(point6);
		Vector3d vector7 = new Vector3d(vector);
		Vector3d vector3d = new Vector3d(vector2);
		Vector3d vector3d2 = new Vector3d(vector3);
		Vector3d vector3d3 = new Vector3d(vector4);
		Vector3d vector3d4 = new Vector3d(vector5);
		Vector3d vector8 = new Vector3d(vector6);
		vector7.Rotate(Math.PI / 2.0, Vector3d.ZAxis);
		vector3d.Rotate(Math.PI / 2.0, Vector3d.ZAxis);
		vector3d2.Rotate(Math.PI / 2.0, Vector3d.ZAxis);
		vector3d3.Rotate(Math.PI / 2.0, Vector3d.ZAxis);
		vector3d4.Rotate(Math.PI / 2.0, Vector3d.ZAxis);
		vector8.Rotate(Math.PI / 2.0, Vector3d.ZAxis);
		CSBlockUtil.CSUtilMath.LineInt(out var outpoint, point7, vector7, point2, vector3d);
		CSBlockUtil.CSUtilMath.LineInt(out var outpoint2, outpoint, vector3d, point3, vector3d2);
		CSBlockUtil.CSUtilMath.LineInt(out var outpoint3, outpoint2, vector3d2, point4, vector3d3);
		CSBlockUtil.CSUtilMath.LineInt(out var outpoint4, outpoint3, vector3d3, point5, vector3d4);
		CSBlockUtil.CSUtilMath.LineInt(out var outpoint5, outpoint4, vector3d4, point8, vector8);
		Point3d point3d2 = new Point3d(point7.X, point7.Y, 0.0);
		Point3d point3d3 = new Point3d(outpoint.X, outpoint.Y, 0.0);
		Point3d point3d4 = new Point3d(outpoint2.X, outpoint2.Y, 0.0);
		Point3d point3d5 = new Point3d(outpoint3.X, outpoint3.Y, 0.0);
		Point3d point3d6 = new Point3d(outpoint4.X, outpoint4.Y, 0.0);
		Point3d point3d7 = new Point3d(outpoint5.X, outpoint5.Y, 0.0);
		Point3d point3d8 = new Point3d(point8.X, point8.Y, 0.0);
		Point3d point3d9 = new Point3d(0.0 - outpoint5.X, outpoint5.Y, 0.0);
		Point3d point3d10 = new Point3d(0.0 - outpoint4.X, outpoint4.Y, 0.0);
		Point3d point3d11 = new Point3d(0.0 - outpoint3.X, outpoint3.Y, 0.0);
		Point3d point3d12 = new Point3d(0.0 - outpoint2.X, outpoint2.Y, 0.0);
		Point3d point3d13 = new Point3d(0.0 - outpoint.X, outpoint.Y, 0.0);
		Curve curve = new LineCurve(point3d2, point3d3);
		Curve curve2 = new LineCurve(point3d3, point3d4);
		Curve curve3 = new LineCurve(point3d4, point3d5);
		Curve curve4 = new LineCurve(point3d5, point3d6);
		Curve curve5 = new LineCurve(point3d6, point3d7);
		Curve curve6 = new LineCurve(point3d7, point3d8);
		Curve curve7 = new LineCurve(point3d8, point3d9);
		Curve curve8 = new LineCurve(point3d9, point3d10);
		Curve curve9 = new LineCurve(point3d10, point3d11);
		Curve curve10 = new LineCurve(point3d11, point3d12);
		Curve curve11 = new LineCurve(point3d12, point3d13);
		Curve curve12 = new LineCurve(point3d13, point3d2);
		if (cornerBlend > 0.0)
		{
			TrimCurve(curve, cornerBlend, CurveEnd.End, out var crvTrimmed);
			TrimCurve(curve2, cornerBlend, CurveEnd.Both, out var crvTrimmed2);
			TrimCurve(curve3, cornerBlend, CurveEnd.Both, out var crvTrimmed3);
			TrimCurve(curve4, cornerBlend, CurveEnd.Both, out var crvTrimmed4);
			TrimCurve(curve5, cornerBlend, CurveEnd.Both, out var crvTrimmed5);
			TrimCurve(curve6, cornerBlend, CurveEnd.Both, out var crvTrimmed6);
			TrimCurve(curve7, cornerBlend, CurveEnd.Both, out var crvTrimmed7);
			TrimCurve(curve8, cornerBlend, CurveEnd.Both, out var crvTrimmed8);
			TrimCurve(curve9, cornerBlend, CurveEnd.Both, out var crvTrimmed9);
			TrimCurve(curve10, cornerBlend, CurveEnd.Both, out var crvTrimmed10);
			TrimCurve(curve11, cornerBlend, CurveEnd.Both, out var crvTrimmed11);
			TrimCurve(curve12, cornerBlend, CurveEnd.Start, out var crvTrimmed12);
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
			curve = crvTrimmed;
			curve2 = crvTrimmed2;
			curve3 = crvTrimmed3;
			curve4 = crvTrimmed4;
			curve5 = crvTrimmed5;
			curve6 = crvTrimmed6;
			curve7 = crvTrimmed7;
			curve8 = crvTrimmed8;
			curve9 = crvTrimmed9;
			curve10 = crvTrimmed10;
			curve11 = crvTrimmed11;
			curve12 = crvTrimmed12;
		}
		return new Curve[12]
		{
			curve, curve2, curve3, curve4, curve5, curve6, curve7, curve8, curve9, curve10,
			curve11, curve12
		};
	}

	private static bool TrimCurve(Curve crv, double trimDistance, CurveEnd whichCurveEnd, out Curve crvTrimmed)
	{
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
		Curve curve = crv.DuplicateCurve();
		crvTrimmed = ((whichCurveEnd == CurveEnd.None) ? curve : curve.Trim(whichCurveEnd, trimDistance));
		return true;
	}
}
