using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Interactivity;

namespace GV.CounterSketch.Studio.Utilities.Behaviors;

[Description("Used to minimize a Parent window when the button is clicked.")]
public class ButtonBehaviorMinimizeParentWindow : Behavior<Button>
{
	protected override void OnAttached()
	{
		((Behavior)this).OnAttached();
		base.AssociatedObject.Click += AssociatedObject_Click;
	}

	protected override void OnDetaching()
	{
		((Behavior)this).OnDetaching();
		base.AssociatedObject.Click -= AssociatedObject_Click;
	}

	private void AssociatedObject_Click(object sender, RoutedEventArgs e)
	{
		Window window = Window.GetWindow(base.AssociatedObject);
		if (window != null)
		{
			window.WindowState = WindowState.Minimized;
		}
	}
}
