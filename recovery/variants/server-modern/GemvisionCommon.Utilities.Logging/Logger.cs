#define DEBUG
using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using GemvisionCommon.Helpers;

namespace GemvisionCommon.Utilities.Logging;

public sealed class Logger
{
	protected const string BlankMessage = "No message";

	private const int MaxLogAgeDays = 5;

	private const int MaxLogSize = 10485760;

	private readonly string _nameFileLog;

	private readonly string _pathDirLog;

	public string PathFileLog { get; }

	public Logger(string pathFileLog)
	{
		_nameFileLog = Path.GetFileName(pathFileLog);
		_pathDirLog = Path.GetDirectoryName(pathFileLog);
		PathFileLog = pathFileLog;
	}

	public static string GetLogMessageFormatted(DateTime timestamp, string msg, params object[] msgParams)
	{
		string logMessageFormatted = GetLogMessageFormatted(msg, msgParams);
		return $"[{timestamp}] {logMessageFormatted}";
	}

	public static string GetLogMessageFormatted(string msg, params object[] msgParams)
	{
		if (string.IsNullOrWhiteSpace(msg))
		{
			msg = "No message";
		}
		if (msgParams != null && msgParams.Any())
		{
			return string.Format(msg, msgParams);
		}
		return msg;
	}

	public static string GetLogMessageFromException(Exception e)
	{
		return GetLogMessageFromException(e, doRecurseInnerException: true);
	}

	public void WriteToLog(Exception e)
	{
		WriteToLog(DateTime.Now, e);
	}

	public void WriteToLog(DateTime timestamp, Exception e)
	{
		string logMessageFromException = GetLogMessageFromException(e);
		WriteToLog(timestamp, logMessageFromException);
	}

	public void WriteToLog(string msg, params object[] msgParams)
	{
		WriteToLog(DateTime.Now, msg, msgParams);
	}

	public void WriteToLog(DateTime timestamp, string msg, params object[] msgParams)
	{
		CheckRollLog();
		WriteToLogAux(timestamp, msg, msgParams);
	}

	protected virtual void WriteToLogAux(string msgFormatted)
	{
		if (!FileSystemHelper.ValidateDirectoryPath(_pathDirLog))
		{
			return;
		}
		try
		{
			using FileStream stream = new FileStream(PathFileLog, FileMode.Append, FileAccess.Write);
			using StreamWriter streamWriter = new StreamWriter(stream);
			streamWriter.WriteLine(msgFormatted);
		}
		catch (Exception e)
		{
			string logMessageFromException = GetLogMessageFromException(e);
			Debug.WriteLine(logMessageFromException);
		}
	}

	private void CheckRollLog()
	{
		FileInfo fileInfo = new FileInfo(PathFileLog);
		if (fileInfo.Exists && fileInfo.Length > 10485760)
		{
			RollLog(fileInfo);
		}
	}

	private static string CleanTimeString(string input)
	{
		return input.Replace(":", string.Empty).Replace("/", string.Empty).Replace("-", string.Empty);
	}

	private void DeleteOldLogs()
	{
		try
		{
			string[] files = Directory.GetFiles(_pathDirLog);
			files.ToList().ForEach(delegate(string file)
			{
				if (file.Contains($"-{_nameFileLog}"))
				{
					FileInfo fileInfo = new FileInfo(file);
					if (fileInfo.Exists)
					{
						WriteToLog("DeleteOldLogs: Found archived log file - {0}.", file);
						if (!(fileInfo.CreationTime.Date >= DateTime.Now.Date.AddDays(-5.0)))
						{
							WriteToLog("DeleteOldLogs: Age of log file exceeds {0}, deleting.", 5);
							fileInfo.Delete();
						}
					}
				}
			});
		}
		catch (Exception ex)
		{
			WriteToLog("DeleteOldLogs: Exception - {0}", ex);
		}
	}

	private static string GetLogMessageFromException(Exception e, bool doRecurseInnerException)
	{
		if (e == null)
		{
			return "No message";
		}
		StringBuilder stringBuilder = new StringBuilder();
		string message = e.Message;
		if (!string.IsNullOrWhiteSpace(message))
		{
			stringBuilder.AppendLine();
			stringBuilder.AppendLine(message);
		}
		string source = e.Source;
		if (!string.IsNullOrWhiteSpace(source))
		{
			stringBuilder.AppendLine();
			stringBuilder.AppendLine(source);
		}
		string stackTrace = e.StackTrace;
		if (!string.IsNullOrWhiteSpace(stackTrace))
		{
			stringBuilder.AppendLine();
			stringBuilder.AppendLine(stackTrace);
		}
		if (!doRecurseInnerException)
		{
			return stringBuilder.ToString();
		}
		if (e.InnerException == null)
		{
			return stringBuilder.ToString();
		}
		string logMessageFromException = GetLogMessageFromException(e.InnerException, doRecurseInnerException: false);
		if (string.IsNullOrWhiteSpace(logMessageFromException))
		{
			return stringBuilder.ToString();
		}
		stringBuilder.AppendLine("***** Inner Exception *****");
		stringBuilder.AppendLine(logMessageFromException);
		return stringBuilder.ToString();
	}

	private void RollLog(FileInfo fileInfo)
	{
		WriteToLogAux(DateTime.Now, "RollLog: Rolling log file; Current Size - {0}; Maximum Size - {1}.", fileInfo.Length, 10485760);
		DateTime now = DateTime.Now;
		string destFileName = Path.Combine(_pathDirLog, $"{CleanTimeString(now.ToShortDateString())}{CleanTimeString(now.ToShortTimeString())}-{_nameFileLog}");
		if (File.Exists(PathFileLog))
		{
			try
			{
				File.Move(PathFileLog, destFileName);
			}
			catch (Exception e)
			{
				WriteToLogAux(DateTime.Now, e);
			}
		}
		DeleteOldLogs();
	}

	private void WriteToLogAux(DateTime timestamp, Exception e)
	{
		string logMessageFromException = GetLogMessageFromException(e);
		WriteToLogAux(timestamp, logMessageFromException);
	}

	private void WriteToLogAux(DateTime timestamp, string msg, params object[] msgParams)
	{
		string logMessageFormatted = GetLogMessageFormatted(timestamp, msg, msgParams);
		WriteToLogAux(logMessageFormatted);
	}
}
