using Rhino.Geometry;

namespace CS.CSValues.CSValueHandle.Obsolete.HandleSupportClasses;

public sealed class CSBendMorph1 : CSSpaceMorph
{
	private readonly double _aAngle;

	private readonly double _pLength;

	private readonly double _pRadius;

	private readonly Transform _xDown;

	private readonly Transform _xUp;

	public CSBendMorph1(Point3d p0, Point3d p1, Arc arc)
	{
		if (arc.IsValid)
		{
			Vector3d source = new Vector3d(p1 - p0);
			_pLength = source.Length;
			source.Unitize();
			Vector3d normal = arc.Plane.Normal;
			_pRadius = arc.Radius;
			_aAngle = arc.Angle;
			_xDown = CSCoreMath.Orient3Point(p0, source, normal, new Point3d(0.0, 0.0, 0.0), new Vector3d(1.0, 0.0, 0.0), new Vector3d(0.0, 1.0, 0.0));
			_xDown.TryGetInverse(out _xUp);
		}
	}

	public override Point3d MorphPoint(Point3d point)
	{
		if (_pLength == 0.0)
		{
			return point;
		}
		Point3d result = point;
		result.Transform(_xDown);
		double angleRadians = result.X / _pLength * _aAngle;
		result.X = 0.0;
		Transform xform = Transform.Rotation(angleRadians, new Vector3d(0.0, 1.0, 0.0), new Point3d(0.0, 0.0, 0.0 - _pRadius));
		result.Transform(xform);
		result.Transform(_xUp);
		return result;
	}
}
