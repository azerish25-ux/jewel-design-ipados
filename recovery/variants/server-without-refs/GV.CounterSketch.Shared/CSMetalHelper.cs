using GV.CounterSketch.Shared.Pricing.Enums;

namespace GV.CounterSketch.Shared;

public static class CSMetalHelper
{
	public static double GetFactorFromMetalMaterial(MetalMaterial metalMaterial)
	{
		return metalMaterial switch
		{
			MetalMaterial.YellowGold => 12.88, 
			MetalMaterial.RoseGold => 13.03, 
			MetalMaterial.Platinum => 20.06, 
			MetalMaterial.YellowGold10K => 11.45, 
			MetalMaterial.YellowGold18K => 15.41, 
			MetalMaterial.Palladium => 12.4, 
			MetalMaterial.Silver => 10.4, 
			MetalMaterial.WhiteGold14KX1 => 12.61, 
			MetalMaterial.Continuum => 10.4, 
			MetalMaterial.WhiteGold18K => 15.41, 
			MetalMaterial.WhiteGold10K => 10.99, 
			MetalMaterial.WhiteGold9K => 12.59, 
			MetalMaterial.YellowGold9K => 11.08, 
			MetalMaterial.RoseGold18K => 15.41, 
			MetalMaterial.RoseGold10K => 11.54, 
			MetalMaterial.GreenGold10K => 14.17, 
			MetalMaterial.GreenGold14K => 14.17, 
			MetalMaterial.GreenGold18K => 15.89, 
			MetalMaterial.YellowGold22K => 17.89, 
			MetalMaterial.PalladiumWhite14K => 14.4, 
			MetalMaterial.PalladiumWhite18K => 15.65, 
			_ => 12.6, 
		} * 0.001 / 1.55517;
	}

	public static double GetFactorFromStudioMetalQualityEnumeration(StudioMetalQuality studioMetalQuality)
	{
		return studioMetalQuality switch
		{
			StudioMetalQuality.Gold9KYellow => GetFactorFromStullerMetalString("10KY"), 
			StudioMetalQuality.Gold9KWhite => GetFactorFromStullerMetalString("10KW"), 
			StudioMetalQuality.Gold9KRose => GetFactorFromStullerMetalString("10KR"), 
			StudioMetalQuality.Gold10KYellow => GetFactorFromStullerMetalString("10KY"), 
			StudioMetalQuality.Gold10KWhite => GetFactorFromStullerMetalString("10KW"), 
			StudioMetalQuality.Gold10KRose => GetFactorFromStullerMetalString("10KR"), 
			StudioMetalQuality.Gold14KYellow => GetFactorFromStullerMetalString("14KY"), 
			StudioMetalQuality.Gold14KWhite => GetFactorFromStullerMetalString("14KW"), 
			StudioMetalQuality.Gold14KRose => GetFactorFromStullerMetalString("14KR"), 
			StudioMetalQuality.Gold14PallWhite => GetFactorFromStullerMetalString("14PW"), 
			StudioMetalQuality.Gold14KX1 => GetFactorFromStullerMetalString("14X1"), 
			StudioMetalQuality.Gold18KYellow => GetFactorFromStullerMetalString("18KY"), 
			StudioMetalQuality.Gold18KWhite => GetFactorFromStullerMetalString("18KW"), 
			StudioMetalQuality.Gold18KRose => GetFactorFromStullerMetalString("18KR"), 
			StudioMetalQuality.Gold18PallWhite => GetFactorFromStullerMetalString("18PW"), 
			StudioMetalQuality.Platinum => GetFactorFromStullerMetalString("PLAT"), 
			StudioMetalQuality.Palladium => GetFactorFromStullerMetalString("PALL"), 
			StudioMetalQuality.Silver925 => GetFactorFromStullerMetalString("STER"), 
			StudioMetalQuality.Continuum => GetFactorFromStullerMetalString("STER"), 
			_ => GetFactorFromStullerMetalString("14KW"), 
		};
	}

	public static double GetFactorFromStullerMetalString(string metal)
	{
		double num;
		switch (metal)
		{
		case "10KR":
			num = 11.54;
			break;
		case "10KW":
			num = 10.99;
			break;
		case "10KY":
			num = 11.45;
			break;
		case "10KG":
			num = 14.17;
			break;
		case "10X1":
			num = 11.18;
			break;
		case "14KG":
			num = 14.17;
			break;
		case "14KR":
			num = 13.03;
			break;
		case "14KW":
			num = 12.6;
			break;
		case "14KY":
			num = 12.88;
			break;
		case "14PW":
			num = 14.4;
			break;
		case "14X1":
			num = 12.61;
			break;
		case "18KG":
			num = 15.89;
			break;
		case "18KR":
			num = 15.41;
			break;
		case "18KW":
			num = 15.41;
			break;
		case "18KY":
			num = 15.41;
			break;
		case "18PW":
		case "18X1":
			num = 15.65;
			break;
		case "9PW":
			num = 12.59;
			break;
		case "PALL":
			num = 12.4;
			break;
		case "PLAT":
			num = 20.06;
			break;
		case "PLRU":
			num = 20.7;
			break;
		case "STER":
			num = 10.4;
			break;
		case "CONT":
			num = 10.4;
			break;
		default:
			num = 12.6;
			break;
		}
		return num * 0.001 / 1.55517;
	}

	public static string GetMetalCodeFromMetalMaterial(MetalMaterial material)
	{
		return material switch
		{
			MetalMaterial.YellowGold => "14KY", 
			MetalMaterial.WhiteGold => "14KW", 
			MetalMaterial.RoseGold => "14KR", 
			MetalMaterial.Platinum => "PLAT", 
			MetalMaterial.YellowGold10K => "10KY", 
			MetalMaterial.YellowGold18K => "18KY", 
			MetalMaterial.Palladium => "PLDM", 
			MetalMaterial.Silver => "SLVR", 
			MetalMaterial.WhiteGold14KX1 => "14X1", 
			MetalMaterial.Continuum => "CNTM", 
			MetalMaterial.WhiteGold18K => "18KW", 
			MetalMaterial.WhiteGold10K => "10KW", 
			MetalMaterial.WhiteGold9K => "9KW", 
			MetalMaterial.YellowGold9K => "9KY", 
			MetalMaterial.RoseGold18K => "18KR", 
			MetalMaterial.RoseGold10K => "10KR", 
			MetalMaterial.GreenGold10K => "10KG", 
			MetalMaterial.GreenGold14K => "14KG", 
			MetalMaterial.GreenGold18K => "18KG", 
			MetalMaterial.YellowGold22K => "22KY", 
			MetalMaterial.PalladiumWhite14K => "14PW", 
			MetalMaterial.PalladiumWhite18K => "18PW", 
			_ => "XX", 
		};
	}

	public static string GetMetalCodeFromMetalString(string metal)
	{
		return metal switch
		{
			"9K" => "10KW", 
			"10K" => "10KW", 
			"14K" => "14KW", 
			"14K X-1" => "14X1", 
			"18K" => "18KW", 
			"Palladium" => "PALL", 
			"Platinum 950" => "PLAT", 
			"Silver 925" => "STER", 
			_ => "14KY", 
		};
	}

	public static string GetMetalCodeFromMetalStringAndColor(string metal, string color)
	{
		return metal switch
		{
			"9K" => color switch
			{
				"Yellow" => "9KY", 
				"White" => "9KW", 
				"Rose" => "9KR", 
				_ => "9KY", 
			}, 
			"10K" => color switch
			{
				"Yellow" => "10KY", 
				"White" => "10KW", 
				"Rose" => "10KR", 
				_ => "10KY", 
			}, 
			"14K" => color switch
			{
				"Yellow" => "14KY", 
				"White" => "14KW", 
				"Rose" => "14KR", 
				"Palladium Alloy White" => "14PW", 
				_ => "14KY", 
			}, 
			"14K X-1" => "14X1", 
			"18K" => color switch
			{
				"Yellow" => "18KY", 
				"White" => "18KW", 
				"Rose" => "18KR", 
				"Palladium Alloy White" => "18PW", 
				_ => "18KY", 
			}, 
			"Palladium" => "PALL", 
			"Platinum 950" => "PLAT", 
			"Silver 925" => "STER", 
			_ => "14KY", 
		};
	}

	public static decimal GetMetalPennyWeight(double volume, MetalMaterial whichMetalMaterial)
	{
		if (double.IsNaN(volume))
		{
			return 0m;
		}
		double factorFromMetalMaterial = GetFactorFromMetalMaterial(whichMetalMaterial);
		return new decimal(volume * factorFromMetalMaterial);
	}

	public static string GetMetalStringFromMetalCode(string code)
	{
		return code switch
		{
			"9KY" => "9kt yellow", 
			"9KW" => "9kt white", 
			"9X1" => "9kt X1 white", 
			"9KR" => "9kt red", 
			"9KG" => "9kt green", 
			"10KY" => "10kt yellow", 
			"10KW" => "10kt white", 
			"10X1" => "10kt X1 white", 
			"10KR" => "10kt red", 
			"10KG" => "10kt green", 
			"14KY" => "14kt yellow", 
			"14KW" => "14kt white", 
			"14X1" => "14kt X1 white", 
			"14KR" => "14kt red", 
			"14KG" => "14kt green", 
			"14PW" => "14kt white palladium", 
			"18KY" => "18kt yellow", 
			"18KW" => "18kt white", 
			"18X1" => "18kt X1 white", 
			"18KR" => "18kt red", 
			"18KG" => "18kt green", 
			"PLAT" => ".950 platinum/cobalt (standard)", 
			"PLIR" => ".900 platinum/iridium", 
			"PLRU" => ".950 platinum/ruthenium", 
			"PALL" => ".950 super palladium", 
			"STER" => "Standard Sterling", 
			"ARGENTIUM/STERLING" => "Argentium Sterling", 
			_ => null, 
		};
	}

	public static double GetPennyWeightFactorFromQuickEstimateMetalString(string metal)
	{
		return metal switch
		{
			"9K" => GetFactorFromStullerMetalString("10KW"), 
			"10K" => GetFactorFromStullerMetalString("10KW"), 
			"14K" => GetFactorFromStullerMetalString("14KW"), 
			"14K X-1" => GetFactorFromStullerMetalString("14X1"), 
			"18K" => GetFactorFromStullerMetalString("18KW"), 
			"Palladium" => GetFactorFromStullerMetalString("PALL"), 
			"Platinum 950" => GetFactorFromStullerMetalString("PLAT"), 
			"Silver 925" => GetFactorFromStullerMetalString("STER"), 
			_ => GetFactorFromStullerMetalString("14KW"), 
		};
	}
}
