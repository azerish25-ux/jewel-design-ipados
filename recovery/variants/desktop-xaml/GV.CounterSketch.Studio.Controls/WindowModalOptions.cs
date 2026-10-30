using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
using GV.CounterSketch.Studio.ViewModel;
using GV.CounterSketchServices.WebServices;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Messaging;
using Localization;

namespace GV.CounterSketch.Studio.Controls;

public partial class WindowModalOptions : Window, IComponentConnector
{
	public delegate void SubmitFullDebugReportHandler(object sender, DebugReportEventArgs e);

	public static readonly DependencyProperty SelectedTabIndexProperty = DependencyProperty.Register("SelectedTabIndex", typeof(int), typeof(WindowModalOptions), new PropertyMetadata(0));

	public static readonly DependencyProperty UsersProperty = DependencyProperty.Register("Users", typeof(ObservableCollection<CSUser>), typeof(WindowModalOptions), new UIPropertyMetadata(new ObservableCollection<CSUser>()));

	private static CounterSketchOptions _resetOptionValues;

	private string _portfolioProjectSaveDirectory;

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
			Messenger.Default.Register<string>(this, ProcessGenericMessage);
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
		if (SecurityHelper.IsStreaming)
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
					Messenger.Default.Send("WindowModalAccount_ReloadCulture");
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
	internal Delegate _CreateDelegate(Type delegateType, string handler)
	{
		return Delegate.CreateDelegate(delegateType, this, handler);
	}
}
