using System;
using System.CodeDom.Compiler;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using System.Windows.Markup;
using System.Windows.Media;
using System.Windows.Threading;
using GV.CounterSketch.Shared.Model;
using GV.CounterSketch.Shared.Model.Objects;
using GV.CounterSketch.Studio.Utilities.Helpers;
using GV.CounterSketch.Studio.ViewModel;
using Stuller.Web.API.v2.Models;

namespace GV.CounterSketch.Studio.Controls;

public sealed partial class CSValueGemSizeControl : CSUIControl, IComponentConnector
{
	public static readonly DependencyProperty IsManualStoneSizingEnabledProperty = DependencyProperty.Register("IsManualStoneSizingEnabled", typeof(bool), typeof(CSValueGemSizeControl));

	public static readonly DependencyProperty IsStoneAssignedEnabledProperty = DependencyProperty.Register("IsStoneAssignedEnabled", typeof(bool), typeof(CSValueGemSizeControl));

	public static readonly DependencyProperty MaxCaratProperty = DependencyProperty.Register("MaxCarat", typeof(double), typeof(CSValueGemSizeControl));

	public static readonly DependencyProperty MinCaratProperty = DependencyProperty.Register("MinCarat", typeof(double), typeof(CSValueGemSizeControl));

	public static readonly DependencyProperty StoneAssignmentVisibilityProperty = DependencyProperty.Register("StoneAssignmentVisibility", typeof(Visibility), typeof(CSValueGemSizeControl), new PropertyMetadata(Visibility.Collapsed));

	private bool _isStoneTrayEventWired;

	public bool IsManualStoneSizingEnabled
	{
		get
		{
			return (bool)GetValue(IsManualStoneSizingEnabledProperty);
		}
		set
		{
			SetValue(IsManualStoneSizingEnabledProperty, value);
		}
	}

	public bool IsStoneAssignedEnabled
	{
		get
		{
			return (bool)GetValue(IsStoneAssignedEnabledProperty);
		}
		set
		{
			SetValue(IsStoneAssignedEnabledProperty, value);
		}
	}

	public double MaxCarat
	{
		get
		{
			return (double)GetValue(MaxCaratProperty);
		}
		set
		{
			SetValue(MaxCaratProperty, value);
		}
	}

	public double MinCarat
	{
		get
		{
			return (double)GetValue(MinCaratProperty);
		}
		set
		{
			SetValue(MinCaratProperty, value);
		}
	}

	public Visibility StoneAssignmentVisibility
	{
		get
		{
			return (Visibility)GetValue(StoneAssignmentVisibilityProperty);
		}
		set
		{
			SetValue(StoneAssignmentVisibilityProperty, value);
		}
	}

	public CSValueGemSizeControl(UIControl uiControl)
	{
		InitializeComponent();
		base.LocalUIControl = uiControl;
		base.ProfilePolyLineColor = Colors.Gray;
		SetControl();
		base.DefaultControlValue = (int)base.LocalUIControl.Value1;
	}

	public override void SetControl()
	{
		base.IsInitializing = true;
		base.Title = base.LocalUIControl.ControlName;
		base.SortIndex = base.LocalUIControl.SortIndex;
		base.GemTableIndex = base.LocalUIControl.GemTableIndex;
		base.GemTableItemCount = base.LocalUIControl.GemTableItemCount;
		UpdateGemFields();
		UpdateCustomGemSizeVisualState();
		GetIconForUIControl();
		base.SetControl();
		if (base.LocalUIControl.IsLargeGemEnabled)
		{
			if (!_isStoneTrayEventWired)
			{
				ViewModelLocator.Current.StoneTrayVM.StoneChosenEvent += StoneTrayVM_StoneChosenEvent;
				_isStoneTrayEventWired = true;
			}
			if (ViewModelLocator.Current.StoneTrayVM.IsStoneAssigned)
			{
				UpdateControlFromAssignedCenterGem();
			}
		}
		base.IsInitializing = false;
	}

	public void UnwireFromStoneTrayViewModel()
	{
		if (_isStoneTrayEventWired)
		{
			ViewModelLocator.Current.StoneTrayVM.StoneChosenEvent -= StoneTrayVM_StoneChosenEvent;
			_isStoneTrayEventWired = false;
		}
	}

	public override void UpdateUIControlSource(UIControl uiControl)
	{
		base.IsUpdatingControlSource = true;
		base.UpdateUIControlSource(uiControl);
		UpdateCustomGemSizeVisualState();
		base.LocalUIControl.UpdateTarget = string.Empty;
		UpdateGemFields();
		base.IsUpdatingControlSource = false;
	}

	private void BtnResetControlClick(object sender, RoutedEventArgs e)
	{
		popupSubMenu.IsOpen = false;
		base.GemTableIndex = (int)base.DefaultControlValue2;
		base.LocalUIControl.Value1 = base.DefaultControlValue;
		base.LocalUIControl.GemTableIndex = (int)base.DefaultControlValue2;
		SetGraphValue(isQuickSet: false);
	}

	private void BtnSubMenuClick(object sender, RoutedEventArgs e)
	{
		popupSubMenu.IsOpen = true;
		popupSubMenu.Focus();
	}

	private void DisableCustomGemClick(object sender, RoutedEventArgs e)
	{
		base.LocalUIControl.IsCustomGemSize = false;
		UpdateCustomGemSizeVisualState();
	}

	private void PopupSubMenuMouseLeave(object sender, MouseEventArgs e)
	{
		popupSubMenu.IsOpen = false;
	}

	private void SldSizeMouseLeftButtonUp(object sender, MouseButtonEventArgs e)
	{
		if (!base.IsUpdatingControlSource && !base.IsInitializing && base.LocalUIControl != null)
		{
			base.Dispatcher.BeginInvoke((Action)delegate
			{
				base.LocalUIControl.GemTableIndex = (int)sldSize.Value;
				base.LocalUIControl.UpdateTarget = "Slider";
				SetGraphValue(isQuickSet: false);
			}, DispatcherPriority.Background);
		}
	}

	private void SldSizeValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
	{
		if (!base.IsInitializing && !base.IsUpdatingControlSource && base.LocalUIControl != null)
		{
			base.Dispatcher.BeginInvoke((Action)delegate
			{
				base.LocalUIControl.GemTableIndex = (int)sldSize.Value;
				base.GemCarat = CaratToQuarterCaratDisplay(base.LocalUIControl.GemTableSizeInfoList.ElementAt(base.LocalUIControl.GemTableIndex).Carat);
				base.GemHeight = base.LocalUIControl.GemTableSizeInfoList.ElementAt(base.LocalUIControl.GemTableIndex).Height;
				base.GemWidth = base.LocalUIControl.GemTableSizeInfoList.ElementAt(base.LocalUIControl.GemTableIndex).Width;
				base.GemDepth = base.LocalUIControl.GemTableSizeInfoList.ElementAt(base.LocalUIControl.GemTableIndex).Depth;
				base.LocalUIControl.UpdateTarget = "Slider";
				SetGraphValue(isQuickSet: true);
			}, DispatcherPriority.Background);
		}
	}

	private void StoneTrayVM_StoneChosenEvent(object sender, EventArgs e)
	{
		if (ViewModelLocator.Current.StoneTrayVM.IsStoneAssigned)
		{
			UpdateControlFromAssignedCenterGem();
		}
	}

	private void TxtDepthLostFocus(object sender, RoutedEventArgs e)
	{
		if (double.TryParse(txtDepth.Text, NumberStyles.Any, CultureInfo.InvariantCulture, out var result) && Math.Abs(base.LocalUIControl.GemDepth - result) > 0.0)
		{
			base.LocalUIControl.IsCustomGemSize = true;
			base.LocalUIControl.GemDepth = result;
			SetGraphValue(isQuickSet: false);
		}
	}

	private void TxtHeightLostFocus(object sender, RoutedEventArgs e)
	{
		if (double.TryParse(txtHeight.Text, NumberStyles.Any, CultureInfo.InvariantCulture, out var result) && Math.Abs(base.LocalUIControl.GemHeight - result) > 0.0)
		{
			base.LocalUIControl.IsCustomGemSize = true;
			base.LocalUIControl.GemHeight = result;
			SetGraphValue(isQuickSet: false);
		}
	}

	private void TxtPercentageLostFocus(object sender, RoutedEventArgs e)
	{
		if (double.TryParse(txtPercentage.Text, NumberStyles.Any, CultureInfo.InvariantCulture, out var result) && Math.Abs(base.LocalUIControl.GemPercentage - result) > 0.0)
		{
			base.LocalUIControl.IsCustomGemSize = true;
			base.LocalUIControl.GemPercentage = result;
			SetGraphValue(isQuickSet: false);
		}
	}

	private void TxtWidthLostFocus(object sender, RoutedEventArgs e)
	{
		if (double.TryParse(txtWidth.Text, NumberStyles.Any, CultureInfo.InvariantCulture, out var result) && Math.Abs(base.LocalUIControl.GemWidth - result) > 0.0)
		{
			base.LocalUIControl.IsCustomGemSize = true;
			base.LocalUIControl.GemWidth = result;
			SetGraphValue(isQuickSet: false);
		}
	}

	private void UpdateControlFromAssignedCenterGem()
	{
		Diamond providedStone = ViewModelLocator.Current.StoneTrayVM.StoneChosen;
		if (base.LocalUIControl.DataItems.FirstOrDefault((DataItem _) => StullerDiamondAPIHelper.ConvertStullerStoneShapeToGemvisionCounterSketchStoneShapeString(providedStone.Shape).Contains(_.VectorResourceName.Trim().ToLowerInvariant().Replace(" ", ""))) != null)
		{
			double num = (double)providedStone.Length;
			double num2 = (double)providedStone.Width;
			double.TryParse(providedStone.TotalDepth, out var result);
			base.LocalUIControl.GemHeight = num;
			base.LocalUIControl.GemWidth = ((Math.Abs(num2) < 0.1) ? num : num2);
			base.LocalUIControl.GemDepth = result;
			base.LocalUIControl.IsCustomGemSize = true;
			base.LocalUIControl.IsAssignedFromStone = true;
			SetGraphValue(isQuickSet: false);
		}
	}

	private void UpdateCustomGemSizeVisualState()
	{
		VisualStateManager.GoToState(this, base.LocalUIControl.IsCustomGemSize ? "CustomGemSizeState" : "AutomaticGemSizeState", useTransitions: false);
	}

	private void UpdateGemFields()
	{
		base.GemHeight = base.LocalUIControl.GemHeight;
		base.GemWidth = base.LocalUIControl.GemWidth;
		base.GemDepth = base.LocalUIControl.GemDepth;
		base.GemPercentage = base.LocalUIControl.GemPercentage;
		base.GemTableIndex = base.LocalUIControl.GemTableIndex;
		base.GemTableItemCount = base.LocalUIControl.GemTableItemCount;
		base.GemTableSizeInfoList = base.LocalUIControl.GemTableSizeInfoList;
		if (base.LocalUIControl.IsLargeGemEnabled && ViewModelLocator.Current.StoneTrayVM.IsStoneAssigned)
		{
			base.GemCarat = ViewModelLocator.Current.StoneTrayVM.StoneChosen.CaratWeight.ToString("#.##");
			IsManualStoneSizingEnabled = false;
			IsStoneAssignedEnabled = true;
		}
		else
		{
			base.GemCarat = CaratToQuarterCaratDisplay(base.LocalUIControl.GemCarat);
			IsManualStoneSizingEnabled = true;
			IsStoneAssignedEnabled = false;
		}
		StoneAssignmentVisibility = ((!base.LocalUIControl.IsLargeGemEnabled) ? Visibility.Collapsed : Visibility.Visible);
		if (base.LocalUIControl.GemTableSizeInfoList != null && base.LocalUIControl.GemTableSizeInfoList.Any())
		{
			MinCarat = base.LocalUIControl.GemTableSizeInfoList.Min((GemTableSizeInfo _) => _.Carat);
			MaxCarat = base.LocalUIControl.GemTableSizeInfoList.Max((GemTableSizeInfo _) => _.Carat);
		}
	}

	[DebuggerNonUserCode]
	[GeneratedCode("PresentationBuildTasks", "4.0.0.0")]
	internal Delegate _CreateDelegate(Type delegateType, string handler)
	{
		return Delegate.CreateDelegate(delegateType, this, handler);
	}
}
