using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Markup;
using System.Windows.Media;
using GV.CounterSketch.Studio.Utilities.Helpers;

namespace GV.CounterSketch.Studio.Utilities.Converters;

public class StringToResourceDictionaryDrawingImageConverter : MarkupExtension, IValueConverter
{
	private static StringToResourceDictionaryDrawingImageConverter _converter;

	public override object ProvideValue(IServiceProvider serviceProvider)
	{
		return _converter ?? (_converter = new StringToResourceDictionaryDrawingImageConverter());
	}

	public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
	{
		string text = value as string;
		if (!string.IsNullOrWhiteSpace(text))
		{
			DrawingImage imageFromResourceDictionaryByName = ImageHelper.GetImageFromResourceDictionaryByName(text);
			if (imageFromResourceDictionaryByName != null)
			{
				return imageFromResourceDictionaryByName;
			}
		}
		return Binding.DoNothing;
	}

	public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
	{
		throw new NotImplementedException();
	}
}
