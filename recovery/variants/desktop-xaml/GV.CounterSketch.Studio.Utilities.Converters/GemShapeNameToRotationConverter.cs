using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Markup;
using GV.CounterSketch.Studio.Utilities.Helpers;

namespace GV.CounterSketch.Studio.Utilities.Converters;

public class GemShapeNameToRotationConverter : MarkupExtension, IValueConverter
{
	private static GemShapeNameToRotationConverter _converter;

	public override object ProvideValue(IServiceProvider serviceProvider)
	{
		return _converter ?? (_converter = new GemShapeNameToRotationConverter());
	}

	public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
	{
		string text = value as string;
		if (!string.IsNullOrWhiteSpace(text))
		{
			return GemHelper.GetGemShapeRotationAngle(text);
		}
		return Binding.DoNothing;
	}

	public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
	{
		return null;
	}
}
