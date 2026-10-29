using System;
using System.Text.RegularExpressions;

namespace GV.CounterSketch.Common;

public static class StringExtensions
{
	public static string Take(this string theString, int count, bool ellipsis)
	{
		int num = Math.Min(count, theString.Length);
		string text = theString.Substring(0, num);
		if (ellipsis && num < theString.Length)
		{
			text += "...";
		}
		return text;
	}

	public static bool EzStringCompare(this string theString, string other)
	{
		string text = Regex.Replace(theString, "\\s+", "").ToLowerInvariant();
		string text2 = Regex.Replace(other, "\\s+", "").ToLowerInvariant();
		return text == text2;
	}
}
