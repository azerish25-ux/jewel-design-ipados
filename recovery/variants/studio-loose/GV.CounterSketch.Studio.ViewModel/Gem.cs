using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
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
using Stuller.Web.API.v2.Models;

namespace GV.CounterSketch.Studio.ViewModel;

[Serializable]
public class Gem : ViewModelBase
{
	private string _agtaInformation;

	private Diamond _assignedDiamond;

	private double _carat;

	private double _customerMarkedUpCost;

	private double _customerMarkup;

	private string _error;

	private string _gemNumber;

	private string _gemSource = Strings.Purchase_Stone_s;

	private int _groupId;

	private double _height;

	private string _imageURL;

	private bool _isCenterStone;

	private bool _isLargeStone;

	private bool _isMeleeStone;

	private bool _isNotInStock;

	private bool _isRedBox;

	private bool _isSideStone;

	private List<string> _largeStoneSourceOptions;

	private double _length;

	private GemMaterial _material;

	private List<string> _meleeSourceOptions;

	private int _quantity;

	private string _shape;

	private ObservableCollection<string> _stoneQualities;

	private string _stoneQuality;

	private string _stoneType;

	private double _stullerCost;

	private string _stullerId;

	private string _type;

	private string _value;

	private string _stullerDescription;

	private string _stullerImage;

	public string Details
	{
		get
		{
			if (Source == Strings.Purchase_Stone_s)
			{
				if (IsLargeStone)
				{
					if (!IsRedBox)
					{
						if (!string.IsNullOrEmpty(StullerId))
						{
							return $"{Height:0.00} x {Width:0.00} - {StullerId}";
						}
						return $"{Height:0.00} x {Width:0.00}";
					}
					if (AssignedDiamond != null)
					{
						CultureInfo provider = new CultureInfo(UIHelper.CounterSketchOptions.CurrentCultureCode);
						if (AssignedDiamond.ShowcasePrice != null)
						{
							decimal value = AssignedDiamond.ShowcasePrice.Value;
							return string.Format(provider, "{5:0.00} x {6:0.00} - {0}, {1:C}, {2}, {3}, {4}", AssignedDiamond.SerialNumber, value, AssignedDiamond.Color, AssignedDiamond.Clarity, AssignedDiamond.Make, Height, Width);
						}
						return string.Empty;
					}
				}
				return "---";
			}
			if (Source == Strings.Use_My_Stone_s)
			{
				if (!string.IsNullOrEmpty(Value))
				{
					return "$" + Value;
				}
				return "---";
			}
			return "---";
		}
	}

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

	[XmlIgnore]
	public bool IsDiamond => Material.ToString().ToLower() == "diamond";

	[XmlIgnore]
	public bool IsDiamondMaterial => Material.ToString().ToLower().Contains("diamond");

	[XmlIgnore]
	public bool IsDiamondAssigned => AssignedDiamond != null;

	[XmlIgnore]
	public bool IsGemstone => !Material.ToString().ToLower().Contains("diamond");

	public bool IsCustomerStoneSupplied => Source == Strings.Use_My_Stone_s;

	public bool IsNoStoneSupplied => Source == Strings.No_Stone_s_Supplied;

	[XmlIgnore]
	public List<string> SourceOptions
	{
		get
		{
			if (UIHelper.KeyProfile.KeyProfileId == 2)
			{
				return new List<string>
				{
					Strings.Purchase_Stone_s,
					Strings.No_Stone_s_Supplied
				};
			}
			return new List<string>
			{
				Strings.Purchase_Stone_s,
				Strings.Use_My_Stone_s,
				Strings.No_Stone_s_Supplied
			};
		}
	}

	public List<string> LargeStoneSourceOptions
	{
		get
		{
			List<OrderSummaryPrefItemModel> disabled = UIHelper.CounterSketchOptions.DisabledOrderSummaryItems.Where((OrderSummaryPrefItemModel _) => _.Section == "Large Stones Source").ToList();
			_largeStoneSourceOptions = ((disabled.Count == 0) ? SourceOptions : SourceOptions.Where((string _) => disabled.All((OrderSummaryPrefItemModel a) => a.Title != _)).ToList());
			return _largeStoneSourceOptions;
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

	public Diamond AssignedDiamond
	{
		get
		{
			return _assignedDiamond;
		}
		set
		{
			_assignedDiamond = value;
			if (_assignedDiamond != null)
			{
				Width = Convert.ToDouble(_assignedDiamond.Width);
				Height = Convert.ToDouble(_assignedDiamond.Length);
				Carat = (double)_assignedDiamond.CaratWeight;
				StullerId = _assignedDiamond.SerialNumber.ToString();
				IsRedBox = true;
				IsNotInStock = false;
			}
			if (!((ViewModelBase)this).IsInDesignMode)
			{
				ViewModelLocator.Current.GemReportVM.AssignDiamondToGem(this, value);
			}
			if (value != null && value.Images != null && value.Images.Any())
			{
				ImageURL = value.Images[0].ThumbnailUrl;
				StullerImage = value.Images[0].FullUrl;
			}
			((ObservableObject)this).RaisePropertyChanged("AssignedDiamond");
			((ObservableObject)this).RaisePropertyChanged("Details");
			((ObservableObject)this).RaisePropertyChanged("IsDiamondAssigned");
			ForceValidation();
		}
	}

	public StudioSetType SetType { get; set; }

	public string GemMountingType { get; set; }

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

	public string GemNumber
	{
		get
		{
			return _gemNumber;
		}
		set
		{
			_gemNumber = value;
			((ObservableObject)this).RaisePropertyChanged("GemNumber");
		}
	}

	public int GroupId
	{
		get
		{
			return _groupId;
		}
		set
		{
			_groupId = value;
			((ObservableObject)this).RaisePropertyChanged("GroupId");
		}
	}

	public double Height
	{
		get
		{
			return _height;
		}
		set
		{
			_height = value;
			((ObservableObject)this).RaisePropertyChanged("Height");
		}
	}

	public string ImageURL
	{
		get
		{
			return _imageURL;
		}
		set
		{
			_imageURL = value;
			((ObservableObject)this).RaisePropertyChanged("ImageURL");
		}
	}

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
		}
	}

	public bool IsCenterStone
	{
		get
		{
			return _isCenterStone;
		}
		set
		{
			_isCenterStone = value;
			((ObservableObject)this).RaisePropertyChanged("IsCenterStone");
		}
	}

	[XmlIgnore]
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
			if (!value && !((ViewModelBase)this).IsInDesignMode && !UIHelper.IsOrderedDesignLoaded)
			{
				StoneQuality = StoneQualities.FirstOrDefault();
				if (ViewModelLocator.Current.OrderSummaryVM != null)
				{
					ViewModelLocator.Current.OrderSummaryVM.ResetQuote();
				}
				if (ViewModelLocator.Current.StoneTrayVM != null && IsDiamond)
				{
					ViewModelLocator.Current.StoneTrayVM.ProvidedGemToFit = this;
					ViewModelLocator.Current.StoneTrayVM.ClearAssignedStone();
				}
			}
			((ObservableObject)this).RaisePropertyChanged("Details");
			ForceValidation();
			if (!((ViewModelBase)this).IsInDesignMode && !UIHelper.IsOrderedDesignLoaded && ViewModelLocator.Current.OrderSummaryVM != null)
			{
				ViewModelLocator.Current.OrderSummaryVM.ReValidateOrder();
			}
		}
	}

	public bool IsLargeStone
	{
		get
		{
			return _isLargeStone;
		}
		set
		{
			_isLargeStone = value;
			((ObservableObject)this).RaisePropertyChanged("IsLargeStone");
		}
	}

	public bool IsMeleeStone
	{
		get
		{
			return _isMeleeStone;
		}
		set
		{
			_isMeleeStone = value;
			((ObservableObject)this).RaisePropertyChanged("IsMeleeStone");
		}
	}

	public bool IsSideStone
	{
		get
		{
			return _isSideStone;
		}
		set
		{
			_isSideStone = value;
			((ObservableObject)this).RaisePropertyChanged("IsSideStone");
		}
	}

	public double Length
	{
		get
		{
			return _length;
		}
		set
		{
			_length = value;
			((ObservableObject)this).RaisePropertyChanged("Length");
		}
	}

	public GemMaterial Material
	{
		get
		{
			return _material;
		}
		set
		{
			_material = value;
			((ObservableObject)this).RaisePropertyChanged<BitmapImage>((Expression<Func<BitmapImage>>)(() => Image));
			((ObservableObject)this).RaisePropertyChanged<GemMaterial>((Expression<Func<GemMaterial>>)(() => Material));
			((ObservableObject)this).RaisePropertyChanged<bool>((Expression<Func<bool>>)(() => IsDiamond));
			((ObservableObject)this).RaisePropertyChanged<bool>((Expression<Func<bool>>)(() => IsGemstone));
			((ObservableObject)this).RaisePropertyChanged<string>((Expression<Func<string>>)(() => Type));
			((ObservableObject)this).RaisePropertyChanged<ObservableCollection<string>>((Expression<Func<ObservableCollection<string>>>)(() => StoneQualities));
			((ObservableObject)this).RaisePropertyChanged<string>((Expression<Func<string>>)(() => StoneQuality));
			((ObservableObject)this).RaisePropertyChanged<string>((Expression<Func<string>>)(() => Details));
			if (!IsDiamond)
			{
				IsRedBox = false;
			}
			((ObservableObject)this).RaisePropertyChanged<bool>((Expression<Func<bool>>)(() => IsRedBox));
		}
	}

	public int Quantity
	{
		get
		{
			return _quantity;
		}
		set
		{
			_quantity = value;
			((ObservableObject)this).RaisePropertyChanged("Quantity");
		}
	}

	public string Shape
	{
		get
		{
			return _shape;
		}
		set
		{
			_shape = value;
			((ObservableObject)this).RaisePropertyChanged("Shape");
		}
	}

	public string Source
	{
		get
		{
			return _gemSource;
		}
		set
		{
			_gemSource = value;
			if (IsCustomerStoneSupplied)
			{
				_isRedBox = false;
			}
			((ObservableObject)this).RaisePropertyChanged<string>((Expression<Func<string>>)(() => Source));
			((ObservableObject)this).RaisePropertyChanged<string>((Expression<Func<string>>)(() => Details));
			((ObservableObject)this).RaisePropertyChanged<string>((Expression<Func<string>>)(() => Type));
			((ObservableObject)this).RaisePropertyChanged<string>((Expression<Func<string>>)(() => StullerId));
			((ObservableObject)this).RaisePropertyChanged<string>((Expression<Func<string>>)(() => StoneType));
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

	public ObservableCollection<string> StoneQualities
	{
		get
		{
			if (((ViewModelBase)this).IsInDesignMode)
			{
				_stoneQualities = new ObservableCollection<string>(StullerProductAPIHelper.GetUIValidDiamondQualities(Shape));
			}
			else if (!UIHelper.IsOrderedDesignLoaded)
			{
				_stoneQualities = new ObservableCollection<string>(StullerProductAPIHelper.GetQualitiesFromMaterialName(Material.ToString(), Shape));
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

	public string StoneQuality
	{
		get
		{
			if (_stoneQuality == null && _stoneQualities != null && _stoneQualities != null && _stoneQualities.Any())
			{
				_stoneQuality = _stoneQualities.FirstOrDefault();
				((ObservableObject)this).RaisePropertyChanged<string>((Expression<Func<string>>)(() => StoneQuality));
				((ObservableObject)this).RaisePropertyChanged<string>((Expression<Func<string>>)(() => Details));
				Error = null;
			}
			return _stoneQuality;
		}
		set
		{
			if (value != null)
			{
				_stoneQuality = value;
				((ObservableObject)this).RaisePropertyChanged<string>((Expression<Func<string>>)(() => StoneQuality));
				((ObservableObject)this).RaisePropertyChanged<string>((Expression<Func<string>>)(() => Details));
			}
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

	public string StullerId
	{
		get
		{
			return _stullerId;
		}
		set
		{
			_stullerId = value;
			((ObservableObject)this).RaisePropertyChanged<string>((Expression<Func<string>>)(() => Details));
			((ObservableObject)this).RaisePropertyChanged<string>((Expression<Func<string>>)(() => StullerId));
			ForceValidation();
			if (!((ViewModelBase)this).IsInDesignMode && !UIHelper.IsOrderedDesignLoaded && ViewModelLocator.Current.OrderSummaryVM != null)
			{
				ViewModelLocator.Current.OrderSummaryVM.ReValidateOrder();
			}
		}
	}

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
			((ObservableObject)this).RaisePropertyChanged("StoneTypes");
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
			ForceValidation();
			if (!((ViewModelBase)this).IsInDesignMode && !UIHelper.IsOrderedDesignLoaded && ViewModelLocator.Current.OrderSummaryVM != null)
			{
				ViewModelLocator.Current.OrderSummaryVM.ReValidateOrder();
			}
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
			}
		}
	}

	public void ForceValidation()
	{
		if (UIHelper.IsOrderedDesignLoaded || IsNoStoneSupplied)
		{
			Error = null;
			return;
		}
		if (IsLargeStone)
		{
			if (!IsDiamondAssigned && IsRedBox)
			{
				Error = Strings.Diamond_must_be_assigned_to_Estimate;
			}
			else
			{
				Error = null;
			}
		}
		if (!IsRedBox)
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

	public void SetIsRedBox(bool isRedBox)
	{
		_isRedBox = isRedBox;
	}
}
