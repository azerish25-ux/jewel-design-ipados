using System;
using System.IO;
using System.Xml.Serialization;
using GemvisionCommon.Helpers;
using GemvisionCommon.Utilities.Logging;

namespace GemvisionCommon.Utilities.Serialization;

public class Serializer<T> where T : class, new()
{
	public delegate void SerializerExceptionHandler(Exception ex);

	private SerializerExceptionHandler _onSerializerException;

	private readonly Logger _logger;

	public SerializerExceptionHandler OnSerializerException
	{
		set
		{
			_onSerializerException = value;
		}
	}

	public Serializer()
	{
	}

	public Serializer(Logger logger)
	{
		_logger = logger;
	}

	public bool Read(string pathFile, out T classToDeserialze)
	{
		classToDeserialze = null;
		try
		{
			if (File.Exists(pathFile))
			{
				using FileStream stream = File.OpenRead(pathFile);
				XmlSerializer xmlSerializer = new XmlSerializer(typeof(T));
				classToDeserialze = xmlSerializer.Deserialize(stream) as T;
			}
		}
		catch (Exception ex)
		{
			_logger?.WriteToLog(ex);
			_onSerializerException?.Invoke(ex);
			return false;
		}
		return classToDeserialze != null;
	}

	public bool Write(string pathFile, T classToSerialze)
	{
		try
		{
			if (!FileSystemHelper.ValidateDirectoryPath(pathFile))
			{
				return false;
			}
			using FileStream fileStream = File.Open(pathFile, FileMode.Create, FileAccess.Write);
			XmlSerializer xmlSerializer = new XmlSerializer(typeof(T));
			xmlSerializer.Serialize(fileStream, classToSerialze);
			fileStream.Flush();
		}
		catch (Exception ex)
		{
			_logger?.WriteToLog(ex);
			_onSerializerException?.Invoke(ex);
			return false;
		}
		return true;
	}
}
