using Rhino.Geometry;
using Rhino.Geometry.Intersect;

namespace CSBlockUtil;

public sealed class CSPullToBaseBrep : SpaceMorph
{
	private readonly Brep _baseBrep;

	private readonly BrepFace _baseSrf;

	private readonly double _zMax;

	private readonly double _zMin;

	private readonly double _zSize;

	private readonly Transform _xDown;

	private readonly Transform _xUp;

	public int Count;

	public CSPullToBaseBrep(BoundingBox boundingBox, Brep inBrep, Transform xUp, Transform xDown)
	{
		_baseBrep = inBrep.DuplicateBrep();
		_xDown = xDown;
		_xUp = xUp;
		_baseBrep.Transform(_xDown);
		_baseSrf = null;
		if (_baseBrep.IsSurface)
		{
			_baseSrf = _baseBrep.Faces[0];
			_baseBrep = null;
		}
		_zMax = boundingBox.Max.Z;
		_zMin = boundingBox.Min.Z;
		_zSize = _zMax - _zMin;
	}

	public override Point3d MorphPoint(Point3d inputPoint)
	{
		Count++;
		Point3d point3d = inputPoint;
		point3d.Transform(_xDown);
		double z = point3d.Z;
		point3d.Z = _zMax;
		LineCurve intersector = new LineCurve(point3d, point3d + new Vector3d(0.0, 0.0, -30.0));
		double num = Intersect(_baseBrep, intersector, point3d);
		point3d.Z = _zMax - (_zMax - z) * num / _zSize;
		point3d.Transform(_xUp);
		return point3d;
	}

	private double Intersect(Brep srfBrep, Curve intersector, Point3d objPt)
	{
		double num = double.NaN;
		if (_baseSrf != null)
		{
			CurveIntersections curveIntersections = Intersection.CurveSurface(intersector, _baseSrf, CSUtil.Tolerance, CSUtil.Tolerance);
			if (curveIntersections.Count <= 0)
			{
				return _zSize;
			}
			{
				foreach (IntersectionEvent item in curveIntersections)
				{
					double num2 = item.PointA.DistanceTo(intersector.PointAtStart);
					if (double.IsNaN(num) || num > num2)
					{
						num = num2;
					}
				}
				return num;
			}
		}
		if (!Intersection.CurveBrep(intersector, srfBrep, CSUtil.Tolerance, out var overlapCurves, out var intersectionPoints))
		{
			return _zSize;
		}
		if (intersectionPoints.Length == 0 && overlapCurves.Length == 0)
		{
			return _zSize;
		}
		Curve[] array = overlapCurves;
		foreach (Curve obj in array)
		{
			obj.ClosestPoint(objPt, out var t);
			t = obj.PointAt(t).DistanceTo(objPt);
			if (double.IsNaN(num) || num > t)
			{
				num = t;
			}
		}
		Point3d[] array2 = intersectionPoints;
		foreach (Point3d point3d in array2)
		{
			double num3 = point3d.DistanceTo(objPt);
			if (double.IsNaN(num) || num > num3)
			{
				num = num3;
			}
		}
		return num;
	}
}
