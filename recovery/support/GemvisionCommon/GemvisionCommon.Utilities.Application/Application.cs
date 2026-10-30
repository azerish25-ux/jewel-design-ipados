using System;
using System.Diagnostics;
using System.IO;
using GemvisionCommon.Helpers;
using GemvisionCommon.Utilities.Logging;
using GemvisionCommon.Utilities.Serialization;

namespace GemvisionCommon.Utilities.Application;

public class Application<T> where T : class, new()
{
	public class OnAfterGetDefaultSettingsEventArgs : EventArgs
	{
		public string StackTrace { get; }

		public OnAfterGetDefaultSettingsEventArgs()
		{
			StackTrace = new StackTrace().ToString();
		}

		public override string ToString()
		{
			return StackTrace;
		}
	}

	private readonly string _filePathOptions;

	private T _options;

	public Logger Logger { get; }

	public T Options
	{
		get
		{
			if (_options == null)
			{
				ReadOptions();
			}
			return _options;
		}
	}

	public event EventHandler<EventArgs> OnAfterGetDefaultSettings;

	public Application(string filePathLog, string filePathOptions)
	{
		_filePathOptions = filePathOptions;
		Logger = new Logger(filePathLog);
	}

	public T ReadOptions()
	{
		if (File.Exists(_filePathOptions) && RetryHelper.Do((Func<bool>)ReadOptionsAux, 3))
		{
			return _options;
		}
		_options = new T();
		RaiseEvent(this.OnAfterGetDefaultSettings, new OnAfterGetDefaultSettingsEventArgs());
		WriteOptions();
		return _options;
	}

	public bool WriteOptions()
	{
		if (_options == null)
		{
			return false;
		}
		return RetryHelper.Do((Func<bool>)WriteOptionsAux, 3);
	}

	public bool WriteReport(string pathFileReport)
	{
		return WriteOptions() && FileSystemHelper.CopyFile(_filePathOptions, pathFileReport, doOverwrite: true);
	}

	private void RaiseEvent(EventHandler<EventArgs> whichEvent, EventArgs e)
	{
		whichEvent?.Invoke(this, e);
	}

	private bool ReadOptionsAux()
	{
		Serializer<T> serializer = new Serializer<T>(Logger);
		if (!serializer.Read(_filePathOptions, out var classToDeserialze))
		{
			return false;
		}
		if (classToDeserialze == null)
		{
			return false;
		}
		_options = classToDeserialze;
		return true;
	}

	private bool WriteOptionsAux()
	{
		Serializer<T> serializer = new Serializer<T>(Logger);
		return serializer.Write(_filePathOptions, _options);
	}
}
