using System.Collections.Generic;
using CS;
using CS.CSObjects.CSBlocks;
using CS.Parametric;
using CS.Parametric.Channels;
using CSCompatibility;
using CSDatas;
using RMA.OpenNURBS;
using RMA.Rhino;
using Rhino;
using Rhino.DocObjects;
using Rhino.Geometry;

namespace CSBlocks.Core;

[CSBlock("RhinoObjectsFromBoundingBox", "{16358839-FDC6-44c0-A62E-B2E872A76697}")]
public sealed class CSRhinoObjectsFromBoundingBox : CSBlock, ICSBlockObjectWatcher
{
	private readonly List<GeometryBase> _objects = new List<GeometryBase>();

	private bool _useObjects;

	public CSInputChannel iBoundingBox { get; }

	public CSInputChannel iGrowBox { get; }

	public CSOutputChannel oObjects { get; }

	public CSRhinoObjectsFromBoundingBox(string identifier)
		: base(identifier)
	{
		iBoundingBox = new CSInputChannel(GetPropertyName(() => iBoundingBox), this, ChannelDataType.Any, ChannelArrayType.Individual);
		iGrowBox = new CSInputChannel(GetPropertyName(() => iGrowBox), this, ChannelDataType.Double, ChannelArrayType.Individual);
		oObjects = new CSOutputChannel(GetPropertyName(() => oObjects), this, ChannelDataType.Any, ChannelArrayType.Array);
	}

	public bool CheckForChangeOnDelete(MRhinoObject obj)
	{
		return CheckForChangeOnAdd(obj);
	}

	public bool CheckForChangeOnAdd(MRhinoObject obj)
	{
		if (_useObjects)
		{
			return false;
		}
		if (iBoundingBox == null)
		{
			return false;
		}
		if (iBoundingBox.Data == null)
		{
			return false;
		}
		BoundingBox boundingBox = BoundingBox.Unset;
		iBoundingBox.Data.GetBoundingBox(ref boundingBox);
		if (iGrowBox != null)
		{
			double value = 0.0;
			iGrowBox.GetDouble(ref value);
			boundingBox.Max += new Vector3d(value, value, value);
			boundingBox.Min -= new Vector3d(value, value, value);
		}
		Point3d pt = new Point3d(boundingBox.Min.X, boundingBox.Min.Y, boundingBox.Min.Z - 1000.0);
		Point3d pt2 = new Point3d(boundingBox.Max.X, boundingBox.Max.Y, boundingBox.Max.Z + 1000.0);
		OnBoundingBox onBoundingBox = new OnBoundingBox(pt.Convert(), pt2.Convert());
		OnBoundingBox tight_box = new OnBoundingBox();
		obj.GetTightBoundingBox(ref tight_box);
		if (tight_box.Min().DistanceTo(boundingBox.Min.Convert()) < 0.1 && tight_box.Max().DistanceTo(boundingBox.Max.Convert()) < 0.1)
		{
			Change();
			return true;
		}
		if (!onBoundingBox.Includes(tight_box))
		{
			return false;
		}
		Change();
		return true;
	}

	public bool CheckForChangeOnReplace(MRhinoObject oldObject, MRhinoObject obj)
	{
		return CheckForChangeOnAdd(oldObject);
	}

	public override bool Read(ref OnBinaryArchive archive, CSParaGraph graph, int major, int minor)
	{
		base.Read(ref archive, graph, major, minor);
		uint tcode = 0u;
		int value = 0;
		if (!archive.BeginRead3dmChunk(ref tcode, ref value))
		{
			return false;
		}
		_objects.Clear();
		_useObjects = true;
		On3dmObjectAttributes attributes = new On3dmObjectAttributes();
		if (tcode == 268435475)
		{
			OnObject @object;
			while (archive.Read3dmObject(out @object, ref attributes, 0u) != 0 && @object != null)
			{
				_objects.Add(@object.Convert());
			}
		}
		archive.EndRead3dmChunk();
		return true;
	}

	public override bool Update()
	{
		CSDataArray cSDataArray = new CSDataArray();
		if (_useObjects)
		{
			foreach (GeometryBase @object in _objects)
			{
				cSDataArray.Append(new CSOnGeometryData(@object));
			}
		}
		else
		{
			if (iBoundingBox == null)
			{
				return UpdateMissing("BoundingBox");
			}
			if (iBoundingBox.Data == null)
			{
				return UpdateError("BoundingBox Missing");
			}
			BoundingBox boundingBox = BoundingBox.Unset;
			iBoundingBox.Data.GetBoundingBox(ref boundingBox);
			if (iGrowBox != null)
			{
				double value = 0.0;
				iGrowBox.GetDouble(ref value);
				boundingBox.Max += new Vector3d(value, value, value);
				boundingBox.Min -= new Vector3d(value, value, value);
			}
			Point3d min = new Point3d(boundingBox.Min.X, boundingBox.Min.Y, boundingBox.Min.Z - 1000.0);
			Point3d max = new Point3d(boundingBox.Max.X, boundingBox.Max.Y, boundingBox.Max.Z + 1000.0);
			BoundingBox boundingBox2 = new BoundingBox(min, max);
			foreach (RhinoObject selectedObject in RhinoDoc.ActiveDoc.Objects.GetSelectedObjects(includeLights: false, includeGrips: false))
			{
				if (selectedObject.Geometry == null)
				{
					continue;
				}
				BoundingBox boundingBox3 = selectedObject.Geometry.GetBoundingBox(selectedObject.Geometry is Curve);
				if ((boundingBox3.Min.CompareTo(boundingBox.Min) != 0 || boundingBox3.Max.CompareTo(boundingBox.Max) != 0) && boundingBox2.Contains(boundingBox3))
				{
					GeometryBase geometryBase = selectedObject.Geometry.Duplicate();
					if (selectedObject.Attributes.Name.Length > 0)
					{
						geometryBase.SetUserString("CSName", selectedObject.Attributes.Name);
					}
					if (selectedObject.Attributes.LayerIndex >= 0 && selectedObject.Attributes.LayerIndex < RhinoDoc.ActiveDoc.Layers.Count)
					{
						string name = RhinoDoc.ActiveDoc.Layers[selectedObject.Attributes.LayerIndex].Name;
						geometryBase.SetUserString("CSLayer", name);
					}
					if (geometryBase != null)
					{
						cSDataArray.Append(new CSOnGeometryData(geometryBase));
					}
				}
			}
		}
		oObjects.SetData(cSDataArray);
		return base.Update();
	}

	public override bool Write(ref OnBinaryArchive archive)
	{
		base.Write(ref archive);
		archive.BeginWrite3dmChunk(268435475u, 0);
		if (_useObjects)
		{
			foreach (GeometryBase @object in _objects)
			{
				On3dmObjectAttributes attributes = new On3dmObjectAttributes();
				archive.Write3dmObject(@object.Convert(), attributes);
			}
		}
		else
		{
			if (iBoundingBox == null)
			{
				return false;
			}
			if (iBoundingBox.Data == null)
			{
				return false;
			}
			BoundingBox boundingBox = BoundingBox.Unset;
			iBoundingBox.Data.GetBoundingBox(ref boundingBox);
			if (iGrowBox != null)
			{
				double value = 0.0;
				iGrowBox.GetDouble(ref value);
				boundingBox.Max += new Vector3d(value, value, value);
				boundingBox.Min -= new Vector3d(value, value, value);
			}
			Point3d min = new Point3d(boundingBox.Min.X, boundingBox.Min.Y, boundingBox.Min.Z - 1000.0);
			Point3d max = new Point3d(boundingBox.Max.X, boundingBox.Max.Y, boundingBox.Max.Z + 1000.0);
			BoundingBox boundingBox2 = new BoundingBox(min, max);
			foreach (RhinoObject selectedObject in RhinoDoc.ActiveDoc.Objects.GetSelectedObjects(includeLights: false, includeGrips: false))
			{
				if (selectedObject.Geometry == null)
				{
					continue;
				}
				BoundingBox boundingBox3 = selectedObject.Geometry.GetBoundingBox(selectedObject.Geometry is Curve);
				if ((boundingBox3.Min.CompareTo(boundingBox.Min) == 0 && boundingBox3.Max.CompareTo(boundingBox.Max) == 0) || !boundingBox2.Contains(boundingBox3))
				{
					continue;
				}
				GeometryBase geometryBase = selectedObject.Geometry.Duplicate();
				if (geometryBase != null)
				{
					if (selectedObject.Attributes.Name.Length > 0)
					{
						geometryBase.SetUserString("CSName", selectedObject.Attributes.Name);
					}
					if (selectedObject.Attributes.LayerIndex >= 0 && selectedObject.Attributes.LayerIndex < RhinoDoc.ActiveDoc.Layers.Count)
					{
						string name = RhinoDoc.ActiveDoc.Layers[selectedObject.Attributes.LayerIndex].Name;
						geometryBase.SetUserString("CSLayer", name);
					}
					On3dmObjectAttributes attributes2 = new On3dmObjectAttributes();
					archive.Write3dmObject(geometryBase.Convert(), attributes2);
				}
			}
		}
		return archive.EndWrite3dmChunk();
	}
}
