using System;
using System.Collections.Generic;
using System.IO;
using CS;
using CS.CSObjects;
using CS.CSObjects.CSBlocks;
using CS.Parametric;
using CS.Parametric.Channels;
using CS.Profile;
using CSBlockUtil;
using CSDatas;
using Newtonsoft.Json.Linq;
using RMA.OpenNURBS;
using Rhino;
using Rhino.DocObjects;
using Rhino.Geometry;

namespace CSBlocks.Core;

[CSBlock("GroupOfMatrixProfiles", "{98415B72-DADB-4b9b-94A9-1E6C5865B5F2}")]
public sealed class CSGroupOfMatrixProfiles : CSBlock
{
	private readonly List<string> _curveNames = new List<string>();

	private readonly List<NurbsCurve> _curves = new List<NurbsCurve>();

	private readonly List<List<double>> _paramArray = new List<List<double>>();

	public CSGroupOfMatrixProfiles(string identifier)
		: base(identifier)
	{
		base.IsOutputBlockProvider = true;
	}

	public override bool AddObjectFromCurrentSelection()
	{
		ObjectEnumeratorSettings settings = new ObjectEnumeratorSettings
		{
			DeletedObjects = false,
			ActiveObjects = true,
			ReferenceObjects = true,
			IncludeGrips = false,
			IncludeLights = false,
			SelectedObjectsFilter = true
		};
		IEnumerable<RhinoObject> objectList = RhinoDoc.ActiveDoc.Objects.GetObjectList(settings);
		bool result = false;
		foreach (RhinoObject item in objectList)
		{
			string name = item.Attributes.Name;
			if (!(item.Geometry is Curve curve))
			{
				continue;
			}
			NurbsCurve nurbsCurve = curve.ToNurbsCurve();
			if (nurbsCurve == null)
			{
				continue;
			}
			PreChange("AddObjectFromCurrentSelection");
			_curves.Add(curve.ToNurbsCurve());
			_curveNames.Add(name);
			_paramArray.Add(new List<double>());
			int num = 1;
			while (true)
			{
				RhinoObject rhinoObject = CSUtilRhino.FindObjectInDocument(name + "_p" + num);
				if (rhinoObject == null || !(rhinoObject.Geometry is Point point) || !nurbsCurve.ClosestPoint(point.Location, out var t))
				{
					break;
				}
				_paramArray[_paramArray.Count - 1].Add(t);
				num++;
			}
			Change();
			Update();
			result = true;
		}
		return result;
	}

	public override bool AddObjectFromDocument(string objectName)
	{
		ObjectEnumeratorSettings settings = new ObjectEnumeratorSettings
		{
			DeletedObjects = false,
			ActiveObjects = true,
			ReferenceObjects = true,
			IncludeGrips = false,
			IncludeLights = true,
			SelectedObjectsFilter = true
		};
		foreach (RhinoObject @object in RhinoDoc.ActiveDoc.Objects.GetObjectList(settings))
		{
			if (!(@object.Attributes.Name == objectName) || !(@object.Geometry is Curve curve))
			{
				continue;
			}
			NurbsCurve nurbsCurve = curve.ToNurbsCurve();
			if (nurbsCurve != null)
			{
				PreChange("AddObjectFromDocument");
				_curves.Add(curve.ToNurbsCurve());
				_curveNames.Add(objectName);
				_paramArray.Add(new List<double>());
				int num = 1;
				while (true)
				{
					RhinoObject rhinoObject = CSUtilRhino.FindObjectInDocument(objectName + "_p" + num);
					if (rhinoObject == null || !(rhinoObject.Geometry is Point point) || !nurbsCurve.ClosestPoint(point.Location, out var t))
					{
						break;
					}
					_paramArray[_paramArray.Count - 1].Add(t);
					num++;
				}
				Change();
				Update();
			}
			return true;
		}
		return false;
	}

	public override bool AddObjectFromFile(string fileName)
	{
		CSProfileCurveXML cSProfileCurveXML = CSProfileCurveXML.ReadFromFile(Path.Combine(CSApplicationHelper.Application.CSLibraryPath, fileName));
		if (cSProfileCurveXML != null)
		{
			NurbsCurve nurbsCurve = cSProfileCurveXML.GetNurbsCurve();
			if (nurbsCurve != null)
			{
				PreChange("AddObjectFromFile");
				_curves.Add(nurbsCurve);
				_paramArray.Add(new List<double>());
				if (cSProfileCurveXML.SweepParam != null)
				{
					double[] sweepParam = cSProfileCurveXML.SweepParam;
					foreach (double item in sweepParam)
					{
						_paramArray[_paramArray.Count - 1].Add(item);
					}
				}
				_curveNames.Add(cSProfileCurveXML.Name ?? fileName);
				Change();
				Update();
				return true;
			}
		}
		return false;
	}

	public override bool AddObjectFromLibrary(string libraryFileName)
	{
		return AddObjectFromFile(Path.Combine(CSApplicationHelper.Application.CSLibraryPath, libraryFileName));
	}

	public override bool CopySelected(int outputNumber)
	{
		if (outputNumber < 0)
		{
			return false;
		}
		if (outputNumber >= _curves.Count)
		{
			return false;
		}
		if (_curves[outputNumber] == null)
		{
			return false;
		}
		PreChange("CopySelected");
		_curves.Insert(outputNumber, _curves[outputNumber].ToNurbsCurve());
		if (outputNumber < _paramArray.Count)
		{
			_paramArray.Insert(outputNumber, new List<double>(_paramArray[outputNumber]));
		}
		if (outputNumber < _curveNames.Count)
		{
			_curveNames.Insert(outputNumber, _curveNames[outputNumber]);
		}
		Change();
		return true;
	}

	public override bool DeleteObject(int index)
	{
		PreChange("DeleteObject");
		if (index >= _curves.Count)
		{
			return false;
		}
		_curves.RemoveAt(index);
		_paramArray.RemoveAt(index);
		_curveNames.RemoveAt(index);
		Change();
		Update();
		return true;
	}

	public override string GetOutputChannelIdentifier(int outputNumber)
	{
		if (outputNumber < 0)
		{
			return null;
		}
		if (outputNumber >= _curveNames.Count)
		{
			return null;
		}
		return _curveNames[outputNumber];
	}

	public override int ObjectCount()
	{
		return _curves.Count;
	}

	public override bool Read(ref OnBinaryArchive archive, CSParaGraph graph, int major, int minor)
	{
		base.Read(ref archive, graph, major, minor);
		_curves.Clear();
		_paramArray.Clear();
		_curveNames.Clear();
		NurbsCurve[] items = null;
		CSUtilIO.ReadArray(ref items, ref archive);
		if (items != null)
		{
			NurbsCurve[] array = items;
			foreach (NurbsCurve item in array)
			{
				_curves.Add(item);
			}
		}
		int num = 0;
		archive.ReadInt(ref num);
		for (int j = 0; j < num; j++)
		{
			_paramArray.Add(new List<double>());
			double[] items2 = null;
			CSUtilIO.ReadArray(ref items2, ref archive);
			if (items2 != null)
			{
				double[] array2 = items2;
				foreach (double item2 in array2)
				{
					_paramArray[_paramArray.Count - 1].Add(item2);
				}
			}
		}
		if (major >= 2 || (major == 1 && minor >= 1))
		{
			string[] items3 = null;
			if (!CSUtilIO.ReadArray(ref items3, ref archive))
			{
				return false;
			}
			if (items3 == null)
			{
				return true;
			}
			string[] array3 = items3;
			foreach (string item3 in array3)
			{
				_curveNames.Add(item3);
			}
		}
		return true;
	}

	public override bool ReadJson(JObject jBlock, CSParaGraph graph, int major, int minor)
	{
		base.ReadJson(jBlock, graph, major, minor);
		_curves.Clear();
		_paramArray.Clear();
		_curveNames.Clear();
		foreach (JObject item in (JArray)jBlock["Curves"])
		{
			_curves.Add((NurbsCurve)CSUtilIO.GeometryBaseFromString(item["Curve"].ToString()));
			_curveNames.Add(item["Name"].ToString());
			JArray obj = (JArray)item["Params"];
			List<double> list = new List<double>();
			foreach (JToken item2 in obj)
			{
				list.Add(Convert.ToDouble(item2));
			}
			_paramArray.Add(list);
		}
		return true;
	}

	public override bool SetStyle(CSObject setObject, bool showRewirePrompt = true)
	{
		base.SetStyle(setObject, showRewirePrompt);
		CSGroupOfMatrixProfiles cSGroupOfMatrixProfiles = setObject as CSGroupOfMatrixProfiles;
		_curves.Clear();
		_paramArray.Clear();
		_curveNames.Clear();
		if (cSGroupOfMatrixProfiles != null)
		{
			for (int i = 0; i < cSGroupOfMatrixProfiles._curves.Count; i++)
			{
				_curves.Add(cSGroupOfMatrixProfiles._curves[i].ToNurbsCurve());
			}
			for (int j = 0; j < cSGroupOfMatrixProfiles._paramArray.Count; j++)
			{
				_paramArray.Add(new List<double>());
				_paramArray[j].AddRange(cSGroupOfMatrixProfiles._paramArray[j]);
			}
			for (int k = 0; k < cSGroupOfMatrixProfiles._curveNames.Count; k++)
			{
				_curveNames.Add(cSGroupOfMatrixProfiles._curveNames[k]);
			}
		}
		return true;
	}

	public override bool SetUndo(CSObject setObject)
	{
		base.SetUndo(setObject);
		CSGroupOfMatrixProfiles cSGroupOfMatrixProfiles = setObject as CSGroupOfMatrixProfiles;
		_curves.Clear();
		_paramArray.Clear();
		_curveNames.Clear();
		if (cSGroupOfMatrixProfiles != null)
		{
			for (int i = 0; i < cSGroupOfMatrixProfiles._curves.Count; i++)
			{
				_curves.Add(cSGroupOfMatrixProfiles._curves[i].ToNurbsCurve());
			}
			for (int j = 0; j < cSGroupOfMatrixProfiles._paramArray.Count; j++)
			{
				_paramArray.Add(new List<double>());
				_paramArray[j].AddRange(cSGroupOfMatrixProfiles._paramArray[j]);
			}
			for (int k = 0; k < cSGroupOfMatrixProfiles._curveNames.Count; k++)
			{
				_curveNames.Add(cSGroupOfMatrixProfiles._curveNames[k]);
			}
		}
		return true;
	}

	public override bool Update()
	{
		base.OutputChannels.Clear();
		for (int i = 0; i < _curves.Count; i++)
		{
			CSProfileData data = ((_curveNames.Count > i) ? new CSProfileData(_curves[i].ToNurbsCurve(), _paramArray[i], Transform.Identity, _curveNames[i]) : new CSProfileData(_curves[i].ToNurbsCurve(), _paramArray[i], Transform.Identity, null));
			new CSOutputChannel($"{base.Identifier}_{i}", this, ChannelDataType.Profile, ChannelArrayType.Individual).SetData(data);
		}
		return base.Update();
	}

	public bool Update(int index)
	{
		if (base.OutputChannels.Count != _curves.Count)
		{
			return Update();
		}
		if (index >= base.OutputChannels.Count || index < 0)
		{
			return Update();
		}
		CSProfileData data = ((_curveNames.Count > index) ? new CSProfileData(_curves[index].ToNurbsCurve(), _paramArray[index], Transform.Identity, _curveNames[index]) : new CSProfileData(_curves[index].ToNurbsCurve(), _paramArray[index], Transform.Identity, null));
		new CSOutputChannel($"{base.Identifier}_{index}", this, ChannelDataType.Profile, ChannelArrayType.Individual).SetData(data);
		return base.Update();
	}

	public override bool UpdateGroupObject(int index, NurbsCurve curve, List<double> curveParameters)
	{
		PreChange("UpdateGroupObject");
		_curves[index] = curve.ToNurbsCurve();
		_paramArray[index].Clear();
		foreach (double curveParameter in curveParameters)
		{
			_paramArray[index].Add(curveParameter);
		}
		Change();
		Update(index);
		return true;
	}

	public override bool Write(ref OnBinaryArchive archive)
	{
		base.Write(ref archive);
		CSUtilIO.WriteArray(_curves.ToArray(), ref archive);
		archive.WriteInt(_curves.Count);
		for (int i = 0; i < _curves.Count; i++)
		{
			CSUtilIO.WriteArray(_paramArray[i].ToArray(), ref archive);
		}
		CSUtilIO.WriteArray(_curveNames.ToArray(), ref archive);
		return true;
	}

	public override void WriteJson(ref JObject archive)
	{
		base.WriteJson(ref archive);
		JArray jArray = new JArray();
		for (int i = 0; i < _curves.Count; i++)
		{
			JObject jObject = new JObject();
			jObject.Add("Name", _curveNames[i]);
			jObject.Add("Curve", CSUtilIO.GeometryBaseToString(_curves[i]));
			jObject.Add("Params", new JArray(_paramArray[i]));
			jArray.Add(jObject);
		}
		archive.Add("Curves", jArray);
	}
}
