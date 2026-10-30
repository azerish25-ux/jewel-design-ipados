using System;
using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Input;
using System.Windows.Markup;
using GV.CounterSketch.Shared.Model;

namespace GV.CounterSketch.Studio.Controls;

public sealed class CSValueOnOffControl : CSUIControl, IComponentConnector
{
	internal CSValueOnOffControl CSValueToggleUserControl;

	internal Border borderSubMenu;

	internal Button btnSubMenu;

	internal CheckBox chkOnOff;

	internal TextBlock txtCategory;

	internal TextBlock txtTitle;

	internal Popup popupSubMenu;

	internal StackPanel stackSubMenuActions;

	internal Button btnResetControl;

	private bool _contentLoaded;

	public CSValueOnOffControl(UIControl uiControl)
	{
		InitializeComponent();
		base.LocalUIControl = uiControl;
		SetControl();
	}

	public override void SetControl()
	{
		base.IsInitializing = true;
		string title = (base.CategoryName = ((base.LocalUIControl.CategoryName != null && !base.LocalUIControl.CategoryName.ToLowerInvariant().Contains("design option")) ? (base.LocalUIControl.CategoryName + ":") : string.Empty));
		base.Title = title;
		if (string.IsNullOrWhiteSpace(base.CategoryName) && base.LocalUIControl.ControlName.Contains(":"))
		{
			base.Title = base.LocalUIControl.ControlName.Split(':').LastOrDefault();
			base.CategoryName = base.LocalUIControl.ControlName.Split(':').FirstOrDefault() + ":";
		}
		else
		{
			base.Title = base.LocalUIControl.ControlName;
		}
		base.SortIndex = base.LocalUIControl.SortIndex;
		base.OnOff = base.LocalUIControl.Value1 > 0.5;
		GetIconForUIControl();
		base.SetControl();
		base.IsInitializing = false;
	}

	public override void UpdateUIControlSource(UIControl uiControl)
	{
		base.IsUpdatingControlSource = true;
		base.UpdateUIControlSource(uiControl);
		base.IsUpdatingControlSource = false;
	}

	private void BtnResetControlClick(object sender, RoutedEventArgs e)
	{
		popupSubMenu.IsOpen = false;
		base.LocalUIControl.Value1 = 0.0;
		SetGraphValue(isQuickSet: false);
	}

	private void BtnSubMenuClick(object sender, RoutedEventArgs e)
	{
		popupSubMenu.IsOpen = true;
		popupSubMenu.Focus();
	}

	private void ChkOnOffChecked(object sender, RoutedEventArgs e)
	{
		if (!base.IsInitializing)
		{
			base.LocalUIControl.Value1 = 1.0;
			SetGraphValue(isQuickSet: false);
		}
	}

	private void ChkOnOffUnchecked(object sender, RoutedEventArgs e)
	{
		if (!base.IsInitializing && !base.IsUpdatingControlSource)
		{
			base.LocalUIControl.Value1 = 0.0;
			SetGraphValue(isQuickSet: false);
		}
	}

	private void PopupSubMenuMouseLeave(object sender, MouseEventArgs e)
	{
		popupSubMenu.IsOpen = false;
	}

	[DebuggerNonUserCode]
	[GeneratedCode("PresentationBuildTasks", "4.0.0.0")]
	public void InitializeComponent()
	{
		if (!_contentLoaded)
		{
			_contentLoaded = true;
			Uri resourceLocator = new Uri("/CounterSketch Studio;component/controls/csvalueonoffcontrol.xaml", UriKind.Relative);
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
			CSValueToggleUserControl = (CSValueOnOffControl)target;
			break;
		case 2:
			borderSubMenu = (Border)target;
			break;
		case 3:
			btnSubMenu = (Button)target;
			btnSubMenu.Click += BtnSubMenuClick;
			break;
		case 4:
			chkOnOff = (CheckBox)target;
			chkOnOff.Checked += ChkOnOffChecked;
			chkOnOff.Unchecked += ChkOnOffUnchecked;
			break;
		case 5:
			txtCategory = (TextBlock)target;
			break;
		case 6:
			txtTitle = (TextBlock)target;
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
