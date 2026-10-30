using System;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Windows;
using System.Windows.Interop;

namespace GV.CounterSketch.Shared;

public static class WindowExtensions
{
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

	public struct RECT
	{
		public int Bottom;

		public int Left;

		public int Right;

		public int Top;
	}

	public const int HWND_TOPMOST = -1;

	public const int SWP_DRAWFRAME = 32;

	public const int SWP_HIDEWINDOW = 128;

	public const int SWP_NOACTIVATE = 16;

	public const int SWP_NOMOVE = 2;

	public const int SWP_NOSIZE = 1;

	public const int SWP_NOZORDER = 4;

	public const int SWP_SHOWWINDOW = 64;

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

	private const int WM_CLOSE = 16;

	private const int WS_EX_NOACTIVATE = 134217728;

	private const int WS_EX_TOOLWINDOW = 128;

	private const int WS_EX_TOPMOST = 8;

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

	[DllImport("user32.dll")]
	public static extern IntPtr SetActiveWindow(IntPtr hWnd);

	[DllImport("user32.dll")]
	public static extern IntPtr SetFocus(IntPtr hWnd);

	[DllImport("user32.dll")]
	[return: MarshalAs(UnmanagedType.Bool)]
	public static extern bool SetForegroundWindow(IntPtr hWnd);

	public static void SetOwnerWindow(this Window window)
	{
		window.SetOwnerWindow(forceNewOwner: false);
	}

	public static void SetOwnerWindow(this Window window, bool forceNewOwner)
	{
		window.SetOwnerWindow(Process.GetCurrentProcess().MainWindowHandle, forceNewOwner: false);
	}

	public static void SetOwnerWindow(this Window window, Window owner)
	{
		window.SetOwnerWindow(owner, forceNewOwner: false);
	}

	public static void SetOwnerWindow(this Window window, IntPtr owner)
	{
		window.SetOwnerWindow(owner, forceNewOwner: false);
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

	[DllImport("user32.dll")]
	public static extern bool SetWindowPos(IntPtr hWnd, IntPtr hWndInsertAfter, int X, int Y, int cx, int cy, int uFlags);

	[DllImport("user32.dll")]
	public static extern bool ShowWindow(IntPtr hWnd, WindowShowStyle nCmdShow);

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

	[DllImport("user32.dll")]
	[return: MarshalAs(UnmanagedType.Bool)]
	private static extern bool RedrawWindow(IntPtr hWnd, [In] ref RECT lprcUpdate, IntPtr hrgnUpdate, uint flags);

	[DllImport("user32.dll")]
	[return: MarshalAs(UnmanagedType.Bool)]
	private static extern bool RedrawWindow(IntPtr hWnd, IntPtr lprcUpdate, IntPtr hrgnUpdate, uint flags);

	[DllImport("user32.dll")]
	[return: MarshalAs(UnmanagedType.Bool)]
	public static extern bool GetWindowRect(IntPtr hWnd, out RECT lpRect);
}
