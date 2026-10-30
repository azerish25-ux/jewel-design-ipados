using System.Runtime.Serialization;

namespace GV.CounterSketch.Shared.Pricing.Enums;

[DataContract]
public enum StudioGemColorQuality
{
	[EnumMember]
	VS_G,
	[EnumMember]
	SI1_GH,
	[EnumMember]
	SI2_SI3_GH,
	[EnumMember]
	I1_IJ
}
