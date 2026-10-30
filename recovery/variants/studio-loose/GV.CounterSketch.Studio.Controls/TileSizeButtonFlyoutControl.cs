using System;
using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Input;
using System.Windows.Markup;
using DevExpress.Xpf.LayoutControl;
using GV.CounterSketch.Common.Annotations;
using GV.CounterSketch.Studio.Model;
using GV.CounterSketch.Studio.ViewModel;
using GalaSoft.MvvmLight;

namespace GV.CounterSketch.Studio.Controls;

public class TileSizeButtonFlyoutControl : UserControl, INotifyPropertyChanged, IComponentConnector
{
	public static readonly DependencyProperty CurrentTileSizeProperty = DependencyProperty.Register("CurrentTileSize", typeof(TileSize), typeof(TileSizeButtonFlyoutControl), new FrameworkPropertyMetadata((object)(TileSize)2, OnTileSizeChanged));

	public static readonly DependencyProperty IsOpenProperty = DependencyProperty.Register("IsOpen", typeof(bool), typeof(TileSizeButtonFlyoutControl), new PropertyMetadata(false));

	private bool _isSmall;

	private bool _isWide;

	private bool _isMedium;

	private bool _isLarge;

	internal TileSizeButtonFlyoutControl TileSizeControl;

	internal Popup popup;

	internal Button SmallButton;

	internal Button MediumButton;

	internal Button WideButton;

	internal Button LargeButton;

	internal VisualStateGroup VisualStateGroup;

	internal VisualState Default;

	internal VisualState Open;

	private bool _contentLoaded;

	public TileSize CurrentTileSize
	{
		get
		{
			//IL_000b: Unknown result type (might be due to invalid IL or missing references)
			return (TileSize)GetValue(CurrentTileSizeProperty);
		}
		set
		{
			//IL_0006: Unknown result type (might be due to invalid IL or missing references)
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
		//IL_002b: Unknown result type (might be due to invalid IL or missing references)
		if (d is TileSizeButtonFlyoutControl tileSizeButtonFlyoutControl)
		{
			UpdateSizeBindings((TileSize)(((int?)(e.NewValue as TileSize?)) ?? 2), tileSizeButtonFlyoutControl);
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
		//IL_001b: Unknown result type (might be due to invalid IL or missing references)
		InitializeComponent();
		if (ViewModelBase.IsInDesignModeStatic)
		{
			CurrentTileSize = (TileSize)1;
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
		CurrentTileSize = (TileSize)3;
	}

	private void MediumButton_OnClick(object sender, RoutedEventArgs e)
	{
		CurrentTileSize = (TileSize)1;
	}

	private void SmallButton_OnClick(object sender, RoutedEventArgs e)
	{
		CurrentTileSize = (TileSize)0;
	}

	private void TileSizeButtonFlyoutControl_OnMouseLeave(object sender, MouseEventArgs e)
	{
	}

	private static void UpdateSizeBindings(TileSize newSize, TileSizeButtonFlyoutControl control)
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0016: Expected I4, but got Unknown
		switch ((int)newSize)
		{
		case 0:
			control.IsSmall = true;
			control.IsMedium = false;
			control.IsWide = false;
			control.IsLarge = false;
			break;
		case 1:
			control.IsSmall = false;
			control.IsMedium = true;
			control.IsWide = false;
			control.IsLarge = false;
			break;
		case 2:
			control.IsSmall = false;
			control.IsMedium = false;
			control.IsWide = true;
			control.IsLarge = false;
			break;
		case 3:
			control.IsSmall = false;
			control.IsMedium = false;
			control.IsWide = false;
			control.IsLarge = true;
			break;
		}
	}

	private void WideButton_OnClick(object sender, RoutedEventArgs e)
	{
		CurrentTileSize = (TileSize)2;
	}

	[DebuggerNonUserCode]
	[GeneratedCode("PresentationBuildTasks", "4.0.0.0")]
	public void InitializeComponent()
	{
		if (!_contentLoaded)
		{
			_contentLoaded = true;
			Uri resourceLocator = new Uri("/CounterSketch Studio;component/controls/tilesizebuttonflyoutcontrol.xaml", UriKind.Relative);
			Application.LoadComponent(this, resourceLocator);
		}
	}

	[DebuggerNonUserCode]
	[GeneratedCode("PresentationBuildTasks", "4.0.0.0")]
	[EditorBrowsable(EditorBrowsableState.Never)]
	void IComponentConnector.Connect(int connectionId, object target)
	{
		switch (connectionId)
		{
		case 1:
			TileSizeControl = (TileSizeButtonFlyoutControl)target;
			TileSizeControl.MouseLeave += TileSizeButtonFlyoutControl_OnMouseLeave;
			break;
		case 2:
			((Button)target).Click += Button_Click;
			break;
		case 3:
			popup = (Popup)target;
			break;
		case 4:
			SmallButton = (Button)target;
			SmallButton.Click += SmallButton_OnClick;
			break;
		case 5:
			MediumButton = (Button)target;
			MediumButton.Click += MediumButton_OnClick;
			break;
		case 6:
			WideButton = (Button)target;
			WideButton.Click += WideButton_OnClick;
			break;
		case 7:
			LargeButton = (Button)target;
			LargeButton.Click += LargeButton_OnClick;
			break;
		case 8:
			VisualStateGroup = (VisualStateGroup)target;
			break;
		case 9:
			Default = (VisualState)target;
			break;
		case 10:
			Open = (VisualState)target;
			break;
		default:
			_contentLoaded = true;
			break;
		}
	}
}
