using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Interactivity;
using GV.CounterSketch.Shared;
using GV.CounterSketch.Studio.Utilities.Helpers;

namespace GV.CounterSketch.Studio.Utilities.Behaviors;

[Description("Used to force focus to the window when mouse enters.")]
public class WindowBehaviorRequestFocusWhenMouseEnters : Behavior<Window>
{
	protected override void OnAttached()
	{
		((Behavior)this).OnAttached();
		base.AssociatedObject.MouseEnter += AssociatedObject_MouseEnter;
	}

	protected override void OnDetaching()
	{
		((Behavior)this).OnDetaching();
		base.AssociatedObject.MouseEnter -= AssociatedObject_MouseEnter;
	}

	private void AssociatedObject_MouseEnter(object sender, MouseEventArgs e)
	{
		if (!(Keyboard.FocusedElement is TextBox) && !UIHelper.FocusRequestDisabled)
		{
			Win32Utility.AllowSetForegroungWindowSetProcessId(-1);
			base.AssociatedObject.Activate();
		}
	}
}
