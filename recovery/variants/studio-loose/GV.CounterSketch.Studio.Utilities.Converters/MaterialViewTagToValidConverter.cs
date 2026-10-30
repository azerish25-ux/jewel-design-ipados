using System;
using System.Globalization;
using System.Windows.Data;
using GV.CounterSketch.Studio.ViewModel;

namespace GV.CounterSketch.Studio.Utilities.Converters;

public class MaterialViewTagToValidConverter : IMultiValueConverter
{
	public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
	{
		if (values != null && values.Length == 3)
		{
			string text = values[0].ToString();
			if (!bool.Parse(values[1].ToString()))
			{
				return false;
			}
			string value = values[2].ToString();
			switch ((Sections)Enum.Parse(typeof(Sections), value))
			{
			case Sections.Search:
				if (text == "SearchView")
				{
					return true;
				}
				break;
			case Sections.Customize:
				if (text == "CustomizeView")
				{
					return true;
				}
				break;
			default:
				throw new ArgumentOutOfRangeException();
			case Sections.Tiles:
			case Sections.Order:
				break;
			}
		}
		return false;
	}

	public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
	{
		throw new NotImplementedException();
	}
}
