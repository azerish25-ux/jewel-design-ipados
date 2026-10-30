using System;
using System.Globalization;
using System.IO;
using System.Windows.Data;
using System.Windows.Markup;
using System.Windows.Media.Imaging;

namespace GV.CounterSketch.Studio.Utilities.Converters;

public class ImagePathToBitmapImageWithSizeConverter : MarkupExtension, IMultiValueConverter
{
	private static ImagePathToBitmapImageWithSizeConverter _converter;

	public override object ProvideValue(IServiceProvider serviceProvider)
	{
		return _converter ?? (_converter = new ImagePathToBitmapImageWithSizeConverter());
	}

	public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
	{
		if (values.Length < 3)
		{
			return Binding.DoNothing;
		}
		string text = values[0] as string;
		if (values[1] is double)
		{
			double num = (double)values[1];
			if (values[2] is double)
			{
				double num2 = (double)values[2];
				if (text == null)
				{
					text = "pack://application:,,,/CounterSketch Studio;component/Images/MissingImage.png";
				}
				else if (!text.Contains("pack:"))
				{
					if (File.Exists(text))
					{
						BitmapImage bitmapImage = new BitmapImage();
						using WrappingStream streamSource = new WrappingStream(new MemoryStream(File.ReadAllBytes(text), writable: false));
						bitmapImage.BeginInit();
						bitmapImage.DecodePixelHeight = (int)num;
						bitmapImage.DecodePixelWidth = (int)num2;
						bitmapImage.CacheOption = BitmapCacheOption.OnLoad;
						bitmapImage.StreamSource = streamSource;
						bitmapImage.EndInit();
						bitmapImage.Freeze();
						return bitmapImage;
					}
					text = "pack://application:,,,/CounterSketch Studio;component/Images/MissingImage.png";
				}
				BitmapImage bitmapImage2 = new BitmapImage();
				bitmapImage2.BeginInit();
				bitmapImage2.CacheOption = BitmapCacheOption.OnLoad;
				bitmapImage2.UriSource = new Uri(text);
				bitmapImage2.EndInit();
				bitmapImage2.Freeze();
				return bitmapImage2;
			}
			return Binding.DoNothing;
		}
		return Binding.DoNothing;
	}

	public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
	{
		throw new NotImplementedException();
	}
}
