using System.ComponentModel;

namespace CSMaterials;

public enum MaterialClassification
{
	Undefined,
	UseOriginal,
	[Description("Center Gems")]
	GemCenter,
	[Description("Side Gems")]
	GemSide,
	[Description("Accent Gems")]
	GemAccent,
	[Description("Metal Primary")]
	MetalPrimary,
	[Description("Metal Secondary")]
	MetalSecondary,
	GemDesigner,
	MetalDesigner,
	GemUser,
	MetalUser,
	Props,
	TryOnHand,
	TryOnFingernails,
	TryOnShirt,
	Selected,
	MouseOver,
	Highlighted,
	Debug,
	Finger,
	Cutting,
	Creation
}
