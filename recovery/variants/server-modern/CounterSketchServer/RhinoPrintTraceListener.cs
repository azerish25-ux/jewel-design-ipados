using System;
using System.Diagnostics;
using CS;

namespace CounterSketchServer;

public class RhinoPrintTraceListener : TraceListener
{
	public override void Write(string message)
	{
		CSUtil.Print(message);
	}

	public override void WriteLine(string message)
	{
		Write(message + Environment.NewLine);
	}
}
