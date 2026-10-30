using System;

namespace Geomagic.Utilities;

public class StandardConsole
{
	public static IConsoleWriter Writer { get; set; }

	static StandardConsole()
	{
		Writer = new StandardOutputWriter();
	}

	public static void SetBackgroundColor(ConsoleColor Color)
	{
		Writer.SetBackgroundColor(Color);
	}

	public static void SetForegroundColor(ConsoleColor Color)
	{
		Writer.SetForegroundColor(Color);
	}

	public static void ResetColors()
	{
		Writer.ResetColors();
	}

	public static void Write(string Value)
	{
		Writer.Write(Value);
	}

	public static void WriteLine(string Value)
	{
		Writer.WriteLine(Value);
	}

	public static void WriteLine()
	{
		Writer.WriteLine();
	}
}
