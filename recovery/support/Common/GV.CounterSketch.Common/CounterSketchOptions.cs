using System;
using System.Collections.Generic;
using System.IO;
using System.Windows.Media;
using System.Xml.Serialization;

namespace GV.CounterSketch.Common;

[Serializable]
public class CounterSketchOptions : ICloneable
{
	private static string _settingsPath;

	private int _currencyEnumValue;

	private List<string> _excludedMaterials;

	private List<string> _excludeQEColumnsList;

	private List<string> _excludeQeRowsList;

	private int _keyId;

	private string _primaryDefaultMaterialName;

	private string _secondaryDefaultMaterialName;

	private string _currentCultureCode;

	private float _currentExchangeRate;

	private ColorTheme _themingColorTheme;

	private string _centerGemDefaultMaterialName;

	private string _sideGemDefaultMaterialName;

	private string _accentGemDefaultMaterialName;

	private List<string> _excludedGemMaterialsList;

	private List<OrderSummaryPrefItemModel> _disabledOrderSummaryItems;

	private List<OrderSummaryPrefItemModel> _defaultOrderSummaryItems;

	private bool _isLargeStoneSettingPriceToggled = true;

	private Color _highlightAccentColor;

	private string _culture;

	public string PrimaryDefaultMaterialName
	{
		get
		{
			return _primaryDefaultMaterialName;
		}
		set
		{
			_primaryDefaultMaterialName = value;
		}
	}

	public bool IsLargeStoneSettingPriceToggled
	{
		get
		{
			return _isLargeStoneSettingPriceToggled;
		}
		set
		{
			_isLargeStoneSettingPriceToggled = value;
		}
	}

	public string SecondaryDefaultMaterialName
	{
		get
		{
			return _secondaryDefaultMaterialName;
		}
		set
		{
			_secondaryDefaultMaterialName = value;
		}
	}

	public int MemberId { get; set; }

	public double ViewPortHandleScaleFactor { get; set; }

	public string CenterGemDefaultMaterialName
	{
		get
		{
			return _centerGemDefaultMaterialName;
		}
		set
		{
			_centerGemDefaultMaterialName = value;
		}
	}

	public string SideGemDefaultMaterialName
	{
		get
		{
			return _sideGemDefaultMaterialName;
		}
		set
		{
			_sideGemDefaultMaterialName = value;
		}
	}

	public string AccentGemDefaultMaterialName
	{
		get
		{
			return _accentGemDefaultMaterialName;
		}
		set
		{
			_accentGemDefaultMaterialName = value;
		}
	}

	public List<string> ExcludedGemMaterialsList
	{
		get
		{
			if (_excludedGemMaterialsList != null)
			{
				return _excludedGemMaterialsList;
			}
			_excludedGemMaterialsList = new List<string>();
			return _excludedGemMaterialsList;
		}
		set
		{
			_excludedGemMaterialsList = value;
		}
	}

	public bool ShowNewDesignsFirst { get; set; }

	public bool AllowMoreThanOneInstanceOfCounterSketch { get; set; }

	[XmlIgnore]
	public int AlternateKeyId { get; set; }

	public string ApplicationCurrentVersion { get; set; }

	public string ApplicationLatestRhinoBuildDate { get; set; }

	public string ApplicationCurrentRhinoBuildDate { get; set; }

	public string ApplicationLatestVersion { get; set; }

	public string ApplicationRhinoUpdatePath { get; set; }

	public string ApplicationMsiUpdatePath { get; set; }

	public string ApplicationMsiUpdateSwitches { get; set; }

	public bool IsManualUpdateModeActive { get; set; }

	public bool IsCustomizePartsListMultiSelect { get; set; }

	public string BackupDirectory { get; set; }

	public string BillToAccountNumber { get; set; }

	public string BuyerFirstName { get; set; }

	public string UserDefinedPortfolioPath { get; set; }

	public string BuyerLastName { get; set; }

	public string BuyerEMail { get; set; }

	public bool BypassErrorCatch { get; set; }

	public bool UseStullerServerForUpdates { get; set; }

	public string Company { get; set; }

	public string CountryCode { get; set; }

	public bool IsNoToggleVisible { get; set; }

	public bool IsBookAndImageToggleVisible { get; set; }

	public bool IsDigitalImageOnlyToggleVisible { get; set; }

	public string Culture
	{
		get
		{
			if (!string.IsNullOrWhiteSpace(_culture))
			{
				return _culture;
			}
			return "en";
		}
		set
		{
			_culture = value;
		}
	}

	public int CurrencyEnumValue
	{
		get
		{
			return _currencyEnumValue;
		}
		set
		{
			_currencyEnumValue = ((value == 0) ? 1 : value);
		}
	}

	public string CurrentCultureCode
	{
		get
		{
			if (_currentCultureCode == null)
			{
				_currentCultureCode = "en-US";
			}
			return _currentCultureCode;
		}
		set
		{
			_currentCultureCode = value;
		}
	}

	public float CurrentExchangeRate
	{
		get
		{
			if ((double)Math.Abs(_currentExchangeRate - 0f) < 0.0)
			{
				_currentExchangeRate = 1f;
			}
			return _currentExchangeRate;
		}
		set
		{
			_currentExchangeRate = value;
		}
	}

	public string CustomLogoPath { get; set; }

	public string DefaultSearchCriteria { get; set; }

	public bool DisableViewportHighlight { get; set; }

	public string Email { get; set; }

	public bool ExpandStackControl { get; set; }

	public string ExportSearchResultsPath { get; set; }

	public List<string> ExcludeQERowsList
	{
		get
		{
			if (_excludeQeRowsList == null)
			{
				_excludeQeRowsList = new List<string>();
			}
			return _excludeQeRowsList;
		}
		set
		{
			_excludeQeRowsList = value;
		}
	}

	public List<string> ExcludeQEColumnsList
	{
		get
		{
			if (_excludeQEColumnsList == null)
			{
				_excludeQEColumnsList = new List<string>();
			}
			return _excludeQEColumnsList;
		}
		set
		{
			_excludeQEColumnsList = value;
		}
	}

	public List<string> ExcludedMaterialsList
	{
		get
		{
			if (_excludedMaterials == null)
			{
				_excludedMaterials = new List<string>();
			}
			return _excludedMaterials;
		}
		set
		{
			_excludedMaterials = value;
		}
	}

	public List<OrderSummaryPrefItemModel> DisabledOrderSummaryItems
	{
		get
		{
			return _disabledOrderSummaryItems;
		}
		set
		{
			_disabledOrderSummaryItems = value;
		}
	}

	public List<OrderSummaryPrefItemModel> DefaultOrderSummaryItems
	{
		get
		{
			return _defaultOrderSummaryItems;
		}
		set
		{
			_defaultOrderSummaryItems = value;
		}
	}

	public DateTime GemDatabaseLastUpdate { get; set; }

	public bool? HideAdvancedStackOptions { get; set; }

	public bool? HideFreehandMode { get; set; }

	public bool HideNEWRibbons { get; set; }

	public bool IsFirstTimeRun { get; set; }

	public bool IsQuarterCaratLargeStoneRoundingEnabled { get; set; }

	public bool IsUnregistered { get; set; }

	public bool IncludeScreenshotWhenPrinting { get; set; }

	public int KeyId
	{
		get
		{
			return _keyId;
		}
		set
		{
			_keyId = value;
		}
	}

	public string LastOpenedInventoryPortfolio { get; set; }

	public string LastOpenedPortfolio { get; set; }

	public double Latitude { get; set; }

	public int LibraryVersion { get; set; }

	public double Longitude { get; set; }

	public bool OpenLastOpenedPortfolio { get; set; }

	public string Phone { get; set; }

	public int PortfolioManagerDesignWidthSliderValue { get; set; }

	public int ProjetIncreaseSupportType { get; set; }

	public string ProjetLastIPLocation { get; set; }

	public int ProjetShowSavedJobsUI { get; set; }

	public bool RenderGroundPlaneAlphaed { get; set; }

	public bool RenderPropsAlphaed { get; set; }

	public string RenderSaveLocation { get; set; }

	public string RingSizeSetting { get; set; }

	public int SearchResultDesignWidth { get; set; }

	public int SearchResultPartWidth { get; set; }

	public string RhinoSerialCode { get; set; }

	public byte[] SerialCode { get; set; }

	public string ShipToAccountNumber { get; set; }

	public string ShipToAddress1 { get; set; }

	public string ShipToAddress2 { get; set; }

	public string ShipToCity { get; set; }

	public string ShipToContactEMail { get; set; }

	public string ShipToContactPhone { get; set; }

	public string ShipToName { get; set; }

	public string ShipToState { get; set; }

	public string ShipToZip { get; set; }

	public string ShipToCountry { get; set; }

	public bool ShowComponentsInSearchDetail { get; set; }

	public bool IsHideAdvancedStackDefault { get; set; }

	public bool ShowDesignerInSearchDetail { get; set; }

	public bool ShowKeywordsInSearchDetail { get; set; }

	public bool ShowModelNumber { get; set; }

	public bool ShowPackagesInSearchDetail { get; set; }

	public bool ShowQuoteEmailWhileOrdering { get; set; }

	public bool ShowSeriesInSearchDetail { get; set; }

	public bool ShowSPIToolTipNumber { get; set; }

	public bool ShowStatusBar { get; set; }

	public double UIScaleFactor { get; set; }

	public bool UpdateDelayedPreviously { get; set; }

	[XmlIgnore]
	public bool UseAlternateKeyId { get; set; }

	[XmlIgnore]
	public int UserTypeId { get; set; }

	public string ViewportBackgroundColor { get; set; }

	public string ViewportGraphicPath { get; set; }

	public string RenderEnvironmentName { get; set; }

	public string RenderGroundPlaneMaterialName { get; set; }

	public string RenderPropName { get; set; }

	public string RenderPropPrimaryMaterialName { get; set; }

	public string RenderPropSecondaryMaterialName { get; set; }

	public string RenderResolutionSize { get; set; }

	public bool StullerHasAuthenticated { get; set; }

	public bool DisplayRedboxLogo { get; set; }

	public bool EnableProjetPrinterSupport { get; set; }

	public int EstimateColorChoice { get; set; }

	public bool? HideQuickEstimate { get; set; }

	public bool IsWhatsNewVisibleMainUI { get; set; }

	public byte[] Password { get; set; }

	public string Username { get; set; }

	public string LastDesignExportPath { get; set; }

	public bool HasAgreedToEula { get; set; }

	public string LastEulaPathAgreedTo { get; set; }

	public string CurrentEulaPath { get; set; }

	public bool HasEverAndEver { get; set; }

	public bool HideEverAndEverIfAvailable { get; set; }

	public bool IsDesignTemplateOne { get; set; }

	public bool IsDesignTemplateTwo { get; set; }

	public bool IsDesignTemplateThree { get; set; }

	public Color TileBackgroundColor { get; set; }

	public Color SearchBackgroundColor { get; set; }

	[XmlIgnore]
	public string JewelerShowcaseURL { get; set; }

	[XmlIgnore]
	public bool AllowFreezePriceForFreehandParts { get; set; }

	public double AnimationTransitionTime { get; set; }

	public bool KeepPartsWindowOpen { get; set; }

	public bool PrivateLabel { get; set; }

	public string AlternatePassword { get; set; }

	public bool RequirePasswordForInventoryMode { get; set; }

	public bool RequirePasswordForPreferences { get; set; }

	public bool UseAlternatePassword { get; set; }

	public ColorTheme ThemingColorTheme
	{
		get
		{
			return _themingColorTheme;
		}
		set
		{
			_themingColorTheme = value;
		}
	}

	public ImageTheme ThemingImageTheme { get; set; }

	public Color HighlightAccentSavedColor
	{
		get
		{
			return _highlightAccentColor;
		}
		set
		{
			_highlightAccentColor = value;
		}
	}

	public ColorTheme ThemingLastCustomColorTheme { get; set; }

	public bool AllowUnsafeHTTPHeaders { get; set; }

	public bool DebugMode { get; set; }

	public bool DisableExpect100Continue { get; set; }

	public bool DisableHardwareRendering { get; set; }

	public string LocalDownloadCachePath { get; set; }

	public int LastKeyProfileId { get; set; }

	public double TileBackgroundBrightness { get; set; }

	public double TileBackgroundSaturation { get; set; }

	public bool TileBackgroundIsGrayscale { get; set; }

	public bool TileIsLightTheme { get; set; }

	public bool TileIsTechTheme { get; set; }

	public bool IsPartsTemplateOne { get; set; }

	public bool IsPartsTemplateTwo { get; set; }

	public bool IsPartsTemplateThree { get; set; }

	[XmlIgnore]
	public bool HasStullerShowcaseMarkups { get; set; }

	public bool IsTimedBackupEnabled { get; set; }

	public int TimedBackupSeconds { get; set; }

	public bool IsViewportBitmapStretched { get; set; }

	public bool IsBlockParallelProcessingEnabled { get; set; }

	public int SearchResultPageSizeDesigns { get; set; } = 72;

	public int SearchResultPageSizeParts { get; set; } = 36;

	public int StreamingAuthenticationTimeoutMilliseconds { get; set; } = 60000;

	public bool SendSMSNotifications { get; set; }

	private CounterSketchOptions()
	{
		DefaultCounterSketchOptions();
	}

	public object Clone()
	{
		try
		{
			using MemoryStream memoryStream = new MemoryStream();
			XmlSerializer xmlSerializer = new XmlSerializer(typeof(CounterSketchOptions));
			xmlSerializer.Serialize(memoryStream, this);
			memoryStream.Position = 0L;
			return xmlSerializer.Deserialize(memoryStream);
		}
		catch (Exception arg)
		{
			GlobalErrorHandler.WriteErrorToEventLog($"Error calling CounterSketchOptions.Clone(): {arg}");
		}
		return null;
	}

	public static CounterSketchOptions NewCounterSketchOptions()
	{
		return new CounterSketchOptions();
	}

	public static CounterSketchOptions ReadSettings(string overridePath = null)
	{
		try
		{
			ValidateSettingsPath(overridePath);
			if (File.Exists(_settingsPath))
			{
				using FileStream stream = File.OpenRead(_settingsPath);
				if (new XmlSerializer(typeof(CounterSketchOptions)).Deserialize(stream) is CounterSketchOptions counterSketchOptions)
				{
					if (counterSketchOptions.PortfolioManagerDesignWidthSliderValue == 0)
					{
						counterSketchOptions.PortfolioManagerDesignWidthSliderValue = 150;
					}
					return counterSketchOptions;
				}
			}
		}
		catch (Exception ex)
		{
			GlobalErrorHandler.WriteErrorToEventLog("Error Reading XML Settings.  Default Settings will be used.\n" + ex.Message);
		}
		return new CounterSketchOptions();
	}

	public void WriteSettings()
	{
		ValidateSettingsPath(Path.GetDirectoryName(_settingsPath));
		using FileStream stream = File.Open(_settingsPath, FileMode.Create, FileAccess.Write);
		new XmlSerializer(typeof(CounterSketchOptions)).Serialize(stream, this);
	}

	private void DefaultCounterSketchOptions()
	{
		UIScaleFactor = 1.0;
		ViewPortHandleScaleFactor = 1.0;
		CurrencyEnumValue = 1;
		ShowStatusBar = true;
		RequirePasswordForInventoryMode = true;
		RequirePasswordForPreferences = true;
		UseAlternatePassword = false;
		RingSizeSetting = "UnitedStates";
		GemDatabaseLastUpdate = DateTime.MinValue;
		ShowDesignerInSearchDetail = true;
		ShowSeriesInSearchDetail = true;
		ShowComponentsInSearchDetail = true;
		IsHideAdvancedStackDefault = true;
		ShowPackagesInSearchDetail = true;
		ShowKeywordsInSearchDetail = true;
		ShowQuoteEmailWhileOrdering = true;
		EnableProjetPrinterSupport = true;
		DisplayRedboxLogo = true;
		IsQuarterCaratLargeStoneRoundingEnabled = true;
		IsLargeStoneSettingPriceToggled = true;
		IsWhatsNewVisibleMainUI = true;
		ShowModelNumber = true;
		DefaultOrderSummaryItems = new List<OrderSummaryPrefItemModel>();
		DisabledOrderSummaryItems = new List<OrderSummaryPrefItemModel>();
		IsFirstTimeRun = true;
		IsDesignTemplateOne = true;
		TileBackgroundColor = Colors.DimGray;
		TileBackgroundBrightness = 0.5;
		TileBackgroundSaturation = 0.5;
		TileBackgroundIsGrayscale = false;
		AnimationTransitionTime = 0.3;
		IsTimedBackupEnabled = true;
		TimedBackupSeconds = 300;
		HideQuickEstimate = true;
		IsBlockParallelProcessingEnabled = true;
	}

	private static void ValidateSettingsPath(string overridePath = null)
	{
		if (overridePath != null)
		{
			_settingsPath = overridePath;
		}
		else
		{
			_settingsPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData), "Gemvision\\CounterSketch Studio");
		}
		if (!Directory.Exists(_settingsPath))
		{
			Directory.CreateDirectory(_settingsPath);
		}
		_settingsPath = Path.Combine(_settingsPath, "CounterSketchApplicationSettings.xml");
	}

	public int GetKeyId()
	{
		if (!UseAlternateKeyId)
		{
			return KeyId;
		}
		return AlternateKeyId;
	}

	public void SetKeyId(int key)
	{
		KeyId = key;
	}
}
