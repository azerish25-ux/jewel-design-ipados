using System.Collections.Generic;
using System.Globalization;
using System.Linq;

namespace GV.CounterSketch.Studio.Utilities.Helpers;

public static class StullerGemHelper
{
	private static readonly List<MeleeGemInfo> MeleeGemsOptions = new List<MeleeGemInfo>();

	public static List<string> GetAvailableStullerGemColorsByType(string type, string quality)
	{
		return (from stringValue in MeleeGemsOptions.Where((MeleeGemInfo melee) => melee.Type == type).SelectMany((MeleeGemInfo melee) => melee.QualityColors.Where((KeyValuePair<string, List<string>> meleeQualityColor) => meleeQualityColor.Key == quality).SelectMany((KeyValuePair<string, List<string>> qualityColor) => qualityColor.Value)).Select(TitleCaseString)
			orderby stringValue
			select stringValue).ToList();
	}

	public static List<string> GetAvailableStullerGemQualitiesByType(string type)
	{
		return (from stringValue in MeleeGemsOptions.Where((MeleeGemInfo melee) => melee.Type == type).SelectMany((MeleeGemInfo melee) => melee.QualityColors.Keys).Select(TitleCaseString)
			orderby stringValue
			select stringValue).ToList();
	}

	public static List<string> GetAvailableStullerGemTypes()
	{
		return MeleeGemsOptions.Select((MeleeGemInfo melee) => melee.Type).Select(TitleCaseString).ToList();
	}

	public static string TitleCaseString(string inputString)
	{
		return new CultureInfo("en-US", useUserOverride: false).TextInfo.ToTitleCase(inputString);
	}
}
