using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml.Serialization;
using Newtonsoft.Json.Linq;
using RMA.OpenNURBS;

namespace CS.Parametric;

public sealed class CSUserData : IEnumerable<UserDataElement>, IEnumerable
{
	private List<UserDataElement> elements = new List<UserDataElement>();

	public List<Tuple<string, string, string>> AllDataElements
	{
		get
		{
			List<Tuple<string, string, string>> dataElements = new List<Tuple<string, string, string>>();
			elements?.ForEach(delegate(UserDataElement _)
			{
				dataElements.Add(new Tuple<string, string, string>(_.Type, _.Key, _.Value));
			});
			return dataElements;
		}
	}

	public CSUserData()
	{
	}

	public CSUserData(CSUserData copyObject)
	{
		elements.AddRange(copyObject.elements);
	}

	public IEnumerator<UserDataElement> GetEnumerator()
	{
		return elements.GetEnumerator();
	}

	IEnumerator IEnumerable.GetEnumerator()
	{
		return ((IEnumerable)elements).GetEnumerator();
	}

	public bool Add(CSUserData data)
	{
		if (data == null)
		{
			return false;
		}
		foreach (UserDataElement element in data.elements)
		{
			AddKeyNoReplace(element.Type, element.Key, element.Value);
		}
		return true;
	}

	public bool AddKey(string type, string key, string value)
	{
		if (type == null || key == null || value == null)
		{
			return false;
		}
		lock (elements)
		{
			RemoveKey(type, key);
			elements.Add(new UserDataElement(type, key, value));
		}
		return true;
	}

	public bool AddKeyNoReplace(string type, string key, string value)
	{
		if (type == null || key == null || value == null)
		{
			return false;
		}
		if (FindValue(type, key) != null)
		{
			return false;
		}
		elements.Add(new UserDataElement(type, key, value));
		return true;
	}

	public bool AddSerializedObject<T>(string type, string key, T serializableObject)
	{
		bool result = false;
		try
		{
			XmlSerializer xmlSerializer = new XmlSerializer(typeof(T));
			StringWriter stringWriter = new StringWriter();
			xmlSerializer.Serialize(stringWriter, serializableObject);
			result = AddKey(type, key, stringWriter.ToString());
		}
		catch (Exception)
		{
		}
		return result;
	}

	public IEnumerable<UserDataElement> AllKeys()
	{
		for (int i = 0; i < elements.Count; i++)
		{
			yield return elements[i];
		}
	}

	public bool Clear()
	{
		elements.Clear();
		return true;
	}

	public IEnumerable<UserDataElement> ElementsThatMatchType(string type)
	{
		for (int i = 0; i < elements.Count; i++)
		{
			if (elements[i].Type.CompareTo(type) == 0)
			{
				yield return elements[i];
			}
		}
	}

	public bool FindBool(string type, string key, bool defaultValue = false)
	{
		if (string.IsNullOrEmpty(FindValue(type, key)))
		{
			return defaultValue;
		}
		if (!bool.TryParse(FindValue(type, key), out var result))
		{
			return defaultValue;
		}
		return result;
	}

	public bool FindBool(string type, string key, out bool value)
	{
		if (FindValue(type, key, out var value2))
		{
			return bool.TryParse(value2, out value);
		}
		value = false;
		return false;
	}

	public bool FindInt(string type, string key, out int value)
	{
		if (FindValue(type, key, out var value2))
		{
			return int.TryParse(value2, out value);
		}
		value = 0;
		return false;
	}

	public int FindInt(string type, string key)
	{
		int.TryParse(FindValue(type, key), out var result);
		return result;
	}

	public T FindSerializedObject<T>(string type, string key)
	{
		T result = default(T);
		string text = FindValue(type, key);
		if (!string.IsNullOrWhiteSpace(text))
		{
			try
			{
				XmlSerializer xmlSerializer = new XmlSerializer(typeof(T));
				StringReader textReader = new StringReader(text);
				result = (T)xmlSerializer.Deserialize(textReader);
				return result;
			}
			catch (Exception ex)
			{
				CSUtil.PrintLine($"Problem Deserializing {type} : {key} = {typeof(T)} : {ex}");
			}
		}
		return result;
	}

	public string FindValue(string type, string key)
	{
		return (from el in elements
			where string.Equals(el.Type, type) && string.Equals(el.Key, key)
			select el.Value).FirstOrDefault();
	}

	public bool FindValue(string type, string key, out string value)
	{
		List<UserDataElement> list = elements.Where((UserDataElement el) => string.Equals(el.Type, type) && string.Equals(el.Key, key)).ToList();
		bool flag = list.Any();
		value = (flag ? list[0].Value : null);
		return flag;
	}

	public void ReadJSon(JToken jSon)
	{
		elements = jSon.ToObject<List<UserDataElement>>();
	}

	public bool Read(ref OnBinaryArchive archive)
	{
		bool flag = true;
		elements.Clear();
		int num = 0;
		flag &= archive.ReadInt(ref num);
		elements = new List<UserDataElement>(num);
		for (int i = 0; i < num; i++)
		{
			elements.Add(new UserDataElement());
			flag &= elements[i].Read(ref archive);
		}
		return flag;
	}

	public bool RemoveAllDataOfType(string type)
	{
		bool result = false;
		foreach (UserDataElement item in elements.ToList())
		{
			if (item.Type.CompareTo(type) == 0)
			{
				elements.Remove(item);
				result = true;
			}
		}
		return result;
	}

	public bool RemoveKey(string type, string key)
	{
		foreach (UserDataElement element in elements)
		{
			if (element.Type.CompareTo(type) == 0 && element.Key.CompareTo(key) == 0)
			{
				elements.Remove(element);
				return true;
			}
		}
		return false;
	}

	public override string ToString()
	{
		if (elements.Count == 0)
		{
			return "";
		}
		string text = "";
		for (int i = 0; i < elements.Count; i++)
		{
			text = text + elements[i].Type + ":" + elements[i].Key + "=" + elements[i].Value + "\n";
		}
		return text;
	}

	public bool Write(ref OnBinaryArchive archive)
	{
		bool flag = true;
		flag &= archive.WriteInt(elements.Count);
		for (int i = 0; i < elements.Count; i++)
		{
			flag &= elements[i].Write(ref archive);
		}
		return flag;
	}

	public JArray WriteJSon()
	{
		return JArray.FromObject(elements);
	}
}
