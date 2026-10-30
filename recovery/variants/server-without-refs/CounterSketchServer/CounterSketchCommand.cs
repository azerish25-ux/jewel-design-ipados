using System;
using System.Runtime.InteropServices;
using System.Windows.Threading;
using CounterSketchServer.Utilities.Helpers;
using Rhino;
using Rhino.Commands;

namespace CounterSketchServer;

[Guid("D6BC9740-A03D-4465-91C1-6207F7B8D5A2")]
public class CounterSketchCommand : Command
{
	private static bool _hasCommandExecuted;

	public override string EnglishName => "CounterSketchServer";

	public static CounterSketchCommand Instance { get; private set; }

	public CounterSketchCommand()
	{
		Instance = this;
	}

	protected override Result RunCommand(RhinoDoc doc, RunMode mode)
	{
		//IL_002a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0042: Unknown result type (might be due to invalid IL or missing references)
		if (!_hasCommandExecuted)
		{
			Dispatcher.CurrentDispatcher.UnhandledException += CurrentDispatcher_UnhandledException;
			try
			{
				CSVRHelper.StartServer();
				_hasCommandExecuted = true;
				return (Result)0;
			}
			catch (Exception arg)
			{
				GlobalErrorHandler.WriteInfoToEventLog($"Error occurred starting CounterSketch SERVER!\n\n{arg}");
			}
		}
		return (Result)3;
	}

	private void CurrentDispatcher_UnhandledException(object sender, DispatcherUnhandledExceptionEventArgs e)
	{
		GlobalErrorHandler.WriteErrorToEventLog("CounterSketch Server Exception: " + e.Exception);
		e.Handled = true;
	}
}
