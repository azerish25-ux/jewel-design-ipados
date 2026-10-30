using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Markup;

namespace GV.CounterSketch.Studio.Utilities.Converters;

internal class BooleanToVariableOpacityConverter : MarkupExtension, IValueConverter
{
	private static BooleanToVariableOpacityConverter _converter;

	public override object ProvideValue(IServiceProvider serviceProvider)
	{
		if (_converter == null)
		{
			_converter = new BooleanToVariableOpacityConverter();
		}
		return _converter;
	}

	public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
	{
		bool num = bool.Parse(value.ToString());
		double result = 0.0;
		if (parameter != null)
		{
			double.TryParse(parameter.ToString(), out result);
		}
		if (num)
		{
			return 1;
		}
		return result;
	}

	public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
	{
		throw new Exception("The method or operation is not implemented.");
	}
}
