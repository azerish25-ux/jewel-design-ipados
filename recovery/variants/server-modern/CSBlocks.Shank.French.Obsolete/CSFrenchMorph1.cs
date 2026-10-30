using System.Collections.Generic;
using Rhino.Geometry;

namespace CSBlocks.Shank.French.Obsolete;

public sealed class CSFrenchMorph1 : SpaceMorph
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

	public CSFrenchMorph1(double bottomSize, double midSize, double topSize)
	{
		_yScale = 2.0;
		YScale = new double[101];
		ZScale = new double[101];
		XShift = new double[101];
		CrvTangent = (Vector3d[])(object)new Vector3d[101];
		CrvPoints = (Point3d[])(object)new Point3d[101];
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
		//IL_001c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0028: Unknown result type (might be due to invalid IL or missing references)
		//IL_002d: Unknown result type (might be due to invalid IL or missing references)
		//IL_004f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0050: Unknown result type (might be due to invalid IL or missing references)
		//IL_0055: Unknown result type (might be due to invalid IL or missing references)
		//IL_016a: Unknown result type (might be due to invalid IL or missing references)
		//IL_016f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0171: Unknown result type (might be due to invalid IL or missing references)
		//IL_0172: Unknown result type (might be due to invalid IL or missing references)
		//IL_017d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0182: Unknown result type (might be due to invalid IL or missing references)
		//IL_0187: Unknown result type (might be due to invalid IL or missing references)
		//IL_018b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0190: Unknown result type (might be due to invalid IL or missing references)
		double x = ((Point3d)(ref point)).X;
		x = Interp(XShift, x);
		Point3d val = Interp(CrvPoints, x);
		Point3d val2 = Interp(BaseCrvPoints, x);
		double num = Interp(ZScale, x);
		double num2 = _yScale * Interp(YScale, x);
		Vector3d val3 = val - val2;
		double length = ((Vector3d)(ref val3)).Length;
		((Vector3d)(ref val3)).Unitize();
		double z = ((Point3d)(ref point)).Z;
		double num3 = 0.0;
		if (length < _totalSize * num)
		{
			num3 = length * z;
		}
		else if (WhichPart == 1 || (WhichPart == 0 && z < _basePer))
		{
			double num4 = _bottomSize * num;
			num3 = z / _basePer * num4;
		}
		else if (WhichPart == 3 || (WhichPart == 0 && z > 1.0 - _topPer))
		{
			double num5 = _topSize * num;
			num3 = length - (1.0 - z) / _topPer * num5;
		}
		else if (WhichPart == 2 || WhichPart == 0)
		{
			double num6 = _bottomSize * num;
			double num7 = _topSize * num;
			double num8 = length - (num6 + num7);
			double num9 = (z - _basePer) / _midPer;
			num3 = num6 + num8 * num9;
		}
		Vector3d tan = GetTan(x, num3 / length);
		return val2 + tan * (((Point3d)(ref point)).Y * num2) + val3 * num3;
	}

	private Vector3d GetTan(double x, double z)
	{
		//IL_002d: Unknown result type (might be due to invalid IL or missing references)
		//IL_003b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0047: Unknown result type (might be due to invalid IL or missing references)
		//IL_004c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0052: Unknown result type (might be due to invalid IL or missing references)
		//IL_0057: Unknown result type (might be due to invalid IL or missing references)
		//IL_005c: Unknown result type (might be due to invalid IL or missing references)
		//IL_001e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0023: Unknown result type (might be due to invalid IL or missing references)
		//IL_005d: Unknown result type (might be due to invalid IL or missing references)
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
		//IL_0021: Unknown result type (might be due to invalid IL or missing references)
		//IL_002a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0031: Unknown result type (might be due to invalid IL or missing references)
		//IL_0036: Unknown result type (might be due to invalid IL or missing references)
		//IL_003c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0041: Unknown result type (might be due to invalid IL or missing references)
		//IL_0019: Unknown result type (might be due to invalid IL or missing references)
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
