using System.ComponentModel;
using System.Windows;
using System.Windows.Input;
using System.Windows.Interactivity;

namespace GV.CounterSketch.Studio.Utilities.Behaviors;

[Description("Used to force the click event to stay alive on the Button Click.")]
public class ButtonBehaviorBubbleClickEvent : Behavior<FrameworkElement>
{
	protected override void OnAttached()
	{
		((Behavior)this).OnAttached();
		base.AssociatedObject.PreviewMouseUp += AssociatedObject_PreviewMouseUp;
		base.AssociatedObject.MouseUp += AssociatedObject_MouseUp;
	}

	protected override void OnDetaching()
	{
		((Behavior)this).OnDetaching();
		base.AssociatedObject.PreviewMouseUp -= AssociatedObject_PreviewMouseUp;
		base.AssociatedObject.MouseUp -= AssociatedObject_MouseUp;
	}

	private void AssociatedObject_MouseUp(object sender, MouseButtonEventArgs e)
	{
		e.Handled = false;
	}

	private void AssociatedObject_PreviewMouseUp(object sender, MouseButtonEventArgs e)
	{
		e.Handled = false;
	}
}
