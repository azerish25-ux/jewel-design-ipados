using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Windows;
using System.Windows.Markup;
using System.Windows.Media;
using System.Xml;
using System.Xml.Linq;

namespace GV.CounterSketch.Common.Utilities.Helpers;

public static class ThemeHelper
{
	public static readonly ColorTheme DebugColorTheme = new ColorTheme
	{
		Name = "Debug",
		WindowFrameBackground = (Color)ColorConverter.ConvertFromString("Red"),
		BaseBackground = (Color)ColorConverter.ConvertFromString("Blue"),
		ToolHeaderBackground = (Color)ColorConverter.ConvertFromString("Green"),
		TextBoxAndFrameBackground = (Color)ColorConverter.ConvertFromString("Orange"),
		HighlightAndAccent = (Color)ColorConverter.ConvertFromString("Pink")
	};

	public static readonly string ImageThemeRootPath = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), "ImageThemes\\");

	private static readonly List<string> ThemeDicitonaries = new List<string> { "/ResourceDictionaries/Themes/ThemeCounterSketch.xaml" };

	public static ColorTheme DefaultColorTheme { get; set; }

	public static string CustomThemePath { get; set; }

	public static ColorTheme CurrentImageColorTheme { get; set; }

	public static ColorTheme CurrentColorTheme { get; set; }

	public static Color ColorHueAdjustment(Color baseColor, double saturationValue, double brightnessValue, bool isContrast = false)
	{
		HsbColor hsbColor = HsbColor.FromColor(baseColor);
		if (isContrast)
		{
			hsbColor.Hue = hsbColor.HueInverse;
		}
		hsbColor.Saturation = saturationValue;
		hsbColor.Brightness = brightnessValue;
		return hsbColor.ToColor();
	}

	public static ColorTheme CreateCustomThemeFromColor(Color selectedColor)
	{
		return new ColorTheme
		{
			Name = "Custom Theme",
			WindowFrameBackground = ColorHueAdjustment(selectedColor, 0.3, 0.8),
			BaseBackground = ColorHueAdjustment(selectedColor, 0.25, 0.35),
			ToolHeaderBackground = ColorHueAdjustment(selectedColor, 0.45, 0.3),
			TextBoxAndFrameBackground = ColorHueAdjustment(selectedColor, 0.45, 0.25),
			HighlightAndAccent = ColorHueAdjustment(selectedColor, 1.0, 1.0, isContrast: true)
		};
	}

	public static double GetBrightnessFromColor(Color color)
	{
		return HsbColor.FromColor(color).Brightness;
	}

	public static bool GetIsColorGrayscale(Color color)
	{
		if (HsbColor.FromColor(color).Saturation != 0.0)
		{
			return false;
		}
		return true;
	}

	public static double GetSaturationFromColor(Color color)
	{
		return HsbColor.FromColor(color).Saturation;
	}

	public static void Initialize(KeyProfile keyProfile, string themePath)
	{
		CustomThemePath = themePath;
		DefaultColorTheme = new ColorTheme
		{
			Name = keyProfile.ApplicationName,
			WindowFrameBackground = keyProfile.WindowFrameBackground,
			TextBoxAndFrameBackground = keyProfile.TextBoxAndFrameBackground,
			ToolHeaderBackground = keyProfile.ToolHeaderBackground,
			BaseBackground = keyProfile.BaseBackground,
			HighlightAndAccent = keyProfile.HighlightAndAccent,
			HighlightForeground = keyProfile.HighlightForeground
		};
	}

	public static void RemoveThemeResourceDictionaries(string resourceDistionaryToIgnore = null)
	{
		Collection<ResourceDictionary> mergedDictionaries = Application.Current.Resources.MergedDictionaries;
		ResourceDictionary resourceDictionary = mergedDictionaries.Where((ResourceDictionary resource) => resource.Source != null && resource.Source.ToString().Contains("CustomTheme")).FirstOrDefault();
		if (resourceDictionary != null && resourceDictionary.Source.OriginalString != resourceDistionaryToIgnore)
		{
			mergedDictionaries.Remove(resourceDictionary);
		}
	}

	public static void SaveAndApplyUpdatedCustomTheme(ColorTheme customColorTheme)
	{
		if (customColorTheme == null)
		{
			customColorTheme = DefaultColorTheme;
		}
		CurrentColorTheme = customColorTheme;
		XDocument xDocument = new XDocument();
		XNamespace xNamespace = "http://schemas.microsoft.com/winfx/2006/xaml/presentation";
		XNamespace xNamespace2 = "http://schemas.microsoft.com/winfx/2006/xaml";
		XElement xElement = new XElement(xNamespace + "ResourceDictionary", new XAttribute(XNamespace.Xmlns + "x", "http://schemas.microsoft.com/winfx/2006/xaml"), new XAttribute(XNamespace.Xmlns + "PresentationOptions", "http://schemas.microsoft.com/winfx/2006/xaml/presentation/options"));
		XElement xElement2 = XElement.Parse(XamlWriter.Save(new SolidColorBrush(customColorTheme.WindowFrameBackground)));
		XElement xElement3 = XElement.Parse(XamlWriter.Save(new SolidColorBrush(customColorTheme.BaseBackground)));
		XElement xElement4 = XElement.Parse(XamlWriter.Save(new SolidColorBrush(customColorTheme.ToolHeaderBackground)));
		XElement xElement5 = XElement.Parse(XamlWriter.Save(new SolidColorBrush(customColorTheme.TextBoxAndFrameBackground)));
		XElement xElement6 = XElement.Parse(XamlWriter.Save(new SolidColorBrush(customColorTheme.HighlightAndAccent)));
		XElement xElement7 = XElement.Parse(XamlWriter.Save(customColorTheme.WindowFrameBackground));
		XElement xElement8 = XElement.Parse(XamlWriter.Save(customColorTheme.BaseBackground));
		XElement xElement9 = XElement.Parse(XamlWriter.Save(customColorTheme.ToolHeaderBackground));
		XElement xElement10 = XElement.Parse(XamlWriter.Save(customColorTheme.TextBoxAndFrameBackground));
		XElement xElement11 = XElement.Parse(XamlWriter.Save(customColorTheme.HighlightAndAccent));
		XElement xElement12 = XElement.Parse(XamlWriter.Save(customColorTheme.HighlightForeground));
		XElement xElement13 = XElement.Parse(XamlWriter.Save(customColorTheme.BackgroundGradient));
		xElement.Add(xElement2, xElement7, xElement3, xElement8, xElement4, xElement9, xElement5, xElement10, xElement6, xElement11, xElement13, xElement12);
		xElement2.SetAttributeValue(xNamespace2 + "Key", "WindowFrameBackground");
		xElement7.SetAttributeValue(xNamespace2 + "Key", "WindowFrameBackgroundColor");
		xElement3.SetAttributeValue(xNamespace2 + "Key", "BaseBackground");
		xElement8.SetAttributeValue(xNamespace2 + "Key", "BaseBackgroundColor");
		xElement4.SetAttributeValue(xNamespace2 + "Key", "ToolHeaderBackground");
		xElement9.SetAttributeValue(xNamespace2 + "Key", "ToolHeaderBackgroundColor");
		xElement5.SetAttributeValue(xNamespace2 + "Key", "TextBoxAndFrameBackground");
		xElement10.SetAttributeValue(xNamespace2 + "Key", "TextBoxAndFrameBackgroundColor");
		xElement6.SetAttributeValue(xNamespace2 + "Key", "HighlightAndAccent");
		xElement11.SetAttributeValue(xNamespace2 + "Key", "HighlightAndAccentColor");
		xElement13.SetAttributeValue(xNamespace2 + "Key", "BackgroundGradient");
		xElement12.SetAttributeValue(xNamespace2 + "Key", "HighlightForeground");
		xDocument.Add(xElement);
		xDocument.Save(CustomThemePath);
		ApplyCustomTheme();
	}

	public static void SaveAndApplyUpdatedCustomTheme(Color windowFrameBackground, Color baseBackground, Color toolHeaderBackground, Color textBoxAndFrameBackground, Color highlightAndAccent)
	{
		SaveAndApplyUpdatedCustomTheme(new ColorTheme
		{
			WindowFrameBackground = windowFrameBackground,
			BaseBackground = baseBackground,
			ToolHeaderBackground = toolHeaderBackground,
			TextBoxAndFrameBackground = textBoxAndFrameBackground,
			HighlightAndAccent = highlightAndAccent
		});
	}

	public static void SetCounterSketchDefaultTheme()
	{
		if (!File.Exists(CustomThemePath))
		{
			SaveAndApplyUpdatedCustomTheme(DefaultColorTheme);
		}
		else if (!UpdateOldCounterSketchTheme())
		{
			ApplyCustomTheme();
		}
	}

	public static void SwapWindowFrameColorToInventoryYellow(bool revertToTheme = false)
	{
		Application.Current.Resources["WindowFrameBackground"] = (revertToTheme ? new SolidColorBrush(CurrentColorTheme.WindowFrameBackground) : new SolidColorBrush(Colors.Yellow));
	}

	public static bool UpdateOldCounterSketchTheme()
	{
		string text = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData), "Gemvision\\CounterSketch Studio\\CounterSketchApplicationSettings.xml");
		if (File.Exists(text))
		{
			XDocument xDocument = XDocument.Load(text);
			if (xDocument.Root != null)
			{
				XElement xElement = xDocument.Root.Elements("ThemingColorTheme").FirstOrDefault();
				if (xElement != null)
				{
					XElement xElement2 = xElement.Element("Name");
					if (xElement2 != null && xElement2.Value.Trim() == "CounterSketch")
					{
						SaveAndApplyUpdatedCustomTheme(null);
						return true;
					}
				}
			}
		}
		return false;
	}

	private static void ApplyCustomTheme()
	{
		if (!File.Exists(CustomThemePath))
		{
			return;
		}
		using XmlReader reader = XmlReader.Create(CustomThemePath);
		ResourceDictionary resourceDictionary = (ResourceDictionary)XamlReader.Load(reader);
		resourceDictionary.Source = new Uri(CustomThemePath);
		if (Application.Current != null)
		{
			Collection<ResourceDictionary> mergedDictionaries = Application.Current.Resources.MergedDictionaries;
			RemoveThemeResourceDictionaries();
			mergedDictionaries.Add(resourceDictionary);
		}
	}
}
