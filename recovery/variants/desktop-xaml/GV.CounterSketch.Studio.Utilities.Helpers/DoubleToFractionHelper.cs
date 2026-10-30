using System;
using System.Linq;
using System.Text;

namespace GV.CounterSketch.Studio.Utilities.Helpers;

public static class DoubleToFractionHelper
{
	public static string Convert(decimal pvalue, bool skip_rounding = false, decimal dplaces = 0.25m)
	{
		decimal num = pvalue;
		if (!skip_rounding)
		{
			num = DecimalRound(num, dplaces);
		}
		if (num == Math.Round(num, 0))
		{
			return num.ToString();
		}
		decimal num2 = Math.Truncate(num);
		decimal num3 = num - num2;
		uint value = 0u;
		uint result = 1u;
		if (num3 > 0m)
		{
			string text = num3.ToString().Remove(0, 2);
			uint length = (uint)text.Length;
			value = (uint)Math.Pow(10.0, length);
			uint.TryParse(text, out result);
			uint num4 = GreatestCommonDivisor(result, value);
			value /= num4;
			result /= num4;
		}
		StringBuilder stringBuilder = new StringBuilder();
		if (num2 > 0m)
		{
			stringBuilder.Append(num2);
		}
		if (num3 > 0m)
		{
			if (stringBuilder.Length > 0)
			{
				stringBuilder.Append(" ");
			}
			stringBuilder.Append(result);
			stringBuilder.Append("/");
			stringBuilder.Append(value);
		}
		return stringBuilder.ToString();
	}

	private static decimal DecimalRound(decimal val, decimal places)
	{
		string[] array = Convert(places, skip_rounding: true).Split('/');
		if (array.AsEnumerable().Count() == 2)
		{
			int num = System.Convert.ToInt32(array[1]);
			return Math.Round(val * (decimal)num) / (decimal)num;
		}
		return val;
	}

	private static uint GreatestCommonDivisor(uint valA, uint valB)
	{
		if (valA == 0 && valB == 0)
		{
			return 0u;
		}
		if (valA == 0 && valB != 0)
		{
			return valB;
		}
		if (valA != 0 && valB == 0)
		{
			return valA;
		}
		uint num = valA;
		uint num2 = valB;
		while (num != num2)
		{
			if (num > num2)
			{
				num -= num2;
			}
			else
			{
				num2 -= num;
			}
		}
		return num;
	}
}
