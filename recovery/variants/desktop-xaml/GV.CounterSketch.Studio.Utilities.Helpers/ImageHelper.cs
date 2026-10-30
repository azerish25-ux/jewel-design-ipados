using System;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.IO;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace GV.CounterSketch.Studio.Utilities.Helpers;

public class ImageHelper
{
	public struct PixelColor
	{
		public byte Blue;

		public byte Green;

		public byte Red;

		public byte Alpha;
	}

	public static uint ColorToUInt(System.Drawing.Color color)
	{
		return (uint)((color.A << 24) | (color.R << 16) | (color.G << 8) | color.B);
	}

	public static byte[] ConvertBitmapImageToByteArray(BitmapImage imageSource)
	{
		Stream streamSource = imageSource.StreamSource;
		byte[] result = null;
		if (streamSource != null && streamSource.Length > 0)
		{
			using BinaryReader binaryReader = new BinaryReader(streamSource);
			result = binaryReader.ReadBytes((int)streamSource.Length);
		}
		return result;
	}

	public static string ConvertBitmapToBase64String(Bitmap bitmap)
	{
		MemoryStream memoryStream = new MemoryStream();
		bitmap.Save(memoryStream, ImageFormat.Bmp);
		return Convert.ToBase64String(memoryStream.ToArray());
	}

	public static BitmapImage ConvertBitmapToBitmapImage(Bitmap bitmap)
	{
		BitmapImage bitmapImage = new BitmapImage();
		if (bitmap != null)
		{
			double num = bitmap.Height;
			double num2 = bitmap.Width;
			using WrappingStream streamSource = new WrappingStream(new MemoryStream(ConvertImageToByteArray(bitmap), writable: false));
			bitmapImage.BeginInit();
			bitmapImage.DecodePixelHeight = (int)num;
			bitmapImage.DecodePixelWidth = (int)num2;
			bitmapImage.CacheOption = BitmapCacheOption.OnLoad;
			bitmapImage.StreamSource = streamSource;
			bitmapImage.EndInit();
			bitmapImage.Freeze();
		}
		return bitmapImage;
	}

	public static BitmapImage ConvertBitmapToBitmapImageNoWrapper(Bitmap bitmap, int designImageWidth = 0)
	{
		using MemoryStream memoryStream = new MemoryStream();
		bitmap.Save(memoryStream, ImageFormat.Png);
		memoryStream.Position = 0L;
		BitmapImage bitmapImage = new BitmapImage();
		bitmapImage.BeginInit();
		memoryStream.Seek(0L, SeekOrigin.Begin);
		if (designImageWidth != 0)
		{
			bitmapImage.DecodePixelHeight = designImageWidth;
			bitmapImage.DecodePixelWidth = designImageWidth;
		}
		bitmapImage.StreamSource = memoryStream;
		bitmapImage.CacheOption = BitmapCacheOption.OnLoad;
		bitmapImage.EndInit();
		return bitmapImage;
	}

	public static Bitmap ConvertByteArrayToBitmap(byte[] bmp)
	{
		if (bmp != null)
		{
			return (Bitmap)TypeDescriptor.GetConverter(typeof(Bitmap)).ConvertFrom(bmp);
		}
		return null;
	}

	public static byte[] ConvertImageToByteArray(System.Drawing.Image image)
	{
		byte[] array = new byte[0];
		using MemoryStream memoryStream = new MemoryStream();
		image.Save(memoryStream, ImageFormat.Png);
		return memoryStream.ToArray();
	}

	public static Bitmap ConvertRenderTargetBitmapToBitmap(RenderTargetBitmap renderTargetBitmap)
	{
		MemoryStream stream = new MemoryStream();
		BmpBitmapEncoder bmpBitmapEncoder = new BmpBitmapEncoder();
		bmpBitmapEncoder.Frames.Add(BitmapFrame.Create(renderTargetBitmap));
		bmpBitmapEncoder.Save(stream);
		return new Bitmap(stream);
	}

	public static void ConvertToJpeg(UIElement uiElement, string path, double dpi)
	{
		string value = CreateJpeg(ConvertToBitmap(uiElement, dpi));
		if (path == null)
		{
			return;
		}
		try
		{
			using FileStream fileStream = File.Create(path);
			using (StreamWriter streamWriter = new StreamWriter(fileStream, Encoding.Default))
			{
				streamWriter.Write(value);
				streamWriter.Close();
			}
			fileStream.Close();
		}
		catch (Exception)
		{
		}
	}

	public static RenderTargetBitmap CreateBitmapFromFrameworkElement(FrameworkElement element)
	{
		double actualWidth = element.ActualWidth;
		double actualHeight = element.ActualHeight;
		RenderTargetBitmap renderTargetBitmap = new RenderTargetBitmap((int)Math.Round(actualWidth), (int)Math.Round(actualHeight), 96.0, 96.0, PixelFormats.Default);
		DrawingVisual drawingVisual = new DrawingVisual();
		using (DrawingContext drawingContext = drawingVisual.RenderOpen())
		{
			VisualBrush brush = new VisualBrush(element);
			drawingContext.DrawRectangle(brush, null, new Rect(default(System.Windows.Point), new System.Windows.Size(actualWidth, actualHeight)));
		}
		renderTargetBitmap.Render(drawingVisual);
		renderTargetBitmap.Freeze();
		return renderTargetBitmap;
	}

	public static void ExportBitmapImageAsPNG(BitmapSource imageSource, string exportPath)
	{
		if (imageSource == null)
		{
			return;
		}
		Application.Current.Dispatcher.Invoke(delegate
		{
			imageSource.Freeze();
			using FileStream fileStream = new FileStream(exportPath, FileMode.Create);
			PngBitmapEncoder pngBitmapEncoder = new PngBitmapEncoder();
			pngBitmapEncoder.Frames.Add(BitmapFrame.Create(imageSource));
			pngBitmapEncoder.Save(fileStream);
			fileStream.Close();
		});
	}

	public static void ExportDrawingImageAsPNG(ImageSource imageSource, string exportPath)
	{
		if (imageSource != null)
		{
			imageSource.Freeze();
			DrawingVisual drawingVisual = new DrawingVisual();
			DrawingContext drawingContext = drawingVisual.RenderOpen();
			drawingContext.DrawImage(imageSource, new Rect(0.0, 0.0, imageSource.Width, imageSource.Height));
			drawingContext.Close();
			RenderTargetBitmap renderTargetBitmap = new RenderTargetBitmap((int)imageSource.Width, (int)imageSource.Height, 96.0, 96.0, PixelFormats.Default);
			renderTargetBitmap.Render(drawingVisual);
			renderTargetBitmap.Freeze();
			using FileStream fileStream = new FileStream(exportPath, FileMode.Create);
			PngBitmapEncoder pngBitmapEncoder = new PngBitmapEncoder();
			pngBitmapEncoder.Frames.Add(BitmapFrame.Create(renderTargetBitmap));
			pngBitmapEncoder.Save(fileStream);
			fileStream.Close();
		}
	}

	public static void ExportDrawingImageStretchedAsJPEG(ImageSource imageSource, string exportPath, double height, double width, Stretch stretch)
	{
		Grid grid = new Grid();
		grid.Height = height;
		grid.Width = width;
		System.Windows.Controls.Image element = new System.Windows.Controls.Image
		{
			Source = imageSource,
			Stretch = stretch,
			VerticalAlignment = VerticalAlignment.Center,
			HorizontalAlignment = HorizontalAlignment.Center
		};
		grid.Children.Add(element);
		grid.Arrange(new Rect(0.0, 0.0, width, height));
		ConvertToJpeg(grid, exportPath, 96.0);
	}

	public static Bitmap GetCroppedImage(string path, int height, int width)
	{
		Rectangle rect = new Rectangle
		{
			Height = height,
			Width = width,
			Location = new System.Drawing.Point(0, 0)
		};
		try
		{
			Bitmap bitmap = new Bitmap(path);
			return bitmap.Clone(rect, bitmap.PixelFormat);
		}
		catch (Exception ex)
		{
			DebuggingHelper.PrintTimedOutput(ex.ToString());
			return null;
		}
	}

	public static Bitmap GetCroppedRoundedImage(string path, int cornerRadius, int height, int width)
	{
		Bitmap bitmap = new Bitmap(width, height);
		Rectangle rect = new Rectangle
		{
			Height = height,
			Width = width,
			Location = new System.Drawing.Point(0, 0)
		};
		using (Bitmap bitmap2 = new Bitmap(path))
		{
			using Bitmap bitmap3 = bitmap2.Clone(rect, bitmap2.PixelFormat);
			using Graphics graphics = Graphics.FromImage(bitmap);
			graphics.Clear(System.Drawing.Color.White);
			graphics.SmoothingMode = SmoothingMode.HighQuality;
			graphics.CompositingQuality = CompositingQuality.HighQuality;
			graphics.InterpolationMode = InterpolationMode.HighQualityBicubic;
			using System.Drawing.Brush brush = new TextureBrush(bitmap3);
			GraphicsPath graphicsPath = new GraphicsPath();
			graphicsPath.AddArc(-1, -1, cornerRadius, cornerRadius, 180f, 90f);
			graphicsPath.AddArc(width - cornerRadius, -1, cornerRadius, cornerRadius, 270f, 90f);
			graphicsPath.AddArc(width - cornerRadius, height - cornerRadius, cornerRadius, cornerRadius, 0f, 90f);
			graphicsPath.AddArc(-1, height - cornerRadius, cornerRadius, cornerRadius, 90f, 90f);
			graphics.FillPath(brush, graphicsPath);
		}
		bitmap.MakeTransparent(System.Drawing.Color.White);
		return bitmap;
	}

	public static DrawingImage GetImageFromResourceDictionaryByName(string imageName)
	{
		DrawingImage result = null;
		object obj = Application.Current.TryFindResource(imageName);
		if (obj != null && obj is DrawingBrush drawingBrush)
		{
			result = new DrawingImage(drawingBrush.Drawing);
		}
		return result;
	}

	public static PixelColor[,] GetPixels(BitmapSource source)
	{
		if (source.Format != PixelFormats.Bgra32)
		{
			source = new FormatConvertedBitmap(source, PixelFormats.Bgra32, null, 0.0);
		}
		int pixelWidth = source.PixelWidth;
		int pixelHeight = source.PixelHeight;
		PixelColor[,] array = new PixelColor[pixelWidth, pixelHeight];
		source.CopyPixelsFast(array, pixelWidth * 4, 0);
		return array;
	}

	public static void PutPixels(WriteableBitmap bitmap, PixelColor[,] pixels, int x, int y)
	{
		int length = pixels.GetLength(0);
		int length2 = pixels.GetLength(1);
		bitmap.WritePixels(new Int32Rect(0, 0, length, length2), pixels, length * 4, x, y);
	}

	public static System.Drawing.Color UIntToColor(uint color)
	{
		byte alpha = (byte)(color >> 24);
		byte red = (byte)(color >> 16);
		byte green = (byte)(color >> 8);
		byte blue = (byte)color;
		return System.Drawing.Color.FromArgb(alpha, red, green, blue);
	}

	private static RenderTargetBitmap ConvertToBitmap(UIElement uiElement, double dpi)
	{
		double num = dpi / 96.0;
		uiElement.Measure(new System.Windows.Size(double.PositiveInfinity, double.PositiveInfinity));
		System.Windows.Size desiredSize = uiElement.DesiredSize;
		Rect finalRect = new Rect(desiredSize);
		uiElement.Arrange(finalRect);
		RenderTargetBitmap renderTargetBitmap = new RenderTargetBitmap((int)(num * ((finalRect.Width > 0.0) ? finalRect.Width : 1.0)), (int)(num * ((finalRect.Height > 0.0) ? finalRect.Height : 1.0)), num * 96.0, num * 96.0, PixelFormats.Default);
		renderTargetBitmap.Render(uiElement);
		return renderTargetBitmap;
	}

	private static string CreateJpeg(RenderTargetBitmap bitmap)
	{
		JpegBitmapEncoder jpegBitmapEncoder = new JpegBitmapEncoder();
		jpegBitmapEncoder.Frames.Add(BitmapFrame.Create(bitmap));
		using MemoryStream memoryStream = new MemoryStream();
		jpegBitmapEncoder.Save(memoryStream);
		memoryStream.Seek(0L, SeekOrigin.Begin);
		string result;
		using (StreamReader streamReader = new StreamReader(memoryStream, Encoding.Default))
		{
			result = streamReader.ReadToEnd();
			streamReader.Close();
		}
		memoryStream.Close();
		return result;
	}
}
