using System;
using System.Globalization;
using System.Windows.Data;
using GV.CounterSketch.Studio.ViewModel;

namespace GV.CounterSketch.Studio.Utilities.Converters;

internal class SectionNameToDisabledBooleanConverter : IValueConverter
{
	public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
	{
		if (value is string sectionName)
		{
			return ViewModelLocator.Current.PreferenceOrderScreenVM.IsSectionDisabled(sectionName);
		}
		return false;
	}

	public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
	{
		if (value is int num && num != 0)
		{
			ViewModelLocator.Current.PreferenceOrderScreenVM.DisableSection(parameter as string);
		}
		else
		{
			ViewModelLocator.Current.PreferenceOrderScreenVM.EnableSection(parameter as string);
		}
		return parameter;
	}
}
