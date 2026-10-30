using System.Windows;
using System.Windows.Threading;

namespace GV.CounterSketch.Studio.Utilities.Helpers;

public class DispatcherHelper
{
	private static readonly DispatcherOperationCallback exitFrameCallback = ExitFrame;

	public static void WaitForPriority()
	{
		DispatcherFrame dispatcherFrame = new DispatcherFrame();
		DispatcherOperation dispatcherOperation = Application.Current.Dispatcher.BeginInvoke(DispatcherPriority.ApplicationIdle, exitFrameCallback, dispatcherFrame);
		Dispatcher.PushFrame(dispatcherFrame);
		if (dispatcherOperation.Status != DispatcherOperationStatus.Completed)
		{
			dispatcherOperation.Abort();
		}
	}

	public static void WaitForPriority(Dispatcher thisDispatcher)
	{
		DispatcherFrame dispatcherFrame = new DispatcherFrame();
		DispatcherOperation dispatcherOperation = thisDispatcher.BeginInvoke(DispatcherPriority.ApplicationIdle, exitFrameCallback, dispatcherFrame);
		Dispatcher.PushFrame(dispatcherFrame);
		if (dispatcherOperation.Status != DispatcherOperationStatus.Completed)
		{
			dispatcherOperation.Abort();
		}
	}

	private static object ExitFrame(object state)
	{
		(state as DispatcherFrame).Continue = false;
		return null;
	}
}
