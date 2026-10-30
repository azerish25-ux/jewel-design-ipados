using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Media;

namespace GV.CounterSketch.Studio.Utilities.Converters;

public class EngravingColorToForegroundConverter : IValueConverter
{
	public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
	{
		if (value != null)
		{
			string text = value.ToString();
			switch (text.ToLower())
			{
			case "red":
				return new SolidColorBrush(Colors.Red);
			case "black":
				return new SolidColorBrush(Colors.Black);
			case "brown":
				return new SolidColorBrush(Colors.SaddleBrown);
			case "blue":
				return new SolidColorBrush(Colors.Blue);
			case "green":
				return new SolidColorBrush(Colors.Green);
			case "pink":
				return new SolidColorBrush(Colors.HotPink);
			case "none":
				return new SolidColorBrush(Colors.White);
			default:
				if (text.ToLower().Contains("green"))
				{
					return new SolidColorBrush(Color.FromArgb(byte.MaxValue, 187, 193, 123));
				}
				if (text.ToLower().Contains("rose"))
				{
					return new SolidColorBrush(Color.FromArgb(byte.MaxValue, 242, 198, 161));
				}
				if (text.ToLower().Contains("yellow"))
				{
					return new SolidColorBrush(Color.FromArgb(byte.MaxValue, 254, 228, 132));
				}
				return new SolidColorBrush(Color.FromArgb(byte.MaxValue, 223, 219, 216));
			}
		}
		return 14;
	}

	public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
	{
		throw new NotImplementedException();
	}
}
