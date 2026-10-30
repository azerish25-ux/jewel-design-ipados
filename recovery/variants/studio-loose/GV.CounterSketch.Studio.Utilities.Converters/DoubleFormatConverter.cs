using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Markup;

namespace GV.CounterSketch.Studio.Utilities.Converters;

public class DoubleFormatConverter : MarkupExtension, IValueConverter
{
	private static DoubleFormatConverter _converter;

	public override object ProvideValue(IServiceProvider serviceProvider)
	{
		return _converter ?? (_converter = new DoubleFormatConverter());
	}

	public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
	{
		if (parameter is string format)
		{
			return string.Format(culture, format, value);
		}
		return Math.Round((double)value, 4).ToString();
	}

	public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
	{
		try
		{
			string text = value.ToString();
			if (string.IsNullOrEmpty(text))
			{
				return 0;
			}
			if (text == "-")
			{
				return 0;
			}
			return double.Parse(text);
		}
		catch (Exception)
		{
			return 0;
		}
	}
}
