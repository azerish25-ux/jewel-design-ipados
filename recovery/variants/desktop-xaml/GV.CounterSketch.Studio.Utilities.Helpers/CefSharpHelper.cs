using System;
using System.IO;
using CefSharp;
using CefSharp.Wpf;

namespace GV.CounterSketch.Studio.Utilities.Helpers;

public class CefSharpHelper
{
	public static void Initialize()
	{
		Cef.Initialize((AbstractCefSettings)new CefSettings
		{
			BrowserSubprocessPath = "CefSharp.BrowserSubprocess.exe",
			LogSeverity = LogSeverity.Verbose,
			CachePath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "CefSharp\\Cache")
		}, false, (IBrowserProcessHandler)null);
	}

	public static void Shutdown()
	{
		Cef.Shutdown();
	}
}
