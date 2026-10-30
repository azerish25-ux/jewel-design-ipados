using System;
using System.Globalization;
using System.Windows.Data;

namespace GV.CounterSketch.Studio.Utilities.Converters;

internal class LanguageCodeToNameConverter : IValueConverter
{
	public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
	{
		string text = value as string;
		if (text == null)
		{
			return new CultureInfo(text).DisplayName;
		}
		return value;
	}

	public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
	{
		throw new NotImplementedException();
	}
}
