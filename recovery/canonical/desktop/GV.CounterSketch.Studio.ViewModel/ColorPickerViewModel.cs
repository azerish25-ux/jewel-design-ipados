using System;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using GV.CounterSketch.Common.Utilities.Helpers;
using GV.CounterSketch.Studio.Utilities.Helpers;
using GalaSoft.MvvmLight;

namespace GV.CounterSketch.Studio.ViewModel;

public class ColorPickerViewModel : ViewModelBase
{
	private double _brightnessAdjustment;

	private BitmapImage _colorImage;

	private bool _isAdjustmentVisible = true;

	private double _saturationAdjustment;

	private Color _selectedColor = Colors.White;

	public double BrightnessAdjustment
	{
		get
		{
			return _brightnessAdjustment;
		}
		set
		{
			_brightnessAdjustment = value;
			RaisePropertyChanged("BrightnessAdjustment");
			RaisePropertyChanged("SelectedColor");
			RaisePropertyChanged("SelectedColorDisplaySwatch");
			if (!IsInitializing)
			{
				this.SelectedColorChanging?.Invoke(this, new EventArgs());
			}
		}
	}

	public BitmapImage ColorImage
	{
		get
		{
			return _colorImage;
		}
		set
		{
			_colorImage = value;
			RaisePropertyChanged("ColorImage");
		}
	}

	public bool IsAdjustmentVisible
	{
		get
		{
			return _isAdjustmentVisible;
		}
		set
		{
			_isAdjustmentVisible = value;
			RaisePropertyChanged("IsAdjustmentVisible");
		}
	}

	public bool IsInitializing { get; set; }

	public double SaturationAdjustment
	{
		get
		{
			return _saturationAdjustment;
		}
		set
		{
			_saturationAdjustment = value;
			RaisePropertyChanged("SaturationAdjustment");
			RaisePropertyChanged("SelectedColor");
			RaisePropertyChanged("SelectedColorDisplaySwatch");
			if (!IsInitializing)
			{
				this.SelectedColorChanging?.Invoke(this, new EventArgs());
			}
		}
	}

	public Color SelectedColor
	{
		get
		{
			return _selectedColor;
		}
		set
		{
			_selectedColor = value;
			RaisePropertyChanged("SelectedColor");
			RaisePropertyChanged("SelectedColorDisplaySwatch");
			if (!IsInitializing)
			{
				this.SelectedColorChanging?.Invoke(this, new EventArgs());
			}
		}
	}

	public event EventHandler SelectedColorChanging;

	public ColorPickerViewModel(Color color, double saturationAdjustment = 1.0, double brightnessAdjustment = 1.0)
	{
		SelectedColor = color;
		_saturationAdjustment = saturationAdjustment;
		_brightnessAdjustment = brightnessAdjustment;
		ColorImage = new BitmapImage(new Uri("pack://application:,,,/CounterSketch Studio;component/Images/HueGradient.png"));
		if (!base.IsInDesignMode)
		{
			IsInitializing = true;
			_saturationAdjustment = saturationAdjustment;
			_brightnessAdjustment = brightnessAdjustment;
			IsInitializing = false;
		}
	}

	public ColorPickerViewModel(Color color)
		: this(color, ThemeHelper.GetSaturationFromColor(color), ThemeHelper.GetBrightnessFromColor(color))
	{
	}

	public ColorPickerViewModel()
		: this(UIHelper.KeyProfile.HighlightAndAccent)
	{
	}

	public void UpdateSelectedColor(Color color)
	{
		SelectedColor = color;
	}
}
