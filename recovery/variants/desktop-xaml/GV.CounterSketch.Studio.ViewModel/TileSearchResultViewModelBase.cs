using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Imaging;
using GV.CounterSketch.Shared;
using GV.CounterSketch.Studio.Database;
using GV.CounterSketch.Studio.Model.Entities.CounterSketchLibrary;
using GV.CounterSketch.Studio.Model.Objects;
using GV.CounterSketch.Studio.Model.Objects.Enums;
using GV.CounterSketch.Studio.Utilities.Extensions;
using GV.CounterSketch.Studio.Utilities.Helpers;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using Localization;
using Stuller.Web.API.v2.Models;

namespace GV.CounterSketch.Studio.ViewModel;

public class TileSearchResultViewModelBase : ViewModelBase
{
	private static CancellationTokenSource _cancellationTokenSource = new CancellationTokenSource();

	protected bool IsKeywordSearch;

	private string _currentDetailImageView = "P";

	private ObservableCollection<DesignSearch> _currentPageDesigns;

	private BitmapSource _designDetailImage;

	private int _designImageWidth;

	private string _gemSearchFilterLength;

	private Size _gemSearchFilterSize;

	private string _gemSearchFilterWidth;

	private bool _isEverAndEverPackage;

	private bool _isFilterModeAllParts;

	private bool _isFilterModeCounterPartsOnly;

	private bool _isFilterModeHasGem;

	private bool _isFilterModeStaticPartsOnly;

	private bool _isFrontView;

	private bool _isIterationAvailable;

	private bool _isPackageSearch;

	private bool _isPerspectiveView;

	private bool _isShowingPartSearchResults;

	private bool _isSideView;

	private bool _isTopView;

	private bool _isUpdatingSearchFilters;

	private bool _isVariantDesignIncludedInResults;

	private string _lastUsedKeyword;

	private string _lastUsedPackageName;

	private int _numberOfSelectedAdvancedFilters;

	private bool _scrollSection;

	private ObservableCollection<SearchFilterViewModel> _searchFilters;

	private ObservableCollection<DesignSearch> _searchResults;

	private double _sectionWidth;

	private DesignSearch _selectedDesignSearch;

	private ObservableCollection<DesignIteration> _selectedDesignSearchFeaturedIterations;

	private string _selectedDesignSearchKeywords;

	private Collection<string> _selectedDesignSearchShapes;

	private ObservableCollection<DesignSearch> _selectedDesignSearchVariants;

	private TileSearchCategory _selectedSearchTileCategory;

	private bool _showNewDesignsFirst;

	private bool _showNewDesignsFirstOption;

	private int _totalResults;

	private int _minimumSearchResultSize;

	private Visibility _resultsVisibility;

	private bool _isDesignShapesVisible;

	private bool _isShowingChainResults;

	private int _pageNumber;

	private int _numberOfPages;

	private bool _showNextPageButton;

	private bool _showPreviousPageButton;

	private int _pageSize;

	private List<int> _pages;

	private RelayCommand<int> _commandGotoPage;

	private bool _showLastPageButton;

	private bool _showFirstPageButton;

	private GridLength _sessionPicksRowHeight;

	private bool _isSessionPicksOpen;

	private static GridLength _sessionPicksRowHeightOpen = new GridLength(240.0);

	private static GridLength _sessionPicksRowHeightClosed = new GridLength(49.0);

	private ObservableCollection<DesignSearch> _sessionPickDesigns;

	public BitmapSource DesignDetailImageFront => SelectedDesignSearch?.ImageFront;

	public BitmapSource DesignDetailImagePerspective => SelectedDesignSearch?.ImagePerspective;

	public BitmapSource DesignDetailImageSide => SelectedDesignSearch?.ImageSide;

	public BitmapSource DesignDetailImageTop => SelectedDesignSearch?.ImageTop;

	public bool IsDesignSelected => SelectedDesignSearch != null;

	public bool HasDesignsMarked
	{
		get
		{
			if (SessionPickDesigns != null)
			{
				return SessionPickDesigns.Any();
			}
			return false;
		}
	}

	public bool IsSessionPicksOpen
	{
		get
		{
			return _isSessionPicksOpen;
		}
		set
		{
			if (value)
			{
				SessionPicksRowHeight = _sessionPicksRowHeightOpen;
			}
			else
			{
				SessionPicksRowHeight = _sessionPicksRowHeightClosed;
			}
			_isSessionPicksOpen = value;
			RaisePropertyChanged("IsSessionPicksOpen");
		}
	}

	public RelayCommand CommandToggleSessionPicks { get; set; }

	public UIElement RootElement
	{
		get
		{
			if (base.IsInDesignMode)
			{
				return null;
			}
			return (UIElement)(Application.Current.MainWindow?.Content);
		}
	}

	public bool ShowSPIToolTipNumber
	{
		get
		{
			if (UIHelper.CounterSketchOptions != null && UIHelper.CounterSketchOptions.ShowSPIToolTipNumber)
			{
				return true;
			}
			return false;
		}
	}

	public Visibility SPINumberVisibility
	{
		get
		{
			if (ViewModelBase.IsInDesignModeStatic)
			{
				return Visibility.Visible;
			}
			if (UIHelper.CounterSketchOptions != null && UIHelper.CounterSketchOptions.ShowModelNumber)
			{
				return Visibility.Visible;
			}
			return Visibility.Collapsed;
		}
	}

	public Thickness SectionMargin
	{
		get
		{
			if (IsShowingPartSearchResults)
			{
				return new Thickness(-1.0 * SectionWidth, 0.0, 0.0, 0.0);
			}
			return new Thickness(0.0, 0.0, 0.0, 0.0);
		}
	}

	public int SessionPickDesignCount
	{
		get
		{
			if (SessionPickDesigns == null)
			{
				return 0;
			}
			return SessionPickDesigns.Count();
		}
	}

	public ObservableCollection<DesignSearch> SessionPickDesigns
	{
		get
		{
			return _sessionPickDesigns;
		}
		set
		{
			_sessionPickDesigns = value;
			RaisePropertyChanged("SessionPickDesigns");
		}
	}

	public int FirstDesignIndexOnPage
	{
		get
		{
			if (PageNumber == 1)
			{
				return 1;
			}
			return PageNumber * PageSize - PageSize + 1;
		}
	}

	public int LastDesignIndexOnPage
	{
		get
		{
			if (SearchResults == null)
			{
				return 0;
			}
			int num = PageNumber * PageSize;
			if (num > SearchResults.Count)
			{
				num = SearchResults.Count;
			}
			return num;
		}
	}

	public Visibility ResultsVisibility
	{
		get
		{
			return _resultsVisibility;
		}
		set
		{
			_resultsVisibility = value;
			RaisePropertyChanged("ResultsVisibility");
		}
	}

	public bool IsSyncingResultsWithPortfolio { get; set; }

	public bool ShowNewDesignsFirstOption
	{
		get
		{
			return _showNewDesignsFirstOption;
		}
		set
		{
			if (_showNewDesignsFirstOption != value)
			{
				_showNewDesignsFirstOption = value;
				ShowNewDesignsFirst = false;
				ResetSearchActiveSearchFilters();
				RaisePropertyChanged("ShowNewDesignsFirstOption");
			}
		}
	}

	public bool ShowNewDesignsFirst
	{
		get
		{
			return _showNewDesignsFirst;
		}
		set
		{
			if (UIHelper.CounterSketchOptions.ShowNewDesignsFirst != value)
			{
				UIHelper.CounterSketchOptions.ShowNewDesignsFirst = value;
				UIHelper.CounterSketchOptions.WriteSettings();
			}
			_showNewDesignsFirst = value;
			RaisePropertyChanged("ShowNewDesignsFirst");
		}
	}

	public RelayCommand CommandGotoFirstPage { get; set; }

	public RelayCommand CommandGotoPreviousPage { get; set; }

	public RelayCommand<DesignSearch> CommandRemoveFromSessionPicks { get; private set; }

	public RelayCommand CommandGotoNextPage { get; set; }

	public RelayCommand CommandGotoLastPage { get; set; }

	public bool ShowFirstPageButton
	{
		get
		{
			return _showFirstPageButton;
		}
		set
		{
			_showFirstPageButton = value;
			RaisePropertyChanged("ShowFirstPageButton");
		}
	}

	public bool ShowLastPageButton
	{
		get
		{
			return _showLastPageButton;
		}
		set
		{
			_showLastPageButton = value;
			RaisePropertyChanged("ShowLastPageButton");
		}
	}

	public bool ShowNextPageButton
	{
		get
		{
			return _showNextPageButton;
		}
		set
		{
			_showNextPageButton = value;
			RaisePropertyChanged("ShowNextPageButton");
		}
	}

	public bool ShowPreviousPageButton
	{
		get
		{
			return _showPreviousPageButton;
		}
		set
		{
			_showPreviousPageButton = value;
			RaisePropertyChanged("ShowPreviousPageButton");
		}
	}

	public RelayCommand<ScrollViewer> CommandPreviousSessionDesign { get; set; }

	public RelayCommand<ScrollViewer> CommandNextSessionDesign { get; set; }

	public RelayCommand CommandAddMarkedDesignsToActiveProject { get; set; }

	public RelayCommand CommandAddMarkedDesignsToCollection { get; set; }

	public RelayCommand CommandClearMarkedDesigns { get; set; }

	public RelayCommand<string> CommandOpenDesignWithSelectedShape { get; set; }

	public RelayCommand CommandCloseDesignInfo { get; set; }

	public RelayCommand CommandGemSearchFilterSizeChanged { get; set; }

	public RelayCommand CommandNextDesignInfo { get; set; }

	public RelayCommand CommandPreviousDesignInfo { get; set; }

	public RelayCommand<DesignIteration> CommandOpenDesignFromDesignIteration { get; set; }

	public RelayCommand<DesignSearch> CommandOpenDesignFromDesignSearch { get; set; }

	public RelayCommand<DesignSearch> CommandSelectDesignSearch { get; set; }

	public RelayCommand CommandResetResults { get; set; }

	public RelayCommand CommandResetSearchActiveSearchFilters { get; set; }

	public RelayCommand CommandSelectedDesignSearchChanged { get; set; }

	public RelayCommand<SelectionChangedEventArgs> CommandSelectedVariantSearchChanged { get; set; }

	public RelayCommand<string> CommandSwitchDesignDetailImage { get; set; }

	public RelayCommand<SelectionChangedEventArgs> CommandUpdateResultsWithSelectedFilterCriteria { get; set; }

	public RelayCommand CommandUpdateSearchFromFilter { get; set; }

	public ObservableCollection<DesignSearch> CurrentPageDesigns
	{
		get
		{
			return _currentPageDesigns;
		}
		set
		{
			_currentPageDesigns = value;
			RaisePropertyChanged("CurrentPageDesigns");
		}
	}

	public GridLength SessionPicksRowHeight
	{
		get
		{
			return _sessionPicksRowHeight;
		}
		set
		{
			_sessionPicksRowHeight = value;
			RaisePropertyChanged("SessionPicksRowHeight");
		}
	}

	public int PageNumber
	{
		get
		{
			return _pageNumber;
		}
		set
		{
			_pageNumber = value;
			RaisePropertyChanged("PageNumber");
		}
	}

	public int NumberOfPages
	{
		get
		{
			return _numberOfPages;
		}
		set
		{
			_numberOfPages = value;
			RaisePropertyChanged("NumberOfPages");
		}
	}

	public BitmapSource DesignDetailImage
	{
		get
		{
			if (_designDetailImage == null)
			{
				_designDetailImage = DesignDetailImagePerspective;
			}
			return _designDetailImage;
		}
		set
		{
			_designDetailImage = value;
			RaisePropertyChanged("DesignDetailImage");
		}
	}

	public RelayCommand CommandUpdateSearchResultImageSizes { get; set; }

	public int DesignImageWidth
	{
		get
		{
			return _designImageWidth;
		}
		set
		{
			if (_designImageWidth != value)
			{
				_designImageWidth = value;
				RaisePropertyChanged("DesignImageWidth");
			}
		}
	}

	public string GemSearchFilterLength
	{
		get
		{
			return _gemSearchFilterLength;
		}
		set
		{
			if (_gemSearchFilterLength != value)
			{
				_gemSearchFilterLength = value;
				RaisePropertyChanged("GemSearchFilterLength");
			}
		}
	}

	public string GemSearchFilterWidth
	{
		get
		{
			return _gemSearchFilterWidth;
		}
		set
		{
			if (_gemSearchFilterWidth != value)
			{
				_gemSearchFilterWidth = value;
				RaisePropertyChanged("GemSearchFilterWidth");
			}
		}
	}

	public bool IsIterationAvailable
	{
		get
		{
			return _isIterationAvailable;
		}
		set
		{
			_isIterationAvailable = value;
			RaisePropertyChanged("IsIterationAvailable");
		}
	}

	public bool IsPackageSearch
	{
		get
		{
			return _isPackageSearch;
		}
		set
		{
			_isPackageSearch = value;
			RaisePropertyChanged("IsPackageSearch");
		}
	}

	public bool IsEverAndEverPackage
	{
		get
		{
			return _isEverAndEverPackage;
		}
		set
		{
			_isEverAndEverPackage = value;
			RaisePropertyChanged("IsEverAndEverPackage");
		}
	}

	public string LastUsedPackageName
	{
		get
		{
			return _lastUsedPackageName;
		}
		set
		{
			_lastUsedPackageName = value;
			RaisePropertyChanged("LastUsedPackageName");
		}
	}

	public int NumberOfSelectedAdvancedFilters
	{
		get
		{
			return _numberOfSelectedAdvancedFilters;
		}
		set
		{
			if (_numberOfSelectedAdvancedFilters != value)
			{
				_numberOfSelectedAdvancedFilters = value;
				RaisePropertyChanged("NumberOfSelectedAdvancedFilters");
			}
		}
	}

	public bool IsTopView
	{
		get
		{
			return _isTopView;
		}
		set
		{
			_isTopView = value;
			RaisePropertyChanged("IsTopView");
			if (value)
			{
				UIHelper.CurrentSearchResultImageType = ImageType.Top;
				IsSideView = false;
				IsFrontView = false;
				IsPerspectiveView = false;
			}
		}
	}

	public bool IsFrontView
	{
		get
		{
			return _isFrontView;
		}
		set
		{
			_isFrontView = value;
			RaisePropertyChanged("IsFrontView");
			if (value)
			{
				UIHelper.CurrentSearchResultImageType = ImageType.Front;
				IsSideView = false;
				IsTopView = false;
				IsPerspectiveView = false;
			}
		}
	}

	public bool IsSideView
	{
		get
		{
			return _isSideView;
		}
		set
		{
			_isSideView = value;
			RaisePropertyChanged("IsSideView");
			if (value)
			{
				UIHelper.CurrentSearchResultImageType = ImageType.Side;
				IsFrontView = false;
				IsTopView = false;
				IsPerspectiveView = false;
			}
		}
	}

	public bool IsPerspectiveView
	{
		get
		{
			return _isPerspectiveView;
		}
		set
		{
			_isPerspectiveView = value;
			RaisePropertyChanged("IsPerspectiveView");
			if (value)
			{
				UIHelper.CurrentSearchResultImageType = ImageType.Perspective;
				IsSideView = false;
				IsFrontView = false;
				IsTopView = false;
			}
		}
	}

	public ObservableCollection<SearchFilterViewModel> SearchFilters
	{
		get
		{
			return _searchFilters;
		}
		set
		{
			if (_searchFilters != value)
			{
				_searchFilters = value;
				RaisePropertyChanged("SearchFilters");
			}
		}
	}

	public ObservableCollection<DesignSearch> SearchResults
	{
		get
		{
			return _searchResults;
		}
		set
		{
			if (value == null)
			{
				return;
			}
			if (base.IsInDesignMode)
			{
				IOrderedEnumerable<DesignSearch> collection = from design in value
					orderby design.DefaultStyleElementOrder, design.DefaultStyleOrder, design.DefaultSettingTypeOrder, design.SecondaryStyleElementOrder, design.SPINumber
					select design;
				_searchResults = new ObservableCollection<DesignSearch>(collection);
				return;
			}
			if (ShowNewDesignsFirst)
			{
				IOrderedEnumerable<DesignSearch> collection2 = from design in value
					orderby design.IsNewDesign != 1, design.DefaultStyleElementOrder, design.DefaultStyleOrder, design.DefaultSettingTypeOrder, design.SecondaryStyleElementOrder, design.SPINumber
					select design;
				_searchResults = new ObservableCollection<DesignSearch>(collection2);
			}
			else
			{
				IOrderedEnumerable<DesignSearch> collection3 = from design in value
					orderby design.DefaultStyleElementOrder, design.DefaultStyleOrder, design.DefaultSettingTypeOrder, design.SecondaryStyleElementOrder, design.SPINumber
					select design;
				_searchResults = new ObservableCollection<DesignSearch>(collection3);
			}
			SyncSearchResultsWithPortfolioSearchFavorites();
			RaisePropertyChanged(() => SearchResults);
			ViewModelLocator.Current.MainVM.PopulateDebugInfo($"Search Results: {SearchResults?.Count}, Loaded Results: {CurrentPageDesigns?.Count}");
			IsShowingChainResults = false;
			if (SelectedSearchTileCategory?.Title != null && SelectedSearchTileCategory.Title.Contains("Chain") && _searchResults.Any((DesignSearch _) => _.IsFreeHandPart == 1))
			{
				IsShowingChainResults = true;
			}
			SessionPickDesigns?.ToList().ForEach(delegate(DesignSearch d)
			{
				_searchResults.FirstOrDefault((DesignSearch _) => _.DesignId == d.DesignId)?.SetIsMarkedWithoutSearchResultUpdate(marked: true);
			});
			if (_searchResults.Count > 0)
			{
				PageNumber = 1;
				NumberOfPages = (int)Math.Ceiling((double)_searchResults.Count / (double)PageSize);
				PopulatePageResults();
			}
			else
			{
				ShowResultsOrMessage(resultsVisible: false);
			}
		}
	}

	public RelayCommand<int> CommandGotoPage
	{
		get
		{
			if (_commandGotoPage == null)
			{
				_commandGotoPage = new RelayCommand<int>(GotoPage);
			}
			return _commandGotoPage;
		}
		set
		{
			_commandGotoPage = value;
		}
	}

	public List<int> Pages
	{
		get
		{
			return _pages;
		}
		set
		{
			_pages = value;
			RaisePropertyChanged("Pages");
		}
	}

	public int PageSize
	{
		get
		{
			return _pageSize;
		}
		set
		{
			if (_pageSize != value)
			{
				_pageSize = value;
				RaisePropertyChanged("PageSize");
				PageNumber = 1;
				if (this is TileSearchResultDesignsViewModel)
				{
					UIHelper.CounterSketchOptions.SearchResultPageSizeDesigns = value;
				}
				else
				{
					UIHelper.CounterSketchOptions.SearchResultPageSizeParts = value;
				}
				UIHelper.CounterSketchOptions.WriteSettings();
				PopulatePageResults();
			}
		}
	}

	public bool IsDesignShapesVisible
	{
		get
		{
			return _isDesignShapesVisible;
		}
		set
		{
			_isDesignShapesVisible = value;
			RaisePropertyChanged("IsDesignShapesVisible");
		}
	}

	public Collection<string> SelectedDesignSearchShapes
	{
		get
		{
			return _selectedDesignSearchShapes;
		}
		set
		{
			_selectedDesignSearchShapes = value;
			RaisePropertyChanged("SelectedDesignSearchShapes");
			if (value.Any())
			{
				IsDesignShapesVisible = true;
			}
			else
			{
				IsDesignShapesVisible = false;
			}
		}
	}

	public DesignSearch SelectedDesignSearch
	{
		get
		{
			return _selectedDesignSearch;
		}
		set
		{
			if (_selectedDesignSearch == value)
			{
				return;
			}
			_selectedDesignSearch = value;
			RaisePropertyChanged("SelectedDesignSearch");
			if (_selectedDesignSearch == null)
			{
				return;
			}
			List<long> shapeSequence = new long[9] { 21L, 16L, 5L, 8L, 9L, 18L, 13L, 17L, 11L }.ToList();
			if (!base.IsInDesignMode)
			{
				Task.Run(delegate
				{
					SelectedDesignSearchShapes = new Collection<string>();
					long designId = value.DesignId;
					using CounterSketchLibraryDataEntities1 counterSketchLibraryDataEntities = new CounterSketchLibraryDataEntities1(CSLibraryDAL.NameOfConnectionString);
					SelectedDesignSearchShapes = new Collection<string>((from d in counterSketchLibraryDataEntities.DesignGemTables.Where((DesignGemTable d) => d.DesignId == designId).ToList()
						orderby shapeSequence.IndexOf(d.GemTableShapeSize.GemTableShapeId)
						select SearchHelper.GetGemShapeFromGemTableShapeId(d.GemTableShapeSize.GemTableShapeId)).Distinct().ToList());
				}).ContinueWith(delegate
				{
					Thread.Sleep(200);
					RaisePropertyChanged("IsDesignSelected");
				});
			}
			_selectedDesignSearch.UpdateAllImageTypes();
			RaisePropertyChanged("DesignDetailImagePerspective");
			RaisePropertyChanged("DesignDetailImageFront");
			RaisePropertyChanged("DesignDetailImageSide");
			RaisePropertyChanged("DesignDetailImageTop");
			SwitchDesignDetailImage(_currentDetailImageView);
			RaisePropertyChanged("DesignDetailImage");
		}
	}

	public ObservableCollection<DesignIteration> SelectedDesignSearchFeaturedIterations
	{
		get
		{
			return _selectedDesignSearchFeaturedIterations;
		}
		set
		{
			if (_selectedDesignSearchFeaturedIterations != value)
			{
				_selectedDesignSearchFeaturedIterations = value;
				RaisePropertyChanged("SelectedDesignSearchFeaturedIterations");
			}
		}
	}

	public string SelectedDesignSearchKeywords
	{
		get
		{
			if (base.IsInDesignMode)
			{
				return "This, Keyword, Is, Very, Long, So, Don't, Expect Much, To Appear, Below This, Word.";
			}
			if (SelectedDesignSearch != null)
			{
				return _selectedDesignSearchKeywords;
			}
			return string.Empty;
		}
		set
		{
			if (_selectedDesignSearchKeywords != value)
			{
				_selectedDesignSearchKeywords = value;
				RaisePropertyChanged("SelectedDesignSearchKeywords");
			}
		}
	}

	public ObservableCollection<DesignSearch> SelectedDesignSearchVariants
	{
		get
		{
			return _selectedDesignSearchVariants;
		}
		set
		{
			if (_selectedDesignSearchVariants != value)
			{
				_selectedDesignSearchVariants = value;
				RaisePropertyChanged("SelectedDesignSearchVariants");
			}
		}
	}

	public TileSearchCategory SelectedSearchTileCategory
	{
		get
		{
			return _selectedSearchTileCategory;
		}
		set
		{
			if (_selectedSearchTileCategory != value)
			{
				_selectedSearchTileCategory = value;
				RaisePropertyChanged("SelectedSearchTileCategory");
			}
		}
	}

	public int TotalResults
	{
		get
		{
			return _totalResults;
		}
		set
		{
			_totalResults = value;
			RaisePropertyChanged("TotalResults");
		}
	}

	public RelayCommand CommandTriggerSearchRefresh { get; set; }

	public string SelectedCenterShapeName { get; set; }

	public RelayCommand CommandAddDesignFromDesignDetailsToActiveProject { get; set; }

	public double SectionWidth
	{
		get
		{
			return _sectionWidth;
		}
		set
		{
			_sectionWidth = value;
			RaisePropertyChanged("SectionWidth");
			RaisePropertyChanged(() => SectionMargin);
		}
	}

	public bool IsShowingPartSearchResults
	{
		get
		{
			return _isShowingPartSearchResults;
		}
		set
		{
			_isShowingPartSearchResults = value;
			RaisePropertyChanged("IsShowingPartSearchResults");
		}
	}

	public bool IsShowingChainResults
	{
		get
		{
			return _isShowingChainResults;
		}
		set
		{
			_isShowingChainResults = value;
			RaisePropertyChanged("IsShowingChainResults");
		}
	}

	public bool ScrollSection
	{
		get
		{
			return _scrollSection;
		}
		set
		{
			_scrollSection = value;
			RaisePropertyChanged("ScrollSection");
			RaisePropertyChanged(() => SectionMargin);
		}
	}

	public RelayCommand CommandNavigateBackToTiles { get; set; }

	public RelayCommand CommandNavigateForwardToResults { get; set; }

	public bool IsBusyLoadingImages { get; set; }

	public bool IsFilterModeHasGem
	{
		get
		{
			return _isFilterModeHasGem;
		}
		set
		{
			_isFilterModeHasGem = value;
			RaisePropertyChanged("IsFilterModeHasGem");
			if (value)
			{
				IsFilterModeAllParts = false;
				IsFilterModeCounterPartsOnly = false;
				IsFilterModeStaticPartsOnly = false;
			}
		}
	}

	public bool IsFilterModeAllParts
	{
		get
		{
			return _isFilterModeAllParts;
		}
		set
		{
			_isFilterModeAllParts = value;
			RaisePropertyChanged("IsFilterModeAllParts");
			if (value)
			{
				IsFilterModeCounterPartsOnly = false;
				IsFilterModeHasGem = false;
				IsFilterModeStaticPartsOnly = false;
			}
		}
	}

	public bool IsFilterModeCounterPartsOnly
	{
		get
		{
			return _isFilterModeCounterPartsOnly;
		}
		set
		{
			_isFilterModeCounterPartsOnly = value;
			RaisePropertyChanged("IsFilterModeCounterPartsOnly");
			if (value)
			{
				IsFilterModeAllParts = false;
				IsFilterModeHasGem = false;
				IsFilterModeStaticPartsOnly = false;
			}
		}
	}

	public bool IsFilterModeStaticPartsOnly
	{
		get
		{
			return _isFilterModeStaticPartsOnly;
		}
		set
		{
			_isFilterModeStaticPartsOnly = value;
			RaisePropertyChanged("IsFilterModeStaticPartsOnly");
			if (value)
			{
				IsFilterModeAllParts = false;
				IsFilterModeCounterPartsOnly = false;
				IsFilterModeHasGem = false;
			}
		}
	}

	public int MinimumSearchResultSize
	{
		get
		{
			return _minimumSearchResultSize;
		}
		set
		{
			_minimumSearchResultSize = value;
			RaisePropertyChanged("MinimumSearchResultSize");
		}
	}

	protected ObservableCollection<DesignSearch> BaseSearchResults { get; set; }

	public TileSearchResultViewModelBase()
	{
		Populate();
	}

	public void ToggleSessionPicks()
	{
		IsSessionPicksOpen = !IsSessionPicksOpen;
	}

	public void NextSessionDesign(ScrollViewer scrollviewer)
	{
		scrollviewer?.PageRight();
	}

	public void PreviousSessionDesign(ScrollViewer scrollviewer)
	{
		scrollviewer?.PageLeft();
	}

	public void ClearIsVariantDesignIncludedInResultsFlag()
	{
		_isVariantDesignIncludedInResults = false;
	}

	public void ClearSearchResults()
	{
		ResetImageUpdateTasks();
		_searchResults = new ObservableCollection<DesignSearch>();
		CurrentPageDesigns = new ObservableCollection<DesignSearch>();
		PageNumber = 1;
	}

	public void CloseDesignInfo()
	{
		SelectedDesignSearch = null;
		RaisePropertyChanged("IsDesignSelected");
	}

	public void GemSearchFilterSizeChanged()
	{
		if (double.TryParse(GemSearchFilterLength, NumberStyles.Any, CultureInfo.InvariantCulture, out var result) && double.TryParse(GemSearchFilterWidth, NumberStyles.Any, CultureInfo.InvariantCulture, out var result2))
		{
			_gemSearchFilterSize.Height = result;
			_gemSearchFilterSize.Width = result2;
			UpdateSearchResults(new SearchFilterViewModel
			{
				Title = "Center Stone Size"
			});
		}
		else
		{
			_gemSearchFilterSize.Height = 0.0;
			_gemSearchFilterSize.Width = 0.0;
		}
	}

	private int GetAttributeIdFromDesignElementName(string elementName)
	{
		int result = 0;
		switch (elementName)
		{
		case "classic":
			result = 57;
			break;
		case "vintage":
			result = 58;
			break;
		case "contemporary":
			result = 59;
			break;
		case "european":
			result = 187;
			break;
		case "art deco":
			result = 292;
			break;
		case "art nouveau":
			result = 293;
			break;
		case "glamorous":
			result = 295;
			break;
		case "modern":
			result = 296;
			break;
		}
		return result;
	}

	public void GetDesignSearchResultsByDesignElement(string elementName, string secondaryElementName = null)
	{
		int firstAttribute = GetAttributeIdFromDesignElementName(elementName);
		int secondAttribute = -1;
		if (secondaryElementName != null)
		{
			secondAttribute = GetAttributeIdFromDesignElementName(secondaryElementName);
		}
		SearchFilterViewModel searchFilterViewModel = SearchFilters.FirstOrDefault((SearchFilterViewModel sf) => sf.Title == "Style");
		if (searchFilterViewModel == null)
		{
			return;
		}
		IEnumerable<SearchFilterItem> enumerable = searchFilterViewModel.SearchFilterItems.Where((SearchFilterItem sfi) => sfi.SearchAttributeId == firstAttribute || sfi.SearchAttributeId == secondAttribute);
		if (enumerable != null && enumerable.Any())
		{
			enumerable.ForEach(delegate(SearchFilterItem fi)
			{
				fi.IsSelected = true;
			});
			searchFilterViewModel.SelectedSearchFilterItem = enumerable.FirstOrDefault();
			UpdateSearchResults(searchFilterViewModel);
		}
	}

	public void GetDesignSearchResultsByKeyword(string keyWord, bool noUpdate = false)
	{
		_lastUsedKeyword = keyWord;
		if (string.IsNullOrWhiteSpace(keyWord))
		{
			return;
		}
		ClearSearchResults();
		SearchResults = new ObservableCollection<DesignSearch>(SearchHelper.GetDesignsForKeyword(keyWord));
		EventHelper.RecordEvent(EventHelper.EventTypes.Search_Keyword, "Library:Criteria:Count", $"{UIHelper.CounterSketchOptions.LibraryVersion}:{keyWord}:{SearchResults.Count}");
		BaseSearchResults = new ObservableCollection<DesignSearch>(SearchResults);
		SelectedSearchTileCategory = ViewModelLocator.Current.TileSearchMainVM.TileSearchCategoryVM.TileSearchCategories.FirstOrDefault((TileSearchCategory tile) => tile.Id == -1);
		if (!noUpdate)
		{
			Task.Run(delegate
			{
				ResetFilters();
			});
		}
		IsKeywordSearch = true;
		IsPackageSearch = false;
	}

	public void GetDesignSearchResultsByPackageKitName(string selectedPackageName, bool noUpdate = false)
	{
		LastUsedPackageName = selectedPackageName;
		if (string.IsNullOrWhiteSpace(selectedPackageName))
		{
			return;
		}
		ClearSearchResults();
		SearchResults = new ObservableCollection<DesignSearch>(SearchHelper.GetDesignsForPackageName(selectedPackageName));
		BaseSearchResults = new ObservableCollection<DesignSearch>(SearchResults);
		if (selectedPackageName.Contains("Ever & Ever Bridal Collection"))
		{
			SelectedSearchTileCategory = ViewModelLocator.Current.TileSearchMainVM.TileSearchCategoryVM.TileSearchCategories.FirstOrDefault((TileSearchCategory tile) => tile.Id == 19);
		}
		else
		{
			SelectedSearchTileCategory = ViewModelLocator.Current.TileSearchMainVM.TileSearchCategoryVM.TileSearchCategories.FirstOrDefault((TileSearchCategory tile) => tile.Id == 15);
		}
		if (!noUpdate)
		{
			Task.Run(delegate
			{
				ResetFilters();
			});
		}
		IsPackageSearch = true;
		IsKeywordSearch = false;
	}

	public void GetDesignSearchResultsBySPINumber(string spiNumber)
	{
		DesignSearch designBySPINumber = SearchHelper.GetDesignBySPINumber(spiNumber);
		if (designBySPINumber != null)
		{
			SearchResults = new ObservableCollection<DesignSearch>(new List<DesignSearch> { designBySPINumber });
			GemSearchFilterLength = null;
			GemSearchFilterWidth = null;
			Task.Run(delegate
			{
				ResetFilters();
			});
		}
		else
		{
			CurrentPageDesigns = new ObservableCollection<DesignSearch>();
			SearchResults = new ObservableCollection<DesignSearch>();
		}
	}

	public virtual void GetDesignSearchResultsByTileCategory(bool noUpdate = false)
	{
	}

	public void GotoPage(int page)
	{
		PageNumber = page;
		PopulatePageResults();
	}

	public void NavigateBackToTiles()
	{
		IsShowingPartSearchResults = false;
		ScrollSection = true;
		ScrollSection = false;
		ScrollSection = true;
		ScrollSection = false;
		ViewModelLocator.Current.TileSearchMainVM.TileSearchResultPartsVM.ResetFilters(isDefaultingMaterials: false);
	}

	public void NavigateForwardToResults()
	{
		CloseDesignInfo();
		ViewModelLocator.Current.TileSearchMainVM.IsSearchingDesigns = false;
		switch (UIHelper.CurrentSearchResultImageType)
		{
		case ImageType.Perspective:
			IsPerspectiveView = true;
			break;
		case ImageType.Top:
			IsTopView = true;
			break;
		case ImageType.Side:
			IsSideView = true;
			break;
		case ImageType.Front:
			IsFrontView = true;
			break;
		}
		IsShowingPartSearchResults = true;
		ScrollSection = true;
		ScrollSection = false;
		ScrollSection = true;
		ScrollSection = false;
	}

	public void Reset()
	{
		IsEverAndEverPackage = false;
		ResetSearchActiveSearchFilters();
		ResetResults();
		ClearMarkedDesigns();
	}

	public void ResetFilters(bool isDefaultingMaterials = true)
	{
		IsFilterModeAllParts = true;
		if (isDefaultingMaterials)
		{
			ViewModelLocator.Current.StudioMaterialSelectionVM.DefaultMaterialGroupOptions();
		}
		CreateSearchFilters();
		UpdateDesignMaterials();
		if (ViewModelLocator.Current.StoneTrayVM.IsStoneAssigned)
		{
			ViewModelLocator.Current.StoneTrayVM.StoneChosen = ViewModelLocator.Current.StoneTrayVM.StoneChosen;
		}
	}

	public static void ResetImageUpdateTasks()
	{
		if (_cancellationTokenSource != null)
		{
			_cancellationTokenSource.Cancel();
		}
		_cancellationTokenSource = new CancellationTokenSource();
	}

	public void ShowResultsOrMessage(bool resultsVisible)
	{
		if (resultsVisible)
		{
			ResultsVisibility = Visibility.Visible;
		}
		else
		{
			ResultsVisibility = Visibility.Collapsed;
		}
	}

	public void SwitchDesignDetailImage(string imageToShow)
	{
		_currentDetailImageView = imageToShow;
		switch (imageToShow)
		{
		case "P":
			DesignDetailImage = DesignDetailImagePerspective;
			break;
		case "F":
			DesignDetailImage = DesignDetailImageFront;
			break;
		case "S":
			DesignDetailImage = DesignDetailImageSide;
			break;
		case "T":
			DesignDetailImage = DesignDetailImageTop;
			break;
		}
	}

	public void SyncSearchResultsWithPortfolioSearchFavorites()
	{
		IsSyncingResultsWithPortfolio = true;
		if (SearchResults != null)
		{
			SearchResults.ForEach(delegate(DesignSearch designSearch)
			{
				designSearch.IsSearchFavorite = ProjectManagementHelper.ActiveProject.SearchFavoriteDesigns.Any((GV.CounterSketch.Studio.Model.Objects.Design d) => d.DesignId == designSearch.DesignId);
			});
		}
		IsSyncingResultsWithPortfolio = false;
	}

	public void UpdateDesignMarkedStatus()
	{
		if (SessionPickDesigns == null)
		{
			SessionPickDesigns = new ObservableCollection<DesignSearch>();
		}
		if (SearchResults != null)
		{
			List<DesignSearch> source = SearchResults.Where((DesignSearch _) => _.IsMarked).ToList();
			List<DesignSearch> unmarkedDesigns = SearchResults.Where((DesignSearch _) => !_.IsMarked).ToList();
			source.Where((DesignSearch _) => SessionPickDesigns.All((DesignSearch d) => d.DesignId != _.DesignId)).ToList().ForEach(delegate(DesignSearch d)
			{
				DesignSearch shallowCopy = d.GetShallowCopy();
				SessionPickDesigns.Add(shallowCopy);
			});
			SessionPickDesigns.Where((DesignSearch _) => unmarkedDesigns.Any((DesignSearch d) => d.DesignId == _.DesignId)).ToList().ForEach(delegate(DesignSearch _)
			{
				SessionPickDesigns.Remove(_);
			});
		}
		RaisePropertyChanged("HasDesignsMarked");
		RaisePropertyChanged("SessionPickDesigns");
		RaisePropertyChanged("SessionPickDesignCount");
		if (!HasDesignsMarked)
		{
			IsSessionPicksOpen = false;
		}
	}

	public async void UpdateDesignMaterials()
	{
		GlobalErrorHandler.WriteMethodInfo("UpdateDesignMaterials");
		Stopwatch.StartNew();
		ViewModelLocator.Current.StudioMaterialSelectionVM.UpdateSelectedMaterialViewModelProperties();
		if (_searchResults != null && _searchResults.Count > 0)
		{
			UpdateSearchResultImages();
		}
		if (SelectedDesignSearch != null)
		{
			SelectedDesignSearch.UpdateImageSource();
			RaisePropertyChanged("DesignDetailImagePerspective");
			RaisePropertyChanged("DesignDetailImageFront");
			RaisePropertyChanged("DesignDetailImageSide");
			RaisePropertyChanged("DesignDetailImageTop");
			SwitchDesignDetailImage(_currentDetailImageView);
			RaisePropertyChanged("DesignDetailImage");
		}
	}

	public void UpdateSearchFiltersUsingAssignedStone(bool overrideSearchActiveRequirements = false)
	{
		if (_searchFilters == null)
		{
			return;
		}
		SearchFilterViewModel searchFilterViewModel = _searchFilters.FirstOrDefault((SearchFilterViewModel _) => _.Title == "Center Shape");
		if (!ViewModelLocator.Current.StoneTrayVM.IsStoneAssigned)
		{
			return;
		}
		Diamond stoneChosen = ViewModelLocator.Current.StoneTrayVM.StoneChosen;
		int selectedShapeAttribute = GetGemAttibuteFromStullerShape(stoneChosen.Shape);
		if ((ViewModelLocator.Current.MainVM.IsSearchModeActive || overrideSearchActiveRequirements) && searchFilterViewModel != null)
		{
			searchFilterViewModel.SelectedSearchFilterItem = searchFilterViewModel.SearchFilterItems.FirstOrDefault((SearchFilterItem _) => _.SearchAttributeId == selectedShapeAttribute);
			_gemSearchFilterLength = stoneChosen.Length.ToString(CultureInfo.InvariantCulture);
			RaisePropertyChanged("GemSearchFilterLength");
			GemSearchFilterWidth = ((stoneChosen.Width.ToString(CultureInfo.InvariantCulture) != "0.00") ? stoneChosen.Width.ToString(CultureInfo.InvariantCulture) : stoneChosen.Length.ToString(CultureInfo.InvariantCulture));
			if (searchFilterViewModel.SelectedSearchFilterItem != null)
			{
				searchFilterViewModel.SelectedSearchFilterItem.IsSelected = true;
			}
		}
	}

	public void UpdateSearchResultImageSizes()
	{
		if (UIHelper.CounterSketchOptions != null)
		{
			if (this is TileSearchResultPartsViewModel)
			{
				UIHelper.CounterSketchOptions.SearchResultPartWidth = _designImageWidth;
			}
			else
			{
				UIHelper.CounterSketchOptions.SearchResultDesignWidth = _designImageWidth;
			}
			UIHelper.CounterSketchOptions.WriteSettings();
		}
		UpdateSearchResultImages();
	}

	public void UpdateSearchResults(SearchFilterViewModel searchFilterViewModel)
	{
		if (ViewModelLocator.Current.TileSearchMainVM.IsSearchingDesigns)
		{
			UIHelper.ShowWaitWindow(waitVisible: true, Strings.UpdatingSearchResults);
		}
		else
		{
			ViewModelLocator.Current.TileViewPartsVM.IsBusySearching = true;
		}
		Stopwatch.StartNew();
		switch (searchFilterViewModel.Title)
		{
		case "Center Shape":
			EventHelper.RecordEvent(EventHelper.EventTypes.Search_CenterShape, "Shape", $"{searchFilterViewModel.SelectedSearchFilterItem?.PrimaryText}");
			break;
		case "Center Setting":
			EventHelper.RecordEvent(EventHelper.EventTypes.Search_CenterSetting, "Setting", $"{searchFilterViewModel.SelectedSearchFilterItem?.PrimaryText}");
			break;
		case "Side Shape":
			EventHelper.RecordEvent(EventHelper.EventTypes.Search_SideShape, "Shape", $"{searchFilterViewModel.SelectedSearchFilterItem?.PrimaryText}");
			break;
		case "Side Setting":
			EventHelper.RecordEvent(EventHelper.EventTypes.Search_SideSetting, "Setting", $"{searchFilterViewModel.SelectedSearchFilterItem?.PrimaryText}");
			break;
		case "Style":
			EventHelper.RecordEvent(EventHelper.EventTypes.Search_Style, "Style", $"{searchFilterViewModel.SelectedSearchFilterItem?.PrimaryText}");
			break;
		case "Design Element":
			EventHelper.RecordEvent(EventHelper.EventTypes.Search_DesignElement, "DesignElement", $"{searchFilterViewModel.SelectedSearchFilterItem?.PrimaryText}");
			break;
		}
		SearchFilterItem searchFilterItem = null;
		SearchFilterViewModel searchFilterViewModel2 = SearchFilters.FirstOrDefault((SearchFilterViewModel filter) => filter.Title == "Design Element" && filter.SelectedSearchFilterItem != null);
		if (searchFilterViewModel2 != null && searchFilterViewModel2.SelectedSearchFilterItem != null)
		{
			searchFilterItem = searchFilterViewModel2.SelectedSearchFilterItem;
		}
		if (SearchFilters.Any((SearchFilterViewModel filter) => filter.Title == "Center Shape" && filter.SearchFilterItems.Any((SearchFilterItem _) => _.IsSelected)))
		{
			_isVariantDesignIncludedInResults = true;
			if (BaseSearchResults != null && BaseSearchResults.Count > 0)
			{
				if (SelectedSearchTileCategory == null)
				{
					UIHelper.ShowWaitWindow(waitVisible: false);
					return;
				}
				IEnumerable<DesignSearch> source;
				switch (SelectedSearchTileCategory.Id)
				{
				case 0:
					source = new ObservableCollection<DesignSearch>(SearchHelper.GetAllBaseRingDesigns(includeVariants: true));
					break;
				case 3:
					source = SearchHelper.GetPendantDesigns();
					break;
				case 2:
				case 4:
				case 5:
				case 6:
				case 7:
				case 8:
				case 9:
				case 10:
				case 11:
				{
					int collectionId = 0;
					switch (SelectedSearchTileCategory.Id)
					{
					case 2:
						collectionId = 170;
						break;
					case 4:
						collectionId = 5;
						break;
					case 5:
						collectionId = 53;
						break;
					case 6:
						collectionId = 263;
						break;
					case 7:
						collectionId = 207;
						break;
					case 8:
						collectionId = 49;
						break;
					case 9:
						collectionId = 78;
						break;
					case 10:
						collectionId = 272;
						break;
					case 11:
						collectionId = 55;
						break;
					}
					source = SearchHelper.GetDesignsByCollectionId(collectionId, includeVariants: true);
					break;
				}
				case 12:
					source = SearchHelper.Get3CDesigns(includeVariants: true);
					break;
				case 13:
					source = SearchHelper.GetThreeStoneDesigns(includeVariants: true);
					break;
				case 14:
					source = SearchHelper.GetMensDesigns(includeVariants: true);
					break;
				case 15:
					source = SearchHelper.GetPackagedDesigns(includeVariants: true);
					break;
				case 17:
					source = new ObservableCollection<DesignSearch>(SearchHelper.GetNewDesigns(includeVariants: true));
					break;
				default:
					source = BaseSearchResults;
					break;
				}
				if (ShowNewDesignsFirst)
				{
					IOrderedEnumerable<DesignSearch> collection = from design in source
						orderby design.IsNewDesign != 1, design.DefaultStyleElementOrder, design.DefaultStyleOrder, design.DefaultSettingTypeOrder, design.SecondaryStyleElementOrder, design.SPINumber
						select design;
					BaseSearchResults = new ObservableCollection<DesignSearch>(collection);
				}
				else
				{
					IOrderedEnumerable<DesignSearch> collection2 = from design in source
						orderby design.DefaultStyleElementOrder, design.DefaultStyleOrder, design.DefaultSettingTypeOrder, design.SecondaryStyleElementOrder, design.SPINumber
						select design;
					BaseSearchResults = new ObservableCollection<DesignSearch>(collection2);
				}
			}
		}
		else if (searchFilterViewModel.Title == "Center Shape" && searchFilterViewModel.SelectedSearchFilterItem != null && searchFilterViewModel.SelectedSearchFilterItem.PrimaryText == TranslationHelper.GetTranslatedString("Show All"))
		{
			if (BaseSearchResults != null && BaseSearchResults.Count > 0)
			{
				if (SelectedSearchTileCategory == null)
				{
					UIHelper.ShowWaitWindow(waitVisible: false);
					return;
				}
				IEnumerable<DesignSearch> source;
				switch (SelectedSearchTileCategory.Id)
				{
				case 0:
					source = new ObservableCollection<DesignSearch>(SearchHelper.GetAllBaseRingDesigns());
					break;
				case 3:
					source = SearchHelper.GetPendantDesigns();
					break;
				case 2:
				case 4:
				case 5:
				case 6:
				case 7:
				case 8:
				case 9:
				case 10:
				case 11:
				{
					int collectionId2 = 0;
					switch (SelectedSearchTileCategory.Id)
					{
					case 2:
						collectionId2 = 170;
						break;
					case 4:
						collectionId2 = 5;
						break;
					case 5:
						collectionId2 = 53;
						break;
					case 6:
						collectionId2 = 263;
						break;
					case 7:
						collectionId2 = 207;
						break;
					case 8:
						collectionId2 = 49;
						break;
					case 9:
						collectionId2 = 78;
						break;
					case 10:
						collectionId2 = 272;
						break;
					case 11:
						collectionId2 = 55;
						break;
					}
					source = SearchHelper.GetDesignsByCollectionId(collectionId2);
					break;
				}
				case 12:
					source = SearchHelper.Get3CDesigns();
					break;
				case 13:
					source = SearchHelper.GetThreeStoneDesigns();
					break;
				case 14:
					source = SearchHelper.GetMensDesigns();
					break;
				case 15:
					source = SearchHelper.GetPackagedDesigns();
					break;
				case 17:
					source = new ObservableCollection<DesignSearch>(SearchHelper.GetNewDesigns());
					break;
				default:
					source = BaseSearchResults;
					break;
				}
				if (ShowNewDesignsFirst)
				{
					IEnumerable<DesignSearch> collection3 = source.Where((DesignSearch _) => _.IsNewDesign == 1).Concat(source.Where((DesignSearch _) => _.IsNewDesign != 1));
					BaseSearchResults = new ObservableCollection<DesignSearch>(collection3);
				}
				else
				{
					BaseSearchResults = new ObservableCollection<DesignSearch>(source);
				}
				_isVariantDesignIncludedInResults = false;
			}
		}
		else if (searchFilterViewModel.Title == "Style" && BaseSearchResults == null)
		{
			BaseSearchResults = new ObservableCollection<DesignSearch>(SearchHelper.GetAllBaseRingDesigns());
		}
		if (BaseSearchResults != null)
		{
			List<SearchFilterItem> queryFilters = new List<SearchFilterItem>();
			SearchFilters.ToList().ForEach(delegate(SearchFilterViewModel f)
			{
				if (f.SearchFilterItems.Count((SearchFilterItem _) => _.IsSelected) == 1)
				{
					queryFilters.Add(f.SearchFilterItems.First((SearchFilterItem _) => _.IsSelected));
				}
				else if (f.SearchFilterItems.Count((SearchFilterItem _) => _.IsSelected) > 1)
				{
					SearchFilterItem searchFilterItem2 = new SearchFilterItem
					{
						GroupedAttributes = new List<GV.CounterSketch.Studio.Model.Entities.CounterSketchLibrary.Attribute>()
					};
					f.SearchFilterItems.Where((SearchFilterItem _) => _.IsSelected).ToList().ForEach(delegate(SearchFilterItem filterItem)
					{
						List<GV.CounterSketch.Studio.Model.Entities.CounterSketchLibrary.Attribute> groupedAttributes = filterItem.GroupedAttributes;
						if (groupedAttributes != null && groupedAttributes.Any())
						{
							searchFilterItem2.GroupedAttributes.AddRange(filterItem.GroupedAttributes);
						}
						else
						{
							GV.CounterSketch.Studio.Model.Entities.CounterSketchLibrary.Attribute item = new GV.CounterSketch.Studio.Model.Entities.CounterSketchLibrary.Attribute
							{
								AttributeCSOrder = filterItem.CSOrder,
								AttributeId = filterItem.SearchAttributeId,
								AttributeName = filterItem.PrimaryText + " " + filterItem.SecondaryText
							};
							searchFilterItem2.GroupedAttributes?.Add(item);
						}
					});
					queryFilters.Add(searchFilterItem2);
				}
			});
			SearchResults = new ObservableCollection<DesignSearch>(SearchHelper.UpdateDesignResultsWithFilters(queryFilters, BaseSearchResults.Select((DesignSearch result) => result.DesignId).ToList(), searchFilterItem));
		}
		UpdateDesignMaterials();
		UIHelper.ShowWaitWindow(waitVisible: false);
		ViewModelLocator.Current.TileViewPartsVM.IsBusySearching = false;
	}

	public void UpdateSPINumberVisibility()
	{
		RaisePropertyChanged("SPINumberVisibility");
		RaisePropertyChanged("ShowSPIToolTipNumber");
		RaisePropertyChanged("ShowDesignerNameInSearchDetail");
		RaisePropertyChanged("ShowComponentsInSearchDetail");
		RaisePropertyChanged("ShowSeriesInSearchDetail");
		RaisePropertyChanged("ShowPackagesInSearchDetail");
		RaisePropertyChanged("ShowKeywordsInSearchDetail");
	}

	private void AddDesignFromDesignDetailsToActiveProject()
	{
		GV.CounterSketch.Studio.Model.Objects.Design designFromDesignSearch = ProjectManagementHelper.GetDesignFromDesignSearch(SelectedDesignSearch);
		ProjectManagementHelper.AddDesignFromLibraryToActiveProject(designFromDesignSearch);
		LoadHelper.LoadDesign(designFromDesignSearch);
		CloseDesignInfo();
	}

	private void AddMarkedDesignsToActiveProject()
	{
		UIHelper.ShowWaitWindow(waitVisible: true);
		foreach (DesignSearch item in SearchResults.Where((DesignSearch x) => x.IsMarked))
		{
			ProjectManagementHelper.AddDesignFromLibraryToActiveProject(ProjectManagementHelper.GetDesignFromDesignSearch(item));
			item.IsMarked = false;
			SessionPickDesigns = new ObservableCollection<DesignSearch>();
		}
		CloseDesignInfo();
		UpdateDesignMarkedStatus();
		UIHelper.ShowWaitWindow(waitVisible: false);
	}

	private void AddMarkedDesignsToCollection()
	{
		throw new NotImplementedException();
	}

	private void ClearMarkedDesigns()
	{
		SearchResults?.ToList().ForEach(delegate(DesignSearch _)
		{
			_.ClearMarked();
		});
		SessionPickDesigns = new ObservableCollection<DesignSearch>();
		ViewModelLocator.Current.TileSearchMainVM.TileSearchResultDesignsVM.UpdateDesignMarkedStatus();
	}

	private SearchFilterViewModel CreateFilter(string filterName, int attributeTypeId, bool? resultsContainPendants)
	{
		SearchFilterViewModel obj = new SearchFilterViewModel
		{
			Title = filterName,
			ParentViewModel = this
		};
		bool freehandOnlyFilters = this is TileSearchResultPartsViewModel && attributeTypeId == 12;
		obj.SearchFilterItems = ((attributeTypeId != 6) ? SearchHelper.GetSearchFilterItems(attributeTypeId, resultsContainPendants, freehandOnlyFilters) : SearchHelper.GetDesignBySearchFilterItems());
		obj.SelectedSearchFilterItem = obj.SearchFilterItems.LastOrDefault();
		return obj;
	}

	private void CreateSearchFilters()
	{
		_isUpdatingSearchFilters = true;
		Stopwatch.StartNew();
		bool? resultsContainPendants = ((BaseSearchResults == null) ? new bool?(false) : (BaseSearchResults.All((DesignSearch result) => result.ComponentId == 4) ? new bool?(true) : (BaseSearchResults.All((DesignSearch result) => result.ComponentId == 1) ? new bool?(false) : ((bool?)null))));
		SearchFilterViewModel item = CreateFilter("Center Shape", 7, false);
		SearchFilterViewModel item2 = CreateFilter("Center Setting", 9, false);
		SearchFilterViewModel item3 = CreateFilter("Style", 12, false);
		SearchFilterViewModel item4 = CreateFilter("Design Element", 13, resultsContainPendants);
		SearchFilters = new ObservableCollection<SearchFilterViewModel> { item, item2, item3, item4 };
		if (ViewModelLocator.Current.StoneTrayVM.IsStoneAssigned)
		{
			UpdateSearchFiltersUsingAssignedStone(overrideSearchActiveRequirements: true);
		}
		else
		{
			GemSearchFilterLength = null;
			GemSearchFilterWidth = null;
			_gemSearchFilterSize.Width = 0.0;
			_gemSearchFilterSize.Height = 0.0;
		}
		_isUpdatingSearchFilters = false;
	}

	private ObservableCollection<DesignSearch> GenerateTestResults()
	{
		return new ObservableCollection<DesignSearch>
		{
			new DesignSearch
			{
				ComponentId = 1L,
				DesignId = 3411L,
				SPINumber = "SCS0208-OVF-0-00-EG",
				SeriesNumber = "1100",
				Designer = "Jason Stevenson",
				DefaultStyleElement = "Cool Jewels",
				SecondaryStyleElement = "Gaudy Glitter",
				IsSearchFavorite = true,
				DesignImageSource = UIHelper.SampleDesignImage,
				IsSelected = false
			},
			new DesignSearch
			{
				ComponentId = 1L,
				DesignId = 2L,
				SPINumber = "SCS0001-01",
				SeriesNumber = "1200",
				Designer = "Jason Stevenson",
				DefaultStyleElement = "Cool Jewels",
				SecondaryStyleElement = "Gaudy Glitter",
				IsSelected = true,
				DesignImageSource = UIHelper.SampleDesignImage
			},
			new DesignSearch
			{
				ComponentId = 1L,
				DesignId = 2L,
				SPINumber = "SCS0001-02",
				Designer = "Jason Stevenson",
				SeriesNumber = "1100",
				DefaultStyleElement = "Cool Jewels",
				SecondaryStyleElement = "Gaudy Glitter",
				IsSearchFavorite = true,
				DesignImageSource = UIHelper.SampleDesignImage,
				IsSelected = false,
				IsMarked = true
			},
			new DesignSearch
			{
				ComponentId = 1L,
				DesignId = 2L,
				SPINumber = "SCS0001-00",
				Designer = "Jason Stevenson",
				DefaultStyleElement = "Cool Jewels",
				SecondaryStyleElement = "Gaudy Glitter",
				DesignImageSource = UIHelper.SampleDesignImage,
				IsMarked = true
			},
			new DesignSearch
			{
				ComponentId = 1L,
				DesignId = 2L,
				SPINumber = "SCS0001-00",
				Designer = "Jason Stevenson",
				DefaultStyleElement = "Cool Jewels",
				SecondaryStyleElement = "Gaudy Glitter",
				DesignImageSource = UIHelper.SampleDesignImage,
				IsMarked = true
			},
			new DesignSearch
			{
				ComponentId = 1L,
				DesignId = 2L,
				SPINumber = "SCS0001-00",
				Designer = "Jason Stevenson",
				DefaultStyleElement = "Cool Jewels",
				SecondaryStyleElement = "Gaudy Glitter",
				DesignImageSource = UIHelper.SampleDesignImage,
				IsMarked = true
			},
			new DesignSearch
			{
				ComponentId = 1L,
				DesignId = 2L,
				SPINumber = "SCS0001-00",
				Designer = "Jason Stevenson",
				DefaultStyleElement = "Cool Jewels",
				SecondaryStyleElement = "Gaudy Glitter",
				DesignImageSource = UIHelper.SampleDesignImage,
				IsMarked = true
			},
			new DesignSearch
			{
				ComponentId = 1L,
				DesignId = 2L,
				SPINumber = "SCS0001-00",
				Designer = "Jason Stevenson",
				DefaultStyleElement = "Cool Jewels",
				SecondaryStyleElement = "Gaudy Glitter",
				DesignImageSource = UIHelper.SampleDesignImage,
				IsMarked = true
			},
			new DesignSearch
			{
				ComponentId = 1L,
				DesignId = 2L,
				SPINumber = "SCS0001-00",
				Designer = "Jason Stevenson",
				DefaultStyleElement = "Cool Jewels",
				SecondaryStyleElement = "Gaudy Glitter",
				DesignImageSource = UIHelper.SampleDesignImage,
				IsMarked = true
			},
			new DesignSearch
			{
				ComponentId = 1L,
				DesignId = 2L,
				SPINumber = "SCS0001-00",
				Designer = "Jason Stevenson",
				DefaultStyleElement = "Cool Jewels",
				SecondaryStyleElement = "Gaudy Glitter",
				IsMarked = true
			},
			new DesignSearch
			{
				ComponentId = 1L,
				DesignId = 2L,
				SPINumber = "SCS0001-00",
				Designer = "Jason Stevenson",
				DefaultStyleElement = "Cool Jewels",
				SecondaryStyleElement = "Gaudy Glitter"
			},
			new DesignSearch
			{
				ComponentId = 1L,
				DesignId = 2L,
				SPINumber = "SCS0001-00",
				Designer = "Jason Stevenson",
				DefaultStyleElement = "Cool Jewels",
				SecondaryStyleElement = "Gaudy Glitter"
			},
			new DesignSearch
			{
				ComponentId = 1L,
				DesignId = 2L,
				SPINumber = "SCS0001-00",
				Designer = "Jason Stevenson",
				DefaultStyleElement = "Cool Jewels",
				SecondaryStyleElement = "Gaudy Glitter"
			},
			new DesignSearch
			{
				ComponentId = 1L,
				DesignId = 2L,
				SPINumber = "SCS0001-00",
				Designer = "Jason Stevenson",
				DefaultStyleElement = "Cool Jewels",
				SecondaryStyleElement = "Gaudy Glitter"
			},
			new DesignSearch
			{
				ComponentId = 1L,
				DesignId = 2L,
				SPINumber = "SCS0001-00",
				Designer = "Jason Stevenson",
				DefaultStyleElement = "Cool Jewels",
				SecondaryStyleElement = "Gaudy Glitter"
			},
			new DesignSearch
			{
				ComponentId = 1L,
				DesignId = 2L,
				SPINumber = "SCS0001-00",
				Designer = "Jason Stevenson",
				DefaultStyleElement = "Cool Jewels",
				SecondaryStyleElement = "Gaudy Glitter"
			},
			new DesignSearch
			{
				ComponentId = 1L,
				DesignId = 2L,
				SPINumber = "SCS0001-00",
				Designer = "Jason Stevenson",
				DefaultStyleElement = "Cool Jewels",
				SecondaryStyleElement = "Gaudy Glitter"
			},
			new DesignSearch
			{
				ComponentId = 1L,
				DesignId = 2L,
				SPINumber = "SCS0001-00",
				Designer = "Jason Stevenson",
				DefaultStyleElement = "Cool Jewels",
				SecondaryStyleElement = "Gaudy Glitter"
			},
			new DesignSearch
			{
				ComponentId = 1L,
				DesignId = 2L,
				SPINumber = "SCS0001-00",
				Designer = "Jason Stevenson",
				DefaultStyleElement = "Cool Jewels",
				SecondaryStyleElement = "Gaudy Glitter"
			},
			new DesignSearch
			{
				ComponentId = 1L,
				DesignId = 2L,
				SPINumber = "SCS0001-00",
				Designer = "Jason Stevenson",
				DefaultStyleElement = "Cool Jewels",
				SecondaryStyleElement = "Gaudy Glitter"
			},
			new DesignSearch
			{
				ComponentId = 1L,
				DesignId = 2L,
				SPINumber = "SCS0001-00",
				Designer = "Jason Stevenson",
				DefaultStyleElement = "Cool Jewels",
				SecondaryStyleElement = "Gaudy Glitter"
			},
			new DesignSearch
			{
				ComponentId = 1L,
				DesignId = 2L,
				SPINumber = "SCS0001-00",
				Designer = "Jason Stevenson",
				DefaultStyleElement = "Cool Jewels",
				SecondaryStyleElement = "Gaudy Glitter"
			},
			new DesignSearch
			{
				ComponentId = 1L,
				DesignId = 2L,
				SPINumber = "SCS0001-00",
				Designer = "Jason Stevenson",
				DefaultStyleElement = "Cool Jewels",
				SecondaryStyleElement = "Gaudy Glitter"
			},
			new DesignSearch
			{
				ComponentId = 1L,
				DesignId = 2L,
				SPINumber = "SCS0001-00",
				Designer = "Jason Stevenson",
				DefaultStyleElement = "Cool Jewels",
				SecondaryStyleElement = "Gaudy Glitter"
			},
			new DesignSearch
			{
				ComponentId = 1L,
				DesignId = 2L,
				SPINumber = "SCS0001-00",
				Designer = "Jason Stevenson",
				DefaultStyleElement = "Cool Jewels",
				SecondaryStyleElement = "Gaudy Glitter"
			},
			new DesignSearch
			{
				ComponentId = 1L,
				DesignId = 2L,
				SPINumber = "SCS0001-00",
				Designer = "Jason Stevenson",
				DefaultStyleElement = "Cool Jewels",
				SecondaryStyleElement = "Gaudy Glitter"
			},
			new DesignSearch
			{
				ComponentId = 1L,
				DesignId = 2L,
				SPINumber = "SCS0001-00",
				Designer = "Jason Stevenson",
				DefaultStyleElement = "Cool Jewels",
				SecondaryStyleElement = "Gaudy Glitter"
			},
			new DesignSearch
			{
				ComponentId = 1L,
				DesignId = 2L,
				SPINumber = "SCS0001-00",
				Designer = "Jason Stevenson",
				DefaultStyleElement = "Cool Jewels",
				SecondaryStyleElement = "Gaudy Glitter"
			},
			new DesignSearch
			{
				ComponentId = 1L,
				DesignId = 2L,
				SPINumber = "SCS0001-00",
				Designer = "Jason Stevenson",
				DefaultStyleElement = "Cool Jewels",
				SecondaryStyleElement = "Gaudy Glitter"
			},
			new DesignSearch
			{
				ComponentId = 1L,
				DesignId = 2L,
				SPINumber = "SCS0001-00",
				Designer = "Jason Stevenson",
				DefaultStyleElement = "Cool Jewels",
				SecondaryStyleElement = "Gaudy Glitter"
			},
			new DesignSearch
			{
				ComponentId = 1L,
				DesignId = 2L,
				SPINumber = "SCS0001-00",
				Designer = "Jason Stevenson",
				DefaultStyleElement = "Cool Jewels",
				SecondaryStyleElement = "Gaudy Glitter"
			},
			new DesignSearch
			{
				ComponentId = 1L,
				DesignId = 2L,
				SPINumber = "SCS0001-00",
				Designer = "Jason Stevenson",
				DefaultStyleElement = "Cool Jewels",
				SecondaryStyleElement = "Gaudy Glitter"
			},
			new DesignSearch
			{
				ComponentId = 1L,
				DesignId = 2L,
				SPINumber = "SCS0001-00",
				Designer = "Jason Stevenson",
				DefaultStyleElement = "Cool Jewels",
				SecondaryStyleElement = "Gaudy Glitter"
			},
			new DesignSearch
			{
				ComponentId = 1L,
				DesignId = 2L,
				SPINumber = "SCS0001-00",
				Designer = "Jason Stevenson",
				DefaultStyleElement = "Cool Jewels",
				SecondaryStyleElement = "Gaudy Glitter"
			},
			new DesignSearch
			{
				ComponentId = 1L,
				DesignId = 2L,
				SPINumber = "SCS0001-00",
				Designer = "Jason Stevenson",
				DefaultStyleElement = "Cool Jewels",
				SecondaryStyleElement = "Gaudy Glitter"
			},
			new DesignSearch
			{
				ComponentId = 1L,
				DesignId = 2L,
				SPINumber = "SCS0001-00",
				Designer = "Jason Stevenson",
				DefaultStyleElement = "Cool Jewels",
				SecondaryStyleElement = "Gaudy Glitter"
			},
			new DesignSearch
			{
				ComponentId = 1L,
				DesignId = 2L,
				SPINumber = "SCS0001-00",
				Designer = "Jason Stevenson",
				DefaultStyleElement = "Cool Jewels",
				SecondaryStyleElement = "Gaudy Glitter"
			},
			new DesignSearch
			{
				ComponentId = 1L,
				DesignId = 2L,
				SPINumber = "SCS0001-00",
				Designer = "Jason Stevenson",
				DefaultStyleElement = "Cool Jewels",
				SecondaryStyleElement = "Gaudy Glitter",
				IsSelected = true
			},
			new DesignSearch
			{
				ComponentId = 1L,
				DesignId = 2L,
				SPINumber = "SCS0001-00",
				Designer = "Jason Stevenson",
				DefaultStyleElement = "Cool Jewels",
				SecondaryStyleElement = "Gaudy Glitter"
			},
			new DesignSearch
			{
				ComponentId = 1L,
				DesignId = 2L,
				SPINumber = "SCS0001-00",
				Designer = "Jason Stevenson",
				DefaultStyleElement = "Cool Jewels",
				SecondaryStyleElement = "Gaudy Glitter"
			},
			new DesignSearch
			{
				ComponentId = 1L,
				DesignId = 2L,
				SPINumber = "SCS0001-00",
				Designer = "Jason Stevenson",
				DefaultStyleElement = "Cool Jewels",
				SecondaryStyleElement = "Gaudy Glitter"
			},
			new DesignSearch
			{
				ComponentId = 1L,
				DesignId = 2L,
				SPINumber = "SCS0001-00",
				Designer = "Jason Stevenson",
				DefaultStyleElement = "Cool Jewels",
				SecondaryStyleElement = "Gaudy Glitter"
			},
			new DesignSearch
			{
				ComponentId = 1L,
				DesignId = 2L,
				SPINumber = "SCS0001-00",
				Designer = "Jason Stevenson",
				DefaultStyleElement = "Cool Jewels",
				SecondaryStyleElement = "Gaudy Glitter"
			},
			new DesignSearch
			{
				ComponentId = 1L,
				DesignId = 2L,
				SPINumber = "SCS0001-00",
				Designer = "Jason Stevenson",
				DefaultStyleElement = "Cool Jewels",
				SecondaryStyleElement = "Gaudy Glitter"
			},
			new DesignSearch
			{
				ComponentId = 1L,
				DesignId = 2L,
				SPINumber = "SCS0001-00",
				Designer = "Jason Stevenson",
				DefaultStyleElement = "Cool Jewels",
				SecondaryStyleElement = "Gaudy Glitter"
			},
			new DesignSearch
			{
				ComponentId = 1L,
				DesignId = 2L,
				SPINumber = "SCS0001-00",
				Designer = "Jason Stevenson",
				DefaultStyleElement = "Cool Jewels",
				SecondaryStyleElement = "Gaudy Glitter"
			},
			new DesignSearch
			{
				ComponentId = 1L,
				DesignId = 2L,
				SPINumber = "SCS0001-00",
				Designer = "Jason Stevenson",
				DefaultStyleElement = "Cool Jewels",
				SecondaryStyleElement = "Gaudy Glitter"
			},
			new DesignSearch
			{
				ComponentId = 1L,
				DesignId = 2L,
				SPINumber = "SCS0001-00",
				Designer = "Jason Stevenson",
				DefaultStyleElement = "Cool Jewels",
				SecondaryStyleElement = "Gaudy Glitter"
			},
			new DesignSearch
			{
				ComponentId = 1L,
				DesignId = 2L,
				SPINumber = "SCS0001-00",
				Designer = "Jason Stevenson",
				DefaultStyleElement = "Cool Jewels",
				SecondaryStyleElement = "Gaudy Glitter"
			},
			new DesignSearch
			{
				ComponentId = 1L,
				DesignId = 2L,
				SPINumber = "SCS0001-00",
				Designer = "Jason Stevenson",
				DefaultStyleElement = "Cool Jewels",
				SecondaryStyleElement = "Gaudy Glitter"
			},
			new DesignSearch
			{
				ComponentId = 1L,
				DesignId = 2L,
				SPINumber = "SCS0001-00",
				Designer = "Jason Stevenson",
				DefaultStyleElement = "Cool Jewels",
				SecondaryStyleElement = "Gaudy Glitter"
			},
			new DesignSearch
			{
				ComponentId = 1L,
				DesignId = 2L,
				SPINumber = "SCS0001-00",
				Designer = "Jason Stevenson",
				DefaultStyleElement = "Cool Jewels",
				SecondaryStyleElement = "Gaudy Glitter"
			},
			new DesignSearch
			{
				ComponentId = 1L,
				DesignId = 2L,
				SPINumber = "SCS0001-00",
				Designer = "Jason Stevenson",
				DefaultStyleElement = "Cool Jewels",
				SecondaryStyleElement = "Gaudy Glitter"
			},
			new DesignSearch
			{
				ComponentId = 1L,
				DesignId = 2L,
				SPINumber = "SCS0001-00",
				Designer = "Jason Stevenson",
				DefaultStyleElement = "Cool Jewels",
				SecondaryStyleElement = "Gaudy Glitter"
			},
			new DesignSearch
			{
				ComponentId = 1L,
				DesignId = 2L,
				SPINumber = "SCS0001-00",
				Designer = "Jason Stevenson",
				DefaultStyleElement = "Cool Jewels",
				SecondaryStyleElement = "Gaudy Glitter"
			},
			new DesignSearch
			{
				ComponentId = 1L,
				DesignId = 2L,
				SPINumber = "SCS0001-00",
				Designer = "Jason Stevenson",
				DefaultStyleElement = "Cool Jewels",
				SecondaryStyleElement = "Gaudy Glitter"
			},
			new DesignSearch
			{
				ComponentId = 1L,
				DesignId = 2L,
				SPINumber = "SCS0001-00",
				Designer = "Jason Stevenson",
				DefaultStyleElement = "Cool Jewels",
				SecondaryStyleElement = "Gaudy Glitter"
			},
			new DesignSearch
			{
				ComponentId = 1L,
				DesignId = 2L,
				SPINumber = "SCS0001-00",
				Designer = "Jason Stevenson",
				DefaultStyleElement = "Cool Jewels",
				SecondaryStyleElement = "Gaudy Glitter"
			},
			new DesignSearch
			{
				ComponentId = 1L,
				DesignId = 2L,
				SPINumber = "SCS0001-00",
				Designer = "Jason Stevenson",
				DefaultStyleElement = "Cool Jewels",
				SecondaryStyleElement = "Gaudy Glitter"
			},
			new DesignSearch
			{
				ComponentId = 1L,
				DesignId = 2L,
				SPINumber = "SCS0001-00",
				Designer = "Jason Stevenson",
				DefaultStyleElement = "Cool Jewels",
				SecondaryStyleElement = "Gaudy Glitter"
			},
			new DesignSearch
			{
				ComponentId = 1L,
				DesignId = 2L,
				SPINumber = "SCS0001-00",
				Designer = "Jason Stevenson",
				DefaultStyleElement = "Cool Jewels",
				SecondaryStyleElement = "Gaudy Glitter"
			},
			new DesignSearch
			{
				ComponentId = 1L,
				DesignId = 2L,
				SPINumber = "SCS0001-00",
				Designer = "Jason Stevenson",
				DefaultStyleElement = "Cool Jewels",
				SecondaryStyleElement = "Gaudy Glitter"
			},
			new DesignSearch
			{
				ComponentId = 1L,
				DesignId = 2L,
				SPINumber = "SCS0001-00",
				Designer = "Jason Stevenson",
				DefaultStyleElement = "Cool Jewels",
				SecondaryStyleElement = "Gaudy Glitter"
			},
			new DesignSearch
			{
				ComponentId = 1L,
				DesignId = 2L,
				SPINumber = "SCS0001-00",
				Designer = "Jason Stevenson",
				DefaultStyleElement = "Cool Jewels",
				SecondaryStyleElement = "Gaudy Glitter"
			},
			new DesignSearch
			{
				ComponentId = 1L,
				DesignId = 2L,
				SPINumber = "SCS0001-00",
				Designer = "Jason Stevenson",
				DefaultStyleElement = "Cool Jewels",
				SecondaryStyleElement = "Gaudy Glitter"
			},
			new DesignSearch
			{
				ComponentId = 1L,
				DesignId = 2L,
				SPINumber = "SCS0001-00",
				Designer = "Jason Stevenson",
				DefaultStyleElement = "Cool Jewels",
				SecondaryStyleElement = "Gaudy Glitter"
			},
			new DesignSearch
			{
				ComponentId = 1L,
				DesignId = 2L,
				SPINumber = "SCS0001-00",
				Designer = "Jason Stevenson",
				DefaultStyleElement = "Cool Jewels",
				SecondaryStyleElement = "Gaudy Glitter"
			},
			new DesignSearch
			{
				ComponentId = 1L,
				DesignId = 2L,
				SPINumber = "SCS0001-00",
				Designer = "Jason Stevenson",
				DefaultStyleElement = "Cool Jewels",
				SecondaryStyleElement = "Gaudy Glitter"
			},
			new DesignSearch
			{
				ComponentId = 1L,
				DesignId = 2L,
				SPINumber = "SCS0001-00",
				Designer = "Jason Stevenson",
				DefaultStyleElement = "Cool Jewels",
				SecondaryStyleElement = "Gaudy Glitter"
			},
			new DesignSearch
			{
				ComponentId = 1L,
				DesignId = 2L,
				SPINumber = "SCS0001-00",
				Designer = "Jason Stevenson",
				DefaultStyleElement = "Cool Jewels",
				SecondaryStyleElement = "Gaudy Glitter"
			},
			new DesignSearch
			{
				ComponentId = 1L,
				DesignId = 2L,
				SPINumber = "SCS0001-00",
				Designer = "Jason Stevenson",
				DefaultStyleElement = "Cool Jewels",
				SecondaryStyleElement = "Gaudy Glitter"
			},
			new DesignSearch
			{
				ComponentId = 1L,
				DesignId = 2L,
				SPINumber = "SCS0001-00",
				Designer = "Jason Stevenson",
				DefaultStyleElement = "Cool Jewels",
				SecondaryStyleElement = "Gaudy Glitter"
			},
			new DesignSearch
			{
				ComponentId = 1L,
				DesignId = 2L,
				SPINumber = "SCS0001-00",
				Designer = "Jason Stevenson",
				DefaultStyleElement = "Cool Jewels",
				SecondaryStyleElement = "Gaudy Glitter"
			},
			new DesignSearch
			{
				ComponentId = 1L,
				DesignId = 2L,
				SPINumber = "SCS0001-00",
				Designer = "Jason Stevenson",
				DefaultStyleElement = "Cool Jewels",
				SecondaryStyleElement = "Gaudy Glitter",
				IsSelected = true
			},
			new DesignSearch
			{
				ComponentId = 1L,
				DesignId = 2L,
				SPINumber = "SCS0001-00",
				Designer = "Jason Stevenson",
				DefaultStyleElement = "Cool Jewels",
				SecondaryStyleElement = "Gaudy Glitter"
			},
			new DesignSearch
			{
				ComponentId = 1L,
				DesignId = 2L,
				SPINumber = "SCS0001-00",
				Designer = "Jason Stevenson",
				DefaultStyleElement = "Cool Jewels",
				SecondaryStyleElement = "Gaudy Glitter"
			},
			new DesignSearch
			{
				ComponentId = 1L,
				DesignId = 2L,
				SPINumber = "SCS0001-00",
				Designer = "Jason Stevenson",
				DefaultStyleElement = "Cool Jewels",
				SecondaryStyleElement = "Gaudy Glitter"
			},
			new DesignSearch
			{
				ComponentId = 1L,
				DesignId = 2L,
				SPINumber = "SCS0001-00",
				Designer = "Jason Stevenson",
				DefaultStyleElement = "Cool Jewels",
				SecondaryStyleElement = "Gaudy Glitter"
			},
			new DesignSearch
			{
				ComponentId = 1L,
				DesignId = 2L,
				SPINumber = "SCS0001-00",
				Designer = "Jason Stevenson",
				DefaultStyleElement = "Cool Jewels",
				SecondaryStyleElement = "Gaudy Glitter"
			},
			new DesignSearch
			{
				ComponentId = 1L,
				DesignId = 2L,
				SPINumber = "SCS0001-00",
				Designer = "Jason Stevenson",
				DefaultStyleElement = "Cool Jewels",
				SecondaryStyleElement = "Gaudy Glitter"
			},
			new DesignSearch
			{
				ComponentId = 1L,
				DesignId = 2L,
				SPINumber = "SCS0001-00",
				Designer = "Jason Stevenson",
				DefaultStyleElement = "Cool Jewels",
				SecondaryStyleElement = "Gaudy Glitter"
			},
			new DesignSearch
			{
				ComponentId = 1L,
				DesignId = 2L,
				SPINumber = "SCS0001-00",
				Designer = "Jason Stevenson",
				DefaultStyleElement = "Cool Jewels",
				SecondaryStyleElement = "Gaudy Glitter"
			},
			new DesignSearch
			{
				ComponentId = 1L,
				DesignId = 2L,
				SPINumber = "SCS0001-00",
				Designer = "Jason Stevenson",
				DefaultStyleElement = "Cool Jewels",
				SecondaryStyleElement = "Gaudy Glitter"
			},
			new DesignSearch
			{
				ComponentId = 1L,
				DesignId = 2L,
				SPINumber = "SCS0001-00",
				Designer = "Jason Stevenson",
				DefaultStyleElement = "Cool Jewels",
				SecondaryStyleElement = "Gaudy Glitter"
			},
			new DesignSearch
			{
				ComponentId = 1L,
				DesignId = 2L,
				SPINumber = "SCS0001-00",
				Designer = "Jason Stevenson",
				DefaultStyleElement = "Cool Jewels",
				SecondaryStyleElement = "Gaudy Glitter"
			},
			new DesignSearch
			{
				ComponentId = 1L,
				DesignId = 2L,
				SPINumber = "SCS0001-00",
				Designer = "Jason Stevenson",
				DefaultStyleElement = "Cool Jewels",
				SecondaryStyleElement = "Gaudy Glitter"
			},
			new DesignSearch
			{
				ComponentId = 1L,
				DesignId = 2L,
				SPINumber = "SCS0001-00",
				Designer = "Jason Stevenson",
				DefaultStyleElement = "Cool Jewels",
				SecondaryStyleElement = "Gaudy Glitter"
			},
			new DesignSearch
			{
				ComponentId = 1L,
				DesignId = 2L,
				SPINumber = "SCS0001-00",
				Designer = "Jason Stevenson",
				DefaultStyleElement = "Cool Jewels",
				SecondaryStyleElement = "Gaudy Glitter"
			},
			new DesignSearch
			{
				ComponentId = 1L,
				DesignId = 2L,
				SPINumber = "SCS0001-00",
				Designer = "Jason Stevenson",
				DefaultStyleElement = "Cool Jewels",
				SecondaryStyleElement = "Gaudy Glitter"
			},
			new DesignSearch
			{
				ComponentId = 1L,
				DesignId = 2L,
				SPINumber = "SCS0001-00",
				Designer = "Jason Stevenson",
				DefaultStyleElement = "Cool Jewels",
				SecondaryStyleElement = "Gaudy Glitter"
			},
			new DesignSearch
			{
				ComponentId = 1L,
				DesignId = 2L,
				SPINumber = "SCS0001-00",
				Designer = "Jason Stevenson",
				DefaultStyleElement = "Cool Jewels",
				SecondaryStyleElement = "Gaudy Glitter"
			},
			new DesignSearch
			{
				ComponentId = 1L,
				DesignId = 2L,
				SPINumber = "SCS0001-00",
				Designer = "Jason Stevenson",
				DefaultStyleElement = "Cool Jewels",
				SecondaryStyleElement = "Gaudy Glitter"
			},
			new DesignSearch
			{
				ComponentId = 1L,
				DesignId = 2L,
				SPINumber = "SCS0001-00",
				Designer = "Jason Stevenson",
				DefaultStyleElement = "Cool Jewels",
				SecondaryStyleElement = "Gaudy Glitter"
			},
			new DesignSearch
			{
				ComponentId = 1L,
				DesignId = 2L,
				SPINumber = "SCS0001-00",
				Designer = "Jason Stevenson",
				DefaultStyleElement = "Cool Jewels",
				SecondaryStyleElement = "Gaudy Glitter"
			},
			new DesignSearch
			{
				ComponentId = 1L,
				DesignId = 2L,
				SPINumber = "SCS0001-00",
				Designer = "Jason Stevenson",
				DefaultStyleElement = "Cool Jewels",
				SecondaryStyleElement = "Gaudy Glitter"
			},
			new DesignSearch
			{
				ComponentId = 1L,
				DesignId = 2L,
				SPINumber = "SCS0001-00",
				Designer = "Jason Stevenson",
				DefaultStyleElement = "Cool Jewels",
				SecondaryStyleElement = "Gaudy Glitter"
			},
			new DesignSearch
			{
				ComponentId = 1L,
				DesignId = 2L,
				SPINumber = "SCS0001-00",
				Designer = "Jason Stevenson",
				DefaultStyleElement = "Cool Jewels",
				SecondaryStyleElement = "Gaudy Glitter"
			},
			new DesignSearch
			{
				ComponentId = 1L,
				DesignId = 2L,
				SPINumber = "SCS0001-00",
				Designer = "Jason Stevenson",
				DefaultStyleElement = "Cool Jewels",
				SecondaryStyleElement = "Gaudy Glitter"
			},
			new DesignSearch
			{
				ComponentId = 1L,
				DesignId = 2L,
				SPINumber = "SCS0001-00",
				Designer = "Jason Stevenson",
				DefaultStyleElement = "Cool Jewels",
				SecondaryStyleElement = "Gaudy Glitter"
			},
			new DesignSearch
			{
				ComponentId = 1L,
				DesignId = 2L,
				SPINumber = "SCS0001-00",
				Designer = "Jason Stevenson",
				DefaultStyleElement = "Cool Jewels",
				SecondaryStyleElement = "Gaudy Glitter"
			},
			new DesignSearch
			{
				ComponentId = 1L,
				DesignId = 2L,
				SPINumber = "SCS0001-00",
				Designer = "Jason Stevenson",
				DefaultStyleElement = "Cool Jewels",
				SecondaryStyleElement = "Gaudy Glitter"
			},
			new DesignSearch
			{
				ComponentId = 1L,
				DesignId = 2L,
				SPINumber = "SCS0001-00",
				Designer = "Jason Stevenson",
				DefaultStyleElement = "Cool Jewels",
				SecondaryStyleElement = "Gaudy Glitter"
			},
			new DesignSearch
			{
				ComponentId = 1L,
				DesignId = 2L,
				SPINumber = "SCS0001-00",
				Designer = "Jason Stevenson",
				DefaultStyleElement = "Cool Jewels",
				SecondaryStyleElement = "Gaudy Glitter"
			},
			new DesignSearch
			{
				ComponentId = 1L,
				DesignId = 2L,
				SPINumber = "SCS0001-00",
				Designer = "Jason Stevenson",
				DefaultStyleElement = "Cool Jewels",
				SecondaryStyleElement = "Gaudy Glitter"
			},
			new DesignSearch
			{
				ComponentId = 1L,
				DesignId = 2L,
				SPINumber = "SCS0001-00",
				Designer = "Jason Stevenson",
				DefaultStyleElement = "Cool Jewels",
				SecondaryStyleElement = "Gaudy Glitter"
			},
			new DesignSearch
			{
				ComponentId = 1L,
				DesignId = 2L,
				SPINumber = "SCS0001-00",
				Designer = "Jason Stevenson",
				DefaultStyleElement = "Cool Jewels",
				SecondaryStyleElement = "Gaudy Glitter"
			},
			new DesignSearch
			{
				ComponentId = 1L,
				DesignId = 2L,
				SPINumber = "SCS0001-00",
				Designer = "Jason Stevenson",
				DefaultStyleElement = "Cool Jewels",
				SecondaryStyleElement = "Gaudy Glitter"
			},
			new DesignSearch
			{
				ComponentId = 1L,
				DesignId = 2L,
				SPINumber = "SCS0001-00",
				Designer = "Jason Stevenson",
				DefaultStyleElement = "Cool Jewels",
				SecondaryStyleElement = "Gaudy Glitter"
			},
			new DesignSearch
			{
				ComponentId = 1L,
				DesignId = 2L,
				SPINumber = "SCS0001-00",
				Designer = "Jason Stevenson",
				DefaultStyleElement = "Cool Jewels",
				SecondaryStyleElement = "Gaudy Glitter"
			},
			new DesignSearch
			{
				ComponentId = 1L,
				DesignId = 2L,
				SPINumber = "SCS0001-00",
				Designer = "Jason Stevenson",
				DefaultStyleElement = "Cool Jewels",
				SecondaryStyleElement = "Gaudy Glitter",
				IsSelected = true
			},
			new DesignSearch
			{
				ComponentId = 1L,
				DesignId = 2L,
				SPINumber = "SCS0001-00",
				Designer = "Jason Stevenson",
				DefaultStyleElement = "Cool Jewels",
				SecondaryStyleElement = "Gaudy Glitter"
			},
			new DesignSearch
			{
				ComponentId = 1L,
				DesignId = 2L,
				SPINumber = "SCS0001-00",
				Designer = "Jason Stevenson",
				DefaultStyleElement = "Cool Jewels",
				SecondaryStyleElement = "Gaudy Glitter"
			},
			new DesignSearch
			{
				ComponentId = 1L,
				DesignId = 2L,
				SPINumber = "SCS0001-00",
				Designer = "Jason Stevenson",
				DefaultStyleElement = "Cool Jewels",
				SecondaryStyleElement = "Gaudy Glitter"
			},
			new DesignSearch
			{
				ComponentId = 1L,
				DesignId = 2L,
				SPINumber = "SCS0001-00",
				Designer = "Jason Stevenson",
				DefaultStyleElement = "Cool Jewels",
				SecondaryStyleElement = "Gaudy Glitter"
			},
			new DesignSearch
			{
				ComponentId = 1L,
				DesignId = 2L,
				SPINumber = "SCS0001-00",
				Designer = "Jason Stevenson",
				DefaultStyleElement = "Cool Jewels",
				SecondaryStyleElement = "Gaudy Glitter"
			},
			new DesignSearch
			{
				ComponentId = 1L,
				DesignId = 2L,
				SPINumber = "SCS0001-00",
				Designer = "Jason Stevenson",
				DefaultStyleElement = "Cool Jewels",
				SecondaryStyleElement = "Gaudy Glitter"
			},
			new DesignSearch
			{
				ComponentId = 1L,
				DesignId = 2L,
				SPINumber = "SCS0001-00",
				Designer = "Jason Stevenson",
				DefaultStyleElement = "Cool Jewels",
				SecondaryStyleElement = "Gaudy Glitter"
			},
			new DesignSearch
			{
				ComponentId = 1L,
				DesignId = 2L,
				SPINumber = "SCS0001-00",
				Designer = "Jason Stevenson",
				DefaultStyleElement = "Cool Jewels",
				SecondaryStyleElement = "Gaudy Glitter"
			},
			new DesignSearch
			{
				ComponentId = 1L,
				DesignId = 2L,
				SPINumber = "SCS0001-00",
				Designer = "Jason Stevenson",
				DefaultStyleElement = "Cool Jewels",
				SecondaryStyleElement = "Gaudy Glitter"
			},
			new DesignSearch
			{
				ComponentId = 1L,
				DesignId = 2L,
				SPINumber = "SCS0001-00",
				Designer = "Jason Stevenson",
				DefaultStyleElement = "Cool Jewels",
				SecondaryStyleElement = "Gaudy Glitter"
			},
			new DesignSearch
			{
				ComponentId = 1L,
				DesignId = 2L,
				SPINumber = "SCS0001-00",
				Designer = "Jason Stevenson",
				DefaultStyleElement = "Cool Jewels",
				SecondaryStyleElement = "Gaudy Glitter"
			},
			new DesignSearch
			{
				ComponentId = 1L,
				DesignId = 2L,
				SPINumber = "SCS0001-00",
				Designer = "Jason Stevenson",
				DefaultStyleElement = "Cool Jewels",
				SecondaryStyleElement = "Gaudy Glitter"
			},
			new DesignSearch
			{
				ComponentId = 1L,
				DesignId = 2L,
				SPINumber = "SCS0001-00",
				Designer = "Jason Stevenson",
				DefaultStyleElement = "Cool Jewels",
				SecondaryStyleElement = "Gaudy Glitter"
			},
			new DesignSearch
			{
				ComponentId = 1L,
				DesignId = 2L,
				SPINumber = "SCS0001-00",
				Designer = "Jason Stevenson",
				DefaultStyleElement = "Cool Jewels",
				SecondaryStyleElement = "Gaudy Glitter"
			},
			new DesignSearch
			{
				ComponentId = 1L,
				DesignId = 2L,
				SPINumber = "SCS0001-00",
				Designer = "Jason Stevenson",
				DefaultStyleElement = "Cool Jewels",
				SecondaryStyleElement = "Gaudy Glitter"
			},
			new DesignSearch
			{
				ComponentId = 1L,
				DesignId = 2L,
				SPINumber = "SCS0001-00",
				Designer = "Jason Stevenson",
				DefaultStyleElement = "Cool Jewels",
				SecondaryStyleElement = "Gaudy Glitter"
			},
			new DesignSearch
			{
				ComponentId = 1L,
				DesignId = 2L,
				SPINumber = "SCS0001-00",
				Designer = "Jason Stevenson",
				DefaultStyleElement = "Cool Jewels",
				SecondaryStyleElement = "Gaudy Glitter"
			},
			new DesignSearch
			{
				ComponentId = 1L,
				DesignId = 2L,
				SPINumber = "SCS0001-00",
				Designer = "Jason Stevenson",
				DefaultStyleElement = "Cool Jewels",
				SecondaryStyleElement = "Gaudy Glitter"
			},
			new DesignSearch
			{
				ComponentId = 1L,
				DesignId = 2L,
				SPINumber = "SCS0001-00",
				Designer = "Jason Stevenson",
				DefaultStyleElement = "Cool Jewels",
				SecondaryStyleElement = "Gaudy Glitter"
			},
			new DesignSearch
			{
				ComponentId = 1L,
				DesignId = 2L,
				SPINumber = "SCS0001-00",
				Designer = "Jason Stevenson",
				DefaultStyleElement = "Cool Jewels",
				SecondaryStyleElement = "Gaudy Glitter"
			},
			new DesignSearch
			{
				ComponentId = 1L,
				DesignId = 2L,
				SPINumber = "SCS0001-00",
				Designer = "Jason Stevenson",
				DefaultStyleElement = "Cool Jewels",
				SecondaryStyleElement = "Gaudy Glitter"
			},
			new DesignSearch
			{
				ComponentId = 1L,
				DesignId = 2L,
				SPINumber = "SCS0001-00",
				Designer = "Jason Stevenson",
				DefaultStyleElement = "Cool Jewels",
				SecondaryStyleElement = "Gaudy Glitter"
			},
			new DesignSearch
			{
				ComponentId = 1L,
				DesignId = 2L,
				SPINumber = "SCS0001-00",
				Designer = "Jason Stevenson",
				DefaultStyleElement = "Cool Jewels",
				SecondaryStyleElement = "Gaudy Glitter"
			},
			new DesignSearch
			{
				ComponentId = 1L,
				DesignId = 2L,
				SPINumber = "SCS0001-00",
				Designer = "Jason Stevenson",
				DefaultStyleElement = "Cool Jewels",
				SecondaryStyleElement = "Gaudy Glitter"
			},
			new DesignSearch
			{
				ComponentId = 1L,
				DesignId = 2L,
				SPINumber = "SCS0001-00",
				Designer = "Jason Stevenson",
				DefaultStyleElement = "Cool Jewels",
				SecondaryStyleElement = "Gaudy Glitter"
			},
			new DesignSearch
			{
				ComponentId = 1L,
				DesignId = 2L,
				SPINumber = "SCS0001-00",
				Designer = "Jason Stevenson",
				DefaultStyleElement = "Cool Jewels",
				SecondaryStyleElement = "Gaudy Glitter"
			},
			new DesignSearch
			{
				ComponentId = 1L,
				DesignId = 2L,
				SPINumber = "SCS0001-00",
				Designer = "Jason Stevenson",
				DefaultStyleElement = "Cool Jewels",
				SecondaryStyleElement = "Gaudy Glitter"
			},
			new DesignSearch
			{
				ComponentId = 1L,
				DesignId = 2L,
				SPINumber = "SCS0001-00",
				Designer = "Jason Stevenson",
				DefaultStyleElement = "Cool Jewels",
				SecondaryStyleElement = "Gaudy Glitter"
			},
			new DesignSearch
			{
				ComponentId = 1L,
				DesignId = 2L,
				SPINumber = "SCS0001-00",
				Designer = "Jason Stevenson",
				DefaultStyleElement = "Cool Jewels",
				SecondaryStyleElement = "Gaudy Glitter"
			},
			new DesignSearch
			{
				ComponentId = 1L,
				DesignId = 2L,
				SPINumber = "SCS0001-00",
				Designer = "Jason Stevenson",
				DefaultStyleElement = "Cool Jewels",
				SecondaryStyleElement = "Gaudy Glitter"
			},
			new DesignSearch
			{
				ComponentId = 1L,
				DesignId = 2L,
				SPINumber = "SCS0001-00",
				Designer = "Jason Stevenson",
				DefaultStyleElement = "Cool Jewels",
				SecondaryStyleElement = "Gaudy Glitter"
			},
			new DesignSearch
			{
				ComponentId = 1L,
				DesignId = 2L,
				SPINumber = "SCS0001-00",
				Designer = "Jason Stevenson",
				DefaultStyleElement = "Cool Jewels",
				SecondaryStyleElement = "Gaudy Glitter"
			},
			new DesignSearch
			{
				ComponentId = 1L,
				DesignId = 2L,
				SPINumber = "SCS0001-00",
				Designer = "Jason Stevenson",
				DefaultStyleElement = "Cool Jewels",
				SecondaryStyleElement = "Gaudy Glitter"
			},
			new DesignSearch
			{
				ComponentId = 1L,
				DesignId = 2L,
				SPINumber = "SCS0001-00",
				Designer = "Jason Stevenson",
				DefaultStyleElement = "Cool Jewels",
				SecondaryStyleElement = "Gaudy Glitter"
			},
			new DesignSearch
			{
				ComponentId = 1L,
				DesignId = 2L,
				SPINumber = "SCS0001-00",
				Designer = "Jason Stevenson",
				DefaultStyleElement = "Cool Jewels",
				SecondaryStyleElement = "Gaudy Glitter"
			},
			new DesignSearch
			{
				ComponentId = 1L,
				DesignId = 2L,
				SPINumber = "SCS0001-00",
				Designer = "Jason Stevenson",
				DefaultStyleElement = "Cool Jewels",
				SecondaryStyleElement = "Gaudy Glitter",
				IsSelected = true
			},
			new DesignSearch
			{
				ComponentId = 1L,
				DesignId = 2L,
				SPINumber = "SCS0001-00",
				Designer = "Jason Stevenson",
				DefaultStyleElement = "Cool Jewels",
				SecondaryStyleElement = "Gaudy Glitter"
			},
			new DesignSearch
			{
				ComponentId = 1L,
				DesignId = 2L,
				SPINumber = "SCS0001-00",
				Designer = "Jason Stevenson",
				DefaultStyleElement = "Cool Jewels",
				SecondaryStyleElement = "Gaudy Glitter"
			},
			new DesignSearch
			{
				ComponentId = 1L,
				DesignId = 2L,
				SPINumber = "SCS0001-00",
				Designer = "Jason Stevenson",
				DefaultStyleElement = "Cool Jewels",
				SecondaryStyleElement = "Gaudy Glitter"
			},
			new DesignSearch
			{
				ComponentId = 1L,
				DesignId = 2L,
				SPINumber = "SCS0001-00",
				Designer = "Jason Stevenson",
				DefaultStyleElement = "Cool Jewels",
				SecondaryStyleElement = "Gaudy Glitter"
			},
			new DesignSearch
			{
				ComponentId = 1L,
				DesignId = 2L,
				SPINumber = "SCS0001-00",
				Designer = "Jason Stevenson",
				DefaultStyleElement = "Cool Jewels",
				SecondaryStyleElement = "Gaudy Glitter"
			},
			new DesignSearch
			{
				ComponentId = 1L,
				DesignId = 2L,
				SPINumber = "SCS0001-00",
				Designer = "Jason Stevenson",
				DefaultStyleElement = "Cool Jewels",
				SecondaryStyleElement = "Gaudy Glitter"
			},
			new DesignSearch
			{
				ComponentId = 1L,
				DesignId = 2L,
				SPINumber = "SCS0001-00",
				Designer = "Jason Stevenson",
				DefaultStyleElement = "Cool Jewels",
				SecondaryStyleElement = "Gaudy Glitter"
			},
			new DesignSearch
			{
				ComponentId = 1L,
				DesignId = 2L,
				SPINumber = "SCS0001-00",
				Designer = "Jason Stevenson",
				DefaultStyleElement = "Cool Jewels",
				SecondaryStyleElement = "Gaudy Glitter"
			},
			new DesignSearch
			{
				ComponentId = 1L,
				DesignId = 2L,
				SPINumber = "SCS0001-00",
				Designer = "Jason Stevenson",
				DefaultStyleElement = "Cool Jewels",
				SecondaryStyleElement = "Gaudy Glitter"
			},
			new DesignSearch
			{
				ComponentId = 1L,
				DesignId = 2L,
				SPINumber = "SCS0001-00",
				Designer = "Jason Stevenson",
				DefaultStyleElement = "Cool Jewels",
				SecondaryStyleElement = "Gaudy Glitter"
			},
			new DesignSearch
			{
				ComponentId = 1L,
				DesignId = 2L,
				SPINumber = "SCS0001-00",
				Designer = "Jason Stevenson",
				DefaultStyleElement = "Cool Jewels",
				SecondaryStyleElement = "Gaudy Glitter"
			},
			new DesignSearch
			{
				ComponentId = 1L,
				DesignId = 2L,
				SPINumber = "SCS0001-00",
				Designer = "Jason Stevenson",
				DefaultStyleElement = "Cool Jewels",
				SecondaryStyleElement = "Gaudy Glitter"
			},
			new DesignSearch
			{
				ComponentId = 1L,
				DesignId = 2L,
				SPINumber = "SCS0001-00",
				Designer = "Jason Stevenson",
				DefaultStyleElement = "Cool Jewels",
				SecondaryStyleElement = "Gaudy Glitter"
			},
			new DesignSearch
			{
				ComponentId = 1L,
				DesignId = 2L,
				SPINumber = "SCS0001-00",
				Designer = "Jason Stevenson",
				DefaultStyleElement = "Cool Jewels",
				SecondaryStyleElement = "Gaudy Glitter"
			},
			new DesignSearch
			{
				ComponentId = 1L,
				DesignId = 2L,
				SPINumber = "SCS0001-00",
				Designer = "Jason Stevenson",
				DefaultStyleElement = "Cool Jewels",
				SecondaryStyleElement = "Gaudy Glitter"
			},
			new DesignSearch
			{
				ComponentId = 1L,
				DesignId = 2L,
				SPINumber = "SCS0001-00",
				Designer = "Jason Stevenson",
				DefaultStyleElement = "Cool Jewels",
				SecondaryStyleElement = "Gaudy Glitter"
			},
			new DesignSearch
			{
				ComponentId = 1L,
				DesignId = 2L,
				SPINumber = "SCS0001-00",
				Designer = "Jason Stevenson",
				DefaultStyleElement = "Cool Jewels",
				SecondaryStyleElement = "Gaudy Glitter"
			},
			new DesignSearch
			{
				ComponentId = 1L,
				DesignId = 2L,
				SPINumber = "SCS0001-00",
				Designer = "Jason Stevenson",
				DefaultStyleElement = "Cool Jewels",
				SecondaryStyleElement = "Gaudy Glitter"
			},
			new DesignSearch
			{
				ComponentId = 1L,
				DesignId = 2L,
				SPINumber = "SCS0001-00",
				Designer = "Jason Stevenson",
				DefaultStyleElement = "Cool Jewels",
				SecondaryStyleElement = "Gaudy Glitter"
			},
			new DesignSearch
			{
				ComponentId = 1L,
				DesignId = 2L,
				SPINumber = "SCS0001-00",
				Designer = "Jason Stevenson",
				DefaultStyleElement = "Cool Jewels",
				SecondaryStyleElement = "Gaudy Glitter"
			},
			new DesignSearch
			{
				ComponentId = 1L,
				DesignId = 2L,
				SPINumber = "SCS0001-00",
				Designer = "Jason Stevenson",
				DefaultStyleElement = "Cool Jewels",
				SecondaryStyleElement = "Gaudy Glitter"
			},
			new DesignSearch
			{
				ComponentId = 1L,
				DesignId = 2L,
				SPINumber = "SCS0001-00",
				Designer = "Jason Stevenson",
				DefaultStyleElement = "Cool Jewels",
				SecondaryStyleElement = "Gaudy Glitter"
			},
			new DesignSearch
			{
				ComponentId = 1L,
				DesignId = 2L,
				SPINumber = "SCS0001-00",
				Designer = "Jason Stevenson",
				DefaultStyleElement = "Cool Jewels",
				SecondaryStyleElement = "Gaudy Glitter"
			},
			new DesignSearch
			{
				ComponentId = 1L,
				DesignId = 2L,
				SPINumber = "SCS0001-00",
				Designer = "Jason Stevenson",
				DefaultStyleElement = "Cool Jewels",
				SecondaryStyleElement = "Gaudy Glitter"
			},
			new DesignSearch
			{
				ComponentId = 1L,
				DesignId = 2L,
				SPINumber = "SCS0001-00",
				Designer = "Jason Stevenson",
				DefaultStyleElement = "Cool Jewels",
				SecondaryStyleElement = "Gaudy Glitter"
			},
			new DesignSearch
			{
				ComponentId = 1L,
				DesignId = 2L,
				SPINumber = "SCS0001-00",
				Designer = "Jason Stevenson",
				DefaultStyleElement = "Cool Jewels",
				SecondaryStyleElement = "Gaudy Glitter"
			},
			new DesignSearch
			{
				ComponentId = 1L,
				DesignId = 2L,
				SPINumber = "SCS0001-00",
				Designer = "Jason Stevenson",
				DefaultStyleElement = "Cool Jewels",
				SecondaryStyleElement = "Gaudy Glitter"
			},
			new DesignSearch
			{
				ComponentId = 1L,
				DesignId = 2L,
				SPINumber = "SCS0001-00",
				Designer = "Jason Stevenson",
				DefaultStyleElement = "Cool Jewels",
				SecondaryStyleElement = "Gaudy Glitter"
			},
			new DesignSearch
			{
				ComponentId = 1L,
				DesignId = 2L,
				SPINumber = "SCS0001-00",
				Designer = "Jason Stevenson",
				DefaultStyleElement = "Cool Jewels",
				SecondaryStyleElement = "Gaudy Glitter"
			},
			new DesignSearch
			{
				ComponentId = 1L,
				DesignId = 2L,
				SPINumber = "SCS0001-00",
				Designer = "Jason Stevenson",
				DefaultStyleElement = "Cool Jewels",
				SecondaryStyleElement = "Gaudy Glitter"
			},
			new DesignSearch
			{
				ComponentId = 1L,
				DesignId = 2L,
				SPINumber = "SCS0001-00",
				Designer = "Jason Stevenson",
				DefaultStyleElement = "Cool Jewels",
				SecondaryStyleElement = "Gaudy Glitter"
			},
			new DesignSearch
			{
				ComponentId = 1L,
				DesignId = 2L,
				SPINumber = "SCS0001-00",
				Designer = "Jason Stevenson",
				DefaultStyleElement = "Cool Jewels",
				SecondaryStyleElement = "Gaudy Glitter"
			},
			new DesignSearch
			{
				ComponentId = 1L,
				DesignId = 2L,
				SPINumber = "SCS0001-00",
				Designer = "Jason Stevenson",
				DefaultStyleElement = "Cool Jewels",
				SecondaryStyleElement = "Gaudy Glitter"
			},
			new DesignSearch
			{
				ComponentId = 1L,
				DesignId = 2L,
				SPINumber = "SCS0001-00",
				Designer = "Jason Stevenson",
				DefaultStyleElement = "Cool Jewels",
				SecondaryStyleElement = "Gaudy Glitter"
			}
		};
	}

	private int GetGemAttibuteFromStullerShape(string stullerStoneShape)
	{
		int result = 0;
		switch (stullerStoneShape.Trim().ToLowerInvariant())
		{
		case "cushion":
			result = 279;
			break;
		case "antique square":
			result = 25;
			break;
		case "asscher":
			result = 26;
			break;
		case "emerald cut":
			result = 26;
			break;
		case "half moon":
			result = 28;
			break;
		case "heart shape":
			result = 29;
			break;
		case "marquise":
			result = 30;
			break;
		case "oval":
			result = 31;
			break;
		case "pear shape":
			result = 32;
			break;
		case "radiant":
			result = 81;
			break;
		case "round":
			result = 82;
			break;
		case "square":
			result = 33;
			break;
		case "straight baguette":
			result = 23;
			break;
		case "triangle":
			result = 83;
			break;
		case "trillion":
			result = 83;
			break;
		case "tapered baguette":
			result = 23;
			break;
		}
		return result;
	}

	private void GotoFirstPage()
	{
		PageNumber = 1;
		PopulatePageResults();
	}

	private void GotoLastPage()
	{
		PageNumber = NumberOfPages;
		PopulatePageResults();
	}

	private void GotoNextPage()
	{
		int num = PageNumber + 1;
		if (num > NumberOfPages)
		{
			num = NumberOfPages;
		}
		PageNumber = num;
		PopulatePageResults();
	}

	private void GotoPreviousPage()
	{
		int num = PageNumber - 1;
		if (num < 1)
		{
			num = 1;
		}
		PageNumber = num;
		PopulatePageResults();
	}

	private void InitializeCommands()
	{
		CommandUpdateResultsWithSelectedFilterCriteria = new RelayCommand<SelectionChangedEventArgs>(UpdateResultsWithSelectedFilterCriteria);
		CommandResetSearchActiveSearchFilters = new RelayCommand(ResetSearchActiveSearchFilters);
		CommandAddMarkedDesignsToActiveProject = new RelayCommand(AddMarkedDesignsToActiveProject);
		CommandAddMarkedDesignsToCollection = new RelayCommand(AddMarkedDesignsToCollection);
		CommandClearMarkedDesigns = new RelayCommand(ClearMarkedDesigns);
		CommandResetResults = new RelayCommand(ResetResults);
		CommandOpenDesignFromDesignSearch = new RelayCommand<DesignSearch>(OpenDesignFromDesignSearch);
		CommandSelectDesignSearch = new RelayCommand<DesignSearch>(SelectDesignSearch);
		CommandOpenDesignFromDesignIteration = new RelayCommand<DesignIteration>(OpenDesignFromDesignIteration);
		CommandSelectedVariantSearchChanged = new RelayCommand<SelectionChangedEventArgs>(SelectedVariantSearchChanged);
		CommandSelectedDesignSearchChanged = new RelayCommand(SelectedDesignSearchChanged);
		CommandAddDesignFromDesignDetailsToActiveProject = new RelayCommand(AddDesignFromDesignDetailsToActiveProject);
		CommandGemSearchFilterSizeChanged = new RelayCommand(GemSearchFilterSizeChanged);
		CommandSwitchDesignDetailImage = new RelayCommand<string>(SwitchDesignDetailImage);
		CommandTriggerSearchRefresh = new RelayCommand(TriggerSearchRefresh);
		CommandCloseDesignInfo = new RelayCommand(CloseDesignInfo);
		CommandNextDesignInfo = new RelayCommand(MoveToNextDesignInfo);
		CommandPreviousDesignInfo = new RelayCommand(MoveToPreviousDesignInfo);
		CommandOpenDesignWithSelectedShape = new RelayCommand<string>(OpenDesignWithSelectedShape);
		CommandNavigateBackToTiles = new RelayCommand(NavigateBackToTiles);
		CommandNavigateForwardToResults = new RelayCommand(NavigateForwardToResults);
		CommandUpdateSearchResultImageSizes = new RelayCommand(UpdateSearchResultImageSizes);
		CommandUpdateSearchFromFilter = new RelayCommand(UpdateSearchFromFilter);
		CommandGotoFirstPage = new RelayCommand(GotoFirstPage);
		CommandGotoLastPage = new RelayCommand(GotoLastPage);
		CommandGotoNextPage = new RelayCommand(GotoNextPage);
		CommandGotoPreviousPage = new RelayCommand(GotoPreviousPage);
		CommandRemoveFromSessionPicks = new RelayCommand<DesignSearch>(RemoveFromSessionPicks);
		CommandToggleSessionPicks = new RelayCommand(ToggleSessionPicks);
		CommandNextSessionDesign = new RelayCommand<ScrollViewer>(NextSessionDesign);
		CommandPreviousSessionDesign = new RelayCommand<ScrollViewer>(PreviousSessionDesign);
	}

	private void MoveToNextDesignInfo()
	{
		DesignSearch selectedDesignSearch = SelectedDesignSearch;
		int num = CurrentPageDesigns.IndexOf(selectedDesignSearch);
		num++;
		if (num < CurrentPageDesigns.Count())
		{
			SelectedDesignSearch = CurrentPageDesigns[num];
		}
	}

	private void MoveToPreviousDesignInfo()
	{
		DesignSearch selectedDesignSearch = SelectedDesignSearch;
		int num = CurrentPageDesigns.IndexOf(selectedDesignSearch);
		num--;
		if (num >= 0)
		{
			SelectedDesignSearch = CurrentPageDesigns[num];
		}
	}

	private void OpenDesignFromDesignIteration(DesignIteration selectedDesignIteration)
	{
		if (selectedDesignIteration != null)
		{
			GV.CounterSketch.Studio.Model.Objects.Design designFromDesignSearch = ProjectManagementHelper.GetDesignFromDesignSearch(SearchHelper.GetDesignSearchByDesignId(selectedDesignIteration.DesignId));
			designFromDesignSearch.MaterialMetal1 = selectedDesignIteration.MaterialMetal1;
			designFromDesignSearch.MaterialMetal2 = selectedDesignIteration.MaterialMetal2;
			designFromDesignSearch.MaterialGem1 = selectedDesignIteration.MaterialGem1;
			designFromDesignSearch.MaterialGem2 = selectedDesignIteration.MaterialGem2;
			designFromDesignSearch.MaterialGem3 = selectedDesignIteration.MaterialGem3;
			designFromDesignSearch.SelectedIterationId = selectedDesignIteration.IterationId;
			ProjectManagementHelper.AddDesignFromLibraryToActiveProject(designFromDesignSearch);
			LoadHelper.LoadDesign(designFromDesignSearch);
		}
	}

	private void OpenDesignFromDesignSearch(DesignSearch selectedDesign)
	{
		if (selectedDesign.IsFreeHandPart != 1 && selectedDesign.IsFreeHandPartWithGem != 1 && selectedDesign.IsStaticFreeHandPart != 1 && !ProjectManagementHelper.ContinueAfterSaveDesignPrompt())
		{
			return;
		}
		UIHelper.ShowWaitWindow(waitVisible: true);
		if (selectedDesign != null)
		{
			CloseDesignInfo();
			GV.CounterSketch.Studio.Model.Objects.Design designFromDesignSearch = ProjectManagementHelper.GetDesignFromDesignSearch(selectedDesign);
			if (selectedDesign.IsFreeHandPart == 1 || selectedDesign.IsFreeHandPartWithGem == 1 || selectedDesign.IsStaticFreeHandPart == 1)
			{
				GC.Collect(GC.MaxGeneration, GCCollectionMode.Forced);
				ViewModelLocator.Current.MainVM.IsViewportControlsVisible = true;
				UIHelper.CoreDataService.SetComponentVisibility(ComponentsEnum.Ring, visible: true);
				ViewModelLocator.Current.DesignStackVM.UpdateDesignStackDisplayComponents(isResetingComponents: true);
				EventHelper.RecordEvent(EventHelper.EventTypes.Part_Opened, "Part", selectedDesign.SPINumber);
				designFromDesignSearch.Path = FileSystemHelper.GetTempPath("countersketch", ".tmp", designFromDesignSearch.SPINumber);
				designFromDesignSearch = ProjectManagementHelper.ExtractDesignFromLibrary(designFromDesignSearch);
				int? centerGemControlGraphIndex = SearchHelper.GetCenterGemControlGraphIndex(designFromDesignSearch.DesignId);
				UIHelper.CoreDataService.DocumentImportCSGPart(designFromDesignSearch.Path, centerGemControlGraphIndex);
				ViewModelLocator.Current.CustomizeVM.IsPartToolBoxVisible = false;
				ViewModelLocator.Current.MainVM.NavigateToSection(Sections.Customize);
				UIHelper.ShowWaitWindow(waitVisible: false);
			}
			else
			{
				ProjectManagementHelper.AddDesignFromLibraryToActiveProject(designFromDesignSearch);
				LoadHelper.LoadDesign(designFromDesignSearch);
				ProjectManagementHelper.IsFileModified = false;
				ViewModelLocator.Current.BreadCrumbVM.RefreshOrderVisibility();
			}
		}
	}

	private void OpenDesignWithSelectedShape(string shape)
	{
		ViewModelLocator.Current.TileSearchMainVM.TileSearchResultDesignsVM.SelectedCenterShapeName = shape;
		OpenDesignFromDesignSearch(SelectedDesignSearch);
	}

	private void Populate()
	{
		if (base.IsInDesignMode)
		{
			DesignImageWidth = 200;
			TotalResults = 50;
			LastUsedPackageName = "Sample Package Name";
			SelectedSearchTileCategory = new TileSearchCategory
			{
				Description = "These collections represent what may be viewable\n and on-hand as a sample in the case.",
				TileImagePath = string.Format("pack://application:,,,/CounterSketch Studio;component/Images/Search/SearchTiles/{0}.jpg", "CSTile_Kits"),
				Id = 15,
				Title = "Packages/Kits"
			};
			IsPackageSearch = true;
			SearchResults = GenerateTestResults();
			CurrentPageDesigns = new ObservableCollection<DesignSearch>(SearchResults.Take(34));
			SelectedDesignSearchVariants = new ObservableCollection<DesignSearch>(SearchResults.Take(5));
			_selectedDesignSearchFeaturedIterations = new ObservableCollection<DesignIteration>();
			CurrentPageDesigns[0].IsSelected = true;
			SelectedDesignSearch = CurrentPageDesigns[0];
			DesignDetailImage = DesignDetailImagePerspective;
			RaisePropertyChanged(() => SelectedDesignSearchKeywords);
			SelectedDesignSearchShapes = new Collection<string> { "Round", "Oval", "Princess" };
			SearchFilters = new ObservableCollection<SearchFilterViewModel>
			{
				new SearchFilterViewModel(),
				new SearchFilterViewModel(),
				new SearchFilterViewModel()
			};
			ShowNextPageButton = false;
			ShowPreviousPageButton = true;
			NumberOfPages = 10;
			PageNumber = 1;
			PageSize = 36;
			Pages = new List<int> { 1, 2, 3, 4, 5 };
			TotalResults = 100;
			IsSessionPicksOpen = true;
			return;
		}
		InitializeCommands();
		if (UIHelper.CounterSketchOptions.SearchResultDesignWidth == 0)
		{
			UIHelper.CounterSketchOptions.SearchResultDesignWidth = 200;
			UIHelper.CounterSketchOptions.WriteSettings();
		}
		if (UIHelper.CounterSketchOptions.SearchResultPartWidth == 0)
		{
			UIHelper.CounterSketchOptions.SearchResultPartWidth = 150;
			UIHelper.CounterSketchOptions.WriteSettings();
		}
		if (UIHelper.CounterSketchOptions != null)
		{
			if (this is TileSearchResultPartsViewModel)
			{
				DesignImageWidth = UIHelper.CounterSketchOptions.SearchResultPartWidth;
			}
			else
			{
				DesignImageWidth = UIHelper.CounterSketchOptions.SearchResultDesignWidth;
			}
		}
		TotalResults = 0;
		_showNewDesignsFirstOption = !UIHelper.CounterSketchOptions.HideNEWRibbons;
		RaisePropertyChanged("ShowNewDesignsFirstOption");
		_showNewDesignsFirst = UIHelper.CounterSketchOptions.ShowNewDesignsFirst;
		RaisePropertyChanged("ShowNewDesignsFirst");
		IsPerspectiveView = true;
		IsFilterModeAllParts = true;
		if (this is TileSearchResultDesignsViewModel)
		{
			PageSize = UIHelper.CounterSketchOptions.SearchResultPageSizeDesigns;
		}
		else
		{
			PageSize = UIHelper.CounterSketchOptions.SearchResultPageSizeParts;
		}
		IsSessionPicksOpen = false;
	}

	private void PopulatePageResults()
	{
		if (base.IsInDesignMode)
		{
			return;
		}
		ShowResultsOrMessage(resultsVisible: true);
		if (SearchResults == null)
		{
			return;
		}
		if (SearchResults.Any())
		{
			NumberOfPages = (int)Math.Ceiling((double)SearchResults.Count / (double)PageSize);
			int start = Math.Max(1, PageNumber - 2);
			Pages = (from _ in Enumerable.Range(start, 5)
				where _ <= NumberOfPages
				select _).Take(5).ToList();
			if (Pages.Count < 5)
			{
				Pages = Enumerable.Range(1, NumberOfPages).ToList().Skip(Math.Max(0, NumberOfPages - 5))
					.ToList();
			}
			IEnumerable<DesignSearch> collection = SearchResults.Skip(PageSize * (PageNumber - 1)).Take(PageSize);
			CurrentPageDesigns = new ObservableCollection<DesignSearch>(collection);
			UpdateSearchResultImages();
			SelectedDesignSearch = null;
			RaisePropertyChanged(() => FirstDesignIndexOnPage);
			RaisePropertyChanged(() => LastDesignIndexOnPage);
			ViewModelLocator.Current.MainVM.PopulateDebugInfo($"Search Results: {SearchResults.Count}, Loaded Results: {CurrentPageDesigns.Count}");
		}
		if (PageNumber == 1)
		{
			ShowFirstPageButton = false;
		}
		else
		{
			ShowFirstPageButton = true;
		}
		if (PageNumber == NumberOfPages)
		{
			ShowLastPageButton = false;
		}
		else
		{
			ShowLastPageButton = true;
		}
		if (PageNumber == NumberOfPages)
		{
			ShowNextPageButton = false;
		}
		else
		{
			ShowNextPageButton = true;
		}
		if (PageNumber == 1)
		{
			ShowPreviousPageButton = false;
		}
		else
		{
			ShowPreviousPageButton = true;
		}
	}

	private void RemoveFromSessionPicks(DesignSearch design)
	{
		SessionPickDesigns.Remove(design);
		SearchResults.FirstOrDefault((DesignSearch _) => _.DesignId == design.DesignId)?.SetIsMarkedWithoutSearchResultUpdate(marked: false);
	}

	private void ResetResults()
	{
		SelectedSearchTileCategory = null;
		ClearSearchResults();
		_isVariantDesignIncludedInResults = false;
	}

	private void ResetSearchActiveSearchFilters()
	{
		IsFilterModeAllParts = true;
		if (!IsKeywordSearch)
		{
			ViewModelLocator.Current.TileSearchMainVM.KeywordSearch = string.Empty;
		}
		if (_isVariantDesignIncludedInResults)
		{
			if (IsKeywordSearch)
			{
				GetDesignSearchResultsByKeyword(_lastUsedKeyword);
			}
			else if (_isPackageSearch)
			{
				GetDesignSearchResultsByPackageKitName(_lastUsedPackageName);
				_isVariantDesignIncludedInResults = false;
			}
			else
			{
				GetDesignSearchResultsByTileCategory();
				_isVariantDesignIncludedInResults = false;
			}
		}
		else
		{
			if (BaseSearchResults != null)
			{
				SearchResults = new ObservableCollection<DesignSearch>(BaseSearchResults);
			}
			ResetFilters();
		}
	}

	private void SelectDesignSearch(DesignSearch designSearch)
	{
		SelectedDesignSearch = designSearch;
	}

	private void SelectedDesignSearchChanged()
	{
		ViewModelLocator.Current.TileSearchMainVM.UpdateSPIEnabled();
	}

	private void SelectedVariantSearchChanged(SelectionChangedEventArgs commandArguements)
	{
		DesignSearch designSearch = ((commandArguements.AddedItems.Count > 0) ? (commandArguements.AddedItems[0] as DesignSearch) : null);
		if (designSearch != null)
		{
			_selectedDesignSearch = designSearch;
			RaisePropertyChanged("SelectedDesignSearch");
			RaisePropertyChanged("DesignDetailImagePerspective");
			RaisePropertyChanged("DesignDetailImageFront");
			RaisePropertyChanged("DesignDetailImageSide");
			RaisePropertyChanged("DesignDetailImageTop");
			SwitchDesignDetailImage(_currentDetailImageView);
			RaisePropertyChanged("DesignDetailImage");
		}
	}

	public void StoneTrayVMStoneChosen()
	{
		_searchFilters?.ToList().ForEach(delegate(SearchFilterViewModel _)
		{
			_.UpdateDisabledStatus();
		});
		if (ViewModelLocator.Current.StoneTrayVM.IsStoneAssigned)
		{
			UpdateSearchFiltersUsingAssignedStone();
			if (ViewModelLocator.Current.MainVM.IsSearchModeActive)
			{
				SearchFilterViewModel searchFilterViewModel = _searchFilters.FirstOrDefault((SearchFilterViewModel _) => _.Title == "Center Shape");
				if (searchFilterViewModel != null)
				{
					UpdateSearchResults(searchFilterViewModel);
				}
			}
			return;
		}
		GemSearchFilterLength = null;
		GemSearchFilterWidth = null;
		_gemSearchFilterSize.Height = 0.0;
		_gemSearchFilterSize.Width = 0.0;
		if (_searchFilters == null)
		{
			return;
		}
		SearchFilterViewModel searchFilterViewModel2 = _searchFilters.FirstOrDefault((SearchFilterViewModel _) => _.Title == "Center Shape");
		if (searchFilterViewModel2 != null)
		{
			searchFilterViewModel2.SearchFilterItems.ToList().ForEach(delegate(SearchFilterItem _)
			{
				_.IsSelected = false;
			});
			searchFilterViewModel2.SelectedSearchFilterItem = searchFilterViewModel2.SearchFilterItems.FirstOrDefault();
			UpdateSearchResults(searchFilterViewModel2);
		}
	}

	private void TriggerSearchRefresh()
	{
		SearchResults = SearchResults;
		UpdateSearchResultImages();
	}

	private void UpdateResultsWithSelectedFilterCriteria(SelectionChangedEventArgs eventArgs)
	{
		if (!_isUpdatingSearchFilters)
		{
			Stopwatch.StartNew();
			NumberOfSelectedAdvancedFilters = ((SearchFilters != null) ? SearchFilters.Where((SearchFilterViewModel filter) => filter.SelectedSearchFilterItem != null).Count((SearchFilterViewModel filter) => filter.SelectedSearchFilterItem.SearchAttributeId > -1) : 0);
			if (eventArgs.AddedItems.Count != 0 && eventArgs.Source is FrameworkElement { DataContext: SearchFilterViewModel dataContext })
			{
				UpdateSearchResults(dataContext);
			}
		}
	}

	private void UpdateSearchFromFilter()
	{
		IEnumerable<DesignSearch> source = (IsFilterModeStaticPartsOnly ? BaseSearchResults.Where((DesignSearch _) => _.IsStaticFreeHandPart == 1) : (IsFilterModeCounterPartsOnly ? BaseSearchResults.Where((DesignSearch _) => _.IsFreeHandPart == 1 || _.IsFreeHandPartWithGem == 1) : ((!IsFilterModeHasGem) ? BaseSearchResults : BaseSearchResults.Where((DesignSearch _) => _.IsFreeHandPartWithGem == 1))));
		IEnumerable<SearchFilterItem> activeFilterItems = from filter in SearchFilters
			where filter.SelectedSearchFilterItem != null && filter.SelectedSearchFilterItem.PrimaryText != TranslationHelper.GetTranslatedString("Show All")
			select filter.SelectedSearchFilterItem;
		SearchFilterViewModel searchFilterViewModel = SearchFilters.FirstOrDefault((SearchFilterViewModel filter) => filter.Title == "Design Element" && filter.SelectedSearchFilterItem != null);
		SearchFilterItem searchFilterItem = null;
		if (searchFilterViewModel != null && searchFilterViewModel.SelectedSearchFilterItem != null)
		{
			searchFilterItem = searchFilterViewModel.SelectedSearchFilterItem;
		}
		SearchResults = new ObservableCollection<DesignSearch>(SearchHelper.UpdateDesignResultsWithFilters(activeFilterItems, source.Select((DesignSearch result) => result.DesignId).ToList(), searchFilterItem));
	}

	private async Task UpdateSearchResultImages()
	{
		if (SearchResults == null || CurrentPageDesigns == null || CurrentPageDesigns.Count < 1 || base.IsInDesignMode)
		{
			return;
		}
		IsBusyLoadingImages = true;
		ResetImageUpdateTasks();
		CancellationToken token = _cancellationTokenSource.Token;
		try
		{
			await Task.WhenAll(CurrentPageDesigns.Select((DesignSearch t) => Task.Run(delegate
			{
				t.UpdateImageSource();
			}, token)));
		}
		catch (TaskCanceledException)
		{
		}
		IsBusyLoadingImages = false;
	}
}
