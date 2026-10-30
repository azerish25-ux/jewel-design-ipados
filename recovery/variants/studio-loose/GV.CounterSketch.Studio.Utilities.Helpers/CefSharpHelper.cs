using System;
using System.IO;
using CefSharp;
using CefSharp.Wpf;

namespace GV.CounterSketch.Studio.Utilities.Helpers;

public class CefSharpHelper
{
	public static void Initialize()
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0005: Unknown result type (might be due to invalid IL or missing references)
		//IL_0010: Unknown result type (might be due to invalid IL or missing references)
		//IL_0017: Unknown result type (might be due to invalid IL or missing references)
		//IL_0035: Expected O, but got Unknown
		Cef.Initialize((AbstractCefSettings)new CefSettings
		{
			BrowserSubprocessPath = "CefSharp.BrowserSubprocess.exe",
			LogSeverity = (LogSeverity)1,
			CachePath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "CefSharp\\Cache")
		}, false, (IBrowserProcessHandler)null);
	}

	public static void Shutdown()
	{
		Cef.Shutdown();
	}
}
