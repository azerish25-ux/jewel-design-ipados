using System;
using System.Diagnostics;
using System.Runtime.InteropServices;

namespace Geomagic.Utilities;

public class InstanceManager
{
	private bool _AllowOnlyOneInstance;

	private bool _ActivateMainWindow;

	private bool _EliminateZombies;

	private int _Timeout = 1000;

	public bool AllowOnlyOneInstance
	{
		get
		{
			return _AllowOnlyOneInstance;
		}
		set
		{
			_AllowOnlyOneInstance = value;
		}
	}

	public bool ActivateMainWindow
	{
		get
		{
			return _ActivateMainWindow;
		}
		set
		{
			_ActivateMainWindow = value;
		}
	}

	public bool EliminateZombies
	{
		get
		{
			return _EliminateZombies;
		}
		set
		{
			_EliminateZombies = value;
		}
	}

	public int Timeout
	{
		get
		{
			return _Timeout;
		}
		set
		{
			_Timeout = value;
		}
	}

	[DllImport("User32.dll", EntryPoint = "FindWindow")]
	private static extern IntPtr FindWindowNative(string className, string windowName);

	[DllImport("User32.dll", EntryPoint = "SetForegroundWindow")]
	private static extern IntPtr SetForegroundWindowNative(IntPtr hWnd);

	public void Run()
	{
		Process currentProcess = Process.GetCurrentProcess();
		Process[] processesByName = Process.GetProcessesByName(currentProcess.ProcessName);
		if (EliminateZombies)
		{
			for (int i = 0; i < processesByName.Length; i++)
			{
				if (processesByName[i].MainWindowHandle == IntPtr.Zero && processesByName[i].Id != currentProcess.Id)
				{
					processesByName[i].Kill();
					processesByName[i].WaitForExit(Timeout);
				}
			}
			processesByName = Process.GetProcessesByName(currentProcess.ProcessName);
		}
		if (!AllowOnlyOneInstance || processesByName.Length <= 1)
		{
			return;
		}
		IntPtr foregroundWindowNative = IntPtr.Zero;
		for (int j = 0; j < processesByName.Length; j++)
		{
			if (processesByName[j].MainWindowHandle != IntPtr.Zero)
			{
				foregroundWindowNative = processesByName[j].MainWindowHandle;
				break;
			}
		}
		if (ActivateMainWindow)
		{
			SetForegroundWindowNative(foregroundWindowNative);
		}
		Environment.Exit(0);
	}
}
