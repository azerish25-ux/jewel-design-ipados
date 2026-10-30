using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Media.Imaging;

namespace GV.CounterSketch.Studio.Utilities.Converters;

public class GemShapeStringToImageConverter : IValueConverter
{
	public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
	{
		if (value != null && value.ToString().Length > 3)
		{
			string text = null;
			try
			{
				string text2 = value.ToString();
				if (text2.ToLower().Contains("square"))
				{
					text2 = "Sq. Cushion";
				}
				if (text2.ToLower().Contains("half"))
				{
					text2 = "Half Moon";
				}
				text = "pack://application:,,,/CounterSketch Studio;component/Images/Search/FilterItems/GemShapes/" + text2 + ".png";
				BitmapImage bitmapImage = new BitmapImage();
				bitmapImage.BeginInit();
				bitmapImage.CacheOption = BitmapCacheOption.OnLoad;
				bitmapImage.UriSource = new Uri(text);
				bitmapImage.EndInit();
				if (bitmapImage.CanFreeze)
				{
					bitmapImage.Freeze();
				}
				return bitmapImage;
			}
			catch (Exception)
			{
			}
		}
		return Binding.DoNothing;
	}

	public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
	{
		throw new NotImplementedException();
	}
}
