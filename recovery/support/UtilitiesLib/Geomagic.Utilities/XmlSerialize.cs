using System;
using System.IO;
using System.Reflection;
using System.Text;
using System.Xml;
using System.Xml.Serialization;

namespace Geomagic.Utilities;

public class XmlSerialize
{
	private static Encoding _Encoding = Encoding.UTF8;

	private static Formatting _Formatting = Formatting.Indented;

	private static bool _ResolveWorkingDirectory = false;

	private static ResolveEventHandler WorkingDirectoryResolver = delegate(object sender, ResolveEventArgs args)
	{
		Assembly result = null;
		string text = args.Name.Split(',')[0];
		Assembly[] assemblies = AppDomain.CurrentDomain.GetAssemblies();
		Assembly[] array = assemblies;
		foreach (Assembly assembly in array)
		{
			if (text == assembly.FullName.Split(',')[0])
			{
				result = assembly;
				break;
			}
		}
		return result;
	};

	public static Encoding Encoding
	{
		get
		{
			return _Encoding;
		}
		set
		{
			_Encoding = value;
		}
	}

	public static Formatting Formatting
	{
		get
		{
			return _Formatting;
		}
		set
		{
			_Formatting = value;
		}
	}

	public static bool ResolveWorkingDirectory
	{
		get
		{
			return _ResolveWorkingDirectory;
		}
		set
		{
			_ResolveWorkingDirectory = value;
		}
	}

	private static void AddResolvers()
	{
		if (ResolveWorkingDirectory)
		{
			AppDomain.CurrentDomain.AssemblyResolve += WorkingDirectoryResolver;
		}
	}

	private static void RemoveResolvers()
	{
		if (ResolveWorkingDirectory)
		{
			AppDomain.CurrentDomain.AssemblyResolve -= WorkingDirectoryResolver;
		}
	}

	public static void ToFile(object ToSerialize, string Dest)
	{
		Type type = ToSerialize.GetType();
		using XmlTextWriter xmlTextWriter = new XmlTextWriter(Dest, Encoding);
		xmlTextWriter.Formatting = Formatting;
		XmlSerializer xmlSerializer = new XmlSerializer(type);
		xmlSerializer.Serialize(xmlTextWriter, ToSerialize);
	}

	public static string ToString(object ToSerialize)
	{
		string text = null;
		Type type = ToSerialize.GetType();
		using MemoryStream memoryStream = new MemoryStream();
		using XmlTextWriter xmlTextWriter = new XmlTextWriter(memoryStream, Encoding);
		xmlTextWriter.Formatting = Formatting;
		XmlSerializer xmlSerializer = new XmlSerializer(type);
		xmlSerializer.Serialize(xmlTextWriter, ToSerialize);
		return Encoding.GetString(memoryStream.GetBuffer());
	}

	public static T FromFile<T>(string Source)
	{
		T val = default(T);
		AddResolvers();
		try
		{
			using XmlTextReader xmlReader = new XmlTextReader(Source);
			XmlSerializer xmlSerializer = new XmlSerializer(typeof(T));
			return (T)xmlSerializer.Deserialize(xmlReader);
		}
		finally
		{
			RemoveResolvers();
		}
	}

	public static T FromString<T>(string Xml)
	{
		T val = default(T);
		AddResolvers();
		try
		{
			Xml = Xml.Trim();
			using MemoryStream input = new MemoryStream(Encoding.GetBytes(Xml));
			using XmlTextReader xmlReader = new XmlTextReader(input);
			XmlSerializer xmlSerializer = new XmlSerializer(typeof(T));
			return (T)xmlSerializer.Deserialize(xmlReader);
		}
		finally
		{
			RemoveResolvers();
		}
	}
}
