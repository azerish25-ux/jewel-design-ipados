using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using CS;
using CS.CSObjects;
using CS.CSObjects.CSBlocks;
using CS.Parametric;
using CS.Parametric.Channels;
using CSBlockDataUtils;
using CSCompatibility;
using CSDatas;
using Newtonsoft.Json.Linq;
using RMA.OpenNURBS;
using Rhino;
using Rhino.DocObjects;
using Rhino.Geometry;

namespace CSBlocks.Core;

[CSBlock("GroupOfRhinoObjects", "{96E74C38-4170-40fc-9646-3FEECF472C80}")]
public sealed class CSGroupOfRhinoObjects : CSBlock
{
	private sealed class RhinoObjects
	{
		public List<GeometryBase> Objects;

		public Bitmap PreviewImage;

		public OrientToGemShape ValidOrientShapes;

		private bool _attemptedTightBox;

		private BoundingBox _tightBoundingBox;

		public RhinoObjects()
		{
			Objects = new List<GeometryBase>();
		}

		public RhinoObjects(List<GeometryBase> objects)
		{
			Objects = objects;
		}

		public RhinoObjects(List<GeometryBase> objects, Bitmap previewImage)
		{
			Objects = objects;
			PreviewImage = previewImage;
		}

		public static RhinoObjects Duplicate(RhinoObjects source)
		{
			RhinoObjects rhinoObjects = new RhinoObjects
			{
				Objects = new List<GeometryBase>()
			};
			for (int i = 0; i < source.Objects.Count; i++)
			{
				rhinoObjects.Objects.Add(source.Objects[i].Duplicate());
			}
			if (source.PreviewImage != null)
			{
				rhinoObjects.PreviewImage = new Bitmap(source.PreviewImage);
			}
			rhinoObjects.ValidOrientShapes = source.ValidOrientShapes;
			rhinoObjects._tightBoundingBox = (source._tightBoundingBox.IsValid ? BoundingBox.Unset : source._tightBoundingBox);
			return rhinoObjects;
		}

		public BoundingBox GetTightBoundingBox()
		{
			if (_attemptedTightBox)
			{
				return _tightBoundingBox;
			}
			_attemptedTightBox = true;
			_tightBoundingBox = BoundingBox.Unset;
			bool flag = false;
			foreach (GeometryBase @object in Objects)
			{
				if (@object != null)
				{
					BoundingBox boundingBox = @object.GetBoundingBox(@object is Curve);
					_tightBoundingBox.Union(boundingBox);
					flag |= _tightBoundingBox.IsValid;
				}
			}
			if (!flag)
			{
				_tightBoundingBox = BoundingBox.Unset;
			}
			return _tightBoundingBox;
		}

		public static RhinoObjects ReadJson(JObject jObj)
		{
			RhinoObjects rhinoObjects = new RhinoObjects();
			foreach (JToken item in (JArray)jObj["Objects"])
			{
				rhinoObjects.Objects.Add(CSUtilIO.GeometryBaseFromString(item.ToString()));
			}
			JToken jToken = jObj["PreviewBitmap"];
			if (jToken != null)
			{
				rhinoObjects.PreviewImage = CSUtilIO.BitmapFromString(jToken.ToString());
			}
			JToken jToken2 = jObj["OrientShapes"];
			if (jToken2 != null)
			{
				rhinoObjects.ValidOrientShapes = new OrientToGemShape();
				rhinoObjects.ValidOrientShapes.ReadJson((JObject)jToken2);
			}
			return rhinoObjects;
		}

		public JObject WriteJSon()
		{
			JObject jObject = new JObject();
			JArray jArray = new JArray();
			foreach (GeometryBase @object in Objects)
			{
				if (@object != null)
				{
					string text = CSUtilIO.GeometryBaseToString(@object);
					if (text != null)
					{
						jArray.Add(text);
					}
				}
			}
			jObject.Add("Objects", jArray);
			if (PreviewImage != null)
			{
				jObject.Add("PreviewBitmap", CSUtilIO.BitmapToString(PreviewImage));
			}
			if (ValidOrientShapes != null)
			{
				jObject.Add("OrientShapes", ValidOrientShapes.WriteJson());
			}
			return jObject;
		}
	}

	private string _previousRestrictTo;

	private List<RhinoObjects> _rhinoObjects = new List<RhinoObjects>();

	public CSInputChannel InOnOff { get; }

	public CSInputChannel InRestrictTo { get; }

	public CSGroupOfRhinoObjects(string identifier)
		: base(identifier)
	{
		base.IsOutputBlockProvider = true;
		InRestrictTo = new CSInputChannel(GetPropertyName(() => InRestrictTo), this, ChannelDataType.Any, ChannelArrayType.Array | ChannelArrayType.Individual);
		InOnOff = new CSInputChannel(GetPropertyName(() => InOnOff), this, ChannelDataType.Boolean, ChannelArrayType.Individual);
	}

	public override bool AddObjectFromCurrentSelection()
	{
		bool result = false;
		List<GeometryBase> list = new List<GeometryBase>();
		Bitmap previewImage = RhinoDoc.ActiveDoc.Views.ActiveView.CaptureToBitmap();
		foreach (RhinoObject selectedObject in RhinoDoc.ActiveDoc.Objects.GetSelectedObjects(includeLights: false, includeGrips: false))
		{
			if (selectedObject.Geometry == null)
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
				list.Add(geometryBase);
				result = true;
			}
		}
		PreChange("AddObjectFromCurrentSelection");
		_rhinoObjects.Add(new RhinoObjects(list, previewImage));
		Change();
		Update();
		return result;
	}

	public override bool AddObjectFromDocument(string objectName)
	{
		RhinoObjects rhinoObjects = new RhinoObjects();
		foreach (RhinoObject @object in RhinoDoc.ActiveDoc.Objects)
		{
			if (!(@object.Attributes.Name != objectName))
			{
				GeometryBase geometryBase = @object.Geometry.Duplicate();
				if (@object.Attributes.Name.Length > 0)
				{
					geometryBase.SetUserString("CSName", @object.Attributes.Name);
				}
				if (@object.Attributes.LayerIndex >= 0 && @object.Attributes.LayerIndex < RhinoDoc.ActiveDoc.Layers.Count)
				{
					string name = RhinoDoc.ActiveDoc.Layers[@object.Attributes.LayerIndex].Name;
					geometryBase.SetUserString("CSLayer", name);
				}
				rhinoObjects.Objects.Add(geometryBase);
			}
		}
		if (rhinoObjects.Objects.Count > 0)
		{
			PreChange("AddObjectFromDocument");
			_rhinoObjects.Add(rhinoObjects);
			Change();
			Update();
			return true;
		}
		return false;
	}

	public override bool AddObjectFromFile(string fileName)
	{
		return AddObjectFromFile(fileName, null);
	}

	public bool AddObjectFromFile(string fileName, string objectName)
	{
		CSUtilFile.ReadOneObject(Path.Combine(CSApplicationHelper.Application.CSLibraryPath, fileName), out var objects, out var previewImage, out var notes);
		OrientToGemShape orientToGemShape = null;
		if (!string.IsNullOrEmpty(notes))
		{
			orientToGemShape = OrientToGemShape.ReadFromString(notes);
			if (orientToGemShape == null)
			{
				CSUtil.Print("Error loading gem shape!\n");
			}
		}
		if (objects != null)
		{
			if (objectName != null)
			{
				for (int i = 0; i < objects.Count; i++)
				{
					bool flag = false;
					string userString = objects[i].GetUserString("CSName");
					if (!string.IsNullOrWhiteSpace(userString) && string.Compare(userString, objectName, StringComparison.Ordinal) == 0)
					{
						flag = true;
					}
					if (!flag)
					{
						objects.RemoveAt(i);
						i--;
					}
				}
			}
			if (objects.Count > 0)
			{
				PreChange("AddObjectFromFile");
				RhinoObjects item = new RhinoObjects(objects)
				{
					PreviewImage = previewImage,
					ValidOrientShapes = orientToGemShape
				};
				_rhinoObjects.Add(item);
				Change();
				Update();
				return true;
			}
		}
		return false;
	}

	public override bool BlankOutBeforeUpdate()
	{
		return false;
	}

	public override bool DeleteObject(int index)
	{
		PreChange("DeleteObject");
		_rhinoObjects.RemoveAt(index);
		Change();
		Update();
		return true;
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
		//IL_0105: Unknown result type (might be due to invalid IL or missing references)
		//IL_010c: Expected O, but got Unknown
		//IL_0051: Unknown result type (might be due to invalid IL or missing references)
		//IL_0058: Expected O, but got Unknown
		base.Read(ref archive, graph, major, minor);
		int[] items = null;
		CSUtilIO.ReadArray(ref items, ref archive);
		_rhinoObjects.Clear();
		uint num = 0u;
		int num2 = 0;
		if (!archive.BeginRead3dmChunk(ref num, ref num2))
		{
			return false;
		}
		if (num == 268435475)
		{
			OnObject val2 = default(OnObject);
			for (int i = 0; i < items.Length; i++)
			{
				RhinoObjects rhinoObjects = new RhinoObjects();
				for (int j = 0; j < items[i]; j++)
				{
					On3dmObjectAttributes val = new On3dmObjectAttributes();
					archive.Read3dmObject(ref val2, ref val, 0u);
					if (val2 != null)
					{
						GeometryBase geometryBase = val2.DuplicateOnObject().Convert();
						if (geometryBase != null)
						{
							rhinoObjects.Objects.Add(geometryBase);
						}
					}
					else
					{
						CSUtil.Print("RhinoObjects Unable to read\n");
					}
				}
				_rhinoObjects.Add(rhinoObjects);
			}
		}
		int num3 = 0;
		archive.ReadInt(ref num3);
		if (_rhinoObjects.Count == num3)
		{
			for (int k = 0; k < num3; k++)
			{
				int num4 = 0;
				archive.ReadInt(ref num4);
				if (num4 == 0)
				{
					_rhinoObjects[k].PreviewImage = null;
					continue;
				}
				OnWindowsBitmap val3 = new OnWindowsBitmap();
				val3.ReadCompressed(ref archive);
				_rhinoObjects[k].PreviewImage = CSUtil.BitmapFromOnWindowsBitmap(val3);
				((OnObject)val3).Dispose();
			}
		}
		if (major >= 2 || (major == 1 && minor >= 1))
		{
			int num5 = 0;
			archive.ReadInt(ref num5);
			for (int l = 0; l < _rhinoObjects.Count && l < num5; l++)
			{
				int num6 = 0;
				archive.ReadInt(ref num6);
				if (num6 == 1)
				{
					_rhinoObjects[l].ValidOrientShapes = new OrientToGemShape();
					_rhinoObjects[l].ValidOrientShapes.Read(archive);
				}
				else
				{
					_rhinoObjects[l].ValidOrientShapes = null;
				}
			}
		}
		archive.EndRead3dmChunk();
		return true;
	}

	public override bool ReadJson(JObject jBlock, CSParaGraph graph, int major, int minor)
	{
		base.ReadJson(jBlock, graph, major, minor);
		JArray obj = (JArray)jBlock["RhinoObjects"];
		_rhinoObjects.Clear();
		foreach (JToken item in obj)
		{
			_rhinoObjects.Add(RhinoObjects.ReadJson((JObject)item));
		}
		return true;
	}

	public override bool SetBitmapToChannel(int channel, Bitmap image)
	{
		if (channel < 0 || channel >= _rhinoObjects.Count)
		{
			return false;
		}
		_rhinoObjects[channel].PreviewImage = new Bitmap(image);
		Change();
		Update();
		return true;
	}

	public override bool SetUndo(CSObject setObject)
	{
		if (!(setObject is CSGroupOfRhinoObjects cSGroupOfRhinoObjects))
		{
			return base.SetUndo(setObject);
		}
		_rhinoObjects = new List<RhinoObjects>();
		cSGroupOfRhinoObjects._rhinoObjects.ForEach(delegate(RhinoObjects _)
		{
			_rhinoObjects.Add(RhinoObjects.Duplicate(_));
		});
		return base.SetUndo(setObject);
	}

	public override bool Update()
	{
		CSGemData cSGemData = null;
		CSDataArray flattenArray = InRestrictTo.GetFlattenArray();
		if (flattenArray.GetArrayCount() > 0)
		{
			cSGemData = flattenArray.GetArrayItem(0) as CSGemData;
		}
		string text = null;
		if (cSGemData != null)
		{
			text = cSGemData.GemShape;
		}
		if (_previousRestrictTo != null && text != null && string.Compare(_previousRestrictTo, text, StringComparison.Ordinal) == 0 && base.OutputChannels.Count == _rhinoObjects.Count)
		{
			return base.Update();
		}
		_previousRestrictTo = text;
		double x = 1.0;
		double y = 1.0;
		base.OutputChannels.Clear();
		string text2 = null;
		for (int i = 0; i < _rhinoObjects.Count; i++)
		{
			RhinoObjects rhinoObjects = _rhinoObjects[i];
			if (rhinoObjects.Objects == null)
			{
				continue;
			}
			bool disabled = false;
			bool flag = false;
			if (text != null)
			{
				disabled = true;
				if (rhinoObjects.ValidOrientShapes != null)
				{
					disabled = true;
					if (rhinoObjects.ValidOrientShapes.FindShape(text, ref x, ref y))
					{
						disabled = false;
						flag = true;
						text2 = rhinoObjects.ValidOrientShapes.Mounting;
					}
				}
				if (!flag)
				{
					BoundingBox tightBoundingBox = rhinoObjects.GetTightBoundingBox();
					if (tightBoundingBox.IsValid)
					{
						x = tightBoundingBox.Max.X - 0.5;
						y = tightBoundingBox.Max.Y - 0.5;
						flag = true;
					}
				}
			}
			Transform transform = default(Transform);
			if (flag)
			{
				if (x < 0.001)
				{
					x = 1.0;
				}
				if (y < 0.001)
				{
					y = 1.0;
				}
				transform = Transform.Scale(Plane.WorldXY, 1.0 / x, 1.0 / y, 2.0 / (x + y));
			}
			else
			{
				transform = Transform.Identity;
			}
			if (rhinoObjects.Objects.Count > 1)
			{
				CSDataArray cSDataArray = new CSDataArray();
				cSDataArray.SetBitmap(rhinoObjects.PreviewImage);
				cSDataArray.Disabled = disabled;
				cSDataArray.NormalizedToGem = true;
				foreach (GeometryBase @object in rhinoObjects.Objects)
				{
					if (@object != null)
					{
						GeometryBase geometryBase = @object.Duplicate();
						geometryBase.MakeDeformable();
						geometryBase.Transform(transform);
						CSOnGeometryData cSOnGeometryData = new CSOnGeometryData(geometryBase)
						{
							NormalizedToGem = true
						};
						if (text2 != null)
						{
							cSOnGeometryData.GetGeometry().SetUserString("CSMounting", text2);
						}
						cSDataArray.Append(cSOnGeometryData);
					}
					else
					{
						CSUtil.PrintLine("RhinoObjects Invalid Object");
					}
				}
				new CSOutputChannel($"{base.Identifier}_{i}", this, ChannelDataType.Any, ChannelArrayType.Array).SetData(cSDataArray);
			}
			else
			{
				if (rhinoObjects.Objects.Count != 1 || rhinoObjects.Objects[0] == null)
				{
					continue;
				}
				if (rhinoObjects.Objects[0] != null)
				{
					GeometryBase geometryBase2 = rhinoObjects.Objects[0].Duplicate();
					geometryBase2.Transform(transform);
					CSOnGeometryData cSOnGeometryData2 = new CSOnGeometryData(geometryBase2);
					if (text2 != null)
					{
						cSOnGeometryData2.GetGeometry().SetUserString("CSMounting", text2);
					}
					cSOnGeometryData2.SetBitmap(rhinoObjects.PreviewImage);
					cSOnGeometryData2.Disabled = disabled;
					cSOnGeometryData2.NormalizedToGem = true;
					new CSOutputChannel($"{base.Identifier}_{i}", this, ChannelDataType.Any, ChannelArrayType.Any).SetData(cSOnGeometryData2);
				}
				else
				{
					CSUtil.PrintLine("RhinoObjects Invalid Object");
				}
			}
		}
		return base.Update();
	}

	public override bool Write(ref OnBinaryArchive archive)
	{
		//IL_0065: Unknown result type (might be due to invalid IL or missing references)
		//IL_006c: Expected O, but got Unknown
		//IL_011e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0125: Expected O, but got Unknown
		base.Write(ref archive);
		List<int> list = new List<int>();
		for (int i = 0; i < _rhinoObjects.Count; i++)
		{
			list.Add(_rhinoObjects[i].Objects.Count);
		}
		CSUtilIO.WriteArray(list.ToArray(), ref archive);
		archive.BeginWrite3dmChunk(268435475u, 0);
		for (int j = 0; j < _rhinoObjects.Count; j++)
		{
			for (int k = 0; k < _rhinoObjects[j].Objects.Count; k++)
			{
				On3dmObjectAttributes val = new On3dmObjectAttributes();
				archive.Write3dmObject((IOnObject)(object)_rhinoObjects[j].Objects[k].Convert(), (IOn3dmObjectAttributes)(object)val);
			}
		}
		int count = _rhinoObjects.Count;
		archive.WriteInt(count);
		for (int l = 0; l < _rhinoObjects.Count; l++)
		{
			if (_rhinoObjects[l].PreviewImage == null)
			{
				archive.WriteInt(0);
				continue;
			}
			archive.WriteInt(1);
			OnWindowsBitmap val2 = new OnWindowsBitmap(_rhinoObjects[l].PreviewImage);
			try
			{
				val2.WriteCompressed(ref archive);
			}
			finally
			{
				((IDisposable)val2)?.Dispose();
			}
		}
		int count2 = _rhinoObjects.Count;
		archive.WriteInt(count2);
		for (int m = 0; m < _rhinoObjects.Count; m++)
		{
			if (_rhinoObjects[m].ValidOrientShapes != null)
			{
				archive.WriteInt(1);
				_rhinoObjects[m].ValidOrientShapes.Write(archive);
			}
			else
			{
				archive.WriteInt(0);
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
		foreach (RhinoObjects rhinoObject in _rhinoObjects)
		{
			jArray.Add(rhinoObject.WriteJSon());
		}
		archive.Add("RhinoObjects", jArray);
	}
}
