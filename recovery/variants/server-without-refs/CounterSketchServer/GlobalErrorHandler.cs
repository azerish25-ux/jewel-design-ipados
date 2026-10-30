using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.CompilerServices;
using System.Text;
using CounterSketchServer.Utilities.Extensions;
using GV.CounterSketch.Shared;

namespace CounterSketchServer;

internal static class GlobalErrorHandler
{
	private static int _eventCounter;

	static GlobalErrorHandler()
	{
		LogHelper.Log.Setup(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData), "Gemvision\\CounterSketch Studio"), "CounterSketchServer.log");
	}

	public static List<string> GetLogEntries()
	{
		List<string> list = LogHelper.Log.ReadLogItems();
		list.Reverse();
		return list;
	}

	public static string GetLogEntriesAsString()
	{
		StringBuilder stringBuilder = new StringBuilder();
		try
		{
			foreach (string logEntry in GetLogEntries())
			{
				stringBuilder.AppendLine(logEntry);
			}
		}
		catch (Exception)
		{
		}
		return stringBuilder.ToString();
	}

	public static void WriteErrorToEventLog(string eventLogMessage)
	{
		try
		{
			_eventCounter++;
			string message = $"Err({_eventCounter}): {eventLogMessage.Take(30000, ellipsis: true)}";
			LogHelper.Log.WriteLog(message);
		}
		catch (Exception)
		{
		}
	}

	public static void WriteInfoToEventLog(string eventLogMessage)
	{
		try
		{
			_eventCounter++;
			string message = $"Inf({_eventCounter}): {eventLogMessage.Take(30000, ellipsis: true)}";
			LogHelper.Log.WriteLog(message);
		}
		catch (Exception)
		{
		}
	}

	public static void WriteMethodInfo([CallerMemberName] string caller = "UNKNOWN")
	{
		try
		{
			_eventCounter++;
			string message = $"Inf({_eventCounter}): [{caller}]";
			LogHelper.Log.WriteLog(message);
		}
		catch (Exception)
		{
		}
	}

	public static void WriteWarningToEventLog(string eventLogMessage)
	{
		try
		{
			_eventCounter++;
			string message = $"Wrn({_eventCounter}): {eventLogMessage.Take(30000, ellipsis: true)}";
			LogHelper.Log.WriteLog(message);
		}
		catch (Exception)
		{
		}
	}
}
