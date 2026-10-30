using System;
using System.Collections.Generic;
using System.Globalization;
using System.Windows;
using System.Windows.Data;
using System.Windows.Markup;
using System.Windows.Media;

namespace GV.CounterSketch.Studio.Utilities.Converters;

public class PointToPolyPointConverter : MarkupExtension, IValueConverter
{
	private static PointToPolyPointConverter _converter;

	public override object ProvideValue(IServiceProvider serviceProvider)
	{
		return _converter ?? (_converter = new PointToPolyPointConverter());
	}

	public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
	{
		if (value is IEnumerable<Point> collection)
		{
			return new PointCollection(collection);
		}
		return null;
	}

	public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
	{
		throw new NotImplementedException();
	}
}
