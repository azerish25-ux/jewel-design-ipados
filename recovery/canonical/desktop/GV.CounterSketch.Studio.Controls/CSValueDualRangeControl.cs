using System;
using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Input;
using System.Windows.Markup;
using System.Windows.Media;
using System.Windows.Shapes;
using System.Windows.Threading;
using GV.CounterSketch.Shared.Model;
using GalaSoft.MvvmLight;
using Telerik.Windows.Controls;

namespace GV.CounterSketch.Studio.Controls;

public sealed class CSValueDualRangeControl : CSUIControl, IComponentConnector
{
	internal CSValueDualRangeControl theCSValueDualRangeControl;

	internal Grid gridDisplayImage;

	internal Rectangle rectImage;

	internal RadSlider sliderA;

	internal TextBlock txtTitle;

	internal Button btnSubMenu;

	internal Popup popupSubMenu;

	internal StackPanel stackSubMenuActions;

	internal Button btnResetControl;

	private bool _contentLoaded;

	public CSValueDualRangeControl()
	{
		if (GalaSoft.MvvmLight.ViewModelBase.IsInDesignModeStatic)
		{
			base.LocalUIControl = new UIControl
			{
				ControlName = "Design Time Name",
				EnglishControlName = "Design Time Name",
				EnglishCategoryName = "Category Name",
				Value1 = 2.0,
				Value2 = 4.0,
				Minimum1 = 1.0,
				Maximum1 = 8.0,
				Minimum2 = 1.0,
				Maximum2 = 8.0,
				Step1 = 0.5,
				SortIndex = 1
			};
			SetControl();
		}
	}

	public CSValueDualRangeControl(UIControl uiControl)
	{
		InitializeComponent();
		base.LocalUIControl = uiControl;
		base.DefaultControlValue = base.LocalUIControl.Value1;
		base.DefaultControlValue2 = base.LocalUIControl.Value2;
		SetControl();
	}

	public override void SetControl()
	{
		base.IsInitializing = true;
		if (base.LocalUIControl != null)
		{
			base.Title = base.LocalUIControl.ControlName;
			base.Value = base.LocalUIControl.Value1;
			base.Value2 = base.LocalUIControl.Value2;
			base.MinValue = base.LocalUIControl.Minimum1;
			base.MaxValue = base.LocalUIControl.Maximum1;
			base.MinValue2 = base.LocalUIControl.Minimum2;
			base.MaxValue2 = base.LocalUIControl.Maximum2;
			base.StepValue = base.LocalUIControl.Step1;
			base.SortIndex = base.LocalUIControl.SortIndex;
		}
		if (!string.IsNullOrEmpty(base.PathToIconImage))
		{
			DrawingBrush drawingBrush = (DrawingBrush)rectImage.TryFindResource(base.PathToIconImage);
			if (drawingBrush != null)
			{
				rectImage.Fill = drawingBrush;
			}
		}
		base.OriginalStep = base.StepValue;
		base.SetControl();
		base.IsInitializing = false;
	}

	public override void UpdateUIControlSource(UIControl uiControl)
	{
		base.IsUpdatingControlSource = true;
		base.UpdateUIControlSource(uiControl);
		base.Title = base.LocalUIControl.ControlName;
		base.Value = base.LocalUIControl.Value1;
		base.Value2 = base.LocalUIControl.Value2;
		base.MinValue = base.LocalUIControl.Minimum1;
		base.MaxValue = base.LocalUIControl.Maximum1;
		base.MinValue2 = base.LocalUIControl.Minimum2;
		base.MaxValue2 = base.LocalUIControl.Maximum2;
		base.StepValue = base.LocalUIControl.Step1;
		base.SortIndex = base.LocalUIControl.SortIndex;
		base.IsUpdatingControlSource = false;
	}

	private void BtnResetControlClick(object sender, RoutedEventArgs e)
	{
		popupSubMenu.IsOpen = false;
		base.Value = base.DefaultControlValue;
		base.Value2 = base.DefaultControlValue2;
		if (base.LocalUIControl != null && !base.IsInitializing)
		{
			base.LocalUIControl.Value1 = base.Value;
			base.LocalUIControl.Value2 = base.Value2;
			SetGraphValue(isQuickSet: false);
		}
	}

	private void BtnSubMenuClick(object sender, RoutedEventArgs e)
	{
		popupSubMenu.IsOpen = true;
		popupSubMenu.Focus();
	}

	private void PopupSubMenuMouseLeave(object sender, MouseEventArgs e)
	{
		popupSubMenu.IsOpen = false;
	}

	private void SliderMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
	{
		base.AllowSystemKeyCapture = true;
	}

	private void SliderMouseLeftButtonUp(object sender, MouseButtonEventArgs e)
	{
		if (base.LocalUIControl != null && !base.IsInitializing && !base.IsUpdatingControlSource)
		{
			base.Dispatcher.BeginInvoke((Action)delegate
			{
				base.LocalUIControl.Value1 = base.Value;
				base.LocalUIControl.Value2 = base.Value2;
				SetGraphValue(isQuickSet: false);
			}, DispatcherPriority.Background);
		}
		base.StepValue = base.OriginalStep;
		base.AllowSystemKeyCapture = false;
	}

	private void SliderPreviewKeyDown(object sender, KeyEventArgs e)
	{
		if (base.AllowSystemKeyCapture)
		{
			if ((e.Key == Key.LeftCtrl || e.Key == Key.RightCtrl) && base.StepValue == base.OriginalStep)
			{
				base.OriginalStep = base.StepValue;
				base.StepValue *= 10.0;
			}
			if ((e.Key == Key.LeftShift || e.Key == Key.RightShift) && base.StepValue == base.OriginalStep)
			{
				base.OriginalStep = base.StepValue;
				base.StepValue /= 10.0;
			}
		}
	}

	private void SliderPreviewKeyUp(object sender, KeyEventArgs e)
	{
		if (e.Key == Key.LeftCtrl || e.Key == Key.RightCtrl || e.Key == Key.LeftShift || e.Key == Key.RightShift)
		{
			base.StepValue = base.OriginalStep;
		}
	}

	private void SliderValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
	{
		if (base.LocalUIControl != null && !base.IsInitializing && !base.IsUpdatingControlSource)
		{
			base.Dispatcher.BeginInvoke((Action)delegate
			{
				base.LocalUIControl.Value1 = sliderA.SelectionStart;
				base.LocalUIControl.Value2 = sliderA.SelectionEnd;
				SetGraphValue(isQuickSet: true);
			}, DispatcherPriority.Background);
		}
	}

	[DebuggerNonUserCode]
	[GeneratedCode("PresentationBuildTasks", "4.0.0.0")]
	public void InitializeComponent()
	{
		if (!_contentLoaded)
		{
			_contentLoaded = true;
			Uri resourceLocator = new Uri("/CounterSketch Studio;component/controls/csvaluedualrangecontrol.xaml", UriKind.Relative);
			Application.LoadComponent(this, resourceLocator);
		}
	}

	[DebuggerNonUserCode]
	[GeneratedCode("PresentationBuildTasks", "4.0.0.0")]
	internal Delegate _CreateDelegate(Type delegateType, string handler)
	{
		return Delegate.CreateDelegate(delegateType, this, handler);
	}

	[DebuggerNonUserCode]
	[GeneratedCode("PresentationBuildTasks", "4.0.0.0")]
	[EditorBrowsable(EditorBrowsableState.Never)]
	void IComponentConnector.Connect(int connectionId, object target)
	{
		switch (connectionId)
		{
		case 1:
			theCSValueDualRangeControl = (CSValueDualRangeControl)target;
			break;
		case 2:
			gridDisplayImage = (Grid)target;
			break;
		case 3:
			rectImage = (Rectangle)target;
			break;
		case 4:
			sliderA = (RadSlider)target;
			sliderA.PreviewKeyDown += SliderPreviewKeyDown;
			sliderA.PreviewKeyUp += SliderPreviewKeyUp;
			sliderA.PreviewMouseLeftButtonDown += SliderMouseLeftButtonDown;
			sliderA.PreviewMouseLeftButtonUp += SliderMouseLeftButtonUp;
			sliderA.ValueChanged += SliderValueChanged;
			break;
		case 5:
			txtTitle = (TextBlock)target;
			break;
		case 6:
			btnSubMenu = (Button)target;
			btnSubMenu.Click += BtnSubMenuClick;
			break;
		case 7:
			popupSubMenu = (Popup)target;
			popupSubMenu.MouseLeave += PopupSubMenuMouseLeave;
			break;
		case 8:
			stackSubMenuActions = (StackPanel)target;
			break;
		case 9:
			btnResetControl = (Button)target;
			btnResetControl.Click += BtnResetControlClick;
			break;
		default:
			_contentLoaded = true;
			break;
		}
	}
}
