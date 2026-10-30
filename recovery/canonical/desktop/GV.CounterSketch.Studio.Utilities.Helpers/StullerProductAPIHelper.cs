using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GV.CounterSketch.Studio.ViewModel;
using Stuller.Web.API.v2.Models;

namespace GV.CounterSketch.Studio.Utilities.Helpers;

public static class StullerProductAPIHelper
{
	private static List<AdvancedProductFilter> _advandedProductFilters;

	private static List<AdvancedProductFilter> _allStoneColorFilters;

	private static List<AdvancedProductFilter> _allStoneFamilyFilters;

	private static List<AdvancedProductFilter> _allStoneQualityFilters;

	private static List<AdvancedProductFilter> _allStoneShapeFilters;

	private static List<AdvancedProductFilter> _allStoneUniquenessFilters;

	private static bool _isGlobalFilterCollectionInitialized;

	private static Dictionary<Tuple<double, double, string, string, string>, Product> _productCache;

	private static Dictionary<string, List<string>> _qualityCache = new Dictionary<string, List<string>>();

	private static Dictionary<Tuple<double, double, string, string, string>, Product> ProductCache
	{
		get
		{
			if (_productCache == null)
			{
				_productCache = new Dictionary<Tuple<double, double, string, string, string>, Product>();
			}
			return _productCache;
		}
	}

	public static string CreateSizeString(string documentShape, double height, double width, double other = 0.0)
	{
		string result = string.Empty;
		int num;
		switch (documentShape.ToLowerInvariant())
		{
		case "square":
		case "emerald square":
		case "radiant square":
		case "round":
		case "princess":
		case "trillion straight 2":
		case "trillion straight":
		case "triangle 2":
		case "triangle":
			num = 1;
			break;
		case "emerald":
		case "tapered baguette builder":
			num = 3;
			break;
		default:
			num = 2;
			break;
		}
		switch (num)
		{
		case 1:
			result = $"{height}";
			break;
		case 2:
			result = $"{height}X{width}";
			break;
		case 3:
			result = $"{height}X{width}X{other}";
			break;
		}
		return result;
	}

	public static AdvancedProductFilter CreateStoneSizeAdvancedProductFilter(string documentShape, double height, double width, double other = 0.0)
	{
		string value = CreateSizeString(documentShape, height, width, other);
		return new AdvancedProductFilter
		{
			Type = AdvancedProductFilterType.StoneSize,
			Values = new List<AdvancedProductFilterValue>
			{
				new AdvancedProductFilterValue
				{
					Value = value
				}
			}
		};
	}

	public static List<AdvancedProductFilter> GetAdvancedProductFilterFromString(this List<AdvancedProductFilter> filters, string desiredValue)
	{
		return (from _ in filters
			where _.Values.Any((AdvancedProductFilterValue v) => v.Value != null && v.Value.Trim().ToLowerInvariant() == desiredValue.Trim().ToLowerInvariant())
			select new AdvancedProductFilter
			{
				Type = _.Type,
				Values = _.Values.Where((AdvancedProductFilterValue v) => v.Value != null && v.Value.Trim().ToLowerInvariant() == desiredValue.Trim().ToLowerInvariant()).ToList()
			}).ToList();
	}

	public static List<AdvancedProductFilter> GetAdvancedProductFilters()
	{
		GlobalErrorHandler.WriteMethodInfo("GetAdvancedProductFilters");
		using StullerAPIClient stullerAPIClient = new StullerAPIClient(SecurityHelper.UserName, Encoding.Default.GetString(DataProtectionHelper.Unprotect(SecurityHelper.Password)));
		return stullerAPIClient.FetchAdvancedProductFilterResponse(new AdvancedProductFiltersRequest()).AdvancedProductFilter;
	}

	public static List<AdvancedProductFilter> GetAdvancedProductFilters(List<AdvancedProductFilter> productFilters, AdvancedProductFilterType desiredFilter)
	{
		return GetAdvancedProductFilters(productFilters, new List<AdvancedProductFilterType> { desiredFilter });
	}

	public static List<AdvancedProductFilter> GetAdvancedProductFilters(List<AdvancedProductFilter> productFilters, List<AdvancedProductFilterType> desiredFilters)
	{
		GlobalErrorHandler.WriteMethodInfo("GetAdvancedProductFilters");
		using StullerAPIClient stullerAPIClient = new StullerAPIClient(SecurityHelper.UserName, Encoding.Default.GetString(DataProtectionHelper.Unprotect(SecurityHelper.Password)));
		return stullerAPIClient.FetchRelatedAdvancedProductFilterResponse(new RelatedAdvancedProductFiltersRequest
		{
			AdvancedProductFilters = productFilters,
			RelatedAdvancedProductFilter = desiredFilters,
			Filter = new List<ProductFilter> { ProductFilter.Orderable }
		}).AdvancedProductFilter;
	}

	public static string GetAGTADescriptionFromCode(string agtaCharacterCode)
	{
		string result = "(N) Natural";
		switch (agtaCharacterCode.Trim().ToLowerInvariant())
		{
		case "n":
			result = "(N) Natural";
			break;
		case "b":
			result = "(B) Bleaching";
			break;
		case "c":
			result = "(C) Coating";
			break;
		case "d":
			result = "(D) Dyeing";
			break;
		case "f":
			result = "(F) Filling";
			break;
		case "h":
			result = "(H) Heating";
			break;
		case "hp":
			result = "(HP) Heating and Pressure";
			break;
		case "i":
			result = "(I) Impregnation";
			break;
		case "imit":
			result = "(IMIT) Imitation";
			break;
		case "l":
			result = "(L) Lasering";
			break;
		case "o":
			result = "(O) Oiling/Resin Infusion";
			break;
		case "r":
			result = "(R) Irradiation";
			break;
		case "syn":
			result = "(SYN) Synthetic";
			break;
		case "u":
			result = "(U) Diffusion";
			break;
		case "w":
			result = "(W) Waxing";
			break;
		}
		return result;
	}

	public static Product GetNonSerializedProduct(double height, double width, string shape, string materialName, string quality)
	{
		Product value = null;
		Tuple<double, double, string, string, string> key = new Tuple<double, double, string, string, string>(height, width, shape, materialName, quality);
		if (ProductCache.ContainsKey(key) && ProductCache.TryGetValue(key, out value))
		{
			return value;
		}
		try
		{
			Tuple<string, string> stoneFilterValuesFromMaterialName = GetStoneFilterValuesFromMaterialName(materialName);
			string item = stoneFilterValuesFromMaterialName.Item1;
			string item2 = stoneFilterValuesFromMaterialName.Item2;
			if (item.ToLowerInvariant().Trim() == "none")
			{
				return null;
			}
			if (string.IsNullOrWhiteSpace(quality))
			{
				quality = "standard";
			}
			value = GetNonSerializedProduct(height, width, shape, item, item2, quality);
		}
		catch (Exception arg)
		{
			GlobalErrorHandler.WriteErrorToEventLog($"Unable to look up product, Exception: {arg}");
		}
		if (value != null)
		{
			ProductCache.Add(key, value);
		}
		return value;
	}

	public static Product GetNonSerializedProduct(double height, double width, string shape, string family, string color, string quality)
	{
		Product result = null;
		InitializeGlobalFilterCollection();
		AdvancedProductFilter stoneSizeFilter = CreateStoneSizeAdvancedProductFilter(shape, height, width);
		List<AdvancedProductFilter> shapeFilters = new List<AdvancedProductFilter>();
		List<string> list = ConvertGemvisionCounterSketchStoneShapeToStullerStoneShapeString(shape);
		list.ForEach(delegate(string _)
		{
			AdvancedProductFilter advancedProductFilter2 = _allStoneShapeFilters.GetAdvancedProductFilterFromString(_).FirstOrDefault();
			if (advancedProductFilter2 != null)
			{
				shapeFilters.Add(advancedProductFilter2);
			}
		});
		AdvancedProductFilter stoneFamilyFilter = _allStoneFamilyFilters.GetAdvancedProductFilterFromString(family).FirstOrDefault();
		AdvancedProductFilter stoneColorFilter = null;
		AdvancedProductFilter stoneQualityFilter = null;
		AdvancedProductFilter stoneUniquenessFilter = null;
		if (quality.Trim().ToLower() == "imitation")
		{
			List<AdvancedProductFilter> stoneColorAdvancedProductFilters = GetStoneColorAdvancedProductFilters(list, family);
			if (stoneColorAdvancedProductFilters != null)
			{
				string text = stoneColorAdvancedProductFilters.Select((AdvancedProductFilter cf) => cf.Values.FirstOrDefault((AdvancedProductFilterValue cfv) => cfv.Value.Trim().ToLowerInvariant().Contains("imitation")).Value).FirstOrDefault();
				if (text != null)
				{
					stoneColorFilter = _allStoneColorFilters.GetAdvancedProductFilterFromString(text).FirstOrDefault();
					stoneQualityFilter = _allStoneQualityFilters.GetAdvancedProductFilterFromString("standard").FirstOrDefault();
				}
			}
		}
		else
		{
			string desiredValue = color;
			if (family.Trim().ToLowerInvariant() == "diamond" && string.IsNullOrWhiteSpace(color))
			{
				Tuple<string, string> stullerDiamondQualityFromUIQuality = GetStullerDiamondQualityFromUIQuality(quality);
				color = stullerDiamondQualityFromUIQuality.Item1;
				quality = stullerDiamondQualityFromUIQuality.Item2;
				desiredValue = ((!(shape.ToLower() != "round") || quality.Contains("vq") || !(shape.ToLower() != "princess")) ? color : "ghi");
			}
			else if (quality.ToLowerInvariant().Contains("forever"))
			{
				stoneUniquenessFilter = _allStoneUniquenessFilters.GetAdvancedProductFilterFromString(quality).FirstOrDefault();
				quality = "very good";
			}
			else
			{
				quality = GetStullerGemQualityFromUIQuality(quality);
			}
			stoneColorFilter = _allStoneColorFilters.GetAdvancedProductFilterFromString(desiredValue).FirstOrDefault();
			stoneQualityFilter = _allStoneQualityFilters.GetAdvancedProductFilterFromString(quality).FirstOrDefault();
		}
		AdvancedProductFilter advancedProductFilter = null;
		ProductResponse productResponse = GetNonSerializedProducts(stoneCutFilter: (!(family.Trim().ToLowerInvariant() == "diamond")) ? new AdvancedProductFilter
		{
			Type = AdvancedProductFilterType.StoneCut,
			Values = new List<AdvancedProductFilterValue>
			{
				new AdvancedProductFilterValue
				{
					DisplayValue = "Full Cut",
					Value = "Full Cut"
				},
				new AdvancedProductFilterValue
				{
					DisplayValue = "Faceted",
					Value = "Faceted"
				},
				new AdvancedProductFilterValue
				{
					DisplayValue = "Princess",
					Value = "Princess"
				},
				new AdvancedProductFilterValue
				{
					DisplayValue = "Cab",
					Value = "Cab"
				},
				new AdvancedProductFilterValue
				{
					DisplayValue = "Diamond Cut",
					Value = "Diamond Cut"
				},
				new AdvancedProductFilterValue
				{
					DisplayValue = "Step",
					Value = "Step"
				},
				new AdvancedProductFilterValue
				{
					DisplayValue = "Faceted-Machine Cut",
					Value = "Faceted-Machine Cut"
				},
				new AdvancedProductFilterValue
				{
					DisplayValue = "Princess-Machine Cut",
					Value = "Princess-Machine Cut"
				},
				new AdvancedProductFilterValue
				{
					DisplayValue = "Buff Top",
					Value = "Buff Top"
				},
				new AdvancedProductFilterValue
				{
					DisplayValue = "Star Faceted-Machine Cut",
					Value = "Star Faceted-Machine Cut"
				},
				new AdvancedProductFilterValue
				{
					DisplayValue = "Radiant",
					Value = "Radiant"
				},
				new AdvancedProductFilterValue
				{
					DisplayValue = "Hearts & Arrows",
					Value = "Hearts & Arrows"
				},
				new AdvancedProductFilterValue
				{
					DisplayValue = "Standard",
					Value = "Standard"
				},
				new AdvancedProductFilterValue
				{
					DisplayValue = "Brilliant",
					Value = "Brilliant"
				},
				new AdvancedProductFilterValue
				{
					DisplayValue = "Point",
					Value = "Point"
				},
				new AdvancedProductFilterValue
				{
					DisplayValue = "Trillion",
					Value = "Trillion"
				},
				new AdvancedProductFilterValue
				{
					DisplayValue = "Step-Machine Cut",
					Value = "Step-Machine Cut"
				},
				new AdvancedProductFilterValue
				{
					DisplayValue = "Cut",
					Value = "Cut"
				},
				new AdvancedProductFilterValue
				{
					DisplayValue = "Smooth",
					Value = "Smooth"
				},
				new AdvancedProductFilterValue
				{
					DisplayValue = "Brilliant-Machine Cut",
					Value = "Brilliant-Machine Cut"
				}
			}
		} : new AdvancedProductFilter
		{
			Type = AdvancedProductFilterType.StoneCut,
			Values = new List<AdvancedProductFilterValue>
			{
				new AdvancedProductFilterValue
				{
					DisplayValue = "Faceted",
					Value = "Faceted"
				},
				new AdvancedProductFilterValue
				{
					DisplayValue = "Princess",
					Value = "Princess"
				},
				new AdvancedProductFilterValue
				{
					DisplayValue = "Full Cut",
					Value = "Full Cut"
				}
			}
		}, stoneSizeFilter: stoneSizeFilter, stoneShapeFilters: shapeFilters, stoneFamilyFilter: stoneFamilyFilter, stoneColorFilter: stoneColorFilter, stoneQualityFilter: stoneQualityFilter, stoneUniquenessFilter: stoneUniquenessFilter);
		if (productResponse != null && productResponse.Products != null)
		{
			result = productResponse.Products.FirstOrDefault();
		}
		return result;
	}

	public static ProductResponse GetNonSerializedProducts(AdvancedProductFilter stoneSizeFilter, List<AdvancedProductFilter> stoneShapeFilters, AdvancedProductFilter stoneFamilyFilter, AdvancedProductFilter stoneColorFilter, AdvancedProductFilter stoneQualityFilter, AdvancedProductFilter stoneUniquenessFilter, AdvancedProductFilter stoneCutFilter, int desiredResultSize = 1)
	{
		GlobalErrorHandler.WriteMethodInfo("GetNonSerializedProducts");
		AdvancedProductFilter advancedProductFilter = new AdvancedProductFilter
		{
			Type = AdvancedProductFilterType.ProductType,
			Values = new List<AdvancedProductFilterValue>
			{
				new AdvancedProductFilterValue
				{
					Value = "Calibrated Gemstone"
				},
				new AdvancedProductFilterValue
				{
					Value = "Non-Serialized Diamond"
				}
			}
		};
		List<AdvancedProductFilter> list = new List<AdvancedProductFilter>();
		if (advancedProductFilter != null)
		{
			list.Add(advancedProductFilter);
		}
		if (stoneSizeFilter != null)
		{
			list.Add(stoneSizeFilter);
		}
		if (stoneShapeFilters != null)
		{
			list.AddRange(stoneShapeFilters);
		}
		if (stoneFamilyFilter != null)
		{
			list.Add(stoneFamilyFilter);
		}
		if (stoneColorFilter != null)
		{
			list.Add(stoneColorFilter);
		}
		if (stoneQualityFilter != null)
		{
			list.Add(stoneQualityFilter);
		}
		if (stoneUniquenessFilter != null)
		{
			list.Add(stoneUniquenessFilter);
		}
		if (stoneCutFilter != null)
		{
			list.Add(stoneCutFilter);
		}
		list.Add(new AdvancedProductFilter
		{
			Type = AdvancedProductFilterType.BestFitStone
		});
		using StullerAPIClient stullerAPIClient = new StullerAPIClient(SecurityHelper.UserName, Encoding.Default.GetString(DataProtectionHelper.Unprotect(SecurityHelper.Password)));
		GlobalErrorHandler.WriteMethodInfo("GetNonSerializedProducts");
		ProductRequest request = new ProductRequest
		{
			AdvancedProductFilters = list,
			Filter = new List<ProductFilter> { ProductFilter.Orderable }
		};
		return stullerAPIClient.FetchProductResponse(request);
	}

	public static List<string> GetQualitiesFromMaterialName(string materialName, string shape = "")
	{
		string key = $"{materialName}:{shape}";
		List<string> value = new List<string>();
		if (_qualityCache.TryGetValue(key, out value) && !ViewModelLocator.Current.PreferenceOrderScreenVM.IsCacheInvalid)
		{
			return value;
		}
		Tuple<string, string> stoneFilterValuesFromMaterialName = GetStoneFilterValuesFromMaterialName(materialName);
		string item = stoneFilterValuesFromMaterialName.Item1;
		string item2 = stoneFilterValuesFromMaterialName.Item2;
		if (item.ToLowerInvariant().Trim() == "none")
		{
			return new List<string>();
		}
		List<string> shapes = ConvertGemvisionCounterSketchStoneShapeToStullerStoneShapeString(shape);
		bool isIncludingImitation = CheckMaterialSupportsImitation(materialName);
		List<string> stoneQualities = GetStoneQualities(shapes, item, item2, isIncludingImitation);
		if (_qualityCache.ContainsKey(key))
		{
			_qualityCache[key] = stoneQualities;
		}
		else
		{
			_qualityCache.Add(key, stoneQualities);
		}
		return stoneQualities;
	}

	public static List<AdvancedProductFilter> GetStoneColorAdvancedProductFilters(List<string> shapes = null, string family = "")
	{
		List<AdvancedProductFilter> result = new List<AdvancedProductFilter>();
		List<AdvancedProductFilter> list = new List<AdvancedProductFilter>();
		InitializeGlobalFilterCollection();
		if (shapes != null && shapes.Any())
		{
			List<AdvancedProductFilter> selectedStoneShapeOptions = new List<AdvancedProductFilter>();
			shapes.ForEach(delegate(string _)
			{
				selectedStoneShapeOptions.Add(_allStoneShapeFilters.GetAdvancedProductFilterFromString(_).FirstOrDefault());
			});
			list.AddRange(selectedStoneShapeOptions);
		}
		else
		{
			list.AddRange(_allStoneShapeFilters);
		}
		if (!string.IsNullOrWhiteSpace(family))
		{
			List<AdvancedProductFilter> advancedProductFilterFromString = _allStoneFamilyFilters.GetAdvancedProductFilterFromString(family);
			list.AddRange(advancedProductFilterFromString);
		}
		else
		{
			list.AddRange(GetStoneFamilyAdvancedProductFilters(shapes));
		}
		List<AdvancedProductFilter> advancedProductFilters = GetAdvancedProductFilters(list, AdvancedProductFilterType.StoneColor);
		if (advancedProductFilters != null && advancedProductFilters.Any() && advancedProductFilters.Any((AdvancedProductFilter _) => _.Values.Any()))
		{
			result = advancedProductFilters;
		}
		return result;
	}

	public static List<AdvancedProductFilter> GetStoneFamilyAdvancedProductFilters(List<string> shapes = null)
	{
		List<AdvancedProductFilter> result = new List<AdvancedProductFilter>();
		List<AdvancedProductFilter> list = new List<AdvancedProductFilter>();
		InitializeGlobalFilterCollection();
		if (shapes != null && shapes.Any())
		{
			List<AdvancedProductFilter> selectedStoneShapeOptions = new List<AdvancedProductFilter>();
			shapes.ForEach(delegate(string _)
			{
				selectedStoneShapeOptions.Add(_allStoneShapeFilters.GetAdvancedProductFilterFromString(_).FirstOrDefault());
			});
			list.AddRange(selectedStoneShapeOptions);
		}
		else
		{
			list.AddRange(_allStoneShapeFilters);
		}
		List<AdvancedProductFilter> advancedProductFilters = GetAdvancedProductFilters(list, AdvancedProductFilterType.StoneFamily);
		if (advancedProductFilters != null)
		{
			result = advancedProductFilters;
		}
		return result;
	}

	public static Tuple<string, string> GetStoneFilterValuesFromMaterialName(string materialName)
	{
		Tuple<string, string> result = new Tuple<string, string>(string.Empty, string.Empty);
		switch (materialName.Trim().ToLowerInvariant())
		{
		case "alexandrite":
			result = new Tuple<string, string>("alexandrite", "standard");
			break;
		case "amethyst":
			result = new Tuple<string, string>("amethyst", "standard");
			break;
		case "aquamarine":
			result = new Tuple<string, string>("aquamarine", "standard");
			break;
		case "bloodstone":
			result = new Tuple<string, string>("bloodstone", "standard");
			break;
		case "citrine":
			result = new Tuple<string, string>("citrine", "standard");
			break;
		case "citrinemadeira":
			result = new Tuple<string, string>("citrine", "reddish orange");
			break;
		case "citrineyellow":
			result = new Tuple<string, string>("citrine", "yellowish orange");
			break;
		case "czblack":
			result = new Tuple<string, string>("cz", "black");
			break;
		case "czlavender":
			result = new Tuple<string, string>("cz", "lavender");
			break;
		case "czpink":
			result = new Tuple<string, string>("cz", "pink");
			break;
		case "czpinklight":
			result = new Tuple<string, string>("cz", "light pink");
			break;
		case "czwhite":
			result = new Tuple<string, string>("cz", "white");
			break;
		case "czyellow":
			result = new Tuple<string, string>("cz", "yellow");
			break;
		case "diamond":
			result = new Tuple<string, string>("diamond", "");
			break;
		case "diamondapplegreen":
			result = new Tuple<string, string>("diamond", "apple green");
			break;
		case "diamondaquablue":
			result = new Tuple<string, string>("diamond", "aqua blue");
			break;
		case "diamondblack":
			result = new Tuple<string, string>("diamond", "black");
			break;
		case "diamondblue":
			result = new Tuple<string, string>("diamond", "blue");
			break;
		case "diamondburntorange":
			result = new Tuple<string, string>("diamond", "burnt orange");
			break;
		case "diamondcape":
			result = new Tuple<string, string>("diamond", "light yellow");
			break;
		case "diamondchampagne":
			result = new Tuple<string, string>("diamond", "yellowish/pinkish/brown");
			break;
		case "diamondcognac":
			result = new Tuple<string, string>("diamond", "cognac");
			break;
		case "diamonddarkgreen":
			result = new Tuple<string, string>("diamond", "dark green");
			break;
		case "diamondgarnetred":
			result = new Tuple<string, string>("diamond", "garnet red");
			break;
		case "diamondkl":
			result = new Tuple<string, string>("diamond", "faint yellow");
			break;
		case "diamondpink":
			result = new Tuple<string, string>("diamond", "pink");
			break;
		case "diamondpinkpure":
			result = new Tuple<string, string>("diamond", "pink");
			break;
		case "diamondpurple":
			result = new Tuple<string, string>("diamond", "purple");
			break;
		case "diamondpurplishpink":
			result = new Tuple<string, string>("diamond", "purplish/pink");
			break;
		case "diamondtealblue":
			result = new Tuple<string, string>("diamond", "teal blue");
			break;
		case "diamondtoplightbrown":
			result = new Tuple<string, string>("diamond", "top light brown");
			break;
		case "diamondyellow":
			result = new Tuple<string, string>("diamond", "yellow");
			break;
		case "diamondyellowgolden":
			result = new Tuple<string, string>("diamond", "golden yellow");
			break;
		case "diamondyellowlight":
			result = new Tuple<string, string>("diamond", "light yellow");
			break;
		case "emerald":
			result = new Tuple<string, string>("emerald", "standard");
			break;
		case "garnetandradite":
			result = new Tuple<string, string>("garnet", "yellowish green");
			break;
		case "garnetbrazilian":
			result = new Tuple<string, string>("garnet", "purplish red");
			break;
		case "garnetcolorchange":
			result = new Tuple<string, string>("garnet", "bluish green/reddish purple");
			break;
		case "garnetmozambique":
			result = new Tuple<string, string>("garnet", "dark orange red");
			break;
		case "garnetrhodolite":
			result = new Tuple<string, string>("garnet", "reddish purple");
			break;
		case "garnetrhodolitepinkrose":
			result = new Tuple<string, string>("garnet", "pink rose");
			break;
		case "garnetrhodoliteraspberry":
			result = new Tuple<string, string>("garnet", "raspberry");
			break;
		case "garnetspessartite":
			result = new Tuple<string, string>("garnet", "orange");
			break;
		case "garnettsavorite":
			result = new Tuple<string, string>("garnet", "green");
			break;
		case "iolite":
			result = new Tuple<string, string>("iolite", "standard");
			break;
		case "moissanite":
			result = new Tuple<string, string>("moissanite", "standard");
			break;
		case "marcasite":
			result = new Tuple<string, string>("marcasite", "standard");
			break;
		case "morganite":
			result = new Tuple<string, string>("morganite", "pink");
			break;
		case "onyx":
			result = new Tuple<string, string>("onyx", "standard");
			break;
		case "opalmexicanfireorange":
			result = new Tuple<string, string>("opal", "orange-redish orange");
			break;
		case "opalmexicanfirered":
			result = new Tuple<string, string>("opal", "cherry");
			break;
		case "peridot":
			result = new Tuple<string, string>("peridot", "standard");
			break;
		case "quartzgreen":
			result = new Tuple<string, string>("quartz", "green");
			break;
		case "quartzlemon":
			result = new Tuple<string, string>("quartz", "lemon");
			break;
		case "quartzrose":
			result = new Tuple<string, string>("quartz", "rose");
			break;
		case "quartzrosedefrance":
			result = new Tuple<string, string>("quartz", "rose de france");
			break;
		case "quartzsmoky":
			result = new Tuple<string, string>("quartz", "smoky");
			break;
		case "ruby":
			result = new Tuple<string, string>("ruby", "standard");
			break;
		case "rubymadagascar":
			result = new Tuple<string, string>("ruby", "standard");
			break;
		case "sapphireblue":
			result = new Tuple<string, string>("sapphire", "blue");
			break;
		case "sapphireorange":
			result = new Tuple<string, string>("sapphire", "orange");
			break;
		case "sapphirepadparadscha":
			result = new Tuple<string, string>("sapphire", "padparadscha");
			break;
		case "sapphirepink":
			result = new Tuple<string, string>("sapphire", "pink");
			break;
		case "sapphirepurple":
			result = new Tuple<string, string>("sapphire", "purple");
			break;
		case "sapphirered":
			result = new Tuple<string, string>("sapphire", "red");
			break;
		case "sapphireviolet":
			result = new Tuple<string, string>("sapphire", "bluish purple");
			break;
		case "sapphirewhite":
			result = new Tuple<string, string>("sapphire", "white");
			break;
		case "sapphireyellow":
			result = new Tuple<string, string>("sapphire", "yellow");
			break;
		case "spinelaqua":
			result = new Tuple<string, string>("spinel", "aqua");
			break;
		case "spinelblack":
			result = new Tuple<string, string>("spinel", "black");
			break;
		case "spinelpink":
			result = new Tuple<string, string>("spinel", "pink");
			break;
		case "tanzanite":
			result = new Tuple<string, string>("tanzanite", "standard");
			break;
		case "topazaquablue":
			result = new Tuple<string, string>("topaz", "light blue");
			break;
		case "topazblazingred":
			result = new Tuple<string, string>("topaz", "blazing red");
			break;
		case "topazcomplexion":
			result = new Tuple<string, string>("topaz", "light peach");
			break;
		case "topazglacierblue":
			result = new Tuple<string, string>("topaz", "light bright blue");
			break;
		case "topaziceblue":
			result = new Tuple<string, string>("topaz", "ice blue");
			break;
		case "topazicebluefrosted":
			result = new Tuple<string, string>("topaz", "ice blue frosted");
			break;
		case "topazlondonblue":
			result = new Tuple<string, string>("topaz", "london blue");
			break;
		case "topazmysticgreen":
			result = new Tuple<string, string>("topaz", "green");
			break;
		case "topazmysticpurepink":
			result = new Tuple<string, string>("topaz", "pure pink");
			break;
		case "topazmysticsunrise":
			result = new Tuple<string, string>("topaz", "sunrise");
			break;
		case "topazparadiseblue":
			result = new Tuple<string, string>("topaz", "light bright blue");
			break;
		case "topazparaiba":
			result = new Tuple<string, string>("topaz", "paraiba");
			break;
		case "topazpassionaquablue":
			result = new Tuple<string, string>("topaz", "light blue");
			break;
		case "topazpassionbabypink":
			result = new Tuple<string, string>("topaz", "baby pink");
			break;
		case "topazpassionglacierblue":
			result = new Tuple<string, string>("topaz", "light bright blue");
			break;
		case "topazpassionhoney":
			result = new Tuple<string, string>("topaz", "honey");
			break;
		case "topazpassioniceblue":
			result = new Tuple<string, string>("topaz", "light blue");
			break;
		case "topazpassionparadiseblue":
			result = new Tuple<string, string>("topaz", "light bright blue");
			break;
		case "topazpassionpink":
			result = new Tuple<string, string>("topaz", "pink");
			break;
		case "topazpassionteal":
			result = new Tuple<string, string>("topaz", "greenish/bluish");
			break;
		case "topazpoppy":
			result = new Tuple<string, string>("topaz", "poppy");
			break;
		case "topazpreciousgolden":
			result = new Tuple<string, string>("topaz", "golden");
			break;
		case "topazpreciousimperial":
			result = new Tuple<string, string>("topaz", "imperial");
			break;
		case "topazrainforest":
			result = new Tuple<string, string>("topaz", "rainforest");
			break;
		case "topazskyblue":
			result = new Tuple<string, string>("topaz", "skyblue");
			break;
		case "topazswissblue":
			result = new Tuple<string, string>("topaz", "swiss blue");
			break;
		case "topazteal":
			result = new Tuple<string, string>("topaz", "teal");
			break;
		case "topazturquoisefrosted":
			result = new Tuple<string, string>("topaz", "turquoise frosted");
			break;
		case "topazwhite":
			result = new Tuple<string, string>("topaz", "white");
			break;
		case "tourmalineblueparaiba":
			result = new Tuple<string, string>("tourmaline", "blue (paraiba)");
			break;
		case "tourmalinebronze":
			result = new Tuple<string, string>("tourmaline", "bronze");
			break;
		case "tourmalinechrome":
			result = new Tuple<string, string>("tourmaline", "green (chrome)");
			break;
		case "tourmalineearth":
			result = new Tuple<string, string>("tourmaline", "earth");
			break;
		case "tourmalinegolden":
			result = new Tuple<string, string>("tourmaline", "golden");
			break;
		case "tourmalinegreen":
			result = new Tuple<string, string>("tourmaline", "green");
			break;
		case "tourmalineolive":
			result = new Tuple<string, string>("tourmaline", "olive");
			break;
		case "tourmalineparaiba":
			result = new Tuple<string, string>("tourmaline", "green (paraiba)");
			break;
		case "tourmalinepink":
			result = new Tuple<string, string>("tourmaline", "pink");
			break;
		case "zirconblue":
			result = new Tuple<string, string>("zircon", "blue");
			break;
		}
		return result;
	}

	public static List<string> GetStoneQualities(List<string> shapes = null, string family = "", string color = "", bool isIncludingImitation = false)
	{
		List<string> list = new List<string>();
		if (family.Trim().ToLowerInvariant() == "diamond" && string.IsNullOrWhiteSpace(color))
		{
			list = GetUIValidDiamondQualities(shapes);
		}
		else
		{
			List<AdvancedProductFilter> stoneQualityAdvancedProductFilters = GetStoneQualityAdvancedProductFilters(shapes, family, color);
			if (stoneQualityAdvancedProductFilters != null)
			{
				list = stoneQualityAdvancedProductFilters.SelectMany((AdvancedProductFilter f) => f.Values.Select((AdvancedProductFilterValue v) => v.Value)).ToList();
				if (family.ToLowerInvariant().Trim() == "moissanite")
				{
					List<string> uniquenessFilters = new List<string>();
					list.ToList().ForEach(delegate(string quality)
					{
						List<AdvancedProductFilter> stoneUniquenessAdvancedProductFilters = GetStoneUniquenessAdvancedProductFilters(shapes, family, color, quality);
						uniquenessFilters.AddRange(stoneUniquenessAdvancedProductFilters.SelectMany((AdvancedProductFilter f) => f.Values.Select((AdvancedProductFilterValue v) => v.Value)).ToList());
					});
					List<string> moissaniteQualityList = new List<string> { "Forever One", "Forever Brilliant", "Forever Classic" };
					moissaniteQualityList.ToList().ForEach(delegate(string _)
					{
						if (uniquenessFilters.All((string uf) => uf.ToLowerInvariant().Trim() != _.ToLowerInvariant().Trim()))
						{
							moissaniteQualityList.Remove(_);
						}
					});
					list = moissaniteQualityList;
				}
				else
				{
					list = GetUIValidGemQualities(list, isIncludingImitation && family.Trim().ToLowerInvariant() != "diamond");
				}
			}
		}
		if (!list.Any())
		{
			list.Add("Standard");
		}
		return list;
	}

	public static List<AdvancedProductFilter> GetStoneQualityAdvancedProductFilters(List<string> shapes = null, string family = "", string color = "")
	{
		List<AdvancedProductFilter> result = new List<AdvancedProductFilter>();
		List<AdvancedProductFilter> list = new List<AdvancedProductFilter>();
		InitializeGlobalFilterCollection();
		if (shapes.Any())
		{
			List<AdvancedProductFilter> selectedStoneShapeOptions = new List<AdvancedProductFilter>();
			shapes.ForEach(delegate(string _)
			{
				selectedStoneShapeOptions.Add(_allStoneShapeFilters.GetAdvancedProductFilterFromString(_).FirstOrDefault());
			});
		}
		else
		{
			list.AddRange(_allStoneShapeFilters);
		}
		if (!string.IsNullOrWhiteSpace(family))
		{
			List<AdvancedProductFilter> advancedProductFilterFromString = _allStoneFamilyFilters.GetAdvancedProductFilterFromString(family);
			list.AddRange(advancedProductFilterFromString);
		}
		else
		{
			list.AddRange(GetStoneFamilyAdvancedProductFilters(shapes));
		}
		if (!string.IsNullOrWhiteSpace(color))
		{
			List<AdvancedProductFilter> advancedProductFilterFromString2 = _allStoneColorFilters.GetAdvancedProductFilterFromString(color);
			list.AddRange(advancedProductFilterFromString2);
		}
		else
		{
			list.AddRange(GetStoneColorAdvancedProductFilters(shapes, family));
		}
		List<AdvancedProductFilter> advancedProductFilters = GetAdvancedProductFilters(list, AdvancedProductFilterType.StoneQuality);
		if (advancedProductFilters != null)
		{
			result = advancedProductFilters;
		}
		return result;
	}

	public static List<AdvancedProductFilter> GetStoneUniquenessAdvancedProductFilters(List<string> shapes = null, string family = "", string color = "", string quality = "")
	{
		List<AdvancedProductFilter> result = new List<AdvancedProductFilter>();
		List<AdvancedProductFilter> list = new List<AdvancedProductFilter>();
		InitializeGlobalFilterCollection();
		if (shapes.Any())
		{
			List<AdvancedProductFilter> selectedStoneShapeOptions = new List<AdvancedProductFilter>();
			shapes.ForEach(delegate(string _)
			{
				selectedStoneShapeOptions.Add(_allStoneShapeFilters.GetAdvancedProductFilterFromString(_).FirstOrDefault());
			});
		}
		else
		{
			list.AddRange(_allStoneShapeFilters);
		}
		if (!string.IsNullOrWhiteSpace(family))
		{
			List<AdvancedProductFilter> advancedProductFilterFromString = _allStoneFamilyFilters.GetAdvancedProductFilterFromString(family);
			list.AddRange(advancedProductFilterFromString);
		}
		else
		{
			list.AddRange(GetStoneFamilyAdvancedProductFilters(shapes));
		}
		if (!string.IsNullOrWhiteSpace(color))
		{
			List<AdvancedProductFilter> advancedProductFilterFromString2 = _allStoneColorFilters.GetAdvancedProductFilterFromString(color);
			list.AddRange(advancedProductFilterFromString2);
		}
		else
		{
			list.AddRange(GetStoneColorAdvancedProductFilters(shapes, family));
		}
		if (!string.IsNullOrWhiteSpace(quality))
		{
			List<AdvancedProductFilter> advancedProductFilterFromString3 = _allStoneQualityFilters.GetAdvancedProductFilterFromString(quality);
			list.AddRange(advancedProductFilterFromString3);
		}
		else
		{
			list.AddRange(GetStoneQualityAdvancedProductFilters(shapes, family, color));
		}
		List<AdvancedProductFilter> advancedProductFilters = GetAdvancedProductFilters(list, AdvancedProductFilterType.StoneUniqueness);
		if (advancedProductFilters != null)
		{
			result = advancedProductFilters;
		}
		return result;
	}

	public static List<string> GetUIValidDiamondQualities(string shape = "Round")
	{
		return GetUIValidDiamondQualities(new List<string> { shape });
	}

	public static List<string> GetUIValidDiamondQualities(List<string> shapes)
	{
		List<string> list = new List<string>();
		list.Add("VS:G-H");
		list.Add("SI1:G-H");
		list.Add("SI2-SI3:G-H");
		list.Add("I1:I-J");
		if (shapes.Contains("Round"))
		{
			list.Add("VQ1:WHITE GHI");
			list.Add("VQ2:WHITE GHI");
			list.Add("VQ3:WHITE GHI");
		}
		return list;
	}

	public static void InitializeGlobalFilterCollection()
	{
		if (!_isGlobalFilterCollectionInitialized)
		{
			_advandedProductFilters = GetAdvancedProductFilters();
			_allStoneShapeFilters = _advandedProductFilters.Where((AdvancedProductFilter a) => a.Type == AdvancedProductFilterType.StoneShape).ToList();
			_allStoneFamilyFilters = _advandedProductFilters.Where((AdvancedProductFilter a) => a.Type == AdvancedProductFilterType.StoneFamily).ToList();
			_allStoneColorFilters = _advandedProductFilters.Where((AdvancedProductFilter a) => a.Type == AdvancedProductFilterType.StoneColor).ToList();
			_allStoneQualityFilters = _advandedProductFilters.Where((AdvancedProductFilter a) => a.Type == AdvancedProductFilterType.StoneQuality).ToList();
			_allStoneUniquenessFilters = _advandedProductFilters.Where((AdvancedProductFilter a) => a.Type == AdvancedProductFilterType.StoneUniqueness).ToList();
			_isGlobalFilterCollectionInitialized = true;
		}
	}

	public static void Reset()
	{
		_qualityCache = new Dictionary<string, List<string>>();
		_productCache = new Dictionary<Tuple<double, double, string, string, string>, Product>();
	}

	private static bool CheckMaterialSupportsImitation(string materialName)
	{
		bool result = false;
		switch (materialName.Trim().ToLowerInvariant())
		{
		case "tourmalinepink":
			result = true;
			break;
		case "ruby":
			result = true;
			break;
		case "citrine":
			result = true;
			break;
		case "emerald":
			result = true;
			break;
		case "peridot":
			result = true;
			break;
		case "aquamarine":
			result = true;
			break;
		case "zirconblue":
			result = true;
			break;
		case "alexandrite":
			result = true;
			break;
		case "sapphireblue":
			result = true;
			break;
		case "amethyst":
			result = true;
			break;
		case "garnetmozambique":
			result = true;
			break;
		}
		return result;
	}

	private static List<string> ConvertGemvisionCounterSketchStoneShapeToStullerStoneShapeString(string studioGemShapeString)
	{
		List<string> list = new List<string>();
		if (studioGemShapeString == null)
		{
			return list;
		}
		switch (studioGemShapeString.Trim().ToLowerInvariant())
		{
		case "cushion":
			list.Add("Antique Cushion");
			list.Add("Antique Square");
			list.Add("Cushion");
			break;
		case "cushionsquare":
			list.Add("Antique");
			list.Add("Antique Square");
			list.Add("Cushion");
			break;
		case "emeraldsquare":
			list.Add("Asscher");
			list.Add("Emerald");
			list.Add("Emerald Cut");
			list.Add("Radiant");
			break;
		case "emerald":
			list.Add("Emerald Cut");
			list.Add("Emerald/Octagon");
			break;
		case "halfmoon":
			list.Add("Half Moon");
			break;
		case "heart":
			list.Add("Heart Shape");
			break;
		case "marquise":
			list.Add("Marquise");
			break;
		case "oval":
			list.Add("Oval");
			break;
		case "pear":
			list.Add("Pear Shape");
			break;
		case "radiant":
			list.Add("Radiant");
			break;
		case "round":
			list.Add("Round");
			break;
		case "princess":
			list.Add("Square");
			list.Add("Princess");
			list.Add("Modified Square");
			break;
		case "baguettestraight":
			list.Add("Straight Baguette");
			break;
		case "triangle":
			list.Add("Triangle");
			break;
		case "trillion":
		case "trillionround":
			list.Add("Trillion");
			break;
		case "baguettetapered":
			list.Add("Tapered Baguette");
			break;
		}
		return list;
	}

	private static Tuple<string, string> GetStullerDiamondQualityFromUIQuality(string uiQuality)
	{
		string[] source = uiQuality.Trim().ToLowerInvariant().Split(':');
		string text = source.FirstOrDefault();
		string text2 = source.LastOrDefault();
		if (text2 == "h-i")
		{
			text2 = "q36-h-i";
		}
		if (text == "i1-i2")
		{
			text = "q36-i2";
		}
		return new Tuple<string, string>(text2, text);
	}

	private static string GetStullerGemQualityFromUIQuality(string uiQuality)
	{
		return uiQuality.Trim().ToLowerInvariant() switch
		{
			"swarovski" => "fine", 
			"chatham" => "gem", 
			_ => uiQuality.Trim().ToLowerInvariant(), 
		};
	}

	private static List<string> GetUIValidGemQualities(List<string> availableQualities, bool isIncludingImitation)
	{
		List<string> list = new List<string>();
		if (availableQualities.Any((string _) => _.Trim().ToLowerInvariant() == "si1"))
		{
			list.Add("SI1");
		}
		if (availableQualities.Any((string _) => _.Trim().ToLowerInvariant() == "si"))
		{
			list.Add("SI");
		}
		if (availableQualities.Any((string _) => _.Trim().ToLowerInvariant() == "i1"))
		{
			list.Add("I1");
		}
		if (UIHelper.KeyProfile.KeyProfileId != 2 && availableQualities.Any((string _) => _.Trim().ToLowerInvariant() == "fine"))
		{
			list.Add("Swarovski");
		}
		if (availableQualities.Any((string _) => _.Trim().ToLowerInvariant() == "aaa"))
		{
			list.Add("AAA");
		}
		if (availableQualities.Any((string _) => _.Trim().ToLowerInvariant() == "aa"))
		{
			list.Add("AA");
		}
		if (availableQualities.Any((string _) => _.Trim().ToLowerInvariant() == "a"))
		{
			list.Add("A");
		}
		if (availableQualities.Any((string _) => _.Trim().ToLowerInvariant() == "gem"))
		{
			list.Add("Chatham");
		}
		if (UIHelper.KeyProfile.KeyProfileId != 2 && isIncludingImitation)
		{
			list.Add("Imitation");
		}
		return list;
	}
}
