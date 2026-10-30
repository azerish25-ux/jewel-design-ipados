using System;
using Rhino.Geometry;

namespace CS.CSValues.CSValueHandle.HandleSupportClasses;

public sealed class CSBendMorph : CSSpaceMorph
{
	private readonly double _aAngle;

	private readonly int _bendMode;

	private readonly double _pLength;

	private readonly double _pRadius;

	private readonly bool _validArc;

	private readonly Transform _xDown;

	private readonly Transform _xUp;

	public CSBendMorph(CSBendPoints bend)
	{
		_bendMode = bend._bendMode;
		_validArc = bend.GetArc(out var arc);
		if (_validArc)
		{
			Vector3d source = bend.EndPoint - bend.BasePoint;
			_pLength = source.Length;
			source.Unitize();
			Vector3d normal = arc.Plane.Normal;
			_pRadius = arc.Radius;
			_aAngle = arc.Angle;
			_xDown = CSCoreMath.Orient3Point(bend.BasePoint, source, normal, Point3d.Origin, Vector3d.XAxis, Vector3d.YAxis);
			_xDown.TryGetInverse(out _xUp);
		}
	}

	public override Point3d MorphPoint(Point3d point)
	{
		if (!_validArc)
		{
			return point;
		}
		if (Math.Abs(_pLength - 0.0) < 0.0001)
		{
			return point;
		}
		Point3d result = point;
		result.Transform(_xDown);
		if (_bendMode == 0 && result.X < 0.0)
		{
			return point;
		}
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
		Transform xform = ((!(num < 0.0) || _bendMode != 2) ? Transform.Rotation(num, new Vector3d(0.0, 1.0, 0.0), new Point3d(0.0, 0.0, 0.0 - _pRadius)) : Transform.Rotation(num, new Vector3d(0.0, -1.0, 0.0), new Point3d(0.0, 0.0, _pRadius)));
		result.Transform(xform);
		result.Transform(_xUp);
		return result;
	}
}
