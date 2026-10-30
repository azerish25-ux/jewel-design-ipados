using System;
using System.Drawing;
using System.Windows;
using System.Windows.Interop;
using System.Windows.Media.Imaging;
using GV.CounterSketch.Studio.Utilities.Helpers;

namespace GV.CounterSketch.Studio.Utilities.Extensions;

public static class BitmapExtensions
{
	public static BitmapSource ToBitmapSource(this Bitmap source)
	{
		BitmapSource bitmapSource = null;
		using SafeHBitmapHandle safeHBitmapHandle = new SafeHBitmapHandle(source.GetHbitmap(), ownsHandle: true);
		return Imaging.CreateBitmapSourceFromHBitmap(safeHBitmapHandle.DangerousGetHandle(), IntPtr.Zero, Int32Rect.Empty, BitmapSizeOptions.FromEmptyOptions());
	}
}
