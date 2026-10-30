using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;
using DevExpress.Xpf.LayoutControl;

namespace GV.CounterSketch.Studio.Utilities.Converters;

public class TileSizeToTextVisibilityConverter : IValueConverter
{
	public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_0006: Unknown result type (might be due to invalid IL or missing references)
		//IL_0028: Unknown result type (might be due to invalid IL or missing references)
		//IL_001b: Unknown result type (might be due to invalid IL or missing references)
		TileSize val = (TileSize)value;
		if (parameter != null)
		{
			bool.TryParse(parameter.ToString(), out var result);
			if (result)
			{
				return ((int)val != 0) ? Visibility.Collapsed : Visibility.Visible;
			}
		}
		return ((int)val == 0) ? Visibility.Collapsed : Visibility.Visible;
	}

	public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
	{
		throw new NotImplementedException();
	}
}
