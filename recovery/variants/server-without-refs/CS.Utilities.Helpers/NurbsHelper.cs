using System;
using System.Collections.Generic;
using System.Linq;
using GV.CounterSketch.Shared.CAD;
using Rhino;
using Rhino.DocObjects;
using Rhino.Geometry;
using Rhino.Runtime;

namespace CS.Utilities.Helpers;

public static class NurbsHelper
{
	public static BoundingBox GetBoundingBox(NurbsCurve nc)
	{
		//IL_0007: Unknown result type (might be due to invalid IL or missing references)
		//IL_000c: Unknown result type (might be due to invalid IL or missing references)
		//IL_001a: Unknown result type (might be due to invalid IL or missing references)
		//IL_001f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0034: Unknown result type (might be due to invalid IL or missing references)
		//IL_0039: Unknown result type (might be due to invalid IL or missing references)
		//IL_004e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0053: Unknown result type (might be due to invalid IL or missing references)
		//IL_0068: Unknown result type (might be due to invalid IL or missing references)
		//IL_006d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0082: Unknown result type (might be due to invalid IL or missing references)
		//IL_0087: Unknown result type (might be due to invalid IL or missing references)
		//IL_009c: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a1: Unknown result type (might be due to invalid IL or missing references)
		BoundingBox boundingBox = ((GeometryBase)GetVarFromCurve(nc)).GetBoundingBox(false);
		BoundingBox boundingBox2 = new BoundingBox();
		Point3D max = boundingBox2.Max;
		Point3d val = ((BoundingBox)(ref boundingBox)).Max;
		max.X = ((Point3d)(ref val)).X;
		Point3D max2 = boundingBox2.Max;
		val = ((BoundingBox)(ref boundingBox)).Max;
		max2.Y = ((Point3d)(ref val)).Y;
		Point3D max3 = boundingBox2.Max;
		val = ((BoundingBox)(ref boundingBox)).Max;
		max3.Z = ((Point3d)(ref val)).Z;
		Point3D min = boundingBox2.Min;
		val = ((BoundingBox)(ref boundingBox)).Min;
		min.X = ((Point3d)(ref val)).X;
		Point3D min2 = boundingBox2.Min;
		val = ((BoundingBox)(ref boundingBox)).Min;
		min2.Y = ((Point3d)(ref val)).Y;
		Point3D min3 = boundingBox2.Min;
		val = ((BoundingBox)(ref boundingBox)).Min;
		min3.Z = ((Point3d)(ref val)).Z;
		return boundingBox2;
	}

	public static double GetClosestPoint(NurbsCurve nc, Point3D testHit)
	{
		//IL_001f: Unknown result type (might be due to invalid IL or missing references)
		NurbsCurve varFromCurve = GetVarFromCurve(nc);
		Point3d val = default(Point3d);
		((Point3d)(ref val))._002Ector(testHit.X, testHit.Y, testHit.Z);
		double result = default(double);
		((Curve)varFromCurve).ClosestPoint(val, ref result);
		return result;
	}

	public static Point3D GetNormalizedPointAt(NurbsCurve nc, double editPointParam)
	{
		//IL_0007: Unknown result type (might be due to invalid IL or missing references)
		//IL_000c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0015: Unknown result type (might be due to invalid IL or missing references)
		//IL_001a: Unknown result type (might be due to invalid IL or missing references)
		NurbsCurve varFromCurve = GetVarFromCurve(nc);
		Interval domain = ((Curve)varFromCurve).Domain;
		Point3d val = ((Curve)varFromCurve).PointAt(((Interval)(ref domain)).ParameterAt(editPointParam));
		return new Point3D(((Point3d)(ref val)).X, ((Point3d)(ref val)).Y, ((Point3d)(ref val)).Z);
	}

	public static IEnumerable<RhinoObject> GetNormalObjects()
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0005: Unknown result type (might be due to invalid IL or missing references)
		//IL_0010: Unknown result type (might be due to invalid IL or missing references)
		//IL_0017: Unknown result type (might be due to invalid IL or missing references)
		//IL_001e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0025: Unknown result type (might be due to invalid IL or missing references)
		//IL_002c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0033: Unknown result type (might be due to invalid IL or missing references)
		//IL_003a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0042: Expected O, but got Unknown
		ObjectEnumeratorSettings val = new ObjectEnumeratorSettings
		{
			ObjectTypeFilter = (ObjectType)2097208,
			DeletedObjects = false,
			NormalObjects = true,
			IncludeGrips = false,
			IncludeLights = false,
			IncludePhantoms = false,
			LockedObjects = false,
			HiddenObjects = false
		};
		return RhinoDoc.ActiveDoc.Objects.GetObjectList(val);
	}

	public static List<Point2D> GetPolyline(NurbsCurve nc)
	{
		//IL_0057: Unknown result type (might be due to invalid IL or missing references)
		//IL_005c: Unknown result type (might be due to invalid IL or missing references)
		//IL_006d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0072: Unknown result type (might be due to invalid IL or missing references)
		PolylineCurve val = ((Curve)GetVarFromCurve(nc)).ToPolyline(0, 0, 0.0, 0.0, 0.0, 0.01, 0.0, 0.0, false);
		List<Point2D> list = new List<Point2D>();
		for (int i = 0; i < val.PointCount; i++)
		{
			Point2D point2D = new Point2D();
			Point3d val2 = val.Point(i);
			point2D.X = ((Point3d)(ref val2)).X;
			val2 = val.Point(i);
			point2D.Y = ((Point3d)(ref val2)).Y;
			Point2D item = point2D;
			list.Add(item);
		}
		return list;
	}

	public static NurbsCurve GetVarFromCurve(NurbsCurve nurbsCurve)
	{
		Point3d[] array = ((IEnumerable<Point4D>)nurbsCurve.Points).Select((Func<Point4D, Point3d>)((Point4D point) => new Point3d(point.X, point.Y, point.Z))).ToArray();
		NurbsCurve val = NurbsCurve.Create(nurbsCurve.IsClosed, nurbsCurve.Degree, (IEnumerable<Point3d>)array);
		if (val == null)
		{
			return null;
		}
		if (!((CommonObject)val).IsValid)
		{
			return null;
		}
		return val;
	}

	public static NurbsCurve Rebuild(NurbsCurve nc, int degree, int pointCount)
	{
		return SetVarFromCurve(((Curve)GetVarFromCurve(nc)).Rebuild(pointCount, degree, true));
	}

	public static NurbsCurve SetDomain(NurbsCurve nc, int t0, int t1)
	{
		//IL_000b: Unknown result type (might be due to invalid IL or missing references)
		NurbsCurve varFromCurve = GetVarFromCurve(nc);
		((Curve)varFromCurve).Domain = new Interval((double)t0, (double)t1);
		return SetVarFromCurve(varFromCurve);
	}

	public static NurbsCurve SetVarFromCurve(NurbsCurve nurbsCurve)
	{
		//IL_0053: Unknown result type (might be due to invalid IL or missing references)
		//IL_0058: Unknown result type (might be due to invalid IL or missing references)
		//IL_0069: Unknown result type (might be due to invalid IL or missing references)
		//IL_006e: Unknown result type (might be due to invalid IL or missing references)
		//IL_007f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0084: Unknown result type (might be due to invalid IL or missing references)
		//IL_0095: Unknown result type (might be due to invalid IL or missing references)
		//IL_009a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0106: Unknown result type (might be due to invalid IL or missing references)
		//IL_010b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0118: Unknown result type (might be due to invalid IL or missing references)
		//IL_011d: Unknown result type (might be due to invalid IL or missing references)
		NurbsCurve nurbsCurve2 = new NurbsCurve
		{
			Dimension = ((Curve)nurbsCurve).Dimension,
			IsRational = (nurbsCurve.IsRational ? 1 : 0),
			Order = nurbsCurve.Order
		};
		int count = nurbsCurve.Points.Count;
		nurbsCurve2.Points = new Point4D[count];
		for (int i = 0; i < count; i++)
		{
			ControlPoint val = nurbsCurve.Points[i];
			Point4D[] points = nurbsCurve2.Points;
			int num = i;
			Point4D point4D = new Point4D();
			Point3d location = ((ControlPoint)(ref val)).Location;
			point4D.X = ((Point3d)(ref location)).X;
			location = ((ControlPoint)(ref val)).Location;
			point4D.Y = ((Point3d)(ref location)).Y;
			location = ((ControlPoint)(ref val)).Location;
			point4D.Z = ((Point3d)(ref location)).Z;
			point4D.W = ((ControlPoint)(ref val)).Weight;
			points[num] = point4D;
		}
		int count2 = nurbsCurve.Knots.Count;
		nurbsCurve2.Knots = new double[count2];
		for (int j = 0; j < count2; j++)
		{
			nurbsCurve2.Knots[j] = nurbsCurve.Knots[j];
		}
		double[] array = new double[2];
		Interval domain = ((Curve)nurbsCurve).Domain;
		array[0] = ((Interval)(ref domain)).T0;
		domain = ((Curve)nurbsCurve).Domain;
		array[1] = ((Interval)(ref domain)).T1;
		nurbsCurve2.Domain = array;
		nurbsCurve2.Degree = ((Curve)nurbsCurve).Degree;
		nurbsCurve2.IsClamped = nurbsCurve.Knots.IsClampedEnd || nurbsCurve.Knots.IsClampedStart;
		nurbsCurve2.IsClosed = ((Curve)nurbsCurve).IsClosed;
		return nurbsCurve2;
	}
}
