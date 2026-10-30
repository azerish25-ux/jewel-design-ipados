using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Interactivity;
using System.Windows.Interop;

namespace GV.CounterSketch.Studio.Utilities.Behaviors;

[Description("Used to close a non-modal parent window when clicking the button.")]
public class ButtonBehaviorCloseParentNonModalWindow : Behavior<Button>
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
		Window window = Window.GetWindow(base.AssociatedObject);
		if (window != null && !ComponentDispatcher.IsThreadModal)
		{
			window.Close();
		}
	}
}
