using CSBlockUtil;
using Rhino.Geometry;

namespace CS.CSValues.CSValueHandle;

public abstract class CSValueXFormBase : CSValueBase
{
	protected Transform _value;

	protected CSValueXFormBase(string identifier)
		: base(identifier)
	{
		_value = Transform.Identity;
	}

	protected CSValueXFormBase(string identifier, Transform value)
		: base(identifier)
	{
		_value = value;
	}

	public bool ApplyTransform(Transform transform)
	{
		if (!transform.IsValid)
		{
			return false;
		}
		PreChange("ApplyTransform");
		_value = transform * _value;
		Change();
		return true;
	}

	public Transform GetResetTransform()
	{
		Point3d origin = Point3d.Origin;
		origin.Transform(_value);
		Vector3d xAxis = Vector3d.XAxis;
		xAxis.Transform(_value);
		xAxis.Unitize();
		Vector3d yAxis = Vector3d.YAxis;
		yAxis.Transform(_value);
		yAxis.Unitize();
		Vector3d zAxis = Vector3d.ZAxis;
		zAxis.Transform(_value);
		zAxis.Unitize();
		return CSUtilMath.Transform3PointToOrigin(origin, xAxis, yAxis, zAxis);
	}

	public void Reset()
	{
		Transform resetTransform = GetResetTransform();
		ApplyTransform(resetTransform);
	}
}
