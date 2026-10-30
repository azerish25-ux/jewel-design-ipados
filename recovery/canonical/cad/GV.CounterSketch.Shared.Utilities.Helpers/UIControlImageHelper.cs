using System.IO;
using System.Windows.Media.Imaging;

namespace GV.CounterSketch.Shared.Utilities.Helpers;

public sealed class UIControlImageHelper
{
	public static byte[] BitmapSource2ByteArray(BitmapSource bitmap)
	{
		using MemoryStream memoryStream = new MemoryStream();
		PngBitmapEncoder pngBitmapEncoder = new PngBitmapEncoder();
		pngBitmapEncoder.Frames.Add(BitmapFrame.Create(bitmap));
		pngBitmapEncoder.Save(memoryStream);
		return memoryStream.ToArray();
	}

	public static BitmapSource ByteArray2BitmapSource(byte[] buffer)
	{
		using MemoryStream bitmapStream = new MemoryStream(buffer);
		return BitmapFrame.Create(bitmapStream, BitmapCreateOptions.None, BitmapCacheOption.OnLoad);
	}
}
