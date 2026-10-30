using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows;
using System.Windows.Interop;

namespace GV.CounterSketch.Studio.Utilities;

public static class WindowHelper
{
	public delegate bool Win32Callback(IntPtr hwnd, IntPtr lParam);

	public enum WindowShowStyle : uint
	{
		Hide = 0u,
		ShowNormal = 1u,
		ShowMinimized = 2u,
		ShowMaximized = 3u,
		Maximize = 3u,
		ShowNormalNoActivate = 4u,
		Show = 5u,
		Minimize = 6u,
		ShowMinNoActivate = 7u,
		ShowNoActivate = 8u,
		Restore = 9u,
		ShowDefault = 10u,
		ForceMinimized = 11u
	}

	public struct POINT
	{
		public int X;

		public int Y;

		public POINT(Point pt)
		{
			if (pt.X > 2147483647.0)
			{
				pt.X = 2147483647.0;
			}
			if (pt.X < -2147483648.0)
			{
				pt.X = -2147483648.0;
			}
			if (double.IsNaN(pt.X))
			{
				pt.X = 0.0;
			}
			if (pt.Y > 2147483647.0)
			{
				pt.Y = 2147483647.0;
			}
			if (pt.Y < -2147483648.0)
			{
				pt.Y = -2147483648.0;
			}
			if (double.IsNaN(pt.Y))
			{
				pt.Y = 0.0;
			}
			X = Convert.ToInt32(pt.X);
			Y = Convert.ToInt32(pt.Y);
		}

		public POINT(int x, int y)
		{
			X = x;
			Y = y;
		}
	}

	public enum GetWindowType : uint
	{
		GW_HWNDFIRST,
		GW_HWNDLAST,
		GW_HWNDNEXT,
		GW_HWNDPREV,
		GW_OWNER,
		GW_CHILD,
		GW_ENABLEDPOPUP
	}

	public struct RECT
	{
		public int Bottom;

		public int Left;

		public int Right;

		public int Top;
	}

	public const int HWND_TOPMOST = -1;

	private const int RDW_ALLCHILDREN = 128;

	private const int RDW_ERASE = 4;

	private const int RDW_ERASENOW = 512;

	private const int RDW_FRAME = 1024;

	private const int RDW_INTERNALPAINT = 2;

	private const int RDW_INVALIDATE = 1;

	private const int RDW_NOCHILDREN = 64;

	private const int RDW_NOERASE = 32;

	private const int RDW_NOFRAME = 2048;

	private const int RDW_NOINTERNALPAINT = 16;

	private const int RDW_UPDATENOW = 256;

	private const int RDW_VALIDATE = 8;

	public const int SWP_DRAWFRAME = 32;

	public const int SWP_HIDEWINDOW = 128;

	public const int SWP_NOACTIVATE = 16;

	public const int SWP_NOMOVE = 2;

	public const int SWP_NOSIZE = 1;

	public const int SWP_NOZORDER = 4;

	public const int SWP_SHOWWINDOW = 64;

	private const int WM_CLOSE = 16;

	private const int WS_EX_NOACTIVATE = 134217728;

	private const int WS_EX_TOOLWINDOW = 128;

	private const int WS_EX_TOPMOST = 8;

	public static void SetOwnerWindow(this Window window)
	{
		SetOwnerWindow(window, forceNewOwner: false);
	}

	public static void SetOwnerWindow(this Window window, bool forceNewOwner)
	{
		SetOwnerWindow(window, Process.GetCurrentProcess().MainWindowHandle, forceNewOwner: false);
	}

	public static void SetOwnerWindow(this Window window, Window owner)
	{
		SetOwnerWindow(window, owner, forceNewOwner: false);
	}

	public static void SetOwnerWindow(this Window window, IntPtr owner)
	{
		SetOwnerWindow(window, owner, forceNewOwner: false);
	}

	public static void SetOwnerWindow(this Window window, Window owner, bool forceNewOwner)
	{
		if (forceNewOwner || !HasOwner(window))
		{
			window.Owner = owner;
		}
	}

	public static void SetOwnerWindow(this Window window, IntPtr owner, bool forceNewOwner)
	{
		if (!forceNewOwner && HasOwner(window))
		{
			return;
		}
		new WindowInteropHelper(window).Owner = owner;
		if (window.WindowStartupLocation != WindowStartupLocation.CenterOwner)
		{
			return;
		}
		window.Loaded += delegate
		{
			if (GetWindowRect(owner, out var lpRect))
			{
				int num = lpRect.Right - lpRect.Left;
				int num2 = lpRect.Bottom - lpRect.Top;
				int num3 = num / 2 + lpRect.Left;
				int num4 = num2 / 2 + lpRect.Top;
				window.WindowStartupLocation = WindowStartupLocation.Manual;
				window.Left = (double)num3 - window.ActualWidth / 2.0;
				window.Top = (double)num4 - window.ActualHeight / 2.0;
			}
		};
	}

	public static void ShowWindowTopMost(IntPtr handle)
	{
		SetWindowPos(handle, (IntPtr)(-1), 0, 0, 0, 0, 83);
	}

	private static bool HasOwner(Window window)
	{
		if (window.Owner == null)
		{
			return new WindowInteropHelper(window).Owner != IntPtr.Zero;
		}
		return true;
	}

	public static List<IntPtr> GetRootWindowsOfProcess(int pid)
	{
		List<IntPtr> childWindows = GetChildWindows(IntPtr.Zero);
		List<IntPtr> list = new List<IntPtr>();
		foreach (IntPtr item in childWindows)
		{
			GetWindowThreadProcessId(item, out var lpdwProcessId);
			if (lpdwProcessId == pid)
			{
				list.Add(item);
			}
		}
		return list;
	}

	public static List<IntPtr> GetChildWindows(IntPtr parent)
	{
		List<IntPtr> list = new List<IntPtr>();
		GCHandle value = GCHandle.Alloc(list);
		try
		{
			Win32Callback callback = EnumWindow;
			EnumChildWindows(parent, callback, GCHandle.ToIntPtr(value));
			return list;
		}
		finally
		{
			if (value.IsAllocated)
			{
				value.Free();
			}
		}
	}

	public static string GetCaptionOfWindow(IntPtr hwnd)
	{
		string result = "";
		StringBuilder stringBuilder = null;
		try
		{
			int windowTextLength = GetWindowTextLength(hwnd);
			stringBuilder = new StringBuilder("", windowTextLength + 5);
			GetWindowText(hwnd, stringBuilder, windowTextLength + 2);
			if (!string.IsNullOrEmpty(stringBuilder.ToString()) && !string.IsNullOrWhiteSpace(stringBuilder.ToString()))
			{
				result = stringBuilder.ToString();
			}
		}
		catch (Exception ex)
		{
			result = ex.Message;
		}
		finally
		{
			stringBuilder = null;
		}
		return result;
	}

	public static string GetClassNameOfWindow(IntPtr hwnd)
	{
		string result = "";
		StringBuilder stringBuilder = null;
		try
		{
			int num = 1000;
			stringBuilder = new StringBuilder("", num + 5);
			GetClassName(hwnd, stringBuilder, num + 2);
			if (!string.IsNullOrEmpty(stringBuilder.ToString()) && !string.IsNullOrWhiteSpace(stringBuilder.ToString()))
			{
				result = stringBuilder.ToString();
			}
		}
		catch (Exception ex)
		{
			result = ex.Message;
		}
		finally
		{
			stringBuilder = null;
		}
		return result;
	}

	private static bool EnumWindow(IntPtr handle, IntPtr pointer)
	{
		((GCHandle.FromIntPtr(pointer).Target as List<IntPtr>) ?? throw new InvalidCastException("GCHandle Target could not be cast as List<IntPtr>")).Add(handle);
		return true;
	}

	public static string GetWindowTitle(IntPtr windowHandle)
	{
		uint fuFlags = 2u;
		uint msg = 13u;
		int num = 32768;
		string empty = string.Empty;
		IntPtr intPtr = Marshal.AllocCoTaskMem(num);
		Marshal.Copy(empty.ToCharArray(), 0, intPtr, empty.Length);
		SendMessageTimeout(windowHandle, msg, (IntPtr)num, intPtr, fuFlags, 1000u, out var _);
		empty = Marshal.PtrToStringAuto(intPtr);
		Marshal.FreeCoTaskMem(intPtr);
		return empty;
	}

	public static string GetTopWindowName(uint processId)
	{
		StringBuilder stringBuilder = new StringBuilder(1000);
		IntPtr intPtr = OpenProcess(1040u, bInheritHandle: false, processId);
		GetModuleFileNameEx(intPtr, IntPtr.Zero, stringBuilder, stringBuilder.Capacity);
		CloseHandle(intPtr);
		return stringBuilder.ToString();
	}

	[DllImport("user32.dll")]
	public static extern IntPtr GetForegroundWindow();

	[DllImport("psapi.dll")]
	private static extern uint GetModuleFileNameEx(IntPtr hWnd, IntPtr hModule, StringBuilder lpFileName, int nSize);

	[DllImport("kernel32.dll")]
	public static extern IntPtr OpenProcess(uint dwDesiredAccess, bool bInheritHandle, uint dwProcessId);

	[DllImport("kernel32.dll")]
	public static extern bool CloseHandle(IntPtr handle);

	[DllImport("user32.dll")]
	public static extern bool ClientToScreen(IntPtr hWnd, ref POINT lpPoint);

	[DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
	public static extern IntPtr FindWindow(string lpClassName, string lpWindowName);

	[DllImport("user32.dll", EntryPoint = "FindWindow", SetLastError = true)]
	public static extern IntPtr FindWindowByCaption(IntPtr ZeroOnly, string lpWindowName);

	[DllImport("user32.dll", EntryPoint = "FindWindow", SetLastError = true)]
	public static extern IntPtr FindWindowByClassName(string lpWindowName, IntPtr ZeroOnly);

	[DllImport("user32.dll", SetLastError = true)]
	public static extern bool MoveWindow(IntPtr hWnd, int X, int Y, int nWidth, int nHeight, bool bRepaint);

	public static void RedrawWindow(IntPtr hWnd)
	{
		RedrawWindow(hWnd, IntPtr.Zero, IntPtr.Zero, 1415u);
	}

	[DllImport("user32.dll", SetLastError = true)]
	public static extern IntPtr GetWindow(IntPtr hWnd, GetWindowType uCmd);

	[DllImport("user32.dll", CharSet = CharSet.Auto)]
	public static extern IntPtr SendMessage(IntPtr hWnd, uint Msg, IntPtr wParam, IntPtr lParam);

	[DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
	[return: MarshalAs(UnmanagedType.Bool)]
	public static extern bool PostMessage(HandleRef hWnd, uint Msg, IntPtr wParam, IntPtr lParam);

	[DllImport("user32.dll")]
	public static extern IntPtr SetActiveWindow(IntPtr hWnd);

	[DllImport("user32.dll")]
	public static extern bool AttachThreadInput(uint idAttach, uint idAttachTo, bool fAttach);

	[DllImport("kernel32.dll")]
	public static extern uint GetCurrentThreadId();

	[DllImport("user32.dll")]
	public static extern IntPtr SetFocus(IntPtr hWnd);

	[DllImport("user32.dll")]
	[return: MarshalAs(UnmanagedType.Bool)]
	public static extern bool SetForegroundWindow(IntPtr hWnd);

	[DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
	public static extern IntPtr SendMessageTimeout(IntPtr hWnd, uint Msg, IntPtr wParam, IntPtr lParam, uint fuFlags, uint uTimeout, out IntPtr lpdwResult);

	[DllImport("user32.dll")]
	[return: MarshalAs(UnmanagedType.Bool)]
	public static extern bool GetWindowRect(IntPtr hWnd, out RECT lpRect);

	[DllImport("user32.dll")]
	[return: MarshalAs(UnmanagedType.Bool)]
	private static extern bool RedrawWindow(IntPtr hWnd, [In] ref RECT lprcUpdate, IntPtr hrgnUpdate, uint flags);

	[DllImport("user32.dll")]
	[return: MarshalAs(UnmanagedType.Bool)]
	private static extern bool RedrawWindow(IntPtr hWnd, IntPtr lprcUpdate, IntPtr hrgnUpdate, uint flags);

	[DllImport("user32.dll")]
	public static extern int GetWindowThreadProcessId(IntPtr hWnd, out uint lpdwProcessId);

	[DllImport("user32.Dll")]
	[return: MarshalAs(UnmanagedType.Bool)]
	public static extern bool EnumChildWindows(IntPtr parentHandle, Win32Callback callback, IntPtr lParam);

	[DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
	private static extern int GetWindowTextLength(IntPtr hWnd);

	[DllImport("User32.dll", CharSet = CharSet.Auto, SetLastError = true)]
	private static extern long GetWindowText(IntPtr hwnd, StringBuilder lpString, long cch);

	[DllImport("User32.dll")]
	private static extern IntPtr GetParent(IntPtr hwnd);

	[DllImport("User32.dll")]
	private static extern IntPtr ChildWindowFromPoint(IntPtr hWndParent, POINT p);

	[DllImport("User32.dll", CharSet = CharSet.Auto, SetLastError = true)]
	private static extern long GetClassName(IntPtr hwnd, StringBuilder lpClassName, long nMaxCount);

	[DllImport("user32.dll")]
	public static extern bool SetWindowPos(IntPtr hWnd, IntPtr hWndInsertAfter, int X, int Y, int cx, int cy, int uFlags);

	[DllImport("user32.dll")]
	public static extern bool ShowWindow(IntPtr hWnd, WindowShowStyle cmdShow);

	[DllImport("user32.dll")]
	public static extern bool ShowWindowAsync(IntPtr hWnd, WindowShowStyle cmdShow);

	[DllImport("User32.dll")]
	public static extern IntPtr FindWindowEx(IntPtr hwndParent, IntPtr hwndChildAfter, string strClassName, string strWindowName);

	[DllImport("user32.dll")]
	private static extern int GetWindowText(IntPtr hWnd, StringBuilder text, int count);
}
