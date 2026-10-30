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
		: base((CSRailData)copy)
	{
		Radius = copy.Radius;
		if (copy._Crv == null)
		{
			return;
		}
		_Crv = ((Curve)copy._Crv).ToNurbsCurve();
		string[] allKeys = ((GeometryBase)copy._Crv).GetUserStrings().AllKeys;
		foreach (string text in allKeys)
		{
			string userString = ((GeometryBase)copy._Crv).GetUserString(text);
			if (!string.IsNullOrEmpty(userString))
			{
				((GeometryBase)_Crv).SetUserString(text, userString);
			}
		}
	}

	public override bool AddToDocumentExport(ref ConcurrentBag<RhinoRenderObject> rhinoRenderObjects, Transform transform, CSOptionsExport exportOptions)
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_000d: Unknown result type (might be due to invalid IL or missing references)
		//IL_000e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0066: Unknown result type (might be due to invalid IL or missing references)
		//IL_006b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0072: Unknown result type (might be due to invalid IL or missing references)
		//IL_008f: Expected O, but got Unknown
		//IL_002c: Unknown result type (might be due to invalid IL or missing references)
		//IL_003e: Unknown result type (might be due to invalid IL or missing references)
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
		ObjectAttributes val = new ObjectAttributes
		{
			MaterialSource = (ObjectMaterialSource)1,
			Name = $"Ring Rail {sizeRadius.RingSizeTypeAbrv} {sizeRadius.Size}"
		};
		Guid guid = RhinoDoc.ActiveDoc.Objects.AddCurve((Curve)(object)_Crv, val);
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
