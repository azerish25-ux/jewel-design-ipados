using System.ComponentModel;
using System.Windows;
using System.Windows.Input;
using System.Windows.Interactivity;

namespace GV.CounterSketch.Studio.Utilities.Behaviors;

[Description("Used to capture a mousewheel tunneling event on the uielement and create a bubbling event on the parent element.")]
public class UIElementBehaviorSuppressMouseClickEvents : Behavior<UIElement>
{
	protected override void OnAttached()
	{
		((Behavior)this).OnAttached();
		base.AssociatedObject.PreviewMouseDown += AssociatedObject_PreviewMouseDown;
		base.AssociatedObject.PreviewMouseUp += AssociatedObject_PreviewMouseUp;
		base.AssociatedObject.MouseDown += AssociatedObject_MouseDown;
		base.AssociatedObject.MouseUp += AssociatedObject_MouseUp;
		base.AssociatedObject.PreviewMouseLeftButtonDown += AssociatedObject_PreviewMouseLeftButtonDown;
		base.AssociatedObject.PreviewMouseLeftButtonUp += AssociatedObject_PreviewMouseLeftButtonUp;
		base.AssociatedObject.MouseLeftButtonUp += AssociatedObject_MouseLeftButtonUp;
		base.AssociatedObject.MouseLeftButtonDown += AssociatedObject_MouseLeftButtonDown;
	}

	protected override void OnDetaching()
	{
		((Behavior)this).OnDetaching();
		base.AssociatedObject.PreviewMouseDown -= AssociatedObject_PreviewMouseDown;
		base.AssociatedObject.PreviewMouseUp -= AssociatedObject_PreviewMouseUp;
		base.AssociatedObject.MouseDown -= AssociatedObject_MouseDown;
		base.AssociatedObject.MouseUp -= AssociatedObject_MouseUp;
		base.AssociatedObject.PreviewMouseLeftButtonDown -= AssociatedObject_PreviewMouseLeftButtonDown;
		base.AssociatedObject.PreviewMouseLeftButtonUp -= AssociatedObject_PreviewMouseLeftButtonUp;
		base.AssociatedObject.MouseLeftButtonUp -= AssociatedObject_MouseLeftButtonUp;
		base.AssociatedObject.MouseLeftButtonDown -= AssociatedObject_MouseLeftButtonDown;
	}

	private void AssociatedObject_MouseDown(object sender, MouseButtonEventArgs e)
	{
		e.Handled = true;
	}

	private void AssociatedObject_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
	{
		e.Handled = true;
	}

	private void AssociatedObject_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
	{
		e.Handled = true;
	}

	private void AssociatedObject_MouseUp(object sender, MouseButtonEventArgs e)
	{
		e.Handled = true;
	}

	private void AssociatedObject_PreviewMouseDown(object sender, MouseButtonEventArgs e)
	{
		e.Handled = true;
	}

	private void AssociatedObject_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
	{
		e.Handled = true;
	}

	private void AssociatedObject_PreviewMouseLeftButtonUp(object sender, MouseButtonEventArgs e)
	{
		e.Handled = true;
	}

	private void AssociatedObject_PreviewMouseUp(object sender, MouseButtonEventArgs e)
	{
		e.Handled = true;
	}
}
