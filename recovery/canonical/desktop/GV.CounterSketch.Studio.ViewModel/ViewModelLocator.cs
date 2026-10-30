using CommonServiceLocator;
using GalaSoft.MvvmLight.Ioc;

namespace GV.CounterSketch.Studio.ViewModel;

public sealed class ViewModelLocator
{
	private static GemReportViewModel _gemReportViewModel;

	private static OrderSummaryViewModel _orderSummaryViewModel;

	private WindowOrderHistoryViewModel _windowOrderHistoryVm;

	public DebugDesignInfoViewmodel DebugDesignInfoVM => ServiceLocator.Current.GetInstance<DebugDesignInfoViewmodel>();

	public SearchFilterViewModel SampleSearchFilterViewModelVm => ServiceLocator.Current.GetInstance<SearchFilterViewModel>();

	public ItemChooserViewModelBase ItemChooserVM => ServiceLocator.Current.GetInstance<ItemChooserViewModelBase>();

	public MainViewModel MainVM => ServiceLocator.Current.GetInstance<MainViewModel>();

	public TileViewDesignsViewModel TileViewDesignsVM => ServiceLocator.Current.GetInstance<TileViewDesignsViewModel>();

	public TileViewPartsViewModel TileViewPartsVM => ServiceLocator.Current.GetInstance<TileViewPartsViewModel>();

	public UserPartsViewModel UserPartsVM => ServiceLocator.Current.GetInstance<UserPartsViewModel>();

	public CustomizeViewModel CustomizeVM => ServiceLocator.Current.GetInstance<CustomizeViewModel>();

	public BackupRestoreViewModel BackupRestoreVM => ServiceLocator.Current.GetInstance<BackupRestoreViewModel>();

	public DesignImageSummaryViewModel DesignImageSummaryVM => ServiceLocator.Current.GetInstance<DesignImageSummaryViewModel>();

	public DesignStackViewModel DesignStackVM => ServiceLocator.Current.GetInstance<DesignStackViewModel>();

	public PreferenceMaterialsViewModel PreferenceMaterialsVM => ServiceLocator.Current.GetInstance<PreferenceMaterialsViewModel>();

	public PreferenceOrderScreenOptionsViewModel PreferenceOrderScreenVM => ServiceLocator.Current.GetInstance<PreferenceOrderScreenOptionsViewModel>();

	public FreehandStackViewModel FreehandStackVM => ServiceLocator.Current.GetInstance<FreehandStackViewModel>();

	public WindowOrderResultViewModel WindowOrderResultVM => ServiceLocator.Current.GetInstance<WindowOrderResultViewModel>();

	public PortfolioManagerViewModel PortfolioManagerVM => ServiceLocator.Current.GetInstance<PortfolioManagerViewModel>();

	public StudioMaterialSelectionViewModel StudioMaterialSelectionVM => ServiceLocator.Current.GetInstance<StudioMaterialSelectionViewModel>();

	public ThemePickerViewModel ThemePickerVM => ServiceLocator.Current.GetInstance<ThemePickerViewModel>();

	public TileSearchMainViewModel TileSearchMainVM => ServiceLocator.Current.GetInstance<TileSearchMainViewModel>();

	public UpdateInfoViewModel UpdateInfoVM => ServiceLocator.Current.GetInstance<UpdateInfoViewModel>();

	public WindowDesignTryOnViewModel WindowDesignTryOnVM => ServiceLocator.Current.GetInstance<WindowDesignTryOnViewModel>();

	public WindowExportDesignViewModel WindowExportDesignVM => ServiceLocator.Current.GetInstance<WindowExportDesignViewModel>();

	public WindowExportSearchResultsViewModel WindowExportSearchResultsVM => ServiceLocator.Current.GetInstance<WindowExportSearchResultsViewModel>();

	public WindowRenderOptionsViewModel WindowRenderOptionsVM => ServiceLocator.Current.GetInstance<WindowRenderOptionsViewModel>();

	public StoneTrayViewModel StoneTrayVM => ServiceLocator.Current.GetInstance<StoneTrayViewModel>();

	public QuickEstimateViewModel QuickEstimateVM => ServiceLocator.Current.GetInstance<QuickEstimateViewModel>();

	public static ViewModelLocator Current { get; private set; }

	public GemReportViewModel GemReportVM
	{
		get
		{
			if (_gemReportViewModel == null)
			{
				_gemReportViewModel = new GemReportViewModel();
			}
			return _gemReportViewModel;
		}
		set
		{
			_gemReportViewModel = value;
		}
	}

	public WindowOrderHistoryViewModel WindowOrderHistoryVM
	{
		get
		{
			if (_windowOrderHistoryVm == null)
			{
				_windowOrderHistoryVm = new WindowOrderHistoryViewModel();
			}
			return _windowOrderHistoryVm;
		}
		set
		{
			_windowOrderHistoryVm = value;
		}
	}

	public OrderSummaryViewModel OrderSummaryVM
	{
		get
		{
			if (_orderSummaryViewModel == null)
			{
				_orderSummaryViewModel = new OrderSummaryViewModel();
			}
			return _orderSummaryViewModel;
		}
		set
		{
			_orderSummaryViewModel = value;
		}
	}

	public CustomizePartsListViewModel CustomizePartsListVM => ServiceLocator.Current.GetInstance<CustomizePartsListViewModel>();

	public BreadCrumbViewModel BreadCrumbVM => ServiceLocator.Current.GetInstance<BreadCrumbViewModel>();

	public ViewModelLocator()
	{
		if (Current == null)
		{
			ServiceLocator.SetLocatorProvider(() => SimpleIoc.Default);
			SimpleIoc.Default.Register<BackupRestoreViewModel>();
			SimpleIoc.Default.Register<BreadCrumbViewModel>();
			SimpleIoc.Default.Register<CustomizeViewModel>();
			SimpleIoc.Default.Register<CustomizePartsListViewModel>();
			SimpleIoc.Default.Register<DesignImageSummaryViewModel>();
			SimpleIoc.Default.Register<DesignStackViewModel>();
			SimpleIoc.Default.Register<FreehandStackViewModel>();
			SimpleIoc.Default.Register<ItemChooserViewModelBase>();
			SimpleIoc.Default.Register<MainViewModel>();
			SimpleIoc.Default.Register<PortfolioManagerViewModel>();
			SimpleIoc.Default.Register<PreferenceMaterialsViewModel>();
			SimpleIoc.Default.Register<PreferenceOrderScreenOptionsViewModel>();
			SimpleIoc.Default.Register<QuickEstimateViewModel>();
			SimpleIoc.Default.Register<SearchFilterViewModel>();
			SimpleIoc.Default.Register<StoneTrayViewModel>();
			SimpleIoc.Default.Register<StudioMaterialSelectionViewModel>();
			SimpleIoc.Default.Register<ThemePickerViewModel>();
			SimpleIoc.Default.Register<TileSearchMainViewModel>();
			SimpleIoc.Default.Register<TileViewDesignsViewModel>();
			SimpleIoc.Default.Register<TileViewPartsViewModel>();
			SimpleIoc.Default.Register<UpdateInfoViewModel>();
			SimpleIoc.Default.Register<UserPartsViewModel>();
			SimpleIoc.Default.Register<WindowDesignTryOnViewModel>();
			SimpleIoc.Default.Register<WindowExportDesignViewModel>();
			SimpleIoc.Default.Register<WindowExportSearchResultsViewModel>();
			SimpleIoc.Default.Register<WindowOrderResultViewModel>();
			SimpleIoc.Default.Register<WindowRenderOptionsViewModel>();
			SimpleIoc.Default.Register<DebugDesignInfoViewmodel>();
			GlobalErrorHandler.WriteInfoToEventLog("Building the UI - Creating VM Locator.", 2);
			Current = this;
		}
	}

	public static void Cleanup()
	{
	}
}
