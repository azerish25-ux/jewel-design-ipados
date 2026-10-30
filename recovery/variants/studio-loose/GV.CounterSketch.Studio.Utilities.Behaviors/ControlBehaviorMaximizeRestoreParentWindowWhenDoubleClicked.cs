using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Interactivity;

namespace GV.CounterSketch.Studio.Utilities.Behaviors;

[Description("Used to maximize/restore a Parent window when the control is double clicked.")]
public class ControlBehaviorMaximizeRestoreParentWindowWhenDoubleClicked : Behavior<Control>
{
	protected override void OnAttached()
	{
		((Behavior)this).OnAttached();
		base.AssociatedObject.MouseDoubleClick += AssociatedObject_MouseDoubleClick;
	}

	protected override void OnDetaching()
	{
		((Behavior)this).OnDetaching();
		base.AssociatedObject.MouseDoubleClick -= AssociatedObject_MouseDoubleClick;
	}

	private void AssociatedObject_MouseDoubleClick(object sender, MouseButtonEventArgs e)
	{
		Window window = Window.GetWindow(base.AssociatedObject);
		if (window != null)
		{
			if (window.WindowState == WindowState.Normal)
			{
				window.ResizeMode = ResizeMode.NoResize;
				window.WindowState = WindowState.Maximized;
			}
			else
			{
				window.ResizeMode = ResizeMode.CanResize;
				window.WindowState = WindowState.Normal;
			}
		}
	}
}
