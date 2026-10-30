using System;
using System.Globalization;
using System.Threading;
using System.Windows.Data;

namespace GV.CounterSketch.Studio.Utilities.Converters;

public class StringToUpperCaseConverter : IValueConverter
{
	public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
	{
		string text = (string)value;
		if (text != null)
		{
			return Thread.CurrentThread.CurrentCulture.TextInfo.ToTitleCase(text.ToUpper());
		}
		return text;
	}

	public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
	{
		throw new NotImplementedException();
	}
}
