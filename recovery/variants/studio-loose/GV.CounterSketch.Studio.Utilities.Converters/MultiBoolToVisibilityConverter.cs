using System;
using System.Globalization;
using System.Linq;
using System.Windows;
using System.Windows.Data;

namespace GV.CounterSketch.Studio.Utilities.Converters;

public class MultiBoolToVisibilityConverter : IMultiValueConverter
{
	public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
	{
		bool flag = values.OfType<bool>().All((bool _) => _);
		if (parameter != null)
		{
			bool.TryParse(parameter.ToString(), out var result);
			if (result)
			{
				flag = !flag;
			}
		}
		return (!flag) ? Visibility.Collapsed : Visibility.Visible;
	}

	public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
	{
		throw new NotImplementedException();
	}
}
