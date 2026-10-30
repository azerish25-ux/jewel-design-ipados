using System;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows;

namespace GV.CounterSketch.Studio.Utilities;

public class Win32Helper
{
	[Flags]
	public enum SWP : uint
	{
		SWP_NOSIZE = 1u,
		SWP_NOMOVE = 2u,
		SWP_NOZORDER = 4u,
		SWP_NOREDRAW = 8u,
		SWP_NOACTIVATE = 0x10u,
		SWP_FRAMECHANGED = 0x20u,
		SWP_SHOWWINDOW = 0x40u,
		SWP_HIDEWINDOW = 0x80u,
		SWP_NOCOPYBITS = 0x100u,
		SWP_NOOWNERZORDER = 0x200u,
		SWP_NOSENDCHANGING = 0x400u,
		SWP_DRAWFRAME = 0x20u,
		SWP_NOREPOSITION = 0x200u,
		SWP_DEFERERASE = 0x2000u,
		SWP_ASYNCWINDOWPOS = 0x4000u
	}

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

	[Flags]
	public enum WindowStyles : uint
	{
		WS_OVERLAPPED = 0u,
		WS_POPUP = 0x80000000u,
		WS_CHILD = 0x40000000u,
		WS_MINIMIZE = 0x20000000u,
		WS_VISIBLE = 0x10000000u,
		WS_DISABLED = 0x8000000u,
		WS_CLIPSIBLINGS = 0x4000000u,
		WS_CLIPCHILDREN = 0x2000000u,
		WS_MAXIMIZE = 0x1000000u,
		WS_BORDER = 0x800000u,
		WS_DLGFRAME = 0x400000u,
		WS_VSCROLL = 0x200000u,
		WS_HSCROLL = 0x100000u,
		WS_SYSMENU = 0x80000u,
		WS_THICKFRAME = 0x40000u,
		WS_GROUP = 0x20000u,
		WS_TABSTOP = 0x10000u,
		WS_MINIMIZEBOX = 0x20000u,
		WS_MAXIMIZEBOX = 0x10000u,
		WS_CAPTION = 0xC00000u,
		WS_TILED = 0u,
		WS_ICONIC = 0x20000000u,
		WS_SIZEBOX = 0x40000u,
		WS_TILEDWINDOW = 0xCF0000u,
		WS_OVERLAPPEDWINDOW = 0xCF0000u,
		WS_POPUPWINDOW = 0x80880000u,
		WS_CHILDWINDOW = 0x40000000u,
		WS_EX_DLGMODALFRAME = 1u,
		WS_EX_NOPARENTNOTIFY = 4u,
		WS_EX_TOPMOST = 8u,
		WS_EX_ACCEPTFILES = 0x10u,
		WS_EX_TRANSPARENT = 0x20u,
		WS_EX_MDICHILD = 0x40u,
		WS_EX_TOOLWINDOW = 0x80u,
		WS_EX_WINDOWEDGE = 0x100u,
		WS_EX_CLIENTEDGE = 0x200u,
		WS_EX_CONTEXTHELP = 0x400u,
		WS_EX_RIGHT = 0x1000u,
		WS_EX_LEFT = 0u,
		WS_EX_RTLREADING = 0x2000u,
		WS_EX_LTRREADING = 0u,
		WS_EX_LEFTSCROLLBAR = 0x4000u,
		WS_EX_RIGHTSCROLLBAR = 0u,
		WS_EX_CONTROLPARENT = 0x10000u,
		WS_EX_STATICEDGE = 0x20000u,
		WS_EX_APPWINDOW = 0x40000u,
		WS_EX_OVERLAPPEDWINDOW = 0x300u,
		WS_EX_PALETTEWINDOW = 0x188u,
		WS_EX_LAYERED = 0x80000u,
		WS_EX_NOINHERITLAYOUT = 0x100000u,
		WS_EX_LAYOUTRTL = 0x400000u,
		WS_EX_COMPOSITED = 0x2000000u,
		WS_EX_NOACTIVATE = 0x8000000u
	}

	public struct WINDOWPOS
	{
		public IntPtr hwnd;

		public IntPtr hwndInsertAfter;

		public int x;

		public int y;

		public int cx;

		public int cy;

		public int flags;
	}

	[StructLayout(LayoutKind.Sequential)]
	internal class MonitorInfo
	{
		public int cbSize;

		public uint dwFlags;

		public RECT rcMonitor;

		public RECT rcWork;
	}

	internal struct RECT
	{
		public long bottom;

		public long left;

		public long right;

		public long top;
	}

	public enum MouseEventDataXButtons : uint
	{
		XBUTTON1 = 1u,
		XBUTTON2
	}

	[Flags]
	public enum MouseEventFlags : uint
	{
		LEFTDOWN = 2u,
		LEFTUP = 4u,
		MIDDLEDOWN = 0x20u,
		MIDDLEUP = 0x40u,
		MOVE = 1u,
		ABSOLUTE = 0x8000u,
		RIGHTDOWN = 8u,
		RIGHTUP = 0x10u,
		WHEEL = 0x800u,
		XDOWN = 0x80u,
		XUP = 0x100u
	}

	private const int GWL_EXSTYLE = -20;

	private const int GWL_ID = -12;

	private const int GWL_STYLE = -16;

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

	private const int WS_CHILD = 1073741824;

	public static bool AllowSetForegroungWindowSetProcessId(int processId)
	{
		return AllowSetForegroundWindow(processId);
	}

	public static void CleanWindow(IntPtr windowHandle)
	{
		SetWindowLong(windowHandle, -16, 1174405120);
		long num = GetWindowLong(windowHandle, -16);
		num &= 0xDE33FFFFu;
		SetWindowLong(windowHandle, -16, (int)num);
		long num2 = GetWindowLong(windowHandle, -20);
		num2 &= 0xFFFDFDFEu;
		SetWindowLong(windowHandle, -20, (int)num2);
		SetWindowPos(windowHandle, IntPtr.Zero, 0, 0, 0, 0, 551u);
	}

	[DllImport("user32.dll")]
	public static extern IntPtr GetCapture();

	[DllImport("user32.dll")]
	public static extern IntPtr GetForegroundWindow();

	public static WindowStyles GetWindowStyle(IntPtr hWnd)
	{
		return (WindowStyles)GetWindowLong(hWnd, -16);
	}

	public static void HideNonManagedWindow(IntPtr windowHandle)
	{
		ShowWindow(windowHandle, 0u);
	}

	public static void WindowSetHidden(IntPtr windowHandle)
	{
		SetWindowPos(windowHandle, windowHandle, 0, 0, 0, 0, 128u);
	}

	public static void WindowSetVisible(IntPtr windowHandle)
	{
		SetWindowPos(windowHandle, windowHandle, 0, 0, 0, 0, 64u);
	}

	[DllImport("user32.dll")]
	public static extern void mouse_event(int dwFlags, int dx, int dy, int dwData, int dwExtraInfo);

	public static void MouseLeftClick()
	{
		mouse_event(2, 0, 0, 0, 0);
		mouse_event(4, 0, 0, 0, 0);
	}

	[DllImport("user32.dll")]
	public static extern bool IsWindow(IntPtr hWnd);

	[DllImport("user32.dll", CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Auto, ExactSpelling = true, SetLastError = true)]
	public static extern void MoveWindow(IntPtr hwnd, int X, int Y, int nWidth, int nHeight, bool bRepaint);

	public static bool PositionWindow(IntPtr windowHandle, int x, int y, int x2, int y2)
	{
		return SetWindowPos(windowHandle, (IntPtr)0, x, y, x2, y2, 16u);
	}

	public static bool PositionWindowWithoutDraw(IntPtr parentHandle, IntPtr childHandle, int x, int y, int x2, int y2)
	{
		return SetWindowPos(childHandle, parentHandle, x, y, x2, y2, 12u);
	}

	public static void RedrawWindow(IntPtr hWnd)
	{
		RedrawWindow(hWnd, IntPtr.Zero, IntPtr.Zero, 1415u);
	}

	[DllImport("user32.dll")]
	public static extern bool ReleaseCapture();

	public static bool ReparentWindow(IntPtr childWindowToReparent, IntPtr newParentWindowHandle)
	{
		if (SetParent(childWindowToReparent, newParentWindowHandle) == (IntPtr)0)
		{
			return false;
		}
		return true;
	}

	[DllImport("user32.dll")]
	public static extern IntPtr SendMessage(IntPtr hWnd, int Msg, IntPtr wParam, IntPtr lParam);

	[DllImport("user32.dll")]
	public static extern IntPtr SetCapture(IntPtr hWnd);

	[DllImport("user32.dll")]
	public static extern IntPtr SetFocus(IntPtr hWnd);

	public static void SetWindowExtendedStyle(IntPtr hWnd, WindowStyles windowStyle)
	{
		SetWindowLong(hWnd, -20, (int)windowStyle);
	}

	[DllImport("user32.dll")]
	public static extern bool SetWindowPos(IntPtr hWnd, IntPtr hWndInsertAfter, int X, int Y, int cx, int cy, uint uFlags);

	public static void SetWindowStyle(IntPtr hWnd, WindowStyles windowStyle)
	{
		SetWindowLong(hWnd, -16, (int)windowStyle);
	}

	public static bool SetWindowToForeground(IntPtr handle)
	{
		return SetForegroundWindow(handle);
	}

	[DllImport("user32.dll")]
	public static extern bool ShowWindow(IntPtr hWnd, uint nCmdShow);

	public static void ShowWindowTopMost(IntPtr handle)
	{
		SetWindowPos(handle, new IntPtr(-1), 0, 0, 0, 0, 83u);
	}

	public static void ShowWindowWithStyle(IntPtr windowHandle, WindowShowStyle windowShowStyle)
	{
		ShowWindow(windowHandle, (uint)windowShowStyle);
	}

	public static void UnParentWindow(IntPtr childWindow)
	{
		SetParent(childWindow, IntPtr.Zero);
		SetWindowPos(childWindow, IntPtr.Zero, 0, 0, 0, 0, 32u);
	}

	[DllImport("user32.dll")]
	public static extern IntPtr WindowFromPoint(WindowHelper.POINT point);

	public static IntPtr WindowFromPoint(Point point)
	{
		return WindowFromPoint(new WindowHelper.POINT(point));
	}

	[DllImport("user32.dll")]
	private static extern bool AllowSetForegroundWindow(int dwProcessId);

	[DllImport("user32.dll", SetLastError = true)]
	public static extern int GetWindowLong(IntPtr hWnd, int nIndex);

	[DllImport("user32.dll")]
	[return: MarshalAs(UnmanagedType.Bool)]
	private static extern bool RedrawWindow(IntPtr hWnd, IntPtr lprcUpdate, IntPtr hrgnUpdate, uint flags);

	[DllImport("user32.dll")]
	[return: MarshalAs(UnmanagedType.Bool)]
	private static extern bool SetForegroundWindow(IntPtr hWnd);

	[DllImport("user32.dll", SetLastError = true)]
	public static extern IntPtr SetParent(IntPtr hWndChild, IntPtr hWndNewParent);

	[DllImport("user32.dll")]
	public static extern int SetWindowLong(IntPtr hWnd, int nIndex, int dwNewLong);

	public static IntPtr SetWindowLongPtr(HandleRef hWnd, int nIndex, IntPtr dwNewLong)
	{
		if (IntPtr.Size == 8)
		{
			return SetWindowLongPtr64(hWnd, nIndex, dwNewLong);
		}
		return new IntPtr(SetWindowLong32(hWnd, nIndex, dwNewLong.ToInt32()));
	}

	[DllImport("user32.dll", EntryPoint = "SetWindowLong")]
	private static extern int SetWindowLong32(HandleRef hWnd, int nIndex, int dwNewLong);

	[DllImport("user32.dll", EntryPoint = "SetWindowLongPtr")]
	private static extern IntPtr SetWindowLongPtr64(HandleRef hWnd, int nIndex, IntPtr dwNewLong);

	[DllImport("user32")]
	internal static extern bool GetMonitorInfo(IntPtr hMonitor, MonitorInfo lpmi);

	[DllImport("User32")]
	internal static extern IntPtr MonitorFromWindow(IntPtr handle, int flags);

	[DllImport("user32.dll", SetLastError = true)]
	public static extern IntPtr FindWindow(string lpClassName, string lpWindowName);

	[DllImport("user32.dll", EntryPoint = "FindWindow", SetLastError = true)]
	public static extern IntPtr FindWindowByCaption(IntPtr ZeroOnly, string lpWindowName);

	[DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
	public static extern int GetWindowTextLength(IntPtr hWnd);

	[DllImport("User32.dll", CharSet = CharSet.Auto, SetLastError = true)]
	public static extern long GetWindowText(IntPtr hwnd, StringBuilder lpString, long cch);

	[DllImport("User32.dll")]
	public static extern IntPtr GetParent(IntPtr hwnd);

	[DllImport("User32.dll", CharSet = CharSet.Auto, SetLastError = true)]
	public static extern long GetClassName(IntPtr hwnd, StringBuilder lpClassName, long nMaxCount);

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
}
