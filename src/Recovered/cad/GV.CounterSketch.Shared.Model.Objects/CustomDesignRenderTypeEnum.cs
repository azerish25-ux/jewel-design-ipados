using System.Runtime.Serialization;

namespace GV.CounterSketch.Shared.Model.Objects;

[DataContract]
public enum CustomDesignRenderTypeEnum
{
	[EnumMember]
	Library,
	[EnumMember]
	Top,
	[EnumMember]
	Front,
	[EnumMember]
	Right,
	[EnumMember]
	Perspective
}
