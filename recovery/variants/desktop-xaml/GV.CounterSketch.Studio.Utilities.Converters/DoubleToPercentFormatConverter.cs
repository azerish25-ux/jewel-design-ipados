using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Markup;

namespace GV.CounterSketch.Studio.Utilities.Converters;

public class DoubleToPercentFormatConverter : MarkupExtension, IValueConverter
{
	private static DoubleToPercentFormatConverter _converter;

	public override object ProvideValue(IServiceProvider serviceProvider)
	{
		return _converter ?? (_converter = new DoubleToPercentFormatConverter());
	}

	object IValueConverter.Convert(object value, Type targetType, object parameter, CultureInfo culture)
	{
		if (value is double num)
		{
			return num.ToString("P0");
		}
		return null;
	}

	object IValueConverter.ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
	{
		throw new Exception("The method or operation is not implemented.");
	}
}
