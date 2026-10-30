using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Media.Imaging;
using GV.CounterSketch.Shared;
using GV.CounterSketch.Studio.ViewModel;
using Localization;

namespace GV.CounterSketch.Studio.Utilities.Helpers;

public class MetalHelper
{
	private static readonly Dictionary<MetalMaterial, BitmapImage> _metalMaterialImageCache = new Dictionary<MetalMaterial, BitmapImage>();

	public static BitmapImage GetImageForMetals(Enum value)
	{
		return _metalMaterialImageCache[(MetalMaterial)(object)value];
	}

	public static string GetImagePathForMetals(Enum value)
	{
		return $"pack://application:,,,/CounterSketch Studio;component/Images/Search/FilterItems/Metals/{Enum.GetName(value.GetType(), value)}.png";
	}

	public static MetalMaterial GetMaterialFromMetalFriendlyName(string name)
	{
		return GetMetalMaterialOptionItems().FirstOrDefault((MaterialOptionItem m) => m.PrimaryText == name)?.MetalMaterial ?? MetalMaterial.Platinum;
	}

	public static MetalMaterial GetMaterialFromMetalInteger(int value)
	{
		return GetMetalMaterialOptionItems().FirstOrDefault((MaterialOptionItem m) => m.SearchAttributeId == value)?.MetalMaterial ?? MetalMaterial.Platinum;
	}

	public static string GetMetalFriendlyNameFromMaterial(MetalMaterial material)
	{
		MaterialOptionItem materialOptionItem = GetMetalMaterialOptionItems().FirstOrDefault((MaterialOptionItem m) => m.MetalMaterial == material);
		if (materialOptionItem != null)
		{
			return materialOptionItem.PrimaryText;
		}
		return "Unknown";
	}

	public static ObservableCollection<MaterialOptionItem> GetMetalMaterialOptionItems(bool isResultsFiltered = true)
	{
		List<MaterialOptionItem> list = ((UIHelper.KeyProfile.KeyProfileId != 2) ? new List<MaterialOptionItem>
		{
			new MaterialOptionItem
			{
				Description = "Platinum is a white metal, but unlike gold it is used in jewelry in almost its pure form (approximately 95% pure). It is also more durable than gold, and has a heavier, more substantial feel.",
				IsFavorite = false,
				PrimaryText = "Platinum .950",
				SecondaryText = string.Empty,
				IsReadOnly = true,
				MetalMaterial = MetalMaterial.Platinum
			},
			new MaterialOptionItem
			{
				Description = "Karat palladium white gold has a greyer tone than nickel whites. Palladium is used as the whitener in place of nickel. This metal will meet European standards for purity. This metal is less prone to stress corrosion and is softer as compared to nickel white gold.",
				IsFavorite = false,
				PrimaryText = "18K Palladium White Gold",
				SecondaryText = string.Empty,
				MetalMaterial = MetalMaterial.PalladiumWhite18K,
				IsReadOnly = true
			},
			new MaterialOptionItem
			{
				Description = "Karat palladium white gold has a greyer tone than nickel whites. Palladium is used as the whitener in place of nickel. This metal will meet European standards for purity. This metal is less prone to stress corrosion and is softer as compared to nickel white gold.",
				IsFavorite = false,
				PrimaryText = "14K Palladium White Gold",
				SecondaryText = string.Empty,
				MetalMaterial = MetalMaterial.PalladiumWhite14K,
				IsReadOnly = true
			},
			new MaterialOptionItem
			{
				Description = "Palladium is in the same family as platinum, but it is softer and much less expensive. Similar light weight as silver and whiter in color than platinum or white gold, palladium is also hypoallergenic. ",
				IsFavorite = false,
				PrimaryText = "Palladium .950",
				SecondaryText = string.Empty,
				MetalMaterial = MetalMaterial.Palladium,
				IsReadOnly = true
			},
			new MaterialOptionItem
			{
				Description = "Karat white gold is a precious metal alloy that provides  exceptional  white color and bright finish. To cover its natural white-yellow tint it is commonly plated with an extremely hard element called rhodium.",
				IsFavorite = false,
				PrimaryText = "18K White Gold",
				SecondaryText = string.Empty,
				MetalMaterial = MetalMaterial.WhiteGold18K,
				IsReadOnly = true,
				RemoveFromUK = true
			},
			new MaterialOptionItem
			{
				Description = "Karat white gold is a precious metal alloy that provides  exceptional  white color and bright finish. To cover its natural white-yellow tint it is commonly plated with an extremely hard element called rhodium.",
				IsFavorite = false,
				PrimaryText = "14K White Gold",
				SecondaryText = string.Empty,
				MetalMaterial = MetalMaterial.WhiteGold,
				IsReadOnly = true,
				RemoveFromUK = true
			},
			new MaterialOptionItem
			{
				Description = "With Stuller's X-1 White Gold®, you get premium white color that provides the look of platinum at the cost of gold. You no longer need to rhodium plate in order to achieve a bright white surface finish.",
				IsFavorite = false,
				PrimaryText = "14K X-1 White Gold",
				SecondaryText = string.Empty,
				MetalMaterial = MetalMaterial.WhiteGold14KX1,
				IsReadOnly = true,
				RemoveFromUK = true
			},
			new MaterialOptionItem
			{
				Description = "Karat white gold is a precious metal alloy that provides  exceptional  white color and bright finish. To cover its natural white-yellow tint it is commonly plated with an extremely hard element called rhodium.",
				IsFavorite = false,
				PrimaryText = "10K White Gold",
				SecondaryText = string.Empty,
				MetalMaterial = MetalMaterial.WhiteGold10K,
				IsReadOnly = true,
				RemoveFromUK = true
			},
			new MaterialOptionItem
			{
				Description = "Designed especially for the European market this alloy meets the UK standard and requirements for 9kt White.",
				IsFavorite = false,
				PrimaryText = "9K White Gold",
				SecondaryText = string.Empty,
				MetalMaterial = MetalMaterial.WhiteGold9K,
				IsReadOnly = true,
				IsUKOnly = true
			},
			new MaterialOptionItem
			{
				Description = "Continuum™ sterling silver is .925 pure silver. It is considered a European-friendly sterling silver, as it contains no nickel. It is a grade 1 bright white metal. This sterling silver has an as-cast hardness that allows setting of high quality stones.",
				IsFavorite = false,
				PrimaryText = "Continuum Sterling Silver",
				SecondaryText = string.Empty,
				MetalMaterial = MetalMaterial.Continuum,
				IsReadOnly = true
			},
			new MaterialOptionItem
			{
				Description = "22 Karat yellow gold has a combination of copper and silver as its main alloy addition and is softer than 18 Karat yellow due to its higher gold content. This metal has a warm-rich yellow color.",
				IsFavorite = false,
				PrimaryText = "22K Yellow Gold",
				SecondaryText = string.Empty,
				MetalMaterial = MetalMaterial.YellowGold22K,
				IsReadOnly = true
			},
			new MaterialOptionItem
			{
				Description = "18K Gold contains 18 parts gold and 6 parts another metal(s), making it 75% gold. So, 18K Gold is more expensive due to the higher gold content, and because it has more gold, it is more yellow. However, the extra gold also makes it softer.",
				IsFavorite = false,
				PrimaryText = "18K Yellow Gold",
				SecondaryText = string.Empty,
				MetalMaterial = MetalMaterial.YellowGold18K,
				IsReadOnly = true
			},
			new MaterialOptionItem
			{
				Description = "14K Gold contains 14 parts gold and 10 parts another metal(s), making it 58.3% gold. A setting made with 14K Gold will hold stones a little better than a higher karat because the higher percent of alloy makes it a harder metal.",
				IsFavorite = false,
				PrimaryText = "14K Yellow Gold",
				SecondaryText = string.Empty,
				MetalMaterial = MetalMaterial.YellowGold,
				IsReadOnly = true
			},
			new MaterialOptionItem
			{
				Description = "10K Gold contains 10 parts gold and 14 parts another metal(s), making it 41.7% gold. 10K is the minimum karat designation that can still be called gold in the US.",
				IsFavorite = false,
				PrimaryText = "10K Yellow Gold",
				SecondaryText = string.Empty,
				MetalMaterial = MetalMaterial.YellowGold10K,
				IsReadOnly = true,
				RemoveFromUK = true
			},
			new MaterialOptionItem
			{
				Description = "Designed especially for the European market this alloy meets the UK standard and requirements for 9kt Yellow.",
				IsFavorite = false,
				PrimaryText = "9K Yellow Gold",
				SecondaryText = string.Empty,
				MetalMaterial = MetalMaterial.YellowGold9K,
				IsReadOnly = true,
				IsUKOnly = true
			},
			new MaterialOptionItem
			{
				Description = "Karat rose gold is made using a mix of pure gold with alloys including copper. The copper provides the rose-reddish color.",
				IsFavorite = false,
				PrimaryText = "18K Rose Gold",
				SecondaryText = string.Empty,
				MetalMaterial = MetalMaterial.RoseGold18K,
				IsReadOnly = true
			},
			new MaterialOptionItem
			{
				Description = "Karat rose gold is made using a mix of pure gold with alloys including copper. The copper provides the rose-reddish color.",
				IsFavorite = false,
				PrimaryText = "14K Rose Gold",
				SecondaryText = string.Empty,
				MetalMaterial = MetalMaterial.RoseGold,
				IsReadOnly = true
			},
			new MaterialOptionItem
			{
				Description = "Karat rose gold is made using a mix of pure gold with alloys including copper. The copper provides the rose-reddish color.",
				IsFavorite = false,
				PrimaryText = "10K Rose Gold",
				SecondaryText = string.Empty,
				MetalMaterial = MetalMaterial.RoseGold10K,
				IsReadOnly = true
			},
			new MaterialOptionItem
			{
				Description = "Karat green gold has a combination of copper and silver as its main alloy addition with a higher percentage of silver than copper to give it the green tint. This metal has a softer hardness due to the higher silver content.",
				IsFavorite = false,
				PrimaryText = "18K Green Gold",
				SecondaryText = string.Empty,
				MetalMaterial = MetalMaterial.GreenGold18K,
				IsReadOnly = true
			},
			new MaterialOptionItem
			{
				Description = "Karat green gold has a combination of copper and silver as its main alloy addition with a higher percentage of silver than copper to give it the green tint. This metal has a softer hardness due to the higher silver content.",
				IsFavorite = false,
				PrimaryText = "14K Green Gold",
				SecondaryText = string.Empty,
				MetalMaterial = MetalMaterial.GreenGold14K,
				IsReadOnly = true
			},
			new MaterialOptionItem
			{
				Description = "Karat green gold has a combination of copper and silver as its main alloy addition with a higher percentage of silver than copper to give it the green tint. This metal has a softer hardness due to the higher silver content.",
				IsFavorite = false,
				PrimaryText = "10K Green Gold",
				SecondaryText = string.Empty,
				MetalMaterial = MetalMaterial.GreenGold10K,
				IsReadOnly = true
			}
		} : new List<MaterialOptionItem>
		{
			new MaterialOptionItem
			{
				Description = "Platinum is a white metal, but unlike gold it is used in jewelry in almost its pure form (approximately 95% pure). It is also more durable than gold, and has a heavier, more substantial feel.",
				IsFavorite = false,
				PrimaryText = "Platinum .950",
				SecondaryText = string.Empty,
				IsReadOnly = true,
				MetalMaterial = MetalMaterial.Platinum
			},
			new MaterialOptionItem
			{
				Description = "Karat palladium white gold has a greyer tone than nickel whites. Palladium is used as the whitener in place of nickel. This metal will meet European standards for purity. This metal is less prone to stress corrosion and is softer as compared to nickel white gold.",
				IsFavorite = false,
				PrimaryText = "18K Palladium White Gold",
				SecondaryText = string.Empty,
				MetalMaterial = MetalMaterial.PalladiumWhite18K,
				IsReadOnly = true
			},
			new MaterialOptionItem
			{
				Description = "Karat palladium white gold has a greyer tone than nickel whites. Palladium is used as the whitener in place of nickel. This metal will meet European standards for purity. This metal is less prone to stress corrosion and is softer as compared to nickel white gold.",
				IsFavorite = false,
				PrimaryText = "14K Palladium White Gold",
				SecondaryText = string.Empty,
				MetalMaterial = MetalMaterial.PalladiumWhite14K,
				IsReadOnly = true
			},
			new MaterialOptionItem
			{
				Description = "Palladium is in the same family as platinum, but it is softer and much less expensive. Similar light weight as silver and whiter in color than platinum or white gold, palladium is also hypoallergenic. ",
				IsFavorite = false,
				PrimaryText = "Palladium .950",
				SecondaryText = string.Empty,
				MetalMaterial = MetalMaterial.Palladium,
				IsReadOnly = true
			},
			new MaterialOptionItem
			{
				Description = "Karat white gold is a precious metal alloy that provides  exceptional  white color and bright finish. To cover its natural white-yellow tint it is commonly plated with an extremely hard element called rhodium.",
				IsFavorite = false,
				PrimaryText = "18K White Gold",
				SecondaryText = string.Empty,
				MetalMaterial = MetalMaterial.WhiteGold18K,
				IsReadOnly = true,
				RemoveFromUK = true
			},
			new MaterialOptionItem
			{
				Description = "Karat white gold is a precious metal alloy that provides  exceptional  white color and bright finish. To cover its natural white-yellow tint it is commonly plated with an extremely hard element called rhodium.",
				IsFavorite = false,
				PrimaryText = "14K White Gold",
				SecondaryText = string.Empty,
				MetalMaterial = MetalMaterial.WhiteGold,
				IsReadOnly = true,
				RemoveFromUK = true
			},
			new MaterialOptionItem
			{
				Description = "Karat white gold is a precious metal alloy that provides  exceptional  white color and bright finish. To cover its natural white-yellow tint it is commonly plated with an extremely hard element called rhodium.",
				IsFavorite = false,
				PrimaryText = "10K White Gold",
				SecondaryText = string.Empty,
				MetalMaterial = MetalMaterial.WhiteGold10K,
				IsReadOnly = true,
				RemoveFromUK = true
			},
			new MaterialOptionItem
			{
				Description = "Designed especially for the European market this alloy meets the UK standard and requirements for 9kt White.",
				IsFavorite = false,
				PrimaryText = "9K White Gold",
				SecondaryText = string.Empty,
				MetalMaterial = MetalMaterial.WhiteGold9K,
				IsReadOnly = true,
				IsUKOnly = true
			},
			new MaterialOptionItem
			{
				Description = "18K Gold contains 18 parts gold and 6 parts another metal(s), making it 75% gold. So, 18K Gold is more expensive due to the higher gold content, and because it has more gold, it is more yellow. However, the extra gold also makes it softer.",
				IsFavorite = false,
				PrimaryText = "18K Yellow Gold",
				SecondaryText = string.Empty,
				MetalMaterial = MetalMaterial.YellowGold18K,
				IsReadOnly = true
			},
			new MaterialOptionItem
			{
				Description = "14K Gold contains 14 parts gold and 10 parts another metal(s), making it 58.3% gold. A setting made with 14K Gold will hold stones a little better than a higher karat because the higher percent of alloy makes it a harder metal.",
				IsFavorite = false,
				PrimaryText = "14K Yellow Gold",
				SecondaryText = string.Empty,
				MetalMaterial = MetalMaterial.YellowGold,
				IsReadOnly = true
			},
			new MaterialOptionItem
			{
				Description = "10K Gold contains 10 parts gold and 14 parts another metal(s), making it 41.7% gold. 10K is the minimum karat designation that can still be called gold in the US.",
				IsFavorite = false,
				PrimaryText = "10K Yellow Gold",
				SecondaryText = string.Empty,
				MetalMaterial = MetalMaterial.YellowGold10K,
				IsReadOnly = true,
				RemoveFromUK = true
			},
			new MaterialOptionItem
			{
				Description = "Designed especially for the European market this alloy meets the UK standard and requirements for 9kt Yellow.",
				IsFavorite = false,
				PrimaryText = "9K Yellow Gold",
				SecondaryText = string.Empty,
				MetalMaterial = MetalMaterial.YellowGold9K,
				IsReadOnly = true,
				IsUKOnly = true
			},
			new MaterialOptionItem
			{
				Description = "Karat rose gold is made using a mix of pure gold with alloys including copper. The copper provides the rose-reddish color.",
				IsFavorite = false,
				PrimaryText = "18K Rose Gold",
				SecondaryText = string.Empty,
				MetalMaterial = MetalMaterial.RoseGold18K,
				IsReadOnly = true
			},
			new MaterialOptionItem
			{
				Description = "Karat rose gold is made using a mix of pure gold with alloys including copper. The copper provides the rose-reddish color.",
				IsFavorite = false,
				PrimaryText = "14K Rose Gold",
				SecondaryText = string.Empty,
				MetalMaterial = MetalMaterial.RoseGold,
				IsReadOnly = true
			},
			new MaterialOptionItem
			{
				Description = "Karat rose gold is made using a mix of pure gold with alloys including copper. The copper provides the rose-reddish color.",
				IsFavorite = false,
				PrimaryText = "10K Rose Gold",
				SecondaryText = string.Empty,
				MetalMaterial = MetalMaterial.RoseGold10K,
				IsReadOnly = true
			}
		});
		list.ForEach(delegate(MaterialOptionItem _)
		{
			_.PrimaryText = TranslationHelper.GetTranslatedMetalMaterial(_.PrimaryText.Trim() + " " + _.SecondaryText.Trim());
			_.SecondaryText = string.Empty;
			_.Description = TranslationHelper.GetTranslatedMetalMaterial(_.Description);
		});
		if (CountryHelper.IsAccountInEUCountry)
		{
			list.RemoveAll((MaterialOptionItem m) => m.RemoveFromUK);
		}
		else
		{
			list.RemoveAll((MaterialOptionItem m) => m.IsUKOnly);
		}
		if (isResultsFiltered)
		{
			list = list.Where((MaterialOptionItem _) => !UIHelper.CounterSketchOptions.ExcludedMaterialsList.Contains(_.PrimaryText)).ToList();
		}
		return new ObservableCollection<MaterialOptionItem>(list);
	}

	public static void UpdateMaterialImageCache()
	{
		foreach (object value in Enum.GetValues(typeof(MetalMaterial)))
		{
			if (!_metalMaterialImageCache.ContainsKey((MetalMaterial)value))
			{
				_metalMaterialImageCache.Add((MetalMaterial)value, new BitmapImage(new Uri($"pack://application:,,,/CounterSketch Studio;component/Images/Search/FilterItems/Metals/{Enum.GetName(value.GetType(), value)}.png")));
			}
		}
	}
}
