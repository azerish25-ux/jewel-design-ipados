using System;
using GV.CounterSketch.Shared;

namespace CS.Reporting;

public sealed class CSReportSurface
{
	public bool Closed;

	public ComponentsEnum WhichComponent;

	public string Name;

	public MetalParts Part;

	public double Volume;

	public string GetReportString()
	{
		string empty = string.Empty;
		if (Closed)
		{
			double num = Math.Round(Volume, 2);
			empty = empty + "Solid " + Math.Round(num, 2) + " cubic mm. " + Math.Round(num * 0.0154881045, 2) + "g";
		}
		else
		{
			empty += "Open Solid";
		}
		return empty + "    : " + Name;
	}
}
