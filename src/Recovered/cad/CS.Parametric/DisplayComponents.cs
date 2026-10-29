using System;
using System.ComponentModel;

namespace CS.Parametric;

[Flags]
public enum DisplayComponents
{
	[Description("None")]
	None = 0,
	[Description("Ring")]
	Ring = 1,
	[Description("Band 1")]
	Band = 2,
	[Description("Band 2")]
	Band2 = 4,
	[Description("Pendant")]
	Pendant = 8,
	[Description("Bracelet")]
	Bracelet = 0x10,
	[Description("Earring")]
	Earring = 0x20,
	[Description("Necklace")]
	Necklace = 0x40,
	[Description("Pin Broach")]
	PinBroach = 0x80,
	[Description("All")]
	All = 0xFF,
	[Description("All No Bands")]
	AllNoBands = 0xF9
}
