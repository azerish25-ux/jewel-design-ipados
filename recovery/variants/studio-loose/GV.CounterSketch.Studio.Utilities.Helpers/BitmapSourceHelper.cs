using System;
using System.Windows;
using System.Windows.Media.Imaging;

namespace GV.CounterSketch.Studio.Utilities.Helpers;

public static class BitmapSourceHelper
{
	public unsafe static void CopyPixelsFast(this BitmapSource source, ImageHelper.PixelColor[,] pixels, int stride, int offset)
	{
		fixed (ImageHelper.PixelColor* ptr = &pixels[0, 0])
		{
			source.CopyPixels(new Int32Rect(0, 0, source.PixelWidth, source.PixelHeight), (IntPtr)(ptr + offset), pixels.GetLength(0) * pixels.GetLength(1) * sizeof(ImageHelper.PixelColor), stride);
		}
	}
}
