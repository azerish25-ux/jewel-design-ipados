using System;
using System.CodeDom.Compiler;
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
using GV.CounterSketch.Studio.ViewModel;
using GalaSoft.MvvmLight.Messaging;

namespace GV.CounterSketch.Studio.Controls;

public sealed partial class CSValueChooserControl : CSUIControl, IComponentConnector
{
	public static readonly DependencyProperty SelectedDataItemIndexProperty = DependencyProperty.Register("SelectedDataItemIndex", typeof(int), typeof(CSValueChooserControl));

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

	public CSValueChooserControl(UIControl uiControl)
	{
		InitializeComponent();
		base.LocalUIControl = uiControl;
		base.ProfilePolyLineColor = Colors.Gray;
		SetControl();
	}

	public override void SetControl()
	{
		base.IsInitializing = true;
		PopulateProfiles();
		base.Title = base.LocalUIControl.ControlName;
		base.SortIndex = base.LocalUIControl.SortIndex;
		SelectedDataItemIndex = (int)base.LocalUIControl.Value1;
		base.DefaultControlValue = SelectedDataItemIndex;
		GetIconForUIControl();
		base.SetControl();
		if (UIHelper.CounterSketchOptions.HideAdvancedStackOptions ?? UIHelper.KeyProfile.HideAdvancedStackOptions)
		{
			btnAdvancedProfileEditCopy.Visibility = Visibility.Collapsed;
		}
		base.IsInitializing = false;
	}

	public override void UpdateUIControlSource(UIControl uiControl)
	{
		base.IsUpdatingControlSource = true;
		base.UpdateUIControlSource(uiControl);
		PopulateProfiles();
		SelectedDataItemIndex = (int)base.LocalUIControl.Value1;
		base.DefaultControlValue = SelectedDataItemIndex;
		pagedListBox.ScrollSelectedItemIntoView();
		base.IsUpdatingControlSource = false;
	}

	private void BtnAdvancedProfileEditCopyClick(object sender, RoutedEventArgs e)
	{
		popupSubMenu.IsOpen = false;
		ButtonCurveEditorCopyClicked(this, new EventArgs());
	}

	private void BtnResetControlClick(object sender, RoutedEventArgs e)
	{
		popupSubMenu.IsOpen = false;
		SelectedDataItemIndex = (int)base.DefaultControlValue;
		base.LocalUIControl.Value1 = SelectedDataItemIndex;
		SetGraphValue(isQuickSet: false);
	}

	private void BtnSubMenuClick(object sender, RoutedEventArgs e)
	{
		popupSubMenu.IsOpen = true;
		popupSubMenu.Focus();
	}

	private void ButtonCurveEditorCopyClicked(object sender, EventArgs e)
	{
		Messenger.Default.Send<NotificationMessage<UIControl>, DesignStackViewModel>(new NotificationMessage<UIControl>(base.LocalUIControl, "AccessCurveEditorCopy"));
	}

	private void PopulateProfiles()
	{
		base.Profiles = new ObservableCollection<ValueChooserDataItem>(base.LocalUIControl.DataItems.Select((DataItem dataItem) => new ValueChooserDataItem(dataItem)));
	}

	private void PopupSubMenuMouseLeave(object sender, MouseEventArgs e)
	{
		popupSubMenu.IsOpen = false;
	}

	private void Selector_OnSelectionChanged(object sender, SelectionChangedEventArgs e)
	{
		if (!base.IsInitializing && !base.IsUpdatingControlSource && base.LocalUIControl != null && e.AddedItems.Count > 0 && e.AddedItems[0] is ValueChooserDataItem item)
		{
			base.LocalUIControl.Value1 = base.Profiles.IndexOf(item);
			SetGraphValue(isQuickSet: false);
		}
	}

	[DebuggerNonUserCode]
	[GeneratedCode("PresentationBuildTasks", "4.0.0.0")]
	internal Delegate _CreateDelegate(Type delegateType, string handler)
	{
		return Delegate.CreateDelegate(delegateType, this, handler);
	}
}
