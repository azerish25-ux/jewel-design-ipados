#define TRACE
using System;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Windows;
using System.Windows.Interop;

namespace GV.CounterSketch.Studio.Controls;

public static class FrameworkElementExtensions
{
	[StructLayout(LayoutKind.Sequential, Pack = 1)]
	public struct InputKeys
	{
		public uint type;

		public uint wVk;

		public uint wScan;

		public uint dwFlags;

		public uint time;

		public uint dwExtra;
	}

	public const uint INPUT_KEYBOARD = 1u;

	public const uint KEYEVENTF_EXTENDEDKEY = 1u;

	public const uint KEYEVENTF_KEYUP = 2u;

	[DllImport("user32.dll", SetLastError = true)]
	private static extern bool BringWindowToTop(IntPtr hWnd);

	[DllImport("user32.dll", SetLastError = true)]
	private static extern bool BringWindowToTop(HandleRef hWnd);

	[DllImport("user32.dll", SetLastError = true)]
	private static extern uint GetWindowThreadProcessId(IntPtr hWnd, out uint lpdwProcessId);

	[DllImport("user32.dll")]
	private static extern IntPtr GetWindowThreadProcessId(IntPtr hWnd, IntPtr ProcessId);

	[DllImport("user32.dll")]
	private static extern IntPtr GetForegroundWindow();

	[DllImport("kernel32.dll")]
	private static extern IntPtr GetCurrentThreadId();

	[DllImport("user32.dll")]
	private static extern bool AttachThreadInput(IntPtr idAttach, IntPtr idAttachTo, bool fAttach);

	[DllImport("user32.dll")]
	private static extern IntPtr GetLastActivePopup(IntPtr hWnd);

	[DllImport("user32.dll")]
	private static extern IntPtr SetActiveWindow(IntPtr hWnd);

	[DllImport("User32.DLL")]
	public static extern uint SendInput(uint nInputs, InputKeys[] inputs, int cbSize);

	public static void BringWindowToTop(this FrameworkElement frameworkElement)
	{
		if (frameworkElement == null)
		{
			return;
		}
		Window window = null;
		FrameworkElement frameworkElement2 = frameworkElement;
		while (frameworkElement2 != null && window == null)
		{
			if (frameworkElement2 is Window)
			{
				window = (Window)frameworkElement2;
				break;
			}
			frameworkElement2 = frameworkElement2.Parent as FrameworkElement;
		}
		if (window == null)
		{
			try
			{
				HwndSource hwndSource = (HwndSource)PresentationSource.FromVisual(frameworkElement);
				if (hwndSource.Handle != IntPtr.Zero)
				{
					SetActiveWindow(hwndSource.Handle);
					SetForegroundWindowEx(hwndSource.Handle);
					return;
				}
			}
			catch (Exception)
			{
			}
		}
		try
		{
			IntPtr intPtr = ((window != null) ? new WindowInteropHelper(window).Handle : Process.GetCurrentProcess().MainWindowHandle);
			if (intPtr != IntPtr.Zero)
			{
				SetForegroundWindowEx(intPtr);
			}
		}
		catch (Exception)
		{
		}
	}

	private static void SetForegroundWindowEx(IntPtr hWnd)
	{
		IntPtr windowThreadProcessId = GetWindowThreadProcessId(GetForegroundWindow(), IntPtr.Zero);
		IntPtr currentThreadId = GetCurrentThreadId();
		if (!AttachThreadInput(windowThreadProcessId, currentThreadId, fAttach: true))
		{
			Trace.TraceWarning("Failed to attach thread input");
			return;
		}
		SetActiveWindow(GetLastActivePopup(hWnd));
		if (!AttachThreadInput(windowThreadProcessId, currentThreadId, fAttach: false))
		{
			Trace.TraceWarning("Failed to detach thread input");
		}
		else
		{
			BringWindowToTop(hWnd);
		}
	}
}
