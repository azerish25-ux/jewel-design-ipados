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
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_000c: Unknown result type (might be due to invalid IL or missing references)
		//IL_001c: Unknown result type (might be due to invalid IL or missing references)
		//IL_001d: Unknown result type (might be due to invalid IL or missing references)
		//IL_002a: Unknown result type (might be due to invalid IL or missing references)
		//IL_002b: Unknown result type (might be due to invalid IL or missing references)
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
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_0006: Unknown result type (might be due to invalid IL or missing references)
		//IL_002a: Unknown result type (might be due to invalid IL or missing references)
		//IL_002f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0032: Unknown result type (might be due to invalid IL or missing references)
		//IL_0055: Unknown result type (might be due to invalid IL or missing references)
		//IL_0075: Unknown result type (might be due to invalid IL or missing references)
		//IL_0082: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a2: Unknown result type (might be due to invalid IL or missing references)
		//IL_00af: Unknown result type (might be due to invalid IL or missing references)
		//IL_00cf: Unknown result type (might be due to invalid IL or missing references)
		//IL_00dc: Unknown result type (might be due to invalid IL or missing references)
		Transform xform = base.Xform;
		if (!((Transform)(ref xform)).IsValid)
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
		//IL_000f: Unknown result type (might be due to invalid IL or missing references)
		//IL_001c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0028: Unknown result type (might be due to invalid IL or missing references)
		//IL_0035: Unknown result type (might be due to invalid IL or missing references)
		//IL_0041: Unknown result type (might be due to invalid IL or missing references)
		//IL_004e: Unknown result type (might be due to invalid IL or missing references)
		//IL_005e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0065: Unknown result type (might be due to invalid IL or missing references)
		//IL_006c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0075: Unknown result type (might be due to invalid IL or missing references)
		//IL_007a: Unknown result type (might be due to invalid IL or missing references)
		if (((Transform)(ref _Xform)).IsValid)
		{
			Point3d item = default(Point3d);
			((Point3d)(ref item))._002Ector(Vector3d.XAxis);
			((Point3d)(ref item)).Transform(_Xform);
			Point3d item2 = default(Point3d);
			((Point3d)(ref item2))._002Ector(Vector3d.YAxis);
			((Point3d)(ref item2)).Transform(_Xform);
			Point3d item3 = default(Point3d);
			((Point3d)(ref item3))._002Ector(Vector3d.ZAxis);
			((Point3d)(ref item3)).Transform(_Xform);
			List<Point3d> list = new List<Point3d> { item, item2, item3 };
			boundingBox = new BoundingBox((IEnumerable<Point3d>)list);
			return true;
		}
		return base.GetBoundingBox(ref boundingBox);
	}
}
