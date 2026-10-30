using System.ComponentModel;
using System.Windows;
using System.Windows.Automation.Peers;
using System.Windows.Automation.Provider;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Interactivity;

namespace GV.CounterSketch.Studio.Utilities.Behaviors;

[Description("Used to simulate a click event on target button when the Enter KeyUp event occurs in the textbox.")]
public class TextBoxBehaviorClickTargetButtonOnEnterKeyUp : Behavior<TextBox>
{
	public static readonly DependencyProperty TargetButtonProperty = DependencyProperty.Register("TargetButton", typeof(Button), typeof(TextBoxBehaviorClickTargetButtonOnEnterKeyUp));

	[Description("Button that is clicked when 'Enter' is KeyedUp in the textbox.")]
	[Category("Target")]
	[DisplayName("Button")]
	public Button TargetButton
	{
		get
		{
			return (Button)GetValue(TargetButtonProperty);
		}
		set
		{
			SetValue(TargetButtonProperty, value);
		}
	}

	protected override void OnAttached()
	{
		base.OnAttached();
		base.AssociatedObject.KeyUp += AssociatedObject_KeyUp;
	}

	protected override void OnDetaching()
	{
		base.OnDetaching();
		base.AssociatedObject.KeyUp -= AssociatedObject_KeyUp;
	}

	private void AssociatedObject_KeyUp(object sender, KeyEventArgs e)
	{
		if ((e.Key == Key.Return || e.Key == Key.Return) && TargetButton != null && TargetButton.IsEnabled && new ButtonAutomationPeer(TargetButton).GetPattern(PatternInterface.Invoke) is IInvokeProvider invokeProvider)
		{
			invokeProvider.Invoke();
		}
	}
}
