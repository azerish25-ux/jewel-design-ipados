using System;
using System.Globalization;
using System.IO;
using System.Windows.Data;
using System.Windows.Markup;
using System.Windows.Media.Imaging;
using GV.CounterSketch.Studio.Model.Objects;
using GV.CounterSketch.Studio.Utilities.Helpers;

namespace GV.CounterSketch.Studio.Utilities.Converters;

public class DesignToFavoriteImageConverter : MarkupExtension, IValueConverter
{
	private static DesignToFavoriteImageConverter _converter;

	public object Convert(object values, Type targetType, object parameter, CultureInfo culture)
	{
		if (values is Design design)
		{
			string text = ((!design.IsBaseDesign) ? design.FavoriteImagePathSearchOrientation : design.BaseDesignImagePath);
			if (text != null && File.Exists(text))
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
		}
		return UIHelper.GetBlankImage();
	}

	public object ConvertBack(object value, Type targetTypes, object parameter, CultureInfo culture)
	{
		throw new NotImplementedException();
	}

	public override object ProvideValue(IServiceProvider serviceProvider)
	{
		return _converter ?? (_converter = new DesignToFavoriteImageConverter());
	}
}
