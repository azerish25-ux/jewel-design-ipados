using System;
using System.Linq.Expressions;
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
			((ObservableObject)this).RaisePropertyChanged("SelectedAttributeId");
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
			((ObservableObject)this).RaisePropertyChanged("SelectedAttribute");
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
			((ObservableObject)this).RaisePropertyChanged("ImagePath");
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
			((ObservableObject)this).RaisePropertyChanged("Title");
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
			((ObservableObject)this).RaisePropertyChanged("CustomHeight");
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
			((ObservableObject)this).RaisePropertyChanged("ErrorMessage");
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
			((ObservableObject)this).RaisePropertyChanged("CustomWidth");
		}
	}

	public TileSize Size
	{
		get
		{
			//IL_0001: Unknown result type (might be due to invalid IL or missing references)
			return _size;
		}
		set
		{
			//IL_000c: Unknown result type (might be due to invalid IL or missing references)
			//IL_000d: Unknown result type (might be due to invalid IL or missing references)
			ErrorMessage = string.Empty;
			_size = value;
			((ObservableObject)this).RaisePropertyChanged("Size");
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
			((ObservableObject)this).RaisePropertyChanged("IsGroupStart");
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
			((ObservableObject)this).RaisePropertyChanged("TileCommandType");
			((ObservableObject)this).RaisePropertyChanged("IsHitTestVisible");
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
		((ObservableObject)this).RaisePropertyChanged<bool>((Expression<Func<bool>>)(() => IsHitTestVisible));
	}

	public void FixCustomSize()
	{
		//IL_0031: Unknown result type (might be due to invalid IL or missing references)
		//IL_0036: Unknown result type (might be due to invalid IL or missing references)
		//IL_0037: Unknown result type (might be due to invalid IL or missing references)
		//IL_004d: Expected I4, but got Unknown
		ErrorMessage = string.Empty;
		if (CustomWidth == 0.0 || CustomHeight == 0.0)
		{
			TileSize size = Size;
			switch ((int)size)
			{
			case 0:
				CustomHeight = 70.0;
				CustomWidth = 70.0;
				break;
			case 1:
				CustomHeight = 150.0;
				CustomWidth = 150.0;
				break;
			case 2:
				CustomHeight = 150.0;
				CustomWidth = 310.0;
				break;
			case 3:
				CustomHeight = 310.0;
				CustomWidth = 310.0;
				break;
			}
		}
	}

	public void RefreshImagePath()
	{
		((ObservableObject)this).RaisePropertyChanged("ImagePath");
	}

	private double GetMinimumHeight()
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_0006: Unknown result type (might be due to invalid IL or missing references)
		//IL_0007: Unknown result type (might be due to invalid IL or missing references)
		//IL_001d: Expected I4, but got Unknown
		TileSize size = Size;
		return (int)size switch
		{
			0 => 70.0, 
			1 => 150.0, 
			2 => 150.0, 
			3 => 310.0, 
			_ => 70.0, 
		};
	}

	private double GetMinimumWidth()
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_0006: Unknown result type (might be due to invalid IL or missing references)
		//IL_0007: Unknown result type (might be due to invalid IL or missing references)
		//IL_001d: Expected I4, but got Unknown
		TileSize size = Size;
		return (int)size switch
		{
			0 => 70.0, 
			1 => 150.0, 
			2 => 310.0, 
			3 => 310.0, 
			_ => 70.0, 
		};
	}
}
