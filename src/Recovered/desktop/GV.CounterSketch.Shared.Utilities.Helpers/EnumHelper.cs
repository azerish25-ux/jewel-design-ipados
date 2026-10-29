using System;
using System.ComponentModel;

namespace GV.CounterSketch.Shared.Utilities.Helpers;

public static class EnumHelper
{
	public static string GetEnumDescriptionFromValue(Enum enumValue)
	{
		string result = string.Empty;
		if (enumValue.GetType().GetField(enumValue.ToString()).GetCustomAttributes(typeof(DescriptionAttribute), inherit: false) is DescriptionAttribute[] array)
		{
			result = ((array.Length != 0) ? array[0].Description : string.Empty);
		}
		return result;
	}

	public static string GetEnumDescriptionFromValueDefault(Enum whichFont, string descriptionDefault = "Unknown")
	{
		string enumDescriptionFromValue = GetEnumDescriptionFromValue(whichFont);
		if (!string.IsNullOrWhiteSpace(enumDescriptionFromValue))
		{
			return enumDescriptionFromValue;
		}
		return $"{descriptionDefault}({whichFont})";
	}
}
