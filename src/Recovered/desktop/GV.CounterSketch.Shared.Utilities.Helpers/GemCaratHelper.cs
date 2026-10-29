namespace GV.CounterSketch.Shared.Utilities.Helpers;

public class GemCaratHelper
{
	public static double GetSpecificGravityForMaterial(GemMaterial material)
	{
		double result = 3.52;
		switch (material)
		{
		case GemMaterial.Unassigned:
			result = 3.52;
			break;
		case GemMaterial.CZWhite:
			result = 5.8;
			break;
		case GemMaterial.Diamond:
			result = 3.52;
			break;
		case GemMaterial.DiamondKL:
			result = 3.52;
			break;
		case GemMaterial.SapphireWhite:
			result = 4.0;
			break;
		case GemMaterial.TopazWhite:
			result = 3.53;
			break;
		case GemMaterial.CZPink:
			result = 5.8;
			break;
		case GemMaterial.CZPinkLight:
			result = 5.8;
			break;
		case GemMaterial.DiamondPink:
			result = 3.52;
			break;
		case GemMaterial.DiamondPinkPure:
			result = 3.52;
			break;
		case GemMaterial.GarnetRhodolitePinkRose:
			result = 3.84;
			break;
		case GemMaterial.GarnetRhodoliteRaspberry:
			result = 3.84;
			break;
		case GemMaterial.Morganite:
			result = 2.72;
			break;
		case GemMaterial.QuartzRose:
			result = 2.66;
			break;
		case GemMaterial.SapphirePadparadscha:
			result = 4.0;
			break;
		case GemMaterial.SapphirePink:
			result = 4.0;
			break;
		case GemMaterial.SpinelPink:
			result = 3.6;
			break;
		case GemMaterial.TopazComplexion:
			result = 3.53;
			break;
		case GemMaterial.TopazMysticPurePink:
			result = 3.53;
			break;
		case GemMaterial.TopazPassionBabyPink:
			result = 3.53;
			break;
		case GemMaterial.TopazPassionPink:
			result = 3.53;
			break;
		case GemMaterial.TourmalinePink:
			result = 3.06;
			break;
		case GemMaterial.DiamondGarnetRed:
			result = 3.6;
			break;
		case GemMaterial.GarnetBrazilian:
			result = 3.84;
			break;
		case GemMaterial.GarnetMozambique:
			result = 3.84;
			break;
		case GemMaterial.GarnetRhodolite:
			result = 3.84;
			break;
		case GemMaterial.OpalMexicanFireRed:
			result = 2.15;
			break;
		case GemMaterial.Ruby:
			result = 4.0;
			break;
		case GemMaterial.RubyMadagascar:
			result = 4.0;
			break;
		case GemMaterial.SapphireRed:
			result = 4.0;
			break;
		case GemMaterial.TopazBlazingRed:
			result = 3.53;
			break;
		case GemMaterial.Citrine:
			result = 2.66;
			break;
		case GemMaterial.CitrineMadeira:
			result = 2.66;
			break;
		case GemMaterial.DiamondBurntOrange:
			result = 3.6;
			break;
		case GemMaterial.GarnetSpessartite:
			result = 4.15;
			break;
		case GemMaterial.OpalMexicanFireOrange:
			result = 2.15;
			break;
		case GemMaterial.SapphireOrange:
			result = 4.0;
			break;
		case GemMaterial.TopazPoppy:
			result = 3.53;
			break;
		case GemMaterial.TopazPreciousImperial:
			result = 3.53;
			break;
		case GemMaterial.TopazMysticSunrise:
			result = 3.53;
			break;
		case GemMaterial.TourmalineGolden:
			result = 3.06;
			break;
		case GemMaterial.CitrineYellow:
			result = 2.66;
			break;
		case GemMaterial.CZYellow:
			result = 5.8;
			break;
		case GemMaterial.DiamondCape:
			result = 3.6;
			break;
		case GemMaterial.DiamondYellow:
			result = 3.6;
			break;
		case GemMaterial.DiamondYellowGolden:
			result = 3.6;
			break;
		case GemMaterial.DiamondYellowLight:
			result = 3.6;
			break;
		case GemMaterial.QuartzLemon:
			result = 2.66;
			break;
		case GemMaterial.SapphireYellow:
			result = 4.0;
			break;
		case GemMaterial.TopazPassionHoney:
			result = 3.53;
			break;
		case GemMaterial.TopazPreciousGolden:
			result = 3.53;
			break;
		case GemMaterial.Bloodstone:
			result = 2.6;
			break;
		case GemMaterial.DiamondAppleGreen:
			result = 3.6;
			break;
		case GemMaterial.DiamondDarkGreen:
			result = 3.6;
			break;
		case GemMaterial.Emerald:
			result = 2.72;
			break;
		case GemMaterial.GarnetTsavorite:
			result = 3.61;
			break;
		case GemMaterial.Peridot:
			result = 3.34;
			break;
		case GemMaterial.QuartzGreen:
			result = 2.66;
			break;
		case GemMaterial.TopazMysticGreen:
			result = 3.53;
			break;
		case GemMaterial.TopazRainforest:
			result = 3.53;
			break;
		case GemMaterial.TourmalineChrome:
			result = 3.06;
			break;
		case GemMaterial.TourmalineGreen:
			result = 3.06;
			break;
		case GemMaterial.TourmalineOlive:
			result = 3.06;
			break;
		case GemMaterial.TourmalineParaiba:
			result = 3.06;
			break;
		case GemMaterial.Aquamarine:
			result = 2.72;
			break;
		case GemMaterial.DiamondAquaBlue:
			result = 3.52;
			break;
		case GemMaterial.DiamondTealBlue:
			result = 3.52;
			break;
		case GemMaterial.SpinelAqua:
			result = 3.6;
			break;
		case GemMaterial.TopazAquaBlue:
			result = 3.53;
			break;
		case GemMaterial.TopazIceBlue:
			result = 3.53;
			break;
		case GemMaterial.TopazIceBlueFrosted:
			result = 3.53;
			break;
		case GemMaterial.TopazParadiseBlue:
			result = 3.53;
			break;
		case GemMaterial.TopazParaiba:
			result = 3.53;
			break;
		case GemMaterial.TopazPassionAquaBlue:
			result = 3.53;
			break;
		case GemMaterial.TopazSkyBlue:
			result = 3.53;
			break;
		case GemMaterial.TopazTeal:
			result = 3.53;
			break;
		case GemMaterial.TopazTurquoiseFrosted:
			result = 3.53;
			break;
		case GemMaterial.TourmalineBlueParaiba:
			result = 3.06;
			break;
		case GemMaterial.ZirconBlue:
			result = 4.32;
			break;
		case GemMaterial.Alexandrite:
			result = 3.73;
			break;
		case GemMaterial.DiamondBlue:
			result = 3.6;
			break;
		case GemMaterial.Iolite:
			result = 2.61;
			break;
		case GemMaterial.SapphireBlue:
			result = 4.0;
			break;
		case GemMaterial.TopazGlacierBlue:
			result = 3.53;
			break;
		case GemMaterial.TopazLondonBlue:
			result = 3.53;
			break;
		case GemMaterial.TopazPassionGlacierBlue:
			result = 3.53;
			break;
		case GemMaterial.TopazPassionIceBlue:
			result = 3.53;
			break;
		case GemMaterial.TopazPassionParadiseBlue:
			result = 3.53;
			break;
		case GemMaterial.TopazPassionTeal:
			result = 3.53;
			break;
		case GemMaterial.TopazSwissBlue:
			result = 3.53;
			break;
		case GemMaterial.Amethyst:
			result = 2.66;
			break;
		case GemMaterial.CZLavender:
			result = 5.8;
			break;
		case GemMaterial.DiamondPurple:
			result = 3.6;
			break;
		case GemMaterial.DiamondPurplishPink:
			result = 3.6;
			break;
		case GemMaterial.GarnetColorChange:
			result = 3.8;
			break;
		case GemMaterial.QuartzRoseDeFrance:
			result = 2.66;
			break;
		case GemMaterial.SapphirePurple:
			result = 4.0;
			break;
		case GemMaterial.SapphireViolet:
			result = 4.0;
			break;
		case GemMaterial.Tanzanite:
			result = 3.3;
			break;
		case GemMaterial.DiamondChampagne:
			result = 3.6;
			break;
		case GemMaterial.DiamondCognac:
			result = 3.6;
			break;
		case GemMaterial.DiamondTopLightBrown:
			result = 3.6;
			break;
		case GemMaterial.GarnetAndradite:
			result = 3.84;
			break;
		case GemMaterial.QuartzSmoky:
			result = 2.66;
			break;
		case GemMaterial.TourmalineBronze:
			result = 3.06;
			break;
		case GemMaterial.TourmalineEarth:
			result = 3.06;
			break;
		case GemMaterial.CZBlack:
			result = 5.8;
			break;
		case GemMaterial.DiamondBlack:
			result = 3.6;
			break;
		case GemMaterial.Marcasite:
			result = 5.0;
			break;
		case GemMaterial.Onyx:
			result = 2.6;
			break;
		case GemMaterial.SpinelBlack:
			result = 3.6;
			break;
		}
		return result;
	}
}
