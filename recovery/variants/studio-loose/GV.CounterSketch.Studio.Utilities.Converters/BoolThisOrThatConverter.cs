using System;
using System.Globalization;
using System.Windows.Data;

namespace GV.CounterSketch.Studio.Utilities.Converters;

public class BoolThisOrThatConverter : IMultiValueConverter
{
	public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
	{
		if (values.Length == 3)
		{
			bool.TryParse(values[0].ToString(), out var result);
			if (result)
			{
				return values[1];
			}
			return values[2];
		}
		return Binding.DoNothing;
	}

	public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
	{
		throw new NotImplementedException();
	}
}
