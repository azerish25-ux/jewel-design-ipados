using System;
using System.Collections.Generic;
using System.ComponentModel;

namespace GV.CounterSketch.Studio.Utilities.Helpers;

public static class EnumHelper
{
	public static List<string> ConvertEnumListToStrings<T>(object enumValues)
	{
		List<string> list = new List<string>();
		if (enumValues is T[] array)
		{
			T[] array2 = array;
			for (int i = 0; i < array2.Length; i++)
			{
				if (array2[i] is Enum enumValue)
				{
					string enumDescriptionFromValue = GetEnumDescriptionFromValue(enumValue);
					list.Add(enumDescriptionFromValue);
				}
			}
		}
		return list;
	}

	public static string GetEnumDescriptionFromValue(Enum enumValue)
	{
		string result = string.Empty;
		if (enumValue.GetType().GetField(enumValue.ToString()).GetCustomAttributes(typeof(DescriptionAttribute), inherit: false) is DescriptionAttribute[] array)
		{
			result = ((array.Length != 0) ? array[0].Description : string.Empty);
		}
		return result;
	}

	public static List<string> GetEnumListOrderedByName<T>()
	{
		string[] names = Enum.GetNames(typeof(T));
		Array.Sort(names);
		return new List<string>(names);
	}

	public static string GetEnumNameFromDescription(string description, Type enumType)
	{
		string[] names = Enum.GetNames(enumType);
		foreach (string text in names)
		{
			if (GetEnumDescriptionFromValue(Enum.Parse(enumType, text) as Enum).Equals(description))
			{
				return text;
			}
		}
		return null;
	}

	public static object GetEnumValueFromDescription(string description, Type enumType)
	{
		string[] names = Enum.GetNames(enumType);
		foreach (string value in names)
		{
			if (GetEnumDescriptionFromValue(Enum.Parse(enumType, value) as Enum).Equals(description))
			{
				return Enum.Parse(enumType, value);
			}
		}
		throw new Exception("No value found for this description");
	}
}
