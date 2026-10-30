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
		//IL_0007: Unknown result type (might be due to invalid IL or missing references)
		//IL_0008: Unknown result type (might be due to invalid IL or missing references)
		_pointStore = pointStore;
	}

	private CSPointData(CSPointData copy)
		: base(copy)
	{
		//IL_0009: Unknown result type (might be due to invalid IL or missing references)
		//IL_000e: Unknown result type (might be due to invalid IL or missing references)
		_pointStore = copy._pointStore;
	}

	public bool Display(DisplayPipeline displayPipeline, CSOptionsDisplay displayOptions, CSDisplayMaterial csDisplayMaterial)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_0006: Unknown result type (might be due to invalid IL or missing references)
		//IL_004a: Unknown result type (might be due to invalid IL or missing references)
		if (_pointStore == Point3d.Unset)
		{
			return false;
		}
		if (csDisplayMaterial == null)
		{
			csDisplayMaterial = displayOptions.GetCSDisplayMaterialFromName(base.CurrentMaterialGroup.MaterialName);
		}
		Color color = ((csDisplayMaterial != null) ? csDisplayMaterial.Mat.Diffuse : Color.White);
		if (displayOptions.DoShowPoints)
		{
			displayPipeline.DrawPoint(_pointStore, (PointStyle)1, 2, color);
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
		//IL_0002: Unknown result type (might be due to invalid IL or missing references)
		//IL_0007: Unknown result type (might be due to invalid IL or missing references)
		value = _pointStore;
		return true;
	}

	public override bool HasPoint()
	{
		return true;
	}

	public bool ApplyTransform(Transform transform)
	{
		//IL_0006: Unknown result type (might be due to invalid IL or missing references)
		((Point3d)(ref _pointStore)).Transform(transform);
		return true;
	}

	public override CSData Duplicate()
	{
		return new CSPointData(this);
	}

	public override bool GetBoundingBox(ref BoundingBox boundingBox)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_0006: Unknown result type (might be due to invalid IL or missing references)
		//IL_0016: Unknown result type (might be due to invalid IL or missing references)
		//IL_001c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0021: Unknown result type (might be due to invalid IL or missing references)
		//IL_0026: Unknown result type (might be due to invalid IL or missing references)
		if (_pointStore == Point3d.Unset)
		{
			return false;
		}
		boundingBox = new BoundingBox(_pointStore, _pointStore);
		return true;
	}

	public override bool Morph(SpaceMorph spaceMorph)
	{
		//IL_0003: Unknown result type (might be due to invalid IL or missing references)
		//IL_0008: Unknown result type (might be due to invalid IL or missing references)
		//IL_000d: Unknown result type (might be due to invalid IL or missing references)
		_pointStore = spaceMorph.MorphPoint(_pointStore);
		return true;
	}

	public override string Report()
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_0006: Unknown result type (might be due to invalid IL or missing references)
		if (!(_pointStore == Point3d.Unset))
		{
			return $"{((Point3d)(ref _pointStore)).X}:{((Point3d)(ref _pointStore)).Y}:{((Point3d)(ref _pointStore)).Z}";
		}
		return "Point (empty)";
	}

	~CSPointData()
	{
		Dispose(disposing: false);
	}
}
