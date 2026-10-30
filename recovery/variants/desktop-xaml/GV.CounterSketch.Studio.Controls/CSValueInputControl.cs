using System.Windows;
using System.Windows.Input;
using System.Windows.Markup;
using System.Windows.Media;
using GV.CounterSketch.Shared.Model;

namespace GV.CounterSketch.Studio.Controls;

public sealed partial class CSValueInputControl : CSUIControl, IComponentConnector
{
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
}
