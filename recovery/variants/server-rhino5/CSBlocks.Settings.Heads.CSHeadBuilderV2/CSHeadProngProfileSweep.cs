using System;
using System.Collections.Generic;
using System.Linq;
using CS;
using CSBlockUtil;
using CSBlocks.Settings.Heads.CSHeadBuilderV1.Obsolete;
using GV.CounterSketch.Shared;
using Rhino.Geometry;
using Rhino.Geometry.Intersect;

namespace CSBlocks.Settings.Heads.CSHeadBuilderV2;

internal static class CSHeadProngProfileSweep
{
	public static bool ProngProfileSweep(int prongIndex, ProngParam[] prongParams, Curve headTop, double paramTop, double prongTopOffset, Curve headBase, double paramBase, double prongBaseOffset, ValueListItem baseStyle, double basePointSize, Transform xUp, bool enableDoItYourself, bool planarDome, Point3d focalPoint, out List<Brep> topDome, out List<Brep> middleProng, out List<Brep> lowerDome, out List<Curve> outCurves, out List<Brep> outProngs, out string error)
	{
		ProngParam prongParam = prongParams[prongIndex];
		outCurves = new List<Curve>();
		outProngs = new List<Brep>();
		topDome = new List<Brep>();
		middleProng = new List<Brep>();
		lowerDome = new List<Brep>();
		Curve prongRail = prongParam.ProngRail;
		Point3d pointAtStart = prongRail.PointAtStart;
		Vector3d vector3d = CSUtilMath.AverageTangentAt(headTop, paramTop);
		vector3d.Unitize();
		Vector3d headTopNrm = CSUtilMath.AverageNormalAt(Vector3d.ZAxis, vector3d);
		headTopNrm.Unitize();
		BoundingBox boundingBox = prongParam.ProngTopProfile.GetBoundingBox(accurate: true);
		double x = boundingBox.Diagonal.X;
		double y = boundingBox.Diagonal.Y;
		double num = CSUtilMath.AngleBetweenVectorsWithDirection(new Vector3d(0.0, 0.0, -1.0), prongRail.TangentAt(prongRail.Domain.Min), vector3d);
		double num2 = 1.0 / Math.Cos(num);
		Transform transform = Transform.Scale(Plane.WorldXY, prongParam.ProngTopX / x, prongParam.ProngTopY / y * num2, 0.0);
		if (!planarDome)
		{
			transform = Transform.Scale(Plane.WorldXY, prongParam.ProngTopX / x, prongParam.ProngTopY / y, 0.0);
		}
		Transform transform2 = Transform.Translation(0.0, prongTopOffset, 0.0);
		Transform transform3 = CSUtilMath.Orient3Point(Point3d.Origin, Vector3d.XAxis, Vector3d.ZAxis, pointAtStart, vector3d, Vector3d.ZAxis);
		Curve prongTopProfile = prongParam.ProngTopProfile.DuplicateCurve();
		prongTopProfile.Transform(transform3 * transform2 * transform);
		Vector3d vector3d2 = new Vector3d(0.0, 1.0, 0.0);
		vector3d2.Transform(transform3);
		vector3d2.Unitize();
		Point3d rotationCenter = pointAtStart + vector3d2 * prongTopOffset;
		Transform xform = CSUtilMath.Rotation(Vector3d.ZAxis * -1.0, prongRail.TangentAtStart, rotationCenter);
		transform3.TryGetInverse(out var inverseTransform);
		if (!planarDome)
		{
			prongTopProfile.Transform(xform);
		}
		Vector3d vector3d3;
		if (baseStyle != ValueListItem.HeadBuilderProngHead)
		{
			vector3d3 = vector3d;
		}
		else
		{
			vector3d3 = CSUtilMath.AverageTangentAt(headBase, paramBase);
			vector3d3.Unitize();
		}
		BoundingBox boundingBox2 = prongParam.ProngBaseProfile.GetBoundingBox(accurate: true);
		double x2 = boundingBox2.Diagonal.X;
		double y2 = boundingBox2.Diagonal.Y;
		Transform transform4 = Transform.Scale(Plane.WorldXY, prongParam.ProngBaseX / x2, prongParam.ProngBaseY / y2, 0.0);
		if (baseStyle == ValueListItem.HeadBuilderProngHeadCutToPoint)
		{
			prongBaseOffset = 0.0 - prongParam.ProngBaseY + basePointSize;
		}
		Transform transform5 = Transform.Translation(0.0, prongBaseOffset, 0.0);
		Vector3d tangentAtEnd = prongRail.TangentAtEnd;
		Brep dome = null;
		Brep dome2 = null;
		if (prongParam.ProngDomeHeight <= 0.0)
		{
			if (!ProngExtension(0.0, ref prongRail, ref prongTopProfile))
			{
				error = "Unable to create top prong cap.";
				return false;
			}
			if (!planarDome)
			{
				prongTopProfile.Transform(xform);
			}
		}
		else
		{
			if (!ProngDome(prongParam, num, num2, planarDome, prongRail, inverseTransform, ref dome))
			{
				error = "Unable to create top dome";
				return false;
			}
			dome.Transform(transform3 * transform2);
		}
		double z = prongRail.PointAtEnd.Z + prongParam.ProngBaseDomeHeight;
		double num3 = CSUtilMath.AngleBetweenVectorsWithDirection(new Vector3d(0.0, 0.0, -1.0), prongRail.TangentAtEnd, (baseStyle != ValueListItem.HeadBuilderProngHead) ? vector3d : vector3d3);
		double length = ((num3 < 0.0) ? (Math.Tan(0.0 - num3) * prongParam.ProngBaseY + 0.05) : 0.05);
		if (baseStyle != ValueListItem.HeadBuilderProngHead)
		{
			length = ((num3 < 0.0) ? (Math.Tan(0.0 - num3) * prongParam.ProngBaseY + 0.05) : (Math.Tan(num3) * prongParam.ProngBaseY + 0.05));
		}
		prongRail = prongRail.Extend(CurveEnd.End, length, CurveExtensionStyle.Line);
		Point3d pointAtEnd = prongRail.PointAtEnd;
		Transform transform6 = CSUtilMath.Orient3Point(Point3d.Origin, Vector3d.XAxis, Vector3d.ZAxis, pointAtEnd, (baseStyle != ValueListItem.HeadBuilderProngHead) ? vector3d : vector3d3, -tangentAtEnd);
		Curve curve = prongParam.ProngBaseProfile.DuplicateCurve();
		curve.Transform(transform6 * transform5 * transform4);
		Curve[] profiles = new Curve[2] { prongTopProfile, curve };
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
		BoundingBox boundingBox3 = array[0].GetBoundingBox(accurate: true);
		BoundingBox boundingBox4 = array[1].GetBoundingBox(accurate: true);
		Brep brep = ((boundingBox3.Center.Z > boundingBox4.Center.Z) ? array[0] : array[1]);
		if (prongParam.ProngBaseDomeHeight > 0.0)
		{
			List<Curve> list = new List<Curve>();
			List<Curve> list2 = new List<Curve>();
			foreach (BrepEdge edge in brep.Edges)
			{
				if (edge.Valence == EdgeAdjacency.Naked)
				{
					list.Add(edge.DuplicateCurve());
				}
				else
				{
					list2.Add(edge.DuplicateCurve());
				}
			}
			Curve[] array2 = Curve.JoinCurves(list);
			if (array2.Length != 2)
			{
				error = "Prong edge curve extract failed";
				return false;
			}
			BoundingBox boundingBox5 = array2[0].GetBoundingBox(accurate: true);
			BoundingBox boundingBox6 = array2[1].GetBoundingBox(accurate: true);
			Curve obj = ((boundingBox5.Center.Z < boundingBox6.Center.Z) ? array2[0] : array2[1]);
			Transform xform2 = CSUtilMath.Orient3Point(list2[0].PointAtEnd, (baseStyle != ValueListItem.HeadBuilderProngHead) ? vector3d : vector3d3, Vector3d.ZAxis, Point3d.Origin, Vector3d.XAxis, Vector3d.ZAxis);
			obj.Transform(xform2);
			curve = obj;
			double angle = CSUtilMath.AngleBetweenVectorsWithDirection(new Vector3d(0.0, 0.0, -1.0), list2[0].TangentAtEnd, (baseStyle != ValueListItem.HeadBuilderProngHead) ? vector3d : vector3d3);
			if (!ProngBaseDome(curve, 0.0 - prongParam.ProngBaseDomeHeight, angle, ref dome2))
			{
				error = "Failed prong base";
				return false;
			}
			Transform xform3 = CSUtilMath.Orient3Point(Point3d.Origin, Vector3d.XAxis, Vector3d.ZAxis, list2[0].PointAtEnd, (baseStyle != ValueListItem.HeadBuilderProngHead) ? vector3d : vector3d3, Vector3d.ZAxis);
			curve.Transform(xform3);
			dome2.Transform(xform3);
		}
		GeometryBase[] arrGeometry = new GeometryBase[3] { prongRail, prongTopProfile, curve };
		List<GeometryBase> outputGeometries = new List<GeometryBase>();
		if (!ArrangeGeometry(prongParam.ProngArrangement, xUp, vector3d, headTopNrm, prongParam.ProngTopX, prongParam.ProngOverlap, arrGeometry, null, ref outputGeometries))
		{
			error = "Invalid head style";
			return false;
		}
		foreach (Curve item14 in outputGeometries)
		{
			outCurves.Add(item14);
		}
		if (enableDoItYourself)
		{
			GeometryBase[] arrGeometry2;
			if (dome2 != null)
			{
				arrGeometry2 = new GeometryBase[2] { dome, dome2 };
				List<GeometryBase> outputGeometries2 = new List<GeometryBase>();
				GeometryBase[] arrGeometry3 = new GeometryBase[1] { dome2 };
				if (!ArrangeGeometry(prongParam.ProngArrangement, xUp, vector3d, headTopNrm, prongParam.ProngTopX, prongParam.ProngOverlap, arrGeometry3, null, ref outputGeometries2))
				{
					error = "Invalid head style";
					return false;
				}
				foreach (Brep item15 in outputGeometries2)
				{
					lowerDome.Add(item15);
				}
			}
			else
			{
				arrGeometry2 = new GeometryBase[1] { dome };
			}
			List<GeometryBase> outputGeometries3 = new List<GeometryBase>();
			if (!ArrangeGeometry(prongParam.ProngArrangement, xUp, vector3d, headTopNrm, prongParam.ProngTopX, prongParam.ProngOverlap, arrGeometry2, null, ref outputGeometries3))
			{
				error = "Invalid head style";
				return false;
			}
			foreach (Brep item16 in outputGeometries3)
			{
				outProngs.Add(item16);
			}
			GeometryBase[] arrGeometry4 = new GeometryBase[1] { dome };
			List<GeometryBase> outputGeometries4 = new List<GeometryBase>();
			if (!ArrangeGeometry(prongParam.ProngArrangement, xUp, vector3d, headTopNrm, prongParam.ProngTopX, prongParam.ProngOverlap, arrGeometry4, null, ref outputGeometries4))
			{
				error = "Invalid head style";
				return false;
			}
			foreach (Brep item17 in outputGeometries4)
			{
				topDome.Add(item17);
			}
		}
		else
		{
			List<Brep> list3 = new List<Brep>();
			if (dome != null)
			{
				list3.Add(dome);
			}
			if (dome2 != null)
			{
				list3.Add(dome2);
			}
			if (brep != null)
			{
				list3.Add(brep);
			}
			Brep brep2 = CSUtilRhino.JoinAllBreps(list3.ToArray(), 0.01);
			if (brep2 == null)
			{
				error = "Failed JoinBreps";
				return false;
			}
			if (dome2 == null)
			{
				Brep brep3 = CSUtilRhino.CapPlanarHoles(brep2, 0.01);
				if (brep3 == null)
				{
					error = "Failed CapPlanarHoles";
					return false;
				}
				brep2 = brep3;
			}
			Brep brep4 = null;
			if (baseStyle == ValueListItem.HeadBuilderProngHeadCutToPoint)
			{
				List<Point3d> points;
				if (prongParams.Length > 2)
				{
					Curve prongRail2 = prongParams[(prongIndex + 1) % prongParams.Length].ProngRail;
					if (prongRail2 == null)
					{
						error = "Failed to get prongRailCrvP";
						return false;
					}
					Curve curve2 = prongRail2.DuplicateCurve();
					Curve prongRail3 = prongParams[(prongIndex - 1 + prongParams.Length) % prongParams.Length].ProngRail;
					if (prongRail3 == null)
					{
						error = "Failed to get prongRailCrvM";
						return false;
					}
					Curve curve3 = prongRail3.DuplicateCurve();
					Vector3d vector3d4 = new Vector3d(curve2.PointAtStart);
					Vector3d vector3d5 = new Vector3d(prongRail.PointAtStart);
					Vector3d vector3d6 = new Vector3d(curve3.PointAtStart);
					vector3d4.Z = 0.0;
					vector3d5.Z = 0.0;
					vector3d6.Z = 0.0;
					vector3d4.Unitize();
					vector3d5.Unitize();
					vector3d6.Unitize();
					vector3d4 *= 20.0;
					vector3d5 *= 20.0;
					vector3d6 *= 20.0;
					Point3d item5 = new Point3d(vector3d4 + vector3d5);
					Point3d item6 = new Point3d(0.0, 0.0, 0.0);
					Point3d item7 = new Point3d(vector3d6 + vector3d5);
					Point3d item8 = new Point3d(vector3d5 * -2.0);
					item5.Z = 20.0;
					item6.Z = 20.0;
					item7.Z = 20.0;
					item8.Z = 20.0;
					points = new List<Point3d> { item5, item6, item7, item8, item5 };
				}
				else
				{
					Vector3d vector3d7 = new Vector3d(prongRail.PointAtStart);
					vector3d7.Z = 0.0;
					Vector3d vector3d8 = vector3d7;
					vector3d8.Unitize();
					vector3d8 *= 20.0;
					Vector3d vector3d9 = new Vector3d(vector3d8);
					vector3d9.Transform(Transform.Rotation(CSUtilMath.ToRadians(90.0), Point3d.Origin));
					Point3d item9 = new Point3d(vector3d9);
					Point3d item10 = new Point3d(0.0, 0.0, 0.0);
					Point3d item11 = new Point3d(-vector3d9);
					Point3d item12 = new Point3d(vector3d8 * -2.0);
					item9.Z = 20.0;
					item10.Z = 20.0;
					item11.Z = 20.0;
					item12.Z = 20.0;
					points = new List<Point3d> { item9, item10, item11, item12, item9 };
				}
				PolylineCurve curveA = new PolylineCurve(points);
				PolylineCurve polylineCurve = new PolylineCurve(points);
				Transform xform4 = Transform.Translation(0.0, 0.0, -40.0);
				polylineCurve.Transform(xform4);
				brep4 = CSUtilRhino.CapPlanarHoles(NurbsSurface.CreateRuledSurface(curveA, polylineCurve).ToBrep(), 0.001);
				CSUtilGeometry.FixBrepSolidOrientation(ref brep2, doFullClean: true);
				CSUtilGeometry.FixBrepSolidOrientation(ref brep4, doFullClean: true);
			}
			List<GeometryBase> outputGeometries5 = new List<GeometryBase>();
			if (!ArrangeGeometry(prongParam.ProngArrangement, xUp, vector3d, headTopNrm, prongParam.ProngTopX, prongParam.ProngOverlap, new GeometryBase[1] { brep2 }, brep4, ref outputGeometries5))
			{
				error = "Invalid head style";
				return false;
			}
			foreach (Brep item18 in outputGeometries5)
			{
				outProngs.Add(item18);
			}
			Brep brep5 = dome?.DuplicateBrep();
			Brep brep6 = dome2?.DuplicateBrep();
			Brep brep7 = brep?.DuplicateBrep();
			brep5?.Transform(xUp);
			brep6?.Transform(xUp);
			brep7?.Transform(xUp);
			prongParam.TopDome.Add(brep5);
			prongParam.LowerDome.Add(brep6);
			prongParam.MiddleProng.Add(brep7);
		}
		error = "";
		return true;
	}

	private static bool ArrangeGeometry(int prongArrangement, Transform xUp, Vector3d headTopTan, Vector3d headTopNrm, double prongTopX, double overlap, GeometryBase[] arrGeometry, Brep cutter, ref List<GeometryBase> outputGeometries)
	{
		switch ((CSHeadStyles5.EHeadProngArrangment)prongArrangement)
		{
		case CSHeadStyles5.EHeadProngArrangment.Single:
		{
			GeometryBase[] array = arrGeometry;
			for (int i = 0; i < array.Length; i++)
			{
				GeometryBase item4 = CutGeometry(array[i], cutter, xUp, xUp);
				outputGeometries.Add(item4);
			}
			break;
		}
		case CSHeadStyles5.EHeadProngArrangment.Double:
		{
			double num3 = overlap * prongTopX;
			Transform transform4 = Transform.Translation(headTopTan * 1.0 * num3);
			GeometryBase[] array = arrGeometry;
			for (int i = 0; i < array.Length; i++)
			{
				GeometryBase item5 = CutGeometry(array[i], cutter, xUp * transform4, xUp);
				outputGeometries.Add(item5);
			}
			Transform transform5 = Transform.Translation(headTopTan * -1.0 * num3);
			array = arrGeometry;
			for (int i = 0; i < array.Length; i++)
			{
				GeometryBase item6 = CutGeometry(array[i], cutter, xUp * transform5, xUp);
				outputGeometries.Add(item6);
			}
			break;
		}
		case CSHeadStyles5.EHeadProngArrangment.Triple:
		{
			double num = overlap * prongTopX;
			Transform transform = Transform.Translation(headTopTan * 1.0 * num);
			GeometryBase[] array = arrGeometry;
			for (int i = 0; i < array.Length; i++)
			{
				GeometryBase item = CutGeometry(array[i], cutter, xUp * transform, xUp);
				outputGeometries.Add(item);
			}
			Transform transform2 = Transform.Translation(headTopTan * -1.0 * num);
			array = arrGeometry;
			for (int i = 0; i < array.Length; i++)
			{
				GeometryBase item2 = CutGeometry(array[i], cutter, xUp * transform2, xUp);
				outputGeometries.Add(item2);
			}
			double num2 = overlap * prongTopX * 2.0 * Math.Sqrt(3.0) / 2.0;
			Transform transform3 = Transform.Translation(headTopNrm * num2);
			array = arrGeometry;
			for (int i = 0; i < array.Length; i++)
			{
				GeometryBase item3 = CutGeometry(array[i], cutter, xUp * transform3, xUp);
				outputGeometries.Add(item3);
			}
			break;
		}
		default:
			return false;
		}
		return true;
	}

	private static GeometryBase CutGeometry(GeometryBase g, Brep cut, Transform T, Transform xUp)
	{
		if (!(g is Brep brep) || cut == null)
		{
			GeometryBase geometryBase = g.Duplicate();
			geometryBase.Transform(T);
			return geometryBase;
		}
		Brep brep2 = brep.DuplicateBrep();
		brep2.Transform(T);
		Brep brep3 = cut.DuplicateBrep();
		brep3.Transform(xUp);
		Brep[] array = Brep.CreateBooleanDifference(brep2, brep3, 0.001);
		if (array == null)
		{
			return brep2.DuplicateBrep();
		}
		return array[0];
	}

	private static bool ProngBaseDome(Curve prongProfile, double radius, double angle, ref Brep dome)
	{
		BoundingBox boundingBox = prongProfile.GetBoundingBox(accurate: true);
		Curve curve = prongProfile.DuplicateCurve();
		curve.Translate(new Vector3d(-boundingBox.Center));
		NurbsSurface nurbsSurface = CSUtilRailRevolve.RailRevolveRC(CSUtilGeometry.CreateDomeRC(radius).ToNurbsCurve(), curve.ToNurbsCurve(), 0, zAdjust: true);
		if (nurbsSurface == null)
		{
			return false;
		}
		double num = Math.Tan(angle);
		for (int i = 0; i < nurbsSurface.Points.CountU; i++)
		{
			for (int j = 0; j < nurbsSurface.Points.CountV; j++)
			{
				ControlPoint controlPoint = nurbsSurface.Points.GetControlPoint(i, j);
				Point3d location = controlPoint.Location;
				location.Y += num * location.Z;
				controlPoint.Location = location;
				nurbsSurface.Points.SetControlPoint(i, j, controlPoint);
			}
		}
		nurbsSurface.Translate(new Vector3d(boundingBox.Center));
		dome = nurbsSurface.ToBrep();
		return true;
	}

	private static bool ProngDome(ProngParam prongParam, double angle, double fixScale, bool planarDome, Curve prongRail, Transform reverseOrient, ref Brep dome)
	{
		BoundingBox boundingBox = prongParam.ProngTopProfile.GetBoundingBox(accurate: true);
		bool flag = prongParam.ProngDomeHeight > 0.0;
		double blendAmount = prongParam.DomeBlendAmountInside;
		double blendAmount2 = prongParam.DomeBlendAmountOutside;
		double blendStart = prongParam.DomeBlendStartInside;
		double blendStart2 = prongParam.DomeBlendStartOutside;
		double blendEnd = prongParam.DomeBlendEndInside;
		double blendEnd2 = prongParam.DomeBlendEndOutside;
		if (!flag)
		{
			blendAmount = 1.0;
			blendAmount2 = 1.0;
			blendStart = 1.0;
			blendStart2 = 1.0;
			blendEnd = 1.0;
			blendEnd2 = 1.0;
		}
		Transform transform = Transform.Scale(Plane.WorldXY, prongParam.ProngTopX / boundingBox.Diagonal.X, prongParam.ProngTopY / boundingBox.Diagonal.Y * fixScale, 0.0);
		if (!planarDome && flag)
		{
			transform = Transform.Scale(Plane.WorldXY, prongParam.ProngTopX / boundingBox.Diagonal.X, prongParam.ProngTopY / boundingBox.Diagonal.Y, 0.0);
		}
		Transform transform2 = Transform.Translation(0.0, (0.0 - prongParam.ProngTopY) / 2.0, 0.0);
		Curve curve = prongParam.ProngTopProfile.DuplicateCurve();
		curve.Transform(transform2 * transform);
		Curve curve2 = prongRail.DuplicateCurve();
		if (flag && !planarDome)
		{
			curve2.Transform(reverseOrient);
			Transform xform = CSUtilMath.Rotation(Vector3d.ZAxis * -1.0, curve2.TangentAtStart, curve.PointAtStart);
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
		double num = prongParam.ProngDomeHeight;
		Vector3d vector3d4 = Vector3d.ZAxis;
		if (!flag)
		{
			vector3d4.Z = 0.0 - vector3d4.Z;
			num = 0.0 - num;
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
				center += vector3d4 * (num / Math.Cos(angle));
			}
			else
			{
				center += Vector3d.ZAxis * num;
			}
			center.Transform(CSUtilMath.Rotation(CSUtilMath.ToRadians(prongParam.ProngDomeSettingDistance), Vector3d.XAxis, rotationCenter));
		}
		else
		{
			center += vector3d4 * (num / Math.Cos(angle));
			center.Transform(CSUtilMath.Rotation(CSUtilMath.ToRadians(prongParam.ProngDomeSettingDistance), Vector3d.XAxis, rotationCenter));
		}
		Vector3d vector3d5 = Vector3d.YAxis * -1.0;
		vector3d5.Rotate(CSUtilMath.ToRadians(prongParam.ProngDomeSettingAngle) + CSUtilMath.ToRadians(prongParam.ProngDomeSettingDistance), Vector3d.XAxis);
		vector3d5.Unitize();
		Vector3d vectorEnd = new Vector3d(vector3d5);
		vectorEnd.Reverse();
		if (!planarDome && flag)
		{
			vector3d4 = -curve2.TangentAtStart;
		}
		Curve curve5 = CSUtilCurves.BlendCurve(curve3.PointAtStart, center, vector3d4, vector3d5, blendAmount, blendStart, blendEnd);
		Curve curve6 = CSUtilCurves.BlendCurve(curve3.PointAtEnd, center, vector3d4, vectorEnd, blendAmount2, blendStart2, blendEnd2);
		Curve[] array2 = Curve.JoinCurves(new Curve[2] { curve5, curve6 });
		if (array2 == null)
		{
			return false;
		}
		if (array2.Length == 0)
		{
			return false;
		}
		NurbsSurface surface2 = NurbsSurface.CreateFromCorners(new Point3d(-20.0, 0.0, -10.0), new Point3d(20.0, 0.0, -10.0), new Point3d(20.0, 0.0, 10.0), new Point3d(-20.0, 0.0, 10.0));
		CurveIntersections curveIntersections2 = Intersection.CurveSurface(curve3, surface2, 0.01, 0.01);
		if (curveIntersections2.Count <= 0)
		{
			return false;
		}
		double parameterA2 = curveIntersections2[0].ParameterA;
		Curve[] array3 = curve3.Split(parameterA2);
		Curve curve7 = array3[0];
		Curve curve8 = array3[1];
		if (curve7 == null || curve8 == null)
		{
			return false;
		}
		curve8.Reverse();
		CurveIntersections curveIntersections3 = Intersection.CurveSurface(curve4, surface2, 0.01, 0.01);
		if (!curveIntersections3.Any())
		{
			return false;
		}
		double parameterA3 = curveIntersections3[0].ParameterA;
		Curve[] array4 = curve4.Split(parameterA3);
		Curve curve9 = array4[0];
		Curve curve10 = array4[1];
		if (curve9 == null || curve10 == null)
		{
			return false;
		}
		curve10.Reverse();
		if (!CSUtilSweepRC.Sweep2(curve7, curve8, array2, new double[1] { curve7.Domain.Min }, new double[1] { curve8.Domain.Min }, out var breps, closed: false, maintainHeight: false))
		{
			return false;
		}
		if (!CSUtilSweepRC.Sweep2(curve9, curve10, array2, new double[1] { curve9.Domain.Min }, new double[1] { curve9.Domain.Min }, out var breps2, closed: false, maintainHeight: false))
		{
			return false;
		}
		Brep brep = CSUtilRhino.JoinAllBreps_BackwardCompatibility(new List<Brep> { breps, breps2 }.ToArray(), CSApplicationHelper.Application.Tolerance);
		if (brep == null)
		{
			return false;
		}
		Transform xform2 = Transform.Translation(0.0, prongParam.ProngTopY / 2.0, 0.0);
		brep.Transform(xform2);
		dome = brep;
		return true;
	}

	private static bool ProngExtension(double prongExtensionHeight, ref Curve prongRail, ref Curve prongTopProfile)
	{
		Vector3d vector3d = prongRail.TangentAt(prongRail.Domain.Min);
		vector3d.Unitize();
		Point3d pointAtStart = prongRail.PointAtStart;
		Point3d point3d = pointAtStart - vector3d * prongExtensionHeight;
		Transform xform = Transform.Translation(point3d - pointAtStart);
		prongTopProfile.Transform(xform);
		LineCurve lineCurve = new LineCurve(pointAtStart, point3d);
		Curve curve = Curve.JoinCurves(new Curve[2] { prongRail, lineCurve })[0];
		if ((curve.PointAtStart - point3d).Length > 0.01)
		{
			curve.Reverse();
		}
		prongRail = curve;
		return true;
	}
}
