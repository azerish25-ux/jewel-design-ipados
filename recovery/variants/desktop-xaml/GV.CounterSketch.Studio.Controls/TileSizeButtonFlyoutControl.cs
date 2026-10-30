using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Markup;
using DevExpress.Xpf.LayoutControl;
using GV.CounterSketch.Common.Annotations;
using GV.CounterSketch.Studio.Model;
using GV.CounterSketch.Studio.ViewModel;
using GalaSoft.MvvmLight;

namespace GV.CounterSketch.Studio.Controls;

public partial class TileSizeButtonFlyoutControl : UserControl, INotifyPropertyChanged, IComponentConnector
{
	public static readonly DependencyProperty CurrentTileSizeProperty = DependencyProperty.Register("CurrentTileSize", typeof(TileSize), typeof(TileSizeButtonFlyoutControl), new FrameworkPropertyMetadata(TileSize.Large, OnTileSizeChanged));

	public static readonly DependencyProperty IsOpenProperty = DependencyProperty.Register("IsOpen", typeof(bool), typeof(TileSizeButtonFlyoutControl), new PropertyMetadata(false));

	private bool _isSmall;

	private bool _isWide;

	private bool _isMedium;

	private bool _isLarge;

	public TileSize CurrentTileSize
	{
		get
		{
			return (TileSize)GetValue(CurrentTileSizeProperty);
		}
		set
		{
			SetValue(CurrentTileSizeProperty, value);
		}
	}

	public bool IsOpen
	{
		get
		{
			return (bool)GetValue(IsOpenProperty);
		}
		set
		{
			SetValue(IsOpenProperty, value);
		}
	}

	public bool IsSmall
	{
		get
		{
			return _isSmall;
		}
		set
		{
			if (value != _isSmall)
			{
				_isSmall = value;
				OnPropertyChanged("IsSmall");
			}
		}
	}

	public bool IsMedium
	{
		get
		{
			return _isMedium;
		}
		set
		{
			if (value != _isMedium)
			{
				_isMedium = value;
				OnPropertyChanged("IsMedium");
			}
		}
	}

	public bool IsLarge
	{
		get
		{
			return _isLarge;
		}
		set
		{
			if (value != _isLarge)
			{
				_isLarge = value;
				OnPropertyChanged("IsLarge");
			}
		}
	}

	public bool IsWide
	{
		get
		{
			return _isWide;
		}
		set
		{
			if (value != _isWide)
			{
				_isWide = value;
				OnPropertyChanged("IsWide");
			}
		}
	}

	public event PropertyChangedEventHandler PropertyChanged;

	private static void OnTileSizeChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
	{
		if (d is TileSizeButtonFlyoutControl tileSizeButtonFlyoutControl)
		{
			UpdateSizeBindings((e.NewValue as TileSize?) ?? TileSize.Large, tileSizeButtonFlyoutControl);
			tileSizeButtonFlyoutControl.IsOpen = false;
			if (tileSizeButtonFlyoutControl.DataContext != null && ViewModelLocator.Current.TileViewDesignsVM.IsInEditMode)
			{
				((TileItem)tileSizeButtonFlyoutControl.DataContext).CustomHeight = 0.0;
				((TileItem)tileSizeButtonFlyoutControl.DataContext).CustomWidth = 0.0;
				((TileItem)tileSizeButtonFlyoutControl.DataContext).FixCustomSize();
			}
		}
	}

	public TileSizeButtonFlyoutControl()
	{
		InitializeComponent();
		if (ViewModelBase.IsInDesignModeStatic)
		{
			CurrentTileSize = TileSize.Small;
		}
		UpdateSizeBindings(CurrentTileSize, this);
	}

	[NotifyPropertyChangedInvocator]
	protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
	{
		this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
	}

	private void Button_Click(object sender, RoutedEventArgs e)
	{
		IsOpen = !IsOpen;
	}

	private void LargeButton_OnClick(object sender, RoutedEventArgs e)
	{
		CurrentTileSize = TileSize.ExtraLarge;
	}

	private void MediumButton_OnClick(object sender, RoutedEventArgs e)
	{
		CurrentTileSize = TileSize.Small;
	}

	private void SmallButton_OnClick(object sender, RoutedEventArgs e)
	{
		CurrentTileSize = TileSize.ExtraSmall;
	}

	private void TileSizeButtonFlyoutControl_OnMouseLeave(object sender, MouseEventArgs e)
	{
	}

	private static void UpdateSizeBindings(TileSize newSize, TileSizeButtonFlyoutControl control)
	{
		switch (newSize)
		{
		case TileSize.ExtraSmall:
			control.IsSmall = true;
			control.IsMedium = false;
			control.IsWide = false;
			control.IsLarge = false;
			break;
		case TileSize.Small:
			control.IsSmall = false;
			control.IsMedium = true;
			control.IsWide = false;
			control.IsLarge = false;
			break;
		case TileSize.Large:
			control.IsSmall = false;
			control.IsMedium = false;
			control.IsWide = true;
			control.IsLarge = false;
			break;
		case TileSize.ExtraLarge:
			control.IsSmall = false;
			control.IsMedium = false;
			control.IsWide = false;
			control.IsLarge = true;
			break;
		}
	}

	private void WideButton_OnClick(object sender, RoutedEventArgs e)
	{
		CurrentTileSize = TileSize.Large;
	}
}
