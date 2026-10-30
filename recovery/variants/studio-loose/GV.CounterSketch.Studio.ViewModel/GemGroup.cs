using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Linq.Expressions;
using System.Windows.Media.Imaging;
using System.Xml.Serialization;
using GV.CounterSketch.Common;
using GV.CounterSketch.Shared;
using GV.CounterSketch.Shared.Pricing.Enums;
using GV.CounterSketch.Studio.Utilities.Helpers;
using GalaSoft.MvvmLight;
using Localization;

namespace GV.CounterSketch.Studio.ViewModel;

[Serializable]
public class GemGroup : ViewModelBase, ICloneable
{
	private string _agtaInformation;

	private double _carat;

	private double _customerMarkedUpCost;

	private double _customerMarkup;

	private string _error;

	private int _estimateQualityLevel;

	private string _gemGroupSource = Strings.Purchase_Stone_s;

	private List<Gem> _gemsInGroup;

	private bool _isNotInStock;

	private bool _isRedBox;

	private GemMaterial _material;

	private List<string> _meleeSourceOptions;

	private string _stoneColor;

	private ObservableCollection<string> _stoneQualities;

	private string _stoneQuality;

	private string _stoneType;

	private double _stullerCost;

	private string _stullerId;

	private string _type;

	private string _value;

	private string _stullerDescription;

	private string _stullerImage;

	[XmlIgnore]
	public string Details
	{
		get
		{
			if (string.IsNullOrEmpty(StullerId))
			{
				return "---";
			}
			return StullerId + " - " + AgtaInformation;
		}
	}

	[XmlIgnore]
	public List<string> GemCaratToolTip => new List<string>(GemsInGroup.GroupBy((Gem gemKey) => new { gemKey.Height, gemKey.Width }, (key, IEnumerable<Gem> grouping) => string.Format("{2} - {0} x {1}", key.Height, key.Width, grouping.Count())));

	[XmlIgnore]
	public bool HasErrors => !string.IsNullOrEmpty(Error);

	[XmlIgnore]
	public BitmapImage Image => GemHelper.GetImageForGems(Material);

	public string StullerImage
	{
		get
		{
			return _stullerImage;
		}
		set
		{
			_stullerImage = value;
			((ObservableObject)this).RaisePropertyChanged("StullerImage");
		}
	}

	public bool IsCustomerStoneSupplied => Source == Strings.Use_My_Stone_s;

	public bool IsNoStoneSupplied => Source == Strings.No_Stone_s_Supplied;

	[XmlIgnore]
	public bool IsDiamond => Material.ToString().ToLower().Contains("diamond");

	[XmlIgnore]
	public bool IsGemstone => !Material.ToString().ToLower().Contains("diamond");

	public string GemNumbersString => string.Join(",", GemNumbers);

	[XmlIgnore]
	public List<string> SourceOptions
	{
		get
		{
			if (UIHelper.KeyProfile.KeyProfileId == 2)
			{
				return new List<string> { Strings.Purchase_Stone_s };
			}
			return new List<string>
			{
				Strings.Purchase_Stone_s,
				Strings.Use_My_Stone_s,
				Strings.No_Stone_s_Supplied
			};
		}
	}

	public List<string> MeleeSourceOptions
	{
		get
		{
			List<OrderSummaryPrefItemModel> disabled = UIHelper.CounterSketchOptions.DisabledOrderSummaryItems.Where((OrderSummaryPrefItemModel _) => _.Section == "Melee Source").ToList();
			_meleeSourceOptions = ((disabled.Count == 0) ? SourceOptions : SourceOptions.Where((string _) => disabled.All((OrderSummaryPrefItemModel a) => a.Title != _)).ToList());
			return _meleeSourceOptions;
		}
	}

	public string EstimateQualityLevelName => StullerProductAPIHelper.GetUIValidDiamondQualities(Shape)[EstimateQualityLevel];

	public string StullerDescription
	{
		get
		{
			return _stullerDescription;
		}
		set
		{
			_stullerDescription = value;
			((ObservableObject)this).RaisePropertyChanged("StullerDescription");
			((ObservableObject)this).RaisePropertyChanged<string>((Expression<Func<string>>)(() => Details));
		}
	}

	public StudioSetType SetType { get; set; }

	public double Carat
	{
		get
		{
			return _carat;
		}
		set
		{
			_carat = value;
			((ObservableObject)this).RaisePropertyChanged("Carat");
		}
	}

	public List<string> GemNumbers { get; set; }

	public List<Gem> GemsInGroup
	{
		get
		{
			return _gemsInGroup;
		}
		set
		{
			_gemsInGroup = value;
			((ObservableObject)this).RaisePropertyChanged("GemsInGroup");
			((ObservableObject)this).RaisePropertyChanged<string>((Expression<Func<string>>)(() => Details));
		}
	}

	public string GemMountingType { get; set; }

	public double Height { get; set; }

	public int Id { get; set; }

	public bool IsNotInStock
	{
		get
		{
			return _isNotInStock;
		}
		set
		{
			_isNotInStock = value;
			((ObservableObject)this).RaisePropertyChanged("IsNotInStock");
			ForceValidation();
			if (value)
			{
				DebuggingHelper.PrintTimedOutput("Not In Stock at Quality: " + StoneQuality);
			}
		}
	}

	public bool HasCheckedInventory { get; set; }

	public bool IsCenterStone { get; set; }

	public bool IsLargeStone { get; set; }

	public bool IsMeleeStone { get; set; }

	public bool IsSideStone { get; set; }

	public double Length { get; set; }

	public GemMaterial Material
	{
		get
		{
			return _material;
		}
		set
		{
			_material = value;
			((ObservableObject)this).RaisePropertyChanged("Material");
			((ObservableObject)this).RaisePropertyChanged<bool>((Expression<Func<bool>>)(() => IsGemstone));
			((ObservableObject)this).RaisePropertyChanged<bool>((Expression<Func<bool>>)(() => IsDiamond));
			if (!IsDiamond)
			{
				IsRedBox = false;
			}
			((ObservableObject)this).RaisePropertyChanged<bool>((Expression<Func<bool>>)(() => IsRedBox));
		}
	}

	public int Quantity { get; set; }

	public string Shape { get; set; }

	public string Source
	{
		get
		{
			return _gemGroupSource;
		}
		set
		{
			_gemGroupSource = value;
			if (GemsInGroup != null)
			{
				GemsInGroup.ForEach(delegate(Gem _)
				{
					_.Source = value;
				});
			}
			if (IsCustomerStoneSupplied)
			{
				_isRedBox = false;
			}
			((ObservableObject)this).RaisePropertyChanged<string>((Expression<Func<string>>)(() => Source));
			((ObservableObject)this).RaisePropertyChanged<string>((Expression<Func<string>>)(() => Details));
			((ObservableObject)this).RaisePropertyChanged<string>((Expression<Func<string>>)(() => Type));
			((ObservableObject)this).RaisePropertyChanged<string>((Expression<Func<string>>)(() => StullerId));
			((ObservableObject)this).RaisePropertyChanged<string>((Expression<Func<string>>)(() => StoneType));
			((ObservableObject)this).RaisePropertyChanged<string>((Expression<Func<string>>)(() => StoneColor));
			((ObservableObject)this).RaisePropertyChanged<string>((Expression<Func<string>>)(() => StoneQuality));
			((ObservableObject)this).RaisePropertyChanged<bool>((Expression<Func<bool>>)(() => HasErrors));
			((ObservableObject)this).RaisePropertyChanged<bool>((Expression<Func<bool>>)(() => IsCustomerStoneSupplied));
			((ObservableObject)this).RaisePropertyChanged<bool>((Expression<Func<bool>>)(() => IsNoStoneSupplied));
			ForceValidation();
			if (!((ViewModelBase)this).IsInDesignMode && !UIHelper.IsOrderedDesignLoaded && ViewModelLocator.Current.OrderSummaryVM != null)
			{
				ViewModelLocator.Current.OrderSummaryVM.ReValidateOrder();
			}
		}
	}

	public string StoneColor
	{
		get
		{
			return _stoneColor;
		}
		set
		{
			_stoneColor = value;
			((ObservableObject)this).RaisePropertyChanged<string>((Expression<Func<string>>)(() => StoneColor));
			((ObservableObject)this).RaisePropertyChanged<string>((Expression<Func<string>>)(() => Details));
		}
	}

	public ObservableCollection<string> StoneQualities
	{
		get
		{
			if (!UIHelper.IsOrderedDesignLoaded)
			{
				if (((ViewModelBase)this).IsInDesignMode || IsQuickEstimateGroup)
				{
					_stoneQualities = new ObservableCollection<string>(StullerProductAPIHelper.GetUIValidDiamondQualities(Shape));
				}
				else
				{
					_stoneQualities = new ObservableCollection<string>(StullerProductAPIHelper.GetQualitiesFromMaterialName(Material.ToString(), Shape));
				}
			}
			else
			{
				_stoneQualities = new ObservableCollection<string>();
			}
			return _stoneQualities;
		}
		set
		{
			_stoneQualities = value;
			((ObservableObject)this).RaisePropertyChanged<ObservableCollection<string>>((Expression<Func<ObservableCollection<string>>>)(() => StoneQualities));
			((ObservableObject)this).RaisePropertyChanged<string>((Expression<Func<string>>)(() => Details));
		}
	}

	public string StoneQuality
	{
		get
		{
			return _stoneQuality;
		}
		set
		{
			_stoneQuality = value;
			if (GemsInGroup != null && StoneQuality != null)
			{
				GemsInGroup.ForEach(delegate(Gem _)
				{
					_.StoneQuality = value;
				});
			}
			((ObservableObject)this).RaisePropertyChanged<string>((Expression<Func<string>>)(() => StoneQuality));
			((ObservableObject)this).RaisePropertyChanged<string>((Expression<Func<string>>)(() => Details));
			HasCheckedInventory = false;
			ForceValidation();
		}
	}

	public string StoneType
	{
		get
		{
			return _stoneType;
		}
		set
		{
			_stoneType = value;
			((ObservableObject)this).RaisePropertyChanged<string>((Expression<Func<string>>)(() => StoneType));
			((ObservableObject)this).RaisePropertyChanged<string>((Expression<Func<string>>)(() => Type));
		}
	}

	public string StullerId
	{
		get
		{
			return _stullerId;
		}
		set
		{
			_stullerId = value;
			((ObservableObject)this).RaisePropertyChanged<string>((Expression<Func<string>>)(() => StullerId));
			((ObservableObject)this).RaisePropertyChanged<string>((Expression<Func<string>>)(() => Details));
		}
	}

	[XmlIgnore]
	public string Type
	{
		get
		{
			if (!string.IsNullOrEmpty(_stoneType))
			{
				return _stoneType;
			}
			return _type;
		}
		set
		{
			_type = value;
		}
	}

	public string Value
	{
		get
		{
			return _value;
		}
		set
		{
			_value = value;
			((ObservableObject)this).RaisePropertyChanged<string>((Expression<Func<string>>)(() => Details));
			((ObservableObject)this).RaisePropertyChanged<string>((Expression<Func<string>>)(() => Value));
		}
	}

	public double Width { get; set; }

	public string Error
	{
		get
		{
			return _error;
		}
		set
		{
			if (!(_error == value))
			{
				_error = value;
				((ObservableObject)this).RaisePropertyChanged("Error");
				((ObservableObject)this).RaisePropertyChanged<bool>((Expression<Func<bool>>)(() => HasErrors));
			}
		}
	}

	public string AgtaInformation
	{
		get
		{
			return _agtaInformation;
		}
		set
		{
			if (!(_agtaInformation == value))
			{
				_agtaInformation = value;
				((ObservableObject)this).RaisePropertyChanged<string>((Expression<Func<string>>)(() => AgtaInformation));
				((ObservableObject)this).RaisePropertyChanged<string>((Expression<Func<string>>)(() => Details));
			}
		}
	}

	public bool IsRedBox
	{
		get
		{
			return _isRedBox;
		}
		set
		{
			_isRedBox = value;
			((ObservableObject)this).RaisePropertyChanged("IsRedBox");
			ForceValidation();
			if (!((ViewModelBase)this).IsInDesignMode && !UIHelper.IsOrderedDesignLoaded && ViewModelLocator.Current.OrderSummaryVM != null)
			{
				ViewModelLocator.Current.OrderSummaryVM.ReValidateOrder();
			}
		}
	}

	public int EstimateQualityLevel
	{
		get
		{
			return _estimateQualityLevel;
		}
		set
		{
			_estimateQualityLevel = value;
			((ObservableObject)this).RaisePropertyChanged("EstimateQualityLevel");
			((ObservableObject)this).RaisePropertyChanged("EstimateQualityLevelName");
		}
	}

	public double CustomerMarkedUpCost
	{
		get
		{
			return _customerMarkedUpCost;
		}
		set
		{
			_customerMarkedUpCost = value;
			((ObservableObject)this).RaisePropertyChanged("CustomerMarkedUpCost");
		}
	}

	public double CustomerMarkup
	{
		get
		{
			return _customerMarkup;
		}
		set
		{
			_customerMarkup = value;
			((ObservableObject)this).RaisePropertyChanged("CustomerMarkup");
		}
	}

	public double StullerCost
	{
		get
		{
			return _stullerCost;
		}
		set
		{
			_stullerCost = value;
			((ObservableObject)this).RaisePropertyChanged("StullerCost");
		}
	}

	public bool IsQuickEstimateGroup { get; set; }

	public object Clone()
	{
		return MemberwiseClone();
	}

	public void ForceValidation()
	{
		if (UIHelper.IsOrderedDesignLoaded || IsNoStoneSupplied)
		{
			Error = null;
		}
		else if (HasCheckedInventory)
		{
			if (string.IsNullOrEmpty(StullerId))
			{
				Error = Strings.Items_not_in_stock_error;
			}
			else
			{
				Error = null;
			}
		}
	}
}
