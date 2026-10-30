using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Forms;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Threading;
using GV.CounterSketch.Common.Utilities.Helpers;
using GV.CounterSketch.Studio.Controls;
using GV.CounterSketch.Studio.Database;
using GV.CounterSketch.Studio.Model.Entities.CounterSketchLibrary;
using GV.CounterSketch.Studio.Model.Objects;
using GV.CounterSketch.Studio.Model.Objects.Enums;
using GV.CounterSketch.Studio.SecurityServices;
using GV.CounterSketch.Studio.Utilities;
using GV.CounterSketch.Studio.Utilities.Helpers;
using GV.CounterSketch.Studio.View;
using GV.CounterSketch.Studio.WinForms;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Messaging;
using Localization;

namespace GV.CounterSketch.Studio.ViewModel;

public class MainViewModel : ViewModelBase
{
	private const int _maxSectionNumber = 2;

	private static string _libraryVersion;

	private static int _modelCount;

	private static string _rhinoVersion;

	private readonly DispatcherTimer _isRhinoBusyDispatcherTimer;

	private BitmapImage _icon;

	private bool _isLiveChatVisible;

	private List<double> _keyTimes;

	private bool _scrollSection;

	private List<IEasingFunction> _sectionEasingFunctions;

	private List<EasingMode> _sectionEasingModes;

	private int _sectionPageNumber;

	private KeyTime _sectionTransitionTime;

	private double _sectionWidth;

	private IEasingFunction _selectedEasingFunction;

	private EasingMode _selectedEasingMode;

	private double _selectedKeyTime;

	private string _title;

	private string _activePortfolioName;

	private int _backupProgressBarValue;

	private bool _backupProgressBarVisibility;

	private bool _camModeEnabled;

	private bool _isCustomizeModeActive;

	private bool _isFreehandModeActive;

	private bool _isStreaming;

	private bool _ismMenuToggleButtonChecked;

	private bool _isOrderedDesignLoaded;

	private bool _isOrderModeActive;

	private bool _isSearchModeActive;

	private bool _isViewportControlsVisible;

	private string _prototypeKitZipPath;

	private Visibility _quickEstimateVisibility;

	private BitmapImage _rhinoCapture;

	private string _saveProjectString;

	private bool _showAdminMenu;

	private bool _showDebugMenu;

	private bool _showViewportControlHandleToggle;

	private string _sPiNumber;

	private WindowBrowser _styleQuizBrowser;

	private WindowRenderOptionsView _theWindow;

	private WindowDesignTryOnView _windowDesignTryOnView;

	private WindowGemReportView _windowGemReporter;

	private ObservableCollection<LookBookPage> _lookBookPages;

	private WindowModalOptions _windowModalOptions;

	private bool _isSaving;

	private FrmPrintToProJet1200 _frmPrintToProJet1200;

	private bool _isRhinoBusy;

	private bool _isCommonCenterPointToggled = true;

	private bool _isFreehandOnly;

	private string _debugInfoText;

	private int _timedBackupSeconds;

	private bool _isTimedBackupEnabled;

	private bool _isTester;

	private double _uIScaleInverseFactor;

	public string CountryCode
	{
		get
		{
			if (UIHelper.CounterSketchOptions != null)
			{
				return UIHelper.CounterSketchOptions.CountryCode;
			}
			return string.Empty;
		}
	}

	public Thickness SectionMargin => new Thickness((double)(SectionPageNumber * -1) * SectionWidth, 0.0, 0.0, 0.0);

	public bool IsChatDisabledForInternationalKeys => UIHelper.KeyProfile.KeyProfileId == 3;

	public bool IsFreehandOnly
	{
		get
		{
			return _isFreehandOnly;
		}
		set
		{
			_isFreehandOnly = value;
			RaisePropertyChanged("IsFreehandOnly");
		}
	}

	public string DebugInfoText
	{
		get
		{
			return _debugInfoText;
		}
		set
		{
			_debugInfoText = value;
			RaisePropertyChanged("DebugInfoText");
		}
	}

	public bool IsCommonCenterPointToggled
	{
		get
		{
			return _isCommonCenterPointToggled;
		}
		set
		{
			if (value != _isCommonCenterPointToggled)
			{
				_isCommonCenterPointToggled = value;
				UIHelper.CoreDataService.ToggleFreehandCommonCenterMode(_isCommonCenterPointToggled);
				if (_isCommonCenterPointToggled)
				{
					EventHelper.RecordEvent(EventHelper.EventTypes.Customize_CommonCenter);
				}
				RaisePropertyChanged("IsCommonCenterPointToggled");
			}
		}
	}

	public RelayCommand CommandInstallProjetUsbDrivers { get; protected set; }

	public RelayCommand CommandCreateQuickPortfolio { get; set; }

	public string ActivePortfolioName
	{
		get
		{
			return _activePortfolioName;
		}
		set
		{
			_activePortfolioName = value;
			RaisePropertyChanged(() => ActivePortfolioName);
		}
	}

	public int BackupProgressBarValue
	{
		get
		{
			return _backupProgressBarValue;
		}
		set
		{
			_backupProgressBarValue = value;
			RaisePropertyChanged(() => BackupProgressBarValue);
		}
	}

	public bool BackupProgressBarVisibility
	{
		get
		{
			return _backupProgressBarVisibility;
		}
		set
		{
			_backupProgressBarVisibility = value;
			RaisePropertyChanged(() => BackupProgressBarVisibility);
		}
	}

	public RelayCommand CommandCloseCounterSketch { get; set; }

	public RelayCommand<FrameworkElement> CommandCopyFrameworkElementToClipboard { get; set; }

	public RelayCommand CommandSetCamModeState { get; set; }

	public RelayCommand CommandWindowSize1080 { get; set; }

	public RelayCommand CommandWindowSize1050 { get; set; }

	public RelayCommand CommandCreateBugWorkItem { get; set; }

	public RelayCommand CommandCreateProductBacklogWorkItem { get; set; }

	public RelayCommand CommandDesignFeedback { get; set; }

	public RelayCommand CommandDesignTryOn { get; set; }

	public RelayCommand CommandDisableRhinoViewportGridlines { get; set; }

	public RelayCommand<string> CommandToggleFreehandCommonCenterMode { get; set; }

	public RelayCommand CommandDisableStackHighlight { get; set; }

	public RelayCommand CommandDownloadTutorials { get; set; }

	public RelayCommand CommandEditTileLayoutDesigns { get; set; }

	public RelayCommand CommandEditTileLayoutParts { get; set; }

	public RelayCommand CommandEnableRhinoViewportGridlines { get; set; }

	public RelayCommand CommandEnableStackHighlight { get; set; }

	public RelayCommand CommandExportSearchImages { get; set; }

	public RelayCommand CommandExportDesign { get; set; }

	public RelayCommand CommandFreeze { get; set; }

	public RelayCommand CommandHideViewportControls { get; set; }

	public RelayCommand CommandImportDesign { get; set; }

	public RelayCommand CommandImportPart { get; set; }

	public RelayCommand CommandNewSessionClick { get; set; }

	public RelayCommand CommandOpenAnyGemAnyColorMaterialSelection { get; set; }

	public RelayCommand CommandOpenAboutWindow { get; set; }

	public RelayCommand CommandOpenQuickEstimateWindow { get; set; }

	public RelayCommand CommandOpenSearchKeywordWindow { get; set; }

	public RelayCommand CommandOpenAccountInfoWindow { get; set; }

	public RelayCommand CommandOpenBackupOptions { get; set; }

	public RelayCommand CommandOpenDesignTryOnWindow { get; set; }

	public RelayCommand<object> CommandOpenLink { get; set; }

	public RelayCommand CommandOpenLiveChatWindow { get; set; }

	public RelayCommand CommandOpenQuoteHistory { get; set; }

	public RelayCommand CommandOpenPortfolioManager { get; set; }

	public RelayCommand<CSPreferenceTabs> CommandOpenPreferencesWindow { get; set; }

	public RelayCommand CommandOpenPrintCatalogHistoryWindow { get; set; }

	public RelayCommand CommandOpenPrintCatalogWindow { get; set; }

	public RelayCommand CommandOpenReleaseNotesLibraryWindow { get; set; }

	public RelayCommand CommandOpenReleaseNotesWindow { get; set; }

	public RelayCommand CommandOpenResourceImageView { get; set; }

	public RelayCommand CommandOpenStyleQuizWindow { get; set; }

	public RelayCommand CommandOpenTeamViewer { get; set; }

	public RelayCommand CommandOpenTroubleshootingWindow { get; set; }

	public RelayCommand CommandOpenUserGuide { get; set; }

	public RelayCommand CommandOpenUserGuideAddendum { get; set; }

	public RelayCommand CommandOpenCamStandards { get; set; }

	public RelayCommand CommandOpenCamMountingForm { get; set; }

	public RelayCommand CommandOpenCamStoneForm { get; set; }

	public RelayCommand CommandOpenUsersOptions { get; set; }

	public RelayCommand CommandQuickBackup { get; set; }

	public RelayCommand CommandRedo { get; set; }

	public RelayCommand CommandResetOrderInformation { get; set; }

	public RelayCommand CommandSaveCurrentDesign { get; set; }

	public RelayCommand<object> CommandSendEmail { get; set; }

	public RelayCommand CommandSendFeedback { get; set; }

	public RelayCommand CommandSendQuickDebugReport { get; set; }

	public RelayCommand CommandSetCustomizeModeActive { get; set; }

	public RelayCommand CommandSetFreehandModeActive { get; protected set; }

	public RelayCommand CommandSetOrderModeActive { get; protected set; }

	public RelayCommand CommandSetRhinoCameraViewFront { get; set; }

	public RelayCommand CommandSetRhinoCameraViewPerspective { get; set; }

	public RelayCommand CommandSetRhinoCameraViewSide { get; set; }

	public RelayCommand CommandSetRhinoCameraViewTop { get; set; }

	public RelayCommand CommandGenerateRender { get; set; }

	public RelayCommand CommandOpenRenderDialogue { get; set; }

	public RelayCommand CommandSetSearchModeActive { get; set; }

	public RelayCommand CommandSpinDesign { get; set; }

	public RelayCommand CommandSpinDesignStop { get; set; }

	public RelayCommand CommandUndo { get; set; }

	public RelayCommand CommandUnFreeze { get; set; }

	public RelayCommand CommandUnregisterComputer { get; set; }

	public RelayCommand<string> CommandUseAlternateCountryCode { get; set; }

	public RelayCommand CommandViewCounterSketchLog { get; set; }

	public RelayCommand CommandViewGemInfo { get; set; }

	public RelayCommand CommandViewNeedsTestingBugs { get; set; }

	public RelayCommand CommandViewOpenBugs { get; set; }

	public RelayCommand CommandViewProductBacklog { get; set; }

	public RelayCommand CommandOpenPrintToProJet1200 { get; set; }

	public RelayCommand CommandOpenPrintToEnvisionTec { get; set; }

	public RelayCommand CommandCheckProjetAndUpsell { get; protected set; }

	public RelayCommand CommandPerformDistanceMeasurement { get; protected set; }

	public Visibility QuickEstimateVisibility
	{
		get
		{
			return _quickEstimateVisibility;
		}
		set
		{
			_quickEstimateVisibility = value;
			RaisePropertyChanged("QuickEstimateVisibility");
		}
	}

	public bool IsCustomizeModeActive
	{
		get
		{
			return _isCustomizeModeActive;
		}
		set
		{
			if (_isCustomizeModeActive != value)
			{
				_isCustomizeModeActive = value;
				RaisePropertyChanged(() => IsCustomizeModeActive);
			}
		}
	}

	public bool IsProjetPrinterSupportEnabled
	{
		get
		{
			return UIHelper.CounterSketchOptions.EnableProjetPrinterSupport;
		}
		set
		{
			UIHelper.CounterSketchOptions.EnableProjetPrinterSupport = value;
			UIHelper.CounterSketchOptions.WriteSettings();
			RaisePropertyChanged("IsProjetPrinterSupportEnabled");
		}
	}

	public bool IsWhatsNewVisibleMainUI
	{
		get
		{
			return UIHelper.CounterSketchOptions.IsWhatsNewVisibleMainUI;
		}
		set
		{
			UIHelper.CounterSketchOptions.IsWhatsNewVisibleMainUI = value;
			UIHelper.CounterSketchOptions.WriteSettings();
			RaisePropertyChanged("IsWhatsNewVisibleMainUI");
		}
	}

	public bool IsFreehandModeActive
	{
		get
		{
			return _isFreehandModeActive;
		}
		set
		{
			_isFreehandModeActive = value;
			RaisePropertyChanged(() => IsFreehandModeActive);
		}
	}

	public bool IsStreaming
	{
		get
		{
			return _isStreaming;
		}
		set
		{
			_isStreaming = value;
			RaisePropertyChanged("IsStreaming");
		}
	}

	public bool IsMenuToggleButtonChecked
	{
		get
		{
			return _ismMenuToggleButtonChecked;
		}
		set
		{
			_ismMenuToggleButtonChecked = value;
			RaisePropertyChanged(() => IsMenuToggleButtonChecked);
		}
	}

	public bool IsRhinoBusy
	{
		[DebuggerStepThrough]
		get
		{
			return _isRhinoBusy;
		}
		[DebuggerStepThrough]
		set
		{
			_isRhinoBusy = value;
			RaisePropertyChanged("IsRhinoBusy");
		}
	}

	public bool IsSearchModeActive
	{
		get
		{
			return _isSearchModeActive;
		}
		set
		{
			if (_isSearchModeActive != value)
			{
				_isSearchModeActive = value;
				RaisePropertyChanged("IsSearchModeActive");
				RaisePropertyChanged("ShowCanvasControls");
			}
		}
	}

	public bool IsViewportControlsVisible
	{
		get
		{
			return _isViewportControlsVisible;
		}
		set
		{
			_isViewportControlsVisible = value;
			RaisePropertyChanged(() => IsViewportControlsVisible);
			if (!base.IsInDesignMode)
			{
				UIHelper.CoreDataService.ToggleViewportControls(value);
			}
		}
	}

	public ObservableCollection<LookBookPage> LookBookPages
	{
		get
		{
			return _lookBookPages;
		}
		set
		{
			_lookBookPages = value;
			RaisePropertyChanged("LookBookPages");
		}
	}

	public BitmapImage RhinoCapture
	{
		get
		{
			return _rhinoCapture;
		}
		set
		{
			_rhinoCapture = value;
			RaisePropertyChanged(() => RhinoCapture);
		}
	}

	public bool IsSaving
	{
		get
		{
			return _isSaving;
		}
		set
		{
			_isSaving = value;
			RaisePropertyChanged("IsSaving");
		}
	}

	public string SaveProjectString
	{
		get
		{
			return _saveProjectString;
		}
		set
		{
			_saveProjectString = value;
			RaisePropertyChanged("SaveProjectString");
		}
	}

	public bool ShowAdminMenu
	{
		get
		{
			return _showAdminMenu;
		}
		set
		{
			_showAdminMenu = value;
			RaisePropertyChanged(() => ShowAdminMenu);
		}
	}

	public bool ShowDebugMenu
	{
		get
		{
			return _showDebugMenu;
		}
		set
		{
			_showDebugMenu = value;
			RaisePropertyChanged(() => ShowDebugMenu);
		}
	}

	public bool IsOrderedDesignLoaded
	{
		get
		{
			return _isOrderedDesignLoaded;
		}
		set
		{
			_isOrderedDesignLoaded = value;
			RaisePropertyChanged(() => IsOrderedDesignLoaded);
		}
	}

	public bool IsOrderModeActive
	{
		get
		{
			return _isOrderModeActive;
		}
		set
		{
			if (value)
			{
				ViewModelLocator.Current.OrderSummaryVM.Populate();
			}
			_isOrderModeActive = value;
			RaisePropertyChanged("IsOrderModeActive");
		}
	}

	public bool ShowViewportControlHandleToggle
	{
		get
		{
			return _showViewportControlHandleToggle;
		}
		set
		{
			_showViewportControlHandleToggle = value;
			RaisePropertyChanged("ShowViewportControlHandleToggle");
		}
	}

	public static WindowAnyGemAnyColorView WindowAnyGemAnyColor { get; set; }

	public EasingMode SelectedEasingMode
	{
		get
		{
			return _selectedEasingMode;
		}
		set
		{
			_selectedEasingMode = value;
			RaisePropertyChanged("SelectedEasingMode");
		}
	}

	public List<EasingMode> SectionEasingModes
	{
		get
		{
			return _sectionEasingModes;
		}
		set
		{
			_sectionEasingModes = value;
			RaisePropertyChanged("SectionEasingModes");
		}
	}

	public List<double> KeyTimes
	{
		get
		{
			return _keyTimes;
		}
		set
		{
			_keyTimes = value;
			RaisePropertyChanged("KeyTimes");
		}
	}

	public KeyTime SectionTransitionTime
	{
		get
		{
			return _sectionTransitionTime;
		}
		set
		{
			_sectionTransitionTime = value;
			RaisePropertyChanged("SectionTransitionTime");
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
		}
	}

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
			TriggerSectionAdjustment();
		}
	}

	public double SelectedKeyTime
	{
		get
		{
			return _selectedKeyTime;
		}
		set
		{
			_selectedKeyTime = value;
			RaisePropertyChanged("SelectedKeyTime");
			KeyTime sectionTransitionTime = KeyTime.FromTimeSpan(TimeSpan.FromSeconds(value));
			SectionTransitionTime = sectionTransitionTime;
			if (value != UIHelper.CounterSketchOptions.AnimationTransitionTime)
			{
				UIHelper.CounterSketchOptions.AnimationTransitionTime = value;
				UIHelper.CounterSketchOptions.WriteSettings();
			}
		}
	}

	public IEasingFunction SelectedEasingFunction
	{
		get
		{
			return _selectedEasingFunction;
		}
		set
		{
			_selectedEasingFunction = value;
			RaisePropertyChanged("SelectedEasingFunction");
		}
	}

	public List<IEasingFunction> SectionEasingFunctions
	{
		get
		{
			return _sectionEasingFunctions;
		}
		set
		{
			_sectionEasingFunctions = value;
			RaisePropertyChanged("SectionEasingFunctions");
		}
	}

	public int PreviousSectionPageNumber { get; set; }

	public int SectionPageNumber
	{
		get
		{
			return _sectionPageNumber;
		}
		set
		{
			PreviousSectionPageNumber = _sectionPageNumber;
			_sectionPageNumber = value;
			RaisePropertyChanged("SectionPageNumber");
			RaisePropertyChanged(() => SectionMargin);
		}
	}

	public RelayCommand CommandMoveForward { get; set; }

	public RelayCommand CommandMaximizeRestoreMainWindow { get; set; }

	public RelayCommand CommandMoveBackward { get; set; }

	public BitmapImage Icon
	{
		get
		{
			return _icon;
		}
		set
		{
			_icon = value;
			RaisePropertyChanged("Icon");
		}
	}

	public string Title
	{
		get
		{
			return _title;
		}
		set
		{
			_title = value;
			RaisePropertyChanged("Title");
		}
	}

	public bool IsLiveChatVisible
	{
		get
		{
			return _isLiveChatVisible;
		}
		set
		{
			_isLiveChatVisible = value;
			RaisePropertyChanged("IsLiveChatVisible");
		}
	}

	public string SPINumber
	{
		get
		{
			return _sPiNumber;
		}
		set
		{
			_sPiNumber = value;
			RaisePropertyChanged("SPINumber");
		}
	}

	public bool IsDisplayingRedboxLogo
	{
		get
		{
			return UIHelper.CounterSketchOptions.DisplayRedboxLogo;
		}
		set
		{
			RaisePropertyChanged("IsDisplayingRedboxLogo");
		}
	}

	public RelayCommand CommandOpenProjetInstructions { get; protected set; }

	public RelayCommand CommandToggleWindowMaximized { get; set; }

	public int TimedBackupSeconds
	{
		get
		{
			return _timedBackupSeconds;
		}
		set
		{
			_timedBackupSeconds = value;
			RaisePropertyChanged("TimedBackupSeconds");
		}
	}

	public bool IsTimedBackupEnabled
	{
		get
		{
			return _isTimedBackupEnabled;
		}
		set
		{
			_isTimedBackupEnabled = value;
			RaisePropertyChanged("IsTimedBackupEnabled");
		}
	}

	public double UIScaleInverseFactor
	{
		get
		{
			return _uIScaleInverseFactor;
		}
		set
		{
			_uIScaleInverseFactor = value;
			RaisePropertyChanged("UIScaleInverseFactor");
		}
	}

	public RelayCommand<string> CommandExcuteTest { get; set; } = new RelayCommand<string>(ExecuteTest);

	public bool IsTester
	{
		get
		{
			return _isTester;
		}
		set
		{
			_isTester = value;
			RaisePropertyChanged("IsTester");
		}
	}

	public RelayCommand CommandShopForDiamond => new RelayCommand(ShopForDiamond);

	public RelayCommand CommandShopForGemstone => new RelayCommand(ShopForGemstone);

	public MainViewModel()
	{
		if (base.IsInDesignMode)
		{
			IsSearchModeActive = true;
		}
		if (!base.IsInDesignMode)
		{
			InitializeUI();
			InitializeCommands();
			_isRhinoBusyDispatcherTimer = new DispatcherTimer(new TimeSpan(0, 0, 0, 0, 2400), DispatcherPriority.Background, UpdateIsBusy, System.Windows.Application.Current.Dispatcher);
			ObservableCollection<LookBookPage> lookBookPages = new ObservableCollection<LookBookPage>
			{
				new LookBookPage
				{
					ImagePath = "pack://application:,,,/CounterSketch Studio;component/Images/MoreThanARingImages/MoreThanARing_1.png"
				},
				new LookBookPage
				{
					ImagePath = "pack://application:,,,/CounterSketch Studio;component/Images/MoreThanARingImages/MoreThanARing_Blank.png"
				},
				new LookBookPage
				{
					ImagePath = "pack://application:,,,/CounterSketch Studio;component/Images/MoreThanARingImages/MoreThanARing_2.png"
				},
				new LookBookPage
				{
					ImagePath = "pack://application:,,,/CounterSketch Studio;component/Images/MoreThanARingImages/MoreThanARing_Blank.png"
				},
				new LookBookPage
				{
					ImagePath = "pack://application:,,,/CounterSketch Studio;component/Images/MoreThanARingImages/MoreThanARing_3.png"
				},
				new LookBookPage
				{
					ImagePath = "pack://application:,,,/CounterSketch Studio;component/Images/MoreThanARingImages/MoreThanARing_Blank.png"
				},
				new LookBookPage
				{
					ImagePath = "pack://application:,,,/CounterSketch Studio;component/Images/MoreThanARingImages/MoreThanARing_4.png"
				},
				new LookBookPage
				{
					ImagePath = "pack://application:,,,/CounterSketch Studio;component/Images/MoreThanARingImages/MoreThanARing_Blank.png"
				},
				new LookBookPage
				{
					ImagePath = "pack://application:,,,/CounterSketch Studio;component/Images/MoreThanARingImages/MoreThanARing_5.png"
				},
				new LookBookPage
				{
					ImagePath = "pack://application:,,,/CounterSketch Studio;component/Images/MoreThanARingImages/MoreThanARing_Blank.png"
				},
				new LookBookPage
				{
					ImagePath = "pack://application:,,,/CounterSketch Studio;component/Images/MoreThanARingImages/MoreThanARing_6.png"
				},
				new LookBookPage
				{
					ImagePath = "pack://application:,,,/CounterSketch Studio;component/Images/MoreThanARingImages/MoreThanARing_Blank.png"
				},
				new LookBookPage
				{
					ImagePath = "pack://application:,,,/CounterSketch Studio;component/Images/MoreThanARingImages/MoreThanARing_7.png"
				},
				new LookBookPage
				{
					ImagePath = "pack://application:,,,/CounterSketch Studio;component/Images/MoreThanARingImages/MoreThanARing_Blank.png"
				},
				new LookBookPage
				{
					ImagePath = "pack://application:,,,/CounterSketch Studio;component/Images/MoreThanARingImages/MoreThanARing_8.png"
				},
				new LookBookPage
				{
					ImagePath = "pack://application:,,,/CounterSketch Studio;component/Images/MoreThanARingImages/MoreThanARing_Blank.png"
				},
				new LookBookPage
				{
					ImagePath = "pack://application:,,,/CounterSketch Studio;component/Images/MoreThanARingImages/MoreThanARing_9.png"
				},
				new LookBookPage
				{
					ImagePath = "pack://application:,,,/CounterSketch Studio;component/Images/MoreThanARingImages/MoreThanARing_Blank.png"
				},
				new LookBookPage
				{
					ImagePath = "pack://application:,,,/CounterSketch Studio;component/Images/MoreThanARingImages/MoreThanARing_10.png"
				},
				new LookBookPage
				{
					ImagePath = "pack://application:,,,/CounterSketch Studio;component/Images/MoreThanARingImages/MoreThanARing_Blank.png"
				},
				new LookBookPage
				{
					ImagePath = "pack://application:,,,/CounterSketch Studio;component/Images/MoreThanARingImages/MoreThanARing_11.png"
				},
				new LookBookPage
				{
					ImagePath = "pack://application:,,,/CounterSketch Studio;component/Images/MoreThanARingImages/MoreThanARing_Blank.png"
				},
				new LookBookPage
				{
					ImagePath = "pack://application:,,,/CounterSketch Studio;component/Images/MoreThanARingImages/MoreThanARing_12.png"
				},
				new LookBookPage
				{
					ImagePath = "pack://application:,,,/CounterSketch Studio;component/Images/MoreThanARingImages/MoreThanARing_Blank.png"
				},
				new LookBookPage
				{
					ImagePath = "pack://application:,,,/CounterSketch Studio;component/Images/MoreThanARingImages/MoreThanARing_13.png"
				}
			};
			LookBookPages = lookBookPages;
			UIHelper.CoreDataService.ToggleRhinoViewportAxis(enableAxis: false);
			UIHelper.CoreDataService.ToggleRhinoViewportGridlines(enableGridlines: false);
			UIHelper.CoreDataService.UpdateViewPortHandleDrawScale(UIHelper.CounterSketchOptions.ViewPortHandleScaleFactor);
			UIHelper.CoreDataService.SetIsBlockParallelProcessingEnabled(UIHelper.CounterSketchOptions.IsBlockParallelProcessingEnabled);
			IsFreehandOnly = UIHelper.KeyProfile.IsDesignLibraryDisabled;
			SetQeOptionVisibility(UIHelper.CounterSketchOptions.HideQuickEstimate);
			PopulateDebugInfo();
		}
	}

	public void CloseAnyGemAnyColorWindow()
	{
		UIHelper.CoreDataService.DocumentToggleGemSelectionMode(isEnabled: false);
		WindowAnyGemAnyColor.Close();
		WindowAnyGemAnyColor = null;
	}

	public void CloseMainMenuPopup()
	{
		IsMenuToggleButtonChecked = false;
		Messenger.Default.Send<string, ProgramMenuPopup>("ResetMenuViewState");
	}

	public void InitializeUI()
	{
		IsLiveChatVisible = true;
		SetupUISections();
		SetupApplicationCaches();
		SetupUIDefaults();
	}

	public void NavigateToSection(Sections sectionNumber, bool notifyBreadCrumb = true)
	{
		ViewModelLocator.Current.TileSearchMainVM.IsSearchingDesigns = true;
		ViewModelLocator.Current.StudioMaterialSelectionVM.CloseAllMaterialPickers();
		ViewModelLocator.Current.TileSearchMainVM.TileSearchResultDesignsVM.CloseDesignInfo();
		ViewModelLocator.Current.TileSearchMainVM.TileSearchResultPartsVM.CloseDesignInfo();
		ViewModelLocator.Current.TileSearchMainVM.SetSearchViewOption();
		if (sectionNumber == Sections.Search)
		{
			ViewModelLocator.Current.TileSearchMainVM.TileSearchResultDesignsVM.ResetFilters(isDefaultingMaterials: false);
		}
		if (sectionNumber == Sections.Customize && ViewModelLocator.Current.CustomizeVM.HasPartsToolboxBeenShown && UIHelper.CounterSketchOptions.KeepPartsWindowOpen)
		{
			ViewModelLocator.Current.CustomizeVM.IsPartToolBoxVisible = true;
		}
		else
		{
			ViewModelLocator.Current.CustomizeVM.IsPartToolBoxVisible = false;
		}
		using (System.Windows.Application.Current.Dispatcher.DisableProcessing())
		{
			SectionPageNumber = (int)sectionNumber;
			ScrollSection = true;
			ScrollSection = false;
			IsOrderModeActive = sectionNumber == Sections.Order;
			IsSearchModeActive = sectionNumber == Sections.Search;
			IsCustomizeModeActive = sectionNumber == Sections.Customize;
			if (notifyBreadCrumb)
			{
				ViewModelLocator.Current.BreadCrumbVM.NavigateToSection(sectionNumber);
			}
		}
	}

	public void OpenPreferencesWindow(CSPreferenceTabs tabOption)
	{
		CloseMainMenuPopup();
		if (tabOption != CSPreferenceTabs.Troubleshooting && UIHelper.CounterSketchOptions.RequirePasswordForPreferences)
		{
			SecurityHelper.AuthenticationMode = SecurityHelper.LoginMode.NormalMode;
			SecurityHelper.ReasonForPrompt = "Authentication required for Preferences";
			SecurityHelper.PromptForAuthentication();
			if (!SecurityHelper.IsAuthenticated)
			{
				return;
			}
		}
		if (_windowModalOptions == null)
		{
			_windowModalOptions = new WindowModalOptions();
		}
		else
		{
			_windowModalOptions.LoadOptions();
		}
		Window mainWindow = System.Windows.Application.Current.MainWindow;
		if (mainWindow != null)
		{
			_windowModalOptions.Owner = mainWindow;
		}
		_windowModalOptions.ShowTab(tabOption);
		UIHelper.ToggleHardwareRenderingForWindow(_windowModalOptions, UIHelper.CounterSketchOptions.DisableHardwareRendering);
		_windowModalOptions.WindowStartupLocation = WindowStartupLocation.CenterOwner;
		_windowModalOptions.ShowDialog();
		SetQeOptionVisibility(UIHelper.CounterSketchOptions.HideQuickEstimate);
		Messenger.Default.Send(UIHelper.CounterSketchOptions);
	}

	public void OpenReleaseNotesLibraryWindow()
	{
		CloseMainMenuPopup();
		if (UIHelper.IsInternetAccessible())
		{
			string libraryVersion;
			using (CSLibraryDAL cSLibraryDAL = new CSLibraryDAL())
			{
				libraryVersion = cSLibraryDAL.GetLibraryVersion();
			}
			WindowBrowser windowBrowser = new WindowBrowser(new Uri($"http://countersketch.biz/ReleaseNotesLibrary/ReleaseNote_{libraryVersion}.html?{Guid.NewGuid()}"));
			windowBrowser.Width = 1030.0;
			windowBrowser.Height = 800.0;
			windowBrowser.Owner = System.Windows.Application.Current.MainWindow;
			windowBrowser.Show();
			windowBrowser.BringIntoView();
			windowBrowser.BringWindowToTop();
		}
	}

	public void OpenReleaseNotesWindow()
	{
		OpenURLInOurBrowserWindow($"http://countersketch.biz/ReleaseNotes/ReleaseNote_{UIHelper.VersionString}.html?{Guid.NewGuid()}");
	}

	public void OpenURLInOurBrowserWindow(string url)
	{
		CloseMainMenuPopup();
		if (UIHelper.IsInternetAccessible())
		{
			WindowBrowser windowBrowser = new WindowBrowser(new Uri(url))
			{
				Width = 1030.0,
				Height = 800.0,
				Owner = System.Windows.Application.Current.MainWindow
			};
			if (SystemParameters.PrimaryScreenHeight <= 768.0 && SystemParameters.PrimaryScreenWidth <= 1366.0)
			{
				windowBrowser.Width = 1030.0;
				windowBrowser.Height = 700.0;
			}
			windowBrowser.Show();
			windowBrowser.BringIntoView();
			windowBrowser.BringWindowToTop();
		}
	}

	public async void OpenStoneShopperWindow()
	{
		PunchoutHelper.ShowDiamondShopper();
	}

	public void PopulateDebugInfo(string message = null)
	{
		if (UIHelper.CounterSketchOptions.UserTypeId < 8)
		{
			return;
		}
		int num = 0;
		int num2 = 0;
		int num3 = 0;
		int num4 = 0;
		int num5 = 0;
		try
		{
			if (_rhinoVersion == null)
			{
				UIHelper.CoreDataService.RhinoGetVersion(delegate(string version)
				{
					_rhinoVersion = version;
				});
			}
			using (CSLibraryDAL cSLibraryDAL = new CSLibraryDAL())
			{
				_libraryVersion = cSLibraryDAL.GetLibraryVersion();
				_modelCount = cSLibraryDAL.GetModelCount();
			}
			using (CounterSketchLibraryDataEntities1 counterSketchLibraryDataEntities = new CounterSketchLibraryDataEntities1(CSLibraryDAL.NameOfConnectionString))
			{
				num = counterSketchLibraryDataEntities.DesignSearches.Count((DesignSearch _) => _.IsFreeHandPart != (long?)1L && _.IsFreeHandPartWithGem != (long?)1L && _.IsStaticFreeHandPart != (long?)1L);
				num2 = counterSketchLibraryDataEntities.DesignSearches.Count((DesignSearch _) => _.IsFreeHandPart == (long?)1L || _.IsFreeHandPartWithGem == (long?)1L || _.IsStaticFreeHandPart == (long?)1L);
				num4 = counterSketchLibraryDataEntities.DesignSearches.Count((DesignSearch _) => _.IsFreeHandPartWithGem == (long?)1L);
				num5 = counterSketchLibraryDataEntities.DesignSearches.Count((DesignSearch _) => _.IsFreeHandPart == (long?)1L || _.IsFreeHandPartWithGem == (long?)1L);
				num3 = counterSketchLibraryDataEntities.DesignSearches.Count((DesignSearch _) => _.IsStaticFreeHandPart == (long?)1L);
			}
			DebugInfoText = $"KEY: {UIHelper.CounterSketchOptions.GetKeyId()} - CS: {UIHelper.Version} - RHINO: {_rhinoVersion} - Library: {_libraryVersion} - Total Designs + Parts: {_modelCount} - Designs: {num} - Parts: {num2} - Static Parts: {num3} - Counter Parts: {num5} - Parts w/ Gems: {num4}              {message}";
		}
		catch (Exception)
		{
		}
	}

	public void Redo()
	{
		UIHelper.CoreDataService.GraphRedoChange(ViewModelLocator.Current.DesignStackVM.UpdateDesignStackUIControls);
	}

	public void ResetApplication()
	{
		System.Windows.Application.Current.Dispatcher.BeginInvoke((Action)delegate
		{
			UIHelper.CoreDataService.SetCAMModeState(CAMModeOn: false);
			UIHelper.CoreDataService.DocumentSetModified(modified: false);
			UIHelper.CoreDataService.DocumentReset();
			UIHelper.LoadedDesign = new GV.CounterSketch.Studio.Model.Objects.Design();
			ViewModelLocator.Current.DesignStackVM.Reset();
			ViewModelLocator.Current.FreehandStackVM.Reset();
			ViewModelLocator.Current.CustomizePartsListVM.Reset();
			ViewModelLocator.Current.TileSearchMainVM.Reset();
			ViewModelLocator.Current.BreadCrumbVM.Reset();
			if (UIHelper.KeyProfile.IsDesignLibraryDisabled)
			{
				ViewModelLocator.Current.BreadCrumbVM.LabelEditing = "Customize";
				ViewModelLocator.Current.CustomizePartsListVM.AddPart();
			}
			else
			{
				NavigateToSection(Sections.Tiles);
			}
			ProjectManagementHelper.ResetProjectManagement();
			ViewModelLocator.Current.StudioMaterialSelectionVM.DefaultMaterialGroupOptions();
			UIHelper.RaiseMaterialsUpdatedEvent();
			ViewModelLocator.Current.DesignStackVM.UpdateDesignStackDisplayComponents(isResetingComponents: true);
			UIHelper.IsOrderedDesignLoaded = false;
			ViewModelLocator.Current.OrderSummaryVM.Reset(skipVolume: true, skipGemShop: true);
			ViewModelLocator.Current.WindowRenderOptionsVM.Reset();
			UIHelper.CoreDataService.TurnHandOff();
			UIHelper.CoreDataService.UpdateRhinoAppearanceMaterials(string.Empty, string.Empty, isRenderLayDesignFlat: false);
			ViewModelLocator.Current.WindowRenderOptionsVM.IsRenderDesignLayFlat = false;
			UIHelper.ShowWaitWindow(waitVisible: false);
			StullerProductAPIHelper.Reset();
			ViewModelLocator.Current.StoneTrayVM.Reset();
			ViewModelLocator.Current.GemReportVM.ResetPreviousQualityAssignments();
			ViewModelLocator.Current.TileSearchMainVM.TileSearchResultDesignsVM.UpdateSearchFiltersUsingAssignedStone(overrideSearchActiveRequirements: true);
			ViewModelLocator.Current.CustomizeVM.Reset();
			PunchoutHelper.Reset();
			UIHelper.IsResettingApplicationState = false;
			PopulateDebugInfo();
		});
	}

	public void ResetOrderInformation()
	{
		CloseMainMenuPopup();
		ViewModelLocator.Current.OrderSummaryVM.Populate();
		UIHelper.IsGemReportActive = true;
		ViewModelLocator.Current.GemReportVM.Populate();
	}

	public void SaveCurrentDesign()
	{
		IsSaving = true;
		Task.Run(delegate
		{
			ProjectManagementHelper.AddDesignOnCanvasToHistoryOfActiveProject(UIHelper.LoadedDesign);
		}).ContinueWith((Task t) => IsSaving = false);
	}

	public void SetCustomizeModeActive()
	{
		IsCustomizeModeActive = true;
		IsFreehandModeActive = false;
		IsSearchModeActive = false;
	}

	public void SetFreehandModeActive()
	{
		ViewModelLocator.Current.CustomizePartsListVM.AddPart();
	}

	public void SetFreehandModeActive(bool skipTabChange = false)
	{
		IsFreehandModeActive = true;
		IsCustomizeModeActive = false;
		IsSearchModeActive = false;
	}

	public void SetIsRhinoBusy(bool isBusy)
	{
		IsRhinoBusy = isBusy;
		if (isBusy)
		{
			_isRhinoBusyDispatcherTimer.Start();
		}
		else
		{
			_isRhinoBusyDispatcherTimer.Stop();
			RaisePropertyChanged(() => IsRhinoBusy);
		}
		ViewModelLocator.Current.BreadCrumbVM.RefreshOrderVisibility();
	}

	public void SetOrderModeActive()
	{
		if (UIHelper.IsInternetAccessible())
		{
			IsOrderModeActive = true;
			IsCustomizeModeActive = true;
			IsFreehandModeActive = false;
			IsSearchModeActive = false;
		}
	}

	public void SetQeOptionVisibility(bool? hideQuickEstimate)
	{
		if (hideQuickEstimate == true)
		{
			QuickEstimateVisibility = Visibility.Collapsed;
		}
		else
		{
			QuickEstimateVisibility = Visibility.Visible;
		}
	}

	public void SetSearchModeActive()
	{
		IsSearchModeActive = true;
		IsCustomizeModeActive = false;
		IsFreehandModeActive = false;
		ViewModelLocator.Current.TileSearchMainVM.TileSearchResultDesignsVM.UpdateSearchFiltersUsingAssignedStone();
	}

	public void SetSectionTransitionTimeToInstant()
	{
		SectionTransitionTime = KeyTime.FromTimeSpan(TimeSpan.FromSeconds(0.0));
	}

	public void SetSectionTransitionTimeToNormal()
	{
		SectionTransitionTime = KeyTime.FromTimeSpan(TimeSpan.FromSeconds(SelectedKeyTime));
	}

	public void Undo()
	{
		UIHelper.CoreDataService.GraphUndoChange(ViewModelLocator.Current.DesignStackVM.UpdateDesignStackUIControls);
	}

	private void CenterWindowOnScreen()
	{
		double primaryScreenWidth = SystemParameters.PrimaryScreenWidth;
		double primaryScreenHeight = SystemParameters.PrimaryScreenHeight;
		double width = System.Windows.Application.Current.MainWindow.Width;
		double height = System.Windows.Application.Current.MainWindow.Height;
		System.Windows.Application.Current.MainWindow.Left = primaryScreenWidth / 2.0 - width / 2.0;
		System.Windows.Application.Current.MainWindow.Top = primaryScreenHeight / 2.0 - height / 2.0;
	}

	private void CheckProjetAndUpsell()
	{
		if (ProjectManagementHelper.IsDocumentCanvasEmpty())
		{
			System.Windows.MessageBox.Show(Strings.Design_Canvas_No_Visible_Items_To_Print);
			return;
		}
		if (!UIHelper.CounterSketchOptions.EnableProjetPrinterSupport)
		{
			System.Windows.MessageBox.Show(Strings.Projet_MakeSureThatProjectIsOn);
		}
		string printerIpAddress = UIHelper.CounterSketchOptions.ProjetLastIPLocation;
		UIHelper.ShowWaitWindow(waitVisible: true, Strings.Projet_Searching);
		bool foundPrinter = false;
		System.Windows.Application.Current.Dispatcher.Invoke(delegate
		{
			foundPrinter = UIHelper.CoreDataService.FindProjetPrinter(out printerIpAddress);
		});
		UIHelper.ShowWaitWindow(waitVisible: false);
		if (string.IsNullOrWhiteSpace(printerIpAddress))
		{
			UIHelper.RaiseApplicationMessageEvent(null, ApplicationMessageType.SingleLineAlert, Strings.Projet_MakeSureThatProjectIsOn, messageIsModal: true);
		}
		else
		{
			IsProjetPrinterSupportEnabled = true;
			UIHelper.CounterSketchOptions.ProjetLastIPLocation = printerIpAddress;
			NavigateToSection(Sections.Customize);
			UIHelper.CoreDataService.DocumentSetCameraView("perspective");
			OpenPrintToProject1200();
		}
		UIHelper.CounterSketchOptions.WriteSettings();
	}

	private void CloseCounterSketch()
	{
		CloseMainMenuPopup();
		System.Windows.Application.Current.MainWindow.Close();
	}

	private void CopyFrameworkElementToClipboard(FrameworkElement frameworkElement)
	{
		System.Windows.Clipboard.SetImage(ImageHelper.CreateBitmapFromFrameworkElement(frameworkElement));
	}

	private void CreateQuickPortfolio()
	{
		while (true)
		{
			WindowMessage windowMessage = new WindowMessage(WindowMessage.WindowType.QuestionAnswer)
			{
				Title = Strings.CreateNewPortfolioPrompt,
				Question = Strings.NewPortfolioName,
				Topmost = true,
				Owner = System.Windows.Application.Current.MainWindow
			};
			windowMessage.ShowDialog();
			if (windowMessage.IsAccepted == true)
			{
				string text = ProjectManagementHelper.VerifyProjectName(windowMessage.Answer);
				if (string.IsNullOrEmpty(text))
				{
					ProjectManagementHelper.CreateNewProject(windowMessage.Answer, new CustomerInfo(), ProjectType.Portfolio);
					break;
				}
				UIHelper.RaiseApplicationMessageEvent(null, ApplicationMessageType.SingleLineAlert, Strings.PortfolioName + " - " + text, messageIsModal: true);
				continue;
			}
			break;
		}
	}

	private void DesignTryOn()
	{
		if (_windowDesignTryOnView == null)
		{
			_windowDesignTryOnView = new WindowDesignTryOnView
			{
				Owner = System.Windows.Application.Current.MainWindow
			};
		}
		_windowDesignTryOnView.Show();
		_windowDesignTryOnView.BringWindowToTop();
		ViewModelLocator.Current.WindowDesignTryOnVM.TurnHandOn();
	}

	private void DisableRhinoViewportGridlines()
	{
		UIHelper.CoreDataService.ToggleRhinoViewportAxis(enableAxis: false);
		UIHelper.CoreDataService.ToggleRhinoViewportGridlines(enableGridlines: false);
	}

	private void DisableStackHighlight()
	{
		UIHelper.CounterSketchOptions.DisableViewportHighlight = true;
		UIHelper.CounterSketchOptions.WriteSettings();
	}

	private void EditTileLayoutDesigns()
	{
		NavigateToSection(Sections.Tiles);
		ViewModelLocator.Current.TileViewDesignsVM.EditLayout();
	}

	private void EditTileLayoutParts()
	{
		ViewModelLocator.Current.TileSearchMainVM.TileSearchResultPartsVM.NavigateBackToTiles();
		NavigateToSection(Sections.Customize);
		ViewModelLocator.Current.CustomizeVM.IsPartToolBoxVisible = true;
		ViewModelLocator.Current.TileViewPartsVM.EditLayout();
	}

	private void EnableRhinoViewportGridlines()
	{
		EventHelper.RecordEvent(EventHelper.EventTypes.Customize_Grid);
		EventHelper.RecordEvent(EventHelper.EventTypes.Customize_Axis);
		UIHelper.CoreDataService.ToggleRhinoViewportGridlines(enableGridlines: true);
		UIHelper.CoreDataService.ToggleRhinoViewportAxis(enableAxis: true);
	}

	private void EnableStackHighlight()
	{
		UIHelper.CounterSketchOptions.DisableViewportHighlight = false;
		UIHelper.CounterSketchOptions.WriteSettings();
	}

	private void ExportDesign()
	{
		CloseMainMenuPopup();
		SaveFileDialog saveFileDialog = new SaveFileDialog
		{
			Filter = "CounterSketch Design File|*.csg",
			Title = "Save a CounterSketch Design File"
		};
		saveFileDialog.ShowDialog();
		if (saveFileDialog.FileName == "")
		{
			return;
		}
		try
		{
			EventHelper.RecordEvent(EventHelper.EventTypes.Design_Exported, $"IsNew={DesignHelper.IsNewDesign(UIHelper.LoadedDesign.DesignId)}", UIHelper.LoadedDesign.DesignId.ToString(CultureInfo.InvariantCulture));
			UIHelper.CoreDataService.DocumentWrite(saveFileDialog.FileName);
		}
		catch (UnauthorizedAccessException)
		{
		}
	}

	private void ExportSearchImages()
	{
		ViewModelLocator.Current.WindowExportSearchResultsVM.Populate(ViewModelLocator.Current.TileSearchMainVM.TileSearchResultDesignsVM.CurrentPageDesigns);
		WindowExportSearchResultsView windowExportSearchResultsView = new WindowExportSearchResultsView();
		windowExportSearchResultsView.Owner = System.Windows.Application.Current.MainWindow;
		windowExportSearchResultsView.ShowDialog();
	}

	private void GenerateRender()
	{
		ViewModelLocator.Current.WindowRenderOptionsVM.Render();
	}

	private void ImportDesign()
	{
		CloseMainMenuPopup();
		EventHelper.RecordEvent(EventHelper.EventTypes.Design_Imported, $"IsNew={DesignHelper.IsNewDesign(UIHelper.LoadedDesign.DesignId)}", UIHelper.LoadedDesign.DesignId.ToString(CultureInfo.InvariantCulture));
		OpenFileDialog openFileDialog = new OpenFileDialog
		{
			Filter = "CounterSketch Graph (*.csg)|*.csg|All files (*.*)|*.*"
		};
		if (openFileDialog.ShowDialog() == DialogResult.OK)
		{
			LoadHelper.LoadDesign(openFileDialog.FileName);
		}
	}

	private void InitializeCommands()
	{
		CommandUnregisterComputer = new RelayCommand(UnregisterComputer);
		CommandQuickBackup = new RelayCommand(QuickBackup);
		CommandImportDesign = new RelayCommand(ImportDesign);
		CommandExportDesign = new RelayCommand(ExportDesign);
		CommandSetSearchModeActive = new RelayCommand(SetSearchModeActive);
		CommandSetCustomizeModeActive = new RelayCommand(SetCustomizeModeActive);
		CommandSendQuickDebugReport = new RelayCommand(SendQuickDebugReport);
		CommandOpenUserGuide = new RelayCommand(OpenUserGuide);
		CommandOpenUserGuideAddendum = new RelayCommand(OpenUserGuideAddendum);
		CommandOpenQuoteHistory = new RelayCommand(OpenQuoteHistory);
		CommandResetOrderInformation = new RelayCommand(ResetOrderInformation);
		CommandSendEmail = new RelayCommand<object>(SendEmail);
		CommandOpenAboutWindow = new RelayCommand(OpenAboutWindow);
		CommandOpenSearchKeywordWindow = new RelayCommand(OpenSearchKeyword);
		CommandOpenReleaseNotesWindow = new RelayCommand(OpenReleaseNotesWindow);
		CommandOpenReleaseNotesLibraryWindow = new RelayCommand(OpenReleaseNotesLibraryWindow);
		CommandViewCounterSketchLog = new RelayCommand(ViewCounterSketchLog);
		CommandOpenTeamViewer = new RelayCommand(OpenTeamViewer);
		CommandUseAlternateCountryCode = new RelayCommand<string>(UseAlternateCountryCode);
		CommandEnableRhinoViewportGridlines = new RelayCommand(EnableRhinoViewportGridlines);
		CommandDisableRhinoViewportGridlines = new RelayCommand(DisableRhinoViewportGridlines);
		CommandSetRhinoCameraViewTop = new RelayCommand(SetRhinoCameraViewTop);
		CommandSetRhinoCameraViewFront = new RelayCommand(SetRhinoCameraViewFront);
		CommandSetRhinoCameraViewSide = new RelayCommand(SetRhinoCameraViewSide);
		CommandSetRhinoCameraViewPerspective = new RelayCommand(SetRhinoCameraViewPerspective);
		CommandSpinDesign = new RelayCommand(SpinDesign);
		CommandSpinDesignStop = new RelayCommand(SpinDesignStop);
		CommandViewGemInfo = new RelayCommand(ViewGemInfo);
		CommandUndo = new RelayCommand(Undo);
		CommandRedo = new RelayCommand(Redo);
		CommandOpenDesignTryOnWindow = new RelayCommand(OpenDesignTryOnWindow);
		CommandDesignTryOn = new RelayCommand(DesignTryOn);
		CommandSetFreehandModeActive = new RelayCommand(SetFreehandModeActive);
		CommandOpenPreferencesWindow = new RelayCommand<CSPreferenceTabs>(OpenPreferencesWindow);
		CommandNewSessionClick = new RelayCommand(NewSessionClick);
		CommandOpenAccountInfoWindow = new RelayCommand(OpenAccountInfoWindow);
		CommandOpenPortfolioManager = new RelayCommand(OpenPortfolioManager);
		CommandOpenResourceImageView = new RelayCommand(OpenResourceImageView);
		CommandCopyFrameworkElementToClipboard = new RelayCommand<FrameworkElement>(CopyFrameworkElementToClipboard);
		CommandDisableStackHighlight = new RelayCommand(DisableStackHighlight);
		CommandEnableStackHighlight = new RelayCommand(EnableStackHighlight);
		CommandOpenStyleQuizWindow = new RelayCommand(OpenStyleQuizWindow);
		CommandOpenRenderDialogue = new RelayCommand(OpenRenderDialogue);
		CommandGenerateRender = new RelayCommand(GenerateRender);
		CommandSetOrderModeActive = new RelayCommand(SetOrderModeActive);
		CommandOpenPrintToEnvisionTec = new RelayCommand(OpenPrintToEnvisionTec);
		CommandOpenPrintToProJet1200 = new RelayCommand(OpenPrintToProject1200);
		CommandOpenCamStandards = new RelayCommand(OpenCamStandards);
		CommandCheckProjetAndUpsell = new RelayCommand(CheckProjetAndUpsell);
		CommandPerformDistanceMeasurement = new RelayCommand(PerformDistanceMeasurement);
		CommandOpenProjetInstructions = new RelayCommand(OpenProjetInstructions);
		CommandInstallProjetUsbDrivers = new RelayCommand(InstallProjetUsbDrivers);
		CommandSetCamModeState = new RelayCommand(SetCamModeState);
		CommandWindowSize1080 = new RelayCommand(WindowSize1080);
		CommandWindowSize1050 = new RelayCommand(WindowSize1050);
		CommandToggleWindowMaximized = new RelayCommand(ToggleWindowMaximized);
		CommandOpenCamMountingForm = new RelayCommand(OpenCamMountingForm);
		CommandOpenCamStoneForm = new RelayCommand(OpenCamStoneForm);
		CommandMoveBackward = new RelayCommand(MoveBackward);
		CommandMoveForward = new RelayCommand(MoveForward);
		CommandSaveCurrentDesign = new RelayCommand(SaveCurrentDesign);
		CommandEditTileLayoutDesigns = new RelayCommand(EditTileLayoutDesigns);
		CommandEditTileLayoutParts = new RelayCommand(EditTileLayoutParts);
		CommandExportSearchImages = new RelayCommand(ExportSearchImages);
		CommandOpenQuickEstimateWindow = new RelayCommand(OpenQuickEstimateWindow);
		CommandOpenAnyGemAnyColorMaterialSelection = new RelayCommand(OpenAnyGemAnyColorMaterialSelection);
		CommandToggleFreehandCommonCenterMode = new RelayCommand<string>(ToggleFreehandCommonCenterMode);
		CommandOpenTroubleshootingWindow = new RelayCommand(OpenTroubleshootingWindow);
		CommandCloseCounterSketch = new RelayCommand(CloseCounterSketch);
		CommandCreateQuickPortfolio = new RelayCommand(CreateQuickPortfolio);
	}

	private void InstallProjetUsbDrivers()
	{
		if (System.Windows.MessageBox.Show(Strings.Project_Enable_USB_Support_Prompt, Strings.Projet_1200_USB_Support, MessageBoxButton.YesNo) != MessageBoxResult.Yes)
		{
			return;
		}
		try
		{
			string text = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ProgramFilesX86), "CounterSketch Studio\\ProjetUSBDrivers\\BONE_64_Bit_Drivers.exe");
			if (File.Exists(text))
			{
				ProcessStartInfo startInfo = new ProcessStartInfo(text, " /A /SW /SE");
				Process process = new Process();
				process.StartInfo = startInfo;
				process.Start();
				process.WaitForExit();
			}
		}
		catch (Exception ex)
		{
			GlobalErrorHandler.WriteErrorToEventLog(Strings.Projet_ErrorInstallingUSBDrivers + ex);
		}
	}

	private void MoveBackward()
	{
		if (SectionPageNumber != 0)
		{
			NavigateToSection((Sections)(SectionPageNumber - 1));
		}
	}

	private void MoveForward()
	{
		if (SectionPageNumber != 2)
		{
			NavigateToSection((Sections)(SectionPageNumber + 1));
		}
	}

	private void NewSessionClick()
	{
		CloseMainMenuPopup();
		if (!ProjectManagementHelper.PromptForPortfolioSave())
		{
			return;
		}
		if (ProjectManagementHelper.IsTempProjectDirty())
		{
			ProjectManagementHelper.DeleteProject(ProjectManagementHelper.GlobalProjectCollection.FirstOrDefault((Project _) => _.Name == ProjectManagementHelper.TempProjectName));
		}
		ResetApplication();
	}

	private void OpenAboutWindow()
	{
		CloseMainMenuPopup();
		WindowAboutView obj = new WindowAboutView
		{
			Owner = System.Windows.Application.Current.MainWindow
		};
		UIHelper.ToggleHardwareRenderingForWindow(obj, UIHelper.CounterSketchOptions.DisableHardwareRendering);
		obj.ShowDialog();
	}

	private void OpenAccountInfoWindow()
	{
		CloseMainMenuPopup();
		UIHelper.OpenAccountWindow(promptAuthentication: true);
	}

	private void OpenAnyGemAnyColorMaterialSelection()
	{
		if (WindowAnyGemAnyColor == null)
		{
			UIHelper.CoreDataService.DocumentToggleGemSelectionMode(isEnabled: true);
			WindowAnyGemAnyColor = new WindowAnyGemAnyColorView(new MaterialOptionSelectorViewModel(Strings.AnyGemAnyColor, MaterialAssignmentMode.GemAnyColor, 105))
			{
				Owner = System.Windows.Application.Current.MainWindow
			};
			WindowAnyGemAnyColor.Show();
			WindowAnyGemAnyColor.Top = RhinoWindowHelper.CurrentRhinoPosition.Top + 25;
			WindowAnyGemAnyColor.Left = RhinoWindowHelper.CurrentRhinoPosition.Left + 25;
		}
	}

	private void OpenCamMountingForm()
	{
		CloseMainMenuPopup();
		if (!UIHelper.IsInternetAccessible())
		{
			return;
		}
		try
		{
			ViewModelLocator.Current.MainVM.OpenURLInOurBrowserWindow("http://www.stuller.com/static/i/cadcamservices/CadCamMountingSubmissionForm.docx");
		}
		catch (Exception ex)
		{
			UIHelper.RaiseApplicationMessageEvent(this, ApplicationMessageType.Exception, ex.Message, messageIsModal: false);
		}
	}

	private void OpenCamStandards()
	{
		CloseMainMenuPopup();
		if (!UIHelper.IsInternetAccessible())
		{
			return;
		}
		try
		{
			ViewModelLocator.Current.MainVM.OpenURLInOurBrowserWindow("http://www.stuller.com/apps/images/contentBlock/CAM/New_Production_Standards.pdf");
		}
		catch (Exception ex)
		{
			UIHelper.RaiseApplicationMessageEvent(this, ApplicationMessageType.Exception, ex.Message, messageIsModal: false);
		}
	}

	private void OpenCamStoneForm()
	{
		CloseMainMenuPopup();
		if (!UIHelper.IsInternetAccessible())
		{
			return;
		}
		try
		{
			ViewModelLocator.Current.MainVM.OpenURLInOurBrowserWindow("http://www.stuller.com/static/i/cadcamservices/CadCamStoneSubmissionForm.docx");
		}
		catch (Exception ex)
		{
			UIHelper.RaiseApplicationMessageEvent(this, ApplicationMessageType.Exception, ex.Message, messageIsModal: false);
		}
	}

	private void OpenDesignTryOnWindow()
	{
		CloseMainMenuPopup();
		WindowDesignTryOnView windowDesignTryOnView = new WindowDesignTryOnView();
		windowDesignTryOnView.Owner = System.Windows.Application.Current.MainWindow;
		windowDesignTryOnView.Show();
	}

	private void OpenPortfolioManager()
	{
		CloseMainMenuPopup();
		PortfolioManagerView portfolioManagerView = UIHelper.PortfolioManagerView;
		portfolioManagerView.Show();
		portfolioManagerView.WindowState = WindowState.Normal;
		portfolioManagerView.BringIntoView();
		portfolioManagerView.BringWindowToTop();
	}

	private void OpenPrintToEnvisionTec()
	{
		System.Windows.MessageBox.Show("Thank you, user!  But our princess is in another castle", "Print to EnvisionTEC", MessageBoxButton.OK, MessageBoxImage.None);
	}

	private void OpenPrintToProject1200()
	{
		CloseMainMenuPopup();
		if (!UIHelper.CoreDataService.IsProjetDialogLoaded() && !ProjectManagementHelper.IsDocumentCanvasEmpty())
		{
			_frmPrintToProJet1200 = new FrmPrintToProJet1200(UIHelper.LoadedDesign.DesignId.ToString());
			_frmPrintToProJet1200.Show();
		}
	}

	private void OpenProjetInstructions()
	{
		CloseMainMenuPopup();
		if (!UIHelper.IsInternetAccessible())
		{
			return;
		}
		try
		{
			ViewModelLocator.Current.MainVM.OpenURLInOurBrowserWindow("http://countersketch.biz/UserGuides/Projet1200/Instructions.html");
		}
		catch (Exception ex)
		{
			UIHelper.RaiseApplicationMessageEvent(this, ApplicationMessageType.Exception, ex.Message, messageIsModal: false);
		}
	}

	private static void OpenQuickEstimateWindow()
	{
		if (UIHelper.IsInternetAccessible())
		{
			if (ProjectManagementHelper.IsDocumentCanvasEmpty())
			{
				UIHelper.RaiseApplicationMessageEvent("Empty Design", ApplicationMessageType.MultiLineInfo, "The design canvas is empty, please Open or Build a design and try again!", messageIsModal: true);
				return;
			}
			ViewModelLocator.Current.GemReportVM.Populate();
			ViewModelLocator.Current.DesignStackVM.UpdateDesignStackDisplayComponents();
			ViewModelLocator.Current.OrderSummaryVM.SendQuickEstimateShowcaseMarkups();
		}
	}

	private void OpenQuoteHistory()
	{
		CloseMainMenuPopup();
		if (UIHelper.IsInternetAccessible())
		{
			ViewModelLocator.Current.WindowOrderHistoryVM.PopulateOrderHistory();
			WindowOrderHistoryView obj = new WindowOrderHistoryView
			{
				Owner = System.Windows.Application.Current.MainWindow
			};
			UIHelper.ToggleHardwareRenderingForWindow(obj, UIHelper.CounterSketchOptions.DisableHardwareRendering);
			obj.Show();
			obj.BringWindowToTop();
		}
	}

	private void OpenRenderDialogue()
	{
		if (ViewModelLocator.Current.DesignStackVM.SelectedDesignComponentDisplay != null)
		{
			bool designIsPendant = ViewModelLocator.Current.WindowRenderOptionsVM.DesignIsPendant;
			ViewModelLocator.Current.WindowRenderOptionsVM.DesignIsPendant = ViewModelLocator.Current.DesignStackVM.SelectedDesignComponentDisplay.Name == "Pendant";
			if (designIsPendant != ViewModelLocator.Current.WindowRenderOptionsVM.DesignIsPendant)
			{
				ViewModelLocator.Current.WindowRenderOptionsVM.PopulateRenderOptions();
			}
		}
		EventHelper.RecordEvent(EventHelper.EventTypes.Render_Options);
		if (_theWindow == null)
		{
			_theWindow = new WindowRenderOptionsView
			{
				Owner = System.Windows.Application.Current.MainWindow
			};
		}
		_theWindow.Show();
		_theWindow.BringIntoView();
		_theWindow.BringWindowToTop();
	}

	private void OpenResourceImageView()
	{
		CloseMainMenuPopup();
		WindowResourceImageView windowResourceImageView = new WindowResourceImageView();
		windowResourceImageView.Owner = System.Windows.Application.Current.MainWindow;
		windowResourceImageView.Show();
	}

	private void OpenSearchKeyword()
	{
		CloseMainMenuPopup();
		WindowSearchKeyword windowSearchKeyword = new WindowSearchKeyword();
		windowSearchKeyword.Owner = System.Windows.Application.Current.MainWindow;
		windowSearchKeyword.Show();
		windowSearchKeyword.BringIntoView();
		windowSearchKeyword.BringWindowToTop();
	}

	private void OpenTeamViewer()
	{
		CloseMainMenuPopup();
		try
		{
			Process.Start(Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), "support.exe"));
		}
		catch
		{
		}
	}

	private void OpenTroubleshootingWindow()
	{
		string text = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "TroubleShooter\\TroubleShooter.exe");
		if (File.Exists(text))
		{
			Process.Start(text);
		}
	}

	private void OpenUserGuide()
	{
		CloseMainMenuPopup();
		if (!UIHelper.IsInternetAccessible())
		{
			return;
		}
		try
		{
			if (UIHelper.KeyProfile.KeyProfileId == 3)
			{
				Process.Start("http://countersketch.biz/Userguides/UserGuideInternational.pdf");
			}
			else
			{
				Process.Start("http://countersketch.biz/Userguides/UserGuide60.pdf");
			}
		}
		catch (Exception ex)
		{
			UIHelper.RaiseApplicationMessageEvent(this, ApplicationMessageType.Exception, ex.Message, messageIsModal: false);
		}
	}

	private void OpenUserGuideAddendum()
	{
		CloseMainMenuPopup();
		if (!UIHelper.IsInternetAccessible())
		{
			return;
		}
		try
		{
			Process.Start("http://countersketch.biz/Userguides/UserGuideAddendum2.5.pdf");
		}
		catch (Exception ex)
		{
			UIHelper.RaiseApplicationMessageEvent(this, ApplicationMessageType.Exception, ex.Message, messageIsModal: false);
		}
	}

	private void PerformDistanceMeasurement()
	{
		UIHelper.CoreDataService.PerformDistanceMeasurement();
	}

	private void QuickBackup()
	{
		CloseMainMenuPopup();
		if (UIHelper.CounterSketchOptions.RequirePasswordForPreferences)
		{
			SecurityHelper.PromptForAuthentication();
			if (SecurityHelper.IsAuthenticated)
			{
				EventHelper.RecordEvent(EventHelper.EventTypes.Backup_Created);
				BackupHelper.CreateBackup();
			}
		}
		else
		{
			EventHelper.RecordEvent(EventHelper.EventTypes.Backup_Created);
			BackupHelper.CreateBackup();
		}
	}

	private void SendEmail(object obj)
	{
		string arg = obj as string;
		try
		{
			Process.Start(new ProcessStartInfo($"mailto://{arg}"));
		}
		catch
		{
			GlobalErrorHandler.WriteWarningToEventLog("User attempted to send an email, but no program handled the message.");
		}
	}

	private void SendQuickDebugReport()
	{
		CloseMainMenuPopup();
		if (UIHelper.IsInternetAccessible())
		{
			DebuggingHelper.SendQuickDebugReport();
		}
	}

	private void SetCamModeState()
	{
		_camModeEnabled = !_camModeEnabled;
		string empty = string.Empty;
		empty = ((!_camModeEnabled) ? Strings.CAMMode_TurningOffManufactureSettings : Strings.CAMMode_PreparingDesignforManufacture);
		UIHelper.ShowWaitWindow(waitVisible: true, empty);
		UIHelper.CoreDataService.SetCAMModeState(_camModeEnabled);
		UIHelper.ShowWaitWindow(waitVisible: false);
	}

	private void SetRhinoCameraViewFront()
	{
		EventHelper.RecordEvent(EventHelper.EventTypes.Customize_ViewThrough);
		UIHelper.CoreDataService.DocumentSetCameraView("Front");
	}

	private void SetRhinoCameraViewPerspective()
	{
		EventHelper.RecordEvent(EventHelper.EventTypes.Customize_ViewPerspective);
		UIHelper.CoreDataService.DocumentSetCameraView("Perspective");
	}

	private void SetRhinoCameraViewSide()
	{
		EventHelper.RecordEvent(EventHelper.EventTypes.Customize_ViewSide);
		UIHelper.CoreDataService.DocumentSetCameraView("Side");
	}

	private void SetRhinoCameraViewTop()
	{
		EventHelper.RecordEvent(EventHelper.EventTypes.Customize_ViewTop);
		UIHelper.CoreDataService.DocumentSetCameraView("Top");
	}

	private void SetupApplicationCaches()
	{
		UIHelper.CounterSketchOptions.AllowMoreThanOneInstanceOfCounterSketch = false;
		UIHelper.CounterSketchOptions.WriteSettings();
		UIHelper.AllowApplicationClose = true;
		GemHelper.UpdateMaterialImageCache();
		MetalHelper.UpdateMaterialImageCache();
	}

	private void SetupUIDefaults()
	{
		if (!string.IsNullOrEmpty(UIHelper.CounterSketchOptions.BackupDirectory))
		{
			BackupHelper.CleanBackupFiles(Path.Combine(UIHelper.CounterSketchOptions.BackupDirectory, "CSBackup"));
		}
		string iconPath = UIHelper.KeyProfile.IconPath;
		if (!base.IsInDesignMode)
		{
			Uri uriSource = new Uri("pack://application:,,,/CounterSketch Studio;component/Images/" + iconPath);
			Icon = new BitmapImage(uriSource);
		}
		Title = UIHelper.KeyProfile.ApplicationName;
		if (UIHelper.KeyProfile.KeyProfileId == 3)
		{
			IsLiveChatVisible = false;
		}
		if (UIHelper.KeyProfile.KeyProfileId == 2)
		{
			IsLiveChatVisible = false;
			ThemeHelper.SaveAndApplyUpdatedCustomTheme(UIHelper.KeyProfile.WindowFrameBackground, UIHelper.KeyProfile.BaseBackground, UIHelper.KeyProfile.ToolHeaderBackground, UIHelper.KeyProfile.TextBoxAndFrameBackground, UIHelper.KeyProfile.HighlightAndAccent);
			return;
		}
		Messenger.Default.Send<string, WindowCounterSketchSplash>(Strings.Setting_Theme);
		GlobalErrorHandler.WriteInfoToEventLog("Building the UI - Setting Theme.", 2);
		if (!base.IsInDesignMode)
		{
			ThemeHelper.SetCounterSketchDefaultTheme();
		}
	}

	private void SetupUISections()
	{
		SectionPageNumber = 0;
		KeyTimes = new List<double>
		{
			0.0, 0.1, 0.2, 0.3, 0.4, 0.5, 0.75, 1.0, 1.25, 1.5,
			1.75, 2.0
		};
		SelectedKeyTime = UIHelper.CounterSketchOptions.AnimationTransitionTime;
		List<IEasingFunction> sectionEasingFunctions = new List<IEasingFunction>
		{
			new CircleEase(),
			new BounceEase(),
			new CubicEase(),
			new ElasticEase(),
			new ExponentialEase(),
			new PowerEase(),
			new QuadraticEase(),
			new QuarticEase(),
			new QuinticEase(),
			new SineEase()
		};
		SectionEasingFunctions = sectionEasingFunctions;
		SelectedEasingFunction = SectionEasingFunctions.Skip(5).First();
		List<EasingMode> sectionEasingModes = new List<EasingMode>
		{
			EasingMode.EaseIn,
			EasingMode.EaseInOut,
			EasingMode.EaseOut
		};
		SectionEasingModes = sectionEasingModes;
		SelectedEasingMode = SectionEasingModes.Skip(1).First();
		ViewModelLocator.Current.CustomizeVM.IsPartsListSectionVisible = !UIHelper.KeyProfile.ForceHideFreehand && !UIHelper.KeyProfile.HideFreehandMode;
	}

	private void SpinDesign()
	{
		EventHelper.RecordEvent(EventHelper.EventTypes.Customize_DocumentSpin);
		CloseMainMenuPopup();
		UIHelper.CoreDataService.DocumentSpinDesign(isEnabled: true);
		UIHelper.IsDesignSpinning = true;
	}

	private void SpinDesignStop()
	{
		CloseMainMenuPopup();
		UIHelper.CoreDataService.DocumentSpinDesign(isEnabled: false);
		UIHelper.IsDesignSpinning = false;
	}

	private void ToggleFreehandCommonCenterMode(string isToggled)
	{
		IsCommonCenterPointToggled = Convert.ToBoolean(isToggled);
	}

	private void ToggleWindowMaximized()
	{
		Window mainWindow = System.Windows.Application.Current.MainWindow;
		if (mainWindow.WindowState == WindowState.Maximized)
		{
			mainWindow.WindowState = WindowState.Normal;
		}
		else
		{
			mainWindow.WindowState = WindowState.Maximized;
		}
	}

	private void TriggerSectionAdjustment()
	{
		KeyTime sectionTransitionTime = SectionTransitionTime;
		SetSectionTransitionTimeToInstant();
		ScrollSection = true;
		SectionTransitionTime = sectionTransitionTime;
		ScrollSection = false;
	}

	private void UnregisterComputer()
	{
		GlobalErrorHandler.WriteMethodInfo("UnregisterComputer");
		CloseMainMenuPopup();
		SecurityHelper.AuthenticationMode = SecurityHelper.LoginMode.NormalMode;
		SecurityHelper.PromptForAuthentication();
		if (!SecurityHelper.IsAuthenticated)
		{
			return;
		}
		UIHelper.RaiseApplicationMessageEvent(this, ApplicationMessageType.AcceptPrompt, Strings.DoYouWantToUnregister, messageIsModal: true);
		if (UIHelper.LastMessageBoxAccepted == true)
		{
			EventHelper.RecordEvent(EventHelper.EventTypes.Application_UnregisteredComputer);
			using (SecurityServiceClient securityServiceClient = new SecurityServiceClient("BasicHttpBinding_ISecurityService"))
			{
				securityServiceClient.ResetKey(UIHelper.CounterSketchOptions.GetKeyId());
			}
			UIHelper.CounterSketchOptions.IsUnregistered = true;
			UIHelper.CounterSketchOptions.WriteSettings();
			UIHelper.RaiseApplicationMessageEvent(this, ApplicationMessageType.SingleLineInfo, Strings.ThisComputerIsUnregistered, messageIsModal: true);
			System.Windows.Application.Current.MainWindow?.Close();
		}
	}

	[DebuggerStepThrough]
	private void UpdateIsBusy(object sender, EventArgs e)
	{
		RaisePropertyChanged(() => IsRhinoBusy);
	}

	private void UseAlternateCountryCode(string countryCode)
	{
		UIHelper.CounterSketchOptions.CountryCode = countryCode;
		ViewModelLocator.Current.StudioMaterialSelectionVM.ReloadMaterialSearchFilters();
		RaisePropertyChanged(() => CountryCode);
	}

	private void ViewCounterSketchLog()
	{
		WindowDebug windowDebug = new WindowDebug();
		windowDebug.Owner = System.Windows.Application.Current.MainWindow;
		windowDebug.Show();
		windowDebug.BringIntoView();
	}

	private void ViewGemInfo()
	{
		UIHelper.IsGemReportActive = true;
		ViewModelLocator.Current.GemReportVM.Populate();
		if (_windowGemReporter == null)
		{
			_windowGemReporter = new WindowGemReportView
			{
				Topmost = true
			};
			_windowGemReporter.Show();
		}
		else if (!_windowGemReporter.IsVisible)
		{
			_windowGemReporter.Visibility = Visibility.Visible;
		}
	}

	private void WindowSize1050()
	{
		System.Windows.Application.Current.MainWindow.Height = 1050.0;
		System.Windows.Application.Current.MainWindow.Width = 1680.0;
		CenterWindowOnScreen();
	}

	private void WindowSize1080()
	{
		System.Windows.Application.Current.MainWindow.Height = 1080.0;
		System.Windows.Application.Current.MainWindow.Width = 1920.0;
		CenterWindowOnScreen();
	}

	public void NavigateFromStyleQuiz(string primarySearchElement, string secondarySearchElement)
	{
		GlobalErrorHandler.WriteInfoToEventLog($"Style Quiz Results: {primarySearchElement} : {secondarySearchElement}", 2);
		ViewModelLocator.Current.TileSearchMainVM.Reset();
		ViewModelLocator.Current.TileSearchMainVM.SetSelectedSearchTile(ViewModelLocator.Current.TileSearchMainVM.TileSearchCategoryVM.TileSearchCategories.FirstOrDefault(), skipPopulate: true);
		Thread.Sleep(500);
		Task.Factory.StartNew(delegate
		{
			System.Windows.Application.Current.Dispatcher.Invoke(delegate
			{
				ViewModelLocator.Current.TileSearchMainVM.TileSearchResultDesignsVM.GetDesignSearchResultsByDesignElement(primarySearchElement, secondarySearchElement);
			});
		});
	}

	public void OpenStyleQuizWindow()
	{
		EventHelper.RecordEvent(EventHelper.EventTypes.WhatsYourStyle);
		Uri uri = new Uri("http://events.gemvision.com.s3-website-us-east-1.amazonaws.com/style/?cssopen=True");
		TextInfo textInfo = new CultureInfo(UIHelper.CounterSketchOptions.Culture, useUserOverride: false).TextInfo;
		_styleQuizBrowser = new WindowBrowser(uri)
		{
			Width = 1024.0,
			Height = 768.0,
			Title = textInfo.ToTitleCase(Strings.TAKETHEQUIZ.ToLower()),
			Owner = System.Windows.Application.Current.MainWindow
		};
		_styleQuizBrowser.Show();
		_styleQuizBrowser.BringIntoView();
		_styleQuizBrowser.BringWindowToTop();
	}

	private static void ExecuteTest(string testName)
	{
		if (UIHelper.CounterSketchOptions.UserTypeId < 7 || testName == null)
		{
			return;
		}
		string text = testName.ToUpper();
		if (text != null)
		{
			switch (text)
			{
			case "CURATED":
				UIHelper.CounterSketchOptions.PrivateLabel = false;
				UIHelper.CounterSketchOptions.HasEverAndEver = false;
				ViewModelLocator.Current.TileSearchMainVM.TileSearchCategoryVM.PopulateCategories();
				ViewModelLocator.Current.TileViewDesignsVM.Populate();
				break;
			case "PRIVATELABEL":
				UIHelper.CounterSketchOptions.PrivateLabel = true;
				UIHelper.CounterSketchOptions.HasEverAndEver = false;
				ViewModelLocator.Current.TileSearchMainVM.TileSearchCategoryVM.PopulateCategories();
				ViewModelLocator.Current.TileViewDesignsVM.Populate();
				break;
			case "EVERANDEVER":
				UIHelper.CounterSketchOptions.PrivateLabel = false;
				UIHelper.CounterSketchOptions.HasEverAndEver = true;
				ViewModelLocator.Current.TileSearchMainVM.TileSearchCategoryVM.PopulateCategories();
				ViewModelLocator.Current.TileViewDesignsVM.Populate();
				break;
			case "OPENPROGRAMDATAFOLDER":
				Process.Start(UIHelper.CSDataRootPath);
				break;
			case "OPENLOGFILE":
				Process.Start(Path.Combine(UIHelper.CSDataRootPath, "Logging\\CounterSketch.log"));
				break;
			case "CHECKFORUPDATE":
				UpdateHelper.ManualUpdate();
				break;
			case "RESETMEMBERSHIP":
				SecurityHelper.ValidateStoredCredentials();
				ViewModelLocator.Current.TileSearchMainVM.TileSearchCategoryVM.PopulateCategories();
				ViewModelLocator.Current.TileViewDesignsVM.Populate();
				break;
			}
		}
	}

	private void ShopForDiamond()
	{
		PunchoutHelper.ShowDiamondShopper();
	}

	private void ShopForGemstone()
	{
		PunchoutHelper.ShowGemstoneShopper();
	}
}
