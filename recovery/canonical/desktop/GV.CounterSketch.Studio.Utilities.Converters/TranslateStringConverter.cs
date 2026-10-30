using System;
using System.Globalization;
using System.Windows.Data;
using Localization;

namespace GV.CounterSketch.Studio.Utilities.Converters;

internal class TranslateStringConverter : IValueConverter
{
	public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
	{
		if (value == null)
		{
			return null;
		}
		return TranslationHelper.GetTranslatedString(value as string);
	}

	public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
	{
		throw new NotImplementedException();
	}
}
