using System.Collections.Generic;
using Rhino.Geometry;

namespace CSBlocks.Shank.French;

public sealed class CSFrenchMorph : SpaceMorph
{
	public readonly Point3d[] CrvPoints;

	public readonly Vector3d[] CrvTangent;

	public readonly double[] CrvParam;

	public readonly double[] YScale;

	public readonly double[] ZScale;

	public readonly double[] XShift;

	private readonly double _bottomSize;

	private readonly double _topSize;

	private readonly double _yScale;

	private readonly double _totalSize;

	private readonly double _basePer;

	private readonly double _midPer;

	private readonly double _topPer;

	public Point3d[] BaseCrvPoints;

	public int WhichPart;

	public CSFrenchMorph(double bottomSize, double midSize, double topSize)
	{
		_yScale = 2.0;
		YScale = new double[101];
		ZScale = new double[101];
		XShift = new double[101];
		CrvTangent = new Vector3d[101];
		CrvPoints = new Point3d[101];
		CrvParam = new double[101];
		_bottomSize = bottomSize;
		_topSize = topSize;
		_totalSize = _topSize + midSize + _bottomSize;
		_basePer = _bottomSize / _totalSize;
		_midPer = midSize / _totalSize;
		_topPer = _topSize / _totalSize;
	}

	public override Point3d MorphPoint(Point3d point)
	{
		double x = point.X;
		x = Interp(XShift, x);
		Point3d point3d = Interp(CrvPoints, x);
		Point3d point3d2 = Interp(BaseCrvPoints, x);
		double num = Interp(ZScale, x);
		double num2 = _yScale * Interp(YScale, x);
		Vector3d vector3d = point3d - point3d2;
		double length = vector3d.Length;
		vector3d.Unitize();
		double z = point.Z;
		double num3 = 0.0;
		if (WhichPart == 1 || (WhichPart == 0 && z < _basePer))
		{
			double num4 = _bottomSize * num;
			double num5 = _topSize * num;
			if (length < _totalSize * num && length - (num4 + num5) < 0.1)
			{
				num4 = length - (num5 + 0.1);
			}
			num3 = z / _basePer * num4;
		}
		else if (WhichPart == 3 || (WhichPart == 0 && z > 1.0 - _topPer))
		{
			double num6 = _topSize * num;
			num3 = length - (1.0 - z) / _topPer * num6;
		}
		else if (WhichPart == 2 || WhichPart == 0)
		{
			double num7 = _bottomSize * num;
			double num8 = _topSize * num;
			double num9 = length - (num7 + num8);
			if (num9 < 0.1)
			{
				num9 = 0.1;
				num7 = length - (num8 + 0.1);
			}
			double num10 = (z - _basePer) / _midPer;
			num3 = num7 + num9 * num10;
		}
		Vector3d tan = GetTan(x, num3 / length);
		return point3d2 + tan * (point.Y * num2) + vector3d * num3;
	}

	private Vector3d GetTan(double x, double z)
	{
		double num = x * 100.0;
		int num2 = (int)num;
		double num3 = num - (double)num2;
		if (num2 == 100)
		{
			return CrvTangent[num2];
		}
		return CrvTangent[num2] + (CrvTangent[num2 + 1] - CrvTangent[num2]) * num3;
	}

	private static Point3d Interp(IReadOnlyList<Point3d> v, double d)
	{
		double num = d * 100.0;
		int num2 = (int)num;
		double num3 = num - (double)num2;
		if (num2 == 100)
		{
			return v[num2];
		}
		return v[num2] + (v[num2 + 1] - v[num2]) * num3;
	}

	private static double Interp(IReadOnlyList<double> v, double d)
	{
		double num = d * 100.0;
		int num2 = (int)num;
		double num3 = num - (double)num2;
		if (num2 == 100)
		{
			return v[num2];
		}
		return v[num2] + (v[num2 + 1] - v[num2]) * num3;
	}
}
