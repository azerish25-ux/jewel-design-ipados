using System;
using System.IO;
using System.Reflection;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;

namespace Geomagic.Utilities;

public class BinarySerializer
{
	private static SerializationBinder _Binder = null;

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

	public static SerializationBinder Binder
	{
		get
		{
			return _Binder;
		}
		set
		{
			_Binder = value;
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
		using Stream serializationStream = new FileStream(Dest, FileMode.Create);
		IFormatter formatter = new BinaryFormatter();
		formatter.Serialize(serializationStream, ToSerialize);
	}

	public static byte[] ToBytes(object ToSerialize)
	{
		byte[] array = null;
		using MemoryStream memoryStream = new MemoryStream();
		IFormatter formatter = new BinaryFormatter();
		formatter.Serialize(memoryStream, ToSerialize);
		return memoryStream.ToArray();
	}

	public static T FromFile<T>(string Source)
	{
		T val = default(T);
		AddResolvers();
		try
		{
			using Stream serializationStream = new FileStream(Source, FileMode.Open);
			IFormatter formatter = new BinaryFormatter();
			formatter.Binder = Binder;
			return (T)formatter.Deserialize(serializationStream);
		}
		finally
		{
			RemoveResolvers();
		}
	}

	public static T FromBytes<T>(byte[] Bytes)
	{
		T val = default(T);
		AddResolvers();
		try
		{
			using MemoryStream serializationStream = new MemoryStream(Bytes);
			IFormatter formatter = new BinaryFormatter();
			formatter.Binder = Binder;
			return (T)formatter.Deserialize(serializationStream);
		}
		finally
		{
			RemoveResolvers();
		}
	}
}
