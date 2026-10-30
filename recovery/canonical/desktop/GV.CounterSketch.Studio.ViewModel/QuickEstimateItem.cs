using GV.CounterSketch.Shared;
using GV.CounterSketch.Studio.Utilities.Helpers;
using GalaSoft.MvvmLight;

namespace GV.CounterSketch.Studio.ViewModel;

public class QuickEstimateItem : ViewModelBase
{
	private bool _isOutOfStockLevel0;

	private bool _isOutOfStockLevel1;

	private bool _isOutOfStockLevel2;

	private bool _isOutOfStockLevel3;

	private int _metalEstimateLevel;

	private MetalMaterial _metalMaterial;

	private string _metalName;

	private double _priceMountingOnly;

	private string _priceMountingOnlyString;

	private double _priceWithGemLevel0;

	private decimal _priceWithGemLevel0Setting;

	private string _priceWithGemLevel0String;

	private double _priceWithGemLevel1;

	private decimal _priceWithGemLevel1Setting;

	private string _priceWithGemLevel1String;

	private double _priceWithGemLevel2;

	private decimal _priceWithGemLevel2Setting;

	private string _priceWithGemLevel2String;

	private double _priceWithGemLevel3;

	private decimal _priceWithGemLevel3Setting;

	private string _priceWithGemLevel3String;

	private string _weight;

	public bool? IsMetalRowEnabled
	{
		get
		{
			return !UIHelper.CounterSketchOptions.ExcludeQERowsList.Contains(MetalName);
		}
		set
		{
			if (!ViewModelBase.IsInDesignModeStatic && ViewModelLocator.Current.QuickEstimateVM.IsInEditMode)
			{
				if (value == true && UIHelper.CounterSketchOptions.ExcludeQERowsList.Contains(MetalName))
				{
					UIHelper.CounterSketchOptions.ExcludeQERowsList.Remove(MetalName);
				}
				else if (value != true && !UIHelper.CounterSketchOptions.ExcludeQERowsList.Contains(MetalName))
				{
					UIHelper.CounterSketchOptions.ExcludeQERowsList.Add(MetalName);
				}
				RaisePropertyChanged("IsMetalRowEnabled");
			}
		}
	}

	public bool IsOutOfStockLevel0
	{
		get
		{
			return _isOutOfStockLevel0;
		}
		set
		{
			_isOutOfStockLevel0 = value;
			RaisePropertyChanged("IsOutOfStockLevel0");
		}
	}

	public bool IsOutOfStockLevel1
	{
		get
		{
			return _isOutOfStockLevel1;
		}
		set
		{
			_isOutOfStockLevel1 = value;
			RaisePropertyChanged("IsOutOfStockLevel1");
		}
	}

	public bool IsOutOfStockLevel2
	{
		get
		{
			return _isOutOfStockLevel2;
		}
		set
		{
			_isOutOfStockLevel2 = value;
			RaisePropertyChanged("IsOutOfStockLevel2");
		}
	}

	public bool IsOutOfStockLevel3
	{
		get
		{
			return _isOutOfStockLevel3;
		}
		set
		{
			_isOutOfStockLevel3 = value;
			RaisePropertyChanged("IsOutOfStockLevel3");
		}
	}

	public int MetalEstimateLevel
	{
		get
		{
			return _metalEstimateLevel;
		}
		set
		{
			_metalEstimateLevel = value;
			RaisePropertyChanged("MetalEstimateLevel");
			RaisePropertyChanged("MetalName");
		}
	}

	public MetalMaterial MetalMaterial
	{
		get
		{
			return _metalMaterial;
		}
		set
		{
			_metalMaterial = value;
			RaisePropertyChanged("MetalMaterial");
		}
	}

	public string MetalName
	{
		get
		{
			return _metalName;
		}
		set
		{
			_metalName = value;
			RaisePropertyChanged("MetalName");
		}
	}

	public double PriceMountingOnly
	{
		get
		{
			return _priceMountingOnly;
		}
		set
		{
			_priceMountingOnly = value;
			RaisePropertyChanged("PriceMountingOnly");
		}
	}

	public double PriceWithGemLevel0
	{
		get
		{
			return _priceWithGemLevel0;
		}
		set
		{
			_priceWithGemLevel0 = value;
			RaisePropertyChanged("PriceWithGemLevel0");
		}
	}

	public double PriceWithGemLevel1
	{
		get
		{
			return _priceWithGemLevel1;
		}
		set
		{
			_priceWithGemLevel1 = value;
			RaisePropertyChanged("PriceWithGemLevel1");
		}
	}

	public double PriceWithGemLevel2
	{
		get
		{
			return _priceWithGemLevel2;
		}
		set
		{
			_priceWithGemLevel2 = value;
			RaisePropertyChanged("PriceWithGemLevel2");
		}
	}

	public double PriceWithGemLevel3
	{
		get
		{
			return _priceWithGemLevel3;
		}
		set
		{
			_priceWithGemLevel3 = value;
			RaisePropertyChanged("PriceWithGemLevel3");
		}
	}

	public decimal PriceWithGemLevel0Setting
	{
		get
		{
			return _priceWithGemLevel0Setting;
		}
		set
		{
			_priceWithGemLevel0Setting = value;
			RaisePropertyChanged("PriceWithGemLevel0Setting");
		}
	}

	public decimal PriceWithGemLevel1Setting
	{
		get
		{
			return _priceWithGemLevel1Setting;
		}
		set
		{
			_priceWithGemLevel1Setting = value;
			RaisePropertyChanged("PriceWithGemLevel1Setting");
		}
	}

	public decimal PriceWithGemLevel2Setting
	{
		get
		{
			return _priceWithGemLevel2Setting;
		}
		set
		{
			_priceWithGemLevel2Setting = value;
			RaisePropertyChanged("PriceWithGemLevel2Setting");
		}
	}

	public decimal PriceWithGemLevel3Setting
	{
		get
		{
			return _priceWithGemLevel3Setting;
		}
		set
		{
			_priceWithGemLevel3Setting = value;
			RaisePropertyChanged("PriceWithGemLevel3Setting");
		}
	}

	public string PriceMountingOnlyString
	{
		get
		{
			return _priceMountingOnlyString;
		}
		set
		{
			_priceMountingOnlyString = value;
			RaisePropertyChanged("PriceMountingOnlyString");
		}
	}

	public string PriceWithGemLevel0String
	{
		get
		{
			return _priceWithGemLevel0String;
		}
		set
		{
			_priceWithGemLevel0String = value;
			RaisePropertyChanged("PriceWithGemLevel0String");
		}
	}

	public string PriceWithGemLevel1String
	{
		get
		{
			return _priceWithGemLevel1String;
		}
		set
		{
			_priceWithGemLevel1String = value;
			RaisePropertyChanged("PriceWithGemLevel1String");
		}
	}

	public string PriceWithGemLevel2String
	{
		get
		{
			return _priceWithGemLevel2String;
		}
		set
		{
			_priceWithGemLevel2String = value;
			RaisePropertyChanged("PriceWithGemLevel2String");
		}
	}

	public string PriceWithGemLevel3String
	{
		get
		{
			return _priceWithGemLevel3String;
		}
		set
		{
			_priceWithGemLevel3String = value;
			RaisePropertyChanged("PriceWithGemLevel3String");
		}
	}

	public decimal QuotedCamAssemblyPrice { get; set; }

	public decimal QuotedCamPrice { get; set; }

	public decimal QuotedMetalPrice { get; set; }

	public decimal QuotedPolishPrice { get; set; }

	public decimal QuotedRingSizePrice { get; set; }

	public double QuotedSettingPrice { get; set; }

	public double TotalCaratWeight { get; set; }

	public string Weight
	{
		get
		{
			return _weight;
		}
		set
		{
			_weight = value;
			RaisePropertyChanged("Weight");
		}
	}
}
