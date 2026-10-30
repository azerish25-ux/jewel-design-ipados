using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace GV.CounterSketch.Studio.Utilities.Converters;

public class BooleanToVisibilityConverter : IValueConverter
{
	public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
	{
		if (value != null && bool.TryParse(value.ToString(), out var result))
		{
			if (parameter != null)
			{
				bool.TryParse(parameter.ToString(), out var result2);
				if (result2)
				{
					result = !result;
				}
			}
			return (!result) ? Visibility.Collapsed : Visibility.Visible;
		}
		return Visibility.Collapsed;
	}

	public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
	{
		throw new Exception("The method or operation is not implemented.");
	}
}
