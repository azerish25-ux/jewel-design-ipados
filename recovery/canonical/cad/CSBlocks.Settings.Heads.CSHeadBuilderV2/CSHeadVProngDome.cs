using System.Collections.Generic;
using CS;
using CSBlockUtil;
using Rhino.Geometry;
using Rhino.Geometry.Intersect;

namespace CSBlocks.Settings.Heads.CSHeadBuilderV2;

internal static class CSHeadVProngDome
{
	public static Brep VProngDome(int prongNumber, Brep vProng, double topZ, double baseZ, NurbsSurface intSrf0, NurbsSurface intSrf1, double radiusTop, double radiusBottom)
	{
		Transform.Translation((prongNumber + 1) * 20, 40.0, 0.0);
		if (!CutBrep(vProng, topZ, intSrf0, out var crvInner, out var crvOuter))
		{
			return null;
		}
		if (!CutBrep(vProng, baseZ, intSrf0, out var crvInner2, out var crvOuter2))
		{
			return null;
		}
		GetEdgeCurves(vProng, crvInner, intSrf0, intSrf1, out var edgeCurveStart, out var edgeCurveEnd);
		if (edgeCurveStart == null || edgeCurveEnd == null)
		{
			return null;
		}
		Point3d point3d;
		Point3d point3d2;
		Vector3d tangentAtStart;
		Vector3d tangentAtEnd;
		if (crvInner2 != null)
		{
			point3d = crvInner2.PointAtStart;
			point3d2 = crvInner2.PointAtEnd;
			tangentAtStart = crvInner2.TangentAtStart;
			tangentAtEnd = crvInner2.TangentAtEnd;
		}
		else
		{
			NurbsSurface surface = NurbsSurface.CreateFromCorners(new Point3d(40.0, 40.0, baseZ), new Point3d(40.0, -40.0, baseZ), new Point3d(-40.0, -40.0, baseZ), new Point3d(-40.0, 40.0, baseZ));
			CurveIntersections curveIntersections = Intersection.CurveSurface(edgeCurveStart, surface, CSApplicationHelper.Application.Tolerance, CSApplicationHelper.Application.Tolerance);
			double num = 9999.0;
			Point3d point3d3 = Point3d.Unset;
			foreach (IntersectionEvent item in curveIntersections)
			{
				double length = new Vector3d(item.PointA).Length;
				if (length < num)
				{
					num = length;
					point3d3 = item.PointA;
				}
			}
			point3d = point3d3;
			point3d2 = point3d3;
			tangentAtStart = crvOuter2.TangentAtStart;
			tangentAtEnd = crvOuter2.TangentAtEnd;
		}
		Curve curve = ProngCurve(crvInner.PointAtStart, crvOuter.PointAtStart, crvInner.TangentAtStart * -1.0, crvOuter.TangentAtStart * -1.0, radiusTop);
		Curve curve2 = ProngCurve(crvInner.PointAtEnd, crvOuter.PointAtEnd, crvInner.TangentAtEnd, crvOuter.TangentAtEnd, radiusTop);
		Curve curve3 = ProngCurve(point3d, crvOuter2.PointAtStart, tangentAtStart * -1.0, crvOuter2.TangentAtStart * -1.0, radiusBottom);
		Curve curve4 = ProngCurve(point3d2, crvOuter2.PointAtEnd, tangentAtEnd, crvOuter2.TangentAtEnd, radiusBottom);
		Curve curve5 = CurveBetween2Points(edgeCurveStart, crvInner.PointAtStart, point3d);
		Curve curve6 = CurveBetween2Points(edgeCurveStart, crvOuter.PointAtStart, crvOuter2.PointAtStart);
		CSUtilSweepRC.Sweep2(curve5, curve6, new Curve[2] { curve, curve3 }, new double[2]
		{
			curve5.Domain.Min,
			curve5.Domain.Max
		}, new double[2]
		{
			curve6.Domain.Min,
			curve6.Domain.Max
		}, out var breps, closed: false, maintainHeight: true);
		Curve c = CurveBetween2Points(edgeCurveStart, crvInner.PointAtStart, crvOuter.PointAtStart);
		Vector3d v = curve5.TangentAtStart * -1.0 + curve6.TangentAtStart * -1.0;
		v.Unitize();
		Vector3d v2 = crvInner.TangentAtStart * -1.0 + crvOuter.TangentAtStart * -1.0;
		v2.Unitize();
		NurbsSurface nurbsSurface = LoftDir(curve, c, v, v2);
		Curve c2 = CurveBetween2Points(edgeCurveStart, point3d, crvOuter2.PointAtStart);
		v = curve5.TangentAtEnd + curve6.TangentAtEnd;
		v.Unitize();
		v2 = tangentAtStart * -1.0 + crvOuter2.TangentAtStart * -1.0;
		v2.Unitize();
		NurbsSurface nurbsSurface2 = LoftDir(curve3, c2, v, v2);
		curve5 = CurveBetween2Points(edgeCurveEnd, crvInner.PointAtEnd, point3d2);
		curve6 = CurveBetween2Points(edgeCurveEnd, crvOuter.PointAtEnd, crvOuter2.PointAtEnd);
		CSUtilSweepRC.Sweep2(curve5, curve6, new Curve[2] { curve2, curve4 }, new double[2]
		{
			curve5.Domain.Min,
			curve5.Domain.Max
		}, new double[2]
		{
			curve6.Domain.Min,
			curve6.Domain.Max
		}, out var breps2, closed: false, maintainHeight: true);
		Curve c3 = CurveBetween2Points(edgeCurveEnd, crvInner.PointAtEnd, crvOuter.PointAtEnd);
		v = curve5.TangentAtStart * -1.0 + curve6.TangentAtStart * -1.0;
		v.Unitize();
		v2 = crvInner.TangentAtEnd + crvOuter.TangentAtEnd;
		v2.Unitize();
		NurbsSurface nurbsSurface3 = LoftDir(curve2, c3, v, v2);
		Curve c4 = CurveBetween2Points(edgeCurveEnd, point3d2, crvOuter2.PointAtEnd);
		v = curve5.TangentAtEnd + curve6.TangentAtEnd;
		v.Unitize();
		v2 = tangentAtEnd + crvOuter2.TangentAtEnd;
		v2.Unitize();
		NurbsSurface nurbsSurface4 = LoftDir(curve4, c4, v, v2);
		return Brep.JoinBreps(new Brep[7]
		{
			vProng,
			breps,
			breps2,
			nurbsSurface.ToBrep(),
			nurbsSurface2.ToBrep(),
			nurbsSurface3.ToBrep(),
			nurbsSurface4.ToBrep()
		}, CSApplicationHelper.Application.Tolerance)[0];
	}

	private static Curve CurveBetween2Points(Curve crv, Point3d point0, Point3d point1)
	{
		if ((crv.PointAtStart - point0).Length > CSApplicationHelper.Application.Tolerance)
		{
			crv.ClosestPoint(point0, out var t);
			crv.ChangeClosedCurveSeam(t);
		}
		crv.ClosestPoint(point1, out var t2);
		Curve[] array = crv.Split(t2);
		if (array.Length != 2)
		{
			return null;
		}
		double length = array[0].GetLength();
		double length2 = array[1].GetLength();
		if (length < length2)
		{
			return array[0];
		}
		Curve obj = array[1];
		obj.Reverse();
		return obj;
	}

	private static bool CutBrep(Brep vProng, double zCut, NurbsSurface intSrf, out Curve crvInner, out Curve crvOuter)
	{
		NurbsSurface surface = NurbsSurface.CreateFromCorners(new Point3d(40.0, 40.0, zCut), new Point3d(40.0, -40.0, zCut), new Point3d(-40.0, -40.0, zCut), new Point3d(-40.0, 40.0, zCut));
		Intersection.BrepSurface(vProng, surface, CSApplicationHelper.Application.Tolerance, out var intersectionCurves, out var _);
		intersectionCurves = Curve.JoinCurves(intersectionCurves, CSApplicationHelper.Application.Tolerance);
		if (intersectionCurves.Length == 2)
		{
			Point3d point3d = new Point3d(0.0, 0.0, zCut);
			double length = (intersectionCurves[0].PointAtNormalizedLength(0.5) - point3d).Length;
			double length2 = (intersectionCurves[1].PointAtNormalizedLength(0.5) - point3d).Length;
			crvInner = ((length < length2) ? intersectionCurves[0] : intersectionCurves[1]);
			crvOuter = ((length < length2) ? intersectionCurves[1] : intersectionCurves[0]);
			TestFlip(ref crvInner, intSrf);
			TestFlip(ref crvOuter, intSrf);
			return true;
		}
		if (intersectionCurves.Length == 1)
		{
			crvInner = null;
			crvOuter = intersectionCurves[0];
			TestFlip(ref crvOuter, intSrf);
			return true;
		}
		crvInner = null;
		crvOuter = null;
		return false;
	}

	private static void GetEdgeCurves(Brep vProng, Curve crvTopInner, NurbsSurface srf0, NurbsSurface srf1, out Curve edgeCurveStart, out Curve edgeCurveEnd)
	{
		Curve[] array = vProng.DuplicateEdgeCurves(nakedOnly: true);
		if (array.Length != 2)
		{
			double[] array2 = new double[6] { 0.0, 0.2, 0.4, 0.6, 0.8, 1.0 };
			List<Curve> list = new List<Curve>();
			List<Curve> list2 = new List<Curve>();
			Curve[] array3 = array;
			foreach (Curve curve in array3)
			{
				double num = 0.0;
				double num2 = 0.0;
				double[] array4 = array2;
				foreach (double length in array4)
				{
					Point3d point3d = curve.PointAtNormalizedLength(length);
					srf0.ClosestPoint(point3d, out var u, out var v);
					Point3d point3d2 = srf0.PointAt(u, v);
					srf1.ClosestPoint(point3d, out u, out v);
					Point3d point3d3 = srf1.PointAt(u, v);
					num += (point3d - point3d2).Length;
					num2 += (point3d - point3d3).Length;
				}
				if (num < num2)
				{
					list.Add(curve);
				}
				else
				{
					list2.Add(curve);
				}
			}
			array = new Curve[2];
			if (list.Count == 1)
			{
				array[0] = list[0];
			}
			else
			{
				Curve[] array5 = Curve.JoinCurves(list, CSApplicationHelper.Application.Tolerance);
				if (array5.Length == 1)
				{
					array[0] = array5[0];
				}
				else
				{
					array[0] = null;
				}
			}
			if (list2.Count == 1)
			{
				array[1] = list2[0];
			}
			else
			{
				Curve[] array6 = Curve.JoinCurves(list2, CSApplicationHelper.Application.Tolerance);
				if (array6.Length == 1)
				{
					array[1] = array6[0];
				}
				else
				{
					array[1] = null;
				}
			}
		}
		for (int k = 0; k < 2; k++)
		{
			if (array[k].IsClosed)
			{
				continue;
			}
			if (array[k].IsClosable(CSApplicationHelper.Application.Tolerance))
			{
				array[k].MakeClosed(CSApplicationHelper.Application.Tolerance);
				if (array[k].IsClosed)
				{
					continue;
				}
			}
			Curve[] array7 = Curve.JoinCurves(new Curve[2]
			{
				array[k],
				new LineCurve(new Line(array[k].PointAtStart, array[k].PointAtEnd))
			});
			array[k] = array7[0];
		}
		if (array[0] == null || array[1] == null)
		{
			edgeCurveStart = null;
			edgeCurveEnd = null;
			return;
		}
		Point3d pointAtStart = crvTopInner.PointAtStart;
		array[0].ClosestPoint(pointAtStart, out var t);
		Point3d point3d4 = array[0].PointAt(t);
		array[1].ClosestPoint(pointAtStart, out t);
		Point3d point3d5 = array[1].PointAt(t);
		double length2 = (point3d4 - pointAtStart).Length;
		double length3 = (point3d5 - pointAtStart).Length;
		edgeCurveStart = ((length2 < length3) ? array[0] : array[1]);
		edgeCurveEnd = ((length2 < length3) ? array[1] : array[0]);
	}

	private static NurbsSurface LoftDir(Curve c0, Curve c1, Vector3d v0, Vector3d v1)
	{
		Brep brep = Brep.CreateFromLoft(new Curve[2] { c0, c1 }, Point3d.Unset, Point3d.Unset, LoftType.Tight, closed: false)[0];
		if (brep.Faces.Count != 1)
		{
			return null;
		}
		NurbsSurface nurbsSurface = brep.Faces[0].ToNurbsSurface();
		if (nurbsSurface.Points.CountU != 4)
		{
			return null;
		}
		int countV = nurbsSurface.Points.CountV;
		for (int i = 0; i < countV; i++)
		{
			Point3d location = nurbsSurface.Points.GetControlPoint(0, i).Location;
			Point3d location2 = nurbsSurface.Points.GetControlPoint(3, i).Location;
			double num = location.DistanceTo(location2);
			location += v0 * num / 3.0;
			location2 += v1 * num / 3.0;
			nurbsSurface.Points.SetControlPoint(1, i, location);
			nurbsSurface.Points.SetControlPoint(2, i, location2);
		}
		return nurbsSurface;
	}

	private static Curve ProngCurve(Point3d domeStart0, Point3d domeStart1, Vector3d prongDir0, Vector3d prongDir1, double domeRadius)
	{
		Point3d pointEnd = (domeStart0 + domeStart1) / 2.0;
		Vector3d vector3d = prongDir0 + prongDir1;
		vector3d.Unitize();
		pointEnd += vector3d * domeRadius;
		Vector3d vector3d2 = domeStart0 - domeStart1;
		vector3d2.Unitize();
		Vector3d vectorEnd = new Vector3d(vector3d2);
		vectorEnd.Reverse();
		Curve curve = CSUtilCurves.BlendCurve(domeStart0, pointEnd, prongDir0, vector3d2);
		Curve curve2 = CSUtilCurves.BlendCurve(domeStart1, pointEnd, prongDir1, vectorEnd);
		return Curve.JoinCurves(new Curve[2] { curve, curve2 })[0].Rebuild(10, 3, preserveTangents: true);
	}

	private static void TestFlip(ref Curve crv, NurbsSurface srf)
	{
		srf.ClosestPoint(crv.PointAtStart, out var u, out var v);
		Point3d point3d = srf.PointAt(u, v);
		srf.ClosestPoint(crv.PointAtEnd, out u, out v);
		Point3d point3d2 = srf.PointAt(u, v);
		double length = (point3d - crv.PointAtStart).Length;
		double length2 = (point3d2 - crv.PointAtEnd).Length;
		if (length > length2)
		{
			crv.Reverse();
		}
	}
}
