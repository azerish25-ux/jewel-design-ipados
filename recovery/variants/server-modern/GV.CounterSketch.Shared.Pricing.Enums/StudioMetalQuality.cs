using System.ComponentModel;
using System.Runtime.Serialization;

namespace GV.CounterSketch.Shared.Pricing.Enums;

[DataContract]
public enum StudioMetalQuality
{
	[EnumMember]
	[Description("Gold - 9K - Yellow")]
	Gold9KYellow,
	[EnumMember]
	[Description("Gold - 9K - White")]
	Gold9KWhite,
	[EnumMember]
	[Description("Gold - 9K - Rose")]
	Gold9KRose,
	[EnumMember]
	[Description("Gold - 10K - Yellow")]
	Gold10KYellow,
	[EnumMember]
	[Description("Gold - 10K - White")]
	Gold10KWhite,
	[EnumMember]
	[Description("Gold - 10K - Rose")]
	Gold10KRose,
	[EnumMember]
	[Description("Gold - 14K - Yellow")]
	Gold14KYellow,
	[EnumMember]
	[Description("Gold - 14K - White")]
	Gold14KWhite,
	[EnumMember]
	[Description("Gold - 14K - Rose")]
	Gold14KRose,
	[EnumMember]
	[Description("Gold - 14K - Palladium White")]
	Gold14PallWhite,
	[EnumMember]
	[Description("Gold - 14KX1 - White")]
	Gold14KX1,
	[EnumMember]
	[Description("Gold - 18K - Yellow")]
	Gold18KYellow,
	[EnumMember]
	[Description("Gold - 18K - White")]
	Gold18KWhite,
	[EnumMember]
	[Description("Gold - 18K - Rose")]
	Gold18KRose,
	[EnumMember]
	[Description("Gold - 18K - Palladium White")]
	Gold18PallWhite,
	[EnumMember]
	[Description("Platinum")]
	Platinum,
	[EnumMember]
	[Description("Palladium")]
	Palladium,
	[EnumMember]
	[Description("Silver - 925")]
	Silver925,
	[EnumMember]
	[Description("Continuum")]
	Continuum
}
