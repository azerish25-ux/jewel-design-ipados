using System;

namespace Geomagic.Utilities;

public interface IConsoleWriter
{
	void SetBackgroundColor(ConsoleColor Color);

	void SetForegroundColor(ConsoleColor Color);

	void ResetColors();

	void Write(string Value);

	void WriteLine(string Value);

	void WriteLine();
}
