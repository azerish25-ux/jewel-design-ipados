using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;
using System.Windows.Markup;

namespace GV.CounterSketch.Studio.Utilities.Converters;

[ValueConversion(typeof(Visibility), typeof(Visibility))]
public class InvertVisibilityConverter : MarkupExtension, IValueConverter
{
	private static InvertVisibilityConverter _converter;

	public override object ProvideValue(IServiceProvider serviceProvider)
	{
		return _converter ?? (_converter = new InvertVisibilityConverter());
	}

	public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
	{
		if ((Visibility)value == Visibility.Collapsed)
		{
			return Visibility.Visible;
		}
		return Visibility.Collapsed;
	}

	public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
	{
		throw new Exception("The method or operation is not implemented.");
	}
}
