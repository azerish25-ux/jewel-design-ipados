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
		: base((CSRailData)copy)
	{
		Scale = copy.Scale;
	}

	public override bool AddToDocumentExport(ref ConcurrentBag<RhinoRenderObject> rhinoRenderObjects, Transform transform, CSOptionsExport exportOptions)
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_000d: Unknown result type (might be due to invalid IL or missing references)
		//IL_000e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0053: Unknown result type (might be due to invalid IL or missing references)
		//IL_0058: Unknown result type (might be due to invalid IL or missing references)
		//IL_005f: Unknown result type (might be due to invalid IL or missing references)
		//IL_006b: Expected O, but got Unknown
		//IL_002b: Unknown result type (might be due to invalid IL or missing references)
		//IL_003d: Unknown result type (might be due to invalid IL or missing references)
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
		ObjectAttributes val = new ObjectAttributes
		{
			MaterialSource = (ObjectMaterialSource)1,
			Name = "Outside Ring Rail"
		};
		Guid guid = RhinoDoc.ActiveDoc.Objects.AddCurve((Curve)(object)_Crv, val);
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
