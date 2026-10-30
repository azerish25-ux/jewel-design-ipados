using System;
using System.Globalization;
using System.IO;
using System.Windows.Data;
using System.Windows.Media.Imaging;

namespace GV.CounterSketch.Studio.Utilities.Converters;

public class ImagePathToOptionalBitmapImageConverter : IValueConverter
{
	public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
	{
		string text = value as string;
		if (string.IsNullOrEmpty(text) || !File.Exists(text))
		{
			return Binding.DoNothing;
		}
		BitmapImage bitmapImage = new BitmapImage();
		using WrappingStream streamSource = new WrappingStream(new MemoryStream(File.ReadAllBytes(text), writable: false));
		bitmapImage.BeginInit();
		bitmapImage.CacheOption = BitmapCacheOption.OnLoad;
		bitmapImage.StreamSource = streamSource;
		bitmapImage.EndInit();
		bitmapImage.Freeze();
		return bitmapImage;
	}

	public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
	{
		throw new NotImplementedException();
	}
}
