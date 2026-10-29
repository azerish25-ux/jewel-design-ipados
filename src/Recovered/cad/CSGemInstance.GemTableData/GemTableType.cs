using System.ComponentModel;

namespace CSGemInstance.GemTableData;

public enum GemTableType
{
	[Description("Unknown")]
	Unknown,
	[Description("Cabochon")]
	Cabochon,
	[Description("Diamond")]
	Diamond,
	[Description("Gem")]
	Gem,
	[Description("Simple")]
	Simple
}
