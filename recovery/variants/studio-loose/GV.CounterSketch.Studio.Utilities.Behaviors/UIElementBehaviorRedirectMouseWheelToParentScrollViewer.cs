using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Interactivity;
using GV.CounterSketch.Studio.Utilities.Helpers;

namespace GV.CounterSketch.Studio.Utilities.Behaviors;

[Description("Used to capture a mousewheel tunneling event on the uielement and create a bubbling event on the parent scrollviewer element.")]
public class UIElementBehaviorRedirectMouseWheelToParentScrollViewer : Behavior<UIElement>
{
	protected override void OnAttached()
	{
		((Behavior)this).OnAttached();
		base.AssociatedObject.PreviewMouseWheel += AssociatedObject_PreviewMouseWheel;
	}

	protected override void OnDetaching()
	{
		((Behavior)this).OnDetaching();
		base.AssociatedObject.PreviewMouseWheel -= AssociatedObject_PreviewMouseWheel;
	}

	private void AssociatedObject_PreviewMouseWheel(object sender, MouseWheelEventArgs e)
	{
		e.Handled = true;
		MouseWheelEventArgs e2 = new MouseWheelEventArgs(e.MouseDevice, e.Timestamp, e.Delta)
		{
			RoutedEvent = UIElement.MouseWheelEvent,
			Source = sender
		};
		WpfHelper.FindAncestorOrSelf<ScrollViewer>((FrameworkElement)sender)?.RaiseEvent(e2);
	}
}
