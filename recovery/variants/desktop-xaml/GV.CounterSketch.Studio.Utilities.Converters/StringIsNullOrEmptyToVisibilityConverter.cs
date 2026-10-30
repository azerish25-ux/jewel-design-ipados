using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;
using System.Windows.Markup;

namespace GV.CounterSketch.Studio.Utilities.Converters;

public class StringIsNullOrEmptyToVisibilityConverter : MarkupExtension, IValueConverter
{
	private static StringIsNullOrEmptyToVisibilityConverter _converter;

	public override object ProvideValue(IServiceProvider serviceProvider)
	{
		return _converter ?? (_converter = new StringIsNullOrEmptyToVisibilityConverter());
	}

	public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
	{
		return string.IsNullOrEmpty(value as string) ? Visibility.Collapsed : Visibility.Visible;
	}

	public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
	{
		throw new NotImplementedException();
	}
}
