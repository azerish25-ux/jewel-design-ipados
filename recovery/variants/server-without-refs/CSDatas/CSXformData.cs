using System.Drawing;
using CSDatas.Interfaces;
using CSMaterials;
using Rhino.Display;
using Rhino.Geometry;

namespace CSDatas;

public class CSXformData : CSData, ICSDisplayObject, ICSPoint, ICSTransform, ICSTransformable
{
	protected Transform _Xform;

	public Transform Xform => _Xform;

	public CSXformData()
	{
		//IL_0007: Unknown result type (might be due to invalid IL or missing references)
		//IL_000c: Unknown result type (might be due to invalid IL or missing references)
		_Xform = Transform.Identity;
	}

	public CSXformData(Transform transform)
	{
		//IL_0007: Unknown result type (might be due to invalid IL or missing references)
		//IL_0008: Unknown result type (might be due to invalid IL or missing references)
		//IL_001b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0014: Unknown result type (might be due to invalid IL or missing references)
		//IL_001c: Unknown result type (might be due to invalid IL or missing references)
		_Xform = ((transform != Transform.Unset) ? transform : Transform.Identity);
	}

	protected CSXformData(CSXformData copy)
		: base(copy)
	{
		//IL_0009: Unknown result type (might be due to invalid IL or missing references)
		//IL_000e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0022: Unknown result type (might be due to invalid IL or missing references)
		//IL_001a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0027: Unknown result type (might be due to invalid IL or missing references)
		_Xform = ((copy._Xform != Transform.Unset) ? copy.Xform : Transform.Identity);
	}

	public virtual bool Display(DisplayPipeline displayPipeline, CSOptionsDisplay displayOptions, CSDisplayMaterial overrideMaterial = null)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_0006: Unknown result type (might be due to invalid IL or missing references)
		//IL_001f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0024: Unknown result type (might be due to invalid IL or missing references)
		//IL_0025: Unknown result type (might be due to invalid IL or missing references)
		//IL_002a: Unknown result type (might be due to invalid IL or missing references)
		//IL_002d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0035: Unknown result type (might be due to invalid IL or missing references)
		//IL_0041: Unknown result type (might be due to invalid IL or missing references)
		//IL_0049: Unknown result type (might be due to invalid IL or missing references)
		//IL_0055: Unknown result type (might be due to invalid IL or missing references)
		//IL_005d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0069: Unknown result type (might be due to invalid IL or missing references)
		//IL_0070: Unknown result type (might be due to invalid IL or missing references)
		//IL_007e: Unknown result type (might be due to invalid IL or missing references)
		//IL_007f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0092: Unknown result type (might be due to invalid IL or missing references)
		//IL_0093: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a6: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a7: Unknown result type (might be due to invalid IL or missing references)
		if (_Xform == Transform.Unset)
		{
			return false;
		}
		if (!displayOptions.DoShowPoints)
		{
			return true;
		}
		Transform xform = _Xform;
		Point3d origin = Point3d.Origin;
		((Point3d)(ref origin)).Transform(xform);
		Point3d val = default(Point3d);
		((Point3d)(ref val))._002Ector(Vector3d.XAxis);
		((Point3d)(ref val)).Transform(xform);
		Point3d val2 = default(Point3d);
		((Point3d)(ref val2))._002Ector(Vector3d.YAxis);
		((Point3d)(ref val2)).Transform(xform);
		Point3d val3 = default(Point3d);
		((Point3d)(ref val3))._002Ector(Vector3d.ZAxis);
		((Point3d)(ref val3)).Transform(xform);
		displayPipeline.DrawPoint(origin, (PointStyle)1, 2, Color.White);
		displayPipeline.DrawLine(origin, val, Color.FromArgb(255, 0, 0));
		displayPipeline.DrawLine(origin, val2, Color.FromArgb(0, 255, 0));
		displayPipeline.DrawLine(origin, val3, Color.FromArgb(0, 0, 255));
		return true;
	}

	public virtual bool AssignCurrentMaterialGroup(MaterialGroup materialGroup)
	{
		return true;
	}

	public override bool GetPoint(out Point3d value)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_0006: Unknown result type (might be due to invalid IL or missing references)
		//IL_000c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0011: Unknown result type (might be due to invalid IL or missing references)
		//IL_0021: Unknown result type (might be due to invalid IL or missing references)
		value = Point3d.Origin;
		if (_Xform == Transform.Unset)
		{
			return false;
		}
		((Point3d)(ref value)).Transform(_Xform);
		return true;
	}

	public bool GetTransform(out Transform value)
	{
		//IL_0002: Unknown result type (might be due to invalid IL or missing references)
		//IL_0007: Unknown result type (might be due to invalid IL or missing references)
		value = _Xform;
		return ((Transform)(ref value)).IsValid;
	}

	public virtual bool ApplyTransform(Transform transform)
	{
		//IL_001d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0022: Unknown result type (might be due to invalid IL or missing references)
		//IL_0023: Unknown result type (might be due to invalid IL or missing references)
		//IL_0019: Unknown result type (might be due to invalid IL or missing references)
		//IL_0028: Unknown result type (might be due to invalid IL or missing references)
		if (!((Transform)(ref transform)).IsValid)
		{
			return false;
		}
		_Xform = (((Transform)(ref _Xform)).IsValid ? (_Xform * transform) : transform);
		return true;
	}

	public override CSData Duplicate()
	{
		return new CSXformData(this);
	}

	~CSXformData()
	{
		Dispose(disposing: false);
	}
}
