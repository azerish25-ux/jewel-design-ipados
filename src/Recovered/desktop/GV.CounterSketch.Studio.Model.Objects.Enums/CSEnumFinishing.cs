using System.ComponentModel;

namespace GV.CounterSketch.Studio.Model.Objects.Enums;

public enum CSEnumFinishing
{
	[Description("Prototype")]
	SampleModel,
	[Description("Semi-Finished Casting (No Gems)")]
	SemiFinish,
	[Description("Finished Casting (No Gems)")]
	Finished,
	[Description("Finished & Set (As Specified)")]
	FinishedAndSet
}
