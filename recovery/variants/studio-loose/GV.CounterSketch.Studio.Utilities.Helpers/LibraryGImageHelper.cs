using System;
using System.Globalization;
using System.Windows.Media.Imaging;
using DesignLibrary;
using DesignLibrary.Enumerations;
using GV.CounterSketch.Shared;
using GV.CounterSketch.Studio.Model.Objects.Enums;
using SceneG;

namespace GV.CounterSketch.Studio.Utilities.Helpers;

public static class LibraryGImageHelper
{
	public static bool HasGem1 { get; set; }

	public static bool HasGem2 { get; set; }

	public static bool HasGem3 { get; set; }

	public static bool HasMetal1 { get; set; }

	public static bool HasMetal2 { get; set; }

	public static Tuple<BitmapImage, BitmapImage, BitmapImage, BitmapImage> GetExtraImageTypeSources(int materialMetal1, int materialMetal2, int materialGem1, int materialGem2, int materialGem3, long designId, long Version)
	{
		return new Tuple<BitmapImage, BitmapImage, BitmapImage, BitmapImage>(GetImageSource(ImageType.Front, materialMetal1, materialMetal2, materialGem1, materialGem2, materialGem3, designId, Version), GetImageSource(ImageType.Side, materialMetal1, materialMetal2, materialGem1, materialGem2, materialGem3, designId, Version), GetImageSource(ImageType.Top, materialMetal1, materialMetal2, materialGem1, materialGem2, materialGem3, designId, Version), GetImageSource(ImageType.Perspective, materialMetal1, materialMetal2, materialGem1, materialGem2, materialGem3, designId, Version));
	}

	public static void GetGem(int gemId, out double r, out double g, out double b)
	{
		switch ((GemMaterial)Enum.Parse(typeof(GemMaterial), gemId.ToString(CultureInfo.InvariantCulture)))
		{
		case GemMaterial.CZWhite:
			r = 0.95;
			g = 0.95;
			b = 0.95;
			break;
		case GemMaterial.Diamond:
		case GemMaterial.Moissanite:
			r = 1.0;
			g = 1.0;
			b = 1.0;
			break;
		case GemMaterial.DiamondKL:
			r = 0.97;
			g = 0.94;
			b = 0.88;
			break;
		case GemMaterial.SapphireWhite:
			r = 1.0;
			g = 1.0;
			b = 1.0;
			break;
		case GemMaterial.TopazWhite:
			r = 1.0;
			g = 1.0;
			b = 1.0;
			break;
		case GemMaterial.CZPink:
			r = 0.9;
			g = 0.68;
			b = 0.64;
			break;
		case GemMaterial.CZPinkLight:
			r = 0.95;
			g = 0.85;
			b = 0.89;
			break;
		case GemMaterial.DiamondPink:
			r = 1.0;
			g = 0.72;
			b = 0.8;
			break;
		case GemMaterial.DiamondPinkPure:
			r = 0.92;
			g = 0.41;
			b = 0.5;
			break;
		case GemMaterial.GarnetRhodolitePinkRose:
			r = 1.0;
			g = 0.25;
			b = 0.44;
			break;
		case GemMaterial.GarnetRhodoliteRaspberry:
			r = 0.66;
			g = 0.19;
			b = 0.26;
			break;
		case GemMaterial.Morganite:
			r = 1.0;
			g = 0.87;
			b = 0.89;
			break;
		case GemMaterial.QuartzRose:
			r = 1.0;
			g = 0.87;
			b = 0.87;
			break;
		case GemMaterial.SapphirePadparadscha:
			r = 0.84;
			g = 0.3;
			b = 0.32;
			break;
		case GemMaterial.SapphirePink:
			r = 1.0;
			g = 0.28;
			b = 0.64;
			break;
		case GemMaterial.SpinelPink:
			r = 1.0;
			g = 0.65;
			b = 0.74;
			break;
		case GemMaterial.TopazComplexion:
			r = 1.0;
			g = 0.93;
			b = 1.0;
			break;
		case GemMaterial.TopazMysticPurePink:
			r = 0.88;
			g = 0.43;
			b = 0.72;
			break;
		case GemMaterial.TopazPassionBabyPink:
			r = 1.0;
			g = 0.71;
			b = 0.88;
			break;
		case GemMaterial.TopazPassionPink:
			r = 1.0;
			g = 0.62;
			b = 0.91;
			break;
		case GemMaterial.TourmalinePink:
			r = 0.73;
			g = 0.32;
			b = 0.44;
			break;
		case GemMaterial.DiamondGarnetRed:
			r = 0.96;
			g = 0.26;
			b = 0.18;
			break;
		case GemMaterial.GarnetBrazilian:
			r = 0.55;
			g = 0.16;
			b = 0.22;
			break;
		case GemMaterial.GarnetMozambique:
			r = 0.73;
			g = 0.12;
			b = 0.13;
			break;
		case GemMaterial.GarnetRhodolite:
			r = 0.61;
			g = 0.12;
			b = 0.2;
			break;
		case GemMaterial.OpalMexicanFireRed:
			r = 0.9;
			g = 0.15;
			b = 0.01;
			break;
		case GemMaterial.Ruby:
			r = 0.92;
			g = 0.0;
			b = 0.1;
			break;
		case GemMaterial.RubyMadagascar:
			r = 0.92;
			g = 0.0;
			b = 0.24;
			break;
		case GemMaterial.SapphireRed:
			r = 0.92;
			g = 0.13;
			b = 0.14;
			break;
		case GemMaterial.TopazBlazingRed:
			r = 0.81;
			g = 0.24;
			b = 0.35;
			break;
		case GemMaterial.Citrine:
			r = 1.0;
			g = 0.58;
			b = 0.06;
			break;
		case GemMaterial.CitrineMadeira:
			r = 0.61;
			g = 0.1;
			b = 0.1;
			break;
		case GemMaterial.DiamondBurntOrange:
			r = 1.0;
			g = 0.51;
			b = 0.18;
			break;
		case GemMaterial.GarnetSpessartite:
			r = 0.93;
			g = 0.41;
			b = 0.22;
			break;
		case GemMaterial.OpalMexicanFireOrange:
			r = 1.0;
			g = 0.35;
			b = 0.2;
			break;
		case GemMaterial.SapphireOrange:
			r = 0.95;
			g = 0.58;
			b = 0.0;
			break;
		case GemMaterial.TopazPoppy:
			r = 0.93;
			g = 0.44;
			b = 0.2;
			break;
		case GemMaterial.TopazPreciousImperial:
			r = 0.91;
			g = 0.76;
			b = 0.65;
			break;
		case GemMaterial.TopazMysticSunrise:
			r = 1.0;
			g = 0.61;
			b = 0.45;
			break;
		case GemMaterial.TourmalineGolden:
			r = 0.89;
			g = 0.49;
			b = 0.0;
			break;
		case GemMaterial.CitrineYellow:
			r = 0.97;
			g = 0.71;
			b = 0.14;
			break;
		case GemMaterial.CZYellow:
			r = 1.0;
			g = 0.89;
			b = 0.18;
			break;
		case GemMaterial.DiamondCape:
			r = 1.0;
			g = 1.0;
			b = 0.85;
			break;
		case GemMaterial.DiamondYellow:
			r = 1.0;
			g = 0.95;
			b = 0.44;
			break;
		case GemMaterial.DiamondYellowGolden:
			r = 1.0;
			g = 0.83;
			b = 0.39;
			break;
		case GemMaterial.DiamondYellowLight:
			r = 1.0;
			g = 0.97;
			b = 0.66;
			break;
		case GemMaterial.QuartzLemon:
			r = 0.9;
			g = 0.89;
			b = 0.41;
			break;
		case GemMaterial.SapphireYellow:
			r = 1.0;
			g = 0.86;
			b = 0.12;
			break;
		case GemMaterial.TopazPassionHoney:
			r = 1.0;
			g = 0.88;
			b = 0.54;
			break;
		case GemMaterial.TopazPreciousGolden:
			r = 1.0;
			g = 0.54;
			b = 0.35;
			break;
		case GemMaterial.Bloodstone:
			r = 0.2;
			g = 0.42;
			b = 0.34;
			break;
		case GemMaterial.DiamondAppleGreen:
			r = 0.94;
			g = 1.0;
			b = 0.55;
			break;
		case GemMaterial.DiamondDarkGreen:
			r = 0.57;
			g = 0.67;
			b = 0.3;
			break;
		case GemMaterial.Emerald:
			r = 0.1;
			g = 0.51;
			b = 0.19;
			break;
		case GemMaterial.GarnetTsavorite:
			r = 0.0;
			g = 0.68;
			b = 0.11;
			break;
		case GemMaterial.Peridot:
			r = 0.71;
			g = 1.0;
			b = 0.33;
			break;
		case GemMaterial.QuartzGreen:
			r = 0.89;
			g = 1.0;
			b = 0.76;
			break;
		case GemMaterial.TopazMysticGreen:
			r = 0.38;
			g = 0.3;
			b = 0.41;
			break;
		case GemMaterial.TopazRainforest:
			r = 0.25;
			g = 0.85;
			b = 0.31;
			break;
		case GemMaterial.TourmalineChrome:
			r = 0.11;
			g = 0.46;
			b = 0.0;
			break;
		case GemMaterial.TourmalineGreen:
			r = 0.4;
			g = 0.59;
			b = 0.32;
			break;
		case GemMaterial.TourmalineOlive:
			r = 0.5;
			g = 0.53;
			b = 0.15;
			break;
		case GemMaterial.TourmalineParaiba:
			r = 0.65;
			g = 0.89;
			b = 0.74;
			break;
		case GemMaterial.Aquamarine:
			r = 0.82;
			g = 1.0;
			b = 1.0;
			break;
		case GemMaterial.DiamondAquaBlue:
			r = 0.86;
			g = 1.0;
			b = 1.0;
			break;
		case GemMaterial.DiamondTealBlue:
			r = 0.79;
			g = 1.0;
			b = 0.95;
			break;
		case GemMaterial.SpinelAqua:
			r = 0.8;
			g = 0.91;
			b = 1.0;
			break;
		case GemMaterial.TopazAquaBlue:
			r = 0.85;
			g = 0.95;
			b = 1.0;
			break;
		case GemMaterial.TopazIceBlue:
			r = 0.69;
			g = 0.87;
			b = 1.0;
			break;
		case GemMaterial.TopazIceBlueFrosted:
			r = 0.76;
			g = 0.88;
			b = 1.0;
			break;
		case GemMaterial.TopazParadiseBlue:
			r = 0.31;
			g = 0.72;
			b = 0.9;
			break;
		case GemMaterial.TopazParaiba:
			r = 0.16;
			g = 0.78;
			b = 0.87;
			break;
		case GemMaterial.TopazPassionAquaBlue:
			r = 0.87;
			g = 0.95;
			b = 1.0;
			break;
		case GemMaterial.TopazSkyBlue:
			r = 0.61;
			g = 0.9;
			b = 1.0;
			break;
		case GemMaterial.TopazTeal:
			r = 0.49;
			g = 0.99;
			b = 1.0;
			break;
		case GemMaterial.TopazTurquoiseFrosted:
			r = 0.72;
			g = 0.87;
			b = 0.86;
			break;
		case GemMaterial.TourmalineBlueParaiba:
			r = 0.77;
			g = 0.96;
			b = 0.88;
			break;
		case GemMaterial.ZirconBlue:
			r = 0.68;
			g = 0.96;
			b = 0.94;
			break;
		case GemMaterial.Alexandrite:
			r = 0.26;
			g = 0.41;
			b = 0.46;
			break;
		case GemMaterial.DiamondBlue:
			r = 0.56;
			g = 0.86;
			b = 1.0;
			break;
		case GemMaterial.Iolite:
			r = 0.24;
			g = 0.24;
			b = 0.45;
			break;
		case GemMaterial.SapphireBlue:
			r = 0.12;
			g = 0.27;
			b = 0.81;
			break;
		case GemMaterial.TopazGlacierBlue:
			r = 0.26;
			g = 0.52;
			b = 0.84;
			break;
		case GemMaterial.TopazLondonBlue:
			r = 0.0;
			g = 0.64;
			b = 0.86;
			break;
		case GemMaterial.TopazPassionGlacierBlue:
			r = 0.1;
			g = 0.39;
			b = 0.81;
			break;
		case GemMaterial.TopazPassionIceBlue:
			r = 0.19;
			g = 0.6;
			b = 0.81;
			break;
		case GemMaterial.TopazPassionParadiseBlue:
			r = 0.22;
			g = 0.48;
			b = 0.96;
			break;
		case GemMaterial.TopazPassionTeal:
			r = 0.2;
			g = 0.51;
			b = 0.63;
			break;
		case GemMaterial.TopazSwissBlue:
			r = 0.45;
			g = 0.85;
			b = 1.0;
			break;
		case GemMaterial.Amethyst:
			r = 0.51;
			g = 0.2;
			b = 0.77;
			break;
		case GemMaterial.CZLavender:
			r = 0.47;
			g = 0.47;
			b = 0.72;
			break;
		case GemMaterial.DiamondPurple:
			r = 0.92;
			g = 0.4;
			b = 0.93;
			break;
		case GemMaterial.DiamondPurplishPink:
			r = 0.98;
			g = 0.61;
			b = 0.82;
			break;
		case GemMaterial.GarnetColorChange:
			r = 0.42;
			g = 0.3;
			b = 0.6;
			break;
		case GemMaterial.QuartzRoseDeFrance:
			r = 1.0;
			g = 0.75;
			b = 1.0;
			break;
		case GemMaterial.SapphirePurple:
			r = 0.53;
			g = 0.25;
			b = 0.63;
			break;
		case GemMaterial.SapphireViolet:
			r = 0.74;
			g = 0.53;
			b = 0.97;
			break;
		case GemMaterial.Tanzanite:
			r = 0.26;
			g = 0.27;
			b = 0.58;
			break;
		case GemMaterial.DiamondChampagne:
			r = 1.0;
			g = 0.92;
			b = 0.75;
			break;
		case GemMaterial.DiamondCognac:
			r = 0.8;
			g = 0.63;
			b = 0.33;
			break;
		case GemMaterial.DiamondTopLightBrown:
			r = 0.98;
			g = 0.87;
			b = 0.77;
			break;
		case GemMaterial.GarnetAndradite:
			r = 0.73;
			g = 0.51;
			b = 0.2;
			break;
		case GemMaterial.QuartzSmoky:
			r = 0.62;
			g = 0.52;
			b = 0.43;
			break;
		case GemMaterial.TourmalineBronze:
			r = 0.66;
			g = 0.45;
			b = 0.14;
			break;
		case GemMaterial.TourmalineEarth:
			r = 0.66;
			g = 0.35;
			b = 0.1;
			break;
		case GemMaterial.CZBlack:
			r = 0.08;
			g = 0.08;
			b = 0.08;
			break;
		case GemMaterial.DiamondBlack:
			r = 0.2;
			g = 0.2;
			b = 0.2;
			break;
		case GemMaterial.Marcasite:
			r = 0.5;
			g = 0.5;
			b = 0.5;
			break;
		case GemMaterial.Onyx:
			r = 0.02;
			g = 0.02;
			b = 0.02;
			break;
		case GemMaterial.SpinelBlack:
			r = 0.15;
			g = 0.15;
			b = 0.15;
			break;
		default:
			r = 0.5;
			g = 0.5;
			b = 0.5;
			break;
		}
	}

	public static BitmapImage GetImageSource(byte[] fileBytes, int materialMetal1, int materialMetal2, int materialGem1, int materialGem2, int materialGem3, long designId, long Version, int designImageWidth = 0)
	{
		if (fileBytes == null || fileBytes.Length == 0)
		{
			return null;
		}
		GImageFileLoader gImageFileLoader = new GImageFileLoader(fileBytes);
		double r;
		double g;
		double b;
		if (gImageFileLoader.IsLayerPresent("Metal01"))
		{
			HasMetal1 = true;
			GetMetal(materialMetal1, out r, out g, out b);
			gImageFileLoader.SetLayer("Metal01", r, g, b);
		}
		if (gImageFileLoader.IsLayerPresent("Metal02"))
		{
			HasMetal2 = true;
			GetMetal(materialMetal2, out r, out g, out b);
			gImageFileLoader.SetLayer("Metal02", r, g, b);
		}
		if (gImageFileLoader.IsLayerPresent("Gem01"))
		{
			HasGem1 = true;
			GetGem(materialGem1, out r, out g, out b);
			gImageFileLoader.SetLayer("Gem01", r, g, b);
		}
		if (gImageFileLoader.IsLayerPresent("Gem02"))
		{
			HasGem2 = true;
			GetGem(materialGem2, out r, out g, out b);
			gImageFileLoader.SetLayer("Gem02", r, g, b);
		}
		if (gImageFileLoader.IsLayerPresent("Gem03"))
		{
			HasGem3 = true;
			GetGem(materialGem3, out r, out g, out b);
			gImageFileLoader.SetLayer("Gem03", r, g, b);
		}
		BitmapImage bitmapImage = ImageHelper.ConvertBitmapToBitmapImageNoWrapper(gImageFileLoader.GetImage(), designImageWidth);
		bitmapImage.Freeze();
		return bitmapImage;
	}

	public static BitmapImage GetImageSource(ImageType imageType, int materialMetal1, int materialMetal2, int materialGem1, int materialGem2, int materialGem3, long designId, long Version, int designImageWidth = 0)
	{
		if (designId != 0L)
		{
			string text = null;
			long num = 0L;
			try
			{
				return GetImageSource(CSGLLibrary.GetImageData(designId, Version, (EImageType)imageType), materialMetal1, materialMetal2, materialGem1, materialGem2, materialGem3, designId, Version, designImageWidth);
			}
			catch (Exception ex)
			{
				GlobalErrorHandler.WriteWarningToEventLog($"Error Extracting Image for DesignId:{designId}\nLibrary:{text}\nFile Offset:{num}\nException:\n{ex}\n");
				return null;
			}
		}
		return null;
	}

	public static void GetMetal(int metalId, out double r, out double g, out double b)
	{
		switch (metalId)
		{
		case 0:
			r = 0.85;
			g = 0.6;
			b = 0.17;
			break;
		case 1:
			r = 0.720721;
			g = 0.720721;
			b = 0.585586;
			break;
		case 2:
			r = 0.81;
			g = 0.45;
			b = 0.35;
			break;
		case 3:
			r = 0.63964;
			g = 0.63964;
			b = 0.72973;
			break;
		case 4:
			r = 0.85;
			g = 0.62;
			b = 0.27;
			break;
		case 5:
			r = 0.85;
			g = 0.56;
			b = 0.1;
			break;
		case 6:
			r = 0.68;
			g = 0.68;
			b = 0.68;
			break;
		case 7:
			r = 0.6;
			g = 0.58;
			b = 0.08;
			break;
		case 8:
			r = 0.864;
			g = 0.864;
			b = 0.864;
			break;
		case 9:
			r = 0.720721;
			g = 0.720721;
			b = 0.585586;
			break;
		case 10:
			r = 0.864;
			g = 0.864;
			b = 0.864;
			break;
		case 11:
			r = 0.720721;
			g = 0.720721;
			b = 0.585586;
			break;
		case 12:
			r = 0.720721;
			g = 0.720721;
			b = 0.585586;
			break;
		case 13:
			r = 0.720721;
			g = 0.720721;
			b = 0.585586;
			break;
		case 14:
			r = 0.85;
			g = 0.62;
			b = 0.27;
			break;
		case 15:
			r = 0.8;
			g = 0.36;
			b = 0.24;
			break;
		case 16:
			r = 0.74;
			g = 0.45;
			b = 0.38;
			break;
		case 17:
			r = 0.71;
			g = 0.73;
			b = 0.26;
			break;
		case 18:
			r = 0.6;
			g = 0.58;
			b = 0.08;
			break;
		case 19:
			r = 0.68;
			g = 0.63;
			b = 0.06;
			break;
		case 20:
			r = 0.94;
			g = 0.64;
			b = 0.0;
			break;
		case 21:
			r = 0.68;
			g = 0.68;
			b = 0.68;
			break;
		case 22:
			r = 0.68;
			g = 0.68;
			b = 0.68;
			break;
		default:
			r = 1.0;
			g = 1.0;
			b = 1.0;
			break;
		}
	}
}
