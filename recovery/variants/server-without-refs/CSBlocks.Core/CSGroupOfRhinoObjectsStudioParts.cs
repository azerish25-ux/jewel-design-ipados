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
		//IL_00e0: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e2: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e9: Expected O, but got Unknown
		bool flag = base.ReadJson(archive, graph, major, minor);
		JArray obj = (JArray)archive["RhinoObjects"];
		JArray jArray = (JArray)archive["RhinoGemObjects"];
		Objects = new List<GeometryBase>();
		foreach (JToken item in obj)
		{
			GeometryBase val = CSUtilIO.GeometryBaseFromString(item.ToString());
			flag = flag && val != null;
			if (val != null)
			{
				Objects.Add(val);
			}
		}
		if (jArray != null)
		{
			foreach (JObject item2 in jArray)
			{
				item2.GetMatrix("GemTransform", out var transform);
				string text = item2.GetValue("GemLayer").ToString();
				string text2 = item2.GetValue("GemType").ToString();
				string text3 = item2.GetValue("GemShape").ToString();
				InstanceReferenceGeometry val2 = new InstanceReferenceGeometry(Guid.Empty, transform);
				((GeometryBase)val2).SetUserString("CSLayer", text);
				((GeometryBase)val2).SetUserString("CSType", text2);
				((GeometryBase)val2).SetUserString("CSShape", text3);
				Objects.Add((GeometryBase)(object)val2);
			}
		}
		return flag;
	}

	public static bool ReadOneObject(string fileName, out List<GeometryBase> objects, out Bitmap previewImage)
	{
		File3dm val = File3dm.Read(fileName);
		previewImage = File3dm.ReadPreviewImage(fileName);
		objects = new List<GeometryBase>();
		foreach (File3dmObject @object in val.Objects)
		{
			if (@object.Attributes.IsInstanceDefinitionObject)
			{
				continue;
			}
			GeometryBase val2 = @object.Geometry.Duplicate();
			if (val2 == null)
			{
				continue;
			}
			if (!string.IsNullOrWhiteSpace(@object.Attributes.Name))
			{
				val2.SetUserString("CSName", @object.Attributes.Name);
			}
			if (@object.Attributes.LayerIndex >= 0 && @object.Attributes.LayerIndex < val.Layers.Count)
			{
				string name = val.Layers[@object.Attributes.LayerIndex].Name;
				val2.SetUserString("CSLayer", name);
			}
			InstanceReferenceGeometry val3 = (InstanceReferenceGeometry)(object)((val2 is InstanceReferenceGeometry) ? val2 : null);
			if (val3 != null)
			{
				Guid parentIdefId = val3.ParentIdefId;
				for (int i = 0; i < val.InstanceDefinitions.Count; i++)
				{
					if (val.InstanceDefinitions[i].Id == parentIdefId)
					{
						string[] array = val.InstanceDefinitions[i].Name.Split('_');
						if (array.Length == 2 || array.Length == 3)
						{
							val2.SetUserString("CSType", array[0]);
							val2.SetUserString("CSShape", array[1]);
						}
					}
				}
			}
			objects.Add(val2);
		}
		val.Dispose();
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
		//IL_0054: Unknown result type (might be due to invalid IL or missing references)
		//IL_0059: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d9: Unknown result type (might be due to invalid IL or missing references)
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
				GeometryBase val = @object.Duplicate();
				InstanceReferenceGeometry val2 = (InstanceReferenceGeometry)(object)((@object is InstanceReferenceGeometry) ? @object : null);
				CSData cSData;
				MaterialClassification materialClassificationOnDisplay;
				if (val2 != null)
				{
					Transform xform = val2.Xform;
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
					val.MakeDeformable();
					cSData = new CSOnGeometryData(val);
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
		//IL_0075: Unknown result type (might be due to invalid IL or missing references)
		base.WriteJson(ref archive);
		JArray jArray = new JArray();
		JArray jArray2 = new JArray();
		foreach (GeometryBase @object in Objects)
		{
			if (@object == null)
			{
				continue;
			}
			InstanceReferenceGeometry val = (InstanceReferenceGeometry)(object)((@object is InstanceReferenceGeometry) ? @object : null);
			if (val != null)
			{
				string userString = @object.GetUserString("CSLayer");
				string userString2 = @object.GetUserString("CSType");
				string userString3 = @object.GetUserString("CSShape");
				JObject item = new JObject
				{
					{ "GemTransform", val.Xform },
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
