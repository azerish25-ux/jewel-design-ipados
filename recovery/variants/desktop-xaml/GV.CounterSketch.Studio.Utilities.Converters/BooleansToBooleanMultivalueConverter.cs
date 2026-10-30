using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Windows.Data;
using System.Windows.Markup;

namespace GV.CounterSketch.Studio.Utilities.Converters;

public class BooleansToBooleanMultivalueConverter : MarkupExtension, IMultiValueConverter
{
	private static BooleansToBooleanMultivalueConverter _converter;

	public override object ProvideValue(IServiceProvider serviceProvider)
	{
		if (_converter == null)
		{
			_converter = new BooleansToBooleanMultivalueConverter();
		}
		return _converter;
	}

	public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
	{
		List<bool> list = values.OfType<bool>().ToList();
		bool returnValue = true;
		list.ForEach(delegate(bool providedValue)
		{
			returnValue &= providedValue;
		});
		return returnValue;
	}

	public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
	{
		if (targetTypes != null)
		{
			object[] array = new object[targetTypes.Count() - 1];
			bool flag = true;
			if (value is bool)
			{
				flag = (bool)value;
			}
			for (int i = 0; i < targetTypes.Count() - 1; i++)
			{
				array[i] = flag;
			}
			return array;
		}
		return null;
	}
}
