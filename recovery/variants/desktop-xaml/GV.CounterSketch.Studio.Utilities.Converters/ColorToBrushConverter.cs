using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Markup;
using System.Windows.Media;

namespace GV.CounterSketch.Studio.Utilities.Converters;

public class ColorToBrushConverter : MarkupExtension, IValueConverter
{
	private static ColorToBrushConverter _converter;

	public override object ProvideValue(IServiceProvider serviceProvider)
	{
		return _converter ?? (_converter = new ColorToBrushConverter());
	}

	public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
	{
		return new SolidColorBrush((Color)value);
	}

	public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
	{
		return ((SolidColorBrush)value).Color;
	}
}
