using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Interactivity;

namespace GV.CounterSketch.Studio.Utilities.Behaviors;

[Description("Used to remove focus from the textbox during the Enter key up event.  Focus returns to this element afterward.")]
public class TextBoxBehaviorLoseFocusOnEnterKeyUp : Behavior<TextBox>
{
	public static readonly DependencyProperty MyPropertyProperty = DependencyProperty.Register("FocusTargetElement", typeof(UIElement), typeof(TextBoxBehaviorLoseFocusOnEnterKeyUp));

	private static Key _keyLastPressedDown;

	private static UIElement _lastElementKeyPressIn;

	public UIElement FocusTargetElement
	{
		get
		{
			return (UIElement)((DependencyObject)(object)this).GetValue(MyPropertyProperty);
		}
		set
		{
			((DependencyObject)(object)this).SetValue(MyPropertyProperty, (object)value);
		}
	}

	protected override void OnAttached()
	{
		((Behavior)this).OnAttached();
		base.AssociatedObject.PreviewKeyDown += AssociatedObject_PreviewKeyDown;
		base.AssociatedObject.PreviewKeyUp += AssociatedObjectPreviewKeyUp;
	}

	protected override void OnDetaching()
	{
		((Behavior)this).OnDetaching();
		base.AssociatedObject.PreviewKeyUp -= AssociatedObjectPreviewKeyUp;
	}

	private void AssociatedObject_PreviewKeyDown(object sender, KeyEventArgs e)
	{
		_lastElementKeyPressIn = base.AssociatedObject;
		_keyLastPressedDown = e.Key;
	}

	private void AssociatedObjectPreviewKeyUp(object sender, KeyEventArgs e)
	{
		if (e.Key == Key.Return || e.Key == Key.Return || e.Key == Key.Tab)
		{
			if (FocusTargetElement != null && FocusTargetElement.Focusable)
			{
				FocusTargetElement.Focus();
			}
			else if (e.Key == _keyLastPressedDown && _lastElementKeyPressIn == base.AssociatedObject)
			{
				base.AssociatedObject.MoveFocus(new TraversalRequest(FocusNavigationDirection.Next));
			}
			e.Handled = true;
		}
	}
}
