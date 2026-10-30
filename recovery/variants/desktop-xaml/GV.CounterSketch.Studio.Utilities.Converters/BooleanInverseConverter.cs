using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Markup;

namespace GV.CounterSketch.Studio.Utilities.Converters;

public class BooleanInverseConverter : MarkupExtension, IValueConverter
{
	private static BooleanInverseConverter _converter;

	public override object ProvideValue(IServiceProvider serviceProvider)
	{
		if (_converter == null)
		{
			_converter = new BooleanInverseConverter();
		}
		return _converter;
	}

	public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
	{
		bool flag = false;
		if (value is bool)
		{
			flag = !(bool)value;
		}
		if (parameter is bool && (bool)parameter)
		{
			flag = !flag;
		}
		return flag;
	}

	public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
	{
		bool flag = false;
		if (value is bool)
		{
			flag = !(bool)value;
		}
		return flag;
	}
}
