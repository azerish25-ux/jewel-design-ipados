using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;

namespace GV.CounterSketch.Shared;

public class Log
{
	private string _logFileDirectory = string.Empty;

	private string _logFileName = string.Empty;

	private string _logFilePath = string.Empty;

	private static readonly object _syncObject = new object();

	public static DateTime GetDateTimeForEvent(string eventMessage)
	{
		DateTime result = default(DateTime);
		string text = eventMessage.Split(']').FirstOrDefault();
		if (text != null)
		{
			text = text.TrimStart('[');
			DateTime.TryParse(text, out result);
		}
		return result;
	}

	public string ReadLog()
	{
		if (string.IsNullOrWhiteSpace(_logFilePath))
		{
			return null;
		}
		string result = string.Empty;
		try
		{
			using FileStream stream = new FileStream(_logFilePath, FileMode.Open, FileAccess.Read);
			using StreamReader streamReader = new StreamReader(stream);
			result = streamReader.ReadToEnd();
		}
		catch (Exception)
		{
		}
		return result;
	}

	public List<string> ReadLogItems(bool includeSourceFileName = false)
	{
		if (string.IsNullOrWhiteSpace(_logFilePath))
		{
			return null;
		}
		List<string> list = new List<string>();
		try
		{
			using FileStream stream = new FileStream(_logFilePath, FileMode.Open, FileAccess.Read);
			using StreamReader streamReader = new StreamReader(stream);
			StringBuilder stringBuilder = new StringBuilder();
			while (!streamReader.EndOfStream)
			{
				string text = streamReader.ReadLine();
				if (text.StartsWith("["))
				{
					list.Add(string.Concat(stringBuilder, includeSourceFileName ? (", Source: " + _logFileName) : string.Empty));
					stringBuilder = new StringBuilder(text);
				}
				else
				{
					stringBuilder.Append(text);
				}
			}
		}
		catch (Exception)
		{
		}
		return list;
	}

	public void Setup(string logFileParentDirectory, string logFileName)
	{
		_logFileDirectory = logFileParentDirectory + "\\Logging";
		if (!Directory.Exists(_logFileDirectory))
		{
			Directory.CreateDirectory(_logFileDirectory);
		}
		_logFileName = logFileName;
		_logFilePath = Path.Combine(_logFileDirectory, _logFileName);
	}

	protected virtual bool IsFileLocked(string filePath)
	{
		FileStream fileStream = null;
		FileInfo fileInfo = new FileInfo(filePath);
		try
		{
			if (fileInfo.Exists)
			{
				fileStream = fileInfo.Open(FileMode.Open, FileAccess.Read, FileShare.None);
			}
		}
		catch (IOException)
		{
			return true;
		}
		finally
		{
			fileStream?.Close();
		}
		return false;
	}

	public void WriteLog(string message, params object[] parameters)
	{
		if (string.IsNullOrWhiteSpace(_logFilePath) || !Monitor.TryEnter(_syncObject))
		{
			return;
		}
		try
		{
			if (IsFileLocked(_logFilePath))
			{
				return;
			}
			CheckLogSizeForRoll(10485760);
			if (parameters != null && parameters.Any())
			{
				message = string.Format(message, parameters);
			}
			message = $"[{DateTime.Now}] {message}";
			using FileStream stream = new FileStream(_logFilePath, FileMode.Append, FileAccess.Write);
			using StreamWriter streamWriter = new StreamWriter(stream);
			streamWriter.WriteLine(message);
		}
		finally
		{
			Monitor.Exit(_syncObject);
		}
	}

	public void WriteLogBypassRollCheck(string message, params object[] parameters)
	{
		if (string.IsNullOrWhiteSpace(_logFilePath))
		{
			return;
		}
		if (parameters != null)
		{
			message = string.Format(message, parameters);
		}
		try
		{
			message = $"[{DateTime.Now}] {message}";
			using FileStream stream = new FileStream(_logFilePath, FileMode.Append, FileAccess.Write);
			using StreamWriter streamWriter = new StreamWriter(stream);
			streamWriter.WriteLine(message);
		}
		catch (Exception)
		{
		}
	}

	private void CheckLogSizeForRoll(int maxLogSize)
	{
		if (!string.IsNullOrWhiteSpace(_logFilePath))
		{
			FileInfo fileInfo = new FileInfo(_logFilePath);
			if (fileInfo.Exists && fileInfo.Length > maxLogSize)
			{
				WriteLogBypassRollCheck("CheckLogSizeForRoll: Rolling existing log file; Current Size - {0}; Maximum Size - {1}.", fileInfo.Length, maxLogSize);
				RollLog();
			}
		}
	}

	private static string CleanTimeString(string input)
	{
		return input.Replace(":", string.Empty).Replace("/", string.Empty).Replace("-", string.Empty);
	}

	private void DeleteOldLogs(int maxLogAge)
	{
		if (string.IsNullOrWhiteSpace(_logFilePath))
		{
			return;
		}
		try
		{
			Directory.GetFiles(Path.GetDirectoryName(_logFilePath)).ToList().ForEach(delegate(string file)
			{
				if (file.Contains("-" + _logFileName))
				{
					FileInfo fileInfo = new FileInfo(file);
					if (fileInfo.Exists)
					{
						WriteLog("DeleteOldLogs: Found archived log file - {0}.", file);
						if (fileInfo.CreationTime.Date < DateTime.Now.Date.AddDays(maxLogAge * -1))
						{
							WriteLog("DeleteOldLogs: Age of log file exceeds {0}, deleting.", maxLogAge);
							fileInfo.Delete();
						}
					}
				}
			});
		}
		catch (Exception ex)
		{
			WriteLog("DeleteOldLogs: Exception - {0}", ex);
		}
	}

	private void RollLog()
	{
		if (!string.IsNullOrWhiteSpace(_logFilePath))
		{
			string text = Path.Combine(_logFileDirectory, $"{CleanTimeString(DateTime.Now.ToShortDateString())}{CleanTimeString(DateTime.Now.ToShortTimeString())}-{_logFileName}");
			if (File.Exists(_logFilePath))
			{
				WriteLogBypassRollCheck("RollLog: Rolling existing log file - {0}.", text);
				File.Move(_logFilePath, text);
			}
			DeleteOldLogs(5);
		}
	}
}
