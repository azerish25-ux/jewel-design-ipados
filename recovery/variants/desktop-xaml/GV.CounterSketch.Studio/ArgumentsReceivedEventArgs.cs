using System;

namespace GV.CounterSketch.Studio;

public class ArgumentsReceivedEventArgs : EventArgs
{
	public string[] Arguments { get; set; }
}
