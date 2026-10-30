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
		//IL_002d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0032: Unknown result type (might be due to invalid IL or missing references)
		//IL_0037: Unknown result type (might be due to invalid IL or missing references)
		//IL_003c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0052: Unknown result type (might be due to invalid IL or missing references)
		//IL_0057: Unknown result type (might be due to invalid IL or missing references)
		//IL_005c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0061: Unknown result type (might be due to invalid IL or missing references)
		//IL_0091: Unknown result type (might be due to invalid IL or missing references)
		//IL_0096: Unknown result type (might be due to invalid IL or missing references)
		//IL_0099: Unknown result type (might be due to invalid IL or missing references)
		//IL_009e: Unknown result type (might be due to invalid IL or missing references)
		_angle = angle;
		_xNegativeRotation = CSUtilMath.Rotation(-1.0 * _angle, Vector3d.YAxis, Point3d.Origin);
		_xPositiveRotation = CSUtilMath.Rotation(1.0 * _angle, Vector3d.YAxis, Point3d.Origin);
		_radiusPow2 = railRadius * railRadius;
		if (_radiusPow2 <= 0.0)
		{
			_radiusPow2 = 10.0;
		}
		Point3d max = ((BoundingBox)(ref boundingBox)).Max;
		Point3d min = ((BoundingBox)(ref boundingBox)).Min;
		_zMax = ((Point3d)(ref max)).Z;
		_zMin = ((Point3d)(ref min)).Z;
		_zSize = _zMax - _zMin;
		_v = v;
	}

	public override Point3d MorphPoint(Point3d inputPoint)
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_0005: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e2: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e4: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e6: Unknown result type (might be due to invalid IL or missing references)
		//IL_00eb: Unknown result type (might be due to invalid IL or missing references)
		//IL_0131: Unknown result type (might be due to invalid IL or missing references)
		//IL_013b: Unknown result type (might be due to invalid IL or missing references)
		Point3d val = inputPoint;
		((Point3d)(ref val)).Transform(_xNegativeRotation);
		double num = (((Point3d)(ref val)).Z - _zMin) / _zSize;
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
		Point2d p = default(Point2d);
		((Point2d)(ref p))._002Ector(((Point3d)(ref val)).X, 0.0);
		Point2d p2 = default(Point2d);
		((Point2d)(ref p2))._002Ector(((Point3d)(ref val)).X, _zMax);
		Point2d val2 = IntersectCircleAndLine(_radiusPow2, p, p2);
		double num6 = _zMax - ((Point2d)(ref val2)).Y;
		double num7 = (((Point2d)(ref val2)).Y + num6 * num) * num2 + ((Point3d)(ref val)).Z * num5;
		Point3d result = default(Point3d);
		((Point3d)(ref result))._002Ector(((Point3d)(ref val)).X, ((Point3d)(ref val)).Y, num7);
		((Point3d)(ref result)).Transform(_xPositiveRotation);
		return result;
	}

	private static Point2d IntersectCircleAndLine(double r2, Point2d p1, Point2d p2)
	{
		//IL_006f: Unknown result type (might be due to invalid IL or missing references)
		double num = ((Point2d)(ref p2)).X - ((Point2d)(ref p1)).X;
		double num2 = ((Point2d)(ref p2)).Y - ((Point2d)(ref p1)).Y;
		double num3 = num * num + num2 * num2;
		double num4 = ((Point2d)(ref p1)).X * ((Point2d)(ref p2)).Y - ((Point2d)(ref p2)).X * ((Point2d)(ref p1)).Y;
		double num5 = Math.Sqrt(r2 * num3 - num4 * num4);
		double num6 = (num4 * num2 + num * num5) / num3;
		double num7 = ((0.0 - num4) * num + num2 * num5) / num3;
		return new Point2d(num6, num7);
	}
}
