using System.Collections.Generic;
using System.Linq;
using GV.CounterSketch.Shared.CAD;
using Rhino;
using Rhino.DocObjects;
using Rhino.Geometry;

namespace CS.Utilities.Helpers;

public static class NurbsHelper
{
	public static GV.CounterSketch.Shared.CAD.BoundingBox GetBoundingBox(GV.CounterSketch.Shared.CAD.NurbsCurve nc)
	{
		Rhino.Geometry.BoundingBox boundingBox = GetVarFromCurve(nc).GetBoundingBox(accurate: false);
		return new GV.CounterSketch.Shared.CAD.BoundingBox
		{
			Max = 
			{
				X = boundingBox.Max.X,
				Y = boundingBox.Max.Y,
				Z = boundingBox.Max.Z
			},
			Min = 
			{
				X = boundingBox.Min.X,
				Y = boundingBox.Min.Y,
				Z = boundingBox.Min.Z
			}
		};
	}

	public static double GetClosestPoint(GV.CounterSketch.Shared.CAD.NurbsCurve nc, Point3D testHit)
	{
		Rhino.Geometry.NurbsCurve varFromCurve = GetVarFromCurve(nc);
		Point3d testPoint = new Point3d(testHit.X, testHit.Y, testHit.Z);
		varFromCurve.ClosestPoint(testPoint, out var t);
		return t;
	}

	public static Point3D GetNormalizedPointAt(GV.CounterSketch.Shared.CAD.NurbsCurve nc, double editPointParam)
	{
		Rhino.Geometry.NurbsCurve varFromCurve = GetVarFromCurve(nc);
		Point3d point3d = varFromCurve.PointAt(varFromCurve.Domain.ParameterAt(editPointParam));
		return new Point3D(point3d.X, point3d.Y, point3d.Z);
	}

	public static IEnumerable<RhinoObject> GetNormalObjects()
	{
		ObjectEnumeratorSettings settings = new ObjectEnumeratorSettings
		{
			ObjectTypeFilter = (ObjectType.Surface | ObjectType.Brep | ObjectType.Mesh | ObjectType.PolysrfFilter),
			DeletedObjects = false,
			NormalObjects = true,
			IncludeGrips = false,
			IncludeLights = false,
			IncludePhantoms = false,
			LockedObjects = false,
			HiddenObjects = false
		};
		return RhinoDoc.ActiveDoc.Objects.GetObjectList(settings);
	}

	public static List<Point2D> GetPolyline(GV.CounterSketch.Shared.CAD.NurbsCurve nc)
	{
		PolylineCurve polylineCurve = GetVarFromCurve(nc).ToPolyline(0, 0, 0.0, 0.0, 0.0, 0.01, 0.0, 0.0, keepStartPoint: false);
		List<Point2D> list = new List<Point2D>();
		for (int i = 0; i < polylineCurve.PointCount; i++)
		{
			Point2D item = new Point2D
			{
				X = polylineCurve.Point(i).X,
				Y = polylineCurve.Point(i).Y
			};
			list.Add(item);
		}
		return list;
	}

	public static Rhino.Geometry.NurbsCurve GetVarFromCurve(GV.CounterSketch.Shared.CAD.NurbsCurve nurbsCurve)
	{
		Point3d[] points = nurbsCurve.Points.Select((Point4D point) => new Point3d(point.X, point.Y, point.Z)).ToArray();
		Rhino.Geometry.NurbsCurve nurbsCurve2 = Rhino.Geometry.NurbsCurve.Create(nurbsCurve.IsClosed, nurbsCurve.Degree, points);
		if (nurbsCurve2 == null)
		{
			return null;
		}
		if (!nurbsCurve2.IsValid)
		{
			return null;
		}
		return nurbsCurve2;
	}

	public static GV.CounterSketch.Shared.CAD.NurbsCurve Rebuild(GV.CounterSketch.Shared.CAD.NurbsCurve nc, int degree, int pointCount)
	{
		return SetVarFromCurve(GetVarFromCurve(nc).Rebuild(pointCount, degree, preserveTangents: true));
	}

	public static GV.CounterSketch.Shared.CAD.NurbsCurve SetDomain(GV.CounterSketch.Shared.CAD.NurbsCurve nc, int t0, int t1)
	{
		Rhino.Geometry.NurbsCurve varFromCurve = GetVarFromCurve(nc);
		varFromCurve.Domain = new Interval(t0, t1);
		return SetVarFromCurve(varFromCurve);
	}

	public static GV.CounterSketch.Shared.CAD.NurbsCurve SetVarFromCurve(Rhino.Geometry.NurbsCurve nurbsCurve)
	{
		GV.CounterSketch.Shared.CAD.NurbsCurve nurbsCurve2 = new GV.CounterSketch.Shared.CAD.NurbsCurve
		{
			Dimension = nurbsCurve.Dimension,
			IsRational = (nurbsCurve.IsRational ? 1 : 0),
			Order = nurbsCurve.Order
		};
		int count = nurbsCurve.Points.Count;
		nurbsCurve2.Points = new Point4D[count];
		for (int i = 0; i < count; i++)
		{
			ControlPoint controlPoint = nurbsCurve.Points[i];
			nurbsCurve2.Points[i] = new Point4D
			{
				X = controlPoint.Location.X,
				Y = controlPoint.Location.Y,
				Z = controlPoint.Location.Z,
				W = controlPoint.Weight
			};
		}
		int count2 = nurbsCurve.Knots.Count;
		nurbsCurve2.Knots = new double[count2];
		for (int j = 0; j < count2; j++)
		{
			nurbsCurve2.Knots[j] = nurbsCurve.Knots[j];
		}
		nurbsCurve2.Domain = new double[2]
		{
			nurbsCurve.Domain.T0,
			nurbsCurve.Domain.T1
		};
		nurbsCurve2.Degree = nurbsCurve.Degree;
		nurbsCurve2.IsClamped = nurbsCurve.Knots.IsClampedEnd || nurbsCurve.Knots.IsClampedStart;
		nurbsCurve2.IsClosed = nurbsCurve.IsClosed;
		return nurbsCurve2;
	}
}
