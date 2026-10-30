using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Forms;
using System.Windows.Input;
using System.Windows.Markup;
using GV.CounterSketch.Common;
using GV.CounterSketch.Common.Utilities.Helpers;
using GV.CounterSketch.Studio.Controls.BLL;
using GV.CounterSketch.Studio.Model.Objects.Enums;
using GV.CounterSketch.Studio.Utilities.Helpers;
using GV.CounterSketch.Studio.View;
using GV.CounterSketch.Studio.ViewModel;
using GV.CounterSketchServices.WebServices;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Messaging;
using Localization;

namespace GV.CounterSketch.Studio.Controls;

public class WindowModalOptions : Window, IComponentConnector
{
	public delegate void SubmitFullDebugReportHandler(object sender, DebugReportEventArgs e);

	public static readonly DependencyProperty SelectedTabIndexProperty = DependencyProperty.Register("SelectedTabIndex", typeof(int), typeof(WindowModalOptions), new PropertyMetadata(0));

	public static readonly DependencyProperty UsersProperty = DependencyProperty.Register("Users", typeof(ObservableCollection<CSUser>), typeof(WindowModalOptions), new UIPropertyMetadata(new ObservableCollection<CSUser>()));

	private static CounterSketchOptions _resetOptionValues;

	private string _portfolioProjectSaveDirectory;

	internal WindowModalOptions TheWindow;

	internal DockPanel PanelOptions;

	internal System.Windows.Controls.Button BtnClose;

	internal System.Windows.Controls.Button BtnCancel;

	internal System.Windows.Controls.TabControl TabControl;

	internal TabItem TabItemDisplay;

	internal System.Windows.Controls.CheckBox ChkExpandStackOnDesignOpen;

	internal System.Windows.Controls.CheckBox ChkDisplayRedboxLogo;

	internal System.Windows.Controls.CheckBox ChkIsQuarterCaratLargeStoneRoundingEnabled;

	internal System.Windows.Controls.CheckBox ChkIsWhatsNewVisibleMainUi;

	internal System.Windows.Controls.CheckBox ChkHideAdvancedStackOptions;

	internal DockPanel PnlHideEverAndEver;

	internal System.Windows.Controls.CheckBox ChkHideEverAndEverTile;

	internal DockPanel PnlHideFreehand;

	internal System.Windows.Controls.CheckBox ChkHideFreehandMode;

	internal System.Windows.Controls.CheckBox ChkHideQuickEstimate;

	internal System.Windows.Controls.CheckBox ChkOpenLastOpenedPortfolio;

	internal System.Windows.Controls.CheckBox ChkToggleLargeStoneSettingPrice;

	internal System.Windows.Controls.CheckBox ChkToggleKeepPartsWindowOpen;

	internal System.Windows.Controls.CheckBox ChkToggleKeepPartsWindowOpen1;

	internal System.Windows.Controls.CheckBox ChkIsHideAdvancedStackDefault;

	internal Slider sliderViewportHandleScaleFactor;

	internal System.Windows.Controls.CheckBox ChkShowModelNumber;

	internal System.Windows.Controls.CheckBox ChkShowSpiToolTips;

	internal System.Windows.Controls.CheckBox ChkHideNewRibbons;

	internal TabItem TabItemMaterials;

	internal TabItem TabItemOrderSummary;

	internal TabItem TabItemRegionSetting;

	internal System.Windows.Controls.ComboBox CboRingSize;

	internal System.Windows.Controls.ComboBox CboCurrencyType;

	internal StackPanel StackPanelLanguages;

	internal System.Windows.Controls.ComboBox CboLanguage;

	internal TabItem TabItemTheme;

	internal ThemePickerView ThemePickerView;

	internal TabItem TabItemUpdate;

	internal StackPanel PnlDesigns;

	internal System.Windows.Controls.Button BtnImportLibrary;

	internal StackPanel PnlApplication;

	internal DockPanel PnlUpdateMode;

	internal System.Windows.Controls.Button BtnUpdateApplication;

	internal System.Windows.Controls.CheckBox checkBoxUseStullerServer;

	internal StackPanel PnlExplanation;

	internal TextBlock TxtUpdateModeExplanation;

	internal DockPanel PnlVersion;

	internal TextBlock TxtCurrentVersion;

	internal TextBlock TxtLatestVersion;

	internal TextBlock TxtUpdateProgressText;

	internal System.Windows.Controls.ProgressBar ProUpdateProgressBar;

	internal TabItem TabItemBackupRecovery;

	internal BackupRestoreView BackupRestore;

	internal TextBlock textBlock;

	internal TabItem TabItemTroubleShooting;

	internal System.Windows.Controls.CheckBox ChkDisableMultiCoreRendering;

	internal System.Windows.Controls.Button BtnShowLog;

	internal System.Windows.Controls.Button BtnLaunchTroubleShooter;

	internal System.Windows.Controls.TextBox TxtDebugComments;

	internal StackPanel PnlSupportInfo;

	internal TextBlock TxtSupportHours;

	internal TextBlock TxtSupportPhone;

	internal TextBlock TxtSupportEmailLabel;

	internal TextBlock TxtSupportEmail;

	internal System.Windows.Controls.Button BtnSubmitDebugReport;

	private bool _contentLoaded;

	public int SelectedTabIndex
	{
		get
		{
			return (int)GetValue(SelectedTabIndexProperty);
		}
		set
		{
			SetValue(SelectedTabIndexProperty, value);
		}
	}

	public ObservableCollection<CSUser> Users
	{
		get
		{
			return (ObservableCollection<CSUser>)GetValue(UsersProperty);
		}
		set
		{
			SetValue(UsersProperty, value);
		}
	}

	public bool IsTroubleShooting { get; set; }

	public event EventHandler ShowDesignModelNumberToolTipChangeRequested;

	public event SubmitFullDebugReportHandler SubmitFullDebugReportRequested;

	public event EventHandler UseCustomerLogoChangeRequested;

	public WindowModalOptions()
	{
		InitializeComponent();
		List<string> itemsSource = EnumHelper.ConvertEnumListToStrings<UIRingSizeOptionEnum>(Enum.GetValues(typeof(UIRingSizeOptionEnum)));
		CboRingSize.ItemsSource = itemsSource;
		UpdateApplicationVersionInformation();
		if (!ViewModelBase.IsInDesignModeStatic)
		{
			StoreOptionsForRevertingChanges();
			Messenger.Default.Register<string>((object)this, (Action<string>)ProcessGenericMessage, false);
		}
		else
		{
			LoadOptions();
		}
	}

	public void ShowTab(CSPreferenceTabs tabOption)
	{
		if (tabOption == CSPreferenceTabs.Troubleshooting)
		{
			IsTroubleShooting = true;
			TabItemDisplay.Visibility = Visibility.Collapsed;
			TabItemMaterials.Visibility = Visibility.Collapsed;
			TabItemOrderSummary.Visibility = Visibility.Collapsed;
			TabItemRegionSetting.Visibility = Visibility.Collapsed;
			TabItemTheme.Visibility = Visibility.Collapsed;
			TabItemUpdate.Visibility = Visibility.Collapsed;
			TabItemBackupRecovery.Visibility = Visibility.Collapsed;
			TabItemTroubleShooting.Visibility = Visibility.Visible;
		}
		else
		{
			IsTroubleShooting = false;
			TabItemDisplay.Visibility = Visibility.Visible;
			TabItemMaterials.Visibility = Visibility.Visible;
			TabItemOrderSummary.Visibility = Visibility.Visible;
			TabItemRegionSetting.Visibility = Visibility.Visible;
			TabItemTheme.Visibility = Visibility.Visible;
			TabItemUpdate.Visibility = Visibility.Visible;
			TabItemBackupRecovery.Visibility = Visibility.Visible;
			TabItemTroubleShooting.Visibility = Visibility.Visible;
		}
		if (!SecurityHelper.IsStreaming)
		{
			TabItemUpdate.Visibility = Visibility.Collapsed;
			TabItemBackupRecovery.Visibility = Visibility.Collapsed;
			TabItemOrderSummary.Visibility = Visibility.Collapsed;
			TabItemTroubleShooting.Visibility = Visibility.Collapsed;
		}
		string tabName = string.Empty;
		switch (tabOption)
		{
		case CSPreferenceTabs.Display:
			tabName = "TabItemDisplay";
			break;
		case CSPreferenceTabs.Materials:
			tabName = "TabItemMaterials";
			break;
		case CSPreferenceTabs.Order:
			tabName = "TabItemOrderSummary";
			break;
		case CSPreferenceTabs.Region:
			tabName = "TabItemRegionSetting";
			break;
		case CSPreferenceTabs.Theme:
			tabName = "TabItemTheme";
			break;
		case CSPreferenceTabs.Update:
			tabName = "TabItemUpdate";
			break;
		case CSPreferenceTabs.Backup:
			tabName = "TabItemBackupRecovery";
			break;
		case CSPreferenceTabs.Troubleshooting:
			tabName = "TabItemTroubleShooting";
			break;
		}
		TabItem tabItem = TabControl.Items.Cast<TabItem>().FirstOrDefault((TabItem item) => string.Equals(item.Name, tabName, StringComparison.CurrentCultureIgnoreCase));
		if (tabItem != null)
		{
			SelectedTabIndex = TabControl.Items.IndexOf(tabItem);
		}
	}

	public void StoreOptionsForRevertingChanges()
	{
		_resetOptionValues = UIHelper.CounterSketchOptions.Clone() as CounterSketchOptions;
		LoadOptions();
		ViewModelLocator.Current.ThemePickerVM.InitializeColorPickerViewModels();
	}

	private void BtnCancelClick(object sender, RoutedEventArgs e)
	{
		if (_resetOptionValues != null)
		{
			UIHelper.CounterSketchOptions = _resetOptionValues;
			UIHelper.CoreDataService.UpdateViewPortHandleDrawScale(UIHelper.CounterSketchOptions.ViewPortHandleScaleFactor);
			ViewModelLocator.Current.ThemePickerVM.ViewportBitmapStretchedChanged();
		}
		if (UIHelper.KeyProfile.KeyProfileId != 2)
		{
			ThemeHelper.SaveAndApplyUpdatedCustomTheme(UIHelper.CounterSketchOptions.ThemingColorTheme);
		}
		Hide();
	}

	private void BtnCloseClick(object sender, RoutedEventArgs e)
	{
		SaveOptions();
		if (this.ShowDesignModelNumberToolTipChangeRequested != null)
		{
			this.ShowDesignModelNumberToolTipChangeRequested(sender, e);
		}
		if (this.UseCustomerLogoChangeRequested != null)
		{
			this.UseCustomerLogoChangeRequested(sender, e);
		}
		Hide();
	}

	private void BtnImportLibraryClick(object sender, RoutedEventArgs e)
	{
		CSLibraryHelper.LibraryUpdaterNew(runModelImportCheck: true);
		CSLibraryHelper.LibraryDatabaseUpdateNew();
	}

	private void BtnLaunchTroubleShooterClick(object sender, RoutedEventArgs e)
	{
		string text = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "TroubleShooter\\TroubleShooter.exe");
		if (File.Exists(text))
		{
			Process.Start(text);
		}
	}

	private void BtnShowLogClick(object sender, RoutedEventArgs e)
	{
		WindowDebug windowDebug = new WindowDebug();
		windowDebug.Owner = System.Windows.Application.Current.MainWindow;
		windowDebug.Show();
		windowDebug.BringIntoView();
	}

	private void BtnSubmitDebugReportClick(object sender, RoutedEventArgs e)
	{
		if (this.SubmitFullDebugReportRequested != null)
		{
			this.SubmitFullDebugReportRequested(sender, new DebugReportEventArgs
			{
				Comments = TxtDebugComments.Text
			});
		}
	}

	private void BtnUpdateApplicationClick(object sender, RoutedEventArgs e)
	{
		UpdateHelper.ManualUpdate();
	}

	public void LoadOptions()
	{
		_portfolioProjectSaveDirectory = UIHelper.CounterSketchOptions.UserDefinedPortfolioPath;
		if (UIHelper.KeyProfile.ForceHideFreehand)
		{
			PnlHideFreehand.Visibility = Visibility.Collapsed;
		}
		if (!UIHelper.CounterSketchOptions.HasEverAndEver)
		{
			PnlHideEverAndEver.Visibility = Visibility.Collapsed;
		}
		if (UIHelper.KeyProfile.KeyProfileId == 2)
		{
			TabItemTheme.Visibility = Visibility.Collapsed;
			System.Windows.Controls.CheckBox chkIsQuarterCaratLargeStoneRoundingEnabled = ChkIsQuarterCaratLargeStoneRoundingEnabled;
			bool value = (UIHelper.CounterSketchOptions.IsQuarterCaratLargeStoneRoundingEnabled = true);
			chkIsQuarterCaratLargeStoneRoundingEnabled.IsChecked = value;
			ChkIsQuarterCaratLargeStoneRoundingEnabled.IsEnabled = false;
		}
		if (UIHelper.KeyProfile.KeyProfileId == 3)
		{
			PnlSupportInfo.Visibility = Visibility.Collapsed;
		}
		if (UIHelper.KeyProfile.KeyProfileId == 2)
		{
			TxtSupportPhone.Text = Strings.SupportPhone_Zales;
			TxtSupportHours.Text = Strings.SupportHours_Zales_1 + "\\n" + Strings.SupportHours_Zales_2;
			TxtSupportEmail.Visibility = Visibility.Collapsed;
			TxtSupportEmailLabel.Visibility = Visibility.Collapsed;
		}
		else
		{
			TxtSupportPhone.Text = Strings.SupportPhone;
			TxtSupportHours.Text = Strings.SupportHours_1 + "\n" + Strings.SupportHours_2;
		}
		ChkShowModelNumber.IsChecked = UIHelper.CounterSketchOptions.ShowModelNumber;
		ChkShowSpiToolTips.IsChecked = UIHelper.CounterSketchOptions.ShowSPIToolTipNumber;
		ChkExpandStackOnDesignOpen.IsChecked = UIHelper.CounterSketchOptions.ExpandStackControl;
		ChkHideQuickEstimate.IsChecked = UIHelper.CounterSketchOptions.HideQuickEstimate;
		ChkHideAdvancedStackOptions.IsChecked = UIHelper.CounterSketchOptions.HideAdvancedStackOptions;
		ChkHideFreehandMode.IsChecked = UIHelper.CounterSketchOptions.HideFreehandMode;
		ChkIsQuarterCaratLargeStoneRoundingEnabled.IsChecked = UIHelper.CounterSketchOptions.IsQuarterCaratLargeStoneRoundingEnabled;
		ChkIsWhatsNewVisibleMainUi.IsChecked = UIHelper.CounterSketchOptions.IsWhatsNewVisibleMainUI;
		ChkDisplayRedboxLogo.IsChecked = UIHelper.CounterSketchOptions.DisplayRedboxLogo;
		ChkIsHideAdvancedStackDefault.IsChecked = UIHelper.CounterSketchOptions.IsHideAdvancedStackDefault;
		ChkHideEverAndEverTile.IsChecked = UIHelper.CounterSketchOptions.HideEverAndEverIfAvailable;
		ChkToggleLargeStoneSettingPrice.IsChecked = UIHelper.CounterSketchOptions.IsLargeStoneSettingPriceToggled;
		if (!string.IsNullOrEmpty(UIHelper.CounterSketchOptions.RingSizeSetting))
		{
			CboRingSize.SelectedValue = EnumHelper.GetEnumDescriptionFromValue((UIRingSizeOptionEnum)Enum.Parse(typeof(UIRingSizeOptionEnum), UIHelper.CounterSketchOptions.RingSizeSetting));
		}
		else
		{
			CboRingSize.SelectedValue = EnumHelper.GetEnumDescriptionFromValue(UIRingSizeOptionEnum.UnitedStates);
		}
		if (UIHelper.CounterSketchOptions.CurrencyEnumValue == 0)
		{
			CboCurrencyType.SelectedValue = CSEnumCurrencyTypeOption.USD;
		}
		else
		{
			CboCurrencyType.SelectedValue = EnumHelper.GetEnumDescriptionFromValue((CSEnumCurrencyTypeOption)UIHelper.CounterSketchOptions.CurrencyEnumValue);
		}
		ChkHideNewRibbons.IsChecked = UIHelper.CounterSketchOptions.HideNEWRibbons;
		checkBoxUseStullerServer.IsChecked = UIHelper.CounterSketchOptions.UseStullerServerForUpdates;
		ChkOpenLastOpenedPortfolio.IsChecked = UIHelper.CounterSketchOptions.OpenLastOpenedPortfolio;
		StackPanelLanguages.Visibility = Visibility.Visible;
		List<CultureInfo> availableCultureInfos = TranslationHelper.GetAvailableCultureInfos();
		CboLanguage.ItemsSource = availableCultureInfos;
		if (!string.IsNullOrWhiteSpace(UIHelper.CounterSketchOptions.Culture))
		{
			CboLanguage.SelectedItem = availableCultureInfos.FirstOrDefault((CultureInfo _) => _.TwoLetterISOLanguageName == UIHelper.CounterSketchOptions.Culture);
		}
		sliderViewportHandleScaleFactor.Value = UIHelper.CounterSketchOptions.ViewPortHandleScaleFactor * 100.0;
		ChkToggleKeepPartsWindowOpen.IsChecked = UIHelper.CounterSketchOptions.KeepPartsWindowOpen;
		ViewModelLocator.Current.MainVM.IsTimedBackupEnabled = UIHelper.CounterSketchOptions.IsTimedBackupEnabled;
		ViewModelLocator.Current.MainVM.TimedBackupSeconds = UIHelper.CounterSketchOptions.TimedBackupSeconds;
		ViewModelLocator.Current.ThemePickerVM.ViewPortBackgroundPath = UIHelper.CounterSketchOptions.ViewportGraphicPath;
		ViewModelLocator.Current.ThemePickerVM.IsViewportBitmapStretched = UIHelper.CounterSketchOptions.IsViewportBitmapStretched;
		ChkDisableMultiCoreRendering.IsChecked = !UIHelper.CounterSketchOptions.IsBlockParallelProcessingEnabled;
	}

	private void ProcessGenericMessage(string message)
	{
		base.Dispatcher.Invoke(delegate
		{
			if (message != null)
			{
				string text = message;
				if (!(text == "ClosePreferencesWindow"))
				{
					if (text == "GoToGreyScaleState")
					{
						Hide();
					}
				}
				else
				{
					Hide();
				}
			}
		});
	}

	private void SaveOptions()
	{
		bool flag = false;
		if (!IsTroubleShooting)
		{
			UIHelper.CounterSketchOptions.ShowModelNumber = ChkShowModelNumber.IsChecked == true;
			UIHelper.CounterSketchOptions.ShowSPIToolTipNumber = ChkShowSpiToolTips.IsChecked == true;
			UIHelper.CounterSketchOptions.ExpandStackControl = ChkExpandStackOnDesignOpen.IsChecked == true;
			UIHelper.CounterSketchOptions.IsLargeStoneSettingPriceToggled = ChkToggleLargeStoneSettingPrice.IsChecked == true;
			UIHelper.CounterSketchOptions.HideQuickEstimate = ChkHideQuickEstimate.IsChecked == true;
			UIHelper.CounterSketchOptions.HideAdvancedStackOptions = ChkHideAdvancedStackOptions.IsChecked == true;
			UIHelper.CounterSketchOptions.HideFreehandMode = ChkHideFreehandMode.IsChecked == true;
			UIHelper.CounterSketchOptions.IsQuarterCaratLargeStoneRoundingEnabled = ChkIsQuarterCaratLargeStoneRoundingEnabled.IsChecked == true;
			UIHelper.CounterSketchOptions.IsWhatsNewVisibleMainUI = ChkIsWhatsNewVisibleMainUi.IsChecked == true;
			UIHelper.CounterSketchOptions.DisplayRedboxLogo = ChkDisplayRedboxLogo.IsChecked == true;
			UIHelper.CounterSketchOptions.IsHideAdvancedStackDefault = ChkIsHideAdvancedStackDefault.IsChecked == true;
			UIHelper.CounterSketchOptions.HideEverAndEverIfAvailable = ChkHideEverAndEverTile.IsChecked == true;
			if (UIHelper.CounterSketchOptions.HideEverAndEverIfAvailable)
			{
				ViewModelLocator.Current.TileSearchMainVM.TileSearchCategoryVM.HasEverAndEver = false;
			}
			else if (UIHelper.CounterSketchOptions.HasEverAndEver)
			{
				ViewModelLocator.Current.TileSearchMainVM.TileSearchCategoryVM.HasEverAndEver = true;
			}
			ViewModelLocator.Current.PreferenceMaterialsVM.UpdateMaterialPrefrenceSettings();
			ViewModelLocator.Current.PreferenceOrderScreenVM.UpdatePreferenceOrderSummaryCounterSketchOptions();
			if (ViewModelLocator.Current.MainVM.IsOrderModeActive)
			{
				ViewModelLocator.Current.OrderSummaryVM.Populate(skipVolume: true, skipGemShop: true);
			}
			if (EnumHelper.GetEnumValueFromDescription(CboRingSize.SelectedValue.ToString(), typeof(UIRingSizeOptionEnum)).ToString() != UIHelper.CounterSketchOptions.RingSizeSetting)
			{
				UIHelper.CounterSketchOptions.RingSizeSetting = EnumHelper.GetEnumValueFromDescription(CboRingSize.SelectedValue.ToString(), typeof(UIRingSizeOptionEnum)).ToString();
				if (UIHelper.CounterSketchOptions.RingSizeSetting != null && UIHelper.CounterSketchOptions.RingSizeSetting.Length > 1)
				{
					UIHelper.CoreDataService.DocumentSetRingSizeSetting(UIHelper.CounterSketchOptions.RingSizeSetting);
				}
				DispatcherHelper.WaitForPriority();
			}
			UIHelper.CounterSketchOptions.UseStullerServerForUpdates = checkBoxUseStullerServer.IsChecked == true;
			BitsHelper.UpdateServerLocationForDownloads(UIHelper.CounterSketchOptions.UseStullerServerForUpdates);
			UIHelper.CounterSketchOptions.HideNEWRibbons = ChkHideNewRibbons.IsChecked == true;
			UIHelper.CounterSketchOptions.OpenLastOpenedPortfolio = ChkOpenLastOpenedPortfolio.IsChecked == true;
			UIHelper.CounterSketchOptions.ViewPortHandleScaleFactor = sliderViewportHandleScaleFactor.Value / 100.0;
			UIHelper.CounterSketchOptions.IsViewportBitmapStretched = ViewModelLocator.Current.ThemePickerVM.IsViewportBitmapStretched;
			UIHelper.CounterSketchOptions.ViewportGraphicPath = ViewModelLocator.Current.ThemePickerVM.ViewPortBackgroundPath;
			UIHelper.CounterSketchOptions.KeepPartsWindowOpen = ChkToggleKeepPartsWindowOpen.IsChecked == true;
			if (UIHelper.IsInternetAccessible())
			{
				if (CboCurrencyType.SelectedValue != null)
				{
					UIHelper.CounterSketchOptions.CurrencyEnumValue = (int)EnumHelper.GetEnumValueFromDescription(CboCurrencyType.SelectedValue.ToString(), typeof(CSEnumCurrencyTypeOption));
				}
				try
				{
					using CounterSketchService counterSketchService = new CounterSketchService();
					string cultureCode;
					float exchangeRate = counterSketchService.GetExchangeRate(Enum.GetName(typeof(CSEnumCurrencyTypeOption), UIHelper.CounterSketchOptions.CurrencyEnumValue), out cultureCode);
					if (cultureCode != null)
					{
						UIHelper.CounterSketchOptions.CurrentExchangeRate = exchangeRate;
						UIHelper.CounterSketchOptions.CurrentCultureCode = cultureCode;
					}
				}
				catch (Exception)
				{
				}
				if (UIHelper.CounterSketchOptions.CurrentCultureCode != null)
				{
					Messenger.Default.Send<string>("WindowModalAccount_ReloadCulture");
				}
			}
			string twoLetterISOLanguageName = ((CultureInfo)CboLanguage.SelectedItem).TwoLetterISOLanguageName;
			if (twoLetterISOLanguageName != UIHelper.CounterSketchOptions.Culture)
			{
				flag = true;
			}
			UIHelper.CounterSketchOptions.Culture = twoLetterISOLanguageName;
			ViewModelLocator.Current.TileSearchMainVM.TileSearchResultDesignsVM.UpdateSPINumberVisibility();
			ViewModelLocator.Current.TileSearchMainVM.TileSearchResultDesignsVM.ShowNewDesignsFirstOption = !UIHelper.CounterSketchOptions.HideNEWRibbons;
			if (_portfolioProjectSaveDirectory != UIHelper.CounterSketchOptions.UserDefinedPortfolioPath)
			{
				ProjectManagementHelper.ResetProjectManagement();
			}
			UIHelper.CounterSketchOptions.IsTimedBackupEnabled = ViewModelLocator.Current.MainVM.IsTimedBackupEnabled;
			int num = ViewModelLocator.Current.MainVM.TimedBackupSeconds;
			if (num < 60)
			{
				num = 60;
			}
			UIHelper.CounterSketchOptions.TimedBackupSeconds = num;
			(System.Windows.Application.Current.MainWindow as MainWindow)?.UpdateTimedBackupTimer(UIHelper.CounterSketchOptions.TimedBackupSeconds);
		}
		UIHelper.CounterSketchOptions.IsBlockParallelProcessingEnabled = ChkDisableMultiCoreRendering.IsChecked != true;
		UIHelper.CounterSketchOptions.WriteSettings();
		UIHelper.CoreDataService.ToggleQuarterCaratGemRounding(UIHelper.CounterSketchOptions.IsQuarterCaratLargeStoneRoundingEnabled);
		ViewModelLocator.Current.MainVM.IsProjetPrinterSupportEnabled = UIHelper.CounterSketchOptions.EnableProjetPrinterSupport;
		ViewModelLocator.Current.MainVM.IsDisplayingRedboxLogo = UIHelper.CounterSketchOptions.DisplayRedboxLogo;
		ViewModelLocator.Current.MainVM.IsWhatsNewVisibleMainUI = UIHelper.CounterSketchOptions.IsWhatsNewVisibleMainUI;
		ViewModelLocator.Current.ThemePickerVM.ApplyCustomColorTheme();
		UIHelper.CoreDataService.SetIsBlockParallelProcessingEnabled(UIHelper.CounterSketchOptions.IsBlockParallelProcessingEnabled);
		ViewModelLocator.Current.TileSearchMainVM.TileSearchCategoryVM.SetFreehandTileState(UIHelper.CounterSketchOptions.HideFreehandMode == true);
		UIHelper.CoreDataService.UpdateViewPortHandleDrawScale(UIHelper.CounterSketchOptions.ViewPortHandleScaleFactor);
		if (flag && System.Windows.MessageBox.Show(this, TranslationHelper.GetTranslatedString("LanguageChangeRestartPrmpt"), TranslationHelper.GetTranslatedString("Restart"), MessageBoxButton.YesNo) == MessageBoxResult.Yes)
		{
			base.Dispatcher.Invoke(delegate
			{
				UIHelper.CounterSketchOptions.AllowMoreThanOneInstanceOfCounterSketch = true;
				UIHelper.CounterSketchOptions.WriteSettings();
				System.Windows.Forms.Application.Restart();
				System.Windows.Application.Current.Shutdown();
			});
		}
	}

	private void SliderViewportHandleScaleFactor_OnValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
	{
		UIHelper.CoreDataService.UpdateViewPortHandleDrawScale(e.NewValue / 100.0);
	}

	private void TxtSupportEmailMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
	{
		try
		{
			Process.Start(new ProcessStartInfo("mailto://countersketch@gemvision.com"));
			e.Handled = true;
		}
		catch
		{
		}
	}

	private void UpdateApplicationVersionInformation()
	{
		TxtCurrentVersion.Text = UIHelper.VersionString;
		TxtLatestVersion.Text = UIHelper.LatestVersionString;
	}

	[DebuggerNonUserCode]
	[GeneratedCode("PresentationBuildTasks", "4.0.0.0")]
	public void InitializeComponent()
	{
		if (!_contentLoaded)
		{
			_contentLoaded = true;
			Uri resourceLocator = new Uri("/CounterSketch Studio;component/controls/windowmodaloptions.xaml", UriKind.Relative);
			System.Windows.Application.LoadComponent(this, resourceLocator);
		}
	}

	[DebuggerNonUserCode]
	[GeneratedCode("PresentationBuildTasks", "4.0.0.0")]
	internal Delegate _CreateDelegate(Type delegateType, string handler)
	{
		return Delegate.CreateDelegate(delegateType, this, handler);
	}

	[DebuggerNonUserCode]
	[GeneratedCode("PresentationBuildTasks", "4.0.0.0")]
	[EditorBrowsable(EditorBrowsableState.Never)]
	void IComponentConnector.Connect(int connectionId, object target)
	{
		switch (connectionId)
		{
		case 1:
			TheWindow = (WindowModalOptions)target;
			break;
		case 2:
			PanelOptions = (DockPanel)target;
			break;
		case 3:
			BtnClose = (System.Windows.Controls.Button)target;
			BtnClose.Click += BtnCloseClick;
			break;
		case 4:
			BtnCancel = (System.Windows.Controls.Button)target;
			BtnCancel.Click += BtnCancelClick;
			break;
		case 5:
			TabControl = (System.Windows.Controls.TabControl)target;
			break;
		case 6:
			TabItemDisplay = (TabItem)target;
			break;
		case 7:
			ChkExpandStackOnDesignOpen = (System.Windows.Controls.CheckBox)target;
			break;
		case 8:
			ChkDisplayRedboxLogo = (System.Windows.Controls.CheckBox)target;
			break;
		case 9:
			ChkIsQuarterCaratLargeStoneRoundingEnabled = (System.Windows.Controls.CheckBox)target;
			break;
		case 10:
			ChkIsWhatsNewVisibleMainUi = (System.Windows.Controls.CheckBox)target;
			break;
		case 11:
			ChkHideAdvancedStackOptions = (System.Windows.Controls.CheckBox)target;
			break;
		case 12:
			PnlHideEverAndEver = (DockPanel)target;
			break;
		case 13:
			ChkHideEverAndEverTile = (System.Windows.Controls.CheckBox)target;
			break;
		case 14:
			PnlHideFreehand = (DockPanel)target;
			break;
		case 15:
			ChkHideFreehandMode = (System.Windows.Controls.CheckBox)target;
			break;
		case 16:
			ChkHideQuickEstimate = (System.Windows.Controls.CheckBox)target;
			break;
		case 17:
			ChkOpenLastOpenedPortfolio = (System.Windows.Controls.CheckBox)target;
			break;
		case 18:
			ChkToggleLargeStoneSettingPrice = (System.Windows.Controls.CheckBox)target;
			break;
		case 19:
			ChkToggleKeepPartsWindowOpen = (System.Windows.Controls.CheckBox)target;
			break;
		case 20:
			ChkToggleKeepPartsWindowOpen1 = (System.Windows.Controls.CheckBox)target;
			break;
		case 21:
			ChkIsHideAdvancedStackDefault = (System.Windows.Controls.CheckBox)target;
			break;
		case 22:
			sliderViewportHandleScaleFactor = (Slider)target;
			sliderViewportHandleScaleFactor.ValueChanged += SliderViewportHandleScaleFactor_OnValueChanged;
			break;
		case 23:
			ChkShowModelNumber = (System.Windows.Controls.CheckBox)target;
			break;
		case 24:
			ChkShowSpiToolTips = (System.Windows.Controls.CheckBox)target;
			break;
		case 25:
			ChkHideNewRibbons = (System.Windows.Controls.CheckBox)target;
			break;
		case 26:
			TabItemMaterials = (TabItem)target;
			break;
		case 27:
			TabItemOrderSummary = (TabItem)target;
			break;
		case 28:
			TabItemRegionSetting = (TabItem)target;
			break;
		case 29:
			CboRingSize = (System.Windows.Controls.ComboBox)target;
			break;
		case 30:
			CboCurrencyType = (System.Windows.Controls.ComboBox)target;
			break;
		case 31:
			StackPanelLanguages = (StackPanel)target;
			break;
		case 32:
			CboLanguage = (System.Windows.Controls.ComboBox)target;
			break;
		case 33:
			TabItemTheme = (TabItem)target;
			break;
		case 34:
			ThemePickerView = (ThemePickerView)target;
			break;
		case 35:
			TabItemUpdate = (TabItem)target;
			break;
		case 36:
			PnlDesigns = (StackPanel)target;
			break;
		case 37:
			BtnImportLibrary = (System.Windows.Controls.Button)target;
			BtnImportLibrary.Click += BtnImportLibraryClick;
			break;
		case 38:
			PnlApplication = (StackPanel)target;
			break;
		case 39:
			PnlUpdateMode = (DockPanel)target;
			break;
		case 40:
			BtnUpdateApplication = (System.Windows.Controls.Button)target;
			BtnUpdateApplication.Click += BtnUpdateApplicationClick;
			break;
		case 41:
			checkBoxUseStullerServer = (System.Windows.Controls.CheckBox)target;
			break;
		case 42:
			PnlExplanation = (StackPanel)target;
			break;
		case 43:
			TxtUpdateModeExplanation = (TextBlock)target;
			break;
		case 44:
			PnlVersion = (DockPanel)target;
			break;
		case 45:
			TxtCurrentVersion = (TextBlock)target;
			break;
		case 46:
			TxtLatestVersion = (TextBlock)target;
			break;
		case 47:
			TxtUpdateProgressText = (TextBlock)target;
			break;
		case 48:
			ProUpdateProgressBar = (System.Windows.Controls.ProgressBar)target;
			break;
		case 49:
			TabItemBackupRecovery = (TabItem)target;
			break;
		case 50:
			BackupRestore = (BackupRestoreView)target;
			break;
		case 51:
			textBlock = (TextBlock)target;
			break;
		case 52:
			TabItemTroubleShooting = (TabItem)target;
			break;
		case 53:
			ChkDisableMultiCoreRendering = (System.Windows.Controls.CheckBox)target;
			break;
		case 54:
			BtnShowLog = (System.Windows.Controls.Button)target;
			BtnShowLog.Click += BtnShowLogClick;
			break;
		case 55:
			BtnLaunchTroubleShooter = (System.Windows.Controls.Button)target;
			BtnLaunchTroubleShooter.Click += BtnLaunchTroubleShooterClick;
			break;
		case 56:
			TxtDebugComments = (System.Windows.Controls.TextBox)target;
			break;
		case 57:
			PnlSupportInfo = (StackPanel)target;
			break;
		case 58:
			TxtSupportHours = (TextBlock)target;
			break;
		case 59:
			TxtSupportPhone = (TextBlock)target;
			break;
		case 60:
			TxtSupportEmailLabel = (TextBlock)target;
			break;
		case 61:
			TxtSupportEmail = (TextBlock)target;
			TxtSupportEmail.MouseLeftButtonDown += TxtSupportEmailMouseLeftButtonDown;
			break;
		case 62:
			BtnSubmitDebugReport = (System.Windows.Controls.Button)target;
			BtnSubmitDebugReport.Click += BtnSubmitDebugReportClick;
			break;
		default:
			_contentLoaded = true;
			break;
		}
	}
}
