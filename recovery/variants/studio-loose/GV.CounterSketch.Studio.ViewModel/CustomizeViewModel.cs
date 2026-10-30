using System;
using System.Windows;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using GV.CounterSketch.Studio.Utilities.Helpers;
using GV.CounterSketch.Studio.View;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;

namespace GV.CounterSketch.Studio.ViewModel;

public class CustomizeViewModel : ViewModelBase
{
	private bool _isInDesignStackMode;

	private bool _isOrderSectionVisible;

	private double _orderSectionWidth;

	private BitmapImage _rhinoCaptureImage;

	private bool _isRhinoCaptureImageVisible;

	private int _rhinoRowSpan;

	private bool _isPartToolBoxVisible;

	private bool _isPartsListSectionVisible;

	private KeyTime _orderTransitionTime;

	private bool _isCustomizePartsListVisible;

	private WindowTileViewParts _windowParts;

	public bool IsCustomizePartsListVisible
	{
		get
		{
			return _isCustomizePartsListVisible;
		}
		set
		{
			_isCustomizePartsListVisible = value;
			((ObservableObject)this).RaisePropertyChanged("IsCustomizePartsListVisible");
		}
	}

	public double OrderSectionWidth
	{
		get
		{
			return _orderSectionWidth;
		}
		set
		{
			_orderSectionWidth = value;
			((ObservableObject)this).RaisePropertyChanged("OrderSectionWidth");
		}
	}

	public KeyTime OrderTransitionTime
	{
		get
		{
			return _orderTransitionTime;
		}
		set
		{
			if (!(value == _orderTransitionTime))
			{
				_orderTransitionTime = value;
				((ObservableObject)this).RaisePropertyChanged("OrderTransitionTime");
			}
		}
	}

	public bool IsOrderSectionVisible
	{
		get
		{
			return _isOrderSectionVisible;
		}
		set
		{
			_isOrderSectionVisible = value;
			((ObservableObject)this).RaisePropertyChanged("IsOrderSectionVisible");
		}
	}

	public bool HasPartsToolboxBeenShown { get; set; }

	public bool IsPartToolBoxVisible
	{
		get
		{
			return _isPartToolBoxVisible;
		}
		set
		{
			_isPartToolBoxVisible = value;
			((ObservableObject)this).RaisePropertyChanged("IsPartToolBoxVisible");
			if (_windowParts == null)
			{
				_windowParts = new WindowTileViewParts();
				_windowParts.Owner = Application.Current.MainWindow;
				_windowParts.WindowStartupLocation = WindowStartupLocation.CenterOwner;
				double height = ((SystemParameters.PrimaryScreenHeight >= 796.0) ? 796.0 : (SystemParameters.PrimaryScreenHeight - 50.0));
				double width = ((SystemParameters.PrimaryScreenWidth >= 1530.0) ? 1530.0 : (SystemParameters.PrimaryScreenWidth - 50.0));
				_windowParts.Height = height;
				_windowParts.Width = width;
			}
			if (value)
			{
				ViewModelLocator.Current.TileSearchMainVM.IsSearchingDesigns = false;
				HasPartsToolboxBeenShown = true;
				_windowParts.Show();
			}
			else if (!value && !((ViewModelBase)this).IsInDesignMode)
			{
				ViewModelLocator.Current.TileSearchMainVM.IsSearchingDesigns = true;
				_windowParts.Hide();
			}
		}
	}

	public bool IsPartsListSectionVisible
	{
		get
		{
			return _isPartsListSectionVisible;
		}
		set
		{
			_isPartsListSectionVisible = value;
			((ObservableObject)this).RaisePropertyChanged("IsPartsListSectionVisible");
			if (_isPartsListSectionVisible)
			{
				RhinoRowSpan = 1;
			}
			else
			{
				RhinoRowSpan = 2;
			}
		}
	}

	public RelayCommand CommandOpenOrder { get; set; }

	public RelayCommand CommandCloseOrder { get; set; }

	public bool IsInDesignStackMode
	{
		get
		{
			return _isInDesignStackMode;
		}
		set
		{
			_isInDesignStackMode = value;
			((ObservableObject)this).RaisePropertyChanged("IsInDesignStackMode");
		}
	}

	public bool IsRhinoCaptureImageVisible
	{
		get
		{
			return _isRhinoCaptureImageVisible;
		}
		set
		{
			_isRhinoCaptureImageVisible = value;
			((ObservableObject)this).RaisePropertyChanged("IsRhinoCaptureImageVisible");
		}
	}

	public int RhinoRowSpan
	{
		get
		{
			return _rhinoRowSpan;
		}
		set
		{
			_rhinoRowSpan = value;
			((ObservableObject)this).RaisePropertyChanged("RhinoRowSpan");
		}
	}

	public BitmapImage RhinoCaptureImage
	{
		get
		{
			return _rhinoCaptureImage;
		}
		set
		{
			_rhinoCaptureImage = value;
			((ObservableObject)this).RaisePropertyChanged("RhinoCaptureImage");
		}
	}

	public CustomizeViewModel()
	{
		if (!((ViewModelBase)this).IsInDesignMode)
		{
			InitializeCommands();
			IsOrderSectionVisible = false;
		}
		else
		{
			IsOrderSectionVisible = true;
		}
		OrderTransitionTime = KeyTime.FromTimeSpan(TimeSpan.FromSeconds(0.5));
		IsInDesignStackMode = true;
		if (UIHelper.KeyProfile.KeyProfileId == 2 || UIHelper.KeyProfile.KeyProfileId == 4)
		{
			IsCustomizePartsListVisible = false;
		}
		else
		{
			IsCustomizePartsListVisible = true;
		}
	}

	internal void Reset()
	{
		IsOrderSectionVisible = false;
		IsInDesignStackMode = true;
	}

	private void CloseOrder()
	{
		IsOrderSectionVisible = false;
	}

	private void InitializeCommands()
	{
		//IL_000e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0018: Expected O, but got Unknown
		//IL_0026: Unknown result type (might be due to invalid IL or missing references)
		//IL_0030: Expected O, but got Unknown
		CommandOpenOrder = new RelayCommand((Action)OpenOrder, false);
		CommandCloseOrder = new RelayCommand((Action)CloseOrder, false);
	}

	private void OpenOrder()
	{
		IsOrderSectionVisible = true;
	}
}
