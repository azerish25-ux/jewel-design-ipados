using System;
using System.Collections.Generic;
using System.IO;
using System.Xml;
using System.Xml.Serialization;
using CS.CSObjects;
using CS.CSValues;

namespace CS.Parametric;

public sealed class CAMValueSettings
{
	public int BlockLength
	{
		get
		{
			return Settings.DesignInfo.SignatureLength;
		}
		set
		{
			Settings.DesignInfo.SignatureLength = value;
		}
	}

	public int DesignID
	{
		get
		{
			return Settings.DesignInfo.DesignId;
		}
		set
		{
			Settings.DesignInfo.DesignId = value;
		}
	}

	public int Version
	{
		get
		{
			return Settings.DesignInfo.Version;
		}
		set
		{
			Settings.DesignInfo.Version = value;
		}
	}

	public CAMSetting Settings { get; set; } = new CAMSetting();

	public string Signature
	{
		get
		{
			return Settings.DesignInfo.Signature;
		}
		set
		{
			Settings.DesignInfo.Signature = value;
		}
	}

	public string FileSignature { get; set; }

	public CAMValueSettings(int nDesignID, int nVersion, string strSignature, int nBlockLength)
	{
		FileSignature = Path.GetTempFileName();
		Settings = new CAMSetting();
		Settings.DesignInfo.Signature = strSignature;
		Settings.DesignInfo.SignatureLength = nBlockLength;
		Settings.DesignInfo.DesignId = nDesignID;
		Settings.DesignInfo.Version = nVersion;
	}

	public byte[] GetBytes()
	{
		if (Settings.Values.Count > 0)
		{
			XmlSerializer xmlSerializer = new XmlSerializer(typeof(CAMSetting));
			using MemoryStream memoryStream = new MemoryStream();
			using XmlWriter xmlWriter = XmlWriter.Create(memoryStream);
			xmlSerializer.Serialize(xmlWriter, Settings);
			return memoryStream.ToArray();
		}
		return null;
	}

	public void GetSettings(List<CSObject> objects)
	{
		Settings.Values.Clear();
		foreach (CSObject @object in objects)
		{
			if (@object is CSValue)
			{
				CSValue cSValue = @object as CSValue;
				if (cSValue.HasCAM())
				{
					CAMValue cAMValue = new CAMValue();
					cAMValue.PatchWire = @object.ParentPatchWire.Identifier;
					cAMValue.Item = @object.Identifier;
					cAMValue.Absolute = cSValue.CAMAbsolute;
					cAMValue.Delta = cSValue.CAMDelta;
					cAMValue.IsAbsolute = cSValue.CAMIsAbsolute;
					cAMValue.Note = cSValue.CAMNote;
					cAMValue.Index = objects.IndexOf(@object);
					Settings.Values.Add(cAMValue);
				}
			}
		}
	}

	public bool Read()
	{
		bool result = false;
		try
		{
			if (File.Exists(FileSignature))
			{
				result = true;
				using FileStream stream = File.OpenRead(FileSignature);
				CAMSetting settings = new XmlSerializer(typeof(CAMSetting)).Deserialize(stream) as CAMSetting;
				Settings = settings;
			}
		}
		catch (Exception)
		{
			result = false;
			Settings = null;
		}
		if (Settings == null)
		{
			Settings = new CAMSetting();
		}
		return result;
	}

	public bool ReadBytes(byte[] bytes)
	{
		bool result;
		try
		{
			using (MemoryStream memoryStream = new MemoryStream())
			{
				memoryStream.Write(bytes, 0, bytes.Length);
				memoryStream.Position = 0L;
				XmlSerializer xmlSerializer = new XmlSerializer(typeof(CAMSetting));
				Settings = xmlSerializer.Deserialize(memoryStream) as CAMSetting;
			}
			result = true;
		}
		catch (Exception)
		{
			result = false;
			Settings = null;
		}
		if (Settings == null)
		{
			Settings = new CAMSetting();
		}
		return result;
	}

	public bool Write()
	{
		bool result = false;
		if (Settings.Values.Count > 0)
		{
			using FileStream stream = File.Open(FileSignature, FileMode.Create, FileAccess.Write);
			new XmlSerializer(typeof(CAMSetting)).Serialize(stream, Settings);
			result = true;
		}
		return result;
	}
}
