using System;
using System.CodeDom.Compiler;
using System.Diagnostics;
using System.Windows;
using System.Windows.Markup;
using GV.CounterSketch.Shared.Model;

namespace GV.CounterSketch.Studio.Controls;

public sealed partial class CSValueOnOffGraphicControl : CSUIControl, IComponentConnector
{
	public CSValueOnOffGraphicControl(UIControl uiControl)
	{
		InitializeComponent();
		base.LocalUIControl = uiControl;
		SetControl();
	}

	public override void SetControl()
	{
		base.IsInitializing = true;
		base.Title = base.LocalUIControl.ControlName;
		base.CategoryName = base.LocalUIControl.CategoryName + ":";
		base.SortIndex = base.LocalUIControl.SortIndex;
		base.OnOff = base.LocalUIControl.Value1 > 0.5;
		switch (base.LocalUIControl.ControlName)
		{
		case "Mirror Front and Back":
			Top.Visibility = Visibility.Visible;
			break;
		case "Mirror Opposite Side":
			Side.Visibility = Visibility.Visible;
			break;
		case "Mirror Quad":
			Quad.Visibility = Visibility.Visible;
			break;
		case "Part On Off":
			Duplicate.Visibility = Visibility.Visible;
			break;
		}
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

	[DebuggerNonUserCode]
	[GeneratedCode("PresentationBuildTasks", "4.0.0.0")]
	internal Delegate _CreateDelegate(Type delegateType, string handler)
	{
		return Delegate.CreateDelegate(delegateType, this, handler);
	}
}
