using System;
using System.Globalization;
using System.Windows.Data;

namespace GV.CounterSketch.Studio.Utilities.Converters;

internal class NullObjectToDoNothingConverter : IValueConverter
{
	public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
	{
		if (value == null)
		{
			return Binding.DoNothing;
		}
		return value;
	}

	public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
	{
		throw new NotImplementedException();
	}
}
