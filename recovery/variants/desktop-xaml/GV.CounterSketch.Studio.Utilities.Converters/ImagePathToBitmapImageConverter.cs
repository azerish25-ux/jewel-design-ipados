using System;
using System.Globalization;
using System.IO;
using System.Windows.Data;
using System.Windows.Markup;
using System.Windows.Media.Imaging;

namespace GV.CounterSketch.Studio.Utilities.Converters;

public class ImagePathToBitmapImageConverter : MarkupExtension, IValueConverter
{
	private static ImagePathToBitmapImageConverter _converter;

	public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
	{
		string text = value as string;
		if (text == null)
		{
			return Binding.DoNothing;
		}
		if (text.Contains("http:") || text.Contains("https:"))
		{
			return value;
		}
		if (!text.Contains("pack:"))
		{
			if (File.Exists(text))
			{
				BitmapImage bitmapImage = new BitmapImage();
				using WrappingStream streamSource = new WrappingStream(new MemoryStream(File.ReadAllBytes(text), writable: false));
				bitmapImage.BeginInit();
				bitmapImage.CacheOption = BitmapCacheOption.OnLoad;
				bitmapImage.StreamSource = streamSource;
				bitmapImage.EndInit();
				bitmapImage.Freeze();
				return bitmapImage;
			}
			text = "pack://application:,,,/CounterSketch Studio;component/Images/MissingImage.png";
		}
		try
		{
			BitmapImage bitmapImage2 = new BitmapImage();
			bitmapImage2.BeginInit();
			bitmapImage2.CacheOption = BitmapCacheOption.OnLoad;
			bitmapImage2.UriSource = new Uri(text);
			bitmapImage2.EndInit();
			if (bitmapImage2.CanFreeze)
			{
				bitmapImage2.Freeze();
			}
			return bitmapImage2;
		}
		catch
		{
		}
		return Binding.DoNothing;
	}

	public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
	{
		throw new NotImplementedException();
	}

	public override object ProvideValue(IServiceProvider serviceProvider)
	{
		return _converter ?? (_converter = new ImagePathToBitmapImageConverter());
	}
}
