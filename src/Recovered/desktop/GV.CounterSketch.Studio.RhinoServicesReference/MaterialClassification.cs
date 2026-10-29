using System.CodeDom.Compiler;
using System.Runtime.Serialization;

namespace GV.CounterSketch.Studio.RhinoServicesReference;

[GeneratedCode("System.Runtime.Serialization", "4.0.0.0")]
[DataContract(Name = "MaterialClassification", Namespace = "http://schemas.datacontract.org/2004/07/CSMaterials")]
public enum MaterialClassification
{
	[EnumMember]
	Undefined,
	[EnumMember]
	UseOriginal,
	[EnumMember]
	GemCenter,
	[EnumMember]
	GemSide,
	[EnumMember]
	GemAccent,
	[EnumMember]
	MetalPrimary,
	[EnumMember]
	MetalSecondary,
	[EnumMember]
	GemDesigner,
	[EnumMember]
	MetalDesigner,
	[EnumMember]
	GemUser,
	[EnumMember]
	MetalUser,
	[EnumMember]
	Props,
	[EnumMember]
	TryOnHand,
	[EnumMember]
	TryOnFingernails,
	[EnumMember]
	TryOnShirt,
	[EnumMember]
	Selected,
	[EnumMember]
	MouseOver,
	[EnumMember]
	Highlighted,
	[EnumMember]
	Debug,
	[EnumMember]
	Finger,
	[EnumMember]
	Cutting,
	[EnumMember]
	Creation
}
