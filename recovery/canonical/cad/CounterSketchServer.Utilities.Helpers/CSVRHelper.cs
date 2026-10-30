using System;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.ServiceModel;
using System.Threading;
using System.Timers;
using System.Windows;
using System.Windows.Threading;
using CS;
using CSBlocks.Settings.Bezels;
using GV.CounterSketch.Shared.Utilities;
using GalaSoft.MvvmLight.Threading;
using RMA.Rhino;
using Rhino;

namespace CounterSketchServer.Utilities.Helpers;

public static class CSVRHelper
{
	private static bool _isServerRunning;

	private static bool _isTimerBusy;

	private static System.Timers.Timer _parentStillAliveTimer;

	private static ServiceHost _rhinoServicesHost;

	public static int ParentProcessId { get; set; }

	public static void KillRhino()
	{
		GlobalErrorHandler.WriteMethodInfo("KillRhino");
		RhinoApp.Exit();
		Thread.Sleep(100);
		Process.GetCurrentProcess().Kill();
	}

	public static int StartServer()
	{
		GlobalErrorHandler.WriteMethodInfo("StartServer");
		if (_isServerRunning)
		{
			return 1;
		}
		RhUtil.RhinoApp().HideToolBars(0, bRedraw: true);
		RhUtil.RhinoApp().HideWindowBars();
		RhUtil.RhinoApp().HideControlBars();
		CSApplicationHelper.WhichCounterSketchApplication = CSApplicationHelper.CounterSketchApplication.Studio;
		if (!CSApplicationHelper.Application.OnLoad())
		{
			return 0;
		}
		CSApplicationHelper.Application.BlockList.Clear();
		CSApplicationHelper.Application.RegisterCSBlocks();
		try
		{
			Assembly assembly = Assembly.GetAssembly(typeof(CSBezel));
			CSApplicationHelper.Application.RegisterCSBlocks(assembly);
		}
		catch
		{
		}
		CSApplicationHelper.Application.PrintTimingInformation = false;
		DispatcherHelper.Initialize();
		DispatcherHelper.UIDispatcher.UnhandledException += UnhandledExceptionHandler;
		if (!CreateRhinoServicesHost())
		{
			return 0;
		}
		if (_parentStillAliveTimer != null)
		{
			_parentStillAliveTimer.Elapsed -= _parentStillAliveTimer_Tick;
			_parentStillAliveTimer.Stop();
		}
		_parentStillAliveTimer = new System.Timers.Timer(5000.0);
		_parentStillAliveTimer.Elapsed += _parentStillAliveTimer_Tick;
		_parentStillAliveTimer.Start();
		_isServerRunning = true;
		return 1;
	}

	public static void StopServer()
	{
		_isServerRunning = false;
		_rhinoServicesHost.Abort();
	}

	private static void _parentStillAliveTimer_Tick(object sender, EventArgs e)
	{
		if (!_isTimerBusy && ParentProcessId != 0)
		{
			_isTimerBusy = true;
			_parentStillAliveTimer.Enabled = false;
			if (!(from x in Process.GetProcesses()
				where x.Id == ParentProcessId
				select x).Any())
			{
				GlobalErrorHandler.WriteErrorToEventLog("Rhino Server failed to find Client, Rhino will be killed.\nProcess Id: " + ParentProcessId);
				_parentStillAliveTimer.Stop();
				_parentStillAliveTimer.Dispose();
				KillRhino();
			}
			_parentStillAliveTimer.Enabled = true;
			_isTimerBusy = false;
		}
	}

	private static bool CreateRhinoServicesHost()
	{
		GlobalErrorHandler.WriteMethodInfo("CreateRhinoServicesHost");
		CSUtil.Print("Starting CS Server...\n");
		_rhinoServicesHost = ServiceHostExtensions.CreateNewServiceHost<RhinoServices>();
		_rhinoServicesHost.AddNetTcpBindingAndEndpoint<IRhinoServices>();
		_rhinoServicesHost.AddNetTcpMexBindingAndEndpoint();
		_rhinoServicesHost.IncreaseThrottlingMaxValues();
		_rhinoServicesHost.IncreaseObjectGraphMaxValues();
		_rhinoServicesHost.Faulted += RhinoServicesHostFaulted;
		try
		{
			_rhinoServicesHost.Open();
		}
		catch (AddressAlreadyInUseException)
		{
			GlobalErrorHandler.WriteErrorToEventLog("Rhino Server failed to start because the Port 8556 was occupied!");
			MessageBox.Show("There is a process running using Port 8556 which CounterSketch requires.\n\nPlease look for any running Rhino4.exe processes using the Task Manager and kill them.  Then relaunch CounterSketch.", "ERROR!", MessageBoxButton.OK, MessageBoxImage.Exclamation, MessageBoxResult.OK, MessageBoxOptions.ServiceNotification);
			return false;
		}
		GlobalErrorHandler.WriteInfoToEventLog("Rhino Server Open and Ready!");
		CSUtil.Print("CS Server open for business.\n");
		return true;
	}

	private static void RhinoServicesHostFaulted(object sender, EventArgs e)
	{
		GlobalErrorHandler.WriteErrorToEventLog("Rhino Server Faulted!");
		_isServerRunning = false;
		_rhinoServicesHost.Abort();
		CreateRhinoServicesHost();
	}

	private static void UnhandledExceptionHandler(object sender, DispatcherUnhandledExceptionEventArgs e)
	{
		GlobalErrorHandler.WriteErrorToEventLog("CounterSketch Server Exception: " + e.Exception);
		e.Handled = true;
	}
}
