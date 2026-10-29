using System.ComponentModel;

namespace GV.CounterSketch.Studio.Model.Objects.Enums;

public enum CSEnumStoneOptions
{
	[Description("I will supply for setting")]
	Provided,
	[Description("Supplied from Stuller")]
	Stuller,
	[Description("No stone supplied (set myself)")]
	None
}
