using System;
using Rhino.Geometry;

namespace CSBlockUtil;

public sealed class CSPullToRailMorph : SpaceMorph
{
	private readonly double _angle;

	private readonly double _radiusPow2;

	private readonly double _zMax;

	private readonly double _zMin;

	private readonly double _zSize;

	private readonly double _pull = 0.95;

	private readonly double _v;

	private readonly Transform _xNegativeRotation;

	private readonly Transform _xPositiveRotation;

	public CSPullToRailMorph(double angle, double railRadius, double v, BoundingBox boundingBox)
	{
		_angle = angle;
		_xNegativeRotation = CSUtilMath.Rotation(-1.0 * _angle, Vector3d.YAxis, Point3d.Origin);
		_xPositiveRotation = CSUtilMath.Rotation(1.0 * _angle, Vector3d.YAxis, Point3d.Origin);
		_radiusPow2 = railRadius * railRadius;
		if (_radiusPow2 <= 0.0)
		{
			_radiusPow2 = 10.0;
		}
		Point3d max = boundingBox.Max;
		Point3d min = boundingBox.Min;
		_zMax = max.Z;
		_zMin = min.Z;
		_zSize = _zMax - _zMin;
		_v = v;
	}

	public override Point3d MorphPoint(Point3d inputPoint)
	{
		Point3d point3d = inputPoint;
		point3d.Transform(_xNegativeRotation);
		double num = (point3d.Z - _zMin) / _zSize;
		double num2 = Math.Pow(1.0 - num / _v, 2.0);
		if (num > _v)
		{
			num2 = 0.0;
		}
		if (_v >= _pull)
		{
			double num3 = 1.0 - num;
			double num4 = (_v - _pull) / (1.0 - _pull);
			num2 = num3 * num4 + (1.0 - num4) * num2;
		}
		double num5 = 1.0 - num2;
		Point2d point2d = IntersectCircleAndLine(p1: new Point2d(point3d.X, 0.0), p2: new Point2d(point3d.X, _zMax), r2: _radiusPow2);
		double num6 = _zMax - point2d.Y;
		double z = (point2d.Y + num6 * num) * num2 + point3d.Z * num5;
		Point3d result = new Point3d(point3d.X, point3d.Y, z);
		result.Transform(_xPositiveRotation);
		return result;
	}

	private static Point2d IntersectCircleAndLine(double r2, Point2d p1, Point2d p2)
	{
		double num = p2.X - p1.X;
		double num2 = p2.Y - p1.Y;
		double num3 = num * num + num2 * num2;
		double num4 = p1.X * p2.Y - p2.X * p1.Y;
		double num5 = Math.Sqrt(r2 * num3 - num4 * num4);
		double x = (num4 * num2 + num * num5) / num3;
		double y = ((0.0 - num4) * num + num2 * num5) / num3;
		return new Point2d(x, y);
	}
}
