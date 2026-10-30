using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Markup;
using GV.CounterSketch.Studio.Utilities.Helpers;

namespace GV.CounterSketch.Studio.Utilities.Converters;

public class GemShapeNameToVectorGraphicConverter : MarkupExtension, IValueConverter
{
	private static GemShapeNameToVectorGraphicConverter _converter;

	public override object ProvideValue(IServiceProvider serviceProvider)
	{
		return _converter ?? (_converter = new GemShapeNameToVectorGraphicConverter());
	}

	public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
	{
		string text = value as string;
		if (!string.IsNullOrWhiteSpace(text))
		{
			return GemHelper.GetGemShapeImage(text);
		}
		return Binding.DoNothing;
	}

	public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
	{
		return null;
	}
}
