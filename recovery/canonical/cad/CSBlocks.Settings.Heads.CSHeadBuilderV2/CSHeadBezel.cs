using System;
using System.Collections.Generic;
using CS;
using CSBlockDataUtils.CSGemOffsetCurves;
using CSBlockUtil;
using CSDatas;
using GV.CounterSketch.Shared;
using Rhino;
using Rhino.Geometry;
using Rhino.Geometry.Intersect;

namespace CSBlocks.Settings.Heads.CSHeadBuilderV2;

internal static class CSHeadBezel
{
	public static bool BezelProfile(Curve prongRail, ProngParam prongParam, double prongTopOffset, double prongBaseOffset, ValueListItem baseStyle, bool planarDome, double bezelSeatHeight, double bezelSeatLength, double bezelSeatAngle, double bezelBlendPercentage, out Curve bezelProngOutline, out string error)
	{
		bezelProngOutline = null;
		Curve curve = new PolylineCurve(new List<Point3d>
		{
			new Point3d(0.0, 0.0, 0.0),
			new Point3d(-0.5, 0.0, 0.0),
			new Point3d(-0.5, 1.0, 0.0),
			new Point3d(0.5, 1.0, 0.0),
			new Point3d(0.5, 0.0, 0.0),
			new Point3d(0.0, 0.0, 0.0)
		});
		Curve curve2 = new PolylineCurve(new List<Point3d>
		{
			new Point3d(0.0, 0.0, 0.0),
			new Point3d(-0.5, 0.0, 0.0),
			new Point3d(-0.5, 1.0, 0.0),
			new Point3d(0.5, 1.0, 0.0),
			new Point3d(0.5, 0.0, 0.0),
			new Point3d(0.0, 0.0, 0.0)
		});
		Point3d pointAtStart = prongRail.PointAtStart;
		pointAtStart.Z = 0.0;
		Vector3d vector3d = new Vector3d(pointAtStart);
		vector3d.Unitize();
		Point3d point3d = new Point3d(5.0, 0.0, 0.0);
		Transform xform = CSBlockUtil.CSUtilMath.Orient3Point(prongRail.PointAtStart, vector3d, Vector3d.ZAxis, point3d, Vector3d.XAxis, Vector3d.ZAxis);
		Transform xform2 = CSBlockUtil.CSUtilMath.Orient3Point(point3d, Vector3d.XAxis, Vector3d.ZAxis, prongRail.PointAtStart, vector3d, Vector3d.ZAxis);
		prongRail = prongRail.DuplicateCurve();
		prongRail.Transform(xform);
		double num = CSBlockUtil.CSUtilMath.AngleBetweenVectorsWithDirection(new Vector3d(0.0, 0.0, -1.0), prongRail.TangentAt(prongRail.Domain.Min), -Vector3d.YAxis);
		double num2 = (planarDome ? (1.0 / Math.Cos(num)) : 1.0);
		Transform transform = Transform.Scale(Plane.WorldXY, prongParam.ProngTopX, prongParam.ProngTopY * num2, 0.0);
		Transform transform2 = Transform.Translation(0.0, prongTopOffset, 0.0);
		Transform transform3 = CSBlockUtil.CSUtilMath.Orient3Point(Point3d.Origin, Vector3d.XAxis, Vector3d.ZAxis, point3d, -Vector3d.YAxis, Vector3d.ZAxis);
		Curve curve3 = curve.DuplicateCurve();
		curve3.Transform(transform3 * transform2 * transform);
		Transform xform3 = CSBlockUtil.CSUtilMath.Rotation(Vector3d.ZAxis * -1.0, prongRail.TangentAtStart, point3d);
		if (!planarDome)
		{
			curve3.Transform(xform3);
		}
		Curve domeCurve = null;
		if (!(prongParam.ProngDomeHeight <= 0.0))
		{
			if (!ProngDomeCurve(curve, prongParam.ProngTopX, prongParam.ProngTopY, num, prongParam.ProngDomeHeight, prongParam.ProngDomeSettingDistance, prongParam.ProngDomeSettingAngle, num2, planarDome, prongParam.DomeBlendAmountInside, prongParam.DomeBlendAmountOutside, prongParam.DomeBlendStartInside, prongParam.DomeBlendStartOutside, prongParam.DomeBlendEndInside, prongParam.DomeBlendEndOutside, prongRail, ref domeCurve))
			{
				error = "Error Creating Prong Dome Curve";
				return false;
			}
			domeCurve.Transform(transform3 * transform2);
		}
		double z = prongRail.PointAtEnd.Z + prongParam.ProngBaseDomeHeight;
		double num3 = CSBlockUtil.CSUtilMath.AngleBetweenVectorsWithDirection(new Vector3d(0.0, 0.0, -1.0), prongRail.TangentAtEnd, -Vector3d.YAxis);
		double length = ((num3 < 0.0) ? (Math.Tan(0.0 - num3) * prongParam.ProngBaseY + 0.05) : 0.05);
		if (baseStyle != ValueListItem.HeadBuilderProngHead)
		{
			length = ((num3 < 0.0) ? (Math.Tan(0.0 - num3) * prongParam.ProngBaseY + 0.05) : (Math.Tan(num3) * prongParam.ProngBaseY + 0.05));
		}
		prongRail = prongRail.Extend(CurveEnd.End, length, CurveExtensionStyle.Line);
		Point3d pointAtEnd = prongRail.PointAtEnd;
		Transform transform4 = Transform.Scale(Plane.WorldXY, prongParam.ProngBaseX, prongParam.ProngBaseY, 0.0);
		Transform transform5 = Transform.Translation(0.0, prongBaseOffset, 0.0);
		Vector3d tangentAtEnd = prongRail.TangentAtEnd;
		Transform transform6 = CSBlockUtil.CSUtilMath.Orient3Point(Point3d.Origin, Vector3d.XAxis, Vector3d.ZAxis, pointAtEnd, -Vector3d.YAxis, -tangentAtEnd);
		Curve curve4 = curve2.DuplicateCurve();
		curve4.Transform(transform6 * transform5 * transform4);
		Curve[] profiles = new Curve[2] { curve3, curve4 };
		double[] parameters = new double[2]
		{
			prongRail.Domain.Min,
			prongRail.Domain.Max
		};
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
		Brep splitter = NurbsSurface.CreateFromCorners(new Point3d(20.0, 20.0, z), new Point3d(-20.0, 20.0, z), new Point3d(-20.0, -20.0, z), new Point3d(20.0, -20.0, z)).ToBrep();
		Brep[] array = breps[0].Split(splitter, 0.01);
		if (array.Length != 2)
		{
			error = "Prong split failed";
			return false;
		}
		BoundingBox boundingBox = array[0].GetBoundingBox(accurate: true);
		BoundingBox boundingBox2 = array[1].GetBoundingBox(accurate: true);
		Brep brep = ((boundingBox.Center.Z > boundingBox2.Center.Z) ? array[0] : array[1]);
		Point3d pointAtStart2 = prongRail.PointAtStart;
		Point3d pointAtEnd2 = prongRail.PointAtEnd;
		NurbsSurface surface = NurbsSurface.CreateFromCorners(pointAtEnd2 + new Point3d(-4.0, 0.0, -3.0), pointAtEnd2 + new Point3d(8.0, 0.0, -3.0), pointAtStart2 + new Point3d(8.0, 0.0, 3.0), pointAtStart2 + new Point3d(-4.0, 0.0, 3.0));
		Intersection.BrepSurface(brep, surface, CSApplicationHelper.Application.Tolerance, out var intersectionCurves, out var _);
		List<Curve> list = new List<Curve>();
		Curve curve5 = ((intersectionCurves[0].PointAtStart.X < intersectionCurves[1].PointAtStart.X) ? intersectionCurves[0] : intersectionCurves[1]);
		Curve curve6 = ((intersectionCurves[0].PointAtStart.X < intersectionCurves[1].PointAtStart.X) ? intersectionCurves[1] : intersectionCurves[0]);
		if (curve5.PointAtStart.Z < curve5.PointAtEnd.Z)
		{
			curve5.Reverse();
		}
		if (curve6.PointAtStart.Z < curve6.PointAtEnd.Z)
		{
			curve6.Reverse();
		}
		Curve domeCurve2;
		if (prongParam.ProngBaseDomeHeight > 0.0)
		{
			double angle = CSBlockUtil.CSUtilMath.AngleBetweenVectorsWithDirection(new Vector3d(0.0, 0.0, -1.0), curve5.TangentAtEnd, -Vector3d.YAxis);
			if (!ProngBaseDomeCurve((curve5.PointAtEnd - curve6.PointAtEnd).Length, 0.0 - prongParam.ProngBaseDomeHeight, angle, out domeCurve2))
			{
				error = "Failed prong base";
				return false;
			}
		}
		else
		{
			domeCurve2 = new LineCurve(curve5.PointAtEnd, curve6.PointAtEnd);
		}
		Transform xform4 = CSBlockUtil.CSUtilMath.Orient3Point(Point3d.Origin, Vector3d.XAxis, Vector3d.ZAxis, curve5.PointAtEnd, -Vector3d.YAxis, Vector3d.ZAxis);
		domeCurve2.Transform(xform4);
		if (bezelSeatHeight > 0.0 && bezelSeatLength > 0.0)
		{
			Point3d item = point3d;
			item.Z -= bezelSeatHeight;
			double num4 = RhinoMath.ToRadians(bezelSeatAngle + 180.0);
			double x = item.X + bezelSeatLength * Math.Cos(num4);
			double num5 = item.Z + bezelSeatLength * Math.Sin(num4);
			Point3d point3d2 = new Point3d(x, item.Y, num5);
			PolylineCurve polylineCurve = new PolylineCurve(new List<Point3d> { point3d, item, point3d2 });
			if (bezelBlendPercentage > 0.99)
			{
				Vector3d vectorStart = curve5.PointAtEnd - point3d2;
				vectorStart.Unitize();
				Curve curve7 = CSUtilCurves.BlendCurve(point3d2, curve5.PointAtEnd, vectorStart, curve5.TangentAtEnd * -1.0);
				curve5 = Curve.JoinCurves(new Curve[2] { polylineCurve, curve7 })[0];
			}
			else
			{
				double z2 = curve5.PointAtEnd.Z - (curve5.PointAtEnd.Z - num5) * bezelBlendPercentage;
				PolylineCurve curveB = new PolylineCurve(new List<Point3d>
				{
					new Point3d(0.0, 0.0, z2),
					new Point3d(50.0, 0.0, z2)
				});
				CurveIntersections curveIntersections = Intersection.CurveCurve(curve5, curveB, CSApplicationHelper.Application.Tolerance, CSApplicationHelper.Application.Tolerance);
				if (curveIntersections.Count != 1)
				{
					error = "Bezel seat Intersection Error";
					return false;
				}
				curve5 = curve5.Trim(curveIntersections[0].ParameterA, curve5.Domain.Max);
				Vector3d vectorStart2 = curve5.PointAtStart - point3d2;
				vectorStart2.Unitize();
				Curve curve8 = CSUtilCurves.BlendCurve(point3d2, curve5.PointAtStart, vectorStart2, curve5.TangentAtStart * -1.0);
				curve8 = curve8.Rebuild(5, 3, preserveTangents: false);
				curve5 = Curve.JoinCurves(new Curve[3] { polylineCurve, curve8, curve5 })[0];
			}
		}
		list.Add(curve5);
		list.Add(curve6);
		if (domeCurve == null)
		{
			error = "Missing prongTopDomeCrv";
			return false;
		}
		list.Add(domeCurve.ToNurbsCurve());
		list.Add(domeCurve2);
		NurbsCurve nurbsCurve = Curve.JoinCurves(new List<NurbsCurve>
		{
			list[0].ToNurbsCurve(),
			list[1].Rebuild(5, 3, preserveTangents: false),
			list[2].Rebuild(8, 3, preserveTangents: false),
			list[3].Rebuild(8, 3, preserveTangents: false)
		}, CSApplicationHelper.Application.Tolerance)[0].ToNurbsCurve();
		nurbsCurve.MakeDeformable();
		nurbsCurve.MakeClosed(0.01);
		bezelProngOutline = nurbsCurve;
		bezelProngOutline.Transform(xform2);
		error = "";
		return true;
	}

	public static bool RevolveBezel(Curve bezelProngOutline1, CSGemData copyGem, Curve headBase, double cornerBlend, out NurbsSurface bezelSurface, out string error)
	{
		if (!GemOffsetCurve.GemOffset(copyGem.GemShape, copyGem, 0.0, out Curve curve, false, out error, cornerBlend, 0.0, 0.0))
		{
			bezelSurface = null;
			error = "CurveTop Inside:" + error;
			return false;
		}
		NurbsCurve nurbsCurve = bezelProngOutline1.ToNurbsCurve();
		NurbsCurve nurbsCurve2 = curve.ToNurbsCurve();
		Point3d rotationCenter = copyGem.GetCurve().PointAt(copyGem.BezelParam);
		if (nurbsCurve.TryGetPlane(out var plane, 0.1))
		{
			Transform xform = Transform.Rotation(plane.Normal, Vector3d.YAxis, rotationCenter);
			nurbsCurve.Transform(xform);
		}
		bezelSurface = NurbsSurface.Create(3, isRational: true, nurbsCurve.Order, nurbsCurve2.Order, nurbsCurve.Points.Count, nurbsCurve2.Points.Count);
		BoundingBox boundingBox = headBase.GetBoundingBox(accurate: true);
		double z = boundingBox.Center.Z;
		double gemXSize = copyGem.GemXSize;
		double gemYSize = copyGem.GemYSize;
		double x = boundingBox.Diagonal.X;
		double y = boundingBox.Diagonal.Y;
		double num = gemXSize - x;
		double num2 = gemYSize - y;
		for (int i = 0; i < nurbsCurve.Points.Count; i++)
		{
			ControlPoint controlPoint = nurbsCurve.Points[i];
			double num3 = controlPoint.Location.Z / z;
			double num4 = (gemXSize - num * num3) / gemXSize;
			double num5 = (gemYSize - num2 * num3) / gemYSize;
			Transform transform = Transform.Scale(Plane.WorldXY, num4, num5, 1.0);
			Transform transform2 = Transform.Scale(Plane.WorldXY, 1.0 / num4, 1.0 / num5, 1.0);
			copyGem.ApplyTransform(transform);
			double distance = controlPoint.Location.X - rotationCenter.X + num / 2.0 * num3;
			if (!GemOffsetCurve.GemOffset(copyGem.GemShape, copyGem, distance, out curve, false, out error, cornerBlend, 0.0, 0.0))
			{
				error = "CurveTop Inside:" + error;
				return false;
			}
			copyGem.ApplyTransform(transform2);
			NurbsCurve nurbsCurve3 = curve.ToNurbsCurve();
			nurbsCurve3.Translate(Vector3d.ZAxis * controlPoint.Location.Z);
			for (int j = 0; j < nurbsCurve3.Points.Count; j++)
			{
				ControlPoint controlPoint2 = nurbsCurve3.Points[j];
				double weight = nurbsCurve.Points[i].Weight * nurbsCurve3.Points[j].Weight;
				bezelSurface.Points.SetControlPoint(i, j, new ControlPoint(controlPoint2.Location.X, controlPoint2.Location.Y, controlPoint2.Location.Z, weight));
			}
		}
		for (int k = 0; k < nurbsCurve.Knots.Count; k++)
		{
			bezelSurface.KnotsU[k] = nurbsCurve.Knots[k];
		}
		for (int l = 0; l < nurbsCurve2.Knots.Count; l++)
		{
			bezelSurface.KnotsV[l] = nurbsCurve2.Knots[l];
		}
		error = "";
		return true;
	}

	private static bool ProngBaseDomeCurve(double prongYSize, double radius, double angle, out Curve domeCurve)
	{
		Curve curve = CSUtilGeometry.CreateDomeRC(radius, prongYSize / 2.0);
		curve.Transform(Transform.Rotation(Math.PI / 2.0, Vector3d.ZAxis, Point3d.Origin));
		Curve curve2 = curve.DuplicateCurve();
		curve2.Transform(Transform.Rotation(Math.PI, Vector3d.ZAxis, Point3d.Origin));
		Curve[] array = Curve.JoinCurves(new Curve[2] { curve, curve2 }, CSApplicationHelper.Application.Tolerance);
		if (array.Length != 1)
		{
			domeCurve = null;
			return false;
		}
		NurbsCurve nurbsCurve = array[0].ToNurbsCurve();
		double num = Math.Tan(angle);
		for (int i = 0; i < nurbsCurve.Points.Count; i++)
		{
			ControlPoint value = nurbsCurve.Points[i];
			value.Location = new Point3d(value.Location.X, value.Location.Y + num * value.Location.Z, value.Location.Z);
			nurbsCurve.Points[i] = value;
		}
		nurbsCurve.Translate(new Vector3d(0.0, prongYSize / 2.0, 0.0));
		domeCurve = nurbsCurve;
		return true;
	}

	private static bool ProngDomeCurve(Curve prongProfile, double prongX, double prongY, double angle, double radius, double rotateX, double prongSettingAngle, double fixScale, bool planarDome, double blendAmountInside, double blendAmountOutside, double blendStartInside, double blendStartOutside, double blendEndInside, double blendEndOutside, Curve prongRail, ref Curve domeCurve)
	{
		BoundingBox boundingBox = prongProfile.GetBoundingBox(accurate: true);
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
		Transform transform = Transform.Scale(Plane.WorldXY, prongX / boundingBox.Diagonal.X, prongY / boundingBox.Diagonal.Y * fixScale, 0.0);
		if (!planarDome && flag)
		{
			transform = Transform.Scale(Plane.WorldXY, prongX / boundingBox.Diagonal.X, prongY / boundingBox.Diagonal.Y, 0.0);
		}
		Transform transform2 = Transform.Translation(0.0, (0.0 - prongY) / 2.0, 0.0);
		Curve curve = prongProfile.DuplicateCurve();
		curve.Transform(transform2 * transform);
		Curve curve2 = prongRail.DuplicateCurve();
		if (flag && !planarDome)
		{
			Vector3d source = new Vector3d(0.0 - prongRail.PointAtStart.X, 0.0 - prongRail.PointAtStart.Y, 0.0);
			curve2.Transform(CSBlockUtil.CSUtilMath.Orient3Point(prongRail.PointAtStart, Vector3d.ZAxis, source, Point3d.Origin, Vector3d.ZAxis, -Vector3d.YAxis));
			Transform xform = CSBlockUtil.CSUtilMath.Rotation(Vector3d.ZAxis * -1.0, curve2.TangentAtStart, curve.PointAtStart);
			curve.Transform(xform);
		}
		LineCurve lineCurve = new LineCurve(new Point3d(0.0, -50.0, 0.0), new Point3d(0.0, 50.0, 0.0));
		NurbsSurface surface = NurbsSurface.CreateFromCorners(new Point3d(0.0, -20.0, -10.0), new Point3d(0.0, 20.0, -10.0), new Point3d(0.0, 20.0, 10.0), new Point3d(0.0, -20.0, 10.0));
		CurveIntersections curveIntersections = Intersection.CurveSurface(curve, surface, 0.01, 0.01);
		if (curveIntersections.Count <= 1)
		{
			return false;
		}
		double parameterA = curveIntersections[0].ParameterA;
		double t = curveIntersections[1].ParameterA;
		if (Math.Abs(parameterA - curve.Domain.Min) > 0.0001 && Math.Abs(parameterA - curve.Domain.Max) > 0.0001)
		{
			Point3d testPoint = curve.PointAt(t);
			curve.ChangeClosedCurveSeam(parameterA);
			curve.ClosestPoint(testPoint, out t);
		}
		Curve[] array = curve.Split(t);
		Curve curve3 = array[0];
		Curve curve4 = array[1];
		if (curve3 == null || curve4 == null)
		{
			return false;
		}
		Vector3d vector3d = lineCurve.TangentAt(lineCurve.Domain.Mid);
		vector3d.Unitize();
		Vector3d vector3d2 = curve3.TangentAt(curve3.Domain.Mid);
		vector3d2.Unitize();
		if (vector3d * vector3d2 < 0.0)
		{
			curve3.Reverse();
		}
		Vector3d vector3d3 = curve4.TangentAt(curve4.Domain.Mid);
		vector3d3.Unitize();
		if (vector3d * vector3d3 < 0.0)
		{
			curve4.Reverse();
		}
		Vector3d vector3d4 = Vector3d.ZAxis;
		if (!flag)
		{
			vector3d4.Z = 0.0 - vector3d4.Z;
			radius = 0.0 - radius;
		}
		if (planarDome || !flag)
		{
			vector3d4.Rotate(0.0 - angle, Vector3d.XAxis);
		}
		vector3d4.Unitize();
		Point3d center = boundingBox.Center;
		center.Transform(transform2 * transform);
		Point3d rotationCenter = new Point3d(center);
		if (flag)
		{
			if (!planarDome)
			{
				center += vector3d4 * (radius / Math.Cos(angle));
			}
			else
			{
				center += Vector3d.ZAxis * radius;
			}
			center.Transform(CSBlockUtil.CSUtilMath.Rotation(CSBlockUtil.CSUtilMath.ToRadians(rotateX), Vector3d.XAxis, rotationCenter));
		}
		else
		{
			center += vector3d4 * (radius / Math.Cos(angle));
			center.Transform(CSBlockUtil.CSUtilMath.Rotation(CSBlockUtil.CSUtilMath.ToRadians(rotateX), Vector3d.XAxis, rotationCenter));
		}
		Vector3d vector3d5 = Vector3d.YAxis * -1.0;
		vector3d5.Rotate(CSBlockUtil.CSUtilMath.ToRadians(prongSettingAngle) + CSBlockUtil.CSUtilMath.ToRadians(rotateX), Vector3d.XAxis);
		vector3d5.Unitize();
		Vector3d vectorEnd = new Vector3d(vector3d5);
		vectorEnd.Reverse();
		if (!planarDome && flag)
		{
			vector3d4 = -curve2.TangentAtStart;
		}
		Curve curve5 = CSUtilCurves.BlendCurve(curve3.PointAtStart, center, vector3d4, vector3d5, blendAmountInside, blendStartInside, blendEndInside);
		Curve curve6 = CSUtilCurves.BlendCurve(curve3.PointAtEnd, center, vector3d4, vectorEnd, blendAmountOutside, blendStartOutside, blendEndOutside);
		Curve[] array2 = Curve.JoinCurves(new Curve[2] { curve5, curve6 });
		if (array2 == null)
		{
			return false;
		}
		if (array2.Length == 0)
		{
			return false;
		}
		domeCurve = array2[0];
		Transform xform2 = Transform.Translation(0.0, prongY / 2.0, 0.0);
		domeCurve.Transform(xform2);
		return true;
	}
}
