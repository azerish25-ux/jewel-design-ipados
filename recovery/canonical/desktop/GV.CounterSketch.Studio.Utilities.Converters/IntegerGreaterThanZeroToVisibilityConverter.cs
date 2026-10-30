using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;
using System.Windows.Markup;

namespace GV.CounterSketch.Studio.Utilities.Converters;

public class IntegerGreaterThanZeroToVisibilityConverter : MarkupExtension, IValueConverter
{
	private static IntegerGreaterThanZeroToVisibilityConverter _converter;

	public override object ProvideValue(IServiceProvider serviceProvider)
	{
		return _converter ?? (_converter = new IntegerGreaterThanZeroToVisibilityConverter());
	}

	public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
	{
		bool result = false;
		if (parameter != null)
		{
			bool.TryParse(parameter.ToString(), out result);
		}
		if (value == null)
		{
			return Visibility.Collapsed;
		}
		int.TryParse(value.ToString(), out var result2);
		if (result2 > 0)
		{
			if (result)
			{
				return Visibility.Collapsed;
			}
			return Visibility.Visible;
		}
		if (result)
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
