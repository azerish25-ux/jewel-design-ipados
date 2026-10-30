using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Linq;
using System.Xml.Serialization;
using GV.CounterSketch.Shared;
using GV.CounterSketch.Shared.Pricing;
using GV.CounterSketch.Shared.Pricing.Enums;
using GV.CounterSketch.Studio.Model.Objects;
using GV.CounterSketch.Studio.Utilities.Extensions;
using GV.CounterSketch.Studio.Utilities.Helpers;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Messaging;
using Localization;
using Stuller.Web.API.v2.Models;

namespace GV.CounterSketch.Studio.ViewModel;

[Serializable]
public class GemReportViewModel : ViewModelBase
{
	private double _caratGrandTotal;

	private double _caratGrandTotalQty;

	private double _caratTotalCenter;

	private double _caratTotalCenterQty;

	private double _caratTotalLarge;

	private double _caratTotalLargeQty;

	private double _caratTotalMelee;

	private double _caratTotalMeleeQty;

	private ObservableCollection<GemGroup> _gemGroups;

	private ObservableCollection<Gem> _gems;

	private bool _groupedStoneViewActive;

	private bool _isStonesSectionVisible;

	private bool _isZalesOrderForm;

	private ObservableCollection<Gem> _largeGems;

	private Dictionary<Tuple<GemMaterial, string>, string> _meleeQualityAssignments = new Dictionary<Tuple<GemMaterial, string>, string>();

	private ObservableCollection<GemGroup> _stoneReportGems;

	public double CaratGrandTotal
	{
		get
		{
			return _caratGrandTotal;
		}
		set
		{
			if (!value.Equals(_caratGrandTotal))
			{
				_caratGrandTotal = value;
				RaisePropertyChanged("CaratGrandTotal");
			}
		}
	}

	public double CaratGrandTotalQty
	{
		get
		{
			return _caratGrandTotalQty;
		}
		set
		{
			if (!value.Equals(_caratGrandTotalQty))
			{
				_caratGrandTotalQty = value;
				RaisePropertyChanged("CaratGrandTotalQty");
			}
		}
	}

	public double CaratTotalCenter
	{
		get
		{
			return _caratTotalCenter;
		}
		set
		{
			if (!value.Equals(_caratTotalCenter))
			{
				_caratTotalCenter = value;
				RaisePropertyChanged("CaratTotalCenter");
			}
		}
	}

	public double CaratTotalCenterQty
	{
		get
		{
			return _caratTotalCenterQty;
		}
		set
		{
			if (!value.Equals(_caratTotalCenterQty))
			{
				_caratTotalCenterQty = value;
				RaisePropertyChanged("CaratTotalCenterQty");
			}
		}
	}

	public double CaratTotalLarge
	{
		get
		{
			return _caratTotalLarge;
		}
		set
		{
			if (!value.Equals(_caratTotalLarge))
			{
				_caratTotalLarge = value;
				RaisePropertyChanged("CaratTotalLarge");
			}
		}
	}

	public double CaratTotalLargeQty
	{
		get
		{
			return _caratTotalLargeQty;
		}
		set
		{
			if (!value.Equals(_caratTotalLargeQty))
			{
				_caratTotalLargeQty = value;
				RaisePropertyChanged("CaratTotalLargeQty");
			}
		}
	}

	public double CaratTotalMelee
	{
		get
		{
			return _caratTotalMelee;
		}
		set
		{
			if (!value.Equals(_caratTotalMelee))
			{
				_caratTotalMelee = value;
				RaisePropertyChanged("CaratTotalMelee");
			}
		}
	}

	public double CaratTotalMeleeQty
	{
		get
		{
			return _caratTotalMeleeQty;
		}
		set
		{
			if (!value.Equals(_caratTotalMeleeQty))
			{
				_caratTotalMeleeQty = value;
				RaisePropertyChanged("CaratTotalMeleeQty");
			}
		}
	}

	[XmlIgnore]
	public RelayCommand<int> CommandUpdateSourceMeleeGems { get; private set; }

	[XmlIgnore]
	public RelayCommand<GemGroup> CommandGemGroupSetSourceNoStone { get; private set; }

	[XmlIgnore]
	public RelayCommand<GemGroup> CommandGemGroupSetSourceProvided { get; private set; }

	[XmlIgnore]
	public RelayCommand<GemGroup> CommandGemGroupSetSourceStuller { get; private set; }

	[XmlIgnore]
	public RelayCommand<Gem> CommandGemSetSourceNoStone { get; private set; }

	[XmlIgnore]
	public RelayCommand<Gem> CommandGemSetSourceProvided { get; private set; }

	[XmlIgnore]
	public RelayCommand<Gem> CommandGemSetSourceStuller { get; private set; }

	[XmlIgnore]
	public RelayCommand CommandHighlightCancel { get; private set; }

	[XmlIgnore]
	public RelayCommand<Gem> CommandShopForDiamond { get; private set; }

	[XmlIgnore]
	public RelayCommand<Gem> CommandShopForGemstone { get; private set; }

	[XmlIgnore]
	public RelayCommand<Gem> CommandHighlightGem { get; private set; }

	[XmlIgnore]
	public RelayCommand<GemGroup> CommandHighlightGemGroup { get; private set; }

	[XmlIgnore]
	public RelayCommand CommandOpenStullerSite { get; private set; }

	[XmlIgnore]
	public RelayCommand<Gem> CommandShopForStone { get; private set; }

	[XmlIgnore]
	public RelayCommand CommandUpdateGemGroups { get; private set; }

	[XmlIgnore]
	public RelayCommand CommandUpdateGemsFromGemGroups { get; private set; }

	public ObservableCollection<GemGroup> GemGroups
	{
		get
		{
			if (_gemGroups == null)
			{
				_gemGroups = new ObservableCollection<GemGroup>();
			}
			return _gemGroups;
		}
		set
		{
			_gemGroups = value;
			RaisePropertyChanged(() => GemGroups);
		}
	}

	public ObservableCollection<Gem> Gems
	{
		get
		{
			if (_gems == null)
			{
				_gems = new ObservableCollection<Gem>();
			}
			return _gems;
		}
		set
		{
			_gems = value;
			RaisePropertyChanged(() => Gems);
		}
	}

	public bool GroupedStoneViewActive
	{
		get
		{
			return _groupedStoneViewActive;
		}
		set
		{
			_groupedStoneViewActive = value;
			RaisePropertyChanged(() => GroupedStoneViewActive);
		}
	}

	public bool IsZalesOrderForm
	{
		get
		{
			return _isZalesOrderForm;
		}
		set
		{
			_isZalesOrderForm = value;
			RaisePropertyChanged("IsZalesOrderForm");
		}
	}

	public ObservableCollection<Gem> LargeGems
	{
		get
		{
			if (_largeGems == null)
			{
				_largeGems = new ObservableCollection<Gem>();
			}
			return _largeGems;
		}
		set
		{
			_largeGems = value;
			RaisePropertyChanged(() => LargeGems);
		}
	}

	public ObservableCollection<GemGroup> StoneReportGems
	{
		get
		{
			if (_stoneReportGems == null)
			{
				_stoneReportGems = new ObservableCollection<GemGroup>();
			}
			return _stoneReportGems;
		}
		set
		{
			_stoneReportGems = value;
			RaisePropertyChanged(() => StoneReportGems);
		}
	}

	public bool IsStonesSectionVisible
	{
		get
		{
			return _isStonesSectionVisible;
		}
		set
		{
			_isStonesSectionVisible = value;
			RaisePropertyChanged("IsStonesSectionVisible");
		}
	}

	[XmlIgnore]
	public List<Tuple<Gem, Diamond>> AssignedDiamondsList { get; set; }

	public GemReportViewModel()
	{
		AssignedDiamondsList = new List<Tuple<Gem, Diamond>>();
		if (base.IsInDesignMode)
		{
			try
			{
				GemHelper.UpdateMaterialImageCache();
				PopulateTestData();
				GroupedStoneViewActive = true;
				return;
			}
			catch (Exception)
			{
				return;
			}
		}
		InitializeCommands();
		Messenger.Default.Register<Design>(this, DesignOpenedRefresh);
	}

	public void AssignDiamondToGem(Gem gemSearchedFor, Diamond diamondAssigned)
	{
		if (!gemSearchedFor.IsCenterStone && gemSearchedFor.IsSideStone)
		{
			double heightMax = gemSearchedFor.Height * 1.1;
			double heightMin = gemSearchedFor.Height * 0.9;
			double widthMax = gemSearchedFor.Width * 1.1;
			double widthMin = gemSearchedFor.Width * 0.9;
			Tuple<Gem, Diamond> tuple = AssignedDiamondsList.FirstOrDefault((Tuple<Gem, Diamond> _) => _.Item1.IsSideStone && heightMin < _.Item1.Height && heightMax > _.Item1.Height && widthMin < _.Item1.Width && widthMax > _.Item1.Width && _.Item1.Shape == gemSearchedFor.Shape && _.Item1.GemNumber == gemSearchedFor.GemNumber);
			if (tuple != null)
			{
				AssignedDiamondsList.Remove(tuple);
			}
			if (diamondAssigned != null)
			{
				AssignedDiamondsList.Add(new Tuple<Gem, Diamond>(gemSearchedFor, diamondAssigned));
			}
		}
	}

	public ObservableCollection<string> GetGemColors(string stullerType, string stullerQuality)
	{
		if (!ViewModelBase.IsInDesignModeStatic)
		{
			return new ObservableCollection<string>(StullerGemHelper.GetAvailableStullerGemColorsByType(stullerType, stullerQuality));
		}
		return new ObservableCollection<string>();
	}

	public ObservableCollection<string> GetGemQualities(string stullerType)
	{
		if (!ViewModelBase.IsInDesignModeStatic)
		{
			return new ObservableCollection<string>(StullerGemHelper.GetAvailableStullerGemQualitiesByType(stullerType));
		}
		return new ObservableCollection<string>();
	}

	public ObservableCollection<string> GetStullerGemTypes(string requestedType)
	{
		if (!ViewModelBase.IsInDesignModeStatic && requestedType != null)
		{
			int length = requestedType.Length;
			if (length > 2)
			{
				length = 2;
			}
			return new ObservableCollection<string>(from t in StullerGemHelper.GetAvailableStullerGemTypes()
				where t.StartsWith(requestedType.Substring(0, length), ignoreCase: true, CultureInfo.InvariantCulture)
				select t);
		}
		return new ObservableCollection<string>();
	}

	public bool IsDiamondAssignedToNonCenterStone(int serialNumber)
	{
		bool result = false;
		if (AssignedDiamondsList != null)
		{
			result = AssignedDiamondsList.Any((Tuple<Gem, Diamond> _) => _.Item2 != null && _.Item2.SerialNumber == serialNumber);
		}
		return result;
	}

	public void Populate(bool updateMaterialsOnly = false)
	{
		if (updateMaterialsOnly)
		{
			UpdateGemMaterialsChange(UIHelper.CoreDataService.DocumentGetStudioGemList());
		}
		else
		{
			LoadGemData(UIHelper.CoreDataService.DocumentGetStudioGemList());
		}
		if (Gems.Count > 0)
		{
			IsStonesSectionVisible = true;
		}
		GroupedStoneViewActive = true;
	}

	public void ResetPreviousQualityAssignments()
	{
		_meleeQualityAssignments = new Dictionary<Tuple<GemMaterial, string>, string>();
	}

	public void UpdateFromUserData(GemReportViewModel gemReportViewModel)
	{
		CaratGrandTotal = gemReportViewModel.CaratGrandTotal;
		CaratGrandTotalQty = gemReportViewModel.CaratGrandTotalQty;
		CaratTotalCenter = gemReportViewModel.CaratTotalCenter;
		CaratTotalCenterQty = gemReportViewModel.CaratTotalCenterQty;
		CaratTotalLarge = gemReportViewModel.CaratTotalLarge;
		CaratTotalLargeQty = gemReportViewModel.CaratTotalLargeQty;
		CaratTotalMelee = gemReportViewModel.CaratTotalMelee;
		CaratTotalMeleeQty = gemReportViewModel.CaratTotalMeleeQty;
		LargeGems = gemReportViewModel.LargeGems;
		Gems = gemReportViewModel.Gems;
		GemGroups = gemReportViewModel.GemGroups;
		AssignedDiamondsList = gemReportViewModel.AssignedDiamondsList;
		GroupedStoneViewActive = gemReportViewModel.GroupedStoneViewActive;
		if (Gems.Count > 0)
		{
			IsStonesSectionVisible = true;
		}
	}

	public void UpdateGemGroupQualityAssignments(GemGroup gemGroup)
	{
		if (_meleeQualityAssignments.Any((KeyValuePair<Tuple<GemMaterial, string>, string> prev) => prev.Key.Item1 == gemGroup.Material && prev.Key.Item2 == gemGroup.Shape))
		{
			KeyValuePair<Tuple<GemMaterial, string>, string> keyValuePair = _meleeQualityAssignments.FirstOrDefault((KeyValuePair<Tuple<GemMaterial, string>, string> prev) => prev.Key.Item1 == gemGroup.Material && prev.Key.Item2 == gemGroup.Shape);
			_meleeQualityAssignments.Remove(keyValuePair.Key);
		}
		_meleeQualityAssignments.Add(new Tuple<GemMaterial, string>(gemGroup.Material, gemGroup.Shape), gemGroup.StoneQuality);
	}

	public void UpdateGemGroupsFromGems()
	{
		GemGroups = CreateGemGroups(Gems);
	}

	public void UpdateGemsFromGemGroups()
	{
		ObservableCollection<Gem> observableCollection = new ObservableCollection<Gem>();
		foreach (GemGroup gemGroup in GemGroups)
		{
			foreach (string gemNumber in gemGroup.GemNumbers)
			{
				foreach (Gem gem in Gems)
				{
					if (gem.GemNumber == gemNumber)
					{
						observableCollection.Add(new Gem
						{
							Carat = gem.Carat,
							GemNumber = gem.GemNumber,
							GroupId = gem.GroupId,
							Height = gem.Height,
							Width = gem.Width,
							Length = gem.Length,
							Quantity = gem.Quantity,
							Shape = gem.Shape,
							Type = gem.Type,
							Source = gemGroup.Source,
							Value = gemGroup.Value,
							StullerId = gemGroup.StullerId,
							StoneType = gemGroup.StoneType,
							StoneQuality = gemGroup.StoneQuality,
							StoneQualities = gemGroup.StoneQualities,
							Material = gemGroup.Material,
							IsMeleeStone = gemGroup.IsMeleeStone,
							IsCenterStone = gemGroup.IsCenterStone,
							IsSideStone = gemGroup.IsSideStone,
							IsLargeStone = gemGroup.IsLargeStone,
							AgtaInformation = gemGroup.AgtaInformation,
							IsRedBox = gemGroup.IsRedBox,
							GemMountingType = gem.GemMountingType
						});
					}
				}
			}
		}
		Gems = observableCollection;
	}

	private ObservableCollection<GemGroup> CreateGemGroups(IEnumerable<Gem> gemList)
	{
		bool flag = false;
		if (!base.IsInDesignMode)
		{
			flag = ViewModelLocator.Current.OrderSummaryVM.IsGroupingMeleeBySize;
		}
		ObservableCollection<GemGroup> observableCollection = ((!flag) ? new ObservableCollection<GemGroup>(gemList.GroupBy((Gem gemKey) => new { gemKey.Material, gemKey.Shape }, (key, IEnumerable<Gem> grouping) =>
		{
			Gem[] source = (grouping as Gem[]) ?? grouping.ToArray();
			return new GemGroup
			{
				GemsInGroup = source.ToList(),
				Carat = source.Sum((Gem g) => g.Carat),
				Material = key.Material,
				Shape = key.Shape,
				Source = source.Select((Gem gem) => gem.Source).FirstOrDefault(),
				StoneType = source.Select((Gem gem) => gem.StoneType).FirstOrDefault(),
				StoneQuality = source.Select((Gem gem) => gem.StoneQuality).FirstOrDefault(),
				Type = source.Select((Gem gem) => gem.Type).FirstOrDefault(),
				StullerId = source.Select((Gem gem) => gem.StullerId).FirstOrDefault(),
				StullerDescription = source.Select((Gem gem) => gem.StullerDescription).FirstOrDefault(),
				Value = source.Select((Gem gem) => gem.Value).FirstOrDefault(),
				GemNumbers = source.Select((Gem gem) => gem.GemNumber).ToList(),
				Quantity = source.Count(),
				IsMeleeStone = source.First().IsMeleeStone,
				IsCenterStone = source.First().IsCenterStone,
				IsSideStone = source.First().IsSideStone,
				IsLargeStone = source.First().IsLargeStone,
				AgtaInformation = source.First().AgtaInformation,
				IsRedBox = source.First().IsRedBox,
				GemMountingType = source.First().GemMountingType
			};
		})) : new ObservableCollection<GemGroup>(gemList.GroupBy((Gem gemKey) => new { gemKey.Material, gemKey.Shape, gemKey.Width, gemKey.Height }, (key, IEnumerable<Gem> grouping) =>
		{
			Gem[] source = (grouping as Gem[]) ?? grouping.ToArray();
			return new GemGroup
			{
				GemsInGroup = source.ToList(),
				Carat = source.Sum((Gem g) => g.Carat),
				Material = key.Material,
				Shape = key.Shape,
				Source = source.Select((Gem gem) => gem.Source).FirstOrDefault(),
				StoneType = source.Select((Gem gem) => gem.StoneType).FirstOrDefault(),
				StoneQuality = source.Select((Gem gem) => gem.StoneQuality).FirstOrDefault(),
				Type = source.Select((Gem gem) => gem.Type).FirstOrDefault(),
				StullerId = source.Select((Gem gem) => gem.StullerId).FirstOrDefault(),
				StullerDescription = source.Select((Gem gem) => gem.StullerDescription).FirstOrDefault(),
				Value = source.Select((Gem gem) => gem.Value).FirstOrDefault(),
				GemNumbers = source.Select((Gem gem) => gem.GemNumber).ToList(),
				Quantity = source.Count(),
				IsMeleeStone = source.First().IsMeleeStone,
				IsCenterStone = source.First().IsCenterStone,
				IsSideStone = source.First().IsSideStone,
				IsLargeStone = source.First().IsLargeStone,
				AgtaInformation = source.First().AgtaInformation,
				IsRedBox = source.First().IsRedBox,
				GemMountingType = source.First().GemMountingType
			};
		})));
		observableCollection.ForEach(delegate(GemGroup _)
		{
			if (_meleeQualityAssignments.Any((KeyValuePair<Tuple<GemMaterial, string>, string> prev) => prev.Key.Item1 == _.Material && prev.Key.Item2 == _.Shape))
			{
				KeyValuePair<Tuple<GemMaterial, string>, string> keyValuePair = _meleeQualityAssignments.FirstOrDefault((KeyValuePair<Tuple<GemMaterial, string>, string> prev) => prev.Key.Item1 == _.Material && prev.Key.Item2 == _.Shape);
				_.StoneQuality = keyValuePair.Value;
			}
			else
			{
				_.StoneQuality = _.StoneQualities.FirstOrDefault();
			}
		});
		return observableCollection;
	}

	private void DesignOpenedRefresh(Design obj)
	{
		if (UIHelper.IsGemReportActive)
		{
			Populate();
		}
	}

	private void GemClearProvidedDetails(Gem gem)
	{
		gem.StoneType = string.Empty;
		gem.Value = string.Empty;
		gem.StullerId = string.Empty;
		gem.StoneQuality = string.Empty;
	}

	private void GemGroupClearProvidedDetails(GemGroup gemGroup)
	{
		gemGroup.StoneType = string.Empty;
		gemGroup.Value = string.Empty;
		gemGroup.StullerId = null;
		gemGroup.StullerDescription = null;
		gemGroup.StoneQuality = string.Empty;
		gemGroup.StoneColor = string.Empty;
	}

	private void GemGroupSetSourceNoStone(GemGroup gemGroup)
	{
		GemGroupClearProvidedDetails(gemGroup);
		UpdateGemGroupSource(Strings.No_Stone_s_Supplied, gemGroup);
		gemGroup.Source = Strings.No_Stone_s_Supplied;
	}

	private void GemGroupSetSourceProvided(GemGroup gemGroup)
	{
		GemGroupClearProvidedDetails(gemGroup);
		UpdateGemGroupSource(Strings.Use_My_Stone_s, gemGroup);
		gemGroup.Source = Strings.Use_My_Stone_s;
	}

	private void GemGroupSetSourceStuller(GemGroup gemGroup)
	{
		GemGroupClearProvidedDetails(gemGroup);
		UpdateGemGroupSource(Strings.Purchase_Stone_s, gemGroup);
		gemGroup.Source = Strings.Purchase_Stone_s;
	}

	private void GemSetSourceNoStone(Gem gem)
	{
		gem.Source = Strings.No_Stone_s_Supplied;
		GemClearProvidedDetails(gem);
	}

	private void GemSetSourceProvided(Gem gem)
	{
		gem.Source = Strings.Use_My_Stone_s;
		GemClearProvidedDetails(gem);
	}

	private void GemSetSourceStuller(Gem gem)
	{
		gem.Source = Strings.Purchase_Stone_s;
		GemClearProvidedDetails(gem);
	}

	private void HighlightCancel()
	{
		if (!UIHelper.IsOrderedDesignLoaded)
		{
			UIHelper.CoreDataService.ClearHighlightedGems();
		}
	}

	private void HighlightGem(Gem gem)
	{
		if (!UIHelper.IsOrderedDesignLoaded && gem != null)
		{
			UIHelper.CoreDataService.HighlightGemFromGemReportByGemNumber(new List<string> { gem.GemNumber });
		}
	}

	private void HighlightGemGroup(GemGroup gemGroup)
	{
		if (!UIHelper.IsOrderedDesignLoaded && gemGroup != null)
		{
			UIHelper.CoreDataService.HighlightGemFromGemReportByGemNumber(gemGroup.GemNumbers);
		}
	}

	private void InitializeCommands()
	{
		CommandGemGroupSetSourceStuller = new RelayCommand<GemGroup>(GemGroupSetSourceStuller);
		CommandGemGroupSetSourceProvided = new RelayCommand<GemGroup>(GemGroupSetSourceProvided);
		CommandGemGroupSetSourceNoStone = new RelayCommand<GemGroup>(GemGroupSetSourceNoStone);
		CommandGemSetSourceNoStone = new RelayCommand<Gem>(GemSetSourceNoStone);
		CommandGemSetSourceProvided = new RelayCommand<Gem>(GemSetSourceProvided);
		CommandGemSetSourceStuller = new RelayCommand<Gem>(GemSetSourceStuller);
		CommandUpdateGemGroups = new RelayCommand(UpdateGemGroupsFromGems);
		CommandUpdateGemsFromGemGroups = new RelayCommand(UpdateGemsFromGemGroups);
		CommandOpenStullerSite = new RelayCommand(OpenStullerSite);
		CommandHighlightGem = new RelayCommand<Gem>(HighlightGem);
		CommandHighlightGemGroup = new RelayCommand<GemGroup>(HighlightGemGroup);
		CommandHighlightCancel = new RelayCommand(HighlightCancel);
		CommandShopForDiamond = new RelayCommand<Gem>(ShopForDiamond);
		CommandShopForGemstone = new RelayCommand<Gem>(ShopForGemstone);
		CommandUpdateSourceMeleeGems = new RelayCommand<int>(UpdateSourceMeleeGems);
	}

	private void LoadGemData(List<StudioGem> originalGemList)
	{
		CaratGrandTotal = 0.0;
		CaratTotalCenter = 0.0;
		CaratTotalLarge = 0.0;
		CaratTotalMelee = 0.0;
		CaratGrandTotalQty = 0.0;
		CaratTotalCenterQty = 0.0;
		CaratTotalLargeQty = 0.0;
		CaratTotalMeleeQty = 0.0;
		IEnumerable<StudioGem> enumerable = originalGemList.Where((StudioGem gem) => gem.IsMeleeStone);
		IEnumerable<StudioGem> source = originalGemList.Where((StudioGem gem) => gem.IsLargeStone);
		List<MaterialOptionItem> gemMaterialOptionItems = GemHelper.GetGemMaterialOptionItems().ToList();
		ObservableCollection<Gem> source2 = new ObservableCollection<Gem>(originalGemList.Select((StudioGem g) => new Gem
		{
			Type = ((gemMaterialOptionItems.FirstOrDefault((MaterialOptionItem m) => m.SearchAttributeId == (int)g.GemMaterialOption) != null) ? gemMaterialOptionItems.FirstOrDefault((MaterialOptionItem m) => m.SearchAttributeId == (int)g.GemMaterialOption).PrimaryText : "UnAssigned"),
			Source = Strings.Purchase_Stone_s,
			Shape = Enum.GetName(typeof(StudioGemShape), g.GemShapeOption),
			Length = (double)g.Length,
			Width = (double)g.Width,
			Height = (double)g.Height.Value,
			Carat = (double)g.Weight.GetValueOrDefault(),
			Quantity = g.Quantity,
			GemNumber = g.GemNumber,
			Material = g.GemMaterialOption,
			IsLargeStone = g.IsLargeStone,
			IsMeleeStone = g.IsMeleeStone,
			IsCenterStone = g.IsCenterStone,
			IsSideStone = g.IsSideStone,
			GemMountingType = g.GemMountingType
		}));
		StoneReportGems = new ObservableCollection<GemGroup>(source2.GroupBy((Gem gemKey) => new { gemKey.Shape, gemKey.Width, gemKey.Length, gemKey.Height, gemKey.Material }, (key, IEnumerable<Gem> grouping) =>
		{
			Gem[] source4 = (grouping as Gem[]) ?? grouping.ToArray();
			return new GemGroup
			{
				Shape = key.Shape,
				Length = key.Length,
				Width = key.Width,
				Height = key.Height,
				GemsInGroup = source4.ToList(),
				GemNumbers = source4.Select((Gem gem) => gem.GemNumber).ToList(),
				Carat = source4.Sum((Gem g) => g.Carat),
				Material = key.Material,
				Source = source4.First().Source,
				StoneType = source4.First().StoneType,
				StoneQuality = source4.First().StoneQuality,
				Type = source4.First().Type,
				StullerId = source4.First().StullerId,
				StullerDescription = source4.First().StullerDescription,
				Value = source4.First().Value,
				Quantity = source4.Count(),
				IsMeleeStone = source4.First().IsMeleeStone,
				IsCenterStone = source4.First().IsCenterStone,
				IsSideStone = source4.First().IsSideStone,
				IsLargeStone = source4.First().IsLargeStone,
				AgtaInformation = source4.First().AgtaInformation,
				GemMountingType = source4.First().GemMountingType
			};
		}));
		originalGemList.ForEach(delegate(StudioGem g)
		{
			if (g.Weight.HasValue)
			{
				double caratTotalMeleeQty;
				if (g.IsMeleeStone)
				{
					CaratTotalMelee += (double)g.Weight.Value;
					caratTotalMeleeQty = CaratTotalMeleeQty;
					CaratTotalMeleeQty = caratTotalMeleeQty + 1.0;
				}
				else if (g.IsCenterStone)
				{
					CaratTotalCenter += (double)g.Weight.Value;
					caratTotalMeleeQty = CaratTotalCenterQty;
					CaratTotalCenterQty = caratTotalMeleeQty + 1.0;
				}
				else if (g.IsLargeStone)
				{
					CaratTotalLarge += (double)g.Weight.Value;
					caratTotalMeleeQty = CaratTotalLargeQty;
					CaratTotalLargeQty = caratTotalMeleeQty + 1.0;
				}
				CaratGrandTotal += (double)g.Weight.Value;
				caratTotalMeleeQty = CaratGrandTotalQty;
				CaratGrandTotalQty = caratTotalMeleeQty + 1.0;
			}
		});
		List<Tuple<Gem, Diamond>> currentAssignments = new List<Tuple<Gem, Diamond>>(AssignedDiamondsList);
		AssignedDiamondsList = new List<Tuple<Gem, Diamond>>();
		List<Gem> originalLargeGems = new List<Gem>(LargeGems);
		LargeGems = new ObservableCollection<Gem>(source.ToList().Select(delegate(StudioGem g)
		{
			Gem gem = originalLargeGems.FirstOrDefault((Gem _) => _.GemNumber == g.GemNumber);
			Diamond assignedDiamond = null;
			if (g.IsCenterStone && ViewModelLocator.Current.StoneTrayVM.IsStoneAssigned)
			{
				assignedDiamond = ViewModelLocator.Current.StoneTrayVM.StoneChosen;
			}
			else if (currentAssignments.Any())
			{
				double heightMax = (double)g.Height.Value * 1.1;
				double heightMin = (double)g.Height.Value * 0.9;
				double widthMax = (double)g.Width * 1.1;
				double widthMin = (double)g.Width * 0.9;
				Tuple<Gem, Diamond> tuple = currentAssignments.FirstOrDefault((Tuple<Gem, Diamond> _) => _.Item1.IsSideStone && heightMin < _.Item1.Height && heightMax > _.Item1.Height && widthMin < _.Item1.Width && widthMax > _.Item1.Width && _.Item1.Shape == g.GemShapeOption.ToString() && _.Item1.GemNumber == g.GemNumber);
				if (tuple != null)
				{
					assignedDiamond = tuple.Item2;
					currentAssignments.Remove(tuple);
				}
			}
			bool isRedBox = true;
			if (gem != null && gem.IsRedBox)
			{
				isRedBox = gem.IsRedBox;
			}
			Gem gem2 = new Gem();
			gem2.Type = ((gemMaterialOptionItems.FirstOrDefault((MaterialOptionItem m) => m.SearchAttributeId == (int)g.GemMaterialOption) != null) ? gemMaterialOptionItems.FirstOrDefault((MaterialOptionItem m) => m.SearchAttributeId == (int)g.GemMaterialOption).PrimaryText : "UnAssigned");
			gem2.Source = Strings.Purchase_Stone_s;
			gem2.Shape = Enum.GetName(typeof(StudioGemShape), g.GemShapeOption);
			gem2.Length = (double)g.Length;
			gem2.Width = (double)g.Width;
			gem2.Height = (double)g.Height.Value;
			gem2.Carat = (double)g.Weight.GetValueOrDefault();
			gem2.Quantity = g.Quantity;
			gem2.GemNumber = g.GemNumber;
			gem2.Material = g.GemMaterialOption;
			gem2.IsLargeStone = g.IsLargeStone;
			gem2.IsMeleeStone = g.IsMeleeStone;
			gem2.IsCenterStone = g.IsCenterStone;
			gem2.IsSideStone = g.IsSideStone;
			gem2.AssignedDiamond = assignedDiamond;
			gem2.GemMountingType = g.GemMountingType;
			gem2.SetIsRedBox(isRedBox);
			return gem2;
		}));
		IList<StudioGem> source3 = (enumerable as IList<StudioGem>) ?? enumerable.ToList();
		Gems = new ObservableCollection<Gem>(source3.Select((StudioGem g) => new Gem
		{
			Type = ((gemMaterialOptionItems.FirstOrDefault((MaterialOptionItem m) => m.SearchAttributeId == (int)g.GemMaterialOption) != null) ? gemMaterialOptionItems.FirstOrDefault((MaterialOptionItem m) => m.SearchAttributeId == (int)g.GemMaterialOption).PrimaryText : "UnAssigned"),
			Source = Strings.Purchase_Stone_s,
			Shape = Enum.GetName(typeof(StudioGemShape), g.GemShapeOption),
			Length = (double)g.Length,
			Width = (double)g.Width,
			Height = (double)g.Height.Value,
			Carat = (double)g.Weight.GetValueOrDefault(),
			Quantity = g.Quantity,
			GemNumber = g.GemNumber,
			Material = g.GemMaterialOption,
			IsLargeStone = g.IsLargeStone,
			IsMeleeStone = g.IsMeleeStone,
			IsCenterStone = g.IsCenterStone,
			IsSideStone = g.IsSideStone,
			GemMountingType = g.GemMountingType
		}));
		GemGroups = CreateGemGroups(Gems);
	}

	private void OpenStullerSite()
	{
		ViewModelLocator.Current.MainVM.OpenURLInOurBrowserWindow("http://www.stuller.com");
	}

	private void PopulateTestData()
	{
		LoadGemData(new List<StudioGem>
		{
			new StudioGem
			{
				GemMaterialOption = GemMaterial.Diamond,
				GemShapeOption = StudioGemShape.Round,
				Length = 2m,
				Width = 4m,
				Height = 4,
				Weight = 1.02m,
				Quantity = 1,
				IsCenterStone = true
			},
			new StudioGem
			{
				GemMaterialOption = GemMaterial.Diamond,
				GemShapeOption = StudioGemShape.Round,
				Length = 2m,
				Width = 5m,
				Height = 5,
				Weight = 2.02m,
				Quantity = 1,
				IsLargeStone = true
			},
			new StudioGem
			{
				GemMaterialOption = GemMaterial.Diamond,
				GemShapeOption = StudioGemShape.Round,
				Length = 2m,
				Width = 5m,
				Height = 5,
				Weight = 2.02m,
				Quantity = 1,
				IsLargeStone = true
			},
			new StudioGem
			{
				GemMaterialOption = GemMaterial.Diamond,
				GemShapeOption = StudioGemShape.Round,
				Length = 2m,
				Width = 5m,
				Height = 5,
				Weight = 2.02m,
				Quantity = 1,
				IsLargeStone = true
			},
			new StudioGem
			{
				GemMaterialOption = GemMaterial.Diamond,
				GemShapeOption = StudioGemShape.Round,
				Length = 2m,
				Width = 5m,
				Height = 5,
				Weight = 2.02m,
				Quantity = 1,
				IsLargeStone = true
			},
			new StudioGem
			{
				GemMaterialOption = GemMaterial.Aquamarine,
				GemShapeOption = StudioGemShape.Round,
				Length = 4m,
				Width = 4m,
				Height = 4,
				Weight = 3,
				Quantity = 2,
				IsLargeStone = true
			},
			new StudioGem
			{
				GemMaterialOption = GemMaterial.Ruby,
				GemShapeOption = StudioGemShape.Round,
				Length = 2m,
				Width = 3.1245m,
				Height = 4,
				Weight = 1m,
				Quantity = 3,
				IsMeleeStone = true
			},
			new StudioGem
			{
				GemMaterialOption = GemMaterial.Alexandrite,
				GemShapeOption = StudioGemShape.CushionSquare,
				Length = 3m,
				Width = 3m,
				Height = 4,
				Weight = 1.3m,
				Quantity = 4,
				IsMeleeStone = true
			},
			new StudioGem
			{
				GemMaterialOption = GemMaterial.Alexandrite,
				GemShapeOption = StudioGemShape.CushionSquare,
				Length = 3m,
				Width = 3.1m,
				Height = 4,
				Weight = 0.7m,
				Quantity = 6,
				IsMeleeStone = true
			},
			new StudioGem
			{
				GemMaterialOption = GemMaterial.Alexandrite,
				GemShapeOption = StudioGemShape.CushionSquare,
				Length = 1m,
				Width = 3m,
				Height = 4,
				Weight = 1m,
				Quantity = 7,
				IsMeleeStone = true
			}
		});
		Gem gem = LargeGems.First();
		gem.IsRedBox = true;
		gem.ImageURL = "http://stuller.scene7.com/is/image/Stuller/f43c252f-594e-45af-827d-a28300f9d620?bgc=000000&$geminfo$";
		gem.AssignedDiamond = new Diamond
		{
			SerialNumber = 12345
		};
		LargeGems.Skip(1).First().ForceValidation();
		Gem gem2 = LargeGems.Skip(2).First();
		gem2.AssignedDiamond = null;
		gem2.Source = Strings.No_Stone_s_Supplied;
		gem2.ForceValidation();
		GemGroup gemGroup = GemGroups[0];
		gemGroup.Source = Strings.Purchase_Stone_s;
		gemGroup.ForceValidation();
		GemGroup gemGroup2 = GemGroups[1];
		gemGroup2.Source = Strings.Use_My_Stone_s;
		gemGroup2.ForceValidation();
		GemGroup gemGroup3 = GemGroups[2];
		gemGroup3.Source = Strings.No_Stone_s_Supplied;
		gemGroup3.ForceValidation();
		GroupedStoneViewActive = false;
	}

	private void ShopForDiamond(Gem suppliedGem)
	{
		PunchoutHelper.ShowDiamondShopper();
	}

	private void ShopForGemstone(Gem suppliedGem)
	{
		PunchoutHelper.ShowGemstoneShopper();
	}

	private void UpdateGemGroupSource(string gemSource, GemGroup gemGroup)
	{
		Gems.Where((Gem g) => gemGroup.GemNumbers.Contains(g.GemNumber)).ForEach(delegate(Gem g)
		{
			g.Source = gemSource;
		});
	}

	private void UpdateGemMaterialsChange(List<StudioGem> newGemList)
	{
		IEnumerable<StudioGem> enumerable = newGemList.Where((StudioGem gem) => gem.IsMeleeStone);
		IEnumerable<StudioGem> largeGems = newGemList.Where((StudioGem gem) => gem.IsLargeStone || gem.IsCenterStone);
		List<MaterialOptionItem> gemMaterialOptionItems = GemHelper.GetGemMaterialOptionItems().ToList();
		ObservableCollection<Gem> stoneReportGems = new ObservableCollection<Gem>(newGemList.Select((StudioGem g) => new Gem
		{
			Type = ((gemMaterialOptionItems.FirstOrDefault((MaterialOptionItem m) => m.SearchAttributeId == (int)g.GemMaterialOption) != null) ? gemMaterialOptionItems.FirstOrDefault((MaterialOptionItem m) => m.SearchAttributeId == (int)g.GemMaterialOption).PrimaryText : "UnAssigned"),
			Source = Strings.Purchase_Stone_s,
			Shape = Enum.GetName(typeof(StudioGemShape), g.GemShapeOption),
			Length = (double)g.Length,
			Width = (double)g.Width,
			Height = (double)g.Height.Value,
			Carat = (double)g.Weight.GetValueOrDefault(),
			Quantity = g.Quantity,
			GemNumber = g.GemNumber,
			Material = g.GemMaterialOption,
			IsLargeStone = g.IsLargeStone,
			IsMeleeStone = g.IsMeleeStone,
			IsCenterStone = g.IsCenterStone,
			IsSideStone = g.IsSideStone,
			GemMountingType = g.GemMountingType
		}));
		StoneReportGems.ForEach(delegate(GemGroup gemGroup)
		{
			Gem gem = stoneReportGems.FirstOrDefault((Gem gem2) => gemGroup.GemNumbers.Contains(gem2.GemNumber));
			if (gem != null)
			{
				gemGroup.Type = gem.Type;
				gemGroup.Material = gem.Material;
				gemGroup.Carat = stoneReportGems.Where((Gem gem2) => gemGroup.GemNumbers.Contains(gem2.GemNumber)).Sum((Gem g) => g.Carat);
			}
		});
		LargeGems.ForEach(delegate(Gem largeGem)
		{
			StudioGem matchingGem = largeGems.FirstOrDefault((StudioGem _) => _.GemNumber == largeGem.GemNumber);
			if (matchingGem != null)
			{
				largeGem.Type = ((gemMaterialOptionItems.FirstOrDefault((MaterialOptionItem m) => m.SearchAttributeId == (int)matchingGem.GemMaterialOption) != null) ? gemMaterialOptionItems.FirstOrDefault((MaterialOptionItem m) => m.SearchAttributeId == (int)matchingGem.GemMaterialOption).PrimaryText : "UnAssigned");
				largeGem.Carat = (double)matchingGem.Weight.GetValueOrDefault();
				largeGem.Material = matchingGem.GemMaterialOption;
				largeGem.IsRedBox = matchingGem.GemMaterialOption == GemMaterial.Diamond;
			}
		});
		IList<StudioGem> studioGems = (enumerable as IList<StudioGem>) ?? enumerable.ToList();
		Gems.ForEach(delegate(Gem gem)
		{
			StudioGem matchingGem = studioGems.FirstOrDefault((StudioGem _) => gem.GemNumber == _.GemNumber);
			if (matchingGem != null)
			{
				gem.Material = matchingGem.GemMaterialOption;
				gem.Type = ((gemMaterialOptionItems.FirstOrDefault((MaterialOptionItem m) => m.SearchAttributeId == (int)matchingGem.GemMaterialOption) != null) ? gemMaterialOptionItems.FirstOrDefault((MaterialOptionItem m) => m.SearchAttributeId == (int)matchingGem.GemMaterialOption).PrimaryText : "UnAssigned");
				gem.Carat = (double)matchingGem.Weight.GetValueOrDefault();
			}
		});
		GemGroups = CreateGemGroups(Gems);
	}

	private void UpdateSourceMeleeGems(int selectedIndex)
	{
		ViewModelLocator.Current.OrderSummaryVM.ResetQuote();
		if (selectedIndex >= 0)
		{
			ViewModelLocator.Current.OrderSummaryVM.ShopNonSerializedGems();
			ViewModelLocator.Current.OrderSummaryVM.ReValidateOrder();
		}
	}
}
