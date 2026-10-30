using System.Collections;
using System.Collections.Generic;
using System.Xml;

namespace GV.CounterSketch.Studio.Controls.MarkupConverter;

internal static class HtmlCssParser
{
	private static readonly string[] _borderStyles = new string[9] { "none", "dotted", "dashed", "solid", "double", "groove", "ridge", "inset", "outset" };

	private static readonly string[] _clears = new string[4] { "none", "left", "right", "both" };

	private static readonly string[] _colors = new string[140]
	{
		"aliceblue", "antiquewhite", "aqua", "aquamarine", "azure", "beige", "bisque", "black", "blanchedalmond", "blue",
		"blueviolet", "brown", "burlywood", "cadetblue", "chartreuse", "chocolate", "coral", "cornflowerblue", "cornsilk", "crimson",
		"cyan", "darkblue", "darkcyan", "darkgoldenrod", "darkgray", "darkgreen", "darkkhaki", "darkmagenta", "darkolivegreen", "darkorange",
		"darkorchid", "darkred", "darksalmon", "darkseagreen", "darkslateblue", "darkslategray", "darkturquoise", "darkviolet", "deeppink", "deepskyblue",
		"dimgray", "dodgerblue", "firebrick", "floralwhite", "forestgreen", "fuchsia", "gainsboro", "ghostwhite", "gold", "goldenrod",
		"gray", "green", "greenyellow", "honeydew", "hotpink", "indianred", "indigo", "ivory", "khaki", "lavender",
		"lavenderblush", "lawngreen", "lemonchiffon", "lightblue", "lightcoral", "lightcyan", "lightgoldenrodyellow", "lightgreen", "lightgrey", "lightpink",
		"lightsalmon", "lightseagreen", "lightskyblue", "lightslategray", "lightsteelblue", "lightyellow", "lime", "limegreen", "linen", "magenta",
		"maroon", "mediumaquamarine", "mediumblue", "mediumorchid", "mediumpurple", "mediumseagreen", "mediumslateblue", "mediumspringgreen", "mediumturquoise", "mediumvioletred",
		"midnightblue", "mintcream", "mistyrose", "moccasin", "navajowhite", "navy", "oldlace", "olive", "olivedrab", "orange",
		"orangered", "orchid", "palegoldenrod", "palegreen", "paleturquoise", "palevioletred", "papayawhip", "peachpuff", "peru", "pink",
		"plum", "powderblue", "purple", "red", "rosybrown", "royalblue", "saddlebrown", "salmon", "sandybrown", "seagreen",
		"seashell", "sienna", "silver", "skyblue", "slateblue", "slategray", "snow", "springgreen", "steelblue", "tan",
		"teal", "thistle", "tomato", "turquoise", "violet", "wheat", "white", "whitesmoke", "yellow", "yellowgreen"
	};

	private static readonly string[] _floats = new string[3] { "left", "right", "none" };

	private static readonly string[] _fontAbsoluteSizes = new string[7] { "xx-small", "x-small", "small", "medium", "large", "x-large", "xx-large" };

	private static readonly string[] _fontGenericFamilies = new string[5] { "serif", "sans-serif", "monospace", "cursive", "fantasy" };

	private static readonly string[] _fontRelativeSizes = new string[2] { "larger", "smaller" };

	private static readonly string[] _fontSizeUnits = new string[9] { "px", "mm", "cm", "in", "pt", "pc", "em", "ex", "%" };

	private static readonly string[] _fontStyles = new string[3] { "normal", "italic", "oblique" };

	private static readonly string[] _fontVariants = new string[2] { "normal", "small-caps" };

	private static readonly string[] _fontWeights = new string[13]
	{
		"normal", "bold", "bolder", "lighter", "100", "200", "300", "400", "500", "600",
		"700", "800", "900"
	};

	private static readonly string[] _listStylePositions = new string[2] { "inside", "outside" };

	private static readonly string[] _listStyleTypes = new string[9] { "disc", "circle", "square", "decimal", "lower-roman", "upper-roman", "lower-alpha", "upper-alpha", "none" };

	private static readonly string[] _systemColors = new string[28]
	{
		"activeborder", "activecaption", "appworkspace", "background", "buttonface", "buttonhighlight", "buttonshadow", "buttontext", "captiontext", "graytext",
		"highlight", "highlighttext", "inactiveborder", "inactivecaption", "inactivecaptiontext", "infobackground", "infotext", "menu", "menutext", "scrollbar",
		"threeddarkshadow", "threedface", "threedhighlight", "threedlightshadow", "threedshadow", "window", "windowframe", "windowtext"
	};

	private static readonly string[] _textAligns = new string[4] { "left", "right", "center", "justify" };

	private static readonly string[] _textDecorations = new string[5] { "none", "underline", "overline", "line-through", "blink" };

	private static readonly string[] _textTransforms = new string[4] { "none", "capitalize", "uppercase", "lowercase" };

	private static readonly string[] _verticalAligns = new string[8] { "baseline", "sub", "super", "top", "text-top", "middle", "bottom", "text-bottom" };

	private static string[] _blocks = new string[4] { "block", "inline", "list-item", "none" };

	internal static void GetElementPropertiesFromCssAttributes(XmlElement htmlElement, string elementName, CssStylesheet stylesheet, Hashtable localProperties, List<XmlElement> sourceContext)
	{
		string style = stylesheet.GetStyle(elementName, sourceContext);
		string attribute = HtmlToXamlConverter.GetAttribute(htmlElement, "style");
		string text = ((style != null) ? style : null);
		if (attribute != null)
		{
			text = ((text == null) ? attribute : (text + ";" + attribute));
		}
		if (text == null)
		{
			return;
		}
		string[] array = text.Split(';');
		for (int i = 0; i < array.Length; i++)
		{
			string[] array2 = array[i].Split(':');
			if (array2.Length == 2)
			{
				string text2 = array2[0].Trim().ToLower();
				string styleValue = HtmlToXamlConverter.UnQuote(array2[1].Trim()).ToLower();
				int nextIndex = 0;
				switch (text2)
				{
				case "font":
					ParseCssFont(styleValue, localProperties);
					break;
				case "font-family":
					ParseCssFontFamily(styleValue, ref nextIndex, localProperties);
					break;
				case "font-size":
					ParseCssSize(styleValue, ref nextIndex, localProperties, "font-size", mustBeNonNegative: true);
					break;
				case "font-style":
					ParseCssFontStyle(styleValue, ref nextIndex, localProperties);
					break;
				case "font-weight":
					ParseCssFontWeight(styleValue, ref nextIndex, localProperties);
					break;
				case "font-variant":
					ParseCssFontVariant(styleValue, ref nextIndex, localProperties);
					break;
				case "line-height":
					ParseCssSize(styleValue, ref nextIndex, localProperties, "line-height", mustBeNonNegative: true);
					break;
				case "color":
					ParseCssColor(styleValue, ref nextIndex, localProperties, "color");
					break;
				case "text-decoration":
					ParseCssTextDecoration(styleValue, ref nextIndex, localProperties);
					break;
				case "text-transform":
					ParseCssTextTransform(styleValue, ref nextIndex, localProperties);
					break;
				case "background-color":
					ParseCssColor(styleValue, ref nextIndex, localProperties, "background-color");
					break;
				case "background":
					ParseCssBackground(styleValue, ref nextIndex, localProperties);
					break;
				case "text-align":
					ParseCssTextAlign(styleValue, ref nextIndex, localProperties);
					break;
				case "vertical-align":
					ParseCssVerticalAlign(styleValue, ref nextIndex, localProperties);
					break;
				case "text-indent":
					ParseCssSize(styleValue, ref nextIndex, localProperties, "text-indent", mustBeNonNegative: false);
					break;
				case "width":
				case "height":
					ParseCssSize(styleValue, ref nextIndex, localProperties, text2, mustBeNonNegative: true);
					break;
				case "margin":
					ParseCssRectangleProperty(styleValue, ref nextIndex, localProperties, text2);
					break;
				case "margin-top":
				case "margin-right":
				case "margin-bottom":
				case "margin-left":
					ParseCssSize(styleValue, ref nextIndex, localProperties, text2, mustBeNonNegative: true);
					break;
				case "padding":
					ParseCssRectangleProperty(styleValue, ref nextIndex, localProperties, text2);
					break;
				case "padding-top":
				case "padding-right":
				case "padding-bottom":
				case "padding-left":
					ParseCssSize(styleValue, ref nextIndex, localProperties, text2, mustBeNonNegative: true);
					break;
				case "border":
					ParseCssBorder(styleValue, ref nextIndex, localProperties);
					break;
				case "border-style":
				case "border-width":
				case "border-color":
					ParseCssRectangleProperty(styleValue, ref nextIndex, localProperties, text2);
					break;
				case "float":
					ParseCssFloat(styleValue, ref nextIndex, localProperties);
					break;
				case "clear":
					ParseCssClear(styleValue, ref nextIndex, localProperties);
					break;
				}
			}
		}
	}

	private static void ParseCssBackground(string styleValue, ref int nextIndex, Hashtable localValues)
	{
	}

	private static void ParseCssBorder(string styleValue, ref int nextIndex, Hashtable localProperties)
	{
		while (ParseCssRectangleProperty(styleValue, ref nextIndex, localProperties, "border-width") || ParseCssRectangleProperty(styleValue, ref nextIndex, localProperties, "border-style") || ParseCssRectangleProperty(styleValue, ref nextIndex, localProperties, "border-color"))
		{
		}
	}

	private static string ParseCssBorderStyle(string styleValue, ref int nextIndex)
	{
		return ParseWordEnumeration(_borderStyles, styleValue, ref nextIndex);
	}

	private static void ParseCssClear(string styleValue, ref int nextIndex, Hashtable localProperties)
	{
		ParseWordEnumeration(_clears, styleValue, ref nextIndex, localProperties, "clear");
	}

	private static string ParseCssColor(string styleValue, ref int nextIndex)
	{
		ParseWhiteSpace(styleValue, ref nextIndex);
		string text = null;
		if (nextIndex < styleValue.Length)
		{
			int num = nextIndex;
			char c = styleValue[nextIndex];
			if (c == '#')
			{
				nextIndex++;
				while (nextIndex < styleValue.Length)
				{
					c = char.ToUpper(styleValue[nextIndex]);
					if (('0' > c || c > '9') && ('A' > c || c > 'F'))
					{
						break;
					}
					nextIndex++;
				}
				if (nextIndex > num + 1)
				{
					text = styleValue.Substring(num, nextIndex - num);
				}
			}
			else if (styleValue.Substring(nextIndex, 3).ToLower() == "rbg")
			{
				while (nextIndex < styleValue.Length && styleValue[nextIndex] != ')')
				{
					nextIndex++;
				}
				if (nextIndex < styleValue.Length)
				{
					nextIndex++;
				}
				text = "gray";
			}
			else if (char.IsLetter(c))
			{
				text = ParseWordEnumeration(_colors, styleValue, ref nextIndex);
				if (text == null)
				{
					text = ParseWordEnumeration(_systemColors, styleValue, ref nextIndex);
					if (text != null)
					{
						text = "black";
					}
				}
			}
		}
		return text;
	}

	private static void ParseCssColor(string styleValue, ref int nextIndex, Hashtable localValues, string propertyName)
	{
		string text = ParseCssColor(styleValue, ref nextIndex);
		if (text != null)
		{
			localValues[propertyName] = text;
		}
	}

	private static void ParseCssFloat(string styleValue, ref int nextIndex, Hashtable localProperties)
	{
		ParseWordEnumeration(_floats, styleValue, ref nextIndex, localProperties, "float");
	}

	private static void ParseCssFont(string styleValue, Hashtable localProperties)
	{
		int nextIndex = 0;
		ParseCssFontStyle(styleValue, ref nextIndex, localProperties);
		ParseCssFontVariant(styleValue, ref nextIndex, localProperties);
		ParseCssFontWeight(styleValue, ref nextIndex, localProperties);
		ParseCssSize(styleValue, ref nextIndex, localProperties, "font-size", mustBeNonNegative: true);
		ParseWhiteSpace(styleValue, ref nextIndex);
		if (nextIndex < styleValue.Length && styleValue[nextIndex] == '/')
		{
			nextIndex++;
			ParseCssSize(styleValue, ref nextIndex, localProperties, "line-height", mustBeNonNegative: true);
		}
		ParseCssFontFamily(styleValue, ref nextIndex, localProperties);
	}

	private static void ParseCssFontFamily(string styleValue, ref int nextIndex, Hashtable localProperties)
	{
		string text = null;
		while (nextIndex < styleValue.Length)
		{
			string text2 = ParseWordEnumeration(_fontGenericFamilies, styleValue, ref nextIndex);
			if (text2 == null)
			{
				if (nextIndex < styleValue.Length && (styleValue[nextIndex] == '"' || styleValue[nextIndex] == '\''))
				{
					char c = styleValue[nextIndex];
					nextIndex++;
					int num = nextIndex;
					while (nextIndex < styleValue.Length && styleValue[nextIndex] != c)
					{
						nextIndex++;
					}
					text2 = "\"" + styleValue.Substring(num, nextIndex - num) + "\"";
				}
				if (text2 == null)
				{
					int num2 = nextIndex;
					while (nextIndex < styleValue.Length && styleValue[nextIndex] != ',' && styleValue[nextIndex] != ';')
					{
						nextIndex++;
					}
					if (nextIndex > num2)
					{
						text2 = styleValue.Substring(num2, nextIndex - num2).Trim();
						if (text2.Length == 0)
						{
							text2 = null;
						}
					}
				}
			}
			ParseWhiteSpace(styleValue, ref nextIndex);
			if (nextIndex < styleValue.Length && styleValue[nextIndex] == ',')
			{
				nextIndex++;
			}
			if (text2 == null)
			{
				break;
			}
			if (text == null && text2.Length > 0)
			{
				if (text2[0] == '"' || text2[0] == '\'')
				{
					text2 = text2.Substring(1, text2.Length - 2);
				}
				text = text2;
			}
		}
		if (text != null)
		{
			localProperties["font-family"] = text;
		}
	}

	private static void ParseCssFontStyle(string styleValue, ref int nextIndex, Hashtable localProperties)
	{
		ParseWordEnumeration(_fontStyles, styleValue, ref nextIndex, localProperties, "font-style");
	}

	private static void ParseCssFontVariant(string styleValue, ref int nextIndex, Hashtable localProperties)
	{
		ParseWordEnumeration(_fontVariants, styleValue, ref nextIndex, localProperties, "font-variant");
	}

	private static void ParseCssFontWeight(string styleValue, ref int nextIndex, Hashtable localProperties)
	{
		ParseWordEnumeration(_fontWeights, styleValue, ref nextIndex, localProperties, "font-weight");
	}

	private static void ParseCssListStyle(string styleValue, Hashtable localProperties)
	{
		int nextIndex = 0;
		while (nextIndex < styleValue.Length)
		{
			string text = ParseCssListStyleType(styleValue, ref nextIndex);
			if (text != null)
			{
				localProperties["list-style-type"] = text;
				continue;
			}
			string text2 = ParseCssListStylePosition(styleValue, ref nextIndex);
			if (text2 != null)
			{
				localProperties["list-style-position"] = text2;
				continue;
			}
			string text3 = ParseCssListStyleImage(styleValue, ref nextIndex);
			if (text3 != null)
			{
				localProperties["list-style-image"] = text3;
				continue;
			}
			break;
		}
	}

	private static string ParseCssListStyleImage(string styleValue, ref int nextIndex)
	{
		return null;
	}

	private static string ParseCssListStylePosition(string styleValue, ref int nextIndex)
	{
		return ParseWordEnumeration(_listStylePositions, styleValue, ref nextIndex);
	}

	private static string ParseCssListStyleType(string styleValue, ref int nextIndex)
	{
		return ParseWordEnumeration(_listStyleTypes, styleValue, ref nextIndex);
	}

	private static bool ParseCssRectangleProperty(string styleValue, ref int nextIndex, Hashtable localProperties, string propertyName)
	{
		string text = ((propertyName == "border-color") ? ParseCssColor(styleValue, ref nextIndex) : ((propertyName == "border-style") ? ParseCssBorderStyle(styleValue, ref nextIndex) : ParseCssSize(styleValue, ref nextIndex, mustBeNonNegative: true)));
		if (text != null)
		{
			localProperties[propertyName + "-top"] = text;
			localProperties[propertyName + "-bottom"] = text;
			localProperties[propertyName + "-right"] = text;
			localProperties[propertyName + "-left"] = text;
			text = ((propertyName == "border-color") ? ParseCssColor(styleValue, ref nextIndex) : ((propertyName == "border-style") ? ParseCssBorderStyle(styleValue, ref nextIndex) : ParseCssSize(styleValue, ref nextIndex, mustBeNonNegative: true)));
			if (text != null)
			{
				localProperties[propertyName + "-right"] = text;
				localProperties[propertyName + "-left"] = text;
				text = ((propertyName == "border-color") ? ParseCssColor(styleValue, ref nextIndex) : ((propertyName == "border-style") ? ParseCssBorderStyle(styleValue, ref nextIndex) : ParseCssSize(styleValue, ref nextIndex, mustBeNonNegative: true)));
				if (text != null)
				{
					localProperties[propertyName + "-bottom"] = text;
					text = ((propertyName == "border-color") ? ParseCssColor(styleValue, ref nextIndex) : ((propertyName == "border-style") ? ParseCssBorderStyle(styleValue, ref nextIndex) : ParseCssSize(styleValue, ref nextIndex, mustBeNonNegative: true)));
					if (text != null)
					{
						localProperties[propertyName + "-left"] = text;
					}
				}
			}
			return true;
		}
		return false;
	}

	private static string ParseCssSize(string styleValue, ref int nextIndex, bool mustBeNonNegative)
	{
		ParseWhiteSpace(styleValue, ref nextIndex);
		int num = nextIndex;
		if (nextIndex < styleValue.Length && styleValue[nextIndex] == '-')
		{
			nextIndex++;
		}
		if (nextIndex < styleValue.Length && char.IsDigit(styleValue[nextIndex]))
		{
			while (nextIndex < styleValue.Length && (char.IsDigit(styleValue[nextIndex]) || styleValue[nextIndex] == '.'))
			{
				nextIndex++;
			}
			string text = styleValue.Substring(num, nextIndex - num);
			string text2 = ParseWordEnumeration(_fontSizeUnits, styleValue, ref nextIndex);
			if (text2 == null)
			{
				text2 = "px";
			}
			if (mustBeNonNegative && styleValue[num] == '-')
			{
				return "0";
			}
			return text + text2;
		}
		return null;
	}

	private static void ParseCssSize(string styleValue, ref int nextIndex, Hashtable localValues, string propertyName, bool mustBeNonNegative)
	{
		string text = ParseCssSize(styleValue, ref nextIndex, mustBeNonNegative);
		if (text != null)
		{
			localValues[propertyName] = text;
		}
	}

	private static void ParseCssTextAlign(string styleValue, ref int nextIndex, Hashtable localProperties)
	{
		ParseWordEnumeration(_textAligns, styleValue, ref nextIndex, localProperties, "text-align");
	}

	private static void ParseCssTextDecoration(string styleValue, ref int nextIndex, Hashtable localProperties)
	{
		for (int i = 1; i < _textDecorations.Length; i++)
		{
			localProperties["text-decoration-" + _textDecorations[i]] = "false";
		}
		while (nextIndex < styleValue.Length)
		{
			string text = ParseWordEnumeration(_textDecorations, styleValue, ref nextIndex);
			if (text != null && !(text == "none"))
			{
				localProperties["text-decoration-" + text] = "true";
				continue;
			}
			break;
		}
	}

	private static void ParseCssTextTransform(string styleValue, ref int nextIndex, Hashtable localProperties)
	{
		ParseWordEnumeration(_textTransforms, styleValue, ref nextIndex, localProperties, "text-transform");
	}

	private static void ParseCssVerticalAlign(string styleValue, ref int nextIndex, Hashtable localProperties)
	{
		ParseWordEnumeration(_verticalAligns, styleValue, ref nextIndex, localProperties, "vertical-align");
	}

	private static void ParseWhiteSpace(string styleValue, ref int nextIndex)
	{
		while (nextIndex < styleValue.Length && char.IsWhiteSpace(styleValue[nextIndex]))
		{
			nextIndex++;
		}
	}

	private static bool ParseWord(string word, string styleValue, ref int nextIndex)
	{
		ParseWhiteSpace(styleValue, ref nextIndex);
		for (int i = 0; i < word.Length; i++)
		{
			if (nextIndex + i >= styleValue.Length || word[i] != styleValue[nextIndex + i])
			{
				return false;
			}
		}
		if (nextIndex + word.Length < styleValue.Length && char.IsLetterOrDigit(styleValue[nextIndex + word.Length]))
		{
			return false;
		}
		nextIndex += word.Length;
		return true;
	}

	private static string ParseWordEnumeration(string[] words, string styleValue, ref int nextIndex)
	{
		for (int i = 0; i < words.Length; i++)
		{
			if (ParseWord(words[i], styleValue, ref nextIndex))
			{
				return words[i];
			}
		}
		return null;
	}

	private static void ParseWordEnumeration(string[] words, string styleValue, ref int nextIndex, Hashtable localProperties, string attributeName)
	{
		string text = ParseWordEnumeration(words, styleValue, ref nextIndex);
		if (text != null)
		{
			localProperties[attributeName] = text;
		}
	}
}
