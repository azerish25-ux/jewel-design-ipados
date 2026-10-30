using System.Runtime.Serialization;

namespace GV.CounterSketch.Shared.Pricing.Enums;

[DataContract]
public enum StudioGemType
{
	[EnumMember]
	Genuine,
	[EnumMember]
	Lab,
	[EnumMember]
	Imitation,
	[EnumMember]
	Customer
}
