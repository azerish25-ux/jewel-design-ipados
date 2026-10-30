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
		//IL_002a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0030: Unknown result type (might be due to invalid IL or missing references)
		//IL_0035: Unknown result type (might be due to invalid IL or missing references)
		//IL_003a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0052: Unknown result type (might be due to invalid IL or missing references)
		//IL_0057: Unknown result type (might be due to invalid IL or missing references)
		//IL_005a: Unknown result type (might be due to invalid IL or missing references)
		//IL_005f: Unknown result type (might be due to invalid IL or missing references)
		//IL_007c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0081: Unknown result type (might be due to invalid IL or missing references)
		//IL_0082: Unknown result type (might be due to invalid IL or missing references)
		//IL_0083: Unknown result type (might be due to invalid IL or missing references)
		//IL_0088: Unknown result type (might be due to invalid IL or missing references)
		//IL_008d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0092: Unknown result type (might be due to invalid IL or missing references)
		//IL_0097: Unknown result type (might be due to invalid IL or missing references)
		_bendMode = bend._bendMode;
		_validArc = bend.GetArc(out var arc);
		if (_validArc)
		{
			Vector3d source = bend.EndPoint - bend.BasePoint;
			_pLength = ((Vector3d)(ref source)).Length;
			((Vector3d)(ref source)).Unitize();
			Plane plane = ((Arc)(ref arc)).Plane;
			Vector3d normal = ((Plane)(ref plane)).Normal;
			_pRadius = ((Arc)(ref arc)).Radius;
			_aAngle = ((Arc)(ref arc)).Angle;
			_xDown = CSCoreMath.Orient3Point(bend.BasePoint, source, normal, Point3d.Origin, Vector3d.XAxis, Vector3d.YAxis);
			((Transform)(ref _xDown)).TryGetInverse(ref _xUp);
		}
	}

	public override Point3d MorphPoint(Point3d point)
	{
		//IL_0008: Unknown result type (might be due to invalid IL or missing references)
		//IL_002c: Unknown result type (might be due to invalid IL or missing references)
		//IL_002d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0031: Unknown result type (might be due to invalid IL or missing references)
		//IL_002a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0055: Unknown result type (might be due to invalid IL or missing references)
		//IL_0144: Unknown result type (might be due to invalid IL or missing references)
		//IL_0162: Unknown result type (might be due to invalid IL or missing references)
		//IL_0167: Unknown result type (might be due to invalid IL or missing references)
		//IL_016c: Unknown result type (might be due to invalid IL or missing references)
		//IL_016f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0178: Unknown result type (might be due to invalid IL or missing references)
		//IL_0182: Unknown result type (might be due to invalid IL or missing references)
		//IL_00fe: Unknown result type (might be due to invalid IL or missing references)
		//IL_011b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0120: Unknown result type (might be due to invalid IL or missing references)
		//IL_0125: Unknown result type (might be due to invalid IL or missing references)
		if (!_validArc)
		{
			return point;
		}
		if (Math.Abs(_pLength - 0.0) < 0.0001)
		{
			return point;
		}
		Point3d result = point;
		((Point3d)(ref result)).Transform(_xDown);
		if (_bendMode == 0 && ((Point3d)(ref result)).X < 0.0)
		{
			return point;
		}
		double num = ((Point3d)(ref result)).X / _pLength * _aAngle;
		if (num > _aAngle)
		{
			num = _aAngle;
			((Point3d)(ref result)).X = ((Point3d)(ref result)).X - _pLength;
		}
		else if (num < 0.0 - _aAngle)
		{
			num = 0.0 - _aAngle;
			((Point3d)(ref result)).X = ((Point3d)(ref result)).X + _pLength;
		}
		else
		{
			((Point3d)(ref result)).X = 0.0;
		}
		Transform val = ((!(num < 0.0) || _bendMode != 2) ? Transform.Rotation(num, new Vector3d(0.0, 1.0, 0.0), new Point3d(0.0, 0.0, 0.0 - _pRadius)) : Transform.Rotation(num, new Vector3d(0.0, -1.0, 0.0), new Point3d(0.0, 0.0, _pRadius)));
		((Point3d)(ref result)).Transform(val);
		((Point3d)(ref result)).Transform(_xUp);
		return result;
	}
}
