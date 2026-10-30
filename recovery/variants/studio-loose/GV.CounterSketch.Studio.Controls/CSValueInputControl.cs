using System;
using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Markup;
using System.Windows.Media;
using System.Windows.Shapes;
using GV.CounterSketch.Shared.Model;

namespace GV.CounterSketch.Studio.Controls;

public sealed class CSValueInputControl : CSUIControl, IComponentConnector
{
	internal CSValueInputControl TheValueInputControl;

	internal Grid GridDisplayImage;

	internal Rectangle RectImage;

	internal Border BorderSubMenu;

	internal TextBlock TxtTitle;

	internal TextBox TxtValue;

	private bool _contentLoaded;

	public CSValueInputControl(UIControl uiControl)
	{
		InitializeComponent();
		base.LocalUIControl = uiControl;
		base.DefaultControlValueGeneric = uiControl.ValueGeneric;
		SetControl();
	}

	public override void SetControl()
	{
		base.IsInitializing = true;
		base.Title = base.LocalUIControl.ControlName;
		base.ValueGeneric = base.LocalUIControl.ValueGeneric;
		base.SortIndex = base.LocalUIControl.SortIndex;
		if (!string.IsNullOrEmpty(base.PathToIconImage))
		{
			DrawingBrush drawingBrush = (DrawingBrush)RectImage.TryFindResource(base.PathToIconImage);
			if (drawingBrush != null)
			{
				RectImage.Fill = drawingBrush;
			}
		}
		base.SetControl();
		base.IsInitializing = false;
	}

	public override void UpdateUIControlSource(UIControl uiControl)
	{
		base.UpdateUIControlSource(uiControl);
		base.ValueGeneric = uiControl.ValueGeneric;
	}

	private void TxtValue_OnKeyUp(object sender, KeyEventArgs e)
	{
		if (e.Key == Key.Return)
		{
			TraversalRequest request = new TraversalRequest(FocusNavigationDirection.Next);
			(Keyboard.FocusedElement as UIElement)?.MoveFocus(request);
		}
	}

	private void TxtValue_OnLostFocus(object sender, RoutedEventArgs e)
	{
		if (!base.IsInitializing && base.LocalUIControl != null)
		{
			base.LocalUIControl.ValueGeneric = TxtValue.Text;
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
			Uri resourceLocator = new Uri("/CounterSketch Studio;component/controls/csvalueinputcontrol.xaml", UriKind.Relative);
			Application.LoadComponent(this, resourceLocator);
		}
	}

	[DebuggerNonUserCode]
	[GeneratedCode("PresentationBuildTasks", "4.0.0.0")]
	[EditorBrowsable(EditorBrowsableState.Never)]
	void IComponentConnector.Connect(int connectionId, object target)
	{
		switch (connectionId)
		{
		case 1:
			TheValueInputControl = (CSValueInputControl)target;
			break;
		case 2:
			GridDisplayImage = (Grid)target;
			break;
		case 3:
			RectImage = (Rectangle)target;
			break;
		case 4:
			BorderSubMenu = (Border)target;
			break;
		case 5:
			TxtTitle = (TextBlock)target;
			break;
		case 6:
			TxtValue = (TextBox)target;
			TxtValue.KeyUp += TxtValue_OnKeyUp;
			TxtValue.LostFocus += TxtValue_OnLostFocus;
			break;
		default:
			_contentLoaded = true;
			break;
		}
	}
}
