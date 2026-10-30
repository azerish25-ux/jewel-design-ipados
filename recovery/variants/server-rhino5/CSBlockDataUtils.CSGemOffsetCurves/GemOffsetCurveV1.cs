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

	private static Curve[] GemOffsetCurvesCushion(CSGemData gem, double distance)
	{
		double num = gem.GemXSize / 2.0;
		double num2 = gem.GemYSize / 2.0;
		double num3 = num / 4.0;
		double num4 = num2 / 5.0;
		Point3d point3d = new Point3d(0.0, 5.0 * num4, 0.0);
		Point3d point3d2 = new Point3d(-2.18319 * num3, 4.54508 * num4, 0.0);
		Point3d point3d3 = new Point3d(2.18319 * num3, 4.54508 * num4, 0.0);
		Arc arc = new Arc(point3d2, point3d, point3d3);
		Vector3d vector3d = new Vector3d(point3d2 - arc.Center);
		vector3d.Unitize();
		Vector3d vector3d2 = new Vector3d(point3d - arc.Center);
		vector3d2.Unitize();
		Vector3d vector3d3 = new Vector3d(point3d3 - arc.Center);
		vector3d3.Unitize();
		Arc arc2 = new Arc(point3d2 + vector3d * distance, point3d + vector3d2 * distance, point3d3 + vector3d3 * distance);
		Point3d point3d4 = new Point3d(4.0 * num3, 0.0, 0.0);
		Point3d point3d5 = new Point3d(3.63684 * num3, 2.72661 * num4, 0.0);
		Point3d point3d6 = new Point3d(3.63684 * num3, -2.72661 * num4, 0.0);
		Arc arc3 = new Arc(point3d5, point3d4, point3d6);
		Vector3d vector3d4 = new Vector3d(point3d5 - arc3.Center);
		vector3d4.Unitize();
		Vector3d vector3d5 = new Vector3d(point3d4 - arc3.Center);
		vector3d5.Unitize();
		Vector3d vector3d6 = new Vector3d(point3d6 - arc3.Center);
		vector3d6.Unitize();
		Arc arc4 = new Arc(point3d5 + vector3d4 * distance, point3d4 + vector3d5 * distance, point3d6 + vector3d6 * distance);
		if (arc2.EndPoint.X > arc4.StartPoint.X || arc4.StartPoint.Y > arc2.EndPoint.Y)
		{
			CSBlockUtil.CSUtilMath.GetUnscaledXforms(gem.Xform, out var _, out var xDown);
			NurbsCurve nurbsCurve = gem.GetNurbsCurve();
			nurbsCurve.Transform(xDown);
			return new Curve[1] { CSUtilGeometry.OriginOffsetOldXY(nurbsCurve, distance, closed: true) };
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

	private static Curve[] GemOffsetCurvesEmerald(CSGemData gem, double distance, double cornerPercentX = 0.769277, double cornerPercentY = 0.8462)
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
		return new Curve[9]
		{
			new LineCurve(point3d8, point3d9),
			new LineCurve(point3d9, point3d10),
			new LineCurve(point3d10, point3d11),
			new LineCurve(point3d11, point3d12),
			new LineCurve(point3d12, point3d13),
			new LineCurve(point3d13, point3d14),
			new LineCurve(point3d14, point3d15),
			new LineCurve(point3d15, point3d16),
			new LineCurve(point3d16, point3d8)
		};
	}

	private static Curve[] GemOffsetCurvesEmeraldSquare(CSGemData gem, double distance)
	{
		return GemOffsetCurvesEmerald(gem, distance, 0.7688, 0.7688);
	}

	private static Curve[] GemOffsetCurvesHeart(CSGemData gem, double distance)
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
		if (num9 < 0.04)
		{
			num5 = -2.7953078235558855;
			distance = num5 / num3;
			return GemOffsetCurvesHeart(gem, distance);
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
		return new Curve[6] { nurbsCurve3, nurbsCurve2, nurbsCurve, nurbsCurve4, nurbsCurve5, nurbsCurve6 };
	}

	private static Curve[] GemOffsetCurvesMarquise(CSGemData gem, double distance)
	{
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
		Point3d startPoint = new Point3d(0.0, num6, 0.0);
		Point3d pointOnInterior = new Point3d(2.5 + num4, 0.0, 0.0);
		Point3d endPoint = new Point3d(0.0, 0.0 - num6, 0.0);
		Arc arc = new Arc(startPoint, pointOnInterior, endPoint);
		Transform xform = Transform.Scale(Plane.WorldXY, num / 2.5, num2 / 5.0, 1.0);
		NurbsCurve nurbsCurve = arc.ToNurbsCurve();
		nurbsCurve.Transform(xform);
		Transform xform2 = Transform.Scale(Plane.WorldXY, -1.0, -1.0, 1.0);
		NurbsCurve nurbsCurve2 = nurbsCurve.ToNurbsCurve();
		nurbsCurve2.Transform(xform2);
		return new Curve[2] { nurbsCurve, nurbsCurve2 };
	}

	private static Curve[] GemOffsetCurvesPear(CSGemData gem, double distance)
	{
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
		NurbsCurve nurbsCurve = new NurbsCurve(3, rational: false, 4, 8);
		nurbsCurve.Points[0] = new ControlPoint(0.0, (num10 - 0.329747) * num4, 0.0, 1.0);
		nurbsCurve.Points[1] = new ControlPoint(0.20354483588156877 * num9 * num3, (0.9940940639751765 * num10 - 0.329747) * num4, 0.0, 1.0);
		nurbsCurve.Points[2] = new ControlPoint(0.6123362632073628 * num9 * num3, (0.8879139319627639 * num10 - 0.329747) * num4, 0.0, 1.0);
		nurbsCurve.Points[3] = new ControlPoint(1.012658540190205 * num9 * num3, (0.3703501564029002 * num10 - 0.329747) * num4, 0.0, 1.0);
		nurbsCurve.Points[4] = new ControlPoint(1.016703337103465 * num9 * num3, (-0.2872952282636081 * num12 - 0.329747) * num4, 0.0, 1.0);
		nurbsCurve.Points[5] = new ControlPoint(0.5443786297392439 * num9 * num3, (-0.7315634760021571 * num12 - 0.329747) * num4, 0.0, 1.0);
		nurbsCurve.Points[6] = new ControlPoint(0.19128191296704555 * num9 * num3, (-0.9245961035017445 * num12 - 0.329747) * num4, 0.0, 1.0);
		nurbsCurve.Points[7] = new ControlPoint(0.0, (-1.0 * num12 - 0.329747) * num4, 0.0, 1.0);
		nurbsCurve.Knots[0] = 0.0;
		nurbsCurve.Knots[1] = 0.0;
		nurbsCurve.Knots[2] = 0.0;
		nurbsCurve.Knots[3] = 0.2;
		nurbsCurve.Knots[4] = 0.4;
		nurbsCurve.Knots[5] = 0.6;
		nurbsCurve.Knots[6] = 0.8;
		nurbsCurve.Knots[7] = 1.0;
		nurbsCurve.Knots[8] = 1.0;
		nurbsCurve.Knots[9] = 1.0;
		Transform xform = Transform.Scale(Plane.WorldXY, -1.0, 1.0, 1.0);
		NurbsCurve nurbsCurve2 = nurbsCurve.ToNurbsCurve();
		nurbsCurve2.Transform(xform);
		nurbsCurve2.Reverse();
		return new Curve[2] { nurbsCurve, nurbsCurve2 };
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

	private static Curve[] GemOffsetCurvesSquare(CSGemData gem, double distance)
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
		LineCurve lineCurve = new LineCurve(point3d, point3d2);
		LineCurve lineCurve2 = new LineCurve(point3d2, point3d3);
		LineCurve lineCurve3 = new LineCurve(point3d3, point3d4);
		LineCurve lineCurve4 = new LineCurve(point3d4, point3d5);
		LineCurve lineCurve5 = new LineCurve(point3d5, point3d);
		return new Curve[5] { lineCurve, lineCurve2, lineCurve3, lineCurve4, lineCurve5 };
	}
}
