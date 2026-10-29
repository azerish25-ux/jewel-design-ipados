using System.Runtime.Serialization;

namespace GV.CounterSketch.Shared;

[DataContract]
public enum GemMaterial
{
	[EnumMember]
	Unassigned = 0,
	[EnumMember]
	CZWhite = 100,
	[EnumMember]
	Diamond = 105,
	[EnumMember]
	DiamondKL = 110,
	[EnumMember]
	Moissanite = 113,
	[EnumMember]
	SapphireWhite = 115,
	[EnumMember]
	TopazWhite = 120,
	[EnumMember]
	CZPink = 200,
	[EnumMember]
	CZPinkLight = 205,
	[EnumMember]
	DiamondPink = 210,
	[EnumMember]
	DiamondPinkPure = 215,
	[EnumMember]
	GarnetRhodolitePinkRose = 220,
	[EnumMember]
	GarnetRhodoliteRaspberry = 225,
	[EnumMember]
	Morganite = 230,
	[EnumMember]
	QuartzRose = 235,
	[EnumMember]
	SapphirePadparadscha = 240,
	[EnumMember]
	SapphirePink = 245,
	[EnumMember]
	SpinelPink = 250,
	[EnumMember]
	TopazComplexion = 255,
	[EnumMember]
	TopazMysticPurePink = 260,
	[EnumMember]
	TopazPassionBabyPink = 265,
	[EnumMember]
	TopazPassionPink = 270,
	[EnumMember]
	TourmalinePink = 275,
	[EnumMember]
	DiamondGarnetRed = 300,
	[EnumMember]
	GarnetBrazilian = 305,
	[EnumMember]
	GarnetMozambique = 310,
	[EnumMember]
	GarnetRhodolite = 315,
	[EnumMember]
	OpalMexicanFireRed = 320,
	[EnumMember]
	Ruby = 325,
	[EnumMember]
	RubyMadagascar = 330,
	[EnumMember]
	SapphireRed = 335,
	[EnumMember]
	TopazBlazingRed = 340,
	[EnumMember]
	Citrine = 400,
	[EnumMember]
	CitrineMadeira = 405,
	[EnumMember]
	DiamondBurntOrange = 410,
	[EnumMember]
	GarnetSpessartite = 415,
	[EnumMember]
	OpalMexicanFireOrange = 420,
	[EnumMember]
	SapphireOrange = 425,
	[EnumMember]
	TopazPoppy = 430,
	[EnumMember]
	TopazPreciousImperial = 435,
	[EnumMember]
	TopazMysticSunrise = 440,
	[EnumMember]
	TourmalineGolden = 445,
	[EnumMember]
	CitrineYellow = 500,
	[EnumMember]
	CZYellow = 505,
	[EnumMember]
	DiamondCape = 510,
	[EnumMember]
	DiamondYellow = 515,
	[EnumMember]
	DiamondYellowGolden = 520,
	[EnumMember]
	DiamondYellowLight = 525,
	[EnumMember]
	QuartzLemon = 530,
	[EnumMember]
	SapphireYellow = 535,
	[EnumMember]
	TopazPassionHoney = 540,
	[EnumMember]
	TopazPreciousGolden = 545,
	[EnumMember]
	Bloodstone = 600,
	[EnumMember]
	DiamondAppleGreen = 605,
	[EnumMember]
	DiamondDarkGreen = 610,
	[EnumMember]
	Emerald = 615,
	[EnumMember]
	GarnetTsavorite = 620,
	[EnumMember]
	Peridot = 625,
	[EnumMember]
	QuartzGreen = 630,
	[EnumMember]
	TopazMysticGreen = 635,
	[EnumMember]
	TopazRainforest = 640,
	[EnumMember]
	TourmalineChrome = 645,
	[EnumMember]
	TourmalineGreen = 650,
	[EnumMember]
	TourmalineOlive = 655,
	[EnumMember]
	TourmalineParaiba = 660,
	[EnumMember]
	Aquamarine = 700,
	[EnumMember]
	DiamondAquaBlue = 705,
	[EnumMember]
	DiamondTealBlue = 710,
	[EnumMember]
	SpinelAqua = 715,
	[EnumMember]
	TopazAquaBlue = 720,
	[EnumMember]
	TopazIceBlue = 725,
	[EnumMember]
	TopazIceBlueFrosted = 730,
	[EnumMember]
	TopazParadiseBlue = 735,
	[EnumMember]
	TopazParaiba = 740,
	[EnumMember]
	TopazPassionAquaBlue = 745,
	[EnumMember]
	TopazSkyBlue = 750,
	[EnumMember]
	TopazTeal = 755,
	[EnumMember]
	TopazTurquoiseFrosted = 760,
	[EnumMember]
	TourmalineBlueParaiba = 765,
	[EnumMember]
	ZirconBlue = 770,
	[EnumMember]
	Alexandrite = 800,
	[EnumMember]
	DiamondBlue = 805,
	[EnumMember]
	Iolite = 810,
	[EnumMember]
	SapphireBlue = 815,
	[EnumMember]
	TopazGlacierBlue = 820,
	[EnumMember]
	TopazLondonBlue = 825,
	[EnumMember]
	TopazPassionGlacierBlue = 830,
	[EnumMember]
	TopazPassionIceBlue = 835,
	[EnumMember]
	TopazPassionParadiseBlue = 840,
	[EnumMember]
	TopazPassionTeal = 845,
	[EnumMember]
	TopazSwissBlue = 850,
	[EnumMember]
	Amethyst = 900,
	[EnumMember]
	CZLavender = 905,
	[EnumMember]
	DiamondPurple = 910,
	[EnumMember]
	DiamondPurplishPink = 915,
	[EnumMember]
	GarnetColorChange = 920,
	[EnumMember]
	QuartzRoseDeFrance = 925,
	[EnumMember]
	SapphirePurple = 930,
	[EnumMember]
	SapphireViolet = 935,
	[EnumMember]
	Tanzanite = 940,
	[EnumMember]
	DiamondChampagne = 1000,
	[EnumMember]
	DiamondCognac = 1005,
	[EnumMember]
	DiamondTopLightBrown = 1010,
	[EnumMember]
	GarnetAndradite = 1015,
	[EnumMember]
	QuartzSmoky = 1020,
	[EnumMember]
	TourmalineBronze = 1025,
	[EnumMember]
	TourmalineEarth = 1030,
	[EnumMember]
	CZBlack = 1100,
	[EnumMember]
	DiamondBlack = 1105,
	[EnumMember]
	Marcasite = 1110,
	[EnumMember]
	Onyx = 1115,
	[EnumMember]
	SpinelBlack = 1120,
	[EnumMember]
	OpalWhite = 1290,
	[EnumMember]
	Pearl = 1400
}
