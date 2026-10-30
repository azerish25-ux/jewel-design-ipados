using System;
using System.Windows.Media.Imaging;
using GV.CounterSketch.Shared;
using GV.CounterSketch.Studio.Model.Objects.Enums;
using GV.CounterSketch.Studio.UserControls.PagedListBox;
using GV.CounterSketch.Studio.Utilities.Helpers;
using GalaSoft.MvvmLight;

namespace GV.CounterSketch.Studio.ViewModel;

public class MaterialOptionItem : ViewModelBase, IPagedListBoxItem
{
	private bool _isAccent;

	private bool _isCenter;

	private bool _isDisableBoxVisible = true;

	private bool _isDisabled;

	private bool _isPrimary;

	private bool _isSecondary;

	private bool _isSide;

	private string _primaryText;

	public string Abbreviation
	{
		get
		{
			return CSMetalHelper.GetMetalCodeFromMetalMaterial(MetalMaterial);
		}
		set
		{
		}
	}

	public string ImagePath
	{
		get
		{
			if (IsGem)
			{
				return $"pack://application:,,,/CounterSketch Studio;component/Images/Search/FilterItems/GemColors/{Convert.ToInt32(GemMaterial)}_{Enum.GetName(GemMaterial.GetType(), GemMaterial)}.jpg";
			}
			return "pack://application:,,,/CounterSketch Studio;component/Images/Search/FilterItems/Metals/" + Enum.GetName(MetalMaterial.GetType(), MetalMaterial) + ".png";
		}
		set
		{
		}
	}

	public BitmapImage Image
	{
		get
		{
			if (IsGem)
			{
				return GemHelper.GetImageForGems(GemMaterial);
			}
			return MetalHelper.GetImageForMetals(MetalMaterial);
		}
	}

	public int SearchAttributeId
	{
		get
		{
			if (IsGem)
			{
				return (int)GemMaterial;
			}
			return (int)MetalMaterial;
		}
	}

	public bool IsDisabled
	{
		get
		{
			return _isDisabled;
		}
		set
		{
			if ((value && IsPrimary) || IsSecondary || IsCenter || IsAccent || IsSide)
			{
				_isDisabled = false;
				((ObservableObject)this).RaisePropertyChanged("IsDisabled");
			}
			else
			{
				_isDisabled = value;
				((ObservableObject)this).RaisePropertyChanged("IsDisabled");
			}
		}
	}

	public GemColorType ColorGroup { get; set; }

	public GemMaterial GemMaterial { get; set; }

	public bool IsGem { get; set; }

	public bool IsUKOnly { get; set; }

	public MetalMaterial MetalMaterial { get; set; }

	public bool RemoveFromUK { get; set; }

	public bool IsPrimary
	{
		get
		{
			return _isPrimary;
		}
		set
		{
			_isPrimary = value;
			IsDisabled = false;
			IsDisableBoxVisible = !value;
			((ObservableObject)this).RaisePropertyChanged("IsPrimary");
		}
	}

	public bool IsSecondary
	{
		get
		{
			return _isSecondary;
		}
		set
		{
			_isSecondary = value;
			IsDisabled = false;
			IsDisableBoxVisible = !value;
			((ObservableObject)this).RaisePropertyChanged("IsSecondary");
		}
	}

	public bool IsDisableBoxVisible
	{
		get
		{
			if (_isCenter || _isPrimary || _isSecondary || _isAccent || _isSide)
			{
				return false;
			}
			return _isDisableBoxVisible;
		}
		set
		{
			_isDisableBoxVisible = value;
			((ObservableObject)this).RaisePropertyChanged("IsDisableBoxVisible");
		}
	}

	public bool IsCenter
	{
		get
		{
			return _isCenter;
		}
		set
		{
			_isCenter = value;
			IsDisabled = false;
			IsDisableBoxVisible = !value;
			((ObservableObject)this).RaisePropertyChanged("IsCenter");
		}
	}

	public bool IsSide
	{
		get
		{
			return _isSide;
		}
		set
		{
			_isSide = value;
			IsDisabled = false;
			IsDisableBoxVisible = !value;
			((ObservableObject)this).RaisePropertyChanged("IsSide");
		}
	}

	public bool IsAccent
	{
		get
		{
			return _isAccent;
		}
		set
		{
			_isAccent = value;
			IsDisabled = false;
			IsDisableBoxVisible = !value;
			((ObservableObject)this).RaisePropertyChanged("IsAccent");
		}
	}

	public string Description { get; set; }

	public bool IsFavorite { get; set; }

	public bool IsReadOnly { get; set; }

	public string PrimaryText
	{
		get
		{
			if (IsCenter && ViewModelLocator.Current.StoneTrayVM.IsStoneAssigned)
			{
				return ViewModelLocator.Current.StoneTrayVM.StoneChosen.StoneType;
			}
			return _primaryText;
		}
		set
		{
			_primaryText = value;
			((ObservableObject)this).RaisePropertyChanged("PrimaryText");
		}
	}

	public string SecondaryText { get; set; }
}
