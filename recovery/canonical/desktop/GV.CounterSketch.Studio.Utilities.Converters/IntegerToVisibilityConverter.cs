using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;
using System.Windows.Markup;

namespace GV.CounterSketch.Studio.Utilities.Converters;

public class IntegerToVisibilityConverter : MarkupExtension, IValueConverter
{
	private static IntegerToVisibilityConverter _converter;

	public override object ProvideValue(IServiceProvider serviceProvider)
	{
		return _converter ?? (_converter = new IntegerToVisibilityConverter());
	}

	public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
	{
		int result = 0;
		if (parameter != null)
		{
			int.TryParse(parameter.ToString(), out result);
		}
		if (value == null)
		{
			return Visibility.Collapsed;
		}
		int.TryParse(value.ToString(), out var result2);
		if (result2 > result)
		{
			return Visibility.Visible;
		}
		return Visibility.Collapsed;
	}

	public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
	{
		throw new Exception("The method or operation is not implemented.");
	}
}
