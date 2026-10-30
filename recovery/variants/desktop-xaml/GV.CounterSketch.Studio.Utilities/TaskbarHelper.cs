#define TRACE
using System;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Text;

namespace GV.CounterSketch.Studio.Utilities;

public static class TaskbarHelper
{
	public enum ABEdge
	{
		ABE_LEFT,
		ABE_TOP,
		ABE_RIGHT,
		ABE_BOTTOM
	}

	public enum ABState
	{
		ABS_MANUAL,
		ABS_AUTOHIDE,
		ABS_ALWAYSONTOP,
		ABS_AUTOHIDEANDONTOP
	}

	private enum ABMsg
	{
		ABM_NEW,
		ABM_REMOVE,
		ABM_QUERYPOS,
		ABM_SETPOS,
		ABM_GETSTATE,
		ABM_GETTASKBARPOS,
		ABM_ACTIVATE,
		ABM_GETAUTOHIDEBAR,
		ABM_SETAUTOHIDEBAR,
		ABM_WINDOWPOSCHANGED,
		ABM_SETSTATE
	}

	public struct APPBARDATA
	{
		public int cbSize;

		public IntPtr hWnd;

		public bool lParam;

		public WindowHelper.RECT rc;

		public int uCallbackMessage;

		public int uEdge;
	}

	public struct TaskbarState
	{
		public ABEdge T_EDGE;

		public WindowHelper.RECT T_SIZE;

		public ABState T_STATE;
	}

	public static TaskbarState GetTaskbarState()
	{
		TaskbarState WinTaskBar = default(TaskbarState);
		GetWinTaskbarState(ref WinTaskBar);
		return WinTaskBar;
	}

	[DllImport("user32.dll", SetLastError = true)]
	private static extern IntPtr FindWindow(string lpClassName, string lpWindowName);

	[DllImport("user32.dll")]
	private static extern int GetSystemMetrics(int Index);

	[DllImport("SHELL32", CallingConvention = CallingConvention.StdCall)]
	private static extern int SHAppBarMessage(int dwMessage, ref APPBARDATA pData);

	internal static bool GetWinTaskbarState(ref TaskbarState WinTaskBar)
	{
		APPBARDATA pData = default(APPBARDATA);
		long num = 0L;
		new StringBuilder();
		int num2 = 0;
		TaskbarState taskbarState = default(TaskbarState);
		try
		{
			pData.cbSize = Marshal.SizeOf(pData);
			pData.uEdge = 0;
			pData.hWnd = FindWindow("Shell_TrayWnd", "");
			pData.lParam = true;
			num = SHAppBarMessage(Convert.ToInt32(ABMsg.ABM_GETSTATE), ref pData);
			long num3 = num;
			if ((ulong)num3 <= 3uL)
			{
				switch ((int)num3)
				{
				case 0:
					taskbarState.T_STATE = ABState.ABS_MANUAL;
					break;
				case 2:
					taskbarState.T_STATE = ABState.ABS_ALWAYSONTOP;
					break;
				case 1:
					taskbarState.T_STATE = ABState.ABS_AUTOHIDE;
					break;
				case 3:
					taskbarState.T_STATE = ABState.ABS_AUTOHIDEANDONTOP;
					break;
				}
			}
			for (num2 = 0; num2 <= 3; num2++)
			{
				pData.uEdge = num2;
				if ((long)SHAppBarMessage(Convert.ToInt32(ABMsg.ABM_GETAUTOHIDEBAR), ref pData) > 0L)
				{
					switch (num2)
					{
					case 0:
						taskbarState.T_EDGE = ABEdge.ABE_LEFT;
						break;
					case 1:
						taskbarState.T_EDGE = ABEdge.ABE_TOP;
						break;
					case 2:
						taskbarState.T_EDGE = ABEdge.ABE_RIGHT;
						break;
					case 3:
						taskbarState.T_EDGE = ABEdge.ABE_BOTTOM;
						break;
					}
				}
			}
			taskbarState.T_SIZE = pData.rc;
			WinTaskBar = taskbarState;
			return true;
		}
		catch (Exception)
		{
			Trace.Assert(condition: false);
			return false;
		}
	}
}
