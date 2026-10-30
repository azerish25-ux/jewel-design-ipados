using System.ComponentModel;
using System.Windows;
using System.Windows.Input;
using System.Windows.Interactivity;

namespace GV.CounterSketch.Studio.Utilities.Behaviors;

[Description("Used to drag a Parent window when mousedown event on the uielement.")]
public class UIElementBehaviorDragParentWindow : Behavior<UIElement>
{
	protected override void OnAttached()
	{
		base.OnAttached();
		base.AssociatedObject.MouseLeftButtonDown += AssociatedObject_MouseLeftButtonDown;
	}

	protected override void OnDetaching()
	{
		base.OnDetaching();
		base.AssociatedObject.MouseLeftButtonDown -= AssociatedObject_MouseLeftButtonDown;
	}

	private void AssociatedObject_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
	{
		if (Mouse.LeftButton == MouseButtonState.Pressed)
		{
			Window.GetWindow(base.AssociatedObject).DragMove();
		}
	}
}
