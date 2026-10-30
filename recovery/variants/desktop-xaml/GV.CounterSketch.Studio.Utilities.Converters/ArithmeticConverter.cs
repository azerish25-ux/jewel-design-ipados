using System;
using System.Globalization;
using System.Text.RegularExpressions;
using System.Windows.Data;
using System.Windows.Markup;

namespace GV.CounterSketch.Studio.Utilities.Converters;

public class ArithmeticConverter : MarkupExtension, IValueConverter
{
	private const string ArithmeticParseExpression = "([+\\-*/]{1,1})\\s{0,}(\\-?[\\d\\.]+)";

	private static ArithmeticConverter _converter;

	private readonly Regex _arithmeticRegex = new Regex("([+\\-*/]{1,1})\\s{0,}(\\-?[\\d\\.]+)");

	public override object ProvideValue(IServiceProvider serviceProvider)
	{
		if (_converter == null)
		{
			_converter = new ArithmeticConverter();
		}
		return _converter;
	}

	public object Convert(object input, Type targetType, object parameter, CultureInfo culture)
	{
		if (!double.TryParse(input?.ToString(), out var result))
		{
			return null;
		}
		if (parameter != null)
		{
			string text = parameter.ToString();
			if (text.Length > 0)
			{
				Match match = _arithmeticRegex.Match(text);
				if (match != null && match.Groups.Count == 3)
				{
					string text2 = match.Groups[1].Value.Trim();
					if (double.TryParse(match.Groups[2].Value, NumberStyles.Any, CultureInfo.InvariantCulture, out var result2))
					{
						double num = result;
						double num2 = 0.0;
						switch (text2)
						{
						case "+":
							num2 = num + result2;
							break;
						case "-":
							num2 = num - result2;
							break;
						case "*":
							num2 = num * result2;
							break;
						case "/":
							num2 = num / result2;
							break;
						}
						return num2;
					}
				}
			}
		}
		return null;
	}

	public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
	{
		throw new Exception("The method or operation is not implemented.");
	}
}
