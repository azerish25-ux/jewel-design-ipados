using System.ComponentModel;
using System.Windows;
using System.Windows.Input;
using System.Windows.Interactivity;

namespace GV.CounterSketch.Studio.Utilities.Behaviors;

[Description("Used to give focus to another element when the FrameworkElement is focused.")]
public class FrameworkElementBehaviorRedirectFocusToTarget : TargetedTriggerAction<FrameworkElement>
{
	public static readonly DependencyProperty RefocusElementProperty = DependencyProperty.Register("RefocusElement", typeof(FrameworkElement), typeof(FrameworkElementBehaviorRedirectFocusToTarget), new UIPropertyMetadata(null));

	[Category("TargetElement")]
	public FrameworkElement RefocusElement
	{
		get
		{
			return (FrameworkElement)((DependencyObject)(object)this).GetValue(RefocusElementProperty);
		}
		set
		{
			((DependencyObject)(object)this).SetValue(RefocusElementProperty, (object)value);
		}
	}

	protected override void Invoke(object parameter)
	{
		if (RefocusElement != null)
		{
			if (!RefocusElement.IsEnabled)
			{
				RefocusElement.IsEnabled = true;
			}
			FocusManager.SetFocusedElement((DependencyObject)(object)this, RefocusElement);
			Keyboard.Focus(RefocusElement);
		}
	}
}
