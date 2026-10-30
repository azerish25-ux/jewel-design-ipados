using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace GV.CounterSketch.Studio.Utilities.Converters;

public class TwoObjectsEqualToVisibilityConverter : IMultiValueConverter
{
	public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
	{
		bool result = false;
		if (parameter != null)
		{
			bool.TryParse(parameter.ToString(), out result);
		}
		if (values.Length == 2 && values[0]?.ToString() == values[1]?.ToString())
		{
			return result ? Visibility.Hidden : Visibility.Visible;
		}
		return (!result) ? Visibility.Hidden : Visibility.Visible;
	}

	public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
	{
		throw new NotImplementedException();
	}
}
