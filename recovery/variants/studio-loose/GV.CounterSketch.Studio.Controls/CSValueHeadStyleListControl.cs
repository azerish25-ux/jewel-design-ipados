using System;
using System.CodeDom.Compiler;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Input;
using System.Windows.Markup;
using System.Windows.Media;
using System.Windows.Shapes;
using GV.CounterSketch.Shared.Model;
using GV.CounterSketch.Studio.Model.Objects;
using GV.CounterSketch.Studio.UserControls.PagedListBox;
using GV.CounterSketch.Studio.Utilities.Helpers;

namespace GV.CounterSketch.Studio.Controls;

public sealed class CSValueHeadStyleListControl : CSUIControl, IComponentConnector
{
	public static readonly DependencyProperty SelectedDataItemIndexProperty = DependencyProperty.Register("SelectedDataItemIndex", typeof(int), typeof(CSValueHeadStyleListControl));

	private ResourceDictionary _listItemImageDictionary;

	internal CSValueHeadStyleListControl TheValueHeadStyleListControl;

	internal Grid GridDisplayImage;

	internal Rectangle RectImage;

	internal Popup PopupSubMenu;

	internal StackPanel StackSubMenuActions;

	internal Button BtnResetControl;

	internal TextBlock TxtTitle;

	internal Border BorderSubMenu;

	internal Button BtnSubMenu;

	internal PagedListBox pagedListBox;

	private bool _contentLoaded;

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

	public CSValueHeadStyleListControl(UIControl uiControl)
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
		base.HeadStyleListItems = new ObservableCollection<ValueListDisplayItem>(HeadStyleListItemImageHelper.GetValueHeadStyleListItemsFromListIndexPairs(base.LocalUIControl.HeadStyleListItems, ref _listItemImageDictionary));
		SelectedDataItemIndex = base.LocalUIControl.ListIndex;
		base.DefaultControlValue = SelectedDataItemIndex;
		((UIElement)(object)pagedListBox).Visibility = Visibility.Visible;
		pagedListBox.ScrollSelectedItemIntoView();
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
		base.HeadStyleListItems = new ObservableCollection<ValueListDisplayItem>(HeadStyleListItemImageHelper.GetValueHeadStyleListItemsFromListIndexPairs(base.LocalUIControl.HeadStyleListItems, ref _listItemImageDictionary));
		SelectedDataItemIndex = base.LocalUIControl.ListIndex;
		base.DefaultControlValue = SelectedDataItemIndex;
		((UIElement)(object)pagedListBox).Visibility = Visibility.Visible;
		pagedListBox.ScrollSelectedItemIntoView();
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

	private void PopupSubMenuMouseLeave(object sender, MouseEventArgs e)
	{
		PopupSubMenu.IsOpen = false;
	}

	private void Selector_OnSelectionChanged(object sender, SelectionChangedEventArgs e)
	{
		if (!base.IsInitializing && !base.IsUpdatingControlSource && base.LocalUIControl != null)
		{
			base.LocalUIControl.ListIndex = SelectedDataItemIndex;
			SetGraphValue(isQuickSet: false);
		}
	}

	[DebuggerNonUserCode]
	[GeneratedCode("PresentationBuildTasks", "4.0.0.0")]
	public void InitializeComponent()
	{
		if (!_contentLoaded)
		{
			_contentLoaded = true;
			Uri resourceLocator = new Uri("/CounterSketch Studio;component/controls/csvalueheadstylelistcontrol.xaml", UriKind.Relative);
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
			TheValueHeadStyleListControl = (CSValueHeadStyleListControl)target;
			break;
		case 2:
			GridDisplayImage = (Grid)target;
			break;
		case 3:
			RectImage = (Rectangle)target;
			break;
		case 4:
			PopupSubMenu = (Popup)target;
			PopupSubMenu.MouseLeave += PopupSubMenuMouseLeave;
			break;
		case 5:
			StackSubMenuActions = (StackPanel)target;
			break;
		case 6:
			BtnResetControl = (Button)target;
			BtnResetControl.Click += BtnResetControlClick;
			break;
		case 7:
			TxtTitle = (TextBlock)target;
			break;
		case 8:
			BorderSubMenu = (Border)target;
			break;
		case 9:
			BtnSubMenu = (Button)target;
			BtnSubMenu.Click += BtnSubMenuClick;
			break;
		case 10:
			pagedListBox = (PagedListBox)target;
			break;
		default:
			_contentLoaded = true;
			break;
		}
	}
}
