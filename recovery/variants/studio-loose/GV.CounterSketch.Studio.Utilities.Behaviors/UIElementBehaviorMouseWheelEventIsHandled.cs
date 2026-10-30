using System.ComponentModel;
using System.Windows;
using System.Windows.Input;
using System.Windows.Interactivity;

namespace GV.CounterSketch.Studio.Utilities.Behaviors;

[Description("Used to mark the MouseWheel event as handled on the uielement before it bubbles up the tree.")]
public class UIElementBehaviorMouseWheelEventIsHandled : Behavior<UIElement>
{
	protected override void OnAttached()
	{
		((Behavior)this).OnAttached();
		base.AssociatedObject.MouseWheel += AssociatedObject_MouseWheel;
	}

	protected override void OnDetaching()
	{
		((Behavior)this).OnDetaching();
		base.AssociatedObject.MouseWheel -= AssociatedObject_MouseWheel;
	}

	private static void AssociatedObject_MouseWheel(object sender, MouseWheelEventArgs e)
	{
		e.Handled = true;
	}
}
