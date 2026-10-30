using System;
using System.Collections.Concurrent;
using CSCompatibility;
using CSDatas.Interfaces;
using CSMaterials;
using CSRingSizes.RingSize;
using Rhino;
using Rhino.DocObjects;
using Rhino.Geometry;

namespace CSDatas;

public sealed class CSRingRailData : CSRailData
{
	public double Radius { get; }

	public CSRingRailData(Curve crv, double radius)
		: base(crv)
	{
		Radius = radius;
	}

	private CSRingRailData(CSRingRailData copy)
		: base(copy)
	{
		Radius = copy.Radius;
		if (copy._Crv == null)
		{
			return;
		}
		_Crv = copy._Crv.ToNurbsCurve();
		string[] allKeys = copy._Crv.GetUserStrings().AllKeys;
		foreach (string key in allKeys)
		{
			string userString = copy._Crv.GetUserString(key);
			if (!string.IsNullOrEmpty(userString))
			{
				_Crv.SetUserString(key, userString);
			}
		}
	}

	public override bool AddToDocumentExport(ref ConcurrentBag<RhinoRenderObject> rhinoRenderObjects, Transform transform, CSOptionsExport exportOptions)
	{
		if (transform != Transform.Unset && transform != Transform.Identity)
		{
			CSData cSData = Duplicate();
			(cSData as ICSTransformable)?.ApplyTransform(transform);
			if (cSData is ICSDocumentObject iCSDocumentObject)
			{
				return iCSDocumentObject.AddToDocumentExport(ref rhinoRenderObjects, Transform.Unset, exportOptions);
			}
		}
		if (_Crv == null)
		{
			return false;
		}
		UniversalRingSize sizeRadius = exportOptions.RingSize.GetSizeRadius(Radius);
		ObjectAttributes attributes = new ObjectAttributes
		{
			MaterialSource = ObjectMaterialSource.MaterialFromObject,
			Name = $"Ring Rail {sizeRadius.RingSizeTypeAbrv} {sizeRadius.Size}"
		};
		Guid guid = RhinoDoc.ActiveDoc.Objects.AddCurve(_Crv, attributes);
		if (guid == Guid.Empty)
		{
			return true;
		}
		RhinoScriptInterface.SetObjectData(guid, "MatrixRing", "RingRegion", sizeRadius.ringSizeType);
		RhinoScriptInterface.SetObjectData(guid, "MatrixRing", "RingSize", sizeRadius.Size);
		RhinoScriptInterface.SetObjectData(guid, "MatrixRing", "Radius", sizeRadius.radiusMM);
		rhinoRenderObjects.Add(new RhinoRenderObject(guid, base.CurrentMaterialGroup));
		return true;
	}

	public override CSData Duplicate()
	{
		return new CSRingRailData(this);
	}

	public override string Report()
	{
		return "Radius " + Radius;
	}
}
