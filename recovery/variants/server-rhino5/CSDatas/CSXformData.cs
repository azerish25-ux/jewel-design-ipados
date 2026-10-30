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
		_Xform = Transform.Identity;
	}

	public CSXformData(Transform transform)
	{
		_Xform = ((transform != Transform.Unset) ? transform : Transform.Identity);
	}

	protected CSXformData(CSXformData copy)
		: base(copy)
	{
		_Xform = ((copy._Xform != Transform.Unset) ? copy.Xform : Transform.Identity);
	}

	public virtual bool Display(DisplayPipeline displayPipeline, CSOptionsDisplay displayOptions, CSDisplayMaterial overrideMaterial = null)
	{
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
		origin.Transform(xform);
		Point3d to = new Point3d(Vector3d.XAxis);
		to.Transform(xform);
		Point3d to2 = new Point3d(Vector3d.YAxis);
		to2.Transform(xform);
		Point3d to3 = new Point3d(Vector3d.ZAxis);
		to3.Transform(xform);
		displayPipeline.DrawPoint(origin, PointStyle.ControlPoint, 2, Color.White);
		displayPipeline.DrawLine(origin, to, Color.FromArgb(255, 0, 0));
		displayPipeline.DrawLine(origin, to2, Color.FromArgb(0, 255, 0));
		displayPipeline.DrawLine(origin, to3, Color.FromArgb(0, 0, 255));
		return true;
	}

	public virtual bool AssignCurrentMaterialGroup(MaterialGroup materialGroup)
	{
		return true;
	}

	public override bool GetPoint(out Point3d value)
	{
		value = Point3d.Origin;
		if (_Xform == Transform.Unset)
		{
			return false;
		}
		value.Transform(_Xform);
		return true;
	}

	public bool GetTransform(out Transform value)
	{
		value = _Xform;
		return value.IsValid;
	}

	public virtual bool ApplyTransform(Transform transform)
	{
		if (!transform.IsValid)
		{
			return false;
		}
		_Xform = (_Xform.IsValid ? (_Xform * transform) : transform);
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
