using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Json;
using System.Xml.Serialization;
using Newtonsoft.Json;

namespace GV.CounterSketch.Studio.Utilities;

public class SerializeHelper<T> where T : class, new()
{
	public T ReadXMLSettings(string path)
	{
		try
		{
			if (File.Exists(path))
			{
				using FileStream stream = File.OpenRead(path);
				return new XmlSerializer(typeof(T)).Deserialize(stream) as T;
			}
			Directory.CreateDirectory(Path.GetDirectoryName(path));
		}
		catch (Exception)
		{
		}
		return new T();
	}

	public T ReadJSONSettings(string path)
	{
		try
		{
			if (File.Exists(path))
			{
				using FileStream stream = File.OpenRead(path);
				if (new DataContractJsonSerializer(typeof(T)).ReadObject(stream) is T result)
				{
					return result;
				}
			}
			else
			{
				Directory.CreateDirectory(Path.GetDirectoryName(path));
			}
		}
		catch (Exception)
		{
		}
		return new T();
	}

	public List<T> ReadListFromJSON(string path)
	{
		if (path != null && File.Exists(path))
		{
			return JsonConvert.DeserializeObject<List<T>>(File.ReadAllText(path));
		}
		return new List<T>();
	}

	public void WriteXMLSettings(string path, T classToSerialze)
	{
		using FileStream stream = File.Open(path, FileMode.Create, FileAccess.Write);
		new XmlSerializer(typeof(T)).Serialize(stream, classToSerialze);
	}

	public void WriteJSONSettings(string path, T classToSerialze)
	{
		using FileStream stream = File.Open(path, FileMode.Create, FileAccess.Write);
		new DataContractJsonSerializer(typeof(T)).WriteObject(stream, classToSerialze);
	}

	public void WriteListToJSON(string path, List<T> classesToSerialize)
	{
		string contents = JsonConvert.SerializeObject(classesToSerialize, Formatting.Indented);
		if (File.Exists(path))
		{
			File.SetAttributes(path, FileAttributes.Normal);
			File.Delete(path);
		}
		File.AppendAllText(path, contents);
	}
}
