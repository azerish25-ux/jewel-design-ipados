using System;
using System.Collections.Generic;
using System.Globalization;

namespace GV.CounterSketch.Shared;

public static class EnumExtensions
{
	public static bool Contains<T>(this Enum value, T request)
	{
		int num = Convert.ToInt32(value, CultureInfo.InvariantCulture);
		int num2 = Convert.ToInt32(request, CultureInfo.InvariantCulture);
		if (num2 == (num & num2))
		{
			return true;
		}
		return false;
	}

	public static IEnumerable<T> GetAllItems<T>() where T : struct
	{
		foreach (object value in Enum.GetValues(typeof(T)))
		{
			yield return (T)value;
		}
	}

	public static IEnumerable<T> GetAllItems<T>(this Enum value)
	{
		foreach (object value2 in Enum.GetValues(typeof(T)))
		{
			yield return (T)value2;
		}
	}

	public static IEnumerable<T> GetAllSelectedItems<T>(this Enum value)
	{
		int valueAsInt = Convert.ToInt32(value, CultureInfo.InvariantCulture);
		foreach (object value2 in Enum.GetValues(typeof(T)))
		{
			int num = Convert.ToInt32(value2, CultureInfo.InvariantCulture);
			if (num == (valueAsInt & num))
			{
				yield return (T)value2;
			}
		}
	}
}
