using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;
using GV.CounterSketch.Studio.Model;

namespace GV.CounterSketch.Studio.Utilities.Converters;

internal class TileCommandIsCustomToVisibilityConverter : IValueConverter
{
	public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
	{
		bool result = false;
		if (parameter != null)
		{
			bool.TryParse(parameter.ToString(), out result);
		}
		if (((value as TileCommandType?) ?? TileCommandType.SearchByTileSearchCategory) == TileCommandType.CustomTile)
		{
			if (result)
			{
				return Visibility.Collapsed;
			}
			return Visibility.Visible;
		}
		if (result)
		{
			return Visibility.Visible;
		}
		return Visibility.Collapsed;
	}

	public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
	{
		throw new NotImplementedException();
	}
}
