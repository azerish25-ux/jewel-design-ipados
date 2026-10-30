using System;
using System.Globalization;
using System.Linq;
using System.Windows;
using System.Windows.Data;
using GV.CounterSketch.Studio.ViewModel;
using Stuller.Web.API.v2.Models;

namespace GV.CounterSketch.Studio.Utilities.Converters;

public class SerializedStoneIdToVisibilityConverter : IMultiValueConverter
{
	public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
	{
		object obj = values[1];
		Diamond val = (Diamond)((obj is Diamond) ? obj : null);
		if (val != null)
		{
			int stoneSerialNumber = val.SerialNumber;
			return (ViewModelLocator.Current.StoneTrayVM.StoneResults == null || !ViewModelLocator.Current.StoneTrayVM.StoneResults.Any((Diamond stone) => stone.SerialNumber == stoneSerialNumber)) ? Visibility.Collapsed : Visibility.Visible;
		}
		return Visibility.Collapsed;
	}

	public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
	{
		throw new NotImplementedException();
	}
}
