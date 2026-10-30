using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace GV.CounterSketch.Studio.Utilities.Converters;

public class BooleanNullableToVisibilityConverter : IValueConverter
{
	public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
	{
		bool? flag = value as bool?;
		if (!flag.HasValue)
		{
			return Visibility.Collapsed;
		}
		if (parameter != null)
		{
			bool.TryParse(parameter.ToString(), out var result);
			if (result)
			{
				flag = !flag;
			}
		}
		return (!flag.Value) ? Visibility.Collapsed : Visibility.Visible;
	}

	public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
	{
		throw new NotImplementedException();
	}
}
