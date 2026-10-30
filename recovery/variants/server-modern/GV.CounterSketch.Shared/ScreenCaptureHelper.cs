using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace GV.CounterSketch.Shared;

public sealed class ScreenCaptureHelper
{
	private sealed class GDI32
	{
		public const int SRCCOPY = 13369376;

		[DllImport("gdi32.dll")]
		public static extern bool BitBlt(IntPtr hObject, int nXDest, int nYDest, int nWidth, int nHeight, IntPtr hObjectSource, int nXSrc, int nYSrc, int dwRop);

		[DllImport("gdi32.dll")]
		public static extern IntPtr CreateCompatibleBitmap(IntPtr hDC, int nWidth, int nHeight);

		[DllImport("gdi32.dll")]
		public static extern IntPtr CreateCompatibleDC(IntPtr hDC);

		[DllImport("gdi32.dll")]
		public static extern bool DeleteDC(IntPtr hDC);

		[DllImport("gdi32.dll")]
		public static extern bool DeleteObject(IntPtr hObject);

		[DllImport("gdi32.dll")]
		public static extern IntPtr SelectObject(IntPtr hDC, IntPtr hObject);
	}

	private sealed class User32
	{
		public struct RECT
		{
			public int left;

			public int top;

			public int right;

			public int bottom;
		}

		[DllImport("user32.dll", EntryPoint = "SendMessageA")]
		public static extern uint SendMessage(IntPtr hWnd, int wMsg, IntPtr wParam, IntPtr lParam);

		[DllImport("user32.dll")]
		public static extern IntPtr GetDesktopWindow();

		[DllImport("user32.dll")]
		public static extern IntPtr GetWindowDC(IntPtr hWnd);

		[DllImport("user32.dll")]
		public static extern IntPtr ReleaseDC(IntPtr hWnd, IntPtr hDC);

		[DllImport("user32.dll")]
		public static extern IntPtr GetWindowRect(IntPtr hWnd, ref RECT rect);
	}

	public unsafe static Bitmap GrabScreenshot()
	{
		SIZE sIZE = default(SIZE);
		sIZE.cx = Screen.PrimaryScreen.Bounds.Width;
		sIZE.cy = Screen.PrimaryScreen.Bounds.Height;
		IntPtr dC = PlatformInvokeUSER32.GetDC(IntPtr.Zero);
		IntPtr intPtr = PlatformInvokeGDI32.CreateCompatibleDC(dC);
		IntPtr intPtr2 = (IntPtr)(void*)null;
		IntPtr zero = IntPtr.Zero;
		intPtr2 = PlatformInvokeGDI32.CreateCompatibleBitmap(dC, sIZE.cx, sIZE.cy);
		zero = PlatformInvokeGDI32.SelectObject(intPtr, intPtr2);
		PlatformInvokeGDI32.BitBlt(intPtr, 0, 0, sIZE.cx, sIZE.cy, dC, 0, 0, 13369376);
		PlatformInvokeGDI32.SelectObject(intPtr, zero);
		PlatformInvokeGDI32.DeleteDC(intPtr);
		PlatformInvokeUSER32.ReleaseDC(IntPtr.Zero, dC);
		Bitmap result = Image.FromHbitmap(intPtr2);
		PlatformInvokeGDI32.DeleteObject(intPtr2);
		return result;
	}

	public static Bitmap GrabScreenshotOfWindow(IntPtr windowHandle)
	{
		if (windowHandle == IntPtr.Zero)
		{
			return null;
		}
		Win32Utility.SetWindowToForeground(windowHandle);
		User32.RECT rect = default(User32.RECT);
		User32.GetWindowRect(windowHandle, ref rect);
		int width = rect.right - rect.left;
		int height = rect.bottom - rect.top;
		Bitmap bitmap = new Bitmap(width, height, PixelFormat.Format32bppArgb);
		Graphics.FromImage(bitmap).CopyFromScreen(rect.left, rect.top, 0, 0, new Size(width, height), CopyPixelOperation.SourceCopy);
		return bitmap;
	}
}
