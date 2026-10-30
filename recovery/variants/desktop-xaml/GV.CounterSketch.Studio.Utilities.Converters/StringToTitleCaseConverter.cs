using System;
using System.Globalization;
using System.Threading;
using System.Windows.Data;

namespace GV.CounterSketch.Studio.Utilities.Converters;

public class StringToTitleCaseConverter : IValueConverter
{
	public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
	{
		string text = value as string;
		string text2 = text;
		if (text2 != null)
		{
			text2 = Thread.CurrentThread.CurrentCulture.TextInfo.ToTitleCase(text.ToLower());
		}
		return text2;
	}

	public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
	{
		throw new NotImplementedException();
	}
}
