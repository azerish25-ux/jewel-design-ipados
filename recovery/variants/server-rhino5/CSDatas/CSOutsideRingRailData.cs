using System;
using System.Collections.Concurrent;
using CSDatas.Interfaces;
using CSMaterials;
using Rhino;
using Rhino.DocObjects;
using Rhino.Geometry;

namespace CSDatas;

public sealed class CSOutsideRingRailData : CSRailData
{
	public double Scale { get; }

	public CSOutsideRingRailData(Curve newCrv)
		: base(newCrv)
	{
		Scale = 1.0;
	}

	public CSOutsideRingRailData(Curve crv, double newScale)
		: base(crv)
	{
		Scale = newScale;
	}

	public CSOutsideRingRailData(CSOutsideRingRailData copy)
		: base(copy)
	{
		Scale = copy.Scale;
	}

	public override bool AddToDocumentExport(ref ConcurrentBag<RhinoRenderObject> rhinoRenderObjects, Transform transform, CSOptionsExport exportOptions)
	{
		if (transform != Transform.Unset && transform != Transform.Identity)
		{
			CSData cSData = Duplicate();
			if (cSData is ICSTransformable iCSTransformable)
			{
				iCSTransformable.ApplyTransform(transform);
			}
			if (cSData is ICSDocumentObject iCSDocumentObject)
			{
				return iCSDocumentObject.AddToDocumentExport(ref rhinoRenderObjects, Transform.Unset, exportOptions);
			}
		}
		if (_Crv == null)
		{
			return false;
		}
		ObjectAttributes attributes = new ObjectAttributes
		{
			MaterialSource = ObjectMaterialSource.MaterialFromObject,
			Name = "Outside Ring Rail"
		};
		Guid guid = RhinoDoc.ActiveDoc.Objects.AddCurve(_Crv, attributes);
		if (guid != Guid.Empty)
		{
			rhinoRenderObjects.Add(new RhinoRenderObject(guid, base.CurrentMaterialGroup));
		}
		return true;
	}

	public override CSData Duplicate()
	{
		return new CSOutsideRingRailData(this);
	}

	public override string Report()
	{
		return "Outside Rail";
	}
}
