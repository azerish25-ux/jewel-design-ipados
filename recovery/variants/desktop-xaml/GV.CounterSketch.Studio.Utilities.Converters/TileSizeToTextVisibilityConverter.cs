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
		TileSize tileSize = (TileSize)value;
		if (parameter != null)
		{
			bool.TryParse(parameter.ToString(), out var result);
			if (result)
			{
				return (tileSize != TileSize.ExtraSmall) ? Visibility.Collapsed : Visibility.Visible;
			}
		}
		return (tileSize == TileSize.ExtraSmall) ? Visibility.Collapsed : Visibility.Visible;
	}

	public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
	{
		throw new NotImplementedException();
	}
}
