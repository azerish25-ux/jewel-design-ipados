using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Markup;
using GV.CounterSketch.Studio.Model.Objects.Enums;
using GV.CounterSketch.Studio.Utilities.Helpers;

namespace GV.CounterSketch.Studio.Utilities.Converters;

public class GemColorTypeEnumToDescriptionConverter : MarkupExtension, IValueConverter
{
	private static GemColorTypeEnumToDescriptionConverter _converter;

	public override object ProvideValue(IServiceProvider serviceProvider)
	{
		return _converter ?? (_converter = new GemColorTypeEnumToDescriptionConverter());
	}

	public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
	{
		if (!(value is Enum))
		{
			if (value is Array)
			{
				List<string> list = new List<string>();
				{
					foreach (object item in (IEnumerable)value)
					{
						if (item is Enum)
						{
							list.Add(EnumHelper.GetEnumDescriptionFromValue((Enum)item));
						}
					}
					return list;
				}
			}
			return value;
		}
		return EnumHelper.GetEnumDescriptionFromValue((Enum)value);
	}

	public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
	{
		return EnumHelper.GetEnumValueFromDescription(value.ToString(), typeof(GemColorType));
	}
}
