using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;
using GV.CounterSketch.Studio.ViewModel;
using Stuller.Web.API.v2.Models;

namespace GV.CounterSketch.Studio.Utilities.Converters;

public class StoneTrayAssignedToVisibilityConverter : IMultiValueConverter
{
	public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
	{
		bool flag = false;
		if (values[1] is Diamond { SerialNumber: var serialNumber } diamond)
		{
			flag = (ViewModelLocator.Current.StoneTrayVM.StoneChosen != null && ViewModelLocator.Current.StoneTrayVM.StoneChosen.SerialNumber == serialNumber) || ViewModelLocator.Current.GemReportVM.IsDiamondAssignedToNonCenterStone(diamond.SerialNumber);
			if (parameter != null)
			{
				bool.TryParse(parameter.ToString(), out var result);
				if (result)
				{
					flag = !flag;
				}
			}
		}
		return (!flag) ? Visibility.Collapsed : Visibility.Visible;
	}

	public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
	{
		throw new NotImplementedException();
	}
}
