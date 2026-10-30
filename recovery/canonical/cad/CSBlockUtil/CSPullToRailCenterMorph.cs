using System;
using Rhino.Geometry;

namespace CSBlockUtil;

public sealed class CSPullToRailCenterMorph : SpaceMorph
{
	private readonly double _angle;

	private readonly double _radiusPow2;

	private readonly double _zMax;

	private readonly double _zMin;

	private readonly double _zSize;

	private readonly double _pullValue;

	private readonly Transform _xNegativeRotation;

	private readonly Transform _xPositiveRotation;

	public int Count;

	public CSPullToRailCenterMorph(double angle, double railRadius, double pullAmount, BoundingBox boundingBox)
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
		_pullValue = pullAmount;
	}

	public override Point3d MorphPoint(Point3d inputPoint)
	{
		Count++;
		Point3d point3d = inputPoint;
		point3d.Transform(_xNegativeRotation);
		double x = point3d.X;
		double num = point3d.Z - _zMin;
		Point2d point2d = new Point2d(x * _pullValue, 0.0);
		Point2d p = new Point2d(x, _zMax);
		Vector3d vector3d = new Vector3d(x * (1.0 - _pullValue), 0.0, _zMax);
		double length = (IntersectCircleAndLine(_radiusPow2, point2d, p) - point2d).Length;
		double num2 = vector3d.Length - length;
		vector3d.Unitize();
		double num3 = length + num2 * num / _zSize;
		vector3d *= num3;
		vector3d.X += point2d.X;
		Point3d result = new Point3d(vector3d.X, point3d.Y, vector3d.Z);
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
