using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Markup;
using System.Windows.Media;
using GV.CounterSketch.Shared.Model;
using GV.CounterSketch.Studio.Model.Objects;
using GV.CounterSketch.Studio.Utilities.Helpers;

namespace GV.CounterSketch.Studio.Controls;

public sealed partial class CSValueFontListControl : CSUIControl, IComponentConnector
{
	public static readonly DependencyProperty SelectedDataItemIndexProperty = DependencyProperty.Register("SelectedDataItemIndex", typeof(int), typeof(CSValueFontListControl));

	private ResourceDictionary _listItemImageDictionary;

	public int SelectedDataItemIndex
	{
		get
		{
			return (int)GetValue(SelectedDataItemIndexProperty);
		}
		set
		{
			SetValue(SelectedDataItemIndexProperty, value);
		}
	}

	public CSValueFontListControl(UIControl uiControl)
	{
		InitializeComponent();
		base.LocalUIControl = uiControl;
		base.DefaultControlValue = uiControl.ListIndex;
		SetControl();
	}

	public override void SetControl()
	{
		base.IsInitializing = true;
		base.Title = base.LocalUIControl.ControlName;
		base.ListItemStrings = new List<string>(base.LocalUIControl.DataItems.Select((DataItem _) => _.Identifier).ToList());
		SelectedDataItemIndex = base.LocalUIControl.ListIndex;
		base.DefaultControlValue = SelectedDataItemIndex;
		CboItems.SelectedIndex = base.LocalUIControl.ListIndex;
		base.SortIndex = base.LocalUIControl.SortIndex;
		base.DefaultControlValue = base.LocalUIControl.ListIndex;
		if (!string.IsNullOrEmpty(base.PathToIconImage))
		{
			DrawingBrush drawingBrush = (DrawingBrush)RectImage.TryFindResource(base.PathToIconImage);
			if (drawingBrush != null)
			{
				RectImage.Fill = drawingBrush;
				RectImage.Width = 50.0;
			}
			else
			{
				RectImage.Width = 0.0;
			}
		}
		base.SetControl();
		base.IsInitializing = false;
	}

	public override void UpdateUIControlSource(UIControl uiControl)
	{
		base.IsUpdatingControlSource = true;
		base.UpdateUIControlSource(uiControl);
		base.Title = uiControl.ControlName;
		if (uiControl.ListItems != null)
		{
			base.ListItems = (string.IsNullOrWhiteSpace(base.LocalUIControl.ValueListImageModifier) ? new ObservableCollection<ValueListDisplayItem>(ListItemImageHelper.GetValueListItemsFromListIndexPairs(base.LocalUIControl.ListItems, ref _listItemImageDictionary)) : new ObservableCollection<ValueListDisplayItem>(ListItemImageHelper.GetValueListItemsFromListIndexPairs(base.LocalUIControl.ListItems, base.LocalUIControl.ValueListImageModifier, ref _listItemImageDictionary)));
		}
		if (uiControl.DataItems != null)
		{
			base.ListItemStrings = new List<string>(uiControl.DataItems.Select((DataItem _) => _.Identifier).ToList());
		}
		SelectedDataItemIndex = base.LocalUIControl.ListIndex;
		base.DefaultControlValue = SelectedDataItemIndex;
		CboItems.SelectedIndex = uiControl.ListIndex;
		base.SortIndex = uiControl.SortIndex;
		base.IsUpdatingControlSource = false;
	}

	private void BtnResetControlClick(object sender, RoutedEventArgs e)
	{
		PopupSubMenu.IsOpen = false;
		if (!double.IsNaN(base.DefaultControlValue))
		{
			SelectedDataItemIndex = (int)base.DefaultControlValue;
			base.LocalUIControl.ListIndex = SelectedDataItemIndex;
			CboItems.SelectedIndex = (int)base.DefaultControlValue;
			if (base.LocalUIControl != null && !base.IsInitializing)
			{
				base.LocalUIControl.ListIndex = (int)base.DefaultControlValue;
				SetGraphValue(isQuickSet: false);
			}
		}
	}

	private void BtnSubMenuClick(object sender, RoutedEventArgs e)
	{
		PopupSubMenu.IsOpen = true;
		PopupSubMenu.Focus();
	}

	private void CboItems_SelectionChanged(object sender, SelectionChangedEventArgs e)
	{
		if (!base.IsInitializing && !base.IsUpdatingControlSource)
		{
			base.LocalUIControl.ListIndex = CboItems.SelectedIndex;
			SetGraphValue(isQuickSet: false);
		}
	}

	private void PopupSubMenuMouseLeave(object sender, MouseEventArgs e)
	{
		PopupSubMenu.IsOpen = false;
	}

	[DebuggerNonUserCode]
	[GeneratedCode("PresentationBuildTasks", "4.0.0.0")]
	internal Delegate _CreateDelegate(Type delegateType, string handler)
	{
		return Delegate.CreateDelegate(delegateType, this, handler);
	}
}
