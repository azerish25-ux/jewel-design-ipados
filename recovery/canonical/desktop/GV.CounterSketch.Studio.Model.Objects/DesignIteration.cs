using System.Windows.Media.Imaging;
using GV.CounterSketch.Studio.Model.Objects.Enums;
using GV.CounterSketch.Studio.UserControls.PagedListBox;
using GV.CounterSketch.Studio.Utilities.Helpers;
using GalaSoft.MvvmLight;

namespace GV.CounterSketch.Studio.Model.Objects;

public class DesignIteration : ViewModelBase, IPagedListBoxItem
{
	private string _description;

	private int _designId;

	private BitmapSource _designImageSource;

	private bool _imageLoading;

	private string _imagePath;

	private bool _isFavorite;

	private bool _isReadOnly;

	private bool _isSelected;

	private int _iterationId;

	private string _primaryText;

	private string _secondaryText;

	public string Abbreviation { get; set; }

	public int DesignId
	{
		get
		{
			return _designId;
		}
		set
		{
			if (value != _designId)
			{
				_designId = value;
				RaisePropertyChanged("DesignId");
			}
		}
	}

	public BitmapSource DesignImageSource
	{
		get
		{
			return _designImageSource;
		}
		set
		{
			_designImageSource = value;
			RaisePropertyChanged("DesignImageSource");
		}
	}

	public bool IsSelected
	{
		get
		{
			return _isSelected;
		}
		set
		{
			_isSelected = value;
			RaisePropertyChanged("IsSelected");
		}
	}

	public bool IsVisible { get; set; }

	public int MaterialGem1 { get; set; }

	public int MaterialGem2 { get; set; }

	public int MaterialGem3 { get; set; }

	public int MaterialMetal1 { get; set; }

	public int MaterialMetal2 { get; set; }

	public int IterationId
	{
		get
		{
			return _iterationId;
		}
		set
		{
			if (value != _iterationId)
			{
				_iterationId = value;
				RaisePropertyChanged("IterationId");
			}
		}
	}

	public string Description
	{
		get
		{
			return _description;
		}
		set
		{
			if (!(value == _description))
			{
				_description = value;
				RaisePropertyChanged("Description");
			}
		}
	}

	public string ImagePath
	{
		get
		{
			return _imagePath;
		}
		set
		{
			if (!(value == _imagePath))
			{
				_imagePath = value;
				RaisePropertyChanged("ImagePath");
			}
		}
	}

	public bool IsFavorite
	{
		get
		{
			return _isFavorite;
		}
		set
		{
			if (!value.Equals(_isFavorite))
			{
				_isFavorite = value;
				RaisePropertyChanged("IsFavorite");
			}
		}
	}

	public bool IsReadOnly
	{
		get
		{
			return _isReadOnly;
		}
		set
		{
			if (!value.Equals(_isReadOnly))
			{
				_isReadOnly = value;
				RaisePropertyChanged("IsReadOnly");
			}
		}
	}

	public string PrimaryText
	{
		get
		{
			return _primaryText;
		}
		set
		{
			if (!(value == _primaryText))
			{
				_primaryText = value;
				RaisePropertyChanged("PrimaryText");
			}
		}
	}

	public string SecondaryText
	{
		get
		{
			return _secondaryText;
		}
		set
		{
			if (!(value == _secondaryText))
			{
				_secondaryText = value;
				RaisePropertyChanged("SecondaryText");
			}
		}
	}

	public void NotifyImageSourceChanged()
	{
		RaisePropertyChanged("DesignImageSource");
	}

	public void UpdateImageSource()
	{
		DesignImageSource = LibraryGImageHelper.GetImageSource(ImageType.Perspective, MaterialMetal1, MaterialMetal2, MaterialGem1, MaterialGem2, MaterialGem3, DesignId, IterationId);
	}

	public void UpdateImageSourceNoNotify()
	{
		if (!_imageLoading)
		{
			_imageLoading = true;
			_designImageSource = LibraryGImageHelper.GetImageSource(ImageType.Perspective, MaterialMetal1, MaterialMetal2, MaterialGem1, MaterialGem2, MaterialGem3, DesignId, IterationId);
			_imageLoading = false;
		}
	}
}
