using System;
using System.Collections;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;
using System.Windows;
using System.Windows.Interop;
using GV.CounterSketch.Studio.Utilities.Extensions;
using GV.CounterSketch.Studio.Utilities.Helpers;

namespace GV.CounterSketch.Studio.Utilities;

public static class RhinoWindowHelper
{
	private delegate bool EnumedWindow(IntPtr handleWindow, ArrayList handles);

	private static RhinoPosition _currentRhinoPosition;

	public static bool IsRhinoRunning
	{
		get
		{
			Process[] processesByName = Process.GetProcessesByName("rhino");
			int num = 0;
			if (num < processesByName.Length)
			{
				_ = processesByName[num];
				return true;
			}
			return false;
		}
	}

	public static Process RhinoCounterSketchServerProcess { get; set; }

	public static IntPtr RhinoPerspectiveViewWindow { get; set; }

	public static IntPtr RhinoWindowHandle { get; set; }

	public static RhinoPosition CurrentRhinoPosition
	{
		get
		{
			if (_currentRhinoPosition == null)
			{
				_currentRhinoPosition = new RhinoPosition();
			}
			return _currentRhinoPosition;
		}
		set
		{
			_currentRhinoPosition = value;
		}
	}

	public static bool IsNoServerSwitchEnabled { get; set; }

	public static void Hide()
	{
		Win32Helper.ShowWindow(RhinoWindowHandle, 0u);
	}

	public static void KillAllRhinoProcesses()
	{
		Process[] processesByName = Process.GetProcessesByName("rhino");
		for (int i = 0; i < processesByName.Length; i++)
		{
			processesByName[i].Kill();
		}
	}

	public static bool ReparentRhino(Window parentWindow)
	{
		IntPtr handle = new WindowInteropHelper(parentWindow).Handle;
		if (!UIHelper.CoreDataService.RhinoWindowReparent(handle))
		{
			GlobalErrorHandler.WriteErrorToEventLog("ERROR *** : Failed to reparent Rhino Window.");
			return false;
		}
		RhinoWindowHandle = UIHelper.CoreDataService.GetRhinoWindowHandle();
		return true;
	}

	[DebuggerStepThrough]
	public static void RepositionRhino(FrameworkElement rhinoArea)
	{
		if (RhinoWindowHandle == IntPtr.Zero)
		{
			return;
		}
		double uIScaleFactor = UIHelper.CounterSketchOptions.UIScaleFactor;
		Window window = VisualTreeUtils.FindVisualAncestor<Window>(rhinoArea);
		if (window != null)
		{
			Point pt = rhinoArea.TransformToAncestor(window).Transform(new Point(0.0, 0.0));
			pt = rhinoArea.TransformFromDeviceDPI(pt);
			Size size = new Size(rhinoArea.ActualWidth, rhinoArea.ActualHeight);
			size = rhinoArea.TransformFromDeviceDPI(size);
			RhinoPosition rhinoPosition = new RhinoPosition(CurrentRhinoPosition)
			{
				Top = (int)pt.Y,
				Left = (int)pt.X,
				Width = (int)(uIScaleFactor * size.Width),
				Height = (int)(uIScaleFactor * size.Height)
			};
			if (!CurrentRhinoPosition.Equals(rhinoPosition))
			{
				Win32Helper.PositionWindow(RhinoWindowHandle, rhinoPosition.Left, rhinoPosition.Top, rhinoPosition.Width, rhinoPosition.Height);
				CurrentRhinoPosition = rhinoPosition;
			}
		}
	}

	public static void RepositionRhinoForced()
	{
		if (RhinoWindowHandle != IntPtr.Zero)
		{
			Win32Helper.PositionWindow(RhinoWindowHandle, CurrentRhinoPosition.Left, CurrentRhinoPosition.Top, CurrentRhinoPosition.Width, CurrentRhinoPosition.Height);
		}
	}

	public static void Show()
	{
		Win32Helper.ShowWindow(RhinoWindowHandle, 5u);
		RepositionRhinoForced();
	}

	[DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
	[return: MarshalAs(UnmanagedType.Bool)]
	private static extern bool EnumWindows(EnumedWindow lpEnumFunc, ArrayList lParam);

	private static IntPtr GetRhinoTopWindow(int rhinoProcessId, bool overrideProcessIdDebug = false)
	{
		GlobalErrorHandler.WriteInfoToEventLog("------------------------------------------------", 2);
		string value = "Rhinoceros 5.0";
		string value2 = "Afx:00007FF";
		foreach (IntPtr item in GetWindows().Cast<IntPtr>())
		{
			GetWindowThreadProcessId(item, out var processId);
			if (processId == rhinoProcessId || overrideProcessIdDebug)
			{
				string captionOfWindow = Win32Helper.GetCaptionOfWindow(item);
				string classNameOfWindow = Win32Helper.GetClassNameOfWindow(item);
				GlobalErrorHandler.WriteInfoToEventLog("Rhino Window: " + item + " " + captionOfWindow + " - " + classNameOfWindow, 2);
				if (captionOfWindow.Contains(value) && classNameOfWindow.Contains(value2))
				{
					GlobalErrorHandler.WriteInfoToEventLog("Found Rhino Top Window Handle:" + item, 2);
					GlobalErrorHandler.WriteInfoToEventLog("------------------------------------------------", 2);
					return item;
				}
			}
		}
		GlobalErrorHandler.WriteInfoToEventLog("Rhino Top Window Handle Not Found", 2);
		GlobalErrorHandler.WriteInfoToEventLog("------------------------------------------------", 2);
		return IntPtr.Zero;
	}

	private static bool GetWindowHandle(IntPtr windowHandle, ArrayList windowHandles)
	{
		windowHandles.Add(windowHandle);
		return true;
	}

	private static ArrayList GetWindows()
	{
		ArrayList arrayList = new ArrayList();
		EnumWindows(GetWindowHandle, arrayList);
		return arrayList;
	}

	[DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
	private static extern int GetWindowThreadProcessId(IntPtr handle, out int processId);
}
