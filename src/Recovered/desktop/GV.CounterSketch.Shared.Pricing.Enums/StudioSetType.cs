using System.Runtime.Serialization;

namespace GV.CounterSketch.Shared.Pricing.Enums;

[DataContract]
public enum StudioSetType
{
	[EnumMember]
	Prong,
	[EnumMember]
	Bezel,
	[EnumMember]
	Channel
}
