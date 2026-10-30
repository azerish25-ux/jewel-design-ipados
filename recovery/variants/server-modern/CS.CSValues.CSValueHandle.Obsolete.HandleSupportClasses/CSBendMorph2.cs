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
		//IL_0015: Unknown result type (might be due to invalid IL or missing references)
		//IL_001b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0020: Unknown result type (might be due to invalid IL or missing references)
		//IL_0040: Unknown result type (might be due to invalid IL or missing references)
		//IL_0045: Unknown result type (might be due to invalid IL or missing references)
		//IL_0048: Unknown result type (might be due to invalid IL or missing references)
		//IL_004d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0050: Unknown result type (might be due to invalid IL or missing references)
		//IL_0055: Unknown result type (might be due to invalid IL or missing references)
		//IL_0072: Unknown result type (might be due to invalid IL or missing references)
		//IL_0077: Unknown result type (might be due to invalid IL or missing references)
		//IL_0078: Unknown result type (might be due to invalid IL or missing references)
		//IL_0079: Unknown result type (might be due to invalid IL or missing references)
		//IL_007e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0083: Unknown result type (might be due to invalid IL or missing references)
		//IL_0088: Unknown result type (might be due to invalid IL or missing references)
		//IL_008d: Unknown result type (might be due to invalid IL or missing references)
		_bendMode = bend._bendMode;
		Vector3d source = default(Vector3d);
		((Vector3d)(ref source))._002Ector(bend.EndPoint - bend.BasePoint);
		_pLength = ((Vector3d)(ref source)).Length;
		((Vector3d)(ref source)).Unitize();
		Arc arc = bend.GetArc();
		Plane plane = ((Arc)(ref arc)).Plane;
		Vector3d normal = ((Plane)(ref plane)).Normal;
		_pRadius = ((Arc)(ref arc)).Radius;
		_aAngle = ((Arc)(ref arc)).Angle;
		_xDown = CSCoreMath.Orient3Point(bend.BasePoint, source, normal, Point3d.Origin, Vector3d.XAxis, Vector3d.YAxis);
		((Transform)(ref _xDown)).TryGetInverse(ref _xUp);
	}

	public override Point3d MorphPoint(Point3d point)
	{
		//IL_0013: Unknown result type (might be due to invalid IL or missing references)
		//IL_0014: Unknown result type (might be due to invalid IL or missing references)
		//IL_0018: Unknown result type (might be due to invalid IL or missing references)
		//IL_0011: Unknown result type (might be due to invalid IL or missing references)
		//IL_0040: Unknown result type (might be due to invalid IL or missing references)
		//IL_003c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0133: Unknown result type (might be due to invalid IL or missing references)
		//IL_0151: Unknown result type (might be due to invalid IL or missing references)
		//IL_0156: Unknown result type (might be due to invalid IL or missing references)
		//IL_015b: Unknown result type (might be due to invalid IL or missing references)
		//IL_015e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0167: Unknown result type (might be due to invalid IL or missing references)
		//IL_0171: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ed: Unknown result type (might be due to invalid IL or missing references)
		//IL_010a: Unknown result type (might be due to invalid IL or missing references)
		//IL_010f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0114: Unknown result type (might be due to invalid IL or missing references)
		if (_pLength == 0.0)
		{
			return point;
		}
		Point3d result = point;
		((Point3d)(ref result)).Transform(_xDown);
		if (_bendMode == 0 && ((Point3d)(ref result)).X < 0.0)
		{
			return point;
		}
		Transform val = default(Transform);
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
		val = ((!(num < 0.0) || _bendMode != 2) ? Transform.Rotation(num, new Vector3d(0.0, 1.0, 0.0), new Point3d(0.0, 0.0, 0.0 - _pRadius)) : Transform.Rotation(num, new Vector3d(0.0, -1.0, 0.0), new Point3d(0.0, 0.0, _pRadius)));
		((Point3d)(ref result)).Transform(val);
		((Point3d)(ref result)).Transform(_xUp);
		return result;
	}
}
