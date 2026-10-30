using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;
using System.Windows.Markup;

namespace GV.CounterSketch.Studio.Utilities.Converters;

public class BooleanToVisibilityHiddenConverter : MarkupExtension, IValueConverter
{
	private static BooleanToVisibilityHiddenConverter _converter;

	public override object ProvideValue(IServiceProvider serviceProvider)
	{
		return _converter ?? (_converter = new BooleanToVisibilityHiddenConverter());
	}

	public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
	{
		bool flag = bool.Parse(value.ToString());
		if (parameter != null)
		{
			bool.TryParse(parameter.ToString(), out var result);
			if (result)
			{
				flag = !flag;
			}
		}
		return (!flag) ? Visibility.Hidden : Visibility.Visible;
	}

	public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
	{
		throw new Exception("The method or operation is not implemented.");
	}
}
