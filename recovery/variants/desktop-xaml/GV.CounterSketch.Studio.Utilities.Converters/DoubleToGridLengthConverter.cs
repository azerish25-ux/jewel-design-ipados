using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace GV.CounterSketch.Studio.Utilities.Converters;

public class DoubleToGridLengthConverter : IValueConverter
{
	public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
	{
		if (value == null)
		{
			return null;
		}
		return new GridLength(double.Parse(value.ToString()));
	}

	public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
	{
		if (value is GridLength)
		{
			return new GridLengthConverter().ConvertTo(value, typeof(GridLength));
		}
		return null;
	}
}
