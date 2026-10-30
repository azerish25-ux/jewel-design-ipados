using System.Windows;
using System.Windows.Controls;
using System.Windows.Interactivity;

namespace GV.CounterSketch.Studio.Utilities.Behaviors;

public class ListBoxBehaviorScrollSelectedItemIntoViewWhenSizeChanges : Behavior<ListBox>
{
	protected override void OnAttached()
	{
		((Behavior)this).OnAttached();
		base.AssociatedObject.SizeChanged += AssociatedObject_SizeChanged;
	}

	protected override void OnDetaching()
	{
		((Behavior)this).OnDetaching();
		base.AssociatedObject.SizeChanged -= AssociatedObject_SizeChanged;
	}

	private void AssociatedObject_SizeChanged(object sender, SizeChangedEventArgs e)
	{
		base.AssociatedObject.ScrollIntoView(base.AssociatedObject.SelectedItem);
	}
}
