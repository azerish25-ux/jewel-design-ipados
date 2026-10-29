using System;

namespace CounterSketchServer.Utilities.Extensions;

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
}
