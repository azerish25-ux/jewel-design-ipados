using System;
using System.Globalization;
using System.Windows.Data;
using GV.CounterSketch.Studio.Utilities.Helpers;
using Localization;

namespace GV.CounterSketch.Studio.Utilities.Converters;

internal class EnumToTranslatedDescriptionConverter : IValueConverter
{
	public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
	{
		if (value != null && value.ToString().Contains("Disconnected"))
		{
			return Binding.DoNothing;
		}
		return TranslationHelper.GetTranslatedString(EnumHelper.GetEnumDescriptionFromValue((Enum)value));
	}

	public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
	{
		throw new NotImplementedException();
	}
}
