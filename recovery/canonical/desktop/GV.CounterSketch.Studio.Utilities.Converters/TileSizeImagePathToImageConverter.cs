using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Resources;
using System.Windows;
using System.Windows.Data;
using System.Windows.Media.Imaging;
using DevExpress.Xpf.LayoutControl;
using GV.CounterSketch.Studio.Utilities.Helpers;

namespace GV.CounterSketch.Studio.Utilities.Converters;

public class TileSizeImagePathToImageConverter : IMultiValueConverter
{
	private static string[] _resourceNames;

	public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
	{
		if (values.Length != 4)
		{
			return Binding.DoNothing;
		}
		if (values[0] == DependencyProperty.UnsetValue || values[1] == DependencyProperty.UnsetValue || values[2] == DependencyProperty.UnsetValue || values[3] == DependencyProperty.UnsetValue)
		{
			return Binding.DoNothing;
		}
		TileSize tileSize = (TileSize)values[0];
		string text = (string)values[1];
		bool flag = (bool)values[2];
		bool flag2 = false;
		if (text == null)
		{
			return Binding.DoNothing;
		}
		if (text != UIHelper.CSFeaturedDesign)
		{
			if (tileSize == TileSize.Large)
			{
				text = text.Replace("1240x1240", "1240x600");
			}
			string text2 = "Metal";
			if (flag2)
			{
				text2 = "Composite";
			}
			string text3 = "Gray";
			if (flag)
			{
				text3 = "White";
			}
			string[] array = text.Split('/');
			string fileNameWithoutExtension = Path.GetFileNameWithoutExtension(array[array.Length - 1]);
			string text4 = text.Substring(0, text.LastIndexOf('/') + 1);
			string text5 = fileNameWithoutExtension + "_" + text2 + "_" + text3 + ".png";
			if (text5.StartsWith("FH-"))
			{
				text5 = fileNameWithoutExtension + ".png";
			}
			text = text4 + text5;
			try
			{
				BitmapImage bitmapImage = new BitmapImage();
				bitmapImage.BeginInit();
				bitmapImage.CacheOption = BitmapCacheOption.OnLoad;
				bitmapImage.UriSource = new Uri(text);
				bitmapImage.EndInit();
				if (bitmapImage.CanFreeze)
				{
					bitmapImage.Freeze();
				}
				return bitmapImage;
			}
			catch (Exception)
			{
				GlobalErrorHandler.WriteErrorToEventLog("Possible Missing Tile Image: " + text);
				return Binding.DoNothing;
			}
		}
		BitmapImage bitmapImage2 = new BitmapImage();
		try
		{
			using (WrappingStream streamSource = new WrappingStream(new MemoryStream(File.ReadAllBytes(text), writable: false)))
			{
				bitmapImage2.BeginInit();
				bitmapImage2.CacheOption = BitmapCacheOption.OnLoad;
				bitmapImage2.StreamSource = streamSource;
				bitmapImage2.EndInit();
				bitmapImage2.Freeze();
			}
			return bitmapImage2;
		}
		catch (Exception)
		{
			GlobalErrorHandler.WriteErrorToEventLog("Possible Missing Image: " + text);
			return Binding.DoNothing;
		}
	}

	public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
	{
		throw new NotImplementedException();
	}

	private static bool ResourceExists(string resourceName)
	{
		if (_resourceNames == null)
		{
			string[] manifestResourceNames = Assembly.GetExecutingAssembly().GetManifestResourceNames();
			ResourceSet resourceSet = new ResourceSet(Assembly.GetExecutingAssembly().GetManifestResourceStream(manifestResourceNames[0]));
			List<string> list = new List<string>();
			foreach (DictionaryEntry item in resourceSet)
			{
				list.Add((string)item.Key);
			}
			_resourceNames = list.ToArray();
		}
		return _resourceNames.Any((string _) => _.Contains(resourceName.ToLower()));
	}
}
