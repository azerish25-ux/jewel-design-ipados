using System;
using System.Collections;
using System.Globalization;
using System.Windows.Data;

namespace GV.CounterSketch.Studio.Utilities.Converters;

internal class NullListItemToDoNothingConverter : IValueConverter
{
	public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
	{
		if (value is IList list && parameter is string s)
		{
			int result = 0;
			int.TryParse(s, out result);
			if (list.Count < result + 1)
			{
				return Binding.DoNothing;
			}
			return list[result];
		}
		return Binding.DoNothing;
	}

	public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
	{
		throw new NotImplementedException();
	}
}
