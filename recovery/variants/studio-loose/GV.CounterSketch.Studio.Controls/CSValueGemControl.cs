using System;
using System.CodeDom.Compiler;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Timers;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Input;
using System.Windows.Markup;
using System.Windows.Media;
using System.Windows.Shapes;
using System.Windows.Threading;
using GV.CounterSketch.Shared.Model;
using GV.CounterSketch.Shared.Model.Objects;
using GV.CounterSketch.Studio.Model.Objects;
using GV.CounterSketch.Studio.Utilities.Helpers;
using GV.CounterSketch.Studio.ViewModel;
using Stuller.Web.API.v2.Models;
using Telerik.Windows.Controls;

namespace GV.CounterSketch.Studio.Controls;

public sealed class CSValueGemControl : CSUIControl, IComponentConnector
{
	public static readonly DependencyProperty IsManualStoneSizingEnabledProperty = DependencyProperty.Register("IsManualStoneSizingEnabled", typeof(bool), typeof(CSValueGemControl));

	public static readonly DependencyProperty IsStoneAssignedEnabledProperty = DependencyProperty.Register("IsStoneAssignedEnabled", typeof(bool), typeof(CSValueGemControl));

	public static readonly DependencyProperty MaxCaratProperty = DependencyProperty.Register("MaxCarat", typeof(double), typeof(CSValueGemControl));

	public static readonly DependencyProperty MinCaratProperty = DependencyProperty.Register("MinCarat", typeof(double), typeof(CSValueGemControl));

	public static readonly DependencyProperty SelectedGemDataItemProperty = DependencyProperty.Register("SelectedGemDataIndex", typeof(int), typeof(CSValueGemControl));

	public static readonly DependencyProperty ShapeProperty = DependencyProperty.Register("Shape", typeof(string), typeof(CSValueGemControl));

	public static readonly DependencyProperty ShapeSelectionVisibilityProperty = DependencyProperty.Register("ShapeSelectionVisibility", typeof(Visibility), typeof(CSValueGemControl), new PropertyMetadata(Visibility.Visible));

	public static readonly DependencyProperty StoneAssignmentVisibilityProperty = DependencyProperty.Register("StoneAssignmentVisibility", typeof(Visibility), typeof(CSValueGemControl), new PropertyMetadata(Visibility.Collapsed));

	private static Timer _updateDelayTimer;

	private bool _isStoneTrayEventWired;

	internal CSValueGemControl CSValueGemUserControl;

	internal TextBlock txtTitle;

	internal Border borderSubMenu;

	internal Button btnSubMenu;

	internal TextBlock txtCarat;

	internal TextBlock txtSize;

	internal RadSlider sldSize;

	internal Button tglCustomGem;

	internal Grid gridDimensions;

	internal Path path;

	internal TextBlock textBlock;

	internal TextBox txtHeight;

	internal Path path1;

	internal TextBlock textBlockWidth;

	internal TextBox txtWidth;

	internal Path path2;

	internal TextBlock textBlock1;

	internal TextBox txtDepth;

	internal Path path3;

	internal TextBlock textBlock3;

	internal TextBox txtPercentage;

	internal Popup popupSubMenu;

	internal StackPanel stackSubMenuActions;

	internal Button btnResetControl;

	internal VisualStateGroup CustomGemSizeStates;

	internal VisualState CustomGemSizeState;

	internal VisualState AutomaticGemSizeState;

	private bool _contentLoaded;

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

	public int SelectedGemDataIndex
	{
		get
		{
			return (int)GetValue(SelectedGemDataItemProperty);
		}
		set
		{
			SetValue(SelectedGemDataItemProperty, value);
		}
	}

	public string Shape
	{
		get
		{
			return (string)GetValue(ShapeProperty);
		}
		set
		{
			SetValue(ShapeProperty, value);
		}
	}

	public Visibility ShapeSelectionVisibility
	{
		get
		{
			return (Visibility)GetValue(ShapeSelectionVisibilityProperty);
		}
		set
		{
			SetValue(ShapeSelectionVisibilityProperty, value);
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

	public CSValueGemControl(UIControl uiControl)
	{
		InitializeComponent();
		base.LocalUIControl = uiControl;
		base.ProfilePolyLineColor = Colors.Gray;
		SetControl();
		base.DefaultControlValue = (int)base.LocalUIControl.Value1;
		base.DefaultControlValue2 = base.LocalUIControl.GemTableIndex;
	}

	public override void SetControl()
	{
		base.IsInitializing = true;
		PopulateGems();
		base.Title = base.LocalUIControl.ControlName;
		base.SortIndex = base.LocalUIControl.SortIndex;
		base.GemTableIndex = base.LocalUIControl.GemTableIndex;
		base.GemTableItemCount = base.LocalUIControl.GemTableItemCount;
		SelectedGemDataIndex = (int)base.LocalUIControl.Value1;
		GetIconForUIControl();
		base.SetControl();
		UpdateGemFields();
		UpdateCustomGemSizeVisualState();
		if (base.LocalUIControl.IsLargeGemEnabled)
		{
			Application.Current.Dispatcher.InvokeAsync(delegate
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
				UpdateSelectedShapeFromShapeName();
			});
		}
		if (base.LocalUIControl.ImportGroupIdentifier != Guid.Empty)
		{
			Application.Current.Dispatcher.InvokeAsync(delegate
			{
				base.IsInitializing = true;
				Guid? guid = ViewModelLocator.Current.CustomizePartsListVM.ImportGroupsCollection.FirstOrDefault((ImportGroup _) => _.Identifier == base.LocalUIControl.ImportGroupIdentifier)?.CenterGemControlGuid;
				if (guid.HasValue)
				{
					Guid blockInstanceGuid = base.LocalUIControl.BlockInstanceGuid;
					Guid? guid2 = guid;
					if (blockInstanceGuid == guid2)
					{
						UpdateSelectedShapeFromShapeName(isPartAssignment: true);
					}
				}
				base.IsInitializing = false;
			});
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
		base.GemTableIndex = base.LocalUIControl.GemTableIndex;
		base.GemTableItemCount = base.LocalUIControl.GemTableItemCount;
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

	private void ButtonBase_OnClick(object sender, RoutedEventArgs e)
	{
		if (base.LocalUIControl.IsLargeGemEnabled && ViewModelLocator.Current.StoneTrayVM.IsStoneAssigned)
		{
			if (ViewModelLocator.Current.StoneTrayVM.StoneChosen != null)
			{
				_ = (double)ViewModelLocator.Current.StoneTrayVM.StoneChosen.CaratWeight;
			}
		}
		else
		{
			_ = base.LocalUIControl.GemCarat;
		}
		PunchoutHelper.ShowDiamondShopper();
	}

	private void DisableCustomGemClick(object sender, RoutedEventArgs e)
	{
		base.LocalUIControl.IsCustomGemSize = false;
		if (base.LocalUIControl.IsLargeGemEnabled && ViewModelLocator.Current.StoneTrayVM.IsStoneAssigned)
		{
			ViewModelLocator.Current.StoneTrayVM.ClearAssignedStoneCommand.Execute((object)null);
		}
		SetGraphValue(isQuickSet: false);
	}

	private void PopulateGems()
	{
		if (base.LocalUIControl.DataItems != null && base.LocalUIControl.DataItems.Any())
		{
			base.Gems = new ObservableCollection<GemDataItem>(base.LocalUIControl.DataItems.Select((DataItem dataItem) => new GemDataItem(dataItem)));
			ShapeSelectionVisibility = ((base.Gems.Count <= 1) ? Visibility.Collapsed : Visibility.Visible);
		}
	}

	private void PopupSubMenuMouseLeave(object sender, MouseEventArgs e)
	{
		popupSubMenu.IsOpen = false;
	}

	private void Selector_OnSelectionChanged(object sender, SelectionChangedEventArgs e)
	{
		if (!base.IsInitializing && !base.IsUpdatingControlSource && base.LocalUIControl != null && e.AddedItems.Count > 0 && e.AddedItems[0] is GemDataItem gemDataItem)
		{
			if (ViewModelLocator.Current.StoneTrayVM.IsStoneAssigned && !StullerDiamondAPIHelper.ConvertStullerStoneShapeToGemvisionCounterSketchStoneShapeString(ViewModelLocator.Current.StoneTrayVM.StoneChosen.Shape).Contains(gemDataItem.VectorResourceName.Trim().ToLowerInvariant().Replace(" ", "")))
			{
				ViewModelLocator.Current.StoneTrayVM.ClearAssignedStoneCommand.Execute((object)null);
			}
			base.LocalUIControl.Value1 = base.Gems.IndexOf(gemDataItem);
			SetGraphValue(isQuickSet: false);
		}
	}

	private void SldSizeMouseLeftButtonUp(object sender, MouseButtonEventArgs e)
	{
		_updateDelayTimer?.Stop();
		if (!base.IsUpdatingControlSource && !base.IsInitializing && base.LocalUIControl != null)
		{
			base.Dispatcher.BeginInvoke((Action)delegate
			{
				base.LocalUIControl.GemTableIndex = (int)((RangeBase)(object)sldSize).Value;
				_updateDelayTimer?.Stop();
				SetGraphValue(isQuickSet: false);
			}, DispatcherPriority.Background);
		}
	}

	private void SldSizeValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
	{
		if (base.IsInitializing || base.IsUpdatingControlSource || base.LocalUIControl == null)
		{
			return;
		}
		long num = 300L;
		if (_updateDelayTimer == null)
		{
			_updateDelayTimer = new Timer();
			_updateDelayTimer.AutoReset = false;
			_updateDelayTimer.Interval = num;
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
		base.LocalUIControl.GemTableIndex = (int)((RangeBase)(object)sldSize).Value;
		base.GemCarat = CaratToQuarterCaratDisplay(base.LocalUIControl.GemTableSizeInfoList.ElementAt(base.LocalUIControl.GemTableIndex).Carat);
		base.GemHeight = base.LocalUIControl.GemTableSizeInfoList.ElementAt(base.LocalUIControl.GemTableIndex).Height;
		base.GemWidth = base.LocalUIControl.GemTableSizeInfoList.ElementAt(base.LocalUIControl.GemTableIndex).Width;
		base.GemDepth = base.LocalUIControl.GemTableSizeInfoList.ElementAt(base.LocalUIControl.GemTableIndex).Depth;
		_updateDelayTimer.Start();
	}

	private void StoneTrayVM_StoneChosenEvent(object sender, EventArgs e)
	{
		if (ViewModelLocator.Current.StoneTrayVM.IsStoneAssigned)
		{
			UpdateControlFromAssignedCenterGem();
		}
		else
		{
			base.LocalUIControl.IsAssignedFromStone = false;
			SetGraphValue(isQuickSet: false);
		}
		Application.Current.Dispatcher.InvokeAsync(UpdateGemFields);
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

	private void UpdateControlFromAssignedCenterGem(bool overrideShape = false, string shape = null)
	{
		Diamond providedStone = ViewModelLocator.Current.StoneTrayVM.StoneChosen;
		DataItem dataItem = base.LocalUIControl.DataItems.FirstOrDefault((DataItem _) => StullerDiamondAPIHelper.ConvertStullerStoneShapeToGemvisionCounterSketchStoneShapeString(providedStone.Shape).Contains(_.VectorResourceName.Trim().ToLowerInvariant().Replace(" ", "")));
		if (dataItem != null)
		{
			int num = base.LocalUIControl.DataItems.IndexOf(dataItem);
			double num2 = (double)providedStone.Length;
			double num3 = (double)providedStone.Width;
			double.TryParse(providedStone.TotalDepth, out var result);
			base.LocalUIControl.Value1 = num;
			SetGraphValue(isQuickSet: false);
			base.LocalUIControl.GemHeight = num2;
			base.LocalUIControl.GemWidth = ((Math.Abs(num3) < 0.1) ? num2 : num3);
			base.LocalUIControl.GemDepth = result;
			base.LocalUIControl.IsCustomGemSize = true;
			base.LocalUIControl.IsAssignedFromStone = true;
			SetGraphValue(isQuickSet: false);
		}
		else
		{
			base.LocalUIControl.IsCustomGemSize = false;
			base.LocalUIControl.IsAssignedFromStone = false;
			SetGraphValue(isQuickSet: false);
			ViewModelLocator.Current.StoneTrayVM.AssignmentFailed = true;
		}
	}

	private void UpdateCustomGemSizeVisualState()
	{
		Application.Current.Dispatcher.Invoke(delegate
		{
			VisualStateManager.GoToState(this, (base.LocalUIControl.IsCustomGemSize || base.LocalUIControl.IsAssignedFromStone) ? "CustomGemSizeState" : "AutomaticGemSizeState", useTransitions: false);
		});
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
			if (ViewModelLocator.Current.StoneTrayVM.StoneChosen != null)
			{
				base.GemCarat = ViewModelLocator.Current.StoneTrayVM.StoneChosen.CaratWeight.ToString("#.##");
				IsManualStoneSizingEnabled = false;
				IsStoneAssignedEnabled = true;
			}
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
			MinCarat = RoundToQuaterIncrement(base.LocalUIControl.GemTableSizeInfoList.Min((GemTableSizeInfo _) => _.Carat));
			MaxCarat = RoundToQuaterIncrement(base.LocalUIControl.GemTableSizeInfoList.Max((GemTableSizeInfo _) => _.Carat));
		}
		if (base.LocalUIControl.DataItems != null && base.LocalUIControl.DataItems.Any())
		{
			DataItem dataItem = base.LocalUIControl.DataItems.ElementAt((int)base.LocalUIControl.Value1);
			if (dataItem != null)
			{
				Shape = dataItem.VectorResourceName;
				SelectedGemDataIndex = base.LocalUIControl.DataItems.IndexOf(dataItem);
			}
		}
		UpdateCustomGemSizeVisualState();
	}

	private void UpdateSelectedShapeFromShapeName(bool isPartAssignment = false)
	{
		string shape = (isPartAssignment ? ViewModelLocator.Current.TileSearchMainVM.TileSearchResultPartsVM.SelectedCenterShapeName : ViewModelLocator.Current.TileSearchMainVM.TileSearchResultDesignsVM.SelectedCenterShapeName);
		if (shape != null)
		{
			if (shape.Contains("Sq"))
			{
				shape = "CushionSquare";
			}
			if (shape == "Asscher")
			{
				shape = "EmeraldSquare";
			}
			DataItem dataItem = base.LocalUIControl.DataItems.FirstOrDefault((DataItem _) => _.VectorResourceName.Trim().ToLowerInvariant().Replace(" ", "") == shape.ToLower());
			if (dataItem != null)
			{
				int num = base.LocalUIControl.DataItems.IndexOf(dataItem);
				base.LocalUIControl.Value1 = num;
				SetGraphValue(isQuickSet: false);
				UpdateGemFields();
			}
		}
	}

	private void ButtonShopDiamond_Click(object sender, RoutedEventArgs e)
	{
		PunchoutHelper.ShowDiamondShopper();
	}

	private void Button_ShopGemstone(object sender, RoutedEventArgs e)
	{
		PunchoutHelper.ShowGemstoneShopper();
	}

	[DebuggerNonUserCode]
	[GeneratedCode("PresentationBuildTasks", "4.0.0.0")]
	public void InitializeComponent()
	{
		if (!_contentLoaded)
		{
			_contentLoaded = true;
			Uri resourceLocator = new Uri("/CounterSketch Studio;component/controls/csvaluegemcontrol.xaml", UriKind.Relative);
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
		//IL_00ff: Unknown result type (might be due to invalid IL or missing references)
		//IL_0109: Expected O, but got Unknown
		switch (connectionId)
		{
		case 1:
			CSValueGemUserControl = (CSValueGemControl)target;
			break;
		case 2:
			txtTitle = (TextBlock)target;
			break;
		case 3:
			borderSubMenu = (Border)target;
			break;
		case 4:
			btnSubMenu = (Button)target;
			btnSubMenu.Click += BtnSubMenuClick;
			break;
		case 5:
			txtCarat = (TextBlock)target;
			txtCarat.LostFocus += TxtPercentageLostFocus;
			break;
		case 6:
			txtSize = (TextBlock)target;
			break;
		case 7:
			sldSize = (RadSlider)target;
			((UIElement)(object)sldSize).PreviewMouseLeftButtonUp += SldSizeMouseLeftButtonUp;
			((RangeBase)(object)sldSize).ValueChanged += SldSizeValueChanged;
			break;
		case 8:
			tglCustomGem = (Button)target;
			tglCustomGem.Click += DisableCustomGemClick;
			break;
		case 9:
			gridDimensions = (Grid)target;
			break;
		case 10:
			path = (Path)target;
			break;
		case 11:
			textBlock = (TextBlock)target;
			break;
		case 12:
			txtHeight = (TextBox)target;
			txtHeight.LostFocus += TxtHeightLostFocus;
			break;
		case 13:
			path1 = (Path)target;
			break;
		case 14:
			textBlockWidth = (TextBlock)target;
			break;
		case 15:
			txtWidth = (TextBox)target;
			txtWidth.LostFocus += TxtWidthLostFocus;
			break;
		case 16:
			path2 = (Path)target;
			break;
		case 17:
			textBlock1 = (TextBlock)target;
			break;
		case 18:
			txtDepth = (TextBox)target;
			txtDepth.LostFocus += TxtDepthLostFocus;
			break;
		case 19:
			path3 = (Path)target;
			break;
		case 20:
			textBlock3 = (TextBlock)target;
			break;
		case 21:
			txtPercentage = (TextBox)target;
			txtPercentage.LostFocus += TxtPercentageLostFocus;
			break;
		case 22:
			((Button)target).Click += ButtonShopDiamond_Click;
			break;
		case 23:
			((Button)target).Click += Button_ShopGemstone;
			break;
		case 24:
			popupSubMenu = (Popup)target;
			popupSubMenu.MouseLeave += PopupSubMenuMouseLeave;
			break;
		case 25:
			stackSubMenuActions = (StackPanel)target;
			break;
		case 26:
			btnResetControl = (Button)target;
			btnResetControl.Click += BtnResetControlClick;
			break;
		case 27:
			CustomGemSizeStates = (VisualStateGroup)target;
			break;
		case 28:
			CustomGemSizeState = (VisualState)target;
			break;
		case 29:
			AutomaticGemSizeState = (VisualState)target;
			break;
		default:
			_contentLoaded = true;
			break;
		}
	}
}
