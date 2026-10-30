using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using CS;
using CS.CSObjects;
using CS.CSObjects.CSBlocks;
using CS.JSonDoc;
using CS.Parametric;
using CS.Parametric.Channels;
using CSCompatibility;
using CSDatas;
using CSGemInstance;
using CSMaterials;
using Newtonsoft.Json.Linq;
using RMA.OpenNURBS;
using Rhino.FileIO;
using Rhino.Geometry;

namespace CSBlocks.Core;

[CSBlock("GroupOfRhinoObjectsStudioParts", "{66112753-194d-4e74-a267-b13e12a85068}")]
public sealed class CSGroupOfRhinoObjectsStudioParts : CSBlock
{
	public List<GeometryBase> Objects;

	public CSInputChannel InOnOff { get; }

	public CSGroupOfRhinoObjectsStudioParts(string identifier)
		: base(identifier)
	{
		base.IsOutputBlockProvider = true;
		InOnOff = new CSInputChannel(GetPropertyName(() => InOnOff), this, ChannelDataType.Boolean, ChannelArrayType.Individual);
	}

	public override bool AddObjectFromFile(string fileName)
	{
		return AddObjectFromFile(fileName, null);
	}

	public bool AddObjectFromFile(string fileName, string objectName)
	{
		ReadOneObject(Path.Combine(CSApplicationHelper.Application.CSLibraryPath, fileName), out var objects, out var _);
		if (objects != null)
		{
			Objects = objects;
			Change();
			Update();
			return true;
		}
		return false;
	}

	public override bool BlankOutBeforeUpdate()
	{
		return false;
	}

	public override CSData GetOutputChannelData(int outputChannelIndex)
	{
		if (outputChannelIndex >= 0 && outputChannelIndex < base.OutputChannels.Count)
		{
			bool value = true;
			InOnOff.GetBoolean(ref value);
			if (!value)
			{
				return new CSData();
			}
			return base.OutputChannels[outputChannelIndex].Data;
		}
		return new CSData();
	}

	public override bool Read(ref OnBinaryArchive archive, CSParaGraph graph, int major, int minor)
	{
		//IL_004b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0052: Expected O, but got Unknown
		base.Read(ref archive, graph, major, minor);
		int num = 0;
		archive.ReadInt(ref num);
		bool flag = true;
		Objects = new List<GeometryBase>();
		if (num > 0)
		{
			uint num2 = 0u;
			int num3 = 0;
			flag = archive.BeginRead3dmChunk(ref num2, ref num3);
			if (!flag)
			{
				return false;
			}
			if (num2 == 268435475)
			{
				OnObject val2 = default(OnObject);
				for (int i = 0; i < num; i++)
				{
					On3dmObjectAttributes val = new On3dmObjectAttributes();
					archive.Read3dmObject(ref val2, ref val, 0u);
					if (val2 != null)
					{
						Objects.Add(val2.Convert().Duplicate());
					}
					else
					{
						CS.CSUtil.Print("RhinoObjects Unable to read\n");
					}
				}
			}
		}
		if (!archive.EndRead3dmChunk())
		{
			flag = false;
		}
		return flag;
	}

	public override bool ReadJson(JObject archive, CSParaGraph graph, int major, int minor)
	{
		bool flag = base.ReadJson(archive, graph, major, minor);
		JArray obj = (JArray)archive["RhinoObjects"];
		JArray jArray = (JArray)archive["RhinoGemObjects"];
		Objects = new List<GeometryBase>();
		foreach (JToken item in obj)
		{
			GeometryBase geometryBase = CSUtilIO.GeometryBaseFromString(item.ToString());
			flag = flag && geometryBase != null;
			if (geometryBase != null)
			{
				Objects.Add(geometryBase);
			}
		}
		if (jArray != null)
		{
			foreach (JObject item2 in jArray)
			{
				item2.GetMatrix("GemTransform", out var transform);
				string value = item2.GetValue("GemLayer").ToString();
				string value2 = item2.GetValue("GemType").ToString();
				string value3 = item2.GetValue("GemShape").ToString();
				InstanceReferenceGeometry instanceReferenceGeometry = new InstanceReferenceGeometry(Guid.Empty, transform);
				instanceReferenceGeometry.SetUserString("CSLayer", value);
				instanceReferenceGeometry.SetUserString("CSType", value2);
				instanceReferenceGeometry.SetUserString("CSShape", value3);
				Objects.Add(instanceReferenceGeometry);
			}
		}
		return flag;
	}

	public static bool ReadOneObject(string fileName, out List<GeometryBase> objects, out Bitmap previewImage)
	{
		File3dm file3dm = File3dm.Read(fileName);
		previewImage = File3dm.ReadPreviewImage(fileName);
		objects = new List<GeometryBase>();
		foreach (File3dmObject @object in file3dm.Objects)
		{
			if (@object.Attributes.IsInstanceDefinitionObject)
			{
				continue;
			}
			GeometryBase geometryBase = @object.Geometry.Duplicate();
			if (geometryBase == null)
			{
				continue;
			}
			if (!string.IsNullOrWhiteSpace(@object.Attributes.Name))
			{
				geometryBase.SetUserString("CSName", @object.Attributes.Name);
			}
			if (@object.Attributes.LayerIndex >= 0 && @object.Attributes.LayerIndex < file3dm.Layers.Count)
			{
				string name = file3dm.Layers[@object.Attributes.LayerIndex].Name;
				geometryBase.SetUserString("CSLayer", name);
			}
			if (geometryBase is InstanceReferenceGeometry { ParentIdefId: var parentIdefId })
			{
				for (int i = 0; i < file3dm.InstanceDefinitions.Count; i++)
				{
					if (file3dm.InstanceDefinitions[i].Id == parentIdefId)
					{
						string[] array = file3dm.InstanceDefinitions[i].Name.Split('_');
						if (array.Length == 2 || array.Length == 3)
						{
							geometryBase.SetUserString("CSType", array[0]);
							geometryBase.SetUserString("CSShape", array[1]);
						}
					}
				}
			}
			objects.Add(geometryBase);
		}
		file3dm.Dispose();
		return true;
	}

	public override bool SetUndo(CSObject setObject)
	{
		if (!(setObject is CSGroupOfRhinoObjectsStudioParts cSGroupOfRhinoObjectsStudioParts))
		{
			return base.SetUndo(setObject);
		}
		if (cSGroupOfRhinoObjectsStudioParts.Objects == null)
		{
			Objects = null;
			return base.SetUndo(setObject);
		}
		Objects = new List<GeometryBase>();
		cSGroupOfRhinoObjectsStudioParts.Objects.ForEach(delegate(GeometryBase _)
		{
			Objects.Add(_.Duplicate());
		});
		return base.SetUndo(setObject);
	}

	public override bool Update()
	{
		base.OutputChannels.Clear();
		if (Objects != null)
		{
			CSDataArray cSDataArray = new CSDataArray();
			foreach (GeometryBase @object in Objects)
			{
				if (@object == null)
				{
					continue;
				}
				GeometryBase geometryBase = @object.Duplicate();
				CSData cSData;
				MaterialClassification materialClassificationOnDisplay;
				if (@object is InstanceReferenceGeometry { Xform: var xform })
				{
					string userString = @object.GetUserString("CSType");
					string userString2 = @object.GetUserString("CSShape");
					if (string.IsNullOrEmpty(userString) || string.IsNullOrEmpty(userString2))
					{
						continue;
					}
					CSApplicationHelper.Application.Document.GemTable.LoadGemInTable(userString, userString2);
					CSGemInstanceDefinition gem = CSApplicationHelper.Application.Document.GemTable.GetGem(userString, userString2, CSGemInstanceDefinitionHelper.CurrentVersionMajor, CSGemInstanceDefinitionHelper.CurrentVersionMinor, 0, isUsingMajorMinor: false);
					if (gem == null)
					{
						continue;
					}
					cSData = new CSGemData(userString, userString2, gem, xform);
					materialClassificationOnDisplay = MaterialClassification.GemCenter;
				}
				else
				{
					geometryBase.MakeDeformable();
					cSData = new CSOnGeometryData(geometryBase);
					materialClassificationOnDisplay = MaterialClassification.MetalSecondary;
				}
				string userString3 = @object.GetUserString("CSLayer");
				switch (userString3.ToLower().Replace(" ", string.Empty))
				{
				case "metal01":
				case "metal03":
					cSData.SetMaterialClassificationOnDisplay(MaterialClassification.MetalPrimary);
					break;
				case "metal02":
				case "metal04":
					cSData.SetMaterialClassificationOnDisplay(MaterialClassification.MetalSecondary);
					break;
				case "gem01":
					cSData.SetMaterialClassificationOnDisplay(MaterialClassification.GemCenter);
					break;
				case "gem02":
					cSData.SetMaterialClassificationOnDisplay(MaterialClassification.GemSide);
					break;
				case "gem03":
					cSData.SetMaterialClassificationOnDisplay(MaterialClassification.GemAccent);
					break;
				default:
					cSData.SetMaterialClassificationOnDisplay(materialClassificationOnDisplay);
					break;
				}
				cSDataArray.Append(cSData);
			}
			new CSOutputChannel($"{base.Identifier}", this, ChannelDataType.Any, ChannelArrayType.Array).SetData(cSDataArray);
		}
		return base.Update();
	}

	public override bool Write(ref OnBinaryArchive archive)
	{
		//IL_0040: Unknown result type (might be due to invalid IL or missing references)
		//IL_0046: Expected O, but got Unknown
		base.Write(ref archive);
		if (Objects == null)
		{
			archive.WriteInt(0);
		}
		else
		{
			archive.WriteInt(Objects.Count);
			archive.BeginWrite3dmChunk(268435475u, 0);
			for (int i = 0; i < Objects.Count; i++)
			{
				On3dmObjectAttributes val = new On3dmObjectAttributes();
				archive.Write3dmObject((IOnObject)(object)Objects[i].Convert(), (IOn3dmObjectAttributes)(object)val);
			}
		}
		if (!archive.EndWrite3dmChunk())
		{
			return false;
		}
		return true;
	}

	public override void WriteJson(ref JObject archive)
	{
		base.WriteJson(ref archive);
		JArray jArray = new JArray();
		JArray jArray2 = new JArray();
		foreach (GeometryBase @object in Objects)
		{
			if (@object == null)
			{
				continue;
			}
			if (@object is InstanceReferenceGeometry instanceReferenceGeometry)
			{
				string userString = @object.GetUserString("CSLayer");
				string userString2 = @object.GetUserString("CSType");
				string userString3 = @object.GetUserString("CSShape");
				JObject item = new JObject
				{
					{ "GemTransform", instanceReferenceGeometry.Xform },
					{ "GemLayer", userString },
					{ "GemType", userString2 },
					{ "GemShape", userString3 }
				};
				jArray2.Add(item);
			}
			else
			{
				string text = CSUtilIO.GeometryBaseToString(@object);
				if (text != null)
				{
					jArray.Add(text);
				}
			}
		}
		archive.Add("RhinoObjects", jArray);
		if (jArray2.Count > 0)
		{
			archive.Add("RhinoGemObjects", jArray2);
		}
	}
}
