using System;
using System.CodeDom.Compiler;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Input;
using System.Windows.Markup;
using System.Windows.Media;
using GV.CounterSketch.Shared.Model;
using GV.CounterSketch.Studio.Model.Objects;
using GV.CounterSketch.Studio.Utilities.Helpers;
using GV.CounterSketch.Studio.ViewModel;
using Stuller.Web.API.v2.Models;

namespace GV.CounterSketch.Studio.Controls;

public sealed class CSValueGemShapeControl : CSUIControl, IComponentConnector
{
	public static readonly DependencyProperty SelectedGemShapeDataIndexProperty = DependencyProperty.Register("SelectedGemShapeDataIndex", typeof(int), typeof(CSValueGemShapeControl));

	public static readonly DependencyProperty ShapeProperty = DependencyProperty.Register("Shape", typeof(string), typeof(CSValueGemShapeControl));

	public static readonly DependencyProperty ShapeSelectionVisibilityProperty = DependencyProperty.Register("ShapeSelectionVisibility", typeof(Visibility), typeof(CSValueGemShapeControl), new PropertyMetadata(Visibility.Visible));

	private bool _isStoneTrayEventWired;

	internal CSValueGemShapeControl CSValueGemShapeUserControl;

	internal Border borderSubMenu;

	internal Button btnSubMenu;

	internal TextBlock txtTitle;

	internal Popup popupSubMenu;

	internal StackPanel stackSubMenuActions;

	internal Button btnResetControl;

	private bool _contentLoaded;

	public int SelectedGemShapeDataIndex
	{
		get
		{
			return (int)GetValue(SelectedGemShapeDataIndexProperty);
		}
		set
		{
			SetValue(SelectedGemShapeDataIndexProperty, value);
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

	public CSValueGemShapeControl(UIControl uiControl)
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
		SelectedGemShapeDataIndex = (int)base.LocalUIControl.Value1;
		UpdateGemFields();
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

	private void PopulateGems()
	{
		if (base.LocalUIControl.DataItems != null && base.LocalUIControl.DataItems.Any())
		{
			base.Gems = new ObservableCollection<GemDataItem>(base.LocalUIControl.DataItems.Select((DataItem dataItem) => new GemDataItem(dataItem)));
			base.Visibility = ((base.Gems.Count <= 1) ? Visibility.Collapsed : Visibility.Visible);
		}
	}

	private void PopupSubMenuMouseLeave(object sender, MouseEventArgs e)
	{
		popupSubMenu.IsOpen = false;
	}

	private void Selector_OnSelectionChanged(object sender, SelectionChangedEventArgs e)
	{
		if (!base.IsInitializing && !base.IsUpdatingControlSource && base.LocalUIControl != null && e.AddedItems.Count > 0 && e.AddedItems[0] is GemDataItem item)
		{
			base.LocalUIControl.Value1 = base.Gems.IndexOf(item);
			SetGraphValue(isQuickSet: false);
		}
	}

	private void StoneTrayVM_StoneChosenEvent(object sender, EventArgs e)
	{
		if (ViewModelLocator.Current.StoneTrayVM.IsStoneAssigned)
		{
			UpdateControlFromAssignedCenterGem();
		}
	}

	private void UpdateControlFromAssignedCenterGem()
	{
		Diamond providedStone = ViewModelLocator.Current.StoneTrayVM.StoneChosen;
		DataItem dataItem = base.LocalUIControl.DataItems.FirstOrDefault((DataItem _) => StullerDiamondAPIHelper.ConvertStullerStoneShapeToGemvisionCounterSketchStoneShapeString(providedStone.Shape).Contains(_.VectorResourceName.Trim().ToLowerInvariant().Replace(" ", "")));
		if (dataItem != null)
		{
			int num = base.LocalUIControl.DataItems.IndexOf(dataItem);
			base.LocalUIControl.Value1 = num;
			SetGraphValue(isQuickSet: false);
		}
	}

	private void UpdateGemFields()
	{
		base.GemHeight = base.LocalUIControl.GemHeight;
		base.GemWidth = base.LocalUIControl.GemWidth;
		base.GemDepth = base.LocalUIControl.GemDepth;
		base.GemPercentage = base.LocalUIControl.GemPercentage;
		base.GemCarat = CaratToQuarterCaratDisplay(base.LocalUIControl.GemCarat);
		if (base.LocalUIControl.DataItems != null && base.LocalUIControl.DataItems.Any())
		{
			DataItem dataItem = base.LocalUIControl.DataItems.ElementAt((int)base.LocalUIControl.Value1);
			if (dataItem != null)
			{
				Shape = dataItem.VectorResourceName;
			}
		}
	}

	[DebuggerNonUserCode]
	[GeneratedCode("PresentationBuildTasks", "4.0.0.0")]
	public void InitializeComponent()
	{
		if (!_contentLoaded)
		{
			_contentLoaded = true;
			Uri resourceLocator = new Uri("/CounterSketch Studio;component/controls/csvaluegemshapecontrol.xaml", UriKind.Relative);
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
			CSValueGemShapeUserControl = (CSValueGemShapeControl)target;
			break;
		case 2:
			borderSubMenu = (Border)target;
			break;
		case 3:
			btnSubMenu = (Button)target;
			btnSubMenu.Click += BtnSubMenuClick;
			break;
		case 4:
			txtTitle = (TextBlock)target;
			break;
		case 5:
			popupSubMenu = (Popup)target;
			popupSubMenu.MouseLeave += PopupSubMenuMouseLeave;
			break;
		case 6:
			stackSubMenuActions = (StackPanel)target;
			break;
		case 7:
			btnResetControl = (Button)target;
			btnResetControl.Click += BtnResetControlClick;
			break;
		default:
			_contentLoaded = true;
			break;
		}
	}
}
