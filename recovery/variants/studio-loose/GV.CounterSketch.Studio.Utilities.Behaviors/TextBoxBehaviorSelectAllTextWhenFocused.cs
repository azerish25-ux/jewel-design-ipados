using System.ComponentModel;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Interactivity;

namespace GV.CounterSketch.Studio.Utilities.Behaviors;

[Description("Used to select all text in the textbox when it receives focus.")]
public class TextBoxBehaviorSelectAllTextWhenFocused : Behavior<TextBox>
{
	protected override void OnAttached()
	{
		((Behavior)this).OnAttached();
		base.AssociatedObject.PreviewMouseLeftButtonDown += AssociatedObject_PreviewMouseLeftButtonDown;
		base.AssociatedObject.GotKeyboardFocus += AssociatedObject_GotKeyboardFocus;
		base.AssociatedObject.MouseDoubleClick += AssociatedObject_MouseDoubleClick;
	}

	protected override void OnDetaching()
	{
		((Behavior)this).OnDetaching();
		base.AssociatedObject.PreviewMouseLeftButtonDown -= AssociatedObject_PreviewMouseLeftButtonDown;
		base.AssociatedObject.GotKeyboardFocus -= AssociatedObject_GotKeyboardFocus;
		base.AssociatedObject.MouseDoubleClick -= AssociatedObject_MouseDoubleClick;
	}

	private void AssociatedObject_GotKeyboardFocus(object sender, KeyboardFocusChangedEventArgs e)
	{
		base.AssociatedObject.SelectAll();
		e.Handled = true;
	}

	private void AssociatedObject_MouseDoubleClick(object sender, MouseButtonEventArgs e)
	{
		base.AssociatedObject.SelectAll();
		e.Handled = true;
	}

	private void AssociatedObject_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
	{
		if (!base.AssociatedObject.IsKeyboardFocusWithin)
		{
			base.AssociatedObject.Focus();
			e.Handled = true;
		}
	}
}
