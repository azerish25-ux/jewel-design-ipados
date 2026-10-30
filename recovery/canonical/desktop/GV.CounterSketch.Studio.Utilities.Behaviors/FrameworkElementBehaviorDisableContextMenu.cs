using System.ComponentModel;
using System.Windows;
using System.Windows.Input;
using System.Windows.Interactivity;

namespace GV.CounterSketch.Studio.Utilities.Behaviors;

[Description("Used to smother a RightClick event on the UIElement.")]
public class FrameworkElementBehaviorDisableContextMenu : Behavior<FrameworkElement>
{
	public static readonly DependencyProperty IsEnabledProperty = DependencyProperty.Register("IsEnabled", typeof(bool), typeof(FrameworkElementBehaviorDisableContextMenu));

	[Description("Used to enable/disable the attached behavior.")]
	[Category("Common Properties")]
	public bool IsEnabled
	{
		get
		{
			return (bool)GetValue(IsEnabledProperty);
		}
		set
		{
			SetValue(IsEnabledProperty, value);
		}
	}

	protected override void OnAttached()
	{
		base.OnAttached();
		base.AssociatedObject.PreviewMouseRightButtonDown += AssociatedObject_PreviewMouseRightButtonDown;
	}

	protected override void OnDetaching()
	{
		base.OnDetaching();
		base.AssociatedObject.PreviewMouseRightButtonDown -= AssociatedObject_PreviewMouseRightButtonDown;
	}

	private void AssociatedObject_PreviewMouseRightButtonDown(object sender, MouseButtonEventArgs e)
	{
		if (!IsEnabled)
		{
			base.AssociatedObject.ContextMenu.Visibility = Visibility.Collapsed;
		}
		else
		{
			base.AssociatedObject.ContextMenu.Visibility = Visibility.Visible;
		}
	}
}
