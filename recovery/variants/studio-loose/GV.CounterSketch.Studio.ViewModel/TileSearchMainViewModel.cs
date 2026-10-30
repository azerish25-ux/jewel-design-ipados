using System;
using System.Diagnostics;
using System.Linq;
using GV.CounterSketch.Studio.Model.Objects.Enums;
using GV.CounterSketch.Studio.Utilities.Helpers;
using GV.CounterSketch.Studio.View;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Messaging;

namespace GV.CounterSketch.Studio.ViewModel;

public class TileSearchMainViewModel : ViewModelBase
{
	public TileSearchCategory _keywordResetTile;

	private string _currentVisualState;

	private bool _isSearchFromTile;

	private string _keywordSearch;

	private TileSearchCategoryViewModel _tileSearchCategoryVM;

	private TileSearchResultDesignsViewModel _tileSearchResultDesignsVM;

	private TileSearchResultPartsViewModel _tileSearchResultPartsVM;

	private bool _isSearchingDesigns;

	private bool _isSearchingParts;

	private TileSearchResultViewModelBase _activeSearchResultViewModel;

	private bool _isFreehandOnlyMode;

	private bool _isSearchingUserParts;

	public bool IsKeywordPopulated => !string.IsNullOrWhiteSpace(KeywordSearch);

	public bool IsSearchingUserParts
	{
		get
		{
			return _isSearchingUserParts;
		}
		set
		{
			_isSearchingUserParts = value;
			((ObservableObject)this).RaisePropertyChanged("IsSearchingUserParts");
		}
	}

	public bool IsFreehandOnlyMode
	{
		get
		{
			return _isFreehandOnlyMode;
		}
		set
		{
			_isFreehandOnlyMode = value;
			if (value)
			{
				IsSearchingDesigns = false;
			}
			((ObservableObject)this).RaisePropertyChanged("IsFreehandOnlyMode");
		}
	}

	public RelayCommand<string> CommandChooseFeaturedModelBySPI { get; set; }

	public RelayCommand CommandClearSearchKeyword { get; set; }

	public RelayCommand CommandPopulateSearchFromKeyword { get; set; }

	public RelayCommand CommandPopulateSearchResults { get; set; }

	public RelayCommand CommandPopulateSearchResultsFromEverAndEver { get; set; }

	public RelayCommand CommandPopulateSearchResultsFromPackageKit { get; set; }

	public RelayCommand<TileSearchCategory> CommandSetSelectedSearchTile { get; set; }

	public RelayCommand CommandTransitionToTileSearchCategoryState { get; set; }

	public string CurrentVisualState
	{
		get
		{
			return _currentVisualState;
		}
		set
		{
			_currentVisualState = value;
			((ObservableObject)this).RaisePropertyChanged("CurrentVisualState");
			((ObservableObject)this).RaisePropertyChanged("IsSPIEnabled");
		}
	}

	public string KeywordSearch
	{
		get
		{
			return _keywordSearch;
		}
		set
		{
			if (!(_keywordSearch == value))
			{
				_keywordSearch = value;
				((ObservableObject)this).RaisePropertyChanged("KeywordSearch");
				((ObservableObject)this).RaisePropertyChanged("IsKeywordPopulated");
			}
		}
	}

	public TileSearchCategoryViewModel TileSearchCategoryVM
	{
		get
		{
			return _tileSearchCategoryVM;
		}
		set
		{
			if (_tileSearchCategoryVM != value)
			{
				_tileSearchCategoryVM = value;
				((ObservableObject)this).RaisePropertyChanged("TileSearchCategoryVM");
			}
		}
	}

	public TileSearchResultViewModelBase ActiveSearchResultViewModel
	{
		get
		{
			return _activeSearchResultViewModel;
		}
		set
		{
			if (_activeSearchResultViewModel != value)
			{
				_activeSearchResultViewModel = value;
				((ObservableObject)this).RaisePropertyChanged("ActiveSearchResultViewModel");
			}
		}
	}

	public bool IsSearchingDesigns
	{
		get
		{
			return _isSearchingDesigns;
		}
		set
		{
			_isSearchingDesigns = value;
			((ObservableObject)this).RaisePropertyChanged("IsSearchingDesigns");
			_isSearchingParts = !value;
			((ObservableObject)this).RaisePropertyChanged("IsSearchingParts");
			if (value)
			{
				ActiveSearchResultViewModel = TileSearchResultDesignsVM;
				IsSearchingUserParts = false;
			}
			else
			{
				ActiveSearchResultViewModel = TileSearchResultPartsVM;
			}
		}
	}

	public bool IsSearchingParts
	{
		get
		{
			return _isSearchingParts;
		}
		set
		{
			_isSearchingParts = value;
			((ObservableObject)this).RaisePropertyChanged("IsSearchingParts");
			IsSearchingDesigns = !value;
			if (value)
			{
				IsSearchingUserParts = false;
			}
		}
	}

	public TileSearchResultDesignsViewModel TileSearchResultDesignsVM
	{
		get
		{
			return _tileSearchResultDesignsVM;
		}
		set
		{
			if (_tileSearchResultDesignsVM != value)
			{
				_tileSearchResultDesignsVM = value;
				((ObservableObject)this).RaisePropertyChanged("TileSearchResultDesignsVM");
			}
		}
	}

	public TileSearchResultPartsViewModel TileSearchResultPartsVM
	{
		get
		{
			return _tileSearchResultPartsVM;
		}
		set
		{
			if (_tileSearchResultPartsVM != value)
			{
				_tileSearchResultPartsVM = value;
				((ObservableObject)this).RaisePropertyChanged("TileSearchResultPartsVM");
			}
		}
	}

	public RelayCommand CommandShowStoneShopper { get; set; }

	public bool SkipPopulateAfterStorboardCompletes { get; set; }

	public TileSearchMainViewModel()
	{
		CurrentVisualState = "TileSearchCategories";
		TileSearchCategoryVM = new TileSearchCategoryViewModel();
		TileSearchResultDesignsVM = new TileSearchResultDesignsViewModel();
		TileSearchResultPartsVM = new TileSearchResultPartsViewModel();
		IsSearchingDesigns = true;
		IsFreehandOnlyMode = UIHelper.KeyProfile.IsDesignLibraryDisabled;
		if (!((ViewModelBase)this).IsInDesignMode)
		{
			InitializeCommands();
		}
		ActiveSearchResultViewModel = TileSearchResultDesignsVM;
	}

	public void PopulateSearchResults()
	{
		if (_isSearchFromTile)
		{
			Stopwatch.StartNew();
			ActiveSearchResultViewModel.GetDesignSearchResultsByTileCategory();
			_isSearchFromTile = false;
			Messenger.Default.Send<string, TileSearchResultListView>("ResetScroll");
		}
	}

	public void Reset()
	{
		ViewModelLocator.Current.StudioMaterialSelectionVM.DefaultMaterialGroupOptions();
		ActiveSearchResultViewModel.Reset();
		TransitionToTileSearchCategoryState();
	}

	public void SetSearchViewOption()
	{
		switch (UIHelper.CurrentSearchResultImageType)
		{
		case ImageType.Perspective:
			ActiveSearchResultViewModel.IsPerspectiveView = true;
			break;
		case ImageType.Top:
			ActiveSearchResultViewModel.IsTopView = true;
			break;
		case ImageType.Side:
			ActiveSearchResultViewModel.IsSideView = true;
			break;
		case ImageType.Front:
			ActiveSearchResultViewModel.IsFrontView = true;
			break;
		}
	}

	public void SetSelectedPartTile(TileSearchCategory searchCategory)
	{
		IsSearchingUserParts = false;
		TileSearchResultPartsVM.SelectedSearchTileCategory = searchCategory;
		_isSearchFromTile = true;
		KeywordSearch = string.Empty;
		_keywordResetTile = searchCategory;
		TileSearchResultPartsVM.ClearSearchResults();
		ViewModelLocator.Current.TileSearchMainVM.TileSearchResultPartsVM.NavigateForwardToResults();
	}

	public void SetSelectedSearchTile(TileSearchCategory searchCategory, bool skipPopulate = false)
	{
		IsSearchingUserParts = false;
		if (searchCategory.Id == 1)
		{
			ViewModelLocator.Current.MainVM.SetFreehandModeActive();
			return;
		}
		TileSearchResultDesignsVM.ClearSearchResults();
		ActiveSearchResultViewModel.SelectedSearchTileCategory = searchCategory;
		_isSearchFromTile = true;
		KeywordSearch = string.Empty;
		_keywordResetTile = searchCategory;
		CurrentVisualState = "TileSearchResults";
		SkipPopulateAfterStorboardCompletes = skipPopulate;
		ViewModelLocator.Current.MainVM.NavigateToSection(Sections.Search);
	}

	public void SetSelectedUserPartTile()
	{
		IsSearchingUserParts = true;
		_isSearchFromTile = true;
		KeywordSearch = string.Empty;
		TileSearchResultPartsVM.ClearSearchResults();
		ViewModelLocator.Current.TileSearchMainVM.TileSearchResultPartsVM.NavigateForwardToResults();
		ViewModelLocator.Current.TileSearchMainVM.TileSearchResultPartsVM.NavigateForwardToResults();
	}

	public void TransitionToTileSearchCategoryState()
	{
		CurrentVisualState = "TileSearchCategories";
		ActiveSearchResultViewModel.NumberOfSelectedAdvancedFilters = 0;
		ActiveSearchResultViewModel.Reset();
	}

	public void UpdateSPIEnabled()
	{
		((ObservableObject)this).RaisePropertyChanged("IsSPIEnabled");
	}

	private void ChooseFeaturedModelBySPI(string obj)
	{
		string spiNumber = ((!obj.Contains('.')) ? obj : obj.Split('.')[1].Trim());
		ActiveSearchResultViewModel.GetDesignSearchResultsBySPINumber(spiNumber);
	}

	private void ClearSearchKeyword()
	{
		KeywordSearch = null;
		if (_keywordResetTile != null && CurrentVisualState == "TileSearchResults")
		{
			if (_keywordResetTile.Id != -1)
			{
				TileSearchCategoryVM.SelectedTileSearchCategory = _keywordResetTile;
				if (_keywordResetTile.Id != 14)
				{
					SetSelectedSearchTile(_keywordResetTile);
				}
				else
				{
					ActiveSearchResultViewModel.GetDesignSearchResultsByPackageKitName(ActiveSearchResultViewModel.LastUsedPackageName);
				}
			}
			else
			{
				TransitionToTileSearchCategoryState();
			}
		}
		ActiveSearchResultViewModel.ClearIsVariantDesignIncludedInResultsFlag();
	}

	private void InitializeCommands()
	{
		//IL_000e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0018: Expected O, but got Unknown
		//IL_0026: Unknown result type (might be due to invalid IL or missing references)
		//IL_0030: Expected O, but got Unknown
		//IL_003e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0048: Expected O, but got Unknown
		//IL_0056: Unknown result type (might be due to invalid IL or missing references)
		//IL_0060: Expected O, but got Unknown
		//IL_006e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0078: Expected O, but got Unknown
		//IL_0086: Unknown result type (might be due to invalid IL or missing references)
		//IL_0090: Expected O, but got Unknown
		//IL_00b6: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c0: Expected O, but got Unknown
		CommandPopulateSearchResults = new RelayCommand((Action)PopulateSearchResults, false);
		CommandPopulateSearchFromKeyword = new RelayCommand((Action)PopulateSearchFromKeyword, false);
		CommandPopulateSearchResultsFromEverAndEver = new RelayCommand((Action)PopulateSearchResultsFromEverAndEver, false);
		CommandTransitionToTileSearchCategoryState = new RelayCommand((Action)TransitionToTileSearchCategoryState, false);
		CommandClearSearchKeyword = new RelayCommand((Action)ClearSearchKeyword, false);
		CommandPopulateSearchResultsFromPackageKit = new RelayCommand((Action)PopulateSearchResultsFromPackageKit, false);
		CommandChooseFeaturedModelBySPI = new RelayCommand<string>((Action<string>)ChooseFeaturedModelBySPI, false);
		CommandShowStoneShopper = new RelayCommand((Action)ShowStoneShopper, false);
	}

	private void PopulateSearchFromKeyword()
	{
		if (CurrentVisualState == "TileSearchCategories")
		{
			if (UIHelper.KeyProfile.KeyProfileId != 2)
			{
				_keywordResetTile = TileSearchCategoryVM.TileSearchCategories.FirstOrDefault((TileSearchCategory tile) => tile.Id == -1);
			}
			else
			{
				_keywordResetTile = TileSearchCategoryVM.TileSearchCategories.FirstOrDefault((TileSearchCategory tile) => tile.Id == 0);
			}
		}
		Stopwatch.StartNew();
		ViewModelLocator.Current.MainVM.NavigateToSection(Sections.Search);
		ActiveSearchResultViewModel.GetDesignSearchResultsByKeyword(KeywordSearch);
	}

	private void PopulateSearchResultsFromEverAndEver()
	{
		TileSearchCategoryVM.SelectedPackageName = "Ever & Ever Bridal Collection";
		if (!string.IsNullOrWhiteSpace(TileSearchCategoryVM.SelectedPackageName))
		{
			KeywordSearch = string.Empty;
			_keywordResetTile = TileSearchCategoryVM.TileSearchCategories.FirstOrDefault((TileSearchCategory tile) => tile.Id == 19);
			CurrentVisualState = "TileSearchResults";
			Stopwatch.StartNew();
			EventHelper.RecordEvent(EventHelper.EventTypes.Search_Tile, "Package", TileSearchCategoryVM.SelectedPackageName);
			ActiveSearchResultViewModel.GetDesignSearchResultsByPackageKitName(TileSearchCategoryVM.SelectedPackageName);
			TileSearchCategoryVM.SelectedPackageIndex = -1;
		}
	}

	private void PopulateSearchResultsFromPackageKit()
	{
		if (!string.IsNullOrWhiteSpace(TileSearchCategoryVM.SelectedPackageName))
		{
			KeywordSearch = string.Empty;
			_keywordResetTile = TileSearchCategoryVM.TileSearchCategories.FirstOrDefault((TileSearchCategory tile) => tile.Id == 14);
			CurrentVisualState = "TileSearchResults";
			Stopwatch.StartNew();
			EventHelper.RecordEvent(EventHelper.EventTypes.Search_Tile, "Package", TileSearchCategoryVM.SelectedPackageName);
			ActiveSearchResultViewModel.GetDesignSearchResultsByPackageKitName(TileSearchCategoryVM.SelectedPackageName);
			TileSearchCategoryVM.SelectedPackageIndex = -1;
		}
	}

	private void ShowStoneShopper()
	{
		PunchoutHelper.ShowDiamondShopper();
	}
}
