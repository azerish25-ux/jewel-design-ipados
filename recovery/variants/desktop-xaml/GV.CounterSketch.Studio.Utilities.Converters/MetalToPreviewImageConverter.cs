using System;
using System.Globalization;
using System.Windows.Data;
using GV.CounterSketch.Shared;
using GV.CounterSketch.Studio.Utilities.Helpers;

namespace GV.CounterSketch.Studio.Utilities.Converters;

internal class MetalToPreviewImageConverter : IValueConverter
{
	public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
	{
		if (value != null)
		{
			return MetalHelper.GetImageForMetals((MetalMaterial)Enum.Parse(typeof(MetalMaterial), value.ToString()));
		}
		return null;
	}

	public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
	{
		throw new NotImplementedException();
	}
}
