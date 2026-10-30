using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;
using System.Windows.Markup;

namespace GV.CounterSketch.Studio.Utilities.Converters;

public class OrderQuantityToVisibilityConverter : MarkupExtension, IValueConverter
{
	private static OrderQuantityToVisibilityConverter _converter;

	public override object ProvideValue(IServiceProvider serviceProvider)
	{
		return _converter ?? (_converter = new OrderQuantityToVisibilityConverter());
	}

	public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
	{
		int result = 0;
		if (value != null)
		{
			int.TryParse(value.ToString(), out result);
		}
		if (result > 0)
		{
			return Visibility.Visible;
		}
		return Visibility.Collapsed;
	}

	public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
	{
		return new NotImplementedException();
	}
}
