using System;
using System.CodeDom.Compiler;
using System.Diagnostics;
using System.Timers;
using System.Windows;
using System.Windows.Input;
using System.Windows.Markup;
using System.Windows.Media;
using System.Windows.Threading;
using GV.CounterSketch.Shared.Model;
using GV.CounterSketch.Studio.Utilities.Helpers;
using Localization;

namespace GV.CounterSketch.Studio.Controls;

public sealed partial class CSValueRangeControl : CSUIControl, IComponentConnector
{
	private static Timer _updateDelayTimer;

	public CSValueRangeControl(UIControl uiControl)
	{
		InitializeComponent();
		base.LocalUIControl = uiControl;
		base.DefaultControlValue = double.NaN;
		SetControl();
		slider.AddHandler(UIElement.MouseLeftButtonUpEvent, new MouseButtonEventHandler(SliderMouseLeftButtonUp), handledEventsToo: true);
		slider.AddHandler(UIElement.MouseLeftButtonDownEvent, new MouseButtonEventHandler(SliderMouseLeftButtonDown), handledEventsToo: true);
	}

	public override void SetControl()
	{
		base.IsInitializing = true;
		base.Title = base.LocalUIControl.ControlName;
		base.Value = base.LocalUIControl.Value1;
		base.MinValue = base.LocalUIControl.Minimum1;
		base.MaxValue = base.LocalUIControl.Maximum1;
		base.StepValue = base.LocalUIControl.Step1;
		base.OriginalStep = base.StepValue;
		base.SortIndex = base.LocalUIControl.SortIndex;
		base.HasWarningMessage = base.LocalUIControl.HasMissingObject;
		base.WarningMessage = UntranslatedStrings.ControlRangeExpandedWarning;
		if (double.IsNaN(base.DefaultControlValue))
		{
			base.DefaultControlValue = base.Value;
		}
		if (!string.IsNullOrEmpty(base.PathToIconImage))
		{
			DrawingBrush drawingBrush = (DrawingBrush)rectImage.TryFindResource(base.PathToIconImage);
			if (drawingBrush != null)
			{
				rectImage.Fill = drawingBrush;
			}
		}
		base.SetControl();
		if (UIHelper.CounterSketchOptions.HideAdvancedStackOptions ?? UIHelper.KeyProfile.HideAdvancedStackOptions)
		{
			btnExpandControl.Visibility = Visibility.Collapsed;
		}
		base.IsInitializing = false;
	}

	public override void UpdateUIControlSource(UIControl uiControl)
	{
		base.IsUpdatingControlSource = true;
		base.UpdateUIControlSource(uiControl);
		base.Title = base.LocalUIControl.ControlName;
		base.Value = base.LocalUIControl.Value1;
		base.MinValue = base.LocalUIControl.Minimum1;
		base.MaxValue = base.LocalUIControl.Maximum1;
		base.StepValue = base.LocalUIControl.Step1;
		base.OriginalStep = base.StepValue;
		base.SortIndex = base.LocalUIControl.SortIndex;
		base.HasWarningMessage = base.LocalUIControl.HasMissingObject;
		base.IsUpdatingControlSource = false;
	}

	private void BtnExpandControlClick(object sender, RoutedEventArgs e)
	{
		popupSubMenu.IsOpen = false;
		if (base.LocalUIControl != null && !base.IsInitializing && !base.IsUpdatingControlSource)
		{
			UIHelper.CoreDataService.GraphItemAddUserDataValue(base.LocalUIControl.BlockInstanceGuid, "ExpandedSlider", "IsOn", "True", replaceCurrentValue: true);
			double num = base.LocalUIControl.Maximum1 - base.LocalUIControl.Minimum1;
			if (num < 0.01)
			{
				num = 0.01;
			}
			base.LocalUIControl.Minimum1 -= num;
			base.LocalUIControl.Maximum1 += num;
			SetGraphValue(isQuickSet: false);
			base.MinValue = base.LocalUIControl.Minimum1;
			base.MaxValue = base.LocalUIControl.Maximum1;
			base.HasWarningMessage = true;
		}
	}

	private void BtnResetControlClick(object sender, RoutedEventArgs e)
	{
		Reset();
	}

	public void Reset()
	{
		popupSubMenu.IsOpen = false;
		if (!double.IsNaN(base.DefaultControlValue))
		{
			base.Value = base.DefaultControlValue;
			double result = -1.0;
			double result2 = -1.0;
			string s = UIHelper.CoreDataService.GraphItemFindUserDataValue(base.LocalUIControl.BlockInstanceGuid, "ExpandedSlider", "OriginalMin");
			string s2 = UIHelper.CoreDataService.GraphItemFindUserDataValue(base.LocalUIControl.BlockInstanceGuid, "ExpandedSlider", "OriginalMax");
			double.TryParse(s, out result);
			double.TryParse(s2, out result2);
			if (result != -1.0)
			{
				base.MinValue = result;
			}
			if (result2 != -1.0)
			{
				base.MaxValue = result2;
			}
			base.HasWarningMessage = false;
			if (base.LocalUIControl != null && !base.IsInitializing)
			{
				base.LocalUIControl.Value1 = base.Value;
				base.LocalUIControl.Minimum1 = base.MinValue;
				base.LocalUIControl.Maximum1 = base.MaxValue;
				UIHelper.CoreDataService.GraphItemRemoveUserDataValue(base.LocalUIControl.BlockInstanceGuid, "ExpandedSlider", "IsOn");
				SetGraphValue(isQuickSet: false);
			}
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
		base.IsMouseDown = true;
	}

	private void SliderMouseLeftButtonUp(object sender, MouseButtonEventArgs e)
	{
		if (base.LocalUIControl != null && !base.IsInitializing && !base.IsUpdatingControlSource)
		{
			base.Dispatcher.BeginInvoke((Action)delegate
			{
				base.LocalUIControl.Value1 = slider.Value;
				_updateDelayTimer?.Stop();
				SetGraphValue(isQuickSet: false);
			}, DispatcherPriority.Background);
		}
		base.StepValue = base.OriginalStep;
		base.AllowSystemKeyCapture = false;
		base.IsMouseDown = false;
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
		if (base.LocalUIControl == null || base.IsInitializing || base.IsUpdatingControlSource)
		{
			return;
		}
		long num = 300L;
		if (_updateDelayTimer == null)
		{
			_updateDelayTimer = new Timer
			{
				AutoReset = false,
				Interval = num
			};
			_updateDelayTimer.Elapsed += delegate
			{
				SetGraphValue(isQuickSet: true);
				_updateDelayTimer.Stop();
			};
			_updateDelayTimer.Start();
		}
		else
		{
			_updateDelayTimer.Stop();
		}
		base.LocalUIControl.Value1 = slider.Value;
		_updateDelayTimer.Start();
	}

	[DebuggerNonUserCode]
	[GeneratedCode("PresentationBuildTasks", "4.0.0.0")]
	internal Delegate _CreateDelegate(Type delegateType, string handler)
	{
		return Delegate.CreateDelegate(delegateType, this, handler);
	}
}
