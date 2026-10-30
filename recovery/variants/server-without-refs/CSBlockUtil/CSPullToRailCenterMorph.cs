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
		//IL_001e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0023: Unknown result type (might be due to invalid IL or missing references)
		//IL_0028: Unknown result type (might be due to invalid IL or missing references)
		//IL_002d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0043: Unknown result type (might be due to invalid IL or missing references)
		//IL_0048: Unknown result type (might be due to invalid IL or missing references)
		//IL_004d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0052: Unknown result type (might be due to invalid IL or missing references)
		//IL_0082: Unknown result type (might be due to invalid IL or missing references)
		//IL_0087: Unknown result type (might be due to invalid IL or missing references)
		//IL_008a: Unknown result type (might be due to invalid IL or missing references)
		//IL_008f: Unknown result type (might be due to invalid IL or missing references)
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
		_pullValue = pullAmount;
	}

	public override Point3d MorphPoint(Point3d inputPoint)
	{
		//IL_000e: Unknown result type (might be due to invalid IL or missing references)
		//IL_000f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0013: Unknown result type (might be due to invalid IL or missing references)
		//IL_0088: Unknown result type (might be due to invalid IL or missing references)
		//IL_0089: Unknown result type (might be due to invalid IL or missing references)
		//IL_008b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0090: Unknown result type (might be due to invalid IL or missing references)
		//IL_0091: Unknown result type (might be due to invalid IL or missing references)
		//IL_0096: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c5: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c9: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ce: Unknown result type (might be due to invalid IL or missing references)
		//IL_0104: Unknown result type (might be due to invalid IL or missing references)
		//IL_010e: Unknown result type (might be due to invalid IL or missing references)
		Count++;
		Point3d val = inputPoint;
		((Point3d)(ref val)).Transform(_xNegativeRotation);
		double x = ((Point3d)(ref val)).X;
		double num = ((Point3d)(ref val)).Z - _zMin;
		Point2d val2 = default(Point2d);
		((Point2d)(ref val2))._002Ector(x * _pullValue, 0.0);
		Point2d p = default(Point2d);
		((Point2d)(ref p))._002Ector(x, _zMax);
		Vector3d val3 = default(Vector3d);
		((Vector3d)(ref val3))._002Ector(x * (1.0 - _pullValue), 0.0, _zMax);
		Vector2d val4 = IntersectCircleAndLine(_radiusPow2, val2, p) - val2;
		double length = ((Vector2d)(ref val4)).Length;
		double num2 = ((Vector3d)(ref val3)).Length - length;
		((Vector3d)(ref val3)).Unitize();
		double num3 = length + num2 * num / _zSize;
		val3 *= num3;
		((Vector3d)(ref val3)).X = ((Vector3d)(ref val3)).X + ((Point2d)(ref val2)).X;
		Point3d result = default(Point3d);
		((Point3d)(ref result))._002Ector(((Vector3d)(ref val3)).X, ((Point3d)(ref val)).Y, ((Vector3d)(ref val3)).Z);
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
