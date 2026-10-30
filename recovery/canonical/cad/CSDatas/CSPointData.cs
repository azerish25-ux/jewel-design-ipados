using System.Drawing;
using CSDatas.Interfaces;
using CSMaterials;
using Rhino.Display;
using Rhino.Geometry;

namespace CSDatas;

public sealed class CSPointData : CSData, ICSDisplayObject, ICSPoint, ICSTransformable
{
	private Point3d _pointStore;

	public CSPointData(Point3d pointStore)
	{
		_pointStore = pointStore;
	}

	private CSPointData(CSPointData copy)
		: base(copy)
	{
		_pointStore = copy._pointStore;
	}

	public bool Display(DisplayPipeline displayPipeline, CSOptionsDisplay displayOptions, CSDisplayMaterial csDisplayMaterial)
	{
		if (_pointStore == Point3d.Unset)
		{
			return false;
		}
		if (csDisplayMaterial == null)
		{
			csDisplayMaterial = displayOptions.GetCSDisplayMaterialFromName(base.CurrentMaterialGroup.MaterialName);
		}
		Color color = csDisplayMaterial?.Mat.Diffuse ?? Color.White;
		if (displayOptions.DoShowPoints)
		{
			displayPipeline.DrawPoint(_pointStore, PointStyle.ControlPoint, 2, color);
		}
		return true;
	}

	public bool AssignCurrentMaterialGroup(MaterialGroup materialGroup)
	{
		base.CurrentMaterialGroup = materialGroup;
		return true;
	}

	public override bool GetPoint(out Point3d value)
	{
		value = _pointStore;
		return true;
	}

	public override bool HasPoint()
	{
		return true;
	}

	public bool ApplyTransform(Transform transform)
	{
		_pointStore.Transform(transform);
		return true;
	}

	public override CSData Duplicate()
	{
		return new CSPointData(this);
	}

	public override bool GetBoundingBox(ref BoundingBox boundingBox)
	{
		if (_pointStore == Point3d.Unset)
		{
			return false;
		}
		boundingBox = new BoundingBox(_pointStore, _pointStore);
		return true;
	}

	public override bool Morph(SpaceMorph spaceMorph)
	{
		_pointStore = spaceMorph.MorphPoint(_pointStore);
		return true;
	}

	public override string Report()
	{
		if (!(_pointStore == Point3d.Unset))
		{
			return $"{_pointStore.X}:{_pointStore.Y}:{_pointStore.Z}";
		}
		return "Point (empty)";
	}

	~CSPointData()
	{
		Dispose(disposing: false);
	}
}
