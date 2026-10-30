using System.Windows;
using System.Windows.Controls;
using System.Windows.Interactivity;

namespace GV.CounterSketch.Studio.Utilities.Behaviors;

public class ListBoxBehaviorScrollSelectedItemIntoViewWhenVisible : Behavior<ListBox>
{
	protected override void OnAttached()
	{
		base.OnAttached();
		base.AssociatedObject.IsVisibleChanged += AssociatedObject_IsVisibleChanged;
	}

	protected override void OnDetaching()
	{
		base.OnDetaching();
		base.AssociatedObject.IsVisibleChanged -= AssociatedObject_IsVisibleChanged;
	}

	private void AssociatedObject_IsVisibleChanged(object sender, DependencyPropertyChangedEventArgs e)
	{
		if ((bool)e.NewValue)
		{
			base.AssociatedObject.ScrollIntoView(base.AssociatedObject.SelectedItem);
		}
	}
}
