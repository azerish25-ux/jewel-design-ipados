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
			nurbsCurve.Transform(xDown);
			array = new Curve[1] { CSUtilGeometry.OriginOffsetOldXY(nurbsCurve, distance, closed: true) };
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
		OnCurve[] array2 = new OnCurve[array.Length];
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
		NurbsCurve nurbsCurve = CSUtilCurves.BlendCurve(curve1, curve2);
		nurbsCurve = nurbsCurve.Trim(new Interval(0.0, 0.5)).ToNurbsCurve();
		CSUtilCurves.MergeCurves((IEnumerable<Curve>)new Curve[2] { curve1, nurbsCurve }, out Curve[] mergedCrvs, 0.01);
		curve1 = mergedCrvs[0];
		NurbsCurve nurbsCurve2 = CSUtilCurves.BlendCurve(curve1, curve2);
		CSUtilCurves.MergeCurves((IEnumerable<Curve>)new Curve[2] { curve2, nurbsCurve2 }, out Curve[] mergedCrvs2, 0.01);
		curve2 = mergedCrvs2[0];
	}

	private static Curve[] GemOffsetCurvesBaguetteTapered(CSGemData gem, double distance)
	{
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
		Point3d point3d = new Point3d(0.0, num3, 0.0);
		Point3d point3d2 = new Point3d(num, num3, 0.0);
		Point3d point3d3 = new Point3d(num2, 0.0 - num3, 0.0);
		Point3d point3d4 = new Point3d(0.0 - num2, 0.0 - num3, 0.0);
		Point3d point3d5 = new Point3d(0.0 - num, num3, 0.0);
		LineCurve lineCurve = new LineCurve(point3d, point3d2);
		LineCurve lineCurve2 = new LineCurve(point3d2, point3d3);
		LineCurve lineCurve3 = new LineCurve(point3d3, point3d4);
		LineCurve lineCurve4 = new LineCurve(point3d4, point3d5);
		LineCurve lineCurve5 = new LineCurve(point3d5, point3d);
		return new Curve[5] { lineCurve, lineCurve2, lineCurve3, lineCurve4, lineCurve5 };
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
		Arc arc4 = new Arc(startPoint, pointOnInterior, endPoint);
		arc3.Trim(new Interval(arc3.AngleDomain.Mid, arc3.AngleDomain.Max));
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
		Curve curve1A = new ArcCurve(arc3);
		Curve curve = new ArcCurve(arc5);
		Curve curve2 = new ArcCurve(arc6);
		Curve curve3 = new ArcCurve(arc7);
		Curve curve1B = new ArcCurve(arc4);
		if (cornerBlend > 0.0)
		{
			Trim5Curves(cornerBlend, ref curve1A, ref curve, ref curve2, ref curve3, ref curve1B);
			BlendCorner(ref curve1A, ref curve);
			BlendCorner(ref curve, ref curve2);
			BlendCorner(ref curve2, ref curve3);
			BlendCorner(ref curve3, ref curve1B);
		}
		return new Curve[5] { curve1A, curve, curve2, curve3, curve1B };
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
		return new Curve[9] { curve, curve2, curve3, curve4, curve5, curve6, curve7, curve8, curve9 };
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
			Point3d pointAtEnd = nurbsCurve.PointAtEnd;
			Vector3d vector3d = new Vector3d(nurbsCurve.PointAtEnd - nurbsCurve.PointAtStart);
			vector3d.Unitize();
			pointAtEnd -= vector3d * cornerBlend;
			nurbsCurve.ClosestPoint(pointAtEnd, out var t);
			nurbsCurve = nurbsCurve.Trim(new Interval(nurbsCurve.Domain.Min, t)).ToNurbsCurve();
			pointAtEnd = nurbsCurve4.PointAtStart;
			vector3d = new Vector3d(nurbsCurve4.PointAtEnd - nurbsCurve4.PointAtStart);
			vector3d.Unitize();
			pointAtEnd += vector3d * cornerBlend;
			nurbsCurve4.ClosestPoint(pointAtEnd, out t);
			nurbsCurve4 = nurbsCurve4.Trim(new Interval(t, nurbsCurve4.Domain.Max)).ToNurbsCurve();
			Curve curve = nurbsCurve;
			Curve curve2 = nurbsCurve4;
			BlendCorner(ref curve, ref curve2);
			nurbsCurve = curve.ToNurbsCurve();
			nurbsCurve4 = curve2.ToNurbsCurve();
			pointAtEnd = nurbsCurve6.PointAtEnd;
			vector3d = new Vector3d(nurbsCurve6.PointAtEnd - nurbsCurve6.PointAtStart);
			vector3d.Unitize();
			pointAtEnd -= vector3d * cornerBlend;
			nurbsCurve6.ClosestPoint(pointAtEnd, out t);
			nurbsCurve6 = nurbsCurve6.Trim(new Interval(nurbsCurve6.Domain.Min, t)).ToNurbsCurve();
			pointAtEnd = nurbsCurve3.PointAtStart;
			vector3d = new Vector3d(nurbsCurve3.PointAtEnd - nurbsCurve3.PointAtStart);
			vector3d.Unitize();
			pointAtEnd += vector3d * cornerBlend;
			nurbsCurve3.ClosestPoint(pointAtEnd, out t);
			nurbsCurve3 = nurbsCurve3.Trim(new Interval(t, nurbsCurve3.Domain.Max)).ToNurbsCurve();
			Curve curve3 = nurbsCurve3;
			Curve curve4 = nurbsCurve6;
			BlendCorner(ref curve4, ref curve3);
			nurbsCurve3 = curve3.ToNurbsCurve();
			nurbsCurve6 = curve4.ToNurbsCurve();
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
			Point3d pointAtStart = nurbsCurve2.PointAtStart;
			pointAtStart += new Vector3d(0.0, cornerBlend, 0.0);
			nurbsCurve2.ClosestPoint(pointAtStart, out var t);
			nurbsCurve2 = nurbsCurve2.Trim(new Interval(t, nurbsCurve2.Domain.Max)).ToNurbsCurve();
			pointAtStart = nurbsCurve2.PointAtEnd;
			pointAtStart += new Vector3d(0.0, 0.0 - cornerBlend, 0.0);
			nurbsCurve2.ClosestPoint(pointAtStart, out t);
			nurbsCurve2 = nurbsCurve2.Trim(new Interval(nurbsCurve2.Domain.Min, t)).ToNurbsCurve();
			pointAtStart = nurbsCurve.PointAtStart;
			pointAtStart += new Vector3d(0.0, 0.0 - cornerBlend, 0.0);
			nurbsCurve.ClosestPoint(pointAtStart, out t);
			nurbsCurve = nurbsCurve.Trim(new Interval(t, nurbsCurve.Domain.Max)).ToNurbsCurve();
			pointAtStart = nurbsCurve.PointAtEnd;
			pointAtStart += new Vector3d(0.0, cornerBlend, 0.0);
			nurbsCurve.ClosestPoint(pointAtStart, out t);
			nurbsCurve = nurbsCurve.Trim(new Interval(nurbsCurve.Domain.Min, t)).ToNurbsCurve();
			Curve curve = nurbsCurve;
			Curve curve2 = nurbsCurve2;
			BlendCorner(ref curve, ref curve2);
			BlendCorner(ref curve2, ref curve);
			nurbsCurve = curve.ToNurbsCurve();
			nurbsCurve2 = curve2.ToNurbsCurve();
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
		Transform xform2 = Transform.Scale(Plane.WorldXY, 1.0, -1.0, 1.0);
		Curve curve3 = curve.DuplicateCurve();
		curve3.Transform(xform2);
		curve3.Reverse();
		Curve curve4 = curve2.DuplicateCurve();
		curve4.Transform(xform2);
		curve4.Reverse();
		return new Curve[4] { curve, curve3, curve4, curve2 };
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
		Vector3d inputVector = new Vector3d(point3d3 - point3d);
		vector3d.Unitize();
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
		Transform xform = Transform.Scale(Plane.WorldXY, -1.0, 1.0, 1.0);
		Curve curve = nurbsCurve2.DuplicateCurve();
		curve.Transform(xform);
		curve.Reverse();
		Curve curve2 = nurbsCurve.DuplicateCurve();
		curve2.Transform(xform);
		curve2.Reverse();
		if (gem.VersionMajor > 1)
		{
			nurbsCurve2.Translate(new Vector3d(0.0, 0.08 * gemYSize, 0.0));
			nurbsCurve.Translate(new Vector3d(0.0, 0.08 * gemYSize, 0.0));
			curve.Translate(new Vector3d(0.0, 0.08 * gemYSize, 0.0));
			curve2.Translate(new Vector3d(0.0, 0.08 * gemYSize, 0.0));
		}
		if (cornerBlend > 0.0)
		{
			Point3d pointAtEnd2 = nurbsCurve2.PointAtEnd;
			Vector3d vector3d2 = new Vector3d(nurbsCurve2.PointAtEnd - nurbsCurve2.PointAtStart);
			vector3d2.Unitize();
			pointAtEnd2 -= vector3d2 * cornerBlend;
			nurbsCurve2.ClosestPoint(pointAtEnd2, out var t);
			nurbsCurve2 = nurbsCurve2.Trim(new Interval(nurbsCurve2.Domain.Min, t)).ToNurbsCurve();
			pointAtEnd2 = curve.PointAtStart;
			vector3d2 = new Vector3d(curve.PointAtEnd - curve.PointAtStart);
			vector3d2.Unitize();
			pointAtEnd2 += vector3d2 * cornerBlend;
			curve.ClosestPoint(pointAtEnd2, out t);
			curve = curve.Trim(new Interval(t, curve.Domain.Max)).ToNurbsCurve();
			Curve curve3 = nurbsCurve2;
			Curve curve4 = curve;
			BlendCorner(ref curve3, ref curve4);
			nurbsCurve2 = curve3.ToNurbsCurve();
			curve = curve4.ToNurbsCurve();
		}
		return new Curve[4] { nurbsCurve, nurbsCurve2, curve, curve2 };
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
		Transform transform2 = Transform.Scale(Plane.WorldXY, -1.0, 1.0, 1.0);
		Transform transform3 = Transform.Scale(Plane.WorldXY, num, num2, 0.0);
		NurbsCurve nurbsCurve = new Circle(Point3d.Origin, 1.0).ToNurbsCurve();
		nurbsCurve.Transform(transform3 * transform2 * transform);
		return new Curve[1] { nurbsCurve };
	}

	private static Curve[] GemOffsetCurvesSquare(CSGemData gem, double distance, double cornerBlend)
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
		Curve curve = new LineCurve(point3d, point3d2 + new Vector3d(0.0 - cornerBlend, 0.0, 0.0));
		Curve curve2 = new LineCurve(point3d2 + new Vector3d(0.0, 0.0 - cornerBlend, 0.0), point3d3 + new Vector3d(0.0, cornerBlend, 0.0));
		Curve curve3 = new LineCurve(point3d3 + new Vector3d(0.0 - cornerBlend, 0.0, 0.0), point3d4 + new Vector3d(cornerBlend, 0.0, 0.0));
		Curve curve4 = new LineCurve(point3d4 + new Vector3d(0.0, cornerBlend, 0.0), point3d5 + new Vector3d(0.0, 0.0 - cornerBlend, 0.0));
		Curve curve5 = new LineCurve(point3d5 + new Vector3d(cornerBlend, 0.0, 0.0), point3d);
		if (cornerBlend > 0.0)
		{
			BlendCorner(ref curve, ref curve2);
			BlendCorner(ref curve2, ref curve3);
			BlendCorner(ref curve3, ref curve4);
			BlendCorner(ref curve4, ref curve5);
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

	private static void Trim5Curves(double cornerBlend, ref Curve curve1A, ref Curve curve2, ref Curve curve3, ref Curve curve4, ref Curve curve1B)
	{
		Point3d pointAtEnd = curve1A.PointAtEnd;
		Vector3d vector3d = new Vector3d(pointAtEnd - curve1A.PointAtStart);
		vector3d.Unitize();
		vector3d *= cornerBlend;
		pointAtEnd -= vector3d;
		curve1A.ClosestPoint(pointAtEnd, out var t);
		curve1A = curve1A.Trim(new Interval(curve1A.Domain.Min, t));
		pointAtEnd = curve2.PointAtStart;
		vector3d = new Vector3d(pointAtEnd - curve2.PointAtEnd);
		vector3d.Unitize();
		vector3d *= cornerBlend;
		pointAtEnd -= vector3d;
		curve2.ClosestPoint(pointAtEnd, out t);
		curve2 = curve2.Trim(new Interval(t, curve2.Domain.Max));
		pointAtEnd = curve2.PointAtEnd;
		vector3d = new Vector3d(pointAtEnd - curve2.PointAtStart);
		vector3d.Unitize();
		vector3d *= cornerBlend;
		pointAtEnd -= vector3d;
		curve2.ClosestPoint(pointAtEnd, out t);
		curve2 = curve2.Trim(new Interval(curve2.Domain.Min, t));
		pointAtEnd = curve3.PointAtStart;
		vector3d = new Vector3d(pointAtEnd - curve3.PointAtEnd);
		vector3d.Unitize();
		vector3d *= cornerBlend;
		pointAtEnd -= vector3d;
		curve3.ClosestPoint(pointAtEnd, out t);
		curve3 = curve3.Trim(new Interval(t, curve3.Domain.Max));
		pointAtEnd = curve3.PointAtEnd;
		vector3d = new Vector3d(pointAtEnd - curve3.PointAtStart);
		vector3d.Unitize();
		vector3d *= cornerBlend;
		pointAtEnd -= vector3d;
		curve3.ClosestPoint(pointAtEnd, out t);
		curve3 = curve3.Trim(new Interval(curve3.Domain.Min, t));
		pointAtEnd = curve4.PointAtStart;
		vector3d = new Vector3d(pointAtEnd - curve4.PointAtEnd);
		vector3d.Unitize();
		vector3d *= cornerBlend;
		pointAtEnd -= vector3d;
		curve4.ClosestPoint(pointAtEnd, out t);
		curve4 = curve4.Trim(new Interval(t, curve4.Domain.Max));
		pointAtEnd = curve4.PointAtEnd;
		vector3d = new Vector3d(pointAtEnd - curve4.PointAtStart);
		vector3d.Unitize();
		vector3d *= cornerBlend;
		pointAtEnd -= vector3d;
		curve4.ClosestPoint(pointAtEnd, out t);
		curve4 = curve4.Trim(new Interval(curve4.Domain.Min, t));
		pointAtEnd = curve1B.PointAtStart;
		vector3d = new Vector3d(pointAtEnd - curve1B.PointAtEnd);
		vector3d.Unitize();
		vector3d *= cornerBlend;
		pointAtEnd -= vector3d;
		curve1B.ClosestPoint(pointAtEnd, out t);
		curve1B = curve1B.Trim(new Interval(t, curve1B.Domain.Max));
	}

	private static void Trim9Curves(double cornerBlend, ref Curve curve0, ref Curve curve1, ref Curve curve2, ref Curve curve3, ref Curve curve4, ref Curve curve5, ref Curve curve6, ref Curve curve7, ref Curve curve8)
	{
		double length = curve1.GetLength();
		double length2 = curve2.GetLength();
		double length3 = curve3.GetLength();
		double length4 = curve4.GetLength();
		double length5 = curve5.GetLength();
		double length6 = curve6.GetLength();
		double length7 = curve7.GetLength();
		double length8 = curve8.GetLength();
		Point3d pointAtEnd = curve0.PointAtEnd;
		Vector3d vector3d = new Vector3d(curve0.PointAtEnd - curve0.PointAtStart);
		vector3d.Unitize();
		pointAtEnd -= vector3d * cornerBlend;
		curve0.ClosestPoint(pointAtEnd, out var t);
		curve0 = curve0.Trim(new Interval(curve0.Domain.Min, t));
		pointAtEnd = curve1.PointAtStart;
		vector3d = new Vector3d(curve1.PointAtStart - curve1.PointAtEnd);
		vector3d.Unitize();
		double num = cornerBlend;
		if (cornerBlend * 2.0 > length)
		{
			num = length * 0.49;
		}
		pointAtEnd -= vector3d * num;
		curve1.ClosestPoint(pointAtEnd, out t);
		curve1 = curve1.Trim(new Interval(t, curve1.Domain.Max));
		pointAtEnd = curve1.PointAtEnd;
		vector3d = new Vector3d(curve1.PointAtEnd - curve1.PointAtStart);
		vector3d.Unitize();
		num = cornerBlend;
		if (cornerBlend * 2.0 > length)
		{
			num = length * 0.49;
		}
		pointAtEnd -= vector3d * num;
		curve1.ClosestPoint(pointAtEnd, out t);
		curve1 = curve1.Trim(new Interval(curve1.Domain.Min, t));
		pointAtEnd = curve2.PointAtStart;
		vector3d = new Vector3d(curve2.PointAtStart - curve2.PointAtEnd);
		vector3d.Unitize();
		num = cornerBlend;
		if (cornerBlend * 2.0 > length2)
		{
			num = length2 * 0.49;
		}
		pointAtEnd -= vector3d * num;
		curve2.ClosestPoint(pointAtEnd, out t);
		curve2 = curve2.Trim(new Interval(t, curve2.Domain.Max));
		pointAtEnd = curve2.PointAtEnd;
		vector3d = new Vector3d(curve2.PointAtEnd - curve2.PointAtStart);
		vector3d.Unitize();
		num = cornerBlend;
		if (cornerBlend * 2.0 > length2)
		{
			num = length2 * 0.49;
		}
		pointAtEnd -= vector3d * num;
		curve2.ClosestPoint(pointAtEnd, out t);
		curve2 = curve2.Trim(new Interval(curve2.Domain.Min, t));
		pointAtEnd = curve3.PointAtStart;
		vector3d = new Vector3d(curve3.PointAtStart - curve3.PointAtEnd);
		vector3d.Unitize();
		num = cornerBlend;
		if (cornerBlend * 2.0 > length3)
		{
			num = length3 * 0.49;
		}
		pointAtEnd -= vector3d * num;
		curve3.ClosestPoint(pointAtEnd, out t);
		curve3 = curve3.Trim(new Interval(t, curve3.Domain.Max));
		pointAtEnd = curve3.PointAtEnd;
		vector3d = new Vector3d(curve3.PointAtEnd - curve3.PointAtStart);
		vector3d.Unitize();
		num = cornerBlend;
		if (cornerBlend * 2.0 > length3)
		{
			num = length3 * 0.49;
		}
		pointAtEnd -= vector3d * num;
		curve3.ClosestPoint(pointAtEnd, out t);
		curve3 = curve3.Trim(new Interval(curve3.Domain.Min, t));
		pointAtEnd = curve4.PointAtStart;
		vector3d = new Vector3d(curve4.PointAtStart - curve4.PointAtEnd);
		vector3d.Unitize();
		num = cornerBlend;
		if (cornerBlend * 2.0 > length4)
		{
			num = length4 * 0.49;
		}
		pointAtEnd -= vector3d * num;
		curve4.ClosestPoint(pointAtEnd, out t);
		curve4 = curve4.Trim(new Interval(t, curve4.Domain.Max));
		pointAtEnd = curve4.PointAtEnd;
		vector3d = new Vector3d(curve4.PointAtEnd - curve4.PointAtStart);
		vector3d.Unitize();
		num = cornerBlend;
		if (cornerBlend * 2.0 > length4)
		{
			num = length4 * 0.49;
		}
		pointAtEnd -= vector3d * num;
		curve4.ClosestPoint(pointAtEnd, out t);
		curve4 = curve4.Trim(new Interval(curve4.Domain.Min, t));
		pointAtEnd = curve5.PointAtStart;
		vector3d = new Vector3d(curve5.PointAtStart - curve5.PointAtEnd);
		vector3d.Unitize();
		num = cornerBlend;
		if (cornerBlend * 2.0 > length5)
		{
			num = length5 * 0.49;
		}
		pointAtEnd -= vector3d * num;
		curve5.ClosestPoint(pointAtEnd, out t);
		curve5 = curve5.Trim(new Interval(t, curve5.Domain.Max));
		pointAtEnd = curve5.PointAtEnd;
		vector3d = new Vector3d(curve5.PointAtEnd - curve5.PointAtStart);
		vector3d.Unitize();
		num = cornerBlend;
		if (cornerBlend * 2.0 > length5)
		{
			num = length5 * 0.49;
		}
		pointAtEnd -= vector3d * num;
		curve5.ClosestPoint(pointAtEnd, out t);
		curve5 = curve5.Trim(new Interval(curve5.Domain.Min, t));
		pointAtEnd = curve6.PointAtStart;
		vector3d = new Vector3d(curve6.PointAtStart - curve6.PointAtEnd);
		vector3d.Unitize();
		num = cornerBlend;
		if (cornerBlend * 2.0 > length6)
		{
			num = length6 * 0.49;
		}
		pointAtEnd -= vector3d * num;
		curve6.ClosestPoint(pointAtEnd, out t);
		curve6 = curve6.Trim(new Interval(t, curve6.Domain.Max));
		pointAtEnd = curve6.PointAtEnd;
		vector3d = new Vector3d(curve6.PointAtEnd - curve6.PointAtStart);
		vector3d.Unitize();
		num = cornerBlend;
		if (cornerBlend * 2.0 > length6)
		{
			num = length6 * 0.49;
		}
		pointAtEnd -= vector3d * num;
		curve6.ClosestPoint(pointAtEnd, out t);
		curve6 = curve6.Trim(new Interval(curve6.Domain.Min, t));
		pointAtEnd = curve7.PointAtStart;
		vector3d = new Vector3d(curve7.PointAtStart - curve7.PointAtEnd);
		vector3d.Unitize();
		num = cornerBlend;
		if (cornerBlend * 2.0 > length7)
		{
			num = length7 * 0.49;
		}
		pointAtEnd -= vector3d * num;
		curve7.ClosestPoint(pointAtEnd, out t);
		curve7 = curve7.Trim(new Interval(t, curve7.Domain.Max));
		pointAtEnd = curve7.PointAtEnd;
		vector3d = new Vector3d(curve7.PointAtEnd - curve7.PointAtStart);
		vector3d.Unitize();
		num = cornerBlend;
		if (cornerBlend * 2.0 > length7)
		{
			num = length7 * 0.49;
		}
		pointAtEnd -= vector3d * num;
		curve7.ClosestPoint(pointAtEnd, out t);
		curve7 = curve7.Trim(new Interval(curve7.Domain.Min, t));
		pointAtEnd = curve8.PointAtStart;
		vector3d = new Vector3d(curve8.PointAtEnd - curve8.PointAtStart);
		vector3d.Unitize();
		num = cornerBlend;
		if (cornerBlend * 2.0 > length8)
		{
			num = length8 * 0.49;
		}
		pointAtEnd += vector3d * num;
		curve8.ClosestPoint(pointAtEnd, out t);
		curve8 = curve8.Trim(new Interval(t, curve8.Domain.Max));
	}
}
