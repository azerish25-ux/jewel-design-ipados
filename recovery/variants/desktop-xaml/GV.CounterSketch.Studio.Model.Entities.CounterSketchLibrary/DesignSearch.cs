using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Media.Imaging;
using System.Windows.Threading;
using GV.CounterSketch.Studio.Model.Objects.Enums;
using GV.CounterSketch.Studio.UserControls.PagedListBox;
using GV.CounterSketch.Studio.Utilities.Helpers;
using GV.CounterSketch.Studio.ViewModel;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;

namespace GV.CounterSketch.Studio.Model.Entities.CounterSketchLibrary;

public class DesignSearch : INotifyPropertyChanged, IPagedListBoxItem
{
	private BitmapSource _designImageSource;

	private bool _isSearchFavorite;

	private bool _isSelected;

	private long _lastDesignId;

	private int _lastGem1;

	private int _lastGem2;

	private int _lastGem3;

	private ImageType _lastImageType;

	private int _lastImageWidth;

	private int _lastMetal1;

	private int _lastMetal2;

	private long _lastVersion;

	private bool _mainImageChanged;

	private bool _isMarked;

	private bool _isMouseOverCheckBox;

	private DateTime _lastMarkedDateTime;

	private readonly DispatcherTimer _isMouseOverDelayTimer;

	public long DesignId { get; set; }

	public string SPINumber { get; set; }

	public string SeriesNumber { get; set; }

	public DateTime? LastUpdateDate { get; set; }

	public long? PrimaryDesignComponentId { get; set; }

	public long? ComponentId { get; set; }

	public string DefaultStyle { get; set; }

	public string DefaultStyleElement { get; set; }

	public string SecondaryStyleElement { get; set; }

	public string PrimaryDesignComponentCenterGemSetting { get; set; }

	public long? IsIncludedWithBaseDesigns { get; set; }

	public long? Is3CDesign { get; set; }

	public string Designer { get; set; }

	public long? IsBaseDesign { get; set; }

	public long? IsVariantDesign { get; set; }

	public long? IsFreeHandPart { get; set; }

	public long? IsFreeHandPartWithGem { get; set; }

	public long? IsStaticFreeHandPart { get; set; }

	public long? IsNewDesign { get; set; }

	public string StullerBaseNo { get; set; }

	public long? DefaultSettingTypeOrder { get; set; }

	public long? DefaultStyleElementOrder { get; set; }

	public long? DefaultStyleOrder { get; set; }

	public long? SecondaryStyleElementOrder { get; set; }

	public decimal? CenterGemMinLength { get; set; }

	public decimal? CenterGemMaxLength { get; set; }

	public decimal? CenterGemMinWidth { get; set; }

	public decimal? CenterGemMaxWidth { get; set; }

	public long Version { get; set; }

	public virtual Design Design { get; set; }

	public bool ShowNewRibbons
	{
		get
		{
			if (ViewModelBase.IsInDesignModeStatic)
			{
				return true;
			}
			return !UIHelper.CounterSketchOptions.HideNEWRibbons;
		}
	}

	public int MaterialGem1 => ViewModelLocator.Current.StudioMaterialSelectionVM.GemCenterMaterial.SearchAttributeId;

	public int MaterialGem2 => ViewModelLocator.Current.StudioMaterialSelectionVM.GemSideMaterial.SearchAttributeId;

	public int MaterialGem3 => ViewModelLocator.Current.StudioMaterialSelectionVM.GemAccentMaterial.SearchAttributeId;

	public int MaterialMetal1 => ViewModelLocator.Current.StudioMaterialSelectionVM.MetalPrimaryMaterial.SearchAttributeId;

	public int MaterialMetal2 => ViewModelLocator.Current.StudioMaterialSelectionVM.MetalSecondaryMaterial.SearchAttributeId;

	public bool IsImageLoaded => DesignImageSource != null;

	public BitmapSource DesignImageSource
	{
		get
		{
			if (ViewModelBase.IsInDesignModeStatic)
			{
				return UIHelper.SampleDesignImage;
			}
			return _designImageSource;
		}
		set
		{
			_designImageSource = value;
			_mainImageChanged = true;
			OnPropertyChanged("DesignImageSource");
			OnPropertyChanged("IsImageLoaded");
		}
	}

	public bool IsVisibleInScrollExtent { get; set; }

	public bool IsSearchFavorite
	{
		get
		{
			if (ViewModelBase.IsInDesignModeStatic)
			{
				return true;
			}
			return _isSearchFavorite;
		}
		set
		{
			_isSearchFavorite = value;
			OnPropertyChanged("IsSearchFavorite");
			if (!ViewModelBase.IsInDesignModeStatic && !ViewModelLocator.Current.TileSearchMainVM.TileSearchResultDesignsVM.IsSyncingResultsWithPortfolio)
			{
				if (_isSearchFavorite)
				{
					ProjectManagementHelper.AddDesignSearchFromLibraryToActiveProjectAsSearchFavorite(this);
				}
				else
				{
					ProjectManagementHelper.DeleteSearchFavoriteFromActiveProject(this);
				}
			}
		}
	}

	public bool IsMouseOverCheckBox
	{
		get
		{
			if (IsFreeHandPart == 1 || IsFreeHandPartWithGem == 1 || IsStaticFreeHandPart == 1)
			{
				return false;
			}
			return _isMouseOverCheckBox;
		}
		set
		{
			if (value)
			{
				_isMouseOverDelayTimer?.Stop();
				_isMouseOverCheckBox = value;
				OnPropertyChanged("IsMouseOverCheckBox");
			}
			else
			{
				_isMouseOverDelayTimer?.Start();
			}
		}
	}

	public RelayCommand CommandToggleMarkedStatus { get; set; }

	public bool IsMarked
	{
		get
		{
			return _isMarked;
		}
		set
		{
			_isMarked = value;
			OnPropertyChanged("IsMarked");
			IsMouseOverCheckBox = value;
			LastMarkedDateTime = DateTime.Now;
			if (!ViewModelBase.IsInDesignModeStatic && ViewModelLocator.Current.TileSearchMainVM.TileSearchResultDesignsVM != null)
			{
				ViewModelLocator.Current.TileSearchMainVM.TileSearchResultDesignsVM.UpdateDesignMarkedStatus();
			}
		}
	}

	public bool IsSelected
	{
		get
		{
			if (ViewModelBase.IsInDesignModeStatic)
			{
				return true;
			}
			return _isSelected;
		}
		set
		{
			_isSelected = value;
			OnPropertyChanged("IsSelected");
		}
	}

	public BitmapSource ImagePerspective { get; set; }

	public BitmapSource ImageSide { get; set; }

	public BitmapSource ImageTop { get; set; }

	public BitmapSource ImageFront { get; set; }

	public string Abbreviation { get; set; }

	public DateTime LastMarkedDateTime
	{
		get
		{
			return _lastMarkedDateTime;
		}
		set
		{
			_lastMarkedDateTime = value;
			OnPropertyChanged("LastMarkedDateTime");
		}
	}

	public string Description { get; set; }

	public string ImagePath
	{
		get
		{
			return null;
		}
		set
		{
		}
	}

	public bool IsFavorite { get; set; }

	public bool IsReadOnly
	{
		get
		{
			_ = ViewModelBase.IsInDesignModeStatic;
			return true;
		}
		set
		{
		}
	}

	public string PrimaryText { get; set; }

	public string SecondaryText { get; set; }

	public List<Tuple<decimal, decimal>> CenterSizes { get; set; }

	public event PropertyChangedEventHandler PropertyChanged;

	public DesignSearch()
	{
		if (!ViewModelBase.IsInDesignModeStatic)
		{
			_isMouseOverDelayTimer = new DispatcherTimer(DispatcherPriority.Background, Application.Current.Dispatcher)
			{
				Interval = new TimeSpan(0, 0, 0, 0, 750)
			};
			_isMouseOverDelayTimer.Tick += ClearMouseOverState;
			CommandToggleMarkedStatus = new RelayCommand(ToggleMarkedStatus);
		}
	}

	public DesignSearch GetShallowCopy()
	{
		return (DesignSearch)MemberwiseClone();
	}

	public void SetIsMarkedWithoutSearchResultUpdate(bool marked)
	{
		_isMarked = marked;
		OnPropertyChanged("SetIsMarkedWithoutSearchResultUpdate");
		IsMouseOverCheckBox = marked;
	}

	public void ClearMarked()
	{
		_isMouseOverDelayTimer?.Stop();
		_isMarked = false;
		OnPropertyChanged("IsMarked");
		_isMouseOverCheckBox = false;
		OnPropertyChanged("IsMouseOverCheckBox");
	}

	public void ExportImage(string folder)
	{
		if (folder == null)
		{
			return;
		}
		string text = null;
		try
		{
			string path = $"{SPINumber}_{MaterialMetal1}_{MaterialMetal2}_{MaterialGem1}_{MaterialGem2}_{MaterialGem3}.png";
			if (!Directory.Exists(folder))
			{
				Directory.CreateDirectory(folder);
			}
			text = string.Format(Path.Combine(folder, path));
			using FileStream stream = new FileStream(text, FileMode.Create);
			PngBitmapEncoder pngBitmapEncoder = new PngBitmapEncoder();
			pngBitmapEncoder.Frames.Add(BitmapFrame.Create(DesignImageSource));
			pngBitmapEncoder.Save(stream);
		}
		catch (Exception ex)
		{
			GlobalErrorHandler.WriteWarningToEventLog("Error Exporting Image from Search: " + text + "\n" + ex);
		}
	}

	public void ToggleMarkedStatus()
	{
		IsMarked = !IsMarked;
	}

	public void UpdateAllImageTypes()
	{
		if (!HaveAllImagesBeenGenerated() || ImageFront == null || ImageSide == null || ImageTop == null || ImagePerspective == null)
		{
			Tuple<BitmapImage, BitmapImage, BitmapImage, BitmapImage> extraImageTypeSources = LibraryGImageHelper.GetExtraImageTypeSources(MaterialMetal1, MaterialMetal2, MaterialGem1, MaterialGem2, MaterialGem3, DesignId, Version);
			ImageFront = extraImageTypeSources.Item1;
			ImageSide = extraImageTypeSources.Item2;
			ImageTop = extraImageTypeSources.Item3;
			ImagePerspective = extraImageTypeSources.Item4;
			_mainImageChanged = false;
		}
	}

	public void UpdateImageSource()
	{
		if (!HasImageBeenUpdated())
		{
			GlobalErrorHandler.WriteInfoToEventLog($"Updating Search Image: Gem Material = {MaterialGem1}");
			DesignImageSource = LibraryGImageHelper.GetImageSource(UIHelper.CurrentSearchResultImageType, MaterialMetal1, MaterialMetal2, MaterialGem1, MaterialGem2, MaterialGem3, DesignId, Version, UIHelper.CounterSketchOptions.SearchResultDesignWidth);
			_lastImageType = UIHelper.CurrentSearchResultImageType;
			_lastMetal1 = MaterialMetal1;
			_lastMetal2 = MaterialMetal2;
			_lastGem1 = MaterialGem1;
			_lastGem2 = MaterialGem2;
			_lastGem3 = MaterialGem3;
			_lastDesignId = DesignId;
			_lastVersion = Version;
			_lastImageWidth = UIHelper.CounterSketchOptions.SearchResultDesignWidth;
		}
	}

	protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
	{
		this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
	}

	private void ClearMouseOverState(object sender, EventArgs e)
	{
		_isMouseOverDelayTimer?.Stop();
		if (!IsMarked)
		{
			_isMouseOverCheckBox = false;
			OnPropertyChanged("IsMouseOverCheckBox");
		}
	}

	private bool HasImageBeenUpdated()
	{
		if (UIHelper.CurrentSearchResultImageType == _lastImageType && MaterialMetal1 == _lastMetal1 && MaterialMetal2 == _lastMetal2 && MaterialGem1 == _lastGem1 && MaterialGem2 == _lastGem2 && MaterialGem3 == _lastGem3 && DesignId == _lastDesignId && Version == _lastVersion)
		{
			return _lastImageWidth == UIHelper.CounterSketchOptions.SearchResultDesignWidth;
		}
		return false;
	}

	private bool HaveAllImagesBeenGenerated()
	{
		return !_mainImageChanged;
	}
}
