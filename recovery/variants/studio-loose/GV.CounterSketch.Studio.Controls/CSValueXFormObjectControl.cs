using System;
using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Markup;
using System.Windows.Threading;
using GV.CounterSketch.Shared.Model;
using GalaSoft.MvvmLight;

namespace GV.CounterSketch.Studio.Controls;

public class CSValueXFormObjectControl : CSUIControl, IComponentConnector
{
	public static readonly DependencyProperty OnOffFlipSideProperty = DependencyProperty.Register("OnOffFlipSide", typeof(bool), typeof(CSValueXFormObjectControl));

	public static readonly DependencyProperty OnOffOppositeProperty = DependencyProperty.Register("OnOffOpposite", typeof(bool), typeof(CSValueXFormObjectControl));

	public static readonly DependencyProperty OnOffQuadProperty = DependencyProperty.Register("OnOffQuad", typeof(bool), typeof(CSValueXFormObjectControl));

	public static readonly DependencyProperty OnOffSideProperty = DependencyProperty.Register("OnOffSide", typeof(bool), typeof(CSValueXFormObjectControl));

	public static readonly DependencyProperty OnOffTopProperty = DependencyProperty.Register("OnOffTop", typeof(bool), typeof(CSValueXFormObjectControl));

	internal CSValueXFormObjectControl CSValueXFormObjectUserControl;

	internal ListBoxItem flipSideItem;

	internal CheckBox flipSide;

	internal Viewbox Flip;

	internal ListBoxItem mirrorTopItem;

	internal CheckBox mirrorTop;

	internal Viewbox Top;

	internal ListBoxItem mirrorSideItem;

	internal CheckBox mirrorSide;

	internal Viewbox Side1;

	internal ListBoxItem mirrorOppositeItem;

	internal CheckBox mirrorOpposite;

	internal Viewbox Opposite2;

	internal ListBoxItem mirrorQuadItem;

	internal CheckBox mirrorQuad;

	internal Viewbox Quad3;

	private bool _contentLoaded;

	public bool OnOffFlipSide
	{
		get
		{
			return (bool)GetValue(OnOffFlipSideProperty);
		}
		set
		{
			SetValue(OnOffFlipSideProperty, value);
		}
	}

	public bool OnOffOpposite
	{
		get
		{
			return (bool)GetValue(OnOffOppositeProperty);
		}
		set
		{
			SetValue(OnOffOppositeProperty, value);
		}
	}

	public bool OnOffQuad
	{
		get
		{
			return (bool)GetValue(OnOffQuadProperty);
		}
		set
		{
			SetValue(OnOffQuadProperty, value);
		}
	}

	public bool OnOffSide
	{
		get
		{
			return (bool)GetValue(OnOffSideProperty);
		}
		set
		{
			SetValue(OnOffSideProperty, value);
		}
	}

	public bool OnOffTop
	{
		get
		{
			return (bool)GetValue(OnOffTopProperty);
		}
		set
		{
			SetValue(OnOffTopProperty, value);
		}
	}

	public CSValueXFormObjectControl(UIControl uiControl)
	{
		InitializeComponent();
		base.LocalUIControl = uiControl;
		SetControl();
	}

	public CSValueXFormObjectControl()
	{
		if (ViewModelBase.IsInDesignModeStatic)
		{
			base.LocalUIControl = new UIControl
			{
				ControlName = "Object Part Control Title",
				EnglishControlName = "Object Part Control Title",
				CategoryName = "Object Part Category",
				EnglishCategoryName = "Object Part Category",
				SortIndex = 1
			};
		}
	}

	public void FlipSelector_OnSelectionChanged(object sender, SelectionChangedEventArgs e)
	{
		if (base.IsInitializing)
		{
			return;
		}
		int value = 0;
		if (e.AddedItems.Count > 0 && e.AddedItems[0] is ListBoxItem { Name: { } name } && name == "flipSideItem")
		{
			value = 1;
		}
		if (base.LocalUIControl != null && !base.IsInitializing && !base.IsUpdatingControlSource)
		{
			base.Dispatcher.BeginInvoke((Action)delegate
			{
				base.LocalUIControl.Value1 = value;
				SetGraphValue(isQuickSet: false);
			}, DispatcherPriority.Background);
		}
	}

	public void MirrorSelector_OnSelectionChanged(object sender, SelectionChangedEventArgs e)
	{
		if (base.IsInitializing)
		{
			return;
		}
		int value = 0;
		if (e.AddedItems.Count > 0 && e.AddedItems[0] is ListBoxItem listBoxItem)
		{
			switch (listBoxItem.Name)
			{
			case "mirrorSideItem":
				value = 1;
				break;
			case "mirrorOppositeItem":
				value = 2;
				break;
			case "mirrorTopItem":
				value = 3;
				break;
			case "mirrorQuadItem":
				value = 4;
				break;
			}
		}
		if (base.LocalUIControl != null && !base.IsInitializing && !base.IsUpdatingControlSource)
		{
			base.Dispatcher.BeginInvoke((Action)delegate
			{
				base.LocalUIControl.Value2 = value;
				SetGraphValue(isQuickSet: false);
			}, DispatcherPriority.Background);
		}
	}

	public sealed override void SetControl()
	{
		base.IsInitializing = true;
		base.Title = base.LocalUIControl.ControlName;
		base.CategoryName = $"{base.LocalUIControl.CategoryName}:";
		base.SortIndex = base.LocalUIControl.SortIndex;
		int num = (int)base.LocalUIControl.Value1;
		if (num != 0 && num == 1)
		{
			OnOffFlipSide = true;
			flipSideItem.IsSelected = true;
		}
		switch ((int)base.LocalUIControl.Value2)
		{
		case 1:
			OnOffSide = true;
			mirrorSideItem.IsSelected = true;
			break;
		case 2:
			OnOffOpposite = true;
			mirrorOppositeItem.IsSelected = true;
			break;
		case 3:
			OnOffTop = true;
			mirrorTopItem.IsSelected = true;
			break;
		case 4:
			OnOffQuad = true;
			mirrorQuadItem.IsSelected = true;
			break;
		}
		base.SetControl();
		base.IsInitializing = false;
	}

	public override void UpdateUIControlSource(UIControl uiControl)
	{
		base.IsUpdatingControlSource = true;
		base.UpdateUIControlSource(uiControl);
		base.IsUpdatingControlSource = false;
	}

	[DebuggerNonUserCode]
	[GeneratedCode("PresentationBuildTasks", "4.0.0.0")]
	public void InitializeComponent()
	{
		if (!_contentLoaded)
		{
			_contentLoaded = true;
			Uri resourceLocator = new Uri("/CounterSketch Studio;component/controls/csvaluexformobjectcontrol.xaml", UriKind.Relative);
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
			CSValueXFormObjectUserControl = (CSValueXFormObjectControl)target;
			break;
		case 2:
			flipSideItem = (ListBoxItem)target;
			break;
		case 3:
			flipSide = (CheckBox)target;
			break;
		case 4:
			Flip = (Viewbox)target;
			break;
		case 5:
			mirrorTopItem = (ListBoxItem)target;
			break;
		case 6:
			mirrorTop = (CheckBox)target;
			break;
		case 7:
			Top = (Viewbox)target;
			break;
		case 8:
			mirrorSideItem = (ListBoxItem)target;
			break;
		case 9:
			mirrorSide = (CheckBox)target;
			break;
		case 10:
			Side1 = (Viewbox)target;
			break;
		case 11:
			mirrorOppositeItem = (ListBoxItem)target;
			break;
		case 12:
			mirrorOpposite = (CheckBox)target;
			break;
		case 13:
			Opposite2 = (Viewbox)target;
			break;
		case 14:
			mirrorQuadItem = (ListBoxItem)target;
			break;
		case 15:
			mirrorQuad = (CheckBox)target;
			break;
		case 16:
			Quad3 = (Viewbox)target;
			break;
		default:
			_contentLoaded = true;
			break;
		}
	}
}
