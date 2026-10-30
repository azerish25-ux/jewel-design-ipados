using System;
using System.IO;

namespace Geomagic.Utilities;

public class StandardOutputWriter : IConsoleWriter
{
	public void SetBackgroundColor(ConsoleColor Color)
	{
		Console.BackgroundColor = Color;
	}

	public void SetForegroundColor(ConsoleColor Color)
	{
		Console.ForegroundColor = Color;
	}

	public void ResetColors()
	{
		Console.ResetColor();
	}

	public void Write(string Value)
	{
		using StreamWriter streamWriter = new StreamWriter(Console.OpenStandardOutput());
		streamWriter.Write(Value);
	}

	public void WriteLine(string Value)
	{
		using StreamWriter streamWriter = new StreamWriter(Console.OpenStandardOutput());
		streamWriter.WriteLine(Value);
	}

	public void WriteLine()
	{
		using StreamWriter streamWriter = new StreamWriter(Console.OpenStandardOutput());
		streamWriter.WriteLine();
	}
}
