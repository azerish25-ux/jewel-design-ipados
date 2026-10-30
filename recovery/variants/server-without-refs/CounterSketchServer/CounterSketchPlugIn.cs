using System;
using System.Windows.Input;
using System.Windows.Interactivity;
using CounterSketchServer.Utilities.Helpers;
using Rhino;
using Rhino.PlugIns;

namespace CounterSketchServer;

public class CounterSketchPlugIn : PlugIn
{
	private static bool _hasServerInitialized;

	private static EventTrigger _naughtyAssemblyMerge = new EventTrigger();

	public static CounterSketchPlugIn Instance { get; private set; }

	protected override LoadReturnCode OnLoad(ref string errorMessage)
	{
		//IL_0032: Unknown result type (might be due to invalid IL or missing references)
		//IL_003c: Expected O, but got Unknown
		GlobalErrorHandler.WriteInfoToEventLog("\n\nCounterSketchServer Plugin Initializing.\n");
		GlobalErrorHandler.WriteMethodInfo("OnLoad");
		Instance = this;
		RhinoApp.Idle += RhinoAppOnIdle;
		RhinoApp.KeyboardEvent += new KeyboardHookEvent(RhinoApp_KeyboardEvent);
		return (LoadReturnCode)1;
	}

	private void RhinoApp_KeyboardEvent(int key)
	{
		if (RhinoServices.StudioUICallback != null)
		{
			Key keyPressed = KeyInterop.KeyFromVirtualKey(key);
			RhinoServices.StudioUICallback.RhinoWindowKeyPressedCallback(keyPressed);
		}
	}

	protected override void OnShutdown()
	{
		CSVRHelper.StopServer();
		((PlugIn)this).OnShutdown();
	}

	private void RhinoAppOnIdle(object sender, EventArgs eventArgs)
	{
		if (!_hasServerInitialized)
		{
			GlobalErrorHandler.WriteMethodInfo("RhinoAppOnIdle");
			if (CSVRHelper.StartServer() == 1)
			{
				GlobalErrorHandler.WriteInfoToEventLog("CounterSketchServer Plugin Initialized.");
				_hasServerInitialized = true;
				RhinoApp.Idle -= RhinoAppOnIdle;
			}
		}
	}
}
