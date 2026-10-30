using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Markup;

namespace GV.CounterSketch.Studio.Utilities.Converters;

public class BooleanToOpacityConverter : MarkupExtension, IValueConverter
{
	private static BooleanToOpacityConverter _converter;

	public override object ProvideValue(IServiceProvider serviceProvider)
	{
		if (_converter == null)
		{
			_converter = new BooleanToOpacityConverter();
		}
		return _converter;
	}

	public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
	{
		bool flag = bool.Parse(value.ToString());
		bool result = false;
		if (parameter != null)
		{
			bool.TryParse(parameter.ToString(), out result);
		}
		if (result)
		{
			flag = !flag;
		}
		if (flag)
		{
			return 1;
		}
		return 0.45;
	}

	public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
	{
		throw new Exception("The method or operation is not implemented.");
	}
}
