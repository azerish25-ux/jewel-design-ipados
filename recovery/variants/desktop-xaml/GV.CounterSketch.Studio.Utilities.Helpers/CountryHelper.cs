using System.Collections.Generic;
using System.Linq;

namespace GV.CounterSketch.Studio.Utilities.Helpers;

public static class CountryHelper
{
	public static List<string> EUCountries { get; } = new List<string>
	{
		"AU", "BE", "BG", "CZ", "DK", "DE", "EE", "IE", "EL", "ES",
		"FR", "GB", "HR", "IT", "CY", "LV", "LT", "LU", "HU", "MT",
		"NL", "NZ", "AT", "PL", "PT", "RO", "SI", "SK", "SE", "UK"
	};

	public static bool IsAccountInEUCountry => EUCountries.Any((string cc) => cc == UIHelper.CounterSketchOptions.CountryCode);
}
