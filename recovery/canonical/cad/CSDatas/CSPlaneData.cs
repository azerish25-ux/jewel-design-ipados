using System.Collections.Generic;
using System.Drawing;
using CSMaterials;
using Rhino.ApplicationSettings;
using Rhino.Display;
using Rhino.Geometry;

namespace CSDatas;

public sealed class CSPlaneData : CSXformData
{
	public CSPlaneData(CSXformData copy)
		: base(copy)
	{
	}

	public override bool ApplyTransform(Transform transform)
	{
		if (!base.ApplyTransform(transform))
		{
			return false;
		}
		if (!CSUtilMath.GetUnscaledXform(base.Xform, out var xUp))
		{
			return true;
		}
		if (xUp != Transform.Unset)
		{
			_Xform = xUp;
		}
		return true;
	}

	public override bool AssignCurrentMaterialGroup(MaterialGroup materialGroup)
	{
		base.CurrentMaterialGroup = materialGroup;
		return true;
	}

	public override bool Display(DisplayPipeline displayPipeline, CSOptionsDisplay displayOptions, CSDisplayMaterial csDisplayMaterial = null)
	{
		if (!base.Xform.IsValid)
		{
			return true;
		}
		if (csDisplayMaterial == null)
		{
			csDisplayMaterial = displayOptions.GetCSDisplayMaterialFromName(base.CurrentMaterialGroup.MaterialName);
		}
		Transform modelTransform = displayPipeline.ModelTransform;
		displayPipeline.ModelTransform = base.Xform;
		Color diffuse = csDisplayMaterial.Mat.Diffuse;
		Color worldCoordIconYAxisColor = AppearanceSettings.WorldCoordIconYAxisColor;
		Color worldCoordIconZAxisColor = AppearanceSettings.WorldCoordIconZAxisColor;
		displayPipeline.DrawLine(Point3d.Origin, new Point3d(1.0, 0.0, 0.0), diffuse, 1);
		displayPipeline.DrawLine(Point3d.Origin, new Point3d(0.0, 1.0, 0.0), worldCoordIconYAxisColor, 1);
		displayPipeline.DrawLine(Point3d.Origin, new Point3d(0.0, 0.0, 1.0), worldCoordIconZAxisColor, 1);
		displayPipeline.ModelTransform = modelTransform;
		return true;
	}

	public override CSData Duplicate()
	{
		return new CSPlaneData(this);
	}

	public override bool GetBoundingBox(ref BoundingBox boundingBox)
	{
		if (_Xform.IsValid)
		{
			Point3d item = new Point3d(Vector3d.XAxis);
			item.Transform(_Xform);
			Point3d item2 = new Point3d(Vector3d.YAxis);
			item2.Transform(_Xform);
			Point3d item3 = new Point3d(Vector3d.ZAxis);
			item3.Transform(_Xform);
			List<Point3d> points = new List<Point3d> { item, item2, item3 };
			boundingBox = new BoundingBox(points);
			return true;
		}
		return base.GetBoundingBox(ref boundingBox);
	}
}
