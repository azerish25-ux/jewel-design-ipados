using System.ComponentModel;
using System.Windows;
using System.Windows.Input;
using System.Windows.Interactivity;

namespace GV.CounterSketch.Studio.Utilities.Behaviors;

[Description("Used to execute a command when a 'DoubleClick' is detected on the uielement.")]
public class UIElementBehaviorMouseDoubleClickCommand : Behavior<UIElement>
{
	[Description("Command parameter to pass when executing the command.")]
	[Category("Command")]
	public static readonly DependencyProperty CommandParameterProperty = DependencyProperty.Register("CommandParameter", typeof(object), typeof(UIElementBehaviorMouseDoubleClickCommand));

	[Description("Command to execute when 'DoubleClick' is detected.")]
	[Category("Command")]
	public static readonly DependencyProperty DoubleClickCommandProperty = DependencyProperty.Register("DoubleClickCommand", typeof(ICommand), typeof(UIElementBehaviorMouseDoubleClickCommand));

	public object CommandParameter
	{
		get
		{
			return ((DependencyObject)(object)this).GetValue(CommandParameterProperty);
		}
		set
		{
			((DependencyObject)(object)this).SetValue(CommandParameterProperty, value);
		}
	}

	public ICommand DoubleClickCommand
	{
		get
		{
			return (ICommand)((DependencyObject)(object)this).GetValue(DoubleClickCommandProperty);
		}
		set
		{
			((DependencyObject)(object)this).SetValue(DoubleClickCommandProperty, (object)value);
		}
	}

	protected override void OnAttached()
	{
		((Behavior)this).OnAttached();
		base.AssociatedObject.PreviewMouseLeftButtonDown += AssociatedObject_MouseLeftButtonDown;
	}

	protected override void OnDetaching()
	{
		((Behavior)this).OnDetaching();
		base.AssociatedObject.PreviewMouseLeftButtonDown -= AssociatedObject_MouseLeftButtonDown;
	}

	private void AssociatedObject_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
	{
		if (e.ClickCount == 2 && DoubleClickCommand != null && DoubleClickCommand.CanExecute(null))
		{
			DoubleClickCommand.Execute(CommandParameter);
		}
	}
}
