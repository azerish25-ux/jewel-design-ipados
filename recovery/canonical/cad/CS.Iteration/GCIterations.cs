using System;
using System.Collections.Generic;
using System.IO;
using System.Xml.Serialization;
using CS.CSObjects;
using CS.CSObjects.CSTreeObjects;
using CS.Parametric;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace CS.Iteration;

[Serializable]
public sealed class GCIterations
{
	public List<GCOption> options = new List<GCOption>();

	public void CleanUp()
	{
		List<GCOption> list = new List<GCOption>();
		foreach (GCOption option in options)
		{
			if (option.GetIterationCount() == 0)
			{
				list.Add(option);
			}
		}
		foreach (GCOption item in list)
		{
			options.Remove(item);
		}
	}

	public void Clear()
	{
		options.Clear();
	}

	public GCIterations ConvertFromString(string data)
	{
		GCIterations gCIterations = null;
		XmlSerializer xmlSerializer = new XmlSerializer(GetType());
		using StringReader stringReader = new StringReader(data);
		gCIterations = (GCIterations)xmlSerializer.Deserialize(stringReader);
		stringReader.Close();
		return gCIterations;
	}

	public string ConvertToString()
	{
		XmlSerializer xmlSerializer = new XmlSerializer(GetType());
		string text = "";
		using StringWriter stringWriter = new StringWriter();
		xmlSerializer.Serialize(stringWriter, this);
		return stringWriter.ToString();
	}

	public GCOption FindGCOptionFromCSObject(CSObject obj)
	{
		foreach (GCOption option in options)
		{
			if (option.obj == obj)
			{
				return option;
			}
		}
		return null;
	}

	public void FindObjectsFromGuid()
	{
		Dictionary<Guid, CSObject> dictionary = new Dictionary<Guid, CSObject>();
		try
		{
			foreach (CSObject item in CSApplicationHelper.Application.Document.Graph.AllObjects())
			{
				if (item == null)
				{
					continue;
				}
				string text = item.UserData.FindValue(GCOption.UserDataType, GCOption.UserDataGuidName);
				if (item.Deleted || string.IsNullOrEmpty(text))
				{
					continue;
				}
				Guid guid = new Guid(text);
				if (object.Equals(guid, Guid.Empty))
				{
					continue;
				}
				if (dictionary.ContainsKey(guid))
				{
					CSPatchWireStart startObject = CSUtil.GetStartObject(item);
					if (startObject != null)
					{
						CSUtil.Print(string.Concat("Duplicate Guid Found:", guid, " : ", startObject.Identifier, ":", item.Identifier, "\n"));
					}
					else
					{
						CSUtil.Print(string.Concat("Duplicate Guid Found:", guid, " : ", item.Identifier, " : ", item.TableIndex, "\n"));
					}
					item.UserData.RemoveKey(GCOption.UserDataType, GCOption.UserDataGuidName);
				}
				else
				{
					dictionary.Add(guid, item);
				}
			}
		}
		catch (Exception ex)
		{
			CSUtil.Print(string.Concat(ex, "\n"));
		}
		foreach (GCOption option in options)
		{
			option.SetGuid(dictionary);
		}
		for (int i = 0; i < options.Count; i++)
		{
			if (options[i].obj == null)
			{
				options.RemoveAt(i);
				i--;
			}
		}
	}

	public List<int> GetIterationSettingsFromIndex(int index)
	{
		List<int> list = new List<int>();
		for (int i = 0; i < options.Count; i++)
		{
			int iterationCount = options[i].GetIterationCount();
			list.Add(index % iterationCount);
			index /= iterationCount;
		}
		return list;
	}

	public int GetTotalIterationCount()
	{
		int num = 1;
		if (options.Count == 0)
		{
			return 0;
		}
		foreach (GCOption option in options)
		{
			if (option.GetIterationCount() > 0)
			{
				num *= option.GetIterationCount();
			}
		}
		return num;
	}

	public bool ReadFromFile(string fileName)
	{
		XmlSerializer xmlSerializer = new XmlSerializer(GetType());
		GCIterations gCIterations = null;
		using (FileStream fileStream = new FileStream(fileName, FileMode.Open, FileAccess.Read))
		{
			gCIterations = (GCIterations)xmlSerializer.Deserialize(fileStream);
			fileStream.Close();
		}
		if (gCIterations == null)
		{
			return false;
		}
		options = gCIterations.options;
		return true;
	}

	public bool ReadFromUserData(CSUserData data)
	{
		string text = data.FindValue("GCIterations", "Iterations");
		if (text != null)
		{
			GCIterations gCIterations = ConvertFromString(text);
			if (gCIterations == null)
			{
				return false;
			}
			options = gCIterations.options;
			return true;
		}
		return false;
	}

	public bool RestoreOriginal()
	{
		bool flag = true;
		foreach (GCOption option in options)
		{
			flag &= option.RestoreOrginalToObject();
		}
		return flag;
	}

	public bool SaveToFile(string filePath)
	{
		XmlSerializer xmlSerializer = new XmlSerializer(GetType());
		using (FileStream fileStream = new FileStream(filePath, FileMode.Create))
		{
			xmlSerializer.Serialize(fileStream, this);
			fileStream.Close();
		}
		return true;
	}

	public bool SetIterationFromIndex(int index)
	{
		bool flag = true;
		for (int i = 0; i < options.Count; i++)
		{
			GCOption gCOption = options[i];
			int iterationCount = gCOption.GetIterationCount();
			if (iterationCount > 0)
			{
				flag &= gCOption.SetFromIteration(index % iterationCount);
				index /= iterationCount;
			}
			else
			{
				flag = false;
			}
		}
		return flag;
	}

	public bool StoreOriginal()
	{
		bool flag = true;
		foreach (GCOption option in options)
		{
			flag &= option.StoreOrginalFromObject();
		}
		return flag;
	}

	public override string ToString()
	{
		return $"Iteration - {ConvertToString()}";
	}

	public bool WriteToUserData(CSUserData data)
	{
		data.RemoveKey("GCIterations", "Iterations");
		if (options.Count < 1)
		{
			return false;
		}
		string value = ConvertToString();
		return data.AddKey("GCIterations", "Iterations", value);
	}

	public JArray WriteJson()
	{
		JsonSerializer jsonSerializer = new JsonSerializer();
		jsonSerializer.TypeNameHandling = TypeNameHandling.Auto;
		return JArray.FromObject(options, jsonSerializer);
	}

	public void ReadJSon(JToken jSon)
	{
		JsonSerializer jsonSerializer = new JsonSerializer();
		jsonSerializer.TypeNameHandling = TypeNameHandling.Auto;
		options = jSon.ToObject<List<GCOption>>(jsonSerializer);
	}
}
