using System.ComponentModel;

namespace CS;

public enum LayerNumber
{
	[Description("Metal Primary")]
	Metal01 = 4,
	[Description("Metal Secondary")]
	Metal02 = 5,
	[Description("Center Gems")]
	Gem01 = 8,
	[Description("Side Gems")]
	Gem02 = 9,
	[Description("Accent Gems")]
	Gem03 = 10,
	User01 = 12,
	User02 = 13,
	User03 = 14,
	User04 = 15,
	Finger = 17,
	Cutting = 18,
	Creation = 19
}
