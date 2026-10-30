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
		//IL_0013: Unknown result type (might be due to invalid IL or missing references)
		//IL_0015: Unknown result type (might be due to invalid IL or missing references)
		//IL_001b: Unknown result type (might be due to invalid IL or missing references)
		//IL_001c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0028: Unknown result type (might be due to invalid IL or missing references)
		//IL_0068: Unknown result type (might be due to invalid IL or missing references)
		//IL_006d: Unknown result type (might be due to invalid IL or missing references)
		//IL_007d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0082: Unknown result type (might be due to invalid IL or missing references)
		_baseBrep = inBrep.DuplicateBrep();
		_xDown = xDown;
		_xUp = xUp;
		((GeometryBase)_baseBrep).Transform(_xDown);
		_baseSrf = null;
		if (_baseBrep.IsSurface)
		{
			_baseSrf = _baseBrep.Faces[0];
			_baseBrep = null;
		}
		Point3d val = ((BoundingBox)(ref boundingBox)).Max;
		_zMax = ((Point3d)(ref val)).Z;
		val = ((BoundingBox)(ref boundingBox)).Min;
		_zMin = ((Point3d)(ref val)).Z;
		_zSize = _zMax - _zMin;
	}

	public override Point3d MorphPoint(Point3d inputPoint)
	{
		//IL_000e: Unknown result type (might be due to invalid IL or missing references)
		//IL_000f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0013: Unknown result type (might be due to invalid IL or missing references)
		//IL_0032: Unknown result type (might be due to invalid IL or missing references)
		//IL_0033: Unknown result type (might be due to invalid IL or missing references)
		//IL_004f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0054: Unknown result type (might be due to invalid IL or missing references)
		//IL_0059: Unknown result type (might be due to invalid IL or missing references)
		//IL_005f: Expected O, but got Unknown
		//IL_0067: Unknown result type (might be due to invalid IL or missing references)
		//IL_0090: Unknown result type (might be due to invalid IL or missing references)
		//IL_009a: Unknown result type (might be due to invalid IL or missing references)
		Count++;
		Point3d val = inputPoint;
		((Point3d)(ref val)).Transform(_xDown);
		double z = ((Point3d)(ref val)).Z;
		((Point3d)(ref val)).Z = _zMax;
		LineCurve intersector = new LineCurve(val, val + new Vector3d(0.0, 0.0, -30.0));
		double num = Intersect(_baseBrep, (Curve)(object)intersector, val);
		((Point3d)(ref val)).Z = _zMax - (_zMax - z) * num / _zSize;
		((Point3d)(ref val)).Transform(_xUp);
		return val;
	}

	private double Intersect(Brep srfBrep, Curve intersector, Point3d objPt)
	{
		//IL_00bf: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ca: Unknown result type (might be due to invalid IL or missing references)
		//IL_00cf: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d3: Unknown result type (might be due to invalid IL or missing references)
		//IL_004a: Unknown result type (might be due to invalid IL or missing references)
		//IL_004f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0054: Unknown result type (might be due to invalid IL or missing references)
		//IL_0105: Unknown result type (might be due to invalid IL or missing references)
		//IL_010a: Unknown result type (might be due to invalid IL or missing references)
		//IL_010e: Unknown result type (might be due to invalid IL or missing references)
		double num = double.NaN;
		if (_baseSrf != null)
		{
			CurveIntersections val = Intersection.CurveSurface(intersector, (Surface)(object)_baseSrf, CSUtil.Tolerance, CSUtil.Tolerance);
			if (val.Count <= 0)
			{
				return _zSize;
			}
			{
				foreach (IntersectionEvent item in val)
				{
					Point3d pointA = item.PointA;
					double num2 = ((Point3d)(ref pointA)).DistanceTo(intersector.PointAtStart);
					if (double.IsNaN(num) || num > num2)
					{
						num = num2;
					}
				}
				return num;
			}
		}
		Curve[] array = default(Curve[]);
		Point3d[] array2 = default(Point3d[]);
		if (!Intersection.CurveBrep(intersector, srfBrep, CSUtil.Tolerance, ref array, ref array2))
		{
			return _zSize;
		}
		if (array2.Length == 0 && array.Length == 0)
		{
			return _zSize;
		}
		Curve[] array3 = array;
		double num3 = default(double);
		foreach (Curve obj in array3)
		{
			obj.ClosestPoint(objPt, ref num3);
			Point3d val2 = obj.PointAt(num3);
			num3 = ((Point3d)(ref val2)).DistanceTo(objPt);
			if (double.IsNaN(num) || num > num3)
			{
				num = num3;
			}
		}
		Point3d[] array4 = array2;
		for (int i = 0; i < array4.Length; i++)
		{
			Point3d val3 = array4[i];
			double num4 = ((Point3d)(ref val3)).DistanceTo(objPt);
			if (double.IsNaN(num) || num > num4)
			{
				num = num4;
			}
		}
		return num;
	}
}
