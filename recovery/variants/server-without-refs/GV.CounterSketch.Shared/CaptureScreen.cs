using System;
using System.Drawing;

namespace GV.CounterSketch.Shared;

public sealed class CaptureScreen
{
	public static Bitmap GetDesktopImage()
	{
		IntPtr dC = PlatformInvokeUSER32.GetDC(PlatformInvokeUSER32.GetDesktopWindow());
		IntPtr intPtr = PlatformInvokeGDI32.CreateCompatibleDC(dC);
		SIZE sIZE = default(SIZE);
		sIZE.cx = PlatformInvokeUSER32.GetSystemMetrics(0);
		sIZE.cy = PlatformInvokeUSER32.GetSystemMetrics(1);
		IntPtr intPtr2 = PlatformInvokeGDI32.CreateCompatibleBitmap(dC, sIZE.cx, sIZE.cy);
		if (intPtr2 != IntPtr.Zero)
		{
			IntPtr bmp = PlatformInvokeGDI32.SelectObject(intPtr, intPtr2);
			PlatformInvokeGDI32.BitBlt(intPtr, 0, 0, sIZE.cx, sIZE.cy, dC, 0, 0, 13369376);
			PlatformInvokeGDI32.SelectObject(intPtr, bmp);
			PlatformInvokeGDI32.DeleteDC(intPtr);
			PlatformInvokeUSER32.ReleaseDC(PlatformInvokeUSER32.GetDesktopWindow(), dC);
			Bitmap result = Image.FromHbitmap(intPtr2);
			PlatformInvokeGDI32.DeleteObject(intPtr2);
			GC.Collect();
			return result;
		}
		return null;
	}
}
