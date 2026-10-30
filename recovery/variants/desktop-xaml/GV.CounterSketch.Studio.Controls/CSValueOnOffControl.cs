using System;
using System.CodeDom.Compiler;
using System.Diagnostics;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using System.Windows.Markup;
using GV.CounterSketch.Shared.Model;

namespace GV.CounterSketch.Studio.Controls;

public sealed partial class CSValueOnOffControl : CSUIControl, IComponentConnector
{
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
	internal Delegate _CreateDelegate(Type delegateType, string handler)
	{
		return Delegate.CreateDelegate(delegateType, this, handler);
	}
}
