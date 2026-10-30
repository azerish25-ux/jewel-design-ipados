using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.IO.Pipes;
using System.Linq;
using System.Threading;
using GV.CounterSketch.Shared;

namespace GemvisionApplicationManager.ApplicationService;

public static class ApplicationInteropHelper
{
	private static readonly string _counterSketchNamedPipesIdentifier;

	private static readonly int _maxNamedPipesRetryAttempts;

	private static bool _hasRequestedCounterSketchStart;

	private static string _loggingPrefix;

	private static string _designId;

	private static string _gem1;

	private static string _gem2;

	private static string _gem3;

	private static string _iterationIndex;

	private static string _iterationId;

	private static string _designVersion;

	private static string _fingerSizeUS;

	private static string _metal1;

	private static string _metal2;

	static ApplicationInteropHelper()
	{
		_counterSketchNamedPipesIdentifier = "{A8891938-3019-4120-A38E-3417D2013134}";
		_maxNamedPipesRetryAttempts = 5;
		_loggingPrefix = "\t\t\tApplicationInterop :";
		LogHelper.Log.Setup(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData), "Gemvision\\CounterSketch Studio"), "GemvisionApplicationManager.log");
	}

	public static bool ProcessApplicationAction(ApplicationMessage applicationMessage)
	{
		LogHelper.Log.WriteLog($"{_loggingPrefix} Processing Application Message - {applicationMessage}.");
		bool result = false;
		if (applicationMessage.ApplicationType == ApplicationType.CounterSketch && applicationMessage.ApplicationActionType == ApplicationActionType.OpenDesign)
		{
			List<string> source = applicationMessage.Message.Split(';').ToList();
			_designId = (from argument in source
				where argument.Split('=')[0].ToUpper() == "DESIGNID"
				select argument.Split('=')[1]).FirstOrDefault();
			_metal1 = (from argument in source
				where argument.Split('=')[0].ToUpper() == "METAL1"
				select argument.Split('=')[1]).FirstOrDefault() ?? "0";
			_metal2 = (from argument in source
				where argument.Split('=')[0].ToUpper() == "METAL2"
				select argument.Split('=')[1]).FirstOrDefault() ?? "0";
			_gem1 = (from argument in source
				where argument.Split('=')[0].ToUpper() == "GEM1"
				select argument.Split('=')[1]).FirstOrDefault() ?? "0";
			_gem2 = (from argument in source
				where argument.Split('=')[0].ToUpper() == "GEM2"
				select argument.Split('=')[1]).FirstOrDefault() ?? "0";
			_gem3 = (from argument in source
				where argument.Split('=')[0].ToUpper() == "GEM3"
				select argument.Split('=')[1]).FirstOrDefault() ?? "0";
			_iterationIndex = (from argument in source
				where argument.Split('=')[0].ToUpper() == "ITERATION"
				select argument.Split('=')[1]).FirstOrDefault();
			_iterationId = (from argument in source
				where argument.Split('=')[0].ToUpper() == "ITERATIONCOMPONENTID"
				select argument.Split('=')[1]).FirstOrDefault();
			_designVersion = (from argument in source
				where argument.Split('=')[0].ToUpper() == "DESIGNVERSION"
				select argument.Split('=')[1]).FirstOrDefault();
			_fingerSizeUS = (from argument in source
				where argument.Split('=')[0].ToUpper() == "FINGERSIZEUS"
				select argument.Split('=')[1]).FirstOrDefault();
			bool flag = false;
			int num = 0;
			do
			{
				LogHelper.Log.WriteLog($"{_loggingPrefix} Sending message to CounterSketch Attempt {num}.");
				flag = SendMessageToCounterSketch();
			}
			while (!flag && num++ <= _maxNamedPipesRetryAttempts);
			if (!flag)
			{
				GemvisionApplicationService.OnProcessedServiceEventNotification("Unable to Process Request", "CounterSketch did not respond in a reasonable timeframe.", ServiceEventStatus.Error);
			}
			result = true;
			_hasRequestedCounterSketchStart = false;
		}
		return result;
	}

	private static bool SendMessageToCounterSketch()
	{
		bool flag = false;
		try
		{
			using NamedPipeClientStream namedPipeClientStream = new NamedPipeClientStream(_counterSketchNamedPipesIdentifier);
			using StreamWriter streamWriter = new StreamWriter(namedPipeClientStream);
			if (!string.IsNullOrWhiteSpace(_designId) && Process.GetProcesses().FirstOrDefault((Process process) => process.ProcessName == "CounterSketch Studio") != null)
			{
				namedPipeClientStream.Connect(10000);
				if (namedPipeClientStream.IsConnected)
				{
					_hasRequestedCounterSketchStart = false;
					streamWriter.WriteLine("URLPROTOCOLREQUEST");
					streamWriter.WriteLine("DesignId={0}", _designId);
					streamWriter.WriteLine("Metal1={0}", _metal1);
					streamWriter.WriteLine("Metal2={0}", _metal2);
					streamWriter.WriteLine("Gem1={0}", _gem1);
					streamWriter.WriteLine("Gem2={0}", _gem2);
					streamWriter.WriteLine("Gem3={0}", _gem3);
					streamWriter.WriteLine("Iteration={0}", _iterationIndex);
					streamWriter.WriteLine("IterationComponentId={0}", _iterationId);
					streamWriter.WriteLine("DesignVersion={0}", _designVersion);
					streamWriter.WriteLine("FingerSizeUS={0}", _fingerSizeUS);
					flag = true;
				}
			}
		}
		catch (TimeoutException ex)
		{
			LogHelper.Log.WriteLog(_loggingPrefix + " TimeoutException sending message to CounterSketch.", ex);
		}
		catch (IOException ex2)
		{
			LogHelper.Log.WriteLog(_loggingPrefix + " IOException sending message to CounterSketch.", ex2);
		}
		catch (Exception ex3)
		{
			LogHelper.Log.WriteLog(_loggingPrefix + " Fell through to generic exception sending message to CounterSketch.", ex3);
		}
		finally
		{
			if (!flag && !_hasRequestedCounterSketchStart)
			{
				StartCounterSketch();
			}
		}
		return flag;
	}

	private static void StartCounterSketch()
	{
		LogHelper.Log.WriteLog(_loggingPrefix + " Wake up CounterSketch...");
		string text = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ProgramFiles), "CounterSketch Studio\\CounterSketch Studio.exe");
		LogHelper.Log.WriteLog(_loggingPrefix + " Starting CounterSketch at this path - " + text);
		Process.Start(text);
		Thread.Sleep(15000);
		_hasRequestedCounterSketchStart = true;
	}
}
