using System;
using System.Runtime.InteropServices;

namespace GV.CounterSketch.Studio.Utilities.Helpers;

public class MessageFilter : IOleMessageFilter
{
	public static void Register()
	{
		MessageFilter newFilter = new MessageFilter();
		IOleMessageFilter oldFilter = null;
		CoRegisterMessageFilter(newFilter, out oldFilter);
	}

	public static void Revoke()
	{
		IOleMessageFilter oldFilter = null;
		CoRegisterMessageFilter(null, out oldFilter);
	}

	[DllImport("Ole32.dll")]
	private static extern int CoRegisterMessageFilter(IOleMessageFilter newFilter, out IOleMessageFilter oldFilter);

	int IOleMessageFilter.HandleInComingCall(int dwCallType, IntPtr hTaskCaller, int dwTickCount, IntPtr lpInterfaceInfo)
	{
		return 0;
	}

	int IOleMessageFilter.RetryRejectedCall(IntPtr hTaskCallee, int dwTickCount, int dwRejectType)
	{
		if (dwRejectType == 2)
		{
			return 99;
		}
		return -1;
	}

	int IOleMessageFilter.MessagePending(IntPtr hTaskCallee, int dwTickCount, int dwPendingType)
	{
		return 2;
	}
}
