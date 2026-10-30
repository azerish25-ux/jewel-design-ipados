using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Markup;
using System.Windows.Media;

namespace GV.CounterSketch.Studio.Utilities.Converters;

public class PartsTabNameToBackgroundColorConverter : MarkupExtension, IValueConverter
{
	private static PartsTabNameToBackgroundColorConverter _converter;

	public override object ProvideValue(IServiceProvider serviceProvider)
	{
		return _converter ?? (_converter = new PartsTabNameToBackgroundColorConverter());
	}

	public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
	{
		return new SolidColorBrush(Color.FromArgb(byte.MaxValue, 204, 198, 193));
	}

	public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
	{
		throw new NotImplementedException();
	}
}
