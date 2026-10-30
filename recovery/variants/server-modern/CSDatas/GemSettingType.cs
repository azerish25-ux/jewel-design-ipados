using System.ComponentModel;

namespace CSDatas;

public enum GemSettingType
{
	[Description("Unknown")]
	XX,
	[Description("Bright Cut")]
	BC,
	[Description("Faux Bright Cut")]
	FB,
	[Description("Bar")]
	BR,
	[Description("Bezel")]
	BZ,
	[Description("Half Bezel")]
	HB,
	[Description("Prong")]
	PR,
	[Description("V-Prong")]
	VP,
	[Description("Channel")]
	CH,
	[Description("Prong Half Channel")]
	PC,
	[Description("Cement")]
	CM,
	[Description("Faux Pave")]
	FP,
	[Description("Pave")]
	PV,
	[Description("Fish Tail")]
	FT,
	[Description("Gypsy")]
	GP,
	[Description("Mabe")]
	MB,
	[Description("Pin Point")]
	PT,
	[Description("Shared Prong")]
	SH,
	[Description("French Set")]
	FC,
	[Description("Claw Prong")]
	CW
}
