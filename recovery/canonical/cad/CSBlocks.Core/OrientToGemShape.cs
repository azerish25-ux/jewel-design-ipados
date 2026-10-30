using System;
using System.IO;
using System.Xml.Serialization;
using CS.JSonDoc;
using Newtonsoft.Json.Linq;
using RMA.OpenNURBS;

namespace CSBlocks.Core;

[Serializable]
[XmlRoot("OrientToGemShape")]
public sealed class OrientToGemShape
{
	public ScaleToGemShape[] scaleToGemShape;

	[XmlAttribute]
	public string Mounting;

	public bool FindShape(string shape, ref double x, ref double y)
	{
		for (int i = 0; i < scaleToGemShape.Length; i++)
		{
			if (scaleToGemShape[i] != null && scaleToGemShape[i].shape.CompareTo(shape) == 0)
			{
				x = scaleToGemShape[i].x;
				y = scaleToGemShape[i].y;
				return true;
			}
		}
		return false;
	}

	public bool ReadJson(JObject obj)
	{
		Mounting = obj["Mounting"].ToString();
		JArray jArray = (JArray)obj["Shapes"];
		scaleToGemShape = new ScaleToGemShape[jArray.Count];
		for (int i = 0; i < jArray.Count; i++)
		{
			JObject archive = (JObject)jArray[i];
			scaleToGemShape[i] = new ScaleToGemShape();
			archive.ReadJsonValue<string>("Shape", out scaleToGemShape[i].shape);
			archive.ReadJsonValue<double>("X", out scaleToGemShape[i].x);
			archive.ReadJsonValue<double>("Y", out scaleToGemShape[i].y);
		}
		return true;
	}

	public bool Read(OnBinaryArchive archive)
	{
		archive.ReadString(ref Mounting);
		int i = 0;
		archive.ReadInt(ref i);
		if (i < 0)
		{
			return false;
		}
		scaleToGemShape = new ScaleToGemShape[i];
		for (int j = 0; j < scaleToGemShape.Length; j++)
		{
			scaleToGemShape[j] = new ScaleToGemShape();
			archive.ReadString(ref scaleToGemShape[j].shape);
			archive.ReadDouble(ref scaleToGemShape[j].x);
			archive.ReadDouble(ref scaleToGemShape[j].y);
		}
		return true;
	}

	public static OrientToGemShape ReadFromFile(string fileName)
	{
		XmlSerializer xmlSerializer = new XmlSerializer(typeof(OrientToGemShape));
		FileStream fileStream = new FileStream(fileName, FileMode.Open, FileAccess.Read);
		OrientToGemShape result = null;
		try
		{
			result = (OrientToGemShape)xmlSerializer.Deserialize(fileStream);
		}
		catch (Exception)
		{
		}
		fileStream.Close();
		return result;
	}

	public void ReadFromString()
	{
	}

	public static OrientToGemShape ReadFromString(string str)
	{
		XmlSerializer xmlSerializer = new XmlSerializer(typeof(OrientToGemShape));
		TextReader textReader = new StringReader(str);
		OrientToGemShape result = null;
		try
		{
			result = (OrientToGemShape)xmlSerializer.Deserialize(textReader);
		}
		catch (Exception)
		{
		}
		return result;
	}

	public JObject WriteJson()
	{
		JArray jArray = new JArray();
		ScaleToGemShape[] array = this.scaleToGemShape;
		foreach (ScaleToGemShape scaleToGemShape in array)
		{
			JObject item = new JObject
			{
				{ "Shape", scaleToGemShape.shape },
				{ "X", scaleToGemShape.x },
				{ "Y", scaleToGemShape.y }
			};
			jArray.Add(item);
		}
		return new JObject
		{
			{ "Mounting", Mounting },
			{ "Shapes", jArray }
		};
	}

	public bool Write(OnBinaryArchive archive)
	{
		archive.WriteString(Mounting);
		archive.WriteInt(scaleToGemShape.Length);
		for (int i = 0; i < scaleToGemShape.Length; i++)
		{
			archive.WriteString(scaleToGemShape[i].shape);
			archive.WriteDouble(scaleToGemShape[i].x);
			archive.WriteDouble(scaleToGemShape[i].y);
		}
		return true;
	}
}
