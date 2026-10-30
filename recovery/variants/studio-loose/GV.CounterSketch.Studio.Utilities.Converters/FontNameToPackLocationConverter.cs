using System;
using System.Globalization;
using System.Linq;
using System.Windows.Data;
using System.Windows.Markup;

namespace GV.CounterSketch.Studio.Utilities.Converters;

public class FontNameToPackLocationConverter : MarkupExtension, IValueConverter
{
	private static FontNameToPackLocationConverter _converter;

	public override object ProvideValue(IServiceProvider serviceProvider)
	{
		return _converter ?? (_converter = new FontNameToPackLocationConverter());
	}

	public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
	{
		_ = string.Empty;
		return "pack://application,,,/EmbeddedFonts;component/Fonts/#" + value;
	}

	public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
	{
		string empty = string.Empty;
		empty = value as string;
		if (!string.IsNullOrEmpty(empty))
		{
			empty = empty.Split('/').Last().Substring(1);
		}
		return empty;
	}
}
