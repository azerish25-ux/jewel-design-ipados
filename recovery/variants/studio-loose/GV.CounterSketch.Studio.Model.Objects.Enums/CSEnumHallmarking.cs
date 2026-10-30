using System.ComponentModel;

namespace GV.CounterSketch.Studio.Model.Objects.Enums;

public enum CSEnumHallmarking
{
	[Description("No quality mark or trademark.")]
	None,
	[Description("Laser engrave quality mark and Stuller trademark.")]
	QualityStuller,
	[Description("Laser engrave quality mark and my logo.")]
	QualityLogo,
	[Description("Laser engrave quality mark, Stuller trademark and my logo.")]
	QualityLogoStuller
}
