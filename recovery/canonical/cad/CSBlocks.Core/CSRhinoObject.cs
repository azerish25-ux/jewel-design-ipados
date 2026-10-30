using System;
using System.Collections.Generic;
using System.Linq;
using CS;
using CS.CSObjects;
using CS.CSObjects.CSBlocks;
using CS.Parametric;
using CS.Parametric.Channels;
using CSCompatibility;
using CSDatas;
using Newtonsoft.Json.Linq;
using RMA.OpenNURBS;
using RMA.Rhino;
using Rhino;
using Rhino.DocObjects;
using Rhino.Geometry;

namespace CSBlocks.Core;

[CSBlock("RhinoObject", "{1759D0DD-8B8E-4b56-B861-C1D7C25A21A6}")]
public sealed class CSRhinoObject : CSBlock, ICSBlockObjectWatcher
{
	public List<Guid> ObjectGuids { get; } = new List<Guid>();

	public List<GeometryBase> OnObjects { get; } = new List<GeometryBase>();

	public CSOutputChannel OutObject { get; }

	public bool UseObjects { get; private set; }

	public Guid[] ObjectUuids
	{
		set
		{
			ObjectGuids.Clear();
			OnObjects.Clear();
			ObjectGuids.AddRange(value);
			UseObjects = false;
			Change();
		}
	}

	public CSRhinoObject(string identifier)
		: base(identifier)
	{
		OutObject = new CSOutputChannel(GetPropertyName(() => OutObject), this, ChannelDataType.Any, ChannelArrayType.Any);
	}

	public override bool ReadJson(JObject jBlock, CSParaGraph graph, int major, int minor)
	{
		base.ReadJson(jBlock, graph, major, minor);
		JArray obj = (JArray)jBlock["RhinoObjects"];
		ObjectGuids.Clear();
		OnObjects.Clear();
		UseObjects = true;
		foreach (JToken item in obj)
		{
			OnObjects.Add(CSUtilIO.GeometryBaseFromString(item.ToString()));
		}
		return true;
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
		ObjectGuids.Clear();
		OnObjects.Clear();
		UseObjects = true;
		On3dmObjectAttributes attributes = new On3dmObjectAttributes();
		if (tcode == 268435475)
		{
			OnObject @object;
			while (archive.Read3dmObject(out @object, ref attributes, 0u) != 0 && @object != null)
			{
				OnObjects.Add(@object.Convert());
			}
		}
		return archive.EndRead3dmChunk();
	}

	public override bool SetUndo(CSObject setObject)
	{
		if (!(setObject is CSRhinoObject cSRhinoObject))
		{
			return base.SetUndo(setObject);
		}
		ObjectGuids.Clear();
		OnObjects.Clear();
		ObjectGuids.AddRange(cSRhinoObject.ObjectGuids);
		OnObjects.AddRange(cSRhinoObject.OnObjects);
		UseObjects = cSRhinoObject.UseObjects;
		return base.SetUndo(setObject);
	}

	public override bool Update()
	{
		CSDataArray cSDataArray = new CSDataArray();
		bool flag = true;
		if (UseObjects)
		{
			foreach (GeometryBase onObject in OnObjects)
			{
				cSDataArray.Append(new CSOnGeometryData(onObject.Duplicate()));
			}
		}
		else
		{
			foreach (Guid objectGuid in ObjectGuids)
			{
				RhinoObject rhinoObject = RhinoDoc.ActiveDoc.Objects.Find(objectGuid);
				GeometryBase geometryBase = rhinoObject?.Geometry;
				if (geometryBase == null)
				{
					continue;
				}
				GeometryBase geometryBase2 = geometryBase.Duplicate();
				if (geometryBase2 != null)
				{
					if (!string.IsNullOrWhiteSpace(rhinoObject.Attributes.Name))
					{
						geometryBase2.SetUserString("CSName", rhinoObject.Attributes.Name);
					}
					if (rhinoObject.Attributes.LayerIndex >= 0 && rhinoObject.Attributes.LayerIndex < RhinoDoc.ActiveDoc.Layers.Count)
					{
						string name = RhinoDoc.ActiveDoc.Layers[rhinoObject.Attributes.LayerIndex].Name;
						geometryBase2.SetUserString("CSLayer", name);
					}
					cSDataArray.Append(new CSOnGeometryData(geometryBase2));
				}
				else
				{
					flag = false;
				}
			}
		}
		for (int i = 0; i < OnObjects.Count; i++)
		{
			(cSDataArray.GetArrayItem(i) as CSOnGeometryData)?.PreProcessDisplayInfo();
		}
		OutObject.SetData(cSDataArray);
		if (!flag)
		{
			return UpdateError("Unable to process Block Rhino Object");
		}
		return base.Update();
	}

	public override void WriteJson(ref JObject archive)
	{
		base.WriteJson(ref archive);
		JArray jArray = new JArray();
		if (UseObjects)
		{
			foreach (GeometryBase onObject in OnObjects)
			{
				jArray.Add(CSUtilIO.GeometryBaseToString(onObject));
			}
		}
		else
		{
			foreach (Guid objectGuid in ObjectGuids)
			{
				RhinoObject rhinoObject = RhinoDoc.ActiveDoc.Objects.Find(objectGuid);
				GeometryBase geometryBase = rhinoObject?.Geometry;
				if (geometryBase == null)
				{
					continue;
				}
				GeometryBase geometryBase2 = geometryBase.Duplicate();
				if (geometryBase2 != null)
				{
					if (!string.IsNullOrWhiteSpace(rhinoObject.Attributes.Name))
					{
						geometryBase2.SetUserString("CSName", rhinoObject.Attributes.Name);
					}
					if (rhinoObject.Attributes.LayerIndex >= 0 && rhinoObject.Attributes.LayerIndex < RhinoDoc.ActiveDoc.Layers.Count)
					{
						string name = RhinoDoc.ActiveDoc.Layers[rhinoObject.Attributes.LayerIndex].Name;
						geometryBase2.SetUserString("CSLayer", name);
					}
					jArray.Add(CSUtilIO.GeometryBaseToString(geometryBase2));
				}
			}
		}
		archive.Add("RhinoObjects", jArray);
	}

	public override bool Write(ref OnBinaryArchive archive)
	{
		base.Write(ref archive);
		archive.BeginWrite3dmChunk(268435475u, 0);
		if (UseObjects)
		{
			foreach (GeometryBase onObject in OnObjects)
			{
				On3dmObjectAttributes attributes = new On3dmObjectAttributes();
				archive.Write3dmObject(onObject.Convert(), attributes);
			}
		}
		else
		{
			foreach (Guid objectGuid in ObjectGuids)
			{
				IRhinoObject rhinoObject = new MRhinoObjRef(objectGuid).Object();
				if (rhinoObject == null)
				{
					continue;
				}
				OnGeometry onGeometry = OnGeometry.Cast(rhinoObject.Geometry().DuplicateOnObject());
				if (onGeometry != null)
				{
					if (rhinoObject.Attributes().m_name.Length > 0)
					{
						onGeometry.SetUserString("CSName", rhinoObject.Attributes().m_name);
					}
					if (rhinoObject.Attributes().m_layer_index >= 0 && rhinoObject.Attributes().m_layer_index < RhUtil.RhinoApp().ActiveDoc().m_layer_table.LayerCount())
					{
						string name = RhUtil.RhinoApp().ActiveDoc().m_layer_table[rhinoObject.Attributes().m_layer_index].m_name;
						onGeometry.SetUserString("CSLayer", name);
					}
					On3dmObjectAttributes attributes2 = new On3dmObjectAttributes();
					archive.Write3dmObject(onGeometry, attributes2);
				}
			}
		}
		return archive.EndWrite3dmChunk();
	}

	public bool CheckForChangeOnAdd(MRhinoObject obj)
	{
		if (UseObjects)
		{
			return false;
		}
		if (ObjectGuids.All((Guid guid) => guid != obj.Attributes().m_uuid))
		{
			return true;
		}
		Change();
		return true;
	}

	public bool CheckForChangeOnReplace(MRhinoObject oldObject, MRhinoObject obj)
	{
		return false;
	}

	public bool CheckForChangeOnDelete(MRhinoObject obj)
	{
		if (UseObjects)
		{
			return false;
		}
		if (ObjectGuids.All((Guid guid) => guid != obj.Attributes().m_uuid))
		{
			return true;
		}
		Change();
		return true;
	}
}
