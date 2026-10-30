using Rhino.Geometry;

namespace CS.CSValues.CSValueHandle.Obsolete.HandleSupportClasses;

public sealed class CSBendMorph2 : CSSpaceMorph
{
	private readonly double _aAngle;

	private readonly int _bendMode;

	private readonly double _pLength;

	private readonly double _pRadius;

	private readonly Transform _xDown;

	private readonly Transform _xUp;

	public CSBendMorph2(CSBendPoints bend)
	{
		_bendMode = bend._bendMode;
		Vector3d source = new Vector3d(bend.EndPoint - bend.BasePoint);
		_pLength = source.Length;
		source.Unitize();
		Arc arc = bend.GetArc();
		Vector3d normal = arc.Plane.Normal;
		_pRadius = arc.Radius;
		_aAngle = arc.Angle;
		_xDown = CSCoreMath.Orient3Point(bend.BasePoint, source, normal, Point3d.Origin, Vector3d.XAxis, Vector3d.YAxis);
		_xDown.TryGetInverse(out _xUp);
	}

	public override Point3d MorphPoint(Point3d point)
	{
		if (_pLength == 0.0)
		{
			return point;
		}
		Point3d result = point;
		result.Transform(_xDown);
		if (_bendMode == 0 && result.X < 0.0)
		{
			return point;
		}
		Transform transform = default(Transform);
		double num = result.X / _pLength * _aAngle;
		if (num > _aAngle)
		{
			num = _aAngle;
			result.X -= _pLength;
		}
		else if (num < 0.0 - _aAngle)
		{
			num = 0.0 - _aAngle;
			result.X += _pLength;
		}
		else
		{
			result.X = 0.0;
		}
		transform = ((!(num < 0.0) || _bendMode != 2) ? Transform.Rotation(num, new Vector3d(0.0, 1.0, 0.0), new Point3d(0.0, 0.0, 0.0 - _pRadius)) : Transform.Rotation(num, new Vector3d(0.0, -1.0, 0.0), new Point3d(0.0, 0.0, _pRadius)));
		result.Transform(transform);
		result.Transform(_xUp);
		return result;
	}
}
