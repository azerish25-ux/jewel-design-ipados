using CSBlockUtil;
using Rhino.Geometry;

namespace CS.CSValues.CSValueHandle;

public abstract class CSValueXFormBase : CSValueBase
{
	protected Transform _value;

	protected CSValueXFormBase(string identifier)
		: base(identifier)
	{
		//IL_0008: Unknown result type (might be due to invalid IL or missing references)
		//IL_000d: Unknown result type (might be due to invalid IL or missing references)
		_value = Transform.Identity;
	}

	protected CSValueXFormBase(string identifier, Transform value)
		: base(identifier)
	{
		//IL_0008: Unknown result type (might be due to invalid IL or missing references)
		//IL_0009: Unknown result type (might be due to invalid IL or missing references)
		_value = value;
	}

	public bool ApplyTransform(Transform transform)
	{
		//IL_0017: Unknown result type (might be due to invalid IL or missing references)
		//IL_0019: Unknown result type (might be due to invalid IL or missing references)
		//IL_001e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0023: Unknown result type (might be due to invalid IL or missing references)
		if (!((Transform)(ref transform)).IsValid)
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
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0005: Unknown result type (might be due to invalid IL or missing references)
		//IL_0009: Unknown result type (might be due to invalid IL or missing references)
		//IL_0013: Unknown result type (might be due to invalid IL or missing references)
		//IL_0018: Unknown result type (might be due to invalid IL or missing references)
		//IL_001c: Unknown result type (might be due to invalid IL or missing references)
		//IL_002e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0033: Unknown result type (might be due to invalid IL or missing references)
		//IL_0037: Unknown result type (might be due to invalid IL or missing references)
		//IL_0049: Unknown result type (might be due to invalid IL or missing references)
		//IL_004e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0052: Unknown result type (might be due to invalid IL or missing references)
		//IL_0064: Unknown result type (might be due to invalid IL or missing references)
		//IL_0065: Unknown result type (might be due to invalid IL or missing references)
		//IL_0066: Unknown result type (might be due to invalid IL or missing references)
		//IL_0067: Unknown result type (might be due to invalid IL or missing references)
		//IL_0068: Unknown result type (might be due to invalid IL or missing references)
		Point3d origin = Point3d.Origin;
		((Point3d)(ref origin)).Transform(_value);
		Vector3d xAxis = Vector3d.XAxis;
		((Vector3d)(ref xAxis)).Transform(_value);
		((Vector3d)(ref xAxis)).Unitize();
		Vector3d yAxis = Vector3d.YAxis;
		((Vector3d)(ref yAxis)).Transform(_value);
		((Vector3d)(ref yAxis)).Unitize();
		Vector3d zAxis = Vector3d.ZAxis;
		((Vector3d)(ref zAxis)).Transform(_value);
		((Vector3d)(ref zAxis)).Unitize();
		return CSUtilMath.Transform3PointToOrigin(origin, xAxis, yAxis, zAxis);
	}

	public void Reset()
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_0006: Unknown result type (might be due to invalid IL or missing references)
		//IL_0008: Unknown result type (might be due to invalid IL or missing references)
		Transform resetTransform = GetResetTransform();
		ApplyTransform(resetTransform);
	}
}
