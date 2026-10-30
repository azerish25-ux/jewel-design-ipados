using System;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Interactivity;
using System.Windows.Threading;

namespace GV.CounterSketch.Studio.Utilities.Behaviors;

public class ListBoxBehaviorScrollSelectedItemIntoViewOnMouseLeave : Behavior<ListBox>
{
	protected override void OnAttached()
	{
		base.OnAttached();
		base.AssociatedObject.MouseLeave += AssociatedObject_MouseLeave;
	}

	protected override void OnDetaching()
	{
		base.OnDetaching();
		base.AssociatedObject.MouseLeave -= AssociatedObject_MouseLeave;
	}

	private void AssociatedObject_MouseLeave(object sender, MouseEventArgs e)
	{
		DispatcherTimer dispatcherTimer = new DispatcherTimer();
		dispatcherTimer.Interval = new TimeSpan(0, 0, 0, 0, 600);
		dispatcherTimer.Tick += timer_Tick;
		dispatcherTimer.Start();
	}

	private void timer_Tick(object sender, EventArgs e)
	{
		if (sender is DispatcherTimer dispatcherTimer)
		{
			dispatcherTimer.Stop();
		}
		base.AssociatedObject.ScrollIntoView(base.AssociatedObject.SelectedItem);
	}
}
