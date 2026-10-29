using System;

namespace GV.CounterSketch.Studio.Utilities.Helpers;

public class EnvironmentHelper
{
	public static string ProgramFilesx86()
	{
		if (8 == IntPtr.Size || !string.IsNullOrEmpty(Environment.GetEnvironmentVariable("PROCESSOR_ARCHITEW6432")))
		{
			return Environment.GetEnvironmentVariable("ProgramFiles(x86)");
		}
		return Environment.GetEnvironmentVariable("ProgramFiles");
	}
}
