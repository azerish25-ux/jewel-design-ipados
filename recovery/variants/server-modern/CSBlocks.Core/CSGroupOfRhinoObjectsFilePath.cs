using System.Collections.Generic;
using System.Drawing;
using System.IO;
using CS;
using CS.CSObjects.CSBlocks;
using CS.JSonDoc;
using CS.Parametric;
using CS.Parametric.Channels;
using CSBlockDataUtils;
using CSDatas;
using Newtonsoft.Json.Linq;
using RMA.OpenNURBS;
using Rhino.Geometry;

namespace CSBlocks.Core;

[CSBlock("GroupOfRhinoObjectsFilePath", "{C3E1FCF1-AE33-446c-892F-DBD4B6BA523F}")]
public sealed class CSGroupOfRhinoObjectsFilePath : CSBlock
{
	private sealed class RhinoObjects
	{
		public readonly List<GeometryBase> Objects;

		public readonly string Filename;

		public Bitmap PreviewImage;

		public OrientToGemShape ValidOrientShapes;

		private BoundingBox _tightBoundingBox;

		private bool _attemptedTightBox;

		public RhinoObjects(List<GeometryBase> objects, string filename)
		{
			Objects = objects;
			Filename = filename;
		}

		public BoundingBox GetTightBoundingBox()
		{
			//IL_0017: Unknown result type (might be due to invalid IL or missing references)
			//IL_001c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0009: Unknown result type (might be due to invalid IL or missing references)
			//IL_0046: Unknown result type (might be due to invalid IL or missing references)
			//IL_004b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0052: Unknown result type (might be due to invalid IL or missing references)
			//IL_008e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0083: Unknown result type (might be due to invalid IL or missing references)
			//IL_0088: Unknown result type (might be due to invalid IL or missing references)
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
					((BoundingBox)(ref _tightBoundingBox)).Union(boundingBox);
					flag |= ((BoundingBox)(ref _tightBoundingBox)).IsValid;
				}
			}
			if (!flag)
			{
				_tightBoundingBox = BoundingBox.Unset;
			}
			return _tightBoundingBox;
		}
	}

	private readonly List<RhinoObjects> _rhinoObjects = new List<RhinoObjects>();

	private string _previousRestrictTo;

	public CSInputChannel iRestrictTo { get; }

	public CSGroupOfRhinoObjectsFilePath(string identifier)
		: base(identifier)
	{
		iRestrictTo = new CSInputChannel(GetPropertyName(() => iRestrictTo), this, ChannelDataType.Any, ChannelArrayType.Array | ChannelArrayType.Individual);
	}

	public override bool AddObjectFromFile(string fileName)
	{
		return AddObjectFromFile(fileName, null);
	}

	public bool AddObjectFromFile(string fileName, string objectName)
	{
		List<GeometryBase> list = new List<GeometryBase>();
		Bitmap previewImage = null;
		CSUtilFile.ReadOneObject(Path.Combine(CSApplicationHelper.Application.CSLibraryPath, fileName), out var objects, out previewImage, out var notes);
		foreach (GeometryBase item in objects)
		{
			list.Add(item);
		}
		OrientToGemShape orientToGemShape = null;
		if (!string.IsNullOrEmpty(notes))
		{
			orientToGemShape = OrientToGemShape.ReadFromString(notes);
			if (orientToGemShape == null)
			{
				CSUtil.Print("Error loading gem shape!\n");
			}
		}
		if (list != null)
		{
			if (objectName != null)
			{
				for (int i = 0; i < list.Count; i++)
				{
					bool flag = false;
					string text = "";
					text = list[i].GetUserString("CSName");
					if (text != "" && text.CompareTo(objectName) == 0)
					{
						flag = true;
					}
					if (!flag)
					{
						list.RemoveAt(i);
						i--;
					}
				}
			}
			if (list.Count > 0)
			{
				PreChange("AddObjectFromFile");
				RhinoObjects rhinoObjects = new RhinoObjects(list, fileName);
				rhinoObjects.PreviewImage = previewImage;
				rhinoObjects.ValidOrientShapes = orientToGemShape;
				_rhinoObjects.Add(rhinoObjects);
				Change();
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

	public override bool Read(ref OnBinaryArchive archive, CSParaGraph graph, int major, int minor)
	{
		base.Read(ref archive, graph, major, minor);
		string[] items = null;
		CSUtilIO.ReadArray(ref items, ref archive);
		_rhinoObjects.Clear();
		if (items != null)
		{
			for (int i = 0; i < items.Length; i++)
			{
				AddObjectFromFile(items[i]);
			}
		}
		return true;
	}

	public override bool ReadJson(JObject archive, CSParaGraph graph, int major, int minor)
	{
		bool flag = base.ReadJson(archive, graph, major, minor);
		flag &= archive.ReadJsonObject<string[]>("ObjectFilesnames", out var value);
		_rhinoObjects.Clear();
		if (value == null)
		{
			return flag;
		}
		string[] array = value;
		foreach (string fileName in array)
		{
			flag &= AddObjectFromFile(fileName);
		}
		return flag;
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

	public override bool Update()
	{
		//IL_01b0: Unknown result type (might be due to invalid IL or missing references)
		//IL_01b5: Unknown result type (might be due to invalid IL or missing references)
		//IL_0106: Unknown result type (might be due to invalid IL or missing references)
		//IL_010b: Unknown result type (might be due to invalid IL or missing references)
		//IL_017f: Unknown result type (might be due to invalid IL or missing references)
		//IL_01a7: Unknown result type (might be due to invalid IL or missing references)
		//IL_01ac: Unknown result type (might be due to invalid IL or missing references)
		//IL_0118: Unknown result type (might be due to invalid IL or missing references)
		//IL_011d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0133: Unknown result type (might be due to invalid IL or missing references)
		//IL_0138: Unknown result type (might be due to invalid IL or missing references)
		//IL_02eb: Unknown result type (might be due to invalid IL or missing references)
		//IL_021b: Unknown result type (might be due to invalid IL or missing references)
		CSGemData cSGemData = null;
		if (iRestrictTo != null && iRestrictTo.OutputChannelBlock != null)
		{
			cSGemData = iRestrictTo.GetFlattenArray().GetArrayItem(0) as CSGemData;
		}
		string text = null;
		if (cSGemData != null)
		{
			text = cSGemData.GemShape;
		}
		if (_previousRestrictTo != null && text != null && _previousRestrictTo.CompareTo(text) == 0 && base.OutputChannels.Count == _rhinoObjects.Count)
		{
			return base.Update();
		}
		_previousRestrictTo = text;
		double x = 1.0;
		double y = 1.0;
		string text2 = null;
		base.OutputChannels.Clear();
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
					if (((BoundingBox)(ref tightBoundingBox)).IsValid)
					{
						Point3d max = ((BoundingBox)(ref tightBoundingBox)).Max;
						x = ((Point3d)(ref max)).X - 0.5;
						max = ((BoundingBox)(ref tightBoundingBox)).Max;
						y = ((Point3d)(ref max)).Y - 0.5;
						flag = true;
					}
				}
			}
			Transform val;
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
				val = Transform.Scale(Plane.WorldXY, 1.0 / x, 1.0 / y, 2.0 / (x + y));
			}
			else
			{
				val = Transform.Identity;
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
						GeometryBase obj = @object.Duplicate();
						obj.MakeDeformable();
						obj.Transform(val);
						CSOnGeometryData cSOnGeometryData = new CSOnGeometryData(obj)
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
						CSUtil.Print("RhinoObjects Invalid Object\n");
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
					GeometryBase obj2 = rhinoObjects.Objects[0].Duplicate();
					obj2.Transform(val);
					CSOnGeometryData cSOnGeometryData2 = new CSOnGeometryData(obj2);
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
					CSUtil.Print("RhinoObjects Invalid Object\n");
				}
			}
		}
		return base.Update();
	}

	public override bool Write(ref OnBinaryArchive archive)
	{
		base.Write(ref archive);
		string[] array = new string[_rhinoObjects.Count];
		for (int i = 0; i < array.Length; i++)
		{
			array[i] = _rhinoObjects[i].Filename;
		}
		CSUtilIO.WriteArray(array, ref archive);
		return true;
	}

	public override void WriteJson(ref JObject archive)
	{
		base.WriteJson(ref archive);
		string[] array = new string[_rhinoObjects.Count];
		for (int i = 0; i < array.Length; i++)
		{
			array[i] = _rhinoObjects[i].Filename;
		}
		archive.Add("ObjectFilesnames", JArray.FromObject(array));
	}
}
