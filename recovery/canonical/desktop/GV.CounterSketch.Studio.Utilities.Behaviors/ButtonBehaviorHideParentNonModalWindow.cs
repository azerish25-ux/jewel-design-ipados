using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Interactivity;

namespace GV.CounterSketch.Studio.Utilities.Behaviors;

[Description("Used to close a non-modal parent window when clicking the button.")]
public class ButtonBehaviorHideParentNonModalWindow : Behavior<Button>
{
	protected override void OnAttached()
	{
		base.OnAttached();
		base.AssociatedObject.Click += AssociatedObject_Click;
	}

	protected override void OnDetaching()
	{
		base.OnDetaching();
		base.AssociatedObject.Click -= AssociatedObject_Click;
	}

	private void AssociatedObject_Click(object sender, RoutedEventArgs e)
	{
		Window.GetWindow(base.AssociatedObject)?.Hide();
	}
}
