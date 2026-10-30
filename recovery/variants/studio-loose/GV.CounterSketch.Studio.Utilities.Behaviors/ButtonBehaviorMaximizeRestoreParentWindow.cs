using System;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Interactivity;

namespace GV.CounterSketch.Studio.Utilities.Behaviors;

[Description("Used to maximize/restore a Parent window when the button is clicked.")]
public class ButtonBehaviorMaximizeRestoreParentWindow : Behavior<Button>
{
	private static Window _window;

	protected override void OnAttached()
	{
		((Behavior)this).OnAttached();
		base.AssociatedObject.Click += AssociatedObject_Click;
		_window = Window.GetWindow(base.AssociatedObject);
		if (_window != null)
		{
			_window.StateChanged += WindowStateChanged;
		}
	}

	protected override void OnDetaching()
	{
		((Behavior)this).OnDetaching();
		base.AssociatedObject.Click -= AssociatedObject_Click;
	}

	private void AssociatedObject_Click(object sender, RoutedEventArgs e)
	{
		if (_window != null)
		{
			if (_window.WindowState == WindowState.Normal)
			{
				_window.ResizeMode = ResizeMode.NoResize;
				_window.WindowState = WindowState.Maximized;
			}
			else
			{
				_window.ResizeMode = ResizeMode.CanResize;
				_window.WindowState = WindowState.Normal;
			}
		}
	}

	private void WindowStateChanged(object sender, EventArgs e)
	{
		if (_window != null)
		{
			if (_window.WindowState == WindowState.Normal)
			{
				VisualStateManager.GoToState(base.AssociatedObject, "Maximize", useTransitions: false);
			}
			else
			{
				VisualStateManager.GoToState(base.AssociatedObject, "Restore", useTransitions: false);
			}
		}
	}
}
