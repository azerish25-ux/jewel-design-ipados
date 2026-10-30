using System;

namespace Geomagic.Utilities;

public class DebugConsole
{
	public static IConsoleWriter Writer { get; set; }

	static DebugConsole()
	{
		Writer = new StandardOutputWriter();
	}

	public static void SetBackgroundColor(ConsoleColor Color)
	{
	}

	public static void SetForegroundColor(ConsoleColor Color)
	{
	}

	public static void ResetColors()
	{
	}

	public static void Write(string Value)
	{
	}

	public static void WriteLine(string Value)
	{
	}

	public static void WriteLine()
	{
	}
}
