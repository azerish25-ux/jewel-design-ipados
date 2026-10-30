using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;
using System.Xml.Serialization;
using GV.CounterSketch.Common;
using GV.CounterSketch.Shared;
using GV.CounterSketch.Studio.Controls;
using GV.CounterSketch.Studio.Controls.BLL;
using GV.CounterSketch.Studio.Database;
using GV.CounterSketch.Studio.Model.Objects.Enums;
using GV.CounterSketch.Studio.RhinoServicesReference;
using GV.CounterSketch.Studio.Utilities.Extensions;
using GV.CounterSketch.Studio.Utilities.Helpers;
using GV.CounterSketch.Studio.View;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Messaging;
using Localization;
using Microsoft.Win32;
using Stuller.Web.API.v2.Models;
using Stuller.Web.API.v2.Models.Cam;

namespace GV.CounterSketch.Studio.ViewModel;

[Serializable]
public sealed class OrderSummaryViewModel : ViewModelBase
{
	public static readonly RoutedEvent SubmittedRequestForQuoteEvent = EventManager.RegisterRoutedEvent("SubmittedRequestForQuote", RoutingStrategy.Bubble, typeof(RoutedEventHandler), typeof(OrderSummaryViewModel));

	private static readonly BackgroundWorker BackgroundVolumeWorker = new BackgroundWorker();

	private readonly List<string> _gemOrderingErrorList = new List<string>();

	private string _accentGems;

	private string _additionalDesignNotes;

	private bool _attemptingToOrder;

	private string _bandAccentGems;

	private string _busyMessage;

	private string _buyerEMail;

	private string _buyerFirstname;

	private string _buyerLastname;

	private CamJobResponse _camJobResponseResult;

	private string _centerGem;

	private string _combinedMetalWeight;

	private string _customerAddress1;

	private string _customerAddress2;

	private string _customerCity;

	private string _customerCountry;

	private string _customerEmail;

	private string _customerFirstName;

	private string _customerLastName;

	private string _customerMiddleInitial;

	private string _customerPhone1;

	private string _customerPhone2;

	private string _customerPostalCode;

	private string _customerReferenceNumber;

	private string _customerState;

	private string _customerSuffix;

	private string _customerTitle;

	private List<GemGroup> _debugEstimateGemGroups;

	private ImageSource _designPreviewImage;

	private string _engravingMessage;

	private IEnumerable<CSEnumFinishing> _finishingOptions;

	private IEnumerable<CSEnumHallmarking> _hallmarkingOptions;

	private bool _includeDuplicateMatchingBand;

	private bool _includeWaxMemo;

	private bool _isBookAndImagesToggleVisible;

	private bool _isBusy;

	private bool _isCustomEngravingSelected;

	private bool _isDesignQuoted;

	private bool _isDigitalImagesToggleVisible;

	private bool _isEngagementOptionSelected;

	private bool _isEngravingSectionVisible;

	private bool _isHallmarkOptionFourVisible;

	private bool _isHallmarkOptionOneVisible;

	private bool _isHallmarkOptionThreeVisible;

	private bool _isHallmarkOptionTwoVisible;

	private bool _isHallmarkSectionVisible;

	private bool _isMeleeSourceOptionOneVisible;

	private bool _isMeleeSourceOptionThreeVisible;

	private bool _isMeleeSourceOptionTwoVisible;

	private bool _isMoreThanARingSectionVisible;

	private bool _isNoToggleVisible;

	private bool _isSecondaryMetalIncluded;

	private bool _isWaxMemoSectionVisible;

	private bool _isZalesUser;

	private MetalMaterial[] _metals;

	private string _modelNumber;

	private string _notesToCustomer;

	private string _orderReference;

	private string _primaryMetal;

	private string _primaryMetalWeight;

	private decimal _quotedCamAssemblyPrice;

	private decimal _quotedCamPrice;

	private decimal _quotedCastings;

	private decimal _quotedGemsSettingPrice;

	private decimal _quotedInsideEngravingPrice;

	private double _quotedLargeStonesPrice;

	private double _quotedMeleeStonesPrice;

	private decimal _quotedMetalPrice;

	private decimal _quotedMoreThanARingPrice;

	private decimal _quotedOutsideEngravingPrice;

	private decimal _quotedPolishPrice;

	private double _quotedPrice;

	private decimal _quotedRingSizePrice;

	private double _quotedSettingPrice;

	private double _quotedSettingPriceMarkedUp;

	private decimal _quotedSpecialFinishPrice;

	private string _selectedRingSize;

	private string _secondaryMetal;

	private string _secondaryMetalWeight;

	private CSEnumEngraving _selectedEngraving = CSEnumEngraving.NoEngraving;

	private CSEnumEngravingFillColor _selectedEngravingFillColor = CSEnumEngravingFillColor.None;

	private string _selectedEngravingFont;

	private CSEnumEngravingFontsSize _selectedEngravingFontSize;

	private CSEnumFinishing _selectedFinishing = CSEnumFinishing.SemiFinish;

	private CSEnumHallmarking _selectedHallmarking;

	private bool _selectedMoreThanARing;

	private bool _selectedMoreThanARingDigitalOnly;

	private bool _selectedNoMoreThanARing;

	private bool _showEngagementOptions;

	private bool _showEngagementSendOptions;

	private bool _showEngravingOptions;

	private string _sideGems;

	private bool _stonesEnabled;

	private double _totalCaratWeight;

	private bool _understandEngagementRingQuestion;

	private List<string> _validationErrors;

	private bool _isWaxMemoYesVisible;

	private bool _isWaxMemoNoVisible;

	private bool _isGroupingMeleeBySize;

	private List<string> _ringSizes;

	private WindowDesignNotes _windowDesignNotes;

	private bool _isFreehandQuoteExempt;

	private bool _isUsingStullerShowcaseMarkups;

	private ObservableCollection<string> _attachedFiles;

	private bool _isSupportingDocsVisible;

	private bool sendSMSNotifications;

	[XmlIgnore]
	public string ComponentsInOrder => string.Join(",", UIHelper.VisibleComponents);

	public bool IsCustomerReferenceNumberPopulated => !string.IsNullOrWhiteSpace(CustomerReferenceNumber);

	[XmlIgnore]
	public IEnumerable<CSEnumEngravingFillColor> EngravingFillColorOptions => Enum.GetValues(typeof(CSEnumEngravingFillColor)).Cast<CSEnumEngravingFillColor>();

	[XmlIgnore]
	public List<string> EngravingFontOptions => new List<string> { "Bookman Old Style", "Colonna MT", "Edwardian Script ITC", "Lucida Handwriting Italic", "Old English Text MT", "Script MT Bold" };

	[XmlIgnore]
	public IEnumerable<CSEnumEngravingFontsSize> EngravingFontSizeOptions => Enum.GetValues(typeof(CSEnumEngravingFontsSize)).Cast<CSEnumEngravingFontsSize>();

	[XmlIgnore]
	public bool HasValidationErrors
	{
		get
		{
			if (base.IsInDesignMode)
			{
				return true;
			}
			if (_validationErrors != null)
			{
				return _validationErrors.Count > 0;
			}
			return false;
		}
	}

	public bool IsBuyerEMailPopulated => !string.IsNullOrWhiteSpace(BuyerEMail);

	public bool IsBuyerFirstnamePopulated => !string.IsNullOrWhiteSpace(BuyerFirstname);

	public bool IsBuyerLastnamePopulated => !string.IsNullOrWhiteSpace(BuyerLastname);

	public bool IsBuyerAndReferencePopulated
	{
		get
		{
			if (SecurityHelper.IsStreaming)
			{
				return true;
			}
			if (IsBuyerFirstnamePopulated)
			{
				return IsBuyerLastnamePopulated;
			}
			return false;
		}
	}

	public bool IsDesignQuotable
	{
		get
		{
			if (!ProjectManagementHelper.IsDocumentCanvasEmpty() && !HasValidationErrors)
			{
				return ViewModelLocator.Current.OrderSummaryVM.CalculatedVolumes.Count > 0;
			}
			return false;
		}
	}

	public bool IsSendingTestOrder
	{
		get
		{
			if (!string.IsNullOrWhiteSpace(CustomerReferenceNumber))
			{
				return CustomerReferenceNumber.Trim().ToUpperInvariant() == "RCGVTEST";
			}
			return false;
		}
	}

	[XmlIgnore]
	public ObservableCollection<MaterialOptionItem> MetalMaterialOptions => MetalHelper.GetMetalMaterialOptionItems();

	public bool QuoteRequired_Notes
	{
		get
		{
			if (!string.IsNullOrWhiteSpace(AdditionalDesignNotes) || !string.IsNullOrWhiteSpace(NotesToCustomer))
			{
				return true;
			}
			return false;
		}
	}

	public bool QuoteRequired_Freehand
	{
		get
		{
			if (ViewModelLocator.Current.CustomizePartsListVM.ImportGroupsCollection != null && !UIHelper.CounterSketchOptions.AllowFreezePriceForFreehandParts && UIHelper.CoreDataService.GetNumberofVisibleFreehandParts() > 0)
			{
				return true;
			}
			return false;
		}
	}

	public bool QuoteRequiredManualSizing => UIHelper.CoreDataService.IsManualSizingModeOn(ViewModelLocator.Current.StoneTrayVM.IsStoneAssigned);

	public bool QuoteRequired_UserSuppliedStones
	{
		get
		{
			if (ViewModelLocator.Current.GemReportVM.LargeGems.Any((Gem _) => _.IsCustomerStoneSupplied) || ViewModelLocator.Current.GemReportVM.Gems.Any((Gem _) => _.IsCustomerStoneSupplied) || ViewModelLocator.Current.GemReportVM.GemGroups.Any((GemGroup _) => _.IsCustomerStoneSupplied))
			{
				return true;
			}
			return false;
		}
	}

	public bool QuoteRequired_ExpandedSliders
	{
		get
		{
			bool requiresQuote = false;
			UIHelper.CoreDataService.OrderGetExpandedSliderList(delegate(IEnumerable<string> sliders)
			{
				requiresQuote = sliders.Any();
			});
			if (requiresQuote)
			{
				return true;
			}
			return false;
		}
	}

	public bool Requires24HourQuote
	{
		get
		{
			if (!base.IsInDesignMode)
			{
				if (QuoteRequired_Notes)
				{
					return true;
				}
				if (QuoteRequired_UserSuppliedStones)
				{
					return true;
				}
				if (QuoteRequired_ExpandedSliders)
				{
					return true;
				}
				if (QuoteRequired_Freehand)
				{
					return true;
				}
				if (QuoteRequiredManualSizing)
				{
					return true;
				}
			}
			return false;
		}
	}

	public List<string> ValidationErrors
	{
		get
		{
			if (base.IsInDesignMode)
			{
				return new List<string> { "Missing Bill/ship to account number - Update Account Info", "You must Acknowledge Engagement Ring Statement." };
			}
			_validationErrors = new List<string>();
			_validationErrors.AddRange(_gemOrderingErrorList);
			if (!StonesEnabled)
			{
				_validationErrors = new List<string>();
			}
			if (ProjectManagementHelper.IsDocumentCanvasEmpty() || ViewModelLocator.Current.OrderSummaryVM.CalculatedVolumes.Count < 1)
			{
				_validationErrors.Add(Strings.DesignEmptyOpenBuild);
			}
			else
			{
				_validationErrors.Remove(Strings.DesignEmptyOpenBuild);
			}
			if (string.IsNullOrEmpty(UIHelper.CounterSketchOptions.BillToAccountNumber) || string.IsNullOrEmpty(UIHelper.CounterSketchOptions.ShipToAccountNumber))
			{
				_validationErrors.Add(Strings.Missing_BillShip_To_Account_Number);
			}
			else
			{
				_validationErrors.Remove(Strings.Missing_BillShip_To_Account_Number);
			}
			if (UIHelper.KeyProfile.KeyProfileId == 2)
			{
				if (string.IsNullOrEmpty(CustomerReferenceNumber) && _attemptingToOrder)
				{
					_validationErrors.Add(Strings.ReferenceNoIsRequired);
				}
				else
				{
					_validationErrors.Remove(Strings.ReferenceNoIsRequired);
				}
			}
			if (!SecurityHelper.IsStreaming)
			{
				if (string.IsNullOrEmpty(UIHelper.CounterSketchOptions.Email))
				{
					_validationErrors.Add(Strings.MissingQuoteEmailAddress);
				}
				else
				{
					_validationErrors.Remove(Strings.MissingQuoteEmailAddress);
				}
				if (string.IsNullOrEmpty(UIHelper.CounterSketchOptions.Phone))
				{
					_validationErrors.Add(Strings.MissingQuotePhoneAddress);
				}
				else
				{
					_validationErrors.Remove(Strings.MissingQuotePhoneAddress);
				}
			}
			if (IsEngagementOptionSelected)
			{
				if (ShowEngagementSendOptions)
				{
					if (!UnderstandEngagementRingQuestion)
					{
						_validationErrors.Add(Strings.YouMustAcknowledgeEngagementRing);
					}
					else
					{
						_validationErrors.Remove(Strings.YouMustAcknowledgeEngagementRing);
					}
				}
				else
				{
					_validationErrors.Remove(Strings.YouMustAcknowledgeEngagementRing);
				}
			}
			else
			{
				_validationErrors.Remove(Strings.YouMustAcknowledgeEngagementRing);
			}
			return _validationErrors;
		}
	}

	public Visibility ExportStl3DMVisibility
	{
		get
		{
			if (UIHelper.KeyProfile.ExportDesignAllowed || FeatureHelper.HasExportFeature() || UIHelper.KeyProfile.KeyProfileId == 7)
			{
				return Visibility.Visible;
			}
			return Visibility.Collapsed;
		}
	}

	public bool CustomerWillSupplyStonesForSetting
	{
		get
		{
			if (ViewModelLocator.Current.GemReportVM.LargeGems.Any((Gem _) => _.Source == Strings.Use_My_Stone_s) || ViewModelLocator.Current.GemReportVM.Gems.Any((Gem _) => _.Source == Strings.Use_My_Stone_s))
			{
				return true;
			}
			return false;
		}
	}

	public bool IsFreehandQuoteExempt
	{
		get
		{
			return _isFreehandQuoteExempt;
		}
		set
		{
			_isFreehandQuoteExempt = value;
			RaisePropertyChanged("IsFreehandQuoteExempt");
		}
	}

	public bool IsUsingStullerShowcaseMarkups
	{
		get
		{
			return _isUsingStullerShowcaseMarkups;
		}
		set
		{
			_isUsingStullerShowcaseMarkups = value;
			RaisePropertyChanged("IsUsingStullerShowcaseMarkups");
		}
	}

	[XmlIgnore]
	public byte[] PricingImageBytes { get; set; }

	public string AccentGems
	{
		get
		{
			return _accentGems;
		}
		set
		{
			if (!(value == _accentGems))
			{
				_accentGems = value;
				RaisePropertyChanged(() => AccentGems);
			}
		}
	}

	public string AdditionalDesignNotes
	{
		get
		{
			return _additionalDesignNotes;
		}
		set
		{
			_additionalDesignNotes = value;
			RaisePropertyChanged(() => Requires24HourQuote);
			RaisePropertyChanged(() => AdditionalDesignNotes);
			ReValidateOrder();
		}
	}

	public string BusyMessage
	{
		get
		{
			return _busyMessage;
		}
		set
		{
			_busyMessage = value;
			RaisePropertyChanged("BusyMessage");
		}
	}

	public string BandAccentGems
	{
		get
		{
			return _bandAccentGems;
		}
		set
		{
			if (!(value == _bandAccentGems))
			{
				_bandAccentGems = value;
				RaisePropertyChanged(() => BandAccentGems);
			}
		}
	}

	public string BuyerFirstname
	{
		get
		{
			return _buyerFirstname;
		}
		set
		{
			_buyerFirstname = value;
			RaisePropertyChanged("BuyerFirstname");
			RaisePropertyChanged("IsBuyerAndReferencePopulated");
			RaisePropertyChanged("IsBuyerFirstnamePopulated");
		}
	}

	public string BuyerLastname
	{
		get
		{
			return _buyerLastname;
		}
		set
		{
			_buyerLastname = value;
			RaisePropertyChanged("BuyerLastname");
			RaisePropertyChanged("IsBuyerAndReferencePopulated");
			RaisePropertyChanged("IsBuyerLastnamePopulated");
		}
	}

	public string BuyerEMail
	{
		get
		{
			return _buyerEMail;
		}
		set
		{
			_buyerEMail = value;
			RaisePropertyChanged("BuyerEMail");
			RaisePropertyChanged("IsBuyerEMailPopulated");
		}
	}

	[XmlIgnore]
	public Dictionary<Tuple<ComponentsEnum, MaterialClassification>, double> CalculatedVolumes { get; set; }

	public CamJobResponse CamJobResponseResult
	{
		get
		{
			return _camJobResponseResult;
		}
		set
		{
			_camJobResponseResult = value;
			RaisePropertyChanged("CamJobResponseResult");
		}
	}

	public string CenterGem
	{
		get
		{
			return _centerGem;
		}
		set
		{
			if (!(value == _centerGem))
			{
				_centerGem = value;
				RaisePropertyChanged(() => CenterGem);
			}
		}
	}

	[XmlIgnore]
	public RelayCommand CommandCloseDesignNotesWindow { get; set; }

	[XmlIgnore]
	public RelayCommand CommandOpenDesignNotesWindow { get; set; }

	[XmlIgnore]
	public RelayCommand CommandOpenMoreThanARingPreview { get; set; }

	[XmlIgnore]
	public RelayCommand CommandOrderSubmit { get; set; }

	[XmlIgnore]
	public RelayCommand CommandPrintQuote { get; set; }

	[XmlIgnore]
	public RelayCommand CommandExportSTL3DM { get; set; }

	[XmlIgnore]
	public RelayCommand CommandQuoteSubmit { get; set; }

	[XmlIgnore]
	public RelayCommand CommandSaveQuote { get; set; }

	public string CustomerAddress1
	{
		get
		{
			return _customerAddress1;
		}
		set
		{
			if (!(value == _customerAddress1))
			{
				_customerAddress1 = value;
				RaisePropertyChanged(() => CustomerAddress1);
			}
		}
	}

	public string CustomerAddress2
	{
		get
		{
			return _customerAddress2;
		}
		set
		{
			if (!(value == _customerAddress2))
			{
				_customerAddress2 = value;
				RaisePropertyChanged(() => CustomerAddress2);
			}
		}
	}

	public string CustomerCity
	{
		get
		{
			return _customerCity;
		}
		set
		{
			if (!(value == _customerCity))
			{
				_customerCity = value;
				RaisePropertyChanged(() => CustomerCity);
			}
		}
	}

	public string CustomerCountry
	{
		get
		{
			return _customerCountry;
		}
		set
		{
			if (!(value == _customerCountry))
			{
				_customerCountry = value;
				RaisePropertyChanged(() => CustomerCountry);
			}
		}
	}

	public string CustomerEmail
	{
		get
		{
			return _customerEmail;
		}
		set
		{
			if (!(value == _customerEmail))
			{
				_customerEmail = value;
				RaisePropertyChanged(() => CustomerEmail);
			}
		}
	}

	public string CustomerFirstName
	{
		get
		{
			return _customerFirstName;
		}
		set
		{
			if (!(value == _customerFirstName))
			{
				_customerFirstName = value;
				RaisePropertyChanged(() => CustomerFirstName);
			}
		}
	}

	public string CustomerLastName
	{
		get
		{
			return _customerLastName;
		}
		set
		{
			if (!(value == _customerLastName))
			{
				_customerLastName = value;
				RaisePropertyChanged(() => CustomerLastName);
			}
		}
	}

	public string CustomerMiddleInitial
	{
		get
		{
			return _customerMiddleInitial;
		}
		set
		{
			if (!(value == _customerMiddleInitial))
			{
				_customerMiddleInitial = value;
				RaisePropertyChanged(() => CustomerMiddleInitial);
			}
		}
	}

	public string CustomerPhone1
	{
		get
		{
			return _customerPhone1;
		}
		set
		{
			if (!(value == _customerPhone1))
			{
				_customerPhone1 = value;
				RaisePropertyChanged(() => CustomerPhone1);
			}
		}
	}

	public string CustomerPhone2
	{
		get
		{
			return _customerPhone2;
		}
		set
		{
			if (!(value == _customerPhone2))
			{
				_customerPhone2 = value;
				RaisePropertyChanged(() => CustomerPhone2);
			}
		}
	}

	public string CustomerPostalCode
	{
		get
		{
			return _customerPostalCode;
		}
		set
		{
			if (!(value == _customerPostalCode))
			{
				_customerPostalCode = value;
				RaisePropertyChanged(() => CustomerPostalCode);
			}
		}
	}

	public string CustomerReferenceNumber
	{
		get
		{
			return _customerReferenceNumber;
		}
		set
		{
			_customerReferenceNumber = value;
			RaisePropertyChanged(() => CustomerReferenceNumber);
			RaisePropertyChanged(() => IsSendingTestOrder);
			RaisePropertyChanged("IsBuyerAndReferencePopulated");
			RaisePropertyChanged("IsCustomerReferenceNumberPopulated");
		}
	}

	public string CustomerState
	{
		get
		{
			return _customerState;
		}
		set
		{
			if (!(value == _customerState))
			{
				_customerState = value;
				RaisePropertyChanged(() => CustomerState);
			}
		}
	}

	public string CustomerSuffix
	{
		get
		{
			return _customerSuffix;
		}
		set
		{
			if (!(value == _customerSuffix))
			{
				_customerSuffix = value;
				RaisePropertyChanged(() => CustomerSuffix);
			}
		}
	}

	public string CustomerTitle
	{
		get
		{
			return _customerTitle;
		}
		set
		{
			if (!(value == _customerTitle))
			{
				_customerTitle = value;
				RaisePropertyChanged(() => CustomerTitle);
			}
		}
	}

	public string EngravingMessage
	{
		get
		{
			return _engravingMessage;
		}
		set
		{
			if (!(value == _engravingMessage))
			{
				_engravingMessage = value;
				if (!string.IsNullOrWhiteSpace(_engravingMessage) && _engravingMessage.Length > 13)
				{
					_engravingMessage = _engravingMessage.Substring(0, 13);
				}
				RaisePropertyChanged(() => EngravingMessage);
				ResetQuote();
				ReValidateOrder();
			}
		}
	}

	[XmlIgnore]
	public IEnumerable<CSEnumFinishing> FinishingOptions
	{
		get
		{
			return _finishingOptions;
		}
		set
		{
			if (ViewModelBase.IsInDesignModeStatic)
			{
				_finishingOptions = new List<CSEnumFinishing> { CSEnumFinishing.FinishedAndSet };
				return;
			}
			if (UIHelper.KeyProfile.KeyProfileId == 2)
			{
				List<CSEnumFinishing> source = new List<CSEnumFinishing>
				{
					CSEnumFinishing.Finished,
					CSEnumFinishing.FinishedAndSet
				};
				_finishingOptions = source.Where((CSEnumFinishing i) => UIHelper.CounterSketchOptions.DisabledOrderSummaryItems.All((OrderSummaryPrefItemModel _) => _.Title != i.GetDescription()));
			}
			_finishingOptions = from CSEnumFinishing i in Enum.GetValues(typeof(CSEnumFinishing))
				where UIHelper.CounterSketchOptions.DisabledOrderSummaryItems.All((OrderSummaryPrefItemModel _) => _.Title != i.GetDescription())
				select i;
			RaisePropertyChanged("FinishingOptions");
		}
	}

	[XmlIgnore]
	public IEnumerable<CSEnumHallmarking> HallmarkingOptions
	{
		get
		{
			return _hallmarkingOptions;
		}
		set
		{
			_hallmarkingOptions = from CSEnumHallmarking _ in Enum.GetValues(typeof(CSEnumHallmarking))
				where UIHelper.CounterSketchOptions.DisabledOrderSummaryItems.All((OrderSummaryPrefItemModel a) => a.Title != _.GetDescription())
				select _;
			RaisePropertyChanged("HallmarkingOptions");
		}
	}

	public bool IsBusy
	{
		get
		{
			return _isBusy;
		}
		set
		{
			if (!value || !_isBusy)
			{
				_isBusy = value;
				UIHelper.ShowWaitWindow(value, BusyMessage);
			}
		}
	}

	public bool IsCustomEngravingSelected
	{
		get
		{
			return _isCustomEngravingSelected;
		}
		set
		{
			_isCustomEngravingSelected = value;
			RaisePropertyChanged(() => IsCustomEngravingSelected);
			ResetQuote();
			ReValidateOrder();
		}
	}

	public bool IsDesignQuoted
	{
		get
		{
			return _isDesignQuoted;
		}
		set
		{
			_isDesignQuoted = value;
			RaisePropertyChanged("IsDesignQuoted");
		}
	}

	public bool IsEngagementOptionSelected
	{
		get
		{
			return _isEngagementOptionSelected;
		}
		set
		{
			_isEngagementOptionSelected = value;
			RaisePropertyChanged(() => IsEngagementOptionSelected);
			ResetQuote();
			ReValidateOrder();
		}
	}

	public bool IsSecondaryMetalIncluded
	{
		get
		{
			return _isSecondaryMetalIncluded;
		}
		set
		{
			_isSecondaryMetalIncluded = value;
			RaisePropertyChanged("IsSecondaryMetalIncluded");
		}
	}

	public bool IsZalesUser
	{
		get
		{
			return _isZalesUser;
		}
		set
		{
			_isZalesUser = value;
			RaisePropertyChanged("IsZalesUser");
		}
	}

	public string ModelNumber
	{
		get
		{
			return _modelNumber;
		}
		set
		{
			_modelNumber = value;
			RaisePropertyChanged(() => ModelNumber);
		}
	}

	public string OrderReference
	{
		get
		{
			return _orderReference;
		}
		set
		{
			_orderReference = value;
			RaisePropertyChanged(() => OrderReference);
		}
	}

	public decimal PennyWeightMetalPrimary { get; set; }

	public decimal PennyWeightMetalSecondary { get; set; }

	public string PrimaryMetal
	{
		get
		{
			return _primaryMetal;
		}
		set
		{
			if (value == _primaryMetal)
			{
				return;
			}
			_primaryMetal = value;
			if (!string.IsNullOrEmpty(value) && !base.IsInDesignMode)
			{
				ViewModelLocator.Current.StudioMaterialSelectionVM.SetMetal(isPrimary: true, MetalMaterialOptions.First((MaterialOptionItem _) => _.PrimaryText == value).SearchAttributeId);
				CalculateVolumeInBackground();
			}
			RaisePropertyChanged(() => PrimaryMetal);
		}
	}

	public string PrimaryMetalWeight
	{
		get
		{
			return _primaryMetalWeight;
		}
		set
		{
			_primaryMetalWeight = value;
			RaisePropertyChanged("PrimaryMetalWeight");
		}
	}

	public string CombinedMetalWeight
	{
		get
		{
			return _combinedMetalWeight;
		}
		set
		{
			_combinedMetalWeight = value;
			RaisePropertyChanged("CombinedMetalWeight");
		}
	}

	public decimal QuotedCamAssemblyPrice
	{
		get
		{
			return _quotedCamAssemblyPrice;
		}
		set
		{
			_quotedCamAssemblyPrice = value;
			RaisePropertyChanged("QuotedCamAssemblyPrice");
		}
	}

	public decimal QuotedCamPrice
	{
		get
		{
			return _quotedCamPrice;
		}
		set
		{
			_quotedCamPrice = value;
			RaisePropertyChanged("QuotedCamPrice");
		}
	}

	public decimal QuotedCastings
	{
		get
		{
			return _quotedCastings;
		}
		set
		{
			_quotedCastings = value;
			RaisePropertyChanged("QuotedCastings");
		}
	}

	public string QuotedDesignPath { get; set; }

	public decimal QuotedGemsSettingPrice
	{
		get
		{
			return _quotedGemsSettingPrice;
		}
		set
		{
			_quotedGemsSettingPrice = value;
			RaisePropertyChanged("QuotedGemsSettingPrice");
		}
	}

	public decimal QuotedInsideEngravingPrice
	{
		get
		{
			return _quotedInsideEngravingPrice;
		}
		set
		{
			_quotedInsideEngravingPrice = value;
			RaisePropertyChanged("QuotedInsideEngravingPrice");
		}
	}

	public double QuotedLargeStonesPrice
	{
		get
		{
			return _quotedLargeStonesPrice;
		}
		set
		{
			_quotedLargeStonesPrice = value;
			RaisePropertyChanged("QuotedLargeStonesPrice");
		}
	}

	public double QuotedMeleeStonesPrice
	{
		get
		{
			return _quotedMeleeStonesPrice;
		}
		set
		{
			_quotedMeleeStonesPrice = value;
			RaisePropertyChanged("QuotedMeleeStonesPrice");
		}
	}

	public decimal QuotedMetalPrice
	{
		get
		{
			return _quotedMetalPrice;
		}
		set
		{
			_quotedMetalPrice = value;
			RaisePropertyChanged("QuotedMetalPrice");
		}
	}

	public decimal QuotedMoreThanARingPrice
	{
		get
		{
			return _quotedMoreThanARingPrice;
		}
		set
		{
			_quotedMoreThanARingPrice = value;
			RaisePropertyChanged("QuotedMoreThanARingPrice");
		}
	}

	public decimal QuotedOutsideEngravingPrice
	{
		get
		{
			return _quotedOutsideEngravingPrice;
		}
		set
		{
			_quotedOutsideEngravingPrice = value;
			RaisePropertyChanged("QuotedOutsideEngravingPrice");
		}
	}

	public decimal QuotedPolishPrice
	{
		get
		{
			return _quotedPolishPrice;
		}
		set
		{
			_quotedPolishPrice = value;
			RaisePropertyChanged("QuotedPolishPrice");
		}
	}

	public double QuotedPrice
	{
		get
		{
			return _quotedPrice;
		}
		set
		{
			_quotedPrice = value;
			RaisePropertyChanged("QuotedPrice");
		}
	}

	public decimal QuotedRingSizePrice
	{
		get
		{
			return _quotedRingSizePrice;
		}
		set
		{
			_quotedRingSizePrice = value;
			RaisePropertyChanged("QuotedRingSizePrice");
		}
	}

	public double QuotedSettingPrice
	{
		get
		{
			return _quotedSettingPrice;
		}
		set
		{
			_quotedSettingPrice = value;
			RaisePropertyChanged("QuotedSettingPrice");
		}
	}

	public double QuotedSettingPriceMarkedUp
	{
		get
		{
			return _quotedSettingPriceMarkedUp;
		}
		set
		{
			_quotedSettingPriceMarkedUp = value;
			RaisePropertyChanged("QuotedSettingPriceMarkedUp");
		}
	}

	public decimal QuotedSpecialFinishPrice
	{
		get
		{
			return _quotedSpecialFinishPrice;
		}
		set
		{
			_quotedSpecialFinishPrice = value;
			RaisePropertyChanged("QuotedSpecialFinishPrice");
		}
	}

	public string QuoteEMail { get; set; }

	public List<GemGroup> ShopLargeGemGroups { get; set; }

	public List<GemGroup> ShopMeleeGemGroups { get; set; }

	public string QuotePhone { get; set; }

	public string RequestForQuoteFileName { get; set; }

	public string NotesToCustomer
	{
		get
		{
			return _notesToCustomer;
		}
		set
		{
			_notesToCustomer = value;
			RaisePropertyChanged("NotesToCustomer");
		}
	}

	public string SelectedRingSize
	{
		get
		{
			return _selectedRingSize;
		}
		set
		{
			_selectedRingSize = value;
			RaisePropertyChanged(() => SelectedRingSize);
		}
	}

	public List<string> RingSizes
	{
		get
		{
			return _ringSizes;
		}
		set
		{
			_ringSizes = value;
			RaisePropertyChanged(() => RingSizes);
		}
	}

	public string SecondaryMetal
	{
		get
		{
			return _secondaryMetal;
		}
		set
		{
			if (value == _secondaryMetal)
			{
				return;
			}
			_secondaryMetal = value;
			if (!string.IsNullOrEmpty(value) && !base.IsInDesignMode)
			{
				ViewModelLocator.Current.StudioMaterialSelectionVM.SetMetal(isPrimary: false, MetalMaterialOptions.First((MaterialOptionItem _) => _.PrimaryText == value).SearchAttributeId);
				CalculateVolumeInBackground();
			}
			RaisePropertyChanged(() => SecondaryMetal);
		}
	}

	public string SecondaryMetalWeight
	{
		get
		{
			return _secondaryMetalWeight;
		}
		set
		{
			_secondaryMetalWeight = value;
			RaisePropertyChanged("SecondaryMetalWeight");
		}
	}

	public CSEnumEngraving SelectedEngraving
	{
		get
		{
			return _selectedEngraving;
		}
		set
		{
			if (value != _selectedEngraving)
			{
				ShowEngravingOptions = value == CSEnumEngraving.IncludeEngraving;
				_selectedEngraving = value;
				RaisePropertyChanged(() => SelectedEngraving);
				ResetQuote();
				ReValidateOrder();
			}
		}
	}

	public CSEnumEngravingFillColor SelectedEngravingFillColor
	{
		get
		{
			return _selectedEngravingFillColor;
		}
		set
		{
			if (value != _selectedEngravingFillColor)
			{
				_selectedEngravingFillColor = value;
				RaisePropertyChanged(() => SelectedEngravingFillColor);
				RaisePropertyChanged(() => SelectedEngraving);
				ResetQuote();
				ReValidateOrder();
			}
		}
	}

	public string SelectedEngravingFont
	{
		get
		{
			return _selectedEngravingFont;
		}
		set
		{
			if (!(value == _selectedEngravingFont))
			{
				_selectedEngravingFont = value;
				RaisePropertyChanged(() => SelectedEngravingFont);
				RaisePropertyChanged(() => SelectedEngraving);
				ResetQuote();
				ReValidateOrder();
			}
		}
	}

	public CSEnumEngravingFontsSize SelectedEngravingFontSize
	{
		get
		{
			return _selectedEngravingFontSize;
		}
		set
		{
			if (value != _selectedEngravingFontSize)
			{
				_selectedEngravingFontSize = value;
				RaisePropertyChanged(() => SelectedEngravingFontSize);
				RaisePropertyChanged(() => SelectedEngraving);
				ResetQuote();
				ReValidateOrder();
			}
		}
	}

	public CSEnumFinishing SelectedFinishing
	{
		get
		{
			return _selectedFinishing;
		}
		set
		{
			if (value != _selectedFinishing)
			{
				_selectedFinishing = value;
				RaisePropertyChanged(() => SelectedFinishing);
				switch (value)
				{
				case CSEnumFinishing.SampleModel:
					StonesEnabled = false;
					break;
				case CSEnumFinishing.SemiFinish:
					StonesEnabled = false;
					break;
				case CSEnumFinishing.Finished:
					StonesEnabled = false;
					break;
				case CSEnumFinishing.FinishedAndSet:
					StonesEnabled = true;
					break;
				}
				ResetQuote();
				ReValidateOrder();
			}
		}
	}

	public CSEnumHallmarking SelectedHallmarking
	{
		get
		{
			return _selectedHallmarking;
		}
		set
		{
			if (value != _selectedHallmarking)
			{
				_selectedHallmarking = value;
				RaisePropertyChanged(() => SelectedHallmarking);
				ResetQuote();
				ReValidateOrder();
			}
		}
	}

	public bool SelectedMoreThanARing
	{
		get
		{
			return _selectedMoreThanARing;
		}
		set
		{
			if (value != _selectedMoreThanARing)
			{
				_selectedMoreThanARing = value;
				if (value)
				{
					SelectedNoMoreThanARing = false;
					SelectedMoreThanARingDigitalOnly = false;
				}
				RaisePropertyChanged(() => SelectedMoreThanARing);
				ResetQuote();
				ReValidateOrder();
			}
		}
	}

	public bool SelectedMoreThanARingDigitalOnly
	{
		get
		{
			return _selectedMoreThanARingDigitalOnly;
		}
		set
		{
			if (value != _selectedMoreThanARingDigitalOnly)
			{
				_selectedMoreThanARingDigitalOnly = value;
				if (value)
				{
					SelectedNoMoreThanARing = false;
					SelectedMoreThanARing = false;
				}
				RaisePropertyChanged(() => SelectedMoreThanARingDigitalOnly);
				ResetQuote();
				ReValidateOrder();
			}
		}
	}

	public bool SelectedNoMoreThanARing
	{
		get
		{
			return _selectedNoMoreThanARing;
		}
		set
		{
			if (value != _selectedNoMoreThanARing)
			{
				_selectedNoMoreThanARing = value;
				if (value)
				{
					SelectedMoreThanARing = false;
					SelectedMoreThanARingDigitalOnly = false;
				}
				RaisePropertyChanged(() => SelectedNoMoreThanARing);
				ResetQuote();
				ReValidateOrder();
			}
		}
	}

	public bool ShowEngagementOptions
	{
		get
		{
			return _showEngagementOptions;
		}
		set
		{
			if (value != _showEngagementOptions)
			{
				_showEngagementOptions = value;
				RaisePropertyChanged(() => ShowEngagementOptions);
			}
		}
	}

	public bool ShowEngagementSendOptions
	{
		get
		{
			return _showEngagementSendOptions;
		}
		set
		{
			if (value != _showEngagementSendOptions)
			{
				_showEngagementSendOptions = value;
				RaisePropertyChanged(() => ShowEngagementSendOptions);
				ResetQuote();
				ReValidateOrder();
			}
		}
	}

	public bool ShowEngravingOptions
	{
		get
		{
			return _showEngravingOptions;
		}
		set
		{
			if (value != _showEngravingOptions)
			{
				_showEngravingOptions = value;
				RaisePropertyChanged(() => ShowEngravingOptions);
			}
		}
	}

	public string SideGems
	{
		get
		{
			return _sideGems;
		}
		set
		{
			if (!(value == _sideGems))
			{
				_sideGems = value;
				RaisePropertyChanged(() => SideGems);
			}
		}
	}

	public bool StonesEnabled
	{
		get
		{
			return _stonesEnabled;
		}
		set
		{
			_stonesEnabled = value;
			RaisePropertyChanged(() => StonesEnabled);
		}
	}

	public double TotalCaratWeight
	{
		get
		{
			return _totalCaratWeight;
		}
		set
		{
			_totalCaratWeight = value;
			RaisePropertyChanged("TotalCaratWeight");
		}
	}

	public bool UnderstandEngagementRingQuestion
	{
		get
		{
			return _understandEngagementRingQuestion;
		}
		set
		{
			_understandEngagementRingQuestion = value;
			RaisePropertyChanged(() => UnderstandEngagementRingQuestion);
			ResetQuote();
			ReValidateOrder();
		}
	}

	[XmlIgnore]
	public RelayCommand CommandAttachFile { get; set; }

	[XmlIgnore]
	public RelayCommand CommandClearFiles { get; set; }

	[XmlIgnore]
	public RelayCommand CommandClearDesignNotes { get; set; }

	[XmlIgnore]
	public ObservableCollection<string> AttachedFiles
	{
		get
		{
			if (_attachedFiles == null)
			{
				_attachedFiles = new ObservableCollection<string>();
			}
			return _attachedFiles;
		}
		set
		{
			_attachedFiles = value;
			RaisePropertyChanged("AttachedFiles");
		}
	}

	[XmlIgnore]
	public ImageSource DesignPreviewImage
	{
		get
		{
			return _designPreviewImage;
		}
		set
		{
			_designPreviewImage = value;
			RaisePropertyChanged("DesignPreviewImage");
		}
	}

	public string QuotedPriceString { get; set; }

	public string QuotedSettingPriceMarkedUpString { get; set; }

	public string QuotedLargeStonesPriceString { get; set; }

	[XmlIgnore]
	public WindowExportDesign ExportWindow { get; set; }

	[XmlIgnore]
	public byte[] StoneReportImageBytes { get; set; }

	[XmlIgnore]
	public bool IsMeleeSourceOptionOneVisible
	{
		get
		{
			return _isMeleeSourceOptionOneVisible;
		}
		set
		{
			_isMeleeSourceOptionOneVisible = value;
			RaisePropertyChanged("IsMeleeSourceOptionOneVisible");
		}
	}

	[XmlIgnore]
	public bool IsMeleeSourceOptionTwoVisible
	{
		get
		{
			return _isMeleeSourceOptionTwoVisible;
		}
		set
		{
			_isMeleeSourceOptionTwoVisible = value;
			RaisePropertyChanged("IsMeleeSourceOptionTwoVisible");
		}
	}

	[XmlIgnore]
	public bool IsMeleeSourceOptionThreeVisible
	{
		get
		{
			return _isMeleeSourceOptionThreeVisible;
		}
		set
		{
			_isMeleeSourceOptionThreeVisible = value;
			RaisePropertyChanged("IsMeleeSourceOptionThreeVisible");
		}
	}

	[XmlIgnore]
	public bool IsHallmarkOptionOneVisible
	{
		get
		{
			return _isHallmarkOptionOneVisible;
		}
		set
		{
			_isHallmarkOptionOneVisible = value;
			RaisePropertyChanged("IsHallmarkOptionOneVisible");
		}
	}

	[XmlIgnore]
	public bool IsHallmarkOptionTwoVisible
	{
		get
		{
			return _isHallmarkOptionTwoVisible;
		}
		set
		{
			_isHallmarkOptionTwoVisible = value;
			RaisePropertyChanged("IsHallmarkOptionTwoVisible");
		}
	}

	[XmlIgnore]
	public bool IsHallmarkOptionThreeVisible
	{
		get
		{
			return _isHallmarkOptionThreeVisible;
		}
		set
		{
			_isHallmarkOptionThreeVisible = value;
			RaisePropertyChanged("IsHallmarkOptionThreeVisible");
		}
	}

	[XmlIgnore]
	public bool IsHallmarkOptionFourVisible
	{
		get
		{
			return _isHallmarkOptionFourVisible;
		}
		set
		{
			_isHallmarkOptionFourVisible = value;
			RaisePropertyChanged("IsHallmarkOptionFourVisible");
		}
	}

	[XmlIgnore]
	public bool IsEngravingSectionVisible
	{
		get
		{
			return _isEngravingSectionVisible;
		}
		set
		{
			_isEngravingSectionVisible = value;
			RaisePropertyChanged("IsEngravingSectionVisible");
		}
	}

	[XmlIgnore]
	public bool IsHallmarkSectionVisible
	{
		get
		{
			return _isHallmarkSectionVisible;
		}
		set
		{
			_isHallmarkSectionVisible = value;
			RaisePropertyChanged("IsHallmarkSectionVisible");
		}
	}

	[XmlIgnore]
	public bool IsNoToggleVisible
	{
		get
		{
			return _isNoToggleVisible;
		}
		set
		{
			_isNoToggleVisible = value;
			RaisePropertyChanged("IsNoToggleVisible");
		}
	}

	[XmlIgnore]
	public bool IsBookAndImagesToggleVisible
	{
		get
		{
			return _isBookAndImagesToggleVisible;
		}
		set
		{
			_isBookAndImagesToggleVisible = value;
			RaisePropertyChanged("IsBookAndImagesToggleVisible");
		}
	}

	[XmlIgnore]
	public bool IsDigitalImagesToggleVisible
	{
		get
		{
			return _isDigitalImagesToggleVisible;
		}
		set
		{
			_isDigitalImagesToggleVisible = value;
			RaisePropertyChanged("IsDigitalImagesToggleVisible");
		}
	}

	[XmlIgnore]
	public bool IsMoreThanARingSectionVisible
	{
		get
		{
			return _isMoreThanARingSectionVisible;
		}
		set
		{
			_isMoreThanARingSectionVisible = value;
			RaisePropertyChanged("IsMoreThanARingSectionVisible");
		}
	}

	[XmlIgnore]
	public bool IsWaxMemoSectionVisible
	{
		get
		{
			return _isWaxMemoSectionVisible;
		}
		set
		{
			_isWaxMemoSectionVisible = value;
			RaisePropertyChanged("IsWaxMemoSectionVisible");
		}
	}

	public bool IsSupportingDocsVisible
	{
		get
		{
			return _isSupportingDocsVisible;
		}
		set
		{
			_isSupportingDocsVisible = value;
			RaisePropertyChanged("IsSupportingDocsVisible");
		}
	}

	public bool IsWaxMemoYesVisible
	{
		get
		{
			return _isWaxMemoYesVisible;
		}
		set
		{
			_isWaxMemoYesVisible = value;
			RaisePropertyChanged("IsWaxMemoYesVisible");
		}
	}

	public bool IsWaxMemoNoVisible
	{
		get
		{
			return _isWaxMemoNoVisible;
		}
		set
		{
			_isWaxMemoNoVisible = value;
			RaisePropertyChanged("IsWaxMemoNoVisible");
		}
	}

	[XmlIgnore]
	public List<GemGroup> DebugEstimateGemGroups
	{
		get
		{
			return _debugEstimateGemGroups;
		}
		set
		{
			_debugEstimateGemGroups = value;
			RaisePropertyChanged("DebugEstimateGemGroups");
		}
	}

	public bool SendSMSNotifications
	{
		get
		{
			return sendSMSNotifications;
		}
		set
		{
			sendSMSNotifications = value;
			RaisePropertyChanged("SendSMSNotifications");
		}
	}

	[XmlIgnore]
	public bool ShowDuplicateMatchingBandOption
	{
		get
		{
			return false;
		}
		set
		{
			RaisePropertyChanged("ShowDuplicateMatchingBandOption");
		}
	}

	public bool IncludeWaxMemo
	{
		get
		{
			return _includeWaxMemo;
		}
		set
		{
			_includeWaxMemo = value;
			RaisePropertyChanged("IncludeWaxMemo");
		}
	}

	public bool IncludeDuplicateMatchingBand
	{
		get
		{
			return _includeDuplicateMatchingBand;
		}
		set
		{
			_includeDuplicateMatchingBand = value;
			RaisePropertyChanged("IncludeDuplicateMatchingBand");
		}
	}

	public bool IsFinishingReadOnly
	{
		get
		{
			if (UIHelper.KeyProfile.KeyProfileId == 2 && ViewModelLocator.Current.GemReportVM.Gems.Count() > 0)
			{
				_selectedFinishing = CSEnumFinishing.FinishedAndSet;
				RaisePropertyChanged(() => SelectedFinishing);
				StonesEnabled = true;
				return true;
			}
			return false;
		}
	}

	public bool IsGroupingMeleeBySize
	{
		get
		{
			return _isGroupingMeleeBySize;
		}
		set
		{
			if (value != _isGroupingMeleeBySize)
			{
				_isGroupingMeleeBySize = value;
				RaisePropertyChanged("IsGroupingMeleeBySize");
				ViewModelLocator.Current.GemReportVM.Populate();
			}
		}
	}

	public OrderSummaryViewModel()
	{
		if (base.IsInDesignMode)
		{
			PrimaryMetal = "Platinum .950";
			SecondaryMetal = "14K Yellow Gold";
			CenterGem = "Center Gem: From Stuller; ID #25146532";
			SideGems = "Custom Per Gem";
			AccentGems = "GH, VS Diamond Accent Stones";
			BandAccentGems = "GH, VS Diamond Accent Stones";
			EngravingMessage = "Sample Engraving";
			SelectedEngraving = CSEnumEngraving.IncludeEngraving;
			IsCustomEngravingSelected = true;
			RaisePropertyChanged(() => ValidationErrors);
			RaisePropertyChanged(() => HasValidationErrors);
			IsEngagementOptionSelected = true;
			IsZalesUser = false;
			SelectedFinishing = CSEnumFinishing.FinishedAndSet;
			BuyerFirstname = "Joe";
			BuyerLastname = "Buyer";
			SelectedRingSize = "8.5";
			ModelNumber = "SCS001234-99-RND-OVF";
			QuotedLargeStonesPrice = 1111.11;
			QuotedSettingPrice = 777.77;
			QuotedPrice = 6845.0;
			TotalCaratWeight = 2.22;
			IsWaxMemoSectionVisible = true;
			IsSupportingDocsVisible = true;
			AttachedFiles = new ObservableCollection<string> { "TestFile1.txt", "TestFile2.txt" };
			AdditionalDesignNotes = "Sample Design Notes";
		}
		else
		{
			InitializeCommands();
			IsZalesUser = UIHelper.KeyProfile.KeyProfileId == 2;
			SetupVolumeWorker();
		}
	}

	public void AttachFile()
	{
		OpenFileDialog openFileDialog = new OpenFileDialog();
		if (openFileDialog.ShowDialog() == true)
		{
			AttachedFiles.Add(openFileDialog.FileName);
		}
	}

	public void ClearFiles()
	{
		AttachedFiles = new ObservableCollection<string>();
	}

	public void ClearDesignNotes()
	{
		AdditionalDesignNotes = null;
	}

	public void MarkupDiamond(ref Diamond diamond)
	{
		diamond.Price.Value = diamond.ShowcasePrice.Value;
	}

	public void NotifyCanvasChange()
	{
		ReValidateOrder();
	}

	public void Populate(bool skipVolume = false, bool skipGemShop = false)
	{
		IsDesignQuoted = false;
		UpdateControlVisibilityFromCSOptions();
		HallmarkingOptions = HallmarkingOptions;
		FinishingOptions = FinishingOptions;
		SelectedHallmarking = Enum.GetValues(typeof(CSEnumHallmarking)).Cast<CSEnumHallmarking>().FirstOrDefault((CSEnumHallmarking _) => UIHelper.CounterSketchOptions.DefaultOrderSummaryItems.Any((OrderSummaryPrefItemModel i) => i?.Title == _.GetDescription()));
		RaisePropertyChanged("ComponentsInOrder");
		ModelNumber = UIHelper.LoadedDesign.SPINumber;
		if (ViewModelBase.IsInDesignModeStatic)
		{
			NotesToCustomer = "Design has been edited by Gemvision.";
		}
		if (ViewModelBase.IsInDesignModeStatic)
		{
			return;
		}
		NotesToCustomer = UIHelper.CoreDataService.GetNotesToCustomerForCurrentDesign();
		PopulateRingSize();
		UIHelper.CoreDataService.MaterialsGetMetal(delegate(MetalMaterial[] metals, Exception arg)
		{
			_metals = metals;
		});
		_primaryMetal = MetalHelper.GetMetalFriendlyNameFromMaterial(_metals[0]);
		RaisePropertyChanged(() => PrimaryMetal);
		_secondaryMetal = MetalHelper.GetMetalFriendlyNameFromMaterial(_metals[1]);
		RaisePropertyChanged(() => SecondaryMetal);
		if (!UIHelper.IsOrderedDesignLoaded)
		{
			ResetQuote();
		}
		ShowDuplicateMatchingBandOption = false;
		if (ViewModelLocator.Current.DesignStackVM.DesignComponents.Any((DesignComponentDisplay dc) => dc.Name == "Band1" && dc.IsEnabled))
		{
			ShowDuplicateMatchingBandOption = true;
		}
		if (ViewModelLocator.Current.DesignStackVM.DesignComponents.Any((DesignComponentDisplay dc) => dc.Name == "Band2" && dc.IsEnabled))
		{
			ShowDuplicateMatchingBandOption = false;
		}
		IsSecondaryMetalIncluded = UIHelper.CoreDataService.AreSecondaryMetalObjectsVisible();
		if (!UIHelper.IsOrderedDesignLoaded)
		{
			ViewModelLocator.Current.GemReportVM.Populate();
		}
		TotalCaratWeight = ViewModelLocator.Current.GemReportVM.LargeGems.Sum((Gem _) => _.Carat) + ViewModelLocator.Current.GemReportVM.Gems.Sum((Gem _) => _.Carat);
		if (!UIHelper.IsOrderedDesignLoaded)
		{
			if (ViewModelLocator.Current.GemReportVM.LargeGems.Count == 0 && ViewModelLocator.Current.GemReportVM.Gems.Count == 0)
			{
				SelectedFinishing = CSEnumFinishing.Finished;
			}
			else
			{
				SelectedFinishing = CSEnumFinishing.FinishedAndSet;
			}
			if (!skipGemShop)
			{
				ShopNonSerializedGems();
			}
			if (!skipVolume)
			{
				CalculateVolumeInBackground();
			}
			PopulateCustomerInfoFromPortfolio();
			RaisePropertyChanged(() => Requires24HourQuote);
			ReValidateOrder();
			IsSupportingDocsVisible = !SecurityHelper.IsStreaming;
		}
	}

	public void PopulateRingSize()
	{
		Application.Current.Dispatcher.Invoke(delegate
		{
			UIHelper.CoreDataService.OrderGetCurrentRingSizes(delegate(List<string> ringSizes)
			{
				if (ringSizes.Any())
				{
					RingSizes = ringSizes;
				}
				else
				{
					RingSizes = new List<string> { "n/a" };
				}
			});
		});
	}

	public void PrintQuote()
	{
	}

	public void Reset(bool skipVolume = false, bool skipGemShop = false)
	{
		CamJobResponseResult = null;
		AccentGems = string.Empty;
		AdditionalDesignNotes = string.Empty;
		BandAccentGems = string.Empty;
		CenterGem = string.Empty;
		CustomerAddress1 = string.Empty;
		CustomerAddress2 = string.Empty;
		CustomerCity = string.Empty;
		CustomerCountry = string.Empty;
		CustomerEmail = string.Empty;
		CustomerFirstName = string.Empty;
		CustomerLastName = string.Empty;
		CustomerMiddleInitial = string.Empty;
		CustomerPhone1 = string.Empty;
		CustomerPhone2 = string.Empty;
		CustomerPostalCode = string.Empty;
		CustomerReferenceNumber = string.Empty;
		CustomerState = string.Empty;
		CustomerSuffix = string.Empty;
		CustomerTitle = string.Empty;
		EngravingMessage = string.Empty;
		IsCustomEngravingSelected = false;
		IsEngagementOptionSelected = false;
		OrderReference = string.Empty;
		QuoteEMail = string.Empty;
		QuotePhone = string.Empty;
		QuotedDesignPath = string.Empty;
		QuotedPrice = 0.0;
		RequestForQuoteFileName = string.Empty;
		SelectedEngraving = CSEnumEngraving.NoEngraving;
		SelectedEngravingFillColor = CSEnumEngravingFillColor.None;
		SelectedEngravingFont = "Bookman Old Style";
		SelectedEngravingFontSize = CSEnumEngravingFontsSize.Large;
		SelectedFinishing = CSEnumFinishing.FinishedAndSet;
		SelectedHallmarking = CSEnumHallmarking.None;
		SelectedMoreThanARing = false;
		SelectedMoreThanARingDigitalOnly = false;
		SelectedNoMoreThanARing = true;
		ShowEngagementOptions = false;
		ShowEngagementSendOptions = false;
		ShowEngravingOptions = false;
		SideGems = SideGems;
		IncludeDuplicateMatchingBand = false;
		AttachedFiles = new ObservableCollection<string>();
		CamJobResponseResult = null;
		Populate(skipVolume, skipGemShop);
		ViewModelLocator.Current.GemReportVM.Populate();
		RaisePropertyChanged(() => ValidationErrors);
		IsSupportingDocsVisible = !SecurityHelper.IsStreaming;
	}

	public void ResetQuote(bool clearGemErrors = true)
	{
		IsDesignQuoted = false;
		QuotedCamAssemblyPrice = 0m;
		QuotedCamPrice = 0m;
		QuotedCastings = 0m;
		QuotedGemsSettingPrice = 0m;
		QuotedInsideEngravingPrice = 0m;
		QuotedMeleeStonesPrice = 0.0;
		QuotedMetalPrice = 0m;
		QuotedMoreThanARingPrice = 0m;
		QuotedOutsideEngravingPrice = 0m;
		QuotedPolishPrice = 0m;
		QuotedRingSizePrice = 0m;
		QuotedSettingPrice = 0.0;
		QuotedSpecialFinishPrice = 0m;
		QuotedLargeStonesPrice = 0.0;
		QuotedSettingPriceMarkedUp = 0.0;
		QuotedPrice = 0.0;
		if (!base.IsInDesignMode)
		{
			CamJobResponseResult = null;
		}
		if (clearGemErrors)
		{
			_gemOrderingErrorList.Clear();
		}
	}

	public void ReValidateOrder()
	{
		if (!UIHelper.IsOrderedDesignLoaded)
		{
			RaisePropertyChanged(() => Requires24HourQuote);
			RaisePropertyChanged(() => IsFinishingReadOnly);
			ValidateStoneAssignments();
			RaisePropertyChanged(() => ValidationErrors);
			RaisePropertyChanged(() => IsDesignQuotable);
		}
	}

	public void ShopNonSerializedGems()
	{
		DebuggingHelper.PrintTimedOutput("ShopForGems");
		ViewModelLocator.Current.GemReportVM.GemGroups.ToList().ForEach(delegate(GemGroup _)
		{
			_.StullerId = null;
		});
		ViewModelLocator.Current.GemReportVM.Gems.ToList().ForEach(delegate(Gem _)
		{
			_.StullerId = null;
		});
		ViewModelLocator.Current.GemReportVM.LargeGems.ToList().ForEach(delegate(Gem _)
		{
			_.StullerId = null;
		});
		if (ViewModelLocator.Current.GemReportVM.GemGroups.Count > 0)
		{
			ShopMeleeGemGroups = new List<GemGroup>(ViewModelLocator.Current.GemReportVM.Gems.GroupBy((Gem gemKey) => new
			{
				gemKey.Material,
				gemKey.Shape,
				gemKey.Width,
				gemKey.Height,
				ViewModelLocator.Current.GemReportVM.GemGroups.FirstOrDefault((GemGroup _) => _.GemNumbers.Contains(gemKey.GemNumber)).StoneQuality,
				gemKey.Source,
				gemKey.SetType
			}, (key, IEnumerable<Gem> gems) =>
			{
				Gem[] array = (gems as Gem[]) ?? gems.ToArray();
				return new GemGroup
				{
					Material = key.Material,
					Shape = key.Shape,
					Width = key.Width,
					Height = key.Height,
					Quantity = array.Length,
					StoneQuality = key.StoneQuality,
					GemsInGroup = new List<Gem>(array),
					GemNumbers = new List<string>(array.Select((Gem g) => g.GemNumber)),
					Source = key.Source,
					SetType = key.SetType,
					GemMountingType = array.First().GemMountingType,
					StullerImage = array.FirstOrDefault()?.StullerImage
				};
			}));
		}
		else
		{
			ShopMeleeGemGroups = new List<GemGroup>();
		}
		ShopLargeGemGroups = new List<GemGroup>(ViewModelLocator.Current.GemReportVM.LargeGems.Where((Gem _) => !_.IsRedBox).GroupBy((Gem gemKey) => new { gemKey.Material, gemKey.Shape, gemKey.Width, gemKey.Height, gemKey.StoneQuality, gemKey.Source, gemKey.SetType }, (key, IEnumerable<Gem> gems) =>
		{
			Gem[] array = (gems as Gem[]) ?? gems.ToArray();
			return new GemGroup
			{
				Material = key.Material,
				Shape = key.Shape,
				Width = key.Width,
				Height = key.Height,
				Quantity = array.Length,
				StoneQuality = key.StoneQuality,
				GemsInGroup = new List<Gem>(array),
				GemNumbers = new List<string>(array.Select((Gem g) => g.GemNumber)),
				Source = key.Source,
				SetType = key.SetType,
				GemMountingType = array.First().GemMountingType
			};
		}));
		ShopMeleeGemGroups.ForEach(ShopGemGroup);
		ShopLargeGemGroups.ForEach(ShopGemGroup);
		ViewModelLocator.Current.GemReportVM.GemGroups.ToList().ForEach(delegate(GemGroup _)
		{
			_.IsNotInStock = _.GemsInGroup.Any((Gem gem) => ShopMeleeGemGroups.Any((GemGroup q) => q.IsNotInStock && q.GemNumbers.Contains(gem.GemNumber)));
			List<string> agtaInfo = new List<string>();
			ShopMeleeGemGroups.ForEach(delegate(GemGroup gg)
			{
				if (gg.GemNumbers.Any((string gn) => _.GemNumbers.Any((string text) => gn == text)))
				{
					agtaInfo.Add(gg.AgtaInformation);
				}
			});
			_.AgtaInformation = string.Join(", ", agtaInfo.Distinct());
			List<string> partNumbers = new List<string>();
			ShopMeleeGemGroups.ForEach(delegate(GemGroup gg)
			{
				if (gg.GemNumbers.Any((string gn) => _.GemNumbers.Any((string text) => gn == text)))
				{
					partNumbers.Add(gg.StullerId);
				}
			});
			_.StullerId = string.Join(", ", partNumbers.Distinct());
			List<string> descriptions = new List<string>();
			ShopMeleeGemGroups.ForEach(delegate(GemGroup gg)
			{
				if (_.IsNotInStock)
				{
					_.StullerImage = null;
				}
				else if (gg.GemNumbers.Any((string gn) => _.GemNumbers.Any((string text) => gn == text)))
				{
					_.StullerImage = gg.StullerImage;
					descriptions.Add(gg.StullerDescription);
				}
			});
			_.StullerDescription = string.Join(", ", descriptions.Distinct());
			_.HasCheckedInventory = true;
			_.ForceValidation();
		});
		ViewModelLocator.Current.GemReportVM.LargeGems.ToList().ForEach(delegate(Gem gem)
		{
			GemGroup gemGroup = ShopLargeGemGroups.FirstOrDefault((GemGroup g) => g.GemNumbers.Contains(gem.GemNumber));
			if (gemGroup != null)
			{
				gem.IsNotInStock = gemGroup.IsNotInStock;
				gem.AgtaInformation = gemGroup.AgtaInformation;
				gem.StullerId = gemGroup.StullerId;
				if (gem.IsNotInStock)
				{
					gem.StullerImage = null;
					gem.StullerDescription = null;
				}
				else
				{
					gem.StullerImage = gemGroup.StullerImage;
					gem.StullerDescription = gemGroup.StullerDescription;
				}
			}
			gem.ForceValidation();
		});
		ViewModelLocator.Current.PreferenceOrderScreenVM.IsCacheInvalid = false;
	}

	public void UpdateFromUserData(OrderSummaryViewModel orderSummaryViewModel)
	{
		AccentGems = orderSummaryViewModel.AccentGems;
		AdditionalDesignNotes = orderSummaryViewModel.AdditionalDesignNotes;
		BandAccentGems = orderSummaryViewModel.BandAccentGems;
		CenterGem = orderSummaryViewModel.CenterGem;
		CustomerAddress1 = orderSummaryViewModel.CustomerAddress1;
		CustomerAddress2 = orderSummaryViewModel.CustomerAddress2;
		CustomerCity = orderSummaryViewModel.CustomerCity;
		CustomerCountry = orderSummaryViewModel.CustomerCountry;
		CustomerEmail = orderSummaryViewModel.CustomerEmail;
		CustomerFirstName = orderSummaryViewModel.CustomerFirstName;
		CustomerLastName = orderSummaryViewModel.CustomerLastName;
		CustomerMiddleInitial = orderSummaryViewModel.CustomerMiddleInitial;
		CustomerPhone1 = orderSummaryViewModel.CustomerPhone1;
		CustomerPhone2 = orderSummaryViewModel.CustomerPhone2;
		CustomerPostalCode = orderSummaryViewModel.CustomerPostalCode;
		CustomerReferenceNumber = orderSummaryViewModel.CustomerReferenceNumber;
		CustomerState = orderSummaryViewModel.CustomerState;
		CustomerSuffix = orderSummaryViewModel.CustomerSuffix;
		CustomerTitle = orderSummaryViewModel.CustomerTitle;
		EngravingMessage = orderSummaryViewModel.EngravingMessage;
		IsCustomEngravingSelected = orderSummaryViewModel.IsCustomEngravingSelected;
		IsEngagementOptionSelected = orderSummaryViewModel.IsEngagementOptionSelected;
		OrderReference = orderSummaryViewModel.OrderReference;
		PrimaryMetal = orderSummaryViewModel.PrimaryMetal;
		QuoteEMail = orderSummaryViewModel.QuoteEMail;
		QuotePhone = orderSummaryViewModel.QuotePhone;
		QuotedDesignPath = orderSummaryViewModel.QuotedDesignPath;
		QuotedPrice = orderSummaryViewModel.QuotedPrice;
		RequestForQuoteFileName = orderSummaryViewModel.RequestForQuoteFileName;
		SecondaryMetal = orderSummaryViewModel.SecondaryMetal;
		SelectedEngraving = orderSummaryViewModel.SelectedEngraving;
		SelectedEngravingFillColor = orderSummaryViewModel.SelectedEngravingFillColor;
		SelectedEngravingFont = orderSummaryViewModel.SelectedEngravingFont;
		SelectedEngravingFontSize = orderSummaryViewModel.SelectedEngravingFontSize;
		SelectedFinishing = orderSummaryViewModel.SelectedFinishing;
		SelectedHallmarking = orderSummaryViewModel.SelectedHallmarking;
		SelectedMoreThanARing = orderSummaryViewModel.SelectedMoreThanARing;
		SelectedMoreThanARingDigitalOnly = orderSummaryViewModel.SelectedMoreThanARingDigitalOnly;
		ShowEngagementOptions = orderSummaryViewModel.ShowEngagementOptions;
		ShowEngagementSendOptions = orderSummaryViewModel.ShowEngagementSendOptions;
		ShowEngravingOptions = orderSummaryViewModel.ShowEngravingOptions;
		SideGems = orderSummaryViewModel.SideGems;
		PrimaryMetalWeight = orderSummaryViewModel.PrimaryMetalWeight;
		SecondaryMetalWeight = orderSummaryViewModel.SecondaryMetalWeight;
		QuotedCamAssemblyPrice = orderSummaryViewModel.QuotedCamAssemblyPrice;
		QuotedCamPrice = orderSummaryViewModel.QuotedCamPrice;
		QuotedCastings = orderSummaryViewModel.QuotedCastings;
		QuotedGemsSettingPrice = orderSummaryViewModel.QuotedGemsSettingPrice;
		QuotedInsideEngravingPrice = orderSummaryViewModel.QuotedInsideEngravingPrice;
		QuotedLargeStonesPrice = orderSummaryViewModel.QuotedLargeStonesPrice;
		QuotedLargeStonesPriceString = orderSummaryViewModel.QuotedLargeStonesPriceString;
		QuotedMeleeStonesPrice = orderSummaryViewModel.QuotedMeleeStonesPrice;
		QuotedMetalPrice = orderSummaryViewModel.QuotedMetalPrice;
		QuotedMoreThanARingPrice = orderSummaryViewModel.QuotedMoreThanARingPrice;
		QuotedOutsideEngravingPrice = orderSummaryViewModel.QuotedOutsideEngravingPrice;
		QuotedPolishPrice = orderSummaryViewModel.QuotedPolishPrice;
		QuotedPriceString = orderSummaryViewModel.QuotedPriceString;
		QuotedSettingPrice = orderSummaryViewModel.QuotedSettingPrice;
		QuotedSettingPriceMarkedUp = orderSummaryViewModel.QuotedSettingPriceMarkedUp;
		QuotedSettingPriceMarkedUpString = orderSummaryViewModel.QuotedSettingPriceMarkedUpString;
		QuotedSpecialFinishPrice = orderSummaryViewModel.QuotedSpecialFinishPrice;
		CamJobResponseResult = orderSummaryViewModel.CamJobResponseResult;
		SendSMSNotifications = orderSummaryViewModel.sendSMSNotifications;
	}

	private void _backgroundVolumeWorker_DoWork(object sender, DoWorkEventArgs e)
	{
		e.Result = DoVolumeCalculation();
	}

	private void _backgroundVolumeWorker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
	{
		CalculatedVolumes = e.Result as Dictionary<Tuple<ComponentsEnum, MaterialClassification>, double>;
		if (CalculatedVolumes != null)
		{
			UpdateWeightsFromVolumes();
		}
		ReValidateOrder();
		UIHelper.ShowWaitWindow(waitVisible: false);
	}

	private void AddDesignDoc(CamJob camJob, string pathToDesignFile, string fileName)
	{
		byte[] array = null;
		if (File.Exists(pathToDesignFile))
		{
			using FileStream fileStream = new FileStream(pathToDesignFile, FileMode.Open, FileAccess.Read);
			array = new byte[fileStream.Length];
			fileStream.Read(array, 0, Convert.ToInt32(fileStream.Length));
			fileStream.Close();
		}
		if (array != null)
		{
			CamJobFile item = new CamJobFile
			{
				CamJobFileType = CamJobFileType.Cad,
				FileName = fileName,
				CamJobDownloadType = CamJobDownloadType.Content,
				FileContent = Convert.ToBase64String(array)
			};
			if (camJob.CamJobFiles == null)
			{
				camJob.CamJobFiles = new List<CamJobFile>();
			}
			camJob.CamJobFiles.Add(item);
		}
	}

	private void AddSupportingDocs(CamJob camJob)
	{
		System.Drawing.Size captureSize = new System.Drawing.Size(1024, 768);
		Messenger.Default.Send<string, WindowOrderConfirmationPrompt>("CapturePricingImage");
		CamJobFile item = new CamJobFile
		{
			CamJobFileType = CamJobFileType.Support,
			CamJobDownloadType = CamJobDownloadType.Content,
			FileName = "PricingImage.png",
			FileContent = Convert.ToBase64String(PricingImageBytes)
		};
		camJob.CamJobFiles.Add(item);
		File.WriteAllBytes(Path.Combine(UIHelper.CSQuotesPath, "OrderPricing_" + Path.GetFileNameWithoutExtension(RequestForQuoteFileName) + ".png"), PricingImageBytes);
		Messenger.Default.Send<string, OrderSummaryView>("CaptureStoneReportImage");
		CamJobFile item2 = new CamJobFile
		{
			CamJobFileType = CamJobFileType.Support,
			CamJobDownloadType = CamJobDownloadType.Content,
			FileName = "StoneReportImage.png",
			FileContent = Convert.ToBase64String(StoneReportImageBytes)
		};
		camJob.CamJobFiles.Add(item2);
		CamJobFile item3 = new CamJobFile
		{
			CamJobFileType = CamJobFileType.Support,
			CamJobDownloadType = CamJobDownloadType.Content,
			FileName = "ViewPort-Perspective.png",
			FileContent = Convert.ToBase64String(WpfHelper.BitmapToPngByte(UIHelper.CoreDataService.RequestScreenCaptureDefault(captureSize, zoom: false, 0.0)))
		};
		camJob.CamJobFiles.Add(item3);
		CamJobFile item4 = new CamJobFile
		{
			CamJobFileType = CamJobFileType.Support,
			CamJobDownloadType = CamJobDownloadType.Content,
			FileName = "ViewPort-Front.png",
			FileContent = Convert.ToBase64String(WpfHelper.BitmapToPngByte(UIHelper.CoreDataService.RequestScreenCaptureFront(captureSize)))
		};
		camJob.CamJobFiles.Add(item4);
		CamJobFile item5 = new CamJobFile
		{
			CamJobFileType = CamJobFileType.Support,
			CamJobDownloadType = CamJobDownloadType.Content,
			FileName = "ViewPort-Side.png",
			FileContent = Convert.ToBase64String(WpfHelper.BitmapToPngByte(UIHelper.CoreDataService.RequestScreenCaptureSide(captureSize)))
		};
		camJob.CamJobFiles.Add(item5);
		CamJobFile item6 = new CamJobFile
		{
			CamJobFileType = CamJobFileType.Support,
			CamJobDownloadType = CamJobDownloadType.Content,
			FileName = "ViewPort-Top.png",
			FileContent = Convert.ToBase64String(WpfHelper.BitmapToPngByte(UIHelper.CoreDataService.RequestScreenCaptureTop(captureSize)))
		};
		camJob.CamJobFiles.Add(item6);
		AttachedFiles.ToList().ForEach(delegate(string path)
		{
			CamJobFile item7 = new CamJobFile
			{
				CamJobFileType = CamJobFileType.Support,
				CamJobDownloadType = CamJobDownloadType.Content,
				FileName = Path.GetFileName(path),
				FileContent = Convert.ToBase64String(File.ReadAllBytes(path))
			};
			camJob.CamJobFiles.Add(item7);
		});
	}

	private double CalculateTotalVolumeMetalPerClassification(MaterialClassification whichMaterialClassification, Dictionary<Tuple<ComponentsEnum, MaterialClassification>, double> currentVolumeList)
	{
		if (currentVolumeList.Count > 0)
		{
			return currentVolumeList.Keys.Where((Tuple<ComponentsEnum, MaterialClassification> key) => key.Item2 == whichMaterialClassification).Sum((Tuple<ComponentsEnum, MaterialClassification> key) => currentVolumeList[key]);
		}
		return 0.0;
	}

	private void CalculateVolume()
	{
		ResetQuote();
		PrimaryMetalWeight = " -- " + Strings.Calculating + " --";
		SecondaryMetalWeight = " -- " + Strings.Calculating + " --";
		PennyWeightMetalPrimary = 0m;
		PennyWeightMetalSecondary = 0m;
		Dictionary<Tuple<ComponentsEnum, MaterialClassification>, double> volumes = new Dictionary<Tuple<ComponentsEnum, MaterialClassification>, double>();
		Task.Factory.StartNew(() => volumes = DoVolumeCalculation()).Wait();
		CalculatedVolumes = volumes;
		UpdateWeightsFromVolumes();
	}

	private void CalculateVolumeInBackground()
	{
		if (!BackgroundVolumeWorker.IsBusy && !UIHelper.IsOrderedDesignLoaded)
		{
			UIHelper.ShowWaitWindow(waitVisible: true, Strings.CalculatingMetalWeight);
			ResetQuote(clearGemErrors: false);
			PrimaryMetalWeight = " -- " + Strings.Calculating + " --";
			SecondaryMetalWeight = " -- " + Strings.Calculating + " --";
			PennyWeightMetalPrimary = 0m;
			PennyWeightMetalSecondary = 0m;
			BackgroundVolumeWorker.RunWorkerAsync();
		}
	}

	private void CloseDesignNotesWindow()
	{
		if (_windowDesignNotes != null)
		{
			_windowDesignNotes.Close();
		}
	}

	private Dictionary<Tuple<ComponentsEnum, MaterialClassification>, double> DoVolumeCalculation()
	{
		if (UIHelper.IsOrderedDesignLoaded)
		{
			return null;
		}
		int singleMetalCastingCount;
		bool hasSingleMetalCastingCount = CastingHelper.TryGetSingleMetalCastingCount(UIHelper.LoadedDesign.DesignId, out singleMetalCastingCount);
		DebuggingHelper.PrintTimedOutput("Calling Voxel Volume Logic...");
		Dictionary<Tuple<ComponentsEnum, MaterialClassification>, double> metalVolumesPerComponent = UIHelper.CoreDataService.GetMetalVolumesPerComponent(hasSingleMetalCastingCount, singleMetalCastingCount);
		if (metalVolumesPerComponent.Count > 0)
		{
			DebuggingHelper.PrintTimedOutput("Voxel Volume Logic Returned:");
			metalVolumesPerComponent.ForEach(delegate(KeyValuePair<Tuple<ComponentsEnum, MaterialClassification>, double> volume)
			{
				DebuggingHelper.PrintTimedOutput($"Volume Result: {volume.Key.Item1} : {volume.Key.Item2} = {volume.Value}");
			});
		}
		else
		{
			DebuggingHelper.PrintTimedOutput("Voxel Volume Logic Returned.");
		}
		return metalVolumesPerComponent;
	}

	private void ExportSTL3DM()
	{
		if (UIHelper.IsInternetAccessible() && ExportWindow == null)
		{
			ViewModelLocator.Current.WindowExportDesignVM.Populate();
			ExportWindow = new WindowExportDesign
			{
				Owner = Application.Current.MainWindow,
				WindowStartupLocation = WindowStartupLocation.CenterOwner
			};
			ExportWindow.Show();
		}
	}

	private int GetNumberOfCastingsFromVisibleComponent(ComponentsEnum whichComponent)
	{
		switch (whichComponent)
		{
		case ComponentsEnum.Ring:
		case ComponentsEnum.Pendant:
			if (UIHelper.CoreDataService.AreSecondaryMetalObjectsVisibleForComponent(whichComponent))
			{
				if (PrimaryMetal == SecondaryMetal)
				{
					if (CastingHelper.TryGetSingleMetalCastingCount(UIHelper.LoadedDesign.DesignId, out var singleMetalCastingCount))
					{
						return singleMetalCastingCount;
					}
					return 1;
				}
				return 2;
			}
			return 1;
		case ComponentsEnum.Band1:
		{
			int num = 0;
			num = ((!UIHelper.CoreDataService.AreSecondaryMetalObjectsVisibleForComponent(whichComponent)) ? 1 : 2);
			if (IncludeDuplicateMatchingBand)
			{
				num++;
			}
			return num;
		}
		case ComponentsEnum.Band2:
			if (UIHelper.CoreDataService.AreSecondaryMetalObjectsVisibleForComponent(whichComponent))
			{
				return 2;
			}
			return 1;
		default:
			GlobalErrorHandler.WriteInfoToEventLog($"Invalid ComponentEnum type of {whichComponent}", 2);
			return 0;
		}
	}

	private int GetNumberOfCastingsFromVisibleComponets()
	{
		int num = 0;
		foreach (ComponentsEnum visibleComponent in UIHelper.VisibleComponents)
		{
			int numberOfCastingsFromVisibleComponent = GetNumberOfCastingsFromVisibleComponent(visibleComponent);
			GlobalErrorHandler.WriteInfoToEventLog($"Component type {visibleComponent} has {numberOfCastingsFromVisibleComponent} castings", 2);
			num += numberOfCastingsFromVisibleComponent;
		}
		QuotedCastings = num;
		GlobalErrorHandler.WriteInfoToEventLog($"Total castings: {num}", 2);
		return num;
	}

	private void InitializeCommands()
	{
		CommandOpenMoreThanARingPreview = new RelayCommand(OpenMoreThanARingPreview);
		CommandPrintQuote = new RelayCommand(PrintQuote);
		CommandOpenDesignNotesWindow = new RelayCommand(OpenDesignNotesWindow);
		CommandCloseDesignNotesWindow = new RelayCommand(CloseDesignNotesWindow);
		CommandQuoteSubmit = new RelayCommand(SendQuote);
		CommandExportSTL3DM = new RelayCommand(ExportSTL3DM);
		CommandAttachFile = new RelayCommand(AttachFile);
		CommandClearFiles = new RelayCommand(ClearFiles);
		CommandClearDesignNotes = new RelayCommand(ClearDesignNotes);
	}

	private void OpenDesignNotesWindow()
	{
		_windowDesignNotes = new WindowDesignNotes
		{
			Owner = Application.Current.MainWindow,
			WindowStartupLocation = WindowStartupLocation.CenterOwner
		};
		_windowDesignNotes.Show();
	}

	private void OpenMoreThanARingPreview()
	{
		WindowMoreThanARingPreview windowMoreThanARingPreview = new WindowMoreThanARingPreview();
		windowMoreThanARingPreview.Owner = Application.Current.MainWindow;
		windowMoreThanARingPreview.ShowDialog();
	}

	private void PopulateCustomerInfoFromPortfolio()
	{
		PortfolioManagerViewModel portfolioManagerVM = ViewModelLocator.Current.PortfolioManagerVM;
		CustomerFirstName = portfolioManagerVM.FirstName;
		CustomerLastName = portfolioManagerVM.LastName;
		CustomerAddress1 = portfolioManagerVM.Address1;
		CustomerAddress2 = portfolioManagerVM.Address2;
		CustomerCity = portfolioManagerVM.City;
		CustomerState = portfolioManagerVM.State;
		CustomerCountry = portfolioManagerVM.Country;
		CustomerPostalCode = portfolioManagerVM.Zip;
		CustomerPhone1 = portfolioManagerVM.PhoneNumber;
		CustomerEmail = portfolioManagerVM.Email;
	}

	private async void SendOrder()
	{
		DesignPreviewImage = ImageHelper.ConvertBitmapToBitmapImage(UIHelper.CoreDataService.RequestScreenCaptureDefault(new System.Drawing.Size(280, 270), zoom: true, 0.0));
		if (UIHelper.KeyProfile.BuyerInfoDefaultsEmpty)
		{
			BuyerFirstname = string.Empty;
			BuyerLastname = string.Empty;
			BuyerEMail = string.Empty;
		}
		else
		{
			BuyerFirstname = UIHelper.CounterSketchOptions.BuyerFirstName;
			BuyerLastname = UIHelper.CounterSketchOptions.BuyerLastName;
			BuyerEMail = UIHelper.CounterSketchOptions.BuyerEMail;
		}
		if (string.IsNullOrWhiteSpace(BuyerEMail))
		{
			BuyerEMail = QuoteEMail;
		}
		if (SecurityHelper.IsStreaming)
		{
			BuyerFirstname = SecurityHelper.UserName;
		}
		IsFreehandQuoteExempt = UIHelper.CounterSketchOptions.AllowFreezePriceForFreehandParts;
		IsUsingStullerShowcaseMarkups = true;
		WindowOrderConfirmationPrompt windowOrderConfirmationPrompt = new WindowOrderConfirmationPrompt();
		windowOrderConfirmationPrompt.Owner = Application.Current.MainWindow;
		windowOrderConfirmationPrompt.ShowDialog();
		if (!windowOrderConfirmationPrompt.ProceedWithOrder)
		{
			IsBusy = false;
			return;
		}
		CamJobResponseResult.CamJob.CustomerReference = CustomerReferenceNumber;
		CamJobResponseResult.CamJob.Comments = AdditionalDesignNotes;
		CamJobResponseResult.CamJob.TotalRetailPrice = (decimal)QuotedPrice;
		UIHelper.CounterSketchOptions.BuyerFirstName = BuyerFirstname;
		UIHelper.CounterSketchOptions.BuyerLastName = BuyerLastname;
		UIHelper.CounterSketchOptions.BuyerEMail = BuyerEMail;
		UIHelper.CounterSketchOptions.WriteSettings();
		CamJobResponseResult.CamJob.BuyerFirstName = BuyerFirstname;
		CamJobResponseResult.CamJob.BuyerLastName = BuyerLastname;
		CamJobResponseResult.CamJob.EmailAddress = BuyerEMail;
		CamJobResponseResult.CamJob.CustomerReference = CustomerReferenceNumber;
		OrderHelper.SerializeOrderInformationIntoDesignUserData();
		RequestForQuoteFileName = string.Concat(Guid.NewGuid(), ".csg");
		string cSQuotesPath = UIHelper.CSQuotesPath;
		if (!Directory.Exists(cSQuotesPath))
		{
			Directory.CreateDirectory(cSQuotesPath);
		}
		QuotedDesignPath = Path.Combine(cSQuotesPath, RequestForQuoteFileName);
		UIHelper.CoreDataService.DocumentSetFilename(RequestForQuoteFileName);
		UIHelper.CoreDataService.DocumentWrite(QuotedDesignPath);
		AddDesignDoc(CamJobResponseResult.CamJob, QuotedDesignPath, RequestForQuoteFileName);
		AddSupportingDocs(CamJobResponseResult.CamJob);
		UIHelper.ShowWaitWindow(waitVisible: true);
		try
		{
			await Task.Factory.StartNew(delegate
			{
				if (Requires24HourQuote)
				{
					GlobalErrorHandler.WriteInfoToEventLog("Calling Send24HourOrder.", 2);
					CamJobResponseResult = StullerCamAPIHelper.Send24HourOrder(CamJobResponseResult.CamJob);
				}
				else
				{
					GlobalErrorHandler.WriteInfoToEventLog("Calling SendOrder.", 2);
					CamJobResponseResult = StullerCamAPIHelper.SendOrder(CamJobResponseResult.CamJob);
				}
			});
			if (CamJobResponseResult != null && CamJobResponseResult.CamJobErrors != null && CamJobResponseResult.CamJobErrors.Any())
			{
				_gemOrderingErrorList.AddRange(CamJobResponseResult.CamJobErrors.Select((CamJobError _) => _.Message));
				RaisePropertyChanged(() => ValidationErrors);
				RaisePropertyChanged(() => HasValidationErrors);
				if (HasValidationErrors)
				{
					IsDesignQuoted = false;
					IsBusy = false;
				}
				if (Requires24HourQuote)
				{
					GlobalErrorHandler.WriteInfoToEventLog("Called Send 24 Hour Order, got the following Errors:" + CamJobResponseResult.CamJobErrors.Select((CamJobError _) => _.Message).Aggregate((string current, string next) => current + "," + next));
				}
				else
				{
					GlobalErrorHandler.WriteInfoToEventLog("Called Send Order, got the following Errors:" + CamJobResponseResult.CamJobErrors.Select((CamJobError _) => _.Message).Aggregate((string current, string next) => current + "," + next));
				}
				UIHelper.ShowWaitWindow(waitVisible: false);
				IsBusy = false;
				return;
			}
		}
		finally
		{
			IsDesignQuoted = false;
			DispatcherHelper.WaitForPriority();
		}
		if (CamJobResponseResult != null && CamJobResponseResult.CamJob != null)
		{
			if (IsCustomEngravingSelected)
			{
				EventHelper.RecordEvent(EventHelper.EventTypes.Order_Engraving, "DesignId", UIHelper.LoadedDesign.DesignId.ToString(CultureInfo.InvariantCulture));
			}
			if (SelectedMoreThanARing)
			{
				EventHelper.RecordEvent(EventHelper.EventTypes.Order_MoreThanARingBook, "DesignId", UIHelper.LoadedDesign.DesignId.ToString(CultureInfo.InvariantCulture));
			}
			if (SelectedMoreThanARingDigitalOnly)
			{
				EventHelper.RecordEvent(EventHelper.EventTypes.Order_MoreThanARingDigital, "DesignId", UIHelper.LoadedDesign.DesignId.ToString(CultureInfo.InvariantCulture));
			}
			using (CSApplicationDAL cSApplicationDAL = new CSApplicationDAL())
			{
				cSApplicationDAL.CreateQuote(BuyerFirstname, BuyerLastname, BuyerEMail, QuotePhone, AdditionalDesignNotes, QuotedDesignPath, DateTime.Now, CamJobResponseResult.CamJob.CamJobID.GetValueOrDefault(), CustomerReferenceNumber, CustomerFirstName, CustomerLastName, CustomerAddress1, CustomerAddress2, CustomerCity, CustomerState, CustomerPostalCode, CustomerPhone1, AdditionalDesignNotes);
			}
			UIHelper.ShowWaitWindow(waitVisible: false);
			if (CamJobResponseResult != null && CamJobResponseResult.CamJob.CamJobID.GetValueOrDefault() > 0)
			{
				ViewModelLocator.Current.WindowOrderResultVM.Populate(CamJobResponseResult.CamJob.CamJobID.GetValueOrDefault(), CustomerWillSupplyStonesForSetting);
				WindowOrderResultView windowOrderResultView = new WindowOrderResultView();
				windowOrderResultView.Owner = Application.Current.MainWindow;
				windowOrderResultView.ShowDialog();
				OrderHelper.SerializeOrderInformationIntoDesignUserData();
				UIHelper.CoreDataService.DocumentSetFilename(RequestForQuoteFileName);
				UIHelper.CoreDataService.DocumentWrite(QuotedDesignPath);
				EventHelper.RecordEvent(EventHelper.EventTypes.Design_Ordered, "DesignId", UIHelper.LoadedDesign.DesignId.ToString(CultureInfo.InvariantCulture));
				ViewModelLocator.Current.GemReportVM.LargeGems.Where((Gem g) => !string.IsNullOrEmpty(g.StullerId)).ToList().ForEach(delegate(Gem _)
				{
					EventHelper.RecordEvent(EventHelper.EventTypes.Order_Stuller_Item, "Serial Number", _.StullerId);
				});
				(from g in ViewModelLocator.Current.GemReportVM.Gems
					where !string.IsNullOrEmpty(g.StullerId)
					select g.StullerId).Distinct().ToList().ForEach(delegate(string _)
				{
					EventHelper.RecordEvent(EventHelper.EventTypes.Order_Stuller_Item, "Item Number", _);
				});
				IsBusy = false;
				UIHelper.IsOrderedDesignLoaded = true;
			}
			else
			{
				string text = Strings.ErrorSendingQuote + "!\r";
				if (CamJobResponseResult != null && CamJobResponseResult.CamJobErrors != null && CamJobResponseResult.CamJobErrors.Any())
				{
					string text2 = CamJobResponseResult.CamJobErrors.Select((CamJobError _) => _.Message).Aggregate((string current, string next) => current + ", " + next);
					text += text2;
				}
				UIHelper.RaiseApplicationMessageEvent("Send Order", ApplicationMessageType.MultiLineInfo, text, messageIsModal: true);
			}
		}
		IsBusy = false;
	}

	private void ShopGemGroup(GemGroup gemGroup)
	{
		if (!string.IsNullOrWhiteSpace(gemGroup.StoneQuality))
		{
			ViewModelLocator.Current.GemReportVM.UpdateGemGroupQualityAssignments(gemGroup);
		}
		Product stullerProduct = StullerProductAPIHelper.GetNonSerializedProduct(gemGroup.Height, gemGroup.Width, gemGroup.Shape, gemGroup.Material.ToString(), gemGroup.StoneQuality);
		if (stullerProduct != null)
		{
			string agtaInformation = StullerProductAPIHelper.GetAGTADescriptionFromCode(stullerProduct.AGTA);
			gemGroup.GemsInGroup.ToList().ForEach(delegate(Gem g)
			{
				g.IsNotInStock = false;
				g.StullerId = stullerProduct.SKU;
				g.AgtaInformation = agtaInformation;
				g.StullerImage = stullerProduct.Images.FirstOrDefault()?.FullUrl;
			});
			gemGroup.IsNotInStock = false;
			gemGroup.HasCheckedInventory = true;
			gemGroup.StullerId = stullerProduct.SKU;
			gemGroup.StullerDescription = stullerProduct.Description;
			gemGroup.AgtaInformation = agtaInformation;
			gemGroup.StullerImage = stullerProduct.Images.FirstOrDefault()?.FullUrl;
			gemGroup.ForceValidation();
		}
		else
		{
			gemGroup.GemsInGroup.ToList().ForEach(delegate(Gem g)
			{
				g.IsNotInStock = true;
			});
			gemGroup.IsNotInStock = true;
			gemGroup.HasCheckedInventory = true;
			gemGroup.StullerId = null;
			gemGroup.StullerDescription = null;
			gemGroup.StullerImage = null;
			gemGroup.ForceValidation();
		}
	}

	private void UpdateControlVisibilityFromCSOptions()
	{
		IsMeleeSourceOptionOneVisible = UIHelper.CounterSketchOptions.DisabledOrderSummaryItems.All((OrderSummaryPrefItemModel _) => _.Title != Strings.Purchase_Stone_s);
		IsMeleeSourceOptionTwoVisible = UIHelper.CounterSketchOptions.DisabledOrderSummaryItems.All((OrderSummaryPrefItemModel _) => _.Title != Strings.Use_My_Stone_s);
		IsMeleeSourceOptionThreeVisible = UIHelper.CounterSketchOptions.DisabledOrderSummaryItems.All((OrderSummaryPrefItemModel _) => _.Title != Strings.No_Stone_s_Supplied);
		IsHallmarkOptionOneVisible = UIHelper.CounterSketchOptions.DisabledOrderSummaryItems.All((OrderSummaryPrefItemModel _) => _.Title != Strings.Noqualitymarkortrademark);
		IsHallmarkOptionTwoVisible = UIHelper.CounterSketchOptions.DisabledOrderSummaryItems.All((OrderSummaryPrefItemModel _) => _.Title != Strings.LaserengravequalitymarkandStullertrademark);
		IsHallmarkOptionThreeVisible = UIHelper.CounterSketchOptions.DisabledOrderSummaryItems.All((OrderSummaryPrefItemModel _) => _.Title != Strings.Laserengravequalitymarkandmylogo);
		IsHallmarkOptionFourVisible = UIHelper.CounterSketchOptions.DisabledOrderSummaryItems.All((OrderSummaryPrefItemModel _) => _.Title != Strings.LaserengravequalitymarkStullertrademarkandmylogo);
		IsHallmarkSectionVisible = IsHallmarkOptionTwoVisible || IsHallmarkOptionThreeVisible || IsHallmarkOptionFourVisible;
		IsEngravingSectionVisible = UIHelper.CounterSketchOptions.DisabledOrderSummaryItems.All((OrderSummaryPrefItemModel _) => _.Title != Strings.AddCustomEngraving);
		IsNoToggleVisible = UIHelper.CounterSketchOptions.DisabledOrderSummaryItems.All((OrderSummaryPrefItemModel _) => _.Title != Strings.No);
		IsBookAndImagesToggleVisible = UIHelper.CounterSketchOptions.DisabledOrderSummaryItems.All((OrderSummaryPrefItemModel _) => _.Title != Strings.BookAndDigitalImages);
		IsDigitalImagesToggleVisible = UIHelper.CounterSketchOptions.DisabledOrderSummaryItems.All((OrderSummaryPrefItemModel _) => _.Title != Strings.DigitalImagesOnly);
		IsMoreThanARingSectionVisible = IsBookAndImagesToggleVisible || IsDigitalImagesToggleVisible;
		IsWaxMemoYesVisible = UIHelper.CounterSketchOptions.DisabledOrderSummaryItems.All((OrderSummaryPrefItemModel _) => _.Title != Strings.YesSendAWaxMemo);
		IsWaxMemoNoVisible = UIHelper.CounterSketchOptions.DisabledOrderSummaryItems.All((OrderSummaryPrefItemModel _) => _.Title != Strings.NoWaxMemo);
		IsWaxMemoSectionVisible = IsWaxMemoNoVisible || IsWaxMemoYesVisible;
		SelectedNoMoreThanARing = UIHelper.CounterSketchOptions.DefaultOrderSummaryItems.Any((OrderSummaryPrefItemModel _) => _?.Title == Strings.No);
		SelectedMoreThanARing = UIHelper.CounterSketchOptions.DefaultOrderSummaryItems.Any((OrderSummaryPrefItemModel _) => _?.Title == Strings.BookAndDigitalImages);
		SelectedMoreThanARingDigitalOnly = UIHelper.CounterSketchOptions.DefaultOrderSummaryItems.Any((OrderSummaryPrefItemModel _) => _?.Title == Strings.DigitalImagesOnly);
		IncludeWaxMemo = UIHelper.CounterSketchOptions.DefaultOrderSummaryItems.Any((OrderSummaryPrefItemModel _) => _?.Title == Strings.YesSendAWaxMemo);
	}

	private void SendQuote()
	{
		BusyMessage = TranslationHelper.GetTranslatedString("Calculating Pricing");
		IsBusy = true;
		ResetQuote();
		if (ProjectManagementHelper.IsDocumentCanvasEmpty())
		{
			UIHelper.RaiseApplicationMessageEvent("Empty Design", ApplicationMessageType.MultiLineInfo, Strings.TheDesignisemptypleaseOpenorBuildadesignandtryagain, messageIsModal: true);
			IsBusy = false;
			return;
		}
		if (!UIHelper.IsInternetAccessible())
		{
			IsBusy = false;
			return;
		}
		if (StonesEnabled)
		{
			ShopNonSerializedGems();
			DebuggingHelper.PrintTimedOutput("ShopGems Complete");
			ValidateStoneAssignments();
		}
		_attemptingToOrder = false;
		RaisePropertyChanged(() => ValidationErrors);
		RaisePropertyChanged(() => HasValidationErrors);
		if (HasValidationErrors)
		{
			IsBusy = false;
			return;
		}
		BusyMessage = TranslationHelper.GetTranslatedString("Calculating Pricing");
		IsBusy = true;
		List<GemGroup> second = null;
		List<GemGroup> first = null;
		if (StonesEnabled)
		{
			first = new List<GemGroup>(ViewModelLocator.Current.GemReportVM.Gems.GroupBy((Gem gemKey) => new { gemKey.IsCustomerStoneSupplied, gemKey.IsNoStoneSupplied, gemKey.StullerId }, (key, IEnumerable<Gem> gems) =>
			{
				Gem[] array = (gems as Gem[]) ?? gems.ToArray();
				return new GemGroup
				{
					Material = gems.FirstOrDefault().Material,
					Shape = gems.FirstOrDefault().Shape,
					Width = gems.FirstOrDefault().Width,
					Height = gems.FirstOrDefault().Height,
					Quantity = gems.Count(),
					StoneQuality = gems.FirstOrDefault().StoneQuality,
					GemsInGroup = new List<Gem>(array),
					GemNumbers = new List<string>(array.Select((Gem g) => g.GemNumber)),
					Source = gems.FirstOrDefault().Source,
					SetType = gems.FirstOrDefault().SetType,
					StullerId = key.StullerId,
					GemMountingType = array.First().GemMountingType
				};
			}));
			second = new List<GemGroup>(ViewModelLocator.Current.GemReportVM.LargeGems.Where((Gem _) => _.IsDiamondAssigned || !_.IsRedBox).Select(delegate(Gem _)
			{
				string stullerId = null;
				if (_.AssignedDiamond != null && _.IsRedBox)
				{
					stullerId = _.AssignedDiamond.SerialNumber.ToString();
				}
				else if (!_.IsRedBox)
				{
					stullerId = _.StullerId;
				}
				return new GemGroup
				{
					StullerId = stullerId,
					Material = _.Material,
					Shape = _.Shape,
					Width = _.Width,
					Height = _.Height,
					Quantity = 1,
					StoneQuality = _.StoneQuality,
					GemsInGroup = new List<Gem> { _ },
					GemNumbers = new List<string> { _.GemNumber },
					Source = _.Source,
					Value = _.Value,
					SetType = _.SetType,
					GemMountingType = _.GemMountingType
				};
			}));
			if (_gemOrderingErrorList.Any())
			{
				RaisePropertyChanged(() => ValidationErrors);
				RaisePropertyChanged(() => HasValidationErrors);
				if (HasValidationErrors)
				{
					IsDesignQuoted = false;
					IsBusy = false;
					return;
				}
			}
		}
		int stoneStullerCount = 0;
		int stoneCustomerCount = 0;
		int stoneNoneCount = 0;
		int totalStoneCount = 0;
		List<GemGroup> gemGroupings = null;
		if (StonesEnabled)
		{
			gemGroupings = first.Union(second).ToList();
			stoneStullerCount = ViewModelLocator.Current.GemReportVM.Gems.Count((Gem _) => _.Source == Strings.Purchase_Stone_s);
			stoneCustomerCount = ViewModelLocator.Current.GemReportVM.Gems.Count((Gem _) => _.Source == Strings.Use_My_Stone_s);
			stoneNoneCount = ViewModelLocator.Current.GemReportVM.Gems.Count((Gem _) => _.Source == Strings.No_Stone_s_Supplied);
			totalStoneCount = ViewModelLocator.Current.GemReportVM.Gems.Count;
		}
		string empty = string.Empty;
		string empty2 = string.Empty;
		CalculateVolume();
		string currentCultureCode = UIHelper.CounterSketchOptions.CurrentCultureCode;
		int numberOfCastingsFromVisibleComponets = GetNumberOfCastingsFromVisibleComponets();
		string empty3 = string.Empty;
		_ = string.Empty;
		Convert.ToDecimal("0");
		List<ClientMarkup> markups = new List<ClientMarkup>
		{
			new ClientMarkup
			{
				Name = "MarkupDiamondSize1",
				Value = (decimal)UIHelper.KeyProfile.MarkupDiamondSize1
			},
			new ClientMarkup
			{
				Name = "MarkupDiamondSize2",
				Value = (decimal)UIHelper.KeyProfile.MarkupDiamondSize2
			},
			new ClientMarkup
			{
				Name = "MarkupDiamondSize3",
				Value = (decimal)UIHelper.KeyProfile.MarkupDiamondSize3
			},
			new ClientMarkup
			{
				Name = "MarkupDiamondSize4",
				Value = (decimal)UIHelper.KeyProfile.MarkupDiamondSize4
			},
			new ClientMarkup
			{
				Name = "MarkupDiamondSize5",
				Value = (decimal)UIHelper.KeyProfile.MarkupDiamondSize5
			},
			new ClientMarkup
			{
				Name = "MarkupGemSize1",
				Value = (decimal)UIHelper.KeyProfile.MarkupGemSize1
			},
			new ClientMarkup
			{
				Name = "MarkupGemSize2",
				Value = (decimal)UIHelper.KeyProfile.MarkupGemSize2
			},
			new ClientMarkup
			{
				Name = "MarkupGemSize3",
				Value = (decimal)UIHelper.KeyProfile.MarkupGemSize3
			},
			new ClientMarkup
			{
				Name = "MarkupGemSize4",
				Value = (decimal)UIHelper.KeyProfile.MarkupGemSize4
			},
			new ClientMarkup
			{
				Name = "MarkupGemSize5",
				Value = (decimal)UIHelper.KeyProfile.MarkupGemSize5
			},
			new ClientMarkup
			{
				Name = "MarkupSetting",
				Value = (decimal)UIHelper.KeyProfile.MarkupSetting
			},
			new ClientMarkup
			{
				Name = "MarkupProfileDefined1:" + UIHelper.KeyProfile.MarkupProfileDefined1Name,
				Value = (decimal)UIHelper.KeyProfile.MarkupProfileDefined1
			},
			new ClientMarkup
			{
				Name = "MarkupProfileDefined1:" + UIHelper.KeyProfile.MarkupProfileDefined2Name,
				Value = (decimal)UIHelper.KeyProfile.MarkupProfileDefined2
			},
			new ClientMarkup
			{
				Name = "MarkupProfileDefined1:" + UIHelper.KeyProfile.MarkupProfileDefined3Name,
				Value = (decimal)UIHelper.KeyProfile.MarkupProfileDefined3
			}
		};
		MetalMaterial materialFromMetalFriendlyName = MetalHelper.GetMaterialFromMetalFriendlyName(PrimaryMetal);
		MetalMaterial materialFromMetalFriendlyName2 = MetalHelper.GetMaterialFromMetalFriendlyName(SecondaryMetal);
		QuoteEMail = UIHelper.CounterSketchOptions.Email;
		QuotePhone = UIHelper.CounterSketchOptions.Phone;
		SendSMSNotifications = UIHelper.CounterSketchOptions.SendSMSNotifications;
		double selectedRingSizeMillimeters = 0.0;
		UIHelper.CoreDataService.OrderGetCurrentRingSizesDiameter(delegate(List<double> ringDiameterSizes)
		{
			int index = RingSizes?.IndexOf(SelectedRingSize) ?? 0;
			if (ringDiameterSizes.Any())
			{
				selectedRingSizeMillimeters = ringDiameterSizes.ElementAt(index);
			}
		});
		CamJob camJob = StullerCamAPIHelper.CreateCamJob(BuyerFirstname, BuyerLastname, QuoteEMail, QuotePhone, SendSMSNotifications, UIHelper.CounterSketchOptions.ShipToAccountNumber, UIHelper.CounterSketchOptions.Company, materialFromMetalFriendlyName, materialFromMetalFriendlyName2, gemGroupings, currentCultureCode, (decimal)selectedRingSizeMillimeters, EngravingMessage, SelectedEngravingFont, SelectedEngravingFontSize.ToString(), SelectedEngravingFillColor.ToString(), numberOfCastingsFromVisibleComponets, SelectedMoreThanARing, SelectedMoreThanARingDigitalOnly, SelectedFinishing.ToString(), SelectedHallmarking.ToString(), stoneStullerCount, stoneCustomerCount, totalStoneCount, stoneNoneCount, empty, empty2, AdditionalDesignNotes, CustomerReferenceNumber, UIHelper.LoadedDesign.SPINumber, UIHelper.LoadedDesign.SeriesNumber, empty3, IsEngagementOptionSelected, ShowEngagementSendOptions, SelectedFinishing == CSEnumFinishing.SampleModel, markups, null, isQuickEstimateOnly: false, IncludeDuplicateMatchingBand, IncludeWaxMemo);
		GlobalErrorHandler.WriteInfoToEventLog("Calling GetEstimate.", 2);
		CamJobResponseResult = StullerCamAPIHelper.GetEstimate(camJob);
		if (CamJobResponseResult != null && CamJobResponseResult.CamJobErrors != null && CamJobResponseResult.CamJobErrors.Any())
		{
			_gemOrderingErrorList.AddRange(CamJobResponseResult.CamJobErrors.Select((CamJobError _) => _.Message));
			RaisePropertyChanged(() => ValidationErrors);
			RaisePropertyChanged(() => HasValidationErrors);
			if (HasValidationErrors)
			{
				IsDesignQuoted = false;
				IsBusy = false;
			}
			GlobalErrorHandler.WriteInfoToEventLog(string.Format("Called GetEstimate, got the following Errors:{0}", CamJobResponseResult.CamJobErrors.Select((CamJobError _) => _.Message).Aggregate((string current, string next) => current + "," + next)));
		}
		else
		{
			CalculatePricesUsingStullerShowcaseMarkups();
			QuotedLargeStonesPriceString = string.Format(new CultureInfo(currentCultureCode), "{1} : {0:C}", QuotedLargeStonesPrice, CultureInfo.CurrentCulture.TextInfo.ToTitleCase(Strings.LARGESTONES));
			QuotedSettingPriceMarkedUpString = string.Format(new CultureInfo(currentCultureCode), "{1} : {0:C}", QuotedSettingPriceMarkedUp, CultureInfo.CurrentCulture.TextInfo.ToTitleCase(Strings.SETTING));
			QuotedPriceString = string.Format(new CultureInfo(currentCultureCode), "{1} : {0:C}", QuotedPrice, Strings.TotalPrice);
			EventHelper.RecordEvent(EventHelper.EventTypes.Design_Quoted, $"IsNew={DesignHelper.IsNewDesign(UIHelper.LoadedDesign.DesignId)}", UIHelper.LoadedDesign.DesignId.ToString(CultureInfo.InvariantCulture));
			IsDesignQuoted = true;
			SendOrder();
		}
	}

	private void CalculatePricesUsingStullerShowcaseMarkups()
	{
		QuotedPrice = (double)CamJobResponseResult.CamJob.TotalShowcasePrice.GetValueOrDefault();
	}

	public void SendQuickEstimateShowcaseMarkups()
	{
		if (ProjectManagementHelper.IsDocumentCanvasEmpty())
		{
			UIHelper.RaiseApplicationMessageEvent("Empty Design", ApplicationMessageType.MultiLineInfo, Strings.TheDesignisemptypleaseOpenorBuildadesignandtryagain, messageIsModal: true);
		}
		else if (UIHelper.IsOrderedDesignLoaded)
		{
			UIHelper.RaiseApplicationMessageEvent("Previously Ordered Design", ApplicationMessageType.MultiLineInfo, Strings.Thedesignhasalreadybeenorderedpleasesaveandreopentoestimateagain, messageIsModal: true);
		}
		else
		{
			if (!UIHelper.IsInternetAccessible())
			{
				return;
			}
			BusyMessage = Strings.QuickEstimating;
			IsBusy = true;
			List<GemGroup> list = null;
			list = (DebugEstimateGemGroups = new List<GemGroup>(ViewModelLocator.Current.GemReportVM.Gems.GroupBy((Gem gemKey) => new { gemKey.Shape, gemKey.Width, gemKey.Height }, (key, IEnumerable<Gem> gems) =>
			{
				Gem[] array = (gems as Gem[]) ?? gems.ToArray();
				return new GemGroup
				{
					Material = GemMaterial.Diamond,
					Shape = key.Shape,
					Width = key.Width,
					Height = key.Height,
					Quantity = array.Count(),
					GemsInGroup = new List<Gem>(array),
					SetType = array[0].SetType,
					GemNumbers = new List<string>(array.Select((Gem g) => g.GemNumber)),
					Value = "0",
					Carat = array.Max((Gem _) => _.Carat),
					IsQuickEstimateGroup = true,
					GemMountingType = array.First().GemMountingType
				};
			})));
			List<GemGroup> estimateGemGroups = new List<GemGroup>();
			bool gemsMissing = false;
			int qualityLevel;
			for (qualityLevel = 0; qualityLevel <= 3; qualityLevel++)
			{
				list.ForEach(delegate(GemGroup gemGroup)
				{
					gemGroup.StoneQuality = gemGroup.StoneQualities[qualityLevel];
					Product stullerProduct = StullerProductAPIHelper.GetNonSerializedProduct(gemGroup.Height, gemGroup.Width, gemGroup.Shape, gemGroup.Material.ToString(), gemGroup.StoneQuality);
					if (stullerProduct != null)
					{
						gemGroup.GemsInGroup.ToList().ForEach(delegate(Gem g)
						{
							g.IsNotInStock = false;
							g.StullerId = stullerProduct.SKU;
						});
						gemGroup.IsNotInStock = false;
						gemGroup.HasCheckedInventory = true;
						gemGroup.StullerId = stullerProduct.SKU;
					}
					else
					{
						gemsMissing = true;
						gemGroup.GemsInGroup.ToList().ForEach(delegate(Gem g)
						{
							g.IsNotInStock = true;
						});
						gemGroup.IsNotInStock = true;
						gemGroup.HasCheckedInventory = true;
						gemGroup.StullerId = null;
					}
					if (gemGroup.Clone() is GemGroup gemGroup2)
					{
						gemGroup2.EstimateQualityLevel = qualityLevel;
						estimateGemGroups.Add(gemGroup2);
					}
				});
			}
			ViewModelLocator.Current.QuickEstimateVM.GemsIncludedInEstimate = list.Sum((GemGroup _) => _.GemsInGroup.Count).ToString();
			ViewModelLocator.Current.QuickEstimateVM.GemsIncludedInEstimateWeight = list.Sum((GemGroup _) => _.Carat * (double)_.Quantity).ToString(CultureInfo.InvariantCulture);
			ViewModelLocator.Current.QuickEstimateVM.GemsNotIncludedInEstimate = ViewModelLocator.Current.GemReportVM.LargeGems.Count.ToString();
			ViewModelLocator.Current.QuickEstimateVM.GemsNotIncludedInEstimateWeight = ViewModelLocator.Current.GemReportVM.LargeGems.Sum((Gem _) => _.Carat).ToString(CultureInfo.InvariantCulture);
			int num = 0;
			int stoneCustomerCount = 0;
			int stoneNoneCount = 0;
			int num2 = 0;
			DebugEstimateGemGroups = estimateGemGroups;
			List<GemGroup> list3 = estimateGemGroups;
			num = list3.Sum((GemGroup g) => g.GemsInGroup.Count);
			num2 = num;
			string empty = string.Empty;
			string empty2 = string.Empty;
			CalculateVolume();
			if (ViewModelLocator.Current.OrderSummaryVM.CalculatedVolumes.Count < 1)
			{
				UIHelper.RaiseApplicationMessageEvent("Empty Design", ApplicationMessageType.MultiLineInfo, Strings.TheDesignCannotBeEstimatedWithoutAnyMetal, messageIsModal: true);
				return;
			}
			string currentCultureCode = UIHelper.CounterSketchOptions.CurrentCultureCode;
			int numberOfCastingsFromVisibleComponets = GetNumberOfCastingsFromVisibleComponets();
			string empty3 = string.Empty;
			QuoteEMail = UIHelper.CounterSketchOptions.Email;
			QuotePhone = UIHelper.CounterSketchOptions.Phone;
			SendSMSNotifications = UIHelper.CounterSketchOptions.SendSMSNotifications;
			List<MetalMaterial> list4 = (from _ in MetalHelper.GetMetalMaterialOptionItems()
				select _.MetalMaterial).ToList();
			int count = list4.Count;
			double selectedRingSizeMillimeters = 0.0;
			UIHelper.CoreDataService.OrderGetCurrentRingSizesDiameter(delegate(List<double> ringDiameterSizes)
			{
				int index = RingSizes?.IndexOf(SelectedRingSize) ?? 0;
				if (ringDiameterSizes.Any())
				{
					selectedRingSizeMillimeters = ringDiameterSizes.ElementAt(index);
				}
			});
			CamJob camJob = StullerCamAPIHelper.CreateCamJob(BuyerFirstname, BuyerLastname, QuoteEMail, QuotePhone, SendSMSNotifications, UIHelper.CounterSketchOptions.ShipToAccountNumber, UIHelper.CounterSketchOptions.Company, null, null, list3, currentCultureCode, (decimal)selectedRingSizeMillimeters, EngravingMessage, SelectedEngravingFont, SelectedEngravingFontSize.ToString(), SelectedEngravingFillColor.ToString(), numberOfCastingsFromVisibleComponets, SelectedMoreThanARing, SelectedMoreThanARingDigitalOnly, SelectedFinishing.ToString(), SelectedHallmarking.ToString(), num, stoneCustomerCount, num2, stoneNoneCount, empty, empty2, AdditionalDesignNotes, CustomerReferenceNumber, UIHelper.LoadedDesign.SPINumber, UIHelper.LoadedDesign.SeriesNumber, empty3, IsEngagementOptionSelected, ShowEngagementSendOptions, SelectedFinishing == CSEnumFinishing.SampleModel, null, list4, isQuickEstimateOnly: true);
			GlobalErrorHandler.WriteInfoToEventLog("Calling GetEstimate.", 2);
			CamJobResponseResult = StullerCamAPIHelper.GetEstimate(camJob);
			if (CamJobResponseResult != null && CamJobResponseResult.CamJobErrors != null && CamJobResponseResult.CamJobErrors.Any())
			{
				_gemOrderingErrorList.AddRange(CamJobResponseResult.CamJobErrors.Select((CamJobError _) => _.Message));
				RaisePropertyChanged(() => ValidationErrors);
				RaisePropertyChanged(() => HasValidationErrors);
				if (HasValidationErrors)
				{
					IsDesignQuoted = false;
					IsBusy = false;
				}
				GlobalErrorHandler.WriteInfoToEventLog(string.Format("Called GetEstimate, got the following Errors:{0}", CamJobResponseResult.CamJobErrors.Select((CamJobError _) => _.Message).Aggregate((string current, string next) => current + "," + next)));
				return;
			}
			ViewModelLocator.Current.QuickEstimateVM.Reset();
			ViewModelLocator.Current.QuickEstimateVM.EstimateDateTime = DateTime.Now;
			ViewModelLocator.Current.QuickEstimateVM.DesignName = UIHelper.LoadedDesign.SPINumber;
			int num3 = CamJobResponseResult.CamJob.CamJobMetals.Count / count;
			for (int num4 = 0; num4 < count; num4++)
			{
				int count2 = num3 * num4;
				List<CamJobMetal> source = CamJobResponseResult.CamJob.CamJobMetals.Skip(count2).Take(num3).ToList();
				QuickEstimateItem quickEstimateItem = new QuickEstimateItem
				{
					MetalEstimateLevel = num4
				};
				ViewModelLocator.Current.QuickEstimateVM.QuickEstimateItems.Add(quickEstimateItem);
				quickEstimateItem.MetalName = MetalHelper.GetMetalFriendlyNameFromMaterial(list4[num4]);
				quickEstimateItem.MetalMaterial = list4[num4];
				int num5 = CamJobResponseResult.CamJob.CamJobGems.Count / 4;
				for (int gemEstimateLevel = 0; gemEstimateLevel < 4; gemEstimateLevel++)
				{
					int count3 = num5 * gemEstimateLevel;
					List<CamJobGem> matchingPricedGemGroups = CamJobResponseResult.CamJob.CamJobGems.Skip(count3).Take(num5).ToList();
					GlobalErrorHandler.WriteInfoToEventLog("Applying Stone Markups.", 2);
					List<GemGroup> list5 = (from _ in estimateGemGroups
						where _.EstimateQualityLevel == gemEstimateLevel && _.Carat >= UIHelper.KeyProfile.Range1Max
						select (_)).ToList();
					List<GemGroup> list6 = (from _ in estimateGemGroups
						where _.EstimateQualityLevel == gemEstimateLevel && _.Carat >= UIHelper.KeyProfile.Range2Max && _.Carat < UIHelper.KeyProfile.Range1Max
						select (_)).ToList();
					List<GemGroup> list7 = (from _ in estimateGemGroups
						where _.EstimateQualityLevel == gemEstimateLevel && _.Carat >= UIHelper.KeyProfile.Range3Max && _.Carat < UIHelper.KeyProfile.Range2Max
						select (_)).ToList();
					List<GemGroup> list8 = (from _ in estimateGemGroups
						where _.EstimateQualityLevel == gemEstimateLevel && _.Carat >= UIHelper.KeyProfile.Range4Max && _.Carat < UIHelper.KeyProfile.Range3Max
						select (_)).ToList();
					List<GemGroup> list9 = (from _ in estimateGemGroups
						where _.EstimateQualityLevel == gemEstimateLevel && _.Carat < UIHelper.KeyProfile.Range4Max
						select (_)).ToList();
					list5.ForEach(delegate(GemGroup _)
					{
						SetGemCostsForEstimateStullerShowcaseMarkups(_, UIHelper.KeyProfile.MarkupDiamondSize1, matchingPricedGemGroups);
					});
					list6.ForEach(delegate(GemGroup _)
					{
						SetGemCostsForEstimateStullerShowcaseMarkups(_, UIHelper.KeyProfile.MarkupDiamondSize2, matchingPricedGemGroups);
					});
					list7.ForEach(delegate(GemGroup _)
					{
						SetGemCostsForEstimateStullerShowcaseMarkups(_, UIHelper.KeyProfile.MarkupDiamondSize3, matchingPricedGemGroups);
					});
					list8.ForEach(delegate(GemGroup _)
					{
						SetGemCostsForEstimateStullerShowcaseMarkups(_, UIHelper.KeyProfile.MarkupDiamondSize4, matchingPricedGemGroups);
					});
					list9.ForEach(delegate(GemGroup _)
					{
						SetGemCostsForEstimateStullerShowcaseMarkups(_, UIHelper.KeyProfile.MarkupDiamondSize5, matchingPricedGemGroups);
					});
					List<GemGroup> list10 = list5.Where((GemGroup _) => _.IsNotInStock).Union(list6.Where((GemGroup _) => _.IsNotInStock)).Union(list7.Where((GemGroup _) => _.IsNotInStock))
						.Union(list8.Where((GemGroup _) => _.IsNotInStock))
						.Union(list9.Where((GemGroup _) => _.IsNotInStock))
						.ToList();
					switch (gemEstimateLevel)
					{
					case 0:
						if (list10.Count > 0)
						{
							quickEstimateItem.IsOutOfStockLevel0 = true;
						}
						quickEstimateItem.PriceWithGemLevel0 = estimateGemGroups.Where((GemGroup _) => _.EstimateQualityLevel == 0).Sum((GemGroup _) => _.CustomerMarkedUpCost);
						quickEstimateItem.PriceWithGemLevel0Setting = matchingPricedGemGroups.Sum((CamJobGem _) => _.ShowcaseExtendedSettingPrice.GetValueOrDefault());
						ViewModelLocator.Current.QuickEstimateVM.OutOfStockLevel0 = list10;
						break;
					case 1:
						if (list10.Count > 0)
						{
							quickEstimateItem.IsOutOfStockLevel1 = true;
						}
						quickEstimateItem.PriceWithGemLevel1 = estimateGemGroups.Where((GemGroup _) => _.EstimateQualityLevel == 1).Sum((GemGroup _) => _.CustomerMarkedUpCost);
						quickEstimateItem.PriceWithGemLevel1Setting = matchingPricedGemGroups.Sum((CamJobGem _) => _.ShowcaseExtendedSettingPrice.GetValueOrDefault());
						ViewModelLocator.Current.QuickEstimateVM.OutOfStockLevel1 = list10;
						break;
					case 2:
						if (list10.Count > 0)
						{
							quickEstimateItem.IsOutOfStockLevel2 = true;
						}
						quickEstimateItem.PriceWithGemLevel2 = estimateGemGroups.Where((GemGroup _) => _.EstimateQualityLevel == 2).Sum((GemGroup _) => _.CustomerMarkedUpCost);
						quickEstimateItem.PriceWithGemLevel2Setting = matchingPricedGemGroups.Sum((CamJobGem _) => _.ShowcaseExtendedSettingPrice.GetValueOrDefault());
						ViewModelLocator.Current.QuickEstimateVM.OutOfStockLevel2 = list10;
						break;
					case 3:
						if (list10.Count > 0)
						{
							quickEstimateItem.IsOutOfStockLevel3 = true;
						}
						quickEstimateItem.PriceWithGemLevel3 = estimateGemGroups.Where((GemGroup _) => _.EstimateQualityLevel == 3).Sum((GemGroup _) => _.CustomerMarkedUpCost);
						quickEstimateItem.PriceWithGemLevel3Setting = matchingPricedGemGroups.Sum((CamJobGem _) => _.ShowcaseExtendedSettingPrice.GetValueOrDefault());
						ViewModelLocator.Current.QuickEstimateVM.OutOfStockLevel3 = list10;
						break;
					}
				}
				GlobalErrorHandler.WriteInfoToEventLog("Applying Remaining Markups.", 2);
				if (CamJobResponseResult != null)
				{
					quickEstimateItem.QuotedMetalPrice = source.Sum((CamJobMetal _) => _.ShowcasePrice.GetValueOrDefault());
					quickEstimateItem.QuotedRingSizePrice = CamJobResponseResult.CamJob.CamJobRingSize.ShowcasePrice.GetValueOrDefault();
					quickEstimateItem.QuotedCamAssemblyPrice = CamJobResponseResult.CamJob.ShowcaseCamAssemblyPrice.GetValueOrDefault();
					quickEstimateItem.QuotedCamPrice = CamJobResponseResult.CamJob.ShowcaseCamPrice.GetValueOrDefault();
					quickEstimateItem.QuotedPolishPrice = CamJobResponseResult.CamJob.ShowcasePolishPrice.GetValueOrDefault();
					quickEstimateItem.QuotedSettingPrice = (double)quickEstimateItem.QuotedRingSizePrice + (double)quickEstimateItem.QuotedCamAssemblyPrice + (double)quickEstimateItem.QuotedCamPrice + (double)quickEstimateItem.QuotedPolishPrice + (double)quickEstimateItem.QuotedMetalPrice;
					quickEstimateItem.PriceMountingOnly = quickEstimateItem.QuotedSettingPrice;
					quickEstimateItem.PriceWithGemLevel0 += (double)quickEstimateItem.PriceWithGemLevel0Setting + quickEstimateItem.QuotedSettingPrice;
					quickEstimateItem.PriceWithGemLevel1 += (double)quickEstimateItem.PriceWithGemLevel1Setting + quickEstimateItem.QuotedSettingPrice;
					quickEstimateItem.PriceWithGemLevel2 += (double)quickEstimateItem.PriceWithGemLevel2Setting + quickEstimateItem.QuotedSettingPrice;
					quickEstimateItem.PriceWithGemLevel3 += (double)quickEstimateItem.PriceWithGemLevel3Setting + quickEstimateItem.QuotedSettingPrice;
					quickEstimateItem.PriceMountingOnly += (double)CamJobResponseResult.CamJob.CounterSketchShowcaseAdditionalCharge.GetValueOrDefault();
					quickEstimateItem.PriceWithGemLevel0 += (double)CamJobResponseResult.CamJob.CounterSketchShowcaseAdditionalCharge.GetValueOrDefault();
					quickEstimateItem.PriceWithGemLevel1 += (double)CamJobResponseResult.CamJob.CounterSketchShowcaseAdditionalCharge.GetValueOrDefault();
					quickEstimateItem.PriceWithGemLevel2 += (double)CamJobResponseResult.CamJob.CounterSketchShowcaseAdditionalCharge.GetValueOrDefault();
					quickEstimateItem.PriceWithGemLevel3 += (double)CamJobResponseResult.CamJob.CounterSketchShowcaseAdditionalCharge.GetValueOrDefault();
					quickEstimateItem.PriceMountingOnly *= (double)CamJobResponseResult.CamJob.ShowcaseSalesTaxPercentage.GetValueOrDefault() + 1.0;
					quickEstimateItem.PriceWithGemLevel0 *= (double)CamJobResponseResult.CamJob.ShowcaseSalesTaxPercentage.GetValueOrDefault() + 1.0;
					quickEstimateItem.PriceWithGemLevel1 *= (double)CamJobResponseResult.CamJob.ShowcaseSalesTaxPercentage.GetValueOrDefault() + 1.0;
					quickEstimateItem.PriceWithGemLevel2 *= (double)CamJobResponseResult.CamJob.ShowcaseSalesTaxPercentage.GetValueOrDefault() + 1.0;
					quickEstimateItem.PriceWithGemLevel3 *= (double)CamJobResponseResult.CamJob.ShowcaseSalesTaxPercentage.GetValueOrDefault() + 1.0;
					quickEstimateItem.TotalCaratWeight = ViewModelLocator.Current.GemReportVM.Gems.Sum((Gem _) => _.Carat);
					double num6 = CalculateTotalVolumeMetalPerClassification(MaterialClassification.MetalPrimary, CalculatedVolumes);
					double num7 = CalculateTotalVolumeMetalPerClassification(MaterialClassification.MetalSecondary, CalculatedVolumes);
					decimal metalPennyWeight = CSMetalHelper.GetMetalPennyWeight(num6 + num7, list4[num4]);
					quickEstimateItem.Weight = $"{metalPennyWeight:0.00} DWT ({(double)metalPennyWeight * 1.55517:0.00} g)";
					quickEstimateItem.PriceMountingOnlyString = string.Format(new CultureInfo(currentCultureCode), "{0:C}", quickEstimateItem.PriceMountingOnly);
					quickEstimateItem.PriceWithGemLevel0String = string.Format(new CultureInfo(currentCultureCode), "{0:C}", quickEstimateItem.PriceWithGemLevel0);
					quickEstimateItem.PriceWithGemLevel1String = string.Format(new CultureInfo(currentCultureCode), "{0:C}", quickEstimateItem.PriceWithGemLevel1);
					quickEstimateItem.PriceWithGemLevel2String = string.Format(new CultureInfo(currentCultureCode), "{0:C}", quickEstimateItem.PriceWithGemLevel2);
					quickEstimateItem.PriceWithGemLevel3String = string.Format(new CultureInfo(currentCultureCode), "{0:C}", quickEstimateItem.PriceWithGemLevel3);
				}
			}
			EventHelper.RecordEvent(EventHelper.EventTypes.Design_Estimated, $"IsNew={DesignHelper.IsNewDesign(UIHelper.LoadedDesign.DesignId)}", UIHelper.LoadedDesign.DesignId.ToString(CultureInfo.InvariantCulture));
			WindowQuickEstimateView obj = new WindowQuickEstimateView
			{
				Owner = Application.Current.MainWindow
			};
			ViewModelLocator.Current.QuickEstimateVM.UpdateBindings();
			ViewModelLocator.Current.QuickEstimateVM.IsMissingGems = gemsMissing;
			obj.ShowDialog();
			BusyMessage = null;
			IsBusy = false;
		}
	}

	private void SetGemCostsForEstimateStullerShowcaseMarkups(GemGroup _, double markup, List<CamJobGem> pricedGemGroups)
	{
		CamJobGem camJobGem = pricedGemGroups.FirstOrDefault((CamJobGem pjg) => pjg.StoneMapReference == _.GemNumbersString);
		if (camJobGem != null)
		{
			if (camJobGem.Quantity == 0)
			{
				_.StullerCost = 0.0;
				_.CustomerMarkup = markup;
				_.CustomerMarkedUpCost = 0.0;
			}
			else
			{
				_.StullerCost = (double)camJobGem.ExtendedPrice.GetValueOrDefault();
				_.CustomerMarkup = markup;
				_.CustomerMarkedUpCost = (double)camJobGem.ShowcaseExtendedPrice.GetValueOrDefault();
			}
		}
	}

	private void SetupVolumeWorker()
	{
		CalculatedVolumes = new Dictionary<Tuple<ComponentsEnum, MaterialClassification>, double>();
		BackgroundVolumeWorker.DoWork += _backgroundVolumeWorker_DoWork;
		BackgroundVolumeWorker.RunWorkerCompleted += _backgroundVolumeWorker_RunWorkerCompleted;
	}

	private void UpdateWeightsFromVolumes(bool useSpecifiedMetal = false, MetalMaterial specifiedMetalMaterial = MetalMaterial.WhiteGold)
	{
		decimal num = default(decimal);
		decimal num2 = default(decimal);
		MetalMaterial[] metalMaterials = new MetalMaterial[2];
		if (useSpecifiedMetal)
		{
			metalMaterials = new MetalMaterial[2] { specifiedMetalMaterial, specifiedMetalMaterial };
		}
		else
		{
			UIHelper.CoreDataService.MaterialsGetMetal(delegate(MetalMaterial[] arg, Exception exception)
			{
				metalMaterials = arg;
			});
		}
		MetalMaterial whichMetalMaterial = metalMaterials[0];
		num = CSMetalHelper.GetMetalPennyWeight(CalculateTotalVolumeMetalPerClassification(MaterialClassification.MetalPrimary, CalculatedVolumes), whichMetalMaterial);
		MetalMaterial whichMetalMaterial2 = metalMaterials[1];
		num2 = CSMetalHelper.GetMetalPennyWeight(CalculateTotalVolumeMetalPerClassification(MaterialClassification.MetalSecondary, CalculatedVolumes), whichMetalMaterial2);
		PennyWeightMetalPrimary = num;
		PennyWeightMetalSecondary = num2;
		PrimaryMetalWeight = $"{PennyWeightMetalPrimary:0.00} DWT ({(double)PennyWeightMetalPrimary * 1.55517:0.00} g)";
		CombinedMetalWeight = $"{PennyWeightMetalPrimary + PennyWeightMetalSecondary:0.00} DWT ({(double)(PennyWeightMetalPrimary + PennyWeightMetalSecondary) * 1.55517:0.00} g)";
		SecondaryMetalWeight = ((PrimaryMetal == SecondaryMetal && num2 <= 0m) ? "NA" : $"{PennyWeightMetalSecondary:0.00} DWT ({(double)PennyWeightMetalSecondary * 1.55517:0.00} g)");
		GlobalErrorHandler.WriteInfoToEventLog("Calculating Volume Complete.", 2);
	}

	private void ValidateStoneAssignments()
	{
		_gemOrderingErrorList.Clear();
		if (ViewModelLocator.Current.GemReportVM.GemGroups != null)
		{
			ViewModelLocator.Current.GemReportVM.GemGroups.Where((GemGroup _) => _.IsNotInStock && !_.IsNoStoneSupplied && !_.IsCustomerStoneSupplied).ForEach(delegate(GemGroup meleeGroup)
			{
				_gemOrderingErrorList.Add(meleeGroup.Type + " " + meleeGroup.Shape + " " + Strings.MeleeGroupsAreNotAllInStockAtThatQuality);
			});
		}
		if (ViewModelLocator.Current.GemReportVM.LargeGems != null)
		{
			ViewModelLocator.Current.GemReportVM.LargeGems.Where((Gem _) => _.IsNotInStock && !_.IsRedBox && !_.IsCustomerStoneSupplied && !_.IsNoStoneSupplied).ForEach(delegate(Gem largeGem)
			{
				_gemOrderingErrorList.Add($"{largeGem.Type} {largeGem.Shape} - {largeGem.Height:0.00} x {largeGem.Width:0.00} {Strings.IsNotInStock}.");
			});
		}
		if (ViewModelLocator.Current.GemReportVM.LargeGems != null)
		{
			ViewModelLocator.Current.GemReportVM.LargeGems.Where((Gem _) => !_.IsDiamondAssigned && _.IsRedBox && !_.IsNoStoneSupplied && !_.IsCustomerStoneSupplied).ForEach(delegate(Gem largeGem)
			{
				_gemOrderingErrorList.Add($"{largeGem.Type} {largeGem.Shape} - {largeGem.Height:0.00} x {largeGem.Width:0.00} {Strings.IsNotAssignedADiamond}.");
			});
		}
		if (ViewModelLocator.Current.GemReportVM.LargeGems != null)
		{
			ViewModelLocator.Current.GemReportVM.LargeGems.Where((Gem _) => _.IsNotInStock && _.IsDiamondAssigned && _.IsRedBox).ForEach(delegate(Gem largeGem)
			{
				_gemOrderingErrorList.Add($"{largeGem.Type} {largeGem.Shape} - {largeGem.Height:0.00} x {largeGem.Width:0.00} #{largeGem.StullerId} {Strings.IsNoLongerAvailablePleaseAssignAnotherDiamond}.");
			});
		}
		if (ViewModelLocator.Current.GemReportVM.LargeGems != null && ViewModelLocator.Current.GemReportVM.LargeGems.Any((Gem _) => _.IsCustomerStoneSupplied && string.IsNullOrWhiteSpace(_.Value)))
		{
			_gemOrderingErrorList.Add(Strings.WhenSupplyingLargeStonesYouMustSpecifyTheEstimatedValue + ".");
		}
		int num = ViewModelLocator.Current.GemReportVM.Gems.Count((Gem _) => _.Source == Strings.No_Stone_s_Supplied) + ViewModelLocator.Current.GemReportVM.LargeGems.Count((Gem _) => _.Source == Strings.No_Stone_s_Supplied);
		int num2 = ViewModelLocator.Current.GemReportVM.Gems.Count + ViewModelLocator.Current.GemReportVM.LargeGems.Count;
		if (num == num2 && SelectedFinishing == CSEnumFinishing.FinishedAndSet)
		{
			_gemOrderingErrorList.Add(Strings.InvalidFinishSpecifiedwhenallgemsareNoStoneSupplieduseFinished + ".");
		}
		RaisePropertyChanged(() => ValidationErrors);
		RaisePropertyChanged(() => HasValidationErrors);
		if (HasValidationErrors)
		{
			IsDesignQuoted = false;
		}
	}
}
