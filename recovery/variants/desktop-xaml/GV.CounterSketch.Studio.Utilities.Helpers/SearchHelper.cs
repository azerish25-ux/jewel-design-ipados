using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Threading;
using GV.CounterSketch.Studio.Database;
using GV.CounterSketch.Studio.Model.Entities.CounterSketchLibrary;
using GV.CounterSketch.Studio.ViewModel;
using GalaSoft.MvvmLight;
using Localization;

namespace GV.CounterSketch.Studio.Utilities.Helpers;

public class SearchHelper
{
	private static readonly Dictionary<int, List<int>> _centerShapeAttributeToGemTableShapeIdLookup;

	private static readonly string _connString;

	private static readonly Dictionary<int, int> _everAndEverBandToSeriesCrossReference;

	private static bool _isDonePrecachingKeywords;

	public static List<string> SearchKeywords { get; }

	public static Dictionary<int, string> DesignKeywords { get; private set; }

	static SearchHelper()
	{
		_centerShapeAttributeToGemTableShapeIdLookup = new Dictionary<int, List<int>>
		{
			{
				0,
				new List<int> { 2, 14 }
			},
			{
				23,
				new List<int> { 1, 29 }
			},
			{
				24,
				new List<int> { 3 }
			},
			{
				25,
				new List<int> { 4, 6 }
			},
			{
				26,
				new List<int> { 7, 8, 9 }
			},
			{
				28,
				new List<int> { 10 }
			},
			{
				29,
				new List<int> { 11 }
			},
			{
				30,
				new List<int> { 12, 13 }
			},
			{
				31,
				new List<int> { 15, 16 }
			},
			{
				32,
				new List<int> { 17 }
			},
			{
				33,
				new List<int> { 18, 22 }
			},
			{
				81,
				new List<int> { 19, 20 }
			},
			{
				82,
				new List<int> { 21 }
			},
			{
				83,
				new List<int> { 23, 24, 25, 26, 27, 28 }
			},
			{
				279,
				new List<int> { 5 }
			}
		};
		_connString = "Data Source=" + UIHelper.CSLibraryDatabasePath + ";BinaryGUID=No";
		_everAndEverBandToSeriesCrossReference = new Dictionary<int, int>
		{
			{ 122122, 122085 },
			{ 122125, 122090 },
			{ 122127, 122093 },
			{ 122128, 122094 },
			{ 122129, 122096 },
			{ 122138, 122114 },
			{ 122145, 121987 },
			{ 122147, 121992 },
			{ 122150, 121995 },
			{ 122151, 121996 },
			{ 122152, 121997 },
			{ 122153, 121999 },
			{ 122154, 122000 },
			{ 122156, 122002 },
			{ 122158, 122044 },
			{ 122159, 122045 },
			{ 122163, 122052 },
			{ 122166, 122063 },
			{ 122167, 122064 },
			{ 122168, 122065 },
			{ 122180, 122179 },
			{ 122182, 122181 },
			{ 122186, 71638 },
			{ 122189, 122188 },
			{ 122192, 122191 },
			{ 122208, 122207 },
			{ 122217, 122216 },
			{ 122220, 122219 },
			{ 122222, 122221 },
			{ 122233, 122232 },
			{ 122239, 122238 },
			{ 122269, 122268 },
			{ 122275, 122274 },
			{ 122286, 122285 },
			{ 122289, 122288 },
			{ 122349, 122348 },
			{ 122351, 122350 },
			{ 122377, 122376 },
			{ 122710, 122003 },
			{ 122711, 122699 },
			{ 122717, 122665 },
			{ 122721, 122669 },
			{ 122728, 121988 },
			{ 122805, 122804 },
			{ 122814, 122790 },
			{ 122839, 122838 },
			{ 122849, 122848 },
			{ 122865, 122864 },
			{ 122871, 122870 },
			{ 122876, 122875 },
			{ 122893, 122892 },
			{ 122936, 122933 },
			{ 122937, 122934 },
			{ 122957, 122956 },
			{ 122959, 122909 },
			{ 122994, 122993 },
			{ 122997, 122986 },
			{ 123002, 123001 },
			{ 123024, 123023 },
			{ 123040, 123038 },
			{ 123088, 123081 },
			{ 123135, 123134 },
			{ 123147, 123082 },
			{ 123152, 123151 },
			{ 123224, 123021 },
			{ 123246, 123240 },
			{ 123254, 123253 },
			{ 123268, 123267 },
			{ 123272, 123271 },
			{ 123306, 123305 },
			{ 123311, 123310 },
			{ 123334, 123333 },
			{ 123346, 123345 },
			{ 51220, 122089 },
			{ 51223, 122099 },
			{ 51226, 122105 },
			{ 51228, 122118 },
			{ 51229, 122119 },
			{ 51230, 69706 },
			{ 51233, 121986 },
			{ 51234, 121990 },
			{ 51235, 121998 },
			{ 51236, 122004 },
			{ 51237, 122005 },
			{ 51238, 122011 },
			{ 51239, 71602 },
			{ 51247, 121981 },
			{ 51248, 122043 },
			{ 51249, 122047 },
			{ 51251, 122053 },
			{ 51252, 122054 },
			{ 51253, 122059 },
			{ 51254, 122060 },
			{ 51302, 122177 },
			{ 51304, 122187 },
			{ 51305, 122200 },
			{ 51309, 122218 },
			{ 51322, 122231 },
			{ 51355, 122279 },
			{ 51364, 122316 },
			{ 51391, 122353 },
			{ 51488, 122563 },
			{ 51543, 122676 },
			{ 51544, 122705 },
			{ 51594, 122854 },
			{ 51596, 122884 },
			{ 51599, 122903 },
			{ 51619, 122917 },
			{ 51621, 122935 },
			{ 51653, 123044 },
			{ 51657, 123059 },
			{ 51691, 123136 },
			{ 51715, 123173 },
			{ 51725, 123213 },
			{ 51815, 123173 }
		};
		SearchKeywords = new List<string>();
		DesignKeywords = new Dictionary<int, string>();
		if (ViewModelBase.IsInDesignModeStatic)
		{
			SearchKeywords.Add("TestKeyword 0");
			SearchKeywords.Add("TestKeyword 1");
			SearchKeywords.Add("TestKeyword 2");
			SearchKeywords.Add("TestKeyword 3");
			SearchKeywords.Add("TestKeyword 4");
			SearchKeywords.Add("TestKeyword 5");
		}
	}

	public static List<DesignSearch> Get3CDesigns(bool includeVariants = false)
	{
		using CounterSketchLibraryDataEntities1 counterSketchLibraryDataEntities = new CounterSketchLibraryDataEntities1(CSLibraryDAL.NameOfConnectionString);
		return new List<DesignSearch>(counterSketchLibraryDataEntities.DesignSearches.Where((DesignSearch designSearch) => designSearch.Design.DesignAttributes.Any((GV.CounterSketch.Studio.Model.Entities.CounterSketchLibrary.DesignAttribute designAttribute) => designAttribute.AttributeId == (long?)358L && designAttribute.AttributeValue != null && designAttribute.AttributeValue != string.Empty && designAttribute.AttributeValue != "0")));
	}

	public static List<DesignSearch> GetAllBaseRingDesigns(bool includeVariants = false)
	{
		using CounterSketchLibraryDataEntities1 counterSketchLibraryDataEntities = new CounterSketchLibraryDataEntities1(CSLibraryDAL.NameOfConnectionString);
		return new List<DesignSearch>(counterSketchLibraryDataEntities.DesignSearches.Where((DesignSearch design) => design.ComponentId == (long?)1L && design.IsFreeHandPart != (long?)1L && design.IsFreeHandPartWithGem != (long?)1L && design.IsStaticFreeHandPart != (long?)1L && (includeVariants ? true : (design.IsBaseDesign == (long?)1L || design.IsIncludedWithBaseDesigns == (long?)1L))));
	}

	public static int? GetCenterGemControlGraphIndex(int designId)
	{
		int? result = null;
		using (CounterSketchLibraryDataEntities1 counterSketchLibraryDataEntities = new CounterSketchLibraryDataEntities1(CSLibraryDAL.NameOfConnectionString))
		{
			Design design = counterSketchLibraryDataEntities.Designs.FirstOrDefault((Design _) => _.DesignId == (long)designId);
			if (design != null)
			{
				GV.CounterSketch.Studio.Model.Entities.CounterSketchLibrary.DesignAttribute designAttribute = design.DesignAttributes.FirstOrDefault((GV.CounterSketch.Studio.Model.Entities.CounterSketchLibrary.DesignAttribute da) => da.AttributeId == 377);
				if (designAttribute != null)
				{
					int result2 = 0;
					if (int.TryParse(designAttribute.AttributeValue, out result2))
					{
						result = result2;
						return result;
					}
				}
			}
		}
		return result;
	}

	public static Guid GetCenterGemControlGuid(int designId)
	{
		Guid result = default(Guid);
		using (CounterSketchLibraryDataEntities1 counterSketchLibraryDataEntities = new CounterSketchLibraryDataEntities1(CSLibraryDAL.NameOfConnectionString))
		{
			Design design = counterSketchLibraryDataEntities.Designs.FirstOrDefault((Design _) => _.DesignId == (long)designId);
			if (design != null)
			{
				GV.CounterSketch.Studio.Model.Entities.CounterSketchLibrary.DesignAttribute designAttribute = design.DesignAttributes.FirstOrDefault((GV.CounterSketch.Studio.Model.Entities.CounterSketchLibrary.DesignAttribute da) => da.AttributeId == 378);
				if (designAttribute != null)
				{
					result = new Guid(designAttribute.AttributeValue);
					return result;
				}
			}
		}
		return result;
	}

	public static ObservableCollection<SearchFilterItem> GetDesignBySearchFilterItems()
	{
		using CounterSketchLibraryDataEntities1 counterSketchLibraryDataEntities = new CounterSketchLibraryDataEntities1(CSLibraryDAL.NameOfConnectionString);
		List<string> list = null;
		list = counterSketchLibraryDataEntities.Designs.Select((Design design) => design.Designer).Distinct().ToList();
		List<SearchFilterItem> list2 = new List<SearchFilterItem>();
		list2.AddRange(list.Select((string filterItem) => new SearchFilterItem
		{
			PrimaryText = filterItem,
			SearchAttributeId = -1,
			IsReadOnly = true,
			IsDesignedByFilter = true
		}));
		List<SearchFilterItem> list3 = list2.OrderBy((SearchFilterItem filterItem) => filterItem.PrimaryText).ToList();
		list3.Add(new SearchFilterItem
		{
			PrimaryText = TranslationHelper.GetTranslatedString("Show All"),
			SecondaryText = " ",
			IsReadOnly = true,
			SearchAttributeId = -1,
			IsDesignedByFilter = true
		});
		return new ObservableCollection<SearchFilterItem>(list3);
	}

	public static DesignSearch GetDesignBySPINumber(string spiNumber)
	{
		using CounterSketchLibraryDataEntities1 counterSketchLibraryDataEntities = new CounterSketchLibraryDataEntities1(CSLibraryDAL.NameOfConnectionString);
		return counterSketchLibraryDataEntities.DesignSearches.Where((DesignSearch d) => d.SPINumber.Contains(spiNumber)).FirstOrDefault();
	}

	public static List<DesignSearch> GetDesignsByCollectionId(int collectionId, bool includeVariants = false)
	{
		using CounterSketchLibraryDataEntities1 counterSketchLibraryDataEntities = new CounterSketchLibraryDataEntities1(CSLibraryDAL.NameOfConnectionString);
		return new List<DesignSearch>(counterSketchLibraryDataEntities.DesignSearches.Where((DesignSearch design) => design.ComponentId == (long?)1L && design.Design.DesignAttributes.Any((GV.CounterSketch.Studio.Model.Entities.CounterSketchLibrary.DesignAttribute da) => da.AttributeId == (long?)(long)collectionId) && (includeVariants ? true : (design.IsBaseDesign == (long?)1L || design.IsIncludedWithBaseDesigns == (long?)1L))));
	}

	public static DesignSearch GetDesignSearchByDesignId(int designId)
	{
		using CounterSketchLibraryDataEntities1 counterSketchLibraryDataEntities = new CounterSketchLibraryDataEntities1(CSLibraryDAL.NameOfConnectionString);
		return counterSketchLibraryDataEntities.DesignSearches.FirstOrDefault((DesignSearch design) => design.DesignId == (long)designId);
	}

	public static List<DesignSearch> GetDesignsForKeyword(string keyword)
	{
		keyword = keyword.ToLowerInvariant().Replace("2 stone", "2stone").Replace("3 stone", "3stone")
			.Replace("5 stone", "5stone")
			.Replace("7 stone", "7stone");
		int result = 0;
		if (int.TryParse(keyword, out result))
		{
			int value = 0;
			_everAndEverBandToSeriesCrossReference.TryGetValue(result, out value);
			if (value != 0)
			{
				keyword = value.ToString();
			}
		}
		string[] keywords = keyword.ToLower().Trim().Split(' ');
		CounterSketchLibraryDataEntities1 context = new CounterSketchLibraryDataEntities1(CSLibraryDAL.NameOfConnectionString);
		try
		{
			if (!_isDonePrecachingKeywords)
			{
				int num = 0;
				while (!_isDonePrecachingKeywords)
				{
					Thread.Sleep(1000);
					if (num++ > 10)
					{
						break;
					}
				}
			}
			IEnumerable<DesignSearch> collection = ((!keyword.Contains("id:")) ? (from pair in DesignKeywords
				where keywords.All((string word) => pair.Value.Contains(word) || pair.Value.Contains(word.TrimEnd('s')))
				select context.DesignSearches.FirstOrDefault((DesignSearch design) => design.DesignId == (long)((KeyValuePair<int, string>)pair).Key)) : context.DesignSearches.Where((DesignSearch design) => design.DesignId.ToString() == keyword.Replace("id:", string.Empty)));
			return new List<DesignSearch>(collection);
		}
		finally
		{
			if (context != null)
			{
				((IDisposable)context).Dispose();
			}
		}
	}

	public static List<DesignSearch> GetDesignsForPackageName(string selectedPackageName)
	{
		using CounterSketchLibraryDataEntities1 counterSketchLibraryDataEntities = new CounterSketchLibraryDataEntities1(CSLibraryDAL.NameOfConnectionString);
		return new List<DesignSearch>(from design in counterSketchLibraryDataEntities.Designs
			where design.DesignAttributes.Any((GV.CounterSketch.Studio.Model.Entities.CounterSketchLibrary.DesignAttribute designAttribute) => designAttribute.Attribute.AttributeTypeId == 30 && designAttribute.Attribute.AttributeName == selectedPackageName)
			select design.DesignSearch);
	}

	public static List<DesignSearch> GetEverAndEverDesigns()
	{
		using CounterSketchLibraryDataEntities1 counterSketchLibraryDataEntities = new CounterSketchLibraryDataEntities1(CSLibraryDAL.NameOfConnectionString);
		IQueryable<long> designs = from d in counterSketchLibraryDataEntities.Designs
			where d.DesignAttributes.Any((GV.CounterSketch.Studio.Model.Entities.CounterSketchLibrary.DesignAttribute a) => a.AttributeId == (long?)376L)
			select d.DesignId;
		return new List<DesignSearch>(counterSketchLibraryDataEntities.DesignSearches.Where((DesignSearch design) => design.ComponentId == (long?)1L && (design.IsBaseDesign == (long?)1L || design.IsIncludedWithBaseDesigns == (long?)1L) && designs.Contains(design.DesignId)));
	}

	public static string GetGemShapeFromGemTableShapeId(long id)
	{
		long num = id - 1;
		if ((ulong)num <= 28uL)
		{
			switch ((int)num)
			{
			case 0:
				return "Baguette";
			case 28:
				return "Baguette Tappered";
			case 2:
				return "Calf";
			case 3:
			case 4:
				return "Square Cushion";
			case 5:
				return "Cushion";
			case 6:
			case 7:
				return "Asscher";
			case 8:
				return "Emerald";
			case 9:
				return "Half Moon";
			case 10:
				return "Heart";
			case 11:
			case 12:
				return "Marquise";
			case 13:
				return "Octagon";
			case 14:
			case 15:
				return "Oval";
			case 16:
				return "Pear";
			case 17:
				return "Princess";
			case 18:
			case 19:
			case 21:
				return "Radiant";
			case 20:
				return "Round";
			case 22:
			case 23:
			case 24:
			case 25:
			case 26:
			case 27:
				return "Trillion";
			}
		}
		return "Round";
	}

	public static List<DesignSearch> GetMensDesigns(bool includeVariants = false)
	{
		using CounterSketchLibraryDataEntities1 counterSketchLibraryDataEntities = new CounterSketchLibraryDataEntities1(CSLibraryDAL.NameOfConnectionString);
		IQueryable<long> designs = from d in counterSketchLibraryDataEntities.Designs
			where d.DesignAttributes.Any((GV.CounterSketch.Studio.Model.Entities.CounterSketchLibrary.DesignAttribute a) => a.AttributeId == (long?)321L || a.AttributeId == (long?)18L)
			select d.DesignId;
		return new List<DesignSearch>(counterSketchLibraryDataEntities.DesignSearches.Where((DesignSearch design) => design.ComponentId == (long?)1L && (includeVariants ? true : (design.IsBaseDesign == (long?)1L || design.IsIncludedWithBaseDesigns == (long?)1L)) && designs.Contains(design.DesignId)));
	}

	public static List<DesignSearch> GetNewDesigns(bool includeVariants = false, bool isFreehand = false)
	{
		try
		{
			using CounterSketchLibraryDataEntities1 counterSketchLibraryDataEntities = new CounterSketchLibraryDataEntities1(CSLibraryDAL.NameOfConnectionString);
			if (isFreehand)
			{
				return new List<DesignSearch>(counterSketchLibraryDataEntities.DesignSearches.Where((DesignSearch design) => design.IsNewDesign == (long?)1L && (design.IsFreeHandPart == (long?)1L || design.IsFreeHandPartWithGem == (long?)1L || design.IsStaticFreeHandPart == (long?)1L)));
			}
			return new List<DesignSearch>(counterSketchLibraryDataEntities.DesignSearches.Where((DesignSearch design) => design.IsNewDesign == (long?)1L && design.IsFreeHandPart != (long?)1L && design.IsFreeHandPartWithGem != (long?)1L && design.IsStaticFreeHandPart != (long?)1L));
		}
		catch (Exception)
		{
		}
		return new List<DesignSearch>();
	}

	public static Collection<GV.CounterSketch.Studio.Model.Entities.CounterSketchLibrary.Attribute> GetPackageAttributes()
	{
		CounterSketchLibraryDataEntities1 context = new CounterSketchLibraryDataEntities1(CSLibraryDAL.NameOfConnectionString);
		try
		{
			Collection<GV.CounterSketch.Studio.Model.Entities.CounterSketchLibrary.Attribute> collection = new Collection<GV.CounterSketch.Studio.Model.Entities.CounterSketchLibrary.Attribute>((from a in context.Attributes
				where a.AttributeTypeId == 30 && context.DesignAttributes.Any((GV.CounterSketch.Studio.Model.Entities.CounterSketchLibrary.DesignAttribute da) => da.AttributeId == (long?)a.AttributeId)
				orderby a.AttributeCSOrder
				select a).ToList());
			collection.Insert(0, new GV.CounterSketch.Studio.Model.Entities.CounterSketchLibrary.Attribute
			{
				AttributeId = 0L,
				AttributeCSOrder = 0L,
				AttributeName = "None"
			});
			return collection;
		}
		finally
		{
			if (context != null)
			{
				((IDisposable)context).Dispose();
			}
		}
	}

	public static List<DesignSearch> GetPackagedDesigns(bool includeVariants = false)
	{
		using CounterSketchLibraryDataEntities1 counterSketchLibraryDataEntities = new CounterSketchLibraryDataEntities1(CSLibraryDAL.NameOfConnectionString);
		return new List<DesignSearch>(counterSketchLibraryDataEntities.DesignSearches.Where((DesignSearch design) => design.ComponentId == (long?)1L && design.Design.DesignAttributes.Any((GV.CounterSketch.Studio.Model.Entities.CounterSketchLibrary.DesignAttribute da) => da.Attribute.AttributeTypeId == 30) && (includeVariants || design.IsBaseDesign == (long?)1L || design.IsIncludedWithBaseDesigns == (long?)1L)));
	}

	public static string GetPackagesForDesignSearch(DesignSearch selectedDesignSearch)
	{
		using CounterSketchLibraryDataEntities1 counterSketchLibraryDataEntities = new CounterSketchLibraryDataEntities1(CSLibraryDAL.NameOfConnectionString);
		IQueryable<string> values = from p in counterSketchLibraryDataEntities.DesignAttributes
			where p.DesignId == selectedDesignSearch.DesignId && p.Attribute.AttributeTypeId == 30
			select p.Attribute.AttributeName;
		return string.Join(", ", values);
	}

	public static List<DesignSearch> GetPendantDesigns()
	{
		using CounterSketchLibraryDataEntities1 counterSketchLibraryDataEntities = new CounterSketchLibraryDataEntities1(CSLibraryDAL.NameOfConnectionString);
		return new List<DesignSearch>(counterSketchLibraryDataEntities.DesignSearches.Where((DesignSearch design) => design.ComponentId == (long?)4L));
	}

	public static ObservableCollection<SearchFilterItem> GetSearchFilterItems(int attributeTypeId, bool? generatePendantFilters, bool freehandOnlyFilters = false)
	{
		using CounterSketchLibraryDataEntities1 counterSketchLibraryDataEntities = new CounterSketchLibraryDataEntities1(CSLibraryDAL.NameOfConnectionString);
		List<SearchFilterItem> searchFilterItems = new List<SearchFilterItem>();
		List<GV.CounterSketch.Studio.Model.Entities.CounterSketchLibrary.Attribute> list = new List<GV.CounterSketch.Studio.Model.Entities.CounterSketchLibrary.Attribute>();
		list = ((generatePendantFilters == true) ? counterSketchLibraryDataEntities.Attributes.Where((GV.CounterSketch.Studio.Model.Entities.CounterSketchLibrary.Attribute attribute) => attribute.AttributeTypeId == (long)attributeTypeId && !attribute.AttributeName.StartsWith("FH-") && (bool?)attribute.AttributeName.ToLower().Contains("pendant") == generatePendantFilters).ToList() : ((!freehandOnlyFilters) ? counterSketchLibraryDataEntities.Attributes.Where((GV.CounterSketch.Studio.Model.Entities.CounterSketchLibrary.Attribute attribute) => attribute.AttributeTypeId == (long)attributeTypeId && !attribute.AttributeName.StartsWith("FH-")).ToList() : counterSketchLibraryDataEntities.Attributes.Where((GV.CounterSketch.Studio.Model.Entities.CounterSketchLibrary.Attribute attribute) => attribute.AttributeTypeId == (long)attributeTypeId && attribute.AttributeName.StartsWith("FH-")).ToList()));
		list.ForEach(delegate(GV.CounterSketch.Studio.Model.Entities.CounterSketchLibrary.Attribute attribute)
		{
			AddSearchFilterItem(attribute, searchFilterItems);
		});
		IOrderedEnumerable<SearchFilterItem> orderedEnumerable = null;
		orderedEnumerable = ((attributeTypeId != 13) ? searchFilterItems.OrderBy((SearchFilterItem filterItem) => filterItem.CSOrder) : searchFilterItems.OrderBy((SearchFilterItem filterItem) => filterItem.PrimaryText));
		searchFilterItems = orderedEnumerable.ToList();
		searchFilterItems.Add(new SearchFilterItem
		{
			PrimaryText = TranslationHelper.GetTranslatedString("Show All"),
			SecondaryText = " ",
			Description = TranslationHelper.GetTranslatedString("NoSearchFilterApplied"),
			ImagePath = GetSearchFilterItemImagePath(attributeTypeId),
			IsReadOnly = true,
			SearchAttributeId = -1
		});
		return new ObservableCollection<SearchFilterItem>(searchFilterItems);
	}

	public static List<DesignSearch> GetThreeStoneDesigns(bool includeVariants = false)
	{
		List<DesignSearch> allBaseRingDesigns = GetAllBaseRingDesigns();
		IEnumerable<SearchFilterItem> enumerable = from x in GetSearchFilterItems(13, false)
			where x.SearchAttributeId == 48
			select x;
		return UpdateDesignResultsWithFilters(enumerable, allBaseRingDesigns.Select((DesignSearch result) => result.DesignId).ToList(), enumerable.FirstOrDefault()).ToList();
	}

	public static void UpdateDesignKeywordsDictionary()
	{
		HashSet<string> keywordHashSet = new HashSet<string>();
		Stopwatch stopwatch = new Stopwatch();
		stopwatch.Start();
		using (CounterSketchLibraryDataEntities1 counterSketchLibraryDataEntities = new CounterSketchLibraryDataEntities1(CSLibraryDAL.NameOfConnectionString))
		{
			try
			{
				DesignKeywords = new Dictionary<int, string>();
				counterSketchLibraryDataEntities.Designs.ToList().ForEach(delegate(Design design)
				{
					List<string> list = (from designAttribute in design.DesignAttributes
						where designAttribute.Attribute != null && (designAttribute.Attribute.AttributeTypeId == 5 || designAttribute.Attribute.AttributeTypeId == 7 || designAttribute.Attribute.AttributeTypeId == 9 || designAttribute.Attribute.AttributeTypeId == 10 || designAttribute.Attribute.AttributeTypeId == 11 || designAttribute.Attribute.AttributeTypeId == 12 || designAttribute.Attribute.AttributeTypeId == 13 || designAttribute.Attribute.AttributeTypeId == 14 || designAttribute.Attribute.AttributeTypeId == 18 || designAttribute.Attribute.AttributeTypeId == 30 || designAttribute.Attribute.AttributeTypeId == 34)
						select (designAttribute.Attribute.AttributeId != 366) ? designAttribute.Attribute.AttributeName : designAttribute.AttributeValue).ToList();
					list.ForEach(delegate(string _)
					{
						if (_.StartsWith("fh-", ignoreCase: true, CultureInfo.InvariantCulture))
						{
							_ = _.Substring(3, _.Length - 3);
						}
						keywordHashSet.Add(_);
					});
					string text = string.Join(" ", list);
					DesignKeywords.Add((int)design.DesignId, text.Replace("2 stone", "2stone").Replace("3 stone", "3stone").Replace("5 stone", "5stone")
						.Replace("7 stone", "7stone")
						.ToLower() + " " + design.Designer.ToLower() + " " + design.StullerNo.ToLower() + " " + design.SeriesNumber.ToLower());
				});
				SearchKeywords.AddRange(keywordHashSet.ToList());
			}
			catch (Exception ex)
			{
				GlobalErrorHandler.WriteErrorToEventLog("Error Creating Keyword Cache\n" + ex);
				_isDonePrecachingKeywords = false;
				return;
			}
			_isDonePrecachingKeywords = true;
		}
		stopwatch.Stop();
	}

	public static ObservableCollection<DesignSearch> UpdateDesignResultsWithFilters(IEnumerable<SearchFilterItem> activeFilterItems, List<long> currentResultIds, SearchFilterItem searchFilterItem, int? centerShapeAttribute = null, double? desiredHeight = null, double? desiredWidth = null)
	{
		if (activeFilterItems != null)
		{
			List<int> ungroupedActiveFilterItems = (from item in activeFilterItems
				where item.GroupedAttributes == null && !item.IsDesignedByFilter
				select item.SearchAttributeId).ToList();
			List<IEnumerable<long>> groupedActiveFilterItems = (from item in activeFilterItems
				where item.GroupedAttributes != null && !item.IsDesignedByFilter
				select from subItem in item.GroupedAttributes
					select subItem.AttributeId).ToList();
			List<string> selectedActiveFilterItems = ((searchFilterItem.GroupedAttributes != null) ? searchFilterItem.GroupedAttributes.Select((GV.CounterSketch.Studio.Model.Entities.CounterSketchLibrary.Attribute item) => item.AttributeName).ToList() : new List<string> { (!searchFilterItem.IsDesignedByFilter) ? (searchFilterItem.PrimaryText + " " + searchFilterItem.SecondaryText) : string.Empty });
			using CounterSketchLibraryDataEntities1 counterSketchLibraryDataEntities = new CounterSketchLibraryDataEntities1(CSLibraryDAL.NameOfConnectionString);
			Stopwatch.StartNew();
			IQueryable<Design> source = counterSketchLibraryDataEntities.Designs.Where((Design design) => currentResultIds.Contains(design.DesignId));
			IEnumerable<DesignSearch> enumerable = from design in source
				where ungroupedActiveFilterItems.All((int filterItem) => (from designAttribute in design.DesignAttributes
					where designAttribute.ComponentId == (long?)0L || designAttribute.ComponentId == (long?)1L || designAttribute.ComponentId == (long?)4L
					select designAttribute.AttributeId).Contains((long)filterItem))
				select design.DesignSearch;
			IEnumerable<DesignSearch> enumerable2 = null;
			if (groupedActiveFilterItems.Any())
			{
				IEnumerable<DesignSearch> enumerable3 = null;
				enumerable3 = ((groupedActiveFilterItems.Count <= 1) ? (from design in source.ToList()
					where groupedActiveFilterItems.All((IEnumerable<long> groupedFilterItem) => (from designAttribute in design.DesignAttributes
						where designAttribute.ComponentId == 0 || designAttribute.ComponentId == 1 || designAttribute.ComponentId == 4
						select designAttribute.AttributeId).Intersect(groupedFilterItem.Select((Func<long, long?>)((long item) => item))).Any())
					select design.DesignSearch) : (from design in source.ToList()
					where groupedActiveFilterItems.All((IEnumerable<long> groupedFilterItem) => (from designAttribute in design.DesignAttributes
						where designAttribute.ComponentId == 0 || designAttribute.ComponentId == 1 || designAttribute.ComponentId == 4
						select designAttribute.AttributeId).Intersect(groupedFilterItem.Select((Func<long, long?>)((long item) => item))).Any())
					select design.DesignSearch));
				enumerable2 = from design in enumerable.Intersect(enumerable3)
					orderby selectedActiveFilterItems.Contains(design.DefaultStyleElement) descending, design.DefaultStyleElementOrder, design.DefaultStyleOrder, design.DefaultSettingTypeOrder, design.SecondaryStyleElementOrder, design.SPINumber
					select design;
			}
			else
			{
				enumerable2 = from design in enumerable
					orderby selectedActiveFilterItems.Contains(design.DefaultStyleElement) descending, design.DefaultStyleElementOrder, design.DefaultStyleOrder, design.DefaultSettingTypeOrder, design.SecondaryStyleElementOrder, design.SPINumber
					select design;
			}
			if ((desiredHeight.GetValueOrDefault() > 0.0 || desiredWidth.GetValueOrDefault() > 0.0) && centerShapeAttribute.HasValue)
			{
				enumerable2 = UpdateResultsUsingCenterGemSize(enumerable2, centerShapeAttribute.Value, desiredHeight.Value, desiredWidth.Value);
			}
			return new ObservableCollection<DesignSearch>(enumerable2);
		}
		return new ObservableCollection<DesignSearch>();
	}

	public static IEnumerable<DesignSearch> UpdateResultsUsingCenterGemSize(IEnumerable<DesignSearch> filteredResults, int centerShapeAttribute, double desiredHeight, double desiredWidth)
	{
		decimal heightMin = (decimal)(desiredHeight * 0.9);
		decimal heightMax = (decimal)(desiredHeight * 1.1);
		decimal widthMin = (decimal)(desiredWidth * 0.9);
		decimal widthMax = (decimal)(desiredWidth * 1.1);
		List<int> centerShapeIds = ((centerShapeAttribute != -1) ? _centerShapeAttributeToGemTableShapeIdLookup[centerShapeAttribute] : new List<int>());
		return filteredResults.Where((DesignSearch _) => _.Design.DesignGemTables.Any((DesignGemTable dgt) => (centerShapeIds.Contains((int)dgt.GemTableShapeSize.GemTableShapeId) || centerShapeAttribute == -1) && dgt.GemTableShapeSize.Height <= heightMax && dgt.GemTableShapeSize.Height >= heightMin && dgt.GemTableShapeSize.Width <= widthMax && dgt.GemTableShapeSize.Width >= widthMin));
	}

	private static void AddSearchFilterItem(GV.CounterSketch.Studio.Model.Entities.CounterSketchLibrary.Attribute attribute, List<SearchFilterItem> searchFilterItems)
	{
		string empty = string.Empty;
		string primaryText = string.Empty;
		string secondaryText = string.Empty;
		SearchFilterItem searchFilterItem = null;
		bool flag = false;
		long attributeId = attribute.AttributeId;
		if (attributeId <= 206)
		{
			if (attributeId <= 54)
			{
				if (attributeId <= 46)
				{
					if (attributeId == 38)
					{
						empty = "Prong";
						primaryText = TranslationHelper.GetTranslatedString(empty);
						searchFilterItem = searchFilterItems.FirstOrDefault((SearchFilterItem filterItem) => filterItem.PrimaryText == primaryText && filterItem.SecondaryText == secondaryText);
						if (searchFilterItem != null)
						{
							searchFilterItem.GroupedAttributes.Add(attribute);
						}
						else
						{
							searchFilterItem = new SearchFilterItem
							{
								PrimaryText = primaryText,
								SecondaryText = secondaryText,
								SearchAttributeId = 13000,
								ImagePath = GetSearchFilterItemImagePath(attribute, empty),
								Description = GetFilterItemDescription(empty, isGemSpecific: true),
								IsReadOnly = true,
								CSOrder = attribute.AttributeCSOrder,
								GroupedAttributes = new List<GV.CounterSketch.Studio.Model.Entities.CounterSketchLibrary.Attribute> { attribute }
							};
							flag = true;
						}
						goto IL_126a;
					}
					if (attributeId == 46)
					{
						goto IL_043b;
					}
				}
				else if (attributeId == 50 || attributeId == 54)
				{
					goto IL_0acb;
				}
			}
			else if (attributeId <= 68)
			{
				if (attributeId == 66)
				{
					goto IL_0783;
				}
				if (attributeId == 68)
				{
					goto IL_0d6b;
				}
			}
			else
			{
				long num = attributeId - 73;
				if ((ulong)num <= 7uL)
				{
					switch ((int)num)
					{
					case 6:
						goto IL_0783;
					case 7:
						goto IL_082b;
					case 3:
						goto IL_0c1b;
					case 0:
						goto IL_0cc3;
					case 1:
						goto IL_0d6b;
					case 2:
					case 4:
					case 5:
						goto IL_115b;
					}
				}
				long num2 = attributeId - 163;
				if ((ulong)num2 <= 6uL)
				{
					switch ((int)num2)
					{
					case 1:
						goto IL_0783;
					case 2:
					case 5:
						goto IL_08d3;
					case 6:
						goto IL_0e13;
					case 0:
						goto IL_0ebb;
					case 3:
					case 4:
						goto IL_115b;
					}
				}
				long num3 = attributeId - 190;
				if ((ulong)num3 <= 16uL)
				{
					switch ((int)num3)
					{
					case 0:
						break;
					case 9:
						goto IL_0633;
					case 2:
					case 3:
						goto IL_06db;
					case 6:
						goto IL_0783;
					case 11:
					case 13:
					case 14:
						goto IL_08d3;
					case 4:
						goto IL_0c1b;
					case 7:
						goto IL_0cc3;
					case 12:
						goto IL_0d6b;
					case 10:
					case 15:
					case 16:
						goto IL_0e13;
					case 1:
						goto IL_0f63;
					default:
						goto IL_115b;
					}
					goto IL_058b;
				}
			}
		}
		else if (attributeId <= 325)
		{
			if (attributeId <= 291)
			{
				long num4 = attributeId - 252;
				if ((ulong)num4 <= 12uL)
				{
					switch ((int)num4)
					{
					case 0:
						goto IL_06db;
					case 8:
					case 12:
						goto IL_097b;
					case 2:
					case 3:
					case 4:
					case 5:
					case 9:
						goto IL_0a23;
					case 6:
						goto IL_0f63;
					case 1:
					case 7:
					case 10:
					case 11:
						goto IL_115b;
					}
				}
				long num5 = attributeId - 289;
				if ((ulong)num5 <= 2uL)
				{
					switch ((int)num5)
					{
					case 1:
						break;
					case 0:
						goto IL_082b;
					case 2:
						goto IL_0a23;
					default:
						goto IL_115b;
					}
					empty = "Simple";
					primaryText = TranslationHelper.GetTranslatedString(empty);
					searchFilterItem = searchFilterItems.FirstOrDefault((SearchFilterItem filterItem) => filterItem.PrimaryText == primaryText && filterItem.SecondaryText == secondaryText);
					if (searchFilterItem != null)
					{
						searchFilterItem.GroupedAttributes.Add(attribute);
					}
					else
					{
						searchFilterItem = new SearchFilterItem
						{
							PrimaryText = primaryText,
							SecondaryText = secondaryText,
							SearchAttributeId = 13002,
							ImagePath = GetSearchFilterItemImagePath(attribute, empty),
							Description = GetFilterItemDescription(empty),
							IsReadOnly = true,
							CSOrder = attribute.AttributeCSOrder,
							GroupedAttributes = new List<GV.CounterSketch.Studio.Model.Entities.CounterSketchLibrary.Attribute> { attribute }
						};
						flag = true;
					}
					goto IL_126a;
				}
			}
			else
			{
				if (attributeId == 299)
				{
					goto IL_0633;
				}
				long num6 = attributeId - 316;
				if ((ulong)num6 <= 9uL)
				{
					switch ((int)num6)
					{
					case 9:
						break;
					case 7:
						goto IL_0633;
					case 0:
					case 5:
						goto IL_0acb;
					case 6:
					case 8:
						goto IL_0b73;
					default:
						goto IL_115b;
					}
					goto IL_043b;
				}
			}
		}
		else if (attributeId <= 343)
		{
			if (attributeId == 331)
			{
				goto IL_058b;
			}
			if ((ulong)(attributeId - 342) <= 1uL)
			{
				goto IL_0633;
			}
		}
		else
		{
			if (attributeId == 344)
			{
				goto IL_082b;
			}
			if (attributeId == 395)
			{
				goto IL_0a23;
			}
			long num7 = attributeId - 423;
			if ((ulong)num7 <= 56uL)
			{
				switch ((int)num7)
				{
				case 15:
				case 16:
				case 17:
				case 18:
				case 19:
				case 20:
				case 21:
				case 52:
					break;
				case 54:
					goto IL_0a23;
				case 0:
				case 23:
				case 28:
				case 37:
				case 38:
				case 40:
				case 41:
				case 53:
				case 56:
					goto IL_0ebb;
				case 31:
				case 32:
					goto IL_0f63;
				case 33:
				case 34:
				case 35:
				case 44:
				case 45:
				case 46:
				case 47:
				case 48:
					goto IL_100b;
				case 22:
				case 24:
					goto IL_10b3;
				default:
					goto IL_115b;
				}
				goto IL_097b;
			}
		}
		goto IL_115b;
		IL_0c1b:
		empty = "Weave";
		primaryText = TranslationHelper.GetTranslatedString(empty);
		searchFilterItem = searchFilterItems.FirstOrDefault((SearchFilterItem filterItem) => filterItem.PrimaryText == primaryText && filterItem.SecondaryText == secondaryText);
		if (searchFilterItem != null)
		{
			searchFilterItem.GroupedAttributes.Add(attribute);
		}
		else
		{
			searchFilterItem = new SearchFilterItem
			{
				PrimaryText = primaryText,
				SecondaryText = secondaryText,
				SearchAttributeId = 13013,
				ImagePath = GetSearchFilterItemImagePath(attribute, empty),
				Description = GetFilterItemDescription(empty),
				IsReadOnly = true,
				CSOrder = attribute.AttributeCSOrder,
				GroupedAttributes = new List<GV.CounterSketch.Studio.Model.Entities.CounterSketchLibrary.Attribute> { attribute }
			};
			flag = true;
		}
		goto IL_126a;
		IL_0acb:
		empty = "Bands";
		primaryText = TranslationHelper.GetTranslatedString(empty);
		searchFilterItem = searchFilterItems.FirstOrDefault((SearchFilterItem filterItem) => filterItem.PrimaryText == primaryText && filterItem.SecondaryText == secondaryText);
		if (searchFilterItem != null)
		{
			searchFilterItem.GroupedAttributes.Add(attribute);
		}
		else
		{
			searchFilterItem = new SearchFilterItem
			{
				PrimaryText = primaryText,
				SecondaryText = secondaryText,
				SearchAttributeId = 13011,
				ImagePath = GetSearchFilterItemImagePath(attribute, empty),
				Description = GetFilterItemDescription(empty),
				IsReadOnly = true,
				CSOrder = attribute.AttributeCSOrder,
				GroupedAttributes = new List<GV.CounterSketch.Studio.Model.Entities.CounterSketchLibrary.Attribute> { attribute }
			};
			flag = true;
		}
		goto IL_126a;
		IL_0ebb:
		empty = "Setting";
		primaryText = TranslationHelper.GetTranslatedString(empty);
		searchFilterItem = searchFilterItems.FirstOrDefault((SearchFilterItem filterItem) => filterItem.PrimaryText == primaryText && filterItem.SecondaryText == secondaryText);
		if (searchFilterItem != null)
		{
			searchFilterItem.GroupedAttributes.Add(attribute);
		}
		else
		{
			searchFilterItem = new SearchFilterItem
			{
				PrimaryText = primaryText,
				SecondaryText = secondaryText,
				SearchAttributeId = 13017,
				ImagePath = GetSearchFilterItemImagePath(attribute, empty),
				Description = GetFilterItemDescription(empty),
				IsReadOnly = true,
				CSOrder = attribute.AttributeCSOrder,
				GroupedAttributes = new List<GV.CounterSketch.Studio.Model.Entities.CounterSketchLibrary.Attribute> { attribute }
			};
			flag = true;
		}
		goto IL_126a;
		IL_0e13:
		empty = "Organic";
		primaryText = TranslationHelper.GetTranslatedString(empty);
		searchFilterItem = searchFilterItems.FirstOrDefault((SearchFilterItem filterItem) => filterItem.PrimaryText == primaryText && filterItem.SecondaryText == secondaryText);
		if (searchFilterItem != null)
		{
			searchFilterItem.GroupedAttributes.Add(attribute);
		}
		else
		{
			searchFilterItem = new SearchFilterItem
			{
				PrimaryText = primaryText,
				SecondaryText = secondaryText,
				SearchAttributeId = 13016,
				ImagePath = GetSearchFilterItemImagePath(attribute, empty),
				Description = GetFilterItemDescription(empty),
				IsReadOnly = true,
				CSOrder = attribute.AttributeCSOrder,
				GroupedAttributes = new List<GV.CounterSketch.Studio.Model.Entities.CounterSketchLibrary.Attribute> { attribute }
			};
			flag = true;
		}
		goto IL_126a;
		IL_0d6b:
		empty = "Scroll & Swirl";
		primaryText = TranslationHelper.GetTranslatedString(empty);
		searchFilterItem = searchFilterItems.FirstOrDefault((SearchFilterItem filterItem) => filterItem.PrimaryText == primaryText && filterItem.SecondaryText == secondaryText);
		if (searchFilterItem != null)
		{
			searchFilterItem.GroupedAttributes.Add(attribute);
		}
		else
		{
			searchFilterItem = new SearchFilterItem
			{
				PrimaryText = primaryText,
				SecondaryText = secondaryText,
				SearchAttributeId = 13015,
				ImagePath = GetSearchFilterItemImagePath(attribute, empty),
				Description = GetFilterItemDescription(empty),
				IsReadOnly = true,
				CSOrder = attribute.AttributeCSOrder,
				GroupedAttributes = new List<GV.CounterSketch.Studio.Model.Entities.CounterSketchLibrary.Attribute> { attribute }
			};
			flag = true;
		}
		goto IL_126a;
		IL_0a23:
		empty = "Symbolic";
		primaryText = TranslationHelper.GetTranslatedString(empty);
		searchFilterItem = searchFilterItems.FirstOrDefault((SearchFilterItem filterItem) => filterItem.PrimaryText == primaryText && filterItem.SecondaryText == secondaryText);
		if (searchFilterItem != null)
		{
			searchFilterItem.GroupedAttributes.Add(attribute);
		}
		else
		{
			searchFilterItem = new SearchFilterItem
			{
				PrimaryText = primaryText,
				SecondaryText = secondaryText,
				SearchAttributeId = 13010,
				ImagePath = GetSearchFilterItemImagePath(attribute, empty),
				Description = GetFilterItemDescription(empty),
				IsReadOnly = true,
				CSOrder = attribute.AttributeCSOrder,
				GroupedAttributes = new List<GV.CounterSketch.Studio.Model.Entities.CounterSketchLibrary.Attribute> { attribute }
			};
			flag = true;
		}
		goto IL_126a;
		IL_10b3:
		empty = "Bail";
		primaryText = TranslationHelper.GetTranslatedString(empty);
		searchFilterItem = searchFilterItems.FirstOrDefault((SearchFilterItem filterItem) => filterItem.PrimaryText == primaryText && filterItem.SecondaryText == secondaryText);
		if (searchFilterItem != null)
		{
			searchFilterItem.GroupedAttributes.Add(attribute);
		}
		else
		{
			searchFilterItem = new SearchFilterItem
			{
				PrimaryText = primaryText,
				SecondaryText = secondaryText,
				SearchAttributeId = 13020,
				ImagePath = GetSearchFilterItemImagePath(attribute, empty),
				Description = GetFilterItemDescription(empty),
				IsReadOnly = true,
				CSOrder = attribute.AttributeCSOrder,
				GroupedAttributes = new List<GV.CounterSketch.Studio.Model.Entities.CounterSketchLibrary.Attribute> { attribute }
			};
			flag = true;
		}
		goto IL_126a;
		IL_0783:
		empty = "Pattern";
		primaryText = TranslationHelper.GetTranslatedString(empty);
		searchFilterItem = searchFilterItems.FirstOrDefault((SearchFilterItem filterItem) => filterItem.PrimaryText == primaryText && filterItem.SecondaryText == secondaryText);
		if (searchFilterItem != null)
		{
			searchFilterItem.GroupedAttributes.Add(attribute);
		}
		else
		{
			searchFilterItem = new SearchFilterItem
			{
				PrimaryText = primaryText,
				SecondaryText = secondaryText,
				SearchAttributeId = 13006,
				ImagePath = GetSearchFilterItemImagePath(attribute, empty),
				Description = GetFilterItemDescription(empty),
				IsReadOnly = true,
				CSOrder = attribute.AttributeCSOrder,
				GroupedAttributes = new List<GV.CounterSketch.Studio.Model.Entities.CounterSketchLibrary.Attribute> { attribute }
			};
			flag = true;
		}
		goto IL_126a;
		IL_058b:
		empty = "Decorative Setting";
		primaryText = TranslationHelper.GetTranslatedString(empty);
		searchFilterItem = searchFilterItems.FirstOrDefault((SearchFilterItem filterItem) => filterItem.PrimaryText == primaryText && filterItem.SecondaryText == secondaryText);
		if (searchFilterItem != null)
		{
			searchFilterItem.GroupedAttributes.Add(attribute);
		}
		else
		{
			searchFilterItem = new SearchFilterItem
			{
				PrimaryText = primaryText,
				SecondaryText = secondaryText,
				SearchAttributeId = 13003,
				ImagePath = GetSearchFilterItemImagePath(attribute, empty),
				Description = GetFilterItemDescription(empty),
				IsReadOnly = true,
				CSOrder = attribute.AttributeCSOrder,
				GroupedAttributes = new List<GV.CounterSketch.Studio.Model.Entities.CounterSketchLibrary.Attribute> { attribute }
			};
			flag = true;
		}
		goto IL_126a;
		IL_100b:
		empty = "Misc";
		primaryText = TranslationHelper.GetTranslatedString(empty);
		searchFilterItem = searchFilterItems.FirstOrDefault((SearchFilterItem filterItem) => filterItem.PrimaryText == primaryText && filterItem.SecondaryText == secondaryText);
		if (searchFilterItem != null)
		{
			searchFilterItem.GroupedAttributes.Add(attribute);
		}
		else
		{
			searchFilterItem = new SearchFilterItem
			{
				PrimaryText = primaryText,
				SecondaryText = secondaryText,
				SearchAttributeId = 13019,
				ImagePath = GetSearchFilterItemImagePath(attribute, empty),
				Description = GetFilterItemDescription(empty),
				IsReadOnly = true,
				CSOrder = attribute.AttributeCSOrder,
				GroupedAttributes = new List<GV.CounterSketch.Studio.Model.Entities.CounterSketchLibrary.Attribute> { attribute }
			};
			flag = true;
		}
		goto IL_126a;
		IL_0cc3:
		empty = "Milgrain";
		primaryText = TranslationHelper.GetTranslatedString(empty);
		searchFilterItem = searchFilterItems.FirstOrDefault((SearchFilterItem filterItem) => filterItem.PrimaryText == primaryText && filterItem.SecondaryText == secondaryText);
		if (searchFilterItem != null)
		{
			searchFilterItem.GroupedAttributes.Add(attribute);
		}
		else
		{
			searchFilterItem = new SearchFilterItem
			{
				PrimaryText = primaryText,
				SecondaryText = secondaryText,
				SearchAttributeId = 13014,
				ImagePath = GetSearchFilterItemImagePath(attribute, empty),
				Description = GetFilterItemDescription(empty),
				IsReadOnly = true,
				CSOrder = attribute.AttributeCSOrder,
				GroupedAttributes = new List<GV.CounterSketch.Studio.Model.Entities.CounterSketchLibrary.Attribute> { attribute }
			};
			flag = true;
		}
		goto IL_126a;
		IL_0f63:
		empty = "Nature";
		primaryText = TranslationHelper.GetTranslatedString(empty);
		searchFilterItem = searchFilterItems.FirstOrDefault((SearchFilterItem filterItem) => filterItem.PrimaryText == primaryText && filterItem.SecondaryText == secondaryText);
		if (searchFilterItem != null)
		{
			searchFilterItem.GroupedAttributes.Add(attribute);
		}
		else
		{
			searchFilterItem = new SearchFilterItem
			{
				PrimaryText = primaryText,
				SecondaryText = secondaryText,
				SearchAttributeId = 13018,
				ImagePath = GetSearchFilterItemImagePath(attribute, empty),
				Description = GetFilterItemDescription(empty),
				IsReadOnly = true,
				CSOrder = attribute.AttributeCSOrder,
				GroupedAttributes = new List<GV.CounterSketch.Studio.Model.Entities.CounterSketchLibrary.Attribute> { attribute }
			};
			flag = true;
		}
		goto IL_126a;
		IL_08d3:
		empty = "Sculptural";
		primaryText = TranslationHelper.GetTranslatedString(empty);
		searchFilterItem = searchFilterItems.FirstOrDefault((SearchFilterItem filterItem) => filterItem.PrimaryText == primaryText && filterItem.SecondaryText == secondaryText);
		if (searchFilterItem != null)
		{
			searchFilterItem.GroupedAttributes.Add(attribute);
		}
		else
		{
			searchFilterItem = new SearchFilterItem
			{
				PrimaryText = primaryText,
				SecondaryText = secondaryText,
				SearchAttributeId = 13008,
				ImagePath = GetSearchFilterItemImagePath(attribute, empty),
				Description = GetFilterItemDescription(empty),
				IsReadOnly = true,
				CSOrder = attribute.AttributeCSOrder,
				GroupedAttributes = new List<GV.CounterSketch.Studio.Model.Entities.CounterSketchLibrary.Attribute> { attribute }
			};
			flag = true;
		}
		goto IL_126a;
		IL_082b:
		empty = "Asymmetrical";
		primaryText = TranslationHelper.GetTranslatedString(empty);
		searchFilterItem = searchFilterItems.FirstOrDefault((SearchFilterItem filterItem) => filterItem.PrimaryText == primaryText && filterItem.SecondaryText == secondaryText);
		if (searchFilterItem != null)
		{
			searchFilterItem.GroupedAttributes.Add(attribute);
		}
		else
		{
			searchFilterItem = new SearchFilterItem
			{
				PrimaryText = primaryText,
				SecondaryText = secondaryText,
				SearchAttributeId = 13007,
				ImagePath = GetSearchFilterItemImagePath(attribute, empty),
				Description = GetFilterItemDescription(empty),
				IsReadOnly = true,
				CSOrder = attribute.AttributeCSOrder,
				GroupedAttributes = new List<GV.CounterSketch.Studio.Model.Entities.CounterSketchLibrary.Attribute> { attribute }
			};
			flag = true;
		}
		goto IL_126a;
		IL_043b:
		empty = "Prong";
		primaryText = TranslationHelper.GetTranslatedString(empty);
		searchFilterItem = searchFilterItems.FirstOrDefault((SearchFilterItem filterItem) => filterItem.PrimaryText == primaryText && filterItem.SecondaryText == secondaryText);
		if (searchFilterItem != null)
		{
			searchFilterItem.GroupedAttributes.Add(attribute);
		}
		else
		{
			searchFilterItem = new SearchFilterItem
			{
				PrimaryText = primaryText,
				SecondaryText = secondaryText,
				SearchAttributeId = 13001,
				ImagePath = GetSearchFilterItemImagePath(attribute, empty),
				Description = GetFilterItemDescription(empty, isGemSpecific: true),
				IsReadOnly = true,
				CSOrder = attribute.AttributeCSOrder,
				GroupedAttributes = new List<GV.CounterSketch.Studio.Model.Entities.CounterSketchLibrary.Attribute> { attribute }
			};
			flag = true;
		}
		goto IL_126a;
		IL_097b:
		empty = "Animals";
		primaryText = TranslationHelper.GetTranslatedString(empty);
		searchFilterItem = searchFilterItems.FirstOrDefault((SearchFilterItem filterItem) => filterItem.PrimaryText == primaryText && filterItem.SecondaryText == secondaryText);
		if (searchFilterItem != null)
		{
			searchFilterItem.GroupedAttributes.Add(attribute);
		}
		else
		{
			searchFilterItem = new SearchFilterItem
			{
				PrimaryText = primaryText,
				SecondaryText = secondaryText,
				SearchAttributeId = 13009,
				ImagePath = GetSearchFilterItemImagePath(attribute, empty),
				Description = GetFilterItemDescription(empty),
				IsReadOnly = true,
				CSOrder = attribute.AttributeCSOrder,
				GroupedAttributes = new List<GV.CounterSketch.Studio.Model.Entities.CounterSketchLibrary.Attribute> { attribute }
			};
			flag = true;
		}
		goto IL_126a;
		IL_126a:
		if (flag)
		{
			searchFilterItem.PrimaryText = searchFilterItem.PrimaryText.Replace("FH-", "");
			searchFilterItems.Add(searchFilterItem);
		}
		return;
		IL_0633:
		empty = "Side Detail";
		primaryText = TranslationHelper.GetTranslatedString(empty);
		searchFilterItem = searchFilterItems.FirstOrDefault((SearchFilterItem filterItem) => filterItem.PrimaryText == primaryText && filterItem.SecondaryText == secondaryText);
		if (searchFilterItem != null)
		{
			searchFilterItem.GroupedAttributes.Add(attribute);
		}
		else
		{
			searchFilterItem = new SearchFilterItem
			{
				PrimaryText = primaryText,
				SecondaryText = secondaryText,
				SearchAttributeId = 13004,
				ImagePath = GetSearchFilterItemImagePath(attribute, empty),
				Description = GetFilterItemDescription(empty),
				IsReadOnly = true,
				CSOrder = attribute.AttributeCSOrder,
				GroupedAttributes = new List<GV.CounterSketch.Studio.Model.Entities.CounterSketchLibrary.Attribute> { attribute }
			};
			flag = true;
		}
		goto IL_126a;
		IL_0b73:
		empty = "Fashion Bands";
		primaryText = TranslationHelper.GetTranslatedString(empty);
		searchFilterItem = searchFilterItems.FirstOrDefault((SearchFilterItem filterItem) => filterItem.PrimaryText == primaryText && filterItem.SecondaryText == secondaryText);
		if (searchFilterItem != null)
		{
			searchFilterItem.GroupedAttributes.Add(attribute);
		}
		else
		{
			searchFilterItem = new SearchFilterItem
			{
				PrimaryText = primaryText,
				SecondaryText = secondaryText,
				SearchAttributeId = 13012,
				ImagePath = GetSearchFilterItemImagePath(attribute, empty),
				Description = GetFilterItemDescription(empty),
				IsReadOnly = true,
				CSOrder = attribute.AttributeCSOrder,
				GroupedAttributes = new List<GV.CounterSketch.Studio.Model.Entities.CounterSketchLibrary.Attribute> { attribute }
			};
			flag = true;
		}
		goto IL_126a;
		IL_06db:
		empty = "Clusters";
		primaryText = TranslationHelper.GetTranslatedString(empty);
		searchFilterItem = searchFilterItems.FirstOrDefault((SearchFilterItem filterItem) => filterItem.PrimaryText == primaryText && filterItem.SecondaryText == secondaryText);
		if (searchFilterItem != null)
		{
			searchFilterItem.GroupedAttributes.Add(attribute);
		}
		else
		{
			searchFilterItem = new SearchFilterItem
			{
				PrimaryText = primaryText,
				SecondaryText = secondaryText,
				SearchAttributeId = 13005,
				ImagePath = GetSearchFilterItemImagePath(attribute, empty),
				Description = GetFilterItemDescription(empty),
				IsReadOnly = true,
				CSOrder = attribute.AttributeCSOrder,
				GroupedAttributes = new List<GV.CounterSketch.Studio.Model.Entities.CounterSketchLibrary.Attribute> { attribute }
			};
			flag = true;
		}
		goto IL_126a;
		IL_115b:
		if (attribute.AttributeId != 64 && attribute.AttributeId != 67 && attribute.AttributeId != 320 && attribute.AttributeId != 298 && attribute.AttributeId != 188)
		{
			string text = (attribute.AttributeName.Contains("Pendant") ? attribute.AttributeName.Substring(8) : attribute.AttributeName);
			bool isGemSpecific = attribute.AttributeTypeId == 7 || attribute.AttributeTypeId == 14 || attribute.AttributeTypeId == 9 || attribute.AttributeTypeId == 10;
			searchFilterItem = new SearchFilterItem
			{
				PrimaryText = TranslationHelper.GetTranslatedString(text.Replace("FH-", "")),
				SearchAttributeId = Convert.ToInt32(attribute.AttributeId),
				ImagePath = GetSearchFilterItemImagePath(attribute),
				Description = GetFilterItemDescription(attribute.AttributeName, isGemSpecific),
				IsReadOnly = true,
				CSOrder = attribute.AttributeCSOrder
			};
			flag = true;
		}
		goto IL_126a;
	}

	private static string GetFilterItemDescription(string filterItemName, bool isGemSpecific = false)
	{
		return null;
	}

	private static string GetSearchFilterItemImagePath(int attributeTypeId)
	{
		string result = string.Empty;
		switch (attributeTypeId)
		{
		case 7:
		case 14:
			result = $"pack://application:,,,/CounterSketch Studio;component/Images/Search/FilterItems/GemShapes/ShowAll_{attributeTypeId}.png";
			break;
		case 9:
		case 10:
			result = $"pack://application:,,,/CounterSketch Studio;component/Images/Search/FilterItems/GemSettings/ShowAll_{attributeTypeId}.png";
			break;
		case 12:
			result = $"pack://application:,,,/CounterSketch Studio;component/Images/Search/FilterItems/Styles/ShowAll_{attributeTypeId}.png";
			break;
		case 13:
			result = $"pack://application:,,,/CounterSketch Studio;component/Images/Search/FilterItems/DesignElements/ShowAll_{attributeTypeId}.png";
			break;
		}
		return result;
	}

	private static string GetSearchFilterItemImagePath(GV.CounterSketch.Studio.Model.Entities.CounterSketchLibrary.Attribute attribute, string groupName = null)
	{
		string result = string.Empty;
		long attributeTypeId = attribute.AttributeTypeId;
		long num = attributeTypeId - 7;
		if ((ulong)num <= 7uL)
		{
			switch ((int)num)
			{
			case 0:
			case 7:
				result = (string.IsNullOrEmpty(groupName) ? ("pack://application:,,,/CounterSketch Studio;component/Images/Search/FilterItems/GemShapes/" + (attribute.AttributeName.Contains(":") ? attribute.AttributeName.Remove(attribute.AttributeName.IndexOf(':'), 1) : attribute.AttributeName) + ".png") : ("pack://application:,,,/CounterSketch Studio;component/Images/Search/FilterItems/GemShapes/" + groupName + ".png"));
				break;
			case 2:
			case 3:
				result = (string.IsNullOrEmpty(groupName) ? ("pack://application:,,,/CounterSketch Studio;component/Images/Search/FilterItems/GemSettings/" + (attribute.AttributeName.Contains(":") ? attribute.AttributeName.Remove(attribute.AttributeName.IndexOf(':'), 1) : attribute.AttributeName) + ".png") : ("pack://application:,,,/CounterSketch Studio;component/Images/Search/FilterItems/GemSettings/" + groupName + ".png"));
				break;
			case 5:
				result = (string.IsNullOrEmpty(groupName) ? ("pack://application:,,,/CounterSketch Studio;component/Images/Search/FilterItems/Styles/" + (attribute.AttributeName.Contains(":") ? attribute.AttributeName.Remove(attribute.AttributeName.IndexOf(':'), 1) : attribute.AttributeName) + ".png") : ("pack://application:,,,/CounterSketch Studio;component/Images/Search/FilterItems/Styles/" + groupName + ".png"));
				break;
			case 6:
				result = (string.IsNullOrEmpty(groupName) ? ("pack://application:,,,/CounterSketch Studio;component/Images/Search/FilterItems/DesignElements/" + (attribute.AttributeName.Contains(":") ? attribute.AttributeName.Remove(attribute.AttributeName.IndexOf(':'), 1) : attribute.AttributeName) + ".png") : ("pack://application:,,,/CounterSketch Studio;component/Images/Search/FilterItems/DesignElements/" + groupName + ".png"));
				break;
			}
		}
		return result;
	}
}
