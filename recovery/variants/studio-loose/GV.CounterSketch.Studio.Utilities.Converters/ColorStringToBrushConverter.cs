using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Markup;
using System.Windows.Media;

namespace GV.CounterSketch.Studio.Utilities.Converters;

public class ColorStringToBrushConverter : MarkupExtension, IValueConverter
{
	private static ColorStringToBrushConverter _converter;

	public override object ProvideValue(IServiceProvider serviceProvider)
	{
		return _converter ?? (_converter = new ColorStringToBrushConverter());
	}

	public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
	{
		Color color = Colors.Blue;
		if (value is string)
		{
			color = (Color)ColorConverter.ConvertFromString(value as string);
		}
		return new SolidColorBrush(color);
	}

	public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
	{
		return ((SolidColorBrush)value).Color;
	}
}
