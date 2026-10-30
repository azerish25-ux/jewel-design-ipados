using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Drawing;
using System.Linq;
using System.Windows;
using System.Windows.Data;
using System.Windows.Media;
using GV.CounterSketch.Shared;
using GV.CounterSketch.Studio.Utilities.Helpers;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using Localization;

namespace GV.CounterSketch.Studio.ViewModel;

public class QuickEstimateViewModel : ViewModelBase
{
	private readonly object _lock;

	private string _columnName1;

	private string _columnName2;

	private string _columnName3;

	private string _columnName4;

	private string _columnName5;

	private string _columnName6;

	private string _designName;

	private int _estimateColorChoice;

	private DateTime _estimateDateTime;

	private Collection<QuickEstimateItem> _filteredQuickEstimateItems;

	private bool _isAllColorChoice;

	private bool _isInEditMode;

	private bool _isMissingGems;

	private bool _isMissingGemsDetailsVisible;

	private bool _isWhiteColorChoice;

	private bool _isYellowColorChoice;

	private Collection<QuickEstimateItem> _quickEstimateItems;

	private ImageSource _screenCaptureImage;

	private bool _showScreenCapture;

	public string MissingGemDetailsGHSI1 => GetMissingDetails(OutOfStockLevel1);

	public bool MissingGemDetailsGHSI1Visibile => OutOfStockLevel1.Count > 0;

	public string MissingGemDetailsGHSI23 => GetMissingDetails(OutOfStockLevel2);

	public bool MissingGemDetailsGHSI23Visibile => OutOfStockLevel2.Count > 0;

	public string MissingGemDetailsGVS => GetMissingDetails(OutOfStockLevel0);

	public bool MissingGemDetailsGVSVisibile => OutOfStockLevel0.Count > 0;

	public string MissingGemDetailsIJI1 => GetMissingDetails(OutOfStockLevel3);

	public bool MissingGemDetailsIJI1Visibile => OutOfStockLevel3.Count > 0;

	public bool IsColumn1Visible
	{
		get
		{
			if (!IsInEditMode)
			{
				return !UIHelper.CounterSketchOptions.ExcludeQEColumnsList.Contains(ColumnName1);
			}
			return true;
		}
	}

	public bool IsColumn2Visible
	{
		get
		{
			if (!IsInEditMode)
			{
				return !UIHelper.CounterSketchOptions.ExcludeQEColumnsList.Contains(ColumnName2);
			}
			return true;
		}
	}

	public bool IsColumn3Visible
	{
		get
		{
			if (!IsInEditMode)
			{
				return !UIHelper.CounterSketchOptions.ExcludeQEColumnsList.Contains(ColumnName3);
			}
			return true;
		}
	}

	public bool IsColumn4Visible
	{
		get
		{
			if (!IsInEditMode)
			{
				return !UIHelper.CounterSketchOptions.ExcludeQEColumnsList.Contains(ColumnName4);
			}
			return true;
		}
	}

	public bool IsColumn5Visible
	{
		get
		{
			if (!IsInEditMode)
			{
				return !UIHelper.CounterSketchOptions.ExcludeQEColumnsList.Contains(ColumnName5);
			}
			return true;
		}
	}

	public bool IsColumn6Visible
	{
		get
		{
			if (!IsInEditMode)
			{
				return !UIHelper.CounterSketchOptions.ExcludeQEColumnsList.Contains(ColumnName6);
			}
			return true;
		}
	}

	public bool IsColumn1VisibleChecked => !UIHelper.CounterSketchOptions.ExcludeQEColumnsList.Contains(ColumnName1);

	public bool IsColumn2VisibleChecked => !UIHelper.CounterSketchOptions.ExcludeQEColumnsList.Contains(ColumnName2);

	public bool IsColumn3VisibleChecked => !UIHelper.CounterSketchOptions.ExcludeQEColumnsList.Contains(ColumnName3);

	public bool IsColumn4VisibleChecked => !UIHelper.CounterSketchOptions.ExcludeQEColumnsList.Contains(ColumnName4);

	public bool IsColumn5VisibleChecked => !UIHelper.CounterSketchOptions.ExcludeQEColumnsList.Contains(ColumnName5);

	public bool IsColumn6VisibleChecked => !UIHelper.CounterSketchOptions.ExcludeQEColumnsList.Contains(ColumnName6);

	public RelayCommand CommandSetMetalToWhite { get; set; }

	public RelayCommand CommandSetMetalToYellow { get; set; }

	public RelayCommand CommandSetMetalToAll { get; set; }

	public RelayCommand<FrameworkElement> CommandPrintQuickEstimate { get; private set; }

	public RelayCommand CommandShowEditMode { get; set; }

	public RelayCommand CommandHideEditMode { get; set; }

	public string DesignName
	{
		get
		{
			return _designName;
		}
		set
		{
			_designName = value;
			RaisePropertyChanged("DesignName");
		}
	}

	public DateTime EstimateDateTime
	{
		get
		{
			return _estimateDateTime;
		}
		set
		{
			_estimateDateTime = value;
			RaisePropertyChanged("EstimateDateTime");
		}
	}

	public Collection<QuickEstimateItem> FilteredQuickEstimateItems
	{
		get
		{
			return _filteredQuickEstimateItems;
		}
		set
		{
			_filteredQuickEstimateItems = value;
			RaisePropertyChanged("FilteredQuickEstimateItems");
		}
	}

	public string GemsIncludedInEstimate { get; set; }

	public string GemsIncludedInEstimateWeight { get; set; }

	public string GemsNotIncludedInEstimate { get; set; }

	public string GemsNotIncludedInEstimateWeight { get; set; }

	public bool IncludeScreenshotWhenPrinting
	{
		get
		{
			return UIHelper.CounterSketchOptions.IncludeScreenshotWhenPrinting;
		}
		set
		{
			if (UIHelper.CounterSketchOptions.IncludeScreenshotWhenPrinting != value)
			{
				UIHelper.CounterSketchOptions.IncludeScreenshotWhenPrinting = value;
				UIHelper.CounterSketchOptions.WriteSettings();
			}
		}
	}

	public bool IsMissingGems
	{
		get
		{
			return _isMissingGems;
		}
		set
		{
			GlobalErrorHandler.WriteInfoToEventLog("Missing Gems from Estimating? " + value, 2);
			_isMissingGems = value;
			RaisePropertyChanged("IsMissingGems");
		}
	}

	public bool IsMissingGemsDetailsVisible
	{
		get
		{
			return _isMissingGemsDetailsVisible;
		}
		set
		{
			_isMissingGemsDetailsVisible = value;
			RaisePropertyChanged("IsMissingGemsDetailsVisible");
		}
	}

	public List<GemGroup> OutOfStockLevel0 { get; set; }

	public List<GemGroup> OutOfStockLevel1 { get; set; }

	public List<GemGroup> OutOfStockLevel2 { get; set; }

	public List<GemGroup> OutOfStockLevel3 { get; set; }

	public Collection<QuickEstimateItem> QuickEstimateItems
	{
		get
		{
			return _quickEstimateItems;
		}
		set
		{
			_quickEstimateItems = value;
			RaisePropertyChanged("QuickEstimateItems");
		}
	}

	public RelayCommand<string> CommandSetQEColumnVisibile { get; protected set; }

	public RelayCommand<string> CommandSetQEColumnCollapsed { get; protected set; }

	public string ColumnName1
	{
		get
		{
			return _columnName1;
		}
		set
		{
			_columnName1 = value;
			RaisePropertyChanged("ColumnName1");
		}
	}

	public string ColumnName2
	{
		get
		{
			return _columnName2;
		}
		set
		{
			_columnName2 = value;
			RaisePropertyChanged("ColumnName2");
		}
	}

	public string ColumnName3
	{
		get
		{
			return _columnName3;
		}
		set
		{
			_columnName3 = value;
			RaisePropertyChanged("ColumnName3");
		}
	}

	public string ColumnName4
	{
		get
		{
			return _columnName4;
		}
		set
		{
			_columnName4 = value;
			RaisePropertyChanged("ColumnName4");
		}
	}

	public string ColumnName5
	{
		get
		{
			return _columnName5;
		}
		set
		{
			_columnName5 = value;
			RaisePropertyChanged("ColumnName5");
		}
	}

	public string ColumnName6
	{
		get
		{
			return _columnName6;
		}
		set
		{
			_columnName6 = value;
			RaisePropertyChanged("ColumnName6");
		}
	}

	public int EstimateColorChoice
	{
		get
		{
			return _estimateColorChoice;
		}
		set
		{
			switch (value)
			{
			case 0:
				IsWhiteColorChoice = true;
				IsYellowColorChoice = false;
				IsAllColorChoice = false;
				break;
			case 1:
				IsWhiteColorChoice = false;
				IsYellowColorChoice = true;
				IsAllColorChoice = false;
				break;
			case 2:
				IsWhiteColorChoice = false;
				IsYellowColorChoice = false;
				IsAllColorChoice = true;
				break;
			}
			_estimateColorChoice = value;
			RaisePropertyChanged("EstimateColorChoice");
		}
	}

	public bool IsInEditMode
	{
		get
		{
			return _isInEditMode;
		}
		set
		{
			_isInEditMode = value;
			RaisePropertyChanged("IsInEditMode");
		}
	}

	public bool IsWhiteColorChoice
	{
		get
		{
			return _isWhiteColorChoice;
		}
		set
		{
			_isWhiteColorChoice = value;
			RaisePropertyChanged("IsWhiteColorChoice");
		}
	}

	public bool IsYellowColorChoice
	{
		get
		{
			return _isYellowColorChoice;
		}
		set
		{
			_isYellowColorChoice = value;
			RaisePropertyChanged("IsYellowColorChoice");
		}
	}

	public bool IsAllColorChoice
	{
		get
		{
			return _isAllColorChoice;
		}
		set
		{
			_isAllColorChoice = value;
			RaisePropertyChanged("IsAllColorChoice");
		}
	}

	public bool ShowScreenCapture
	{
		get
		{
			return _showScreenCapture;
		}
		set
		{
			_showScreenCapture = value;
			RaisePropertyChanged("ShowScreenCapture");
		}
	}

	public ImageSource ScreenCaptureImage
	{
		get
		{
			return _screenCaptureImage;
		}
		set
		{
			_screenCaptureImage = value;
			RaisePropertyChanged("ScreenCaptureImage");
		}
	}

	public QuickEstimateViewModel()
	{
		Reset();
		_lock = new object();
		BindingOperations.EnableCollectionSynchronization(QuickEstimateItems, _lock);
		if (base.IsInDesignMode)
		{
			DesignName = "SCS0012345-00";
			IsMissingGems = true;
			IsMissingGemsDetailsVisible = true;
			OutOfStockLevel0 = new List<GemGroup>
			{
				new GemGroup
				{
					Carat = 0.26,
					Height = 2.0,
					Width = 3.0,
					Length = 1.0,
					Shape = "Cushion",
					Quantity = 1
				},
				new GemGroup
				{
					Carat = 0.26,
					Height = 3.0,
					Width = 3.0,
					Length = 1.5,
					Shape = "Square Cushion",
					Quantity = 1
				},
				new GemGroup
				{
					Carat = 0.26,
					Height = 3.0,
					Width = 3.0,
					Length = 1.0,
					Shape = "Round",
					Quantity = 2
				}
			};
			OutOfStockLevel1 = new List<GemGroup>
			{
				new GemGroup
				{
					Carat = 0.26,
					Height = 3.0,
					Width = 3.0,
					Length = 1.0,
					Shape = "Round",
					Quantity = 1
				}
			};
			QuickEstimateItems.Add(new QuickEstimateItem
			{
				MetalEstimateLevel = 0,
				MetalMaterial = MetalMaterial.Platinum,
				MetalName = "Platinum",
				QuotedMetalPrice = 999.999m,
				PriceMountingOnly = 1024.123,
				PriceWithGemLevel0 = 1111.123,
				PriceWithGemLevel1 = 1222.123,
				PriceWithGemLevel2 = 1333.123,
				PriceWithGemLevel3 = 1444.123,
				Weight = "123.44 DWT (136.5g)",
				TotalCaratWeight = 1.99,
				IsOutOfStockLevel0 = true,
				IsMetalRowEnabled = true
			});
			QuickEstimateItems.Add(new QuickEstimateItem
			{
				MetalEstimateLevel = 1,
				MetalMaterial = MetalMaterial.WhiteGold,
				MetalName = "White Gold 22K",
				QuotedMetalPrice = 999.999m,
				PriceMountingOnly = 1024.123,
				PriceWithGemLevel0 = 1111.123,
				PriceWithGemLevel1 = 1222.123,
				PriceWithGemLevel2 = 1333.123,
				PriceWithGemLevel3 = 1444.123,
				Weight = "123.44 DWT (136.5g)",
				TotalCaratWeight = 1.99
			});
			QuickEstimateItems.Add(new QuickEstimateItem
			{
				MetalEstimateLevel = 2,
				MetalMaterial = MetalMaterial.YellowGold,
				MetalName = "Yellow Gold 14K",
				QuotedMetalPrice = 999.999m,
				PriceMountingOnly = 1024.123,
				PriceWithGemLevel0 = 1111.123,
				PriceWithGemLevel1 = 1222.123,
				PriceWithGemLevel2 = 1333.123,
				PriceWithGemLevel3 = 1444.123,
				Weight = "123.44 DWT (136.5g)",
				TotalCaratWeight = 1.99
			});
			QuickEstimateItems.Add(new QuickEstimateItem
			{
				MetalEstimateLevel = 0,
				MetalMaterial = MetalMaterial.Platinum,
				MetalName = "Platinum",
				QuotedMetalPrice = 999.999m,
				PriceMountingOnly = 1024.123,
				PriceWithGemLevel0 = 1111.123,
				PriceWithGemLevel1 = 1222.123,
				PriceWithGemLevel2 = 1333.123,
				PriceWithGemLevel3 = 1444.123,
				Weight = "123.44 DWT (136.5g)",
				TotalCaratWeight = 1.99,
				IsOutOfStockLevel0 = true
			});
			QuickEstimateItems.Add(new QuickEstimateItem
			{
				MetalEstimateLevel = 1,
				MetalMaterial = MetalMaterial.WhiteGold,
				MetalName = "White Gold 22K",
				QuotedMetalPrice = 999.999m,
				PriceMountingOnly = 1024.123,
				PriceWithGemLevel0 = 1111.123,
				PriceWithGemLevel1 = 1222.123,
				PriceWithGemLevel2 = 1333.123,
				PriceWithGemLevel3 = 1444.123,
				Weight = "123.44 DWT (136.5g)",
				TotalCaratWeight = 1.99
			});
			QuickEstimateItems.Add(new QuickEstimateItem
			{
				MetalEstimateLevel = 2,
				MetalMaterial = MetalMaterial.YellowGold,
				MetalName = "Yellow Gold 14K",
				QuotedMetalPrice = 999.999m,
				PriceMountingOnly = 1024.123,
				PriceWithGemLevel0 = 1111.123,
				PriceWithGemLevel1 = 1222.123,
				PriceWithGemLevel2 = 1333.123,
				PriceWithGemLevel3 = 1444.123,
				Weight = "123.44 DWT (136.5g)",
				TotalCaratWeight = 1.99
			});
			FilteredQuickEstimateItems = QuickEstimateItems;
			GemsIncludedInEstimate = "7";
			GemsIncludedInEstimateWeight = "1.77 ctw";
			GemsNotIncludedInEstimate = "1";
			GemsNotIncludedInEstimateWeight = ".98 ctw";
			EstimateColorChoice = 2;
			IsInEditMode = false;
		}
		else
		{
			InitializeCommands();
		}
		List<string> uIValidDiamondQualities = StullerProductAPIHelper.GetUIValidDiamondQualities();
		ColumnName1 = TranslationHelper.GetTranslatedString("Weights");
		ColumnName2 = uIValidDiamondQualities[0];
		ColumnName3 = uIValidDiamondQualities[1];
		ColumnName4 = uIValidDiamondQualities[2];
		ColumnName5 = uIValidDiamondQualities[3];
		ColumnName6 = TranslationHelper.GetTranslatedString("Mounting Only");
	}

	public void HideEditMode()
	{
		IsInEditMode = !IsInEditMode;
		UIHelper.CounterSketchOptions.WriteSettings();
		UpdateRowColumnChanges();
	}

	public void Reset()
	{
		QuickEstimateItems = new Collection<QuickEstimateItem>();
		IsInEditMode = false;
		EstimateDateTime = DateTime.Now;
		IsMissingGemsDetailsVisible = false;
		IsMissingGems = false;
	}

	public void ShowEditMode()
	{
		IsInEditMode = !IsInEditMode;
		UpdateRowColumnChanges();
	}

	public void UpdateBindings()
	{
		RaisePropertyChanged("MissingGemDetailsGVSVisibile");
		RaisePropertyChanged("MissingGemDetailsGVS");
		RaisePropertyChanged("MissingGemDetailsGHSI1Visibile");
		RaisePropertyChanged("MissingGemDetailsGHSI1");
		RaisePropertyChanged("MissingGemDetailsGHSI23Visibile");
		RaisePropertyChanged("MissingGemDetailsGHSI23");
		RaisePropertyChanged("MissingGemDetailsIJI1Visibile");
		RaisePropertyChanged("MissingGemDetailsIJI1");
		RaisePropertyChanged("IsMissingGems");
		FilterQuickEstimateItems();
	}

	private void FilterQuickEstimateItems()
	{
		if (UIHelper.CounterSketchOptions.EstimateColorChoice == 0)
		{
			UIHelper.CounterSketchOptions.ExcludeQERowsList = (from _ in QuickEstimateItems
				where _.MetalName.Contains("Yellow Gold")
				select _.MetalName).ToList();
			UIHelper.CounterSketchOptions.EstimateColorChoice = -1;
		}
		else if (UIHelper.CounterSketchOptions.EstimateColorChoice == 1)
		{
			UIHelper.CounterSketchOptions.ExcludeQERowsList = (from _ in QuickEstimateItems
				where _.MetalName.Contains("White Gold")
				select _.MetalName).ToList();
			UIHelper.CounterSketchOptions.EstimateColorChoice = -1;
		}
		FilteredQuickEstimateItems = new Collection<QuickEstimateItem>(QuickEstimateItems.Where((QuickEstimateItem _) => !UIHelper.CounterSketchOptions.ExcludeQERowsList.Contains(_.MetalName)).ToList());
	}

	private string GetMissingDetails(List<GemGroup> groups)
	{
		string output = string.Empty;
		groups.ForEach(delegate(GemGroup _)
		{
			output += $"{_.Quantity} : {_.Shape} - {_.Width}x{_.Height}, {_.Carat} ctw \n";
		});
		return output;
	}

	private void InitializeCommands()
	{
		CommandPrintQuickEstimate = new RelayCommand<FrameworkElement>(PrintQuickEstimate);
		CommandShowEditMode = new RelayCommand(ShowEditMode);
		CommandHideEditMode = new RelayCommand(HideEditMode);
		CommandSetQEColumnVisibile = new RelayCommand<string>(SetQEColumnVisibile);
		CommandSetQEColumnCollapsed = new RelayCommand<string>(SetQEColumnCollapsed);
	}

	private void PrintQuickEstimate(FrameworkElement element)
	{
		ShowScreenCapture = true;
		ScreenCaptureImage = null;
		if (IncludeScreenshotWhenPrinting)
		{
			ScreenCaptureImage = ImageHelper.ConvertBitmapToBitmapImage(UIHelper.CoreDataService.RequestScreenCaptureDefault(new System.Drawing.Size(400, 300), zoom: true, 0.0));
		}
		element.Print(saveOnly: false);
		ShowScreenCapture = false;
	}

	private void SetQEColumnCollapsed(string columnName)
	{
		UpdateQEColumnVisibility(columnName, isVisible: false);
	}

	private void SetQEColumnVisibile(string columnName)
	{
		UpdateQEColumnVisibility(columnName, isVisible: true);
	}

	private void UpdateQEColumnVisibility(string columnName, bool isVisible)
	{
		if (IsInEditMode)
		{
			if (isVisible && UIHelper.CounterSketchOptions.ExcludeQEColumnsList.Contains(columnName))
			{
				UIHelper.CounterSketchOptions.ExcludeQEColumnsList.Remove(columnName);
			}
			else if (!isVisible && !UIHelper.CounterSketchOptions.ExcludeQEColumnsList.Contains(columnName))
			{
				UIHelper.CounterSketchOptions.ExcludeQEColumnsList.Add(columnName);
			}
			UIHelper.CounterSketchOptions.WriteSettings();
		}
	}

	private void UpdateRowColumnChanges()
	{
		QuickEstimateItems.ToList().ForEach(delegate(QuickEstimateItem _)
		{
			_.IsMetalRowEnabled = UIHelper.CounterSketchOptions.ExcludeQERowsList.All((string e) => e != _.MetalName);
		});
		if (!IsInEditMode)
		{
			FilterQuickEstimateItems();
		}
		if (IsInEditMode)
		{
			FilteredQuickEstimateItems = QuickEstimateItems;
		}
		RaisePropertyChanged(() => IsColumn1Visible);
		RaisePropertyChanged(() => IsColumn2Visible);
		RaisePropertyChanged(() => IsColumn3Visible);
		RaisePropertyChanged(() => IsColumn4Visible);
		RaisePropertyChanged(() => IsColumn5Visible);
		RaisePropertyChanged(() => IsColumn6Visible);
		RaisePropertyChanged(() => IsColumn1VisibleChecked);
		RaisePropertyChanged(() => IsColumn2VisibleChecked);
		RaisePropertyChanged(() => IsColumn3VisibleChecked);
		RaisePropertyChanged(() => IsColumn4VisibleChecked);
		RaisePropertyChanged(() => IsColumn5VisibleChecked);
		RaisePropertyChanged(() => IsColumn6VisibleChecked);
	}
}
