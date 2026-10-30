using System;
using System.Runtime.Serialization;
using DevExpress.Xpf.LayoutControl;
using GV.CounterSketch.Studio.Model.Entities.CounterSketchLibrary;
using GV.CounterSketch.Studio.ViewModel;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;

namespace GV.CounterSketch.Studio.Model;

public class TileItem : ViewModelBase, ICloneable
{
	private string _imagePath;

	private bool _isGroupStart;

	private TileSize _size;

	private string _title;

	private TileCommandType _tileCommandType;

	private GV.CounterSketch.Studio.Model.Entities.CounterSketchLibrary.Attribute _selectedAttribute;

	private long _selectedAttributeId;

	private double _customWidth;

	private double _customHeight;

	private string _errorMessage;

	[IgnoreDataMember]
	public bool IsHitTestVisible
	{
		get
		{
			if (ParentViewModel == null)
			{
				return false;
			}
			if (ParentViewModel.IsInEditMode)
			{
				return true;
			}
			bool flag = TileCommandType == TileCommandType.CustomTile && !string.IsNullOrWhiteSpace(CustomURL);
			if (TileCommandType == TileCommandType.CustomTile)
			{
				if (!flag)
				{
					return SelectedAttributeId > 0;
				}
				return true;
			}
			return true;
		}
	}

	public long SelectedAttributeId
	{
		get
		{
			return _selectedAttributeId;
		}
		set
		{
			_selectedAttributeId = value;
			RaisePropertyChanged("SelectedAttributeId");
		}
	}

	[IgnoreDataMember]
	public GV.CounterSketch.Studio.Model.Entities.CounterSketchLibrary.Attribute SelectedAttribute
	{
		get
		{
			return _selectedAttribute;
		}
		set
		{
			_selectedAttribute = value;
			SelectedAttributeId = value.AttributeId;
			RaisePropertyChanged("SelectedAttribute");
		}
	}

	public int Id { get; set; }

	public string ImagePath
	{
		get
		{
			return _imagePath;
		}
		set
		{
			_imagePath = value;
			RaisePropertyChanged("ImagePath");
		}
	}

	public string Title
	{
		get
		{
			return _title;
		}
		set
		{
			_title = value;
			RaisePropertyChanged("Title");
		}
	}

	public double CustomHeight
	{
		get
		{
			return _customHeight;
		}
		set
		{
			ErrorMessage = string.Empty;
			double minimumHeight = GetMinimumHeight();
			if (value < minimumHeight)
			{
				ErrorMessage = $" Size limits Width to no less than {minimumHeight} ";
				value = minimumHeight;
			}
			_customHeight = value;
			RaisePropertyChanged("CustomHeight");
		}
	}

	public string ErrorMessage
	{
		get
		{
			return _errorMessage;
		}
		set
		{
			_errorMessage = value;
			RaisePropertyChanged("ErrorMessage");
		}
	}

	public double CustomWidth
	{
		get
		{
			return _customWidth;
		}
		set
		{
			ErrorMessage = string.Empty;
			double minimumWidth = GetMinimumWidth();
			if (value < minimumWidth)
			{
				ErrorMessage = $" Size limits Width to no less than {minimumWidth} ";
				value = minimumWidth;
			}
			_customWidth = value;
			RaisePropertyChanged("CustomWidth");
		}
	}

	public TileSize Size
	{
		get
		{
			return _size;
		}
		set
		{
			ErrorMessage = string.Empty;
			_size = value;
			RaisePropertyChanged("Size");
		}
	}

	public bool IsGroupStart
	{
		get
		{
			return _isGroupStart;
		}
		set
		{
			_isGroupStart = value;
			RaisePropertyChanged("IsGroupStart");
		}
	}

	public string CustomURL { get; set; }

	public TileCommandType TileCommandType
	{
		get
		{
			return _tileCommandType;
		}
		set
		{
			_tileCommandType = value;
			RaisePropertyChanged("TileCommandType");
			RaisePropertyChanged("IsHitTestVisible");
		}
	}

	[IgnoreDataMember]
	public object TileCommandObjectInput { get; set; }

	[IgnoreDataMember]
	public RelayCommand<object> TileCommandObject { get; set; }

	[IgnoreDataMember]
	public TileViewViewModelBase ParentViewModel { get; set; }

	public object Clone()
	{
		return MemberwiseClone();
	}

	public void CheckIsHitTestVisible()
	{
		RaisePropertyChanged(() => IsHitTestVisible);
	}

	public void FixCustomSize()
	{
		ErrorMessage = string.Empty;
		if (CustomWidth == 0.0 || CustomHeight == 0.0)
		{
			switch (Size)
			{
			case TileSize.ExtraSmall:
				CustomHeight = 70.0;
				CustomWidth = 70.0;
				break;
			case TileSize.Small:
				CustomHeight = 150.0;
				CustomWidth = 150.0;
				break;
			case TileSize.Large:
				CustomHeight = 150.0;
				CustomWidth = 310.0;
				break;
			case TileSize.ExtraLarge:
				CustomHeight = 310.0;
				CustomWidth = 310.0;
				break;
			}
		}
	}

	public void RefreshImagePath()
	{
		RaisePropertyChanged("ImagePath");
	}

	private double GetMinimumHeight()
	{
		return Size switch
		{
			TileSize.ExtraSmall => 70.0, 
			TileSize.Small => 150.0, 
			TileSize.Large => 150.0, 
			TileSize.ExtraLarge => 310.0, 
			_ => 70.0, 
		};
	}

	private double GetMinimumWidth()
	{
		return Size switch
		{
			TileSize.ExtraSmall => 70.0, 
			TileSize.Small => 150.0, 
			TileSize.Large => 310.0, 
			TileSize.ExtraLarge => 310.0, 
			_ => 70.0, 
		};
	}
}
