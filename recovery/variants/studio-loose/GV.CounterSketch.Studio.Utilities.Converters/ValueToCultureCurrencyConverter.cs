using System;
using System.Globalization;
using System.Windows.Data;
using GV.CounterSketch.Studio.Utilities.Helpers;

namespace GV.CounterSketch.Studio.Utilities.Converters;

internal class ValueToCultureCurrencyConverter : IValueConverter
{
	public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
	{
		return string.Format(new CultureInfo(UIHelper.CounterSketchOptions.CurrentCultureCode), "{0:C}", value);
	}

	public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
	{
		throw new NotImplementedException();
	}
}
