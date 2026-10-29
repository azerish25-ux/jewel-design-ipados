using System.Runtime.Serialization;

namespace GV.CounterSketch.Shared;

[DataContract]
public enum GemActions
{
	[EnumMember]
	Gem,
	[EnumMember]
	GemWithHead,
	[EnumMember]
	GemWithCustomHead,
	[EnumMember]
	GemWithBezel
}
