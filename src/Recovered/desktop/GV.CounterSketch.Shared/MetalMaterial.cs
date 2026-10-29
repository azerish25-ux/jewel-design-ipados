using System.Runtime.Serialization;

namespace GV.CounterSketch.Shared;

[DataContract]
public enum MetalMaterial
{
	[EnumMember]
	YellowGold = 0,
	[EnumMember]
	WhiteGold = 1,
	[EnumMember]
	RoseGold = 2,
	[EnumMember]
	Platinum = 3,
	[EnumMember]
	YellowGold10K = 4,
	[EnumMember]
	YellowGold18K = 5,
	[EnumMember]
	Palladium = 6,
	[EnumMember]
	Silver = 8,
	[EnumMember]
	WhiteGold14KX1 = 9,
	[EnumMember]
	Continuum = 10,
	[EnumMember]
	WhiteGold18K = 11,
	[EnumMember]
	WhiteGold10K = 12,
	[EnumMember]
	WhiteGold9K = 13,
	[EnumMember]
	YellowGold9K = 14,
	[EnumMember]
	RoseGold18K = 15,
	[EnumMember]
	RoseGold10K = 16,
	[EnumMember]
	GreenGold10K = 17,
	[EnumMember]
	GreenGold14K = 18,
	[EnumMember]
	GreenGold18K = 19,
	[EnumMember]
	YellowGold22K = 20,
	[EnumMember]
	PalladiumWhite14K = 21,
	[EnumMember]
	PalladiumWhite18K = 22
}
