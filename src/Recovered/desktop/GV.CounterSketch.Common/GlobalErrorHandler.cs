using System;
using System.IO;
using GV.CounterSketch.Shared;

namespace GV.CounterSketch.Common;

internal static class GlobalErrorHandler
{
	private static int _eventCounter;

	private static bool LoggingEnabled => true;

	static GlobalErrorHandler()
	{
		LogHelper.Log.Setup(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData), "Gemvision\\CounterSketch Studio"), "CounterSketch.log");
	}

	public static void WriteErrorToEventLog(string eventLogMessage)
	{
		try
		{
			_eventCounter++;
			if (LoggingEnabled)
			{
				LogHelper.Log.WriteLog("Error({0}): {1}", _eventCounter, eventLogMessage.Take(30000, ellipsis: true));
			}
		}
		catch (Exception)
		{
		}
	}

	public static void WriteInfoToEventLog(string eventLogMessage)
	{
		if (LoggingEnabled)
		{
			try
			{
				_eventCounter++;
				LogHelper.Log.WriteLog("Info({0}): {1}", _eventCounter, eventLogMessage.Take(30000, ellipsis: true));
			}
			catch (Exception)
			{
			}
		}
	}

	public static void WriteWarningToEventLog(string eventLogMessage)
	{
		try
		{
			_eventCounter++;
			if (LoggingEnabled)
			{
				LogHelper.Log.WriteLog("Warning({0}): {1}", _eventCounter, eventLogMessage.Take(30000, ellipsis: true));
			}
		}
		catch (Exception)
		{
		}
	}
}
