using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Timers;
using System.Xml.Linq;
using GV.CounterSketch.Shared;

namespace GemvisionApplicationManager.ApplicationService;

public class GemvisionApplicationService
{
	public delegate void ProcessedServiceEventNotificationDelegate(ServiceEventNotificationArgs e);

	private static string _counterSketchIdentifier = string.Empty;

	private static bool _isExecuting;

	private static bool _isShutDown;

	private static Timer _timer;

	private static readonly int _timerFrequency = 15000;

	private static string _loggingPrefix = "\tService :";

	private static List<IApplicationServiceEvent> _applicationServiceEvents { get; set; }

	public static event ProcessedServiceEventNotificationDelegate ProcessedServiceEventNotification;

	public static void ExecuteApplicationEvents()
	{
		LogHelper.Log.WriteLog(_loggingPrefix + " Timer Kicked, Executing Application Service Events.");
		_applicationServiceEvents.Where((IApplicationServiceEvent _) => !_.IsExecuting).ToList().ForEach(delegate(IApplicationServiceEvent _)
		{
			_.Execute();
		});
	}

	public static void GetApplicationEvents()
	{
		LogHelper.Log.WriteLog(_loggingPrefix + " Getting configured service events.");
		_applicationServiceEvents = new List<IApplicationServiceEvent>();
		if (_counterSketchIdentifier != string.Empty)
		{
			LogHelper.Log.WriteLog(_loggingPrefix + " Adding RemoteMessageAppService Event for CounterSketch.");
			_applicationServiceEvents.Add(new RemoteMessageApplicationServiceEvent(_counterSketchIdentifier));
		}
	}

	public static string GetInstalledCounterSketchIdentifier()
	{
		string text = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData), "Gemvision\\CounterSketch Studio\\CounterSketchApplicationSettings.xml");
		if (File.Exists(text))
		{
			XElement xElement = XDocument.Load(text).Root?.Elements("KeyId").FirstOrDefault();
			if (xElement != null)
			{
				return xElement.Value;
			}
		}
		return string.Empty;
	}

	public static void Initialize()
	{
		LogHelper.Log.Setup(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData), "Gemvision\\CounterSketch Studio"), "GemvisionApplicationManager.log");
		LogHelper.Log.WriteLog(_loggingPrefix + " Initializing Service");
		GetInstalledApplicationIdentifiers();
		GetApplicationEvents();
		CreateApplicationServiceTimer();
	}

	public static void OnProcessedServiceEventNotification(string eventName, string eventInformation, ServiceEventStatus status)
	{
		GemvisionApplicationService.ProcessedServiceEventNotification?.Invoke(new ServiceEventNotificationArgs(eventName, eventInformation, status));
	}

	public void Start()
	{
		Initialize();
	}

	public void Stop()
	{
		_isShutDown = true;
	}

	private static void CreateApplicationServiceTimer()
	{
		LogHelper.Log.WriteLog(_loggingPrefix + " Creating Keep-Alive Timer to process Application Service Events");
		_timer = new Timer(_timerFrequency);
		_timer.Elapsed += OnTimedEvent;
		_timer.AutoReset = true;
		_timer.Enabled = true;
		_timer.Start();
		GC.KeepAlive(_timer);
	}

	private static void GetInstalledApplicationIdentifiers()
	{
		LogHelper.Log.WriteLog(_loggingPrefix + " Getting installed application identifiers.");
		_counterSketchIdentifier = GetInstalledCounterSketchIdentifier();
		if (_counterSketchIdentifier != string.Empty)
		{
			LogHelper.Log.WriteLog(_loggingPrefix + "\t CounterSketch Identifier - " + _counterSketchIdentifier);
		}
	}

	private static void OnTimedEvent(object sender, ElapsedEventArgs elapsedEventArgs)
	{
		if (_isShutDown)
		{
			_timer.Stop();
			_timer.Dispose();
		}
		else
		{
			ExecuteApplicationEvents();
		}
	}
}
