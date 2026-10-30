using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net;
using System.Runtime.CompilerServices;
using System.Text;
using System.Windows;
using GV.CounterSketch.Shared;
using GV.CounterSketch.Studio.Controls;
using GV.CounterSketch.Studio.Model.Objects.Enums;
using GV.CounterSketch.Studio.Utilities.Extensions;
using GV.CounterSketch.Studio.Utilities.Helpers;
using GalaSoft.MvvmLight;
using Localization;

namespace GV.CounterSketch.Studio;

internal static class GlobalErrorHandler
{
	private static int _eventCounter;

	private static int _eventLoggingVerbosity;

	private static bool _loggingEnabled;

	private static bool LoggingEnabled
	{
		get
		{
			if (!_loggingEnabled)
			{
				if (UIHelper.CounterSketchOptions != null && UIHelper.CounterSketchOptions.DebugMode)
				{
					_eventLoggingVerbosity = 2;
					_loggingEnabled = true;
				}
				else
				{
					string text = ConfigurationManager.AppSettings.Get("EventLoggingVerbosity");
					if (text != null)
					{
						int.TryParse(text, out _eventLoggingVerbosity);
						if (_eventLoggingVerbosity == 0)
						{
							_loggingEnabled = false;
						}
						else
						{
							_loggingEnabled = true;
						}
					}
				}
			}
			return _loggingEnabled;
		}
	}

	public static List<string> GetLogEntries()
	{
		return LogHelper.Log.ReadLogItems().Union(LogHelper.Log.ReadLogItems()).OrderByDescending(Log.GetDateTimeForEvent)
			.ToList();
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
		catch
		{
		}
		return stringBuilder.ToString();
	}

	public static void HandleException(object sender, Exception exception)
	{
		try
		{
			if (UIHelper.CounterSketchOptions.BypassErrorCatch)
			{
				return;
			}
			string text = $"An Application Error has occurred.\nEXCEPTION:\n{exception}";
			WriteErrorToEventLog(text);
			UIHelper.RaiseApplicationMessageEvent("Global Exception Handler", ApplicationMessageType.Exception, exception.ToString(), messageIsModal: false);
			EventHelper.RecordImmediateEvent(EventHelper.EventTypes.Error, exception.ToString(), sender.ToString());
			WindowMessage windowMessage = new WindowMessage();
			windowMessage.Width = 700.0;
			windowMessage.Height = 650.0;
			if (Application.Current != null)
			{
				Window mainWindow = Application.Current.MainWindow;
				if (mainWindow != null)
				{
					windowMessage.Owner = mainWindow;
				}
			}
			windowMessage.BringWindowToTop();
			windowMessage.ShowInTaskbar = true;
			windowMessage.WindowStartupLocation = WindowStartupLocation.CenterScreen;
			if (exception is WebException)
			{
				if (exception.Message.Contains("Could not establish trust relationship for the SSL/TLS secure channel."))
				{
					windowMessage.Message = Strings.WeCouldNotEstablishSecureConnectionToServers;
				}
			}
			else
			{
				windowMessage.Message = text;
			}
			windowMessage.ShowDialog();
		}
		catch
		{
		}
	}

	public static void WriteErrorToEventLog(string eventLogMessage)
	{
		try
		{
			_eventCounter++;
			if (LoggingEnabled)
			{
				LogHelper.Log.WriteLog("Err({0}): {1}", _eventCounter, eventLogMessage.Take(30000, ellipsis: true));
			}
		}
		catch (Exception)
		{
		}
	}

	public static void WriteInfoToEventLog(string eventLogMessage)
	{
		WriteInfoToEventLog(eventLogMessage, 1);
	}

	public static void WriteInfoToEventLog(string eventLogMessage, int loggingLevel)
	{
		if (ViewModelBase.IsInDesignModeStatic || !LoggingEnabled || loggingLevel > _eventLoggingVerbosity)
		{
			return;
		}
		try
		{
			_eventCounter++;
			LogHelper.Log.WriteLog("Inf({0}): {1}", _eventCounter, eventLogMessage.Take(30000, ellipsis: true));
		}
		catch (Exception)
		{
		}
	}

	public static void WriteMethodInfo([CallerMemberName] string caller = "UNKNOWN")
	{
		if (_eventLoggingVerbosity == 2)
		{
			try
			{
				_eventCounter++;
				LogHelper.Log.WriteLog("Inf({0}): [{1}]", _eventCounter, caller);
			}
			catch (Exception)
			{
			}
		}
	}

	public static void WriteMethodInfoOverride([CallerMemberName] string caller = "UNKNOWN")
	{
		try
		{
			_eventCounter++;
			LogHelper.Log.WriteLog("Inf({0}): [{1}]", _eventCounter, caller);
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
			if (LoggingEnabled)
			{
				LogHelper.Log.WriteLog("Wrn({0}): {1}", _eventCounter, eventLogMessage.Take(30000, ellipsis: true));
			}
		}
		catch (Exception)
		{
		}
	}
}
