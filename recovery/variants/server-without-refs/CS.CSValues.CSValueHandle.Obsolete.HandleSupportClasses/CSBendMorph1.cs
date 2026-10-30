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
		//IL_0012: Unknown result type (might be due to invalid IL or missing references)
		//IL_0013: Unknown result type (might be due to invalid IL or missing references)
		//IL_0014: Unknown result type (might be due to invalid IL or missing references)
		//IL_0035: Unknown result type (might be due to invalid IL or missing references)
		//IL_003a: Unknown result type (might be due to invalid IL or missing references)
		//IL_003d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0042: Unknown result type (might be due to invalid IL or missing references)
		//IL_005e: Unknown result type (might be due to invalid IL or missing references)
		//IL_005f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0060: Unknown result type (might be due to invalid IL or missing references)
		//IL_007c: Unknown result type (might be due to invalid IL or missing references)
		//IL_009c: Unknown result type (might be due to invalid IL or missing references)
		//IL_00bc: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c1: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c6: Unknown result type (might be due to invalid IL or missing references)
		if (((Arc)(ref arc)).IsValid)
		{
			Vector3d source = default(Vector3d);
			((Vector3d)(ref source))._002Ector(p1 - p0);
			_pLength = ((Vector3d)(ref source)).Length;
			((Vector3d)(ref source)).Unitize();
			Plane plane = ((Arc)(ref arc)).Plane;
			Vector3d normal = ((Plane)(ref plane)).Normal;
			_pRadius = ((Arc)(ref arc)).Radius;
			_aAngle = ((Arc)(ref arc)).Angle;
			_xDown = CSCoreMath.Orient3Point(p0, source, normal, new Point3d(0.0, 0.0, 0.0), new Vector3d(1.0, 0.0, 0.0), new Vector3d(0.0, 1.0, 0.0));
			((Transform)(ref _xDown)).TryGetInverse(ref _xUp);
		}
	}

	public override Point3d MorphPoint(Point3d point)
	{
		//IL_0013: Unknown result type (might be due to invalid IL or missing references)
		//IL_0014: Unknown result type (might be due to invalid IL or missing references)
		//IL_0018: Unknown result type (might be due to invalid IL or missing references)
		//IL_0062: Unknown result type (might be due to invalid IL or missing references)
		//IL_0080: Unknown result type (might be due to invalid IL or missing references)
		//IL_0085: Unknown result type (might be due to invalid IL or missing references)
		//IL_008a: Unknown result type (might be due to invalid IL or missing references)
		//IL_008d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0096: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a0: Unknown result type (might be due to invalid IL or missing references)
		//IL_0011: Unknown result type (might be due to invalid IL or missing references)
		if (_pLength == 0.0)
		{
			return point;
		}
		Point3d result = point;
		((Point3d)(ref result)).Transform(_xDown);
		double num = ((Point3d)(ref result)).X / _pLength * _aAngle;
		((Point3d)(ref result)).X = 0.0;
		Transform val = Transform.Rotation(num, new Vector3d(0.0, 1.0, 0.0), new Point3d(0.0, 0.0, 0.0 - _pRadius));
		((Point3d)(ref result)).Transform(val);
		((Point3d)(ref result)).Transform(_xUp);
		return result;
	}
}
